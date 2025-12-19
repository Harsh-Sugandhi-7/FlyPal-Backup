
'Created : Saylee
'Dates   : 3-Feb-2014



Public Class wfrptGROExpenseReport_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ToDate As String = ""
    Dim RecText As String = ""
    Dim RecNo As String = ""
    Dim InternalReceiptNo As String = ""
    Dim Aircraft As String = ""
    Dim Store As String = ""
    Dim DCNo As String = ""
    Dim Status As String = ""
    Dim ReceiptCumInvoice As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim OrdNo As String = ""
    Dim OrdText As String = ""
    Dim IssNo As String = ""
    Dim IssText As String = ""
    Dim InvNo As String = ""
    Dim InvText As String = ""
    Dim ReleaseNoteNo As String = ""
    Dim Fromdate As String
    Dim mTransTypeID As Int16
    Dim Tital As String
    Dim WorkShop As String = ""
    Dim ReceivingStoreID As String
    Dim ReceivingStore As String
    Dim CustomBillofEntry As String = ""
    Public mPartTypeList As PartTypeList
    Dim mPartType As Integer
    Dim mPartTypeName As String = ""
    'Added By Prashant 28-Dec-2010
    Dim WorkOrderText As String = ""
    Dim WorkOrderNo As String = ""
    Dim mDistinctWOText As nDistinctWOText
    '-----------------------------

    'Added By Utkarsh ON 21-Dec-2011 FOR ALL13122011
    Public Type As String = ""
    Public TextType As String = ""
    'End
    Dim mCompanyDetail As New CompanyDetail
    Dim mSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Dim Supplier As String = String.Empty
    Public mCustomerList As VendorList
    Dim SupplierID As Guid
    Public mModelList As ModelList
    Public ModelName As String = ""
    Dim mModelID As Guid = Guid.Empty

    Public mStoreList As StoreList
    Public mVendorList As VendorList
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
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
        'Added By Vikrant 27-Mar-2018 For Deccan26032018
        If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
            lblValuedStores.Visible = True
            cmbStoreType.Visible = True
            lblType.Visible = True
        Else
            If AppSettings("IsGSTApplicable") = "True" Then
                lblGST.Text = "Step VII. Selection For Values With/Without GST"
                lblCustomerStore.Text = "Step VIII. Selection of Store/Customer"
                lblStep11.Text = "Step IX. Display Report"
            Else
                lblCustomerStore.Text = "Step VII. Selection of Store/Customer"
                lblStep11.Text = "Step VIII. Display Report"
            End If
            lblValuedStores.Visible = False
            cmbStoreType.Visible = False
            lblType.Visible = False
            'lblStep11.Text = IIf(AppSettings("IsGSTApplicable") = "True", "Step VIII. Display Report", "Step VII. Display Report") '"Step VII. Display Report"
        End If
        'End
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblExpenses1.Visible = True
        lblAircraft1.Visible = True
        lblCustomerName.Visible = True
        lblModel1.Visible = True
        lblStoreType.Visible = IIf(AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ", True, False) ' SPZ Code added by Saylee on 13-Jun-2022 'Added By Vikrant 27-Mar-2018 For Deccan26032018
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblExpenses1.Visible = False
        lblAircraft1.Visible = False
        lblCustomerName.Visible = False
        lblModel1.Visible = False
        lblStoreType.Visible = False 'Added By Vikrant 27-Mar-2018 For Deccan26032018
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
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1/1/1900"
            ToDate = "1/1/2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            Fromdate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " (" & cmbDateRange.SelectedItem.Text & ")"
        End If

        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
        Aircraft = txtAircraft.Text.Trim 'Added By Prashant 04-May-2015 FOR ALL04052015
        If txtSupplierList.Text.Trim = "" Then
            lblCustomerName.Text = "Supplier : All"
        Else
            lblCustomerName.Text = "Supplier :" & mCustomerList(txtSupplierList.Text.Trim).Name
        End If
        lblAircraft1.Text = "Aircraft :  " & Aircraft
        Session("PartNo") = PartNo
        Session("Description") = Description

        If (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = False) Then
            ModelName = ""
            lblModel1.Text = "Model : " & "All"
        ElseIf (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = True) Then
            ModelName = "Common/No Applicability"
        Else
            ModelName = cmbModel.SelectedItem.Text
            mModelID = New Guid(cmbModel.SelectedValue)
            lblModel1.Text = "Model : " & cmbModel.SelectedItem.Text
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblExpenses1.Text = "Expenses : " & cmbExpenses.SelectedItem.Text
        lblStoreType.Text = "Store Type : " & IIf(cmbStoreType.SelectedIndex > 0, cmbStoreType.SelectedItem.Text, "All") 'Added By Vikrant 27-Mar-2018 For Deccan26032018
        mSearchingCriteria = lblDateRangeFrom.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblExpenses1.Text + ", " + lblAircraft1.Text + ", " + lblCustomerName.Text + ", " + lblModel1.Text + ", " + lblStoreType.Text
    End Sub
    Private Sub ResetValues()
        Fromdate = "1/1/1900"
        ToDate = "1/1/2200"
        Supplier = ""
        PartNo = ""
        ReceivingStoreID = "{00000000-0000-0000-0000-000000000000}"
        Description = ""
        Session("PartNo") = ""
        Session("Description") = ""
    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        'Session("IsExcel") = IsExcel
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptGROExpenseReport As rptGROExpenseReport

        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsGROExpenseReport
        SetValues()
        myReport = New crptGROExpenseReport
        If txtSupplierList.Text = "" Then
            SupplierID = Guid.Empty
        Else
            SupplierID = mCustomerList(Trim(txtSupplierList.Text)).ID
        End If
        mrptGROExpenseReport = rptGROExpenseReport.GetrptGROExpenseReport(Fromdate, ToDate, PartNo, Description, cmbExpenses.SelectedIndex, Aircraft, _
                                                                          SupplierID.ToString, ModelID:=mModelID.ToString, CommonOrApplicability:=chkCommonOrApplicability.Checked, _
                                                                          IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), _
                                                                          EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, _
                                                                          CustomerID:=cmbCustomer.SelectedValue.ToString, StoreID:=cmbStore.SelectedValue.ToString)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, IIf(chkWithGST.Visible, IIf(chkWithGST.Checked, "GRO Expense Report", "GRO Expense Report (Values excluding GST)"), "GRO Expense Report"), IIf(Fromdate = "1/1/1900", "", New SmartDate(Fromdate).FormattedText), _
              IIf(ToDate = "1/1/2200", "", New SmartDate(ToDate).FormattedText), PartNo, Description, _
              IIf(cmbExpenses.SelectedIndex = 0, "", cmbExpenses.SelectedItem.Text), AppSettings("Product Version"), AppSettings("SINote"), _
              Aircraft, Trim(txtSupplierList.Text), SearchStr8:=ModelName, SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:=IIf(cmbStoreType.SelectedIndex > 0, cmbStoreType.SelectedItem.ToString, ""))

        If mrptGROExpenseReport.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mrptGROExpenseReport.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1224)
        End If
        ds.Clear()
        If IsExcel = False Then 'PDF format
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mrptGROExpenseReport)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        Else
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "rptGROExpenseReport", mrptGROExpenseReport)
            Dim columnToRemove2 As String() = {"ID", "CompanyName", "ShortName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"ReceiptText", "ReceiptNo", "OrderTransTypeID", "OrderIsOverhaul", "DisplayQty", "ReceiptTransTypeID", _
                                              "OrderText", "OrderNo", "OrderAmend", "OrderDate"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("rptGROExpenseReport").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("rptGROExpenseReport").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("rptGROExpenseReport").Columns.Contains("ReceiptDate") Then
                ds.Tables("rptGROExpenseReport").Columns("ReceiptDate").ColumnName = "Receipt Date"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("ReceiptTextNo") Then
                ds.Tables("rptGROExpenseReport").Columns("ReceiptTextNo").ColumnName = "GRO Ref."
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("PartName") Then
                ds.Tables("rptGROExpenseReport").Columns("PartName").ColumnName = "Part Number"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("EffRate") Then
                ds.Tables("rptGROExpenseReport").Columns("EffRate").ColumnName = "Charge (" + Report.CurrencySymbol + ")"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("OrderNumber") Then
                ds.Tables("rptGROExpenseReport").Columns("OrderNumber").ColumnName = "Order No."
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("rptGROExpenseReport").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("DeliveryNo") Then
                ds.Tables("rptGROExpenseReport").Columns("DeliveryNo").ColumnName = "Delivery No."
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("DeliveryDate") Then
                ds.Tables("rptGROExpenseReport").Columns("DeliveryDate").ColumnName = "Delivery Date"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("VendorInvoiceDate") Then
                ds.Tables("rptGROExpenseReport").Columns("VendorInvoiceDate").ColumnName = "Vendor Invoice Date"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("VendorInvoiceNo") Then
                ds.Tables("rptGROExpenseReport").Columns("VendorInvoiceNo").ColumnName = "Vendor Invoice No"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("CreatedBy") Then
                ds.Tables("rptGROExpenseReport").Columns("CreatedBy").ColumnName = "Created By"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("AuthorizedBy") Then
                ds.Tables("rptGROExpenseReport").Columns("AuthorizedBy").ColumnName = "Authorized By"
            End If
            'TDD
            If ds.Tables("rptGROExpenseReport").Columns.Contains("CEffRate") Then
                ds.Tables("rptGROExpenseReport").Columns("CEffRate").ColumnName = "Charge (Invoice Cur.)"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("TotalCEffCharge") Then
                ds.Tables("rptGROExpenseReport").Columns("TotalCEffCharge").ColumnName = "Total Charge (Invoice Cur.)"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("CurrencySymbol") Then
                ds.Tables("rptGROExpenseReport").Columns("CurrencySymbol").ColumnName = "Invoice Currency"
            End If
            If ds.Tables("rptGROExpenseReport").Columns.Contains("TotalCharge") Then
                ds.Tables("rptGROExpenseReport").Columns("TotalCharge").ColumnName = "Total Charge (" + Report.CurrencySymbol + ")"
            End If
            'End

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Part No"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Description"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Expenses"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Aircraft"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr7") Then
                ds.Tables("ReportData").Columns("SearchStr7").ColumnName = "Supplier"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr11") Then
                ds.Tables("ReportData").Columns("SearchStr11").ColumnName = "Store Type"
            End If

            ds.Tables("rptGROExpenseReport").Columns("Receipt Date").SetOrdinal(0)
            ds.Tables("rptGROExpenseReport").Columns("Part Number").SetOrdinal(1)
            ds.Tables("rptGROExpenseReport").Columns("SerialNo").SetOrdinal(2)
            ds.Tables("rptGROExpenseReport").Columns("Description").SetOrdinal(3)
            ds.Tables("rptGROExpenseReport").Columns("Supplier").SetOrdinal(4)
            ds.Tables("rptGROExpenseReport").Columns("GRO Ref.").SetOrdinal(5)
            ds.Tables("rptGROExpenseReport").Columns("Order No.").SetOrdinal(6)
            ds.Tables("rptGROExpenseReport").Columns("Order Date").SetOrdinal(7)
            ds.Tables("rptGROExpenseReport").Columns("ReceiptType").SetOrdinal(8)
            ds.Tables("rptGROExpenseReport").Columns("Qty").SetOrdinal(9)
            ds.Tables("rptGROExpenseReport").Columns("Unit").SetOrdinal(10)
            ds.Tables("rptGROExpenseReport").Columns("Location").SetOrdinal(11)
            'ds.Tables("rptGROExpenseReport").Columns("TotalCharge").SetOrdinal(12)

            Dim dsNew As New DataSet

            dsNew.Clear()
            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("rptGROExpenseReport"))
            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("rptGROExpenseReport").TableName = "GRO Expense Report"
			Session("ExcelFileName") = "GRO Expense Report"

			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        End If
        ' MarkLog(Util.Action.Print, "GROExpenseReport", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        MarkLog(Util.Action.Print, "GROExpenseReport", IIf(IsExcel = True, "Export To excel ", "") + mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) ' iif Added by Shital on 18-Jan-2021
        ResetValues()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, True, True)
        Session("mCustomerList") = mCustomerList
        'Model
        mModelList = ModelList.GetAirframeModelList("(All)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList

        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        '
        'Customer
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, False)
        cmbCustomer.DataSource = mCustomerList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            'Ajay 09-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "GROExpenseReport") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            DataFieldBind()
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
        upnlDateRange.Update()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport(False) Else upnlValidationsummary.Update()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then SetReport(True) Else upnlValidationsummary.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Session.Remove("mCustomerList")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        If Not IsDate(txtFromDate.Text.Trim) Then
            txtFromDate.Text = ""
        End If
    End Sub
    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        If Not IsDate(txtToDate.Text.Trim) Then
            txtToDate.Text = ""
        End If
    End Sub
    Private Sub chkCommonOrApplicability_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkCommonOrApplicability.CheckedChanged
        If chkCommonOrApplicability.Checked = True Then
            cmbModel.Enabled = False
            cmbModel.SelectedIndex = 0
            cmbModel.DataBind()
        Else
            cmbModel.Enabled = True
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
    End Sub
    Private Sub cmbCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustomer.SelectedIndexChanged
        'Requested for Customer Stores  
        If chkCustomerStock.Checked Then
            If Not cmbCustomer.SelectedIndex <= 0 Then 'If Customer Selected
                mStoreList = StoreList.GetStoreList(New Guid(cmbCustomer.SelectedValue.ToString), "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
        End If
        cmbStore.DataBind()
        Session("mStoreList") = mStoreList
        If cmbCustomer.Enabled = True Then
            setFocus(cmbCustomer)
        End If
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then
            lblCustomer.Enabled = True
            cmbCustomer.Enabled = True

            If Not cmbCustomer.SelectedIndex <= 0 Then                       'If Customer Selected
                mStoreList = StoreList.GetStoreList(New Guid(cmbCustomer.SelectedValue.ToString), "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            setFocus(cmbCustomer)
        Else
            cmbCustomer.SelectedIndex = 0
            lblCustomer.Enabled = False
            cmbCustomer.Enabled = False

            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)         'Self
            cmbStore.DataSource = mStoreList

            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            If cmbStore.Enabled = True Then
                setFocus(cmbStore)
            End If
        End If
    End Sub
    'Ajay 09-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "GROExpenseReport")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "GROExpenseReport")
    End Sub
    '-----
#End Region

End Class