Public Class wfNRCPartOnOff_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Description "
	Public mNRC As NRC
	Protected mItemList As ItemList
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mNRC = Session("mNRC")
		mItemList = Session("mItemList")
	End Sub
	Private Sub setSession()
		Session("mNRC") = mNRC
	End Sub
	Private Sub RemoveSession()
		Session.Remove("Edit")
		Session.Remove("mItemList")
	End Sub
	Private Sub DataFieldBind()
		mItemList = ItemList.GetItemsList(0, IsSelectTagRequired:=True)
		cmbOffPartNo.DataSource = mItemList
		cmbOnPartNo.DataSource = mItemList
		Session("mItemList") = mItemList
		DataBind()
	End Sub
	Private Function setObject() As Boolean
		mNRC.NRCPartOnOffs.CurrentItem.SrNo = mNRC.NRCPartOnOffs.CurrentIndex + 1

		mNRC.NRCPartOnOffs.CurrentItem.OffPartID = New Guid(cmbOffPartNo.SelectedValue)
		mNRC.NRCPartOnOffs.CurrentItem.OffPartName = cmbOffPartNo.SelectedItem.Text
		mNRC.NRCPartOnOffs.CurrentItem.OffPartDescription = Trim(txtOffPartDescription.Text)
		mNRC.NRCPartOnOffs.CurrentItem.OffPartSerialNo = Trim(txtOffPartSerialNo.Text)

		mNRC.NRCPartOnOffs.CurrentItem.OnPartID = New Guid(cmbOnPartNo.SelectedValue)
		mNRC.NRCPartOnOffs.CurrentItem.OnPartName = IIf(cmbOnPartNo.SelectedIndex > 0, cmbOnPartNo.SelectedItem.Text, "")
		mNRC.NRCPartOnOffs.CurrentItem.OnPartDescription = Trim(txtOnPartDescription.Text)
		mNRC.NRCPartOnOffs.CurrentItem.OnPartSerialNo = Trim(txtOnPartSerialNo.Text)

		mNRC.NRCPartOnOffs.CurrentItem.ReleaseNoteNo = Trim(txtReleaseNoteNo.Text)
		mNRC.ApplyEdit()
		Return True
	End Function
	Private Sub addAttributes()
		'txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value)")
	End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "cmbOffPartNo" Then
			If cmbOffPartNo.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Select Part No. from list"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub
	Private Sub ControlVisibility()
		txtOffPartDescription.Enabled = False
		txtOffPartDescription.BackColor = Color.Gainsboro
		txtOnPartDescription.Enabled = False
		txtOnPartDescription.BackColor = Color.Gainsboro
	End Sub
#End Region

#Region " Events "
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		getSession()
		addAttributes()
		If Not IsPostBack Then
			cmbOffPartNo.Focus()
			DataFieldBind()
			ControlVisibility()
		End If
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		If IsValid Then
			'If mNRC.NRCPartOnOffs.CurrentItem.IsNew And Not Session("Edit") = True Then 'And mNRC.NRCPartOnOffs.Contains(cmbOffPartNo.SelectedItem.Text) Then
			'    Session("Duplicate") = "Duplicate"
			'    Session("ToCleareList") = "ToCleareList"
			'    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
			'    Exit Sub
			'End If

			If setObject() Then
				Session("mNRC") = mNRC
				RemoveSession()
				Dim mopenas As String = Request.QueryString("Type")
				If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
					Exit Sub
				End If
			End If
		Else
			upnlValidationSummary.Update()
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If mNRC.NRCPartOnOffs.CurrentItem.IsNew And Not Session("Edit") = True Then mNRC.NRCPartOnOffs.Remove(mNRC.NRCPartOnOffs.CurrentItem)
		RemoveSession()
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub cmbOffPartNo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbOffPartNo.SelectedIndexChanged
		If cmbOffPartNo.SelectedIndex = 0 Then
			txtOffPartDescription.Text = ""
		Else
			txtOffPartDescription.Text = mItemList(cmbOffPartNo.SelectedIndex).Description
		End If
		ControlVisibility()
	End Sub
	Private Sub cmbOnPartNo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbOnPartNo.SelectedIndexChanged
		If cmbOnPartNo.SelectedIndex = 0 Then
			txtOnPartDescription.Text = ""
		Else
			txtOnPartDescription.Text = mItemList(cmbOnPartNo.SelectedIndex).Description
		End If
	End Sub
#End Region

End Class