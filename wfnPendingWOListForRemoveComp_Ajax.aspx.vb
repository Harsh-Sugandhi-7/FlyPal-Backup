Public Class wfnPendingWOListForRemoveComp_Ajax
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mnPendingWOListForRemoveComp As nPendingWOListForRemoveComp
	Public mnPendingWOItemListForRemovedComp As nPendingWOItemListForRemovedComp
	Public mnPendingWOItemListForRemovedCompInfo As nPendingWOItemListForRemovedComp.nPendingWOItemListForRemovedCompInfo
	Public mReceiptCumInvoice As ReceiptCumInvoice
	Public WOID As Guid
	Public ItemID As Guid         'Added By Utkarsh On 3-Feb-2011
	Public mItemList As ItemList  'Added By Utkarsh On 3-Feb-2011
	Public Description As String  'Added By Utkarsh On 9-Feb-2011
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mnPendingWOListForRemoveComp = Session("mnPendingWOListForRemoveComp")
		mnPendingWOItemListForRemovedComp = Session("mnPendingWOItemListForRemovedComp")
		mReceiptCumInvoice = Session("mReceiptCumInvoice")
		WOID = Session("WOID")
		mItemList = Session("mItemList")            'Added By Utkarsh On 3-Feb-2011
		ItemID = Session("ItemID")                  'Added By Utkarsh On 3-Feb-2011
		Description = Session("Description")        'Added By Utkarsh On 9-Feb-2011
	End Sub
	Private Sub setSession()
		Session("mnPendingWOListForRemoveComp") = mnPendingWOListForRemoveComp
		Session("mnPendingWOItemListForRemovedComp") = mnPendingWOItemListForRemovedComp
		Session("mItemList") = mItemList        'Added By Utkarsh On 3-Feb-2011
		Session("ItemID") = ItemID              'Added By Utkarsh On 3-Feb-2011
		Session("Description") = Description    'Added By Utkarsh On 9-Feb-2011
	End Sub
	Private Sub setdatagrid(ByVal lookintype As Integer, ByVal ItemName As String) '--Added By Utkarsh On 3-Feb-2011
		dgPartSearch.Visible = True
		lblResult2.Visible = True
		lblFooterNote.Visible = True
		dgPartSearch.DataSource = Nothing
		mItemList = ItemList.GetItemList(lookintype, ItemName.Substring(0, 3), "", "", "", "", "", False)
		dgPartSearch.DataSource = mItemList
		dgPartSearch.DataBind()
		Session("mItemList") = mItemList
		lblResult2.Text = "List of Parts as  per criteria:" & mItemList.Count & " Record(s) found."
		upnlPartSearch.Update()
	End Sub '--------------------------------
	Private Sub setObject(ByVal Index As Int32)
		mnPendingWOItemListForRemovedComp = Session("mnPendingWOItemListForRemovedComp")
		mnPendingWOItemListForRemovedCompInfo = mnPendingWOItemListForRemovedComp.Item(Index)

		If mnPendingWOItemListForRemovedCompInfo.IsInventoryPart = False Then '---Added By Utkarsh On 3-Feb-2011
			' setdatagrid(1, mnPendingWOItemListForRemovedCompInfo.OffPartNo)
			mnPendingWOItemListForRemovedCompInfo.ItemID = CType(Session("ItemID"), Guid)
			If mnPendingWOItemListForRemovedCompInfo.OffDescription = "" Then
				mnPendingWOItemListForRemovedCompInfo.OffDescription = CType(Session("Description"), String)
			End If
			dgPartSearch.Visible = False
			lblResult2.Visible = False
			lblFooterNote.Visible = False
		End If
		Dim mItem As Item = Item.GetItem(mnPendingWOItemListForRemovedCompInfo.ItemID)
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 17 'From Work Order
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartFromListisSerialized = mItem.SerialisedStatus
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mnPendingWOItemListForRemovedCompInfo.ItemID '----
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BaseUnitID = mItem.UnitID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mItem.UnitID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WOJobCompID = mnPendingWOItemListForRemovedCompInfo.ID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = 1 'CDec(IIf(mItem.SerialisedStatus, 1, 0))
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mnPendingWOItemListForRemovedCompInfo.OffPartNo
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mnPendingWOItemListForRemovedCompInfo.OffDescription
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = mnPendingWOItemListForRemovedCompInfo.OffSerialNo
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Remark = mnPendingWOItemListForRemovedCompInfo.OffRemark
		mReceiptCumInvoice.WOID = WOID

		'If mItem.ExpiryMonths > 0 Then
		'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoice.Receipt.RecdDate
		'    If Not (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate) Is System.DBNull.Value Then
		'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = CDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate).AddMonths(mItem.ExpiryMonths)
		'    End If
		'End If
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID = mItem.ItemTagID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName = mItem.ItemTagName

		'Added on  07-Sep-2016 by Shital
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss = mItem.IsAirworthiCheck

		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		Session("TotalCount") = CDec(IIf(mItem.SerialisedStatus, 1, 0)).ToString
		Session("mTotalPendingItemQty") = CDec(IIf(mItem.SerialisedStatus, 1, 0)).ToString
		mItem = Nothing
		Session("Pending") = False
		Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx" & "&ChildPage1=" & "wfSearchPartListForRCI_Ajax.aspx")
	End Sub
	Private Sub SetTitle()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order as per criteria : " & mnPendingWOListForRemoveComp.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "E.O. No."
			dgWOList.Columns(2).HeaderText = "E.O.Date"
		Else
			lblResult.Text = "List of W.O. as per criteria :" & mnPendingWOListForRemoveComp.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "W.O. No."
			dgWOList.Columns(2).HeaderText = "W.O.Date"
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		txtDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted
		mnPendingWOListForRemoveComp = nPendingWOListForRemoveComp.GetnPendingWOListForRemoveComp(txtDate.Text, mReceiptCumInvoice.WOID.ToString)
		dgWOList.DataSource = mnPendingWOListForRemoveComp
		Session("mnPendingWOListForRemoveComp") = mnPendingWOListForRemoveComp
		DataBind()
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		getSession()
		If txtDate.Text = "" Then
			txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
		End If
		If Not IsPostBack Then
			DataFieldBind()
			dgPartSearch.Visible = False 'Added By Utkarsh On 3-Feb-2011
			lblResult2.Visible = False 'Added By Utkarsh On 9-Feb-2011
			lblFooterNote.Visible = False
		End If
		SetTitle()
	End Sub
	Private Sub dgWOList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOList.PageIndex * dgWOList.PageSize
				WOID = mnPendingWOListForRemoveComp(Index).ID
				dgPartSearch.Visible = False 'Added By Utkarsh On 3-Feb-2011
				lblResult2.Visible = False   'Added By Utkarsh On 9-Feb-2011
				lblFooterNote.Visible = False
				mnPendingWOItemListForRemovedComp = nPendingWOItemListForRemovedComp.GetnPendingWOItemListForRemovedComp(WOID)
				Session("mnPendingWOItemListForRemovedComp") = mnPendingWOItemListForRemovedComp
				Session("WOID") = WOID
				dgSparesList.DataSource = mnPendingWOItemListForRemovedComp
				dgSparesList.DataBind()
				If mnPendingWOItemListForRemovedComp.Count > 0 Then lblNote.Visible = True : upnlPartSearch.Update()
				lblResult1.Text = "List of spares For WO. as per criteria :" & mnPendingWOItemListForRemovedComp.Count & " Record(s) found."
				dgWOList.DataSource = mnPendingWOListForRemoveComp
				dgWOList.DataBind()
				upnlSparesList.Update()
		End Select
	End Sub
	Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
		If mReceiptCumInvoice.IsNew Then
			mReceiptCumInvoice.RecCumInvDate = txtDate.Text
		End If
		dgWOList.PageIndex = 0
		mnPendingWOListForRemoveComp = nPendingWOListForRemoveComp.GetnPendingWOListForRemoveComp(txtDate.Text, mReceiptCumInvoice.WOID.ToString)
		dgWOList.DataSource = mnPendingWOListForRemoveComp
		Session("mnPendingWOListForRemoveComp") = mnPendingWOListForRemoveComp
		dgWOList.DataBind()
		SetTitle()
		upnlWOList.Update()

		dgSparesList.Visible = False
		lblResult1.Visible = False
		lblFooterNote.Visible = False
		upnlSparesList.Update()

		dgPartSearch.Visible = False
		lblResult2.Visible = False
		lblNote.Visible = False
		upnlPartSearch.Update()
	End Sub
	Private Sub dgSparesList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSparesList.RowCommand
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgSparesList.PageIndex * dgSparesList.PageSize
				Session("mIndex") = Index 'Added By Utkarsh On 3-Feb-2011
				dgSparesList.DataSource = mnPendingWOItemListForRemovedComp
				dgSparesList.DataBind()
				If mnPendingWOItemListForRemovedComp.Item(Index).IsInventoryPart = False Then '--Added By Utkarsh On 4-Feb-2011
					setdatagrid(1, mnPendingWOItemListForRemovedComp.Item(Index).OffPartNo)
					Exit Sub
				End If '-------------------------------
				setObject(Index)
				'Session("mReceiptCumInvoice") = mReceiptCumInvoice
				'Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?ChildPage1=" & "wfnPendingWOListForRemoveComp_Ajax.aspx&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage"))
		End Select
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If Request.QueryString("BackPage") = "wfReceiptCumInvoice_Ajax.aspx" Then 'ReceiptcumInvoice
			mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
			Session("mReceiptCumInvoice") = mReceiptCumInvoice
		End If
		Response.Redirect(Request.QueryString("BackPage"))
	End Sub
	Private Sub dgWOList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting 'Added by Utkarsh 22-Dec-2010
		mnPendingWOListForRemoveComp.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mnPendingWOListForRemoveComp
		Session("mnPendingWOListForRemoveComp") = mnPendingWOListForRemoveComp
		dgWOList.DataBind()
	End Sub '----------------------------------
	Private Sub dgSparesList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSparesList.Sorting 'Added by Utkarsh 22-Dec-2010
		mnPendingWOItemListForRemovedComp.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgSparesList.DataSource = mnPendingWOItemListForRemovedComp
		Session("mnPendingWOItemListForRemovedComp") = mnPendingWOItemListForRemovedComp
		dgSparesList.DataBind()
		upnlSparesList.Update()
	End Sub '--------------------------------
	Private Sub dgPartSearch_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand 'Added By Utkarsh On 3-Feb-2011
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index As Int16 = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
				ItemID = mItemList(Index).ID
				Description = mItemList(Index).Description
				Session("ItemID") = ItemID
				Session("Description") = Description
				setObject(CType(Session("mIndex"), Integer))
		End Select
	End Sub
	Private Sub dgPartSearch_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartSearch.Sorting
		mnPendingWOItemListForRemovedComp.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgPartSearch.DataSource = mItemList
		Session("mItemList") = mItemList
		dgPartSearch.DataBind()
		upnlPartSearch.Update()
	End Sub '--------------------------------
	Private Sub btnAddPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPart.Click
		Dim mItem As Item
		mItem = Item.NewItem
		Session("mItem") = mItem
		Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfnPendingWOListForRemoveComp_Ajax.aspx" & "&ChildPage1=" & "index.aspx")
	End Sub
	Private Sub dgWOList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		lblResult1.Visible = True
		dgWOList.DataSource = mnPendingWOListForRemoveComp
		Session("mnPendingWOListForRemoveComp") = mnPendingWOListForRemoveComp
		dgWOList.DataBind()
		upnlWOList.Update()
	End Sub
#End Region

End Class