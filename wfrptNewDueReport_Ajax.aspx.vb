Imports System.Linq
Imports System.Collections.Generic

Public Class wfrptNewDueReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mInvoiceItemList As InvoiceItemList
    Public mCRate As Decimal
    Public mrptDueReport As rptDueReportForOnlyDueReport
    Public mInvoiceID As Guid
    Public mInvoiceItemID As Guid

    Dim PartNo As String
    Dim Location As String
    Dim SearchIndex1 As String
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid
    Public mIsReturnFromOHRepair As Boolean
    Public mMachineNameValueList As MachineNameValueList
    Public mDueLimits As DueLimits
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mrptDueReport = CType(Session("mrptDueReport"), rptDueReportForOnlyDueReport)
        mMachineNameValueList = Session("mMachineNameValueList")
        mDueLimits = CType(Session("mDueLimits"), DueLimits)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptDueReport")
        Session.Remove("mDueLimits")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptNewDueReport_Ajax.aspx" Then
            Session.Remove("mrptDueReport")
            Session.Remove("mDueLimits")
            Session.Remove("mMachineNameValueList")
        End If
        
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        Location = ""
    End Sub
    Private Sub GetDueLimits()
        mDueLimits = DueLimits.GetDueLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdvDuePeriodLimits.DataSource = mDueLimits
        gdvDuePeriodLimits.DataBind()
        Session("mDueLimits") = mDueLimits
    End Sub
    Private Sub FindNow()
        GetDueLimits()
        mrptDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString, IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.ToString, ""))
        mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
        'mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Descending)
        Session("mrptDueReport") = mrptDueReport
        BindGrid()
    End Sub
    Private Sub BindGrid()
        dgPartSearch.DataSource = mrptDueReport
        dgPartSearch.DataBind()
        lblResult.Text = "List of Parts: " & mrptDueReport.Count.ToString & " Record(s) found. "

    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            'mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text) 'Added by Saylee on 12-Nov-2012
        Next i
        mDueLimits.Save()
        Session("mDueLimits") = mDueLimits
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim serchstr7 As String
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsMonthlyMaintenanceReport
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = ""
        Dim searchstr As String
        Dim mtmpMachineList As tmpMachineList
        Dim ReportStatusList As rptStatusList
        Dim ToShowAssemblyCurrentValues As String = "False"
        Dim PeriodColumnsForExportToExcel As New List(Of String)

        ReportStatusList = New rptStatusList

        For Each mDueLimit As DueLimit In mDueLimits
            If CDec(Val(mDueLimit.PeriodLimit)) >= 0 Then
                If searchstr = "" Then
                    searchstr = "For Next" & " " & searchstr & " " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                Else
                    searchstr = searchstr & ", " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                End If
            End If
        Next
        searchstr = searchstr & ", " & "As On Date:" & Today.Date.ToString(AppSettings("DateFormat"))
        myReport = New crptNewDueReport
        ReportName = "New Due Report"

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, ReportName, searchstr, ToShowAssemblyCurrentValues, cmbAircraft.SelectedItem.ToString, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", serchstr7, "", "", AppSettings("Logo"))    'Changed By Utkarsh For Report Logo.

        ds.Clear()
        If IsExcel Then
            da.Fill(ds, "ExcelrptDueReport", mrptDueReport)
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)

            Dim columnToRemove As String() = {"HourType", "AssemblyMonitorInspStatusID", "StatusMasterID", "SrNo", "PartName", "CompSerialNo", "Position",
                                              "MasterCode", "Reference", "CodeType", "Type", "SinceNewForGrid", "EstimatedDate", "Freq3ForGrid", "RegNo",
                                              "Assembly", "Zone", "Area", "IsRII", "DataType", "JobDescription", "MaintenanceEvent", "Remark", "PartDescription",
                                              "AssemblyTypeID", "StartDate", "SinceNewTSNCSN", "DueAsof2", "ATAID", "AssemblyModel", "AssemblySerialNo",
                                              "AssemblyPositionInComp", "LogBook", "AssemblyTypeName", "Number", "EstimatedHoursforGrid", "EstimatedHours", "OnAssemblyOrComponent",
                                               "Description", "JobDescriptionDetail", "RemainingValueForSorting", "DueStatus", "IsLater", "ActivityTypeID",
                                              "ActivityTypeName", "AssemblyCompID", "mID", "ID", "WONumber", "DoneAt2ForGrid", "DueAsOf2ForGrid",
                                              "ElapsedValue", "RemainingTime2ForGrid", "AircraftAssemblyDetails", "DescDetails", "PeriodID", "PeriodUnitID",
                                              "AssemblyStatusPeriodID", "LogID", "IsMaster", "Code_Desc", "ModelSerialNo", "CompInfo", "ModelMonitorInspID",
                                              "PlannedWODetails", "Note", "MonitorType", "ATACode", "FrequencyValue", "DoneOnValue", "DueOnValue", "RemainingValue",
                                              "AssemblyCurrentValueByAirFrame", "ExtensionValueFormatted", "TaskNo", "DueOnValueAsofAssembly", "DueOnValueAsofAssemblyExcel",
                                              "MonitorTypeID", "IsCompleted", "SourceDoc", "Skill", "SkillID", "B22CurrentValue", "B22LifeLimit", "B22RemainingValue", "B22IsCurrentThrust",
                                              "B24CurrentValue", "B24LifeLimit", "B24RemainingValue", "B24IsCurrentThrust", "B26CurrentValue", "B26LifeLimit", "B26IsCurrentThrust",
                                              "B26RemainingValue", "IsThrustMonitoringComp", "PlannedDetails", "PeriodIDWithDecValue", "Created", "IsAttachmentAdded", "SkillCode"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExcelrptDueReport").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelrptDueReport").Columns.Remove(columnToRemove(i))
                End If
            Next

            'set Column Sequence
            ds.Tables("ExcelrptDueReport").Columns("AircraftAssemblyDetailsExcel").SetOrdinal(0)
            ds.Tables("ExcelrptDueReport").Columns("Code").SetOrdinal(1)
            ds.Tables("ExcelrptDueReport").Columns("ATAChapter").SetOrdinal(2)
            ds.Tables("ExcelrptDueReport").Columns("DescDetailsExcel").SetOrdinal(3)
            ds.Tables("ExcelrptDueReport").Columns("DoneOnDate").SetOrdinal(4)
            ds.Tables("ExcelrptDueReport").Columns("DoneWONO").SetOrdinal(5)
            ds.Tables("ExcelrptDueReport").Columns("FrequencyExcel").SetOrdinal(6)
            ds.Tables("ExcelrptDueReport").Columns("DoneAt2ForGridExcel").SetOrdinal(7)
            ds.Tables("ExcelrptDueReport").Columns("ElapsedValueExcel").SetOrdinal(8)
            ds.Tables("ExcelrptDueReport").Columns("DueAsOf2ForGridExcel").SetOrdinal(9)
            ds.Tables("ExcelrptDueReport").Columns("AssemblyCurrentValueByAirFrameExcel").SetOrdinal(10)
            ds.Tables("ExcelrptDueReport").Columns("RemainingTime2ForGridExcel").SetOrdinal(11)
            ds.Tables("ExcelrptDueReport").Columns("WONumberExcel").SetOrdinal(12)

            If ds.Tables("ExcelrptDueReport").Columns.Contains("AssemblyCurrentValueByAirFrameExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("AssemblyCurrentValueByAirFrameExcel").ColumnName = "Due as of Airframe"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("ElapsedValueExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("ElapsedValueExcel").ColumnName = "Elapsed"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("RemainingTime2ForGridExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("RemainingTime2ForGridExcel").ColumnName = "Remaining"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("DueAsOf2ForGridExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("DueAsOf2ForGridExcel").ColumnName = "Due On"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("DoneOnDate") Then
                ds.Tables("ExcelrptDueReport").Columns("DoneOnDate").ColumnName = "Done At"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("WONumberExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("WONumberExcel").ColumnName = "WO.No."
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("ATAChapter") Then
                ds.Tables("ExcelrptDueReport").Columns("ATAChapter").ColumnName = "ATA"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("Code") Then
                ds.Tables("ExcelrptDueReport").Columns("Code").ColumnName = "Type"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("FrequencyExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("FrequencyExcel").ColumnName = "Frequency"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("DoneAt2ForGridExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("DoneAt2ForGridExcel").ColumnName = "Done On"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("AircraftAssemblyDetailsExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("AircraftAssemblyDetailsExcel").ColumnName = "Assembly Info."
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("DescDetailsExcel") Then
                ds.Tables("ExcelrptDueReport").Columns("DescDetailsExcel").ColumnName = "Description"
            End If
            If ds.Tables("ExcelrptDueReport").Columns.Contains("DoneWONO") Then
                ds.Tables("ExcelrptDueReport").Columns("DoneWONO").ColumnName = "Done WO NO"
            End If

            Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite",
                                                           "ReportName", "SearchStr2", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "ProductVersion",
                                                           "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15",
                                                           "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23",
                                                           "SearchStr24", "SearchStr25", "ShortName", "SearchStr26", "SearchStr27", "SearchStr28",
                                                           "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34",
                                                           "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40",
                                                           "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46",
                                                           "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ApprovalNo"}

            For i As Integer = 0 To columnToRemoveCriteria.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Due Limit"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
            End If
            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ExcelrptDueReport"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
			dsNew.Tables("ExcelrptDueReport").TableName = "Express Due Report"
			Session("DataTableToBeFormattedForExportToExcel") = "Express Due Report"
			Session("ExcelFileName") = "Express Due Report"
			PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "WO.No.", "Done On", "Due On", "Elapsed", "Remaining", "Assembly Info.", "Description", "Due as of Airframe"})
			Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "New Due Report Excel", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else

            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, mrptDueReport)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            da.Fill(ds, ReportStatusList)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If




    End Sub
    Private Sub Enability()
        btnPrint.Enabled = IIf(mrptDueReport.Count > 0, True, False)
        btnExport.Enabled = IIf(mrptDueReport.Count > 0, True, False)
        upnlBtns1.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mrptDueReport = rptDueReportForOnlyDueReport.NewList
        'mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
        Session("mrptDueReport") = mrptDueReport
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        GetDueLimits()
        'mrptDueReport = Nothing
        dgPartSearch.DataSource = mrptDueReport
        dgPartSearch.DataBind()
        lblResult.Text = "List of Parts: 0 Record(s) found. "
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptNewDueReport_Ajax.aspx"
            DataFieldBind()
            Enability()
        End If
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Dim StartDateTime, EndDateTime As DateTime
        StartDateTime = DateTime.Now
        dgPartSearch.PageIndex = 0
        'GetDueLimits()
        SetGridObject()
        FindNow()
        Enability()
        EndDateTime = DateTime.Now
        MarkLog(Util.Action.View, "New Due Report", "Aircraft: " + cmbAircraft.SelectedItem.ToString + ", Time Taken in Seconds:" + (EndDateTime - StartDateTime).Seconds.ToString, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose1.Click, btnClose.Click
        MarkLog(Util.Action.Close, "New Due Report", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    'Added By Prashant 22-June-2009 for grid sorting 
    Private Sub dgPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartSearch.Sorting
        mrptDueReport.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mrptDueReport") = mrptDueReport
        BindGrid()
    End Sub

#End Region



    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        SetReport()
        MarkLog(Util.Action.Print, "New Due Report Print", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub

    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        GetDueLimits()
        mrptDueReport = rptDueReportForOnlyDueReport.NewList
        Session("mrptDueReport") = mrptDueReport
        BindGrid()
        Enability()
        upnlGrid.Update()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMonthlyMaintenanceReport
        SetReport(True)
        
    End Sub
End Class