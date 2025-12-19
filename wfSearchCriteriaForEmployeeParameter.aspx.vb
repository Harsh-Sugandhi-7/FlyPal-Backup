Partial Class wfSearchCriteriaForEmployeeParameter
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
    Dim FromDate As String
    Dim ToDate As String
    Protected mEmployeeList As EmployeeList
    Dim mCityList As CityInvList
    Protected mSkillList As SkillList

#End Region

#Region " Business Method "
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetValues()
        Dim mCrew As String = ""
        Dim mParameter As String = ""

        If cmbCrewList.SelectedIndex > 0 Then

            mCrew = cmbCrewList.SelectedItem.Text
            lblCrewSelection.Text = "Employee : " & mCrew
        Else
            mCrew = ""
            lblCrewSelection.Text = "Employee : All"
        End If
        If cmbParameterList.SelectedIndex > 0 Then

            mParameter = cmbParameterList.SelectedItem.Text
            lblParameterSelection.Text = "Skill : " & mParameter
        Else
            mParameter = ""
            lblParameterSelection.Text = "Skill : All"
        End If
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rptobj As EmployeeSkillList
        Dim ds As New dsEmployeeSkill

        Dim mEmployeeID As New Guid(cmbCrewList.SelectedValue.ToString)
        Dim Label As String
        If txtValue.Text = "" Then
            Label = ""
        Else
            Label = lblValue.Text + " " + txtValue.Text
        End If

        Dim SearchStr1 As String = ""
        Dim SearchStr2 As String = ""
        Dim SearchStr3 As String = ""
        Dim SearchStr4 As String = ""
        Dim SearchStr5 As String = ""
        Dim mCompanyDetail As New CompanyDetail

        
        SearchStr3 = IIf(cmbCrewList.SelectedIndex > 0, cmbCrewList.SelectedItem.Text, "")
        SearchStr4 = IIf(cmbParameterList.SelectedIndex > 0, cmbParameterList.SelectedItem.Text, "")

        If txtValue.Text = "" Then
            SearchStr5 = ""
        Else
            SearchStr5 = txtValue.Text
        End If

        Dim mParameter As String
        If cmbParameterList.SelectedIndex > 0 Then
            mParameter = cmbParameterList.SelectedItem.Text
        Else
            mParameter = ""
        End If
        myReport = New crEmployeeParameter
        rptobj = EmployeeSkillList.GetEmployeeSkillList(mEmployeeID, "", mParameter, txtValue.Text)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Employee Skill List Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))



        If rptobj.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfSearchCriteriaForEmployeeParameter.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 27-Feb-2012
        da.Fill(ds, rptobj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        da = Nothing
        rptobj = Nothing
        ds = Nothing
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()

        cmbCrewList.DataSource = EmployeeList.GetEmployeeList("", "", "(All)")
        cmbCrewList.DataBind()

        cmbParameterList.DataSource = SkillList.GetSkillList("", "(All)")
        cmbParameterList.DataBind()
        'txtFromDate.Value = Today.Date
        'txtFromDate.Value = Today.Date
        DataBind()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            If cmbCrewList.Enabled = True Then
                SetFocus(cmbCrewList)
            End If
            DataFieldBind()
        End If
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetValues()
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        'Response.Redirect("Index.aspx")
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblSummary.Visible = True
        lblCrewSelection.Visible = True
        lblParameterSelection.Visible = True
        lblValueSelect.Visible = True
        lblValueSelect.Text = "Value : " + txtValue.Text
        SetValues()
    End Sub
    Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        cmbCrewList.Visible = Not CType(sender, Boolean)
        cmbParameterList.Visible = Not CType(sender, Boolean)
    End Sub
    Private Sub cmbCrewList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCrewList.SelectedIndexChanged
        SetFocus(cmbCrewList)
    End Sub
    Private Sub cmbParameterList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbParameterList.SelectedIndexChanged
        SetFocus(cmbParameterList)
    End Sub
#End Region

End Class
