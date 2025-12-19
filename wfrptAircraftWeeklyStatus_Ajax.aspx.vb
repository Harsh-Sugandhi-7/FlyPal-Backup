Public Class wfrptAircraftWeeklyStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mModelList As ModelList
    Dim StartDate As String
    Dim MachineName As String
    Dim ModelID As Guid
    Dim Aircraft As String
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mModelList = CType(Session("mModelList"), ModelList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptAircraftWeeklyStatus_Ajax.aspx" Then
            Session.Remove("mModelList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtDate.Text.Trim) Then
            StartDate = ""
        Else
            StartDate = txtDate.Text.Trim
        End If
        If cmbAircraft.SelectedIndex = 0 Then
            ModelID = New Guid("{00000000-0000-0000-0000-000000000000}")
        Else
            ModelID = New Guid(Request.Form("cmbAircraft").ToString)
        End If
        Aircraft = IIf(ModelID.Equals(Guid.Empty), "", mModelList(ModelID).ModelName)
        lblDateRangeFrom.Text = "Date : " & IIf(StartDate <> "", StartDate, "")
        lblAircraft1.Text = "Model : " & IIf(Aircraft <> "", Aircraft, "")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblAircraft1.Text
    End Sub
    Private Sub ResetValues()
        StartDate = txtDate.Text.Trim
        ModelID = Guid.Empty
        Aircraft = ""
    End Sub
    Private Sub SetReport()
        Dim ReportName As String = ""
        SetValues()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim String2 As String = String.Empty
        Dim String4 As String = String.Empty
        Dim String5 As String = String.Empty
        Dim mAircraftWeeklyStatus As AircraftWeeklyStatus
        Dim mTotalHoursModelWise As TotalHoursModelWise
        Dim mFlightLogClassificationNameAndCount As FlightLogClassificationNameAndCount
        Dim mMachineCertificateListByModel As MachineCertificateListByModel 'Added By Vikrant On 07-Apr-2015
        Dim dsAircraftWeeklyStatus As New dsAircraftWeeklyStatus
        myReport = New crptAircraftWeeklyStatus

        mAircraftWeeklyStatus = AircraftWeeklyStatus.GetAircraftWeeklyStatus(ModelID.ToString, StartDate)
        mTotalHoursModelWise = TotalHoursModelWise.GetTotalHoursModelWise(ModelID.ToString, StartDate)
        mFlightLogClassificationNameAndCount = FlightLogClassificationNameAndCount.GetFlightLogClassificationNameAndCount(ModelID.ToString, StartDate)
        mMachineCertificateListByModel = MachineCertificateListByModel.GetMachineCertificateList(ModelID.ToString, StartDate) 'Added By Vikrant On 07-Apr-2015

        If mAircraftWeeklyStatus.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            String2 = "From : " + mAircraftWeeklyStatus(0).FromDateFormatted + " To " + mAircraftWeeklyStatus(0).ToDateFormatted
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1291)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "Aircraft Weekly Status Report", StartDate, String2, "", String4, String5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(dsAircraftWeeklyStatus)
        da.Fill(dsAircraftWeeklyStatus, mAircraftWeeklyStatus)
        da.Fill(dsAircraftWeeklyStatus, mTotalHoursModelWise)
        da.Fill(dsAircraftWeeklyStatus, mFlightLogClassificationNameAndCount)
        da.Fill(dsAircraftWeeklyStatus, Report)
        da.Fill(dsAircraftWeeklyStatus, mrptImage)
        da.Fill(dsAircraftWeeklyStatus, mMachineCertificateListByModel) 'Added By Vikrant On 07-Apr-2015
        myReport.SetDataSource(dsAircraftWeeklyStatus)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "AircraftWeeklyStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModelList = ModelList.GetAirframeModelList("(All)")
        cmbAircraft.DataSource = mModelList
        Session("mModelList") = mModelList
        cmbAircraft.DataBind()
    End Sub
#End Region

#Region "events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptAircraftWeeklyStatus_Ajax.aspx"
            ResetValues()
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
           SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mModelList = Nothing
        Session("MiddleFrame") = ""
        Session.Remove("mModelList")
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

End Class