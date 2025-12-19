Imports System.Text
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfToolsCheckInList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Variable Declaration "
    Public mReceiptCumInvoiceList As ReceiptCumInvoiceList
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptReceiptReg
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, OrderText, ReceiptText, Name, OrderNo, ReceiptNo, ReceiveFromEmpName, ReceiptPartNoSearch, _
        ReceiptDescriptionSearch, SearchText As String
    Dim mTransTypeId As Trans
    Private mReceiptTypeList As ReceiptTypeList
    Private mSearchReceiptTypeList As ReceiptTypeList
    Dim ReceiptTypeID As Int16
    Dim EventLogID As Guid
    Dim mReceiptDetails As String
    Dim mModuleName As String
    Dim mTransactionListCount As TransactionListCount 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Public mEmployeeStatus As EmployeeStatus
    'Added By Vikrant On 26-Nov-2018 For APFT26112018
    Public mCategoryList As CategoryList
    Public CategoryID As String
    'End
    Dim mToolsCheckInAgainstList As ToolsCheckInAgainstList
    Public mnWOListForCombo As nWOListForCombo
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReceiptCumInvoice = Session("mReceiptCumInvoice")
        mReceiptCumInvoiceList = Session("mReceiptCumInvoiceList")
        mTransTypeId = Session("mTransTypeId")
        mDistinctTextListForOrder = Session("mDistinctTextListForOrder")
        mDistinctTextListForReceipt = Session("mDistinctTextListForReceipt")
        mReceiptTypeList = Session("mReceiptTypeList")
        mSearchReceiptTypeList = Session("mSearchReceiptTypeList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        OrderText = Session("OrderText")
        ReceiptText = Session("ReceiptText")
        Name = Session("Name")
        OrderNo = IIf(IsNothing(Session("OrderNo")), 0, Session("OrderNo"))
        ReceiptNo = IIf(IsNothing(Session("ReceiptNo")), 0, Session("ReceiptNo"))
        ReceiptTypeID = Session("ReceiptTypeID") 'Changes by Kalpesh Shah as on 23-01-2008
        mModuleName = Session("mModuleName") 'Added By Utkarsh On 21-Jul-2011 For All19072011
        mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1

        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        ReceiveFromEmpName = Session("ReceiveFromEmpName")
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        mCategoryList = Session("mCategoryList")
        CategoryID = IIf(IsNothing(Session("CategoryID")), Guid.Empty.ToString, Session("CategoryID"))
        'End
        ReceiptPartNoSearch = Session("ReceiptPartNoSearch")
        ReceiptDescriptionSearch = Session("ReceiptDescriptionSearch")
        SearchText = Session("SearchText") 'Ajay 18-Jan-2023
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mReceiptCumInvoiceList")
        Session.Remove("mDistinctTextListForOrder")
        Session.Remove("mDistinctTextListForReceipt")
        Session.Remove("mReceiptTypeList")
        Session.Remove("mSearchReceiptTypeList")
        Session.Remove("mModuleName") 'Added By Utkarsh On 21-Jul-2011 For All19072011
        Session.Remove("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Session.Remove("mCategoryList")
        Session.Remove("CategoryID")
        'End
        Session.Remove("ReceiptPartNoSearch")
        Session.Remove("ReceiptDescriptionSearch")
        Session.Remove("SearchText") 'Ajay 18-Jan-2023
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfToolsCheckInList_Ajax.aspx?") <= 0 Then
            Session.Remove("mReceiptCumInvoiceList")
            Session.Remove("mReceiptCumInvoice")
            Session.Remove("mDistinctTextListForOrder")
            Session.Remove("mDistinctTextListForReceipt")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("OrderText")
            Session.Remove("ReceiptText")
            Session.Remove("Name")
            Session.Remove("OrderNo")
            Session.Remove("ReceiptNo")
            Session.Remove("mReceiptTypeList")
            Session.Remove("mSearchReceiptTypeList")
            Session.Remove("ReceiptTypeID")
            Session.Remove("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
            Session.Remove("mCurrentpage")
            Session.Remove("mpageSize")
            Session.Remove("mpageindex")
            Session.Remove("pagecount")
            Session.Remove("totalCount")
            Session.Remove("ReceiveFromEmpName")
            'Added By Vikrant On 26-Nov-2018 For APFT26112018
            Session.Remove("mCategoryList")
            Session.Remove("CategoryID")
            'End
            Session.Remove("ReceiptPartNoSearch")
            Session.Remove("ReceiptDescriptionSearch")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        Session("mTransTypeId") = mTransTypeId
        mReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(Trans.ReceiveToolsFromEmployee)
        If cmbToolsCheckInAgainst.SelectedValue = 1 Then
            mReceiptCumInvoice.ToolsCheckInAgainstID = 1
        ElseIf cmbToolsCheckInAgainst.SelectedValue = 2 Then
            mReceiptCumInvoice.ToolsCheckInAgainstID = 2
            mReceiptCumInvoice.WOID = New Guid(cmbWorkOrder.SelectedValue.ToString)
        End If
        mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Sub EditRecord(ByVal ReceiptID As Guid, ByVal InvoiceID As Guid)
        mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID, InvoiceID)
        mReceiptCumInvoice.MarkClean()
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "ToolsCheckIn"

        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
        End Select
        Return True
    End Function
    Private Sub DeleteRecord(ByVal mReceiptID As Guid, ByVal mInvoiceID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptID, mInvoiceID)
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Sub SetControl()
        SetPeriod(DateIndex)

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgToolsReceiptList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgToolsReceiptList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        CallFindNow(SearchIndex)

        dgToolsReceiptList.DataBind()

        'cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        'cmbStatus.SelectedValue = StatusId

        'If cmbOrderText.Items.Contains(New System.Web.UI.WebControls.ListItem(OrderText)) Then
        '    cmbOrderText.SelectedValue = OrderText
        'Else
        '    cmbOrderText.SelectedValue = "(All)"
        'End If
        If cmbRecText.Items.Contains(New System.Web.UI.WebControls.ListItem(ReceiptText)) Then
            cmbRecText.SelectedValue = ReceiptText
        Else
            cmbRecText.SelectedValue = "(All)"
        End If
        txtReceiptNo.Text = ReceiptNo
        txtName.Text = Name
        txtReceivedFromEmployee.Text = ReceiveFromEmpName
        txtPartNoSearch.Text = ReceiptPartNoSearch
        txtDescriptionSearch.Text = ReceiptDescriptionSearch
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If Not CategoryID.Equals(Guid.Empty) Then
            cmbCategory.SelectedValue = CategoryID.ToString
        Else
            cmbCategory.SelectedValue = "(All)"
        End If
        'End
        'txtOrderNo.Text = OrderNo

        'cmbSearchReceiptType.SelectedIndex = ReceiptTypeID
        ControlVisibility(SearchIndex, DateIndex, Val(ReceiptNo), Val(OrderNo))
        'Ajay 18-Jan-2023
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mVendorName As String = String.Empty
                        Try
                            mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
                            mVendorName = mReceiptCumInvoiceList(mReceiptCumInvoice.ID).VendorName
                            'Added By Vikrant On 24-July-2014 For BA24072014
                            If AppSettings("LockBackDatedTransaction") = "True" Then
                                If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                                    'Do nothing
                                Else
                                    Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
                                    Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
                                    If (CDate(mReceiptCumInvoice.RecCumInvDate) >= FirstDayofLastMonth) Then
                                        If (CDate(mReceiptCumInvoice.RecCumInvDate) < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                                            msgCount = 1
                                            MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), Month(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                            Exit Sub
                                        End If
                                    Else
                                        msgCount = 1
                                        MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), Month(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                        Exit Sub
                                    End If
                                End If
                            End If
                            'End
                            ReceiptCumInvoice.DeleteReceiptInvoice(mReceiptCumInvoice.Receipt.ID, mReceiptCumInvoice.Invoice.ID)
                            DataFieldBind()
                            SetControl()
                            SetTitle()
                            upnlSearchCriteria.Update()
                            upnlTitle.Update()
                            upnlGrid.Update()
                            'UpdateItemGridView()
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Message.Contains("tabInvoiceItem") Then
                                stringInfo = "Invoice."
                            ElseIf ex.Message.Contains("tabIssueItem") Then
                                stringInfo = "Issue."
                            ElseIf ex.Message.Contains("tabOrderItem") Then
                                stringInfo = "Order."
                            End If
                            If ex.Number = 547 Then
                                mModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mReceiptCumInvoice.TransTypeID).ToString
                                Session("mModuleName") = mModuleName
                                mReceiptDetails = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mVendorName
                                MarkLog(Util.Action.Delete, mModuleName, "Can't delete : " & mReceiptDetails & " is Currently in use", Util.ErrorType.HandledError, mReceiptCumInvoice.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            SetTitle()
                            upnlFindNow.Update()
                            If msgCount = 0 Then
                                mModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mReceiptCumInvoice.TransTypeID).ToString
                                Session("mModuleName") = mModuleName
                                mReceiptDetails = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mVendorName
                                MarkLog(Util.Action.Delete, mModuleName, mReceiptDetails, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString)
                            End If

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()

                    End If
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Fromdate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal Text As String = "", _
    Optional ByVal No As Integer = 0, Optional ByVal ItemName As String = "", Optional ByVal Description As String = "", _
    Optional ByVal ReceivedEmpName As String = "", Optional ByVal CategoryID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal SearchText As String = "") 'Ajay SearchText 10-Jan-2023)
        'clear the obj and grid
        mReceiptCumInvoiceList = Nothing
        dgToolsReceiptList.DataSource = Nothing
        'get the list
        mReceiptCumInvoiceList = ReceiptCumInvoiceList.GetReceiptCumInvoiceList(FromDate:=Fromdate, ToDate:=ToDate, ReceiptText:=Text, ReceiptNo:=No, _
                        IntReceiptNo:="", VendorName:="", AircraftName:="", StoreName:="", DCNO:="", StatusID:=0, ItemName:=ItemName, Description:=Description, OrderText:="", _
                        OrderNo:=0, IssueText:="", IssueNo:=0, ReleaseNoteNo:="", Type:=0, TransTypeID:=80, _
                        ReceivedFromType:=19, ReceivedEmpName:=ReceivedEmpName, IsCustomPaging:=False, CategoryID:=CategoryID, SearchText:=SearchText)
        'bind the list to the datagrid
        'set the session
        'totalCount = mReceiptCumInvoiceList.TotalRecords ' Ajay 03-mar-2023
        totalCount = mReceiptCumInvoiceList.Count
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount
        dgToolsReceiptList.DataSource = mReceiptCumInvoiceList
        dgToolsReceiptList.DataBind()
        Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        UpdateItemGridView()
        dgToolsReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 11-Jan-2023
    End Sub
    Private Sub CallFindNow(ByVal indx As Int32, Optional ByVal IsForPrint As Boolean = False)
        FindNow(Fromdate:=txtFromDate.Text, ToDate:=txtToDate.Text, Text:=Trim(ReceiptText), No:=CInt(Val(ReceiptNo)), ItemName:=Trim(ReceiptPartNoSearch), _
                Description:=Trim(ReceiptDescriptionSearch), ReceivedEmpName:=Trim(ReceiveFromEmpName), _
                CategoryID:=CategoryID, SearchText:=SearchText)
        'Select Case indx
        '    Case 0  'All
        '        FindNow()
        '    Case 1  'Date
        '        FindNow(Fromdate:=txtFromDate.Text, ToDate:=txtToDate.Text)
        '    Case 2  'ReceiptCumInvoice No & Text.
        '        FindNow(Fromdate:=FromDate, ToDate:=ToDate, Text:=ReceiptText, No:=CInt(Val(ReceiptNo)))
        '    Case 3  'Part No.
        '        FindNow(Fromdate:=FromDate, ToDate:=ToDate, ItemName:=Name)
        '    Case 4  'Rec From Emp
        '        FindNow(Fromdate:=FromDate, ToDate:=ToDate, ReceivedEmpName:=ReceiveFromEmpName)
        '        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        '    Case 5  'Category
        '        FindNow(Fromdate:=FromDate, ToDate:=ToDate, CategoryID:=CategoryID)
        '        'End
        'End Select
        dgToolsReceiptList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal PeriodIndex As Int32 = 0, Optional ByVal RectTxt As Int32 = 0, Optional ByVal Ordtxt As Int32 = 0)
        'cmbDate.Visible = CBool(IIf(SearchIndex = 1, True, False))
        lblFromDate.Visible = CBool(IIf(PeriodIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(PeriodIndex <> 0, True, False))
        'cmbRecText.Visible = CBool(IIf(SearchIndex = 2, True, False))
        'txtName.Visible = CBool(IIf(SearchIndex = 3, True, False))
        'txtReceivedFromEmployee.Visible = CBool(IIf(SearchIndex = 4, True, False))
        'txtReceiptNo.Visible = IIf(SearchIndex = 2 And cmbRecText.SelectedIndex > 0, True, False)
        'txtNo.Visible = IIf((SearchIndex = 2 And cmbRecText.SelectedIndex > 0), True, False)
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
        'cmbCategory.Visible = CBool(IIf(SearchIndex = 5, True, False)) 'Added By Vikrant On 26-Nov-2018 For APFT26112018
        txtSearchBox.Visible = True 'Ajay 18-Jan-2023
    End Sub
    Private Sub SetPeriod(ByVal index As Int32)
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        ReceiptText = IIf(cmbRecText.SelectedIndex <= 0, "", cmbRecText.SelectedValue)
        Name = txtName.Text.Trim
        ReceiptNo = txtReceiptNo.Text.Trim
        ReceiveFromEmpName = txtReceivedFromEmployee.Text.Trim
        ReceiptPartNoSearch = txtPartNoSearch.Text.Trim
        ReceiptDescriptionSearch = txtDescriptionSearch.Text.Trim
        CategoryID = cmbCategory.SelectedValue.ToString 'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("OrderText") = OrderText
        Session("ReceiptText") = ReceiptText
        Session("OrderNo") = OrderNo
        Session("ReceiptNo") = ReceiptNo
        Session("Name") = Name
        Session("ReceiveFromEmpName") = ReceiveFromEmpName
        Session("CategoryID") = CategoryID 'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Session("ReceiptPartNoSearch") = ReceiptPartNoSearch
        Session("ReceiptDescriptionSearch") = ReceiptDescriptionSearch
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text) 'Ajay 18-01-2023
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetTitle()
        Session("ModuleName") = "Receive Tools From Employee"

        'lblTitle.Text = "List of Tools Received" + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"
        lblTitle.Text = "List of Tools Received"
        upnlTitle.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        ReceiptText = Session("ReceiptText")
        Name = Session("Name")
        ReceiveFromEmpName = Session("ReceiveFromEmpName")
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("27", , True, "(All)")
        cmbRecText.DataSource = mDistinctTextListForReceipt
        Session("mDistinctTextListForReceipt") = mDistinctTextListForReceipt
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(80)
        Session("mTransactionListCount") = mTransactionListCount
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        CategoryID = IIf(IsNothing(Session("CategoryID")), Guid.Empty.ToString, Session("CategoryID"))
        mCategoryList = CategoryList.GetCategoryList("(All)", True)
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryLists") = mCategoryList
        'If mCategoryList.Count > 2 Then
        '    cmbSearch.Items.Add(New ListItem("Category", "5"))
        'Else
        '    cmbSearch.Items.Remove(New ListItem("Category", "5"))
        'End If
        'Énd
        mToolsCheckInAgainstList = ToolsCheckInAgainstList.GetRequisitionList()
        cmbToolsCheckInAgainst.DataSource = mToolsCheckInAgainstList

        mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1800").FormattedText, New SmartDate("01-01-4400").FormattedText, , , 2)
        cmbWorkOrder.DataSource = mnWOListForCombo

        DataBind()
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        'If totalCount = 0 Then
        lblResult.Text = "As per criteria: " & totalCount & " Record(s) found."
        'Else
        'lblResult.Text = "List of Tools Received as per criteria:" & currentrow + 1 & " to " & currentrow + mReceiptCumInvoiceList.Count & " of " & totalCount & " Record(s) found."
        'End If
        'SliderExtender1.Minimum = 1
        'SliderExtender1.Maximum = pagecount
        'Slidercontrol.Text = mCurrentpage
        'txtPageDisplay.Text = mCurrentpage
        'lblpagecount.Text = pagecount
        'If pagecount > 1 Then
        '    PnlPaging.Visible = Truecolumns
        'Else
        '    PnlPaging.Visible = False

        dgToolsReceiptList.DataBind()
        upnlGrid.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
            mTransTypeId = Util.Trans.ReceiveToolsFromEmployee
            Session("mTransTypeId") = mTransTypeId
            Session("MiddleFrame") = "wfToolsCheckInList_Ajax.aspx?"
            'Ajay 07-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ToolsCheckIn") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            DataFieldBind()
            SetControl()
            SetTitle()
            cmbShowE.SelectedValue = "4" 'Ajay 18-Jan-2023
        End If
    End Sub
    'btnAddNew.Click, Ajay
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click

        'Ajay 07-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ToolsCheckIn") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
        If cmbToolsCheckInAgainst.SelectedValue = 2 And cmbWorkOrder.SelectedIndex = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Select Work Order No. ", MsgBoxStyle.OkOnly, "SelectWorkOrderNo")
            Exit Sub
        End If
        NewRecord()
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        SetTitle()
        MarkLog(Util.Action.[New], TransactionList.GetTransactionList().GetTransactionTypeName(mReceiptCumInvoice.TransTypeID).ToString, "", Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString)

        Dim mPrevTransID As Guid = Guid.Empty
        Dim mPrimaryOrderType As Integer
        Dim mTransaction As Integer
        Dim mFromPartList As Boolean
        If CType(mTransTypeId, Trans) = Util.Trans.ReceiptAgainstPuchaseOrder Then
            mPrimaryOrderType = 3 'TransListOf.Order_Outright
        ElseIf CType(mTransTypeId, Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Then
            mPrimaryOrderType = 4 'TransListOf.Order_ExchangeRepair
        End If
        mTransaction = 3 'Transaction.Order
        mFromPartList = False
        Session("OpenFrom") = 1
        Session("mPrevTransID") = mPrevTransID
        Session("mPrimaryOrderType") = mPrimaryOrderType
        Session("mTransaction") = mTransaction
        Session("mFromPartList") = mFromPartList
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingToolsToReceiveFromEmployee_Ajax.aspx?BackPage=index.aspx');", True)
    End Sub
    Private Sub dgToolsReceiptList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgToolsReceiptList.RowCommand

        'Ajay 07-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ToolsCheckIn") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
        Select Case e.CommandName
            Case "EditView"
                'Dim index As Integer = CInt(e.CommandArgument) '+ dgToolsReceiptList.PageIndex * dgToolsReceiptList.PageSize
                Dim mId As Guid = New Guid(e.CommandArgument.ToString)
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                EditRecord(mReceiptCumInvoiceList(mId).ReceiptID, mReceiptCumInvoiceList(mId).InvoiceID)
                mTransTypeId = mReceiptCumInvoice.TransTypeID
                SetTitle()
                mReceiptDetails = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mReceiptCumInvoiceList(mReceiptCumInvoice.ID).VendorName
                MarkLog(Util.Action.Edit, TransactionList.GetTransactionList().GetTransactionTypeName(mReceiptCumInvoice.TransTypeID).ToString, mReceiptDetails, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString)
                Dim str As String
                str = "openledgersame('wfToolsCheckIn_Ajax.aspx?BackPage=Index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                'Dim index As Integer = CInt(e.CommandArgument) '+ dgToolsReceiptList.PageIndex * dgToolsReceiptList.PageSize
                Dim mId As Guid = New Guid(e.CommandArgument.ToString)
                If (Not IsInRole(Rights.Delete)) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mReceiptCumInvoiceList(mId).ReceiptID, mReceiptCumInvoiceList(mId).InvoiceID)
        End Select
    End Sub
    Private Sub dgReceiptList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgToolsReceiptList.PageIndexChanging
        dgToolsReceiptList.PageIndex = e.NewPageIndex
        mCurrentpage = e.NewPageIndex
        Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        dgToolsReceiptList.DataSource = mReceiptCumInvoiceList
        dgToolsReceiptList.DataBind()
        dgToolsReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 18-Jan-2023
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        dgToolsReceiptList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex)
        dgToolsReceiptList.DataBind()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnBottom.Update()
    End Sub
    'Private Sub cmbSearchCriteria_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
    '    cmbDate.SelectedIndex = 0
    '    cmbRecText.SelectedIndex = 0
    '    ClearControl()
    '    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
    '    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
    '    SetPeriod(DateIndex)
    '    If cmbSearch.Enabled = True Then
    '        setFocus(cmbSearch)
    '    End If
    'End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbRecText.SelectedIndexChanged
        If sender.ID = "cmbDate" Then
            Dim PeriodIndex As Int32 = CInt(IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0))
            ControlVisibility(1, PeriodIndex, 0, 0)
            SetPeriod(PeriodIndex)
            If cmbDate.Enabled = True Then
                setFocus(cmbDate)
            End If
        ElseIf sender.ID = "cmbRecText" Then
            txtReceiptNo.Text = "0"
            If cmbRecText.Enabled = True Then
                setFocus(cmbRecText)
            End If
        End If
    End Sub
    ', btnClose.Click Ajay
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Session("mCount") = Nothing
        mReceiptCumInvoice = Nothing
        mDistinctTextListForOrder = Nothing
        mDistinctTextListForReceipt = Nothing
        mReceiptCumInvoiceList = Nothing
        RemoveSessions()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgReceiptList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgToolsReceiptList.Sorting
        mReceiptCumInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        dgToolsReceiptList.DataSource = mReceiptCumInvoiceList
        dgToolsReceiptList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'Ajay 07-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 07-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "ToolsCheckIn")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 07-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "ToolsCheckIn")
    End Sub
    'Ajay 11-Jan-2023
    Private Sub txtSearchBox_TextChanged(sender As Object, e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        SetControl()
        dgToolsReceiptList.DataBind()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnBottom.Update()
    End Sub
    '-----
    'Ajay 18-Jan-2023
    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim ExpiryDateList = ((From res In mWOList).ToList.Take(CInt(DropDownList1.SelectedItem.ToString))).ToList
        dgToolsReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgToolsReceiptList.DataSource = mReceiptCumInvoiceList
        dgToolsReceiptList.DataBind()
        'dgEmployeeList.PageIndex = e.OnSelectedIndexChanged
        ' DataBind()

        'SetGrid()
        'GridColumnsVisibility()
        'upnlGridView.Update()
        'upnlResult.Update()

        SetControl()

        upnlGrid.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub cmbToolsCheckInAgainst_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbToolsCheckInAgainst.SelectedIndexChanged
        If cmbToolsCheckInAgainst.SelectedIndex = 0 Then
            cmbWorkOrder.Visible = False
        ElseIf cmbToolsCheckInAgainst.SelectedIndex = 1 Then
            cmbWorkOrder.Visible = True
        End If
        upnlGrid.Update()
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmployeeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As EmpNoNameAutoComplete
        itemlist = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region



End Class