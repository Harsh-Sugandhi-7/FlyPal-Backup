Public Class wfSalesOrderForPurchaseOrder_Ajax
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mSalesOrderForPurchaseOrderList As SalesOrderForPurchaseOrderList
	Public mSalesOrderItemsForPurchaseOrder As SalesOrderItemsForPurchaseOrder
	Public mOrder As Order
	Public mSelectList() As Boolean
	Public mPrevTransID As Guid
	Private mIsAll As Boolean = False
	Private mOrderDate As String
	Private mVendorID As Guid
	Private mSelectedQuotationIndex As Integer = -1
#End Region

#Region "Business Methods"
	Private Sub GetSession()
		mSelectedQuotationIndex = Session("mSelectedQuotationIndex")
		mOrder = Session("mOrder")
		mSalesOrderForPurchaseOrderList = Session("mSalesOrderForPurchaseOrderList")
		mSalesOrderItemsForPurchaseOrder = Session("mSalesOrderItemsForPurchaseOrder")
	End Sub
	Private Sub SetMultipleObject()
		Dim chkSelect As CheckBox
		Dim Recordno, PageItems As Integer
		PageItems = dgSalesOrderItemList.Rows.Count - 1
		For I As Integer = 0 To PageItems
			Recordno = I + dgSalesOrderItemList.PageSize * dgSalesOrderItemList.PageIndex
			chkSelect = CType(dgSalesOrderItemList.Rows(I).FindControl("chkSelect"), CheckBox)
			mSalesOrderItemsForPurchaseOrder(Recordno).IsSelected = chkSelect.Checked
			mSalesOrderItemsForPurchaseOrder(Recordno).MarkClean()
		Next
		Session("mSalesOrderItemsForPurchaseOrder") = mSalesOrderItemsForPurchaseOrder
	End Sub
	'----ADded by Shital on 04-Feb-2021
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Confirmation" Then
						Try
							Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")
							MarkLog(Util.Action.Save, "Pending Purchase Quotation Item list", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
						Catch ex As SqlException
							MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
							Exit Sub
						End Try
					End If

			End Select
		End If
	End Sub
	'--------
#End Region

#Region "Data Binding"
	Public Sub DataFieldBind()
		If mIsAll Then
			mSalesOrderForPurchaseOrderList = SalesOrderForPurchaseOrderList.GetSalesOrderForPurchaseOrderList(txtDate.Text, Guid.Empty.ToString, Guid.Empty.ToString)
		Else
			mSalesOrderForPurchaseOrderList = SalesOrderForPurchaseOrderList.GetSalesOrderForPurchaseOrderList(txtDate.Text, Guid.Empty.ToString, mPrevTransID.ToString)
		End If
		dgSalesOrderList.DataSource = mSalesOrderForPurchaseOrderList
		Session("mSalesOrderForPurchaseOrderList") = mSalesOrderForPurchaseOrderList
		dgSalesOrderList.DataBind()
		lblResult.Text = "List of Sales Order : " + mSalesOrderForPurchaseOrderList.Count.ToString + " Record (s) found"
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then
			If mPrevTransID.Equals(Guid.Empty) Then
				rdbFromAllPendingQuotation.Checked = True
			Else
				rdbFromLastQuotation.Checked = True
			End If
			If txtDate.Text = "" Then
				txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			End If
			If mOrder.IsNew Then
				txtDate.Enabled = True
				rdbFromLastQuotation.Checked = False
				rdbFromAllPendingQuotation.Checked = True
				txtDate.Text = mOrder.OrderDateFormatted
				If mOrder.OrderItems.Count - 1 = -1 Then
					txtDate.Enabled = True
				Else
					txtDate.Enabled = False
				End If
			Else
				txtDate.Enabled = False
				rdbFromLastQuotation.Checked = True
				rdbFromAllPendingQuotation.Checked = False
				txtDate.Text = mOrder.OrderDateFormatted
			End If
			DataFieldBind()
		End If
	End Sub
	Private Sub rdbFromLastQuotation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbFromLastQuotation.CheckedChanged
		mIsAll = False
	End Sub
	Private Sub rdbFromAllPendingQuotation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbFromAllPendingQuotation.CheckedChanged
		mIsAll = True
	End Sub
	Private Sub dgSalesOrderList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSalesOrderList.RowCommand
		Select Case e.CommandName
			Case "SelectRecord"
				Dim index As Integer = CInt(e.CommandArgument) + dgSalesOrderList.PageIndex * dgSalesOrderList.PageSize
				mSelectedQuotationIndex = index
				Session("mSelectedQuotationIndex") = mSelectedQuotationIndex
				mSalesOrderItemsForPurchaseOrder = SalesOrderItemsForPurchaseOrder.GetSalesOrderForPurchaseOrder(mSalesOrderForPurchaseOrderList.Item(index).ID)
				dgSalesOrderItemList.DataSource = mSalesOrderItemsForPurchaseOrder
				Session("mSalesOrderItemsForPurchaseOrder") = mSalesOrderItemsForPurchaseOrder
				dgSalesOrderItemList.DataBind()
				lblResult1.Text = "List of Sales Order Item (s): " + mSalesOrderItemsForPurchaseOrder.Count.ToString + " Record (s) found"
				If mSalesOrderItemsForPurchaseOrder.Count >= 0 Then
					btnDone.Enabled = True
				Else
					btnDone.Enabled = False
				End If
				upnlButtons.Update()
				upnlSalesOrderItemList.Update()
		End Select
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		DataFieldBind()
		upnlSalesOrderList.Update()
		upnlSalesOrderItemList.Update()
	End Sub
	Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
		If mSelectedQuotationIndex > -1 Then
			If mOrder.VendorID.Equals(Guid.Empty) Then
				With mSalesOrderForPurchaseOrderList(mSelectedQuotationIndex)
					mOrder.OrderDate = txtDate.Text
					mOrder.CurrencyID = .CurrencyID
					mOrder.ConversionFactor = .ConversionFactor
				End With
			End If
		End If

		SetMultipleObject()
		Session("SalesOrderItems") = "True"
		'Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx") 'Commented  by Shital on 15-Feb-2021

		'Added by Shital on 15-Feb-2021
		Dim chkSelect As CheckBox
		Dim ItemNames As String = ""
		Dim Recordno, PageItems As Integer
		PageItems = dgSalesOrderItemList.Rows.Count - 1
		For I As Integer = 0 To PageItems
			Recordno = I + dgSalesOrderItemList.PageSize * dgSalesOrderItemList.PageIndex
			chkSelect = CType(dgSalesOrderItemList.Rows(I).FindControl("chkSelect"), CheckBox)
			If chkSelect.Checked And mSalesOrderItemsForPurchaseOrder(Recordno).orderItemReceiptBalanceQuantity > 0.0 Then
				ItemNames = ItemNames + mSalesOrderItemsForPurchaseOrder(Recordno).SalesOrderItemName + ","
			End If
		Next
		If ItemNames <> "" Then
			MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, "There are " + ItemNames.ToString.TrimEnd(",") + "An Order already exists for this Part or its Alternate Part. Do you still want to create another Order ?", MsgBoxStyle.YesNo, "Confirmation")
		Else
			Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")
		End If
		'--------
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Session("SalesOrderItems") = "True"
		Session("mSalesOrderItemsForPurchaseOrder") = Nothing
		Response.Redirect(Request.QueryString("BackPage"))
	End Sub
	Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
		DataFieldBind()
		upnlSalesOrderList.Update()
		upnlSalesOrderItemList.Update()
	End Sub
	Private Sub dgSalesOrderList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSalesOrderList.Sorting
		mSalesOrderForPurchaseOrderList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mSalesOrderForPurchaseOrderList") = mSalesOrderForPurchaseOrderList
		dgSalesOrderList.DataSource = mSalesOrderForPurchaseOrderList
		dgSalesOrderList.DataBind()
		upnlSalesOrderList.Update()
	End Sub
	Private Sub dgSalesOrderList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSalesOrderList.PageIndexChanging
		dgSalesOrderList.PageIndex = e.NewPageIndex
		dgSalesOrderList.DataSource = mSalesOrderForPurchaseOrderList
		Session("mSalesOrderForPurchaseOrderList") = mSalesOrderForPurchaseOrderList
		dgSalesOrderList.DataBind()
		upnlSalesOrderList.Update()
	End Sub
	Private Sub dgSalesOrderItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSalesOrderItemList.PageIndexChanging
		SetMultipleObject()
		dgSalesOrderItemList.PageIndex = e.NewPageIndex
		dgSalesOrderItemList.DataSource = mSalesOrderItemsForPurchaseOrder
		Session("mSalesOrderItemsForPurchaseOrder") = mSalesOrderItemsForPurchaseOrder
		dgSalesOrderItemList.DataBind()
		upnlSalesOrderItemList.Update()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
#End Region

End Class