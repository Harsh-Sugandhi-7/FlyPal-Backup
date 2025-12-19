Public Class wfrptLoanInOutWardRecords_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Dim mDateSearchingCriteria As String = String.Empty
    Public mMachineNameValueList As MachineNameValueList
    Public mVendorList As VendorList
    Public mStoreList As StoreList
    Public mCustomerList As VendorList
    Public mCategoryList As CategoryList
    Public mWorkShopList As WorkShopList
    Public mStoreID As Guid
    Public mCustomerID As Guid
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Dim mLoanOutWardRecords As LoanOutWardRecords
    Dim mLoanInWardRecords As LoanInWardRecords
    Dim mCompanyDetail As New CompanyDetail
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        mCustomerList = CType(Session("mCustomerList"), VendorList)
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        lblFromDate.Text = "From Date : " & FromDate
        lblToDate.Text = "To Date     : " & ToDate
        lblCust.Text = "Customer : " & IIf(cmbCustomer.SelectedIndex > 0, cmbCustomer.SelectedItem.Text, "All")
        lblRecevingStore.Text = "Store : " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")
        lblSupp.Text = "Supplier : " & IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, "All")
        lblAir.Text = "Aircraft : " & IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "All")
        lblWoShop.Text = "WorkShop : " & IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "All")
        lblCate.Text = "Category : " & IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "All")
        lblPartNo.Text = "Part No. : " & PartNo
        lblPartDescription.Text = "Description : " & Description
        mDateSearchingCriteria = lblFromDate.Text.Trim + ", " + lblToDate.Text.Trim + ", " + lblCust.Text.Trim + ", " + lblRecevingStore.Text + ", " + lblSupp.Text + ", " + lblAir.Text + ", " + lblWoShop.Text + ", " + lblCate.Text + ", " + lblPartNo.Text + ", " + lblPartDescription.Text
    End Sub
    Private Sub ControlVisibility()
        lblSummary.Visible = True
        lblFromDate.Visible = True
        lblToDate.Visible = True
        lblCust.Visible = True
        lblRecevingStore.Visible = True
        lblSupp.Visible = True
        lblAir.Visible = True
        lblWoShop.Visible = True
        lblCate.Visible = True
        lblPartNo.Visible = True
        lblPartDescription.Visible = True
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Customer
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, False)
        cmbCustomer.DataSource = mCustomerList
        Session("mCustomerList") = mCustomerList
        'Store
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        'Category
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList

        'Vendor
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbSupplier.DataSource = mVendorList

        'Machine
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(All)")
        cmbAircraft.DataSource = mMachineNameValueList

        'WorkShop
        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(All)")
        cmbWorkShop.DataSource = mWorkShopList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack And Session("sender") = "" Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
        End If
    End Sub
    Private Sub cmbCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustomer.SelectedIndexChanged
        'Requested for Customer Stores  
        If chkCustomerStock.Checked Then
            If Not cmbCustomer.SelectedIndex <= 0 Then 'If Customer Selected
                mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID

                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
        End If
        cmbStore.DataBind()
        Session("mStoreList") = mStoreList
        If cmbCustomer.Enabled = True Then
            SetFocus(cmbCustomer)
        End If
        upnlStore.Update()
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then
            cmbCustomer.Enabled = True

            If Not cmbCustomer.SelectedIndex <= 0 Then                       'If Customer Selected
                mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            SetFocus(cmbCustomer)
        Else
            cmbCustomer.SelectedIndex = 0
            cmbCustomer.Enabled = False

            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)         'Self
            cmbStore.DataSource = mStoreList

            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            If cmbStore.Enabled = True Then
                SetFocus(cmbStore)
            End If
        End If
        upnlCustomer.Update()
        upnlStore.Update()
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(sender As Object, e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility()
        upnlSerachCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsLoanInOutWardRecords
        Dim da As New CSLA.Data.ObjectAdapter

        SetValues()

        If cmbFormat.SelectedValue = 1 Then   '1 LOAN - OUTWARD  2 LOAN - INWARD
            myReport = New crptLoanOutWardRecords
            mLoanOutWardRecords = LoanOutWardRecords.GetLoanOutWardRecords(FromDate, ToDate, cmbFormat.SelectedValue, chkCustomerStock.Checked, _
                                                                           cmbCustomer.SelectedValue.ToString, cmbStore.SelectedValue.ToString, _
                                                                           cmbAircraft.SelectedValue.ToString, cmbWorkShop.SelectedValue.ToString, _
                                                                           cmbSupplier.SelectedValue.ToString, chkShowInValuation.Checked, _
                                                                           cmbCategory.SelectedValue.ToString, PartNo, Description, AppSettings("ClientCode"))
        Else
            myReport = New crptLoanInWardRecords
            mLoanInWardRecords = LoanInWardRecords.GetLoanInWardRecords(FromDate, ToDate, cmbFormat.SelectedValue, chkCustomerStock.Checked, cmbCustomer.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbAircraft.SelectedValue.ToString, cmbWorkShop.SelectedValue.ToString, cmbSupplier.SelectedValue.ToString, chkShowInValuation.Checked, cmbCategory.SelectedValue.ToString, PartNo, Description)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbCustomer.SelectedIndex > 0, cmbCustomer.SelectedItem.Text, ""), SearchStr4:=IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), SearchStr5:=IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, ""), SearchStr7:=IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, ""), SearchStr8:=IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), SearchStr9:=PartNo, SearchStr10:=AppSettings("Logo"), SearchStr11:=Description)
        If cmbFormat.SelectedValue = 1 Then
            If (mLoanOutWardRecords.Count <= 0) Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1292)
            End If
        Else
            If (mLoanInWardRecords.Count <= 0) Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1292)
            End If
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        If cmbFormat.SelectedValue = 1 Then
            da.Fill(ds, mLoanOutWardRecords)
        Else
            da.Fill(ds, mLoanInWardRecords)
        End If
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "LoanInOutWardRecords", mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsExcelLoanInOutWardRecords
        SetValues()
        If cmbFormat.SelectedValue = 1 Then   '1 LOAN - OUTWARD  2 LOAN - INWARD
            mLoanOutWardRecords = LoanOutWardRecords.GetLoanOutWardRecords(FromDate, ToDate, cmbFormat.SelectedValue, chkCustomerStock.Checked, _
                                                                           cmbCustomer.SelectedValue.ToString, cmbStore.SelectedValue.ToString, _
                                                                           cmbAircraft.SelectedValue.ToString, cmbWorkShop.SelectedValue.ToString, _
                                                                           cmbSupplier.SelectedValue.ToString, chkShowInValuation.Checked, _
                                                                           cmbCategory.SelectedValue.ToString, PartNo, Description, AppSettings("ClientCode"))
        Else
            mLoanInWardRecords = LoanInWardRecords.GetLoanInWardRecords(FromDate, ToDate, cmbFormat.SelectedValue, chkCustomerStock.Checked, cmbCustomer.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbAircraft.SelectedValue.ToString, cmbWorkShop.SelectedValue.ToString, cmbSupplier.SelectedValue.ToString, chkShowInValuation.Checked, cmbCategory.SelectedValue.ToString, PartNo, Description)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbCustomer.SelectedIndex > 0, cmbCustomer.SelectedItem.Text, ""), SearchStr4:=IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), SearchStr5:=IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, ""), SearchStr7:=IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, ""), SearchStr8:=IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), SearchStr9:=PartNo, SearchStr10:=AppSettings("Logo"), SearchStr11:=Description)

        If cmbFormat.SelectedValue = 1 Then
            If (mLoanOutWardRecords.Count <= 0) Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1292)
            End If
        Else
            If (mLoanInWardRecords.Count <= 0) Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1292)
            End If
        End If

        ds.Clear()
        If cmbFormat.SelectedValue = 1 Then
            da.Fill(ds, mLoanOutWardRecords)
        Else
            da.Fill(ds, mLoanInWardRecords)
        End If
        da.Fill(ds, Report)

        Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
            End If
        Next
        Dim columnToRemove As String()
        If cmbFormat.SelectedValue = 1 Then
            columnToRemove = {"IssueDate", "ReceiptDate", "EffectiveRate", "ReceiptFrom", "ReceiptNo"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("LoanOutWardRecords").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("LoanOutWardRecords").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("LoanOutWardRecords").Columns.Contains("IssueDateFormatted") Then
                ds.Tables("LoanOutWardRecords").Columns("IssueDateFormatted").ColumnName = "Date"
            End If
            If ds.Tables("LoanOutWardRecords").Columns.Contains("ItemName") Then
                ds.Tables("LoanOutWardRecords").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("LoanOutWardRecords").Columns.Contains("ItemDescription") Then
                ds.Tables("LoanOutWardRecords").Columns("ItemDescription").ColumnName = "Description"
            End If
            If ds.Tables("LoanOutWardRecords").Columns.Contains("IssueTo") Then
                ds.Tables("LoanOutWardRecords").Columns("IssueTo").ColumnName = "To Whom"
            End If
            If ds.Tables("LoanOutWardRecords").Columns.Contains("IssueItemQty") Then
                ds.Tables("LoanOutWardRecords").Columns("IssueItemQty").ColumnName = "Qty."
            End If
            If ds.Tables("LoanOutWardRecords").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("LoanOutWardRecords").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
            End If
        Else
            columnToRemove = {"ReceiptDate", "IssueDate", "EffectiveRate", "IssueNo", "IssueTo"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("LoanInWardRecords").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("LoanInWardRecords").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("LoanInWardRecords").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("LoanInWardRecords").Columns("ReceiptDateFormatted").ColumnName = "Date"
            End If
            If ds.Tables("LoanInWardRecords").Columns.Contains("ItemName") Then
                ds.Tables("LoanInWardRecords").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("LoanInWardRecords").Columns.Contains("ItemDescription") Then
                ds.Tables("LoanInWardRecords").Columns("ItemDescription").ColumnName = "Description"
            End If
            If ds.Tables("LoanInWardRecords").Columns.Contains("ReceiptFrom") Then
                ds.Tables("LoanInWardRecords").Columns("ReceiptFrom").ColumnName = "From Whom"
            End If
            If ds.Tables("LoanInWardRecords").Columns.Contains("ReceiptItemQty") Then
                ds.Tables("LoanInWardRecords").Columns("ReceiptItemQty").ColumnName = "Qty."
            End If
            If ds.Tables("LoanInWardRecords").Columns.Contains("IssueDateFormatted") Then
                ds.Tables("LoanInWardRecords").Columns("IssueDateFormatted").ColumnName = "Issue Date"
            End If
        End If

        If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
            ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
            ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
            ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Customer"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
            ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Store"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
            ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Supplier"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
            ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Aircraft"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr7") Then
            ds.Tables("ReportData").Columns("SearchStr7").ColumnName = "Work Shop"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
            ds.Tables("ReportData").Columns("SearchStr8").ColumnName = "Category"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr9") Then
            ds.Tables("ReportData").Columns("SearchStr9").ColumnName = "Part No."
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr11") Then
            ds.Tables("ReportData").Columns("SearchStr11").ColumnName = "Description"
        End If
        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Tables("ReportData").TableName = "Searching Criteria"

		If cmbFormat.SelectedValue = 1 Then
			dsNew.Merge(ds.Tables("LoanOutWardRecords"))
			dsNew.Tables("LoanOutWardRecords").TableName = "LOAN - OUTWARD"
			Session("ExcelFileName") = "LOAN - OUTWARD"
		Else
			dsNew.Merge(ds.Tables("LoanInWardRecords"))
			dsNew.Tables("LoanInWardRecords").TableName = "LOAN - INWARD"
			Session("ExcelFileName") = "LOAN - INWARD"
		End If

		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "LoanInOutWardRecords", "Export To excel " + mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
#End Region

End Class