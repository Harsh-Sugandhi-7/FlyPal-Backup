Public Class wfrptAircraftServiceabilityReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As Guid
    Dim Aircraft As String
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptAircraftServiceabilityReport_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text.Trim) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.Trim
        End If
        If Not IsDate(txtToDate.Text.Trim) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.Trim
        End If
        MachineID = New Guid(Request.Form("cmbAircraft").ToString)
        Aircraft = IIf(MachineID.Equals(Guid.Empty), "", mMachineNameValueList(MachineID).RegNo)
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", StartDate, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", EndDate, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblAircraft1.Text
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.Trim
        EndDate = txtToDate.Text.Trim
        MachineID = Guid.Empty
        Aircraft = ""
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim ReportName As String = ""
        Dim AirframeSerialNo As String = ""
        Dim AirframeModel As String = ""
        SetValues()

        Dim ReportStatusList As New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim mrptAircraftServiceabilityReport As rptAircraftServiceabilityReport
        Dim dsrptAircraftServiceability As New dsrptAircraftServiceability

        Dim mtmpMachineList As tmpMachineList
        If Not Aircraft = "(All)" Then
            mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, EndDate)
            For i As Integer = 0 To mtmpMachineList.Count - 1
                ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , Year(CDate(txtFromDate.Text.Trim)).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted

                If mtmpMachineList(i).TypeID = 1 Then
                    AirframeSerialNo = mtmpMachineList(i).SerialNo
                    AirframeModel = mtmpMachineList(i).ModelName
                End If
            Next
        End If

        If AppSettings("ClientCode") = "TSL" Then
            myReport = New crrptAircraftServiceabilityReportTSL      'Added by Shital on 16-Dec-2021 for TSL
        Else
            myReport = New crrptAircraftServiceabilityReport
        End If


        mrptAircraftServiceabilityReport = rptAircraftServiceabilityReport.GetAircraftServiceabilityReport(StartDate, EndDate, , MachineID.ToString)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "", StartDate, EndDate, mMachineNameValueList(MachineID).RegNo, AirframeModel, AirframeSerialNo, AppSettings("Product Version"), AppSettings("SINote"), AirframeModel + " (" + AirframeSerialNo + ")", "", "", "", AppSettings("Logo"))

        If mrptAircraftServiceabilityReport.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not IsExcel Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1260)
        End If

        If IsExcel Then
            da.Fill(dsrptAircraftServiceability, "ExcelrptAircraftServiceabilityReport", mrptAircraftServiceabilityReport)
            da.Fill(dsrptAircraftServiceability, "ReportData", Report)
            Dim columnToRemove2 As String() = {"ID", "ShortName", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "SearchStr4", "SearchStr5", "ProductVersion", "SINote", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsrptAircraftServiceability.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    dsrptAircraftServiceability.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"ID", "Year", "HourType", "RegNo", "S_Status", "SM_Status", "USM_Status", "ModelName", "SrNo", "LogText", "LogNo", "LogPageNo", "FlyingLandings", "FlyingCycles"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If dsrptAircraftServiceability.Tables("ExcelrptAircraftServiceabilityReport").Columns.Contains(columnToRemove(i)) Then
                    dsrptAircraftServiceability.Tables("ExcelrptAircraftServiceabilityReport").Columns.Remove(columnToRemove(i))
                End If
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsrptAircraftServiceability.Tables("ReportData"))
            dsNew.Merge(dsrptAircraftServiceability.Tables("ExcelrptAircraftServiceabilityReport"))

            dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Reg No."
            dsNew.Tables("ReportData").Columns("SearchStr6").ColumnName = "Assembly"

            dsNew.Tables("ExcelrptAircraftServiceabilityReport").Columns("LogTextNo").ColumnName = "Log No."
            dsNew.Tables("ExcelrptAircraftServiceabilityReport").Columns("LogPageNoFormatted").ColumnName = "Log Page No."
            dsNew.Tables("ExcelrptAircraftServiceabilityReport").Columns("NoOfFlights").ColumnName = "No. of Log Pages"
            dsNew.Tables("ExcelrptAircraftServiceabilityReport").Columns("FlyingHours").ColumnName = "Flying Hours"

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptAircraftServiceabilityReport").TableName = "Aircraft Serviceability Status Report"

            
            Session("DataTableToBeFormattedForExportToExcel") = "Aircraft Serviceability Status Report"
			Session("ExcelFileName") = "Aircraft Serviceability Status Report"
			Session("dsNew") = dsNew

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "AircraftStatus", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
            ResetValues()
        Else
            Dim mrptImage As rptImage = rptImage.GetImage(dsrptAircraftServiceability)
            da.Fill(dsrptAircraftServiceability, mrptAircraftServiceabilityReport)
            da.Fill(dsrptAircraftServiceability, Report)
            da.Fill(dsrptAircraftServiceability, ReportStatusList)
            da.Fill(dsrptAircraftServiceability, mrptImage)
            myReport.SetDataSource(dsrptAircraftServiceability)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "AircraftStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            ResetValues()
        End If

        
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
#End Region

#Region "events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptAircraftServiceabilityReport_Ajax.aspx"
            ResetValues()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport(False)
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        Session.Remove("mMachineNameValueList")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        End If
    End Sub
#End Region


End Class