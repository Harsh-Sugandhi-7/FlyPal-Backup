Public Class wfrptConsumption_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCategoryList As CategoryList
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String
    Public Description As String
    Public StrAircraft As String
    Public StrCategory As String
    Dim mTransTypeID As Integer
    Dim Supplier As String = ""
    Dim Store As String = ""
    Dim ToStore As String = ""
    Dim WorkShop As String = ""
    Dim WorkOrderText As String = ""
    Dim WorkOrderNo As String = ""
    Dim mStoreList As StoreList
    Public Shadows Title As String
    Dim EventLogID As Guid
    Dim mSearchingCriteria As String = String.Empty
    Public mCustomerList As VendorList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mTransTypeID = CType(Session("mTransTypeID"), Int16)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mTransTypeID")
    End Sub
    Private Sub Controlvisibility(ByVal Index As Int16)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
        lblDateRangeFrom.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblCategoryName.Visible = False
    End Sub
    Private Sub addattributes()
        txtWONo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtWONo').value,event)")
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
    Private Sub SetValues()
        mTransTypeID = CType(cmbIssue.SelectedValue, Int16)
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range  : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range  : " & New SmartDate(txtFromDate.Text).FormattedText & " To Date : " & New SmartDate(txtToDate.Text).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

        If txtAircraft.Text.Trim = "" Then
            StrAircraft = ""
        Else
            StrAircraft = txtAircraft.Text.Trim
        End If

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        If cmbCategory.SelectedIndex = 0 Then
            StrCategory = ""
            lblCategoryName.Text = "Category Name : All"
        Else
            StrCategory = cmbCategory.SelectedItem.Text
            lblCategoryName.Text = "Category Name : " & StrCategory
        End If
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")

        ToStore = IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, "") 'Added By Prashant 29-Apr-2013 'ALL29042013-4
        Store = IIf(cmbType.SelectedIndex = 3 And cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "") 'Added By Prashant 29-Apr-2013 'ALL29042013-4
        WorkShop = IIf(cmbType.SelectedIndex = 5 And txtWorkShop.Text <> "", txtWorkShop.Text, "")
        WorkOrderNo = IIf(cmbType.SelectedIndex = 6, txtWONo.Text.Trim, "")
        WorkOrderText = IIf(cmbType.SelectedIndex = 6 And txtWorkOrder.Text <> "", txtWorkOrder.Text, "")
        lblFromStore.Text = "From Store    : " & IIf(ToStore <> "", ToStore, "All")
        If cmbType.SelectedItem.Text = "Customer" Then
            Supplier = txtCustomer.Text.Trim
            lblVendor.Text = IIf(mTransTypeID = 25 Or mTransTypeID = 26 Or mTransTypeID = 78, "Customer : " & Supplier, "Supplier : " & Supplier)
        ElseIf cmbType.SelectedItem.Text = "Supplier" Then
            Supplier = txtSupplier.Text.Trim
        End If
        Select Case cmbType.SelectedIndex
            Case 0
                lblVendor.Text = "To Type : All"
            Case 1 'Vendor
                lblVendor.Text = IIf(mTransTypeID = 25 Or mTransTypeID = 26 Or mTransTypeID = 78, "Customer : " & IIf(Supplier <> "", Supplier, "All"), "Supplier : " & IIf(Supplier <> "", Supplier, "All"))
            Case 2 'Aircraft
                lblVendor.Text = "Aircraft : " & IIf(StrAircraft <> "", StrAircraft, "All")
            Case 3 'Store
                lblVendor.Text = "Store : " & IIf(Store <> "", Store, "All")
            Case 4 'Discard
                lblVendor.Text = "Discard "
            Case 5  'WorkShop
                lblVendor.Text = "WorkShop : " & IIf(WorkShop <> "", WorkShop, "All")
            Case 6  'WorkOrder
                If WorkOrderNo <> "" Then
                    lblVendor.Text = "WorkOrder : " & IIf(WorkOrderText <> "", WorkOrderText & "-" & WorkOrderNo, "All")
                Else
                    lblVendor.Text = "WorkOrder : " & IIf(WorkOrderText <> "", WorkOrderText, "All")
                End If
        End Select
        mSearchingCriteria = lblDateRangeFrom.Text + ", " + lblCategoryName.Text + ", " + lblVendor.Text.ToString + ", " + lblFromStore.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Private Function GetTitle(Optional ByVal Value As String = "") As String
        Dim mTransTypeList As TransactionList
        Dim mTitle As String
        mTransTypeList = TransactionList.GetTransactionList()
        mTransTypeID = CType(cmbIssue.SelectedValue, Int16)
        mTransTypeList = TransactionList.GetTransactionList("Issue")     'Added By Prashant 24/09/07
        mTitle = mTransTypeList.GetTransactionTypeName(cmbIssue.SelectedValue).ToString + " Consumption Report " + " (" + Value + ")"
        Return mTitle
    End Function
    Private Sub SetReport(ByVal IsExcel As Boolean)
        ' Session("IsExcel") = IsExcel
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteria
        Dim ds As New dsConsumption
        Dim rptConsumption As rptConsumption
        myReport = New crptConsumption
        Dim value As String = ""

        If rdoBase.Checked = True Then                         'Added By Prashant 18-Dec-2012 All18122012
            value = "Base Value"
        ElseIf rdoLanding.Checked = True Then
            value = "Landing Value"
        Else
            value = "Commercial Value"
        End If
        Title = GetTitle(value)
        If cmbType.SelectedIndex = 1 Then
            rptConsumption = rptConsumption.GetConsumption(FromDate, ToDate, StrCategory, Store, Supplier, StrAircraft, 1, cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText, Val(WorkOrderNo), PartNo, chkIsValued.Checked, value, Format:=CInt(cmbFormat.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, CustomerID:=cmbCustomer.SelectedValue.ToString)
        End If
        If cmbType.SelectedIndex = 2 Then 'Aircraft
            rptConsumption = rptConsumption.GetConsumption(FromDate, ToDate, StrCategory, Store, Supplier, StrAircraft, 2, cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText, Val(WorkOrderNo), PartNo, chkIsValued.Checked, value, Format:=CInt(cmbFormat.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, CustomerID:=cmbCustomer.SelectedValue.ToString)
        End If
        If cmbType.SelectedIndex = 3 Then 'Store
            rptConsumption = rptConsumption.GetConsumption(FromDate, ToDate, StrCategory, Store, Supplier, StrAircraft, 8, cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText, Val(WorkOrderNo), PartNo, chkIsValued.Checked, value, Format:=CInt(cmbFormat.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, CustomerID:=cmbCustomer.SelectedValue.ToString)
        End If
        If cmbType.SelectedIndex = 4 Then
            rptConsumption = rptConsumption.GetConsumption(FromDate, ToDate, StrCategory, Store, Supplier, StrAircraft, 7, cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText, Val(WorkOrderNo), PartNo, chkIsValued.Checked, value, Format:=CInt(cmbFormat.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, CustomerID:=cmbCustomer.SelectedValue.ToString)
        End If
        If cmbType.SelectedIndex <> 1 And cmbType.SelectedIndex <> 2 And cmbType.SelectedIndex <> 3 And cmbType.SelectedIndex <> 4 And cmbType.SelectedIndex <> 5 And cmbType.SelectedIndex <> 6 Then
            rptConsumption = rptConsumption.GetConsumption(FromDate, ToDate, StrCategory, Store, Supplier, StrAircraft, 0, cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText, Val(WorkOrderNo), PartNo, chkIsValued.Checked, value, Format:=CInt(cmbFormat.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, CustomerID:=cmbCustomer.SelectedValue.ToString)
        End If
        If cmbType.SelectedIndex = 5 Then 'Workshop
            rptConsumption = rptConsumption.GetConsumption(FromDate, ToDate, StrCategory, Store, Supplier, StrAircraft, 16, cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText, Val(WorkOrderNo), PartNo, chkIsValued.Checked, value, Format:=CInt(cmbFormat.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, CustomerID:=cmbCustomer.SelectedValue.ToString)
        End If
        If cmbType.SelectedIndex = 6 Then 'WorkOrder
            rptConsumption = rptConsumption.GetConsumption(FromDate, ToDate, StrCategory, Store, Supplier, StrAircraft, 17, cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText, Val(WorkOrderNo), PartNo, chkIsValued.Checked, value, Format:=CInt(cmbFormat.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, CustomerID:=cmbCustomer.SelectedValue.ToString)
        End If

        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, Supplier, Title, StrCategory, "", _
                                                              Store, StrAircraft, value, Description, AppSettings("Logo"), CType(cmbIssue.SelectedValue, Int16), ToStore, _
                                                              WorkShop, WorkOrderText, WorkOrderNo, Search1:=cmbFormat.SelectedValue, Search2:=txtBottomLine.Text.Trim, _
                                                              Search3:=IIf(cmbCustomer.SelectedIndex > 0, cmbCustomer.SelectedItem.Text, ""), Search4:=AppSettings("ClientCode"))

        If rptConsumption.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1112)
        End If
        'MarkLog(Util.Action.Print, "ConsumptionReport", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)'Commented by Shital on 18-Jan-2021
        If IsExcel = False Then     'PDF format
            Dim mrptImage As rptImage = rptImage.GetImage(ds)

            da.Fill(ds, "rptImage", mrptImage)
            da.Fill(ds, "rptSearchingCriteria", objSearch)
            da.Fill(ds, "rptConsumption", rptConsumption)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "ConsumptionReport", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            ds.Clear()
            da.Fill(ds, "rptSearchingCriteria", objSearch)
            da.Fill(ds, "rptConsumption", rptConsumption)

            Dim columnToRemove2 As String()
            If cmbDateRange.SelectedIndex = 0 Then
                columnToRemove2 = {"ReportDate", "CompanyName", "BranchName", "Nomenclature", "KitName", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "Search1", "Search2", "Search2", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "RelNoteNo", "FromDate", "ToDate"}
            Else
                columnToRemove2 = {"ReportDate", "CompanyName", "BranchName", "Nomenclature", "KitName", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "Search1", "Search2", "Search2", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "RelNoteNo"}
            End If
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            'If ds.Tables("rptSearchingCriteria").Columns.Contains("SearchStr1") Then
            '    ds.Tables("rptSearchingCriteria").Columns("SearchStr1").ColumnName = "From Date"
            'End If
            'If ds.Tables("rptSearchingCriteria").Columns.Contains("SearchStr2") Then
            '    ds.Tables("rptSearchingCriteria").Columns("SearchStr2").ColumnName = "To Date"
            'End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Search3") Then
                ds.Tables("rptSearchingCriteria").Columns("Search3").ColumnName = "Customer"
            End If
            'If ds.Tables("rptSearchingCriteria").Columns.Contains("SearchStr4") Then
            '    ds.Tables("rptSearchingCriteria").Columns("SearchStr4").ColumnName = "Part No."
            'End If

            Dim RaisedIndentSparesStatusToRemove As String() = {"AircraftName", "TotalAmount", "CategoryTotal", "FromStoreName", "ToTypeName",
                                                                "RequisitionItemTypeID", "ScheduleTotal", "UnScheduleTotal", "ReceiptText", "ReceiptNo",
                                                                "ReceiptDate"}

            For i As Integer = 0 To RaisedIndentSparesStatusToRemove.Length - 1
                If ds.Tables("rptConsumption").Columns.Contains(RaisedIndentSparesStatusToRemove(i)) Then
                    ds.Tables("rptConsumption").Columns.Remove(RaisedIndentSparesStatusToRemove(i))
                End If
            Next

            If ds.Tables("rptConsumption").Columns.Contains("IssueToName") Then
                ds.Tables("rptConsumption").Columns("IssueToName").ColumnName = "Issue To"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("CategoryName") Then
                ds.Tables("rptConsumption").Columns("CategoryName").ColumnName = "Category"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("PartName") Then
                ds.Tables("rptConsumption").Columns("PartName").ColumnName = "Part Number"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("PartDescription") Then
                ds.Tables("rptConsumption").Columns("PartDescription").ColumnName = "Description"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("IssueNo") Then
                ds.Tables("rptConsumption").Columns("IssueNo").ColumnName = "Issue No."
            End If
            If ds.Tables("rptConsumption").Columns.Contains("IssueDate") Then
                ds.Tables("rptConsumption").Columns("IssueDate").ColumnName = "Issue Date"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("ReferenceNo") Then
                ds.Tables("rptConsumption").Columns("ReferenceNo").ColumnName = "Reference No."
            End If

            If ds.Tables("rptConsumption").Columns.Contains("SupplierInvoiceNo") Then
                ds.Tables("rptConsumption").Columns("SupplierInvoiceNo").ColumnName = "Supplier Invoice No."
            End If
            If ds.Tables("rptConsumption").Columns.Contains("SupplierInvoiceDate") Then
                ds.Tables("rptConsumption").Columns("SupplierInvoiceDate").ColumnName = "Supplier Invoice Date"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("IssueQty") Then
                ds.Tables("rptConsumption").Columns("IssueQty").ColumnName = "Qty."
            End If

            If ds.Tables("rptConsumption").Columns.Contains("SerialNo") Then
                ds.Tables("rptConsumption").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("rptConsumption").Columns.Contains("RequisitionItemTypeName") Then
                ds.Tables("rptConsumption").Columns("RequisitionItemTypeName").ColumnName = "Maintenance Type"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("ReceiptNumber") Then
                ds.Tables("rptConsumption").Columns("ReceiptNumber").ColumnName = "Receipt Number"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("rptConsumption").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
            End If
            If ds.Tables("rptConsumption").Columns.Contains("UpdateDateTimeStamp") Then
                ds.Tables("rptConsumption").Columns("UpdateDateTimeStamp").ColumnName = "Update Date"
            End If

            ds.Tables("rptConsumption").Columns("Issue To").SetOrdinal(0)
            ds.Tables("rptConsumption").Columns("Category").SetOrdinal(1)
            ds.Tables("rptConsumption").Columns("Part Number").SetOrdinal(2)
            ds.Tables("rptConsumption").Columns("Description").SetOrdinal(3)
            ds.Tables("rptConsumption").Columns("Issue No.").SetOrdinal(4)
            ds.Tables("rptConsumption").Columns("Issue Date").SetOrdinal(5)
            ds.Tables("rptConsumption").Columns("Reference No.").SetOrdinal(6)
            ds.Tables("rptConsumption").Columns("Supplier Invoice No.").SetOrdinal(7)
            ds.Tables("rptConsumption").Columns("Supplier Invoice Date").SetOrdinal(8)
            ds.Tables("rptConsumption").Columns("Qty.").SetOrdinal(9)
            ds.Tables("rptConsumption").Columns("Serial No.").SetOrdinal(10)
            ds.Tables("rptConsumption").Columns("Maintenance Type").SetOrdinal(11)

            Dim dtCloned As DataSet = ds.Clone()
            dtCloned.Tables("rptConsumption").Columns("EffRate").DataType = GetType(Decimal)
            dtCloned.Tables("rptConsumption").Columns("Amount").DataType = GetType(Decimal)
            For Each row As DataRow In ds.Tables("rptConsumption").Rows
                dtCloned.Tables("rptConsumption").ImportRow(row)
            Next

            If dtCloned.Tables("rptConsumption").Columns.Contains("EffRate") Then
                dtCloned.Tables("rptConsumption").Columns("EffRate").ColumnName = value
            End If
            'Amount
            If dtCloned.Tables("rptConsumption").Columns.Contains("Amount") Then
                dtCloned.Tables("rptConsumption").Columns("Amount").ColumnName = "Amount In" & " " & objSearch(0).CurrencySymbol
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Merge(dtCloned.Tables("rptConsumption"))
            dsNew.Tables("rptConsumption").TableName = Title
			Session("ExcelFileName") = Title
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "ConsumptionReport", "Export To excel " + mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub SetIssueCombo()
        Me.cmbIssue.Items.Clear()

        cmbIssue.Items.Add(New ListItem("(All)", "00"))
        If User.IsInRole("IssueToAircraftView") Then cmbIssue.Items.Add(New ListItem("To Aircraft", "14"))
        If User.IsInRole("IssueToStoreView") Then cmbIssue.Items.Add(New ListItem("To Store", "15"))
        If User.IsInRole("IssueToCustomerView") Then cmbIssue.Items.Add(New ListItem("To Customer", "25"))
        If User.IsInRole("IssueToCustomerView") Then cmbIssue.Items.Add(New ListItem("Loan To Customer", "26"))
        If User.IsInRole("IssueLoanToStoreView") Then cmbIssue.Items.Add(New ListItem("Loan To Another Store", "17"))
        If User.IsInRole("IssueLoanToAircraftView") Then cmbIssue.Items.Add(New ListItem("Loan To Aircraft", "20"))
        If User.IsInRole("LoanIssueToVendorView") Then cmbIssue.Items.Add(New ListItem("Loan To Supplier", "24"))
        If User.IsInRole("IssueLoanReturnToStoreView") Then cmbIssue.Items.Add(New ListItem("Loan Return To Store", "18"))
        If User.IsInRole("IssueToVendorForExchangeView") Then cmbIssue.Items.Add(New ListItem("To Supplier For Exchange/Repair", "16"))
        If User.IsInRole("IssueToDiscardView") Then cmbIssue.Items.Add(New ListItem("Part Discard", "19"))
        If User.IsInRole("IssueToWorkShopView") Then cmbIssue.Items.Add(New ListItem("To WorkShop", "44"))
        If User.IsInRole("IssueLoanToWorkShopView") Then cmbIssue.Items.Add(New ListItem("Loan To WorkShop", "45"))
        If User.IsInRole("IssueforLoanReturntoSupplierView") Then cmbIssue.Items.Add(New ListItem("Loan Return to Supplier", "49"))
        If User.IsInRole("IssueforLoanReturntoCustomerView") Then cmbIssue.Items.Add(New ListItem("Loan Return to Customer", "51"))
        If User.IsInRole("IssueToWorkOrderView") Then cmbIssue.Items.Add(New ListItem("To WorkOrder", "52"))
        If User.IsInRole("IssuetoSupplierasRentalLeaseView") Then cmbIssue.Items.Add(New ListItem("Supplier As Rental/Lease", "55"))
        'Added By Prashant 28-Dec-2010
        If User.IsInRole("IssueToWorkOrderAsSparesView") Then cmbIssue.Items.Add(New ListItem("To WorkOrder As Spares", "59"))
        If User.IsInRole("IssueToWorkOrderAsToolsView") Then cmbIssue.Items.Add(New ListItem("To WorkOrder As Tools", "60"))
        '-----------------------------
        If User.IsInRole("IssuetoSupplierNoneView") Then cmbIssue.Items.Add(New ListItem("To Supplier As None", "63")) 'Added By Prashant 18-Dec-2012 'All18122012
        If User.IsInRole("IssueToCustomerAsNoneNew") Then cmbIssue.Items.Add(New ListItem("To Customer As None", "78"))
        If mTransTypeID = 0 Then
            lblStep3.Text = "Step III. Selection of All"
        End If
    End Sub
    Private Sub SetVendor()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Supplier", "1"))
        cmbType.Items.Add(New ListItem("Aircraft", "2"))
        cmbType.Items.Add(New ListItem("Store", "8"))
        cmbType.Items.Add(New ListItem("Discard", "7"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
        cmbType.Items.Add(New ListItem("WorkOrder", "17"))
    End Sub
    Private Sub SetCustomer()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Customer", "1"))
        cmbType.Items.Add(New ListItem("Aircraft", "2"))
        cmbType.Items.Add(New ListItem("Store", "8"))
        cmbType.Items.Add(New ListItem("Discard", "7"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
        cmbType.Items.Add(New ListItem("WorkOrder", "17"))
    End Sub
    Private Sub SetTitle()
        cmbType.Enabled = False
        txtWONo.Text = ""
        txtSupplier.Text = ""
        txtCustomer.Text = ""
        txtAircraft.Text = ""
        cmbStore.SelectedIndex = 0 'Added By Prashant 29-Apr-2013 'ALL29042013-4
        txtWorkShop.Text = ""
        txtWorkOrder.Text = ""
        Dim Index As Int16 = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedIndex, 0)
        lblType1.Visible = (Index > 0)
        lblType1.Text = IIf(Index = 0, "", IIf(Index = 1, IIf(mTransTypeID = 26 Or mTransTypeID = 25 Or mTransTypeID = 51 Or mTransTypeID = 78, "Customer ", "Supplier  "), IIf(Index = 2, "Aircraft  ", IIf(Index = 3, "Store  ", IIf(Index = 6, "WorkOrder  ", IIf(Index = 5, "WorkShop  ", ""))))))
        txtCustomer.Visible = IIf(cmbType.SelectedItem.Text = "Customer", True, False)
        txtSupplier.Visible = IIf(cmbType.SelectedItem.Text = "Supplier", True, False)
        txtAircraft.Visible = (Index = 2)
        cmbStore.Visible = (Index = 3) 'Added By Prashant 29-Apr-2013 'ALL29042013-4
        txtWorkShop.Visible = (Index = 5)
        txtWONo.Visible = False
        txtWorkOrder.Visible = (Index = 6)
        txtWONo.Visible = (Index = 6)
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList

        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        cmbFromStore.DataSource = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        'Customer
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, False)
        cmbCustomer.DataSource = mCustomerList

        Session("mCategoryList") = mCategoryList
        Session("mStoreList") = mStoreList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addattributes()
        If Not IsPostBack Then
            setFocus(cmbIssue)
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            DataFieldBind()
            Controlvisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            SetTitle()
            SetIssueCombo()
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        Controlvisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRangeFrom.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblVendor.Visible = True
        lblCategoryName.Visible = True
        lblFromStore.Visible = True
        SetValues()
        upnlCurrentSearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mCategoryList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbIssue_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbIssue.SelectedIndexChanged
        addattributes()
        If cmbIssue.Enabled = True Then
            setFocus(cmbIssue)
        End If
        mTransTypeID = CType(cmbIssue.SelectedValue, Int16)
        Select Case (mTransTypeID)
            Case 0
                lblStep3.Text = "Step III. Selection of All"
                cmbType.SelectedIndex = 0
            Case 14
                lblStep3.Text = "Step III. Selection of Aircraft"
                cmbType.SelectedIndex = 2
            Case 15
                lblStep3.Text = "Step III. Selection of Store"
                cmbType.SelectedIndex = 3
            Case 16
                lblStep3.Text = "Step III. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 17
                lblStep3.Text = "Step III. Selection of Store"
                cmbType.SelectedIndex = 3
            Case 18
                lblStep3.Text = "Step III. Selection of Store"
                cmbType.SelectedIndex = 3
            Case 19
                lblStep3.Text = "Step III. Discard"
                cmbType.SelectedIndex = 4
            Case 20
                lblStep3.Text = "Step III. Selection of Aircraft"
                cmbType.SelectedIndex = 2
            Case 24
                lblStep3.Text = "Step III. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 25, 78
                lblStep3.Text = "Step III. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 26
                lblStep3.Text = "Step III. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 44
                lblStep3.Text = "Step III. Selection of WorkShop"
                cmbType.SelectedIndex = 5
            Case 45
                lblStep3.Text = "Step III. Selection of WorkShop"
                cmbType.SelectedIndex = 5
            Case 49
                lblStep3.Text = "Step III. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 51
                lblStep3.Text = "Step III. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 52
                lblStep3.Text = "Step III. Selection of WorkOrder"
                cmbType.SelectedIndex = 6
            Case 55
                lblStep3.Text = "Step III. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
                'Added By Prashant 28-Dec-2010
            Case 59
                lblStep3.Text = "Step III. Selection of WorkOrder"
                cmbType.SelectedIndex = 6
            Case 60
                lblStep3.Text = "Step III. Selection of WorkOrder"
                cmbType.SelectedIndex = 6
                '-----------------------------
            Case 63
                lblStep3.Text = "Step III. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
        End Select
        txtAircraft.Text = ""
        txtSupplier.Text = ""
        txtCustomer.Text = ""
        txtWorkOrder.Text = ""
        txtWorkShop.Text = ""
        SetTitle()
    End Sub
    Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedValue = 1 Then
            lblGROValuesInfo.Visible = True
        Else
            lblGROValuesInfo.Visible = False
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub cmbCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustomer.SelectedIndexChanged
        'Requested for Customer Stores  
        If chkCustomerStock.Checked Then
            If Not cmbCustomer.SelectedIndex <= 0 Then 'If Customer Selected
                'mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
                mStoreList = StoreList.GetStoreList(New Guid(cmbCustomer.SelectedValue.ToString), "(All)", True)    'Passing selected customer 
                cmbFromStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbFromStore.DataSource = mStoreList
            End If
        End If
        cmbFromStore.DataBind()
        Session("mStoreList") = mStoreList
        If cmbCustomer.Enabled = True Then
            setFocus(cmbCustomer)
        End If
        upnlFromStore.Update()
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then
            lblCustomer.Enabled = True
            cmbCustomer.Enabled = True

            If Not cmbCustomer.SelectedIndex <= 0 Then                       'If Customer Selected
                'mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
                mStoreList = StoreList.GetStoreList(New Guid(cmbCustomer.SelectedValue.ToString), "(All)", True)    'Passing selected customer 
                cmbFromStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbFromStore.DataSource = mStoreList
            End If
            cmbFromStore.DataBind()
            Session("mStoreList") = mStoreList
            setFocus(cmbCustomer)
        Else
            cmbCustomer.SelectedIndex = 0
            lblCustomer.Enabled = False
            cmbCustomer.Enabled = False

            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)         'Self
            cmbFromStore.DataSource = mStoreList

            cmbFromStore.DataBind()
            Session("mStoreList") = mStoreList
            If cmbStore.Enabled = True Then
                setFocus(cmbFromStore)
            End If
        End If
        upnlFromStore.Update()
    End Sub
#End Region

End Class