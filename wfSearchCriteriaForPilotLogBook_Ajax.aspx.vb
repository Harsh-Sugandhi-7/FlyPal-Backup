'Converted in to Ajax Prashant on 4-Jun-2020 ALL04062020
Public Class wfSearchCriteriaForPilotLogBook_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim mAssemblylist As AssemblyList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As String
    Dim AssemblyID As String
    Dim Aircraft As String
    Dim AssemblyType As String
    Dim AssemblyText As String
    Dim Model As String
    Dim SerialNo As String
    Dim RegNo As String

    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail

    Dim objLogPilotRegister As ReportPilotLogRegister
    Dim objLogDetail As AssemblyLogDifferncePeriodList
    Dim dsLogRegister As New dsLogRegister

    Dim objEleLogRegister As New ReportHistoryCumLogRegister
    Dim objEleLogDetail As AssemblyLogDifferncePeriodList
    Dim dsEleLogRegister As New dsHistoryCumLogRegister
    Dim LogType As Integer
    Dim mEmployeeList As EmployeeList
    Dim PilotID As String
    Dim PilotCoPilot As String
    Public mFlightLogClassificationList As FlightLogClassificationList
    Dim FlightClassificationName As String  'Added By Prashant 4-Jun-2020 ALL04062020
    Public EventLogDetails As String = String.Empty
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mEmployeeList = CType(Session("mPilotList"), EmployeeList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForPilotLogBook_Ajax.aspx" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblylist")
            Session.Remove("mPilotList")
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
        lblPilot1.Visible = True
        lblFlightLogClassification1.Visible = True
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.ToString
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.ToString
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If cmbAircraft.SelectedIndex > 0 Then
            MachineID = cmbAircraft.SelectedValue.ToString
            RegNo = mMachineList(cmbAircraft.SelectedIndex).RegNo
        Else
            AssemblyText = ""
            MachineID = Guid.Empty.ToString
        End If
        PilotID = cmbPilotList.SelectedValue.ToString

        If (chkPilot.Checked) And (chkCoPilot.Checked) And (cmbPilotList.SelectedIndex = 0) Then
            PilotCoPilot = "All Pilot & CoPilot"
        ElseIf (chkPilot.Checked) And (chkCoPilot.Checked) And (cmbPilotList.SelectedIndex > 0) Then
            PilotCoPilot = cmbPilotList.SelectedItem.Text + " " + "(As Pilot & CoPilot)"
        ElseIf (chkPilot.Checked) And (cmbPilotList.SelectedIndex = 0) Then
            PilotCoPilot = "All Pilot"
        ElseIf (chkCoPilot.Checked) And (cmbPilotList.SelectedIndex = 0) Then
            PilotCoPilot = "All CoPilot"
        ElseIf (chkPilot.Checked) And (cmbPilotList.SelectedIndex > 0) Then
            PilotCoPilot = cmbPilotList.SelectedItem.Text + " " + "(As Pilot)"
        ElseIf (chkCoPilot.Checked) And (cmbPilotList.SelectedIndex > 0) Then
            PilotCoPilot = cmbPilotList.SelectedItem.Text + " " + "(As CoPilot)"
        End If

      FlightClassificationName = String.Empty
        For i As Integer = 0 To ChkFlightLogClassificationList.Items.Count - 1
            If ChkFlightLogClassificationList.Items(i).Selected Then
                If FlightClassificationName.Length = 0 Then
                    FlightClassificationName = ChkFlightLogClassificationList.Items(i).Text
                Else
                    FlightClassificationName = FlightClassificationName + "," + ChkFlightLogClassificationList.Items(i).Text
                End If
            End If
        Next

        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblPilot1.Text = PilotCoPilot
        lblFlightLogClassification1.Text = "Flight Log Classification : " & IIf(FlightClassificationName = "", "(All)", FlightClassificationName)
        EventLogDetails = lblDateRangeFrom.Text + " " + lblDateRangeTo.Text + ", " + lblAircraft1.Text + ", " + lblPilot1.Text + ", " + lblFlightLogClassification1.Text
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text
        MachineID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyType = ""
        Aircraft = ""
        AssemblyText = ""
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim serchstr7 As String  'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 
        Dim str1 As String = ""
        If chkLogNo.Checked Then
            str1 = "Log No."
        Else
            str1 = ""
        End If
        If chkLogPageNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Log Page No."
        Else
            str1 = str1 + "/" + "Log Page No."
        End If

        If chkFlightNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Flight No."
        Else
            str1 = str1 + "/" + "Flight No."
        End If

        str1 = str1 + vbCrLf + "Classification"
        If mAssemblylist Is Nothing Then
            AssemblyID = "{00000000-0000-0000-0000-000000000000}"
            myReport = New crPilotLogRegisterAll
            ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, AssemblyType + " " + "Details", , , _
            mMachineList(cmbAircraft.SelectedIndex).RegNo, , "", "", PilotCoPilot, IIf(FlightClassificationName = "", "(All)", FlightClassificationName), , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))
        Else
            AssemblyID = mAssemblylist.Item(0).ID.ToString
            myReport = New crPilotLogRegister
            ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, AssemblyType + " " + "Details", , , _
            mMachineList(cmbAircraft.SelectedIndex).RegNo, cmbPilotList.SelectedItem.Text, mAssemblylist.Item(0).ModelName, mAssemblylist.Item(0).SerialNo, PilotCoPilot, IIf(FlightClassificationName = "", "(All)", FlightClassificationName), , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))
        End If
        objLogDetail = AssemblyLogDifferncePeriodList.GetAssemblyLogDifferencePeriodList(StartDate, EndDate, New Guid(AssemblyID), True)
        objLogPilotRegister = ReportPilotLogRegister.GetPilotRegister(StartDate, EndDate, AssemblyID, MachineID, , FlightClassificationName, , PilotID, chkPilot.Checked, chkCoPilot.Checked, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked, SkipIsForInventoryAircarft:=True)

        'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
            If cmbAircraft.SelectedIndex > 0 Then
                serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
            Else
                serchstr7 = ""
            End If
        Else
            serchstr7 = ""
        End If
        'End

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
             mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Pilot Log Register", "From Date: " + New SmartDate(StartDate).FormattedText, _
        "To Date: " + New SmartDate(EndDate).FormattedText, "Aircraft: " + cmbAircraft.SelectedItem.Text, _
        "Pilot: " + cmbPilotList.SelectedItem.Text, str1, AppSettings("Product Version"), AppSettings("SINote"), _
        "", serchstr7, lblFlightLogClassification1.Text, rdbLocal.Checked.ToString, AppSettings("Logo"))

        If objLogPilotRegister.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objLogPilotRegister.Count > 0 Then

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1011)

            '*******************************
        End If
        da.Fill(dsLogRegister, objLogDetail)
        da.Fill(dsLogRegister, objLogPilotRegister)
        da.Fill(dsLogRegister, Report)
        da.Fill(dsLogRegister, ReportStatusList)
        Dim mrptImage As rptImage = rptImage.GetImage(dsLogRegister)
        da.Fill(dsLogRegister, mrptImage)
        myReport.SetDataSource(dsLogRegister)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "PilotLogBook", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        'Dim custValidator As CustomValidator
        'custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "cmbAircraft" Then
        '    If cmbAircraft.SelectedIndex = 0 Then
        '        custValidator.ErrorMessage = "Please select the Aircraft"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
    End Sub
    Private Sub DataFieldBind()
        mMachineList = MachineList.GetMachineListMonitoringStatus(Now.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(All)", SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        cmbAircraft.DataBind()
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)", , , True)
        cmbPilotList.DataSource = mEmployeeList
        Session("mPilotList") = mEmployeeList
        cmbPilotList.DataBind()

        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("")
        ChkFlightLogClassificationList.DataSource = mFlightLogClassificationList
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        ChkFlightLogClassificationList.DataBind()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForPilotLogBook_Ajax.aspx"
            ResetValues()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            rdbLocal.Visible = IIf(AppSettings("ClientCode") = "GEP", True, False)
            rdbutC.Visible = IIf(AppSettings("ClientCode") = "GEP", True, False)
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblylist = Nothing
        mEmployeeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            mAssemblylist = Nothing
            Session("mAssemblylist") = mAssemblylist
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text, , True)
            Session("mAssemblyList") = mAssemblylist
            rdbLocal.Checked = IIf(mMachineList(New Guid(cmbAircraft.SelectedValue)).IsUTC = False, True, False)
            rdbUTC.Checked = IIf(mMachineList(New Guid(cmbAircraft.SelectedValue)).IsUTC = True, True, False)
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        upnlLocalUTC.Update()
    End Sub
    'Added by Abhishek on 22-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            Dim serchstr7 As String
            Dim str1 As String = ""
            If chkLogNo.Checked Then
                str1 = "Log No."
            Else
                str1 = ""
            End If
            If chkLogPageNo.Checked = False Then
                '
            ElseIf str1 = "" Then
                str1 = "Log Page No."
            Else
                str1 = str1 + "/" + "Log Page No."
            End If

            If chkFlightNo.Checked = False Then
                '
            ElseIf str1 = "" Then
                str1 = "Flight No."
            Else
                str1 = str1 + "/" + "Flight No."
            End If
            If mAssemblylist Is Nothing Then
                AssemblyID = "{00000000-0000-0000-0000-000000000000}"
                myReport = New crPilotLogRegisterAll
                ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, AssemblyType + " " + "Details", , , _
                mMachineList(cmbAircraft.SelectedIndex).RegNo, , "", "", PilotCoPilot, FlightClassificationName, , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))
            Else
                AssemblyID = mAssemblylist.Item(0).ID.ToString
                myReport = New crPilotLogRegister
                ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, AssemblyType + " " + "Details", , , _
                mMachineList(cmbAircraft.SelectedIndex).RegNo, cmbPilotList.SelectedItem.Text, mAssemblylist.Item(0).ModelName, mAssemblylist.Item(0).SerialNo, PilotCoPilot, FlightClassificationName, , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))
            End If
            objLogDetail = AssemblyLogDifferncePeriodList.GetAssemblyLogDifferencePeriodList(StartDate, EndDate, New Guid(AssemblyID), True)
            objLogPilotRegister = ReportPilotLogRegister.GetPilotRegister(StartDate, EndDate, AssemblyID, MachineID, , FlightClassificationName, , PilotID, chkPilot.Checked, chkCoPilot.Checked, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked, SkipIsForInventoryAircarft:=True)

            'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                If cmbAircraft.SelectedIndex > 0 Then
                    serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
                Else
                    serchstr7 = ""
                End If
            Else
                serchstr7 = ""
            End If

            'End

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Pilot Log Register", "From Date: " + New SmartDate(StartDate).FormattedText, _
            "To Date: " + New SmartDate(EndDate).FormattedText, "Aircraft: " + cmbAircraft.SelectedItem.Text, _
            "Pilot: " + cmbPilotList.SelectedItem.Text, str1, AppSettings("Product Version"), AppSettings("SINote"), _
            "", serchstr7, lblFlightLogClassification1.Text, rdbLocal.Checked.ToString, AppSettings("Logo"))

            If objLogPilotRegister.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
                'Added By Utkarsh On 7-Jun-2011 For All07062011

            ElseIf objLogPilotRegister.Count > 0 Then

                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1011)

                '*******************************
            End If

            dsLogRegister.Clear()
            da.Fill(dsLogRegister, objLogDetail)
            da.Fill(dsLogRegister, "ExcelReportPilotLogRegister", objLogPilotRegister)
            da.Fill(dsLogRegister, Report)
            da.Fill(dsLogRegister, ReportStatusList)
            'da.Fill(dsLogRegister, "ExcelReportLogRegister", objLogPilotRegister)

            Dim columnToRemove1 As String() = {"ReferencedDocuments", "LogID", "AssemblyID", "LogDate", "LogDateFormatted", "RegNo", "Col1Label", "Col1Diff", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col1Final", "Col2Label", "Col2DiffInDecimal", "Col2DiffPeriodID", "Col2DiffPeriodUnitID", "Col2Final", "Col3Label", "Col3Diff", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col3Final", "Col4Label", "Col4Diff", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", "Col4Final", "ColLabel", "ColDiff", "ColFinal", "LogPageNo", "IsLogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocumentsHeading"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains(columnToRemove1(i)) Then
                    dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "SearchStr6", "SearchStr7", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsLogRegister.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    dsLogRegister.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr1") Then
                dsLogRegister.Tables("ReportData").Columns("SearchStr1").ColumnName = "FromDate "
            End If

            If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr2") Then
                dsLogRegister.Tables("ReportData").Columns("SearchStr2").ColumnName = "DateTo "
            End If

            If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr3") Then
                dsLogRegister.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
            End If

            If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr4") Then
                dsLogRegister.Tables("ReportData").Columns("SearchStr4").ColumnName = "Pilot"
            End If
            If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr5") Then
                dsLogRegister.Tables("ReportData").Columns("SearchStr5").ColumnName = "Reference Document"
            End If
            If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr8") Then
                dsLogRegister.Tables("ReportData").Columns("SearchStr8").ColumnName = "Flight Log Classification"
            End If


            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("PilotName") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("PilotName").ColumnName = "Pilot "
            End If

            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("CoPilotName") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("CoPilotName").ColumnName = "Co-Pilot"
            End If

            'If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("ReferencedDocuments") Then
            '    dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("ReferencedDocuments").ColumnName = "Log.No./Log Page No./Flight No."
            'End If
            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("TimeInAir") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("TimeInAir").ColumnName = "In Air"
            End If

            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("TimeOnGround") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("TimeOnGround").ColumnName = "Ground"
            End If
            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("LogNo") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("LogNo").ColumnName = "Log No."
            End If

            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("LogPageNoFormatted") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("LogPageNoFormatted").ColumnName = "Page No."
            End If
            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("FlightNo") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("FlightNo").ColumnName = "Flight No."
            End If
            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("Col2Diff") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("Col2Diff").ColumnName = "Cycles/Landing"
            End If
            If dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns.Contains("FlightLogClassificationName") Then
                dsLogRegister.Tables("ExcelReportPilotLogRegister").Columns("FlightLogClassificationName").ColumnName = "Flight Log Classification"
            End If


            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsLogRegister.Tables("ReportData"))
            dsNew.Merge(dsLogRegister.Tables("ExcelReportPilotLogRegister"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExcelReportPilotLogRegister").TableName = "Pilot Log Book"
			Session("ExcelFileName") = "Pilot Log Book"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "PilotLogBook", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub chkSelectAllFlightLogClassification_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelectAllFlightLogClassification.CheckedChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showTextField", "showTextField();", True)
    End Sub
#End Region


End Class