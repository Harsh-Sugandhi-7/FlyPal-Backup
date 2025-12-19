Public Class wfInvoiceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mInvoiceList As InvoiceList
    Public mInvoice As Invoice
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mDistinctTextListForInvoice As DistinctTextListForInvoice
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim SearchIndex, PeriodIndex, FromDate, ToDate, StatusId, OrderText, ReceiptText, InvoiceText, PartNoSearch, No, _
        ReceiptNoSearchForInvoice, OrderNoSearchForInvoice, SupplierInvoiceNoSearch, SupplierSearchForInvoice, SearchText As String
    Public mTransTypeID As Trans
    Private mInvoiceTypeList As InvoiceTypeList 'Changes by Kalpesh Shah as on 23-01-2008
    Private mSearchInvoiceTypeList As InvoiceTypeList
    Dim InvoiceTypeID As Int16 '----
    Public mName As String
    Dim EventLogID As Guid      'Added By Utkarsh On 21-Jul-2011 For All19072011
    Dim InvDetail As String
    Dim mModuleName As String   'End
    'Dim mTransactionListCount As TransactionListCount  'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Dim mFileAttach As FileAttach
    Dim stringInfo As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mInvoice = Session("mInvoice")
        mInvoiceList = Session("mInvoiceList")
        mDistinctTextListForOrder = Session("mDistinctTextListForOrder")
        mDistinctTextListForReceipt = Session("mDistinctTextListForReceipt")
        mDistinctTextListForInvoice = Session("mDistinctTextListForInvoice")
        mInvoiceTypeList = Session("mInvoiceTypeList") 'Changes by Kalpesh Shah as on 23-01-2008
        mSearchInvoiceTypeList = Session("mSearchInvoiceTypeList") '--
        SearchIndex = Session("SearchIndex")
        PeriodIndex = Session("PeriodIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        OrderText = Session("OrderText")
        ReceiptText = Session("ReceiptText")
        InvoiceText = Session("InvoiceText")
        PartNoSearch = Session("PartNoSearch")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        mTransTypeID = Session("mTransTypeId")
        InvoiceTypeID = Session("InvoiceTypeID") 'Changes by Kalpesh Shah as on 23-01-2008
        mModuleName = Session("mModuleName") 'Added By Utkarsh On 21-Jul-2011 For All19072011
        'mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1

        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        mFileAttach = Session("mFileAttach")

        ReceiptNoSearchForInvoice = IIf(IsNothing(Session("ReceiptNoSearchForInvoice")), 0, Session("ReceiptNoSearchForInvoice"))
        OrderNoSearchForInvoice = IIf(IsNothing(Session("OrderNoSearchForInvoice")), 0, Session("OrderNoSearchForInvoice"))
        SupplierInvoiceNoSearch = Session("SupplierInvoiceNoSearch")
        SupplierSearchForInvoice = Session("SupplierSearchForInvoice")
        SearchText = Session("SearchTextInvoiceList")
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mInvoiceList")
        Session.Remove("mDistinctTextListForOrder")
        Session.Remove("mDistinctTextListForReceipt")
        Session.Remove("mDistinctTextListForInvoice")
        Session.Remove("mTransTypeId")
        Session.Remove("mInvoiceTypeList") 'Changes by Kalpesh Shah as on 23-01-2008
        Session.Remove("mSearchInvoiceTypeList") '--
        Session.Remove("mModuleName") 'Added By Utkarsh On 21-Jul-2011 For All19072011
        'Session.Remove("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        Session.Remove("mFileAttach")
        Session.Remove("ReceiptNoSearchForInvoice")
        Session.Remove("OrderNoSearchForInvoice")
        Session.Remove("SupplierInvoiceNoSearch")
        Session.Remove("SupplierSearchForInvoice")
        Session.Remove("SearchTextInvoiceList")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfInvoiceList_Ajax.aspx?") <= 0 Then
            Session.Remove("mInvoiceList")
            Session.Remove("mDistinctTextListForOrder")
            Session.Remove("mDistinctTextListForReceipt")
            Session.Remove("mDistinctTextListForInvoice")
            Session.Remove("SearchIndex")
            Session.Remove("PeriodIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("OrderText")
            Session.Remove("InvoiceText")
            Session.Remove("ReceiptText")
            Session.Remove("PartNoSearch")
            Session.Remove("No")
            Session.Remove("mInvocieTypeList") 'Changes by Kalpesh Shah as on 23-01-2008
            Session.Remove("mSearchInvocieTypeList")
            Session.Remove("InvocieTypeID") '--
            Session.Remove("mCurrentpage")
            Session.Remove("mpageSize")
            Session.Remove("mpageindex")
            Session.Remove("pagecount")
            Session.Remove("totalCount")
            Session.Remove("mFileAttach")
            Session.Remove("ReceiptNoSearchForInvoice")
            Session.Remove("OrderNoSearchForInvoice")
            Session.Remove("SupplierInvoiceNoSearch")
            Session.Remove("SupplierSearchForInvoice")
            Session.Remove("SearchTextInvoiceList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        mTransTypeID = CType(cmbInvoiceType.SelectedValue, Util.Trans)
        Session("mTransTypeId") = mTransTypeID
        mInvoice = Invoice.NewInvoice(mTransTypeID)
        mInvoice.InvoiceDate = Today.Date
        Session("mInvoice") = mInvoice
        'mFileAttach = FileAttach.NewAttachment(Guid.Empty, mInvoice.ID)
        'Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mInvoice = Invoice.GetInvoice(mID)
        Session("mInvoice") = mInvoice
        'If mInvoice.IsAttachmentAdded Then
        '    mFileAttach = FileAttach.GetAttachment(mInvoice.ID)
        'Else
        '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mInvoice.ID)
        'End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        GridBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mInvoice = Invoice.GetInvoice(mID)
        Session("mInvoice") = mInvoice
    End Sub
    Private Sub SetControl()
        SetPeriod(PeriodIndex)

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgInvoiceList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgInvoiceList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        CallFindNow(SearchIndex)

        dgInvoiceList.DataBind()

        'cmbSearchCriteria.SelectedIndex = SearchIndex
        cmbPeriod.SelectedIndex = PeriodIndex
        cmbStatus.SelectedValue = StatusId
        cmbSearchInvoiceType.SelectedValue = CStr(InvoiceTypeID)
        If cmbInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(InvoiceText)) Then
            cmbInvoiceText.SelectedValue = InvoiceText
        Else
            cmbInvoiceText.SelectedValue = "(All)"
        End If
        If cmbOrderText.Items.Contains(New System.Web.UI.WebControls.ListItem(OrderText)) Then
            cmbOrderText.SelectedValue = OrderText
        Else
            cmbOrderText.SelectedValue = "(All)"
        End If
        If cmbReceiptText.Items.Contains(New System.Web.UI.WebControls.ListItem(ReceiptText)) Then
            cmbReceiptText.SelectedValue = ReceiptText
        Else
            cmbReceiptText.SelectedValue = "(All)"
        End If
        txtPartNoSearch.Text = PartNoSearch
        txtInvoiceNo.Text = No
        txtReceipNo.Text = ReceiptNoSearchForInvoice
        txtOrderNo.Text = OrderNoSearchForInvoice
        txtSupplierInvoiceNo.Text = SupplierInvoiceNoSearch
        txtSupplier.Text = SupplierSearchForInvoice
        ControlVisibility(SearchIndex, PeriodIndex)
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub ClearControl()
        'txtName.Text = ""
        txtInvoiceNo.Text = ""
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mVendorName As String
                        Try
                            mInvoice = CType(Session("mInvoice"), Invoice)
                            mVendorName = mInvoiceList(mInvoice.ID).VendorName
                            If mInvoice.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mInvoice.ID)
                            End If
                            If AppSettings("LockBackDatedTransaction") = "True" Then
                                If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                                    'Do nothing
                                Else
                                    Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
                                    Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
                                    If (CDate(mInvoice.InvoiceDate) >= FirstDayofLastMonth) Then
                                        If (CDate(mInvoice.InvoiceDate) < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                                            MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mInvoice.InvoiceDate).AddMonths(1)), Month(CDate(mInvoice.InvoiceDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                            Exit Sub
                                        End If
                                    Else
                                        MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mInvoice.InvoiceDate).AddMonths(1)), Month(CDate(mInvoice.InvoiceDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                        Exit Sub
                                    End If
                                End If
                            End If
                            'End
                            Dim mTransTypeList As TransactionList
                            mTransTypeList = TransactionList.GetTransactionList()
                            mModuleName = mTransTypeList.GetTransactionTypeName(mInvoice.TransTypeID).ToString
                            Session("mModuleName") = mModuleName

                            mInvoice.Delete()
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mInvoice.Save()
                            DataFieldBind()

                            SetControl()
                            UpdateItemGridView()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                If ex.Message.Contains("tabOtherChargeInvoices") Then
                                    stringInfo = "Other Charge Invoices."
                                ElseIf ex.Message.Contains("tabPaymentItem") Then
                                    stringInfo = "Payment."
                                ElseIf ex.Message.Contains("tabInvoiceCharge") Then
                                    stringInfo = "Invoice Charge."
                                End If
                                InvDetail = mInvoice.InvoiceNo + " Dated : " + mInvoice.InvoiceDateFormatted + " from " + mInvoiceList(mInvoice.ID).VendorName
                                MarkLog(Util.Action.Delete, mModuleName, "Can't delete : " & InvDetail & " is Currently in use", Util.ErrorType.NoError, mInvoice.ID, EventLogID)
                                ' MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 50000 Then
                                'MarkLog(Util.Action.Delete, mModuleName, "Can't delete : " & mEnquiryDetail & " is Currently in use", Util.ErrorType.NoError, mEnquiry.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            'SetTitle()
                            upnlFindNow.Update()
                            If msgCount = 0 Then
                                InvDetail = mInvoice.InvoiceNo + "," + " Dated : " + mInvoice.InvoiceDateFormatted + "," + " from :  " + mVendorName
                                MarkLog(Util.Action.Delete, mModuleName, InvDetail, Util.ErrorType.NoError, mInvoice.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                        SetGrid()
                    End If
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal InvoiceText As String = "", Optional ByVal InvoiceNo As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", _
                        Optional ByVal ToDate As String = "1/1/2200", Optional ByVal ReceiptText As String = "", Optional ByVal ReceiptNo As Integer = 0, _
                        Optional ByVal OrderText As String = "", Optional ByVal OrderNo As Integer = 0, Optional ByVal VendorName As String = "", _
                        Optional ByVal ItemName As String = "", Optional ByVal StatusID As Integer = 0, Optional ByVal tmpTransTypeID As Util.Trans = Util.Trans.None, _
                        Optional ByVal IsForPrint As Boolean = False, Optional ByVal SupplierInvoiceNo As String = "", Optional ByVal SearchText As String = "")
        'clear the obj and grid
        mInvoiceList = Nothing
        dgInvoiceList.DataSource = Nothing
        If IsForPrint = True Then
            mInvoiceList = InvoiceList.GetInvoiceList(InvoiceText, InvoiceNo, FromDate, ToDate, ReceiptText, ReceiptNo, OrderText, OrderNo, VendorName, ItemName, StatusID, tmpTransTypeID, False, CurrentPage:=mpageindex, PageSize:=mpageSize, SupplierInvoiceNo:=SupplierInvoiceNo, SearchText:=SearchText)
            Exit Sub
        Else
            mInvoiceList = InvoiceList.GetInvoiceList(InvoiceText, InvoiceNo, FromDate, ToDate, ReceiptText, ReceiptNo, OrderText, OrderNo, VendorName, ItemName, StatusID, tmpTransTypeID, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize, SupplierInvoiceNo:=SupplierInvoiceNo, SearchText:=SearchText)
        End If

        'bind the list to the datagrid
        'set the session
        totalCount = mInvoiceList.TotalRecords
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount
        dgInvoiceList.DataSource = mInvoiceList
        dgInvoiceList.DataBind()
        Session("mInvoiceList") = mInvoiceList
        UpdateItemGridView()
    End Sub
    Private Sub CallFindNow(ByVal indx As Int32, Optional ByVal IsForPrint As Boolean = False)
        FindNow(InvoiceText:=Trim(InvoiceText), InvoiceNo:=CInt(Val(No)), FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, _
                 ReceiptText:=Trim(ReceiptText), ReceiptNo:=Trim(ReceiptNoSearchForInvoice), OrderText:=Trim(OrderText), _
                 OrderNo:=CInt(Val(OrderNoSearchForInvoice)), VendorName:=Trim(SupplierSearchForInvoice), ItemName:=Trim(PartNoSearch), StatusID:=CInt(StatusId), _
                 tmpTransTypeID:=InvoiceTypeID, IsForPrint:=IsForPrint, SupplierInvoiceNo:=Trim(SupplierInvoiceNoSearch), SearchText:=SearchText)
        'Select Case indx
        '    Case -1
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", 0, , IsForPrint:=IsForPrint) 'for all records
        '    Case 0  'all
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", 0, , IsForPrint:=IsForPrint) 'for all records
        '    Case 1 'Order date
        '        Call FindNow("", 0, txtFromDate.Text, txtToDate.Text, "", 0, "", 0, "", "", 0, , IsForPrint:=IsForPrint)
        '    Case 2  'Invoice Text 
        '        Call FindNow(InvoiceText, CInt(Val(No)), FromDate, ToDate, "", 0, "", 0, "", "", 0, , IsForPrint:=IsForPrint)
        '    Case 3 ' Part No 
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", Name, 0, , IsForPrint:=IsForPrint)
        '    Case 4 ' Vendor Name
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, Name, "", 0, , IsForPrint:=IsForPrint)
        '    Case 5  'Order Text 
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, OrderText, CInt(Val(No)), "", "", 0, , IsForPrint:=IsForPrint)
        '    Case 6  'Receipt Text 
        '        Call FindNow("", 0, FromDate, ToDate, ReceiptText, CInt(Val(No)), "", 0, "", "", 0, , IsForPrint:=IsForPrint)
        '        'Changes by Kalpesh Shah as on 23-01-2008
        '        'Status' was 7 which is shifted to 8. Now 7 is 'Invoice Type' 
        '    Case 7 'Invoice Type
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", 0, CType(cmbSearchInvoiceType.SelectedValue, Util.Trans), IsForPrint:=IsForPrint)
        '    Case 8  'Status Text 
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", CInt(StatusId), , IsForPrint:=IsForPrint)
        '    Case 9  'Supplier Invoice No. 
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", 0, , IsForPrint:=IsForPrint, SupplierInvoiceNo:=Name)
        'End Select
        dgInvoiceList.PageIndex = 0   'Added Code on MAy,2007  
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal PeriodIndex As Int32 = 0, Optional ByVal RectTxt As Int32 = 0, Optional ByVal Ordtxt As Int32 = 0)
        'cmbPeriod.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = IIf(PeriodIndex <> 0, True, False)
        lblToDate.Visible = IIf(PeriodIndex <> 0, True, False)
        'txtFromDate.Visible = IIf(SearchIndex = 1 And PeriodIndex <> 0, True, False)
        'txtToDate.Visible = IIf(SearchIndex = 1 And PeriodIndex <> 0, True, False)
        'cmbInvoiceText.Visible = IIf(SearchIndex = 2, True, False)
        'cmbOrderText.Visible = IIf(SearchIndex = 5, True, False)
        'cmbReceiptText.Visible = IIf(SearchIndex = 6, True, False)
        'txtInvoiceNo.Visible = IIf(SearchIndex = 2 And cmbInvoiceText.SelectedIndex <> 0 Or SearchIndex = 5 And cmbOrderText.SelectedIndex <> 0 Or SearchIndex = 6 And cmbReceiptText.SelectedIndex <> 0, True, False)
        'lblNo.Visible = IIf(SearchIndex = 2 And cmbInvoiceText.SelectedIndex <> 0 Or SearchIndex = 5 And cmbOrderText.SelectedIndex <> 0 Or SearchIndex = 6 And cmbReceiptText.SelectedIndex <> 0, True, False)
        'txtName.Visible = IIf(SearchIndex = 3 Or SearchIndex = 4 Or SearchIndex = 9, True, False)
        'Changes by Kalpesh Shah as on 23-01-2008
        'cmbStatus.Visible = IIf(SearchIndex = 7, True, False)
        'Me.cmbSearchInvoiceType.Visible = CBool(IIf(SearchIndex = 7, True, False))
        'cmbStatus.Visible = CBool(IIf(SearchIndex = 8, True, False))
        '----------------------------------------
        If PeriodIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (PeriodIndex = 1 Or PeriodIndex = 2 Or PeriodIndex = 3 Or PeriodIndex = 4 Or PeriodIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub SetPeriod(ByVal index As Int32) 'CNDC
        Select Case index
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
                FromDate = IIf(PeriodIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(PeriodIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearchCriteria.SelectedIndex < 0, 0, cmbSearchCriteria.SelectedIndex)
        PeriodIndex = IIf(cmbPeriod.SelectedIndex < 0, 0, cmbPeriod.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue)
        ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)
        InvoiceText = IIf(cmbInvoiceText.SelectedIndex <= 0, "", cmbInvoiceText.SelectedValue)
        InvoiceTypeID = IIf(cmbSearchInvoiceType.SelectedIndex <= 0, 0, cmbSearchInvoiceType.SelectedValue)

        PartNoSearch = txtPartNoSearch.Text.Trim
        No = txtInvoiceNo.Text.Trim
        ReceiptNoSearchForInvoice = txtReceipNo.Text.Trim
        OrderNoSearchForInvoice = txtOrderNo.Text.Trim
        SupplierInvoiceNoSearch = txtSupplierInvoiceNo.Text.Trim
        SupplierSearchForInvoice = txtSupplier.Text.Trim
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text)

        Session("ReceiptNoSearchForInvoice") = ReceiptNoSearchForInvoice
        Session("OrderNoSearchForInvoice") = OrderNoSearchForInvoice
        Session("SupplierInvoiceNoSearch") = SupplierInvoiceNoSearch
        Session("SupplierSearchForInvoice") = SupplierSearchForInvoice
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("PeriodIndex") = PeriodIndex
        Session("StatusId") = StatusId
        Session("OrderText") = OrderText
        Session("ReceiptText") = ReceiptText
        Session("InvoiceText") = InvoiceText
        Session("No") = No
        Session("PartNoSearch") = PartNoSearch
        Session("InvoiceTypeID") = InvoiceTypeID
        Session("SearchTextInvoiceList") = SearchText
    End Sub
    Private Sub addAttributes()
        txtInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetGrid()
        'BtnPrint.Enabled = IIf(dgInvoiceList.Rows.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(dgInvoiceList.Rows.Count = 0, False, True)
    End Sub
    Private Sub SetTitle() 'Added By Utkarsh On 21-Jul-2011 For All19072011
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        mModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        Session("mModuleName") = mModuleName
        'lblList.Text = "List of Invoice" + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString + "]"
        lblList.Text = "List of Invoice"
        upnlTitle.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        PeriodIndex = IIf(IsNothing(PeriodIndex), 1, PeriodIndex)
        StatusId = Session("StatusId")
        OrderText = Session("OrderText")
        ReceiptText = Session("ReceiptText")
        InvoiceText = Session("InvoiceText")
        InvoiceTypeID = Session("InvoiceTypeID") 'Changes by Kalpesh Shah as on 23-01-2008
        PartNoSearch = Session("PartNoSearch")
        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("2", , True, "(All)")
        cmbReceiptText.DataSource = mDistinctTextListForReceipt
        mDistinctTextListForInvoice = DistinctTextListForInvoice.GetDistinctTextListForInvoice("15", , True, "(All)")
        cmbInvoiceText.DataSource = mDistinctTextListForInvoice
        'mInvoiceList = InvoiceList.GetInvoiceList("", 0, "1/1/1900", "1/1/2200", "", 0, "", 0, "", "", 0, mTransTypeID)
        'mTransactionListCount = TransactionListCount.GetTransactionListCountt(21) 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
        'Session("mTransactionListCount") = mTransactionListCount 'End
        'mSearchInvoiceTypeList = InvoiceTypeList.GetSimpleInvoiceTypeList() 'Changes by Kalpesh Shah as on 23-01-2008
        'cmbSearchInvoiceType.DataSource = mSearchInvoiceTypeList
        mInvoiceTypeList = InvoiceTypeList.GetSimpleInvoiceTypeList()
        cmbInvoiceType.DataSource = mInvoiceTypeList '---

        'dgInvoiceList.DataSource = mInvoiceList
        'Session("mInvoiceList") = mInvoiceList
        DataBind()
    End Sub
    Private Sub GridBind()
        dgInvoiceList.DataSource = mInvoiceList
        dgInvoiceList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = "As per criteria :" & totalCount & " Record(s) found."
        Else
            lblResult.Text = "As per criteria :" & currentrow + 1 & " to " & currentrow + mInvoiceList.Count & " of " & totalCount & " Record(s) found."
        End If
        SliderExtender1.Minimum = 1
        SliderExtender1.Maximum = pagecount
        Slidercontrol.Text = mCurrentpage
        txtPageDisplay.Text = mCurrentpage
        lblpagecount.Text = pagecount
        If pagecount > 1 Then
            PnlPaging.Visible = True
        Else
            PnlPaging.Visible = False
        End If
        dgInvoiceList.DataBind()
        SetGrid()
        upnlGridView.Update()
        upnlResult.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)         'Added By Utkarsh On 20-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbPeriod.Enabled = True Then
                setFocus(cmbPeriod)
            End If
            mTransTypeID = Request.QueryString("TransTypeId")   'Added By Utkarsh On 21-Jul-2011 For All19072011
            Session("mTransTypeId") = mTransTypeID 'End
            Session("MiddleFrame") = "wfInvoiceList_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            SetGrid()
            SetTitle()
        End If
    End Sub
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click ', btnAddNew.Click
        NewRecord()
        If (Not User.IsInRole("InvoiceNew")) And (Not User.IsInRole("InvoiceEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Session("OpenFrom") = "1"
        'Changed By Utkarsh On 21-Jul-2011 For All19072011
        SetTitle()
        MarkLog(Util.Action.[New], "Purchase Invoice", "", Util.ErrorType.NoError, mInvoice.ID, EventLogID)
        'End
        'If (mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 1 And mInvoice.IsNew) Then
        If (mInvoice.InvoiceItems.Count = 0) Then
            Session("mPrevTransID") = Guid.Empty
            Session("mOrderTranstypeID") = 0
        Else
            Session("mPrevTransID") = mInvoice.InvoiceItems.Item(mInvoice.InvoiceItems.Count - 2).ItemDetailForInvoice.ReceiptID
            Session("mOrderTranstypeID") = mInvoice.InvoiceItems.Item(mInvoice.InvoiceItems.Count - 2).ItemDetailForInvoice.OrderTranstypeID
        End If
        Session("mTransaction") = 5  'Transaction.Receipt
        Dim str As String
        str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType=3&ChildPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub dgInvoiceList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInvoiceList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument) '+ dgInvoiceList.PageIndex * dgInvoiceList.PageSize
                Dim mID As Guid = mInvoiceList(index).ID
                If (Not User.IsInRole("InvoiceView") And Not User.IsInRole("InvoiceEdit")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                GridBind()
                EditRecord(mID)
                'Changed By Utkarsh On 21-Jul-2011 For All19072011
                mTransTypeID = mInvoice.TransTypeID
                SetTitle()
                InvDetail = mInvoice.InvoiceNo + " Dated : " + mInvoice.InvoiceDateFormatted + " from " + mInvoiceList(mInvoice.ID).VendorName
                MarkLog(Util.Action.Edit, mModuleName, InvDetail, Util.ErrorType.NoError, mInvoice.ID, EventLogID)
                'End
                Dim str As String
                str = "openledgersame('wfInvoice_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                Dim index As Integer = CInt(e.CommandArgument) '+ dgInvoiceList.PageIndex * dgInvoiceList.PageSize
                Dim mID As Guid = mInvoiceList(index).ID
                If (Not User.IsInRole("InvoiceDelete")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                GridBind()
                DeleteRecord(mID)
            Case "ViewRec" '=====================Added By Saylee on 8th Aug 2007=================
                If (Not User.IsInRole("InvoiceAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                GridBind()
                Dim index As Integer = CInt(e.CommandArgument) '+ dgInvoiceList.PageIndex * dgInvoiceList.PageSize
                Dim mID As Guid = mInvoiceList(index).ID
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                End If
                dgInvoiceList.DataSource = mInvoiceList
                dgInvoiceList.DataBind()
        End Select
        SetGrid()
    End Sub
    Private Sub dgInvoiceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgInvoiceList.PageIndexChanging
        dgInvoiceList.PageIndex = e.NewPageIndex
        mCurrentpage = e.NewPageIndex
        GridBind()
        UpdateItemGridView()
        Session("mInvoiceList") = mInvoiceList
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        dgInvoiceList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex)
        dgInvoiceList.DataBind()
        SetGrid()
        'BtnPrint.Enabled = IIf(mInvoiceList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mInvoiceList.Count = 0, False, True)
        upnlGridView.Update()
        upnTopButtons.Update()
        'upnBottomButtons.Update()
    End Sub
    'Private Sub cmbSearchCriteria_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchCriteria.SelectedIndexChanged
    '    cmbPeriod.SelectedIndex = 0
    '    cmbReceiptText.SelectedIndex = 0
    '    cmbOrderText.SelectedIndex = 0
    '    cmbInvoiceType.SelectedIndex = 0
    '    cmbStatus.SelectedIndex = 0
    '    cmbInvoiceText.SelectedIndex = 0
    '    cmbSearchInvoiceType.SelectedIndex = 0
    '    ClearControl()
    '    Dim PeriodIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0 And cmbPeriod.Visible, cmbPeriod.SelectedIndex, 0)
    '    ControlVisibility(cmbSearchCriteria.SelectedIndex, PeriodIndex)
    '    SetPeriod(PeriodIndex)
    '    If cmbSearchCriteria.Enabled = True Then
    '        setFocus(cmbSearchCriteria)
    '    End If
    'End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged, cmbOrderText.SelectedIndexChanged, cmbInvoiceText.SelectedIndexChanged, cmbReceiptText.SelectedIndexChanged, cmbInvoiceType.SelectedIndexChanged, cmbSearchInvoiceType.SelectedIndexChanged
        If sender.ID = "cmbPeriod" Then
            'ClearControl()
            Dim PeriodIndex As Int32 = CInt(IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0))
            ControlVisibility(1, PeriodIndex, 0, 0)
            SetPeriod(PeriodIndex)
            If cmbPeriod.Enabled = True Then
                setFocus(cmbPeriod)
            End If
        ElseIf sender.ID = "cmbOrderText" Then
            txtOrderNo.Text = "0"
            'ClearControl()
            'Dim SearchIndex As Int32 = cmbSearchCriteria.SelectedIndex
            Dim PeriodIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0)
            ControlVisibility(1, PeriodIndex)
            If cmbOrderText.Enabled = True Then
                setFocus(cmbOrderText)
            End If
        ElseIf sender.ID = "cmbInvoiceText" Then
            txtInvoiceNo.Text = "0"
            'ClearControl()
            'Dim SearchIndex As Int32 = cmbSearchCriteria.SelectedIndex
            Dim PeriodIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0)
            ControlVisibility(1, PeriodIndex)
            If cmbInvoiceText.Enabled = True Then
                setFocus(cmbInvoiceText)
            End If
        ElseIf sender.ID = "cmbReceiptText" Then
            txtReceipNo.Text = "0"
            'ClearControl()
            'Dim SearchIndex As Int32 = cmbSearchCriteria.SelectedIndex
            Dim PeriodIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0)
            ControlVisibility(1, PeriodIndex)
            If cmbReceiptText.Enabled = True Then
                setFocus(cmbReceiptText)
            End If
        ElseIf sender.ID = "cmbInvoiceType" Then
            mTransTypeID = CType(cmbInvoiceType.SelectedValue, Util.Trans)
            Session("mTransTypeId") = mTransTypeID
        ElseIf sender.ID = "cmbSearchInvoiceType" Then
            InvoiceTypeID = cmbSearchInvoiceType.SelectedValue
            Session("InvoiceTypeID") = InvoiceTypeID
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click ', btnClose.Click
        Session("MiddleFrame") = ""
        Session("mCount") = Nothing
        mInvoice = Nothing
        mDistinctTextListForOrder = Nothing
        mDistinctTextListForReceipt = Nothing
        mInvoiceList = Nothing
        RemoveSessions()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgInvoiceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInvoiceList.Sorting
        mInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInvoiceList") = mInvoiceList
        GridBind()
        UpdateItemGridView()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnGridPaging_Click(sender As Object, e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        dgInvoiceList.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(1)
        upnlFindNow.Update()
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        setVariables()
        dgInvoiceList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgInvoiceList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        pagecount = CInt(Session("pagecount"))
        SetControl()
        upnlGridView.Update()
        upnlResult.Update()
    End Sub
    Private Sub txtSearchBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        dgInvoiceList.DataBind()
        SetControl()
        upnlGridView.Update()
        upnlResult.Update()
    End Sub
#End Region

#Region " Report "
    'Created By :- Jyoti
    'Dated On 9/5/2007

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click ', BtnPrint.Click
        If Not User.IsInRole("InvoicePrint") Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        'For Invoice List
        Dim Rpt As New crInvoiceList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        CallFindNow(1, True)
        SearchStr1 = ""
        SearchStr2 = ""
        'If cmbSearchCriteria.SelectedIndex = 0 Then
        '    'All
        '    SearchStr1 = "The report shows all records till date."
        '    SearchStr2 = ""
        'ElseIf cmbSearchCriteria.SelectedIndex = 1 Then
        '    'Date
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    If cmbPeriod.SelectedIndex = 0 Then
        '        SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text
        '    ElseIf cmbPeriod.SelectedIndex = 6 Then
        '        SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
        '    Else
        '        SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
        '    End If
        'ElseIf cmbSearchCriteria.SelectedIndex = 2 Then
        '    'Invoice 
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbInvoiceText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 3 Then
        '    'Part Number
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtName.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 4 Then
        '    'Vendor
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtName.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 5 Then
        '    'Order
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbOrderText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 6 Then
        '    'Receipt 
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbReceiptText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 7 Then
        '    'Status
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbSearchInvoiceType.SelectedItem.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 8 Then
        '    'Status
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        'End If

        ReportDetails.Add(New rptStatus(, 0, , _
              dgInvoiceList.Columns.Item(1).HeaderText, dgInvoiceList.Columns.Item(2).HeaderText, dgInvoiceList.Columns.Item(3).HeaderText, _
              dgInvoiceList.Columns.Item(4).HeaderText, dgInvoiceList.Columns.Item(5).HeaderText, dgInvoiceList.Columns.Item(6).HeaderText, _
              dgInvoiceList.Columns.Item(7).HeaderText, dgInvoiceList.Columns.Item(8).HeaderText, dgInvoiceList.Columns.Item(9).HeaderText, _
              dgInvoiceList.Columns.Item(10).HeaderText))
        Dim I As Integer
        For I = 0 To mInvoiceList.Count - 1
            ReportDetails.Add(New rptStatus(, 1, , mInvoiceList(I).InvDateFormatted.ToString, _
                       mInvoiceList(I).InvoiceNo, mInvoiceList(I).InvoiceType, mInvoiceList(I).VendorName, mInvoiceList(I).VendorInvoiceNo, mInvoiceList(I).VendorInvoiceDateFormatted.ToString, mInvoiceList(I).CurrencyName, mInvoiceList(I).CGrandTotal.ToString, mInvoiceList(I).StatusName, mInvoiceList(I).UserName))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Invoice List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mInvoiceList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
#End Region
#End Region
End Class