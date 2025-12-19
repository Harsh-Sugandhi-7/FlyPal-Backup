Public Class wfSearchCriteriaForEmployeeDesgHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ' Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    ' Protected WithEvents txtFromDate As SIControls.SICalendar
    ' Protected WithEvents txtToDate As SIControls.SICalendar
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
    Protected mCrewList As EmployeeList
    Protected mEmployeeDesignationList As EmployeeDesignationList
    Dim var As String
    Dim DateIndex, FromDate, ToDate As String
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mEmployeeDesignationList = Session("mEmployeeDesignationList")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDesignationList") = mEmployeeDesignationList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mEmployeeDesignationList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Public Sub ControlVisibility()

    End Sub
    Public Sub SetReport()
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As EmployeeDesignationList
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim dsCrewDesignation As New dsCrewDesignation


        myReport = New crEmployeeDesgSalaryHistory

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String

        If cmbCrewList.SelectedIndex > 0 Then
            SearchStr1 = cmbCrewList.SelectedItem.Text
        Else
            SearchStr1 = ""
        End If

        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        SearchStr2 = New SmartDate(FromDate).FormattedText
        SearchStr3 = New SmartDate(ToDate).FormattedText

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Employee Designation-Salary History Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        obj = EmployeeDesignationList.GetEmployeeDesignationList(New Guid(cmbCrewList.SelectedValue.ToString), , txtFromDate.Text.ToString, txtToDate.Text.ToString)

        Dim mEmployeeDesgSalaryHeadAllownceList As EmployeeDesgSalaryHeadAllownceList = EmployeeDesgSalaryHeadAllownceList.GetEmployeeDesgSalaryHeadAllownceList(New Guid(cmbCrewList.SelectedValue.ToString), txtFromDate.Text.ToString, txtToDate.Text.ToString)

        If obj.Count <= 0 Then
          MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mEmployeeDesgSalaryHeadAllownceList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(dsCrewDesignation)
        da.Fill(dsCrewDesignation, mrptImage)
        da.Fill(dsCrewDesignation, obj)
        da.Fill(dsCrewDesignation, mEmployeeDesgSalaryHeadAllownceList)
        da.Fill(dsCrewDesignation, Report)
        myReport.SetDataSource(dsCrewDesignation)
        Session("CrystalReport") = myReport
        Dim Str As String

        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        ResetValues()
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
    End Sub
    Public Sub setValues()
        Dim mCrew As String = ""

        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText

        If cmbCrewList.SelectedIndex > 0 Then
            mCrew = cmbCrewList.SelectedItem.Text
            lblCrewName.Text = "Employee : " & mCrew
        Else
            mCrew = ""
            lblCrewName.Text = "Employee : All"

        End If
    End Sub

#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbCrewList.DataSource = EmployeeList.GetEmployeeList(, , "(SELECT)")
        cmbCrewList.DataBind()
       
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "cmbCrewList" Then
            If cmbCrewList.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Employee"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            setFocus(cmbCrewList)
        End If
        ControlVisibility()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            setValues()
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblCrewName.Visible = True
        lblDateRangeFrom.Visible = True
        setValues()
    End Sub
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    '    cmbCrewList.Visible = Not CType(sender, Boolean)
    'End Sub
#End Region

End Class