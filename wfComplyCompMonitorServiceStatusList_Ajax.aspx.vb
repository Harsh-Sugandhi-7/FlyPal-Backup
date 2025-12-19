'AJAX Conversion by vikrant on 23-Mar-2015
Imports System.Linq
Public Class wfComplyCompMonitorServiceStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    Public mTmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
    'Commented n Added by Shital on 23-Jun-2021
    Private mrptDueReport As rptDueReport

    Public mAssemblylist As AssemblyList
    Public DoneOn As String
    Public AircraftId As String
    Public AssemblyId As String
    Public mCompInfo As String   'Added Code  Jan,29,2007
    Public ComplyCompMonitorServiceInfo As String   'Added Code   Jan,29,2007
    Public mComplyMonitorDetailForMail As String 'Added by Sachin On 18-10-23

    'Public mInstallCompStatus As CompStatus  'Added Code
    Public mMachine As Machine
    Public PartNo As String = String.Empty
    Public AircraftName As String 'Added By Vikrant for faster processing
    Private mPartMonitorServiceTypeList As PartMonitorServiceTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String = String.Empty 'Added by Saylee on 30-July-2009

    Private mUpdateComplyHistoryCompMonitorServiceStatusList As UpdateComplyHistoryCompMonitorServiceStatusList

    'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance

    Dim ShowNotApplicable As Boolean = False
    Dim ShowOneTimeMasterRecords As Boolean = False

    Dim mMonitorInfo As String
    Dim mMonitorType As String
    Dim mMonitorDesc As String
    Dim mAircraft As String
    Dim mAssemblyDetails As String
    Dim mCompDetail As String

    Dim EventLogID As Guid 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim IDForEventLog As Guid
    'Added By Prashant On 27-Nov-2014
    Dim mFileAttach As FileAttach
    Dim RecordsToShow As Integer
    Dim IsReadOnly As Boolean 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Dim mModuleList As ModuleList 'Added by Sachin on 18-10-2023
    Dim CodeFormNoDesc As String
    Public mIsSpareComponent As Integer 'Added By Prashant On 17-Sep-2020 For ALL27072020
    Public RadioChecked As Integer
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mTmpComplyCompMonitorServiceStatusList = CType(Session("mTmpComplyCompMonitorServiceStatusList"), tmpComplyCompMonitorServiceStatusList)
        mrptDueReport = CType(Session("mrptDueReport"), rptDueReport) 'Commented n Added by Shital on 23-Jun-2021
        DoneOn = CType(Session("DoneOn"), String)
        AircraftId = CType(Session("AircraftId"), String)
        AssemblyId = CType(Session("AssemblyId"), String)
        '   mInstallCompStatus = CType(Session("InstallCompStatus"), CompStatus)
        AircraftName = CType(Session("AircraftName"), String) 'Added By Vikrant for faster processing
        'Added by Rahul on 29-Apr-2009
        PartNo = CType(Session("PartNo"), String)
        SerialNo = CType(Session("SerialNo"), String)
        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009
        mModuleList = Session("mModuleList") 'Added by Sachin on 18-10-2023
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        ShowNotApplicable = CType(Session("ShowNotApplicable"), Boolean) 'Added by Saylee on 7th-Jan-2011
        ShowOneTimeMasterRecords = CType(Session("ShowOneTimeMasterRecords"), Boolean)
        RecordsToShow = CType(Session("RecordsToShow"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        CodeFormNoDesc = Session("CodeFormNoDesc")
        mIsSpareComponent = CType(Session("mIsSpareComponent"), Integer) 'Added By Prashant On 17-Sep-2020 For ALL27072020
        RadioChecked = CType(Session("RadioChecked"), Integer)
    End Sub
    Private Sub SetSession()
        Session("mAssemblylist") = mAssemblylist
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
        Session("mrptDueReport") = mrptDueReport 'Commented n Added by Shital on 23-Jun-2021
        Session("DoneOn") = DoneOn
        Session("AircraftId") = AircraftId
        Session("AssemblyId") = AssemblyId
        Session("AircraftName") = AircraftName 'Added By Vikrant for faster processing
        '  Session("InstallCompStatus") = mInstallCompStatus
        'Added by Rahul on 29-Apr-2009
        Session("SerialNo") = SerialNo
        Session("PartNo") = PartNo

        Session("MonitorTypeID") = MonitorTypeID 'Added by Saylee on 30-July-2009

        Session("mMachineMaintenance") = mMachineMaintenance 'Added by Saylee on 9th-Oct-2009
        Session("ShowNotApplicable") = ShowNotApplicable 'Added by Saylee on 7th-Oct-2010
        Session("ShowOneTimeMasterRecords") = ShowOneTimeMasterRecords
        Session("RadioChecked") = RadioChecked
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblylist")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mTmpComplyCompMonitorServiceStatusList")
        Session.Remove("mrptDueReport") 'Commented n Added by Shital on 23-Jun-2021
        Session.Remove("RecordsToShow")
        ' Session.Remove("mInstallCompStatus")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyCompMonitorServiceStatusList_Ajax.aspx?SpareComponent=" & Session("mIsSpareComponent") Then
            Session.Remove("mAssemblylist")
            Session.Remove("mMachineNameValueList")
            Session.Remove("mTmpComplyCompMonitorServiceStatusList")
            Session.Remove("mrptDueReport") 'Commented n Added by Shital on 23-Jun-2021
            Session.Remove("DoneOn")
            Session.Remove("AircraftId")
            Session.Remove("AircraftName") 'Added By Vikrant for faster processing
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
            Session.Remove("mIsSpareComponent")  'Added By Saylee On 17-Sep-2020 For ALL27072020
            Session.Remove("RadioChecked")
        End If
    End Sub
    Private Sub EnableLinks()
        'Commented n Added by Shital on 23-Jun-2021
        'If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
        '    If RecordsToShow < mTmpComplyCompMonitorServiceStatusList.Count Then
        If mIsSpareComponent = 0 Then 'Added By Vikrant for faster processing


            If Not mrptDueReport Is Nothing Then
                If RecordsToShow < mrptDueReport.Count Then
                    lnkShowAllRecords.Enabled = True
                    lnkShowAllRecordsTop.Enabled = True
                Else
                    lnkShowAllRecords.Enabled = False
                    lnkShowAllRecordsTop.Enabled = False
                End If
            End If
        Else

            If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
                If RecordsToShow < mTmpComplyCompMonitorServiceStatusList.Count Then
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
        'btnPrint.Enabled = (mTmpComplyCompMonitorServiceStatusList.Count > 0)
        'btnPrintTop.Enabled = (mTmpComplyCompMonitorServiceStatusList.Count > 0)
        If mIsSpareComponent = 0 Then 'Added By Vikrant for faster processing
            btnPrint.Enabled = (mrptDueReport.Count > 0)
            btnPrintTop.Enabled = (mrptDueReport.Count > 0)
            'End
        Else 'existing flow for spare assembly keep as it is
            btnPrintTop.Enabled = (mTmpComplyCompMonitorServiceStatusList.Count > 0)
            btnPrint.Enabled = (mTmpComplyCompMonitorServiceStatusList.Count > 0)
        End If

        dgDueMonitoringList.Columns(22).Visible = IIf(chkApplicable.Checked, False, True)
        EnableLinks()

        'Added By Saylee On 27-Jul-2020 For ALL27072020
        btnAddNew.Visible = IIf(mIsSpareComponent = 0, True, False)
        btnAddNewTop.Visible = IIf(mIsSpareComponent = 0, True, False)
        phDateAircraft.Visible = IIf(mIsSpareComponent = 0, True, False)
        phSpareComp.Visible = IIf(mIsSpareComponent = 1, True, False)
        phAssembly.Visible = IIf(mIsSpareComponent = 0 Or rdbSpareAssemblyComponent.Checked, True, False)

        upnlSearchCriteria.Update()
        'End
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
        If rdbSpareComponent.Checked = True Then
            Session("RadioChecked") = 1
        ElseIf rdbRemovedComp.Checked Then
            Session("RadioChecked") = 2
        ElseIf rdbSpareAssemblyComponent.Checked Then
            Session("RadioChecked") = 3
        End If
        dgDueMonitoringList.PageIndex = 0

        If mIsSpareComponent = 0 Then 'Added By Vikrant for faster processing
            mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, , cmbAssembly.SelectedValue, 4,
                                                      CInt(IIf(cmbMonitorType.SelectedIndex > 0, cmbMonitorType.SelectedValue, 0)), chkApplicable.Checked,
                                                      chkOneTimeMasterRecords.Checked, CodeFormNoDesc:=Trim(txtCodeFormNo.Text), PartName:=Trim(txtPart.Text), CompSerialNo:=Trim(txtSerialNo.Text))
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

        Else  'existing flow for spare assembly keep as it is
            mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked = True, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareComponent:=mIsSpareComponent, ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))
            Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
            End If

        End If
        dgDueMonitoringList.DataBind()
        SetPage()
        ControlVisibility()
        SetGrid()
        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
    End Sub
    Private Sub ComplyRecord(ByVal Index As Int32)
        'frm.ComplyCompMonitorServiceInfo = mtmpComplyCompMonitorServiceStatusList(dgDueMonitoringList.CurrentRowIndex).PartMonitorServiceInfo
        '        ComplyCompMonitorServiceInfo = mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).PartMonitorServiceInfo
        '       Dim mCompInfo As String = "[Part: " & mInstallCompStatus.PartName & " Serial No.: " & mInstallCompStatus.SerialNo & " ]"
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        ''mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        'Added by Saylee on 5-Nov-2020 for ALL27072020
        Dim mHourType As Integer = 1
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus
        Dim mCompStatus As CompStatus
        If mIsSpareComponent = 0 Then 'Added By Vikrant for faster processing

            If mrptDueReport.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            '***********

            ' Dim mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)


            If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
            Else

                mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)

            End If

            mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mrptDueReport(Index).ID, mrptDueReport(Index).AssemblyStatusID, mrptDueReport(Index).CompStatusID, mHourType, , mCompStatus, mCompStatus.IsSpareComp)
        Else
            If mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            If mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)

            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)

            End If
            mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(Index).CompStatusID, mHourType, , mCompStatus, mCompStatus.IsSpareComp)

        End If

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



            If mIsSpareComponent = 0 Then 'Added By Vikrant for faster processing
                If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                    Session("mAssemblyStatus") = mAssemblyStatus
                    mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
                Else
                    mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
                End If
                'End
                mCompInfo = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter + "->" + mrptDueReport.Item(Index).Description
                Session("mCompInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter + "->" + mrptDueReport.Item(Index).Description

                MaintDetail = "Reg No. : " + mrptDueReport(Index).RegNo & " Assembly Info : " & mrptDueReport(Index).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(Index).TypeDet & " Done On Date : " & mrptDueReport(Index).DoneOnDate.ToString & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                MarkLog(Util.Action.Comply, "ComponentInspections", MaintDetail, Util.ErrorType.NoError, mrptDueReport(Index).ID, EventLogID)
            Else 'existing flow for spare assembly keep as it is
                If mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
                    Session("mAssemblyStatus") = mAssemblyStatus
                    mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
                Else
                    mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
                End If

                MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnValueFormatted
                MarkLog(Util.Action.Comply, "ComponentInspections", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorServiceStatusList.Item(Index).ID, EventLogID)
            End If

            Session("mMachine") = mMachine
            Session("mCompStatus") = mCompStatus
            '''' Session("mAssemblyStatus") = mAssemblyStatus
            'Rajnish 21-07-2008
            mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End

            RemoveSession()

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)

        End If
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus
        'Added by Saylee on 5-Nov-2020 for ALL27072020
        ' mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        Dim mHourType As Integer = 1

        Dim mCompStatus As CompStatus
        If mIsSpareComponent = 0 Then 'Added By Vikrant for faster processing
            If mrptDueReport.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            '***********
            If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)

            Else
                mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
            End If
            mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mrptDueReport(Index).ID, mrptDueReport(Index).AssemblyStatusID, mrptDueReport(Index).CompStatusID, mHourType, , mCompStatus, mCompStatus.IsSpareComp)

        Else
            If mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            If mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020

                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
            Else

                mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)

            End If
            mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(Index).CompStatusID, mHourType, , mCompStatus, mCompStatus.IsSpareComp)

        End If




        If mPrevCompMonitorServiceStatus.IsMaster And mPrevCompMonitorServiceStatus.IsApplicable And chkApplicable.Checked = False Then
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit the component.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mPrevCompMonitorServiceStatus.IsMaster) And (Not mPrevCompMonitorServiceStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records

            Session("mCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("EnFrom") = 1 'EditRecord
            Dim mAssemblyStatus As AssemblyStatus
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
            If mIsSpareComponent = 0 Then
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                mCompInfo = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                Session("mCompInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description

            Else
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
                mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
                Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description

            End If


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



            ''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)

            'Commented And Added by Saylee on 3-Dec-2019 , as to open Master form for NOT Appilcable Records and not COMPLY form
            ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************

            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevCompMonitorServiceStatus.IsMaster = False) And (mPrevCompMonitorServiceStatus.IsCompleted = False) And mPrevCompMonitorServiceStatus.IsDone = False) Then
            Dim mPartMonitorService As PartMonitorService
            If mIsSpareComponent = 0 Then
                mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).CompStatusID, mHourType, True, , mCompStatus.IsSpareComp)
                mPartMonitorService = PartMonitorService.GetPartMonitorService(mrptDueReport.Item(Index).StatusMasterID, mHourType)
            Else
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
                mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mHourType, True, , mCompStatus.IsSpareComp)
                mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
                mPartMonitorService = PartMonitorService.GetPartMonitorService(mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceID, mHourType)
            End If
            Session("mPartMonitorService") = mPartMonitorService

            Session("mMachine") = mMachine
            '  Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Session("mCompStatus") = mCompStatus
            Session("EnFrom") = 1
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorServiceStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************
        Else

            'mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, calDate.Value.ToString, mMachine.HourType)
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mHourType, True, IsForSpareComp:=True)

            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("EnFrom") = 1 'EditRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
            '''''''''' Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
            '''''''''''  mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)
            Session("mMachine") = mMachine
            '''''''''' Session("mAssemblyStatus") = mAssemblyStatus
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
            If mIsSpareComponent = 0 Then
                mCompInfo = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                Session("mCompInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description

            Else
                mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
                Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
                ''****************************************
            End If


            ''MarkLog(Util.Action.Edit, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
        End If
        'Commented n Added by Shital on 23-Jun-2021
        'Added By Utkarsh On 28-Jul-2011 For All19072011


        If mIsSpareComponent = 0 Then
            MaintDetail = "Reg No. : " + mrptDueReport(Index).RegNo & " Assembly Info : " & mrptDueReport(Index).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(Index).TypeDet & " Done On Date : " & mrptDueReport(Index).DoneOnDate.ToString & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
            MarkLog(Util.Action.Edit, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mrptDueReport(Index).ID, EventLogID)
        Else
            MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnValueFormatted
            MarkLog(Util.Action.Edit, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, EventLogID)

        End If
        'End
    End Sub
    Private Sub HistoryRecords(ByVal Index As Int32)
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus

        'Added by Saylee on 5-Nov-2020 for ALL27072020
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
        Dim mHourType As Integer = 1

        '  'If mPrevCompMonitorServiceStatus.IsMaster Then
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyCompMonitorServiceStatusList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        '
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus


        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus

        Session("EnFrom") = 1 'EditRecord

        Dim mCompStatus As CompStatus
        If mIsSpareComponent = 0 Then

            If mrptDueReport.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mrptDueReport(Index).ID, mrptDueReport(Index).AssemblyStatusID, mrptDueReport(Index).CompStatusID, mHourType, , , mrptDueReport.Item(Index).IsSpareComponent)
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mHourType, IsForSpareComp:=mrptDueReport.Item(Index).IsSpareComponent)

            If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)

            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)

            End If
            mCompInfo = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
            Session("mCompInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description

            Session("ATA") = mrptDueReport.Item(Index).ATAChapter.ToString
            Session("Description") = mrptDueReport.Item(Index).Description
            Session("PartSerialNo") = mrptDueReport.Item(Index).PartSerialNo
            MaintDetail = "Reg No. : " + mrptDueReport(Index).RegNo & " Assembly Info : " & mrptDueReport(Index).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(Index).TypeDet & " Done On Date : " & mrptDueReport(Index).DoneOnDate & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
            MarkLog(Util.Action.View, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mrptDueReport(Index).ID, EventLogID)
        Else
            If mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(Index).CompStatusID, mHourType, , , mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent)
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mHourType, IsForSpareComp:=mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent)


            If mTmpComplyCompMonitorServiceStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorServiceStatusList(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList.Item(Index).DoneOn.ToString)

            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorServiceStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)

            End If

            mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
            Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(Index).Description
            MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(Index).DoneOnValueFormatted
            MarkLog(Util.Action.View, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID, EventLogID)

        End If



        Session("mMachine") = mMachine
        '''''''''' Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        'RemoveSession()


        mUpdateComplyHistoryCompMonitorServiceStatusList = UpdateComplyHistoryCompMonitorServiceStatusList.GetComplyHistoryCompMonitorServiceStatusList(mCompStatus.CompID, mCompMonitorServiceStatus.PartMonitorServiceID, mHourType, TaskNo:=mCompMonitorServiceStatus.PartMonitorService.TaskCardNo)
        Session("mUpdateComplyHistoryCompMonitorServiceStatusList") = mUpdateComplyHistoryCompMonitorServiceStatusList

        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorServiceStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompServiceHistoryWindow", "OpenCompServiceHistoryWindow();", True)
        'End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        If mIsSpareComponent = 0 Then
            If chkApplicable.Checked And mrptDueReport(Index).ModelActivityCount > 1 Then 'Revise Activity
                MSGBoxCtrl.Show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            End If
            mrptDueReport.CurrentIndex = Index
            Session("mrptDueReport") = mrptDueReport
        Else
            If chkApplicable.Checked And mTmpComplyCompMonitorServiceStatusList(Index).PartActivityCount > 1 Then 'Revise Activity
                MSGBoxCtrl.Show("Delete Alert!", "You are trying to delete record which is already revised .", "Do you still want to continue?", MsgBoxStyle.YesNo, "Delete")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            End If
            mTmpComplyCompMonitorServiceStatusList.CurrentIndex = Index
            Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
        End If


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
                            If mIsSpareComponent = 0 Then
                                'Added on 25-Jun for new object faster processing
                                IDForEventLog = mrptDueReport(mrptDueReport.CurrentIndex).ID

                                mMonitorInfo = mrptDueReport.Item(mrptDueReport.CurrentIndex).TypeDet
                                mMonitorType = "" ' mrptDueReport.Item(mrptDueReport.CurrentIndex).MonitorType
                                mMonitorDesc = mrptDueReport.Item(mrptDueReport.CurrentIndex).Description
                                mAircraft = mrptDueReport.Item(mrptDueReport.CurrentIndex).RegNo
                                mAssemblyDetails = mrptDueReport.Item(mrptDueReport.CurrentIndex).Assembly 'mrptDueReport.Item(mrptDueReport.CurrentIndex).ModelName + "-" + mrptDueReport.Item(mrptDueReport.CurrentIndex).SerialNo '+ (IIf(mrptDueReport.Item(mrptDueReport.CurrentIndex).Position <> "", " (" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Position + ")", ""))
                                mCompDetail = mrptDueReport.Item(mrptDueReport.CurrentIndex).PartName + "-" + mrptDueReport.Item(mrptDueReport.CurrentIndex).CompSerialNo + (IIf(mrptDueReport.Item(mrptDueReport.CurrentIndex).Position <> "", " (" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Position + ")", ""))

                                MaintDetail = "Reg No. : " + mrptDueReport(mrptDueReport.CurrentIndex).RegNo & " Assembly Info : " & mrptDueReport(mrptDueReport.CurrentIndex).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(mrptDueReport.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(mrptDueReport.CurrentIndex).TypeDet
                                mComplyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> <b> Component Details : </b>" + mCompDetail + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mMonitorDesc
                                'Added on 25-Jun for new object faster processing
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mrptDueReport(mrptDueReport.CurrentIndex).ID, 8) 'Commented n Added by Shital on 23-Jun-2021
                                '=============================
                                If mrptDueReport(mrptDueReport.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(mrptDueReport.CurrentIndex).ID)
                                End If
                                CompMonitorServiceStatus.DeleteCompMonitorServiceStatus(mrptDueReport(mrptDueReport.CurrentIndex).ID)
                            Else
                                'Added By Utkarsh On 27-Jul-2011 For All19072011
                                IDForEventLog = mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID

                                mMonitorInfo = mTmpComplyCompMonitorServiceStatusList.Item(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).MonitorInfo
                                mMonitorType = mTmpComplyCompMonitorServiceStatusList.Item(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).MonitorType
                                mMonitorDesc = mTmpComplyCompMonitorServiceStatusList.Item(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).Description
                                mAircraft = mTmpComplyCompMonitorServiceStatusList.Item(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).MachineInfo
                                mAssemblyDetails = mTmpComplyCompMonitorServiceStatusList.Item(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).AssemblyInfo
                                mCompDetail = mTmpComplyCompMonitorServiceStatusList.Item(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).CompInfo

                                MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                                mComplyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> <b> Component Details : </b>" + mCompDetail + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mMonitorDesc

                                'Added by Saylee on 9th-Oct-2009
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID, 8)
                                If mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID)
                                End If
                                CompMonitorServiceStatus.DeleteCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(mTmpComplyCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID)

                            End If

                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            Session("mMachineMaintenance") = mMachineMaintenance
                            SendMail(mComplyMonitorDetailForMail)
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
                                MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
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
        If mIsSpareComponent = 0 Then

            Dim ServiceMPDTitle As String = ""

            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                lbltitle.Text = "List of Maintenance Event"
                ServiceMPDTitle = "Maintenance Event(s)"
                lblMonitorType.InnerText = "Task Type"
            Else
                lbltitle.Text = "List of Component Service Status"
                ServiceMPDTitle = "Component Service Status"
                lblMonitorType.InnerText = "Service Type"
            End If

            'Added by Shital on 23-Jun-2021 for faster processing
            If RecordsToShow < mrptDueReport.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of " + ServiceMPDTitle + " Status as per selected criteria : " & RecordsToShow.ToString & " of " & mrptDueReport.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of " + ServiceMPDTitle + " Status as per selected criteria : " & mrptDueReport.Count & " Record(s) found."
            End If
        Else
            If RecordsToShow < mTmpComplyCompMonitorServiceStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Component Service Status as per selected criteria : " & RecordsToShow.ToString & " of " & mTmpComplyCompMonitorServiceStatusList.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Component Service Status as per selected criteria : " & mTmpComplyCompMonitorServiceStatusList.Count & " Record(s) found."
            End If
        End If

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

        If mIsSpareComponent = 1 Then
            IsReadOnly = False
        End If

        For j As Integer = 0 To dgDueMonitoringList.Rows.Count - 1
            B = CType(Me.dgDueMonitoringList.Rows(j).Cells(27).Text, Boolean)
            c = CType(Me.dgDueMonitoringList.Rows(j).Cells(29).Text, Boolean)
            If B = True Then
                dgDueMonitoringList.Rows(j).Cells(26).Enabled = False
            End If
            If c = False Then
                dgDueMonitoringList.Rows(j).Cells(28).Enabled = False
            End If

            'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(24).Enabled = False
                dgDueMonitoringList.Rows(j).Cells(25).Enabled = False
                btnAddNewTop.Enabled = False
                btnAddNew.Enabled = False
                lblReadOnly.Visible = True
            Else
                dgDueMonitoringList.Rows(j).Cells(23).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(24).Enabled = True
                dgDueMonitoringList.Rows(j).Cells(25).Enabled = True
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

        If mIsSpareComponent = 0 Then

            If AppSettings("IsShowAllRecordsVisible") = "True" Then

                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
        Else
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)

                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList

            End If
        End If

        dgDueMonitoringList.DataBind()
        SetGrid()
    End Sub
    Public Sub SendMail(mComplyMonitorDetailForMail)
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'If AppSettings("MailsRequire") = "True" Then
        If mModuleList.Item("ComponentServiceMonitor").MailsRequire = True Then
            If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
                'Do nothing
                Exit Sub
            End If
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Task Details :  <br/> <br/>  " & mComplyMonitorDetailForMail & " <br/> <b> Deleted by User:</b> " + User.Identity.Name + "<b> on: </b>" + New SmartDate(Today.Date).FormattedText + "</font></P> ")
            str = str + ("</body></html>")
            'SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Assembly Inspections Deleted", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), Info:=str, ToMailID:=mModuleList.Item("Order").SendToMailID, Remark:=Session("SendMailRemark"), ReportGenratedBy:=Session("ReportGenratedBy"))

            SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Task Deleted", Info:=str, ToMailID:=mModuleList.Item("ComponentServiceMonitor").SendToMailID, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Bind machine Combo
        Dim MachineId As String, AssemId As Guid
        Dim MachineName As String 'Added By Vikrant for faster processing
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
            MachineName = mMachineNameValueList(0).RegNo 'Added By Vikrant for faster processing
            AircraftName = mMachineNameValueList(0).RegNo 'Added By Vikrant for faster processing

        Else
            MachineId = AircraftId
            ' MachineName = ""  'Added By Vikrant for faster processing"
            If MachineName Is Nothing Then MachineName = mMachineNameValueList(0).RegNo
            If AircraftName Is Nothing Then AircraftName = mMachineNameValueList(0).RegNo
        End If

        IsReadOnly = mMachineNameValueList(New Guid(MachineId)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly

        mAssemblylist = AssemblyList.GetAssemblyList(0, MachineId, txtDate.Text, "(ALL)", IsForSpareAssembly:=rdbSpareAssemblyComponent.Checked)
        ' mAssemblylist = AssemblyList.GetAssemblyList(0, MachineId, txtDate.Text, "(ALL)")
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

        If RadioChecked = 1 Then
            rdbSpareComponent.Checked = True
            rdbRemovedComp.Checked = False
            rdbSpareAssemblyComponent.Checked = False
        ElseIf RadioChecked = 2 Then
            rdbRemovedComp.Checked = True
            rdbSpareAssemblyComponent.Checked = False
            rdbSpareComponent.Checked = False
        ElseIf RadioChecked = 3 Then
            rdbSpareAssemblyComponent.Checked = True
            rdbSpareComponent.Checked = False
            rdbRemovedComp.Checked = False
        End If

        'Commented And Added By Rahul on 29-Apr-2009

        If mIsSpareComponent = 0 Then
            'Commented n Added by Shital on 23-Jun-2021
            mrptDueReport = rptDueReport.GetList(DoneOn, AircraftName, , True, , AssemId.ToString, 4, CType(MonitorTypeID, Integer), ShowNotApplicable, chkOneTimeMasterRecords.Checked,
                                           CodeFormNoDesc:=Trim(CodeFormNoDesc), PartName:=Trim(PartNo), CompSerialNo:=Trim(SerialNo))

			mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
			If AppSettings("IsShowAllRecordsVisible") = "True" Then

                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else

                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            Session("mrptDueReport") = mrptDueReport
        Else

            mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(DoneOn, MachineId, PartNo, SerialNo, AssemId, , , , CType(MonitorTypeID, Integer), , , , ShowNotApplicable, IIf(ShowOneTimeMasterRecords = True, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=CodeFormNoDesc, IsSpareComponent:=mIsSpareComponent, ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))

            If AppSettings("IsShowAllRecordsVisible") = "True" Then

                Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)

                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
            End If
            Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
        End If



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
        chkApplicable.Checked = IIf(ShowNotApplicable, True, False)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 28-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            'Added By Saylee On 27-Jul-2020 For ALL27072020
            If Session("mIsSpareComponent") Is Nothing Or (Session("mIsSpareComponent") <> Request.QueryString("SpareComponent")) Then
                mIsSpareComponent = Request.QueryString("SpareComponent")
            End If

            Session("mIsSpareComponent") = mIsSpareComponent
            'End
            Session("MiddleFrame") = "wfComplyCompMonitorServiceStatusList_Ajax.aspx?SpareComponent=" & Session("mIsSpareComponent") 'SpareAssembly Added By Saylee On 27-Jul-2020 For ALL27072020
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            DataFieldBind()
            ControlVisibility()
            SetPage()
            SetRights() 'Added By Prashant 31-Mar-2011
            SetGrid()
            cmbAircraftList.Focus()
        End If
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            lblCodeFormNo.InnerText = "Task No./Description"
            dgDueMonitoringList.HeaderRow.Cells(10).Text = "Description"
            dgDueMonitoringList.HeaderRow.Cells(7).Text = "Task Type"
            dgDueMonitoringList.Columns(10).HeaderText = "Description"
            dgDueMonitoringList.Columns(1).Visible = True
        Else
            dgDueMonitoringList.HeaderRow.Cells(10).Text = "Code/Form No./Description"
            dgDueMonitoringList.Columns(10).HeaderText = "Code/Form No./Description"
            dgDueMonitoringList.HeaderRow.Cells(7).Text = "Service Type"
            lblCodeFormNo.InnerText = "Code/Form No./Description"
            dgDueMonitoringList.Columns(1).Visible = False
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session.Remove("From")
        Session.Remove("DoneOn")
        Session.Remove("AircraftId")
        Session.Remove("AircraftName") 'Added By Vikrant for faster processing
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
            Session("AircraftName") = cmbAircraftList.SelectedItem.ToString  'Added By Vikrant for faster processing
            Session("AssemblyId") = cmbAssembly.SelectedValue
            'Added By Rahul on 29-Apr-2009
            Session("PartNo") = Trim(txtPart.Text)
            Session("SerialNo") = Trim(txtSerialNo.Text)
            '==================================
            Session("ShowNotApplicable") = chkApplicable.Checked  'Added by Saylee on 7-Jan-2011
            Session("ShowOneTimeMasterRecords") = chkOneTimeMasterRecords.Checked
            Session("CodeFormNoDesc") = Trim(txtCodeFormNo.Text)

            If rdbSpareComponent.Checked = True Then
                Session("RadioChecked") = 1
            ElseIf rdbRemovedComp.Checked Then
                Session("RadioChecked") = 2
            ElseIf rdbSpareAssemblyComponent.Checked Then
                Session("RadioChecked") = 3
            End If

            dgDueMonitoringList.PageIndex = 0


            If mIsSpareComponent = 0 Then
                mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, , cmbAssembly.SelectedValue, 4, CInt(cmbMonitorType.SelectedValue), chkApplicable.Checked, chkOneTimeMasterRecords.Checked, Trim(txtCodeFormNo.Text), Trim(txtPart.Text), Trim(txtSerialNo.Text))
                mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)

                If AppSettings("IsShowAllRecordsVisible") = "True" Then

                    Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                                Select StatusInfo).ToList.Take(RecordsToShow)
                    dgDueMonitoringList.DataSource = List
                Else

                    dgDueMonitoringList.DataSource = mrptDueReport
                End If
                Session("mrptDueReport") = mrptDueReport
            Else
                mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , , chkApplicable.Checked, SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareComponent:=mIsSpareComponent, ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))  'SpareAssembly Added By Saylee On 27-Jul-2020 For ALL27072020
                If AppSettings("IsShowAllRecordsVisible") = "True" Then
                    Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
                                Select StatusInfo).ToList.Take(RecordsToShow)

                    dgDueMonitoringList.DataSource = List
                Else

                    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList

                End If
                Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
            End If

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

        Select Case e.CommandName
            Case "Comply"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentServiceMonitorNew")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ComplyRecord(index)
            Case "EditRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentServiceMonitorView") And Not User.IsInRole("ComponentServiceMonitorEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecord(index)
            Case "DeleteRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentServiceMonitorDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(index)
            Case "History"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentServiceMonitorView") And Not User.IsInRole("ComponentServiceMonitorEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                HistoryRecords(index)
            Case "ViewRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                If mIsSpareComponent = 0 Then
                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(index).ID)  'Added by Shital on 23-Jun-2021
                Else
                    mFileAttach = FileAttach.GetAttachment(mTmpComplyCompMonitorServiceStatusList(index).ID)
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

        If mIsSpareComponent = 0 Then
            mrptDueReport.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending) 'Commented n Added by Shital on 23-Jun-2021

            If AppSettings("IsShowAllRecordsVisible") = "True" Then

                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else

                dgDueMonitoringList.DataSource = mrptDueReport 'Added by Shital on 23-Jun-2021
            End If
            Session("mrptDueReport") = mrptDueReport
        Else
            mTmpComplyCompMonitorServiceStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)


                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList

            End If
            Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
        End If

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
    Private Sub hdnBtnCompServiceHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnCompServiceHistory.Click



        If mIsSpareComponent = 0 Then
            'Added By shital for faster processing
            mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, , cmbAssembly.SelectedValue, 4, CInt(cmbMonitorType.SelectedValue), chkApplicable.Checked, chkOneTimeMasterRecords.Checked, Trim(txtCodeFormNo.Text), Trim(txtPart.Text), Trim(txtSerialNo.Text))
            mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mrptDueReport
            End If

        Else

            mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , , chkApplicable.Checked, SortBy:="MinimumRemainingValue", IsSpareComponent:=mIsSpareComponent, ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))
            'Vikrant
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorServiceStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
            End If
            Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList


        End If

        Session("mrptDueReport") = mrptDueReport
        dgDueMonitoringList.DataBind()
        SetPage()
        ControlVisibility()
        SetGrid()
        upnlgrid.Update()
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub chkApplicable_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkApplicable.CheckedChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Protected Sub chkOneTimeMasterRecords_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOneTimeMasterRecords.CheckedChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        FindNow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    'Private Sub txtCodeFormNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtCodeFormNo.TextChanged
    '    FindNow()
    '    upnlgrid.Update()
    '    upnlActionBtn.Update()
    '    upnlActionBtnTop.Update()
    'End Sub
    Private Sub lnkShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAllRecords.Click, lnkShowAllRecordsTop.Click
        If mIsSpareComponent = 0 Then
            RecordsToShow = mrptDueReport.Count
            Session("RecordsToShow") = RecordsToShow
            dgDueMonitoringList.DataSource = mrptDueReport
        Else
            RecordsToShow = mTmpComplyCompMonitorServiceStatusList.Count
            Session("RecordsToShow") = RecordsToShow

            dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        End If



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
    'Added by Saylee on 27-Jul-2020 fro 27-Jul-2020
    Private Sub rdbSpareAssemblyComponent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbSpareAssemblyComponent.CheckedChanged, rdbSpareComponent.CheckedChanged, rdbRemovedComp.CheckedChanged
        If rdbSpareAssemblyComponent.Checked Then
            phAssembly.Visible = True
            mAssemblylist = AssemblyList.GetAssemblyList(0, cmbAircraftList.SelectedValue.ToString, txtDate.Text, "(ALL)", IsForSpareAssembly:=rdbSpareAssemblyComponent.Checked)
            Session("mAssemblylist") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
            If (rdbSpareAssemblyComponent.Checked = True) Then
                Dim da As New CSLA.Data.ObjectAdapter
                Dim ds As New DataSet()
                da.Fill(ds, mAssemblylist)
                Dim dv As DataView = ds.Tables(0).DefaultView
                dv.RowFilter = "IsSpareAssembly='True'"
                For Each dr As DataRowView In dv
                    For Each item As ListItem In cmbAssembly.Items
                        If dr("ID").ToString() = item.Value.ToString() Then
                            item.Attributes.Add("style", "background-color:#ffbf00;color:black;font-weight:bold;")
                        End If
                    Next
                Next
            End If

            FindNow()
            upnlgrid.Update()
        ElseIf rdbSpareComponent.Checked Or rdbRemovedComp.Checked Then
            phAssembly.Visible = False
            FindNow()
            upnlgrid.Update()
        End If
        ControlVisibility()
    End Sub
#End Region

#Region " Report "
    'Created By:- Jyoti
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Private SearchStr4 As String = ""
    Private Searchstr5 As String = ""

    Dim Part As String = String.Empty
    Dim SerialNo As String = String.Empty
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If (Not User.IsInRole("ComponentServiceMonitorPrint")) Then
            'Commented By Utkarsh On 28-Jul-2011 For All19072011
            '   MarkLog(Util.Action.Print, "ComplyCompMonitorServiceStatus", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'End
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If mIsSpareComponent = 0 Then
            dgDueMonitoringList.DataSource = mrptDueReport 'Added by Shital on 23-Jun-2021
        Else
            dgDueMonitoringList.DataSource = mTmpComplyCompMonitorServiceStatusList
        End If


        dgDueMonitoringList.DataBind()
        SetGrid()
        Rpt = New crListComplyCompMonitorStatus
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "Date :" + "  " + txtDate.Text

        If Part = "" Then
            SearchStr2 = ""
        Else
            SearchStr2 = "Part :" + " " + Part
        End If
        If SerialNo = "" Then
            SearchStr3 = ""
        Else
            SearchStr3 = "Serial No. :" + " " + SerialNo
        End If

        SearchStr4 = "Aircraft :" + "  " + cmbAircraftList.SelectedItem.Text
        Searchstr5 = "Assembly :" + "  " + cmbAssembly.SelectedItem.Text
        'Changed By Yogita on 9-Jan-2008
        ReportDetails.Add(New rptStatus(, 0, "",
              , , , dgDueMonitoringList.Columns.Item(1).HeaderText, , dgDueMonitoringList.Columns.Item(5).HeaderText,
             dgDueMonitoringList.Columns.Item(6).HeaderText, dgDueMonitoringList.Columns.Item(8).HeaderText,
             dgDueMonitoringList.Columns.Item(9).HeaderText, dgDueMonitoringList.Columns.Item(10).HeaderText,
              dgDueMonitoringList.Columns.Item(11).HeaderText, dgDueMonitoringList.Columns.Item(12).HeaderText,
              dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText,
              dgDueMonitoringList.Columns.Item(15).HeaderText, dgDueMonitoringList.Columns.Item(16).HeaderText,
              dgDueMonitoringList.Columns.Item(17).HeaderText, dgDueMonitoringList.Columns.Item(18).HeaderText, , ,
              dgDueMonitoringList.Columns.Item(19).HeaderText, , , , , , , dgDueMonitoringList.Columns.Item(20).HeaderText))


        Dim TotalCount As Integer

        If mIsSpareComponent = 0 Then
            TotalCount = Me.mrptDueReport.Count 'Added by Shital on 23-Jun-2021
        Else
            TotalCount = Me.mTmpComplyCompMonitorServiceStatusList.Count
        End If
        '  

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
            str(15) = ""

            If Me.dgDueMonitoringList.Rows(I).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgDueMonitoringList.Rows(I).Cells(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(1) = Me.dgDueMonitoringList.Rows(I).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgDueMonitoringList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgDueMonitoringList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf)
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
            If Me.dgDueMonitoringList.Rows(I).Cells(20).Text <> "&nbsp;" Then str(15) = Me.dgDueMonitoringList.Rows(I).Cells(20).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 1, ,
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8),
             str(9), str(10), str(11), str(12), str(13), , , str(14), , , , , , , str(15)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Comply Component Service Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, Searchstr5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))


        '   If mTmpComplyCompMonitorServiceStatusList.Count = 0 Then
        If mrptDueReport.Count = 0 Then 'Commented n Added by Shital on 23-Jun-2021
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        'Commented By Utkarsh On 28-Jul-2011 For All19072011

        '      MarkLog(Util.Action.Print, "ComplyCompMonitorServiceStatus", "List of Comply Component Monitor Service Status Report", Util.ErrorType.NoError, Guid.Empty)

        'End

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region
#End Region






End Class