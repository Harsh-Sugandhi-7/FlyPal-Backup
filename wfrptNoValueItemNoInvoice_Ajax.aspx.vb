Public Class wfrptNoValueItemNoInvoice_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStore As Store
    Public mStoreList As StoreList
    Public mCustomerList As VendorList
    Public PartNo As String = ""
    Public Description As String = ""
    Public strStore As String
    Public strCustomer As String
    Dim mNoValueItemNoInvoiceSearchingCriteria As String = String.Empty


    'Added by Abhishek on 13-SEP-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim ds As New dsNoValueItemNoInvoice
    Dim objSearch As rptSearchingCriteria
    Dim rpt As rptNoValueItemNoInvoice
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mCustomerList") = mCustomerList
        Session("mStoreList") = mStoreList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCustomerList")
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility2()
        lblStoreName.Visible = True
        lblCustomerName.Visible = IIf(cmbSupplier.Enabled = True, True, False)
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblStoreName.Visible = False
        lblCustomerName.Visible = False
    End Sub
    Private Sub SetValues()
        If cmbStoreList.SelectedIndex = 0 Then
            strStore = ""
            lblStoreName.Text = "Store : All"
        Else
            strStore = Store.GetStore(New Guid(cmbStoreList.SelectedValue)).Name
            lblStoreName.Text = "Store : " & strStore
        End If
        If cmbSupplier.SelectedIndex = 0 Then
            strCustomer = ""
            lblCustomerName.Text = "Supplier : All"
        Else
            strCustomer = Vendor.GetVendor(New Guid(cmbSupplier.SelectedValue)).Name
            lblCustomerName.Text = "Supplier : " & strCustomer
        End If
        'Added By Shweta ON 06-Dec-2012 FOR ALL28112012
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        'End
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        mNoValueItemNoInvoiceSearchingCriteria = lblCustomerName.Text.Trim + ", " + lblStoreName.Text + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim
    End Sub
#End Region

#Region " Set Report "
    Private Sub setNoValueItemNoInvoiceList()
        Dim MyReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsNoValueItemNoInvoice
        Dim obj As rptNoValueItemNoInvoice
        'rpt as obj
        Dim objSearch As rptSearchingCriteria
        MyReport = New crptNoValueItemNoInvoice
        SetValues()
        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, strCustomer, "", "", "", strStore, "", "", Description, "", 0, "", "", "", AppSettings("Logo"))
        obj = rptNoValueItemNoInvoice.GetNoValueItemNoInvoiceList(mCustomerList.Item(cmbSupplier.SelectedIndex).ID.ToString, mStoreList.Item(cmbStoreList.SelectedIndex).ID.ToString, PartNo, Description)

        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 510)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, objSearch)
        MyReport.SetDataSource(ds)
        Session("CrystalReport") = MyReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "NoValueItemNoInvoice", mNoValueItemNoInvoiceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Supplier
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", False, True, False)
        cmbSupplier.DataSource = mCustomerList
        Session("mCustomerList") = mCustomerList
        'Store
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True)
        cmbStoreList.DataSource = mStoreList
        Session("mStoreList") = mStoreList

        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack And Session("sender") = "" Then
            If cmbSupplier.Enabled = True Then
                SetFocus(cmbSupplier)
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        setNoValueItemNoInvoiceList()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    'Added by Abhishek on 13-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            rpt = rptNoValueItemNoInvoice.GetNoValueItemNoInvoiceList(mCustomerList.Item(cmbSupplier.SelectedIndex).ID.ToString, mStoreList.Item(cmbStoreList.SelectedIndex).ID.ToString, PartNo, Description)
            ' objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, strCustomer, "", "", "", strStore, "", "", Description, "", 0, "", "", "", "", "", "", "", "", "", "")
            objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, strCustomer, "", "", "", strStore, "", "", Description, "", 0, "", "", "", "", "", "", "", "", "", "")

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 510)
            End If
            ds.Clear()

            da.Fill(ds, objSearch)
            da.Fill(ds, "ExcelrptNoValueItemNoInvoice", rpt)

            Dim columnToRemove1 As String() = {"Remark", "OrderNo", "ReceiptNo"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"CompanyName", "FromDate", "ToDate", "BranchName", "Category", "Nomenclature", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("OrderText") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("OrderText").ColumnName = "Order No. "
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("ReceiptText") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("ReceiptText").ColumnName = "Receipt No."
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("ReceiptDate") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("ReceiptDate").ColumnName = "Receipt Date"
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("SupplierName") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("SupplierName").ColumnName = "Supplier"
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("PartName") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("PartName").ColumnName = "Part No"
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("Description") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("Description").ColumnName = "Description"
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("SerialNo") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("SerialNo").ColumnName = "Serial No"
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("ReceiptQty") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("ReceiptQty").ColumnName = "Receipt Qty."
            End If

            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("InvoiceQty") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("InvoiceQty").ColumnName = "Invoice Qty."
            End If
            If ds.Tables("ExcelrptNoValueItemNoInvoice").Columns.Contains("InvoiceBalanceQty") Then
                ds.Tables("ExcelrptNoValueItemNoInvoice").Columns("InvoiceBalanceQty").ColumnName = "InvoiceBalance Qty."
            End If


         
            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelrptNoValueItemNoInvoice"))

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptNoValueItemNoInvoice").TableName = "No Value Item No Invoice"
			Session("ExcelFileName") = "No Value Item No Invoice"

			Session("dsNew") = dsNew
            Session("DataTableToBeFormattedForExportToExcel") = "No Value Item No Invoice"
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "NoValueItemNoInvoice", "Export To excel " + mNoValueItemNoInvoiceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
End Class