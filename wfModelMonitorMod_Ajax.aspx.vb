Public Class wfModelMonitorMod_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'for Object
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mModelMonitorMod As ModelMonitorMod
    Public mModelMonitorModPeriods As ModelMonitorModPeriods
    Public Flag As Int16
    'For Combo
    Public mATAList As ATAList
    Public mModelMonitorModTypeList As ModelMonitorModTypeList
    Public mModelMonitorModPeriodUnitList As ModelMonitorModPeriodUnitList
    Dim mSelectPeriodUnits As SelectPeriodUnits
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
    'Added by vikrant on 27-July-2011
    Dim EventLogID As Guid
    Public mDirectiveDetail As String
    Public mModel As String
    'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
    Public mLinkMaintenanceActionList As LinkMaintenanceActionList
    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance
    'Added by Saylee on 8-Aug-2012    
    Public mMonitorType As String
    Public mDescrition As String
    Public mDetail As String
    'End
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mPrevAssemblyMonitorModStatusForRevise As AssemblyMonitorModStatus   'Revise Activity
    Dim email As Thread
	Dim mModuleList As ModuleList 'Added by shital on 02-Jul-2020 for Add EMailIDs field in csTransType 
	Dim mIssuingAuthorityTypeList As IssuingAuthorityTypeList
#End Region

#Region " Business Methods "
	Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mModelMonitorMod = CType(Session("mModelMonitorMod"), ModelMonitorMod)
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorModTypeList = CType(Session("mModelMonitorModTypeList"), ModelMonitorModTypeList)
        mModelMonitorModPeriodUnitList = CType(Session("mModelMonitorModPeriodUnitList"), ModelMonitorModPeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = Session("mMaintenanceTaskAndKit")
        mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
        mLinkMaintenanceActionList = Session("mLinkMaintenanceActionList") 'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
        mLinkMaintenanceList = Session("mLinkMaintenanceList") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mPrevAssemblyMonitorModStatusForRevise = Session("mPrevAssemblyMonitorModStatusForRevise") 'Revise Activity
        mModuleList = Session("mModuleList") 'Added by shital on 02-Jul-2020 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mModelMonitorMod") = mModelMonitorMod
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mATAList") = mATAList
        Session("mModelMonitorModTypeList") = mModelMonitorModTypeList
        Session("mModelMonitorModPeriodUnitList") = mModelMonitorModPeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList 'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorModTypeList")
        Session.Remove("mModelMonitorModPeriodUnitList")
        Session.Remove("mLinkMaintenanceActionList") 'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
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
    Private Sub SetObject()
        'mModelMonitorMod.Code = Trim(txtCode.Text)
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
                mModelMonitorMod.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
            Else
                mModelMonitorMod.Code = Trim(txtCode.Text)
            End If
        Else
            mModelMonitorMod.Code = Trim(txtCode.Text)
        End If
        mModelMonitorMod.Reference = Trim(txtReference.Text)
        mModelMonitorMod.Description = Trim(txtDescription.Text)
        mModelMonitorMod.Number = Trim(txtModificationNo.Text)
        If calIssueDate.Text = "" Then
            mModelMonitorMod.IssueDate = System.DBNull.Value
        Else
            mModelMonitorMod.IssueDate = calIssueDate.Text
        End If
        mModelMonitorMod.Note = Trim(txtNote.Text)
        mModelMonitorMod.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mModelMonitorMod.ModelMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
        mModelMonitorMod.Applicability = Trim(txtApplicability.Text)
        mModelMonitorMod.ComplianceRequirement = Trim(txtComplianceRequirement.Text)
        mModelMonitorMod.RequiredManHours = txtRequiredManHours.Text.Trim
        mModelMonitorMod.SupersededByADNumber = Trim(txtSupersededByADNumber.Text)
        mModelMonitorMod.Zone = Trim(txtZone.Text) 'Added by Saylee on 23-July-2013 for BA22072013 
        mModelMonitorMod.Area = Trim(txtArea.Text)
        mModelMonitorMod.IsRII = chkIsRII.Checked 'End
		mModelMonitorMod.RefAttachlink = txtRefAttachLink.Text    'Added by Shital on 07-FEb-2022
		mModelMonitorMod.IssuingAuthorityID = CType(Val(cmbIssuingAuthority.SelectedValue), Int32)
		mModelMonitorMod.IssuingAuthority = cmbIssuingAuthority.SelectedItem.Text

		If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorMod.IsAttachmentAdded = True
            Else
                mModelMonitorMod.IsAttachmentAdded = False
            End If
        End If
        Session("mModelMonitorMod") = mModelMonitorMod
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorMod.ModelMonitorModPeriods
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If .Item(i).PeriodID = 2 And Decimal.MaxValue < Val(txtFrequencyValue.Text.Trim) Then
                    .Item(i).FrequencyValue = ""
                Else
                    .Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
                End If
            Next i
        End With
    End Sub
    Private Sub SetPage()
        If mModelMonitorMod.IsNew Then
            lblTitle.Text = "Model Directive of [ Model: " & mModelMonitorMod.Model.Name & "] [New]"
        Else
            lblTitle.Text = "Model Directive of [ Model: " & mModelMonitorMod.Model.Name & "]"
        End If
        lblResult.Text = "List Of Linked Maintenance Activity : " & mLinkMaintenanceList.Count & " Record(s) found." 'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
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
        'btnAddPeriodUnit.Enabled = mModelMonitorModPeriodUnitList.Count > 0
        If Session("ModelIDFromModelCreation") = Nothing Then
            btnAddPeriodUnit.Enabled = mModelMonitorModPeriodUnitList.Count > 0 'Session("ModelIDFromModelCreation") = Nothing,'Added by Saylee on 14-Nov-2019
        Else
            btnAddPeriodUnit.Enabled = True
        End If
        btnPrint.Enabled = Not mModelMonitorMod.IsNew
        btnSendMail.Enabled = Not mModelMonitorMod.IsNew 'Added by Shital on 02-Jul-2020 for sendmail functionality
        If AppSettings("LinkMaintenance") = True Then 'Added By Utkarsh On 27-Jun-2012
            If Not mLinkMaintenanceList Is Nothing Then
                dgLinkedMaintenanceList.Columns(7).Visible = mLinkMaintenanceList.ShowDirectiveNo
            End If
        End If 'End

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorMod.IsNew Then
            Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList = Session("mModelMonitorModConfiguredList")
            If Not mModelMonitorModConfiguredList Is Nothing Then
                If mModelMonitorModConfiguredList.Count > 0 Then
                    cmbMonitorModType.Enabled = False
                Else
                    cmbMonitorModType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mModelMonitorMod.ModelMonitorModPeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mModelMonitorModConfiguredList.Count > 0 Then
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
        If Not Session("OpenFromADSBReviewMeeting") Is Nothing Then   'Added by Saylee on 28-Sep-2022 for Review Meeting
            btnSaveSelect.Visible = False
            btnSave.Visible = True
        End If

    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mModelMonitorMod.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub SaveLinkList() 'Added by Saylee on 17-Sep-2014 for ALL17092014 'This new function is called in "Save" function
        If dgLinkedMaintenanceList.Rows.Count > 0 Then
            SetLinkMaintenanceGridObject()
            Dim mLinkMaintenanceListClone As LinkMaintenanceList
            mLinkMaintenanceListClone = CType(mLinkMaintenanceList.Clone, LinkMaintenanceList)
            Try
                mLinkMaintenanceList = CType(mLinkMaintenanceList.Save, LinkMaintenanceList)
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Or ex.Number = 50000 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If
                mLinkMaintenanceList = mLinkMaintenanceListClone
                Session("mLinkMaintenanceList") = mLinkMaintenanceList
            End Try
            'Added By Saylee ON 29-May-2012 FOR Link Maintenance
            dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
            dgLinkedMaintenanceList.DataBind()
            upnlLinkedMaintenanceList.Update()
            upnlLinkMaint.Update()
        End If
    End Sub
    Private Function Save() As Boolean
        Dim mModelMonitorModClone As ModelMonitorMod
        mModelMonitorModClone = CType(mModelMonitorMod, ModelMonitorMod)
        SetObject()
        SetGridObject()
        If mModelMonitorMod.IsValid = True Then
            If mModelMonitorMod.ModelMonitorModPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "Model Directive cannot be saved without period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mModelMonitorMod.ApplyEdit()
                mModelMonitorMod = CType(mModelMonitorMod.Save(), ModelMonitorMod)
                'Revise Activity
                Dim mMaintenanceKit As MaintenanceKit
                Dim mMaintenanceKitOld As MaintenanceKit
                Dim mMaintenanceTask, mMaintenanceTaskOld As MaintenanceTask
                If mModelMonitorMod.ReviseRemark <> "" Then
                    Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount
                    mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorMod.ID)
                    If mMaintenanceKitDetailsCount.MaintenanceSparesCount = 0 And mMaintenanceKitDetailsCount.MaintenanceTasksCount = 0 And mMaintenanceKitDetailsCount.MaintenanceToolsCount = 0 Then
                        mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)

                        'mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mModelMonitorMod.PrevRefID)
                        'mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorMod.PrevRefID, False)
                        'Tools
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorMod.PrevRefID, True)

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
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorMod.PrevRefID, False)

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
                        mMaintenanceTaskOld = MaintenanceTask.GetMaintenanceTaskByParent(mModelMonitorMod.PrevRefID)

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
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorMod") = mModelMonitorMod
                Return True
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Or ex.Number = 50000 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If
                mModelMonitorMod = mModelMonitorModClone
                Session("mModelMonitorMod") = mModelMonitorMod
                Return False
            Finally
                'Added by Vikrant on 1-Aug-2011
                mModel = mModelMonitorMod.Model.Name
                mDirectiveDetail = "Model : " & mModel & " Model Directive Type : " & mModelMonitorMod.ModelMonitorModTypeName & " Directive No: " & mModelMonitorMod.Number & " Description : " & mModelMonitorMod.Description
                MarkLog(Util.Action.Save, "Model Directive", mDirectiveDetail, Util.ErrorType.NoError, mModelMonitorMod.ID, EventLogID)
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

        Dim clnModelMonitorMod As ModelMonitorMod = mModelMonitorMod.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mModelMonitorMod.ModelMonitorModPeriods.Add(mModelMonitorMod.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
                End If
            Next
            For I As Integer = 0 To mModelMonitorMod.ModelMonitorModPeriods.Count - 1
                mModelMonitorMod.ModelMonitorModPeriods(I).MonitorTypeID = mModelMonitorModTypeList(mModelMonitorMod.ModelMonitorModTypeID).MonitorTypeID
                If mModelMonitorModTypeList(mModelMonitorMod.ModelMonitorModTypeID).MonitorTypeID = 3 Then
                    mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorMod") = mModelMonitorMod
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mModelMonitorMod = clnModelMonitorMod
            Session("mModelMonitorMod") = mModelMonitorMod
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.Show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorMod = Nothing
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        End Try
    End Sub
    Private Sub SetPeroidUnits()


        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
            For i As Integer = 0 To mModelMonitorModPeriodUnitList.Count - 1
                If Not mModelMonitorMod.ModelMonitorModPeriods.Contains(mModelMonitorModPeriodUnitList(i).ID) Then
                    mSelectPeriodUnits.Add(mModelMonitorModPeriodUnitList(i).ID, mModelMonitorModPeriodUnitList(i).PeriodID, mModelMonitorModPeriodUnitList(i).Name)
                End If
            Next
        Else
            'Added by Saylee on 14-Nov-2019
            Dim i As Int32
            Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
            While i <= mPeriodUnitList.Count - 1
                If mModelMonitorMod.ModelMonitorModPeriods.Contains(mPeriodUnitList(i).ID) = False Then
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
        mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, mModelMonitorMod.ModelID, mHourType, ID)
        Session("mModelMonitorMod") = mModelMonitorMod
        lblTitle.Text = "Model Directive of [ Model: " & mModelMonitorMod.Model.Name & " ][New]"
    End Sub
    Private Sub UpdatePanel()
        upnlMonitorDirectiveDetails.Update()
        upnlATAMaster.Update()
        upnlMonitorDirectiveType.Update()
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
    Private Sub SetRights()
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyModificationPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
                btnSaveSelect.Enabled = False
                btnSaveSelect.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyModificationPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
                btnSaveSelect.Enabled = False
                btnSaveSelect.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount 'Revise Activity
        If mModelMonitorMod.IsNew And mModelMonitorMod.ReviseRemark <> "" Then
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorMod.PrevRefID)
        Else
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorMod.ID)
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
                If (Not mModelMonitorMod.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mModelMonitorMod.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "<SELECT>")
        Session("mATAList") = mATAList

        If Session("ModelIDFromModelCreation") = Nothing Then 'added by saylee on 14-Nov-2019
            mModelMonitorModPeriodUnitList = ModelMonitorModPeriodUnitList.GetModelMonitorModPeriodUnitList(mAssemblyMonitorModStatus.AssemblyStatusID)
        End If

        Session("mModelMonitorModPeriodUnitList") = mModelMonitorModPeriodUnitList
        cmbATAChapter.DataSource = mATAList
        cmbMonitorModType.DataSource = mModelMonitorModTypeList
        dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods
        calIssueDate.Text = mModelMonitorMod.IssueDateFormatted.ToString 'Added on 28-05-2007 by Saylee
        mLinkMaintenanceActionList = LinkMaintenanceActionList.GetLinkMaintActionList(True) 'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
        If mLinkMaintenanceList Is Nothing Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorMod.ID.ToString)
        End If
        dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
        lnkLinkMaint1.Text = "Click to add Link Maintenance Activity " + "(" + mLinkMaintenanceList.Count.ToString + " activite(s))"
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList
		Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End

		mIssuingAuthorityTypeList = IssuingAuthorityTypeList.GetIssuingAuthorityTypeList(IsSelectTagRequired:=True)
		cmbIssuingAuthority.DataSource = mIssuingAuthorityTypeList
		'Added by saylee on 1-Jun-2016
		If Not mModelMonitorMod.IsNew Then
			Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
			mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mModelMonitorMod.ModelID, mModelMonitorMod.ID.ToString)
			Session("mModelMonitorModConfiguredList") = mModelMonitorModConfiguredList
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
        ElseIf custValidator.ControlToValidate = "cmbMonitorModType" Then
            If cmbMonitorModType.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
            'ElseIf custValidator.ControlToValidate = "txtDescription" Then
            '    If Len(txtDescription.Text) > 1000 Then
            '        custValidator.ErrorMessage = "Description can't be more than 1000 chars."
            '        e.IsValid = False
            '    End If
        ElseIf custValidator.ControlToValidate = "txtReference" Then
            If Len(txtReference.Text) > 500 Then
                custValidator.ErrorMessage = "Reference Too Long"
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note can't be more than 1000 chars."
                e.IsValid = False
            End If
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetObject()
        SetGridObject()
        Dim str As String = ""
        If Not mModelMonitorMod.IsValid Then
            For i As Integer = 0 To mModelMonitorMod.GetBrokenRulesCollection.Count - 1
                str = str + mModelMonitorMod.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For counter As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            If Not mModelMonitorMod.ModelMonitorModPeriods.Item(counter).IsValid Then
                For i As Integer = 0 To mModelMonitorMod.ModelMonitorModPeriods(counter).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorMod.ModelMonitorModPeriods.Item(counter).GetBrokenRulesCollection(i).Description + "<BR>"
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
            If Not mModelMonitorMod.ModelMonitorModPeriods.Item(i).IsValid Then
                For x As Integer = 0 To mModelMonitorMod.ModelMonitorModPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorMod.ModelMonitorModPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
    Public Sub CustomValidate3(ByVal s As Object, ByVal e As ServerValidateEventArgs) 'Added By Utkarsh ON 11-Jan-2012 FOR Link Maintenance
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
        'Added by vikrant on 27-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtCode.Enabled = True Then
                setFocus(txtCode)
            End If
            If txtCode.Enabled = True And AppSettings("ShowMaintenanceForNewClients") = "False" Then
                setFocus(txtCode)
                cmbMonitorType.Items.Add(New ListItem("Service", "1"))
                cmbMonitorType.Items.Add(New ListItem("Inspection", "2"))
                cmbMonitorType.Items.Add(New ListItem("Directive", "3"))
            Else

                cmbMonitorType.Items.Add(New ListItem("MPD", "1"))
                cmbMonitorType.Items.Add(New ListItem("Directive", "3"))
            End If
            mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("<SELECT>")
            Session("mModelMonitorModTypeList") = mModelMonitorModTypeList
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
    Private Sub dgPeriods_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPeriods.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
                If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
                    If mAssemblyStatus.IsMaster Then 'Added By Utkarsh On 15-Mar-2011
                        If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationDelete")) = False Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    ElseIf Not mAssemblyStatus.IsMaster Then
                        If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationDelete")) = False Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    End If '*******************************
                End If
                'Added by saylee on 1-Jun-2016
                Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
                mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mModelMonitorMod.ModelID, mModelMonitorMod.ID.ToString)

                If mModelMonitorModConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mModelMonitorModConfiguredList.Count - 1
                        If i = mModelMonitorModConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorMod.ModelMonitorModPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If

                Dim mPeriodName As String = mModelMonitorMod.ModelMonitorModPeriods.Item(Index).PeriodUnitName
                mModelMonitorMod.ModelMonitorModPeriods.Remove(mModelMonitorMod.ModelMonitorModPeriods.Item(Index).ID)
                Session("mModelMonitorMod") = mModelMonitorMod
                'Added by vikrant on 27-July-2011
                mDirectiveDetail = "Period : " + mPeriodName
                MarkLog(Util.Action.Remove, "Model Directive", mDirectiveDetail, Util.ErrorType.NoError, mModelMonitorMod.ID, EventLogID)
                dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods
                dgPeriods.DataBind()
                upnlPeriods.Update()
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
        If Save() Then
            ControlVisibility()
            SetPage()
            UpdatePanel()
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub imgbtnATAChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnATAChapter.Click
        RemoveSession()
    End Sub
    Private Sub btnAddPeriodUnit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriodUnit.Click
        SetObject()
        SetPeroidUnits()
        SetGridObject()

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorMod.IsNew Then
            Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
            mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mModelMonitorMod.ModelID, mModelMonitorMod.ID.ToString)

            If mModelMonitorModConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorModConfiguredList.Count - 1
                    If i = mModelMonitorModConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.Show("Alert!", "Directive is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow()", True)
    End Sub
    Private Sub cmbMonitorModType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMonitorModType.SelectedIndexChanged
        mModelMonitorMod.ModelMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If cmbMonitorModType.SelectedIndex > 0 Then
                Dim mMaxCodeMaintActTypewiseForModel As MaxCodeMaintActTypewiseForModel
                mMaxCodeMaintActTypewiseForModel = MaxCodeMaintActTypewiseForModel.GetCode(mModelMonitorMod.ModelID, 7, CInt(cmbMonitorModType.SelectedValue))
                If Int32.TryParse(mMaxCodeMaintActTypewiseForModel.Code, Nothing) Then
                    Dim TempCode As String = (CInt(mMaxCodeMaintActTypewiseForModel.Code) + 1).ToString
                    If TempCode.Length < 3 Then
                        mModelMonitorMod.Code = TempCode.PadLeft(3, "0"c)
                    Else
                        mModelMonitorMod.Code = TempCode
                    End If
                    txtCode.DataBind()
                Else
                    mModelMonitorMod.Code = ""
                    txtCode.DataBind()
                End If
            Else
                mModelMonitorMod.Code = ""
                txtCode.DataBind()
            End If
            upnlMonitorDirectiveDetails.Update()
        End If
        'End
        dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods
        dgPeriods.DataBind()
        REM: for ReadOnlyFrequencyColumn
        For i As Integer = 0 To dgPeriods.Rows.Count - 1
            Dim txtFreqVal As TextBox = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            txtFreqVal.ReadOnly = mModelMonitorMod.ReadOnlyFrequencyColumn
        Next
        If cmbMonitorModType.Enabled = True Then
            setFocus(cmbMonitorModType)
        End If
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Session("EditMasterRecord") = "False"
        Session.Remove("mMaintenanceTaskAndKit")
        'Added by vikrant on 27-July-2011
        Session.Remove("mPrevAssemblyMonitorModStatusForRevise") 'Revise Activity
        MarkLog(Util.Action.Close, "Model Directive", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
    End Sub
    Private Sub btnSaveSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSelect.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub

        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************

        If Save() Then
            Session("mModelMonitorMod") = mModelMonitorMod
            mModelMonitorMod = CType(Session("mModelMonitorMod"), ModelMonitorMod)
            If Session("NewPage") = "True" Or mModelMonitorMod.ReviseRemark <> "" Then 'Revise Activity
                'Revise Activity
                If Not mPrevAssemblyMonitorModStatusForRevise Is Nothing And mModelMonitorMod.ReviseRemark <> "" Then
                    If mPrevAssemblyMonitorModStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mAssemblyMonitorModStatus.AsOnDate = mPrevAssemblyMonitorModStatusForRevise.AsOnDateFormatted.ToString
                    Else
                        mAssemblyMonitorModStatus.AsOnDate = mPrevAssemblyMonitorModStatusForRevise.DoneOnFormatted.ToString
                    End If

                End If                'End
                mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mModelMonitorMod.ID, mHourType)
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mModelMonitorMod.IssueDate, mAssemblyStatus.Assembly.ModelID, mHourType)
                With mAssemblyMonitorModStatus
                    .ModelMonitorModID(True) = mModelMonitorMod.ID
                    .ModelMonitorMod.Reference = mModelMonitorMod.Reference
                    .ModelMonitorMod.Description = mModelMonitorMod.Description
                    .ModelMonitorMod.IssueDate = mModelMonitorMod.IssueDate
                    .ModelMonitorMod.RequiredManHours = mModelMonitorMod.RequiredManHours
                End With
                'Revise Activity
                If Not mPrevAssemblyMonitorModStatusForRevise Is Nothing Then
                    If mPrevAssemblyMonitorModStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mAssemblyMonitorModStatus.DoneOn = System.DBNull.Value
                    Else
                        mAssemblyMonitorModStatus.DoneOn = mPrevAssemblyMonitorModStatusForRevise.DoneOnFormatted.ToString
                        mAssemblyMonitorModStatus.AsOnDate = mPrevAssemblyMonitorModStatusForRevise.DoneOnFormatted.ToString
                    End If
                End If
                'End
                SetSession()
                Session.Remove("Edit")
                Session.Remove("mModelMonitorModList")
                Session("FromModelMonitorModList") = True
                Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
                Session.Remove("mMaintenanceTaskAndKit")
                Session("mIssueDate") = calIssueDate.Text
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                'Response.Redirect("wfAssemblyMonitorModStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                Dim str As String

                If AppSettings("ShowAllValuesPageEnable") = "True" Then
                    Session("MiddleFrame") = "wfComplyAssemblyMonitorModStatusListShowValues_Ajax.aspx?" 'Revise Activity
                Else
                    If Not Session("mIsSpareAssembly") Is Nothing Then 'Added By Vikrant On 27-Jul-2020 For ALL27072020
                        Session("MiddleFrame") = "wfComplyAssemblyMonitorModStatusList_Ajax.aspx?SpareAssembly=" & Session("mIsSpareAssembly")
                        'End
                    Else 'Existing condition
                        Session("MiddleFrame") = "wfComplyAssemblyMonitorModStatusList_Ajax.aspx?SpareAssembly=0" 'Revise Activity
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
                    str = "openledgersame('wfAssemblyMonitorModStatusNew_Ajax.aspx?BackPage=Index.aspx');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
                End If



            Else
                With mAssemblyMonitorModStatus
                    .ModelMonitorModID(False) = mModelMonitorMod.ID
                    .ModelMonitorMod.Reference = mModelMonitorMod.Reference
                    .ModelMonitorMod.Description = mModelMonitorMod.Description
                    .ModelMonitorMod.IssueDate = mModelMonitorMod.IssueDate
                End With

                SetSession()
                Session.Remove("Edit")
                Session.Remove("mModelMonitorModList")
                Session("FromModelMonitorModList") = True
                Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
                'Added By Saylee ON 29-May-2012 FOR Link Maintenance
                Session.Remove("mLinkMaintenanceActionList")
                Session.Remove("mLinkMaintenanceList")
                Session.Remove("URL")
                Session.Remove("MaintenanceActivityID")
                'End
                Session.Remove("mMaintenanceTaskAndKit")
                'Response.Redirect("wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                Dim str As String
                If Session("IsOpenFromMaster") = True Then
                    Session.Remove("IsOpenFromMaster")
                    If mAssemblyStatus.IsSpareAssembly = True Then  'Added by Saylee on 10-Feb-2020,  All27072020
                        str = "openledgersame('wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfSpareAssemblyStatus.aspx');"
                    Else
                        str = "openledgersame('wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfAssemblyStatus_Ajax.aspx');"
                    End If
                Else
                    str = "openledgersame('wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=Index.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx');"
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            End If
        End If
    End Sub
    Private Sub btnAddNewLinkMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewLinkMaintenance.Click
        Dim URL As Stack = New Stack    'STACK to store url of current page
        URL.Push(Request.Url)           'Inserting URL in STACK
        Session("URL") = URL
        Session("MaintenanceActivityID") = mModelMonitorMod.ID
        Session("ModelIDForMPD") = mModelMonitorMod.ModelID  'Added By Vikrant For MPD
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
            SetObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorMod.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)
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
            SetObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorMod.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)
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
            SetObject()
            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorMod.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)
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
        dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods
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
        mModelMonitorMod.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mModelMonitorMod.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mModelMonitorMod.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mModelMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
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

        If mModelMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mModelMonitorMod.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub


    'Added by Shital on 01-JUl-2020
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click

        Session("UserEmailID") = mModuleList.Item("AssemblyModifications").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AssemblyModifications").SendCCMailID
        Session("SmtpHost") = mModuleList.Item("AssemblyModifications").SmtpHost
        Session("SmtpPort") = mModuleList.Item("AssemblyModifications").SmtpPort
        Session("SmtpUser") = mModuleList.Item("AssemblyModifications").SmtpUser
        Session("SmtpPassword") = mModuleList.Item("AssemblyModifications").SmtpPassword
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

        'ToMailIDs = mModuleList.Item("AssemblyModifications").SendToMailID
        'CCMailIDs = mModuleList.Item("AssemblyModifications").SendCCMailID

        'Added by Saylee on 7-Sep-2020 for APFT07092020
        Dim ModName As String = "AD/SB(s)"
        If AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then
            ModName = "ACMD"
        End If
        '*************************

        ToMailIDs = Session("ToSendMailIDs")
        CCMailIDs = Session("CcSendMailIDs")

        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "</br> ")

        str = str + ("<p><font face=""Calibri"">")
        str = str + mCompanyDetail.CompanyName
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        'str = str + " Following Assembly Modification Added  in FlyPal System and need your attentions."
        str = str + " Following Assembly " + ModName + " Added  in FlyPal System and need your attentions." 'Added By Prashant On 5-Oct-2020 APFT05102020
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + "Please Login to FlyPal® for detailed information."
        str = str + ("</font></p>")


        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Directive Type: " + "</b>" + cmbMonitorModType.SelectedItem.Text.ToString + "</p><p><b>Directive No: " + "</b>" + mModelMonitorMod.Number)
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Description: " + "</b>" + mModelMonitorMod.Description + "</p><p><b>Issue Date: " + "</b>" + mModelMonitorMod.IssueDateFormatted)
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Reference: " + "</b>" + mModelMonitorMod.Reference.ToString)
        str = str + ("</font></p>")

        'Added by shital on 30-Oct-2020
        Dim MyFile As String
        If mModelMonitorMod.IsAttachmentAdded = True Then
            If mModelMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
                mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
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

        SendMailFile.SendMailFile(, User.Identity.Name, ModName, Info:=str, ToMailID:=ToMailIDs.ToString, CCMailID:=CCMailIDs, ReportPath:=MyFile, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
             SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

        Dim mModelMoniterModDetail As String = ModName + " Notification sent successfully to " + ToMailIDs.ToString.TrimEnd(",") + " by " + User.Identity.Name
        MarkLog(Util.Action.SendMail, "Modification Master", mModelMoniterModDetail, Util.ErrorType.HandledError, mModelMonitorMod.ID, EventLogID)

        'MSGBoxCtrl.show("Mail!", "Mail Sent Successfully", "", MsgBoxStyle.OkOnly, "")
    End Sub
    'End
#End Region

#Region " Report "
    'Created By :- Pallavi , Date -10/08/2006
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Rpt = New crDetModelMonitorMod
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 14
        RHCount = Me.mModelMonitorMod.ModelMonitorModPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Code/Form No.", _
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                  dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Code/Form No.", _
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "ATA Chapter", _
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "ATA Chapter", _
                                                   cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Directive Type", _
                                    cmbMonitorModType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Directive Type", _
                                     cmbMonitorModType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Directive No.", _
                                    txtModificationNo.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Directive No.", _
                                     txtModificationNo.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                         CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Description", _
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", lblReference.Text, _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                          CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", lblReference.Text, _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                                     "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Effective Date", _
                                   New SmartDate(calIssueDate.Text).FormattedText, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Effective Date", _
                                     New SmartDate(calIssueDate.Text).FormattedText, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 6 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Superseded AD Number", _
                                   txtSupersededByADNumber.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Superseded AD Number", _
                                     txtSupersededByADNumber.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 7 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Applicability", _
                                   txtApplicability.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Applicability", _
                                     txtApplicability.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 8 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Method of Compliance", _
                                   txtComplianceRequirement.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Method of Compliance", _
                                     txtComplianceRequirement.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 9 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Zone", _
                                   txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Zone", _
                                     txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 10 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Area", _
                                   txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Area", _
                                     txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 11 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Note", _
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                           CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Note", _
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 12 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Estd. Man Hours", _
                                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                           CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "Estd. Man Hours", _
                                     txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            ElseIf I = 13 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "", _
                     "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Directives Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Directives Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Directives", _
                               CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValueFormatted, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Model Directive Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

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