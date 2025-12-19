'AJAX Conversion by Saylee On 21-May-2015

Public Class wfPartMonitorInsp_Ajax
    Inherits System.Web.UI.Page

#Region " Variable declartion "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mCompMonitorInspStatus As CompMonitorInspStatus
    Public mPartMonitorInsp As PartMonitorInsp
    Public mPartMonitorInspPeriodUnitList As PartMonitorInspPeriodUnitList
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mPartMonitorInspTypeList As PartMonitorInspTypeList
    Dim Flag As Int16
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    ' Public Type As Boolean = False    'Code Added  Jan-10,2007
    Public mIssueDate As String
    Public mCompMonitorInspStatusList As tmpCompMonitorInspStatusList

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mPrevCompMonitorInspStatusForRevise As CompMonitorInspStatus   'Revise Activity
    Public mIsSpareComp As Boolean = False  'Added by Shital on 30-Sep-2020 for SpareComp
#End Region

#Region " Business Methdods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorInspStatus = CType(Session("mCompMonitorInspStatus"), CompMonitorInspStatus)
        mPartMonitorInsp = CType(Session("mPartMonitorInsp"), PartMonitorInsp)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mPartMonitorInspPeriodUnitList = CType(Session("mPartMonitorInspPeriodUnitList"), PartMonitorInspPeriodUnitList)
        '    Type = CType(Request.QueryString("Type"), Boolean)   'Code Added Jan-10,2007
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mCompMonitorInspStatusList = CType(Session("mCompMonitorInspStatusList"), tmpCompMonitorInspStatusList)
        mIssueDate = Session("mIssueDate")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mPrevCompMonitorInspStatusForRevise = Session("mPrevCompMonitorInspStatusForRevise") 'Revise Activity
        mIsSpareComp = Session("IsSpareComp") 'Added by Shital on 30-Sep-2020 for SpareComp
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mPartMonitorInsp") = mPartMonitorInsp
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mPartMonitorInspPeriodUnitList") = mPartMonitorInspPeriodUnitList
        '   Session("Type") = Type   'Code Added Jan-10,2007
        Session("mIssueDate") = mIssueDate

        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mPartMonitorInspTypeList")
        Session.Remove("mPartMonitorInsp")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub SetObject()
        mPartMonitorInsp.Code = Trim(txtCode.Text)
        mPartMonitorInsp.Reference = Trim(txtReference.Text)
        mPartMonitorInsp.Description = Trim(txtDescription.Text)
        mPartMonitorInsp.Note = Trim(txtNote.Text)
        mPartMonitorInsp.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mPartMonitorInsp.PartMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        mPartMonitorInsp.ShowInCofA = chkShowInCofA.Checked
        mPartMonitorInsp.RequiredManHours = txtRequiredManHours.Text.Trim
        'Added by Saylee on 23-July-2013 for BA22072013 
        mPartMonitorInsp.Zone = Trim(txtZone.Text)
        mPartMonitorInsp.Area = Trim(txtArea.Text)
        mPartMonitorInsp.IsRII = chkIsRII.Checked
        'End

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mPartMonitorInsp.IsAttachmentAdded = True
            Else
                mPartMonitorInsp.IsAttachmentAdded = False
            End If
            'Else
            '    .IsAttachmentAdded = False
        End If

        Session("mPartMonitorInsp") = mPartMonitorInsp
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mPartMonitorInsp.PartMonitorInspPeriods
            Dim I As Integer
            For I = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(I).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                .Item(I).FrequencyValue = Trim(txtFrequencyValue.Text)
            Next I
        End With
        Session("mPartMonitorInsp") = mPartMonitorInsp
    End Sub
    Private Sub AddSelectedPeroidUnits()
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

        Dim clnPartMonitorInsp As PartMonitorInsp = mPartMonitorInsp.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mPartMonitorInsp.PartMonitorInspPeriods.Add(mPartMonitorInsp.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
                    mPartMonitorInsp.SetZeroFrequencyValue()
                End If
            Next
            Session("mPartMonitorInsp") = mPartMonitorInsp
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mPartMonitorInsp = clnPartMonitorInsp
            Session("mPartMonitorInsp") = mPartMonitorInsp
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnPartMonitorInsp = Nothing
            mSelectPeriodUnits = Nothing
            Session.Remove("mSelectPeriodUnits")
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mPartMonitorInspPeriodUnitList.Count - 1
            If Not mPartMonitorInsp.PartMonitorInspPeriods.Contains(mPartMonitorInspPeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mPartMonitorInspPeriodUnitList(i).ID, mPartMonitorInspPeriodUnitList(i).PeriodID, mPartMonitorInspPeriodUnitList(i).Name)
            End If
        Next
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub SetPage()
        If mPartMonitorInsp.IsNew Then
            lblTitle.Text = "Part Inspection of [ Part: " & mPartMonitorInsp.Part.Name & "][New]"
        Else
            lblTitle.Text = "Part Inspection of [ Part: " & mPartMonitorInsp.Part.Name & "]"
        End If

        'Added By Saylee ON 4-Feb-2013 for BA04022013
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            lblReference.Text = "Task Source Reference"
        ElseIf AppSettings("ClientCode") = "Indamer" Then  'Added By Prashant 3-Apr-2013  'Indamer03042013
            lblReference.Text = "Task Code/Reference"
            txtReference.ToolTip = "Enter Task Code/Reference"
        Else
            lblReference.Text = "Reference"
        End If
        lnkSpares.Enabled = Not mPartMonitorInsp.IsNew
        lnkTools.Enabled = Not mPartMonitorInsp.IsNew
        lnkTaskCards.Enabled = Not mPartMonitorInsp.IsNew
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

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

    Private Function Save() As Boolean
        Dim mPartMonitorInspClone As PartMonitorInsp
        mPartMonitorInspClone = CType(mPartMonitorInsp, PartMonitorInsp)
        SetObject()
        SetGridObject()
        If mPartMonitorInsp.IsValid = True Then
            If mPartMonitorInsp.PartMonitorInspPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Part Inspection.Part Inspection can not be saved without period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mPartMonitorInsp.ApplyEdit()
                mPartMonitorInsp = CType(mPartMonitorInsp.Save(), PartMonitorInsp)
                'Revise Activity
                Dim mMaintenanceKit As MaintenanceKit
                Dim mMaintenanceKitOld As MaintenanceKit
                Dim mMaintenanceTask, mMaintenanceTaskOld As MaintenanceTask
                If mPartMonitorInsp.ReviseRemark <> "" Then
                    Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount
                    mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorInsp.ID)
                    If mMaintenanceKitDetailsCount.MaintenanceSparesCount = 0 And mMaintenanceKitDetailsCount.MaintenanceTasksCount = 0 And mMaintenanceKitDetailsCount.MaintenanceToolsCount = 0 Then
                        mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompInsp(mPartMonitorInsp)

                        'mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mModelMonitorInsp.PrevRefID)
                        'mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mModelMonitorInsp.PrevRefID, False)
                        'Tools
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mPartMonitorInsp.PrevRefID, True)

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
                        mMaintenanceKitOld = MaintenanceKit.GetMaintenanceKitByParent(mPartMonitorInsp.PrevRefID, False)

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
                        mMaintenanceTaskOld = MaintenanceTask.GetMaintenanceTaskByParent(mPartMonitorInsp.PrevRefID)

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
                '     MarkLog(Util.Action.Save, "PartMonitorSer", "ATAChapter->" + mPartMonitorInsp.ATAChapter + " -> " + " Part Name -> " + mPartMonitorInsp.Part.Name + " Part Monitor Insp Type Name -> " + mPartMonitorInsp.PartMonitorInspTypeName, Util.ErrorType.NoError, mPartMonitorInsp.ID)
                'End
                Session("mPartMonitorInsp") = mPartMonitorInsp
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
                mPartMonitorInsp = mPartMonitorInspClone
                Session("mPartMonitorInsp") = mPartMonitorInsp
                Return False
            Finally
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                'MaintDetail = "Monitor Insp Type : " + mPartMonitorInsp.PartMonitorInspTypeName + " Description : " + mPartMonitorInsp.Description
                MaintDetail = "Part : " & mCompStatus.PartNameSerialNo & " Part Modification Type : " & mPartMonitorInsp.PartMonitorInspTypeName & " Description : " & mPartMonitorInsp.Description
                MarkLog(Util.Action.Save, "Part Inspection", MaintDetail, Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub ControlVisibilty()
        btnPrint.Enabled = Not mPartMonitorInsp.IsNew
        btnAddPeriodUnit.Enabled = mPartMonitorInspPeriodUnitList.Count > 0
        'If mPartMonitorInspPeriodUnitList.Count > 0 Then btnAddPeriodUnit.BackColor = Color.Gray

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorInsp.IsNew Then
            Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList = Session("mPartMonitorInspConfiguredList")
            If Not mPartMonitorInspConfiguredList Is Nothing Then
                If mPartMonitorInspConfiguredList.Count > 0 Then
                    cmbMonitorInspType.Enabled = False
                Else
                    cmbMonitorInspType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mPartMonitorInsp.PartMonitorInspPeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mPartMonitorInspConfiguredList.Count > 0 Then
                            txtFrequencyValue.Enabled = False
                        Else
                            txtFrequencyValue.Enabled = True
                        End If

                    Next i
                End With
            End If

        End If
    End Sub
    Private Sub SetRights()
        If mIsSpareComp = False Then 'If Condition added by shitalon 30-sep-2020 for ALL27072020

            If mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentInspectionPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentInspectionNew") Or User.IsInRole("MachineComponentInspectionEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                    btnSaveSelect.Enabled = False
                    btnSaveSelect.ToolTip = "You are not Authorized user"
                End If
            ElseIf Not mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentInspectionPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentInspectionNew") Or User.IsInRole("MachineComponentInspectionEdit")) = False Then
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
        If mPartMonitorInsp.IsNew And mPartMonitorInsp.ReviseRemark <> "" Then
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorInsp.PrevRefID)
        Else
            mMaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorInsp.ID)
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
                If (Not mPartMonitorInsp.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mPartMonitorInsp.ID)
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
        mPartMonitorInspTypeList = PartMonitorInspTypeList.GetPartMonitorInspTypeList("(SELECT)")
        cmbMonitorInspType.DataSource = mPartMonitorInspTypeList
        Session("mPartMonitorInspTypeList") = mPartMonitorInspTypeList
        mPartMonitorInspPeriodUnitList = PartMonitorInspPeriodUnitList.GetPartMonitorInspPeriodUnitList(mCompMonitorInspStatus.CompStatusID)
        Session("mPartMonitorInspPeriodUnitList") = mPartMonitorInspPeriodUnitList
        dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods
        DataBind()

        'Added By Saylee on 10-Sep-2009, to set ATA chapter of the Component.
        If mPartMonitorInsp.ATAID.Equals(Guid.Empty) And mPartMonitorInsp.IsNew Then
            cmbATAChapter.SelectedValue = mCompStatus.ATAID.ToString
        End If

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorInsp.IsNew Then
            Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
            mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInsp.PartID, mPartMonitorInsp.ID.ToString)
            Session("mPartMonitorInspConfiguredList") = mPartMonitorInspConfiguredList
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
        If Not mPartMonitorInsp.IsValid Then
            For i As Integer = 0 To mPartMonitorInsp.GetBrokenRulesCollection.Count - 1
                str = str + mPartMonitorInsp.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Items(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mPartMonitorInsp.PartMonitorInspPeriods(i).IsValid Then
                For j As Integer = 0 To mPartMonitorInsp.PartMonitorInspPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorInsp.PartMonitorInspPeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mPartMonitorInsp.PartMonitorInspPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mPartMonitorInsp.PartMonitorInspPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorInsp.PartMonitorInspPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        If mPartMonitorInsp.IsAttachmentAdded Then
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
            If txtCode.Enabled = True Then
                txtCode.Focus()
            End If
            '            Type = CType(Request.QueryString("Type"), Boolean)   'Code Added Jan-10,2007
            '           Session("Type") = Type                               'Code Added Jan-10,2007 
            AddSelectedPeroidUnits()
            DataFieldBind()
            ControlVisibilty()
            SetPage()
            SetRights() 'Added By Prashant 15-Mar-2011
            ControlVisibilityForAttachment()
            SetToolsSparesCount()
        End If
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
            upnlTitle.Update()
            upnlOtherDetails.Update()
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
                Index = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
                'Added by saylee on 1-Jun-2016
                Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
                mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInsp.PartID, mPartMonitorInsp.ID.ToString)

                If mPartMonitorInspConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mPartMonitorInspConfiguredList.Count - 1
                        If i = mPartMonitorInspConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mPartMonitorInsp.PartMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If


                mPartMonitorInsp.PartMonitorInspPeriods.Remove(mPartMonitorInsp.PartMonitorInspPeriods.Item(Index).ID)
                Session("mPartMonitorInsp") = mPartMonitorInsp
                dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods
                dgPeriods.DataBind()
        End Select
    End Sub
    Private Sub btnAddPeriodUnit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPeriodUnit.Click
        SetPeroidUnits()
        SetGridObject()
        SetObject()

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorInsp.IsNew Then
            Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
            mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInsp.PartID, mPartMonitorInsp.ID.ToString)

            If mPartMonitorInspConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mPartMonitorInspConfiguredList.Count - 1
                    If i = mPartMonitorInspConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "Inspection is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If


        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow()", True)
        'Response.Redirect("wfSelectPeriodUnit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorInsp_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Part Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End

        RemoveSession()
        Session("EditMasterRecord") = "False"
        Session.Remove("mMaintenanceTaskAndKit")
        Session.Remove("mPrevCompMonitorInspStatusForRevise") 'Revise Activity
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("GChildPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
    End Sub
    Private Sub imgbtnATAChapter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnATAChapter.Click
        SetObject()             'Added Code By Girish on May,25,2007 Due to combo getting refreshed
        'Response.Redirect("wfATA_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage3=wfPartMonitorInsp_Ajax.aspx")
    End Sub
    Private Sub btnSaveSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSelect.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub

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
        If Save() Then
            If Session("NewPage") = "True" Or mPartMonitorInsp.ReviseRemark <> "" Then 'Revise Activity

                mIssueDate = Session("mIssueDate")
                'Revise Activity
                If Not mPrevCompMonitorInspStatusForRevise Is Nothing And mPartMonitorInsp.ReviseRemark <> "" Then
                    If mPrevCompMonitorInspStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mIssueDate = mPrevCompMonitorInspStatusForRevise.AsOnDateFormatted.ToString
                    Else
                        mIssueDate = mPrevCompMonitorInspStatusForRevise.DoneOnFormatted.ToString
                    End If
                End If
                'End
                mPartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(mPartMonitorInsp.ID, mHourType)
                Session("mPartMonitorInsp") = mPartMonitorInsp
                ' mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mIssueDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
                If mIsSpareComp = False Then
                    mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mIssueDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mCompStatus.HourType)
                Else
                    mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty, mIssueDate, mCompStatus.Comp.PartID, Guid.Empty, mCompStatus.ID, mCompStatus.HourType)
                End If
                With mCompMonitorInspStatus
                    .PartMonitorInspID(True) = mPartMonitorInsp.ID
                    '.PartMonitorInsp.Code = mPartMonitorInsp.Code
                    .PartMonitorInsp.Reference = mPartMonitorInsp.Reference
                    .PartMonitorInsp.Description = mPartMonitorInsp.Description

                    .PartMonitorInsp.RequiredManHours = mPartMonitorInsp.RequiredManHours
                    '---------------------------------
                    '.PartMonitorInsp.PartMonitorInspTypeID = mPartMonitorInsp.PartMonitorInspTypeID
                End With
                'Revise Activity
                If Not mPrevCompMonitorInspStatusForRevise Is Nothing Then
                    If mPrevCompMonitorInspStatusForRevise.DoneOnFormatted.ToString = "" Then
                        mCompMonitorInspStatus.DoneOn = System.DBNull.Value
                    Else
                        mCompMonitorInspStatus.DoneOn = mPrevCompMonitorInspStatusForRevise.DoneOnFormatted.ToString
                    End If
                End If
                'End
                SetSession()
                Session("mIssueDate") = mIssueDate
                Session.Remove("Edit")
                Session.Remove("mPartMonitorInspList")
                Session("FromPartMonitorInspList") = True
                '====================
                Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
                Session.Remove("mMaintenanceTaskAndKit")
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                ' Session("MiddleFrame") = "wfComplyCompMonitorInspStatusList_Ajax.aspx?" 'Revise Activity

                If AppSettings("ShowAllValuesPageEnable") = "True" Then
                    Session("MiddleFrame") = "wfComplyCompMonitorInspStatusListShowValues_Ajax.aspx?"
                Else
                    Session("MiddleFrame") = "wfComplyCompMonitorInspStatusList_Ajax.aspx?SpareComponent=" & IIf(mIsSpareComp = False, 0, 1) 'Revise Activity
                End If

                Response.Redirect("wfCompMonitorInspStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            Else
                If Session("URLForCompInst") Is Nothing Then 'dont remove session as Part Service Count Required on wfCompMonitorServiceStatus_AJAX btnBack.Click
                    Session.Remove("mPartMonitorServiceList")
                Else
                    Session("StatusPageOpenFrom") = Request.QueryString("GChildPage2")
                    'Dim URLForPartServiceList As New Stack
                    'URLForPartServiceList.Push(Request.Url)
                    'Session("URLForPartServiceList") = URLForPartServiceList
                End If
                mCompMonitorInspStatus.PartMonitorInspID(False) = mPartMonitorInsp.ID
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
                Session.Remove("mMaintenanceTaskAndKit")
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Response.Redirect("wfCompMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            End If
        End If
        '------------------------------------------------------------
    End Sub
    Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mPartMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorInsp.ID)
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

        If mPartMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorInsp.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mPartMonitorInsp.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub cmbMonitorInspType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorInspType.SelectedIndexChanged
        mPartMonitorInsp.PartMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mPartMonitorInsp.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub hdnBtnPeriodUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnPeriodUnit.Click
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        AddSelectedPeroidUnits()
        dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods
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

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompInsp(mPartMonitorInsp)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorInsp.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 3
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorInsp_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkSpares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSpares.Click
        If IsValid Then
            SetObject()

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompInsp(mPartMonitorInsp)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorInsp.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 2
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorInsp_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkTaskCards_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTaskCards.Click
        If IsValid Then
            SetObject()

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompInsp(mPartMonitorInsp)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorInsp.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 1   'Added by Saylee on 23-July-2013 for BA22072013 

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorInsp_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnAddTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTools.Click
        SetToolsSparesCount()

        If Not mMaintenanceTaskAndKit Is Nothing Then
            If Session("mChild") = 1 Then
                mPartMonitorInsp.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            ElseIf Session("mChild") = 2 Then
                mPartMonitorInsp.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            ElseIf Session("mChild") = 3 Then
                mPartMonitorInsp.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

        End If
        Session("mPartMonitorInsp") = mPartMonitorInsp
        Session.Remove("mChild")
        upnlOtherDetails.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mPartMonitorInsp.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorInsp.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPartMonitorInsp.ID)
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
        Rpt = New crDetPartMonitorInsp
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mPartMonitorInsp.PartMonitorInspPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Code/Form No.", _
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                 dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Code/Form No.", _
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "ATA Chapter", _
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                            CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).FrequencyValue, String)))

                Else
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "ATA Chapter", _
                                                    cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", lblReference.Text, _
                                 txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                     CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).PeriodUnitName, String), _
                     CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", lblReference.Text, _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                                     "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                                   CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Description", _
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Insp Type", _
                                    cmbMonitorInspType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                                   CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Insp Type", _
                                  cmbMonitorInspType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Note", _
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                                CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "Note", _
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Insp", _
                             "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "", _
                     "", , , , , , , , , , , , , , , , , "Frequency of Insp", _
              CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).PeriodUnitName, String), _
              CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Insp Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Insp", _
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Insp Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Insp", _
                                  CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorInsp.PartMonitorInspPeriods(I).FrequencyValue, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Part Inspection Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 27-Jul-2011 For All19072011
        '      MarkLog(Util.Action.Print, "PartMonitorSer", "Part Name -> " + mPartMonitorInsp.Part.Name + "PartMonitor Insp Type -> " + mPartMonitorInsp.PartMonitorInspTypeName, Util.ErrorType.HandledError, mPartMonitorInsp.ID)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

End Class