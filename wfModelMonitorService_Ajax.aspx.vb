'Modified by Harsh on 27th May, 5th Jun 2024 for FLYPAL-1659, FLYPAL-1685

Imports System.Text

Public Class wfModelMonitorService_Ajax
    Inherits Page

#Region " Variable Declaration "
    Public mModelMonitorService As ModelMonitorService
    Public mATAList As ATAList
    Public mModelMonitorServicePeriodUnitList As ModelMonitorServicePeriodUnitList
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mModelMonitorServiceTypeList As ModelMonitorServiceTypeList
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Dim Flag As Int16
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
    Dim EventLogID As Guid 'Added by Vikrant on 27-July-2011
    Public mUnit As String
    Public mModel As String
    Public mMonitorType As String
    Public mDescrition As String
    Public mDetail As String
    Public mLinkMaintenanceActionList As LinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance 'End
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 07-Jul-2020 for Add EMailIDs field in csTransType 

    Dim mMPDTypeList As MPDTypeList 'Added by Saylee on 19-Apr-2023
    Dim mMPDSkillList As MPDSkillList 'Added by Saylee on 19-Apr-2023

    Dim mLastMPDRef As LastMPDAMPRef 'Added by Ajay on 20-07-2023

    Dim mPreviousAssemblyMonitorServiceStatusForRevise As AssemblyMonitorServiceStatus  'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity

#End Region

#Region " Business Methdods "
    Private Sub GetSession()
        mAssemblyMonitorServiceStatus = CType(Session("mAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        mModelMonitorService = CType(Session("mModelMonitorService"), ModelMonitorService)
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorServiceTypeList = CType(Session("mModelMonitorServiceTypeList"), ModelMonitorServiceTypeList)
        mModelMonitorServicePeriodUnitList = CType(Session("mModelMonitorServicePeriodUnitList"), ModelMonitorServicePeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mAssemblyMonitorServiceStatusList = CType(Session("mAssemblyMonitorServiceStatusList"), tmpAssemblyMonitorServiceStatusList)
        mLinkMaintenanceActionList = Session("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        mLinkMaintenanceList = Session("mLinkMaintenanceList") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mModuleList = Session("mModuleList") 'Added by shital on 07-Jul-2020 for Add EMailIDs field in csTransType
        mLastMPDRef = Session("mLastMPDRef")
        mPreviousAssemblyMonitorServiceStatusForRevise = Session("PreviousAssemblyMonitorServiceStatusForRevise") 'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
    End Sub
    Private Sub SetSession()
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mModelMonitorService") = mModelMonitorService
        Session("mATAList") = mATAList
        Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList
        Session("mModelMonitorServicePeriodUnitList") = mModelMonitorServicePeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End

        Session("mLastMPDRef") = mLastMPDRef

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorServiceTypeList")
        Session.Remove("mSelectPeriodUnits")
        Session.Remove("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session.Remove("mLinkMaintenanceList")
        Session.Remove("URL")
        Session.Remove("MaintenanceActivityID") 'End
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")

        Session.Remove("mLastMPDRef")
    End Sub
    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()
        'mModelMonitorService.Code = Trim(txtCode.Text)
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
                mModelMonitorService.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
            Else
                mModelMonitorService.Code = Trim(txtCode.Text)
            End If


        Else
            mModelMonitorService.Code = Trim(txtCode.Text)

        End If

        mModelMonitorService.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mModelMonitorService.Reference = Trim(txtReference.Text)
        mModelMonitorService.Description = Trim(txtDescription.Text)
        mModelMonitorService.ModelMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue.ToString), Int32)
        mModelMonitorService.Note = Trim(txtNote.Text)
        mModelMonitorService.ShowInCofA = chkShowInCofA.Checked
        mModelMonitorService.RequiredManHours = txtRequiredManHours.Text.Trim
        mModelMonitorService.Zone = Trim(txtZone.Text) 'Added by Saylee on 23-July-2013 for BA22072013 
        mModelMonitorService.Area = Trim(txtArea.Text)
        mModelMonitorService.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorService.IsAttachmentAdded = True
            Else
                mModelMonitorService.IsAttachmentAdded = False
            End If
        End If

        'Added by Saylee on 19-Apr-2023
        mModelMonitorService.TaskCardNo = txtTaskCardNo.Text.Trim
        mModelMonitorService.TaskHeading = txtTaskCardHeader.Text.Trim
        mModelMonitorService.Applicability = txtApplicability.Text.Trim
        mModelMonitorService.Source = txtSource.Text.Trim
        mModelMonitorService.Access = txtAccess.Text.Trim
        mModelMonitorService.MPDSkillID = Val(cmbSkillcode.SelectedValue.ToString)
        mModelMonitorService.MPDTypeID = Val(cmbMPDType.SelectedValue.ToString)
        mModelMonitorService.AccessOpenCloseManHours = txtAccessManHours.Text.Trim
        ''********************

        Session("mModelMonitorService") = mModelMonitorService



    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorService.ModelMonitorServicePeriods
            For i As Integer = 0 To .Count - 1
                REM: Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                REM:Setting the Object with the Values of the Controls
                If .Item(i).PeriodID = 2 And Decimal.MaxValue <= Val(txtFrequencyValue.Text.Trim) Then    'Hours 
                    .Item(i).FrequencyValue = ""
                Else
                    .Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
                End If
            Next i
        End With
        Session("mModelMonitorService") = mModelMonitorService
    End Sub
    Private Sub SetCaption()

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD"
        Else
            ServiceMPDTitle = "Service"
        End If


        If mModelMonitorService.IsNew Then
            lblTitle.Text = "Model " + ServiceMPDTitle + " of [ Model : " & mModelMonitorService.Model.Name & " ] [New]"
        Else
            lblTitle.Text = "Model " + ServiceMPDTitle + " of [ Model : " & mModelMonitorService.Model.Name & " ]"
        End If
        'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        lblResult.Text = "List of Linked Maintenance Activity : " & mLinkMaintenanceList.Count & " Record(s) found."
        'Added By Saylee ON 4-Feb-2013 for BA04022013
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            lblReference.Text = "Task Source Reference"
        ElseIf AppSettings("ClientCode") = "Indamer" Then 'Added By Prashant 3-Apr-2013  'Indamer03042013
            lblReference.Text = "Task Code/Reference"
            txtReference.ToolTip = "Enter Task Code/Reference"
        Else
            lblReference.Text = "Reference Doc."
        End If
        'End
        upnlTitle.Update()
    End Sub
    Private Sub ControlVisibility()

        If Session("ModelIDFromModelCreation") = Nothing Then
            btnAddPeriodUnit.Enabled = mModelMonitorServicePeriodUnitList.Count > 0 'Session("ModelIDFromModelCreation") = Nothing,'Added by Saylee on 14-Nov-2019
        Else
            btnAddPeriodUnit.Enabled = True
        End If

        btnPrint.Enabled = Not mModelMonitorService.IsNew
        btnSendMail.Enabled = Not mModelMonitorService.IsNew 'Added by Shital on 02-Jul-2020 for sendmail functionality
        If AppSettings("LinkMaintenance") = True Then 'Added By Utkarsh On 27-Jun-2012
            If Not mLinkMaintenanceList Is Nothing Then
                dgLinkedMaintenanceList.Columns(7).Visible = mLinkMaintenanceList.ShowDirectiveNo
            End If
        End If 'End
        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorService.IsNew Then
            Dim mModelMonitorServiceConfiguredList As ModelMonitorConfiguredList = Session("mModelMonitorServiceConfiguredList")
            If Not mModelMonitorServiceConfiguredList Is Nothing Then
                If mModelMonitorServiceConfiguredList.Count > 0 Then
                    cmbMonitorServiceType.Enabled = False
                Else
                    cmbMonitorServiceType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mModelMonitorService.ModelMonitorServicePeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mModelMonitorServiceConfiguredList.Count > 0 Then
                            txtFrequencyValue.Enabled = False
                        Else
                            txtFrequencyValue.Enabled = True
                        End If

                    Next i
                End With
            End If

        End If
        If Not Session("OpenFromModelCreation") Is Nothing Then
            btnSaveSelect.Visible = False
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mModelMonitorService.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    'Added by Saylee on 17-Sep-2014 for ALL17092014
    'This new function is called in "Save" function
    Private Sub SaveLinkList()
        If dgLinkedMaintenanceList.Rows.Count > 0 Then
            SetLinkMaintenanceGridObject()
            Dim mLinkMaintenanceListClone As LinkMaintenanceList
            mLinkMaintenanceListClone = CType(mLinkMaintenanceList.Clone, LinkMaintenanceList)
            Try
                mLinkMaintenanceList = CType(mLinkMaintenanceList.Save, LinkMaintenanceList)
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If
                mLinkMaintenanceList = mLinkMaintenanceListClone
                Session("mLinkMaintenanceList") = mLinkMaintenanceList
            End Try
            dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
            dgLinkedMaintenanceList.DataBind()
            upnlLinkedMaintenanceList.Update()
            upnlLinkMaint.Update()
        End If
    End Sub
    Private Function Save() As Boolean

        If Not IsValid Then Exit Function

        SetObject()
        SetGridObject()

        Dim mModelMonitorServiceClone As ModelMonitorService
        mModelMonitorServiceClone = CType(mModelMonitorService, ModelMonitorService)

        If mModelMonitorService.IsValid = True Then

            Try

                Dim ServiceMPDTitle As String = ""

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Model Service"
                End If

                If mModelMonitorService.ModelMonitorServicePeriods.Count = 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired,
                                    MSGBox.Message_text.PeriodRequired,
                                    ServiceMPDTitle + " cannot be saved without Period units",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False

                End If

                'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                If Not Session("ModelMonitorServiceIDToBeLinked") Is Nothing And mModelMonitorService.IsNew Then

                    Dim OldModelMonitorService As ModelMonitorService
                    Dim mOldFreqValues As New StringBuilder
                    OldModelMonitorService = ModelMonitorService.GetModelMonitorService(mAssemblyMonitorServiceStatus.ModelMonitorServiceID)

                    For i As Integer = 0 To OldModelMonitorService.ModelMonitorServicePeriods.Count - 1

                        If OldModelMonitorService.ModelMonitorServicePeriods(i).PeriodID = 2 And
                           Decimal.MaxValue < Val(OldModelMonitorService.ModelMonitorServicePeriods(i).FrequencyValue) Then
                            'Do Nothing
                        Else

                            mOldFreqValues.Append(OldModelMonitorService.ModelMonitorServicePeriods(i).FrequencyValueFormatted.ToString + " " +
                                                  OldModelMonitorService.ModelMonitorServicePeriods(i).PeriodUnitName + ",")

                        End If

                    Next

                    OldModelMonitorService = ModelMonitorService.GetModelMonitorService(New Guid(Session("ModelMonitorServiceIDToBeLinked").
                                                                                                 ToString))

                    mModelMonitorService.PreviousRefID = New Guid(Session("ModelMonitorServicePreviousRefIDToBeLinked").ToString)
                    mModelMonitorService.ReviseRemark = "This is Revised Service with previous frequency : " +
                                                         mOldFreqValues.ToString.TrimEnd(CChar(",")) +
                                                         ", previous description : " + OldModelMonitorService.Description


                End If

                mModelMonitorService.ApplyEdit()
                mModelMonitorService = CType(mModelMonitorService.Save, ModelMonitorService)

                'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                Dim mMaintenanceKit As MaintenanceKit
                Dim mMaintenanceKitOld As MaintenanceKit
                Dim mMaintenanceTask,
                    mMaintenanceTaskOld As MaintenanceTask

                If mModelMonitorService.ReviseRemark <> "" Then

                    Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount
                    mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorService.ID)

                    If mMaintenanceKitDetailsCount.MaintenanceSparesCount = 0 And
                       mMaintenanceKitDetailsCount.MaintenanceTasksCount = 0 And
                       mMaintenanceKitDetailsCount.MaintenanceToolsCount = 0 Then

                        mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)

                        'Tools
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorService.PreviousRefID,
                                                                                      True)
                        mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID,
                                                                           mMaintenanceTaskAndKit.ID,
                                                                           mMaintenanceTaskAndKit.IsAssembly,
                                                                           True)

                        For i As Integer = 0 To mMaintenanceKitOld.MaintenanceKitDetails.Count - 1

                            mMaintenanceKit.MaintenanceKitDetails.Add(mMaintenanceKit.ID)
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = i + 1
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = mMaintenanceKitOld.MaintenanceKitDetails(i).ItemID
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = mMaintenanceKitOld.MaintenanceKitDetails(i).Qty
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Note = mMaintenanceKitOld.MaintenanceKitDetails(i).Note
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = mMaintenanceKitOld.MaintenanceKitDetails(i).Remark

                        Next

                        mMaintenanceKit.Save()
                        'End

                        'Spares
                        mMaintenanceKitOld = Nothing
                        mMaintenanceKit = Nothing
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorService.PreviousRefID,
                                                                                      False)
                        mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID,
                                                                           mMaintenanceTaskAndKit.ID,
                                                                           mMaintenanceTaskAndKit.IsAssembly,
                                                                           False)

                        For i As Integer = 0 To mMaintenanceKitOld.MaintenanceKitDetails.Count - 1

                            mMaintenanceKit.MaintenanceKitDetails.Add(mMaintenanceKit.ID)
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = i + 1
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = mMaintenanceKitOld.MaintenanceKitDetails(i).ItemID
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = mMaintenanceKitOld.MaintenanceKitDetails(i).Qty
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Note = mMaintenanceKitOld.MaintenanceKitDetails(i).Note
                            mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = mMaintenanceKitOld.MaintenanceKitDetails(i).Remark

                        Next

                        mMaintenanceKit.Save()
                        'End

                        'Tasks
                        mMaintenanceTaskOld = MaintenanceTask.GetMaintenanceTaskByParent(mModelMonitorService.PreviousRefID)
                        mMaintenanceTask = MaintenanceTask.NewMaintenanceTask(mMaintenanceTaskAndKit.MaintenanceTypeID,
                                                                              mMaintenanceTaskAndKit.ID,
                                                                              mMaintenanceTaskAndKit.IsAssembly)

                        For i As Integer = 0 To mMaintenanceTaskOld.MaintenanceTaskDetails.Count - 1

                            mMaintenanceTask.MaintenanceTaskDetails.Add(mMaintenanceTask.ID)
                            mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.SrNo = i + 1
                            mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task = mMaintenanceTaskOld.MaintenanceTaskDetails(i).Task
                            mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardNo = mMaintenanceTaskOld.MaintenanceTaskDetails(i).TaskCardNo
                            mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Note = mMaintenanceTaskOld.MaintenanceTaskDetails(i).Note
                            mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardID = mMaintenanceTaskOld.MaintenanceTaskDetails(i).TaskCardID

                        Next

                        mMaintenanceTask.Save()
                        'End

                    End If

                End If


                SaveAttachment()
                SaveLinkList()
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorService") = mModelMonitorService
                mModel = mModelMonitorService.Model.Name
                mMonitorType = cmbMonitorServiceType.SelectedItem.Text
                mDescrition = txtDescription.Text
                mDetail = "Model : " + mModel + " Monitor Type : " + mMonitorType + " Description : " + mDescrition

                MarkLog(Action:=Action.Save,
                        ModuleName:="Model Service",
                        Detail:=mDetail,
                        ErrorType:=ErrorType.NoError,
                        TransID:=mModelMonitorService.ID,
                        EventLogID)

                'End

                If cmbATAChapter.Enabled = True Then
                    setFocus(cmbATAChapter)
                End If

                Return True

            Catch ex As SqlException

                If ex.Number = 8145 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.ProcedureError,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.Duplicate,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 547 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.saveAlert,
                                    "This Entry is used by Some One.",
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

                mModelMonitorService = mModelMonitorServiceClone
                Session("mModelMonitorService") = mModelMonitorService

                Return False

            End Try

        Else
            Return False
        End If

    End Function
    Private Sub AddSelectedPeriodUnits()
        Dim clnModelMonitorService = mModelMonitorService.Clone
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Try
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit As SelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected = True Then
                    mModelMonitorService.ModelMonitorServicePeriods.Add(mModelMonitorService.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
                End If
            Next
            For i As Integer = 0 To mModelMonitorService.ModelMonitorServicePeriods.Count - 1
                mModelMonitorService.ModelMonitorServicePeriods(i).MonitorTypeID = mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID
                If mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID = 3 Then        'this is for No Frequency
                    mModelMonitorService.ModelMonitorServicePeriods(i).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorService") = mModelMonitorService
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mModelMonitorService = clnModelMonitorService
            Session("mModelMonitorService") = mModelMonitorService
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.Show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorService = Nothing
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        End Try
    End Sub
    Private Sub SetPeriodUnits()
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits()
        Dim i As Int32
        If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
            While i <= mModelMonitorServicePeriodUnitList.Count - 1
                If mModelMonitorService.ModelMonitorServicePeriods.Contains(mModelMonitorServicePeriodUnitList(i).ID) = False Then
                    mSelectPeriodUnits.Add(mModelMonitorServicePeriodUnitList(i).ID, mModelMonitorServicePeriodUnitList(i).PeriodID, mModelMonitorServicePeriodUnitList(i).Name)
                End If
                i = i + 1
            End While
        Else
            'Added by Saylee on 14-Nov-2019
            Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
            While i <= mPeriodUnitList.Count - 1
                If mModelMonitorService.ModelMonitorServicePeriods.Contains(mPeriodUnitList(i).ID) = False Then
                    mSelectPeriodUnits.Add(mPeriodUnitList(i).ID, mPeriodUnitList(i).PeriodID, mPeriodUnitList(i).PeriodUnitName)
                End If
                i = i + 1
            End While
            '*********************************
        End If
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub NewRecord()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************

        'Modified by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
        Dim ID As Guid = Guid.NewGuid
        mModelMonitorService = ModelMonitorService.NewModelMonitorService(ID:=ID,
                                                                          ModelID:=mModelMonitorService.ModelID,
                                                                          HourType:=mHourType,
                                                                          PreviousRefID:=ID)
        Session("mModelMonitorService") = mModelMonitorService.ModelMonitorServicePeriods
    End Sub
    Private Sub UpdatePanel()
        upnlMonitorServiceDetails.Update()
        upnlATAMaster.Update()
        upnlMonitorServiceType.Update()
        upnlPeriods.Update()
        upnlOtherDetails.DataBind()
        upnlOtherDetails.Update()
        upnlActionBtn.Update()
        upnlLinkMaint.DataBind()
        upnlLinkMaint.Update()
        upnlTitle.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Try
                            Save()
                            NewRecord()
                            SetCaption()
                            UpdatePanel()
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteLM" Then
                        Try
                            If Not mLinkMaintenanceList.Count = 1 Then
                                mLinkMaintenanceList.Remove(mLinkMaintenanceList.CurrentItem)
                            Else
                                If mLinkMaintenanceList.CurrentItem.IsNew Then
                                    mLinkMaintenanceList.Remove(mLinkMaintenanceList.CurrentItem)
                                Else
                                    mLinkMaintenanceList.Remove(mLinkMaintenanceList.CurrentItem)
                                    mLinkMaintenanceList.Save()
                                End If

                            End If
                            Session("mLinkMaintenanceList") = mLinkMaintenanceList
                            dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
                            dgLinkedMaintenanceList.DataBind()
                            lnkLinkMaint1.Text = "Click to add Link Maintenance Activity " + "(" + mLinkMaintenanceList.Count.ToString + " activity(s))"
                            lblResult.Text = "List Of Linked Maintenance Activity : " & mLinkMaintenanceList.Count & " Record(s) found." 'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
                            upnlLinkMaint.Update()
                            upnlLinkedMaintenanceList.Update()
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("Sender") = ""
                        NewRecord()
                        UpdatePanel()
                    End If
                    If MSGBoxCtrl.Sender = "DeleteLM" Then
                        UpdatePanel()
                        upnlLinkMaint.Update()
                        upnlLinkedMaintenanceList.Update()
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                    End If
            End Select
        End If
    End Sub
    Private Sub SetRights() 'Added By Utkarsh On 14-Mar-2011
        If mAssemblyStatus.IsMaster Then
            If (User.IsInRole("MachineAssemblyServicePrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
                btnSaveSelect.Enabled = False
                btnSaveSelect.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (User.IsInRole("MachineAssemblyServicePrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
                btnSaveSelect.Enabled = False
                btnSaveSelect.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub '*******************************
    Private Sub SetToolsSparesCount()

        'Modified by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount

        If mModelMonitorService.IsNew And mModelMonitorService.ReviseRemark <> "" Then
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorService.PreviousRefID)
        Else
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorService.ID)
        End If

        lnkTools.Text = "Tools (" + mMaintenanceKitDetailsCount.MaintenanceToolsCount.ToString + " record(s))"
        lnkSpares.Text = "Spares (" + mMaintenanceKitDetailsCount.MaintenanceSparesCount.ToString + " record(s))"
        lnkTaskCards.Text = "Task Cards (" + mMaintenanceKitDetailsCount.MaintenanceTasksCount.ToString + " record(s))"

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
                If (Not mModelMonitorService.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mModelMonitorService.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList(, "<SELECT>")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        If Session("ModelIDFromModelCreation") = Nothing Then 'added by saylee on 14-Nov-2019
            mModelMonitorServicePeriodUnitList = ModelMonitorServicePeriodUnitList.GetModelMonitorServicePeriodUnitList(mAssemblyMonitorServiceStatus.AssemblyStatusID)         'mModel.ID)
        ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
            ' mModelMonitorServicePeriodUnitList =  ModelMonitorServicePeriodUnitList.
        End If
        Session("mModelMonitorServicePeriodUnitList") = mModelMonitorServicePeriodUnitList
        cmbMonitorServiceType.DataSource = mModelMonitorServiceTypeList
        dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods


        mLinkMaintenanceActionList = LinkMaintenanceActionList.GetLinkMaintActionList(True) 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        If mLinkMaintenanceList Is Nothing Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorService.ID.ToString)
        End If
        dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
        lnkLinkMaint1.Text = "Click to add Link Maintenance Activity " + "(" + mLinkMaintenanceList.Count.ToString + " activity(s))"
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorService.IsNew Then
            Dim mModelMonitorServiceConfiguredList As ModelMonitorConfiguredList
            mModelMonitorServiceConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorService.ModelID, mModelMonitorService.ID.ToString)
            Session("mModelMonitorServiceConfiguredList") = mModelMonitorServiceConfiguredList
        End If

        mMPDTypeList = MPDTypeList.GetTypeList(True)
        cmbMPDType.DataSource = mMPDTypeList

        mMPDSkillList = MPDSkillList.GetSkillList(True)
        cmbSkillcode.DataSource = mMPDSkillList

        'Added by Ajay 21-01-2023
        mLastMPDRef = LastMPDAMPRef.GetLastMPDAMPRefForModel(mMachine.AssemblyStatus.Assembly.ModelID)
        Session("mLastMPDRef") = mLastMPDRef
        If (mLastMPDRef.MPDNo <> "") Then lblMPDNo.Text = "MPD No.: " + mLastMPDRef.MPDNo + ",Rev No.: " + mLastMPDRef.RevNo + ",Dated: " + mLastMPDRef.FromDateFormatted


        DataBind()


    End Sub
    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbATAChapter" Then
            ''''If cmbATAChapter.SelectedIndex <= 0 Then
            ''''    custValidator.ErrorMessage = "Please Select ATAChapter from the list."
            ''''    e.IsValid = False
            ''''Else
            ''''    e.IsValid = True
            ''''End If
        ElseIf custValidator.ControlToValidate = "cmbMonitorServiceType" Then
            '''If cmbMonitorServiceType.SelectedIndex <= 0 Then
            '''    custValidator.ErrorMessage = "Please Select Task Type from the list."
            '''    e.IsValid = False
            '''Else
            '''    e.IsValid = True
            '''End If
            'ElseIf custValidator.ControlToValidate = "txtDescription" Then
            '    If Len(txtDescription.Text) > 1000 Then
            '        custValidator.ErrorMessage = "Description can't be more than 1000 chars."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        ElseIf custValidator.ControlToValidate = "txtReference" Then
            If Len(txtReference.Text) > 500 Then
                custValidator.ErrorMessage = "Reference Too Long"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note can't be more than 1000 chars."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        End If
    End Sub
    Public Sub CustomValidate1(s As Object, e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim CustValidator As CustomValidator = CType(s, CustomValidator)
        Dim counter As Integer
        SetObject()

        SetGridObject()
        Dim str As String = ""
        If Not mModelMonitorService.IsValid Then
            For i As Integer = 0 To mModelMonitorService.GetBrokenRulesCollection.Count - 1
                str = str + mModelMonitorService.GetBrokenRulesCollection(i).Description + "<BR>"
            Next i
        End If
        For counter = 0 To dgPeriods.Rows.Count - 1
            If Not mModelMonitorService.ModelMonitorServicePeriods(counter).IsValid Then
                For i As Integer = 0 To mModelMonitorService.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorService.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            CustValidator.ErrorMessage = str
            e.IsValid = False
        Else
            e.IsValid = True
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        For counter As Integer = 0 To dgPeriods.Rows.Count - 1
            If Not mModelMonitorService.ModelMonitorServicePeriods(counter).IsValid Then
                For i As Integer = 0 To mModelMonitorService.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorService.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvATAChapter.ErrorMessage = str
            cvATAChapter.IsValid = False
            Return False
        Else
            cvATAChapter.IsValid = True
            Return True
        End If
    End Function
    Public Sub CustomValidate3(s As Object, e As ServerValidateEventArgs) 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Dim CustValidator As CustomValidator = CType(s, CustomValidator)
        Dim counter As Integer
        SetLinkMaintenanceGridObject()
        Dim str As String = ""
        For counter = 0 To dgLinkedMaintenanceList.Rows.Count - 1
            If Not mLinkMaintenanceList(counter).IsValid Then
                For i As Integer = 0 To mLinkMaintenanceList(counter).GetBrokenRulesCollection.Count - 1
                    str = str + mLinkMaintenanceList(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            CustValidator.ErrorMessage = str
            e.IsValid = False
        Else
            e.IsValid = True
        End If
    End Sub
    Private Sub SetLinkMaintenanceGridObject()
        Dim txtRemark As TextBox
        Dim cmbAction As DropDownList

        For i As Integer = 0 To dgLinkedMaintenanceList.Rows.Count - 1
            txtRemark = CType(dgLinkedMaintenanceList.Rows(i).FindControl("txtRemark"), TextBox)
            cmbAction = CType(dgLinkedMaintenanceList.Rows(i).FindControl("cmbLinkMaintActionlist"), DropDownList)
            mLinkMaintenanceList(i).Remark = txtRemark.Text.Trim
            mLinkMaintenanceList(i).MaintenanceActionID = cmbAction.SelectedValue
        Next
        Session("mLinkMaintenanceList") = mLinkMaintenanceList
    End Sub 'End
#End Region

#Region " Events "
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        REM: put here your code to initialize the page
        GetSession()

        'Added by Vikrant on 27-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            If txtCode.Enabled = True And AppSettings("ShowMaintenanceForNewClients") = "False" Then

                setFocus(txtCode)
                cmbMonitorType.Items.Add(New ListItem("Service", "1"))
                cmbMonitorType.Items.Add(New ListItem("Inspection", "2"))
                cmbMonitorType.Items.Add(New ListItem("Directive", "3"))

            Else
                setFocus(txtTaskCardNo)
                cmbMonitorType.Items.Add(New ListItem("MPD", "1"))
                cmbMonitorType.Items.Add(New ListItem("Directive", "3"))
            End If

            mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("<SELECT>")
            Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList
            AddSelectedPeriodUnits()
            DataFieldBind()
            ControlVisibility()
            SetCaption()

            If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019 
                SetRights()  'Added By Utkarsh On 14-Mar-2011
            End If

            SetToolsSparesCount()
            ControlVisibilityForAttachment()

        End If

        If AppSettings("ClientCode") = "Heligo" Then
            lblZone.InnerText = "System"
        End If

    End Sub
    Private Sub GVPeriodsRowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgPeriods.RowCommand

        Select Case e.CommandName

            Case "DeleteRec"

                Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize

                If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019

                    If mAssemblyStatus.IsMaster Then 'Added By Utkarsh On 15-Mar-2011

                        If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then

                            mUnit = mModelMonitorService.ModelMonitorServicePeriods(Index).PeriodUnitName
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub

                        End If

                    ElseIf Not mAssemblyStatus.IsMaster Then

                        If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If

                    End If '*******************************

                End If

                Try

                    'Modified by Harsh on 7th June 2024 for reducing the load time while revising a record
                    If mModelMonitorService.ReviseRemark <> "" OrElse mModelMonitorService.IsNew Then

                        GoTo SkipProcessing

                    Else

                        'Added by saylee on 1-Jun-2016
                        Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
                        mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorService.ModelID,
                                                                                                                      mModelMonitorService.ID.ToString)

                        If mModelMonitorConfiguredList.Count > 0 Then

                            Dim SerialNos As String = String.Empty

                            For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1

                                If i = mModelMonitorConfiguredList.Count - 1 Then
                                    SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                                Else
                                    SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                                End If

                            Next

                            MSGBoxCtrl.Show("Remove Alert!",
                                            "Selected " + mModelMonitorService.ModelMonitorServicePeriods.Item(Index).PeriodUnitName +
                                                      " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. 
                                                      So cannot be removed",
                                            "In Order to remove frequency please delete all configured status first.",
                                            MsgBoxStyle.OkOnly,
                                            "")

                            Exit Select

                        End If

                    End If

                Catch ex As Exception
                    ex.GetBaseException()
                End Try

SkipProcessing: mModelMonitorService.ModelMonitorServicePeriods.Remove(mModelMonitorService.
                                                                                                ModelMonitorServicePeriods.
                                                                                                    Item(Index).ID)
                Session("mModelMonitorService") = mModelMonitorService
                dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods
                dgPeriods.DataBind()
                upnlPeriods.Update()

        End Select

    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsValid Then
            If Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub
            If Save() = True Then
                ControlVisibility()
                SetCaption()
                UpdatePanel()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnATAChapter_Click(sender As Object, e As EventArgs) Handles imgbtnATAChapter.Click
        SetSession()
    End Sub

    Private Sub AddPeriodUnit(sender As Object, e As EventArgs) Handles btnAddPeriodUnit.Click

        SetObject()
        SetPeriodUnits()
        SetGridObject()

        'Added by saylee on 1-Jun-2016
        'Modified by Harsh on 7th June 2024 for reducing the load time while revising a record
        If Not mModelMonitorService.IsNew Then

            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
            mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorService.ModelID,
                                                                                                          mModelMonitorService.ID.ToString)

            If mModelMonitorConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1

                    If i = mModelMonitorConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                    End If

                Next

                Dim ServiceMPDTitle As String = ""

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Service"
                End If

                MSGBoxCtrl.Show("Alert!",
                                ServiceMPDTitle + " is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. 
                                          So new Frequency cannot be added",
                                "In Order to add frequency please delete all configured status first.",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            End If

        End If

        ScriptManager.RegisterStartupScript(Me,
                                            Me.GetType,
                                            "OpenPeriodUnitWindow",
                                            "OpenPeriodUnitWindow()",
                                            True)

    End Sub

    Private Sub cmbMonitorServiceType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMonitorServiceType.SelectedIndexChanged
        mModelMonitorService.ModelMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue), Int32)
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If cmbMonitorServiceType.SelectedIndex > 0 Then
                Dim mMaxCodeMaintActTypewiseForModel As MaxCodeMaintActTypewiseForModel
                mMaxCodeMaintActTypewiseForModel = MaxCodeMaintActTypewiseForModel.GetCode(mModelMonitorService.ModelID, 5, CInt(cmbMonitorServiceType.SelectedValue))
                If Int32.TryParse(mMaxCodeMaintActTypewiseForModel.Code, Nothing) Then
                    Dim TempCode As String = (CInt(mMaxCodeMaintActTypewiseForModel.Code) + 1).ToString
                    If TempCode.Length < 3 Then
                        mModelMonitorService.Code = TempCode.PadLeft(3, "0"c)
                    Else
                        mModelMonitorService.Code = TempCode
                    End If
                    txtCode.DataBind()
                Else
                    mModelMonitorService.Code = ""
                    txtCode.DataBind()
                End If
            Else
                mModelMonitorService.Code = ""
                txtCode.DataBind()
            End If
            upnlMonitorServiceDetails.Update()
        End If
        'End
        For i As Integer = 0 To mModelMonitorService.ModelMonitorServicePeriods.Count - 1
            mModelMonitorService.ModelMonitorServicePeriods(i).MonitorTypeID = mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID
            If mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID = 3 Then        'this is for No Frequency
                mModelMonitorService.ModelMonitorServicePeriods(i).FrequencyValue = CStr(0)
            End If
        Next
        dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods
        dgPeriods.DataBind()

        REM: for ReadOnlyFrequencyColumn
        For i As Integer = 0 To dgPeriods.Rows.Count - 1
            Dim txtFreqVal As TextBox = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            txtFreqVal.ReadOnly = mModelMonitorService.ReadOnlyFrequencyColumn
        Next
        If cmbMonitorServiceType.Enabled = True Then
            setFocus(cmbMonitorServiceType)
        End If
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        REM: dereferencing the objects when form is closing to free the memory
        mATAList = Nothing
        mModelMonitorServiceTypeList = Nothing
        RemoveSession()
        Session("EditMasterRecord") = "False"
        Session("mModelMonitorService") = mModelMonitorService
        Session.Remove("mMaintenanceTaskAndKit")
        Session.Remove("PreviousAssemblyMonitorServiceStatusForRevise") 'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
        'Added by Vikrant on 27-July-2011
        MarkLog(Action.Close, "Model Service", "", ErrorType.NoError, Guid.Empty, EventLogID)

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
    End Sub

    Private Sub SaveSelect(sender As Object, e As EventArgs) Handles btnSaveSelect.Click

        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0

        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************

        If IsValid Then

            If Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub

            If Save() = True Then

                Session("mModelMonitorService") = mModelMonitorService
                mModelMonitorService = CType(Session("mModelMonitorService"), ModelMonitorService)

                If Session("NewPage") = "True" Or mModelMonitorService.ReviseRemark <> "" Then   'Modified by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity

                    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                    If mPreviousAssemblyMonitorServiceStatusForRevise IsNot Nothing AndAlso
                       mModelMonitorService.ReviseRemark <> "" Then

                        If mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString() = "" Then

                            mAssemblyMonitorServiceStatus.AsOnDate = IIf(mPreviousAssemblyMonitorServiceStatusForRevise.
                                                                                        AsOnDateFormatted.ToString() = "",
                                                                         DBNull.Value,
                                                                         mPreviousAssemblyMonitorServiceStatusForRevise.
                                                                                        AsOnDateFormatted.ToString())

                        Else
                            mAssemblyMonitorServiceStatus.AsOnDate = mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString()
                        End If

                    End If

                    If mPreviousAssemblyMonitorServiceStatusForRevise IsNot Nothing Then

                        If mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString() = "" Then
                            mAssemblyMonitorServiceStatus.DoneOn = DBNull.Value
                        Else
                            mAssemblyMonitorServiceStatus.DoneOn = mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString()
                        End If

                    End If
                    'End


                    '======= Saylee On 30-07-2008
                    mModelMonitorService = ModelMonitorService.GetModelMonitorService(mModelMonitorService.ID,
                                                                                      mMachine.HourType)

                    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid,
                                                                                                                 mAssemblyStatus.AssemblyID,
                                                                                                                 mAssemblyStatus.ID,
                                                                                                                 Session("mIssueDate"),
                                                                                                                 mAssemblyStatus.
                                                                                                                          Assembly.ModelID,
                                                                                                                 mHourType)

                    With mAssemblyMonitorServiceStatus

                        .ModelMonitorServiceID(True) = mModelMonitorService.ID
                        '.ModelMonitorService.Code = mModelMonitorService.Code
                        .ModelMonitorService.Reference = mModelMonitorService.Reference
                        .ModelMonitorService.Description = mModelMonitorService.Description
                        .ModelMonitorService.RequiredManHours = mModelMonitorService.RequiredManHours

                    End With

                    SetSession()
                    Session.Remove("Edit")
                    Session("FromModelMonitorServiceList") = True
                    '====================
                    Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
                    'Added By Utkarsh ON 28-May-2012 FOR Link Maintenance
                    Session.Remove("mLinkMaintenanceActionList")
                    Session.Remove("mLinkMaintenanceList")
                    Session.Remove("URL")
                    Session.Remove("MaintenanceActivityID")
                    'End
                    Session.Remove("mMaintenanceTaskAndKit")
                    Session.Remove("mFileAttach")
                    Session.Remove("IsAttachmentDeleted")

                    Dim str As String

                    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                    If Session("RevisedFromListPage") IsNot Nothing AndAlso
                       Session("RevisedFromListPage").ToString.ToLower = "true" Then

                        Dim openAs As String = Request.QueryString("Type")
                        Session.Remove("RevisedFromListPage")

                        If openAs IsNot Nothing AndAlso openAs = "pup" Then

                            ScriptManager.RegisterStartupScript(Me,
                                                                Me.GetType,
                                                                "On Close Script",
                                                                "CallParentCallback();",
                                                                True)

                            Exit Sub

                        End If

                    Else
                        str = "openledgersame('wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');"
                    End If


                    ScriptManager.RegisterStartupScript(Me,
                                                        Me.GetType(),
                                                        "OpenScript",
                                                        str,
                                                        True)
                Else

                    With mAssemblyMonitorServiceStatus

                        .ModelMonitorServiceID(False) = mModelMonitorService.ID
                        '.ModelMonitorService.Code = mModelMonitorService.Code
                        .ModelMonitorService.Reference = mModelMonitorService.Reference
                        .ModelMonitorService.Description = mModelMonitorService.Description
                        .ModelMonitorService.RequiredManHours = mModelMonitorService.RequiredManHours
                        '.ModelMonitorService.ModelMonitorServiceTypeID = mModelMonitorService.ModelMonitorServiceTypeID

                    End With

                    SetSession()
                    Session.Remove("Edit")
                    Session("FromModelMonitorServiceList") = True
                    Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
                    'Added By Utkarsh ON 28-May-2012 FOR Link Maintenance
                    Session.Remove("mLinkMaintenanceActionList")
                    Session.Remove("mLinkMaintenanceList")
                    Session.Remove("URL")
                    Session.Remove("MaintenanceActivityID")
                    'End
                    Session.Remove("mMaintenanceTaskAndKit")

                    Dim str As String

                    If Session("IsOpenFromMaster") = True Then

                        Session.Remove("IsOpenFromMaster")

                        If mAssemblyStatus.IsSpareAssembly = True Then  'Added by Saylee on 10-Feb-2020,  All27072020
                            str = "openledgersame('wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfSpareAssemblyStatus.aspx');"
                        Else
                            str = "openledgersame('wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfAssemblyStatus_Ajax.aspx');"
                        End If

                    Else
                        str = "openledgersame('wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx');"
                    End If

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        str,
                                                        True)

                End If

            End If

        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub btnAddNewLinkMaintenance_Click(sender As Object, e As EventArgs) Handles btnAddNewLinkMaintenance.Click
        SetLinkMaintenanceGridObject()
        Dim URL As Stack = New Stack    'STACK to store url of current page
        URL.Push(Request.Url)           'Inserting URL in STACK
        Session("URL") = URL
        Session("MaintenanceActivityID") = mModelMonitorService.ID
        Session("ModelIDForMPD") = mModelMonitorService.ModelID  'Added By Vikrant For MPD
        Response.Redirect("wfModelMonitorActivityList.aspx?FromType=" & cmbMonitorType.SelectedValue)
    End Sub
    Private Sub dgLinkedMaintenanceList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgLinkedMaintenanceList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteLM")
                Dim Index As Int32 = CInt(e.CommandArgument) + dgLinkedMaintenanceList.PageIndex * dgLinkedMaintenanceList.PageSize
                mLinkMaintenanceList.CurrentIndex = Index
                Session("mLinkMaintenanceList") = mLinkMaintenanceList
        End Select
    End Sub
    Private Sub dgLinkedMaintenanceList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgLinkedMaintenanceList.Sorting
        mLinkMaintenanceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLinkMaintenanceList") = mLinkMaintenanceList
        dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
        dgLinkedMaintenanceList.DataBind()
        upnlLinkedMaintenanceList.Update()
    End Sub 'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub lnkTools_Click(sender As Object, e As EventArgs) Handles lnkTools.Click
        If IsValid Then
            SetObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorService.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 3
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkSpares_Click(sender As Object, e As EventArgs) Handles lnkSpares.Click
        If IsValid Then
            SetObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorService.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 2
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkTaskCards_Click(sender As Object, e As EventArgs) Handles lnkTaskCards.Click
        If IsValid Then
            SetObject()
            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorService.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
                '''''mModelMonitorService.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)
            End If
            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 1 'Added by Saylee on 23-July-2013 for BA22072013
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnAddTools_Click(sender As Object, e As EventArgs) Handles hdnBtnTools.Click
        SetToolsSparesCount()
        upnlOtherDetails.Update()
        Session.Remove("mChild")
    End Sub
    Private Sub hdnBtnPeriodUnit_Click(sender As Object, e As EventArgs) Handles hdnBtnPeriodUnit.Click
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        AddSelectedPeriodUnits()
        dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(sender As Object, e As EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "<SELECT>")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click
        mModelMonitorService.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick
        If mModelMonitorService.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorService.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mModelMonitorService.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(sender As Object, e As Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mModelMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorService.ID)
            Session("mFileAttach") = mFileAttach
        End If
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If
        End If
    End Sub

    Private Sub btnDelAttach_Click(sender As Object, e As EventArgs) Handles btnDelAttach.Click

        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mModelMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorService.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mModelMonitorService.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub

    'Added by Shital on 07-JUl-2020
    Private Sub btnSendMail_Click(sender As Object, e As EventArgs) Handles btnSendMail.Click

        Session("UserEmailID") = mModuleList.Item("AssemblyServiceMonitor").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AssemblyServiceMonitor").SendCCMailID
        Session("SmtpHost") = mModuleList.Item("AssemblyServiceMonitor").SmtpHost
        Session("SmtpPort") = mModuleList.Item("AssemblyServiceMonitor").SmtpPort
        Session("SmtpUser") = mModuleList.Item("AssemblyServiceMonitor").SmtpUser
        Session("SmtpPassword") = mModuleList.Item("AssemblyServiceMonitor").SmtpPassword
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub

    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() NotifyMail())
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path,
                     OpenMode.Append,
                     OpenAccess.ReadWrite)

            WriteLine(1,
                      Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)

            FileClose(1)

        End Try
    End Sub

    Public Sub NotifyMail()
        Dim str As String
        Dim mSendMailFile As New SendMailFile
        Dim ToMailIDs As String = ""
        Dim CCMailIDs As String = ""

        'ToMailIDs = mModuleList.Item("AssemblyServiceMonitor").SendToMailID
        'CCMailIDs = mModuleList.Item("AssemblyServiceMonitor").SendCCMailID
        ToMailIDs = Session("ToSendMailIDs")
        CCMailIDs = Session("CcSendMailIDs")

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD"
        Else
            ServiceMPDTitle = "Service"
        End If

        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "</br> ")

        str = str + ("<p><font face=""Calibri"">")
        str = str + mCompanyDetail.CompanyName
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + " Following Assembly " + ServiceMPDTitle + " Added in FlyPal System and need your attentions."
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + "Please Login to FlyPal® for detailed information."
        str = str + ("</font></p>")


        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Inspection Type: " + "</b>" + cmbMonitorServiceType.SelectedItem.Text.ToString + "</p><p><b>Code: " + "</b>" + mModelMonitorService.Code)
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Description: " + "</b>" + mModelMonitorService.Description + "</p><p><b>ATA: " + "</b>" + mModelMonitorService.ATAChapter)
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Reference: " + "</b>" + mModelMonitorService.Reference.ToString)
        str = str + ("</font></p>")

        'Added by shital on 30-Oct-2020
        Dim MyFile As String
        If mModelMonitorService.IsAttachmentAdded = True Then
            If mModelMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
                mFileAttach = FileAttach.GetAttachment(mModelMonitorService.ID)
                Session("mFileAttach") = mFileAttach
            End If
            If mFileAttach.Size > 0 Then
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                MyFile = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension

                Dim fs As FileStream
                If File.Exists("C:\Temp\") = False Then
                    IO.File.Delete(MyFile)
                    fs = File.Create(MyFile)
                    fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)

                    fs.Close()
                End If
            End If

        End If
        '--------

        SendMailFile.SendMailFile(, User.Identity.Name, ServiceMPDTitle, Info:=str, ToMailID:=ToMailIDs.ToString, CCMailID:=CCMailIDs, ReportPath:=MyFile, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
             SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

        Dim mModelMoniterModDetail As String = ServiceMPDTitle + " Notification sent successfully to " + ToMailIDs.ToString.TrimEnd(",") + " by " + User.Identity.Name
        MarkLog(Action.SendMail, "Service master", mModelMoniterModDetail, ErrorType.HandledError, mModelMonitorService.ID, EventLogID)

    End Sub
    'End

#End Region

#Region " Report "

    'Created By :- Pallavi , Date -10/08/2006
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"

    Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click

        Rpt = New crDetModelMonitorService
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 9
        RHCount = Me.mModelMonitorService.ModelMonitorServicePeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD"
        Else
            ServiceMPDTitle = "Service"
        End If


        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Code/Form No.",
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
            dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Code/Form No.",
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "ATA Chapter",
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                          CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "ATA Chapter",
                                                   cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", lblReference.Text,
                             txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                           CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", lblReference.Text,
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                                     "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Description",
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Description",
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", ServiceMPDTitle + " Type",
                                    cmbMonitorServiceType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", ServiceMPDTitle + " Type",
                                     cmbMonitorServiceType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Zone",
                                    txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Zone",
                                     txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Area",
                                    txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Area",
                                     txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 6 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Note",
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Note",
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 7 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Estd. Man Hours",
                                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                  CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Estd. Man Hours",
                                     txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 8 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "",
                     "", , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                 CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                 CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "",
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "",
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring " + ServiceMPDTitle,
                 CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                 CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Model " + ServiceMPDTitle + " Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

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


End Class