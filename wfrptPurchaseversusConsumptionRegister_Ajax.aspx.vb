Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Public Class wfrptPurchaseversusConsumptionRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mVendor As Vendor
    Public mItemList As ItemList
    Public mVendorList As VendorList
    Public mOrderTextList As DistinctTextListForOrder
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public Supplier As String = ""
    Public OrdText As String = ""
    Public OrdNo As String = ""
    Public Amend As String = ""
    Public QuotationNo As String = ""
    Public Status As String = ""
    Public IntOrderNo As String = ""
    Public PriorityName As String = ""
    Public PriorityID As Integer
    Public mPriorityList As PriorityList
    Public Aircraft As String = "" 'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Public mCategoryLists As CategoryList
    Public mModelList As ModelList
    Public mCustomerList As VendorList
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        mPriorityList = CType(Session("mPriorityList"), PriorityList)
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = CType(Session("PartNo"), String)
        Description = CType(Session("Description"), String)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
        Session("mItemList") = mItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mPriorityList")
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
        lblTransType.Visible = True
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblOrderNo.Visible = True
        lblQuotNo1.Visible = True
        lblIntOrderNo.Visible = True
        lblStatus1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblPriority1.Visible = True
        lblAircraft1.Visible = True 'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
        lblExpenses1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblVendor.Visible = False
        lblOrderNo.Visible = False
        lblQuotNo1.Visible = False
        lblIntOrderNo.Visible = False
        lblStatus1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblPriority1.Visible = False
        lblAircraft1.Visible = False 'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
        lblExpenses1.Visible = False
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        Supplier = txtSupplier.Text.Trim
        lblVendor.Text = "Supplier :  " & Supplier

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")

        Session("PartNo") = PartNo
        Session("Description") = Description

        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblQuotNo1.Text = "Supp. Quot. No. : " & IIf(QuotationNo <> "", QuotationNo, "All")
        lblOrderNo.Text = "Order No.: " & IIf(OrdText + OrdNo + Amend <> "", OrdText + "-" + OrdNo + Amend, "All")
        lblIntOrderNo.Text = "Internal Order No.: " & IIf(IntOrderNo <> "", IntOrderNo, "All")
        lblTransType.Text = "Order Type     : " & IIf(cmbOrderType.SelectedIndex > 0, cmbOrderType.SelectedItem.Text, "All")

        mCompleteSearchingCriteria = lblTransType.Text + ", " + lblDateRange.Text + ", " + lblVendor.Text + ", " + lblQuotNo1.Text + ", " + lblIntOrderNo.Text + ", " + _
           lblOrderNo.Text + ", " + lblStatus1.Text + ", " + lblPriority1.Text + ", " + lblAircraft1.Text + ", " + lblExpenses1.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text

    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        Dim objReg As rptPurchaseversusConsumption
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsPurchaseversusConsumption
        Dim mCompanyDetail As New CompanyDetail

        SetValues()
        Dim mModelID As Guid = Guid.Empty
        If txtModelList.Text.Trim <> "" Then
            mModelList = ModelList.GetModelList(0, "", , , "(All)")
            mModelID = mModelList.Item(txtModelList.Text.Trim).ID
        End If


        objReg = rptPurchaseversusConsumption.GetPurchaseversusConsumptionList(PartNo, Description, FromDate, ToDate, _
                                                                               Supplier, cmbOrderType.SelectedValue, cmbCategory.SelectedValue, _
                                                                               mModelID.ToString)
        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1510)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Purchase versus Consumption", SearchStr1:=New SmartDate(FromDate).FormattedText, SearchStr2:=New SmartDate(ToDate).FormattedText, _
        SearchStr3:=cmbOrderType.SelectedItem.Text, SearchStr4:=Supplier, SearchStr5:=cmbCategory.SelectedItem.Text, _
        ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=txtModelList.Text.Trim, SearchStr7:=PartNo, _
        SearchStr8:=Description, SearchStr9:="", _
        SearchStr10:=AppSettings("Logo"))

        ds.Clear()
        da.Fill(ds, objReg)
        da.Fill(ds, Report)

        Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", _
                                           "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", _
                                           "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", _
                                           "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", _
                                           "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
            End If
        Next

        Dim columnToRemove As String() = {"Amount", "CurrencyName", "OrderText", "OrderNo", "Amend", "ReceiptText", "ReceiptNo", "IssueText", "IssueNo", "IssueDate", "IssueNumber"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptPurchaseversusConsumption").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptPurchaseversusConsumption").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
        dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
        dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Type Of Order"
        dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Supplier"
        dsNew.Tables("ReportData").Columns("SearchStr5").ColumnName = "Category"
        dsNew.Tables("ReportData").Columns("SearchStr6").ColumnName = "Model"
        dsNew.Tables("ReportData").Columns("SearchStr7").ColumnName = "Part No."
        dsNew.Tables("ReportData").Columns("SearchStr8").ColumnName = "Description"
        '
        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Merge(ds.Tables("rptPurchaseversusConsumption"))

        dsNew.Tables("rptPurchaseversusConsumption").Columns("OrderDate").ColumnName = "Order Date"
        dsNew.Tables("rptPurchaseversusConsumption").Columns("OrderNumber").ColumnName = "Order No."
        dsNew.Tables("rptPurchaseversusConsumption").Columns("PartNo").ColumnName = "Part No."
        dsNew.Tables("rptPurchaseversusConsumption").Columns("PartDescription").ColumnName = "Description"

        dsNew.Tables("rptPurchaseversusConsumption").Columns("OrderQty").ColumnName = "Order Qty."
        dsNew.Tables("rptPurchaseversusConsumption").Columns("ReceiptDate").ColumnName = "Receipt Date"
        dsNew.Tables("rptPurchaseversusConsumption").Columns("ReceiptNumber").ColumnName = "Receipt No."

        dsNew.Tables("rptPurchaseversusConsumption").Columns("ReceiptDisplayQty").ColumnName = "Received Qty."

        dsNew.Tables("rptPurchaseversusConsumption").Columns("EffRate").ColumnName = "Effective Rate (NPR)"
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("IssueDate").ColumnName = "Issue Date"
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("IssueNumber").ColumnName = "Issue No."
        dsNew.Tables("rptPurchaseversusConsumption").Columns("IssueDisplayQty").ColumnName = "Issue Qty."
        dsNew.Tables("rptPurchaseversusConsumption").Columns("ReqNumber").ColumnName = "MRN/PPS"

        'dsNew.Tables("rptPurchaseversusConsumption").Columns("OrdQty").ColumnName = "Ord. Qty."
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("RecQty").ColumnName = "Rec. Qty."
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("BalQty").ColumnName = "Bal. Qty."
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("CAmount").ColumnName = "Bal. Amount"
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("CurrencyName").ColumnName = "Currency"
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Amount").ColumnName = "Bal. Amount In Base Curr."
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("FollowUpTextNo").ColumnName = "Follow Up No."
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("FollowUpDate").ColumnName = "Follow Up Date"
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("AWBNo").ColumnName = "AWB No."
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("ProformaNo").ColumnName = "Proforma No."
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("ReturnInDays").ColumnName = "Return In Days"
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("ShipmentStatus").ColumnName = "Shipmen Status"
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("FollowUpRemarks").ColumnName = "Remark"

        ''set Column Sequence
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Qty.").SetOrdinal(3)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Order Date").SetOrdinal(1)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Int. Order No.").SetOrdinal(2)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Order Type").SetOrdinal(3)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Supplier").SetOrdinal(4)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Part No.").SetOrdinal(5)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Description").SetOrdinal(6)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Serial No.").SetOrdinal(7)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Deliv. In Days").SetOrdinal(8)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Priority").SetOrdinal(9)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Remaining Days").SetOrdinal(10)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Ord. Qty.").SetOrdinal(11)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Rec. Qty.").SetOrdinal(12)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Bal. Qty.").SetOrdinal(13)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Bal. Amount").SetOrdinal(14)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Currency").SetOrdinal(15)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Bal. Amount In Base Curr.").SetOrdinal(16)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Follow Up No.").SetOrdinal(17)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Follow Up Date").SetOrdinal(18)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("AWB No.").SetOrdinal(19)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Proforma No.").SetOrdinal(20)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Return In Days").SetOrdinal(21)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("TD").SetOrdinal(22)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Shipmen Status").SetOrdinal(23)
        'dsNew.Tables("rptPurchaseversusConsumption").Columns("Remark").SetOrdinal(24)

        dsNew.Tables("rptPurchaseversusConsumption").TableName = "Purchase versus Consumption"
		Session("ExcelFileName") = "Purchase versus Consumption"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
        MarkLog(Util.Action.Print, "PurchaseversusConsumption", "Export To excel", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
    Private Sub SetDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select

        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()
            If cmbOrderType.Enabled = True Then
                SetFocus(cmbOrderType)
            End If
            DataFieldBind()
            ControlVisibility(6)
            SetDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        SetDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid Then
            ControlVisibility2()
            SetValues()
            upnlDisplaySearchCriteria.Update()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
               Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                   Select c.Name).Take(count).ToList
        End If
    End Function

End Class