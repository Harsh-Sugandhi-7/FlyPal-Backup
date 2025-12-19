Partial Class wfSearchCriteriaForEmployeeDesignation
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txtFromDate As SIControls.SICalendar
    'Protected WithEvents txtToDate As SIControls.SICalendar

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
    'Dim FromDate As String = "1-1-1900"
    'Dim ToDate As String = "1-1-2200"
    Protected mEmployeeDesignationList As EmployeeDesignationList
    Protected mCrewList As EmployeeList
    Protected mDesignation As DesignationList
    Dim a As String
    Dim DateIndex, FromDate, ToDate As String
#End Region

#Region " Business Method "
    Private Sub GetSession()

        mEmployeeDesignationList = Session("mEmployeeDesignationList")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDesignationList") = mEmployeeDesignationList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mEmployeeDesignationList")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    'Private Sub PageInitialization()
    '    txtFromDate.Value = Today.Date
    '    txtToDate.Value = Today.Date
    'End Sub
    'Private Sub ResetValues()
    '    ToDate = Format(CDate(Today.Date).Year, "")
    'End Sub

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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select
    End Sub
    Private Sub SetValues()
        Dim mModule As String = ""
        Dim Desin As String = ""
        Dim mCrew As String = ""

        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If cmbCrewList.SelectedIndex > 0 Then

            mCrew = cmbCrewList.SelectedItem.Text
            lblCrewName.Text = "Employee : " & mCrew
        Else
            mCrew = ""
            lblCrewName.Text = "Employee : All"

        End If

        If cmbDesignation.SelectedIndex > 0 Then

            Desin = cmbDesignation.SelectedItem.Text

            lblDesignation1.Text = "Designation : " & Desin
        Else
            Desin = ""

            lblDesignation1.Text = "Designation : All"

        End If

    End Sub
    Public Sub ControlVisibility()
        If cmbDateRange.SelectedIndex = 0 Then
            txtFromDate.Visible = False
            txtToDate.Visible = False
            lblFromDate.Visible = False
            lblToDate.Visible = False
        Else
            txtFromDate.Visible = True
            txtToDate.Visible = True
            lblFromDate.Visible = True
            lblToDate.Visible = True
        End If
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"

    End Sub
    Private Sub ControlVisibility(ByVal DateIndex As Int32)
        lblFromDate.Visible = IIf(DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(DateIndex <> 0, True, False)

        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-1-1900")
                txtToDate.Text = CDate("1-1-2200")
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
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Public Sub SetReport()
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As EmployeeDesignationList
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim dsCrewDesignation As New dsCrewDesignation

        'Here crTestCaseStatusReport is used to show the Test Case Summary Report
        myReport = New crCrewDesignation

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
       
        
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            SearchStr1 = ""
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            SearchStr1 = New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If cmbCrewList.SelectedIndex > 0 Then
            SearchStr2 = "By Employee Name :" + " " + cmbCrewList.SelectedItem.Text
        Else
            SearchStr2 = ""
        End If

        If cmbDesignation.SelectedIndex > 0 Then
            SearchStr3 = "By Designation :" + " " + cmbDesignation.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        Dim DesNm As String
        If cmbDesignation.SelectedIndex > 0 Then
            DesNm = cmbDesignation.SelectedItem.Text
        Else
            DesNm = ""
        End If


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Employee Designation List", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        obj = EmployeeDesignationList.GetEmployeeDesignationList(New Guid(cmbCrewList.SelectedValue.ToString), "", txtFromDate.Text, txtToDate.Text, DesNm)
        If obj.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfSearchCriteriaForEmployeeDesignation.aspx?"
            'MSGBoxCtrl.Show("No Recocord Found!", "There is no record for this search criteria", "", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            'msg1.Show()
            Exit Sub
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(dsCrewDesignation) 'Added by Shweta on 27-Feb-2012
        da.Fill(dsCrewDesignation, obj)
        da.Fill(dsCrewDesignation, mrptImage)
        da.Fill(dsCrewDesignation, Report)
        myReport.SetDataSource(dsCrewDesignation)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbCrewList.DataSource = EmployeeList.GetEmployeeList(, , "(ALL)")
        cmbCrewList.DataBind()

        cmbDesignation.DataSource = DesignationList.GetDesignationList(, "(ALL)")
        cmbDesignation.DataBind()

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If cmbDateRange.Enabled = True Then
                SetFocus(cmbDateRange)
            End If
            DataFieldBind()
            setDatePeroid(0)
            'PageInitialization()
        End If
        ControlVisibility()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetValues()
            'SetParameterValues()
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("mDefectList") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRangeFrom.Visible = True
        lblCrewName.Visible = True
        lblDesignation1.Visible = True

        SetValues()
    End Sub
    Private Sub cmbDesignation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetFocus(cmbDesignation)
    End Sub
    Private Sub cmbCrewList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCrewList.SelectedIndexChanged
        SetFocus(cmbCrewList)
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDateRange.SelectedIndex >= 0, cmbDateRange.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setPeriod(DateIndex)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
#End Region

End Class
