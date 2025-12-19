'AJAX Conversion by vikrant on 24-Jun-2015
Imports System.Linq
Public Class wfManualSubscription_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mManualSubscriptionList As ManualSubscriptionList
    Public mCategoryListForManualSubscription As CategoryNameValueList
    Dim EventLogID As Guid
#End Region

#Region "Methods"
    Private Sub GetSession()
        mCategoryListForManualSubscription = Session("mCategoryListForManualSubscription")
        mManualSubscriptionList = Session("mManualSubscriptionList")
    End Sub
    Private Sub addAttributes()
        txtWarningDays.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtWarningDays').value,event)")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryListForManualSubscription")
        Session.Remove("mManualSubscriptionList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                Case MsgBoxResult.OK

            End Select
        End If

    End Sub
    Private Sub DataFieldBind()
        mCategoryListForManualSubscription = CategoryNameValueList.GetCategoryNameValueList("(ALL)")
        Session("mCategoryListForManualSubscription") = mCategoryListForManualSubscription
        cmbCategory.DataSource = mCategoryListForManualSubscription
        cmbCategory.DataBind()

        'txtWarningDays.DataBind()

        mManualSubscriptionList = ManualSubscriptionList.GetManualSubscriptionList(txtManualName.Text.Trim, New Guid(cmbCategory.SelectedValue.ToString), Val(txtWarningDays.Text))
        Session("mManualSubscriptionList") = mManualSubscriptionList
        'mManualSubscriptionList.Sort("DueStatus", ComponentModel.ListSortDirection.Ascending)
        Dim List = (From StatusInfo As ManualSubscription In mManualSubscriptionList
                    Select StatusInfo Order By StatusInfo.DueStatus).ToList

        dgManualRevList.DataSource = List
        dgManualRevList.DataBind()

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblManual.Text = "Manual Subscription Report"
            lblList.Text = "List of Manual & Subscription as per criteria : " & mManualSubscriptionList.Count & " Record(s) found."
        Else
            lblManual.Text = "Manual Revision Report"
            lblList.Text = "List of Manual & Revision as per criteria : " & mManualSubscriptionList.Count & " Record(s) found."
        End If
    End Sub
    Private Sub ControlVisibility()
        'If mManualSubscriptionList.Count = 0 Then
        '    btnPrint.Enabled = False
        '    btnPrintTop.Enabled = False
        'Else
        '    btnPrint.Enabled = True
        '    btnPrintTop.Enabled = True
        'End If

        'If mManualSubscriptionList.Count > 20 Then
        '    btnPrintTop.Visible = True
        '    btnCloseTop.Visible = True
        'Else
        '    btnPrintTop.Visible = True
        '    btnCloseTop.Visible = True
        'End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
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

        mManualSubscriptionList = ManualSubscriptionList.GetManualSubscriptionList(txtManualName.Text.Trim, New Guid(cmbCategory.SelectedValue.ToString), Val(txtWarningDays.Text))
        ' mManualSubscriptionList.Sort("DueStatus", ComponentModel.ListSortDirection.Ascending)
        Dim List = (From StatusInfo As ManualSubscription In mManualSubscriptionList
                    Select StatusInfo Order By StatusInfo.DueStatus).ToList

        dgManualRevList.DataSource = List

        dgManualRevList.DataBind()
        Session("mManualSubscriptionList") = mManualSubscriptionList

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblList.Text = "List of Manual & Subscription as per criteria : " & mManualSubscriptionList.Count & " Record(s) found."
        Else
            lblList.Text = "List of Manual & Revision as per criteria : " & mManualSubscriptionList.Count & " Record(s) found."
        End If
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
        dgManualRevList.DataSource = mManualSubscriptionList
        Session("mManualSubscriptionList") = mManualSubscriptionList
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
        ' Dim Rpt As New crManualRevision
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsManualSubscription
        Dim Obj As ManualSubscriptionList
        ' Dim mManualSubscriptionList As ManualSubscriptionList
        Rpt = New crManualSubscriptionList
        'GetList()
        mManualSubscriptionList = Session("mManualSubscriptionList")
        'dgManualRevList.DataSource = mManualSubscriptionList
        'dgManualRevList.DataBind()


        Dim ReportName As String
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            ReportName = "Manual Subscription Report"
        Else
            ReportName = "Manual Revision Report"
        End If

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, ReportName, "", txtManualName.Text.Trim, IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.ToString, ""), "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        dgManualRevList.Visible = True

        Obj = mManualSubscriptionList
        ds.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 27-Feb-2012
        da.Fill(ds, Obj)
        da.Fill(ds, mrptImage) 'Added by Shweta on 27-Feb-2012
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)

        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        'Code Added 

        'Me.dgManualRevList.DataSource = Obj
    End Sub

    Private Sub dgManualRevList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgManualRevList.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim WarningDays As Integer = Integer.Parse(txtWarningDays.Text)
            Dim DueStatus As Integer = Integer.Parse(e.Row.Cells(20).Text)

            If DueStatus = 1 Then

                e.Row.Cells(21).BackColor = System.Drawing.Color.Red

            ElseIf DueStatus = 2 Then
                e.Row.Cells(21).BackColor = System.Drawing.Color.Yellow
            ElseIf DueStatus = 3 Then
                e.Row.Cells(21).BackColor = System.Drawing.Color.Green
            End If

        End If


    End Sub

#End Region

#End Region



End Class