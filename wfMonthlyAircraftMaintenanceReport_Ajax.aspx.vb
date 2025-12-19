Imports System.Collections.Generic

Public Class wfMonthlyAircraftMaintenanceReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private mMachineNameValueList As MachineNameValueList
    Private mMonthlyMaintenanceReport As New MonthlyMaintenanceReport

    Dim mMachineList As MachineList

    Dim EventLogDetail As String = String.Empty
    Dim EventLogID As Guid = Guid.Empty

    Public mATAList As ATAList 'Added by Saylee on 27-Jun-2016
    Dim RecordAsOf As String

    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 16-08-2023
    Dim AMPNo As String = ""
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mATAList") = mATAList
    End Sub
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
        mATAList = CType(Session("mATAList"), ATAList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If
        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
    End Sub
    Private Sub DataFieldBinding()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        cmbMachine.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        'cmbMachine.DataBind()

        'Added by Saylee on 27-jun-2016
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        'cmbATAChapter.DataBind()
        '***************************
        DataBind()
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        lblModel1.Text = "Aircraft : " & IIf(cmbMachine.SelectedIndex > 0, cmbMachine.SelectedItem.Text, "")
        EventLogDetail = lblyear1.Text + ";" + lblModel1.Text
    End Sub
    Public Function GetAircraftFlownValue(ByVal MachineID As String, Optional ByVal Year As Integer = 1900, Optional ByVal MonthInt As Integer = 0) As String

        Dim FlyingHrs As String = String.Empty
        Dim FlyingHrsInString As String = String.Empty

        Dim mrptAircraftMonthlyFlyingListHRS As rptAircraftMonthlyFlyingList
        mrptAircraftMonthlyFlyingListHRS = rptAircraftMonthlyFlyingList.GetrptAircraftMonthlyFlyingList(Year, MachineID, 1)

        Dim mrptAircraftMonthlyFlyingListCYC As rptAircraftMonthlyFlyingList
        mrptAircraftMonthlyFlyingListCYC = rptAircraftMonthlyFlyingList.GetrptAircraftMonthlyFlyingList(Year, MachineID, 3)
        Select Case MonthInt
            Case 1
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).JanFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).JanFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).JanFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).JanFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).JanFlyingHrsInString + " Cycles", "")
            Case 2
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).FebFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).FebFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).FebFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).FebFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).FebFlyingHrsInString + " Cycles", "")
            Case 3
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).MarFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).MarFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).MarFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).MarFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).MarFlyingHrsInString + " Cycles", "")
            Case 4
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).AprFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).AprFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).AprFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).AprFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).AprFlyingHrsInString + " Cycles", "")
            Case 5
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).MayFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).MayFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).MayFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).MayFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).MayFlyingHrsInString + " Cycles", "")
            Case 6
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).JunFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).JunFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).JunFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).JunFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).JunFlyingHrsInString + " Cycles", "")
            Case 7
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).JulFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).JulFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).JulFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).JulFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).JulFlyingHrsInString + " Cycles", "")
            Case 8
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).AugFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).AugFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).AugFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).AugFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).AugFlyingHrsInString + " Cycles", "")
            Case 9
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).SepFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).SepFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).SepFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).SepFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).SepFlyingHrsInString + " Cycles", "")
            Case 10
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).OctFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).OctFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).OctFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).OctFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).OctFlyingHrsInString + " Cycles", "")
            Case 11
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).NovFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).NovFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).NovFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).NovFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).NovFlyingHrsInString + " Cycles", "")
            Case 12
                FlyingHrs = mrptAircraftMonthlyFlyingListHRS(0).DecFlyingHrs
                FlyingHrsInString = IIf(mrptAircraftMonthlyFlyingListHRS(0).DecFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListHRS(0).DecFlyingHrsInString + " Hours", "") + vbCrLf + IIf(mrptAircraftMonthlyFlyingListCYC(0).DecFlyingHrsInString <> "", mrptAircraftMonthlyFlyingListCYC(0).DecFlyingHrsInString + " Cycles", "")
        End Select
        Return FlyingHrsInString
    End Function
    Private Sub SetExcel(mMonthlyMaintenanceReport As MonthlyMaintenanceReport, SearchingCriteria As ReportData, ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMonthlyMaintenanceReport

        '''Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)

        '''reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
        '''Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
        '''Select c).ToList
        '''Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
        '''Session("reportmaintdetailslist") = reportmaintdetailslist

        da.Fill(ds, "ExcelMonthlyMaintenanceReport", mMonthlyMaintenanceReport)
        da.Fill(ds, "ReportData", SearchingCriteria)

        Dim columnToRemove As String() = {"TSNValueEng1", "TSNValueEng2", "TSNValueProp1", "TSNValueProp2", "SinceOHEng1", "SinceOHEng2", "SinceOHProp1", "SinceOHProp2", "LogID", "TimeInAir", "TimeInAirInteger", "AssemblyID", "SerialNoPosition112", "SerialNoPosition122", "SerialNoPosition212", "SerialNoPosition222", "LogPageNo", "LogPageNoFormatted", "FlightNo", "IsLogNo", "IsLogPageNo", "IsFlightNo", "LogText", "LogNo", "LogTextNo", "ReferencedDocuments", "ReferencedDocumentsHeading", "HourType", "SrNo", "LicNo", "DoneBy", "Defect", "Rectification", "Item", "CAT", "LogDate"}

        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns.Remove(columnToRemove(i))
            End If
        Next

        'set Column Sequence
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SerialNoPosition111").SetOrdinal(0)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("TSNValueEng1Excel").SetOrdinal(1)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SinceOHEng1Excel").SetOrdinal(2)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SerialNoPosition121").SetOrdinal(3)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("TSNValueEng2Excel").SetOrdinal(4)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SinceOHEng2Excel").SetOrdinal(5)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SerialNoPosition211").SetOrdinal(6)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("TSNValueProp1Excel").SetOrdinal(7)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SinceOHProp1Excel").SetOrdinal(8)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SerialNoPosition221").SetOrdinal(9)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("TSNValueProp2Excel").SetOrdinal(10)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("SinceOHProp2Excel").SetOrdinal(11)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("Date").SetOrdinal(12)
        ds.Tables("ExcelMonthlyMaintenanceReport").Columns("Description").SetOrdinal(13)

        For i As Integer = 0 To ds.Tables("ExcelMonthlyMaintenanceReport").Columns.Count - 1
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SerialNoPosition111" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Eng1 Serial No.(Position)"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SerialNoPosition121" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Eng2 Serial No.(Position)"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SerialNoPosition211" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Propeller1 Serial No.(Position)"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SerialNoPosition221" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Propeller2 Serial No.(Position)"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SinceOHEng1Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Eng1 TSO"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SinceOHEng2Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Eng2 TSO"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SinceOHProp1Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Propeller1 TSO"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "SinceOHProp2Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Propeller2 TSO"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "TSNValueEng1Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Eng1 TSN"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "TSNValueEng2Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Eng2 TSN"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "TSNValueProp1Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Propeller1 TSN"
            End If
            If ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "TSNValueProp2Excel" Then
                ds.Tables("ExcelMonthlyMaintenanceReport").Columns(i).ColumnName = "Propeller2 TSN"
            End If
        Next
        Dim columnToRemoveCriteria As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr9", "SearchStr10", "ShortName", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        'set Column Sequence
        ds.Tables("ReportData").Columns("ReportName").SetOrdinal(0)
        ds.Tables("ReportData").Columns("ReportDate").SetOrdinal(1)
        ds.Tables("ReportData").Columns("SearchStr1").SetOrdinal(2)
        ds.Tables("ReportData").Columns("SearchStr2").SetOrdinal(3)
        ds.Tables("ReportData").Columns("SearchStr4").SetOrdinal(4)
        ds.Tables("ReportData").Columns("SearchStr5").SetOrdinal(5)
        ds.Tables("ReportData").Columns("SearchStr6").SetOrdinal(6)
        ds.Tables("ReportData").Columns("SearchStr7").SetOrdinal(7)
        ds.Tables("ReportData").Columns("SearchStr8").SetOrdinal(8)
        ds.Tables("ReportData").Columns("SearchStr3").SetOrdinal(9)
        ds.Tables("ReportData").Columns("SearchStr11").SetOrdinal(10)
        ds.Tables("ReportData").Columns("SearchStr12").SetOrdinal(11)
        ds.Tables("ReportData").Columns("SearchStr13").SetOrdinal(12)
        ds.Tables("ReportData").Columns("SearchStr14").SetOrdinal(13)
        ds.Tables("ReportData").Columns("SearchStr15").SetOrdinal(14)


        For i As Integer = 0 To ds.Tables("ReportData").Columns.Count - 1
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Month"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Year"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Aircraft Flown Value"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Aircraft Serial No."
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr6" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Aircraft Type"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr7" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Aircraft TSN"
            End If

            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr8" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Aircraft CSN"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr11" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "ATA Chapter"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr12" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Show Compliance"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr13" Then
                ds.Tables("ReportData").Columns(i).ColumnName = IIf(AppSettings("MELSnagNomenclature") = "True", "Show Pireps/ADD/Defect", "Show Pireps/MEL/Snag")
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr14" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Show Maintenance Activity"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr15" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Show Install/Removal"
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Merge(ds.Tables("ExcelMonthlyMaintenanceReport"))


        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("ExcelMonthlyMaintenanceReport").TableName = ReportName
        Session("DataTableToBeFormattedForExportToExcel") = ReportName
		Session("ExcelFileName") = ReportName
		PeriodColumnsForExportToExcel.AddRange(New String() {"Eng1 TSO", "Eng2 TSO", "Propeller1 TSO", "Propeller2 TSO", "Eng1 TSN", "Eng2 TSN", "Propeller1 TSN", "Propeller2 TSN"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "MonthlyMaintenanceReport", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsMonthlyMaintenanceReport
        Dim AircraftFlownValue As String = String.Empty
        Dim myReport = New crMaintenanceActivityReport
        Dim mMachineList As MachineList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim StartDateM As New SmartDate
        Dim EndDateM As New SmartDate
        Dim RegNo, SerialNo, AssemblyType, HrsTSNValue, CYCTSNValue As String
        Dim Periodcount As Integer

        If chkMonth.Checked Then
            mMonthlyMaintenanceReport = MonthlyMaintenanceReport.GetMonthlyMaintenanceReport(cmbMachine.SelectedValue.ToString, , _
                                                                                             CInt(cmbYear.SelectedItem.ToString), _
                                                                                             cmbMonth.SelectedIndex + 1, chkShowCompliance.Checked, _
                                                                                             chkShowPirepsMELSnag.Checked, chkInstallRemoval.Checked, _
                                                                                             chkShowMaintActivity.Checked, _
                                                                                             cmbATAChapter.SelectedValue.ToString, IsByDate:=False)
            ReportName = "Monthly Maintenance Report- " + StrConv((cmbMonth.SelectedItem.Text).Substring(0, 3), vbUpperCase) + " " + cmbYear.SelectedItem.Text
        Else
            mMonthlyMaintenanceReport = MonthlyMaintenanceReport.GetMonthlyMaintenanceReport(cmbMachine.SelectedValue.ToString, , _
                                                                                             CInt(cmbYear.SelectedItem.ToString), _
                                                                                             cmbMonth.SelectedIndex + 1, chkShowCompliance.Checked, _
                                                                                             chkShowPirepsMELSnag.Checked, chkInstallRemoval.Checked, _
                                                                                             chkShowMaintActivity.Checked, _
                                                                                             cmbATAChapter.SelectedValue.ToString, IsByDate:=True, _
                                                                                             FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text)
            ReportName = "Maintenance Report From " + New SmartDate(txtFromDate.Text).FormattedText + " To " + New SmartDate(txtToDate.Text).FormattedText
        End If

        If mMonthlyMaintenanceReport.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1285)
        End If
        If chkMonth.Checked Then
            StartDateM = New SmartDate(CStr(DateAdd(DateInterval.Month, (cmbMonth.SelectedIndex + 1) - 1, DateSerial(CInt(cmbYear.SelectedItem.ToString), 1, 1))))
            EndDateM = New SmartDate(CStr(DateAdd("d", -1, DateAdd("m", 1, StartDateM.Date))))
            RecordAsOf = CType(Day(CDate(EndDateM.ToString)), String) + " " + StrConv((cmbMonth.SelectedItem.Text).Substring(0, 3), vbUpperCase) + " " + cmbYear.SelectedItem.Text
        Else
            StartDateM = New SmartDate(txtFromDate.Text)
            EndDateM = New SmartDate(txtToDate.Text)
            RecordAsOf = New SmartDate(txtToDate.Text).FormattedText
        End If

        SetValues()
        AircraftFlownValue = GetAircraftFlownValue(cmbMachine.SelectedValue.ToString, CInt(cmbYear.SelectedItem.ToString), cmbMonth.SelectedIndex + 1)

        mMachineList = MachineList.GetMachineListMonitoringStatus(EndDateM.Text, cmbMachine.SelectedValue.ToString, , , , , , , , , , , True, , , "Airframe", SkipIsForInventoryAircarft:=True)
        HrsTSNValue = ""
        CYCTSNValue = ""
        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                If ObjAssemblyStatus.AssemblyTypeID = 1 Then
                    Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                    For Count1 As Integer = 0 To Periodcount - 1
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID = 1 Then
                            HrsTSNValue = CType(IIf(HrsTSNValue = "", HrsTSNValue, HrsTSNValue + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyCurrentValue
                        ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID = 3 Then
                            CYCTSNValue = CType(IIf(CYCTSNValue = "", CYCTSNValue, CYCTSNValue + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyCurrentValue
                        End If
                    Next
                    RegNo = cmbMachine.SelectedItem.Text
                    SerialNo = ObjAssemblyStatus.SerialNo
                    AssemblyType = ObjAssemblyStatus.Model 'ObjAssemblyStatus.AssemblyType
                End If
            Next
        Next

        'Added by Ajay 14-08-2023
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            If cmbMachine.SelectedIndex > 0 Then
                mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(MachineID:=New Guid(cmbMachine.SelectedValue.ToLower))
                Session("mLastAMPRef") = mLastAMPRef
                If (mLastAMPRef.AMPNo <> "") Then AMPNo = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
            Else
                AMPNo = ""
            End If
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                 mCompanyDetail.WebSite, ReportName, StrConv((cmbMonth.SelectedItem.Text).Substring(0, 3), vbUpperCase),
                 cmbYear.SelectedItem.Text, IIf(IsExcel, AircraftFlownValue.Replace(vbCrLf, Chr(10)), AircraftFlownValue), RegNo, SerialNo,
                  AppSettings("Product Version"), AppSettings("SINote"), AssemblyType, HrsTSNValue, CYCTSNValue, RecordAsOf, AppSettings("Logo"),
                 cmbATAChapter.SelectedItem.ToString, IIf(chkShowCompliance.Checked, "Yes", "No"), IIf(chkShowPirepsMELSnag.Checked, "Yes", "No"),
                 IIf(chkShowMaintActivity.Checked, "Yes", "No"), IIf(chkInstallRemoval.Checked, "Yes", "No"), SearchStr16:=AppSettings("ClientCode"), SearchStr17:=AppSettings("MELSnagNomenclature").ToString, SearchStr18:=AMPNo)

        If IsExcel Then
            SetExcel(mMonthlyMaintenanceReport, Report, ReportName)
        Else
            Dim mrptImage As rptImage = rptImage.GetImage(ds)

            da.Fill(ds, mMonthlyMaintenanceReport)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str1 As String
            Str1 = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
            MarkLog(Util.Action.Print, "MonthlyMaintenanceReport", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub ControlVisibility()
        If chkMonth.Checked = True Then
            cmbMonth.Enabled = True
            cmbYear.Enabled = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            cmbMonth.Enabled = False
            cmbYear.Enabled = False
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            'txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            'txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            SetCombo()
            DataFieldBinding()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            If chkShowCompliance.Checked = False And chkInstallRemoval.Checked = False And chkShowMaintActivity.Checked = False And chkShowPirepsMELSnag.Checked = False Then
                MSGBoxCtrl.show("Selection Alert!", "Select atleast one Maintenance Activity.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnByExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnByExcel.Click
        If IsValid Then

            If chkShowCompliance.Checked = False And chkInstallRemoval.Checked = False And chkShowMaintActivity.Checked = False And chkShowPirepsMELSnag.Checked = False Then
                MSGBoxCtrl.show("Selection Alert!", "Select atleast one Maintenance Activity.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If


            SetReport(True)
        End If
    End Sub
    Private Sub chkMonth_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMonth.CheckedChanged
        If chkMonth.Checked = True Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            txtFromDate.Text = ""
            txtToDate.Text = ""
            cmbMonth.Enabled = True
            cmbYear.Enabled = True
        Else
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            cmbMonth.Enabled = False
            cmbYear.Enabled = False
        End If
        upnlMonth.Update()
    End Sub
    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        If chkDate.Checked = True Then
            cmbMonth.Enabled = False
            cmbYear.Enabled = False
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        Else
            cmbMonth.Enabled = True
            cmbYear.Enabled = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            txtFromDate.Text = ""
            txtToDate.Text = ""
        End If
        upnlMonth.Update()
    End Sub
#End Region

End Class