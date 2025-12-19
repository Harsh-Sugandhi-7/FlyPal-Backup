Public Class wfrptGRNwisePurchaseSummaryList_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents txtToDate As System.Web.UI.WebControls.TextBox


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public FromDate As String
    Public ToDate As String
    Public mCompanyDetail As New CompanyDetail
    Public mCategoryList As CategoryList
    Public strCategory, strCategoryList As String
    Dim SelectedCategoryCnt As Integer = 0
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    Dim FormatType As String
    Dim Rate As String
    Public mModelList As ModelList
    Public mCustomerList As VendorList
    Public strCustomer As String
    Public SupplierID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = Session("mCategoryList")
        mCustomerList = CType(Session("mCustomerList"), VendorList)
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryList")
        Session.Remove("mCustomerList")
    End Sub
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
        ElseIf Index = 0 Then
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblCategory1.Visible = True
        lblModel.Visible = True
        lblCustomerName.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblCategory1.Visible = False
        lblModel.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
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
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
       
        strCategory = String.Empty
        SelectedCategoryCnt = 0
        For i As Integer = 0 To ChklistCategory.Items.Count - 1
            If ChklistCategory.Items(i).Selected Then
                SelectedCategoryCnt = SelectedCategoryCnt + 1
                If strCategory.Length = 0 Then
                    strCategory = ChklistCategory.Items(i).Text
                    strCategoryList = ChklistCategory.Items(i).Text
                Else
                    strCategory = strCategory + "," + ChklistCategory.Items(i).Text
                    strCategoryList = strCategoryList + ", " + ChklistCategory.Items(i).Text
                End If
            End If
        Next

        lblCategory1.Text = "Category Name : " & IIf(strCategory.Length > 0, strCategory, "All")
        'End
        lblModel.Text = "Model : " & IIf(cmbModel.SelectedIndex = 0, "All", cmbModel.SelectedItem.Text)
        Rate = IIf(rdoBase.Checked Or rdoCommercial.Checked, IIf(rdoBase.Checked, "By Base Value", "By Commercial Value"), "By Landing Value")
        FormatType = cmbFormat.SelectedItem.Text
        If txtSupplierList.Text.Trim = "" Then
            lblCustomerName.Text = "Supplier : All"
        Else
            strCustomer = mCustomerList(txtSupplierList.Text.Trim).Name
            lblCustomerName.Text = "Supplier :" & strCustomer
        End If
        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + ", " + Rate + "," + FormatType + ", " + lblCategory1.Text + ", " + lblModel.Text + ", " + lblCustomerName.Text
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As GRNwisePurchaseSummaryList
        Dim ReportLabel As String
        Dim Value As String = ""
        Dim ReportName As String = ""
        SetValues()

        If rdoBase.Checked = True Then
            Value = "Base Value"
            ReportLabel = "GRN Wise Purchase Summary (Base Value)"
        ElseIf rdoLanding.Checked = True Then
            Value = "Landing Value"
            ReportLabel = "GRN Wise Purchase Summary (Landing Value)"
        Else
            Value = "Commercial Value"
            ReportLabel = "GRN Wise Purchase Summary (Commercial Value)"
        End If

        If cmbFormat.SelectedIndex = 0 Then
            myReport = New crptGRNwiseSummaryList
        ElseIf cmbFormat.SelectedIndex = 1 Then
            myReport = New crptGRNwiseSummaryListFormat2
        End If
        If txtSupplierList.Text = "" Then
            SupplierID = Guid.Empty
        Else
            SupplierID = mCustomerList(Trim(txtSupplierList.Text)).ID
        End If
        rpt = GRNwisePurchaseSummaryList.GetGRNwisePurchaseSummaryList(FromDate, ToDate, strCategory, Value, cmbModel.SelectedValue, SupplierID.ToString,
                                                                       ClientCode:=AppSettings("ClientCode"))

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, strCategoryList, IIf(cmbModel.SelectedIndex = 0, "", cmbModel.SelectedItem.Text), Trim(txtSupplierList.Text), AppSettings("Product Version"), AppSettings("SINote"), , , , , AppSettings("Logo"))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1257)
        End If
        If IsExcel = False Then 'If PDF format
            Dim ds As New dsGRNwisePurchaseSummaryList
            ds.Clear()
            Dim mrptImage As rptImage
            mrptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "GRNPurchaseSummaryReport", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcelGRNwisePurchaseSummaryList
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "GRNwisePurchaseSummaryList", rpt)
            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"SrNo", "CategoryID", "VendorInvoiceDate"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("GRNwisePurchaseSummaryList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("GRNwisePurchaseSummaryList").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("GRNwisePurchaseSummaryList").Columns.Contains("RecNo") Then
                ds.Tables("GRNwisePurchaseSummaryList").Columns("RecNo").ColumnName = "GRN No."
            End If
            If ds.Tables("GRNwisePurchaseSummaryList").Columns.Contains("VendorName") Then
                ds.Tables("GRNwisePurchaseSummaryList").Columns("VendorName").ColumnName = "Supplier"
            End If
            If ds.Tables("GRNwisePurchaseSummaryList").Columns.Contains("VendorInvoiceNo") Then
                ds.Tables("GRNwisePurchaseSummaryList").Columns("VendorInvoiceNo").ColumnName = "Supplier Invoice No."
            End If
            If ds.Tables("GRNwisePurchaseSummaryList").Columns.Contains("VendorInvoiceDateFormatted") Then
                ds.Tables("GRNwisePurchaseSummaryList").Columns("VendorInvoiceDateFormatted").ColumnName = "Supplier Invoice Date"
            End If
            If ds.Tables("GRNwisePurchaseSummaryList").Columns.Contains("TotalEffRate") Then
                ds.Tables("GRNwisePurchaseSummaryList").Columns("TotalEffRate").ColumnName = "Total"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Category"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Model"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Supplier"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("ReportData").TableName = "Searching Criteria"
			ds.Tables("GRNwisePurchaseSummaryList").TableName = ReportLabel
			Session("ExcelFileName") = ReportLabel
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "GRNPurchaseSummaryReport", "Export To excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
  Private Function CheckCheckedCategories() As Boolean
        Dim j As Integer = 0
        For j = 0 To ChklistCategory.Items.Count - 1
            If ChklistCategory.Items(j).Selected = True Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList()
        ChklistCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList

        mModelList = ModelList.GetAirframeModelList("(SELECT)")
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()

        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, True, True)
        Session("mCustomerList") = mCustomerList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            'Ajay 09-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "GRNPurchaseSummaryReport") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            DataFieldBind()
            cmbDateRange.SelectedIndex = 6
            ControlVisibility(6)
            setDatePeroid(6)
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
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
         If IsValid Then
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
    End Sub

    'Ajay 09-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "GRNPurchaseSummaryReport")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "GRNPurchaseSummaryReport")
    End Sub
    '-----
#End Region

End Class