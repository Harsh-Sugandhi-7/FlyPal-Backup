

'Created By: Saylee on 13-Dec-2024



Imports System.Text
Public Class wfConfigureMPDAMPList_Ajax
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
    Protected mModelMonitorServiceList As ModelMonitorServiceList
    Protected mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Dim mMPDConfigurableList As MPDConfigurableList
    Protected mAssemblyMonitorServiceStatusList As AssemblyMonitorServiceStatusList 'MPD Slow
    Dim mFileAttach As FileAttach
    Dim mMachine As Machine
    Dim mUpdateComplyHistoryAssemblyMonitorServiceStatusList As UpdateComplyHistoryAssemblyMonitorServiceStatusList
    Dim mBoardInfo As AircraftInformationBoard.BoardInfo
    Dim mMachineMaintenance As MachineMaintenance
    Dim mServiceDetail As String
    Public mATAList As ATAList
    Public mModelMonitorServiceTypeList As ModelMonitorServiceTypeList
    Dim SelectedAssemblyIndex, AssConfigNonConfigTabIndex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA As Integer
    Dim Description As String = String.Empty
    Dim ConfigMPDTabIndex As Integer
    Dim MPDNo As String = String.Empty
    Dim Frequency As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyList = Session("mAssemblyList")
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelMonitorServiceList = CType(Session("mModelMonitorServiceList"), ModelMonitorServiceList)
        mAssemblyMonitorServiceStatus = CType(Session("mAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        mMPDConfigurableList = Session("mMPDConfigurableList")
        SelectedAssemblyIndex = IIf(Session("SelectedAssemblyIndex") Is Nothing, 0, Session("SelectedAssemblyIndex"))
        SelectedAssemblyTypeIndex = IIf(Session("SelectedAssemblyTypeIndex") Is Nothing, 0, Session("SelectedAssemblyTypeIndex"))
        AssConfigNonConfigTabIndex = IIf(Session("AssConfigNonConfigTabIndex") Is Nothing, 0, Session("AssConfigNonConfigTabIndex"))
        ConfigMPDTabIndex = IIf(Session("ConfigMPDTabIndex") Is Nothing, 0, Session("ConfigMPDTabIndex"))
        SelectedMonitorType = IIf(Session("SelectedMonitorType") Is Nothing, 0, Session("SelectedMonitorType"))
        ATA = IIf(Session("ATA") Is Nothing, 0, Session("ATA"))
        Description = IIf(Session("Description") Is Nothing, String.Empty, Session("Description"))
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorServiceTypeList = CType(Session("mModelMonitorServiceTypeList"), ModelMonitorServiceTypeList)
        MPDNo = IIf(Session("MPDNo") Is Nothing, String.Empty, Session("MPDNo"))
        mAssemblyMonitorServiceStatusList = Session("mAssemblyMonitorServiceStatusListMPD") 'MPD Slow
        Frequency = IIf(Session("Frequency") Is Nothing, String.Empty, Session("Frequency"))
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfConfigureMPDList_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub getGridRecords()
        If mAssemblyList.Count > 0 Then
            cmbAssembly.Enabled = True
            mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromMPD:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ServiceType:=SelectedMonitorType, TaskCardNo:=MPDNo, Frequency:=Frequency)
            dgNonConfigList.DataSource = mModelMonitorServiceList
            dgNonConfigList.DataBind()
            Session("mModelMonitorServiceList") = mModelMonitorServiceList

            'MPD Slow
            'mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(mAssemblyList(SelectedAssemblyIndex).ModelID, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ServiceType:=SelectedMonitorType)
            'Session("mMPDConfigurableList") = mMPDConfigurableList
            mAssemblyMonitorServiceStatusList = AssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatuslist(CurrentDate:=Today.Date.ToString, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID, ATACode:=mATAList(ATA).ATACode, Description:=Description, MonitorTypeID:=SelectedMonitorType, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsServiceStatusPeriodsRequired:=False, IsForConfiguredList:=True, IsComplied:=True, CodeFormNoDesc:=MPDNo)
            Session("mAssemblyMonitorServiceStatusListMPD") = mAssemblyMonitorServiceStatusList
            dgConfigList.DataSource = mAssemblyMonitorServiceStatusList
            'End
            dgConfigList.DataBind()

            SetGrid()

            SetPage(mAssemblyMonitorServiceStatusList.Count, mModelMonitorServiceList.Count, IIf(mModelMonitorServiceList.Count > 0, mModelMonitorServiceList.RecordCount, 0))
            ControlVisibility()
        Else
            cmbAssembly.Enabled = False
            mModelMonitorServiceList = Nothing
            'MPD Slow
            'mMPDConfigurableList = Nothing
            'Session("mMPDConfigurableList") = mMPDConfigurableList
            mAssemblyMonitorServiceStatusList = Nothing
            Session("mAssemblyMonitorServiceStatusListMPD") = mAssemblyMonitorServiceStatusList
            dgConfigList.DataSource = mAssemblyMonitorServiceStatusList
            'End
            Session("mModelMonitorServiceList") = mModelMonitorServiceList

            dgNonConfigList.DataSource = mModelMonitorServiceList
            dgNonConfigList.DataBind()

            dgConfigList.DataBind()
            SetGrid()
            SetPage()
            lblConfigResult.Visible = False
            lblNonConfigResult.Visible = False
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
        Session.Remove("mAssemblyList")
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelMonitorServiceList")
        Session.Remove("mAssemblyMonitorServiceStatus")
        Session.Remove("mMPDConfigurableList")
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorServiceTypeList")
        Session.Remove("MPDNo")
        Session.Remove("mAssemblyMonitorServiceStatusListMPD") 'MPD Slow Perf
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModelMonitorServiceList.CurrentIndex = Index
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
    End Sub
    Private Sub ControlVisibility()
        If Not mModelMonitorServiceList Is Nothing Then
            lblNonConfigResult.Visible = (mModelMonitorServiceList.Count > 0)
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                dgNonConfigList.HeaderRow.Cells(2).Text = "Task No."
            Else
                dgNonConfigList.HeaderRow.Cells(2).Text = "Code/Form No."
            End If

        End If
        If Not mAssemblyMonitorServiceStatusList Is Nothing Then 'MPD Slow
            lblConfigResult.Visible = (mAssemblyMonitorServiceStatusList.Count > 0)
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                dgConfigList.HeaderRow.Cells(5).Text = "Task No."
                dgConfigList.HeaderRow.Cells(7).Text = "Description"
            Else
                dgConfigList.Columns(5).Visible = False
                dgConfigList.HeaderRow.Cells(7).Text = "Code/Form No./Description"
            End If
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
                        Dim ModelMonitorServiceID As Guid
                        Try
                            Session("sender") = ""
                            Dim index As Integer = Session("Index")
                            'MPD Slow
                            'IDForEventLog = mMPDConfigurableList(index).AssemblyMonitorServiceStatusID
                            'ModelMonitorServiceID = mMPDConfigurableList(index).ModelMonitorServiceID
                            'mServiceDetail = "Aircraft : " + mMPDConfigurableList(index).RegNo + " Monitor Info. : " + mMPDConfigurableList(index).MonitorInfo + " Monitor Type : " + mMPDConfigurableList(index).MonitorType + " Description : " + mMPDConfigurableList(index).Description
                            IDForEventLog = mAssemblyMonitorServiceStatusList(index).ID
                            ModelMonitorServiceID = mAssemblyMonitorServiceStatusList(index).ModelMonitorServiceID
                            mServiceDetail = "Aircraft : " + mAssemblyList(SelectedAssemblyIndex).RegNo + " Monitor Info. : " + mAssemblyMonitorServiceStatusList(index).Type + " Monitor Type : " + mAssemblyMonitorServiceStatusList(index).MonitorType + " Description : " + mAssemblyMonitorServiceStatusList(index).Description
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(IDForEventLog)
                            '********************************
                            If mAssemblyMonitorServiceStatusList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(IDForEventLog)
                            End If
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(IDForEventLog, 6)
                            '=============================
                            AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(IDForEventLog)
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
                                If LinkMaintenanceList.GetLinkMaintenanceList(ModelMonitorServiceID.ToString).Count > 0 Then
                                    MSGBoxCtrl.Show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                    Exit Sub
                                End If
                            End If
                            'End
                            mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromMPD:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ServiceType:=SelectedMonitorType, Frequency:=Frequency)
                            dgNonConfigList.DataSource = mModelMonitorServiceList
                            dgNonConfigList.DataBind()
                            Session("mModelMonitorServiceList") = mModelMonitorServiceList

                            'MPD Slow
                            'mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(mAssemblyList(SelectedAssemblyIndex).ModelID, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ServiceType:=SelectedMonitorType)
                            'Session("mMPDConfigurableList") = mMPDConfigurableList
                            mAssemblyMonitorServiceStatusList = AssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatuslist(CurrentDate:=Today.Date.ToString, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID, ATACode:=mATAList(ATA).ATACode, Description:=Description, MonitorTypeID:=SelectedMonitorType, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsServiceStatusPeriodsRequired:=False, IsForConfiguredList:=True, IsComplied:=True)
                            Session("mAssemblyMonitorServiceStatusListMPD") = mAssemblyMonitorServiceStatusList
                            'End
                            dgConfigList.DataSource = mAssemblyMonitorServiceStatusList
                            dgConfigList.DataBind()

                            SetGrid()
                            SetPage(mAssemblyMonitorServiceStatusList.Count, mModelMonitorServiceList.Count, IIf(mModelMonitorServiceList.Count > 0, mModelMonitorServiceList.RecordCount, 0))
                            ControlVisibility()
                            upnlTabs.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "AssemblyServices", "Can't delete :" & mServiceDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyServices", mServiceDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    End If

                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mServiceDetail = "Model : " + mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ModelName + " ATA : " + mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ATAChapter + " Description : " + mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).Description
                            If mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).ID)
                            End If
                            ModelMonitorService.DeleteModelMonitorService(mModelMonitorServiceList.CurrentItem.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            MarkLog(Util.Action.Delete, "Model Service", mServiceDetail, Util.ErrorType.NoError, mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ID, EventLogID)
                            mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromMPD:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ServiceType:=SelectedMonitorType, Frequency:=Frequency)
                            dgNonConfigList.DataSource = mModelMonitorServiceList
                            dgNonConfigList.DataBind()
                            SetGrid()
                            SetPage(mAssemblyMonitorServiceStatusList.Count, mModelMonitorServiceList.Count, IIf(mModelMonitorServiceList.Count > 0, mModelMonitorServiceList.RecordCount, 0))
                            ControlVisibility()
                            upnlTabs.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "") 'Added by Vikrant on 28-July-2011
                                MarkLog(Util.Action.Delete, "Model Service", "Can't Delete:" & mServiceDetail & " is already in use", Util.ErrorType.NoError, mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ID, EventLogID)
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
    Private Sub SetPage(Optional ByVal ConfigMPDCount As Integer = 0, Optional ByVal NonConfigMPDCount As Integer = 0, Optional ByVal NonConfigMPDTotalCount As Integer = 0)
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
        Else
            ServiceMPDTitle = "Services"
        End If


        If mAssemblyList.Count > 0 Then
            lblConfigResult.Text = "List of Configured " + ServiceMPDTitle + " for Model '" + mAssemblyList(SelectedAssemblyIndex).ModelName + "' on Aircraft '" + mAssemblyList(SelectedAssemblyIndex).RegNo + "' : " + ConfigMPDCount.ToString + " Record(s)"
            lblNonConfigResult.Text = "List of Non Configured " + ServiceMPDTitle + " for Model '" + mAssemblyList(SelectedAssemblyIndex).ModelName + "' : " + NonConfigMPDCount.ToString + " Record(s)"
        End If
        lblConfigTabPanel.Text = "Configured(" + ConfigMPDCount.ToString + ")"

        If AppSettings("ClientCode") = "IAT" Then
            lblNonConfigTabPanel.Text = "Non-Configured(" + NonConfigMPDTotalCount.ToString + ")"
        Else
            lblNonConfigTabPanel.Text = "Non-Configured(" + NonConfigMPDCount.ToString + ")"
        End If

    End Sub
    Private Sub SetGrid(Optional ByVal IsConfigGrid As Boolean = True, Optional ByVal IsNonConfigGrid As Boolean = True)
        Dim P, C, IsReadOnly As Boolean
        ' If IsConfigGrid Then
        'For j As Integer = 0 To dgConfigList.Rows.Count - 1
        'C = CType(Me.dgConfigList.Rows(j).Cells(22).Text, Boolean) 'IsMaster
        'P = CType(Me.dgConfigList.Rows(j).Cells(27).Text, Boolean) 'IsAttachmentAdded

        'If C = True Then
        '    dgConfigList.Rows(j).Cells(24).Enabled = False 'History
        'End If
        'If P = False Then
        '    dgConfigList.Rows(j).Cells(26).Enabled = False 'View
        'End If

        'dgConfigList.Rows(j).Cells(23).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, False, True) 'Delete
        ' dgConfigList.Rows(j).Cells(22).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, False, True) 'Edit


        ' Next
        ' End If
        If IsNonConfigGrid Then
            For j As Integer = 0 To dgNonConfigList.Rows.Count - 1
                'P = CType(Me.dgNonConfigList.Rows(j).Cells(15).Text, Boolean)
                'If P = False Then
                '    dgNonConfigList.Rows(j).Cells(14).Enabled = False
                'End If
                dgNonConfigList.Rows(j).Cells(11).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = False, True, False) 'Config
            Next
        End If
        lblReadOnly.Visible = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, True, False)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mModelMonitorService As ModelMonitorService
        mModelMonitorService = ModelMonitorService.GetModelMonitorService(mId, mAssemblyList(SelectedAssemblyIndex).HourType) 'HourType=1 as diff is only show purpose H OR HD
        Session("mModelMonitorService") = mModelMonitorService
        mServiceDetail = "Model : " & mAssemblyList(SelectedAssemblyIndex).ModelName & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description
        MarkLog(Util.Action.Edit, "Model Service", mServiceDetail, Util.ErrorType.NoError, mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ID, EventLogID)
        Session("ModelIDForMPD") = mAssemblyList(SelectedAssemblyIndex).ModelID
        Session("ModelName") = mAssemblyList(SelectedAssemblyIndex).ModelName
        Session("IsFromMPDConfig") = True
        Session.Remove("mAssemblyMonitorServiceStatusListMPD") 'MPD Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewMPDService_Ajax.aspx?BackPage=wfConfigureMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub EditConfiguredRecord(ByVal AssemblyMonitorServiceStatusID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(AssemblyMonitorServiceStatusID, AssemblyStausID, HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("Edit") = True
        Session.Remove("mAssemblyMonitorServiceStatusListMPD") 'MPD Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=index.aspx');", True)
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal AssemblyMonitorServiceStatusID As Guid, ByVal AssemblyStatusID As Guid)
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(AssemblyMonitorServiceStatusID, AssemblyStatusID, mMachine.HourType)

        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)

        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow();", True)
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

        mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("(All)")
        cmbMonitorType.DataSource = mModelMonitorServiceTypeList
        cmbMonitorType.DataBind()
        Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList

        getGridRecords()
        cmbAssemblyType.SelectedIndex = SelectedAssemblyTypeIndex
        cmbAssembly.SelectedIndex = SelectedAssemblyIndex
        txtFrequency.Text = Frequency
        txtMPDNo.Text = MPDNo
        txtDescription.Text = Description
        cmbMonitorType.SelectedIndex = SelectedMonitorType
        cmbATAChapter.SelectedIndex = ATA
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        If Session("MiddleFrame") <> "wfConfigureMPDList_Ajax.aspx" Then
            Session.Remove("MPDNo")
            Session.Remove("mAssemblyMonitorServiceStatusListMPD")
            Session.Remove("IsTabIndexChaged")
            Session.Remove("ConfigMPDTabIndex")
            Session.Remove("SelectedModelIndex")
            Session.Remove("SelectedAssemblyTypeIndex")
            Session.Remove("SelectedMonitorType")
            Session.Remove("ATA")
            Session.Remove("ModelDescription")
        End If
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            ' Session("MiddleFrame") = "wfConfigureCompMPDList_Ajax.aspx"
            DataFieldBind()
            SetGrid()
            ControlVisibility()
            TbConfigNonConfig.ActiveTabIndex = IIf(CType(Session("ActiveTabindexForConfigMPDAMP"), Integer) > 0, CType(Session("ActiveTabindexForConfigMPDAMP"), Integer), 0)
        End If
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
            If Not dgNonConfigList.HeaderRow Is Nothing Then dgNonConfigList.HeaderRow.Cells(2).Text = "Task No."

            dgNonConfigList.ToolTip = "MPD List"
            lblMonitorType.InnerText = "Maintenance Event"
            lblCodeTaskNo.InnerText = "Task No."
            If Not dgConfigList.HeaderRow Is Nothing Then dgConfigList.HeaderRow.Cells(5).Text = "Task No."
            If Not dgConfigList.HeaderRow Is Nothing Then dgConfigList.HeaderRow.Cells(7).Text = "Description"
        Else
            ServiceMPDTitle = "Services"
            If Not dgNonConfigList.HeaderRow Is Nothing Then dgNonConfigList.HeaderRow.Cells(2).Text = "Code/Form No."

            dgNonConfigList.ToolTip = "Model Service List"
            lblMonitorType.InnerText = "Service Type"
            lblCodeTaskNo.InnerText = "Code/Form No."
            dgConfigList.Columns(5).Visible = False
            If Not dgConfigList.HeaderRow Is Nothing Then dgConfigList.HeaderRow.Cells(7).Text = "Code/Form No./Description"
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
                mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)
                mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mMachine") = Machine.GetMachine(mAssemblyList(SelectedAssemblyIndex).MachineID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = mAssemblyList(SelectedAssemblyIndex).RegNo
                Session.Remove("mAssemblyMonitorServiceStatusListMPD") 'MPD Slow Perf
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=index.aspx');", True)
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
        Session.Remove("ConfigMPDTabIndex")
        Session.Remove("Frequency")
        Session("MiddleFrame") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgNonConfigList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgNonConfigList.Sorting
        mModelMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        dgNonConfigList.DataSource = mModelMonitorServiceList
        dgNonConfigList.DataBind()
        SetGrid(False, True)
        SetPage(mAssemblyMonitorServiceStatusList.Count, mModelMonitorServiceList.Count, IIf(mModelMonitorServiceList.Count > 0, mModelMonitorServiceList.RecordCount, 0))
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
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        AssConfigNonConfigTabIndex = 0
        Session("AssConfigNonConfigTabIndex") = AssConfigNonConfigTabIndex
        SelectedAssemblyIndex = cmbAssembly.SelectedIndex
        Session("SelectedAssemblyIndex") = SelectedAssemblyIndex
        mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromMPD:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ServiceType:=SelectedMonitorType, Frequency:=Frequency)
        dgNonConfigList.DataSource = mModelMonitorServiceList
        dgNonConfigList.DataBind()
        'MPD Slow
        'mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(mAssemblyList(SelectedAssemblyIndex).ModelID, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ServiceType:=SelectedMonitorType)
        'Session("mMPDConfigurableList") = mMPDConfigurableList
        mAssemblyMonitorServiceStatusList = AssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatuslist(CurrentDate:=Today.Date.ToString, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID, ATACode:=mATAList(ATA).ATACode, Description:=Description, MonitorTypeID:=SelectedMonitorType, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsServiceStatusPeriodsRequired:=False, IsForConfiguredList:=True, IsComplied:=True)
        Session("mAssemblyMonitorServiceStatusListMPD") = mAssemblyMonitorServiceStatusList
        dgConfigList.DataSource = mAssemblyMonitorServiceStatusList
        'End
        dgConfigList.DataBind()

        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        SetGrid()
        SetPage(mAssemblyMonitorServiceStatusList.Count, mModelMonitorServiceList.Count, IIf(mModelMonitorServiceList.Count > 0, mModelMonitorServiceList.RecordCount, 0))
        ControlVisibility()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub hdnBtnModelServiceMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelServiceMaster.Click
        mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(mAssemblyList(SelectedAssemblyIndex).ModelID, IsFromMPD:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, Description:=Description, ServiceType:=SelectedMonitorType, Frequency:=Frequency)
        dgNonConfigList.DataSource = mModelMonitorServiceList
        dgNonConfigList.DataBind()
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        SetGrid(False, True)
        SetPage(mAssemblyMonitorServiceStatusList.Count, mModelMonitorServiceList.Count, IIf(mModelMonitorServiceList.Count > 0, mModelMonitorServiceList.RecordCount, 0))
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
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = dgConfigList.Rows(CInt(e.CommandArgument)).Cells(4).Text.ToString
                EditConfiguredRecord(mAssemblyMonitorServiceStatusList(CInt(e.CommandArgument)).ID, AssemblyStatusID, HourType)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteConfigRecord")
                'mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex = index
                'Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
            Case "History"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                HistoryRecords(mAssemblyList(SelectedAssemblyIndex).MachineID, mAssemblyMonitorServiceStatusList(CInt(e.CommandArgument)).ID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatusList(CInt(e.CommandArgument)).ID)
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
            Case "ShowVal"
                Dim AssemblyMonitorServiceStatusIDs As New stringbuilder
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                AssemblyMonitorServiceStatusIDs.Append("<AssMonServiceID>")
                AssemblyMonitorServiceStatusIDs.Append("<id>")
                AssemblyMonitorServiceStatusIDs.Append(New Guid(currentRow.Cells(0).Text))
                AssemblyMonitorServiceStatusIDs.Append("</id>")
                AssemblyMonitorServiceStatusIDs.Append("</AssMonServiceID>")


                Dim mtmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
                mtmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList([Date]:=Today.Date.ToString, MachineID:=mAssemblyList(cmbAssembly.SelectedIndex).MachineID.ToString, Model:=mAssemblyList(cmbAssembly.SelectedIndex).ModelName, SerialNo:=mAssemblyList(cmbAssembly.SelectedIndex).SerialNo, AssemblyMonitorServiceStatusIDs:=AssemblyMonitorServiceStatusIDs.ToString, IsForConfiguredList:=True)

                Dim FrequencyLabel, DoneOnLabel, CurrentLabel, ElapsedLabel, ExtensionLabel, DueOnLabel, AssemblyDueOnLabel, RemainingLabel As Label
                Dim Frequencylnkbtn, DoneOnlnkbtn, Currentlnkbtn, Elapsedlnkbtn, Extensionlnkbtn, DueOnlnkbtn, AssemblyDueOnlnkbtn, Remaininglnkbtn As LinkButton


                FrequencyLabel = CType(currentRow.FindControl("lblFreqValues"), Label)
                DoneOnLabel = CType(currentRow.FindControl("lblDoneOnValues"), Label)
                CurrentLabel = CType(currentRow.FindControl("lblCurrentValues"), Label)
                ElapsedLabel = CType(currentRow.FindControl("lblElapsedValues"), Label)
                ExtensionLabel = CType(currentRow.FindControl("lblExtensionValues"), Label)
                DueOnLabel = CType(currentRow.FindControl("lblDueAtValues"), Label)
                AssemblyDueOnLabel = CType(currentRow.FindControl("lblDueAtAirframeValues"), Label)
                RemainingLabel = CType(currentRow.FindControl("lblRemainingValues"), Label)

                Frequencylnkbtn = CType(currentRow.FindControl("lnkFreqValue"), LinkButton)
                DoneOnlnkbtn = CType(currentRow.FindControl("lnkDoneOnValue"), LinkButton)
                Currentlnkbtn = CType(currentRow.FindControl("lnkCurrentValue"), LinkButton)
                Elapsedlnkbtn = CType(currentRow.FindControl("lnkElapsedValue"), LinkButton)
                Extensionlnkbtn = CType(currentRow.FindControl("lnkExtensionValue"), LinkButton)
                DueOnlnkbtn = CType(currentRow.FindControl("lnkDueAtValue"), LinkButton)
                AssemblyDueOnlnkbtn = CType(currentRow.FindControl("lnkDueAtAirframeValue"), LinkButton)
                Remaininglnkbtn = CType(currentRow.FindControl("lnkRemainingValue"), LinkButton)

                Frequencylnkbtn.Visible = False
                DoneOnlnkbtn.Visible = False
                Currentlnkbtn.Visible = False
                Elapsedlnkbtn.Visible = False
                Extensionlnkbtn.Visible = False
                DueOnlnkbtn.Visible = False
                AssemblyDueOnlnkbtn.Visible = False
                Remaininglnkbtn.Visible = False

                If mtmpComplyAssemblyMonitorServiceStatusList.Count > 0 Then
                    FrequencyLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).FrequencyValueFormatted
                    DoneOnLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).DoneOnValueFormatted
                    CurrentLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).CurrentValueFormatted
                    ElapsedLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).ElapsedValueFormatted
                    ExtensionLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).ExtensionValueFormatted
                    DueOnLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).DueOnValueFormattedForGrid
                    AssemblyDueOnLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).AssemblyDueOnValueTextFormattedByAirFrame
                    RemainingLabel.Text = mtmpComplyAssemblyMonitorServiceStatusList(0).RemainingValueFormattedForGrid
                Else
                    FrequencyLabel.Text = ""
                    DoneOnLabel.Text = ""
                    CurrentLabel.Text = ""
                    ElapsedLabel.Text = ""
                    ExtensionLabel.Text = ""
                    DueOnLabel.Text = ""
                    AssemblyDueOnLabel.Text = ""
                    RemainingLabel.Text = ""
                End If
        End Select
    End Sub
    Private Sub dgConfigList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgConfigList.Sorting
        mAssemblyMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyMonitorServiceStatusListMPD") = mAssemblyMonitorServiceStatusList
        dgConfigList.DataSource = mAssemblyMonitorServiceStatusList
        dgConfigList.DataBind()
        SetGrid(True, False)
        SetPage(mAssemblyMonitorServiceStatusList.Count, mModelMonitorServiceList.Count, IIf(mModelMonitorServiceList.Count > 0, mModelMonitorServiceList.RecordCount, 0))
        ControlVisibility()
    End Sub
    Private Sub TbConfigNonConfig_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbConfigNonConfig.ActiveTabChanged
        AssConfigNonConfigTabIndex = TbConfigNonConfig.ActiveTabIndex
        Session("AssConfigNonConfigTabIndex") = AssConfigNonConfigTabIndex
        lblFreq.Visible = IIf(TbConfigNonConfig.ActiveTabIndex = 0, True, False)
        txtFrequency.Visible = IIf(TbConfigNonConfig.ActiveTabIndex = 0, True, False)
        upnlFindNow.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
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
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged, txtMPDNo.TextChanged, txtFrequency.TextChanged
        Description = txtDescription.Text.Trim
        Session("Description") = Description
        MPDNo = txtMPDNo.Text.Trim
        Session("MPDNo") = MPDNo
        Frequency = Trim(txtFrequency.Text)
        Session("Frequency") = Frequency

        getGridRecords()
        upnlTabs.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub



    Private Sub btnGroupConfigure_Click(sender As Object, e As System.EventArgs) Handles btnGroupConfigure.Click
        Dim AssemblyID As Guid
        Dim AssemblyStatusID As Guid
        Dim HourType As Integer
        Dim ModelID As Guid
        Dim chkBox As CheckBox
        Dim IDArray As New StringBuilder
        For i As Integer = 0 To dgNonConfigList.Rows.Count - 1
            chkBox = CType(dgNonConfigList.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)

            If chkBox.Checked Then
                'mModelMonitorServiceList(i).IsSelected = True
                IDArray.Append(dgNonConfigList.DataKeys(i).Values(0).ToString + ",")
            End If
        Next
        If IDArray.ToString.Trim = "" Then
            MSGBoxCtrl.Show("Alert!", "Please select At least One MPD to configure", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        AssemblyID = mAssemblyList(SelectedAssemblyIndex).ID
        AssemblyStatusID = mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID
        ModelID = mAssemblyList(SelectedAssemblyIndex).ModelID
        HourType = mAssemblyList(SelectedAssemblyIndex).HourType
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)
        'mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = New Guid(IDArray.ToString.TrimEnd(",").Split(",")(0))
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        'For Each mAssemblyStatusPeriod As AssemblyStatusPeriod In mAssemblyStatus.AssemblyStatusPeriods
        '    mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
        'Next
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("IsOpenFromMPD") = "True"
        Session("RegNo") = mAssemblyList(SelectedAssemblyIndex).RegNo
        Session.Remove("mAssemblyMonitorServiceStatusListMPD") 'MPD Slow Perf
        Session("IDsArrayStr") = IDArray.ToString.TrimEnd(",")
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        '''
        Dim tmpAssemblyStatusList As AssemblyStatusList
        Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
        tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(Today.Date.ToString, mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, , , , , , , ModelID.ToString, , , True, , , , , , , , , , , , , "", , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=True, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
        AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
        '''
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorServiceStatusGroupConfigureFromMPD_Ajax.aspx?GChildPage2=index.aspx');", True)
    End Sub

#End Region
End Class