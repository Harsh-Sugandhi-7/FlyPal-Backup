'AJAX Conversion By Vikrant On 19-Mar-2015
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Public Class wfComplyAssemblyMonitorModStatusListShowValues_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mMachineNameValueList As MachineNameValueList
    ' Private mTmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList   Commeted By Shital on 02-May-2019 for MPD Slow
    Protected mAssemblyMonitorModStatusListNew As AssemblyMonitorModStatusList
    Private DoneOn As String
    Private AircraftId As String
    Dim mMachine As Machine
    Public mBoardInfo As AircraftInformationBoard.BoardInfo 'Added by Saylee on 22-May-2009
    Private mModelMonitorModTypeList As ModelMonitorModTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String 'Added by Saylee on 30-July-2009
    Private DirectiveNo As String 'Added by Saylee on 07-Aug-2009
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
        '    mTmpComplyAssemblyMonitorModStatusList = CType(Session("mTmpComplyAssemblyMonitorModStatusList"), tmpComplyAssemblyMonitorModStatusList)   Commeted By Shital on 02-May-2019 for MPD Slow
        mAssemblyMonitorModStatusListNew = CType(Session("mAssemblyMonitorModStatusListNew"), AssemblyMonitorModStatusList)
        DoneOn = CType(Session("DoneOn"), String)
        AircraftId = CType(Session("AircraftId"), String)
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009
        DirectiveNo = Session("DirectiveNo") 'Added by Saylee on 07-Aug-2009
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        ShowNotApplicable = CType(Session("ShowNotApplicable"), Boolean) 'Added by Saylee on 7th-Jan-2011
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AssemblyId = CType(Session("AssemblyId"), String)
        SkipOneTimeDoneMRecords = CType(Session("SkipOneTimeDoneMRecords"), Boolean)
        RecordsToShow = CType(Session("RecordsToShow"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        CodeFormNoDesc = Session("CodeFormNoDesc")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        'Session.Remove("mTmpComplyAssemblyMonitorModStatusList")        Commeted By Shital on 02-May-2019 for MPD Slow
        Session.Remove("mAssemblyMonitorModStatusListNew")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 9th-Oct-2009
        Session.Remove("RecordsToShow")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyAssemblyMonitorModStatusListShowValues_Ajax.aspx?" Then
            'Session.Remove("mTmpComplyAssemblyMonitorModStatusList")  Commeted By Shital on 02-May-2019 for MPD Slow
            Session.Remove("mAssemblyMonitorModStatusListNew")
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
        End If
    End Sub
    Private Sub EnableLinks()
        ' Commeted By Shital on 02-May-2019 for MPD Slow
        'If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
        '    If RecordsToShow < mTmpComplyAssemblyMonitorModStatusList.Count Then
        '        lnkShowAllRecords.Enabled = True
        '        lnkShowAllRecordsTop.Enabled = True
        '    Else
        '        lnkShowAllRecords.Enabled = False
        '        lnkShowAllRecordsTop.Enabled = False
        '    End If
        'End If


        If Not mAssemblyMonitorModStatusListNew Is Nothing Then
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                     Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                   Select StatusInfo).ToList
            If RecordsToShow < List.Count Then
                lnkShowAllRecords.Enabled = True
                lnkShowAllRecordsTop.Enabled = True
            Else
                lnkShowAllRecords.Enabled = False
                lnkShowAllRecordsTop.Enabled = False
            End If
        End If
    End Sub
    Private Sub ControlVisibility()
        ' Commeted By Shital on 02-May-2019 for MPD Slow
        'btnPrint.Enabled = (mTmpComplyAssemblyMonitorModStatusList.Count > 0)
        'btnPrintTop.Enabled = (mTmpComplyAssemblyMonitorModStatusList.Count > 0)
        btnPrint.Enabled = (mAssemblyMonitorModStatusListNew.Count > 0)
        btnPrintTop.Enabled = (mAssemblyMonitorModStatusListNew.Count > 0)
        dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
        EnableLinks()
    End Sub
    Private Sub ComplyRecord(ByVal ID As Guid)
        ' mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)     Commeted By Shital on 02-May-2019 for MPD Slow
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))

        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        ' Commeted By Shital on 02-May-2019 for MPD Slow
        ' Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mAssemblyMonitorModStatusListNew(ID).ID, mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID, mMachine.HourType)
        If (mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And (mPrevAssemblyMonitorModStatus.IsCompleted Or mPrevAssemblyMonitorModStatus.FetchRecordCount(mPrevAssemblyMonitorModStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not mAssemblyMonitorModStatusListNew(ID).IsApplicable Then
            MSGBoxCtrl.show(MSGBox.Message_title.MonitoringNotApplicable, MSGBox.Message_text.MonitoringNotApplicable, "You are trying to comply the record.Directives monitoring is not applicable, can not be complied.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, txtDate.Text, mPrevAssemblyMonitorModStatus.ModelMonitorMod.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus

            ' Commeted By Shital on 02-May-2019 for MPD Slow
            'Added by vikrant on 27-July-2011
            'mAircraft = mTmpComplyAssemblyMonitorModStatusList(Index).MachineInfo
            'mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList(Index).ModNumber
            'mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModInfo
            'mMonitorType = mTmpComplyAssemblyMonitorModStatusList(Index).MonitorType

            mAircraft = cmbAircraftList.SelectedItem.ToString
            mDirectiveNo = mAssemblyMonitorModStatusListNew(ID).Number
            mMonitorInfo = mAssemblyMonitorModStatusListNew(ID).Type
            mMonitorType = mAssemblyMonitorModStatusListNew(ID).MonitorType

            Dim DoneOnValue As String
            For i As Integer = 0 To mPrevAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
                If i = 0 Then
                    DoneOnValue = mPrevAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(i).DoneOnValueFormatted
                Else
                    DoneOnValue = DoneOnValue + " " + mPrevAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(i).DoneOnValueFormatted
                End If
            Next
            ' Commeted By Shital on 02-May-2019 for MPD Slow
            'mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Done On Date : " & mTmpComplyAssemblyMonitorModStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorModStatusList(Index).DoneOnValueFormatted
            'MarkLog(Util.Action.Comply, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, EventLogID)
            mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Done On Date : " & mAssemblyMonitorModStatusListNew(ID).DoneOnFormatted & " Done On Value : " & DoneOnValue
            MarkLog(Util.Action.Comply, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatusListNew(ID).ID, EventLogID)

            Session("From") = 0 'New record
            ''
            mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

            ' Commeted By Shital on 02-May-2019 for MPD Slow
            ' Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID)
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

            ' Commeted By Shital on 02-May-2019 for MPD Slow
            'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorModStatusListNew(ID).Reference + "->" + mAssemblyMonitorModStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorModStatusListNew(ID).Description

            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        ' Commeted By Shital on 02-May-2019 for MPD Slow
        ' mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        Dim mAssemblyStatus As AssemblyStatus

        ' Commeted By Shital on 02-May-2019 for MPD Slow
        '   Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mAssemblyMonitorModStatusListNew(ID).ID, mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID, mMachine.HourType)

        If mPrevAssemblyMonitorModStatus.IsMaster And mPrevAssemblyMonitorModStatus.IsApplicable And chkApplicable.Checked = False Then
            ' MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit Comply Assembly Directives Status.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mPrevAssemblyMonitorModStatus.IsMaster) And (Not mPrevAssemblyMonitorModStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records
            Session("mAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("From") = 1 'Edit record

            ' Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)

            ' Commeted By Shital on 02-May-2019 for MPD Slow
            'mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID)

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

            ' Commeted By Shital on 02-May-2019 for MPD Slow
            ' Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorModStatusListNew(ID).Reference + "->" + mAssemblyMonitorModStatusListNew(ID).Number + "->" + mAssemblyMonitorModStatusListNew(ID).Type + "->" + mAssemblyMonitorModStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorModStatusListNew(ID).Description

            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
            '**********************************************************************
            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevAssemblyMonitorModStatus.IsMaster = False) And (mPrevAssemblyMonitorModStatus.IsCompleted = False) And mPrevAssemblyMonitorModStatus.IsDone = False) Then

            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mAssemblyMonitorModStatusListNew(ID).ID, mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID, mMachine.HourType)

            Dim mModelMonitorMod As ModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mAssemblyMonitorModStatusListNew(ID).ModelMonitorModID, mMachine.HourType)
            Session("mModelMonitorMod") = mModelMonitorMod

            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID)
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
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID)
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

            Dim DoneOnValue As String
            For i As Integer = 0 To mPrevAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
                If i = 0 Then
                    DoneOnValue = mPrevAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(i).DoneOnValueFormatted
                Else
                    DoneOnValue = DoneOnValue + " " + mPrevAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(i).DoneOnValueFormatted
                End If
            Next
            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorModStatusListNew(ID).Reference + "->" + mAssemblyMonitorModStatusListNew(ID).Number + "->" + mAssemblyMonitorModStatusListNew(ID).Type + "->" + mAssemblyMonitorModStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorModStatusListNew(ID).Description

            'Added by vikrant on 27-July-2011
            'mAircraft = mTmpComplyAssemblyMonitorModStatusList(Index).MachineInfo
            'mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList(Index).ModNumber
            'mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModInfo
            'mMonitorType = mTmpComplyAssemblyMonitorModStatusList(Index).MonitorType

            mAircraft = cmbAircraftList.SelectedItem.ToString
            mDirectiveNo = mAssemblyMonitorModStatusListNew(ID).Number
            mMonitorInfo = mAssemblyMonitorModStatusListNew(ID).Type
            mMonitorType = mAssemblyMonitorModStatusListNew(ID).MonitorType
            mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Done On Date :" & mAssemblyMonitorModStatusListNew(ID).DoneOnFormatted & " Done On Value : " & DoneOnValue
            MarkLog(Util.Action.Edit, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatusListNew(ID).ID, EventLogID)
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub HistoryRecords(ByVal ID As Guid)  'Added by Saylee on 09-Sep-2009
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        ' Commeted By Shital on 02-May-2019 for MPD Slow
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        'Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(ID, mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID, mMachine.HourType)
        If mPrevAssemblyMonitorModStatus.IsMaster Then
            MSGBoxCtrl.show("Master Record!", "There is no history for this record", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("From") = 1 'Edit record
            ''
            ' Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorModStatusListNew(ID).AssemblyStatusID)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(Index).Description
            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.Assembly.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorModStatusListNew(ID).Reference + "->" + mAssemblyMonitorModStatusListNew.Item(ID).Number + "->" + mAssemblyMonitorModStatusListNew(ID).Type + "->" + mAssemblyMonitorModStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorModStatusListNew(ID).Description

            Session("ATA") = mAssemblyMonitorModStatusListNew(ID).ATACode.ToString
            Session("Description") = mAssemblyMonitorModStatusListNew(ID).Description
            Session("ModelSerialNo") = "[Model: " & mAssemblyStatus.Assembly.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"

            mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorModStatus.ModelMonitorModID, mMachine.HourType)
            Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList

            'RemoveSession()
            'Added by Vikrant on 3-Aug-2011
            mAircraft = cmbAircraftList.SelectedItem.Text
            'mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModNumber
            'mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModelMonitorModInfo
            'mMonitorType = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).MonitorType
            'mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType

            mDirectiveNo = mAssemblyMonitorModStatusListNew(ID).Number
            mMonitorInfo = mAssemblyMonitorModStatusListNew(ID).Type
            mMonitorType = mAssemblyMonitorModStatusListNew(ID).MonitorType
            mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType

            MarkLog(Util.Action.View, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfUpdateComplyHistoryAssemblyMonitorModStatusList.aspx?GChildPage2=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDirectiveHistoryWindow", "OpenDirectiveHistoryWindow()", True)
        End If
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        'Revise Activity
        'If chkApplicable.Checked And mTmpComplyAssemblyMonitorModStatusList(Index).ModelActivityCount > 1 Then 'Revise Activity
        '    MSGBoxCtrl.show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
        'Else
        '    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        'End If
        'mTmpComplyAssemblyMonitorModStatusList.CurrentIndex = Index
        'Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList

        If chkApplicable.Checked And mAssemblyMonitorModStatusListNew(ID).ModelActivityCount > 1 Then 'Revise Activity
            MSGBoxCtrl.show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        End If
        mAssemblyMonitorModStatusListNew.CurrentIndex = mAssemblyMonitorModStatusListNew(ID, "")
        Session("mAssemblyMonitorModStatusListNew") = mAssemblyMonitorModStatusListNew
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
                            ''Added by vikrant on 27-July-2011
                            'IDForEventLog = mTmpComplyAssemblyMonitorModStatusList(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID
                            'mAircraft = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).MachineInfo
                            'mDirectiveNo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModNumber
                            'mMonitorInfo = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).ModelMonitorModInfo
                            'mMonitorType = mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).MonitorType
                            'mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType
                            ''End
                            ''Added by Saylee on 28-May-2009
                            'mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.AssemblyMonitorModStatusID)
                            ''********************************

                            ''Added by Saylee on 9th-Oct-2009
                            'mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.AssemblyMonitorModStatusID, 7)
                            ''=============================
                            'If mTmpComplyAssemblyMonitorModStatusList(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).IsAttachmentAdded = True Then
                            '    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorModStatusList(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID)
                            'End If

                            'AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.AssemblyMonitorModStatusID)
                            'MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            'If Not mFileAttach Is Nothing Then
                            '    If mFileAttach.Size > 0 Then
                            '        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                            '    End If
                            'End If
                            'Session("mMachineMaintenance") = mMachineMaintenance
                            ''MarkLog(Util.Action.Comply, "Assembly Monitor Directive", mDirectiveDetail, Util.ErrorType.NoError, mTmpComplyAssemblyMonitorModStatusList.Item(mTmpComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, EventLogID)

                            ''Added by Saylee on 28-May-2009
                            'mBoardInfo.IsComplyDelete = True
                            'mBoardInfo.ApplyEdit()
                            'mBoardInfo.Save()
                            'Session("mAircraftInformationBoardList") = Nothing
                            ''********************************
                            ''Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                            'If AppSettings("LinkMaintenance") = "True" Then
                            '    If LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorModStatusList.CurrentItem.ModelMonitorModID.ToString).Count > 0 Then
                            '        MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                            '        Exit Sub
                            '    End If
                            'End If
                            ''End

                            'Added by vikrant on 27-July-2011
                            IDForEventLog = mAssemblyMonitorModStatusListNew(mAssemblyMonitorModStatusListNew.CurrentIndex).ID
                            mAircraft = cmbAircraftList.SelectedItem.Text
                            mDirectiveNo = mAssemblyMonitorModStatusListNew.Item(mAssemblyMonitorModStatusListNew.CurrentIndex).Number
                            mMonitorInfo = mAssemblyMonitorModStatusListNew.Item(mAssemblyMonitorModStatusListNew.CurrentIndex).Type
                            mMonitorType = mAssemblyMonitorModStatusListNew.Item(mAssemblyMonitorModStatusListNew.CurrentIndex).MonitorType
                            mDirectiveDetail = "Aircraft : " & mAircraft & " Directive No. : " & mDirectiveNo & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mAssemblyMonitorModStatusListNew(mAssemblyMonitorModStatusListNew.CurrentIndex).ID)
                            '********************************

                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatusListNew(mAssemblyMonitorModStatusListNew.CurrentIndex).ID, 7)
                            '=============================
                            If mAssemblyMonitorModStatusListNew(mAssemblyMonitorModStatusListNew.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatusListNew(mAssemblyMonitorModStatusListNew.CurrentIndex).ID)
                            End If

                            AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(mAssemblyMonitorModStatusListNew(mAssemblyMonitorModStatusListNew.CurrentIndex).ID)
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
                            'Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                            If AppSettings("LinkMaintenance") = "True" Then
                                If LinkMaintenanceList.GetLinkMaintenanceList(mAssemblyMonitorModStatusListNew(mAssemblyMonitorModStatusListNew.CurrentIndex).ModelMonitorModID.ToString).Count > 0 Then
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
                                MarkLog(Util.Action.Delete, "AssemblyModifications", "Can't delete :" & mDirectiveDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
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

        'mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(txtDate.Text, cmbAircraftList.SelectedValue, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , cmbMonitorType.SelectedValue, , , DirectiveNo, chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue")
        ''Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
        'End If
        'Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList

        mAssemblyMonitorModStatusListNew = AssemblyMonitorModStatusList.GetAssemblyMonitorModStatuslist(CurrentDate:=txtDate.Text, AssemblyStatusPeriodList:=Nothing, AssemblyID:=New Guid(cmbAircraftAssembly.SelectedValue), MonitorTypeID:=CType(cmbMonitorType.SelectedValue, Integer), MachineID:=cmbAircraftList.SelectedValue.ToString, IsModStatusPeriodsRequired:=False, IsRecordsDirectFetch:=True, IsComplied:=True, CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True), DirectiveNo:=Trim(txtDirectiveNo.Text))
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                       Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                     Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        Session("mAssemblyMonitorModStatusListNew") = mAssemblyMonitorModStatusListNew

        dgDueMonitoringList.DataBind()
        SetGrid()
        ControlVisibility()
    End Sub
    Private Sub SetPage()
        'If RecordsToShow < mTmpComplyAssemblyMonitorModStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    lblResult.Text = "List of Assembly Directives Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyAssemblyMonitorModStatusList.Count & " Record(s) shown."
        'Else
        '    lblResult.Text = "List of Assembly Directives Status as per selected criteria : " & mTmpComplyAssemblyMonitorModStatusList.Count & " Record(s) found."
        'End If
        Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                     Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                   Select StatusInfo).ToList
        If RecordsToShow < List.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
            lblResult.Text = "List of Assembly Directive Status as per selected criteria : " & RecordsToShow.ToString & " of " & List.Count & " Record(s) shown."
        Else
            lblResult.Text = "List of Assembly Directive Status as per selected criteria : " & List.Count & " Record(s) found."
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
        'Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
        'End If
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        dgDueMonitoringList.DataBind()
        SetGrid()
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

        'mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(DoneOn, cmbAircraftList.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , CType(MonitorTypeID, Integer), , , DirectiveNo, ShowNotApplicable, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue")
        mAssemblyMonitorModStatusListNew = AssemblyMonitorModStatusList.GetAssemblyMonitorModStatuslist(CurrentDate:=DoneOn, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblylist(cmbAircraftAssembly.SelectedIndex).ID, MonitorTypeID:=CType(MonitorTypeID, Integer), MachineID:=cmbAircraftList.SelectedValue.ToString, IsModStatusPeriodsRequired:=False, IsRecordsDirectFetch:=True, IsComplied:=True, CodeFormNoDesc:=CodeFormNoDesc, IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True), DirectiveNo:=DirectiveNo)

        chkApplicable.Checked = IIf(ShowNotApplicable, True, False)
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            'Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
            '                                           Select StatusInfo).ToList.Take(RecordsToShow)
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                       Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        dgDueMonitoringList.DataBind()
        'Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
        Session("mAssemblyMonitorModStatusListNew") = mAssemblyMonitorModStatusListNew

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
        txtCodeFormNo.Text = CodeFormNoDesc
        txtCodeFormNo.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by vikrant on 27-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            cmbAircraftList.Focus()
            Session("MiddleFrame") = "wfComplyAssemblyMonitorModStatusListShowValues_Ajax.aspx?"
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
    Private Sub txtCodeFormNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtCodeFormNo.TextChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub chkOneTimeMasterRecords_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkOneTimeMasterRecords.CheckedChanged
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
        Dim AssemblyMonitorModStatusID As Guid
        Dim Model, SerialNo As String

        Select Case e.CommandName
            Case "Comply"
                If Not User.IsInRole("AssemblyModificationsNew") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                ComplyRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "EditRec"
                If (Not User.IsInRole("AssemblyModificationsView") And Not User.IsInRole("AssemblyModificationsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                EditRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "DeleteRec"
                If (Not User.IsInRole("AssemblyModificationsDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                DeleteRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "History"  'Added by Saylee on 09-Sep-2009
                If (Not User.IsInRole("AssemblyModificationsView") And Not User.IsInRole("AssemblyModificationsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                HistoryRecords(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "ViewRec"
                'GridBind()
                'dgDueMonitoringList.Columns(21).Visible = IIf(chkApplicable.Checked, False, True)
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatusListNew(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString)).ID)
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
                Dim AssemblyMonitorModStatusIDs As New StringBuilder
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                AssemblyMonitorModStatusIDs.Append("<AssMonModID>")
                AssemblyMonitorModStatusIDs.Append("<id>")
                AssemblyMonitorModStatusIDs.Append(New Guid(currentRow.Cells(0).Text))
                AssemblyMonitorModStatusIDs.Append("</id>")
                AssemblyMonitorModStatusIDs.Append("</AssMonModID>")

                'GridBind()
                'SetGrid()
                'ControlVisibility()
                AssemblyMonitorModStatusID = New Guid(currentRow.Cells(0).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorModStatusListNew(AssemblyMonitorModStatusID).AssemblyStatusID)
                Model = mAssemblyStatus.Assembly.ModelName
                SerialNo = mAssemblyStatus.Assembly.SerialNo
                'AssemblyId = New Guid(currentRow.Cells(3).Text)
                Dim mtmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList

                mtmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList([Date]:=Today.Date.ToString, MachineID:=cmbAircraftList.SelectedValue.ToString, Model:=Model, SerialNo:=SerialNo, AssemblyMonitorModStatusIDs:=AssemblyMonitorModStatusIDs.ToString, IsForConfiguredList:=True, ShowNotApplicable:=chkApplicable.Checked)
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

                If mtmpComplyAssemblyMonitorModStatusList.Count > 0 Then
                    FrequencyLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).FrequencyValueFormatted
                    DoneOnLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).DoneOnValueFormatted
                    CurrentLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).CurrentValueFormatted
                    ElapsedLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).ElapsedValueFormatted
                    ExtensionLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).ExtensionValueFormatted
                    DueOnLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).DueOnValueFormattedForGrid
                    AssemblyDueOnLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).AssemblyDueOnValueTextFormattedByAirFrame
                    RemainingLabel.Text = mtmpComplyAssemblyMonitorModStatusList(0).RemainingValueFormattedForGrid
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
        'mTmpComplyAssemblyMonitorModStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
        ''Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo In mTmpComplyAssemblyMonitorModStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
        'End If
        'Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList

        mAssemblyMonitorModStatusListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        Session("mAssemblyMonitorModStatusListNew") = mAssemblyMonitorModStatusListNew

        dgDueMonitoringList.DataBind()
        SetGrid()
        ControlVisibility()
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
        'RecordsToShow = mTmpComplyAssemblyMonitorModStatusList.Count
        'Session("RecordsToShow") = RecordsToShow
        'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
        RecordsToShow = mAssemblyMonitorModStatusListNew.Count
        Session("RecordsToShow") = RecordsToShow
        Dim List = (From StatusInfo As AssemblyMonitorModStatusInfo In mAssemblyMonitorModStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
        dgDueMonitoringList.DataSource = List
        dgDueMonitoringList.DataBind()
        SetPage()
        SetGrid()
        ControlVisibility()
        upnlActionBtn.Update()
    End Sub
#End Region

#Region " Report "
    'Created By:- Rajnish on 22-09-2006

#Region " Report Variable Declaration "
    'Dim mCompanyDetail As New CompanyDetail
    'Private SearchStr1 As String = ""
    'Private SearchStr2 As String = ""
    'Private SearchStr3 As String = ""
    'Private SearchStr4 As String = ""
#End Region

#Region " Event "

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        '     If (Not User.IsInRole("AssemblyModificationsPrint")) Then
        '         MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
        '         Exit Sub
        '     End If
        '     dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
        '     dgDueMonitoringList.DataBind()
        '     SetGrid()

        '     Dim Rpt As New crListComplyAssemblyMonitorStatus
        '     Dim da As New CSLA.Data.ObjectAdapter
        '     Dim ds As New dsCommon
        '     Dim ReportDetails As New rptStatusList

        '     SearchStr1 = "Date :" + "  " + txtDate.Text
        '     SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        '     SearchStr3 = ""
        '     SearchStr4 = "Aircraft :" + "  " + cmbAircraftList.SelectedItem.Text

        '     ReportDetails.Add(New rptStatus(, 1, , _
        '           , , , dgDueMonitoringList.Columns.Item(0).HeaderText, , dgDueMonitoringList.Columns.Item(5).HeaderText, dgDueMonitoringList.Columns.Item(7).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(8).HeaderText, dgDueMonitoringList.Columns.Item(9).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(10).HeaderText, dgDueMonitoringList.Columns.Item(11).HeaderText, dgDueMonitoringList.Columns.Item(12).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText, dgDueMonitoringList.Columns.Item(15).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(16).HeaderText, dgDueMonitoringList.Columns.Item(17).HeaderText, dgDueMonitoringList.Columns.Item(18).HeaderText, _
        '           , , , , , , , , , dgDueMonitoringList.Columns.Item(19).HeaderText))

        '     Dim TotalCount As Integer
        '     TotalCount = Me.mTmpComplyAssemblyMonitorModStatusList.Count
        '     Dim I As Integer
        '     Dim str(15) As String
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
        '         If Me.dgDueMonitoringList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(5).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(7).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(3) = Me.dgDueMonitoringList.Rows(I).Cells(8).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(4) = Me.dgDueMonitoringList.Rows(I).Cells(9).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(5) = Me.dgDueMonitoringList.Rows(I).Cells(10).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(6) = Me.dgDueMonitoringList.Rows(I).Cells(11).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(7) = Me.dgDueMonitoringList.Rows(I).Cells(12).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(13).Text <> "&nbsp;" Then str(8) = Me.dgDueMonitoringList.Rows(I).Cells(13).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(14).Text <> "&nbsp;" Then str(9) = Me.dgDueMonitoringList.Rows(I).Cells(14).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(15).Text <> "&nbsp;" Then str(10) = Me.dgDueMonitoringList.Rows(I).Cells(15).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(16).Text <> "&nbsp;" Then str(11) = Me.dgDueMonitoringList.Rows(I).Cells(16).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(17).Text <> "&nbsp;" Then str(12) = Me.dgDueMonitoringList.Rows(I).Cells(17).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(18).Text <> "&nbsp;" Then str(13) = Me.dgDueMonitoringList.Rows(I).Cells(18).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(19).Text <> "&nbsp;" Then str(14) = Me.dgDueMonitoringList.Rows(I).Cells(19).Text.Replace("<BR>", vbCrLf)

        '         ReportDetails.Add(New rptStatus(, 2, , _
        '          , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8), str(9), _
        '     str(10), str(11), str(12), str(13), , , , , , , , , , str(14)))
        '     Next

        '     mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        '     Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, "List of Comply Assembly Directives Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        '     If mTmpComplyAssemblyMonitorModStatusList.Count = 0 Then
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
    End Sub
#End Region

#End Region

    
End Class