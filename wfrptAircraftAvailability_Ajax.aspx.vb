Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Flypal.FASFlyingReportOfADayForAllAircraft

Public Class wfrptAircraftAvailability_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mMachineNameValueList As MachineNameValueList
    Dim mrptAircraftAvailabilityReport As rptAircraftAvailabilityReport
    Dim ReportDate As Date
    Dim email As Thread
    Dim mFAScsReportList As FAScsReportList
    Dim MachineName As Guid
    Dim AircraftName As String
    Dim AircraftIds() As Guid
    Dim AsonDate As String = ""
    Dim BeforeAsOnDate As String = ""
    Dim EventLogDetail As String
    Dim mYear As Integer
    Dim mMonth As Integer
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mFAScsReportList = Session("mFAScsReportList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub SetValues()
        AircraftName = String.Empty
        Dim j As Integer = 1
        ReDim AircraftIds(10)
        Dim IsCountGreater As Boolean = False
        For i As Integer = 0 To ChklistAircraft.Items.Count - 1
            If ChklistAircraft.Items(i).Selected Then
                If AircraftName.Length = 0 Then
                    AircraftName = ChklistAircraft.Items(i).Text
                Else
                    AircraftName = AircraftName + "," + ChklistAircraft.Items(i).Text
                End If
                If (j > 10) Then
                    IsCountGreater = True
                    Exit For
                Else
                    AircraftIds(j) = New Guid(ChklistAircraft.Items(i).Value)
                    j = j + 1
                    IsCountGreater = False
                End If
            End If
        Next
    End Sub
    Private Sub SetReport(AircraftIds() As Guid, Optional ByVal ByMail As Boolean = False)
        Dim mCompanyDetail As New CompanyDetail
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                                     mCompanyDetail.WebSite, "", AsonDate, AircraftName, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), _
                                     "", "", "", "", AppSettings("Logo"))

        ReportDate = Today.Date.ToString
        'mrptAircraftAvailabilityReport = rptAircraftAvailabilityReport.GetAircraftAvailabilityReport(CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1, AircraftIds)
        mrptAircraftAvailabilityReport = rptAircraftAvailabilityReport.GetAircraftAvailabilityReport(0, 0, AircraftIds, FromDate:=txtFromDate.Text, _
                                                                                                    ToDate:=txtToDate.Text)
        Dim MyTime, MyDate, MyStr, Company As String
        Dim tmpMachineID1 As Guid = Guid.Empty
        Dim tmpMachineID2 As Guid = Guid.Empty
        Dim tmpMachineID3 As Guid = Guid.Empty
        MyTime = Now.ToString("hh:mm tt")
        MyDate = Now.ToString("dd-MMM-yyyy")
        MyStr = MyDate & "," & MyTime
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Company = mCompanyDetail.CompanyName
        Dim j As Integer = 0
        Dim str As String = ""

        str = str + ("<html>" & "<head>" & "</head>" & "<body >")
        If ByMail Then
            'str = str + ("<P><font face=""Calibri"">Dear " + "User" + ",</font></P> " & "<font face=""Calibri""><p> " & Company & "</font></p>" & "<p> " + "<font face=""Calibri"">Aircraft availability and flying achieved for the month of " & cmbMonth.SelectedItem.Text & " " & cmbYear.SelectedItem.Text & " is appended below: " & ".</font></p> ")
            'str = str + ("<P><font face=""Calibri"">Dear " + "All" + ",</font></P> " & "<p> " + "<font face=""Calibri"">Aircraft availability and flying achieved for the month of " & cmbMonth.SelectedItem.Text & " " & cmbYear.SelectedItem.Text & " is appended below: " & ".</font></p> ")
            str = str + ("<P><font face=""Calibri"">Dear " + "All" + ",</font></P> " & "<p> " + "<font face=""Calibri"">Aircraft availability and flying achieved From " & New SmartDate(txtFromDate.Text).FormattedText & " To " & New SmartDate(txtToDate.Text).FormattedText & " is appended below: " & ".</font></p> ")
        End If
        str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
        str = str + ("<tr>" & "<td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">SI No</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">A/C Type</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">A/C Regn.</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Total Working Days</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">A/C Available (Days)</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Availability % </b>" & "</font>" & "</td><td align=""Center"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Flight Time</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Block Time</b>" & "</font>" & "</td></tr>")

        Dim count = 0
        For j = 0 To mrptAircraftAvailabilityReport.Count - 1
            count = (From rptAircraftAvailabilityReportInfo As rptAircraftAvailabilityReport.rptAircraftAvailabilityReportInfo In mrptAircraftAvailabilityReport
                                Where rptAircraftAvailabilityReportInfo.MachineID = mrptAircraftAvailabilityReport.Item(j).MachineID
                                Select rptAircraftAvailabilityReportInfo).Count()
            str = str + ("<TR>")

            If mrptAircraftAvailabilityReport.Count - 1 = j Then
                str = str + ("<TD WIDTH=200px colspan=""5"" style=""background-color: #009dd9;color: white"" align=""center"">")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + "Total for the Month"
                str = str + ("</font>")
                str = str + ("</TD>")
            Else
                str = str + ("<TD WIDTH=30px align=""left"" rowspan='" & count & "'>")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + (CType(mrptAircraftAvailabilityReport.Item(j).SrNo, String))
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=80px align=""left"" rowspan='" & count & "'>")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + (CType(mrptAircraftAvailabilityReport.Item(j).ModelName, String))
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=80px align=""left"" rowspan='" & count & "'>")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + (CType(mrptAircraftAvailabilityReport.Item(j).RegNo, String))
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=50px align=""left"" rowspan='" & count & "'>")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + (CType(mrptAircraftAvailabilityReport.Item(j).TotalDays, String))
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=50px align=""left"" rowspan='" & count & "'>")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + (CType(mrptAircraftAvailabilityReport.Item(j).TotalACAvailableInDays, String))
                str = str + ("</font>")
                str = str + ("</TD>")
            End If

            str = str + ("<TD WIDTH=50px align=""left"" rowspan='" & count & "'>")
            str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + (CType(mrptAircraftAvailabilityReport.Item(j).TotalAvailablePercent, String))
            str = str + ("</font>")
            str = str + ("</TD>")

            If mrptAircraftAvailabilityReport.Item(j).TotalBlockTime = "0:00" Then
                str = str + ("<TD WIDTH=100px colspan=""2"">")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + "No Flying for the Month"
                str = str + ("</font>")
                str = str + ("</TD>")
            Else
                str = str + ("<TD WIDTH=50px align=""center"">")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + (IIf(CType(mrptAircraftAvailabilityReport.Item(j).TotalFlyingHours, String) = "0:00", "&nbsp;", CType(mrptAircraftAvailabilityReport.Item(j).TotalFlyingHours, String)))
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=50px align=""center"">")
                str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + (IIf(CType(mrptAircraftAvailabilityReport.Item(j).TotalBlockTime, String) = "0:00", "&nbsp;", CType(mrptAircraftAvailabilityReport.Item(j).TotalBlockTime, String)))
                str = str + ("</font>")
                str = str + ("</TD>")
            End If
            str = str + ("</TR>")
        Next
        str = str + ("</TABLE>")
        str = str + ("</body></html>")
        If ByMail = True Then
            str = str + ("<p><font face=""Calibri"">")
            If Session("SendMailRemark").Trim = "" Then
                'Do nothing 
            Else
                str = str + ("<p><font face=""Calibri""> <b> Remark : " & Session("SendMailRemark").Trim & "</b></font></p>")
            End If
            str = str + ("<p><font face=""Calibri""><b>Report generated at : </b>" & MyStr & "</font></p>")
            If Session("ReportGenratedBy").Trim = "" Then
                'Do nothing 
            Else
                str = str + ("<p><font face=""Calibri""><b> Report generated by : " & Session("ReportGenratedBy").Trim & "</b></font></p>")
            End If
            str = str + ("Kindly Login into <b>FlyPal®</b> for more details.</p><p>&nbsp;</p><p>&nbsp;</p>")
            str = str + ("</font>")

            str = str + ("<p><p><font face=""Calibri"">")
            str = str + ("<b>Regards,</b></p></p>")
            str = str + ("</font>")

            str = str + ("<p><font face=""Calibri"">")
            str = str + ("<b>FlyPal® Alerts Service</b></p>")
            str = str + ("</font>")

            str = str + ("<p><font face=""Calibri"">")
            str = str + ("<font color=""#FF0000"">*</font>This is automated Email generated by FlyPal® Alerts Service. Please do not reply.</p>")
            str = str + ("</font>")
        End If
        If ByMail = False Then
            Dim lblText As New Literal
            lblText.Text = str
            tblFlyingEntries.Controls.Add(lblText)
        Else
            SendMailFile.SendMailFile(Nothing, Thread.CurrentPrincipal.Identity.Name, IIf(AppSettings("ClientCode") = "APFT" Or
                                                                                                                       AppSettings("ClientCode") = "AAP", "KPI From ", "Aircraft Availability Report From ") + txtFromDate.Text + " To " + txtToDate.Text,
                                       "", " From " + txtFromDate.Text + " To " + txtToDate.Text, "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True,
                                       MailBody:=str, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                                        SmtpHost:=mModuleList.Item("AircraftAvailabilityReport").SmtpHost, SmtpPort:=mModuleList.Item("AircraftAvailabilityReport").SmtpPort,
                                         SmtpUser:=mModuleList.Item("AircraftAvailabilityReport").SmtpUser, SmtpPassword:=mModuleList.Item("AircraftAvailabilityReport").SmtpPassword)
        End If
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptAircraftAvailability_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mFAScsReportList")
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBinding()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True)
        Session("mMachineNameValueList") = mMachineNameValueList
        ChklistAircraft.DataSource = mMachineNameValueList

        mFAScsReportList = FAScsReportList.GetFAScsReportList()
        Session("mFAScsReportList") = mFAScsReportList

        'Dim i As Integer
        'For i = 0 To 5
        '    cmbYear.Items.Add(Year(Now.Date) - (5 - i))
        'Next
        'For i = 1 To 5
        '    cmbYear.Items.Add(Year(Now.Date) + i)
        'Next

        'mYear = Year(Now.Date)
        'mMonth = Month(Now.Date)

        'cmbYear.SelectedValue = mYear
        'cmbMonth.SelectedValue = mMonth
        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptAircraftAvailability_Ajax.aspx"
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBinding()
        End If
    End Sub
    'Private Sub cmbYear_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbYear.SelectedIndexChanged
    '    Dim month As Integer = cmbMonth.SelectedValue
    '    Dim year As Integer = cmbYear.SelectedValue
    '    Dim days As Integer = System.DateTime.DaysInMonth(year, month)
    'End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            Dim MachineList As Guid()
            Dim RegNoList As String()
            Dim j As Integer = 1
            ReDim MachineList(10)
            ReDim RegNoList(10)
            Dim IsCountGreater As Boolean = False
            Dim IsChecked As Boolean = False
            For i As Integer = 0 To ChklistAircraft.Items.Count - 1
                If ChklistAircraft.Items(i).Selected Then
                    IsChecked = True
                    Exit For
                Else
                    IsChecked = False
                End If
            Next
            If IsChecked = False Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoOfAircrafts, SIMsgBox.Message_text.NoneAircraftsChecked, "", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfServiceabilityEntry.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoOfAircrafts, MSGBox.Message_text.NoneAircraftsChecked, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            AircraftName = String.Empty
            ReDim AircraftIds(10)
            For i As Integer = 0 To ChklistAircraft.Items.Count - 1
                If ChklistAircraft.Items(i).Selected Then
                    If (j > 10) Then
                        IsCountGreater = True
                        Exit For
                    Else
                        AircraftIds(j) = New Guid(ChklistAircraft.Items(i).Value)
                        j = j + 1
                        IsCountGreater = False
                    End If
                End If
            Next
            If IsCountGreater = True Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoOfAircrafts, SIMsgBox.Message_text.NoOfAircrafts, "", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfServiceabilityEntry.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoOfAircrafts, MSGBox.Message_text.NoOfAircrafts, "", MsgBoxStyle.OkOnly, "")
            Else
                SetReport(AircraftIds)
            End If
            upnlDynTable.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "FlyingReport", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        Dim IsChecked As Boolean = False

        For i As Integer = 0 To ChklistAircraft.Items.Count - 1
            If ChklistAircraft.Items(i).Selected Then
                IsChecked = True
                Exit For
            Else
                IsChecked = False
            End If
        Next

        If IsChecked = False Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoOfAircrafts, SIMsgBox.Message_text.NoneAircraftsChecked, "", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfServiceabilityEntry.aspx?BackPage=" & Request.QueryString("BackPage")
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoOfAircrafts, MSGBox.Message_text.NoneAircraftsChecked, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("AircraftAvailabilityReport").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AircraftAvailabilityReport").SendCCMailID
        '--------------------------

        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            AircraftName = String.Empty
            ReDim AircraftIds(10)
            Dim j As Integer = 1
            For i As Integer = 0 To ChklistAircraft.Items.Count - 1
                If ChklistAircraft.Items(i).Selected Then
                    If (j > 10) Then
                        Exit For
                    Else
                        AircraftIds(j) = New Guid(ChklistAircraft.Items(i).Value)
                        j = j + 1
                    End If
                End If
            Next
            email = New Thread(Sub() SetReport(AircraftIds, True))
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
#End Region

End Class