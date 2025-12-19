Imports javax.transaction

Public Class wfReceiptPendingForInvoice
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Dim VendorID As Guid 'Sankalp
    Dim mDateSearchingCriteria As String = String.Empty
    Dim ds As New dsPendingInvoice
    Dim da As New CSLA.Data.ObjectAdapter
    Dim obj As PendingInvoiceList
    Dim mCompanyDetail As New CompanyDetail
    Dim mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim ReceiptText As String
    Dim ReceiptNo As String
    Public mVendorList As VendorList

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("2", , True, "(All)")
        mVendorList = VendorList.GetVendorstList(0,,,,,, "(All)", False, True, False)
        cmbVendor.DataSource = mVendorList
        cmbReceiptText.DataSource = mDistinctTextListForReceipt
        DataBind()
    End Sub

#End Region

#Region "Bussiness Logic "
    Private Sub addAttributes()
        txtReceiptNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReceiptNo').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            txtFromDate.Text = Today.Date.AddYears(-1).ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub SetReport(Optional ByVal IsForExcel As Boolean = False)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsPendingInvoice
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail

        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        VendorID = New Guid(cmbVendor.SelectedValue)
        ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)
        ReceiptNo = IIf(txtReceiptNo.Text <= 0, 0, Convert.ToInt32(txtReceiptNo.Text))
        obj = PendingInvoiceList.GetPendingToInvoiceList(VendorID,,,,, FromDate,
                                                         ToDate, ReceiptText,
                                                         CInt(Val(ReceiptNo)),
                                                         IsForReport:=1)
        myReport = New crPendingInvoiceList
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Report As New ReportData(mCompanyDetail.CompanyName,
									 mCompanyDetail.Address,
									 mCompanyDetail.Tel1,
									 mCompanyDetail.Tel2,
									 mCompanyDetail.Fax,
									 mCompanyDetail.Email,
									 mCompanyDetail.WebSite,
									 "Receipt Pending For Invoice",
									 New SmartDate(txtFromDate.Text).FormattedText,
									 New SmartDate(txtToDate.Text).FormattedText,
									 cmbReceiptText.SelectedItem.Text,
									 ReceiptNo,
									 cmbVendor.SelectedItem.Text,
									 AppSettings("Product Version"),
									 AppSettings("SINote"),
									 "", "", "", "",
									 AppSettings("Logo"))

		If obj.Count <= 0 Then
			MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 725)
		End If


		If IsForExcel = False Then
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, obj)
            da.Fill(ds, mrptImage)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            mDateSearchingCriteria = txtFromDate.Text.Trim + ", " + txtToDate.Text.Trim
            MarkLog(Util.Action.Print, "Receipt Pending For Invoice", mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
			ds.Clear()

			da.Fill(ds, "ReportData", Report)
			da.Fill(ds, "PendingInvoiceList", obj)

			ds.Tables("PendingInvoiceList").Columns("ReceiptNumber").SetOrdinal(0)
			ds.Tables("PendingInvoiceList").Columns("ReceiptDateFormatted").SetOrdinal(1)
			ds.Tables("PendingInvoiceList").Columns("ItemName").SetOrdinal(2)
			ds.Tables("PendingInvoiceList").Columns("ItemDescription").SetOrdinal(3)
			ds.Tables("PendingInvoiceList").Columns("SerialNo").SetOrdinal(4)
			ds.Tables("PendingInvoiceList").Columns("BalanceQtyToDisplay").SetOrdinal(5)
			ds.Tables("PendingInvoiceList").Columns("DisplayUnitName").SetOrdinal(6)
			ds.Tables("PendingInvoiceList").Columns("OrderNumber").SetOrdinal(7)
			ds.Tables("PendingInvoiceList").Columns("OrderDateFormatted").SetOrdinal(8)
			ds.Tables("PendingInvoiceList").Columns("VendorName").SetOrdinal(9)
			ds.Tables("PendingInvoiceList").Columns("ReleaseNoteNo").SetOrdinal(10)
			ds.Tables("PendingInvoiceList").Columns("ReleaseNoteDateFormatted").SetOrdinal(11)
			ds.Tables("PendingInvoiceList").Columns("OrderRate").SetOrdinal(12)
			ds.Tables("PendingInvoiceList").Columns("CurrencyName").SetOrdinal(13)
			ds.Tables("PendingInvoiceList").Columns("CurrencyConversionFactor").SetOrdinal(14)
			ds.Tables("PendingInvoiceList").Columns("CCommercialRate").SetOrdinal(15)
			ds.Tables("PendingInvoiceList").Columns("TransType").SetOrdinal(16)
			ds.Tables("PendingInvoiceList").Columns("CreatedBy").SetOrdinal(17)


			Dim columnToRemove1 As String() = {"AuthorizeBy", "IsSelected", "ReceiptItemID", "IsSerialized", "IssueNumber", "ReleaseNoteDate", "IssueDateFormatted",
                                               "OrderDate", "BalanceQty", "Amend", "CurrecnyID", "CGSTPercentage",
                                               "SGSTPercentage", "IGSTPercentage", "DisplayQty", "DisplayUnitID", "BaseUnitID", "Factor",
                                               "ReceiptDate", "IssueDate", "ApprovalNo", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("PendingInvoiceList").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("PendingInvoiceList").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ReportName", "CurrencyName", "CurrencySymbol", "ShortName", "SINote", "ProductVersion", "ApprovalNo", "SearchStr3", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next
            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date "
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("ReceiptNumber") Then
                ds.Tables("PendingInvoiceList").Columns("ReceiptNumber").ColumnName = "Receipt No."
            End If
			If ds.Tables("PendingInvoiceList").Columns.Contains("ReceiptDateFormatted") Then
				ds.Tables("PendingInvoiceList").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
			End If
			'Sankalp Client Required Supplier NAME
			If ds.Tables("PendingInvoiceList").Columns.Contains("VendorName") Then
				ds.Tables("PendingInvoiceList").Columns("VendorName").ColumnName = "Supplier"
			End If
			If ds.Tables("PendingInvoiceList").Columns.Contains("ItemName") Then
                ds.Tables("PendingInvoiceList").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("ItemDescription") Then
                ds.Tables("PendingInvoiceList").Columns("ItemDescription").ColumnName = "Description"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("SerialNo") Then
                ds.Tables("PendingInvoiceList").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("BalanceQtyToDisplay") Then
                ds.Tables("PendingInvoiceList").Columns("BalanceQtyToDisplay").ColumnName = "Balance Qty."
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("DisplayUnitName") Then
                ds.Tables("PendingInvoiceList").Columns("DisplayUnitName").ColumnName = "Unit"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("OrderNumber") Then
                ds.Tables("PendingInvoiceList").Columns("OrderNumber").ColumnName = "Order No."
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("PendingInvoiceList").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("ReleaseNoteNo") Then
                ds.Tables("PendingInvoiceList").Columns("ReleaseNoteNo").ColumnName = "R. N. No."
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("ReleaseNoteDateFormatted") Then
                ds.Tables("PendingInvoiceList").Columns("ReleaseNoteDateFormatted").ColumnName = "R. N. Date"
            End If

            If ds.Tables("PendingInvoiceList").Columns.Contains("OrderRate") Then
                ds.Tables("PendingInvoiceList").Columns("OrderRate").ColumnName = "NetRate"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("CurrencyName") Then
                ds.Tables("PendingInvoiceList").Columns("CurrencyName").ColumnName = "Currency"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("CurrencyConversionFactor") Then
                ds.Tables("PendingInvoiceList").Columns("CurrencyConversionFactor").ColumnName = "Conversion Factor"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("CCommercialRate") Then
                ds.Tables("PendingInvoiceList").Columns("CCommercialRate").ColumnName = "CommercialRate"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("TransType") Then
                ds.Tables("PendingInvoiceList").Columns("TransType").ColumnName = "Invoice Against Pending"
            End If
            If ds.Tables("PendingInvoiceList").Columns.Contains("CreatedBy") Then
                ds.Tables("PendingInvoiceList").Columns("CreatedBy").ColumnName = "Created By"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("PendingInvoiceList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("PendingInvoiceList").TableName = "Receipt Pending For Invoice"
			Session("ExcelFileName") = "Receipt Pending For Invoice"

			Session("dsNew") = dsNew
            mDateSearchingCriteria = txtFromDate.Text.Trim + ", " + txtToDate.Text.Trim
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "Receipt Pending For Invoice", "Export To Excel " + mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        Else
            upnlValidations.Update()
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(IsForExcel:=True)
        End If
    End Sub
    Private Sub cmbReceiptText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptText.SelectedIndexChanged
        If sender.ID = "cmbReceiptText" Then
            txtReceiptNo.Text = "0"
            If cmbReceiptText.Enabled = True Then
                SetFocus(cmbReceiptText)
            End If
            upnlDetails.Update()
        End If
    End Sub
#End Region

End Class