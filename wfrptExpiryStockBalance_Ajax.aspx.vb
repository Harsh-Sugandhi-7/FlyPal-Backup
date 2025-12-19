Imports System.Text
Imports System.Collections.Generic
Public Class wfrptExpiryStockBalance_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public ToDate As String
    Public PartNo As String
    Public Description As String
    Dim EventLogID As Guid 'Added by Prashant
    Dim mExpiryStockBalanceSearchingCriteria As String = String.Empty
    'Added By Vikrant On 23-July-2014
    Public mCategoryLists As CategoryList
    Public StrCategory As String = String.Empty
    Dim mCategoryID As Guid
    'End
    Dim StoreIDList, StoreNameList As String
    Public StrStore As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList) 'Added By Vikrant On 23-July-2014
    End Sub
    Private Sub SetSession()
        Session("mAircraftList") = mStoreList
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("mCategoryLists") = mCategoryLists 'Added By Vikrant On 23-July-2014
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mCategoryLists") 'Added By Vikrant On 23-July-2014
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Controlvisibility(ByVal Index As Int16)
        lblStoreName.Visible = False
        lblDateRange.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblCategoryName.Visible = False
    End Sub
    Private Sub SetValues()
        ToDate = txtDate.Text.ToString
        lblDateRange.Text = "Date : " & New SmartDate(txtDate.Text.ToString).FormattedText
        'Code added by shweta on 30/12/11
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")
        ''End
        StoreIDList = hdnStoreIDList.Value
        StoreNameList = hdnStoreNameList.Value
        StrStore = IIf(StoreNameList = String.Empty, "All", StoreNameList)

        Session("PartNo") = PartNo
        Session("Description") = Description

        lblPartNo.Text = "Part No. : " + IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " + IIf(Description <> "", Description, "All")
        lblStoreName.Text = "Store : " + StrStore 'IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")


        'Added By Vikrant On 23-July-2014
        If cmbCategory.SelectedIndex = 0 Then
            StrCategory = ""
            mCategoryID = Guid.Empty
            lblCategoryName.Text = "Category Name : All"
        Else
            StrCategory = cmbCategory.SelectedItem.ToString
            mCategoryID = New Guid(cmbCategory.SelectedValue)
            lblCategoryName.Text = "Category : " & StrCategory
        End If
        'End
        mExpiryStockBalanceSearchingCriteria = lblDateRange.Text.Trim + ", " + lblStoreName.Text.Trim + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim + ", " + cmbOrderBy.SelectedItem.Text.Trim + ", " + lblCategoryName.Text
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteria
        Dim ds As New dsExpiryStockBalance
        Dim rpt As rptExpiryStockBalance
        Dim StoreIDXML As New StringBuilder
        myReport = New crptExpiryStoreBalance

        If StoreIDList.ToString <> "" Then
            StoreIDXML.Append("<StoreIDs>")
            For Each value As String In StoreIDList.Split(",")
                StoreIDXML.Append("<ID>")
                StoreIDXML.Append(value)
                StoreIDXML.Append("</ID>")
            Next
            StoreIDXML.Append("</StoreIDs>")
        End If

        rpt = rptExpiryStockBalance.GetExpiryStockBalance(ToDate, PartNo, Description, "", cmbOrderBy.SelectedIndex, StoreIDXML.ToString, cmbCategory.SelectedValue.ToString, IsValuedStore:=chkIsValuedStore.Checked)
        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), ToDate, "", PartNo, "", "", "", "", StoreNameList.ToString, "", "", Description, AppSettings("Logo"), FromStore:=StrCategory, Search1:=txtBottomLine.Text)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not IsExcel Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 702)
        End If
        ds.Clear()

        If IsExcel Then
            Dim PeriodColumnsForExportToExcel As New List(Of String)

            da.Fill(ds, "ExcelrptExpiryStockBalance", rpt)
            da.Fill(ds, "rptSearchingCriteria", objSearch)
            Dim columnToRemove2 As String() = {"ToDate", "CompanyName", "SupplierName", "BranchName", "Category", "Nomenclature", "Aircraft", "KitName", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "RelNoteNo"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "ExpiryDateDBValue", "StoreName", "LocationName", "Text", "No", "ReceiptDate"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExcelrptExpiryStockBalance").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelrptExpiryStockBalance").Columns.Remove(columnToRemove(i))
                End If
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelrptExpiryStockBalance"))

            dsNew.Tables("rptSearchingCriteria").Columns("FromStore").ColumnName = "Category"

            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("ReceiptDateFormatted").ColumnName = "ReceiptDate"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("PartName").ColumnName = "Part No"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("InvoiceNumber").ColumnName = "Invoice No"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("PeriodMonth").ColumnName = "Period(Month)"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("PeriodQtrs").ColumnName = "Period(Qtrs)"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("ManDate").ColumnName = "StartDate"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("CureQtrYear").ColumnName = "Start Qtrs"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("ExpQtrYear").ColumnName = "Expiry Qtrs"
            dsNew.Tables("ExcelrptExpiryStockBalance").Columns("StoreLocationName").ColumnName = "Store"

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptExpiryStockBalance").TableName = "Expiry Stock Balance"

            PeriodColumnsForExportToExcel.AddRange(New String() {"Period(Qtrs)", "Period(Month)", "BalQty"})
            Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            Session("DataTableToBeFormattedForExportToExcel") = "Expiry Stock Balance"
			Session("ExcelFileName") = "Expiry Stock Balance"
			Session("dsNew") = dsNew

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "ExpiryStockBalance", "Export To excel " + mExpiryStockBalanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        Else
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, objSearch)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

            MarkLog(Util.Action.Print, "ExpiryStockBalance", mExpiryStockBalanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        mStoreList = StoreList.GetStoreList(0, "", , True)
        'cmbStore.DataSource = mStoreList
        ChkStoreList.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        'Added By Vikrant On 23-July-2014
        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists
        'End

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            txtDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            DataFieldBind()
            Controlvisibility(2)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRange.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblStoreName.Visible = True
        lblCategoryName.Visible = True
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mStoreList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class