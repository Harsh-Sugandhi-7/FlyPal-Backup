'***********************************
'AJAX Conversion By Vikrant On 27-Jun-2014
'***********************************


Public Class wfEnquiryItem_Ajax
	Inherits Page

#Region "Enumaration"

	Private Enum Rights

		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		FindNow = 7

	End Enum

#End Region

#Region " Variable Description "

	Public mEnquiry As Enquiry
	Public mModelList As ModelList
	Public mPriorityList As PriorityList
	Public mItemTypeList As ItemTypeList

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		mEnquiry = Session("mEnquiry")
		mModelList = Session("mModelList")
		mPriorityList = Session("mPriorityList")
		mItemTypeList = Session("mItemTypeList")

	End Sub

	Private Sub SetSession()

		Session("mEnquiry") = mEnquiry
		Session("mModelList") = mModelList
		Session("mPriorityList") = mPriorityList
		Session("mItemTypeList") = mItemTypeList

	End Sub

	Private Sub RemoveSession()

		Session.Remove("mModelList")
		Session.Remove("mPriorityList")
		Session.Remove("mItemTypeList")

	End Sub

	Private Sub AddAttributes()

		Try

			txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
			txtReqDays.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReqDays').value,event)")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetPage()

		Try

			If Session("Edit") Then

				lblTitle.Text = $"Enquiry Item [ {mEnquiry.EnquiryItems.CurrentItem.ItemName} ]"
				txtPartNo.BackColor = Color.Silver

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function SetObject() As Boolean

		Try

			mEnquiry.EnquiryItems.CurrentItem.ItemTypeID = CInt(cmbPartTypeList.SelectedValue)
			mEnquiry.EnquiryItems.CurrentItem.SrNo = mEnquiry.EnquiryItems.CurrentIndex + 1
			mEnquiry.EnquiryItems.CurrentItem.Qty = Val(txtQty.Text)
			mEnquiry.EnquiryItems.CurrentItem.ModelID = New Guid(cmbApplicable.SelectedValue)
			mEnquiry.EnquiryItems.CurrentItem.ModelName = cmbApplicable.SelectedItem.Text
			mEnquiry.EnquiryItems.CurrentItem.Remark = Trim(txtRemark.Text)
			mEnquiry.EnquiryItems.CurrentItem.Note = Trim(txtNote.Text)
			mEnquiry.EnquiryItems.CurrentItem.PriorityID = CInt(cmbPriority.SelectedValue)
			mEnquiry.EnquiryItems.CurrentItem.ItemName = txtPartNo.Text
			mEnquiry.EnquiryItems.CurrentItem.ItemDescription = txtDescription.Text
			'Added Code By Girish , July,17,2007
			mEnquiry.EnquiryItems.CurrentItem.RequiredInDays = Val(txtReqDays.Text)
			mEnquiry.EnquiryItems.CurrentItem.IPCReference = txtIPCRefer.Text

			Dim TmpItem As Item = Item.GetItem(mEnquiry.EnquiryItems.CurrentItem.ItemID)

			If mEnquiry.EnquiryItems.Contains(mEnquiry.EnquiryItems.CurrentItem) Then

				mEnquiry.CancelEdit()
				MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate,
								MSGBox.Message_Text.Duplicate,
								"Enquiry Item",
								MsgBoxStyle.OkOnly,
								"")

				Exit Function

			ElseIf TmpItem.NotInUse = True Then

				If CDate(TmpItem.NotInUseDate) <= CDate(mEnquiry.Date) Then

					MSGBoxCtrl.Show("Save Alert!",
									"Part is not applicable since " + TmpItem.NotInUseDateFormatted + " <br><br> Select another Part from list & try again",
									"",
									MsgBoxStyle.OkOnly,
									"")
					Exit Function

				End If

			Else
				mEnquiry.ApplyEdit()
			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Sub ControlVisibility()

		Try

			'Added by Vikrant For New Requisition

			If AppSettings("NewRequisition") = "True" Then

				If (mEnquiry.EnquiryItems.CurrentItem IsNot Nothing AndAlso
					mEnquiry.EnquiryItems.CurrentItem.RequisitionItemEnquiryItems.Count > 0) Then 'Commented & Added For New Requisition

					cmbPriority.Enabled = False
				End If

			Else

				If (mEnquiry.EnquiryItems.CurrentItem IsNot Nothing AndAlso
					mEnquiry.EnquiryItems.CurrentItem.EnquiryItemRequisitionItems.Count > 0) Then

					cmbPriority.Enabled = False
				End If

			End If

			If ((Session("Edit") And (Not mEnquiry.EnquiryItems.CurrentItem.ItemID.Equals(Guid.Empty))) Or
				(Not mEnquiry.EnquiryItems.CurrentItem.ItemID.Equals(Guid.Empty))) Then

				txtPartNo.BackColor = Color.Gainsboro
				txtDescription.BackColor = Color.Gainsboro
				txtPartNo.ToolTip = "Part No."
				txtDescription.ToolTip = "Description"

			Else

				txtPartNo.BackColor = Color.White
				txtDescription.BackColor = Color.White
				txtPartNo.ToolTip = "Enter Part No."
				txtDescription.ToolTip = "Enter Description"

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		Try

			MsgBoxResult = MSGBoxCtrl.Result

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

					Case MsgBoxResult.No
						Session("Sender") = ""
					Case MsgBoxResult.Ok
						Session("sender") = ""
					Case MsgBoxResult.Ok And Session("sender") = "Authorization"
						Session("sender") = ""
				End Select

			ElseIf MsgBoxResult = -1 Then
				Session("sender") = ""
			ElseIf MsgBoxResult = 0 And Session("sender") = "Authorization" Then
				Session("sender") = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean

		Dim IsInRoleString As String = ""
		Try

			'Deciding IsInRole String to check Rights
			Select Case mEnquiry.TransTypeID
				Case Trans.Enquiry
					IsInRoleString = "Enquiry"
				Case Trans.RequestingForQuotation
					IsInRoleString = "RequestingForQuotation"
				Case Trans.OverHaulRepairEnquiry
					IsInRoleString = "PurchaseEnquiryRepairOverHaul"
				Case Trans.RentialLeaseEnquiry
					IsInRoleString = "PurchaseEnquiryRentalLease"
			End Select

			'Depending upon decided IsInRole String; checking Rights of the User
			Select Case CheckFor
				Case Rights.[New]
					Return User.IsInRole(IsInRoleString + "New")
				Case Rights.Edit
					Return User.IsInRole(IsInRoleString + "Edit")
				Case Rights.Save
					Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
				Case Rights.Delete
					Return User.IsInRole(IsInRoleString + "Delete")
				Case Rights.View
					Return User.IsInRole(IsInRoleString + "View")
				Case Rights.Print
					Return User.IsInRole(IsInRoleString + "Print")
				Case Rights.FindNow
					Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			mModelList = ModelList.GetModelList(ItemID:=mEnquiry.EnquiryItems.CurrentItem.ItemID,
												AddNone:=True)

			Session("mModelList") = mModelList
			cmbApplicable.DataSource = mModelList
			mItemTypeList = ItemTypeList.GetItemTypeList
			Session("mItemTypeList") = mItemTypeList
			cmbPartTypeList.DataSource = mItemTypeList
			mPriorityList = PriorityList.GetPriorityList(, , AddToppitem:="")
			Session("mPriorityList") = mPriorityList
			cmbPriority.DataSource = mPriorityList

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddPart()

		Try

			If AppSettings("NewRequisition") = "True" Then

				'Added by Vikrant For New Requisition
				Dim mRequisitionItemNew As RequisitionItemNew
				Dim mRequisitionItemsNew As RequisitionItemsNew = Session("mRequisitionItemsNew")

				If mRequisitionItemsNew Is Nothing Then Exit Sub

				For Each mRequisitionItemNew In mRequisitionItemsNew

					If mRequisitionItemNew.IsSelect Then

						With mEnquiry.EnquiryItems.CurrentItem

							.IPCReference = mRequisitionItemNew.IPCReference
							.PriorityID = mRequisitionItemNew.PriorityID

							If Not .RequisitionItemEnquiryItems.Contains(RequisitionItemID:=mRequisitionItemNew.ID) Then

								.RequisitionItemEnquiryItems.Add(EnquiryItemID:= .ID,
																 RequisitionItemID:=mRequisitionItemNew.ID,
																 Qty:=mRequisitionItemNew.EnquiryBalQty,
																 RequisitionNo:=mRequisitionItemNew.RequisitionNo) 'Commented & Added For New Requisition
							Else

								MSGBoxCtrl.Show(MSGBox.Message_Title.ValidationAlert,
												MSGBox.Message_Text.ValidationAlert,
												"Requisition item already taken for Enquiry",
												MsgBoxStyle.OkOnly,
												"Close")
								Exit Sub

							End If

						End With

					End If

				Next

			Else 'End

				Dim mRequisitionItem As RequisitionItem
				Dim mRequisitionItems As RequisitionItems = Session("mRequisitionItems")

				If mRequisitionItems Is Nothing Then Exit Sub

				For Each mRequisitionItem In mRequisitionItems

					If mRequisitionItem.IsSelect Then

						With mEnquiry.EnquiryItems.CurrentItem

							.IPCReference = mRequisitionItem.IPCReference
							.PriorityID = mRequisitionItem.PriorityID
							'Check is Requisition Part is present ?

							If Not .EnquiryItemRequisitionItems.Contains(RequisitionItemID:=mRequisitionItem.ID) Then

								.EnquiryItemRequisitionItems.Add(EnquiryItemID:= .ID,
																 RequisitionItemID:=mRequisitionItem.ID,
																 Qty:=mRequisitionItem.EnquiryBalQty,
																 RequisitionNo:=mRequisitionItem.RequisitionNo)
							Else

								MSGBoxCtrl.Show(MSGBox.Message_Title.ValidationAlert,
												MSGBox.Message_Text.ValidationAlert,
												"Requisition item already taken for Enquiry",
												MsgBoxStyle.OkOnly,
												"Close")
								Exit Sub

							End If

						End With

					End If

				Next

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			getSession()
			addAttributes()

			If CType(Session("AddPart"), String) = "True" Then 'chk whether code gets executed
				'Add selected part(s) to Enquiry Items
				AddPart()

				Session("AddPart") = "False"
				Session("AddRequisitionParts") = "False"

			Else
				Session("AddPart") = "False"
				Session("AddRequisitionParts") = "False"
			End If

			If Not IsPostBack Then

				If txtPartNo.Enabled = True Then
					txtPartNo.Focus()
				End If

				DataFieldBind()
				SetPage()
				ControlVisibility()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub imgbtnPartNo_Click(sender As Object, e As EventArgs) Handles imgbtnPartNo.Click
		setObject()
		mEnquiry.EnquiryItems.CurrentItem.ModelID = Guid.Empty
		Session("mEnquiry") = mEnquiry
		Session("PartNo") = txtPartNo.Text
		Session("mPriorityList") = mPriorityList
		Session("mItemTypeList") = mItemTypeList
		'Response.Redirect("wfPartStockStatusListForEnquiry.aspx?BackPage=wfEnquiry_Ajax.aspx&ChildPage=wfEnquiryItem_Ajax.aspx")
	End Sub

	Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
		If (Not IsInRole(Rights.[New]) And mEnquiry.IsNew) Or (Not IsInRole(Rights.Edit) And Not mEnquiry.IsNew) Then
			setObject()
			setSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If IsValid Then
			If setObject() Then
				Session("mEnquiry") = mEnquiry
				RemoveSession()
				Session.Remove("Edit")
				Response.Redirect(Request.QueryString("BackPage"))
			End If
		End If
	End Sub

	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
		If mEnquiry.EnquiryItems.CurrentItem.IsNew And Not Session("Edit") = True Then mEnquiry.EnquiryItems.Remove(mEnquiry.EnquiryItems.CurrentItem)
		Session.Remove("Edit")
		RemoveSession()
		Response.Redirect(Request.QueryString("BackPage"))
	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Private Sub HdnBtnItemList(sender As Object, e As EventArgs) Handles hdnimgBtnItemList.Click

		Try

			ControlVisibility()

			mModelList = ModelList.GetModelList(mEnquiry.EnquiryItems.CurrentItem.ItemID, True)
			Session("mModelList") = mModelList
			cmbApplicable.DataSource = mModelList
			cmbApplicable.DataBind()
			upnlEnqItemDetails.DataBind()
			upnlEnqItemDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class