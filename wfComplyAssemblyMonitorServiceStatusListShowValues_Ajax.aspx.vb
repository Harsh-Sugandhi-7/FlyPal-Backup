'AJAX Conversion By Vikrant On 20-Mar-2015
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports CSLA
Public Class wfComplyAssemblyMonitorServiceStatusListShowValues_Ajax
    Inherits System.Web.UI.Page

#Region "  Variable Declaration "
    Private mMachineNameValueList As MachineNameValueList
    'Private mTmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
    Private mAssemblyMonitorServiceStatusListNew As AssemblyMonitorServiceStatusList
    Private DoneOn As String
    Private AircraftId As String
    Public mAssemblyInfo As String                                          'Code Added 29,Jan,2007
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus    'Code Feb,1,2007
    Dim mMachine As Machine
    Public mBoardInfo As AircraftInformationBoard.BoardInfo 'Added by Saylee on 22-May-2009
    Private mModelMonitorServiceTypeList As ModelMonitorServiceTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String 'Added by Saylee on 30-July-2009
    'Added by Saylee on 09-Sep-2009
    Private mUpdateComplyHistoryAssemblyMonitorServiceStatusList As UpdateComplyHistoryAssemblyMonitorServiceStatusList
    'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance
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
    Dim ShowNotApplicable As Boolean = False
    Dim CodeFormNoDesc As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        'mTmpComplyAssemblyMonitorServiceStatusList = CType(Session("mTmpComplyAssemblyMonitorServiceStatusList"), tmpComplyAssemblyMonitorServiceStatusList)
        mAssemblyMonitorServiceStatusListNew = CType(Session("mAssemblyMonitorServiceStatusListNew"), AssemblyMonitorServiceStatusList)
        DoneOn = CType(Session("DoneOn"), String)
        AircraftId = CType(Session("AircraftId"), String)
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 6th-Oct-2009
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
        'Session.Remove("mTmpComplyAssemblyMonitorServiceStatusList")
        Session.Remove("mAssemblyMonitorServiceStatusListNew")
        Session.Remove("RecordsToShow")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyAssemblyMonitorServiceStatusListShowValues_Ajax.aspx?" Then
            'Session.Remove("mTmpComplyAssemblyMonitorServiceStatusList")
            Session.Remove("mAssemblyMonitorServiceStatusListNew")
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
        End If
    End Sub
    Private Sub EnableLinks()
        'If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
        '    If RecordsToShow < mTmpComplyAssemblyMonitorServiceStatusList.Count Then
        '        lnkShowAllRecords.Enabled = True
        '        lnkShowAllRecordsTop.Enabled = True
        '    Else
        '        lnkShowAllRecords.Enabled = False
        '        lnkShowAllRecordsTop.Enabled = False
        '    End If
        'End If
        If Not mAssemblyMonitorServiceStatusListNew Is Nothing Then
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
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
        'btnPrint.Enabled = (mAssemblyMonitorServiceStatusListNew.Count > 0)
        'btnPrintTop.Enabled = (mAssemblyMonitorServiceStatusListNew.Count > 0)
        dgDueMonitoringList.Columns(18).Visible = IIf(chkApplicable.Checked, False, True)
        EnableLinks()
    End Sub
    Private Sub ComplyRecord(ByVal ID As Guid)
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        'Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        'Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        'If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'Else
        '    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, txtDate.Text, mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        '    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        '    Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        '    Session("From") = 0 'New record
        '    ''
        '    mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
        '    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

        '    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
        '    Session("mMachine") = mMachine
        '    Session("mAssemblyStatus") = mAssemblyStatus
        '    ''NewMachineMaintenance(mAssemblyStatus, mAssemblyMonitorServiceStatus.ID)

        '    'Added by Saylee on 22-May-2009
        '    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        '    Session("mBoardInfo") = mBoardInfo
        '    '**************************************

        '    'Added By Vikrant On 25-Nov-2014
        '    Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID) 'Sort = 1 : Installation
        '    Session("mFileAttach") = mFileAttach
        '    'End

        '    Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description

        '    RemoveSession()
        '    'Changed by Vikrant on 26-July-2011
        '    mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineInfo
        '    mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo
        '    mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType
        '    mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(Index).Description
        '    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnValueFormatted
        '    MarkLog(Util.Action.Comply, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx'); ", True)
        'End If
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatusListNew.Item(ID).ID, mAssemblyMonitorServiceStatusListNew.Item(ID).AssemblyStatusID, mMachine.HourType)
        If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, txtDate.Text, mPrevAssemblyMonitorServiceStatus.ModelMonitorService.ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
            Session("From") = 0 'New record
            ''
            mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID)
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

            Dim DoneOnValue As String
            For i As Integer = 0 To mPrevAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
                If i = 0 Then
                    DoneOnValue = mPrevAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).DoneOnValueFormatted
                Else
                    DoneOnValue = DoneOnValue + " " + mPrevAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).DoneOnValueFormatted
                End If
            Next

            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorServiceStatusListNew(ID).Reference + "->" + mAssemblyMonitorServiceStatusListNew(ID).Type + "->" + mAssemblyMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorServiceStatusListNew(ID).Description


            'Changed by Vikrant on 26-July-2011
            mAircraft = cmbAircraftList.SelectedItem.ToString
            mMonitorInfo = mAssemblyMonitorServiceStatusListNew(ID).Type
            mMonitorType = mAssemblyMonitorServiceStatusListNew(ID).MonitorType
            mMonitorDesc = mAssemblyMonitorServiceStatusListNew(ID).Description
            mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & mAssemblyMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString & " Done On Value : " & DoneOnValue
            MarkLog(Util.Action.Comply, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx'); ", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        'Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        'Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)

        'If mPrevAssemblyMonitorServiceStatus.IsMaster And mPrevAssemblyMonitorServiceStatus.IsApplicable And chkApplicable.Checked = False Then
        '    'MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit the record.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
        '    MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'ElseIf (mPrevAssemblyMonitorServiceStatus.IsMaster) And (Not mPrevAssemblyMonitorServiceStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records
        '    Session("mAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        '    Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        '    Session("From") = 1 'Edit record
        '    ''
        '    'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        '    Session("mMachine") = mMachine
        '    Session("mAssemblyStatus") = mAssemblyStatus

        '    'Added by Saylee on 29-June-2009
        '    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        '    Session("mBoardInfo") = mBoardInfo
        '    '**************************************

        '    'Added By Vikrant On 25-Nov-2014
        '    If mPrevAssemblyMonitorServiceStatus.IsAttachmentAdded Then
        '        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevAssemblyMonitorServiceStatus.ID) 'Sort = 1 - Installation
        '        Session("mFileAttach") = mFileAttach
        '    Else
        '        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevAssemblyMonitorServiceStatus.ID)
        '        Session("mFileAttach") = mFileAttach
        '    End If
        '    'End

        '    Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description

        '    ' ''GetMachineMaintenance(mPrevAssemblyMonitorServiceStatus.ID)    'Added by Saylee on 7-Oct-2009
        '    RemoveSession()
        '    ''MarkLog(Util.Action.Edit, "ComplyAssemblyMonitorServiceStatus", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        '    '**********************************************************************
        '    'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        'ElseIf ((mPrevAssemblyMonitorServiceStatus.IsMaster = False) And (mPrevAssemblyMonitorServiceStatus.IsCompleted = False) And mPrevAssemblyMonitorServiceStatus.IsDone = False) Then

        '    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)

        '    Dim mModelMonitorService As ModelMonitorService = ModelMonitorService.GetModelMonitorService(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelMonitorServiceID, mMachine.HourType)
        '    Session("mModelMonitorService") = mModelMonitorService

        '    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
        '    Session("mMachine") = mMachine
        '    Session("mAssemblyStatus") = mAssemblyStatus
        '    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        '    Session("From") = 1 'Edit record
        '    Session("NewPage") = "True"
        '    '    Response.Redirect("wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx")
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
        '    '**********************************************************************

        'Else
        '    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType, True)
        '    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        '    Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        '    Session("From") = 1 'Edit record
        '    ''
        '    'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        '    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
        '    Session("mMachine") = mMachine
        '    Session("mAssemblyStatus") = mAssemblyStatus

        '    'Added by Saylee on 29-June-2009
        '    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        '    Session("mBoardInfo") = mBoardInfo
        '    '**************************************

        '    'Added By Vikrant On 25-Nov-2014
        '    If mAssemblyMonitorServiceStatus.IsAttachmentAdded Then
        '        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID) 'Sort = 1 - Installation
        '        Session("mFileAttach") = mFileAttach
        '    Else
        '        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID)
        '        Session("mFileAttach") = mFileAttach
        '    End If
        '    'End

        '    Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description

        '    ' ''GetMachineMaintenance(mPrevAssemblyMonitorServiceStatus.ID)    'Added by Saylee on 7-Oct-2009
        '    RemoveSession()
        '    'Changed by Vikrant on 26-July-2011
        '    mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineInfo
        '    mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo
        '    mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType
        '    mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(Index).Description
        '    mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date :" & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneOnValueFormatted
        '    MarkLog(Util.Action.Edit, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        'End If
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(ID, mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID, mMachine.HourType)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID)

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

            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorServiceStatusListNew(ID).Reference + "->" + mAssemblyMonitorServiceStatusListNew(ID).Type + "->" + mAssemblyMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorServiceStatusListNew(ID).Description
            RemoveSession()
            ''MarkLog(Util.Action.Edit, "ComplyAssemblyMonitorServiceStatus", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
            '**********************************************************************
            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevAssemblyMonitorServiceStatus.IsMaster = False) And (mPrevAssemblyMonitorServiceStatus.IsCompleted = False) And mPrevAssemblyMonitorServiceStatus.IsDone = False) Then
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(ID, mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID, mMachine.HourType)

            Dim mModelMonitorService As ModelMonitorService = ModelMonitorService.GetModelMonitorService(mAssemblyMonitorServiceStatusListNew(ID).ModelMonitorServiceID, mMachine.HourType)
            Session("mModelMonitorService") = mModelMonitorService

            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID)
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
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID)
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

            Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorServiceStatusListNew(ID).Reference + "->" + mAssemblyMonitorServiceStatusListNew(ID).Type + "->" + mAssemblyMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorServiceStatusListNew(ID).Description
            Dim DoneOnValue As String
            For i As Integer = 0 To mPrevAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
                If i = 0 Then
                    DoneOnValue = mPrevAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).DoneOnValueFormatted
                Else
                    DoneOnValue = DoneOnValue + " " + mPrevAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).DoneOnValueFormatted
                End If
            Next

            'Changed by Vikrant on 26-July-2011
            mAircraft = cmbAircraftList.SelectedItem.ToString
            mMonitorInfo = mAssemblyMonitorServiceStatusListNew(ID).Type
            mMonitorType = mAssemblyMonitorServiceStatusListNew(ID).MonitorType
            mMonitorDesc = mAssemblyMonitorServiceStatusListNew(ID).Description
            mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & mAssemblyMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString & " Done On Value : " & DoneOnValue
            MarkLog(Util.Action.Edit, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)

            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub HistoryRecords(ByVal ID As Guid)  'Added by Saylee on 09-Sep-2009
        'mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        'Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        'Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        ''If mPrevAssemblyMonitorServiceStatus.IsMaster Then
        ''    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Service Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        ''    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfComplyAssemblyMonitorServiceStatusListShowValues_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        ''    msg.Show()
        ''    Exit Sub
        ''Else
        'mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        'Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        'Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        'Session("From") = 1 'Edit record
        '''
        ''Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)
        'Session("mMachine") = mMachine
        'Session("mAssemblyStatus") = mAssemblyStatus

        ''Added by Saylee on 29-June-2009
        'mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        'Session("mBoardInfo") = mBoardInfo
        ''**************************************
        'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description

        'Session("ATA") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ATA.ToString
        'Session("Description") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).Description
        'Session("ModelSerialNo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).ModelSerialNo

        'mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine.HourType)
        'Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList


        ''RemoveSession()
        ''Added by Vikrant on 3-Aug-2011
        'mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineInfo
        'mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo
        'mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType
        'mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(Index).Description
        'mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc
        'MarkLog(Util.Action.View, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        '''MarkLog(Util.Action.Edit, "ComplyAssemblyMonitorServiceStatus", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
        ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryAssemblyMonitorServiceStatusList.aspx?GChildPage2=Index.aspx');", True)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow()", True)
        ''End If
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(ID, mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID, mMachine.HourType)
        
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorServiceStatusListNew(ID).AssemblyStatusID)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************
        Session("mAssemblyInfo") = cmbAircraftList.SelectedItem.ToString + "->" + "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]" + "->" + mAssemblyMonitorServiceStatusListNew(ID).Reference + "->" + mAssemblyMonitorServiceStatusListNew(ID).Type + "->" + mAssemblyMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mAssemblyMonitorServiceStatusListNew(ID).Description

        Session("ATA") = mAssemblyMonitorServiceStatusListNew(ID).ATACode.ToString
        Session("Description") = mAssemblyMonitorServiceStatusListNew(ID).Description
        Session("ModelSerialNo") = "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"

        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList


        'RemoveSession()
        'Added by Vikrant on 3-Aug-2011
        mAircraft = cmbAircraftList.SelectedItem.ToString
        mMonitorInfo = mAssemblyMonitorServiceStatusListNew(ID).Type
        mMonitorType = mAssemblyMonitorServiceStatusListNew(ID).MonitorType
        mMonitorDesc = mAssemblyMonitorServiceStatusListNew(ID).Description
        mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date : " & mAssemblyMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString

        MarkLog(Util.Action.View, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ''MarkLog(Util.Action.Edit, "ComplyAssemblyMonitorServiceStatus", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryAssemblyMonitorServiceStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow()", True)
        'End If
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAssemblyMonitorServiceStatusListNew.CurrentIndex = mAssemblyMonitorServiceStatusListNew(ID, "")
        Session("mAssemblyMonitorServiceStatusListNew") = mAssemblyMonitorServiceStatusListNew
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
                            ''Added by vikrant on 26-July-2011
                            'IDForEventLog = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID
                            'mAircraft = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).MachineInfo
                            'mMonitorInfo = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).ModelMonitorServiceInfo
                            'mMonitorType = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).MonitorType
                            'mMonitorDesc = mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).Description
                            'mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date :" & mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).DoneOnFormatted & " Done On Value : " & mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).DoneOnValueFormatted
                            ''End
                            ''Added by Saylee on 28-May-2009
                            'mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.AssemblyMonitorServiceStatusID)
                            ''********************************
                            ''Added by Saylee on 6th-Oct-2009
                            'mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.AssemblyMonitorServiceStatusID, 5)
                            ''=============================
                            'If mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).IsAttachmentAdded = True Then
                            '    mFileAttach = FileAttach.GetAttachment(mTmpComplyAssemblyMonitorServiceStatusList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID)
                            'End If
                            'AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.AssemblyMonitorServiceStatusID)
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
                            ''Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                            'If AppSettings("LinkMaintenance") = "True" Then
                            '    If LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorServiceStatusList.CurrentItem.ModelMonitorServiceID.ToString).Count > 0 Then
                            '        MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                            '        Exit Sub
                            '    End If
                            'End If
                            ''End

                            Session("sender") = ""
                            'Added by vikrant on 26-July-2011
                            IDForEventLog = mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).ID
                            mAircraft = cmbAircraftList.SelectedItem.ToString
                            mMonitorInfo = mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).Type
                            mMonitorType = mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).MonitorType
                            mMonitorDesc = mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).Description
                            mAssemblyMonitorDetail = "Aircraft : " & mAircraft & " Monitor Info. : " & mMonitorInfo & " Monitor Type : " & mMonitorType & " Description : " & mMonitorDesc & " Done On Date :" & mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).DoneOnFormatted
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).ID)
                            '********************************
                            'Added by Saylee on 6th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).ID, 5)
                            '=============================
                            If mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).ID)
                            End If
                            AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).ID)
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
                            'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                            If AppSettings("LinkMaintenance") = "True" Then
                                If LinkMaintenanceList.GetLinkMaintenanceList(mAssemblyMonitorServiceStatusListNew(mAssemblyMonitorServiceStatusListNew.CurrentIndex).ModelMonitorServiceID.ToString).Count > 0 Then
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
                                MarkLog(Util.Action.Delete, "AssemblyServiceMonitor", "Can't delete :" & mAssemblyMonitorDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyServiceMonitor", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
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
        'mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, cmbAircraftList.SelectedValue, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , cmbMonitorType.SelectedValue, , , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue")
        mAssemblyMonitorServiceStatusListNew = AssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatuslist(CurrentDate:=txtDate.Text, AssemblyStatusPeriodList:=Nothing, AssemblyID:=New Guid(cmbAircraftAssembly.SelectedValue), MonitorTypeID:=CType(cmbMonitorType.SelectedValue, Integer), MachineID:=cmbAircraftList.SelectedValue.ToString, IsServiceStatusPeriodsRequired:=False, IsForConfiguredList:=True, IsComplied:=True, CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True))
        'Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo In mTmpComplyAssemblyMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
        'End If
        'Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                       Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                     Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        Session("mAssemblyMonitorServiceStatusListNew") = mAssemblyMonitorServiceStatusListNew
        dgDueMonitoringList.DataBind()
        SetGrid()
        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
    End Sub
    Private Sub SetPage()
        'If RecordsToShow < mTmpComplyAssemblyMonitorServiceStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    lblResult.Text = "List of Assembly Service Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyAssemblyMonitorServiceStatusList.Count & " Record(s) shown."
        'Else
        '    lblResult.Text = "List of Assembly Service Status as per selected criteria : " & mTmpComplyAssemblyMonitorServiceStatusList.Count & " Record(s) found."
        'End If
        Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                     Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                   Select StatusInfo).ToList
        If RecordsToShow < List.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
            lblResult.Text = "List of Assembly Service Status as per selected criteria : " & RecordsToShow.ToString & " of " & List.Count.ToString & " Record(s) shown."
        Else
            lblResult.Text = "List of Assembly Service Status as per selected criteria : " & List.Count.ToString & " Record(s) found."
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

        For j As Integer = 0 To dgDueMonitoringList.Rows.Count - 1
            B = CType(Me.dgDueMonitoringList.Rows(j).Cells(22).Text, Boolean)
            c = CType(Me.dgDueMonitoringList.Rows(j).Cells(24).Text, Boolean)
            If B = True Then
                dgDueMonitoringList.Rows(j).Cells(21).Enabled = False
            End If
            If c = False Then
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = False
            End If

            'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgDueMonitoringList.Rows(j).Cells(18).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(19).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(20).Enabled = False
                btnAddNewTop.Enabled = False
                btnAddNew.Enabled = False
                lblReadOnly.Visible = True
            Else
                dgDueMonitoringList.Rows(j).Cells(18).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(19).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(20).Enabled = True
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
        '    Dim List = (From StatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo In mTmpComplyAssemblyMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
        'End If
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            Dim List = (From StatusInfo As AssemblyMonitorInspStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        dgDueMonitoringList.DataBind()
        SetGrid()
        dgDueMonitoringList.Columns(18).Visible = IIf(chkApplicable.Checked, False, True)
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

        'If mMachineNameValueList.Count > 1 And (IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString) Then mMachineId = mMachineNameValueList(1).ID Else mMachineId = New Guid(AircraftId)

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
        txtCodeFormNo.Text = CodeFormNoDesc
        chkApplicable.Checked = ShowNotApplicable 'Added by Saylee on 7-Jan-2011
        '-----------------------------------------
        'mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(DoneOn, cmbAircraftList.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""), , , , CType(MonitorTypeID, Integer), , , ShowNotApplicable, IIf(chkOneTimeMasterRecords.Checked, False, True), SortBy:="MinimumRemainingValue")
        mAssemblyMonitorServiceStatusListNew = AssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatuslist(CurrentDate:=DoneOn, AssemblyStatusPeriodList:=Nothing, AssemblyID:=mAssemblylist(cmbAircraftAssembly.SelectedIndex).ID, MonitorTypeID:=CType(MonitorTypeID, Integer), MachineID:=cmbAircraftList.SelectedValue.ToString, IsServiceStatusPeriodsRequired:=False, IsForConfiguredList:=True, IsComplied:=True, CodeFormNoDesc:=CodeFormNoDesc, IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True))
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        Session("mAssemblyMonitorServiceStatusListNew") = mAssemblyMonitorServiceStatusListNew
        dgDueMonitoringList.DataBind()  'Added Code



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
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            cmbAircraftList.Focus()
            Session("MiddleFrame") = "wfComplyAssemblyMonitorServiceStatusListShowValues_Ajax.aspx?"
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
        RemoveSession()
        Session.Remove("DoneOn")
        Session.Remove("AircraftId")
        Session.Remove("From")
        Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
        Session.Remove("AssemblyId")
        Session.Remove("SkipOneTimeDoneMRecords")
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
        ControlVisibility()
        SetPage()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub dgDueMonitoringList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueMonitoringList.RowCommand
        Dim AssemblyMonitorServiceStatusID As Guid
        Dim Model, SerialNo As String
        Select Case e.CommandName
            Case "Comply"
                If Not User.IsInRole("AssemblyServiceMonitorNew") Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                ComplyRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "EditRec"
                If (Not User.IsInRole("AssemblyServiceMonitorView") And Not User.IsInRole("AssemblyServiceMonitorEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                EditRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "DeleteRec"
                If (Not User.IsInRole("AssemblyServiceMonitorDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                DeleteRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "History" 'Added by Saylee on 09-Sep-2009
                If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'GridBind()
                'dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                HistoryRecords(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "ViewRec"
                'GridBind()
                'dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
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
                        Dim Str As String
                        Str = ""
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
            Case "ShowVal"
                'GridBind()
                'dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
                Dim AssemblyMonitorServiceStatusIDs As New StringBuilder
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                AssemblyMonitorServiceStatusIDs.Append("<AssMonServiceID>")
                AssemblyMonitorServiceStatusIDs.Append("<id>")
                AssemblyMonitorServiceStatusIDs.Append(New Guid(currentRow.Cells(0).Text))
                AssemblyMonitorServiceStatusIDs.Append("</id>")
                AssemblyMonitorServiceStatusIDs.Append("</AssMonServiceID>")

                'GridBind()
                'SetGrid()
                'ControlVisibility()
                AssemblyMonitorServiceStatusID = New Guid(currentRow.Cells(0).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyMonitorServiceStatusListNew(AssemblyMonitorServiceStatusID).AssemblyStatusID)
                Model = mAssemblyStatus.Assembly.ModelName
                SerialNo = mAssemblyStatus.Assembly.SerialNo
                'AssemblyId = New Guid(currentRow.Cells(3).Text)
                Dim mtmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
                mtmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList([Date]:=Today.Date.ToString, MachineID:=cmbAircraftList.SelectedValue.ToString, Model:=Model, SerialNo:=SerialNo, AssemblyMonitorServiceStatusIDs:=AssemblyMonitorServiceStatusIDs.ToString, ShowNotApplicable:=chkApplicable.Checked)
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
        'mTmpComplyAssemblyMonitorServiceStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
        mAssemblyMonitorServiceStatusListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        'Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo In mTmpComplyAssemblyMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
        'End If
        'Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
            Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        'Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        Session("mAssemblyMonitorServiceStatusListNew") = mAssemblyMonitorServiceStatusListNew
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
            ScriptManager1.AsyncPostBackErrorMessage = _
               e.Exception.Message & _
               e.Exception.Data("ExtraInfo").ToString()
        Else
            ScriptManager1.AsyncPostBackErrorMessage = _
               "An unspecified error occurred."
        End If
    End Sub
    Private Sub lnkShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAllRecords.Click, lnkShowAllRecordsTop.Click
        'RecordsToShow = mTmpComplyAssemblyMonitorServiceStatusList.Count
        'Session("RecordsToShow") = RecordsToShow
        ''Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        ''                                               Select StatusInfo).ToList.Take(RecordsToShow)
        'dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
        'dgDueMonitoringList.DataBind()
        'RecordsToShow = mTmpComplyAssemblyMonitorInspStatusList.Count
        RecordsToShow = mAssemblyMonitorServiceStatusListNew.Count
        Session("RecordsToShow") = RecordsToShow
        Dim List = (From StatusInfo As AssemblyMonitorServiceStatusInfo In mAssemblyMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
        dgDueMonitoringList.DataSource = List
        dgDueMonitoringList.DataBind()
        lnkShowAllRecords.Enabled = False
        lnkShowAllRecords.Enabled = False
        SetPage()
        SetGrid()
        ControlVisibility()
        upnlActionBtn.Update()
    End Sub
#End Region

#Region " Report "
    ' Created by - Rajnish on 22-06-2006 
#Region " Report Variable Declaration "
    'Dim mCompanyDetail As New CompanyDetail
    'Private SearchStr1 As String = ""
    'Private SearchStr2 As String = ""
    'Private SearchStr3 As String = ""
    'Private SearchStr4 As String = ""
    'Dim ShowNotApplicable As Boolean = False
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        '     If (Not User.IsInRole("AssemblyServiceMonitorPrint")) Then
        '         MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
        '         Exit Sub
        '     End If
        '     dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
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
        '     'ReportDetails.Add(New rptStatus(, 0, dgDueMonitoringList.CaptionText))
        '     ReportDetails.Add(New rptStatus(, 1, , _
        '          , , , dgDueMonitoringList.Columns.Item(0).HeaderText, , dgDueMonitoringList.Columns.Item(4).HeaderText, dgDueMonitoringList.Columns.Item(6).HeaderText, _
        '          dgDueMonitoringList.Columns.Item(7).HeaderText, dgDueMonitoringList.Columns.Item(8).HeaderText, _
        '          dgDueMonitoringList.Columns.Item(9).HeaderText, dgDueMonitoringList.Columns.Item(10).HeaderText, dgDueMonitoringList.Columns.Item(11).HeaderText, _
        '          dgDueMonitoringList.Columns.Item(12).HeaderText, dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText, _
        '          dgDueMonitoringList.Columns.Item(15).HeaderText, dgDueMonitoringList.Columns.Item(16).HeaderText, dgDueMonitoringList.Columns.Item(17).HeaderText, _
        '          , , , , , , , , , dgDueMonitoringList.Columns.Item(18).HeaderText))

        '     Dim TotalCount As Integer
        '     TotalCount = Me.mTmpComplyAssemblyMonitorServiceStatusList.Count
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

        '         If Me.dgDueMonitoringList.Rows(I).Cells(0).Text <> "&nbsp;" Then str(0) = Me.dgDueMonitoringList.Rows(I).Cells(0).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(4).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgDueMonitoringList.Rows(I).Cells(7).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgDueMonitoringList.Rows(I).Cells(8).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgDueMonitoringList.Rows(I).Cells(9).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgDueMonitoringList.Rows(I).Cells(10).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgDueMonitoringList.Rows(I).Cells(11).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(8) = Me.dgDueMonitoringList.Rows(I).Cells(12).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(13).Text <> "&nbsp;" Then str(9) = Me.dgDueMonitoringList.Rows(I).Cells(13).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(14).Text <> "&nbsp;" Then str(10) = Me.dgDueMonitoringList.Rows(I).Cells(14).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(15).Text <> "&nbsp;" Then str(11) = Me.dgDueMonitoringList.Rows(I).Cells(15).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(16).Text <> "&nbsp;" Then str(12) = Me.dgDueMonitoringList.Rows(I).Cells(16).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(17).Text <> "&nbsp;" Then str(13) = Me.dgDueMonitoringList.Rows(I).Cells(17).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(18).Text <> "&nbsp;" Then str(14) = Me.dgDueMonitoringList.Rows(I).Cells(18).Text.Replace("<BR>", vbCrLf).Replace("<BR>", vbCrLf)

        '         ReportDetails.Add(New rptStatus(, 2, , _
        '          , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), _
        '          str(7), str(8), str(9), str(10), str(11), str(12), str(13), , , , , , , , , , str(14)))
        '     Next
        '     mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        '     Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, "List of Comply Assembly Service Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        '     If mTmpComplyAssemblyMonitorServiceStatusList.Count = 0 Then
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
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#End Region

End Class