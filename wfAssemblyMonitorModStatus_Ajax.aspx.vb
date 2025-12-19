Imports System.Linq
Public Class wfAssemblyMonitorModStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Private Flag As Int16
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
    Private mEnFrom As From
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 13th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 13th-Oct-2009
    Dim EventLogID As Guid
    Public mDirectiveDetail As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mDirectiveNo As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    'Added By Vikrant For ADSBConfig
    Dim IsOpenFromADSB As String = "False"
    Dim RegNo As String = String.Empty
    Public PeriodValues(,) As String
    'End
    Public mIsSpareAssembly As Integer 'Added By Saylee On 27-Jul-2020 For ALL27072020
#End Region

#Region " Enum "  ''Not used in code below!!
    Public Enum From
        FromMaster = 0
        FromEntries = 1
    End Enum
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 13th-Oct-2009
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        'Added By Vikrant For ADSBConfig
        IsOpenFromADSB = Session("IsOpenFromADSB")
        RegNo = Session("RegNo")
        'End
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added By Saylee On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 And txtDoneOnDate.Text <> "" Then
                    txtDnOnDate.Enabled = False
                Else
                    txtDnOnDate.Enabled = True
                End If
            End With
        Next j
    End Sub
    Private Sub ControlToVisibility()
        btnPrint.Enabled = Not mAssemblyMonitorModStatus.IsNew
        btnSelect.Enabled = mAssemblyMonitorModStatus.IsNew
        REM: For No Frequency
        'Added by Saylee on 7-Dec-2010
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "AVE" Then
            'dgCurrentValue.Columns(3).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        Else
            dgCurrentValue.Columns(3).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        End If
        '************************************************
        dgCurrentValue.Columns(4).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(6).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(5).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        'Added By Utkarsh ON 28-Jun-2013 FOR ALL28062013
        dgDoneOnValue.Columns(7).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Saylee On 27-Jul-2020 For ALL27072020
        'End
        If mAssemblyMonitorModStatus.ModelMonitorModID.Equals(Guid.Empty) Then
            txtDoneOnDate.BackColor = Color.Gainsboro
            txtDoneOnDate.Enabled = False
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True
            'Added by Saylee on 16-Dec-2010
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "AVE" Then
                'do nothing- keep WorkOrdNo open
            Else
                txtWorkOrdNo.Enabled = Not (mAssemblyMonitorModStatus.ModelMonitorMod.ReadOnlyFrequencyColumn)
                txtWorkOrdNo.BackColor = Color.Gainsboro
                txtWorkOrdNo.ReadOnly = True
            End If
            chkApplicable.Enabled = False 'Added By Rajnish 22-12-2007
        End If
        If txtRemark.ReadOnly Then txtRemark.BackColor = Color.Gainsboro
        If txtWorkOrdNo.ReadOnly Then txtWorkOrdNo.BackColor = Color.Gainsboro
        'Added by Saylee on 7-Dec-2010
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "AVE" Then
            ''If Not (mAssemblyMonitorModStatus.ModelMonitorMod.ReadOnlyFrequencyColumn) = False Then calDoneOn.Enabled = False
        Else
            If Not (mAssemblyMonitorModStatus.ModelMonitorMod.ReadOnlyFrequencyColumn) = False Then txtDoneOnDate.Enabled = False
        End If
        '************************************************
        If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If
        ControlVisibilityForAttachment()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorModStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 13th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 13th-Oct-2009
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
        Session.Remove("Edit")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()

        Try

            With mAssemblyMonitorModStatus

                If txtDoneOnDate.Text = "" Then
                    .DoneOn = System.DBNull.Value
                Else
                    .DoneOn = txtDoneOnDate.Text
                End If

                .DoneWONo = Trim(txtWorkOrdNo.Text)
                .DoneRemark = Trim(txtRemark.Text)
                .IsApplicable = chkApplicable.Checked
                .SourceDoc = Trim(txtSourceDoc.Text)
                .RevisionNo = Trim(txtRevisionNo.Text)
                .BookNo = Trim(txtBookNo.Text)
                .PageNo = Trim(txtPageNo.Text)
                .RequiredManHours = Trim(txtRequiredManHours.Text)

                If txtExtensionDate.Text = "" Then
                    .ExtensionDate = System.DBNull.Value
                Else
                    .ExtensionDate = txtExtensionDate.Text
                End If

                .ApprovalRemark = txtApprovalRemark.Text
                .IsLater = chkIsLater.Checked          'Added By Prashant 17-Aug-2010
                'Added By Prashant On 12-Jun-2012 FOR ALL08062012
                Dim LicenseNo As String = String.Empty
                Dim EmpName As String = String.Empty

                If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                    LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                    EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2,
                                  txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
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

                End If

                .MethodOfCompliance = Trim(txtMethodOfCompliance.Text) 'Added By Harsh on 10-Oct-2024

            End With

            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, calDoneOn, txtDueOnValue, txtExtensionValue As TextBox
        If mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3 Then
            For i As Integer = 0 To Me.dgCurrentValue.Rows.Count - 1
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
                With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                    .Item(i).ElapsedValue = txtElapsedValue.Text.Trim
                    .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
                End With
            Next i
        End If
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            calDoneOn = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(j).DoneOnValue = ""
                    Else
                        .Item(j).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(j).DoneOnValue = Trim(calDoneOn.Text)
                End If
                .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next j
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    Private Function Save() As Boolean
        Dim AssemblyMonitorModStatusClone As AssemblyMonitorModStatus
        AssemblyMonitorModStatusClone = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 13th-Oct-2009
        If mAssemblyMonitorModStatus.IsValid = True Then
            If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Directives Status.Assembly Directives Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfAssemblyMonitorModStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
                'msg1.Show()
                'Return False
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Assembly Directives Status.Assembly Directives Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            End If
            'aded By Deven on 24-Sep-2009 ------
            If Not Session("IsOpenFromADSB") = "True" Then 'Condition Added By Vikrant For ADSBConfig
                If mAssemblyMonitorModStatusList.Contains(mAssemblyMonitorModStatus.ModelMonitorModID) And mAssemblyMonitorModStatus.IsNew = True Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Assembly Directive Status.", MsgBoxStyle.OkOnly, "")
                    Return False
                    Exit Function
                End If
            End If
            
            '-------------------------------------------------

            Try
                'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                If Not mAssemblyMonitorModStatus.DoneByID.Equals(Guid.Empty) AndAlso mAssemblyMonitorModStatus.DoneOn.ToString.Length > 0 Then
                    Dim Title As String = "Save Alert !"
                    Dim Message As String = ""
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorModStatus.DoneByID.ToString, mAssemblyMonitorModStatus.DoneOn.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
                        Return False
                    End If
                End If
                'End
                mAssemblyMonitorModStatus.ApplyEdit()
                mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
                SaveAttachment()
                SaveMachineMaintenance()  'Added by Saylee on 13th-Oct-2009
                mDirectiveNo = txtModNumber.Text
                mMonitorType = txtModelMonitorModTypeName.Text
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                ControlToVisibility()
                Return True
            Catch ex As SqlException
                Session("AssemblyMonitorModStatusClone") = AssemblyMonitorModStatusClone
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                AssemblyMonitorModStatusClone = Nothing
                mDirectiveDetail = "Reg No. : " & RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Directive No. : " & mDirectiveNo & " Description : " & mAssemblyMonitorModStatus.ModelMonitorMod.Description & " Monitor Type : " & mAssemblyMonitorModStatus.ModelMonitorMod.ModelMonitorModTypeName
                MarkLog(Util.Action.Save, "Assembly Directive Status", mDirectiveDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        Dim AssemblyInfo As String = "[Model: " & mAssemblyStatus.ModelName & " SerialNo: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        If mAssemblyMonitorModStatus.IsNew Then
            lblTitle.Text = "Assembly Directives Status " & AssemblyInfo & " [New]"
        Else
            lblTitle.Text = "Assembly Directives Status" & AssemblyInfo
        End If
        lblAssemblyValues.InnerText = mAssemblyStatus.AssemblyTypeName & " Values"
        upnlTitle.Update()
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
                                SetTitle()
                                UpdatePanel()
                            End If
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("Sender") = ""
                        UpdatePanel()
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                    End If
            End Select
        End If
    End Sub
    Public Function CheckPeriods() As Boolean 'Added by Saylee on 21-Aug-2008
        SetObject()
        SetGridObject()
        Dim mAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriod
        For Each mAssemblyMonitorModStatusPeriod In mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
            If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mAssemblyMonitorModStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 13th-Oct-2009
        If Not (mMachineMaintenanceList.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 7, txtDoneOnDate.Text.ToString, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
        End If
        With mMachineMaintenance
            .MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID
            If txtDoneOnDate.Text <> "" Then
                .Date = txtDoneOnDate.Text
            Else
                .Date = System.DBNull.Value
            End If
            Dim mLog As Log
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtDoneOnDate.Text.ToString, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(txtDoneOnDate.Text, mAssemblyStatus.MachineID)
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
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mAssemblyMonitorModStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID)
            Session("mFileAttach") = mFileAttach
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
                If (Not mAssemblyMonitorModStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyMonitorModStatus.ID)
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
    'Added By Utkarsh On 15-Mar-2011
    Private Sub SetRights()
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyModificationPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyModificationPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    '*******************************
    'Added By Utkarsh On 16-May-2012 FOR ALL15052012
    Private Sub SetColor()
        If Not mAssemblyMonitorModStatus Is Nothing Then
            If mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is System.DBNull.Value Then
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
    Private Sub UpdatePanel()
        upnlDoneOnDetails.Update()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
        upnlDocumentDetails.Update()
        upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        upnlSelectMonitoringService.Update()
    End Sub
    'MLNo
    Public Sub SetLicenceCount()
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    'Added By Vikrant For ADSBConfig
    Private Sub NewRecord(ByVal LogID As Guid, ByVal LogDate As String, ByVal ModelMonitorModID As Guid)
        Dim mAssemblyStatusList As AssemblyStatusList
        Dim mMachineList As MachineList
        Dim LatestRemovedOn As SmartDate
        Dim AssemblyStatusID As Guid = Guid.Empty

        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mAssemblyStatus.MachineID.ToString _
        , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , _
        , , SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList

        If mAssemblyStatusList.Count = 0 Then
            mMachineList = MachineList.GetMachineListWithRemoval(LogDate, Guid.Empty.ToString _
                   , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , _
                   , SkipIsForInventoryAircarft:=True)
            For i As Integer = 0 To mMachineList.Count - 1
                If mMachineList(i).AssemblyStatusList.Count > 0 Then

                    Dim mtempAssemblyList = (From AssemblyStatusInfo As AssemblyStatusInfo In mMachineList(i).AssemblyStatusList
                                                        Order By CDate(AssemblyStatusInfo.RemovedOn) Descending
                                                        Select AssemblyStatusInfo).ToList
                    If AssemblyStatusID.Equals(Guid.Empty) Then
                        AssemblyStatusID = mtempAssemblyList(0).ID
                        LatestRemovedOn = New SmartDate(mtempAssemblyList(0).RemovedOn.ToString)
                    ElseIf LatestRemovedOn.CompareTo(New SmartDate(mtempAssemblyList(0).RemovedOn.ToString)) < 0 Then
                        AssemblyStatusID = mtempAssemblyList(0).ID
                        LatestRemovedOn = mtempAssemblyList(0).RemovedOn
                    End If
                End If
            Next
        Else
            AssemblyStatusID = mAssemblyStatusList(0).ID
        End If
        'End

        'Here instead of mPrevAssemblyMonitorInspStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType)

        mAssemblyMonitorModStatus.ModelMonitorModID(False) = ModelMonitorModID

        mAssemblyMonitorModStatus.BeginEdit()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean, ByVal AssemblyMonitorModStatusID As Guid)
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(AssemblyMonitorModStatusID, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.HourType, DoneOnDate, mAssemblyStatus.Assembly.ModelID, IsConsiderDoneOnDate:=IIf(Session("IsOpenFromADSB") = "True", True, False))
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    Private Sub SetFromClone(ByVal clnAssemblyMonitorInspStatus As AssemblyMonitorModStatus)
        mAssemblyMonitorModStatus.DoneWONo = clnAssemblyMonitorInspStatus.DoneWONo
        mAssemblyMonitorModStatus.DoneRemark = clnAssemblyMonitorInspStatus.DoneRemark
        mAssemblyMonitorModStatus.DoneByID = clnAssemblyMonitorInspStatus.DoneByID
        mAssemblyMonitorModStatus.LicenseNo = clnAssemblyMonitorInspStatus.LicenseNo
        mAssemblyMonitorModStatus.Place = clnAssemblyMonitorInspStatus.Place
        'Added By Vikrant On 25-Nov-2014
        mAssemblyMonitorModStatus.IsAttachmentAdded = clnAssemblyMonitorInspStatus.IsAttachmentAdded
        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyMonitorInspStatus.MaintenanceDoneByEmployees
            If Session("From") = 0 Then 'New Record
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            ElseIf Session("From") = 1 Then 'Edit Record
                If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                End If
            End If
        Next
        'End
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAssemblyMonitorModStatus.ID
            Session("mFileAttach") = mFileAttach
        End If
        'End
        clnAssemblyMonitorInspStatus = Nothing
    End Sub
    Public Sub SetGridObjectFromObject()
        Dim j As Int32
        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        ReDim PeriodValues(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)
        'End
        For j = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1

            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                        .Item(j).DoneOnValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                        '*********************************************************
                        '.Item(j).DoneOnValueFormatted = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                        .Item(j).DoneOnValueFormatted = txtDoneOnDate.Text
                        '*********************************************************
                        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID  'To Check same Period
                        PeriodValues(j, 1) = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                        'End
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                    .Item(j).DoneOnValue = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                    'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID 'To Check same Period
                    PeriodValues(j, 1) = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                    'End
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted)
            End With
        Next j
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    'End
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods

        'Added on 28-05-2007 by Saylee
        txtDoneOnDate.Text = mAssemblyMonitorModStatus.DoneOnFormatted.ToString
        txtExtensionDate.Text = mAssemblyMonitorModStatus.ExtensionDateFormatted.ToString

        'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours + ")"
        BindLicenceNo() 'MLNo
        DataBind()
        ControlVisibilityForDatePeriod()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        dgCurrentValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
        ControlVisibilityForDatePeriod()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "txtRemark" Then
        '    If Len(txtRemark.Text) > 500 Then
        '        custValidator.ErrorMessage = "Max. length of Remark should be 500 char"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'Added By Prashant On 121-Jun-2012 FOR ALL08062012
        If custValidator.ControlToValidate = "txtLicenceNo" Then
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
        If Not mAssemblyMonitorModStatus.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorModStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyMonitorModStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
                setFocus(btnSelect)
            End If
            DataFieldBind()
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If
        ControlToVisibility()
        SetRights()  'Added By Utkarsh On 15-Mar-2011
        SetTitle()
        SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
    End Sub
    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        SetObject()
        SetGridObject()
        Response.Redirect("wfModelMonitorModList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfAssemblyMonitorModStatus_Ajax.aspx")
    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtElapsedValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                .Item(I).ElapsedValue = txtElapsedValue.Text.Trim
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Protected Sub txtRemainingValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemainingValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtRemainingValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                .Item(I).RemainingValue = txtRemainingValue.Text
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim calDoneOn As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            calDoneOn = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                '.Item(i).IsCalculate = chkcalValues.Checked
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(i).DoneOnValueFormatted = ""
                    Else
                        .Item(i).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(i).DoneOnValue = Trim(calDoneOn.Text)
                End If
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Protected Sub txtDueOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDueOnValue As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
            mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                ' .Item(i).IsCalculate = chkcalValues.Checked
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
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        'If IsPostBack Then
        '    SetObject()
        '    DataBindGrid()
        '    upnlRedLabel.Update()
        '    upnlCurrentValue.Update()
        '    upnlDoneOnValue.Update()
        'End If
        If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyMonitorModStatus.DoneOn.ToString), SmartDate.StringToDate(txtDoneOnDate.Text)) <> 0 Then
            If IsOpenFromADSB = "True" Then 'Added For ADSBConfig
                Dim AsOnDate As String
                If txtDoneOnDate.Text <> "" Then
                    If CDate(txtDoneOnDate.Text) <= CDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
                        AsOnDate = mAssemblyStatus.AsOnDateFormatted.ToString
                    Else
                        AsOnDate = txtDoneOnDate.Text
                    End If
                Else
                    AsOnDate = Today.Date.ToString(AppSettings("DateFormat"))
                End If

                Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus = mAssemblyMonitorModStatus.Clone
                If Session("Edit") = True Or Not mAssemblyMonitorModStatus.IsNew Then 'Old Record
                    EditRecord(Guid.Empty, AsOnDate, False, clnAssemblyMonitorModStatus.ID)
                Else 'New Record
                    NewRecord(Guid.Empty, AsOnDate, clnAssemblyMonitorModStatus.ModelMonitorMod.ID)
                End If
                SetFromClone(clnAssemblyMonitorModStatus)
                SetGridObjectFromObject()
            Else
                SetObject()
            End If
            DataBindGrid()
            upnlRedLabel.Update()
            upnlCurrentValue.Update()
            upnlDoneOnValue.Update()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If CheckPeriods() = False Then
                'Added By Utkarsh On 16-May-2012 FOR ALL15052012
                If mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is System.DBNull.Value Then
                    MSGBoxCtrl.show("Save Alert!", "You are about to comply one time directive status.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    Exit Sub
                End If
                'End
                If Save() = True Then
                    SetTitle()
                    UpdatePanel()
                    ''  MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")

                    'Added by Saylee on 28-Sep-2022 for Review Meeting
                    Dim mADSBConfiguration As ADSBConfiguration
                    mADSBConfiguration = Session("mADSBConfiguration")

                    If Not mADSBConfiguration Is Nothing Then
                        mADSBConfiguration.ModelMonitorModID = mAssemblyMonitorModStatus.ModelMonitorMod.ID
                        mADSBConfiguration.AssemblyStatusID = mAssemblyStatus.ID


                        Try
                            mADSBConfiguration.Save()
                            MSGBoxCtrl.show("AD/SB Configuration..!!!", "SuccessFully Configured..!!!", "", MsgBoxStyle.OkOnly, "")
                        Catch ex As Exception
                            MSGBoxCtrl.show("AD/SB Configuration..!!!", "Configuration Failed..!!!", "", MsgBoxStyle.OkOnly, "")
                        End Try
                    Else
                        MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    End If
                    ''*****************************************************************


                Else
                    upnlValidationSummary.Update()
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodNotPresent, MSGBox.Message_text.PeriodNotPresent, "Period used to monitor this maintenance activity is not present in Assembly Status", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not mAssemblyMonitorModStatus.IsNew Then
            mDirectiveDetail = "Reg No. : " & RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Directive No. : " & mDirectiveNo & " Description : " & mAssemblyMonitorModStatus.ModelMonitorMod.Description & " Monitor Type : " & mAssemblyMonitorModStatus.ModelMonitorMod.ModelMonitorModTypeName
            MarkLog(Util.Action.Close, "Assembly Directive Status", mDirectiveDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
        Else
            MarkLog(Util.Action.Close, "Assembly Directive Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mAssemblyMonitorModStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
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
        mAssemblyMonitorModStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorModStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorModStatus.ID)
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
            Session("mMaintenanceID") = mAssemblyMonitorModStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyMonitorModStatus.DoneOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtRequiredManHours.DataBind()
        upnlDoneOnDetails.Update()
    End Sub
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
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
            If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, 7, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If
        Else
            If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlDoneOnDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            upnlDoneOnDetails.Update()
        End If
    End Sub
    'End
#End Region

#Region " Report "
    '    'Created By :- Pallavi , Date -09/08/2006
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Rpt = New crDetAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Type ", _
                  txtModelMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                      dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, _
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Type ", _
                            txtModelMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                             "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number", _
                             txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number", _
                            txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                            txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Note", _
                                   txtDirNote.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Note", _
                                    txtDirNote.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , lblNote.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 6
        RHCount1 = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
            New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
            dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, , _
            dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText, _
            dgDoneOnValue.Columns.Item(5).HeaderText, dgDoneOnValue.Columns.Item(6).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
                                New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                      "", "", , "", , "", ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                    txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                        txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                    mAssemblyMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                        mAssemblyMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                        txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours ", _
                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours ", _
                        txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                    txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                        txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                    "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                                          "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                                 "", "", , "", , "", "", , lblNote1.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DoneOnValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        If temp2 < RHCount2 Then
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
            txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
            dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, "Extension Date ", _
            dgDoneOnValue.Columns.Item(3).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(4).HeaderText, _
            dgDoneOnValue.Columns.Item(5).HeaderText, dgDoneOnValue.Columns.Item(6).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
                                txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                                      "", txtExtensionDate.Text, , "", , "", ""))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                    txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Assembly Directive Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
#End Region
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