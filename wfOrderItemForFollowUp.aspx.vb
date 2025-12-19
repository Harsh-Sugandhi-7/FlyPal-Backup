Imports System.Collections.Generic
Imports System.Web.Services

'Created By Utkarsh ON 19-Jun-2013 FOR ALL18062013-1
Public Class wfOrderItemForFollowUp
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mOrderItemFollowUps As OrderItemFollowUps
	Public mOtherOrderItemFollowUps As OrderItemFollowUps
	Dim mOrderItemListForFollowUp As OrderItemListForFollowUp
	Public mOrderItemFollowUp As OrderItemFollowUp
	Dim EventLogID As Guid
	Dim mEventLogDetail As String
	Protected OrderItemID As Guid
	Protected OrderID As Guid
	Private Flag As Int16
	Dim eventlogdetails As List(Of String) = New List(Of String)
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mOrderItemFollowUps = CType(Session("mOrderItemFollowUps"), OrderItemFollowUps)
		mOrderItemListForFollowUp = Session("mOrderItemListForFollowUp")
		OrderItemID = Session("OrderItemID")
		OrderID = Session("OrderID")
		eventlogdetails = IIf(Session("eventlogdetails") Is Nothing, New List(Of String), Session("eventlogdetails"))
	End Sub
	Private Sub RemoveSessions()
		Session.Remove("mOrderItemFollowUps")
		Session.Remove("OrderItemID")
		Session.Remove("OrderItemFollowUpsEdit")
		Session.Remove("OrderDate")
		Session.Remove("OrderTextNo")
		Session.Remove("SupplierName")
		Session.Remove("SrNo")
		Session.Remove("PartNo")
		Session.Remove("OrderID")
		Session.Remove("mOrderItemListForFollowUp")
		Session.Remove("eventlogdetails")
	End Sub
	Private Sub SetPage()
		lblTitle.Text = "Order Follow Up [" & Session("PartNo").ToString & "]"
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType, "focusscript", str)
	End Sub
	Private Sub SetObject(Optional ByVal SetSrNo As Boolean = False, Optional SrNo As String = "")
		mOrderItemFollowUps.CurrentItem.OrderDate = txtOrderDate.Text.Trim
		mOrderItemFollowUps.CurrentItem.FollowUpDate = txtFollowUpDate.Text
		mOrderItemFollowUps.CurrentItem.AWBNo = txtAWBNo.Text.Trim
		mOrderItemFollowUps.CurrentItem.TD = txtTD.Text.Trim
		mOrderItemFollowUps.CurrentItem.ProformaNo = txtProformaNo.Text.Trim
		mOrderItemFollowUps.CurrentItem.ShipmentStatus = txtShipmentStatus.Text.Trim
		mOrderItemFollowUps.CurrentItem.ReturnInDays = Val(txtReturnInDays.Text.Trim)
		mOrderItemFollowUps.CurrentItem.FollowUpRemarks = txtRemark.Text.Trim
		If SetSrNo Then
			mOrderItemFollowUps.CurrentItem.CreateFollowUpText = txtOrderNo.Text.Trim & "-" & SrNo
		Else
			If mOrderItemFollowUps.Count >= 2 Then
				mOrderItemFollowUps.CurrentItem.CreateFollowUpText(mOrderItemFollowUps(mOrderItemFollowUps.Count - 2).FollowUpNo + 1) = txtOrderNo.Text.Trim & "-" & Session("SrNo").ToString
			Else
				mOrderItemFollowUps.CurrentItem.CreateFollowUpText = txtOrderNo.Text.Trim & "-" & SrNo
			End If


			'If mOrderItemFollowUps.CurrentItem.IsNew = True Then
			'    If mOrderItemFollowUps(mOrderItemFollowUps.Count - 1).FollowUpNo = mOrderItemFollowUps.CurrentItem.FollowUpNo Then
			'        mOrderItemFollowUps.CurrentItem.FollowUpNo = mOrderItemFollowUps.CurrentItem.FollowUpNo + 1
			'        'mOrderItemFollowUps.CurrentItem.FollowUpTextNo = mOrderItemFollowUp.FollowUpTextNo
			'    End If
			'End If

		End If
	End Sub
	Private Sub DeleteRecord(ByVal Index As Int32)
		'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.RemoveItem, SIMsgBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
		'msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?BackPage=" & Request.QueryString("BackPage")
		Session("sender") = "Delete"
		'msg1.Show()
		mOrderItemFollowUps.CurrentIndex = Index
		Session("mOrderItemFollowUps") = mOrderItemFollowUps
		Session("OrderItemFollowUpsEdit") = False
	End Sub
	Private Sub MessageBoxResult()
		'Dim Result1 As MsgBoxResult
		'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
		'    Result1 = -1
		'Else
		'    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		'End If

		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							Session("Sender") = ""
							Dim mOrderItemFollowUps As OrderItemFollowUps
							mOrderItemFollowUps = CType(Session("mOrderItemFollowUps"), OrderItemFollowUps)
							mOrderItemFollowUps.Remove(mOrderItemFollowUps.CurrentItem)
							mOrderItemFollowUps.CurrentIndex = mOrderItemFollowUps.Count - 1
							Session("mOrderItemFollowUps") = mOrderItemFollowUps
							Response.Redirect("wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
						Catch ex As SqlException
							If ex.Number = 8145 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							ElseIf ex.Number = 2627 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							ElseIf ex.Number = 547 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							End If
						End Try
					ElseIf MSGBoxCtrl.Sender = "Close" Then   '' Close confirmation
						Session("sender") = ""
						If Session("IsValid") Then
							Session.Remove("IsValid")
							DataFieldBind()
							Save()
						Else
							Session.Remove("IsValid")
							Response.Redirect("wfOrderItemForFollowUp.aspx?BackPage=" & Request.QueryString("BackPage"))
						End If
					ElseIf MSGBoxCtrl.Sender = "Status" Then
						Session("sender") = ""
						If Session("IsValid") Then
							Session.Remove("IsValid")
							DataFieldBind()
							Save()
						Else
							Session.Remove("IsValid")
							Response.Redirect("wfOrderItemForFollowUp.aspx?BackPage=" & Request.QueryString("BackPage"))
						End If
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						RemoveSessions()
						Session("Sender") = ""
						Response.Redirect("Index.aspx")
					Else
						Session("Sender") = ""
						Response.Redirect("wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					End If
				Case MsgBoxResult.Ok
					Session("sender") = ""
					'DataFieldBind()
					Response.Redirect("wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			Response.Redirect("wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			DataFieldBind()
		End If
	End Sub
	Private Function Save() As Boolean
		Dim OrderItemFollowUpClone As OrderItemFollowUps
		OrderItemFollowUpClone = CType(mOrderItemFollowUps.Clone, OrderItemFollowUps)
		'setObject()
		If customvalidate1() Then
			Try
				'Dim count As Integer = 0

				'Dim check As IGrouping(Of Guid, OrderItemFollowUp) = From m As OrderItemFollowUp In mOrderItemFollowUps
				'            Where m.IsNew
				'            Group By key = m.OrderItemID Into results = Group


				For i As Integer = 0 To mOrderItemFollowUps.Count - 1
					If mOrderItemFollowUps(i).IsNew Then
						If mOrderItemFollowUps(i).OrderItemID.Equals(OrderItemID) Then
							mEventLogDetail = "Order Date : " & txtOrderDate.Text & " ,Order No. " & txtOrderNo.Text & "  ,Part No. " & Session("PartNo").ToString & "  ,FO No. " & mOrderItemFollowUps(i).FollowUpTextNo
						Else
							mEventLogDetail = "Order Date : " & txtOrderDate.Text & " ,Order No. " & txtOrderNo.Text & "  ,Part No. " & mOrderItemListForFollowUp(mOrderItemFollowUps(i).OrderItemID).PartName & "  ,FO No. " & mOrderItemFollowUps(i).FollowUpTextNo
						End If
						eventlogdetails.Add(mEventLogDetail)
					End If
				Next

				Session("eventlogdetails") = eventlogdetails

				mOrderItemFollowUps = CType(mOrderItemFollowUps.Save(), OrderItemFollowUps)
				Session("mOrderItemFollowUps") = mOrderItemFollowUps
				Return True
			Catch ex As SqlException
				Session("OrderItemFollowUpClone") = OrderItemFollowUpClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
					msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=Index.aspx"
					msg1.Show()
				ElseIf ex.Number = 8145 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
					msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=Index.aspx"
					msg1.Show()
				ElseIf ex.Number = 2627 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
					msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=Index.aspx"
					msg1.Show()
				ElseIf ex.Number = 547 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
					msg1.ReplacePage = "wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=Index.aspx"
					msg1.Show()
				End If
				Return False
			Finally
				OrderItemFollowUpClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function
	Private Function customvalidate1() As Boolean
		Dim str As String = ""
		For i As Integer = 0 To mOrderItemFollowUps.Count - 1
			If Not mOrderItemFollowUps(i).IsValid Then
				For j As Integer = 0 To mOrderItemFollowUps(i).GetBrokenRulesCollection.Count - 1
					str = str + "Sr. No. " & i + 1 & " : " & mOrderItemFollowUps(i).GetBrokenRulesCollection(j).Description + "<BR>"
				Next
			End If
		Next
		If str <> "" Then
			cvCommon.ErrorMessage = str
			cvCommon.IsValid = False
			Return False
		End If
		Return True
	End Function
	Private Sub setGridStatus()
		'For Each Item As DataGridItem In dgOrderItemFollowUp.Items
		'    If Not Item.Cells(1).Text.Equals(OrderItemID.ToString) Then
		'        Item.Cells(11).Enabled = False
		'        Item.Cells(12).Enabled = False
		'    End If
		'Next


	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		dgOrderItemFollowUp.DataSource = mOrderItemFollowUps
		dgOrderItemFollowUp.DataBind()
		If txtFollowUpDate.Text.Trim = "" Then
			txtFollowUpDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)

		End If
		If Session("OrderItemFollowUpsEdit") = True Then
			txtFollowUpDate.Text = mOrderItemFollowUps.CurrentItem.FollowUpDateFormatted
			txtText.Text = mOrderItemFollowUps.CurrentItem.FollowUpText
			txtNo.Text = mOrderItemFollowUps.CurrentItem.FollowUpNo
			txtAWBNo.Text = mOrderItemFollowUps.CurrentItem.AWBNo
			txtTD.Text = mOrderItemFollowUps.CurrentItem.TD
			txtProformaNo.Text = mOrderItemFollowUps.CurrentItem.ProformaNo
			txtShipmentStatus.Text = mOrderItemFollowUps.CurrentItem.ShipmentStatus
			txtReturnInDays.Text = mOrderItemFollowUps.CurrentItem.ReturnInDays
			txtRemark.Text = mOrderItemFollowUps.CurrentItem.FollowUpRemarks
		End If
		txtOrderDate.Text = Session("OrderDate")
		txtOrderNo.Text = Session("OrderTextNo")
		txtSupplier.Text = Session("SupplierName")
	End Sub
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		If Flag = 1 Then Exit Sub
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		Dim str As String = ""
		'Log Maintenance Activity
		If txtFollowUpDate.Text.Trim = "" Then
			str = str + "Follow Up Date should not be blank" & "<br/>"
			e.IsValid = False
		End If
		If txtFollowUpDate.Text.Trim <> "" AndAlso txtOrderDate.Text.Trim <> "" AndAlso (CDate(txtFollowUpDate.Text.Trim) < CDate(txtOrderDate.Text.Trim)) Then
			str = str + "Follow Up Date should be not less than Order Date" & "<br/>"
			e.IsValid = False
		End If
		If txtAWBNo.Text.Length > 50 Then
			'txtMainActivity.Text = txtMainActivity.Text.Substring(0, 996) + "..."
			str = str + "AWB No. is too long" & "<br/>"
			e.IsValid = False
		End If
		If txtProformaNo.Text.Length > 50 Then
			'txtMainActivity.Text = txtMainActivity.Text.Substring(0, 996) + "..."
			str = str + "Proforma No. is too long" & "<br/>"
			e.IsValid = False
		End If
		If txtTD.Text.Length > 50 Then
			'txtMainActivity.Text = txtMainActivity.Text.Substring(0, 996) + "..."
			str = str + "TD is too long" & "<br/>"
			e.IsValid = False
		End If
		If txtRemark.Text.Length > 500 Then
			'txtMainActivity.Text = txtMainActivity.Text.Substring(0, 996) + "..."
			str = str + "Remark is too long" & "<br/>"
			e.IsValid = False
		End If
		If txtShipmentStatus.Text.Length > 50 Then
			'txtMainActivity.Text = txtMainActivity.Text.Substring(0, 996) + "..."
			str = str + "Shipment Status is too long"
			e.IsValid = False
		End If

		If str <> "" Then
			custValidator.ErrorMessage = str
			e.IsValid = False
		End If
		Flag = 1
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack And Session("Sender") = "" Then
			DataFieldBind()
			setFocus(txtText)
		End If
		SetPage()
		MessageBoxResult()
		setGridStatus()
	End Sub
	'Private Sub dgOrderItemFollowUp_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgOrderItemFollowUp.ItemCommand
	'    Dim indx As Int32 = e.Item.ItemIndex + dgOrderItemFollowUp.CurrentPageIndex * dgOrderItemFollowUp.PageSize
	'    Select Case e.CommandName
	'        Case "Edit"
	'            mOrderItemFollowUps.CurrentIndex = indx
	'            Session("mOrderItemFollowUps") = mOrderItemFollowUps
	'            Session("OrderItemFollowUpsEdit") = True
	'            DataFieldBind()
	'        Case "Remove"
	'            DeleteRecord(indx)
	'    End Select
	'End Sub

#End Region
	Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		'If (Not User.IsInRole("OrderFollowUpNew") And mOrderItemFollowUps.CurrentItem.IsNew) Or (Not User.IsInRole("OrderFollowUpEdit") And Not mOrderItemFollowUps.CurrentItem.IsNew) Then
		'    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
		'    msg.ReplacePage = "wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=Index.aspx"
		'    Session("sender") = "Authorization"
		'    msg.Show()
		'    Exit Sub
		'End If
		If Not IsValid Then Exit Sub
		If Session("OrderItemFollowUpsEdit") = False Then
			mOrderItemFollowUps.Add(OrderItemID)
			mOrderItemFollowUps.CurrentIndex = mOrderItemFollowUps.Count - 1
			SetObject()
			If chkAddOrderItem.Checked AndAlso Not (mOrderItemListForFollowUp Is Nothing) Then
				'If tblFOList.Rows.Count > 1 Then
				'    Dim chk As System.Web.UI.HtmlControls.HtmlInputCheckBox = CType(tblFOList.Rows(0).Cells(0).FindControl("ChkSelectItem0"), System.Web.UI.HtmlControls.HtmlInputCheckBox)
				'End If
				For i As Integer = 0 To dgOrderList.Items.Count - 1
					Dim checbox As CheckBox = dgOrderList.Items(i).FindControl("chkSelect")
					If checbox.Checked Then
						mOrderItemFollowUps.Add(mOrderItemListForFollowUp(i).OrderItemID, mOrderItemListForFollowUp(i).LastFOSrNo)
						mOrderItemFollowUps.CurrentIndex = mOrderItemFollowUps.Count - 1
						SetObject(True, mOrderItemListForFollowUp(i).SrNo)
					End If
				Next

			End If
			Session("mOrderItemFollowUps") = mOrderItemFollowUps
			Response.Redirect("wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		Else
			SetObject()
			Session("mOrderItemFollowUps") = mOrderItemFollowUps
			Session("OrderItemFollowUpsEdit") = False
			Response.Redirect("wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		End If
		setGridStatus()
	End Sub

	Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If (Not User.IsInRole("OrderFollowUpNew")) Or (Not User.IsInRole("OrderFollowUpEdit")) Then
			SetObject()
			mEventLogDetail = "Order Date : " & txtOrderDate.Text & " ,Order No. " & txtOrderNo.Text & "  ,Part No. " & Session("PartNo").ToString
			MarkLog(Util.Action.Save, "OrderFollowUp", User.Identity.Name & " is not Authorized User to save " & mEventLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			msg.ReplacePage = "wfOrderItemForFollowUp.aspx?MsgResult=0&BackPage=Index.aspx"
			Session("sender") = "Authorization"
			msg.Show()
			Exit Sub
		End If
		If IsValid Then
			If Save() = True Then
				If eventlogdetails IsNot Nothing AndAlso eventlogdetails.Count > 0 Then
					For i As Integer = 0 To eventlogdetails.Count - 1
						MarkLog(Util.Action.Save, "OrderFollowUp", eventlogdetails(i), Util.ErrorType.HandledError, mOrderItemFollowUps.CurrentItem.ID, EventLogID)
					Next
					Response.Redirect("wfOrderItemForFollowUp.aspx?BackPage=Index.aspx")
				End If
			End If
		End If
		setGridStatus()
	End Sub

	Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
		MarkLog(Util.Action.Close, "OrderFollowUp", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		RemoveSessions()
		Response.Redirect("Index.aspx")
	End Sub
	Protected Sub chkAddOrderItem_CheckedChanged(sender As Object, e As EventArgs) Handles chkAddOrderItem.CheckedChanged
		If chkAddOrderItem.Checked Then
			mOrderItemListForFollowUp = OrderItemListForFollowUp.GetOrderItemListForFollowUp(AsOnDate:=Now.Date.ToString, OrderID:=OrderID.ToString)
			mOrderItemListForFollowUp.Remove(OrderItemID)
			dgOrderList.DataSource = mOrderItemListForFollowUp
			Session("mOrderItemListForFollowUp") = mOrderItemListForFollowUp
			dgOrderList.DataBind()
			dgOrderList.Visible = True
		Else
			mOrderItemListForFollowUp = Nothing
			Session("mOrderItemListForFollowUp") = mOrderItemListForFollowUp
			dgOrderList.DataSource = mOrderItemListForFollowUp
			dgOrderList.Visible = False
		End If

	End Sub

#Region "Service Methods"
	<WebMethod()>
	Public Shared Function GetOrderItemListForFO(ByVal OrderID As String, ByVal OrderItemID As String) As String
		Dim mOrderItemListForFollowUp As OrderItemListForFollowUp
		mOrderItemListForFollowUp = OrderItemListForFollowUp.GetOrderItemListForFollowUp(OrderID:=OrderID)
		Dim i As Integer = 0
		Dim Table As String = ""
		For Each item As OrderItemListForFollowUp.OrderItemListForFollowUpInfo In mOrderItemListForFollowUp
			Table = Table & "<tr Class=clsdgItem>"
			Table = Table & "<TD width=0>" & item.OrderID.ToString & "</TD>"
			Table = Table & "<TD width=0>" & item.OrderItemID.ToString & "</TD>"
			Table = Table & "<TD><INPUT id=ChkSelectItem" & i & " type=checkbox /> </TD>"
			Table = Table & "<TD >" & item.OrderDate & "</TD>"
			Table = Table & "<TD >" & item.OrderTextNo & "</TD>"
			Table = Table & "<TD >" & item.IntOrderNo & "</TD>"
			Table = Table & "<TD " & item.OrderType & "</TD>"
			Table = Table & "<TD>" & item.SupplierName & "</TD>"
			Table = Table & "<TD>" & item.PartName & "</TD>"
			Table = Table & "<TD >" & item.PartDescription & "</TD>"
			Table = Table & "<TD >" & item.SerialNo & "</TD>"
			Table = Table & "<TD  align=right>" & item.DeliveryInDays & "</TD>"
			Table = Table & "<TD  align=right>" & item.PriorityName & "</TD>"
			Table = Table & "<TD  align=right>" & item.RemainingDays & "</TD>"
			Table = Table & "<TD  align=right>" & item.OrdQty & "</TD>"
			Table = Table & "<TD  align=right>" & item.RecQty & "</TD>"
			Table = Table & "<TD align=right>" & item.BalQty & "</TD>"
			Table = Table & "<TD align=right>" & item.CAmount & "</TD>"
			Table = Table & "<TD >" & item.CurrencyName & "</TD>"
			Table = Table & "<TD align=right>" & item.Amount & "</TD>"
			Table = Table & "</tr>"
			i = i + 1
		Next
		If mOrderItemListForFollowUp.Count > 0 Then
			Return Table
		Else
			Return "No record found.."
		End If
	End Function

	'Private Sub dgOrderItemFollowUp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgOrderItemFollowUp.RowCommand

	'    'Dim indx As Int32 = e.Item.ItemIndex + dgOrderItemFollowUp.CurrentPageIndex * dgOrderItemFollowUp.PageSize
	'    'Dim indx As Integer = 0
	'    'Dim mId As Guid = dgOrderItemFollowUp(indx).ID
	'    'Select Case e.CommandName
	'    '    Case "Edit"    
	'    '        indx = CInt(e.CommandArgument) '+ dgOrderItemFollowUp.PageSize * dgOrderItemFollowUp.PageIndex
	'    '        mOrderItemFollowUps.CurrentIndex = indx
	'    '        Session("mOrderItemFollowUps") = mOrderItemFollowUps
	'    '        Session("OrderItemFollowUpsEdit") = True
	'    '        DataFieldBind()
	'    '    Case "Remove"
	'    '        indx = CInt(e.CommandArgument) '+ dgOrderItemFollowUp.PageSize * dgOrderItemFollowUp.PageIndex
	'    '        DeleteRecord(indx)
	'    'End Select

	'    Dim Idx As Int32
	'    Select Case e.CommandName
	'        Case "Edit"

	'            Idx = CInt(e.CommandArgument) + dgOrderItemFollowUp.PageIndex * dgOrderItemFollowUp.PageSize

	'            mOrderItemFollowUps.CurrentIndex = Idx
	'            Session("mOrderItemFollowUps") = mOrderItemFollowUps
	'            Session("OrderItemFollowUpsEdit") = True
	'            DataFieldBind()
	'        Case "Remove"
	'            Idx = CInt(e.CommandArgument) + dgOrderItemFollowUp.PageIndex * dgOrderItemFollowUp.PageSize
	'            DeleteRecord(Idx)
	'    End Select

	'    'mnWOJob.WOJobSpares.CurrentIndex = index
	'    'txtSpareDesc.Text = mnWOJob.WOJobSpares.Item(index).Description
	'    'txtReqQty.Text = mnWOJob.WOJobSpares.Item(index).RequiredQty
	'    'cmbItemList.SelectedValue = mnWOJob.WOJobSpares.Item(index).ItemID.ToString
	'    'chkIsForBilling.Checked = mnWOJob.WOJobSpares.Item(index).IsForBilling
	'    'txtRemark.Text = mnWOJob.WOJobSpares.Item(index).Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
	'    'txtEffRate.Text = mnWOJob.WOJobSpares.Item(index).EffRate
	'    'txtEstimatedCost.Text = mnWOJob.WOJobSpares.Item(index).EstimatedCost
	'    'setFocus(cmbItemList)
	'    'upnlDesc.Update()
	'    'upnlPart.Update()
	'    'Session("mnWOJob") = mnWOJob

	'End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	Protected Sub dgOrderItemFollowUp_RowCommand1(sender As Object, e As GridViewCommandEventArgs)
		Dim Idx As Int32
		Select Case e.CommandName
			Case "Edit"

				Idx = CInt(e.CommandArgument) + dgOrderItemFollowUp.PageIndex * dgOrderItemFollowUp.PageSize

				mOrderItemFollowUps.CurrentIndex = Idx
				Session("mOrderItemFollowUps") = mOrderItemFollowUps
				Session("OrderItemFollowUpsEdit") = True
				DataFieldBind()
			Case "Remove"
				Idx = CInt(e.CommandArgument) + dgOrderItemFollowUp.PageIndex * dgOrderItemFollowUp.PageSize
				DeleteRecord(Idx)
		End Select
	End Sub

	Private Sub dgOrderItemFollowUp_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgOrderItemFollowUp.RowEditing
		e.Cancel = True
	End Sub

#End Region

End Class