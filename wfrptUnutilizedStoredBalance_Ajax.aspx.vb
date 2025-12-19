Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Public Class wfrptUnutilizedStoredBalance_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStore As Store
    Public mStoreList As StoreList
    Public mCustomerList As VendorList
    Public PartNo As String = ""
    Public Description As String = ""
    Public strStore, ModelName, AssemblyType, Location As String
    Public AssemblyTypeID As Integer
    Public mAssemblyTypeList As AssemblyTypeList
    Public mModelList As ModelList
    Public mStoreID As Guid
    Public mCustomerID As Guid
    Public ToDate As String
    Public DateFrom As String
    Public DateTo As String
    Public mDateFrom As String
    Public mDateTo As String
    Dim mStoreBalanceForAgingSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
    Public mCategoryLists As CategoryList
    Public mATAList As ATAList
    Dim atacodeList As String = ""
    Public StrATAcode As String
    Dim mModelID As Guid = Guid.Empty
#End Region

#Region " Helper Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        ' Create array of movies  
        Dim movies() As String = {"Star Wars", "Star Trek", "Superman", "Memento", "Shrek", "Shrek II"}
        Dim mModelList As ModelListAutoComplete

        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, AssemblyTypID)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
               Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                   Select c.Name).Take(count).ToList
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCustomerList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim type As String = contextKey.Split("=")(1)
        Dim mVendorListAutoComplete As VendorListAutoComplete = VendorListAutoComplete.GetVendorListAutoComplete(prefixText, type)
        If count = 0 Then
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).ToArray
        Else
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).Take(count).ToArray
        End If
    End Function
    Private Sub GetSession()
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mModelList = CType(Session("mModelList"), ModelList)
        Location = Session("Location")
    End Sub
    Private Sub SetSession()
        Session("mCustomerList") = mCustomerList
        Session("mStoreList") = mStoreList
        Session("mModelList") = mModelList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCustomerList")
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mModelList")
        Session.Remove("Location")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility2()
        lblStoreName.Visible = True
        lblCustomerName.Visible = IIf(txtCustomerList.Enabled = True, True, False)
        lblAssembly1.Visible = True
        lblModel1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblDaysRanges.Visible = True
        lblDatesRange.Visible = True
        lblDateRange.Visible = True
        lblCategoryName.Visible = True
    End Sub
    Private Sub SetCustomerID()
        If hdnCustomerID.Value <> String.Empty Then
            mCustomerID = New Guid(hdnCustomerID.Value.ToString)
        End If
    End Sub
    Private Sub ControlVisibility3()
        lblDateRange.Visible = False
        lblStoreName.Visible = False
        lblCustomerName.Visible = False
        lblAssembly1.Visible = False
        lblModel1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblDaysRanges.Visible = False
        lblDatesRange.Visible = False
        lblCategoryName.Visible = False
    End Sub
    Private Sub SetValues()
        If txtDate.Text.ToString = "" Then
            ToDate = "1/1/3050"
            lblDateRange.Text = "Date Range  : All"
        Else
            ToDate = txtDate.Text.ToString
            lblDateRange.Text = "Date : " & New SmartDate(txtDate.Text.ToString).FormattedText
        End If
        If txtFromDate.Text.ToString = "" Then
            DateFrom = "1/1/1900"
            mDateFrom = "1/1/1900"
        Else
            DateFrom = txtFromDate.Text.ToString
            mDateFrom = New SmartDate(txtFromDate.Text.ToString).FormattedText
        End If

        If txtToDate.Text.ToString = "" Then
            DateTo = "1/1/3050"
            mDateTo = "1/1/3050"
        Else
            DateTo = txtToDate.Text.ToString
            mDateTo = New SmartDate(txtToDate.Text.ToString).FormattedText
        End If
        If txtFromDate.Text.ToString <> "" And txtToDate.Text.ToString <> "" Then
            lblDatesRange.Text = "Date Range " & New SmartDate(txtFromDate.Text.ToString).FormattedText + "    " + "And " & New SmartDate(txtToDate.Text.ToString).FormattedText
        Else
            lblDatesRange.Text = "Date Range : "
        End If
        If cmbStore.SelectedIndex = 0 Then
            strStore = ""
            lblStoreName.Text = "Store : All"
        Else
            strStore = Store.GetStore(New Guid(cmbStore.SelectedValue)).Name
            lblStoreName.Text = "Store : " & strStore
        End If
        If cmbCategory.SelectedIndex = 0 Then
            lblCategoryName.Text = "Category : All"
        Else
            lblCategoryName.Text = "Category : " & cmbCategory.SelectedItem.Text
        End If

        SetCustomerID()
        mStoreID = mStoreList.Item(cmbStore.SelectedIndex).ID

        If txtCustomerList.Text.Trim = "" Then
            lblCustomerName.Text = "Customer : All"
        Else
            lblCustomerName.Text = "Customer :" & txtCustomerList.Text.Trim
        End If

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

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

        atacodeList = hdnATACodeList.Value
        StrATAcode = IIf(atacodeList = String.Empty, String.Empty, atacodeList)
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        mStoreBalanceForAgingSearchingCriteria = lblDateRange.Text + ", Days Range : " + lblDaysRanges.Text + ", " + lblCustomerName.Text + ", " + lblStoreName.Text + " , " + lblCategoryName.Text + ", " + Location + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblAssembly1.Text + ", " + lblModel1.Text + ", " + IIf(chkIsValued.Checked = True, "Valued", "Not Valued") + ", " + ")"
    End Sub
    Private Sub SetReport1(ByVal IsExcel As Boolean)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptStoreBalanceForAgingReport
        Dim mStoreID As Guid = mStoreList.Item(cmbStore.SelectedIndex).ID
        SetCustomerID()
        SetValues()

        myReport = New crptStoreBalanceForUnutilizedItemsReport '1
        rpt = rptStoreBalanceForAgingReport.GetUnutilisedStoreBalance(PartNo, Description, strStore, False, mStoreID, mCustomerID, False, True, _
                                                                      AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, DateFrom, _
                                                                      DateTo, cmbCategory.SelectedValue.ToString, ATACodeList:=StrATAcode, ModelID:=mModelID.ToString, _
                                                                      CommonOrApplicability:=chkCommonOrApplicability.Checked)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", ToDate, PartNo, mDateFrom, mDateTo, _
                                                              txtFromDate.Text, txtToDate.Text, strStore, IIf(cmbCategory.SelectedIndex > 0, _
                                                                                                              cmbCategory.SelectedItem.Text, ""), "", Description, AppSettings("Logo"), , , "", "", ModelName, Search1:=StrATAcode)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1343)
        End If
        If IsExcel = False Then  'PDF format
            Dim ds As New dsStoreBalance
            ds.Clear()
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "UnutilizedStoredBalance", mStoreBalanceForAgingSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcelStoreBalanceForAgingReport
            ds.Clear()
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            da.Fill(ds, "rptStoreBalanceForAgingReport", rpt)

            Dim columnToRemove2 As String() = {"FromDate", "FromStore", "WorkShop", "CompanyName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"Folio", "IsSortByFolio", "StartDate", "ExpiryDate", "StoreName", "Remark", "OnOrder", "GroupBy", "Heading", "CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "CureQtrYear", "ExpQtrYear", "BatchNo", "DaysSinceStockInValue", "Unit", "ReceiptText", "ReceiptNo", "FromTypeID", "SupplierName", "FromStoreName", "AircraftName", "ATACode", "ATANomenclature"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("rptStoreBalanceForAgingReport").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("ReceiptNumberANDDate") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("ReceiptNumberANDDate").ColumnName = "Receipt No."
            End If
            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("BalQty") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("BalQty").ColumnName = "Stock Qty."
            End If
            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("VendorInvoiceNo") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("VendorInvoiceNo").ColumnName = "Supplier Inv. No."
            End If
            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("VendorInvoiceDate") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("VendorInvoiceDate").ColumnName = "Supplier Inv. Date"
            End If
            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("Amount") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("Amount").ColumnName = "Extended Cost"
            End If
            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("DaysSinceStockInValueDetail") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("DaysSinceStockInValueDetail").ColumnName = "Aging Period"
            End If

            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("OrderNo") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("OrderNo").ColumnName = "Order No."
            End If

            If ds.Tables("rptStoreBalanceForAgingReport").Columns.Contains("RequisitionNo") Then
                ds.Tables("rptStoreBalanceForAgingReport").Columns("RequisitionNo").ColumnName = "Requisition No."
            End If

            If ds.Tables("rptSearchingCriteria").Columns.Contains("ToDate") Then
                ds.Tables("rptSearchingCriteria").Columns("ToDate").ColumnName = "As On Date"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("SupplierName") Then
                ds.Tables("rptSearchingCriteria").Columns("SupplierName").ColumnName = "Date Range From"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("BranchName") Then
                ds.Tables("rptSearchingCriteria").Columns("BranchName").ColumnName = "Date Range To"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Category") Then
                ds.Tables("rptSearchingCriteria").Columns("Category").ColumnName = "From Days"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Nomenclature") Then
                ds.Tables("rptSearchingCriteria").Columns("Nomenclature").ColumnName = "To Days"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Aircraft") Then
                ds.Tables("rptSearchingCriteria").Columns("Aircraft").ColumnName = "Category"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("KitName") Then
                ds.Tables("rptSearchingCriteria").Columns("KitName").ColumnName = "Bin Location"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("WorkOrderText") Then
                ds.Tables("rptSearchingCriteria").Columns("WorkOrderText").ColumnName = "Assembly"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("WorkOrderNo") Then
                ds.Tables("rptSearchingCriteria").Columns("WorkOrderNo").ColumnName = "Model"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Search1") Then
                ds.Tables("rptSearchingCriteria").Columns("Search1").ColumnName = "ATA Code"
            End If

            'Added on 27-Sep-2016
            ds.Tables("rptSearchingCriteria").Columns.Remove(ds.Tables("rptSearchingCriteria").Columns("From Days"))
            ds.Tables("rptSearchingCriteria").Columns.Remove(ds.Tables("rptSearchingCriteria").Columns("To Days"))
            ds.Tables("rptSearchingCriteria").Columns.Remove(ds.Tables("rptSearchingCriteria").Columns("Bin Location"))
            ds.Tables("rptSearchingCriteria").Columns.Remove(ds.Tables("rptSearchingCriteria").Columns("Assembly"))
            ds.Tables("rptSearchingCriteria").Columns.Remove(ds.Tables("rptSearchingCriteria").Columns("Model"))

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
			ds.Tables("rptStoreBalanceForAgingReport").TableName = "Aging Report For Store Balance"
			Session("ExcelFileName") = "Aging Report For Store Balance"
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "UnutilizedStoredBalance", "Export To Excel " + mStoreBalanceForAgingSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists

        mATAList = ATAList.GetATAList()
        ChkATACodeList.DataSource = mATAList

        'Model
        mModelList = ModelList.GetAirframeModelList("(All)")
        cmbModel.DataSource = mModelList
       Session("mModelList") = mModelList

        DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
            If cmbStore.Enabled = True Then
                setFocus(cmbStore)
            End If
            DataFieldBind()

        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport1(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport1(True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtCustomerList_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCustomerList.TextChanged
        If chkCustomerStock.Checked Then
            If txtCustomerList.Text.Trim <> "" Then                       'If Customer Selected
                SetCustomerID()
                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)      'Passing selected customer 
                cmbStore.DataSource = mStoreList
            Else
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)           'All
                cmbStore.DataSource = mStoreList
            End If
        End If
        cmbStore.DataBind()
        Session("mStoreList") = mStoreList
        upnlCustomerSelection.Update()
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then

            txtCustomerList.Enabled = True

            If txtCustomerList.Text.Trim <> "" Then

                SetCustomerID()

                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)
                cmbStore.DataSource = mStoreList
            Else
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)          'All
                cmbStore.DataSource = mStoreList
            End If
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
        Else
            txtCustomerList.Text = ""
            txtCustomerList.Enabled = False
            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)             'Self
            cmbStore.DataSource = mStoreList

            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
        End If
        upnlCustomerSelection.Update()
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
    End Sub
    Private Sub txtTodate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
    End Sub
    Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
        If Not IsDate(txtDate.Text) Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
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
#End Region

End Class