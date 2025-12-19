'AJAX Conversion By Vikrant On 23-Jan-2014

Public Class wfrptConsumableItemListReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ToDate As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim Fromdate As String
    Dim EventLogDetail As String = String.Empty
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
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
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1/1/1900"
            ToDate = "1/1/2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            Fromdate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " (" & cmbDateRange.SelectedItem.Text & ")"
        End If

        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If

        Session("PartNo") = PartNo
        Session("Description") = Description

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        EventLogDetail = lblDateRangeFrom.Text + "," + lblPartNo.Text + "," + lblDesc.Text
    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptConsumableItemList As rptConsumableItemList
        Dim objSearch As rptSearchingCriteria
        'Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsConsumableItemList
        SetValues()
        myReport = New crptConsumableItemList
        mrptConsumableItemList = rptConsumableItemList.GetConsumableItemList(Fromdate, ToDate, PartNo, Description)
        objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, PartNo, AppSettings("Logo"), "", "", "", "", "", "", Description, "", 0, "", "", "", "", Search9:=Today.Date.ToString(AppSettings("DateFormat")))

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        '      mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        '      mCompanyDetail.WebSite, "Consumable Item Report", New SmartDate(Fromdate).FormattedText, New SmartDate(ToDate).FormattedText, PartNo, Description, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mrptConsumableItemList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf (mrptConsumableItemList.Count > 0 And IsExcel = False) Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1225)
        End If

        If IsExcel = False Then
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mrptConsumableItemList)
            da.Fill(ds, objSearch)
            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "ConsumableItemListReport", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            ds.Clear()
            da.Fill(ds, "ExcelrptConsumableItemList", mrptConsumableItemList)
            da.Fill(ds, "rptSearchingCriteria", objSearch)

            Dim columnToRemove As String() = {"CompanyName", "SupplierName", "BranchName", "Category", "Nomenclature", "Store", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search10"}
            Dim columnToRemove1 As String() = {"IssueText", "IssueNo", "TotalAmount", "SerialNo", "RegNo", "WorkShopName", "WorkOrderNo", "ToTypeID", "TransTypeID"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove(i))
                End If
            Next

            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptConsumableItemList").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptConsumableItemList").Columns.Remove(columnToRemove1(i))
                End If
            Next


            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelrptConsumableItemList"))

            dsNew.Tables("rptSearchingCriteria").Columns("Search9").ColumnName = "Report Date"

            dsNew.Tables("ExcelrptConsumableItemList").Columns("IssueTextNo").ColumnName = "Issue No."
            dsNew.Tables("ExcelrptConsumableItemList").Columns("IssueQty").ColumnName = "Qty."
            dsNew.Tables("ExcelrptConsumableItemList").Columns("IssueDate").ColumnName = "Date"
            dsNew.Tables("ExcelrptConsumableItemList").Columns("Destination").ColumnName = "Issue To"
            dsNew.Tables("ExcelrptConsumableItemList").Columns("EffRate").ColumnName = "Rate"
            dsNew.Tables("ExcelrptConsumableItemList").Columns("Amount").ColumnName = "Total Price"
            dsNew.Tables("ExcelrptConsumableItemList").Columns("PartName").ColumnName = "PartNo"

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptConsumableItemList").TableName = "Consumable Item List"
			Session("ExcelFileName") = "Consumable Item List"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "ConsumableItemListReport", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If

      
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport(False)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then SetReport(True)
    End Sub
#End Region

End Class