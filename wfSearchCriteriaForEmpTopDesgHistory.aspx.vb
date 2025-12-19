
'Created by : Saylee
'Date       : 23-Feb-2010

Partial Class wfSearchCriteriaForEmpTopDesgHistory
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
    Protected mCrewList As EmployeeList
    Protected mEmployeeTopDesgList As EmployeeTopDesgList

#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mEmployeeTopDesgList = Session("mEmployeeTopDesgList")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeTopDesgList") = mEmployeeTopDesgList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mEmployeeTopDesgList")
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
        Dim obj As EmployeeTopDesgList
        Dim mSalaryHeadAllownceList As SalaryHeadAllownceList
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim dsCrewDesignation As New dsCrewDesignation


        myReport = New crEmployeeTopDesgList

        Dim SearchStr1 As String
        Dim EmployeeID As String


        If cmbCrewList.SelectedIndex > 0 Then
            SearchStr1 = cmbCrewList.SelectedItem.Text
            EmployeeID = cmbCrewList.SelectedValue.ToString
        Else
            SearchStr1 = "(All)"
            EmployeeID = "{00000000-0000-0000-0000-000000000000}"
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Employee Salary History", SearchStr1, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        obj = EmployeeTopDesgList.GetEmployeeTopDesgList(EmployeeID)
        mSalaryHeadAllownceList = SalaryHeadAllownceList.GetSalaryHeadAllownceList()

        If obj.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfSearchCriteriaForEmpTopDesgHistory.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(dsCrewDesignation) 'Added by Shweta on 28-Feb-2012
        da.Fill(dsCrewDesignation, obj)
        da.Fill(dsCrewDesignation, mrptImage)
        da.Fill(dsCrewDesignation, mSalaryHeadAllownceList)
        da.Fill(dsCrewDesignation, Report)
        myReport.SetDataSource(dsCrewDesignation)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
    Private Sub ResetValues()

    End Sub
    Public Sub setValues()
        Dim mCrew As String = ""

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
        cmbCrewList.DataSource = EmployeeList.GetEmployeeList(, , "(ALL)")
        cmbCrewList.DataBind()

        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        ''Dim custValidator As CustomValidator
        ''custValidator = CType(s, CustomValidator)

        ''If custValidator.ControlToValidate = "cmbCrewList" Then
        ''    If cmbCrewList.SelectedIndex = 0 Then
        ''        custValidator.ErrorMessage = "Please select the Employee"
        ''        e.IsValid = False
        ''    Else
        ''        e.IsValid = True
        ''    End If
        ''  End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            SetFocus(cmbCrewList)
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
        setValues()
    End Sub
#End Region
End Class
