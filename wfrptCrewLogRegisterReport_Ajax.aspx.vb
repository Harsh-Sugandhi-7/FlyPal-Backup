Public Class wfrptCrewLogRegisterReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ReportStatusList As New rptStatusList
    Dim mMachineNameValueList As MachineNameValueList
    Dim mAssemblylist As AssemblyList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As String
    Dim AssemblyID As String
    Dim Aircraft As String
    Dim Model As String
    Dim SerialNo As String
    Dim RegNo As String

    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail

    Dim objCrewLogRegister As CrewLogRegisterReport
    Dim dsLogRegister As New dsCrewLogRegisterReport

    Dim LogType As Integer
    Dim mEmployeeList As EmployeeList
    Dim CrewID As String
    Dim crew As String
    Dim CrewName As String
    Dim AllAircraft As Boolean = False

    Dim mDutyTypeList As DutyTypeList
    Dim mDutyTypeList2 As DutyTypeList
    Public mDutyAs As String

    Dim CoPilotID As String
    Dim CoPilot As String
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mEmployeeList = CType(Session("mPilotList"), EmployeeList)
        mDutyTypeList = CType(Session("mDutyTypeList"), DutyTypeList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptCrewLogRegisterReport_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblylist")
            Session.Remove("mPilotList")
            Session.Remove("mDutyTypeList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDutyType1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblPilot1.Visible = True
        lblCopilot.Visible = True 'Added By Prashant 18-Jun-2013  ALL18062013
        lblDutyType2.Visible = True
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
            RegNo = mMachineNameValueList(cmbAircraft.SelectedIndex).RegNo
        End If
        MachineID = cmbAircraft.SelectedValue.ToString
        'CrewID = cmbPilotList.SelectedValue.ToString
        'crew = cmbPilotList.SelectedItem.Text

        CrewID = mEmployeeList.Item(txtSearch.Text.Trim, "").ID.ToString

        CoPilotID = mEmployeeList.Item(txtCoPilot.Text.Trim, "").ID.ToString 'Added By Prashant 18-Jun-2013  ALL18062013
        If txtSearch.Text.Trim = "" Then
            lblPilot1.Text = "Crew 1: (All)"
            'CrewName = "Crew Name : (All)"
            CrewName = "Crew 1 : (All)" 'Added By Prashant 18-Jun-2013  ALL18062013"
        Else
            crew = mEmployeeList(txtSearch.Text.Trim, "").Name
            'CrewName = "Crew Name : " & crew  'Commented By Prashant 18-Jun-2013  ALL18062013
            CrewName = "Crew 1: " & crew    'Added By Prashant 18-Jun-2013  ALL18062013"
        End If

        If txtCoPilot.Text.Trim = "" Then
            lblCopilot.Text = "Crew 2 : (All)"
            CoPilot = "Crew 2 : (All)" 'Added By Prashant 18-Jun-2013  ALL18062013"
        Else
            CoPilot = "Crew 2 : " & txtCoPilot.Text.Trim    'Added By Prashant 18-Jun-2013  ALL18062013"
        End If

        mDutyAs = IIf(cmbDutyAs1.SelectedIndex > 0, "On Duty As: " & cmbDutyAs1.SelectedItem.Text, "On Duty As: (All)")
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "(All)")
        lblPilot1.Text = CrewName
        lblCopilot.Text = CoPilot
        lblDutyType1.Text = "Duty As 1 : " & IIf(cmbDutyAs1.SelectedItem.Text = "(All)" Or cmbDutyAs1.SelectedItem.Text = "(SELECT)", "(All)", cmbDutyAs1.SelectedItem.Text)
        lblDutyType2.Text = "Duty As 2 : " & cmbDutyAs2.SelectedItem.Text
        EventLogDetail = lblDateRangeFrom.Text + "," + lblDateRangeTo.Text + "," + lblAircraft1.Text + "," + lblPilot1.Text + "," + lblCopilot.Text + "," + lblDutyType1.Text + "," + lblDutyType2.Text  'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.ToString
        EndDate = txtToDate.Text.ToString
        MachineID = "{00000000-0000-0000-0000-000000000000}"
        Aircraft = ""
        crew = ""
        CrewID = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Private Sub SetReport()
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

        Dim mCheckedDetail As Boolean = True
        If optDetail.Checked Then
            If AppSettings("ClientCode") = "BAMS" Then 'Added By Vikrant On 01-Mar-2021 For BAMS01032021
                myReport = New crptCrewLogRegisterReportBAMS
                'End
            Else
                myReport = New crptCrewLogRegisterReport
            End If
            mCheckedDetail = True
        ElseIf optSummary.Checked Then
            If AppSettings("ClientCode") = "BAMS" Then 'Added By Vikrant On 01-Mar-2021 For BAMS01032021
                myReport = New crptCrewLogRegisterSummaryReportBAMS
                'End
            Else
                myReport = New crptCrewLogRegisterSummaryReport
            End If

            mCheckedDetail = False
        End If


        ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, , , , _
        cmbAircraft.SelectedItem.Text, , "", "", crew, , , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))

        objCrewLogRegister = CrewLogRegisterReport.GetCrewLogRegister(StartDate, EndDate, MachineID, True, , CrewID, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked, cmbDutyAs1.SelectedValue, , mCheckedDetail, CoPilotID, cmbDutyAs2.SelectedValue, optSingle.Checked, optBoth.Checked, SkipIsForInventoryAircarft:=True)


        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
            If cmbAircraft.SelectedIndex > 0 Then
                serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
            Else
                serchstr7 = ""
            End If
        Else
            serchstr7 = ""
        End If


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
             mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Crew Log Register", "From Date: " + New SmartDate(StartDate).FormattedText, "To Date: " + New SmartDate(EndDate).FormattedText, "Aircraft: " + cmbAircraft.SelectedItem.Text, CrewName, lblDutyType1.Text, AppSettings("Product Version"), AppSettings("SINote"), CoPilot, serchstr7, lblDutyType2.Text, str1, AppSettings("Logo"))

        If objCrewLogRegister.Count = 0 Then
            'ResetValues()
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfrptCrewLogRegisterReport.aspx?Backpage="
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objCrewLogRegister.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1241)
        End If
        da.Fill(dsLogRegister, objCrewLogRegister)
        da.Fill(dsLogRegister, Report)
        da.Fill(dsLogRegister, ReportStatusList)
        Dim mrptImage As rptImage = rptImage.GetImage(dsLogRegister)
        da.Fill(dsLogRegister, mrptImage)
        myReport.SetDataSource(dsLogRegister)
        Session("CrystalReport") = myReport

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)

        Dim str As String = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)
        MarkLog(Util.Action.Print, "CrewLogBook", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        ResetValues()
    End Sub

#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , 0, 0, "", "", "", True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)", , , False)
        Session("mPilotList") = mEmployeeList
        mDutyTypeList = DutyTypeList.GetDutyTypeList(True, "(All)")
        cmbDutyAs1.DataSource = mDutyTypeList
        cmbDutyAs1.DataBind()
        mDutyTypeList2 = DutyTypeList.GetDutyTypeList(True, "(SELECT)")
        cmbDutyAs2.DataSource = mDutyTypeList2
        cmbDutyAs2.DataBind()
        Session("mDutyTypeList") = mDutyTypeList
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptCrewLogRegisterReport_Ajax.aspx"
            ResetValues()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            optDetail.Checked = True
            optSingle.Checked = True
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        upnlDispalyReport.Update()
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        'If IsValid = True Then
        If (optBoth.Checked = True And (String.IsNullOrEmpty(txtSearch.Text) Or String.IsNullOrEmpty(txtCoPilot.Text))) Then
            MSGBoxCtrl.show(MSGBox.Message_title.CrewSelection, MSGBox.Message_text.CrewSelection, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If (optBoth.Checked = True And (Not String.IsNullOrEmpty(txtSearch.Text) Or Not String.IsNullOrEmpty(txtCoPilot.Text)) And (cmbDutyAs1.SelectedIndex = 0 Or cmbDutyAs2.SelectedIndex = 0)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.DutyAsSelection, MSGBox.Message_text.DutyAsSelection, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If (Not String.IsNullOrEmpty(txtSearch.Text) And Not String.IsNullOrEmpty(txtCoPilot.Text)) Then
            If txtSearch.Text.Equals(txtCoPilot.Text.Trim) Then
                MSGBoxCtrl.show(MSGBox.Message_title.CrewSelection, MSGBox.Message_text.SameCrews, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        If (cmbDutyAs1.SelectedIndex > 0 And cmbDutyAs2.SelectedIndex > 0) Then
            If cmbDutyAs1.SelectedValue.Equals(cmbDutyAs2.SelectedValue) Then
                MSGBoxCtrl.show(MSGBox.Message_title.DutyAsSelection, MSGBox.Message_text.SameDutyAs, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        SetReport()
        'End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        mAssemblylist = Nothing
        mEmployeeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            mAssemblylist = Nothing
            Session("mAssemblylist") = mAssemblylist
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, , True)
            Session("mAssemblyList") = mAssemblylist
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
    End Sub
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    '    Me.cmbAircraft.Visible = Not CType(sender, Boolean)
    '    upnlDates.Update()
    'End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        upnlDates.Update()
    End Sub
    'Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
    '    upnlDates.Update()
    'End Sub
    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        upnlDates.Update()
    End Sub
    Private Sub optSingle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optSingle.CheckedChanged
        txtCoPilot.Enabled = False
        cmbDutyAs2.Enabled = False
        txtCoPilot.Text = ""
        cmbDutyAs2.SelectedIndex = 0
        mDutyTypeList = DutyTypeList.GetDutyTypeList(True, "(All)")
        cmbDutyAs1.DataSource = mDutyTypeList
        cmbDutyAs1.DataBind()
        upnlAllInfo.Update()
    End Sub
    Private Sub optBoth_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optBoth.CheckedChanged
        txtCoPilot.Enabled = True
        cmbDutyAs2.Enabled = True
        mDutyTypeList = DutyTypeList.GetDutyTypeList(True, "(SELECT)")
        cmbDutyAs1.DataSource = mDutyTypeList
        cmbDutyAs1.DataBind()
        upnlAllInfo.Update()
    End Sub
#End Region
    'Added by Abhishek on 19-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            If (optBoth.Checked = True And (String.IsNullOrEmpty(txtSearch.Text) Or String.IsNullOrEmpty(txtCoPilot.Text))) Then
                MSGBoxCtrl.show(MSGBox.Message_title.CrewSelection, MSGBox.Message_text.CrewSelection, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If (optBoth.Checked = True And (Not String.IsNullOrEmpty(txtSearch.Text) Or Not String.IsNullOrEmpty(txtCoPilot.Text)) And (cmbDutyAs1.SelectedIndex = 0 Or cmbDutyAs2.SelectedIndex = 0)) Then
                MSGBoxCtrl.show(MSGBox.Message_title.DutyAsSelection, MSGBox.Message_text.DutyAsSelection, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If (Not String.IsNullOrEmpty(txtSearch.Text) And Not String.IsNullOrEmpty(txtCoPilot.Text)) Then
                If txtSearch.Text.Equals(txtCoPilot.Text.Trim) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.CrewSelection, MSGBox.Message_text.SameCrews, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
            If (cmbDutyAs1.SelectedIndex > 0 And cmbDutyAs2.SelectedIndex > 0) Then
                If cmbDutyAs1.SelectedValue.Equals(cmbDutyAs2.SelectedValue) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DutyAsSelection, MSGBox.Message_text.SameDutyAs, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
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

            Dim mCheckedDetail As Boolean = True
            If optDetail.Checked Then
                ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, , , , _
           cmbAircraft.SelectedItem.Text, , "", "", crew, , , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))

                objCrewLogRegister = CrewLogRegisterReport.GetCrewLogRegister(StartDate, EndDate, MachineID, True, , CrewID, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked, cmbDutyAs1.SelectedValue, , mCheckedDetail, CoPilotID, cmbDutyAs2.SelectedValue, optSingle.Checked, optBoth.Checked, SkipIsForInventoryAircarft:=True)


                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                    If cmbAircraft.SelectedIndex > 0 Then
                        serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
                    Else
                        serchstr7 = ""
                    End If
                Else
                    serchstr7 = ""
                End If


                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Crew Log Register", "From Date: " + New SmartDate(StartDate).FormattedText, "To Date: " + New SmartDate(EndDate).FormattedText, "Aircraft: " + cmbAircraft.SelectedItem.Text, CrewName, lblDutyType1.Text, AppSettings("Product Version"), AppSettings("SINote"), CoPilot, serchstr7, lblDutyType2.Text, str1, AppSettings("Logo"))

                If objCrewLogRegister.Count = 0 Then
                    'ResetValues()
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfrptCrewLogRegisterReport.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf objCrewLogRegister.Count > 0 Then
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1241)
                End If
                da.Fill(dsLogRegister, "ExcelCrewLogRegisterReport", objCrewLogRegister)
                da.Fill(dsLogRegister, Report)
                da.Fill(dsLogRegister, ReportStatusList)
                Dim columnToRemove1 As String() = {"DutyAs2", "CrewName", "Crew2Name", "DutyAs", "LogID", "LogNo", "RegNo", "CrewID", "ReferencedDocumentsHeading", "GroupBy", "Heading", "SrNoHeading", "SrNo", "SingleOne", "Both", "IsLogPageNo", "IsLogNo", "IsFlightNo", "LogPageNoFormatted", "FlightNo", "LogPageNo", "LogDateFormatted", "TotalTimeInAirDaily", "Col2DffDaily", "Col1FinalInInteger", "Col2FinalInInteger", "Col3FinalInInteger", "Col4FinalInInteger", "TotalTimeInAirDailyInInteger", "Col2DffDailyInInteger"}
                For i As Integer = 0 To columnToRemove1.Length - 1
                    If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains(columnToRemove1(i)) Then
                        dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Remove(columnToRemove1(i))
                    End If
                Next




                Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "ShortName", "SearchStr7", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
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
                    dsLogRegister.Tables("ReportData").Columns("SearchStr4").ColumnName = "Crew1"
                End If

                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr5") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr5").ColumnName = "DutyType1"
                End If
                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr6") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr6").ColumnName = "Crew2"
                End If
                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr8") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr8").ColumnName = "DutyType2"
                End If

                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr9") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr9").ColumnName = "Reference Document"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("LogDate") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("LogDate").ColumnName = "Log Date"
                End If
                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("LogNo") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("LogNo").ColumnName = "Log.No."
                End If
                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TakeOff") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TakeOff").ColumnName = "Take Off Time"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TouchDown") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TouchDown").ColumnName = "Touch Down Time"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("BlockTime") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("BlockTime").ColumnName = "Block Time"
                End If
                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TimeInAir") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TimeInAir").ColumnName = "In Air"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TimeOnGround") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TimeOnGround").ColumnName = "Ground"
                End If
                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("CrewName") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("CrewName").ColumnName = "Crew Name"
                End If

                'If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("DutyAs") Then
                '    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("DutyAs").ColumnName = "DutyType1"
                'End If
                'If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("Crew2Name") Then
                '    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("Crew2Name").ColumnName = "Crew2"
                'End If
                'If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("DutyAs2") Then
                '    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("DutyAs2").ColumnName = "DutyType2"
                'End If
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(dsLogRegister.Tables("ReportData"))
                dsNew.Merge(dsLogRegister.Tables("ExcelCrewLogRegisterReport"))

                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Tables("ExcelCrewLogRegisterReport").TableName = "Crew Log Book"
				Session("ExcelFileName") = "Crew Log Book"
				Session("dsNew") = dsNew
				'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
				'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
				'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
				'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                ' myReport = New crptCrewLogRegisterReport
                mCheckedDetail = True
            ElseIf optSummary.Checked Then
                ' myReport = New crptCrewLogRegisterSummaryReport
                ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, , , , _
          cmbAircraft.SelectedItem.Text, , "", "", crew, , , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))

                objCrewLogRegister = CrewLogRegisterReport.GetCrewLogRegister(StartDate, EndDate, MachineID, True, , CrewID, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked, cmbDutyAs1.SelectedValue, , mCheckedDetail, CoPilotID, cmbDutyAs2.SelectedValue, optSingle.Checked, optBoth.Checked, SkipIsForInventoryAircarft:=True)


                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                    If cmbAircraft.SelectedIndex > 0 Then
                        serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
                    Else
                        serchstr7 = ""
                    End If
                Else
                    serchstr7 = ""
                End If


                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Crew Log Register", "From Date: " + New SmartDate(StartDate).FormattedText, "To Date: " + New SmartDate(EndDate).FormattedText, "Aircraft: " + cmbAircraft.SelectedItem.Text, CrewName, lblDutyType1.Text, AppSettings("Product Version"), AppSettings("SINote"), CoPilot, serchstr7, lblDutyType2.Text, str1, AppSettings("Logo"))

                If objCrewLogRegister.Count = 0 Then
                    'ResetValues()
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfrptCrewLogRegisterReport.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf objCrewLogRegister.Count > 0 Then
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1241)
                End If
                da.Fill(dsLogRegister, "ExcelCrewLogRegisterReport", objCrewLogRegister)
                da.Fill(dsLogRegister, Report)
                da.Fill(dsLogRegister, ReportStatusList)
                Dim columnToRemove1 As String() = {"CrewName", "DutyAs", "Crew2Name", "DutyAs2", "LogID", "LogNo", "RegNo", "CrewID", "ReferencedDocumentsHeading", "GroupBy", "Heading", "SrNoHeading", "SrNo", "SingleOne", "Both", "IsLogPageNo", "IsLogNo", "IsFlightNo", "LogPageNoFormatted", "FlightNo", "LogPageNo", "LogDateFormatted"}
                For i As Integer = 0 To columnToRemove1.Length - 1
                    If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains(columnToRemove1(i)) Then
                        dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Remove(columnToRemove1(i))
                    End If
                Next




                Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "ShortName", "SearchStr7", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
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
                    dsLogRegister.Tables("ReportData").Columns("SearchStr4").ColumnName = "Crew1"
                End If

                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr5") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr5").ColumnName = "DutyType1"
                End If
                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr6") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr6").ColumnName = "Crew2"
                End If
                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr8") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr8").ColumnName = "DutyType2"
                End If

                If dsLogRegister.Tables("ReportData").Columns.Contains("SearchStr9") Then
                    dsLogRegister.Tables("ReportData").Columns("SearchStr9").ColumnName = "Reference Document"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("LogDate") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("LogDate").ColumnName = "Log Date"
                End If
                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("LogNo") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("LogNo").ColumnName = "Log.No."
                End If
                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TakeOff") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TakeOff").ColumnName = "Take Off Time"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TouchDown") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TouchDown").ColumnName = "Touch Down Time"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("BlockTime") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("BlockTime").ColumnName = "Block Time"
                End If
                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TimeInAir") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TimeInAir").ColumnName = "In Air"
                End If

                If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("TimeOnGround") Then
                    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("TimeOnGround").ColumnName = "Ground"
                End If
                'If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("CrewName") Then
                '    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("CrewName").ColumnName = "Crew1"
                'End If

                'If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("DutyAs") Then
                '    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("DutyAs").ColumnName = "DutyType1"
                'End If
                'If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("Crew2Name") Then
                '    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("Crew2Name").ColumnName = "Crew2"
                'End If
                'If dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns.Contains("DutyAs2") Then
                '    dsLogRegister.Tables("ExcelCrewLogRegisterReport").Columns("DutyAs2").ColumnName = "DutyType2"
                'End If
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(dsLogRegister.Tables("ReportData"))
                dsNew.Merge(dsLogRegister.Tables("ExcelCrewLogRegisterReport"))

                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Tables("ExcelCrewLogRegisterReport").TableName = "Crew Log Book"
				Session("ExcelFileName") = "Crew Log Book"
				Session("dsNew") = dsNew
                'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
                'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
                'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                MarkLog(Util.Action.Print, "CrewLogBook", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
                mCheckedDetail = False
            End If


          
        End If
    End Sub
End Class