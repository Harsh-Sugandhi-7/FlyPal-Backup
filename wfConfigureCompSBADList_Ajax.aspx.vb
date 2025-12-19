
'Created by : Saylee
'Date:        15-Apr-2019



Public Class wfConfigureCompSBADList_Ajax
    Inherits System.Web.UI.Page


#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Variable Declaration "
    Protected mAssemblyList As AssemblyList
    Protected mAssemblyTypeList As AssemblyTypeList
    Protected mCompListForComboBox As CompListForComboBox
    Protected mPartMonitorModTypeList As PartMonitorModTypeList
    Protected mPartMonitorModList As PartMonitorModList
    Protected mCompMonitorModStatus As CompMonitorModStatus
    Dim mCompADSBConfigurableList As CompADSBConfigurableList
    Dim mCompMonitorModStatusList As CompMonitorModStatusList
    Dim mFileAttach As FileAttach
    Dim mMachine As Machine
    Dim mUpdateComplyHistoryCompMonitorModStatusList As UpdateComplyHistoryCompMonitorModStatusList
    Dim mBoardInfo As AircraftInformationBoard.BoardInfo
    Dim mMachineMaintenance As MachineMaintenance
    Dim mModectionDetail As String
    Public mATAList As ATAList
    Dim SelectedAssemblyIndex, ActiveTabindex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA, SelectedCompIndex As Integer
    Dim Description As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyList = Session("mAssemblyListForConfigCompMPD")
        mAssemblyTypeList = CType(Session("mAssemblyTypeListForConfigCompMPD"), AssemblyTypeList)
        mPartMonitorModList = CType(Session("mPartMonitorModList"), PartMonitorModList)
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mCompADSBConfigurableList = Session("mCompADSBConfigurableList")
        mCompMonitorModStatusList = Session("mCompMonitorModStatusList")
        SelectedAssemblyIndex = IIf(Session("SelectedAssemblyIndexForConfigCompMPD") Is Nothing, 0, Session("SelectedAssemblyIndexForConfigCompMPD"))
        SelectedAssemblyTypeIndex = IIf(Session("SelectedAssemblyTypeIndexForConfigCompMPD") Is Nothing, 0, Session("SelectedAssemblyTypeIndexForConfigCompMPD"))
        ActiveTabindex = IIf(Session("ActiveTabindexForConfigCompMPD") Is Nothing, 0, Session("ActiveTabindexForConfigCompMPD"))
        SelectedMonitorType = IIf(Session("SelectedMonitorTypeForConfigCompMPD") Is Nothing, 0, Session("SelectedMonitorTypeForConfigCompMPD"))
        ATA = IIf(Session("ATAForConfigCompMPD") Is Nothing, 0, Session("ATAForConfigCompMPD"))
        Description = IIf(Session("DescriptionForConfigCompMPD") Is Nothing, String.Empty, Session("DescriptionForConfigCompMPD"))
        mATAList = CType(Session("mATAListForConfigCompMPD"), ATAList)
        mPartMonitorModTypeList = CType(Session("mPartMonitorModTypeListForConfigCompMPD"), PartMonitorModTypeList)
        SelectedCompIndex = IIf(Session("SelectedCompIndexForConfigCompMPD") Is Nothing, 0, Session("SelectedCompIndexForConfigCompMPD"))
        mCompListForComboBox = Session("mCompListForComboBox")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfConfigureCompSBADList_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub getGridRecords()
        If mAssemblyList.Count > 0 Then
            mCompListForComboBox = CompListForComboBox.GetCompList(Today.Date.ToString, mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString)
            Session("mCompListForComboBox") = mCompListForComboBox
            cmbComponent.DataSource = mCompListForComboBox
            cmbComponent.DataBind()

            cmbAssembly.Enabled = True
            If mCompListForComboBox.Count > 0 Then
                cmbComponent.Enabled = True
                mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ModType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=txtDescription.Text.Trim, IsFromADSB:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                dgNonConfigList.DataSource = mPartMonitorModList
                dgNonConfigList.DataBind()
                Session("mPartMonitorModList") = mPartMonitorModList

                mCompADSBConfigurableList = CompADSBConfigurableList.GetADSBConfigurationList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, PartMonitorModID:=Guid.Empty.ToString, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ModificationType:=SelectedMonitorType, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
                dgConfigList.DataSource = mCompADSBConfigurableList
                'mCompMonitorModStatusList = CompMonitorModStatusList.GetCompMonitorModStatusList(CurrentDate:=Today.Date.ToString, CompID:=mCompListForComboBox(SelectedCompIndex).CompID, SerialNo:=mCompListForComboBox(SelectedCompIndex).CompSerialNo, CompStatusPeriodList:=Nothing, IsFromADSB:=True, Description:=Description, ATACode:=mATAList(ATA).ATACode, MonitorTypeID:=SelectedMonitorType, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID.ToString, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsComplied:=True)
                'Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                ' dgConfigList.DataSource = mCompMonitorModStatusList
                dgConfigList.DataBind()

                SetGrid()
                SetPage(mCompADSBConfigurableList.Count, mPartMonitorModList.Count)
                ControlVisibility()
            Else
                cmbComponent.Enabled = False
                mPartMonitorModList = Nothing
                mCompADSBConfigurableList = Nothing
                Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
                'mCompMonitorModStatusList = Nothing
                ' Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                Session("mPartMonitorModList") = mPartMonitorModList

                dgNonConfigList.DataSource = mPartMonitorModList
                dgNonConfigList.DataBind()
                dgConfigList.DataSource = mCompADSBConfigurableList
                dgConfigList.DataBind()
                SetGrid()
                SetPage()
                lblConfigResult.Visible = False
                lblNonConfigResult.Visible = False
            End If
        Else
            cmbAssembly.Enabled = False
            cmbComponent.Enabled = False
            mPartMonitorModList = Nothing
            mCompADSBConfigurableList = Nothing
            Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
            ' mCompMonitorModStatusList = Nothing
            'Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
            mCompListForComboBox = Nothing
            Session("mCompListForComboBox") = mCompListForComboBox
            Session("mPartMonitorModList") = mPartMonitorModList

            dgNonConfigList.DataSource = mPartMonitorModList
            dgNonConfigList.DataBind()
            dgConfigList.DataSource = mCompADSBConfigurableList
            dgConfigList.DataBind()
            SetGrid()
            lblConfigResult.Visible = False
            lblNonConfigResult.Visible = False
            lblConfigTabPanel.Text = "Configured(0)"
            lblNonConfigTabPanel.Text = "Non-Configured(0)"
        End If
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "ConfigureMPD"


        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
        End Select
    End Function
    Private Sub RemoveSession()
        Session.Remove("mAssemblyListForConfigCompMPD")
        Session.Remove("mAssemblyTypeListForConfigCompMPD")
        Session.Remove("mPartMonitorModList")
        Session.Remove("mCompMonitorModStatus")
        Session.Remove("mCompADSBConfigurableList")
        Session.Remove("mCompMonitorModStatusList")
        Session.Remove("mATAListForConfigCompMPD")
        Session.Remove("mPartMonitorModTypeListForConfigCompMPD")
        Session.Remove("mCompListForComboBox")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartMonitorModList.CurrentIndex = Index
        Session("mPartMonitorModList") = mPartMonitorModList
    End Sub
    Private Sub ControlVisibility()
        If Not mPartMonitorModList Is Nothing Then
            lblNonConfigResult.Visible = (mPartMonitorModList.Count > 0)
        End If
        If Not mCompADSBConfigurableList Is Nothing Then
            lblConfigResult.Visible = (mCompADSBConfigurableList.Count > 0)
        End If

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteConfigRecord" Then
                        Dim IDForEventLog As Guid
                        Dim PartMonitorModID As Guid
                        Try
                            Session("sender") = ""
                            Dim index As Integer = Session("Index")
                            IDForEventLog = mCompADSBConfigurableList(index).ID
                            PartMonitorModID = mCompADSBConfigurableList(index).PartMonitorModID
                            mModectionDetail = "Part : " + mCompListForComboBox(SelectedCompIndex).PartNoSerialNo + " Aircraft : " + mAssemblyList(SelectedAssemblyIndex).RegNo + " Monitor Type : " + mCompADSBConfigurableList(index).MonitorType + " Description : " + mCompADSBConfigurableList(index).Description
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(IDForEventLog)
                            '********************************
                            If mCompADSBConfigurableList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(IDForEventLog)
                            End If
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(IDForEventLog, 9)
                            '=============================
                            CompMonitorModStatus.DeleteCompMonitorModStatus(IDForEventLog)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If

                            Session("mMachineMaintenance") = mMachineMaintenance
                            'Added by Saylee on 28-May-2009
                            mBoardInfo.IsComplyDelete = True
                            mBoardInfo.ApplyEdit()
                            mBoardInfo.Save()
                            Session("mAircraftInformationBoardList") = Nothing
                            '********************************
                            'End
                            mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ModType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=Description, IsFromADSB:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                            dgNonConfigList.DataSource = mPartMonitorModList
                            dgNonConfigList.DataBind()
                            Session("mPartMonitorModList") = mPartMonitorModList

                            mCompADSBConfigurableList = CompADSBConfigurableList.GetADSBConfigurationList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, PartMonitorModID:=Guid.Empty.ToString, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ModificationType:=SelectedMonitorType, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                            Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
                            'mCompMonitorModStatusList = CompMonitorModStatusList.GetCompMonitorModStatusList(CurrentDate:=Today.Date.ToString, CompID:=mCompListForComboBox(SelectedCompIndex).CompID, SerialNo:=mCompListForComboBox(SelectedCompIndex).CompSerialNo, CompStatusPeriodList:=Nothing, IsFromADSB:=True, Description:=Description, ATACode:=mATAList(ATA).ATACode, MonitorTypeID:=SelectedMonitorType, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID.ToString, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsComplied:=True)
                            ' Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                            dgConfigList.DataSource = mCompADSBConfigurableList
                            dgConfigList.DataBind()

                            SetGrid()
                            SetPage(mCompADSBConfigurableList.Count, mPartMonitorModList.Count)
                            ControlVisibility()
                            upnlTabs.Update()
                            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "ComponentModections", "Can't delete :" & mModectionDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentModections", mModectionDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    End If

                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim IDForEventLog As Guid
                        Try
                            Session("sender") = ""

                            IDForEventLog = mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).ID
                            If mPartMonitorModList(IDForEventLog).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorModList(IDForEventLog).ID)
                            End If
                            mModectionDetail = "Part : " + mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).PartName + " ATA : " + mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).ATAChapter + " Description : " + mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).Description
                            PartMonitorMod.DeletePartMonitorMod(mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            MarkLog(Util.Action.Delete, "Part Modection", mModectionDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ModType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=Description, IsFromADSB:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                            dgNonConfigList.DataSource = mPartMonitorModList
                            dgNonConfigList.DataBind()
                            SetGrid()
                            SetPage(mCompADSBConfigurableList.Count, mPartMonitorModList.Count)
                            ControlVisibility()
                            upnlTabs.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "") 'Added by Vikrant on 28-July-2011
                                MarkLog(Util.Action.Delete, "Part Modection", "Can't Delete:" & mModectionDetail & " is already in use", Util.ErrorType.NoError, IDForEventLog, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetPage(Optional ByVal ConfigMPDCount As Integer = 0, Optional ByVal NonConfigMPDCount As Integer = 0)
        If mAssemblyList.Count > 0 And mCompListForComboBox.Count > 0 Then
            lblConfigResult.Text = "List of Configured MPD's for Component '" + mCompListForComboBox(SelectedCompIndex).PartNoSerialNo + "' Installed on Aircraft '" + mAssemblyList(SelectedAssemblyIndex).RegNo + "' : " + ConfigMPDCount.ToString + " Record(s)"
            lblNonConfigResult.Text = "List of Non Configured MPD's for Part '" + mCompListForComboBox(SelectedCompIndex).PartName + "' : " + NonConfigMPDCount.ToString + " Record(s)"
        End If
        lblConfigTabPanel.Text = "Configured(" + ConfigMPDCount.ToString + ")"
        lblNonConfigTabPanel.Text = "Non-Configured(" + NonConfigMPDCount.ToString + ")"
    End Sub
    Private Sub SetGrid(Optional ByVal IsConfigGrid As Boolean = True, Optional ByVal IsNonConfigGrid As Boolean = True)
        Dim P, C, IsReadOnly As Boolean
        If IsConfigGrid Then
            For j As Integer = 0 To dgConfigList.Rows.Count - 1
                C = CType(Me.dgConfigList.Rows(j).Cells(25).Text, Boolean) 'IsMaster
                P = CType(Me.dgConfigList.Rows(j).Cells(27).Text, Boolean) 'IsAttachmentAdded
                IsReadOnly = CType(Me.dgConfigList.Rows(j).Cells(28).Text, Boolean) 'IsReadOnly

                If C = True Then
                    dgConfigList.Rows(j).Cells(24).Enabled = False 'History
                End If
                If P = False Then
                    dgConfigList.Rows(j).Cells(26).Enabled = False 'View
                End If

                dgConfigList.Rows(j).Cells(22).Enabled = IIf(IsReadOnly, False, True) 'Delete
                dgConfigList.Rows(j).Cells(23).Enabled = IIf(IsReadOnly, False, True) 'Edit

            Next
        End If
        If IsNonConfigGrid Then
            For j As Integer = 0 To dgNonConfigList.Rows.Count - 1
                P = CType(Me.dgNonConfigList.Rows(j).Cells(14).Text, Boolean)
                If P = False Then
                    dgNonConfigList.Rows(j).Cells(13).Enabled = False
                End If

                dgNonConfigList.Rows(j).Cells(10).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = False, True, False)
            Next
        End If
        lblReadOnly.Visible = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, True, False)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mPartMonitorMod As PartMonitorMod
        mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mId, 1) 'HourType=1 as diff is only show purpose H OR HD
        Session("mPartMonitorMod") = mPartMonitorMod
        mModectionDetail = "Part : " & mCompListForComboBox(SelectedCompIndex).PartName & " Part Modection Type : " & mPartMonitorMod.PartMonitorModTypeName & " Description : " & mPartMonitorMod.Description
        MarkLog(Util.Action.Edit, "Part Modection", mModectionDetail, Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
        Session("PartIDForNewCompMPD") = mCompListForComboBox(SelectedCompIndex).PartID
        Session("IsFromMPDConfig") = True
        Session.Remove("mCompMonitorModStatusList") 'MPD Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompADSB_Ajax.aspx?BackPage=wfConfigureCompSBADList_Ajax.aspx');", True)
    End Sub
    Private Sub EditConfiguredRecord(ByVal CompMonitorModStatusID As Guid, ByVal AssemblyStausID As Guid, ByVal CompStatusID As Guid, ByVal HourType As Integer)
        mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(CompMonitorModStatusID, AssemblyStausID, CompStatusID, HourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("Edit") = True
        Session.Remove("mCompMonitorModStatusList") 'MPD Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCompMonitorModStatus_Ajax.aspx?GChildPage4=index.aspx');", True)
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal CompMonitorModStatusID As Guid, ByVal AssemblyStatusID As Guid, ByVal CompStatusID As Guid)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorModStatus As CompMonitorModStatus
        Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(CompMonitorModStatusID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
        Session("EnFrom") = 1 'EditRecord
        ''
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, Today.Date.ToString)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus

        mUpdateComplyHistoryCompMonitorModStatusList = UpdateComplyHistoryCompMonitorModStatusList.GetComplyHistoryCompMonitorModStatusList(mCompStatus.CompID, mCompMonitorModStatus.PartMonitorModID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorModStatusList") = mUpdateComplyHistoryCompMonitorModStatusList

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDirectiveHistoryWindow", "OpenDirectiveHistoryWindow();", True)
        'End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeListForConfigCompMPD") = mAssemblyTypeList
        cmbAssemblyType.DataBind()

        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, MachineID:=Guid.Empty.ToString, InstalledOn:=Today.Date.ToString, AddTopItem:="", IsInstalled:=True, SkipIsForInventoryAircarft:=True)
        cmbAssembly.DataSource = mAssemblyList
        Session("mAssemblyListForConfigCompMPD") = mAssemblyList
        cmbAssembly.DataBind()

        mATAList = ATAList.GetATAList("", "(All)") 'Added By Saylee on 12-Aug-2010
        Session("mATAListForConfigCompMPD") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        mPartMonitorModTypeList = PartMonitorModTypeList.GetPartMonitorModTypeList("(All)")
        cmbMonitorType.DataSource = mPartMonitorModTypeList
        cmbMonitorType.DataBind()
        Session("mPartMonitorModTypeListForConfigCompMPD") = mPartMonitorModTypeList

        getGridRecords()
        cmbAssemblyType.SelectedIndex = SelectedAssemblyTypeIndex
        cmbAssembly.SelectedIndex = SelectedAssemblyIndex
        cmbComponent.SelectedIndex = SelectedCompIndex
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            ' Session("MiddleFrame") = "wfConfigureCompSBADList_Ajax.aspx"
            DataFieldBind()
            SetGrid()
            ControlVisibility()
            TbConfigNonConfig.ActiveTabIndex = IIf(CType(Session("ActiveTabindexForConfigCompMPD"), Integer) > 0, CType(Session("ActiveTabindexForConfigCompMPD"), Integer), 0)
        End If
    End Sub
    Private Sub dgNonConfigList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgNonConfigList.RowCommand
        Dim mID As Guid
        Dim AssemblyID As Guid
        Dim AssemblyStatusID As Guid
        Dim HourType As Integer
        Dim ModelID As Guid
        Select Case e.CommandName
            Case "Config"
                AssemblyID = mAssemblyList(SelectedAssemblyIndex).ID
                AssemblyStatusID = mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID
                ModelID = mAssemblyList(SelectedAssemblyIndex).ModelID
                HourType = mAssemblyList(SelectedAssemblyIndex).HourType
                mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompListForComboBox(SelectedCompIndex).CompID, AssemblyStatusID, Today.Date.ToString, mCompListForComboBox(SelectedCompIndex).PartID, ModelID, mCompListForComboBox(SelectedCompIndex).CompStatusID, HourType)
                mCompMonitorModStatus.PartMonitorModID(False) = New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mCompListForComboBox(SelectedCompIndex).CompStatusID, AssemblyStatusID, Today.Date.ToString)
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyList(SelectedAssemblyIndex).MachineID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                Session("IsOpenFromADSB") = "True"
                Session("mCompStatus") = mCompStatus
                Session("mMachine") = mMachine
                Session.Remove("mCompMonitorModStatusList") 'MPD Slow Perf
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCompMonitorModStatus_Ajax.aspx?GChildPage4=index.aspx');", True)
            Case "EditRec"
                EditRecord(New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "DeleteRec"
                DeleteRecord(CInt(e.CommandArgument))
            Case "View"
                mID = New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttachForConfigCompMPD") = mFileAttach
                If mFileAttach.Size > 0 Then
                    'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mManual.FileExtension
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session.Remove("SelectedAssemblyIndexForConfigCompMPD")
        Session.Remove("SelectedAssemblyTypeIndexForConfigCompMPD")
        Session.Remove("ActiveTabindexForConfigCompMPD")
        Session.Remove("SelectedMonitorTypeForConfigCompMPD")
        Session.Remove("ATAForConfigCompMPD")
        Session.Remove("DescriptionForConfigCompMPD")
        Session.Remove("ConfigMPDTabIndex")
        Session("MiddleFrame") = ""
        Session.Remove("SelectedCompIndexForConfigCompMPD") 'Added by Shital on 14-Jun-2019
        'Response.Redirect("index.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgNonConfigList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgNonConfigList.Sorting
        mPartMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorModList") = mPartMonitorModList
        dgNonConfigList.DataSource = mPartMonitorModList
        dgNonConfigList.DataBind()
        SetGrid(False, True)
        SetPage(mCompADSBConfigurableList.Count, mPartMonitorModList.Count)
        ControlVisibility()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        ActiveTabindex = 0
        Session("ActiveTabindexForConfigCompMPD") = ActiveTabindex
        SelectedAssemblyIndex = 0
        Session("SelectedAssemblyIndexForConfigCompMPD") = SelectedAssemblyIndex
        SelectedAssemblyTypeIndex = cmbAssemblyType.SelectedIndex
        Session("SelectedAssemblyTypeIndexForConfigCompMPD") = SelectedAssemblyTypeIndex
        SelectedCompIndex = 0
        Session("SelectedCompIndexForConfigCompMPD") = SelectedCompIndex
        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, MachineID:=Guid.Empty.ToString, InstalledOn:=Today.Date.ToString, AddTopItem:="", IsInstalled:=True, SkipIsForInventoryAircarft:=True)
        Session("mAssemblyListForConfigCompMPD") = mAssemblyList
        cmbAssembly.DataSource = mAssemblyList
        cmbAssembly.DataBind()
        getGridRecords()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        ActiveTabindex = 0
        Session("ActiveTabindexForConfigCompMPD") = ActiveTabindex
        SelectedAssemblyIndex = cmbAssembly.SelectedIndex
        Session("SelectedAssemblyIndexForConfigCompMPD") = SelectedAssemblyIndex
        SelectedCompIndex = 0
        Session("SelectedCompIndexForConfigCompMPD") = SelectedCompIndex
        getGridRecords()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub hdnBtnModHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModHistory.Click
        mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ModType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=Description, IsFromADSB:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
        dgNonConfigList.DataSource = mPartMonitorModList
        dgNonConfigList.DataBind()
        Session("mPartMonitorModList") = mPartMonitorModList
        SetGrid(False, True)
        SetPage(mCompADSBConfigurableList.Count, mPartMonitorModList.Count)
        ControlVisibility()
        upnlTabs.Update()
    End Sub
    Private Sub dgConfigList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgConfigList.RowCommand
        Dim CompStatusID, CompMonitorModStatusID As Guid
        Dim HourType As Integer
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                CompStatusID = mCompListForComboBox(SelectedCompIndex).CompStatusID
                HourType = CInt(dgConfigList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID, Today.Date.ToString)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID)
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyList(SelectedAssemblyIndex).MachineID)
                Session("mCompStatus") = mCompStatus
                Session("IsOpenFromADSB") = "True"
                Session("RegNo") = dgConfigList.Rows(CInt(e.CommandArgument)).Cells(4).Text.ToString
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mMachine") = mMachine
                EditConfiguredRecord(mCompADSBConfigurableList(CInt(e.CommandArgument)).ID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID, mCompListForComboBox(SelectedCompIndex).CompStatusID, HourType)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteConfigRecord")
            Case "History"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                HistoryRecords(mAssemblyList(SelectedAssemblyIndex).MachineID, mCompADSBConfigurableList(CInt(e.CommandArgument)).ID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID, mCompListForComboBox(SelectedCompIndex).CompStatusID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mCompADSBConfigurableList(CInt(e.CommandArgument)).ID)
                Session("mFileAttachForConfigCompMPD") = mFileAttach
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
            
        End Select
    End Sub
    Private Sub dgConfigList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgConfigList.Sorting
        mCompADSBConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
        dgConfigList.DataSource = mCompADSBConfigurableList
        dgConfigList.DataBind()
        SetGrid(True, False)
        SetPage(mCompADSBConfigurableList.Count, mPartMonitorModList.Count)
        ControlVisibility()
    End Sub
    Private Sub TbConfigNonConfig_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbConfigNonConfig.ActiveTabChanged
        ActiveTabindex = TbConfigNonConfig.ActiveTabIndex
        Session("ActiveTabindexForConfigCompMPD") = ActiveTabindex
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        ATA = cmbATAChapter.SelectedIndex
        Session("ATAForConfigCompMPD") = ATA
        getGridRecords()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        SelectedMonitorType = CInt(cmbMonitorType.SelectedValue)
        Session("SelectedMonitorTypeForConfigCompMPD") = SelectedMonitorType
        getGridRecords()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged
        Description = txtDescription.Text.Trim
        Session("DescriptionForConfigCompMPD") = Description
        getGridRecords()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub cmbComponent_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbComponent.SelectedIndexChanged
        SelectedCompIndex = cmbComponent.SelectedIndex
        Session("SelectedCompIndexForConfigCompMPD") = SelectedCompIndex
        mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ModType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=txtDescription.Text.Trim, IsFromADSB:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
        dgNonConfigList.DataSource = mPartMonitorModList
        dgNonConfigList.DataBind()
        Session("mPartMonitorModList") = mPartMonitorModList

        mCompADSBConfigurableList = CompADSBConfigurableList.GetADSBConfigurationList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, PartMonitorModID:=Guid.Empty.ToString, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ModificationType:=SelectedMonitorType, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
        Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
        'mCompMonitorModStatusList = CompMonitorModStatusList.GetCompMonitorModStatusList(CurrentDate:=Today.Date.ToString, CompID:=mCompListForComboBox(SelectedCompIndex).CompID, SerialNo:=mCompListForComboBox(SelectedCompIndex).CompSerialNo, CompStatusPeriodList:=Nothing, IsFromADSB:=True, Description:=Description, ATACode:=mATAList(ATA).ATACode, MonitorTypeID:=SelectedMonitorType, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID.ToString, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsComplied:=True)
        'Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
        dgConfigList.DataSource = mCompADSBConfigurableList
        dgConfigList.DataBind()

        SetGrid()
        SetPage(mCompADSBConfigurableList.Count, mPartMonitorModList.Count)
        ControlVisibility()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
#End Region

End Class