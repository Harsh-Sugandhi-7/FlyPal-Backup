Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.ModelListAutoComplete
Public Class wfrptAircraftMaitenanceProgram_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mModelList As ModelList
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataBind()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As AMPList
        Dim objsearch As rptSearchingCriteria
        Dim ds As New dsAMP
        Dim mCompanyDetail As New CompanyDetail
        'SetCustomerID()
        'Dim mModelID As Guid = Guid.Empty
        'If txtModelList.Text.Trim <> "" Then
        '    mModelID = mModelList.Item(txtModelList.Text.Trim).ID
        'End If
        'Dim mSupplierID As Guid = mCustomerList.Item(txtSupplierList.Text.Trim).ID
        'SetValues()
        rpt = AMPList.GetList(txtModelNo.Text.Trim, chkIsService.Checked, chkIsInspection.Checked, ChkDirective.Checked)

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "AIRCRAFT MAINTENANCE PROGRAM", txtModelNo.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:="", SearchStr10:=AppSettings("Logo"))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, "ReportData", Report)
        da.Fill(ds, "AMPList", rpt)

        Dim columnToRemove2 As String() = {"ID", "SearchStr2", "SearchStr3", "SearchStr4", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ShortName"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
            End If
        Next

        Dim columnToRemove As String() = {"MaintID", "MaintActivityTypeID", "PeriodUnitID", "PeriodID", "FrequencyValue", "MonitorTypeID"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("AMPList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("AMPList").Columns.Remove(columnToRemove(i))
            End If
        Next
        If ds.Tables("AMPList").Columns.Contains("TaskReference") Then
            'ds.Tables("AMPList").Columns("TaskReference").ColumnName = "TASK REFERENCE"
            ds.Tables("AMPList").Columns("TaskReference").ColumnName = "TASK NO"
        End If
        If ds.Tables("AMPList").Columns.Contains("Sources") Then
            ds.Tables("AMPList").Columns("Sources").ColumnName = "SOURCES"
        End If
        If ds.Tables("AMPList").Columns.Contains("MaintActivityTypeName") Then
            ds.Tables("AMPList").Columns("MaintActivityTypeName").ColumnName = "Type"
        End If
        If ds.Tables("AMPList").Columns.Contains("Description") Then
            ds.Tables("AMPList").Columns("Description").ColumnName = "DESCRIPTION"
        End If
        'If ds.Tables("AMPList").Columns.Contains("FrequencyValue") Then
        '    ds.Tables("AMPList").Columns("FrequencyValue").ColumnName = "THRESHOLD INTERVAL SAMPLE"
        'End If
        If ds.Tables("AMPList").Columns.Contains("ThresholdAccordingToTypeIDForExcel") Then
            ds.Tables("AMPList").Columns("ThresholdAccordingToTypeIDForExcel").ColumnName = "THRESHOLD INTERVAL SAMPLE"
        End If

        If ds.Tables("AMPList").Columns.Contains("FrequencyAccordingToTypeIDForExcel") Then
            ds.Tables("AMPList").Columns("FrequencyAccordingToTypeIDForExcel").ColumnName = "FREQUENCY INTERVAL SAMPLE"
        End If
        If ds.Tables("AMPList").Columns.Contains("JobProcedure") Then
            ds.Tables("AMPList").Columns("JobProcedure").ColumnName = "JOB PROCEDURE"
        End If
        If ds.Tables("AMPList").Columns.Contains("Zone") Then
            ds.Tables("AMPList").Columns("Zone").ColumnName = "ZONE"
        End If
        If ds.Tables("AMPList").Columns.Contains("Access") Then
            ds.Tables("AMPList").Columns("Access").ColumnName = "ACCESS"
        End If
        If ds.Tables("AMPList").Columns.Contains("RequiredManHours") Then
            ds.Tables("AMPList").Columns("RequiredManHours").ColumnName = "MH"
        End If
        If ds.Tables("AMPList").Columns.Contains("Note") Then
            ds.Tables("AMPList").Columns("Note").ColumnName = "Note"
        End If



        ds.Tables("AMPList").Columns("TASK NO").SetOrdinal(0)
        ds.Tables("AMPList").Columns("Type").SetOrdinal(1)
        ds.Tables("AMPList").Columns("SOURCES").SetOrdinal(2)
        ds.Tables("AMPList").Columns("DESCRIPTION").SetOrdinal(3)
        ds.Tables("AMPList").Columns("THRESHOLD INTERVAL SAMPLE").SetOrdinal(4)
        ds.Tables("AMPList").Columns("FREQUENCY INTERVAL SAMPLE").SetOrdinal(5)
        ds.Tables("AMPList").Columns("JOB PROCEDURE").SetOrdinal(6)
        ds.Tables("AMPList").Columns("ZONE").SetOrdinal(7)
        ds.Tables("AMPList").Columns("ACCESS").SetOrdinal(8)
        ds.Tables("AMPList").Columns("MH").SetOrdinal(9)
        ds.Tables("AMPList").Columns("Note").SetOrdinal(10)
        ds.Tables("AMPList").Columns("EFFECTIVITY").SetOrdinal(11)



        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Tables("ReportData").Columns("ReportDate").ColumnName = "Report Date"
        dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "Model"

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Merge(ds.Tables("AMPList"))
        dsNew.Tables("AMPList").TableName = "AIRCRAFT MAINTENANCE PROGRAM"
		Session("ExcelFileName") = "AIRCRAFT MAINTENANCE PROGRAM"
		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "AircraftMaintenanceProgram", "Export To excel " + "Model No." + txtModelNo.Text, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region " Service Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 0)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
                    Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                    Select c.Name).Take(count).ToList
        End If
    End Function

#End Region


End Class