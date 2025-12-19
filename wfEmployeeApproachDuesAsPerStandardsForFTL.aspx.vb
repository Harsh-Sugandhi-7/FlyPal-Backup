Partial Class wfEmployeeApproachDuesAsPerStandardsForFTL
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
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
    Dim mEmployeeList As EmployeeList
    Dim mEmployeeApproachDuesAsPerStandardsForFTL As EmployeeApproachDuesAsPerStandardsForFTL
    Dim FromDate, ToDate, SearchStr1, SearchStr2, SearchStr3, SearchStr4 As String
#End Region

#Region " Helper Method "
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsEmployeeApproachDuesAsPerStandardsForFTL
        Dim AsPerCompanyStandard As Integer

        If rbCompanyStandard.Checked = True Then    'Company Standard
            myReport = New crEmployeeApproachDuesAsPerCompanyStandardsForFTL
            AsPerCompanyStandard = 1
            SearchStr3 = "Pilot Approach Dues As Per Company Standard For FTL"
        Else                                        'Govt. Standard
            myReport = New crEmployeeApproachDuesAsPerGovtStandardForFTL
            AsPerCompanyStandard = 2
            SearchStr3 = "Pilot Approach Dues As Per Govt. Standard For FTL"
        End If

        Dim mEmployeeID As New Guid(cmbEmployeeList.SelectedValue.ToString)

        If Not (txtToDate.IsDateValue) Then
            ToDate = ""
        Else
            ToDate = txtToDate.Value.ToString
        End If

        SearchStr1 = New SmartDate(txtToDate.Value.ToString).FormattedText
        SearchStr2 = IIf(cmbEmployeeList.SelectedIndex > 0, cmbEmployeeList.SelectedItem.Text, "")

        mEmployeeApproachDuesAsPerStandardsForFTL = EmployeeApproachDuesAsPerStandardsForFTL.GetEmployeeApproachDuesAsPerStandardsForFTLs(ToDate, mEmployeeID, AsPerCompanyStandard)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim ReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, SearchStr3, SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"))

        If mEmployeeApproachDuesAsPerStandardsForFTL.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfEmployeeApproachDuesAsPerStandardsForFTL.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, mEmployeeApproachDuesAsPerStandardsForFTL)
        da.Fill(ds, ReportData)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", str)
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "cmbDesignationList" Then
            If txtToDate.Text = "" Then
                CustValid.ErrorMessage = "Please select the Date "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)", , , True)
        cmbEmployeeList.DataSource = mEmployeeList
        Session("mEmployeeList") = mEmployeeList
        DataBind()
    End Sub

#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtToDate.ShowClearButton = False
        'Put user code to initialize the page here
        If Not IsPostBack Then
            If cmbEmployeeList.Enabled = True Then
                SetFocus(cmbEmployeeList)
            End If
            txtToDate.Value = Now.Date
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
        cmbEmployeeList.Visible = Not CType(sender, Boolean)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRangeTo.Visible = True
        lblEmployeeCriteria.Visible = True
        lblDateRangeTo.Text = "To : " & New SmartDate(txtToDate.Value.ToString).FormattedText
        lblEmployeeCriteria.Text = IIf(cmbEmployeeList.SelectedIndex > 0, "Pilot : " & cmbEmployeeList.SelectedItem.Text, "Pilot : (All)")

        If rbCompanyStandard.Checked Then
            lblServiceCriteria.Visible = True
            lblServiceCriteria.Text = "Standard: " & rbCompanyStandard.Text
        ElseIf rbGovtStandard.Checked Then
            lblServiceCriteria.Visible = True
            lblServiceCriteria.Text = "Standard: " & rbGovtStandard.Text
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("index.aspx")
    End Sub
    Private Sub cmbEmployeeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEmployeeList.SelectedIndexChanged
        SetFocus(cmbEmployeeList)
    End Sub
#End Region

End Class
