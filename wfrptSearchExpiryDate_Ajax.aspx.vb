'Created by Prashant

Public Class wfrptSearchExpiryDate_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public mNomenclatureList As NomenclatureList
    Public mNomenclature As NomenClature
    Public mCategoryList As CategoryList
    Public DateRange As String = ""
    Public FromDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public StoreName = "", Nomenclature = "", Category As String = ""
    Dim NameOfStore As String = ""
    Dim mExpiryDateSearchingCriteria As String = String.Empty
    Dim mStore As Store
    'Added by Abhishek on 13-SEP-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteria
    Dim rpt As rptExpiryDate
    Dim ds As New dsExpiryDate
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        mNomenclatureList = CType(Session("mNomenclatureList"), NomenclatureList)
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mStoreList") = mStoreList
        Session("mNomenclatureList") = mNomenclatureList
        Session("mCategoryList") = mCategoryList
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mStoreList")
        Session.Remove("mNomenclatureList")
        Session.Remove("mCategoryList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility()
        lblDateRange.Visible = False
        lblRangeDisp.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblStoreName.Visible = False
        lblNomenclatureName.Visible = False
        lblCategoryName.Visible = False
    End Sub
    Private Sub Display()
        lblDateRange.Visible = True
        lblRangeDisp.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblStoreName.Visible = True
        lblNomenclatureName.Visible = True
        lblCategoryName.Visible = True
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtDate.Text.Trim) Then
            FromDate = "1/1/1900"
        Else
            FromDate = txtDate.Text.Trim
        End If
        DateRange = cmbRange.SelectedItem.Text
        PartNo = IIf(IsNothing(PartNo) And PartNo = "", "", PartNo)
        Description = IIf(IsNothing(Description) And Description = "", "", Description)
        lblDateRange.Text = "Date : " & IIf(FromDate <> "1/1/1900", New SmartDate(txtDate.Text.Trim).FormattedText, "All")
        lblRangeDisp.Text = "Date Range : " & DateRange
        If cmbStoreList.SelectedIndex > 0 Then
            StoreName = Store.GetStore(New Guid(cmbStoreList.SelectedValue.ToString)).Name
            NameOfStore = IIf(cmbStoreList.SelectedIndex > 0, cmbStoreList.SelectedItem.Text, "")
        Else
            StoreName = ""
            NameOfStore = ""
        End If
        Nomenclature = IIf(cmbNomenclatureList.SelectedIndex > 0, cmbNomenclatureList.SelectedItem.Text, "")
        Category = IIf(cmbCategoryList.SelectedIndex > 0, cmbCategoryList.SelectedItem.Text, "")
        lblStoreName.Text = "Store Name  : " & IIf(NameOfStore <> "", NameOfStore, "All")
        lblNomenclatureName.Text = "Nomenclature Name : " & IIf(Nomenclature <> "", Nomenclature, "All")
        lblCategoryName.Text = "Category Name : " & IIf(Category <> "", Category, "All")

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
        mExpiryDateSearchingCriteria = lblDateRange.Text.Trim + ", " + lblRangeDisp.Text + ", " + lblStoreName.Text.Trim + ", " + lblCategoryName.Text + ", " + lblNomenclatureName.Text + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptExpiryDate
        SetValues()
        Dim ds As New dsExpiryDate
        myReport = New crptExpiryDate
        rpt = rptExpiryDate.GetExpiryDate(FromDate, PartNo, Description, Category, Nomenclature, StoreName, cmbRange.SelectedIndex, FromDate)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, "", PartNo, "", "", Category, Nomenclature, NameOfStore, "", "", Description, DateRange, 0, "", "", "", AppSettings("Logo"))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 509)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, rpt)
        da.Fill(ds, mrptImage)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "ExpiryDate", mExpiryDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategoryList.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList
        mNomenclatureList = NomenclatureList.GetNomenclatureList("(All)")
        cmbNomenclatureList.DataSource = mNomenclatureList
        Session("mNomenclatureList") = mNomenclatureList
        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStoreList.DataSource = mStoreList
        Session("mStorelist") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            RemoveSession()
            setFocus(cmbRange)
            DataFieldBind()
            cmbRange.SelectedIndex = 2
        End If
        MessageBoxResult()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mStoreList = Nothing
        mNomenclatureList = Nothing
        mCategoryList = Nothing
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
            rpt = rptExpiryDate.GetExpiryDate(FromDate, PartNo, Description, Category, Nomenclature, StoreName, cmbRange.SelectedIndex, FromDate)
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, "", PartNo, "", "", Category, Nomenclature, NameOfStore, "", "", Description, DateRange, 0, "", "", "", AppSettings("Logo"))
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 509)
            End If
            ds.Clear()

            da.Fill(ds, objsearch)
            da.Fill(ds, "ExcelrptExpiryDate", rpt)

            Dim columnToRemove1 As String() = {"ReceiptId", "SrNo", "ExpQtrs", "DateDifference", "StoreName", "Location", "ExpYear"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptExpiryDate").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptExpiryDate").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"CompanyName", "ToDate", "SupplierName", "BranchName", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("ReceiptDate") Then
                ds.Tables("ExcelrptExpiryDate").Columns("ReceiptDate").ColumnName = "Receipt Date "
            End If

            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("ReceiptText") Then
                ds.Tables("ExcelrptExpiryDate").Columns("ReceiptText").ColumnName = "Receipt No."
            End If

            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("PartName") Then
                ds.Tables("ExcelrptExpiryDate").Columns("PartName").ColumnName = "Part Number"
            End If


            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("PartDescription") Then
                ds.Tables("ExcelrptExpiryDate").Columns("PartDescription").ColumnName = "Description"
            End If

            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("SerialNo") Then
                ds.Tables("ExcelrptExpiryDate").Columns("SerialNo").ColumnName = "Serial No."
            End If

            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("NomenclatureName") Then
                ds.Tables("ExcelrptExpiryDate").Columns("NomenclatureName").ColumnName = "Nomenclature Name"
            End If

            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("CategoryName") Then
                ds.Tables("ExcelrptExpiryDate").Columns("CategoryName").ColumnName = "Category Name"
            End If
            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("RecQty") Then
                ds.Tables("ExcelrptExpiryDate").Columns("RecQty").ColumnName = "Qty."
            End If
            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("ExpiryDate") Then
                ds.Tables("ExcelrptExpiryDate").Columns("ExpiryDate").ColumnName = "Expiry Date"
            End If
            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("ExpQtrYear") Then
                ds.Tables("ExcelrptExpiryDate").Columns("ExpQtrYear").ColumnName = "ExpQtr/Year"
            End If
            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("StoreNameLocation") Then
                ds.Tables("ExcelrptExpiryDate").Columns("StoreNameLocation").ColumnName = "Store-Location"
            End If
            If ds.Tables("ExcelrptExpiryDate").Columns.Contains("BatchNo") Then
                ds.Tables("ExcelrptExpiryDate").Columns("BatchNo").ColumnName = "Batch No."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelrptExpiryDate"))

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptExpiryDate").TableName = "Expiry Date "
			Session("ExcelFileName") = "Expiry Date "
			Session("dsNew") = dsNew
			Session("DataTableToBeFormattedForExportToExcel") = "Expiry Date"
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "ExpiryDate", "Export To Excel " + mExpiryDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
End Class