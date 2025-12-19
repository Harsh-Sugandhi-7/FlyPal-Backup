'Added By Vikrant on 09-Oct-2014

Public Class wfrptRequisitionItemStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mDistinctTextListForRequisition As DistinctTextListForRequisition
    Dim TransTypeID, ReqTypeID As Integer
    Dim SearchIndex, FromDate, ToDate, RequisitionText, Name, No, BranchIndex, OrderText, ReceiptText, IssueText As String
    Dim EventLogID As Guid
    Dim mIssue As Issue
    Dim mReceiptCumInvoice As ReceiptCumInvoice
    Dim mReceipt As Receipt
    Dim mOrder As Order
    Dim mrptRequisitionItemStatusList As rptRequisitionItemStatusList
    Public mRequisitionEngineeringBranchesList As RequisitionEngineeringBranchesList
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mDistinctTextListForIssue As DistinctTextListForIssue
    'Added By Vikrant On 06-Sep-2018 For BA05092018
    Public PriorityID As Integer = 0
    Public mPriorityList As PriorityList
    'End
    Dim mEnquiry As Enquiry
    Dim mQuotation As Quotation
    Public EventLogDetails As String = String.Empty
#End Region

#Region " Business Methods"
    Private Sub GetSession()
        mrptRequisitionItemStatusList = Session("mrptRequisitionItemStatusList")
        mDistinctTextListForRequisition = Session("mDistinctTextListForRequisition")
        SearchIndex = Session("SearchIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        ReqTypeID = Session("ReqTypeID")
        RequisitionText = IIf(IsNothing(Session("RequisitionText")), "", Session("RequisitionText"))
        Name = IIf(IsNothing(Session("Name")), "", Session("Name"))
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        TransTypeID = IIf(IsNothing(Session("TransTypeID")), 0, CInt(Session("TransTypeID")))
        BranchIndex = Session("BrancheIndex")

        OrderText = IIf(IsNothing(Session("OrderText")), "", Session("OrderText"))
        ReceiptText = IIf(IsNothing(Session("ReceiptText")), "", Session("ReceiptText"))
        IssueText = IIf(IsNothing(Session("IssueText")), "", Session("IssueText"))
        'Added By Vikrant On 06-Sep-2018 For BA05092018
        PriorityID = IIf(IsNothing(Session("PriorityID")), 0, Session("PriorityID"))
        mPriorityList = Session("mPriorityList")
        'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptRequisitionItemStatusList")
        Session.Remove("mDistinctTextListForRequisition")
        Session.Remove("SearchIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("ReqTypeID")
        Session.Remove("RequisitionText")
        Session.Remove("Name")
        Session.Remove("No")
        Session.Remove("TransTypeID")
        Session.Remove("BranchIndex")
        Session.Remove("OrderText")
        Session.Remove("ReceiptText")
        Session.Remove("IssueText")
        'Added By Vikrant On 06-Sep-2018 For BA05092018
        Session.Remove("PriorityID")
        Session.Remove("mPriorityList")
        'End
    End Sub
    Private Sub ClearAll()
        TransTypeID = Session("TransTypeID")
        If Session("MiddleFrame") <> "wfrptRequisitionItemStatus_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", _
                        Optional ByVal ToDate As String = "1/1/3300", Optional ByVal ReqTypeID As Integer = 0, Optional ByVal TransTypeID As Integer = 0, _
                        Optional ByVal OrdText As String = "", Optional ByVal OrdNo As Integer = 0, Optional ByVal RecText As String = "", _
                        Optional ByVal RecNo As Integer = 0, Optional ByVal IssText As String = "", Optional ByVal IssNo As Integer = 0, _
                        Optional ByVal Issued As Integer = 0, Optional ByVal NotIssued As Integer = 0, Optional ByVal PriorityID As Integer = 0, _
                        Optional ByVal Type As Integer = 0, Optional ByVal ShowPartPurchaseReqTransactionOnly As Boolean = False)
        mrptRequisitionItemStatusList = Nothing
        dgRequisitionItemList.DataSource = Nothing
        mrptRequisitionItemStatusList = rptRequisitionItemStatusList.GetRequisitionItemStatusList(ItemName, Text, No, FromDate, ToDate, ReqTypeID, _
                                                                                                  TransTypeID, IIf(cmbReqType.SelectedValue = 0, -1, cmbRequisitionEngineeringBranches.SelectedValue), _
                                                                                                   OrdText, OrdNo, RecText, RecNo, IssText, IssNo, Issued, _
                                                                                                   NotIssued, PriorityID, Type, chkShowPPReqOnly.Checked, AppSettings("ClientCode").ToString)
        Session("mrptRequisitionItemStatusList") = mrptRequisitionItemStatusList
        dgRequisitionItemList.DataSource = mrptRequisitionItemStatusList
        lblResult.Text = "List of Order Item(s) as per Criteria :" & mrptRequisitionItemStatusList.Count & " Record(s) found."
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer, Optional ByVal IsForPrint As Integer = 0) '
        Select Case Index
            Case 0  'all
                Call FindNow("", "", 0, FromDate, ToDate, Type:=IsForPrint, ReqTypeID:=ReqTypeID)     'for all records
            Case 1  'Req Text , No 
                Call FindNow("", RequisitionText, CInt(Val(No)), FromDate, ToDate, Type:=IsForPrint, ReqTypeID:=ReqTypeID)
            Case 2  'ItemName
                Call FindNow(Name, "", 0, FromDate, ToDate, Type:=IsForPrint, ReqTypeID:=ReqTypeID)
            Case 3 ' ReqType
                Call FindNow("", "", 0, FromDate, ToDate, , TransTypeID, Type:=IsForPrint)
            Case 4  'Order Text , Order No 
                Call FindNow("", "", 0, FromDate, ToDate, ReqTypeID, 0, OrderText, CInt(Val(No)), Type:=IsForPrint)
            Case 5  'Receipt Text , No 
                Call FindNow("", "", 0, FromDate, ToDate, ReqTypeID, 0, "", 0, ReceiptText, CInt(Val(No)), Type:=IsForPrint)
            Case 6  'Issue Text , No 
                Call FindNow("", "", 0, FromDate, ToDate, ReqTypeID, 0, "", 0, "", 0, IssueText, CInt(Val(No)), Type:=IsForPrint)
            Case 7  'Issued 
                Call FindNow("", "", 0, FromDate, ToDate, ReqTypeID, 0, "", 0, "", 0, "", 0, Index, Type:=IsForPrint)
            Case 8  'Not issued
                Call FindNow("", "", 0, FromDate, ToDate, ReqTypeID, 0, "", 0, "", 0, "", 0, 0, Index, Type:=IsForPrint)
                'Added By Vikrant On 06-Sep-2018 For BA05092018
            Case 9  'Priority
                Call FindNow("", "", 0, FromDate, ToDate, ReqTypeID, 0, "", 0, "", 0, "", 0, 0, 0, PriorityID, Type:=IsForPrint)
        End Select
        dgRequisitionItemList.PageIndex = 0
    End Sub
    Private Sub ClearControls()
        cmbReqType.SelectedIndex = 0
        txtNo.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub DataFieldBind()
        SearchIndex = IIf(IsNothing(SearchIndex), 0, SearchIndex)

        mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)")
        cmbRequisitionText.DataSource = mDistinctTextListForRequisition

        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder

        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("13", , True, "(All)")
        cmbReceipText.DataSource = mDistinctTextListForReceipt

        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue

        If FromDate Is Nothing And ToDate Is Nothing Then
            FromDate = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
            ToDate = Now.Date.ToString(AppSettings("DateFormat"))
        End If

        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate

        mrptRequisitionItemStatusList = rptRequisitionItemStatusList.GetRequisitionItemStatusList("", "", , txtFromDate.Text, txtToDate.Text, ClientCode:=AppSettings("ClientCode").ToString)

        dgRequisitionItemList.DataSource = mrptRequisitionItemStatusList
        dgRequisitionItemList.DataBind()

        'OrderType = IIf(IsNothing(OrderType), 0, OrderType)

        Session("SearchIndex") = SearchIndex
        Session("mDistinctTextListForRequisition") = mDistinctTextListForRequisition
        Session("mrptRequisitionItemStatusList") = mrptRequisitionItemStatusList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("BrancheIndex") = BranchIndex
        'Session("Name") = Name
        'Session("No") = No
        'Session("TransTypeID") = TransTypeID

        mRequisitionEngineeringBranchesList = RequisitionEngineeringBranchesList.GetRequisitionEngineeringBranchesList(TransTypeID, True, "(All)")
        cmbRequisitionEngineeringBranches.DataSource = mRequisitionEngineeringBranchesList

        'Added By Vikrant On 06-Sep-2018 For BA05092018
        mPriorityList = PriorityList.GetPriorityList(, , "(All)")
        Session("mPriorityList") = mPriorityList
        cmbPriority.DataSource = mPriorityList
        'End

        'Added by shital on 31-Oct-2019
        cmbRequisitionType.SelectedValue = 1
        ReqTypeID = cmbRequisitionType.SelectedValue
        Session("ReqTypeID") = ReqTypeID
        '-------

        DataBind()

        'RequisitionText = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedItem.Text)
        'TransTypeID = IIf(cmbReqType.SelectedIndex <= 0, "", cmbReqType.SelectedValue)

        'Session("RequisitionText") = RequisitionText


        lblResult.Text = "List of Requisition Item(s) as per Criteria :" & mrptRequisitionItemStatusList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32)
        cmbRequisitionText.Visible = IIf(SearchIndex = 1, True, False)
        cmbOrderText.Visible = IIf(SearchIndex = 4, True, False)
        cmbReceipText.Visible = IIf(SearchIndex = 5, True, False)
        cmbIssueText.Visible = IIf(SearchIndex = 6, True, False)
        lblNo.Visible = IIf((SearchIndex = 1 Or SearchIndex = 4 Or SearchIndex = 5 Or SearchIndex = 6) And (cmbRequisitionText.SelectedIndex <> 0 Or cmbOrderText.SelectedIndex <> 0 Or cmbReceipText.SelectedIndex <> 0 Or cmbIssueText.SelectedIndex <> 0), True, False)
        txtNo.Visible = IIf((SearchIndex = 1 Or SearchIndex = 4 Or SearchIndex = 5 Or SearchIndex = 6) And (cmbRequisitionText.SelectedIndex <> 0 Or cmbOrderText.SelectedIndex <> 0 Or cmbReceipText.SelectedIndex <> 0 Or cmbIssueText.SelectedIndex <> 0), True, False)
        txtName.Visible = IIf(SearchIndex = 2, True, False)
        cmbReqType.Visible = IIf(SearchIndex = 3, True, False)
        BtnPrint.Enabled = IIf(dgRequisitionItemList.Rows.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(dgRequisitionItemList.Rows.Count = 0, False, True)
        cmbRequisitionEngineeringBranches.Visible = IIf(SearchIndex = 3 And (cmbReqType.SelectedValue = 65 Or cmbReqType.SelectedValue = 72), True, False)
        lblBranch.Visible = IIf(SearchIndex = 3 And (cmbReqType.SelectedValue = 65 Or cmbReqType.SelectedValue = 72), True, False)
        cmbPriority.Visible = IIf(SearchIndex = 9, True, False) 'Added By Vikrant On 06-Sep-2018 For BA05092018
        If cmbFormat.SelectedValue = "1" And
           (AppSettings("ClientCode") = "APFT" Or
            AppSettings("ClientCode") = "STR" Or
            AppSettings("ClientCode") = "AAP") And
            AppSettings("ShowExportToExcelButton") = "True" Then
            btnExportToExcelTop.Visible = True
            btnExportToExcelTop.Visible = True
        Else
            btnExportToExcelTop.Visible = False
            btnExportToExcelBottom.Visible = False
        End If
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub SetControl()
        CallFindNow(SearchIndex)
        dgRequisitionItemList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate


        cmbRequisitionText.SelectedValue = IIf(RequisitionText = "", "(All)", RequisitionText)
        cmbOrderText.SelectedValue = IIf(OrderText = "", "(All)", OrderText)
        cmbReceipText.SelectedValue = IIf(ReceiptText = "", "(All)", ReceiptText)
        cmbIssueText.SelectedValue = IIf(IssueText = "", "(All)", IssueText)
        cmbReqType.SelectedValue = IIf(IsNothing(TransTypeID), 0, TransTypeID)
        txtName.Text = Name
        txtNo.Text = No
        cmbRequisitionEngineeringBranches.SelectedIndex = BranchIndex
        'If mDistinctTextListForRequisition.Contains(RequisitionText) Then
        '    cmbRequisitionText.SelectedValue = IIf(RequisitionText = "", "(All)", RequisitionText)
        'Else
        '    RequisitionText.SelectedValue = "(All)"
        'End If

        txtName.Text = Name
        txtNo.Text = No
        cmbPriority.SelectedValue = IIf(IsNothing(PriorityID), 0, PriorityID) 'Added By Vikrant On 06-Sep-2018 For BA05092018

        lblResult.Text = "List of Requisition Item(s) as per Criteria :" & mrptRequisitionItemStatusList.Count & " Record(s) found."
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        'Dim Rpt As New crptRequisitionItemStatus
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsRequisitionItemStatus
        Dim mCompanyDetail As New CompanyDetail

        If cmbFormat.SelectedIndex = 0 Then
            Rpt = New crptRequisitionItemStatus
        Else
            If AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then
                Rpt = New crptRequisitionItemStatusForAPFT
                CallFindNow(SearchIndex, 1)
            Else
                Rpt = New crptRequisitionItemStatusFormat2
            End If
        End If

        mrptRequisitionItemStatusList = Session("mrptRequisitionItemStatusList")

        If mrptRequisitionItemStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1298)
        End If
        'Dim mrptRequisitionItemStatusList As rptRequisitionItemStatusList
        'mrptRequisitionItemStatusList = rptRequisitionItemStatusList.GetRequisitionItemStatusList()
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Requisition Item Status Report", FromDate, ToDate, txtName.Text, cmbReqType.SelectedItem.Text, _
        cmbSearch.SelectedValue.ToString, AppSettings("Product Version"), AppSettings("SINote"), cmbSearch.SelectedValue.ToString, "", "", "", _
        AppSettings("Logo"))
        EventLogDetails = "Date Range " + FromDate + ", " + ToDate + ", " + "Req. Type " + cmbReqType.SelectedItem.Text
        If IsExcel = False Then         'PDF format
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mrptRequisitionItemStatusList)
            da.Fill(ds, Report)
            Rpt.SetDataSource(ds)
            Session("CrystalReport") = Rpt
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", "openTranDetail();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "RequisitionItemStatus", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            '-------------------------------------------------------------------------------------------
        Else                            'Excel format
            ds.Clear()
            da.Fill(ds, mrptRequisitionItemStatusList)
            da.Fill(ds, Report)

            Dim columnToRemove2 As String() = {"ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", _
                                               "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr3", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", _
                                               "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", _
                                               "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", _
                                               "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            Dim columnToRemove1 As String()

            If AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then
                columnToRemove1 = {"ReqID", "ReqTypeID", "ReqTypeName", "RequisitionEngineeringBranch", "TransTypeID", "TransTypeName",
                                   "ReqItemID", "WOID", "WONo", "MachineID", "ItemID", "WorkShopID", "WorkShopName", "IssueDetails",
                                   "ReceiptDetails", "OrderDetails", "LocationName", "Date", "Text", "No", "Remark", "Priority", "IssueDate",
                                   "Rate", "IssueDetailsForExportToExcel", "ReceiptDetailsForExportToExcel", "OrderDetailsForExportToExcel"}
            Else
                columnToRemove1 = {"ReqID", "ReqTypeID", "ReqTypeName", "RequisitionEngineeringBranch", "TransTypeID", "TransTypeName", _
                                   "ReqItemID", "WOID", "WONo", "MachineID", "ItemID", "WorkShopID", "WorkShopName", "IssueNo", "IssueDate", "IssueDetails", _
                                   "ReceiptDetails", "OrderDetails", "LocationName", "Date", "Text", "No", "IssueFormatted", "IssueRemark", "Priority", _
                                   "IssueDate", "Rate", "IssueQty"}
            End If

            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("rptRequisitionItemStatusList").Columns.Remove(columnToRemove1(i))
                End If
            Next

            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("PartNo") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("PartNo").ColumnName = "Part No."
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("PartDescription") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("PartDescription").ColumnName = "Description"
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("AlternatePart") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("AlternatePart").ColumnName = "Alternate Part"
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("ReqQty") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("ReqQty").ColumnName = "Req. Qty."
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("RequisitionTextNo") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("RequisitionTextNo").ColumnName = "Requisition No."
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("DateFormatted") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("DateFormatted").ColumnName = "Requisition Date"
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("MachineName") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("MachineName").ColumnName = "Aircraft"
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("MaintenanceType") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("MaintenanceType").ColumnName = "Maintenance Type"
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("EmployeeName") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("EmployeeName").ColumnName = "Requested By"
            End If
            If AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("IssueQty") Then
                    ds.Tables("rptRequisitionItemStatusList").Columns("IssueQty").ColumnName = "Issue Qty."
                End If
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("IssueNo") Then
                    ds.Tables("rptRequisitionItemStatusList").Columns("IssueNo").ColumnName = "Issue No."
                End If
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("IssueFormatted") Then
                    ds.Tables("rptRequisitionItemStatusList").Columns("IssueFormatted").ColumnName = "Issue Date"
                End If
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("IssueRemark") Then
                    ds.Tables("rptRequisitionItemStatusList").Columns("IssueRemark").ColumnName = "Issue Remark"
                End If
            Else
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("OrderDetailsForExportToExcel") Then
                    ds.Tables("rptRequisitionItemStatusList").Columns("OrderDetailsForExportToExcel").ColumnName = "Order Details"
                End If
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("IssueDetailsForExportToExcel") Then
                    ds.Tables("rptRequisitionItemStatusList").Columns("IssueDetailsForExportToExcel").ColumnName = "Issue Details"
                End If
                If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("ReceiptDetailsForExportToExcel") Then
                    ds.Tables("rptRequisitionItemStatusList").Columns("ReceiptDetailsForExportToExcel").ColumnName = "Receipt Details"
                End If
            End If

            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("SerialNo") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("IssuedBy") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("IssuedBy").ColumnName = "Issued By"
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("StockQty") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("StockQty").ColumnName = "Stock Qty."
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("BinLocation") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("BinLocation").ColumnName = "Bin Location"
            End If
            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("EffRate") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("EffRate").ColumnName = "Effective Rate"
            End If

            If ds.Tables("rptRequisitionItemStatusList").Columns.Contains("ReturnQty") Then
                ds.Tables("rptRequisitionItemStatusList").Columns("ReturnQty").ColumnName = "Return Qty."
            End If

            ds.Tables("rptRequisitionItemStatusList").Columns("Part No.").SetOrdinal(0)
            ds.Tables("rptRequisitionItemStatusList").Columns("Description").SetOrdinal(1)
            ds.Tables("rptRequisitionItemStatusList").Columns("Alternate Part").SetOrdinal(2)
            ds.Tables("rptRequisitionItemStatusList").Columns("Req. Qty.").SetOrdinal(3)
            ds.Tables("rptRequisitionItemStatusList").Columns("Requisition No.").SetOrdinal(4)
            ds.Tables("rptRequisitionItemStatusList").Columns("Requisition Date").SetOrdinal(5)
            ds.Tables("rptRequisitionItemStatusList").Columns("Aircraft").SetOrdinal(6)
            ds.Tables("rptRequisitionItemStatusList").Columns("Maintenance Type").SetOrdinal(7)
            ds.Tables("rptRequisitionItemStatusList").Columns("Requested By").SetOrdinal(8)

            ds.Tables("rptRequisitionItemStatusList").Columns("Return Qty.").SetOrdinal(10)
            If AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then
                ds.Tables("rptRequisitionItemStatusList").Columns("Issue Qty.").SetOrdinal(9)
                ds.Tables("rptRequisitionItemStatusList").Columns("Issue No.").SetOrdinal(11)
                ds.Tables("rptRequisitionItemStatusList").Columns("Issue Date").SetOrdinal(12)
                ds.Tables("rptRequisitionItemStatusList").Columns("Issued By").SetOrdinal(14)
                ds.Tables("rptRequisitionItemStatusList").Columns("Stock Qty.").SetOrdinal(15)
                ds.Tables("rptRequisitionItemStatusList").Columns("Bin Location").SetOrdinal(16)
                ds.Tables("rptRequisitionItemStatusList").Columns("Effective Rate").SetOrdinal(17)
                ds.Tables("rptRequisitionItemStatusList").Columns("Issue Remark").SetOrdinal(18)
            Else
                ds.Tables("rptRequisitionItemStatusList").Columns("Order Details").SetOrdinal(9)
                ds.Tables("rptRequisitionItemStatusList").Columns("Issue Details").SetOrdinal(11)
                ds.Tables("rptRequisitionItemStatusList").Columns("Receipt Details").SetOrdinal(12)
                ds.Tables("rptRequisitionItemStatusList").Columns("Serial No.").SetOrdinal(13)
                ds.Tables("rptRequisitionItemStatusList").Columns("Stock Qty.").SetOrdinal(14)
                ds.Tables("rptRequisitionItemStatusList").Columns("Bin Location").SetOrdinal(15)
                ds.Tables("rptRequisitionItemStatusList").Columns("Effective Rate").SetOrdinal(16)
                ds.Tables("rptRequisitionItemStatusList").Columns("Remark").SetOrdinal(17)
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Merge(ds.Tables("rptRequisitionItemStatusList"))
            dsNew.Tables("rptRequisitionItemStatusList").TableName = "Requisition Item Status Report"
			Session("ExcelFileName") = "Requisition Item Status Report"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "RequisitionItemStatus", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbSearch.Enabled = True Then
                cmbSearch.Focus()
            End If
            Session("MiddleFrame") = "wfrptRequisitionItemStatus_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            ControlVisibility(SearchIndex)
        End If
    End Sub
    Private Sub dgRequisitionItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRequisitionItemList.PageIndexChanging
        dgRequisitionItemList.PageIndex = e.NewPageIndex
        dgRequisitionItemList.DataSource = mrptRequisitionItemStatusList
        Session("mrptRequisitionItemStatusList") = mrptRequisitionItemStatusList
        dgRequisitionItemList.DataBind()
    End Sub
    Private Sub dgRequisitionItemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgRequisitionItemList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ReqItemID As Guid = (DataBinder.Eval(e.Row.DataItem, "ReqItemID"))
            Dim dgTransactionDetails As GridView = DirectCast(e.Row.FindControl("dgTransactionDetails"), GridView)
            'AddHandler dgTransactionDetails.RowCommand, AddressOf dgTransactionDetails_RowCommand

            Dim mRequisitionItemTransactionDetails As RequisitionItemTransactionDetails = RequisitionItemTransactionDetails.GetRequisitionItemTransactionDetails(ReqItemID.ToString, chkShowPPReqOnly.Checked, AppSettings("ClientCode").ToString)
            dgTransactionDetails.DataSource = mRequisitionItemTransactionDetails
            dgTransactionDetails.DataBind()

            If mRequisitionItemTransactionDetails.Count > 0 Then
                e.Row.Cells(0).BackColor = Color.Yellow
            End If
        End If
    End Sub
    Protected Sub dgTransactionDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "TranasactionNo"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim id As Guid = New Guid(CType(sender, GridView).DataKeys(index).Item(0).ToString)
                Dim Type As Integer = CType(sender, GridView).DataKeys(index).Item(1)
                Dim InvoiceID As Guid = New Guid(CType(sender, GridView).DataKeys(index).Item(2).ToString)
                Select Case Type
                    Case 1 'Enquiry
                        mEnquiry = Enquiry.GetEnquiry(id)
                        Session("mEnquiry") = mEnquiry
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfEnquiry_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 2 'Quotation
                        mQuotation = Quotation.GetQuotation(id)
                        Session("mQuotation") = mQuotation
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfQuotation_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 3 'Order
                        mOrder = Order.GetOrder(id)
                        Session("mOrder") = mOrder
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPurchaseOrder_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 4 'Receipt
                        mReceipt = Receipt.GetReceipt(id)
                        Session("mReceipt") = mReceipt
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfReceipt_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 5 'RCI
                        mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(id, InvoiceID)
                        Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfReceiptCumInvoice_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 6 'Issue
                        mIssue = Issue.GetIssue(id)
                        Session("mIssue") = mIssue
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfIssue_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                End Select
        End Select
    End Sub
    Private Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click, btnPrintTop.Click
        SetReport(False)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        SearchIndex = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        RequisitionText = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedItem.Text)
        OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedItem.Text)
        ReceiptText = IIf(cmbReceipText.SelectedIndex <= 0, "", cmbReceipText.SelectedItem.Text)
        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedItem.Text)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        TransTypeID = IIf(cmbReqType.SelectedIndex <= 0, 0, cmbReqType.SelectedValue)
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        BranchIndex = cmbRequisitionEngineeringBranches.SelectedIndex.ToString
        'Added By Vikrant On 06-Sep-2018 For BA05092018
        PriorityID = IIf(cmbPriority.SelectedIndex <= 0, 0, cmbPriority.SelectedValue)
        Session("PriorityID") = PriorityID
        'End

        'Added by shital on 31-Oct-2019
        ReqTypeID = IIf(cmbRequisitionType.SelectedIndex <= 0, 0, cmbRequisitionType.SelectedValue)
        Session("ReqTypeID") = ReqTypeID
        '-------------

        Session("SearchIndex") = SearchIndex
        Session("RequisitionText") = RequisitionText
        Session("OrderText") = OrderText
        Session("ReceiptText") = ReceiptText
        Session("IssueText") = IssueText
        Session("Name") = Name
        Session("No") = No
        Session("TransTypeID") = TransTypeID
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("BrancheIndex") = BranchIndex

        CallFindNow(SearchIndex)
        dgRequisitionItemList.DataBind()
        ControlVisibility(SearchIndex)
        lblResult.Text = "List of Requisition Item(s) as per Criteria :" & mrptRequisitionItemStatusList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgRequisitionItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRequisitionItemList.Sorting
        mrptRequisitionItemStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mrptRequisitionItemStatusList") = mrptRequisitionItemStatusList
        dgRequisitionItemList.DataSource = mrptRequisitionItemStatusList
        dgRequisitionItemList.DataBind()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbRequisitionText.SelectedIndex = 0
        cmbOrderText.SelectedIndex = 0
        cmbReceipText.SelectedIndex = 0
        cmbIssueText.SelectedIndex = 0
        cmbPriority.ClearSelection() 'Added By Vikrant On 06-Sep-2018 For BA05092018
        ClearControls()
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If
    End Sub
    Private Sub cmbRequisitionText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbRequisitionText.SelectedIndexChanged
        ClearControls()
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbRequisitionText.Enabled = True Then
            cmbRequisitionText.Focus()
        End If
    End Sub
    Private Sub cmbReqType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbReqType.SelectedIndexChanged
        If cmbReqType.SelectedValue = 65 Or cmbReqType.SelectedValue = 72 Then
            lblBranch.Visible = True
            cmbRequisitionEngineeringBranches.Visible = True
        Else
            lblBranch.Visible = False
            cmbRequisitionEngineeringBranches.Visible = False
        End If
        mRequisitionEngineeringBranchesList = RequisitionEngineeringBranchesList.GetRequisitionEngineeringBranchesList(cmbReqType.SelectedValue, True, "(All)")
        cmbRequisitionEngineeringBranches.DataSource = mRequisitionEngineeringBranchesList
        cmbRequisitionEngineeringBranches.DataBind()
    End Sub
    Private Sub cmbOrderText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbOrderText.SelectedIndexChanged
        ClearControls()
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbOrderText.Enabled = True Then
            cmbOrderText.Focus()
        End If
    End Sub
    Private Sub cmbReceipText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbReceipText.SelectedIndexChanged
        ClearControls()
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbReceipText.Enabled = True Then
            cmbReceipText.Focus()
        End If
    End Sub
    Private Sub cmbIssueText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbIssueText.SelectedIndexChanged
        ClearControls()
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbIssueText.Enabled = True Then
            cmbIssueText.Focus()
        End If
    End Sub
    Private Sub btnExportToExcelTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToExcelTop.Click, btnExportToExcelBottom.Click
        SetReport(True)
    End Sub
    Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedValue = "1" And
           (AppSettings("ClientCode") = "APFT" Or
            AppSettings("ClientCode") = "STR" Or
            AppSettings("ClientCode") = "AAP") And
           AppSettings("ShowExportToExcelButton") = "True" Then
            btnExportToExcelTop.Visible = True
            btnExportToExcelBottom.Visible = True
        Else
            btnExportToExcelTop.Visible = False
            btnExportToExcelBottom.Visible = False
        End If
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
#End Region


End Class