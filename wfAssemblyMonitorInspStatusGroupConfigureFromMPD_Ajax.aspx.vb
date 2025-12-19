Imports System.Linq
Imports System.Text
Public Class wfAssemblyMonitorInspStatusGroupConfigureFromMPD_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyMonitorInspStatus, mPrevAssemblyMonitorInspStatus, TmpAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mMachine As Machine
    Private Flag As Int16
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
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
    Public mModelMonitorInspList As ModelMonitorInspList
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
        mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        TmpAssemblyMonitorInspStatus = CType(Session("TmpAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
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
        mPrevAssemblyMonitorInspStatus = Session("mPrevAssemblyMonitorInspStatus")
        IDsArrayStr = Session("IDsArrayStr")
        mModelMonitorInspList = Session("mModelMonitorInspList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorInspStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 12th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 12th-Oct-2009
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
        Session.Remove("Edit")
        Session.Remove("mModelMonitorInspList")
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
    Private Sub NewRecord(ByVal LogID As Guid, ByVal LogDate As String, ByVal ModelMonitorInspID As Guid)
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

        'Here instead of mPrevAssemblyMonitorInspStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType)

        mAssemblyMonitorInspStatus.ModelMonitorInspID(False) = ModelMonitorInspID
        If LogDate <> "" Then
            mAssemblyMonitorInspStatus.DoneOn = LogDate
        End If

        mAssemblyMonitorInspStatus.BeginEdit()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean, ByVal AssemblyMonitorInspStatusID As Guid)
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(AssemblyMonitorInspStatusID, mAssemblyStatus.ID, mAssemblyStatus.HourType, IIf(Session("IsOpenFromMPD") = "True", True, False), DoneOnDate)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
    End Sub
    Private Sub SetFromClone(ByVal clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus)
        mAssemblyMonitorInspStatus.DoneWONo = clnAssemblyMonitorInspStatus.DoneWONo
        mAssemblyMonitorInspStatus.DoneRemark = clnAssemblyMonitorInspStatus.DoneRemark
        mAssemblyMonitorInspStatus.DoneByID = clnAssemblyMonitorInspStatus.DoneByID
        mAssemblyMonitorInspStatus.LicenseNo = clnAssemblyMonitorInspStatus.LicenseNo
        mAssemblyMonitorInspStatus.Place = clnAssemblyMonitorInspStatus.Place
        'Added By Vikrant On 25-Nov-2014
        mAssemblyMonitorInspStatus.IsAttachmentAdded = clnAssemblyMonitorInspStatus.IsAttachmentAdded
        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyMonitorInspStatus.MaintenanceDoneByEmployees
            If Session("From") = 0 Then 'New Record
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            ElseIf Session("From") = 1 Then 'Edit Record
                If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                End If
            End If
        Next
        'End
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAssemblyMonitorInspStatus.ID
            Session("mFileAttach") = mFileAttach
        End If
        'End
        clnAssemblyMonitorInspStatus = Nothing
    End Sub
    Public Sub SetGridObjectFromObject()
        Dim j As Int32
        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        ReDim PeriodValues(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)
        'End
        For j = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1

            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                        .Item(j).DoneOnValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted)
                        '*********************************************************
                        '.Item(j).DoneOnValueFormatted = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted)
                        .Item(j).DoneOnValueFormatted = txtDoneOnDate.Text
                        '*********************************************************
                        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID  'To Check same Period
                        PeriodValues(j, 1) = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                        'End
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted)
                    .Item(j).DoneOnValue = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted)
                    'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID 'To Check same Period
                    PeriodValues(j, 1) = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                    'End
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted)
            End With
        Next j
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
    End Sub
    Private Sub SetObject(ByVal TmpAssemblyMonitorInspStatus As AssemblyMonitorInspStatus)
        With TmpAssemblyMonitorInspStatus
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
        Session("TmpAssemblyMonitorInspStatus") = TmpAssemblyMonitorInspStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtDnDate, txtExtensionValue As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            If Not TmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, "") Is Nothing Then
                txtDnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
                txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
                With TmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
        Session("TmpAssemblyMonitorInspStatus") = TmpAssemblyMonitorInspStatus
    End Sub
    Private Sub Save()
        Dim IDsArray = IDsArrayStr.Split(",")
        Dim str As String
        For Each ID As String In IDsArray
            TmpAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyMonitorInspStatus.AssemblyID, mAssemblyMonitorInspStatus.AssemblyStatusID, IIf(txtDoneOnDate.Text.Trim = "", Today.Date.ToString, txtDoneOnDate.Text.Trim), mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, mAssemblyStatus.HourType)
            TmpAssemblyMonitorInspStatus.ModelMonitorInspID(False) = New Guid(ID)

            Dim AssemblyMonitorInspStatusClone As AssemblyMonitorInspStatus
            AssemblyMonitorInspStatusClone = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)
            SetObject(TmpAssemblyMonitorInspStatus)
            SetGridObject()
            SetMachineMaintenanceObject() 'Added by Saylee on 12th-Oct-2009
            If TmpAssemblyMonitorInspStatus.IsValid = True Then
                If TmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
                    '''Add MPD  that are not getting configured
                    '''MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Assembly Inspection.Assembly Inspection Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                    '''Return False
                    '''Exit Function
                End If
                'aded By Deven on 24-Sep-2009 ------
                '''If Not Session("IsOpenFromMPD") = "True" Then 'Condition Added By Vikrant For MPD
                '''    If mAssemblyMonitorInspStatusList.Contains(mAssemblyMonitorInspStatus.ModelMonitorInspID) And mAssemblyMonitorInspStatus.IsNew = True Then
                '''        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Assembly Inspection Status.", MsgBoxStyle.OkOnly, "")
                '''        Return False
                '''        Exit Function
                '''    End If
                '''End If

                Try
                    'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                    '''If Not mAssemblyMonitorInspStatus.DoneByID.Equals(Guid.Empty) AndAlso mAssemblyMonitorInspStatus.DoneOn.ToString.Length > 0 Then
                    '''    Dim Title As String = "Save Alert !"
                    '''    Dim Message As String = ""
                    '''    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorInspStatus.DoneByID.ToString, mAssemblyMonitorInspStatus.DoneOn.ToString)
                    '''    If mEmployeeStatus(0).Information <> "" Then
                    '''        Message = mEmployeeStatus(0).Information
                    '''        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
                    '''        Return False
                    '''    End If
                    '''End If
                    'End
                    TmpAssemblyMonitorInspStatus.ApplyEdit()
                    TmpAssemblyMonitorInspStatus = CType(TmpAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
                    SaveAttachment()
                    SaveMachineMaintenance()  'Added by Saylee on 12th-Oct-2009
                    '''Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                    ControlVisibilityForAttachment()

                Catch ex As SqlException
                    Session("AssemblyMonitorInspStatusClone") = AssemblyMonitorInspStatusClone
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If

                Finally
                    AssemblyMonitorInspStatusClone = Nothing
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    'MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & " Monitor Type : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelMonitorInspTypeName
                    MachineDetail = "Reg No. : " & RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & " Monitor Type : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelMonitorInspTypeName
                    MarkLog(Util.Action.Save, "Assembly Inspection Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
                    'End
                End Try
            Else
                '''Add MPD  that are not getting configured
                For i As Integer = 0 To TmpAssemblyMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                    str = str + TmpAssemblyMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
                If str = "" Then
                    For i As Integer = 0 To TmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
                        For j As Integer = 0 To TmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                            str = str + TmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesString.ToString + "<BR>"
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
        If mAssemblyMonitorInspStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
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
                If (Not TmpAssemblyMonitorInspStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, TmpAssemblyMonitorInspStatus.ID)
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
        If Session("From") = 0 And Not (mMachineMaintenanceList.Contains(TmpAssemblyMonitorInspStatus.ID, 6, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 6, txtDoneOnDate.Text, TmpAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(TmpAssemblyMonitorInspStatus.ID, 6)
        End If
        With mMachineMaintenance
            .MaintenanceID = TmpAssemblyMonitorInspStatus.ID 'TransactionID
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
            If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf (Not mAssemblyStatus.IsMaster) Then
            If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    Private Sub SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
        '''If Not mAssemblyMonitorInspStatus Is Nothing Then
        '''    If mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And Not mAssemblyMonitorInspStatus.DoneOn Is System.DBNull.Value Then
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
        '''If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 Then
        '''    lblLicenceCount.Text = "and " + (mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        '''End If
        '''lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        '''If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
        '''    txtLicenceNo.Text = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
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
        Dim ModMonitorInspIDs As New stringbuilder
        Dim IDsArray = IDsArrayStr.Split(",")
        For Each ID As String In IDsArray
            ModMonitorInspIDs.Append("<ModMonInspID>")
            ModMonitorInspIDs.Append("<id>")
            ModMonitorInspIDs.Append(New Guid(ID))
            ModMonitorInspIDs.Append("</id>")
            ModMonitorInspIDs.Append("</ModMonInspID>")
        Next
        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(ModelMonitorInspIDs:=ModMonitorInspIDs.ToString, ModelID:=mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID)
        Session("mModelMonitorInspList") = mModelMonitorInspList
        '''
        'dgDoneOnValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
        dgDoneOnValue.DataSource = CType(Session("AssemblyStatusPeriodList"), AssemblyStatusPeriodList)
        '''
        'Added on 28-05-2007 by Saylee
        '''txtDoneOnDate.Text = mAssemblyMonitorInspStatus.DoneOnFormatted.ToString
        '''txtExtensionDate.Text = mAssemblyMonitorInspStatus.ExtensionDateFormatted.ToString
        'Added by Saylee on 12th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        '''If mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours + ")"
        BindLicenceNo() 'MLNo
        dgNonConfigList.DataSource = mModelMonitorInspList
        DataBind()
        ControlVisibilityForDatePeriod()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        '''dgCurrentValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
        '''dgCurrentValue.DataBind()
        '''dgDoneOnValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
            '''If btnSelectMonitoringInspection.Enabled = True Then
            '''    setFocus(btnSelectMonitoringInspection)
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
        '''For i As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
        '''    txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)

        '''    With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
            If TmpAssemblyMonitorInspStatus.IsValid Then
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
            If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyMonitorInspStatus.DoneOn.ToString), SmartDate.StringToDate(txtDoneOnDate.Text)) <> 0 Then
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

                    Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = mAssemblyMonitorInspStatus.Clone
                    NewRecord(Guid.Empty, AsOnDate, New Guid(IDsArrayStr.Split(",")(0)))
                    SetFromClone(clnAssemblyMonitorInspStatus)
                    SetGridObjectFromObject()
                Else
                    SetObject(mAssemblyMonitorInspStatus)
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
        If Not mAssemblyMonitorInspStatus.IsNew Then 'Changed By Utkarsh On 1-Aug-2011 For All19072011
            'MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & " Monitor Type : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelMonitorInspTypeName
            MachineDetail = "Reg No. : " & RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & " Monitor Type : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelMonitorInspTypeName
            MarkLog(Util.Action.Close, "Assembly Inspection Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
        Else
            MarkLog(Util.Action.Close, "Assembly Inspection Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If 'End
        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mAssemblyMonitorInspStatus.IsAttachmentAdded = True
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
        mAssemblyMonitorInspStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorInspStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorInspStatus.ID)
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
            SetObject(mAssemblyMonitorInspStatus)
            Session("mMaintenanceID") = mAssemblyMonitorInspStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyMonitorInspStatus.DoneOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
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
            If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, 6, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If
        Else
            If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
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