'AJAX Conversion By Vikrant On 08-July-2014

Public Class wfPartStockStatusListForEnquiry_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration"
	Dim mItemStockStatusList As ItemStockStatusList
	Public mEnquiry As Enquiry
	Dim PartNo As String
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mItemStockStatusList = Session("mItemStockStatusList")
		mEnquiry = Session("mEnquiry")
		PartNo = Session("PartNo")
	End Sub
	Private Sub setSession()
		Session("mItemStockStatusList") = mItemStockStatusList
		Session("mEnquiry") = mEnquiry
	End Sub
	Private Sub setObject(ByVal ItemId As Guid)
		mEnquiry.EnquiryItems.CurrentItem.ItemID = ItemId
		mEnquiry.EnquiryItems.CurrentItem.Qty = 0
		mEnquiry.EnquiryItems.CurrentItem.Remark = ""
		mEnquiry.EnquiryItems.CurrentItem.Note = ""
		Session("mEnquiry") = mEnquiry
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
				Case MsgBoxResult.No
					Session("Sender") = ""
				Case Else
					Session("Sender") = ""
			End Select
		ElseIf Result1 = -1 Then
			Session("Sender") = ""
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mItemStockStatusList = ItemStockStatusList.GetItemStockStatusList(txtSearch.Text.Trim, mEnquiry.Date.ToString) 'mEnquiry.Date.ToString Added by Prashant 19-Feb-2013 All19022013
		Session("mItemStockStatusList") = mItemStockStatusList
		dgPartStockStatusList.DataSource = mItemStockStatusList
		DataBind()
	End Sub
	'Added by Vikrant On 11-Jul-2019 For ALL11072019	
	Private Sub ControlVisibility()
		If AppSettings("ShowFirstPriorityParts") = "True" Then
			dgPartStockStatusList.Columns(4).Visible = True
		Else
			dgPartStockStatusList.Columns(4).Visible = False
		End If
	End Sub
	'End	
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
		getSession()
		If Not IsPostBack Then
			If txtSearch.Enabled = True Then
				txtSearch.Focus()
			End If
			PartNo = Request.QueryString("PartNo")
			Session("PartNo") = PartNo
			'txtSearch.Text = mEnquiry.EnquiryItems.CurrentItem.ItemName
			txtSearch.Text = PartNo
			DataFieldBind()
			lblResult.Text = "Part Stock Status List :" & mItemStockStatusList.Count & " No.of Record Found(s)."
		Else
			dgPartStockStatusList.DataSource = mItemStockStatusList
			dgPartStockStatusList.DataBind()
		End If
		ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		dgPartStockStatusList.PageIndex = 0
		mItemStockStatusList = ItemStockStatusList.GetItemStockStatusList(txtSearch.Text.Trim, mEnquiry.Date.ToString) 'mEnquiry.Date.ToString Added by Prashant 19-Feb-2013 All19022013
		Session("mItemStockStatusList") = mItemStockStatusList
		dgPartStockStatusList.DataSource = mItemStockStatusList
		dgPartStockStatusList.DataBind()
		lblResult.Text = "Part Stock Status List :" & mItemStockStatusList.Count & " No.of Record Found(s)."
		ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
	End Sub
	Private Sub dgPartStockStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartStockStatusList.PageIndexChanging
		dgPartStockStatusList.PageIndex = e.NewPageIndex
		dgPartStockStatusList.DataSource = mItemStockStatusList
		Session("mItemStockStatusList") = mItemStockStatusList
		dgPartStockStatusList.DataBind()
		ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
	End Sub
	Private Sub dgPartStockStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartStockStatusList.RowCommand
		Select Case e.CommandName
			Case "Select"
				Dim Index As Integer = CInt(e.CommandArgument) + dgPartStockStatusList.PageIndex * dgPartStockStatusList.PageSize
				Dim ItemId As Guid = mItemStockStatusList(Index).ItemID
				setObject(ItemId)
				Session.Remove("mItemStockStatusList")
				'Added by vikrant for popup
				Dim mopenas As String = Request.QueryString("Type")
				If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
					Exit Sub
				End If
				'End
				'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & ItemId.ToString)
		End Select
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Session.Remove("mItemStockStatusList")
		'Added by vikrant for popup
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End
		'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub dgPartStockStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartStockStatusList.Sorting
		mItemStockStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mItemStockStatusList") = mItemStockStatusList
		dgPartStockStatusList.DataSource = mItemStockStatusList
		dgPartStockStatusList.DataBind()
		ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	'Added by Vikrant On 11-Jul-2019 For ALL11072019	
	Private Sub dgPartStockStatusList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgPartStockStatusList.RowDataBound
		If (e.Row.RowType = DataControlRowType.DataRow) Then
			If AppSettings("ShowFirstPriorityParts") = "True" AndAlso (e.Row.Cells(3).Text <> "" And e.Row.Cells(3).Text <> "&nbsp;") And (e.Row.Cells(1).Text <> e.Row.Cells(4).Text) Then
				e.Row.Cells(4).Font.Bold = True
			End If
		End If
	End Sub
	'End
	Private Sub btnAddNewPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewPart.Click
		If (Not User.IsInRole("PartNew")) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		If IsValid Then
			Dim mItem As Item
			mItem = Item.NewItem()
			Session("mItem") = mItem
			Session("Create") = "False"
			Session("PartInfo") = "True"

			Dim URL As Stack = New Stack    'STACK to store url of current page
			URL.Push(Request.Url)           'Inserting URL in STACK
			Session("URL") = URL
			Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfPartStockStatusListForEnquiry_Ajax.aspx")
		End If
	End Sub
#End Region


End Class