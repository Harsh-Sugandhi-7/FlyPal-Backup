'AJAX Conversion by Vikrant On 14-May-2015

Public Class wfPartMonitorService_Ajax
    Inherits System.Web.UI.Page

#Region " Variable declartion "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Public mPartMonitorService As PartMonitorService
    Public mPartMonitorServicePeriodUnitList As PartMonitorServicePeriodUnitList
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mPartMonitorServiceTypeList As PartMonitorServiceTypeList
    Dim Flag As Int16
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    ' Public Type As Boolean = False    'Code Added  Jan-10,2007
    Public mIssueDate As String
    Public mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mPrevCompMonitorServiceStatusForRevise As CompMonitorServiceStatus   'Revise Activity
    Public mIsSpareComp As Boolean = False  'Added by Shital on 30-Sep-2020 for SpareComp
    Dim mMPDTypeList As MPDTypeList 'Added by Saylee on 19-Apr-2023
    Dim mMPDSkillList As MPDSkillList 'Added by Saylee on 19-Apr-2023

    Dim mLastMPDRef As LastMPDAMPRef 'Added by Ajay on 20-07-2023
#End Region

#Region " Business Methdods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        mPartMonitorService = CType(Session("mPartMonitorService"), PartMonitorService)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mPartMonitorServicePeriodUnitList = CType(Session("mPartMonitorServicePeriodUnitList"), PartMonitorServicePeriodUnitList)
        '    Type = CType(Request.QueryString("Type"), Boolean)   'Code Added Jan-10,2007
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mCompMonitorServiceStatusList = CType(Session("mCompMonitorServiceStatusList"), tmpCompMonitorServiceStatusList)
        mIssueDate = Session("mIssueDate")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mPrevCompMonitorServiceStatusForRevise = Session("mPrevCompMonitorServiceStatusForRevise") 'Revise Activity
        mIsSpareComp = Session("IsSpareComp") 'Added by Shital on 30-Sep-2020 for SpareComp
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPartMonitorService") = mPartMonitorService
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mPartMonitorServicePeriodUnitList") = mPartMonitorServicePeriodUnitList
        '   Session("Type") = Type   'Code Added Jan-10,2007
        Session("mIssueDate") = mIssueDate

        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mPartMonitorServiceTypeList")
        Session.Remove("mPartMonitorService")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub SetObject()
        mPartMonitorService.Code = Trim(txtCode.Text)
        mPartMonitorService.Reference = Trim(txtReference.Text)
        mPartMonitorService.Description = Trim(txtDescription.Text)
        mPartMonitorService.Note = Trim(txtNote.Text)
        mPartMonitorService.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mPartMonitorService.PartMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue), Int32)
        mPartMonitorService.ShowInCofA = chkShowInCofA.Checked
        mPartMonitorService.RequiredManHours = txtRequiredManHours.Text.Trim
        'Added by Saylee on 23-July-2013 for BA22072013 
        mPartMonitorService.Zone = Trim(txtZone.Text)
        mPartMonitorService.Area = Trim(txtArea.Text)
        mPartMonitorService.IsRII = chkIsRII.Checked
        'End

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mPartMonitorService.IsAttachmentAdded = True
            Else
                mPartMonitorService.IsAttachmentAdded = False
            End If
            'Else
            '    .IsAttachmentAdded = False
        End If

        'Added by Saylee on 19-Apr-2023
        mPartMonitorService.TaskCardNo = txtTaskCardNo.Text.Trim
        mPartMonitorService.TaskHeading = txtTaskCardHeader.Text.Trim
        mPartMonitorService.Applicability = txtApplicability.Text.Trim
        mPartMonitorService.Source = txtSource.Text.Trim
        mPartMonitorService.Access = txtAccess.Text.Trim
        mPartMonitorService.MPDSkillID = Val(cmbSkillcode.SelectedValue.ToString)
        mPartMonitorService.MPDTypeID = Val(cmbMPDType.SelectedValue.ToString)
        mPartMonitorService.AccessOpenCloseManHours = txtAccessManHours.Text.Trim
        ''********************

        Session("mPartMonitorService") = mPartMonitorService
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mPartMonitorService.PartMonitorServicePeriods
            Dim I As Integer
            For I = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(I).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                .Item(I).FrequencyValue = Trim(txtFrequencyValue.Text)
            Next I
        End With
        Session("mPartMonitorService") = mPartMonitorService
    End Sub
    Private Sub AddSelectedPeroidUnits()
        Dim clnPartMonitorService As PartMonitorService = mPartMonitorService.Clone
        Try
            'Added by Saylee on 10-Feb-2020,  All27072020
            Dim mHourType As Integer = 0
            If mIsSpareComp = False Then  'Added by Shital on 30-Sep-2020 for All27072020
                If mAssemblyStatus.IsSpareAssembly = True Then
                    mHourType = mAssemblyStatus.HourType
                Else
                    mHourType = mMachine.HourType
                End If
            End If

            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mPartMonitorService.PartMonitorServicePeriods.Add(mPartMonitorService.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
                    mPartMonitorService.SetZeroFrequencyValue()
                End If
            Next
            Session("mPartMonitorService") = mPartMonitorService
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mPartMonitorService = clnPartMonitorService
            Session("mPartMonitorService") = mPartMonitorService
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnPartMonitorService = Nothing
            mSelectPeriodUnits = Nothing
            Session.Remove("mSelectPeriodUnits")
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mPartMonitorServicePeriodUnitList.Count - 1
            If Not mPartMonitorService.PartMonitorServicePeriods.Contains(mPartMonitorServicePeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mPartMonitorServicePeriodUnitList(i).ID, mPartMonitorServicePeriodUnitList(i).PeriodID, mPartMonitorServicePeriodUnitList(i).Name)
            End If
        Next
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub SetPage()

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD"
        Else
            ServiceMPDTitle = "Service"
        End If


        If mPartMonitorService.IsNew Then
            lblTitle.Text = "Part " + ServiceMPDTitle + " of [ Part: " & mPartMonitorService.Part.Name & "][New]"
        Else
            lblTitle.Text = "Part " + ServiceMPDTitle + " of [ Part: " & mPartMonitorService.Part.Name & "]"
        End If

        'Added By Saylee ON 4-Feb-2013 for BA04022013
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            lblReference.Text = "Task Source Reference"
        ElseIf AppSettings("ClientCode") = "Indamer" Then  'Added By Prashant 3-Apr-2013  'Indamer03042013
            lblReference.Text = "Task Code/Reference"
            txtReference.ToolTip = "Enter Task Code/Reference"
        Else
            lblReference.Text = "Reference Doc."
        End If
        lnkSpares.Enabled = Not mPartMonitorService.IsNew
        lnkTools.Enabled = Not mPartMonitorService.IsNew
        lnkTaskCards.Enabled = Not mPartMonitorService.IsNew
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

                Case MsgBoxResult.Cancel

                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
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

    Private Function Save() As Boolean
        Dim mPartMonitorServiceClone As PartMonitorService
        mPartMonitorServiceClone = CType(mPartMonitorService, PartMonitorService)
        SetObject()
        SetGridObject()
        If mPartMonitorService.IsValid = True Then
            If mPartMonitorService.PartMonitorServicePeriods.Count = 0 Then
                Dim ServiceMPDTitle As String = ""
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Part Service"
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save " + ServiceMPDTitle + "." + ServiceMPDTitle + " can not be saved without period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mPartMonitorService.ApplyEdit()
                mPartMonitorService = CType(mPartMonitorService.Save(), PartMonitorService)
                'Revise Activity
                Dim mMaintenanceKit As MaintenanceKit
                Dim mMaintenanceKitOld As MaintenanceKit
                Dim mMaintenanceTask, mMaintenanceTaskOld As MaintenanceTask
                If mPartMonitorService.ReviseRemark <> "" Then
                    Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount
                    mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorService.ID)
                    If mMaintenanceKitDetailsCount.MaintenanceSparesCount = 0 And mMaintenanceKitDetailsCount.MaintenanceTasksCount = 0 And mMaintenanceKitDetailsCount.MaintenanceToolsCount = 0 Then
                        mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompService(mPartMonitorService)

                        'mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mModelMonitorInsp.PrevRefID)
                        'mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorInsp.PrevRefID, False)
                        'Tools
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mPartMonitorService.PrevRefID, True)

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
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mPartMonitorService.PrevRefID, False)

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
                        mMaintenanceTaskOld = MaintenanceTask.GetMaintenanceTaskByParent(mPartMonitorService.PrevRefID)

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
                'Commented By Utkarsh On 27-Jul-2011 For All19072011
                '     MarkLog(Util.Action.Save, "PartMonitorSer", "ATAChapter->" + mPartMonitorService.ATAChapter + " -> " + " Part Name -> " + mPartMonitorService.Part.Name + " Part Monitor Service Type Name -> " + mPartMonitorService.PartMonitorServiceTypeName, Util.ErrorType.NoError, mPartMonitorService.ID)
                'End
                Session("mPartMonitorService") = mPartMonitorService
                Return True
            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
                mPartMonitorService = mPartMonitorServiceClone
                Session("mPartMonitorService") = mPartMonitorService
                Return False
            Finally
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                'MaintDetail = "Monitor Service Type : " + mPartMonitorService.PartMonitorServiceTypeName + " Description : " + mPartMonitorService.Description
                MaintDetail = "Part : " & mCompStatus.PartNameSerialNo & " Part Modification Type : " & mPartMonitorService.PartMonitorServiceTypeName & " Description : " & mPartMonitorService.Description
                MarkLog(Util.Action.Save, "Part Service", MaintDetail, Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub ControlVisibilty()
        btnPrint.Enabled = Not mPartMonitorService.IsNew
        btnAddPeriodUnit.Enabled = mPartMonitorServicePeriodUnitList.Count > 0

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorService.IsNew Then
            Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList = Session("mPartMonitorServiceConfiguredList")
            If Not mPartMonitorServiceConfiguredList Is Nothing Then
                If mPartMonitorServiceConfiguredList.Count > 0 Then
                    cmbMonitorServiceType.Enabled = False
                Else
                    cmbMonitorServiceType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mPartMonitorService.PartMonitorServicePeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mPartMonitorServiceConfiguredList.Count > 0 Then
                            txtFrequencyValue.Enabled = False
                        Else
                            txtFrequencyValue.Enabled = True
                        End If

                    Next i
                End With
            End If

        End If

        'If mPartMonitorServicePeriodUnitList.Count > 0 Then btnAddPeriodUnit.BackColor = Color.Gray
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
                    btnSaveSelect.Enabled = False
                    btnSaveSelect.ToolTip = "You are not Authorized user"
                End If
            ElseIf Not mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentServicePrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentServiceNew") Or User.IsInRole("MachineComponentServiceEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                    btnSaveSelect.Enabled = False
                    btnSaveSelect.ToolTip = "You are not Authorized user"
                End If
            End If
        End If
    End Sub
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount 'Revise Activity
        If mPartMonitorService.IsNew And mPartMonitorService.ReviseRemark <> "" Then
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorService.PrevRefID)
        Else
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorService.ID)
        End If

        lnkTools.Text = "Tools (" + mMaintenanceKitDetailsCount.MaintenanceToolsCount.ToString + " record(s))"
        lnkSpares.Text = "Spares (" + mMaintenanceKitDetailsCount.MaintenanceSparesCount.ToString + " record(s))"
        lnkTaskCards.Text = "Task Cards (" + mMaintenanceKitDetailsCount.MaintenanceTasksCount.ToString + " record(s))"
        upnlOtherDetails.DataBind()
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
                If (Not mPartMonitorService.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mPartMonitorService.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList("(SELECT)")
        cmbMonitorServiceType.DataSource = mPartMonitorServiceTypeList
        Session("mPartMonitorServiceTypeList") = mPartMonitorServiceTypeList
        mPartMonitorServicePeriodUnitList = PartMonitorServicePeriodUnitList.GetPartMonitorServicePeriodUnitList(mCompMonitorServiceStatus.CompStatusID)
        Session("mPartMonitorServicePeriodUnitList") = mPartMonitorServicePeriodUnitList
        dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods


        mMPDTypeList = MPDTypeList.GetTypeList(True)
        cmbMPDType.DataSource = mMPDTypeList

        mMPDSkillList = MPDSkillList.GetSkillList(True)
        cmbSkillcode.DataSource = mMPDSkillList

        'Added by Ajay 21-01-2023
        mLastMPDRef = LastMPDAMPRef.GetLastMPDAMPRefForModel(mMachine.AssemblyStatus.Assembly.ModelID)
        Session("mLastMPDRef") = mLastMPDRef
        If (mLastMPDRef.MPDNo = "") Then

        Else
            lblMPDNo.Text = "MPD No.: " + mLastMPDRef.MPDNo + ",Rev No.: " + mLastMPDRef.RevNo + ",Dated: " + mLastMPDRef.FromDateFormatted
        End If


        DataBind()

        'Added By Saylee on 10-Sep-2009, to set ATA chapter of the Component.
        If mPartMonitorService.ATAID.Equals(Guid.Empty) And mPartMonitorService.IsNew Then
            cmbATAChapter.SelectedValue = mCompStatus.ATAID.ToString
        End If

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorService.IsNew Then
            Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
            mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorService.PartID, mPartMonitorService.ID.ToString)
            Session("mPartMonitorServiceConfiguredList") = mPartMonitorServiceConfiguredList
        End If


    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        'If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtFrequencyValue As TextBox
        If Not mPartMonitorService.IsValid Then
            For i As Integer = 0 To mPartMonitorService.GetBrokenRulesCollection.Count - 1
                str = str + mPartMonitorService.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Items(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mPartMonitorService.PartMonitorServicePeriods(i).IsValid Then
                For j As Integer = 0 To mPartMonitorService.PartMonitorServicePeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorService.PartMonitorServicePeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mPartMonitorService.PartMonitorServicePeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mPartMonitorService.PartMonitorServicePeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorService.PartMonitorServicePeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
    Private Sub ControlVisibilityForAttachment()
        If mPartMonitorService.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 26-Jul-2011 For All19072011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                txtTaskCardNo.Focus()
            ElseIf txtCode.Enabled = True Then
                txtCode.Focus()
            End If
            AddSelectedPeroidUnits()
            DataFieldBind()
            ControlVisibilty()
            SetPage()
            SetRights() 'Added By Prashant 15-Mar-2011
            ControlVisibilityForAttachment()
            SetToolsSparesCount()
        End If

        '            Type = CType(Request.QueryString("Type"), Boolean)   'Code Added Jan-10,2007
        '           Session("Type") = Type                               'Code Added Jan-10,2007 


        If AppSettings("ClientCode") = "Heligo" Then
            lblZone.InnerText = "System"

        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
        If Save() Then
            ControlVisibilty()
            SetPage()
            SetToolsSparesCount()
            SetRights()
            upnlActionBtn.Update()
            upnlPeriods.Update()
            upnlOtherDetails.Update()
            upnlTitle.Update()
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub dgPeriods_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPeriods.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                'If (Not User.IsInRole("MachineDelete"))
                If (Not User.IsInRole("MachineDelete") Or Not User.IsInRole("ComponentInstallationDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Added by saylee on 1-Jun-2016
                Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
                mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorService.PartID, mPartMonitorService.ID.ToString)

                If mPartMonitorServiceConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mPartMonitorServiceConfiguredList.Count - 1
                        If i = mPartMonitorServiceConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mPartMonitorService.PartMonitorServicePeriods.Item(Index).PeriodUnitName + " frequency is configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If
                Index = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
                mPartMonitorService.PartMonitorServicePeriods.Remove(mPartMonitorService.PartMonitorServicePeriods.Item(Index).ID)
                Session("mPartMonitorService") = mPartMonitorService
                dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods
                dgPeriods.DataBind()
        End Select
    End Sub
    Private Sub btnAddPeriodUnit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPeriodUnit.Click
        SetPeroidUnits()
        SetGridObject()
        SetObject()

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorService.IsNew Then
            Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
            mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorService.PartID, mPartMonitorService.ID.ToString)

            If mPartMonitorServiceConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mPartMonitorServiceConfiguredList.Count - 1
                    If i = mPartMonitorServiceConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo + ","
                    End If
                Next

                Dim ServiceMPDTitle As String = ""
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Service"
                End If

                MSGBoxCtrl.Show("Alert!", ServiceMPDTitle + " is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow()", True)
        'Response.Redirect("wfSelectPeriodUnit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorService_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Part Service", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End

        RemoveSession()
        Session("EditMasterRecord") = "False"
        Session.Remove("mMaintenanceTaskAndKit")
        Session.Remove("mPrevCompMonitorServiceStatusForRevise") 'Revise Activity
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("GChildPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
    End Sub
    Private Sub imgbtnATAChapter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnATAChapter.Click
        SetObject()             'Added Code By Girish on May,25,2007 Due to combo getting refreshed
        'Response.Redirect("wfATA_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage3=wfPartMonitorService_Ajax.aspx")
    End Sub
    Private Sub btnSaveSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSelect.Click
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mIsSpareComp = False Then
            If mAssemblyStatus.IsSpareAssembly = True Then
                mHourType = mAssemblyStatus.HourType
            Else
                mHourType = mMachine.HourType
            End If
        End If

        '*********************

        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
        If Save() Then
            If Session("NewPage") = "True" Or mPartMonitorService.ReviseRemark <> "" Then 'Revise Activity

                mIssueDate = Session("mIssueDate")
                'Revise Activity
                If Not mPrevCompMonitorServiceStatusForRevise Is Nothing And mPartMonitorService.ReviseRemark <> "" Then
                    If mPrevCompMonitorServiceStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mIssueDate = mPrevCompMonitorServiceStatusForRevise.AsOnDateFormatted.ToString
                    Else
                        mIssueDate = mPrevCompMonitorServiceStatusForRevise.DoneOnFormatted.ToString
                    End If
                End If
                'End
                mPartMonitorService = PartMonitorService.GetPartMonitorService(mPartMonitorService.ID, mHourType)
                Session("mPartMonitorService") = mPartMonitorService
                'mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mIssueDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)
                If mIsSpareComp = False Then
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mIssueDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mHourType, mCompStatus)
                Else
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty, mIssueDate, mCompStatus.Comp.PartID, Guid.Empty, mCompStatus.ID, mCompStatus.HourType, mCompStatus)
                End If

                With mCompMonitorServiceStatus
                    .PartMonitorServiceID(True) = mPartMonitorService.ID
                    '.PartMonitorService.Code = mPartMonitorService.Code
                    .PartMonitorService.Reference = mPartMonitorService.Reference
                    .PartMonitorService.Description = mPartMonitorService.Description

                    .PartMonitorService.RequiredManHours = mPartMonitorService.RequiredManHours
                    '---------------------------------
                    '.PartMonitorService.PartMonitorServiceTypeID = mPartMonitorService.PartMonitorServiceTypeID
                End With
                'Revise Activity
                If Not mPrevCompMonitorServiceStatusForRevise Is Nothing Then
                    If mPrevCompMonitorServiceStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mCompMonitorServiceStatus.DoneOn = System.DBNull.Value
                    Else
                        mCompMonitorServiceStatus.DoneOn = mPrevCompMonitorServiceStatusForRevise.DoneOnFormatted.ToString
                    End If
                End If
                'End
                SetSession()
                Session("mIssueDate") = mIssueDate
                Session.Remove("Edit")
                Session.Remove("mPartMonitorServiceList")
                Session("FromPartMonitorServiceList") = True
                '====================
                Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
                Session.Remove("mMaintenanceTaskAndKit")
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")

                If AppSettings("ShowAllValuesPageEnable") = "True" Then
                    Session("MiddleFrame") = "wfComplyCompMonitorServiceStatusListShowValues_Ajax.aspx?" 'Revise Activity
                Else
                    Session("MiddleFrame") = "wfComplyCompMonitorServiceStatusList_Ajax.aspx?SpareComponent=" & IIf(mIsSpareComp = False, 0, 1) 'Revise Activity
                End If
                Response.Redirect("wfCompMonitorServiceStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            Else
                If Session("URLForCompInst") Is Nothing Then 'dont remove session as Part Service Count Required on wfCompMonitorServiceStatus_AJAX btnBack.Click
                    Session.Remove("mPartMonitorServiceList")
                Else
                    Session("StatusPageOpenFrom") = Request.QueryString("GChildPage2")
                    'Dim URLForPartServiceList As New Stack
                    'URLForPartServiceList.Push(Request.Url)
                    'Session("URLForPartServiceList") = URLForPartServiceList
                End If
                mCompMonitorServiceStatus.PartMonitorServiceID(False) = mPartMonitorService.ID
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
                Session.Remove("mMaintenanceTaskAndKit")
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Response.Redirect("wfCompMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            End If
        End If
        '------------------------------------------------------------
    End Sub
    Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mPartMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorService.ID)
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

        If mPartMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorService.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mPartMonitorService.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub cmbMonitorServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorServiceType.SelectedIndexChanged
        mPartMonitorService.PartMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue), Int32)
        dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mPartMonitorService.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub hdnBtnPeriodUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnPeriodUnit.Click
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        AddSelectedPeroidUnits()
        dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
    Private Sub lnkTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTools.Click
        If IsValid Then
            SetObject()



            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompService(mPartMonitorService)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorService.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 3
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorService_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkSpares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSpares.Click
        If IsValid Then
            SetObject()

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompService(mPartMonitorService)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorService.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            End If


            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 2
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorService_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkTaskCards_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTaskCards.Click
        If IsValid Then
            SetObject()

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompService(mPartMonitorService)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorService.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 1   'Added by Saylee on 23-July-2013 for BA22072013 

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorService_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnAddTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTools.Click
        SetToolsSparesCount()
        If Not mMaintenanceTaskAndKit Is Nothing Then
            If Session("mChild") = 1 Then
                mPartMonitorService.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            ElseIf Session("mChild") = 2 Then
                mPartMonitorService.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            ElseIf Session("mChild") = 3 Then
                mPartMonitorService.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

        End If
        Session("mPartMonitorService") = mPartMonitorService
        Session.Remove("mChild")
        upnlOtherDetails.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mPartMonitorService.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorService.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPartMonitorService.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
#End Region

#Region " Report "
    'Created By :- Pallavi , Date -10/08/2006

#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Rpt = New crDetPartMonitorService
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mPartMonitorService.PartMonitorServicePeriods.Count
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
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                 dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Code/Form No.",
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "ATA Chapter",
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                            CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).FrequencyValue, String)))

                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "ATA Chapter",
                                                    cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", lblReference.Text,
                                 txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                     CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).PeriodUnitName, String),
                     CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", lblReference.Text,
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                                     "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Description",
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                                   CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Description",
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", ServiceMPDTitle + " Type",
                                    cmbMonitorServiceType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                                   CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", ServiceMPDTitle + " Type",
                                  cmbMonitorServiceType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Note",
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                                CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "Note",
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                             "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "",
                     "", , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
              CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).PeriodUnitName, String),
              CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "",
                                         "", , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, ServiceMPDTitle + " Details", "",
                                         "", , , , , , , , , , , , , , , , , "Frequency of " + ServiceMPDTitle,
                                  CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mPartMonitorService.PartMonitorServicePeriods(I).FrequencyValue, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Par " + ServiceMPDTitle + " Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 27-Jul-2011 For All19072011
        '      MarkLog(Util.Action.Print, "PartMonitorSer", "Part Name -> " + mPartMonitorService.Part.Name + "PartMonitor Service Type -> " + mPartMonitorService.PartMonitorServiceTypeName, Util.ErrorType.HandledError, mPartMonitorService.ID)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region
End Class