Public Class wfrptFlyPalStatusReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim ToDate As String
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
#End Region

#Region "Business Methods"
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub

#End Region

#Region "Helper Methods"
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsFlyPalStatusReport
        Dim da As New CSLA.Data.ObjectAdapter
        Dim obj As rptFlypalStatusList
        Dim mCompanyDetail As New CompanyDetail
        Dim mDays As Integer = 0

        ToDate = txtToDate.Text.ToString

        myReport = New crFlypalStatusReport
        'myReport = New MyReport

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        'mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        'mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        '" ", "", New SmartDate(txtToDate.Value.ToString).FormattedText, "", "", "", AppSettings("Product Version"), AppSettings("SINote"))


        obj = rptFlypalStatusList.GetFlypalStatusList(txtToDate.Text.ToString)

        obj.ClientName = mCompanyDetail.CompanyName
        obj.SubscriptionType = AppSettings("Mode").ToString
        Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
        If mCheck.WebAuthentication = True Then

            obj.NoOfAircraftLicence = mCheck.Number("Aircraft").ToString

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

            'obj.DaysRemaining = CInt(strOutString) - mCheck.ElapsedDays
            obj.DaysRemaining = mDays

            If Not AppSettings("DateFormat") Is Nothing Then
                Dim str1 As String = AppSettings("DateFormat").ToString
                obj.SubsritptionValidTill = Format(Today.Date.AddDays(mDays), str1) & "," & " 23:59" & " IST (GMT +05:30)"
            Else
                obj.SubsritptionValidTill = Format(Today.Date.AddDays(mDays), "dd-MMM-yyyy") & "," & " 23:59" & " IST (GMT +05:30)"
            End If

            If mCheck.Number("User") <= 0 Then
                obj.NoOfUserLicence = mCheck.Number("Aircraft").ToString
            Else
                obj.NoOfUserLicence = mCheck.Number("User").ToString
            End If
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
          "FlyPal Status Report", New SmartDate(txtToDate.Text.ToString).FormattedText, obj.SubscriptionType, obj.NoOfAircraftLicence, obj.DaysRemaining, obj.SubsritptionValidTill, AppSettings("Product Version"), AppSettings("SINote"), obj.NoOfUserLicence, obj.TotalNoOfParts, obj.NoOfLoginLastMonth)

        'If case Added By Shital On 20-Sep-2016
        If ByMail = False Then
            If obj.Count <= 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfrptFlyPalStatusReport.aspx?"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
                'Else
                '    
                '   RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 725)
            End If
        End If

        If (ByMail = True And obj.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Flypal Status Report", "Flypal Status  Report", "There is no record for this search criteria.", _
                "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                 SmtpHost:=mModuleList.Item("FlyPal Status Report").SmtpHost, SmtpPort:=mModuleList.Item("FlyPal Status Report").SmtpPort, SmtpUser:=mModuleList.Item("FlyPal Status Report").SmtpUser, SmtpPassword:=mModuleList.Item("FlyPal Status Report").SmtpPassword)
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, obj)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        If (ByMail = True) Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Flypal Status  Report", "Flypal Status  Report", _
                                      " As On Date " + txtToDate.Text, , Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, _
                                      Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                       SmtpHost:=mModuleList.Item("FlyPal Status Report").SmtpHost, SmtpPort:=mModuleList.Item("FlyPal Status Report").SmtpPort, SmtpUser:=mModuleList.Item("FlyPal Status Report").SmtpUser, SmtpPassword:=mModuleList.Item("FlyPal Status Report").SmtpPassword)
        Else
            Dim Str As String
            'Str = "<script language=Javascript>openTranDetail();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If


    End Sub
  
#End Region
#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not IsPostBack Then
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            lblToDate.Text = "As On Date     :" & New SmartDate(txtToDate.Text.ToString).FormattedText

        End If
        'setFocus(txtToDate) 
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType SmartDate(txtToDate.Text.ToString).FormattedText()
    End Sub

    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        If txtToDate.Text.ToString = "" Then
            lblToDate.Text = "As On Date     : " & New SmartDate("1-1-2200").FormattedText
        Else
            lblToDate.Text = "As On Date     :" & New SmartDate(txtToDate.Text.ToString).FormattedText
        End If
    End Sub

    'Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
    '    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    '    Dim ds As New dsFlyPalStatusReport
    '    Dim da As New CSLA.Data.ObjectAdapter
    '    Dim obj As rptFlypalStatusList
    '    Dim mCompanyDetail As New CompanyDetail
    '    Dim mDays As Integer = 0

    '    ToDate = txtToDate.Text.ToString

    '    myReport = New crFlypalStatusReport
    '    'myReport = New MyReport

    '    mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

    '    'Dim Report As New ReportData(mCompanyDetail.CompanyName, _
    '    'mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
    '    'mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
    '    '" ", "", New SmartDate(txtToDate.Value.ToString).FormattedText, "", "", "", AppSettings("Product Version"), AppSettings("SINote"))


    '    obj = rptFlypalStatusList.GetFlypalStatusList(txtToDate.Text.ToString)

    '    obj.ClientName = mCompanyDetail.CompanyName
    '    obj.SubscriptionType = AppSettings("Mode").ToString
    '    Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
    '    If mCheck.WebAuthentication = True Then

    '        obj.NoOfAircraftLicence = mCheck.Number("Aircraft").ToString

    '        'Changes by Kalpesh in 13-3-2013
    '        'These lines commented
    '        '
    '        'Dim strOutString As String = ReadXMLFile()
    '        'strOutString = strOutString.Split(CChar("$"))(1)
    '        'mDays = CInt(strOutString) - mCheck.ElapsedDays


    '        'Changes by Kalpesh in 13-3-2013
    '        'These lines commented
    '        '
    '        mDays = mCheck.Number("Days")
    '        mDays = mDays - mCheck.ElapsedDays
    '        '---------------------------------

    '        'obj.DaysRemaining = CInt(strOutString) - mCheck.ElapsedDays
    '        obj.DaysRemaining = mDays

    '        If Not AppSettings("DateFormat") Is Nothing Then
    '            Dim str1 As String = AppSettings("DateFormat").ToString
    '            obj.SubsritptionValidTill = Format(Today.Date.AddDays(mDays), str1) & "," & " 23:59" & " IST (GMT +05:30)"
    '        Else
    '            obj.SubsritptionValidTill = Format(Today.Date.AddDays(mDays), "dd-MMM-yyyy") & "," & " 23:59" & " IST (GMT +05:30)"
    '        End If

    '        If mCheck.Number("User") <= 0 Then
    '            obj.NoOfUserLicence = mCheck.Number("Aircraft").ToString
    '        Else
    '            obj.NoOfUserLicence = mCheck.Number("User").ToString
    '        End If
    '    End If

    '    Dim Report As New ReportData(mCompanyDetail.CompanyName, _
    '    mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
    '    mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
    '      "FlyPal Status Report", New SmartDate(txtToDate.Text.ToString).FormattedText, obj.SubscriptionType, obj.NoOfAircraftLicence, obj.DaysRemaining, obj.SubsritptionValidTill, AppSettings("Product Version"), AppSettings("SINote"), obj.NoOfUserLicence, obj.TotalNoOfParts, obj.NoOfLoginLastMonth)


    '    If obj.Count <= 0 Then
    '        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
    '        'msg1.ReplacePage = "wfrptFlyPalStatusReport.aspx?"
    '        'msg1.Show()
    '        MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '        Exit Sub
    '        'Else
    '        '    
    '        '   RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 725)
    '    End If
    '    ds.Clear()
    '    da.Fill(ds, obj)
    '    da.Fill(ds, Report)
    '    myReport.SetDataSource(ds)
    '    Session("CrystalReport") = myReport
    '    Dim Str As String
    '    'Str = "<script language=Javascript>openTranDetail();</script>"
    '    'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    '    Str = "openTranDetail();"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    'End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub

    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Response.Redirect("Dashboard.aspx")
    End Sub

    'Added by Shital on 20-Sep-2016
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click

        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("FlyPal Status Report").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("FlyPal Status Report").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgMELBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgMELBtnSendMail.Click
        Dim email As Thread
        Try
            email = New Thread(Sub() SetReport(True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgMELBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
#End Region

End Class