

'AJAX CREATED By : Saylee
'Dated           : 30-Oct-2013


Imports System.Text


Public Class wfnWOJobSpare_AJAX
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Protected mnWO As nWO
	Public mnWOJob As nWOJob
	Public mItemList As ItemList
	Dim mIndex As Int32
	Public PartNo As String         'Added By Utkarsh 14-Dec-2010
	Public Description As String    'Added By Utkarsh 14-Dec-2010
	Public ItemID As String         'Added By Utkarsh 14-Dec-2010

	Public mItemListForCombo As ItemList
	Public mLastEffRate As LastEffRate
	Public PrimaryCategoryIDs As New StringBuilder
	Public mKit As Kit
#End Region

#Region " Enumeration "
	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
	End Enum
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mnWO = Session("mnWO")
		mnWOJob = Session("mnWOJob")
		mItemList = Session("mItemList")
		PartNo = Session("PartNo")              'Added By Utkarsh 14-Dec-2010
		Description = Session("Description")    'Added By Utkarsh 14-Dec-2010
		ItemID = Session("ItemID")              'Added By Utkarsh 14-Dec-2010
		mItemListForCombo = Session("mItemListForCombo")
		mKit = Session("mKit")
	End Sub
	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("mnWOJob") = mnWOJob
		Session("mItemList") = mItemList
		Session("PartNo") = PartNo              'Added By Utkarsh 14-Dec-2010
		Session("Description") = Description    'Added By Utkarsh 14-Dec-2010
		Session("ItemID") = ItemID              'Added By Utkarsh 14-Dec-2010
		Session("mItemListForCombo") = mItemListForCombo
	End Sub

	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean

		Dim IsInRoleString As String = ""

		If AppSettings("ShowNewWOFlow") = "True" Then

			If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then

				If mnWO.TransTypeID = Trans.WO145 Then

					IsInRoleString = "WOCreate"

				Else

					IsInRoleString = "CAMOWOCreate"

				End If

			ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then

				IsInRoleString = "WOPlanning"

			ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then

				IsInRoleString = "WOExecution"

			ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then

				IsInRoleString = "WOCompletion"

			ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then

				IsInRoleString = "WOQCApproval"

			ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then

				IsInRoleString = "WOCAMOUpdate"

			ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then

				IsInRoleString = "WOBilling"

			End If

		Else

			If mnWO.TransTypeID = Trans.WO145 Then

				If Session("MiddleFrame") = "wfnWOJobListToComplete_AJAX.aspx" Then

					IsInRoleString = "WOJobListToComplete"

				Else

					IsInRoleString = "WorkOrder"

				End If

			ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then

				IsInRoleString = "SpareAssemblyWO"

			ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then

				IsInRoleString = "SpareComponentWO"

			ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then

				IsInRoleString = "EngineeringOrder"

			Else

				IsInRoleString = "CAMOWO"

			End If

		End If

		Select Case CheckFor

			Case Rights.View

				Return User.IsInRole(IsInRoleString + "View")

			Case Rights.[New]

				Return User.IsInRole(IsInRoleString + "New")

			Case Rights.Edit

				Return User.IsInRole(IsInRoleString + "Edit")

			Case Rights.Save

				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))

			Case Rights.Delete

				Return User.IsInRole(IsInRoleString + "Delete")

			Case Rights.Print

				Return User.IsInRole(IsInRoleString + "Print")

		End Select

	End Function

	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub SetPage()
		lblWO.Text = mnWO.WONumber
		lblJobLabel.Text = mnWOJob.SrNo

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblWOLabel.Text = "E.O. #"
		Else
			lblWOLabel.Text = "W. O. #"
		End If
	End Sub
	Private Function setObject() As Boolean
		mnWOJob.WOJobSpares.CurrentItem.ItemID = New Guid(cmbItemList.SelectedValue.ToString)
		mnWOJob.WOJobSpares.CurrentItem.PartNo = Trim(cmbItemList.SelectedItem.Text)
		mnWOJob.WOJobSpares.CurrentItem.Description = Trim(txtSpareDesc.Text)
		mnWOJob.WOJobSpares.CurrentItem.RequiredQty = Val(txtReqQty.Text)
		mnWOJob.WOJobSpares.CurrentItem.IsForBilling = chkIsForBilling.Checked
		mnWOJob.WOJobDescription = Trim(txtJobDescription.Text)
		mnWOJob.WOJobSpares.CurrentItem.Remark = Trim(txtRemark.Text) 'Added By Vikrant On 04-Apr-2014 For ALL04042014
		mnWOJob.WOJobSpares.CurrentItem.EffRate = Val(txtEffRate.Text)
		mnWOJob.WOJobSpares.CurrentItem.EstimatedCost = Val(txtEstimatedCost.Text) 'Val(txtEffRate.Text) * Val(txtReqQty.Text)
		mnWOJob.WOJobSpares.CurrentItem.UnitName = mItemListForCombo.Item(mnWOJob.WOJobSpares.CurrentItem.ItemID).UnitName
	End Function
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					'----Added By Utkarsh 18-Dec-2010

					If MSGBoxCtrl.Sender = "Delete" Then
						Session("sender") = ""
						If mnWOJob.WOJobSpares.Item(mnWOJob.WOJobSpares.CurrentIndex).WOIssuedSparesCount > 0 Then
							DataFieldBind()
							MSGBoxCtrl.show("Alert!", "You cannot remove this record, as Issue against this part has been already done!", "", MsgBoxStyle.OkOnly, "")
							Exit Sub
						End If
						'------------------------------------
						Try
							mnWOJob = Session("mnWOJob")
							mnWOJob.WOJobSpares.Remove(mnWOJob.WOJobSpares.CurrentIndex)
							For i As Integer = 0 To mnWOJob.WOJobSpares.Count - 1
								mnWOJob.WOJobSpares(i).SrNo = i + 1
							Next
							Session("mnWOJob") = mnWOJob
							Session("mJobSpareEdit") = False
							DataFieldBind()
							GetSession()
							addAttributes()
							SetPage()
							ControlVisibility()
							txtSpareDesc.Text = ""
							txtReqQty.Text = "0"
							txtRemark.Text = ""
							upnlDesc.Update()
							upnlPart.Update()
							upnldgJobSpare.Update()
							If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobSpares.Count.ToString + "');", True)
						Catch ex As SqlException
						End Try
					End If
				Case MsgBoxResult.No
					Session("sender") = ""
					GetSession()
					addAttributes()
					SetPage()
					ControlVisibility()
					DataFieldBind()
					If MSGBoxCtrl.Sender = "Delete" Then Session.Remove("mJobSpareEdit")
					txtSpareDesc.Text = ""
					txtReqQty.Text = "0"
					txtRemark.Text = ""
					upnlDesc.Update()
					upnlPart.Update()
				Case MsgBoxResult.Ok
					Session("sender") = ""

					If Session("ToCleareList") = "ToCleareList" And Session("Duplicate") = "Duplicate" Then
						Session.Remove("Duplicate")
						Session.Remove("ToCleareList")
						cmbItemList.ClearSelection()
						txtSpareDesc.Text = ""
						txtReqQty.Text = "0"
						txtRemark.Text = ""
						chkIsForBilling.Checked = False
						chkIsForBilling.Checked = False
						upnlDesc.Update()
						upnlPart.Update()
						upnldgJobSpare.Update()
						upnlButtons.Update()
						Session.Remove("ItemID")
					End If
					GetSession()
					addAttributes()
					SetPage()
					ControlVisibility()
					DataFieldBind()
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"
					Session("sender") = ""
					GetSession()
					addAttributes()
					SetPage()
					ControlVisibility()
					DataFieldBind()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""

			If Session("ToCleareList") = "ToCleareList" And Session("Duplicate") = "Duplicate" Then
				Session.Remove("Duplicate")
				Session.Remove("ToCleareList")
				cmbItemList.ClearSelection()
				txtSpareDesc.Text = ""
				txtReqQty.Text = "0"
				txtRemark.Text = ""
				chkIsForBilling.Checked = False
				Session.Remove("ItemID")

			End If

			GetSession()
			addAttributes()
			SetPage()
			ControlVisibility()
			DataFieldBind()
		ElseIf Result1 = 0 Then
			'Session("sender") = ""
			'DataFieldBind()
		End If
	End Sub
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		'If custValidator.ControlToValidate = "cmbItemList" Then
		'    If cmbItemList.SelectedIndex <= 0 Then
		'        custValidator.ErrorMessage = "Select the Part name from the list"
		'        e.IsValid = False
		'    End If
		'Else
		If custValidator.ControlToValidate = "txtSpareDesc" Then
			If Len(txtSpareDesc.Text) > 200 Then
				custValidator.ErrorMessage = "Description must not be greater than 200 characters."
				e.IsValid = False
			End If
			'--Added By Utkarsh On 19-Jan-2011  
		ElseIf custValidator.ControlToValidate = "txtReqQty" Then
			If Val(txtReqQty.Text) = 0 Then
				custValidator.ErrorMessage = "Required Quntity must not be zero."
				e.IsValid = False
			End If
			'----------------------------------------
		End If
	End Sub
	Private Function CustomValidate1() As Boolean
		Dim strMSG As String = ""
		If Not mnWO.IsValid Then
			For i As Integer = 0 To mnWOJob.WOJobSpares.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWOJob.WOJobSpares.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If
		If strMSG.Trim <> "" Then
			cvDescription.ErrorMessage = strMSG
			cvDescription.IsValid = False
			Return False
		End If
		Return True
	End Function
	Private Sub EditRecord(ByVal Index As Int32)
		mnWOJob.WOJobSpares.CurrentIndex = Index
		txtSpareDesc.Text = mnWOJob.WOJobSpares.Item(Index).Description
		txtReqQty.Text = mnWOJob.WOJobSpares.Item(Index).RequiredQty
		cmbItemList.SelectedValue = mnWOJob.WOJobSpares.Item(Index).ItemID.ToString
		chkIsForBilling.Checked = mnWOJob.WOJobSpares.Item(Index).IsForBilling
		txtRemark.Text = mnWOJob.WOJobSpares.Item(Index).Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
		txtEffRate.Text = mnWOJob.WOJobSpares.Item(Index).EffRate
		txtEstimatedCost.Text = mnWOJob.WOJobSpares.Item(Index).EstimatedCost
		setFocus(cmbItemList)
		upnlDesc.Update()
		upnlPart.Update()
		Session("mnWOJob") = mnWOJob
	End Sub
	Private Sub DeleteRecord(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
		mnWOJob.WOJobSpares.CurrentIndex = Index
		Session("mnWOJob") = mnWOJob
	End Sub
	Private Sub ControlVisibility()
		If mnWOJob.WOJobSpares IsNot Nothing Then
			btnAddTop.Visible = IIf(mnWOJob.WOJobSpares.Count > 15, True, False)
			btnAddTop.Enabled = mnWO.WOStatusID <> 3
			btnCloseTop.Visible = IIf(mnWOJob.WOJobSpares.Count > 15, True, False)
			txtSpareDesc.ReadOnly = IIf(cmbItemList.SelectedIndex > 0, True, False)
			txtSpareDesc.BackColor = IIf(cmbItemList.SelectedIndex > 0, Color.Gainsboro, Color.White)
		End If

		btnAdd.Enabled = mnWO.WOStatusID <> 3
		dgJobSpare.Columns(9).Visible = mnWO.WOStatusID <> 3
		If AppSettings("ShowMaintenanceForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True" Then
			phhidecontrols.Visible = False
		End If
		If AppSettings("ShowAMOOnlyForNewClients") = "False" And AppSettings("ShowCAMOOnlyForNewClients") = "False" Then
			InspKit.Visible = True
		End If
	End Sub
	Private Sub ClearControls()  'Added By Utkarsh 14-Dec-2010
		txtSearchFor.Text = ""
	End Sub
	Private Sub SetValues1() 'Added By Utkarsh 14-Dec-2010
		PartNo = IIf(cmbSearch.SelectedIndex = 0, Trim(txtSearchFor.Text), "")
		Description = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
		Session("PartNo") = PartNo
		Session("Description") = Description
	End Sub
	Private Sub FindNow(ByVal LookInType As Integer, ByVal ItemName As String, ByVal Description As String)  'Added By Utkarsh 14-Dec-2010
		'dereference the objects
		mItemList = Nothing
		dgPartSearch.DataSource = Nothing

		'If chkGroundEquipment.Checked = True Then
		'    mItemList = ItemList.GetItemsList(LookInType, ItemName, Description, "", "", "", "", , , True)
		'Else


		If AppSettings("ClientCode") = "IND" Or AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then 'Added by Saylee on 19-Sep-2019
			'Here need to Skip "Tools" Primary Category records
			PrimaryCategoryIDs = New StringBuilder
			PrimaryCategoryIDs.Append("1,") 'Rotables
			PrimaryCategoryIDs.Append("3,") 'Consumables
			PrimaryCategoryIDs.Append("4") 'Others
			' mItemListForCombo = ItemList.GetItemsList(15, ItemName, Description, , , , , False, , False, PrimaryCategoryIDs:=PrimaryCategoryIDs.ToString.TrimEnd(","))
			mItemList = ItemList.GetItemList(15, ItemName, Description, "", "", "", "", False, PrimaryCategoryIDs:=PrimaryCategoryIDs.ToString.TrimEnd(","))
		Else

			mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
		End If

		dgPartSearch.DataSource = mItemList
		dgPartSearch.DataBind()
		Session("mItemList") = mItemList
		lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
	End Sub
	Private Sub Setvalues2()  'Added By Utkarsh 14-Dec-2010
		cmbItemList.SelectedValue = CType(Session("ItemID"), String)
		txtSpareDesc.Text = CType(Session("Description"), String)
	End Sub
	Private Sub ClearControls1()  'Added By Utkarsh 14-Dec-2010
		cmbSearch.SelectedIndex = 0
		txtSearchFor.Text = ""
		dgPartSearch.DataSource = Nothing
		dgPartSearch.PageIndex = 0
		Session("mItemList") = Nothing
	End Sub
	Private Sub FindNow1(ByVal ItemName As String) 'Added By Utkarsh 14-Dec-2010
		mItemList = Nothing
		dgPartSearch.DataSource = Nothing
		'If chkGroundEquipment.Checked = True Then
		'    mItemList = ItemList.GetItemsList(10, ItemName, Description, "", "", "", "", , , True)
		'Else
		If AppSettings("ClientCode") = "IND" Or AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then 'Added by Saylee on 19-Sep-2019
			'Here need to Skip "Tools" Primary Category records
			PrimaryCategoryIDs.Append("1,") 'Rotables
			PrimaryCategoryIDs.Append("3,") 'Consumables
			PrimaryCategoryIDs.Append("4") 'Others
			mItemList = ItemList.GetItemsList(15, ItemName, , "", "", "", "", PrimaryCategoryIDs:=PrimaryCategoryIDs.ToString.TrimEnd(","))
		Else
			mItemList = ItemList.GetItemList(1, ItemName, , "", "", "", "")
		End If

		'End If

		dgPartSearch.DataSource = mItemList
		dgPartSearch.DataBind()
		Session("mItemList") = mItemList
		lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		''mItemListForCombo = ItemList.GetItemsList(0, , , , , , , True, , False)


		If AppSettings("ClientCode") = "IND" Or AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then 'Added by Saylee on 19-Sep-2019
			'Here need to Skip "Tools" Primary Category records
			PrimaryCategoryIDs.Append("1,") 'Rotables
			PrimaryCategoryIDs.Append("3,") 'Consumables
			PrimaryCategoryIDs.Append("4") 'Others
			mItemListForCombo = ItemList.GetItemsList(15, , , , , , , True, , False, PrimaryCategoryIDs:=PrimaryCategoryIDs.ToString.TrimEnd(","))
		Else
			mItemListForCombo = ItemList.GetItemsList(0, , , , , , , True, , False)
		End If

		cmbItemList.DataSource = mItemListForCombo
		Session("mItemListForCombo") = mItemListForCombo
		txtJobDescription.DataBind()
		cmbItemList.DataBind()
		dgJobSpare.DataSource = mnWOJob.WOJobSpares
		dgJobSpare.DataBind()
		'-----Added By Utkarsh 14-Dec-2010
		cmbItemList.SelectedValue = IIf(CType(Session("ItemID"), String) <> Nothing, CType(Session("ItemID"), String), Guid.Empty.ToString)
		'---------------------------------

		PartNo = IIf(cmbItemList.SelectedIndex > 0, cmbItemList.SelectedItem.Text, "")
		Description = ""
		FindNow1(PartNo)
		cmbSearch.SelectedIndex = 0
		txtSearchFor.Text = PartNo
	End Sub
	Private Sub addAttributes()
		txtReqQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReqQty').value,event)")
		txtEffRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEffRate').value,event)")
	End Sub

#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		addAttributes()
		If Not IsPostBack And Session("sender") = "" Then
			setFocus(cmbItemList)
			''''''pnlInner.Visible = False  'Added by Utkarsh 14-Dec-2010
			DataFieldBind()
			If Request.QueryString("Index") IsNot Nothing Then
				Session("mIndex") = Request.QueryString("Index")
				mIndex = CType(Session("mIndex"), Int32)
				mnWOJob.WOJobSpares.CurrentIndex = mIndex
				DataBind()
				cmbItemList.SelectedValue = mnWOJob.WOJobSpares.Item(mIndex).ItemID.ToString
				''chkGroundEquipment.Enabled = False
				Session("mJobSpareEdit") = True
				upnlPart.Update()
			Else
				txtReqQty.Text = "0"
				''chkGroundEquipment.Enabled = True
			End If
		End If
		SetPage()
		ControlVisibility()

	End Sub
	Private Sub dgJobSpare_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgJobSpare.RowCommand
		Dim Idx As Int32
		Select Case e.CommandName
			Case "ViewRec"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Idx = CInt(e.CommandArgument) + dgJobSpare.PageIndex * dgJobSpare.PageSize

				If User.IsInRole("WOCompletionView") And User.IsInRole("WOCompletionEdit") Then
					cmbItemList.Enabled = (mnWOJob.WOJobSpares.Item(Idx).WOIssuedSparesCount = 0)
					txtReqQty.Enabled = (mnWOJob.WOJobSpares.Item(Idx).WOIssuedSparesCount = 0)
					txtEffRate.Enabled = (mnWOJob.WOJobSpares.Item(Idx).WOIssuedSparesCount = 0)
					txtRemark.Enabled = (mnWOJob.WOJobSpares.Item(Idx).WOIssuedSparesCount = 0)
					chkIsForBilling.Enabled = True
				Else
					If mnWOJob.WOJobSpares.Item(Idx).WOIssuedSparesCount > 0 Then
						ControlVisibility()
						MSGBoxCtrl.show("Alert!", "You cannot edit this record, as Issue against this part has been already done!", "", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If

				End If

				'---Added By Utkarsh 18-Dec-2010
				EditRecord(Idx)

				'--------------------------------
				Session("mJobSpareEdit") = True
				dgJobSpare.DataSource = mnWOJob.WOJobSpares

				dgJobSpare.DataBind()
			Case "DeleteRec"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Idx = CInt(e.CommandArgument) + dgJobSpare.PageIndex * dgJobSpare.PageSize
				Dim mRequisitionItemsNew As RequisitionItemsNew
				mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForWO(mnWO.ID, True, mnWO.WODateFormatted.ToString)
				For i As Integer = 0 To mRequisitionItemsNew.Count - 1
					If mnWOJob.WOJobSpares(Idx).ItemID.Equals(mRequisitionItemsNew(i).ItemID) Then
						MSGBoxCtrl.show("Delete Alert!", "Job Spare you are trying to delete is already selected in Requisition " + mRequisitionItemsNew(i).RequisitionNo, "Spare can not be deleted", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
				Next
				DeleteRecord(Idx)
		End Select
	End Sub
	Private Sub dgJobSpare_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgJobSpare.Sorting
		mnWOJob.WOJobSpares.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgJobSpare.DataSource = mnWOJob.WOJobSpares
		Session("mnWOJob") = mnWOJob
		dgJobSpare.DataBind()
	End Sub
	Private Sub cmbItemList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemList.SelectedIndexChanged
		If cmbItemList.SelectedIndex > 0 Then
			mItemListForCombo = Session("mItemListForCombo")
			txtSpareDesc.Text = IIf(cmbItemList.SelectedIndex > 0, mItemListForCombo(New Guid(cmbItemList.SelectedValue.ToString)).Description, "")
			txtEffRate.Text = IIf(cmbItemList.SelectedIndex > 0, LastEffRate.GetLastEffRate(New Guid(cmbItemList.SelectedValue.ToString)).EffRate, 0)
			upnlDesc.Update()
		Else
			txtSpareDesc.Text = ""
			upnlDesc.Update()
		End If

	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
		SetSession()
		'--Added by Utkarsh 20-Dec-2010
		Session.Remove("PartNo")
		Session.Remove("Description")
		Session.Remove("ItemID")
		'--------------------------------
		'Added By Vikrant On 24-May-2019 For New WO
		Dim mopenas As String = Request.QueryString("Type")

		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		Else
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobSpares.Count.ToString + "');", True)
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
			Exit Sub
		End If
		'End
		Response.Redirect(Request.QueryString("BackPage2") & "?CPage1=" & Request.QueryString("CPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
	End Sub

	Private Sub AddSpares(sender As Object, e As EventArgs) Handles btnAddTop.Click, btnAdd.Click

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
		   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

			MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
							MSGBox.Message_text.Authorization,
							"",
							MsgBoxStyle.OkOnly,
							"Authorization")

			Exit Sub

		End If

		If Not Page.IsValid Then upnlSpareValidationSummary.Update() : If Not Request.QueryString("Type") = "pup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "autoWOJobSpareList", "autoWOJobSpareList();", True) : Exit Sub

		If Page.IsValid Then

			If Session("mJobSpareEdit") = False Then

				mnWOJob.WOJobSpares.Add(mnWOJob.ID)

				If Not mnWOJob.WOJobSpares.Contains(New Guid(cmbItemList.SelectedValue), "") Then

					setObject()

					If Not CustomValidate1() Then

						If Not Request.QueryString("Type") = "pup" Then ScriptManager.RegisterStartupScript(Me,
																												 [GetType],
																												 "autoWOJobSpareList",
																												 "autoWOJobSpareList();",
																												 True)
						Exit Sub

					End If
					SetSession()

				Else

					mnWOJob.WOJobSpares.Remove(mnWOJob.WOJobSpares.CurrentItem) 'added by Utkarsh On 28-Jan-2011
					Session("Duplicate") = "Duplicate"
					Session("ToCleareList") = "ToCleareList"

					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
									MSGBox.Message_text.Duplicate,
									"JobSpare",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				End If

				cmbItemList.ClearSelection()
				txtSpareDesc.Text = ""
				txtReqQty.Text = "0"
				chkIsForBilling.Checked = False

			Else

				'Clone
				Dim clnnWOJob As nWOJob                     'added by Utkarsh On 31-Jan-2011
				clnnWOJob = mnWOJob.Clone   'added by Utkarsh On 31-Jan-2011

				Dim clnnWO As nWO    'Dim clnmnWO As nWO
				clnnWO = mnWO.Clone 'clnmnWO = mnWO.Clone

				Dim tmpIndex As Integer = mnWO.WOJobs.CurrentIndex  'added by Utkarsh On 31-Jan-2011

				setObject()
				If mnWOJob.WOJobSpares.CurrentItem.IsDirty Then

					If Not mnWOJob.WOJobSpares.Contains(mnWOJob.WOJobSpares.CurrentItem.ID, mnWOJob.WOJobSpares.CurrentItem.WOJobID, New Guid(cmbItemList.SelectedValue)) Then

						If Not CustomValidate1() Then
							Exit Sub
						End If

						Session("mnWOJob") = mnWOJob
						setFocus(cmbItemList)

					ElseIf Not mnWOJob.WOJobSpares.Contains(New Guid(cmbItemList.SelectedValue), "") Then

						If Not CustomValidate1() Then
							Exit Sub
						End If
						Session("mnWOJob") = mnWOJob
						setFocus(cmbItemList)

					Else

						mnWO.WOJobs.CurrentIndex = tmpIndex 'added by Utkarsh On 31-Jan-2011
						Session("mnWOJob") = clnnWOJob      'added by Utkarsh On 31-Jan-2011
						Session("mnWO") = clnnWO            'added by Utkarsh On 31-Jan-2011
						MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
										MSGBox.Message_text.Duplicate,
										"",
										MsgBoxStyle.OkOnly,
										"")
						Exit Sub

					End If

				End If

				Session("mJobSpareEdit") = False
				cmbItemList.ClearSelection()
				txtSpareDesc.Text = ""
				txtReqQty.Text = "0"
				chkIsForBilling.Checked = False

			End If

			ClearControls1()
			ControlVisibility()
			Session.Remove("PartNo")
			Session.Remove("Description")
			Session.Remove("ItemID")
			DataFieldBind()

			If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me,
																									  [GetType],
																									  "SetTabCount",
																									  "SetTabCount('" +
																											 mnWOJob.WOJobSpares.Count.ToString + "');",
																									  True)
			cmbItemList.ClearSelection()

			txtSpareDesc.Text = ""
			txtReqQty.Text = "0"
			txtRemark.Text = ""
			chkIsForBilling.Checked = False
			txtEffRate.Text = "0"
			txtEstimatedCost.Text = ""

			upnlDesc.Update()
			upnlPart.Update()
			upnldgJobSpare.Update()
			upnlButtons.Update()

			If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me,
																									  [GetType],
																									  "autoWOJobSpareList",
																									  "autoWOJobSpareList();",
																									  True)

		End If

	End Sub

	Private Sub dgPartSearch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
		Select Case e.CommandName
			Case "Select"
				'ClearControls()
				Dim Index1 As Int16 = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
				ItemID = mItemList(Index1).ID.ToString
				PartNo = mItemList(Index1).Name
				Description = mItemList(Index1).Description
				txtEffRate.Text = LastEffRate.GetLastEffRate(New Guid(ItemID)).EffRate
				Session("PartNo") = PartNo
				Session("Description") = Description
				Session("ItemID") = ItemID
				Setvalues2()
				'setObject()
				ClearControls1()
				ControlVisibility() 'Added by Utkarsh 20-Dec-2010
				DataFieldBind()
				pnlInner.Visible = False
				Session("ToCleareList") = "ToCleareList"
				upnlPart.Update()
				upnlDesc.Update()
			Case "ShowPartStatus"  'Added By Prashant on 2-Oct-2022  
				Dim index As Integer = (CInt(e.CommandArgument) + (dgPartSearch.PageSize * dgPartSearch.PageIndex)) 'CInt(e.CommandArgument)
				'Dim mItemStatus As Item = Item.GetItem(mItemList(index).ID) 'Cell(0) Is ItemID
				Dim LinkID As Guid = mItemList(index).ID
				Dim Unit As String = mItemList(index).UnitName

				Dim mStockPartStatus As rptStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
				Dim mOnOrderPartStatus As rptOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
				Dim mReturnablePartStatus As rptReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
				Dim mTransitPartList As rptTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
				Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode"))

				'If mStockPartStatus.Count=0 and mOnOrderPartStatus.Count=0 and mReturnablePartStatus.Count=0 and 
				Session("PartNo") = mItemList(index).Name
				Session("Description") = mItemList(index).Description
				Session("Unit") = Unit

				Session("mStockPartStatus") = mStockPartStatus
				Session("mOnOrderPartStatus") = mOnOrderPartStatus
				Session("mReturnablePartStatus") = mReturnablePartStatus
				Session("mTransitPartList") = mTransitPartList
				Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
				Session("LinkID") = LinkID
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenShowPartNoStatusWindow", "OpenShowPartNoStatusWindow();", True)
		End Select
	End Sub
	Private Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
		If PlaceHolder3.Visible = False Or pnlInner.Visible = False Then
			PlaceHolder3.Visible = True
			pnlInner.Visible = True
			PartNo = IIf(cmbItemList.SelectedIndex > 0, cmbItemList.SelectedItem.Text, "")
			Description = ""
			FindNow1(PartNo)
			cmbSearch.SelectedIndex = 0
			txtSearchFor.Text = PartNo
		ElseIf PlaceHolder3.Visible = True Or pnlInner.Visible = True Then
			PlaceHolder3.Visible = False
			pnlInner.Visible = False
		End If
		upnlPart.Update()
		upnlDesc.Update()
	End Sub
	'Added by Utkarsh 14-Dec-2010
	Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
		Dim Index As Int16 = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
		ClearControls()
		'--Added by Utkarsh 20-dec-2010
		txtSearchFor.ToolTip = IIf(Index = 1, "Enter Desciption to search", "Enter Part No. to search")
		'-------------------------------
		If cmbSearch.Enabled = True Then
			setFocus(cmbSearch)
		End If
	End Sub
	'Added by Utkarsh 14-Dec-2010
	Private Sub ImgBtnFind_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnFind.Click
		dgPartSearch.PageIndex = 0
		SetValues1()
		'FindNow(cmbSearch.SelectedValue, PartNo, Description)
		'If chkGroundEquipment.Checked = True Then
		'    FindNow(10, PartNo, Description)
		'Else
		FindNow(cmbSearch.SelectedValue, PartNo, Description)
		'End If

	End Sub
	Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
		dgPartSearch.PageIndex = e.NewPageIndex
		dgPartSearch.DataSource = mItemList
		Session("mItemList") = mItemList
		dgPartSearch.DataBind()
		setFocus(dgPartSearch)
		lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
		upnlPart.Update()
		upnlDesc.Update()
	End Sub
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		'AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

	'Private Sub chkGroundEquipment_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGroundEquipment.CheckedChanged
	'    If chkGroundEquipment.Checked = True Then
	'        mItemListForCombo = ItemList.GetItemsList(10, , , , , , , True, , True)
	'        cmbItemList.DataSource = mItemListForCombo
	'        Session("mItemListForCombo") = mItemListForCombo
	'        cmbItemList.DataBind()
	'        txtSpareDesc.Text = ""
	'    Else
	'        mItemListForCombo = ItemList.GetItemsList(0, , , , , , , True, , False)
	'        cmbItemList.DataSource = mItemListForCombo
	'        Session("mItemListForCombo") = mItemListForCombo
	'        cmbItemList.DataBind()
	'        txtSpareDesc.Text = ""
	'    End If
	'    ClearControls1()
	'    pnlInner.Visible = False
	'    ControlVisibility()
	'End Sub
	Protected Sub txtReqQty_TextChanged(sender As Object, e As EventArgs) Handles txtReqQty.TextChanged
		txtEstimatedCost.Text = Val(txtEffRate.Text) * Val(txtReqQty.Text)
		upnlEstimatedCost.Update()
	End Sub
	Protected Sub txtEffRate_TextChanged(sender As Object, e As EventArgs) Handles txtEffRate.TextChanged
		txtEstimatedCost.Text = Val(txtEffRate.Text) * Val(txtReqQty.Text)
		upnlEstimatedCost.Update()
	End Sub

	'Added by Shital on 18-Feb-2020
	Private Sub lnkSparesfromInspKit_Click(sender As Object, e As System.EventArgs) Handles lnkSparesfromInspKit.Click
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspKitWindow", "OpenInspKitWindow();", True)
	End Sub
	Private Sub hdnBtnInspKit_Click(sender As Object, e As System.EventArgs) Handles hdnBtnInspKit.Click
		Dim i As Integer
		If mKit IsNot Nothing Then
			For i = 0 To mKit.KitItems.Count - 1
				If Not mnWOJob.WOJobSpares.Contains(mKit.KitItems(i).ItemID, "") Then
					mnWOJob.WOJobSpares.Add(mnWOJob.ID)
					mnWOJob.WOJobSpares.CurrentItem.ItemID = mKit.KitItems(i).ItemID
					mnWOJob.WOJobSpares.CurrentItem.PartNo = mKit.KitItems(i).ItemName
					mnWOJob.WOJobSpares.CurrentItem.Description = mKit.KitItems(i).Description
					mnWOJob.WOJobSpares.CurrentItem.RequiredQty = mKit.KitItems(i).Qty
					mnWOJob.WOJobSpares.CurrentItem.IsForBilling = False
					mnWOJob.WOJobDescription = Trim(txtJobDescription.Text)
					mnWOJob.WOJobSpares.CurrentItem.Remark = ""
					mnWOJob.WOJobSpares.CurrentItem.EffRate = Val(txtEffRate.Text)
					mnWOJob.WOJobSpares.CurrentItem.EstimatedCost = Val(txtEstimatedCost.Text)
					mnWOJob.WOJobSpares.CurrentItem.UnitName = mKit.KitItems(i).UnitName
				Else
					Session("Duplicate") = "Duplicate"
					Session("ToCleareList") = "ToCleareList"
					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "JobSpare", MsgBoxStyle.OkOnly, "")
				End If

			Next
			DataFieldBind()
			upnldgJobSpare.Update()

		End If
	End Sub
	'---------------
#End Region

End Class