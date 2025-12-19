Public Class wfrptCurddlingReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public StartDate As String
    Public PartNo As String = ""
    Public Description As String = ""
    Dim EventLogID As Guid 'Added by Prashant
    Dim mCurddlingReportSearchingCriteria As String = String.Empty

    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteria
    Dim rpt As rptCurddlingReport
    Dim ds As New dsCurdding



#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility()
        lblDateRange.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtDate.Text) Then
            StartDate = Today.Date
        Else
            StartDate = New SmartDate(txtDate.Text).FormattedText
        End If
        lblDateRange.Text = "Date: " & StartDate
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")  'Shweta
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "") 'Shweta
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        mCurddlingReportSearchingCriteria = lblDateRange.Text.Trim + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptCurddlingReport
        Dim ds As New dsCurdding
        Dim ReportDetails As New rptStatusList
        SetValues()
        myReport = New crptCurddingReport
        rpt = rptCurddlingReport.GetCurddlingReport(StartDate, PartNo, Description)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), StartDate, "", PartNo, "", "", "", "", "", "", "", Description, "", 0, "", "", "", AppSettings("Logo"))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 707)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 16-Feb-2012
        da.Fill(ds, rpt)
        da.Fill(ds, objsearch)
        da.Fill(ds, mrptImage) 'Added by Shweta on 16-Feb-2012
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "Curddling", mCurddlingReportSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
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
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            RemoveSession()
            txtDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblDateRange.Visible = True
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()

            rpt = rptCurddlingReport.GetCurddlingReport(StartDate, PartNo, Description)
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), StartDate, "", PartNo, "", "", "", "", "", "", "", Description, "", 0, "", "", "", AppSettings("Logo"))
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 707)
            End If
            ds.Clear()         
            da.Fill(ds, "ExcelrptCurddlingReport", rpt)
            da.Fill(ds, "rptSearchingCriteria", objsearch)

            Dim columnToRemove1 As String() = {"ExpiryDate", "ExpQtrs", "ExpYear", "ExpiryMonths", "ExpQtrYear", "ExpiryQuaters"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptCurddlingReport").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptCurddlingReport").Columns.Remove(columnToRemove1(i))
                End If
            Next


            Dim columnToRemove2 As String() = {"ReportName", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "CompanyName", "FromDate", "ToDate", "SupplierName", "BranchName", "Category", "Nomenclature", "Store", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "WorkShop", "WorkOrderText", "WorkOrderNo", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "ShortName"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next
            If ds.Tables("rptSearchingCriteria").Columns.Contains("SearchStr1") Then
                ds.Tables("rptSearchingCriteria").Columns("SearchStr1").ColumnName = "As On Date"
            End If

            If ds.Tables("rptSearchingCriteria").Columns.Contains("PartNo") Then
                ds.Tables("rptSearchingCriteria").Columns("PartNo").ColumnName = "Part Number"
            End If
         

            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("ExcelExpiryDatesQtrs") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("ExcelExpiryDatesQtrs").ColumnName = "Expiry Date/Qtrs"
            End If
            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("PartName") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("PartName").ColumnName = "Part Number"
            End If

            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("PartDescription") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("PartDescription").ColumnName = "Description"
            End If

            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("StockBalanceQty") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("StockBalanceQty").ColumnName = "Stock Qty."
            End If

            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("ReceiptNo") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("ReceiptNo").ColumnName = "Receipt No."
            End If
            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("RelNoteNo") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("RelNoteNo").ColumnName = "Rel.Note.No."
            End If
            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("SerialNo") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("Location") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("Location").ColumnName = "Store-Location."
            End If
            If ds.Tables("ExcelrptCurddlingReport").Columns.Contains("BatchNo") Then
                ds.Tables("ExcelrptCurddlingReport").Columns("BatchNo").ColumnName = "Batch No."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelrptCurddlingReport"))

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptCurddlingReport").TableName = "Curdling Report"
			Session("ExcelFileName") = "Curdling Report"
			Session("dsNew") = dsNew
            'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "Curddling", "Export To excel " + mCurddlingReportSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
End Class