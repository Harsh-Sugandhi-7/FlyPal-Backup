Imports System.Linq
Imports System.Linq.Enumerable

Public Class wfToolsCheckOutList_Ajax
    Inherits Page

#Region "Enumaration"

    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum

#End Region

#Region "Variable Declaration"

    Public mIssueList As IssueList
    Public mIssue As Issue
    Public mDistinctTextListForIssue As DistinctTextListForIssue
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptIssueReg
    Dim SearchIndex, DateIndex, FromDate, ToDate, IssueText, IssueTypeId, No, OrderText, IssueToEmpName, IssueFromStore, IssuePartNoSearch, IssueDescriptionSearch As String
    Dim mTransTypeID As Trans
    Dim mTransTypeList As TransactionList
    Public ModuleName As String = ""
    Public Tital As String
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mIssueDetail As String
    Dim mTransactionListCount As TransactionListCount 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
    Public mDistinctTextListForOrder As DistinctTextListForOrder

    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer
    Public mEmployeeStatus As EmployeeStatus
    'Added By Vikrant On 26-Nov-2018 For APFT26112018
    Public mCategoryList As CategoryList
    Public CategoryID As String
    'End

#End Region

#Region "Business Methods"

    Private Sub GetSession()

        mIssue = Session("mIssue")
        mIssueList = Session("mIssueList")
        mTransTypeID = Session("mTransTypeID")
        mDistinctTextListForIssue = Session("mDistinctTextListForIssue")
        mDistinctTextListForReceipt = Session("mDistinctTextListForReceipt")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        IssueTypeId = Session("IssueTypeId")
        IssueText = Session("IssueText")
        IssuePartNoSearch = Session("IssuePartNoSearch")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        ModuleName = Session("ModuleName")
        mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        IssueToEmpName = Session("IssueToEmpName")
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        mCategoryList = Session("mCategoryList")
        CategoryID = IIf(IsNothing(Session("CategoryID")), Guid.Empty.ToString, Session("CategoryID"))
        'End
        IssueFromStore = Session("IssueFromStore")
        IssueDescriptionSearch = Session("IssueDescriptionSearch")

    End Sub

    Private Sub SetSession()

        Session("mIssue") = mIssue
        Session("mIssueList") = mIssueList
        Session("mTransTypeID") = mTransTypeID
        Session("mDistinctTextListForIssue") = mDistinctTextListForIssue
        Session("mDistinctTextListForReceipt") = mDistinctTextListForReceipt
        Session("ModuleName") = ModuleName

    End Sub

    Private Sub RemoveSession()

        Session.Remove("mIssue")
        Session.Remove("mIssueList")
        Session.Remove("mDistinctTextListForIssue")
        Session.Remove("mDistinctTextListForReceipt")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("IssueTypeId")
        Session.Remove("IssueText")
        Session.Remove("IssueText")
        Session.Remove("WOText")
        Session.Remove("ReqText")
        Session.Remove("IssuePartNoSearch")
        Session.Remove("No")
        Session.Remove("mMachineList")
        Session.Remove("mTransTypeId")
        Session.Remove("IssueTo")
        Session.Remove("IssueAs")
        Session.Remove("mDistinctWOText")
        Session.Remove("totcnt")
        Session.Remove("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        Session.Remove("IssueToDiscardAsExpired")
        Session.Remove("IssueToEmpName")
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Session.Remove("mCategoryList")
        Session.Remove("CategoryID")
        'End
        Session.Remove("IssueFromStore")
        Session.Remove("IssueDescriptionSearch")

    End Sub

    Private Sub ClearAll()

        If InStr(Session("MiddleFrame"), "wfToolsCheckOutList_Ajax.aspx?") <= 0 Then
            RemoveSession()
            Session.Remove("mOrder")
        End If

    End Sub

    Private Sub NewRecord()

        mIssue = Issue.NewIssue(Trans.IssueToolsToEmployee)
        'Added By Prashant on 17-May-2021 ALL17052021
        If cmbCheckOutAgainst.SelectedValue = "19" Then 'Check Out Against part list So TypeID=19 which was default
            mIssue.ToTypeID = 19
        ElseIf cmbCheckOutAgainst.SelectedValue = "18" Then  'Check Out Against Requisition So TypeID=18 like issue to Aircraft Against Requisition 
            mIssue.ToTypeID = 18
            mIssue.IDate = Today.Date
            mIssue.IssueItems.Add(mIssue.ID, mTransTypeID)
            mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
        End If
        'End Of Added By Prashant on 17-May-2021 ALL17052021
        Session("mIssue") = mIssue

    End Sub

    Private Sub GridBind()

        dgIssueList.DataSource = mIssueList
        dgIssueList.DataBind()

    End Sub

    Private Sub EditRecord(mId As Guid)

        mIssue = Issue.GetIssue(mId)
        mIssue.MarkClean()
        Session("mIssue") = mIssue

        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        If mIssue.TransTypeID = 14 Then
            If mIssue.ToTypeID = 18 Then
                ModuleName = "Issue To Aircraft As Requisition"
            Else
                ModuleName = "Issue To Aircraft"
            End If
        ElseIf mIssue.TransTypeID = 44 Then
            If mIssue.ToTypeID = 18 Then
                ModuleName = "Issue To WorkShop As Requisition"
            Else
                ModuleName = "Issue To WorkShop"
            End If
        Else
            ModuleName = mTransTypeList.GetTransactionTypeName(mIssue.TransTypeID).ToString
        End If
        'ModuleName = mTransTypeList.GetTransactionTypeName(mIssue.TransTypeID).ToString
        Session("ModuleName") = ModuleName
        Session("mIssue") = mIssue
        upnlGridView.Update()

    End Sub

    Private Sub DeleteRecord(mId As Guid)

        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mIssue = Issue.GetIssue(mId)
        Session("mIssue") = mIssue

    End Sub

    Private Sub DataFieldBind()

        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        IssueText = Session("IssueText")
        IssuePartNoSearch = Session("IssuePartNoSearch")
        IssueToEmpName = Session("IssueToEmpName")
        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("26", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue
        Session("mDistinctTextListForIssue") = mDistinctTextListForIssue
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(79)
        Session("mTransactionListCount") = mTransactionListCount
        'End
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        CategoryID = IIf(IsNothing(Session("CategoryID")), Guid.Empty.ToString, Session("CategoryID"))
        mCategoryList = CategoryList.GetCategoryList("(All)", True)
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryLists") = mCategoryList

        DataBind()

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim DestinationName As String = String.Empty
                        Try
                            Dim mIssue As Issue
                            Session("sender") = ""
                            mIssue = CType(Session("mIssue"), Issue)
                            DestinationName = mIssueList(mIssue.ID).Destination
                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
                                If (mIssue.IsSync = 1 Or mIssue.IsSync = 2) Then
                                    MSGBoxCtrl.Show("Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    mIssue.Delete()
                                    mIssue.Save()
                                    'DataFieldBind()
                                    SetControl()
                                    SetTitle()
                                    upnlActionBtn.Update()
                                    upnlActionBtnBottom.Update()
                                End If
                            Else
                                'Added By Vikrant On 24-July-2014 For BA24072014
                                If AppSettings("LockBackDatedTransaction") = "True" Then
                                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                                        'Do nothing
                                    Else
                                        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
                                        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
                                        If (CDate(mIssue.IDate) >= FirstDayofLastMonth) Then
                                            If (CDate(mIssue.IDate) < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                                                MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mIssue.IDate).AddMonths(1)), Month(CDate(mIssue.IDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                                Exit Sub
                                            End If
                                        Else
                                            MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mIssue.IDate).AddMonths(1)), Month(CDate(mIssue.IDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                            Exit Sub
                                        End If
                                    End If
                                End If
                                'End

                                mIssue.Delete()
                                mIssue.Save()
                                DataFieldBind()
                                SetControl()
                                SetTitle()
                                upnlTitle.Update()
                            End If
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                ModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mIssue.TransTypeID).ToString
                                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + DestinationName

                                MarkLog(Action:=Action.Delete,
                                        ModuleName,
                                        Detail:="Can't delete : " & mIssueDetail & " is Currently in use",
                                        ErrorType:=ErrorType.NoError,
                                        TransID:=mIssue.ID, EventLogID,
                                        ModuleNameforGettingID:=TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString)

                                Dim stringInfo As String = ""
                                If ex.Message.Contains("tabExportInvoiceItem") Then
                                    stringInfo = "Export Invoice."
                                ElseIf ex.Message.Contains("tabReceiptItem") Then
                                    stringInfo = "Receipt."
                                ElseIf ex.Message.Contains("tabSalesInvoiceItem") Then
                                    stringInfo = "Sales Invoice."
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            'SetControl()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                ModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mIssue.TransTypeID).ToString
                                Session("ModuleName") = ModuleName
                                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + DestinationName
                                MarkLog(Action:=Action.Delete,
                                        ModuleName,
                                        Detail:=mIssueDetail,
                                        ErrorType:=ErrorType.NoError,
                                        TransID:=mIssue.ID, EventLogID,
                                        ModuleNameforGettingID:=TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "ResetIssuedToEmployee" Then
                        txtIssuedToEmployee.Text = ""
                        txtIssuedToEmployee.DataBind()
                        hdnIssuedToEmployeeId.Value = ""
                        upnlSearchCriteria.Update()
                        Session("sender") = ""
                    End If
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If

    End Sub

    Private Sub FindNow(Optional Text As String = "", Optional No As Integer = 0, Optional FromDate As String = "1-Jan-1900",
                        Optional ToDate As String = "1-Jan-2099", Optional StoreName As String = "", Optional VendorName As String = "",
                        Optional AircraftName As String = "", Optional IssueToType As Int32 = 0, Optional StatusID As Int32 = 0,
                        Optional IssueText As String = "", Optional ReceiptNo As Int32 = 0, Optional RealeaseNoteNo As String = "",
                        Optional SerialNo As String = "", Optional ItemName As String = "", Optional WorkShop As String = "",
                        Optional WOText As String = "", Optional WONo As Int32 = 0, Optional CustomerName As String = "",
                        Optional IsCustomerName As Boolean = False, Optional ReqText As String = "", Optional ReqNo As Integer = 0,
                        Optional OrderText As String = "", Optional OrderNo As Integer = 0, Optional Amend As String = "",
                        Optional IsForPrint As Boolean = False, Optional ToStoreName As String = "", Optional BatchNo As String = "",
                        Optional IssueToEmployeeName As String = "", Optional CategoryID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional Description As String = "")

        mIssueList = Nothing
        Dim IsVendor As Integer

        If IsForPrint Then
            mIssueList = IssueList.GetIssueList(Text, No, FromDate, ToDate, StoreName, VendorName, AircraftName, 19, StatusID, IssueText, ReceiptNo,
                                                RealeaseNoteNo, SerialNo, ItemName, Trans.IssueToolsToEmployee, IsVendor, WorkShop, WOText, WONo,
                                                False, False, CustomerName, IsCustomerName, ReqText, ReqNo, OrderText, OrderNo, Amend,
                                                IsCustomPaging:=False, CurrentPage:=mpageindex, PageSize:=mpageSize, ToStoreName:=ToStoreName,
                                                BatchNo:=BatchNo, IssueToEmpName:=IssueToEmployeeName, CategoryID:=CategoryID, Description:=Description)
            Exit Sub
        Else
            mIssueList = IssueList.GetIssueList(Text, No, FromDate, ToDate, StoreName, VendorName, AircraftName, 19, StatusID, IssueText, ReceiptNo,
                                                RealeaseNoteNo, SerialNo, ItemName, Trans.IssueToolsToEmployee, IsVendor, WorkShop, WOText, WONo,
                                                False, False, CustomerName, IsCustomerName, ReqText, ReqNo, OrderText, OrderNo, Amend,
                                                IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize, ToStoreName:=ToStoreName,
                                                BatchNo:=BatchNo, IssueToEmpName:=IssueToEmployeeName, CategoryID:=CategoryID, Description:=Description)
        End If
        dgIssueList.DataSource = Nothing


        'Get List From the Database as per Criteria             

        'Set DataSource of the Grid
        totalCount = mIssueList.TotalRecords
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount

        Session("mIssueList") = mIssueList
        dgIssueList.DataSource = mIssueList
        UpdateIssueGridView()

    End Sub

    Private Sub UpdateIssueGridView()

        Dim currentRow As Integer = mpageSize * (mpageindex)

        lblResult.Text = $"List of Tools issued as per criteria: {totalCount} Record(s) found."

        dgIssueList.DataBind()
        upnlGrid.Update()

    End Sub

    Private Sub CallFindNow(Index As Integer,
                            Optional IsForPrint As Boolean = False,
                            Optional IssueTypeId As String = "0")

        FindNow(Text:=Trim(IssueText),
                No:=CInt(Val(No)),
                FromDate:=txtFromDate.Text,
                ToDate:=txtToDate.Text,
                ItemName:=Trim(IssuePartNoSearch),
                StoreName:=Trim(IssueFromStore),
                IssueToEmployeeName:=Trim(IssueToEmpName),
                Description:=Trim(IssueDescriptionSearch),
                CategoryID:=CategoryID)

    End Sub

    Private Sub SetPeriod(Index As Int32)

        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-Jan-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("1-Jan-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = Today.AddDays(-6).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = Today.AddDays(1).AddMonths(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
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
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End If
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select

    End Sub

    Private Sub ControlVisibility(SearchIndex As Int32, Optional DateIndex As Int32 = 0)

        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If

    End Sub

    Private Sub ClearControls()

        txtPartNoSearch.Text = ""
        txtIssueNo.Text = ""
        txtIssuedToEmployee.Text = ""

    End Sub

    Private Sub SetVariables()

        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedValue)
        IssuePartNoSearch = txtPartNoSearch.Text.Trim
        IssueDescriptionSearch = txtDescriptionSearch.Text.Trim
        No = txtIssueNo.Text.Trim
        IssueToEmpName = txtIssuedToEmployee.Text.Trim
        IssueFromStore = txtFromStore.Text.Trim
        CategoryID = cmbCategory.SelectedValue.ToString 'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("IssueText") = IssueText
        Session("No") = No
        Session("IssuePartNoSearch") = IssuePartNoSearch
        Session("IssueDescriptionSearch") = IssueDescriptionSearch
        Session("IssueToEmpName") = IssueToEmpName
        Session("CategoryID") = CategoryID 'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Session("IssueFromStore") = IssueFromStore

    End Sub

    Private Sub SetControl()

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgIssueList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgIssueList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        SetPeriod(DateIndex)
        CallFindNow(SearchIndex, , IssueTypeId)
        dgIssueList.DataBind()
        cmbDate.SelectedIndex = DateIndex
        cmbIssueText.SelectedValue = IIf(IssueText = "", "(All)", IssueText)
        txtIssueNo.Text = No
        txtPartNoSearch.Text = IssuePartNoSearch
        txtDescriptionSearch.Text = IssueDescriptionSearch
        txtFromStore.Text = IssueFromStore
        txtIssuedToEmployee.Text = IssueToEmpName
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If Not CategoryID.Equals(Guid.Empty) Then
            cmbCategory.SelectedValue = CategoryID.ToString
        Else
            cmbCategory.SelectedValue = "(All)"
        End If
        'End
        ControlVisibility(SearchIndex, DateIndex)

    End Sub

    Private Sub SetTitle()

        Session("ModuleName") = "Issue Tools To Employee"
        lblTitle.Text = " List of Tools Issued "

    End Sub

    Private Function IsInRole(CheckFor As Rights) As Boolean

        Dim IsInRoleString As String = "ToolsCheckOut"

        'Depending upon decided IsInRole String; checking Rights of the User
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

    End Function
#End Region

#Region "Events"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
            Session("mTransTypeId") = CInt(Trans.IssueToolsToEmployee)
            Session("MiddleFrame") = "wfToolsCheckOutList_Ajax.aspx?"
            'Ajay 08-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, ModuleName:="ToolsCheckOut") Then
                ScriptManager.RegisterStartupScript(page:=Me,
                                                    type:=[GetType],
                                                    key:="MarkAsFavourite",
                                                    script:="MarkAsFavourite();",
                                                    addScriptTags:=True)
            Else
                ScriptManager.RegisterStartupScript(page:=Me,
                                                    type:=[GetType],
                                                    key:="RemoveFromFavourite",
                                                    script:="RemoveFromFavourite();",
                                                    addScriptTags:=True)
            End If
            '--------------------------
            DataFieldBind()
            SetControl()
            SetTitle()
        End If

    End Sub

    Private Sub DateChanged(sender As Object, e As EventArgs) Handles cmbDate.SelectedIndexChanged, cmbIssueText.SelectedIndexChanged

        If sender.ID = "cmbDate" Then
            'Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            SetPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
        ElseIf sender.ID = "cmbIssueText" Then
            txtIssueNo.Text = "0"
            If cmbIssueText.Enabled = True Then
                cmbIssueText.Focus()
            End If
        End If

    End Sub

    Private Sub GridViewRowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgIssueList.RowCommand

        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument) + dgIssueList.PageSize * dgIssueList.PageIndex
                Dim mId As Guid = mIssueList(index).ID
                mTransTypeID = mIssueList(mId).TransType
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    ScriptManager.RegisterStartupScript(page:=Me,
                                                        type:=Me.GetType(),
                                                        key:="OpenScript",
                                                        script:=MessageBox.Show(str:="You are not authorized user",
                                                                                IsTagRequired:=False),
                                                        addScriptTags:=True)
                    Exit Sub
                End If
                GridBind()

                'mIssueList(index).ID
                Dim mDate As String = mIssueList(mId).ILDateFormatted.ToString
                Dim mIssueNo As String = mIssueList(mId).IssueNo
                mIssueDetail = mIssueNo + " Dated : " + mDate + " to " + mIssueList(mId).Destination
                EditRecord(mId)
                Session("IsForWOReturn") = False
                Session("Edit") = True

                'Added By Prashant 20-Jul-2011
                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueList(mIssue.ID).Destination
                MarkLog(Action:=Action.Edit,
                        ModuleName,
                        Detail:=mIssueDetail,
                        ErrorType:=ErrorType.NoError,
                        TransID:=mIssue.ID, EventLogID,
                        ModuleNameforGettingID:=TransactionList.GetTransactionList().GetModuleName(TransTypeID:=mIssue.TransTypeID).ToString)

                ScriptManager.RegisterStartupScript(page:=Me,
                                                    type:=[GetType],
                                                    key:="OpenScript",
                                                    script:="openledgersame('wfToolsCheckOut_Ajax.aspx?BackPage=wfToolsCheckOutList_Ajax.aspx');",
                                                    addScriptTags:=True)
            Case "DeleteRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgIssueList.PageSize * dgIssueList.PageIndex
                Dim mId As Guid = mIssueList(index).ID
                If (Not IsInRole(Rights.Delete)) Then
                    ScriptManager.RegisterStartupScript(page:=Me,
                                                        type:=[GetType],
                                                        key:="OpenScript",
                                                        script:=MessageBox.Show(str:="You are not authorized user",
                                                                                IsTagRequired:=False),
                                                        addScriptTags:=True)
                    Exit Sub
                End If
                GridBind()

                DeleteRecord(mId)
                'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
            Case "ViewRec"

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index As Integer = CInt(e.CommandArgument) + dgIssueList.PageSize * dgIssueList.PageIndex
                Dim mId As Guid = mIssueList(index).ID
                mIssue = Issue.GetIssue(mId)
                If mIssue.Size > 0 Then
                    'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mManual.FileExtension
                    Dim path As String = AppSettings("DOCPath") & StrName & mIssue.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        File.Delete(AppSettings("DOCPath") & StrName & mIssue.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mIssue.ImageFile, 0, mIssue.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(page:=Me,
                                                            type:=[GetType],
                                                            key:="openFile",
                                                            script:="openFile();",
                                                            addScriptTags:=True)
                    End If
                End If
                'End
                GridBind()
        End Select

    End Sub

    Private Sub GridViewPageIndexChanged(source As Object, e As GridViewPageEventArgs) Handles dgIssueList.PageIndexChanging

        dgIssueList.PageIndex = e.NewPageIndex
        dgIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList
        dgIssueList.DataBind()

    End Sub

    Private Sub FindNow(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click

        Try

            SetVariables()
            dgIssueList.PageIndex = 0
            mpageindex = 0
            mCurrentpage = mpageindex + 1
            Session("mpageindex") = mpageindex
            Session("mCurrentpage") = mCurrentpage
            CallFindNow(SearchIndex, , IssueTypeId)
            upnlActionBtn.Update()
            upnlActionBtnBottom.Update()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub


    Private Sub AddNew(sender As Object, e As EventArgs) Handles btnAddNew.Click

        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewRecord()

        Session("mTransTypeID") = mTransTypeID
        Session("mPendingAgainst") = 1
        SetTitle()

        MarkLog(Action:=Action.[New],
                ModuleName:="",
                Detail:="ToolsCheckOut",
                ErrorType:=ErrorType.NoError,
                TransID:=mIssue.ID, EventLogID,
                ModuleNameforGettingID:=TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString())  'Added By Prashant 20-Jul-2011

        'Added By Prashant on 17-May-2021 ALL17052021
        If cmbCheckOutAgainst.SelectedValue = "19" Then 'Check Out Against part list So TypeID=19 which was default
            ScriptManager.RegisterStartupScript(page:=Me,
                                                type:=[GetType],
                                                key:="OpenScript",
                                                script:="openledgersame('wfToolsCheckOut_Ajax.aspx?BackPage=index.aspx');",
                                                addScriptTags:=True)
        ElseIf cmbCheckOutAgainst.SelectedValue = "18" Then  'Check Out Against Requisition So TypeID=18 like issue to Aircraft Against Requisition 
            ScriptManager.RegisterStartupScript(page:=Me,
                                                type:=[GetType],
                                                key:="OpenScript",
                                                script:="openledgersame('wfRequisitionItemListForIssue_Ajax.aspx?BackPage=index.aspx');",
                                                addScriptTags:=True)
        End If
        'End Of Added By Prashant on 17-May-2021 ALL17052021

    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click

        RemoveSession()
        Session("MiddleFrame") = ""
        ModuleName = Nothing
        Response.Redirect("Dashboard.aspx")

    End Sub

    'Added By Prashant 18-June-2009
    Private Sub GridViewSorting(source As Object, e As GridViewSortEventArgs) Handles dgIssueList.Sorting

        mIssueList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList
        dgIssueList.DataBind()

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MessageBoxResult()

    End Sub

    Protected Sub IssuedToEmployee(sender As Object, e As EventArgs)

        'SetEmpID()
        Dim message As String = ""
        If IsNumeric(txtIssuedToEmployee.Text) Then
            Dim mEmployeeListForCombo As EmployeeListForCombo
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtIssuedToEmployee.Text)
            If mEmployeeListForCombo.Count > 0 Then
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeListForCombo(0).ID.ToString, mIssue.IDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
                        Exit Sub
                    End If
                    txtIssuedToEmployee.Text = mEmployeeListForCombo(0).LicenceNoName
                    txtIssuedToEmployee.DataBind()
                End If
                Exit Sub
            End If

        End If

        If hdnIssuedToEmployeeId.Value <> "" Then
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnIssuedToEmployeeId.Value.ToString, Today.Date.ToString)
            If mEmployeeStatus.Count > 0 Then
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
                    Exit Sub
                End If
            Else
                txtIssuedToEmployee.Text = ""
            End If
        Else
            txtIssuedToEmployee.Text = ""
            txtIssuedToEmployee.DataBind()
        End If

    End Sub

    'Ajay 08-Nov-2022
    Private Sub HdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click 'Ajay 07-Nov-2022

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, ModuleName:="ToolsCheckOut")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub HdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click 'Ajay 07-Nov-2022

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, ModuleName:="ToolsCheckOut")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    '-----

#End Region

#Region "Report"

    'Created By :- Jyoti
    'Dated On 11/5/2007
#Region "Report Variable Declaration"

    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String

#End Region

#Region "Event"

    Private Function GetTitle() As String
        'By - Jyoti
        'Dated by - 11/5/2007
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " List Report"

        If mTitle = "" Then
            Return "Goods Outward Note List Report"
        Else
            Return mTitle
        End If
    End Function

#End Region

#End Region

#Region "Service Methods"

    <Services.WebMethod(), Script.Services.ScriptMethod()>
    Public Shared Function GetEmployeeList(prefixText As String, count As Integer, contextKey As String) As String()
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