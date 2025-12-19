'AJAX Conversion By Vikrant On 18-Mar-2015
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Public Class wfComplyAssemblyMonitorInspStatusListShowValues_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mMachineNameValueList As MachineNameValueList
    'Private mTmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
    Protected mAssemblyMonitorInspStatusListNew As AssemblyMonitorInspStatusList 'MPD Slow
    Private DoneOn As String
    Private AircraftId As String
    Public mMachine As Machine
    Public mBoardInfo As AircraftInformationBoard.BoardInfo  'Added by Saylee on 22-May-2009

    Private mModelMonitorInspTypeList As ModelMonitorInspTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String 'Added by Saylee on 30-July-2009

    'Added by Saylee on 09-Sep-2009
    Private mUpdateComplyHistoryAssemblyMonitorInspStatusList As UpdateComplyHistoryAssemblyMonitorInspStatusList

    'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance
    Dim ShowNotApplicable As Boolean = False

    'Added by Vikrant on 26-July-2011
    Dim EventLogID As Guid
    Public mAssemblyMonitorDetail As String
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mMonitorDesc As String
    Dim IDForEventLog As Guid
    'Added By Prashant On 27-Nov-2014
    Dim mFileAttach As FileAttach
    Dim mAssemblylist As AssemblyList  'Added By Prahsnat 15-Jun-2015 
    Private AssemblyId As String
    Dim SkipOneTimeDoneMRecords As Boolean = False
    Dim RecordsToShow As Integer
    Dim IsReadOnly As Boolean 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Dim CodeFormNoDesc As String
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
        'mTmpComplyAssemblyMonitorInspStatusList = CType(Session("mTmpComplyAssemblyMonitorInspStatusList"), tmpComplyAssemblyMonitorInspStatusList)
        mAssemblyMonitorInspStatusListNew = Session("mAssemblyMonitorInspStatusListNew") 'MPD Slow
        DoneOn = Session("DoneOn")
        AircraftId = Session("AircraftId")
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        ShowNotApplicable = CType(Session("ShowNotApplicable"), Boolean) 'Added by Saylee on 7th-Jan-2011
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AssemblyId = CType(Session("AssemblyId"), String)
        SkipOneTimeDoneMRecords = CType(Session("SkipOneTimeDoneMRecords"), Boolean)
        'RecordsToShow = CType(IIf(Session("RecordsToShow") Is Nothing, dgDueMonitoringList.PageSize, Session("RecordsToShow")), Integer)
        RecordsToShow = CType(Session("RecordsToShow"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        CodeFormNoDesc = Session("CodeFormNoDesc")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        'Session.Remove("mTmpComplyAssemblyMonitorInspStatusList")
        Session.Remove("mAssemblyMonitorInspStatusListNew")
        Session.Remove("RecordsToShow")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyAssemblyMonitorInspStatusListShowValues_Ajax.aspx?" Then
            'Session.Remove("mTmpComplyAssemblyMonitorInspStatusList")
            Session.Remove("mAssemblyMonitorInspStatusListNew")
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
        End If
    End Sub
    Private Sub ControlVisibility()
        'btnPrint.Enabled = (mTmpComplyAssemblyMonitorInspStatusList.Count > 0)
        'btnPrintTop.Enabled = (mTmpComplyAssemblyMonitorInspStatusList.Count > 0)
        btnPrint.Enabled = (mAssemblyMonitorInspStatusListNew.Count > 0)
        btnPrintTop.Enabled = (mAssemblyMonitorInspStatusListNew.Count > 0)
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
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
        'If RecordsToShow < mTmpComplyAssemblyMonitorInspStatusList.Count Then

        If Not mAssemblyMonitorInspStatusListNew Is Nothing Then
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                     Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                   Select StatusInfo).ToList
            If RecordsToShow < List.Count Then
                lnkLoadMore.Enabled = True
                lnkLoadMoreTop.Enabled = True
            Else
                lnkLoadMore.Enabled = False
                lnkLoadMoreTop.Enabled = False
            End If
        End If

    End Sub
    Private Sub ComplyRecord(ByVal ID As Guid)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorInspStatusListNew(Index).AssemblyStatusID)
        'Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mAssemblyMonitorInspStatusListNew(ID).ID, mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID, mMachine.HourType)
        If (mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And (mPrevAssemblyMonitorInspStatus.IsCompleted Or mPrevAssemblyMonitorInspStatus.FetchRecordCount(mPrevAssemblyMonitorInspStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            'mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, txtDate.Text, mTmpComplyAssemblyMonitorInspStatusList(Index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, txtDate.Text, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("From") = 0 'New record
            ''
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(Index).MachineID)
            mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

            'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID)
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

            'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
            ''Added by Vikrant on 26-July-2011
            'mAircraft = mTmpComplyAssemblyMonitorInspStatusList(Index).MachineInfo
            'mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(Index).ModelMonitorInspInfo
            'mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(Index).MonitorType
            'mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(Index).Description
            'mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date : " & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnValueFormatted
            'MarkLog(Util.Action.Comply, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)

            Dim DoneOnValue As String
            For i As Integer = 0 To mPrevAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
                If i = 0 Then
                    DoneOnValue = mPrevAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(i).DoneOnValueFormatted
                Else
                    DoneOnValue = DoneOnValue + " " + mPrevAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(i).DoneOnValueFormatted
                End If
            Next
            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorInspStatusListNew(ID).Reference + "->" + mAssemblyMonitorInspStatusListNew(ID).Type + "->" + mAssemblyMonitorInspStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorInspStatusListNew(ID).Description
            'Added by Vikrant on 26-July-2011
            mAircraft = cmbAircraftList.SelectedItem.ToString
            mMonitorInfo = mAssemblyMonitorInspStatusListNew(ID).Type
            mMonitorType = mAssemblyMonitorInspStatusListNew(ID).MonitorType
            mMonitorDesc = mAssemblyMonitorInspStatusListNew(ID).Description
            mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date : " & mAssemblyMonitorInspStatusListNew(ID).DoneOnFormatted.ToString & " Done On Value : " & DoneOnValue
            MarkLog(Util.Action.Comply, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatusListNew(ID).ID, EventLogID)
            'End

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        'Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mAssemblyMonitorInspStatusListNew(ID).ID, mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID, mMachine.HourType)

        If mPrevAssemblyMonitorInspStatus.IsMaster And mPrevAssemblyMonitorInspStatus.IsApplicable And chkApplicable.Checked = False Then
            ' MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit the record.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mPrevAssemblyMonitorInspStatus.IsMaster) And (Not mPrevAssemblyMonitorInspStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records
            Session("mAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("From") = 1 'Edit record
            ''
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(Index).MachineID)
            'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID)

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

            'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorInspStatusListNew(ID).Reference + "->" + mAssemblyMonitorInspStatusListNew(ID).Type + "->" + mAssemblyMonitorInspStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorInspStatusListNew(ID).Description

            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)

            '**********************************************************************
            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevAssemblyMonitorInspStatus.IsMaster = False) And (mPrevAssemblyMonitorInspStatus.IsCompleted = False) And mPrevAssemblyMonitorInspStatus.IsDone = False) Then

            'mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mMachine.HourType)

            Dim mModelMonitorInsp As ModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mAssemblyMonitorInspStatusListNew(ID).ModelMonitorInspID, mMachine.HourType)
            Session("mModelMonitorInsp") = mModelMonitorInsp

            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID)
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
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID)

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

            'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
            ''Added by Vikrant on 26-July-2011
            'mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(Index).ModelMonitorInspInfo
            'mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(Index).MonitorType
            'mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(Index).Description
            'mAssemblyMonitorDetail = "Aircraft : " + cmbAircraftList.SelectedItem.Text + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date :" & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorInspStatusList(Index).DoneOnValueFormatted
            'MarkLog(Util.Action.Edit, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)
            Dim DoneOnValue As String
            For i As Integer = 0 To mPrevAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
                If i = 0 Then
                    DoneOnValue = mPrevAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(i).DoneOnValueFormatted
                Else
                    DoneOnValue = DoneOnValue + " " + mPrevAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(i).DoneOnValueFormatted
                End If
            Next
            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorInspStatusListNew(ID).Reference + "->" + mAssemblyMonitorInspStatusListNew(ID).Type + "->" + mAssemblyMonitorInspStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorInspStatusListNew(ID).Description
            'Added by Vikrant on 26-July-2011
            mAircraft = cmbAircraftList.SelectedItem.ToString
            mMonitorInfo = mAssemblyMonitorInspStatusListNew(ID).Type
            mMonitorType = mAssemblyMonitorInspStatusListNew(ID).MonitorType
            mMonitorDesc = mAssemblyMonitorInspStatusListNew(ID).Description
            mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc & " Done On Date : " & mAssemblyMonitorInspStatusListNew(ID).DoneOnFormatted.ToString & " Done On Value : " & DoneOnValue
            MarkLog(Util.Action.Comply, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatusListNew(ID).ID, EventLogID)
            'End
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub HistoryRecords(ByVal ID As Guid) 'Added by Saylee on 09-Sep-2009
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        'Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mAssemblyMonitorInspStatusListNew(ID).ID, mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID, mMachine.HourType)

        'If mPrevAssemblyMonitorInspStatus.IsMaster Then
        '    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Inspection Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyAssemblyMonitorInspStatusListShowValues_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(Index).MachineID)
        'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorInspStatusListNew(ID).AssemblyStatusID)

        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
        'Session("ATA") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ATA.ToString
        'Session("Description") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).Description
        'Session("ModelSerialNo") = mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelSerialNo

        Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.Assembly.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorInspStatusListNew(ID).Reference + "->" + mAssemblyMonitorInspStatusListNew(ID).Type + "->" + mAssemblyMonitorInspStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorInspStatusListNew(ID).Description
        Session("ATA") = mAssemblyMonitorInspStatusListNew(ID).ATACode.ToString
        Session("Description") = mAssemblyMonitorInspStatusListNew(ID).Description
        Session("ModelSerialNo") = "[Model: " & mAssemblyStatus.Assembly.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"

        mUpdateComplyHistoryAssemblyMonitorInspStatusList = UpdateComplyHistoryAssemblyMonitorInspStatusList.GetComplyHistoryAssemblyMonitorInspStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorInspStatus.ModelMonitorInspID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList

        'RemoveSession()
        'Added by Vikrant on 3-Aug-2011
        mAircraft = cmbAircraftList.SelectedItem.Text
        'mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).ModelMonitorInspInfo
        'mMonitorType = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).MonitorType
        'mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).Description
        mMonitorInfo = mAssemblyMonitorInspStatusListNew(ID).Type
        mMonitorType = mAssemblyMonitorInspStatusListNew(ID).MonitorType
        mMonitorDesc = mAssemblyMonitorInspStatusListNew(ID).Description
        mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc

        MarkLog(Util.Action.View, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfUpdateComplyHistoryAssemblyMonitorInspStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionHistoryWindow", "OpenInspectionHistoryWindow()", True)
        'End If
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        'Revise Activity
        'If chkApplicable.Checked And mTmpComplyAssemblyMonitorInspStatusList(Index).ModelActivityCount > 1 Then 'Revise Activity
        If chkApplicable.Checked And mAssemblyMonitorInspStatusListNew(ID).ModelActivityCount > 1 Then 'Revise Activity
            MSGBoxCtrl.show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        End If

        'mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex = Index
        'Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        mAssemblyMonitorInspStatusListNew.CurrentIndex = mAssemblyMonitorInspStatusListNew(ID, "")
        Session("mAssemblyMonitorInspStatusListNew") = mAssemblyMonitorInspStatusListNew
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
                            'Session("sender") = ""
                            '''Added by Vikrant on 26-July-2011
                            ''mAircraft = cmbAircraftList.SelectedItem.Text
                            'IDForEventLog = mTmpComplyAssemblyMonitorInspStatusList(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID
                            'mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).ModelMonitorInspInfo
                            'mMonitorType = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).MonitorType
                            'mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList.Item(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).Description
                            'mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc
                            ''End
                            ''Added by Saylee on 28-May-2009
                            'mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.AssemblyMonitorInspStatusID)
                            ''********************************
                            'If mTmpComplyAssemblyMonitorInspStatusList(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).IsAttachmentAdded = True Then
                            '    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorInspStatusList(mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID)
                            'End If
                            ''Added by Saylee on 9th-Oct-2009
                            'mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.AssemblyMonitorInspStatusID, 6)
                            ''=============================

                            'AssemblyMonitorInspStatus.DeleteAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.AssemblyMonitorInspStatusID)
                            'MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            'If Not mFileAttach Is Nothing Then
                            '    If mFileAttach.Size > 0 Then
                            '        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                            '    End If
                            'End If

                            'Session("mMachineMaintenance") = mMachineMaintenance
                            ''Added by Saylee on 28-May-2009
                            'mBoardInfo.IsComplyDelete = True
                            'mBoardInfo.ApplyEdit()
                            'mBoardInfo.Save()
                            'Session("mAircraftInformationBoardList") = Nothing
                            ''********************************
                            ''Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                            'If AppSettings("LinkMaintenance") = "True" Then
                            '    If LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorInspStatusList.CurrentItem.ModelMonitorInspID.ToString).Count > 0 Then
                            '        MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                            '        Exit Sub
                            '    End If
                            'End If
                            ''End
                            Session("sender") = ""
                            ''Added by Vikrant on 26-July-2011
                            mAircraft = cmbAircraftList.SelectedItem.Text
                            IDForEventLog = mAssemblyMonitorInspStatusListNew(mAssemblyMonitorInspStatusListNew.CurrentIndex).ID
                            mMonitorInfo = mAssemblyMonitorInspStatusListNew.Item(mAssemblyMonitorInspStatusListNew.CurrentIndex).Type
                            mMonitorType = mAssemblyMonitorInspStatusListNew.Item(mAssemblyMonitorInspStatusListNew.CurrentIndex).MonitorType
                            mMonitorDesc = mAssemblyMonitorInspStatusListNew.Item(mAssemblyMonitorInspStatusListNew.CurrentIndex).Description
                            mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mAssemblyMonitorInspStatusListNew(mAssemblyMonitorInspStatusListNew.CurrentIndex).ID)
                            '********************************
                            If mAssemblyMonitorInspStatusListNew(mAssemblyMonitorInspStatusListNew.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatusListNew(mAssemblyMonitorInspStatusListNew.CurrentIndex).ID)
                            End If
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatusListNew(mAssemblyMonitorInspStatusListNew.CurrentIndex).ID, 6)
                            '=============================

                            AssemblyMonitorInspStatus.DeleteAssemblyMonitorInspStatus(mAssemblyMonitorInspStatusListNew(mAssemblyMonitorInspStatusListNew.CurrentIndex).ID)
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
                                If LinkMaintenanceList.GetLinkMaintenanceList(mAssemblyMonitorInspStatusListNew(mAssemblyMonitorInspStatusListNew.CurrentIndex).ModelMonitorInspID.ToString).Count > 0 Then
                                    MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                    Exit Sub
                                End If
                            End If
                            'End
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
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
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
        'mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(txtDate.Text, cmbAircraftList.SelectedValue, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , cmbMonitorType.SelectedValue, , , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text))
        mAssemblyMonitorInspStatusListNew = AssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(CurrentDate:=txtDate.Text, AssemblyStatusPeriodList:=Nothing, AssemblyID:=New Guid(cmbAircraftAssembly.SelectedValue), MonitorTypeID:=CType(cmbMonitorType.SelectedValue, Integer), MachineID:=cmbAircraftList.SelectedValue.ToString, IsInspStatusPeriodsRequired:=False, IsFromMPD:=True, IsComplied:=True, CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True))
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            'Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
            '                                          Select StatusInfo).ToList.Take(RecordsToShow)
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                       Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                     Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        'Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        Session("mAssemblyMonitorInspStatusListNew") = mAssemblyMonitorInspStatusListNew
        dgDueMonitoringList.DataBind()
        SetGrid()
        ControlVisibility()
    End Sub
    Private Sub SetPage()
        'If RecordsToShow < mTmpComplyAssemblyMonitorInspStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    lblResult.Text = "List of Assembly Inspection Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyAssemblyMonitorInspStatusList.Count & " Record(s) shown."
        'Else
        '    lblResult.Text = "List of Assembly Inspection Status as per selected criteria : " & mTmpComplyAssemblyMonitorInspStatusList.Count & " Record(s) found."
        'End If
        Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                      Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                    Select StatusInfo).ToList
        If RecordsToShow < List.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
            lblResult.Text = "List of Assembly Inspection Status as per selected criteria : " & RecordsToShow.ToString & " of " & List.Count.ToString & " Record(s) shown."
        Else
            lblResult.Text = "List of Assembly Inspection Status as per selected criteria : " & List.Count.ToString & " Record(s) found."
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

        For j As Integer = 0 To dgDueMonitoringList.Rows.Count - 1
            B = CType(Me.dgDueMonitoringList.Rows(j).Cells(24).Text, Boolean) 'IsMaster
            c = CType(Me.dgDueMonitoringList.Rows(j).Cells(26).Text, Boolean) 'IsAttachmentAdded
            If B = True Then
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = False 'History
            End If
            If c = False Then
                dgDueMonitoringList.Rows(j).Cells(25).Enabled = False 'View
            End If

            'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgDueMonitoringList.Rows(j).Cells(20).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = False
                btnAddNewTop.Enabled = False
                btnAddNew.Enabled = False
                lblReadOnly.Visible = True
            Else
                dgDueMonitoringList.Rows(j).Cells(20).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(22).Enabled = True
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
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            'Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
            '                                           Select StatusInfo).ToList.Take(RecordsToShow)
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        dgDueMonitoringList.DataBind()
        SetGrid()
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
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
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraftList.SelectedValue, txtDate.Text.ToString, "(All)", True)
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
        '-----------------------------------------
        txtCodeFormNo.Text = CodeFormNoDesc
        'mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(DoneOn, cmbAircraftList.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , CType(MonitorTypeID, Integer), , , ShowNotApplicable, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text))
        mAssemblyMonitorInspStatusListNew = AssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(CurrentDate:=DoneOn, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblylist(cmbAircraftAssembly.SelectedIndex).ID, MonitorTypeID:=CType(MonitorTypeID, Integer), MachineID:=cmbAircraftList.SelectedValue.ToString, IsInspStatusPeriodsRequired:=False, IsFromMPD:=True, IsComplied:=True, CodeFormNoDesc:=CodeFormNoDesc, IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True))
        Session("mAssemblyMonitorInspStatusListNew") = mAssemblyMonitorInspStatusListNew
        chkApplicable.Checked = ShowNotApplicable 'Added by Saylee on 7-Jan-2011
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            'Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
            '                                           Select StatusInfo).ToList.Take(RecordsToShow)
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        'Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        Session("mAssemblyMonitorInspStatusListNew") = mAssemblyMonitorInspStatusListNew
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

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            cmbAircraftList.Focus()
            Session("MiddleFrame") = "wfComplyAssemblyMonitorInspStatusListShowValues_Ajax.aspx?"
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
        Session.Remove("ATA")
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
        Dim AssemblyMonitorInspStatusID As Guid
        Dim Model, SerialNo As String
        Select Case e.CommandName
            Case "Comply"
                If Not User.IsInRole("AssemblyInspectionsNew") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                ComplyRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "EditRec"
                If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    'Added by Vikrant on 26-July-2011
                    mAircraft = cmbAircraftList.SelectedItem.Text
                    'mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).MonitorType
                    'mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).ModelMonitorInspCode
                    'mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).Code_Desc
                    mMonitorType = mAssemblyMonitorInspStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).MonitorType
                    mMonitorInfo = mAssemblyMonitorInspStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).Code
                    mMonitorDesc = mAssemblyMonitorInspStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).ModelMonitorInspCode_Desc
                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                    MarkLog(Util.Action.Edit, "AssemblyInspections", User.Identity.Name & " is not Authorized User to edit " & mAssemblyMonitorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                EditRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
                'End
            Case "DeleteRec"

                'Changed by Vikrant on 26-July-2011
                mAircraft = cmbAircraftList.SelectedItem.Text
                'mMonitorType = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).MonitorType
                'mMonitorInfo = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).ModelMonitorInspCode
                'mMonitorDesc = mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).Code_Desc
                mMonitorType = mAssemblyMonitorInspStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).MonitorType
                mMonitorInfo = mAssemblyMonitorInspStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).Code
                mMonitorDesc = mAssemblyMonitorInspStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).ModelMonitorInspCode_Desc
                If (Not User.IsInRole("AssemblyInspectionsDelete")) Then
                    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
                    MarkLog(Util.Action.Delete, "AssemblyInspections", User.Identity.Name & " is not Authorized User to delete " & mAssemblyMonitorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                DeleteRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "History"   'Added by Saylee on 09-Sep-2009

                If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                HistoryRecords(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "ViewRec"
                'GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                'mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorInspStatusList(CInt(e.CommandArgument)).ID)
                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).ID)
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
                'GridBind()
                dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                Dim AssemblyMonitorInspStatusIDs As New StringBuilder
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                AssemblyMonitorInspStatusIDs.Append("<AssMonInspID>")
                AssemblyMonitorInspStatusIDs.Append("<id>")
                AssemblyMonitorInspStatusIDs.Append(New Guid(currentRow.Cells(0).Text))
                AssemblyMonitorInspStatusIDs.Append("</id>")
                AssemblyMonitorInspStatusIDs.Append("</AssMonInspID>")

                'GridBind()
                'SetGrid()
                'ControlVisibility()
                AssemblyMonitorInspStatusID = New Guid(currentRow.Cells(0).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorInspStatusListNew(AssemblyMonitorInspStatusID).AssemblyStatusID)
                Model = mAssemblyStatus.Assembly.ModelName
                SerialNo = mAssemblyStatus.Assembly.SerialNo
                'AssemblyId = New Guid(currentRow.Cells(3).Text)
                Dim mtmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
                'mtmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList([Date]:=Today.Date.ToString, Model:=Model, SerialNo:=SerialNo, AssemblyId:=AssemblyId, MachineID:=cmbAircraftList.SelectedValue.ToString, AssemblyMonitorInspStatusIDs:=CompMonitorServiceStatusID.ToString, ShowNotApplicable:=IIf(chkApplicable.Checked, True, False), SkipOneTimeDoneMasterRecords:=IIf(chkOneTimeMasterRecords.Checked = True, False, True), ShowAllRecords:=IIf(chkApplicable.Checked, True, False))
                mtmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList([Date]:=Today.Date.ToString, MachineID:=cmbAircraftList.SelectedValue.ToString, Model:=Model, SerialNo:=SerialNo, AssemblyMonitorInspStatusIDs:=AssemblyMonitorInspStatusIDs.ToString, IsFromMPD:=True, ShowNotApplicable:=chkApplicable.Checked)
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

                If mtmpComplyAssemblyMonitorInspStatusList.Count > 0 Then
                    FrequencyLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).FrequencyValueFormatted
                    DoneOnLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).DoneOnValueFormatted
                    CurrentLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).CurrentValueFormatted
                    ElapsedLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).ElapsedValueFormatted
                    ExtensionLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).ExtensionValueFormatted
                    DueOnLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).DueOnValueFormattedForGrid
                    AssemblyDueOnLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).AssemblyDueOnValueTextFormattedByAirFrame
                    RemainingLabel.Text = mtmpComplyAssemblyMonitorInspStatusList(0).RemainingValueFormattedForGrid
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
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click, btnAddNew.Click
        If IsValid Then
            Session("AircraftIdForInsp") = cmbAircraftList.SelectedValue.ToString
            'Added by Vikrant on 26-July-2011
            MarkLog(Util.Action.[New], "AssemblyInspections", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAssemblyMonitorInspStatusListNew.aspx?BackPage=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAssemblyInspectionListNewWindow", "OpenAssemblyInspectionListNewWindow()", True)
            Session("NewPage") = "True"
        End If
    End Sub
    Private Sub dgDueMonitoringList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueMonitoringList.Sorting
        'mTmpComplyAssemblyMonitorInspStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
        mAssemblyMonitorInspStatusListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            'Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mTmpComplyAssemblyMonitorInspStatusList
            '                                           Select StatusInfo).ToList.Take(RecordsToShow)
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        'Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        Session("mAssemblyMonitorInspStatusListNew") = mAssemblyMonitorInspStatusListNew
        dgDueMonitoringList.DataBind()
        SetGrid()
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
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
        'RecordsToShow = mTmpComplyAssemblyMonitorInspStatusList.Count
        RecordsToShow = mAssemblyMonitorInspStatusListNew.Count
        Session("RecordsToShow") = RecordsToShow
        'Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
        Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorInspStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
        dgDueMonitoringList.DataSource = List
        dgDueMonitoringList.DataBind()
        lnkLoadMore.Enabled = False
        lnkLoadMoreTop.Enabled = False
        SetPage()
        SetGrid()
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
    End Sub
    Protected Sub dgDueMonitoringList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgDueMonitoringList.Columns(i).HeaderText
            Next
        End If
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
    ' Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
    '     If (Not User.IsInRole("AssemblyInspectionsPrint")) Then
    '         MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
    '         Exit Sub
    '     End If
    '     dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
    '     dgDueMonitoringList.DataBind()

    '     SetGrid()
    '     dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)

    '     Dim Rpt As New crListComplyAssemblyMonitorStatus
    '     Dim da As New CSLA.Data.ObjectAdapter
    '     Dim ds As New dsCommon
    '     Dim ReportDetails As New rptStatusList

    '     SearchStr1 = "Date :" + "  " + txtDate.Text
    '     SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
    '     SearchStr3 = ""
    '     SearchStr4 = "Aircraft :" + "  " + cmbAircraftList.SelectedItem.Text

    '     ReportDetails.Add(New rptStatus(, 1, , _
    '                       , , , dgDueMonitoringList.Columns.Item(0).HeaderText, , dgDueMonitoringList.Columns.Item(4).HeaderText, dgDueMonitoringList.Columns.Item(6).HeaderText, _
    '                         dgDueMonitoringList.Columns.Item(7).HeaderText, dgDueMonitoringList.Columns.Item(8).HeaderText, _
    '                        dgDueMonitoringList.Columns.Item(9).HeaderText, dgDueMonitoringList.Columns.Item(10).HeaderText, dgDueMonitoringList.Columns.Item(11).HeaderText, _
    '                        dgDueMonitoringList.Columns.Item(12).HeaderText, dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText, _
    '                        dgDueMonitoringList.Columns.Item(15).HeaderText, dgDueMonitoringList.Columns.Item(16).HeaderText, dgDueMonitoringList.Columns.Item(17).HeaderText, _
    '                       , , , , , , , , , dgDueMonitoringList.Columns.Item(18).HeaderText))

    '     Dim TotalCount As Integer
    '     TotalCount = Me.mTmpComplyAssemblyMonitorInspStatusList.Count
    '     Dim I As Integer
    '     Dim str(14) As String

    '     For I = 0 To TotalCount - 1

    '         str(0) = ""
    '         str(1) = ""
    '         str(2) = ""
    '         str(3) = ""
    '         str(4) = ""
    '         str(5) = ""
    '         str(6) = ""
    '         str(7) = ""
    '         str(8) = ""
    '         str(9) = ""
    '         str(10) = ""
    '         str(11) = ""
    '         str(12) = ""
    '         str(13) = ""
    '         str(14) = ""

    '         If Me.dgDueMonitoringList.Rows(I).Cells(0).Text <> "&nbsp;" Then str(0) = Me.dgDueMonitoringList.Rows(I).Cells(0).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(4).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgDueMonitoringList.Rows(I).Cells(7).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgDueMonitoringList.Rows(I).Cells(8).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgDueMonitoringList.Rows(I).Cells(9).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgDueMonitoringList.Rows(I).Cells(10).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgDueMonitoringList.Rows(I).Cells(11).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(8) = Me.dgDueMonitoringList.Rows(I).Cells(12).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(13).Text <> "&nbsp;" Then str(9) = Me.dgDueMonitoringList.Rows(I).Cells(13).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(14).Text <> "&nbsp;" Then str(10) = Me.dgDueMonitoringList.Rows(I).Cells(14).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(15).Text <> "&nbsp;" Then str(11) = Me.dgDueMonitoringList.Rows(I).Cells(15).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(16).Text <> "&nbsp;" Then str(12) = Me.dgDueMonitoringList.Rows(I).Cells(16).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(17).Text <> "&nbsp;" Then str(13) = Me.dgDueMonitoringList.Rows(I).Cells(17).Text.Replace("<BR>", vbCrLf)
    '         If Me.dgDueMonitoringList.Rows(I).Cells(18).Text <> "&nbsp;" Then str(14) = Me.dgDueMonitoringList.Rows(I).Cells(18).Text.Replace("<BR>", vbCrLf)

    '         ReportDetails.Add(New rptStatus(, 2, , _
    '          , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), _
    '          str(7), str(8), str(9), str(10), str(11), str(12), str(13), , , , , , , , , , str(14)))
    '     Next

    '     mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
    '     Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    'mCompanyDetail.WebSite, "List of Comply Assembly Inspection Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

    '     If mTmpComplyAssemblyMonitorInspStatusList.Count = 0 Then
    '         MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '         Exit Sub
    '     End If
    '     da.Fill(ds, ReportDetails)
    '     da.Fill(ds, Report)
    '     Dim mrptImage As rptImage = rptImage.GetImage(ds)
    '     da.Fill(ds, mrptImage)
    '     Rpt.SetDataSource(ds)
    '     Session("CrystalReport") = Rpt
    '     ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    ' End Sub
#End Region
#End Region


End Class