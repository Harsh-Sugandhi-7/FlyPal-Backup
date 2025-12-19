Public Class wfrptPartPurchaseSheet_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mPriorityList As PriorityList
    Public mrptPartPurchaseStatusList As rptPartPurchaseStatusList
    Public Priority, IssuedStatus As String
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public Requisition As Integer
    Public EmployeeName As String
    Public RequisitionText As String = ""
    Public RequisitionNo As Integer = 0
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    Dim ds As DataSet
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mPriorityList = Session("mPriorityList")
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblRequisitionNo.Visible = True
        lblIssuedStatus.Visible = True
        lblPriority.Visible = True
        lblEmployee1.Visible = True
    End Sub
    Private Sub ControlVisibility(ByVal index As Integer)
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
        End If
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If txtEmployee.Text = "" Then
            EmployeeName = ""
            lblEmployee1.Text = "Employee : All"
        Else
            EmployeeName = txtEmployee.Text.Trim
            lblEmployee1.Text = "Employee : " + EmployeeName
        End If

        If cmbPriority.SelectedIndex <= 0 Then
            Priority = ""
            lblPriority.Text = "Priority : All"
        Else
            Priority = cmbPriority.SelectedItem.ToString
            lblPriority.Text = "Priority : " + Priority
        End If

        If cmbIssueStatus.SelectedIndex <= 0 Then
            IssuedStatus = ""
            lblIssuedStatus.Text = "Issued Status : All"
        Else
            IssuedStatus = cmbIssueStatus.SelectedItem.ToString
            lblIssuedStatus.Text = "Issued Status : " + IssuedStatus
        End If

        If txtRequisitionNo.Text <> "" Then
            RequisitionNo = txtRequisitionNo.Text.Trim
            lblRequisitionNo.Text = txtRequisitionText.Text.Trim + "-" + txtRequisitionNo.Text.Trim
        Else
            lblRequisitionNo.Text = "Requisition No. : " + IIf(txtRequisitionText.Text.Trim <> "", txtRequisitionText.Text.Trim, "")
            RequisitionNo = 0
        End If

        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + ", " + lblPriority.Text + "," + lblRequisitionNo.Text + ", " + lblIssuedStatus.Text + ", " + lblEmployee1.Text
    End Sub
    Public Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        'Dim Rpt As New crptRequisitionItemStatus
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim ds As New dsRequisitionItemStatus
        Dim mCompanyDetail As New CompanyDetail

        Rpt = New crptPartPurchaseSheet

        mrptPartPurchaseStatusList = rptPartPurchaseStatusList.GetList(txtRequisitionText.Text.Trim, Val(txtRequisitionNo.Text), txtFromDate.Text, txtToDate.Text, CInt(cmbIssueStatus.SelectedValue), CInt(cmbPriority.SelectedValue), txtEmployee.Text.Trim, AppSettings("ClientCode").ToString)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Part Purchase Sheet Status", FromDate, ToDate, txtEmployee.Text, IIf(cmbPriority.SelectedIndex <= 0, "", cmbPriority.SelectedItem.Text), "", AppSettings("Product Version"), AppSettings("SINote"), IIf(cmbIssueStatus.SelectedIndex > 0, cmbIssueStatus.SelectedItem.ToString, ""), IIf(Val(txtRequisitionNo.Text) > 0, txtRequisitionText.Text.Trim + "-" + txtRequisitionNo.Text.Trim, txtRequisitionText.Text.Trim), "", IIf(cmbDateRange.SelectedIndex = 0, "True", "False"), AppSettings("Logo"))

        If mrptPartPurchaseStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not IsExcel Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1410)
        End If

        If IsExcel Then
            ds = New dsPartPurchaseSheet
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "ExcelrptPartPurchaseStatusList", mrptPartPurchaseStatusList)

            Dim columnToRemove2 As String() = {"ID", "SearchStr5", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"ReqID", "Date", "Text", "No", "ReqTypeID", "ReqTypeName", "RequisitionEngineeringBranch", "TransTypeID", "TransTypeName", "ReqItemID", "WOID", "WONo", "MachineID", "MachineName", "ItemID", "WorkShopID", "WorkShopName", "LocationName", "OrderDetails", "IssueDetails", "ReceiptDetails"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExcelrptPartPurchaseStatusList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelrptPartPurchaseStatusList").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("ExcelrptPartPurchaseStatusList").Columns.Contains("CRate") Then
                ds.Tables("ExcelrptPartPurchaseStatusList").Columns("CRate").ColumnName = "Rate"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Requested By"
            dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Priority"
            dsNew.Tables("ReportData").Columns("SearchStr6").ColumnName = "Issued Status"
            dsNew.Tables("ReportData").Columns("SearchStr7").ColumnName = "Requisition"

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Merge(ds.Tables("ExcelrptPartPurchaseStatusList"))
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("DateFormatted").ColumnName = "Date"

            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("ReqQty").ColumnName = "Quantity"
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("RequisitionTextNo").ColumnName = "PPS No."
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("PartNo").ColumnName = "Part Number"
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("PartDescription").ColumnName = "Description"
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("OrderDetailsExcel").ColumnName = "PO Number / Date"
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("ReceiptDetailsExcel").ColumnName = "GRN No / Date"
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("IssueDetailsExcel").ColumnName = "Issue Reference / Date"
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("EmployeeName").ColumnName = "Requested By"
            dsNew.Tables("ExcelrptPartPurchaseStatusList").Columns("CostCenter").ColumnName = "Required for A/C or WorkShop"

            dsNew.Tables("ExcelrptPartPurchaseStatusList").TableName = "Part Purchase Sheet Status"
			Session("ExcelFileName") = "Part Purchase Sheet Status"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "PartPurchaseSheetStatus", "Export To excel", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        Else
            ds = New dsRequisitionItemStatus
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mrptPartPurchaseStatusList)
            da.Fill(ds, Report)
            Rpt.SetDataSource(ds)
            Session("CrystalReport") = Rpt
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "PartPurchaseSheet", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
       
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtRequisitionNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtRequisitionNo').value,event)")
    End Sub
    Private Sub DataFieldBind()
        mPriorityList = PriorityList.GetPriorityList(, , "(All)")
        Session("mPriorityList") = mPriorityList
        cmbPriority.DataSource = mPriorityList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            DataFieldBind()
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            cmbDateRange.Focus()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetValues()
            SetReport()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mPriorityList")
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            SetReport(True)
        End If
    End Sub
    
    
End Class