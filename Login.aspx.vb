Imports System.Net
Imports System.Text
Imports Authenticate
Imports System.Security.Cryptography
Imports System.Net.NetworkInformation


Public Class Login
	Inherits Page

#Region " Variables "

	Dim mUserId As Guid
	Dim rnd As New Random
	Dim mUserOTP As UserOTP 'Added at 26-Jul-2016 by Bhushan for OTP password generation change
	Dim mTempUserForOTP As User
	Dim mModuleList As ModuleList
	Dim mCompanyDetail As New CompanyDetail
	Dim mTransactionList As TransactionList
	Dim EncryptMailID As StringBuilder = New StringBuilder

	Dim mDBPassword As String = ""
	Dim MailID As String = String.Empty
	Dim mSubscriptionExtensionInDays As Integer

#End Region

#Region " Method(s) "

	Private Sub AddAttributes()
	End Sub

	Private Sub UpdateSubscriptionDays(Days As Integer) 'Added By Prashant On 23-Apr-2019
		Dim conString As String = AppSettings("DB:FlyPal")
		Dim con = New SqlConnection(conString)
		con.Open()
		Dim cmd As New SqlCommand()
		cmd.Connection = con
		cmd.CommandText = "UpdateSubscriptionDays"
		cmd.CommandType = CommandType.StoredProcedure
		cmd.Parameters.AddWithValue("@Days", Days)
		cmd.ExecuteNonQuery()
		con.Close()
	End Sub

	Private Sub RegistrationDetails()
		Dim mRegistration As Registration.Registration
		Dim ID As Guid = New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}")
		mRegistration = Registration.Registration.GetRegistrationDetails(ID, Registration.Registration.RegistrationOf.Flypal)
		Session("mRegistration") = mRegistration
		If Not mRegistration.IsEntryFound Then
			Response.Redirect("wfRegistration.aspx?BackPage=Login.aspx")
		Else
			'mRegistration = mRegistration.GetRegistrationDetails(ID, Registration.Registration.RegistrationOf.FlypalInv)
			'If mRegistration.IsEntryFound = False Then
			'lblInvalid.Text = "Registration reuiqred to continue."
			'End If
		End If
	End Sub

	Private Function VerifyLoginRule(mlogin As SI.UTILITY.Login) As Boolean

		Try
			Dim mUnSuccessLoginCount As UnSuccessLoginCount
			Dim mMaxLoginAttemptValue As Integer = 0

			Dim mLoginRuleList As LoginRuleList = LoginRuleList.GetLoginRuleList()
			Dim mLoginRule As LoginRuleList.LoginRuleInfo

			Session("LoginRuleID") = Nothing
			Session("mTempUser") = Nothing
			txtOTPUserName.Text = ""

			For Each mLoginRule In mLoginRuleList

				Try
					mTempUserForOTP = SI.UTILITY.User.GetUser(mlogin.UserName)
					Session("TempUserForOTP") = mTempUserForOTP
					If mTempUserForOTP.Name = "" Then
						Exit Function
					End If
				Catch ex As Exception
					Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.Password, Session("RequestInfo"))
					Session("AuthenticatedMessage") = CType(bp.IdentityInfo, BusinessIdentity).AuthenticatedMessage
					lblInvalid.Text = Session("AuthenticatedMessage")
					Exit For
				End Try

				Select Case mLoginRule.ID
					Case 1   'Login Attempt
						If mLoginRule.Enforce = True Then


							mUnSuccessLoginCount = UnSuccessLoginCount.GetUnSuccessLoginCount(mlogin.UserName, mLoginRule.Value)
							mMaxLoginAttemptValue = mLoginRule.Value

							If mUnSuccessLoginCount.UnSuccessLoginCount >= mLoginRule.Value Then

								GenerateOTP(mlogin, mTempUserForOTP, mLoginRule)

								'Exit Sub

								Return False
							End If
						End If
					Case 2   'Browsers and IPAddress

						If mLoginRule.Enforce = True And UCase(mlogin.UserName) <> "BTPLADMIN" And (Not Session("UserId") Is Nothing) Then

							Dim mUserLoginSettingList As UserLoginSettingList = UserLoginSettingList.GetUserLoginSettingList(Session("UserId"))

							If mUserLoginSettingList.Count = 0 Then

								Dim mUserLoginSetting As UserLoginSetting = UserLoginSetting.NewUserLoginSetting(Guid.NewGuid, Session("UserId"), Request.Browser.Browser, Me.Request.UserHostAddress, Now.ToString)
								Page.Validate("a")

								If IsValid Then
									mUserLoginSetting.Save()
								End If

							Else
								'Check Browser
								If Not mUserLoginSettingList.Contains(UserLoginSettingList.LoginSettingType.BrowserName, Request.Browser.Browser) Then
									GenerateOTP(mlogin, mTempUserForOTP, mLoginRule)
									Return False
								End If
								'Check IP Address
								If Not mUserLoginSettingList.Contains(UserLoginSettingList.LoginSettingType.IPAddress, Me.Request.UserHostAddress) Then
									GenerateOTP(mlogin, mTempUserForOTP, mLoginRule)
									Return False
								End If

							End If

						End If
				End Select
			Next
			Return True
		Catch ex As Exception
			lblInvalid.Text = ex.InnerException.ToString + ex.Message.ToString
		End Try

	End Function

	'added on 23-May-2018 for OTP Changes
	Private Sub GenerateOTPClick()
		Try
			lblUserInfoOTP.Visible = False
			txtGenerateOTP.Enabled = True

			Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			Dim small_alphabets As String = "abcdefghijklmnopqrstuvwxyz"
			Dim numbers As String = "1234567890"

			Dim characters As String = numbers

			mTempUserForOTP = Session("TempUserForOTP")

			Dim mlogin As New SI.UTILITY.Login(mTempUserForOTP.Name, mTempUserForOTP.Password)
			BusinessPrincipal.login(mTempUserForOTP.Name, mTempUserForOTP.Password)

			Dim length As Integer = 5

			Dim otp As String = String.Empty

			For i As Integer = 0 To length - 1
				Dim character As String = String.Empty
				Do
					Dim index As Integer = New Random().Next(0, characters.Length)
					character = characters.ToCharArray()(index).ToString()
				Loop While otp.IndexOf(character) <> -1
				otp += character
			Next

			Try
				If mTempUserForOTP.UserEmail <> "" Then

					mUserOTP = UserOTP.GetUserOTP(mTempUserForOTP.UserID, "", CInt(Session("LoginRuleID")))

					If mUserOTP.ID.Equals(Guid.Empty) Or mUserOTP Is Nothing Then
						mUserOTP = UserOTP.NewUserOTP(Guid.NewGuid, mTempUserForOTP.UserID, otp, Now, Now.AddMinutes(30), False, CInt(Session("LoginRuleID")))
					Else
						mUserOTP.OTP = otp
						mUserOTP.SentDateTime = Now
						mUserOTP.ValidDateTime = Now.AddMinutes(30)
						mUserOTP.IsUsed = False

					End If
					Page.Validate("a")
					If IsValid Then
						Try
							mUserOTP.Save()
							If UCase(mlogin.UserName.Trim).Equals("BTPLADMIN") Then
								MailID = "support@bytzsoft.com"
							Else
								MailID = mTempUserForOTP.UserEmail.Trim
							End If
							'P
							SendMailFile.SendMailFile(, mTempUserForOTP.Name, "FlyPal Login OTP [One Time Password].", "", "", , MailID.Trim, "", "", , , True, BodyMessage(mUserOTP, mTempUserForOTP))
						Catch ex As SqlException
							If ex.Number = 8145 Then
								'' MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
								lblInvalid.Text = ex.Procedure
								Exit Sub
							ElseIf ex.Number = 2627 Then
								'' MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
								lblInvalid.Text = ex.Procedure
								Exit Sub
							End If
						End Try
					End If
				End If

			Catch ex As Exception
				''MSGBoxCtrl.show("Error", "Error Sending OTP Mail", ex.InnerException.ToString + ex.Message.ToString, MsgBoxStyle.OkOnly, "")
				lblInvalid.Text = ex.InnerException.ToString + ex.Message.ToString
			End Try
			upnlGenerateOTP.Update()
			btnSubmitOTP.Focus()
			btnSubmitOTP.Enabled = True
		Catch ex As Exception

		End Try
	End Sub

	Private Sub GenerateOTP(mlogin As SI.UTILITY.Login, mTempUserForOTP As User, mLoginRule As LoginRuleList.LoginRuleInfo)

		SingleSessionPreparation.CreateAndStoreSessionToken(txtUserName.Text)

		txtOTPUserName.Text = mlogin.UserName
		Session("LoginRuleID") = mLoginRule.ID

		If mLoginRule.ID = 1 Then   'Max. Login Attempt
			Try
				If UCase(mlogin.UserName.Trim).Equals("BTPLADMIN") Then
					MailID = "support@bytzsoft.com"
				Else
					MailID = mTempUserForOTP.UserEmail.Trim
				End If
				'P
				SendMailFile.SendMailFile(, mTempUserForOTP.Name, "FlyPal user Login[ " + AppSettings("ClientCode") + " ] : Locked.", "", "", , MailID.Trim, "", "", , , True, BodyMessageForLocked(mTempUserForOTP, mLoginRule))

				'Added on 23-may-2018 For OTP Functionality change
				GenerateOTPClick()
				'-------------

			Catch ex As SqlException
				If ex.Number = 8145 Then
					''MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
					lblInvalid.Text = ex.Procedure
					Exit Sub
				ElseIf ex.Number = 2627 Then
					'' MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					lblInvalid.Text = ex.Procedure
					Exit Sub
				End If
			End Try
		Else
			'Added on 23-may-2018 For OTP Functionality change
			GenerateOTPClick()
			'-------------
		End If

		txtGenerateOTP.Text = ""

		If mLoginRule.ID = 1 Then 'Max. Login Attempt 
			lblUserInfoOTP.Text = "<b>FlyPal account has been locked and need to verify your identity.</b>"
		ElseIf mLoginRule.ID = 2 Then 'Browser And IP Address
		Else
		End If

		If UCase(mlogin.UserName.Trim).Equals("BTPLADMIN") Then
			MailID = "support@bytzsoft.com"
		Else
			MailID = mTempUserForOTP.UserEmail.Trim
		End If
		If MailID.Trim.Length > 0 Then
			EncryptMailID.Append(MailID.Substring(0, 1))
			EncryptMailID.Append("****" & MailID.Substring(MailID.IndexOf("@") - 1, 1) & "@")
			EncryptMailID.Append(MailID.Substring(MailID.IndexOf("@") + 1, 1) & "****" & MailID.Substring(MailID.LastIndexOf(".") - 1, 1) & MailID.Substring(MailID.LastIndexOf(".")))
		End If
		lblGenerateOTPInfo.Text = "You will receive OTP on your registered email :<b>" + EncryptMailID.ToString.Trim + "</b>"
		lblNote.Text = "<b>Note :</b> Please ensure your email id is registered and valid. <br/>" + "Having difficulty with OTP generation? Kindly contact with our support team at<br/><b>support@bytzsoft.com</b>"

		btnSubmitOTP.Focus()
		btnSubmitOTP.Enabled = True
		mTempUserForOTP = Nothing
		OpenOTPModalpopup()
		upnlGenerateOTP.Update()

	End Sub

	Public Sub OpenOTPModalpopup()
		'lblDeleteConfirmModal.Text = Msg
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openOTPModalpopup", "openOTPModalpopup();", True)
		upnlOTP.Update()
	End Sub

	Public Sub HideOTPModalpopup()
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideOTPModalpopup", "hideOTPModalpopup();", True)
		upnlOTP.Update()
	End Sub

	Public Sub OpenPopupChangePassword()
		'lblDeleteConfirmModal.Text = Msg
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openChangePasswordModalpopup", "openChangePasswordModalpopup();", True)
		upnlChangePassword.Update()
	End Sub

	Public Sub HidePopupChangePassword()
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideChangePasswordModalpopup", "hideChangePasswordModalpopup();", True)
		upnlChangePassword.Update()
	End Sub

	Private Sub IsAccessOutSideLAN()
		Dim initialIPofHost() As String = Me.Request.Url.Host.Split(".")
		Dim initialIPofCilent() As String = Me.Request.UserHostAddress.Split(".")

		If UBound(initialIPofHost) > 2 And UBound(initialIPofCilent) > 2 Then
			'IF first two slots of IP address are not same then its not Local IP
			'Means its accesing from Internet
			If Not ((initialIPofHost(0) = initialIPofCilent(0)) And (initialIPofHost(1) = initialIPofCilent(1))) Then
				Session("CSLA-Principal") = Nothing
				HttpContext.Current.User = Nothing
				Response.Redirect("NotAccess.htm")
			End If
		End If
	End Sub

	Public Sub ChangePassword()
		Dim UserName As String = txtUserName.Text
		Dim Password As String = txtPassword.Text
		Dim mlogin As New SI.UTILITY.Login(UserName, Password)

		Dim mUserList As UserList = UserList.GetUserList(UserName, , HttpContext.Current.User.Identity.Name)
		mUserId = mUserList.Item(UserName).UserID()
		'Session.Remove("MiddleFrame")
		Response.Redirect("wfChangePassword_Ajax.aspx?RequestedBy=1&UserID=" & mUserId.ToString & "&BackPage=Login.aspx")
	End Sub

	Private Function IPAddress() As String
		Dim server As String = Nothing
		server = Me.Context.Request.UserHostAddress()
		If server = "127.0.0.1" Then
			server = Dns.GetHostName()
		End If

		Dim heserver As IPHostEntry = Dns.Resolve(server)
		Dim curAdd As IPAddress
		For Each curAdd In heserver.AddressList
			curAdd.ToString()
		Next curAdd
		Return curAdd.ToString()

		''Return Me.Context.Request.UserHostAddress()
	End Function

	Private Function MachineName() As String 'Added by Saylee on 14-Aug-2009
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

	Private Function IsChangePasswordRequested() As Boolean
		Try

			BusinessPrincipal.login(txtUserName.Text, txtPassword.Text)

			'Added by Vikrant on 23-July-2012 For ALL11072012
			If AppSettings("PasswordSettings") = "True" Then
				Dim mUser As User = SI.UTILITY.User.GetUser(txtUserName.Text)
				If Not mUser Is Nothing Then
					If (mUser.RemainingDays <= 0 And mUser.Name.ToUpper = "BTPLADMIN") Then 'Added by Prashant ALL11102013
						'Do nothing 
					Else
						If (mUser.RemainingDays <= 0) Then
							mUser.ChangePassword = True
							mUser = CType(mUser.Save, User)
						End If

					End If
				End If
			End If
			'End

			Dim mUserlist As UserList = UserList.GetUserList(txtUserName.Text, , HttpContext.Current.User.Identity.Name)
			If mUserlist.Contains(txtUserName.Text) Then
				Dim mUserInfo As UserList.UserInfo
				mUserInfo = mUserlist.Item(txtUserName.Text)
				'If mUserInfo.Name = txtUserName.Text Then
				If String.Compare(mUserInfo.Name, txtUserName.Text, True) = 0 Then
					If mUserInfo.ChangePassword = True Then
						mUserId = mUserInfo.UserID
						Return True
					Else
						Return False
					End If
				End If
			Else
				Return False
			End If
		Catch ex As Exception
			Return False
		End Try
	End Function

	Private Sub SetDbPassword(UserName As String, Password As String)
		Dim HashValue() As Byte

		Dim MessageString As String
		MessageString = Password & "$$" & LCase(UserName)

		'Create a new instance of UnicodeEncoding to 
		'convert the string into an array of Unicode bytes.
		Dim UE As New UnicodeEncoding

		'Convert the string into an array of bytes.
		Dim MessageBytes As Byte() = UE.GetBytes(MessageString)

		'Create a new instance of SHA1Managed to create 
		'the hash value.
		Dim SHhash As New SHA1Managed

		'Create the hash value from the array of bytes.
		HashValue = SHhash.ComputeHash(MessageBytes)
		Dim Str1 As String

		Str1 = ""
		Dim b As Byte
		For Each b In HashValue
			'If Label1.Text = "" Then
			Str1 = Str1 & Hex(b).ToString
		Next
		mDBPassword = Str1
	End Sub

	Private Function AllowNewAircraft() As Boolean  'Added By Prashant 12-Aug-2014 ALL12082014
		BusinessPrincipal.login(txtUserName.Text, txtPassword.Text)
		Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
		If mCheck.WebAuthentication = True Then
			Dim mAircraftCountForLicense As AircraftCountForLicense = AircraftCountForLicense.GetAircraftCountForLicense
			If mAircraftCountForLicense.Count > mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
				Return False
			Else
				Return True
			End If
		End If
	End Function

	Private Sub SubscriptionReminder()
		Try
			Dim mDays As Integer = 0
			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
			mSubscriptionExtensionInDays = Session("mSubscriptionExtensionInDays")  'Added by Saylee on 3-Apr-2025 for Subscription Extension thru webconfig

			If mCheck.WebAuthentication = True Then

				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				'Dim strOutString As String = ReadXMLFile()
				'strOutString = strOutString.Split(CChar("$"))(1)
				'mDays = CInt(strOutString) - mCheck.ElapsedDays


				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				mDays = mCheck.Number("Days")
				mDays = mDays - mCheck.ElapsedDays
				'---------------------------------

				If mDays <= 30 Then
					lblMessage.Visible = True

					If chkIsLocked.Checked Then 'Changed by Kalpesh Shah
						Select Case mDays
							Case Is < 0
								If AppSettings("Mode") = "AMC" Then
									'lblMessage.Text = "<MARQUEE>Your A.M.C expired since last " + System.Math.Abs(mDays).ToString + " day(s). <b>And it locked till next renewal.</b>" + "" + "Contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details of A.M.C renewal. </MARQUEE>"
									lblMessage.InnerText = "<MARQUEE>Your A.M.C elapsed since last " + System.Math.Abs(mDays).ToString + " day(s). " + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details. </MARQUEE>"
								Else
									lblMessage.InnerText = "<MARQUEE>FlyPal Subscription has lapsed since " + System.Math.Abs(mDays).ToString + " day(s). Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation. </MARQUEE>"
								End If

								chkIsLocked.Text = "Expired"
								UpdateSubscriptionDays(-1)
							Case 0
								If AppSettings("Mode") = "AMC" Then
									'lblMessage.Text = "<MARQUEE>Your A.M.C will expire today. <b>And it will get locked till next renewal.</b>" + "" + "Contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details of A.M.C renewal.</MARQUEE>"
									lblMessage.InnerText = "<MARQUEE>Your A.M.C will elapsed today. " + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details.</MARQUEE>"
								Else
									lblMessage.InnerText = "<MARQUEE>Less than 24 hrs remaining to Renew FlyPal Subscription .Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation.</MARQUEE>"
								End If
								If TimeOfDay > #3:00:01 PM# Then  'Added By Prashant 25-Feb-2021
									UpdateSubscriptionDays(mDays)
								End If
							Case Is > 0
								If AppSettings("Mode") = "AMC" Then
									'lblMessage.Text = "<MARQUEE>Your A.M.C will expire within " + mDays.ToString + " day(s). <b>And it will get locked till next renewal.</b>" + "" + "Contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details of A.M.C renewal.</MARQUEE>"
									lblMessage.InnerText = "<MARQUEE>Your A.M.C will elapsed within " + mDays.ToString + " day(s). " + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details.</MARQUEE>"
								Else
									lblMessage.InnerText = "<MARQUEE>FlyPal Subscription is due for renewal in " + mDays.ToString + " day(s). Please renew at least 48 hrs before due Date, to avoid  Reactivation and Database unlock charges. Kindly Contact BytzSoft Technologies for further details.</MARQUEE>"
								End If
								UpdateSubscriptionDays(mDays)
						End Select
					Else
						Select Case mDays
							Case Is < 0
								If AppSettings("Mode") = "AMC" Then
									'lblMessage.Text = "Your A.M.C expired since last <b>" + System.Math.Abs(mDays).ToString + " day(s).</b>" + vbCrLf + "Contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details of A.M.C renewal."
									lblMessage.InnerText = "Your A.M.C elapsed since last <b>" + System.Math.Abs(mDays).ToString + " day(s).</b>" + vbCrLf + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details."
								Else
									lblMessage.InnerText = "FlyPal Subscription has lapsed since <b>" + System.Math.Abs(mDays).ToString + " day(s).</b>" + vbCrLf + "Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation."
								End If
								UpdateSubscriptionDays(mDays)
							Case 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.InnerText = "Your A.M.C will elapsed today." + vbCrLf + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd. Pvt. Ltd.</b> for further details."
								Else
									lblMessage.InnerText = "Less than 24 hrs remaining to Renew FlyPal Subscription .Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation."
								End If
								If TimeOfDay > #3:00:01 PM# Then 'Added By Prashant 25-Feb-2021
									UpdateSubscriptionDays(mDays)
								End If
							Case Is > 0
								If AppSettings("Mode") = "AMC" Then
									lblMessage.InnerText = "Your A.M.C will elapsed within <b>" + mDays.ToString + " day(s).</b>" + vbCrLf + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details."
								Else
									lblMessage.InnerText = "FlyPal Subscription is due for renewal in <b>" + mDays.ToString + " day(s).</b>" + vbCrLf + "Please renew at least 48 hrs before due Date, to avoid  Reactivation and Database unlock charges. Kindly Contact BytzSoft Technologies for further details."
								End If
								UpdateSubscriptionDays(mDays)
						End Select
					End If
				Else
					UpdateSubscriptionDays(mDays)
					lblMessage.Visible = False
				End If
			ElseIf mSubscriptionExtensionInDays > 0 Then  'Added by Saylee on 3-Apr-2025 for Subscription Extension thru webconfig
				lblMessage.Visible = True
				mDays = mCheck.Number("Days")
				mDays = mDays - mCheck.ElapsedDays
				If AppSettings("Mode") = "AMC" Then
					lblMessage.InnerText = "<MARQUEE>Your A.M.C elapsed since last <b>" + (mDays).ToString + " day(s).</b>" + "" + "Kindly contact <b>BytzSoft Technologies Pvt. Ltd.</b> for further details.</MARQUEE>"
				Else
					' System.Math.Abs(mDays).ToString 
					lblMessage.InnerText = "<MARQUEE>FlyPal Subscription has lapsed since <b>" + (mDays).ToString + " day(s).</b>" + "" + "Kindly Contact BytzSoft Technologies immediately to avoid Account deactivation.</MARQUEE>"
				End If
			End If
		Catch ex As Exception
			'
		Finally
			'
		End Try
	End Sub

	Private Function getDBPassword(Username As String) As String

		Dim DBPassword As String = ""

		Try
			Dim cn As New SqlConnection(AppSettings("DB:FlyPal"))
			Dim cm As New SqlCommand
			Dim dr As SqlDataReader

			cn.Open()

			With cm

				.Connection = cn
				.CommandType = CommandType.StoredProcedure
				.CommandText = "UM_fetchUserByName"

				.Parameters.AddWithValue("@UserName", Username)

				dr = cm.ExecuteReader()

				dr.Read()

				If (dr.HasRows) Then

					DBPassword = dr.GetString(2)

				End If

			End With

			dr.Close()
			cn.Close()

		Catch ex As Exception
			'
		End Try

		Return DBPassword

	End Function

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			If IsNothing(Request.QueryString("ReturnUrl")) = False AndAlso Request.QueryString("ReturnUrl").IndexOf("/APP/") > 0 Then

				Dim mlogin As New SI.UTILITY.Login("btpladmin", "bytzAdmin")
				Dim mUsername As String = Request.QueryString("Username")
				Dim bp As BusinessPrincipal = BusinessPrincipal.login(mUsername, getDBPassword(mUsername), "")

				If Thread.CurrentPrincipal.Identity.IsAuthenticated Then
					Session("CSLA-Principal") = Threading.Thread.CurrentPrincipal
					HttpContext.Current.User = CType(Session("CSLA-Principal"), System.Security.Principal.IPrincipal)
					Web.Security.FormsAuthentication.SetAuthCookie(mUsername, True)
				End If

				Dim url As String = Request.QueryString("ReturnUrl")
				Response.Redirect(url)

			ElseIf IsNothing(Request.QueryString("ReturnUrl")) = False AndAlso Request.QueryString("ReturnUrl").IndexOf("FlypalAPI.asmx") > 0 Then

				Dim mUsername As String = "btpladmin"
				Dim bp As BusinessPrincipal = BusinessPrincipal.login(mUsername, getDBPassword(mUsername), "")

				If Thread.CurrentPrincipal.Identity.IsAuthenticated Then
					Session("CSLA-Principal") = Threading.Thread.CurrentPrincipal
					HttpContext.Current.User = CType(Session("CSLA-Principal"), System.Security.Principal.IPrincipal)
					Web.Security.FormsAuthentication.SetAuthCookie(mUsername, True)
				End If

				Dim url As String = Request.QueryString("ReturnUrl")
				Response.Redirect(url)

			End If

			'Added by Saylee on 3-Apr-2025 for Subscription Extension thru webconfig
			mSubscriptionExtensionInDays = CType(AppSettings("SubscriptionExtensionInDays"), Integer)
			Session("mSubscriptionExtensionInDays") = mSubscriptionExtensionInDays
			'9******************************************************

			AddAttributes()
			SubscriptionReminder() ''Commented for Deccan Only

			Dim Authenticate As New CheckAuthentication(True, Server.MapPath("bin\Authority.xml")) ''Commented for Deccan Only

			'Remote Authentication ------------------------------
			Session("RequestInfo") = Authenticate.RequestInfo
			'----------------------------------------------------

			'Added by Saylee on 3-Apr-2025 for Subscription Extension thru webconfig
			Dim mDays As Integer = 0

			If mSubscriptionExtensionInDays > 0 Then

				Dim mCheck As New CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

				If mCheck.WebAuthentication = False Then
					mDays = mCheck.Number("Days")
					mDays -= mCheck.ElapsedDays
				End If

			End If

			btnCancel.Attributes.Add("OnClick", "javascript:window.close();")
			txtUserName.Focus()

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Private Sub Login(sender As Object, e As EventArgs) Handles btnLogin.Click

		Dim UserName As String = txtUserName.Text
		Dim Password As String = txtPassword.Text
		Dim mlogin As New SI.UTILITY.Login(UserName, Password)

		Try

			lblUserNameError.Visible = False
			Session("ShowDashboardOnLogin") = "True"
			mSubscriptionExtensionInDays = Session("mSubscriptionExtensionInDays")

			If AllowNewAircraft() = False And mSubscriptionExtensionInDays = 0 Then 'Added By Prashant 12-Aug-2014 ALL12082014
				Response.Redirect("Locked.htm")
			End If

			If IsChangePasswordRequested() = False Then

				'Remote Authentication ------------------------------
				Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.password, Session("RequestInfo")) 'Parameter added by kalpesh

				Session("AuthenticatedMessage") = CType(bp.IdentityInfo, BusinessIdentity).AuthenticatedMessage  'added by Kalpesh
				Session("XStatus") = CType(bp.IdentityInfo, BusinessIdentity).XStatus 'added by Kalpesh
				Session("XStatusMessage") = CType(bp.IdentityInfo, BusinessIdentity).XStatusMessage 'added by Kalpesh
				'---------------------------------------------

			Else

				'Remote Authentication ------------------------------
				Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.password, Session("RequestInfo")) 'Parameter added by Kalpesh

				Session("AuthenticatedMessage") = CType(bp.IdentityInfo, BusinessIdentity).AuthenticatedMessage  'added by Kalpesh
				Session("XStatus") = CType(bp.IdentityInfo, BusinessIdentity).XStatus 'added by Kalpesh
				Session("XStatusMessage") = CType(bp.IdentityInfo, BusinessIdentity).XStatusMessage 'added by Kalpesh
				'---------------------------------------------

				If Thread.CurrentPrincipal.Identity.IsAuthenticated Then 'old code

					Dim mUserList As UserList = UserList.GetUserList(UserName, , HttpContext.Current.User.Identity.Name)
					mUserId = mUserList.Item(UserName).UserID()

					Session("CSLA-Principal") = Threading.Thread.CurrentPrincipal
					HttpContext.Current.User = CType(Session("CSLA-Principal"), System.Security.Principal.IPrincipal)
					'Added by Utkarsh on 10-Jan-2013 For ALL10012013
					HttpContext.Current.Session("StyleSheet") = UserList.GetUserList(UserName, , UserName).Item(UserName).StyleSheet 'End
					'Added by Yogita for Redirect to login page
					Web.Security.FormsAuthentication.SetAuthCookie(UserName, True)
					Session("IsAjaxEnabled") = mUserList.Item(UserName).IsAjaxEnabled       'added by yogita on 14-aug-2013 for showing Ajax pages
					'--------------------------------------------------------------------------------

					SetDbPassword(UserName, Password)
					Dim tmpEventLogID As Guid = MarkLog(Action.Login, UserName, mDBPassword, IPAddress(), MachineName(), Thread.CurrentPrincipal.Identity.IsAuthenticated)
					Session("EventLogID") = tmpEventLogID

					'Added by Kalpesh Sir on 3-Nov-2017
					'To restrict concurrent user login (same user cannot login from multiple PCs)
					SingleSessionPreparation.CreateAndStoreSessionToken(txtUserName.Text)

					ChangePassword()
					'Added by Yogita for Redirect to login page
					Session("IsFromLogin") = "True" 'Added for Seasonal Greetings
					'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
					mModuleList = ModuleList.GetModuleList(AddTopItem:="Select")
					Session("mModuleList") = mModuleList

					mTransactionList = TransactionList.GetTransactionList("Select") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
					Session("mTransactionList") = mTransactionList
					'-----------------
					Response.Redirect("Index.aspx")

				Else 'Added by Vikrant on 23-July-2012 For ALL11072012

					'Remote Authentication ------------------------------
					lblUserNameError.Text = Session("AuthenticatedMessage") 'changed by Kalpesh '"Invalid User Name And Password. Please Try Again!!"
					lblUserNameError.Visible = True
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop12", "showAlert();", True)
					upnlErrorList.Update()
					upnlLoginForm.Update()
					'---------------------------------------------

				End If

			End If

			If Thread.CurrentPrincipal.Identity.IsAuthenticated Then

				Session("CSLA-Principal") = Threading.Thread.CurrentPrincipal
				HttpContext.Current.User = CType(Session("CSLA-Principal"), System.Security.Principal.IPrincipal)
				'Added by Utkarsh on 10-Jan-2013 For  ALL10012013
				HttpContext.Current.Session("StyleSheet") = UserList.GetUserList(UserName, , HttpContext.Current.User.Identity.Name).Item(UserName).StyleSheet 'End
				'Added by Yogita for Redirect to login page
				Web.Security.FormsAuthentication.SetAuthCookie(UserName, True)
				'--------------------------------------------------------------------------------

				SetDbPassword(UserName, Password)
				Dim mUserList As UserList = UserList.GetUserList(UserName, , HttpContext.Current.User.Identity.Name)
				Session("UserId") = mUserList.Item(UserName).UserID()
				Session("IsAjaxEnabled") = mUserList.Item(UserName).IsAjaxEnabled       'added by yogita on 14-aug-2013 for showing Ajax pages
				'Added by Kalpesh Shah
				RegistrationDetails()

				'Added by Kalpesh Shah
				If Not HttpContext.Current.User.IsInRole("IsAccessOutSideLAN") Then
					IsAccessOutSideLAN()
				End If
				Session("mAircraftInformationBoardList") = Nothing   'Added by Saylee on 30-Apr-2012 for ALL30042012

				'--- 'BTPLAdmin' login check - Other than BTPL static IP
				Try

					Dim properties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()

					If (UCase(Trim(mlogin.UserName)).Equals("BTPLADMIN")) And properties.DomainName <> "btpl.local" And
									 Not (Me.Request.UserHostAddress.Contains("27.107.137.5") _
									 Or Me.Request.UserHostAddress.Contains("27.107.47.118") _
									 Or Me.Request.UserHostAddress.Contains("127.0.0.1") _
									 Or Me.Request.UserHostAddress.Contains("::1")) Then

						Try

							mTempUserForOTP = SI.UTILITY.User.GetUser(mlogin.UserName)

						Catch ex As Exception

							Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.password, Session("RequestInfo"))
							Session("AuthenticatedMessage") = CType(bp.IdentityInfo, BusinessIdentity).AuthenticatedMessage
							lblUserNameError.Text = Session("AuthenticatedMessage")
							lblUserNameError.Visible = True
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop12", "showAlert();", True)
							upnlErrorList.Update()
							upnlLoginForm.Update()

						End Try

						Session("TempUserForOTP") = mTempUserForOTP
						GenerateOTP(mlogin, mTempUserForOTP, Nothing)

						Exit Sub

					End If

				Catch ex As Exception
					Throw ex
				End Try
				'------------------------------------------------

				If VerifyLoginRule(mlogin) Then

					Dim tmpEventLogID As Guid = MarkLog(Action.Login, UserName, mDBPassword, IPAddress(), MachineName(), Thread.CurrentPrincipal.Identity.IsAuthenticated)
					Session("EventLogID") = tmpEventLogID

					'Added by Yogita for Redirect to login page
					Session("ShowDashboardOnLogin") = "True"

					'Added by Kalpesh Sir on 3-Nov-2017
					'To restrict concurrent user login (same user cannot login from multiple PCs)
					SingleSessionPreparation.CreateAndStoreSessionToken(txtUserName.Text)
					Session("IsFromLogin") = "True" 'Added for Seasonal Greetings

					'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
					mModuleList = ModuleList.GetModuleList(AddTopItem:="Select")
					Session("mModuleList") = mModuleList

					mTransactionList = TransactionList.GetTransactionList("Select") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
					Session("mTransactionList") = mTransactionList
					'-----------------

					'16-Feb-2024 Concurrent User implementation by Kalpesh
					mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
					Session("CorporateID") = mCompanyDetail.CorporateID ' "CON" 'Used in LocalFunction.htm & LocalFunctionAjax.htm
					Session("IsAllowConcurrentLogin") = mCompanyDetail.IsAllowConcurrentLogin 'Used in LocalFunction.htm & LocalFunctionAjax.htm
					Session("AllowActiveUsers") = mCompanyDetail.AllowActiveUsers 'Used in LocalFunction.htm & LocalFunctionAjax.htm
					Session("LoginSession") = Guid.NewGuid.ToString 'Used in LocalFunction.htm & LocalFunctionAjax.htm
					Session("UserNameForConcurrentLogin") = UCase(User.Identity.Name) 'Used in LocalFunction.htm & LocalFunctionAjax.htm

					If UCase(User.Identity.Name) <> UCase("BTPLAdmin") Then

						Dim Result As String = SaveNewUserLoginSession()

						If Result <> "" Then
							Exit Sub
						End If

					End If
					'-----------------------------------

					Response.Redirect("Index.aspx")

				Else
					Exit Sub
				End If

			Else

				SetDbPassword(UserName, Password)
				MarkLog(Action.Login, UserName, mDBPassword, IPAddress(), MachineName(), Thread.CurrentPrincipal.Identity.IsAuthenticated)

				'Remote Authentication ------------------------------
				lblUserNameError.Text = Session("AuthenticatedMessage") 'changed by Kalpesh '"Invalid User Name And Password. Please Try Again!!"
				lblUserNameError.Visible = True
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop12", "showAlert();", True)
				upnlErrorList.Update()
				upnlLoginForm.Update()
				'---------------------------------------------

			End If


			'Added at 26-Jul-2016 by bhushan for OTP password generation change
			VerifyLoginRule(mlogin)
			'End
			'Changed by Kalpesh Shah
			If chkIsLocked.Checked And chkIsLocked.Text = "Expired" Then

				Session("CSLA-Principal") = Nothing
				HttpContext.Current.User = Nothing
				Response.Redirect("Locked.htm")

			End If

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Protected Sub EnterPassword(sender As Object, e As EventArgs) Handles lnkArrow.Click

		Try

			If txtUserName.Text = "" Then
				lblUserNameError.Visible = True
				lblUserNameError.Text = "Please enter username."
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop12", "showAlert();", True)
				upnlErrorList.Update()
				upnlLoginForm.Update()
			Else
				password.Visible = True
				lnkArrow.Enabled = False
				lblUserNameError.Visible = False
				txtPassword.Focus()

			End If

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

#End Region

#Region " OTP Generation "

	Protected Sub GenerateOTP(sender As Object, e As EventArgs)
		GenerateOTPClick()
	End Sub

	Protected Sub btnCloseGenerateOTP_Click(sender As Object, e As EventArgs)
		lblInvalid.Text = ""
		HideOTPModalpopup()
		upnlGenerateOTP.Update()
	End Sub

	Protected Sub btnVerifyOTP_Click(sender As Object, e As EventArgs) Handles btnSubmitOTP.Click
		Try
			SetFocus(txtGenerateOTP)
			mTempUserForOTP = Session("TempUserForOTP")

			Dim mlogin As New SI.UTILITY.Login(mTempUserForOTP.Name, mTempUserForOTP.Password)
			BusinessPrincipal.login(mTempUserForOTP.Name, mTempUserForOTP.Password)

			mUserOTP = UserOTP.GetUserOTP(mTempUserForOTP.UserID, "", CInt(Session("LoginRuleID")))
			If mUserOTP.OTP.Equals(Trim(txtGenerateOTP.Text)) Then

				If (Now <= mUserOTP.ValidDateTime) Then

					mUserOTP.IsUsed = True
					mUserOTP.Save()
					HideOTPModalpopup()

					txtResetPasswordUser.Text = mTempUserForOTP.Name


					If mUserOTP.RuleID = 1 Then  'Max. Login Attempt

						OpenPopupChangePassword()
						btnSubmitOTP.Enabled = False
					ElseIf mUserOTP.RuleID = 2 Then  'Browser and IP Address

						'' MSGBoxCtrl.show("Login Setting", "Do you want to add this configuration to your login setting?", "", MsgBoxStyle.YesNo, "BrowserAndIPAddress")
					Else

						Dim tmpEventLogID As Guid = MarkLog(Util.Action.Login, mTempUserForOTP.Name, mTempUserForOTP.DBPassword, IPAddress(), MachineName(), IsAuthenticated:=True)
						Session("EventLogID") = tmpEventLogID
						Session("ShowDashboardOnLogin") = "True"


						'Added by Kalpesh Sir on 3-Nov-2017
						'To restrict concurrent user login (same user cannot login from multiple PCs)
						'
						SingleSessionPreparation.CreateAndStoreSessionToken(txtUserName.Text)
						Session("IsFromLogin") = "True" 'Added for Seasonal Greetings
						'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
						mModuleList = ModuleList.GetModuleList(AddTopItem:="Select")
						Session("mModuleList") = mModuleList

						mTransactionList = TransactionList.GetTransactionList("Select") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
						Session("mTransactionList") = mTransactionList
						'-----------------


						'16-Feb-2024 Concurrent User implementation by Kalpesh
						'
						mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

						Session("CorporateID") = mCompanyDetail.CorporateID ' "CON" 'Used in LocalFunction.htm & LocalFunctionAjax.htm
						Session("IsAllowConcurrentLogin") = mCompanyDetail.IsAllowConcurrentLogin 'Used in LocalFunction.htm & LocalFunctionAjax.htm
						Session("AllowActiveUsers") = mCompanyDetail.AllowActiveUsers 'Used in LocalFunction.htm & LocalFunctionAjax.htm
						Session("LoginSession") = Guid.NewGuid.ToString 'Used in LocalFunction.htm & LocalFunctionAjax.htm
						Session("UserNameForConcurrentLogin") = UCase(User.Identity.Name) 'Used in LocalFunction.htm & LocalFunctionAjax.htm

						If UCase(User.Identity.Name) <> UCase("BTPLAdmin") Then
							Dim Result As String = SaveNewUserLoginSession()

							If Result <> "" Then
								Exit Sub
							End If

						End If
						'-----------------------------------

						Response.Redirect("Index.aspx")
					End If
				Else
					lblOTPErrorMsg.Visible = True
					lblOTPErrorMsg.Text = "OTP expired."
				End If
				Session("LoginRuleID") = Nothing
				mTempUserForOTP = Nothing
			Else
				lblOTPErrorMsg.Visible = True
				lblOTPErrorMsg.Text = IIf(txtGenerateOTP.Text.Trim = "", "Please enter OTP.", "InValid OTP.")
			End If
			upnlGenerateOTP.Update()
			upnlChangePasswordOTP.Update()
		Catch ex As Exception
		End Try
	End Sub

	Private Function BodyMessage(mUserOTP As UserOTP, mTempUserForOTP As User) As String
		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Message As String = String.Empty

		Message = "<html>"
		Message = Message + "<head>"
		Message = Message + "</head>"
		Message = Message + "<body>"
		Message = Message + "Dear user,<p>"
		Message = Message + " (" + mCompanyDetail.CompanyName + ")" + ",<p>"
		Message = Message + "<font size=""3"">"
		Message = Message + "FlyPal® user login " + "<B><font size=""5"">" + mTempUserForOTP.Name + "</font></B>" + " needs to be validated by entering One Time Password [OTP]." + "</font></p>"
		Message = Message + "OTP: " + mUserOTP.OTP.ToString
		Message = Message + "<font size=""2""><BR>(Please Note, This OTP will be valid for next 00:30 Mins to verify your identity.)<BR></font>"
		Message = Message + "<p><BR><span style=""font-size: 11pt"">Best Regards,</span><font style=""font-size: 11pt""></p>"
		Message = Message + "<p>FlyPal Team.</p>"
		Message = Message + "</body>"
		Message = Message + "</html>"

		Return Message

	End Function

	Private Function BodyMessageForReset(mTempUserForOTP As User) As String
		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

		Dim Message As String = String.Empty

		Message = "<html>"
		Message = Message + "<head>"
		Message = Message + "</head>"
		Message = Message + "<body>"
		Message = Message + "Dear user,<p>"
		Message = Message + " (" + mCompanyDetail.CompanyName + ")" + ",<p>"
		Message = Message + "<font size=""3"">"
		Message = Message + "Your FlyPal® account " + "<B><font size=""5"">" + mTempUserForOTP.Name + ", " + "</font></B>" + "Password has been reset successfully." + "</font></p>"
		Message = Message + "<p><BR><span style=""font-size: 11pt"">Best Regards,</span><font style=""font-size: 11pt""></p>"
		Message = Message + "<p>FlyPal Team.</p>"
		Message = Message + "</body>"
		Message = Message + "</html>"

		Return Message


	End Function

	Private Function BodyMessageForLocked(mTempUserForOTP As User, mLoginRule As LoginRuleList.LoginRuleInfo) As String

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Message As String = String.Empty

		Message = "<html>"
		Message = Message + "<head>"
		Message = Message + "</head>"
		Message = Message + "<body>"
		Message = Message + "Dear user,<p>"
		Message = Message + " (" + mCompanyDetail.CompanyName + ")" + ",<p>"
		Message = Message + "<font size=""3"">"
		Message = Message + "FlyPal® user login: " + "<B><font size=""5"">" + mTempUserForOTP.Name + "</font></B>" + " has been locked on account of maximum attempts to login with invalid credentials. You will need to request for an OTP (One Time Password) on your registered email ID with this user login to continue further. In case of any difficulty in obtaining the OTP please contact support@bytzsoft.com for assistance." + "</font></p>"
		Message = Message + "<BR><BR><B>Why we locked your account?</B> : We take security very seriously and we want to keep you in the loop on important actions in your account. "
		Message = Message + "We were unable to determine whether you have forgotten your Password, or someone else is accessing your account."
		Message = Message + "<p><BR><span style=""font-size: 11pt"">Best Regards,</span><font style=""font-size: 11pt""></p>"
		Message = Message + "<p>FlyPal Team.</p>"
		Message = Message + "</body>"
		Message = Message + "</html>"

		Return Message
	End Function

	Protected Sub btnChangePasswordCancel0_Click(sender As Object, e As System.EventArgs) Handles btnChangePasswordCancel0.Click

		Session.Remove("IsChangePwdRequestedByUser")     'Bootstrap (Added at 30-Nov-2018)

		HidePopupChangePassword()
	End Sub

	Protected Sub btnChangePasswordSave_Click(sender As Object, e As EventArgs) Handles btnChangePasswordSave.Click
		SetFocus(txtResetPasswordNewPassword)
		Try

			If txtResetPasswordNewPassword.Text.Equals(String.Empty) Then
				lblChangePasswordError.Visible = True
				lblChangePasswordError.Text = "New Password Required"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop17", "showAlertChangePassword();", True)
				upnlChangePasswordError.Update()
				Exit Sub
			End If
			If txtResetPasswordConfirmPassword.Text.Equals(String.Empty) Then
				lblChangePasswordError.Visible = True
				lblChangePasswordError.Text = "Confirm password Required"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop17", "showAlertChangePassword();", True)
				upnlChangePasswordError.Update()
				Exit Sub
			End If
			If Not String.Equals(txtResetPasswordNewPassword.Text, txtResetPasswordConfirmPassword.Text) Then
				lblChangePasswordError.Visible = True
				lblChangePasswordError.Text = "Password and New Password are not Same."
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop17", "showAlertChangePassword();", True)
				upnlChangePasswordError.Update()
				Exit Sub
			End If


			mTempUserForOTP = Session("TempUserForOTP")

			Dim mlogin As New SI.UTILITY.Login(mTempUserForOTP.Name, mTempUserForOTP.Password)
			BusinessPrincipal.login(mTempUserForOTP.Name, mTempUserForOTP.Password)

			Page.Validate("a")
			If IsValid Then

				mTempUserForOTP.Password = txtResetPasswordNewPassword.Text.Trim
				mTempUserForOTP.ConfirmPassword = txtResetPasswordConfirmPassword.Text.Trim

				If mTempUserForOTP.IsValid Then

					mTempUserForOTP.ApplyEdit()
					mTempUserForOTP = CType(mTempUserForOTP.Save, User)
					HidePopupChangePassword()

					Try
						If UCase(mlogin.UserName.Trim).Equals("BTPLADMIN") Then
							MailID = "support@bytzsoft.com"
						Else
							MailID = mTempUserForOTP.UserEmail.Trim
						End If
						'P
						SendMailFile.SendMailFile(, mTempUserForOTP.Name, "FlyPal Login Password Reset.", "", "", , MailID.Trim, "", "", , , True, BodyMessageForReset(mTempUserForOTP))
					Catch ex As SqlException
						If ex.Number = 8145 Then
							''MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							lblInvalid.Text = ex.Procedure
							Exit Sub
						ElseIf ex.Number = 2627 Then
							''  MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							lblInvalid.Text = ex.Procedure
							Exit Sub
						End If
					End Try

					'------------------------------------------------

					mlogin = New SI.UTILITY.Login(mTempUserForOTP.Name, mTempUserForOTP.Password)
					Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.password, Session("RequestInfo")) 'Parameter added by Kalpesh

					Session("AuthenticatedMessage") = CType(bp.IdentityInfo, BusinessIdentity).AuthenticatedMessage  'added by Kalpesh
					Session("XStatus") = CType(bp.IdentityInfo, BusinessIdentity).XStatus 'added by Kalpesh
					Session("XStatusMessage") = CType(bp.IdentityInfo, BusinessIdentity).XStatusMessage 'added by Kalpesh
					'---------------------------------------------


					If Thread.CurrentPrincipal.Identity.IsAuthenticated Then

						Dim mUserList As UserList = UserList.GetUserList(mTempUserForOTP.Name, , HttpContext.Current.User.Identity.Name)
						mUserId = mUserList.Item(mTempUserForOTP.Name).UserID()

						Session("CSLA-Principal") = Threading.Thread.CurrentPrincipal
						HttpContext.Current.User = CType(Session("CSLA-Principal"), System.Security.Principal.IPrincipal)
						'Added by Utkarsh on 10-Jan-2013 For ALL10012013
						HttpContext.Current.Session("StyleSheet") = UserList.GetUserList(mTempUserForOTP.Name, , mTempUserForOTP.Name).Item(mTempUserForOTP.Name).StyleSheet
						'End
						'Commneted by Yogita for Redirect to login page ---------------------------------
						'
						'Web.Security.FormsAuthentication.RedirectFromLoginPage(UserName, False)
						'
						'Added by Yogita for Redirect to login page
						'
						Web.Security.FormsAuthentication.SetAuthCookie(mTempUserForOTP.Name, True)
						Session("IsAjaxEnabled") = mUserList.Item(mTempUserForOTP.Name).IsAjaxEnabled       'added by yogita on 14-aug-2013 for showing Ajax pages
						'
						'--------------------------------------------------------------------------------

						''MarkLog(Util.Action.Login, UserName, Password, IPAddress(), System.Environment.MachineName, Thread.CurrentPrincipal.Identity.IsAuthenticated)

						''MarkLog(Util.Action.Login, UserName, Password, IPAddress(), MachineName(), Thread.CurrentPrincipal.Identity.IsAuthenticated)
						SetDbPassword(mTempUserForOTP.Name, mTempUserForOTP.Password)
						Dim tmpEventLogID As Guid = MarkLog(Util.Action.Login, mTempUserForOTP.Name, mDBPassword, IPAddress(), MachineName(), Thread.CurrentPrincipal.Identity.IsAuthenticated)
						Session("EventLogID") = tmpEventLogID
						Session("UserId") = mUserList.Item(mTempUserForOTP.Name).UserID()
						'Added by Kalpesh Sir on 3-Nov-2017
						'To restrict concurrent user login (same user cannot login from multiple PCs)
						'
						SingleSessionPreparation.CreateAndStoreSessionToken(txtUserName.Text)
						Session("IsFromLogin") = "True" 'Added for Seasonal Greetings
						'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
						mModuleList = ModuleList.GetModuleList(AddTopItem:="Select")
						Session("mModuleList") = mModuleList

						mTransactionList = TransactionList.GetTransactionList("Select") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
						Session("mTransactionList") = mTransactionList
						'-----------------
						Response.Redirect("Index.aspx")

					End If

				Else

					If mTempUserForOTP.GetBrokenRulesCollection.Count > 0 Then
						Dim i As Integer
						Dim str As String = ""

						For i = 0 To mTempUserForOTP.GetBrokenRulesCollection.Count - 1
							str = str + mTempUserForOTP.GetBrokenRulesCollection.Item(i).Description + "<br>"
						Next

						lblChangePasswordError.Visible = True
						lblChangePasswordError.Text = str
						upnlChangePasswordError.Update()
					End If

				End If

			End If


		Catch ex As Exception

		End Try
	End Sub

#End Region

#Region " Notification(s) "

	'Used for Notification
	<Services.WebMethod()>
	Public Shared Function GetMessages() As Integer
		Return 0
	End Function

#End Region

#Region " Concurrent User "

	'16-Feb-2024 Concurrent User implementation by Kalpesh
	Private Function SaveNewUserLoginSession() As String

		Try
			Dim a As New UserLoginSession
			a.SaveNewUserLoginSession(New Guid(Session("UserId").ToString), New Guid(Session("LoginSession").ToString), New SmartDate(Now).Date.ToString)

			Return ""
		Catch ex As Exception

			lblUserNameError.Visible = True
			lblUserNameError.Text = ex.Message
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop12", "showAlert();", True)
			upnlErrorList.Update()
			upnlLoginForm.Update()

			Return ex.Message

		End Try

	End Function

#End Region

End Class