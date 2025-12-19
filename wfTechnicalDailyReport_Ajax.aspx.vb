'Added by Utkarsh on 23-Jan-2014

Public Class wfTechnicalDailyReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mAircraftCurrentStatusList As ListOfAircraftCurrentStatusForDaily
    Public mCompInstRemForTechnicalDailyReport As CompInstRemForTechnicalDailyReport
    Public mLogMaintenanceActivityList As LogMaintenanceActivityList
    Public mSnagCorrectiveActionListDailyReport As SnagCorrectiveActionListDailyReport
    Public mMELCorrectiveActionListDailyReport As MELCorrectiveActionListDailyReport
    Public mPartMaterialRequisitionList As PartMaterialRequisitionList
    Dim mMachineNameValueList As MachineNameValueList
    Dim MachineName As Guid
    Dim Aircraft As String
    Dim AsonDate As String = ""
    Dim BeforeAsOnDate As String = ""
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAircraftCurrentStatusList = Session("mAircraftCurrentStatusList")
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mAircraftCurrentStatusList") = mAircraftCurrentStatusList
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRange.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        MachineName = New Guid(Request.Form("cmbAircraft").ToString)
        If MachineName.Equals(Guid.Empty) Then
            Aircraft = ""
        Else
            Aircraft = mMachineNameValueList(MachineName).RegNo
            lblAircraft1.Text = "Aircraft Name : " & Aircraft
        End If
        If Not IsDate(txtDate.Text.Trim) Then
            AsonDate = ""
        Else
            AsonDate = txtDate.Text.Trim
            BeforeAsOnDate = CType(DateAdd(DateInterval.Day, -1, CDate(txtDate.Text.Trim)), String)
            lblDateRange.Text = "As On Date : " & txtDate.Text.Trim
        End If
        EventLogDetail = lblDateRange.Text + ", " + lblAircraft1.Text
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCompInstRemForTechnicalDailyReport
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ActualRoute As String = ""

        SetValues()
        Dim mLoglist As LogList = LogList.GetLogList(MachineName, AsonDate, AsonDate)
        If mLoglist.Count = 1 Then
            Dim mlog As Log = Log.GetLog(mLoglist(0).ID)
            ActualRoute = mlog.SourceName + " - " + mlog.DestinationName
        Else
            Dim i As Integer = mLoglist.Count - 1
            While i >= 0
                Dim mlog As Log = Log.GetLog(mLoglist(i).ID)
                If mLoglist(i).LogTypeID = 1 Then
                    If i = mLoglist.Count - 1 Then
                        ActualRoute = mlog.SourceName + " - " + mlog.DestinationName
                    ElseIf i = 0 And ActualRoute = "" Then
                        ActualRoute = mlog.SourceName + " - " + mlog.DestinationName
                    Else
                        ActualRoute = ActualRoute + " - " + mlog.DestinationName
                    End If
                End If

                i = i - 1
            End While

        End If
        'Dim mMaxLogOfAircraft As MaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(New Guid(MachineName))
        'If CDate(txtDate.Value.ToString) = CDate(mMaxLogOfAircraft.LogDate) Then
        '    Dim mlog As Log = Log.GetLog(mMaxLogOfAircraft.LogID)
        '    ActualRoute = mlog.SourceName + " / " + mlog.DestinationName
        'End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
mCompanyDetail.WebSite, "", AsonDate, Aircraft, ActualRoute, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        SetSession()
        myReport = New crptTechnicalDailyReport



        'AircraftCurrentReport
        mAircraftCurrentStatusList = ListOfAircraftCurrentStatusForDaily.GetListOfAircraftCurrentStatusForDaily(, Aircraft, , , , AsonDate)
        Session("mAircraftCurrentStatusList") = mAircraftCurrentStatusList
        '----------------------------

        'Component Inst/Rem Details
        mCompInstRemForTechnicalDailyReport = CompInstRemForTechnicalDailyReport.GetCompInstRemForTechnicalDailyReport(MachineName, AsonDate)
        Session("mCompInstRemForTechnicalDailyReport") = mCompInstRemForTechnicalDailyReport
        '----------------------------
        'Log Maintenance Activity List
        mLogMaintenanceActivityList = LogMaintenanceActivityList.GetLogMaintenanceActivityList(MachineName.ToString, AsonDate, AsonDate, True)
        Session("mLogMaintenanceActivityList") = mLogMaintenanceActivityList

        'Snag Corrective Action List
        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mSnagCorrectiveActionListDailyReport = SnagCorrectiveActionListDailyReport.GetSnagListDailyReport(AsonDate, MachineName, , "HH:mm")
        Else
            mSnagCorrectiveActionListDailyReport = SnagCorrectiveActionListDailyReport.GetSnagListDailyReport(AsonDate, MachineName)
        End If
        Session("mSnagCorrectiveActionListDailyReport") = mSnagCorrectiveActionListDailyReport

        'MEL Corrective Action List
        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mMELCorrectiveActionListDailyReport = MELCorrectiveActionListDailyReport.GetMELListDailyReport(AsonDate, MachineName, , "HH:mm")
        Else
            mMELCorrectiveActionListDailyReport = MELCorrectiveActionListDailyReport.GetMELListDailyReport(AsonDate, MachineName)
        End If
        Session("mMELCorrectiveActionListDailyReport") = mMELCorrectiveActionListDailyReport

        mPartMaterialRequisitionList = PartMaterialRequisitionList.GetPartMaterialRequisitionList(AsonDate, MachineName)
        Session("mPartMaterialRequisitionList") = mPartMaterialRequisitionList

        If mAircraftCurrentStatusList.Count = 0 And mCompInstRemForTechnicalDailyReport.Count = 0 And mLogMaintenanceActivityList.Count = 0 And mSnagCorrectiveActionListDailyReport.Count = 0 And mMELCorrectiveActionListDailyReport.Count = 0 And mPartMaterialRequisitionList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1264)

        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mAircraftCurrentStatusList)
        da.Fill(ds, mCompInstRemForTechnicalDailyReport)
        da.Fill(ds, mLogMaintenanceActivityList)
        da.Fill(ds, mSnagCorrectiveActionListDailyReport)
        da.Fill(ds, mMELCorrectiveActionListDailyReport)
        da.Fill(ds, mPartMaterialRequisitionList)

        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "TechnicalDailyReport", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfTechnicalDailyReport_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAircraftCurrentStatusList")
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        DataBind()
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack Then 'And Session("Sender") = "" 
            Session("MiddleFrame") = "wfTechnicalDailyReport_Ajax.aspx"
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(txtDate.Text)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "TechnicalDailyReport", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("DashBoard.aspx")
    End Sub
End Class