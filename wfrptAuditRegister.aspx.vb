
'Created By     :   Prashant
'Dated          :   5-Feb-2010
'Modified By    :   Saylee 6-Apr-2010

Partial Class wfrptAuditRegister
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnFindNow As System.Web.UI.WebControls.Button
    Protected WithEvents lblResult As System.Web.UI.WebControls.Label
    Protected WithEvents btnAddTop As System.Web.UI.WebControls.Button
    Protected WithEvents btnCloseTop As System.Web.UI.WebControls.Button
    Protected WithEvents dgAuditSchedule As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmbDateRange As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbScheduleType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar
    Protected WithEvents cmbSearch As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblSearch As System.Web.UI.WebControls.Label

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
    Public mAuditExecutionAuditNoList As AuditExecutionAuditNoList
    Public mAuditTypeList As AuditTypeList
    Public mAuditorList As AuditorList
    Public mAuditStatusList As AuditStatusList
    Dim AuditNo, FromDate, ToDate, AuditType, LeadAuditor As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditExecutionAuditNoList = Session("mAuditExecutionAuditNoList")
        mAuditTypeList = Session("mAuditTypeList")
        mAuditorList = Session("mAuditorList")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        AuditNo = Session("AuditNo")
        AuditType = Session("AuditType")
        LeadAuditor = Session("LeadAuditor")
    End Sub
    Private Sub SetSession()
        Session("mAuditExecutionAuditNoList") = mAuditExecutionAuditNoList
        Session("mAuditTypeList") = mAuditTypeList
        Session("mAuditorList") = mAuditorList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditExecutionAuditNoList")
        Session.Remove("mAuditTypeList")
        Session.Remove("mAuditorList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptAuditRegister.aspx?" Then
            Session.Remove("mAuditSchedule")
            Session.Remove("mAuditExecutionAuditNoList")
            Session.Remove("mAuditTypeList")
            Session.Remove("mAuditorList")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("AuditNo")
            Session.Remove("AuditType")
            Session.Remove("LeadAuditor")
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = 0
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Session("sender") = ""
                            Response.Redirect("wfrptAuditRegister.aspx")
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfrptAuditRegister.aspx?"
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfrptAuditRegister.aspx?"
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfrptAuditRegister.aspx?"
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfrptAuditRegister.aspx")
                Case MsgBoxResult.OK And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfrptAuditRegister.aspx")
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfrptAuditRegister.aspx")
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfrptAuditRegister.aspx")
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        mAuditExecutionAuditNoList = AuditExecutionAuditNoList.GetAuditExecutionAuditNoList("(All)")
        cmbAuditNo.DataSource = mAuditExecutionAuditNoList
        mAuditExecutionAuditNoList = Session("mAuditExecutionAuditNoList")

        mAuditTypeList = AuditTypeList.GetAuditTypeList("(All)")
        cmbAuditType.DataSource = mAuditTypeList
        mAuditTypeList = Session("mAuditTypeList")

        mAuditorList = AuditorList.GetAuditorList("(All)")
        cmbLeadAuditor.DataSource = mAuditorList
        mAuditorList = Session("mAuditorList")

        mAuditStatusList = AuditStatusList.GetAuditStatusList("(All)")
        cmbAuditStatus.DataSource = mAuditStatusList
        Session("mAuditStatusList") = mAuditStatusList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptAuditRegister.aspx?"
            txtFromDate.Value = Today.Date.ToString
            txtToDate.Value = Today.Date.ToString
            DataFieldBind()
            SetFocus(cmbAuditNo)
        End If
        MessageBoxResult()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then Exit Sub
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptAuditRegister As rptAuditRegister
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mdsAuditRegister As New dsAuditRegister
        myReport = New crptAuditRegister
        If cmbAuditNo.SelectedIndex > 0 Then
            AuditNo = cmbAuditNo.SelectedValue
        Else
            AuditNo = ""
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Audit Register", New SmartDate(txtFromDate.Value.ToString).FormattedText, New SmartDate(txtToDate.Value.ToString).FormattedText, AuditNo, cmbLeadAuditor.SelectedItem.Text, cmbAuditType.SelectedItem.Text, AppSettings("Product Version"), AppSettings("SINote"), cmbAuditStatus.SelectedItem.Text, "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        mrptAuditRegister = rptAuditRegister.GetrptAuditRegister(txtFromDate.Value.ToString, txtToDate.Value.ToString, AuditNo, cmbLeadAuditor.SelectedValue.ToString, cmbAuditType.SelectedValue, cmbAuditStatus.SelectedValue)

        If mrptAuditRegister.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfrptAuditRegister.aspx?"
            msg1.Show()
            Exit Sub
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(mdsAuditRegister)
        '----------------------------------------------------------
        da.Fill(mdsAuditRegister, mrptAuditRegister)
        da.Fill(mdsAuditRegister, Report)
        da.Fill(mdsAuditRegister, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(mdsAuditRegister)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRangeFrom.Visible = True
        lblAuditNo1.Visible = True
        lblLeadAuditor1.Visible = True
        lblAuditType1.Visible = True
        lblAuditStatus1.Visible = True
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(txtFromDate.Value.ToString).FormattedText & " To " & New SmartDate(txtToDate.Value.ToString).FormattedText

        If cmbAuditNo.SelectedIndex > 0 Then
            lblAuditNo1.Text = "Audit No. : " & cmbAuditNo.SelectedItem.Text
        Else
            lblAuditNo1.Text = "Audit No. : All"
        End If

        If cmbLeadAuditor.SelectedIndex > 0 Then
            lblLeadAuditor1.Text = "Lead Auditor : " & cmbLeadAuditor.SelectedItem.Text
        Else
            lblLeadAuditor1.Text = "Lead Auditor : All"
        End If

        If cmbAuditType.SelectedIndex > 0 Then
            lblAuditType1.Text = "Audit Type : " & cmbAuditType.SelectedItem.Text
        Else
            lblAuditType1.Text = "Audit Type : All"
        End If

        If cmbAuditStatus.SelectedIndex > 0 Then
            lblAuditStatus1.Text = "Audit Status : " & cmbAuditStatus.SelectedItem.Text
        Else
            lblAuditStatus1.Text = "Audit Status : All"
        End If
    End Sub
    Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
        cmbAuditNo.Visible = Not CType(sender, Boolean)
        cmbAuditType.Visible = Not CType(sender, Boolean)
        cmbLeadAuditor.Visible = Not CType(sender, Boolean)
        cmbAuditStatus.Visible = Not CType(sender, Boolean)
    End Sub
    'Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
    '    cmbAuditNo.Visible = Not CType(sender, Boolean)
    '    cmbAuditType.Visible = Not CType(sender, Boolean)
    '    cmbLeadAuditor.Visible = Not CType(sender, Boolean)
    '    cmbAuditStatus.Visible = Not CType(sender, Boolean)
    'End Sub
#End Region
End Class
