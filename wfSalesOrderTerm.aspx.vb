Partial Class wfSalesOrderTerm
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Dim mTerms As Terms
	Public mSalesOrder As SalesOrder
	Dim Type As Int16
#End Region

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents lblListQuotation As System.Web.UI.WebControls.Label
	Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Business Properties "
	Private Sub GetSession()
		mTerms = Session("mTerms")
		mSalesOrder = Session("mSalesOrder")
	End Sub
	Private Sub SetSession()
		Session("mTerms") = mTerms
		Session("mSalesOrder") = mSalesOrder
	End Sub
	Private Sub setTerms()
		Dim i As Integer
		While i < mTerms.Count
			If mSalesOrder.SalesOrderTerms.Contains(mTerms.Item(i).ID) = True Then
				mTerms.Item(i).IsSelected = True
			Else
				mTerms.Item(i).IsSelected = False
			End If
			i = i + 1
		End While
	End Sub
	Private Sub DataFieldBind()
		Type = Request.QueryString("Type")
		mTerms = Terms.GetTerms(mSalesOrder.ID, Type)
		setTerms()
		dgTerm.DataSource = mTerms
		dgTerm.DataBind()
	End Sub
	Private Sub setSelectedTerms()
		Dim item As DataGridItem
		Dim chkBox As CheckBox
		Dim Recordno, PageItems As Integer
		Dim i As Integer
		PageItems = dgTerm.Items.Count - 1
		' Set Selected Notes value  
		For i = 0 To PageItems
			Recordno = i + dgTerm.PageSize * dgTerm.CurrentPageIndex
			item = dgTerm.Items(i)
			chkBox = CType(item.FindControl("chkSelect"), CheckBox)
			mTerms(Recordno).IsSelected = chkBox.Checked
		Next
		Session("mTerms") = mTerms
	End Sub
	Private Sub setObject()
		Dim i As Integer = 0
		While i < mTerms.Count
			If mTerms.Item(i).IsDirty = True Then
				If mTerms.Item(i).IsSelected = True Then
					If mSalesOrder.SalesOrderTerms.Contains(mTerms.Item(i).ID) = False Then
						mSalesOrder.SalesOrderTerms.Add(mTerms.Item(i).ID)
						mSalesOrder.SalesOrderTerms.CurrentItem.Terms = mTerms.Item(i).Terms
						mSalesOrder.SalesOrderTerms.CurrentItem.TermID = mTerms.Item(i).ID
					End If
				Else
					mSalesOrder.SalesOrderTerms.Remove(mTerms.Item(i).ID, "")
				End If
			End If
			i = i + 1
		End While
	End Sub
	Private overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub SetPage()
		If Type = 5 Then
			lblListSalesOrder.Text = "List of Sales Order Terms"
		Else
			lblListSalesOrder.Text = "List of Issue Terms"
		End If
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then
			DataFieldBind()
			SetSession()
			If imgbtnTerm.Enabled = True Then
				setFocus(imgbtnTerm)
			End If
		End If
		SetPage()
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		setSelectedTerms()
		setObject()
		Session("mSalesOrder") = mSalesOrder

		Dim mopenas As String = Request.QueryString("Typepup")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage"))
	End Sub
	Private Sub imgbtnTerm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnTerm.Click

		' Response.Redirect("wfTerm.aspx?ChildPage=wfSalesOrderTerm.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type"))

	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Session.Remove("mTerms")

		Dim mopenas As String = Request.QueryString("Typepup")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage"))
	End Sub
#End Region

End Class
