

'AJAX Conversion By Saylee On 29-Sep-2014

Public Class wfLogMaintenanceActivityList_Ajax
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Dim mLogMaintenanceActivityList As LogMaintenanceActivityList
    Dim mMachineNameValueList As MachineNameValueList
    Dim FromDate As String
    Dim ToDate As String
    Public AircraftId As String
    Dim mLogDetail As String
    Dim mAssemblylist As AssemblyList 'Added By Vikrant On 02-Sept-2014 For All04092014
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mLogMaintenanceActivityList = CType(Session("mLogMaintenanceActivityList"), LogMaintenanceActivityList)
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        AircraftId = CType(Session("AircraftId"), String)
        mAssemblylist = Session("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mLogMaintenanceActivityList") = mLogMaintenanceActivityList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("AircraftId") = AircraftId
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mLogMaintenanceActivityList")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("AircraftId")
        Session.Remove("OpenFromLMA")
        Session.Remove("LogMaintenanceEdit") 'Changed By Utkarsh on 13-Aug-2013 for ALL13082013-2
        Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "1-1-1900", Optional ByVal ToDate As String = "1-1-3300", Optional ByVal MachineID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal Show_100_Records As Boolean = False)
        mLogMaintenanceActivityList = LogMaintenanceActivityList.GetLogMaintenanceActivityList(MachineID, FromDate, ToDate, Show_100_Records, cmbAssembly.SelectedValue.ToString)
        'Set DataSource of the Grid
        dgLogMaintenanceActivityList.DataSource = mLogMaintenanceActivityList
        Session("mLogMaintenanceActivityList") = mLogMaintenanceActivityList
        dgLogMaintenanceActivityList.DataBind()
        lblResult.Text = "As per criteria :" & mLogMaintenanceActivityList.Count & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    GetSession()
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub EditRecord(ByVal mLogID As Guid, ByVal mID As Guid)
        Dim mLog As Log
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
        Dim mMachine As Machine = Machine.GetMachine(mMachineID)
        ''   Session("mLogList") = mLogList
        Session("mLogList") = Nothing
        Session("mMachine") = mMachine
        mLog = Log.GetLog(mLogID)
        mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
        mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
        MarkLog(Util.Action.Edit, "Log Maintenance Activity", mLogDetail, Util.ErrorType.NoError, mLog.ID, EventLogID)
        AircraftId = Session("MachineID")
        Session("OpenFromLMA") = True
        Session("LogMaintenanceEdit") = True 'Changed by By Utkarsh on 13-Aug-2013 for ALL13082013-2
        mLog.LogMaintenances.CurrentIndex = mLog.LogMaintenances.IndexOf(mLog.LogMaintenances(mID))
        Session("mLog") = mLog

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLogMaintenanceActivityWindow", "OpenLogMaintenanceActivityWindow()", True)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfLogMaintenanceActivityList_Ajax.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mLogMaintenanceActivityList")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("AircraftId")
            Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
        End If
    End Sub
    Private Sub SetGrid()
        For j As Integer = 0 To dgLogMaintenanceActivityList.Rows.Count - 1
            If mLogMaintenanceActivityList(j).LogTypeID = 3 Then
                dgLogMaintenanceActivityList.Rows.Item(j).Cells(11).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region "DataFieldBind"
    Private Sub DataFieldBind()

        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)

        If (Not IsDate(FromDate) Or Not IsDate(ToDate)) Or (FromDate = "1/1/1900" Or ToDate = "1/1/2200") Then
            txtFromDate.Text = ""
            txtToDate.Text = ""
        Else
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        End If

        txtFromDate.DataBind()
        txtToDate.DataBind()

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        If mMachineNameValueList.Count <> 0 Then
            If IsNothing(AircraftId) Then AircraftId = mMachineNameValueList(0).ID.ToString Else AircraftId = AircraftId
        Else
            AircraftId = "00000000-0000-0000-0000-000000000000"
        End If
        Session("AircraftId") = AircraftId
        cmbAircraft.DataBind()

        'Added By Vikrant On 02-Sept-2014 For All04092014
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, AircraftId, Today.Date.ToString, "(All)", True)
        cmbAssembly.DataSource = mAssemblylist
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataBind()
        'End

        mLogMaintenanceActivityList = LogMaintenanceActivityList.GetLogMaintenanceActivityList(AircraftId, FromDate, ToDate, True, cmbAssembly.SelectedValue.ToString)
        dgLogMaintenanceActivityList.DataSource = mLogMaintenanceActivityList
        dgLogMaintenanceActivityList.DataBind()
        Session("mLogMaintenanceActivityList") = mLogMaintenanceActivityList

        If mMachineNameValueList.Count > 1 And IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 1 Else cmbAircraft.SelectedValue = AircraftId
        AircraftId = cmbAircraft.SelectedValue
        Session("AircraftId") = AircraftId
        lblResult.Text = "As per criteria :" & mLogMaintenanceActivityList.Count & " Record(s) found."
        'DataBind()
    End Sub


#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfLogMaintenanceActivityList_Ajax.aspx?"
            DataFieldBind()
        End If
        SetGrid()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Session("AircraftId") = cmbAircraft.SelectedValue
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        dgLogMaintenanceActivityList.PageIndex = 0

        If chkShowAll.Checked = True Then
            FindNow(FromDate, ToDate, mMachineID.ToString)
        Else
            FindNow(FromDate, ToDate, mMachineID.ToString, True)
        End If
        SetGrid()
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlResult.Update()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        Session("AircraftId") = cmbAircraft.SelectedValue
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        dgLogMaintenanceActivityList.PageIndex = 0

        'Added By Vikrant On 02-Sept-2014 For All04092014
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, mMachineID.ToString, Today.Date.ToString, "(All)", True)
        cmbAssembly.DataSource = mAssemblylist
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataBind()
        'End

        If chkShowAll.Checked = True Then
            FindNow(FromDate, ToDate, mMachineID.ToString)
        Else
            FindNow(FromDate, ToDate, mMachineID.ToString, True)
        End If
        SetGrid()
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        'upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub
    Private Sub dgLogMaintenanceActivityList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLogMaintenanceActivityList.PageIndexChanging
        dgLogMaintenanceActivityList.PageIndex = e.NewPageIndex
        dgLogMaintenanceActivityList.DataSource = mLogMaintenanceActivityList
        Session("mLogMaintenanceActivityList") = mLogMaintenanceActivityList
        dgLogMaintenanceActivityList.DataBind()
        SetGrid()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Session.Remove("mLog")
        Session.Remove("OpenFromLMA")
        Session.Remove("LogMaintenanceEdit") 'Changed By Utkarsh on 13-Aug-2013 for ALL13082013-2
        Session.Remove("mLogMaintenanceActivityList")

        Response.Redirect("Dashboard.aspx")

    End Sub
    Private Sub dgLogMaintenanceActivityList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLogMaintenanceActivityList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgLogMaintenanceActivityList.PageSize * dgLogMaintenanceActivityList.PageIndex
                Dim mID As Guid = mLogMaintenanceActivityList(Index).LogID
                Dim mLog As Log
                mLog = Log.GetLog(mID)

                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                'Added by Saylee on 8-Apr-2014 for ALL08042014
                If (Not User.IsInRole("LogMaintenanceActivityNew")) Or (Not User.IsInRole("LogMaintenanceActivityEdit")) Then
                    'setObject()
                    SetSession()
                    MarkLog(Util.Action.Edit, "LogMaintenanceActivityList", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

                    Exit Sub
                End If
                If chkShowAll.Checked = True Then
                    FindNow(FromDate, ToDate, New Guid(cmbAircraft.SelectedValue).ToString)
                Else
                    FindNow(FromDate, ToDate, New Guid(cmbAircraft.SelectedValue).ToString, True)
                End If
                EditRecord(mLog.ID, mLogMaintenanceActivityList(Index).ID)
                SetGrid()
                upnlGridView.Update()
                upnlActionBtnTop.Update()
                'upnlActionBtnBottom.Update()
                upnlResult.Update()
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                'Added by Saylee on 8-Apr-2014 for ALL08042014
                If (Not User.IsInRole("LogMaintenanceActivityView")) Then
                    'setObject()
                    SetSession()
                    MarkLog(Util.Action.Edit, "LogMaintenanceActivityList", User.Identity.Name & " is not Authorized User to View " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                '----------------------------------------------------------------------
                Dim Index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgLogMaintenanceActivityList.PageSize * dgLogMaintenanceActivityList.PageIndex
                Dim mLogMaintenance As LogMaintenance
                mLogMaintenance = LogMaintenance.GetLogMaintenance(mLogMaintenanceActivityList(Index).ID)


                dgLogMaintenanceActivityList.DataSource = mLogMaintenanceActivityList
                dgLogMaintenanceActivityList.DataBind()
                SetGrid()
                upnlGridView.Update()
                upnlActionBtnTop.Update()
                'upnlActionBtnBottom.Update()
                upnlResult.Update()

                If mLogMaintenance.ImageSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mLogMaintenance.FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mLogMaintenance.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mLogMaintenance.ImageFile, 0, mLogMaintenance.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path

                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                Else
                    'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfLogList.aspx?BackPage="
                    'msg1.Show()
                End If
                '-----------------------------------------
        End Select
    End Sub
    Private Sub dgLogMaintenanceActivityList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgLogMaintenanceActivityList.Sorting
        mLogMaintenanceActivityList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLogMaintenanceActivityList") = mLogMaintenanceActivityList
        dgLogMaintenanceActivityList.DataSource = mLogMaintenanceActivityList
        dgLogMaintenanceActivityList.DataBind()
        SetGrid()
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsLogMaintenanceActivityList
        Dim ReportName As String = ""
        Dim rpt As LogMaintenanceActivityList
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
        Dim mCompanyDetail As New CompanyDetail

        If (Not User.IsInRole("LogMaintenanceActivityPrint")) Then  'Added by Saylee on 8-Apr-2014 for ALL08042014
            'setObject()
            SetSession()
            MarkLog(Util.Action.Edit, "LogMaintenanceActivityList", User.Identity.Name & " is not Authorized User to Print " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        myReport = New crLogMaintenanceActivityList
        rpt = LogMaintenanceActivityList.GetLogMaintenanceActivityList(mMachineID.ToString, FromDate, ToDate, IIf(chkShowAll.Checked = True, False, True), cmbAssembly.SelectedValue.ToString)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1251)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Log Maintenance Activity List Report", cmbAircraft.SelectedItem.Text, New SmartDate(txtFromDate.Text.ToString).FormattedText,
        New SmartDate(txtToDate.Text.ToString).FormattedText, cmbAssembly.SelectedItem.Text, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "",
        AppSettings("Logo"))

        If IsExcel = False Then     'PDF format
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, rpt)
            da.Fill(ds, mrptImage)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Else
            ds.Clear()
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            Dim columnToRemove2 As String() = {"ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion",
                                               "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8",
                                               "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15",
                                               "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22",
                                               "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Aircraft"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Assembly"
            End If

            Dim LogMaintenanceActivityListcolumnToRemove As String() = {"ID", "LogID", "LogPageNoFormatted", "LogNo", "EmployeeID", "SrNo", "ImageSize",
                                                                        "SrNoForMachine", "LogTypeID", "LogTypeName", "IsTLP", "LogText"}

            For i As Integer = 0 To LogMaintenanceActivityListcolumnToRemove.Length - 1
                If ds.Tables("LogMaintenanceActivityList").Columns.Contains(LogMaintenanceActivityListcolumnToRemove(i)) Then
                    ds.Tables("LogMaintenanceActivityList").Columns.Remove(LogMaintenanceActivityListcolumnToRemove(i))
                End If
            Next
            If ds.Tables("LogMaintenanceActivityList").Columns.Contains("LogDate") Then
                ds.Tables("LogMaintenanceActivityList").Columns("LogDate").ColumnName = "Date"
            End If
            If ds.Tables("LogMaintenanceActivityList").Columns.Contains("LogTextNo") Then
                ds.Tables("LogMaintenanceActivityList").Columns("LogTextNo").ColumnName = "Log No."
            End If
            If ds.Tables("LogMaintenanceActivityList").Columns.Contains("LogPageNo") Then
                ds.Tables("LogMaintenanceActivityList").Columns("LogPageNo").ColumnName = "Log Page No."
            End If
            If ds.Tables("LogMaintenanceActivityList").Columns.Contains("MaintenanceActivity") Then
                ds.Tables("LogMaintenanceActivityList").Columns("MaintenanceActivity").ColumnName = "Activity"
            End If
            If ds.Tables("LogMaintenanceActivityList").Columns.Contains("NRCWONO") Then
                ds.Tables("LogMaintenanceActivityList").Columns("NRCWONO").ColumnName = "NRC/Wo No."
            End If
            If ds.Tables("LogMaintenanceActivityList").Columns.Contains("EmployeeName") Then
                ds.Tables("LogMaintenanceActivityList").Columns("EmployeeName").ColumnName = "Done By"
            End If
            If ds.Tables("LogMaintenanceActivityList").Columns.Contains("ClosedDate") Then
                ds.Tables("LogMaintenanceActivityList").Columns("ClosedDate").ColumnName = "Closed Date"
            End If

            ds.Tables("LogMaintenanceActivityList").Columns("Date").SetOrdinal(0)
            ds.Tables("LogMaintenanceActivityList").Columns("Log No.").SetOrdinal(1)
            ds.Tables("LogMaintenanceActivityList").Columns("Log Page No.").SetOrdinal(2)
            ds.Tables("LogMaintenanceActivityList").Columns("Activity").SetOrdinal(3)
            ds.Tables("LogMaintenanceActivityList").Columns("NRC/Wo No.").SetOrdinal(4)
            ds.Tables("LogMaintenanceActivityList").Columns("Place").SetOrdinal(5)
            ds.Tables("LogMaintenanceActivityList").Columns("Done By").SetOrdinal(6)
            ds.Tables("LogMaintenanceActivityList").Columns("Closed Date").SetOrdinal(7)

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("LogMaintenanceActivityList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("LogMaintenanceActivityList").TableName = "Log Maintenance Activity List"
			Session("ExcelFileName") = "Log Maintenance Activity List"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Edit, "Log Maintenance Activity", "Export To excel " + mLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
        SetReport(False)
        dgLogMaintenanceActivityList.DataSource = mLogMaintenanceActivityList
        dgLogMaintenanceActivityList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExportTop.Click
        SetReport(True)
        dgLogMaintenanceActivityList.DataSource = mLogMaintenanceActivityList
        dgLogMaintenanceActivityList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnLogMaintenanceActivity_Click(sender As Object, e As System.EventArgs) Handles hdnBtnLogMaintenanceActivity.Click
        'DataFieldBind()
        If chkShowAll.Checked = True Then
            FindNow(FromDate, ToDate, New Guid(cmbAircraft.SelectedValue).ToString)
        Else
            FindNow(FromDate, ToDate, New Guid(cmbAircraft.SelectedValue).ToString, True)
        End If
        SetGrid()
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        'upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub
#End Region
End Class