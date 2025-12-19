Public Class wfrptPreMatureFailureReport
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Dim mDateSearchingCriteria As String = String.Empty
    'Added by Abhishek on 3-OCT-2017
    Dim ds As New dsPreMatureFailure
    Dim da As New CSLA.Data.ObjectAdapter
    Dim obj As rptPreMatureFailure
    Dim mCompanyDetail As New CompanyDetail

#End Region

#Region " Business Methods "

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            lblFromDate.Text = "From Date :" & New SmartDate(txtFromDate.Text).FormattedText
            lblToDate.Text = "To Date     :" & New SmartDate(txtToDate.Text).FormattedText
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim ds As New dsPreMatureFailure
            Dim da As New CSLA.Data.ObjectAdapter
            Dim obj As rptPreMatureFailure
            Dim mCompanyDetail As New CompanyDetail

            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text

            myReport = New crptPreMatureFailure

            obj = rptPreMatureFailure.GetrptPreMatureFailure(FromDate, ToDate)
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, _
            mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
            mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
             "Pre-Mature Failure", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
            If obj.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 725)
            End If
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, obj)
            da.Fill(ds, mrptImage)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            lblFromDate.Text = "From Date :" & New SmartDate(txtFromDate.Text).FormattedText
            lblToDate.Text = "To Date     :" & New SmartDate(txtToDate.Text).FormattedText
            upnlSerachCriteria.Update()
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            mDateSearchingCriteria = lblFromDate.Text.Trim + ", " + lblToDate.Text.Trim
            MarkLog(Util.Action.Print, "Pre-MatureFailure", mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            upnlValidations.Update()
        End If
    End Sub
#End Region
    'Added by Abhishek on 3-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            ' MyReport = New crptPreMatureFailure
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text

            obj = rptPreMatureFailure.GetrptPreMatureFailure(FromDate, ToDate)
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, _
            mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
             mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
             "Pre-Mature Failure", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
            If obj.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 725)
            End If
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "rptPreMatureFailure", obj)
            Dim columnToRemove1 As String() = {"ID", "ReportDate", "SearchStr1", "SearchStr2", "CompanyName", "Address", "Tel1", "Tel2", "ReportName", "Fax", "Email", "WebSite", "IssueDate", "ReceiptDate", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "ProductVersion", "SearchStr3", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("rptPreMatureFailure").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("rptPreMatureFailure").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ReportName", "CurrencyName", "CurrencySymbol", "ShortName", "SINote", "ProductVersion", "SearchStr3", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next
            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Date To"
            End If
            lblFromDate.Text = "From Date :" & New SmartDate(txtFromDate.Text).FormattedText
            lblToDate.Text = "To Date     :" & New SmartDate(txtToDate.Text).FormattedText
            ' upnlSerachCriteria.Update()

            If ds.Tables("rptPreMatureFailure").Columns.Contains("IssueDateFormatted") Then
                ds.Tables("rptPreMatureFailure").Columns("IssueDateFormatted").ColumnName = "Issue Date"
            End If
            If ds.Tables("rptPreMatureFailure").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("rptPreMatureFailure").Columns("ReceiptDateFormatted").ColumnName = "Receive Date"
            End If

            If ds.Tables("rptPreMatureFailure").Columns.Contains("NoOfDays") Then
                ds.Tables("rptPreMatureFailure").Columns("NoOfDays").ColumnName = "No.Of Days"
            End If
            If ds.Tables("rptPreMatureFailure").Columns.Contains("SerialNo") Then
                ds.Tables("rptPreMatureFailure").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("rptPreMatureFailure").Columns.Contains("RegNo") Then
                ds.Tables("rptPreMatureFailure").Columns("RegNo").ColumnName = "Reg.No."
            End If
            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("rptPreMatureFailure"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("rptPreMatureFailure").TableName = "PreMature Failure"
			Session("ExcelFileName") = "PreMature Failure"

			Session("dsNew") = dsNew
            'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
            mDateSearchingCriteria = lblFromDate.Text.Trim + ", " + lblToDate.Text.Trim
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "Pre-MatureFailure", "Export To Excel " + mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

    End Sub
End Class