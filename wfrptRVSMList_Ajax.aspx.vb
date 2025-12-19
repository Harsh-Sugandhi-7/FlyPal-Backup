'Added by Utkarsh on 22-Jan-2014
Imports System.Linq
Imports System.Collections.Generic

Public Class wfrptRVSMList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    Dim FromDate As String = "1-1-1900"
    Dim ToDate As String = "1-1-2200"
    Dim Aircraft As String = ""
    Dim Count As Integer = 0
    Dim AircraftIndex As Integer
    Dim Assembly1 As String
    Dim EventLogDetail As String
#End Region
#Region " Business Method "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub PageInitialization()
        txtFromDate.Text = CDate(Today.Date).AddMonths(-1).ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
    End Sub
    Private Sub ResetValues()
        ToDate = Format(CDate(Today.Date).Year, "")
    End Sub
    Private Sub SetValues()
        If txtToDate.Text.Trim = "" Or txtFromDate.Text.Trim = "" Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            ToDate = txtToDate.Text.Trim
            FromDate = txtFromDate.Text.Trim
            lblDateRangeFrom.Text = "From Date : " & FromDate & " To Date : " & ToDate
        End If

        If cmbAircraft.SelectedIndex = 0 Then       'Aircraft
            Aircraft = ""
            lblAircraft.Text = "Aircraft : "
        Else
            Aircraft = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).RegNo
            lblAircraft.Text = "Aircraft : " & Aircraft
        End If

        EventLogDetail = lblDateRangeFrom.Text + ", " + lblAircraft.Text
    End Sub
    Private Sub ControlVisibility()
        lblSummary.Visible = False
        lblDateRangeFrom.Visible = False
        lblAircraft.Visible = False
        upnlCriteria.Update()
    End Sub
    Private Sub ControlVisibility1()
        lblSummary.Visible = True
        lblDateRangeFrom.Visible = True
        lblAircraft.Visible = True
        upnlCriteria.Update()
    End Sub
    Public Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As RVSMReportList
        Dim ds As New dsLogParameter
        Dim mCompanyDetail As New CompanyDetail
        'SetCustomerID()
        'Dim mModelID As Guid = Guid.Empty
        'If txtModelList.Text.Trim <> "" Then
        '    mModelID = mModelList.Item(txtModelList.Text.Trim).ID
        'End If
        'Dim mSupplierID As Guid = mCustomerList.Item(txtSupplierList.Text.Trim).ID
        'SetValues()
        rpt = RVSMReportList.GetList(New Guid(cmbAircraft.SelectedValue), txtFromDate.Text, txtToDate.Text)

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "RVSM Report", cmbAircraft.SelectedItem.ToString, txtFromDate.Text, txtToDate.Text, "", "", AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:="", SearchStr10:=AppSettings("Logo"))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, "ReportData", Report)
        da.Fill(ds, "RVSMReportList", rpt)

        Dim columnToRemove2 As String() = {"ID", "SearchStr4", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ShortName"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
            End If
        Next

        Dim columnToRemove As String() = {"ParameterType", "ParameterName", "DateFormatString"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("RVSMReportList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("RVSMReportList").Columns.Remove(columnToRemove(i))
            End If
        Next
        If ds.Tables("RVSMReportList").Columns.Contains("LogDate") Then
            ds.Tables("RVSMReportList").Columns("LogDate").ColumnName = "DATE"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("DeparturePlaceCode") Then
            ds.Tables("RVSMReportList").Columns("DeparturePlaceCode").ColumnName = "FROM STN"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("ArrivalPlaceCode") Then
            ds.Tables("RVSMReportList").Columns("ArrivalPlaceCode").ColumnName = "TO STN"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("FLTLVLValue") Then
            ds.Tables("RVSMReportList").Columns("FLTLVLValue").ColumnName = "FLT LVL"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("XPDRValue") Then
            ds.Tables("RVSMReportList").Columns("XPDRValue").ColumnName = "XPDR"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("ALTP1Value") Then
            ds.Tables("RVSMReportList").Columns("ALTP1Value").ColumnName = "ALT P1"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("ALTP2Value") Then
            ds.Tables("RVSMReportList").Columns("ALTP2Value").ColumnName = "ALT P2"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("STDBYALTValue") Then
            ds.Tables("RVSMReportList").Columns("STDBYALTValue").ColumnName = "STDBY ALT"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("ASEValue1") Then
            ds.Tables("RVSMReportList").Columns("ASEValue1").ColumnName = "P1-P2"
        End If

        If ds.Tables("RVSMReportList").Columns.Contains("ASEValue2") Then
            ds.Tables("RVSMReportList").Columns("ASEValue2").ColumnName = "P1-STANDBY"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("ASEValue3") Then
            ds.Tables("RVSMReportList").Columns("ASEValue3").ColumnName = "P2-STANDBY"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("AADValue1") Then
            ds.Tables("RVSMReportList").Columns("AADValue1").ColumnName = "ATC-FL"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("TVEValue1") Then
            ds.Tables("RVSMReportList").Columns("TVEValue1").ColumnName = "P1-FL"
        End If
        If ds.Tables("RVSMReportList").Columns.Contains("TVEValue2") Then
            ds.Tables("RVSMReportList").Columns("TVEValue2").ColumnName = "P2-FL"
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Tables("ReportData").Columns("ReportDate").ColumnName = "Report Date"
        dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "Aircraft"
        dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "From Date"
        dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "To Date"

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Merge(ds.Tables("RVSMReportList"))
        dsNew.Tables("RVSMReportList").TableName = "RVSM Report"
		Session("ExcelFileName") = "RVSM Report"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "RVSMReport", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptRVSMList_Ajax.aspx" Then
            RemoveSessions()
        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DatafieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.Date.ToString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptRVSMList_Ajax.aspx"
            If cmbAircraft.Enabled = True Then
                cmbAircraft.Focus()
            End If
            DatafieldBind()
            PageInitialization()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid() Then
            ControlVisibility1()
            SetValues()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid() Then
            SetValues()
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region


End Class