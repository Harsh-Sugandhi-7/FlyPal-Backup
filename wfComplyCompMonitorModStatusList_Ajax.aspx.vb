'AJAX Conversion by vikrant on 24-Mar-2015
Imports System.Linq
Imports System.Collections
Imports System.Collections.Generic
Public Class wfComplyCompMonitorModStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    Public mTmpComplyCompMonitorModStatusList As tmpComplyCompMonitorModStatusList
    'Commented n Added by Shital on 28-Jun-2021
    Private mrptDueReport As rptDueReport
    Public mAssemblylist As AssemblyList
    Public DoneOn As String
    Public AircraftId As String
    Public AssemblyId As String
    Public mCompInfo As String                   ' Code Added   29,Jan,2007 
    Public ComplyCompMonitorModInfo As String    ' Code Added   29,Jan,2007 
    Public mMachine As Machine
    Public PartNo As String = String.Empty
    Public mComplyMonitorDetailForMail As String 'Added by Sachin On 18-10-23

    Private mPartMonitorModTypeList As PartMonitorModTypeList  'Added by Saylee on 30-July-2009
    Private MonitorTypeID As String = String.Empty 'Added by Saylee on 30-July-2009
    Private mUpdateComplyHistoryCompMonitorModStatusList As UpdateComplyHistoryCompMonitorModStatusList 'Added by Saylee on 09-Sep-2009
    Dim mModuleList As ModuleList 'Added by Sachin on 18-10-2023
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
    Dim CodeFormNoDesc As String
    Public mIsSpareComponent As Integer 'Added By Vikrant On 17-Sep-2020 For ALL27072020
    Public RadioChecked As Integer
    Public AircraftName As String 'Added By Vikrant for faster processing
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mTmpComplyCompMonitorModStatusList = CType(Session("mTmpComplyCompMonitorModStatusList"), tmpComplyCompMonitorModStatusList)
        'Commented n Added by Shital on 28-Jun-2021
        mrptDueReport = CType(Session("mrptDueReport"), rptDueReport)
        DoneOn = CType(Session("DoneOn"), String)
        AircraftId = CType(Session("AircraftId"), String)
        AssemblyId = CType(Session("AssemblyId"), String)

        'Added by Rahul on 29-Apr-2009
        PartNo = CType(Session("PartNo"), String)
        SerialNo = CType(Session("SerialNo"), String)

        MonitorTypeID = Session("MonitorTypeID") 'Added by Saylee on 30-July-2009

        DirectiveNo = CType(Session("DirectiveNo"), String) 'Added by Saylee on 7-Aug-2009
        mModuleList = Session("mModuleList") 'Added by Sachin on 18-10-2023
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009

        ShowNotApplicable = CType(Session("ShowNotApplicable"), Boolean) 'Added by Saylee on 7th-Jan-2011
        ShowOneTimeMasterRecords = CType(Session("ShowOneTimeMasterRecords"), Boolean)
        RecordsToShow = CType(Session("RecordsToShow"), Integer)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        CodeFormNoDesc = Session("CodeFormNoDesc")
        mIsSpareComponent = CType(Session("mIsSpareComponent"), Integer) 'Added By Vikrant On 17-Sep-2020 For ALL27072020
        RadioChecked = CType(Session("RadioChecked"), Integer)
        AircraftName = CType(Session("AircraftName"), String) 'Added By Vikrant for faster processing
    End Sub
    Private Sub SetSession()
        Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
        'Commented n Added by Shital on 28-Jun-2021
        Session("mrptDueReport") = mrptDueReport
        Session("mAssemblylist") = mAssemblylist
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("DoneOn") = DoneOn
        Session("AircraftId") = AircraftId
        Session("AssemblyId") = AssemblyId
        'Added by Rahul on 29-Apr-2009
        Session("SerialNo") = SerialNo
        Session("PartNo") = PartNo

        Session("MonitorTypeID") = MonitorTypeID 'Added by Saylee on 30-July-2009
        Session("DirectiveNo") = DirectiveNo 'Added by Saylee on 7-Aug-2009

        Session("mMachineMaintenance") = mMachineMaintenance 'Added by Saylee on 9th-Oct-2009
        Session("ShowNotApplicable") = ShowNotApplicable 'Added by Saylee on 7th-Oct-2010
        Session("ShowOneTimeMasterRecords") = ShowOneTimeMasterRecords
        Session("RadioChecked") = RadioChecked
        Session("AircraftName") = AircraftName 'Added By Vikrant for faster processing

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblylist")
        Session.Remove("mMachineNameValueList")
        'Commented n Added by Shital on 28-Jun-2021
        Session.Remove("mTmpComplyCompMonitorModStatusList")
        Session.Remove("mrptDueReport")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 9th-Oct-2009
        Session.Remove("RecordsToShow")
        Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        'Session.Remove("mIsSpareComponent")  'Added By Vikrant On 17-Sep-2020 For ALL27072020
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComplyCompMonitorModStatusList_Ajax.aspx?SpareComponent=" & Session("mIsSpareComponent") Then
            'Commented n Added by Shital on 28-Jun-2021
            Session.Remove("mTmpComplyCompMonitorModStatusList")
            Session.Remove("mrptDueReport")
            Session.Remove("mAssemblylist")
            Session.Remove("mMachineNameValueList")
            Session.Remove("DoneOn")
            Session.Remove("AircraftId")
            Session.Remove("AssemblyId")
            'Added by Rahul on 29-Apr-2009
            Session.Remove("PartNo")
            Session.Remove("SerialNo")
            ''====================
            Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009

            Session.Remove("DirectiveNo") 'Added by Saylee on 7-Aug-2009

            Session.Remove("ShowNotApplicable") 'Added by Saylee on 7th-Oct-2010
            Session.Remove("ShowOneTimeMasterRecords")
            Session.Remove("RecordsToShow")
            Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            Session.Remove("CodeFormNoDesc")
            Session.Remove("RadioChecked")
            Session.Remove("AircraftName") 'Added By Vikrant for faster processing
        End If
    End Sub
    Private Sub Findnow()
        RecordsToShow = dgDueMonitoringList.PageSize
        Session("RecordsToShow") = RecordsToShow

        Session("DoneOn") = txtDate.Text
        Session("AircraftId") = cmbAircraftList.SelectedValue
        Session("AssemblyId") = cmbAssembly.SelectedValue
        'Added By Rahul on 29-Apr-2009
        Session("PartNo") = Trim(txtPart.Text)
        Session("SerialNo") = Trim(txtSerialNo.Text)
        Session("DirectiveNo") = Trim(txtDirectiveNo.Text)
        Session("ShowNotApplicable") = chkApplicable.Checked  'Added by Saylee on 7-Jan-2011
        Session("ShowOneTimeMasterRecords") = chkOneTimeMasterRecords.Checked
        Session("CodeFormNoDesc") = Trim(txtCodeFormNo.Text)
        Session("AircraftName") = cmbAircraftList.SelectedItem.ToString  'Added By Vikrant for faster processing

        If rdbSpareComponent.Checked = True Then
            Session("RadioChecked") = 1
        ElseIf rdbRemovedComp.Checked Then
            Session("RadioChecked") = 2
        ElseIf rdbSpareAssemblyComponent.Checked Then
            Session("RadioChecked") = 3
        End If

        If mIsSpareComponent = 0 Then
            mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, , cmbAssembly.SelectedValue, 6, _
                                          CInt(IIf(cmbMonitorType.SelectedIndex > 0, cmbMonitorType.SelectedValue, 0)), chkApplicable.Checked, _
                                          chkOneTimeMasterRecords.Checked, CodeFormNoDesc:=Trim(txtCodeFormNo.Text), PartName:=Trim(txtPart.Text), CompSerialNo:=Trim(txtSerialNo.Text), DirectiveNo:=Trim(txtDirectiveNo.Text))
            mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                'Commented n Added by Shital on 28-Jun-2021
                'Dim List = (From StatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo In mTmpComplyCompMonitorModStatusList
                '                                           Select StatusInfo).ToList.Take(RecordsToShow)
                Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                                                         Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                '  dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList
                dgDueMonitoringList.DataSource = mrptDueReport
            End If
            Session("mrptDueReport") = mrptDueReport
        Else
            mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , DirectiveNo, , chkApplicable.Checked, IIf(chkOneTimeMasterRecords.Checked = True, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareComponent:=CBool(mIsSpareComponent), ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))  'SpareComponent Added By Vikrant On 27-Jul-2020 For ALL27072020

            If AppSettings("IsShowAllRecordsVisible") = "True" Then

                Dim List = (From StatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo In mTmpComplyCompMonitorModStatusList
                                                            Select StatusInfo).ToList.Take(RecordsToShow)
               
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList

            End If
            Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
        End If
       
       

        dgDueMonitoringList.DataBind()
        SetPage()
        ControlVisibility()
        SetGrid()

        Session("MonitorTypeID") = cmbMonitorType.SelectedValue  'Added by Saylee on 30-July-2009
    End Sub
    Private Sub SetPage()
        If mIsSpareComponent = 0 Then
            If RecordsToShow < mrptDueReport.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Component Modification Status as per criteria : " & RecordsToShow.ToString & " of " & mrptDueReport.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Component Modification Status as per criteria : " & mrptDueReport.Count & " Record(s) found."
            End If
        Else
            If RecordsToShow < mTmpComplyCompMonitorModStatusList.Count And AppSettings("IsShowAllRecordsVisible") = "True" Then
                lblResult.Text = "List of Component Modification Status as per criteria : " & RecordsToShow.ToString & " of " & mTmpComplyCompMonitorModStatusList.Count & " Record(s) shown."
            Else
                lblResult.Text = "List of Component Modification Status as per criteria : " & mTmpComplyCompMonitorModStatusList.Count & " Record(s) found."
            End If
        End If
    End Sub

    Private Sub EnableLinks()
        If mIsSpareComponent = 0 Then
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
            If Not mTmpComplyCompMonitorModStatusList Is Nothing Then
                If RecordsToShow < mTmpComplyCompMonitorModStatusList.Count Then
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
        If mIsSpareComponent = 0 Then

            btnPrint.Enabled = (mrptDueReport.Count > 0)
            btnPrintTop.Enabled = (mrptDueReport.Count > 0)
        Else
            btnPrint.Enabled = (mTmpComplyCompMonitorModStatusList.Count > 0)
            btnPrintTop.Enabled = (mTmpComplyCompMonitorModStatusList.Count > 0)
        End If


        dgDueMonitoringList.Columns(23).Visible = IIf(chkApplicable.Checked, False, True)
        EnableLinks()
        'Added By Vikrant On 27-Jul-2020 For ALL27072020
        btnAddNew.Visible = IIf(mIsSpareComponent = 0, True, False)
        btnAddNewTop.Visible = IIf(mIsSpareComponent = 0, True, False)
        phDateAircraft.Visible = IIf(mIsSpareComponent = 0, True, False)
        phSpareComp.Visible = IIf(mIsSpareComponent = 1, True, False)
        phAssembly.Visible = IIf(mIsSpareComponent = 0 Or rdbSpareAssemblyComponent.Checked, True, False)
        dgDueMonitoringList.Columns(21).Visible = IIf(mIsSpareComponent = 1, True, False)
        upnlSearchCriteria.Update()
        'End
    End Sub
    Private Sub ComplyRecord(ByVal Index As Int32)
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)

        'Added by Saylee on 5-Nov-2020 for ALL27072020
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)

        Dim mCompMonitorModStatus As CompMonitorModStatus
        Dim mPrevCompMonitorModStatus As CompMonitorModStatus
        Dim mHourType As Integer = 1
        Dim tmpIsApplicable As Boolean = True
        Dim mCompStatus As CompStatus
        Dim mAssemblyStatus As AssemblyStatus


        If mIsSpareComponent = 0 Then
            If mrptDueReport.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)

                mHourType = mMachine.HourType
            End If
            '***********
            tmpIsApplicable = mrptDueReport.Item(Index).IsApplicable

            If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                 mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                 mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
            Else
                 mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            End If
            mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mrptDueReport(Index).ID, mrptDueReport(Index).AssemblyStatusID, mrptDueReport(Index).CompStatusID, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)

        Else
            If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
                mHourType = mMachine.HourType
            End If
           tmpIsApplicable = mTmpComplyCompMonitorModStatusList.Item(Index).IsApplicable

            If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)
            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            End If
            mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mTmpComplyCompMonitorModStatusList(Index).CompMonitorModStatusID, mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList(Index).CompStatusID, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)

        End If


        If (mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And (mPrevCompMonitorModStatus.IsCompleted Or mPrevCompMonitorModStatus.FetchRecordCount(mPrevCompMonitorModStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Commented n Added by Shital on 28-Jun-2021
            ' ElseIf mTmpComplyCompMonitorModStatusList.Item(Index).IsApplicable = False Then
        ElseIf tmpIsApplicable = False Then
            REM: If IsApplicable of Part Monitor Mod is not checked then it can not be complied
            MSGBoxCtrl.show(MSGBox.Message_title.MonitoringNotApplicable, MSGBox.Message_text.MonitoringNotApplicable, "Monitoring modification is not applicable, can not be complied.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, txtDate.Text, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
            Session("EnFrom") = 0 'ComplyRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
            ''''''' Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
            '''''Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)



            Session("mMachine") = mMachine
            Session("mCompStatus") = mCompStatus
            Session("mAssemblyStatus") = mAssemblyStatus
            'Rajnish 21-07-2008
            mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
            Session("mCompMonitorModStatus") = mCompMonitorModStatus

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End

            RemoveSession()

            If mIsSpareComponent = 0 Then

                mCompInfo = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                Session("mCompInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                'Added By Utkarsh On 28-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mrptDueReport(Index).RegNo & " Assembly Info : " & mrptDueReport(Index).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(Index).TypeDet & " Done On Date : " & mrptDueReport(Index).DoneOnDate.ToString & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
                MarkLog(Util.Action.Comply, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, mrptDueReport(Index).ID, EventLogID)
                'End
            Else
                mCompInfo = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description
                Session("mCompInfo") = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description
                'Added By Utkarsh On 28-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mTmpComplyCompMonitorModStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorModStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorModStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorModStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorModStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorModStatusList(Index).DoneOnValueFormatted
                MarkLog(Util.Action.Comply, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorModStatusList(Index).CompMonitorModStatusID, EventLogID)
                'End
            End If




            ''MarkLog(Util.Action.[New], "ComplyCompMonitorModStatus", mCompInfo + "   " + ComplyCompMonitorModInfo, Util.ErrorType.NoError, Guid.Empty)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorModStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
        End If
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)

        'Added by Saylee on 5-Nov-2020 for ALL27072020
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
        Dim mHourType As Integer = 1
        Dim mCompMonitorModStatus As CompMonitorModStatus
        Dim mPrevCompMonitorModStatus As CompMonitorModStatus
        Dim mCompStatus As CompStatus
        Dim mAssemblyStatus As AssemblyStatus

        If mIsSpareComponent = 0 Then
            If mrptDueReport.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
                mHourType = mMachine.HourType
            End If
             If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            End If
            mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mrptDueReport(Index).ID, mrptDueReport(Index).AssemblyStatusID, mrptDueReport(Index).CompStatusID, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)

        Else
            If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)
            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            End If
            mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mTmpComplyCompMonitorModStatusList(Index).CompMonitorModStatusID, mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList(Index).CompStatusID, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)

        End If
        

       If mPrevCompMonitorModStatus.IsMaster And mPrevCompMonitorModStatus.IsApplicable And chkApplicable.Checked = False Then
            'MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit component.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mPrevCompMonitorModStatus.IsMaster) And (Not mPrevCompMonitorModStatus.IsApplicable) And (chkApplicable.Checked = True) Then 'Editing NOT APPLICABLE Master records
            Session("mCompMonitorModStatus") = mPrevCompMonitorModStatus
            Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
            Session("EnFrom") = 1 'EditRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
            '' Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
            ''''Dim mCompStatus As CompStatus
            '''''' mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)

           
            'Commented n Added by Shital on 28-Jun-2021
           
           
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompStatus") = mCompStatus

            'Added By Vikrant On 25-Nov-2014
            If mPrevCompMonitorModStatus.IsAttachmentAdded Then
                Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mPrevCompMonitorModStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mPrevCompMonitorModStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            RemoveSession()
            'frm.ComplyCompMonitorModInfo = mtmpComplyCompMonitorModStatusList(dgDueMonitoringList.CurrentRowIndex).PartMonitorModInfo
            If mIsSpareComponent = 0 Then
                mCompInfo = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                Session("mCompInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
            Else
                mCompInfo = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description
                Session("mCompInfo") = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description

            End If
            ''***********************************************
            '  ComplyCompMonitorModInfo = mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).PartMonitorModInfo
            ''MarkLog(Util.Action.Edit, "ComplyCompMonitorModStatus", mCompInfo + "   " + ComplyCompMonitorModInfo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)


            'Commented And Added by Saylee on 3-Dec-2019 , as to open Master form for NOT Appilcable Records and not COMPLY form
            ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorModStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorModStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************

            'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        ElseIf ((mPrevCompMonitorModStatus.IsMaster = False) And (mPrevCompMonitorModStatus.IsCompleted = False) And mPrevCompMonitorModStatus.IsDone = False) Then
            'Commented n Added by Shital on 28-Jun-2021
            ' mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompMonitorModStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mHourType, True)
            mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mrptDueReport.Item(Index).ID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).CompStatusID, mHourType, True, IsForSpareComp:=mCompStatus.IsSpareComp)

            ''''''''' Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
            '''''''' Dim mCompStatus As CompStatus
            '''''''''' mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)


           
            'Commented n Added by Shital on 28-Jun-2021
            'If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
            '    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
            '    Session("mAssemblyStatus") = mAssemblyStatus
            '    mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)
            'Else
            '    mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            'End If

            'Dim mPartMonitorMod As PartMonitorMod = PartMonitorMod.GetPartMonitorMod(mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModID, mHourType)
            'If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
            '    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            '    Session("mAssemblyStatus") = mAssemblyStatus
            '    mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
            'Else
            '    mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            'End If


            Dim mPartMonitorMod As PartMonitorMod
            If mIsSpareComponent = 0 Then
                mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mrptDueReport.Item(Index).StatusMasterID, mHourType)
            Else
                mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModID, mHourType)
            End If

            Session("mPartMonitorMod") = mPartMonitorMod

            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            Session("mCompStatus") = mCompStatus
            Session("EnFrom") = 1
            Session("From") = 1 'Edit record
            Session("NewPage") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorModStatusNew_Ajax.aspx?BackPage=Index.aspx');", True)
            '**********************************************************************
        Else
            mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType, True, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
            Session("EnFrom") = 1 'EditRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
            '''''''''''Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
            '''''''''''Dim mCompStatus As CompStatus
            ''''''''''''' mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)

           
            'Commented n Added by Shital on 28-Jun-2021
            'If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
            '    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
            '    Session("mAssemblyStatus") = mAssemblyStatus
            '    mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)
            'Else
            '    mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            'End If
            'If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
            '    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
            '    Session("mAssemblyStatus") = mAssemblyStatus
            '    mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
            'Else
            '    mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            'End If
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompStatus") = mCompStatus

            'Added By Vikrant On 25-Nov-2014
            If mCompMonitorModStatus.IsAttachmentAdded Then
                Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            RemoveSession()
            'frm.ComplyCompMonitorModInfo = mtmpComplyCompMonitorModStatusList(dgDueMonitoringList.CurrentRowIndex).PartMonitorModInfo
            'Added by Saylee on 5-Aug-2009
            If mIsSpareComponent = 0 Then
                mCompInfo = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description
                Session("mCompInfo") = mrptDueReport.Item(Index).RegNo + "->" + mrptDueReport.Item(Index).ModelSerialNo + "->" + mrptDueReport.Item(Index).PartSerialNo + "->" + mrptDueReport.Item(Index).Number + "->" + mrptDueReport.Item(Index).Reference + "->" + mrptDueReport.Item(Index).Type + "->" + mrptDueReport.Item(Index).ATAChapter.ToString + "->" + mrptDueReport.Item(Index).Description

            Else

                mCompInfo = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description
                Session("mCompInfo") = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description

            End If
            '***********************************************
            '  ComplyCompMonitorModInfo = mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).PartMonitorModInfo
            ''MarkLog(Util.Action.Edit, "ComplyCompMonitorModStatus", mCompInfo + "   " + ComplyCompMonitorModInfo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorModStatus_AJAX.aspx?GChildPage2=Index.aspx');", True)
        End If

        'Added By Utkarsh On 28-Jul-2011 For All19072011
        If mIsSpareComponent = 0 Then
            MaintDetail = "Reg No. : " + mrptDueReport(Index).RegNo & " Assembly Info : " & mrptDueReport(Index).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(Index).TypeDet & " Done On Date : " & mrptDueReport(Index).DoneOnDate & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
            MarkLog(Util.Action.Edit, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, mrptDueReport(Index).ID, EventLogID)

        Else
            MaintDetail = "Reg No. : " + mTmpComplyCompMonitorModStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorModStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorModStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorModStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorModStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorModStatusList(Index).DoneOnValueFormatted
            MarkLog(Util.Action.Edit, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorModStatusList(Index).CompMonitorModStatusID, EventLogID)

        End If
        'End
    End Sub
    Private Sub HistoryRecords(ByVal Index As Int32)
        ' mMachine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
        'Added by Saylee on 5-Nov-2020 for ALL27072020
        'mMachine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
        Dim mHourType As Integer = 1
       Dim mPrevCompMonitorModStatus As CompMonitorModStatus
        Dim mCompMonitorModStatus As CompMonitorModStatus
        Dim mCompStatus As CompStatus
        Dim mAssemblyStatus As AssemblyStatus


        If mIsSpareComponent = 0 Then
            If mrptDueReport.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mrptDueReport(Index).MachineID)
                mHourType = mMachine.HourType
            End If
           If mrptDueReport.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mrptDueReport(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mrptDueReport.Item(Index).CompStatusID, mrptDueReport.Item(Index).AssemblyStatusID, mrptDueReport.Item(Index).DoneOnDate.ToString)
            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mrptDueReport.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            End If
            mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mrptDueReport(Index).ID, mrptDueReport(Index).AssemblyStatusID, mrptDueReport(Index).CompStatusID, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)

        Else
            If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Then
                mMachine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
                mHourType = mMachine.HourType
            End If
            If mTmpComplyCompMonitorModStatusList.Item(Index).IsSpareComponent = False Or rdbSpareAssemblyComponent.Checked = True Then 'Added by Saylee on 5-Nov-2020 for ALL27072020
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                mCompStatus = CompStatus.GetCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList.Item(Index).DoneOn.ToString)
            Else
                mCompStatus = CompStatus.GetSpareCompStatus(mTmpComplyCompMonitorModStatusList.Item(Index).CompStatusID, IsForSpareComp:=mIsSpareComponent)
            End If
            mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mTmpComplyCompMonitorModStatusList(Index).CompMonitorModStatusID, mTmpComplyCompMonitorModStatusList(Index).AssemblyStatusID, mTmpComplyCompMonitorModStatusList(Index).CompStatusID, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)

        End If
            'If mPrevCompMonitorModStatus.IsMaster Then
            '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
            '    msg.ReplacePage = "wfComplyCompMonitorModStatusList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
            '    msg.Show()
            '    Exit Sub
            'Else
        mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType, CompStatus:=mCompStatus, IsForSpareComp:=mCompStatus.IsSpareComp)
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
            Session("EnFrom") = 1 'EditRecord
        Session("mMachine") = mMachine
            ''' Session("mAssemblyStatus") = mAssemblyStatus
            Session("mCompStatus") = mCompStatus

        If mIsSpareComponent = 0 Then
            Session("ATA") = mrptDueReport.Item(Index).ATAChapter.ToString
            Session("Description") = mrptDueReport.Item(Index).Description
            Session("PartSerialNo") = mrptDueReport.Item(Index).PartSerialNo
            MaintDetail = "Reg No. : " + mrptDueReport(Index).RegNo & " Assembly Info : " & mrptDueReport(Index).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(Index).TypeDet & " Done On Date : " & mrptDueReport(Index).DoneOnDate & " Done On Value : " & mrptDueReport(Index).DoneAt2ForGrid
            MarkLog(Util.Action.View, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, mrptDueReport(Index).ID, EventLogID)

        Else

            mCompInfo = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description
            Session("mCompInfo") = mTmpComplyCompMonitorModStatusList.Item(Index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(Index).Description
            Session("ATA") = mTmpComplyCompMonitorModStatusList.Item(Index).ATA.ToString
            Session("Description") = mTmpComplyCompMonitorModStatusList.Item(Index).Description
            Session("PartSerialNo") = mTmpComplyCompMonitorModStatusList.Item(Index).PartSerialNo
            MaintDetail = "Reg No. : " + mTmpComplyCompMonitorModStatusList(Index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorModStatusList(Index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorModStatusList(Index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorModStatusList(Index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorModStatusList(Index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorModStatusList(Index).DoneOnValueFormatted
            MarkLog(Util.Action.View, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, mTmpComplyCompMonitorModStatusList(Index).CompMonitorModStatusID, EventLogID)

        End If

        
        mUpdateComplyHistoryCompMonitorModStatusList = UpdateComplyHistoryCompMonitorModStatusList.GetComplyHistoryCompMonitorModStatusList(mCompStatus.CompID, mCompMonitorModStatus.PartMonitorModID, mHourType)
        Session("mUpdateComplyHistoryCompMonitorModStatusList") = mUpdateComplyHistoryCompMonitorModStatusList

            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorModStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompDirectiveHistoryWindow", "OpenCompDirectiveHistoryWindow();", True)
            'End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)

        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "Delete Component Modification Status.", MsgBoxStyle.YesNo, "Delete")
        ''Commented n Added by Shital on 28-Jun-2021

        If mIsSpareComponent = 0 Then
            mrptDueReport.CurrentIndex = Index
            Session("mrptDueReport") = mrptDueReport
        Else
            mTmpComplyCompMonitorModStatusList.CurrentIndex = Index
            Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
        End If
        
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            'Added By Utkarsh On 28-Jul-2011 For All19072011

                            If mIsSpareComponent = 0 Then

                                IDForEventLog = mrptDueReport(mrptDueReport.CurrentIndex).ID


                                mMonitorInfo = mrptDueReport.Item(mrptDueReport.CurrentIndex).TypeDet
                                mMonitorType = "" ' mrptDueReport.Item(mrptDueReport.CurrentIndex).MonitorType
                                mMonitorDesc = mrptDueReport.Item(mrptDueReport.CurrentIndex).Description
                                mAircraft = mrptDueReport.Item(mrptDueReport.CurrentIndex).RegNo
                                mAssemblyDetails = mrptDueReport.Item(mrptDueReport.CurrentIndex).Assembly 'mrptDueReport.Item(mrptDueReport.CurrentIndex).ModelName + "-" + mrptDueReport.Item(mrptDueReport.CurrentIndex).SerialNo '+ (IIf(mrptDueReport.Item(mrptDueReport.CurrentIndex).Position <> "", " (" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Position + ")", ""))
                                mCompDetail = mrptDueReport.Item(mrptDueReport.CurrentIndex).PartName + "-" + mrptDueReport.Item(mrptDueReport.CurrentIndex).CompSerialNo + (IIf(mrptDueReport.Item(mrptDueReport.CurrentIndex).Position <> "", " (" + mrptDueReport.Item(mrptDueReport.CurrentIndex).Position + ")", ""))



                                MaintDetail = "Reg No. : " + mrptDueReport(mrptDueReport.CurrentIndex).RegNo & " Assembly Info : " & mrptDueReport(mrptDueReport.CurrentIndex).ModelSerialNo.Replace(Environment.NewLine, " ") & " Part Info : " & mrptDueReport(mrptDueReport.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mrptDueReport(mrptDueReport.CurrentIndex).TypeDet & " Done On Date : " & mrptDueReport(mrptDueReport.CurrentIndex).DoneOnDate & " Done On Value : " & mrptDueReport(mrptDueReport.CurrentIndex).DoneAt2ForGrid
                                mComplyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> <b> Component Details : </b>" + mCompDetail + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mMonitorDesc

                                'End
                                'Added by Saylee on 9th-Oct-2009
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mrptDueReport(mrptDueReport.CurrentIndex).ID, 10)
                                If mrptDueReport(mrptDueReport.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(mrptDueReport.CurrentIndex).ID)
                                End If
                                CompMonitorModStatus.DeleteCompMonitorModStatus(mrptDueReport(mrptDueReport.CurrentIndex).ID)
                            Else
                                IDForEventLog = mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID

                                mMonitorInfo = mTmpComplyCompMonitorModStatusList.Item(mTmpComplyCompMonitorModStatusList.CurrentIndex).MonitorInfo
                                mMonitorType = mTmpComplyCompMonitorModStatusList.Item(mTmpComplyCompMonitorModStatusList.CurrentIndex).MonitorType
                                mMonitorDesc = mTmpComplyCompMonitorModStatusList.Item(mTmpComplyCompMonitorModStatusList.CurrentIndex).Description
                                mAircraft = mTmpComplyCompMonitorModStatusList.Item(mTmpComplyCompMonitorModStatusList.CurrentIndex).MachineInfo
                                mAssemblyDetails = mTmpComplyCompMonitorModStatusList.Item(mTmpComplyCompMonitorModStatusList.CurrentIndex).AssemblyInfo
                                mCompDetail = mTmpComplyCompMonitorModStatusList.Item(mTmpComplyCompMonitorModStatusList.CurrentIndex).CompInfo

                                MaintDetail = "Reg No. : " + mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).DoneOnValueFormatted
                                mComplyMonitorDetailForMail = "<b> Aircraft : </b>" + mAircraft + "<br/> <b> Assembly Details : </b>" + mAssemblyDetails + "<br/> <b> Component Details : </b>" + mCompDetail + "<br/> <b> Monitor Info. : </b>" + mMonitorInfo + "<br/> <b>Description : </b>" + mMonitorDesc
                                'End
                                'Added by Saylee on 9th-Oct-2009
                                mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID, 10)
                                '=============================
                                If mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                    mFileAttach = FileAttach.GetAttachment(mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID)
                                End If
                                CompMonitorModStatus.DeleteCompMonitorModStatus(mTmpComplyCompMonitorModStatusList(mTmpComplyCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID)

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
                                MarkLog(Util.Action.Delete, "ComponentModifications", "Can't delete : " & MaintDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'mEnquiry.ID)'Added By Utkarsh On 28-Jul-2011 For All19072011
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID) 'Added By Utkarsh On 28-Jul-2011 For All19072011
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
            ' DataFieldBind()
        End If
    End Sub
    'Added By Prashant 31-Mar-2011
    Private Sub SetRights()
        If (User.IsInRole("MachineComponentModificationNew")) = False Then
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
                Dim List = (From StatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo In mTmpComplyCompMonitorModStatusList
                            Select StatusInfo).ToList.Take(RecordsToShow)

                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList

            End If
        End If

        dgDueMonitoringList.DataBind()
        SetGrid()
    End Sub

    Public Sub SendMail(mComplyMonitorDetailForMail)
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'If AppSettings("MailsRequire") = "True" Then
        If mModuleList.Item("ComponentModifications").MailsRequire = True Then
            If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
                'Do nothing
                Exit Sub
            End If
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Task Details :  <br/> <br/>  " & mComplyMonitorDetailForMail & " <br/> <b> Deleted by User:</b> " + User.Identity.Name + "<b> on: </b>" + New SmartDate(Today.Date).FormattedText + "</font></P> ")
            str = str + ("</body></html>")
            'SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Assembly Inspections Deleted", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), Info:=str, ToMailID:=mModuleList.Item("Order").SendToMailID, Remark:=Session("SendMailRemark"), ReportGenratedBy:=Session("ReportGenratedBy"))

            SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Task Deleted", Info:=str, ToMailID:=mModuleList.Item("ComponentModifications").SendToMailID, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
        End If
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

        'calDate.Value = calDate.Value
        'calDate.TitleText = calDate.Text
        'calDate.DateToday = CDate(calDate.Text)
        'calDate.SelectedDate = CDate(calDate.Text)
        Session("DoneOn") = DoneOn

        'mMachineNameValueList = tmpMachineList.GetMachineList(, , , , , "<SELECT>")

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , False, , , True)
        Dim MachineName As String 'Added By Vikrant for faster processing

        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraftList.DataSource = mMachineNameValueList
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

        'bind Assembly Combo
        mAssemblylist = AssemblyList.GetAssemblyList(0, MachineId, txtDate.Text, "(All)", IsForSpareAssembly:=rdbSpareAssemblyComponent.Checked)
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        '
        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then AssemId = mAssemblylist(0).ID Else AssemId = New Guid(AssemblyId)
        'Binding Grid
        'added By Deven
        AssemblyId = AssemId.ToString

        If PartNo Is Nothing Then PartNo = ""
        If SerialNo Is Nothing Then SerialNo = ""
        If MonitorTypeID Is Nothing Then MonitorTypeID = "0"
        If CodeFormNoDesc Is Nothing Then CodeFormNoDesc = ""

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

        If mIsSpareComponent = 0 Then
            ' Added by Shital on 28-Jun-2021
            mrptDueReport = rptDueReport.GetList(DoneOn, AircraftName, , True, , AssemId.ToString, 6, CType(MonitorTypeID, Integer), ShowNotApplicable, chkOneTimeMasterRecords.Checked, _
                                           CodeFormNoDesc:=Trim(CodeFormNoDesc), PartName:=Trim(PartNo), CompSerialNo:=Trim(SerialNo), DirectiveNo:=Trim(DirectiveNo))
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
            'mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(calDate.Value.ToString, MachineId, Trim(txtPart.Text), Trim(txtSerialNo.Text), AssemId)

            mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(DoneOn, MachineId, PartNo, SerialNo, AssemId, , , , CType(MonitorTypeID, Integer), , , DirectiveNo, , ShowNotApplicable, IIf(ShowOneTimeMasterRecords = True, False, True), SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareComponent:=CBool(mIsSpareComponent), ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))  'SpareComponent Added By Vikrant On 27-Jul-2020 For ALL27072020
            If AppSettings("IsShowAllRecordsVisible") = "True" Then

                Dim List = (From StatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo In mTmpComplyCompMonitorModStatusList
                                                           Select StatusInfo).ToList.Take(RecordsToShow)

                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList

            End If
            Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
        End If
      

        'Vikrant
        
        ' 


        'Added by Saylee on 30-July-2009
        mPartMonitorModTypeList = PartMonitorModTypeList.GetPartMonitorModTypeList("(ALL)")
        cmbMonitorType.DataSource = mPartMonitorModTypeList

        DataBind()
        If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then cmbAircraftList.SelectedIndex = 0 Else cmbAircraftList.SelectedValue = AircraftId
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

        txtDirectiveNo.Text = DirectiveNo 'Added bu Saylee on 7-Aug-2009
        chkApplicable.Checked = IIf(ShowNotApplicable, True, False)
        'Added By Vikrant On 27-Jul-2020 For ALL27072020
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
        'End
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 28-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            'Added By Saylee On 27-Jul-2020 For ALL27072020
            If Session("mIsSpareComponent") Is Nothing Then
                mIsSpareComponent = Request.QueryString("SpareComponent")
            End If
            Session("mIsSpareComponent") = mIsSpareComponent
            'End
            Session("MiddleFrame") = "wfComplyCompMonitorModStatusList_Ajax.aspx?SpareComponent=" & Session("mIsSpareComponent") 'SpareComponent Added By Vikrant On 27-Jul-2020 For ALL27072020
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow
            DataFieldBind()
            ControlVisibility()
            SetPage()
            SetRights()
            SetGrid()
            cmbAircraftList.Focus()
        End If
    End Sub
    Private Sub dgDueMonitoringList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueMonitoringList.RowCommand
        Dim index As Int32 '= e.Item.ItemIndex + dgDueMonitoringList.CurrentPageIndex * dgDueMonitoringList.PageSize

        Select Case e.CommandName
            Case "Comply"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentModificationsNew")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ComplyRecord(index)
            Case "EditRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentModificationsView") And Not User.IsInRole("ComponentModificationsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecord(index)
            Case "DeleteRec"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentModificationsDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(index)
            Case "History"
                index = (CInt(e.CommandArgument) + (dgDueMonitoringList.PageSize * dgDueMonitoringList.PageIndex))
                GridBind()
                SetGrid()
                ControlVisibility()
                If (Not User.IsInRole("ComponentModificationsView") And Not User.IsInRole("ComponentModificationsEdit")) Then
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
                    mFileAttach = FileAttach.GetAttachment(mrptDueReport(index).ID)
                Else
                    mFileAttach = FileAttach.GetAttachment(mTmpComplyCompMonitorModStatusList(index).ID) 'Commented n Added by Shital on 28-Jun-2021
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
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                End If
        End Select
    End Sub
    'Private Sub dgDueMonitoringList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDueMonitoringList.PageIndexChanging
    '    dgDueMonitoringList.PageIndex = e.NewPageIndex
    '    'mStockItemList = StockItemList.GetStockItemList("", "")
    '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList
    '    Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
    '    dgDueMonitoringList.DataBind()
    '    SetGrid()
    'End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            RecordsToShow = dgDueMonitoringList.PageSize
            Session("RecordsToShow") = RecordsToShow

            Session("DoneOn") = txtDate.Text
            Session("AircraftId") = cmbAircraftList.SelectedValue
            Session("AssemblyId") = cmbAssembly.SelectedValue
            Session("AircraftName") = cmbAircraftList.SelectedItem.ToString  'Added By Vikrant for faster processing
            'Added By Rahul on 29-Apr-2009
            Session("PartNo") = Trim(txtPart.Text)
            Session("SerialNo") = Trim(txtSerialNo.Text)
            Session("DirectiveNo") = Trim(txtDirectiveNo.Text)
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

            If mIsSpareComponent = 0 Then
                mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, , cmbAssembly.SelectedValue, 6, _
                                        CInt(IIf(cmbMonitorType.SelectedIndex > 0, cmbMonitorType.SelectedValue, 0)), chkApplicable.Checked, _
                                        chkOneTimeMasterRecords.Checked, CodeFormNoDesc:=Trim(txtCodeFormNo.Text), PartName:=Trim(txtPart.Text), _
                                        CompSerialNo:=Trim(txtSerialNo.Text), DirectiveNo:=Trim(txtDirectiveNo.Text))
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
            Else
                mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , DirectiveNo, , chkApplicable.Checked, chkOneTimeMasterRecords.Checked, SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareComponent:=CBool(mIsSpareComponent), ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))  'SpareComponent Added By Vikrant On 27-Jul-2020 For ALL27072020

                mTmpComplyCompMonitorModStatusList.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
                'Vikrant
                If AppSettings("IsShowAllRecordsVisible") = "True" Then

                    Dim List = (From StatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo In mTmpComplyCompMonitorModStatusList
                                                             Select StatusInfo).ToList.Take(RecordsToShow)
                   
                    dgDueMonitoringList.DataSource = List
                Else
                    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList

                End If
                Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList

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
    Private Sub cmbAircraftList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
        REM: Assembly Combo is updated according to the machine(Aircraft) selected. 
        mAssemblylist = AssemblyList.GetAssemblyList(0, New Guid(cmbAircraftList.SelectedValue.ToString).ToString, txtDate.Text, "(ALL)")
        cmbAssembly.DataSource = mAssemblylist
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataBind()
        'New Addition By Yogita on 9-Jan-2008 to solve Bug No:-LCMMS3
        If cmbAircraftList.Enabled = True Then
            cmbAircraftList.Focus()
        End If

        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly


        Findnow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session.Remove("From")
        Session.Remove("DoneOn")
        Session.Remove("AircraftId")
        Session.Remove("AssemblyId")
        Session.Remove("MonitorTypeID")  'Added by Saylee on 30-July-2009
        Session.Remove("ATA")
        Session("MiddleFrame") = ""
        Session.Remove("CodeFormNoDesc")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click, btnAddNew.Click
        If IsValid Then
            'Added By Utkarsh On 28-Jul-2011 For All19072011
            MarkLog(Util.Action.[New], "ComponentModifications", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'End
            Session("AircraftIdForMod") = cmbAircraftList.SelectedValue.ToString
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorModStatusListNew.aspx?BackPage=Index.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompModListNewWindow", "OpenCompModListNewWindow()", True)
            Session("NewPage") = "True"
        End If
    End Sub
    'New addition by Rupali on 23-Jun-09 for Sorting Order
    Private Sub dgDueMonitoringList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueMonitoringList.Sorting
        ' mTmpComplyCompMonitorModStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
        ''Vikrant
        'If AppSettings("IsShowAllRecordsVisible") = "True" Then
        '    Dim List = (From StatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo In mTmpComplyCompMonitorModStatusList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        '    dgDueMonitoringList.DataSource = List
        'Else
        '    dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList
        'End If
        'Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
        'dgDueMonitoringList.DataBind()
        'SetGrid()
        mrptDueReport.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mrptDueReport") = mrptDueReport
        'Vikrant
        If AppSettings("IsShowAllRecordsVisible") = "True" Then
            Dim List = (From StatusInfo As rptDueReport.rptDueReportInfo In mrptDueReport
                                                       Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = List
        Else
            dgDueMonitoringList.DataSource = mrptDueReport
        End If
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
    Private Sub txtDirectiveNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDirectiveNo.TextChanged
        DirectiveNo = txtDirectiveNo.Text
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnCompDirectiveHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnCompDirectiveHistory.Click
        

        If mIsSpareComponent = 0 Then
            mrptDueReport = rptDueReport.GetList(txtDate.Text, cmbAircraftList.SelectedItem.ToString, , True, , cmbAssembly.SelectedValue, 6, CInt(cmbMonitorType.SelectedValue), chkApplicable.Checked, chkOneTimeMasterRecords.Checked, Trim(txtCodeFormNo.Text), Trim(txtPart.Text), Trim(txtSerialNo.Text))
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
        Else
            mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(txtDate.Text, cmbAircraftList.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), , , , cmbMonitorType.SelectedValue, , , DirectiveNo, , chkApplicable.Checked, SortBy:="MinimumRemainingValue", CodeFormNoDesc:=Trim(txtCodeFormNo.Text), IsSpareComponent:=CBool(mIsSpareComponent), ShowComponentForSpareAssembly:=rdbSpareAssemblyComponent.Checked, IsSpareOrRemovedComponent:=IIf(rdbSpareComponent.Checked, 1, IIf(rdbRemovedComp.Checked, 2, 0)))  'SpareComponent Added By Vikrant On 27-Jul-2020 For ALL27072020
            'Vikrant
            mTmpComplyCompMonitorModStatusList.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
            If AppSettings("IsShowAllRecordsVisible") = "True" Then
                Dim List = (From StatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo In mTmpComplyCompMonitorModStatusList
                                                           Select StatusInfo).ToList.Take(RecordsToShow)
                dgDueMonitoringList.DataSource = List
            Else
                dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList
            End If
            Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
        End If
      
        'End
        dgDueMonitoringList.DataBind()
        SetPage()
        ControlVisibility()
        SetGrid()
        upnlgrid.Update()
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        Findnow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub chkApplicable_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkApplicable.CheckedChanged
        Findnow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Protected Sub chkOneTimeMasterRecords_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOneTimeMasterRecords.CheckedChanged
        Findnow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        Findnow()
        upnlgrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub lnkShowAllRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAllRecords.Click, lnkShowAllRecordsTop.Click

        If mIsSpareComponent = 0 Then
            RecordsToShow = mrptDueReport.Count
           
            dgDueMonitoringList.DataSource = mrptDueReport
        Else
            RecordsToShow = mTmpComplyCompMonitorModStatusList.Count
            'Dim list = (From StatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo In mTmpComplyCompMonitorModStatusList
            '                                               Select StatusInfo).ToList.Take(RecordsToShow)
            dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList
        End If
        Session("RecordsToShow") = RecordsToShow

      
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
    'Added by Vikrant on 27-Jul-2020 fro 27-Jul-2020 For ALL27072020
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

            Findnow()
            upnlgrid.Update()
        ElseIf rdbSpareComponent.Checked Or rdbRemovedComp.Checked Then
            phAssembly.Visible = False
            Findnow()
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

    Dim DirectiveNo As String
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If (Not User.IsInRole("ComponentModificationsPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        '   dgDueMonitoringList.DataSource = mTmpComplyCompMonitorModStatusList
        dgDueMonitoringList.DataSource = mrptDueReport
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
        ReportDetails.Add(New rptStatus(, 0, "", _
              , , , dgDueMonitoringList.Columns.Item(1).HeaderText, , dgDueMonitoringList.Columns.Item(5).HeaderText, _
              dgDueMonitoringList.Columns.Item(6).HeaderText, dgDueMonitoringList.Columns.Item(8).HeaderText, _
              dgDueMonitoringList.Columns.Item(9).HeaderText, dgDueMonitoringList.Columns.Item(10).HeaderText, _
              dgDueMonitoringList.Columns.Item(11).HeaderText, dgDueMonitoringList.Columns.Item(12).HeaderText, _
              dgDueMonitoringList.Columns.Item(13).HeaderText, dgDueMonitoringList.Columns.Item(14).HeaderText, _
             dgDueMonitoringList.Columns.Item(15).HeaderText, dgDueMonitoringList.Columns.Item(16).HeaderText, _
              dgDueMonitoringList.Columns.Item(17).HeaderText, dgDueMonitoringList.Columns.Item(18).HeaderText, , , _
              dgDueMonitoringList.Columns.Item(19).HeaderText, , , , , , , dgDueMonitoringList.Columns.Item(20).HeaderText))

        Dim TotalCount As Integer
        ' TotalCount = Me.mTmpComplyCompMonitorModStatusList.Count
        TotalCount = Me.mrptDueReport.Count
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

            ReportDetails.Add(New rptStatus(, 1, , _
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8), _
             str(9), str(10), str(11), str(12), str(13), , , str(14), , , , , , , str(15)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Comply Component Modification Status Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, Searchstr5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        'If mTmpComplyCompMonitorModStatusList.Count = 0 Then
        If mrptDueReport.Count = 0 Then
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
        '    MarkLog(Util.Action.Print, "ComplyCompMonitorModStatus", "List of Comply Component Monitor Modification Status Report", Util.ErrorType.NoError, Guid.Empty)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

#End Region

#End Region





End Class