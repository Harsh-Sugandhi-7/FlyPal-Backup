Public Class wfManualSubscriptionDueList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mManualSubscriptionDueList As ManualSubscriptionDueList
    Public mCategoryListForManualSubscriptionDueList As CategoryNameValueList
#End Region

#Region "Methods"
    Private Sub GetSession()
        mCategoryListForManualSubscriptionDueList = Session("mCategoryListForManualSubscriptionDueList")
        mManualSubscriptionDueList = Session("mManualSubscriptionDueList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryListForManualSubscriptionDueList")
        Session.Remove("mManualSubscriptionDueList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok

            End Select
        End If

    End Sub
    Private Sub DataFieldBind()
        mCategoryListForManualSubscriptionDueList = CategoryNameValueList.GetCategoryNameValueList("(ALL)")
        Session("mCategoryListForManualSubscriptionDueList") = mCategoryListForManualSubscriptionDueList
        cmbCategory.DataSource = mCategoryListForManualSubscriptionDueList
        cmbCategory.DataBind()

        mManualSubscriptionDueList = ManualSubscriptionDueList.GetManualSubscriptionDueListInfo(txtManualName.Text.Trim, _
                                                                                                  cmbCategory.SelectedValue.ToString, _
                                                                                                  cmbRange.SelectedIndex, txtDate.Text.Trim)
        Session("mManualSubscriptionDueList") = mManualSubscriptionDueList
        dgManualRevList.DataSource = mManualSubscriptionDueList
        dgManualRevList.DataBind()

        lblManual.Text = "Manual Subscription Due Report"
        lblList.Text = "List of Manual Subscription Due as per criteria : " & mManualSubscriptionDueList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        'If mManualSubscriptionDueList.Count = 0 Then
        '    btnPrint.Enabled = False
        '    btnPrintTop.Enabled = False
        'Else
        '    btnPrint.Enabled = True
        '    btnPrintTop.Enabled = True
        'End If

        'If mManualSubscriptionDueList.Count > 20 Then
        '    btnPrintTop.Visible = False
        '    btnCloseTop.Visible = True
        'Else
        '    btnPrintTop.Visible = False
        '    btnCloseTop.Visible = False
        'End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            txtDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            cmbRange.SelectedIndex = 2
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        dgManualRevList.PageIndex = 0

        mManualSubscriptionDueList = ManualSubscriptionDueList.GetManualSubscriptionDueListInfo(txtManualName.Text.Trim,
                                                                                                cmbCategory.SelectedValue.ToString,
                                                                                                cmbRange.SelectedIndex, txtDate.Text.Trim)
        dgManualRevList.DataSource = mManualSubscriptionDueList
        dgManualRevList.DataBind()
        Session("mManualSubscriptionDueList") = mManualSubscriptionDueList
        lblList.Text = "List of Manual Subscription Due as per criteria : " & mManualSubscriptionDueList.Count & " Record(s) found."
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgManualRevList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgManualRevList.PageIndexChanging
        dgManualRevList.PageIndex = e.NewPageIndex
        dgManualRevList.DataSource = mManualSubscriptionDueList
        Session("mManualSubscriptionDueList") = mManualSubscriptionDueList
        dgManualRevList.DataBind()
    End Sub
#End Region

#Region " Report "

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New Flypal.CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsManualSubscription
        Dim Obj As ManualSubscriptionDueList
        Rpt = New crManualSubscriptionDueList
        mManualSubscriptionDueList = Session("mManualSubscriptionDueList")

        Dim ReportName As String
        ReportName = "Manual Subscription Due Report"

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, ReportName, "", txtManualName.Text.Trim, IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.ToString, ""), _
         txtDate.Text, cmbRange.SelectedItem.Text, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        dgManualRevList.Visible = True

        Obj = mManualSubscriptionDueList
        ds.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, Obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)

        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region



End Class