Partial Class wfnWOJobSpareView_Ajax
	Inherits System.Web.UI.Page

#Region "Variable Declarations"
	Public mnWOJobSpares As nWOJobSpares
#End Region

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mnWOJobSpares = Session("mnWOJobSpares")
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mnWOJobSpares")
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		dgJobSpare.DataSource = mnWOJobSpares
		dgJobSpare.DataBind()
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		GetSession()
		If Not IsPostBack Then
			DataFieldBind()
		End If
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			RemoveSession()
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
#End Region

End Class
