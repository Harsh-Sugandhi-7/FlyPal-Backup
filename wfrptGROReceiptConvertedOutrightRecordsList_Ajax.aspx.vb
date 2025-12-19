Public Class wfrptGROReceiptConvertedOutrightRecordsList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Dim ToDate As String
    Dim PartNo As String
    Dim Description As String
    Dim mItemID As Guid
    Dim mItemList As ItemList
    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mItemList = Session("mItemList")
        PartNo = Session("PartNo")
        Description = Session("Description")
        mItemID = Session("mItemID")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mItemList")
    End Sub
    Private Sub SetValues()
        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
      
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblReceipt.Text = IIf(txtReceiptTextList.Text <> "" And txtNo.Text <> "", "Receipt No.       : " & txtReceiptTextList.Text + "-" + txtNo.Text, "Receipt No.       : All")
        lblFrom.Text = "From Date : " & txtFromDate.Text
        lblTo.Text = "To Date : " & txtToDate.Text
        mCompleteSearchingCriteria = lblFrom.Text + ", " + lblTo.Text + ", " + lblReceipt.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As GROReceiptConvertedOutrightRecordsList
        Dim mCompanyDetail As New CompanyDetail
        SetValues()
        myReport = New crptGROReceiptConvertedOutrightRecordsList
        rpt = GROReceiptConvertedOutrightRecordsList.GetGROReceiptConvertedOutrightRecordsList(txtReceiptTextList.Text.Trim, Val(txtNo.Text), PartNo, txtFromDate.Text, txtToDate.Text)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1314)
        End If
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "", txtFromDate.Text, txtToDate.Text, IIf(txtReceiptTextList.Text <> "" And txtNo.Text <> "", txtReceiptTextList.Text + "-" + txtNo.Text, ""), PartNo, Description, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If IsExcel = False Then     'PDF format
            Dim ds As New dsGROReceiptConvertedOutrightRecordsList
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "GROToOutrightReceiptReport", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcelGROReceiptConvertedOutrightRecordsList
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "GROReceiptConvertedOutrightRecordsList", rpt)

            Dim columnToRemove2 As String() = {"ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"ReceiptID", "ReceiptDate", "ReceiptText", "ReceiptNo", "ReceiptItemID", "OrderText", "OrderNo", "OrderAmend", "OrderItemID", "OrderItemQty", "InvoiceItemQty"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns("ReceiptDateFormatted").ColumnName = "Date"
            End If
            If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains("ReceiptItemSerialNo") Then
                ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns("ReceiptItemSerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains("VendorName") Then
                ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns("VendorName").ColumnName = "Supplier"
            End If
            If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains("GROEffRate") Then
                ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns("GROEffRate").ColumnName = "Rate"
            End If
            If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains("GROAmount") Then
                ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns("GROAmount").ColumnName = "Amount"
            End If
            If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains("ItemName") Then
                ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns.Contains("ItemDescription") Then
                ds.Tables("GROReceiptConvertedOutrightRecordsList").Columns("ItemDescription").ColumnName = "Description"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Receipt No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Part No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Description"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("ReportData").TableName = "Searching Criteria"
			ds.Tables("GROReceiptConvertedOutrightRecordsList").TableName = "GRO To OutRight Receipt Report"
			Session("ExcelFileName") = "GRO To OutRight Receipt Report"
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "GROToOutrightReceiptReport", "Export To excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If

    End Sub
    Private Sub ControlInVisible()
        lblFrom.Visible = False
        lblTo.Visible = False
        lblReceipt.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub ControlVisible()
        lblFrom.Visible = True
        lblTo.Visible = True
        lblReceipt.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
#End Region

#Region " Data Binding "
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
#End Region

End Class