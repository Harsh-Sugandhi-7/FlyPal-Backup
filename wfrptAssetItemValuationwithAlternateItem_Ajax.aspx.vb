Public Class wfrptAssetItemValuationwithAlternateItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String = ""
    Public Description As String = ""
    Public mCategoryList As CategoryList  'Added By Utkarsh On 12-Oct-2012 FOR ALL11102012-2
    Public strCategory As String
    Dim EventLogID As Guid 'Added by Prashant
    Dim mAssetValuationSearchingCriteria As String = String.Empty
    Public mModelList As ModelList 'Added By Prashant 3-Mar-2014  ALL03032014
    Dim mModel As String = ""
    Public mStoreList As StoreList
    Dim email As Thread
    Dim mVendorList As VendorList
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mCategoryList = Session("mCategoryList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mCategoryList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        If Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 0 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblCategory1.Visible = True
        lblModel.Visible = True
        lblStoreName.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblCategory1.Visible = False
        lblModel.Visible = False
        lblStoreName.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 1 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Quater
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
            Case 3 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 4 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then      ''Date Range
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        'Added By Utkarsh On 12-Oct-2012 FOR ALL11102012-2
        strCategory = String.Empty
        For i As Integer = 0 To ChklistCategory.Items.Count - 1
            If ChklistCategory.Items(i).Selected Then
                If strCategory.Length = 0 Then
                    strCategory = ChklistCategory.Items(i).Text
                Else
                    strCategory = strCategory + "," + ChklistCategory.Items(i).Text
                End If
            End If
        Next

        lblCategory1.Text = "Category Name : " & IIf(strCategory.Length > 0, strCategory, "All")
        'End
        If (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = False) Then
            mModel = ""
        ElseIf (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = True) Then
            mModel = "Common/No Applicability"
        Else
            mModel = cmbModel.SelectedItem.Text
        End If
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblModel.Text = "Model : " & IIf(cmbModel.SelectedIndex = 0, "", cmbModel.SelectedItem.Text)
        lblStoreName.Text = "Store : " & IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedItem.Text)
        mAssetValuationSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblCategory1.Text + ", " + lblModel.Text + ", " + lblStoreName.Text + ", " + "Supplier : " & IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.Text)
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False, Optional ByVal ByMail As Boolean = False)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As AssetItemValuationwithAlternateItem 'rptAssetValuation
        Dim CategoryWise As Integer = 0
        Dim Value As String = ""
        Dim ReportName As String = ""
        SetValues()

        If chkForCategoryWise.Checked = True Then
            myReport = New crptAssetItemValuationwithAlternateItemCategoryWise
            CategoryWise = 1
        Else
            myReport = New crptAssetItemValuationwithAlternateItem
        End If
        rpt = AssetItemValuationwithAlternateItem.GetAssetItemValuationwithAlternateItem(FromDate, ToDate, PartNo, Description, strCategory, cmbModel.SelectedValue, CategoryWise, _
                                                       chkCommonOrApplicability.Checked, StoreID:=cmbStore.SelectedValue, SupplierID:=cmbSupplier.SelectedValue)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, _
                                                              IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.Text), mModel, strCategory, _
                                                              Description, store:=IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedItem.Text), _
                                                              Aircraft:="", KitName:="", Description:="", RelNoteNo:="", TransTypeID:=0, FromStore:="", _
                                                              WorkShop:="", WorkOrderText:="", WorkOrderNo:=AppSettings("Logo"), Search1:=txtBottomLine.Text.Trim)

        If ByMail = False Then
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1223)
            End If
        End If
        If (ByMail = True And rpt.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Asset Valuation", "Asset Valuation", "There is no record for this search criteria.", "", _
                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                     SmtpHost:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpHost, SmtpPort:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpPort, _
                                     SmtpUser:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpUser, SmtpPassword:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpPassword)
            Exit Sub
        End If
        If IsExcel = False Then         'PDF format
            Dim ds As New dsAssetItemValuationwithAlternateItem
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            MarkLog(Util.Action.Print, "AssetValuation", mAssetValuationSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Asset Valuation", "Asset Valuation", " For " + lblDateRangeFrom.Text, _
                                          "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                     SmtpHost:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpHost, SmtpPort:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpPort, _
                                     SmtpUser:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpUser, SmtpPassword:=mModuleList.Item("AssetItemValuationwithAlternateItem").SmtpPassword)
            End If
        ElseIf IsExcel = True Then
            Dim ds1 As New dsExcelAssetItemValuationwithAlternateItem
            ds1.Clear()
            da.Fill(ds1, rpt)
            da.Fill(ds1, objsearch)

            Dim columnToRemove1 As String() = {"ItemID", "CategoryID", "CategoryName", "CategoryGLCode", "PrimaryCategoryID", "ATACode", "ATANomenclature", "CapitalizedQty", "CapitalizedAmount", "TransTypeID", "LinkID"}
            Dim columnToRemove2 As String() = {"CompanyName", "BranchName", "Nomenclature", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}

            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds1.Tables("AssetItemValuationwithAlternateItem").Columns.Contains(columnToRemove1(i)) Then
                    ds1.Tables("AssetItemValuationwithAlternateItem").Columns.Remove(columnToRemove1(i))
                End If
            Next

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds1.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds1.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            ds1.Tables("rptSearchingCriteria").Columns("Aircraft").ColumnName = "Model"

            'If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
            '    ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            'End If
            'If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
            '    ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Customer"
            'End If

            If ds1.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("ConsumedQty") Then
                ds1.Tables("AssetItemValuationwithAlternateItem").Columns("ConsumedQty").ColumnName = "Issue Qty."
            End If
            If ds1.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("ConsumedAmount") Then
                ds1.Tables("AssetItemValuationwithAlternateItem").Columns("ConsumedAmount").ColumnName = "Issue Amount"
            End If
            'If ds.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("ReceiptSerialNo") Then
            '    ds.Tables("AssetItemValuationwithAlternateItem").Columns("ReceiptSerialNo").ColumnName = "Serial No."
            'End If
            'If ds.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("IssuedQty") Then
            '    ds.Tables("AssetItemValuationwithAlternateItem").Columns("IssuedQty").ColumnName = "Issued Qty."
            'End If
            'If ds.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("ReceiptReleaseNoteNo") Then
            '    ds.Tables("AssetItemValuationwithAlternateItem").Columns("ReceiptReleaseNoteNo").ColumnName = "Release Note No."
            'End If
            'If ds.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("CWPTextNo") Then
            '    ds.Tables("AssetItemValuationwithAlternateItem").Columns("CWPTextNo").ColumnName = "Issued to W/O"
            'End If
            'If ds.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("CustomerName") Then
            '    ds.Tables("AssetItemValuationwithAlternateItem").Columns("CustomerName").ColumnName = "Customer"
            'End If
            'If ds.Tables("AssetItemValuationwithAlternateItem").Columns.Contains("StoreName") Then
            '    ds.Tables("AssetItemValuationwithAlternateItem").Columns("StoreName").ColumnName = "Source"
            'End If

            'ds.Tables("AssetItemValuationwithAlternateItem").Columns("Part No.").SetOrdinal(0)
            'ds.Tables("AssetItemValuationwithAlternateItem").Columns("Description").SetOrdinal(1)
            ds1.Tables("AssetItemValuationwithAlternateItem").Columns("BinCardNumber").SetOrdinal(3)
            ds1.Tables("AssetItemValuationwithAlternateItem").Columns("Location").SetOrdinal(4)
            ds1.Tables("AssetItemValuationwithAlternateItem").Columns("SalesQty").SetOrdinal(13)
            ds1.Tables("AssetItemValuationwithAlternateItem").Columns("SalesAmount").SetOrdinal(14)
            'ds.Tables("AssetItemValuationwithAlternateItem").Columns("Release Note No.").SetOrdinal(4)
            'ds.Tables("AssetItemValuationwithAlternateItem").Columns("Issued to W/O").SetOrdinal(5)
            'ds.Tables("AssetItemValuationwithAlternateItem").Columns("Customer").SetOrdinal(6)
            'ds.Tables("AssetItemValuationwithAlternateItem").Columns("Source").SetOrdinal(7)

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds1.Tables("rptSearchingCriteria"))
            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Merge(ds1.Tables("AssetItemValuationwithAlternateItem"))
            dsNew.Tables("AssetItemValuationwithAlternateItem").TableName = "Asset Valuation"
			Session("ExcelFileName") = "Asset Valuation"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "AssetValuation", "Export To excel " + mAssetValuationSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
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
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("DT")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()
        Dim ItemID As New Guid
        If FetchItemByName.GetItemByName(PartNo).Count > 0 Then
            ItemID = FetchItemByName.GetItemByName(PartNo).Item(0).ID
        Else
            ItemID = Guid.Empty
        End If

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelCategorywiseAssetValuation"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@FromDate", FromDate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@CategoryList", strCategory)
        cmd.Parameters.AddWithValue("@ModelID", New Guid(cmbModel.SelectedValue))
        cmd.Parameters.AddWithValue("@ItemID", ItemID)

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(ByVal tbl As DataTable)
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(tbl)
        dsNew.Tables(0).TableName = "Category Wise Asset Valuation"
		Session("ExcelFileName") = "Category Wise Asset Valuation"
		Session("dsNew") = dsNew
		'Session("DataTable") = tbl
		'Session("ReportName") = "RCI Register"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Added By Utkarsh On 12-Oct-2012 FOR ALL11102012-2
        mCategoryList = CategoryList.GetCategoryList()
        ChklistCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList
        'End
        mModelList = ModelList.GetAirframeModelList("ALL")
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()
        'Store
        mStoreList = StoreList.GetStoreList(0, "", "ALL", True)
        cmbStore.DataSource = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        mVendorList = VendorList.GetVendorstList(0, , , , , , "ALL", , IsSupplier:=True)
        cmbSupplier.DataSource = mVendorList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            DataFieldBind()
            ControlVisibility(5)
            setDatePeroid(5)
            cmbDateRange.SelectedIndex = 5
        End If
        MessageBoxResult()
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False, False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True, False)
        Else
            upnlValidationsummary.Update()
        End If
        'If IsValid Then
        '    If chkForCategoryWise.Checked Then
        '        SetValues()
        '        GenerateXLSXFile(CreateDataTable())
        '    Else
        '        Dim da As New CSLA.Data.ObjectAdapter
        '        Dim objsearch As rptSearchingCriteria
        '        Dim rpt As AssetItemValuation
        '        Dim CategoryWise As Integer = 0
        '        SetValues()
        '        Dim ds As New dsExcelAssetItemValuation  'dsValuation
        '        CategoryWise = IIf(chkForCategoryWise.Checked, 1, 0)

        '        rpt = AssetItemValuation.GetAssetItemValuation(FromDate, ToDate, PartNo, Description, strCategory, cmbModel.SelectedValue, CategoryWise, _
        '                                                       chkCommonOrApplicability.Checked, StoreID:=cmbStore.SelectedValue, SupplierID:=cmbSupplier.SelectedValue)
        '        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, _
        '                                                              IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.Text), "", _
        '                                                              strCategory, "", store:=IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedItem.Text), _
        '                                                              Aircraft:=mModel, KitName:="", Description:=Description, RelNoteNo:="", TransTypeID:=0, _
        '                                                              FromStore:="", WorkShop:="", WorkOrderText:="", WorkOrderNo:=AppSettings("Logo"), _
        '                                                              Search1:=txtBottomLine.Text.Trim)

        '        If rpt.Count <= 0 Then
        '            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '            Exit Sub
        '        End If

        '        ds.Clear()
        '        da.Fill(ds, rpt)
        '        da.Fill(ds, objsearch)

        '        Dim columnToRemove1 As String() = {"ItemID", "CategoryID", "CategoryName", "CategoryGLCode", "PrimaryCategoryID", "ATACode", "ATANomenclature", "CapitalizedQty", "CapitalizedAmount"}
        '        Dim columnToRemove2 As String() = {"CompanyName", "BranchName", "Nomenclature", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}

        '        For i As Integer = 0 To columnToRemove2.Length - 1
        '            If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
        '                ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
        '            End If
        '        Next

        '        For i As Integer = 0 To columnToRemove1.Length - 1
        '            If ds.Tables("AssetItemValuation").Columns.Contains(columnToRemove1(i)) Then
        '                ds.Tables("AssetItemValuation").Columns.Remove(columnToRemove1(i))
        '            End If
        '        Next
        '        ds.Tables("rptSearchingCriteria").Columns("Aircraft").ColumnName = "Model"
        '        ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
        '        ds.Tables("AssetItemValuation").TableName = "Asset Valuation"

        '        Session("dsNew") = ds
        '        'Session("ReportName") = ReportName
        '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        '    End If
        'Else
        '    upnlValidationsummary.Update()
        'End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
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
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '     Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail 
        Session("UserEmailID") = mModuleList.Item("AssetItemValuationwithAlternateItem").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AssetItemValuationwithAlternateItem").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(False, True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
#End Region


End Class