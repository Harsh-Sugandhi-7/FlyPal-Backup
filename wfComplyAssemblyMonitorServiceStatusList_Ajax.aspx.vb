'AJAX Conversion By Vikrant On 20-Mar-2015
Imports System.Linq
Public Class wfComplyAssemblyMonitorServiceStatusList_Ajax
    Inherits System.Web.UI.Page

#Region "  Variable Declaration "
    Private mMachineNameValueList As MachineNameValueList

    Private mTmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
    Private mrptDueReport As rptDueReport 'Added by Shital on 18-Jun-2021
    Private DoneOn As String
    Private AircraftId As String
    Public mAssemblyInfo As String                                          'Code Added 29,Jan,2007
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus    'Code Feb,1,2007
    Dim mMachine As Machine
    Public mBoardInfo As AircraftInformationBoard.BoardInfo 'Added by Saylee on 22-May-2009
    Private mModelMonitorServiceTypeList As ModelMonitorServiceTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String 'Added by Saylee on 30-July-2009
    Dim mModuleList As ModuleList 'Added by Sachin on 17-10-2023
    'Added by Saylee on 09-Sep-2009
    Private mUpdateComplyHistoryAssemblyMonitorServiceStatusList As UpdateComplyHistoryAssemblyMonitorServiceStatusList
    'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance
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

#Region " Helper Methods "

    Private Sub GetSession()

        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mTmpComplyAssemblyMonitorServiceStatusList = CType(Session("mTmpComplyAssemblyMonitorServiceStatusList"), tmpComplyAssemblyMonitorServiceStatusList)
        mrptDueReport = CType(Session("mrptDueReport"), rptDueReport) 'Added by Shital on 18-Jun-2021
        DoneOn = CType(Session("DoneOn"), String)
        AircraftId = CType(Session("AircraftId"), String)
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009
        mModuleList = Session("mModuleList") 'Added by Sachin on 17-10-2023
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 6th-Oct-2009
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
        Session.Remove("mTmpComplyAssemblyMonitorServiceStatusList")
        Session.Remove("mrptDueReport") 'Added by Shital on 18-Jun-2021
        Session.Remove("RecordsToShow")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        'Session.Remove("mIsSpareAssembly") 'Added by Saylee on 26-Aug-2020 for All27072020
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyAssemblyMonitorServiceStatusList_Ajax.aspx?SpareAssembly=" & Session("mIsSpareAssembly") Then
            Session.Remove("mTmpComplyAssemblyMonitorServiceStatusList")
            Session.Remove("mrptDueReport") 'Added by Shital on 18-Jun-2021
            Session.Remove("mMachineNameValueList")
            Session.Remove("DoneOn")
            Session.Remove("AircraftId")
            Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
            Session.Remove("mMachineMaintenance") 'Added by Saylee on 6th-Oct-2009
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

            If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then

                If RecordsToShow < mTmpComplyAssemblyMonitorServiceStatusList.Count Then

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

            btnPrint.Enabled = (mTmpComplyAssemblyMonitorServiceStatusList.Count > 0)
            btnPrintTop.Enabled = (mTmpComplyAssemblyMonitorServiceStatusList.Count > 0)

        End If

        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
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
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
            mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        End If

        If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            Dim mAssemblyStatus As AssemblyStatus
            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, txtDate.Text, mrptDueReport(Index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                mAircraft = mrptDueReport(Index).RegNo
                mMonitorInfo = mrptDueReport(Index).Type
                mMonitorType = mrptDueReport(Index).MonitorType
                mMonitorDesc = mrptDueReport(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & mrptDueReport(Index).DoneOnDate & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, txtDate.Text, mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description
                mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineInfo
                mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo
                mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType
                mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnFormatted.ToString & " Done On Value : " & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnValueFormatted
            End If

            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
            Session("From") = 0 'New record
            mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            ''NewMachineMaintenance(mAssemblyStatus, mAssemblyMonitorServiceStatus.ID)

            'Added by Saylee on 22-May-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End




            RemoveSession()
            'Changed by Vikrant on 26-July-2011
            MarkLog(Util.Action.Comply, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx'); ", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mAssemblyStatus As AssemblyStatus
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
            mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
        End If

        If mPrevAssemblyMonitorServiceStatus.IsMaster And mPrevAssemblyMonitorServiceStatus.IsApplicable And chkApplicable.Checked = False Then
            'MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit the record.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mPrevAssemblyMonitorServiceStatus.IsMaster) And (Not mPrevAssemblyMonitorServiceStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records
            Session("mAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
            Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
            Session("From") = 1 'Edit record
            ''
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus

            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            If mPrevAssemblyMonitorServiceStatus.IsAttachmentAdded Then
                Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevAssemblyMonitorServiceStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevAssemblyMonitorServiceStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'End
            Else 'existing flow for spare assembly keep as it is
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description
            End If



            ' ''GetMachineMaintenance(mPrevAssemblyMonitorServiceStatus.ID)    'Added by Saylee on 7-Oct-2009
            RemoveSession()
            ''MarkLog(Util.Action.Edit, "ComplyAssemblyMonitorServiceStatus", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)

            'Commented And Added by Saylee on 3-Dec-2019 , as to open Master form for NOT Appilcable Records and not COMPLY form
            '' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************
            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevAssemblyMonitorServiceStatus.IsMaster = False) And (mPrevAssemblyMonitorServiceStatus.IsCompleted = False) And mPrevAssemblyMonitorServiceStatus.IsDone = False) Then
            Dim mModelMonitorService As ModelMonitorService

            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
                mModelMonitorService = ModelMonitorService.GetModelMonitorService(mrptDueReport.Item(Index).StatusMasterID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
                mModelMonitorService = ModelMonitorService.GetModelMonitorService(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelMonitorServiceID, mMachine.HourType)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
            End If

            Session("mModelMonitorService") = mModelMonitorService
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            '    Response.Redirect("wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************

        Else
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType, True)
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
            Session("From") = 1 'Edit record
            ''
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                'End
            Else 'existing flow for spare assembly keep as it is
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
            End If
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus

            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            If mAssemblyMonitorServiceStatus.IsAttachmentAdded Then
                Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                mAircraft = mrptDueReport(Index).RegNo
                mMonitorInfo = mrptDueReport(Index).Type
                mMonitorType = mrptDueReport(Index).MonitorType
                mMonitorDesc = mrptDueReport(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date :" & mrptDueReport(Index).DoneOnDate & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                'End
            Else 'existing flow for spare assembly keep as it is
                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description
                mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineInfo
                mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo
                mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType
                mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(Index).Description
                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date :" & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnValueFormatted
            End If
            ' ''GetMachineMaintenance(mPrevAssemblyMonitorServiceStatus.ID)    'Added by Saylee on 7-Oct-2009
            RemoveSession()
            MarkLog(Util.Action.Edit, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub HistoryRecords(ByVal Index As Int32)  'Added by Saylee on 09-Sep-2009
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mAssemblyStatus As AssemblyStatus

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
            mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mMachine.HourType)
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
            Session("ATA") = mrptDueReport.Item(Index).ATAChapter.ToString
            Session("Description") = mrptDueReport.Item(Index).Description
            Session("ModelSerialNo") = mrptDueReport.Item(Index).ModelSerialNo
            mAircraft = mrptDueReport(Index).RegNo
            mMonitorInfo = mrptDueReport(Index).Type
            mMonitorType = mrptDueReport(Index).MonitorType
            mMonitorDesc = mrptDueReport(Index).Description
            'End
        Else 'existing flow for spare assembly keep as it is
            mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
            mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
            Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description
            Session("ATA") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString
            Session("Description") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description
            Session("ModelSerialNo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo
            mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineInfo
            mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo
            mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType
            mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(Index).Description
        End If
        'If mPrevAssemblyMonitorServiceStatus.IsMaster Then
        '    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Service Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyAssemblyMonitorServiceStatusList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)


        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************


        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.
                                                                GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID,
                                                                                                                 mAssemblyMonitorServiceStatus.ModelMonitorServiceID,
                                                                                                                 mMachine.HourType, TaskNo:=mAssemblyMonitorServiceStatus.ModelMonitorService.TaskCardNo)
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList


        'RemoveSession()
        'Added by Vikrant on 3-Aug-2011


        mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
        MarkLog(Util.Action.View, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ''MarkLog(Util.Action.Edit, "ComplyAssemblyMonitorServiceStatus", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryAssemblyMonitorServiceStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow()", True)
        'End If
    End Sub

    Private Sub DeleteRecord(ByVal Index As Integer)

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing

            mrptDueReport.CurrentIndex = Index
            Session("mrptDueReport") = mrptDueReport
            'End

            'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
            If chkApplicable.Checked And mrptDueReport(Index).ModelActivityCount > 1 Then

                MSGBoxCtrl.Show("Delete Alert!",
                                "You are trying to Delete a record which is already revised. ",
                                "Do you still want to continue?",
                                MsgBoxStyle.YesNo,
                                "Delete")

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                                MSGBox.Message_text.Delete,
                                "",
                                MsgBoxStyle.YesNo,
                                "Delete")

            End If

        Else 'existing flow for spare assembly keep as it is

            mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex = Index
            Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList

            'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
            If chkApplicable.Checked And mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelActivityCount > 1 Then

                MSGBoxCtrl.Show("Delete Alert!",
                                "You are trying to Delete a record which is already revised. ",
                                "Do you still want to continue?",
                                MsgBoxStyle.YesNo,
                                "Delete")

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                                MSGBox.Message_text.Delete,
                                "",
                                MsgBoxStyle.YesNo,
                                "Delete")

            End If


        End If

        MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                        MSGBox.Message_text.Delete,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

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

                                'Added by vikrant on 26-July-2011
                                IDForEventLog = mrptDueReport(mrptDueReport.CurrentIndex).ID
                                mAircraft = mrptDueReport(mrptDueReport.CurrentIndex).RegNo
                                mMonitorInfo = mrptDueReport(mrptDueReport.CurrentIndex).TypeDet
                                mMonitorType = mrptDueReport(mrptDueReport.CurrentIndex).MonitorType
                                mMonitorDesc = mrptDueReport(mrptDueReport.CurrentIndex).Description

                                mAssemblyDetails = mrptDueReport.Item(mrptDueReport.CurrentIndex).ModelName + "-" +
                                                   mrptDueReport.Item(mrptDueReport.CurrentIndex).SerialNo +
                                                   (IIf(mrptDueReport.Item(mrptDueReport.CurrentIndex).Position <> "",
                                                        " (" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Position + ")",
                                                        ""))

                                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo &
                                                         " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc &
                                                         " Done On Date :" & mrptDueReport(mrptDueReport.CurrentIndex).DoneOnDate &
                                                         " Done On Value : " & mrptDueReport(mrptDueReport.CurrentIndex).DoneAt2ForGrid

                                'mAssemblyMonitorDetailForMail = "Aircraft : " & mAircraft & "<br/> <b> Assembly Details : </b>" + mAssemblyDetails & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date :" & mrptDueReport(mrptDueReport.CurrentIndex).DoneOnDate & " Done On Value : " & mrptDueReport(mrptDueReport.CurrentIndex).DoneAt2ForGrid
                                mAssemblyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mMonitorDesc
                                'End
                                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mrptDueReport.CurrentItem.ID) 'Added by Saylee on 28-May-2009

                                If mrptDueReport.CurrentItem.IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mrptDueReport.CurrentItem.ID)
                                End If

                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mrptDueReport.CurrentItem.ID,
                                                                                               5) 'Added by Saylee on 6th-Oct-2009
                                AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(mrptDueReport.CurrentItem.ID)

                            Else 'existing flow for spare assembly keep as it is
                                'Added by vikrant on 26-July-2011

                                IDForEventLog = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID
                                mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).MachineInfo
                                mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).ModelMonitorServiceInfo
                                mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).MonitorType
                                mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).Description
                                mAssemblyDetails = mTmpComplyAssemblyMonitorServiceStatusList.Item(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyInfo
                                mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo &
                                                         " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc &
                                                         " Done On Date :" &
                                                         mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.
                                                                                                     CurrentIndex).DoneOnFormatted &
                                                        " Done On Value : " &
                                                        mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.
                                                                                                        CurrentIndex).DoneOnValueFormatted

                                'mAssemblyMonitorDetailForMail = "Aircraft : " & mAircraft & "<br/> <b> Assembly Details : </b>" + mAssemblyDetails & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date :" & mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).DoneOnValueFormatted
                                mAssemblyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" +
                                                                mAssemblyDetails + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo +
                                                                "<br/> <b>Description : </b>" + mMonitorDesc

                                'End
                                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.AssemblyMonitorServiceStatusID) 'Added by Saylee on 28-May-2009
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.AssemblyMonitorServiceStatusID, 5) 'Added by Saylee on 6th-Oct-2009

                                If mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID)
                                End If

                                AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.AssemblyMonitorServiceStatusID)

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

                            If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing

                                'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                                If AppSettings("LinkMaintenance") = "True" Then

                                    ' If LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.ModelMonitorServiceID.ToString).Count > 0 Then
                                    If LinkMaintenanceList.GetLinkMaintenanceList(mrptDueReport.CurrentItem.StatusMasterID.ToString).Count > 0 Then

                                        MSGBoxCtrl.Show("Alert !",
                                                        "<BR>Other Maintenance Activity(s) linked with this maintenance activity.
                                                                    To Edit/Delete individual Maintenance Activity go to respective activity.",
                                                        "",
                                                        MsgBoxStyle.OkOnly,
                                                        "LinkMaintenance")

                                        Exit Sub

                                    End If

                                End If
                                'End
                                'End
                            Else 'existing flow for spare assembly keep as it is
                                'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance

                                If AppSettings("LinkMaintenance") = "True" Then

                                    If LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.ModelMonitorServiceID.ToString).Count > 0 Then
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
                                MarkLog(Util.Action.Delete, "AssemblyServiceMonitor", "Can't delete :" & mAssemblyMonitorDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If

                            msgCount = ex.Errors.Count

                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try

                    ElseIf MSGBoxCtrl.Sender = "ReviseActivity" Then 'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity

                        MarkLog(Action:=Action.[New],
                                ModuleName:="Model Service",
                                Detail:="",
                                ErrorType:=ErrorType.NoError,
                                TransID:=Guid.Empty,
                                EventLogID)

                        Dim ID As Guid = Guid.NewGuid
                        Dim mModelMonitorService As ModelMonitorService
                        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = Session("mAssemblyMonitorServiceStatus")
                        Dim mModelMonitorServiceList As ModelMonitorServiceList

                        mMachine = Session("mMachine")
                        mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(ModelID:=mAssemblyMonitorServiceStatus.
                                                                                                                 ModelMonitorService.ModelID,
                                                                                                      GetRecordsByPreviousRefID:=True,
                                                                                                      PreviousRefID:=mAssemblyMonitorServiceStatus.
                                                                                                                        ModelMonitorService.
                                                                                                                            PreviousRefID.ToString())

                        If mModelMonitorServiceList.Count > 1 Then

                            For i As Integer = mModelMonitorServiceList.Count - 1 To 0 Step -1

                                If mModelMonitorServiceList(i).ID.Equals(mAssemblyMonitorServiceStatus.ModelMonitorService.ID) Then
                                    Exit For
                                Else
                                    Session("ModelIDFromModelCreation") = mAssemblyMonitorServiceStatus.ModelMonitorService.ModelID
                                    Session("ModelNameFromModelCreation") = mAssemblyMonitorServiceStatus.ModelMonitorService.Model.Name
                                    Session("mModelMonitorServiceList") = mModelMonitorServiceList
                                    Session("ModelMonitorServiceIDToBeLinked") = mModelMonitorServiceList(i).ID.ToString
                                    Session("ModelMonitorServicePreviousRefIDToBeLinked") = mModelMonitorServiceList(i).
                                                                                                    PreviousRefID.ToString()
                                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                                    Session("PreviousAssemblyMonitorServiceStatusForRevise") = mAssemblyMonitorServiceStatus

                                    ScriptManager.RegisterStartupScript(page:=Me,
                                                                        type:=[GetType],
                                                                        key:="OpenScript",
                                                                        script:="openledgersame('wfModelMonitorServiceList_Ajax.aspx?BackPage=Index.aspx');",
                                                                        addScriptTags:=True)

                                    Exit Sub

                                End If

                            Next

                        End If

                        mModelMonitorService = ModelMonitorService.NewModelMonitorService(OldModelMonitorService:=mAssemblyMonitorServiceStatus.
                                                                                                                    ModelMonitorService,
                                                                                          HourType:=mMachine.HourType)

                        Session("mModelMonitorService") = mModelMonitorService
                        RemoveSession()
                        mModelMonitorService.BeginEdit()
                        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                        Session("PreviousAssemblyMonitorServiceStatusForRevise") = mAssemblyMonitorServiceStatus
                        Session("IsLinkedActivitySelected") = True

                        ScriptManager.RegisterStartupScript(page:=Me,
                                                            type:=Me.GetType,
                                                            key:="Model Service Master",
                                                            script:="OpenModelServiceMasterWindow();",
                                                            addScriptTags:=True)

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
        End If

    End Sub
    Private Sub FindNow()
        RecordsToShow = dgDueMonitoringList.PageSize
        Session("RecordsToShow") = RecordsToShow

        dgDueMonitoringList.PageIndex = 0
        Session("DoneOn") = txtDate.Text
        Session("AircraftId") = cmbAircraftList.SelectedValue
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue
        Session("ShowNotApplicable") = chkApplicable.Checked  'Added by Saylee on 7-Jan-2011
        Session("SkipOneTimeDoneMRecords") = IIf(chkOneTimeMasterRecords.Checked, True, False)
        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
        Session("CodeFormNoDesc") = Trim(txtCodeFormNo.Text)

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, "", cmbAircraftAssembly.SelectedValue.ToString, 1, CInt(IIf(cmbMonitorType.SelectedIndex > 0, cmbMonitorType.SelectedValue, 0)), chkApplicable.Checked, chkOneTimeMasterRecords.Checked, txtCodeFormNo.Text.Trim)
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
            mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, IIf(mIsSpareAssembly = 1, Guid.Empty, cmbAircraftList.SelectedValue).ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , cmbMonitorType.SelectedValue, , , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareAssembly:=mIsSpareAssembly, AssemblyID:=cmbAircraftAssembly.SelectedValue)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo In mTmpComplyAssemblyMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
            End If
            Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
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
        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
    End Sub
    Private Sub SetPage()
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing

            Dim ServiceMPDTitle As String = ""

            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                lbltitle.Text = "List of Maintenance Event"
                ServiceMPDTitle = "Maintenance Event(s)"
                lblMonitorType.InnerText = "Task Type"
            Else
                lbltitle.Text = "List of Assembly Service Status"
                ServiceMPDTitle = "Assembly Service Status"
                lblMonitorType.InnerText = "Service Type"
            End If

            If RecordsToShow < mrptDueReport.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of " + ServiceMPDTitle + " as per selected criteria : " & RecordsToShow.ToString & " of " & mrptDueReport.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of " + ServiceMPDTitle + " as per selected criteria : " & mrptDueReport.Count & " Record(s) found."
            End If



            'End
        Else 'existing flow for spare assembly keep as it is
            If RecordsToShow < mTmpComplyAssemblyMonitorServiceStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Stock/Removed Assembly Service Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyAssemblyMonitorServiceStatusList.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Stock/Removed Assembly Service Status as per selected criteria : " & mTmpComplyAssemblyMonitorServiceStatusList.Count & " Record(s) found."
            End If
            lbltitle.Text = "List of Stock/Removed Assembly Service Status"
        End If

    End Sub
    Private Sub SetRights() 'Added By Prashant On 31-Mar-2011
        If (User.IsInRole("MachineAssemblyServiceNew")) = False Then
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
            If c = False Then
                dgDueMonitoringList.Rows(j).Cells(26).Enabled = False
            End If

            'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = False
                btnAddNewTop.Enabled = False
                btnAddNew.Enabled = False
                lblReadOnly.Visible = True
            Else
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = True
                btnAddNewTop.Enabled = True
                btnAddNew.Enabled = True
                lblReadOnly.Visible = False
            End If
            '*************************
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

        Try

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

                    Dim List = (From StatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo In
                                    mTmpComplyAssemblyMonitorServiceStatusList
                                Select StatusInfo).ToList.Take(RecordsToShow)
                    dgDueMonitoringList.DataSource = List
                Else
                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                End If

            End If

            dgDueMonitoringList.DataBind()
            SetGrid()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Public Sub SendMail(mAssemblyMonitorDetailForMail)
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'If AppSettings("MailsRequire") = "True" Then
        If mModuleList.Item("AssemblyServiceMonitor").MailsRequire = True Then
            If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
                'Do nothing
                Exit Sub
            End If
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Task Details :  <br/> <br/>  " & mAssemblyMonitorDetailForMail & " <br/> <b> Deleted by User:</b> " + User.Identity.Name + "<b> on: </b>" + New SmartDate(Today.Date).FormattedText + "</font></P> ")
            str = str + ("</body></html>")
            'SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Assembly Inspections Deleted", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), Info:=str, ToMailID:=mModuleList.Item("Order").SendToMailID, Remark:=Session("SendMailRemark"), ReportGenratedBy:=Session("ReportGenratedBy"))

            SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Task Deleted", Info:=str, ToMailID:=mModuleList.Item("AssemblyServiceMonitor").SendToMailID, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
        End If
    End Sub

    Private Sub ReviseRecord(Index As Integer)

        Dim mPreviousAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus

        Try

            If mIsSpareAssembly = 0 Then

                mMachine = Machine.GetMachine(mrptDueReport.Item(Index).MachineID)

                mPreviousAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.
                                                            GetAssemblyMonitorServiceStatus(ID:=mrptDueReport.Item(Index).ID,
                                                                                            AssemblyStatusID:=mrptDueReport.Item(Index).
                                                                                                                AssemblyStatusID,
                                                                                            HourType:=mMachine.HourType)

            Else

                mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineID)

                mPreviousAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.
                                                            GetAssemblyMonitorServiceStatus(ID:=mTmpComplyAssemblyMonitorServiceStatusList.
                                                                                                    Item(Index).AssemblyMonitorServiceStatusID,
                                                                                            AssemblyStatusID:=mTmpComplyAssemblyMonitorServiceStatusList.
                                                                                                    Item(Index).AssemblyStatusID,
                                                                                            HourType:=mMachine.HourType)

            End If

            Session("mAssemblyMonitorServiceStatus") = mPreviousAssemblyMonitorServiceStatus
            Session("PreviousAssemblyMonitorServiceStatus") = mPreviousAssemblyMonitorServiceStatus
            Session("From") = 1

            Dim mAssemblyStatus As AssemblyStatus

            If mIsSpareAssembly = 0 Then
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            Else
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
            End If

            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPreviousAssemblyMonitorServiceStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus

            If mIsSpareAssembly = 0 Then

                Session("mAssemblyInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + " -> " +
                                                mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + " -> " +
                                                mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description

            Else

                Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + " -> " +
                                                mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + " -> " +
                                                mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + " -> " +
                                                mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + " -> " +
                                                mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString() + " -> " +
                                                mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description

            End If

            Session("From") = 1
            Session("RevisedFromListPage") = "True"
            Session("NewPage") = "True"

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
    Private Sub SetMachineMaintenanceObject(mMachineMaintenance As MachineMaintenance,
                                            CurrentAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus)

        Try

            With mMachineMaintenance

                Dim mLog As Log

                mMachine = Session("mMachine")

                If Not mLog Is Nothing Then

                    .LogNo = mLog.LogNo
                    .LogID = mLog.ID
                    .LogPageNo = mLog.LogPageNo
                    Session.Remove("mLog")

                Else

                    Dim mMaxLogNo As MaxLogNo

                    mMaxLogNo = MaxLogNo.GetMaxLogNo(mMachineMaintenance.Date,
                                                     mMachineMaintenance.MachineID,
                                                     CurrentAssemblyMonitorServiceStatus.AssemblyID)

                    If mMaxLogNo.Count <> 0 Then

                        .LogNo = mMaxLogNo(0).LogNo
                        .LogID = mMaxLogNo(0).LogId
                        .LogPageNo = mMaxLogNo(0).LogPageNo

                    Else

                        mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(mMachineMaintenance.Date, mMachine.ID)

                        If mMaxLogNo.Count <> 0 Then
                            .LogNo = mMaxLogNo(0).LogNo
                            .LogID = mMaxLogNo(0).LogId
                            .LogPageNo = mMaxLogNo(0).LogPageNo
                        End If

                    End If


                End If

            End With

            If mMachineMaintenance.IsValid = True Then

                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance

            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub


#End Region

#Region " Data Binding "

    Private Sub DataFieldBind(Optional ByVal SkipOneTimeDoneMasterRecords As Boolean = False)

        If IsNothing(DoneOn) Then

            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DoneOn = Today.Date.ToString(AppSettings("DateFormat")) 'Added By Saylee on 29-Apr-2009

        Else
            txtDate.Text = CDate(DoneOn).ToString(AppSettings("DateFormat"))
        End If

        Session("DoneOn") = txtDate.Text
        txtDate.DataBind()

        Dim mMachineId As Guid = Guid.Empty

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString,
                                                                    SkipIsForInventoryAircarft:=True)

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
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0,
                                                                cmbAircraftList.SelectedValue,
                                                                txtDate.Text.ToString,
                                                                "(All)",
                                                                True,
                                                                IsForSpareAssembly:=mIsSpareAssembly)

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

            mrptDueReport = rptDueReport.GetList(DoneOn,
                                                 cmbAircraftList.SelectedItem.ToString, ,
                                                 True,
                                                 "",
                                                 cmbAircraftAssembly.SelectedValue.ToString,
                                                 1,
                                                 CInt(MonitorTypeID),
                                                 ShowNotApplicable,
                                                 chkOneTimeMasterRecords.Checked, CodeFormNoDesc)

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

            mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.
                                                            GetDueMonitorServiceList(DoneOn,
                                                                                     IIf(mIsSpareAssembly = 1,
                                                                                                  Guid.Empty,
                                                                                                  cmbAircraftList.SelectedValue).ToString,
                                                                                     IIf(cmbAircraftAssembly.SelectedIndex > 0,
                                                                                               mAssemblylist(cmbAircraftAssembly.
                                                                                                                            SelectedIndex).ModelName,
                                                                                               ""),
                                                                                     IIf(cmbAircraftAssembly.SelectedIndex > 0,
                                                                                                 mAssemblylist(cmbAircraftAssembly.
                                                                                                                        SelectedIndex).SerialNo,
                                                                                                 ""), , , ,
                                                                                     CType(MonitorTypeID, Integer), , ,
                                                                                     ShowNotApplicable,
                                                                                     IIf(chkOneTimeMasterRecords.Checked,
                                                                                                                False,
                                                                                                                True),
                                                                                     SortBy:="MinimumRemainingValue",
                                                                                     CodeFormNoDesc:=CodeFormNoDesc,
                                                                                     IsSpareAssembly:=CBool(mIsSpareAssembly),
                                                                                     AssemblyID:=cmbAircraftAssembly.SelectedValue)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then

                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorServiceStatusList.
                                                tmpComplyAssemblyMonitorServiceStatusInfo In mTmpComplyAssemblyMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)

                dgDueMonitoringList.DataSource = List

            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
            End If

            Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList

        End If

        dgDueMonitoringList.DataBind()  'Added Code

        chkApplicable.Checked = ShowNotApplicable 'Added by Saylee on 7-Jan-2011

        'Added by Saylee on 30-July-2009
        mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("(All)")
        cmbMonitorType.DataSource = mModelMonitorServiceTypeList

        If IsNothing(MonitorTypeID) Or MonitorTypeID = "" Then
            'Do nothing
        Else
            cmbMonitorType.SelectedValue = MonitorTypeID
        End If

        cmbMonitorType.DataBind()
        Session("MonitorTypeID") = MonitorTypeID
        chkApplicable.Checked = IIf(ShowNotApplicable, True, False)
        'Added by Saylee on 26-Aug-2020 for All27072020

        If (mIsSpareAssembly = 1) Then

            Dim da As New ObjectAdapter
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

            cmbAircraftList.Focus()
            ' 'Added by Saylee on 26-Aug-2020 for All27082020
            mIsSpareAssembly = Request.QueryString("SpareAssembly")
            Session("mIsSpareAssembly") = mIsSpareAssembly
            '************************

            Session("MiddleFrame") = "wfComplyAssemblyMonitorServiceStatusList_Ajax.aspx?SpareAssembly=" & mIsSpareAssembly  ' 'mIsSpareAssembly Added by Saylee on 26-Aug-2020 for All27082020
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            DataFieldBind(True)
            SetPage()
            SetRights()
            SetGrid()
            ControlVisibility()
        End If



        If AppSettings("ShowMaintenanceForNewClients") = "True" Then

            lblCodeFormNo.InnerText = "Task No./Description"
            dgDueMonitoringList.HeaderRow.Cells(8).Text = "Description"
            dgDueMonitoringList.Columns(8).HeaderText = "Description"
            dgDueMonitoringList.Columns(5).HeaderText = "Task Type"
            dgDueMonitoringList.HeaderRow.Cells(5).Text = "Task Type"
            dgDueMonitoringList.Columns(0).Visible = True

        Else

            dgDueMonitoringList.HeaderRow.Cells(8).Text = "Code/Form No./Description"
            dgDueMonitoringList.Columns(8).HeaderText = "Code/Form No./Description"
            lblCodeFormNo.InnerText = "Code/Form No./Description"
            dgDueMonitoringList.Columns(5).HeaderText = "Service Type"
            dgDueMonitoringList.HeaderRow.Cells(5).Text = "Service Type"
            dgDueMonitoringList.Columns(0).Visible = False

        End If

    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session.Remove("DoneOn")
        Session.Remove("AircraftId")
        Session.Remove("From")
        Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
        Session.Remove("AssemblyId")
        Session.Remove("SkipOneTimeDoneMRecords")
        Session.Remove("ATA")
        Session("MiddleFrame") = ""
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
        FindNow()
        ControlVisibility()
        SetPage()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub

    Private Sub GridViewRowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgDueMonitoringList.RowCommand

        Select Case e.CommandName

            Case "Comply"

                If Not User.IsInRole("AssemblyServiceMonitorNew") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                ComplyRecord(CInt(e.CommandArgument))

            Case "EditRec"

                If (Not User.IsInRole("AssemblyServiceMonitorView") And Not User.IsInRole("AssemblyServiceMonitorEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                EditRecord(CInt(e.CommandArgument))

            Case "DeleteRec"

                If (Not User.IsInRole("AssemblyServiceMonitorDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                DeleteRecord(CInt(e.CommandArgument))

            Case "History" 'Added by Saylee on 09-Sep-2009

                If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                HistoryRecords(CInt(e.CommandArgument))

            Case "ViewRec"

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(CInt(e.CommandArgument)).ID)
                    'End
                Else 'existing flow for spare assembly keep as it is
                    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorServiceStatusList(CInt(e.CommandArgument)).ID)
                End If

                Session("mFileAttach") = mFileAttach
                GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)

                If mFileAttach.Size > 0 Then

                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then

                        'Delete File if exist
                        File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = ""
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)

                    End If

                End If

            Case "Revise" 'Added by Saylee on 27-Jul-2023, to give Revise on comply list page

                If (Not User.IsInRole("AssemblyServiceMonitorView") And Not User.IsInRole("AssemblyServiceMonitorEdit")) Then

                    mAircraft = cmbAircraftList.SelectedItem.Text

                    If mIsSpareAssembly = 0 Then

                        mMonitorType = ""
                        mMonitorInfo = mrptDueReport(CInt(e.CommandArgument)).Code
                        mMonitorDesc = mrptDueReport(CInt(e.CommandArgument)).Code_Desc


                    Else

                        mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(CInt(e.CommandArgument)).MonitorType
                        mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(CInt(e.CommandArgument)).ModelMonitorServiceCode
                        mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(CInt(e.CommandArgument)).Code_Desc

                    End If

                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo &
                                             " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc

                    MarkLog(Action.Edit,
                            "Assembly Service",
                            User.Identity.Name & " is not Authorized User to edit " & mAssemblyMonitorDetail,
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Exit Sub

                End If

                RecordsToShow = dgDueMonitoringList.PageSize
                Session("RecordsToShow") = RecordsToShow
                Session("mrptDueReport") = mrptDueReport

                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                dgDueMonitoringList.Columns(27).Visible = IIf(chkApplicable.Checked, False, True)
                ReviseRecord(CInt(e.CommandArgument))

                MSGBoxCtrl.Show("Alert!",
                                "You are about to Revise Model Activity. 
                                           After revision of model activity this Status will become Not Applicable.",
                                "Do you want to continue?",
                                MsgBoxStyle.YesNo,
                                "ReviseActivity")

        End Select

    End Sub
    'Private Sub dgDueMonitoringList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDueMonitoringList.PageIndexChanged
    '    dgDueMonitoringList.PageIndex = e.NewPageIndex
    '    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
    '    Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
    '    dgDueMonitoringList.DataBind()
    '    SetGrid()
    'End Sub
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click, btnAddNew.Click
        If IsValid Then
            Session("AircraftIdForService") = cmbAircraftList.SelectedValue.ToString
            'Added by Vikrant on 26-July-2011
            MarkLog(Util.Action.[New], "AssemblyServiceMonitor", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorServiceStatusListNew.aspx?BackPage=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAssemblyServiceListNewWindow", "OpenAssemblyServiceListNewWindow()", True)
            Session("NewPage") = "True"
        End If
    End Sub
    'New addition by Rupali on 22-Jun-09 for Sorting Order
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
            mTmpComplyAssemblyMonitorServiceStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo In mTmpComplyAssemblyMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
            End If
            Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
        End If
        dgDueMonitoringList.DataBind()
        SetGrid()
    End Sub
    Private Sub hdnBtnServiceHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnServiceHistory.Click
        FindNow()
        ControlVisibility()
        SetPage()
        upnlgrid.Update()
    End Sub
    Protected Sub ScriptManager1_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs)
        If (e.Exception.Data("ExtraInfo") <> Nothing) Then
            ScriptManager1.AsyncPostBackErrorMessage =
               e.Exception.Message &
               e.Exception.Data("ExtraInfo").ToString()
        Else
            ScriptManager1.AsyncPostBackErrorMessage =
               "An unspecified error occurred."
        End If
    End Sub
    Private Sub lnkShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAllRecords.Click, lnkShowAllRecordsTop.Click
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            dgDueMonitoringList.DataSource = mrptDueReport
            RecordsToShow = mrptDueReport.Count
            'End
        Else 'existing flow for spare assembly keep as it is
            dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
            RecordsToShow = mTmpComplyAssemblyMonitorServiceStatusList.Count
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

    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
    Private Sub ModelServiceMaster(sender As Object, e As EventArgs) Handles hdnBtnModelServiceMaster.Click

        Try

            If Session("PreviousAssemblyMonitorServiceStatusForRevise") IsNot Nothing Then

                Dim mPreviousAssemblyMonitorServiceStatusForRevise As AssemblyMonitorServiceStatus
                Dim mCurrentAssemblyMonitorServiceStatusForRevise As AssemblyMonitorServiceStatus
                Dim mMachineMaintenance As MachineMaintenance

                mPreviousAssemblyMonitorServiceStatusForRevise = Session("PreviousAssemblyMonitorServiceStatusForRevise")
                mCurrentAssemblyMonitorServiceStatusForRevise = Session("mAssemblyMonitorServiceStatus")
                mPreviousAssemblyMonitorServiceStatusForRevise.IsApplicable = False
                mMachine = Session("mMachine")

                mPreviousAssemblyMonitorServiceStatusForRevise.Save()

                Session.Remove("PreviousAssemblyMonitorServiceStatusForRevise")
                Session.Remove("RevisedFromListPage")

                If mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString = "" Then
                    mCurrentAssemblyMonitorServiceStatusForRevise.AsOnDate = mPreviousAssemblyMonitorServiceStatusForRevise.AsOnDateFormatted.ToString
                Else
                    mCurrentAssemblyMonitorServiceStatusForRevise.AsOnDate = mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString
                End If

                For i As Integer = 0 To mPreviousAssemblyMonitorServiceStatusForRevise.AssemblyMonitorServiceStatusPeriods.Count - 1

                    Dim PeriodID = mPreviousAssemblyMonitorServiceStatusForRevise.AssemblyMonitorServiceStatusPeriods(i).PeriodID

                    If mCurrentAssemblyMonitorServiceStatusForRevise.AssemblyMonitorServiceStatusPeriods.Contains(PeriodID, "") Then

                        mCurrentAssemblyMonitorServiceStatusForRevise.AssemblyMonitorServiceStatusPeriods.Item(PeriodID, "").DoneOnValue =
                            mPreviousAssemblyMonitorServiceStatusForRevise.AssemblyMonitorServiceStatusPeriods(i).DoneOnValue

                    End If

                Next

                mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mMachine.ID,
                                                                               5,
                                                                               mCurrentAssemblyMonitorServiceStatusForRevise.AsOnDate,
                                                                               mCurrentAssemblyMonitorServiceStatusForRevise.ID,
                                                                               Guid.Empty,
                                                                               0,
                                                                               0,
                                                                               mCurrentAssemblyMonitorServiceStatusForRevise.
                                                                                                AssemblyStatusID)

                mMachineMaintenance.MaintenanceID = mCurrentAssemblyMonitorServiceStatusForRevise.ID
                mCurrentAssemblyMonitorServiceStatusForRevise.IsMaster = False

                mCurrentAssemblyMonitorServiceStatusForRevise.Save()

                SetMachineMaintenanceObject(mMachineMaintenance, mCurrentAssemblyMonitorServiceStatusForRevise)

                RecordsToShow = dgDueMonitoringList.PageSize

                Session("RecordsToShow") = RecordsToShow
                Session.Remove("mAssemblyMonitorServiceStatus")

            End If

            FindNow()
            SetPage()
            upnlgrid.Update()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

#Region " Report "
    ' Created by - Rajnish on 22-06-2006 
#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Private SearchStr4 As String = ""
    Dim ShowNotApplicable As Boolean = False
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If (Not User.IsInRole("AssemblyServiceMonitorPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            dgDueMonitoringList.DataSource = mrptDueReport
            'End
        Else 'existing flow for spare assembly keep as it is
            dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
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
        'ReportDetails.Add(New rptStatus(, 0, dgDueMonitoringList.CaptionText))
        ReportDetails.Add(New rptStatus(, 1, ,
             , , , dgDueMonitoringList.Columns.Item(0).HeaderText, , dgDueMonitoringList.Columns.Item(4).HeaderText, dgDueMonitoringList.Columns.Item(6).HeaderText,
             dgDueMonitoringList.Columns.Item(7).HeaderText, dgDueMonitoringList.Columns.Item(8).HeaderText,
             dgDueMonitoringList.Columns.Item(9).HeaderText, dgDueMonitoringList.Columns.Item(10).HeaderText, dgDueMonitoringList.Columns.Item(11).HeaderText,
             dgDueMonitoringList.Columns.Item(12).HeaderText, dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText,
             dgDueMonitoringList.Columns.Item(15).HeaderText, dgDueMonitoringList.Columns.Item(16).HeaderText, dgDueMonitoringList.Columns.Item(17).HeaderText,
             , , , , , , , , , dgDueMonitoringList.Columns.Item(18).HeaderText))

        Dim TotalCount As Integer
        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            TotalCount = Me.mrptDueReport.Count
            'End
        Else 'existing flow for spare assembly keep as it is
            TotalCount = Me.mTmpComplyAssemblyMonitorServiceStatusList.Count
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

            If Me.dgDueMonitoringList.Rows(I).Cells(0).Text <> "&nbsp;" Then str(0) = Me.dgDueMonitoringList.Rows(I).Cells(0).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(4).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgDueMonitoringList.Rows(I).Cells(7).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgDueMonitoringList.Rows(I).Cells(8).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgDueMonitoringList.Rows(I).Cells(9).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgDueMonitoringList.Rows(I).Cells(10).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgDueMonitoringList.Rows(I).Cells(11).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(8) = Me.dgDueMonitoringList.Rows(I).Cells(12).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(13).Text <> "&nbsp;" Then str(9) = Me.dgDueMonitoringList.Rows(I).Cells(13).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(14).Text <> "&nbsp;" Then str(10) = Me.dgDueMonitoringList.Rows(I).Cells(14).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(15).Text <> "&nbsp;" Then str(11) = Me.dgDueMonitoringList.Rows(I).Cells(15).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(16).Text <> "&nbsp;" Then str(12) = Me.dgDueMonitoringList.Rows(I).Cells(16).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(17).Text <> "&nbsp;" Then str(13) = Me.dgDueMonitoringList.Rows(I).Cells(17).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(18).Text <> "&nbsp;" Then str(14) = Me.dgDueMonitoringList.Rows(I).Cells(18).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 2, ,
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6),
             str(7), str(8), str(9), str(10), str(11), str(12), str(13), , , , , , , , , , str(14)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
   mCompanyDetail.WebSite, "List of Comply Assembly Service Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mIsSpareAssembly = 0 Then 'Added By Shital for faster processing
            If mrptDueReport.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            'End
        Else 'existing flow for spare assembly keep as it is
            If mTmpComplyAssemblyMonitorServiceStatusList.Count = 0 Then
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
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#End Region

End Class