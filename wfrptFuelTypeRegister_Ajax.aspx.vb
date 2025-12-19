'Ajax Conversion By Vikrant On 29-Jan-2014
'Added By Vikrant On 14-June-2013 For ALL05062013

Public Class wfrptFuelTypeRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList
    Dim StartDate As String
    Dim EndDate As String
    Dim Aircraft As String
    Dim EventLogDetail As String = String.Empty

    'Added By Abhishek on 22-SEP-2017
    Dim mCompanyDetail As New CompanyDetail
    Dim dsFuelTypeRegister As New dsFuelTypeRegister
    Dim objFuelTypeRegister As FuelTypeRegister
    Dim da As New CSLA.Data.ObjectAdapter
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptFuelTypeRegister_Ajax.aspx?" Then
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
        upnlCurrentCriteria.Update()
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
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        If StartDate <> "" Then
            lblDateRangeFrom.Text = "From Date : " & New SmartDate(StartDate).FormattedText
        Else
            lblDateRangeFrom.Text = "From Date : "
        End If
        If EndDate <> "" Then
            lblDateRangeTo.Text = "To Date : " & New SmartDate(EndDate).FormattedText
        Else
            lblDateRangeTo.Text = "To Date : "
        End If
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")

        EventLogDetail = lblDateRangeFrom.Text + "," + lblDateRangeTo.Text + "," + lblAircraft1.Text
    End Sub
    Private Sub SetReport()
        Dim mCompanyDetail As New CompanyDetail
        Dim dsFuelTypeRegister As New dsFuelTypeRegister
        Dim objFuelTypeRegister As FuelTypeRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

        SetValues()
        Dim OperatorName As String = ""
        myReport = New crFuelTypeRegister

        objFuelTypeRegister = FuelTypeRegister.GetFuelTypeRegister(StartDate, EndDate, New Guid(cmbAircraft.SelectedValue))

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    mCompanyDetail.WebSite, "Fuel Type Register", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, cmbAircraft.SelectedItem.Text, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"))

        If objFuelTypeRegister.Count = 0 Then
           MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1274)
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(dsFuelTypeRegister)
        da.Fill(dsFuelTypeRegister, objFuelTypeRegister)
        da.Fill(dsFuelTypeRegister, mrptImage)
        da.Fill(dsFuelTypeRegister, Report)
        myReport.SetDataSource(dsFuelTypeRegister)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "FuelTypeRegister", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    'Response.Redirect("wfrptFuelTypeRegister_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfrptFuelTypeRegister_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(Select)", , True)
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
            Session("MiddleFrame") = "wfrptFuelTypeRegister_Ajax.aspx?"
            txtFromDate.Text = CDate(Today.AddDays(1).AddYears(-1)).ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    'Added By Abhishek on 22-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            Dim OperatorName As String = ""
            objFuelTypeRegister = FuelTypeRegister.GetFuelTypeRegister(StartDate, EndDate, New Guid(cmbAircraft.SelectedValue))
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            End If

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Fuel Type Register", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, cmbAircraft.SelectedItem.Text, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"))

            If objFuelTypeRegister.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1274)
            End If
            da.Fill(dsFuelTypeRegister, objFuelTypeRegister)
            da.Fill(dsFuelTypeRegister, Report)
            Dim columnToRemove1 As String() = {"MachineID", "MachineName", "UnitID", "UnitName", "FuelTypeID", "LogNo", "LogID", "LogDate"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If dsFuelTypeRegister.Tables("FuelTypeRegister").Columns.Contains(columnToRemove1(i)) Then
                    dsFuelTypeRegister.Tables("FuelTypeRegister").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"ID", "CompanyName", "ShortName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr4", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr5", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsFuelTypeRegister.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    dsFuelTypeRegister.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If dsFuelTypeRegister.Tables("ReportData").Columns.Contains("SearchStr1") Then
                dsFuelTypeRegister.Tables("ReportData").Columns("SearchStr1").ColumnName = "FromDate "
            End If

            If dsFuelTypeRegister.Tables("ReportData").Columns.Contains("SearchStr2") Then
                dsFuelTypeRegister.Tables("ReportData").Columns("SearchStr2").ColumnName = "DateTo "
            End If
            If dsFuelTypeRegister.Tables("ReportData").Columns.Contains("SearchStr3") Then
                dsFuelTypeRegister.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
            End If


            If dsFuelTypeRegister.Tables("FuelTypeRegister").Columns.Contains("FuelTypeName") Then
                dsFuelTypeRegister.Tables("FuelTypeRegister").Columns("FuelTypeName").ColumnName = "Fuel Type"
            End If

            If dsFuelTypeRegister.Tables("FuelTypeRegister").Columns.Contains("TotalFuelUpliftedPerFuelType") Then
                dsFuelTypeRegister.Tables("FuelTypeRegister").Columns("TotalFuelUpliftedPerFuelType").ColumnName = "Fuel Uplifted"
            End If
        

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsFuelTypeRegister.Tables("ReportData"))
            dsNew.Merge(dsFuelTypeRegister.Tables("FuelTypeRegister"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("FuelTypeRegister").TableName = "Fuel Type Register"
			Session("ExcelFileName") = "Fuel Type Register"
			Session("dsNew") = dsNew
			'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
			'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
			'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
			'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "FuelTypeRegister", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
End Class