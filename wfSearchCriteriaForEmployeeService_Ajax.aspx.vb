Public Class wfSearchCriteriaForEmployeeService_Ajax
    Inherits System.Web.UI.Page


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txtFromDate As SIControls.SICalendar
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
    'Dim mEmployeeList As EmployeeList 'Commented By Utkash On 20-Apr-2011
    Public mEmployeeListForCombo As EmployeeListForCombo 'Added By Utkash On 20-Apr-2011
    Dim mServiceList As ServiceList
    Dim FromDate, ToDate, SearchStr1, SearchStr2, SearchStr3, SearchStr4 As String
#End Region

#Region " Helper Method "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim mEmployeeServiceList As EmployeeServiceList
        Dim ds As New dsEmployeeService
        myReport = New crEmployeeService

        Dim mEmployeeID As New Guid(cmbEmployeeList.SelectedValue.ToString)


        If Not IsDate(txtFromDate.Text.Trim) Then
            FromDate = ""
        Else
            FromDate = txtFromDate.Text.ToString
        End If
        If Not IsDate(txtToDate.Text.Trim) Then
            ToDate = ""
        Else
            ToDate = txtToDate.Text.ToString
        End If

        SearchStr1 = New SmartDate(txtFromDate.Text.ToString).FormattedText
        SearchStr2 = New SmartDate(txtToDate.Text.ToString).FormattedText
        SearchStr3 = IIf(cmbEmployeeList.SelectedIndex > 0, cmbEmployeeList.SelectedItem.Text, "")
        SearchStr4 = IIf(cmbServiceList.SelectedIndex > 0, cmbServiceList.SelectedItem.Text, "")

        Dim mService As String
        If cmbServiceList.SelectedIndex = 0 Then
            mService = ""
        Else
            mService = cmbServiceList.SelectedItem.Text
        End If

        mEmployeeServiceList = EmployeeServiceList.GetEmployeeServiceList(mEmployeeID, "", FromDate, ToDate, mService)
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim ReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Employee Service List Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))



        If mEmployeeServiceList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 28-Feb-2012
        da.Fill(ds, mEmployeeServiceList)
        da.Fill(ds, mrptImage) 'Added by Shweta on 28-Feb-2012
        da.Fill(ds, ReportData)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Commented By Utkash On 20-Apr-2011

        'mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)")
        'cmbEmployeeList.DataSource = mEmployeeList
        'Session("mEmployeeList") = mEmployeeList

        'Added By Utkash On 20-Apr-2011
        mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(All)")
        cmbEmployeeList.DataSource = mEmployeeListForCombo
        Session("mEmployeeListForCombo") = mEmployeeListForCombo


        txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        '******************************

        mServiceList = ServiceList.GetServiceList("", "(All)")
        cmbServiceList.DataSource = mServiceList
        Session("mServiceList") = mServiceList

        DataBind()
    End Sub
#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not IsPostBack Then
            txtFromDate.Text = Now.Date
            txtToDate.Text = Now.Date
            DataFieldBind()
            If cmbEmployeeList.Enabled = True Then
                SetFocus(cmbEmployeeList)
            End If
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    '    cmbEmployeeList.Visible = Not CType(sender, Boolean)
    '    cmbServiceList.Visible = Not CType(sender, Boolean)
    'End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblEmployeeCriteria.Visible = True
        lblServiceCriteria.Visible = True

        lblDateRangeFrom.Text = "From Date : " & New SmartDate(txtFromDate.Text.ToString).FormattedText
        lblDateRangeTo.Text = "To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText
        lblEmployeeCriteria.Text = IIf(cmbEmployeeList.SelectedIndex > 0, "Employee : " & cmbEmployeeList.SelectedItem.Text, "Employee : (All)")
        lblServiceCriteria.Text = IIf(cmbServiceList.SelectedIndex > 0, "Service : " & cmbServiceList.SelectedItem.Text, "Service : (All)")
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        '  Response.Redirect("index.aspx")
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

    Private Sub cmbEmployeeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEmployeeList.SelectedIndexChanged
        SetFocus(cmbEmployeeList)
    End Sub

    Private Sub cmbServiceList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbServiceList.SelectedIndexChanged
        SetFocus(cmbServiceList)
    End Sub

End Class