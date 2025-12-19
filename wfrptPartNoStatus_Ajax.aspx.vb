Public Class wfrptPartNoStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mItemList As ItemList
    Dim PartNo As String
    Dim Description As String
    Dim Nomenclature As String
    Dim Category As String
    Dim Unit As String
    Dim Location As String
    Dim ItemID As Guid
    Dim LinkID As Guid
    Public mStockPartStatus As rptStockPartStatus
    Public mOnOrderPartStatus As rptOnOrderPartStatus
    Public mReturnablePartStatus As rptReturnablePartStatus
    Public mTransitPartList As rptTransitPartList
    Public mRequisitionItems As RequisitionItems
    Dim SerialNo As String                              'Added By Utkarsh On 09-May-2012 FOR 09052012-2
    Public mRequisitionItemsNew As RequisitionItemsNew  'Added By Vikrant on 04-July-2012 For ALL04072012-2
    Dim SearchIndex, SearchText, BatchNo As String
    Dim GSENo As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        Description = IIf(IsNothing(Session("Description")), "", Session("Description"))
        Nomenclature = IIf(IsNothing(Session("Nomenclature")), "", Session("Nomenclature"))
        Category = IIf(IsNothing(Session("Category")), "", Session("Category"))
        Unit = IIf(IsNothing(Session("Unit")), "", Session("Unit"))
        Location = IIf(IsNothing(Session("Location")), "", Session("Location"))
        ItemID = Session("ItemID")
        LinkID = Session("LinkID")
        mStockPartStatus = CType(Session("mStockPartStatus"), rptStockPartStatus)
        mOnOrderPartStatus = CType(Session("mOnOrderPartStatus"), rptOnOrderPartStatus)
        mReturnablePartStatus = CType(Session("mReturnablePartStatus"), rptReturnablePartStatus)
        mTransitPartList = CType(Session("mTransitPartList"), rptTransitPartList)
        mRequisitionItems = CType(Session("mRequisitionItems"), RequisitionItems)
        SerialNo = Session("SerialNo")                                                      'Added By Utkarsh On 09-May-2012 FOR 09052012-2
        mRequisitionItemsNew = CType(Session("mRequisitionItemsNewForPartNoStatus"), RequisitionItemsNew)  'Added By Vikrant on 04-July-2012 For ALL04072012-2
        SearchIndex = Session("SearchIndex")
        SearchText = Session("SearchText")
        BatchNo = Session("BatchNo")
        GSENo = Session("GSENo")
    End Sub
    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("Nomenclature") = Nomenclature
        Session("Category") = Category
        Session("Unit") = Unit
        Session("Location") = Location
        Session("ItemID") = ItemID
        Session("LinkID") = LinkID
        Session("mStockPartStatus") = mStockPartStatus
        Session("mOnOrderPartStatus") = mOnOrderPartStatus
        Session("mReturnablePartStatus") = mReturnablePartStatus
        Session("mTransitPartList") = mTransitPartList
        Session("mRequisitionItems") = mRequisitionItems
        Session("SerialNo") = SerialNo                          'Added By Utkarsh On 09-May-2012 FOR 09052012-2
        Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew  'Added By Vikrant on 04-July-2012 For ALL04072012-2
        Session("SearchIndex") = SearchIndex
        Session("SearchText") = SearchText
        Session("BatchNo") = BatchNo
        Session("GSENo") = GSENo
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("Nomenclature")
        Session.Remove("Category")
        Session.Remove("Unit")
        Session.Remove("mItemList")
        Session.Remove("Location")
        Session.Remove("ItemID")
        Session.Remove("LinkID")
        Session.Remove("mStockPartStatus")
        Session.Remove("mOnOrderPartStatus")
        Session.Remove("mReturnablePartStatus")
        Session.Remove("mTransitPartList")
        Session.Remove("mRequisitionItems")
        Session.Remove("SerialNo")              'Added By Utkarsh On 09-May-2012 FOR 09052012-2
        Session.Remove("mRequisitionItemsNewForPartNoStatus")  'Added By Vikrant on 04-July-2012 For ALL04072012-2
        Session.Remove("BatchNo")
        Session.Remove("GSENo")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub ControlVisibility1(ByVal Index As Int16)
        lblFor.Visible = (Index <> 0)
        txtSearchFor.Visible = (Index <> 0)
    End Sub
    Private Sub FindNow(ByVal LookinType As Integer, Optional ByVal ItemName As String = "", Optional ByVal ItemDescription As String = "", Optional ByVal Nomenclature As String = "", Optional ByVal CategoryName As String = "", Optional ByVal UnitName As String = "", Optional ByVal Location As String = "", Optional ByVal SerialNo As String = "", Optional ByVal BatchNo As String = "", Optional ByVal GSENo As String = "") 'Changed By Utkarsh On 09-May-2012 FOR 09052012-2
        'This step is Imp when details form  is opened dirctly.
        If LookinType = -1 Then
            LookinType = 0
        End If
        dgPartSearch.DataSource = Nothing
        mItemList = Nothing
        'Get List From the Database as per Criteria
        mItemList = ItemList.GetItemList(LookinType, ItemName, ItemDescription, "", CategoryName, UnitName, Location, , SerialNo, BatchNo:=BatchNo, CodeNo:=GSENo)    'Changed By Utkarsh On 09-May-2012 FOR 09052012-2
        'Set DataSource of the Grid
        dgPartSearch.DataSource = mItemList
        dgPartSearch.DataBind()
        Session("mItemList") = mItemList
        lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
        upnlGrid.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        cmbSearch.SelectedIndex = SearchIndex
        txtSearchFor.Visible = IIf(cmbSearch.SelectedIndex > 0, True, False)
        txtSearchFor.Text = SearchText

        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        Description = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
        ' Nomenclature = IIf(cmbSearch.SelectedIndex = 3, Trim(txtSearchFor.Text), "")
        Nomenclature = ""
        Category = IIf(cmbSearch.SelectedIndex = 3, Trim(txtSearchFor.Text), "")
        Unit = IIf(cmbSearch.SelectedIndex = 4, Trim(txtSearchFor.Text), "")
        Location = IIf(cmbSearch.SelectedIndex = 5, Trim(txtSearchFor.Text), "")
        SerialNo = IIf(cmbSearch.SelectedIndex = 6, Trim(txtSearchFor.Text), "")
        BatchNo = IIf(cmbSearch.SelectedIndex = 7, Trim(txtSearchFor.Text), "")
        GSENo = IIf(cmbSearch.SelectedIndex = 8, Trim(txtSearchFor.Text), "")
        'mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)  'Added by Prashant 22-Feb-2013 ALL22022013
        If SerialNo.Length > 0 Or BatchNo.Length > 0 Or GSENo.Length > 0 Then
            mItemList = ItemList.GetItemList(8, PartNo, Description, Nomenclature, Category, Unit, Location, False, SerialNo, BatchNo:=BatchNo, CodeNo:=GSENo)  'Added by Prashant 22-Feb-2013 ALL22022013
        Else

            Dim index As Integer = 0
            If cmbSearch.SelectedIndex >= 3 Then
                index = cmbSearch.SelectedIndex + 1
            Else
                index = cmbSearch.SelectedIndex
            End If
            mItemList = ItemList.GetItemList(index, PartNo, Description, Nomenclature, Category, Unit, Location, False, SerialNo, BatchNo:=BatchNo, CodeNo:=GSENo)  'Added by Prashant 22-Feb-2013 ALL22022013
        End If
        dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            RemoveSession()
            ItemID = Guid.Empty
            LinkID = Guid.Empty
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            'Ajay 08-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "PartNoStatus") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            DataFieldBind()
        End If
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        ClearControls()
        Session("SearchIndex") = Index
        ControlVisibility1(Index)
        If cmbSearch.Enabled = True Then
            SetFocus(cmbSearch)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartSearch.PageIndex = 0
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        Description = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
        '' Nomenclature = IIf(cmbSearch.SelectedIndex = 3, Trim(txtSearchFor.Text), "")
        Nomenclature = ""
        Category = IIf(cmbSearch.SelectedIndex = 3, Trim(txtSearchFor.Text), "")
        Unit = IIf(cmbSearch.SelectedIndex = 4, Trim(txtSearchFor.Text), "")
        Location = IIf(cmbSearch.SelectedIndex = 5, Trim(txtSearchFor.Text), "")
        SerialNo = IIf(cmbSearch.SelectedIndex = 6, Trim(txtSearchFor.Text), "") 'Added By Utkarsh On 09-May-2012 FOR 09052012-2
        SearchIndex = cmbSearch.SelectedIndex
        SearchText = Trim(txtSearchFor.Text)
        BatchNo = IIf(cmbSearch.SelectedIndex = 7, Trim(txtSearchFor.Text), "")
        GSENo = IIf(cmbSearch.SelectedIndex = 8, Trim(txtSearchFor.Text), "")
        SetSession()

        'Added By Utkarsh On 09-May-2012 FOR 09052012-2
        If SerialNo.Length > 0 Or BatchNo.Length > 0 Or GSENo.Length > 0 Then
            FindNow(8, PartNo, Description, Nomenclature, Category, Unit, Location, SerialNo, BatchNo, GSENo:=GSENo)
        Else

            'Added by Saylee on 30-Dec-2021, as now Nomenclature is removed
            Dim index As Integer = 0
            If cmbSearch.SelectedIndex >= 3 Then
                index = cmbSearch.SelectedIndex + 1
            Else
                index = cmbSearch.SelectedIndex
            End If
            '****************************

            FindNow(index, PartNo, Description, Nomenclature, Category, Unit, Location, GSENo:=GSENo)
        End If
        'End
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session.Remove("SearchIndex")
        Session.Remove("SearchText")
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        dgPartSearch.DataBind()
    End Sub
    Private Sub dgPartSearch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
        Dim Index As Integer
        Select Case e.CommandName
            Case "Select"
                dgPartSearch.DataSource = mItemList
                dgPartSearch.DataBind()

                Index = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize

                ClearControls()
                ItemID = New Guid(mItemList(Index).ID.ToString)
                PartNo = mItemList(ItemID).Name
                Description = mItemList(ItemID).Description
                Nomenclature = mItemList(ItemID).Nomenclature
                Category = mItemList(ItemID).CategoryName
                Unit = mItemList(ItemID).UnitName
                Location = mItemList(ItemID).Location

                mItem = Item.GetItem(ItemID)
                LinkID = mItem.LinkID

                SetSession()
                Session("mItemFromPartNoStatus") = mItem
                mStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID, , chkIsValued.Checked)  'Changed By VIkrant on 07-Sept-2012 For ALL07092012
                mOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
                mReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
                mTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
                mRequisitionItems = RequisitionItems.GetRequisitionItemsForPartnoStatus(LinkID)
                mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode")) 'Added By Vikrant on 04-July-2012 For ALL04072012-2

                Session("mStockPartStatus") = mStockPartStatus
                Session("mOnOrderPartStatus") = mOnOrderPartStatus
                Session("mReturnablePartStatus") = mReturnablePartStatus
                Session("mTransitPartList") = mTransitPartList
                Session("mRequisitionItems") = mRequisitionItems
                Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew 'Added By Vikrant on 04-July-2012 For ALL04072012-2

                DataFieldBind()
                Dim str As String
                str = "openledgersame('wfrptShowPartNoStatus_Ajax.aspx?BackPage=Index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End Select
    End Sub
    Private Sub dgPartSearch_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartSearch.Sorting 'New addition by Rupali on 18-Jun-09 for Sorting Order 
        mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItemList") = mItemList
        dgPartSearch.DataSource = mItemList
        dgPartSearch.DataBind()
    End Sub
    'Ajay 08-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "PartNoStatus")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "PartNoStatus")
    End Sub
    '-----
#End Region

End Class