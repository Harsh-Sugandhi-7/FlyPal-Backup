
Imports System.Linq
Public Class wfCompMonitorServiceStatusNew_Ajax
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
    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 22-07-2023
    'End
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
    Private Sub ControlToVisibility()
        btnPrint.Enabled = Not mCompMonitorServiceStatus.IsNew
        btnSelect.Enabled = mCompMonitorServiceStatus.IsNew
        REM: For No Frequency
        dgCurrentValue.Columns(3).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)
        dgCurrentValue.Columns(4).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(5).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)

        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        dgDoneOnValue.Columns(6).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(7).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3)

        'End

        dgDoneOnValue.Columns(3).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 4)
        If mCompMonitorServiceStatus.PartMonitorServiceID.Equals(Guid.Empty) Then
            calDoneOn.BackColor = Color.Gainsboro
            calDoneOn.Enabled = False
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True
            txtWorkOrdNo.BackColor = Color.Gainsboro
            txtWorkOrdNo.ReadOnly = True
            chkApplicable.Enabled = False
        End If
        If txtRemark.ReadOnly Then txtRemark.BackColor = Color.Gainsboro
        If txtWorkOrdNo.ReadOnly Then txtWorkOrdNo.BackColor = Color.Gainsboro

        'Commented by Saylee on 28-June-2018 for ALL28062018 for star air, to add DoneOn Date for OC Service'
        ''If Not (mCompMonitorServiceStatus.PartMonitorService.ReadOnlyFrequencyColumn) = False Then calDoneOn.Enabled = False
        '**************************************************************
        If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If
        'Revise Activity
        Dim txtDueOnValue As TextBox
        btnRevise.Enabled = (mCompMonitorServiceStatus.IsApplicable And Not mCompMonitorServiceStatus.IsNew)
        'Added as Due On Value need to be eneterd for expiry service,but disabled for other service types
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)
            If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 Then
                txtDueOnValue.BackColor = Color.White
                txtDueOnValue.ReadOnly = False
            Else
                txtDueOnValue.BackColor = Color.Gainsboro
                txtDueOnValue.ReadOnly = True
            End If
        Next
        'End
        'End
        ControlVisibilityForAttachment()
    End Sub
    Private Sub ControlVisibilityForGridBeforeBinding()
        dgCurrentValue.Columns(3).Visible = True
        dgCurrentValue.Columns(4).Visible = True

        dgDoneOnValue.Columns(5).Visible = True
        dgDoneOnValue.Columns(6).Visible = True
        dgDoneOnValue.Columns(7).Visible = True
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
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Try
                            If Save() = True Then
                                ControlToVisibility()
                                SetTitle()
                                UpdatePanel()
                            End If
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.[New], "Part Service", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mPartMonitorService As PartMonitorService
                        Dim ID As Guid = Guid.NewGuid 'Revise Activity
                        mPartMonitorService = PartMonitorService.NewPartMonitorService(mCompMonitorServiceStatus.PartMonitorService, mMachine.HourType)
                        Session("mPartMonitorService") = mPartMonitorService
                        'RemoveSession()
                        mPartMonitorService.BeginEdit()
                        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                        Session("mPrevCompMonitorServiceStatusForRevise") = mCompMonitorServiceStatus

                        Dim GChildPage2, GChildPage4, GChildPage5, GChildPage6 As String 'Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
                        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow('" + GChildPageTmp + "');", True)
                        GChildPage2 = Trim(Request.QueryString("GChildPage2"))
                        GChildPage4 = Trim(Request.QueryString("GChildPage4"))
                        GChildPage5 = Trim(Request.QueryString("GChildPage5"))
                        GChildPage6 = Trim(Request.QueryString("GChildPage6"))
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow('" + GChildPage2 + "','" + GChildPage4 + "','" + GChildPage5 + "','" + GChildPage6 + "');", True)
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("Sender") = ""
                    End If
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.Close, "ComponentServiceMonitor", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        RemoveSession()
                        Session.Remove("FromLog")
                        Session.Remove("IsBackFromCompliance") 'Added By Vikrant On 03-Jun-2016 For ALL03062016
                        ''Added by Saylee on 9th-Jan-2008======================================
                        'If Request.QueryString("GChildPage4") <> "" Then
                        '    Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                        'ElseIf Request.QueryString("GChildPage2") <> "" Then
                        '    Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        'End If
                        Response.Redirect(Request.QueryString("BackPage"))
                    End If
                    'End
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                    End If
            End Select
        End If
    End Sub
    Private Sub SetObject()
        With mCompMonitorServiceStatus
            If calDoneOn.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = calDoneOn.Text
            End If
            .DoneWONo = txtWorkOrdNo.Text
            .DoneRemark = txtRemark.Text
            .SourceDoc = Trim(txtSourceDoc.Text)
            .RevisionNo = Trim(txtRevisionNo.Text)
            .BookNo = Trim(txtBookNo.Text)
            .PageNo = Trim(txtPageNo.Text)
            .RequiredManHours = Trim(txtActualManHours.Text)
            If txtExtensionDate.Text = "" Then
                .ExtensionDate = System.DBNull.Value
            Else
                .ExtensionDate = txtExtensionDate.Text
            End If
            .ApprovalRemark = Trim(txtApprovalRemark.Text)
            .IsApplicable = chkApplicable.Checked   'Added By Saylee on 10-Sep-2008
            .DoneBy = Trim(txtDoneBy.Text)          'Added by Saylee On 23-Apr-2009
            .IsLater = chkIsLater.Checked           'Added By Prashant 17-Aug-2010
            Dim LicenseNo As String = String.Empty 'Added By Prashant On 12-Jun-2012 FOR ALL08062012
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .LicenseNo = LicenseNo
            .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            .Place = txtPlace.Text.Trim 'End
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If
            If mCompMonitorServiceStatus.IsNew Then mCompMonitorServiceStatus.IsMaster = False 'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        End With
    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, calDoneOn, txtDueOnValue, txtExtensionValue As TextBox
        If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3 Then
            For i As Integer = 0 To Me.dgCurrentValue.Rows.Count - 1
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            Next i
        End If
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            calDoneOn = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox) 'Added By Shital on 25-Jan-2021
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 4 Then 'if condition Added By Vikrant On 09-Dec-2021,while checking heligo issue found that Due on again setting to current in this property
                    If .Item(j).PeriodID = 2 Then
                        If Not Period.IsDate(calDoneOn.Text.Trim) Then
                            .Item(j).CurrentValue = ""
                        Else
                            .Item(j).CurrentValueFormatted = Trim(calDoneOn.Text)
                        End If
                    Else
                        .Item(j).CurrentValue = Trim(calDoneOn.Text)
                    End If
                End If


                'Commented By Vikrant On 09-Dec-2021,while checking heligo issue found that Current and Done On setting same as Due on which is wrong
                'If mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 Then 'Fixed Value/Expiry Service
                '    If .Item(j).PeriodID = 2 Then
                '        If Not Period.IsDate(txtDueOnValue.Text.Trim) Then
                '            .Item(j).CurrentValue = ""
                '        Else
                '            .Item(j).CurrentValue = Trim(txtDueOnValue.Text)
                '        End If
                '    Else
                '        .Item(j).CurrentValue = Trim(txtDueOnValue.Text)
                '    End If
                'End If
                'End

                'Added By Shital on 25-Jan-2021
                If txtExtensionValue Is Nothing Then
                    .Item(j).ExtensionValue = ""
                Else
                    .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)  'Added By Saylee on 28-07-2008 Shital on 25-Jan-2021
                End If
            End With
        Next j
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
    End Sub
    Private Function Save() As Boolean
        Dim CompMonitorServiceStatusClone As CompMonitorServiceStatus
        CompMonitorServiceStatusClone = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mCompMonitorServiceStatus.IsValid = True Then
            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
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
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(title, message, , False), True)
                    Return False
                End If
            End If
            'End
            'aded By Deven on 24-Sep-2009 ------
            mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, PartMonitorServiceID:=mCompMonitorServiceStatus.PartMonitorServiceID.ToString)
            If mCompMonitorServiceStatusList.Contains(mCompMonitorServiceStatus.PartMonitorServiceID) And mCompMonitorServiceStatus.IsNew = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Component Service Status.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            '-----------------------------------
            Try
                mCompMonitorServiceStatus.ApplyEdit()
                mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                'Revise Activity
                If Not Session("mPrevCompMonitorServiceStatusForRevise") Is Nothing Then
                    Dim mPrevCompMonitorServiceStatusForRevise As CompMonitorServiceStatus
                    mPrevCompMonitorServiceStatusForRevise = Session("mPrevCompMonitorServiceStatusForRevise")
                    mPrevCompMonitorServiceStatusForRevise.IsApplicable = False
                    mPrevCompMonitorServiceStatusForRevise.Save()
                    Session.Remove("mPrevCompMonitorServiceStatusForRevise")
                End If
                'End
                SaveAttachment()
                MaintDetail = "Reg No. : " & Machine.GetMachine(mMachineMaintenance.MachineID).RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
                MarkLog(Util.Action.Save, "Component Service Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                Return True
            Catch ex As SqlException
                Session("CompMonitorServiceStatusClone") = CompMonitorServiceStatusClone
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                CompMonitorServiceStatusClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " SerialNo: " & mCompStatus.Comp.SerialNo & " ]"

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "AMP"
            lblMonitorServiceType.InnerText = "Task Type"
        Else
            ServiceMPDTitle = "Service"
        End If


        If mCompMonitorServiceStatus.IsNew Then
            lblTitle.Text = "Component " + ServiceMPDTitle + " Status " & CompInfo & " [New]"
        Else
            lblTitle.Text = "Component " + ServiceMPDTitle + " Status " & CompInfo
        End If
        upnlTitle.Update()
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
        If Not (mMachineMaintenanceList.Contains(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService, "")) Then 'Added by Saylee on 9th-Oct-2009
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, MaintenanceType.ComponentService, calDoneOn.Text, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService)
        End If
        With mMachineMaintenance
            .MaintenanceID = mCompMonitorServiceStatus.ID 'TransactionID
            '.Date = calDoneOn.Text
            If calDoneOn.Text = "" Then
                .Date = System.DBNull.Value
            Else
                .Date = calDoneOn.Text
            End If
            Dim mLog As Log
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(calDoneOn.Text, mAssemblyStatus.MachineID)
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
    Private Sub SaveMachineMaintenance() 'Added by Saylee on 9th-Oct-2009
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
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
    End Sub 'End
    Private Sub ControlVisibilityForAttachment()
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

        If mCompMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If


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
    Private Sub UpdatePanel()
        upnlMonitoringStatusDetails.Update()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
        upnlDocument.Update()
        upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        upnlSelectMonitoringService.Update()
        upnlRevisedDetails.Update() 'Revise Activity
    End Sub
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
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        calDoneOn.Text = mCompMonitorServiceStatus.DoneOnFormatted.ToString  'Added on 28-05-2007 by Saylee
        txtExtensionDate.Text = mCompMonitorServiceStatus.ExtensionDateFormatted.ToString
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList() 'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        If Val(mCompMonitorServiceStatus.PartMonitorService.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorServiceStatus.PartMonitorService.RequiredManHours + ")"
        End If
        BindLicenceNo() 'MLNo
        DataBind()
        'Added by Ajay 22-01-2023
        mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mMachine.ID)
        Session("mLastAMPRef") = mLastAMPRef
        If (mLastAMPRef.AMPNo <> "") Then lblAMPNo.Text = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
    End Sub
    Private Sub DataBindGrid()
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        dgCurrentValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
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
        ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then 'Added By Utkarsh On 13-Jun-2012 FOR ALL08062012
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If 'End
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If btnSelect.Enabled = True Then
                setFocus(btnSelect)
            End If
            DataFieldBind()
            ControlToVisibility()
            SetTitle()
            SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If
    End Sub
    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        SetObject()
        SetGridObject()
        Response.Redirect("wfPartMonitorServiceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfCompMonitorServiceStatusNew_Ajax.aspx")
    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtElapsedValue"), TextBox)
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                .Item(I).ElapsedValue = txtElapsedValue.Text.Trim
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlToVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtRemaining_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemainingValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtRemainingValue"), TextBox)
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                .Item(I).RemainingValue = txtRemainingValue.Text
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlToVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    'Added By Shital on 25-Jan-2021
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlToVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    '--------------End
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim calDoneOn As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            calDoneOn = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(i).CurrentValueFormatted = ""
                    Else
                        .Item(i).CurrentValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(i).CurrentValue = Trim(calDoneOn.Text)
                End If
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlToVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtDueOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDueOnValue As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
            mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(txtDueOnValue.Text.Trim) Then
                        .Item(i).DueOnValueFormatted = ""
                    Else
                        .Item(i).DueOnValueFormatted = Trim(txtDueOnValue.Text)
                    End If
                Else
                    .Item(i).DueOnValue = Trim(txtDueOnValue.Text)
                End If
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlToVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub calDoneOn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDoneOn.TextChanged
        'If IsPostBack Then
        '    SetObject()
        '    DataBindGrid()
        '    upnlRedLabel.Update()
        '    upnlCurrentValueGrid.Update()
        '    upnlDoneOnValueGrid.Update()
        'End If

        'Added by Saylee on 11-Jul-2018 for ALL21062018, to show current values as per Done On Date selection
        If IsPostBack Then
            Dim tmpmCompMonitorServiceStatus As CompMonitorServiceStatus = mCompMonitorServiceStatus.Clone
            If tmpmCompMonitorServiceStatus.IsNew Then
                If calDoneOn.Text <> "" Then
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, calDoneOn.Text, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)
                Else
                    'Revise Activity
                    'mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, Session("mIssueDate"), mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)
                    If tmpmCompMonitorServiceStatus.PartMonitorService.ReviseRemark <> "" Then
                        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, Today.Date.ToString, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
                    Else
                        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, Session("mIssueDate"), mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
                    End If
                    'End

                End If
                With mCompMonitorServiceStatus
                    Dim mPartMonitorService As PartMonitorService = CType(Session("mPartMonitorService"), PartMonitorService)
                    .PartMonitorServiceID(True) = mPartMonitorService.ID
                    '.PartMonitorService.Code = mPartMonitorService.Code
                    .PartMonitorService.Reference = mPartMonitorService.Reference
                    .PartMonitorService.Description = mPartMonitorService.Description
                    .PartMonitorService.RequiredManHours = mPartMonitorService.RequiredManHours

                End With
            Else

                'Added by Saylee on 11-Jul-2018, to show current values as per Done On Date selection
                If calDoneOn.Text = "" Then
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(tmpmCompMonitorServiceStatus.ID, mCompStatus.CompID, mAssemblyStatus.ID, calDoneOn.Text, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, False, mCompStatus)
                Else
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(tmpmCompMonitorServiceStatus.ID, mCompStatus.CompID, mAssemblyStatus.ID, calDoneOn.Text, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, True, mCompStatus)
                End If
            End If

            DataBindGrid()
            ControlToVisibility()
            upnlRedLabel.Update()
            upnlCurrentValueGrid.Update()
            upnlDoneOnValueGrid.Update()
        End If
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
                    ControlToVisibility()
                    SetTitle()
                    UpdatePanel()
                    'MLNo
                    Session.Remove("mMaintenanceDoneByEmployees")
                    Session.Remove("UserNameForLicenceList")
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    'Added on 27-Feb-2020 By Shital
                    If Not calDoneOn.Text = "" Then
                        Response.Redirect("Index.aspx")
                    End If
                    '=====================================================================
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodNotPresent, MSGBox.Message_text.PeriodNotPresent, "Period used to monitor this maintenance activity is not present in Component Status", MsgBoxStyle.OkOnly, "")
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Session("NewPage") = "True" Or mCompMonitorServiceStatus.PartMonitorService.ReviseRemark <> "" Then 'Revise Activity
            Session("NewPage") = "False"
            MarkLog(Util.Action.Close, "Component Service Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            RemoveSession()
            Response.Redirect("Index.aspx")
        Else
            MarkLog(Util.Action.Close, "Component Service Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            RemoveSession()
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mCompMonitorServiceStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        If mCompMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
        End If
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mCompMonitorServiceStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorServiceStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorServiceStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
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
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        Dim mCompanyDetail As New CompanyDetail
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        Rpt = New crDetComponentMonitorServiceStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4

        Dim MPDType As String = ""
        Dim ReportName As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            MPDType = "Task Type"
            ReportName = "Maintenance Events Detail Report"
        Else
            MPDType = "Service Type"
            ReportName = "Component Service Status Detail Report"
        End If

        RHCount = Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
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
                            txtPartMonitorServiceTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values"))
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
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values"))
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
               txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values"))
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
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values"))
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
            'ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
            '  New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
            '   dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, , _
            '   dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText, dgDoneOnValue.Columns.Item(6).HeaderText, IIf(dgDoneOnValue.Columns.Item(5).Visible = True, dgDoneOnValue.Columns.Item(5).HeaderText, "")))
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On",
              New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , , , , , , , , , , lblCompValues.InnerText,
               dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, ,
               dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(5).HeaderText, dgDoneOnValue.Columns.Item(7).HeaderText, IIf(dgDoneOnValue.Columns.Item(6).Visible = True, dgDoneOnValue.Columns.Item(6).HeaderText, "")))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On",
                                 New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , , , , , , , , , , lblCompValues.InnerText))
        End If

        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No. ",
                    txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.",
                        txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done By Agency",
                    txtDoneBy.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done By Agency",
                        txtDoneBy.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No. ",
                    mCompMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No. ",
                        mCompMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblCompValues.InnerText))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place",
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place",
                        txtPlace.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours",
                    txtActualManHours.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours",
                        txtActualManHours.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText))
                End If

            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark",
                    txtRemark.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark",
                        txtRemark.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "",
                    "", , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                   CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), , , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "",
                                          "", , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                                                 "", "", , "", , "", "", , lblNote1.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Done On Details", "",
                "", , , , , , , , , , , , , , , , , lblCompValues.InnerText,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String),
                , lblNote1.Text))
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
            txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", , , "Extension Date ", , txtExtensionDate.Text))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.",
                                txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", , , "Extension Date ", , txtExtensionDate.Text))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                    txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    , , "Approval Remark", , txtApprovalRemark.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                        "", txtApprovalRemark.Text))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details"))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details"))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details"))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details"))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "",
                "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Service", , , , , , , , , lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ReportName, lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        Dim mrptImage As rptImage = rptImage.GetImage(ds) '-----------Added by Utkarsh for Report Logo---------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'Revise Activity
    Private Sub btnRevise_Click(sender As Object, e As System.EventArgs) Handles btnRevise.Click
        MSGBoxCtrl.Show("Alert!", "You are about to Revise Part Activity.After revision of Part activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
    End Sub
    'End

    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        SetObject()
        SetGridObject()
        Session("mFromType") = 3
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mCompMonitorServiceStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(calDoneOn.Text = "", mCompMonitorServiceStatus.AsOnDate.ToString, calDoneOn.Text))

        ''Added by Saylee on 14-Mar-2016 for ALL11032016
        'If mAssemblyStatus.InstalledOn.ToString <> "" Then
        '    If CDate(mCompMonitorServiceStatus.DoneOn) <= CDate(mAssemblyStatus.InstalledOn) Then 'if Compliance date is same or less than Assembly Inst. Date
        '        Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation = FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)
        '        Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation
        '    End If
        'End If

        '*************************************************
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            Dim LogID As String
            LogID = CType(Session("LogID"), String)
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            If mCompMonitorServiceStatus.IsNew = False Then 'Edit record 
                mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mCompMonitorServiceStatus.ID, mCompStatus.CompID,
                                                                                                 mAssemblyStatus.ID, mLog.Date.ToString, mCompStatus.Comp.PartID,
                                                                                                 mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType,
                                                                                                 True, mCompStatus, mLog.ID.ToString)
            Else
                mCompMonitorServiceStatus.LogID(LogID, mLog.Date.ToString, True, CType(Session("mPartMonitorService"), PartMonitorService)) = New Guid(LogID)
            End If

            Session.Remove("FromLog")
            DataBindGrid()
            ControlToVisibility()
            SetTitle()
            upnlCurrentValueGrid.Update()
            upnlDoneOnValueGrid.Update()
        End If
    End Sub
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