Partial Class wfSearchCriteriaForEmployeeContactInfo
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblFromDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblToDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblParameter As System.Web.UI.WebControls.Label
    Protected WithEvents cmbParameterList As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblValue As System.Web.UI.WebControls.Label
    Protected WithEvents txtValue As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFromDateSelection As System.Web.UI.WebControls.Label
    Protected WithEvents lblToDateSelection As System.Web.UI.WebControls.Label
    Protected WithEvents lblParameterSelection As System.Web.UI.WebControls.Label
    Protected WithEvents cmbCrew As System.Web.UI.WebControls.DropDownList

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
    Dim FromDate As String = "1-1-1900"
    Dim ToDate As String = "1-1-2200"
    Public mId As Guid
    Protected mEmployeeList As EmployeeList
    Dim mCityList As CityInvList
#End Region

#Region " Business Method "
    Private Sub GetSession()
        ' mDefectList = CType(Session("mDefectList"), QualityControlMngmt.QCMReport.TestCaseStatusReport)
        mId = Session("mId")
        'mProject = QualityControlMngmt.QCM.Project.GetProject(mId)
    End Sub
    Private Sub SetSession()
        ' Session("mDefectList") = mDefectList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mDefectList")
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
    Private Sub SetValues()
        Dim mCrew As String = ""
        Dim Status As String = ""
        Dim Severity As String = ""
        Dim mCity As String = ""

        If cmbCrewList.SelectedIndex > 0 Then

            mCrew = cmbCrewList.SelectedItem.Text
            lblCrewSelection.Text = "Employee : " & mCrew
        Else
            mCrew = ""
            lblCrewSelection.Text = "Employee : All"
        End If

        'cmbCrewList.SelectedIndex = 0

        If cmbCityList.SelectedIndex > 0 Then

            mCity = cmbCityList.SelectedItem.Text
            lblCitySelection.Text = "City : " & mCity
        Else
            mCity = ""
            lblCitySelection.Text = "City : All"
        End If
        ' cmbCityList.SelectedIndex = 0
    End Sub
    Public Sub ControlVisibility()
    End Sub

    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        'Machine = ""
        'Password = ""
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rptobj As EmployeeContactInfoList
        Dim ds As New dsEmployeeContactInfo

        Dim mEmployeeID As New Guid(cmbCrewList.SelectedValue.ToString)

        Dim SearchStr1 As String = ""
        Dim SearchStr2 As String = ""
        Dim SearchStr3 As String = ""
        Dim SearchStr4 As String = ""
        Dim SearchStr5 As String = ""
        Dim mCompanyDetail As New CompanyDetail

        SearchStr1 = IIf(cmbCrewList.SelectedIndex > 0, cmbCrewList.SelectedItem.Text, "")
        SearchStr2 = IIf(cmbCityList.SelectedIndex > 0, cmbCityList.SelectedItem.Text, "")

        If txtRelation.Text = "" Then
            SearchStr3 = ""
        Else
            SearchStr3 = txtRelation.Text
        End If

        If txtName.Text = "" Then
            SearchStr4 = ""
        Else
            SearchStr4 = txtName.Text
        End If

        If rdbPortrait.Checked = True Then
            myReport = New crEmployeeContactInfoP
        Else
            myReport = New crEmployeeContactInfoL
        End If

        Dim mCity As String
        If cmbCityList.SelectedIndex = 0 Then
            mCity = ""
        Else
            mCity = cmbCityList.SelectedItem.Text
        End If

        rptobj = EmployeeContactInfoList.GetEmployeeContactInfoList(mEmployeeID, "", txtName.Text, txtRelation.Text, mCity, "")

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Employee Next To Kin Information List Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If rptobj.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfSearchCriteriaForEmployeeContactInfo.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 28-Feb-2012
        da.Fill(ds, rptobj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
        da = Nothing
        rptobj = Nothing
        ds = Nothing
    End Sub

#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbCrewList.DataSource = EmployeeList.GetEmployeeList("", "", "(All)")
        cmbCrewList.DataBind()

        cmbCityList.DataSource = CityInvList.GetCityList(0, "", "", True)
        cmbCityList.DataBind()

        'cmbStatusList.DataSource = QualityControlMngmt.QCM.StatusList.GetStatusList("<SELECT>")
        'cmbStatusList.DataBind()
        'txtFromDate.Value = Today.Date
        'txtFromDate.Value = Today.Date
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then

            If cmbCrewList.Enabled = True Then
                SetFocus(cmbCrewList)
            End If

            DataFieldBind()
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
    'Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not IsValid Then Exit Sub
    '    mDefectList = mDefectList.GetTestCaseStatusReport(mProject.ID, New Guid(cmbModule.SelectedValue.ToString), New Guid(cmbSubModule.SelectedValue.ToString), txtFromDate.FormattedText, txtToDate.FormattedText, cmbStatusList.SelectedValue)
    '    Session("mDefectList") = mDefectList
    '    dgTestCaseList.DataSource = mDefectList
    '    dgTestCaseList.DataBind()
    '    lblResult.Text = "List of Test cases as per criteria : " & mDefectList.Count & " Record(s) Found."
    'End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        '   Response.Redirect("Index.aspx")
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblSummary.Visible = True
        lblCrewSelection.Visible = True
        lblCitySelection.Visible = True
        lblContactNameValue.Visible = True
        lblContactRelationValue.Visible = True
        lblContactNameValue.Text = "Name : " + txtName.Text
        lblContactRelationValue.Text = "Relation : " + txtRelation.Text
        SetValues()
    End Sub
    Private Sub cmbCrewList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCrewList.SelectedIndexChanged
        lblCrewSelection.Text = "Crew : " + cmbCrewList.SelectedItem.Text
        SetFocus(cmbCrewList)
    End Sub
    Private Sub cmbCityList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCityList.SelectedIndexChanged
        lblCitySelection.Text = "City : " + cmbCityList.SelectedItem.Text
        SetFocus(cmbCityList)
    End Sub

#End Region

End Class
