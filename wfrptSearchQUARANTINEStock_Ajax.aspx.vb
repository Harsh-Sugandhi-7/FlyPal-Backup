Public Class wfrptSearchQUARANTINEStock_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCategoryList As CategoryList
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String
    Public Description As String
    Public strCategory As String
    Dim mQUARANTINEStockSearchingCriteria As String = String.Empty

    'Added by Abhishek on 13-SEP-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim ds As New dsQUARANTINEReport
    Dim objsearch As rptSearchingCriteria
    Dim rpt As rptQUARANTINEReport
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
   Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblCategoryName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblCategoryName.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub ControlVisibility(ByVal index As Integer)
        lblFromDate.Visible = IIf(index <> 0, True, False)
        lblToDate.Visible = IIf(index <> 0, True, False)
        lblDateRangeFrom.Visible = False

        If index = 6 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf index = 1 Or index = 2 Or index = 3 Or index = 4 Or index = 5 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            'txtFromDate.Enabled = False
            'txtToDate.Enabled = False
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
        If cmbDateRange.SelectedIndex = 0 Then      'Date Range
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If

        If cmbCategory.SelectedIndex = 0 Then       'Category
            strCategory = ""
            lblCategoryName.Text = "Category : All"
        Else
            strCategory = Category.GetCategory(New Guid(cmbCategory.SelectedValue)).Name
            lblCategoryName.Text = "Category : " & strCategory
        End If
        'Added By Shweta ON 06-Dec-2012 FOR ALL28112012
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        'End
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        mQUARANTINEStockSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblCategoryName.Text + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim
    End Sub
   Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptQUARANTINEReport
        SetValues()
        Dim ds As New dsQUARANTINEReport
        ''myReport = New crptQUARANTINEReport
        myReport = New crptQUARANTINE
        rpt = rptQUARANTINEReport.GetQUARANTINEReport(FromDate, ToDate, PartNo, Description, strCategory)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, "", "", strCategory, "", "", "", "", Description, "", 0, "", "", "", AppSettings("Logo"))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 511)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, rpt)
        da.Fill(ds, mrptImage)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "QUARANTINEStoreStock", mQUARANTINEStockSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                SetFocus(cmbDateRange)
            End If
            DataFieldBind()
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
            SetFocus(cmbDateRange)
        End If
        upnlDateRange.Update()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    'Added by Abhishek on 13-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then

            SetValues()
            rpt = rptQUARANTINEReport.GetQUARANTINEReport(FromDate, ToDate, PartNo, Description, strCategory)
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, "", "", strCategory, "", "", "", "", Description, "", 0, "", "", "", AppSettings("Logo"))
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 511)
            End If
            ds.Clear()

            da.Fill(ds, objsearch)
            da.Fill(ds, "ExcelrptQUARANTINEReport", rpt)

            'Dim columnToRemove1 As String() = {}
            'For i As Integer = 0 To columnToRemove1.Length - 1
            '    If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains(columnToRemove1(i)) Then
            '        ds.Tables("ExcelrptQUARANTINEReport").Columns.Remove(columnToRemove1(i))
            '    End If
            'Next
            If cmbDateRange.SelectedIndex = 0 Then
                Dim columnToRemove2 As String() = {"FromDate", "ToDate", "CompanyName", "SupplierName", "BranchName", "Nomenclature", "Store", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                    End If
                Next
                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("PartName") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("PartName").ColumnName = "Part Number "
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("PartDescription") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("PartDescription").ColumnName = "Description"
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("RecQty") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("RecQty").ColumnName = "Purchase Qty."
                End If


                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("RecAmount") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("RecAmount").ColumnName = "Purchase Amount"
                End If
                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("InvBalQty") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("InvBalQty").ColumnName = "Purchase Inv.Bal.Qty."
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("IssQty") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("IssQty").ColumnName = "Consumed Qty."
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("IssAmount") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("IssAmount").ColumnName = "Consumed Amount"
                End If
                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("Currency") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("Currency").ColumnName = "Currency"
                End If

                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("rptSearchingCriteria"))
                dsNew.Merge(ds.Tables("ExcelrptQUARANTINEReport"))

                dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
                dsNew.Tables("ExcelrptQUARANTINEReport").TableName = "QUARANTINE-Store Stock "
				Session("ExcelFileName") = "QUARANTINE-Store Stock "
				Session("dsNew") = dsNew
				Session("DataTableToBeFormattedForExportToExcel") = "QUARANTINE-Store Stock "
            Else
                Dim columnToRemove2 As String() = {"CompanyName", "SupplierName", "BranchName", "Nomenclature", "Store", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                    End If
                Next
                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("PartName") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("PartName").ColumnName = "Part Number "
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("PartDescription") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("PartDescription").ColumnName = "Description"
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("RecQty") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("RecQty").ColumnName = "Purchase Qty."
                End If


                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("RecAmount") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("RecAmount").ColumnName = "Purchase Amount"
                End If
                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("InvBalQty") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("InvBalQty").ColumnName = "Purchase Inv.Bal.Qty."
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("IssQty") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("IssQty").ColumnName = "Consumed Qty."
                End If

                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("IssAmount") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("IssAmount").ColumnName = "Consumed Amount"
                End If
                If ds.Tables("ExcelrptQUARANTINEReport").Columns.Contains("Currency") Then
                    ds.Tables("ExcelrptQUARANTINEReport").Columns("Currency").ColumnName = "Currency"
                End If

                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("rptSearchingCriteria"))
                dsNew.Merge(ds.Tables("ExcelrptQUARANTINEReport"))

                dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
                dsNew.Tables("ExcelrptQUARANTINEReport").TableName = "QUARANTINE-Store Stock "
				Session("ExcelFileName") = "QUARANTINE-Store Stock "

				Session("dsNew") = dsNew
				Session("DataTableToBeFormattedForExportToExcel") = "QUARANTINE-Store Stock "
            End If
           

          
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "QUARANTINEStoreStock", "Export To Excel " + mQUARANTINEStockSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
End Class