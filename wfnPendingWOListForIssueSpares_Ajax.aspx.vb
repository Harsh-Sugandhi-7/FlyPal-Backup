'AJAX Conversion By Vikrant On 05-Nov-2014

Public Class wfnPendingWOListForIssueSpares_Ajax
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mIssue As Issue
	Public mnPendingWOSpareListForIssue As nPendingWOSpareListForIssue
	Public mnPendingWOListForIssueSpares As nPendingWOListForIssueSpares
	Public mnPendingWOSpareListForIssueInfo As nPendingWOSpareListForIssue.nPendingWOSpareListForIssueInfo
	Public WOID As Guid
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mIssue = Session("mIssue")
		mnPendingWOSpareListForIssue = Session("mnPendingWOSpareListForIssue")
		mnPendingWOListForIssueSpares = Session("mnPendingWOListForIssueSpares")
	End Sub
	Private Sub setSession()
		Session("mIssue") = mIssue
		Session("mnPendingWOSpareListForIssue") = mnPendingWOSpareListForIssue
		Session("mnPendingWOListForIssueSpares") = mnPendingWOListForIssueSpares
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mIssue")
	End Sub
	Public Sub setObject(ByVal Index As Integer)

		mnPendingWOSpareListForIssue = Session("mnPendingWOSpareListForIssue")
		mnPendingWOSpareListForIssue = Session("mnPendingWOSpareListForIssue")

		mnPendingWOSpareListForIssueInfo = mnPendingWOSpareListForIssue.Item(Index)

		mIssue.IssueItems.CurrentItem.nWOPendingQty = mnPendingWOSpareListForIssueInfo.PendingIssuedQty
		'mIssue.IssueItems.CurrentItem.DisplayQty = mnPendingWOSpareListForIssueInfo.PendingIssuedQty

		mIssue.IssueItems.CurrentItem.ItemID = mnPendingWOSpareListForIssueInfo.ItemID
		mIssue.nWOID = mnPendingWOSpareListForIssueInfo.WOID
		mIssue.IssueTo = mnPendingWOSpareListForIssueInfo.WONumber
		mIssue.IssueItems.CurrentItem.WOReqPartID = mnPendingWOSpareListForIssueInfo.ID
		mIssue.ToTypeID = 17 'WorkOrder

		'Added By Utkarsh On 04-Jul-2012 FOR ALL04072012
		mIssue.MachineID = mnPendingWOSpareListForIssueInfo.MachineID
		mIssue.RegNo = mnPendingWOSpareListForIssueInfo.RegNo
		'End

		Session("PartNo") = mnPendingWOSpareListForIssueInfo.PartNo
		Session("RequiredQty") = mnPendingWOSpareListForIssueInfo.RequiredQty
		Session("PendingIssuedQty") = mnPendingWOSpareListForIssueInfo.PendingIssuedQty
		Session("PendingIssuedQtyUnit") = mnPendingWOSpareListForIssueInfo.UnitID.ToString  'Added By Vikrant On 08-May-2019 For BA07052019
		Session("mIssue") = mIssue
	End Sub
	Private Sub SetTitle()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order as per criteria : " & mnPendingWOListForIssueSpares.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "E.O. No."
			dgWOList.Columns(2).HeaderText = "E.O.Date"
			'' dgWOList.DataBind()
		Else
			lblResult.Text = "List of W.O. as per criteria :" & mnPendingWOListForIssueSpares.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "W.O. No."
			dgWOList.Columns(2).HeaderText = "W.O.Date"
			'' dgWOList.DataBind()
		End If
	End Sub

#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		txtDate.Text = CDate(mIssue.IDate).ToString(AppSettings("DateFormat"))
		mnPendingWOListForIssueSpares = nPendingWOListForIssueSpares.GetnPendingWOListForIssueSpares(txtDate.Text, mIssue.nWOID.ToString)
		Session("mnPendingWOListForIssueSpares") = mnPendingWOListForIssueSpares
		dgWOList.DataSource = mnPendingWOListForIssueSpares
		upnlWODetails.DataBind()
	End Sub
#End Region

#Region "Events"

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		getSession()
		If Not IsPostBack Then
			If txtDate.Text = "" Then
				txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			End If
			DataFieldBind()
			If mIssue.IssueItems.Count - 1 = 0 Then
				txtDate.Enabled = True
			Else
				txtDate.Enabled = False
			End If
			SetTitle()
		Else
			dgWOList.DataSource = mnPendingWOListForIssueSpares
			dgWOList.DataBind()
		End If
	End Sub

	Private Sub dgWOList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select Case e.CommandName
			Case "Select"
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOList.PageIndex * dgWOList.PageSize
				WOID = New Guid(dgWOList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
				mnPendingWOSpareListForIssue = nPendingWOSpareListForIssue.GetnPendingWOSpareListForIssue(WOID)
				Session("mnPendingWOSpareListForIssue") = mnPendingWOSpareListForIssue
				dgSparesList.DataSource = mnPendingWOSpareListForIssue
				dgSparesList.DataBind()
				SetTitle()
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					lblResult1.Text = "List of spares For E.O. as per criteria :" & mnPendingWOSpareListForIssue.Count & " Record(s) found."
				Else
					lblResult1.Text = "List of spares For W.O. as per criteria :" & mnPendingWOSpareListForIssue.Count & " Record(s) found."
				End If
				upnlSparesDetails.Update()
		End Select
	End Sub
	Private Sub txtDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
		If mIssue.IsNew Then
			mIssue.IDate = CDate(txtDate.Text)
		End If
		dgWOList.PageIndex = 0
		mnPendingWOListForIssueSpares = nPendingWOListForIssueSpares.GetnPendingWOListForIssueSpares(mIssue.IDate.ToString)
		Session("mnPendingWOListForIssueSpares") = mnPendingWOListForIssueSpares
		dgWOList.DataSource = mnPendingWOListForIssueSpares
		dgWOList.DataBind() 'Added By Prashant 20-Aug-2014 ALL20082014
		SetTitle()
	End Sub

	Private Sub dgSparesList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSparesList.PageIndexChanging
		dgSparesList.PageIndex = e.NewPageIndex
		dgSparesList.DataSource = mnPendingWOSpareListForIssue
		mnPendingWOSpareListForIssue = Session("mnPendingWOSpareListForIssue")
		dgSparesList.DataBind()
	End Sub
	Private Sub dgSparesList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSparesList.RowCommand
		Select Case e.CommandName
			Case "Select"
				Dim Index As Integer = CInt(e.CommandArgument) + dgSparesList.PageIndex * dgSparesList.PageSize
				setObject(index)
				Session("mIssue") = mIssue
				' Session("PartNo") = txtPartNo.Text
				Response.Redirect("wfPartStockStatus_Ajax.aspx?ChildPage=wfIssueItem_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage1=wfnPendingWOListForIssueSpares_Ajax.aspx" & "&Name=" & HttpUtility.UrlEncode(Session("PartNo")))
		End Select
	End Sub
	Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If Request.QueryString("BackPage") = "wfIssue_Ajax.aspx" Then
			mIssue.IssueItems.RemoveAt(mIssue.IssueItems.CurrentIndex)
			Session("Edit") = False
			Response.Redirect(Request.QueryString("BackPage"))
		Else
			Response.Redirect("Index.aspx")
		End If
	End Sub
	'------Added by Utkarsh 22-Dec-2010
	Private Sub dgWOList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting
		mnPendingWOListForIssueSpares.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mnPendingWOListForIssueSpares
		Session("mnPendingWOListForIssueSpares") = mnPendingWOListForIssueSpares
		dgWOList.DataBind()
	End Sub
	'----------------------------------
	'------Added by Utkarsh 22-Dec-2010
	Private Sub dgSparesList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSparesList.Sorting
		mnPendingWOSpareListForIssue.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgSparesList.DataSource = mnPendingWOSpareListForIssue
		Session("mnPendingWOSpareListForIssue") = mnPendingWOSpareListForIssue
		dgSparesList.DataBind()
	End Sub
	'----------------------------------
	'------Added by Saylee 11-Jan-2011
	Private Sub dgWOList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		lblResult1.Visible = True
		dgWOList.DataSource = mnPendingWOListForIssueSpares
		mnPendingWOListForIssueSpares = Session("mnPendingWOListForIssueSpares")
		dgWOList.DataBind()
	End Sub
#End Region

End Class