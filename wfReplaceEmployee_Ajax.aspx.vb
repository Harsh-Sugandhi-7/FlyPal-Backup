Public Class wfReplaceEmployee_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mReplaceEmployee As EmployeeList
    Public CatIndex As Integer = 0
    Public RepCatIndex As Integer = 0
    Public Flag As Boolean = False
    Dim MsgText As String
    Dim Detail As String
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReplaceEmployee = Session("mReplaceEmployee")
        MsgText = Session("MsgText")
    End Sub
    Private Sub SetSession()
        Session("mReplaceEmployee") = mReplaceEmployee
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReplaceEmployee")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfReplaceEmployee_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub EmployeeIDUpdate()

        Flag = Session("Flag")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim cn = New SqlConnection(conString)
        Dim cm As New SqlCommand

        cn.Open()
        Try
            With cm
                .Connection = cn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "UpdateEmployeeID"
                .Parameters.AddWithValue("@EmployeeID", New Guid(cmbEmployee.SelectedValue))
                .Parameters.AddWithValue("@ReplaceWithEmployeeID", New Guid(cmbReplaceWithEmployee.SelectedValue))
                .Parameters.AddWithValue("@IsReplaceAndDelete", Flag)

                Dim dr As New SafeDataReader(.ExecuteReader)
            End With
            'Catch ex As Exception
            '    Throw ex.GetBaseException
            'End Try

        Catch ex As SqlException
            If ex.Number = 2627 Then
                If ex.Message.Contains("UKWOJobDesignationAllocationIDResourceID") Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, , MsgBoxStyle.OkOnly, "")
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "As Resource:- " + cmbReplaceWithEmployee.SelectedItem.Text + " Already Allocated In WO. Job So Resource Can Not Be Allocated Or Replace In Same Allocation.", MsgBoxStyle.OkOnly, "Duplicate")
                    Exit Sub
                End If
            End If
            Throw ex
        End Try
        cn.Close()
        Session.Remove("Flag")
        Detail = "Original Employee : " & cmbEmployee.SelectedItem.Text & " Replaced Employee : " & cmbReplaceWithEmployee.SelectedItem.Text & IIf(Flag = True, (" Deleted Employee : " & cmbEmployee.SelectedItem.Text), "")
        MarkLog(Util.Action.Save, "ReplaceEmployee", Detail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        'MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue2")
                        'Exit Sub
                        EmployeeIDUpdate()
                        DataFieldBind()
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "Continue2" Then
                        'EmployeeIDUpdate()
                        'DataFieldBind()
                        'upnlDetails.Update()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Continue2" Then
                        DataFieldBind()
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Duplicate" Then

                    End If
            End Select
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region " DataFieldBind "
    Public Sub DataFieldBind()
        mReplaceEmployee = EmployeeList.GetEmployeeList(AddTopItem:="(SELECT)")
        cmbEmployee.DataSource = mReplaceEmployee
        Session("mReplaceEmployee") = mReplaceEmployee

        cmbReplaceWithEmployee.DataSource = mReplaceEmployee
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfReplaceEmployee_Ajax.aspx?"
            DataFieldBind()
        End If
    End Sub
    Private Sub btnReplaceNDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceNDelete.Click, btnReplace.Click
        If IsValid Then
            If CType(sender, Button).ID = "btnReplace" Then
                MsgText = "You Are Going To Replace Employee " & cmbEmployee.SelectedItem.Text & " with " & cmbReplaceWithEmployee.SelectedItem.Text & "." & "<BR> <BR> Do you want to continue? "
                Flag = False
            ElseIf CType(sender, Button).ID = "btnReplaceNDelete" Then
                MsgText = "You Are Going To Replace & Delete Employee " & cmbEmployee.SelectedItem.Text & " with " & cmbReplaceWithEmployee.SelectedItem.Text & "." & "<BR> <BR> Do you want to continue? "
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