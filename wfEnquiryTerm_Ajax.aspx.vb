'AJAX Conversion By Vikrant on 10-July-2014

Public Class wfEnquiryTerm_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Dim mTerms As Terms
	Public mEnquiry As Enquiry
	Dim OpenFrom As Int16
#End Region

#Region " Business Properties "
	Private Sub GetSession()
		mTerms = Session("mTerms")
		mEnquiry = Session("mEnquiry")
	End Sub
	Private Sub SetSession()
		Session("mTerms") = mTerms
		Session("mEnquiry") = mEnquiry
	End Sub
	Private Sub setTerms()
		Dim i As Integer
		While i < mTerms.Count
			If mEnquiry.EnquiryTerms.Contains(mTerms.Item(i).ID) = True Then
				mTerms.Item(i).IsSelected = True
			Else
				mTerms.Item(i).IsSelected = False
			End If
			i = i + 1
		End While
	End Sub
	Private Sub DataFieldBind()
		OpenFrom = Request.QueryString("OpenFrom")
		mTerms = Terms.GetTerms(mEnquiry.ID, OpenFrom)
		setTerms()
		dgTerm.DataSource = mTerms
		dgTerm.DataBind()
	End Sub
	Private Sub setSelectedTerms()
		Dim chkBox As CheckBox
		' Set Selected Notes value  
		For i As Integer = 0 To dgTerm.Rows.Count - 1
			chkBox = CType(dgTerm.Rows(i).FindControl("chkSelect"), CheckBox)
			mTerms(i).IsSelected = chkBox.Checked
		Next
		Session("mTerms") = mTerms
	End Sub
	Private Sub setObject()
		Dim i As Integer = 0
		While i < mTerms.Count
			If mTerms.Item(i).IsDirty = True Then
				If mTerms.Item(i).IsSelected = True Then
					If mEnquiry.EnquiryTerms.Contains(mTerms.Item(i).ID) = False Then
						mEnquiry.EnquiryTerms.Add(mTerms.Item(i).ID)
						mEnquiry.EnquiryTerms.CurrentItem.Terms = mTerms.Item(i).Terms
						mEnquiry.EnquiryTerms.CurrentItem.TermID = mTerms.Item(i).ID
					End If
				Else
					mEnquiry.EnquiryTerms.Remove(mTerms.Item(i).ID, "")
				End If
			End If
			i = i + 1
		End While
	End Sub
#End Region

#Region " Events "
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Session.Remove("mTerms")
		'Added by vikrant for popup
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End
		'Response.Redirect(Request.QueryString("BackPage"))
	End Sub
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then
			If imgbtnTerm.Enabled = True Then
				imgbtnTerm.Focus()
			End If
			DataFieldBind()
			SetSession()
			If OpenFrom = 3 Then
				lblListEnquiry.Text = "List of Enquiry Terms"
			End If
		End If
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		setSelectedTerms()
		setObject()
		Session("mEnquiry") = mEnquiry
		'Added by vikrant for popup
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End
		'Response.Redirect(Request.QueryString("BackPage"))
	End Sub
	Private Sub imgbtnTerm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnTerm.Click
		'Response.Redirect("wfTerm.aspx?ChildPage=wfEnquiryTerm.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type"))
	End Sub
	Private Sub dgTerm_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTerm.Sorting
		mTerms.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mTerms") = mTerms
		dgTerm.DataSource = mTerms
		dgTerm.DataBind()
	End Sub
	Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
		DataFieldBind()
		Session("mTerms") = mTerms
		upnlEnquiryDetails.Update()
	End Sub
#End Region



End Class