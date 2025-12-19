'Added by Vikrant On 11-Jul-2019 For ALL11072019	
Public Class wfUpdateFirstPriorityStatusofItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public PartNo As String = ""
    Public Description As String = ""
    Dim EventLogID As Guid
    Dim mAlternatePartNumbers As AlternatePartNumbers
    Dim mItem As Item
    Dim PartNoSelected As String = ""
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mAlternatePartNumbers = Session("mAlternatePartNumbers")
        mItem = Session("mItem")
        PartNoSelected = Session("PartNoSelected")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mItem")
        Session.Remove("PartNoSelected")
    End Sub
    Private Sub SetValues()
        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Function SetItemID() As Guid
        Dim mID As Guid = Guid.Empty
        Dim chkBox As CheckBox
        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            chkBox = CType(dgItemsList.Rows(i).FindControl("chkSelect"), CheckBox)
            If chkBox.Checked Then
                mID = New Guid(dgItemsList.DataKeys(i).Value.ToString)
                PartNoSelected = dgItemsList.Rows(i).Cells(3).Text
                Session("PartNoSelected") = PartNoSelected
            End If
        Next
        Return mID
    End Function
    Private Sub SetGrid()
        Dim chkBox As CheckBox
        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            chkBox = CType(dgItemsList.Rows(i).FindControl("chkSelect"), CheckBox)
            chkBox.Checked = IIf(mAlternatePartNumbers(i).IsFirstPriorityPart, True, False)
        Next
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAlternatePartNumbers = AlternatePartNumbers.NewAlternatePartNumbers
        dgItemsList.DataSource = mAlternatePartNumbers
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        custValidator.ControlToValidate = "txtsearch"
        If txtSearch.Text = "" Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
            If PartNo = "" Or Description = "" Then
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If IsValid Then
            SetValues()
            Dim ItemID As Guid = SetItemID()
            If Not ItemID.Equals(Guid.Empty) Then
                mItem.UpdateFirstPriorityPartStatus(ItemID)
                MarkLog(Action.Save, "ChangeFirstPriorityPartStatus", PartNoSelected + " is marked as First Priority Item amongst its alternates", ErrorType.NoError, ItemID, EventLogID, "")
                MSGBoxCtrl.show("Success!", "First Priority Part Status changed successfully", "", MsgBoxStyle.OkOnly, "Success")
            End If

        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        If IsValid Then
            SetValues()
            mItem = Item.GetItemByName(PartNo)
            Session("mItem") = mItem
            If Not mItem Is Nothing Then
                mAlternatePartNumbers = AlternatePartNumbers.GetItemWithAlternateItemsList(mItem.ID)
                dgItemsList.DataSource = mAlternatePartNumbers
                dgItemsList.DataBind()
                SetGrid()
                upnlItemListDetails.Update()
            Else

            End If
        End If
    End Sub
#End Region


End Class