
'Created by : Saylee
'Date:        12-Apr-2019

Imports System.Text
Public Class wfConfigureSBADList_Ajax
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
    Protected mModelMonitorModList As ModelMonitorModList
    Protected mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    ' Dim mADSBConfigurableList As ADSBConfigurableList
    Protected mAssemblyMonitorModStatusList As AssemblyMonitorModStatusList '
    Dim mFileAttach As FileAttach
    Dim mMachine As Machine
    Dim mUpdateComplyHistoryAssemblyMonitorModStatusList As UpdateComplyHistoryAssemblyMonitorModStatusList
    Dim mBoardInfo As AircraftInformationBoard.BoardInfo
    Dim mMachineMaintenance As MachineMaintenance
    Dim mModectionDetail As String
    Public mATAList As ATAList
    Public mModelMonitorModTypeList As ModelMonitorModTypeList
    Dim SelectedAssemblyIndex, AssConfigNonConfigTabIndex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA As Integer
    Dim Description As String = String.Empty
    Dim ConfigADSBTabIndex As Integer
    Dim ADSBNo As String = String.Empty
    Dim mADSBConfigurableList As ADSBConfigurableList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyList = Session("mAssemblyList")
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelMonitorModList = CType(Session("mModelMonitorModList"), ModelMonitorModList)
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mADSBConfigurableList = Session("mADSBConfigurableList")
        SelectedAssemblyIndex = IIf(Session("SelectedAssemblyIndex") Is Nothing, 0, Session("SelectedAssemblyIndex"))
        SelectedAssemblyTypeIndex = IIf(Session("SelectedAssemblyTypeIndex") Is Nothing, 0, Session("SelectedAssemblyTypeIndex"))
        AssConfigNonConfigTabIndex = IIf(Session("AssConfigNonConfigTabIndex") Is Nothing, 0, Session("AssConfigNonConfigTabIndex"))
        ConfigADSBTabIndex = IIf(Session("ConfigADSBTabIndex") Is Nothing, 0, Session("ConfigADSBTabIndex"))
        SelectedMonitorType = IIf(Session("SelectedMonitorType") Is Nothing, 0, Session("SelectedMonitorType"))
        ATA = IIf(Session("ATA") Is Nothing, 0, Session("ATA"))
        Description = IIf(Session("Description") Is Nothing, String.Empty, Session("Description"))
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorModTypeList = CType(Session("mModelMonitorModTypeList"), ModelMonitorModTypeList)
        ADSBNo = IIf(Session("ADSBNo") Is Nothing, String.Empty, Session("ADSBNo"))
        mAssemblyMonitorModStatusList = Session("mAssemblyMonitorModStatusList") 'ADSB Slow
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfConfigureSBADList_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub getGridRecords()
        If mAssemblyList.Count > 0 Then
            cmbAssembly.Enabled = True
            mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromADSB:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ModificationType:=SelectedMonitorType, DirectiveNo:=ADSBNo)
            dgNonConfigList.DataSource = mModelMonitorModList
            dgNonConfigList.DataBind()
            Session("mModelMonitorModList") = mModelMonitorModList

            'ADSB Slow
            mADSBConfigurableList = ADSBConfigurableList.GetADSBConfigurationList(mAssemblyList(SelectedAssemblyIndex).ModelID, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ModType:=SelectedMonitorType, DirectiveNo:=ADSBNo)
            Session("mADSBConfigurableList") = mADSBConfigurableList
            ' mAssemblyMonitorModStatusList = AssemblyMonitorModStatusList.GetAssemblyMonitorModStatuslist(CurrentDate:=Today.Date.ToString, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID, ATACode:=mATAList(ATA).ATACode, Description:=Description, MonitorTypeID:=SelectedMonitorType, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsForConfiguredList:=True, IsComplied:=True)
            'Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
            dgConfigList.DataSource = mADSBConfigurableList
            'End
            dgConfigList.DataBind()

            SetGrid()

            SetPage(mADSBConfigurableList.Count, mModelMonitorModList.Count, IIf(mModelMonitorModList.Count > 0, mModelMonitorModList.RecordCount, 0))
            ControlVisibility()
        Else
            cmbAssembly.Enabled = False
            mModelMonitorModList = Nothing
            'ADSB Slow
            mADSBConfigurableList = Nothing
            Session("mADSBConfigurableList") = mADSBConfigurableList
            'mAssemblyMonitorModStatusList = Nothing
            'Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
            dgConfigList.DataSource = mADSBConfigurableList
            'End
            Session("mModelMonitorModList") = mModelMonitorModList

            dgNonConfigList.DataSource = mModelMonitorModList
            dgNonConfigList.DataBind()

            dgConfigList.DataBind()
            SetGrid()
            SetPage()
            lblConfigResult.Visible = False
            lblNonConfigResult.Visible = False
        End If
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "ConfigureADSB"


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
        Session.Remove("mAssemblyList")
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelMonitorModList")
        Session.Remove("mAssemblyMonitorModStatus")
        Session.Remove("mADSBConfigurableList")
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorModTypeList")
        Session.Remove("ADSBNo")
        Session.Remove("mAssemblyMonitorModStatusList") 'ADSB Slow Perf
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModelMonitorModList.CurrentIndex = Index
        Session("mModelMonitorModList") = mModelMonitorModList
    End Sub
    Private Sub ControlVisibility()
        If Not mModelMonitorModList Is Nothing Then
            lblNonConfigResult.Visible = (mModelMonitorModList.Count > 0)
        End If
        If Not mADSBConfigurableList Is Nothing Then 'ADSB Slow
            lblConfigResult.Visible = (mADSBConfigurableList.Count > 0)
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
                        Dim ModelMonitorModID As Guid
                        Try
                            Session("sender") = ""
                            Dim index As Integer = Session("Index")
                            'ADSB Slow
                            IDForEventLog = mADSBConfigurableList(index).AssemblyMonitorModStatusID
                            ModelMonitorModID = mADSBConfigurableList(index).ModelMonitorModID
                            mModectionDetail = "Aircraft : " + mADSBConfigurableList(index).RegNo + " Monitor Info. : " + mADSBConfigurableList(index).MonitorInfo + " Monitor Type : " + mADSBConfigurableList(index).MonitorType + " Description : " + mADSBConfigurableList(index).Description
                            'IDForEventLog = mAssemblyMonitorModStatusList(index).ID
                            'ModelMonitorModID = mAssemblyMonitorModStatusList(index).ModelMonitorModID
                            'mModectionDetail = "Aircraft : " + mAssemblyList(SelectedAssemblyIndex).RegNo + " Monitor Info. : " + mAssemblyMonitorModStatusList(index).Type + " Monitor Type : " + mAssemblyMonitorModStatusList(index).MonitorType + " Description : " + mAssemblyMonitorModStatusList(index).Description
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(IDForEventLog)
                            '********************************
                            If mADSBConfigurableList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(IDForEventLog)
                            End If
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(IDForEventLog, 6)
                            '=============================
                            AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(IDForEventLog)
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
                            'Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                            If AppSettings("LinkMaintenance") = "True" Then
                                If LinkMaintenanceList.GetLinkMaintenanceList(ModelMonitorModID.ToString).Count > 0 Then
                                    MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                    Exit Sub
                                End If
                            End If
                            'End
                            mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromADSB:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ModificationType:=SelectedMonitorType)
                            dgNonConfigList.DataSource = mModelMonitorModList
                            dgNonConfigList.DataBind()
                            Session("mModelMonitorModList") = mModelMonitorModList

                            'ADSB Slow
                            mADSBConfigurableList = ADSBConfigurableList.GetADSBConfigurationList(mAssemblyList(SelectedAssemblyIndex).ModelID, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ModType:=SelectedMonitorType)
                            Session("mADSBConfigurableList") = mADSBConfigurableList
                            'mAssemblyMonitorModStatusList = AssemblyMonitorModStatusList.GetAssemblyMonitorModStatuslist(CurrentDate:=Today.Date.ToString, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID, ATACode:=mATAList(ATA).ATACode, Description:=Description, MonitorTypeID:=SelectedMonitorType, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsForConfiguredList:=True, IsComplied:=True)
                            'Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
                            'End
                            dgConfigList.DataSource = mADSBConfigurableList
                            dgConfigList.DataBind()

                            SetGrid()
                            SetPage(mADSBConfigurableList.Count, mModelMonitorModList.Count, IIf(mModelMonitorModList.Count > 0, mModelMonitorModList.RecordCount, 0))
                            ControlVisibility()
                            upnlTabs.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "AssemblyModections", "Can't delete :" & mModectionDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyModections", mModectionDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    End If

                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mModectionDetail = "Model : " + mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelName + " ATA : " + mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ATAChapter + " Description : " + mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).Description
                            If mModelMonitorModList(mModelMonitorModList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mModelMonitorModList(mModelMonitorModList.CurrentIndex).ID)
                            End If
                            ModelMonitorMod.DeleteModelMonitorMod(mModelMonitorModList.CurrentItem.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            MarkLog(Util.Action.Delete, "Model Modection", mModectionDetail, Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
                            mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromADSB:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ModificationType:=SelectedMonitorType)
                            dgNonConfigList.DataSource = mModelMonitorModList
                            dgNonConfigList.DataBind()
                            SetGrid()
                            SetPage(mADSBConfigurableList.Count, mModelMonitorModList.Count, IIf(mModelMonitorModList.Count > 0, mModelMonitorModList.RecordCount, 0))
                            ControlVisibility()
                            upnlTabs.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "") 'Added by Vikrant on 28-July-2011
                                MarkLog(Util.Action.Delete, "Model Modection", "Can't Delete:" & mModectionDetail & " is already in use", Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    If MSGBoxCtrl.Sender = "LinkMaintenance" Then
                        getGridRecords()
                        upnlTabs.Update()
                    End If
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
    Private Sub SetPage(Optional ByVal ConfigADSBCount As Integer = 0, Optional ByVal NonConfigADSBCount As Integer = 0, Optional ByVal NonConfigADSBTotalCount As Integer = 0)
        If mAssemblyList.Count > 0 Then
            lblConfigResult.Text = "List of Configured ADs/SBs for Model '" + mAssemblyList(SelectedAssemblyIndex).ModelName + "' on Aircraft '" + mAssemblyList(SelectedAssemblyIndex).RegNo + "' : " + ConfigADSBCount.ToString + " Record(s)"
            lblNonConfigResult.Text = "List of Non Configured ADs/SBs for Model '" + mAssemblyList(SelectedAssemblyIndex).ModelName + "' : " + NonConfigADSBCount.ToString + " Record(s)"
        End If
        lblConfigTabPanel.Text = "Configured(" + ConfigADSBCount.ToString + ")"

        If AppSettings("ClientCode") = "IAT" Then
            lblNonConfigTabPanel.Text = "Non-Configured(" + NonConfigADSBTotalCount.ToString + ")"
        Else
            lblNonConfigTabPanel.Text = "Non-Configured(" + NonConfigADSBCount.ToString + ")"
        End If

    End Sub
    Private Sub SetGrid(Optional ByVal IsConfigGrid As Boolean = True, Optional ByVal IsNonConfigGrid As Boolean = True)
        Dim P, C, IsReadOnly As Boolean


        If IsConfigGrid Then
            For j As Integer = 0 To dgConfigList.Rows.Count - 1
                C = CType(Me.dgConfigList.Rows(j).Cells(25).Text, Boolean) 'IsMaster
                P = CType(Me.dgConfigList.Rows(j).Cells(27).Text, Boolean) 'IsAttachmentAdded
                IsReadOnly = CType(Me.dgConfigList.Rows(j).Cells(28).Text, Boolean) 'IsReadOnly

                If C = True Or IsReadOnly = True Then
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
                P = CType(Me.dgNonConfigList.Rows(j).Cells(15).Text, Boolean)
                If P = False Then
                    dgNonConfigList.Rows(j).Cells(14).Enabled = False
                End If
                dgNonConfigList.Rows(j).Cells(11).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = False, True, False) 'Config

            Next
        End If
        lblReadOnly.Visible = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, True, False)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mModelMonitorMod As ModelMonitorMod
        mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mId, mAssemblyList(SelectedAssemblyIndex).HourType) 'HourType=1 as diff is only show purpose H OR HD
        Session("mModelMonitorMod") = mModelMonitorMod
        mModectionDetail = "Model : " & mAssemblyList(SelectedAssemblyIndex).ModelName & " Model Modection Type : " & mModelMonitorMod.ModelMonitorModTypeName & " Description : " & mModelMonitorMod.Description
        MarkLog(Util.Action.Edit, "Model Modection", mModectionDetail, Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
        Session("ModelIDForADSB") = mAssemblyList(SelectedAssemblyIndex).ModelID
        Session("ModelName") = mAssemblyList(SelectedAssemblyIndex).ModelName
        Session("IsFromADSBConfig") = True
        Session.Remove("mAssemblyMonitorModStatusList") 'ADSB Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewADSB_Ajax.aspx?BackPage=wfConfigureSBADList_Ajax.aspx');", True)
    End Sub
    Private Sub EditConfiguredRecord(ByVal AssemblyMonitorModStatusID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(AssemblyMonitorModStatusID, AssemblyStausID, HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("Edit") = True
        Session.Remove("mAssemblyMonitorModStatusList") 'ADSB Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=index.aspx');", True)
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal AssemblyMonitorModStatusID As Guid, ByVal AssemblyStatusID As Guid)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(AssemblyMonitorModStatusID, AssemblyStatusID, mMachine.HourType)

        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)

        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorModStatus.ModelMonitorModID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModHistoryWindow", "OpenModHistoryWindow();", True)
        'End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        cmbAssemblyType.DataBind()

        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, MachineID:=Guid.Empty.ToString, InstalledOn:=Today.Date.ToString, AddTopItem:="", IsInstalled:=True, SkipIsForInventoryAircarft:=True)
        cmbAssembly.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList
        cmbAssembly.DataBind()

        mATAList = ATAList.GetATAList("", "(All)") 'Added By Saylee on 12-Aug-2010
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(All)")
        cmbMonitorType.DataSource = mModelMonitorModTypeList
        cmbMonitorType.DataBind()
        Session("mModelMonitorModTypeList") = mModelMonitorModTypeList

        getGridRecords()
        cmbAssemblyType.SelectedIndex = SelectedAssemblyTypeIndex
        cmbAssembly.SelectedIndex = SelectedAssemblyIndex
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        If Session("MiddleFrame") <> "wfConfigureSBADList_Ajax.aspx?" Then
            Session.Remove("ADSBNo")
        End If
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            TbConfigAssemblyADSB.ActiveTabIndex = IIf(CType(Session("ConfigADSBTabIndex"), Integer) > 0, CType(Session("ConfigADSBTabIndex"), Integer), 0)
            If TbConfigAssemblyADSB.ActiveTabIndex = 1 Then 'Comp Tab
                TbConfigAssemblyADSB_ActiveTabChanged(sender, e)
            Else
                Session("MiddleFrame") = "wfConfigureSBADList_Ajax.aspx?"
                DataFieldBind()
                SetGrid()

                TbConfigNonConfig.ActiveTabIndex = IIf(CType(Session("AssConfigNonConfigTabIndex"), Integer) > 0, CType(Session("AssConfigNonConfigTabIndex"), Integer), 0)
                ControlVisibility()
            End If
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
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)
                mAssemblyMonitorModStatus.ModelMonitorModID(False) = New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                Session("IsOpenFromADSB") = "True"
                Session("RegNo") = mAssemblyList(SelectedAssemblyIndex).RegNo
                Session.Remove("mAssemblyMonitorModStatusList") 'ADSB Slow Perf
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=index.aspx');", True)
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
                Session("mFileAttach") = mFileAttach
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
        Session.Remove("SelectedAssemblyIndex")
        Session.Remove("SelectedAssemblyTypeIndex")
        Session.Remove("AssConfigNonConfigTabIndex")
        Session.Remove("SelectedMonitorType")
        Session.Remove("ATA")
        Session.Remove("Description")
        Session.Remove("ConfigADSBTabIndex")
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgNonConfigList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgNonConfigList.Sorting
        mModelMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorModList") = mModelMonitorModList
        dgNonConfigList.DataSource = mModelMonitorModList
        dgNonConfigList.DataBind()
        SetGrid(False, True)
        SetPage(mADSBConfigurableList.Count, mModelMonitorModList.Count, IIf(mModelMonitorModList.Count > 0, mModelMonitorModList.RecordCount, 0))
        ControlVisibility()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        AssConfigNonConfigTabIndex = 0
        Session("AssConfigNonConfigTabIndex") = AssConfigNonConfigTabIndex
        SelectedAssemblyIndex = 0
        Session("SelectedAssemblyIndex") = SelectedAssemblyIndex
        SelectedAssemblyTypeIndex = cmbAssemblyType.SelectedIndex
        Session("SelectedAssemblyTypeIndex") = SelectedAssemblyTypeIndex
        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, MachineID:=Guid.Empty.ToString, InstalledOn:=Today.Date.ToString, AddTopItem:="", IsInstalled:=True, SkipIsForInventoryAircarft:=True)
        Session("mAssemblyList") = mAssemblyList
        cmbAssembly.DataSource = mAssemblyList
        cmbAssembly.DataBind()
        getGridRecords()
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        AssConfigNonConfigTabIndex = 0
        Session("AssConfigNonConfigTabIndex") = AssConfigNonConfigTabIndex
        SelectedAssemblyIndex = cmbAssembly.SelectedIndex
        Session("SelectedAssemblyIndex") = SelectedAssemblyIndex
        mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromADSB:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ModificationType:=SelectedMonitorType)
        dgNonConfigList.DataSource = mModelMonitorModList
        dgNonConfigList.DataBind()
        'ADSB Slow
        mADSBConfigurableList = ADSBConfigurableList.GetADSBConfigurationList(mAssemblyList(SelectedAssemblyIndex).ModelID, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ModType:=SelectedMonitorType)
        Session("mADSBConfigurableList") = mADSBConfigurableList
        'mAssemblyMonitorModStatusList = AssemblyMonitorModStatusList.GetAssemblyMonitorModStatuslist(CurrentDate:=Today.Date.ToString, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID, ATACode:=mATAList(ATA).ATACode, Description:=Description, MonitorTypeID:=SelectedMonitorType, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsForConfiguredList:=True, IsComplied:=True)
        ' Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        dgConfigList.DataSource = mADSBConfigurableList
        'End
        dgConfigList.DataBind()

        Session("mModelMonitorModList") = mModelMonitorModList
        SetGrid()
        SetPage(mADSBConfigurableList.Count, mModelMonitorModList.Count, IIf(mModelMonitorModList.Count > 0, mModelMonitorModList.RecordCount, 0))
        ControlVisibility()
    End Sub
    Private Sub hdnBtnModelModMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelModMaster.Click
        mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromADSB:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ModificationType:=SelectedMonitorType)
        dgNonConfigList.DataSource = mModelMonitorModList
        dgNonConfigList.DataBind()
        Session("mModelMonitorModList") = mModelMonitorModList
        SetGrid(False, True)
        SetPage(mADSBConfigurableList.Count, mModelMonitorModList.Count, IIf(mModelMonitorModList.Count > 0, mModelMonitorModList.RecordCount, 0))
        ControlVisibility()
        upnlTabs.Update()
    End Sub
    Private Sub dgConfigList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgConfigList.RowCommand
        Dim AssemblyStatusID As Guid
        Dim HourType As Integer
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                AssemblyStatusID = mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID
                HourType = CInt(dgConfigList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("IsOpenFromADSB") = "True"
                Session("RegNo") = dgConfigList.Rows(CInt(e.CommandArgument)).Cells(4).Text.ToString
                EditConfiguredRecord(mADSBConfigurableList(CInt(e.CommandArgument)).ID, AssemblyStatusID, HourType)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteConfigRecord")
                'mTmpComplyAssemblyMonitorModStatusList.CurrentIndex = index
                'Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
            Case "History"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                HistoryRecords(mAssemblyList(SelectedAssemblyIndex).MachineID, mADSBConfigurableList(CInt(e.CommandArgument)).ID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mADSBConfigurableList(CInt(e.CommandArgument)).ID)
                Session("mFileAttach") = mFileAttach
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
        mADSBConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mADSBConfigurableList") = mADSBConfigurableList
        dgConfigList.DataSource = mADSBConfigurableList
        dgConfigList.DataBind()
        SetGrid(True, False)
        SetPage(mADSBConfigurableList.Count, mModelMonitorModList.Count, IIf(mModelMonitorModList.Count > 0, mModelMonitorModList.RecordCount, 0))
        ControlVisibility()
    End Sub
    Private Sub TbConfigNonConfig_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbConfigNonConfig.ActiveTabChanged
        AssConfigNonConfigTabIndex = TbConfigNonConfig.ActiveTabIndex
        Session("AssConfigNonConfigTabIndex") = AssConfigNonConfigTabIndex
    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        ATA = cmbATAChapter.SelectedIndex
        Session("ATA") = ATA
        getGridRecords()
        upnlTabs.Update()
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        SelectedMonitorType = CInt(cmbMonitorType.SelectedValue)
        Session("SelectedMonitorType") = SelectedMonitorType
        getGridRecords()
        upnlTabs.Update()
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged, txtADSBNo.TextChanged
        Description = txtDescription.Text.Trim
        Session("Description") = Description
        ADSBNo = txtADSBNo.Text.Trim
        Session("ADSBNo") = ADSBNo

        getGridRecords()
        upnlTabs.Update()
    End Sub
    Private Sub TbConfigAssemblyADSB_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbConfigAssemblyADSB.ActiveTabChanged
        ConfigADSBTabIndex = TbConfigAssemblyADSB.ActiveTabIndex
        Session("ConfigADSBTabIndex") = ConfigADSBTabIndex
        Select Case TbConfigAssemblyADSB.ActiveTabIndex
            Case 0 'Assembly Tab
                Session.Remove("mAssemblyListForConfigCompADSB")
                Session.Remove("mAssemblyTypeListForConfigCompADSB")
                Session.Remove("mPartMonitorModList")
                Session.Remove("mCompMonitorModStatus")
                Session.Remove("mCompADSBConfigurableList")
                Session.Remove("mATAListForConfigCompADSB")
                Session.Remove("mPartMonitorModTypeListForConfigCompADSB")
                Session.Remove("mCompListForComboBox")
                Session.Remove("SelectedAssemblyIndexForConfigCompADSB")
                Session.Remove("SelectedAssemblyTypeIndexForConfigCompADSB")
                Session.Remove("ActiveTabindexForConfigCompADSB")
                Session.Remove("SelectedMonitorTypeForConfigCompADSB")
                Session.Remove("ATAForConfigCompADSB")
                Session.Remove("DescriptionForConfigCompADSB")
                DataFieldBind()
                SetGrid()
                TbConfigNonConfig.ActiveTabIndex = IIf(CType(Session("AssConfigNonConfigTabIndex"), Integer) > 0, CType(Session("AssConfigNonConfigTabIndex"), Integer), 0)
                ControlVisibility()
            Case 1 'Component Tab
                Session.Remove("mAssemblyList")
                Session.Remove("mAssemblyTypeList")
                Session.Remove("mModelMonitorModList")
                Session.Remove("mAssemblyMonitorModStatus")
                Session.Remove("mADSBConfigurableList")
                Session.Remove("mATAList")
                Session.Remove("mModelMonitorModTypeList")
                Session.Remove("SelectedAssemblyIndex")
                Session.Remove("SelectedAssemblyTypeIndex")
                Session.Remove("AssConfigNonConfigTabIndex")
                Session.Remove("SelectedMonitorType")
                Session.Remove("ATA")
                Session.Remove("Description")
                Session.Remove("mAssemblyMonitorModStatusList")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCompADSBConfigureList", "CallCompADSBConfigureList();", True)
        End Select
    End Sub
#End Region


End Class