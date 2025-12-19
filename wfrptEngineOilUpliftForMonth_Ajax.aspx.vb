'Added by utkarsh on 29-Jan-2014
Public Class wfrptEngineOilUpliftForMonth_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mMachineNameValueList As MachineNameValueList
    Public mEngineOilUpliftForMonth As EngineOilUpliftForMonth
    Dim EventLogDetail As String
    Dim MachineID As Guid
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        'If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
        '    For i As Integer = -10 To 10
        '        cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
        '    Next
        '    cmbYear.SelectedIndex = 10
        'End If

        'For k As Integer = 1 To 12
        '    Dim mon As String = MonthName(k, False)
        '    cmbMonth.Items.Add(mon)
        'Next
    End Sub
    Private Sub DataFieldBinding()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
        lblToDate.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        lblyear1.Text = "From Date : " & New SmartDate(txtFromDate.Text).FormattedText '"Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        lblToDate.Text = "To Date : " & New SmartDate(txtToDate.Text).FormattedText
        MachineID = New Guid(Request.Form("cmbAircraft").ToString)
        lblModel1.Text = "Aircraft : " & IIf(MachineID.Equals(Guid.Empty), "", mMachineNameValueList(MachineID).RegNo)
        EventLogDetail = lblyear1.Text + ", " + lblToDate.Text + ", " + lblModel1.Text
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ds As New dsEngineOilUpliftForMonth
        SetValues()
        Dim myReport = New crptEngineOilUpliftForMonth

        'Added by Saylee on 26-Dec-2018 for APFT21122018 
        If AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then
            myReport = New crptEngineOilUpliftForMonthAPFT
        Else
            myReport = New crptEngineOilUpliftForMonth
        End If
        '***********************************

        mEngineOilUpliftForMonth = EngineOilUpliftForMonth.GetEngineOilUpliftForMonth(MachineID.ToString, 0, 0, FromDate:=txtFromDate.Text, _
                                                                                      ToDate:=txtToDate.Text)
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                 mCompanyDetail.WebSite, "", 0, 0, mMachineNameValueList(MachineID).RegNo, New SmartDate(txtFromDate.Text).FormattedText, _
                  New SmartDate(txtToDate.Text).FormattedText, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mEngineOilUpliftForMonth.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1278)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mEngineOilUpliftForMonth)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "Engine Oil Uplift For Month", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not Page.IsPostBack Then
            SetCombo()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBinding()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region
End Class