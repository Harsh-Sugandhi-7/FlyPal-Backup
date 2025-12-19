Imports System.Text
Public Class wfModelMonitorInspection_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'for Object
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mModelMonitorInsp As ModelMonitorInsp
    Public Flag As Int16
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mModelMonitorInspTypeList As ModelMonitorInspTypeList
    Public mModelMonitorInspPeriodUnitList As ModelMonitorInspPeriodUnitList
    Dim str As String
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mIssueDate As String
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
    Dim EventLogID As Guid 'Added by Vikrant on 28-July-2011
    Public mInspectionDetail As String
    Public mModel As String
    Public mMonitorInspectionType As String
    Public mMonitorDesc As String
    Public mLinkMaintenanceActionList As LinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance 'End
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mPrevAssemblyMonitorInspStatusForRevise As AssemblyMonitorInspStatus  'Revise Activity
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 07-Jul-2020 for Add EMailIDs field in csTransType
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        mModelMonitorInsp = CType(Session("mModelMonitorInsp"), ModelMonitorInsp)
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorInspTypeList = CType(Session("mModelMonitorInspTypeList"), ModelMonitorInspTypeList)
        mModelMonitorInspPeriodUnitList = CType(Session("mModelMonitorInspPeriodUnitList"), ModelMonitorInspPeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
        mLinkMaintenanceActionList = Session("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        mLinkMaintenanceList = Session("mLinkMaintenanceList") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mPrevAssemblyMonitorInspStatusForRevise = Session("mPrevAssemblyMonitorInspStatusForRevise") 'Revise Activity
        mModuleList = Session("mModuleList") 'Added by shital on 07-Jul-2020 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mModelMonitorInsp") = mModelMonitorInsp
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mATAList") = mATAList
        Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
        Session("mModelMonitorInspPeriodUnitList") = mModelMonitorInspPeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorInspTypeList")
        Session.Remove("mModelMonitorInspPeriodUnitList")
        Session.Remove("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session.Remove("mLinkMaintenanceList")
        Session.Remove("URL")
        Session.Remove("MaintenanceActivityID") 'End
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub setObject()
        'mModelMonitorInsp.Code = Trim(txtCode.Text)
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
                mModelMonitorInsp.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
            Else
                mModelMonitorInsp.Code = Trim(txtCode.Text)
            End If
        Else
            mModelMonitorInsp.Code = Trim(txtCode.Text)
        End If
        mModelMonitorInsp.Reference = Trim(txtReference.Text)
        mModelMonitorInsp.Description = Trim(txtDescription.Text)
        mModelMonitorInsp.Note = Trim(txtNote.Text)
        mModelMonitorInsp.ATAID = New Guid(cmbATAChapter.SelectedValue)
        mModelMonitorInsp.ModelMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        mModelMonitorInsp.ShowInCofA = chkShowInCofA.Checked
        mModelMonitorInsp.RequiredManHours = txtRequiredManHours.Text
        mModelMonitorInsp.Zone = Trim(txtZone.Text) 'Added by Saylee on 23-July-2013 for BA22072013 
        mModelMonitorInsp.Area = Trim(txtArea.Text)
        mModelMonitorInsp.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorInsp.IsAttachmentAdded = True
            Else
                mModelMonitorInsp.IsAttachmentAdded = False
            End If
        End If
        Session("mModelMonitorInsp") = mModelMonitorInsp
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorInsp.ModelMonitorInspPeriods
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If .Item(i).PeriodID = 2 And Decimal.MaxValue < Val(txtFrequencyValue.Text) Then
                    .Item(i).FrequencyValue = ""
                Else
                    .Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
                End If
            Next i
        End With
        mModelMonitorInsp = Session("mModelMonitorInsp")
    End Sub
    Private Sub SetPage()
        If mModelMonitorInsp.IsNew Then
            lblTitle.Text = "Model Inspection of [ Model: " & mModelMonitorInsp.Model.Name & "] [New]"
        Else
            lblTitle.Text = "Model Inspection of [ Model: " & mModelMonitorInsp.Model.Name & "]"
        End If
        'Added By Utkarsh ON 10-Jan-2012
        lblResult.Text = "List Of Linked Maintenance Activity : " & mLinkMaintenanceList.Count & " Record(s) found."
        'End
        'Added By Saylee ON 4-Feb-2013 for BA04022013
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            lblReference.Text = "Task Source Reference"
        ElseIf AppSettings("ClientCode") = "Indamer" Then 'Added By Prashant 3-Apr-2013  'Indamer03042013
            lblReference.Text = "Task Code/Reference"
            txtReference.ToolTip = "Enter Task Code/Reference"
        Else
            lblReference.Text = "Reference"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub ControlVisibility()
        'btnAddPeriodUnit.Enabled = mModelMonitorInspPeriodUnitList.Count > 0
        If Session("ModelIDFromModelCreation") = Nothing Then
            btnAddPeriodUnit.Enabled = mModelMonitorInspPeriodUnitList.Count > 0 'Session("ModelIDFromModelCreation") = Nothing,'Added by Saylee on 14-Nov-2019
        Else
            btnAddPeriodUnit.Enabled = True
        End If
        btnPrint.Enabled = Not mModelMonitorInsp.IsNew
        btnSendMail.Enabled = Not mModelMonitorInsp.IsNew 'Added by Shital on 02-Jul-2020 for sendmail functionality
        If AppSettings("LinkMaintenance") = True Then 'Added By Utkarsh On 27-Jun-2012
            If Not mLinkMaintenanceList Is Nothing Then
                dgLinkedMaintenanceList.Columns(7).Visible = mLinkMaintenanceList.ShowDirectiveNo
            End If
        End If 'End

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorInsp.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList = Session("mModelMonitorConfiguredList")
            If Not mModelMonitorConfiguredList Is Nothing Then
                If mModelMonitorConfiguredList.Count > 0 Then
                    cmbMonitorInspType.Enabled = False
                Else
                    cmbMonitorInspType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mModelMonitorInsp.ModelMonitorInspPeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mModelMonitorConfiguredList.Count > 0 Then
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
        If Session("OpenFromDiscrepancyCorrectiveActionList") = "True" Then 'Added By Prashant 4-Mar-2024
            btnSave.Visible = False
            upnlOtherDetails.Visible = False
            upnlLinkMaint.Visible = False
            btnSaveSelect.Text = "Save"
            btnSave.ToolTip = "Click to save Model Inspection"
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mModelMonitorInsp.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
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
                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                    If InStr(ex.Message, "FKtabAssemblyMonitorInspStatustabModelMonitorInsp", CompareMethod.Text) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This record is currently used in Assembly Inspection Status", MsgBoxStyle.OkOnly, "")
                    End If
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
        Dim mModelMonitorInspClone As ModelMonitorInsp
        mModelMonitorInspClone = CType(mModelMonitorInsp, ModelMonitorInsp)
        setObject()
        SetGridObject()
        If mModelMonitorInsp.IsValid = True Then
            If mModelMonitorInsp.ModelMonitorInspPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "Model Inspection cannot be saved without Period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                'Revise Activity New
                If Not Session("ModelMonitorInspIDToBeLinked") Is Nothing And mModelMonitorInsp.IsNew Then
                    Dim OldModelMonitorInsp As ModelMonitorInsp
                    Dim mOldFreqValues As New StringBuilder
                    OldModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mAssemblyMonitorInspStatus.ModelMonitorInspID)
                    For i As Integer = 0 To OldModelMonitorInsp.ModelMonitorInspPeriods.Count - 1
                        If OldModelMonitorInsp.ModelMonitorInspPeriods(i).PeriodID = 2 And Decimal.MaxValue < Val(OldModelMonitorInsp.ModelMonitorInspPeriods(i).FrequencyValue) Then
                        Else
                            mOldFreqValues.Append(OldModelMonitorInsp.ModelMonitorInspPeriods(i).FrequencyValueFormatted.ToString + " " + OldModelMonitorInsp.ModelMonitorInspPeriods(i).PeriodUnitName + ",")
                        End If
                    Next

                    OldModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(New Guid(Session("ModelMonitorInspIDToBeLinked").ToString))
                    mModelMonitorInsp.PrevRefID = New Guid(Session("ModelMonitorInspPrevRefIDToBeLinked").ToString)
                    mModelMonitorInsp.ReviseRemark = "This is revised inspection with Old Frequency : " + mOldFreqValues.ToString.TrimEnd(CChar(",")) + ", Old Description : " + OldModelMonitorInsp.Description
                End If
                'End
                mModelMonitorInsp.ApplyEdit()
                mModelMonitorInsp = CType(mModelMonitorInsp.Save(), ModelMonitorInsp)
                'Revise Activity
                Dim mMaintenanceKit As MaintenanceKit
                Dim mMaintenanceKitOld As MaintenanceKit
                Dim mMaintenanceTask, mMaintenanceTaskOld As MaintenanceTask
                If mModelMonitorInsp.ReviseRemark <> "" Then
                    Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount
                    mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorInsp.ID)
                    If mMaintenanceKitDetailsCount.MaintenanceSparesCount = 0 And mMaintenanceKitDetailsCount.MaintenanceTasksCount = 0 And mMaintenanceKitDetailsCount.MaintenanceToolsCount = 0 Then
                        mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForInsp(mModelMonitorInsp)

                        'mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mModelMonitorInsp.PrevRefID)
                        'mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorInsp.PrevRefID, False)
                        'Tools
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorInsp.PrevRefID, True)

                        mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly, True)
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
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorInsp.PrevRefID, False)

                        mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly, False)
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
                        mMaintenanceTaskOld = MaintenanceTask.GetMaintenanceTaskByParent(mModelMonitorInsp.PrevRefID)

                        mMaintenanceTask = MaintenanceTask.NewMaintenanceTask(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly)
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
                'End
                SaveAttachment()
                SaveLinkList()
                mInspectionDetail = "Model : " & mModel & " Model Inspection Type : " & mModelMonitorInsp.ModelMonitorInspTypeName & " Description : " & mModelMonitorInsp.Description
                MarkLog(Util.Action.Save, "Model Inspection", mInspectionDetail, Util.ErrorType.NoError, mModelMonitorInsp.ID, EventLogID)
                'end
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorInsp") = mModelMonitorInsp
                Return True
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    '   MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                    If InStr(ex.Message, "FKtabAssemblyMonitorInspStatustabModelMonitorInsp", CompareMethod.Text) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This record is currently used in Assembly Inspection Status", MsgBoxStyle.OkOnly, "")
                    ElseIf InStr(ex.Message, "FKtabAssemblyMonitorInspStatusPeriodtabModelMonitorInspPeriod", CompareMethod.Text) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This record is currently used in Assembly Inspection Status", MsgBoxStyle.OkOnly, "")
                    End If
                End If
                mModelMonitorInsp = mModelMonitorInspClone
                Session("mModelMonitorInsp") = mModelMonitorInsp
                Return False
            Finally
                mModelMonitorInspClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub AddSelectedPeroidUnits()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************

        Dim clnModelMonitorInsp As ModelMonitorInsp = mModelMonitorInsp.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mModelMonitorInsp.ModelMonitorInspPeriods.Add(mModelMonitorInsp.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
                End If
            Next
            For i As Integer = 0 To mModelMonitorInsp.ModelMonitorInspPeriods.Count - 1
                mModelMonitorInsp.ModelMonitorInspPeriods(i).MonitorTypeID = mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID
                If mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID = 3 Then
                    mModelMonitorInsp.ModelMonitorInspPeriods(i).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorInsp") = mModelMonitorInsp
        Catch ex As Exception
            mModelMonitorInsp = clnModelMonitorInsp
            Session("mModelMonitorInsp") = mModelMonitorInsp
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorInsp = Nothing
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
            For i As Integer = 0 To mModelMonitorInspPeriodUnitList.Count - 1
                If Not mModelMonitorInsp.ModelMonitorInspPeriods.Contains(mModelMonitorInspPeriodUnitList(i).ID) Then
                    mSelectPeriodUnits.Add(mModelMonitorInspPeriodUnitList(i).ID, mModelMonitorInspPeriodUnitList(i).PeriodID, mModelMonitorInspPeriodUnitList(i).Name)
                End If
            Next
        Else
            'Added by Saylee on 14-Nov-2019
            Dim i As Int32
            Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
            While i <= mPeriodUnitList.Count - 1
                If mModelMonitorInsp.ModelMonitorInspPeriods.Contains(mPeriodUnitList(i).ID) = False Then
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

        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(ID, mModelMonitorInsp.ModelID, mHourType, ID)
        Session("mModelMonitorInsp") = mModelMonitorInsp.ModelMonitorInspPeriods
    End Sub
    Private Sub UpdatePanel()
        upnlMonitorInspectionDetails.Update()
        upnlATAMaster.Update()
        upnlMonitorInspectionType.Update()
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
                            SetPage()
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
    Private Sub SetRights() 'Added By Utkarsh On 15-Mar-2011
        If mAssemblyStatus.IsMaster Then
            If (User.IsInRole("MachineAssemblyInspectionPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
                btnSaveSelect.Enabled = False
                btnSaveSelect.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (User.IsInRole("MachineAssemblyInspectionPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
                btnSaveSelect.Enabled = False
                btnSaveSelect.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub '*******************************
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount 'Revise Activity
        If mModelMonitorInsp.IsNew And mModelMonitorInsp.ReviseRemark <> "" Then
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorInsp.PrevRefID)
        Else
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorInsp.ID)
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
                If (Not mModelMonitorInsp.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mModelMonitorInsp.ID)
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
        cmbMonitorInspType.DataSource = mModelMonitorInspTypeList

        If Session("ModelIDFromModelCreation") = Nothing Then 'added by saylee on 14-Nov-2019
            mModelMonitorInspPeriodUnitList = ModelMonitorInspPeriodUnitList.GetModelMonitorInspPeriodUnitList(mAssemblyMonitorInspStatus.AssemblyStatusID)
        End If

        Session("mModelMonitorInspPeriodUnitList") = mModelMonitorInspPeriodUnitList
        dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods

        mLinkMaintenanceActionList = LinkMaintenanceActionList.GetLinkMaintActionList(True) 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        If mLinkMaintenanceList Is Nothing Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorInsp.ID.ToString)
        End If
        dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
        lnkLinkMaint1.Text = "Click to add Link Maintenance Activity " + "(" + mLinkMaintenanceList.Count.ToString + " activite(s))"
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorInsp.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
            mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInsp.ModelID, mModelMonitorInsp.ID.ToString)
            Session("mModelMonitorConfiguredList") = mModelMonitorConfiguredList
        End If
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbATAChapter" Then
            If cmbATAChapter.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbMonitorInspType" Then
            If cmbMonitorInspType.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
            'ElseIf custValidator.ControlToValidate = "txtDescription" Then
            '    If Len(txtDescription.Text) > 1000 Then
            '        custValidator.ErrorMessage = "Description can't be more than 1000 chars."
            '        e.IsValid = False
            '    End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note can't be more than 1000 chars."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtReference" Then
            If Len(txtReference.Text) > 500 Then
                custValidator.ErrorMessage = "Reference Too Long"
                e.IsValid = False
            End If
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        'If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        setObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtFrequencyValue As TextBox
        If Not mModelMonitorInsp.IsValid Then
            For i As Integer = 0 To mModelMonitorInsp.GetBrokenRulesCollection.Count - 1
                str = str + mModelMonitorInsp.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Rows(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mModelMonitorInsp.ModelMonitorInspPeriods(i).IsValid Then
                For j As Integer = 0 To mModelMonitorInsp.ModelMonitorInspPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            If Not mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvDescription.ErrorMessage = str
            cvDescription.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Sub CustomValidate3(ByVal s As Object, ByVal e As ServerValidateEventArgs) 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
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
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by Vikrant on 28-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtCode.Enabled = True Then
                setFocus(txtCode)
            End If
            mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("<SELECT>")
            Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
            AddSelectedPeroidUnits()
            DataFieldBind()
            ControlVisibility()
            SetPage()
            If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019 
                SetRights()  'Added By Utkarsh On 15-Mar-2011
            End If

            SetToolsSparesCount()
            ControlVisibilityForAttachment()
        End If
        If AppSettings("ClientCode") = "Heligo" Then
            lblZone.InnerText = "System"

        End If
    End Sub
    Private Sub dgPeriods_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPeriods.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
                If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
                    If mAssemblyStatus.IsMaster Then 'Added By Utkarsh On 15-Mar-2011
                        If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    ElseIf Not mAssemblyStatus.IsMaster Then
                        If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    End If '*******************************
                End If

                'Added by saylee on 1-Jun-2016
                Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
                mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInsp.ModelID, mModelMonitorInsp.ID.ToString)

                If mModelMonitorConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                        If i = mModelMonitorConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mModelMonitorInsp.ModelMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If

                mModelMonitorInsp.ModelMonitorInspPeriods.Remove(mModelMonitorInsp.ModelMonitorInspPeriods.Item(Index).ID)
                Session("mModelMonitorInsp") = mModelMonitorInsp
                dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods
                dgPeriods.DataBind()
                upnlPeriods.Update()
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub
        If Save() Then
            ControlVisibility()
            SetPage()
            UpdatePanel()
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub imgbtnATAChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnATAChapter.Click
        setObject()
        RemoveSession()
    End Sub
    Private Sub btnAddPeriodUnit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriodUnit.Click
        SetPeroidUnits()
        SetGridObject()
        setObject()

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorInsp.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
            mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInsp.ModelID, mModelMonitorInsp.ID.ToString)

            If mModelMonitorConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                    If i = mModelMonitorConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "Inspection is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow()", True)

    End Sub
    Private Sub cmbMonitorInspType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorInspType.SelectedIndexChanged
        mModelMonitorInsp.ModelMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If cmbMonitorInspType.SelectedIndex > 0 Then
                Dim mMaxCodeMaintActTypewiseForModel As MaxCodeMaintActTypewiseForModel
                mMaxCodeMaintActTypewiseForModel = MaxCodeMaintActTypewiseForModel.GetCode(mModelMonitorInsp.ModelID, 6, CInt(cmbMonitorInspType.SelectedValue))
                If Int32.TryParse(mMaxCodeMaintActTypewiseForModel.Code, Nothing) Then
                    Dim TempCode As String = (CInt(mMaxCodeMaintActTypewiseForModel.Code) + 1).ToString
                    If TempCode.Length < 3 Then
                        mModelMonitorInsp.Code = TempCode.PadLeft(3, "0"c)
                    Else
                        mModelMonitorInsp.Code = TempCode
                    End If
                    txtCode.DataBind()
                Else
                    mModelMonitorInsp.Code = ""
                    txtCode.DataBind()
                End If
            Else
                mModelMonitorInsp.Code = ""
                txtCode.DataBind()
            End If
            upnlMonitorInspectionDetails.Update()
        End If
        'End
        For i As Integer = 0 To mModelMonitorInsp.ModelMonitorInspPeriods.Count - 1
            mModelMonitorInsp.ModelMonitorInspPeriods(i).MonitorTypeID = mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID
            If mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID = 3 Then
                mModelMonitorInsp.ModelMonitorInspPeriods(i).FrequencyValue = CStr(0)
            End If
        Next
        dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session.Remove("mMaintenanceTaskAndKit")
        Session.Remove("mPrevAssemblyMonitorInspStatusForRevise") 'Revise Activity
        Session("EditMasterRecord") = "False"
        Session.Remove("OpenFromDiscrepancyCorrectiveActionList")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
    End Sub
    Private Sub btnSaveSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSelect.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub
        If Save() Then

            Session("mModelMonitorInsp") = mModelMonitorInsp

            mModelMonitorInsp = CType(Session("mModelMonitorInsp"), ModelMonitorInsp)
            mIssueDate = Session("mIssueDate")

            'Added by Saylee on 10-Feb-2020,  All27072020
            Dim mHourType As Integer = 0
            If mAssemblyStatus.IsSpareAssembly = True Then
                mHourType = mAssemblyStatus.HourType
            Else
                mHourType = mMachine.HourType
            End If
            '*********************

            If Session("NewPage") = "True" Or mModelMonitorInsp.ReviseRemark <> "" Then 'Revise Activity

                'Revise Activity
                If Not mPrevAssemblyMonitorInspStatusForRevise Is Nothing And mModelMonitorInsp.ReviseRemark <> "" Then
                    If mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mAssemblyMonitorInspStatus.AsOnDate = mPrevAssemblyMonitorInspStatusForRevise.AsOnDateFormatted.ToString
                    Else
                        mAssemblyMonitorInspStatus.AsOnDate = mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString
                    End If
                End If
                'End
                '======= Saylee On 30-07-2008
                mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mModelMonitorInsp.ID, mHourType)
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mIssueDate, mAssemblyStatus.Assembly.ModelID, mHourType)
                With mAssemblyMonitorInspStatus
                    .ModelMonitorInspID(True) = mModelMonitorInsp.ID
                    '.ModelMonitorInsp.Code = mModelMonitorInsp.Code
                    .ModelMonitorInsp.Reference = mModelMonitorInsp.Reference
                    .ModelMonitorInsp.Description = mModelMonitorInsp.Description
                    .ModelMonitorInsp.RequiredManHours = mModelMonitorInsp.RequiredManHours

                End With
                'Revise Activity
                If Not mPrevAssemblyMonitorInspStatusForRevise Is Nothing Then

                    If mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mAssemblyMonitorInspStatus.DoneOn = System.DBNull.Value
                    Else
                        mAssemblyMonitorInspStatus.DoneOn = mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString
                    End If
                End If
                'End
                SetSession()
                Session.Remove("Edit")
                Session.Remove("mModelMonitorInspList")
                Session("FromModelMonitorInspList") = True
                '====================
                Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
                'Added By Utkarsh ON 28-May-2012 FOR Link Maintenance
                Session.Remove("mLinkMaintenanceActionList")
                Session.Remove("mLinkMaintenanceList")
                Session.Remove("URL")
                Session.Remove("MaintenanceActivityID")
                'End
                Session.Remove("mMaintenanceTaskAndKit")
                Dim str As String
                If AppSettings("ShowAllValuesPageEnable") = "True" Then
                    Session("MiddleFrame") = "wfComplyAssemblyMonitorInspStatusListShowValues_Ajax.aspx?" 'Revise Activity
                Else
                    If Not Session("mIsSpareAssembly") Is Nothing Then 'Added By Vikrant On 27-Jul-2020 For ALL27072020
                        Session("MiddleFrame") = "wfComplyAssemblyMonitorInspStatusList_Ajax.aspx?SpareAssembly=" & Session("mIsSpareAssembly")
                        'End
                    ElseIf Session("OpenFromDiscrepancyCorrectiveActionList") = "True" Then 'Added By Prashant 4-Mar-2024
                        'Here in Discrepancy case, directly save mAssemblyMonitorInspStatus as next page not required to be shown
                        mAssemblyMonitorInspStatus.LogID(Session("RectifiedLogID").ToString, mIssueDate.ToString, True, CType(Session("mModelMonitorInsp"), ModelMonitorInsp)) = New Guid(Session("RectifiedLogID").ToString)
                        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                        If mAssemblyMonitorInspStatus.IsValid Then
                            mAssemblyMonitorInspStatus.Save()
                            Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction
                            mMELSnagCorrectiveAction = Session("mDiscrepancyCorrectiveAction")
                            mMELSnagCorrectiveAction.ModelMonitorInspID = mAssemblyMonitorInspStatus.ModelMonitorInspID
                            mMELSnagCorrectiveAction.ConsideredInWatchList = True
                            If mMELSnagCorrectiveAction.IsValid Then
                                mMELSnagCorrectiveAction.Save()
                                ControlVisibility()
                                SetPage()
                                UpdatePanel()
                                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            End If
                            Exit Sub
                        End If
                    Else 'Existing condition
                        Session("MiddleFrame") = "wfComplyAssemblyMonitorInspStatusList_Ajax.aspx?SpareAssembly=0" 'Revise Activity
                    End If
                End If

                'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
                If Not Session("RevisedFromListPage") Is Nothing Then
                    If Session("RevisedFromListPage") = "True" Then
                        Session.Remove("RevisedFromListPage")
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If

                    End If
                Else
                    str = "openledgersame('wfAssemblyMonitorInspStatusNew_Ajax.aspx?BackPage=Index.aspx');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
                End If


                'Response.Redirect("wfAssemblyMonitorInspStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            Else
                With mAssemblyMonitorInspStatus
                    .ModelMonitorInspID(False) = mModelMonitorInsp.ID
                    '.ModelMonitorInsp.Code = mModelMonitorInsp.Code
                    .ModelMonitorInsp.Reference = mModelMonitorInsp.Reference
                    .ModelMonitorInsp.Description = mModelMonitorInsp.Description
                    .ModelMonitorInsp.RequiredManHours = mModelMonitorInsp.RequiredManHours
                    '.ModelMonitorInsp.ModelMonitorInspTypeID = mModelMonitorInsp.ModelMonitorInspTypeID
                End With
                SetSession()
                Session.Remove("Edit")
                Session.Remove("mModelMonitorInspList")
                Session("FromModelMonitorInspList") = True
                Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
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
                        str = "openledgersame('wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfSpareAssemblyStatus.aspx');"
                    Else
                        str = "openledgersame('wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfAssemblyStatus_Ajax.aspx');"
                    End If

                Else
                    str = "openledgersame('wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx');"
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
                'Response.Redirect("wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                '--------------------------------------------------------------------------------------------
            End If
        End If
    End Sub
    Private Sub btnAddNewLinkMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewLinkMaintenance.Click
        Dim URL As Stack = New Stack    'STACK to store url of current page
        URL.Push(Request.Url)           'Inserting URL in STACK
        Session("URL") = URL
        Session("MaintenanceActivityID") = mModelMonitorInsp.ID
        Session("ModelIDForMPD") = mModelMonitorInsp.ModelID 'Added By Vikrant For MPD
        Response.Redirect("wfModelMonitorActivityList.aspx?FromType=" & cmbMonitorType.SelectedValue)
    End Sub
    Private Sub dgLinkedMaintenanceList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLinkedMaintenanceList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteLM")
                Dim Index As Int32 = CInt(e.CommandArgument) + dgLinkedMaintenanceList.PageIndex * dgLinkedMaintenanceList.PageSize
                mLinkMaintenanceList.CurrentIndex = Index
                Session("mLinkMaintenanceList") = mLinkMaintenanceList
        End Select
    End Sub
    Private Sub dgLinkedMaintenanceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgLinkedMaintenanceList.Sorting
        mLinkMaintenanceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLinkMaintenanceList") = mLinkMaintenanceList
        dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
        dgLinkedMaintenanceList.DataBind()
        upnlLinkedMaintenanceList.Update()
    End Sub 'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub lnkTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTools.Click
        If IsValid Then
            setObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorInsp.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForInsp(mModelMonitorInsp)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 3
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkSpares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSpares.Click
        If IsValid Then
            setObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorInsp.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForInsp(mModelMonitorInsp)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 2
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkTaskCards_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTaskCards.Click
        If IsValid Then
            setObject()
            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorInsp.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForInsp(mModelMonitorInsp)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 1 'Added by Saylee on 23-July-2013 for BA22072013 	
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnAddTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTools.Click
        SetToolsSparesCount()
        Session.Remove("mChild")
        upnlOtherDetails.Update()
    End Sub
    Private Sub hdnBtnPeriodUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnPeriodUnit.Click
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        AddSelectedPeroidUnits()
        dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "<SELECT>")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mModelMonitorInsp.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mModelMonitorInsp.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorInsp.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mModelMonitorInsp.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mModelMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorInsp.ID)
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mModelMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorInsp.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mModelMonitorInsp.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    'Added by Shital on 07-JUl-2020
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click

        Session("UserEmailID") = mModuleList.Item("AssemblyInspections").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AssemblyInspections").SendCCMailID
        Session("SmtpHost") = mModuleList.Item("AssemblyInspections").SmtpHost
        Session("SmtpPort") = mModuleList.Item("AssemblyInspections").SmtpPort
        Session("SmtpUser") = mModuleList.Item("AssemblyInspections").SmtpUser
        Session("SmtpPassword") = mModuleList.Item("AssemblyInspections").SmtpPassword
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
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
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Public Sub NotifyMail()
        Dim str As String
        Dim mSendMailFile As New SendMailFile
        Dim ToMailIDs As String = ""
        Dim CCMailIDs As String = ""

        'ToMailIDs = mModuleList.Item("AssemblyInspections").SendToMailID
        'CCMailIDs = mModuleList.Item("AssemblyInspections").SendCCMailID
        ToMailIDs = Session("ToSendMailIDs")
        CCMailIDs = Session("CcSendMailIDs")


        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "</br> ")

        str = str + ("<p><font face=""Calibri"">")
        str = str + mCompanyDetail.CompanyName
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + " Following Assembly Inspection Added  in FlyPal System and need your attentions."
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + "Please Login to FlyPal® for detailed information."
        str = str + ("</font></p>")


        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Inspection Type: " + "</b>" + cmbMonitorInspType.SelectedItem.Text.ToString + "</p><p><b>Code: " + "</b>" + mModelMonitorInsp.Code)
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Description: " + "</b>" + mModelMonitorInsp.Description + "</p><p><b>ATA: " + "</b>" + mModelMonitorInsp.ATAChapter)
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Reference: " + "</b>" + mModelMonitorInsp.Reference.ToString)
        str = str + ("</font></p>")

        'Added by shital on 30-Oct-2020
        Dim MyFile As String
        If mModelMonitorInsp.IsAttachmentAdded = True Then
            If mModelMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
                mFileAttach = FileAttach.GetAttachment(mModelMonitorInsp.ID)
                Session("mFileAttach") = mFileAttach
            End If
            If mFileAttach.Size > 0 Then
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                MyFile = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension

                Dim fs As FileStream
                If File.Exists("C:\Temp\") = False Then
                    System.IO.File.Delete(MyFile)
                    fs = File.Create(MyFile)
                    fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)

                    fs.Close()
                End If
            End If

        End If
        '--------

        SendMailFile.SendMailFile(, User.Identity.Name, "Inspection", Info:=str, ToMailID:=ToMailIDs.ToString, CCMailID:=CCMailIDs, ReportPath:=MyFile, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
             SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

        Dim mModelMoniterModDetail As String = "Inspection Notification sent successfully to " + ToMailIDs.ToString.TrimEnd(",") + " by " + User.Identity.Name
        MarkLog(Util.Action.SendMail, "Inspection Master", mModelMoniterModDetail, Util.ErrorType.HandledError, mModelMonitorInsp.ID, EventLogID)

        '  MSGBoxCtrl.show("Mail!", "Mail Sent Successfully", "", MsgBoxStyle.OkOnly, "")
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

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Rpt = New crDetModelMonitorInsp
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 9
        RHCount = Me.mModelMonitorInsp.ModelMonitorInspPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Code/Form No.", _
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                 dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Code/Form No.", _
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "ATA Chapter", _
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                          CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "ATA Chapter", _
                                                   cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", lblReference.Text, _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", lblReference.Text, _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                                     "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Description", _
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Inspection Type", _
                                    cmbMonitorInspType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                       CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Inspection Type", _
                                     cmbMonitorInspType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Zone", _
                                    txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Zone", _
                                     txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Area", _
                                    txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Area", _
                                     txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 6 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Note", _
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Note", _
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 7 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Estd. Man Hours ", _
                                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Estd. Man Hours ", _
                                     txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 8 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "", _
                     "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                   CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Model Inspection Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

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