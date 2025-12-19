'Created by utkarsh on 04-oct-2013

Public Class wfrptChangeLocation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStockItemList As StockItemList
    Public mOpenState As Boolean
    Dim PartNo As String
    Dim Location, SearchIndex, PartType, PartNoLocation As String
    Public mCurrentLocation As String
    Public mReceiptItemID As Guid
    'Added by Vikrant on 3-AUG-2011
    Dim EventLogID As Guid
    Public mPartName As String
    Public mLocation As String
    Public mPartType As String
    'Added By Vikrant On 21-Aug-2013 For ALL20082013-2
    Public mStoreList As StoreList
    Public StoreID As Guid
    'End
    Public ItemID As Guid
    Public SerialNo As String
    Dim mCompanyDetail As New CompanyDetail
    Public mCategoryLists As CategoryList
#End Region

#Region " Helper Methods "
    Private Sub GetSessionForLocation()
        mCurrentLocation = CType(Session("mCurrentLocation"), String)
    End Sub
    Private Sub RemoveSessionForPartStore()
        Session.Remove("mItemTypeList")
        Session.Remove("ChangeItemTypeID")
        Session.Remove("ChangeItemTypeName")
        Session.Remove("ChangeStore")
        Session.Remove("IsStoreChangeble")
        Session.Remove("ChangeStoreID")
        Session.Remove("ChangeStoreList")
    End Sub
    Private Sub RemoveSessionForLocation()
        Session.Remove("mCurrentLocation")
    End Sub
    Private Sub GetSession()
        mReceiptItemID = CType(Session("mReceiptItemID"), Guid)
        mStockItemList = CType(Session("mStockItemListForGrid"), StockItemList)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        Location = IIf(IsNothing(Session("Location")), "", Session("Location"))

        PartType = IIf(IsNothing(Session("PartType")), "", Session("PartType"))
        SearchIndex = IIf(IsNothing(Session("SearchIndex")), "", Session("SearchIndex"))
        PartNoLocation = Session("PartNoLocation")
        'Added By Vikrant On 21-Aug-2013 For ALL20082013-2
        'mStoreList = Session("mStoreList")
        StoreID = Session("StoreID")
        'End
        ItemID = CType(Session("ItemID"), Guid)
        SerialNo = Session("SerialNo")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Location")
        Session.Remove("PartType")
        Session.Remove("SearchIndex")
        Session.Remove("PartNoLocation")
        Session.Remove("mStockItemListForGrid")
        'Added By Vikrant On 21-Aug-2013 For ALL20082013-2
        Session.Remove("mStoreList")
        Session.Remove("StoreID")
        'End
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptChangeLocation_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub ChangeLocation(ByVal mReceiptItemID As Guid, ByVal mCurrentLocation As String, ByVal ItemID As Guid, ByVal SerialNo As String)
        Session("mReceiptItemID") = mReceiptItemID
        Session("mCurrentLocation") = mCurrentLocation
        Session("ItemID") = ItemID
        Session("SerialNo") = SerialNo
    End Sub
    Private Sub SetChangePartStore(ByVal mReceiptItemID As Guid, ByVal mItemTypeID As Integer, ByVal Name As String, ByVal Store As String, ByVal IsStoreChangeble As String, ByVal StoreID As Guid)
        Session("mReceiptItemID") = mReceiptItemID
        Session("ChangeItemTypeID") = mItemTypeID
        Session("ChangeItemTypeName") = Name
        Session("ChangeStore") = Store
        Session("IsStoreChangeble") = IsStoreChangeble
        Session("ChangeStoreID") = StoreID
        txtCurrentPT.Text = Name
        txtCurrentStore.Text = Store
        If cmbPT.Enabled = True Then
            setFocus(cmbPT)
        End If
        DataFieldBindForChangePartStore()
        cmbPT.SelectedValue = mItemTypeID
        cmbChangeStore.SelectedValue = StoreID.ToString
        ControlVisibilityForChangePartStore(CBool(IsStoreChangeble))
        upnlChangePartStore.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility1(ByVal SearchIndex As Int32)
        If SearchIndex = 0 Then
            lblFor.Visible = False
            txtSearchFor.Visible = False
            cmbPartType.Visible = False
            cmbPartType.SelectedIndex = 0
            cmbStore.Visible = False
            cmbStore.SelectedIndex = 0
        ElseIf SearchIndex = 1 Then
            lblFor.Visible = True
            txtSearchFor.Visible = True
            txtSearchFor.Text = PartNo
            cmbPartType.Visible = False
            cmbPartType.SelectedIndex = 0
            cmbStore.Visible = False
            cmbStore.SelectedIndex = 0
        ElseIf SearchIndex = 2 Then
            lblFor.Visible = True
            txtSearchFor.Visible = True
            txtSearchFor.Text = Location
            cmbPartType.Visible = False
            cmbPartType.SelectedIndex = 0
            cmbStore.Visible = False
            cmbStore.SelectedIndex = 0
        ElseIf SearchIndex = 3 Then
            lblFor.Visible = False
            txtSearchFor.Visible = False
            cmbPartType.Visible = True
            cmbStore.Visible = False
            cmbStore.SelectedIndex = 0
        ElseIf SearchIndex = 4 Then 'Added By Vikrant On 21-Aug-2013 For ALL20082013-2
            lblFor.Visible = False
            txtSearchFor.Visible = False
            cmbPartType.Visible = False
            cmbPartType.SelectedIndex = 0
            cmbStore.Visible = True
        End If
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        Location = ""
    End Sub
    Private Sub FindNow(ByVal LookinType As Integer, Optional ByVal ItemName As String = "", Optional ByVal Location As String = "", _
                        Optional ByVal ItemTypeID As Integer = 0, Optional ByVal StoreID As String = "{00000000-0000-0000-0000-000000000000}", _
                        Optional ByVal CategoryID As String = "{00000000-0000-0000-0000-000000000000}")
        'This step is Imp when details form  is opened dirctly.
        If LookinType = -1 Then
            LookinType = 0
        End If

        gdPartSearch.DataSource = Nothing
        mStockItemList = Nothing

        'Get List From the Database as per Criteria
        mStockItemList = StockItemList.GetStockItemList(PartNo, Location, ItemTypeID, StoreID, chkBlankLocation.Checked, CategoryID)

        'Set DataSource of the Grid
        gdPartSearch.DataSource = mStockItemList
        Session("mStockItemListForGrid") = mStockItemList

    End Sub
    Public Sub SetControl()
        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        Location = Session("Location")
        PartType = Session("PartType")
        'Added By Vikrant On 21-Aug-2013 For ALL20082013-2
        If cmbSearch.SelectedIndex = 4 Then
            StoreID = Session("StoreID")
        Else
            StoreID = Guid.Empty
        End If

        FindNow(SearchIndex, PartNo, Location, PartType, StoreID.ToString, cmbCategory.SelectedValue.ToString)
        'Commented and added bye utkarsh ajax
        'gdPartSearch.DataBind()
        PartSearchGridBind()

        cmbSearch.SelectedIndex = SearchIndex
        cmbPartType.SelectedValue = PartType

        ControlVisibility1(SearchIndex)
        lblResult.Text = "List of Parts : " & mStockItemList.Count & " Record(s) found. "
    End Sub
    'added by utkarsh on 04-oct-2013
    Private Sub PartSearchGridBind()
        gdPartSearch.DataBind()
        upnlgrid.Update()
    End Sub
    Private Sub UpdateSearchPanel()
        upnlSearch.Update()
    End Sub
    Private Sub StoreListBind()
        'If Not Session("mStoreList") Is Nothing Then
        '    mStoreList = Session("mStoreList")
        'Else
        mStoreList = StoreList.GetStoreList(0, "", True, True)
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        'End If
        cmbStore.DataSource = mStoreList
        cmbStore.DataBind()
        'cmbStore.SelectedValue = StoreID.ToString
    End Sub
    Private Sub BindValueForChangeLocation()
        txtCurrentLocation.Text = mCurrentLocation
        If txtChangedLocation.Enabled = True Then
            setFocus(txtChangedLocation)
        End If
        upnlLocation.Update()
    End Sub
    Private Sub ClearLocationControls()
        txtChangedLocation.Text = ""
    End Sub
    Private Sub ClearChangePartStoreControls()
        ChangeStoreName.Value = ""
        ChangeStoreValue.Value = ""
        ChangeItemTypeName.Value = ""
        ChangeItemTypeValue.Value = ""
    End Sub
    Private Sub ControlVisibilityForChangePartStore(ByVal enableStore As Boolean)
        cmbChangeStore.Enabled = IIf(enableStore, True, False)
    End Sub
    'End
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsStockItemList
        myReport = New crptCostComparisonOfAPart

        FindNow(CInt(cmbSearch.SelectedIndex), PartNo, Location, CInt(cmbPartType.SelectedValue), StoreID.ToString, cmbCategory.SelectedValue.ToString)

        If mStockItemList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1394)
            MarkLog(Util.Action.Print, "StockItemList", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
       mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
       mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
      "Part Cost Comparison", PartNo, Location, PartType, SearchStr4:=IIf(cmbSearch.SelectedIndex = 4 And cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), SearchStr5:=IIf(cmbPartType.SelectedIndex = 0, "", cmbPartType.SelectedItem.Text), _
        ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=IIf(cmbCategory.SelectedIndex = 0, "", cmbCategory.SelectedItem.Text), SearchStr7:="", SearchStr8:="", SearchStr9:="", _
        SearchStr10:=AppSettings("Logo"), SearchStr11:="")

        If IsExcel = False Then     'PDF format
            'ds.Clear()
            'Dim mrptImage As rptImage = rptImage.GetImage(ds)
            'da.Fill(ds, mrptImage)
            'da.Fill(ds, mStockItemList)
            'da.Fill(ds, Report)
            'myReport.SetDataSource(ds)
            'Session("CrystalReport") = myReport
            'Dim Str As String
            'Str = "openTranDetail();"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        ElseIf IsExcel = True Then  'Excel format
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "StockItemList", mStockItemList)

            Dim columnToRemove2 As String() = {"ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr3", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"ID", "Text", "No", "Date", "ItemTypeID", "Name", "StartDate", "StartDateFormatted", "ExpiryDate", "ExpiryDateFormatted", "CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "CureQtrYear", "ExpQtrYear", "ExpiryMonth", "ExpiryQuarter", "IsExpiryMonth", "IsExpiryQuarter", "ExpiryPeriod", "IsStoreChangeble", "StoreID", "ItemID"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("StockItemList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("StockItemList").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("StockItemList").Columns.Contains("ItemName") Then
                ds.Tables("StockItemList").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("StockItemList").Columns.Contains("ItemDesc") Then
                ds.Tables("StockItemList").Columns("ItemDesc").ColumnName = "Description"
            End If
            If ds.Tables("StockItemList").Columns.Contains("SerialNo") Then
                ds.Tables("StockItemList").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("StockItemList").Columns.Contains("ItemTypeStatus") Then
                ds.Tables("StockItemList").Columns("ItemTypeStatus").ColumnName = "Part Type (Part Status)"
            End If

            If ds.Tables("StockItemList").Columns.Contains("DateFormatted") Then
                ds.Tables("StockItemList").Columns("DateFormatted").ColumnName = "Receipt Date"
            End If

            If ds.Tables("StockItemList").Columns.Contains("StockBalQty") Then
                ds.Tables("StockItemList").Columns("StockBalQty").ColumnName = "Qty. in Stock"
            End If
            If ds.Tables("StockItemList").Columns.Contains("ReceiptNo") Then
                ds.Tables("StockItemList").Columns("ReceiptNo").ColumnName = "Receipt No."
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Part No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Location"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Part Type"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Store"
            End If
            'SearchStr6
            If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Category"
            End If
            Dim dsNew As New DataSet

            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("StockItemList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("StockItemList").TableName = "Change Part Location-Type-Store"
			Session("ExcelFileName") = "Change Part Location-Type-Store"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        cmbCategory.DataBind()

        mStockItemList = StockItemList.GetStockItemList("", "", 0, , chkBlankLocation.Checked, cmbCategory.SelectedValue.ToString)
        gdPartSearch.DataSource = mStockItemList
        Session("mStockItemListForGrid") = mStockItemList



        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        Location = Session("Location")
        PartType = Session("PartType")
        lblResult.Text = "List of Parts : " & mStockItemList.Count & " Record(s) found "
        PartSearchGridBind()
        UpdateSearchPanel()
    End Sub
    Private Sub DataFieldBindForChangePartStore()
        Dim mItemTypeList As ItemTypeList
        If Session("mItemTypeList") Is Nothing Then
            mItemTypeList = ItemTypeList.GetItemTypeList()
            cmbPT.DataSource = mItemTypeList
            Session("mItemTypeList") = mItemTypeList
        Else
            cmbPT.DataSource = CType(Session("mItemTypeList"), ItemTypeList)
        End If
        cmbPT.DataBind()

        If Session("ChangeStoreList") Is Nothing Then
            mStoreList = StoreList.GetStoreList(0, "", False, True)
            cmbChangeStore.DataSource = mStoreList
            Session("ChangeStoreList") = mStoreList
        Else
            cmbChangeStore.DataSource = CType(Session("ChangeStoreList"), StoreList)
        End If
        cmbChangeStore.DataBind()
    End Sub
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 3-AUG-2011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptChangeLocation_Ajax.aspx?"
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            StoreListBind()
            DataFieldBind()
            'SetControl()
        End If
    End Sub
    Protected Sub gdPartSearch_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gdPartSearch.RowCommand
        Select Case e.CommandName
            Case "ChangeLocation"
                Dim index As Integer = CInt(e.CommandArgument) + gdPartSearch.PageIndex * gdPartSearch.PageSize
                mPartName = mStockItemList(index).ItemName
                Dim mReceiptItemID As Guid = (mStockItemList(index).ID)
                mCurrentLocation = mStockItemList(index).Location
                If mCurrentLocation = "&nbsp;" Then mCurrentLocation = ""
                ChangeLocation(mReceiptItemID, mCurrentLocation, mStockItemList(index).ItemID, mStockItemList(index).SerialNo)
                MarkLog(Util.Action.Edit, "ChangePartLocation", "Part : " + mPartName + " Location : " + mCurrentLocation, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                BindValueForChangeLocation()
                mdlPopUpChangeLocation.Show()
                gdPartSearch.DataSource = mStockItemList
                Session("mStockItemListForGrid") = mStockItemList
                PartSearchGridBind()
            Case "ChangePartType"    'Added Code
                'Added by Vikrant on 3-AUG-2011
                Dim index As Integer = CInt(e.CommandArgument) + gdPartSearch.PageIndex * gdPartSearch.PageSize
                mPartName = mStockItemList(index).ItemName
                mPartType = mStockItemList(index).ItemTypeStatus
                MarkLog(Util.Action.Edit, "ChangePartLocation", "Part : " + mPartName + " Type : " + mPartType, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                Dim mReceiptItemID As Guid = mStockItemList(index).ID
                Dim mItemTypeID As Integer = mStockItemList(index).ItemTypeID
                Dim ItemType As String = mStockItemList(index).ItemTypeStatus
                'Added By Vikrant On 21-Aug-2013 For ALL20082013-2
                Dim Store As String = mStockItemList(index).Store
                Dim IsStoreChangeble As String = mStockItemList(index).IsStoreChangeble
                Dim StoreID As Guid = mStockItemList(index).StoreID
                SetChangePartStore(mReceiptItemID, mItemTypeID, ItemType, Store, IsStoreChangeble, StoreID)
                mdlPopUpChangePartStore.Show()
                gdPartSearch.DataSource = mStockItemList
                Session("mStockItemListForGrid") = mStockItemList
                PartSearchGridBind()
        End Select

    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        ClearControls()
        ControlVisibility1(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click

        gdPartSearch.PageIndex = 0
        Try
            SearchIndex = cmbSearch.SelectedIndex
            PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
            Location = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
            PartType = cmbPartType.SelectedValue

            If cmbSearch.SelectedIndex = 4 Then
                If (StoreValue.Value <> "") Then
                    StoreID = New Guid(StoreValue.Value)
                Else
                    StoreID = Guid.Empty
                End If
            Else
                StoreID = Guid.Empty
            End If

            Session("SearchIndex") = SearchIndex
            Session("PartNo") = PartNo
            Session("Location") = Location
            Session("PartType") = PartType
            Session("StoreID") = StoreID 'Added By Vikrant On 20-Aug-2013 For ALL20082013-2

            FindNow(SearchIndex, PartNo, Location, PartType, StoreID.ToString, cmbCategory.SelectedValue.ToString)
            lblResult.Text = "List of Parts : " & mStockItemList.Count & " Record(s) found "
            ControlVisibility1(cmbSearch.SelectedIndex)
            PartSearchGridBind()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        Try
            MarkLog(Action.Close, "ChangePartLocation", "", ErrorType.NoError, Guid.Empty, EventLogID)
            mStockItemList = Nothing
            RemoveSession()
            Session("MiddleFrame") = ""
            Response.Redirect("DashBoard.aspx")
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    'Added By Prashant 22-June-2009 for grid sorting
    Private Sub gdPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdPartSearch.Sorting
        mStockItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mStockItemListForGrid") = mStockItemList
        gdPartSearch.DataSource = mStockItemList
        PartSearchGridBind()
    End Sub
    '------------------------------------------------
    'Added By Vikrant On 21-Aug-2013 For ALL20082013-2
    Protected Sub gdPartSearch_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdPartSearch.PageIndexChanging
        gdPartSearch.PageIndex = e.NewPageIndex
        gdPartSearch.DataSource = mStockItemList
        Session("mStockItemListForGrid") = mStockItemList
        PartSearchGridBind()
    End Sub
    'End
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
#End Region

#Region "Change Location"
    Protected Sub btnLocationOk_Click(sender As Object, e As EventArgs) Handles btnLocationOk.Click
        GetSessionForLocation()
        Try
            StockItemList.ChangeLocation(mReceiptItemID, txtChangedLocation.Text.Trim, ItemID:=ItemID, SerialNo:=SerialNo)
            MarkLog(Util.Action.Save, "Location", "Loacation : " + txtChangedLocation.Text.Trim, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            mdlPopUpChangeLocation.Hide()
            RemoveSessionForLocation()
            ClearLocationControls()
            SetControl()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnLocationClose_Click(sender As Object, e As EventArgs) Handles btnLocationClose.Click
        MarkLog(Util.Action.Close, "Location", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionForLocation()
        mdlPopUpChangeLocation.Hide()
    End Sub
#End Region

#Region "Change Part/Store"
    Protected Sub btnChangePartOk_Click(sender As Object, e As EventArgs) Handles btnChangePartOk.Click
        Try
            Dim ItemType As String = IIf(ChangeItemTypeName.Value = "", Session("ChangeItemTypeName").ToString, ChangeItemTypeName.Value)
            Dim StoreName As String = IIf(ChangeStoreName.Value = "", Session("ChangeStore").ToString, ChangeStoreName.Value)

            Dim ItemTypeID As String = IIf(ChangeItemTypeValue.Value = "", Session("ChangeItemTypeID").ToString, ChangeItemTypeValue.Value)
            Dim StoreID As String = IIf(ChangeStoreValue.Value = "", Session("ChangeStoreID").ToString, ChangeStoreValue.Value)

            If Not txtCurrentPT.Text.Equals(ItemType) Then
                StockItemList.ChangeItemTypeID(mReceiptItemID, CInt(ItemTypeID))
                MarkLog(Util.Action.Save, "Part Type", "Old Part Type : " + txtCurrentPT.Text + " New Part Type : " + ItemType, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
            'Added by Vikrant On 21-Aug-2013 For ALL20082013-2
            If CBool(Session("IsStoreChangeble")) And Not txtCurrentStore.Text.Equals(StoreName) Then
                StockItemList.ChangeStore(mReceiptItemID, New Guid(StoreID))
                MarkLog(Util.Action.Save, "Store", "Old Store : " + txtCurrentStore.Text + " New Store : " + StoreName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
            mdlPopUpChangePartStore.Hide()
            RemoveSessionForPartStore()
            ClearChangePartStoreControls()
            SetControl()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnChangePartClose_Click(sender As Object, e As EventArgs) Handles btnChangePartClose.Click
        MarkLog(Util.Action.Close, "Part Type", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mdlPopUpChangePartStore.Hide()
        RemoveSessionForPartStore()
    End Sub
#End Region

End Class