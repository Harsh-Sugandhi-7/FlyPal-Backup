Public Class wfPendingOrdersForPaymentAdvice_Ajax
	Inherits System.Web.UI.Page

#Region "Variables Declaration"
	Dim mPendingOrdersforPaymentAdvice As PendingOrdersForPaymentAdvice
	Dim mPaymentAdvice As PaymentAdvice
	Dim EventLogID As Guid
	Public mDistinctTextListForOrder As DistinctTextListForOrder
#End Region

#Region "Methods"
	Private Sub GetSession()
		mPaymentAdvice = Session("mPaymentAdvice")
		mPendingOrdersforPaymentAdvice = Session("mPendingOrdersforPaymentAdvice")
	End Sub
	Private Sub DataFieldBind(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal Amend As String = "")
		mPendingOrdersforPaymentAdvice = PendingOrdersForPaymentAdvice.GetPaymentAdviceList(mPaymentAdvice.PaymentAdviceDate.ToString, _
																							mPaymentAdvice.VendorID.ToString, _
																							mPaymentAdvice.CurrencyID.ToString, _
																							mPaymentAdvice.ConversionFactor, Text, No, Amend)
		Session("mPendingOrdersforPaymentAdvice") = mPendingOrdersforPaymentAdvice
		dgPendingOrdersForPaymentAdvice.DataSource = mPendingOrdersforPaymentAdvice

		If mPendingOrdersforPaymentAdvice IsNot Nothing And mPendingOrdersforPaymentAdvice.Count <> 0 Then
			'dgPendingOrdersForPaymentAdvice.Columns(4).HeaderText = "Total Order Value (" + mPendingOrdersforPaymentAdvice(0).CurrencySymbol + ")" 'Commented by Vikrant On 28-Feb-2019 For BA28022019
			dgPendingOrdersForPaymentAdvice.Columns(5).HeaderText = "Total Order Value (" + mPendingOrdersforPaymentAdvice(0).BaseCurrencySymbol + ")"
			'Added By Vikrant On 16-Jan-2019 For ALL16012019
			'dgPendingOrdersForPaymentAdvice.Columns(6).HeaderText = "Pending Amount (" + mPendingOrdersforPaymentAdvice(0).CurrencySymbol + ")" 'Commented by Vikrant On 28-Feb-2019 For BA28022019
			dgPendingOrdersForPaymentAdvice.Columns(7).HeaderText = "Pending Amount (" + mPendingOrdersforPaymentAdvice(0).BaseCurrencySymbol + ")"
			'End
		End If

		DataBind()
		upnlPendingOrdersForPaymentAdvice.Update()
	End Sub
	Public Sub GridBind()
		dgPendingOrdersForPaymentAdvice.DataBind()
		upnlPendingOrdersForPaymentAdvice.Update()
	End Sub
	Private Sub SetObject(mId As Guid)
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderID = mPendingOrdersforPaymentAdvice.Item(mId).ID
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderDate = mPendingOrdersforPaymentAdvice.Item(mId).OrderDateFormatted
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderNo = mPendingOrdersforPaymentAdvice.Item(mId).No
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderText = mPendingOrdersforPaymentAdvice.Item(mId).Text
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.VendorName = mPendingOrdersforPaymentAdvice.Item(mId).VendorName
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.CurrencyName = mPendingOrdersforPaymentAdvice.Item(mId).CurrencyName
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.ConversionFactor = mPendingOrdersforPaymentAdvice.Item(mId).ConversionFactor
		'mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderAmount = mPendingOrdersforPaymentAdvice.Item(mId).TotalAmount
		'mPaymentAdvice.PaymentAdviceItems.CurrentItem.COrderAmount = mPendingOrdersforPaymentAdvice.Item(mId).CTotalAmount
		'Commented & Added by Vikrant On 28-Feb-2019 For BA28022019
		'mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderAmount = (mPendingOrdersforPaymentAdvice.Item(mId).TotalAmount - mPendingOrdersforPaymentAdvice.Item(mId).TotalPaymentAdviceAmount)
		'mPaymentAdvice.PaymentAdviceItems.CurrentItem.COrderAmount = (mPendingOrdersforPaymentAdvice.Item(mId).CTotalAmount - mPendingOrdersforPaymentAdvice.Item(mId).TotalPaymentAdviceCAmount)
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderAmount = (mPendingOrdersforPaymentAdvice.Item(mId).TotalAmount - mPendingOrdersforPaymentAdvice.Item(mId).TotalPaymentAdviceAmount)
		mPaymentAdvice.PaymentAdviceItems.CurrentItem.COrderAmount = (mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderAmount) / mPaymentAdvice.ConversionFactor
		'End
		Session("mPaymentAdvice") = mPaymentAdvice
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "RemoveFromPendingList" Then
						Try
							Dim Index As Integer = Session("Index")
							Session.Remove("Index")
							PendingOrdersForPaymentAdvice.RemoveFromPendingList(mPendingOrdersforPaymentAdvice(Index).ID)
							MarkLog(Util.Action.Remove, "PaymentAdvice", "Order " & mPendingOrdersforPaymentAdvice(Index).OrderTextNo & ", dated " & mPendingOrdersforPaymentAdvice(Index).OrderDateFormatted.ToString & " removed from pending list Successfully.", Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
							DataFieldBind()
						Catch ex As SqlException
							MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
							Exit Sub
						End Try
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "RemoveFromPendingList" Then
						Session.Remove("Index")
					End If
				Case MsgBoxResult.Ok

			End Select
		End If
	End Sub
	Private Sub ComboboxBinding()
		mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
		cmbOrderText.DataSource = mDistinctTextListForOrder
		DataBind()
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
#End Region

#Region "Events"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		addAttributes()
		EventLogID = CType(Session("EventLogID"), Guid)
		GetSession()
		If Not IsPostBack Then
			ComboboxBinding()
			DataFieldBind()
		End If
	End Sub

	Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub

	Private Sub dgPendingOrdersForPaymentAdvice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingOrdersForPaymentAdvice.RowCommand
		Select Case e.CommandName
			Case "Select"
				Dim mId As New Guid
				Dim Index As Integer = CInt(e.CommandArgument) + dgPendingOrdersForPaymentAdvice.PageIndex * dgPendingOrdersForPaymentAdvice.PageSize
				If Not mPaymentAdvice.PaymentAdviceItems.Contains(mPendingOrdersforPaymentAdvice(Index).ID) Then
					mPaymentAdvice.PaymentAdviceItems.Add(mPaymentAdvice.ID)
					mId = mPendingOrdersforPaymentAdvice(Index).ID

					SetObject(mId)
					Session("mID") = mId
					Session("mPendingOrdersforPaymentAdvice") = mPendingOrdersforPaymentAdvice
					Session("mPaymentAdvice") = mPaymentAdvice

					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenOrdersForPaymentAdviceWindow", "OpenOrdersForPaymentAdviceWindow();", True)
				Else
					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			Case "RemoveFromPendingList"
				Dim Index As Integer = CInt(e.CommandArgument) + dgPendingOrdersForPaymentAdvice.PageIndex * dgPendingOrdersForPaymentAdvice.PageSize
				Session("Index") = Index
				MSGBoxCtrl.show("Alert!", "Selected Order will get removed from pending list.", "Do you wat to continue?", MsgBoxStyle.YesNo, "RemoveFromPendingList")
		End Select
	End Sub
	Private Sub dgPendingOrdersForPaymentAdvice_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingOrdersForPaymentAdvice.PageIndexChanging
		dgPendingOrdersForPaymentAdvice.PageIndex = e.NewPageIndex
		dgPendingOrdersForPaymentAdvice.DataSource = mPendingOrdersforPaymentAdvice
		Session("mPendingOrdersforPaymentAdvice") = mPendingOrdersforPaymentAdvice
		GridBind()
	End Sub
	'Added By Vikrant On 16-Jan-2019 For ALL16012019
	Private Sub btnClearPendingList_Click(sender As Object, e As System.EventArgs) Handles btnClearPendingList.Click
		If btnClearPendingList.Text = "Remove Order(s) from Pending List" Then
			dgPendingOrdersForPaymentAdvice.Columns(0).HeaderStyle.CssClass = ""
			dgPendingOrdersForPaymentAdvice.Columns(0).ItemStyle.CssClass = ""
			btnClearPendingList.Text = "Save"
			btnClearPendingList.CssClass = "clsButton_Ajax"
		Else
			Dim ShowMsgBox As Boolean = True
			Dim chkBox As CheckBox
			For i As Integer = 0 To dgPendingOrdersForPaymentAdvice.Rows.Count - 1
				chkBox = CType(dgPendingOrdersForPaymentAdvice.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
				If chkBox.Checked Then
					ShowMsgBox = False
					PendingOrdersForPaymentAdvice.RemoveFromPendingList(mPendingOrdersforPaymentAdvice(i).ID)
					MarkLog(Util.Action.Remove, "PaymentAdvice", "Order " & mPendingOrdersforPaymentAdvice(i).OrderTextNo & ", dated " & mPendingOrdersforPaymentAdvice(i).OrderDateFormatted.ToString & " removed from pending list Successfully.", Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)

				End If
			Next
			If ShowMsgBox Then
				MSGBoxCtrl.show("Alert!", "Please Select At least One Record to remove from the list", "", MsgBoxStyle.OkOnly, "MSG")
				Exit Sub
			End If
			DataFieldBind()
			dgPendingOrdersForPaymentAdvice.Columns(0).HeaderStyle.CssClass = "hideGridColumn"
			dgPendingOrdersForPaymentAdvice.Columns(0).ItemStyle.CssClass = "hideGridColumn"
			btnClearPendingList.Text = "Remove Order(s) from Pending List"
			btnClearPendingList.CssClass = "clsButton_Ajax"
		End If
		upnlPendingOrdersForPaymentAdvice.Update()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	'End
	Private Sub cmbOrderText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrderText.SelectedIndexChanged
		txtNo.Text = ""
		txtAmend.Text = ""
	End Sub
	Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click
		DataFieldBind(IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), Val(txtNo.Text), txtAmend.Text.Trim)
	End Sub
#End Region

End Class