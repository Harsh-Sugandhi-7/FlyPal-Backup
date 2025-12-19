Public Class wfSearchCriteriaForMaintenanceManHour_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaratioin"
    Dim mMaintenanceManHourList As MaintenanceManHourList
    Dim MachineID As String
    Dim mMachineNameValueList As MachineNameValueList
    Dim StartDate As String
    Dim EndDate As String
    Dim AssemblyType As String
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Public Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForMaintenanceManHour_Ajax.aspx?" Then
            Session.Remove("mMachineNameValueList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblSerialNo1.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text
        End If
        If (StartDate <> "") Then
            lblDateRangeFrom.Text = "From Date : " & New SmartDate(txtFromDate.Text).FormattedText
        Else
            lblDateRangeFrom.Text = "From Date : "
        End If

        If (EndDate <> "") Then
            lblDateRangeTo.Text = "To Date : " & New SmartDate(txtToDate.Text).FormattedText
        Else
            lblDateRangeTo.Text = "To Date : "
        End If
        MachineID = "{" & cmbAircraft.SelectedValue.ToString & "}"
        lblSerialNo1.Text = "Aircraft :" & IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblSerialNo1.Text
    End Sub
    Private Sub SetReport()
        Dim RptComp As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        SetValues()
        RptComp = New crMaintenanceManHour

        mMaintenanceManHourList = MaintenanceManHourList.GetMaintenanceManHourList(StartDate, EndDate, "", "", "", "", "", "", "", "", MachineID, True, True, False, False, True)


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Maintenance Man Hour Report", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, AssemblyType, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mMaintenanceManHourList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1106)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 22-Feb-2012
        da.Fill(ds, mMaintenanceManHourList)
        da.Fill(ds, mrptImage) 'Added by Shweta on 22-Feb-2012
        da.Fill(ds, Report)
        RptComp.SetDataSource(ds)
        Session("CrystalReport") = RptComp
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "MaintenanceManHour", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    'Response.Redirect("wfSearchCriteriaForMaintenanceManHour_Ajax.aspx?MsgResult=0&Backpage=&ReportType=" & Request.QueryString("ReportType"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfSearchCriteriaForMaintenanceManHour_Ajax.aspx?MsgResult=0&Backpage=&ReportType=" & Request.QueryString("ReportType"))
        End If
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

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForMaintenanceManHour_Ajax.aspx?"
            DataFieldBind()

            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
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
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class