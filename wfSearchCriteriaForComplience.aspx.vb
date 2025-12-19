Partial Class wfSearchCriteriaForComplience
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

#Region "Variable Declaration"
    Dim mAssemblylist As AssemblyList
    Dim mModificationTypeList As ModelMonitorModTypeList
    Dim mMachineList As MachineList
    Dim AsonDate As String
    Dim AssemblyName As String
    Dim MachineName As String
    Dim AssemblyType As String
    Dim ModificationType As String
    Dim mModelList As ModelList
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mModelList = CType(Session("mModelList"), ModelList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
    End Sub
    Private Sub SetSession()
        Session("mModelList") = mModelList
        Session("mAssemblylist") = mAssemblylist
        Session("mModificationTypeList") = mModificationTypeList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForComplience.aspx" Then
            Session.Remove("mModelList")
            Session.Remove("mAssemblylist")
            Session.Remove("mModificationTypeList")
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ResetValues()
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        AssemblyType = ""
        AsonDate = ""
        MachineName = ""
        ModificationType = ""
    End Sub
    Public Sub SetValues()
        If cmbModelList.SelectedItem.Text = "<SELECT>" Then
            lblModel1.Text = "Model : " & ""
        Else
            lblModel1.Text = "Model : " & cmbModelList.SelectedItem.Text
        End If
        If cmbModificationType.SelectedItem.Text = "(All)" Then
            ModificationType = ""
            lblDirectiveType1.Text = "Directive type :" & " All"
        Else
            ModificationType = cmbModificationType.SelectedItem.Text
            lblDirectiveType1.Text = "Directive type : " & ModificationType
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfSearchCriteriaForComplience.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfSearchCriteriaForComplience.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region "DataFieldBind()"
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbModelList" Then
            If cmbModelList.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Model"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        mModelList = ModelList.GetModelList(, ModelList.IsSelectTagRequired.True)
        cmbModelList.DataSource = mModelList
        Session("mModelList") = mModelList
        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(All)")
        cmbModificationType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            If cmbModelList.Enabled = True Then
                SetFocus(cmbModelList)
            End If
            Session("MiddleFrame") = "wfSearchCriteriaForComplience.aspx"
            DataFieldBind()
        End If
        SetSession()
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblylist = Nothing
        mModificationTypeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtAsOnDate_CalendarVisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.cmbModelList.Visible = Not CType(sender, Boolean)
        Me.cmbModificationType.Visible = Not CType(sender, Boolean)
    End Sub
#End Region

#Region "Reports"

    Public Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As ReportComplienceList 'rptKitList
        Dim ds As New dsComplienceList 'dsKit
        myReport = New crListComplience 'crptInspectionList
        SetValues()
        rpt = ReportComplienceList.GetComplienceList(mModificationTypeList.Item(cmbModificationType.SelectedIndex, "").ID, New Guid(cmbModelList.SelectedValue.ToString))
        Dim mReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, _
        mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "Compliance List", _
         ModificationType, "", "", "", "", AppSettings("ProductVersion"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.)
        If rpt.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = ("wfSearchCriteriaForComplience.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            msg1.Show()
            Exit Sub
        Else
            
           RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 912)
        End If
        ds.Clear()
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, rpt)
        da.Fill(ds, mReportData)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
#End Region

End Class
