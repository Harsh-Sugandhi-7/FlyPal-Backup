'Added by Utkarsh on 03-Feb-2014

Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Text
Imports AjaxControlToolkit

Public Class wfrptDirectiveStatusIssuedReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim StartDate As String
    Dim EndDate As String
    Dim Directive As String
    Dim Aircraft As String
    Dim AircraftIndex As Integer
    Dim Assembly1 As String
    Dim AssemblyType As String
    Dim ModelName As String
    Dim ModTypeName As String
    Dim ModelID As String
    Dim MachineID As Guid
    Dim AssemblyID As Guid
    'Private mMachineList As MachineList  'Commented By Utkarsh On 19-Apr-2011
    'Dim mMachineNameValueList As MachineNameValueList 'Added By Utkarsh On 19-Apr-2011

    'Private mModificationTypeList As ModelMonitorModTypeList
    Private mModTypeList As ModTypeList
    Public Shared mAssemblyList As AssemblyList
    Dim EventLogDetail As String

    'Added by Abhishek on 26-SEP-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim ds As New dsrptDirectiveStatusReport
    Dim Obj As rptDirectiveStatusIssuedReport
    Dim mCompanyDetail As New CompanyDetail
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim mModificationTypeList As ModelMonitorModTypeList
    'Added by Shital on 09-Mar-2020
    Dim ModTypeIds As New StringBuilder
    Dim ModTypeNames As New StringBuilder
    Dim ModelMonitorModTypeIds As New StringBuilder
    Dim ModelMonitorModTypeNames As New StringBuilder

#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        'mMachineList = CType(Session("mMachineList"), MachineList)  'Commented By Utkarsh On 19-Apr-2011
        'mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)  'Added By Utkarsh On 19-Apr-2011
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mModTypeList = CType(Session("mModTypeList"), ModTypeList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        'Session("mMachineList") = mMachineList   'Commented By Utkarsh On 19-Apr-2011
        'Session("mMachineNameValueList") = mMachineNameValueList   'Added By Utkarsh On 19-Apr-2011
        Session("mAssemblyList") = mAssemblyList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptDirectiveStatusIssuedReport_Ajax.aspx" Then
            'Session.Remove("mMachineList")   'Commented By Utkarsh On 19-Apr-2011
            'Session.Remove("mMachineNameValueList")   'Added By Utkarsh On 19-Apr-2011
            Session.Remove("mAssemblyList")
            Session.Remove("mModTypeList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblType1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblAssembly1.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If cmbAircraft.SelectedItem.Text = "(SELECT)" Then
            Aircraft = ""
            Assembly1 = ""
            lblAircraft1.Text = "Aircraft Name : " & Aircraft
            lblAssembly1.Text = "Assembly Name : " & Assembly1
        Else
            MachineID = New Guid(cmbAircraft.SelectedValue.ToString)
            AssemblyID = New Guid(cmbAssembly.SelectedValue.ToString)
            If cmbAssembly.SelectedItem.Text = "(All)" Or cmbAssembly.SelectedItem.Text = "(SELECT)" Then
                Assembly1 = ""
                AssemblyType = "(All)"
                ModelName = ""
                ''lblAssembly1.Text = "Assembly Name  : All"          'Added Code
                lblAssembly1.Text = "Assembly Name  : "
                ModelID = "{00000000-0000-0000-0000-000000000000}"
            Else
                AssemblyType = mAssemblyList(AssemblyID).AssemblyType
                Assembly1 = cmbAssembly.SelectedItem.Text
                ModelName = mAssemblyList(AssemblyID).ModelName
                lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
                ModelID = mAssemblyList(AssemblyID).ModelID.ToString
            End If

            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft Name : " & Aircraft
        End If

        If Not IsDate(txtFromDate.Text.Trim) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.Trim
        End If
        If Not IsDate(txtToDate.Text.Trim) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.Trim
        End If

        '   If cmbModificationType.SelectedItem.Text = "(ALL)" Or cmbModificationType.SelectedItem.Text = "(SELECT)" Or cmbModificationType.SelectedItem.Text = "<SELECT>" Then
        If ListDirectiveType.SelectedItem.Text = "(ALL)" Or ListDirectiveType.SelectedItem.Text = "(SELECT)" Or ListDirectiveType.SelectedItem.Text = "<SELECT>" Then
            ModTypeName = "All"
            lblType1.Text = "Directive Name : " & ModTypeName
        Else
            'ModTypeName = cmbModificationType.SelectedItem.Text
            ModTypeName = ListDirectiveType.SelectedItem.Text
            lblType1.Text = "Directive Name : " & ModTypeName
        End If

        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", StartDate, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", EndDate, "")

        EventLogDetail = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + lblType1.Text
    End Sub
    Private Sub ResetValues()
        AssemblyType = ""
        ModelName = ""
        ModTypeName = ""
        txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text
        ModelID = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Private Sub SetReport(Optional ByVal Bymail As Boolean = False)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsrptDirectiveStatusReport
        Dim Obj As rptDirectiveStatusIssuedReport
        Dim OperatorName As String = ""

        Dim mCompanyDetail As New CompanyDetail
        SetValues()

        If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        End If

        'Added by Shital on 09-Mar-2020
        For K As Integer = 0 To ListDirectiveType.Items.Count - 1
            If ListDirectiveType.Items.Item(K).Selected Then
                ModTypeIds.Append(ListDirectiveType.Items.Item(K).Value + ",")
                ModTypeNames.Append(ListDirectiveType.Items.Item(K).Text + ",")
            End If
        Next

        For P As Integer = 0 To ListDirectiveSubType.Items.Count - 1
            If ListDirectiveSubType.Items.Item(P).Selected Then
                If ModelMonitorModTypeIds.ToString = "" Then
                    ModelMonitorModTypeIds.Append("<ModTypeID>")
                End If
                ModelMonitorModTypeIds.Append("<id>")

                ModelMonitorModTypeIds.Append(ListDirectiveSubType.Items.Item(P).Value)
                ModelMonitorModTypeIds.Append("</id>")
            End If
        Next
        If ModelMonitorModTypeIds.ToString <> "" Then
            ModelMonitorModTypeIds.Append("</ModTypeID>")
        End If
        '------------

        'Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, ModTypeName + " Status Report", Aircraft, Assembly1, ModTypeName, txtFromDate.Text, txtToDate.Text, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:=txtBottomLine.Text.Trim, SearchStr7:=OperatorName, SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"))



        Obj = rptDirectiveStatusIssuedReport.GetrptDirectiveStatusIssuedReport(txtFromDate.Text, txtToDate.Text, MachineID, New Guid(ModelID), ModTypeIDs:=ModelMonitorModTypeIds.ToString, AssemblyID:=AssemblyID, SortBy:=cmbSortBy.SelectedItem.ToString, AscDes:=IIf(optAscending.Checked = True, "Asc", "desc"), Type:=cmbAdType.SelectedItem.ToString)

        If Obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub

            'Added By Utkarsh On 7-Jun-2011 For All07062011

        ElseIf Obj.Count > 0 Then

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1161)

            '*******************************
        End If

        Dim mMachineList As MachineList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""

        Dim mAssemblyStatus As AssemblyStatus
        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyList(New Guid(cmbAssembly.SelectedValue)).AssemblyStatusID)

        'If mAssemblyStatus.IsRemoved Then
        '    mMachineList = MachineList.GetMachineListMonitoringStatus(txtToDate.Text, cmbAircraft.SelectedValue, , , , , , , , , , True, True, mAssemblyList(New Guid(cmbAssembly.SelectedValue)).AssemblyStatusID.ToString, _
        '                                                           , , MonitoringModRequired:=False, _
        '                                                          IsAssemblyRemoved:=True, IsCompRemoved:=False, IsComplied:=True, _
        '                                                          IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=False, _
        '                                                          SkipIsForInventoryAircarft:=True) 'IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        'Else
        '    mMachineList = MachineList.GetMachineListMonitoringStatus(txtToDate.Text, cmbAircraft.SelectedValue, , , , , , , , , , True, True, mAssemblyList(New Guid(cmbAssembly.SelectedValue)).AssemblyStatusID.ToString, _
        '                                                          , , MonitoringModRequired:=False, _
        '                                                         IsAssemblyRemoved:=False, IsCompRemoved:=False, IsComplied:=True, _
        '                                                         IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=False, _
        '                                                         SkipIsForInventoryAircarft:=True) 'IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        'End If
        mMachineList = MachineList.GetMachineListMonitoringStatus(txtToDate.Text, cmbAircraft.SelectedValue, , , , , , mAssemblyStatus.Assembly.SerialNo, , mAssemblyStatus.ModelName, , False, True, ,
                                                                 , , MonitoringModRequired:=False,
                                                                IsAssemblyRemoved:=True, IsCompRemoved:=False, IsComplied:=True,
                                                                IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=False,
                                                                SkipIsForInventoryAircarft:=True)
        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                LHLabel2 = ""
                LHData2 = ""
                For i As Integer = 0 To ObjAssemblyStatus.AssemblyStatusPeriodList.Count - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(i).PeriodID <> 2 Then
                        LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(i).PeriodName
                        LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(i).PeriodID, "").AssemblyCurrentValue
                    End If
                Next
            Next

        Next

        Dim ModShortName As String = ""
        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()

        For i As Integer = 0 To mModificationTypeList.Count - 1

            If ModShortName = "" Then
                ModShortName = IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, mModificationTypeList(i, "").CodeType, "")
            Else
                ModShortName = ModShortName + IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, ", " + mModificationTypeList(i, "").CodeType, "")
            End If
        Next


        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ModTypeNames.ToString.TrimEnd(",") + " Status Report", Aircraft, Assembly1, ModTypeNames.ToString.TrimEnd(","), txtFromDate.Text, txtToDate.Text, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:=txtBottomLine.Text.Trim, SearchStr7:=OperatorName, SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"), SearchStr12:=LHLabel2, SearchStr13:=LHData2, SearchStr14:=ModShortName)


        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, Obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        'myReport = New crptDirectiveStatusIssuedReport
        If cmbFormat.SelectedIndex = 0 Then
            If (AppSettings("ClientCode") IsNot Nothing) AndAlso
               AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then
                myReport = New crptDirectiveStatusIssuedReportAPFT 'Format added by Saylee for APFT18092018 on 18-Sep-2018
            ElseIf AppSettings("ClientCode") = "BAMS" Then
                myReport = New crptDirectiveStatusIssuedReportBAMS 'Format added by Prashant on 11-Feb-2022 AS Details Colum was not showing information. As it was showing in case of APFT client format but not for BASM client so…. 
            ElseIf AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "SUH" Then
                myReport = New crptDirectiveStatusIssuedReportIND
            Else
                myReport = New crptDirectiveStatusIssuedReport
            End If
        ElseIf cmbFormat.SelectedIndex = 1 Then
            If (AppSettings("ClientCode") IsNot Nothing) AndAlso
               AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then ' Then
                myReport = New crptDirectiveStatusIssuedReportAPFTFormat2 'Format added by Saylee for APFT18092018 on 18-Sep-2018
            ElseIf AppSettings("ClientCode") = "BAMS" Then
                myReport = New crptDirectiveStatusIssuedReportBAMSFormat2 'Format added by Prashant on 11-Feb-2022 AS Details Colum was not showing information. As it was showing in case of APFT client format but not for BASM client so…. 
            End If
        End If

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        'added by shital on 18-Dec-2019
        If (Bymail = True) Then
            Dim StrMailBody As String = ""
            StrMailBody = MailBody(Obj)

            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "DirectiveIssuedRegister", "", StrMailBody, ,
                                      Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, MailBody:="", Remark:=Session("SendMailRemark"),
                                      ReportGeneratedBy:=Session("ReportGenratedBy"),
                SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "DirectiveIssuedRegister", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

        If Not (Bymail) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If

        MarkLog(Util.Action.Print, "DirectiveIssuedRegister", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Function MailBody(ByVal mrptDirectiveStatusIssuedReport As rptDirectiveStatusIssuedReport) As String

        Dim str As String
        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri""> The Directive Issued Register is attached for your information  </font></P></br> ")


        '   str = str + ("<b>Tools Name :" + "</b><p>")
        str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
        str = str + ("<tr>" & "<td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Sr. No.</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Document No</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Issue/Rev No & Date</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Effective Date</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Subject</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Applicable to A/C Regn</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Remarks</b>" & "</font>" & "</td></tr>")

        For i As Integer = 0 To mrptDirectiveStatusIssuedReport.Count - 1
            str = str + ("<TR>")

            str = str + ("<TD WIDTH=20px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (i + 1).ToString + "."
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=200px >")
            str = str + ("<font face=""Calibri"">")
            str = str + DirectCast(mrptDirectiveStatusIssuedReport(i), Flypal.rptDirectiveStatusIssuedReport.rptDirectiveStatusIssuedReportInfo).DirectiveNo
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=200px >")
            str = str + ("<font face=""Calibri"">")
            str = str + DirectCast(mrptDirectiveStatusIssuedReport(i), Flypal.rptDirectiveStatusIssuedReport.rptDirectiveStatusIssuedReportInfo).IssueDateFormatted
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + DirectCast(mrptDirectiveStatusIssuedReport(i), Flypal.rptDirectiveStatusIssuedReport.rptDirectiveStatusIssuedReportInfo).StatusAsOnDateFormatted
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + DirectCast(mrptDirectiveStatusIssuedReport(i), Flypal.rptDirectiveStatusIssuedReport.rptDirectiveStatusIssuedReportInfo).Description
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + DirectCast(mrptDirectiveStatusIssuedReport(i), Flypal.rptDirectiveStatusIssuedReport.rptDirectiveStatusIssuedReportInfo).RegNo
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + DirectCast(mrptDirectiveStatusIssuedReport(i), Flypal.rptDirectiveStatusIssuedReport.rptDirectiveStatusIssuedReportInfo).Remarks
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("</TR>")
        Next

        str = str + ("</TABLE>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
        str = str + ("</body></html>")

        Return str
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Commented By Utkarsh On 19-Apr-2011

        'mMachineList = MachineList.GetMachineListMonitoringStatus(Today.Date.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(SELECT)")
        'Session("mMachineList") = mMachineList
        '*************************************
        'Added By Utkarsh On 19-Apr-2011
        'mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)")
        'cmbAircraft.DataSource = mMachineNameValueList
        'Session("mMachineNameValueList") = mMachineNameValueList
        ''**************************************
        'cmbAircraft.DataBind()
        mModTypeList = ModTypeList.GetModelTypeList(False, "")
        ' cmbModificationType.DataSource = mModTypeList
        ListDirectiveType.DataSource = mModTypeList
        Session("mModTypeList") = mModTypeList
        ' cmbModificationType.DataBind()
        ListDirectiveType.DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAdType" Then
            If ListDirectiveType.SelectedIndex = -1 Then
                custValidator.ErrorMessage = "Please select the Directive"
                e.IsValid = False
            ElseIf ListDirectiveSubType.SelectedIndex = -1 Then
                custValidator.ErrorMessage = "Please select the Directive Type"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptDirectiveStatusIssuedReport_Ajax.aspx"
            DataFieldBind()
            setFocus(cmbAircraft)
            ResetValues()
            If (AppSettings("ClientCode") IsNot Nothing) Then
                If AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then 'Added By Saylee On 1-Oct-2018 
                    txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Continuing Airworthiness Manager: __________________ Date: _____________"
                    PlaceHolder4.Visible = True  'Added by Shital on 23-Dec-2020
                ElseIf AppSettings("ClientCode") = "GLD" Then
                    PlaceHolder4.Visible = False  ''
                    txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout." + vbCrLf + "CAM Sign & Stamp: __________________ CAM Name: __________________ Date: _____________"
                Else
                    PlaceHolder4.Visible = False  'Added by Shital on 23-Dec-2020
                End If
            Else
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Planning Manager: __________________ License No.: __________ Date: _____________"

            End If
        End If
        If (AppSettings("ClientCode") = "IND") Then
            cmbSortBy.Items.Add(New ListItem("Code", "2"))
            cmbSortBy.DataBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub

    'Added by Shital on 18-Dec-2019
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        Session("UserEmailID") = mModuleList.Item("DirectiveIssuedRegister").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("DirectiveIssuedRegister").SendCCMailID
        Session("SmtpHost") = mModuleList.Item("DirectiveIssuedRegister").SmtpHost
        Session("SmtpPort") = mModuleList.Item("DirectiveIssuedRegister").SmtpPort
        Session("SmtpUser") = mModuleList.Item("DirectiveIssuedRegister").SmtpUser
        Session("SmtpPassword") = mModuleList.Item("DirectiveIssuedRegister").SmtpPassword

        If Session("UserEmailID") = "" Then
            Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        End If
        '--------------------------
        Dim Str As String
        If IsValid Then
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            Exit Sub
        End If
    End Sub

    Private Sub hdnimgLogBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgLogBtnSendMail.Click
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

    '---

    Private Sub ListDirectiveType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListDirectiveType.SelectedIndexChanged
        Dim DirectveTypeIDs As New StringBuilder
        For i As Integer = 0 To ListDirectiveType.Items.Count - 1
            If ListDirectiveType.Items(i).Selected Then
                If DirectveTypeIDs.ToString = "" Then
                    DirectveTypeIDs.Append("<ModTypeID>")
                End If
                DirectveTypeIDs.Append("<id>")
                DirectveTypeIDs.Append(ListDirectiveType.Items(i).Value)
                DirectveTypeIDs.Append("</id>")
            End If
        Next
        If DirectveTypeIDs.ToString <> "" Then
            DirectveTypeIDs.Append("</ModTypeID>")
        End If
        If DirectveTypeIDs.ToString = "" Then
            ListDirectiveSubType.Enabled = False
            ListDirectiveSubType.ClearSelection()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableDirectiveSubType", "disableDirectiveSubType();", True)
        Else
            ListDirectiveSubType.Enabled = True
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList(ModelMonitorModTypeIDs:=DirectveTypeIDs.ToString)
            ListDirectiveSubType.DataSource = mModificationTypeList
            ListDirectiveSubType.DataBind()
            For Each Item As ListItem In ListDirectiveSubType.Items
                Item.Selected = True
            Next
        End If

        upnlDirectiveSubType.Update()
    End Sub
#End Region

#Region "Service Methods"
    'Service method to fetch Aircraft list
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetAircraftList(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim machineList As List(Of CascadingDropDownNameValue) = New List(Of CascadingDropDownNameValue)()
        Dim mMachineNameValueList As MachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        machineList = (From c In mMachineNameValueList
                       Select New CascadingDropDownNameValue(c.RegNo, c.ID.ToString())).ToList
        Return machineList.ToArray
    End Function
    'Service method to fetch Assembly list
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetAssemblyList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As AjaxControlToolkit.CascadingDropDownNameValue()

        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim machineid As Guid

        If (Not kv.ContainsKey("Machine") Or Not Guid.TryParse(kv("Machine"), machineid)) Then
            Return Nothing
        End If

        If machineid.Equals(Guid.Empty) Then
            Return Nothing
        End If

        Dim fromdate As String = IIf(String.IsNullOrEmpty(contextKey), Now.Date.ToString, contextKey)
        Dim asmblylist As List(Of CascadingDropDownNameValue) = New List(Of CascadingDropDownNameValue)()
        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, machineid.ToString, fromdate, "(SELECT)", True)

        HttpContext.Current.Session("mAssemblylist") = mAssemblyList
        asmblylist = (From c In mAssemblyList
                      Select New CascadingDropDownNameValue(c.ModelSerialNoPostion, c.ID.ToString())).ToList
        Return asmblylist.ToArray
    End Function

#End Region

    'Added by Abhishek on 26-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            Dim OperatorName As String = ""
            SetValues()
            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            End If


            'Added by Shital on 09-Mar-2020
            For K As Integer = 0 To ListDirectiveType.Items.Count - 1
                If ListDirectiveType.Items.Item(K).Selected Then
                    ModTypeIds.Append(ListDirectiveType.Items.Item(K).Value + ",")
                End If
            Next

            For K As Integer = 0 To ListDirectiveType.Items.Count - 1
                If ListDirectiveType.Items.Item(K).Selected Then
                    ModTypeNames.Append(ListDirectiveType.Items.Item(K).Text + ",")
                End If
            Next

            For P As Integer = 0 To ListDirectiveSubType.Items.Count - 1
                If ListDirectiveSubType.Items.Item(P).Selected Then
                    If ModelMonitorModTypeIds.ToString = "" Then
                        ModelMonitorModTypeIds.Append("<ModTypeID>")
                    End If
                    ModelMonitorModTypeIds.Append("<id>")

                    ModelMonitorModTypeIds.Append(ListDirectiveSubType.Items.Item(P).Value)
                    ModelMonitorModTypeIds.Append("</id>")
                End If
            Next
            If ModelMonitorModTypeIds.ToString <> "" Then
                ModelMonitorModTypeIds.Append("</ModTypeID>")
            End If
            '------------
            '' Obj = rptDirectiveStatusIssuedReport.GetrptDirectiveStatusIssuedReport(txtFromDate.Text, txtToDate.Text, MachineID, New Guid(ModelID), CInt(cmbModificationType.SelectedValue.ToString), AssemblyID)
            Obj = rptDirectiveStatusIssuedReport.GetrptDirectiveStatusIssuedReport(txtFromDate.Text, txtToDate.Text, MachineID, New Guid(ModelID), ModTypeIDs:=ModelMonitorModTypeIds.ToString, AssemblyID:=AssemblyID, SortBy:=cmbSortBy.SelectedItem.ToString, AscDes:=IIf(optAscending.Checked = True, "Asc", "desc"), Type:=cmbAdType.SelectedItem.ToString)

            If Obj.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub

                'Added By Utkarsh On 7-Jun-2011 For All07062011

            ElseIf Obj.Count > 0 Then

                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1161)

                '*******************************
            End If

            Dim mMachineList As MachineList
            Dim ObjMachine As MachineInfo
            Dim ObjAssemblyStatus As AssemblyStatusInfo
            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""

            Dim mAssemblyStatus As AssemblyStatus
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyList(New Guid(cmbAssembly.SelectedValue)).AssemblyStatusID)

            'If mAssemblyStatus.IsRemoved Then
            '    mMachineList = MachineList.GetMachineListMonitoringStatus(txtToDate.Text, cmbAircraft.SelectedValue, , , , , , , , , , True, True, mAssemblyList(New Guid(cmbAssembly.SelectedValue)).AssemblyStatusID.ToString, _
            '                                                           , , MonitoringModRequired:=False, _
            '                                                          IsAssemblyRemoved:=True, IsCompRemoved:=False, IsComplied:=True, _
            '                                                          IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=False, _
            '                                                          SkipIsForInventoryAircarft:=True) 'IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
            'Else
            '    mMachineList = MachineList.GetMachineListMonitoringStatus(txtToDate.Text, cmbAircraft.SelectedValue, , , , , , , , , , True, True, mAssemblyList(New Guid(cmbAssembly.SelectedValue)).AssemblyStatusID.ToString, _
            '                                                          , , MonitoringModRequired:=False, _
            '                                                         IsAssemblyRemoved:=False, IsCompRemoved:=False, IsComplied:=True, _
            '                                                         IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=False, _
            '                                                         SkipIsForInventoryAircarft:=True) 'IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
            'End If

            mMachineList = MachineList.GetMachineListMonitoringStatus(txtToDate.Text, cmbAircraft.SelectedValue, , , , , , mAssemblyStatus.Assembly.SerialNo, , mAssemblyStatus.ModelName, , False, True, ,
                                                                 , , MonitoringModRequired:=False,
                                                                IsAssemblyRemoved:=True, IsCompRemoved:=False, IsComplied:=True,
                                                                IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=False,
                                                                SkipIsForInventoryAircarft:=True)
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    LHLabel2 = ""
                    LHData2 = ""
                    For i As Integer = 0 To ObjAssemblyStatus.AssemblyStatusPeriodList.Count - 1
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(i).PeriodID <> 2 Then
                            LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + " "), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(i).PeriodName
                            LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + " "), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(i).PeriodID, "").AssemblyCurrentValue
                        End If
                    Next
                Next

            Next

            Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            mCompanyDetail.WebSite, ModTypeName + " Status Report", Aircraft, Assembly1, ModTypeName, txtFromDate.Text, txtToDate.Text, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:=txtBottomLine.Text.Trim, SearchStr7:=OperatorName, SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"), SearchStr12:=LHLabel2, SearchStr13:=LHData2)

            ds.Clear()
            da.Fill(ds, "ExcelrptDirectiveStatusIssuedReport", Obj)
            da.Fill(ds, "ReportData", Report)
            Dim columnToRemove1 As String() = {"ModelMonitorModID", "AssemblyMonitorModStatusID", "RegNo", "HourType", "DirectiveType", "FrequencyValue", "IsDone", "AssemblyCurrentValue", "AssemblyStatusPeriodID", "DoneOn", "DoneOnFormatted", "IssueDate", "ModelMonitorModTypeName", "IsApplicable", "SrNo", "ModelMonitorModTypeID", "MonitorTypeID"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "SearchStr6", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Aircraft"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Assembly"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Directive Type"
            End If


            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Date To"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr12") Then
                ds.Tables("ReportData").Columns("SearchStr12").ColumnName = "Assembly Period(s)"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr13") Then
                ds.Tables("ReportData").Columns("SearchStr13").ColumnName = "Assembly Current Value(s)"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Report Date"
            End If
            If ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns.Contains("SearchStr1") Then
                ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("SearchStr1").ColumnName = "Aircraft"
            End If
            If ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns.Contains("IssueDateFormatted") Then
                ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("IssueDateFormatted").ColumnName = "Issue Date"
            End If
            If ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns.Contains("DueOnValue") Then
                ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("DueOnValue").ColumnName = "Next Due"
            End If
            If ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns.Contains("LastCarriedOut") Then
                ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("LastCarriedOut").ColumnName = "Last Carried"
            End If
            If ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns.Contains("ComplianceRequirement") Then
                ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("ComplianceRequirement").ColumnName = "Method Of Compliance"
            End If

            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("DirectiveNo").SetOrdinal(0)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Reference").SetOrdinal(1)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Description").SetOrdinal(2)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Issue Date").SetOrdinal(3)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Applicabilty").SetOrdinal(4)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("StatusType").SetOrdinal(5)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Type").SetOrdinal(6)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Method Of Compliance").SetOrdinal(7)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Note").SetOrdinal(8)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Last Carried").SetOrdinal(9)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Next Due").SetOrdinal(10)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("RemainingValue").SetOrdinal(11)
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").Columns("Remarks").SetOrdinal(12)

            ds.Tables("ReportData").TableName = "Searching Criteria"
            ds.Tables("ExcelrptDirectiveStatusIssuedReport").TableName = "Excel Directive Issued Report"

            Dim dataview As DataView = ds.Tables("Excel Directive Issued Report").DefaultView
            dataview.Sort = "Code"

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("Searching Criteria"))
            '  dsNew.Merge(ds.Tables("ExcelrptDirectiveStatusIssuedReport"))
            dsNew.Merge(dataview.ToTable())


			Session("ExcelFileName") = "Directive Issued Register"
			Session("dsNew") = dsNew
			Session("DataTableToBeFormattedForExportToExcel") = "ExcelrptDirectiveStatusIssuedReport"
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
            MarkLog(Util.Action.Print, "DirectiveIssuedRegister", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        End If
    End Sub


End Class