Imports System.Configuration.ConfigurationManager
Imports System.Linq
Imports System.Text
Imports Flypal.FASFlyingReportOfADayForAllAircraft
Public Class wfrptFlyingReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mMachineNameValueList As MachineNameValueList
    Dim mFASFlyingReportOfADayForAllAircraft As FASFlyingReportOfADayForAllAircraft
    Dim ReportDate As Date
    Dim mFAScsReportList As FAScsReportList
    Dim email As Thread

    Dim MachineName As Guid
    Dim Aircraft As String
    Dim AsonDate As String = ""
    Dim BeforeAsOnDate As String = ""
    Dim EventLogDetail As String
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
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Dim mCompanyDetail As New CompanyDetail

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
mCompanyDetail.WebSite, "", AsonDate, Aircraft, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        ReportDate = CDate(txtDate.Text)

        mFASFlyingReportOfADayForAllAircraft = FASFlyingReportOfADayForAllAircraft.GetFASFlyingReportOfADayForAllAircraft(ToDayDate:=txtDate.Text, FromDate:=New Date(ReportDate.Year, ReportDate.Month, 1).ToString, ClientCode:=AppSettings("ClientCode"), MachineID:=cmbAircraft.SelectedValue.ToString)
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

        If AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then
            str = str + ("<html>" & "<head>" & "</head>" & "<body >")
            If ByMail Then
                str = str + ("<P><font face=""Calibri"">Dear " + "All" + ",</font>  " & "<p> " + "<font face=""Calibri"">Kindly find the Flying Report For " & ReportDate.ToString(AppSettings("DateFormat")) & ".</font></p> ")
            Else
                str = str + ("<font face=""Calibri"">Flying Report " + IIf(cmbAircraft.SelectedIndex > 0, "of " + cmbAircraft.SelectedItem.ToString + " For ", "For ").ToString & ReportDate.ToString(AppSettings("DateFormat")) & ".</font> ")
            End If
            str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
            str = str + ("<tr>" & "<td align=""left"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>A/C Type</b>" & "</font>" & "</td><td align=""left"" style=""background-color: white"">" & "<font face=""Inter, sans-serif""><b>Regn. No.</b>" & "</font>" & "</td><td align=""Inter"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Flight Time</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Block Time</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>No. of Landings</b>" & "</font>" & "</td></tr>")

            Dim count = 0
            For j = 0 To mFASFlyingReportOfADayForAllAircraft.Count - 1
                count = (From FASFlyingReportOfADayForAllAircraftInfo As FASFlyingReportOfADayForAllAircraftInfo In mFASFlyingReportOfADayForAllAircraft
                         Where FASFlyingReportOfADayForAllAircraftInfo.MachineID = mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID
                         Select FASFlyingReportOfADayForAllAircraftInfo).Count()
                str = str + ("<TR>")
                If (Not tmpMachineID3.Equals(mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID)) Then
                    tmpMachineID3 = mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID
                    str = str + ("<TD WIDTH=400px align=""left"" rowspan='" & count & "'>")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
                    str = str + (CType(mFASFlyingReportOfADayForAllAircraft.Item(j).AircraftModel, String))
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If
                If (Not tmpMachineID1.Equals(mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID)) Then
                    tmpMachineID1 = mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID
                    str = str + ("<TD WIDTH=200px align=""left"" rowspan='" & count & "'>")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
                    str = str + (CType(mFASFlyingReportOfADayForAllAircraft.Item(j).RegNo, String))
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If
                If mFASFlyingReportOfADayForAllAircraft.Item(j).BlockTime = "0:00" Then
                    str = str + ("<TD WIDTH=400px colspan=""3"">")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
                    str = str + "No Flying"
                    str = str + ("</font>")
                    str = str + ("</TD>")
                Else
                    str = str + ("<TD WIDTH=50px align=""center"">")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
                    str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TimeInAir, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TimeInAir, String)))
                    str = str + ("</font>")
                    str = str + ("</TD>")

                    str = str + ("<TD WIDTH=50px align=""center"">")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
                    str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(j).BlockTime, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(j).BlockTime, String)))
                    str = str + ("</font>")
                    str = str + ("</TD>")

                    str = str + ("<TD WIDTH=50px align=""center"">")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
                    str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(j).Landings, String) = "0", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(j).Landings, String)))
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If

                If (Not tmpMachineID2.Equals(mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID)) Then
                    tmpMachineID2 = mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID

                    'str = str + ("<TD WIDTH=200px align=""center"" rowspan='" & count & "'>")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    'str = str + (CType(mFASFlyingReportOfADayForAllAircraft.Item(j).LastFlownDateFormatted, String))
                    'str = str + ("</font>")
                    'str = str + ("</TD>")
                End If
                str = str + ("</TR>")
            Next
            str = str + ("<TR>")

            str = str + ("<TD WIDTH=600px align=""left"" colspan='" & 2 & "'>")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + ("<b>Total flying for the day</b>")
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalTimeInAir, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalTimeInAir, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalBlockTime, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalBlockTime, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandings, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandings, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            'str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            ''str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandings, String) = "0:00", "", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandings, String)))
            'str = str + ("</font>")
            'str = str + ("</TD>")

            str = str + ("</TR>")

            str = str + ("<TR>")

            str = str + ("<TD WIDTH=600px align=""left"" colspan='" & 2 & "'>")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + ("<b>Till date flying for the month</b>")
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalTimeInAirForMonth, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalTimeInAirForMonth, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalBlockTimeForMonth, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalBlockTimeForMonth, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandingsForMonth, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandingsForMonth, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            'str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            ''str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandings, String) = "0:00", "", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandings, String)))
            'str = str + ("</font>")
            'str = str + ("</TD>")
            str = str + ("</TR>")

            '-----Added by Shital on 27-Jul-2020
            str = str + ("<TR>")

            str = str + ("<TD WIDTH=600px align=""left"" colspan='" & 2 & "'>")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + ("<b>Till date flying for the Current FY</b>")
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalTimeInAirForFinancialYear, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalTimeInAirForFinancialYear, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalBlockTimeForFinancialYear, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalBlockTimeForFinancialYear, String)))
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""center"">")
            'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
            str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
            str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandingsForFinancialYear, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(mFASFlyingReportOfADayForAllAircraft.Count - 1).TotalLandingsForFinancialYear, String)))
            str = str + ("</font>")
            str = str + ("</TD>")
            str = str + ("</TR>")
            '-----END
        Else
            str = str + ("<html>" & "<head>" & "</head>" & "<body >")
            If ByMail Then
                str = str + ("<P><font face=""Calibri"">Dear " + "User" + ",</font></P> " & "<font face=""Calibri""><p> " & Company & "</font></p>" & "<p> " + "<font face=""Calibri"">Kindly find the Flying Report of All Aircrafts For " & ReportDate.ToString(AppSettings("DateFormat")) & ".</font></p> ")
            End If
            str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
            'str = str + ("<tr>" & "<td align=""left"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Regn. No.</b>" & "</font>" & "</td><td align=""left"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Take-Off Place</b>" & "</font>" & "</td><td align=""left"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Landing Place</b>" & "</font>" & "</td><td align=""Center"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Airborne time</b>" & "</font>" & "</td><td align=""Center"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Block Time</b>" & "</font>" & "</td><td align=""left"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Remark </b>" & "</font>" & "</td><td align=""left"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Total Hours Flown For The Day </b>" & "</font>" & "</td><td align=""left"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Total Hours Flown Till Date </b>" & "</font>" & "</td><td align=""center"" class=""clsdgHeader"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">TLP Updated Till</b>" & "</font>" & "</td></tr>")
            'str = str + ("<tr>" & "<td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Regn. No.</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Take-Off Place</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Landing Place</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Airborne time</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Block Time</b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Remark </b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Total Hours Flown For The Day </b>" & "</font>" & "</td><td align=""left"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">Total Hours Till Date </b>" & "</font>" & "</td><td align=""center"" style=""background-color: #009dd9;"">" & "<font face=""Calibri""><b style=""color: #FFFFFF"">TLP Updated Till</b>" & "</font>" & "</td></tr>")
            'str = str + ("<tr>" & "<td align=""left"" style=""background-color: White;"">" & "<font face=""Inter""><b>Regn. No.</b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter""><b>Take-Off Place</b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter""><b>Landing Place</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: White;"">" & "<font face=""Inter""><b>Airborne time</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: White;"">" & "<font face=""Inter""><b>Block Time</b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter""><b>Remark </b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter""><b>Total Hours Flown For The Day </b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter""><b>Total Hours Till Date </b>" & "</font>" & "</td><td align=""center"" style=""background-color: White;"">" & "<font face=""Inter""><b>TLP Updated Till</b>" & "</font>" & "</td></tr>")
            str = str + ("<tr>" & "<td align=""left"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Regn. No.</b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Take-Off Place</b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Landing Place</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Airborne time</b>" & "</font>" & "</td><td align=""Center"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Block Time</b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Remark </b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Total Hours Flown For The Day </b>" & "</font>" & "</td><td align=""left"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>Total Hours Till Date </b>" & "</font>" & "</td><td align=""center"" style=""background-color: White;"">" & "<font face=""Inter, sans-serif""><b>TLP Updated Till</b>" & "</font>" & "</td></tr>")

            For j = 0 To mFASFlyingReportOfADayForAllAircraft.Count - 1
                Dim count = (From FASFlyingReportOfADayForAllAircraftInfo As FASFlyingReportOfADayForAllAircraftInfo In mFASFlyingReportOfADayForAllAircraft
                             Where FASFlyingReportOfADayForAllAircraftInfo.MachineID = mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID
                             Select FASFlyingReportOfADayForAllAircraftInfo).Count()
                str = str + ("<TR>")
                If (Not tmpMachineID1.Equals(mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID)) Then
                    tmpMachineID1 = mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID
                    str = str + ("<TD WIDTH=200px align=""left"" rowspan='" & count & "'>")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300; background-color: #f2f2f2"">")
                    str = str + (CType(mFASFlyingReportOfADayForAllAircraft.Item(j).RegNo, String))
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If
                If mFASFlyingReportOfADayForAllAircraft.Item(j).FromPlace = "" Then
                    str = str + ("<TD WIDTH=400px colspan=""2"">")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300;background-color: #f2f2f2"">")
                    str = str + "No Flying for the Day"
                    str = str + ("</font>")
                    str = str + ("</TD>")
                Else
                    str = str + ("<TD WIDTH=200px >")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300;background-color: #f2f2f2"">")
                    str = str + (mFASFlyingReportOfADayForAllAircraft.Item(j).FromPlace)
                    str = str + ("</font>")
                    str = str + ("</TD>")

                    str = str + ("<TD WIDTH=200px align=""left"">")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300;background-color: #f2f2f2"">")
                    str = str + (CType(mFASFlyingReportOfADayForAllAircraft.Item(j).ToPlace, String))
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If

                str = str + ("<TD WIDTH=50px align=""center"">")
                'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300;background-color: #f2f2f2"">")
                str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TimeInAir, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TimeInAir, String)))
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=50px align=""center"">")
                'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300;background-color: #f2f2f2"">")
                str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(j).BlockTime, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(j).BlockTime, String)))
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=200px align=""left"">")
                'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300;background-color: #f2f2f2"">")
                str = str + (CType(mFASFlyingReportOfADayForAllAircraft.Item(j).Remark, String))
                str = str + ("</font>")
                str = str + ("</TD>")

                If (Not tmpMachineID2.Equals(mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID)) Then
                    tmpMachineID2 = mFASFlyingReportOfADayForAllAircraft.Item(j).MachineID
                    str = str + ("<TD WIDTH=200px align=""center"" rowspan='" & count & "'>")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300';background-color: #f2f2f2"">")
                    str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TotalHoursFlownForTheDay, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TotalHoursFlownForTheDay, String)))
                    str = str + ("</font>")
                    str = str + ("</TD>")
                    str = str + ("<TD WIDTH=200px align=""center"" rowspan='" & count & "'>")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300 background-color: #f2f2f2"">")
                    str = str + (IIf(CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TSNHours, String) = "0:00", "&nbsp;", CType(mFASFlyingReportOfADayForAllAircraft.Item(j).TSNHours, String)))

                    str = str + ("</font>")
                    str = str + ("</TD>")
                    str = str + ("<TD WIDTH=200px align=""center"" rowspan='" & count & "'>")
                    'str = str + ("<font style=""font-size: 11px; font-family: Verdana; font-weight: 500"">")
                    str = str + ("<font style=""font-size: 9pt; font-family: 'Inter', sans-serif; font-weight: 300"">")
                    str = str + (CType(mFASFlyingReportOfADayForAllAircraft.Item(j).LastFlownDateFormatted, String))
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If
            Next
        End If

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
            SendMailFile.SendMailFile(Nothing, Thread.CurrentPrincipal.Identity.Name, "Flying Report For " + txtDate.Text, "", _
                                      " For " + New SmartDate(txtDate.Text).FormattedText, "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, MailBody:=str, _
                                      Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=mModuleList.Item("FlyingReport").SmtpHost, SmtpPort:=mModuleList.Item("FlyingReport").SmtpPort, SmtpUser:=mModuleList.Item("FlyingReport").SmtpUser, SmtpPassword:=mModuleList.Item("FlyingReport").SmtpPassword)
        End If
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptFlyingReport_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mFAScsReportList")
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(All)", , True, SkipReadOnlyAircrafts:=True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        mFAScsReportList = FAScsReportList.GetFAScsReportList()
        Session("mFAScsReportList") = mFAScsReportList

        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptFlyingReport_Ajax.aspx"
            txtDate.Text = CDate(Today.Date).AddDays(-1).ToString(AppSettings("DateFormat"))
            SetComboOfMachine(txtDate.Text)
            SetReport()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
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
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("FlyingReport").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("FlyingReport").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
#End Region

End Class