
'AJAX Conversion By     :   Saylee
'Dated                  :   1-Feb-2015



Public Class wfrptAuditFindings_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'Private mAuditExecutionList As AuditExecutionList
    Private mAuditExecutionAuditNoList As AuditExecutionAuditNoList

    'Private mDepartmentList As AuditDepartmentList
    Private mResponsibleDepartmentList As EmployeeDepartmentList
    Private mFindingStatusList As FindingStatusList

    Dim DateIndex, FromDate, ToDate As String
    Public mAuditPriorityList As AuditPriorityList
    Dim mEventLogDetails As String = String.Empty 'Added by Shital on 18-Jan-2021
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        'mAuditExecutionList = Session("mAuditExecutionList")
        mAuditExecutionAuditNoList = Session("mAuditExecutionAuditNoList")
        mResponsibleDepartmentList = Session("mResponsibleDepartmentList")
        mFindingStatusList = Session("mFindingStatusList")
        mAuditPriorityList = Session("mAuditPriorityList")
    End Sub
    Private Sub SetSession()
        ' Session("mAuditExecutionList") = mAuditExecutionList
        Session("mAuditExecutionAuditNoList") = mAuditExecutionAuditNoList
        Session("mResponsibleDepartmentList") = mResponsibleDepartmentList
        Session("mFindingStatusList") = mFindingStatusList
    End Sub
    Public Sub RemoveSessions()
        'Session.Remove("mAuditExecutionList")
        Session.Remove("mAuditExecutionAuditNoList")
        Session.Remove("mResponsibleDepartmentList")
        Session.Remove("mFindingStatusList")

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-1-1900")
                txtToDate.Text = CDate("1-1-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Public Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptAuditFindings As rptAuditFindings
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsrptAuditFindings As New dsrptAuditFindings
        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String
        Dim SearchStr5 As String
        Dim SearchStr7 As String
        If cmbAuditInfoList.SelectedIndex > 0 Then
            SearchStr3 = cmbAuditInfoList.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        Dim IShowAllTasksRequired As Integer = 0
        If chkSummary.Checked = True Then
            IShowAllTasksRequired = 1  ''ShowAllTasks =1 means skips tasks which has no findings
        Else
            IShowAllTasksRequired = 0  ''ShowAllTasks =0 means show tasks which has no findings
        End If
        mrptAuditFindings = rptAuditFindings.GetrptAuditFindings(txtFromDate.Text.ToString, txtToDate.Text.ToString, SearchStr3, cmbDepartmentList.SelectedValue.ToString, cmbFindingStatus.SelectedValue, PriorityLevelID:=Val(cmbFindingLevel.SelectedValue.ToString), ShowAllTasks:=IShowAllTasksRequired, UsedFromFindingEntry:=IShowAllTasksRequired) ''ShowAllTasks =1 means skips tasks which has no findings

        If mrptAuditFindings.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If chkSummary.Checked Then
            myReport = New crptAuditFindingsSummary
        Else
            If cmbFormat.SelectedValue = 0 Then
                myReport = New crptAuditFindingsFormat1
            ElseIf cmbFormat.SelectedValue = 1 Then
                myReport = New crptAuditFindingsFormat2
            End If
        End If

        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        SearchStr1 = New SmartDate(FromDate).FormattedText
        SearchStr2 = New SmartDate(ToDate).FormattedText

        If cmbDepartmentList.SelectedIndex > 0 Then
            SearchStr4 = cmbDepartmentList.SelectedItem.Text
        Else
            SearchStr4 = ""
        End If

        If cmbFindingStatus.SelectedIndex > 0 Then
            SearchStr5 = cmbFindingStatus.SelectedItem.Text
        Else
            SearchStr5 = ""
        End If

        If cmbFindingLevel.SelectedIndex > 0 Then
            SearchStr7 = cmbFindingLevel.SelectedItem.Text
        Else
            SearchStr7 = ""
        End If

        'Added by Saylee on 11-Jul-2023
        Dim legend1 As String = ""
        Dim legend2 As String = ""
        Dim legend3 As String = ""
        Dim legend4 As String = ""
        For i As Integer = 0 To mAuditPriorityList.Count - 1
            If i = 1 Then
                legend1 = mAuditPriorityList(1).Name
            ElseIf i = 2 Then
                legend2 = mAuditPriorityList(2).Name
            ElseIf i = 3 Then
                legend3 = mAuditPriorityList(3).Name
            ElseIf i = 4 Then
                legend4 = mAuditPriorityList(4).Name
            End If
        Next
        ''*****************************

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                     mCompanyDetail.Email, mCompanyDetail.WebSite, "Audit Findings Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4,
                                     SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:=AppSettings("ClientCode"), SearchStr7:=SearchStr7,
                                     SearchStr8:=AppSettings("IssueNoInAudit"), SearchStr9:=AppSettings("RevisionNoInAudit"), SearchStr10:=AppSettings("Logo"),
                                     SearchStr11:=legend1, SearchStr12:=legend2, SearchStr13:=legend3, SearchStr14:=legend4) 'Changed By Utkarsh For Report Logo.

        If IsExcel = False Then         'PDF format
            dsrptAuditFindings.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(dsrptAuditFindings)
            '----------------------------------------------------------
            da.Fill(dsrptAuditFindings, mrptAuditFindings)
            da.Fill(dsrptAuditFindings, Report)
            da.Fill(dsrptAuditFindings, mrptImage) 'Added by Utkarsh for Report Logo
            myReport.SetDataSource(dsrptAuditFindings)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "AuditFindings", mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
            ResetValues()
            '-------------------------------------------------------------------------------------------
        Else                            'Excel format
            dsrptAuditFindings.Clear()
            da.Fill(dsrptAuditFindings, mrptAuditFindings)
            da.Fill(dsrptAuditFindings, Report)

            Dim columnToRemove2 As String() = {"ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsrptAuditFindings.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    dsrptAuditFindings.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If dsrptAuditFindings.Tables("ReportData").Columns.Contains("SearchStr1") Then
                dsrptAuditFindings.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If dsrptAuditFindings.Tables("ReportData").Columns.Contains("SearchStr2") Then
                dsrptAuditFindings.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If dsrptAuditFindings.Tables("ReportData").Columns.Contains("SearchStr3") Then
                dsrptAuditFindings.Tables("ReportData").Columns("SearchStr3").ColumnName = "Audit No."
            End If
            If dsrptAuditFindings.Tables("ReportData").Columns.Contains("SearchStr4") Then
                dsrptAuditFindings.Tables("ReportData").Columns("SearchStr4").ColumnName = "Department"
            End If
            If dsrptAuditFindings.Tables("ReportData").Columns.Contains("SearchStr5") Then
                dsrptAuditFindings.Tables("ReportData").Columns("SearchStr5").ColumnName = "Finding Status"
            End If

            Dim columnToRemove1 As String() = {"AuditExecutionTaskID", "AuditDateFormatted", "AuditCategoryID", "AuditCategoryName", "Code", "Description",
                                               "DepartmentID", "DepartmentName", "Note", "TaskStatusID", "TaskStatusName", "AuditExecutionTaskFindingID",
                                               "Reference", "NCRID", "NCRName", "Category", "PriorityID", "FindingStatusID", "FindingStatusName",
                                               "KindAttention", "Location", "GroupBy", "Heading", "TotalOpen", "TotalClosed",
                                               "TotalOpenClosed", "ExecutionStartDate", "ToMailID", "CCMailID", "NoRecFound",
                                               "AuditOnNameDetail", "AuditorName", "ExecutionEndDateFormatted", "AuditTypeName", "PriorityLevel1ID",
                                               "PriorityLevel2ID", "EntityManager", "OtherParticipants", "Auditors", "SeqNo", "SrNo", "ComplianceDetails",
                                               "DeadLineDate", "CorrectionDate", "ExecutionEndDate"}

            For i As Integer = 0 To columnToRemove1.Length - 1
                If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains(columnToRemove1(i)) Then
                    dsrptAuditFindings.Tables("rptAuditFindings").Columns.Remove(columnToRemove1(i))
                End If
            Next

            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("AuditNo") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("AuditNo").ColumnName = "Audit No."
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("ExecutionStartDateFormatted") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("ExecutionStartDateFormatted").ColumnName = "Audit Date"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("AuditDescription") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("AuditDescription").ColumnName = "Description"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("FindingNo") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("FindingNo").ColumnName = "Finding No."
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("Finding") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("Finding").ColumnName = "Findings"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("PriorityName") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("PriorityName").ColumnName = "Level"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("DeadLineDateFormatted") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("DeadLineDateFormatted").ColumnName = "Comp Due Date"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("RootCause") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("RootCause").ColumnName = "Root Cause"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("CAPA") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("CAPA").ColumnName = "Corrrective Action"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("PreventiveAction") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("PreventiveAction").ColumnName = "Preventive Action"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("AuditStatusName") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("AuditStatusName").ColumnName = "Audit Status"
            End If
            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("CorrectionDateFormatted") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("CorrectionDateFormatted").ColumnName = "Correction Date"
            End If

            If dsrptAuditFindings.Tables("rptAuditFindings").Columns.Contains("HeadOfQualityRemark") Then
                dsrptAuditFindings.Tables("rptAuditFindings").Columns("HeadOfQualityRemark").ColumnName = "Head Of Quality Remark"
            End If

            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Audit No.").SetOrdinal(0)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Audit Date").SetOrdinal(1)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Description").SetOrdinal(2)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Finding No.").SetOrdinal(3)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Findings").SetOrdinal(4)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Level").SetOrdinal(5)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Comp Due Date").SetOrdinal(6)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Root Cause").SetOrdinal(7)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Corrrective Action").SetOrdinal(8)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Preventive Action").SetOrdinal(9)
            dsrptAuditFindings.Tables("rptAuditFindings").Columns("Audit Status").SetOrdinal(10)

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsrptAuditFindings.Tables("ReportData"))
            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Merge(dsrptAuditFindings.Tables("rptAuditFindings"))
            dsNew.Tables("rptAuditFindings").TableName = "Audit Findings Report"
			Session("ExcelFileName") = "Audit Findings Report"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "AuditFindings", "Export To excel  " + mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
        '-------------------------------------------------------------------------------------------
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
    End Sub
    Public Sub setValues()
        Dim mAuditNo As String = ""
        Dim mDepartment As String = ""
        Dim mFindingStatus As String = ""

        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText

        If cmbAuditInfoList.SelectedIndex > 0 Then
            mAuditNo = cmbAuditInfoList.SelectedItem.Text
            lblAuditNo1.Text = "Audit No : " & mAuditNo
        Else
            mAuditNo = ""
            lblAuditNo1.Text = "Audit No : " & "All"
        End If

        If cmbDepartmentList.SelectedIndex > 0 Then
            mDepartment = cmbDepartmentList.SelectedItem.Text
            lblDepartment1.Text = "Department : " & mDepartment
        Else
            mAuditNo = ""
            lblDepartment1.Text = "Department : All"
        End If

        If cmbFindingStatus.SelectedIndex > 0 Then
            mFindingStatus = cmbFindingStatus.SelectedItem.Text
            lblFindingStatus1.Text = "Finding Status : " & mFindingStatus
        Else
            lblFindingStatus1.Text = "Finding Status : All"
        End If
        mEventLogDetails = lblDateRangeFrom.Text + ", " + lblAuditNo1.Text + ", " + lblDepartment1.Text + ", " + lblFindingStatus1.Text 'Added by Shital on 18-Jan-2021
    End Sub

#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mAuditExecutionAuditNoList = AuditExecutionAuditNoList.GetAuditExecutionAuditNoList("(ALL)")  'AuditExecutionList.GetAuditExecutionList("(SELECT)")
        cmbAuditInfoList.DataSource = mAuditExecutionAuditNoList
        Session("mAuditExecutionAuditNoList") = mAuditExecutionAuditNoList

        ' mDepartmentList = AuditDepartmentList.GetAuditDepartmentList("(ALL)")
        mResponsibleDepartmentList = EmployeeDepartmentList.GetEmployeeDepartmentList("(ALL)")
        cmbDepartmentList.DataSource = mResponsibleDepartmentList
        Session("mResponsibleDepartmentList") = mResponsibleDepartmentList

        mFindingStatusList = FindingStatusList.GetFindingStatusList("(ALL)")
        cmbFindingStatus.DataSource = mFindingStatusList
        Session("mFindingStatusList") = mFindingStatusList

        mAuditPriorityList = AuditPriorityList.GetAuditPriorityList("(ALL)")
        cmbFindingLevel.DataSource = mAuditPriorityList
        Session("mAuditPriorityList") = mAuditPriorityList
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        'If custValidator.ControlToValidate = "cmbAuditInfoList" Then
        '    If cmbAuditInfoList.SelectedIndex = 0 Then
        '        custValidator.ErrorMessage = "Please select the Audit No."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            setFocus(cmbAuditInfoList)
            Session("MiddleFrame") = "wfrptAuditFindings_Ajax.aspx?"
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            setValues()
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            setValues()
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblAuditNo1.Visible = True
        lblDepartment1.Visible = True
        lblDateRangeFrom.Visible = True
        lblFindingStatus1.Visible = True
        setValues()
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub chkSummary_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSummary.CheckedChanged
        If chkSummary.Checked Then
            cmbFormat.Enabled = False
            cmbFormat.SelectedIndex = 0
            btnExport.Enabled = True
        Else
            cmbFormat.Enabled = True
            cmbFormat.SelectedIndex = 0
            btnExport.Enabled = False
        End If
        upnlButton.Update()
    End Sub
#End Region


End Class