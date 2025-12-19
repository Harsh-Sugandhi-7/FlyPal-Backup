Public Class wfrptWarrantyMonitoringSheet_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Dim mDateSearchingCriteria As String = String.Empty
    Public mVendorList As VendorList
    Public mWarrantyStatusList As WarrantyStatusList
    Public mWarrantyMonitoringSheet As WarrantyMonitoringSheet
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
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        lblFromDate.Text = "From Date : " & FromDate
        lblToDate.Text = "To Date     : " & ToDate
        lblSupp.Text = "Supplier : " & IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, "All")
        lblStatus.Text = "Status : " & IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "All")
        lblPartNo.Text = "Part No. : " & PartNo
        lblPartDescription.Text = "Description : " & Description
        mDateSearchingCriteria = lblFromDate.Text.Trim + ", " + lblToDate.Text.Trim + ", " + lblSupp.Text + ", " + lblPartNo.Text + ", " + lblPartDescription.Text
    End Sub
    Private Sub ControlVisibility()
        lblSummary.Visible = True
        lblFromDate.Visible = True
        lblToDate.Visible = True
        lblStatus.Visible = True
        lblSupp.Visible = True
        lblPartNo.Visible = True
        lblPartDescription.Visible = True
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'status
        mWarrantyStatusList = WarrantyStatusList.GetWarrantyStatusList(True, "(All)")
        cmbStatus.DataSource = mWarrantyStatusList

        'Vendor
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbSupplier.DataSource = mVendorList

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
        Dim ds As New dsWarrantyMonitoringSheet
        Dim da As New CSLA.Data.ObjectAdapter

        SetValues()
        myReport = New crptWarrantyMonitoringSheet
        mWarrantyMonitoringSheet = WarrantyMonitoringSheet.GetWarrantyMonitoringSheet(FromDate, ToDate, cmbSupplier.SelectedValue.ToString, PartNo, Description, IIf(Val(cmbStatus.SelectedValue) = 0, -1, Val(cmbStatus.SelectedValue)))
        If (mWarrantyMonitoringSheet.Count <= 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1383)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), _
        SearchStr4:=PartNo, SearchStr5:=Description, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
        SearchStr6:=IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, ""), SearchStr7:="", SearchStr8:="", SearchStr9:=PartNo, _
        SearchStr10:=AppSettings("Logo"))

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mWarrantyMonitoringSheet)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "WarrantyMonitoringSheet", mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsWarrantyMonitoringSheet
        SetValues()
        mWarrantyMonitoringSheet = WarrantyMonitoringSheet.GetWarrantyMonitoringSheet(FromDate, ToDate, cmbSupplier.SelectedValue.ToString, PartNo, Description, IIf(Val(cmbStatus.SelectedValue) = 0, -1, Val(cmbStatus.SelectedValue)))

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), _
        SearchStr4:=PartNo, SearchStr5:=Description, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
        SearchStr6:=IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, ""), SearchStr7:="", SearchStr8:="", SearchStr9:=PartNo, _
        SearchStr10:=AppSettings("Logo"))

        If (mWarrantyMonitoringSheet.Count <= 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1383)
        End If

        ds.Clear()
        da.Fill(ds, mWarrantyMonitoringSheet)
        da.Fill(ds, Report)

        Dim columnToRemove As String() = {"OrderNo", "OrderText", "Amend", "ReceiptText", "ReceiptNo", "SrNo", "TDNo", "TDDate"}
        For k As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("WarrantyMonitoringSheet").Columns.Contains(columnToRemove(k)) Then
                ds.Tables("WarrantyMonitoringSheet").Columns.Remove(columnToRemove(k))
            End If
        Next

        Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr11", "ShortName", "ReportName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
            End If
        Next

        If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
            ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
            ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
            ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Supplier"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
            ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Part No."
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
            ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Description"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
            ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Status"
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Merge(ds.Tables("WarrantyMonitoringSheet"))
		dsNew.Tables("WarrantyMonitoringSheet").TableName = "Warranty Monitoring Sheet"
		Session("ExcelFileName") = "Warranty Monitoring Sheet"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "WarrantyMonitoringSheet", "Export To Excel " + mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

End Class