'AJAX Conversion by Saylee On 25-May-2015

Public Class wfPartMonitorMod_AJAX
    Inherits System.Web.UI.Page

#Region " Variable declartion "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mCompMonitorModStatus As CompMonitorModStatus
    Public mPartMonitorMod As PartMonitorMod
    Public mPartMonitorModPeriodUnitList As PartMonitorModPeriodUnitList
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mPartMonitorModTypeList As PartMonitorModTypeList
    Dim Flag As Int16
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    ' Public Type As Boolean = False    'Code Added  Jan-10,2007
    Public mIssueDate As String
    Public mCompMonitorModStatusList As tmpCompMonitorModStatusList

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
	Public mIsSpareComp As Boolean = False  'Added By Prashant 1-Oct-2020 for SpareComp
	Dim mIssuingAuthorityTypeList As IssuingAuthorityTypeList
#End Region

#Region " Business Methdods "
	Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mPartMonitorMod = CType(Session("mPartMonitorMod"), PartMonitorMod)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mPartMonitorModPeriodUnitList = CType(Session("mPartMonitorModPeriodUnitList"), PartMonitorModPeriodUnitList)
        '    Type = CType(Request.QueryString("Type"), Boolean)   'Code Added Jan-10,2007
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mCompMonitorModStatusList = CType(Session("mCompMonitorModStatusList"), tmpCompMonitorModStatusList)
        mIssueDate = Session("mIssueDate")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mIsSpareComp = Session("IsSpareComp") 'Added By Prashant 1-Oct-2020 for SpareComp
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mPartMonitorMod") = mPartMonitorMod
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mPartMonitorModPeriodUnitList") = mPartMonitorModPeriodUnitList
        '   Session("Type") = Type   'Code Added Jan-10,2007
        Session("mIssueDate") = mIssueDate

        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mPartMonitorModTypeList")
        Session.Remove("mPartMonitorMod")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub SetObject()
        With mPartMonitorMod
            .Code = txtCode.Text.Trim
            .ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
            .Reference = txtReference.Text.Trim
            .Description = txtDescription.Text.Trim
            .PartMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
            .Number = txtModificationNo.Text
            If (calIssueDate.Text <> "") Then
                .IssueDate = calIssueDate.Text.ToString
            Else
                .IssueDate = System.DBNull.Value
            End If
            .IsApplicable = chkApplicable.Checked
            .Note = txtNote.Text.Trim
            'Commented and changed to "True" by Saylee on 9th-Jan-2008 for bug-PMMSD4(Maintenance)
            '.ShowInCofA = chkShowInCofA.Checked
            .ShowInCofA = True
            .Applicability = Trim(txtApplicability.Text)
            .ComplianceRequirement = Trim(txtComplianceRequirement.Text)
            '===============================================
            .RequiredManHours = Trim(txtRequiredManHours.Text)

            'Added by Saylee on 23-July-2013 for BA22072013 
            .Zone = Trim(txtZone.Text)
            .Area = Trim(txtArea.Text)
            .IsRII = chkIsRII.Checked
			'End
			.RefAttachlink = txtRefAttachLink.Text    'Added by Shital on 07-FEb-2022
			.IssuingAuthorityID = CType(Val(cmbIssuingAuthority.SelectedValue), Int32)
			.IssuingAuthority = cmbIssuingAuthority.SelectedItem.Text
		End With

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mPartMonitorMod.IsAttachmentAdded = True
            Else
                mPartMonitorMod.IsAttachmentAdded = False
            End If
            'Else
            '    .IsAttachmentAdded = False
        End If

        Session("mPartMonitorMod") = mPartMonitorMod
    End Sub
    Public Sub SetGridObject()
        For i As Integer = 0 To mPartMonitorMod.PartMonitorModPeriods.Count - 1
            Dim txtFreqVal As TextBox = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            mPartMonitorMod.PartMonitorModPeriods(i).FrequencyValue = txtFreqVal.Text.Trim
        Next
        Session("mPartMonitorMod") = mPartMonitorMod
    End Sub
    Private Sub AddSelectedPeroidUnits()
        Dim clnPartMonitorMod As PartMonitorMod = mPartMonitorMod.Clone
        Try
            Dim mHourType As Integer = 0  'Added By Prashant 1-Oct-2020 for SpareComp
            If mIsSpareComp = False Then
                If mAssemblyStatus.IsSpareAssembly = True Then
                    mHourType = mAssemblyStatus.HourType
                Else
                    mHourType = mMachine.HourType
                End If
            End If  'End of Added By Prashant 1-Oct-2020 for SpareComp
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    'mPartMonitorMod.PartMonitorModPeriods.Add(mPartMonitorMod.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mMachine.HourType)
                    mPartMonitorMod.PartMonitorModPeriods.Add(mPartMonitorMod.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType) 'Added By Prashant 1-Oct-2020 for SpareComp
                mPartMonitorMod.SetZeroFrequencyValue()
                End If
            Next
            Session("mPartMonitorMod") = mPartMonitorMod
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mPartMonitorMod = clnPartMonitorMod
            Session("mPartMonitorMod") = mPartMonitorMod
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnPartMonitorMod = Nothing
            mSelectPeriodUnits = Nothing
            Session.Remove("mSelectPeriodUnits")
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mPartMonitorModPeriodUnitList.Count - 1
            If Not mPartMonitorMod.PartMonitorModPeriods.Contains(mPartMonitorModPeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mPartMonitorModPeriodUnitList(i).ID, mPartMonitorModPeriodUnitList(i).PeriodID, mPartMonitorModPeriodUnitList(i).Name)
            End If
        Next
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub SetPage()
        If mPartMonitorMod.IsNew Then
            lblTitle.Text = "Part Modification of [ Part: " & mPartMonitorMod.Part.Name & "][New]"
        Else
            lblTitle.Text = "Part Modification of [ Part: " & mPartMonitorMod.Part.Name & "]"
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
        lnkSpares.Enabled = Not mPartMonitorMod.IsNew
        lnkTools.Enabled = Not mPartMonitorMod.IsNew
        lnkTaskCards.Enabled = Not mPartMonitorMod.IsNew
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Save" Then
                        Session("sender") = ""
                        Save()
                    End If
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
        Dim mPartMonitorModClone As PartMonitorMod
        mPartMonitorModClone = CType(mPartMonitorMod, PartMonitorMod)
        SetObject()
        SetGridObject()
        If mPartMonitorMod.IsValid = True Then
            If mPartMonitorMod.PartMonitorModPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Part Modification.Part Modification can not be saved without period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mPartMonitorMod.ApplyEdit()
                mPartMonitorMod = CType(mPartMonitorMod.Save(), PartMonitorMod)
                SaveAttachment()
                'Commented By Utkarsh On 27-Jul-2011 For All19072011
                '     MarkLog(Util.Action.Save, "PartMonitorSer", "ATAChapter->" + mPartMonitorMod.ATAChapter + " -> " + " Part Name -> " + mPartMonitorMod.Part.Name + " Part Monitor Mod Type Name -> " + mPartMonitorMod.PartMonitorModTypeName, Util.ErrorType.NoError, mPartMonitorMod.ID)
                'End
                Session("mPartMonitorMod") = mPartMonitorMod
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
                mPartMonitorMod = mPartMonitorModClone
                Session("mPartMonitorMod") = mPartMonitorMod
                Return False
            Finally
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                'MaintDetail = "Monitor Mod Type : " + mPartMonitorMod.PartMonitorModTypeName + " Description : " + mPartMonitorMod.Description
                MaintDetail = "Part : " & mCompStatus.PartNameSerialNo & " Part Modification Type : " & mPartMonitorMod.PartMonitorModTypeName & " Description : " & mPartMonitorMod.Description
                MarkLog(Util.Action.Save, "Part Modification", MaintDetail, Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub ControlVisibilty()
        btnPrint.Enabled = Not mPartMonitorMod.IsNew
        btnAddPeriodUnit.Enabled = mPartMonitorModPeriodUnitList.Count > 0

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorMod.IsNew Then
            Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList = Session("mPartMonitorModConfiguredList")
            If Not mPartMonitorModConfiguredList Is Nothing Then
                If mPartMonitorModConfiguredList.Count > 0 Then
                    cmbMonitorModType.Enabled = False
                Else
                    cmbMonitorModType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mPartMonitorMod.PartMonitorModPeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mPartMonitorModConfiguredList.Count > 0 Then
                            txtFrequencyValue.Enabled = False
                        Else
                            txtFrequencyValue.Enabled = True
                        End If

                    Next i
                End With
            End If

        End If
        If Not Session("OpenFromADSBReviewMeeting") Is Nothing Then   'Added by Saylee on 28-Sep-2022 for Review Meeting
            btnSaveSelect.Visible = False
            btnSave.Visible = True
        End If
        'If mPartMonitorModPeriodUnitList.Count > 0 Then btnAddPeriodUnit.BackColor = Color.Gray
    End Sub
    Private Sub SetRights()
        If mIsSpareComp = False Then 'If Condition Added By Prashant 1-Oct-2020 for SpareComp
            If mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentModificationPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentModificationNew") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                    btnSaveSelect.Enabled = False
                    btnSaveSelect.ToolTip = "You are not Authorized user"
                End If
            ElseIf Not mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentModificationPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentModificationNew") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                    btnSaveSelect.Enabled = False
                    btnSaveSelect.ToolTip = "You are not Authorized user"
                End If
            End If
        End If
    End Sub
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorMod.ID)
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
                If (Not mPartMonitorMod.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mPartMonitorMod.ID)
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
        mPartMonitorModTypeList = PartMonitorModTypeList.GetPartMonitorModTypeList("(SELECT)")
        cmbMonitorModType.DataSource = mPartMonitorModTypeList
        Session("mPartMonitorModTypeList") = mPartMonitorModTypeList
        mPartMonitorModPeriodUnitList = PartMonitorModPeriodUnitList.GetPartMonitorModPeriodUnitList(mCompMonitorModStatus.CompStatusID)
        Session("mPartMonitorModPeriodUnitList") = mPartMonitorModPeriodUnitList
		dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods
		mIssuingAuthorityTypeList = IssuingAuthorityTypeList.GetIssuingAuthorityTypeList(IsSelectTagRequired:=True)
		cmbIssuingAuthority.DataSource = mIssuingAuthorityTypeList

		DataBind()

        calIssueDate.Text = mPartMonitorMod.IssueDateFormatted.ToString
        'Added By Saylee on 10-Sep-2009, to set ATA chapter of the Component.
        If mPartMonitorMod.ATAID.Equals(Guid.Empty) And mPartMonitorMod.IsNew Then
            cmbATAChapter.SelectedValue = mCompStatus.ATAID.ToString
        End If

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorMod.IsNew Then
            Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
            mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorMod.PartID, mPartMonitorMod.ID.ToString)
            Session("mPartMonitorModConfiguredList") = mPartMonitorModConfiguredList
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbATAChapter" Then
            If cmbATAChapter.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select ATA Chapter from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbMonitorModType" Then
            If cmbMonitorModType.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select Modification Type from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtReference" Then
            If Len(txtReference.Text) > 500 Then
                custValidator.ErrorMessage = "Reference should not be more than 500 chars."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf custValidator.ControlToValidate = "txtDescription" Then
            '    If Len(txtDescription.Text) > 1000 Then
            '        custValidator.ErrorMessage = "Description can't be more than 1000 chars."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
            'ElseIf custValidator.ControlToValidate = "txtModificationNo" Then
            '    If Val(txtModificationNo.Text) = 0 Then
            '        custValidator.ErrorMessage = "Modification No can't be Zero."
            '        e.IsValid = False
            '    ElseIf txtModificationNo.Text > 9999 Then
            '        custValidator.ErrorMessage = "Modification No can't exceed 9999."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            ''    End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note can't be more than 1000 chars."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtCode" Then
            If Len(txtCode.Text) > 25 Then
                custValidator.ErrorMessage = "Code can't be more than 25 chars."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "calIssueDate" Then
            If calIssueDate.Text = "" Then
                custValidator.ErrorMessage = "Issue Date can't be empty."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
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
        If Not mPartMonitorMod.IsValid Then
            For i As Integer = 0 To mPartMonitorMod.GetBrokenRulesCollection.Count - 1
                str = str + mPartMonitorMod.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Items(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mPartMonitorMod.PartMonitorModPeriods(i).IsValid Then
                For j As Integer = 0 To mPartMonitorMod.PartMonitorModPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorMod.PartMonitorModPeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mPartMonitorMod.PartMonitorModPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mPartMonitorMod.PartMonitorModPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorMod.PartMonitorModPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        If mPartMonitorMod.IsAttachmentAdded Then
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

                'Added by saylee on 1-Jun-2016
                Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
                mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorMod.PartID, mPartMonitorMod.ID.ToString)

                If mPartMonitorModConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mPartMonitorModConfiguredList.Count - 1
                        If i = mPartMonitorModConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mPartMonitorMod.PartMonitorModPeriods.Item(Index).PeriodUnitName + " frequency is configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If

                Index = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
                mPartMonitorMod.PartMonitorModPeriods.Remove(mPartMonitorMod.PartMonitorModPeriods.Item(Index).ID)
                Session("mPartMonitorMod") = mPartMonitorMod
                dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods
                dgPeriods.DataBind()
        End Select
    End Sub
    Private Sub btnAddPeriodUnit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPeriodUnit.Click
        SetPeroidUnits()
        SetGridObject()
        SetObject()


        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorMod.IsNew Then
            Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
            mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorMod.PartID, mPartMonitorMod.ID.ToString)

            If mPartMonitorModConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mPartMonitorModConfiguredList.Count - 1
                    If i = mPartMonitorModConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "Modification is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If


        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow()", True)
        'Response.Redirect("wfSelectPeriodUnit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorMod_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Part Modification", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End

        RemoveSession()
        Session("EditMasterRecord") = "False"
        Session.Remove("mMaintenanceTaskAndKit")
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("GChildPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
    End Sub
    Private Sub imgbtnATAChapter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnATAChapter.Click
        SetObject()             'Added Code By Girish on May,25,2007 Due to combo getting refreshed
        'Response.Redirect("wfATA_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage3=wfPartMonitorMod_Ajax.aspx")
    End Sub
    Private Sub btnSaveSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSelect.Click
        Dim mHourType As Integer = 0 'Added By Prashant 1-Oct-2020 for SpareComp
        If mIsSpareComp = False Then
            If mAssemblyStatus.IsSpareAssembly = True Then
                mHourType = mAssemblyStatus.HourType
            Else
                mHourType = mMachine.HourType
            End If
        End If 'End of Added By Prashant 1-Oct-2020 for SpareComp
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
        If Save() Then
            If Session("NewPage") = "True" Then

                mIssueDate = Session("mIssueDate")
                mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mPartMonitorMod.ID, mMachine.HourType)
                Session("mPartMonitorMod") = mPartMonitorMod
                If mIsSpareComp = False Then 'Added By Prashant 1-Oct-2020 for SpareComp
                    mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mPartMonitorMod.IssueDateFormatted.ToString, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mHourType)
                Else
                    Dim mModelList As ModelList
                    mModelList = ModelList.GetModelList(1, , , , )
                    mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty, _
                                                                                         mPartMonitorMod.IssueDateFormatted.ToString, _
                                                                                         mCompStatus.Comp.PartID, mModelList.Item(0).ID, _
                                                                                         mCompStatus.ID, mCompStatus.HourType)
                End If

                With mCompMonitorModStatus
                    .PartMonitorModID(True) = mPartMonitorMod.ID
                    '.PartMonitorMod.Code = mPartMonitorMod.Code
                    .PartMonitorMod.Reference = mPartMonitorMod.Reference
                    .PartMonitorMod.Description = mPartMonitorMod.Description

                    .PartMonitorMod.RequiredManHours = mPartMonitorMod.RequiredManHours
                    '---------------------------------
                    '.PartMonitorMod.PartMonitorModTypeID = mPartMonitorMod.PartMonitorModTypeID
                End With
                SetSession()
                Session.Remove("Edit")
                Session.Remove("mPartMonitorModList")
                Session("FromPartMonitorModList") = True
                '====================
                Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                Session.Remove("mMaintenanceTaskAndKit")
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")

                Session("mIssueDate") = mIssueDate
                Response.Redirect("wfCompMonitorModStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            Else
                Session.Remove("mPartMonitorModList")
                mCompMonitorModStatus.PartMonitorModID(False) = mPartMonitorMod.ID

                'Added by Saylee on 13-July-2009
                If mPartMonitorMod.MonitorTypeID = 3 Then
                    mCompMonitorModStatus.IsApplicable = False
                End If
                '********************************
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session.Remove("mMaintenanceTaskAndKit")
                Response.Redirect("wfCompMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            End If
        End If
        '------------------------------------------------------------
    End Sub
    Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mPartMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorMod.ID)
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

        If mPartMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorMod.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mPartMonitorMod.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub cmbMonitorModType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorModType.SelectedIndexChanged
        mPartMonitorMod.PartMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
        dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mPartMonitorMod.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub hdnBtnPeriodUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnPeriodUnit.Click
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        AddSelectedPeroidUnits()
        dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods
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

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompMod(mPartMonitorMod)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorMod.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 3
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorMod_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkSpares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSpares.Click
        If IsValid Then
            SetObject()

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompMod(mPartMonitorMod)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorMod.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 2
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorMod_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkTaskCards_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTaskCards.Click
        If IsValid Then
            SetObject()
            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompMod(mPartMonitorMod)

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mPartMonitorMod.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 1   'Added by Saylee on 23-July-2013 for BA22072013 

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow()", True)
            'Response.Redirect("wfMaintenanceKitandTask_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=wfPartMonitorMod_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnAddTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTools.Click
        SetToolsSparesCount()
        If Not mMaintenanceTaskAndKit Is Nothing Then
            If Session("mChild") = 1 Then
                mPartMonitorMod.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            ElseIf Session("mChild") = 2 Then
                mPartMonitorMod.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            ElseIf Session("mChild") = 3 Then
                mPartMonitorMod.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

        End If
        Session("mPartMonitorMod") = mPartMonitorMod
        Session.Remove("mChild")
        upnlOtherDetails.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mPartMonitorMod.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorMod.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPartMonitorMod.ID)
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
        Rpt = New crDetPartMonitorMod
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mPartMonitorMod.PartMonitorModPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Code/Form No.", _
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                 dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Code/Form No.", _
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "ATA Chapter", _
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                            CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValue, String)))

                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "ATA Chapter", _
                                                    cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", lblReference.Text, _
                                 txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                     CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
                     CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", lblReference.Text, _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                                     "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                                   CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Description", _
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Mod Type", _
                                    cmbMonitorModType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                                   CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Mod Type", _
                                  cmbMonitorModType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", lblModNo.Text, _
                                    txtModificationNo.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                                CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", lblModNo.Text, _
                                     txtModificationNo.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                             "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", lblIssueDate.Text, _
                     calIssueDate.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
              CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
              CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", lblIssueDate.Text, _
                                          calIssueDate.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                             "", ""))
                End If
            ElseIf I = 6 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Note", _
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                           CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
                     CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "Note", _
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Mod", _
                             "", ""))
                End If
            ElseIf I = 7 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "", _
                     "", , , , , , , , , , , , , , , , , "Frequency of Mod", _
               CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
               CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Mod Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Mod", _
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Mod Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Mod", _
                                  CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).PeriodUnitName, String), _
                            CType(Me.mPartMonitorMod.PartMonitorModPeriods(I).FrequencyValue, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Part Modification Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 27-Jul-2011 For All19072011
        '      MarkLog(Util.Action.Print, "PartMonitorSer", "Part Name -> " + mPartMonitorMod.Part.Name + "PartMonitor Mod Type -> " + mPartMonitorMod.PartMonitorModTypeName, Util.ErrorType.HandledError, mPartMonitorMod.ID)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

End Class