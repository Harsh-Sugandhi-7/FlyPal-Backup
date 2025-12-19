Imports System.Linq
Public Class wfNRCSpare_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Protected mNRC As NRC
	Public PartNo As String
	Public Description As String
	Dim mFetchItemByName As FetchItemByName
	Public SparePartNo As String = ""
	Public SpareDescription As String = ""
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mNRC = Session("mNRC")
	End Sub
	Private Sub SetSession()
		Session("mNRC") = mNRC
	End Sub
	Public Function FetchItemByNameCount(Optional PartNo As String = "") As Object
		If (PartNo.Trim.IndexOf("[") > 0 And PartNo.Trim.IndexOf("]") > 0) Then
			SparePartNo = PartNo.Substring(0, PartNo.Trim.IndexOf("[")).Trim
			SpareDescription = Mid(PartNo.Trim, PartNo.Trim.IndexOf("[") + 2, PartNo.Trim.IndexOf("]") - PartNo.Trim.IndexOf("[") - 1).Trim
		Else
			SparePartNo = Trim(PartNo)
			SpareDescription = Trim(PartNo)
		End If
		mFetchItemByName = FetchItemByName.GetItemByName(SparePartNo)
		Return mFetchItemByName.Count
	End Function
	Private Function setObject() As Boolean
		mNRC.NRCSpares.CurrentItem.SrNo = mNRC.NRCSpares.CurrentIndex + 1
		If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
			SparePartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
			SpareDescription = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
		Else
			SparePartNo = Trim(txtSearch.Text)
			SpareDescription = Trim(txtSearch.Text)
		End If
		mNRC.NRCSpares.CurrentItem.PartNo = SparePartNo
		mNRC.NRCSpares.CurrentItem.Description = SpareDescription
		mFetchItemByName = FetchItemByName.GetItemByName(SparePartNo)
		If mFetchItemByName.Count > 0 Then
			mNRC.NRCSpares.CurrentItem.ItemID = mFetchItemByName(0).ID
		End If
		mNRC.NRCSpares.CurrentItem.RequiredQty = Val(txtReqQty.Text)
		mNRC.NRCSpares.CurrentItem.IsForBilling = chkIsForBilling.Checked
		mNRC.NRCSpares.CurrentItem.Remark = Trim(txtRemark.Text)
		mNRC.NRCSpares.CurrentItem.EffRate = Val(txtEffRate.Text)
		mNRC.NRCSpares.CurrentItem.EstimatedCost = Val(txtEstimatedCost.Text)


		mNRC.ApplyEdit()
		Return True
	End Function
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes

				Case MsgBoxResult.No

				Case MsgBoxResult.Ok

				Case MsgBoxResult.Ok And Session("sender") = "Authorization"

			End Select
		End If
	End Sub
	Private Sub ControlVisibility()

	End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		'custValidator.ControlToValidate = "txtsearch"


		If custValidator.ControlToValidate = "txtSearch" Then
			If txtSearch.Text = "" Then
				custValidator.ErrorMessage = "Enter Part No."
				e.IsValid = False
			ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
				custValidator.ErrorMessage = "Enter whole part no. and description."
				e.IsValid = False
			ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
				If FetchItemByNameCount(PartNo:=txtSearch.Text.Trim) = 0 Then
					custValidator.ErrorMessage = "Enter whole part no. and description."
					e.IsValid = False
				End If
			End If
		ElseIf custValidator.ControlToValidate = "txtReqQty" Then
			If Val(txtReqQty.Text) = 0 Then
				custValidator.ErrorMessage = "Required Quantity should be greater than 0."
				e.IsValid = False
			End If
			'----------------------------------------
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		If mNRC.NRCSpares.CurrentItem.PartNo <> "" And mNRC.NRCSpares.CurrentItem.Description <> "" Then
			txtSearch.Text = mNRC.NRCSpares.CurrentItem.PartNo + " [" + mNRC.NRCSpares.CurrentItem.Description + "]"
		End If
		DataBind()
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
		If Not IsPostBack Then
			txtSearch.Focus()
			DataFieldBind()
			ControlVisibility()
		End If
	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		If mNRC.NRCSpares.CurrentItem.IsNew And Not Session("EditSpare") = True Then mNRC.NRCSpares.Remove(mNRC.NRCSpares.CurrentItem)
		Session.Remove("EditSpare")
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub btnAddTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		If IsValid Then
			Dim clnNRC As NRC
			clnNRC = mNRC.Clone


			If setObject() Then
				If (mNRC.NRCSpares.Contains(mNRC.NRCSpares.CurrentItem.ItemID, "") And Not Session("EditSpare") = True And Session("EditSpare") IsNot Nothing) Then 'And mNRC.NRCSpares.Contains(cmbOffPartNo.SelectedItem.Text) Then
					mNRC = clnNRC
					Session("mNRC") = clnNRC
					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				ElseIf Session("EditSpare") = True Then
					If mNRC.NRCSpares.Contains(mNRC.NRCSpares.CurrentItem.ID, mNRC.NRCSpares.CurrentItem.NRCID, mNRC.NRCSpares.CurrentItem.ItemID) And mNRC.NRCSpares.Contains(mNRC.NRCSpares.CurrentItem.ItemID, "") Then
						mNRC = clnNRC
						Session("mNRC") = clnNRC
						MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
				End If
				Session.Remove("EditSpare")
				Session("mNRC") = mNRC
				Dim mopenas As String = Request.QueryString("Type")
				If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
					Exit Sub
				End If
			End If
		Else
			upnlSpareValidationSummary.Update()
		End If
	End Sub
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Protected Sub txtReqQty_TextChanged(sender As Object, e As EventArgs) Handles txtReqQty.TextChanged
		txtEstimatedCost.Text = Val(txtEffRate.Text) * Val(txtReqQty.Text)
		txtEffRate.Focus()
	End Sub
	Protected Sub txtEffRate_TextChanged(sender As Object, e As EventArgs) Handles txtEffRate.TextChanged
		txtEstimatedCost.Text = Val(txtEffRate.Text) * Val(txtReqQty.Text)
		txtEstimatedCost.Focus()
	End Sub
#End Region

#Region "Service Methods"
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim itemlist As ItemListAutoComplete
		itemlist = ItemListAutoComplete.GetItemList(prefixText, False)
		If count = 0 Then
			Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
		Else
			Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
		End If
	End Function
#End Region

End Class