Imports System.Text
Public Class wfConfigureCompMPDAMPList_Ajax
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
    Protected mPartMonitorServiceTypeList As PartMonitorServiceTypeList
    Protected mPartMonitorServiceList As PartMonitorServiceList
    Protected mCompMonitorServiceStatus As CompMonitorServiceStatus
    'Dim mCompMPDConfigurableList As CompMPDConfigurableList
    Dim mCompMonitorServiceStatusList As CompMonitorServiceStatusList
    Dim mFileAttach As FileAttach
    Dim mMachine As Machine
    Dim mUpdateComplyHistoryCompMonitorServiceStatusList As UpdateComplyHistoryCompMonitorServiceStatusList
    Dim mBoardInfo As AircraftInformationBoard.BoardInfo
    Dim mMachineMaintenance As MachineMaintenance
    Dim mServiceDetail As String
    Public mATAList As ATAList
    Dim SelectedAssemblyIndex, ActiveTabindex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA, SelectedCompIndex As Integer
    Dim Description As String = String.Empty
    Dim MPDNo As String = String.Empty
    Dim Frequency As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyList = Session("mAssemblyListForConfigCompMPD")
        mAssemblyTypeList = CType(Session("mAssemblyTypeListForConfigCompMPD"), AssemblyTypeList)
        mPartMonitorServiceList = CType(Session("mPartMonitorServiceList"), PartMonitorServiceList)
        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        'mCompMPDConfigurableList = Session("mCompMPDConfigurableList")
        mCompMonitorServiceStatusList = Session("mCompMonitorServiceStatusList")
        SelectedAssemblyIndex = IIf(Session("SelectedAssemblyIndexForConfigCompMPD") Is Nothing, 0, Session("SelectedAssemblyIndexForConfigCompMPD"))
        SelectedAssemblyTypeIndex = IIf(Session("SelectedAssemblyTypeIndexForConfigCompMPD") Is Nothing, 0, Session("SelectedAssemblyTypeIndexForConfigCompMPD"))
        ActiveTabindex = IIf(Session("ActiveTabindexForConfigCompMPD") Is Nothing, 0, Session("ActiveTabindexForConfigCompMPD"))
        SelectedMonitorType = IIf(Session("SelectedMonitorTypeForConfigCompMPD") Is Nothing, 0, Session("SelectedMonitorTypeForConfigCompMPD"))
        ATA = IIf(Session("ATAForConfigCompMPD") Is Nothing, 0, Session("ATAForConfigCompMPD"))
        Description = IIf(Session("DescriptionForConfigCompMPD") Is Nothing, String.Empty, Session("DescriptionForConfigCompMPD"))
        mATAList = CType(Session("mATAListForConfigCompMPD"), ATAList)
        mPartMonitorServiceTypeList = CType(Session("mPartMonitorServiceTypeListForConfigCompMPD"), PartMonitorServiceTypeList)
        SelectedCompIndex = IIf(Session("SelectedCompIndexForConfigCompMPD") Is Nothing, 0, Session("SelectedCompIndexForConfigCompMPD"))
        mCompListForComboBox = Session("mCompListForComboBox")
        MPDNo = IIf(Session("MPDNo") Is Nothing, String.Empty, Session("MPDNo"))
        Frequency = IIf(Session("Frequency") Is Nothing, String.Empty, Session("Frequency"))
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfConfigureCompMPDList_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub getGridRecords()
        If mAssemblyList.Count > 0 Then
            If mCompListForComboBox Is Nothing Then
                mCompListForComboBox = CompListForComboBox.GetCompList(Today.Date.ToString, mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString)
                Session("mCompListForComboBox") = mCompListForComboBox
                cmbComponent.DataSource = mCompListForComboBox
                cmbComponent.DataBind()
            End If


            cmbAssembly.Enabled = True
            If mCompListForComboBox.Count > 0 Then
                cmbComponent.Enabled = True
                mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ServiceType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=txtDescription.Text.Trim, IsFromMPD:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString, TaskCardNo:=txtMPDNo.Text.Trim, Frequency:=txtFrequency.Text.Trim)
                dgNonConfigList.DataSource = mPartMonitorServiceList
                dgNonConfigList.DataBind()
                Session("mPartMonitorServiceList") = mPartMonitorServiceList

                'mCompMPDConfigurableList = CompMPDConfigurableList.GetMPDConfigurationList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, PartMonitorServiceID:=Guid.Empty.ToString, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ServiceType:=SelectedMonitorType, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                'Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
                'dgConfigList.DataSource = mCompMPDConfigurableList
                mCompMonitorServiceStatusList = CompMonitorServiceStatusList.GetCompMonitorServiceStatusList(CurrentDate:=Today.Date.ToString, CompID:=mCompListForComboBox(SelectedCompIndex).CompID, SerialNo:=mCompListForComboBox(SelectedCompIndex).CompSerialNo, CompStatusPeriodList:=Nothing, IsRecordsDirectFetch:=True, Description:=Description, ATACode:=mATAList(ATA).ATACode, MonitorTypeID:=SelectedMonitorType, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID.ToString, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsComplied:=True, CodeFormNoDesc:=txtMPDNo.Text.Trim)
                Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
                dgConfigList.DataSource = mCompMonitorServiceStatusList
                dgConfigList.DataBind()

                SetGrid()
                SetPage(mCompMonitorServiceStatusList.Count, mPartMonitorServiceList.Count)
                ControlVisibility()
            Else
                cmbComponent.Enabled = False
                mPartMonitorServiceList = Nothing
                'mCompMPDConfigurableList = Nothing
                'Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
                mCompMonitorServiceStatusList = Nothing
                Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
                Session("mPartMonitorServiceList") = mPartMonitorServiceList

                dgNonConfigList.DataSource = mPartMonitorServiceList
                dgNonConfigList.DataBind()
                dgConfigList.DataSource = mCompMonitorServiceStatusList
                dgConfigList.DataBind()
                SetGrid()
                SetPage()
                lblConfigResult.Visible = False
                lblNonConfigResult.Visible = False
            End If
        Else
            cmbAssembly.Enabled = False
            cmbComponent.Enabled = False
            mPartMonitorServiceList = Nothing
            'mCompMPDConfigurableList = Nothing
            'Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
            mCompMonitorServiceStatusList = Nothing
            Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
            mCompListForComboBox = Nothing
            Session("mCompListForComboBox") = mCompListForComboBox
            Session("mPartMonitorServiceList") = mPartMonitorServiceList

            dgNonConfigList.DataSource = mPartMonitorServiceList
            dgNonConfigList.DataBind()
            dgConfigList.DataSource = mCompMonitorServiceStatusList
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
        Session.Remove("mPartMonitorServiceList")
        Session.Remove("mCompMonitorServiceStatus")
        'Session.Remove("mCompMPDConfigurableList")
        Session.Remove("mCompMonitorServiceStatusList")
        Session.Remove("mATAListForConfigCompMPD")
        Session.Remove("mPartMonitorServiceTypeListForConfigCompMPD")
        Session.Remove("mCompListForComboBox")
        Session.Remove("MPDNo")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartMonitorServiceList.CurrentIndex = Index
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
    End Sub
    Private Sub ControlVisibility()
        If Not mPartMonitorServiceList Is Nothing Then
            lblNonConfigResult.Visible = (mPartMonitorServiceList.Count > 0)

            If Not dgNonConfigList.HeaderRow Is Nothing Then
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    dgNonConfigList.HeaderRow.Cells(2).Text = "Task No."
                Else
                    dgNonConfigList.HeaderRow.Cells(2).Text = "Code/Form No."
                End If
            End If


        End If
        If Not mCompMonitorServiceStatusList Is Nothing Then
            lblConfigResult.Visible = (mCompMonitorServiceStatusList.Count > 0)
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
                        Dim PartMonitorServiceID As Guid
                        Try
                            Session("sender") = ""
                            Dim index As Integer = Session("Index")
                            IDForEventLog = mCompMonitorServiceStatusList(index).ID
                            PartMonitorServiceID = mCompMonitorServiceStatusList(index).PartMonitorServiceID
                            mServiceDetail = "Part : " + mCompListForComboBox(SelectedCompIndex).PartNoSerialNo + " Aircraft : " + mAssemblyList(SelectedAssemblyIndex).RegNo + " Monitor Type : " + mCompMonitorServiceStatusList(index).MonitorType + " Description : " + mCompMonitorServiceStatusList(index).Description
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(IDForEventLog)
                            '********************************
                            If mCompMonitorServiceStatusList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(IDForEventLog)
                            End If
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(IDForEventLog, 9)
                            '=============================
                            CompMonitorServiceStatus.DeleteCompMonitorServiceStatus(IDForEventLog)
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
                            mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ServiceType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=Description, IsFromMPD:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                            dgNonConfigList.DataSource = mPartMonitorServiceList
                            dgNonConfigList.DataBind()
                            Session("mPartMonitorServiceList") = mPartMonitorServiceList

                            'mCompMPDConfigurableList = CompMPDConfigurableList.GetMPDConfigurationList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, PartMonitorServiceID:=Guid.Empty.ToString, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ServiceType:=SelectedMonitorType, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                            'Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
                            mCompMonitorServiceStatusList = CompMonitorServiceStatusList.GetCompMonitorServiceStatusList(CurrentDate:=Today.Date.ToString, CompID:=mCompListForComboBox(SelectedCompIndex).CompID, SerialNo:=mCompListForComboBox(SelectedCompIndex).CompSerialNo, CompStatusPeriodList:=Nothing, IsRecordsDirectFetch:=True, Description:=Description, ATACode:=mATAList(ATA).ATACode, MonitorTypeID:=SelectedMonitorType, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID.ToString, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsComplied:=True)
                            Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
                            dgConfigList.DataSource = mCompMonitorServiceStatusList
                            dgConfigList.DataBind()

                            SetGrid()
                            SetPage(mCompMonitorServiceStatusList.Count, mPartMonitorServiceList.Count)
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
                                MarkLog(Util.Action.Delete, "ComponentServices", "Can't delete :" & mServiceDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentServices", mServiceDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    End If

                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim IDForEventLog As Guid
                        Try
                            Session("sender") = ""

                            IDForEventLog = mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).ID
                            If mPartMonitorServiceList(IDForEventLog).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorServiceList(IDForEventLog).ID)
                            End If
                            mServiceDetail = "Part : " + mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).PartName + " ATA : " + mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).ATAChapter + " Description : " + mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).Description
                            PartMonitorService.DeletePartMonitorService(mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            MarkLog(Util.Action.Delete, "Part Service", mServiceDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ServiceType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=Description, IsFromMPD:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
                            dgNonConfigList.DataSource = mPartMonitorServiceList
                            dgNonConfigList.DataBind()
                            SetGrid()
                            SetPage(mCompMonitorServiceStatusList.Count, mPartMonitorServiceList.Count)
                            ControlVisibility()
                            upnlTabs.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "") 'Added by Vikrant on 28-July-2011
                                MarkLog(Util.Action.Delete, "Part Service", "Can't Delete:" & mServiceDetail & " is already in use", Util.ErrorType.NoError, IDForEventLog, EventLogID)
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
        Dim P, C As Boolean
        'If IsConfigGrid Then
        '    For j As Integer = 0 To dgConfigList.Rows.Count - 1
        '        C = CType(Me.dgConfigList.Rows(j).Cells(24).Text, Boolean) 'IsMaster
        '        P = CType(Me.dgConfigList.Rows(j).Cells(26).Text, Boolean) 'IsAttachmentAdded


        '        If C = True Then
        '            dgConfigList.Rows(j).Cells(23).Enabled = False 'History
        '        End If
        '        If P = False Then
        '            dgConfigList.Rows(j).Cells(25).Enabled = False 'View
        '        End If

        '        dgConfigList.Rows(j).Cells(21).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, False, True) 'Delete
        '        dgConfigList.Rows(j).Cells(22).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, False, True) 'Edit

        '    Next
        'End If
        If IsNonConfigGrid Then
            For j As Integer = 0 To dgNonConfigList.Rows.Count - 1
                'P = CType(Me.dgNonConfigList.Rows(j).Cells(14).Text, Boolean)
                'If P = False Then
                '    dgNonConfigList.Rows(j).Cells(13).Enabled = False
                'End If

                dgNonConfigList.Rows(j).Cells(10).Enabled = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = False, True, False)
            Next
        End If
        lblReadOnly.Visible = IIf(mAssemblyList(SelectedAssemblyIndex).IsMachineReadOnly = True, True, False)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mPartMonitorService As PartMonitorService
        mPartMonitorService = PartMonitorService.GetPartMonitorService(mId, 1) 'HourType=1 as diff is only show purpose H OR HD
        Session("mPartMonitorService") = mPartMonitorService
        mServiceDetail = "Part : " & mCompListForComboBox(SelectedCompIndex).PartName & " Part Service Type : " & mPartMonitorService.PartMonitorServiceTypeName & " Description : " & mPartMonitorService.Description
        MarkLog(Util.Action.Edit, "Part Service", mServiceDetail, Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
        Session("PartIDForNewCompMPD") = mCompListForComboBox(SelectedCompIndex).PartID
        Session("IsFromMPDConfig") = True
        Session.Remove("mCompMonitorServiceStatusList") 'MPD Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompMPDService_Ajax.aspx?BackPage=wfConfigureMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub EditConfiguredRecord(ByVal CompMonitorServiceStatusID As Guid, ByVal AssemblyStausID As Guid, ByVal CompStatusID As Guid, ByVal HourType As Integer)
        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(CompMonitorServiceStatusID, AssemblyStausID, CompStatusID, HourType)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("Edit") = True
        Session.Remove("mCompMonitorServiceStatusList") 'MPD Slow Perf
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCompMonitorServiceStatus_Ajax.aspx?GChildPage4=index.aspx');", True)
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal CompMonitorServiceStatusID As Guid, ByVal AssemblyStatusID As Guid, ByVal CompStatusID As Guid)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(CompMonitorServiceStatusID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        Session("EnFrom") = 1 'EditRecord
        ''
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, Today.Date.ToString)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus

        mUpdateComplyHistoryCompMonitorServiceStatusList = UpdateComplyHistoryCompMonitorServiceStatusList.GetComplyHistoryCompMonitorServiceStatusList(mCompStatus.CompID, mCompMonitorServiceStatus.PartMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorServiceStatusList") = mUpdateComplyHistoryCompMonitorServiceStatusList

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow();", True)
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

        mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList("(All)")
        cmbMonitorType.DataSource = mPartMonitorServiceTypeList
        cmbMonitorType.DataBind()
        Session("mPartMonitorServiceTypeListForConfigCompMPD") = mPartMonitorServiceTypeList

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
            ' Session("MiddleFrame") = "wfConfigureCompMPDList_Ajax.aspx"
            DataFieldBind()
            SetGrid()
            ControlVisibility()
            TbConfigNonConfig.ActiveTabIndex = IIf(CType(Session("ActiveTabindexForConfigCompMPD"), Integer) > 0, CType(Session("ActiveTabindexForConfigCompMPD"), Integer), 0)
        End If
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
            If Not dgNonConfigList.HeaderRow Is Nothing Then dgNonConfigList.HeaderRow.Cells(3).Text = "Task No."

            dgNonConfigList.ToolTip = "MPD List"
            lblMonitorType.InnerText = "Maintenance Event"
            lblCodeTaskNo.InnerText = "Task No."
            dgConfigList.Columns(5).Visible = True 'Task No
            If Not dgConfigList.HeaderRow Is Nothing Then dgConfigList.HeaderRow.Cells(9).Text = "Description"
            If Not dgConfigList.HeaderRow Is Nothing Then dgConfigList.HeaderRow.Cells(7).Text = "Task Type"
        Else
            ServiceMPDTitle = "Services"
            If Not dgNonConfigList.HeaderRow Is Nothing Then dgNonConfigList.HeaderRow.Cells(3).Text = "Code/Form No."

            dgNonConfigList.ToolTip = "Model Service List"
            lblMonitorType.InnerText = "Service Type"
            lblCodeTaskNo.InnerText = "Code/Form No."

            dgConfigList.Columns(5).Visible = False ''Task No
            If Not dgConfigList.HeaderRow Is Nothing Then dgConfigList.HeaderRow.Cells(9).Text = "Code/Form No./Description"
            If Not dgConfigList.HeaderRow Is Nothing Then dgConfigList.HeaderRow.Cells(7).Text = "Service Type"
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
                mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompListForComboBox(SelectedCompIndex).CompID, AssemblyStatusID, Today.Date.ToString, mCompListForComboBox(SelectedCompIndex).PartID, ModelID, mCompListForComboBox(SelectedCompIndex).CompStatusID, HourType)
                mCompMonitorServiceStatus.PartMonitorServiceID(False) = New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mCompListForComboBox(SelectedCompIndex).CompStatusID, AssemblyStatusID, Today.Date.ToString)
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyList(SelectedAssemblyIndex).MachineID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                Session("IsOpenFromMPD") = "True"
                Session("mCompStatus") = mCompStatus
                Session("mMachine") = mMachine
                Session.Remove("mCompMonitorServiceStatusList") 'MPD Slow Perf
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCompMonitorServiceStatus_Ajax.aspx?GChildPage4=index.aspx');", True)
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
        Session.Remove("SelectedCompIndexForConfigCompMPD")
        Session("MiddleFrame") = ""
        'Response.Redirect("index.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgNonConfigList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgNonConfigList.Sorting
        mPartMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
        dgNonConfigList.DataSource = mPartMonitorServiceList
        dgNonConfigList.DataBind()
        SetGrid(False, True)
        SetPage(mCompMonitorServiceStatusList.Count, mPartMonitorServiceList.Count)
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
        mCompListForComboBox = CompListForComboBox.GetCompList(Today.Date.ToString, mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString)
        Session("mCompListForComboBox") = mCompListForComboBox
        cmbComponent.DataSource = mCompListForComboBox
        cmbComponent.DataBind()
        getGridRecords()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub hdnBtnServiceHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnServiceHistory.Click
        mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ServiceType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=Description, IsFromMPD:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
        dgNonConfigList.DataSource = mPartMonitorServiceList
        dgNonConfigList.DataBind()
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
        SetGrid(False, True)
        SetPage(mCompMonitorServiceStatusList.Count, mPartMonitorServiceList.Count)
        ControlVisibility()
        upnlTabs.Update()
    End Sub
    Private Sub dgConfigList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgConfigList.RowCommand
        Dim CompStatusID, CompMonitorServiceStatusID As Guid
        'Dim HourType As Integer
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                CompStatusID = mCompListForComboBox(SelectedCompIndex).CompStatusID
                'HourType = CInt(dgConfigList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID, Today.Date.ToString)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID)
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyList(SelectedAssemblyIndex).MachineID)
                Session("mCompStatus") = mCompStatus
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = mMachine.RegNo 'dgConfigList.Rows(CInt(e.CommandArgument)).Cells(4).Text.ToString
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mMachine") = mMachine
                EditConfiguredRecord(mCompMonitorServiceStatusList(CInt(e.CommandArgument)).ID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID, mCompListForComboBox(SelectedCompIndex).CompStatusID, mMachine.HourType)
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
                HistoryRecords(mAssemblyList(SelectedAssemblyIndex).MachineID, mCompMonitorServiceStatusList(CInt(e.CommandArgument)).ID, mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID, mCompListForComboBox(SelectedCompIndex).CompStatusID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatusList(CInt(e.CommandArgument)).ID)
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
            Case "ShowVal"
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
                ''CompMonitorServiceStatusID = New Guid(currentRow.Cells(0).Text)


                ''Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
                Dim CompMonitorServiceStatusIDs As New StringBuilder
                CompMonitorServiceStatusIDs.Append("<CompMonServiceID>")
                CompMonitorServiceStatusIDs.Append("<id>")
                CompMonitorServiceStatusIDs.Append(New Guid(currentRow.Cells(0).Text))
                CompMonitorServiceStatusIDs.Append("</id>")
                CompMonitorServiceStatusIDs.Append("</CompMonServiceID>")


                Dim mtmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
                mtmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList([Date]:=Today.Date.ToString, AssemblyID:=mAssemblyList(cmbAssembly.SelectedIndex).ID, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, Part:=mCompListForComboBox(cmbComponent.SelectedIndex).PartName, SerialNo:=mCompListForComboBox(cmbComponent.SelectedIndex).CompSerialNo, CompMonitorServiceStatusIDs:=CompMonitorServiceStatusIDs.ToString, IsFromMPD:=True)

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

                If mtmpComplyCompMonitorServiceStatusList.Count > 0 Then
                    FrequencyLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).FrequencyValueFormatted
                    DoneOnLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).DoneOnValueFormatted
                    CurrentLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).CurrentValueFormatted
                    ElapsedLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).ElapsedValueFormatted
                    ExtensionLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).ExtensionValueFormatted
                    DueOnLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).DueOnValueFormattedForGrid
                    AssemblyDueOnLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).AssemblyDueOnValueTextFormattedByAirFrame
                    RemainingLabel.Text = mtmpComplyCompMonitorServiceStatusList(0).RemainingValueFormattedForGrid
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
        mCompMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        dgConfigList.DataSource = mCompMonitorServiceStatusList
        dgConfigList.DataBind()
        SetGrid(True, False)
        SetPage(mCompMonitorServiceStatusList.Count, mPartMonitorServiceList.Count)
        ControlVisibility()
    End Sub
    Private Sub TbConfigNonConfig_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbConfigNonConfig.ActiveTabChanged
        ActiveTabindex = TbConfigNonConfig.ActiveTabIndex
        Session("ActiveTabindexForConfigCompMPD") = ActiveTabindex
        lblFreq.Visible = IIf(TbConfigNonConfig.ActiveTabIndex = 0, True, False)
        txtFrequency.Visible = IIf(TbConfigNonConfig.ActiveTabIndex = 0, True, False)
        upnlFindNow.Update()
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
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged, txtMPDNo.TextChanged, txtFrequency.TextChanged
        Description = txtDescription.Text.Trim
        Session("DescriptionForConfigCompMPD") = Description

        MPDNo = txtMPDNo.Text.Trim
        Session("MPDNo") = MPDNo
        Frequency = Trim(txtFrequency.Text)
        Session("Frequency") = Frequency


        getGridRecords()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunctionForIntTab", "CallParentFunctionForIntTab();", True)
    End Sub
    Private Sub cmbComponent_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbComponent.SelectedIndexChanged
        SelectedCompIndex = cmbComponent.SelectedIndex
        Session("SelectedCompIndexForConfigCompMPD") = SelectedCompIndex
        mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, ModelID:=mAssemblyList(SelectedAssemblyIndex).ModelID, ServiceType:=SelectedMonitorType, ATACode:=mATAList(ATA).ATACode, Description:=txtDescription.Text.Trim, IsFromMPD:=True, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
        dgNonConfigList.DataSource = mPartMonitorServiceList
        dgNonConfigList.DataBind()
        Session("mPartMonitorServiceList") = mPartMonitorServiceList

        'mCompMPDConfigurableList = CompMPDConfigurableList.GetMPDConfigurationList(PartID:=mCompListForComboBox(SelectedCompIndex).PartID, PartMonitorServiceID:=Guid.Empty.ToString, SkipNonConfiguredRecords:=True, AssemblyStatusID:=mAssemblyList(SelectedAssemblyIndex).AssemblyStatusID.ToString, ATACode:=mATAList(ATA).ATACode, MonitorDesc:=Description, ServiceType:=SelectedMonitorType, CompStatusID:=mCompListForComboBox(SelectedCompIndex).CompStatusID.ToString)
        'Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
        mCompMonitorServiceStatusList = CompMonitorServiceStatusList.GetCompMonitorServiceStatusList(CurrentDate:=Today.Date.ToString, CompID:=mCompListForComboBox(SelectedCompIndex).CompID, SerialNo:=mCompListForComboBox(SelectedCompIndex).CompSerialNo, CompStatusPeriodList:=Nothing, IsRecordsDirectFetch:=True, Description:=Description, ATACode:=mATAList(ATA).ATACode, MonitorTypeID:=SelectedMonitorType, AssemblyID:=mAssemblyList(SelectedAssemblyIndex).ID.ToString, MachineID:=mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, IsComplied:=True)
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        dgConfigList.DataSource = mCompMonitorServiceStatusList
        dgConfigList.DataBind()

        SetGrid()
        SetPage(mCompMonitorServiceStatusList.Count, mPartMonitorServiceList.Count)
        ControlVisibility()
        upnlTabs.Update()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
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
        '''mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)
        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompListForComboBox(SelectedCompIndex).CompID, AssemblyStatusID, Today.Date.ToString, mCompListForComboBox(SelectedCompIndex).PartID, ModelID, mCompListForComboBox(SelectedCompIndex).CompStatusID, HourType)
        'mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = New Guid(IDArray.ToString.TrimEnd(",").Split(",")(0))
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        'For Each mAssemblyStatusPeriod As AssemblyStatusPeriod In mAssemblyStatus.AssemblyStatusPeriods
        '    mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = New Guid(dgNonConfigList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
        'Next
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(mCompListForComboBox(SelectedCompIndex).CompStatusID, AssemblyStatusID, Today.Date.ToString)

        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("IsOpenFromMPD") = "True"
        Session("RegNo") = mAssemblyList(SelectedAssemblyIndex).RegNo
        Session.Remove("mAssemblyMonitorServiceStatusListMPD") 'MPD Slow Perf
        Session("IDsArrayStr") = IDArray.ToString.TrimEnd(",")
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
        Session("mCompStatus") = mCompStatus
        Session("AssemblyStatusID") = AssemblyStatusID
        '''
        'Dim tmpCompStatusList As CompStatusList
        'Dim CompStatusPeriodList As CompStatusPeriodList
        'tmpCompStatusList = CType(MachineList.GetMachineListWithInstallation(Today.Date.ToString, mAssemblyList(SelectedAssemblyIndex).MachineID.ToString, , , , , , , ModelID.ToString, , , True, , , , , , , , , , , , , "", , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=True, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList(0).CompStatusList
        'CompStatusPeriodList = tmpCompStatusList(tmpCompStatusList.FirstItem.ID).CompStatusPeriodList
        Session("CompStatusPeriodList") = mCompStatus.CompStatusPeriods
        '''
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCompMonitorServiceStatusGroupConfigureFromMPD_Ajax.aspx?GChildPage2=index.aspx');", True)
    End Sub
#End Region

End Class