
'Created By     :   Saylee
'Dated           :   31-May-2016



Public Class wfDailyStatusLogReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mDailyStatusLogReport As DailyStatusLogReport
    Private mMachineNameValueList As MachineNameValueList
    Dim DateIndex, FromDate, ToDate, mSearchingCriteria As String
    Dim AOnDate, AOdate As String
    Dim mIsExcel As Boolean
    Dim EventLogID As Guid
    Private mIsPreview As Boolean = False
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mDailyStatusLogReport = Session("mDailyStatusLogReport")
        mMachineNameValueList = Session("mMachineNameValueList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mDailyStatusLogReport") = mDailyStatusLogReport
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mDailyStatusLogReport")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub Display()
        lblyear1.Visible = True
        lblAircraft1.Visible = True
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDailyStatusLogReport.aspx?" Then
            Session.Remove("mDailyStatusLogReport")
            Session.Remove("mMachineNameValueList")
        End If
    End Sub
    'Private Sub SetCombo()
    '    If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
    '        For i As Integer = -10 To 10
    '            cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
    '        Next
    '        cmbYear.SelectedIndex = 10
    '    End If

    '    For k As Integer = 1 To 12
    '        Dim mon As String = MonthName(k, False)
    '        cmbMonth.Items.Add(mon)
    '    Next
    'End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
    End Sub
    Public Sub setValues()
        Dim mAircraft As String = ""
        '  lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        lblyear1.Text = "From " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " To " + New SmartDate(txtToDate.Text.ToString).FormattedText
        If cmbAircraft.SelectedIndex > 0 Then
            mAircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft : " & mAircraft
        Else
            mAircraft = ""
            lblAircraft1.Text = "Aircraft : "
        End If
        mSearchingCriteria = lblyear1.Text + ", " + lblAircraft1.Text 'Added by Shital on 18-Jan-2021
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal ByExcel As Boolean = False)
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsDailyStatusLogReport As New dsDailyStatusLogReport
        'Added by Vikrant On 27-Mar-2019 For StarAir27032019
        If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "IPA" Then
            myReport = New crDailyStatusLogReportStarAir
        Else
            myReport = New crDailyStatusLogReport
        End If
        'End
        setValues()
        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String = String.Empty
        Dim SearchStr4 As String = String.Empty
        Dim SearchStr6 As String = String.Empty
        Dim SearchStr7 As String = String.Empty
        Dim SearchStr8 As String = String.Empty
        Dim SearchStr9 As String = String.Empty
        Dim SearchStr11 As String = String.Empty 'Added by Vikrant On 27-Mar-2019 For StarAir27032019

        SearchStr1 = New SmartDate(txtFromDate.Text.ToString).FormattedText '"All values in report are from " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " to " + New SmartDate(txtToDate.Text.ToString).FormattedText 'StrConv((cmbMonth.SelectedItem.Text).Substring(0, 3), vbUpperCase)
        SearchStr2 = New SmartDate(txtToDate.Text.ToString).FormattedText 'cmbYear.SelectedItem.Text

        If cmbAircraft.SelectedIndex > 0 Then
            SearchStr3 = cmbAircraft.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        Dim StartDateM As New SmartDate
        '  StartDateM = New SmartDate(CStr(DateAdd(DateInterval.Month, cmbMonth.SelectedIndex + 1 - 1, DateSerial(CInt(cmbYear.SelectedItem.ToString), 1, 1))))
        StartDateM = New SmartDate(txtFromDate.Text.ToString)

        Dim mAssemblylist As AssemblyList
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue.ToString, StartDateM.Text.Trim.ToString, , True, True)
        Dim EngCnt As Integer = 0
        Dim PropCnt As Integer = 0
        For i As Integer = 0 To mAssemblylist.Count - 1
            If mAssemblylist(i).AssemblyTypeID = 1 Then
                SearchStr4 = mAssemblylist(i).ModelSerialNoPostion
            ElseIf mAssemblylist(i).AssemblyTypeID = 2 Then
                If EngCnt = 0 Then
                    SearchStr6 = mAssemblylist(i).ModelSerialNoPostion
                    EngCnt = EngCnt + 1
                Else
                    SearchStr7 = mAssemblylist(i).ModelSerialNoPostion
                End If
            ElseIf mAssemblylist(i).AssemblyTypeID = 3 Then
                If PropCnt = 0 Then
                    SearchStr8 = mAssemblylist(i).ModelSerialNoPostion
                    PropCnt = PropCnt + 1
                Else
                    SearchStr9 = mAssemblylist(i).ModelSerialNoPostion
                End If
            ElseIf mAssemblylist(i).AssemblyTypeID = 4 Then 'Added by Vikrant On 27-Mar-2019 For StarAir27032019
                SearchStr11 = mAssemblylist(i).ModelSerialNoPostion

            End If
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, SearchStr7, SearchStr8, SearchStr9, AppSettings("Logo"), SearchStr11:=SearchStr11) 'Changed By Utkarsh For Report Logo.

        'mDailyStatusLogReport = DailyStatusLogReport.GetDailyStatusLogReport(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbAircraft.SelectedValue.ToString, ByExcel)
        mDailyStatusLogReport = DailyStatusLogReport.GetDailyStatusLogReport(txtFromDate.Text.ToString, txtToDate.Text.ToString, cmbAircraft.SelectedValue.ToString, ByExcel)

        If mDailyStatusLogReport.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAuditFindings.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(dsDailyStatusLogReport)
        '----------------------------------------------------------
        da.Fill(dsDailyStatusLogReport, mDailyStatusLogReport)
        da.Fill(dsDailyStatusLogReport, Report)
        da.Fill(dsDailyStatusLogReport, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(dsDailyStatusLogReport)
        Session("CrystalReport") = myReport

        If ByMail Then
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            '   Session("UserEmailID") = SI.UTILITY.User.GetUser(Thread.CurrentPrincipal.Identity.Name).UserEmail
            Session("UserEmailID") = mModuleList.Item("DailyStatusLogReport").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("DailyStatusLogReport").SendCCMailID
            '--------------------------

            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Daily Status Log Report", "Daily Status Log Report", _
                                      "From Date : " + SearchStr1 + ", " + "To Date : " + SearchStr2 + ", " + "Aircraft : " & IIf(SearchStr3 <> "", SearchStr3, "All"), _
                                      "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                        SmtpHost:=mModuleList.Item("DailyStatusLogReport").SmtpHost, SmtpPort:=mModuleList.Item("DailyStatusLogReport").SmtpPort, _
                                        SmtpUser:=mModuleList.Item("DailyStatusLogReport").SmtpUser, SmtpPassword:=mModuleList.Item("DailyStatusLogReport").SmtpPassword)
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        ElseIf ByExcel Then
            SetExcel(mDailyStatusLogReport, Report, "TLP Register")
        Else
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            ResetValues()
        End If
        MarkLog(Util.Action.Print, "DailyStatusLogReport", IIf(ByExcel = True, "Export To excel ", "") + mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub SetExcel(mDailyStatusLogReport As DailyStatusLogReport, SearchingCriteria As ReportData, ReportName As String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsDailyStatusLogReport As New dsDailyStatusLogReport
        da.Fill(dsDailyStatusLogReport, "DailyStatusLogReport", mDailyStatusLogReport)
        da.Fill(dsDailyStatusLogReport, "ReportData", SearchingCriteria)

        Dim columnToRemoveUTC As String()
        Dim columnToRemove As String() = {
                                                 "ID", _
                                                 "LogID", _
                                                 "IsUTC", _
                                                 "HourType", _
                                                 "LogPageNoFormatted", _
                                                 "LogDetailID", _
                                                 "Col3Final", _
                                                 "Col4Final", _
                                                 "ColEng13Final", _
                                                 "ColEng14Final", _
                                                 "ColEng23Final", _
                                                 "ColEng34Final", _
                                                 "ColProp12Final", _
                                                 "ColProp13Final", _
                                                 "ColProp14Final", _
                                                 "ColProp22Final", _
                                                 "ColProp23Final", _
                                                 "ColProp24Final", _
                                                 "ColProp11Diff", _
                                                 "ColProp12Diff", _
                                                 "ColProp13Diff", _
                                                 "ColProp14Diff", _
                                                 "ColProp21Diff", _
                                                 "ColProp22Diff", _
                                                 "ColProp23Diff", _
                                                 "ColProp24Diff", _
                                                 "AssemblyID", _
                                                 "TimeOnGround", _
                                                 "Col1Label", _
                                                 "Col1Diff", _
                                                 "Col2Label", _
                                                 "Col3Label", _
                                                 "Col3Diff", _
                                                 "Col4Label", _
                                                 "Col4Diff", _
                                                 "FlightNo", _
                                                 "TotalTimeInAir", _
                                                 "Remark", _
                                                 "LogPageNoFormattedForExcel", _
                                                 "LogTypeID", _
                                                 "LogDateForOrderBy", _
                                                 "ColEng11Label", _
                                                 "ColEng11Diff", _
                                                 "ColEng12Label", _
                                                 "ColEng13Label", _
                                                 "ColEng13Diff", _
                                                 "ColEng14Label", _
                                                 "ColEng14Diff", _
                                                 "ColEng21Label", _
                                                 "ColEng21Diff", _
                                                 "ColEng22Label", _
                                                 "ColEng23Label", _
                                                 "ColEng23Diff", _
                                                 "ColEng24Label", _
                                                 "ColEng24Diff", _
                                                 "ColEng24Final", _
                                                 "ColProp11Label", _
                                                 "ColProp12Label", _
                                                 "ColProp13Label", _
                                                 "ColProp14Label", _
                                                 "ColProp21Label", _
                                                 "ColProp22Label", _
                                                 "ColProp23Label", _
                                                 "ColProp24Label", _
                                                 "BlockTimeDec", _
                                                 "TimeInAirDec", _
                                                 "Col1DiffInDecimal", _
                                                 "Col1DiffInInteger", _
                                                 "Col1DiffPeriodID", _
                                                 "Col1DiffPeriodUnitID", _
                                                 "Col1FinalInInteger", _
                                                 "Col2DiffInDecimal", _
                                                 "Col2DiffInInteger", _
                                                 "Col2DiffPeriodID", _
                                                 "Col2DiffPeriodUnitID", _
                                                 "Col2FinalInInteger", _
                                                 "Col3DiffInDecimal", _
                                                 "Col3DiffInInteger", _
                                                 "Col3DiffPeriodID", _
                                                 "Col3DiffPeriodUnitID", _
                                                 "Col3FinalInInteger", _
                                                 "Col4DiffInDecimal", _
                                                 "Col4DiffInInteger", _
                                                 "Col4DiffPeriodID", _
                                                 "Col4DiffPeriodUnitID", _
                                                 "Col4FinalInInteger", _
                                                 "TimeInAirInteger", _
                                                 "Description", _
                                                 "LogText", _
                                                 "LogTextNo", _
                                                 "LogTypeName", _
                                                 "Item", _
                                                 "ColEng11DiffInDecimal", _
                                                 "ColEng11DiffInInteger", _
                                                 "ColEng11DiffPeriodID", _
                                                 "ColEng11DiffPeriodUnitID", _
                                                 "ColEng11FinalInInteger", _
                                                 "ColEng12DiffInDecimal", _
                                                 "ColEng12DiffInInteger", _
                                                 "ColEng12DiffPeriodID", _
                                                 "ColEng12DiffPeriodUnitID", _
                                                 "ColEng12FinalInInteger", _
                                                 "ColEng21DiffInDecimal", _
                                                 "ColEng21DiffInInteger", _
                                                 "ColEng21DiffPeriodID", _
                                                 "ColEng21DiffPeriodUnitID", _
                                                 "ColEng21FinalInInteger", _
                                                 "ColEng22DiffInDecimal", _
                                                 "ColEng22DiffInInteger", _
                                                 "ColEng22DiffPeriodID", _
                                                 "ColEng22DiffPeriodUnitID", _
                                                 "ColEng22FinalInInteger", _
                                                 "ColProp11DiffInDecimal", _
                                                 "ColProp11DiffInInteger", _
                                                 "ColProp11DiffPeriodID", _
                                                 "ColProp11DiffPeriodUnitID", _
                                                 "ColProp11FinalInInteger", _
                                                 "ColProp12DiffInDecimal", _
                                                 "ColProp12DiffInInteger", _
                                                 "ColProp12DiffPeriodID", _
                                                 "ColProp12DiffPeriodUnitID", _
                                                 "ColProp12FinalInInteger", _
                                                 "ColProp21DiffInDecimal", _
                                                 "ColProp21DiffInInteger", _
                                                 "ColProp21DiffPeriodID", _
                                                 "ColProp21DiffPeriodUnitID", _
                                                 "ColProp21FinalInInteger", _
                                                 "ColProp22DiffInDecimal", _
                                                 "ColProp22DiffInInteger", _
                                                 "ColProp22DiffPeriodID", _
                                                 "ColProp22DiffPeriodUnitID", _
                                                 "ColProp22FinalInInteger", _
                                                 "Col1Value", _
                                                 "Col2Value", _
                                                 "Col3Value", _
                                                 "Col4Value", _
                                                 "ColEng11Value", _
                                                 "ColEng12Value", _
                                                 "ColEng13Value", _
                                                 "ColEng14Value", _
                                                 "ColEng21Value", _
                                                 "ColEng22Value", _
                                                 "ColEng23Value", _
                                                 "ColEng24Value", _
                                                 "ColProp11Value", _
                                                 "ColProp12Value", _
                                                 "ColProp13Value", _
                                                 "ColProp14Value", _
                                                 "ColProp21Value", _
                                                 "ColProp22Value", _
                                                 "ColProp23Value", _
                                                 "ColProp24Value", _
                                                 "IsForExcel", _
                                                 "SubText", "ColAPU11Value", "ColAPU12Value", "ColAPU13Value", "ColAPU14Value", "ColAPU11Label", _
                                                "ColAPU11Diff", "ColAPU11DiffInDecimal", "ColAPU11DiffInInteger", "ColAPU11DiffPeriodID", _
                                                 "ColAPU11DiffPeriodUnitID", "ColAPU11FinalInInteger", "ColAPU12Label", "ColAPU12Diff", "ColAPU12DiffInDecimal", _
                                                "ColAPU12DiffInInteger", "ColAPU12DiffPeriodID", "ColAPU12DiffPeriodUnitID", "ColAPU12FinalInInteger"
                                    }

        For i As Integer = 0 To columnToRemove.Length - 1
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Contains(columnToRemove(i)) Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Remove(columnToRemove(i))
            End If
        Next

        If mDailyStatusLogReport.Count > 0 Then
            If mDailyStatusLogReport.Item(0).IsUTC Then
                columnToRemoveUTC = {"DepartureTime", "ArrivalTime"}

                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("DepartureUTCTime").SetOrdinal(6)
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ArrivalUTCTime").SetOrdinal(7)
            Else
                columnToRemoveUTC = {"DepartureUTCTime", "ArrivalUTCTime"}

                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("DepartureTime").SetOrdinal(6)
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ArrivalTime").SetOrdinal(7)
            End If

            For i As Integer = 0 To columnToRemoveUTC.Length - 1
                If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Contains(columnToRemoveUTC(i)) Then
                    dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Remove(columnToRemoveUTC(i))
                End If
            Next
        End If
        'Added by Vikrant On 27-Mar-2019 For StarAir27032019
        If Not AppSettings("ClientCode") = "STR" Then
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Contains("ColAPU11Final") Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Remove("ColAPU11Final")
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Contains("ColAPU12Final") Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Remove("ColAPU12Final")
            End If
        Else
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Contains("ColProp11Final") Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Remove("ColProp11Final")
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Contains("ColProp21Final") Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Remove("ColProp21Final")
            End If
        End If
        'End


        'set Column Sequence
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("LogDate").SetOrdinal(0)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("LogNo").SetOrdinal(1)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("LogPageNo").SetOrdinal(2)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("DepartureFrom").SetOrdinal(3)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ArrivalTo").SetOrdinal(4)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("DefectCount").SetOrdinal(5)

        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("BlockTime").SetOrdinal(8)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("TimeInAir").SetOrdinal(8)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("Col1Final").SetOrdinal(10)

        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("Col2Diff").SetOrdinal(11)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("Col2Final").SetOrdinal(12)

        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColEng11Final").SetOrdinal(13)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColEng12Diff").SetOrdinal(14)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColEng12Final").SetOrdinal(15)



        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColEng21Final").SetOrdinal(17)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColEng22Diff").SetOrdinal(18)
        dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColEng22Final").SetOrdinal(19)

        'dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColProp21Final").SetOrdinal(20)


        For i As Integer = 0 To dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Count - 1
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Col1Final" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Total AFH"
            End If

            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Col2Diff" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "LDGS Daily"
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Col2Final" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "LDGS Total"
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColEng11Final" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Engine (1) " + SearchingCriteria.SearchStr6 + " TSN"
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColEng12Diff" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Engine (1) " + SearchingCriteria.SearchStr6 + " Daily Cyc."
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColEng12Final" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Engine (1) " + SearchingCriteria.SearchStr6 + " CSN"
            End If

           

            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColEng21Final" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Engine (2) " + SearchingCriteria.SearchStr7 + " TSN"
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColEng22Diff" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Engine (2) " + SearchingCriteria.SearchStr7 + " Daily Cyc."
            End If
            If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColEng22Final" Then
                dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Engine (2) " + SearchingCriteria.SearchStr7 + " CSN"
            End If

           

            'Added by Vikrant On 27-Mar-2019 For StarAir27032019
            If AppSettings("ClientCode") = "STR" Then
                If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColAPU11Final" Then
                    dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "APU TSN"
                End If
                If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColAPU12Final" Then
                    dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "APU CSN"
                End If
            Else
                'dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColProp11Final").SetOrdinal(16)
                'dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns("ColProp21Final").SetOrdinal(20)

                If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColProp11Final" Then
                    dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Propeller (1) " + SearchingCriteria.SearchStr8 + " TSN"
                End If
                If dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "ColProp21Final" Then
                    dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns(i).ColumnName = "Propeller (2) " + SearchingCriteria.SearchStr9 + " TSN"
                End If
            End If
            'End
        Next

        Dim columnToRemoveCriteria As String() = { _
                                              "ReportDate", _
                                              "ID", _
                                              "CompanyName", _
                                              "Address", _
                                              "Tel1", _
                                              "Tel2", _
                                              "Fax", _
                                              "Email", _
                                              "WebSite", _
                                              "ReportName", _
                                              "SearchStr4", _
                                              "SearchStr5", _
                                              "SearchStr6", _
                                              "SearchStr7", _
                                              "SearchStr8", _
                                              "SearchStr9", _
                                              "ProductVersion", _
                                              "SINote", _
                                              "CurrencyName", _
                                              "CurrencySymbol", _
                                              "SearchStr10", _
                                              "SearchStr11", _
                                              "SearchStr12", _
                                              "SearchStr13", _
                                              "SearchStr14", _
                                              "SearchStr15", _
                                              "SearchStr16", _
                                              "SearchStr17", _
                                              "SearchStr18", _
                                              "SearchStr19", _
                                              "SearchStr20", _
                                              "SearchStr21", _
                                              "SearchStr22", _
                                              "SearchStr23", _
                                              "SearchStr24", _
                                              "SearchStr25" _
                                          }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If dsDailyStatusLogReport.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                dsDailyStatusLogReport.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        For i As Integer = 0 To dsDailyStatusLogReport.Tables("ReportData").Columns.Count - 1
            If dsDailyStatusLogReport.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                dsDailyStatusLogReport.Tables("ReportData").Columns(i).ColumnName = "From Date"
            End If
            If dsDailyStatusLogReport.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                dsDailyStatusLogReport.Tables("ReportData").Columns(i).ColumnName = "To Date"
            End If
            If dsDailyStatusLogReport.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                dsDailyStatusLogReport.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
        Next

        Dim columnscnt As Integer = dsDailyStatusLogReport.Tables("DailyStatusLogReport").Columns.Count

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsDailyStatusLogReport.Tables("ReportData"))
        dsNew.Merge(dsDailyStatusLogReport.Tables("DailyStatusLogReport"))

        dsNew.Tables("DailyStatusLogReport").DefaultView.Sort = "Date"
        dsNew.Tables("DailyStatusLogReport").Columns("Date").ColumnMapping = MappingType.Hidden
        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("DailyStatusLogReport").TableName = "Daily Log Status"
        Session("DataTableToBeFormattedForExportToExcel") = "Daily Log Status"

        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
#End Region

#Region " Data Bindings "
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        upnlSearchCriteria.Update()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then

            'SetCombo()
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            ResetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub
        ' mDailyStatusLogReport = DailyStatusLogReport.GetDailyStatusLogReport(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbAircraft.SelectedValue.ToString)
        mDailyStatusLogReport = DailyStatusLogReport.GetDailyStatusLogReport(txtFromDate.Text.ToString, txtToDate.Text.ToString, cmbAircraft.SelectedValue.ToString)

        If mDailyStatusLogReport.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAuditFindings.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            SetReport(True)

        End If
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Dim email As Thread
        Try
            email = New Thread(Sub() SetReport(True))
            mIsPreview = False
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnByExcel_Click(sender As Object, e As System.EventArgs) Handles btnByExcel.Click

        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        If IsValid = True Then
            mIsExcel = True
            SetReport(, mIsExcel)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        setValues()
        upnlCurrentCriteria.Update()

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        ResetValues()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

End Class