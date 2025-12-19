Public Class wfrptAuditStatusRegisterReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mrptAuditStatusRegisterReport As rptAuditStatusRegisterReport
    Public mAuditTypeList As AuditTypeList
    Public mAuditOnList As AuditOnList
    Dim mSearchingCriteria As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mrptAuditStatusRegisterReport = CType(Session("mrptAuditStatusRegisterReport"), rptAuditStatusRegisterReport)
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()

        mAuditTypeList = AuditTypeList.GetAuditTypeList("(All)")
        cmbAuditType.DataSource = mAuditTypeList
        cmbAuditType.DataBind()

        mAuditOnList = AuditOnList.GetAuditOnList("(All)")
        cmbAuditOnList.DataSource = mAuditOnList
        cmbAuditOnList.DataBind()

        txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-2)).ToString(AppSettings("DateFormat"))
        txtFromDate.DataBind()

        txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
        txtToDate.DataBind()

    End Sub
    Public Sub GridBind(Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/4400", Optional ByVal AuditNo As String = "", Optional ByVal AuditTypeID As Integer = 0, Optional ByVal DepartmentID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal AuditOnID As Integer = 0, Optional ByVal SearchText As String = "")
        mrptAuditStatusRegisterReport = rptAuditStatusRegisterReport.GetrptAuditStatusRegisterReport(FromDate, ToDate, AuditNo, AuditTypeID, DepartmentID, AuditOnID, SearchText, CmbStatus.SelectedValue)
        dgAuditStatusRegister.DataSource = mrptAuditStatusRegisterReport
        lblResult.Text = mrptAuditStatusRegisterReport.Count & " Record(s) Found"
        dgAuditStatusRegister.DataBind()
        lblResult.DataBind()
        Session("mrptAuditStatusRegisterReport") = mrptAuditStatusRegisterReport
        upnlGridView.Update()
        'btnCloseBottom.Visible = (dgAuditStatusRegister.Rows.Count >= 25)
        'btnDisplayBottom.Visible = (dgAuditStatusRegister.Rows.Count >= 25)
        'btnDisplayTop.Enabled = (dgAuditStatusRegister.Rows.Count > 0)
        'btnDisplayBottom.Enabled = (dgAuditStatusRegister.Rows.Count > 0)
        upnlBottomButtons.Update()
        upnlTopButtons.Update()
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsAuditStatusRegisterReport
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim SearchStr6 As String = ""
        myReport = New crptAuditStatusRegisterReport
        mrptAuditStatusRegisterReport = Session("mrptAuditStatusRegisterReport")

        If cmbAuditOnList.SelectedIndex > 0 And txtAuditOnText.Text.Trim <> "" Then
            SearchStr6 = cmbAuditOnList.SelectedItem.Text + " (" + txtAuditOnText.Text.Trim + ")"
        ElseIf cmbAuditOnList.SelectedIndex > 0 Then
            SearchStr6 = cmbAuditOnList.SelectedItem.Text
        Else
            SearchStr6 = ""
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Audit Status Report", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedItem.Text, ""), "", AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:=SearchStr6, Searchstr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptAuditStatusRegisterReport)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        MyReport.SetDataSource(ds)
        Session("CrystalReport") = MyReport
        mSearchingCriteria = txtFromDate.Text + ", " + txtToDate.Text + ", " + txtAuditNo.Text.Trim + ", " + cmbAuditType.SelectedItem.Text + ", " + cmbAuditOnList.SelectedItem.Text + ", " + SearchStr6
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "AuditRegister", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptAuditStatusRegisterReport")
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, cmbAuditType.SelectedValue, Guid.Empty.ToString, CInt(cmbAuditOnList.SelectedValue), txtAuditOnText.Text.Trim)
        End If
    End Sub
    Private Sub txtFromDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtFromDate.TextChanged
        GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedValue, 0), Guid.Empty.ToString, IIf(cmbAuditOnList.SelectedIndex > 0, CInt(cmbAuditOnList.SelectedValue), 0), txtAuditOnText.Text.Trim)
    End Sub
    Private Sub txtToDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtToDate.TextChanged
        GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedValue, 0), Guid.Empty.ToString, IIf(cmbAuditOnList.SelectedIndex > 0, CInt(cmbAuditOnList.SelectedValue), 0), txtAuditOnText.Text.Trim)
    End Sub
    Private Sub txtAuditNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtAuditNo.TextChanged
        GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedValue, 0), Guid.Empty.ToString, IIf(cmbAuditOnList.SelectedIndex > 0, CInt(cmbAuditOnList.SelectedValue), 0), txtAuditOnText.Text.Trim)
    End Sub
    Private Sub cmbAuditOnList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAuditOnList.SelectedIndexChanged
        txtAuditOnText.Text = ""
        If cmbAuditOnList.SelectedIndex > 0 Then
            txtAuditOnText.Visible = True
        Else
            txtAuditOnText.Visible = False
        End If
        GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedValue, 0), Guid.Empty.ToString, IIf(cmbAuditOnList.SelectedIndex > 0, CInt(cmbAuditOnList.SelectedValue), 0), txtAuditOnText.Text.Trim)
    End Sub
    Private Sub txtAuditOnText_TextChanged(sender As Object, e As System.EventArgs) Handles txtAuditOnText.TextChanged
        GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedValue, 0), Guid.Empty.ToString, IIf(cmbAuditOnList.SelectedIndex > 0, CInt(cmbAuditOnList.SelectedValue), 0), txtAuditOnText.Text.Trim)
    End Sub
    Private Sub cmbAuditType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAuditType.SelectedIndexChanged
        GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedValue, 0), Guid.Empty.ToString, IIf(cmbAuditOnList.SelectedIndex > 0, CInt(cmbAuditOnList.SelectedValue), 0), txtAuditOnText.Text.Trim)
    End Sub
    Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles CmbStatus.SelectedIndexChanged
        GridBind(txtFromDate.Text, txtToDate.Text, txtAuditNo.Text.Trim, IIf(cmbAuditType.SelectedIndex > 0, cmbAuditType.SelectedValue, 0), Guid.Empty.ToString, IIf(cmbAuditOnList.SelectedIndex > 0, CInt(cmbAuditOnList.SelectedValue), 0), txtAuditOnText.Text.Trim)
    End Sub
    Private Sub btnDisplayTop_Click(sender As Object, e As System.EventArgs) Handles btnDisplayTop.Click, btnDisplayBottom.Click
        SetReport()
    End Sub
    Private Sub dgAuditStatusRegister_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAuditStatusRegister.PageIndexChanging
        dgAuditStatusRegister.PageIndex = e.NewPageIndex
        dgAuditStatusRegister.DataSource = mrptAuditStatusRegisterReport
        dgAuditStatusRegister.DataBind()
        Session("mrptAuditStatusRegisterReport") = mrptAuditStatusRegisterReport
    End Sub
    Private Sub dgAuditStatusRegister_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgAuditStatusRegister.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If e.Row.Cells(9).Text = "Open" Then
                e.Row.Cells(9).BackColor = Color.FromArgb(128, 191, 255)    'Status Blue
            ElseIf e.Row.Cells(9).Text = "Close" Then
                e.Row.Cells(9).BackColor = Color.FromArgb(34, 177, 76)      'Status Light Green
            ElseIf e.Row.Cells(9).Text = "Schedule" Then
                e.Row.Cells(9).BackColor = Color.FromArgb(255, 242, 0)      'Status Yellow
            End If
        End If
    End Sub
    Private Sub dgAuditStatusRegister_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditStatusRegister.Sorting
        mrptAuditStatusRegisterReport.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mrptAuditStatusRegisterReport") = mrptAuditStatusRegisterReport
        dgAuditStatusRegister.DataSource = mrptAuditStatusRegisterReport
        dgAuditStatusRegister.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub btnCloseBottom_Click(sender As Object, e As System.EventArgs) Handles btnCloseBottom.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

 
End Class