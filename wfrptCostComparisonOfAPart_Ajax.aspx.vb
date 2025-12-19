Public Class wfrptCostComparisonOfAPart_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Dim mDateSearchingCriteria As String = String.Empty
    Public mVendorList As VendorList
    Public mWarrantyStatusList As WarrantyStatusList
    Public mCostComparisonOfAPart As CostComparisonOfAPart
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Dim mCompanyDetail As New CompanyDetail
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        'If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
        '    PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
        '    Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        'Else
        '    PartNo = Trim(txtSearch.Text)
        '    Description = Trim(txtSearch.Text)
        'End If
    End Sub
    Private Sub FindNow()
        mCostComparisonOfAPart = CostComparisonOfAPart.GetCostComparisonOfAPart(New Guid(cmbPartList.SelectedValue.ToString))
        dgPartList.DataSource = mCostComparisonOfAPart
        dgPartList.DataBind()
    End Sub
    Private Sub ControlVisibility()
        If dgPartList.Rows.Count > 25 Then
            btnClose.Visible = True
            btnExport.Visible = True
            btnDisplay.Visible = True
        Else
            btnClose.Visible = False
            btnExport.Visible = False
            btnDisplay.Visible = False
        End If
        If dgPartList.Rows.Count > 0 Then
            TopbtnExport.Enabled = True
            TopbtnDisplay.Enabled = True
        Else
            TopbtnExport.Enabled = False
            TopbtnDisplay.Enabled = False
        End If
        upnlTopButtons.Update()
        upnlActionBtns.Update()
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsCostComparisonOfAPart
        myReport = New crptCostComparisonOfAPart
        Dim EventLogDetail As String = "From Date  :" + txtFromDate.Text.ToString + "," + " To Date  :" + txtToDate.Text.ToString + ", Part No.  :" + cmbPartList.SelectedItem.ToString + ", Supplier  :" + cmbSupplier.SelectedItem.ToString 'Added by Shital on 18-Jan-2021
        mCostComparisonOfAPart = CostComparisonOfAPart.GetCostComparisonOfAPart(New Guid(cmbPartList.SelectedValue.ToString), _
                                                                                IIf(cmbPartList.SelectedIndex = 0, "", cmbPartList.SelectedItem.Text), _
                                                                                "", txtFromDate.Text.Trim, txtToDate.Text.Trim, _
                                                                                cmbSupplier.SelectedValue.ToString, chkCheckForAlternatePart.Checked, CInt(cmbType.SelectedValue))
        If mCostComparisonOfAPart.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1394)
            'MarkLog(Util.Action.Print, "CostComparisonOfAPart", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)'Commented by Shital on 18-Jan-2021
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
       mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
       mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
      "Part Cost Comparison", txtFromDate.Text.Trim, txtToDate.Text.Trim, PartNo, SearchStr4:=IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.Text), SearchStr5:=cmbType.SelectedItem.Text, _
        ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", _
        SearchStr10:=AppSettings("Logo"), SearchStr11:="")

        If IsExcel = False Then     'PDF format
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mCostComparisonOfAPart)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "CostComparisonOfAPart", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        ElseIf IsExcel = True Then  'Excel format
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "CostComparisonOfAPart", mCostComparisonOfAPart)

            Dim columnToRemove2 As String() = {"ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"InvoiceDate", "InvoiceText", "InvoiceNo", "OrderText", "OrderNo", "OrderAmend", "OrderDate", "Rate", "CAmount", "Amount", "CEffRate", "EffRate", "CommercialRate", "Year"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("CostComparisonOfAPart").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("CostComparisonOfAPart").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("CostComparisonOfAPart").Columns.Contains("ItemName") Then
                ds.Tables("CostComparisonOfAPart").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("CostComparisonOfAPart").Columns.Contains("ItemType") Then
                ds.Tables("CostComparisonOfAPart").Columns("ItemType").ColumnName = "Type"
            End If
            If ds.Tables("CostComparisonOfAPart").Columns.Contains("InvoiceDateFormatted") Then
                ds.Tables("CostComparisonOfAPart").Columns("InvoiceDateFormatted").ColumnName = "Invoice Date"
            End If

            If ds.Tables("CostComparisonOfAPart").Columns.Contains("CRate") Then
                ds.Tables("CostComparisonOfAPart").Columns("CRate").ColumnName = "Rate"
            End If

            If ds.Tables("CostComparisonOfAPart").Columns.Contains("CCommercialRate") Then
                ds.Tables("CostComparisonOfAPart").Columns("CCommercialRate").ColumnName = "Commercial Rate"
            End If

            If ds.Tables("CostComparisonOfAPart").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("CostComparisonOfAPart").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("CostComparisonOfAPart").Columns.Contains("VendorName") Then
                ds.Tables("CostComparisonOfAPart").Columns("VendorName").ColumnName = "Supplier"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Part No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Supplier"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Tran. Type"
            End If
            Dim dsNew As New DataSet

            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("CostComparisonOfAPart"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("CostComparisonOfAPart").TableName = "Part Cost Comparison"
			Session("ExcelFileName") = "Part Cost Comparison"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "CostComparisonOfAPart", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        cmbPartList.DataSource = ItemList.GetItemList(0, "", "", "", "", "", "", True)

        'Vendor
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbSupplier.DataSource = mVendorList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
            'FindNow()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If Me.IsValid = False Then upnlValidations.Update()
        mCostComparisonOfAPart = CostComparisonOfAPart.GetCostComparisonOfAPart(New Guid(cmbPartList.SelectedValue.ToString), _
                                                                                IIf(cmbPartList.SelectedIndex = 0, "", cmbPartList.SelectedItem.Text), _
                                                                                "", txtFromDate.Text.Trim, txtToDate.Text.Trim, _
                                                                                cmbSupplier.SelectedValue.ToString, chkCheckForAlternatePart.Checked, CInt(cmbType.SelectedValue))
        dgPartList.DataSource = mCostComparisonOfAPart
        dgPartList.DataBind()
        lblResult.Text = "List of details for the Part No. as per criteria : " & mCostComparisonOfAPart.Count.ToString & " Record(s) found."
        ControlVisibility()
        upnlGridView.Update()
    End Sub
    Private Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click, TopbtnDisplay.Click
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click, TopbtnExport.Click
        SetReport(True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, TopbtnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
    '     Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    '     Dim ds As New dsWarrantyMonitoringSheet
    '     Dim da As New CSLA.Data.ObjectAdapter

    '     SetValues()
    '     myReport = New crptWarrantyMonitoringSheet
    '     mWarrantyMonitoringSheet = WarrantyMonitoringSheet.GetWarrantyMonitoringSheet(FromDate, ToDate, cmbSupplier.SelectedValue.ToString, PartNo, Description, Val(cmbStatus.SelectedValue))
    '     If (mWarrantyMonitoringSheet.Count <= 0) Then
    '         MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '         Exit Sub
    '     Else
    '         RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1383)
    '     End If

    '     mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
    '     Dim Report As New ReportData(mCompanyDetail.CompanyName, _
    '     mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
    '     mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
    '     "", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), _
    '     SearchStr4:=PartNo, SearchStr5:=Description, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
    '     SearchStr6:=IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, ""), SearchStr7:="", SearchStr8:="", SearchStr9:=PartNo, _
    '     SearchStr10:=AppSettings("Logo"))

    '     ds.Clear()
    '     Dim mrptImage As rptImage = rptImage.GetImage(ds)
    '     da.Fill(ds, mWarrantyMonitoringSheet)
    '     da.Fill(ds, mrptImage)
    '     da.Fill(ds, Report)
    '     myReport.SetDataSource(ds)
    '     Session("CrystalReport") = myReport
    '     Dim Str As String
    '     Str = "openTranDetail();"
    '     ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

    '     MarkLog(Util.Action.Print, "WarrantyMonitoringSheet", mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    ' End Sub
    ' Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
    '     Dim da As New CSLA.Data.ObjectAdapter
    '     Dim ds As New dsWarrantyMonitoringSheet
    '     SetValues()
    '     mWarrantyMonitoringSheet = WarrantyMonitoringSheet.GetWarrantyMonitoringSheet(FromDate, ToDate, cmbSupplier.SelectedValue.ToString, PartNo, Description, Val(cmbStatus.SelectedValue))

    '     mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
    '     Dim Report As New ReportData(mCompanyDetail.CompanyName, _
    '     mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
    '     mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
    '     "", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), _
    '     SearchStr4:=PartNo, SearchStr5:=Description, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
    '     SearchStr6:=IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, ""), SearchStr7:="", SearchStr8:="", SearchStr9:=PartNo, _
    '     SearchStr10:=AppSettings("Logo"))

    '     If (mWarrantyMonitoringSheet.Count <= 0) Then
    '         MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '         Exit Sub
    '     Else
    '         RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1383)
    '     End If

    '     ds.Clear()
    '     da.Fill(ds, mWarrantyMonitoringSheet)
    '     da.Fill(ds, Report)

    '     Dim columnToRemove As String() = {"OrderNo", "OrderText", "Amend", "ReceiptText", "ReceiptNo", "SrNo"}
    '     For k As Integer = 0 To columnToRemove.Length - 1
    '         If ds.Tables("WarrantyMonitoringSheet").Columns.Contains(columnToRemove(k)) Then
    '             ds.Tables("WarrantyMonitoringSheet").Columns.Remove(columnToRemove(k))
    '         End If
    '     Next

    '     Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr11", "ShortName", "ReportName"}
    '     For i As Integer = 0 To columnToRemove2.Length - 1
    '         If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
    '             ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
    '         End If
    '     Next

    '     If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
    '         ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
    '     End If
    '     If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
    '         ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
    '     End If
    '     If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
    '         ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Supplier"
    '     End If
    '     If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
    '         ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Part No."
    '     End If
    '     If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
    '         ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Description"
    '     End If
    '     If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
    '         ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Status"
    '     End If

    '     Dim dsNew As New DataSet
    '     dsNew.Clear()

    '     dsNew.Merge(ds.Tables("ReportData"))
    '     dsNew.Tables("ReportData").TableName = "Searching Criteria"
    '     dsNew.Merge(ds.Tables("WarrantyMonitoringSheet"))
    '     dsNew.Tables("WarrantyMonitoringSheet").TableName = "Warranty Monitoring Sheet"
    '     Session("dsNew") = dsNew
    '     ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    ' End Sub
#End Region

   
End Class