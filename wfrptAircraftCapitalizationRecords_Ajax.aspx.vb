Public Class wfrptAircraftCapitalizationRecords_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAircraftCapitalizationRecords As AircraftCapitalizationRecords
    Public mCompanyDetail As New CompanyDetail
    Public mMachineNameValueList As MachineNameValueList
    Public mWorkShopList As WorkShopList
#End Region

#Region " Helper Methods "
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsAircraftCapitalizationRecords
        Dim AircraftOrWorkShop As Integer = 0
        Dim SearchString2 As String = String.Empty
        Dim SearchString3 As String = String.Empty
        Dim ReportName As String = String.Empty
        myReport = New crptAircraftCapitalizationRecords
        If rbAircraft.Checked = True Then
            AircraftOrWorkShop = 1
            SearchString2 = cmbAircraft.SelectedItem.Text
            SearchString3 = "Aircraft : "
            ReportName = "Aircraft Capitalization"
        Else
            AircraftOrWorkShop = 2
            SearchString2 = cmbWorkShop.SelectedItem.Text
            SearchString3 = "WorkShop : "
            ReportName = "WorkShop Capitalization"
        End If

        mAircraftCapitalizationRecords = AircraftCapitalizationRecords.GetAircraftCapitalizationRecords(txtFromDate.Text, txtToDate.Text, cmbAircraft.SelectedValue, cmbWorkShop.SelectedValue, AircraftOrWorkShop)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                  mCompanyDetail.WebSite, ReportName, New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, SearchString2, SearchString3, Today.Date.ToString(AppSettings("DateFormat")), AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If mAircraftCapitalizationRecords.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mAircraftCapitalizationRecords.Count > 0 And IsExcel = False) Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 708)
        End If

        
        If IsExcel = False Then
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mAircraftCapitalizationRecords)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage)
            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            Dim Str1 As String
            Str1 = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
            'Added by Shital on 18-Jan-2021
            Dim EventLogDetails As String = "From Date : " & New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + "To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText + " Aircraft : " & IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "All") + " WorkShop : " & IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "All")
            MarkLog(Util.Action.Print, "AircraftCapitalization", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            '-------
        Else
            ds.Clear()
            da.Fill(ds, "ExcelAircraftCapitalizationRecords", mAircraftCapitalizationRecords)
            da.Fill(ds, "ReportData", Report)

            Dim columnToRemove As String() = {"IssueDate", "MachineID", "mPartNo", "mDescription", "mIssueNo", "mIssueDate", "mRegNo", "mMachineID", "mQty", "mCAmount", "mSerialNo"}
            Dim columnToRemove1 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "SearchStr4", "ProductVersion", "SINote", "ReportDate", "SearchStr6", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExcelAircraftCapitalizationRecords").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelAircraftCapitalizationRecords").Columns.Remove(columnToRemove(i))
                End If
            Next

            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ExcelAircraftCapitalizationRecords"))

            dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = SearchString3.Replace(":", "").Trim
            dsNew.Tables("ReportData").Columns("SearchStr5").ColumnName = "Report Date"

            dsNew.Tables("ExcelAircraftCapitalizationRecords").Columns(0).ColumnName = IIf(rbAircraft.Checked, "Aircraft", "WorkShop")
            dsNew.Tables("ExcelAircraftCapitalizationRecords").Columns(5).ColumnName = "Issue Date"
            dsNew.Tables("ExcelAircraftCapitalizationRecords").Columns(7).ColumnName = "Amount"

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExcelAircraftCapitalizationRecords").TableName = ReportName
			Session("ExcelFileName") = ReportName
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Shital on 18-Jan-2021
            Dim EventLogDetails As String = "From Date : " & New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + "To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText + " Aircraft : " & IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "All") + " WorkShop : " & IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "All")
            MarkLog(Util.Action.Print, "AircraftCapitalization", "Export To excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            '-------
        End If

    End Sub
#End Region

#Region "DataFieldBind"
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, , , , , , , True, "(All)")
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()
        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(All)")
        cmbWorkShop.DataSource = mWorkShopList
        cmbWorkShop.DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            DataFieldBind()
            txtFromDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            txtToDate.Text = New SmartDate(Now.Date.ToString).FormattedText
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRangeFrom.Visible = True
        lblAircraft.Visible = True
        lblWorkShop1.Visible = True
        lblDateRangeFrom.Text = "From Date : " & New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + "To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText
        lblAircraft.Text = "Aircraft : " & IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "All")
        lblWorkShop1.Text = "WorkShop : " & IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "All")
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub rbWorkShop_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbWorkShop.CheckedChanged
        cmbAircraft.SelectedIndex = 0
        cmbWorkShop.Enabled = True
        cmbAircraft.Enabled = False
    End Sub
    Private Sub rbAircraft_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbAircraft.CheckedChanged
        cmbWorkShop.SelectedIndex = 0
        cmbWorkShop.Enabled = False
        cmbAircraft.Enabled = True
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        End If
    End Sub
#End Region

   
End Class