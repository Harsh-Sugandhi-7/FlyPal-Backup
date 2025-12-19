'AJAX Conversion by Vikrant On 10-Jun-2015
Imports System.Linq

Public Class wfCompMonitorModStatusNew_Ajax
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
    Public mCompMonitorModStatus As CompMonitorModStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mMachine As Machine
    Private Flag As Int16
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 9th-Oct-2009
    Dim MaintDetail As String
    Dim EventLogID As Guid
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 05-Aug-2013 For ALL01082013
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public mIsSpareComponent As Integer 'Added By Vikrant On 27-Jul-2020 For ALL27072020
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mCompStatus = Session("mCompStatus")
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 9th-Oct-2009
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")

        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareComponent = Session("mIsSpareComponent") 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mCompStatus") = mCompStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 9th-Oct-2009
    End Sub
    Private Sub NewRecord()
        mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.Comp.ID, mAssemblyStatus.ID, mCompMonitorModStatus.AsOnDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        'MarkLog(Util.Action.[New], "CompMonitorModStatus", " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
        MarkLog(Util.Action.[New], "Component Modification Status", "", Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mCompMonitorModStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub ControlToVisibility()
        btnPrint.Enabled = Not mCompMonitorModStatus.IsNew
        btnSelect.Enabled = mCompMonitorModStatus.IsNew
        REM: For No Frequency
        dgCurrentValue.Columns(3).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)
        dgCurrentValue.Columns(4).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(5).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        dgDoneOnValue.Columns(6).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) AndAlso mIsSpareComponent <> 1 'mIsSpareComponent Added By Vikrant On 27-Jul-2020 For ALL27072020
        dgDoneOnValue.Columns(7).Visible = Not mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 3
        'End
        If mCompMonitorModStatus.PartMonitorModID.Equals(Guid.Empty) Then
            calDoneOn.BackColor = Color.Gainsboro
            calDoneOn.Enabled = False
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True
            txtWorkOrdNo.BackColor = Color.Gainsboro
            txtWorkOrdNo.ReadOnly = True
            chkApplicable.Enabled = False 'Added By Rajnish 22-12-2007
        End If
        If txtRemark.ReadOnly Then txtRemark.BackColor = Color.Gainsboro
        If txtWorkOrdNo.ReadOnly Then txtWorkOrdNo.BackColor = Color.Gainsboro
        ' If mCompMonitorModStatus.EnableDoneOn = False Then calDoneOn.Enabled = False
        If Not (mCompMonitorModStatus.PartMonitorMod.ReadOnlyFrequencyColumn) = False Then calDoneOn.Enabled = False
        If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If
        If mCompMonitorModStatus.PartMonitorMod.ReadOnlyFrequencyColumn = True Then chkApplicable.Checked = False
        ControlVisibilityForAttachment()
        btnSelectLog.Visible = (mIsSpareComponent <> 1) ' Added By Vikrant On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCompMonitorModStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 9th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 9th-Oct-2009
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")

        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mCompMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
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
    Private Sub SetObject()
        With mCompMonitorModStatus
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
            .MethodOfCompliance = Trim(txtMethodOfCompliance.Text)  'Added By Saylee on 10-Oct-2024


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
            If mCompMonitorModStatus.IsNew Then mCompMonitorModStatus.IsMaster = False 'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        End With
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, calDoneOn, txtDueOnValue, txtExtensionValue As TextBox
        '.......................
        If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3 Then
            For i As Integer = 0 To Me.dgCurrentValue.Rows.Count - 1
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            Next i
        End If
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            calDoneOn = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox) 'Added By Shital on 25-Jan-2021
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(j).CurrentValue = Trim(calDoneOn.Text)
                End If
                'Added By Shital on 25-Jan-2021
                If txtExtensionValue Is Nothing Then
                    .Item(j).ExtensionValue = ""
                Else
                    .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)  'Added By Saylee on 28-07-2008 Shital on 25-Jan-2021
                End If
            End With
        Next j
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
    End Sub

    Private Function Save() As Boolean
        Dim CompMonitorModStatusClone As CompMonitorModStatus
        CompMonitorModStatusClone = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mCompMonitorModStatus.IsValid = True Then
            If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Component Service Status. Component Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If

            'Added By Vikrant On 05-Aug-2013 For ALL01082013
            If Not mCompMonitorModStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorModStatus.DoneOn.Equals(System.DBNull.Value) Then
                Dim title As String = "Save Alert !"
                Dim message As String = ""
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorModStatus.DoneByID.ToString, mCompMonitorModStatus.DoneOn)
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, , False), True)
                    Return False
                End If
            End If
            'End

            Dim mCompMonitorModStatusList As tmpCompMonitorModStatusList
            mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, PartMonitorModID:=mCompMonitorModStatus.PartMonitorModID.ToString)
            'Session("mAssemblyStatus") = mAssemblyStatus
            'Session("mCompStatus") = mCompStatus
            If mCompMonitorModStatusList.Contains(mCompMonitorModStatus.PartMonitorModID) And mCompMonitorModStatus.IsNew = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Component Modification Status.", MsgBoxStyle.OkOnly, "")
                Return False
            End If

            Try

                mCompMonitorModStatus.ApplyEdit()
                mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                SaveAttachment()
                Dim MaintDetail As String = "Reg No. : " & Machine.GetMachine(mMachineMaintenance.MachineID).RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
                MarkLog(Util.Action.Save, "Component Modification Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                Return True
            Catch ex As SqlException
                Session("CompMonitorModStatusClone") = CompMonitorModStatusClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                CompMonitorModStatusClone = Nothing
                'MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
                'MarkLog(Util.Action.Save, "Component Modification Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)

            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " SerialNo: " & mCompStatus.Comp.SerialNo & " ]"
        If mCompMonitorModStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareComponent = 0, "", IIf(mCompStatus.IsSpareComp, "Stock ", "Removed ")) + " Component Modification Status " & CompInfo & " [New]" 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        Else
            lblTitle.Text = IIf(mIsSpareComponent = 0, "", IIf(mCompStatus.IsSpareComp, "Stock ", "Removed ")) + " Component Modification Status" & CompInfo 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        End If
        upnlTitle.Update()
    End Sub
    Private Sub UpdatePanel()
        upnlMonitoringStatusDetails.Update()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
        'upnlDocument.Update()
        'upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        upnlSelectMonitoringModification.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("sender") = ""
                        Try
                            If Save() = True Then
                                DataFieldBind()
                                ControlToVisibility()
                                SetTitle()
                                UpdatePanel()
                            End If
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("sender") = ""
                    End If
                    'End
                Case MsgBoxResult.Cancel


                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Public Function CheckPeriods() As Boolean 'Added by Saylee on 21-Aug-2008
        SetObject()
        SetGridObject()
        Dim mCompMonitorModStatusPeriod As CompMonitorModStatusPeriod
        For Each mCompMonitorModStatusPeriod In mCompMonitorModStatus.CompMonitorModStatusPeriods
            If Not mCompStatus.CompStatusPeriods.Contains(mCompMonitorModStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 9th-Oct-2009

        If Session("From") = 0 And Not (mMachineMaintenanceList.Contains(mCompMonitorModStatus.ID, 10, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 10, calDoneOn.Text, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatus.ID, 10)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =10
            .MaintenanceID = mCompMonitorModStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            '.Date = calDoneOn.Text
            If calDoneOn.Text = "" Then
                .Date = System.DBNull.Value
            Else
                .Date = calDoneOn.Text
            End If

            ''Dim mLog As Log = CType(Session("mLog"), Log)
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
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mCompMonitorModStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCompMonitorModStatus.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub SaveMachineMaintenance()
        'Added by Saylee on 9th-Oct-2009
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
    'Added By Utkarsh On 17-May-2012 FOR ALL15052012
    Private Sub SetColor()
        If Not mCompMonitorModStatus Is Nothing Then
            If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And Not mCompMonitorModStatus.DoneOn Is System.DBNull.Value Then
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
    'MLNo
    Public Sub SetLicenceCount()
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        'Added on 28-05-2007 by Saylee
        calDoneOn.Text = mCompMonitorModStatus.DoneOnFormatted.ToString      'Added by Saylee on 9th-Oct-2009
        txtExtensionDate.Text = mCompMonitorModStatus.ExtensionDateFormatted.ToString
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        If Val(mCompMonitorModStatus.PartMonitorMod.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorModStatus.PartMonitorMod.RequiredManHours + ")"
        End If

        BindLicenceNo() 'MLNo
        DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        dgCurrentValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
    End Sub
    Private Sub ControlVisibilityForGridBeforeBinding()
        dgCurrentValue.Columns(3).Visible = True
        dgCurrentValue.Columns(4).Visible = True

        dgDoneOnValue.Columns(5).Visible = True
        dgDoneOnValue.Columns(6).Visible = True
        dgDoneOnValue.Columns(7).Visible = True
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

            'Added By Utkarsh On 13-Jun-2012 FOR ALL08062012
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
        SetGridObject()
        Dim str As String = ""
        Dim txtElapsedValue As TextBox
        Dim txtRemainingValue As TextBox
        If Not mCompMonitorModStatus.IsValid Then
            For i As Integer = 0 To mCompMonitorModStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompMonitorModStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If btnSelect.Enabled = True Then
                btnSelect.Focus()
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
        Response.Redirect("wfPartMonitorModList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfCompMonitorModStatusNew_Ajax.aspx")
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
            Dim tmpmCompMonitorModStatus As CompMonitorModStatus = mCompMonitorModStatus.Clone
            If tmpmCompMonitorModStatus.IsNew Then
                If calDoneOn.Text <> "" Then
                    mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, calDoneOn.Text, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
                Else
                    mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, Session("mIssueDate"), mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
                End If
                With mCompMonitorModStatus
                    Dim mPartMonitorMod As PartMonitorMod = CType(Session("mPartMonitorMod"), PartMonitorMod)
                    .PartMonitorModID(True) = mPartMonitorMod.ID
                    '.PartMonitorMod.Code = mPartMonitorMod.Code
                    .PartMonitorMod.Reference = mPartMonitorMod.Reference
                    .PartMonitorMod.Description = mPartMonitorMod.Description
                    .PartMonitorMod.RequiredManHours = mPartMonitorMod.RequiredManHours

                End With
            Else

                'Added by Saylee on 11-Jul-2018, to show current values as per Done On Date selection
                If calDoneOn.Text = "" Then
                    mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(tmpmCompMonitorModStatus.ID, mCompStatus.CompID, mAssemblyStatus.ID,
                                                                                         calDoneOn.Text, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID,
                                                                                         mCompStatus.ID, mMachine.HourType, False, CompStatus:=mCompStatus)
                Else
                    mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(tmpmCompMonitorModStatus.ID, mCompStatus.CompID, mAssemblyStatus.ID,
                                                                                         calDoneOn.Text, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID,
                                                                                         mCompStatus.ID, mMachine.HourType, True, CompStatus:=mCompStatus)
                End If
            End If

            DataBindGrid()
            upnlRedLabel.Update()
            upnlCurrentValueGrid.Update()
            upnlDoneOnValueGrid.Update()
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mCompMonitorModStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorModStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorModStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        If mCompMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mCompMonitorModStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If CheckPeriods() = False Then
                'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And Not mCompMonitorModStatus.DoneOn Is System.DBNull.Value Then
                    MSGBoxCtrl.Show("Save Alert !", "Component Modification Status is one time and you have entered Done On date.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    Exit Sub
                End If
                'End
                If Save() = True Then
                    'DataFieldBind()
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
        If Session("NewPage") = "True" Then
            Session("NewPage") = "False"
            MarkLog(Util.Action.Close, "Component Modification Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            RemoveSession()
            Response.Redirect("index.aspx")
        Else
            MarkLog(Util.Action.Close, "Component Modification Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            RemoveSession()
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        End If
    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtElapsedValue"), TextBox)
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
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
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
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
        For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorModStatus.CompMonitorModStatusPeriods
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
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not IsDate(calDoneOn.Text.Trim) Then
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
            mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
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
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompMonitorModStatus.ID
            Session("MaintenanceDoneOnDate") = mCompMonitorModStatus.DoneOn.ToString
            mMaintenanceDoneByEmployees = mCompMonitorModStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompMonitorModStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Remove(mCompMonitorModStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
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
            If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, MaintenanceType.ComponentModification, DoneByID, LicenseNo, txtActualManHours.Text, EmpName)
            End If

        Else
            If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        BindLicenceNo()
        SetLicenceCount()
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtActualManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mCompMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
    Private Sub btnSelectLog_Click(sender As Object, e As System.EventArgs) Handles btnSelectLog.Click
        SetObject()
        SetGridObject()
        Session("mFromType") = 3
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mCompMonitorModStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(calDoneOn.Text = "", mCompMonitorModStatus.AsOnDate.ToString, calDoneOn.Text))

        ''Added by Saylee on 14-Mar-2016 for ALL11032016
        'If mAssemblyStatus.InstalledOn.ToString <> "" Then
        '    If CDate(mCompMonitorModStatus.DoneOn) <= CDate(mAssemblyStatus.InstalledOn) Then 'if Compliance date is same or less than Assembly Inst. Date
        '        Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation = FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)
        '        Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation
        '    End If
        '    '*************************************************
        'End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
    End Sub

    Private Sub hdnBtnSelectLog_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            Dim LogID As String
            LogID = CType(Session("LogID"), String)
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            If Session("From") = 1 And mCompMonitorModStatus.IsNew = False Then 'Edit record 
                mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mCompMonitorModStatus.ID, mCompStatus.CompID, mAssemblyStatus.ID,
                                                                                        mLog.Date.ToString, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID,
                                                                                        mCompStatus.ID, mMachine.HourType, True, mLog.ID.ToString, CompStatus:=mCompStatus)
            Else
                mCompMonitorModStatus.LogID(LogID, mLog.Date.ToString, True, CType(Session("mPartMonitorMod"), PartMonitorMod)) = New Guid(LogID)
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

#Region " Report "
    '    'Created By :- Pallavi , Date -09/08/2006
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Rpt = New crDetComponentMonitorModStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Mod. Type", _
                  txtMonitorModType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                      dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, , _
                    dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Mod. Type", _
                            txtMonitorModType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                             "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                            txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String), , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String), , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            'ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
            'calDoneOn.Text, , , , , , , , , , , , , , , , , "Component Values", _
            'dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, , _
            'dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText, dgDoneOnValue.Columns.Item(6).HeaderText, IIf(dgDoneOnValue.Columns.Item(5).Visible = True, dgDoneOnValue.Columns.Item(5).HeaderText, "")))
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
           calDoneOn.Text, , , , , , , , , , , , , , , , , "Component Values", _
           dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, , _
           dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(5).HeaderText, dgDoneOnValue.Columns.Item(7).HeaderText, IIf(dgDoneOnValue.Columns.Item(6).Visible = True, dgDoneOnValue.Columns.Item(6).HeaderText, "")))

        Else
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
                                calDoneOn.Text, , , , , , , , , , , , , , , , , "Component Values", _
                                      "", "", , "", , "", ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                    txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                        txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , "Component Values", _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done By Agency", _
                    txtDoneBy.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done By Agency", _
                        txtDoneBy.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No. ", _
                    mCompMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No. ", _
                      mCompMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                        txtPlace.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours", _
                    txtActualManHours.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours", _
                        txtActualManHours.Text, , , , , , , , , , , , , , , , , lblCompValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                    txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                     CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), IIf(dgDoneOnValue.Columns.Item(6).Visible = True, CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormatted, String), "")))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                        txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values", _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                    "", , , , , , , , , , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                   CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), , , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                                          "", , , , , , , , , , , , , , , , , "Component Values", _
                                                 "", "", , "", , "", "", , lblNote1.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                "", , , , , , , , , , , , , , , , , "Component Values", _
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), _
                , lblNote1.Text))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Assembly Modification Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region
#End Region



End Class