Imports System.Net

Partial Class Profile
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents main As System.Web.UI.HtmlControls.HtmlTable

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Helper Methods "
	Public Sub DatafieldBind()
		Dim mUserList As UserList = UserList.GetUserList(HttpContext.Current.User.Identity.Name, , HttpContext.Current.User.Identity.Name)

		Dim mUserRoleListForProfile As UserRoleListForProfile = UserRoleListForProfile.GetUserRoles(mUserList(HttpContext.Current.User.Identity.Name).UserID)
		dgUser.DataSource = mUserRoleListForProfile
		dgUser.DataBind()

		Dim mLastLoginUserDetails As New LastLoginUserDetails
		mLastLoginUserDetails = LastLoginUserDetails.GetLastLoginUserDetail(HttpContext.Current.User.Identity.Name, MachineName)

		Dim mLast5LoginUserDetails As New Last5LoginUserDetails
		mLast5LoginUserDetails = Last5LoginUserDetails.GetLast5LoginUserDetails(HttpContext.Current.User.Identity.Name, MachineName)
		dgLast5LoginUserDetails.DataSource = mLast5LoginUserDetails
		dgLast5LoginUserDetails.DataBind()

		Dim mLast5EventLogDetails As New Last5EventLogDetails
		mLast5EventLogDetails = Last5EventLogDetails.GetLast5EventLogDetails(HttpContext.Current.User.Identity.Name, MachineName)
		dgLast5EventLogDetails.DataSource = mLast5EventLogDetails
		dgLast5EventLogDetails.DataBind()

		'lblCDate.Text = Date.Now.ToString(Flypal.Util.WebDateFormat) + " " + Date.Now.ToString(Flypal.Util.WebTimeFormat)
		lblCDate.Text = mLastLoginUserDetails.LogInTimeFormatted.ToString
		lblCName.Text = HttpContext.Current.User.Identity.Name

		'Added by Vikrant on 24-July-2012 For ALL11072012
		If AppSettings("PasswordSettings") = "True" Then
			lblPassExpInfo.Visible = True
			lblPassExpiryDetail.Visible = True
			If mUserList(HttpContext.Current.User.Identity.Name).Name.ToUpper = "BTPLADMIN" Then 'Added by Prashant ALL11102013
				'Do Nothing 
			Else

				If mUserList(HttpContext.Current.User.Identity.Name).RemainingDays = 1 Then
					lblPassExpiryDetail.Text = "Today"
				Else
					lblPassExpiryDetail.Text = mUserList(HttpContext.Current.User.Identity.Name).RemainingDays.ToString & " day(s)"
				End If
			End If

		End If
		'End

	End Sub

	Private Function MachineName() As String
		Dim server As String = Nothing
		server = Me.Context.Request.UserHostAddress()
		If server = "127.0.0.1" Then
			server = Dns.GetHostName()
		End If

		Dim heserver As IPHostEntry = Dns.Resolve(server)
		Dim curAdd As IPAddress
		Dim mMachineName As String
		For Each curAdd In heserver.AddressList
			mMachineName = heserver.HostName()
		Next curAdd

		If Me.Context.Request.UserHostAddress() = "127.0.0.1" Then
			Return System.Environment.MachineName
		Else
			Return mMachineName
		End If
	End Function
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		'Added by Utkarsh on 10-Jan-2013 For  ALL10012013
		If Not Page.IsPostBack Then
			cmbStyleSheet.SelectedValue = Session("StyleSheet").ToString
		End If
		'End
		DatafieldBind()
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		Dim str As String

		str = "<script language=javascript>  window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
	End Sub
	'Added by Utkarsh on 10-Jan-2013 For  ALL10012013
	Private Sub btnApplyTheme_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplyTheme.Click
		Dim mUser As User
		mUser = SI.UTILITY.User.GetUser(HttpContext.Current.User.Identity.Name)
		mUser.StyleSheet = cmbStyleSheet.SelectedValue.ToString
		Try
			mUser.Save()
			HttpContext.Current.Session("StyleSheet") = mUser.StyleSheet
			cmbStyleSheet.SelectedValue = Session("StyleSheet").ToString
		Catch

		End Try

	End Sub
	'End
#End Region

End Class
