Partial Class wfAboutFlyPal
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

#Region " Variable Declaration "
	Dim mAboutFlyPal As New AboutFlyPal
	Dim mCompanyDetail As New CompanyDetail
	Shared mPBHList As PBHList  'Added By Vikrant on 01-Dec-2021 for PBH
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mAboutFlyPal = Session("mAboutFlyPal")
		mPBHList = Session("mPBHList")  'Added By Vikrant on 01-Dec-2021 for PBH
	End Sub
	Private Sub SetSession()
		Session("mAboutFlyPal") = mAboutFlyPal
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBinding()
		mAboutFlyPal = AboutFlyPal.GetDetailAboutFlyPal()
		Session("mAboutFlyPal") = mAboutFlyPal
		'Added By Vikrant on 01-Dec-2021 for PBH
		mPBHList = PBHList.GetList(IsAllRecordsRequired:=1) '1: Records with IsRenewed = 0 for each aircraft
		Session("mPBHList") = mPBHList
		dgPBHList.DataSource = mPBHList
		dgPBHList.DataBind()
		SetGrid()
		dgPBHList.Columns(2).Visible = Not mPBHList.IsCombinedHrs

		'End
	End Sub
	'Added By Vikrant on 01-Dec-2021 for PBH
	Private Sub SetGrid()
		Dim RemainingDays As Integer
		Dim RemainingHoursDecimal As Decimal
		Dim FreqHoursDecimal As Decimal
		For j As Integer = 0 To dgPBHList.Rows.Count - 1
			RemainingDays = CType(dgPBHList.Rows.Item(j).Cells(9).Text, Integer)
			RemainingHoursDecimal = CType(dgPBHList.Rows.Item(j).Cells(10).Text, Decimal)
			FreqHoursDecimal = CType(dgPBHList.Rows.Item(j).Cells(11).Text, Decimal)

			If RemainingDays <= 0 OrElse (FreqHoursDecimal > 0 And RemainingHoursDecimal <= 0) Then
				dgPBHList.Rows.Item(j).BackColor = Color.OrangeRed
				dgPBHList.Rows.Item(j).ToolTip = "Subscription Expired"
				dgPBHList.Rows.Item(j).ForeColor = Color.White
			ElseIf RemainingDays < 30 OrElse RemainingHoursDecimal < 1800 Then '1800=30 Hrs
				dgPBHList.Rows.Item(j).BackColor = Color.Yellow
				dgPBHList.Rows.Item(j).ToolTip = "Subscription Expiring"
				dgPBHList.Rows.Item(j).ForeColor = Color.Black
			End If

		Next
	End Sub
	<System.Web.Services.WebMethod(EnableSession:=True)> _
	Public Shared Function SignOut() As String
		Dim Str As String = ""
		Dim SubscriptionExpiredAircraftCount As Integer = 0
		mPBHList = PBHList.GetList(IsAllRecordsRequired:=1) '1: Records with IsRenewed = 0 for each aircraft

		If mPBHList.Count > 0 Then
			For i As Integer = 0 To mPBHList.Count - 1
				If mPBHList(i).RemainingDays <= 0 OrElse (mPBHList(i).HoursFrequencyDec > 0 And mPBHList(i).RemainingHoursDec <= 0) Then
					SubscriptionExpiredAircraftCount += 1
				End If
			Next
			If mPBHList.Count = SubscriptionExpiredAircraftCount Then 'All Aircraft Subscription expired then logout automatically
				Web.Security.FormsAuthentication.SignOut()
				HttpContext.Current.Session.Abandon()
				MarkLog(Util.Action.Logoff)
				Thread.CurrentPrincipal = Nothing
				Str = "Login.aspx"
			End If
		End If

		Return Str
	End Function
	'End
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then
			DataFieldBinding()
		End If
		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		lbk.Text = mCompanyDetail.CompanyName
		lblFlyPalVersion.Text = AppSettings("Product Version")
		lblReleaseNo.Text = mAboutFlyPal.LastUpdateNo.ToString
		lblLastUpdatedDate.Text = mAboutFlyPal.LastUpdateDateFormatted.ToString
		lblCode.Text = AppSettings("ClientCode")
		Dim mDays As Integer = 0
		Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

		Dim mMachineNameValueList As MachineNameValueList
		Dim ReadOnlyAircraftCount As Integer = 0
		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString)
		For i As Integer = 0 To mMachineNameValueList.Count - 1
			If mMachineNameValueList(i).ROCntxt Then
				ReadOnlyAircraftCount += 1
			End If
		Next

		'lblAircraftLicense.Text = mCheck.Number("Aircraft").ToString
		lblAircraftLicense.Text = mCheck.Number("Aircraft").ToString + " (" + (mMachineNameValueList.Count - ReadOnlyAircraftCount).ToString + " Active " + IIf(ReadOnlyAircraftCount > 0, ReadOnlyAircraftCount.ToString + " ReadOnly", "") + ")"

		'lnkWebsite.Attributes.Add("OnClick", "javascript:openHelp();")
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

			lblDaysRemaining.Text = mDays.ToString

			If AppSettings("Mode") = "Subscription" Then
				lblSubscriptionvalidtill.Text = "Subscription valid till :"
			Else
				lblSubscriptionvalidtill.Text = "AMC valid till :"
			End If

			If AppSettings("DateFormat") IsNot Nothing Then
				Dim str As String = AppSettings("DateFormat").ToString
				lblSubscription.Text = Format(Today.Date.AddDays(mDays), str) & "," & " 23:59" & " IST (GMT +05:30)"
			Else
				lblSubscription.Text = Format(Today.Date.AddDays(mDays), "dd-MMM-yyyy") & "," & " 23:59" & " IST (GMT +05:30)"
			End If

			If mCheck.Number("User") <= 0 Then
				lblUserLicense.Text = mCheck.Number("Aircraft").ToString
			Else
				lblUserLicense.Text = mCheck.Number("User").ToString
			End If
		End If
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim str As String
		str = "<script language=javascript>  window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
	End Sub

#End Region


End Class
