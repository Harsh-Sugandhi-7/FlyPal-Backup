Public Class wfrptEventLog_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public FromDate As String
    Public ToDate As String
    Public UserName As String = ""
    Public IPAddress As String = ""
    Public Machine As String = ""
    Public Password As String = ""
    Public ActionID As Short
    Public ModuleName As String
    Public ErrorTypeID As Short
    Public mID As Guid
    Dim Action As String = ""
    Public mActionList As ActionList
    Public mErrorTypeList As ErrorTypeList
    Public mModuleList As DistinctModuleNameListAutoComplete
    'Public mModuleList As ModuleList

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Dim mModuleList1 As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mActionList = CType(Session("mActionList"), ActionList)
        mErrorTypeList = CType(Session("mErrorTypeList"), ErrorTypeList)
        'mModuleList = CType(Session("mModuleList"), ModuleList)
        mModuleList1 = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mActionList") = mActionList
        Session("mErrorTypeList") = mErrorTypeList
        'Session("mModuleList") = mModuleList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mActionList")
        Session.Remove("mErrorTypeList")
        'Session.Remove("mModuleList")
    End Sub
    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            txtFromTime.Visible = True
            txtToTime.Visible = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            txtFromTime.Visible = True
            txtToTime.Visible = True
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
            txtFromTime.Visible = False
            txtToTime.Visible = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblUserName1.Visible = True
        ' lblErrorType1.Visible = True
        lblIPAddress1.Visible = True
        lblMachineName1.Visible = True
        'lblModule1.Visible = True
        lblAction1.Visible = True
        lblModuleName1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblUserName1.Visible = False
        ' lblErrorType1.Visible = False
        lblIPAddress1.Visible = False
        lblMachineName1.Visible = False
        'lblModule1.Visible = False
        lblAction1.Visible = False
        lblModuleName1.Visible = False
    End Sub
    Private Sub SetDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select

        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))

    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        ElseIf (txtFromTime.Text.ToString.Trim = "00:00" Or txtToTime.Text.ToString.Trim = "00:00") Then
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        Else
            FromDate = txtFromDate.Text.ToString + " " + txtFromTime.Text.ToString.Trim
            ToDate = txtToDate.Text.ToString + " " + txtToTime.Text.ToString.Trim
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If
        If txtUserName.Text <> "" Then
            UserName = Trim(txtUserName.Text)
            lblUserName1.Text = "User Name : " & UserName
        Else
            UserName = ""
            lblUserName1.Text = "User Name : All"
        End If
        If txtIPAddress.Text <> "" Then
            IPAddress = Trim(txtIPAddress.Text)
            lblIPAddress1.Text = "IP Address : " & IPAddress
        Else
            IPAddress = ""
            lblIPAddress1.Text = "IP Address : All"
        End If
        If txtMachineName.Text <> "" Then
            Machine = Trim(txtMachineName.Text)
            lblMachineName1.Text = "Machine : " & Machine
        Else
            Machine = ""
            lblMachineName1.Text = "Machine : All"
        End If
        If cmbAction.SelectedIndex > 0 Then
            ActionID = mActionList.Item(cmbAction.SelectedIndex).ID
            lblAction1.Text = "Action : " & cmbAction.SelectedItem.Text
        Else
            ActionID = 0
            lblAction1.Text = "Action : All"
        End If
        'If cmbErrorType.SelectedIndex > 0 Then
        '    ErrorTypeID = mErrorTypeList.Item(cmbErrorType.SelectedIndex).ID
        '    '  lblErrorType1.Text = "Error : " & cmbErrorType.SelectedItem.Text
        'Else
        '    ErrorTypeID = 0
        '    '  lblErrorType1.Text = "Error : All"
        'End If
        'If cmbModule.SelectedIndex > 0 Then
        '    ModuleName = mModuleList.Item(cmbModule.SelectedIndex).Description
        '    lblModule1.Text = "Module : " & ModuleName
        'Else
        '    ModuleName = ""
        '    lblModule1.Text = "Module : All"
        'End If

        lblModuleName1.Text = "Module Name : " & txtModuleName.Text 'Added By Shweta On 14-March-2013 For  ALL11032013 - 2

        mCompleteSearchingCriteria = lblDateRange.Text + ", " + lblUserName1.Text + ", " + lblIPAddress1.Text + ", " + lblMachineName1.Text + ", " + _
                                    lblAction1.Text + ", " + lblModuleName1.Text

        'Commented on 20-Sep-2016
        'upnlDisplaySearchCriteria.Update()

    End Sub

    Public Sub SetReport(Optional ByVal ByMail As Boolean = False) 'Parameter optional Added on 20-Sep-2016  
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        'Dim objSearch As rptSearchingCriteriaForSalesOrder
        ' Dim cRpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        'Dim objReg As rptSalesOrderRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsUserEventLog       'dsSalesOrder
        SetValues()

        Dim mEventLogList As EventLogList
        Dim mEventLogDetailList As EventLogDetailList
        Dim mLoginStatusList As LoginStatusList
        Dim mActionList As ActionList
        Dim mErrorTypeList As ErrorTypeList


        If chkLoginEntriesOnly.Checked = True Then
            myReport = New crptEventLog
        Else
            myReport = New crptEventLogDetail
        End If

        mEventLogList = EventLogList.GetEventLogList(Guid.Empty.ToString, UserName, Password, IPAddress, Machine, FromDate, ToDate, 1, _
                                                     chkByBTPLAdminUser.Checked, CType(ActionID, SI.UTILITY.EventLogDetailList.Action), _
                                                     txtModuleName.Text, IIf(txtFromTime.Text.Trim = "", "00:00", txtFromTime.Text.Trim), _
                                                     IIf(txtToTime.Text.Trim = "", "00:00", txtToTime.Text.Trim))
        'mEventLogDetailList = EventLogDetailList.GetEventLogDetailList(Guid.Empty.ToString, CType(ActionID, SI.UTILITY.EventLogDetailList.Action), ModuleName, CType(ErrorTypeID, SI.UTILITY.EventLogDetailList.ErrorType), Guid.Empty.ToString, UserName, Password, IPAddress, Machine, FromDate, ToDate, 1')'Commented By Shweta On 11-March-2013 For  ALL11032013 - 2
        mEventLogDetailList = EventLogDetailList.GetEventLogDetailList(Guid.Empty.ToString, CType(ActionID, SI.UTILITY.EventLogDetailList.Action), _
                                                                       txtModuleName.Text, CType(ErrorTypeID, SI.UTILITY.EventLogDetailList.ErrorType), _
                                                                       Guid.Empty.ToString, UserName, Password, IPAddress, Machine, FromDate, ToDate, 1, _
                                                                       chkByBTPLAdminUser.Checked, IIf(txtFromTime.Text.Trim = "", "00:00", txtFromTime.Text.Trim), _
                                                                       IIf(txtToTime.Text.Trim = "", "00:00", txtToTime.Text.Trim)) 'Added By Shweta On 11-March-2013 For  ALL11032013 - 2
        mLoginStatusList = LoginStatusList.GetLoginStatusList()
        mActionList = ActionList.GetActionList()
        mErrorTypeList = ErrorTypeList.GetErrorTypeList()

        'If case Added By Shital On 20-Sep-2016
        If ByMail = False Then
            If chkLoginEntriesOnly.Checked = True And mEventLogList.Count <= 0 Then
                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                '''''msg1.ReplacePage = "wfrptEventLog.aspx?Backpage="
                '''''msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf chkLoginEntriesOnly.Checked = False And mEventLogDetailList.Count <= 0 Then
                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                '''''msg1.ReplacePage = "wfrptEventLog.aspx?Backpage="
                '''''msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        'Added By Shital On 20-Sep-2016
        If (ByMail = True And mEventLogList.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Flypal Status Report", "User Login Detail  Report", "There is no record for this search criteria.", _
                "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
               SmtpHost:=mModuleList1.Item("EventLog").SmtpHost, SmtpPort:=mModuleList1.Item("EventLog").SmtpPort, SmtpUser:=mModuleList1.Item("EventLog").SmtpUser, SmtpPassword:=mModuleList1.Item("EventLog").SmtpPassword)

            Exit Sub
        End If

        ds.Clear()
        Dim mSearchingCritera As EventReportData

        'If chkShowLogDetail.Checked = True And chkLoginEntriesOnly.Checked = False Then
        '    mSearchingCritera = New EventReportData("", "", "", "", "", "", "", "User Login Detail Report", New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, UserName, "", IPAddress, "", "", False)
        If chkLoginEntriesOnly.Checked = True Then
            mSearchingCritera = New EventReportData("", "", "", "", "", "", "", "User Login Detail Report", IIf(FromDate = "1-1-1900", "", New SmartDate(FromDate).FormattedText), IIf(ToDate = "1-1-2200", "", New SmartDate(ToDate).FormattedText), UserName, "", IPAddress, "", "", True)
        Else
            mSearchingCritera = New EventReportData("", "", "", "", "", "", "", "User Login Detail Report", New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, UserName, "", IPAddress, "", "", True)
        End If


        da.Fill(ds, mEventLogList)
        da.Fill(ds, mEventLogDetailList)
        da.Fill(ds, mActionList)
        da.Fill(ds, mErrorTypeList)
        da.Fill(ds, mLoginStatusList)
        da.Fill(ds, mSearchingCritera)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        'Added By Shital On 20-Sep-2016
        If (ByMail = True) Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "User Login Detail  Report", "User Login Detail  Report", " ", , _
                                      Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                      ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=mModuleList1.Item("EventLog").SmtpHost, SmtpPort:=mModuleList1.Item("EventLog").SmtpPort, SmtpUser:=mModuleList1.Item("EventLog").SmtpUser, SmtpPassword:=mModuleList1.Item("EventLog").SmtpPassword)

        Else
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        End If
        MarkLog(Util.Action.Print, "EventLog", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mActionList = ActionList.GetActionList("<ALL>")
        cmbAction.DataSource = mActionList
        mErrorTypeList = ErrorTypeList.GetErrorTypeList("<ALL>")
        'cmbErrorType.DataSource = mErrorTypeList
        'mModuleList = ModuleList.GetModuleList(, , "<ALL>")
        'cmbModule.DataSource = mModuleList
        Session("mActionList") = mActionList
        Session("mErrorTypeList") = mErrorTypeList
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        ''If (txtFromDate.Value.ToString) <= (txtToDate.Value.ToString) Then
        ''    e.IsValid = True
        ''Else
        ''    CustValidator.ErrorMessage = "Start Date must be less than End date."
        ''    e.IsValid = False
        ''End If

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                SetFocus(cmbDateRange)
            End If
            DataFieldBind()

            ControlVisibility(6)
            SetDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            'If (chkLoginEntriesOnly.Checked = False) Then
            '    txtModuleName.Enabled = True
            'End If
            txtModuleName.Enabled = IIf(chkLoginEntriesOnly.Checked, True, False)
            'Added By Shweta On 11-March-2013 For  ALL11032013 - 2
            If chkLoginEntriesOnly.Checked And chkLoginEntriesOnly.Enabled = True Then
                txtModuleName.Enabled = False
            Else
                txtModuleName.Enabled = True
            End If
            'End
        End If

        If HttpContext.Current.User.Identity.Name.ToUpper = "BTPLADMIN" Then
            lblStep6.Text = "Step V. Select Login Entry / Activity/ For BTPL Admin "
            chkByBTPLAdminUser.Visible = True
        Else
            lblStep6.Text = "Step V. Select Login Entry / Activity"
            chkByBTPLAdminUser.Visible = False
        End If

    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        SetDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()

        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then Exit Sub
        'If chkLoginEntriesOnly.Checked = False Then
        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select any of Login Entry / Activity", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfrptEventLog.aspx?Backpage="
        '    msg1.Show()
        '    Exit Sub
        ' Else
        SetReport(False)
        'End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub chkShowLogDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    pnlMachine.Visible = chkShowLogDetail.Checked
    '    If chkShowLogDetail.Checked Then
    '        lblStep8.Text = "Step VII. Display Report"
    '    Else
    '        lblStep8.Text = "Step VI. Display Report"
    '    End If
    'End Sub
    'Private Sub cmbErrorType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbErrorType.SelectedIndexChanged
    '    If cmbErrorType.SelectedIndex <> -1 Then
    '        Dim Info As ErrorTypeList.ErrorTypeInfo
    '        Info = mErrorTypeList.Item(cmbErrorType.SelectedIndex)
    '        ErrorTypeID = Info.ID
    '    End If
    'End Sub
    Private Sub cmbAction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAction.SelectedIndexChanged
        If cmbAction.SelectedIndex <> -1 Then
            Dim Info As ActionList.ActionInfo
            Info = mActionList.Item(cmbAction.SelectedIndex)
            ActionID = Info.ID
        End If
    End Sub

    Private Sub chkLoginEntriesOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLoginEntriesOnly.CheckedChanged
        'Added By Shweta On 11-March-2013 For  ALL11032013 - 2
        If chkLoginEntriesOnly.Checked And chkLoginEntriesOnly.Enabled = True Then
            txtModuleName.Enabled = False
            txtModuleName.Text = ""
        Else
            txtModuleName.Enabled = True
        End If
        'End
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    'Added by Shital on 20-Sep-2016
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList1.Item("EventLog").SendToMailID
        Session("UserCcEmailID") = mModuleList1.Item("EventLog").SendCCMailID
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
    Private Sub txtFromTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromTime.TextChanged
        If IsValidTime(txtFromTime.Text.ToString.Trim) = False Then
            txtFromTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim FromDateTime As String = txtFromDate.Text.ToString + " " + txtFromTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(txtFromDate.Text), New SmartDate(FromDateTime).Date) <> 0 Then
                'mnWO.WOPlanedDate = DateTime

            End If
        End If
    End Sub
    Private Sub txtToTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtToTime.TextChanged
        If IsValidTime(txtToTime.Text.ToString.Trim) = False Then
            txtToTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim ToDateTime As String = txtToDate.Text.ToString + " " + txtToTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(txtToDate.Text), New SmartDate(ToDateTime).Date) <> 0 Then
                'mnWO.WOPlanedDate = DateTime
            End If
        End If
    End Sub
    Private Function IsValidTime(ByVal TimeValue As String) As Boolean
        Dim TimeRegulerExpression As String = ""
        If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
            TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
        Else
            TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
        End If
        If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

End Class