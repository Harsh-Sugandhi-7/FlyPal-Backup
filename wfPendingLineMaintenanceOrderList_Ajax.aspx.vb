Public Class wfPendingLineMaintenanceOrderList_Ajax
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mLineMaintInvoice As LineMaintenanceInvoice
	Public mLineMaintPendingOrderList As LineMaintenanceOrderList
	Public mLineMaintPendingOrderItemList As PendingLineMaintenanceOrderItemList
#End Region

#Region "Business Methods"
	Private Sub GetSession()
		mLineMaintInvoice = Session("mLineMaintInvoice")
		mLineMaintPendingOrderList = Session("mLineMaintPendingOrderList")
		mLineMaintPendingOrderItemList = Session("mLineMaintPendingOrderItemList")
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mLineMaintPendingOrderList")
		Session.Remove("mLineMaintPendingOrderItemList")
	End Sub
	Private Sub ControlVisibility()
		If mLineMaintInvoice.LineMaintenanceInvoiceItems.Count = 0 Then
			txtDate.Enabled = True
		Else
			txtDate.Enabled = False
		End If
		If rdbOrders.Checked = True Then
			dgPendingList.Visible = True
			lblResult.Visible = True
			dgPendingItem.Visible = False
			lblResult1.Visible = False
		Else
			dgPendingList.Visible = False
			lblResult.Visible = False
			dgPendingItem.Visible = True
			lblResult1.Visible = True
		End If
	End Sub
	Private Sub GridBind(ByVal PendingOrderList As Boolean, ByVal PendingOrderItemList As Boolean)
		If PendingOrderList Then
			dgPendingList.DataSource = mLineMaintPendingOrderList
			dgPendingList.DataBind()
		ElseIf PendingOrderItemList Then
			dgPendingItem.DataSource = mLineMaintPendingOrderItemList
			dgPendingItem.DataBind()
		End If
	End Sub
	Private Sub DataFieldBind()
		If rdbOrders.Checked = True Then
			dgPendingList.PageIndex = 0
			If Session("LineMaintOrderID") IsNot Nothing Then
				mLineMaintPendingOrderList = LineMaintenanceOrderList.GetPendingOrderList(txtDate.Text, True, Session("LineMaintOrderID").ToString)
			Else
				mLineMaintPendingOrderList = LineMaintenanceOrderList.GetPendingOrderList(txtDate.Text, True)
			End If

			GridBind(True, False)
			lblResult.Text = "Pending Service Order List : " + CStr(mLineMaintPendingOrderList.Count) + " Record(s) Found"
			Session("mLineMaintPendingOrderList") = mLineMaintPendingOrderList
		Else
			dgPendingItem.PageIndex = 0
			If Session("LineMaintOrderID") IsNot Nothing Then
				mLineMaintPendingOrderItemList = PendingLineMaintenanceOrderItemList.GetPendingOrderItemList(txtDate.Text, Session("LineMaintOrderID").ToString)
			Else
				mLineMaintPendingOrderItemList = PendingLineMaintenanceOrderItemList.GetPendingOrderItemList(txtDate.Text)
			End If
			GridBind(False, True)
			lblResult1.Text = "Pending Service Order Item List : " + CStr(mLineMaintPendingOrderItemList.Count) + " Record(s) Found"
			Session("mLineMaintPendingOrderItemList") = mLineMaintPendingOrderItemList
		End If
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Ok
					Session("Sender") = ""

				Case Else
					Session("Sender") = ""
			End Select
		ElseIf Result1 = -1 Then
			Session("Sender") = ""
		End If
	End Sub
	Private Sub setOrderList(ByVal index As Integer)
		Dim mtmpPendingLineMaintenanceOrderItemList As PendingLineMaintenanceOrderItemList
		mtmpPendingLineMaintenanceOrderItemList = PendingLineMaintenanceOrderItemList.GetPendingOrderItemList(txtDate.Text, mLineMaintPendingOrderList(index).ID.ToString)
		For i As Integer = 0 To mtmpPendingLineMaintenanceOrderItemList.Count - 1
			If mLineMaintInvoice.LineMaintenanceInvoiceItems.Contains(mtmpPendingLineMaintenanceOrderItemList(i).ID) Then
				'skip
			Else
				mLineMaintInvoice.LineMaintenanceInvoiceItems.Add(mLineMaintInvoice.ID)
				With mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem
					.JobDetails = mtmpPendingLineMaintenanceOrderItemList(i).JobDetails
					.Qty = mtmpPendingLineMaintenanceOrderItemList(i).Qty
					.Unit = mtmpPendingLineMaintenanceOrderItemList(i).Unit
					.CRate = mtmpPendingLineMaintenanceOrderItemList(i).CRate
					.Remark = mtmpPendingLineMaintenanceOrderItemList(i).Remark
					.Note = mtmpPendingLineMaintenanceOrderItemList(i).Note
					.LineMaintOrderItemID = mtmpPendingLineMaintenanceOrderItemList(i).ID
					.LineMaintOrderDate = mtmpPendingLineMaintenanceOrderItemList(i).OrderDate
					.LineMaintOrderText = mtmpPendingLineMaintenanceOrderItemList(i).Text
					.LineMaintOrderNumber = mtmpPendingLineMaintenanceOrderItemList(i).No
				End With
			End If
		Next
		mLineMaintInvoice.CurrencyID = mLineMaintPendingOrderList(index).CurrencyID
		mLineMaintInvoice.VendorID = mLineMaintPendingOrderList(index).VendorID
		mLineMaintInvoice.MachineID = mLineMaintPendingOrderList(index).MachineID
		mLineMaintInvoice.LocationID = mLineMaintPendingOrderList(index).LocationID
		mLineMaintInvoice.LineMaintenanceInvoiceDate = CDate(txtDate.Text)
		Session("mLineMaintInvoice") = mLineMaintInvoice
		RemoveSession()
		Response.Redirect("wfLineMaintenanceInvoice_Ajax.aspx?BackPage=Index.aspx")
	End Sub
	Private Sub setOrderItemList(ByVal index As Integer)
		If mLineMaintInvoice.LineMaintenanceInvoiceItems.Contains(mLineMaintPendingOrderItemList(index).ID) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Service Invoice Item", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			mLineMaintInvoice.LineMaintenanceInvoiceItems.Add(mLineMaintInvoice.ID)
			With mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem
				.JobDetails = mLineMaintPendingOrderItemList(index).JobDetails
				.Qty = mLineMaintPendingOrderItemList(index).Qty
				.Unit = mLineMaintPendingOrderItemList(index).Unit
				.CRate = mLineMaintPendingOrderItemList(index).CRate
				.Remark = mLineMaintPendingOrderItemList(index).Remark
				.Note = mLineMaintPendingOrderItemList(index).Note
				.LineMaintOrderItemID = mLineMaintPendingOrderItemList(index).ID
				.LineMaintOrderDate = mLineMaintPendingOrderItemList(index).OrderDate
				.LineMaintOrderText = mLineMaintPendingOrderItemList(index).Text
				.LineMaintOrderNumber = mLineMaintPendingOrderItemList(index).No
			End With
			mLineMaintInvoice.CurrencyID = mLineMaintPendingOrderItemList(index).CurrencyID
			mLineMaintInvoice.VendorID = mLineMaintPendingOrderItemList(index).VendorID
			mLineMaintInvoice.MachineID = mLineMaintPendingOrderItemList(index).MachineID
			mLineMaintInvoice.LocationID = mLineMaintPendingOrderItemList(index).LocationID
			mLineMaintInvoice.LineMaintenanceInvoiceDate = CDate(txtDate.Text)
			Session("mLineMaintInvoice") = mLineMaintInvoice
			RemoveSession()
			Response.Redirect("wfLineMaintenanceInvoiceItem_Ajax.aspx?BackPage=wfLineMaintenanceInvoice_Ajax.aspx&ChildPage=Index.aspx")
		End If
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		GetSession()
		If Not IsPostBack And Session("sender") = "" Then
			If txtDate.Text = "" Then
				txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			End If
			DataFieldBind()
			ControlVisibility()
		End If
	End Sub
	Private Sub dgPendingList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingList.PageIndexChanging
		dgPendingList.PageIndex = e.NewPageIndex
		Session("mLineMaintPendingOrderList") = mLineMaintPendingOrderList
		GridBind(True, False)
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		DataFieldBind()
		ControlVisibility()
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		RemoveSession()
		If Request.QueryString("BackPage") = "Index.aspx" Then
			Response.Redirect("Index.aspx")
		Else
			'mLineMaintInvoice.LineMaintenanceInvoiceItems.RemoveAt(mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentIndex)
			Session("Edit") = False
			Response.Redirect(Request.QueryString("BackPage"))
		End If
	End Sub
	Private Sub rdbOrderItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbOrderItem.CheckedChanged
		DataFieldBind()
		ControlVisibility()
	End Sub
	Private Sub rdbOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbOrders.CheckedChanged
		DataFieldBind()
		ControlVisibility()
	End Sub
	Private Sub dgPendingList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingList.RowCommand
		Select Case e.CommandName
			Case "Select"
				GridBind(True, False)
				Dim Index As Integer = CInt(e.CommandArgument) + dgPendingList.PageIndex * dgPendingList.PageSize
				setOrderList(Index)
		End Select
	End Sub
	Private Sub dgPendingList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingList.Sorting
		mLineMaintPendingOrderList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		GridBind(True, False)
		Session("mLineMaintPendingOrderList") = mLineMaintPendingOrderList
	End Sub
	Private Sub dgPendingItem_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingItem.PageIndexChanging
		dgPendingItem.PageIndex = e.NewPageIndex
		Session("mLineMaintPendingOrderItemList") = mLineMaintPendingOrderItemList
		GridBind(False, True)
	End Sub
	Private Sub dgPendingItem_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingItem.RowCommand
		Select Case e.CommandName
			Case "Select"
				GridBind(False, True)
				Dim Index As Integer = CInt(e.CommandArgument) + dgPendingItem.PageIndex * dgPendingItem.PageSize
				setOrderItemList(Index)
		End Select
	End Sub
	Private Sub dgPendingItem_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingItem.Sorting
		mLineMaintPendingOrderItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		GridBind(False, True)
		Session("mLineMaintPendingOrderItemList") = mLineMaintPendingOrderItemList
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
		DataFieldBind()
		ControlVisibility()
	End Sub
#End Region



End Class