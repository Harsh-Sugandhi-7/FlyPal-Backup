'********************************************
'Modified by Harsh Sugandhi on 27th May 2025 for FLYPAL-2439 Not Working Employee change in W.O & Log Module.
'********************************************


Partial Class wfPilot
    Inherits Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As Object

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "

    Public mEmployee As Employee
    Public mEmployeeList As EmployeeList
    Public mDesignationList As DesignationList

    Dim EventLogID As Guid

#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mEmployee = CType(Session("mEmployee"), Employee)
        mEmployeeList = CType(Session("mEmployeeList"), EmployeeList)
        mDesignationList = Session("mDesignationList")
    End Sub

    Private Sub SetSession()
        Session("mEmployee") = mEmployee
        Session("mEmployeeList") = mEmployeeList
        Session("mDesignationList") = mDesignationList
    End Sub

    Private Sub NewRecord()
        mEmployee = Employee.NewPilot()
        Session("mEmployee") = mEmployee
    End Sub

    Private Sub EditRecord(Id As Guid)
        mEmployee = Employee.GetEmployee(Id)
        Session("mEmployee") = mEmployee
        SetFocus(txtPilotName)
    End Sub

    Private Sub DeleteRecord(Id As Guid)

        Try

            mEmployee = Employee.GetEmployee(Id)
            Session("mEmployee") = mEmployee

            MSGBoxCtrl.Show(MSGBox.Message_title.Delete,
                            MSGBox.Message_text.Delete,
                            "",
                            MsgBoxStyle.YesNo,
                            "Delete")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetObject()

        Try

            mEmployee.Name = Trim(txtPilotName.Text)
            mEmployee.EmpNo = Trim(txtCode.Text)
            mEmployee.DesignationID = New Guid(cmbDesignationList.SelectedValue)
            Session("mEmployee") = mEmployee

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        If control.Enabled = False Or control.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript([GetType], "focusscript", str)

    End Sub

    Private Sub MessageBoxResult()

        Dim Result As MsgBoxResult
        Dim msgCount As Integer = 0
        Result = MSGBoxCtrl.Result

        If Result > 0 Then

            Select Case Result
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Session("sender") = ""
                            mEmployee = CType(Session("mEmployee"), Employee)
                            Employee.DeleteEmployee(mEmployee.ID)

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                "",
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                "",
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 50000 Then

                                MSGBoxCtrl.Show(MSGBox.Message_title.DeleteAlert,
                                                MSGBox.Message_text.DeleteAlert,
                                                ex.Message,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            DataFieldBind()
                            upnlGrid.Update()
                            msgCount = ex.Errors.Count

                        Finally

                            If msgCount = 0 Then

                                NewRecord()
                                DataFieldBind()
                                upnlGrid.Update()
                                upnlMain.Update()

                                MarkLog(Action.Delete,
                                        "Pilot",
                                        mEmployee.Name,
                                        ErrorType.NoError,
                                        mEmployee.ID,
                                        EventLogID)

                            End If

                        End Try

                    End If

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()

            End Select

        End If

    End Sub

    Private Sub SetTitle()

        Try

            If mEmployee.IsNew Then
                lbltitle.Text = "Flying Crew  [New]"
            Else

                If Len(mEmployee.Name) > 15 Then
                    lbltitle.Text = "Flying Crew [" & mEmployee.Name.Substring(0, 15) & "...]"
                Else
                    lbltitle.Text = "Flying Crew [" & mEmployee.Name & "]"
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        Try

            mEmployeeList = EmployeeList.GetEmployeeList("", , , , , True)
            Session("mEmployeeList") = mEmployeeList
            dgPilot.DataSource = mEmployeeList

            mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
            cmbDesignationList.DataSource = mDesignationList
            Session("mDesignationList") = mDesignationList

            DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Sub customvalidate(s As Object, e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbDesignationList" Then
            If cmbDesignationList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Designation from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        Try

            If Not IsPostBack And CType(Session("sender"), String) = "" Then

                If txtPilotName.Enabled = True Then
                    SetFocus(txtPilotName)
                End If

                If IsNothing(Request.QueryString("BackPage1")) Or Request.QueryString("BackPage1") = "" Then
                    Session("MiddleFrame") = "wfPilot.aspx?"
                End If

                DataFieldBind()

            End If

            SetTitle()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            MarkLog(Action.Close,
                    "Pilot",
                    "",
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

            Dim _OpenAs As String = Request.QueryString("Typepup")

            If _OpenAs IsNot Nothing AndAlso _OpenAs = "pup" Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "onclose",
                                                    "CallParentCallback();",
                                                    True)
                Exit Sub

            End If

            If Request.QueryString("BackPage1") = "" Or IsNothing(Request.QueryString("BackPage1")) Then

                Session("sender") = ""
                Session("MiddleFrame") = ""
                Response.Redirect("Dashboard.aspx")

            Else
                Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SavePilot(sender As Object, e As EventArgs) Handles btnSave.Click

        If (Not User.IsInRole("EmployeeNew") And mEmployee.IsNew) Or
           (Not User.IsInRole("EmployeeEdit") And Not mEmployee.IsNew) Then

            SetObject()
            SetSession()

            MarkLog(Action.Save,
                    "Pilot",
                    User.Identity.Name & " is not Authorized User to save " & mEmployee.Name,
                    ErrorType.HandledError,
                    Guid.Empty,
                    EventLogID)

            Dim msg As New SIMsgBox(Page,
                                    SIMsgBox.Message_title.Authorization,
                                    SIMsgBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly) With {
                .ReplacePage = "wfPilot.aspx?MsgResult=0&BackPage=" &
                                Request.QueryString("BackPage") &
                                "&BackPage1=" & Request.QueryString("BackPage1")
            }
            Session("sender") = "Authorization"
            msg.Show()
            Exit Sub

        End If

        If IsValid Then

            Try

                SetObject()

                mEmployee.Save()
                If txtPilotName.Enabled = True Then
                    SetFocus(txtPilotName)
                End If

                MarkLog(Action.Save,
                        "Pilot",
                        mEmployee.Name,
                        ErrorType.HandledError,
                        Guid.Empty,
                        EventLogID)

                mEmployee = Employee.NewPilot()
                DataFieldBind()
                SetSession()
                SetTitle()
                upnlDet.Update()
                upnlGrid.Update()
                upnltitle.Update()

            Catch ex As SqlException

                DataFieldBind()
                upnlGrid.Update()

                If ex.Number = 8145 Then

                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.ProcedureError,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.Duplicate,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 547 Then

                    MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete,
                                    MSGBox.Message_text.ReferenceDelete,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

            End Try

        End If

    End Sub

    Private Sub GV_PilotList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgPilot.RowCommand

        Dim index As Integer
        Dim ID As Guid

        Select Case e.CommandName
            Case "View"

                If (Not User.IsInRole("EmployeeView") And Not User.IsInRole("EmployeeEdit")) Then

                    SetObject()
                    SetSession()

                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Exit Sub

                End If

                ID = New Guid(e.CommandArgument.ToString)
                EditRecord(ID)
                txtPilotName.DataBind()
                txtCode.DataBind()
                cmbDesignationList.DataBind()
                upnlDet.Update()
                upnltitle.Update()
                SetTitle()

                MarkLog(Action.Edit,
                        "Pilot",
                        mEmployee.Name,
                        ErrorType.NoError,
                        mEmployee.ID,
                        EventLogID)

            Case "DeleteRec"

                If (Not User.IsInRole("EmployeeDelete")) Then

                    SetObject()
                    SetSession()

                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Exit Sub

                End If

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 20-04-2023
                index = gvr.RowIndex
                ID = mEmployeeList(index).ID

                DeleteRecord(ID)

        End Select

    End Sub

    Private Sub GV_PilotList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgPilot.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim employee As EmployeeList.EmployeeInfo = CType(e.Row.DataItem, EmployeeList.EmployeeInfo)

                If Not employee.IsWorking Then
                    e.Row.BackColor = Color.Yellow
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub AddDesignation(sender As Object, e As EventArgs) Handles imgBtnAddDesignation.Click

        Try

            SetObject()
            DataFieldBind()
            upnlGrid.Update()
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenDesignationWindow",
                                                "OpenDesignationWindow();",
                                                True)
        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub HdnBtnDesignation(sender As Object, e As EventArgs) Handles hdnimgBtnDesignation.Click

        Try

            mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
            cmbDesignationList.DataSource = mDesignationList
            Session("mDesignationList") = mDesignationList
            cmbDesignationList.DataBind()
            DataFieldBind()
            upnlGrid.Update()
            upnlDet.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub AddPilot(sender As Object, e As EventArgs) Handles btnAdd.Click

        Try

            MarkLog(Action.[New],
                    "Pilot",
                    "",
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

            NewRecord()
            DataFieldBind()

            If txtPilotName.Enabled = True Then
                SetFocus(txtPilotName)
            End If

            SetTitle()
            upnlDet.Update()
            upnlGrid.Update()
            upnltitle.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MSGBoxCtrl.HideControl()
        MessageBoxResult()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

End Class
