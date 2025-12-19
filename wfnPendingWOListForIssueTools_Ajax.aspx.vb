'AJAX Conversion By Vikrant On 05-Nov-2014

Public Class wfnPendingWOListForIssueTools_Ajax
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mIssue As Issue
	Public mnPendingWOToolsListForIssue As nPendingWOToolsListForIssue
	Public mnPendingWOListForIssueTools As nPendingWOListForIssueTools
	Public mnPendingWOToolsListForIssueInfo As nPendingWOToolsListForIssue.nPendingWOToolsListForIssueInfo
	Public WOID As Guid
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mIssue = Session("mIssue")
		mnPendingWOToolsListForIssue = Session("mnPendingWOToolsListForIssue")
		mnPendingWOListForIssueTools = Session("mnPendingWOListForIssueTools")
	End Sub
	Private Sub SetSession()
		Session("mIssue") = mIssue
		Session("mnPendingWOToolsListForIssue") = mnPendingWOToolsListForIssue
		Session("mnPendingWOListForIssueTools") = mnPendingWOListForIssueTools
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mIssue")
	End Sub
	Private Sub SetObject(ByVal Index As Integer)

		mnPendingWOToolsListForIssue = Session("mnPendingWOToolsListForIssue")
		mnPendingWOToolsListForIssueInfo = mnPendingWOToolsListForIssue.Item(Index)
		mIssue.IssueItems.CurrentItem.WOReqPartID = mnPendingWOToolsListForIssueInfo.ID

		mIssue.IssueItems.CurrentItem.nWOPendingQty = mnPendingWOToolsListForIssueInfo.ToolsPendingIssuedQty
		'mIssue.IssueItems.CurrentItem.DisplayQty = mnPendingWOToolsListForIssueInfo.ToolsPendingIssuedQty

		mIssue.IssueItems.CurrentItem.ItemID = mnPendingWOToolsListForIssueInfo.ItemID

		mIssue.nWOID = mnPendingWOToolsListForIssueInfo.WOID
		mIssue.IssueTo = mnPendingWOToolsListForIssueInfo.WONumber
		Session("PartNo") = mnPendingWOToolsListForIssueInfo.PartNo
		Session("RequiredQty") = mnPendingWOToolsListForIssueInfo.RequiredQty
		Session("PendingIssuedQty") = mnPendingWOToolsListForIssueInfo.ToolsPendingIssuedQty
		Session("PendingIssuedQtyUnit") = mnPendingWOToolsListForIssueInfo.UnitID.ToString   'Added By Vikrant On 08-May-2019 For BA07052019
		Session("mIssue") = mIssue
	End Sub

#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mnPendingWOListForIssueTools = nPendingWOListForIssueTools.GetnPendingWOListForIssueTools(txtDate.Text, mIssue.nWOID.ToString)
		dgWOList.DataSource = mnPendingWOListForIssueTools
		Session("mnPendingWOListForIssueTools") = mnPendingWOListForIssueTools

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order as per criteria : " & mnPendingWOListForIssueTools.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "E.O. No."
			dgWOList.Columns(2).HeaderText = "E.O.Date"
		Else
			lblResult.Text = "List of W.O. as per criteria : " & mnPendingWOListForIssueTools.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "W.O. No."
			dgWOList.Columns(2).HeaderText = "W.O.Date"
		End If
		DataBind()
	End Sub

#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
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
		Else
			dgWOList.DataSource = mnPendingWOListForIssueTools
			dgWOList.DataBind()
		End If
	End Sub
	Private Sub dgWOList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select Case e.CommandName
			Case "Select"
				WOID = New Guid(dgWOList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
				mnPendingWOToolsListForIssue = nPendingWOToolsListForIssue.GetnPendingWOToolsListForIssue(WOID)
				dgToolsList.DataSource = mnPendingWOToolsListForIssue
				Session("mnPendingWOToolsListForIssue") = mnPendingWOToolsListForIssue
				dgToolsList.DataBind()
				'DataFieldBind()
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					lblResult1.Text = "List of spares For E.O. as per criteria :" & mnPendingWOToolsListForIssue.Count & " Record(s) found."
				Else
					lblResult1.Text = "List of Tools For W.O. as per criteria : " & mnPendingWOToolsListForIssue.Count & " Record(s) found."
				End If
				upnlToolsDetails.Update()
		End Select
	End Sub
	'------Added by Utkarsh 22-Dec-2010
	Private Sub dgWOList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting
		mnPendingWOListForIssueTools.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mnPendingWOListForIssueTools
		Session("mnPendingWOListForIssueTools") = mnPendingWOListForIssueTools
		dgWOList.DataBind()
	End Sub
	'----------------------------------
	Private Sub dgWOList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		lblResult1.Visible = True
		dgWOList.DataSource = mnPendingWOListForIssueTools
		mnPendingWOListForIssueTools = Session("mnPendingWOListForIssueTools")
		dgWOList.DataBind()
	End Sub
	Private Sub dgToolsList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgToolsList.RowCommand
		Select Case e.CommandName
			Case "Select"
				Dim Index As Integer = CInt(e.CommandArgument) + dgToolsList.PageIndex * dgToolsList.PageSize
				SetObject(index)
				Session("mIssue") = mIssue
				Response.Redirect("wfPartStockStatus_Ajax.aspx?ChildPage=wfIssueItem_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage1=wfnPendingWOListForIssueTools_Ajax.aspx" & "&Name=" & HttpUtility.UrlEncode(Session("PartNo")))
		End Select
	End Sub
	'------Added by Utkarsh 22-Dec-2010
	Private Sub dgToolsList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgToolsList.Sorting
		mnPendingWOToolsListForIssue.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgToolsList.DataSource = mnPendingWOToolsListForIssue
		Session("mnPendingWOToolsListForIssue") = mnPendingWOToolsListForIssue
		dgToolsList.DataBind()
	End Sub
	'----------------------------------
	Private Sub dgToolsList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgToolsList.PageIndexChanging
		dgToolsList.PageIndex = e.NewPageIndex
		dgToolsList.DataSource = mnPendingWOToolsListForIssue
		mnPendingWOToolsListForIssue = Session("mnPendingWOToolsListForIssue")
		dgToolsList.DataBind()
	End Sub
	Private Sub txtDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
		If mIssue.IsNew Then
			mIssue.IDate = CDate(txtDate.Text)
		End If
		dgWOList.PageIndex = 0
		mnPendingWOListForIssueTools = nPendingWOListForIssueTools.GetnPendingWOListForIssueTools(mIssue.IDate.ToString, mIssue.nWOID.ToString)
		dgWOList.DataSource = mnPendingWOListForIssueTools
		Session("mnPendingWOListForIssueTools") = mnPendingWOListForIssueTools
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order as per criteria : " & mnPendingWOListForIssueTools.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "E.O. No."
			dgWOList.Columns(2).HeaderText = "E.O.Date"
			dgWOList.DataBind()
		Else
			lblResult.Text = "List of W.O. as per criteria : " & mnPendingWOListForIssueTools.Count & " Record(s) found."
			dgWOList.Columns(1).HeaderText = "W.O. No."
			dgWOList.Columns(2).HeaderText = "W.O.Date"
			dgWOList.DataBind()
		End If
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



#End Region

End Class