Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.ModelListAutoComplete

Public Class wfrptSearchReOderLevelItem_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private mCategoryList As CategoryList
    Private PartNo As String = ""
    Private Description As String = ""
    Private strCategory As String = ""
    Private strNomenclature As String = ""
    Dim mReOderLevelItemSearchingCriteria As String = String.Empty
    'Added By Vikrant On 27-Oct-2014 For ALL27102014
    Dim mModelID As Guid = Guid.Empty
    Dim mModelList As ModelList
    'End
    Public mStoreList As StoreList
#End Region

#Region "Helper Methods"
    Private Sub GetSession()
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mModelList = CType(Session("mModelList"), ModelList) 'Added By Vikrant On 27-Oct-2014 For ALL27102014
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
        Session("Description") = Description
        Session("PartNo") = PartNo
        Session("mModelList") = mModelList  'Added By Vikrant On 27-Oct-2014 For ALL27102014
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryList")
        Session.Remove("Description")
        Session.Remove("PartNo")
        Session.Remove("mModelList") 'Added By Vikrant On 27-Oct-2014 For ALL27102014
    End Sub
    Private Sub SetValues()
        strCategory = ""
        strNomenclature = ""
        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        strCategory = IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "")
        strNomenclature = ""
        lblCategoryName.Text = "Category : " & IIf(strCategory <> "", strCategory, "All")
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        'Added By Vikrant On 27-Oct-2014 For ALL27102014
        If txtModelList.Text.Trim <> "" Then
            mModelID = mModelList.Item(txtModelList.Text.Trim).ID
        Else
            mModelID = Guid.Empty
        End If
        lblModel.Text = "Model       : " & IIf(txtModelList.Text.Trim <> "", txtModelList.Text.Trim, "All")
        'End
        mReOderLevelItemSearchingCriteria = lblCategoryName.Text.Trim + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim + ", " + lblModel.Text.Trim
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim rpt As rptReOrderLevelItem
        Call SetValues()
        rpt = rptReOrderLevelItem.GetMinReOrderItem(PartNo, Description, strCategory,
                                                    "", Guid.Empty, False,
                                                    mModelID.ToString, WithAlternatePatrs:=chkCheckForAlternatePart.Checked,
                                                    IsBAReorderQtyFormulaRequired:=IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", True, False),
                                                    SortBy:=CInt(cmbSortBy.SelectedValue), ClientCode:=AppSettings("ClientCode"),
                                                    StoreName:=IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedValue.ToString))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (rpt.Count > 0 And IsExcel = False) Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1208)
        End If

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myreport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ObjSearch As rptSearchingCriteria
        Dim ds As New dsReOrderLevel

        ObjSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, "", "", strCategory, "", store:=cmbStore.SelectedItem.Text, "", IIf(txtModelList.Text.Trim <> "", txtModelList.Text.Trim, ""), Description, "", 0, "", IIf(chkCheckForAlternatePart.Checked = True, "Considered Alternate Patrs Stock", ""), "", AppSettings("Logo"), , Search9:=Today.Date.ToString(AppSettings("DateFormat")), Search10:=AppSettings("ClientCode"), Search8:=cmbSortBy.SelectedItem.ToString)

        If AppSettings("ClientCode") = "Taj" Then
            myreport = New crptReOrderLevelItemFormatForTaj
        ElseIf AppSettings("ClientCode") = "STR" Then
            If cmbFormat.SelectedIndex = 0 Then
                myreport = New crptReOrderLevelItemForSTR
            Else
                myreport = New crptReOrderLevelItemFormat2ForSTR
            End If
        Else
            If cmbFormat.SelectedIndex = 0 Then
                myreport = New crptReOrderLevelItem
            Else
                myreport = New crptReOrderLevelItemFormat2
            End If
        End If

        If IsExcel = False Then
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, rpt)
            da.Fill(ds, ObjSearch)
            da.Fill(ds, mrptImage)
            myreport.SetDataSource(ds)
            Session("CrystalReport") = myreport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "Re-OrderLevelItems", mReOderLevelItemSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            ds.Clear()
            da.Fill(ds, "ExcelrptReOrderLevelItem", rpt)
            da.Fill(ds, "rptSearchingCriteria", ObjSearch)


            Dim columnToRemove As String() = {"CompanyName", "FromDate", "ToDate", "SupplierName", "BranchName", "Store", "Aircraft", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search10"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove(i))
                End If
            Next
            Dim ColumnToRemoveFromExcelrptReOrderLevelItem As String()
            If AppSettings("ClientCode") = "Taj" Then
                ColumnToRemoveFromExcelrptReOrderLevelItem = {"Nomenclature", "Category", "MinReOrderLevel",
                                                              "OrderQty", "ReturnQty", "StockQty", "OrderNumber",
                                                              "EnquiryNumber", "Note", "PONosYetToReceive", "RequisitionNumber",
                                                              "Unit", "ReOrderQty", "Rate"}

            Else
                ColumnToRemoveFromExcelrptReOrderLevelItem = {"Unit", "ReOrderQty", "Rate", "OrderQuantity", "OrderQuantityForExcel"}
            End If

            For j As Integer = 0 To ColumnToRemoveFromExcelrptReOrderLevelItem.Length - 1
                If ds.Tables("ExcelrptReOrderLevelItem").Columns.Contains(ColumnToRemoveFromExcelrptReOrderLevelItem(j)) Then
                    ds.Tables("ExcelrptReOrderLevelItem").Columns.Remove(ColumnToRemoveFromExcelrptReOrderLevelItem(j))
                End If
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelrptReOrderLevelItem"))

            dsNew.Tables("rptSearchingCriteria").Columns("Search8").ColumnName = "Sort By"
            dsNew.Tables("rptSearchingCriteria").Columns("Search9").ColumnName = "Report Date"
            dsNew.Tables("rptSearchingCriteria").Columns("KitName").ColumnName = "Model"

            If AppSettings("ClientCode") = "Taj" Then
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("PartName").SetOrdinal(0)
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("PartDescription").SetOrdinal(1)
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("MinStockLevel").SetOrdinal(2)
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("TotalStockQty").SetOrdinal(3)
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("OrderQuantity").SetOrdinal(4)


                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("PartName").ColumnName = "Part No."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("PartDescription").ColumnName = "Part Description"
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("MinStockLevel").ColumnName = "Min. Qty."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("TotalStockQty").ColumnName = "Stock Qty."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("OrderQuantity").ColumnName = "Order Qty."
            Else
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("MinReOrderLevel").ColumnName = "Re-Order Qty."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("OrderQty").ColumnName = "Order Qty."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("ReturnQty").ColumnName = "Return Qty."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("StockQty").ColumnName = "Serviceable Stock Qty."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("OrderNumber").ColumnName = "Purchase Order No."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("EnquiryNumber").ColumnName = "Enquiry No."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("Note").ColumnName = "Remarks"
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("TotalStockQty").ColumnName = "Total Stock Qty."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("PartName").ColumnName = "PartNo."
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("PONosYetToReceive").ColumnName = "PO Nos. yet to Receive"
                dsNew.Tables("ExcelrptReOrderLevelItem").Columns("RequisitionNumber").ColumnName = "Requisition No.(Req.Qty./Receipt Qty.)"
            End If


            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptReOrderLevelItem").TableName = "Re-Order Items"

            If AppSettings("ClientCode") = "Taj" Then
                Dim ExcelrptReOrderLevelItemTemp As New List(Of String)
                ExcelrptReOrderLevelItemTemp.AddRange(New String() {"Stock Qty.", "Order Qty."})
                Session("OrderQuantityColumns") = ExcelrptReOrderLevelItemTemp
            End If
			Session("ExcelFileName") = "Re-OrderLevelItems"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "Re-OrderLevelItems", "Export To Excel " + mReOderLevelItemSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub Display()
        lblCategoryName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblModel.Visible = True 'Added By Vikrant On 27-Oct-2014 For ALL27102014
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

#Region "Data Binding"
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(ALL)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList
        'Added By Vikrant On 27-Oct-2014 For ALL27102014
        mModelList = ModelList.GetModelList(0, "", , , "(All)")
        Session("mModelList") = mModelList
        'End
        'Store
        mStoreList = StoreList.GetStoreList(0, "", "(ALL)")
        cmbStore.DataSource = mStoreList
        DataBind()
        cmbSortBy.SelectedValue = IIf((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS"), "2", "1")
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            RemoveSession()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mCategoryList = Nothing
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
#End Region

#Region " Service Methods "
    'Added By Vikrant On 27-Oct-2014 For ALL27102014
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
                    Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                    Select c.Name).Take(count).ToList
        End If
    End Function
    'End
#End Region


End Class