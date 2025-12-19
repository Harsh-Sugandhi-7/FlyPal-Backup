Public Class wfrptSnagATACWiseGraphReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList ' tmpMachineList
    Public mATAList As ATAList
    Dim MachineID As String
    Dim RegNo As String
    Public mrptSnagATACWiseGraphReport As rptSnagATACWiseGraphReport
    Dim IsMajor As Boolean
    Dim IsInvestigationStatus As Boolean
    Dim IsMajorMinor As Integer
    Dim MajorMinor As String
    Dim IsSnagMEL As Integer
    Dim SnagMEL As String
    Public FromDate As String = "1-1-1900"
    Public ToDate As String = "1-1-2200"
    Dim Aircraft, ATAChapter, ATANomenclature, ATAChapterID As String
    Dim ATACode As Integer
    Dim string2 As String

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mrptSnagATACWiseGraphReport = CType(Session("mrptSnagATACWiseGraphReport"), rptSnagATACWiseGraphReport)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mATAList = CType(Session("mATAList"), ATAList)
    End Sub
    Private Sub SetSession()
        Session("mrptSnagATACWiseGraphReport") = mrptSnagATACWiseGraphReport
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mATAList") = mATAList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptSnagATACWiseGraphReport")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mATAList")
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub ControlVisibility(ByVal Index As Int32)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptSnagATACWiseGraphReport_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblAircraft1.Visible = True
        lblATAChapter1.Visible = True
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))    '31-Mar-2006
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
        If txtToDate.Text.ToString = "" Or txtFromDate.Text.ToString = "" Then                     'Date
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "From Date : " & New SmartDate(FromDate).FormattedText & " To Date : " & New SmartDate(ToDate).FormattedText '& " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If rbMajor.Checked = True Then
            IsMajorMinor = 1  'Major
            MajorMinor = 1    'To Show on report Major/Minor/All
        ElseIf rbAll.Checked = True Then
            IsMajorMinor = 2  '"All" means Bot Major and Minor
            MajorMinor = 2
        Else
            IsMajorMinor = 0  'Minor
            MajorMinor = 0
        End If
        If rbAllSnagMEL.Checked = True Then
            IsSnagMEL = 0  'ALL Snag AND MEL
            SnagMEL = 0
        ElseIf rbSnag.Checked = True Then
            IsSnagMEL = 1  'Snag
            SnagMEL = 1
        Else
            IsSnagMEL = 2  'MEL
            SnagMEL = 2
        End If
        ATAChapter = IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "All")
        ATAChapterID = cmbATAChapter.SelectedValue.ToString
        mATAList = CType(Session("mATAList"), ATAList)
        ATANomenclature = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature
        ATACode = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode
        lblATAChapter1.Text = "ATA : " & IIf(ATAChapter <> "", ATAChapter, "")
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "All")
        MachineID = cmbAircraft.SelectedValue.ToString
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        string2 = IIf(Aircraft <> "", Aircraft, "")
        'Added by Archana on 6-Aug-09
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        mCompleteSearchingCriteria = lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblATAChapter1.Text + ", " + "Type :" + _
                IIf(rbAll.Checked, "All", IIf(rbMajor.Checked, "Major", "Minor")) + ", " + "Part :" + IIf(rbAllSnagMEL.Checked, "All", IIf(rbSnag.Checked, IIf(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag"), IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL")))
    End Sub

    'Added by Archana on 6-Aug-09
    Private Sub PageInitialization()
        txtFromDate.Text = Format(Today.Date, AppSettings("DateFormat"))
        txtToDate.Text = Format(Today.Date, AppSettings("DateFormat"))
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsSnagATACWiseGraphReport
        Dim mCompanyDetail As New CompanyDetail

        SetValues()
        myReport = New crSnagATACWiseGraphReport

        mrptSnagATACWiseGraphReport = rptSnagATACWiseGraphReport.GetSnagATACWiseGraphReportList(FromDate, ToDate, MachineID, IsMajorMinor, IsSnagMEL, ATAChapterID)

        Dim mReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, _
        mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
           SnagMEL, New SmartDate(FromDate).FormattedText, Aircraft, MajorMinor, New SmartDate(ToDate).FormattedText, ATAChapter, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", AppSettings("MELSnagNomenclature").ToString, AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        If mrptSnagATACWiseGraphReport.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfrptSnagATACWiseGraphReport.aspx?Backpage="
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1012)
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------

        da.Fill(ds, mrptSnagATACWiseGraphReport)
        da.Fill(ds, mReportData)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "SangATAWise", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '1012
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'mMachineNameValueList = tmpMachineList.GetMachineList("", "", "", "", "", "(All)")
        mMachineNameValueList = MachineNameValueList.GetMachineList("", (Guid.Empty).ToString, 0, 0, "", "", "", True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        'cmbAircraft.DataBind()

        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        'cmbATAChapter.DataBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If Session("MiddleFrame") <> "wfrptSnagATACWiseGraphReport_Ajax.aspx" Then Session("MiddleFrame") = "wfrptSnagATACWiseGraphReport_Ajax.aspx"
            rbAll.Checked = True
            rbAllSnagMEL.Checked = True
            DataFieldBind()
            'Added by Archana on 6-Aug-09
            PageInitialization()
            If cmbAircraft.Enabled = True Then
                setFocus(cmbAircraft)
            End If
            'Commented by Archana on 6-Aug-09
            'If cmbDateRange.Enabled = True Then
            '    SetFocus(cmbDateRange)
            'End If
            'ControlVisibility(2)
            'setDatePeroid(2)
            'cmbDateRange.SelectedIndex = 2
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Commented by Archana on 6-Aug-09
        'Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        'ControlVisibility(index)
        'setDatePeroid(Index)
        'If cmbDateRange.Enabled = True Then
        '    SetFocus(cmbDateRange)
        'End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport()
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        'Response.End()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.Enabled = True Then
            SetFocus(cmbAircraft)
        End If
    End Sub
    'Added by Archana on 6-Aug-09
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    '    cmbAircraft.Visible = Not CType(sender, Boolean)
    '    txtToDate.Enabled = Not CType(sender, Boolean)
    'End Sub
    ''Added by Archana on 6-Aug-09
    'Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
    '    txtFromDate.Enabled = Not CType(sender, Boolean)
    'End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class