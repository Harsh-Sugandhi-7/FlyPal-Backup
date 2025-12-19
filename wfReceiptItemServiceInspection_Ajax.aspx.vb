Public Class wfReceiptItemServiceInspection_Ajax
    Inherits System.Web.UI.Page

#Region " Variables and Declarations "
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Dim mMaintTypeID As Integer
    Dim EventLogID As Guid
    Dim mMaintenanceID As Guid
    Dim mMaintenanceDoneByEmployees As MaintenanceDoneByEmployees
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
#End Region

#Region " Methods "
    Private Sub GetSession()
        mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
        mMaintenanceID = CType(Session("mMaintenanceID"), Guid)
        mMaintenanceDoneByEmployees = CType(Session("mMaintenanceDoneByEmployees"), MaintenanceDoneByEmployees)
        DoneByID = Session("EmployeeID")
        mMaintTypeID = Session("mMaintTypeID")
        EmpName = Session("EmpName")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("EmployeeID")
        Session.Remove("EmpName")
        Session.Remove("MaintenanceDoneOnDate")
        Session.Remove("mID")
    End Sub
    Private Sub setControls(ID As Guid)
    End Sub
    Private Sub ClearControls()
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
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "txtLicenceNo" Then
        '    If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
        '        Dim LcnNo As String = String.Empty
        '        Dim EmployeeName As String = String.Empty
        '        Dim EmpID As Guid = Guid.Empty
        '        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
        '            LcnNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
        '            EmployeeName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        '        Else
        '            LcnNo = Trim(txtLicenceNo.Text)
        '        End If
        '        EmpID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LcnNo, EmployeeName).EmpID
        '        If EmpID.Equals(Guid.Empty) Then
        '            custValidator.ErrorMessage = "Please select Licence No. from given list"
        '            e.IsValid = False
        '        Else
        '            e.IsValid = True
        '        End If

        '    Else
        '        custValidator.ErrorMessage = "Enter Correct License No."
        '        e.IsValid = False
        '    End If
        'ElseIf custValidator.ControlToValidate = "txtRequiredManHours" Then
        '    Dim mActualManHrs As New Period(1, DBNull.Value, 0, True, False)
        '    mActualManHrs.Value = Trim(txtRequiredManHours.Text)
        '    If Not mActualManHrs.IsValid And mActualManHrs.Value <> "" Then
        '        custValidator.ErrorMessage = "Actual Man Hours : " + mActualManHrs.ErrMsg
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
    End Sub
    Private Sub TrimLicenceNo()
        'If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
        '    LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
        '    EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        'Else
        '    LicenseNo = Trim(txtLicenceNo.Text)
        'End If
        'DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        'Session("LicenseNo") = LicenseNo
        'Session("EmployeeID") = DoneByID
        'Session("EmpName") = EmpName
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        '    Dim custValidator As CustomValidator
        '    custValidator = CType(s, CustomValidator)
        '    SetObject()
        '    Dim str As String = ""

        '    If Not mMaintenanceDoneByEmployees.IsValid Then
        '        For k As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
        '            For i As Integer = 0 To mMaintenanceDoneByEmployees(k).GetBrokenRulesCollection.Count - 1
        '                str = str + mMaintenanceDoneByEmployees(k).GetBrokenRulesCollection(i).Description + "<BR>"
        '            Next
        '        Next
        '    End If

        '    If str <> "" Then
        '        custValidator.ErrorMessage = str
        '        e.IsValid = False
        '    End If
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "List of Receipt Item Service Inspection. : " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Count.ToString + " Record(s) found."

        'btnAddTop.Visible = mMaintenanceDoneByEmployees.Count > 8
        'btnCloseTop.Visible = mMaintenanceDoneByEmployees.Count > 8
    End Sub
    Public Function CustomValidateForDate()
        Dim strMsg As String = ""
        For i As Integer = 0 To dgReceiptItemServiceInspectionList.Rows.Count - 1
            Dim txt As TextBox
            txt = dgReceiptItemServiceInspectionList.Rows(i).FindControl("txtDoneDate")
            If Len(txt.Text) = 0 Or txt.Text = "" Then
                strMsg = "Start Date is required"
                cv.ErrorMessage = strMsg
                cv.IsValid = False
                Return False
                Exit Function
            ElseIf CDate(txt.Text) > Today.Date Then
                strMsg = "Start Date should not be greater than today's date"
                cv.ErrorMessage = strMsg
                cv.IsValid = False
                Return False
                Exit Function
            End If
        Next
        Return True
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgReceiptItemServiceInspectionList.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections
        dgReceiptItemServiceInspectionList.DataBind()
    End Sub
    Private Sub ControlVisibility()
        dgReceiptItemServiceInspectionList.Enabled = IIf(mReceiptCumInvoice.StatusID = 2, False, True)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            SetTitle()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        If CustomValidateForDate() = True Then
            For i As Integer = 0 To dgReceiptItemServiceInspectionList.Rows.Count - 1
                Dim txt As TextBox
                txt = dgReceiptItemServiceInspectionList.Rows(i).FindControl("txtDoneDate")
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(i).ServiedInspectedCheckDoneOnDate = IIf(txt.Text <> "", txt.Text, System.DBNull.Value)
            Next
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    'Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
    '    If IsValid Then
    '        Dim DoneOnDate As String = Session("MaintenanceDoneOnDate")
    '        TrimLicenceNo()
    '        If (Not DoneByID.Equals(Guid.Empty)) AndAlso (DoneOnDate <> "") Then
    '            Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(DoneByID.ToString, DoneOnDate)
    '            If (mEmployeeStatus(0).Information <> "") Then
    '                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, mEmployeeStatus(0).Information, MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            End If
    '        End If
    '        If Session("EditLicenceNo") = "True" Then
    '            Dim mID As Guid = CType(Session("mID"), Guid)
    '            If (Not mMaintenanceDoneByEmployees(mID).Equals(LicenseNo, DoneByID)) And mMaintenanceDoneByEmployees.Contains(LicenseNo, DoneByID) Then
    '                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            End If
    '            Session("EditLicenceNo") = "False"

    '            mMaintenanceDoneByEmployees(mID).LicenceNo = LicenseNo
    '            mMaintenanceDoneByEmployees(mID).RequiredManHours = Trim(txtRequiredManHours.Text)
    '            mMaintenanceDoneByEmployees(mID).EmployeeID = DoneByID
    '            mMaintenanceDoneByEmployees(mID).EmployeeName = EmpName
    '        Else
    '            If mMaintenanceDoneByEmployees.Contains(LicenseNo, DoneByID) Then
    '                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            End If
    '            mMaintenanceDoneByEmployees.Add(mMaintenanceID, mMaintTypeID, Guid.Empty, LicenseNo, txtRequiredManHours.Text, EmpName)
    '            mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = DoneByID
    '        End If
    '    Else
    '        upnlValidationSummary.Update()
    '        Exit Sub
    '    End If
    '    Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
    '    dgReceiptItemServiceInspectionList.DataSource = mMaintenanceDoneByEmployees
    '    dgReceiptItemServiceInspectionList.DataBind()
    '    SetTitle()
    '    ClearControls()
    '    upnlDetails.Update()
    'End Sub
    Private Sub dgReceiptItemServiceInspectionList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptItemServiceInspectionList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid
                mID = New Guid(e.CommandArgument.ToString)
                'mMaintenanceDoneByEmployees.CurrentIndex = CInt(e.CommandArgument)
                Session("mID") = mID
                Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
                setControls(mID)
            Case "DeleteRec"
                MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
                Dim mID As Guid
                mID = New Guid(e.CommandArgument.ToString)
                'mMaintenanceDoneByEmployees.CurrentIndex = CInt(e.CommandArgument)
                Session("mID") = mID
                Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
        End Select
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgReceiptItemServiceInspectionList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgReceiptItemServiceInspectionList.Sorting
        mMaintenanceDoneByEmployees.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
        dgReceiptItemServiceInspectionList.DataSource = mMaintenanceDoneByEmployees
        dgReceiptItemServiceInspectionList.DataBind()
    End Sub
    'Private Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtLicenceNo.TextChanged
    '    TrimLicenceNo()
    '    If mMaintenanceDoneByEmployees.Count > 0 Then
    '        mMaintenanceDoneByEmployees(0).EmployeeID = DoneByID
    '        mMaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
    '    Else
    '        mMaintenanceDoneByEmployees.Add(mMaintenanceID, mMaintTypeID, DoneByID, LicenseNo, txtRequiredManHours.Text)
    '    End If
    '    Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
    'End Sub
#End Region

End Class