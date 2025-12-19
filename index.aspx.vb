Partial Class index
	Inherits System.Web.UI.Page

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
	'Added By Vikrant On 22-Oct-2012 For ALL22102012-1   
	Private Function CheckPartStatus() As Boolean
		If User.IsInRole("PartNew") And User.IsInRole("PartEdit") And User.IsInRole("PartDelete") And User.IsInRole("PartView") And User.IsInRole("PartPrint") Then
			Dim mPartTypeList As PartTypeList = PartTypeList.GetPartTypeList(False)
			For i As Integer = 0 To mPartTypeList.Count - 1
				If mPartTypeList(i).PartStatusID = 0 Then
					Return True
				End If
			Next
		End If
	End Function
	'End

	'Added By By Prashant 20-Aug-2013  ALL20082013   
	Private Function CheckPrimaryCategoryStatus() As Boolean
		If User.IsInRole("PartNew") And User.IsInRole("PartEdit") And User.IsInRole("PartDelete") And User.IsInRole("PartView") And User.IsInRole("PartPrint") Then
			Dim mCategoryList As CategoryList = CategoryList.GetCategoryList(False)
			For i As Integer = 0 To mCategoryList.Count - 1
				If mCategoryList(i).PrimaryCategoryID = 0 Then
					Return True
				End If
			Next
		End If
	End Function
	'End
	'NPS
	Private Function CheckForFeedBackStatus(ByVal tmpUser As User) As Boolean
		If UCase(tmpUser.Name) = UCase("BTPLAdmin") Then
			Return False
		End If
		Dim mCompanyDetail As New CompanyDetail
		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

		If mCompanyDetail.IsFeedBackAsk AndAlso tmpUser.IsFeedBackAsk Then
			Dim isUserCreationDateFound As Boolean = Not tmpUser.UserCreationDate.Equals(System.DBNull.Value)
			Dim tempdate As Date = New Date
			Dim Is90DaysCrossedAfteruserCreation As Boolean = False

			If isUserCreationDateFound Then
				tempdate = CDate(tmpUser.UserCreationDate).AddDays(90)
				Is90DaysCrossedAfteruserCreation = (Now.Date >= tempdate)
			End If

			' (SubmittedDate is NULL AND UserCreationDate is NULL ) OR  Means New User
			' (SubmittedDate is NULL AND Login after 3 months of UserCreation)   Then  -> ASK FeedBack
			'
			If (tmpUser.FeedBackSubmittedDate.Equals(System.DBNull.Value) And tmpUser.UserCreationDate.Equals(System.DBNull.Value)) Or _
				(tmpUser.FeedBackSubmittedDate.Equals(System.DBNull.Value) AndAlso Is90DaysCrossedAfteruserCreation) Then
				Session("mCompanyDetail") = mCompanyDetail 'Store Session if True
				Session("mUser") = tmpUser
				Return True
			Else
				Return False
			End If
		Else
			Return False
		End If

	End Function
	'End
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then



			''Kalpesh Shah
			''Added By Vikrant On 22-Oct-2012 For ALL22102012-1
			'If CheckPartStatus() Then
			'    Response.Redirect("wfSelectPartStatusForPartType.aspx?BackPage=Login.aspx")
			'End If
			''End

			''Added By By Prashant 20-Aug-2013  ALL20082013   
			'If CheckPrimaryCategoryStatus() Then
			'    Response.Redirect("wfPrimaryCategoryList.aspx?BackPage=Login.aspx")
			'End If
			''End

			'NPS
			If Session("CSLA-Principal") IsNot Nothing Then
				Dim mUser As SI.UTILITY.User = SI.UTILITY.User.GetUser(User.Identity.Name)
				If CheckForFeedBackStatus(tmpUser:=mUser) Then
					'Response.Redirect("wfFeedBackForm_Ajax.aspx?BackPage=Login.aspx")
					Response.Redirect("wfFeedBack.aspx?BackPage=Login.aspx")  'Sankalp 04-09-25
				End If
			End If
			'END

			If Session("ReminderFired") Is Nothing Then
				Session("ReminderFired") = "True"
				CheckAndStartReminderSystem(True)
			End If
		End If
	End Sub

	Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
		Session("Message") = Context.Server.GetLastError.Message
		Session("Source") = Context.Server.GetLastError.Source
		Session("Trace") = Context.Server.GetLastError.StackTrace
	End Sub

#End Region

#Region "Reminder"
	'Kalpesh
	Private Sub CheckAndStartReminderSystem(ByVal IsCreateLink As Boolean)
		Try
			Dim mReminder As New Reminder
			mReminder = Reminder.GetAutoReminders(User.Identity.Name)

			'Activating Auto Reminder System
			Dim IsReminderStarted As Boolean
			IsReminderStarted = mReminder.StartAutoReminder(Now.DayOfWeek, User)
			If IsReminderStarted Then
				'Activating Reminder list Form
				Response.Redirect("wfReminderList_Ajax.aspx?BackPage=Login.aspx")
			End If
		Catch ex As Exception
			'
		Finally
			'
		End Try
	End Sub
#End Region


End Class
