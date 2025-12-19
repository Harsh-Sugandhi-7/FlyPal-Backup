'Added by Utkarsh on 29-Jan-2014

Public Class wfSearchCriteriaForFuelAndOil_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ReportStatusList As New rptStatusList
    Dim mMachineNameValueList As MachineNameValueList
    Dim mMachineNameValueListForAircraftWiseSummary As MachineNameValueList
    Dim StartDate As String
    Dim EndDate As String
    Dim Engine As String
    Dim MachineName As String
    Dim MachineID As Guid
    Dim Aircraft As String
    Dim EventLogDetail As String
    Dim mCompanyDetail As New CompanyDetail
    Dim dsFuelOil As New dsFuelOilRegister
    Dim objFuelOil As ReportFuelandOilRegister
    Dim dsFuelOilSummary As New dsFuelOilAircraftWiseSummaryRegister
    Dim objFuelOilSummary As ReportAircraftWiseFuelandOilSummaryRegister
    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mTankList As TankList
    Private mAssemblyList As AssemblyList 'APFT
    Dim mFAScsReportList As FAScsReportList
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mMachineNameValueListForAircraftWiseSummary = Session("mMachineNameValueListForAircraftWiseSummary")
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList) 'APFT
        mFAScsReportList = Session("mFAScsReportList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForFuelAndOil_Ajax.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblyList") 'APFT
            Session.Remove("mFAScsReportList")
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

        If chkForAircraftWiseSummary.Checked = False Then
            MachineID = New Guid(Request.Form("cmbAircraft").ToString)
            Aircraft = IIf(MachineID.Equals(Guid.Empty), "", mMachineNameValueList(MachineID).RegNo)
        Else
            Aircraft = String.Empty
            For i As Integer = 0 To ChklistAircraftWiseSummary.Items.Count - 1
                If ChklistAircraftWiseSummary.Items(i).Selected Then
                    If Aircraft.Length = 0 Then
                        Aircraft = ChklistAircraftWiseSummary.Items(i).Text
                    Else
                        Aircraft = Aircraft + ", " + ChklistAircraftWiseSummary.Items(i).Text
                    End If
                End If
            Next
        End If

        If StartDate <> "" Then
            lblDateRangeFrom.Text = "From Date : " & New SmartDate(StartDate).FormattedText
        Else
            lblDateRangeFrom.Text = "From Date : "
        End If
        If EndDate <> "" Then
            lblDateRangeTo.Text = "To Date : " & New SmartDate(EndDate).FormattedText
        Else
            lblDateRangeTo.Text = "To Date : "
        End If
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblAircraft1.Text
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.Trim
        EndDate = txtToDate.Text.Trim
        MachineID = Guid.Empty
        Engine = ""
        Aircraft = ""
    End Sub
    Private Sub SetAircraftWiseSummaryReport(Optional ByMail As Boolean = False)
        SetValues()
        dsFuelOilSummary = New dsFuelOilAircraftWiseSummaryRegister
        If cmbFormat.SelectedIndex = 0 Then
            myReport = New crFuelAndOilAircraftWiseSummery
        Else
            myReport = New crFuelAndOilAircraftWiseSummeryFormat2 'Added by Saylee on 13-Aug-2019 for APFT13082019
        End If

        objFuelOilSummary = ReportAircraftWiseFuelandOilSummaryRegister.GetAircraftWiseFuelOilSummaryRegisterList(Aircraft.ToString, StartDate, EndDate, Guid.Empty.ToString, False, "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                   mCompanyDetail.WebSite, "Aircraft wise Fuel Uplifted Summary", txtFromDate.Text.Trim, txtToDate.Text.Trim, Aircraft, cmbUnitList.SelectedItem.Text, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If objFuelOilSummary.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 913)
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(dsFuelOilSummary)
        da.Fill(dsFuelOilSummary, mrptImage)
        da.Fill(dsFuelOilSummary, Report)
        da.Fill(dsFuelOilSummary, objFuelOilSummary)

        myReport.SetDataSource(dsFuelOilSummary)

        Session("CrystalReport") = myReport

        If ByMail = True Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Aircraft wise Fuel Uplifted Summary", "Aircraft wise Fuel Uplifted Summary", lblDateRangeFrom.Text + " " + lblDateRangeTo.Text + ", " + lblAircraft1.Text,
                                    "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                                     SmtpHost:=mModuleList.Item("FuelAndOilRegister").SmtpHost, SmtpPort:=mModuleList.Item("FuelAndOilRegister").SmtpPort,
                                     SmtpUser:=mModuleList.Item("FuelAndOilRegister").SmtpUser, SmtpPassword:=mModuleList.Item("FuelAndOilRegister").SmtpPassword)

        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If

        MarkLog(Util.Action.Print, "FuelAndOilRegister", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetReport(Optional ByMail As Boolean = False)
        SetValues()
        Dim OperatorName As String = ""

        If cmbFormat.SelectedIndex = 0 Then
            myReport = New crFuelAndOilSummery 'crFuelOil
        ElseIf cmbFormat.SelectedIndex = 1 Then
            myReport = New crFuelAndOilSummeryAPFT 'For APFT Client
        End If

        objFuelOil = ReportFuelandOilRegister.GetFuelOilRegisterList(MachineID.ToString, StartDate, EndDate, cmbAssembly.SelectedValue.ToString, True, cmbTankList.SelectedValue.ToString)   'cmbEngine.SelectedValue.ToString)

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(MachineID)
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        End If



        '    Dim mtmpMachineList As tmpMachineList
        ' mtmpMachineList = tmpMachineList.GetMachineList(, cmbAircraft.SelectedItem.Text, , , , , True, EndDate)
        'For i As Integer = 0 To mtmpMachineList.Count - 1
        '    If mtmpMachineList(i).TypeID = 2 Then
        '        ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , mtmpMachineList(i).TSO, _
        '                          , mtmpMachineList(i).CSO, , , , , , , , , mtmpMachineList(i).Cycles, _
        '                          , mtmpMachineList(i).AllPeriods, Year(txtFromDate.Text).ToString, , _
        '                          mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, _
        '                          mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, _
        '                          mtmpMachineList(i).Landings, mtmpMachineList(i).Postion))
        '    End If
        'Next
        Dim mAircraftCurrValue As AircraftCurrentStatusList = AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(, cmbAircraft.SelectedItem.Text, , , , EndDate.ToString)
        For i As Integer = 0 To mAircraftCurrValue.Count - 1
            If mAircraftCurrValue(i).TypeID = 2 Or mAircraftCurrValue(i).TypeID = 4 Then
                ReportStatusList.Add(New rptStatus(mAircraftCurrValue(i).ID.ToString,
                                                   GroupType:=1, , , , ,
                                                   mAircraftCurrValue(i).TSO,
                                                 , mAircraftCurrValue(i).CSO, , , , , , , , ,
                                                   mAircraftCurrValue(i).Cycles,
                                                 , mAircraftCurrValue(i).AllPeriods,
                                                   Year(txtFromDate.Text).ToString, ,
                                                   mAircraftCurrValue(i).RegNo,
                                                   mAircraftCurrValue(i).ModelName,
                                                   mAircraftCurrValue(i).Type, mAircraftCurrValue(i).SerialNo,
                                                   mAircraftCurrValue(i).ManufacturerName, ,
                                                   mAircraftCurrValue(i).ManufacturingDate,
                                                   mAircraftCurrValue(i).Hours,
                                                   mAircraftCurrValue(i).Landings,
                                                   mAircraftCurrValue(i).Position, RHData7:=mAircraftCurrValue(i).Type))
            End If
        Next
        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                     mCompanyDetail.Address,
                                     mCompanyDetail.Tel1,
                                     mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax,
                                     mCompanyDetail.Email,
                                     mCompanyDetail.WebSite,
                                     IIf(cmbFormat.SelectedIndex = 0, "Fuel And Oil Register", "Fuel Oil Consumption Card"),
                                     txtFromDate.Text.Trim,
                                     txtToDate.Text.Trim,
                                     mMachineNameValueList(MachineID).RegNo,
                                     mAircraftCurrValue(0).AllPeriods,
                                     IIf(cmbAssembly.SelectedIndex > 0, cmbAssembly.SelectedItem.ToString, cmbTankList.SelectedItem.Text),
                                     AppSettings("Product Version"),
                                     AppSettings("SINote"),
                                     AppSettings("ClientCode"),
                                     OperatorName,
                                     SearchStr8:="",
                                     "",
                                     AppSettings("Logo"))

        If objFuelOil.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 913)
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(dsFuelOil) 'Added by Shweta on 22-Feb-2012
        da.Fill(dsFuelOil, objFuelOil)
        da.Fill(dsFuelOil, mrptImage) 'Added by Shweta on 22-Feb-2012
        da.Fill(dsFuelOil, Report)
        da.Fill(dsFuelOil, ReportStatusList)
        myReport.SetDataSource(dsFuelOil)

        Session("CrystalReport") = myReport

        If ByMail Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, IIf(cmbFormat.SelectedIndex = 0, "Fuel And Oil Register", "Fuel Oil Consumption Card"), IIf(cmbFormat.SelectedIndex = 0, "Fuel And Oil Register", "Fuel Oil Consumption Card"), lblDateRangeFrom.Text + " " + lblDateRangeTo.Text + ", " + lblAircraft1.Text,
                                      "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                                     SmtpHost:=mModuleList.Item("FuelAndOilRegister").SmtpHost, SmtpPort:=mModuleList.Item("FuelAndOilRegister").SmtpPort,
                                     SmtpUser:=mModuleList.Item("FuelAndOilRegister").SmtpUser, SmtpPassword:=mModuleList.Item("FuelAndOilRegister").SmtpPassword)

        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If

        MarkLog(Util.Action.Print, "FuelAndOilRegister", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'ResetValues()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , 0, 0, "", "", "", True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        If chkForAircraftWiseSummary.Checked Then
            ChklistAircraftWiseSummary.DataSource = mMachineNameValueList
            ChklistAircraftWiseSummary.DataBind()
            upnlAircraftWiseSummary.Update()
        End If
        mFAScsReportList = FAScsReportList.GetFAScsReportList()
        Session("mFAScsReportList") = mFAScsReportList
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForFuelAndOil_Ajax.aspx?"
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
    Private Sub btnMail_Click(sender As Object, e As System.EventArgs) Handles btnMail.Click
        Dim Str As String
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("FuelAndOilRegister").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("FuelAndOilRegister").SendCCMailID
        '--------------------------
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Dim email As Thread
        Try
            If chkForAircraftWiseSummary.Checked = False Then
                SetReport(True)
            Else
                SetAircraftWiseSummaryReport(True)
            End If

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
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            If chkForAircraftWiseSummary.Checked = False Then
                SetReport()
            Else
                SetAircraftWiseSummaryReport()
            End If

        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Added by Abhishek on 20-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            Dim OperatorName As String = ""
            myReport = New crFuelAndOilSummery 'crFuelOil
            Dim UnitName As String = ""
            objFuelOil = ReportFuelandOilRegister.GetFuelOilRegisterList(MachineID.ToString, StartDate, EndDate, cmbAssembly.SelectedValue.ToString, True, , True)   'cmbEngine.SelectedValue.ToString)

            'Added by Prashant on 11-Aug-2011
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(MachineID)
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            End If

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Fuel And Oil Register", txtFromDate.Text.Trim, txtToDate.Text.Trim, _
        mMachineNameValueList(MachineID).RegNo, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"))

            If objFuelOil.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 913)
                UnitName = objFuelOil(0).UnitName
            End If
            Dim mrptImage As rptImage = rptImage.GetImage(dsFuelOil) 'Added by Shweta on 22-Feb-2012
            da.Fill(dsFuelOil, "ExclReportFuelandOilRegister", objFuelOil)
            da.Fill(dsFuelOil, mrptImage) 'Added by Shweta on 22-Feb-2012
            da.Fill(dsFuelOil, Report)
            Dim columnToRemove1 As String() = {"ID", "SrNo", "TotFlyingHrs", "TotFlyingHrsStr", "TotFuelOilLifted", "TotOilLifted", "TotConsumed", "TotAvgFuelOilConsumption", "UnitName", "TotalBlockTimeStr", "LogText", "LogNo", "LogTypeID", "TimeInAir", "From", "To", "GroupBy", "Heading", "Model", "SerialNo", "FlyingHrs", "AvgUpLifted", "BlockTime", "TotalBlockTime", "TOWeight", "Altitude", "Remark", "RegNo", "Pilot1Name", "BurnOnGround", "WOFuelUplifted", "WOFuelDrainedOut", "ExcelToWeightAltitude", "TimeOnGround", "BlockTimeForReport", "TotalBlockTimeForReport", "TotalBlockTimeForReportStr", "TotalFuelUplift", "AvgFuelConsumption", "AvgOilConsumption", "TotFuelatDept", "TotFuelatArrival", "TotBurnOnGround", "TotTotal", "TotTimeOnGround", "TotTotalBlockTimeForReport", "TotTotalBlockTimeForReportStr", "FuelUpliftTotal"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains(columnToRemove1(i)) Then
                    dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr4", "SearchStr5", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "CurrencyName", "CurrencySymbol"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsFuelOil.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    dsFuelOil.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If dsFuelOil.Tables("ReportData").Columns.Contains("SearchStr1") Then
                dsFuelOil.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date "
            End If

            If dsFuelOil.Tables("ReportData").Columns.Contains("SearchStr2") Then
                dsFuelOil.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If dsFuelOil.Tables("ReportData").Columns.Contains("SearchStr3") Then
                dsFuelOil.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
            End If


            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("Date") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("Date").ColumnName = "Date"
            End If

            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("LogTextNo") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("LogTextNo").ColumnName = "LogTextNo"
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("LogPageNo") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("LogPageNo").ColumnName = "LogPageNo"
            End If
            'If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("Pilot1Name") Then
            '    dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("Pilot1Name").ColumnName = "Crew"
            'End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("FlightLogClassificationName") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("FlightLogClassificationName").ColumnName = "Classification"
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("ExcelDepartureArrival") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("ExcelDepartureArrival").ColumnName = "Departure/Arrival"
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("FlyingHrsStr") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("FlyingHrsStr").ColumnName = "Total Time"
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("BlockTimeStr") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("BlockTimeStr").ColumnName = "Block Time"
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("OilUplifted") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("OilUplifted").ColumnName = "Oil Uplift"
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("FuelatDept") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("FuelatDept").ColumnName = "Fuel on Board" + IIf(UnitName <> "", " (" + UnitName + ")", "")
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("FuelUplifted") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("FuelUplifted").ColumnName = "Fuel Uplift" + IIf(UnitName <> "", " (" + UnitName + ")", "")
            End If
            'If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("BurnOnGround") Then
            '    dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("BurnOnGround").ColumnName = "Burn On Ground (litre)"
            'End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("Total") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("Total").ColumnName = "Total Fuel At Departure" + IIf(UnitName <> "", " (" + UnitName + ")", "")
            End If
            'If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("WOFuelUplifted") Then
            '    dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("WOFuelUplifted").ColumnName = "WO Fuel Uplifted"
            'End If
            'If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("WOFuelDrainedOut") Then
            '    dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("WOFuelDrainedOut").ColumnName = "WO Fuel DrainedOut"
            'End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("FuelatArrive") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("FuelatArrive").ColumnName = "Fuel at Arrival" + IIf(UnitName <> "", " (" + UnitName + ")", "")
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("Consumed") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("Consumed").ColumnName = "Fuel Used" + IIf(UnitName <> "", " (" + UnitName + ")", "")
            End If
            If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("AvgFuelOilConsumption") Then
                dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("AvgFuelOilConsumption").ColumnName = "Avg con./Hr" + IIf(UnitName <> "", " (" + UnitName + ")", "")
            End If
            'If dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns.Contains("ExcelToWeightAltitude") Then
            '    dsFuelOil.Tables("ExclReportFuelandOilRegister").Columns("ExcelToWeightAltitude").ColumnName = "T.O Weight/Altitude"
            'End If
            If objFuelOil.Count = 0 Then
                'Do nothing 
            Else
                DirectCast(dsFuelOil.Tables("ExclReportFuelandOilRegister").Rows(objFuelOil.Count - 1), Flypal.dsFuelOilRegister.ExclReportFuelandOilRegisterRow).AvgFuelOilConsumption = ""
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsFuelOil.Tables("ReportData"))
            dsNew.Merge(dsFuelOil.Tables("ExclReportFuelandOilRegister"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExclReportFuelandOilRegister").TableName = "Fuel and Oil Register"
			Session("ExcelFileName") = "Fuel and Oil Register"
			Session("dsNew") = dsNew

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "FuelAndOilRegister", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbAircraft.SelectedIndex <> 0 And cmbFormat.SelectedIndex = 1 And Not chkForAircraftWiseSummary.Checked Then
            lbltank.Visible = True
            cmbTankList.Visible = True
            'Added For APFT
            ' mAssemblyList = AssemblyList.GetAssemblyListForComboBox(2, cmbAircraft.SelectedValue, txtFromDate.Text, "(All)", True)
            mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text, "(All)", True, ShowEngineAndAPUOnly:=True)
            Session("mAssemblyList") = mAssemblyList
            cmbAssembly.DataSource = mAssemblyList
            cmbAssembly.DataBind()
            cmbAssembly.ClearSelection()
            cmbTankList.ClearSelection()
            cmbTankList.Enabled = True
            lblAssembly.Visible = True
            cmbAssembly.Visible = True
            cmbAssembly.Enabled = True
            'End
        Else
            lbltank.Visible = False
            cmbTankList.Visible = False
            'Added For APFT
            cmbTankList.ClearSelection()
            cmbAssembly.ClearSelection()
            lblAssembly.Visible = False
            cmbAssembly.Visible = False
            'End
        End If
        upnlTank.Update()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex <> 0 Then
            mTankList = TankList.GetTankList(New Guid(cmbAircraft.SelectedValue), "(All)")
            cmbTankList.DataSource = mTankList
            cmbTankList.DataBind()
            'APFT
            ' mAssemblyList = AssemblyList.GetAssemblyListForComboBox(2, cmbAircraft.SelectedValue, txtFromDate.Text, "(All)", True)
            mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text, "(All)", True, ShowEngineAndAPUOnly:=True)
            Session("mAssemblyList") = mAssemblyList
            cmbAssembly.DataSource = mAssemblyList
            cmbAssembly.DataBind()
            'End
        End If
        If cmbAircraft.SelectedIndex <> 0 And cmbFormat.SelectedIndex = 1 Then
            lbltank.Visible = True
            cmbTankList.Visible = True
            'Added For APFT
            cmbAssembly.ClearSelection()
            cmbTankList.ClearSelection()
            lblAssembly.Visible = True
            cmbAssembly.Visible = True
            cmbAssembly.Enabled = True
            cmbTankList.Enabled = True
            'End
        Else
            lbltank.Visible = False
            cmbTankList.Visible = False
            'Added For APFT
            cmbAssembly.ClearSelection()
            cmbTankList.ClearSelection()
            lblAssembly.Visible = False
            cmbAssembly.Visible = False
            cmbAssembly.Enabled = False
            cmbTankList.Enabled = False
            'End
        End If
        upnlTank.Update()
    End Sub
    Private Sub chkForAircraftWiseSummary_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkForAircraftWiseSummary.CheckedChanged
        If chkForAircraftWiseSummary.Checked Then
            cmbAircraft.ClearSelection()
            cmbUnitList.DataSource = UnitListMain.GetUnitList("", "(SELECT)")
            cmbUnitList.DataBind()
            mMachineNameValueListForAircraftWiseSummary = MachineNameValueList.GetMachineList(Today.Date.ToString, , 0, 0, "", "", "", False, "", , True, , , , chkForAircraftWiseSummary.Checked, cmbUnitList.SelectedValue)
            Session("mMachineNameValueListForAircraftWiseSummary") = mMachineNameValueListForAircraftWiseSummary
            ChklistAircraftWiseSummary.DataSource = mMachineNameValueListForAircraftWiseSummary
            ChklistAircraftWiseSummary.DataBind()

            cmbAircraft.Enabled = False
            ChklistAircraftWiseSummary.Visible = True

            'cmbFormat.Enabled = False
            btnExport.Enabled = False
            upnlFormat.Update()
            upnlAircraftWiseSummary.Update()
            upnlAircraft.Update()
            upnlActionBtns.Update()
            cmbUnitList.Enabled = True
            'Added By Vikrant On 24-Aug-2018 For APFT
            cmbAssembly.ClearSelection()
            cmbTankList.ClearSelection()
            cmbAssembly.Enabled = False
            cmbTankList.Enabled = False
            'End
        Else
            'ChklistAircraftWiseSummary.DataSource = Nothing
            'ChklistAircraftWiseSummary.DataBind()
            ChklistAircraftWiseSummary.Visible = False
            chkSelectAll.Visible = False
            cmbFormat.Enabled = True
            btnExport.Enabled = True
            upnlFormat.Update()
            upnlAircraftWiseSummary.Update()
            upnlActionBtns.Update()
            cmbUnitList.ClearSelection()
            cmbAircraft.Enabled = True
            upnlAircraft.Update()
            cmbUnitList.DataSource = Nothing
            cmbUnitList.DataBind()
            cmbUnitList.Enabled = False
            'Added By Vikrant On 24-Aug-2018 For APFT
            cmbAssembly.ClearSelection()
            cmbTankList.ClearSelection()
            cmbAssembly.Enabled = IIf(cmbAircraft.SelectedIndex > 0, True, False)
            cmbTankList.Enabled = IIf(cmbAircraft.SelectedIndex > 0, True, False)
            'End
        End If
        upnlUnitList.Update()
        upnlTank.Update()
    End Sub
    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        For i As Integer = 0 To ChklistAircraftWiseSummary.Items.Count - 1
            ChklistAircraftWiseSummary.Items.Item(i).Selected = chkSelectAll.Checked
        Next
    End Sub
    'Private Sub chkUnit_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkUnit.CheckedChanged
    '    If chkUnit.Checked Then
    '        cmbUnitList.DataSource = UnitListMain.GetUnitList("", "(SELECT)")
    '        cmbUnitList.DataBind()
    '    Else
    '        cmbUnitList.DataSource = Nothing
    '        cmbUnitList.DataBind()
    '    End If
    '    upnlUnitList.Update()

    'End Sub
    Private Sub cmbUnitList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbUnitList.SelectedIndexChanged

        mMachineNameValueListForAircraftWiseSummary = MachineNameValueList.GetMachineList(Today.Date.ToString, , 0, 0, "", "", "", False, "", , True, , , , chkForAircraftWiseSummary.Checked, cmbUnitList.SelectedValue)
        Session("mMachineNameValueListForAircraftWiseSummary") = mMachineNameValueListForAircraftWiseSummary
        ChklistAircraftWiseSummary.DataSource = mMachineNameValueListForAircraftWiseSummary
        ChklistAircraftWiseSummary.DataBind()

        If cmbUnitList.SelectedIndex = 0 Or mMachineNameValueListForAircraftWiseSummary.Count = 0 Then
            chkSelectAll.Visible = False
        Else
            chkSelectAll.Visible = True
        End If
        upnlAircraftWiseSummary.Update()
    End Sub

#End Region


End Class