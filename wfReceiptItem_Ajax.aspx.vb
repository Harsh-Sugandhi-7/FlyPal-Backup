Public Class wfReceiptItem_Ajax
	Inherits Page

#Region " Variable Declaration "

	Public mReceipt As Receipt
    Private mReceiptItem As ReceiptItem
    Public mStoreList As StoreList
    Public mTotalPendingItemQty As Decimal = 0
    Public TotalCount As Decimal = 0
    Public mItemTypeList As PartTypeList
    Public mUnitConverterList As UnitConverterList
    Public mSelectPeriods As SelectPeriods = SelectPeriods.NewSelectPeriods
    Private Flag As Int16
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mLastWarrantyInformation As LastWarrantyInformation
    Public mStore As Store
    Dim mIsOwnedByCustomer As Boolean
    Public mWarrantyStatusList As WarrantyStatusList
	Public mUserHasNoStoreRights As UserHasNoStoreRights

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mReceipt = CType(Session("mReceipt"), Receipt)
		mStoreList = CType(Session("mStoreList"), StoreList)
		mItemTypeList = CType(Session("mItemTypeList"), PartTypeList)
		mTotalPendingItemQty = Session("mTotalPendingItemQty")
		TotalCount = Session("TotalCount")
		mUnitConverterList = CType(Session("mUnitConverterList"), UnitConverterList)
		mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
		mFileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
	End Sub

	Private Sub SetSession()
		Session("mReceipt") = mReceipt
		Session("mStoreList") = mStoreList
		Session("mItemTypeList") = mItemTypeList
		Session("mSelectPeriods") = mSelectPeriods
		Session("mUnitConverterList") = mUnitConverterList
		Session("mFileAttach") = mFileAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
	End Sub

	Private Sub RemoveSessions()
		Session.Remove("mStoreList")
		Session.Remove("mItemTypeList")
		Session("mTotalPendingItemQty") = 0
		Session("TotalCount") = 0
		Session.Remove("mUnitConverterList")
		Session.Remove("mFileAttach")
		Session.Remove("IsAttachmentDeleted")
	End Sub

	Private Overloads Sub SetFocus(cntrl As WebControl)
		If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
		cntrl.Focus()
	End Sub

	Private Sub AddAttributes()
		txtQuantity.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQuantity').value,event)")
		txtCureQtrs.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCureQtrs').value,event)")
		txtCureYear.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCureYear').value,event)")
		txtExpQrts.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExpQrts').value,event)")
		txtExpYear.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExpYear').value,event)")
		txtWarrantyInDays.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtWarrantyInDays').value,event)")
		'Added By Vikrant On 11-Aug-2016 For ALL11082016
		txtExcessQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExcessQty').value,event)")
		txtShortQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtShortQty').value,event)")
		txtRejectedQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRejectedQty').value,event)")
		'End

		'Ajay 10-03-2023
		txtCureQtrs.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQuantity').value,event)")
		txtCureYear.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQuantity').value,event)")
		txtExpQrts.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQuantity').value,event)")
		txtExpYear.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQuantity').value,event)")
		txtWarrantyInDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQuantity').value,event)")
		'--------------------
	End Sub

	Private Sub SetPage()
		If Session("Edit") Then
			lblTitle.Text = "Receipt Item [" & mReceipt.ReceiptItems.CurrentItem.ItemName & "]"
			imgPartNo.BackColor = Color.Silver
			txtPartNo.BackColor = Color.Silver
			txtPartNo.ToolTip = "Part Number"
		End If
		lblSerializedStatus.Visible = mReceipt.ReceiptItems.CurrentItem.IsSerialized And Not Session("Edit")
		If mTotalPendingItemQty - TotalCount + 1 > mTotalPendingItemQty Then
			lblSerializedStatus.Text = " Extra Item : You are trying to add more Items."
		Else
			lblSerializedStatus.Text = "Receiving Serialized Part: " + CType(mTotalPendingItemQty - TotalCount + 1, String) + "/" + CType(mTotalPendingItemQty, String)
		End If
		If chkIsInWarranty.Checked = True Then
			txtWarrantyInDays.Enabled = True
			txtWarrantyStartDate.Enabled = True
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyStartDate.Enabled = False
		End If
	End Sub

	Private Sub NewReceiptItem(mReceiptItem As ReceiptItem)
		mReceipt.ReceiptItems.Add(mReceipt.ID, mReceipt.TransTypeID)
		mReceipt.ReceiptItems.CurrentItem.SrNo = mReceipt.ReceiptItems.CurrentIndex + 1   'CInt(Val(txtSrNo.Text))
		mReceipt.ReceiptItems.CurrentItem.ItemID = mReceiptItem.ItemID
		mReceipt.ReceiptItems.CurrentItem.AlternateItemID = mReceiptItem.AlternateItemID 'Added By Saylee on 12th-Feb-2008
		mReceipt.ReceiptItems.CurrentItem.Part = mReceiptItem.Part
		mReceipt.ReceiptItems.CurrentItem.PartDescription = mReceiptItem.PartDescription
		mReceipt.ReceiptItems.CurrentItem.FromItemTypeID = mReceiptItem.FromItemTypeID
		mReceipt.ReceiptItems.CurrentItem.StockBalanceQty = mReceiptItem.StockBalanceQty
		mReceipt.ReceiptItems.CurrentItem.OrderItemID = mReceiptItem.OrderItemID
		mReceipt.ReceiptItems.CurrentItem.OrderDate = mReceiptItem.OrderDate
		mReceipt.ReceiptItems.CurrentItem.IssueItemID = mReceiptItem.IssueItemID
		mReceipt.ReceiptItems.CurrentItem.IssueDate = mReceiptItem.IssueDate
		mReceipt.ReceiptItems.CurrentItem.FromPartList = mReceiptItem.FromPartList
		mReceipt.ReceiptItems.CurrentItem.ReleaseNoteNo = mReceiptItem.ReleaseNoteNo
		mReceipt.ReceiptItems.CurrentItem.ReleaseNoteDate = mReceiptItem.ReleaseNoteDate
		mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = mReceiptItem.DisplayUnitID   'Added By Prashant 11-May-2010
		mReceipt.ReceiptItems.CurrentItem.DisplayUnitName = mReceiptItem.DisplayUnitName     'Added By Prashant 11-May-2010
		mReceipt.ReceiptItems.CurrentItem.DisplayQty = 1                               'Added By Prashant 11-May-2010
		mReceipt.ReceiptItems.CurrentItem.SerialNo = ""
		mReceipt.ReceiptItems.CurrentItem.StoreID = mReceiptItem.StoreID
		mReceipt.ReceiptItems.CurrentItem.Location = mReceiptItem.Location
		mReceipt.ReceiptItems.CurrentItem.StartDate = mReceiptItem.StartDate
		mReceipt.ReceiptItems.CurrentItem.ExpiryDate = mReceiptItem.ExpiryDate
		mReceipt.ReceiptItems.CurrentItem.Remark = mReceiptItem.Remark
		mReceipt.ReceiptItems.CurrentItem.Note = mReceiptItem.Note
		mReceipt.ReceiptItems.CurrentItem.IsWarranty = mReceiptItem.IsWarranty
		mReceipt.ReceiptItems.CurrentItem.WarrantyInDays = mReceiptItem.WarrantyInDays
		mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate = mReceiptItem.WarrantyStartDate
		mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate = mReceiptItem.WarrantyExpiryDate
		mReceipt.ReceiptItems.CurrentItem.CureQtrs = mReceiptItem.CureQtrs 'Code Added By Deven ===== on 30-06-2008-------------------
		mReceipt.ReceiptItems.CurrentItem.CureYear = mReceiptItem.CureYear
		mReceipt.ReceiptItems.CurrentItem.ExpQtrs = mReceiptItem.ExpQtrs
		mReceipt.ReceiptItems.CurrentItem.ExpYear = mReceiptItem.ExpYear  '----------------------------------------------------------
		mReceipt.ReceiptItems.CurrentItem.BatchNo = mReceiptItem.BatchNo 'Added By Prashant 19/Aug/2008
		mReceipt.ReceiptItems.CurrentItem.CalibrationDoneOnDate = mReceiptItem.CalibrationDoneOnDate 'Added By Prashant 25-Sep-2009
		mReceipt.ReceiptItems.CurrentItem.IsExpiryNA = mReceiptItem.IsExpiryNA '----Added by Vikrant FOR ALL10052012-10--------------
		mReceipt.ReceiptItems.CurrentItem.IsExpiryUnlimited = mReceiptItem.IsExpiryUnlimited '-----------------------------------------------------
		mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = mReceiptItem.PrimaryCategoryID 'Added By Prashant On 07-Oct-2015 For ALL06102015
		mReceipt.ReceiptItems.CurrentItem.CodeNo = ""   'Added By Prashant On 07-Oct-2015 For ALL06102015
		mReceipt.ReceiptItems.CurrentItem.ConditionCheckDoneOnDate = mReceiptItem.ConditionCheckDoneOnDate
		mReceipt.ReceiptItems.CurrentItem.ServiedInspectedCheckDoneOnDate = mReceiptItem.ServiedInspectedCheckDoneOnDate 'Added By Shital On 13-Sep-2019
		mReceipt.ReceiptItems.CurrentItem.WarrantyApplicableStatus = Val(cmbWarrantyStatus.SelectedValue)   '1 Accepted 2 Rejected 0 None
		'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
		mReceipt.ReceiptItems.CurrentItem.ReqEmployeeEmailIDs = mReceiptItem.ReqEmployeeEmailIDs
		mReceipt.ReceiptItems.CurrentItem.ReqNo = mReceiptItem.ReqNo
		mReceipt.ReceiptItems.CurrentItem.ReqEmployeeName = mReceiptItem.ReqEmployeeName
		mReceipt.ReceiptItems.CurrentItem.ReqQty = mReceiptItem.ReqQty
		mReceipt.ReceiptItems.CurrentItem.ReqDate = mReceiptItem.ReqDateFormatted.ToString
		mReceipt.ReceiptItems.CurrentItem.ReqEmployeeID = mReceiptItem.ReqEmployeeID
		mReceipt.ReceiptItems.CurrentItem.ReqItemID = mReceiptItem.ReqItemID
		'End
		mFileAttach = FileAttach.NewAttachmentChild(Guid.NewGuid, mReceipt.ReceiptItems.CurrentItem.ID)
		Session("mFileAttach") = mFileAttach

		mReceipt.ReceiptItems.CurrentItem.ManufacturingDate = mReceiptItem.ManufacturingDate ''Added by Saylee on 9-Mar-2021 for Heligo10032021
	End Sub

	Private Function SetObject() As Boolean
		mReceipt.BeginEdit()
		mReceipt.ReceiptItems.CurrentItem.SrNo = mReceipt.ReceiptItems.CurrentIndex + 1  'CInt(Val(txtSrNo.Text))
		mReceipt.ReceiptItems.CurrentItem.ReleaseNoteNo = Trim(txtReleaseNote.Text)
		If (txtReleaseNoteDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.ReleaseNoteDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.ReleaseNoteDate = CDate(txtReleaseNoteDate.Text)
		End If
		mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = New Guid(cmbUnitConverterList.SelectedValue)
		mReceipt.ReceiptItems.CurrentItem.DisplayUnitName = cmbUnitConverterList.SelectedItem.Text
		mReceipt.ReceiptItems.CurrentItem.DisplayQty = CDec(Val(txtQuantity.Text))
		mReceipt.ReceiptItems.CurrentItem.SerialNo = Trim(txtSerialNo.Text)
		If mReceipt.ReceiptItems.CurrentItem.StoreID.Equals(New Guid(cmbStore.SelectedValue)) = True Then
			'Do nothing
		Else
			mReceipt.ReceiptItems.CurrentItem.ItemTagID = Item.GetItem(mReceipt.ReceiptItems.CurrentItem.ItemID).ItemTagID
			mReceipt.ReceiptItems.CurrentItem.ItemTagName = Item.GetItem(mReceipt.ReceiptItems.CurrentItem.ItemID).ItemTagName
			upnlAttentionInfo.DataBind()
			upnlAttentionInfo.Update()
		End If
		mReceipt.ReceiptItems.CurrentItem.StoreID = New Guid(cmbStore.SelectedValue)
		mReceipt.ReceiptItems.CurrentItem.ItemTypeID = Val(cmbPartType.SelectedValue)
		mReceipt.ReceiptItems.CurrentItem.StoreName = cmbStore.SelectedItem.Text
		mReceipt.ReceiptItems.CurrentItem.Location = Trim(txtLocation.Text)
		If (txtStartDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.StartDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.StartDate = txtStartDate.Text
		End If
		If (txtExpiryDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.ExpiryDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.ExpiryDate = CDate(txtExpiryDate.Text)
		End If
		mReceipt.ReceiptItems.CurrentItem.Remark = Trim(txtRemark.Text)
		mReceipt.ReceiptItems.CurrentItem.Note = Trim(txtNote.Text)
		mReceipt.ReceiptItems.CurrentItem.PreviousWorkScope = Trim(txtPreviousWorkScope.Text)
		'Added By Prashant 12/11/07   'If is in Warranty
		mReceipt.ReceiptItems.CurrentItem.IsWarranty = chkIsInWarranty.Checked
		mReceipt.ReceiptItems.CurrentItem.WarrantyInDays = Val(txtWarrantyInDays.Text)

		If (txtWarrantyStartDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate = CDate(txtWarrantyStartDate.Text)
		End If

		If (txtWarrantyExpiryDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate = CDate(txtWarrantyExpiryDate.Text)
		End If
		'----------------------------

		'Code Added By Deven ===== on 30-06-2008-------------------
		mReceipt.ReceiptItems.CurrentItem.CureQtrs = Val(txtCureQtrs.Text)
		mReceipt.ReceiptItems.CurrentItem.CureYear = Val(txtCureYear.Text)
		mReceipt.ReceiptItems.CurrentItem.ExpQtrs = Val(txtExpQrts.Text)
		mReceipt.ReceiptItems.CurrentItem.ExpYear = Val(txtExpYear.Text)
		'----------------------------------------------------------
		'Added By Prashant 19/Aug/2008
		mReceipt.ReceiptItems.CurrentItem.BatchNo = Trim(txtBatchNo.Text)
		If (txtCalibrationDoneOnDate.Text = "") Then  'Added Ny Prashant 25-sep-2009
			mReceipt.ReceiptItems.CurrentItem.CalibrationDoneOnDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.CalibrationDoneOnDate = CDate(txtCalibrationDoneOnDate.Text)
		End If                                  '----------------------------
		If (txtConditionCheckDoneOnDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.ConditionCheckDoneOnDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.ConditionCheckDoneOnDate = txtConditionCheckDoneOnDate.Text
		End If
		'Added by Shital on 13-Sep-2019
		If (txtServicedInspectedDoneOnDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.ServiedInspectedCheckDoneOnDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.ServiedInspectedCheckDoneOnDate = txtServicedInspectedDoneOnDate.Text
		End If
		'If mReceipt.ReceiptItems.Contains(mReceipt.ReceiptItems.CurrentItem) = True Or mReceipt.ReceiptItems.ContainsAlternatePart(mReceipt.ReceiptItems.CurrentItem) = True Or (mReceipt.ReceiptItems.CurrentItem.FromItemTypeID <> 3 And mReceipt.ReceiptItems.Contains(mReceipt.ReceiptItems.CurrentItem, "") = True) Then
		If mReceipt.ReceiptItems.Contains(mReceipt.ReceiptItems.CurrentItem) = True Or mReceipt.ReceiptItems.ContainsAlternatePart(mReceipt.ReceiptItems.CurrentItem) = True Or (mReceipt.ReceiptItems.CurrentItem.FromItemTypeID <> 3 And mReceipt.ReceiptItems.CurrentItem.FromItemTypeID <> 12 And mReceipt.ReceiptItems.Contains(mReceipt.ReceiptItems.CurrentItem, "") = True) Then
			MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add Duplicate entry in Receipt cum Invoice. <BR><BR> Receipt cum Invoice can not contains non serialized part with same Release Note No.", MsgBoxStyle.OkOnly, "")
			mReceipt.CancelEdit()
			Exit Function
		Else
			mReceipt.ApplyEdit()
		End If
		'If (AppSettings("CodeNo") = "True" And mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = 2 And mReceipt.ReceiptItems.CurrentItem.IsSerialized = True) Then
		'    mReceipt.ReceiptItems.CurrentItem.CodeNo = txtCodeNo.Text.Trim   'Added By Prashant On 07-Oct-2015 For ALL06102015
		'    If (mReceipt.ReceiptItems.ContainsCodeNo(mReceipt.ReceiptItems.CurrentItem) = True) Then 'Added By Prashant On 07-Oct-2015 For ALL06102015    
		'        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You can not add duplicate Code No.", MsgBoxStyle.OkOnly, "")
		'        mReceipt.CancelEdit()
		'        Exit Function
		'    Else
		'        mReceipt.ApplyEdit()
		'    End If
		'End If

		'----Added by Vikrant FOR ALL10052012-10--------------
		mReceipt.ReceiptItems.CurrentItem.IsExpiryNA = chkIsExpiryNA.Checked
		mReceipt.ReceiptItems.CurrentItem.IsExpiryUnlimited = chkIsExpiryUnlimited.Checked
		'-----------------------------------------------------
		mReceipt.ReceiptItems.CurrentItem.IsTransitDamage = chkIsTransitDamage.Checked
		'Added By Vikrant On 11-Aug-2016 For ALL11082016
		mReceipt.ReceiptItems.CurrentItem.ExcessQty = CDec(Val(txtExcessQty.Text))
		mReceipt.ReceiptItems.CurrentItem.ShortQty = CDec(Val(txtShortQty.Text))
		mReceipt.ReceiptItems.CurrentItem.RejectedQty = CDec(Val(txtRejectedQty.Text))
		'End
		mReceipt.ReceiptItems.CurrentItem.WarrantyApplicableStatus = Val(cmbWarrantyStatus.SelectedValue)   '1 Accepted 2 Rejected 0 None


		''Added by Saylee on 9-Mar-2021 for Heligo10032021
		If (txtManufacturingDate.Text = "") Then
			mReceipt.ReceiptItems.CurrentItem.ManufacturingDate = System.DBNull.Value
		Else
			mReceipt.ReceiptItems.CurrentItem.ManufacturingDate = txtManufacturingDate.Text
		End If
		'****************************

		Return True
	End Function

	Private Sub SetGridObject()
		Dim i As Integer
		Dim txtTSNValue1 As TextBox
		Dim txtTSOHValue1 As TextBox
		For i = 0 To dgPeriods.Rows.Count - 1
			txtTSNValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtTSNValue"), TextBox)
			txtTSOHValue1 = CType(Me.dgPeriods.Rows(i).FindControl("txtTSOHValue"), TextBox)

			If mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtTSNValue1.Text) Then
					mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).TSNValueFormatted = ""
				Else
					mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).TSNValueFormatted = Trim(txtTSNValue1.Text)
				End If
			Else
				mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).TSNValue = Trim(txtTSNValue1.Text)
			End If

			If mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtTSOHValue1.Text) Then
					mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).TSOValueFormatted = ""
				Else
					mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).TSOValueFormatted = Trim(txtTSOHValue1.Text)
				End If
			Else
				mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).TSOValue = Trim(txtTSOHValue1.Text)
			End If
		Next i
		Session("mReceipt") = mReceipt
	End Sub

	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							Session("sender") = ""
							Dim mReceipt As Receipt
							mReceipt = CType(Session("mReceipt"), Receipt)
							mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods.RemoveAt(mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods.CurrentIndex)
							Session("mReceipt") = mReceipt
							DataFieldBind()
							upnlTSNTSOValues.Update()
						Catch ex As SqlException
							If ex.Number = 547 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If
						End Try
					End If
					If MSGBoxCtrl.Sender = "StoreTag" Then
						ReceiptItems()
					End If
					If MSGBoxCtrl.Sender = "RemoveAttachment" Then


						Try
							Session("Sender") = ""
							mReceipt = CType(Session("mReceipt"), Receipt)
							mReceipt.ReceiptItems.CurrentItem.FileAttachments.Remove(mReceipt.ReceiptItems.CurrentItem.FileAttachments.CurrentItem)
							dgReceiptAttachment.DataSource = mReceipt.ReceiptItems.CurrentItem.FileAttachments
							dgReceiptAttachment.DataBind()
							upnldgReceiptAttachment.Update()
							upnlAttachment.Update()
							Session("mReceipt") = mReceipt

						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							End If
						End Try
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						DataFieldBind()
					End If
					If MSGBoxCtrl.Sender = "Delete" Then
						DataFieldBind()
					End If
					If MSGBoxCtrl.Sender = "StoreTag" Then
						DataFieldBind()
					End If
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "ResetStore" Then  ''Added By Prashant 13-May-2020
						cmbStore.ClearSelection()
						upnlStore.Update()
					End If
			End Select
		End If
	End Sub

	Private Sub SetPeroids()
		Dim mPeriodlist As PeriodList
		mSelectPeriods = SelectPeriods.NewSelectPeriods
		mPeriodlist = PeriodList.GetPeriodList
		For i As Integer = 0 To mPeriodlist.Count - 1
			If Not mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods.Contains(mPeriodlist(i).ID) Then
				mSelectPeriods.Add(mPeriodlist(i).ID, mPeriodlist(i).PeriodName)
			End If
		Next
		Session("mSelectPeriods") = mSelectPeriods
	End Sub

	Private Sub AddSelectedPeroids()
		Dim mSelectPeriod As SelectPeriod
		If IsNothing(mSelectPeriods) Then
			mSelectPeriods = SelectPeriods.NewSelectPeriods
		End If
		For Each mSelectPeriod In mSelectPeriods
			If mSelectPeriod.IsSelected Then
				mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods.Add(ReceiptItemPeriod.NewReceiptItemPeriod(mReceipt.ReceiptItems.CurrentItem.ID, mReceipt.TransTypeID, mSelectPeriod.PeriodID))
			End If
		Next
		Session("mReceipt") = mReceipt
		Session.Remove("mSelectPeriods")
		mSelectPeriods = Nothing
	End Sub

	Private Sub Controlvisibility()
		'btnAlternatePart.Enabled = CType(mReceipt.TransTypeID, Flypal.Util.Trans) = Util.Trans.ReceiptAgainstPuchaseOrder And mReceipt.ReceiptItems.AllowAlternatePart(mReceipt.ReceiptItems.CurrentItem)
		If (mReceipt.TransTypeID = 6 And mReceipt.ReceiptItems.CurrentItem.IsSerialized = False) Then
			'cmbUnitConverterList.Enabled = (mReceipt.StatusID = 1)
		End If
		If (AppSettings("CodeNo") = "True" And mReceipt.ReceiptItems.CurrentItem.IsSerialized = True And mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = 2) Then
			lblCodeNo.Visible = True
			txtCodeNo.Visible = True
			'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
			lblCodeNo.Text = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "GSE No.", "Code No.")
			txtCodeNo.ToolTip = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "Enter GSE No.", "Enter Code No.")
			'End
		End If
		'Added by Vikrant FOR ALL10052012-10
		'If chkIsExpiryNA.Checked Or chkIsExpiryUnlimited.Checked Then
		If (chkIsExpiryNA.Checked Or chkIsExpiryUnlimited.Checked) And (AppSettings("ClientCode") <> "IND") Then 'IND'Commneted and Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
			If chkIsExpiryNA.Checked Then
				chkIsExpiryUnlimited.Checked = False
				chkIsExpiryUnlimited.Enabled = False
			ElseIf chkIsExpiryUnlimited.Checked Then
				chkIsExpiryNA.Checked = False
				chkIsExpiryNA.Enabled = False
			End If
			txtStartDate.Enabled = False
			txtExpiryDate.Enabled = False
			txtCureQtrs.Enabled = False
			txtCureYear.Enabled = False
			txtExpQrts.Enabled = False
			txtExpYear.Enabled = False

			txtStartDate.Text = ""
			txtExpiryDate.Text = ""
			txtCureQtrs.Text = "0"
			txtCureYear.Text = "0"
			txtExpQrts.Text = "0"
			txtExpYear.Text = "0"

			If mReceipt.StatusID = 2 Then
				chkIsExpiryNA.Enabled = False
				chkIsExpiryUnlimited.Enabled = False
			End If
		Else
			chkIsExpiryNA.Enabled = (mReceipt.StatusID = 1)
			chkIsExpiryUnlimited.Enabled = (mReceipt.StatusID = 1)
			txtStartDate.Enabled = (mReceipt.StatusID = 1)
			txtExpiryDate.Enabled = (mReceipt.StatusID = 1)
			txtCureQtrs.Enabled = (mReceipt.StatusID = 1)
			txtCureYear.Enabled = (mReceipt.StatusID = 1)
			txtExpQrts.Enabled = (mReceipt.StatusID = 1)
			txtExpYear.Enabled = (mReceipt.StatusID = 1)
		End If
		'Ajay

		dgReceiptAttachment.Columns(6).Visible = (mReceipt.StatusID = 1) 'Attachmnet Delete
		'-------------
		'PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
		imgPartNo.Enabled = (mReceipt.StatusID = 1)
		btnAlternatePart.Enabled = (mReceipt.StatusID = 1)
		cmbPartType.Enabled = (mReceipt.StatusID = 1)
		txtQuantity.Enabled = (mReceipt.StatusID = 1)
		txtReleaseNote.Enabled = (mReceipt.StatusID = 1)
		txtReleaseNoteDate.Enabled = (mReceipt.StatusID = 1)
		cmbStore.Enabled = (mReceipt.StatusID = 1)
		txtSerialNo.Enabled = (mReceipt.StatusID = 1)
		txtLocation.Enabled = (mReceipt.StatusID = 1)

		'chkIsExpiryNA.Enabled = (mReceipt.StatusID = 1)
		'chkIsExpiryUnlimited.Enabled = (mReceipt.StatusID = 1)
		'txtStartDate.Enabled = (mReceipt.StatusID = 1)
		'txtExpiryDate.Enabled = (mReceipt.StatusID = 1)
		'txtCureQtrs.Enabled = (mReceipt.StatusID = 1)
		'txtCureYear.Enabled = (mReceipt.StatusID = 1)
		'txtExpQrts.Enabled = (mReceipt.StatusID = 1)
		'txtExpYear.Enabled = (mReceipt.StatusID = 1)
		chkIsInWarranty.Enabled = (mReceipt.StatusID = 1)
		'txtWarrantyStartDate.Enabled = (mReceipt.StatusID = 1)
		txtCalibrationDoneOnDate.Enabled = (mReceipt.StatusID = 1)
		txtConditionCheckDoneOnDate.Enabled = (mReceipt.StatusID = 1)
		txtServicedInspectedDoneOnDate.Enabled = (mReceipt.StatusID = 1)         'Added by Shital on 13-Sep-2019

		txtRemark.Enabled = (mReceipt.StatusID = 1 Or mReceipt.StatusID = 2) ''APFT :ALL18012018 added Or mReceipt.StatusID = 2 by Saylee  on 18-Jan-2019 to open button after authorization ,to save rematk and note
		txtNote.Enabled = (mReceipt.StatusID = 1 Or mReceipt.StatusID = 2) ''APFT :ALL18012018 added Or mReceipt.StatusID = 2 by Saylee  on 18-Jan-2019 to open button after authorization ,to save rematk and note
		txtPreviousWorkScope.Enabled = (mReceipt.StatusID = 1)
		''Commented and Added by Saylee on 20-Feb-2023 for new Style
		''btnAddPeroid.Enabled = (mReceipt.StatusID = 1)
		ImgAddPeroid.Enabled = (mReceipt.StatusID = 1)
		'*****************************

		txtWarrantyInDays.Enabled = (mReceipt.StatusID = 1)
		ImgPartType.Enabled = (mReceipt.StatusID = 1)
		btnOK.Enabled = (mReceipt.StatusID = 1 Or mReceipt.StatusID = 2) ''APFT :ALL18012018 added Or mReceipt.StatusID = 2 by Saylee  on 18-Jan-2019 to open button after authorization ,to save rematk and note

		' btnSelectFile.Disabled = IIf(mReceipt.StatusID >= 2, True, False)
		' btnDelAttach1.Enabled = (mReceipt.StatusID = 1)
		'ImageButton1.Enabled = (mReceipt.StatusID = 1)
		'----
		chkIsTransitDamage.Enabled = (mReceipt.StatusID = 1)
		txtCodeNo.Enabled = (mReceipt.StatusID = 1)
		'If (mReceipt.StatusID = 1 And mReceipt.TransTypeID = 6) Then
		'    txtCodeNo.Enabled = True
		'ElseIf (mReceipt.StatusID = 1 And mReceipt.TransTypeID <> 6 And mReceipt.ReceiptItems.CurrentItem.CodeNo <> "") Then
		'    txtCodeNo.Enabled = False
		'ElseIf (mReceipt.StatusID = 1 And mReceipt.TransTypeID <> 6 And mReceipt.ReceiptItems.CurrentItem.CodeNo = "") Then
		'    txtCodeNo.Enabled = True
		'Else
		'    txtCodeNo.Enabled = False
		'End If
		'PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
		If (chkIsInWarranty.Checked = True And mReceipt.StatusID = 1) Then
			txtWarrantyInDays.Enabled = True
			If Val(txtWarrantyInDays.Text) > 0 Then
				txtWarrantyStartDate.Enabled = True
			Else
				txtWarrantyStartDate.Enabled = False
			End If
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyStartDate.Enabled = False
		End If
		'If ((AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "LAMA") And (mReceipt.TransTypeID = 6 Or mReceipt.TransTypeID = 10)) Then
		If (AppSettings("ClientCode") = "CE" And (mReceipt.TransTypeID = 6 Or mReceipt.TransTypeID = 10)) Then
			mIsOwnedByCustomer = IIf(cmbStore.SelectedIndex > 0, Store.GetStore(New Guid(cmbStore.SelectedValue)).IsOwnedByCustomer, False)
			If mIsOwnedByCustomer = False Then
				txtBatchNo.Enabled = False
			Else
				txtBatchNo.Enabled = True
			End If
		Else
			txtBatchNo.Enabled = (mReceipt.StatusID = 1)
		End If
		txtManufacturingDate.Enabled = (mReceipt.StatusID = 1) 'Added by Saylee on 9-Mar-2021 for Heligo10032021


	End Sub

	Private Sub ControlVisibilityForExpiryInfo() '----Added by Vikrant FOR ALL10052012-10--------------

		Try

			If (
				(txtStartDate.Text <> "" Or txtExpiryDate.Text <> "") Or (txtCureQtrs.Text <> "0" And txtCureQtrs.Text <> "") Or
				(txtCureYear.Text <> "0" And txtCureYear.Text <> "") Or (txtExpQrts.Text <> "0" And txtExpQrts.Text <> "") Or
				(txtExpYear.Text <> "0" And txtExpYear.Text <> "")
			   ) And (AppSettings("ClientCode") <> "IND") Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				chkIsExpiryNA.Enabled = False
				chkIsExpiryUnlimited.Enabled = False
			ElseIf Session("EditForExpiryInfo") = "True" Then
				Session("EditForExpiryInfo") = "False"
				chkIsExpiryNA.Enabled = IIf(chkIsExpiryNA.Checked, True, False)
				chkIsExpiryUnlimited.Enabled = IIf(chkIsExpiryUnlimited.Checked, True, False)
			Else
				chkIsExpiryNA.Enabled = True
				chkIsExpiryUnlimited.Enabled = True
			End If

			'If AppSettings("ClientCode") = "Heligo" AndAlso
			'   mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 AndAlso
			'   mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter = 0 Then

			'	pnlExpiryDetails.Enabled = False
			'	pnlExpiryDetails.CssClass &= " disabled-panel"
			'	pnlExpiryDetails.Attributes("data-message") = "Control as disabled as Expiry Details are not mentioned in Part Master."

			'End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibilityForExcessQty()
		If Val(txtExcessQty.Text) > 0 Then
			txtShortQty.Enabled = False
		Else
			txtShortQty.Enabled = (mReceipt.StatusID = 1)
		End If
		If Val(txtShortQty.Text) > 0 Then
			txtExcessQty.Enabled = False
		Else
			txtExcessQty.Enabled = (mReceipt.StatusID = 1)
		End If
	End Sub

	Private Sub AttachMyFile()
		Try
			mReceipt.ReceiptItems.CurrentItem.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
			mReceipt.ReceiptItems.CurrentItem.Size = Session("FileUpload.FileSize")
			mReceipt.ReceiptItems.CurrentItem.Extension = Session("FileUpload.FileExtension")
			Session.Remove("FileUpload.FileSize")
			Session.Remove("FileUpload.FileContent")
			Session.Remove("FileUpload.FileExtension")
			ControlVisibilityForExpCalibration()
		Catch ex As Exception
			MSGBoxCtrl.Show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
		End Try
	End Sub

	Private Sub ControlVisibilityForExpCalibration()
		If mReceipt.ReceiptItems.CurrentItem.IsAttachmentAdded = True Then
			'   ImageButton1.Visible = True
			' btnDelAttach1.Enabled = True
		Else
			'  ImageButton1.Visible = False
			' btnDelAttach1.Enabled = False
		End If
		upnlAttachment.Update()
	End Sub

	Private Sub SetControl() 'Added By Prashant On 07-Oct-2015 For ALL06102015
		If (mReceipt.TransTypeID <> 6) Then
			mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceipt.ReceiptItems.CurrentItem.ItemID.ToString, txtSerialNo.Text)
			If mLastWarrantyInformation.Count > 0 Then
				txtCodeNo.Text = mLastWarrantyInformation(0).CodeNo
				'txtCodeNo.Enabled = False
				mLastWarrantyInformation = Nothing
			Else
				txtCodeNo.Text = ""
				txtCodeNo.Enabled = True
			End If
			upnlReceivingInformation1.Update()
		End If
	End Sub

#End Region

#Region " Custom Validation(s) "

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
		Dim mItem As Item
		mItem = Item.GetItem(mReceipt.ReceiptItems.CurrentItem.ItemID)

		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		Dim mQtyBalReceived As Decimal = 0
		If custValidator.ControlToValidate = "cmbStore" Then
			If cmbStore.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Please Select the Store"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "cmbPartType" Then
			If cmbPartType.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Please Select the Part Type"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtExpiryDate" Then
			If IsDate(txtExpiryDate.Text) Then
				If Not IsDate(txtExpiryDate.Text) Or Not IsDate(mReceipt.ReceiptItems.CurrentItem.StartDate) Then
					If Not IsDate(txtExpiryDate.Text) Then
						txtExpiryDate.Text = ""
					ElseIf (Not txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And ((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 Or mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0)) Then
						custValidator.ErrorMessage = "Select Cure Date "
						e.IsValid = False
					Else
						e.IsValid = True
					End If
				ElseIf CDate(txtExpiryDate.Text) < (mReceipt.ReceiptItems.CurrentItem.StartDate) Then
					custValidator.ErrorMessage = "Expiry date should be Later to Cure Date."
					e.IsValid = False
					'ElseIf (Not txtExpiryDate.Text = "" And txtStartDate.Text = "") And _
					'        ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And _
					'        ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And _
					'        (mItem.IsExpiryItem = True) Then  'Added by Prashant On 10-Aug-2020 All10082020
					'    custValidator.ErrorMessage = "Enter Expiry Information"
					'    e.IsValid = False
				ElseIf (Not txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) Then
					custValidator.ErrorMessage = "Select Start Date "
					e.IsValid = False
				End If
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtStartDate" Then
			If (txtExpiryDate.Text.ToString = "" And Not txtStartDate.Text.ToString = "") And
				((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And
				((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And
				((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 Or mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0)) And
				Not (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "IND") Then
				custValidator.ErrorMessage = "Select Expiry Date "
				e.IsValid = False
			ElseIf (txtExpiryDate.Text = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And
				AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Then
				custValidator.ErrorMessage = "Select Expiry Date "
				e.IsValid = False
				'ElseIf (txtExpiryDate.Text = "" And Not txtStartDate.Text = "") And _
				'   ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And _
				'   ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And _
				'    (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo") Then 'Added by Prashant On 10-Aug-2020 All10082020

				'    custValidator.ErrorMessage = "Select Expiry Date "
				'    e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtQuantity" Then
			If Session("Edit") = True Then
				mQtyBalReceived = Session("mQtyBalReceived")
			Else
				mQtyBalReceived = CDec(Session("mTotalPendingItemQty"))
				Session("mQtyBalReceived") = mQtyBalReceived
			End If
			If Val(txtQuantity.Text) <= 0 Then
				custValidator.ErrorMessage = "Quantity shoud be non-zero Positive integer."
				e.IsValid = False
			ElseIf Val(txtQuantity.Text) <> 1 And Len(txtSerialNo.Text.Trim) = 0 And mReceipt.ReceiptItems.CurrentItem.IsSerialized Then
				custValidator.ErrorMessage = "Serialized Item should be a Single item. <BR> Serial No. Required Since Part is Serialized."
				e.IsValid = False
			ElseIf Val(txtQuantity.Text) <> 1 And mReceipt.ReceiptItems.CurrentItem.IsSerialized Then
				custValidator.ErrorMessage = "Serialized Item should be a Single item."
				e.IsValid = False
			ElseIf Len(txtSerialNo.Text.Trim) = 0 And mReceipt.ReceiptItems.CurrentItem.IsSerialized Then
				custValidator.ErrorMessage = "Serial No. Required Since Part is Serialized."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtRemark" Then
			If Len(txtRemark.Text) > 500 Then
				custValidator.ErrorMessage = "Maximun Length of Remark should be 500."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtNote" Then
			If Len(txtNote.Text) > 500 Then
				custValidator.ErrorMessage = "Maximun Length of Note should be 500."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtPreviousWorkScope" Then
			If Len(txtPreviousWorkScope.Text) > 500 Then
				custValidator.ErrorMessage = "Maximun Length of Previous Work Scope should be 500."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtCureQtrs" Then
			If (Not txtExpiryDate.Text.ToString = "" Or Not txtStartDate.Text.ToString = "") And (Val(txtCureQtrs.Text) <> 0 Or Val(txtCureYear.Text) <> 0 Or Val(txtExpQrts.Text) <> 0 Or Val(txtExpYear.Text) <> 0) Then
				custValidator.ErrorMessage = "Enter either Cure/Expiry Date or Cure/Expiry Quarters."
				e.IsValid = False
			ElseIf Val(txtCureQtrs.Text) < 0 Or Val(txtCureQtrs.Text) > 4 Then
				custValidator.ErrorMessage = "Cure Quarters should be between 1 to 4"
				e.IsValid = False
			ElseIf (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0") Then
				custValidator.ErrorMessage = "Cure Year also required with Cure Qtrs."
				e.IsValid = False
				''Commented and changed by Saylee on 6-Jan-2009
				'' If txtCalibrationDoneOnDate.Value.ToString = "" And mItem.StatusEquipment = True Then
			ElseIf (mItem.StatusEquipment = True And mItem.BenchmarkMonths > 0 And mItem.CalibrationPeriodInID > 0) Then
				If txtCalibrationDoneOnDate.Text.ToString = "" Then
					custValidator.ErrorMessage = "Part is Calibrated so Calibration Start Date required"
					e.IsValid = False
				ElseIf txtManufacturingDate.Text.ToString = "" And AppSettings("ClientCode") = "Heligo" Then
					custValidator.ErrorMessage = "Part is Calibrated so Manufacturing Date required"
					e.IsValid = False
				ElseIf txtManufacturingDate.Text.ToString <> "" Then
					If CDate(txtCalibrationDoneOnDate.Text) < CDate(txtManufacturingDate.Text) Then
						custValidator.ErrorMessage = "Manufacturing Date should be less than or equal to Calibration Date"
						e.IsValid = False
					End If
				Else 'Added By Vikrant On 17-Jul-2018 For ALL17072018-1
					Dim mCalibrationItemChildList As CalibrationItemChildList
					Dim moldCalibrationItemChild As CalibrationItemChild

					mCalibrationItemChildList = CalibrationItemChildList.GetCalibrationChildList(FromDate:="1/1/1900", ToDate:="1/1/3300", ItemName:=mReceipt.ReceiptItems.CurrentItem.ItemName, Description:=mReceipt.ReceiptItems.CurrentItem.ItemDescription, SerialNo:=mReceipt.ReceiptItems.CurrentItem.SerialNo, ReceiptItemIDToBeSkipped:=mReceipt.ReceiptItems.CurrentItem.ID.ToString)
					If mCalibrationItemChildList.Count > 0 Then
						moldCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(mCalibrationItemChildList(0).ID)
						If CDate(txtCalibrationDoneOnDate.Text) < CDate(moldCalibrationItemChild.DoneOnDate) Then
							custValidator.ErrorMessage = "Calibration Date should be greater than or equal to Last Calibration date (" + moldCalibrationItemChild.DoneOnDateFormatted.ToString + ")"
							e.IsValid = False
						End If
					End If

				End If
				'           ElseIf (txtConditionCheckDoneOnDate.Text = "" And _
				'((mItem.IsConditionCheck = True And mItem.ConditionCheckInterval > 0 And mItem.ConditionCheckIntervalIn > 0) Or (mItem.IsServicedInspected = True And mItem.ServicedInspectedInterval > 0 And mItem.ServicedInspectedIntervalIn > 0))) Then
				'               If mItem.IsConditionCheck = True Then
				'                   custValidator.ErrorMessage = "Part is Condition Checked so Condition Check Start Date required"
				'               Else
				'                   custValidator.ErrorMessage = "Part is Serviced/Inspected so Serviced Inspected Start Date required"
				'               End If
				'               e.IsValid = False

			ElseIf ((txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "")) And ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And ((txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0")) And ((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0)) Then
				custValidator.ErrorMessage = "Expiry Year and Expiry Quarters required."
				e.IsValid = False
				'ElseIf ((txtExpiryDate.Text = "" And txtStartDate.Text = "")) And _
				'        ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And _
				'        ((txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0")) And _
				'        (mItem.IsExpiryItem = True) Then 'Added by Prashant On 10-Aug-2020 All10082020

				'    custValidator.ErrorMessage = "Enter Expiry Information"
				'    e.IsValid = False
				'ElseIf (((mItem.IsExpiryItem = True) And (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo")) And _
				'    ((txtStartDate.Text = "" And txtExpiryDate.Text = "") And _
				'    ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And _
				'    ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")))) Then  'Added by Prashant On 10-Aug-2020 All10082020
				'    custValidator.ErrorMessage = "Enter Expiry Information"
				'    e.IsValid = False
			ElseIf ((
					(mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 Or mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0) And
					Not (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "IND")
					) And
					((txtStartDate.Text = "" And txtExpiryDate.Text = "") And ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And
																			   (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
																			   ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And
																			   (txtCureYear.Text = "" Or txtCureYear.Text = "0")))) Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				custValidator.ErrorMessage = "As " & mReceipt.ReceiptItems.CurrentItem.ExpiryPeriod & ". Enter Expiry Information"
				e.IsValid = False
			ElseIf ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or
				   ((AppSettings("ClientCode") = "IND" Or Appsettings("ClientCode") = "Heligo") And mItem.IsExpiryItem = True)) And
				   ((txtStartDate.Text = "" And txtExpiryDate.Text = "") And
				   ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
				   ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")) And
				   (chkIsExpiryNA.Checked = False) And (chkIsExpiryUnlimited.Checked = False))) Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				custValidator.ErrorMessage = "Enter Expiry Information"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtConditionCheckDoneOnDate" Then

			If (txtConditionCheckDoneOnDate.Text = "" And ((mItem.IsConditionCheck = True And mItem.ConditionCheckInterval > 0 And mItem.ConditionCheckIntervalIn > 0))) Then
				If mItem.IsConditionCheck = True Then
					custValidator.ErrorMessage = "Part is Condition Checked so Condition Check Start Date required"
				End If
				e.IsValid = False
			End If

		ElseIf custValidator.ControlToValidate = "txtServicedInspectedDoneOnDate" Then

			If (txtServicedInspectedDoneOnDate.Text = "" And (mItem.IsServicedInspected = True And mItem.ServicedInspectedInterval > 0 And mItem.ServicedInspectedIntervalIn > 0)) Then
				If mItem.IsServicedInspected = True Then
					custValidator.ErrorMessage = "Part is Serviced/Inspected so Serviced Inspected Start Date required"
				End If
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtExpQrts" Then
			If Val(txtExpQrts.Text) < 0 Or Val(txtExpQrts.Text) > 4 Then
				custValidator.ErrorMessage = "Expiry Quarters should be between 1 to 4"
				e.IsValid = False
			ElseIf (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0") Then
				custValidator.ErrorMessage = "Expiry Year also required with Expiry Qtrs."
				e.IsValid = False
			ElseIf ((txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "")) And ((txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0")) And ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")) And ((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 Or mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0)) Then
				custValidator.ErrorMessage = "Cure Year and Cure Quarters required."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
			'------------------------------
		ElseIf custValidator.ControlToValidate = "txtExpYear" Then
			If (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0") Then
				custValidator.ErrorMessage = "Expiry Qtrs also required with Expiry Year."
				e.IsValid = False
			ElseIf txtExpYear.Text <> "0" And txtExpYear.Text <> "" And Len(txtExpYear.Text) < 4 Then
				custValidator.ErrorMessage = "Expiry Year should be not be less than 4 digits"
				e.IsValid = False
			ElseIf txtExpYear.Text <> "0" And txtExpYear.Text <> "" And Val(txtExpYear.Text) < 1753 Or Val(txtExpYear.Text) > 3030 Then
				custValidator.ErrorMessage = "Enter valid Expiry Year"
				e.IsValid = False
			ElseIf (txtCureYear.Text <> "0" And txtExpYear.Text <> "0") And (Val(txtCureYear.Text) > Val(txtExpYear.Text)) Then
				custValidator.ErrorMessage = "Expiry Year should be Later to Cure Year."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtCureYear" Then
			If (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") Then
				custValidator.ErrorMessage = "Cure Qtrs also required with Cure Year."
				e.IsValid = False
			ElseIf txtCureYear.Text <> "0" And Len(txtCureYear.Text) < 4 Then
				custValidator.ErrorMessage = "Cure Year should be not be less than 4 digits"
				e.IsValid = False
			ElseIf txtCureYear.Text <> "0" And Val(txtCureYear.Text) < 1753 Or Val(txtCureYear.Text) > 3030 Then
				custValidator.ErrorMessage = "Enter valid Cure Year"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtCodeNo" Then
			If (AppSettings("CodeNo") = "True" And mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = 2 And mItem.SerialisedStatus = True) Then
				If (txtCodeNo.Text.Length = 0 Or txtCodeNo.Text.Trim = "") Then
					custValidator.ErrorMessage = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "GSE No. Required", "Code No. Required")
					e.IsValid = False
				Else
					e.IsValid = True
				End If
			End If
		ElseIf custValidator.ControlToValidate = "cmbWarrantyStatus" Then
			If (cmbWarrantyStatus.SelectedIndex = 0 And mReceipt.ReceiptItems.CurrentItem.IsWarrantyApplicableCheckedInOrderItem = True) Then
				custValidator.ErrorMessage = "Please Select Warranty Status As Accepted Or Rejected"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtReleaseNote" Then
			If (txtReleaseNote.Text.Trim = "" Or txtReleaseNote.Text.Trim = String.Empty) And AppSettings("ReleaseNoteNoRequire").ToUpper = "True".ToUpper Then
				custValidator.ErrorMessage = "Release Note No. Require."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub

	Public Sub CustomValidate1(s As Object, e As ServerValidateEventArgs)
		If Flag = 1 Then Exit Sub
		Dim CustValidator As CustomValidator
		CustValidator = CType(s, CustomValidator)
		Dim strMsg As String = ""
		SetObject()
		For j As Integer = 0 To mReceipt.ReceiptItems.CurrentItem.GetBrokenRulesCollection.Count - 1
			strMsg = strMsg + mReceipt.ReceiptItems.CurrentItem.GetBrokenRulesCollection(j).Description + "<Br>"
		Next

		If strMsg.Trim <> "" Then
			If mReceipt.ReceiptItems.CurrentItem.GetBrokenRulesCollection.Count = 1 And
				 Val(cmbPartType.SelectedValue) > 0 And strMsg.Trim.Contains("Part Type is required") Then
				'do nothing
			Else
				CustValidator.ErrorMessage = strMsg
				e.IsValid = False
			End If
		End If
		Flag = 1
	End Sub

	Public Function CustomValidate2() As Boolean
		Dim strMsg As String = ""
		For i As Integer = 0 To mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods.Count - 1
			If Not mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).IsValid Then
				For j As Integer = 0 To mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).GetBrokenRulesCollection.Count - 1
					strMsg = strMsg + mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods(i).GetBrokenRulesCollection(j).Description + "<Br>"
				Next
			End If
		Next i
		If strMsg.Trim <> "" Then
			cvExpiryDate.ErrorMessage = strMsg
			cvExpiryDate.IsValid = False
			Return False
		End If
		Return True
	End Function

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()
		mStoreList = StoreList.GetStoreList(0, "", True)
		cmbStore.DataSource = mStoreList
		Session("mStoreList") = mStoreList
		mItemTypeList = PartTypeList.GetPartTypeList(True)
		cmbPartType.DataSource = mItemTypeList
		Session("mItemTypeList") = mItemTypeList
		mUnitConverterList = UnitConverterList.GetUnitConverterList(mReceipt.ReceiptItems.CurrentItem.ItemID, "(SELECT)")
		cmbUnitConverterList.DataSource = mUnitConverterList
		Session("mUnitConverterList") = mUnitConverterList
		txtStartDate.Text = mReceipt.ReceiptItems.CurrentItem.StartDateFormatted.ToString
		txtExpiryDate.Text = mReceipt.ReceiptItems.CurrentItem.ExpiryDateFormatted.ToString
		If mReceipt.ReceiptItems.CurrentItem.IODateFormatted Is Nothing Then 'Added by Prashant 5-Dec-2018 ALL05122018 
			'Do nothing 
		Else
			txtOrderDate.Text = mReceipt.ReceiptItems.CurrentItem.IODateFormatted.ToString
		End If
		txtReleaseNoteDate.Text = mReceipt.ReceiptItems.CurrentItem.ReleaseNoteDateFormatted.ToString
		txtWarrantyStartDate.Text = mReceipt.ReceiptItems.CurrentItem.WarrantyStartDateFormatted.ToString
		txtWarrantyExpiryDate.Text = mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDateFormatted.ToString
		txtCalibrationDoneOnDate.Text = mReceipt.ReceiptItems.CurrentItem.CalibrationDoneOnDateFormatted.ToString 'Added By Prashant 25-Sep-2009
		txtConditionCheckDoneOnDate.Text = mReceipt.ReceiptItems.CurrentItem.ConditionCheckDoneOnDateFormatted.ToString
		txtServicedInspectedDoneOnDate.Text = mReceipt.ReceiptItems.CurrentItem.ServiedInspectedCheckDoneOnDateFormatted.ToString '        'Added by Shital on 13-Sep-2019
		dgPeriods.DataSource = mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods
		mWarrantyStatusList = WarrantyStatusList.GetWarrantyStatusList(True, "(SELECT)")
		cmbWarrantyStatus.DataSource = mWarrantyStatusList

		dgReceiptAttachment.DataSource = mReceipt.ReceiptItems.CurrentItem.FileAttachments 'Added by Shital on 23-Oct-2020

		txtManufacturingDate.Text = mReceipt.ReceiptItems.CurrentItem.ManufacturingDateFormatted.ToString     'Added by Saylee on 9-Mar-2021 for Heligo10032021

		DataBind()
	End Sub


#End Region

#Region " Events "

	Private Sub Page_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		GetSession()
		AddAttributes()
		txtOrderDate.Enabled = False
		If Not IsPostBack And Session("Sender") = "" Then
			If txtPartNo.Enabled = True Then
				SetFocus(txtPartNo)
			End If
			AddSelectedPeroids()
			DataFieldBind()
			Call cmbPartType_SelectedIndexChanged(Nothing, Nothing)  'Added by Utkarsh on 07-Nov-2011 For ALL07112011
		End If
		SetPage()
		ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
		Controlvisibility()
		ControlVisibilityForExpCalibration()
		If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
			txtBarcodeNo.Visible = True
			lblBarcodeNo.Visible = True
		End If
		ControlVisibilityForExcessQty()
	End Sub

	Private Sub imgbtnPartNo_Click(sender As System.Object, e As System.EventArgs) Handles imgPartNo.Click
		SetObject()
		SetGridObject()
		SetSession()
		If mReceipt.TransTypeID = 67 Then
			Session("ItemNo") = txtPartNo.Text
			Response.Redirect("wfSearchPartListForReceipt_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfReceiptItem_Ajax.aspx" & "&mType=1")
		Else
			Dim mPrevTransID As Guid = Guid.Empty
			Dim mPrimaryOrderType As Integer
			Dim mTransaction As Integer
			If (mReceipt.ReceiptItems.Count = 0) Or (mReceipt.ReceiptItems.Count = 1 And mReceipt.ReceiptItems.CurrentItem.IsNew) Then
				mPrevTransID = Guid.Empty
			Else
				mPrevTransID = mReceipt.ReceiptItems.Item(mReceipt.ReceiptItems.Count - 2).OrderItemDetailForReceipt.OrderID
			End If
			mPrimaryOrderType = 3 'TransListOf.Order_Outright
			mTransaction = 3 'Transaction.Order
			Session("mPrevTransID") = mPrevTransID
			Session("mPrimaryOrderType") = mPrimaryOrderType
			Session("mTransaction") = mTransaction
			Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfReceiptItem_Ajax.aspx" & "&mType=1")
		End If
	End Sub

	Private Sub ReceiptItems()
		If TotalCount <= 0 And mReceipt.ReceiptItems.CurrentItem.IsSerialized Then
			Session.Remove("Edit")
			Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		End If
		If Not CustomValidate2() Then Exit Sub
		If SetObject() Then
			If (AppSettings("CodeNo") = "True" And mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = 2 And mReceipt.ReceiptItems.CurrentItem.IsSerialized = True) Then
				mReceipt.ReceiptItems.CurrentItem.CodeNo = txtCodeNo.Text.Trim   'Added By Prashant On 07-Oct-2015 For ALL06102015
				If (mReceipt.ReceiptItems.ContainsCodeNo(mReceipt.ReceiptItems.CurrentItem) = True) Then 'Added By Prashant On 07-Oct-2015 For ALL06102015    
					MSGBoxCtrl.Show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You can not add duplicate Code No.", MsgBoxStyle.OkOnly, "")
					mReceipt.CancelEdit()
					Exit Sub
				Else
					mReceipt.ApplyEdit()
				End If
			End If
			SetGridObject()
			TotalCount -= 1
			Session("TotalCount") = TotalCount
			If TotalCount > 0 And mReceipt.ReceiptItems.CurrentItem.IsSerialized Then
				NewReceiptItem(mReceipt.ReceiptItems.CurrentItem)
				Session.Remove("Edit")
				Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx")
			End If
			Session("mReceipt") = mReceipt
			Session.Remove("Edit")
			If (Not mReceipt.ReceiptItems.CurrentItem.AlternateItemID.Equals(Guid.Empty)) Then 'Added by Prashant  16-Jul-2013 'ALL15072013
				Session("Note") = "Order item is amended as alternate part is received."
			End If
			RemoveSessions()
			Session.Remove("tmpReceipt")
			Response.Redirect("wfReceipt_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub btnOk_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
		If IsValid Then
			If mReceipt.ReceiptItems.CurrentItem.ItemTagID > 0 Then
				mStore = Store.GetStore(New Guid(cmbStore.SelectedValue))
				If mStore.StoreTags.Contains(New Guid(cmbStore.SelectedValue), mReceipt.ReceiptItems.CurrentItem.ItemTagID) = False Then
					'MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Store is not capable to hold this item. As item tag is " + mReceipt.ReceiptItems.CurrentItem.ItemTagName + "</br>Do you what to continue?", MsgBoxStyle.YesNo, "StoreTag")
					MSGBoxCtrl.Show("Alert!", mReceipt.ReceiptItems.CurrentItem.ItemTagName + " Part!", "Selected store does not facilitate to store this part " + mReceipt.ReceiptItems.CurrentItem.ItemName + " as it is tagged as " + mReceipt.ReceiptItems.CurrentItem.ItemTagName + ".</br>Do you want to continue?", MsgBoxStyle.YesNo, "StoreTag")
					Exit Sub
				End If
			End If
			ReceiptItems()
		Else
			upnlValidationSummary.Update()
		End If
	End Sub

	Private Sub btnBack_Click(sender As System.Object, e As System.EventArgs) Handles btnBack.Click
		TotalCount = 0
		RemoveSessions()
		'Added by Saylee on 7-Jun-2011
		If Not Session("tmpReceipt") Is Nothing Then
			mReceipt = CType(Session("tmpReceipt"), Receipt)
			mReceipt.ReceiptItems.CurrentIndex = CType(Session("ItemIndex"), Integer)
			Session("mReceipt") = mReceipt
			Session.Remove("tmpReceipt")
		End If
		'******************************
		If Request.QueryString("BackPage") = "wfReceipt_Ajax.aspx" Or (Request.QueryString("BackPage") = Nothing) Then

			If mReceipt.ReceiptItems.CurrentItem.IsNew And Not Session("Edit") Then mReceipt.ReceiptItems.Remove(mReceipt.ReceiptItems.CurrentItem)
			Session.Remove("Edit")
			Response.Redirect("wfReceipt_Ajax.aspx")
		Else
			Response.Redirect(Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub dgPeriods_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPeriods.RowCommand
		Select Case e.CommandName
			Case "ForDelete"
				Dim index As Integer = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
				MSGBoxCtrl.Show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "Remove Item TSN / TSOH Values.", MsgBoxStyle.YesNo, "Delete")
				mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods.CurrentIndex = index
				Session("mReceipt") = mReceipt
		End Select
	End Sub

	Private Sub btnAddPeriod_Click(sender As System.Object, e As System.EventArgs) Handles ImgAddPeroid.Click
		SetPeroids()
		SetObject()
		SetGridObject()
		Session("mReceipt") = mReceipt
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
		' Response.Redirect("wfSelectPeriod_Ajax.aspx?BackPage2=wfReceiptItem_Ajax.aspx&BackPage=" & Request.QueryString("BackPage"))
	End Sub

	Private Sub hdnAddPeriod_Click(sender As Object, e As System.EventArgs) Handles hdnAddPeriod.Click
		AddSelectedPeroids()
		mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
		dgPeriods.DataSource = mReceipt.ReceiptItems.CurrentItem.ReceiptItemPeriods
		dgPeriods.DataBind()
		upnlTSNTSOValues.Update()
		upnlTabDetails.Update()
	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	Private Sub txtStartDate_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtStartDate.TextChanged 'Added by Prashant as on 15/02/2008
		If IsDate(txtStartDate.Text) Or (txtStartDate.Text = "") Then
			If Not txtStartDate.Text Is mReceipt.ReceiptItems.CurrentItem.StartDate Then
				'Added by Saylee on 7-Jun-2011
				If Not Session("tmpReceipt") Is Nothing Then
					Dim tmpReceipt As Receipt = mReceipt.Clone
					Session("tmpReceipt") = tmpReceipt
					Session("ItemIndex") = mReceipt.ReceiptItems.CurrentIndex
					'*********************************
				End If
				If (txtStartDate.Text.Trim = String.Empty) Then
					mReceipt.ReceiptItems.CurrentItem.StartDate = System.DBNull.Value
					txtStartDate.Text = ""
				Else
					mReceipt.ReceiptItems.CurrentItem.StartDate = txtStartDate.Text.ToString
					txtStartDate.Text = mReceipt.ReceiptItems.CurrentItem.StartDateFormatted
				End If

				If IsDBNull(mReceipt.ReceiptItems.CurrentItem.ExpiryDateFormatted) Then
					txtExpiryDate.Text = ""
				Else
					txtExpiryDate.Text = mReceipt.ReceiptItems.CurrentItem.ExpiryDateFormatted
				End If
			Else
				txtStartDate.Text = ""
			End If
		End If
		ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
	End Sub

	Private Sub btnAlternatePart_Click(sender As System.Object, e As System.EventArgs) Handles btnAlternatePart.Click
		SetObject()
		Session("mItem") = Item.GetItem(mReceipt.ReceiptItems.CurrentItem.ItemID)
		Response.Redirect("wfAlternatePOPartList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfReceiptItem_Ajax.aspx" & "&mType=1&OpenFrom=1") ' OpenFrom=1 for receipt
	End Sub

	Private Sub txtCureQtrs_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCureQtrs.TextChanged
		If Val(txtCureQtrs.Text) >= 0 And Val(txtCureQtrs.Text) <= 4 Then
			'*********************************
			'Added by Saylee on 7-Jun-2011
			If Val(txtCureYear.Text) = 0 And Session("tmpReceipt") Is Nothing Then
				Dim tmpReceipt As Receipt = mReceipt.Clone
				Session("tmpReceipt") = tmpReceipt
				Session("ItemIndex") = mReceipt.ReceiptItems.CurrentIndex
			End If
			'*********************************
			mReceipt.ReceiptItems.CurrentItem.CureQtrs = Val(txtCureQtrs.Text)
			txtExpQrts.DataBind()
			txtExpYear.DataBind()
			If Val(txtCureQtrs.Text) = 0 Then txtCureQtrs.Text = "0"
			ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
		End If
	End Sub

	Private Sub txtCureYear_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCureYear.TextChanged
		If Val(txtCureQtrs.Text) >= 0 And Val(txtCureQtrs.Text) <= 4 Then
			'*********************************
			'Added by Saylee on 7-Jun-2011
			If Val(txtCureQtrs.Text) = 0 And Session("tmpReceipt") Is Nothing Then
				Dim tmpReceipt As Receipt = mReceipt.Clone
				Session("tmpReceipt") = tmpReceipt
				Session("ItemIndex") = mReceipt.ReceiptItems.CurrentIndex
			End If
			'*********************************
			mReceipt.ReceiptItems.CurrentItem.CureYear = Val(txtCureYear.Text)
			txtExpQrts.DataBind()
			txtExpYear.DataBind()
			ControlVisibilityForExpiryInfo() 'Added by Vikrant FOR ALL10052012-10
		End If
	End Sub

	'Added by Shital on 25-Jun-2020
	Private Sub btnSelectFiles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
		If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
			Exit Sub
		End If
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
	End Sub

	Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
		Try
			If Not mReceipt.ReceiptItems.CurrentItem.FileAttachments.Contains(mReceipt.ReceiptItems.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then


				mReceipt.ReceiptItems.CurrentItem.IsAttachmentAdded = True
				' mReceipt.ReceiptItems.CurrentItem.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort) 'Commmented on 23-oct-2020 by Shital
				mReceipt.ReceiptItems.CurrentItem.FileAttachments.Add(mReceipt.ReceiptItems.CurrentItem.ID, CType(Session("FileUpload.FileName"), String))
				mReceipt.ReceiptItems.CurrentItem.FileAttachments.CurrentItem.ImageFile = mFileAttach.ImageFile
				mReceipt.ReceiptItems.CurrentItem.FileAttachments.CurrentItem.Size = mFileAttach.Size
				mReceipt.ReceiptItems.CurrentItem.FileAttachments.CurrentItem.Extension = mFileAttach.Extension


				Session("mReceipt") = mReceipt
				dgReceiptAttachment.DataSource = mReceipt.ReceiptItems.CurrentItem.FileAttachments
				dgReceiptAttachment.DataBind()
				upnldgReceiptAttachment.Update()
				ControlVisibilityForExpCalibration()
			Else
				Session("mReceipt") = mReceipt
				MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		Catch ex As Exception
		End Try
	End Sub

	Private Sub dgRCIAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptAttachment.RowCommand
		Dim mFileAttachments As FileAttachments
		Select Case e.CommandName
			Case "View"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex

				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				mFileAttachments = mReceipt.ReceiptItems.CurrentItem.FileAttachments

				If mFileAttachments.Count = 1 Then
					mFileAttachments.CurrentIndex = 0
				Else
					mFileAttachments.CurrentIndex = Index - 1
				End If

				If mReceipt.ReceiptItems.CurrentItem.FileAttachments.CurrentItem.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
					End If
				End If
				dgReceiptAttachment.DataSource = mReceipt.ReceiptItems.CurrentItem.FileAttachments
				dgReceiptAttachment.DataBind()
				Controlvisibility()
				upnlAttachment.Update()
				upnldgReceiptAttachment.Update()
			Case "Remove"
				'Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex
				Dim Index As Integer = CInt(e.CommandArgument) + dgReceiptAttachment.PageSize * dgReceiptAttachment.PageIndex
				' DeleteAttachment(Index)
				mFileAttachments = mReceipt.ReceiptItems.CurrentItem.FileAttachments
				If mFileAttachments.Count = 1 Then
					DeleteAttachment(0)
				Else
					DeleteAttachment(Index - 1)
				End If
		End Select

	End Sub

	Private Sub DeleteAttachment(Index As Int32)
		MSGBoxCtrl.Show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
		mReceipt.ReceiptItems.CurrentItem.FileAttachments.CurrentIndex = Index
		Session("mReceipt") = mReceipt
	End Sub
	'End 

	Private Sub chkIsInWarranty_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkIsInWarranty.CheckedChanged
		If chkIsInWarranty.Checked = True Then
			txtWarrantyInDays.Enabled = True
			'txtWarrantyStartDate.Enabled = True
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyInDays.Text = "0"
			txtWarrantyStartDate.Enabled = False
			txtWarrantyStartDate.Text = ""
			txtWarrantyExpiryDate.Text = ""
		End If
	End Sub

	Private Sub txtWarrantyStartDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtWarrantyStartDate.TextChanged
		txtWarrantyInDays.Text = mReceipt.ReceiptItems.CurrentItem.WarrantyInDays
		txtWarrantyInDays.DataBind()
		If IsDate(txtWarrantyStartDate.Text) Then
			If Val(txtWarrantyInDays.Text) <> 0 And IsDate(txtWarrantyStartDate.Text) Then
				txtWarrantyExpiryDate.Text = CDate(DateAdd(DateInterval.Day, Val(txtWarrantyInDays.Text), CDate(txtWarrantyStartDate.Text))).ToString(AppSettings("DateFormat").ToString)
			Else
				txtWarrantyExpiryDate.Text = mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate.ToString(AppSettings("DateFormat").ToString)
			End If
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyInDays.Text = "0"
			txtWarrantyStartDate.Enabled = False
			txtWarrantyStartDate.Text = ""
			txtWarrantyExpiryDate.Text = ""
			chkIsInWarranty.Checked = False
		End If
	End Sub

	Private Sub txtWarrantyInDays_TextChanged(sender As Object, e As System.EventArgs) Handles txtWarrantyInDays.TextChanged
		mReceipt.ReceiptItems.CurrentItem.WarrantyInDays = Val(txtWarrantyInDays.Text)
		'txtWarrantyStartDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
		If Val(txtWarrantyInDays.Text) <> 0 Then
			If mReceipt.ReceiptItems.CurrentItem.WarrantyStartDateFormatted.ToString = "" Then
				txtWarrantyStartDate.Text = mReceipt.RecdDateFormatted.ToString
			Else
				txtWarrantyStartDate.Text = mReceipt.ReceiptItems.CurrentItem.WarrantyStartDateFormatted
			End If
			If Val(txtWarrantyInDays.Text) <> 0 And IsDate(txtWarrantyStartDate.Text) Then
				txtWarrantyExpiryDate.Text = CDate(DateAdd(DateInterval.Day, Val(txtWarrantyInDays.Text), CDate(txtWarrantyStartDate.Text))).ToString(AppSettings("DateFormat").ToString)
			Else
				txtWarrantyExpiryDate.Text = mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate.ToString(AppSettings("DateFormat").ToString)
			End If
		Else
			txtWarrantyInDays.Enabled = False
			txtWarrantyInDays.Text = "0"
			txtWarrantyStartDate.Enabled = False
			txtWarrantyStartDate.Text = ""
			txtWarrantyExpiryDate.Text = ""
			chkIsInWarranty.Checked = False
		End If
	End Sub

	Private Sub imgbtnPartType_Click(sender As System.Object, e As System.EventArgs) Handles ImgPartType.Click 'Added By Utkarsh On 22-Sep-2011 For ALL21092011-2
		SetObject()
		Response.Redirect("wfItemType_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfReceiptItem_Ajax.aspx" & "&mType=1&OpenFrom=1") ' OpenFrom=1 for receipt
	End Sub 'End

	Private Sub cmbPartType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbPartType.SelectedIndexChanged 'Added by Utkarsh on 07-Nov-2011 For ALL07112011
		If cmbPartType.SelectedIndex > 0 Then
			lblColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#" & mItemTypeList(cmbPartType.SelectedIndex).Color)
		Else
			lblColor.BackColor = System.Drawing.Color.WhiteSmoke
		End If
		lblPartStatus.Text = IIf(cmbPartType.SelectedIndex > 0, mItemTypeList(cmbPartType.SelectedIndex).PartStatusName, "") 'Added By Vikrant On 31-Oct-2012 For ALL31102012
	End Sub 'End

	Private Sub txtExpQrts_TextChanged(sender As Object, e As System.EventArgs) Handles txtExpQrts.TextChanged '----Added by Vikrant FOR ALL10052012-10--------------
		ControlVisibilityForExpiryInfo()
	End Sub

	Private Sub txtExpYear_TextChanged(sender As Object, e As System.EventArgs) Handles txtExpYear.TextChanged
		ControlVisibilityForExpiryInfo()
	End Sub

	Private Sub txtExpiryDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtExpiryDate.TextChanged
		ControlVisibilityForExpiryInfo()
	End Sub

	Private Sub chkIsExpiryNA_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIsExpiryNA.CheckedChanged
		If chkIsExpiryNA.Checked Then
			If AppSettings("ClientCode") = "IND" Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				'Do nothing 
			Else
				chkIsExpiryUnlimited.Enabled = False
			End If
		End If
	End Sub

	Private Sub chkIsExpiryUnlimited_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIsExpiryUnlimited.CheckedChanged
		If chkIsExpiryUnlimited.Checked Then
			If AppSettings("ClientCode") = "IND" Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
				'Do nothing 
			Else
				chkIsExpiryNA.Enabled = False
			End If
		End If
	End Sub '-----------------------------------------------------

	Private Sub txtReleaseNoteDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtReleaseNoteDate.TextChanged
		If Not IsDate(txtReleaseNoteDate.Text) Then
			txtReleaseNoteDate.Text = ""
		End If
	End Sub

	Private Sub txtCalibrationDoneOnDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtCalibrationDoneOnDate.TextChanged
		If Not IsDate(txtCalibrationDoneOnDate.Text) Then
			txtCalibrationDoneOnDate.Text = ""
		End If
	End Sub

	Private Sub txtConditionCheckDoneOnDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtConditionCheckDoneOnDate.TextChanged
		If Not IsDate(txtConditionCheckDoneOnDate.Text) Then
			txtConditionCheckDoneOnDate.Text = ""
		End If
	End Sub

	Private Sub txtServicedInspectedDoneOnDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtServicedInspectedDoneOnDate.TextChanged
		If Not IsDate(txtServicedInspectedDoneOnDate.Text) Then
			txtServicedInspectedDoneOnDate.Text = ""
		End If
	End Sub

	Private Sub txtSerialNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtSerialNo.TextChanged
		SetControl()
	End Sub

	Private Sub txtExcessQty_TextChanged(sender As Object, e As System.EventArgs) Handles txtExcessQty.TextChanged
		mReceipt.ReceiptItems.CurrentItem.ExcessQty = CDec(Val(txtExcessQty.Text))
		ControlVisibilityForExcessQty()
		txtShortQty.DataBind()
	End Sub

	Private Sub txtShortQty_TextChanged(sender As Object, e As System.EventArgs) Handles txtShortQty.TextChanged
		mReceipt.ReceiptItems.CurrentItem.ShortQty = CDec(Val(txtShortQty.Text))
		ControlVisibilityForExcessQty()
		txtExcessQty.DataBind()
	End Sub
	'End

	Private Sub cmbStore_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbStore.SelectedIndexChanged
		mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, cmbStore.SelectedValue.ToString) ''Added By Prashant 13-May-2020
		If mUserHasNoStoreRights.Count > 0 Then
			MSGBoxCtrl.Show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "ResetStore")
			Exit Sub
		End If
		If mStoreList(New Guid(cmbStore.SelectedValue)).NotInUse = True Then
			If CDate(mStoreList(New Guid(cmbStore.SelectedValue)).NotInUseDate) <= CDate(mReceipt.RecdDate) Then
				MSGBoxCtrl.Show("Alert!", "Store is not applicable since " + mStoreList(New Guid(cmbStore.SelectedValue)).NotInUseDateFormatted, "Select another Store from list or select date before " + mStoreList(New Guid(cmbStore.SelectedValue)).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		End If ''End of Added By Prashant 13-May-2020
		'If (AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "LAMA") Then
		If (AppSettings("ClientCode") = "CE") Then
			mIsOwnedByCustomer = IIf(cmbStore.SelectedIndex > 0, Store.GetStore(New Guid(cmbStore.SelectedValue)).IsOwnedByCustomer, False)
			'If ((AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "LAMA") And (mReceipt.TransTypeID = 6 Or mReceipt.TransTypeID = 10)) Then
			If (AppSettings("ClientCode") = "CE" And (mReceipt.TransTypeID = 6 Or mReceipt.TransTypeID = 10)) Then
				If mIsOwnedByCustomer = False Then
					txtBatchNo.Enabled = False
				Else
					txtBatchNo.Enabled = True
				End If
			Else
				txtBatchNo.Enabled = True
			End If
			upnlReceivingInformation1.Update()
		End If
	End Sub

	Private Sub tabReceiptDetailsContainer_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles tabReceiptDetailsContainer.ActiveTabChanged
		addAttributes()
	End Sub

#End Region

End Class