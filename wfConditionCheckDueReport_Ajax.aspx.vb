Public Class wfConditionCheckDueReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public PartNo As String = ""
    Public Description As String = ""
    Public mStoreID As String
    Public ToDate, RangeDate As String
    Public Search1, Search2, Search3, Search4, Search5, mSearchingCriteria As String
    Public mCategoryLists As CategoryList
    Public StrCategory As String = String.Empty
    Dim mCategoryID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList)
    End Sub
    Private Sub SetSession()
        Session("mStoreList") = mStoreList
        Session("mCategoryLists") = mCategoryLists
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mStoreList")
        Session.Remove("mCategoryLists")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility2()
        lblDateRange.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblCategoryName.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If

        If txtAsOnDate.Text = "" Then
            ToDate = "1/1/3050"
            lblDateRange.Text = "Date Range  : All"
            lblDateRange.Visible = False
            Search2 = ""
        Else
            ToDate = txtAsOnDate.Text
            lblDateRange.Text = "Date : " & txtAsOnDate.Text
            lblDateRange.Visible = True
            Search2 = "As On Date : " & txtAsOnDate.Text
        End If

        If cmbRange.SelectedIndex = 0 Then         'All
            RangeDate = New SmartDate("1/1/3300").FormattedText
            Search1 = ""
        ElseIf cmbRange.SelectedIndex = 1 Then     '1 Month
            RangeDate = New SmartDate(txtAsOnDate.Text).Date.AddMonths(1).ToShortDateString
            Search1 = "Range : " & cmbRange.SelectedItem.Text
        ElseIf cmbRange.SelectedIndex = 2 Then     '1 Quater
            'RangeDate = New SmartDate(txtDate.Value.ToString).Date.AddMonths(3).ToShortDateString
            RangeDate = New SmartDate(txtAsOnDate.Text).Date.AddMonths(2).ToShortDateString
            Search1 = "Range : " & cmbRange.SelectedItem.Text
        ElseIf cmbRange.SelectedIndex = 3 Then     '1 Year
            RangeDate = New SmartDate(txtAsOnDate.Text).Date.AddYears(1).ToShortDateString
            Search1 = "Range : " & cmbRange.SelectedItem.Text
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        If PartNo = "" Then
            Search3 = ""
        Else
            Search3 = "Part No. : " & PartNo
        End If
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        If Description = "" Then
            Search4 = ""
        Else
            Search4 = "Description : " & Description
        End If

        If StoreID.Value = String.Empty Or StoreID.Value.Equals(Guid.Empty.ToString) Then
            mStoreID = Guid.Empty.ToString
            'Search5 = ""
        Else
            mStoreID = StoreID.Value.ToString
            'Search5 = "Store : " & StoreName.Value.ToString
        End If

        'lblStores.Text = IIf(Search5 <> "", Search5, "Store : All")

        If cmbCategory.SelectedIndex = 0 Then
            StrCategory = ""
            mCategoryID = Guid.Empty
            lblCategoryName.Text = "Category Name : All"
        Else
            StrCategory = "Category : " & cmbCategory.SelectedItem.ToString
            mCategoryID = New Guid(cmbCategory.SelectedValue)
            lblCategoryName.Text = StrCategory
        End If

        mSearchingCriteria = lblDateRange.Text + ", " + Search1 + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblCategoryName.Text
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        'Session("IsExcel") = IsExcel
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim Obj As ConditionCheckDueReport
        Dim mCompanyDetail As New CompanyDetail
        SetValues()

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Condition Check Due Report", Search1, Search2, Search3, Search4, Search5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:=StrCategory, SearchStr10:=AppSettings("Logo"))

        Obj = ConditionCheckDueReport.GetConditionCheckDueReport(, ToDate, PartNo, Description, "", mStoreID, RangeDate, mCategoryID.ToString, IsConditionCheckServicedInspected:=1)

        If Obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If IsExcel = False Then 'If PDF format
            Dim ds As New dsConditionCheckDueReport
            ds.Clear()
            da.Fill(ds, Obj)
            da.Fill(ds, Report)
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            myReport = New crConditionCheckDueReport
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "ConditionCheckDueReport", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcelConditionCheckDueReport
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "ConditionCheckDueReport", Obj)
            Dim columnToRemove As String() = {"OrderDate", "CalibrationStandard", "CondCheckInterval", "IsConditionCheck", "IsServicedInspected", "ConditionCheckServicedInspected"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ConditionCheckDueReport").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ConditionCheckDueReport").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("ConditionCheckDueReport").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("ConditionCheckDueReport").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("ConditionCheckDueReport").Columns.Contains("Frequency") Then
                ds.Tables("ConditionCheckDueReport").Columns("Frequency").ColumnName = "Interval"
            End If
            Dim columnToRemove2 As String() = {"ID", "SearchStr5", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Range"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "As On Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Part No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Description"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Category"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("ReportData").TableName = "Searching Criteria"
            ds.Tables("ConditionCheckDueReport").TableName = "Condition Check Due Report"
            dsNew = ds
            Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)

            MarkLog(Util.Action.Print, "ConditionCheckDueReport", mSearchingCriteria + IIf(IsExcel = True, " Export To excel", ""), Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryLists = CategoryList.GetCategoryList("(ALL)")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            txtAsOnDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then SetReport(True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

End Class