Partial Class wfSearchCriteriaForEmployeeDocument
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtFromDateExpiry As SIControls.SICalendar
    Protected WithEvents txtToDateExpiry As SIControls.SICalendar


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variable Declaration"
    'Dim mEmployeeList As EmployeeList    'Commented By Utkash On 20-Apr-2011
    Public mEmployeeListForCombo As EmployeeListForCombo 'Added By Utkash On 20-Apr-2011
    Dim mDocumentList As DocumentList
    Dim FromDate, ToDate, SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, SearchStr6, SearchStr7 As String
#End Region

#Region "Helper Method"
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetReport()
        Try
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim mCompanyDetail As New CompanyDetail
            Dim mEmployeeDocumentList As EmployeeDocumentList
            Dim ds As New dsEmployeeDocument

            If rbPortrait.Checked = True Then
                myReport = New crEmployeeDocumentP
            Else
                myReport = New crEmployeeDocumentL
            End If

            Dim mEmployeeID As New Guid(cmbEmployeeList.SelectedValue.ToString)

            If Not (txtFromDateExpiry.IsDateValue) Then
                FromDate = ""
            Else
                FromDate = txtFromDateExpiry.Value.ToString
            End If
            If Not (txtToDateExpiry.IsDateValue) Then
                ToDate = ""
            Else
                ToDate = txtToDateExpiry.Value.ToString
            End If

            SearchStr1 = IIf(cmbEmployeeList.SelectedIndex > 0, cmbEmployeeList.SelectedItem.Text, "")
            SearchStr2 = IIf(cmbDocumentList.SelectedIndex > 0, cmbDocumentList.SelectedItem.Text, "")
            SearchStr3 = txtDocumentNo.Text
            SearchStr4 = New SmartDate(txtFromDateExpiry.Value.ToString).FormattedText
            SearchStr5 = New SmartDate(txtToDateExpiry.Value.ToString).FormattedText
            SearchStr6 = txtValidity.Text
            SearchStr7 = txtWarningDays.Text
            Dim mDocument As String
            If cmbDocumentList.SelectedIndex = 0 Then
                mDocument = ""
            Else
                mDocument = cmbDocumentList.SelectedItem.Text
            End If
            Dim mValidity As Integer
            If IsNumeric(txtValidity.Text) Then mValidity = CInt(txtValidity.Text) Else mValidity = 0
            Dim mWarningDays As Integer
            If IsNumeric(txtWarningDays.Text) Then mWarningDays = CInt(txtWarningDays.Text) Else mWarningDays = 0

            mEmployeeDocumentList = EmployeeDocumentList.GetEmployeeDocumentList(mEmployeeID, "", mDocument, txtDocumentNo.Text, mValidity, FromDate, ToDate, mWarningDays, ConsiderWorkingEmpsOnly:=chkWorkingEmpOnly.Checked, Applicability:=CInt(cmbApplicability.SelectedValue))
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim ReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Employee Document List Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, SearchStr7, "", "", AppSettings("Logo"))



            If mEmployeeDocumentList.Count <= 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfSearchCriteriaForEmployeeDocument.aspx?Backpage="
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 28-Feb-2012
            da.Fill(ds, mEmployeeDocumentList)
            da.Fill(ds, mrptimage)
            da.Fill(ds, ReportData)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str As String
            Str = "<script language=Javascript>openTranDetail();</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub addAttributes()
        'WarningDays
        txtWarningDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtWarningDays').value,event)")
        'Validity
        txtValidity.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtValidity').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Commented By Utkash On 20-Apr-2011

        'mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)")
        'cmbEmployeeList.DataSource = mEmployeeList
        'Session("mEmployeeList") = mEmployeeList

        'Added By Utkash On 20-Apr-2011
        mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(All)", ExcludeNotWorkingEmployees:=chkWorkingEmpOnly.Checked)
        cmbEmployeeList.DataSource = mEmployeeListForCombo
        Session("mEmployeeListForCombo") = mEmployeeListForCombo
        '********************************
        mDocumentList = DocumentList.GetDocumentList("", "(All)")
        cmbDocumentList.DataSource = mDocumentList
        Session("mDocumentList") = mDocumentList

        DataBind()
    End Sub
    Private Sub chkWorkingEmpOnly_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkWorkingEmpOnly.CheckedChanged
        DataFieldBind()
    End Sub
#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        txtFromDateExpiry.ShowClearButton = False
        txtToDateExpiry.ShowClearButton = False

        'Put user code to initialize the page here
        If Not IsPostBack Then
            txtFromDateExpiry.Value = Now.Date
            txtToDateExpiry.Value = Now.Date
            DataFieldBind()
            If cmbEmployeeList.Enabled = True Then
                SetFocus(cmbEmployeeList)
            End If
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub txtFromDateExpiry_CalendarVisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromDateExpiry.CalendarVisibleChanged
        cmbEmployeeList.Visible = Not CType(sender, Boolean)
        cmbDocumentList.Visible = Not CType(sender, Boolean)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        'lblDateRangeFrom.Visible = True
        'lblDateRangeTo.Visible = True
        lblEmployeeCriteria.Visible = True
        lblDocumentCriteria.Visible = True
        lblDocumentNoCriteria.Visible = True
        lblValiditycriteria.Visible = True
        lblWarning.Visible = True
        lblApplicability.Visible = True

        lblEmployeeCriteria.Text = IIf(cmbEmployeeList.SelectedIndex > 0, "Employee : " & cmbEmployeeList.SelectedItem.Text, "Employee : (All)")
        lblDocumentCriteria.Text = IIf(cmbDocumentList.SelectedIndex > 0, "Document : " & cmbDocumentList.SelectedItem.Text, "Document : (All)")
        lblDocumentNoCriteria.Text = "Document No : " & txtDocumentNo.Text
        lblValiditycriteria.Text = "Validity : " & txtValidity.Text
        lblWarning.Text = "Warning Days : " & txtWarningDays.Text
        lblApplicability.Text = "Applicability : " & cmbApplicability.SelectedItem.Text
        'lblDateRangeFrom.Text = "From Date : " & New SmartDate(txtFromDateExpiry.Value.ToString).FormattedText
        'lblDateRangeTo.Text = "To Date : " & New SmartDate(txtToDateExpiry.Value.ToString).FormattedText
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ' Response.Redirect("index.aspx")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbEmployeeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEmployeeList.SelectedIndexChanged
        SetFocus(cmbEmployeeList)
    End Sub
    Private Sub cmbDocumentList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDocumentList.SelectedIndexChanged
        SetFocus(cmbDocumentList)
    End Sub
#End Region

    
End Class
