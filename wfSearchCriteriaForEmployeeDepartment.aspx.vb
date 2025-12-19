Partial Class wfSearchCriteriaForEmployeeDepartment
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar

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
    Protected mEmployeeDepartmentInfoList As EmployeeDepartmentInfoList
    Protected mEmployeeList As EmployeeList
    Protected mEmployeeDepartmentList As EmployeeDepartmentList

    Dim DateIndex, FromDate, ToDate As String
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mEmployeeDepartmentInfoList = Session("mEmployeeDepartmentInfoList")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDepartmentInfoList") = mEmployeeDepartmentInfoList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mEmployeeDepartmentInfoList")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
            FromDate = txtFromDate.Value.ToString
            ToDate = txtToDate.Value.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If cmbEmployeeList.SelectedIndex > 0 Then

            mCrew = cmbEmployeeList.SelectedItem.Text
            lblEmployeeName.Text = "Employee : " & mCrew
        Else
            mCrew = ""
            lblEmployeeName.Text = "Employee : All"

        End If

        If cmbEmployeeDepartmentList.SelectedIndex > 0 Then

            Desin = cmbEmployeeDepartmentList.SelectedItem.Text

            lblDepartment1.Text = "Designation : " & Desin
        Else
            Desin = ""

            lblDepartment1.Text = "Designation : All"

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
                txtFromDate.Value = CDate("1-1-1900")
                txtToDate.Value = CDate("1-1-2200")
            Case 1 'Last 1 Week
                txtFromDate.Value = CDate(Today.AddDays(-6))
                txtToDate.Value = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Value = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Value = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Value = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Value = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Value = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Value = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Value = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Value = Today.AddDays(1).AddYears(-1)
                txtToDate.Value = Today.Date
            Case 5 'Current Financial Year
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Value = Today.Date
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Value = FromDate
                txtToDate.Value = ToDate
        End Select
    End Sub
    Public Sub SetReport()
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim dsCrewDesignation As New dsCrewDesignation

        myReport = New crCrewDesignation

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
       
        
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            SearchStr1 = ""
        Else
            FromDate = txtFromDate.Value.ToString
            ToDate = txtToDate.Value.ToString
            SearchStr1 = New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If cmbEmployeeList.SelectedIndex > 0 Then
            SearchStr2 = "By Employee Name :" + " " + cmbEmployeeList.SelectedItem.Text
        Else
            SearchStr2 = ""
        End If

        If cmbEmployeeDepartmentList.SelectedIndex > 0 Then
            SearchStr3 = "By Designation :" + " " + cmbEmployeeDepartmentList.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        Dim DesNm As String
        If cmbEmployeeDepartmentList.SelectedIndex > 0 Then
            DesNm = cmbEmployeeDepartmentList.SelectedItem.Text
        Else
            DesNm = ""
        End If


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Employee Designation List", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"))
        mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentInfoList(New Guid(cmbEmployeeList.SelectedValue.ToString), "", txtFromDate.Text, txtToDate.Text, DesNm)
        If mEmployeeDepartmentInfoList.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfSearchCriteriaForEmployeeDepartment.aspx?"
            msg1.Show()
            Exit Sub
        End If

        da.Fill(dsCrewDesignation, mEmployeeDepartmentInfoList)
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
        cmbEmployeeList.DataSource = EmployeeList.GetEmployeeList(, , "(SELECT)")
        cmbEmployeeList.DataBind()

        cmbEmployeeDepartmentList.DataSource = EmployeeDepartmentList.GetEmployeeDepartmentList("(SELECT)")
        cmbEmployeeDepartmentList.DataBind()

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
            setPeriod(0)
        End If
        ControlVisibility()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetValues()
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
        lblEmployeeName.Visible = True
        lblDepartment1.Visible = True

        SetValues()
    End Sub
    Private Sub cmbEmployeeDepartmentList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEmployeeDepartmentList.SelectedIndexChanged
        SetFocus(cmbEmployeeDepartmentList)
    End Sub
    Private Sub cmbEmployeeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEmployeeList.SelectedIndexChanged
        SetFocus(cmbEmployeeList)
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
