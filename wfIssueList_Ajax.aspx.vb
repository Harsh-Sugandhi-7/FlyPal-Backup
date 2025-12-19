Imports System.Text
'Ajax Conversion by Vikrant
Public Class wfIssueList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        Authorized = 7
    End Enum
#End Region

#Region " Variable Declaration "
    Public mIssueList As IssueList
    Public mIssue As Issue
    Public mDistinctTextListForIssue As DistinctTextListForIssue
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptIssueReg
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, IssueText, ReceiptText, WOText, IssueOrderText, IssueReceiptNo, IssueWoNo, _
        IssueOrderNo, IssueReqNo, IssueTypeId, Name, No, IssueTo, IssueAs, ReqText, IssueFromStore, SearchText As String
    Dim mTransTypeID As Trans
    Dim mTransTypeList As TransactionList
    Public ModuleName As String = ""
    Public Tital As String
    Public mIssueTypeList As IssueTypeList
    Dim mDistinctWOText As nDistinctWOText
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mIssueDetail As String
    Public mDistinctTextListForRequisition As DistinctTextListForRequisition 'Added by vikrant For New Requisition
    Dim mTransactionListCount As TransactionListCount 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer
    Dim IssueVendorName As String = String.Empty
    Dim IssueAircraftName As String = String.Empty
    Dim IssueToStoreName As String = String.Empty
    Dim IssueCustomerName As String = String.Empty
    Dim IssueWorkShopName As String = String.Empty
    Dim IssuePartNoSearch As String = String.Empty
    Dim IssueReleaseNoteNoSearch As String = String.Empty
    Dim IssueSerialNoSearch As String = String.Empty
    Dim IssueBatchNoSearch As String = String.Empty
	Dim ListofIssueItems As New StringBuilder
	Private ReportHelper As New ReportHelper
#End Region

#Region " Business Methods "
	Private Sub GetSession()
        mIssueTypeList = Session("mIssueTypeList")
        mIssue = Session("mIssue")
        mIssueList = Session("mIssueList")
        mTransTypeID = Session("mTransTypeID")
        mDistinctTextListForIssue = Session("mDistinctTextListForIssue")
        mDistinctTextListForReceipt = Session("mDistinctTextListForReceipt")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        IssueTypeId = Session("IssueTypeId")

        IssueText = Session("IssueText")
        ReceiptText = Session("ReceiptText")
        WOText = Session("WOText")
        IssueOrderText = Session("IssueOrderText")
        ReqText = Session("ReqText")

        No = IIf(IsNothing(Session("No")), 0, Session("No"))  'Issue No
        IssueOrderNo = IIf(IsNothing(Session("IssueOrderNo")), 0, Session("IssueOrderNo"))
        IssueReceiptNo = IIf(IsNothing(Session("IssueReceiptNo")), 0, Session("IssueReceiptNo"))
        IssueWoNo = IIf(IsNothing(Session("IssueWoNo")), 0, Session("IssueWoNo"))
        IssueReqNo = IIf(IsNothing(Session("IssueReqNo")), 0, Session("IssueReqNo"))

        Name = Session("Name")
        ModuleName = Session("ModuleName")
        IssueTo = Session("IssueTo")
        IssueAs = Session("IssueAs")
        mDistinctWOText = Session("mDistinctWOText")
        mDistinctTextListForRequisition = Session("mDistinctTextListForRequisition")
        mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")

        IssueVendorName = Session("IssueVendorName")
        IssueAircraftName = Session("IssueAircraftName")
        IssueToStoreName = Session("IssueToStoreName")
        IssueCustomerName = Session("IssueCustomerName")
        IssueWorkShopName = Session("IssueWorkShopName")
        IssuePartNoSearch = Session("IssuePartNoSearch")
        IssueReleaseNoteNoSearch = Session("IssueReleaseNoteNoSearch")
        IssueSerialNoSearch = Session("IssueSerialNoSearch")
        IssueBatchNoSearch = Session("IssueBatchNoSearch")
        IssueFromStore = Session("IssueFromStore")
        SearchText = Session("SearchText") 'Ajay 31-Jan-2023
    End Sub
    Private Sub SetSession()
        Session("mIssueTypeList") = mIssueTypeList
        Session("mIssue") = mIssue
        Session("mIssueList") = mIssueList
        Session("mTransTypeID") = mTransTypeID
        Session("mDistinctTextListForIssue") = mDistinctTextListForIssue
        Session("mDistinctTextListForReceipt") = mDistinctTextListForReceipt
        Session("ModuleName") = ModuleName
        Session("IssueTo") = IssueTo
        Session("IssueAs") = IssueAs
        Session("mDistinctWOText") = mDistinctWOText
        Session("mDistinctTextListForRequisition") = mDistinctTextListForRequisition

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
        Session.Remove("ReceiptText")
        Session.Remove("WOText")
        Session.Remove("IssueOrderText")
        Session.Remove("ReqText")

        Session.Remove("No")
        Session.Remove("IssueOrderNo")
        Session.Remove("IssueReceiptNo")
        Session.Remove("IssueWoNo")
        Session.Remove("IssueReqNo")

        Session.Remove("Name")
        Session.Remove("mMachineList")
        Session.Remove("mTransTypeId")
        Session.Remove("mIssueTypeList")
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
        Session.Remove("IssueVendorName")
        Session.Remove("IssueAircraftName")
        Session.Remove("IssueToStoreName")
        Session.Remove("IssueCustomerName")
        Session.Remove("IssueWorkShopName")
        Session.Remove("IssuePartNoSearch")
        Session.Remove("IssueReleaseNoteNoSearch")
        Session.Remove("IssueSerialNoSearch")
        Session.Remove("IssueBatchNoSearch")
        Session.Remove("IssueFromStore")
        Session.Remove("SearchText") 'Ajay 31-Jan-2023
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfIssueList_Ajax.aspx?") <= 0 Then
            RemoveSession()
            Session.Remove("mOrder")
        End If
    End Sub
    Private Sub NewRecord()
        'Dim IsRequisitionTransaction As Boolean = IIf(mTransTypeID = 14 Or mTransTypeID = 44, IIf(cmbIssueAs.SelectedIndex = 2, True, False), False)
        Dim IsRequisitionTransaction As Boolean = IIf(mTransTypeID = 14 Or mTransTypeID = 44, IIf(cmbIssueAs.SelectedItem.Text = "Requisition", True, False), False)
        mIssue = Issue.NewIssue(mTransTypeID, IsRequisitionTransaction)
        mIssue.IDate = Today.Date
        'If mTransTypeID = 16 Or mTransTypeID = 18 Or mTransTypeID = 49 Or mTransTypeID = 51 Or mTransTypeID = 55 Or mTransTypeID = 58 Or mTransTypeID = 59 Or mTransTypeID = 60 Or ((mTransTypeID = 14 Or mTransTypeID = 44) And cmbIssueAs.SelectedIndex = 2) Then  '55, 58 Added By Prashant 6-Jan-2010  '72 Added by vikrant For New Requisition 
        If mTransTypeID = 16 Or mTransTypeID = 18 Or mTransTypeID = 49 Or mTransTypeID = 51 Or mTransTypeID = 55 Or mTransTypeID = 58 Or mTransTypeID = 59 Or mTransTypeID = 60 Or ((mTransTypeID = 14 Or mTransTypeID = 44) And cmbIssueAs.SelectedItem.Text = "Requisition") Then  '55, 58 Added By Prashant 6-Jan-2010  '72 Added by vikrant For New Requisition 
            mIssue.IssueItems.Add(mIssue.ID, mTransTypeID)
            mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
        End If
        Session("mIssue") = mIssue
        IssueTo = cmbIssueTo.SelectedIndex
        IssueAs = cmbIssueAs.SelectedIndex
        Session("IssueTo") = IssueTo
        Session("IssueAs") = IssueAs
    End Sub
    Private Sub GridBind()
        dgIssueList.DataSource = mIssueList
        dgIssueList.DataBind()
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
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
        ElseIf mIssue.TransTypeID = 59 And mIssue.ToTypeID = 18 Then
            ModuleName = "Issue To Work order As Material Requisition"
        Else
            ModuleName = mTransTypeList.GetTransactionTypeName(mIssue.TransTypeID).ToString
        End If
        'ModuleName = mTransTypeList.GetTransactionTypeName(mIssue.TransTypeID).ToString
        Session("ModuleName") = ModuleName
        Session("mIssue") = mIssue
        IssueTo = cmbIssueTo.SelectedIndex
        IssueAs = cmbIssueAs.SelectedIndex
        Session("IssueTo") = IssueTo
        Session("IssueAs") = IssueAs
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mIssue = Issue.GetIssue(mId)
        Session("mIssue") = mIssue
    End Sub
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        IssueText = Session("IssueText")
        ReceiptText = Session("ReceiptText")
        WOText = Session("WOText")
        ReqText = Session("ReqText")
        IssueTypeId = Session("IssueTypeId")
        Name = Session("Name")
        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("13", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue
        cmbReceiptText.DataSource = mDistinctTextListForReceipt

        mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
        cmbWoText.DataSource = mDistinctWOText
        Session("mDistinctWOText") = mDistinctWOText

        'Added by vikrant For New Requisition
        mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)")
        cmbRequisitionText.DataSource = mDistinctTextListForRequisition
        Session("mDistinctTextListForRequisition") = mDistinctTextListForRequisition
        'End

        mTransactionListCount = TransactionListCount.GetTransactionListCountt(14)
        Session("mTransactionListCount") = mTransactionListCount
        'End


        mIssueTypeList = IssueTypeList.GetIssueTypeList(0)
        cmbIssueAs.Enabled = IIf(mIssueTypeList.Count = 0, False, True)
        'btnAddNew.Enabled = IIf(mIssueTypeList.Count = 0, False, True) '' Ajay 31-01-2023
        btnAddNewTop.Enabled = IIf(mIssueTypeList.Count = 0, False, True)

        cmbIssueAs.DataSource = mIssueTypeList
        Session("mIssueTypeList") = mIssueTypeList

        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)") 'Added By Prashant 11-Jun-2014 ALL11062014-1
        cmbOrderText.DataSource = mDistinctTextListForOrder

        DataBind()
        'If (AppSettings("ShowCAMOOnlyForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True") Then
        If AppSettings("ShowAMOOnlyForNewClients") = "True" Then
            'Set Issue To Work order As Material Requisition as default  
            IssueAs = 3
            Session("IssueAs") = IssueAs
            IssueTo = 6
            Session("IssueTo") = IssueTo
            'End
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
                        Dim DestinationName As String = String.Empty
                        Try
                            Dim mIssue As Issue
                            Session("sender") = ""
                            mIssue = CType(Session("mIssue"), Issue)
                            DestinationName = mIssueList(mIssue.ID).Destination
                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
                                If (mIssue.IsSync = 1 Or mIssue.IsSync = 2) Then
                                    MSGBoxCtrl.show("Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    mIssue.Delete()
                                    mIssue.Save()
                                    'DataFieldBind()
                                    SetControl()
                                    SetTitle()
                                    ControlEnability()
                                    SetGrid() ' 'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
                                    upnlActionBtn.Update()
                                    upnlActionBtnBottom.Update()
                                End If
                            Else
                                'Added By Vikrant On 01-Mar-2018 For BA15022018
                                If mIssue.TransTypeID = Util.Trans.IssueToAircraft And mIssue.ToTypeID = 18 Then
                                    Dim mConsumableAndExpendableList As ConsumableAndExpendableList
                                    mConsumableAndExpendableList = ConsumableAndExpendableList.GetList(ReqID:=mIssue.RequisitionID.ToString)
                                    If mConsumableAndExpendableList.Count > 0 Then
                                        MSGBoxCtrl.show("Delete Alert!", "This Transaction cannot be deleted.Consumable & Expendable(C&E) entry already exists against this transaction", "", MsgBoxStyle.OkOnly, "")
                                        Exit Sub
                                    End If
                                End If
                                'End
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
                                For i As Integer = 0 To mIssue.IssueItems.Count - 1
                                    If mIssue.IssueItems(i).CountOf > 0 Then
                                        MSGBoxCtrl.show("Alert!", "Can not be Deleted<BR>As " + mIssue.IssueItems(i).ItemName + " Serial No. " + mIssue.IssueItems(i).SerialNo + " is already received after this transaction.", "", MsgBoxStyle.OkOnly, "")
                                        Exit Sub
                                    End If
                                Next
                                ListofIssueItems.Append(mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + DestinationName + " ")
                                If mIssue.IssueItems.Count > 0 Then
                                    For l As Integer = 0 To mIssue.IssueItems.Count - 1
                                        ListofIssueItems.Append(mIssue.IssueItems(l).ItemName + " " + mIssue.IssueItems(l).Category + " Qty:- " + mIssue.IssueItems(l).DisplayQty.ToString + " Rate:- " + mIssue.IssueItems(l).EffRate.ToString + " Receipt No.:- " + mIssue.IssueItems(l).ReceiptTextNo.ToString + ", ")
                                    Next
                                End If
                                mIssue.Delete()
                                mIssue.Save()
                                DataFieldBind()
                                SetControl()
                                SetTitle()
                                ControlEnability()
                                SetGrid() ' 'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
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
                                MarkLog(Util.Action.Delete, ModuleName, "Can't delete : " & mIssueDetail & " is Currently in use", Util.ErrorType.NoError, mIssue.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString)
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
                                'MarkLog(Util.Action.Delete, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString)
                                MarkLog(Util.Action.Delete, ModuleName, ListofIssueItems.ToString, Util.ErrorType.NoError, mIssue.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1-Jan-1900", _
                        Optional ByVal ToDate As String = "1-Jan-2099", Optional ByVal StoreName As String = "", Optional ByVal VendorName As String = "", _
                        Optional ByVal AircraftName As String = "", Optional ByVal IssueToType As Int32 = 0, Optional ByVal StatusID As Int32 = 0, _
                        Optional ByVal ReceiptText As String = "", Optional ByVal ReceiptNo As Int32 = 0, Optional ByVal RealeaseNoteNo As String = "", _
                        Optional ByVal SerialNo As String = "", Optional ByVal ItemName As String = "", Optional ByVal WorkShop As String = "", _
                        Optional ByVal WOText As String = "", Optional ByVal WONo As Int32 = 0, Optional ByVal CustomerName As String = "", _
                        Optional ByVal IsCustomerName As Boolean = False, Optional ByVal ReqText As String = "", Optional ByVal ReqNo As Integer = 0, _
                        Optional ByVal IssueOrderText As String = "", Optional ByVal OrderNo As Integer = 0, Optional ByVal Amend As String = "", _
                        Optional ByVal IsForPrint As Boolean = False, Optional ByVal ToStoreName As String = "", Optional ByVal BatchNo As String = "", Optional ByVal SearchText As String = "") 'Ajay SearchText 31-Jan-2023)
        mIssueList = Nothing
        Dim IsVendor As Integer
        If IsForPrint Then
            mIssueList = IssueList.GetIssueList(Text, No, FromDate, ToDate, StoreName, VendorName, AircraftName, IssueToType, StatusID, ReceiptText, ReceiptNo, RealeaseNoteNo, SerialNo, ItemName, , IsVendor, WorkShop, WOText, WONo, False, False, CustomerName, IsCustomerName, ReqText, ReqNo, IssueOrderText, OrderNo, Amend, IsCustomPaging:=False, CurrentPage:=mpageindex, PageSize:=mpageSize, ToStoreName:=ToStoreName, BatchNo:=BatchNo, SearchText:=SearchText) 'Ajay SearchText 31-Jan-2023
            Exit Sub
        Else
            mIssueList = IssueList.GetIssueList(Text, No, FromDate, ToDate, StoreName, VendorName, AircraftName, IssueToType, StatusID, ReceiptText, ReceiptNo, RealeaseNoteNo, SerialNo, ItemName, , IsVendor, WorkShop, WOText, WONo, False, False, CustomerName, IsCustomerName, ReqText, ReqNo, IssueOrderText, OrderNo, Amend, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize, ToStoreName:=ToStoreName, BatchNo:=BatchNo, SearchText:=SearchText) 'Ajay SearchText 31-Jan-2023
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
        dgIssueList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 31-Jan-2022
        UpdateIssueGridView()
    End Sub
    Private Sub UpdateIssueGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = "As per criteria : " & totalCount & " Record(s) found."
        Else
            'lblResult.Text = "As per criteria : " & currentrow + 1 & " to " & currentrow + mIssueList.Count & " of " & totalCount & " Record(s) found."
            lblResult.Text = "As per criteria : " & totalCount & " Record(s) found." 'Ajay 18-05-2023
        End If

        SliderExtender11.Minimum = 1
        SliderExtender11.Maximum = pagecount
        Slidercontrol.Text = mCurrentpage
        txtPageDisplay.Text = mCurrentpage
        lblpagecount.Text = pagecount
        If pagecount > 1 Then
            PnlPaging.Visible = True
        Else
            PnlPaging.Visible = False
        End If

        dgIssueList.DataBind()
        upnlGrid.Update()
        SetGrid()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer, Optional ByVal IsForPrint As Boolean = False, Optional ByVal IssueTypeId As String = "0")
        FindNow(Text:=Trim(IssueText), No:=CInt(Val(No)), FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, StoreName:=Trim(IssueFromStore), VendorName:=Trim(IssueVendorName), _
                AircraftName:=Trim(IssueAircraftName), IssueToType:=IssueTypeId, StatusID:=StatusId, ReceiptText:=Trim(ReceiptText), ReceiptNo:=CInt(Val(IssueReceiptNo)), _
                RealeaseNoteNo:=Trim(IssueReleaseNoteNoSearch), SerialNo:=Trim(IssueSerialNoSearch), ItemName:=Trim(IssuePartNoSearch), WorkShop:=Trim(IssueWorkShopName), _
                WOText:=Trim(WOText), WONo:=CInt(Val(IssueWoNo)), CustomerName:=Trim(IssueCustomerName), IsCustomerName:=IIf(IssueTypeId = "5", True, False), ReqText:=Trim(ReqText), _
                ReqNo:=CInt(Val(IssueReqNo)), IssueOrderText:=Trim(IssueOrderText), OrderNo:=CInt(Val(IssueOrderNo)), Amend:="", IsForPrint:=IsForPrint, _
                ToStoreName:=Trim(IssueToStoreName), BatchNo:=Trim(IssueBatchNoSearch), SearchText:=SearchText)
        'Select Case Index
        '    Case -1 'all
        '        FindNow(IsForPrint:=IsForPrint)
        '    Case 0 'all
        '        FindNow(IsForPrint:=IsForPrint)
        '    Case 1 'issue date
        '        FindNow(, , txtFromDate.Text, txtToDate.Text, IsForPrint:=IsForPrint)
        '    Case 2  'issue no
        '        FindNow(IssueText, CInt(Val(No)), IsForPrint:=IsForPrint)
        '    Case 3  'Receipt no
        '        FindNow(, , , , , , , , , ReceiptText, CInt(Val(No)), IsForPrint:=IsForPrint)
        '    Case 4 'Item name
        '        FindNow(, , , , , , , , , , , , , Trim(Name), IsForPrint:=IsForPrint)
        '    Case 5  'Store Name
        '        FindNow(, , , , Trim(Name), IsForPrint:=IsForPrint)
        '    Case 6  'Release note no
        '        FindNow(, , , , , , , , , , , Trim(Name), IsForPrint:=IsForPrint)
        '    Case 7  'serial no
        '        FindNow(, , , , , , , , , , , , Trim(Name), IsForPrint:=IsForPrint)
        '    Case 8  'Order No.
        '        FindNow(, , , , "", , , , , , , , , , , , , , , , , OrderText, CInt(Val(No)), txtAmend.Text, IsForPrint:=IsForPrint)
        '    Case 9  ''Status 1-incomplete 2-complete 3 authorize 4 cancel
        '        FindNow(, , , , , , , , StatusId, IsForPrint:=IsForPrint)
        '    Case 10 'Issue To
        '        Select Case IssueTypeId
        '            Case "0"
        '                FindNow(IssueToType:=IssueTypeId, IsForPrint:=IsForPrint)
        '            Case "1"  'Supplier
        '                FindNow(, , , , , Trim(Name), , IssueToType:=IssueTypeId, IsForPrint:=IsForPrint)
        '            Case "2"  'Aircraft
        '                FindNow(, , , , , , Trim(Name), IssueToType:=IssueTypeId, IsForPrint:=IsForPrint)
        '            Case "3"  'Store
        '                FindNow(, , , , , , , IssueToType:=IssueTypeId, IsForPrint:=IsForPrint, ToStoreName:=Trim(Name))
        '            Case "4"  'Discard
        '                FindNow(, , , , , , , IssueToType:=IssueTypeId, IsForPrint:=IsForPrint)
        '            Case "5" 'Customer
        '                FindNow(, , , , , , , IssueTypeId, , , , , , , , , , Trim(Name), True, IsForPrint:=IsForPrint)
        '            Case "6" 'WorkShop
        '                FindNow(, , , , , , , IssueTypeId, , , , , , , Trim(Name), IsForPrint:=IsForPrint)
        '            Case "7" 'Work Order
        '                FindNow(, , , , , , , IssueTypeId, , , , , , , , WOText, CInt(Val(No)), IsForPrint:=IsForPrint)
        '            Case "8" 'Requisition
        '                FindNow(, , , , "", , , IssueTypeId, , , , , , , , , , , , ReqText, CInt(Val(No)), IsForPrint:=IsForPrint)
        '        End Select
        '    Case 11  'Batch no
        '        FindNow(, , , , , , , , , , , , , IsForPrint:=IsForPrint, BatchNo:=Trim(Name))
        'End Select
        dgIssueList.PageIndex = 0   'Added Code on May,25,2007
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
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
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        'cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        'cmbIssueText.Visible = IIf(SearchIndex = 2, True, False)
        'cmbReceiptText.Visible = IIf(SearchIndex = 3, True, False)
        'cmbWoText.Visible = IIf(SearchIndex = 10 And cmbIssueToType.SelectedIndex = 7, True, False)
        'cmbRequisitionText.Visible = IIf(SearchIndex = 10 And cmbIssueToType.SelectedIndex = 8, True, False) 'Added by vikrant For New Requisition
        'lblNo.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0 Or SearchIndex = 3 And cmbReceiptText.SelectedIndex <> 0 Or cmbIssueToType.SelectedIndex = 7 And cmbWoText.SelectedIndex <> 0 Or cmbIssueToType.SelectedIndex = 8 And cmbRequisitionText.SelectedIndex <> 0 Or SearchIndex = 8 And cmbOrderText.SelectedIndex <> 0, True, False)
        'txtNo.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0 Or SearchIndex = 3 And cmbReceiptText.SelectedIndex <> 0 Or cmbIssueToType.SelectedIndex = 7 And cmbWoText.SelectedIndex <> 0 Or cmbIssueToType.SelectedIndex = 8 And cmbRequisitionText.SelectedIndex <> 0 Or SearchIndex = 8 And cmbOrderText.SelectedIndex <> 0, True, False)
        'txtName.Visible = IIf(SearchIndex = 4 Or SearchIndex = 5 Or SearchIndex = 6 Or SearchIndex = 7 Or SearchIndex = 11 Or cmbIssueToType.SelectedIndex = 1 Or cmbIssueToType.SelectedIndex = 2 Or cmbIssueToType.SelectedIndex = 3 Or cmbIssueToType.SelectedIndex = 5 Or cmbIssueToType.SelectedIndex = 6, True, False)
        'cmbIssueToType.Visible = IIf(SearchIndex = 10, True, False)
        'cmbStatus.Visible = IIf(SearchIndex = 9, True, False)
        'cmbOrderText.Visible = IIf(SearchIndex = 8, True, False) 'Added By Prashant 11-Jun-2014 ALL11062014-1
        'txtAmend.Visible = IIf(SearchIndex = 8 And cmbOrderText.SelectedIndex <> 0, True, False)
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
        txtSearchBox.Visible = True 'Ajay 31-Jan-2023
    End Sub
    Private Sub ClearControls()
        'txtNo.Text = ""
        'txtName.Text = ""
        txtAmend.Text = ""
    End Sub
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        IssueTypeId = IIf(cmbIssueToType.SelectedIndex <= 0, 0, cmbIssueToType.SelectedValue)

        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedValue)
        ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)
        WOText = IIf(cmbWoText.SelectedIndex <= 0, "", cmbWoText.SelectedValue)
        ReqText = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue) 'Added by vikrant For New Requisition
        IssueOrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue) 'Added By Prashant 11-Jun-2014 ALL11062014-1

        IssueOrderNo = txtOrderNo.Text.Trim
        IssueReceiptNo = txtReceiptNo.Text.Trim
        No = txtIssueNo.Text.Trim
        IssueWoNo = txtWONo.Text.Trim
        IssueReqNo = txtReqNo.Text

        IssuePartNoSearch = txtPartNoSearch.Text.Trim
        IssueReleaseNoteNoSearch = txtReleaseNoteNoSearch.Text.Trim
        IssueSerialNoSearch = txtSerialNoSearch.Text.Trim
        IssueBatchNoSearch = txtBatchNoSearch.Text.Trim
        IssueFromStore = txtFromStore.Text.Trim

        Select Case IssueTypeId 'Issue to
            Case "0"
                IssueVendorName = ""
                IssueAircraftName = ""
                IssueToStoreName = ""
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "1"  'Supplier
                IssueVendorName = txtSearchFor.Text.Trim
                IssueAircraftName = ""
                IssueToStoreName = ""
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "2"  'Aircraft
                IssueVendorName = ""
                IssueAircraftName = txtSearchFor.Text.Trim
                IssueToStoreName = ""
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "3"  'Store
                IssueVendorName = ""
                IssueAircraftName = ""
                IssueToStoreName = txtSearchFor.Text.Trim
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "4" 'Discard

            Case "5" 'Customer
                IssueVendorName = ""
                IssueAircraftName = ""
                IssueToStoreName = ""
                IssueCustomerName = txtSearchFor.Text.Trim
                IssueWorkShopName = ""
            Case "6" 'WorkShop
                IssueVendorName = ""
                IssueAircraftName = ""
                IssueToStoreName = ""
                IssueCustomerName = ""
                IssueWorkShopName = txtSearchFor.Text.Trim
            Case "7" 'Work Order
            Case "8" 'Requisition
        End Select
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId

        Session("IssueText") = IssueText
        Session("ReceiptText") = ReceiptText
        Session("WOText") = WOText
        Session("ReqText") = ReqText
        Session("IssueOrderText") = IssueOrderText

        Session("IssueTypeId") = IssueTypeId

        Session("Name") = Name

        Session("No") = No
        Session("IssueOrderNo") = IssueOrderNo
        Session("IssueReceiptNo") = IssueReceiptNo
        Session("IssueWoNo") = IssueWoNo
        Session("IssueReqNo") = IssueReqNo

        Session("IssueVendorName") = IssueVendorName
        Session("IssueAircraftName") = IssueAircraftName
        Session("IssueToStoreName") = IssueToStoreName
        Session("IssueCustomerName") = IssueCustomerName
        Session("IssueWorkShopName") = IssueWorkShopName
        Session("IssuePartNoSearch") = IssuePartNoSearch
        Session("IssueReleaseNoteNoSearch") = IssueReleaseNoteNoSearch
        Session("IssueSerialNoSearch") = IssueSerialNoSearch
        Session("IssueBatchNoSearch") = IssueBatchNoSearch
        Session("IssueFromStore") = IssueFromStore

        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text) 'Ajay 31-01-2023
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

        setPeriod(DateIndex)
        CallFindNow(SearchIndex, , IssueTypeId)
        dgIssueList.DataBind()
        'cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId

        cmbIssueText.SelectedValue = IIf(IssueText = "", "(All)", IssueText)
        cmbReceiptText.SelectedValue = IIf(ReceiptText = "", "(All)", ReceiptText)
        cmbWoText.SelectedValue = IIf(WOText = "", "(All)", WOText)
        cmbRequisitionText.SelectedValue = IIf(ReqText = "", "(All)", ReqText)
        cmbOrderText.SelectedValue = IIf(IssueOrderText = "", "(All)", IssueOrderText)

        cmbIssueToType.SelectedValue = IssueTypeId
        'txtName.Text = Name
        'txtNo.Text = No
        ControlVisibility(1, DateIndex)
        'lblResult.Text = "List of Issue as per criteria :" & mIssueList.Count & " Record(s) found."
        cmbIssueTo.SelectedIndex = IssueTo
        mIssueTypeList = IssueTypeList.GetIssueTypeList(cmbIssueTo.SelectedIndex)
        cmbIssueAs.Enabled = IIf(mIssueTypeList.Count = 0, False, True)
        'btnAddNew.Enabled = IIf(mIssueTypeList.Count = 0, False, True) ''Ajay 31-01-2022
        btnAddNewTop.Enabled = IIf(mIssueTypeList.Count = 0, False, True)
        cmbIssueAs.DataSource = mIssueTypeList
        cmbIssueAs.DataBind()
		Session("mIssueTypeList") = mIssueTypeList

		If IssueAs >= 0 AndAlso IssueAs < cmbIssueAs.Items.Count Then
			cmbIssueAs.SelectedIndex = IssueAs
		Else
			cmbIssueAs.SelectedIndex = 0
		End If
		'cmbIssueAs.SelectedIndex = IssueAs

		Select Case IssueTypeId 'Received From
            Case "0"
                IssueVendorName = ""
                IssueAircraftName = ""
                IssueToStoreName = ""
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "1"  'Supplier
                txtSearchFor.Text = IssueVendorName
                IssueAircraftName = ""
                IssueToStoreName = ""
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "2"  'Aircraft
                IssueVendorName = ""
                txtSearchFor.Text = IssueAircraftName
                IssueToStoreName = ""
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "3"  'Store
                IssueVendorName = ""
                IssueAircraftName = ""
                txtSearchFor.Text = IssueToStoreName
                IssueCustomerName = ""
                IssueWorkShopName = ""
            Case "4" 'Discard
            Case "5" 'Customer
                IssueVendorName = ""
                IssueAircraftName = ""
                IssueToStoreName = ""
                txtSearchFor.Text = IssueCustomerName
                IssueWorkShopName = ""
            Case "6" 'WorkShop
                IssueVendorName = ""
                IssueAircraftName = ""
                IssueToStoreName = ""
                IssueCustomerName = ""
                txtSearchFor.Text = IssueWorkShopName
            Case "7" 'Work Order
        End Select
        txtOrderNo.Text = IssueOrderNo
        txtReceiptNo.Text = IssueReceiptNo
        txtIssueNo.Text = No
        txtWONo.Text = IssueWoNo
        txtReqNo.Text = IssueReqNo
        txtPartNoSearch.Text = IssuePartNoSearch
        txtReleaseNoteNoSearch.Text = IssueReleaseNoteNoSearch
        txtSerialNoSearch.Text = IssueSerialNoSearch
        txtBatchNoSearch.Text = IssueBatchNoSearch
        txtFromStore.Text = IssueFromStore

        'Ajay 31-Jan-2023
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub SetTitle()
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        If mIssue Is Nothing Then
            ModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        Else
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
        End If

        Session("ModuleName") = ModuleName
        ' lblTitle.Text = "List of Issue " + "    [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]" 'Added by shweta on 23-12-11
        lblTitle.Text = "List of Issue " 'Ajay 18-05-2023
    End Sub
    Private Sub ControlEnability()
        'BtnPrint.Enabled = IIf(dgIssueList.Rows.Count = 0, False, True) ''Ajay 31-01-2022
        btnPrintTop.Enabled = IIf(dgIssueList.Rows.Count = 0, False, True)

        'Ajay 08-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "IssueToAircraft") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
    End Sub
    Private Sub addAttributes()
        'txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights, Optional ByVal Str As String = "") As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        Select Case mTransTypeID
            Case Util.Trans.IssueToAircraft
                IsInRoleString = "IssueToAircraft"
            Case Util.Trans.IssueToStore
                IsInRoleString = "IssueToStore"
            Case Util.Trans.ExchangeRepairIssueToVendor
                IsInRoleString = "IssueToVendorForExchange"
            Case Util.Trans.LoanIssueToStore
                IsInRoleString = "IssueLoanToStore"
            Case Util.Trans.LoanIssuedToAircraft
                IsInRoleString = "IssueLoanToAircraft"
            Case Util.Trans.LoanReturnToStore
                IsInRoleString = "IssueLoanReturnToStore"
            Case Util.Trans.DisacrdPart
                IsInRoleString = "IssueToDiscard"
            Case Util.Trans.IssueToCustomer
                IsInRoleString = "IssueToCustomer"
            Case Util.Trans.LoanIssueToCustomer
                IsInRoleString = "LoanIssueToCustomer"
            Case Util.Trans.LoanIssueToVendor
                IsInRoleString = "LoanIssueToVendor"
            Case Util.Trans.IssueToWorkShop
                IsInRoleString = "IssueToWorkShop"
            Case Util.Trans.LoanIssueToWorkShop
                IsInRoleString = "IssueLoanToWorkShop"
            Case Util.Trans.IssueforLoanReturntoSupplier
                IsInRoleString = "IssueforLoanReturntoSupplier"
            Case Util.Trans.IssuetoSupplierasRentalLease             'Added By Prashant 6-Jan-2010
                IsInRoleString = "IssuetoSupplierasRentalLease"
            Case Util.Trans.IssueToWorkOrderAsSpares
                If cmbIssueAs.SelectedValue = "443" Or Str = "Issue To WorkOrder As Spare Req." Then 'Issue to work order as Material Requisition Added By Prashant on 25-Jun-2021 STR25062021
                    IsInRoleString = "IssuetoworkorderasSparerequisition"
                Else
                    IsInRoleString = "IssueToWorkOrderAsSpares"
                End If
            Case Util.Trans.IssueToWorkOrderAsTools
                IsInRoleString = "IssueToWorkOrderAsTools"
            Case Util.Trans.IssueToWorkOrder
                IsInRoleString = "IssueToWorkOrder"
            Case Util.Trans.IssuetoSupplierNone
                IsInRoleString = "IssuetoSupplierNone"
                'Case Util.Trans.IssueToRequisition  'Added by vikrant For New Requisition
                'IsInRoleString = "IssueToRequisition"
            Case Util.Trans.IssueforLoanReturntoCustomer
                IsInRoleString = "IssueforLoanReturntoCustomer"
            Case Util.Trans.IssueToCustomerAsRepairedReturn
                IsInRoleString = "IssueToCustomerAsRepairedReturn"
            Case Util.Trans.IssueToCustomerAsNone
                IsInRoleString = "IssueToCustomerAsNone"
            Case Util.Trans.IssuetoSupplierasReturn
                IsInRoleString = "IssuetoSupplierasReturn"
        End Select
        'Depending upon decided IsInRole String; checkign Rights of the User
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
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
    Private Sub SetGrid()
        Dim P As Integer
        For j As Integer = 0 To dgIssueList.Rows.Count - 1
            P = CType(Me.dgIssueList.Rows.Item(j).Cells(11).Text, Integer) '' Added by Ajay 01-02-2023  [Cells 12]
            If P <= 0 Then
                dgIssueList.Rows.Item(j).Cells(10).Enabled = False '' Added by Ajay 01-02-2023  [Cells 11]
            End If
        Next
    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            'If cmbSearch.Enabled = True Then
            'cmbSearch.Focus()
            'End If
            Session.Remove("mPendingItemList")
            mTransTypeID = Request.QueryString("TransTypeId")
            Session("mTransTypeId") = mTransTypeID
            Session("MiddleFrame") = "wfIssueList_Ajax.aspx?TransTypeId=" & mTransTypeID
            ''Added by vikrant For New Requisition
            'If AppSettings("NewRequisition") = "True" Then
            '    cmbIssueTo.Items.Add("Requisition")
            'End If
            ''End
            cmbShowE.SelectedValue = "4" 'Ajay 31-Jan-2023
            DataFieldBind()
            SetControl()
            SetTitle()
            ControlEnability()
            SetGrid() ' 'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
        End If
    End Sub
    Private Sub dgIssueList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssueList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgIssueList.PageIndex * dgIssueList.PageSize
                mTransTypeID = mIssueList(index).TransType
                If (Not IsInRole(Rights.View, Str:=mIssueList(index).IssueType) And Not IsInRole(Rights.Edit, Str:=mIssueList(index).IssueType)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    GridBind()
                    Exit Sub
                End If
                GridBind()

                Dim mId As Guid = mIssueList(index).ID
                Dim mDate As String = mIssueList(index).ILDateFormatted.ToString
                Dim mIssueNo As String = mIssueList(index).IssueNo
                mIssueDetail = mIssueNo + " Dated : " + mDate + " to " + mIssueList(mId).Destination
                EditRecord(mId)
                Session("IsForWOReturn") = False
                Session("Edit") = True

                'Added By Prashant 20-Jul-2011
                mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueList(mIssue.ID).Destination
                MarkLog(Util.Action.Edit, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfIssue_Ajax.aspx?BackPage=wfIssueList_Ajax.aspx');", True)
            Case "DeleteRec"
                Dim index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgIssueList.PageIndex * dgIssueList.PageSize
                mTransTypeID = mIssueList(index).TransID
                If (Not IsInRole(Rights.Delete, Str:=mIssueList(index).IssueType)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    GridBind()
                    Exit Sub
                End If


                Dim mId As Guid = mIssueList(index).ID
                DeleteRecord(mId)
                GridBind()
                'Added By Utkarsh ON 18-Oct-2012 FOR ALL18102012
            Case "ViewRec"

                'Comment by Sankalp 29-09-25
                'If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
                '    GridBind()
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                '    GridBind()
                '    Exit Sub
                'End If
                'Dim No As New Random
                'Dim StrName As String = "abc" & No.Next.ToString
                'Dim index As Integer = CInt(e.CommandArgument) ' CInt(e.CommandArgument) + dgIssueList.PageIndex * dgIssueList.PageSize
                'Dim mId As Guid = mIssueList(index).ID
                'mIssue = Issue.GetIssue(mId)
                'If mIssue.Size > 0 Then
                '    'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mManual.FileExtension
                '    Dim path As String = AppSettings("DOCPath") & StrName & mIssue.Extension
                '    Dim fs As FileStream
                '    If File.Exists(AppSettings("DOCPath")) = False Then
                '        'Delete File if exist
                '        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mIssue.Extension)
                '        ' Create the file.
                '        fs = File.Create(path)
                '        '' Add some information to the file.
                '        fs.Write(mIssue.ImageFile, 0, mIssue.ImageFile.Length)
                '        fs.Close()
                '        Session("DOCPath") = path
                '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                '    End If
                'End If
                'End
                Dim index As Integer = CInt(e.CommandArgument)
                Dim mId As Guid = mIssueList(index).ID
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim mFileAttachments As New FileAttachments
                mFileAttachments = FileAttachments.GetChildFileAttachments(mID)
                Dim AttachmentCount As Integer = mFileAttachments.Count
                If AttachmentCount > 1 Then
                    'Session("mFileAttachments") = mnWO.FileAttachments
                    Session("mFileAttachments") = mFileAttachments
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

                Else
                    Dim mFileAttach As FileAttach
                    'Dim No As New Random
                    'Dim StrName As String = "abc" & No.Next.ToString

                    mFileAttach = FileAttach.GetAttachment(mID)
                    If mFileAttach.Size > 0 Then
                        Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
                        Dim fs As FileStream
                        If File.Exists(AppSettings("DOCPath")) = False Then
                            'Delete File if exist
                            IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                            ' Create the file.
                            fs = File.Create(path)
                            '' Add some information to the file.
                            fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                            fs.Close()
                            Session("DOCPath") = path
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
							Dim Detail As String = "Issue Attachment( " + mFileAttach.FileName + ") viewed by  " + User.Identity.Name
							MarkLog(Action.View, "Issue ", Detail, ErrorType.HandledError, mId, EventLogID)
						End If
                    End If
                End If
                GridBind()
        End Select
        SetGrid()
    End Sub
    Private Sub dgIssueList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgIssueList.PageIndexChanged

        dgIssueList.PageIndex = e.NewPageIndex
        dgIssueList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 11-Jan-2023
        dgIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList
        dgIssueList.DataBind()
        SetGrid()
        '-----------------
        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgIssueList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = e.NewPageIndex
        pagecount = CInt(Session("pagecount"))

        'dgIssueList.PageIndex = e.NewPageIndex
        'mCurrentpage = e.NewPageIndex
        'Session("mIssueList") = mIssueList
        'dgIssueList.DataSource = mIssueList
        'dgIssueList.DataBind()
        'dgIssueList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 31-Jan-2023
        'SetGrid()
    End Sub
    'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
    '    ClearControls()
    '    cmbDate.SelectedIndex = 0
    '    cmbIssueText.SelectedIndex = 0
    '    cmbReceiptText.SelectedIndex = 0
    '    cmbWoText.SelectedIndex = 0
    '    cmbRequisitionText.SelectedIndex = 0 'Added by vikrant For New Requisition
    '    cmbIssueToType.SelectedIndex = 0
    '    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
    '    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
    '    setPeriod(DateIndex)
    '    If cmbSearch.Enabled = True Then
    '        cmbSearch.Focus()
    '    End If
    'End Sub
    Private Sub cmbIssueToType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbIssueToType.SelectedIndexChanged
        'ClearControls()
        txtSearchFor.Text = ""
        cmbDate.SelectedIndex = 0
        cmbIssueText.SelectedIndex = 0
        cmbReceiptText.SelectedIndex = 0
        cmbWoText.SelectedIndex = 0
        cmbRequisitionText.SelectedIndex = 0
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(1, DateIndex)
        setPeriod(DateIndex)
        'If cmbSearch.Enabled = True Then
        '    cmbSearch.Focus()
        'End If
    End Sub
    Private Sub cmbIssueText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIssueText.SelectedIndexChanged, cmbReceiptText.SelectedIndexChanged, cmbWoText.SelectedIndexChanged, cmbRequisitionText.SelectedIndexChanged, cmbOrderText.SelectedIndexChanged
        If sender.ID = "cmbIssueText" Then
            txtIssueNo.Text = "0"
            If cmbIssueText.Enabled = True Then
                cmbIssueText.Focus()
            End If
        ElseIf sender.ID = "cmbReceiptText" Then
            txtReceiptNo.Text = "0"
            If cmbReceiptText.Enabled = True Then
                cmbReceiptText.Focus()
            End If
        ElseIf sender.ID = "cmbWoText" Then
            txtWONo.Text = "0"
            If cmbWoText.Enabled = True Then
                cmbWoText.Focus()
            End If
        ElseIf sender.id = "cmbRequisitionText" Then
            txtReqNo.Text = "0"
            If cmbRequisitionText.Enabled = True Then
                cmbRequisitionText.Focus()
            End If
        ElseIf sender.id = "cmbOrderText" Then
            txtOrderNo.Text = "0"
            If cmbOrderText.Enabled = True Then
                cmbOrderText.Focus()
            End If
        End If
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(1, DateIndex)
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        'Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(1, DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            cmbDate.Focus()
        End If
    End Sub
    Private Sub cmbIssueTo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIssueTo.SelectedIndexChanged
        mIssueTypeList = IssueTypeList.GetIssueTypeList(cmbIssueTo.SelectedIndex)
        cmbIssueAs.Enabled = IIf(mIssueTypeList.Count = 0, False, True)
        'btnAddNew.Enabled = IIf(mIssueTypeList.Count = 0, False, True) ''Ajay 31-01-2022
        btnAddNewTop.Enabled = IIf(mIssueTypeList.Count = 0, False, True)

        cmbIssueAs.DataSource = mIssueTypeList
        cmbIssueAs.DataBind()
        Session("mIssueTypeList") = mIssueTypeList
        If cmbIssueTo.Enabled = True Then
            cmbIssueTo.Focus()
        End If
        upnlTitle.Update()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        dgIssueList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex, , IssueTypeId)
        'BtnPrint.Enabled = IIf(mIssueList.Count = 0, False, True) ''Ajay 31-01-2022
        btnPrintTop.Enabled = IIf(mIssueList.Count = 0, False, True)
        upnlTitle.Update()
        upnlActionBtn.Update()
        upnlActionBtnBottom.Update()
        upnlIssueTo.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click 'btnAddNew.Click,'Ajay 31-01-2022

        mIssueTypeList = Session("mIssueTypeList")
        mTransTypeID = mIssueTypeList.Item(cmbIssueAs.SelectedIndex).ID
        NewRecord()

        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            GridBind()
            Exit Sub
        End If
        Session("mTransTypeID") = mTransTypeID
        Session("mPendingAgainst") = 1
        SetTitle()

        MarkLog(Util.Action.[New], TransactionList.GetTransactionList().GetTransactionTypeName(mIssue.TransTypeID).ToString, "", Util.ErrorType.NoError, mIssue.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mIssue.TransTypeID).ToString())  'Added By Prashant 20-Jul-2011

        Session("ISForWOReturn") = False 'Used for updating Returned Qty from WO  to Issue
        If mTransTypeID = 16 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingToReturnForExchangeRepair_Ajax.aspx?BackPage=index.aspx');", True)
        ElseIf mTransTypeID = 18 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingLoanToReturn_Ajax.aspx?BackPage=index.aspx');", True)
        ElseIf mTransTypeID = 49 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingLoanToReturn_Ajax.aspx?BackPage=index.aspx');", True)
        ElseIf mTransTypeID = 51 Or mTransTypeID = 58 Then  '58 Added By Prashant 21-May-2010
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingLoanToReturn_Ajax.aspx?BackPage=index.aspx');", True)
        ElseIf mTransTypeID = 55 Then        'Added By Prashant 6-Jan-2010
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingLoanToReturn_Ajax.aspx?BackPage=index.aspx');", True)
        ElseIf mTransTypeID = 59 Then        'Added By Saylee 8-Dec-2010
            'If cmbIssueAs.SelectedIndex = 3 And cmbIssueAs.SelectedValue = "443" Then 'Issue to work order as Material Requisition Added By Prashant on 25-Jun-2021 STR25062021
            If cmbIssueAs.SelectedValue = "443" Then 'Issue to work order as Material Requisition Added By Prashant on 25-Jun-2021 STR25062021
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRequisitionItemListForIssue_Ajax.aspx?BackPage=index.aspx');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfnPendingWOListForIssueSpares_Ajax.aspx?BackPage=index.aspx');", True)
            End If
        ElseIf mTransTypeID = 60 Then        'Added By Prashant 8-Dec-2010
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfnPendingWOListForIssueTools_Ajax.aspx?BackPage=index.aspx');", True)
            'Added by vikrant For New Engg Requisition
            'ElseIf mTransTypeID = 14 And cmbIssueAs.SelectedIndex = 2 Then 
        ElseIf mTransTypeID = 14 And cmbIssueAs.SelectedItem.Text = "Requisition" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRequisitionItemListForIssue_Ajax.aspx?BackPage=index.aspx');", True)
            'Added by vikrant For New WorkShop Requisition
            'ElseIf mTransTypeID = 44 And cmbIssueAs.SelectedIndex = 2 Then
        ElseIf mTransTypeID = 44 And cmbIssueAs.SelectedItem.Text = "Requisition" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRequisitionItemListForIssue_Ajax.aspx?BackPage=index.aspx');", True)
            'ElseIf mTransTypeID = 19 And cmbIssueAs.SelectedIndex = 1 Then 'Added By Prashant 20-Jul-2016 Issue To discard As Expired
        ElseIf mTransTypeID = 19 And cmbIssueAs.SelectedItem.Text = "Expired" Then 'Added By Prashant 20-Jul-2016 Issue To discard As Expired
            Session("IssueToDiscardAsExpired") = cmbIssueAs.SelectedValue  '192 is SelectedValue
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfIssue_Ajax.aspx?BackPage=index.aspx');", True)
        Else
            Session("IssueToDiscardAsExpired") = "0"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfIssue_Ajax.aspx?BackPage=index.aspx');", True)
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click 'btnClose.Click, 'Ajay 31-01-2022
        RemoveSession()
        Session("MiddleFrame") = ""
        ModuleName = Nothing
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Added By Prashant 18-June-2009
    Private Sub dgIssueList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgIssueList.Sorting
        mIssueList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgIssueList.DataSource = mIssueList
        Session("mIssueList") = mIssueList
        dgIssueList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Ajay 08-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 07-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "IssueToAircraft")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 07-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "IssueToAircraft")
    End Sub
    '-----
#End Region

#Region " Report "
    'Created By :- Jyoti
    'Dated On 11/5/2007
#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region " Event "
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
	'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click 'BtnPrint.Click,  'Ajay 31-01-2023
	'    'For Issue List
	'    Dim Rpt As New crIssueList
	'    Dim da As New CSLA.Data.ObjectAdapter
	'    Dim ds As New dsCommon
	'    Dim ReportDetails As New rptStatusList
	'    'Dim Title As String = GetTitle()
	'    SearchStr1 = "" '"The report shows records filtered by the following criteria"
	'    SearchStr2 = ""
	'    'If cmbSearch.SelectedIndex = 0 Then
	'    '    'All
	'    '    SearchStr1 = ""
	'    '    SearchStr2 = ""
	'    'ElseIf cmbSearch.SelectedIndex = 1 Then
	'    '    'Date
	'    '    If cmbDate.SelectedIndex = 0 Then
	'    '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
	'    '    ElseIf cmbDate.SelectedIndex = 6 Then
	'    '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
	'    '    Else
	'    '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
	'    '    End If
	'    'ElseIf cmbSearch.SelectedIndex = 2 Then
	'    '    'Issue No.
	'    '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
	'    'ElseIf cmbSearch.SelectedIndex = 3 Then
	'    '    'Receipt No.
	'    '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbReceiptText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
	'    'ElseIf cmbSearch.SelectedIndex = 4 Or cmbSearch.SelectedIndex = 5 Or cmbSearch.SelectedIndex = 6 Or cmbSearch.SelectedIndex = 7 Or cmbSearch.SelectedIndex = 11 Then
	'    '    'Part Number, From Store, Release Note No., Serial No., Batch No
	'    '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
	'    'ElseIf cmbSearch.SelectedIndex = 8 Then
	'    '    'Order No.
	'    '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbOrderText.SelectedItem.Text + ":" + " " + cmbWoText.SelectedItem.Text + "    " + lblNo.Text + " " + txtNo.Text + " " + txtAmend.Text
	'    'ElseIf cmbSearch.SelectedIndex = 9 Then
	'    '    'Status
	'    '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
	'    'ElseIf cmbSearch.SelectedIndex = 10 Then
	'    '    'Issue To
	'    '    If cmbIssueToType.SelectedIndex = 7 Then
	'    '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueToType.SelectedItem.Text + ":" + " " + cmbWoText.SelectedItem.Text + "    " + lblNo.Text + " " + txtNo.Text
	'    '    ElseIf cmbIssueToType.SelectedIndex = 8 Then
	'    '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueToType.SelectedItem.Text + ":" + " " + cmbRequisitionText.SelectedItem.Text + "    " + lblNo.Text + " " + txtNo.Text
	'    '    Else
	'    '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueToType.SelectedItem.Text + ":" + " " + txtName.Text
	'    '    End If
	'    'End If

	'    ReportDetails.Add(New rptStatus(, 0, , _
	'          dgIssueList.Columns.Item(1).HeaderText, dgIssueList.Columns.Item(2).HeaderText, dgIssueList.Columns.Item(3).HeaderText, _
	'          dgIssueList.Columns.Item(4).HeaderText, dgIssueList.Columns.Item(5).HeaderText, dgIssueList.Columns.Item(6).HeaderText, _
	'          dgIssueList.Columns.Item(7).HeaderText))

	'    CallFindNow(1, IsForPrint:=True, IssueTypeId:=IssueTypeId)

	'    For k As Integer = 0 To mIssueList.Count - 1
	'        ReportDetails.Add(New rptStatus(, 1, , mIssueList(k).ILDateFormatted, mIssueList(k).IssueNo, mIssueList(k).IssueType, mIssueList(k).StoreName, mIssueList(k).Destination, mIssueList(k).StatusName, mIssueList(k).AuthorizedByName))
	'    Next

	'    mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
	'    Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
	'    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
	'    mCompanyDetail.WebSite, "Issue List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

	'    If mIssueList.Count = 0 Then
	'        MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
	'        Exit Sub
	'    End If
	'    Dim mrptImage As rptImage = rptImage.GetImage(ds)
	'    da.Fill(ds, mrptImage)
	'    da.Fill(ds, ReportDetails)
	'    da.Fill(ds, Report)
	'    Rpt.SetDataSource(ds)
	'    Session("CrystalReport") = Rpt

	'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
	'End Sub
	Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrintTop.Click
		Dim ColumnsCount As Integer = dgIssueList.Columns.Count - 1
		Dim ColumnHeaders(ColumnsCount) As String
		Try
			If Not IsInRole(Rights.Print) Then

				MSGBoxCtrl.Show(MSGBox.Message_title.Authorization,
								MSGBox.Message_text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			CallFindNow(1, IsForPrint:=True, IssueTypeId:=IssueTypeId)
			mIssueList = Session("mIssueList")
			If mIssueList.Count = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
								MSGBox.Message_text.NoRecordFound,
								"There is no record for this search criteria",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub
			End If
			For i As Integer = 0 To ColumnsCount
				ColumnHeaders(i) = dgIssueList.Columns.Item(i).HeaderText
			Next
			Dim Result = ReportHelper.ListReport(List:=mIssueList, ColumnHeaders:=ColumnHeaders,
													IsForAPI:=False, ReportOf:="IssueList")


			Session("myReport") = CType(Result.Item1, Engine.ReportClass)

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"openTranDetail",
												"openTranDetail();",
												True)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	Private Sub btnGridPaging_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        dgIssueList.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(1, , IssueTypeId)
    End Sub

    'Added by Ajay 31-Jan-2023
    Private Sub txtSearchBox_TextChanged(sender As Object, e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        SetControl()
        dgIssueList.DataBind()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnBottom.Update()
    End Sub
    '-----
    'Added by Ajay 31-Jan-2023
    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim ExpiryDateList = ((From res In mWOList).ToList.Take(CInt(DropDownList1.SelectedItem.ToString))).ToList
        dgIssueList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        'dgIssueList.DataSource = mIssueList
        'dgIssueList.DataBind()

        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgIssueList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        pagecount = CInt(Session("pagecount"))

        SetControl()
        upnlGrid.Update()
        upnlActionBtnBottom.Update()
    End Sub
#End Region

#End Region

End Class