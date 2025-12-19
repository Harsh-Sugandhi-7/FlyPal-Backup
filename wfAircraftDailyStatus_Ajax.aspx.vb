Public Class wfAircraftDailyStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Aircraft, Remark As String
    Dim AircraftIndex As Integer
    Dim MachineName As String
    Private mMachineNameValueList As MachineNameValueList

    Private mAircraftDailyStatusList As DailyStatusList
    Private mAircraftDSCList As DailyStatusList

    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAircraftDailyStatusList = CType(Session("mAircraftDailyStatusList"), DailyStatusList)
        mAircraftDSCList = CType(Session("mAircraftDSCList"), DailyStatusList)

        MachineName = Session("AircraftId")
        Aircraft = Session("Aircraft")
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mAircraftDailyStatusList") = mAircraftDailyStatusList
        Session("mAircraftDSCList") = mAircraftDSCList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfAircraftDailyStatus_Ajax.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAircraftDailyStatusList")
            Session.Remove("mAircraftDSCList")
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mAircraftDailyStatusList")
        Session.Remove("mAircraftDSCList")
        Session.Remove("AircraftId")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        upnlSearchCriteria.Update()
    End Sub
    Private Sub SetValues()
        If cmbAircraft.SelectedItem.Text = "(SELECT)" Then
            Aircraft = ""
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            lblAircraft1.Text = "Aircraft : " & Aircraft
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft : " & Aircraft
        End If
        Remark = Trim(txtRemark.Text)
        Session("AircraftId") = MachineName
        Session("Aircraft") = Aircraft
        Session("Remark") = Remark
        mSearchCriteriaForEventLog = lblAircraft1.Text
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Private Sub SetPage()
        If Not mAircraftDailyStatusList Is Nothing Then
            lblResult.Text = "List of Maintenance Activities: " & mAircraftDailyStatusList.Count & " Record(s) found"
        End If

        If Not mAircraftDSCList Is Nothing Then
            lblResult1.Text = "List of Certificates: " & mAircraftDSCList.Count & " Record(s) found"
        End If
    End Sub
    ''Private Sub SetReport()
    ''    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    ''    Dim da As New CSLA.Data.ObjectAdapter
    ''    Dim ds As New Flypal.AircraftInformationBoard.dsAircraftDailyStatus

    ''    Dim mCompanyDetail As New CompanyDetail
    ''    SetValues()

    ''    Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    ''    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    ''    mCompanyDetail.WebSite, "Aircraft Daily Status Report", Aircraft, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))

    ''    mAircraftDailyStatusList = Session("mAircraftDailyStatusList")
    ''    mAircraftDailyStatus = Flypal.AircraftInformationBoard.AircraftDailyStatus.GetAircraftDailyStatusList(New Guid(cmbAircraft.SelectedValue.ToString), User.Identity.Name)
    ''    Dim mBoardInfoList As Flypal.AircraftInformationBoard.BoardInfoList = Flypal.AircraftInformationBoard.BoardInfoList.GetBoardInfoList(New Guid(cmbAircraft.SelectedValue.ToString))

    ''    If mAircraftDailyStatus.Count <= 0 Then
    ''       Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
    ''       msg1.ReplacePage = "wfAircraftDailyStatus_Ajax.aspx?Backpage="
    ''        msg1.Show()
    ''        Exit Sub
    ''    End If
    ''    ds.Clear()
    ''    da.Fill(ds, mAircraftDailyStatus)
    ''    da.Fill(ds, mBoardInfoList)
    ''    da.Fill(ds, Report)
    ''    myReport = New Flypal.AircraftInformationBoard.crAircraftDailystatus

    ''    myReport.SetDataSource(ds)
    ''    Session("CrystalReport") = myReport
    ''    Dim Str As String
    ''    Str = "<script language=Javascript>openTranDetail();</script>"
    ''     ClientScript.RegisterStartupScript(Me.GetType(),"openTranDetail", Str)
    ''End Sub

    Private Sub SetReport1()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsAircraftDailyStatusOfMaintenanceActivity
        Dim mCompanyDetail As New CompanyDetail
        SetValues()
        'mAircraftDSCList = CType(Session("mAircraftDSCList"), DailyStatusList)

        Dim mAircraftDailyStatusOfMaintenanceActivity As AircraftDailyStatusOfMaintenanceActivity
        mAircraftDailyStatusOfMaintenanceActivity = AircraftDailyStatusOfMaintenanceActivity.GetAircraftDailyStatusOfMaintenanceActivityList(New Guid(cmbAircraft.SelectedValue.ToString))

        Dim mAircraftDailyStatus As Flypal.AircraftInformationBoard.AircraftDailyStatus = Flypal.AircraftInformationBoard.AircraftDailyStatus.GetAircraftDailyStatusList(New Guid(cmbAircraft.SelectedValue.ToString), User.Identity.Name)
        Dim mFuelOnArrival As FuelOnArrivalForMachine = FuelOnArrivalForMachine.GetFuelOnArrivalForMachine(New Guid(cmbAircraft.SelectedValue.ToString))

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Aircraft Daily Status Report", Aircraft, mAircraftDailyStatus(New Guid(cmbAircraft.SelectedValue.ToString)).AircraftLocation, mAircraftDailyStatus(New Guid(cmbAircraft.SelectedValue.ToString)).Serviceability, Remark, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mAircraftDailyStatusOfMaintenanceActivity.Count <= 0 And mAircraftDSCList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1163)
        End If
        ds.Clear()

        da.Fill(ds, mAircraftDailyStatus)
        da.Fill(ds, mFuelOnArrival)
        da.Fill(ds, mAircraftDailyStatusOfMaintenanceActivity)
        da.Fill(ds, mAircraftDSCList)
        da.Fill(ds, Report)

        ''   myReport = New crAircraftDailyStatusOfMaintenanceActivity
        myReport = New crADSReport
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "AircraftDailyStatus", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()

        If Not mAircraftDailyStatusList Is Nothing Then
            Session("mAircraftDailyStatusList") = mAircraftDailyStatusList
        End If

        If Not mAircraftDSCList Is Nothing Then
            Session("mAircraftDSCList") = mAircraftDSCList
        End If

        dgAircraftDSCList.DataSource = mAircraftDSCList
        dgAircraftDSCList.DataBind()

        dgDailyStatusList.DataSource = mAircraftDailyStatusList
        dgDailyStatusList.DataBind()

        If (Not MachineName Is Nothing) And (MachineName <> Guid.Empty.ToString) Then
            mAircraftDailyStatusList = DailyStatusList.GetDailyStatusList(New Guid(MachineName.ToString))
            Session("mAircraftDailyStatusList") = mAircraftDailyStatusList
            dgDailyStatusList.DataSource = mAircraftDailyStatusList
            dgDailyStatusList.PageIndex = 0
            dgDailyStatusList.DataBind()

            mAircraftDSCList = DailyStatusList.GetDailyStatusList(New Guid(MachineName.ToString), Guid.Empty.ToString, Guid.Empty.ToString, 7, True)
            Session("mAircraftDSCList") = mAircraftDSCList
            dgAircraftDSCList.DataSource = mAircraftDSCList
            dgAircraftDSCList.PageIndex = 0
            dgAircraftDSCList.DataBind()

            cmbAircraft.SelectedValue = MachineName
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfAircraftDailyStatus_Ajax.aspx?"
            DataFieldBind()
            SetFocus(cmbAircraft)
            ResetValues()
            SetPage()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            '' SetReport() ''dispalays old report
            SetReport1()  ''displays new report
        End If
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex > 0 Then
            mAircraftDailyStatusList = DailyStatusList.GetDailyStatusList(New Guid(cmbAircraft.SelectedValue.ToString))
            Session("mAircraftDailyStatusList") = mAircraftDailyStatusList
            dgDailyStatusList.DataSource = mAircraftDailyStatusList
            Session("mDailyStatusList") = mAircraftDailyStatusList
            dgDailyStatusList.PageIndex = 0
            dgDailyStatusList.DataBind()

            mAircraftDSCList = DailyStatusList.GetDailyStatusList(New Guid(cmbAircraft.SelectedValue.ToString), Guid.Empty.ToString, Guid.Empty.ToString, 7, True)
            Session("mAircraftDSCList") = mAircraftDSCList
            dgAircraftDSCList.DataSource = mAircraftDSCList

            Session("mDailyStatusCertificateList") = mAircraftDSCList
            dgAircraftDSCList.PageIndex = 0
            dgAircraftDSCList.DataBind()

            SetPage()
        Else
            mAircraftDailyStatusList = Nothing
            dgAircraftDSCList.DataSource = mAircraftDailyStatusList '' Nothing
            dgAircraftDSCList.DataBind()
            Session("mDailyStatusList") = mAircraftDailyStatusList
            lblResult.Text = ""


            mAircraftDSCList = Nothing
            dgDailyStatusList.DataSource = mAircraftDSCList '' Nothing
            dgDailyStatusList.DataBind()
            lblResult1.Text = ""

            Session("mAircraftDailyStatusList") = mAircraftDailyStatusList
            Session("mAircraftDSCList") = mAircraftDSCList
            Session("mDailyStatusCertificateList") = mAircraftDSCList

            SetPage()
        End If
        setFocus(cmbAircraft)
        txtRemark.Text = ""
    End Sub
    Private Sub btnMaintenanceActivity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMaintenanceActivity.Click
        If IsValid Then
            SetValues()
            Dim RegNo As String = cmbAircraft.SelectedItem.ToString
            Session("mDailyStatusList") = mAircraftDailyStatusList
            Session("mDailyStatusCertificateList") = mAircraftDSCList
            Dim str As String
            str = "openledgersame('wfDailyStatus_Ajax.aspx?BackPage=Index.aspx" & "&MachineID=" & cmbAircraft.SelectedValue.ToString & "&RegNo=" & RegNo & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgDailyStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDailyStatusList.PageIndexChanging
        dgDailyStatusList.PageIndex = e.NewPageIndex
        dgDailyStatusList.DataSource = mAircraftDailyStatusList
        Session("mAircraftDailyStatusList") = mAircraftDailyStatusList
        setFocus(cmbAircraft)
        dgDailyStatusList.DataBind()

        dgAircraftDSCList.DataSource = mAircraftDSCList
        dgAircraftDSCList.DataBind()
    End Sub
    Private Sub dgDailyStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDailyStatusList.Sorting
        mAircraftDailyStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAircraftDailyStatusList") = mAircraftDailyStatusList
        dgDailyStatusList.DataSource = mAircraftDailyStatusList
        dgDailyStatusList.DataBind()

        dgAircraftDSCList.DataSource = mAircraftDSCList
        dgAircraftDSCList.DataBind()
    End Sub
    Private Sub dgAircraftDSCList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAircraftDSCList.PageIndexChanging
        dgAircraftDSCList.PageIndex = e.NewPageIndex
        dgAircraftDSCList.DataSource = mAircraftDSCList
        Session("mAircraftDSCList") = mAircraftDSCList
        setFocus(cmbAircraft)
        dgAircraftDSCList.DataBind()

        dgDailyStatusList.DataSource = mAircraftDailyStatusList
        dgDailyStatusList.DataBind()
    End Sub
    Private Sub dgAircraftDSCList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAircraftDSCList.Sorting
        mAircraftDSCList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAircraftDSCList") = mAircraftDSCList
        dgAircraftDSCList.DataSource = mAircraftDSCList
        dgAircraftDSCList.DataBind()

        dgDailyStatusList.DataSource = mAircraftDailyStatusList
        dgDailyStatusList.DataBind()
    End Sub
#End Region

    
End Class