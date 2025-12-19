

'CREATED By : Saylee
'Dated      : 23-Jan-2014


Public Class wfrptSectorProfileListForFuel_AJAX
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Private mrptSectorProfileGraphReportForFuel As rptSectorProfileGraphReportForFuel
    Public mMachineNameValueList As MachineNameValueList
    Dim FromDate, ToDate As String
    Private mrptSectors As rptSectors

    Dim EventLogID As Guid
    Dim mSectorProfileSearchingCriteria As String = String.Empty
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mrptSectorProfileGraphReportForFuel = Session("mrptSectorProfileGraphReportForFuel")
        mMachineNameValueList = Session("mMachineNameValueList")
        mrptSectors = Session("mrptSectors")
    End Sub
    Private Sub SetSession()
        Session("mrptSectorProfileGraphReportForFuel") = mrptSectorProfileGraphReportForFuel
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mrptSectors") = mrptSectors
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptSectorProfileGraphReportForFuel")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblModel1.Visible = True
    End Sub
    Public Sub setValues()
        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText
        lblModel1.Text = "Model : " & IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "ALL")

        mSectorProfileSearchingCriteria = lblDateRangeFrom.Text + ", " + lblModel1.Text
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, , , , , , , True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()
        Session("mMachineNameValueList") = mMachineNameValueList

        mrptSectors = rptSectors.GetSectors(txtFromDate.Text.ToString, txtToDate.Text.ToString)
        chkSectors.DataSource = mrptSectors
        chkSectors.DataBind()
        Session("mrptSectors") = mrptSectors
        If chkSectors.Items.Count > 0 Then chkSelectAll.Visible = True
    End Sub
    Private Sub AddSectors()
        Dim PageItems As Integer
        Dim i As Integer
        PageItems = chkSectors.Items.Count - 1

        For i = 0 To PageItems
            mrptSectors(i).IsSelected = chkSectors.Items.Item(i).Selected
        Next
        Session("mrptSectors") = mrptSectors
    End Sub
    Public Sub SetReport()
        GetSession()
        'Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsSectorProfileGraphReport As New dsSectorProfileGraphReport
        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String

        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        SearchStr1 = New SmartDate(FromDate).FormattedText
        SearchStr2 = New SmartDate(ToDate).FormattedText
        SearchStr3 = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.ToString, "ALL")

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
          mCompanyDetail.WebSite, "Sector Profile Report (Fuel Consumption)", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        AddSectors()
        mrptSectorProfileGraphReportForFuel = rptSectorProfileGraphReportForFuel.GetSectorProfileGraphReportList(FromDate, ToDate, cmbAircraft.SelectedValue.ToString, mrptSectors)
        Session("mrptSectorProfileGraphReportForFuel") = mrptSectorProfileGraphReportForFuel

        If mrptSectorProfileGraphReportForFuel.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptSectorProfileListForFuel.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            ' Dim rme As RecentMenuEvent
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1277)
        End If

        Dim myReport = New crptSectorProfileGraphReportForFuel

        Dim mrptImage As rptImage = rptImage.GetImage(dsSectorProfileGraphReport)
        da.Fill(dsSectorProfileGraphReport, mrptSectorProfileGraphReportForFuel)
        da.Fill(dsSectorProfileGraphReport, Report)
        da.Fill(dsSectorProfileGraphReport, mrptImage)
        myReport.SetDataSource(dsSectorProfileGraphReport)

        Session("CrystalReport") = myReport

         ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "FlightLogBook", mSectorProfileSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        ''AddAttributes()
        If Not IsPostBack Then
            txtFromDate.Text = CDate(Today.AddMonths(-1).AddDays(1)).ToString(AppSettings("DateFormat")) 'Today.Date.ToString
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
      If Not IsValid() Then upnlValidationSummary.Update() : Exit Sub
        setValues()
        SetReport()
    End Sub
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSelectAll.CheckedChanged
        For i As Integer = 0 To chkSectors.Items.Count - 1
            chkSectors.Items.Item(i).Selected = chkSelectAll.Checked
        Next

    End Sub
    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        mrptSectors = rptSectors.GetSectors(txtFromDate.Text.ToString, txtToDate.Text.ToString)
        chkSectors.DataSource = mrptSectors
        chkSectors.DataBind()
        Session("mrptSectors") = mrptSectors
        If chkSectors.Items.Count > 0 Then chkSelectAll.Visible = True
        chkSelectAll.Checked = False
        upnlSectors.Update()
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        mrptSectors = rptSectors.GetSectors(txtFromDate.Text.ToString, txtToDate.Text.ToString)
        chkSectors.DataSource = mrptSectors
        chkSectors.DataBind()
        Session("mrptSectors") = mrptSectors
        If chkSectors.Items.Count > 0 Then chkSelectAll.Visible = True
        chkSelectAll.Checked = False
        upnlSectors.Update()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        setValues()
        upnlCriteria.Update()
    End Sub
#End Region

End Class