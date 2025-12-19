Public Class wfSalesOrderCharge
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mSalesOrder As SalesOrder
	Public mSalesOrderCharge As SalesOrderCharge
	Private mChargeList As ChargeList
#End Region

#Region " Buisness Method And Properties "

	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub GetSession()
		mSalesOrder = Session("mSalesOrder")
		mChargeList = Session("mChargeList")
	End Sub
	Private Sub SetSession()
		Session("mSalesOrder") = mSalesOrder
		Session("mChargeList") = mChargeList
	End Sub
	Private Function Setobject() As Boolean
		mSalesOrder.BeginEdit()
		Dim Id As New Guid(cmbCharge.SelectedValue.ToString)
		If Not Id.Equals(Guid.Empty) Then
			mSalesOrder.SalesOrderCharges.CurrentItem.SrNo = mSalesOrder.SalesOrderCharges.CurrentIndex + 1
			mSalesOrder.SalesOrderCharges.CurrentItem.ChargeID = Id
			mSalesOrder.SalesOrderCharges.CurrentItem.ConversionFactor = mSalesOrder.ConversionFactor
			mSalesOrder.SalesOrderCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
			mSalesOrder.SalesOrderCharges.CurrentItem.ConversionFactor = mSalesOrder.ConversionFactor
			mSalesOrder.SalesOrderCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
			If mSalesOrder.SalesOrderItems.Count > 0 Then
				mSalesOrder.SalesOrderCharges.CurrentItem.BasicAmount = mSalesOrder.SalesOrderItems.CTotalAmount                                                        'dated: 21-11-2005    
				''  mSalesOrder.SalesOrderCharges.CurrentItem.TotalAmount = mSalesOrder.SalesOrderItems.CTotalAmount + mSalesOrder.SalesOrderCharges.CGrandTotalAmountCharges    'dated: 21-11-2005
			End If
			If mSalesOrder.SalesOrderCharges.Contains(mSalesOrder.SalesOrderCharges.CurrentItem) Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, " SalesOrder Charge.", MsgBoxStyle.OkOnly)
				msg1.ReplacePage = "wfSalesOrderCharge.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
				msg1.Show()
				mSalesOrder.CancelEdit()
				Exit Function
			Else
				mSalesOrder.ApplyEdit()
				mSalesOrder.CalculateTotal()            'Added By Saylee on 10-Sep-2007
				If mSalesOrder.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
					mSalesOrder.RoundCGrandTotal()
				End If
				Return True
			End If

			txtPercentage.DataBind()
			txtChargeAmount.DataBind()
			Session("mSalesOrder") = mSalesOrder
		Else
			mSalesOrder.CancelEdit()
		End If
	End Function
	Private Sub addAttributes()
		txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
		txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
	End Sub
	Private Sub setControl(ByVal Index As Int32)
		txtPercentage.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 3)
		txtChargeAmount.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 1)
		txtPercentage.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, mChargeList(Index).Percentage)
		txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, txtChargeAmount.Text, 0)
		txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
		txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
		txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, txtChargeAmount.Text)
		'Setobject()
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
			Result1 = -1
		Else
			Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		End If

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If CType(Session("sender"), String) = "Delete" Then
						Try
							'Session("Sender") = ""
							'Dim mSalesOrder As SalesOrder
							'mSalesOrder = CType(Session("mSalesOrder"), SalesOrder)
							'mSalesOrder.SalesOrderItems.RemoveAt(mSalesOrder.SalesOrderItems.CurrentIndex)
							'Session("mSalesOrder") = mSalesOrder
							'Response.Redirect("wfSalesOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
						Catch ex As SqlException
							If ex.Number = 8145 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfSalesOrderCharge.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							ElseIf ex.Number = 2627 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfSalesOrderCharge.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							ElseIf ex.Number = 547 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfSalesOrderCharge.aspx?BackPage=" & Request.QueryString("BackPage")
								msg1.Show()
							End If
						End Try
					End If
				Case MsgBoxResult.No
					Session("Sender") = ""
					Response.Redirect("wfSalesOrderCharge.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
					Session("sender") = ""
					DataFieldBind()
					Response.Redirect("wfSalesOrderCharge.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					Session("sender") = ""
					DataFieldBind()
					Response.Redirect("wfSalesOrderCharge.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			Response.Redirect("wfSalesOrderCharge.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			DataFieldBind()
		End If
	End Sub
#End Region

#Region " Binding Methods "
	Private Sub GetList()
		mChargeList = ChargeList.GetChargeList("", -1, True)
		Session("mChargeList") = mChargeList
	End Sub
	Public Sub DataFieldBind()
		cmbCharge.DataSource = mChargeList
		txtPercentage.DataBind()
		txtChargeAmount.DataBind()
		DataBind()
		If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mSalesOrder.SalesOrderCharges.CurrentItem.ChargeName, mSalesOrder.SalesOrderCharges.CurrentItem.ChargeID.ToString)) Then
			cmbCharge.SelectedValue = mSalesOrder.SalesOrderCharges.CurrentItem.ChargeID.ToString
		Else
			cmbCharge.SelectedValue = Guid.Empty.ToString
		End If
		If Session("EditCharge") Then
			If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mSalesOrder.SalesOrderCharges.CurrentItem.ChargeName, mSalesOrder.SalesOrderCharges.CurrentItem.ChargeID.ToString)) Then 'Added ByRajnish On 09-01-2008
				Dim mCharge As Charge = Charge.GetCharge(mSalesOrder.SalesOrderCharges.CurrentItem.ChargeID)
				txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
				txtPercentage.ToolTip = "Percentage"
				txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
				txtChargeAmount.ToolTip = "Charge Amount"
				txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
				txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
			End If
		End If
	End Sub
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim CustValidator As CustomValidator
		Dim Index As Int32 = IIf(cmbCharge.SelectedIndex <= 0, 0, cmbCharge.SelectedIndex)
		CustValidator = CType(s, CustomValidator)
		If CustValidator.ControlToValidate = "cmbCharge" Then
			If cmbCharge.SelectedIndex = 0 Then
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
		If txtPercentage.Enabled = True Then
			If CustValidator.ControlToValidate = "txtPercentage" Then
				If IsNumeric(txtPercentage.Text) Then
					If CDbl(txtPercentage.Text) <= 0 And mChargeList(Index).PercentageTypeID = 3 Then
						e.IsValid = False
					Else
						e.IsValid = True
					End If
				Else
					e.IsValid = False
				End If
			End If
		End If
		If CustValidator.ControlToValidate = "txtChargeAmount" Then
			If IsNumeric(txtChargeAmount.Text) Then
				If CDbl(txtChargeAmount.Text) <= 0 And mChargeList(Index).PercentageTypeID = 1 Then
					e.IsValid = False
				Else
					e.IsValid = True
				End If
			Else
				e.IsValid = False
			End If
		End If
	End Sub
#End Region

#Region " Events "

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		addAttributes()
		If Not IsPostBack And Session("sender") = "" Then
			If cmbCharge.Enabled = True Then
				setFocus(cmbCharge)
			End If
			GetList()
			DataFieldBind()
		End If
		If Session("Edit") Then
			lblTitle.Text = "Sales Order Charge [ " & mSalesOrder.SalesOrderCharges.CurrentItem.ChargeName & " ]"
		Else
			lblTitle.Text = "Sales Order Charge [ New ]"
		End If
		Session("mSalesOrder") = mSalesOrder
		MessageBoxResult()
	End Sub
	Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
		''If Not (User.IsInRole("SalesOrderNew") And User.IsInRole("SalesOrderEdit") And User.IsInRole("SalesOrderDelete")) Then
		''    Setobject()
		''    SetSession()
		''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
		''    msg.ReplacePage = "wfSalesOrderCharge.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
		''    Session("sender") = "Authorization"
		''    msg.Show()
		''    Exit Sub
		''End If
		'' Response.Redirect("wfCharge.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfSalesOrderCharge.aspx")
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenChargeWindow", "OpenChargeWindow();", True)
	End Sub
	Private Sub cmbCharge_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCharge.SelectedIndexChanged
		Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
		setControl(Index)
		If cmbCharge.Enabled = True Then
			setFocus(cmbCharge)
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If mSalesOrder.SalesOrderCharges.CurrentItem.IsNew And Not Session("EditCharge") = True Then
			mSalesOrder.SalesOrderCharges.Remove(mSalesOrder.SalesOrderCharges.CurrentItem)
		End If
		Session.Remove("EditCharge")
		' Response.Redirect("wfSalesOrder_Ajax.aspx")
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		If IsValid Then
			'=============Commented by Saylee on 5th-Feb-2008 suggested by Kalpesh Sir.
			'Dim Id As New Guid(cmbCharge.SelectedValue)
			'If mSalesOrder.SalesOrderCharges.CurrentItem.IsNew And Not Session("EditCharge") Then
			'    mSalesOrder.SalesOrderCharges.Remove(mSalesOrder.SalesOrderCharges.CurrentItem)
			'    mSalesOrder.SalesOrderCharges.Add(Id)
			'End If
			'================Commented Code End============================================
			Setobject()
			Session.Remove("EditCharge")
			' Response.Redirect("wfSalesOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			Dim mopenas As String = Request.QueryString("Type")
			If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
				Exit Sub
			End If
			'End If
		End If
	End Sub
	Private Sub hdnimgBtnChargeList_Click(sender As Object, e As EventArgs) Handles hdnimgBtnChargeList.Click
		mChargeList = ChargeList.GetChargeList("", -1, True)
		Session("mChargeList") = mChargeList
		cmbCharge.DataSource = mChargeList
		cmbCharge.DataBind()
		upnlOtherChargeDetails.Update()
	End Sub
#End Region

End Class