'Created : Saylee
'Dates   : 3-Feb-2014
Imports System.Collections.Generic
Imports Flypal.ItemListAutoComplete
Imports System.Linq
Public Class wfrptSearchPendingRequisitionNew_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim BlankFromDate As String = ""
    Dim ToDate As String
    Dim BlankToDate As String = ""
    Dim PartNo As String
    Dim Description As String

    Dim mSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    'Added by Abhishek on 14-SEP-2017
    Dim mCompanyDetail As New CompanyDetail
    Dim da As New CSLA.Data.ObjectAdapter
    Dim rpt As rptPendingRequisitionNewList
    Dim dsPenOrd As New dsPendingRequisitionNew
    Dim PeriodColumnsForExportToExcel As New List(Of String)
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
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
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        lblDateRangeFrom.Visible = False
        'lblToDate.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            BlankFromDate = ""
            BlankToDate = ""
            lblDateRangeFrom.Text = "Date Range: All"
        Else
            FromDate = txtFromDate.Text.ToString
            BlankFromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            BlankToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range: " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If

        'Added By Vikrant On 28-Nov-2012 For ALL28112012
        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description
        'End

        lblPartNo.Text = "Part No.: " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description: " & IIf(Description <> "", Description, "All")

        mSearchingCriteria = lblDateRangeFrom.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsForExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As rptPendingRequisitionNewList
        Dim dsPenOrd As New dsPendingRequisitionNew
        myReport = New crptPendingRequisitionNewList
        GetSession()
        SetValues()

        rpt = rptPendingRequisitionNewList.GetrptPendingRequisitionNewList(FromDate, ToDate, PartNo, Description, cmbRequisition.SelectedValue, _
                                                                           IIf(cmbType.Visible, cmbType.SelectedValue, 0), rbPendingForPurchase.Checked, _
                                                                           IsExchangePurchase:=chkExchangePurchase.Checked)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf rpt.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1228)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, _
                                     mCompanyDetail.Email, mCompanyDetail.WebSite, IIf(rbPendingForIssue.Checked, "Pending Requisitions For Issue", "Pending Requisitions For Purchase"), _
                                     New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, PartNo, Description, BlankFromDate, _
                                     AppSettings("Product Version"), AppSettings("SINote"), BlankToDate, SearchStr7:=IIf(chkExchangePurchase.Checked = True, "For Exchange Purchase", ""), _
                                     SearchStr8:=cmbRequisition.SelectedItem.Text, SearchStr9:=AppSettings("ClientCode"), SearchStr10:=AppSettings("Logo"))

        If IsForExcel = False Then
            dsPenOrd.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dsPenOrd)
            da.Fill(dsPenOrd, rpt)

            da.Fill(dsPenOrd, mrptImage)
            da.Fill(dsPenOrd, Report)
            myReport.SetDataSource(dsPenOrd)
            Session("CrystalReport") = myReport

            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "PendingRequisition", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsForExcel = True Then
            dsPenOrd.Clear()
            da.Fill(dsPenOrd, Report)
            da.Fill(dsPenOrd, "ExcelrptPendingRequisitionNewList", rpt)
            Dim columnToRemove1 As String()
            If AppSettings("ClientCode") = "Heligo" Then
                columnToRemove1 = {"RequisitionID", "RequisitionText", "RequisitionNo", "LocationID", "EmployeeID", "ReqTypeID",
                                               "ReqTypeName", "StatusID", "StatusName", "UserName", "AuthorizedBy", "RequisitionItemID", "SrNo", "WOID",
                                               "WONo", "NRCNo", "MachineID", "RegNo", "ReasonForRequest", "ItemID", "IPCReference", "PriorityID",
                                               "AvailableQty", "PurchaseQty", "ReasonForPurchase", "IssueBalQty", "PurchaseBalQty", "EnquiryBalQty",
                                               "QuotationBalQty", "OrderBalQty", "Remark", "Note", "GroupBy", "Heading", "TransTypeID",
                                               "IsPendingForPurchase", "TransType", "IsExchangePurchase"}

                If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("MinReOrderLevel") Then
                    dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("MinReOrderLevel").ColumnName = "Re-order Level"
                End If
                If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("StockBalanceQty") Then
                    dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("StockBalanceQty").ColumnName = "Stock Balance"
                End If
                If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("QuarterlyConsumption") Then
                    dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("QuarterlyConsumption").ColumnName = "Quarterly Consumption"
                End If
                If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("AnnualConsumption") Then
                    dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("AnnualConsumption").ColumnName = "Annual Consumption"
                End If
                PeriodColumnsForExportToExcel.AddRange(New String() {"Re-order Level", "Stock Balance", "Quarterly Consumption", "Annual Consumption"})
                Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            Else
                columnToRemove1 = {"RequisitionID", "RequisitionText", "RequisitionNo", "LocationID", "EmployeeID", "ReqTypeID",
                                               "ReqTypeName", "StatusID", "StatusName", "UserName", "AuthorizedBy", "RequisitionItemID", "SrNo", "WOID",
                                               "WONo", "NRCNo", "MachineID", "RegNo", "ReasonForRequest", "ItemID", "IPCReference", "PriorityID",
                                               "AvailableQty", "PurchaseQty", "ReasonForPurchase", "IssueBalQty", "PurchaseBalQty", "EnquiryBalQty",
                                               "QuotationBalQty", "OrderBalQty", "Remark", "Note", "GroupBy", "Heading", "TransTypeID",
                                               "IsPendingForPurchase", "TransType", "IsExchangePurchase", "MinReOrderLevel", "StockBalanceQty",
                                               "QuarterlyConsumption", "AnnualConsumption"}
            End If

            For i As Integer = 0 To columnToRemove1.Length - 1
                If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains(columnToRemove1(i)) Then
                    dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "Heading", "ProductVersion", "SINote", "ReportName", "ReportDate", "SearchStr7", "SearchStr5", "SearchStr6", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsPenOrd.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    dsPenOrd.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If AppSettings("ClientCode") = "BA" Then
                'Do nothing
            Else
                If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("ContractedWithSupplier") Then
                    dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Remove("ContractedWithSupplier")
                End If
            End If

            If dsPenOrd.Tables("ReportData").Columns.Contains("SearchStr1") Then
                dsPenOrd.Tables("ReportData").Columns("SearchStr1").ColumnName = "FromDate "
            End If

            If dsPenOrd.Tables("ReportData").Columns.Contains("SearchStr2") Then
                dsPenOrd.Tables("ReportData").Columns("SearchStr2").ColumnName = "DateTo "
            End If

            If dsPenOrd.Tables("ReportData").Columns.Contains("SearchStr3") Then
                dsPenOrd.Tables("ReportData").Columns("SearchStr3").ColumnName = "Part No."
            End If

            If dsPenOrd.Tables("ReportData").Columns.Contains("SearchStr4") Then
                dsPenOrd.Tables("ReportData").Columns("SearchStr4").ColumnName = "Description"
            End If
            If dsPenOrd.Tables("ReportData").Columns.Contains("SearchStr8") Then
                dsPenOrd.Tables("ReportData").Columns("SearchStr8").ColumnName = "Part Request/Purchase"
            End If

            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("RequisitionDate") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("RequisitionDate").ColumnName = "Date "
            End If

            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("RequisitionNumber") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("RequisitionNumber").ColumnName = "Req.No."
            End If

            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("EmployeeName") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("EmployeeName").ColumnName = "Employee"
            End If


            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("LocationName") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("LocationName").ColumnName = "Location"
            End If

            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("PartNo") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("PartNo").ColumnName = "Part No."
            End If

            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("Description") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("Description").ColumnName = "Description"
            End If

            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("RequestedQty") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("RequestedQty").ColumnName = "Req.Qty."
            End If
            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("ExcelBalQty") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("ExcelBalQty").ColumnName = "Pending Qty."
            End If
            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("EnquiryNo") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("EnquiryNo").ColumnName = "Enquiry No."
            End If
            If dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns.Contains("TransTypeName") Then
                dsPenOrd.Tables("ExcelrptPendingRequisitionNewList").Columns("TransTypeName").ColumnName = "Requisition"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsPenOrd.Tables("ReportData"))
            dsNew.Merge(dsPenOrd.Tables("ExcelrptPendingRequisitionNewList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptPendingRequisitionNewList").TableName = "Pending Requisition "
			Session("ExcelFileName") = "Pending Requisition"
			Session("dsNew") = dsNew
			Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "PendingRequisition", "Export To Excel " + mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub ControlVisibility1()
        cmbType.Visible = IIf(cmbRequisition.SelectedIndex = 0 Or cmbRequisition.SelectedIndex = 2 Or cmbRequisition.SelectedIndex = 3, True, False)
        'If cmbRequisition.SelectedIndex = 2 Or cmbRequisition.SelectedIndex = 3 Then
        'If cmbRequisition.SelectedIndex = 3 Then 'Planning Req 'Commented by Prashant 20-Oct-2020 STR20102020.Add Requisition Type as “Part Purchase or Part Request” in Planning Requisition module
        '    cmbType.SelectedIndex = 0
        '    cmbType.Enabled = False
        'Else
        '    cmbType.Enabled = True
        'End If
        rbPendingForIssue.Enabled = IIf(cmbRequisition.SelectedIndex = 0 Or cmbRequisition.SelectedIndex = 2 Or cmbRequisition.SelectedIndex = 3, True, False)
        rbPendingForPurchase.Enabled = IIf((cmbRequisition.SelectedIndex = 1 Or _
                                            ((cmbRequisition.SelectedIndex = 0 Or cmbRequisition.SelectedIndex = 2 Or cmbRequisition.SelectedIndex = 3) And cmbType.SelectedIndex = 1)), True, False)
        rbPendingForIssue.Checked = rbPendingForIssue.Enabled
        rbPendingForPurchase.Checked = rbPendingForPurchase.Enabled
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()

            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            ControlVisibility1()
        End If

    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
        upnlDateRange.Update()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRangeFrom.Visible = True
        'lblToDate.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbRequisition_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbRequisition.SelectedIndexChanged
        ControlVisibility1()
    End Sub
    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        ControlVisibility1()
    End Sub
    'Added by Abhishek on 14-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid() Then
            SetReport(True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetItemList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim partlist As ItemListAutoComplete
        partlist = ItemListAutoComplete.GetItemList(prefixText)
        If count = 0 Then
            Return (From c As ItemListAutoCompleteInfo In partlist
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoCompleteInfo In partlist
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

End Class