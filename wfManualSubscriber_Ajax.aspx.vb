'Added By Vikrant On 19-Mar-2014 For ALL19032014

Public Class wfManualSubscriber_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mManual As Manual
    Public mEmployeeList As EmployeeList
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mManual = Session("mManual")
        mEmployeeList = Session("mEmployeeList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeList")
    End Sub
    Private Sub SaveFormtoObject()
        mManual.ManualSubscribers.CurrentItem.EmployeeID = New Guid(cmbEmployeeList.SelectedValue)
        mManual.ManualSubscribers.CurrentItem.EmployeeName = Trim(txtEmployeeName.Text)
        mManual.ManualSubscribers.CurrentItem.Email = Trim(txtEmail.Text)
    End Sub
    Private Sub DataFieldBind(Optional ByVal GetList As Boolean = True)
        If GetList = True Then
            mEmployeeList = EmployeeList.GetEmployeeList(AddTopItem:="(SELECT)")
            'mEmployeeList = EmployeeList.GetEmployeeList()
            Session("mEmployeeList") = mEmployeeList
        End If
        cmbEmployeeList.DataSource = mEmployeeList
        DataBind()
    End Sub
    Private Sub ControlVisibility()
        If cmbEmployeeList.SelectedIndex > 0 Then
            txtEmployeeName.ReadOnly = True
            txtEmployeeName.BackColor = Color.Silver
        Else
            txtEmployeeName.ReadOnly = False
            txtEmployeeName.BackColor = Color.White
        End If
    End Sub
    Private Sub SetValues()
        cmbEmployeeList.SelectedValue = mManual.ManualSubscribers.CurrentItem.EmployeeID.ToString
        txtEmployeeName.Text = mManual.ManualSubscribers.CurrentItem.EmployeeName
        txtEmail.Text = mManual.ManualSubscribers.CurrentItem.Email
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""

        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            setFocus(cmbEmployeeList)
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Page.Validate("1")
        If IsValid Then
            SaveFormtoObject()
            If mManual.ManualSubscribers.Contains(mManual.ManualSubscribers.CurrentItem) Then
                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry in Subscriber.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Try
                If mManual.ManualSubscribers.CurrentItem.IsDirty Then
                    If mManual.ManualSubscribers.CurrentItem.IsSavable Then
                        mManual.ApplyEdit()
                        Session("mManual") = mManual
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                    Else
                        cvControlValidator.ErrorMessage = mManual.ManualSubscribers.CurrentItem.GetBrokenRulesString
                        cvControlValidator.IsValid = mManual.ManualSubscribers.CurrentItem.IsValid
                        upnlValidationSummary.Update()
                    End If
                Else
                    mManual.ApplyEdit()
                    Session("mManual") = mManual
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
            End Try
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Session("EditSubscriber") = False Then Session.Remove("EditSubscriber") : mManual.ManualSubscribers.Remove(mManual.ManualSubscribers.CurrentItem)
        Session("EditSubscriber") = ""
        mManual.CancelEdit()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbEmployeeList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbEmployeeList.SelectedIndexChanged
        txtEmployeeName.Text = IIf(cmbEmployeeList.SelectedIndex > 0, mEmployeeList(cmbEmployeeList.SelectedIndex).Name, "")
        txtEmail.Text = IIf(cmbEmployeeList.SelectedIndex > 0, mEmployeeList(cmbEmployeeList.SelectedIndex).Email, "")
        ControlVisibility()
    End Sub
#End Region



End Class