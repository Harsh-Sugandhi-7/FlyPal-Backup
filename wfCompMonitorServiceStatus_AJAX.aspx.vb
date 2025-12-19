'AJAX Created By: Saylee on 4-May-2015

Imports System.Linq

Public Class wfCompMonitorServiceStatus_AJAX
    Inherits System.Web.UI.Page

#Region "Enumeration"
    Private Enum MaintenanceType
        AssemblyInstallation = 1
        AssemblyRemoval = 2
        ComponentInstallation = 3
        ComponentRemoval = 4
        AssemblyService = 5
        AssemblyInspection = 6
        AssemblyDirective = 7
        ComponentService = 8
        ComponentInspection = 9
        ComponentModification = 10
    End Enum
#End Region

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Private Flag As Int16
    Public mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 13th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 13th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Dim MaintDetail As String 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013

    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public mIsSpareComp As Boolean 'Added by Shital on 30-Sep-2020 for All27072020

    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 20-07-2023

#End Region

#Region " Busines Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        mCompMonitorServiceStatusList = CType(Session("mCompMonitorServiceStatusList"), tmpCompMonitorServiceStatusList)

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 13th-Oct-2009

        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")

        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareComp = Session("IsSpareComp") 'Added by Shital on 30-Sep-2020 for All27072020
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 13th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 13th-Oct-2009

        Session("mFileAttach") = mFileAttach 'Added By Prashant  On 27-Nov-2014
        Session("IsAttachmentDeleted") = IsAttachmentDeleted 'Added By Prashant  On 27-Nov-2014
    End Sub
    Private Sub GetAttachment()
        If mCompMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCompMonitorServiceStatus")

        Session.Remove("mMachineMaintenance")       'Added by Saylee on 13th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 13th-Oct-2009

        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")

        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End

    End Sub
    Private Sub NewRecord()
        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mCompMonitorServiceStatus.IsNew
        btnSelect.Enabled = mCompMonitorServiceStatus.IsNew
        dgCurrentValue.Columns(2).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 4)
        dgCurrentValue.Columns(3).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)
        dgCurrentValue.Columns(4).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)

        dgDoneOnValue.Columns(2).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 4)
        dgDoneOnValue.Columns(3).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 4)
        dgDoneOnValue.Columns(6).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        'dgDoneOnValue.Columns(7).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)
        If mIsSpareComp = False Then 'mIsSpareComp Added By Shital On 1-OCt-2020 For ALL27072020
            dgDoneOnValue.Columns(7).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) AndAlso mAssemblyStatus.IsSpareAssembly = False
            dgDoneOnValue.Columns(8).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) AndAlso mAssemblyStatus.IsSpareAssembly = False AndAlso mIsSpareComp = False
        Else
            dgDoneOnValue.Columns(7).Visible = False
            dgDoneOnValue.Columns(8).Visible = False
        End If

        'End
        'Added By Saylee on 23-07-2008
        dgDoneOnValue.Columns(5).Visible = ((mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) And (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 4))

        'If Not mCompMonitorServiceStatus.EnableDoneOn Then   'previos condn of added code
        If mCompMonitorServiceStatus.PartMonitorService.ID.Equals(Guid.Empty) Then   'Added Code
            calDoneOn.BackColor = Color.Gainsboro
            calDoneOn.Enabled = False               'Added Code 
            txtWorkOrderNo.BackColor = Color.Gainsboro
            txtWorkOrderNo.ReadOnly = True         'Added Rajnish on 22-12-2007
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True               'Added Rajnish on 22-12-2007
        End If
        If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If

        ControlVisibilityForAttachment()
        'Commented by Rajnish on 22-12-2007
        'If mCompMonitorServiceStatus.EnableDoneOn = False Then calDoneOn.Enabled = False 'Added Code
    End Sub
    Private Sub ControlVisibilityForGridBeforeBinding()
        dgCurrentValue.Columns(2).Visible = True
        dgCurrentValue.Columns(3).Visible = True
        dgCurrentValue.Columns(4).Visible = True
        dgDoneOnValue.Columns(2).Visible = True
        dgDoneOnValue.Columns(3).Visible = True
        dgDoneOnValue.Columns(6).Visible = True
        dgDoneOnValue.Columns(7).Visible = True
        dgDoneOnValue.Columns(8).Visible = True
        dgDoneOnValue.Columns(5).Visible = True
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        Save()
                        NewRecord()
                        DataFieldBind()
                        SetPage()
                        ControlVisibilityForDatePeriod()
                        upnlMonitoringStatusDetails.Update()
                        upnlDoneOnValueGrid.Update()
                        upnlCurrentValueGrid.Update()
                        upnlDocument.Update()
                        upnlTitle.Update()
                        'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                        'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                    ElseIf MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("sender") = ""
                        If Save() = True Then
                            SetPage()
                            ControlVisibility()
                            upnlActionBtn.Update()
                            upnlMonitoringStatusDetails.Update()
                            upnlDoneOnValueGrid.Update()
                            upnlCurrentValueGrid.Update()
                            upnlDocument.Update()
                            upnlTitle.Update()
                            upnlMonitoringSelect.Update()
                            'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                        End If
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        NewRecord()
                        DataFieldBind()
                        SetPage()
                        ControlVisibilityForDatePeriod()
                        upnlMonitoringStatusDetails.Update()
                        upnlDoneOnValueGrid.Update()
                        upnlCurrentValueGrid.Update()
                        upnlDocument.Update()
                        upnlTitle.Update()
                        'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                        'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                    ElseIf MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("sender") = ""
                        'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    End If
                    'End
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    SetPage()
                    ControlVisibilityForDatePeriod()
                    upnlMonitoringStatusDetails.Update()
                    upnlDoneOnValueGrid.Update()
                    upnlCurrentValueGrid.Update()
                    upnlDocument.Update()
                    upnlTitle.Update()
                    'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    SetPage()
                    ControlVisibilityForDatePeriod()
                    upnlMonitoringStatusDetails.Update()
                    upnlDoneOnValueGrid.Update()
                    upnlCurrentValueGrid.Update()
                    upnlDocument.Update()
                    upnlTitle.Update()
                    'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetObject()
        With mCompMonitorServiceStatus
            If calDoneOn.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = calDoneOn.Text
            End If
            .DoneWONo = txtWorkOrderNo.Text
            .DoneRemark = txtRemark.Text
            .SourceDoc = Trim(txtSourceDoc.Text)
            .RevisionNo = Trim(txtRevisionNo.Text)
            .BookNo = Trim(txtBookNo.Text)
            .PageNo = Trim(txtPageNo.Text)
            .RequiredManHours = Trim(txtActualManHours.Text)

            'Added By Saylee on 23-07-2008=======================
            'CNDC
            If txtExtensionDate.Text = "" Then
                .ExtensionDate = System.DBNull.Value
            Else
                .ExtensionDate = txtExtensionDate.Text
            End If

            .ApprovalRemark = Trim(txtApprovalRemark.Text)
            '====================================================
            .IsApplicable = chkApplicable.Checked   'Added By Saylee on 10-Sep-2008
            .DoneBy = Trim(txtDoneBy.Text)          'Added by Saylee On 23-Apr-2009
            .IsLater = chkIsLater.Checked           'Added By Prashant 17-Aug-2010

            'Added By Prashant On 12-Jun-2012 FOR ALL08062012
            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .LicenseNo = LicenseNo
            .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            .Place = txtPlace.Text.Trim
            'End
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
                'Else
                '    .IsAttachmentAdded = False
            End If
        End With
    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, txtDoneOnValue, txtDueOnValue, txtExtensionValue As TextBox
        With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3 Then
                    'If .Item(i).PeriodID = 2 Then
                    '    .Item(i).ElapsedValueFormatted = Trim(txtElapsedValue.Text)
                    '    .Item(i).RemainingValueFormatted = Trim(txtRemainingValue.Text)
                    'Else
                    If Not mCompStatus.IsThrustMonitoringComp Then .Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
                    .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
                    'End If
                End If
            Next i
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtDoneOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)
                txtDueOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
                ''Added By Saylee on 23-07-2008
                txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 4 Then
                    If .Item(i).PeriodID = 2 Then
                        If Not Period.IsDate(txtDoneOnValue.Text.Trim) Then
                            .Item(i).DoneOnValueFormatted = ""
                        Else
                            .Item(i).DoneOnValueFormatted = Trim(txtDoneOnValue.Text)
                        End If
                    Else
                        .Item(i).DoneOnValue = Trim(txtDoneOnValue.Text)
                    End If
                End If

                ''Commented By Saylee on 23-07-2008
                'If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3 Then
                '    If .Item(i).PeriodID = 2 Then
                '        If Not Period.IsDate(txtDueOnValue.Text.Trim) Then
                '            .Item(i).DueOnValueFormatted = ""
                '        Else
                '            .Item(i).DueOnValueFormatted = txtDueOnValue.Text.Trim
                '        End If
                '    Else
                '        .Item(i).DueOnValue = txtDueOnValue.Text.Trim
                '    End If
                'End If
                'Added By Saylee on 23-07-2008
                'ExtensionValue
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            Next i
        End With
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
    End Sub
    Private Function Save() As Boolean
        Dim CompMonitorServiceStatusClone As CompMonitorServiceStatus
        CompMonitorServiceStatusClone = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 13th-Oct-2009
        If mCompMonitorServiceStatus.IsValid = True Then
            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
                'MessageBox.Show("Component Service Status can not be saved without period units.", "Comp Monitor Service Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodUnitRequired, SIMsgBox.Message_text.PeriodUnitRequired, "You are trying to save Component Service Status. Component Service Status can not be saved without period units.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Component Service Status. Component Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If

            'Added By Vikrant On 06-Aug-2013 For ALL01082013
            If Not mCompMonitorServiceStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorServiceStatus.DoneOn.Equals(System.DBNull.Value) Then
                Dim title As String = "Save Alert !"
                Dim message As String = ""
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorServiceStatus.DoneByID.ToString, mCompMonitorServiceStatus.DoneOn)
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message))
                    Return False
                End If
            End If
            'End

            'aded By Deven on 24-Sep-2009 ------
            If Not Session("IsOpenFromMPD") = "True" Then 'Condition Added By Saylee For MPD on 5-Jan-2023
                If mCompMonitorServiceStatusList.Contains(mCompMonitorServiceStatus.PartMonitorServiceID) And mCompMonitorServiceStatus.IsNew = True Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "Component Service Status.", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Component Service Status.", MsgBoxStyle.OkOnly, "")
                    Return False
                End If
                '-----------------------------------
            End If
            Try
                mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
                SaveMachineMaintenance()  'Added by Saylee on 13th-Oct-2009
                SaveAttachment()
                'Commented By Utkarsh On 28-Jul-2011 For All19072011

                '     MarkLog(Util.Action.Save, "CompMonitorSerStatus", " Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)

                'End

                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                Return True
            Catch ex As SqlException
                Session("CompMonitorServiceStatusClone") = CompMonitorServiceStatusClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                CompMonitorServiceStatusClone = Nothing
                'Added by Saylee on 10-Feb-2020,  All27072020
                Dim mRegNo As String = ""
                If mIsSpareComp = False Then   'Added by Shital on 05-Oct-2020,  All27072020
                    If mAssemblyStatus.IsSpareAssembly = False Then
                        mRegNo = "Reg No. : " & mMachine.RegNo
                    End If
                End If

                'Added By Utkarsh On 28-Jul-2011 For All19072011

                'MaintDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
                If mIsSpareComp = False Then   'Added by Shital on 05-Oct-2020,  All27072020
                    MaintDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
                Else
                    MaintDetail = " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
                End If
                MarkLog(Util.Action.Save, "Component Service Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)

                'End

            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetPage()
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " SerialNo: " & mCompStatus.Comp.SerialNo & " ]"

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "AMP"
            lblMonitorServiceType.InnerText = "Task Type"
            btnSelect.Text = "Select Monitoring MPD"
            btnSelect.ToolTip = "Click to open Part MPD List screen"
        Else
            ServiceMPDTitle = "Component Service"
            btnSelect.Text = "Select Monitoring Service"
            btnSelect.ToolTip = "Click to open Part Service List screen"
        End If


        If mCompMonitorServiceStatus.IsNew Then
            lblTitle.Text = ServiceMPDTitle + " Status " & CompInfo & " [New]"
        Else
            lblTitle.Text = ServiceMPDTitle + " Status" & CompInfo
        End If
    End Sub
    Public Function CheckPeriods() As Boolean 'Added by Saylee on 21-Aug-2008
        SetObject()
        SetGridObject()
        Dim mCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriod
        For Each mCompMonitorServiceStatusPeriod In mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
            If Not mCompStatus.CompStatusPeriods.Contains(mCompMonitorServiceStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 13th-Oct-2009

        If Not (mMachineMaintenanceList.Contains(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService, "")) Then
            'mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, MaintenanceType.ComponentService, calDoneOn.Text, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
            If mIsSpareComp = False Then
                mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, MaintenanceType.ComponentService, calDoneOn.Text, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
            Else
                mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(Guid.Empty, MaintenanceType.ComponentService, calDoneOn.Text, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, Guid.Empty)
            End If
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mCompMonitorServiceStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            If calDoneOn.Text <> "" Then
                .Date = calDoneOn.Text
            Else
                .Date = System.DBNull.Value
            End If

            '' Dim mLog As Log = CType(Session("mLog"), Log)
            Dim mLog As Log
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                'mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mIsSpareComp = False Then
                    mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                Else
                    mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, Guid.Empty, Guid.Empty)
                End If

                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    ' mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(calDoneOn.Text, mAssemblyStatus.MachineID)
                    If mIsSpareComp = False Then
                        mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(calDoneOn.Text, mAssemblyStatus.MachineID)
                    Else
                        mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(calDoneOn.Text, Guid.Empty)
                    End If
                    If mMaxLogNo.Count <> 0 Then
                        .LogNo = mMaxLogNo(0).LogNo
                        .LogID = mMaxLogNo(0).LogId
                        .LogPageNo = mMaxLogNo(0).LogPageNo
                    End If
                End If
                'End
            End If

        End With

        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub SaveMachineMaintenance()
        'Added by Saylee on 13th-Oct-2009
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
        ''  End If
    End Sub
    Private Sub SetRights()
        If mIsSpareComp = False Then 'If Condition added by shitalon 30-sep-2020 for ALL27072020

            If mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentServicePrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentServiceNew") Or User.IsInRole("MachineComponentServiceEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                End If
            ElseIf Not mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentServicePrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentServiceNew") Or User.IsInRole("MachineComponentServiceEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                End If
            End If
        End If
    End Sub
    'Added By Utkarsh On 17-May-2012 FOR ALL15052012
    Private Sub SetColor()
        If Not mCompMonitorServiceStatus Is Nothing Then
            If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And Not mCompMonitorServiceStatus.DoneOn Is System.DBNull.Value Then
                Dim txtdueOnValue As TextBox
                For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
                    txtdueOnValue = CType(dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
                    txtdueOnValue.BackColor = System.Drawing.Color.Red
                    txtdueOnValue.ForeColor = System.Drawing.Color.White
                Next
                lblRed.Visible = True
                lblInfo.Visible = True
            Else
                lblRed.Visible = False
                lblInfo.Visible = False
            End If
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForAttachment()
        ' If mFileAttach.Size > 0 Then 'change from  to current condition
        If mCompMonitorServiceStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                    'mEmployee.IsAttachmentAdded = True
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mCompMonitorServiceStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCompMonitorServiceStatus.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment()
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    'End
    'MLNo
    Public Sub SetLicenceCount()
        If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 And calDoneOn.Text <> "" Then
                    txtDnOnDate.Enabled = False
                Else
                    txtDnOnDate.Enabled = True
                End If
            End With
        Next j
    End Sub
    Private Sub NewRecordService()

        'mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)
        If mIsSpareComp = False Then 'If Condition Added by Shital for All27072020
            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)
        Else
            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mCompStatus.HourType, mCompStatus)
        End If
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Install Component Service Status", "", Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
        'End
    End Sub
    Private Sub NewRecordInsp()
        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        'mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mMachine.HourType)
        If mIsSpareComp = False Then 'If Condition Added by Shital for All27072020
            mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mMachine.HourType)
        Else
            mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mCompStatus.HourType)
        End If
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Install Component Insp Status", "", Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
        'End
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgDoneOnValue.DataBind()


        calDoneOn.Text = mCompMonitorServiceStatus.DoneOnFormatted.ToString


        'Added By Saylee on 23-07-2008=======================
        txtExtensionDate.Text = mCompMonitorServiceStatus.ExtensionDateFormatted.ToString

        'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        If Val(mCompMonitorServiceStatus.PartMonitorService.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorServiceStatus.PartMonitorService.RequiredManHours + ")"
        End If
        BindLicenceNo() 'MLNo

		'Added by Ajay 21-01-2023
		If Not mMachine Is Nothing Then
			mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mMachine.ID)
			Session("mLastAMPRef") = mLastAMPRef
			If (mLastAMPRef.AMPNo = "") Then

			Else
				lblAMPNo.Text = "AMP No.:" + mLastAMPRef.AMPNo + ",Rev No.:" + mLastAMPRef.RevNo + ",Dated:" + mLastAMPRef.FromDateFormatted
			End If
		End If


		DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        dgCurrentValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
        ControlVisibilityForDatePeriod()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 500 char"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Prashant On 121-Jun-2012 FOR ALL08062012
        ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtElapsedValue As TextBox
        Dim txtRemainingValue As TextBox
        If Not mCompMonitorServiceStatus.IsValid Then
            For i As Integer = 0 To mCompMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            If Not mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvRemark.ErrorMessage = str
            cvRemark.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 28-Jul-2011 For All19072011
        If Not IsPostBack Then
            If btnSelect.Enabled = True Then
                setFocus(btnSelect)
            End If
            Session("mLogList") = Nothing
            DataFieldBind()
            ControlVisibility()
            ControlVisibilityForDatePeriod()
            SetRights()
            SetPage()
            SetColor()

            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If

    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
            Dim a As String = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).AssemblyDueOnValueFormatted
        Next
        'ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mCompMonitorServiceStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Protected Sub txtRemaining_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemainingValue As TextBox
        For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
                Dim a As String = .Item(i).AssemblyDueOnValueFormatted
            End With
        Next
        'ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCompMonitorServiceStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDoneOnValue As TextBox
        For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            txtDoneOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)

            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Period.IsDate(txtDoneOnValue.Text.Trim) Then
                        .Item(i).DoneOnValueFormatted = Trim(txtDoneOnValue.Text)
                    Else
                        .Item(i).DoneOnValueFormatted = ""
                    End If
                Else
                    .Item(i).DoneOnValue = Trim(txtDoneOnValue.Text)
                End If
                Dim a As String = .Item(i).AssemblyDueOnValueFormatted
            End With
        Next
        'ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtDueOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDueOnValue As TextBox
        For j As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)

            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Period.IsDate(txtDueOnValue.Text.Trim) Then
                        .Item(j).DueOnValueFormatted = txtDueOnValue.Text.Trim
                    Else
                        .Item(j).DueOnValueFormatted = ""
                    End If
                Else
                    .Item(j).DueOnValue = Trim(txtDueOnValue.Text)
                End If
                Dim a As String = .Item(j).AssemblyDueOnValueFormatted
            End With
        Next
        'ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
                Dim a As String = .Item(i).AssemblyDueOnValueFormatted
            End With
        Next
        'ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If CheckPeriods() = False Then
                'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And Not mCompMonitorServiceStatus.DoneOn Is System.DBNull.Value Then
                    MSGBoxCtrl.Show("Save Alert !", "Component Service Status is one time and you have entered Done On date.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    Exit Sub
                End If
                'End
                If Save() = True Then
                    'Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    SetPage()
                    ControlVisibility()
                    upnlActionBtn.Update()
                    upnlMonitoringStatusDetails.Update()
                    upnlDoneOnValueGrid.Update()
                    upnlCurrentValueGrid.Update()
                    upnlDocument.Update()
                    upnlTitle.Update()
                    upnlMonitoringSelect.Update()
                    'MLNo
                    Session.Remove("mMaintenanceDoneByEmployees")
                    Session.Remove("UserNameForLicenceList")
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    upnlValidationSummary.Update()
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodNotPresent, MSGBox.Message_text.PeriodNotPresent, "Period used to monitor this maintenance activity is not present in Component Status", MsgBoxStyle.OkOnly, "")
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorServiceStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorServiceStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        SetObject()
        SetGridObject()
        Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=wfCompMonitorServiceStatus_AJAX.aspx")
    End Sub
    Private Sub calDoneOn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDoneOn.TextChanged
        If IsPostBack Then      'Added Code on May,29,2007
            SetObject()
            DataBindGrid()
            SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
            upnlRedLabel.Update()
            upnlDoneOnValueGrid.Update()
            upnlCurrentValueGrid.Update()
        End If
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        Dim mCompanyDetail As New CompanyDetail
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

        Rpt = New crDetComponentMonitorServiceStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim MPDType As String = ""
        Dim ReportName As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            MPDType = "MPD Type"
            ReportName = "AMP Status Detail Report"
        Else
            MPDType = "Service Type"
            ReportName = "Component Service Status Detail Report"
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", MPDType,
                  txtPartMonitorServiceTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                    dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText,
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", MPDType,
                            txtPartMonitorServiceTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter",
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                        CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                        CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                        CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                        CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter",
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                             "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference",
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference",
               txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                    "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description",
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                      CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                      CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                      CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                      CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description",
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
     CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
     CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
     CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
     CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), ,
     , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , "", , , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), ,
     , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
                   New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , dgDoneOnValue.Columns.Item(8).HeaderText, , , , , , , , , "Component Values",
                   dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText,
                 , dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText,
                  dgDoneOnValue.Columns.Item(5).HeaderText, dgDoneOnValue.Columns.Item(6).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
                            New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , , , , , , , , , , "Component Values",
                                  "", "", , "", , "", ""))
        End If

        'LHData6:= CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame , String)

        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No. ",
                    txtWorkOrderNo.Text, , , , , , , ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String),
))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.",
                        txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values",
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                    txtRemark.Text, , , , , , , ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                        txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                    txtActualManHours.Text, , , , , , , ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                        txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                    txtDoneBy.Text, , , , , , , ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                        txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No. ",
                    mCompMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No. ",
                        mCompMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                    txtPlace.Text, , , , , , , ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                        txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                    "", , , , , , , ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String),
                    "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                                          "", , , , , , , , , , , , , , , , , "Component Values",
                                                 "", "", , "", , "", "", , "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
                End If


            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                                   "", , , , , , , ,
                                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String),
                      "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
            End If
        Next

        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        If temp2 < RHCount2 Then
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.",
            txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
            dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, "Extension Date ",
            dgDoneOnValue.Columns.Item(2).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(3).HeaderText,
            dgDoneOnValue.Columns.Item(4).HeaderText, dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.",
                                txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                                      "", txtExtensionDate.Text, , "", , "", ""))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                    txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "",
                "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Service",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ReportName, lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mRegNo As String = ""

        If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020
            If mAssemblyStatus.IsSpareAssembly = False Then

                mRegNo = "Reg No. : " & mMachine.RegNo
            End If
        End If

        'Changed By Utkarsh On 28-Jul-2011 For All19072011
        If Not mCompMonitorServiceStatus.IsNew Then
            'MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
            'MaintDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
            If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020
                MaintDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
            Else
                MaintDetail = " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
            End If
            MarkLog(Util.Action.Close, "Component Service Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
        Else
            MarkLog(Util.Action.Close, "Component Service Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        'End

        RemoveSession()
        If Not Session("URLForCompInst") Is Nothing Then
            Dim mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
            Dim mPartMonitorServiceList As PartMonitorServiceList
            Dim mtmpCompMonitorInspStatusList As tmpCompMonitorInspStatusList
            Dim mPartMonitorInspList As PartMonitorInspList

            Dim IsAllServicesAddedForComp As Boolean = True
            Dim IsAllInspAddedForComp As Boolean = True

            ''mPartMonitorServiceList = Session("mPartMonitorServiceList")
            mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(mCompStatus.Comp.PartID, Guid.Empty)
            If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString) 'ture is for mCompStatus.IsMaster 
            Else
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , Guid.Empty.ToString, Guid.Empty.ToString) 'ture is for mCompStatus.IsMaster 
            End If

            For i As Integer = 0 To mPartMonitorServiceList.Count - 1
                If Not mCompMonitorServiceStatusList.Contains(mPartMonitorServiceList(i).ID) Then
                    IsAllServicesAddedForComp = False
                    Exit For
                End If
            Next
            Session.Remove("mPartMonitorServiceList")
            If Not Session("StatusPageOpenFrom") Is Nothing And Not IsAllServicesAddedForComp Then
                NewRecordService()
                'Dim URLForPartServiceList As Stack = CType(Session("URLForPartServiceList"), Stack)
                'Dim UrlToSearch As String = URLForPartServiceList.Peek.ToString
                'Dim GChildPage2 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage2=") + 12, UrlToSearch.IndexOf("&GChildPage4=") - UrlToSearch.IndexOf("GChildPage2=") - 12)

                'If GChildPage2 = "" Then 'Open From Comp Installation
                '    Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx & &GChildPage5=wfInstallComp_AJAX.aspx")
                'Else
                '    If GChildPage2 = "wfAssemblyStatus_Ajax.aspx" Then 'Open From Comp Installation Aircarft Master
                '        Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfAssemblyStatus_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                '    Else 'Open From Assembly Installation In Maint
                '        Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                '    End If
                'End If
                If Session("StatusPageOpenFrom") = "" Then 'Open From Comp Installation
                    'Session.Remove("StatusPageOpenFrom")
                    Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx & &GChildPage5=wfInstallComp_AJAX.aspx")
                Else
                    If Session("StatusPageOpenFrom") = "wfAssemblyStatus_Ajax.aspx" Then 'Open From Comp Installation Aircarft Master
                        'Session.Remove("StatusPageOpenFrom")
                        Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfAssemblyStatus_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                    Else 'Open From Assembly Installation In Maint
                        'Session.Remove("StatusPageOpenFrom")
                        Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                    End If
                End If
            End If

            mPartMonitorInspList = PartMonitorInspList.GetPartMonitorInspList(mCompStatus.Comp.PartID, Guid.Empty)

            'mtmpCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString) 'ture is for mCompStatus.IsMaster 

            If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020
                mtmpCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString) 'ture is for mCompStatus.IsMaster 
            Else
                mtmpCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , Guid.Empty.ToString, Guid.Empty.ToString) 'ture is for mCompStatus.IsMaster 
            End If

            For i As Integer = 0 To mPartMonitorInspList.Count - 1
                If Not mtmpCompMonitorInspStatusList.Contains(mPartMonitorInspList(i).ID) Then
                    IsAllInspAddedForComp = False
                    Exit For
                End If
            Next

            If Not IsAllInspAddedForComp Then
                NewRecordInsp()
                'Dim URLForPartInspList As Stack = CType(Session("URLForPartInspList"), Stack)
                Dim mCompMonitorInspStatusList As tmpCompMonitorInspStatusList

                ' mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString)
                If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020
                    mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString)
                Else
                    mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , Guid.Empty.ToString, Guid.Empty.ToString, Guid.Empty.ToString)
                End If
                Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList

                'Dim URLForPartServiceList As Stack = CType(Session("URLForPartServiceList"), Stack)
                'Dim UrlToSearch As String = URLForPartServiceList.Peek.ToString
                'Dim GChildPage2 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage2=") + 12, UrlToSearch.IndexOf("&GChildPage4=") - UrlToSearch.IndexOf("GChildPage2=") - 12)
                'Dim GChildPage4 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage4=") + 12, UrlToSearch.IndexOf("&GChildPage5=") - UrlToSearch.IndexOf("GChildPage4=") - 12)
                'Dim GChildPage5 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage5=") + 12, UrlToSearch.IndexOf("&GChildPage6=") - UrlToSearch.IndexOf("GChildPage5=") - 12)
                'Dim GChildPage6 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage6=") + 12, UrlToSearch.Length - UrlToSearch.IndexOf("GChildPage6=") - 12)
                'Response.Redirect("wfPartMonitorInspList_Ajax.aspx?GChildPage2=" + GChildPage2 + "&GChildPage4=" + GChildPage4 & "&GChildPage5=" + GChildPage5 + "&GChildPage6=" & GChildPage6)
                If Session("StatusPageOpenFrom") = "" Then 'Open From Comp Installation
                    Session.Remove("StatusPageOpenFrom")
                    Response.Redirect("wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx & &GChildPage5=wfInstallComp_AJAX.aspx")
                Else
                    If Session("StatusPageOpenFrom") = "wfAssemblyStatus_Ajax.aspx" Then 'Open From Comp Installation Aircarft Master
                        Session.Remove("StatusPageOpenFrom")
                        Response.Redirect("wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfAssemblyStatus_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                    Else 'Open From Assembly Installation In Maint
                        Session.Remove("StatusPageOpenFrom")
                        Response.Redirect("wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                    End If
                End If
            End If
            Dim URLForCompInst As Stack = CType(Session("URLForCompInst"), Stack)
            Session.Remove("URLForCompInst")
            Response.Redirect(URLForCompInst.Peek.ToString)
        End If
        Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompMonitorServiceStatus.ID
            Session("MaintenanceDoneOnDate") = mCompMonitorServiceStatus.DoneOn.ToString
            mMaintenanceDoneByEmployees = mCompMonitorServiceStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Remove(mCompMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        'SetObject()
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        Session("LicenseNo") = LicenseNo
        Session("EmployeeID") = DoneByID
        If Not DoneByID.Equals(Guid.Empty) Then
            If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService, DoneByID, LicenseNo, txtActualManHours.Text, EmpName)
            End If

        Else
            If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        BindLicenceNo()
        SetLicenceCount()
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtActualManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
#End Region

#Region "Service Methods"
    'MLNo
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetLicenseNoList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mLicenses As LicenseNoListWithEmployee
        mLicenses = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, UserNameForLicenceList, , , False)

        If count = 0 Then
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
        Else
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

End Class