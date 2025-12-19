'CREATED By : Saylee
'Dated      : 24-Dec-2013


Public Class wfnWOJobComp_AJAX
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	' Public mPartList As PartList
	Dim mPartListForCombo As PartListForCombo
	Protected mnWOJob As nWOJob
	Protected mnWO As nWO

	Public mRemovalReasonList As RemovalReasonList
	Public mPartListForSerialNos As PartListForSerialNos
	Public mnWOModelNameValueList As nWOModelNameValueList

	Public mnWOModelListForSerialNos As nWOModelListForSerialNos

	Dim ComponentIndex As Integer
	Dim ComponentName As String
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

#Region " Helper Methods "
	Public Sub GetSession()
		mnWO = Session("mnWO")
		mnWOJob = Session("mnWOJob")
		mnWOModelNameValueList = Session("mnWOModelNameValueList")
		mRemovalReasonList = Session("mRemovalReasonList")
		mPartListForCombo = Session("mPartListForCombo")
		mnWOModelListForSerialNos = Session("mnWOModelListForSerialNos")
		mPartListForSerialNos = Session("mPartListForSerialNos")
	End Sub
	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("mnWOJob") = mnWOJob
		Session("mnWOModelNameValueList") = mnWOModelNameValueList
		Session("mPartListForCombo") = mPartListForCombo
		Session("mRemovalReasonList") = mRemovalReasonList
		Session("mnWOModelListForSerialNos") = mnWOModelListForSerialNos
		Session("mPartListForSerialNos") = mPartListForSerialNos
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
			'IsInRoleString = "WorkOrder"
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

	Private Sub DataFieldBind()
		If chkIsAssembly.Checked = False Then
			''Off Part List
			mPartListForCombo = PartListForCombo.GetPartListForCombo(Guid.Empty, "", , , "(SELECT)")
			cmbOffPartList.DataSource = mPartListForCombo
			Session("mPartListForCombo") = mPartListForCombo

			'On Part List
			cmbOnPartList.DataSource = mPartListForCombo

		Else
			mnWOModelNameValueList = nWOModelNameValueList.GetModelList("(SELECT)", False)
			cmbOffPartList.DataSource = mnWOModelNameValueList
			Session("mnWOModelNameValueList") = mnWOModelNameValueList

			'On Part List
			cmbOnPartList.DataSource = mnWOModelNameValueList

		End If
		SetLabels(chkIsAssembly.Checked)
		'Removal Reason List
		mRemovalReasonList = RemovalReasonList.GetRemovalReasonList(, "(SELECT)")
		cmbRemovalReason.DataSource = mRemovalReasonList
		Session("mRemovalReasonList") = mRemovalReasonList

		'Removal/Installation Grid 
		dgRemovalInstallation.DataSource = mnWOJob.WOJobComps

		If cmbOffPartList.SelectedIndex > 0 Then
			cmbOffSerialNo.Enabled = True
		Else
			cmbOffSerialNo.Enabled = False
		End If
		Call cmbOffPartList_SelectedIndexChanged(Nothing, Nothing)

		DataBind()
	End Sub
	Private Sub chkIsRemoval()
		If chkRemoval.Checked = True Then
			cmbOffPartList.Enabled = True
			txtOffPartNo.ReadOnly = False
			txtOffDescription.ReadOnly = False
			txtOffDescription.Enabled = True

			txtOffSerialNo.ReadOnly = False
			cmbOffSerialNo.Enabled = True

			txtOffRemark.ReadOnly = False
			txtOffRemark.Enabled = True

			cmbRemovalReason.Enabled = True
			txtOffTSN.ReadOnly = False
			txtOffTSN.Enabled = True

			txtOffCSN.ReadOnly = False
			txtOffCSN.Enabled = True

			txtOffPosition.ReadOnly = False
			txtOffPosition.Enabled = True


			txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.White)
			'cmbRemovalReason.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffPosition.BackColor = Color.FromKnownColor(KnownColor.White)

			tblRem.BgColor = "#FFFFFF"
		Else
			cmbOffPartList.Enabled = False
			txtOffPartNo.ReadOnly = True

			txtOffDescription.ReadOnly = True
			txtOffDescription.Enabled = False

			txtOffSerialNo.ReadOnly = True
			cmbOffSerialNo.Enabled = False

			txtOffRemark.ReadOnly = True
			txtOffRemark.Enabled = False

			cmbRemovalReason.Enabled = False

			txtOffTSN.ReadOnly = True
			txtOffTSN.Enabled = False

			txtOffCSN.ReadOnly = True
			txtOffCSN.Enabled = False

			txtOffPosition.ReadOnly = True
			txtOffPosition.Enabled = False

			txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.Silver)

			txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.Silver)
			'cmbRemovalReason.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOffPosition.BackColor = Color.FromKnownColor(KnownColor.Silver)

			'cmbOffPartList.SelectedIndex = 0
			cmbOffPartList.ClearSelection()
			txtOffPartNo.Text = ""
			txtOffDescription.Text = ""
			txtOffSerialNo.Text = ""
			txtOffRemark.Text = ""
			'cmbRemovalReason.SelectedIndex = 0
			cmbRemovalReason.ClearSelection()
			txtOffTSN.Text = ""
			txtOffCSN.Text = ""
			'cmbOffSerialNo.SelectedIndex = 0
			cmbOffSerialNo.ClearSelection()

			tblRem.BgColor = "E0E0E0"
		End If
	End Sub
	Private Sub chkIsIntallation()
		If chkInstallation.Checked = True Then

			cmbOnPartList.Enabled = True
			txtOnPartNo.ReadOnly = False
			txtOnPartNo.Enabled = True
			txtOnDescription.ReadOnly = False
			txtOnDescription.Enabled = True

			txtOnSerialNo.ReadOnly = False
			txtOnSerialNo.Enabled = True

			txtOnRemark.ReadOnly = False
			txtOnRemark.Enabled = True

			txtOnTSN.ReadOnly = False
			txtOnTSN.Enabled = True

			txtOnCSN.ReadOnly = False
			txtOnCSN.Enabled = True

			txtOnPosition.ReadOnly = False
			txtOnPosition.Enabled = True

			'Added By Saylee On 15-Oct-2020 For STR12102020
			txtGRN.ReadOnly = False
			txtGRN.Enabled = True

			txtFormNo.ReadOnly = False
			txtFormNo.Enabled = True

			'End

			txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnPosition.BackColor = Color.FromKnownColor(KnownColor.White)

			'Added By Saylee On 15-Oct-2020 For STR12102020
			txtGRN.BackColor = Color.FromKnownColor(KnownColor.White)
			txtFormNo.BackColor = Color.FromKnownColor(KnownColor.White)
			'end
			tblInst.BgColor = "#FFFFFF"
		Else
			cmbOnPartList.Enabled = False
			txtOnPartNo.ReadOnly = True
			txtOnPartNo.Enabled = False
			txtOnDescription.ReadOnly = True
			txtOnDescription.Enabled = False

			txtOnSerialNo.ReadOnly = True
			txtOnSerialNo.Enabled = False

			txtOnRemark.ReadOnly = True
			txtOnRemark.Enabled = False

			txtOnTSN.ReadOnly = True
			txtOnTSN.Enabled = False

			txtOnCSN.ReadOnly = True
			txtOnCSN.Enabled = False

			txtOnPosition.ReadOnly = True
			txtOnPosition.Enabled = False
			'txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
			'txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			'txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			'txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.White)
			'txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.White)
			'txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.White)


			txtGRN.ReadOnly = True
			txtGRN.Enabled = False

			txtFormNo.ReadOnly = True
			txtFormNo.Enabled = False


			txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.Silver)

			txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.Silver) '"E0E0E0"
			txtOnPosition.BackColor = Color.FromKnownColor(KnownColor.Silver)

			txtGRN.BackColor = Color.FromKnownColor(KnownColor.Silver)
			txtFormNo.BackColor = Color.FromKnownColor(KnownColor.Silver)

			'cmbOnPartList.SelectedIndex = 0
			cmbOnPartList.ClearSelection()
			txtOnPartNo.Text = ""
			txtOnDescription.Text = ""
			txtOnSerialNo.Text = ""
			txtOnRemark.Text = ""
			txtOnTSN.Text = ""
			txtOnCSN.Text = ""
			tblInst.BgColor = "E0E0E0"
			txtGRN.Text = ""
			txtFormNo.Text = ""
		End If
	End Sub
	Private Sub SetEnability(ByVal IsInstall As Boolean, ByVal IsRemoval As Boolean)
		If IsInstall = True Then
			cmbOnPartList.Enabled = True
			If cmbOnPartList.SelectedIndex <= 0 Then
				txtOnPartNo.ReadOnly = False
				txtOnPartNo.Enabled = True

				txtOnDescription.ReadOnly = False
				txtOnDescription.Enabled = True

			End If
			txtOnSerialNo.ReadOnly = False
			txtOnSerialNo.Enabled = True

			txtOnRemark.ReadOnly = False
			txtOnRemark.Enabled = True

			txtOnTSN.ReadOnly = False
			txtOnTSN.Enabled = True

			txtOnCSN.ReadOnly = False
			txtOnCSN.Enabled = True

			''txtOnPartNo.BackColor = txtOnPartNo.BackColor.FromKnownColor(KnownColor.White)
			''txtOnDescription.BackColor = txtOnDescription.BackColor.FromKnownColor(KnownColor.White)
			''txtOnSerialNo.BackColor = txtOnSerialNo.BackColor.FromKnownColor(KnownColor.White)

			''txtOnRemark.BackColor = txtOnRemark.BackColor.FromKnownColor(KnownColor.White)
			''txtOnTSN.BackColor = txtOnTSN.BackColor.FromKnownColor(KnownColor.White)
			''txtOnCSN.BackColor = txtOnCSN.BackColor.FromKnownColor(KnownColor.White)

		Else
			cmbOnPartList.Enabled = False

			txtOnPartNo.ReadOnly = True
			txtOnPartNo.Enabled = False
			txtOnDescription.ReadOnly = True
			txtOnDescription.Enabled = False

			txtOnSerialNo.ReadOnly = True
			txtOnSerialNo.Enabled = False

			txtOnRemark.ReadOnly = True
			txtOnRemark.Enabled = False

			txtOnTSN.ReadOnly = True
			txtOnTSN.Enabled = False

			txtOnCSN.ReadOnly = True
			txtOnCSN.Enabled = False

			txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.White)

			'cmbOnPartList.SelectedIndex = 0
			cmbOnPartList.ClearSelection()
			txtOnPartNo.Text = ""
			txtOnDescription.Text = ""
			txtOnSerialNo.Text = ""
			txtOnRemark.Text = ""
			txtOnTSN.Text = ""
			txtOnCSN.Text = ""
			txtGRN.Text = ""
			txtFormNo.Text = ""
		End If

		If IsRemoval = True Then

			cmbOffPartList.Enabled = True

			If cmbOffPartList.SelectedIndex <= 0 Then
				txtOffPartNo.ReadOnly = False
				txtOffDescription.ReadOnly = False
				txtOffDescription.Enabled = True

				txtOffSerialNo.ReadOnly = False
			End If


			''cmbOffSerialNo.Enabled = True

			txtOffRemark.ReadOnly = False
			txtOffRemark.Enabled = True

			cmbRemovalReason.Enabled = True
			txtOffTSN.ReadOnly = False
			txtOffTSN.Enabled = True

			txtOffCSN.ReadOnly = False
			txtOffCSN.Enabled = True

			''txtOffPartNo.BackColor = txtOffPartNo.BackColor.FromKnownColor(KnownColor.White)
			''txtOffDescription.BackColor = txtOffDescription.BackColor.FromKnownColor(KnownColor.White)
			''txtOffSerialNo.BackColor = txtOffSerialNo.BackColor.FromKnownColor(KnownColor.White)

			''txtOffRemark.BackColor = txtOffRemark.BackColor.FromKnownColor(KnownColor.White)
			''cmbRemovalReason.BackColor = cmbRemovalReason.BackColor.FromKnownColor(KnownColor.White)
			''txtOffTSN.BackColor = txtOffTSN.BackColor.FromKnownColor(KnownColor.White)
			''txtOffCSN.BackColor = txtOffCSN.BackColor.FromKnownColor(KnownColor.White)

		Else
			cmbOffPartList.Enabled = False
			txtOffPartNo.ReadOnly = True

			txtOffDescription.ReadOnly = True
			txtOffDescription.Enabled = False

			txtOffSerialNo.ReadOnly = True
			cmbOffSerialNo.Enabled = False

			txtOffRemark.ReadOnly = True
			txtOffRemark.Enabled = False

			cmbRemovalReason.Enabled = False

			txtOffTSN.ReadOnly = True
			txtOffTSN.Enabled = False

			txtOffCSN.ReadOnly = True
			txtOffCSN.Enabled = False

			txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.White)
			cmbRemovalReason.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.White)
			txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.White)

			cmbOffPartList.ClearSelection()
			txtOffPartNo.Text = ""
			txtOffDescription.Text = ""
			txtOffSerialNo.Text = ""
			txtOffRemark.Text = ""
			cmbRemovalReason.ClearSelection()
			txtOffTSN.Text = ""
			txtOffCSN.Text = ""
			cmbOffSerialNo.ClearSelection()
		End If
	End Sub
	Private Sub SetLabels(ByVal IsAssembly As Boolean)
		If IsAssembly = False Then
			lblOffPartList.Text = "Part No."
			lblOffPartNo.Text = "Part Number"
			lblOffDescription.Text = "Part Description"  'Added by shital on 30-Oct-2020'

			lblOnPartList.Text = "Part No."
			lblOnPartNo.Text = "Part Number"
			lblOnDescription.Text = "Part Description"  'Added by shital on 30-Oct-2020'

			txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
			txtOnPartNo.ToolTip = "Enter Part Name for Installed Component"

			txtOffDescription.ToolTip = "Enter Description for Removed Component"
			txtOnDescription.ToolTip = "Enter Description for Installed Component"

			txtOffSerialNo.ToolTip = "Enter Serial Number for Removed Component"
			txtOnSerialNo.ToolTip = "Enter Serial Number for Installed Component"

			txtOffRemark.ToolTip = "Enter Remark for Removed Component"
			txtOnRemark.ToolTip = "Enter Remark for Installed Component"

			txtOffTSN.ToolTip = "Enter TSN for Removed Component"
			txtOnTSN.ToolTip = "Enter TSN for Installed Component"

			txtOffCSN.ToolTip = "Enter CSN for Removed Component"
			txtOnCSN.ToolTip = "Enter CSN for Installed Component"

		Else
			lblOffPartList.Text = "Model No."
			lblOffPartNo.Text = "Model Name"
			lblOffDescription.Text = "Assembly  Description"  'Added by shital on 30-Oct-2020'

			lblOnPartList.Text = "Model No."
			lblOnPartNo.Text = "Model Name"
			lblOnDescription.Text = "Assembly  Description"  'Added by shital on 30-Oct-2020'

			txtOffPartNo.ToolTip = "Enter Model Name for Removed Assembly"
			txtOnPartNo.ToolTip = "Enter Model Name for Installed Assembly"

			txtOffDescription.ToolTip = "Enter Description for Removed Assembly"
			txtOnDescription.ToolTip = "Enter Description for Installed Assembly"

			txtOffSerialNo.ToolTip = "Enter Serial Number for Removed Assembly"
			txtOnSerialNo.ToolTip = "Enter Serial Number for Installed Assembly"

			txtOffRemark.ToolTip = "Enter Remark for Removed Assembly"
			txtOnRemark.ToolTip = "Enter Remark for Installed Assembly"

			txtOffTSN.ToolTip = "Enter TSN for Removed Assembly"
			txtOnTSN.ToolTip = "Enter TSN for Installed Assembly"

			txtOffCSN.ToolTip = "Enter CSN for Removed Assembly"
			txtOnCSN.ToolTip = "Enter CSN for Installed Assembly"
		End If
	End Sub
	Private Sub OnPartSelection()
		If cmbOnPartList.SelectedIndex <= 0 Then

			txtOnSerialNo.Enabled = True


			txtOnPartNo.ReadOnly = False
			txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOnDescription.ReadOnly = False
			txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)

		Else
			txtOnPartNo.ReadOnly = True
			txtOnPartNo.BackColor = Color.Gainsboro

			txtOnDescription.ReadOnly = True
			txtOnDescription.BackColor = Color.Gainsboro

			If chkIsAssembly.Checked = False Then
				'COMPONENT
				mPartListForCombo = Session("mPartListForCombo")
				txtOnPartNo.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, "")
				txtOnDescription.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Description, "")
			Else
				'ASSEMBLY
				mnWOModelNameValueList = Session("mnWOModelNameValueList")
				txtOnPartNo.Text = IIf(cmbOnPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, "")
				txtOnDescription.Text = IIf(cmbOnPartList.SelectedIndex > 0, "", "")
			End If
		End If
	End Sub
	Private Sub OffPartSelection()
		If cmbOffPartList.SelectedIndex <= 0 Then
			cmbOffSerialNo.Enabled = False
			'cmbOffSerialNo.SelectedIndex = 0
			cmbOffSerialNo.ClearSelection()
			ComponentIndex = cmbOffPartList.SelectedIndex
			Session("ComponentIndex") = ComponentIndex

			txtOffSerialNo.Enabled = True
			txtOffPartNo.ReadOnly = False
			txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOffDescription.ReadOnly = False
			txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
			txtOffDescription.ToolTip = "Enter Description for Removed Component"
		Else
			txtOffPartNo.ReadOnly = True
			txtOffPartNo.BackColor = Color.Gainsboro

			txtOffDescription.ReadOnly = True
			txtOffDescription.BackColor = Color.Gainsboro


			If chkIsAssembly.Checked = False Then
				'COMPONENT
				mPartListForCombo = Session("mPartListForCombo")

				cmbOffSerialNo.Enabled = True

				ComponentName = cmbOffPartList.SelectedValue.ToString

				mPartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(mPartListForCombo(New Guid(ComponentName)).Name, "", Today.Date.ToString, , "(SELECT)")
				Session("mPartListForSerialNos") = mPartListForSerialNos

				txtOffPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Name, "")
				txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Description, "")

				txtOffPartNo.ToolTip = "Part Name for Removed Component"
				txtOffDescription.ToolTip = "Description for Removed Component"


				If mPartListForSerialNos.Count > 1 Then
					If Not mPartListForSerialNos(1).SerialNo = "" Then
						cmbOffSerialNo.DataSource = mPartListForSerialNos
						cmbOffSerialNo.DataBind()
					Else
						cmbOffSerialNo.Items.Clear()
						cmbOffSerialNo.Items.Add("(SELECT)")
						cmbOffSerialNo.DataBind()
					End If
				Else
					cmbOffSerialNo.Items.Clear()
					cmbOffSerialNo.Items.Add("(SELECT)")
					cmbOffSerialNo.DataBind()
				End If
			Else
				''ASSEMBLY
				mnWOModelNameValueList = Session("mnWOModelNameValueList")

				cmbOffSerialNo.Enabled = True

				ComponentName = cmbOffPartList.SelectedValue.ToString
				mnWOModelListForSerialNos = nWOModelListForSerialNos.GetModelListForSerialNosList(mnWOModelNameValueList(New Guid(ComponentName)).Name, "", Today.Date.ToString, , "(SELECT)")

				txtOffPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(ComponentName)).Name, "")
				'' txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, "", "")
				txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(ComponentName)).Description, "")

				txtOffPartNo.ToolTip = "Model Name for Removed Assembly"
				txtOffDescription.ToolTip = "Description for Removed Assembly"



				If mnWOModelListForSerialNos.Count > 1 Then
					If Not mnWOModelListForSerialNos(1).SerialNo = "" Then
						cmbOffSerialNo.DataSource = mnWOModelListForSerialNos
						cmbOffSerialNo.DataBind()
					Else
						cmbOffSerialNo.Items.Clear()
						cmbOffSerialNo.Items.Add("(SELECT)")
						cmbOffSerialNo.DataBind()
					End If
				Else
					cmbOffSerialNo.Items.Clear()
					cmbOffSerialNo.Items.Add("(SELECT)")
					cmbOffSerialNo.DataBind()
				End If
			End If
			txtOffPartNo.DataBind()
			txtOffDescription.DataBind()
			Session("mnWOModelListForSerialNos") = mnWOModelListForSerialNos
			ComponentIndex = cmbOffPartList.SelectedIndex
			Session("ComponentIndex") = ComponentIndex
		End If
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					'Added By Vikrant On 27-June-2013 For ALL27062013
					If MSGBoxCtrl.Sender = "Confirm" Then
						Session("sender") = ""
						If Session("mWOJobCompsEdit") = True Then
							mnWOJob = Session("mnWOJob")
							DataFieldBind()
							SetControl(mnWOJob.WOJobComps.CurrentIndex)
							Session("mWOJobCompsEdit") = False
						End If
						If Not Save() Then
							Exit Sub
						End If
						''Response.Redirect("wfnWOJobComp.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
						SetTitle()
						If mnWO.WOStatusID = 3 Then
							ControlVisibility()
						End If

						CallUpdatePanels()
						'End
					ElseIf MSGBoxCtrl.Sender = "Delete" Then
						Try
							Session("sender") = ""
							mnWOJob = Session("mnWOJob")
							mnWOJob.WOJobComps.Remove(mnWOJob.WOJobComps.CurrentIndex)
							For i As Integer = 0 To mnWOJob.WOJobComps.Count - 1
								mnWOJob.WOJobComps(i).SrNo = i + 1
							Next
							Session("mnWO") = mnWO
							Session("mWOJobCompsEdit") = False
							''DataFieldBind()
							'Removal/Installation Grid 
							dgRemovalInstallation.DataSource = mnWOJob.WOJobComps
							' dgRemovalInstallation.DataBind()
							'Response.Redirect("wfnWOJobComp.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
							DataFieldBind()
							SetTitle()
							If mnWO.WOStatusID = 3 Then
								ControlVisibility()
							End If
							ClearControls()
							CallUpdatePanels()
							If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobComps.Count.ToString + "');", True)
						Catch ex As SqlException
						End Try
					End If
				Case MsgBoxResult.No

					'''''Added By Vikrant On 27-June-2013 For ALL27062013
					'Commented by Saylee on 11-July-2013 For ALL27062013
					''''If Session("mWOJobCompsEdit") = False Then
					''''    mnWOJob = Session("mnWOJob")
					''''    mnWOJob.WOJobComps.Remove(mnWOJob.WOJobComps.CurrentIndex)
					''''    Session("mnWOJob") = mnWOJob
					''''Else
					''''    mnWOJob = Session("mnWOJobClone")
					''''    Session("mnWOJob") = mnWOJob
					''''    mnWO = Session("mnWOClone")
					''''    Session("mnWO") = mnWO
					''''End If
					If MSGBoxCtrl.Sender = "Delete" Then 'Added by Saylee on 11-July-2013 For ALL27062013
						Session("sender") = ""
						Session("mWOJobCompsEdit") = False
						DataFieldBind()
						SetTitle()
						If mnWO.WOStatusID = 3 Then
							ControlVisibility()
						End If
						chkInstallation.Checked = True
						chkRemoval.Checked = True
						SetEnability(chkInstallation.Checked, chkRemoval.Checked)
						chkIsRemoval()
						chkIsIntallation()
						ClearAllControls()
						CallUpdatePanels()

					ElseIf MSGBoxCtrl.Sender = "Confirm" Then
						If Session("mWOJobCompsEdit") = False Then
							mnWOJob.WOJobComps.Remove(mnWOJob.WOJobComps.CurrentItem)
						Else
							mnWOJob = Session("mnWOJobClone")
						End If

						Session("mnWOJob") = mnWOJob
						Session("sender") = ""
						Session("mWOJobCompsEdit") = False
						DataFieldBind()
						SetTitle()
						If mnWO.WOStatusID = 3 Then
							ControlVisibility()
						End If
						ClearControls()
						CallUpdatePanels()
						Session("mWOJobCompsEdit") = False
					End If

				Case MsgBoxResult.Ok
					Session("sender") = ""
					DataFieldBind()
					SetTitle()
					If mnWO.WOStatusID = 3 Then
						ControlVisibility()
					End If
					CallUpdatePanels()

				Case MsgBoxResult.Ok And Session("sender") = "Authorization"
					Session("sender") = ""
					DataFieldBind()
					'Response.Redirect("wfnWOJobComp.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
					SetTitle()
					If mnWO.WOStatusID = 3 Then
						ControlVisibility()
					End If
					CallUpdatePanels()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			DataFieldBind()
			'Response.Redirect("wfnWOJobComp.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
			SetTitle()
			If mnWO.WOStatusID = 3 Then
				ControlVisibility()
			End If
			CallUpdatePanels()
		ElseIf Result1 = 0 Then

		End If
	End Sub
	Private Sub ControlVisibility()
		chkIsAssembly.Enabled = mnWO.WOStatusID <> 3
		'Off Part
		chkRemoval.Enabled = mnWO.WOStatusID <> 3
		txtOffPartNo.Enabled = mnWO.WOStatusID <> 3
		cmbOffPartList.Enabled = mnWO.WOStatusID <> 3
		txtOffPartNo.Enabled = mnWO.WOStatusID <> 3
		txtOffDescription.Enabled = mnWO.WOStatusID <> 3
		txtOffSerialNo.Enabled = mnWO.WOStatusID <> 3
		cmbOffSerialNo.Enabled = mnWO.WOStatusID <> 3
		cmbRemovalReason.Enabled = mnWO.WOStatusID <> 3
		txtOffTSN.Enabled = mnWO.WOStatusID <> 3
		txtOffCSN.Enabled = mnWO.WOStatusID <> 3
		txtOffRemark.Enabled = mnWO.WOStatusID <> 3

		'Off Part
		chkInstallation.Enabled = mnWO.WOStatusID <> 3
		cmbOnPartList.Enabled = mnWO.WOStatusID <> 3 And CType(Session("IsInstall"), Boolean) = True
		txtOnPartNo.Enabled = mnWO.WOStatusID <> 3
		txtOnDescription.Enabled = mnWO.WOStatusID <> 3
		txtOnSerialNo.Enabled = mnWO.WOStatusID <> 3
		txtOnTSN.Enabled = mnWO.WOStatusID <> 3
		txtOnCSN.Enabled = mnWO.WOStatusID <> 3
		txtOnRemark.Enabled = mnWO.WOStatusID <> 3
		dgRemovalInstallation.Columns(10).Visible = mnWO.WOStatusID <> 3


		If Session("mWOJobCompsEdit") = True Then
			chkIsAssembly.Enabled = False
			chkRemoval.Enabled = False
			chkInstallation.Enabled = False
		Else
			chkIsAssembly.Enabled = True
			chkRemoval.Enabled = True
			chkInstallation.Enabled = True
		End If
	End Sub
	Private Sub SetTitle()
		' lblJobLabel.Text = mnWOJob.SrNo Ajay h
		' lblWOLabel.Text = mnWO.WONumber
		txtJobDescription.Text = mnWOJob.WOJobDescription

		If Session("mWOJobCompsEdit") = True Then
			chkIsAssembly.Enabled = False
			chkRemoval.Enabled = False
			chkInstallation.Enabled = False
		Else
			chkIsAssembly.Enabled = True
			chkRemoval.Enabled = True
			chkInstallation.Enabled = True
		End If

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			' lblWO.Text = "E.O. #" Ajay h
		Else
			' lblWO.Text = "W. O. #" ajay h
		End If
	End Sub
	Private Sub SetObject()
		mnWOJob.WOJobComps.CurrentItem.IsAssembly = chkIsAssembly.Checked
		mnWOJob.WOJobComps.CurrentItem.IsForRemoval = chkRemoval.Checked
		mnWOJob.WOJobComps.CurrentItem.OffPartID = New Guid(cmbOffPartList.SelectedValue.ToString)

		mnWOJob.WOJobComps.CurrentItem.OffRemark = Trim(txtOffRemark.Text)
		mnWOJob.WOJobComps.CurrentItem.RemovalReasonID = New Guid(cmbRemovalReason.SelectedValue.ToString)
		mnWOJob.WOJobComps.CurrentItem.OffTSN = Trim(txtOffTSN.Text)
		mnWOJob.WOJobComps.CurrentItem.OffCSN = Trim(txtOffCSN.Text)

		mnWOJob.WOJobComps.CurrentItem.IsForInstall = chkInstallation.Checked
		mnWOJob.WOJobComps.CurrentItem.OnRemark = Trim(txtOnRemark.Text)
		mnWOJob.WOJobComps.CurrentItem.OnTSN = Trim(txtOnTSN.Text)
		mnWOJob.WOJobComps.CurrentItem.OnCSN = Trim(txtOnCSN.Text)
		mnWOJob.WOJobComps.CurrentItem.OnSerialNo = Trim(txtOnSerialNo.Text)

		If chkIsAssembly.Checked = False Then 'COMPONENT

			mnWOJob.WOJobComps.CurrentItem.OffPartNo = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOffPartList.SelectedValue.ToString)).Name, Trim(txtOffPartNo.Text))
			mnWOJob.WOJobComps.CurrentItem.OffDescription = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOffPartList.SelectedValue.ToString)).Description, Trim(txtOffDescription.Text))
			'''If cmbOffPartList.SelectedIndex > 0 Then
			'''    ' mnWOJob.WOJobComps.CurrentItem.OffSerialNo = IIf(cmbOffSerialNo.SelectedIndex > 0, mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo, Trim(txtOffSerialNo.Text))
			'''    If cmbOffSerialNo.SelectedIndex > 0 Then
			'''        mnWOJob.WOJobComps.CurrentItem.OffSerialNo = mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo
			'''    Else
			'''        mnWOJob.WOJobComps.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
			'''    End If
			'''Else
			'''    mnWOJob.WOJobComps.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
			'''End If
			If cmbOffPartList.SelectedIndex > 0 Then
				' mnWOJob.WOJobComps.CurrentItem.OffSerialNo = IIf(cmbOffSerialNo.SelectedIndex > 0, mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo, Trim(txtOffSerialNo.Text))
				If cmbOffSerialNo.SelectedIndex > 0 Then
					mnWOJob.WOJobComps.CurrentItem.OffSerialNo = mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo

					'Added By Saylee on 19-Apr-2024 , to track Removal/Inst in system
					'                    Dim mCompStatusList As CompStatusList = CompStatusList.GetCompStatusList(mnWO.MachineID, PartName:=mnWOJob.WOJobComps.CurrentItem.OffPartNo, CompSerialNo:=mnWOJob.WOJobComps.CurrentItem.OffSerialNo)
					Dim mCompStatusList As CompStatusList = CompStatusList.GetCompStatusList(Guid.Empty, CurrentDate:=Today.Date.ToString,
																							 CompID:=mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).CompID.ToString,
																							 PartName:=mnWOJob.WOJobComps.CurrentItem.OffPartNo,
																							 CompSerialNo:=mnWOJob.WOJobComps.CurrentItem.OffSerialNo,
																							 IsCompInstalled:=True, IsCompPeriodsRequired:=False)
					If mCompStatusList.Count = 1 Then
						mnWOJob.WOJobComps.CurrentItem.CompStatusOffID = mCompStatusList(0).ID
					End If


				Else
					mnWOJob.WOJobComps.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
				End If
			Else
				mnWOJob.WOJobComps.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
			End If


			mnWOJob.WOJobComps.CurrentItem.OnPartID = New Guid(cmbOnPartList.SelectedValue.ToString)
			mnWOJob.WOJobComps.CurrentItem.OnPartNo = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, Trim(txtOnPartNo.Text))
			mnWOJob.WOJobComps.CurrentItem.OnDescription = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Description, Trim(txtOnDescription.Text))

		Else 'ASSEMBLY

			mnWOJob.WOJobComps.CurrentItem.OffPartNo = IIf(cmbOffPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(cmbOffPartList.SelectedValue.ToString)).Name, Trim(txtOffPartNo.Text))
			mnWOJob.WOJobComps.CurrentItem.OffDescription = Trim(txtOffDescription.Text)

			If cmbOffPartList.SelectedIndex > 0 Then
				' mnWOJob.WOJobComps.CurrentItem.OffSerialNo = IIf(cmbOffSerialNo.SelectedIndex > 0, mnWOModelListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo, Trim(txtOffSerialNo.Text))
				If cmbOffSerialNo.SelectedIndex > 0 Then
					mnWOJob.WOJobComps.CurrentItem.OffSerialNo = mnWOModelListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo
				Else
					mnWOJob.WOJobComps.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
				End If
			Else
				mnWOJob.WOJobComps.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
			End If
			mnWOJob.WOJobComps.CurrentItem.OnPartID = New Guid(cmbOnPartList.SelectedValue.ToString)
			mnWOJob.WOJobComps.CurrentItem.OnPartNo = IIf(cmbOnPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, Trim(txtOnPartNo.Text))
			mnWOJob.WOJobComps.CurrentItem.OnDescription = Trim(txtOnDescription.Text) 'Commente on 01-jul-2020 by shital' IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Description, Trim(txtOnDescription.Text))
		End If
		'Added By Vikrant On 27-June-2013 For ALL27062013
		mnWOJob.WOJobComps.CurrentItem.OffPosition = Trim(txtOffPosition.Text)
		mnWOJob.WOJobComps.CurrentItem.OnPosition = Trim(txtOnPosition.Text)

		'Added By Saylee On 15-Oct-2020 For STR12102020
		mnWOJob.WOJobComps.CurrentItem.GRN = Trim(txtGRN.Text)
		mnWOJob.WOJobComps.CurrentItem.FormNo = Trim(txtFormNo.Text)
		'End


		If Session("IsFromMessageBox") = True Then
			Session("mnWOJob") = mnWOJob
		End If
		'End
	End Sub
	Private Sub SetControl(ByVal Index As Int32)
		chkIsAssembly.Checked = mnWOJob.WOJobComps.Item(Index).IsAssembly

		'  If chkIsAssembly.Checked = True Then
		Call chkIsAssembly_CheckedChanged(Nothing, Nothing)
		'  End If

		chkRemoval.Checked = mnWOJob.WOJobComps.Item(Index).IsForRemoval

		'OFF Part
		cmbOffPartList.SelectedValue = mnWOJob.WOJobComps.Item(Index).OffPartID.ToString
		txtOffPartNo.Text = mnWOJob.WOJobComps.Item(Index).OffPartNo
		txtOffDescription.Text = mnWOJob.WOJobComps.Item(Index).OffDescription
		txtOffSerialNo.Text = mnWOJob.WOJobComps.Item(Index).OffSerialNo
		txtOffRemark.Text = mnWOJob.WOJobComps.Item(Index).OffRemark
		cmbRemovalReason.SelectedValue = mnWOJob.WOJobComps.Item(Index).RemovalReasonID.ToString
		txtOffTSN.Text = mnWOJob.WOJobComps.Item(Index).OffTSN
		txtOffCSN.Text = mnWOJob.WOJobComps.Item(Index).OffCSN

		'ON Part
		chkInstallation.Checked = mnWOJob.WOJobComps.Item(Index).IsForInstall
		cmbOnPartList.SelectedValue = mnWOJob.WOJobComps.Item(Index).OnPartID.ToString
		txtOnPartNo.Text = mnWOJob.WOJobComps.Item(Index).OnPartNo()
		txtOnDescription.Text = mnWOJob.WOJobComps.Item(Index).OnDescription
		txtOnSerialNo.Text = mnWOJob.WOJobComps.Item(Index).OnSerialNo
		txtOnRemark.Text = mnWOJob.WOJobComps.Item(Index).OnRemark
		txtOnTSN.Text = mnWOJob.WOJobComps.Item(Index).OnTSN
		txtOnCSN.Text = mnWOJob.WOJobComps.Item(Index).OnCSN

		'Added By Vikrant On 27-June-2013 For ALL27062013
		txtOffPosition.Text = mnWOJob.WOJobComps.Item(Index).OffPosition
		txtOnPosition.Text = mnWOJob.WOJobComps.Item(Index).OnPosition
		'End

		'Added By Saylee On 15-Oct-2020 For STR12102020
		txtGRN.Text = mnWOJob.WOJobComps.Item(Index).GRN
		txtFormNo.Text = mnWOJob.WOJobComps.Item(Index).FormNo
		'End

		''Call cmbOffPartList_SelectedIndexChanged(Nothing, Nothing)
		OffPartSelection()
		OnPartSelection()
		cmbOffSerialNo.DataBind()
	End Sub
	Private Sub EditRecord(ByVal Index As Int32)
		mnWOJob.WOJobComps.CurrentIndex = Index
		SetControl(Index)
		setFocus(cmbOffPartList)
		Session("mnWOJob") = mnWOJob
		Session("JobCompEdit") = True
		dgRemovalInstallation.DataSource = mnWOJob.WOJobComps
	End Sub
	Private Sub DeleteRecord(ByVal Index As Int32)
		Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
		''msg1.ReplacePage = "wfnWOJobComp.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
		''Session("sender") = "Delete"
		''msg1.Show()
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
		mnWOJob.WOJobComps.CurrentIndex = Index
		Session("mnWO") = mnWO
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Function CustomValidate1() As Boolean
		Dim strMSG As String = ""
		If Not mnWO.IsValid Or Not mnWOJob.IsValid Then
			For i As Integer = 0 To mnWOJob.WOJobComps.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWOJob.WOJobComps.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		If ((mnWOJob.WOJobComps.Contains(mnWOJob.WOJobComps.CurrentItem, "") And mnWOJob.WOJobComps.CurrentItem.IsForRemoval = True)) Then
			strMSG = strMSG + "This Removal Entry is already done" + "<Br>"
		End If

		If ((mnWOJob.WOJobComps.Contains(mnWOJob.WOJobComps.CurrentItem, "", "") And mnWOJob.WOJobComps.CurrentItem.IsForInstall = True)) Then
			strMSG = strMSG + "This Installation Entry is already done" + "<Br>"
		End If


		'commented by Shital on 18-Jun-2020 (All18062020-Keep part No Serial No non-mandatory for all the job types in ON-OFF)
		'Added by Saylee on 11-July-2013 For ALL27062013
		'If (cmbOffPartList.SelectedIndex <= 0 And chkRemoval.Checked = True And mnWOJob.WOJobComps.CurrentItem.WOJobTypeID <> 1 And mnWOJob.WOJobComps.CurrentItem.WOJobTypeID <> 5) Then  'mnWOJob.WOJobComps.CurrentItem.WOJobTypeID <> 1 Added by Prashant on 15-Apr-2019 LAMA15042019 
		'    strMSG = strMSG + "Select the Part to be removed." + "<Br>"
		' End If
		''Added by Saylee on 11-July-2013 For ALL27062013
		'If cmbOnPartList.SelectedIndex <= 0 And chkInstallation.Checked = True And mnWOJob.WOJobComps.CurrentItem.WOJobTypeID <> 1 And mnWOJob.WOJobComps.CurrentItem.WOJobTypeID <> 5 Then  'mnWOJob.WOJobComps.CurrentItem.WOJobTypeID <> 1 Added by Prashant on 15-Apr-2019 LAMA15042019 
		'    strMSG = strMSG + "Select the Part to be installed." + "<Br>"
		' End If


		'Added by Shital on 18-jun-2020
		If chkIsAssembly.Checked = False Then
			If (cmbOffPartList.SelectedIndex <= 0 And txtOffPartNo.Text = "" And chkRemoval.Checked = True) Then
				strMSG = strMSG + "Select or enter the Part to be removed." + "<Br>"
			End If

			If cmbOnPartList.SelectedIndex <= 0 And txtOnPartNo.Text = "" And chkInstallation.Checked = True Then
				strMSG = strMSG + "Select or enter the Part to be installed." + "<Br>"
			End If
		Else
			If (cmbOffPartList.SelectedIndex <= 0 And txtOffPartNo.Text = "" And chkRemoval.Checked = True) Then
				strMSG = strMSG + "Please Enter or Select the Model to be removed." + "<Br>"
			End If

			If cmbOnPartList.SelectedIndex <= 0 And txtOnPartNo.Text = "" And chkInstallation.Checked = True Then
				strMSG = strMSG + "Please Enter or Select the Model to be installed." + "<Br>"
			End If
		End If
		If strMSG.Trim <> "" Then
			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False
			Return False
		End If
		Return True
	End Function
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "cmbOffPartList" Then
			'If cmbOffPartList.SelectedIndex <= 0 And chkRemoval.Checked = True Then
			If cmbOffPartList.SelectedIndex <= 0 And chkRemoval.Checked = True And (mnWO.WOJobTypeID <> 1 or mnWO.WOJobTypeID <> 5) Then  'mnWO.WOJobTypeID <> 1 Added by Prashant on 15-Apr-2019 LAMA15042019
				custValidator.ErrorMessage = "Select the Part to be removed."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "cmbOnPartList" Then
			'If cmbOnPartList.SelectedIndex <= 0 And chkInstallation.Checked = True Then
			If cmbOnPartList.SelectedIndex <= 0 And chkInstallation.Checked = True And (mnWO.WOJobTypeID <> 1 Or mnWO.WOJobTypeID <> 5) Then  'mnWO.WOJobTypeID <> 1 Added by Prashant on 15-Apr-2019 LAMA15042019
				custValidator.ErrorMessage = "Select the Part to be installed."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "chkRemoval" Or custValidator.ControlToValidate = "chkInstallation" Then
			If chkRemoval.Checked = False And chkInstallation.Checked = False Then
				custValidator.ErrorMessage = "Atleast select one Removal/Installation"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub
	'Added By Vikrant On 27-June-2013 For ALL27062013(Existing Code Added In Separate Function)
	Private Function Save() As Boolean
		If Session("mWOJobCompsEdit") = False Then
			If Session("IsFromMessageBox") = False Then
				mnWOJob.WOJobComps.Add(mnWOJob.ID, mnWOJob.WOJobTypeID)
				SetObject()
			End If
			If Not CustomValidate1() Then
				upnlValidationSummary.Update()
				mnWOJob.WOJobComps.Remove(mnWOJob.WOJobComps.CurrentItem)
				Session("mnWOJob") = mnWOJob
				Return False
			End If

			If (mnWOJob.WOJobComps.CurrentItem.IsValid) And ((Not mnWOJob.WOJobComps.Contains(mnWOJob.WOJobComps.CurrentItem, "") And mnWOJob.WOJobComps.CurrentItem.IsForRemoval = True)) Or ((Not mnWOJob.WOJobComps.Contains(mnWOJob.WOJobComps.CurrentItem, "", "") And mnWOJob.WOJobComps.CurrentItem.IsForInstall = True)) Then
				mnWOJob.ApplyEdit()
				''If Not CustomValidate1() Then Exit Sub
				dgRemovalInstallation.DataSource = mnWOJob.WOJobComps
				dgRemovalInstallation.DataBind()
				Session("mnWOJob") = mnWOJob
				''Response.Redirect("wfnWOJobComp.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
				SetTitle()
				If mnWO.WOStatusID = 3 Then
					ControlVisibility()
				End If
				upnlGrid.Update()
				upnlTitle.Update()
				ClearControls()
				Return True
			Else
				If Not CustomValidate1() Then
					upnlValidationSummary.Update()
					mnWOJob.WOJobComps.Remove(mnWOJob.WOJobComps.CurrentItem)
					Session("mnWOJob") = mnWOJob
					Return False
				End If
			End If
			SetEnability(mnWOJob.WOJobComps.CurrentItem.IsForInstall, mnWOJob.WOJobComps.CurrentItem.IsForRemoval)
		Else
			If Session("IsFromMessageBox") = False Then
				SetObject()
			End If

			If Not CustomValidate1() Then upnlValidationSummary.Update() : Return False

			'If Not CustomValidate1() Then upnlValidationSummary.Update() : Exit Sub
			If (mnWOJob.WOJobComps.CurrentItem.IsValid) And ((Not mnWOJob.WOJobComps.Contains(mnWOJob.WOJobComps.CurrentItem, "") And mnWOJob.WOJobComps.CurrentItem.IsForRemoval = True)) Or ((Not mnWOJob.WOJobComps.Contains(mnWOJob.WOJobComps.CurrentItem, "", "") And mnWOJob.WOJobComps.CurrentItem.IsForInstall = True)) Then
				dgRemovalInstallation.DataSource = mnWOJob.WOJobComps
				dgRemovalInstallation.DataBind()
				Session("mnWO") = mnWO
				setFocus(cmbOffPartList)
				Session("mWOJobCompsEdit") = False
				SetEnability(mnWOJob.WOJobComps.CurrentItem.IsForInstall, mnWOJob.WOJobComps.CurrentItem.IsForRemoval)
				'Response.Redirect("wfnWOJobComp_AJAX.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
				ClearControls()
				Return True
			Else
				If Not CustomValidate1() Then upnlValidationSummary.Update() : Return False
			End If
		End If
		'End
		Return False
	End Function
	Private Sub CallUpdatePanels()
		upnlInst.Update()
		upnlGrid.Update()
		upnlIsAssembly.Update()
		upnlRemoval.Update()
		upnlTitle.Update()
		upnlValidationSummary.Update()
	End Sub
	Private Sub ClearAllControls()
		cmbOnPartList.ClearSelection()
		txtOnPartNo.Text = ""
		txtOnDescription.Text = ""
		txtOnSerialNo.Text = ""
		txtOnRemark.Text = ""
		txtOnTSN.Text = ""
		txtOnCSN.Text = ""
		txtOnPosition.Text = ""
		txtGRN.Text = ""
		txtFormNo.Text = ""

		cmbOffPartList.ClearSelection()
		cmbRemovalReason.ClearSelection()
		cmbOffSerialNo.ClearSelection()
		txtOffPartNo.Text = ""
		txtOffDescription.Text = ""
		txtOffSerialNo.Text = ""
		txtOffRemark.Text = ""
		txtOffTSN.Text = ""
		txtOffCSN.Text = ""
		txtOffPosition.Text = ""
	End Sub
	Private Sub ClearControls()
		chkInstallation.Checked = True
		chkRemoval.Checked = True
		cmbOnPartList.Enabled = True
		txtOnPartNo.ReadOnly = False
		txtOnPartNo.Enabled = True
		txtOnDescription.ReadOnly = False
		txtOnDescription.Enabled = True

		txtOnSerialNo.ReadOnly = False
		txtOnSerialNo.Enabled = True

		txtOnRemark.ReadOnly = False
		txtOnRemark.Enabled = True

		txtOnTSN.ReadOnly = False
		txtOnTSN.Enabled = True

		txtOnCSN.ReadOnly = False
		txtOnCSN.Enabled = True

		txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.White)

		cmbOnPartList.ClearSelection()
		txtOnPartNo.Text = ""
		txtOnDescription.Text = ""
		txtOnSerialNo.Text = ""
		txtOnRemark.Text = ""
		txtOnTSN.Text = ""
		txtOnCSN.Text = ""
		txtOnPosition.Text = ""
		txtGRN.Text = ""
		txtFormNo.Text = ""


		cmbOffPartList.Enabled = True
		txtOffPartNo.ReadOnly = False
		txtOffDescription.ReadOnly = False
		txtOffDescription.Enabled = True

		txtOffSerialNo.ReadOnly = False
		cmbOffSerialNo.Enabled = True

		txtOffRemark.ReadOnly = False
		txtOffRemark.Enabled = True

		cmbRemovalReason.Enabled = True
		txtOffTSN.ReadOnly = False
		txtOffTSN.Enabled = True

		txtOffCSN.ReadOnly = False
		txtOffCSN.Enabled = True

		txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

		txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.White)
		cmbRemovalReason.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.White)
		txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.White)

		cmbOffPartList.ClearSelection()
		cmbRemovalReason.ClearSelection()
		cmbOffSerialNo.ClearSelection()
		txtOffPartNo.Text = ""
		txtOffDescription.Text = ""
		txtOffSerialNo.Text = ""
		txtOffRemark.Text = ""
		txtOffTSN.Text = ""
		txtOffCSN.Text = ""
		txtOffPosition.Text = ""
	End Sub
	Private Sub ControlVisibilityForStar()
		If mnWOJob.WOJobTypeID = 1 Or mnWOJob.WOJobTypeID = 5 Then
			Label1.Visible = False
			Label3.Visible = False
			Label4.Visible = False
			Label5.Visible = False
		Else
			Label1.Visible = True
			Label3.Visible = True
			Label4.Visible = True
			Label5.Visible = True
		End If
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack And Session("sender") = "" Then
			If cmbOffPartList.Enabled = True Then
				setFocus(cmbOffPartList)
			End If
			DataFieldBind()
			chkRemoval.Checked = True
			chkInstallation.Checked = True

			If Session("mWOJobCompsEdit") = True Then
				EditRecord(mnWOJob.WOJobComps.CurrentIndex)
				'dgRemovalInstallation.DataSource = mnWOJob.WOJobComps
				dgRemovalInstallation.DataBind()
				Session("IsInstall") = mnWOJob.WOJobComps.Item(mnWOJob.WOJobComps.CurrentIndex).IsForInstall
				Session("IsRemove") = mnWOJob.WOJobComps.Item(mnWOJob.WOJobComps.CurrentIndex).IsForRemoval
				SetEnability(mnWOJob.WOJobComps.Item(mnWOJob.WOJobComps.CurrentIndex).IsForInstall, mnWOJob.WOJobComps.Item(mnWOJob.WOJobComps.CurrentIndex).IsForRemoval)
			End If
		End If
		SetTitle()
		If mnWO.WOStatusID = 3 Then
			ControlVisibility()
		End If
		'commented by Shital on 18-Jun-2020 (All18062020-Keep part No Serial No non-mandatory for all the job types in ON-OFF)
		' ControlVisibilityForStar()
		'---
	End Sub
	Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTop.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not Page.IsValid() Then upnlValidationSummary.Update() : Exit Sub
		'Added By Vikrant On 27-June-2013 For ALL27062013
		If String.Compare(Trim(txtOffPosition.Text), Trim(txtOnPosition.Text), True) <> 0 And (chkInstallation.Checked = True And chkRemoval.Checked = True) Then
			Dim mnWOJobClone As nWOJob
			Dim mnWOClone As nWO
			mnWOJobClone = CType(mnWOJob.Clone, nWOJob)
			mnWOClone = CType(mnWO.Clone, nWO)
			Session("mnWOJobClone") = mnWOJobClone
			Session("IsFromMessageBox") = True
			If Session("mWOJobCompsEdit") = True Then
				SetObject()
			Else
				mnWOJob.WOJobComps.Add(mnWOJob.ID, mnWOJob.WOJobTypeID)
				SetObject()
			End If

			If Not CustomValidate1() Then 'Added by Saylee on 11-July-2013 For ALL27062013
				upnlValidationSummary.Update()
				mnWOJob.WOJobComps.Remove(mnWOJob.WOJobComps.CurrentItem)
				Session("mnWOJob") = mnWOJob
				Exit Sub
			End If

			'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Off Component Position is not Same as On Component Position." & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
			''msg1.ReplacePage = "wfnWOJobComp_AJAX.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
			''Session("sender") = "Confirm"
			''msg1.Show()
			MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Off Component Position is not Same as On Component Position." & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "Confirm")
			Exit Sub
		Else 'End
			Session("IsFromMessageBox") = False
			If Not Save() Then
				upnlValidationSummary.Update()
				Exit Sub
			Else
				chkIsRemoval()
				chkIsIntallation()
			End If

		End If
		DataFieldBind()
		If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobComps.Count.ToString + "');", True)
		SetTitle()
		If mnWO.WOStatusID = 3 Then
			ControlVisibility()
		End If
		CallUpdatePanels()
	End Sub
	Private Sub dgRemovalInstallation_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovalInstallation.RowCommand
		Dim Index As Int32 = dgRemovalInstallation.PageIndex * dgRemovalInstallation.PageSize + CInt(e.CommandArgument)
		Dim mID As Guid = mnWOJob.WOJobComps.Item(Index).ID
		Select Case e.CommandName
			Case "EditRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("mWOJobCompsEdit") = True
				EditRecord(Index)
				SetEnability(mnWOJob.WOJobComps.Item(Index).IsForInstall, mnWOJob.WOJobComps.Item(Index).IsForRemoval)

				If mnWO.WOStatusID = 3 Then
					ControlVisibility()
				End If

				If Session("mWOJobCompsEdit") = True Then
					chkIsAssembly.Enabled = False
					chkRemoval.Enabled = False
					chkInstallation.Enabled = False
				Else
					chkIsAssembly.Enabled = True
					chkRemoval.Enabled = True
					chkInstallation.Enabled = True
				End If
				upnlIsAssembly.Update()
				upnlInst.Update()
				upnlRemoval.Update()
				dgRemovalInstallation.DataBind()
			Case "DeleteRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				DeleteRecord(Index)
		End Select
	End Sub
	Private Sub cmbOffPartList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOffPartList.SelectedIndexChanged

		''If Session("mWOJobCompsEdit") = False Then
		''    txtOffSerialNo.Text = ""
		''End If

		If cmbOffPartList.SelectedIndex <= 0 Then
			cmbOffSerialNo.Enabled = False
			'cmbOffSerialNo.SelectedIndex = 0
			cmbOffSerialNo.ClearSelection()
			ComponentIndex = cmbOffPartList.SelectedIndex
			Session("ComponentIndex") = ComponentIndex


			txtOffPartNo.Text = ""
			txtOffDescription.Text = ""
			txtOffSerialNo.Enabled = True


			txtOffPartNo.ReadOnly = False
			txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOffDescription.ReadOnly = False
			txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
			txtOffDescription.ToolTip = "Enter Description for Removed Component"
			'New
			If chkInstallation.Checked Then
				cmbOnPartList.ClearSelection()
				txtOnPartNo.Text = ""
				txtOnDescription.Text = ""

				txtOnPartNo.ReadOnly = False
				txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

				txtOnDescription.ReadOnly = False
				txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)

				txtOnPartNo.ToolTip = "Enter Part Name for Installed Component"
				txtOnDescription.ToolTip = "Enter Description for Installed Component"
				upnlInst.Update()
			End If
			'End
		Else
			txtOffPartNo.ReadOnly = True
			txtOffPartNo.BackColor = Color.Gainsboro

			txtOffDescription.ReadOnly = True
			txtOffDescription.BackColor = Color.Gainsboro

			'New
			If chkInstallation.Checked Then
				txtOnPartNo.ReadOnly = True
				txtOnPartNo.BackColor = Color.Gainsboro

				txtOnDescription.ReadOnly = True
				txtOnDescription.BackColor = Color.Gainsboro
			End If
			'End
			If chkIsAssembly.Checked = False Then
				'COMPONENT
				mPartListForCombo = Session("mPartListForCombo")

				cmbOffSerialNo.Enabled = True

				ComponentName = cmbOffPartList.SelectedValue.ToString

				mPartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(mPartListForCombo(New Guid(ComponentName)).Name, "", Today.Date.ToString, , "(SELECT)")
				Session("mPartListForSerialNos") = mPartListForSerialNos

				txtOffPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Name, "")
				txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Description, "")

				txtOffPartNo.ToolTip = "Part Name for Removed Component"
				txtOffDescription.ToolTip = "Description for Removed Component"

				'New
				If chkInstallation.Checked Then
					cmbOnPartList.SelectedValue = cmbOffPartList.SelectedValue.ToString
					txtOnPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Name, "")
					txtOnDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Description, "")

					txtOnPartNo.ToolTip = "Part Name for Installed Component"
					txtOnDescription.ToolTip = "Description for Installed Component"
					cmbOnPartList.DataBind()
					txtOnPartNo.DataBind()
					txtOnDescription.DataBind()
					upnlInst.Update()
				End If

				'End
				If mPartListForSerialNos.Count > 1 Then
					If Not mPartListForSerialNos(1).SerialNo = "" Then
						cmbOffSerialNo.DataSource = mPartListForSerialNos
						cmbOffSerialNo.DataBind()
					Else
						cmbOffSerialNo.Items.Clear()
						cmbOffSerialNo.Items.Add("(SELECT)")
						cmbOffSerialNo.DataBind()
					End If
				Else
					cmbOffSerialNo.Items.Clear()
					cmbOffSerialNo.Items.Add("(SELECT)")
					cmbOffSerialNo.DataBind()
				End If

			Else
				''ASSEMBLY
				mnWOModelNameValueList = Session("mnWOModelNameValueList")

				cmbOffSerialNo.Enabled = True

				ComponentName = cmbOffPartList.SelectedValue.ToString
				mnWOModelListForSerialNos = nWOModelListForSerialNos.GetModelListForSerialNosList(mnWOModelNameValueList(New Guid(ComponentName)).Name, "", Today.Date.ToString, , "(SELECT)")

				txtOffPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(ComponentName)).Name, "")
				''txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, "", "")
				txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(ComponentName)).Description, "")

				txtOffPartNo.ToolTip = "Model Name for Removed Assembly"
				txtOffDescription.ToolTip = "Description for Removed Assembly"

				'New
				If chkInstallation.Checked Then
					cmbOnPartList.SelectedValue = cmbOffPartList.SelectedValue.ToString
					txtOnPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(ComponentName)).Name, "")
					txtOnDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(ComponentName)).Description, "")

					txtOnPartNo.ToolTip = "Part Name for Installed Component"
					txtOnDescription.ToolTip = "Description for Installed Component"
					cmbOnPartList.DataBind()
					txtOnPartNo.DataBind()
					txtOnDescription.DataBind()
					upnlInst.Update()
				End If
				'End

				If mnWOModelListForSerialNos.Count > 1 Then
					If Not mnWOModelListForSerialNos(1).SerialNo = "" Then
						cmbOffSerialNo.DataSource = mnWOModelListForSerialNos
						cmbOffSerialNo.DataBind()
					Else
						cmbOffSerialNo.Items.Clear()
						cmbOffSerialNo.Items.Add("(SELECT)")
						cmbOffSerialNo.DataBind()
					End If
				Else
					cmbOffSerialNo.Items.Clear()
					cmbOffSerialNo.Items.Add("(SELECT)")
					cmbOffSerialNo.DataBind()
				End If
			End If
			txtOffPartNo.DataBind()
			txtOffDescription.DataBind()
			Session("mnWOModelListForSerialNos") = mnWOModelListForSerialNos
			ComponentIndex = cmbOffPartList.SelectedIndex
			Session("ComponentIndex") = ComponentIndex
		End If
		cmbOffSerialNo_SelectedIndexChanged(sender, e)
	End Sub
	Private Sub cmbOffSerialNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOffSerialNo.SelectedIndexChanged
		If cmbOffSerialNo.SelectedIndex <= 0 Then
			txtOffSerialNo.Text = ""

			txtOffSerialNo.Enabled = True
			txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			If chkIsAssembly.Checked = False Then
				'COMPONENT
				txtOffSerialNo.ToolTip = "Enter Serial Number for Removed Component"
			Else
				'ASSEMBLY
				txtOffSerialNo.ToolTip = "Enter Serial Number for Removed Assembly"
			End If

		Else
			txtOffSerialNo.Enabled = True
			txtOffSerialNo.BackColor = Color.Gainsboro

			If chkIsAssembly.Checked = False Then
				'COMPONENT
				mPartListForSerialNos = Session("mPartListForSerialNos")
				txtOffSerialNo.Text = mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo

				txtOffSerialNo.ToolTip = "Serial Number for Removed Component"
			Else
				'ASSEMBLY
				mnWOModelListForSerialNos = Session("mnWOModelListForSerialNos")
				txtOffSerialNo.Text = mnWOModelListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo

				txtOffSerialNo.ToolTip = "Serial Number for Removed Assembly"
			End If
		End If

		txtOffSerialNo.DataBind()
	End Sub
	Private Sub cmbOnPartList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOnPartList.SelectedIndexChanged
		If cmbOnPartList.SelectedIndex <= 0 Then

			txtOnPartNo.Text = ""
			txtOnDescription.Text = ""
			txtOnSerialNo.Enabled = True


			txtOnPartNo.ReadOnly = False
			txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtOnDescription.ReadOnly = False
			txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)

		Else
			txtOnPartNo.ReadOnly = True
			txtOnPartNo.BackColor = Color.Gainsboro

			txtOnDescription.ReadOnly = True
			txtOnDescription.BackColor = Color.Gainsboro
		End If


		If chkIsAssembly.Checked = False Then
			'COMPONENT
			mPartListForCombo = Session("mPartListForCombo")
			txtOnPartNo.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, "")
			txtOnDescription.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Description, "")

			If cmbOnPartList.SelectedIndex <= 0 Then
				txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
				txtOffDescription.ToolTip = "Enter Description for Removed Component"
			Else
				txtOffPartNo.ToolTip = "Part Name for Removed Component"
				txtOffDescription.ToolTip = "Description for Removed Component"
			End If

		Else
			'ASSEMBLY
			mnWOModelNameValueList = Session("mnWOModelNameValueList")
			txtOnPartNo.Text = IIf(cmbOnPartList.SelectedIndex > 0, mnWOModelNameValueList(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, "")
			txtOnDescription.Text = IIf(cmbOnPartList.SelectedIndex > 0, "", "")

			If cmbOnPartList.SelectedIndex <= 0 Then
				txtOffPartNo.ToolTip = "Enter Model Name for Removed Assembly"
				txtOffDescription.ToolTip = "Enter Description for Removed Assembly"
			Else
				txtOffPartNo.ToolTip = "Model Name for Removed Assembly"
				txtOffDescription.ToolTip = "Description for Removed Assembly"
			End If

		End If
	End Sub
	Private Sub chkRemoval_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRemoval.CheckedChanged
		chkIsRemoval()
	End Sub
	Private Sub chkInstallation_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInstallation.CheckedChanged
		chkIsIntallation()
	End Sub
	Private Sub chkIsAssembly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsAssembly.CheckedChanged
		If chkIsAssembly.Checked = False Then
			''Off Part List
			mPartListForCombo = PartListForCombo.GetPartListForCombo(Guid.Empty, "", , , "(SELECT)")
			cmbOffPartList.DataSource = mPartListForCombo
			Session("mPartListForCombo") = mPartListForCombo

			'On Part List
			cmbOnPartList.DataSource = mPartListForCombo
			cmbOffSerialNo.Enabled = False
			'cmbOffSerialNo.SelectedIndex = 0
			cmbOffSerialNo.ClearSelection()
			''cmbOnPartList.Enabled = False
			''txtOnPartNo.ReadOnly = False
			''txtOnDescription.ReadOnly = False
			''txtOnSerialNo.ReadOnly = False

			''txtOnRemark.ReadOnly = False
			''txtOnTSN.ReadOnly = False
			''txtOnCSN.ReadOnly = False
		Else
			''Off Part List
			mnWOModelNameValueList = nWOModelNameValueList.GetModelList("(SELECT)", False)
			cmbOffPartList.DataSource = mnWOModelNameValueList
			Session("mnWOModelNameValueList") = mnWOModelNameValueList
			cmbOffSerialNo.ClearSelection()
			'On Part List
			cmbOnPartList.DataSource = mnWOModelNameValueList

		End If
		SetLabels(chkIsAssembly.Checked)
		cmbOffPartList.DataBind()
		cmbOnPartList.DataBind()

		txtOffPartNo.Text = ""
		txtOffDescription.Text = ""
		txtOffSerialNo.Text = ""
		txtOffRemark.Text = ""
		cmbRemovalReason.ClearSelection()
		txtOffTSN.Text = ""
		txtOffCSN.Text = ""

		txtOnPartNo.Text = ""
		txtOnDescription.Text = ""
		txtOnSerialNo.Text = ""
		txtOnRemark.Text = ""
		txtOnTSN.Text = ""
		txtOnCSN.Text = ""
		txtGRN.Text = ""
		txtFormNo.Text = ""

		upnlInst.Update()
		upnlRemoval.Update()
	End Sub
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		'AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
		SetSession()
		' mnWOJob.CancelEdit()
		Session.Remove("mWOJobCompsEdit")
		If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobComps.Count.ToString + "');", True)
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub

		ElseIf mopenas IsNot Nothing AndAlso mopenas = "childpup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage2") & "?CPage1=" & Request.QueryString("CPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&Index=-1")
	End Sub
	Private Sub imgReason_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReason.Click
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemovalReasonWindow", "OpenRemovalReasonWindow()", True)
	End Sub
	Private Sub hdnBtnRemovalReason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemovalReason.Click
		mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
		cmbRemovalReason.DataSource = mRemovalReasonList
		cmbRemovalReason.DataBind()
		If Session("mWOJobCompsEdit") = True Then
			If Not mnWOJob.WOJobComps.CurrentItem.RemovalReasonID.Equals(Guid.Empty) Then
				cmbRemovalReason.SelectedValue = mnWOJob.WOJobComps.CurrentItem.RemovalReasonID.ToString
			End If
		End If
		upnlRemoval.Update()
	End Sub
#End Region


End Class