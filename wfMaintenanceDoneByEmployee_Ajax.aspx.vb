
Public Class wfMaintenanceDoneByEmployee_Ajax
    Inherits System.Web.UI.Page

#Region " Variables and Declarations "

    Dim mMaintTypeID As Integer
    Dim EventLogID As Guid
    Dim mMaintenanceID As Guid
    Dim mMaintenanceDoneByEmployees As MaintenanceDoneByEmployees
    'Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty

#End Region

#Region " Methods "

    Private Sub GetSession()
        mMaintenanceID = CType(Session("mMaintenanceID"), Guid)
        mMaintenanceDoneByEmployees = CType(Session("mMaintenanceDoneByEmployees"), MaintenanceDoneByEmployees)
        'mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        LicenseNo = Session("LicenseNo")
        DoneByID = Session("EmployeeID")
        mMaintTypeID = Session("mMaintTypeID")
        EmpName = Session("EmpName")
    End Sub

    Private Sub RemoveSession()
        ' Session.Remove("mMaintenanceID")
        Session.Remove("LicenseNo")
        Session.Remove("EmployeeID")
        Session.Remove("EmpName")
        Session.Remove("EditLicenceNo")
        Session.Remove("MaintenanceDoneOnDate")
        Session.Remove("mID")
    End Sub

    Private Sub setControls(ID As Guid)
        'txtLicenceNo.Text = mMaintenanceDoneByEmployees.CurrentItem.LicenceNo + " [" + mMaintenanceDoneByEmployees.CurrentItem.EmployeeName + "]"
        'txtRequiredManHours.Text = mMaintenanceDoneByEmployees.CurrentItem.RequiredManHours

        txtLicenceNo.Text = mMaintenanceDoneByEmployees(ID).LicenceNo + " [" + mMaintenanceDoneByEmployees(ID).EmployeeName + "]"
        txtRequiredManHours.Text = mMaintenanceDoneByEmployees(ID).RequiredManHours
    End Sub

    Private Sub ClearControls()
        txtLicenceNo.Text = String.Empty
        txtRequiredManHours.Text = String.Empty
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        'Added Code
                        Session("sender") = ""
                        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
                        Dim mID As Guid = CType(Session("mID"), Guid)
                        'mMaintenanceDoneByEmployees.Remove(mMaintenanceDoneByEmployees.CurrentItem.ID, "")
                        mMaintenanceDoneByEmployees.Remove(mID, "")
                        Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
                        ClearControls()
                        Session.Remove("EditLicenceNo")
                        'MaintenanceDoneByEmployee.DeleteMaintenanceDoneByEmployee(mMaintenanceDoneByEmployees.CurrentItem.ID, mMaintenanceDoneByEmployees.CurrentItem.MaintenanceID)
                        DataFieldBind()
                        SetTitle()
                        upnlDetails.Update()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                    End If
            End Select
        ElseIf Result1 = -1 Then
        End If
    End Sub

    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

        Dim CustomValidator As CustomValidator
        CustomValidator = CType(s, CustomValidator)

        Try

            If CustomValidator.ControlToValidate = "txtLicenceNo" Then

                If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or
                   (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then

                    Dim LcnNo As String = String.Empty
                    Dim EmployeeName As String = String.Empty
                    Dim EmpID As Guid = Guid.Empty

                    If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then

                        LcnNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                        EmployeeName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2,
                                           txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim

                    Else
                        LcnNo = Trim(txtLicenceNo.Text)
                    End If

                    EmpID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo:=LcnNo, EmpName:=EmployeeName).EmpID

                    If EmpID.Equals(Guid.Empty) Then

                        CustomValidator.ErrorMessage = "Please select Licence No. from the List."
                        e.IsValid = False

                    Else
                        e.IsValid = True
                    End If

                Else

                    CustomValidator.ErrorMessage = "Enter Correct License No."
                    e.IsValid = False

                End If

            ElseIf CustomValidator.ControlToValidate = "txtRequiredManHours" Then

                Dim mActualManHrs As New Period(1, DBNull.Value, 0, True, False)
                mActualManHrs.Value = Trim(txtRequiredManHours.Text)

                If Not mActualManHrs.IsValid And mActualManHrs.Value <> "" Then
                    CustomValidator.ErrorMessage = "Actual Man Hours : " + mActualManHrs.ErrMsg
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub TrimLicenceNo()
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        Session("LicenseNo") = LicenseNo
        Session("EmployeeID") = DoneByID
        Session("EmpName") = EmpName
    End Sub

    Private Sub SetTitle()
        lblResult.Text = "License Nos. : " + mMaintenanceDoneByEmployees.Count.ToString + " Record(s) found."
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()
        '  mMaintenanceDoneByEmployees = MaintenanceDoneByEmployees.GetMaintenanceDoneByEmployees(mMaintenanceID)
        'Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
        dgMaintDoneByList.DataSource = mMaintenanceDoneByEmployees
        dgMaintDoneByList.DataBind()
        txtRequiredManHours.DataBind()
        lblRequiredmanHours.DataBind()
    End Sub

    Private Sub ControlVisibility()
        If mMaintTypeID = 1 Or mMaintTypeID = 2 Or mMaintTypeID = 3 Or mMaintTypeID = 4 Or mMaintTypeID = 11 Or mMaintTypeID = 12 Then
            dgMaintDoneByList.Columns(3).Visible = False
        Else
            dgMaintDoneByList.Columns(3).Visible = True
        End If
    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If txtLicenceNo.Enabled = True Then
                txtLicenceNo.Focus()
            End If
            mMaintTypeID = CType(Request.QueryString("MaintTypeID"), Integer)
            Session("mMaintTypeID") = mMaintTypeID
            DataFieldBind()
            SetTitle()
            ControlVisibility()
        End If
    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click ', btnCloseTop.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub

    Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAdd.Click ', btnAddTop.Click
        If IsValid Then
            Dim DoneOnDate As String = Session("MaintenanceDoneOnDate")
            TrimLicenceNo()
            If (Not DoneByID.Equals(Guid.Empty)) AndAlso (DoneOnDate <> "") Then
                Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(DoneByID.ToString, DoneOnDate)
                If (mEmployeeStatus(0).Information <> "") Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, mEmployeeStatus(0).Information, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
            If Session("EditLicenceNo") = "True" Then
                Dim mID As Guid = CType(Session("mID"), Guid)
                If (Not mMaintenanceDoneByEmployees(mID).Equals(LicenseNo, DoneByID)) And mMaintenanceDoneByEmployees.Contains(LicenseNo, DoneByID) Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("EditLicenceNo") = "False"

                mMaintenanceDoneByEmployees(mID).LicenceNo = LicenseNo
                mMaintenanceDoneByEmployees(mID).RequiredManHours = Trim(txtRequiredManHours.Text)
                mMaintenanceDoneByEmployees(mID).EmployeeID = DoneByID
                mMaintenanceDoneByEmployees(mID).EmployeeName = EmpName
            Else
                If mMaintenanceDoneByEmployees.Contains(LicenseNo, DoneByID) Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mMaintenanceDoneByEmployees.Add(mMaintenanceID, mMaintTypeID, Guid.Empty, LicenseNo, txtRequiredManHours.Text, EmpName)
                mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = DoneByID
            End If
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
        Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
        dgMaintDoneByList.DataSource = mMaintenanceDoneByEmployees
        dgMaintDoneByList.DataBind()
        SetTitle()
        ClearControls()
        upnlDetails.Update()
    End Sub

    Private Sub GV_MaintDoneByList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgMaintDoneByList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid
                Session("EditLicenceNo") = "True"
                mID = New Guid(e.CommandArgument.ToString)
                'mMaintenanceDoneByEmployees.CurrentIndex = CInt(e.CommandArgument)
                Session("mID") = mID
                Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
                setControls(mID)
            Case "DeleteRec"
                MSGBoxCtrl.Show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "Delete")
                Dim mID As Guid
                mID = New Guid(e.CommandArgument.ToString)
                'mMaintenanceDoneByEmployees.CurrentIndex = CInt(e.CommandArgument)
                Session("mID") = mID
                Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
        End Select
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub GV_MaintDoneByList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgMaintDoneByList.Sorting
        mMaintenanceDoneByEmployees.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
        dgMaintDoneByList.DataSource = mMaintenanceDoneByEmployees
        dgMaintDoneByList.DataBind()
    End Sub

#End Region

End Class