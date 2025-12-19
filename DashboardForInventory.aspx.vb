Imports System.Linq
Imports Flypal.CompHistoryList
Partial Class DashboardForInventory
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Datagrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Dim mItemReceiptIssueTransactions As ItemReceiptIssueTransactions
    Dim mItemStockList As ItemStockList
    Dim ItemDescription As String = ""
    Dim PartDescription As String = ""
    Dim ItemName As String = ""
    Dim PartName As String = ""
    Dim SerialNo As String = ""
    Dim ReferenceNo As String = ""
    Dim mTaskCardList As TaskCardList
    Public mATA As ATA
    Public mItem As Item
    Public ATACode As String = ""
    Public mCompHistoryList As CompHistoryList
    Public mModelMonitorAMPRefStatusList As ModelMonitorAMPRefStatusList
    Public chkPartNo As Boolean = False
    Public chkSerialNo As Boolean = False
    Public chkReferenceNo As Boolean = False
    Public chkCodeNo As Boolean = False

#End Region

#Region " Methods "
    Private Sub GetSession()
        PartName = Session("PartName")
        PartDescription = Session("PartDescription")
        SerialNo = Session("SerialNo")
        ReferenceNo = Session("ReferenceNo")
        mItemStockList = Session("mItemStockList")
        mCompHistoryList = Session("mCompHistoryList")
        mModelMonitorAMPRefStatusList = Session("mModelMonitorAMPRefStatusList")

        chkPartNo = CType(Session("chkPartNo"), Boolean)
        chkSerialNo = CType(Session("chkSerialNo"), Boolean)
        chkReferenceNo = CType(Session("chkReferenceNo"), Boolean)
        chkCodeNo = CType(Session("chkCodeNo"), Boolean)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartName")
        Session.Remove("PartDescription")
        Session.Remove("SerialNo")
        Session.Remove("SearchText")
        Session.Remove("FromGrid")
        Session.Remove("ReferenceNo")
        Session.Remove("mCompHistoryList")
        Session.Remove("mModelMonitorAMPRefStatusList")

        Session.Remove("chkPartNo")
        Session.Remove("chkSerialNo")
        Session.Remove("chkReferenceNo")
        Session.Remove("chkCodeNo")

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "DashboardForInventory.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub RemoveRecord(ByVal mCompStatusInfo As CompHistoryListInfo)
        Dim checkRemovedAssemblyList As tmpRemovedAssemblyList = tmpRemovedAssemblyList.GetRemovedAssemblyList(Today.ToString, mCompStatusInfo.MachineID.ToString, Trim(mCompStatusInfo.PartNo), Trim(mCompStatusInfo.SerialNo))
        Session("checkRemovedAssemblyList") = checkRemovedAssemblyList
        If checkRemovedAssemblyList.Contains(mCompStatusInfo.ID) Then
            MSGBoxCtrl.Show(MSGBox.Message_title.ComponentIsRemoved, MSGBox.Message_text.ComponentIsRemoved, "Selected " & mCompStatusInfo.PartDet & ", Already removed, cannot remove again", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.NewRemovalCompStatus(mCompStatusInfo.ID, Today.Date.ToString, mCompStatusInfo.AssemblyStatusID, Guid.Empty.ToString)
        Session("From") = 1 'NewRemove
        Session("mCompStatus") = mCompStatus
        Dim mPrevCompStatus As CompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mCompStatusInfo.Date.ToString)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mPrevCompStatus") = mPrevCompStatus

        Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=2) 'Sort = 2 : Removal
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRemoveComp_Ajax.aspx?BackPage=Index.aspx');", True)

        Dim MaintDetail As String
        MaintDetail = "INST. ON A/C REGN/ S/N. &amp; Assembly Info: " + mCompStatusInfo.RegNoModelSerialNo & " Part Info : " & mCompStatusInfo.PartDet.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.Remove, "ComponentRemoval", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)

    End Sub

    Private Sub InstallRecord(ByVal mCompStatusInfo As CompHistoryListInfo)
        'Added By Utkarsh ON 04-Apr-2013 FOR ALL04042013
        Dim mRemovedCompStatus As CompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, Today.Date.ToString)
        'End

        'If cmbInstalledOnAssembly.SelectedIndex <> 0 AndAlso cmbInstalledOnAssemblyList.SelectedIndex <> 0 AndAlso CheckPeriodsForRemovedCompStatus(mRemovedCompStatus) = False Then
        '    MSGBoxCtrl.Show("Component Status Installation Alert!", "Periods for " & mRemovedCompStatus.PartNameSerialNo & " are mismatching with selected " & cmbInstalledOnAssemblyList.SelectedItem.Text & " Assembly on " & cmbInstalledOnAssembly.SelectedItem.Text & " .Can not be installed.", "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'Else

        Dim mCompStatus As CompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, Guid.Empty, mCompStatusInfo.AssemblyStatusID, Today.Date.ToString, True, mCompStatusInfo.ID.ToString, Guid.Empty.ToString)
        Dim mAssemblyStatus As AssemblyStatus
        Dim mMachine As Machine

        Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1) 'Sort = 1 : Installation
        Session("mFileAttach") = mFileAttach
        '---28-Apr-2009
        Session("IsAdded") = "False"
        Session("InstallOnId") = Guid.Empty.ToString
        Session("mInstallOnAssemblyID") = Guid.Empty.ToString
        '---28-Apr-2009

        Session("From") = 1 'NewInstall
        Session("InstallSelected") = 1
        Session("mCompStatus") = mCompStatus
        Session("mRemovedCompStatus") = mRemovedCompStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine

        Dim MaintDetail As String = ""
        MaintDetail = "INST. ON A/C REGN/ S/N. &amp; Assembly Info: " + mCompStatusInfo.RegNoModelSerialNo & " Part Info : " & mCompStatusInfo.PartDet.Replace(Environment.NewLine, " ")
        'MaintDetail = "Reg No. : " + mCompStatusList(mRemovedCompStatus.ID).MachineInfo & " Assembly Info : " & mCompStatusList(mRemovedCompStatus.ID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusList(mRemovedCompStatus.ID).CompInfo.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.Install, "Component Installation", MaintDetail, Util.ErrorType.NoError, mRemovedCompStatus.ID, EventLogID)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfInstallComp_AJAX.aspx?GChildPage2=Index.aspx');", True)
        '    End If
    End Sub

    Private Sub ComplyRecord(ByVal ID As Guid, ActivityTypeID As Integer)
        Dim mMachine As Machine
        Dim mAircraft As String
        Dim mMonitorInfo As String
        Dim mMonitorType As String
        Dim mMonitorDesc As String
        Dim mTaskNo As String = ""
        Dim mDirectiveNo As String = ""
        Dim mAssemblyMonitorDetail As String
        Dim mBoardInfo As AircraftInformationBoard.BoardInfo

        mModelMonitorAMPRefStatusList = Session("mModelMonitorAMPRefStatusList")
        Select Case ActivityTypeID

            'Assembly Service'
            Case 1 'Assembly Service'
                Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
                Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(ID, mModelMonitorAMPRefStatusList(ID).AssemblyStatusID, mMachine.HourType)

                If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    Dim mAssemblyStatus As AssemblyStatus
                    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid,
                                                                                                                       mPrevAssemblyMonitorServiceStatus.AssemblyID,
                                                                                                                       mPrevAssemblyMonitorServiceStatus.AssemblyStatusID,
                                                                                                                       Today.Date.ToString,
                                                                                                                       mModelMonitorAMPRefStatusList(ID).ModelID,
                                                                                                                       mPrevAssemblyMonitorServiceStatus.ModelMonitorService,
                                                                                                                       Guid.Empty,
                                                                                                                       mPrevAssemblyMonitorServiceStatus.DoneOn.ToString,
                                                                                                                       mMachine.HourType)

                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                    Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList(ID).RegNo + "->" + mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo + "->" + mModelMonitorAMPRefStatusList(ID).Reference + "->" + mModelMonitorAMPRefStatusList(ID).TypeName + "->" + mModelMonitorAMPRefStatusList(ID).Description
                    mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                    mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                    mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                    mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description
                    mTaskNo = mModelMonitorAMPRefStatusList(ID).TaskNo

                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & Today.Date.ToString & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).CurrentValue


                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                    Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
                    Session("From") = 0 'New record
                    mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                    Session("mMachine") = mMachine
                    Session("mAssemblyStatus") = mAssemblyStatus


                    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
                    Session("mBoardInfo") = mBoardInfo

                    Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID) 'Sort = 1 : Installation
                    Session("mFileAttach") = mFileAttach


                    MarkLog(Util.Action.Comply, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx'); ", True)
                End If
            Case 2
                'Assembly Inspection'
                Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
                Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(ID, mModelMonitorAMPRefStatusList(ID).AssemblyStatusID, mMachine.HourType)

                If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    Dim mAssemblyStatus As AssemblyStatus
                    mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid,
                                                                                                                       mPrevAssemblyMonitorInspStatus.AssemblyID,
                                                                                                                       mPrevAssemblyMonitorInspStatus.AssemblyStatusID,
                                                                                                                       Today.Date.ToString,
                                                                                                                       mModelMonitorAMPRefStatusList(ID).ModelID,
                                                                                                                       mPrevAssemblyMonitorInspStatus.ModelMonitorInsp,
                                                                                                                       Guid.Empty,
                                                                                                                       mPrevAssemblyMonitorInspStatus.DoneOn.ToString,
                                                                                                                       mMachine.HourType)

                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                    Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList(ID).RegNo + "->" + mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo + "->" + mModelMonitorAMPRefStatusList(ID).Reference + "->" + mModelMonitorAMPRefStatusList(ID).TypeName + "->" + mModelMonitorAMPRefStatusList(ID).Description
                    mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                    mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                    mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                    mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description
                    mTaskNo = mModelMonitorAMPRefStatusList(ID).TaskNo

                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & Today.Date.ToString & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).CurrentValue


                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                    Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
                    Session("From") = 0 'New record
                    mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                    Session("mMachine") = mMachine
                    Session("mAssemblyStatus") = mAssemblyStatus


                    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
                    Session("mBoardInfo") = mBoardInfo

                    Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorInspStatus.ID) 'Sort = 1 : Installation
                    Session("mFileAttach") = mFileAttach


                    MarkLog(Util.Action.Comply, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=Index.aspx'); ", True)
                End If


            'Assembly Directives
            Case 3
                Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
                Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(ID, mModelMonitorAMPRefStatusList(ID).AssemblyStatusID, mMachine.HourType)

                If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    Dim mAssemblyStatus As AssemblyStatus
                    mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid,
                                                                                                                       mPrevAssemblyMonitorModStatus.AssemblyID,
                                                                                                                       mPrevAssemblyMonitorModStatus.AssemblyStatusID,
                                                                                                                       Today.Date.ToString,
                                                                                                                       mModelMonitorAMPRefStatusList(ID).ModelID,
                                                                                                                       mPrevAssemblyMonitorModStatus.ModelMonitorMod,
                                                                                                                       Guid.Empty,
                                                                                                                       mPrevAssemblyMonitorModStatus.DoneOn.ToString,
                                                                                                                       mMachine.HourType)

                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                    Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList(ID).RegNo + "->" + mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo + "->" + mModelMonitorAMPRefStatusList(ID).Reference + "->" + mModelMonitorAMPRefStatusList(ID).TypeName + "->" + mModelMonitorAMPRefStatusList(ID).Description
                    mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                    mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                    mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                    mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description
                    mDirectiveNo = mModelMonitorAMPRefStatusList(ID).TaskNo

                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Directive No. : " & mDirectiveNo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & Today.Date.ToString & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).CurrentValue


                    Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                    Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
                    Session("From") = 0 'New record
                    mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
                    Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                    Session("mMachine") = mMachine
                    Session("mAssemblyStatus") = mAssemblyStatus


                    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
                    Session("mBoardInfo") = mBoardInfo

                    Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorModStatus.ID) 'Sort = 1 : Installation
                    Session("mFileAttach") = mFileAttach




                    MarkLog(Util.Action.Comply, "AssemblyModifications", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=Index.aspx'); ", True)
                End If

           'Comp Service'
            Case 4 'Comp Service'
                Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
                Dim mHourType As Integer = 1
                Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus
                Dim mCompStatus As CompStatus
                Dim MaintDetail As String = ""

                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mHourType = mMachine.HourType

                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mModelMonitorAMPRefStatusList(ID).CompStatusID,
                                                           mModelMonitorAMPRefStatusList(ID).AssemblyStatusID,
                                                           mModelMonitorAMPRefStatusList(ID).DoneOn.ToString)

                mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ID,
                                                                                                     mModelMonitorAMPRefStatusList(ID).AssemblyStatusID,
                                                                                                     mModelMonitorAMPRefStatusList(ID).CompStatusID,
                                                                                                     mHourType, ,
                                                                                                     mCompStatus,
                                                                                                     mCompStatus.IsSpareComp)



                If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid,
                                                                                                           mPrevCompMonitorServiceStatus.CompID,
                                                                                                           mPrevCompMonitorServiceStatus.AssemblyStatusID,
                                                                                                           Today.Date.ToString,
                                                                                                           mPrevCompMonitorServiceStatus.PartMonitorService.PartID,
                                                                                                           mPrevCompMonitorServiceStatus.PartMonitorService,
                                                                                                           Guid.Empty,
                                                                                                           mPrevCompMonitorServiceStatus.CompStatusID,
                                                                                                           mPrevCompMonitorServiceStatus.DoneOn.ToString,
                                                                                                           mPrevCompMonitorServiceStatus.ID.ToString)
                    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                    Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
                    Session("EnFrom") = 0 'NewRecord
                End If

                Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList(ID).RegNo + "->" + mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo + "->" + mModelMonitorAMPRefStatusList(ID).Reference + "->" + mModelMonitorAMPRefStatusList(ID).TypeName + "->" + mModelMonitorAMPRefStatusList(ID).Description
                mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description
                mTaskNo = mModelMonitorAMPRefStatusList(ID).TaskNo

                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & Today.Date.ToString & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).CurrentValue

                MarkLog(Util.Action.Comply, "ComponentService", mAssemblyMonitorDetail, Util.ErrorType.NoError, ID, EventLogID)

                Session("mMachine") = mMachine
                Session("mCompStatus") = mCompStatus
                mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

                Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach


                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)

            'Component Inspection
            Case 5
                Dim mCompMonitorInspStatus As CompMonitorInspStatus
                Dim mHourType As Integer = 1
                Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus
                Dim mCompStatus As CompStatus
                Dim MaintDetail As String = ""

                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mHourType = mMachine.HourType

                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mModelMonitorAMPRefStatusList(ID).CompStatusID,
                                            mModelMonitorAMPRefStatusList(ID).AssemblyStatusID,
                                            mModelMonitorAMPRefStatusList(ID).DoneOn.ToString)

                mPrevCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(ID,
                                                                                      mModelMonitorAMPRefStatusList(ID).AssemblyStatusID,
                                                                                      mModelMonitorAMPRefStatusList(ID).CompStatusID,
                                                                                      mHourType, ,
                                                                                      mCompStatus,
                                                                                      mCompStatus.IsSpareComp)



                If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted = True Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted = True Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid,
                                                                                                  mPrevCompMonitorInspStatus.CompID,
                                                                                                  mPrevCompMonitorInspStatus.AssemblyStatusID,
                                                                                                  Today.Date.ToString,
                                                                                                  mPrevCompMonitorInspStatus.PartMonitorInsp.PartID,
                                                                                                  mPrevCompMonitorInspStatus.PartMonitorInsp,
                                                                                                  Guid.Empty,
                                                                                                  mPrevCompMonitorInspStatus.CompStatusID,
                                                                                                  mPrevCompMonitorInspStatus.DoneOn.ToString,
                                                                                                  mMachine.HourType)

                    Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                    Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
                    Session("EnFrom") = 0 'NewRecord
                End If

                Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList(ID).RegNo + "->" + mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo + "->" + mModelMonitorAMPRefStatusList(ID).Reference + "->" + mModelMonitorAMPRefStatusList(ID).TypeName + "->" + mModelMonitorAMPRefStatusList(ID).Description
                mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description
                mTaskNo = mModelMonitorAMPRefStatusList(ID).TaskNo

                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & Today.Date.ToString & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).CurrentValue

                MarkLog(Util.Action.Comply, "ComponentInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, ID, EventLogID)

                Session("mMachine") = mMachine
                Session("mCompStatus") = mCompStatus
                mCompMonitorInspStatus.RequiredManHours = mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus

                Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorInspStatus.ID) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach



                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorInspStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)

                'Component Modifications
            Case 6
                Dim mCompMonitorModStatus As CompMonitorModStatus
                Dim mHourType As Integer = 1
                Dim mPrevCompMonitorModStatus As CompMonitorModStatus
                Dim mCompStatus As CompStatus
                Dim MaintDetail As String = ""

                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mHourType = mMachine.HourType

                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mModelMonitorAMPRefStatusList(ID).CompStatusID,
                                            mModelMonitorAMPRefStatusList(ID).AssemblyStatusID,
                                            mModelMonitorAMPRefStatusList(ID).DoneOn.ToString)

                mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(ID,
                                                                                      mModelMonitorAMPRefStatusList(ID).AssemblyStatusID,
                                                                                      mModelMonitorAMPRefStatusList(ID).CompStatusID,
                                                                                      mHourType, ,
                                                                                      mCompStatus,
                                                                                      mCompStatus.IsSpareComp)



                If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted = True Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted = True Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid,
                                                                                               mPrevCompMonitorModStatus.CompID,
                                                                                               mPrevCompMonitorModStatus.AssemblyStatusID,
                                                                                               Today.Date.ToString,
                                                                                               mPrevCompMonitorModStatus.PartMonitorMod.PartID,
                                                                                               mPrevCompMonitorModStatus.PartMonitorMod,
                                                                                               Guid.Empty,
                                                                                               mPrevCompMonitorModStatus.CompStatusID,
                                                                                               mPrevCompMonitorModStatus.DoneOn.ToString,
                                                                                               mMachine.HourType)
                    Session("mCompMonitorModStatus") = mCompMonitorModStatus
                    Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
                    Session("EnFrom") = 0 'NewRecord
                End If

                Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList(ID).RegNo + "->" + mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo + "->" + mModelMonitorAMPRefStatusList(ID).Reference + "->" + mModelMonitorAMPRefStatusList(ID).TypeName + "->" + mModelMonitorAMPRefStatusList(ID).Description
                mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description
                mTaskNo = mModelMonitorAMPRefStatusList(ID).TaskNo

                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & Today.Date.ToString & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).CurrentValue

                MarkLog(Util.Action.Comply, "ComponentModifications", mAssemblyMonitorDetail, Util.ErrorType.NoError, ID, EventLogID)

                Session("mMachine") = mMachine
                Session("mCompStatus") = mCompStatus
                mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
                Session("mCompMonitorModStatus") = mCompMonitorModStatus

                Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach



                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorModStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)

        End Select
    End Sub

    Private Sub HistoryRecords(ByVal ID As Guid, ActivityTypeID As Integer)  'Added by Saylee on 09-Sep-2009

        Dim mAssemblyStatus As AssemblyStatus

        Dim mMachine As Machine
        Dim mAircraft As String
        Dim mMonitorInfo As String
        Dim mMonitorType As String
        Dim mMonitorDesc As String
        Dim mTaskNo As String = ""
        Dim mDirectiveNo As String = ""
        Dim mAssemblyMonitorDetail As String
        Dim mBoardInfo As AircraftInformationBoard.BoardInfo
        Dim mCompInfo As String
        Dim MaintDetail As String
        mModelMonitorAMPRefStatusList = Session("mModelMonitorAMPRefStatusList")

        Select Case ActivityTypeID
              'Assembly Service'
            Case 1 'Assembly Service'
                Dim mUpdateComplyHistoryAssemblyMonitorServiceStatusList As UpdateComplyHistoryAssemblyMonitorServiceStatusList
                Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
                Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mModelMonitorAMPRefStatusList.Item(ID).ID, mModelMonitorAMPRefStatusList.Item(ID).AssemblyStatusID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("ATA") = mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString
                Session("Description") = mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("ModelSerialNo") = mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo
                mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description

				Dim DoneOn As String = ""
				If mPrevAssemblyMonitorServiceStatus.DoneOn.ToString <> "" Then
					DoneOn = mPrevAssemblyMonitorServiceStatus.DoneOn.ToString
				Else
					DoneOn = Today.Date.ToString
				End If

				mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, DoneOn, mMachine.HourType)
				Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
                Session("From") = 1 'Edit record

                Session("mMachine") = mMachine
                Session("mAssemblyStatus") = mAssemblyStatus


                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
                Session("mBoardInfo") = mBoardInfo
                '**************************************


                mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.
                                                                GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID,
                                                                                                                 mAssemblyMonitorServiceStatus.ModelMonitorServiceID,
                                                                                                                 mMachine.HourType, TaskNo:=mAssemblyMonitorServiceStatus.ModelMonitorService.TaskCardNo)
                Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList

                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                MarkLog(Util.Action.View, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow()", True)

            Case 2 'Assembly Insp'
                Dim mUpdateComplyHistoryAssemblyMonitorInspStatusList As UpdateComplyHistoryAssemblyMonitorInspStatusList
                Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
                Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mModelMonitorAMPRefStatusList.Item(ID).ID, mModelMonitorAMPRefStatusList.Item(ID).AssemblyStatusID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("ATA") = mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString
                Session("Description") = mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("ModelSerialNo") = mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo
                mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description

				Dim DoneOn As String = ""
				If mPrevAssemblyMonitorInspStatus.DoneOn.ToString <> "" Then
					DoneOn = mPrevAssemblyMonitorInspStatus.DoneOn.ToString
				Else
					DoneOn = Today.Date.ToString
				End If

				mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, DoneOn, mMachine.HourType)
				Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
                Session("From") = 1 'Edit record

                Session("mMachine") = mMachine
                Session("mAssemblyStatus") = mAssemblyStatus


                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
                Session("mBoardInfo") = mBoardInfo
                '**************************************


                mUpdateComplyHistoryAssemblyMonitorInspStatusList = UpdateComplyHistoryAssemblyMonitorInspStatusList.
                            GetComplyHistoryAssemblyMonitorInspStatusList(mAssemblyStatus.AssemblyID,
                                                                          mAssemblyMonitorInspStatus.ModelMonitorInspID,
                                                                          mMachine.HourType)

                Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList

                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                MarkLog(Util.Action.View, "AssemblyInspMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionHistoryWindow", "OpenInspectionHistoryWindow()", True)

            Case 3 'Assembly Mod'
                Dim mUpdateComplyHistoryAssemblyMonitorModStatusList As UpdateComplyHistoryAssemblyMonitorModStatusList
                Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
                Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mModelMonitorAMPRefStatusList.Item(ID).ID, mModelMonitorAMPRefStatusList.Item(ID).AssemblyStatusID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyInfo") = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("ATA") = mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString
                Session("Description") = mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("ModelSerialNo") = mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo
                mAircraft = mModelMonitorAMPRefStatusList(ID).RegNo
                mMonitorInfo = mModelMonitorAMPRefStatusList(ID).TypeName
                mMonitorType = mModelMonitorAMPRefStatusList(ID).MonitorType
                mMonitorDesc = mModelMonitorAMPRefStatusList(ID).Description

				Dim DoneOn As String = ""
				If mPrevAssemblyMonitorModStatus.DoneOn.ToString <> "" Then
					DoneOn = mPrevAssemblyMonitorModStatus.DoneOn.ToString
				Else
					DoneOn = Today.Date.ToString
				End If

				mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, DoneOn, mMachine.HourType)
				Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
                Session("From") = 1 'Edit record

                Session("mMachine") = mMachine
                Session("mAssemblyStatus") = mAssemblyStatus


                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
                Session("mBoardInfo") = mBoardInfo
                '**************************************


                mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.
                            GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID,
                                                                         mAssemblyMonitorModStatus.ModelMonitorModID,
                                                                         mMachine.HourType)

                Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList

                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                MarkLog(Util.Action.View, "AssemblyModMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDirectiveHistoryWindow", "OpenDirectiveHistoryWindow()", True)


            Case 4 'Comp Service'
                Dim mUpdateComplyHistoryCompMonitorServiceStatusList As UpdateComplyHistoryCompMonitorServiceStatusList
                Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
                Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus

                Dim mHourType As Integer = 1
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus

                Session("EnFrom") = 1 'EditRecord

                Dim mCompStatus As CompStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mHourType = mMachine.HourType

                mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mModelMonitorAMPRefStatusList(ID).ID, mModelMonitorAMPRefStatusList(ID).AssemblyStatusID, mModelMonitorAMPRefStatusList(ID).CompStatusID, mHourType)


				Dim DoneOn As String = ""
				If mPrevCompMonitorServiceStatus.DoneOn.ToString <> "" Then
					DoneOn = mPrevCompMonitorServiceStatus.DoneOn.ToString
				Else
					DoneOn = Today.Date.ToString
				End If

				mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, DoneOn, mHourType)

				mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mModelMonitorAMPRefStatusList.Item(ID).CompStatusID, mModelMonitorAMPRefStatusList.Item(ID).AssemblyStatusID, mModelMonitorAMPRefStatusList.Item(ID).DoneOnFormatted.ToString)
                mCompInfo = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("mCompInfo") = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description

                Session("ATA") = mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString
                Session("Description") = mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("PartSerialNo") = mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo
                MaintDetail = "Reg No. : " + mModelMonitorAMPRefStatusList(ID).RegNo & " Assembly Info : " & mModelMonitorAMPRefStatusList(ID).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mModelMonitorAMPRefStatusList(ID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mModelMonitorAMPRefStatusList(ID).TypeName & " Done On Date : " & mModelMonitorAMPRefStatusList(ID).DoneOnFormatted & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).DoneOnValue
                MarkLog(Util.Action.View, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mModelMonitorAMPRefStatusList(ID).ID, EventLogID)


                Session("mMachine") = mMachine
                '''''''''' Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompStatus") = mCompStatus
                'RemoveSession()


                mUpdateComplyHistoryCompMonitorServiceStatusList = UpdateComplyHistoryCompMonitorServiceStatusList.GetComplyHistoryCompMonitorServiceStatusList(mCompStatus.CompID, mCompMonitorServiceStatus.PartMonitorServiceID, mHourType, TaskNo:=mCompMonitorServiceStatus.PartMonitorService.TaskCardNo)
                Session("mUpdateComplyHistoryCompMonitorServiceStatusList") = mUpdateComplyHistoryCompMonitorServiceStatusList

                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorServiceStatusList.aspx?GChildPage2=ID.aspx');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompServiceHistoryWindow", "OpenCompServiceHistoryWindow();", True)
            Case 5 'Comp Inspection'

                Dim mUpdateComplyHistoryCompMonitorInspStatusList As UpdateComplyHistoryCompMonitorInspStatusList
                Dim mCompMonitorInspStatus As CompMonitorInspStatus
                Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus

                Dim mHourType As Integer = 1
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus

                Session("EnFrom") = 1 'EditRecord

                Dim mCompStatus As CompStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mHourType = mMachine.HourType

                mPrevCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mModelMonitorAMPRefStatusList(ID).ID, mModelMonitorAMPRefStatusList(ID).AssemblyStatusID, mModelMonitorAMPRefStatusList(ID).CompStatusID, mHourType)

				Dim DoneOn As String = ""
				If mPrevCompMonitorInspStatus.DoneOn.ToString <> "" Then
					DoneOn = mPrevCompMonitorInspStatus.DoneOn.ToString
				Else
					DoneOn = Today.Date.ToString
				End If

				mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatusFromEntry(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, DoneOn, mHourType)

				mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mModelMonitorAMPRefStatusList.Item(ID).CompStatusID, mModelMonitorAMPRefStatusList.Item(ID).AssemblyStatusID, mModelMonitorAMPRefStatusList.Item(ID).DoneOnFormatted.ToString)
                mCompInfo = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("mCompInfo") = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description

                Session("ATA") = mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString
                Session("Description") = mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("PartSerialNo") = mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo
                MaintDetail = "Reg No. : " + mModelMonitorAMPRefStatusList(ID).RegNo & " Assembly Info : " & mModelMonitorAMPRefStatusList(ID).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mModelMonitorAMPRefStatusList(ID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mModelMonitorAMPRefStatusList(ID).TypeName & " Done On Date : " & mModelMonitorAMPRefStatusList(ID).DoneOnFormatted & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).DoneOnValue
                MarkLog(Util.Action.View, "ComponentInspMonitor", MaintDetail, Util.ErrorType.NoError, mModelMonitorAMPRefStatusList(ID).ID, EventLogID)


                Session("mMachine") = mMachine
                '''''''''' Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompStatus") = mCompStatus
                'RemoveSession()


                mUpdateComplyHistoryCompMonitorInspStatusList = UpdateComplyHistoryCompMonitorInspStatusList.GetComplyHistoryCompMonitorInspStatusList(mCompStatus.CompID, mCompMonitorInspStatus.PartMonitorInspID, mHourType)
                Session("mUpdateComplyHistoryCompMonitorInspStatusList") = mUpdateComplyHistoryCompMonitorInspStatusList

                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorInspStatusList.aspx?GChildPage2=ID.aspx');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompInspHistoryWindow", "OpenCompInspHistoryWindow();", True)

            Case 6 'Comp Modification'

                Dim mUpdateComplyHistoryCompMonitorModStatusList As UpdateComplyHistoryCompMonitorModStatusList
                Dim mCompMonitorModStatus As CompMonitorModStatus
                Dim mPrevCompMonitorModStatus As CompMonitorModStatus

                Dim mHourType As Integer = 1
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus

                Session("EnFrom") = 1 'EditRecord

                Dim mCompStatus As CompStatus
                mMachine = Machine.GetMachine(mModelMonitorAMPRefStatusList(ID).MachineID)
                mHourType = mMachine.HourType

                mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mModelMonitorAMPRefStatusList(ID).ID, mModelMonitorAMPRefStatusList(ID).AssemblyStatusID, mModelMonitorAMPRefStatusList(ID).CompStatusID, mHourType)

				Dim DoneOn As String = ""
				If mPrevCompMonitorModStatus.DoneOn.ToString <> "" Then
					DoneOn = mPrevCompMonitorModStatus.DoneOn.ToString
				Else
					DoneOn = Today.Date.ToString
				End If

				mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, DoneOn.ToString, mHourType)

				mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mModelMonitorAMPRefStatusList(ID).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mModelMonitorAMPRefStatusList.Item(ID).CompStatusID, mModelMonitorAMPRefStatusList.Item(ID).AssemblyStatusID, mModelMonitorAMPRefStatusList.Item(ID).DoneOnFormatted.ToString)
                mCompInfo = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("mCompInfo") = mModelMonitorAMPRefStatusList.Item(ID).RegNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).ModelSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo + "->" + mModelMonitorAMPRefStatusList.Item(ID).Reference + "->" + mModelMonitorAMPRefStatusList.Item(ID).TypeName + "->" + mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString + "->" + mModelMonitorAMPRefStatusList.Item(ID).Description

                Session("ATA") = mModelMonitorAMPRefStatusList.Item(ID).ATACode.ToString
                Session("Description") = mModelMonitorAMPRefStatusList.Item(ID).Description
                Session("PartSerialNo") = mModelMonitorAMPRefStatusList.Item(ID).PartSerialNo
                MaintDetail = "Reg No. : " + mModelMonitorAMPRefStatusList(ID).RegNo & " Assembly Info : " & mModelMonitorAMPRefStatusList(ID).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mModelMonitorAMPRefStatusList(ID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mModelMonitorAMPRefStatusList(ID).TypeName & " Done On Date : " & mModelMonitorAMPRefStatusList(ID).DoneOnFormatted & " Done On Value : " & mModelMonitorAMPRefStatusList(ID).DoneOnValue
                MarkLog(Util.Action.View, "ComponentModMonitor", MaintDetail, Util.ErrorType.NoError, mModelMonitorAMPRefStatusList(ID).ID, EventLogID)


                Session("mMachine") = mMachine
                '''''''''' Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompStatus") = mCompStatus
                'RemoveSession()


                mUpdateComplyHistoryCompMonitorModStatusList = UpdateComplyHistoryCompMonitorModStatusList.GetComplyHistoryCompMonitorModStatusList(mCompStatus.CompID, mCompMonitorModStatus.PartMonitorModID, mHourType)
                Session("mUpdateComplyHistoryCompMonitorModStatusList") = mUpdateComplyHistoryCompMonitorModStatusList

                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorModStatusList.aspx?GChildPage2=ID.aspx');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompDirectiveHistoryWindow", "OpenCompDirectiveHistoryWindow();", True)

        End Select


    End Sub
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        If rbPartNo.Checked = True Then
            txtSearch.Text = Session("SearchText")
            dgItemReceiptIssueTransactions1.Visible = False
            upnlItemReceiptIssueTransactions.Update()
            lblItemReceiptIssueTransactions.Visible = False
            ''lblSerialNo.Visible = False
            txtSerialNo.Visible = False
            mItemStockList = ItemStockList.GetItemStockList(PartName, PartDescription, Today.Date.ToString)
            dgItemStockList1.DataSource = mItemStockList
            dgItemStockList1.DataBind()
            lblItemList.Visible = True
            lblItemList.Text = "From Stores List Of " + mItemStockList.Count.ToString + " Record(s)"
            upnlItemStockList.Update()
        ElseIf rbSerialNo.Checked = True Then
            lblItemList.Visible = False
            dgItemStockList1.Visible = False
            'lblPartNo.Visible = False
            txtSearch.Visible = False
            lblItemReceiptIssueTransactions.Visible = True
            dgItemReceiptIssueTransactions1.Visible = True
            upnlItemReceiptIssueTransactions.Update()
            ''lblSerialNo.Visible = True
            txtSerialNo.Visible = True
            txtSerialNo.Text = Session("SerialNo")
            mItemReceiptIssueTransactions = ItemReceiptIssueTransactions.GetItemReceiptIssueTransactions(SerialNo)
            dgItemReceiptIssueTransactions1.DataSource = mItemReceiptIssueTransactions
            dgItemReceiptIssueTransactions1.DataBind()
            upnlItemReceiptIssueTransactions.Update()
            upnlItemStockList.Update()
        ElseIf rbReferenceNo.Checked = True Then

            txtReferenceNo.Text = Session("ReferenceNo")
        ElseIf rbCodeNo.Checked = True Then
            txtSerialNo.Text = Session("SerialNo")
        End If
    End Sub

    Private Sub Controlvisibility()
        If User.IsInRole("MaintDashBoardView") And User.IsInRole("InvDashBoardView") Then
            rbPartNo.Visible = True
            rbSerialNo.Visible = True
            rbReferenceNo.Visible = True
            rbCodeNo.Visible = True
        Else
            If User.IsInRole("MaintDashBoardView") Then
                rbPartNo.Visible = False
                rbSerialNo.Visible = True
                rbReferenceNo.Visible = True
                rbCodeNo.Visible = True
            ElseIf User.IsInRole("InvDashBoardView") Then
                rbPartNo.Visible = True
                rbSerialNo.Visible = True
                rbReferenceNo.Visible = False
                rbCodeNo.Visible = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "DashboardForInventory.aspx"
            If Session("FromGrid") = "FromGrid" Then
                rbPartNo.Checked = chkPartNo
                rbSerialNo.Checked = chkSerialNo
                rbReferenceNo.Checked = chkReferenceNo
                rbCodeNo.Checked = chkCodeNo
                Session("FromGrid") = ""
                DataFieldBind()
                imgFindNow_Click(imgFindNow, New ImageClickEventArgs(0, 0))
            End If
            Controlvisibility()
        End If

        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            rbCodeNo.Text = "Task No."
        Else
            rbCodeNo.Text = "TaskCard/Code No."
        End If
    End Sub
    Private Sub imgFindNow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgFindNow.Click
        'If rbPartNo.Checked = True And txtSearch.Text.Trim = "" Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Enter Part No.", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'Else
        If rbSerialNo.Checked = True And txtSerialNo.Text.Trim = "" Then
            MSGBoxCtrl.Show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Enter Serial No.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf rbCodeNo.Checked = True And txtSerialNo.Text.Trim = "" Then
            MSGBoxCtrl.Show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Enter TaskCard Or Code No.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf rbReferenceNo.Checked = True And txtReferenceNo.Text.Trim = "" Then
            MSGBoxCtrl.Show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Select Reference No.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If rbPartNo.Checked = True Then

            dgItemReceiptIssueTransactions1.Visible = False
            upnlItemReceiptIssueTransactions.Update()
            lblItemReceiptIssueTransactions.Visible = False
            ''lblSerialNo.Visible = False
            txtSerialNo.Visible = False
            dgInstallationRemoval1.Visible = False
            upnlInstallationRemoval.Update()
            'lblReferenceNo.Visible = False
            txtReferenceNo.Visible = False

            If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
                ItemName = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
                ItemDescription = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
                mItemStockList = ItemStockList.GetItemStockList(ItemName, ItemDescription, Today.Date.ToString, txtSerialNo.Text.Trim)
                Session("SearchText") = txtSearch.Text.Trim
            Else
                ItemName = Trim(txtSearch.Text)
                ItemDescription = Trim(txtSearch.Text)
                mItemStockList = ItemStockList.GetItemStockList(ItemName, ItemDescription, Today.Date.ToString, txtSerialNo.Text.Trim)
                Session("SearchText") = txtSearch.Text.Trim
            End If

            dgModelMonitorAMPRefStatusList.Visible = False
            dgModelMonitorAMPRefStatusList.DataSource = Nothing
            dgModelMonitorAMPRefStatusList.DataBind()
            upnlItemReceiptIssueTransactions.Update()
            upnlModelMonitorAMPRefStatusList.Update()

            Session("PartName") = ItemName
            Session("PartDescription") = ItemDescription
            lblItemList.Visible = True
            lblItemList.Text = "From Stores List Of " + mItemStockList.Count.ToString + " Record(s)"
            dgItemStockList1.DataSource = mItemStockList
            dgItemStockList1.DataBind()

            Session("mItemStockList") = mItemStockList

            dgTaskCardList.Visible = False
            dgTaskCardList.DataSource = Nothing
            dgTaskCardList.DataBind()
            upnlTaskCardList.Update()

            upnlItemStockList.Update()


            'Removal/Installation Details

            mCompHistoryList = CompHistoryList.GetCompHistoryList(Today.Date.ToString, ItemName, ShowCompianceDetails:=False)
            If mCompHistoryList.Count > 0 Then
                lblInstallationRemoval1.Visible = True
                dgInstallationRemoval1.Visible = True
                dgInstallationRemoval1.DataSource = mCompHistoryList
                dgInstallationRemoval1.DataBind()
                lblInstallationRemoval1.Text = "List of Removal/Installations as per criteria : " + mCompHistoryList.Count.ToString + " Record(s) Found"
                upnlInstallationRemoval.Update()
            Else
                lblInstallationRemoval1.Visible = False
                dgInstallationRemoval1.Visible = False
                upnlInstallationRemoval.Update()
            End If

            'Compiliance Details


            mModelMonitorAMPRefStatusList = ModelMonitorAMPRefStatusList.GetModelMonitorAMPRefStatusList("", ItemName, PartNoRequired:=True)
            If mModelMonitorAMPRefStatusList.Count <= 0 Then
                lblModelMonitorAMPRefStatusList.Visible = False
                dgModelMonitorAMPRefStatusList.DataSource = Nothing
                dgModelMonitorAMPRefStatusList.DataBind()
                dgModelMonitorAMPRefStatusList.Visible = False
                upnlModelMonitorAMPRefStatusList.Update()
            Else
                lblModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.DataSource = mModelMonitorAMPRefStatusList
                mModelMonitorAMPRefStatusList.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
                dgModelMonitorAMPRefStatusList.DataBind()
                lblModelMonitorAMPRefStatusList.Text = "List of Maintenance Activities as per criteria : " + mModelMonitorAMPRefStatusList.Count.ToString + " Record(s) Found"
                upnlModelMonitorAMPRefStatusList.Update()
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    dgModelMonitorAMPRefStatusList.HeaderRow.Cells(2).Text = "Task No./ Directive No."
                End If
            End If

        ElseIf rbSerialNo.Checked = True Then
            lblItemList.Visible = False
            dgItemStockList1.Visible = False
            'lblPartNo.Visible = False
            txtSearch.Visible = False

            'lblReferenceNo.Visible = False
            txtReferenceNo.Visible = False

            ''lblSerialNo.Visible = True
            txtSerialNo.Visible = True
            ''lblSerialNo.Text = "Serial No."
            SerialNo = txtSerialNo.Text.Trim
            Session("SerialNo") = SerialNo
            mItemReceiptIssueTransactions = ItemReceiptIssueTransactions.GetItemReceiptIssueTransactions(txtSerialNo.Text.Trim)

            If mItemReceiptIssueTransactions.Count > 0 Then
                lblItemReceiptIssueTransactions.Visible = True
                lblItemReceiptIssueTransactions.Text = "As per criteria : " + mItemReceiptIssueTransactions.Count.ToString + " Record(s) Found"
                dgItemReceiptIssueTransactions1.Visible = True
                dgItemReceiptIssueTransactions1.DataSource = mItemReceiptIssueTransactions
                dgItemReceiptIssueTransactions1.DataBind()
                upnlItemReceiptIssueTransactions.Update()
            Else
                lblItemReceiptIssueTransactions.Visible = False
                dgItemReceiptIssueTransactions1.Visible = False
                dgItemReceiptIssueTransactions1.Visible = False
                upnlItemReceiptIssueTransactions.Update()
            End If

            Dim mCompIDBySerialNo As CompIDBySerialNo
            mCompIDBySerialNo = CompIDBySerialNo.GetCompIDBySerialNo(SerialNo)
            Dim CompID As Guid

            If mCompIDBySerialNo.Count > 0 Then
                CompID = mCompIDBySerialNo(0).CompID
            Else
                CompID = Guid.Empty
            End If


            mCompHistoryList = CompHistoryList.GetCompHistoryList(Today.Date.ToString, CompID, ShowCompianceDetails:=False)
            If mCompHistoryList.Count > 0 Then
                lblInstallationRemoval1.Visible = True
                dgInstallationRemoval1.Visible = True
                dgInstallationRemoval1.DataSource = mCompHistoryList
                dgInstallationRemoval1.DataBind()
                lblInstallationRemoval1.Text = "List of Removal/Installations as per criteria : " + mCompHistoryList.Count.ToString + " Record(s) Found"
                upnlInstallationRemoval.Update()
            Else
                lblInstallationRemoval1.Visible = False
                dgInstallationRemoval1.Visible = False
                upnlInstallationRemoval.Update()
            End If
            ''lblModelMonitorAMPRefStatusList.Visible = False
            ''dgModelMonitorAMPRefStatusList.Visible = False
            ''dgModelMonitorAMPRefStatusList.DataSource = Nothing
            ''dgModelMonitorAMPRefStatusList.DataBind()

            'Compiliance Details

            mModelMonitorAMPRefStatusList = ModelMonitorAMPRefStatusList.GetModelMonitorAMPRefStatusList("", SerialNo, SerialNoRequired:=True)
            If mModelMonitorAMPRefStatusList.Count <= 0 Then
                lblModelMonitorAMPRefStatusList.Visible = False
                dgModelMonitorAMPRefStatusList.DataSource = Nothing
                dgModelMonitorAMPRefStatusList.DataBind()
                dgModelMonitorAMPRefStatusList.Visible = False
                upnlModelMonitorAMPRefStatusList.Update()
            Else
                lblModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.DataSource = mModelMonitorAMPRefStatusList
                mModelMonitorAMPRefStatusList.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
                dgModelMonitorAMPRefStatusList.DataBind()
                lblModelMonitorAMPRefStatusList.Text = "List of Maintenance Activities as per criteria : " + mModelMonitorAMPRefStatusList.Count.ToString + " Record(s) Found"
                upnlModelMonitorAMPRefStatusList.Update()
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    dgModelMonitorAMPRefStatusList.HeaderRow.Cells(2).Text = "Task No./ Directive No."
                End If
            End If


            upnlItemReceiptIssueTransactions.Update()
            upnlModelMonitorAMPRefStatusList.Update()

            dgTaskCardList.Visible = False
            dgTaskCardList.DataSource = Nothing
            dgTaskCardList.DataBind()
            upnlTaskCardList.Update()
            upnlItemStockList.Update()


        ElseIf rbReferenceNo.Checked = True Then
            lblItemList.Visible = False
            dgItemStockList1.Visible = False
            txtSearch.Visible = False
            txtSerialNo.Visible = False
            txtReferenceNo.Visible = True
            ReferenceNo = txtReferenceNo.Text.Trim
            Session("ReferenceNo") = ReferenceNo
            upnlItemStockList.Update()

            mModelMonitorAMPRefStatusList = ModelMonitorAMPRefStatusList.GetModelMonitorAMPRefStatusList(txtReferenceNo.Text, RefNoRequired:=True)

            Session("mModelMonitorAMPRefStatusList") = mModelMonitorAMPRefStatusList

            If mModelMonitorAMPRefStatusList.Count <= 0 Then
                MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                lblModelMonitorAMPRefStatusList.Visible = False
                dgModelMonitorAMPRefStatusList.DataSource = Nothing
                dgModelMonitorAMPRefStatusList.DataBind()
                upnlModelMonitorAMPRefStatusList.Update()
                Exit Sub
            Else
                lblModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.DataSource = mModelMonitorAMPRefStatusList
                mModelMonitorAMPRefStatusList.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
                lblModelMonitorAMPRefStatusList.Text = "List of Maintenance Activities as per criteria : " + mModelMonitorAMPRefStatusList.Count.ToString + " Record(s) Found"
                dgModelMonitorAMPRefStatusList.DataBind()
                upnlModelMonitorAMPRefStatusList.Update()
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    dgModelMonitorAMPRefStatusList.HeaderRow.Cells(2).Text = "Task No./Directive No."
                End If
            End If
            dgTaskCardList.Visible = False
            dgTaskCardList.DataSource = Nothing
            dgTaskCardList.DataBind()
            upnlTaskCardList.Update()

            upnlItemStockList.Update()
            lblInstallationRemoval1.Visible = False
            dgInstallationRemoval1.Visible = False
            upnlInstallationRemoval.Update()
        ElseIf rbCodeNo.Checked = True Then
            lblItemList.Visible = False
            dgItemStockList1.Visible = False
            txtSearch.Visible = False
            txtSerialNo.Visible = True
            txtReferenceNo.Visible = False
            SerialNo = txtSerialNo.Text.Trim
            Session("SerialNo") = SerialNo
            upnlItemStockList.Update()

            mModelMonitorAMPRefStatusList = ModelMonitorAMPRefStatusList.GetModelMonitorAMPRefStatusList("", txtSerialNo.Text, TaskNoRequired:=True)

            Session("mModelMonitorAMPRefStatusList") = mModelMonitorAMPRefStatusList

            If mModelMonitorAMPRefStatusList.Count <= 0 Then
                lblModelMonitorAMPRefStatusList.Visible = False
                dgModelMonitorAMPRefStatusList.DataSource = Nothing
                dgModelMonitorAMPRefStatusList.DataBind()
                dgModelMonitorAMPRefStatusList.Visible = False
                upnlModelMonitorAMPRefStatusList.Update()
            Else
                lblModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.Visible = True
                dgModelMonitorAMPRefStatusList.DataSource = mModelMonitorAMPRefStatusList
                mModelMonitorAMPRefStatusList.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
                dgModelMonitorAMPRefStatusList.DataBind()
                lblModelMonitorAMPRefStatusList.Text = "List of Maintenance Activities as per criteria : " + mModelMonitorAMPRefStatusList.Count.ToString + " Record(s) Found"
                upnlModelMonitorAMPRefStatusList.Update()
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    dgModelMonitorAMPRefStatusList.HeaderRow.Cells(2).Text = "Task No./ Directive No."
                End If
            End If

            mTaskCardList = TaskCardList.GetTaskCardList("", "", "", txtSerialNo.Text.Trim)
            dgTaskCardList.DataSource = mTaskCardList
            dgTaskCardList.DataBind()
            If mTaskCardList.Count > 0 Then
                lblTaskCardResultList.Visible = True
                lblTaskCardResultList.Text = "List of TaskCards as per criteria : " & "" & mTaskCardList.Count & " Record(s) found."
                dgTaskCardList.Visible = True
            Else
                lblTaskCardResultList.Visible = False
                dgTaskCardList.Visible = False
            End If
            upnlTaskCardList.Update()
            upnlItemStockList.Update()
            lblInstallationRemoval1.Visible = False
            dgInstallationRemoval1.Visible = False
            upnlInstallationRemoval.Update()
        End If
        Session("chkPartNo") = rbPartNo.Checked
        Session("chkSerialNo") = rbSerialNo.Checked
        Session("chkReferenceNo") = rbReferenceNo.Checked
        Session("chkCodeNo") = rbCodeNo.Checked


        Session("mCompHistoryList") = mCompHistoryList
        Session("mModelMonitorAMPRefStatusList") = mModelMonitorAMPRefStatusList
        upnlControls.Update()
    End Sub
    Private Sub dgItemStockList1_RowCommand1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItemStockList1.RowCommand
        Dim index As Int32
        Select Case e.CommandName
            Case "Transactions"
                index = (CInt(e.CommandArgument) + (dgItemStockList1.PageSize * dgItemStockList1.PageIndex))
                Dim ItemName As String = dgItemStockList1.DataKeys(CInt(e.CommandArgument)).Values(0).ToString 'CStr(dgItemStockList1.Rows(index).Cells(1).Text)  'CStr(e.Item.Cells(1).Text)
                Session("ItemName") = ItemName
                Session("FromGrid") = "FromGrid"
                Response.Redirect("wfTransactionsOfInventory_Ajax.aspx")
            Case "StockDetail"
                index = (CInt(e.CommandArgument) + (dgItemStockList1.PageSize * dgItemStockList1.PageIndex))
                Dim ItemName As String = dgItemStockList1.DataKeys(CInt(e.CommandArgument)).Values(0).ToString 'CStr(dgItemStockList1.Rows(index).Cells(1).Text)
                Dim ItemDescription As String = dgItemStockList1.DataKeys(CInt(e.CommandArgument)).Values(1).ToString 'CStr(dgItemStockList1.Rows(index).Cells(2).Text)
                Dim StockQty As String = CStr(dgItemStockList1.Rows(index).Cells(3).Text)
                Session("ItemName") = ItemName
                Session("ItemDescription") = ItemDescription
                Session("StockQty") = StockQty
                Session("FromGrid") = "FromGrid"
                Response.Redirect("wfItemStockDetailList_Ajax.aspx") 'Ajay 27-Dec-2022
            Case "BinCard"
                index = (CInt(e.CommandArgument) + (dgItemStockList1.PageSize * dgItemStockList1.PageIndex))
                Dim ItemName As String = dgItemStockList1.DataKeys(CInt(e.CommandArgument)).Values(0).ToString

                Dim ds As New dsPartHistory
                Dim da As New CSLA.Data.ObjectAdapter
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim objsearch As rptSearchingCriteria
                Dim mCompanyDetail As New CompanyDetail
                Dim rpt As rptPartHistory

                ' rpt = rptPartHistory.GetPartHistory(DirectCast(mItemStockList.CurrentItem, Flypal.ItemStockList.ItemStockListInfo).ItemID, 4, "", Guid.Empty)
                rpt = rptPartHistory.GetPartHistory(ItemName, "", 4, "", Guid.Empty, Guid.Empty, False, True, "Landing Value", SerialNo:="")
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this part", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Applicability As String = ""
                If rpt.Count > 0 Then
                    For i As Integer = 0 To rpt.Count - 1
                        If rpt(i).Applicability <> "" Then
                            Applicability = rpt(i).Applicability
                        End If
                    Next
                End If
                Dim AlternateParts As String = ""
                If rpt.Count > 0 Then
                    For i As Integer = 0 To rpt.Count - 1
                        If rpt(i).AlternateParts <> "" Then
                            AlternateParts = rpt(i).AlternateParts
                        End If
                    Next
                End If
                mItem = Item.GetItem((New Guid(dgItemStockList1.Rows(index).Cells(0).Text))) 'Cell(0) Is ItemID
                If Not mItem.ATAID.Equals(Guid.Empty) Then
                    mATA = ATA.GetATA(mItem.ATAID)
                    ATACode = mATA.ATACode & " - " & mATA.ATANomenclature
                End If

                objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", mItem.Name, AlternateParts,
                                                             AppSettings("Logo"), Applicability, ATACode, "",
                                                             "Landing Value", " Bin Card Report (Landing Value)", mItem.Description, "", 0, FromStore:="", WorkShop:=mItem.UnitName)


                mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                mCompanyDetail.WebSite, "", mItem.Location, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

                ds.Clear()

                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, mrptImage)
                myReport = New crptBinCardReport
                da.Fill(ds, rpt)
                da.Fill(ds, objsearch)
                da.Fill(ds, Report)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            Case "ShowPartStatus"  'Added By Prashant on 19-Feb-2021 Heligo19022021
                index = (CInt(e.CommandArgument) + (dgItemStockList1.PageSize * dgItemStockList1.PageIndex)) 'CInt(e.CommandArgument)
                Dim mItemStatus As Item = Item.GetItem((New Guid(dgItemStockList1.Rows(index).Cells(0).Text))) 'Cell(0) Is ItemID
                Dim LinkID As Guid = mItemStatus.LinkID
                Dim Unit As String = mItemStatus.UnitName

                Dim mStockPartStatus As rptStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
                Dim mOnOrderPartStatus As rptOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
                Dim mReturnablePartStatus As rptReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
                Dim mTransitPartList As rptTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
                Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode"))

                Session("PartNo") = mItemStatus.Name
                Session("Description") = mItemStatus.Description
                Session("Unit") = Unit

                Session("mStockPartStatus") = mStockPartStatus
                Session("mOnOrderPartStatus") = mOnOrderPartStatus
                Session("mReturnablePartStatus") = mReturnablePartStatus
                Session("mTransitPartList") = mTransitPartList
                Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
                Session("LinkID") = LinkID
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenShowPartNoStatusWindow", "OpenShowPartNoStatusWindow();", True)
            Case "PartStatus"  'Added By Prashant on 9-Mar-2021 Heligo09032021
                index = (CInt(e.CommandArgument) + (dgItemStockList1.PageSize * dgItemStockList1.PageIndex)) 'CInt(e.CommandArgument)
                Dim mNameOfItem As String = dgItemStockList1.DataKeys(CInt(e.CommandArgument)).Values(0).ToString 'CStr(dgItemStockList1.Rows(index).Cells(1).Text)
                Session("FromPOItemName") = mNameOfItem
                'Session("mOrderForPartStatus") = mOrder
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenPartStatusWindow", "OpenPartStatusWindow();", True)
        End Select
    End Sub
    Private Sub rbPartNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPartNo.CheckedChanged
        dgItemStockList1.Visible = True
        dgItemReceiptIssueTransactions1.DataSource = Nothing
        dgItemReceiptIssueTransactions1.DataBind()
        lblItemReceiptIssueTransactions.Visible = False
        dgItemReceiptIssueTransactions1.Visible = False
        'lblPartNo.Visible = True
        txtSearch.Visible = True
        txtSerialNo.Text = ""
        'lblSerialNo.Visible = False
        txtSerialNo.Visible = False
        'lblReferenceNo.Visible = False
        txtReferenceNo.Visible = False

        lblModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.DataSource = Nothing
        dgModelMonitorAMPRefStatusList.DataBind()
        upnlItemReceiptIssueTransactions.Update()
        upnlModelMonitorAMPRefStatusList.Update()

        txtReferenceNo.Text = ""
        txtSerialNo.Text = ""

        'mItemStockList = ItemStockList.GetItemStockList("", "", Today.Date.ToString, "")
        'dgItemStockList1.DataSource = mItemStockList
        'dgItemStockList1.DataBind()
        'lblItemList.Visible = True
        'lblItemList.Text = "From Stores List Of " + mItemStockList.Count.ToString + " Record(s)"

        upnlItemReceiptIssueTransactions.Update()
        upnlItemStockList.Update()
        upnlControls.Update()
        lblTaskCardResultList.Visible = False
        dgTaskCardList.Visible = False
        upnlTaskCardList.Update()
    End Sub
    Private Sub rbSerialNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSerialNo.CheckedChanged
        lblItemList.Visible = False
        dgItemStockList1.Visible = False
        dgItemStockList1.DataSource = Nothing
        dgItemStockList1.DataBind()
        dgItemReceiptIssueTransactions1.Visible = True
        txtSearch.Text = ""
        txtSearch.Visible = False
        txtSerialNo.Visible = True
        txtSerialNo.Attributes.Add("placeholder", "Enter Serial No.")
        txtSerialNo.Attributes.Add("class", "clsTextBoxSearch_Ajax")
        txtSerialNo.Text = ""
        txtReferenceNo.Visible = False

        txtReferenceNo.Text = ""
        lblModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.DataSource = Nothing
        dgModelMonitorAMPRefStatusList.DataBind()
        upnlItemReceiptIssueTransactions.Update()
        upnlModelMonitorAMPRefStatusList.Update()

        upnlItemStockList.Update()
        upnlControls.Update()
        lblTaskCardResultList.Visible = False
        dgTaskCardList.Visible = False
        upnlTaskCardList.Update()
    End Sub
    Protected Sub rbReferenceNo_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbReferenceNo.CheckedChanged
        lblItemList.Visible = False
        dgItemStockList1.Visible = False
        dgItemStockList1.DataSource = Nothing
        dgItemStockList1.DataBind()
        txtSearch.Visible = False
        txtSerialNo.Visible = False
        txtReferenceNo.Visible = True

        dgItemReceiptIssueTransactions1.DataSource = Nothing
        dgItemReceiptIssueTransactions1.DataBind()
        lblItemReceiptIssueTransactions.Visible = False
        dgItemReceiptIssueTransactions1.Visible = False
        upnlItemReceiptIssueTransactions.Update()

        txtSerialNo.Text = ""
        txtSearch.Text = ""
        upnlItemStockList.Update()
        upnlControls.Update()
        lblModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.DataSource = Nothing
        dgModelMonitorAMPRefStatusList.DataBind()
        upnlModelMonitorAMPRefStatusList.Update()

        lblTaskCardResultList.Visible = False
        dgTaskCardList.Visible = False
        upnlTaskCardList.Update()
        lblInstallationRemoval1.Visible = False
        dgInstallationRemoval1.Visible = False
        upnlInstallationRemoval.Update()
    End Sub
    Private Sub rbCodeNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCodeNo.CheckedChanged
        lblItemList.Visible = False
        dgItemStockList1.Visible = False
        dgItemStockList1.DataSource = Nothing
        dgItemStockList1.DataBind()
        txtSearch.Visible = False
        txtSerialNo.Visible = True
        txtSerialNo.Attributes.Add("placeholder", "Enter Task Card/Code No.")
        txtSerialNo.Text = ""
        txtReferenceNo.Visible = False
        txtReferenceNo.Text = ""

        lblItemReceiptIssueTransactions.Visible = False
        dgItemReceiptIssueTransactions1.DataSource = Nothing
        dgItemReceiptIssueTransactions1.DataBind()


        dgItemReceiptIssueTransactions1.Visible = False
        upnlItemReceiptIssueTransactions.Update()

        lblModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.Visible = False
        dgModelMonitorAMPRefStatusList.DataSource = Nothing
        dgModelMonitorAMPRefStatusList.DataBind()

        upnlModelMonitorAMPRefStatusList.Update()

        txtSerialNo.Text = ""
        txtSearch.Text = ""
        upnlItemStockList.Update()
        upnlControls.Update()

        lblInstallationRemoval1.Visible = False
        dgInstallationRemoval1.Visible = False


        upnlInstallationRemoval.Update()
    End Sub
    'Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
    '    RemoveSession()
    '    Session("MiddleFrame") = ""
    '    Response.Redirect("Dashboard.aspx")
    'End Sub
    Private Sub btnCloseImageButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCloseImageButton.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetReferenceList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mReferences As ReferenceListAutoComplete
        mReferences = ReferenceListAutoComplete.GetReferenceList(prefixText)

        If count = 0 Then
            Return (From c As ReferenceListAutoComplete.ReferenceListAutoCompleteInfo In mReferences
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Reference, c.Reference())).ToArray
        Else
            Return (From c As ReferenceListAutoComplete.ReferenceListAutoCompleteInfo In mReferences
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Reference, c.Reference())).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As ItemListAutoComplete
        itemlist = ItemListAutoComplete.GetItemList(prefixText, IsSerialisedPartsList:=False, PartsFromBothInventoryMaintenance:=True)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function

    Private Sub dgInstallationRemoval1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgInstallationRemoval1.RowCommand
        Dim mCompStatusInfo As CompHistoryListInfo
        Select Case e.CommandName
            Case "RemoveRecord"
                mCompStatusInfo = mCompHistoryList(New Guid(e.CommandArgument.ToString))
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ComponentRemovalNew")) Then
                    Dim MaintDetail As String
                    MaintDetail = "INST. ON A/C REGN/ S/N. &amp; Assembly Info: " + mCompStatusInfo.RegNoModelSerialNo & " Part Info : " & mCompStatusInfo.PartDet.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Remove, "ComponentRemoval", User.Identity.Name & " is not Authorized User to remove " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("FromGrid") = "FromGrid"
                RemoveRecord(mCompStatusInfo)
            Case "InstallSelected"
                mCompStatusInfo = mCompHistoryList(New Guid(e.CommandArgument.ToString))
                If Not User.IsInRole("ComponentInstallationNew") Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                End If
                Session("FromGrid") = "FromGrid"
                InstallRecord(mCompStatusInfo)
        End Select
    End Sub
    Private Sub dgModelMonitorAMPRefStatusList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgModelMonitorAMPRefStatusList.PageIndexChanging
        dgModelMonitorAMPRefStatusList.PageIndex = e.NewPageIndex
        dgModelMonitorAMPRefStatusList.DataSource = mModelMonitorAMPRefStatusList
        dgModelMonitorAMPRefStatusList.DataBind()
        Session("mModelMonitorAMPRefStatusList") = mModelMonitorAMPRefStatusList
    End Sub

    Private Sub dgModelMonitorAMPRefStatusList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgModelMonitorAMPRefStatusList.RowCommand
        Select Case e.CommandName
            Case "ComplyRecord"
                ''Dim Index As Integer = CInt(e.CommandArgument) + dgModelMonitorAMPRefStatusList.PageSize * dgModelMonitorAMPRefStatusList.PageIndex
                Dim mID As Guid = New Guid(e.CommandArgument.ToString) ''mModelMonitorAMPRefStatusList(Index).ID
                Session("FromGrid") = "FromGrid"
                ComplyRecord(mID, mModelMonitorAMPRefStatusList(mID).ActivityTypeID)
            Case "HistoryRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                HistoryRecords(mID, mModelMonitorAMPRefStatusList(mID).ActivityTypeID)
        End Select
    End Sub

#End Region

End Class
