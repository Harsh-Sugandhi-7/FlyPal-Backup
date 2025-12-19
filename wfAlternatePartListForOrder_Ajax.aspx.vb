'Added By Vikrant On 12-Sep-2018 For ALL12092018
Public Class wfAlternatePartListForOrder_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mItem As Item
	Public mOrder As Order
	Public OrderType As Integer
	Dim mUnitListForConverter As UnitListForConverter
	Dim mItemId As Guid = Guid.Empty
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mItem = Session("mItem")
		mOrder = CType(Session("mOrder"), Order)
		OrderType = Session("OrderType")
	End Sub
	Private Sub SetPage()
		mItem = Session("mItem")
		lblResult.Text = "List of alternate parts For : " + mItem.Name
		If Not mItem.IsNew Then
			lblTitle.Text = "Alternate Part For [" + mItem.Name + "]"
		End If
	End Sub
	Private Sub SetOrderObject(ByVal Index As Integer)
		mOrder.OrderItems.CurrentItem.ItemID = mItem.AlternatePartNos(Index).AlternatePartID
		mOrder.OrderItems.CurrentItem.AlternateItemID = mItem.AlternatePartNos(Index).AlternatePartID

		Dim mCRateOfLastOrderedItem As CRateOfLastOrderedItem
		mCRateOfLastOrderedItem = CRateOfLastOrderedItem.GetCRateOfLastOrderedItem(mOrder.TransTypeID, mItem.AlternatePartNos(Index).AlternatePartID.ToString)

		If mCRateOfLastOrderedItem(0).ItemCRate <> 0 Then
			mOrder.OrderItems.CurrentItem.CRate = mCRateOfLastOrderedItem(0).ItemCRate
		Else
			mOrder.OrderItems.CurrentItem.CRate = mOrder.OrderItems.CurrentItem.ApproximateRate
		End If

		Session("mOrder") = mOrder
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					'Added By Vikrant On 07-Oct-2014 For ALL07102014
					If MSGBoxCtrl.Sender = "SaveItemMaster" Then
						Try
							Session("Sender") = ""
							Dim mItem As Item
							Dim index As Integer = Session("Index")

							mItem = Item.NewItem(mItem.AlternatePartNos(index).PartName, mItem.AlternatePartNos(index).PartDescription, mItem.AlternatePartNos(index).IPCReference)
							Session("mItem") = mItem

							mItemId = mItem.ID
							Session("mItemId") = mItemId
							'setObject(mItemId, mRequisitionItemsNew(index).ID, mRequisitionItemsNew(0).OrderBalQty, mRequisitionItemsNew(0).RequisitionNo)
							Session("mOrder") = mOrder
							Session("PartInfo") = "True"
							Dim URL As Stack = New Stack
							'URL.Push("wfRequisitionPartListForPurchaseOrder_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
							URL.Push(Request.Url.ToString.Substring(Request.Url.ToString.LastIndexOf("/") + 1))
							Session("URL") = URL
							Session("RequisitionItemID") = mOrder.OrderItems.CurrentItem.RequisitionItemOrderItems.CurrentItem.ReqItemID
							Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfAlternatePartListForOrder_Ajax.aspx")
						Catch ex As Exception
							Throw ex.GetBaseException
						End Try
					End If
					'End
				Case MsgBoxResult.No
					'Added By Vikrant On 07-Oct-2014 For ALL07102014
					If MSGBoxCtrl.Sender = "SaveItemMaster" Then
						Session("sender") = ""
					End If
					'End
				Case MsgBoxResult.Ok
			End Select
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		dgAlternatePartList.DataSource = mItem.AlternatePartNos
		Session("Item") = mItem
		DataBind()
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		Session("UnitName") = mOrder.OrderItems.CurrentItem.UnitName
		If Not IsPostBack Then
			DataFieldBind()
			SetPage()
			'------------Added BY Vikrant on 28-03-2012--------------------------------
			If User.IsInRole("AlternatePartInReceiptView") = True Then
				btnCreatealternatepart.Visible = True
			End If
			'--------------------------------------------------------------------------
		End If

	End Sub
	Private Sub dgAlternatePartList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAlternatePartList.RowCommand
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index As Int16 = CInt(e.CommandArgument) + dgAlternatePartList.PageIndex * dgAlternatePartList.PageSize
				Session("Index") = Index
				Dim mtmpItem As Item = Item.GetItem(mItem.AlternatePartNos(Index).AlternatePartID)
				If mOrder.OrderItems.Contains(ItemID:=mtmpItem.ID) Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "Order Item", MsgBoxStyle.OkOnly, "")
					mOrder.CancelEdit()
					Exit Select
				End If
				Dim UnitName As String = mOrder.OrderItems.CurrentItem.UnitName
				Session("UnitName") = UnitName
				Dim ItemID As Guid = Guid.Empty
				Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(mItem.AlternatePartNos(Index).PartName)
				If mFetchItemByName.Count > 0 Then
					ItemID = mFetchItemByName(0).ID
				End If
				If ItemID.Equals(Guid.Empty) Then
					MSGBoxCtrl.Show("Alert", "Part not added in Part Master", "Do you want to add it in Part Master", MsgBoxStyle.YesNo, "SaveItemMaster")
					Exit Sub
				End If

				dgAlternatePartList.DataSource = mItem.AlternatePartNos
				dgAlternatePartList.DataBind()
				upnlAlternatePartList.Update()
				mUnitListForConverter = UnitListForConverter.GetUnitListForConverter()

				If Not mUnitListForConverter.Contains(BaseUnitID:=mUnitListForConverter(UnitName, Guid.Empty).PrimaryUnitID, ConvertUnitID:=mtmpItem.UnitID) Then
					'End If
					'If Not UnitID.Equals(mtmpItem.UnitID) Then
					MSGBoxCtrl.Show("Alert!", "The Unit of the alternate part doesnot match. Please select the alternate part with same unit", "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				SetOrderObject(Index)
				Response.Redirect("wfOrderItem_Ajax.aspx?BackPage=" & "wfPurchaseOrder_Ajax.aspx")
		End Select
	End Sub
	Private Sub dgAlternatePartList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAlternatePartList.PageIndexChanging
		dgAlternatePartList.PageIndex = e.NewPageIndex
		dgAlternatePartList.DataSource = mItem.AlternatePartNos
		dgAlternatePartList.DataBind()
		upnlAlternatePartList.Update()
	End Sub
	'------------Added BY Vikrant on 28-03-2012--------------------------------
	Private Sub btnCreatealternatepart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatealternatepart.Click
		Dim str As String
		' Session("mItem") = mItem
		Session("mAltItem") = mItem
		Dim AlternateType As Integer = 5 ''
		Session("AlternateType") = AlternateType ''
		str = "openledgersame('wfAlternatePart_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage1=wfAlternatePartListForOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx&AlternateType=5'" & "" & ");"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
	End Sub
	'--------------------------------------------------------------------------
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Session.Remove("mItem")
		Response.Redirect("wfOrderItem_Ajax.aspx?BackPage=" & "wfPurchaseOrder_Ajax.aspx")
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
#End Region

End Class