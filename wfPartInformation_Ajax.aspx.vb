Imports System.Reflection

'Created By Utkarsh On 21-Oct-2013

Public Class wfPartInformation_Ajax
	Inherits System.Web.UI.Page


#Region " Variables and Declarations "
	Public mItem As Item
	Public mUnitList As UnitList
	Public mNomenclatureList As NomenclatureList
	Public mCategoryList As CategoryList
	Public mTypeABCList As TypeABCList
	Public mAltTypeList As AltTypeList
	Dim Type As Integer
	Dim EventLogID As Guid 'Added By Utkarsh On 19-Jul-2011 For All19072011
	Dim mATAList As ATAList 'Added By Vikrant on 11-Oct-2012 For ALL10102012
	'Added By Utkarsh FOR opening Stock On 20-Feb-2013 FOR All20022013-3
	Dim openingBalanceDetails As String = String.Empty
	Dim mOpeningBalanceCollection As Collections.Hashtable
	'End
	'IDs for combobox
	Public NomenclatureID As Guid
	Public UnitID As Guid
	Public CategoryID As Guid
	Public ATAID As Guid
	Public PartTypeID As Integer
	Public ABCTypeID As Integer
	Public UnitName As String
	'End
	Public RequisitionItemID As Guid 'Added By Vikrant On 07-Oct-2014 For ALL07102014
	Public mCalibrationPeriodInList As CalibrationPeriodInList
	Public CalibrationPeriodInID As Integer
	Public mItemTagList As ItemTagList
	Public ItemTagID As Integer
	Public ConditionCheckIntervalInID As Integer
	Public ToolTypeID As Integer
	Public mToolTypeList As ToolTypeList
	Public mManufacturerList As ManufacturerList
	Public ManufacturerID As Guid
	Public HSNACSID As Guid
	Public mHSNACSList As HSNACSList
	Public ServicedInspectedIntervalInID As Integer
	Public mContractedVendorList As VendorList
	Public ContractedVendorID As Guid
	Dim mServiceInspectionsList As New ItemServiceInspectionsList
	Public EssentiaCategoryID As Integer
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mItem = Session("mItem")
		mUnitList = Session("mUnitList")
		mNomenclatureList = Session("mNomenclatureList")
		mCategoryList = Session("mCategoryList")
		mTypeABCList = Session("mTypeABCList")
		mAltTypeList = Session("mAltTypeList")
		Type = Session("Type")
		mATAList = Session("mATAList") 'Added By Vikrant on 11-Oct-2012 For ALL10102012

		NomenclatureID = Session("NomenclatureID")
		UnitID = Session("UnitID")
		CategoryID = Session("CategoryID")
		ATAID = Session("ATAID")
		PartTypeID = Session("PartTypeID")
		ABCTypeID = Session("ABCTypeID")
		UnitName = Session("UnitName")
		RequisitionItemID = (CType(Session("RequisitionItemID"), Guid)) 'Added By Vikrant On 07-Oct-2014 For ALL07102014
		mCalibrationPeriodInList = Session("mCalibrationPeriodInList")
		CalibrationPeriodInID = Session("CalibrationPeriodInID")
		mItemTagList = Session("mItemTagList")
		ItemTagID = Session("ItemTagID")
		ConditionCheckIntervalInID = Session("ConditionCheckIntervalInID")
		ToolTypeID = Session("ToolTypeID")
		mManufacturerList = Session("mManufacturerList")
		ManufacturerID = Session("ManufacturerID")
		HSNACSID = Session("HSNACSID")
		ServicedInspectedIntervalInID = Session("ServicedInspectedIntervalInID")
		mContractedVendorList = Session("mContractedVendorList")
		ContractedVendorID = Session("ContractedVendorID")
		EssentiaCategoryID = Session("EssentiaCategoryID")
	End Sub
	'unused code
	Private Sub SetSession()
		Session("mItem") = mItem
		Session("mUnitList") = mUnitList
		Session("mNomenclatureList") = mNomenclatureList
		Session("mCategoryList") = mCategoryList
		Session("mTypeABCList") = mTypeABCList
		Session("mAltTypeList") = mAltTypeList
		Session("mATAList") = mATAList 'Added By Vikrant on 11-Oct-2012 For ALL10102012
		Session("mCalibrationPeriodInList") = mCalibrationPeriodInList
		Session("mItemTagList") = mItemTagList
		Session("mManufacturerList") = mManufacturerList
		Session("mContractedVendorList") = mContractedVendorList
	End Sub
	Private Sub RemoveSession()

		Session.Remove("NomenclatureID")
		Session.Remove("UnitID")
		Session.Remove("CategoryID")
		Session.Remove("ATAID")
		Session.Remove("PartTypeID")
		Session.Remove("ABCTypeID")

		Session.Remove("mItem")
		Session.Remove("mUnitList")
		Session.Remove("mNomenclatureList")
		Session.Remove("mCategoryList")
		Session.Remove("mTypeABCList")
		Session.Remove("mAltTypeList")
		Session.Remove("mATAList")
		Session.Remove("UnitName")
		Session.Remove("mCalibrationPeriodInList")
		Session.Remove("CalibrationPeriodInID")
		Session.Remove("mItemTagList")
		Session.Remove("ItemTagID")
		Session.Remove("ConditionCheckIntervalInID")
		Session.Remove("ToolTypeID")
		Session.Remove("mManufacturerList")
		Session.Remove("ManufacturerID")
		Session.Remove("HSNACSID")
		Session.Remove("ServicedInspectedIntervalInID")
		Session.Remove("mContractedVendorList")
		Session.Remove("ContractedVendorID")
	End Sub
	Private Sub NewRecord()           'Added Code  May19,2007
		mItem = Item.NewItem
		Session("mItem") = mItem
		ClearControls()
		upnlValidations.Update()
	End Sub
	Private Sub NewKit()
		Dim mKit As Kit
		mKit = Kit.NewKit
		mKit.Type = 2
		mKit.ItemID = mItem.ID
		mKit.KitName = mItem.Name
		Session("mKit") = mKit
	End Sub
	Private Sub SetPage()
		If Not mItem.IsNew Then
			lblTitle.Text = "Part Information [" + mItem.Name + "]"
		Else
			lblTitle.Text = "Part Information [New]"
		End If
		upnlTitle.Update()
	End Sub
	Private Sub setObject(Optional ByVal IsForUnit As Boolean = False, Optional ByVal IsForCategory As Boolean = False, Optional ByVal IsForNomenclature As Boolean = False)
		SetIDOnPostBacks()
		mItem.Name = Trim(txtPartNo.Text)
		mItem.Description = Trim(txtDescription.Text)
		mItem.Location = Trim(txtLocation.Text)
		mItem.Rate = Val(txtApproxRate.Text)
		mItem.Folio = Val(txtFolio.Text)
		mItem.StatusKit = chkStatusKit.Checked
		mItem.ExpiryMonths = Val(txtExpiryMonths.Text)
		mItem.ExpiryQuaters = Val(txtExpiryQuaters.Text)
		mItem.BenchmarkMonths = Val(txtBenchmarkMonths.Text)
		mItem.SerialisedStatus = chkSerialisedStatus.Checked
		mItem.StockStatus = chkStockStatus.Checked
		mItem.ValuationStatus = chkValuationStatus.Checked
		mItem.MinStockLevel = Val(txtMinStockLevel.Text)
		'mItem.MinReOrderLevel = Val(txtReOrderLevel.Text)
		mItem.Note = Trim(txtNote.Text)
		mItem.ABCID = ABCTypeID
		mItem.AltTypeID = PartTypeID
		If Not IsForNomenclature Then
			mItem.NomenclatureID = NomenclatureID
			mItem.NomenclatureName = mNomenclatureList(NomenclatureID).Name
		End If
		If Not IsForUnit Then
			mItem.UnitID = UnitID
			mItem.UnitName = UnitName
		End If
		If Not IsForCategory Then
			mItem.CategoryID = CategoryID
		End If
		mItem.StatusEquipment = chkStatusGroundEquipment.Checked
		mItem.Specification = txtSpecification.Text
		mItem.Make = txtMake.Text
		mItem.MaxNoOfUses = Val(txtMaxUses.Text)
		'Added by shweta on 20-Jul-2012 for All20072012-1
		If txtNotInUseDate.Text.Trim = String.Empty Then
			mItem.NotInUseDate = System.DBNull.Value
		Else
			mItem.NotInUseDate = CDate(txtNotInUseDate.Text.Trim)
		End If

		mItem.NotInUse = chkNotInUse1.Checked
		'---
		mItem.ATAID = ATAID
		mItem.MaxStockLevel = Val(txtMaxStockLevel.Text)    'Added By Prashant 25-Feb-2013 All25022013 
		mItem.IPCReference = txtIPCReference.Text.Trim      'Added By Prashant 25-Jul-2013 All25072014
		mItem.StorageLife = Val(txtStorageLife.Text)        'Added By Prashant 27-Oct-2014 ALL27102014-3
		mItem.IsOneTimePurchase = chkIsOneTimePurchase.Checked ''Added By Prashant 11-Nov-2014 ALL11112014
		'Commented and added by vikrant on 23-Nov-2016 For BA21112016
		'mItem.IsConsiderForReOrder = chkIsConsiderForReOrder.Checked 'Added By Prashant 27-Oct-2014 ALL27102014-3
		If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
			mItem.IsConsiderForReOrder = IIf(mItem.IsOneTimePurchase, False, True)
		Else
			mItem.IsConsiderForReOrder = chkIsConsiderForReOrder.Checked
		End If
		'End
		mItem.BinCardNumber = txtBinCardNumber.Text
		'Added by Vikrant on 09-Feb-2015 For ALL09022015
		mItem.CalibrationStandard = Trim(txtCalibrationStandard.Text)
		mItem.AMMCMMReference = Trim(txtAMMCMMReference.Text)
		'End
		mItem.CalibrationPeriodInID = CalibrationPeriodInID
		mItem.ItemTagID = ItemTagID
		mItem.IsAirworthiCheck = chkAirworthiness.Checked   ''Added By Shital 07-Sep-2016
		'Added By Vikrant On 21-Nov-2016 For BA21112016
		If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
			Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
			If MaxMinQtyDiffForReOrder >= 0 Then
				txtReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
			End If
		End If
		mItem.MinReOrderLevel = Val(txtReOrderLevel.Text)
		mItem.IsConditionCheck = chkConditionCheck.Checked
		mItem.IsServicedInspected = chkServicedInspected.Checked
		If chkConditionCheck.Checked = True Then
			mItem.ConditionCheckInterval = Val(txtConditionCheckInterval.Text)
			mItem.ConditionCheckIntervalIn = ConditionCheckIntervalInID
		End If
		If chkServicedInspected.Checked = True Then
			mItem.ServicedInspectedInterval = Val(txtServicedInspected.Text)
			mItem.ServicedInspectedIntervalIn = ServicedInspectedIntervalInID
		End If

		mItem.ToolTypeID = ToolTypeID
		'End
		mItem.ManufacturerID = ManufacturerID
		mItem.HSNACSID = HSNACSID
		mItem.LifeComponent = ChkLifeComponent.Checked
		mItem.ContractedVendorID = ContractedVendorID
		mItem.ManuallyUpdated = chkManuallyUpdated.Checked
		mItem.IsExpiryItem = chkIsExpiryItem.Checked  'Added by Prashant On 10-Aug-2020 All10082020
		mItem.ReOrderQty = txtReOrderQty.Text   'Added by 05-mar-2021
		mItem.EssentialcategoryID = EssentiaCategoryID 'cmbEssentialCatagory.SelectedValue        'added by Shital on 20-Apr-2021

	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Close" And (Session("Type") <> 1 Or Session("PartInfo") <> "True") Then '' Close confirmation
						Session("sender") = ""
						Page.Validate("1")
						If Not CustomValidate1() Then
							Session.Remove("IsValid")
							Session("PartInfo") = "False"
							upnlValidations.Update()
							Exit Sub
						End If
						If Page.IsValid Then
							Save(ClosePageAfterSave:=True)
						Else
							Session.Remove("IsValid")
							Session("PartInfo") = "False"
							upnlValidations.Update()
						End If
					ElseIf (MSGBoxCtrl.Sender = "Close") And (Session("Type") = 1 Or Session("PartInfo") = "True") Then
						Session("sender") = ""
						Page.Validate("1")
						If mItem.IsValid And mItem.IsDirty And Page.IsValid Then
							Session.Remove("IsValid")
							Save(ClosePageAfterSave:=True)
						Else
							Session.Remove("IsValid")
							Session.Remove("mItem")
							Session("PartInfo") = "False"
							ClosePage()
						End If
					ElseIf MSGBoxCtrl.Sender = "DeleteCategory" Then
						GetSessionForCategory()
						Dim msgcount As Integer = 0
						Try
							Category.DeleteCategory(mCategory.ID)
							NewRecordForCategory()
							ClearControlsForCategory()
							DataBindOnCategoryPageLoad()
							controlVisibilityForCategory()
							SetPageForCategory()
						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
							ElseIf ex.Number = 547 Then
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
								MarkLog(Util.Action.Delete, "Category", "Can't delete : " & mCategory.Name & " is Currently in use", Util.ErrorType.NoError, mCategory.ID, EventLogID)
							End If
							NewRecordForCategory()
							ClearControlsForCategory()
							ComboBindForCategory()
							controlVisibilityForCategory()
							SetPageForCategory()
							pnlCategory.DataBind()
							msgcount = ex.Errors.Count
						Finally
							If msgcount = 0 Then
								MarkLog(Util.Action.Delete, "Category", mCategory.Name, Util.ErrorType.NoError, mCategory.ID, EventLogID)
							End If
						End Try
					ElseIf MSGBoxCtrl.Sender = "DeleteNomenclature" Then
						Dim msgCount As Integer = 0
						Try
							GetSessionForNomenclature()
							Session("sender") = ""
							NomenClature.DeleteNomenclature(mNomenclature.ID)
							NewRecordForNomenclature()
							DataFieldBindForNomenclature(True)
							SetPageForNomenclature()
							upnlNomenDetails.Update()
							txtNomenName.DataBind()
						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
							ElseIf ex.Number = 547 Then
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
								MarkLog(Util.Action.Delete, "Nomenclature", "Can't delete : " & mNomenclature.Name & " is Currently in use", Util.ErrorType.NoError, mNomenclature.ID, EventLogID)
							End If
							msgCount = ex.Errors.Count
							NewRecordForNomenclature()
							SetPageForNomenclature()
							txtNomenName.DataBind()
							DataFieldBindForNomenclature()
							upnlNomenDetails.Update()
						Finally
							If msgCount = 0 Then
								MarkLog(Util.Action.Delete, "Nomenclature", mNomenclature.Name, Util.ErrorType.NoError, mNomenclature.ID, EventLogID)
							End If
						End Try
					ElseIf MSGBoxCtrl.Sender = "DeleteUnit" Then
						Dim msgCount As Integer = 0
						Try
							GetSessionForUnit()
							Session("sender") = ""
							Unit.DeleteUnit(mUnit.ID)
							NewRecordForUnit()
							DataFieldBindForUnit(True)
							SetPageForUnit()
							upnlUnitDetails.Update()
							txtUnitName.DataBind()
						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
							ElseIf ex.Number = 547 Then
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
								MarkLog(Util.Action.Delete, "Unit", "Can't delete : " & mUnit.Name & " is Currently in use", Util.ErrorType.NoError, mUnit.ID, EventLogID)
							End If
							msgCount = ex.Errors.Count
							NewRecordForUnit()
							SetPageForUnit()
							upnlUnitDetails.Update()
							txtUnitName.DataBind()
							DataFieldBindForUnit()
						Finally
							If msgCount = 0 Then
								MarkLog(Util.Action.Delete, "Unit", mUnit.Name, Util.ErrorType.NoError, mUnit.ID, EventLogID)
							End If
						End Try
					ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then
						Try
							Session("Sender") = ""
							mItem = CType(Session("mItem"), Item)
							mItem.FileAttachments.Remove(mItem.FileAttachments.CurrentItem)
							dgItemAttachment.DataSource = mItem.FileAttachments
							dgItemAttachment.DataBind()
							upnldgItemAttachment.Update()
							upnlItemAttachment.Update()
							Session("mItem") = mItem
						Catch ex As SqlException

						End Try
					ElseIf MSGBoxCtrl.Sender = "SaveConfirmation" Then 'Added by Shital on 03-Aug-2021
						Try
							Save()
						Catch ex As Exception

						End Try

					End If
				Case MsgBoxResult.No
					If (MSGBoxCtrl.Sender = "Close") And (Session("Type") <> 1 Or Session("PartInfo") <> "True") Then
						Session.Remove("IsValid")
						Session("Sender") = ""
						ClosePage()
					ElseIf (MSGBoxCtrl.Sender = "Close") And (Session("Type") = 1 Or Session("PartInfo") = "True") Then
						'Session.Remove("mItem")
						Session("sender") = ""
						Session("PartInfo") = "False"
						ClosePage()
					ElseIf MSGBoxCtrl.Sender = "DeleteCategory" Then
						GetSessionForCategory()
						NewRecordForCategory()
						ClearControlsForCategory()
						ComboBindForCategory()
						controlVisibilityForCategory()
						SetPageForCategory()
						pnlCategory.DataBind()
					ElseIf MSGBoxCtrl.Sender = "DeleteNomenclature" Then
						GetSessionForNomenclature()
						NewRecordForNomenclature()
						SetPageForNomenclature()
						txtNomenName.DataBind()
						upnlNomenDetails.Update()
					ElseIf MSGBoxCtrl.Sender = "DeleteUnit" Then
						GetSessionForUnit()
						NewRecordForUnit()
						SetPageForUnit()
						upnlUnitDetails.Update()
						txtUnitName.DataBind()
					Else
						Session("sender") = ""
						Session.Remove("IsValid")
						Session("PartInfo") = "False"
						ClosePage()
					End If
				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					Session("sender") = ""
					LoadComboBox()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 Then   'Code Added
			Session("sender") = ""
			'DataFieldBind()
		End If
	End Sub
	Private Sub addattributes1()
		txtBenchmarkMonths.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtBenchmarkMonths.ClientID + "').value,event)")
		txtExpiryMonths.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtExpiryMonths.ClientID + "').value,event)")
		txtExpiryQuaters.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtExpiryQuaters.ClientID + "').value,event)")
		txtMaxUses.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtMaxUses.ClientID + "').value,event)")
		txtMaxStockLevel.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtMaxStockLevel.ClientID + "').value,event)")  'Added By Prashant 25-Feb-2013 All25022013
		txtMinStockLevel.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtMinStockLevel.ClientID + "').value,event)")  'Added By Prashant 25-Feb-2013 All25022013
		txtReOrderLevel.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtReOrderLevel.ClientID + "').value,event)")  'Added By Prashant 25-Feb-2013 All25022013
		txtStorageLife.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtStorageLife.ClientID + "').value,event)")
		txtConditionCheckInterval.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtConditionCheckInterval.ClientID + "').value,event)")
	End Sub
	Private Function Save(Optional ByVal CreatNewRecordAfterSave As Boolean = False, Optional ByVal ClosePageAfterSave As Boolean = False) As Boolean
		If mItem.IsDirty Then mItem.IsSync = 0 'Added by Saylee on 3-June-2010 for Symco bridge


		Dim ItemClone As Item
		ItemClone = mItem.Clone

		Try
			mItem.ApplyEdit()

			CreateMarkLogActions() 'Added By Utkarsh FOR opening Stock On 20-Feb-2013 FOR All20022013-3
			Dim mOldCategory As String = " Old Category:- " & mItem.CategoryName
			mItem.Save()
			Session("PartNo") = mItem.Name
			'Changed By Utkarsh On 19-Jul-2011 For All19072011
			MarkLog(Util.Action.Save, "Part", mItem.Name + mOldCategory + " New Category:- " + Item.GetItem(mItem.ID).CategoryName, Util.ErrorType.NoError, mItem.ID, EventLogID)
			'End

			MarkLogOpeningStock() 'Added By Utkarsh FOR opening Stock On 20-Feb-2013 FOR All20022013-3
			Session.Remove("mOpeningBalanceCollection")
			If ClosePageAfterSave Then
				ClosePage()
			End If
			If CreatNewRecordAfterSave Then
				NewRecord()
			End If
			SetPage()
			If mItem.ItemApplicables.Count > 0 Then
				'If Part saved then alternate part added and applicability also added then item save, then to add applicability to alternate part this is added
				' This is simultaneous addition of records case
				mItem.AddItemApplicability(mItem.ID, mItem.LinkID) 'Added By Prashant om 6-Aug-2021 BA06082021.  
			End If
			ControlVisibilityForActionButtons()
			ControlVisibilityForExpCalibration()
			ControlVisibilityForTabs()
			ControlVisibilityForNotInUse()
			LoadComboBox()
			ControlVisibilityForGenDetails()
			ItemClone = Nothing
			Return True
		Catch ex As SqlException
			If ex.Number = 8145 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
			ElseIf ex.Number = 2627 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
			ElseIf ex.Number = 547 Then
				If InStr(ex.Message, "FK_tabIssueItem_tabReceiptItem*200*", CompareMethod.Text) Then
					MSGBoxCtrl.show(MSGBox.Message_title.OpeningStockDeleteConfirm, MSGBox.Message_text.OpeningStockDeleteConfirm, "", MsgBoxStyle.OkOnly, "")
				ElseIf InStr(ex.Message, "CCtabReceiptItemStockBalanceQty'", CompareMethod.Text) Then
					MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Opening Balance Qty can not be less than Issue Qty.", MsgBoxStyle.OkOnly, "")
				ElseIf InStr(ex.Message, "FKtabIssueItemtabReceiptItem", CompareMethod.Text) Then
					MSGBoxCtrl.show(MSGBox.Message_title.OpeningStockDeleteConfirm, MSGBox.Message_text.OpeningStockDeleteConfirm, "", MsgBoxStyle.OkOnly, "")
				ElseIf InStr(ex.Message, "*16-TB02-CX10*", CompareMethod.Text) Then
					MSGBoxCtrl.show(MSGBox.Message_title.OpeningStockUpdateConfirm, MSGBox.Message_text.OpeningStockUpdateConfirm, "", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Message.IndexOf("CCtabReceiptItemStockBalanceQty") > 0 Then
					MSGBoxCtrl.show(MSGBox.Message_title.OpeningStockUpdateConfirm, MSGBox.Message_text.OpeningStockUpdateConfirm, "Opening stock Qty can not be less than Issue Qty.", MsgBoxStyle.OkOnly, "")
				Else
					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If
			Else
				MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
			End If

			'If mItem.IsDirty Then
			'    mItem = ItemClone
			'    Session("mItem") = mItem
			'End If
			ItemClone = Nothing
			Return False
		End Try
	End Function
	Private Sub AttachMyFile()
		'Try
		'    mItem.AttachFile = CType(Session("FileUpload.FileContent"), Byte())
		'    mItem.Size = Session("FileUpload.FileSize")
		'    mItem.FileExtension = Session("FileUpload.FileExtension")
		'    Session("mItem") = mItem
		'    Session.Remove("FileUpload.FileSize")
		'    Session.Remove("FileUpload.FileContent")
		'    Session.Remove("FileUpload.FileExtension")
		'    ControlVisibilityForExpCalibration()
		'Catch ex As Exception
		'    MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
		'End Try
		Try
			If Not mItem.FileAttachments.Contains(mItem.ID, CType(Session("FileUpload.FileName"), String)) Then

				mItem.FileAttachments.Add(mItem.ID, CType(Session("FileUpload.FileName"), String))
				mItem.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
				mItem.FileAttachments.CurrentItem.Size = Session("Size")
				mItem.FileAttachments.CurrentItem.Extension = Session("Extension")
				Session("mItem") = mItem
				dgItemAttachment.DataSource = mItem.FileAttachments
				dgItemAttachment.DataBind()

				For i As Integer = 0 To mItem.FileAttachments.Count - 1
					Dim txtValue As TextBox
					txtValue = CType(Me.dgItemAttachment.Rows(i).FindControl("txtFileName"), TextBox)
					txtValue.Text = mItem.FileAttachments(i).FileName
				Next

				Session.Remove("Size")
				Session.Remove("ImageFile")
				Session.Remove("Extension")
				Session.Remove("FileUpload.FileName")
				upnlItemAttachment.Update()
				upnldgItemAttachment.Update()
			Else
				Session("mItem") = mItem
				MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		Catch ex As Exception
		End Try
	End Sub
	Private Sub ControlVisibilityForGenDetails()
		imgbtnNomenclature.Visible = IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusList_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusListForEnquiry_Ajax.aspx"), False, True)
		imgbtnCategory.Visible = IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusList_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusListForEnquiry_Ajax.aspx"), False, True)
		imgbtnUnit.Visible = IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusList_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusListForEnquiry_Ajax.aspx"), False, True)
		imgbtnKit.Visible = IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusList_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusListForEnquiry_Ajax.aspx"), False, True)

		'Added by Saylee on 3-Oct-2013 for All03102013 (only addition of "Not mItem.IsNew" code)
		chkStatusKit.Enabled = IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusList_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusListForEnquiry_Ajax.aspx") Or mItem.KitCountOfItem > 0, False, IIf(mItem.IsNew, False, True))
		imgbtnKit.Enabled = IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusList_Ajax.aspx" Or Request.QueryString("BackPage") = "wfPartStockStatusListForEnquiry_Ajax.aspx"), False, IIf(mItem.IsNew, False, IIf(chkStatusKit.Checked, True, False)))
		'End
		If AppSettings("ClientCode") = "BA" Then
			Span15.Visible = True
			cmbEssentialCatagory.Visible = True
		End If
		upnlDetails.Update()
		upnlAdditionalInformation.Update()
		upnlExpBencCal.Update()
	End Sub
	Private Sub ControlVisibilityForTabs()
		'Added by Saylee on 3-Oct-2013 for All03102013 (only addition of "Not mItem.IsNew" code)
		btnAlternatePart.Enabled = IIf(mItem.IsNew, False, True) '(IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx"), False, IIf(mItem.IsNew, False, True)))
		btnApplicability.Enabled = IIf(mItem.IsNew, False, True) '(IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx"), False, IIf(mItem.IsNew, False, True)))
		btnOpeningStock.Enabled = IIf(mItem.IsNew, False, True) '(IIf((Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx"), False, IIf(mItem.IsNew, False, True)))
		btnOpeningStock.Visible = IIf(Session("PartInfo") = "True", False, True) 'Added by Prashant 22-Aug-2018 ALL22082018
		upnlTabs.Update()
	End Sub
	Private Sub ControlVisibilityForExpCalibration()
		If mItem.SerialisedStatus = True Then
			chkSerialisedStatus.Enabled = False
		End If
		'Commented and added by vikrant on 23-Nov-2016 For BA21112016
		'txtReOrderLevel.Enabled = IIf(mItem.IsConsiderForReOrder = True, True, False)
		txtReOrderLevel.Enabled = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", False, IIf(mItem.IsConsiderForReOrder = True, True, False))
		'End
		upnlExpBencCal.Update()
	End Sub
	Private Sub ControlVisibilityForActionButtons()
		If Session("PartInfo") = "True" Then
			btnSaveNew.Enabled = False
			btnSaveNew.Visible = False
			'btnSave.Enabled = False 'Commented by Prashant 22-Aug-2018  ALL22082018
		End If
		If mItem.IsNew = False Then
			If mCategoryList(mItem.CategoryName).PrimaryCategoryID = 2 Then
				ClpnlExpiryBenchcheckCalibrationInformation.Attributes.Add("style", "display:block;")
			Else
				ClpnlExpiryBenchcheckCalibrationInformation.Attributes.Add("style", "display:none;")
			End If
		End If
		upnlExpBencCal.Update()
		upnlActionBtn.Update()
	End Sub
	Private Sub ControlVisibilityForNotInUse()
		txtNotInUseDate.Enabled = chkNotInUse1.Checked
		'upnlNotInUse.Update()
	End Sub
	'changed By Utkarsh FOR opening Stock On 26-Mar-2013 FOR All26032013
	Private Sub CreateMarkLogActions()
		mOpeningBalanceCollection = IIf(Session("mOpeningBalanceCollection") Is Nothing, New Collections.Hashtable, Session("mOpeningBalanceCollection"))
		Dim OpeningBalance As ReceiptInvoice
		Dim i As Integer = 0
		For i = 0 To mItem.OpeningBalances.Count - 1
			OpeningBalance = mItem.OpeningBalances(i)
			If OpeningBalance.IsDirty Then
				openingBalanceDetails = "Part No : " & mItem.Name & ", Description : " & mItem.Description & ", Receipt Date : " & OpeningBalance.InvoiceDateFormatted & ", Receipt No. : " & OpeningBalance.FullInvoiceNo &
				", Quantity : " & OpeningBalance.Qty & ", Release Note No. : " & OpeningBalance.ReleaseNoteNo & ", Store : " & OpeningBalance.StoreName

				If mOpeningBalanceCollection.ContainsKey("save") Then
					mOpeningBalanceCollection.Item("save") = mOpeningBalanceCollection.Item("save") & Environment.NewLine & openingBalanceDetails
				Else
					mOpeningBalanceCollection.Add("save", openingBalanceDetails)
				End If
			End If
		Next
	End Sub
	Private Sub MarkLogOpeningStock()
		If mOpeningBalanceCollection.Count > 0 Then
			Dim i As Integer = 0
			Dim MarkLogSave() As String
			If mOpeningBalanceCollection.ContainsKey("save") Then
				MarkLogSave = mOpeningBalanceCollection.Item("save").ToString.Split(Environment.NewLine)
			End If
			Dim MarkLogDelete() As String
			If mOpeningBalanceCollection.ContainsKey("delete") Then
				MarkLogDelete = mOpeningBalanceCollection.Item("delete").ToString.Split(Environment.NewLine)
			End If
			If Not MarkLogSave Is Nothing Then
				For i = 0 To MarkLogSave.Length - 1
					MarkLog(Util.Action.Save, "Opening Stock", MarkLogSave(i).Trim, Util.ErrorType.NoError, mItem.ID, EventLogID)
				Next
			End If
			If Not MarkLogDelete Is Nothing Then
				For i = 0 To MarkLogDelete.Length - 1
					MarkLog(Util.Action.Delete, "Opening Stock", MarkLogDelete(i).Trim, Util.ErrorType.NoError, mItem.ID, EventLogID)
				Next
			End If
		End If
	End Sub
	'End
	Private Sub SetIDOnLoad()
		NomenclatureID = mItem.NomenclatureID
		Session("NomenclatureID") = NomenclatureID
		'If IsNothing(Session("UnitID")) Then
		'    UnitID = Guid.Empty
		'    Session("UnitID") = UnitID
		'Else
		'    UnitID = Session("UnitID")
		'End If
		UnitID = mItem.UnitID
		Session("UnitID") = UnitID
		UnitName = mItem.UnitName
		Session("UnitName") = UnitName
		CategoryID = mItem.CategoryID
		Session("CategoryID") = CategoryID
		ATAID = mItem.ATAID
		Session("ATAID") = ATAID
		PartTypeID = mItem.AltTypeID
		Session("PartTypeID") = PartTypeID
		ABCTypeID = mItem.ABCID
		Session("ABCTypeID") = ABCTypeID
		CalibrationPeriodInID = mItem.CalibrationPeriodInID
		Session("CalibrationPeriodInID") = CalibrationPeriodInID
		ItemTagID = mItem.ItemTagID
		Session("ItemTagID") = ItemTagID
		ConditionCheckIntervalInID = mItem.ConditionCheckIntervalIn
		Session("ConditionCheckIntervalInID") = ConditionCheckIntervalInID
		ToolTypeID = mItem.ToolTypeID
		Session("ToolTypeID") = ToolTypeID
		ManufacturerID = mItem.ManufacturerID
		Session("ManufacturerID") = ManufacturerID
		HSNACSID = mItem.HSNACSID
		Session("HSNACSID") = HSNACSID
		ServicedInspectedIntervalInID = mItem.ServicedInspectedIntervalIn
		Session("ServicedInspectedIntervalInID") = ServicedInspectedIntervalInID
		ContractedVendorID = mItem.ContractedVendorID
		Session("ContractedVendorID") = ContractedVendorID
		'Added By Shitalon 23-Apr-2021
		EssentiaCategoryID = mItem.EssentialcategoryID
		Session("EssentiaCategoryID") = EssentiaCategoryID
		'---
	End Sub
	Private Sub SetIDOnPostBacks()
		If NomenclatureValue.Value = String.Empty Then
			NomenclatureID = Session("NomenclatureID")
		Else
			NomenclatureID = New Guid(NomenclatureValue.Value)
		End If
		Session("NomenclatureID") = NomenclatureID

		If UnitValue.Value = String.Empty Then
			UnitID = Session("UnitID")
		Else
			UnitID = New Guid(UnitValue.Value)
		End If
		Session("UnitID") = UnitID

		If UnitNameValue.Value = String.Empty Then
			UnitName = Session("UnitName")
		Else
			UnitName = UnitNameValue.Value
		End If
		Session("UnitName") = UnitName

		If CategoryValue.Value = String.Empty Then
			CategoryID = Session("CategoryID")
		Else
			CategoryID = New Guid(CategoryValue.Value)
		End If
		Session("CategoryID") = CategoryID

		If ATAValue.Value = String.Empty Then
			ATAID = Session("ATAID")
		Else
			ATAID = New Guid(ATAValue.Value)
		End If
		Session("ATAID") = ATAID

		If PartTypeValue.Value = String.Empty Then
			PartTypeID = Session("PartTypeID")
		Else
			PartTypeID = CInt(PartTypeValue.Value)
		End If
		Session("PartTypeID") = PartTypeID

		If ABCTypeValue.Value = String.Empty Then
			ABCTypeID = Session("ABCTypeID")
		Else
			ABCTypeID = CInt(ABCTypeValue.Value)
		End If
		Session("ABCTypeID") = ABCTypeID

		If hdnCalibrationPeriodIn.Value = String.Empty Then
			CalibrationPeriodInID = Session("CalibrationPeriodInID")
		Else
			CalibrationPeriodInID = CInt(hdnCalibrationPeriodIn.Value)
		End If
		Session("CalibrationPeriodInID") = CalibrationPeriodInID

		'ItemTagID
		If hdnItemTag.Value = String.Empty Then
			ItemTagID = Session("ItemTagID")
		Else
			ItemTagID = CInt(hdnItemTag.Value)
		End If
		Session("ItemTagID") = ItemTagID

		If hdnConditionCheckIntervalIn.Value = String.Empty Then
			ConditionCheckIntervalInID = Session("ConditionCheckIntervalInID")
		Else
			ConditionCheckIntervalInID = CInt(hdnConditionCheckIntervalIn.Value)
		End If
		Session("ConditionCheckIntervalInID") = ConditionCheckIntervalInID

		If hdnServicedInspected.Value = String.Empty Then
			ServicedInspectedIntervalInID = Session("ServicedInspectedIntervalInID")
		Else
			ServicedInspectedIntervalInID = CInt(hdnServicedInspected.Value)
		End If
		Session("ServicedInspectedIntervalInID") = ServicedInspectedIntervalInID

		If hdnToolType.Value = String.Empty Then
			ToolTypeID = Session("ToolTypeID")
		Else
			ToolTypeID = CInt(hdnToolType.Value)
		End If
		Session("ToolTypeID") = ToolTypeID
		If ManufacturerValue.Value = String.Empty Then
			ManufacturerID = Session("ManufacturerID")
		Else
			ManufacturerID = New Guid(ManufacturerValue.Value)
		End If
		Session("ManufacturerID") = ManufacturerID
		If HSNACSValue.Value = String.Empty Then
			HSNACSID = Session("HSNACSID")
		Else
			HSNACSID = New Guid(HSNACSValue.Value)
		End If
		Session("HSNACSID") = HSNACSID
		If hdnContractedVendor.Value = String.Empty Then
			ContractedVendorID = Session("ContractedVendorID")
		Else
			ContractedVendorID = New Guid(hdnContractedVendor.Value)
		End If
		Session("ContractedVendorID") = ContractedVendorID
		'Added by Shital on 23-Apr-2021

	End Sub
	Private Sub SetSelectedValuesForCombo()
		cmbUnit.SelectedValue = UnitID.ToString
		cmbCategory.SelectedValue = CategoryID.ToString
		cmbNomenclature.SelectedValue = NomenclatureID.ToString
		cmbABCType.SelectedValue = ABCTypeID.ToString
		cmbAltType.SelectedValue = PartTypeID.ToString
		cmbATAList.SelectedValue = ATAID.ToString
		cmbCalibrationPeriodIn.SelectedValue = CalibrationPeriodInID.ToString
		cmbItemTag.SelectedValue = ItemTagID.ToString
		cmbConditionCheckIntervalIn.SelectedValue = ConditionCheckIntervalInID.ToString
		cmbToolType.SelectedValue = ToolTypeID.ToString
		cmbManufacturerList.SelectedValue = ManufacturerID.ToString
		cmbHSNACSList.SelectedValue = HSNACSID.ToString
		cmbServicedInspectedInterval.SelectedValue = ServicedInspectedIntervalInID.ToString
		cmbContractedVendor.SelectedValue = ContractedVendorID.ToString
		cmbEssentialCatagory.SelectedValue = EssentiaCategoryID     'Added by Shital on 23-Apr-2021
	End Sub
	Private Sub LoadComboBox()
		cmbUnit.DataSource = mUnitList
		cmbCategory.DataSource = mCategoryList
		cmbNomenclature.DataSource = mNomenclatureList
		cmbABCType.DataSource = mTypeABCList
		cmbAltType.DataSource = mAltTypeList
		cmbATAList.DataSource = mATAList
		cmbCalibrationPeriodIn.DataSource = mCalibrationPeriodInList
		cmbItemTag.DataSource = mItemTagList
		If IsDBNull(mItem.NotInUseDate) Then
			txtNotInUseDate.Text = ""
		Else
			txtNotInUseDate.Text = mItem.NotInUseDateFormatted
		End If
		cmbConditionCheckIntervalIn.DataSource = mCalibrationPeriodInList
		cmbToolType.DataSource = mToolTypeList
		cmbManufacturerList.DataSource = mManufacturerList
		cmbHSNACSList.DataSource = mHSNACSList
		cmbServicedInspectedInterval.DataSource = mCalibrationPeriodInList
		cmbContractedVendor.DataSource = mContractedVendorList
		pnlPartInformation.DataBind()
		cmbContractedVendor.DataBind()
		SetSelectedValuesForCombo()
	End Sub
	Private Sub ClearControls()
		NomenclatureValue.Value = ""
		UnitValue.Value = ""
		UnitNameValue.Value = ""
		CategoryValue.Value = ""
		ABCTypeValue.Value = ""
		'PartTypeValue.Value = ""
		ATAValue.Value = ""
		hdnCalibrationPeriodIn.Value = ""
		hdnItemTag.Value = ""

		NomenclatureID = Guid.Empty
		Session("NomenclatureID") = NomenclatureID
		UnitID = Guid.Empty
		Session("UnitID") = UnitID
		UnitName = String.Empty
		Session("UnitName") = UnitName
		CategoryID = Guid.Empty
		Session("CategoryID") = CategoryID
		ATAID = Guid.Empty
		Session("ATAID") = ATAID
		ABCTypeID = 0
		Session("ABCTypeID") = ABCTypeID
		'PartTypeID = 0
		'Session("PartTypeID") = PartTypeID
		CalibrationPeriodInID = 0
		Session("CalibrationPeriodInID") = CalibrationPeriodInID
		ItemTagID = 0
		Session("ItemTagID") = ItemTagID
		ConditionCheckIntervalInID = 0
		Session("ConditionCheckIntervalInID") = ConditionCheckIntervalInID
		ToolTypeID = 0
		Session("ToolTypeID") = ToolTypeID
		ManufacturerID = Guid.Empty
		Session("ManufacturerID") = ManufacturerID
		HSNACSID = Guid.Empty
		Session("HSNACSID") = HSNACSID
		ServicedInspectedIntervalInID = 0
		Session("ServicedInspectedIntervalInID") = ServicedInspectedIntervalInID
		ContractedVendorID = Guid.Empty
		Session("ContractedVendorID") = ContractedVendorID
		'Added by Shitalon 23-Apr-2021
		EssentiaCategoryID = 0
		Session("EssentiaCategoryID") = EssentiaCategoryID
	End Sub
	Private Sub ClosePage()
		RemoveSession()
		If Session("Type") = 1 Or Session("PartInfo") = "True" Then
			Session("PartInfo") = "False"
			Session("AddSingleParts") = "NewCreated"
			'Response.Redirect("wfStoreRequisition.aspx?Type=1")
			If Request.QueryString("BackPage") = "wfRequisitionItemSearch_Ajax.aspx" Or
				Request.QueryString("BackPage") = "wfRequisitionItemListForIssue_Ajax.aspx" Or
				Request.QueryString("BackPage") = "wfRequisitionPartListForPurchaseOrder_Ajax.aspx" Or
				Request.QueryString("BackPage") = "wfAlternatePartListForOrder_Ajax.aspx" Or
				Request.QueryString("BackPage") = "wfPartStockStatusList_Ajax.aspx" Or
				Request.QueryString("BackPage") = "wfPartStockStatusListForEnquiry_Ajax.aspx" Or
				Request.QueryString("BackPage") = "wfCommonPartList_Ajax.aspx" Then 'Added By Vikrant For New Requisition
				'Added By Vikrant On 07-Oct-2014 For ALL07102014
				If Request.QueryString("BackPage") = "wfRequisitionItemListForIssue_Ajax.aspx" Or Request.QueryString("BackPage") = "wfRequisitionPartListForPurchaseOrder_Ajax.aspx" Or Request.QueryString("BackPage") = "wfAlternatePartListForOrder_Ajax.aspx" Then
					If Not mItem.IsNew Then
						RequisitionItemNew.UpdateItemIDForRequsition(RequisitionItemID, mItem.ID)
					End If
				End If
				'End
				Dim URL As Stack = CType(Session("URL"), Stack)
				Response.Redirect(URL.Peek.ToString)
			ElseIf Request.QueryString("BackPage") Is Nothing Then 'Added by Prashant 22-Aug-2018  ALL22082018
				If Not mItem.IsNew Then
					RequisitionItemNew.UpdateItemIDForRequsition(RequisitionItemID, mItem.ID)
				End If
				Dim URL As Stack = CType(Session("URL"), Stack)
				Response.Redirect(URL.Peek.ToString) 'End
			Else
				Response.Redirect("wfStoreRequisitionItem.aspx?Type=1&BackPage=wfStoreRequisition.aspx")
			End If
		Else
			'--Added By Utkarsh On 07-Feb-2011
			If Request.QueryString("BackPage") = "wfnPendingWOListForRemoveComp_Ajax.aspx" Then
				Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage1"))
			Else
				Response.Redirect("index.aspx")
			End If
			'Response.Redirect("index.aspx")
			'--------------------------------
		End If
	End Sub
	Private Sub DeleteAttachment(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
		mItem.FileAttachments.CurrentIndex = Index
		Session("mItem") = mItem
	End Sub
	Private Sub DisableName() 'Added by : Saylee 17-Jun-2020, ALL16062020
		If Not mItem.IsNew Then
			Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerItem(mItem.ID)
			If Not mTransCountAsPerMasters Is Nothing Then
				txtPartNo.Enabled = mTransCountAsPerMasters.Count = 0
			End If
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind(Optional ByVal GetUnitList As Boolean = False, Optional ByVal GetcategoryList As Boolean = False,
							  Optional ByVal GetNomenList As Boolean = False, Optional ByVal GetATAList As Boolean = False,
							  Optional ByVal GetABCList As Boolean = False, Optional ByVal GetPartTypeList As Boolean = False,
							  Optional ByVal GetCalibrationPeriodInList As Boolean = False, Optional ByVal GetItemTagList As Boolean = False,
							  Optional ByVal GetToolTypeList As Boolean = False, Optional ByVal GetManufacturerList As Boolean = False,
							  Optional ByVal GetHSNACSList As Boolean = False, Optional ByVal GetContractedVendorList As Boolean = False)
		If GetUnitList Then
			mUnitList = UnitList.GetUnitList(True)
			Session("mUnitList") = mUnitList
		End If
		cmbUnit.DataSource = mUnitList
		'Added Code   Unit
		'If Not mUnitList.Contains(mItem.UnitName) Then
		'    mItem.UnitID = Guid.Empty
		'End If
		'End of Added Code
		If GetcategoryList Then
			mCategoryList = CategoryList.GetCategoryList(True)
			Session("mCategoryList") = mCategoryList
		End If
		cmbCategory.DataSource = mCategoryList

		'Added Code       CategoryList
		'If Not mCategoryList.Contains(mItem.CategoryName) Then
		'    mItem.CategoryID = Guid.Empty
		'End If
		'End of Added Code
		If GetNomenList Then
			mNomenclatureList = NomenclatureList.GetNomenclatureList(True)
			Session("mNomenclatureList") = mNomenclatureList
		End If

		cmbNomenclature.DataSource = mNomenclatureList
		'Added Code             Nomenclature
		'If Not mNomenclatureList.Contains(mItem.NomenclatureName) Then
		'    mItem.NomenclatureID = Guid.Empty
		'End If
		'End of Code
		If GetABCList Then
			mTypeABCList = TypeABCList.GetTypeABCList()
			Session("mTypeABCList") = mTypeABCList
		End If
		cmbABCType.DataSource = mTypeABCList

		If GetPartTypeList Then
			mAltTypeList = AltTypeList.GetAltTypeList()
			Session("mAltTypeList") = mAltTypeList
		End If

		cmbAltType.DataSource = mAltTypeList

		'Added By Vikrant on 11-Oct-2012 For ALL10102012

		If GetATAList Then
			mATAList = ATAList.GetATAList("", "(SELECT)")
			Session("mATAList") = mATAList
		End If

		cmbATAList.DataSource = mATAList

		'End

		If GetCalibrationPeriodInList Then
			mCalibrationPeriodInList = CalibrationPeriodInList.GetCalibrationPeriodInList("(SELECT)")
			Session("mCalibrationPeriodInList") = mCalibrationPeriodInList
		End If
		cmbCalibrationPeriodIn.DataSource = mCalibrationPeriodInList

		cmbConditionCheckIntervalIn.DataSource = mCalibrationPeriodInList

		cmbServicedInspectedInterval.DataSource = mCalibrationPeriodInList

		If GetItemTagList Then
			mItemTagList = ItemTagList.GetItemTagList(True)
			Session("mItemTagList") = mItemTagList
		End If
		cmbItemTag.DataSource = mItemTagList

		If GetToolTypeList Then
			mToolTypeList = ToolTypeList.GetToolTypeList(True, "(SELECT)")
			Session("mToolTypeList") = mToolTypeList
		End If
		cmbToolType.DataSource = mToolTypeList

		If GetManufacturerList Then
			mManufacturerList = ManufacturerList.GetManufacturerList("", "(SELECT)")
			Session("mManufacturerList") = mManufacturerList
		End If

		cmbManufacturerList.DataSource = mManufacturerList

		If GetHSNACSList Then
			mHSNACSList = HSNACSList.GetHSNACSList("", "", "(SELECT)")
			Session("mHSNACSList") = mHSNACSList
		End If

		cmbHSNACSList.DataSource = mHSNACSList

		If GetContractedVendorList Then
			mContractedVendorList = VendorList.GetVendortList(0, , , , , , True, True, True, True)
			Session("mContractedVendorList") = mContractedVendorList
		End If

		cmbContractedVendor.DataSource = mContractedVendorList

		If IsDBNull(mItem.NotInUseDate) Then
			txtNotInUseDate.Text = ""
		Else
			txtNotInUseDate.Text = mItem.NotInUseDateFormatted
		End If

		dgItemAttachment.DataSource = mItem.FileAttachments

		cmbEssentialCatagory.SelectedValue = EssentiaCategoryID     'Added by Shital on 23-Apr-2021

		pnlPartInformation.DataBind()
		SetSelectedValuesForCombo()

	End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		'If custValidator.ControlToValidate = "cmbNomenclature" Then   Commented by Shweta
		If custValidator.ControlToValidate = "txtApproxRate" Then
			If Val(txtApproxRate.Text) < 0 Then
				custValidator.ErrorMessage = "Approximate Rate can't be negative."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtFolio" Then
			If Val(txtFolio.Text) < 0 Then
				custValidator.ErrorMessage = "Folio No. can't be negative."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtDescription" Then
			If Len(txtDescription.Text.Trim) > 100 Then
				custValidator.ErrorMessage = "Part description must not be greater than 100 characters."
				e.IsValid = False
			End If


		ElseIf custValidator.ControlToValidate = "txtNote" Then
			If Len(txtNote.Text) > 500 Then
				' txtNote.Text = txtNote.Text.Substring(0, 96) + "..."
				custValidator.ErrorMessage = "Note field length must not be greater than 500 characters."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtNotInUseDate" Then
			If Len(txtNotInUseDate.Text.Trim) = 0 Then
				' txtNote.Text = txtNote.Text.Substring(0, 96) + "..."
				custValidator.ErrorMessage = "Enter Not In Use Date."
				e.IsValid = False
			End If
			'Added By Vikrant On 21-Nov-2016 For BA21112016
		ElseIf custValidator.ControlToValidate = "txtMinStockLevel" Then
			If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
				If Not chkIsOneTimePurchase.Checked Then
					If CDec(Val(txtMaxStockLevel.Text)) <= 0 Then
						custValidator.ErrorMessage = "Either mark Item as One Time Purchase or enter Max Stock Level quantity."
						e.IsValid = False
					ElseIf (CDec(Val(txtMaxStockLevel.Text)) > 0) Then
						If CDec(Val(txtMaxStockLevel.Text)) - CDec(Val(txtMinStockLevel.Text)) < 0 Then
							custValidator.ErrorMessage = "Max Stock Level quantity should be greater than Min Stock Level quantity."
							e.IsValid = False
						End If
					End If
				End If
			End If
			'End
		ElseIf custValidator.ControlToValidate = "txtAMMCMMReference" Then
			If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS") And mItem.PrimaryCategoryID = 1 And mItem.ItemApplicables.Count = 0 Then
				custValidator.ErrorMessage = "As item is Rotable add applicability."
				e.IsValid = False
			End If
			'ElseIf custValidator.ControlToValidate = "chkServicedInspected" Then
			If chkServicedInspected.Checked And mItem.ItemServiceInspectionsList.Count = 0 Then
				custValidator.ErrorMessage = "service inspection required."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtIPCReference" Then  'Added By Prashant 21-Sep-2020 STR21092020-1
			If (Len(txtIPCReference.Text.Trim) = 0 And AppSettings("ClientCode") = "STR") Then
				custValidator.ErrorMessage = "IPC Reference Required."
				e.IsValid = False
			End If
		End If
	End Sub
	Private Function CustomValidate1() As Boolean
		Dim strMSG As String = ""
		If Not mItem.IsValid Then
			If Not mItem.IsValid Then
				For i As Integer = 0 To mItem.GetBrokenRulesCollection.Count - 1
					strMSG = strMSG + mItem.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

				For j As Integer = 0 To mItem.OpeningBalances.Count - 1
					If Not mItem.OpeningBalances(j).IsValid Then
						For i As Integer = 0 To mItem.OpeningBalances(j).GetBrokenRulesCollection.Count - 1
							strMSG = strMSG + mItem.OpeningBalances(j).GetBrokenRulesCollection(i).Description + "<Br>"
						Next
					End If
				Next

				For j As Integer = 0 To mItem.ItemApplicables.Count - 1
					If Not mItem.ItemApplicables(j).IsValid Then
						For i As Integer = 0 To mItem.ItemApplicables(j).GetBrokenRulesCollection.Count - 1
							strMSG = strMSG + mItem.ItemApplicables(j).GetBrokenRulesCollection(i).Description + "<Br>"
						Next
					End If
				Next

				For j As Integer = 0 To mItem.AlternatePartNos.Count - 1
					If Not mItem.AlternatePartNos(j).IsValid Then
						For i As Integer = 0 To mItem.AlternatePartNos(j).GetBrokenRulesCollection.Count - 1
							strMSG = strMSG + mItem.AlternatePartNos(j).GetBrokenRulesCollection(i).Description + "<Br>"
						Next
					End If
				Next
			End If
			If strMSG.Trim <> "" Then
				cvDescription.ErrorMessage = strMSG
				cvDescription.IsValid = False
				Return False
			End If
		End If
		Return True
	End Function
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		addattributes1()
		EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
		If Not IsPostBack And Session("sender") = "" Then
			If txtPartNo.Enabled = True Then
				setFocus(txtPartNo)
			End If
			Type = Request.QueryString("Type")
			Session("Type") = Type
			SetIDOnLoad()
			DataFieldBind(True, True, True, True, True, True, True, True, True, True, True, True)
			SetPage()
			ControlVisibilityForActionButtons()
			ControlVisibilityForExpCalibration()
			ControlVisibilityForGenDetails()
			ControlVisibilityForTabs()
			ControlVisibilityForNotInUse()
			DisableName() 'Added by : Saylee 17-Jun-2020, ALL16062020
			chkStatusKit.DataBind()
			imgbtnKit.DataBind()
		End If
	End Sub
	Private Sub imgbtnNomenclature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnNomenclature.Click
		'Commented by Amrita on 11-Dec-07 for solving Bug No.CT1
		'setObject() 'EC
		'------------
		setObject(IsForNomenclature:=True)
		DataFieldBind()
		ControlVisibilityForGenDetails()
		SetSession()
		OpenNomenclatureWindow()
	End Sub
	Private Sub imgbtnUnit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnUnit.Click
		'Commented by Amrita on 11-Dec-07 for solving Bug No.CT1
		''setObject() 'EC
		'-------------
		setObject(IsForUnit:=True)
		SetSession()
		DataFieldBind()
		ControlVisibilityForGenDetails()
		OpenUnitWindow()
	End Sub
	Private Sub imgbtnCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCategory.Click
		'Commented by Amrita on 11-Dec-07 for solving Bug No.CT1
		'setObject() 'EC
		'---------------
		setObject(IsForCategory:=True)
		DataFieldBind()
		ControlVisibilityForGenDetails()
		SetSession()
		OpenCategorywindow()
	End Sub
	Private Sub imgbtnKit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnKit.Click
		If Not chkStatusKit.Checked Then Exit Sub
		setObject()
		SetSession()
		Dim mKit As Kit
		Dim mTempItem As Item
		mTempItem = Item.GetItem(mItem.ID)
		''Dim mKitList As KitList = KitList.GetKitList(2, "", mItem.Name)
		''And Not mKitList.Count = 0
		'If mTempItem.StatusKit = True Then
		'    mKit = Kit.Getkit(mItem.Name)
		'    Session("mKit") = mKit
		'Else
		'    NewKit()
		'End If

		Dim mKitList As KitList = KitList.GetKitList(2, mItem.Name, mItem.Name)
		If mKitList.Count = 1 Then          'Open for Edit
			mKit = Kit.Getkit(mItem.Name)
			Session("mKit") = mKit
		Else
			NewKit()   'Open for New
		End If
		DataFieldBind()
		ControlVisibilityForGenDetails()
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenKitWindow", "OpenKitWindow()", True)

		'Response.Redirect("wfKit.aspx?BackPage=wfPartInformation.aspx")
	End Sub
	'This Code is for Save and New
	Private Sub btnSaveNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveNew.Click
		If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
			setObject()
			SetSession()
			'Changed By Utkarsh On 19-Jul-2011 For All19072011
			MarkLog(Util.Action.Save, "Part", User.Identity.Name & " is not Authorized User to Save " & mItem.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			'End
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If IsValid Then
			setObject()
			If Not CustomValidate1() Then upnlValidations.Update() : Exit Sub
			'Save(True)
			'If condition Added by Shital on 03-Aug-2021
			If AppSettings("ClientCode") = "BA" Then
				If mItem.AlternatePartNos.Count > 0 Then
					Dim ExtraMessage As String = ""
					ExtraMessage = "Essential catagory of Alternate part also set as " + cmbEssentialCatagory.SelectedItem.ToString + " do you want to Save?"
					MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveConfirmation")
				Else
					Save(True)
				End If
			Else
				Save(True)
			End If
		Else
			upnlValidations.Update()
		End If
	End Sub
	'This Code is for Save and Close
	Private Sub btnSaveClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveClose.Click
		If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
			setObject()
			SetSession()
			'Changed By Utkarsh On 19-Jul-2011 For All19072011
			MarkLog(Util.Action.Save, "Part", User.Identity.Name & " is not Authorized User to Save " & mItem.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			'End
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If IsValid Then
			setObject()

			If Not CustomValidate1() Then upnlValidations.Update() : Exit Sub
			'Save(ClosePageAfterSave:=True)
			'If condition Added by Shital on 03-Aug-2021
			If AppSettings("ClientCode") = "BA" Then
				If mItem.AlternatePartNos.Count > 0 Then
					Dim ExtraMessage As String = ""
					ExtraMessage = "Essential catagory of Alternate part also set as " + cmbEssentialCatagory.SelectedItem.ToString + " do you want to Save?"
					MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveConfirmation")
				Else
					Save(ClosePageAfterSave:=True)
				End If
			Else
				Save(ClosePageAfterSave:=True)
			End If
		Else
			upnlValidations.Update()
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
		'Changed By Utkarsh On 19-Jul-2011 For All19072011
		MarkLog(Util.Action.Close, "Part", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		'End
		setObject()
		If mItem.IsDirty Then
			If (User.IsInRole("PartNew") And mItem.IsNew) Or (User.IsInRole("PartEdit") And Not mItem.IsNew) Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
				MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
				Session("IsValid") = mItem.IsValid
				Exit Sub
			Else
				ClosePage()
			End If
		Else
			ClosePage()
		End If
	End Sub
	Private Sub chkStatusGroundEquipment_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStatusGroundEquipment.CheckedChanged
		Dim mReceiptItemList As ReceiptItemList
		mReceiptItemList = ReceiptItemList.GetReceiptItemList(mItem.ID)
		If mItem.SerialisedStatus = False And mReceiptItemList.Count > 0 Then

			MSGBoxCtrl.show("Alert!", "Receipts are already available.", "Kindly delete receipt to make it ground equipment", MsgBoxStyle.OkOnly, "")
			ControlVisibilityForExpCalibration()
			chkStatusGroundEquipment.Checked = False
			mReceiptItemList = Nothing
			Exit Sub

		End If
		If chkStatusGroundEquipment.Checked = True Then
			ControlVisibilityForExpCalibration()
			chkSerialisedStatus.Checked = True
		End If

		'txtConditionCheckInterval.Text = 0
		'mItem.ConditionCheckInterval = 0
		'cmbConditionCheckIntervalIn.SelectedValue = 0
		'mItem.ConditionCheckIntervalIn = 0
		'hdnConditionCheckIntervalIn.Value = String.Empty
		'ConditionCheckIntervalInID = 0
		'Session("ConditionCheckIntervalInID") = ConditionCheckIntervalInID

		'txtServicedInspected.Text = 0
		'mItem.ServicedInspectedInterval = 0
		'cmbServicedInspectedInterval.SelectedValue = 0
		'mItem.ServicedInspectedIntervalIn = 0
		'hdnServicedInspected.Value = String.Empty
		'ServicedInspectedIntervalInID = 0
		'Session("ServicedInspectedIntervalInID") = ServicedInspectedIntervalInID

		upnlDetails.Update()
	End Sub
	Private Sub chkServicedInspected_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkServicedInspected.CheckedChanged
		Dim mReceiptItemList As ReceiptItemList
		mReceiptItemList = ReceiptItemList.GetReceiptItemList(mItem.ID)
		If mItem.SerialisedStatus = False And mReceiptItemList.Count > 0 Then

			MSGBoxCtrl.show("Alert!", "Receipts are already available.", "Kindly delete receipt to make it ground equipment", MsgBoxStyle.OkOnly, "")
			ControlVisibilityForExpCalibration()
			chkServicedInspected.Checked = False
			mReceiptItemList = Nothing
			Exit Sub

		End If
		If chkServicedInspected.Checked = True Then
			ControlVisibilityForExpCalibration()
			chkSerialisedStatus.Checked = True
			imgbtnServiceInspections.Visible = True
		Else
			imgbtnServiceInspections.Visible = False
		End If

		'txtBenchmarkMonths.Text = 0
		'mItem.BenchmarkMonths = 0
		'cmbCalibrationPeriodIn.SelectedValue = 0
		'mItem.CalibrationPeriodInID = 0
		'hdnCalibrationPeriodIn.Value = String.Empty
		'CalibrationPeriodInID = 0
		'Session("CalibrationPeriodInID") = CalibrationPeriodInID

		'txtConditionCheckInterval.Text = 0
		'mItem.ConditionCheckInterval = 0
		'cmbConditionCheckIntervalIn.SelectedValue = 0
		'mItem.ConditionCheckIntervalIn = 0
		'hdnConditionCheckIntervalIn.Value = String.Empty
		'ConditionCheckIntervalInID = 0
		'Session("ConditionCheckIntervalInID") = ConditionCheckIntervalInID

		upnlDetails.Update()
	End Sub
	Private Sub chkConditionCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConditionCheck.CheckedChanged
		Dim mReceiptItemList As ReceiptItemList
		mReceiptItemList = ReceiptItemList.GetReceiptItemList(mItem.ID)
		If mItem.SerialisedStatus = False And mReceiptItemList.Count > 0 Then

			MSGBoxCtrl.show("Alert!", "Receipts are already available.", "Kindly delete receipt to make it ground equipment", MsgBoxStyle.OkOnly, "")
			ControlVisibilityForExpCalibration()
			chkConditionCheck.Checked = False
			mReceiptItemList = Nothing
			Exit Sub

		End If
		If chkConditionCheck.Checked = True Then
			ControlVisibilityForExpCalibration()
			chkSerialisedStatus.Checked = True
		End If

		'txtBenchmarkMonths.Text = 0
		'mItem.BenchmarkMonths = 0
		'cmbCalibrationPeriodIn.SelectedValue = 0
		'mItem.CalibrationPeriodInID = 0
		'hdnCalibrationPeriodIn.Value = String.Empty
		'CalibrationPeriodInID = 0
		'Session("CalibrationPeriodInID") = CalibrationPeriodInID

		'txtServicedInspected.Text = 0
		'mItem.ServicedInspectedInterval = 0
		'cmbServicedInspectedInterval.SelectedValue = 0
		'mItem.ServicedInspectedIntervalIn = 0
		'hdnServicedInspected.Value = String.Empty
		'ServicedInspectedIntervalInID = 0
		'Session("ServicedInspectedIntervalInID") = ServicedInspectedIntervalInID

		upnlDetails.Update()
	End Sub
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
			setObject()
			SetSession()
			'Changed By Utkarsh On 19-Jul-2011 For All19072011
			MarkLog(Util.Action.Save, "Part", User.Identity.Name & " is not Authorized User to Save " & mItem.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			'End
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
			Exit Sub
		End If
		If IsValid Then
			setObject()
			If Not CustomValidate1() Then upnlValidations.Update() : Exit Sub
			'If condition Added by Shital on 03-Aug-2021
			If AppSettings("ClientCode") = "BA" Then
				If mItem.AlternatePartNos.Count > 0 Then
					Dim ExtraMessage As String = ""
					ExtraMessage = "Essential catagory of Alternate part also set as " + cmbEssentialCatagory.SelectedItem.ToString + " do you want to Save?"
					MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveConfirmation")
				Else
					Save()
				End If
			Else
				Save()
			End If
			' Save() Commented by Shital on 03-Aug-2021

			dgItemAttachment.DataSource = mItem.FileAttachments
			dgItemAttachment.DataBind()
		Else
			upnlValidations.Update()
		End If
	End Sub
	Private Sub txtExpiryQuaters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpiryQuaters.TextChanged
		If Val(txtExpiryQuaters.Text) <> 0 Then
			mItem.ExpiryQuaters = Val(txtExpiryQuaters.Text)
			txtExpiryQuaters.DataBind()
			txtExpiryMonths.DataBind()
			Session("mItem") = mItem
		Else
			txtExpiryQuaters.Text = "0"
		End If
		ControlVisibilityForExpCalibration()
	End Sub
	Private Sub txtExpiryMonths_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpiryMonths.TextChanged
		If Val(txtExpiryMonths.Text) <> 0 Then
			mItem.ExpiryMonths = Val(txtExpiryMonths.Text)
			txtExpiryQuaters.DataBind()
			txtExpiryMonths.DataBind()
			Session("mItem") = mItem
		End If
		ControlVisibilityForExpCalibration()
	End Sub
	Private Sub chkNotInUse1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotInUse1.CheckedChanged
		If chkNotInUse1.Checked = False Then
			txtNotInUseDate.Text = ""
		End If
		ControlVisibilityForNotInUse()
	End Sub
	Private Sub imgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgBtnATAChapter.Click

		'Response.Redirect("wfATA.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage3=wfPartInformation.aspx")
	End Sub
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	'Private Sub txtNotInUseDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtNotInUseDate.TextChanged
	'    If IsValid Then
	'        If IsDate(txtNotInUseDate.Text.Trim) Then
	'            If txtNotInUseDate.Text.Trim = String.Empty Then
	'                mItem.NotInUseDate = System.DBNull.Value
	'            Else
	'                mItem.NotInUseDate = txtNotInUseDate.Text.Trim
	'            End If
	'            Session("mItem") = mItem
	'        Else
	'            txtNotInUseDate.Text = ""
	'        End If
	'    End If
	'End Sub
	Private Sub hdnimgBtnATAChapter_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
		setObject()
		Session("mItem") = mItem
		DataFieldBind(GetATAList:=True)
		ControlVisibilityForGenDetails()
	End Sub
	Private Sub hdnimgbtnKit_Click(sender As Object, e As System.EventArgs) Handles hdnimgbtnKit.Click
		setObject()
		Session("mItem") = mItem
		DataFieldBind()
		ControlVisibilityForGenDetails()
		upnlExpBencCal.Update()
	End Sub
	Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
		'setObject()
		'Session("mItem") = mItem
		AttachMyFile()
		upnlItemAttachment.Update()
	End Sub
	Protected Sub chkIsConsiderForReOrder_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsConsiderForReOrder.CheckedChanged
		If chkIsConsiderForReOrder.Checked = True Then
			txtReOrderLevel.Enabled = True
		Else
			txtReOrderLevel.Enabled = False
			txtReOrderLevel.Text = 0
		End If
	End Sub
	'Added By Vikrant On 21-Nov-2016 For BA21112016
	Private Sub txtMaxStockLevel_TextChanged(sender As Object, e As System.EventArgs) Handles txtMaxStockLevel.TextChanged
		If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
			Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
			If MaxMinQtyDiffForReOrder >= 0 Then
				txtReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
			End If
		End If
	End Sub
	Private Sub txtMinStockLevel_TextChanged(sender As Object, e As System.EventArgs) Handles txtMinStockLevel.TextChanged
		If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
			Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
			If MaxMinQtyDiffForReOrder >= 0 Then
				txtReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
			End If
		End If
	End Sub
	'End
	Private Sub hdnimgBtnManufacturerChapter_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnManufacturerChapter.Click
		setObject()
		Session("mItem") = mItem
		DataFieldBind(GetManufacturerList:=True)
		ControlVisibilityForGenDetails()
	End Sub
	Private Sub btnSelectFiles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
		setObject()
		Session("mItem") = mItem
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
	End Sub
	Private Sub dgItemAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItemAttachment.RowCommand
		Dim mFileAttachments As FileAttachments
		Select Case e.CommandName
			Case "View"
				Dim Index As Integer = CInt(e.CommandArgument)
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				mFileAttachments = mItem.FileAttachments
				If mFileAttachments.Count = 1 Then
					mFileAttachments.CurrentIndex = 0
				Else
					mFileAttachments.CurrentIndex = Index - 1
				End If

				If mFileAttachments.CurrentItem.Size > 0 Then
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
				dgItemAttachment.DataSource = mItem.FileAttachments
				dgItemAttachment.DataBind()
				upnlItemAttachment.Update()
				upnldgItemAttachment.Update()
			Case "Remove"
				Dim Index As Integer = CInt(e.CommandArgument) + dgItemAttachment.PageSize * dgItemAttachment.PageIndex
				mFileAttachments = mItem.FileAttachments
				If mFileAttachments.Count = 1 Then
					DeleteAttachment(0)
				Else
					DeleteAttachment(Index - 1)
				End If
		End Select
	End Sub

	Private Sub cmbEssentialCatagory_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEssentialCatagory.SelectedIndexChanged
		EssentiaCategoryID = cmbEssentialCatagory.SelectedValue
		Session("EssentiaCategoryID") = EssentiaCategoryID
	End Sub

#End Region

#Region " Navigation "

	'Alternate Part
	Private Sub btnAlternatePart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlternatePart.Click
		setObject()
		Session("mItem") = mItem
		Response.Redirect("wfAlternatePartChild_Ajax.aspx?BackPage=wfPartInformation_Ajax.aspx?Type=" & Session("Type"))
	End Sub

	'Applicability
	Private Sub btnApplicability_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplicability.Click
		Try
			setObject()
			mItem.ItemApplicables.Add(mItem.ID)
			'mItem.ItemApplicables.CurrentIndex = mItem.ItemApplicables.Count - 1
			mItem.ItemApplicables.CurrentItem.SrNo = mItem.ItemApplicables.Count
			mItem.ItemApplicables.CurrentItem.ModelName = ""
			For i As Integer = 0 To mItem.ItemApplicables.Count - 1
				mItem.ItemApplicables(i).SrNo = i + 1
			Next
			Session("mItem") = mItem

			'Response.Redirect("wfApplicableFor.aspx?BackPage=wfPartInformation.aspx")
			Response.Redirect("wfApplicableFor_Ajax.aspx?BackPage=wfPartInformation_Ajax.aspx?Type=" & Session("Type"))
		Catch ex As Exception
			MSGBoxCtrl.show("Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
		End Try
	End Sub

	'Opening Stock
	Private Sub btnOpeningStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpeningStock.Click
		setObject()
		mItem.PrimaryCategoryID = Category.GetCategory(mItem.CategoryID).PrimaryCategoryID
		Session("mItem") = mItem
		'Response.Redirect("wfOpeningBalanceList.aspx?BackPage=wfPartInformation.aspx")
		Response.Redirect("wfOpeningBalanceList_Ajax.aspx?BackPage=wfPartInformation_Ajax.aspx?Type=" & Session("Type"))
	End Sub

#End Region

#Region "Category"

	Public mCategory As Category
	Public mPrimaryCategoryList As PrimaryCategoryList
	Public mCategoryListForGrid As CategoryList
	Public primaryCategoryID As Integer

#Region "Business Methods"
	Private Sub GetSessionForCategory()
		mCategory = Session("mCategory")
		mPrimaryCategoryList = Session("mPrimaryCategoryList")
		mCategoryListForGrid = Session("mCategoryListForGrid")
		primaryCategoryID = Session("primaryCategoryID")
	End Sub
	Private Sub RemoveSessionForCategory()
		Session.Remove("mCategory")
		Session.Remove("mPrimaryCategoryList")
		Session.Remove("mCategoryListForGrid")
		Session.Remove("primaryCategoryID")
	End Sub
	Private Sub OpenCategorywindow()
		NewRecordForCategory()
		setPrimaryCategoryID()
		DataBindOnCategoryPageLoad()
		controlVisibilityForCategory()
		If txtName.Enabled = True Then
			setFocus(txtName)
		End If
		upnlCategoryValidations.Update()
		SetPageForCategory()
		mdlPopUpCategory.Show()
	End Sub
	Private Sub NewRecordForCategory()
		mCategory = Category.NewCategory
		Session("mCategory") = mCategory
	End Sub
	Private Sub DataBindOnCategoryPageLoad()
		mCategoryListForGrid = CategoryList.GetCategoryList
		'gdvCategory.DataSource = mCategoryListForGrid
		Session("mCategoryListForGrid") = mCategoryListForGrid
		GridBindForCategory()
		'upnlCategoryGrid.Update()

		mPrimaryCategoryList = PrimaryCategoryList.GetPrimaryCategoryList("(SELECT)")
		'cmbPrimaryCategory.DataSource = mPrimaryCategoryList
		Session("mPrimaryCategoryList") = mPrimaryCategoryList
		ComboBindForCategory()
		'lblResult.Text = "Category List: " & mCategoryListForGrid.Count & " Record(s) Found."
		pnlCategory.DataBind()
	End Sub
	Private Sub GridBindForCategory()
		gdvCategory.DataSource = mCategoryListForGrid
		gdvCategory.DataBind()
		lblResult.Text = "Category List: " & mCategoryListForGrid.Count & " Record(s) Found."
		upnlCategoryGrid.Update()
	End Sub
	Private Sub ComboBindForCategory()
		cmbPrimaryCategory.DataSource = mPrimaryCategoryList
		cmbPrimaryCategory.DataBind()
		cmbPrimaryCategory.SelectedValue = primaryCategoryID
	End Sub
	Private Sub SetPageForCategory()
		If mCategory.IsNew Then
			lblTitleCategory.Text = "Category Information [New]"
		Else
			If Len(mCategory.Name) > 15 Then
				lblTitleCategory.Text = "Category Information [" & mCategory.Name.Substring(0, 15) & "...]"
			Else
				lblTitleCategory.Text = "Category Information [" & mCategory.Name & "]"
			End If
		End If
		upnlCategoryTitle.Update()
	End Sub
	Private Sub controlVisibilityForCategory()
		lblStarGLCode.Visible = AppSettings("ClientCode") = "Indamer"
		upnlCategoryDetails.Update()
	End Sub
	Private Sub setPrimaryCategoryID()
		If PrimaryCategoryValue.Value = String.Empty Then
			primaryCategoryID = mCategory.PrimaryCategoryID
		Else
			primaryCategoryID = CInt(PrimaryCategoryValue.Value)
		End If
		Session("primaryCategoryID") = primaryCategoryID
	End Sub
	Private Sub setObjectForCategory()
		setPrimaryCategoryID()
		mCategory.Name = txtName.Text.Trim
		mCategory.GLCode = txtGLCode.Text.Trim
		mCategory.PrimaryCategoryID = primaryCategoryID
		Session("mCategory") = mCategory
	End Sub
	Public Sub customvalidateForCategory(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtGLCode" Then
			If Len(txtGLCode.Text.Trim) = 0 And AppSettings("ClientCode") = "Indamer" Then
				custValidator.ErrorMessage = "GLCode required"
				e.IsValid = False
			End If
		End If
	End Sub
	Private Sub EditRecordForCategory(ByVal mId As Guid)
		mCategory = Category.GetCategory(mId)
		Session("mCategory") = mCategory
	End Sub
	Private Sub DeleteRecordForCategory(ByVal mId As Guid)
		MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteCategory")
		mCategory = Category.GetCategory(mId)
		Session("mCategory") = mCategory
	End Sub
	Private Sub ClearControlsForCategory()
		PrimaryCategoryValue.Value = String.Empty
		setPrimaryCategoryID()
	End Sub
	Private Sub DisableCategoryName(ByVal mId As Guid) 'Added by : Saylee 17-Jun-2020, ALL16062020
		Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerCategory(mId)
		If Not mTransCountAsPerMasters Is Nothing Then
			txtName.Enabled = mTransCountAsPerMasters.Count = 0
		End If
	End Sub
#End Region

#Region "Events"
	Protected Sub btnCategoryClose_Click(sender As Object, e As EventArgs) Handles btnCategoryClose.Click
		MarkLog(Util.Action.Close, "Category", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		mCategory = Nothing
		mPrimaryCategoryList = Nothing
		mCategoryListForGrid = Nothing
		PrimaryCategoryValue.Value = String.Empty
		RemoveSessionForCategory()
		Session("sender") = ""
		mdlPopUpCategory.Hide()
		DataFieldBind(GetcategoryList:=True)
		ControlVisibilityForGenDetails()
	End Sub
	Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
		GetSessionForCategory()
		If txtName.Enabled = True Then
			setFocus(txtName)
		End If
		NewRecordForCategory()
		ClearControlsForCategory()
		MarkLog(Util.Action.[New], "Category", "", Util.ErrorType.NoError, mCategory.ID, EventLogID)
		ComboBindForCategory()
		controlVisibilityForCategory()
		SetPageForCategory()
		pnlCategory.DataBind()
	End Sub
	Private Sub gdvCategory_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvCategory.RowCommand
		GetSessionForCategory()
		Select Case e.CommandName
			Case "EditCategory"
				Dim mid As Guid = mCategoryListForGrid(CInt(e.CommandArgument)).ID
				If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
					setObjectForCategory()
					MarkLog(Util.Action.Edit, "Category", User.Identity.Name & "is not authorized user to edit " & mCategory.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				EditRecordForCategory(mid)
				ClearControlsForCategory()
				txtName.DataBind()
				txtGLCode.DataBind()
				ComboBindForCategory()
				controlVisibilityForCategory()
				MarkLog(Util.Action.Edit, "Category", mCategory.Name, Util.ErrorType.NoError, mCategory.ID, EventLogID)
				GridBindForCategory()
				SetPageForCategory()
				'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
			Case "DeleteCategory"
				Dim mid As Guid = mCategoryListForGrid(CInt(e.CommandArgument)).ID
				If (Not User.IsInRole("PartDelete")) Then
					setObjectForCategory()
					MarkLog(Util.Action.Delete, "Category", User.Identity.Name & "is not authorized user to delete " & mCategory.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				DeleteRecordForCategory(mid)
				GridBindForCategory()
		End Select
	End Sub
	Private Sub btnSaveCategory_Click(sender As Object, e As System.EventArgs) Handles btnSaveCategory.Click
		GetSessionForCategory()
		If (Not User.IsInRole("PartNew") And mCategory.IsNew) Or (Not User.IsInRole("PartEdit") And Not mCategory.IsNew) Then
			setObjectForCategory()
			MarkLog(Util.Action.Save, "Category", User.Identity.Name & "is not Authorized User to Save " & mCategory.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
			ComboBindForCategory()
			controlVisibilityForCategory()
			txtName.DataBind()
			txtGLCode.DataBind()
			Exit Sub
		End If
		If IsValid Then
			Try
				setObjectForCategory()
				mCategory.Save()
				MarkLog(Util.Action.Save, "Category", mCategory.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
				NewRecordForCategory()
				ClearControlsForCategory()
				DataBindOnCategoryPageLoad()
				controlVisibilityForCategory()
				SetPageForCategory()
			Catch ex As SqlException
				If ex.Number = 8145 Then
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
				End If
				ComboBindForCategory()
				controlVisibilityForCategory()
				txtName.DataBind()
				txtGLCode.DataBind()
			End Try
		Else
			upnlCategoryValidations.Update()
			setPrimaryCategoryID()
			ComboBindForCategory()
			controlVisibilityForCategory()
		End If
	End Sub
#End Region
#End Region

#Region "Nomenclature"
#Region "variable Declaration"
	Public mNomenclature As NomenClature
	Public mNomenclatureListForGrid As NomenclatureList
#End Region

#Region "business methods"
	Private Sub GetSessionForNomenclature()
		mNomenclature = Session("mNomenclature")
		mNomenclatureListForGrid = Session("mNomenclatureListForGrid")
	End Sub
	Private Sub RemoveSessionForNomenclature()
		Session.Remove("mNomenclature")
		Session.Remove("mNomenclatureListForGrid")
	End Sub
	Private Sub NewRecordForNomenclature()
		mNomenclature = NomenClature.NewNomenClature()
		Session("mNomenclature") = mNomenclature
	End Sub
	Private Sub EditRecordForNomenclature(ByVal mId As Guid)
		mNomenclature = NomenClature.GetNomenclature(mId)
		Session("mNomenclature") = mNomenclature
	End Sub
	Private Sub DeleteRecordForNomenclature(ByVal mId As Guid)
		MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteNomenclature")
		mNomenclature = NomenClature.GetNomenclature(mId)
		Session("mNomenclature") = mNomenclature
	End Sub
	Private Sub setObjectForNomenclature()
		mNomenclature.Name = Trim(txtNomenName.Text)
		Session("mNomenclature") = mNomenclature
	End Sub
	Private Sub DataFieldBindForNomenclature(Optional ByVal FetchfromDatabase As Boolean = False)
		If FetchfromDatabase Then
			mNomenclatureListForGrid = NomenclatureList.GetNomenclatureList
		End If
		gdvNomenclature.DataSource = mNomenclatureListForGrid
		Session("mNomenclatureListForGrid") = mNomenclatureListForGrid
		lblNomenGridRecord.Text = "Nomenclature List: " & mNomenclatureListForGrid.Count & " Record(s) Found."
		gdvNomenclature.DataBind()
		upnlNomenGrid.Update()
	End Sub
	Private Sub SetPageForNomenclature()
		If mNomenclature.IsNew Then
			lblNomenTitle.Text = "Nomenclature [New]"
		Else
			If Len(mNomenclature.Name) > 15 Then
				lblNomenTitle.Text = "Nomenclature [" & mNomenclature.Name.Substring(0, 15) & "...]"
			Else
				lblNomenTitle.Text = "Nomenclature [" & mNomenclature.Name & "]"
			End If
		End If
		upnlNomenTitle.Update()
	End Sub
	Private Sub OpenNomenclatureWindow()
		NewRecordForNomenclature()
		DataFieldBindForNomenclature(True)
		SetPageForNomenclature()
		upnlNomenDetails.Update()
		If txtNomenName.Enabled Then
			setFocus(txtNomenName)
		End If
		txtNomenName.DataBind()
		mdlPopupNomenclature.Show()
		upnlNomenValidations.Update()
	End Sub
	Private Sub DisableNomenclatureName(ByVal mId As Guid) 'Added by : Saylee 17-Jun-2020, ALL16062020
		Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerNomenclature(mId)
		If Not mTransCountAsPerMasters Is Nothing Then
			txtNomenName.Enabled = mTransCountAsPerMasters.Count = 0
		End If
	End Sub
#End Region

#Region "Events"
	Protected Sub btnNomenNew_Click(sender As Object, e As EventArgs) Handles btnNomenNew.Click
		GetSessionForNomenclature()
		If txtNomenName.Enabled = True Then
			setFocus(txtNomenName)
		End If
		NewRecordForNomenclature()
		txtNomenName.DataBind()
		MarkLog(Util.Action.[New], "Nomenclature", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		upnlNomenDetails.Update()
		SetPageForNomenclature()
	End Sub
	Protected Sub btnNomenSave_Click(sender As Object, e As EventArgs) Handles btnNomenSave.Click
		GetSessionForNomenclature()
		If (Not User.IsInRole("PartNew") And mNomenclature.IsNew) Or (Not User.IsInRole("PartEdit") And Not mNomenclature.IsNew) Then
			setObjectForNomenclature()
			MarkLog(Util.Action.Save, "Nomencalture", User.Identity.Name & " is not Authorized User to save ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
			Exit Sub
		End If
		Try
			setObjectForNomenclature()
			mNomenclature.Save()
			MarkLog(Util.Action.Save, "Nomenclature", mNomenclature.Name, Util.ErrorType.HandledError, mNomenclature.ID, EventLogID)
			NewRecordForNomenclature()
			DataFieldBindForNomenclature(True)
			SetPageForNomenclature()
			txtNomenName.DataBind()
		Catch ex As SqlException
			If ex.Number = 8145 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
			ElseIf ex.Number = 2627 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
			ElseIf ex.Number = 547 Then
				MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
			End If
			txtNomenName.DataBind()
		End Try
	End Sub
	Protected Sub btnNomenClose_Click(sender As Object, e As EventArgs) Handles btnNomenClose.Click
		MarkLog(Util.Action.Close, "Nomenclature", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		Session("sender") = ""
		RemoveSessionForNomenclature()
		mNomenclature = Nothing
		mNomenclatureListForGrid = Nothing
		mdlPopupNomenclature.Hide()
		DataFieldBind(GetNomenList:=True)
		ControlVisibilityForGenDetails()
	End Sub

	Private Sub gdvNomenclature_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvNomenclature.PageIndexChanging
		GetSessionForNomenclature()
		gdvNomenclature.PageIndex = e.NewPageIndex
		DataFieldBindForNomenclature()
	End Sub
	Private Sub gdvNomenclature_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvNomenclature.RowCommand
		Select Case e.CommandName
			Case "EditNomen"
				GetSessionForNomenclature()
				Dim index As Integer = CInt(e.CommandArgument) + gdvNomenclature.PageIndex * gdvNomenclature.PageSize
				Dim mid As Guid = mNomenclatureListForGrid(index).ID
				Dim mName As String = mNomenclatureListForGrid(index).Name
				If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
					setObjectForNomenclature()
					MarkLog(Util.Action.Edit, "Nomenclature", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, mid, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
					Exit Sub
				End If
				EditRecordForNomenclature(mid)
				txtNomenName.DataBind()
				upnlNomenDetails.Update()

				MarkLog(Util.Action.Edit, "Nomenclature", mName, Util.ErrorType.NoError, mid, EventLogID)
				DataFieldBindForNomenclature()
				SetPageForNomenclature()
				DisableNomenclatureName(mid) 'Added by : Saylee 18-Jun-2020, ALL16062020
			Case "DeleteNomen"
				Dim index As Integer = CInt(e.CommandArgument) + gdvNomenclature.PageIndex * gdvNomenclature.PageSize
				GetSessionForNomenclature()
				Dim mid As Guid = mNomenclatureListForGrid(index).ID
				Dim mName As String = mNomenclatureListForGrid(index).Name
				If (Not User.IsInRole("PartDelete")) Then
					setObjectForNomenclature()
					MarkLog(Util.Action.Delete, "Nomenclature", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, mid, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
					Exit Sub
				End If
				DataFieldBindForNomenclature()
				DeleteRecordForNomenclature(mid)
		End Select
	End Sub
#End Region
#End Region

#Region "Unit"
	Public mUnit As Unit
	Public mUnitListForGrid As UnitList

#Region "Business Methods"
	Private Sub GetSessionForUnit()
		mUnit = Session("mUnit")
		mUnitListForGrid = Session("mUnitListForGrid")
	End Sub
	Private Sub RemoveSessionForUnit()
		Session.Remove("mUnitListForGrid")
		Session.Remove("mUnit")
		upnlUnitValidations.Update()
	End Sub
	Private Sub NewRecordForUnit()
		mUnit = Unit.NewUnit
		Session("mUnit") = mUnit
	End Sub
	Private Sub EditRecordForUnit(ByVal mId As Guid)
		mUnit = Unit.GetUnit(mId)
		Session("mUnit") = mUnit
	End Sub
	Private Sub setObjectForUnit()
		mUnit.Name = txtUnitName.Text.Trim
		Session("mUnit") = mUnit
	End Sub
	Private Sub DeleteRecordForUnit(ByVal mId As Guid)
		Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
		MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteUnit")
		mUnit = Unit.GetUnit(mId)
		Session("mUnit") = mUnit
	End Sub
	Private Sub DataFieldBindForUnit(Optional ByVal FetchfromDatabase As Boolean = False)
		If FetchfromDatabase Then
			mUnitListForGrid = UnitList.GetUnitList()
			Session("mUnitListForGrid") = mUnitListForGrid
		End If
		gdvUnit.DataSource = mUnitListForGrid
		lblUnitGridTitle.Text = "Unit List: " & mUnitListForGrid.Count & " Record(s) Found."
		gdvUnit.DataBind()
		upnlUnitGrid.Update()
		'DataBind()
	End Sub
	Private Sub SetPageForUnit()
		If mUnit.IsNew Then
			lblUnitTitle.Text = "Unit [NEW]"
		Else
			If Len(mUnit.Name) > 15 Then
				lblUnitTitle.Text = "Unit [" & mUnit.Name.Substring(0, 15) & "...]"
			Else
				lblUnitTitle.Text = "Unit [" & mUnit.Name & "]"
			End If
		End If
		upnlUnitTitle.Update()
	End Sub
	Private Sub OpenUnitWindow()
		NewRecordForUnit()
		DataFieldBindForUnit(True)
		SetPageForUnit()
		upnlUnitDetails.Update()
		txtUnitName.DataBind()
		mdlPopupUnit.Show()
		upnlUnitValidations.Update()
	End Sub
	Private Sub DisableUnitName(ByVal mId As Guid) 'Added by : Saylee 17-Jun-2020, ALL16062020
		Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerUnit(mId)
		If Not mTransCountAsPerMasters Is Nothing Then
			txtUnitName.Enabled = mTransCountAsPerMasters.Count = 0
		End If
	End Sub
#End Region

#Region "events"

	Private Sub gdvUnit_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvUnit.RowCommand
		Select Case e.CommandName
			Case "EditUnit"
				GetSessionForUnit()
				Dim mid As Guid = mUnitListForGrid(CInt(e.CommandArgument)).ID
				Dim mName As String = mUnitListForGrid(CInt(e.CommandArgument)).Name
				If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
					setObjectForNomenclature()
					MarkLog(Util.Action.Edit, "Unit", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, mid, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
					Exit Sub
				End If
				EditRecordForUnit(mid)
				MarkLog(Util.Action.Edit, "Unit", mUnit.Name, Util.ErrorType.NoError, mUnit.ID, EventLogID)
				txtUnitName.DataBind()
				upnlUnitDetails.Update()
				DataFieldBindForUnit()
				SetPageForUnit()
			Case "DeleteUnit"
				GetSessionForUnit()
				Dim mid As Guid = mUnitListForGrid(CInt(e.CommandArgument)).ID
				Dim mName As String = mUnitListForGrid(CInt(e.CommandArgument)).Name
				If (Not User.IsInRole("PartDelete")) Then
					setObjectForNomenclature()
					MarkLog(Util.Action.Delete, "Unit", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, mid, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
					Exit Sub
				End If
				DataFieldBindForUnit()
				DeleteRecordForUnit(mid)
		End Select
	End Sub
	Private Sub btnUnitNew_Click(sender As Object, e As System.EventArgs) Handles btnUnitNew.Click
		GetSessionForUnit()
		If txtUnitName.Enabled Then
			setFocus(txtUnitName)
		End If
		NewRecordForUnit()
		MarkLog(Util.Action.[New], "Unit", "", Util.ErrorType.NoError, mUnit.ID, EventLogID)
		txtUnitName.DataBind()
		SetPageForUnit()
		upnlUnitDetails.Update()
	End Sub
	Private Sub btnUnitClose_Click(sender As Object, e As System.EventArgs) Handles btnUnitClose.Click
		MarkLog(Util.Action.Close, "Unit", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		RemoveSessionForUnit()
		mUnit = Nothing
		mUnitListForGrid = Nothing
		mdlPopupUnit.Hide()
		DataFieldBind(GetUnitList:=True)
		ControlVisibilityForGenDetails()
	End Sub
	Private Sub btnUnitSave_Click(sender As Object, e As System.EventArgs) Handles btnUnitSave.Click
		GetSessionForUnit()
		If (Not User.IsInRole("PartNew") And mUnit.IsNew) Or (Not User.IsInRole("PartEdit") And Not mUnit.IsNew) Then
			setObjectForUnit()
			MarkLog(Util.Action.Save, "Unit", User.Identity.Name & " is not Authorized User to save ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
			Exit Sub
		End If
		Try
			setObjectForUnit()
			mUnit.Save()
			MarkLog(Util.Action.Save, "Unit", mUnit.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			NewRecordForUnit()
			txtUnitName.DataBind()
			DataFieldBindForUnit(True)
			SetPageForUnit()
		Catch ex As SqlException
			If ex.Number = 8145 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
			ElseIf ex.Number = 2627 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
			ElseIf ex.Number = 547 Then
				MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
			End If
			txtUnitName.DataBind()
		End Try
	End Sub

	Private Sub imgbtnServiceInspections_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnServiceInspections.Click
		If IsValid Then
			Try
				setObject()
				'  mItem.ItemServiceInspectionsList.Add(mItem.ID)
				Session("mItem") = mItem
				Session("mItemID") = mItem.ID
				mServiceInspectionsList = mItem.ItemServiceInspectionsList
				Session("mServiceInspectionsList") = mServiceInspectionsList
				ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddServiceInspections", "AddServiceInspections();", True)

			Catch ex As Exception
				MSGBoxCtrl.show("Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
			End Try

		End If
	End Sub

#End Region
#End Region

End Class
