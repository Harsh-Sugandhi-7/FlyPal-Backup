'Added by Utkarsh on 22-Jan-2014

Public Class wfrptAircraftStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As Guid
    Dim Aircraft As String
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptAircraftStatus_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text.Trim) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.Trim
        End If
        If Not IsDate(txtToDate.Text.Trim) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.Trim
        End If
        MachineID = New Guid(Request.Form("cmbAircraft").ToString)
        Aircraft = IIf(MachineID.Equals(Guid.Empty), "", mMachineNameValueList(MachineID).RegNo)
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", StartDate, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", EndDate, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblAircraft1.Text
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.Trim
        EndDate = txtToDate.Text.Trim
        MachineID = Guid.Empty
        Aircraft = ""
    End Sub
    Private Sub SetReport()
        Dim ReportName As String = ""
        SetValues()

        Dim ReportStatusList As New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim mrptAircraftStatus As New rptAircraftStatus
        Dim dsAircraftStatus As New dsAircraftStatus
        Dim mAircraftUtilization As AircraftUtilization

        Dim mtmpMachineList As tmpMachineList
        If Not Aircraft = "(All)" Then
            mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, EndDate)
            For i As Integer = 0 To mtmpMachineList.Count - 1
                ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , Year(CDate(txtFromDate.Text.Trim)).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted
            Next
        End If

        myReport = New crptAircraftStatus

        mrptAircraftStatus = rptAircraftStatus.GetAircraftStatus(StartDate, EndDate, "", _
         "", "", "", "", "", "", "", MachineID.ToString, True, True, True, True, True, Guid.Empty.ToString, True, True, True)   'New Guid("{603F0E6F-C824-4FA1-BCD1-642141BCE053}").ToString ,New Guid("{3f246d1d-a52f-4dae-9fcd-f7e00141ce71}").ToString

        mAircraftUtilization = AircraftUtilization.GetAircraftUtilization(StartDate, EndDate, MachineID.ToString)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "", StartDate, EndDate, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mrptAircraftStatus.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1260)
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(dsAircraftStatus)
        da.Fill(dsAircraftStatus, mrptAircraftStatus)
        da.Fill(dsAircraftStatus, Report)
        da.Fill(dsAircraftStatus, ReportStatusList)
        da.Fill(dsAircraftStatus, mAircraftUtilization)
        da.Fill(dsAircraftStatus, mrptImage)
        myReport.SetDataSource(dsAircraftStatus)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "AircraftStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'ResetValues()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
#End Region

#Region "events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptAircraftStatus_Ajax.aspx"
            ResetValues()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub


    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        Session.Remove("mMachineNameValueList")
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region
End Class