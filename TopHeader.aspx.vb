Imports System.Text


Partial Class TopHeader
	Inherits Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable Declaration "

	Dim MenuID As String
	Dim path As String = String.Empty

	Public mEventLog As EventLog
	Public mMachineLeaseDetailsList As MachineLeaseDetailsList 'Added by Vikrant FOR ALL22032012
	Public objModuleList As RecentlyUsedMenuItemList 'Changed by Utkarsh on 10-Jan-2013 For  ALL10012013

#End Region

#Region " Business Method(s) "

	'Added by Vikrant FOR ALL22032012
	Private Sub ShowLeaseNotification()
		mMachineLeaseDetailsList = MachineLeaseDetailsList.GetMachineLeaseDetailsList()
		If mMachineLeaseDetailsList.Count > 0 Then
			Dim str As New StringBuilder
			str.Append("<MARQUEE>")
			For i As Integer = 0 To mMachineLeaseDetailsList.Count - 1
				If mMachineLeaseDetailsList(i).DaysRemaining = 0 Then
					str.Append("Lease Period For Aircraft " + mMachineLeaseDetailsList(i).RegNo + " is going to be over Today.")
				Else
					str.Append("Lease Period For Aircraft " + mMachineLeaseDetailsList(i).RegNo + " is going to be over" + "(" + mMachineLeaseDetailsList(i).DaysRemaining.ToString + " Days Remaining).")
				End If
			Next
			str.Append("</MARQUEE>")
			lblLeaseNotification.Visible = True
			lblLeaseNotification.Text = str.ToString
		End If
	End Sub
	'End

	Private Sub ShowPasswordExpiryNotification()
		Dim mUser As SI.UTILITY.User = SI.UTILITY.User.GetUser(User.Identity.Name)
		If mUser.Name.ToUpper = "BTPLADMIN" Then 'Added by Prashant ALL11102013
			'Do Nothing
		Else
			If mUser.RemainingDays > 0 And mUser.RemainingDays <= 3 Then
				If mUser.RemainingDays <> 1 Then
					lblPassExpiryInfo.Text = "Your Password will expire in " & mUser.RemainingDays & " day(s)."
				Else
					lblPassExpiryInfo.Text = "Your Password will expire today."
				End If
				lblPassExpiryInfo.Visible = True
			End If
		End If
	End Sub

	Sub ShowPicture()

		Dim ImageName As String = "ClientLogo"
		Dim dataSet As New dsQuotation
		Try

			'Added by Saylee on 30-Oct-2018 for Location
			mEventLog = Session("mEventLog")

			Dim mUser As User = CType(Session("mUser"), User)
			Dim companyLogo As rptImage = rptImage.GetImage(dataSet)

			If mEventLog Is Nothing Then mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))
			If mUser Is Nothing Then mUser = SI.UTILITY.User.GetUser(mEventLog.UserID)

			Session("mUser") = mUser

			If (path <> "") Then

				File.Delete(path)
				path = String.Empty
				Session("path") = path

			End If

			If companyLogo IsNot Nothing Then

				If companyLogo(0).Size > 0 Then

					Dim FileName As String = $"{ImageName}{companyLogo(0).Extension}"
					path = $"{AppSettings("DOCPath")}\{FileName}"
					Dim FileStream As FileStream

					If Not File.Exists(AppSettings("DOCPath")) Then

						'Delete File if exist
						File.Delete(path:=path)

						' Create the file.
						FileStream = File.Create(path:=path)

						'' Add some information to the file.
						FileStream.Write(companyLogo(0).ImageFile, 0, companyLogo(0).ImageFile.Length)
						FileStream.Close()

						Session("path") = path
						Session("DOCPath") = path

						imgClientLogo.Visible = True
						imgClientLogo.Src = $"{AppSettings("HTTPSecurity")}{Me.Request.Url.Host}/{Me.Request.Url.Segments(1)}Documents/{FileName}"

					End If
				Else
					imgClientLogo.Visible = False
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Event(s) "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try

			SubscriptionReminder()
			lnkHelpbtn.Attributes.Add("OnClick", "javascript:openHelp();")

			If Request.QueryString("Close") = "True" Then
				ClientScript.RegisterStartupScript(Me.GetType(), "keyClose", "<script language=javascript>window.close();<script>")
			End If

			If AppSettings("ClientCode") <> "7AR" Then

				If ((Now.Date >= DateSerial(Year(Now.Date), 8, 14)) And (Now.Date <= DateSerial(Year(Now.Date), 8, 16)) Or (Now.Date >= DateSerial(Year(Now.Date), 1, 25)) And (Now.Date <= DateSerial(Year(Now.Date), 1, 27))) Then
					FlagImage.Visible = True
					If (Now.Date = DateSerial(Year(Now.Date), 8, 15)) Then
						FlagImage.ToolTip = "Happy Independence Day"
					Else
						If (Now.Date >= DateSerial(Year(Now.Date), 8, 14)) And (Now.Date <= DateSerial(Year(Now.Date), 8, 16)) Then
							FlagImage.ToolTip = "Happy Independence Week"
						Else
							If (Now.Date = DateSerial(Year(Now.Date), 1, 26)) Then
								FlagImage.ToolTip = "Happy Republic Day"
							Else
								If (Now.Date >= DateSerial(Year(Now.Date), 1, 25)) And (Now.Date <= DateSerial(Year(Now.Date), 1, 27)) Then
									FlagImage.ToolTip = "Happy Republic Week"
								End If
							End If
						End If
					End If
				Else
					FlagImage.Visible = False
				End If
			Else
				FlagImage.Visible = False
			End If

			Dim k As Int16 = 0
			If Session("MenuID") = "" Then
				Dim str As String
				str = "<script language='javascript'>  openledgersame1('DashBoard.aspx?BackPage='); </script>"
				ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
			End If

			objModuleList = RecentlyUsedMenuItemList.GetRecentlyUsedMenuItemList(HttpContext.Current.User.Identity.Name)
			Session("objModuleList") = objModuleList

			ShowLeaseNotification() 'Added by Vikrant FOR ALL22032012
			'Added by Vikrant on 23-July-2012 For ALL11072012
			If AppSettings("PasswordSettings") = "True" Then
				ShowPasswordExpiryNotification()
			End If
			'END

			If User.IsInRole("MaintDashBoardView") = True Or User.IsInRole("InvDashBoardView") = True Then
				hylnktDashBoard.Visible = True
			Else
				hylnktDashBoard.Visible = False
			End If

			If User.IsInRole("ShowWODashBoardView") = True Then
				hylnktWODashBoard.Visible = True
			Else
				hylnktWODashBoard.Visible = False
			End If
			If User.IsInRole("StickyNoteView") = True Then
				hylnkStickyNote.Visible = True
			Else
				hylnkStickyNote.Visible = False
			End If

			mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))
			ShowPicture()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SubscriptionReminder()
		Try
			Dim mDays As Integer = 0

			'Added by Saylee on 3-Apr-2025 for Subscription Extension thru webconfig
			Dim mSubscriptionExtensionInDays As Integer = CType(AppSettings("SubscriptionExtensionInDays"), Integer)
			Session("mSubscriptionExtensionInDays") = mSubscriptionExtensionInDays
			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
			If mCheck.WebAuthentication = True Then

				mDays = mCheck.Number("Days")
				mDays = mDays - mCheck.ElapsedDays

				If mDays <= 30 Then
					lblMessage.Visible = True

					If chkIsLocked.Checked Then 'Changed by Kalpesh Shah
						Select Case mDays
							Case Is < 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.Text = "<MARQUEE>Your A.M.C elapsed since last " + Math.Abs(mDays).ToString + " day(s). " + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details. </MARQUEE>"
								Else
									lblMessage.Text = "<MARQUEE>FlyPal Subscription has lapsed since " + Math.Abs(mDays).ToString + " day(s). Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation. </MARQUEE>"
								End If

								chkIsLocked.Text = "Expired"
							Case 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.Text = "<MARQUEE>Your A.M.C will elapsed today. " + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details.</MARQUEE>"
								Else
									lblMessage.Text = "<MARQUEE>Less than 24 hrs remaining to Renew FlyPal Subscription .Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation.</MARQUEE>"
								End If
							Case Is > 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.Text = "<MARQUEE>Your A.M.C will elapsed within " + mDays.ToString + " day(s). " + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details.</MARQUEE>"
								Else
									lblMessage.Text = "<MARQUEE>FlyPal Subscription is due for renewal in " + mDays.ToString + " day(s). Please renew at least 48 hrs before due Date, to avoid  Reactivation and Database unlock charges. Kindly Contact BytzSoft Technologies for further details.</MARQUEE>"
								End If

						End Select
					Else
						Select Case mDays
							Case Is < 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.Text = "Your A.M.C elapsed since last <b>" + Math.Abs(mDays).ToString + " day(s).</b>" + vbCrLf + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details."
								Else
									lblMessage.Text = "FlyPal Subscription has lapsed since <b>" + Math.Abs(mDays).ToString + " day(s).</b>" + vbCrLf + "Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation."
								End If

							Case 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.Text = "Your A.M.C will elapsed today." + vbCrLf + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd. Pvt. Ltd.</b> for further details."
								Else
									lblMessage.Text = "Less than 24 hrs remaining to Renew FlyPal Subscription .Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation."
								End If

							Case Is > 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.Text = "Your A.M.C will elapsed within <b>" + mDays.ToString + " day(s).</b>" + vbCrLf + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details."
								Else
									lblMessage.Text = "FlyPal Subscription is due for renewal in <b>" + mDays.ToString + " day(s).</b>" + vbCrLf + "Please renew at least 48 hrs before due Date, to avoid  Reactivation and Database unlock charges. Kindly Contact BytzSoft Technologies for further details."
								End If

						End Select
					End If
				Else
					lblMessage.Visible = False
				End If
			ElseIf mSubscriptionExtensionInDays > 0 Then  'Added by Saylee on 3-Apr-2025 for Subscription Extension thru webconfig
				lblMessage.Visible = True
				mDays = mCheck.Number("Days")
				mDays = mDays - mCheck.ElapsedDays
				If AppSettings("Mode") = "AMC" Then
					lblMessage.Text = "<MARQUEE>Your A.M.C elapsed since last <b>" + (mDays).ToString + " day(s).</b>" + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details.</MARQUEE>"
				Else
					lblMessage.Text = "<MARQUEE>FlyPal Subscription has lapsed since <b>" + (mDays).ToString + " day(s).</b>" + "" + "Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation.</MARQUEE>"
				End If
			End If
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try
	End Sub

	Private Sub lnkLogoutbtn_Click(sender As Object, e As ImageClickEventArgs) Handles lnkLogoutbtn.Click

		'16-Feb-2024 Concurrent User implementation by Kalpesh
		Dim a As New UserLoginSession
		a.DeleteUserLoginSession(New Guid(Session("UserId").ToString), New Guid(Session("LoginSession").ToString))
		SignOut()
		'-----------------------------------------------
		Session.Remove("MenuID")
		Session.Remove("MiddleFrame")
		MarkLog(Action.Logoff)

		Thread.CurrentPrincipal = Nothing
		Dim str As String

		Dim str1 As String
		str1 = "delete_cookie();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)

		str = "<script language=javascript>  window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
		Session.Remove("ReminderFired")
	End Sub

	Private Sub Page_Error(sender As Object, e As EventArgs) Handles MyBase.Error
		Session("Message") = Context.Server.GetLastError.Message
		Session("Source") = Context.Server.GetLastError.Source
		Session("Trace") = Context.Server.GetLastError.StackTrace
	End Sub

	Private Sub lnkAboutFlyPalbtn_Click(sender As Object, e As ImageClickEventArgs) Handles lnkAboutFlyPalbtn.Click
		Dim str As String

		str = "<script language=javascript> window.open('wfAboutFlyPal.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');  </script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)

	End Sub

	Private Sub lnkProfilesbtn_Click(sender As Object, e As ImageClickEventArgs) Handles lnkProfilesbtn.Click
		Dim str As String

		str = "<script language=javascript>  window.open('Profile.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
	End Sub

	Private Sub lnkpreferences_Click(sender As Object, e As EventArgs)
		Dim str As String

		str = "<script language=javascript>  window.open('S1.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
	End Sub

#End Region

#Region " Web Method(s) "

	<Services.WebMethod(EnableSession:=True)>
	Public Shared Function SignOut() As String 'Added by Yogita for Explicit sign out


		'16-Feb-2024 Concurrent User implementation by Kalpesh
		Dim a As New UserLoginSession
		a.DeleteUserLoginSession(New Guid(HttpContext.Current.Session("UserId").ToString), New Guid(HttpContext.Current.Session("LoginSession").ToString))
		'------------------------------------------------------

		Web.Security.FormsAuthentication.SignOut()
		HttpContext.Current.Session.Abandon()

		MarkLog(Action.Logoff)

		Thread.CurrentPrincipal = Nothing
		Return ""

	End Function

#End Region

End Class
