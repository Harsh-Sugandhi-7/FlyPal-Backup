'Created by : Saylee
'Dated      : 29-Jul-2022
Public Class wfnPendingWOListForInvoice_Ajax
	Inherits Page

#Region " Variable Declaration "

	Public mnPendingWOListForInvoice As nPendingWOListForInvoice
	Public WOID As Guid
	Public mWOInvoice As WOInvoice
	Dim mCustomerList As VendorList
	Dim mnWOListForCombo As nWOListForCombo
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mnPendingWOListForInvoice = Session("mnPendingWOListForInvoice")
		mWOInvoice = Session("mWOInvoice")
	End Sub

	Private Sub SetSession()
		Session("mnPendingWOListForInvoice") = mnPendingWOListForInvoice
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mIssue")
		Session.Remove("mWOInvoice")
	End Sub

	Private Sub SetTitle()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order as per criteria : " & mnPendingWOListForInvoice.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "E.O. No."
			dgWOList.Columns(2).HeaderText = "E.O.Date"
		Else
			lblResult.Text = "List of W.O as per criteria : " & mnPendingWOListForInvoice.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "W.O No."
			dgWOList.Columns(2).HeaderText = "W.O.Date"
		End If
	End Sub

	Private Sub GetPendingWOList()

		Try
			mnPendingWOListForInvoice = nPendingWOListForInvoice.GetnPendingWOListForInvoice(mWOInvoice.Date.ToString,
																							 cmbWorkOrder.SelectedValue.ToString,
																							 cmbCustomerList.SelectedValue.ToString)
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()
		txtDate.Text = CDate(mWOInvoice.Date).ToString(AppSettings("DateFormat"))
		mnPendingWOListForInvoice = nPendingWOListForInvoice.GetnPendingWOListForInvoice(txtDate.Text, mWOInvoice.WOID.ToString)
		Session("mnPendingWOListForInvoice") = mnPendingWOListForInvoice
		dgWOList.DataSource = mnPendingWOListForInvoice
		upnlWODetails.DataBind()
		mCustomerList = VendorList.GetVendorstList(0, , , , , , "(SELECT)", True)
		cmbCustomerList.DataSource = mCustomerList
		cmbCustomerList.DataBind()
		mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)")
		cmbWorkOrder.DataSource = mnWOListForCombo
		Session("mnWOListForCombo") = mnWOListForCombo
		cmbWorkOrder.DataBind()
	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then
			If txtDate.Text = "" Then
				txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			End If
			DataFieldBind()
			If mWOInvoice.WOInvoiceJobs.Count = 0 Then
				txtDate.Enabled = True
			Else
				txtDate.Enabled = False
			End If
			SetTitle()
		Else
			dgWOList.DataSource = mnPendingWOListForInvoice
			dgWOList.DataBind()
		End If
	End Sub

	Private Sub GridViewSorting(source As Object, e As GridViewSortEventArgs) Handles dgWOList.Sorting
		mnPendingWOListForInvoice.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mnPendingWOListForInvoice
		Session("mnPendingWOListForInvoice") = mnPendingWOListForInvoice
		dgWOList.DataBind()
	End Sub

	Private Sub GridViewPageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		lblResult.Visible = True
		dgWOList.DataSource = mnPendingWOListForInvoice
		mnPendingWOListForInvoice = Session("mnPendingWOListForInvoice")
		dgWOList.DataBind()
	End Sub

	Private Sub GridViewRowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select Case e.CommandName
			Case "Select"
				mnPendingWOListForInvoice = Session("mnPendingWOListForInvoice")
				WOID = New Guid(dgWOList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
				mWOInvoice = WOInvoice.NewWOInvoice(WOID)
				mWOInvoice.WONo = mnPendingWOListForInvoice(WOID).WONo
				mWOInvoice.WOText = mnPendingWOListForInvoice(WOID).WOText
				If txtDate.Text.ToString <> "" Then
					mWOInvoice.Date = CDate(txtDate.Text)
				Else
					mWOInvoice.Date = DBNull.Value
				End If
				Session("mWOInvoice") = mWOInvoice
				SetTitle()
				Response.Redirect("wfnWOInvoice_Ajax.aspx?ChildPage=wfnPendingWOListForInvoice_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
		End Select
	End Sub

	Private Sub DateChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
		If mWOInvoice.IsNew Then
			mWOInvoice.Date = CDate(txtDate.Text)
		End If
		dgWOList.PageIndex = 0
		GetPendingWOList()
		Session("mnPendingWOListForInvoice") = mnPendingWOListForInvoice
		dgWOList.DataSource = mnPendingWOListForInvoice
		dgWOList.DataBind()
		SetTitle()
	End Sub

	Private Sub SearchResult(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click
		If mWOInvoice.IsNew Then
			mWOInvoice.Date = CDate(txtDate.Text)
		End If
		dgWOList.PageIndex = 0
		GetPendingWOList()
		Session("mnPendingWOListForInvoice") = mnPendingWOListForInvoice
		dgWOList.DataSource = mnPendingWOListForInvoice
		dgWOList.DataBind()
		SetTitle()
	End Sub

	Private Sub Back(sender As Object, e As EventArgs) Handles btnBack.Click
		If Request.QueryString("BackPage") = "wfIssue_Ajax.aspx" Then
			mWOInvoice.WOInvoiceJobs.RemoveAt(mWOInvoice.WOInvoiceJobs.CurrentIndex)
			Session("Edit") = False
			Response.Redirect(Request.QueryString("BackPage"))
		Else
			Response.Redirect("Index.aspx")
		End If
	End Sub

#End Region

End Class