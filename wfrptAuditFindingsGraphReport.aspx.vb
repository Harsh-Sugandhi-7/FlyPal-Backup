
'Created By     :   Saylee
'Dated          :   5-Feb-2010
'Modified By    :   Saylee 6-Apr-2010

Partial Class wfrptAuditFindingsGraphReport
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
    'Private mAuditExecutionList As AuditExecutionList
    Private mAuditExecutionAuditNoList As AuditExecutionAuditNoList
    Private mDepartmentList As AuditDepartmentList
    Private mFindingStatusList As FindingStatusList

    Dim DateIndex, FromDate, ToDate As String
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        'mAuditExecutionList = Session("mAuditExecutionList")
        mAuditExecutionAuditNoList = Session("mAuditExecutionAuditNoList")
        mDepartmentList = Session("mDepartmentList")
        mFindingStatusList = Session("mFindingStatusList")
    End Sub
    Private Sub SetSession()
        ' Session("mAuditExecutionList") = mAuditExecutionList
        Session("mAuditExecutionAuditNoList") = mAuditExecutionAuditNoList
        Session("mDepartmentList") = mDepartmentList
        Session("mFindingStatusList") = mFindingStatusList
    End Sub
    Public Sub RemoveSessions()
        'Session.Remove("mAuditExecutionList")
        Session.Remove("mAuditExecutionAuditNoList")
        Session.Remove("mDepartmentList")
        Session.Remove("mFindingStatusList")

    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub ControlVisibility()

    End Sub
    Public Sub SetReport()
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptAuditFindingsGraphReport As rptAuditFindingsGraphReport
        Dim mrptMonthwiseAuditFindingsGraphReport As rptMonthwiseAuditFindingsGraphReport
        ''Dim mrptAuditFindingsGraphReportList As rptAuditFindingsGraphReportList
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsrptAuditFindingsGraphReport As New dsrptAuditFindingsGraphReport


        myReport = New crptAuditFindingsGraphReport



        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String
        Dim SearchStr5 As String

        SearchStr1 = cmbYear.SelectedItem.Text


        If cmbAuditInfoList.SelectedIndex > 0 Then
            SearchStr3 = cmbAuditInfoList.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        If cmbDepartmentList.SelectedIndex > 0 Then
            SearchStr4 = cmbDepartmentList.SelectedItem.Text
        Else
            SearchStr4 = ""
        End If

        If cmbFindingStatus.SelectedIndex > 0 Then
            SearchStr5 = cmbFindingStatus.SelectedItem.Text
        Else
            SearchStr5 = ""
        End If


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Audit Findings Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"))

        ''   mrptAuditFindingsGraphReportList = rptAuditFindingsGraphReportList.GetrptAuditFindingsGraphReportList(cmbYear.SelectedValue, SearchStr3, cmbDepartmentList.SelectedValue.ToString, cmbFindingStatus.SelectedValue)
        mrptMonthwiseAuditFindingsGraphReport = rptMonthwiseAuditFindingsGraphReport.GetrptMonthwiseAuditFindingsGraphReport(cmbYear.SelectedValue, SearchStr3, cmbDepartmentList.SelectedValue.ToString, cmbFindingStatus.SelectedValue)

        mrptAuditFindingsGraphReport = rptAuditFindingsGraphReport.GetrptAuditFindingsGraphReport(cmbYear.SelectedValue, SearchStr3, cmbDepartmentList.SelectedValue.ToString, cmbFindingStatus.SelectedValue)

        If mrptAuditFindingsGraphReport.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfrptAuditFindingsGraphReport.aspx?"
            msg1.Show()
            Exit Sub
        End If

        da.Fill(dsrptAuditFindingsGraphReport, mrptMonthwiseAuditFindingsGraphReport)
        da.Fill(dsrptAuditFindingsGraphReport, mrptAuditFindingsGraphReport)
        da.Fill(dsrptAuditFindingsGraphReport, Report)

        myReport.SetDataSource(dsrptAuditFindingsGraphReport)
        Session("CrystalReport") = myReport

        SetFocus(cmbYear)
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
    End Sub
    Public Sub setValues()
        Dim mAuditNo As String = ""
        Dim mDepartment As String = ""
        Dim mFindingStatus As String = ""

        'FromDate = txtFromDate.Value.ToString
        'ToDate = txtToDate.Value.ToString
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText

        If cmbAuditInfoList.SelectedIndex > 0 Then
            mAuditNo = cmbAuditInfoList.SelectedItem.Text
            lblAuditNo1.Text = "Audit No : " & mAuditNo
        Else
            lblAuditNo1.Text = "Audit No : All"
        End If

        If cmbDepartmentList.SelectedIndex > 0 Then
            mDepartment = cmbDepartmentList.SelectedItem.Text
            lblDepartment1.Text = "Department : " & mDepartment
        Else
            mAuditNo = ""
            lblDepartment1.Text = "Department : All"
        End If

        If cmbFindingStatus.SelectedIndex > 0 Then
            mFindingStatus = cmbFindingStatus.SelectedItem.Text
            lblFindingStatus1.Text = "Finding Status : " & mFindingStatus
        Else
            lblFindingStatus1.Text = "Finding Status : All"
        End If

    End Sub

#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mAuditExecutionAuditNoList = AuditExecutionAuditNoList.GetAuditExecutionAuditNoList("(ALL)")  'AuditExecutionList.GetAuditExecutionList("(SELECT)")
        cmbAuditInfoList.DataSource = mAuditExecutionAuditNoList
        Session("mAuditExecutionAuditNoList") = mAuditExecutionAuditNoList

        mDepartmentList = AuditDepartmentList.GetAuditDepartmentList("(ALL)")
        cmbDepartmentList.DataSource = mDepartmentList
        Session("mDepartmentList") = mDepartmentList

        mFindingStatusList = FindingStatusList.GetFindingStatusList("(ALL)")
        cmbFindingStatus.DataSource = mFindingStatusList
        Session("mFindingStatusList") = mFindingStatusList

        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "cmbAuditInfoList" Then
            ''If cmbAuditInfoList.SelectedIndex = 0 Then
            ''    custValidator.ErrorMessage = "Please select the Audit No."
            ''    e.IsValid = False
            ''Else
            ''    e.IsValid = True
            ''End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        Dim i As Integer
        If cmbYear.Items.Count = 0 Then 'Or cmbYear.SelectedValue = "" Then
            For i = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If
        If Not IsPostBack Then
            DataFieldBind()
            SetFocus(cmbYear)
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
        lblAuditNo1.Visible = True
        lblDepartment1.Visible = True
        lblDateRangeFrom.Visible = True
        lblFindingStatus1.Visible = True
        setValues()
    End Sub
    ''Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    ''    cmbAuditInfoList.Visible = Not CType(sender, Boolean)
    ''    cmbDepartmentList.Visible = Not CType(sender, Boolean)
    ''    cmbFindingStatus.Visible = Not CType(sender, Boolean)
    ''End Sub
#End Region
End Class
