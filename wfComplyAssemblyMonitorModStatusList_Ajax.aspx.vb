'AJAX Conversion By Vikrant On 19-Mar-2015
Imports System.Linq
Public Class wfComplyAssemblyMonitorModStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mMachineNameValueList As MachineNameValueList

    Private mTmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList
    Private mrptDueReport As rptDueReport 'Added by Shital on 15-Jun-2021
    Private DoneOn As String
    Private AircraftId As String
    Dim mMachine As Machine
    Public mBoardInfo As AircraftInformationBoard.BoardInfo 'Added by Saylee on 22-May-2009
    Private mModelMonitorModTypeList As ModelMonitorModTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String 'Added by Saylee on 30-July-2009
    Private DirectiveNo As String 'Added by Saylee on 07-Aug-2009
    Dim mModuleList As ModuleList 'Added by Sachin on 17-10-2023
    Public mAssemblyMonitorDetailForMail As String
    Public mAssemblyDetails As String
    'Added by Saylee on 09-Sep-2009
    Private mUpdateComplyHistoryAssemblyMonitorModStatusList As UpdateComplyHistoryAssemblyMonitorModStatusList
    'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance
    Dim ShowNotApplicable As Boolean = False
    'Added by vikrant on 27-July-2011
    Dim EventLogID As Guid
    Public mDirectiveDetail As String
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mDirectiveNo As String
    Dim IDForEventLog As Guid
    Dim mFileAttach As FileAttach 'Added By Prashant On 27-Nov-2014
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
        mTmpComplyAssemblyMonitorModStatusList = CType(Session("mTmpComplyAssemblyMonitorModStatusList"), tmpComplyAssemblyMonitorModStatusList)
        mrptDueReport = CType(Session("mrptDueReport"), rptDueReport) 'Added by Shital on 15-Jun-2021
        DoneOn = CType(Session("DoneOn"), String)
        AircraftId = CType(Session("AircraftId"), String)
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009
        DirectiveNo = Session("DirectiveNo") 'Added by Saylee on 07-Aug-2009
        mModuleList = Session("mModuleList") 'Added by Sachin on 17-10-2023
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        ShowNotApplicable = CType(Session("ShowNotApplicable"), Boolean) 'Added by Saylee on 7th-Jan-2011
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AssemblyId = CType(Session("AssemblyId"), String)
        SkipOneTimeDoneMRecords = CType(Session("SkipOneTimeDoneMRecords"), Boolean)
        RecordsToShow = CType(Session("RecordsToShow"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        CodeFormNoDesc = Session("CodeFormNoDesc")
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added by Saylee on 26-Aug-2020 for All27072020
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mTmpComplyAssemblyMonitorModStatusList")
        Session.Remove("mrptDueReport") 'Added by Shital on 15-Jun-2021
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 9th-Oct-2009
        Session.Remove("RecordsToShow")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        ' Session.Remove("mIsSpareAssembly") 'Added by Saylee on 26-Aug-2020 for All27072020
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyAssemblyMonitorModStatusList_Ajax.aspx?SpareAssembly=" & Session("mIsSpareAssembly") Then
            Session.Remove("mTmpComplyAssemblyMonitorModStatusList")
            Session.Remove("mrptDueReport") 'Added by Shital on 15-Jun-2021
            Session.Remove("mMachineNameValueList")
            Session.Remove("DoneOn")
            Session.Remove("AircraftId")
            Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
            Session.Remove("DirectiveNo") 'Added by Saylee on 07-Aug-2009
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
    Private Sub EnableLinks()
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            If Not mrptDueReport Is Nothing Then
                If RecordsToShow < mrptDueReport.Count Then
                    lnkShowAllRecords.Enabled = True
                    lnkShowAllRecordsTop.Enabled = True
                Else
                    lnkShowAllRecords.Enabled = False
                    lnkShowAllRecordsTop.Enabled = False
                End If
            End If
            'End
        Else 'existing flow for spare assembly keep as it is
            If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
                If RecordsToShow < mTmpComplyAssemblyMonitorModStatusList.Count Then
                    lnkShowAllRecords.Enabled = True
                    lnkShowAllRecordsTop.Enabled = True
                Else
                    lnkShowAllRecords.Enabled = False
                    lnkShowAllRecordsTop.Enabled = False
                End If
            End If
        End If

    End Sub
    Private Sub ControlVisibility()
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            btnPrint.Enabled = (mrptDueReport.Count > 0)
            btnPrintTop.Enabled = (mrptDueReport.Count > 0)
            'End
        Else 'existing flow for spare assembly keep as it is
            btnPrint.Enabled = (mTmpComplyAssemblyMonitorModStatusList.Count > 0)
            btnPrintTop.Enabled = (mTmpComplyAssemblyMonitorModStatusList.Count > 0)
        End If
        dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
        dgDueMonitoringList.Columns(28).Visible = IIf(chkApplicable.Checked, False, True)

        'Added by Saylee on 26-Aug-2020 for All27072020
        If mIsSpareAssembly = 1 Then
            pllblAircraft.Visible = False
            plAircraft.Visible = False
        Else
            pllblAircraft.Visible = True
            plAircraft.Visible = True
        End If
        If Session("mIsSpareAssembly") = 1 Then
            btnAddNew.Visible = False
            btnAddNewTop.Visible = False
        End If

        EnableLinks()
    End Sub
    Private Sub ComplyRecord(ByVal Index As Int32)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim IsApplicable As Boolean
        Dim mAssemblyStatus As AssemblyStatus
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            IsApplicable = mrptDueReport(Index).IsApplicable

            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
            IsApplicable = mTmpComplyAssemblyMonitorModStatusList(Index).IsApplicable
        End If
        If (mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And (mPrevAssemblyMonitorModStatus.IsCompleted Or mPrevAssemblyMonitorModStatus.FetchRecordCount(mPrevAssemblyMonitorModStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not IsApplicable Then
            MSGBoxCtrl.show(MSGBox.Message_title.MonitoringNotApplicable, MSGBox.Message_text.MonitoringNotApplicable, "You are trying to comply the record.Directives monitoring is not applicable, can not be complied.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                'Commented on 07-Aug-2020 by Shital as previous(last) effective date carried forward for all nexts comply activity
                'mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, txtDate.Text, mTmpComplyAssemblyMonitorModStatusList(Index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, txtDate.Text, mrptDueReport(Index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.AsOnDate.ToString, mMachine.HourType)
                'End
                'Added by vikrant on 27-July-2011
                mAircraft = mrptDueReport(Index).RegNo 'MachineInfo
                mDirectiveNo = mrptDueReport(Index).Number 'ModNumber
                mMonitorInfo = mrptDueReport(Index).Type 'ModelMonitorModInfo
                mMonitorType = mrptDueReport(Index).MonitorType
                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Done On Date : " & mrptDueReport(Index).DoneOnDate & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                MarkLog(Util.Action.Comply, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mrptDueReport.Item(mrptDueReport.CurrentIndex).ID, EventLogID)
                'End
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'End
            Else 'existing flow for spare assembly keep as it is
                'Commented on 07-Aug-2020 by Shital as previous(last) effective date carried forward for all nexts comply activity
                'mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, txtDate.Text, mTmpComplyAssemblyMonitorModStatusList(Index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, txtDate.Text, mTmpComplyAssemblyMonitorModStatusList(Index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.AsOnDate.ToString, mMachine.HourType)
                'End
                'Added by vikrant on 27-July-2011
                mAircraft = mTmpComplyAssemblyMonitorModStatusList(Index).MachineInfo
                mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList(Index).ModNumber
                mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModInfo
                mMonitorType = mTmpComplyAssemblyMonitorModStatusList(Index).MonitorType
                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Done On Date : " & mTmpComplyAssemblyMonitorModStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorModStatusList(Index).DoneOnValueFormatted
                MarkLog(Util.Action.Comply, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, EventLogID)
                'End
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
            End If
            mAssemblyMonitorModStatus.IsLater = mPrevAssemblyMonitorModStatus.IsLater
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("From") = 0 'New record
            ''
            mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus


            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus

            'Added by Saylee on 22-May-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            'Added By Vikrant On 25-Nov-2014
            'Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorModStatus.ID) 'Sort = 1 : Installation
            'Session("mFileAttach") = mFileAttach
            'End
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim mAssemblyStatus As AssemblyStatus
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).ID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If


        If mPrevAssemblyMonitorModStatus.IsMaster And mPrevAssemblyMonitorModStatus.IsApplicable And chkApplicable.Checked = False Then
            ' MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit Comply Assembly Directives Status.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mPrevAssemblyMonitorModStatus.IsMaster) And (Not mPrevAssemblyMonitorModStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records
            Session("mAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("From") = 1 'Edit record

            ' Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
            End If

            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            'Added By Vikrant On 25-Nov-2014
            'If mPrevAssemblyMonitorModStatus.IsAttachmentAdded Then
            '    Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevAssemblyMonitorModStatus.ID) 'Sort = 1 - Installation
            '    Session("mFileAttach") = mFileAttach
            'Else
            '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevAssemblyMonitorModStatus.ID)
            '    Session("mFileAttach") = mFileAttach
            'End If
            'End
            RemoveSession()

            'Commented And Added by Saylee on 3-Dec-2019 , as to open Master form for NOT Appilcable Records and not COMPLY form
            '''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorModStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************
            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevAssemblyMonitorModStatus.IsMaster = False) And (mPrevAssemblyMonitorModStatus.IsCompleted = False) And mPrevAssemblyMonitorModStatus.IsDone = False) Then
            Dim mModelMonitorMod As ModelMonitorMod
            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
                mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mrptDueReport.Item(Index).StatusMasterID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).ID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
                mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelMonitorModID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
            End If

            Session("mModelMonitorMod") = mModelMonitorMod
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            '    Response.Redirect("wfAssemblyMonitorModStatusNew_Ajax.aspx?BackPage=Index.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorModStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType, True)
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("From") = 1 'Edit record
            ''
            ' Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
            End If

            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            'If mAssemblyMonitorModStatus.IsAttachmentAdded Then
            '    Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID) 'Sort = 1 - Installation
            '    Session("mFileAttach") = mFileAttach
            'Else
            '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorModStatus.ID)
            '    Session("mFileAttach") = mFileAttach
            'End If
            'End

            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'Added by vikrant on 27-July-2011
                mAircraft = mrptDueReport(Index).RegNo
                mDirectiveNo = mrptDueReport(Index).Number
                mMonitorInfo = mrptDueReport(Index).Type
                mMonitorType = mrptDueReport(Index).MonitorType
                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Done On Date :" & mrptDueReport(Index).DoneOnDate & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                MarkLog(Util.Action.Edit, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mrptDueReport.Item(mrptDueReport.CurrentIndex).ID, EventLogID)
                'End
                'End
            Else 'existing flow for spare assembly keep as it is
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
                'Added by vikrant on 27-July-2011
                mAircraft = mTmpComplyAssemblyMonitorModStatusList(Index).MachineInfo
                mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList(Index).ModNumber
                mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModInfo
                mMonitorType = mTmpComplyAssemblyMonitorModStatusList(Index).MonitorType
                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Done On Date :" & mTmpComplyAssemblyMonitorModStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorModStatusList(Index).DoneOnValueFormatted
                MarkLog(Util.Action.Edit, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, EventLogID)
                'End
            End If

            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub HistoryRecords(ByVal Index As Int32)  'Added by Saylee on 09-Sep-2009
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If


        If mPrevAssemblyMonitorModStatus.IsMaster Then
            MSGBoxCtrl.Show("Master Record!", "There is no history for this record", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            'Dim mMachine As Machine
            Dim mAssemblyStatus As AssemblyStatus
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("From") = 1 'Edit record

            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                Session("ATA") = mrptDueReport.Item(Index).ATAChapter.ToString
                Session("Description") = mrptDueReport.Item(Index).Description
                Session("ModelSerialNo") = mrptDueReport.Item(Index).ModelSerialNo
                'Added by Vikrant on 3-Aug-2011
                mAircraft = mrptDueReport.Item(mrptDueReport.CurrentIndex).RegNo
                mDirectiveNo = mrptDueReport.Item(mrptDueReport.CurrentIndex).Number
                mMonitorInfo = mrptDueReport.Item(mrptDueReport.CurrentIndex).Type
                mMonitorType = mrptDueReport.Item(mrptDueReport.CurrentIndex).MonitorType
                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType
                'End
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
                Session("ATA") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString
                Session("Description") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
                Session("ModelSerialNo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo
                'Added by Vikrant on 3-Aug-2011
                mAircraft = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).MachineInfo
                mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModNumber
                mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModelMonitorModInfo
                mMonitorType = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).MonitorType
                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType
                'End
            End If
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************


            mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorModStatus.ModelMonitorModID, mMachine.HourType)
            Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList

            'RemoveSession()

            MarkLog(Util.Action.View, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfUpdateComplyHistoryAssemblyMonitorModStatusList.aspx?GChildPage2=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDirectiveHistoryWindow", "OpenDirectiveHistoryWindow()", True)
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            'Revise Activity
            If chkApplicable.Checked And mrptDueReport(Index).ModelActivityCount > 1 Then 'Revise Activity
                MSGBoxCtrl.Show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            End If
            mrptDueReport.CurrentIndex = Index
            Session("mrptDueReport") = mrptDueReport
            'End
            'End
        Else 'existing flow for spare assembly keep as it is
            'Revise Activity
            If chkApplicable.Checked And mTmpComplyAssemblyMonitorModStatusList(Index).ModelActivityCount > 1 Then 'Revise Activity
                MSGBoxCtrl.Show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            End If
            mTmpComplyAssemblyMonitorModStatusList.CurrentIndex = Index ' 'Commented  by Shital on 15-Jun-2021
            Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
        End If
    End Sub
    Private Sub ReviseRecord(ByVal Index As Int32) 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
        '  Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mMachine = Machine.GetMachine(mrptDueReport.Item(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineID)
            mPrevAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If

        Session("mAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
        Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mrptDueReport(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            'End
        Else 'existing flow for spare assembly keep as it is
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
        End If
        'Added By Vikrant On 25-Nov-2014
        'If mPrevAssemblyMonitorModStatus.IsAttachmentAdded Then
        '    Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevAssemblyMonitorModStatus.ID) 'Sort = 1 - Installation
        '    Session("mFileAttach") = mFileAttach
        'Else
        '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevAssemblyMonitorModStatus.ID)
        '    Session("mFileAttach") = mFileAttach
        'End If
        'End

        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        If mIsSpareAssembly = 0 Then 'Added By Vikrant for faster processing
            Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
            'End
        Else 'existing flow for spare assembly keep as it is
            Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
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
                            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                                'Added by vikrant on 27-July-2011
                                IDForEventLog = mrptDueReport(mrptDueReport.CurrentIndex).ID
                                mAircraft = mrptDueReport.Item(mrptDueReport.CurrentIndex).RegNo
                                mDirectiveNo = mrptDueReport.Item(mrptDueReport.CurrentIndex).Number
                                mMonitorInfo = mrptDueReport.Item(mrptDueReport.CurrentIndex).TypeDet
                                mMonitorType = mrptDueReport.Item(mrptDueReport.CurrentIndex).MonitorType
                                mAssemblyDetails = mrptDueReport.Item(mrptDueReport.CurrentIndex).ModelName + "-" + mrptDueReport.Item(mrptDueReport.CurrentIndex).SerialNo + (IIf(mrptDueReport.Item(mrptDueReport.CurrentIndex).Position <> "", " (" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Position + ")", ""))
                                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType
                                mAssemblyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> <b>Directive No. : </b>" + mDirectiveNo + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Description

                                'End
                                'Added by Saylee on 28-May-2009
                                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mrptDueReport.CurrentItem.ID)
                                '********************************
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mrptDueReport.CurrentItem.ID, 7)
                                If mrptDueReport(mrptDueReport.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(mrptDueReport.CurrentIndex).ID)
                                End If
                                AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(mrptDueReport.CurrentItem.ID)
                                'End
                            Else 'existing flow for spare assembly keep as it is
                                'Added by vikrant on 27-July-2011
                                IDForEventLog = mTmpComplyAssemblyMonitorModStatusList(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID
                                mAircraft = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).MachineInfo
                                mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModNumber
                                mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModelMonitorModInfo
                                mMonitorType = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).MonitorType
                                mAssemblyDetails = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyInfo

                                mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType

                                mAssemblyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Directive No. : </b>" + mDirectiveNo + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).Description

                                'End
                                'Added by Saylee on 28-May-2009
                                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.AssemblyMonitorModStatusID)
                                '********************************
                                'Added by Saylee on 9th-Oct-2009
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.AssemblyMonitorModStatusID, 7)
                                '=============================
                                If mTmpComplyAssemblyMonitorModStatusList(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorModStatusList(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID)
                                End If
                                AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.AssemblyMonitorModStatusID)
                            End If

                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            Session("mMachineMaintenance") = mMachineMaintenance
                            'MarkLog(Util.Action.Comply, "Assembly Monitor Directive", mDirectiveDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, EventLogID)

                            'Added by Saylee on 28-May-2009
                            mBoardInfo.IsComplyDelete = True
                            mBoardInfo.ApplyEdit()
                            mBoardInfo.Save()
                            Session("mAircraftInformationBoardList") = Nothing
                            '********************************

                            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                                'Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                                If AppSettings("LinkMaintenance") = "True" Then
                                    If LinkMaintenanceList.GetLinkMaintenanceList(mrptDueReport.CurrentItem.StatusMasterID.ToString).Count > 0 Then
                                        MSGBoxCtrl.Show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                        Exit Sub
                                    End If
                                End If
                                'End
                                'End
                            Else 'existing flow for spare assembly keep as it is
                                'Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                                If AppSettings("LinkMaintenance") = "True" Then
                                    If LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.ModelMonitorModID.ToString).Count > 0 Then
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
                                MarkLog(Util.Action.Delete, "AssemblyModifications", "Can't delete :" & mDirectiveDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "ReviseActivity" Then 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
                        MarkLog(Util.Action.[New], "Model Directive", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mModelMonitorMod As ModelMonitorMod
                        Dim ID As Guid = Guid.NewGuid 'Revise Activity
                        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus = Session("mAssemblyMonitorModStatus")
                        mMachine = Session("mMachine")
                        mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(mAssemblyMonitorModStatus.ModelMonitorMod, mMachine.HourType)
                        'New
                        Dim tmpModelMonitorMod As ModelMonitorMod
                        tmpModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mAssemblyMonitorModStatus.ModelMonitorMod.ID)
                        'If mAssemblyMonitorModStatus.DoneOnFormatted.ToString = "" Then
                        '    mModelMonitorMod.IssueDate = mAssemblyMonitorModStatus.AsOnDateFormatted.ToString
                        'Else
                        '    mModelMonitorMod.IssueDate = mAssemblyMonitorModStatus.DoneOnFormatted.ToString
                        'End If
                        If Not tmpModelMonitorMod.IssueDateFormatted.ToString = "" Then
                            mModelMonitorMod.IssueDate = tmpModelMonitorMod.IssueDate
                        Else
                            mModelMonitorMod.IssueDate = System.DBNull.Value
                        End If
                        'End
                        Session("mModelMonitorMod") = mModelMonitorMod
                        '''''''''''''   RemoveSession()
                        mModelMonitorMod.BeginEdit()
                        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                        Session("mPrevAssemblyMonitorModStatusForRevise") = mAssemblyMonitorModStatus

                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow();", True)

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
        RecordsToShow = dgDueMonitoringList.PageSize
        Session("RecordsToShow") = RecordsToShow

        dgDueMonitoringList.PageIndex = 0
        Session("DoneOn") = txtDate.Text
        Session("AircraftId") = cmbAircraftList.SelectedValue
        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
        Session("DirectiveNo") = Trim(txtDirectiveNo.Text)  'Added by Saylee on 7-Aug-2009
        Session("ShowNotApplicable") = chkApplicable.Checked  'Added by Saylee on 7-Jan-2011
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue
        Session("SkipOneTimeDoneMRecords") = IIf(chkOneTimeMasterRecords.Checked, True, False)
        Session("CodeFormNoDesc") = Trim(txtCodeFormNo.Text)

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, "", cmbAircraftAssembly.SelectedValue.ToString, 3, CInt(IIf(cmbMonitorType.SelectedIndex > 0, cmbMonitorType.SelectedValue, 0)), chkApplicable.Checked, chkOneTimeMasterRecords.Checked, txtCodeFormNo.Text.Trim, , , DirectiveNo)
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
            mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(txtDate.Text, IIf(mIsSpareAssembly = 1, Guid.Empty, cmbAircraftList.SelectedValue).ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , cmbMonitorType.SelectedValue, , , DirectiveNo, chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareAssembly:=mIsSpareAssembly, AssemblyID:=cmbAircraftAssembly.SelectedValue)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            Session("mrptDueReport") = mrptDueReport
        End If
        DataBind()


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
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            If RecordsToShow < mrptDueReport.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Assembly Directives Status as per selected criteria : " & RecordsToShow.ToString & " of " & mrptDueReport.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Assembly Directives Status as per selected criteria : " & mrptDueReport.Count & " Record(s) found."
            End If
            lbltitle.Text = "List of Assembly Directives Status"
            'End
        Else 'existing flow for spare assembly keep as it is
            If RecordsToShow < mTmpComplyAssemblyMonitorModStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Stock/Removed Assembly Directives Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyAssemblyMonitorModStatusList.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Stock/Removed Assembly Directives Status as per selected criteria : " & mTmpComplyAssemblyMonitorModStatusList.Count & " Record(s) found."
            End If
            lbltitle.Text = "List of Stock/Removed Assembly Directives Status"
        End If

    End Sub
    Private Sub SetRights() 'Added By Prashant On 31-Mar-2011
        If (User.IsInRole("MachineAssemblyModificationNew")) = False Then
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
            B = CType(Me.dgDueMonitoringList.Rows(j).Cells(25).Text, Boolean)
            c = CType(Me.dgDueMonitoringList.Rows(j).Cells(27).Text, Boolean)
            If B = True Then
                dgDueMonitoringList.Rows(j).Cells(24).Enabled = False
            End If

            'Commented by Saylee on 27-Jul-2023, as view image button added
            'If c = False Then
            '    dgDueMonitoringList.Rows(j).Cells(26).Enabled = False
            'End If

            'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = False
                btnAddNewTop.Enabled = False
                btnAddNew.Enabled = False
                lblReadOnly.Visible = True
                dgDueMonitoringList.Rows(j).Cells(28).Enabled = False 'Revise
            Else
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = True
                btnAddNewTop.Enabled = True
                btnAddNew.Enabled = True
                lblReadOnly.Visible = False
                dgDueMonitoringList.Rows(j).Cells(28).Enabled = True 'Revise
            End If
            '*************************
            ''Dim MonitorTypeID As Integer = CType(Me.dgDueMonitoringList.Rows(j).Cells(30).Text, Integer) 'Revise 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
            ''dgDueMonitoringList.Rows(j).Cells(28).Enabled = Not (MonitorTypeID = 1 Or MonitorTypeID = 4) And dgDueMonitoringList.Rows(j).Cells(13).Text <> "" 'Revise 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
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
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            'End
        Else 'existing flow for spare assembly keep as it is
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
            End If
        End If

        dgDueMonitoringList.DataBind()
        SetGrid()
    End Sub
    Private Sub SetMachineMaintenanceObject(mMachineMaintenance As MachineMaintenance, CurrAssemblyMonitorMod As AssemblyMonitorModStatus)
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
                mMaxLogNo = MaxLogNo.GetMaxLogNo(mMachineMaintenance.Date, mMachineMaintenance.MachineID, CurrAssemblyMonitorMod.AssemblyID)
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
        If mModuleList.Item("AssemblyModifications").MailsRequire = True Then
            If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
                'Do nothing
                Exit Sub
            End If
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Task Details :  <br/> <br/>  " & mAssemblyMonitorDetailForMail & " <br/> <b> Deleted by User:</b> " + User.Identity.Name + "<b> on: </b>" + New SmartDate(Today.Date).FormattedText + "</font></P> ")
            str = str + ("</body></html>")
            'SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Assembly Inspections Deleted", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), Info:=str, ToMailID:=mModuleList.Item("Order").SendToMailID, Remark:=Session("SendMailRemark"), ReportGenratedBy:=Session("ReportGenratedBy"))

            SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Task Deleted", Info:=str, ToMailID:=mModuleList.Item("AssemblyModifications").SendToMailID, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
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
        cmbAircraftList.DataBind()
        Session("AircraftId") = cmbAircraftList.SelectedValue
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly
        Session("mMachineNameValueList") = mMachineNameValueList

        'Added By Prashant 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraftList.SelectedValue, txtDate.Text.ToString, "(All)", True, IsForSpareAssembly:=mIsSpareAssembly) ' mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27072020
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

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mrptDueReport = rptDueReport.GetList(DoneOn, cmbAircraftList.SelectedItem.ToString, , True, "", cmbAircraftAssembly.SelectedValue.ToString, 3, CInt(MonitorTypeID), ShowNotApplicable, chkOneTimeMasterRecords.Checked, CodeFormNoDesc, , , DirectiveNo)
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
            'mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27072020
            mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(DoneOn, IIf(mIsSpareAssembly = 1, Guid.Empty, cmbAircraftList.SelectedValue).ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , CType(MonitorTypeID, Integer), , , DirectiveNo, ShowNotApplicable, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=CodeFormNoDesc, IsSpareAssembly:=CBool(mIsSpareAssembly), AssemblyID:=cmbAircraftAssembly.SelectedValue)
            'End
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
            End If
            Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
        End If
        dgDueMonitoringList.DataBind()
        'Added by Saylee on 30-July-2009
        mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(All)")
        cmbMonitorType.DataSource = mModelMonitorModTypeList
        If IsNothing(MonitorTypeID) Or MonitorTypeID = "" Then
            'Do nothing
        Else
            cmbMonitorType.SelectedValue = MonitorTypeID
        End If
        cmbMonitorType.DataBind()
        Session("MonitorTypeID") = MonitorTypeID 'Added by Saylee on 30-July-2009

        txtDirectiveNo.Text = DirectiveNo ''Added by Saylee on 07-Aug-2009
        txtDirectiveNo.DataBind()
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
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by vikrant on 27-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            cmbAircraftList.Focus()

            ' 'Added by Saylee on 26-Aug-2020 for All27082020
            mIsSpareAssembly = Request.QueryString("SpareAssembly")
            Session("mIsSpareAssembly") = mIsSpareAssembly
            '************************

            Session("MiddleFrame") = "wfComplyAssemblyMonitorModStatusList_Ajax.aspx?SpareAssembly=" & mIsSpareAssembly  ' 'mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27082020
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
        'Added by vikrant on 27-July-2011
        MarkLog(Util.Action.Close, "AssemblyModifications", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session.Remove("From")
        Session.Remove("DoneOn")
        Session.Remove("AircraftId")
        Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
        Session.Remove("DirectiveNo")  'Added by Saylee on 07-Aug-2009
        Session.Remove("ATA")
        Session.Remove("CodeFormNoDesc")
        Session("MiddleFrame") = ""
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
        FindNow()
        SetPage()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub dgDueMonitoringList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueMonitoringList.RowCommand
        Select Case e.CommandName
            Case "Comply"
                If Not User.IsInRole("AssemblyModificationsNew") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(28).Visible = IIf(chkApplicable.Checked, False, True)
                ComplyRecord(CInt(e.CommandArgument))
            Case "EditRec"
                If (Not User.IsInRole("AssemblyModificationsView") And Not User.IsInRole("AssemblyModificationsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(28).Visible = IIf(chkApplicable.Checked, False, True)
                EditRecord(CInt(e.CommandArgument))
            Case "DeleteRec"
                If (Not User.IsInRole("AssemblyModificationsDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(28).Visible = IIf(chkApplicable.Checked, False, True)
                DeleteRecord(CInt(e.CommandArgument))
            Case "History"  'Added by Saylee on 09-Sep-2009
                If (Not User.IsInRole("AssemblyModificationsView") And Not User.IsInRole("AssemblyModificationsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(28).Visible = IIf(chkApplicable.Checked, False, True)
                HistoryRecords(CInt(e.CommandArgument))
            Case "ViewRec"
                GridBind()
                dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(28).Visible = IIf(chkApplicable.Checked, False, True)
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(CInt(e.CommandArgument)).ID)
                    'End
                Else 'existing flow for spare assembly keep as it is
                    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorModStatusList(CInt(e.CommandArgument)).ID)
                End If
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
            Case "Revise" 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
                If (Not User.IsInRole("AssemblyModificationsView") And Not User.IsInRole("AssemblyModificationsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                GridBind()
                dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(28).Visible = IIf(chkApplicable.Checked, False, True)
                ReviseRecord(CInt(e.CommandArgument))
                MSGBoxCtrl.Show("Alert!", "You are about to Revise Model Activity.After revision of model activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        If IsValid Then
            'Added by vikrant on 27-July-2011
            MarkLog(Util.Action.[New], "AssemblyModifications", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Session("AircraftIdForMod") = cmbAircraftList.SelectedValue.ToString
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorModStatusListNew.aspx?BackPage=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAssemblyDirectiveListNewWindow", "OpenAssemblyDirectiveListNewWindow()", True)
            Session("NewPage") = "True"
        End If
        ' Response.Redirect("wfAssemblyMonitorModStatusListNew.aspx?BackPage=wfComplyAssemblyMonitorModStatusList_Ajax.aspx")
    End Sub
    Private Sub dgDueMonitoringList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueMonitoringList.Sorting
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mrptDueReport.Sort(IIf(e.SortExpression = "RemainingTimeForCompliancePage", "RemainingValueForSorting", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            Session("mrptDueReport") = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            mTmpComplyAssemblyMonitorModStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
            End If
            Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
        End If

        dgDueMonitoringList.DataBind()
        SetGrid()
    End Sub
    Private Sub txtDirectiveNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDirectiveNo.TextChanged
        DirectiveNo = txtDirectiveNo.Text
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnDirectiveHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnDirectiveHistory.Click
        FindNow()
        SetPage()
        upnlgrid.Update()
    End Sub
    Private Sub lnkShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAllRecords.Click, lnkShowAllRecordsTop.Click
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            RecordsToShow = mrptDueReport.Count
            dgDueMonitoringList.DataSource = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            RecordsToShow = mTmpComplyAssemblyMonitorModStatusList.Count
            dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
        End If

        Session("RecordsToShow") = RecordsToShow
        'Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)

        dgDueMonitoringList.DataBind()
        SetPage()
        SetGrid()
        ControlVisibility()
        upnlActionBtn.Update()
    End Sub
    Protected Sub dgDueMonitoringList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgDueMonitoringList.Columns(i).HeaderText
            Next
        End If
    End Sub

    'Added by Saylee on 27-Jul-2023, to give Revise on comply list page
    Private Sub hdnBtnModelModMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelModMaster.Click
        'Revise Activity
        If Not Session("mPrevAssemblyMonitorModStatusForRevise") Is Nothing Then
            Dim mPrevAssemblyMonitorModStatusForRevise As AssemblyMonitorModStatus
            mPrevAssemblyMonitorModStatusForRevise = Session("mPrevAssemblyMonitorModStatusForRevise")
            mPrevAssemblyMonitorModStatusForRevise.IsApplicable = False
            mPrevAssemblyMonitorModStatusForRevise.Save()
            Session.Remove("mPrevAssemblyMonitorModStatusForRevise")
            Session.Remove("RevisedFromListPage")


            Dim mCurrAssemblyMonitorModStatusForRevise As AssemblyMonitorModStatus
            mCurrAssemblyMonitorModStatusForRevise = Session("mAssemblyMonitorModStatus")


            If mPrevAssemblyMonitorModStatusForRevise.DoneOnFormatted.ToString = "" Then
                mCurrAssemblyMonitorModStatusForRevise.AsOnDate = mPrevAssemblyMonitorModStatusForRevise.AsOnDateFormatted.ToString

            Else
                mCurrAssemblyMonitorModStatusForRevise.AsOnDate = mPrevAssemblyMonitorModStatusForRevise.DoneOnFormatted.ToString

            End If
            For i As Integer = 0 To mPrevAssemblyMonitorModStatusForRevise.AssemblyMonitorModStatusPeriods.Count - 1
                Dim PeriodID = mPrevAssemblyMonitorModStatusForRevise.AssemblyMonitorModStatusPeriods(i).PeriodID
                If mCurrAssemblyMonitorModStatusForRevise.AssemblyMonitorModStatusPeriods.Contains(PeriodID, "") Then
                    mCurrAssemblyMonitorModStatusForRevise.AssemblyMonitorModStatusPeriods.Item(PeriodID, "").DoneOnValue = mPrevAssemblyMonitorModStatusForRevise.AssemblyMonitorModStatusPeriods(i).DoneOnValue
                End If
            Next
            mMachine = Session("mMachine")
            Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.NewMachineMaintenance(mMachine.ID, 6, mCurrAssemblyMonitorModStatusForRevise.AsOnDate, mCurrAssemblyMonitorModStatusForRevise.ID, Guid.Empty, 0, 0, mCurrAssemblyMonitorModStatusForRevise.AssemblyStatusID)
            mMachineMaintenance.MaintenanceID = mCurrAssemblyMonitorModStatusForRevise.ID

            mCurrAssemblyMonitorModStatusForRevise.IsMaster = False
            mCurrAssemblyMonitorModStatusForRevise.IsApplicable = True
            mCurrAssemblyMonitorModStatusForRevise.Save()
            SetMachineMaintenanceObject(mMachineMaintenance, mCurrAssemblyMonitorModStatusForRevise)
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            Session.Remove("mAssemblyMonitorModStatus")
            FindNow()
            SetPage()
            upnlgrid.Update()
        End If
        'End
    End Sub
#End Region

#Region " Report "
    'Created By:- Rajnish on 22-09-2006

#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Private SearchStr4 As String = ""
#End Region

#Region " Event "

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If (Not User.IsInRole("AssemblyModificationsPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            dgDueMonitoringList.DataSource = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
        End If
        dgDueMonitoringList.DataBind()
        SetGrid()

        Dim Rpt As New crListComplyAssemblyMonitorStatus
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "Date :" + "  " + txtDate.Text
        SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        SearchStr3 = ""
        SearchStr4 = "Aircraft :" + "  " + cmbAircraftList.SelectedItem.Text

        ReportDetails.Add(New rptStatus(, 1, ,
              , , , dgDueMonitoringList.Columns.Item(0).HeaderText, , dgDueMonitoringList.Columns.Item(5).HeaderText, dgDueMonitoringList.Columns.Item(7).HeaderText,
              dgDueMonitoringList.Columns.Item(8).HeaderText, dgDueMonitoringList.Columns.Item(9).HeaderText,
              dgDueMonitoringList.Columns.Item(10).HeaderText, dgDueMonitoringList.Columns.Item(11).HeaderText, dgDueMonitoringList.Columns.Item(12).HeaderText,
              dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText, dgDueMonitoringList.Columns.Item(15).HeaderText,
              dgDueMonitoringList.Columns.Item(16).HeaderText, dgDueMonitoringList.Columns.Item(17).HeaderText, dgDueMonitoringList.Columns.Item(18).HeaderText,
              , , , , , , , , , dgDueMonitoringList.Columns.Item(19).HeaderText))

        Dim TotalCount As Integer
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            TotalCount = Me.mrptDueReport.Count
            'End
        Else 'existing flow for spare assembly keep as it is
            TotalCount = Me.mTmpComplyAssemblyMonitorModStatusList.Count
        End If

        Dim I As Integer
        Dim str(15) As String
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
            If Me.dgDueMonitoringList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(3) = Me.dgDueMonitoringList.Rows(I).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(4) = Me.dgDueMonitoringList.Rows(I).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(5) = Me.dgDueMonitoringList.Rows(I).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(6) = Me.dgDueMonitoringList.Rows(I).Cells(11).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(7) = Me.dgDueMonitoringList.Rows(I).Cells(12).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(13).Text <> "&nbsp;" Then str(8) = Me.dgDueMonitoringList.Rows(I).Cells(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(14).Text <> "&nbsp;" Then str(9) = Me.dgDueMonitoringList.Rows(I).Cells(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(15).Text <> "&nbsp;" Then str(10) = Me.dgDueMonitoringList.Rows(I).Cells(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(16).Text <> "&nbsp;" Then str(11) = Me.dgDueMonitoringList.Rows(I).Cells(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(17).Text <> "&nbsp;" Then str(12) = Me.dgDueMonitoringList.Rows(I).Cells(17).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(18).Text <> "&nbsp;" Then str(13) = Me.dgDueMonitoringList.Rows(I).Cells(18).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(19).Text <> "&nbsp;" Then str(14) = Me.dgDueMonitoringList.Rows(I).Cells(19).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 2, ,
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8), str(9),
        str(10), str(11), str(12), str(13), , , , , , , , , , str(14)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
   mCompanyDetail.WebSite, "List of Comply Assembly Directives Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            If mrptDueReport.Count = 0 Then     'Added by Shital o 16-Jun-2021
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            'End
        Else 'existing flow for spare assembly keep as it is
            If mTmpComplyAssemblyMonitorModStatusList.Count = 0 Then
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