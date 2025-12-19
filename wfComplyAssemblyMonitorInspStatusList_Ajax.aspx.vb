'AJAX Conversion By Vikrant On 18-Mar-2015
Imports System.Linq
Public Class wfComplyAssemblyMonitorInspStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mMachineNameValueList As MachineNameValueList
    Private mTmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
    Private mrptDueReport As rptDueReport 'Added By Vikrant for faster processing
    Private DoneOn As String
    Private AircraftId As String
    Public mMachine As Machine
    Public mBoardInfo As AircraftInformationBoard.BoardInfo  'Added by Saylee on 22-May-2009

    Private mModelMonitorInspTypeList As ModelMonitorInspTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String 'Added by Saylee on 30-July-2009
    Dim mModuleList As ModuleList 'Added by Sachin on 17-10-2023
    'Added by Saylee on 09-Sep-2009
    Private mUpdateComplyHistoryAssemblyMonitorInspStatusList As UpdateComplyHistoryAssemblyMonitorInspStatusList

    'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance
    Dim ShowNotApplicable As Boolean = False

    'Added by Vikrant on 26-July-2011
    Dim EventLogID As Guid
    Public mAssemblyMonitorDetail As String
    Public mAssemblyMonitorDetailForMail As String
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mMonitorDesc As String
    Public mAssemblyDetails As String
    Dim IDForEventLog As Guid
    'Added By Prashant On 27-Nov-2014
    Dim mFileAttach As FileAttach
    Dim mAssemblylist As AssemblyList  'Added By Prahsnat 15-Jun-2015 
    Private AssemblyId As String
    Dim SkipOneTimeDoneMRecords As Boolean = False
    Dim RecordsToShow As Integer
    Dim IsReadOnly As Boolean 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Dim CodeFormNoDesc As String
    Public mIsSpareAssembly As Integer 'Added by Saylee on 26-Aug-2020 for All27072020
#End Region

#Region " Enum "
    Public Enum From
        NewRecord = 0
        EditRecord = 1
    End Enum
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mTmpComplyAssemblyMonitorInspStatusList = CType(Session("mTmpComplyAssemblyMonitorInspStatusList"), tmpComplyAssemblyMonitorInspStatusList)
        mrptDueReport = CType(Session("mrptDueReport"), rptDueReport) 'Added By Vikrant for faster processing
        DoneOn = Session("DoneOn")
        AircraftId = Session("AircraftId")
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009
        mModuleList = Session("mModuleList") 'Added by Sachin on 17-10-2023
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        ShowNotApplicable = CType(Session("ShowNotApplicable"), Boolean) 'Added by Saylee on 7th-Jan-2011
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AssemblyId = CType(Session("AssemblyId"), String)
        SkipOneTimeDoneMRecords = CType(Session("SkipOneTimeDoneMRecords"), Boolean)
        'RecordsToShow = CType(IIf(Session("RecordsToShow") Is Nothing, dgDueMonitoringList.PageSize, Session("RecordsToShow")), Integer)
        RecordsToShow = CType(Session("RecordsToShow"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        CodeFormNoDesc = Session("CodeFormNoDesc")
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added by Saylee on 26-Aug-2020 for All27072020
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mTmpComplyAssemblyMonitorInspStatusList")
        Session.Remove("mrptDueReport") 'Added By Vikrant for faster processing
        Session.Remove("RecordsToShow")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        'Session.Remove("mIsSpareAssembly") 'Added by Saylee on 26-Aug-2020 for All27072020
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyAssemblyMonitorInspStatusList_Ajax.aspx?SpareAssembly=" & Session("mIsSpareAssembly") Then
            Session.Remove("mTmpComplyAssemblyMonitorInspStatusList")
            Session.Remove("mrptDueReport") 'Added By Vikrant for faster processing
            Session.Remove("mMachineNameValueList")
            Session.Remove("DoneOn")
            Session.Remove("AircraftId")
            Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
            Session.Remove("mMachineMaintenance") 'Added by Saylee on 9th-Oct-2009
            Session.Remove("ShowNotApplicable") 'Added by Saylee on 7th-Oct-2010
            Session.Remove("mAssemblylist")
            Session.Remove("AssemblyId")
            Session.Remove("SkipOneTimeDoneMRecords")
            Session.Remove("RecordsToShow")
            Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            Session.Remove("CodeFormNoDesc")
            Session.Remove("mIsSpareAssembly") 'Added by Saylee on 26-Aug-2020 for All27072020
        End If
    End Sub
    Private Sub ControlVisibility()
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            btnPrint.Enabled = (mrptDueReport.Count > 0)
            btnPrintTop.Enabled = (mrptDueReport.Count > 0)
            'End
        Else 'existing flow for spare assembly keep as it is
            btnPrint.Enabled = (mTmpComplyAssemblyMonitorInspStatusList.Count > 0)
            btnPrintTop.Enabled = (mTmpComplyAssemblyMonitorInspStatusList.Count > 0)
        End If
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
        dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)

        'Added by Saylee on 26-Aug-2020 for All27072020
        If mIsSpareAssembly = 1 Then
            pllblAircraft.Visible = False
            plAircraft.Visible = False
        Else
            pllblAircraft.Visible = True
            plAircraft.Visible = True
        End If
        If Session("mIsSpareAssembly") = 1 Or AppSettings("ShowNewDiscrepancyFlow") = "True" Then
            btnAddNew.Visible = False
            btnAddNewTop.Visible = False
        End If

        EnableLink()
    End Sub
    Private Sub EnableLink()
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    lnkLoadMore.Visible = True
        '    lnkLoadMoreTop.Visible = True
        'Else
        '    lnkLoadMore.Visible = False
        '    lnkLoadMoreTop.Visible = False
        'End If
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            If RecordsToShow < mrptDueReport.Count Then
                lnkLoadMore.Enabled = True
                lnkLoadMoreTop.Enabled = True
            Else
                lnkLoadMore.Enabled = False
                lnkLoadMoreTop.Enabled = False
            End If
            'End
        Else 'existing flow for spare assembly keep as it is
            If RecordsToShow < mTmpComplyAssemblyMonitorInspStatusList.Count Then
                lnkLoadMore.Enabled = True
                lnkLoadMoreTop.Enabled = True
            Else
                lnkLoadMore.Enabled = False
                lnkLoadMoreTop.Enabled = False
            End If
        End If
    End Sub
    Private Sub ComplyRecord(ByVal Index As Int32)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mMachine = Machine.GetMachine(mrptDueReport.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mrptDueReport.Item(Index).AssemblyMonitorInspStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If
        If (mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And (mPrevAssemblyMonitorInspStatus.IsCompleted Or mPrevAssemblyMonitorInspStatus.FetchRecordCount(mPrevAssemblyMonitorInspStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, txtDate.Text, mrptDueReport(Index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, txtDate.Text, mTmpComplyAssemblyMonitorInspStatusList(Index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
            End If
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("From") = 0 'New record
            ''
            'Dim mMachine As Machine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            Dim mAssemblyStatus As AssemblyStatus
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
            End If
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            RemoveSession()

            'Added by Saylee on 22-May-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            'Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorInspStatus.ID) 'Sort = 1 : Installation
            'Session("mFileAttach") = mFileAttach
            'End
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'Added by Vikrant on 26-July-2011
                mAircraft = mrptDueReport(Index).RegNo
                mMonitorInfo = mrptDueReport(Index).Type
                mMonitorType = "" 'mrptDueReport(Index).MonitorType
                mMonitorDesc = mrptDueReport(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date : " & mrptDueReport(Index).DoneOnDate.ToString & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                MarkLog(Util.Action.Comply, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mrptDueReport.Item(mrptDueReport.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)
                'End
            Else 'existing flow for spare assembly keep as it is
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
                'Added by Vikrant on 26-July-2011
                mAircraft = mTmpComplyAssemblyMonitorInspStatusList(Index).MachineInfo
                mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(Index).ModelMonitorInspInfo
                mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(Index).MonitorType
                mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date : " & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnValueFormatted
                MarkLog(Util.Action.Comply, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mMachine = Machine.GetMachine(mrptDueReport.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mrptDueReport.Item(Index).AssemblyMonitorInspStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If
        If mPrevAssemblyMonitorInspStatus.IsMaster And mPrevAssemblyMonitorInspStatus.IsApplicable And chkApplicable.Checked = False Then
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf ((mPrevAssemblyMonitorInspStatus.IsMaster) And (Not mPrevAssemblyMonitorInspStatus.IsApplicable) And (chkApplicable.Checked = True)) Or (mPrevAssemblyMonitorInspStatus.IsMaster And mPrevAssemblyMonitorInspStatus.IsApplicable And chkApplicable.Checked = False) Then 'Editing NOT APPLICABLE Master records
            Session("mAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("From") = 1 'Edit record
            ''
            'Dim mMachine As Machine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
            End If
            'Added By Vikrant On 25-Nov-2014
            'If mPrevAssemblyMonitorInspStatus.IsAttachmentAdded Then
            '    Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevAssemblyMonitorInspStatus.ID) 'Sort = 1 - Installation
            '    Session("mFileAttach") = mFileAttach
            'Else
            '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevAssemblyMonitorInspStatus.ID)
            '    Session("mFileAttach") = mFileAttach
            'End If
            'End

            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'End
            Else 'existing flow for spare assembly keep as it is
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
            End If
            RemoveSession()

            'Commented And Added by Saylee on 3-Dec-2019 , as to open Master form for NOT Appilcable Records and not COMPLY form
            ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorInspStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************

            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevAssemblyMonitorInspStatus.IsMaster = False) And (mPrevAssemblyMonitorInspStatus.IsCompleted = False) And mPrevAssemblyMonitorInspStatus.IsDone = False) Then
            Dim mModelMonitorInsp As ModelMonitorInsp
            Dim mAssemblyStatus As AssemblyStatus
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mrptDueReport.Item(Index).AssemblyMonitorInspStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
                mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mrptDueReport.Item(Index).StatusMasterID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
                mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelMonitorInspID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
            End If
            Session("mModelMonitorInsp") = mModelMonitorInsp
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorInspStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************
        Else
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType, True)
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("From") = 1 'Edit record
            ''
            'Dim mMachine As Machine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
            End If
            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            'Added By Vikrant On 25-Nov-2014
            'If mAssemblyMonitorInspStatus.IsAttachmentAdded Then
            '    Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID) 'Sort = 1 - Installation
            '    Session("mFileAttach") = mFileAttach
            'Else
            '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorInspStatus.ID)
            '    Session("mFileAttach") = mFileAttach
            'End If
            'End
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'Added by Vikrant on 26-July-2011
                mMonitorInfo = mrptDueReport(Index).Type
                mMonitorType = "" 'mrptDueReport(Index).MonitorType
                mMonitorDesc = mrptDueReport(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " + cmbAircraftList.SelectedItem.Text + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date :" & mrptDueReport(Index).DoneOnDate.ToString & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                MarkLog(Util.Action.Edit, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mrptDueReport.Item(mrptDueReport.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)
                'End
            Else 'existing flow for spare assembly keep as it is
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
                'Added by Vikrant on 26-July-2011
                mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(Index).ModelMonitorInspInfo
                mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(Index).MonitorType
                mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " + cmbAircraftList.SelectedItem.Text + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date :" & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnValueFormatted
                MarkLog(Util.Action.Edit, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)
            End If
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub HistoryRecords(ByVal Index As Int32) 'Added by Saylee on 09-Sep-2009
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mMachine = Machine.GetMachine(mrptDueReport.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mrptDueReport.Item(Index).AssemblyMonitorInspStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If
        'If mPrevAssemblyMonitorInspStatus.IsMaster Then
        '    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Inspection Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyAssemblyMonitorInspStatusList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mrptDueReport(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            'End
        Else 'existing flow for spare assembly keep as it is
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
        End If
        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
            Session("ATAChapter") = mrptDueReport.Item(Index).ATAChapter.ToString
            Session("Description") = mrptDueReport.Item(Index).Description
            Session("ModelSerialNo") = mrptDueReport.Item(Index).ModelSerialNo
            mMonitorInfo = mrptDueReport.Item(mrptDueReport.CurrentIndex).Type
            mMonitorType = "" 'mrptDueReport.Item(mrptDueReport.CurrentIndex).MonitorType
            mMonitorDesc = mrptDueReport.Item(mrptDueReport.CurrentIndex).Description
            'End
        Else 'existing flow for spare assembly keep as it is
            Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
            Session("ATA") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString
            Session("Description") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
            Session("ModelSerialNo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo
            mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).ModelMonitorInspInfo
            mMonitorType = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).MonitorType
            mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).Description
        End If
        mAircraft = cmbAircraftList.SelectedItem.Text
        mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc
        mUpdateComplyHistoryAssemblyMonitorInspStatusList = UpdateComplyHistoryAssemblyMonitorInspStatusList.GetComplyHistoryAssemblyMonitorInspStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorInspStatus.ModelMonitorInspID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList
        MarkLog(Util.Action.View, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfUpdateComplyHistoryAssemblyMonitorInspStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionHistoryWindow", "OpenInspectionHistoryWindow();", True)
        'End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            'Revise Activity
            If chkApplicable.Checked And mrptDueReport(Index).ModelActivityCount > 1 Then 'Revise Activity
                MSGBoxCtrl.Show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            End If
            mrptDueReport.CurrentIndex = Index
            Session("mrptDueReport") = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            'Revise Activity
            If chkApplicable.Checked And mTmpComplyAssemblyMonitorInspStatusList(Index).ModelActivityCount > 1 Then 'Revise Activity
                MSGBoxCtrl.Show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            End If

            mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex = Index
            Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        End If
    End Sub

    Private Sub ReviseRecord(ByVal Index As Int32)
        '  Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mMachine = Machine.GetMachine(mrptDueReport.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mrptDueReport.Item(Index).AssemblyMonitorInspStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
            mPrevAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If

        Session("mAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mrptDueReport(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            'End
        Else 'existing flow for spare assembly keep as it is
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
        End If
        'Added By Vikrant On 25-Nov-2014
        'If mPrevAssemblyMonitorInspStatus.IsAttachmentAdded Then
        '    Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevAssemblyMonitorInspStatus.ID) 'Sort = 1 - Installation
        '    Session("mFileAttach") = mFileAttach
        'Else
        '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevAssemblyMonitorInspStatus.ID)
        '    Session("mFileAttach") = mFileAttach
        'End If
        'End

        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
            'End
        Else 'existing flow for spare assembly keep as it is
            Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
        End If


        Session("From") = 1 'Edit record
        Session("RevisedFromListPage") = "True"
        Session("NewPage") = "True"
    End Sub
    Private Sub MessageBoxResult()
        Dim msgCount As Integer = 0
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            ''Added by Vikrant on 26-July-2011
                            'mAircraft = cmbAircraftList.SelectedItem.Text

                            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                                IDForEventLog = mrptDueReport(mrptDueReport.CurrentIndex).ID
                                mMonitorInfo = mrptDueReport.Item(mrptDueReport.CurrentIndex).TypeDet
                                mMonitorType = "" ' mrptDueReport.Item(mrptDueReport.CurrentIndex).MonitorType
                                mMonitorDesc = mrptDueReport.Item(mrptDueReport.CurrentIndex).Description
                                mAircraft = mrptDueReport.Item(mrptDueReport.CurrentIndex).RegNo
                                mAssemblyDetails = mrptDueReport.Item(mrptDueReport.CurrentIndex).ModelName + "-" + mrptDueReport.Item(mrptDueReport.CurrentIndex).SerialNo + (IIf(mrptDueReport.Item(mrptDueReport.CurrentIndex).Position <> "", " (" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Position + ")", ""))
                                'End
                                mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc
                                mAssemblyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mMonitorDesc
                                'Added by Saylee on 28-May-2009
                                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mrptDueReport.CurrentItem.ID)
                                '********************************
                                If mrptDueReport(mrptDueReport.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(mrptDueReport.CurrentIndex).ID)
                                End If

                                'Added by Saylee on 9th-Oct-2009
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mrptDueReport.CurrentItem.ID, 6)
                                '=============================

                                AssemblyMonitorInspStatus.DeleteAssemblyMonitorInspStatus(mrptDueReport.CurrentItem.ID)
                                'End
                            Else 'existing flow for spare assembly keep as it is
                                IDForEventLog = mTmpComplyAssemblyMonitorInspStatusList(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID
                                mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).ModelMonitorInspInfo
                                mMonitorType = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).MonitorType
                                mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).Description
                                mAircraft = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).MachineInfo
                                mAssemblyDetails = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyInfo
                                mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc

                                mAssemblyMonitorDetailForMail = "Aircraft : " + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> Monitor Info. : " + mMonitorInfo + "<br/> Monitor Type : " + mMonitorType + "<br/> Description : " + mMonitorDesc

                                'Added by Saylee on 28-May-2009
                                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.AssemblyMonitorInspStatusID)
                                '********************************
                                If mTmpComplyAssemblyMonitorInspStatusList(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorInspStatusList(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID)
                                End If
                                'Added by Saylee on 9th-Oct-2009
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.AssemblyMonitorInspStatusID, 6)
                                '=============================
                                AssemblyMonitorInspStatus.DeleteAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.AssemblyMonitorInspStatusID)
                            End If

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
                            If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                                'Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                                If AppSettings("LinkMaintenance") = "True" Then
                                    If LinkMaintenanceList.GetLinkMaintenanceList(mrptDueReport.CurrentItem.statusMasterID.ToString).Count > 0 Then
                                        MSGBoxCtrl.Show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                        Exit Sub
                                    End If
                                End If
                                'End
                                'End
                            Else 'existing flow for spare assembly keep as it is
                                'Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                                If AppSettings("LinkMaintenance") = "True" Then
                                    If LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.ModelMonitorInspID.ToString).Count > 0 Then
                                        MSGBoxCtrl.Show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                        Exit Sub
                                    End If
                                End If
                                'End
                            End If

                            SendMail(mAssemblyMonitorDetailForMail)
                            DataFieldBind()
                            SetPage()
                            SetGrid()
                            ControlVisibility()
                            SetRights()
                            upnlgrid.Update()
                            upnlActionBtn.Update()
                            upnlActionBtnTop.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "AssemblyInspections", "Can't delete :" & mAssemblyMonitorDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.[New], "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mModelMonitorInsp As ModelMonitorInsp
                        Dim ID As Guid = Guid.NewGuid
                        'Revise Activity New

                        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = Session("mAssemblyMonitorInspStatus")
                        mMachine = Session("mMachine")
                        Dim mModelMonitorInspList As ModelMonitorInspList
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, GetRecordsByPrevRefID:=True, PrevRefID:=mAssemblyMonitorInspStatus.ModelMonitorInsp.PrevRefID.ToString)

                        If mModelMonitorInspList.Count > 1 Then
                            For i As Integer = mModelMonitorInspList.Count - 1 To 0 Step -1
                                If mModelMonitorInspList(i).ID.Equals(mAssemblyMonitorInspStatus.ModelMonitorInsp.ID) Then
                                    Exit For
                                Else
                                    Session("ModelIDFromModelCreation") = mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID
                                    Session("ModelNameFromModelCreation") = mAssemblyMonitorInspStatus.ModelMonitorInsp.Model.Name
                                    Session("mModelMonitorInspList") = mModelMonitorInspList
                                    Session("ModelMonitorInspIDToBeLinked") = mModelMonitorInspList(i).ID.ToString
                                    Session("ModelMonitorInspPrevRefIDToBeLinked") = mModelMonitorInspList(i).PrevRefID.ToString
                                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                                    Session("mPrevAssemblyMonitorInspStatusForRevise") = mAssemblyMonitorInspStatus
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfModelMonitorInspList_Ajax.aspx?BackPage=Index.aspx');", True)
                                    Exit Sub
                                End If
                            Next
                        End If
                        'END
                        mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(mAssemblyMonitorInspStatus.ModelMonitorInsp, mMachine.HourType)
                        Session("mModelMonitorInsp") = mModelMonitorInsp
                        RemoveSession()
                        mModelMonitorInsp.BeginEdit()
                        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                        Session("mPrevAssemblyMonitorInspStatusForRevise") = mAssemblyMonitorInspStatus
                        Session("IsLinkedActivitySelected") = True 'Revise Activity New
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow();", True)
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    If MSGBoxCtrl.Sender = "LinkMaintenance" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetPage()
                        SetGrid()
                        ControlVisibility()
                        SetRights()
                        upnlgrid.Update()
                        upnlActionBtn.Update()
                        upnlActionBtnTop.Update()
                    End If
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
    Private Sub FindNow()
        dgDueMonitoringList.PageIndex = 0
        Session("DoneOn") = txtDate.Text
        Session("AircraftId") = cmbAircraftList.SelectedValue
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue
        Session("ShowNotApplicable") = chkApplicable.Checked  'Added by Saylee on 7-Jan-2011
        Session("SkipOneTimeDoneMRecords") = IIf(chkOneTimeMasterRecords.Checked, True, False)
        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
        Session("CodeFormNoDesc") = Trim(txtCodeFormNo.Text)


        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, "", cmbAircraftAssembly.SelectedValue.ToString,
                                             2, CInt(IIf(cmbMonitorType.SelectedIndex > 0, cmbMonitorType.SelectedValue, 0)), chkApplicable.Checked,
                                             chkOneTimeMasterRecords.Checked, txtCodeFormNo.Text.Trim)
            mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)

            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            Session("mrptDueReport") = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            'Commented & Added by Saylee on 26-Aug-2020 for All27072020
            '''mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(txtDate.Text, cmbAircraftList.SelectedValue, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , cmbMonitorType.SelectedValue, , , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text))
            mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(txtDate.Text, IIf(mIsSpareAssembly = 1, Guid.Empty, cmbAircraftList.SelectedValue).ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , cmbMonitorType.SelectedValue, , , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareAssembly:=mIsSpareAssembly, AssemblyID:=cmbAircraftAssembly.SelectedValue)
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            End If
            Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
            'End
        End If

        dgDueMonitoringList.DataBind()

        'Added by Saylee on 26-Aug-2020 for All27072020
        If (mIsSpareAssembly = 1) Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New DataSet()
            da.Fill(ds, mAssemblylist)
            Dim dv As DataView = ds.Tables(0).DefaultView
            dv.RowFilter = "IsSpareAssembly='True'"
            For Each dr As DataRowView In dv
                For Each item As ListItem In cmbAircraftAssembly.Items
                    If dr("ID").ToString() = item.Value.ToString() Then
                        item.Attributes.Add("style", "background-color:#ffbf00;color:black;font-weight:bold;")
                    End If
                Next
            Next
        End If

        SetGrid()
        ControlVisibility()
    End Sub
    Private Sub SetPage()
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            If RecordsToShow < mrptDueReport.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Assembly Inspection Status as per selected criteria : " & RecordsToShow.ToString & " of " & mrptDueReport.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Assembly Inspection Status as per selected criteria : " & mrptDueReport.Count & " Record(s) found."
            End If
            lbltitle.Text = "List of Assembly Inspection Status"
            'End
        Else 'existing flow for spare assembly keep as it is
            If RecordsToShow < mTmpComplyAssemblyMonitorInspStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Stock/Removed Assembly Inspection Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyAssemblyMonitorInspStatusList.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Stock/Removed Assembly Inspection Status as per selected criteria : " & mTmpComplyAssemblyMonitorInspStatusList.Count & " Record(s) found."
            End If
            lbltitle.Text = "List of Stock/Removed Assembly Inspection Status"
        End If

    End Sub
    Private Sub SetRights() 'Added By Prashant On 31-Mar-2011
        If (User.IsInRole("MachineAssemblyInspectionNew")) = False Then
            btnAddNew.Enabled = False
            btnAddNew.ToolTip = "You are not authorized user"
            btnAddNewTop.Enabled = False
            btnAddNewTop.ToolTip = "You are not authorized user"
        End If
    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        Dim c As Boolean

        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft

        If mIsSpareAssembly = 1 Then
            IsReadOnly = False
        End If

        For j As Integer = 0 To dgDueMonitoringList.Rows.Count - 1
            B = CType(Me.dgDueMonitoringList.Rows(j).Cells(24).Text, Boolean)
            c = CType(Me.dgDueMonitoringList.Rows(j).Cells(26).Text, Boolean)
            If B = True Then
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = False
            End If

            'Commented by Saylee on 27-Jul-2023, as view image button added
            'If c = False Then
            '    dgDueMonitoringList.Rows(j).Cells(25).Enabled = False
            'End If

            'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgDueMonitoringList.Rows(j).Cells(20).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(27).Enabled = False
                btnAddNewTop.Enabled = False
                btnAddNew.Enabled = False
                lblReadOnly.Visible = True
            Else
                dgDueMonitoringList.Rows(j).Cells(20).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(27).Enabled = True 'Revise
                btnAddNewTop.Enabled = True
                btnAddNew.Enabled = True
                lblReadOnly.Visible = False
            End If
            '*************************
            'Dim MonitorTypeID As Integer = CType(Me.dgDueMonitoringList.Rows(j).Cells(29).Text, Integer) 'Revise 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
            'dgDueMonitoringList.Rows(j).Cells(27).Enabled = Not (MonitorTypeID = 1 Or MonitorTypeID = 4) And dgDueMonitoringList.Rows(j).Cells(13).Text <> "" 'Revise 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
        Next

        'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        'Disable AddNew buttons if Aircraft is ReadOnly
        If IsReadOnly = True Then
            btnAddNewTop.Enabled = False
            btnAddNew.Enabled = False
            lblReadOnly.Visible = True
        Else
            btnAddNewTop.Enabled = True
            btnAddNew.Enabled = True
            lblReadOnly.Visible = False
        End If
        '*************************
    End Sub
    Private Sub GridBind()
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            'End
        Else 'existing flow for spare assembly keep as it is
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            End If
        End If

        dgDueMonitoringList.DataBind()
        SetGrid()
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
        dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
    End Sub
    Private Sub SetMachineMaintenanceObject(mMachineMaintenance As MachineMaintenance, CurrAssemblyMonitorInsp As AssemblyMonitorInspStatus)
        With mMachineMaintenance
            mMachine = Session("mMachine")
            Dim mLog As Log
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(mMachineMaintenance.Date, mMachineMaintenance.MachineID, CurrAssemblyMonitorInsp.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(mMachineMaintenance.Date, mMachine.ID)
                    If mMaxLogNo.Count <> 0 Then
                        .LogNo = mMaxLogNo(0).LogNo
                        .LogID = mMaxLogNo(0).LogId
                        .LogPageNo = mMaxLogNo(0).LogPageNo
                    End If
                End If
                'End
            End If

        End With
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
    End Sub


    Public Sub SendMail(mAssemblyMonitorDetailForMail)
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'If AppSettings("MailsRequire") = "True" Then
        If mModuleList.Item("AssemblyInspections").MailsRequire = True Then
            'If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
            '    'Do nothing
            '    Exit Sub
            'End If
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Task Details :  <br/> <br/>  " & mAssemblyMonitorDetailForMail & " <br/> <b> Deleted by User:</b> " + User.Identity.Name + "<b> on: </b>" + New SmartDate(Today.Date).FormattedText + "</font></P> ")
            str = str + ("</body></html>")
            'SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Assembly Inspections Deleted", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), Info:=str, ToMailID:=mModuleList.Item("Order").SendToMailID, Remark:=Session("SendMailRemark"), ReportGenratedBy:=Session("ReportGenratedBy"))

            SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Task Deleted", Info:=str, ToMailID:=mModuleList.Item("AssemblyInspections").SendToMailID, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind(Optional ByVal SkipOneTimeDoneMasterRecords As Boolean = False)
        If Not IsDate(DoneOn) Then
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DoneOn = Today.Date.ToString(AppSettings("DateFormat")) 'Added By Rahul on 29-Apr-2009
        Else
            txtDate.Text = CDate(DoneOn).ToString(AppSettings("DateFormat"))
        End If
        txtDate.DataBind()

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True)
        cmbAircraftList.DataSource = mMachineNameValueList
        If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then
            'do nothing
        Else
            cmbAircraftList.SelectedValue = AircraftId
        End If
        cmbAircraftList.DataBind()   'Added Code
        Session("AircraftId") = cmbAircraftList.SelectedValue
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly
        Session("mMachineNameValueList") = mMachineNameValueList

        'Added By Prashant 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraftList.SelectedValue, txtDate.Text.ToString, "(All)", True, IsForSpareAssembly:=mIsSpareAssembly) '  ' mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27072020
        cmbAircraftAssembly.DataSource = mAssemblylist
        If (Session("AssemblyId") = Guid.Empty.ToString Or IsNothing(Session("AssemblyId"))) Then
            'Do nothing
        Else
            cmbAircraftAssembly.SelectedValue = CType(Session("AssemblyId"), String)
        End If
        cmbAircraftAssembly.DataBind()
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue
        Session("mAssemblyList") = mAssemblylist
        chkOneTimeMasterRecords.Checked = SkipOneTimeDoneMRecords
        txtCodeFormNo.Text = CodeFormNoDesc
        '-----------------------------------------
        chkApplicable.Checked = ShowNotApplicable 'Added by Saylee on 7-Jan-2011
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mrptDueReport = rptDueReport.GetList(DoneOn, cmbAircraftList.SelectedItem.ToString, , True, "", cmbAircraftAssembly.SelectedValue.ToString, 2, CInt(MonitorTypeID), chkApplicable.Checked, chkOneTimeMasterRecords.Checked, CodeFormNoDesc)
            mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            Session("mrptDueReport") = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            ' mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27072020
            mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(DoneOn, IIf(mIsSpareAssembly = 1, Guid.Empty, cmbAircraftList.SelectedValue).ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , CType(MonitorTypeID, Integer), , , ShowNotApplicable, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=CodeFormNoDesc, IsSpareAssembly:=CBool(mIsSpareAssembly), AssemblyID:=cmbAircraftAssembly.SelectedValue)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            End If
            Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        End If
        dgDueMonitoringList.DataBind()

        'Added by Saylee on 30-July-2009
        mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(All)")
        cmbMonitorType.DataSource = mModelMonitorInspTypeList
        If IsNothing(MonitorTypeID) Or MonitorTypeID = "" Then
            'Do nothing
        Else
            cmbMonitorType.SelectedValue = MonitorTypeID
        End If
        cmbMonitorType.DataBind()
        Session("MonitorTypeID") = MonitorTypeID 'Added by Saylee on 30-July-2009
        chkApplicable.Checked = IIf(ShowNotApplicable, True, False)
        'Added by Saylee on 26-Aug-2020 for All27072020
        If (mIsSpareAssembly = 1) Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New DataSet()
            da.Fill(ds, mAssemblylist)
            Dim dv As DataView = ds.Tables(0).DefaultView
            dv.RowFilter = "IsSpareAssembly='True'"
            For Each dr As DataRowView In dv
                For Each item As ListItem In cmbAircraftAssembly.Items
                    If dr("ID").ToString() = item.Value.ToString() Then
                        item.Attributes.Add("style", "background-color:#ffbf00;color:black;font-weight:bold;")
                    End If
                Next
            Next
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack And Session("sender") = "" Then

            ' 'Added by Saylee on 26-Aug-2020 for All27082020
            mIsSpareAssembly = Request.QueryString("SpareAssembly")
            Session("mIsSpareAssembly") = mIsSpareAssembly
            '************************


            cmbAircraftList.Focus()
            Session("MiddleFrame") = "wfComplyAssemblyMonitorInspStatusList_Ajax.aspx?SpareAssembly=" & mIsSpareAssembly  ' 'mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27082020
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            DataFieldBind(True)
            SetPage()
            ControlVisibility()
            SetRights()
            SetGrid()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        MarkLog(Util.Action.Close, "AssemblyInspections", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session.Remove("From")
        Session("MiddleFrame") = ""
        Session.Remove("DoneOn")
        Session.Remove("AircraftId")
        Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
        Session.Remove("AssemblyId")
        Session.Remove("SkipOneTimeDoneMRecords")
        Session.Remove("ATAChapter")
        Session.Remove("CodeFormNoDesc")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraftList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged 'Added By Prahsnat 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraftList.SelectedValue, txtDate.Text.ToString, "(All)", True)
        cmbAircraftAssembly.DataSource = mAssemblylist
        cmbAircraftAssembly.DataBind()
        Session("mAssemblyList") = mAssemblylist

        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly

        upnlSearchCriteria.Update()
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub cmbAircraftAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraftAssembly.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub chkApplicable_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkApplicable.CheckedChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub chkOneTimeMasterRecords_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkOneTimeMasterRecords.CheckedChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub txtCodeFormNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtCodeFormNo.TextChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        RecordsToShow = dgDueMonitoringList.PageSize
        Session("RecordsToShow") = RecordsToShow
        FindNow()
        SetPage()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub dgDueMonitoringList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueMonitoringList.RowCommand
        'Dim Index As Int16
        'Dim mID As New Guid(e.Item.Cells(0).Text)   'Added by Vikrant on 26-July-2011
        Select Case e.CommandName
            Case "Comply"

                If Not User.IsInRole("AssemblyInspectionsNew") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
                ComplyRecord(CInt(e.CommandArgument))
            Case "EditRec"
                If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    mAircraft = cmbAircraftList.SelectedItem.Text
                    If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                        mMonitorType = "" 'mrptDueReport(CInt(e.CommandArgument)).MonitorType
                        mMonitorInfo = mrptDueReport(CInt(e.CommandArgument)).Code
                        mMonitorDesc = mrptDueReport(CInt(e.CommandArgument)).Code_Desc
                        'End
                    Else 'existing flow for spare assembly keep as it is
                        mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).MonitorType
                        mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).ModelMonitorInspCode
                        mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).Code_Desc
                    End If
                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                    MarkLog(Util.Action.Edit, "AssemblyInspections", User.Identity.Name & " is not Authorized User to edit " & mAssemblyMonitorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
                EditRecord(CInt(e.CommandArgument))
                'End
            Case "DeleteRec"
                If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                    mMonitorType = "" 'mrptDueReport(CInt(e.CommandArgument)).MonitorType
                    mMonitorInfo = mrptDueReport(CInt(e.CommandArgument)).Code
                    mMonitorDesc = mrptDueReport(CInt(e.CommandArgument)).Code_Desc
                    'End
                Else 'existing flow for spare assembly keep as it is
                    mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).MonitorType
                    mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).ModelMonitorInspCode
                    mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).Code_Desc
                End If
                mAircraft = cmbAircraftList.SelectedItem.Text

                If (Not User.IsInRole("AssemblyInspectionsDelete")) Then
                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                    MarkLog(Util.Action.Delete, "AssemblyInspections", User.Identity.Name & " is not Authorized User to delete " & mAssemblyMonitorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
                DeleteRecord(CInt(e.CommandArgument))
            Case "History"   'Added by Saylee on 09-Sep-2009

                If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
                HistoryRecords(CInt(e.CommandArgument))
            Case "ViewRec"
                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(CInt(e.CommandArgument)).ID)
                    'End
                Else 'existing flow for spare assembly keep as it is
                    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).ID)
                End If
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    If mFileAttach.FileName <> "" Then
                        StrName = mFileAttach.FileName
                    Else
                        StrName = StrName & mFileAttach.Extension
                    End If
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName '& mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName) '& mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
            Case "Revise" 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
                If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    mAircraft = cmbAircraftList.SelectedItem.Text
                    If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
                        mMonitorType = "" 'mrptDueReport(CInt(e.CommandArgument)).MonitorType
                        mMonitorInfo = mrptDueReport(CInt(e.CommandArgument)).Code
                        mMonitorDesc = mrptDueReport(CInt(e.CommandArgument)).Code_Desc
                        'End
                    Else 'existing flow for spare assembly keep as it is
                        mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).MonitorType
                        mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).ModelMonitorInspCode
                        mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).Code_Desc
                    End If
                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                    MarkLog(Util.Action.Edit, "AssemblyInspections", User.Identity.Name & " is not Authorized User to edit " & mAssemblyMonitorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                'Added by Harsh on 5th Jun 2024 for FLYPAL-1685.
                RecordsToShow = dgDueMonitoringList.PageSize
                Session("RecordsToShow") = RecordsToShow
                Session("mrptDueReport") = mrptDueReport

                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
                ReviseRecord(CInt(e.CommandArgument))
                MSGBoxCtrl.Show("Alert!", "You are about to Revise Model Activity.After revision of model activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
        End Select
    End Sub
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click, btnAddNew.Click
        If IsValid Then
            Session("AircraftIdForInsp") = cmbAircraftList.SelectedValue.ToString
            'Added by Vikrant on 26-July-2011
            MarkLog(Util.Action.[New], "AssemblyInspections", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorInspStatusListNew.aspx?BackPage=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAssemblyInspectionListNewWindow", "OpenAssemblyInspectionListNewWindow();", True)
            Session("NewPage") = "True"
        End If
    End Sub
    Private Sub dgDueMonitoringList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueMonitoringList.Sorting
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mrptDueReport.Sort(IIf(e.SortExpression = "RemainingTimeForCompliancePage", "RemainingValueForSorting", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            Session("mrptDueReport") = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            mTmpComplyAssemblyMonitorInspStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            End If
            Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        End If
        dgDueMonitoringList.DataBind()
        SetGrid()
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
        dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
    End Sub
    Private Sub hdnBtnInspectionHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnInspectionHistory.Click
        FindNow()
        SetPage()
        upnlgrid.Update()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub lnkLoadMore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLoadMore.Click, lnkLoadMoreTop.Click
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            RecordsToShow = mrptDueReport.Count
            dgDueMonitoringList.DataSource = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            RecordsToShow = mTmpComplyAssemblyMonitorInspStatusList.Count
            dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
        End If
        Session("RecordsToShow") = RecordsToShow
        'Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        dgDueMonitoringList.DataBind()
        lnkLoadMore.Enabled = False
        lnkLoadMoreTop.Enabled = False
        SetPage()
        SetGrid()
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
        dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
    End Sub
    Protected Sub dgDueMonitoringList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgDueMonitoringList.Columns(i).HeaderText
            Next
        End If
    End Sub
    'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
    'Modified by Harsh on 5th Jun 2024 for FLYPAL-1685
    Private Sub hdnBtnModelInspMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelInspMaster.Click

        'Revise Activity
        If Not Session("mPrevAssemblyMonitorInspStatusForRevise") Is Nothing Then
            Dim mPrevAssemblyMonitorInspStatusForRevise As AssemblyMonitorInspStatus
            mPrevAssemblyMonitorInspStatusForRevise = Session("mPrevAssemblyMonitorInspStatusForRevise")
            mPrevAssemblyMonitorInspStatusForRevise.IsApplicable = False
            mPrevAssemblyMonitorInspStatusForRevise.Save()
            Session.Remove("mPrevAssemblyMonitorInspStatusForRevise")
            Session.Remove("RevisedFromListPage")


            Dim mCurrAssemblyMonitorInspStatusForRevise As AssemblyMonitorInspStatus
            mCurrAssemblyMonitorInspStatusForRevise = Session("mAssemblyMonitorInspStatus")
            'If mPrevAssemblyMonitorInspStatusForRevise.AsOnDateFormatted.ToString = "" Then
            '    mCurrAssemblyMonitorInspStatusForRevise.AsOnDate = System.DBNull.Value
            'Else
            '    mCurrAssemblyMonitorInspStatusForRevise.AsOnDate = mPrevAssemblyMonitorInspStatusForRevise.AsOnDateFormatted.ToString
            'End If

            If mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString = "" Then
                mCurrAssemblyMonitorInspStatusForRevise.AsOnDate = mPrevAssemblyMonitorInspStatusForRevise.AsOnDateFormatted.ToString
            Else
                mCurrAssemblyMonitorInspStatusForRevise.AsOnDate = mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString
            End If

            For i As Integer = 0 To mPrevAssemblyMonitorInspStatusForRevise.AssemblyMonitorInspStatusPeriods.Count - 1
                Dim PeriodID = mPrevAssemblyMonitorInspStatusForRevise.AssemblyMonitorInspStatusPeriods(i).PeriodID
                If mCurrAssemblyMonitorInspStatusForRevise.AssemblyMonitorInspStatusPeriods.Contains(PeriodID, "") Then
                    mCurrAssemblyMonitorInspStatusForRevise.AssemblyMonitorInspStatusPeriods.Item(PeriodID, "").DoneOnValue = mPrevAssemblyMonitorInspStatusForRevise.AssemblyMonitorInspStatusPeriods(i).DoneOnValue
                End If
            Next

            mMachine = Session("mMachine")
            Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.NewMachineMaintenance(mMachine.ID, 6, mCurrAssemblyMonitorInspStatusForRevise.AsOnDate, mCurrAssemblyMonitorInspStatusForRevise.ID, Guid.Empty, 0, 0, mCurrAssemblyMonitorInspStatusForRevise.AssemblyStatusID)
            mMachineMaintenance.MaintenanceID = mCurrAssemblyMonitorInspStatusForRevise.ID

            mCurrAssemblyMonitorInspStatusForRevise.IsMaster = False
            mCurrAssemblyMonitorInspStatusForRevise.Save()
            SetMachineMaintenanceObject(mMachineMaintenance, mCurrAssemblyMonitorInspStatusForRevise)
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            Session.Remove("mAssemblyMonitorInspStatus")

        End If
        'End

        FindNow()
        SetPage()
        upnlgrid.Update()

    End Sub
#End Region

#Region " Report "
    'Created By :- Rajnish , Date -22/09/2006
#Region " Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String = String.Empty
    Private SearchStr2 As String = String.Empty
    Private SearchStr3 As String = String.Empty
    Private SearchStr4 As String = String.Empty
#End Region

#Region " Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If (Not User.IsInRole("AssemblyInspectionsPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            dgDueMonitoringList.DataSource = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
        End If

        dgDueMonitoringList.DataBind()

        SetGrid()
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
        dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)

        Dim Rpt As New crListComplyAssemblyMonitorStatus
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "Date :" + "  " + txtDate.Text
        SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        SearchStr3 = ""
        SearchStr4 = "Aircraft :" + "  " + cmbAircraftList.SelectedItem.Text

        ReportDetails.Add(New rptStatus(, 1, ,
                          , , , dgDueMonitoringList.Columns.Item(0).HeaderText, , dgDueMonitoringList.Columns.Item(4).HeaderText, dgDueMonitoringList.Columns.Item(6).HeaderText,
                            dgDueMonitoringList.Columns.Item(7).HeaderText, dgDueMonitoringList.Columns.Item(8).HeaderText,
                           dgDueMonitoringList.Columns.Item(9).HeaderText, dgDueMonitoringList.Columns.Item(10).HeaderText, dgDueMonitoringList.Columns.Item(11).HeaderText,
                           dgDueMonitoringList.Columns.Item(12).HeaderText, dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText,
                           dgDueMonitoringList.Columns.Item(15).HeaderText, dgDueMonitoringList.Columns.Item(16).HeaderText, dgDueMonitoringList.Columns.Item(17).HeaderText,
                          , , , , , , , , , dgDueMonitoringList.Columns.Item(18).HeaderText))

        Dim TotalCount As Integer
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            TotalCount = Me.mrptDueReport.Count
            'End
        Else 'existing flow for spare assembly keep as it is
            TotalCount = Me.mTmpComplyAssemblyMonitorInspStatusList.Count
        End If

        Dim I As Integer
        Dim str(14) As String

        For I = 0 To TotalCount - 1

            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            str(9) = ""
            str(10) = ""
            str(11) = ""
            str(12) = ""
            str(13) = ""
            str(14) = ""

            If Me.dgDueMonitoringList.Rows(I).Cells(0).Text <> "&nbsp;" Then str(0) = Me.dgDueMonitoringList.Rows(I).Cells(0).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgDueMonitoringList.Rows(I).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgDueMonitoringList.Rows(I).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgDueMonitoringList.Rows(I).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgDueMonitoringList.Rows(I).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgDueMonitoringList.Rows(I).Cells(11).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(8) = Me.dgDueMonitoringList.Rows(I).Cells(12).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(13).Text <> "&nbsp;" Then str(9) = Me.dgDueMonitoringList.Rows(I).Cells(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(14).Text <> "&nbsp;" Then str(10) = Me.dgDueMonitoringList.Rows(I).Cells(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(15).Text <> "&nbsp;" Then str(11) = Me.dgDueMonitoringList.Rows(I).Cells(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(16).Text <> "&nbsp;" Then str(12) = Me.dgDueMonitoringList.Rows(I).Cells(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(17).Text <> "&nbsp;" Then str(13) = Me.dgDueMonitoringList.Rows(I).Cells(17).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(18).Text <> "&nbsp;" Then str(14) = Me.dgDueMonitoringList.Rows(I).Cells(18).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 2, ,
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6),
             str(7), str(8), str(9), str(10), str(11), str(12), str(13), , , , , , , , , , str(14)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
   mCompanyDetail.WebSite, "List of Comply Assembly Inspection Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            If mrptDueReport.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            'End
        Else 'existing flow for spare assembly keep as it is
            If mTmpComplyAssemblyMonitorInspStatusList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region
#End Region
End Class