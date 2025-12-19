

'Created By Saylee on 13-Dec-2024


Imports System.Linq
Imports System.Text
Public Class wfAssemblyMonitorServiceStatusGroupConfigureFromMPD_Ajax
    Inherits System.Web.UI.Page
#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyMonitorServiceStatus, mPrevAssemblyMonitorServiceStatus, TmpAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mMachine As Machine
    Private Flag As Int16
    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
    Private mEnFrom As From
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 12th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 12th-Oct-2009
    Dim EventLogID As Guid      'Added By Utkarsh On 1-Aug-2011 For All19072011
    Dim MachineDetail As String 'Added By Utkarsh On 1-Aug-2011 For All19072011
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'Added By Vikrant For MPD
    Dim IsOpenFromMPD As String = "False"
    Dim RegNo As String = String.Empty
    'End
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public PeriodValues(,) As String
    Public IDsArrayStr As String
    Public mModelMonitorServiceList As ModelMonitorServiceList
#End Region

#Region " Enum "
    Public Enum From
        FromMaster = 0
        FromEntries = 1
    End Enum
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorServiceStatus = CType(Session("mAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        TmpAssemblyMonitorServiceStatus = CType(Session("TmpAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        mAssemblyMonitorServiceStatusList = CType(Session("mAssemblyMonitorServiceStatusList"), tmpAssemblyMonitorServiceStatusList)
        mMachine = CType(Session("mMachine"), Machine)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 12th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 12th-Oct-2009
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'Added By Vikrant For MPD
        IsOpenFromMPD = Session("IsOpenFromMPD")
        RegNo = Session("RegNo")
        'End
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mPrevAssemblyMonitorServiceStatus = Session("mPrevAssemblyMonitorServiceStatus")
        IDsArrayStr = Session("IDsArrayStr")
        mModelMonitorServiceList = Session("mModelMonitorServiceList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorServiceStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 12th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 12th-Oct-2009
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
        Session.Remove("Edit")
        Session.Remove("mModelMonitorServiceList")
    End Sub
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            With mAssemblyStatus.AssemblyStatusPeriods
                If .Item(j).PeriodID = 2 And txtDoneOnDate.Text <> "" Then
                    txtDnOnDate.Enabled = False
                Else
                    txtDnOnDate.Enabled = True
                End If
            End With
        Next j
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord(ByVal LogID As Guid, ByVal LogDate As String, ByVal ModelMonitorServiceID As Guid)
        Dim mAssemblyStatusList As AssemblyStatusList
        Dim mMachineList As MachineList
        Dim LatestRemovedOn As SmartDate
        Dim AssemblyStatusID As Guid = Guid.Empty

        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mAssemblyStatus.MachineID.ToString _
        , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , ,
        , , SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList

        If mAssemblyStatusList.Count = 0 Then
            mMachineList = MachineList.GetMachineListWithRemoval(LogDate, Guid.Empty.ToString _
                   , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , ,
                   , SkipIsForInventoryAircarft:=True)
            For i As Integer = 0 To mMachineList.Count - 1
                If mMachineList(i).AssemblyStatusList.Count > 0 Then

                    Dim mtempAssemblyList = (From AssemblyStatusInfo As AssemblyStatusInfo In mMachineList(i).AssemblyStatusList
                                             Order By CDate(AssemblyStatusInfo.RemovedOn) Descending
                                             Select AssemblyStatusInfo).ToList
                    Session("AssemblyStatusPeriodList") = mMachineList(i).AssemblyStatusList(mMachineList(i).AssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
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
            Session("AssemblyStatusPeriodList") = mAssemblyStatusList(mAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
        End If
        'End

        'Here instead of mPrevAssemblyMonitorServiceStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType)

        mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = ModelMonitorServiceID
        If LogDate <> "" Then
            mAssemblyMonitorServiceStatus.DoneOn = LogDate
        End If

        mAssemblyMonitorServiceStatus.BeginEdit()
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean, ByVal AssemblyMonitorServiceStatusID As Guid)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(AssemblyMonitorServiceStatusID, mAssemblyStatus.ID, mAssemblyStatus.HourType, IIf(Session("IsOpenFromMPD") = "True", True, False), DoneOnDate)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
    End Sub
    Private Sub SetFromClone(ByVal clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus)
        mAssemblyMonitorServiceStatus.DoneWONo = clnAssemblyMonitorServiceStatus.DoneWONo
        mAssemblyMonitorServiceStatus.DoneRemark = clnAssemblyMonitorServiceStatus.DoneRemark
        mAssemblyMonitorServiceStatus.DoneByID = clnAssemblyMonitorServiceStatus.DoneByID
        mAssemblyMonitorServiceStatus.LicenseNo = clnAssemblyMonitorServiceStatus.LicenseNo
        mAssemblyMonitorServiceStatus.Place = clnAssemblyMonitorServiceStatus.Place
        'Added By Vikrant On 25-Nov-2014
        mAssemblyMonitorServiceStatus.IsAttachmentAdded = clnAssemblyMonitorServiceStatus.IsAttachmentAdded
        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees
            If Session("From") = 0 Then 'New Record
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            ElseIf Session("From") = 1 Then 'Edit Record
                If Not mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                End If
            End If
        Next
        'End
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAssemblyMonitorServiceStatus.ID
            Session("mFileAttach") = mFileAttach
        End If
        'End
        clnAssemblyMonitorServiceStatus = Nothing
    End Sub
    Public Sub SetGridObjectFromObject()
        Dim j As Int32
        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        ReDim PeriodValues(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)
        'End
        For j = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1

            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                        .Item(j).DoneOnValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted)
                        '*********************************************************
                        '.Item(j).DoneOnValueFormatted = Trim(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted)
                        .Item(j).DoneOnValueFormatted = txtDoneOnDate.Text
                        '*********************************************************
                        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID  'To Check same Period
                        PeriodValues(j, 1) = Trim(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                        'End
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted)
                    .Item(j).DoneOnValue = Trim(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted)
                    'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID 'To Check same Period
                    PeriodValues(j, 1) = Trim(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                    'End
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted)
            End With
        Next j
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
    End Sub
    Private Sub SetObject(ByVal TmpAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus)
        With TmpAssemblyMonitorServiceStatus
            If txtDoneOnDate.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = txtDoneOnDate.Text
            End If
            .DoneWONo = txtWorkOrderNo.Text
            .DoneRemark = txtRemark.Text
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
            .IsApplicable = chkApplicable.Checked
            .IsLater = chkIsLater.Checked          'Added By Prashant 17-Aug-2010
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
            End If
        End With
        Session("TmpAssemblyMonitorServiceStatus") = TmpAssemblyMonitorServiceStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtDnDate, txtExtensionValue As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            If Not TmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, "") Is Nothing Then
                txtDnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
                txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
                With TmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                    If .Item(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, "").PeriodID = 2 Then
                        If Not Period.IsDate(txtDnDate.Text.Trim) Then
                            .Item(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, "").DoneOnValue = ""
                        Else
                            .Item(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, "").DoneOnValueFormatted = Trim(txtDnDate.Text)
                        End If
                    Else
                        .Item(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, "").DoneOnValue = Trim(txtDnDate.Text)

                    End If
                    .Item(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, "").ExtensionValue = Trim(txtExtensionValue.Text)
                End With
            End If

        Next j
        Session("TmpAssemblyMonitorServiceStatus") = TmpAssemblyMonitorServiceStatus
    End Sub
    Private Sub Save()
        Dim IDsArray = IDsArrayStr.Split(",")
        Dim str As String
        For Each ID As String In IDsArray
            TmpAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyMonitorServiceStatus.AssemblyID, mAssemblyMonitorServiceStatus.AssemblyStatusID, IIf(txtDoneOnDate.Text.Trim = "", Today.Date.ToString, txtDoneOnDate.Text.Trim), mAssemblyMonitorServiceStatus.ModelMonitorService.ModelID, mAssemblyStatus.HourType)
            TmpAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = New Guid(ID)

            Dim AssemblyMonitorServiceStatusClone As AssemblyMonitorServiceStatus
            AssemblyMonitorServiceStatusClone = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)
            SetObject(TmpAssemblyMonitorServiceStatus)
            SetGridObject()
            SetMachineMaintenanceObject() 'Added by Saylee on 12th-Oct-2009
            If TmpAssemblyMonitorServiceStatus.IsValid = True Then
                If TmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
                    '''Add MPD  that are not getting configured
                    '''MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Assembly Service.Assembly Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                    '''Return False
                    '''Exit Function
                End If
                'aded By Deven on 24-Sep-2009 ------
                '''If Not Session("IsOpenFromMPD") = "True" Then 'Condition Added By Vikrant For MPD
                '''    If mAssemblyMonitorServiceStatusList.Contains(mAssemblyMonitorServiceStatus.ModelMonitorServiceID) And mAssemblyMonitorServiceStatus.IsNew = True Then
                '''        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Assembly Service Status.", MsgBoxStyle.OkOnly, "")
                '''        Return False
                '''        Exit Function
                '''    End If
                '''End If

                Try
                    'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                    '''If Not mAssemblyMonitorServiceStatus.DoneByID.Equals(Guid.Empty) AndAlso mAssemblyMonitorServiceStatus.DoneOn.ToString.Length > 0 Then
                    '''    Dim Title As String = "Save Alert !"
                    '''    Dim Message As String = ""
                    '''    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorServiceStatus.DoneByID.ToString, mAssemblyMonitorServiceStatus.DoneOn.ToString)
                    '''    If mEmployeeStatus(0).Information <> "" Then
                    '''        Message = mEmployeeStatus(0).Information
                    '''        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
                    '''        Return False
                    '''    End If
                    '''End If
                    'End
                    TmpAssemblyMonitorServiceStatus.ApplyEdit()
                    TmpAssemblyMonitorServiceStatus = CType(TmpAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
                    SaveAttachment()
                    SaveMachineMaintenance()  'Added by Saylee on 12th-Oct-2009
                    '''Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                    ControlVisibilityForAttachment()

                Catch ex As SqlException
                    Session("AssemblyMonitorServiceStatusClone") = AssemblyMonitorServiceStatusClone
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If

                Finally
                    AssemblyMonitorServiceStatusClone = Nothing
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    'MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & " Monitor Type : " & mAssemblyMonitorServiceStatus.ModelMonitorService.ModelMonitorServiceTypeName
                    MachineDetail = "Reg No. : " & RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & " Monitor Type : " & mAssemblyMonitorServiceStatus.ModelMonitorService.ModelMonitorServiceTypeName
                    MarkLog(Util.Action.Save, "Assembly Service Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
                    'End
                End Try
            Else
                '''Add MPD  that are not getting configured
                For i As Integer = 0 To TmpAssemblyMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                    str = str + TmpAssemblyMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
                If str = "" Then
                    For i As Integer = 0 To TmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
                        For j As Integer = 0 To TmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                            str = str + TmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesString.ToString + "<BR>"
                        Next
                    Next
                End If
                If str <> "" Then
                    cvRemark.ErrorMessage = str
                    cvRemark.IsValid = False
                    upnlValidationSummary.Update()
                    Exit Sub
                End If
            End If
        Next
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mAssemblyMonitorServiceStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID)
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
                If (Not TmpAssemblyMonitorServiceStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, TmpAssemblyMonitorServiceStatus.ID)
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
    Private Sub SetTitle()
        Dim AssemblyInfo As String = "[Model: " & mAssemblyStatus.ModelName & " SerialNo: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        lblTitle.Text = "MPD Configuration For " & AssemblyInfo
        lblAssemblyValues.InnerText = mAssemblyStatus.AssemblyTypeName & " Values"
        upnlTitle.Update()
    End Sub
    Private Sub UpdatePanel()
        upnlMonitoringStatusDetails.Update()
        '''upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
        upnlDocumentDetails.Update()
        upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        '''upnlSelectMonitoringService.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "SaveSuccess" Then
                        RemoveSession()
                        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                    End If
            End Select
        End If
    End Sub
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 12th-Oct-2009
        If Session("From") = 0 And Not (mMachineMaintenanceList.Contains(TmpAssemblyMonitorServiceStatus.ID, 6, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 6, txtDoneOnDate.Text, TmpAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(TmpAssemblyMonitorServiceStatus.ID, 6)
        End If
        With mMachineMaintenance
            .MaintenanceID = TmpAssemblyMonitorServiceStatus.ID 'TransactionID
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
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtDoneOnDate.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With
        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub SaveMachineMaintenance() 'Added by Saylee on 12th-Oct-2009
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub SetRights() 'Added By Utkarsh On 15-Mar-2011
        If mAssemblyStatus.IsMaster Then
            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf (Not mAssemblyStatus.IsMaster) Then
            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    Private Sub SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
        '''If Not mAssemblyMonitorServiceStatus Is Nothing Then
        '''    If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is System.DBNull.Value Then
        '''        Dim txtdueOnValue As TextBox
        '''        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
        '''            txtdueOnValue = CType(dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
        '''            txtdueOnValue.BackColor = System.Drawing.Color.Red
        '''            txtdueOnValue.ForeColor = System.Drawing.Color.White
        '''        Next
        '''        lblRed.Visible = True
        '''        lblInfo.Visible = True
        '''    Else
        '''        lblRed.Visible = False
        '''        lblInfo.Visible = False
        '''    End If
        '''End If
    End Sub
    'MLNo
    Public Sub SetLicenceCount()
        '''If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 1 Then
        '''    lblLicenceCount.Text = "and " + (mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        '''End If
        '''lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        '''If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
        '''    txtLicenceNo.Text = mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        '''Else
        '''    txtLicenceNo.Text = String.Empty
        '''End If
    End Sub
    'End
#End Region

#Region " DataBinding "
    Private Sub SetGrid()
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            Dim txtDnOnDate As TextBox
            For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
                txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
                If CInt(dgDoneOnValue.DataKeys(j).Values(0).ToString) = 2 Then
                    txtDnOnDate.Text = txtDoneOnDate.Text
                End If
            Next j
        Next
    End Sub
    Private Sub DataFieldBind()
        Dim ModMonitorServiceIDs As New stringbuilder
        Dim IDsArray = IDsArrayStr.Split(",")
        For Each ID As String In IDsArray
            ModMonitorServiceIDs.Append("<ModMonServiceID>")
            ModMonitorServiceIDs.Append("<id>")
            ModMonitorServiceIDs.Append(New Guid(ID))
            ModMonitorServiceIDs.Append("</id>")
            ModMonitorServiceIDs.Append("</ModMonServiceID>")
        Next
        mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(ModelMonitorServiceIDs:=ModMonitorServiceIDs.ToString, ModelID:=mAssemblyMonitorServiceStatus.ModelMonitorService.ModelID)
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        '''
        'dgDoneOnValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
        dgDoneOnValue.DataSource = CType(Session("AssemblyStatusPeriodList"), AssemblyStatusPeriodList)
        '''
        'Added on 28-05-2007 by Saylee
        '''txtDoneOnDate.Text = mAssemblyMonitorServiceStatus.DoneOnFormatted.ToString
        '''txtExtensionDate.Text = mAssemblyMonitorServiceStatus.ExtensionDateFormatted.ToString
        'Added by Saylee on 12th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        '''If mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours + ")"
        BindLicenceNo() 'MLNo
        dgNonConfigList.DataSource = mModelMonitorServiceList
        DataBind()
        ControlVisibilityForDatePeriod()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        '''dgCurrentValue.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        '''dgCurrentValue.DataBind()
        '''dgDoneOnValue.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        'dgDoneOnValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
        dgDoneOnValue.DataSource = CType(Session("AssemblyStatusPeriodList"), AssemblyStatusPeriodList)
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
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

    End Sub
    Public Function CustomValidate2() As Boolean

        Dim str As String = ""
        Dim txtDnOnDate As TextBox

        For j As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            Dim mDoneOnValue As New Period(CInt(dgDoneOnValue.DataKeys(j).Values(0).ToString), DBNull.Value, IsDate:=True)
            txtDnOnDate = CType(dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            mDoneOnValue.Value = Trim(txtDnOnDate.Text)

            If Trim(txtDnOnDate.Text) = "" Then
                If CInt(dgDoneOnValue.DataKeys(j).Values(0).ToString) = 2 Then
                    If txtDoneOnDate.Text = "" Then
                        str = str + "Please enter " + dgDoneOnValue.Rows(j).Cells(1).Text + " Value" + "<BR>"
                    End If
                Else
                    str = str + "Please enter " + dgDoneOnValue.Rows(j).Cells(1).Text + " Value" + "<BR>"
                End If

            Else
                If Not mDoneOnValue.IsValid Then
                    str = str + dgDoneOnValue.Rows(j).Cells(1).Text + " " + mDoneOnValue.ErrMsg + "<BR>"
                End If
            End If

        Next j

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
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 1-Aug-2011 For All19072011
        If Not IsPostBack Then
            '''If btnSelectMonitoringService.Enabled = True Then
            '''    setFocus(btnSelectMonitoringService)
            '''End If
            DataFieldBind()
            ControlVisibilityForAttachment()
            SetRights()  'Added By Utkarsh On 15-Mar-2011
            SetTitle()
            SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If
    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '''Dim txtElapsedValue As TextBox
        '''For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
        '''    txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)

        '''    With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        '''        .Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
        '''    End With
        '''Next
        '''DataBindGrid()
        '''upnlCurrentValue.Update()
        '''upnlDoneOnValue.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            Save()
            If TmpAssemblyMonitorServiceStatus.IsValid Then
                SetTitle()
                UpdatePanel()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "SaveSuccess")
            End If

        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        If IsPostBack Then     'Added Code
            If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyMonitorServiceStatus.DoneOn.ToString), SmartDate.StringToDate(txtDoneOnDate.Text)) <> 0 Then
                If IsOpenFromMPD = "True" Then 'Added For MPD
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

                    Dim clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = mAssemblyMonitorServiceStatus.Clone
                    NewRecord(Guid.Empty, AsOnDate, New Guid(IDsArrayStr.Split(",")(0)))
                    SetFromClone(clnAssemblyMonitorServiceStatus)
                    SetGridObjectFromObject()
                Else
                    SetObject(mAssemblyMonitorServiceStatus)
                End If
                DataBindGrid()
                SetGrid()
                upnlRedLabel.Update()
                '''upnlCurrentValue.Update()
                upnlDoneOnValue.Update()
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not mAssemblyMonitorServiceStatus.IsNew Then 'Changed By Utkarsh On 1-Aug-2011 For All19072011
            'MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & " Monitor Type : " & mAssemblyMonitorServiceStatus.ModelMonitorService.ModelMonitorServiceTypeName
            MachineDetail = "Reg No. : " & RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & " Monitor Type : " & mAssemblyMonitorServiceStatus.ModelMonitorService.ModelMonitorServiceTypeName
            MarkLog(Util.Action.Close, "Assembly Service Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
        Else
            MarkLog(Util.Action.Close, "Assembly Service Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If 'End
        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mAssemblyMonitorServiceStatus.IsAttachmentAdded = True
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
        mAssemblyMonitorServiceStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorServiceStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorServiceStatus.ID)
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
            SetObject(mAssemblyMonitorServiceStatus)
            Session("mMaintenanceID") = mAssemblyMonitorServiceStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyMonitorServiceStatus.DoneOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
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
            If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatus.ID, 6, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If
        Else
            If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
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