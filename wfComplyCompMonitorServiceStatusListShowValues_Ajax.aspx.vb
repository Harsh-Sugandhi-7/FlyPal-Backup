'Added by vikrant on 07-May-2019

Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text 'SV
Public Class wfComplyCompMonitorServiceStatusListShowValues_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    'Public mTmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
    Public mCompMonitorServiceStatusListNew As CompMonitorServiceStatusList 'SV  
    Public mAssemblylist As AssemblyList
    Public DoneOn As String
    Public AircraftId As String
    Public AssemblyId As String
    Public mCompInfo As String   'Added Code  Jan,29,2007
    Public ComplyCompMonitorServiceInfo As String   'Added Code   Jan,29,2007
    'Public mInstallCompStatus As CompStatus  'Added Code
    Public mMachine As Machine
    Public PartNo As String = String.Empty

    Private mPartMonitorServiceTypeList As PartMonitorServiceTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String = String.Empty 'Added by Saylee on 30-July-2009

    Private mUpdateComplyHistoryCompMonitorServiceStatusList As UpdateComplyHistoryCompMonitorServiceStatusList

    'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance

    Dim ShowNotApplicable As Boolean = False
    Dim ShowOneTimeMasterRecords As Boolean = False

    Dim EventLogID As Guid 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim IDForEventLog As Guid
    'Added By Prashant On 27-Nov-2014
    Dim mFileAttach As FileAttach
    Dim RecordsToShow As Integer
    Dim IsReadOnly As Boolean 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Dim SerialNo As String = String.Empty
    Dim Part As String = String.Empty
    Dim CodeFormNoDesc As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        'mTmpComplyCompMonitorServiceStatusList = CType(Session("mTmpComplyCompMonitorServiceStatusList"), tmpComplyCompMonitorServiceStatusList)
        mCompMonitorServiceStatusListNew = CType(Session("mCompMonitorServiceStatusListNew"), CompMonitorServiceStatusList) 'SV
        DoneOn = CType(Session("DoneOn"), String)
        AircraftId = CType(Session("AircraftId"), String)
        AssemblyId = CType(Session("AssemblyId"), String)
        '   mInstallCompStatus = CType(Session("InstallCompStatus"), CompStatus)

        'Added by Rahul on 29-Apr-2009
        PartNo = CType(Session("PartNo"), String)
        SerialNo = CType(Session("SerialNo"), String)
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        ShowNotApplicable = CType(Session("ShowNotApplicable"), Boolean) 'Added by Saylee on 7th-Jan-2011
        ShowOneTimeMasterRecords = CType(Session("ShowOneTimeMasterRecords"), Boolean)
        RecordsToShow = CType(Session("RecordsToShow"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        CodeFormNoDesc = Session("CodeFormNoDesc")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblylist")
        Session.Remove("mMachineNameValueList")
        'Session.Remove("mTmpComplyCompMonitorServiceStatusList")
        Session.Remove("mCompMonitorServiceStatusListNew") 'SV
        Session.Remove("RecordsToShow")
        ' Session.Remove("mInstallCompStatus")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyCompMonitorServiceStatusListShowValues_Ajax.aspx?" Then
            Session.Remove("mAssemblylist")
            Session.Remove("mMachineNameValueList")
            'Session.Remove("mTmpComplyCompMonitorServiceStatusList")
            Session.Remove("mCompMonitorServiceStatusListNew") 'SV
            Session.Remove("DoneOn")
            Session.Remove("AircraftId")
            Session.Remove("AssemblyId")
            'Added by Rahul on 29-Apr-2009
            Session.Remove("PartNo")
            Session.Remove("SerialNo")
            ''====================
            Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009

            Session.Remove("mMachineMaintenance") 'Added by Saylee on 9th-Oct-2009
            Session.Remove("ShowNotApplicable") 'Added by Saylee on 7th-Oct-2010
            Session.Remove("ShowOneTimeMasterRecords")
            Session.Remove("RecordsToShow")
            Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            Session.Remove("CodeFormNoDesc")
        End If
    End Sub
    Private Sub EnableLinks()
        'If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
        '    If RecordsToShow < mTmpComplyCompMonitorServiceStatusList.Count Then
        '        lnkShowAllRecords.Enabled = True
        '        lnkShowAllRecordsTop.Enabled = True
        '    Else
        '        lnkShowAllRecords.Enabled = False
        '        lnkShowAllRecordsTop.Enabled = False
        '    End If
        'End If
        'SV
        If Not mCompMonitorServiceStatusListNew Is Nothing Then
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
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
        'End
    End Sub
    Private Sub ControlVisibility()
        'SV
        ''btnPrint.Enabled = (mTmpComplyCompMonitorServiceStatusList.Count > 0)
        ''btnPrintTop.Enabled = (mTmpComplyCompMonitorServiceStatusList.Count > 0)
        'End
        dgDueMonitoringList.Columns(20).Visible = IIf(chkApplicable.Checked, False, True)
        EnableLinks()
    End Sub
    Private Sub FindNow()
        RecordsToShow = dgDueMonitoringList.PageSize
        Session("RecordsToShow") = RecordsToShow

        Session("DoneOn") = txtDate.Text
        Session("AircraftId") = cmbAircraftList.SelectedValue
        Session("AssemblyId") = cmbAssembly.SelectedValue
        'Added By Rahul on 29-Apr-2009
        Session("PartNo") = Trim(txtPart.Text)
        Session("SerialNo") = Trim(txtSerialNo.Text)
        '==================================
        Session("ShowNotApplicable") = chkApplicable.Checked  'Added by Saylee on 7-Jan-2011
        Session("ShowOneTimeMasterRecords") = chkOneTimeMasterRecords.Checked
        Session("CodeFormNoDesc") = Trim(txtCodeFormNo.Text)

        dgDueMonitoringList.PageIndex = 0
        'mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked = True, False, True), SortBy:="MinimumRemainingValue")
        mCompMonitorServiceStatusListNew = CompMonitorServiceStatusList.GetCompMonitorServiceStatusList(MachineID:=cmbAircraftList.SelectedValue, CurrentDate:=txtDate.Text, SerialNo:=Trim(txtSerialNo.Text), IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True), CompID:=Guid.Empty, CompStatusPeriodList:=Nothing, PartName:=Trim(txtPart.Text), AssemblyID:=cmbAssembly.SelectedValue, MonitorTypeID:=CType(cmbMonitorType.SelectedValue, Integer), IsApplicable:=Not (ShowNotApplicable), IsRecordsDirectFetch:=True, IsMaster:=False, IsComplied:=True, IsServicePeriodsRequired:=False, CodeFormNoDesc:=Trim(txtCodeFormNo.Text)) 'SV
        'Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        'End If
        'Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                       Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                     Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                       Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                     Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        Session("mCompMonitorServiceStatusListNew") = mCompMonitorServiceStatusListNew
        dgDueMonitoringList.DataBind()
        SetPage()
        ControlVisibility()
        SetGrid()
        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
    End Sub
    Private Sub ComplyRecord(ByVal ID As Guid)
        ''frm.ComplyCompMonitorServiceInfo = mtmpComplyCompMonitorServiceStatusList(dgDueMonitoringList.CurrentRowIndex).PartMonitorServiceInfo
        ''        ComplyCompMonitorServiceInfo = mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).PartMonitorServiceInfo
        ''       Dim mCompInfo As String = "[Part: " & mInstallCompStatus.PartName & " Serial No.: " & mInstallCompStatus.SerialNo & " ]"
        'Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        'Dim mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
        'Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(Index).CompStatusID, mMachine.HourType, , mCompStatus)

        'If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'Else
        '    mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, txtDate.Text, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
        '    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        '    Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        '    Session("EnFrom") = 0 'NewRecord
        '    'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        '    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
        '    mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
        '    Session("mMachine") = mMachine
        '    Session("mCompStatus") = mCompStatus
        '    Session("mAssemblyStatus") = mAssemblyStatus
        '    'Rajnish 21-07-2008
        '    mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
        '    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

        '    'Added By Vikrant On 25-Nov-2014
        '    Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID) 'Sort = 1 : Installation
        '    Session("mFileAttach") = mFileAttach
        '    'End

        '    RemoveSession()
        '    'Added by Saylee on 5-Aug-2009
        '    mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        '    Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        '    ''*****************************************

        '    'Added By Utkarsh On 28-Jul-2011 For All19072011

        '    MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnValueFormatted
        '    MarkLog(Util.Action.Comply, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, EventLogID)

        '    'End

        '    ''MarkLog(Util.Action.[New], "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, Guid.Empty)
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
        '       End If
        'SV
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mCompMonitorServiceStatusListNew(ID).CompStatusID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString)
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).CompStatusID, mMachine.HourType, , mCompStatus)

        If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, txtDate.Text, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("EnFrom") = 0 'NewRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMonitorServiceStatusListNew(ID).AssemblyStatusID)
            mCompStatus = CompStatus.GetCompStatus(mCompMonitorServiceStatusListNew(ID).CompStatusID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString)
            Session("mMachine") = mMachine
            Session("mCompStatus") = mCompStatus
            Session("mAssemblyStatus") = mAssemblyStatus
            'Rajnish 21-07-2008
            mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End

            Dim DoneOnValue As String
            For i As Integer = 0 To mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
                If i = 0 Then
                    DoneOnValue = mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DoneOnValueFormatted
                Else
                    DoneOnValue = DoneOnValue + " " + mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DoneOnValueFormatted
                End If
            Next

            RemoveSession()
            'Added by Saylee on 5-Aug-2009
            mCompInfo = cmbAircraftList.SelectedItem.ToString + "->" + mAssemblyStatus.Assembly.ModelName + vbCrLf + mAssemblyStatus.Assembly.SerialNo + vbCrLf + "->" + "[Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & " ]" + "->" + mCompMonitorServiceStatusListNew(ID).Reference + "->" + mCompMonitorServiceStatusListNew(ID).Type + "->" + mCompMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mCompMonitorServiceStatusListNew(ID).Description
            Session("mCompInfo") = mCompInfo
            ''*****************************************

            'Added By Utkarsh On 28-Jul-2011 For All19072011
            MaintDetail = "Reg No. : " + cmbAircraftList.SelectedItem.ToString & " Assembly Info : " & mAssemblyStatus.Assembly.ModelName + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " & mCompStatus.Description & " " & mCompStatus.SerialNo & " " & mCompStatus.Position & " Monitor Info : " & mCompMonitorServiceStatusListNew(ID).Type & " Done On Date : " & mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString & " Done On Value : " & DoneOnValue
            MarkLog(Util.Action.Comply, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, ID, EventLogID)
            'End

            ''MarkLog(Util.Action.[New], "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, Guid.Empty)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
            'End
        End If
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        'Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)

        'Dim mCompStatus As CompStatus
        'mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)

        'Dim mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(Index).CompStatusID, mMachine.HourType, , mCompStatus)

        'If mPrevCompMonitorServiceStatus.IsMaster And mPrevCompMonitorServiceStatus.IsApplicable And chkApplicable.Checked = False Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit the component.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'ElseIf (mPrevCompMonitorServiceStatus.IsMaster) And (Not mPrevCompMonitorServiceStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records

        '    Session("mCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        '    Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        '    Session("EnFrom") = 1 'EditRecord
        '    'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        '    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
        '    Session("mMachine") = mMachine
        '    Session("mAssemblyStatus") = mAssemblyStatus
        '    Session("mCompStatus") = mCompStatus

        '    'Added By Vikrant On 25-Nov-2014
        '    If mPrevCompMonitorServiceStatus.IsAttachmentAdded Then
        '        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevCompMonitorServiceStatus.ID) 'Sort = 1 - Installation
        '        Session("mFileAttach") = mFileAttach
        '    Else
        '        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevCompMonitorServiceStatus.ID)
        '        Session("mFileAttach") = mFileAttach
        '    End If
        '    'End

        '    RemoveSession()
        '    'Added by Saylee on 5-Aug-2009
        '    mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        '    Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        '    ''*****************************************


        '    ''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
        '    '**********************************************************************
        '    'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        'ElseIf ((mPrevCompMonitorServiceStatus.IsMaster = False) And (mPrevCompMonitorServiceStatus.IsCompleted = False) And mPrevCompMonitorServiceStatus.IsDone = False) Then

        '    mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mMachine.HourType, True)

        '    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)

        '    mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)

        '    Dim mPartMonitorService As PartMonitorService = PartMonitorService.GetPartMonitorService(mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceID, mMachine.HourType)
        '    Session("mPartMonitorService") = mPartMonitorService

        '    Session("mMachine") = mMachine
        '    Session("mAssemblyStatus") = mAssemblyStatus
        '    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        '    Session("mCompStatus") = mCompStatus
        '    Session("EnFrom") = 1
        '    Session("From") = 1 'Edit record
        '    Session("NewPage") = "True"
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
        '    '**********************************************************************
        'Else

        '    'mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, calDate.Value.ToString, mMachine.HourType)
        '    mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mMachine.HourType, True)

        '    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        '    Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        '    Session("EnFrom") = 1 'EditRecord
        '    'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        '    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
        '    mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
        '    Session("mMachine") = mMachine
        '    Session("mAssemblyStatus") = mAssemblyStatus
        '    Session("mCompStatus") = mCompStatus

        '    'Added By Vikrant On 25-Nov-2014
        '    If mCompMonitorServiceStatus.IsAttachmentAdded Then
        '        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID) 'Sort = 1 - Installation
        '        Session("mFileAttach") = mFileAttach
        '    Else
        '        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID)
        '        Session("mFileAttach") = mFileAttach
        '    End If
        '    'End

        '    RemoveSession()
        '    'Added by Saylee on 5-Aug-2009
        '    mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        '    Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        '    ''*****************************************

        '    ''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
        'End If
        ''Added By Utkarsh On 28-Jul-2011 For All19072011
        'MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnValueFormatted
        'MarkLog(Util.Action.Edit, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, EventLogID)
        ''End

        'SV
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        Dim mAssemblyStatus As AssemblyStatus
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))

        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(mCompMonitorServiceStatusListNew(ID).CompStatusID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString)

        Dim mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).CompStatusID, mMachine.HourType, , mCompStatus)

        If mPrevCompMonitorServiceStatus.IsMaster And mPrevCompMonitorServiceStatus.IsApplicable And chkApplicable.Checked = False Then
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit the component.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mPrevCompMonitorServiceStatus.IsMaster) And (Not mPrevCompMonitorServiceStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records

            Session("mCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("EnFrom") = 1 'EditRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMonitorServiceStatusListNew(ID).AssemblyStatusID)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompStatus") = mCompStatus

            'Added By Vikrant On 25-Nov-2014
            If mPrevCompMonitorServiceStatus.IsAttachmentAdded Then
                Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevCompMonitorServiceStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevCompMonitorServiceStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            RemoveSession()
            'Added by Saylee on 5-Aug-2009
            mCompInfo = cmbAircraftList.SelectedItem.ToString + "->" + mAssemblyStatus.Assembly.ModelName + vbCrLf + mAssemblyStatus.Assembly.SerialNo + vbCrLf + "->" + "[Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & " ]" + "->" + mCompMonitorServiceStatusListNew(ID).Reference + "->" + mCompMonitorServiceStatusListNew(ID).Type + "->" + mCompMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mCompMonitorServiceStatusListNew(ID).Description
            Session("mCompInfo") = mCompInfo
            ''*****************************************


            ''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
            '**********************************************************************
            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevCompMonitorServiceStatus.IsMaster = False) And (mPrevCompMonitorServiceStatus.IsCompleted = False) And mPrevCompMonitorServiceStatus.IsDone = False) Then

            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).CompStatusID, mMachine.HourType, True)

            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMonitorServiceStatusListNew(ID).AssemblyStatusID)

            mCompStatus = CompStatus.GetCompStatus(mCompMonitorServiceStatusListNew(ID).CompStatusID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString)

            Dim mPartMonitorService As PartMonitorService = PartMonitorService.GetPartMonitorService(mCompMonitorServiceStatusListNew(ID).PartMonitorServiceID, mMachine.HourType)
            Session("mPartMonitorService") = mPartMonitorService

            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Session("mCompStatus") = mCompStatus
            Session("EnFrom") = 1
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************
        Else

            'mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, calDate.Value.ToString, mMachine.HourType)
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mMachine.HourType, True)

            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("EnFrom") = 1 'EditRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMonitorServiceStatusListNew(ID).AssemblyStatusID)
            mCompStatus = CompStatus.GetCompStatus(mCompMonitorServiceStatusListNew(ID).CompStatusID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompStatus") = mCompStatus

            'Added By Vikrant On 25-Nov-2014
            If mCompMonitorServiceStatus.IsAttachmentAdded Then
                Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            RemoveSession()
            'Added by Saylee on 5-Aug-2009
            mCompInfo = cmbAircraftList.SelectedItem.ToString + "->" + mAssemblyStatus.Assembly.ModelName + vbCrLf + mAssemblyStatus.Assembly.SerialNo + vbCrLf + "->" + "[Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & " ]" + "->" + mCompMonitorServiceStatusListNew(ID).Reference + "->" + mCompMonitorServiceStatusListNew(ID).Type + "->" + mCompMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mCompMonitorServiceStatusListNew(ID).Description
            Session("mCompInfo") = mCompInfo
            ''*****************************************

            ''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
        End If
        Dim DoneOnValue As String
        For i As Integer = 0 To mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            If i = 0 Then
                DoneOnValue = mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DoneOnValueFormatted
            Else
                DoneOnValue = DoneOnValue + " " + mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DoneOnValueFormatted
            End If
        Next
        'Added By Utkarsh On 28-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + cmbAircraftList.SelectedItem.ToString & " Assembly Info : " & mAssemblyStatus.Assembly.ModelName + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " & mCompStatus.Description & " " & mCompStatus.SerialNo & " " & mCompStatus.Position & " Monitor Info : " & mCompMonitorServiceStatusListNew(ID).Type & " Done On Date : " & mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString & " Done On Value : " & DoneOnValue
        MarkLog(Util.Action.Edit, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, ID, EventLogID)
        'End
        'End
    End Sub
    Private Sub HistoryRecords(ByVal ID As Guid)
        'Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        'Dim mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(Index).CompStatusID, mMachine.HourType)
        ''If mPrevCompMonitorServiceStatus.IsMaster Then
        ''    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfComplyCompMonitorServiceStatusListShowValues_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        ''    msg.Show()
        ''    Exit Sub
        ''Else
        ''mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, calDate.Value.ToString, mMachine.HourType)
        'mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)

        'Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        'Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        'Session("EnFrom") = 1 'EditRecord
        ''Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        'Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
        'Dim mCompStatus As CompStatus
        'mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
        'Session("mMachine") = mMachine
        'Session("mAssemblyStatus") = mAssemblyStatus
        'Session("mCompStatus") = mCompStatus
        ''RemoveSession()
        ''Added by Saylee on 5-Aug-2009
        'mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        'Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        '''*****************************************

        'Session("ATA") = mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString
        'Session("Description") = mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
        'Session("PartSerialNo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo

        'mUpdateComplyHistoryCompMonitorServiceStatusList = UpdateComplyHistoryCompMonitorServiceStatusList.GetComplyHistoryCompMonitorServiceStatusList(mCompStatus.CompID, mCompMonitorServiceStatus.PartMonitorServiceID, mMachine.HourType)
        'Session("mUpdateComplyHistoryCompMonitorServiceStatusList") = mUpdateComplyHistoryCompMonitorServiceStatusList


        '''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)

        ''Added By Utkarsh On 28-Jul-2011 For All19072011
        'MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnValueFormatted
        'MarkLog(Util.Action.View, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, EventLogID)
        ''End
        '' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorServiceStatusList.aspx?GChildPage2=Index.aspx');", True)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompServiceHistoryWindow", "OpenCompServiceHistoryWindow();", True)
        ''End If

        'SV
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue))
        Dim mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).CompStatusID, mMachine.HourType)
        'If mPrevCompMonitorServiceStatus.IsMaster Then
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyCompMonitorServiceStatusListShowValues_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        'mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, calDate.Value.ToString, mMachine.HourType)
        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)

        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        Session("EnFrom") = 1 'EditRecord
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMonitorServiceStatusListNew(ID).AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(mCompMonitorServiceStatusListNew(ID).CompStatusID, mCompMonitorServiceStatusListNew(ID).AssemblyStatusID, mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        'RemoveSession()
        'Added by Saylee on 5-Aug-2009
        mCompInfo = cmbAircraftList.SelectedItem.ToString + "->" + mAssemblyStatus.Assembly.ModelName + vbCrLf + mAssemblyStatus.Assembly.SerialNo + vbCrLf + "->" + "[Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & " ]" + "->" + mCompMonitorServiceStatusListNew(ID).Reference + "->" + mCompMonitorServiceStatusListNew(ID).Type + "->" + mCompMonitorServiceStatusListNew(ID).ATACode.ToString + "->" + mCompMonitorServiceStatusListNew(ID).Description
        Session("mCompInfo") = mCompInfo
        ''*****************************************

        Session("ATA") = mCompMonitorServiceStatusListNew(ID).ATACode.ToString
        Session("Description") = mCompMonitorServiceStatusListNew(ID).Description
        Session("PartSerialNo") = "[Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & " ]"

        mUpdateComplyHistoryCompMonitorServiceStatusList = UpdateComplyHistoryCompMonitorServiceStatusList.GetComplyHistoryCompMonitorServiceStatusList(mCompStatus.CompID, mCompMonitorServiceStatus.PartMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorServiceStatusList") = mUpdateComplyHistoryCompMonitorServiceStatusList


        ''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)

        Dim DoneOnValue As String
        For i As Integer = 0 To mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            If i = 0 Then
                DoneOnValue = mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DoneOnValueFormatted
            Else
                DoneOnValue = DoneOnValue + " " + mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DoneOnValueFormatted
            End If
        Next

        'Added By Utkarsh On 28-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + cmbAircraftList.SelectedItem.ToString & " Assembly Info : " & mAssemblyStatus.Assembly.ModelName + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " & mCompStatus.Description & " " & mCompStatus.SerialNo & " " & mCompStatus.Position & " Monitor Info : " & mCompMonitorServiceStatusListNew(ID).Type & " Done On Date : " & mCompMonitorServiceStatusListNew(ID).DoneOnFormatted.ToString & " Done On Value : " & DoneOnValue
        MarkLog(Util.Action.View, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, ID, EventLogID)
        'End
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorServiceStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompServiceHistoryWindow", "OpenCompServiceHistoryWindow();", True)
        'End If
        'End
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        'If chkApplicable.Checked And mTmpComplyCompMonitorServiceStatusList(Index).PartActivityCount > 1 Then 'Revise Activity
        '    MSGBoxCtrl.show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
        'Else
        '    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        'End If
        'mTmpComplyCompMonitorServiceStatusList.CurrentIndex = Index
        'Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList

        'SV
        If chkApplicable.Checked And mCompMonitorServiceStatusListNew(ID).PartActivityCount > 1 Then 'Revise Activity
            MSGBoxCtrl.show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        End If
        mCompMonitorServiceStatusListNew.CurrentIndex = mCompMonitorServiceStatusListNew(ID, "")
        Session("mCompMonitorServiceStatusListNew") = mCompMonitorServiceStatusListNew
        'End
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
                            'Added By Utkarsh On 27-Jul-2011 For All19072011
                            IDForEventLog = mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).ID
                            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).AssemblyStatusID)
                            Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).CompStatusID, mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).AssemblyStatusID, mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).DoneOnFormatted.ToString)
                            MaintDetail = "Reg No. : " + cmbAircraftList.SelectedItem.ToString & " Assembly Info : " & mAssemblyStatus.Assembly.ModelName + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " & mCompStatus.Description & " " & mCompStatus.SerialNo & " " & mCompStatus.Position & " Monitor Info : " & mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).Type & " Done On Date : " & mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).DoneOnFormatted.ToString
                            'End
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).ID, 8)
                            '=============================
                            If mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).ID)
                            End If
                            CompMonitorServiceStatus.DeleteCompMonitorServiceStatus(mCompMonitorServiceStatusListNew(mCompMonitorServiceStatusListNew.CurrentIndex).ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            Session("mMachineMaintenance") = mMachineMaintenance
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
                                MarkLog(Util.Action.Delete, "ComponentServiceMonitor", "Can't delete : " & MaintDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'mLog.ID)'Added By Utkarsh On 27-Jul-2011 For All19072011
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID) 'Added By Utkarsh On 27-Jul-2011 For All19072011
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
            '   DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        'If RecordsToShow < mTmpComplyCompMonitorServiceStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    lblResult.Text = "List of Component Service Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyCompMonitorServiceStatusList.Count & " Record(s) shown."
        'Else
        '    lblResult.Text = "List of Component Service Status as per selected criteria : " & mTmpComplyCompMonitorServiceStatusList.Count & " Record(s) found."
        'End If
        'SV
        Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                     Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                   Select StatusInfo).ToList
        If RecordsToShow < List.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
            lblResult.Text = "List of Component Service Status as per selected criteria : " & RecordsToShow.ToString & " of " & List.Count.ToString & " Record(s) shown."
        Else
            lblResult.Text = "List of Component Service Status as per selected criteria : " & List.Count.ToString & " Record(s) found."
        End If
        'End
    End Sub
    'Added By Prashant 31-Mar-2011
    Private Sub SetRights()
        If (User.IsInRole("MachineComponentServiceNew")) = False Then
            btnAddNewTop.Enabled = False
            btnAddNewTop.ToolTip = "You are not authorized user"
            btnAddNew.Enabled = False
            btnAddNew.ToolTip = "You are not authorized user"
        End If
    End Sub
    '-----------------------------
    Private Sub SetGrid()
        Dim B As Boolean
        Dim c As Boolean

        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft

        For j As Integer = 0 To dgDueMonitoringList.Rows.Count - 1
            B = CType(Me.dgDueMonitoringList.Rows(j).Cells(24).Text, Boolean)
            c = CType(Me.dgDueMonitoringList.Rows(j).Cells(26).Text, Boolean)
            If B = True Then
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = False
            End If
            If c = False Then
                dgDueMonitoringList.Rows(j).Cells(25).Enabled = False
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
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        'End If
        'SV
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            'Dim List = (From StatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo In mtmpComplyCompMonitorServiceStatusList
            '                                           Select StatusInfo).ToList.Take(RecordsToShow)
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            'dgDueMonitoringList.DataSource = mtmpComplyCompMonitorServiceStatusList
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                      Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        'End
        dgDueMonitoringList.DataBind()
        SetGrid()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Bind machine Combo
        Dim MachineId As String, AssemId As Guid
        If Not IsDate(DoneOn) Then
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DoneOn = Today.Date.ToString(AppSettings("DateFormat")) 'Added By Rahul on 29-Apr-2009
        Else
            txtDate.Text = CDate(DoneOn).ToString(AppSettings("DateFormat"))
        End If


        'Commented on May,28,2007 By Girish
        'calDate.TitleText = calDate.Text
        'calDate.DateToday = CDate(calDate.Text)
        'calDate.SelectedDate = CDate(calDate.Text)
        Session("DoneOn") = txtDate.Text

        'mMachineNameValueList = tmpMachineList.GetMachineList(, , , , , "<SELECT>")

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , False, , , True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraftList.DataSource = mMachineNameValueList

        'bind Assembly Combo
        If mMachineNameValueList.Count > 0 And (IsNothing(AircraftId)) Then
            MachineId = mMachineNameValueList(0).ID.ToString
            AssemblyId = Guid.Empty.ToString
        Else
            MachineId = AircraftId
        End If

        IsReadOnly = mMachineNameValueList(New Guid(MachineId)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly

        mAssemblylist = AssemblyList.GetAssemblyList(0, MachineId, txtDate.Text, "(ALL)")
        ''mAssemblylist = mAssemblylist.GetAssemblyList(0, New Guid(cmbAircraft.SelectedValue.ToString).ToString, Trim(calDate.Text), "<ALL>")
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        'Binding Grid
        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then AssemId = mAssemblylist(0).ID Else AssemId = New Guid(AssemblyId)
        'added By Deven
        AssemblyId = AssemId.ToString

        If PartNo Is Nothing Then PartNo = ""
        If SerialNo Is Nothing Then SerialNo = ""
        If MonitorTypeID Is Nothing Then MonitorTypeID = "0"

        txtCodeFormNo.Text = CodeFormNoDesc

        'Commented And Added By Rahul on 29-Apr-2009
        'mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(calDate.Value.ToString, MachineId, Trim(txtPart.Text), Trim(txtSerialNo.Text), AssemId)
        'mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(DoneOn, MachineId, PartNo, SerialNo, AssemId, , , , CType(MonitorTypeID, Integer), , , , ShowNotApplicable, IIf(ShowOneTimeMasterRecords = True, False, True), SortBy:="MinimumRemainingValue")
        mCompMonitorServiceStatusListNew = CompMonitorServiceStatusList.GetCompMonitorServiceStatusList(MachineID:=MachineId, CurrentDate:=DoneOn, SerialNo:=SerialNo, IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True), CompID:=Guid.Empty, CompStatusPeriodList:=Nothing, PartName:=PartNo, AssemblyID:=AssemId.ToString, MonitorTypeID:=CInt(Val(MonitorTypeID)), IsApplicable:=Not (ShowNotApplicable), IsRecordsDirectFetch:=True, IsMaster:=False, IsComplied:=True, IsServicePeriodsRequired:=False, CodeFormNoDesc:=CodeFormNoDesc) 'SV
        'Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        'End If
        'Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList

        'SV
        chkApplicable.Checked = IIf(ShowNotApplicable, True, False)
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        Session("mCompMonitorServiceStatusListNew") = mCompMonitorServiceStatusListNew
        'End
        'Added by Saylee on 30-July-2009
        mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList("(ALL)")
        cmbMonitorType.DataSource = mPartMonitorServiceTypeList

        DataBind()
        If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then cmbAircraftList.SelectedIndex = 0 Else cmbAircraftList.SelectedValue = AircraftId
        'Changed By Yogita on 9-Jan-2008 cmbAssemblyList.SelectedIndex = 1
        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then cmbAssembly.SelectedIndex = 0 Else cmbAssembly.SelectedValue = AssemblyId
        Session("MachineId") = cmbAircraftList.SelectedValue
        Session("AssemblyId") = cmbAssembly.SelectedValue
        'Added By Rahul on 29-Apr-2009
        txtPart.Text = PartNo
        txtSerialNo.Text = SerialNo
        '===========================
        chkApplicable.Checked = ShowNotApplicable 'Added by Saylee on 7-Jan-2011
        chkOneTimeMasterRecords.Checked = ShowOneTimeMasterRecords

        If IsNothing(MonitorTypeID) Or MonitorTypeID = "" Then cmbMonitorType.SelectedIndex = 0 Else cmbMonitorType.SelectedValue = MonitorTypeID 'Added by Saylee on 30-July-2009
        Session("MonitorTypeID") = MonitorTypeID 'Added by Saylee on 30-July-2009

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 28-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfComplyCompMonitorServiceStatusListShowValues_Ajax.aspx?"
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            DataFieldBind()
            ControlVisibility()
            SetPage()
            SetRights() 'Added By Prashant 31-Mar-2011
            SetGrid()
            cmbAircraftList.Focus()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session.Remove("From")
        Session.Remove("DoneOn")
        Session.Remove("AircraftId")
        Session.Remove("AssemblyId")
        Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
        Session("MiddleFrame") = ""
        Session.Remove("ATA")
        Session.Remove("CodeFormNoDesc")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            Session("DoneOn") = txtDate.Text
            Session("AircraftId") = cmbAircraftList.SelectedValue
            Session("AssemblyId") = cmbAssembly.SelectedValue
            'Added By Rahul on 29-Apr-2009
            Session("PartNo") = Trim(txtPart.Text)
            Session("SerialNo") = Trim(txtSerialNo.Text)
            '==================================
            Session("ShowNotApplicable") = chkApplicable.Checked  'Added by Saylee on 7-Jan-2011
            Session("ShowOneTimeMasterRecords") = chkOneTimeMasterRecords.Checked

            dgDueMonitoringList.PageIndex = 0
            'mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , , chkApplicable.Checked, SortBy:="MinimumRemainingValue")
            mCompMonitorServiceStatusListNew = CompMonitorServiceStatusList.GetCompMonitorServiceStatusList(MachineID:=cmbAircraftList.SelectedValue, CurrentDate:=txtDate.Text, SerialNo:=Trim(txtSerialNo.Text), IsForDueReport:=IIf(chkOneTimeMasterRecords.Checked, False, True), CompID:=Guid.Empty, CompStatusPeriodList:=Nothing, PartName:=Trim(txtPart.Text), AssemblyID:=cmbAssembly.SelectedValue, MonitorTypeID:=CInt(Val(MonitorTypeID)), IsApplicable:=Not (ShowNotApplicable), IsRecordsDirectFetch:=True, IsMaster:=False, IsComplied:=True, IsServicePeriodsRequired:=False, CodeFormNoDesc:=Trim(txtCodeFormNo.Text)) 'SV
            'Vikrant
            'If AppSettings("IsShowAllRecordsVisible") = "True" Then
            '    Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
            '                                               Select StatusInfo).ToList.Take(RecordsToShow)
            '    dgDueMonitoringList.DataSource = List
            'Else
            '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
            'End If
            'Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList

            'SV
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                            Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                          Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                           Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                         Select StatusInfo).ToList
                dgDueMonitoringList.DataSource = List
            End If
            Session("mCompMonitorServiceStatusListNew") = mCompMonitorServiceStatusListNew
            'End
            dgDueMonitoringList.DataBind()
            SetPage()
            ControlVisibility()
            SetGrid()
            Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
            '*************************************
            upnlgrid.Update()
            upnlActionBtn.Update()
            upnlActionBtnTop.Update()
        End If
    End Sub

    Private Sub dgDueMonitoringList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueMonitoringList.RowCommand
        Dim index As Int32
        Dim CompMonitorServiceStatusID, AssemblyID As Guid
        Select Case e.CommandName
            Case "Comply"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                'SV
                'GridBind()
                'SetGrid()
                'ControlVisibility()
                'End
                If (Not User.IsInRole("ComponentServiceMonitorNew")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ComplyRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "EditRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                'SV
                'GridBind()
                'SetGrid()
                'ControlVisibility()
                'End
                If (Not User.IsInRole("ComponentServiceMonitorView") And Not User.IsInRole("ComponentServiceMonitorEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "DeleteRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                'SV
                'GridBind()
                'SetGrid()
                'ControlVisibility()
                'End
                If (Not User.IsInRole("ComponentServiceMonitorDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "History"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                'SV
                'GridBind()
                'SetGrid()
                'ControlVisibility()
                'End
                If (Not User.IsInRole("ComponentServiceMonitorView") And Not User.IsInRole("ComponentServiceMonitorEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                HistoryRecords(New Guid(dgDueMonitoringList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
            Case "ViewRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                'SV
                'GridBind()
                'SetGrid()
                'ControlVisibility()
                'End
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
            Case "ShowVal" 'SV
                'GridBind()
                Dim CompMonitorServiceStatusIDs As New StringBuilder
                Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                CompMonitorServiceStatusIDs.Append("<CompMonServiceID>")
                CompMonitorServiceStatusIDs.Append("<id>")
                CompMonitorServiceStatusIDs.Append(New Guid(currentRow.Cells(0).Text))
                CompMonitorServiceStatusIDs.Append("</id>")
                CompMonitorServiceStatusIDs.Append("</CompMonServiceID>")

                'GridBind()
                'SetGrid()
                'ControlVisibility()
                CompMonitorServiceStatusID = New Guid(currentRow.Cells(0).Text)
                PartNo = currentRow.Cells(1).Text
                SerialNo = IIf(Trim(currentRow.Cells(2).Text) = "&nbsp;", "", Trim(currentRow.Cells(2).Text))
                AssemblyID = New Guid(currentRow.Cells(3).Text)

                Dim mtmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
                mtmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList([Date]:=Today.Date.ToString, AssemblyID:=AssemblyID, MachineID:=cmbAircraftList.SelectedValue.ToString, CompMonitorServiceStatusIDs:=CompMonitorServiceStatusIDs.ToString, Part:=PartNo, SerialNo:=SerialNo, ShowNotApplicable:=IIf(chkApplicable.Checked, True, False), SkipOneTimeDoneMasterRecords:=IIf(chkOneTimeMasterRecords.Checked = True, False, True), ShowAllRecords:=IIf(chkApplicable.Checked, True, False))

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
    'Private Sub dgDueMonitoringList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDueMonitoringList.PageIndexChanging
    '    dgDueMonitoringList.PageIndex = e.NewPageIndex
    '    'mStockItemList = StockItemList.GetStockItemList("", "")
    '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
    '    Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
    '    dgDueMonitoringList.DataBind()
    '    SetGrid()
    'End Sub
    Private Sub cmbAircraftList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
        mAssemblylist = AssemblyList.GetAssemblyList(0, cmbAircraftList.SelectedValue.ToString, txtDate.Text, "(ALL)")
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        cmbAssembly.DataBind()
        'New Addition By Yogita on 9-Jan-2008 to solve bug No:-LCMSS4
        If cmbAircraftList.Enabled = True Then
            cmbAircraftList.Focus()
        End If


        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly


        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click, btnAddNew.Click
        If IsValid Then
            'Added By Utkarsh On 28-Jul-2011 For All19072011
            MarkLog(Util.Action.[New], "ComponentServiceMonitor", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'End
            Session("AircraftIdForService") = cmbAircraftList.SelectedValue.ToString
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorServiceStatusListNew.aspx?BackPage=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompServiceListNewWindow", "OpenCompServiceListNewWindow()", True)
            Session("NewPage") = "True"
        End If
    End Sub
    'New addition by Rupali on 23wfmachin-Jun-09 for Sorting Order
    Private Sub dgDueMonitoringList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueMonitoringList.Sorting
        'mTmpComplyCompMonitorServiceStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
        mCompMonitorServiceStatusListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending) 'SV
        'Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        'End If
        'Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList

        'SV
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
            dgDueMonitoringList.DataSource = List
        End If
        Session("mCompMonitorServiceStatusListNew") = mCompMonitorServiceStatusListNew
        'End

        dgDueMonitoringList.DataBind()
        SetGrid()
    End Sub
    Private Sub txtPart_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPart.TextChanged
        Part = txtPart.Text
    End Sub
    Private Sub txtSerialNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSerialNo.TextChanged
        SerialNo = txtSerialNo.Text
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Private Sub txtCodeFormNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtCodeFormNo.TextChanged
    '    FindNow()
    '    upnlgrid.Update()
    '    upnlActionBtn.Update()
    '    upnlActionBtnTop.Update()
    'End Sub
    Private Sub hdnBtnCompServiceHistory_Click(sender As Object, e As System.EventArgs) Handles hdnBtnCompServiceHistory.Click
        'mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , , chkApplicable.Checked, SortBy:="MinimumRemainingValue")
        ''Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        'End If
        'Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
        'dgDueMonitoringList.DataBind()
        'SetPage()
        'ControlVisibility()
        'SetGrid()
        'upnlgrid.Update()

        'SV
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
        'End
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub chkApplicable_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkApplicable.CheckedChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Protected Sub chkOneTimeMasterRecords_CheckedChanged(sender As Object, e As EventArgs) Handles chkOneTimeMasterRecords.CheckedChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub lnkShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAllRecords.Click, lnkShowAllRecordsTop.Click
        ''RecordsToShow = mTmpComplyCompMonitorServiceStatusList.Count
        ''Session("RecordsToShow") = RecordsToShow
        ''dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        'SV
        RecordsToShow = mCompMonitorServiceStatusListNew.Count
        Session("RecordsToShow") = RecordsToShow
        Dim List = (From StatusInfo As CompMonitorServiceStatusInfo In mCompMonitorServiceStatusListNew
                        Where StatusInfo.IsApplicable = Not (chkApplicable.Checked)
                                                       Select StatusInfo).ToList
        'End
        dgDueMonitoringList.DataSource = List
        dgDueMonitoringList.DataBind()
        SetPage()
        SetGrid()
        ControlVisibility()
        upnlActionBtn.Update()
    End Sub
#End Region

#Region " Report "
    'Created By:- Jyoti
#Region " Report Variable "
    'SV
    'Dim mCompanyDetail As New CompanyDetail
    'Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

    'Private SearchStr1 As String = ""
    'Private SearchStr2 As String = ""
    'Private SearchStr3 As String = ""
    'Private SearchStr4 As String = ""
    'Private Searchstr5 As String = ""

    'Dim Part As String = String.Empty
    'Dim SerialNo As String = String.
    'End
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        'SV
        '     If (Not User.IsInRole("ComponentServiceMonitorPrint")) Then
        '         'Commented By Utkarsh On 28-Jul-2011 For All19072011
        '         '   MarkLog(Util.Action.Print, "ComplyCompMonitorServiceStatus", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        '         'End
        '         MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
        '         Exit Sub
        '     End If
        '     dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        '     dgDueMonitoringList.DataBind()
        '     SetGrid()
        '     Rpt = New crListComplyCompMonitorStatus
        '     Dim da As New CSLA.Data.ObjectAdapter
        '     Dim ds As New dsCommon
        '     Dim ReportDetails As New rptStatusList

        '     SearchStr1 = "Date :" + "  " + txtDate.Text

        '     If Part = "" Then
        '         SearchStr2 = ""
        '     Else
        '         SearchStr2 = "Part :" + " " + Part
        '     End If
        '     If SerialNo = "" Then
        '         SearchStr3 = ""
        '     Else
        '         SearchStr3 = "Serial No. :" + " " + SerialNo
        '     End If

        '     SearchStr4 = "Aircraft :" + "  " + cmbAircraftList.SelectedItem.Text
        '     Searchstr5 = "Assembly :" + "  " + cmbAssembly.SelectedItem.Text
        '     'Changed By Yogita on 9-Jan-2008
        '     ReportDetails.Add(New rptStatus(, 0, "", _
        '           , , , dgDueMonitoringList.Columns.Item(1).HeaderText, , dgDueMonitoringList.Columns.Item(5).HeaderText, _
        '          dgDueMonitoringList.Columns.Item(6).HeaderText, dgDueMonitoringList.Columns.Item(8).HeaderText, _
        '          dgDueMonitoringList.Columns.Item(9).HeaderText, dgDueMonitoringList.Columns.Item(10).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(11).HeaderText, dgDueMonitoringList.Columns.Item(12).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(15).HeaderText, dgDueMonitoringList.Columns.Item(16).HeaderText, _
        '           dgDueMonitoringList.Columns.Item(17).HeaderText, dgDueMonitoringList.Columns.Item(18).HeaderText, , , _
        '           dgDueMonitoringList.Columns.Item(19).HeaderText, , , , , , , dgDueMonitoringList.Columns.Item(20).HeaderText))


        '     Dim TotalCount As Integer
        '     TotalCount = Me.mTmpComplyCompMonitorServiceStatusList.Count
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
        '         str(15) = ""

        '         If Me.dgDueMonitoringList.Rows(I).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgDueMonitoringList.Rows(I).Cells(1).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(5).Text.Replace("<BR>", vbCrLf)
        '         If Me.dgDueMonitoringList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf)
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
        '         If Me.dgDueMonitoringList.Rows(I).Cells(20).Text <> "&nbsp;" Then str(15) = Me.dgDueMonitoringList.Rows(I).Cells(20).Text.Replace("<BR>", vbCrLf)

        '         ReportDetails.Add(New rptStatus(, 1, , _
        '          , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8), _
        '          str(9), str(10), str(11), str(12), str(13), , , str(14), , , , , , , str(15)))
        '     Next

        '     mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        '     Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, "List of Comply Component Service Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, Searchstr5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))


        '     If mTmpComplyCompMonitorServiceStatusList.Count = 0 Then
        '         MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '         Exit Sub
        '     End If

        '     da.Fill(ds, ReportDetails)
        '     da.Fill(ds, Report)
        '     Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '     da.Fill(ds, mrptImage)
        '     Rpt.SetDataSource(ds)
        '     Session("CrystalReport") = Rpt

        '     'Commented By Utkarsh On 28-Jul-2011 For All19072011

        '     '      MarkLog(Util.Action.Print, "ComplyCompMonitorServiceStatus", "List of Comply Component Monitor Service Status Report", Util.ErrorType.NoError, Guid.Empty)

        '     'End

        '     ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region
#End Region






End Class