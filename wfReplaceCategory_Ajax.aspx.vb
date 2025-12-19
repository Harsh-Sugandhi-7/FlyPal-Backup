Public Class wfReplaceCategory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCategoryList As CategoryList
    Public CatIndex As Integer = 0
    Public RepCatIndex As Integer = 0
    Public Flag As Boolean = False
    Dim MsgText As String
    Dim Detail As String
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCategoryList = Session("mCategoryList")
        MsgText = Session("MsgText")
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryList")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfReplaceCategory_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub UpdateCategoryIDOfItems()
        Try
            Dim mItems As Items = Flypal.Items.GetItems(0, "", "", "", cmbCategory.SelectedItem.Text)
            Dim mItem As Item = Item.GetItem(mItems(0).ID)
            Flag = Session("Flag")
            mItem.UpdateCategoryID(New Guid(cmbCategory.SelectedValue), New Guid(cmbReplaceWithCategory.SelectedValue), Flag)
            Session.Remove("Flag")
            Detail = "Original Category : " & cmbCategory.SelectedItem.Text & " Replaced Category : " & cmbReplaceWithCategory.SelectedItem.Text & IIf(Flag = True, (" Deleted Category : " & cmbCategory.SelectedItem.Text), "")
            MarkLog(Util.Action.Save, "ReplaceCategory", Detail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue2")
                        Exit Sub
                    End If
                    If MSGBoxCtrl.Sender = "Continue2" Then
                        UpdateCategoryIDOfItems()
                        DataFieldBind()
                        upnlDetails.Update()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Continue2" Then
                        DataFieldBind()
                    End If
            End Select
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " DataFieldBind "
    Public Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList(True)
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList

        cmbReplaceWithCategory.DataSource = mCategoryList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbCategory.Enabled = True Then
                setFocus(cmbCategory)
            End If
            Session("MiddleFrame") = "wfReplaceCategory_Ajax.aspx?"
            DataFieldBind()
        End If
    End Sub
    Private Sub btnReplaceNDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceNDelete.Click, btnReplace.Click
        If IsValid Then
            If CType(sender, Button).ID = "btnReplace" Then
                MsgText = "You Are Going To Replace Category " & cmbCategory.SelectedItem.Text & " with " & cmbReplaceWithCategory.SelectedItem.Text & "." & "<BR> <BR> Do you want to continue? "
                Flag = False
            ElseIf CType(sender, Button).ID = "btnReplaceNDelete" Then
                MsgText = "You Are Going To Replace & Delete Category " & cmbCategory.SelectedItem.Text & " with " & cmbReplaceWithCategory.SelectedItem.Text & "." & "<BR> <BR> Do you want to continue? "
                Flag = True
            End If
            Session("MsgText") = MsgText
            Session("Flag") = Flag
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue1")
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class