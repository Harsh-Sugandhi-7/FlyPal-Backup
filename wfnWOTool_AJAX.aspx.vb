Public Class wfnWOTool_AJAX
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mnWO As nWO
	Public mItemList As ItemList
	Public PartNo As String         'Added By Utkarsh 13-Dec-2010
	Public Description As String    'Added By Utkarsh 13-Dec-2010
	Public ItemID As String         'Added By Utkarsh 13-Dec-2010
	Public mItemListForCombo As ItemList
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
		mItemList = Session("mItemList")
		PartNo = Session("PartNo")              'Added By Utkarsh 13-Dec-2010
		Description = Session("Description")    'Added By Utkarsh 13-Dec-2010
		ItemID = Session("ItemID")              'Added By Utkarsh 13-Dec-2010
		mItemListForCombo = Session("mItemListForCombo")
	End Sub
	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("mItemList") = mItemList
		Session("PartNo") = PartNo              'Added By Utkarsh 13-Dec-2010
		Session("Description") = Description    'Added By Utkarsh 13-Dec-2010
		Session("ItemID") = ItemID              'Added By Utkarsh 13-Dec-2010
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

				IsInRoleString = "WorkOrder"

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
		If Session("Edit") Then
			lblTitle.Text = "Tool [" & mnWO.WOTools.CurrentItem.PartNo & " ]"
		Else
			lblTitle.Text = "Tool [New]"
		End If

		lblWOLabel.Text = mnWO.WONumber

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblWO.Text = "E.O. #"
		Else
			lblWO.Text = "W. O. #"
		End If
	End Sub
	Private Function setObject() As Boolean
		mnWO.WOTools.CurrentItem.ItemID = New Guid(cmbItemList.SelectedValue.ToString)
		mnWO.WOTools.CurrentItem.PartNo = Trim(cmbItemList.SelectedItem.Text)
		mnWO.WOTools.CurrentItem.Description = Trim(txtDesc.Text)
		mnWO.WOTools.CurrentItem.RequiredQty = Val(txtReqQty.Text)
		mnWO.WOTools.CurrentItem.WOToolRemark = Trim(txtRemark.Text)
		mnWO.WOTools.CurrentItem.Range = mItemListForCombo(cmbItemList.SelectedIndex).Specification 'Added By Prashant 13-Oct-2020 STR12102020
		mnWO.ApplyEdit()
		Return True
	End Function
	Private Sub ClearControls()  'Added By Utkarsh 13-Dec-2010
		txtSearchFor.Text = ""
	End Sub
	Private Sub SetValues1() 'Added By Utkarsh 13-Dec-2010
		PartNo = IIf(cmbSearch.SelectedIndex = 0, Trim(txtSearchFor.Text), "")
		Description = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
		Session("PartNo") = PartNo
		Session("Description") = Description
	End Sub
	Private Sub FindNow(ByVal LookInType As Integer, ByVal ItemName As String, ByVal Description As String)  'Added By Utkarsh 13-Dec-2010
		'dereference the objects
		mItemList = Nothing
		dgPartSearch.DataSource = Nothing
		''If chkGroundEquipment.Checked = True Then
		''    mItemList = ItemList.GetItemsList(LookInType, ItemName, Description, "", "", "", "", , , True)
		''Else
		''    mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
		''End If
		mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
		dgPartSearch.DataSource = mItemList
		dgPartSearch.DataBind()
		Session("mItemList") = mItemList
		lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
		'ControlVisibility2()
	End Sub
	Private Sub Setvalues2()  'Added By Utkarsh 13-Dec-2010
		cmbItemList.SelectedValue = CType(Session("ItemID"), String)
		txtDesc.Text = CType(Session("Description"), String)
	End Sub
	Private Sub ClearControls1()  'Added By Utkarsh 13-Dec-2010
		cmbSearch.SelectedIndex = 0
		txtSearchFor.Text = ""
		dgPartSearch.DataSource = Nothing
		dgPartSearch.PageIndex = 0
		Session("mItemList") = Nothing
	End Sub
	Private Sub FindNow1(ByVal ItemName As String) 'Added By Utkarsh 13-Dec-2010
		mItemList = Nothing
		dgPartSearch.DataSource = Nothing
		''If chkGroundEquipment.Checked = True Then
		''    mItemList = ItemList.GetItemsList(10, ItemName, Description, "", "", "", "", , ,  True)
		''Else
		''    mItemList = ItemList.GetItemList(1, ItemName, , "", "", "", "")
		''End If
		If AppSettings("ClientCode") = "IND" Or ((AppSettings("ShowMaintenanceForNewClients") = "True" And (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True"))) Then
			mItemList = ItemList.GetItemsList(15, ItemName, Description, "", "", "", "", , , PrimaryCategoryIDs:=2)
		Else
			mItemList = ItemList.GetItemList(1, ItemName, , "", "", "", "")
		End If
		dgPartSearch.DataSource = mItemList
		dgPartSearch.DataBind()
		Session("mItemList") = mItemList
		lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		If AppSettings("ClientCode") = "IND" Or ((AppSettings("ShowMaintenanceForNewClients") = "True" And (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True"))) Then
			mItemListForCombo = ItemList.GetItemsList(15, , , , , , , True, , PrimaryCategoryIDs:=2)
		Else
			mItemListForCombo = ItemList.GetItemList(0, , , , , , , True)
		End If
		cmbItemList.DataSource = mItemListForCombo
		Session("mItemListForCombo") = mItemListForCombo
		DataBind()
		'-----Added By Utkarsh 14-Dec-2010
		cmbItemList.SelectedValue = IIf(CType(Session("ItemID"), String) <> Nothing, CType(Session("ItemID"), String), IIf(mnWO.WOTools.CurrentItem Is Nothing, Guid.Empty.ToString, mnWO.WOTools.CurrentItem.ItemID.ToString))
		'---------------------------------

		PartNo = IIf(cmbItemList.SelectedIndex > 0, cmbItemList.SelectedItem.Text, "")
		Description = ""
		FindNow1(PartNo)
		cmbSearch.SelectedIndex = 0
		txtSearchFor.Text = PartNo
	End Sub

	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		If custValidator.ControlToValidate = "cmbItemList" Then
			If cmbItemList.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Select the Part name from the list"
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtDesc" Then
			If Len(txtDesc.Text) > 200 Then
				custValidator.ErrorMessage = "Description must not be greater than 200 characters."
				e.IsValid = False
			End If
		ElseIf custValidator.ControlToValidate = "txtReqQty" Then
			If Val(txtReqQty.Text) = 0 Then
				custValidator.ErrorMessage = "Qty. Required"
				e.IsValid = False
			End If
		End If
	End Sub
	Private Function CustomValidate1() As Boolean
		Dim strMSG As String = ""
		If Not mnWO.IsValid Then
			For i As Integer = 0 To mnWO.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWO.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If
		Dim mnWOTool As nWOTool
		If Not mnWO.WOTools.IsValid Then
			For Each mnWOTool In mnWO.WOTools
				For i As Integer = 0 To mnWOTool.GetBrokenRulesCollection.Count - 1
					strMSG = strMSG + mnWOTool.GetBrokenRulesCollection(i).Description + "<Br>"
				Next
			Next
		End If
		If strMSG.Trim <> "" Then
			cvDescription.ErrorMessage = strMSG
			cvDescription.IsValid = False
			Return False
		End If
		Return True
	End Function
	'Added by Utkarsh 20-Dec-2010
	Private Sub addAttributes()
		txtReqQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReqQty').value,event)")
	End Sub
	'DataGridPageChangedEventArgs
	'Added by Utkarsh 13-Dec-2010
	Public Sub NewPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
		dgPartSearch.PageIndex = e.NewPageIndex
		dgPartSearch.DataSource = mItemList
		Session("mItemList") = mItemList
		dgPartSearch.DataBind()
		setFocus(dgPartSearch)
		lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		addAttributes() 'Added by Utkarsh 20-Dec-2010
		If Not IsPostBack Then
			setFocus(cmbItemList)

			'''' pnlInner.Visible = False  'Added by Utkarsh 13-Dec-2010
			DataFieldBind()
			If (AppSettings("ShowMaintenanceForNewClients") = "True" And (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True")) Then
				chkGroundEquipment.Visible = False
			End If
			'
		End If
		SetPage()
	End Sub
	Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not Page.IsValid Then upnlValidationSummary.Update() : Exit Sub

		'If (Not User.IsInRole("WorkOrderNew") And mnWO.IsNew) Or (Not User.IsInRole("WorkOrderEdit") And Not mnWO.IsNew) Then
		'    setObject()
		'    SetSession()
		'    ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
		'    ''msg.ReplacePage = "wfnWOTool.aspx?BackPage1=" & Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage")
		'    ''Session("sender") = "Authorization"
		'    ''msg.Show()
		'    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
		'    Exit Sub
		'End If

		If Page.IsValid Then
			'If mnWO.WOTools.Contains(New Guid(cmbItemList.SelectedValue.ToString)) And mnWO.WOTools.CurrentItem.IsNew And Session("ComeForEdit") = "" Then
			'    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
			'    msg1.ReplacePage = "wfnWOTool.aspx?BackPage1=" & Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage")
			'    msg1.Show()
			'    mnWO.WOTools.CurrentItem.Description = ""
			'    Exit Sub
			'End If

			If Session("Edit") = False Then
				If Not mnWO.WOTools.Contains(New Guid(cmbItemList.SelectedValue)) Then
					setObject()
					If Not CustomValidate1() Then
						Exit Sub
					End If
					Session("mnWO") = mnWO
					Session("Edit") = False
				Else
					''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
					''msg1.ReplacePage = "wfnWOTool.aspx?BackPage1=" & Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage")
					''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
					mnWO.WOTools.CurrentItem.Description = ""      'added by Utkarsh On 31-Jan-2011
					Exit Sub
				End If
			Else

				Dim clnnWO As nWO
				clnnWO = mnWO.Clone

				setObject()
				If mnWO.WOTools.CurrentItem.IsDirty Then
					If Not mnWO.WOTools.Contains(mnWO.WOTools.CurrentItem.ID, mnWO.WOTools.CurrentItem.WOID, New Guid(cmbItemList.SelectedValue)) Then
						If Not CustomValidate1() Then
							Exit Sub
						End If
						Session("mnWO") = mnWO
						setFocus(cmbItemList)

					ElseIf Not mnWO.WOTools.Contains(New Guid(cmbItemList.SelectedValue)) Then
						If Not CustomValidate1() Then
							Exit Sub
						End If
						Session("mnWO") = mnWO
						setFocus(cmbItemList)
					Else
						mnWO = clnnWO                        'added by Utkarsh On 31-Jan-2011
						Session("mnWO") = clnnWO            'added by Utkarsh On 31-Jan-2011
						''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
						''msg1.ReplacePage = "wfnWOTool.aspx?BackPage1=" & Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage")
						''msg1.Show()
						MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")

						Exit Sub
					End If
				End If
				Session("Edit") = False
			End If
			ClearControls1()
			pnlInner.Visible = False
			Session.Remove("PartNo")
			Session.Remove("Description")
			Session.Remove("ItemID")
			Session.Remove("ComeForEdit")

			DataFieldBind()
			upnlPart.Update()
			upnlDesc.Update()


			Dim mopenas As String = Request.QueryString("Type")
			If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
				Exit Sub
			End If
			Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub
	Private Sub cmbItemList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemList.SelectedIndexChanged
		txtDesc.Text = IIf(cmbItemList.SelectedIndex > 0, mItemListForCombo(cmbItemList.SelectedIndex).Description, "")
		upnlDesc.Update()
	End Sub
	Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
		If mnWO.WOTools.CurrentItem.IsNew And Not Session("Edit") = True Then mnWO.WOTools.Remove(mnWO.WOTools.CurrentItem)
		Session("Edit") = False

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub chkGroundEquipment_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGroundEquipment.CheckedChanged
		If (chkGroundEquipment.Checked = True) And (AppSettings("ShowMaintenanceForNewClients") = "True" And (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True")) Then
			mItemListForCombo = ItemList.GetItemsList(15, , , , , , , True, , PrimaryCategoryIDs:=2)
			cmbItemList.DataSource = mItemListForCombo
			Session("mItemListForCombo") = mItemListForCombo
			cmbItemList.DataBind()
			txtDesc.Text = ""
		ElseIf chkGroundEquipment.Checked = True Then
			mItemListForCombo = ItemList.GetItemsList(10,  , ,  , , , , True, , True)
			cmbItemList.DataSource = mItemListForCombo
			Session("mItemListForCombo") = mItemListForCombo
			cmbItemList.DataBind()
			txtDesc.Text = ""
		Else
			mItemListForCombo = ItemList.GetItemsList(0, , , , , , , True, , False)
			cmbItemList.DataSource = mItemListForCombo
			Session("mItemListForCombo") = mItemListForCombo
			cmbItemList.DataBind()
			txtDesc.Text = ""
		End If
		ClearControls()
		pnlInner.Visible = False
		upnlPart.Update()
		upnlDesc.Update()
	End Sub
	'Added by Utkarsh 13-Dec-2010
	Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
		If pnlInner.Visible = False Then
			pnlInner.Visible = True
			PartNo = IIf(cmbItemList.SelectedIndex > 0, cmbItemList.SelectedItem.Text, "")
			Description = ""
			FindNow1(PartNo)
			cmbSearch.SelectedIndex = 0
			txtSearchFor.Text = PartNo
		ElseIf pnlInner.Visible = True Then
			pnlInner.Visible = False
		End If
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
	'Added by Utkarsh 13-Dec-2010
	Private Sub dgPartSearch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
		Select Case e.CommandName
			Case "Select"
				Dim Index As Int16 = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
				ItemID = mItemList(Index).ID.ToString
				PartNo = mItemList(Index).Name
				Description = mItemList(Index).Description
				Session("PartNo") = PartNo
				Session("Description") = Description
				Session("ItemID") = ItemID
				Setvalues2()
				mnWO.WOTools.CurrentItem.Description = Trim(txtDesc.Text)
				'setObject()
				ClearControls1()
				DataFieldBind()
				pnlInner.Visible = False
				Session.Remove("ItemID")
				Session.Remove("PartNo")
				Session.Remove("Description")

				If (AppSettings("ShowMaintenanceForNewClients") = "True" And (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True")) Then
					chkGroundEquipment.Visible = False
				End If


				upnlPart.Update()
				upnlDesc.Update()
		End Select
	End Sub
	'Added by Utkarsh 13-Dec-2010
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
	'Added by Utkarsh 13-Dec-2010
	Private Sub ImgBtnFind_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnFind.Click
		dgPartSearch.PageIndex = 0
		SetValues1()
		'''If chkGroundEquipment.Checked = True Then
		'''    FindNow(10, PartNo, Description)
		'''Else
		'''    FindNow(cmbSearch.SelectedValue, PartNo, Description)
		'''End If

		FindNow(cmbSearch.SelectedValue, PartNo, Description)
	End Sub

#End Region
End Class