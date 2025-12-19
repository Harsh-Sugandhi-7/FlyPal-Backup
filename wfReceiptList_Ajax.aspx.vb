Public Class wfReceiptList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mReceiptList As ReceiptList
    Public mReceipt As Receipt
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptReceiptReg
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, ReceiptOrderText, ReceiptText, Name, ReceiptOrderNo, ReceiptNo, _
        ReceiptInternalReceiptNoSearch, ReceiptDCNoSearch, ReceiptPartNoSearch, ReceiptDescriptionSearch, ReceiptReleaseNoteNoSearch, ReceiptCustomBillofEntrySearch, _
        ReceiptSerialNoSearch, ReceiptBatchNoSearch, ReceiptSupplier, SearchText As String
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
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReceipt = Session("mReceipt")
        mReceiptList = Session("mReceiptList")
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
        ReceiptOrderText = Session("ReceiptOrderText")
        ReceiptText = Session("ReceiptText")
        Name = Session("Name")
        ReceiptOrderNo = IIf(IsNothing(Session("ReceiptOrderNo")), 0, Session("ReceiptOrderNo"))
        ReceiptNo = IIf(IsNothing(Session("ReceiptNo")), 0, Session("ReceiptNo"))
        ReceiptTypeID = Session("ReceiptTypeID") 'Changes by Kalpesh Shah as on 23-01-2008
        mModuleName = Session("mModuleName") 'Added By Utkarsh On 21-Jul-2011 For All19072011
        mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        mFileAttach = Session("mFileAttach")

        ReceiptInternalReceiptNoSearch = Session("ReceiptInternalReceiptNoSearch")
        ReceiptDCNoSearch = Session("ReceiptDCNoSearch")
        ReceiptPartNoSearch = Session("ReceiptPartNoSearch")
        ReceiptDescriptionSearch = Session("ReceiptDescriptionSearch")
        ReceiptReleaseNoteNoSearch = Session("ReceiptReleaseNoteNoSearch")
        ReceiptCustomBillofEntrySearch = Session("ReceiptCustomBillofEntrySearch")
        ReceiptSerialNoSearch = Session("ReceiptSerialNoSearch")
        ReceiptBatchNoSearch = Session("ReceiptBatchNoSearch")
        ReceiptSupplier = Session("ReceiptSupplier")
        SearchText = Session("SearchText") 'Ajay 18-Jan-2023
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mReceiptList")
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
        Session.Remove("mFileAttach")

        Session.Remove("ReceiptInternalReceiptNoSearch")
        Session.Remove("ReceiptDCNoSearch")
        Session.Remove("ReceiptPartNoSearch")
        Session.Remove("ReceiptDescriptionSearch")
        Session.Remove("ReceiptReleaseNoteNoSearch")
        Session.Remove("ReceiptCustomBillofEntrySearch")
        Session.Remove("ReceiptSerialNoSearch")
        Session.Remove("ReceiptBatchNoSearch")
        Session.Remove("ReceiptSupplier")

        Session.Remove("SearchText") 'Ajay 18-Jan-2023
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfReceiptList_Ajax.aspx?") <= 0 Then
            Session.Remove("mReceiptList")
            Session.Remove("mReceipt")
            Session.Remove("mDistinctTextListForOrder")
            Session.Remove("mDistinctTextListForReceipt")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("ReceiptOrderText")
            Session.Remove("ReceiptText")
            Session.Remove("Name")
            Session.Remove("ReceiptOrderNo")
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
            Session.Remove("mFileAttach")

            Session.Remove("ReceiptInternalReceiptNoSearch")
            Session.Remove("ReceiptDCNoSearch")
            Session.Remove("ReceiptPartNoSearch")
            Session.Remove("ReceiptDescriptionSearch")
            Session.Remove("ReceiptReleaseNoteNoSearch")
            Session.Remove("ReceiptCustomBillofEntrySearch")
            Session.Remove("ReceiptSerialNoSearch")
            Session.Remove("ReceiptBatchNoSearch")
            Session.Remove("ReceiptSupplier")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachmentChild(ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub NewRecord()
        mTransTypeId = CType(cmbReceiptType.SelectedValue, Util.Trans)
        Session("mTransTypeId") = mTransTypeId
        mReceipt = Receipt.NewReceipt(mTransTypeId)
        If CType(mTransTypeId, Trans) = Util.Trans.RCIFromSupplierAsNone Then 'Added by Prashant 5-Dec-2018 ALL05122018 
            'Do nothing 
        Else
            mReceipt.ReceiptItems.Add(mReceipt.ID, mReceipt.TransTypeID)
            mReceipt.ReceiptItems.CurrentIndex = mReceipt.ReceiptItems.Count - 1
        End If
        Session("mReceipt") = mReceipt
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mReceipt = Receipt.GetReceipt(mID)
        mReceipt.MarkClean()
        Session("mReceipt") = mReceipt
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        
        GridBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        'Ajay 07-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
        mReceipt = Receipt.GetReceipt(mID)
        Session("mReceipt") = mReceipt
    End Sub
    Private Sub SetControl()
        SetPeriod(DateIndex)

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgReceiptList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgReceiptList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        CallFindNow(SearchIndex)

        dgReceiptList.DataBind()

        'cmbSearchCriteria.SelectedIndex = SearchIndex
        cmbPeriod.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId

        If cmbOrderText.Items.Contains(New System.Web.UI.WebControls.ListItem(ReceiptOrderText)) Then
            cmbOrderText.SelectedValue = ReceiptOrderText
        Else
            cmbOrderText.SelectedValue = "(All)"
        End If
        If cmbReceiptText.Items.Contains(New System.Web.UI.WebControls.ListItem(ReceiptText)) Then
            cmbReceiptText.SelectedValue = ReceiptText
        Else
            cmbReceiptText.SelectedValue = "(All)"
        End If
        txtOrderNo.Text = ReceiptOrderNo
        txtReceiptNo.Text = ReceiptNo
        cmbSearchReceiptType.SelectedIndex = ReceiptTypeID

        txtInternalReceiptNoSearch.Text = ReceiptInternalReceiptNoSearch
        txtDCNoSearch.Text = ReceiptDCNoSearch
        txtPartNoSearch.Text = ReceiptPartNoSearch
        txtDescriptionSearch.Text = ReceiptDescriptionSearch
        txtReleaseNoteNoSearch.Text = ReceiptReleaseNoteNoSearch
        txtCustomBillofEntrySearch.Text = ReceiptCustomBillofEntrySearch
        txtSerialNoSearch.Text = ReceiptSerialNoSearch
        txtBatchNoSearch.Text = ReceiptBatchNoSearch
        txtSupplierSearch.Text = ReceiptSupplier
        ControlVisibility(SearchIndex, DateIndex, Val(ReceiptNo), Val(ReceiptOrderNo))

        'Ajay 11-Jan-2023
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub ClearControl()
        txtOrderNo.Text = ""
        txtReceiptNo.Text = ""
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
                            mReceipt = CType(Session("mReceipt"), Receipt)
                            mVendorName = mReceiptList(mReceipt.ID).VendorName
                            If mReceipt.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachmentChild(mReceipt.ID)
                            End If
                            'Added By Vikrant On 24-July-2014 For BA24072014
                            If AppSettings("LockBackDatedTransaction") = "True" Then
                                If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                                    'Do nothing
                                Else
                                    Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
                                    Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
                                    If (CDate(mReceipt.RecdDate) >= FirstDayofLastMonth) Then
                                        If (CDate(mReceipt.RecdDate) < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                                            msgCount = 1
                                            MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mReceipt.RecdDate).AddMonths(1)), Month(CDate(mReceipt.RecdDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                            Exit Sub
                                        End If
                                    Else
                                        msgCount = 1
                                        MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be deleted until " & DateSerial(Year(CDate(mReceipt.RecdDate).AddMonths(1)), Month(CDate(mReceipt.RecdDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                        Exit Sub
                                    End If
                                End If
                            End If
                            'End
                            mReceipt.Delete()
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mReceipt.Save()
                            DataFieldBind()
                            SetControl()
                            UpdateItemGridView()
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Message.Contains("tabInvoiceItem") Then
                                stringInfo = "Invoice."
                            ElseIf ex.Message.Contains("tabIssueItem") Then
                                stringInfo = "Issue."
                            ElseIf ex.Message.Contains("tabOrderItem") Then
                                stringInfo = "Order."
                            ElseIf ex.Message.Contains("tabConditionCheckItem") Then
                                stringInfo = "Condition Check."
                            ElseIf ex.Message.Contains("tabCalibrationItem") Then
                                stringInfo = "Calibration."
                            ElseIf ex.Message.Contains("tabOtherChargeInvoices") Then
                                stringInfo = "Docket Charge."
                            ElseIf ex.Message.Contains("tabComponentReservation") Then
                                stringInfo = "Component Reservation."
                            Else
                                stringInfo = ""
                            End If
                            If ex.Number = 547 Then
                                mModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mReceipt.TransTypeID).ToString
                                Session("mModuleName") = mModuleName
                                mReceiptDetails = mReceipt.ReceiptNo + " Dated : " + mReceipt.RecdDateFormatted + " from " + mVendorName
                                MarkLog(Util.Action.Delete, mModuleName, "Can't delete : " & mReceiptDetails & " is Currently in use", Util.ErrorType.HandledError, mReceipt.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceipt.TransTypeID).ToString)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            SetTitle()
                            upnlResult.Update()
                            If msgCount = 0 Then
                                mModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mReceipt.TransTypeID).ToString
                                Session("mModuleName") = mModuleName
                                mReceiptDetails = mReceipt.ReceiptNo + " Dated : " + mReceipt.RecdDateFormatted + " from " + mVendorName
                                MarkLog(Util.Action.Delete, mModuleName, mReceiptDetails, Util.ErrorType.NoError, mReceipt.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceipt.TransTypeID).ToString)
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
    Private Sub FindNow(Optional ByVal Fromdate As String = "1/1/1900", _
    Optional ByVal ToDate As String = "1/1/2200", Optional ByVal Text As String = "", _
    Optional ByVal No As Integer = 0, Optional ByVal IntReceiptNo As String = "", _
    Optional ByVal VendorName As String = "", Optional ByVal AircraftName As String = "", _
    Optional ByVal DCNo As String = "", Optional ByVal StatusID As Integer = 0, _
    Optional ByVal ItemName As String = "", Optional ByVal ReceiptOrderNo As Integer = 0, _
    Optional ByVal ReceiptOrderText As String = "", Optional ByVal IssueNo As Integer = 0, _
    Optional ByVal IssueText As String = "", Optional ByVal ReleaseNoteNo As String = "", _
    Optional ByVal Type As Integer = 1, Optional ByVal tmpTransTypeID As Util.Trans = Util.Trans.None, Optional ByVal AWBNo As String = "", Optional ByVal SerialNo As String = "", Optional ByVal Description As String = "", Optional ByVal IsForPrint As Boolean = False, Optional ByVal BatchNo As String = "", Optional ByVal SearchText As String = "") 'Ajay SearchText 18-Jan-2023)
        'clear the obj and grid
        mReceiptList = Nothing
        dgReceiptList.DataSource = Nothing
        'get the list
        'Changes by Kalpesh Shah as on 23-01-2008
        'mReceiptList = mReceiptList.GetRecepitList(Fromdate, ToDate, Text, No, IntReceiptNo, VendorName, AircraftName, DCNo, StatusID, ItemName, ReceiptOrderNo, ReceiptOrderText, IssueNo, IssueText, ReleaseNoteNo, Type)
        If IsForPrint = True Then
            mReceiptList = ReceiptList.GetRecepitList(Fromdate, ToDate, Text, No, IntReceiptNo, VendorName, AircraftName, DCNo, StatusID, ItemName, ReceiptOrderNo, ReceiptOrderText, IssueNo, IssueText, ReleaseNoteNo, Type, tmpTransTypeID, AWBNo, False, CurrentPage:=mpageindex, PageSize:=mpageSize, SerialNo:=SerialNo, Description:=Description, BatchNo:=BatchNo, SearchText:=SearchText)
            Exit Sub
        Else
            mReceiptList = ReceiptList.GetRecepitList(Fromdate, ToDate, Text, No, IntReceiptNo, VendorName, AircraftName, DCNo, StatusID, ItemName, ReceiptOrderNo, ReceiptOrderText, IssueNo, IssueText, ReleaseNoteNo, Type, tmpTransTypeID, AWBNo, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize, SerialNo:=SerialNo, Description:=Description, BatchNo:=BatchNo, SearchText:=SearchText)
        End If

        'bind the list to the datagrid
        'set the session
        totalCount = mReceiptList.TotalRecords
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount
        dgReceiptList.DataSource = mReceiptList
        dgReceiptList.DataBind()
        Session("mReceiptList") = mReceiptList
        UpdateItemGridView()
        dgReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 11-Jan-2022
    End Sub
    Private Sub CallFindNow(ByVal indx As Int32, Optional ByVal IsForPrint As Boolean = False)
        FindNow(Fromdate:=txtFromDate.Text, ToDate:=txtToDate.Text, Text:=Trim(ReceiptText), No:=CInt(Val(ReceiptNo)), _
                IntReceiptNo:=Trim(ReceiptInternalReceiptNoSearch), VendorName:=Trim(ReceiptSupplier), _
                AircraftName:="", DCNo:=ReceiptDCNoSearch, StatusID:=CInt(StatusId), ItemName:=Trim(ReceiptPartNoSearch), _
                ReceiptOrderNo:=CInt(Val(ReceiptOrderNo)), ReceiptOrderText:=Trim(ReceiptOrderText), IssueNo:=0, IssueText:="", _
                ReleaseNoteNo:=Trim(ReceiptReleaseNoteNoSearch), Type:=1, tmpTransTypeID:=CType(ReceiptTypeID.ToString, Util.Trans), _
                AWBNo:=Trim(ReceiptCustomBillofEntrySearch), SerialNo:=Trim(ReceiptSerialNoSearch), Description:=Trim(ReceiptDescriptionSearch), _
                IsForPrint:=IsForPrint, BatchNo:=Trim(ReceiptBatchNoSearch), SearchText:=SearchText)
        'Select Case indx
        '    Case 0  'All
        '        FindNow(, , "", 0, "", "", "", "", 0, "", 0, "", 0, "", "", 1, , IsForPrint:=IsForPrint)
        '    Case 1  'Date
        '        FindNow(txtFromDate.Text, txtToDate.Text, "", 0, "", "", "", "", 0, "", 0, "", 0, "", "", 1, IsForPrint:=IsForPrint)
        '    Case 2  'Receipt No & Text.
        '        FindNow(FromDate, ToDate, ReceiptText, CInt(Val(ReceiptNo)), "", "", "", "", 0, "", 0, "", 0, "", "", 1, , IsForPrint:=IsForPrint)
        '    Case 3  'Internal receipt No.
        '        FindNow(FromDate, ToDate, "", 0, Name, "", "", "", 0, "", 0, "", 0, "", "", 1, , , IsForPrint:=IsForPrint)
        '    Case 4  'Part No.
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", 0, Name, 0, "", 0, "", "", 1, , , IsForPrint:=IsForPrint)
        '    Case 5  'Vendor
        '        FindNow(FromDate, ToDate, "", 0, "", Name, "", "", 0, "", 0, "", 0, "", "", 1, , , IsForPrint:=IsForPrint)
        '    Case 6  'Order No & Text.
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", 0, "", Val(ReceiptOrderNo), ReceiptOrderText, 0, "", "", 1, , , IsForPrint:=IsForPrint)
        '    Case 7  'D.C.No.
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", Name, 0, "", 0, "", 0, "", "", 1, , , IsForPrint:=IsForPrint)
        '    Case 8  'ReleaseNoteNo.
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", 0, "", 0, "", 0, "", Name, 1, , , IsForPrint:=IsForPrint)
        '        'Changes by Kalpesh Shah as on 23-01-2008
        '        'Status' was 9 which is shifted to 10. Now 9 is 'Receipt Type' 
        '    Case 9  'Receipt Type
        'FindNow(, , "", 0, "", "", "", "", 0, "", 0, "", 0, "", "", 1, CType(cmbSearchReceiptType.SelectedValue, Util.Trans), IsForPrint:=IsForPrint)
        '        'Changes by Kalpesh Shah as on 23-01-2009
        '        'Status' was 10 which is shifted to 11. Now 10 is 'AWBNo' 
        '    Case 10  'AWBNo
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", 0, "", 0, "", 0, "", "", 1, , Name, IsForPrint:=IsForPrint)
        '    Case 11  'Status
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", CInt(StatusId), "", 0, "", 0, "", "", 1, , IsForPrint:=IsForPrint)
        '    Case 12  'SerialNo
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", CInt(StatusId), "", 0, "", 0, "", "", 1, , "", Name, IsForPrint:=IsForPrint)
        '    Case 13  'Description
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", CInt(StatusId), "", 0, "", 0, "", "", 1, , "", "", Name, IsForPrint:=IsForPrint)
        '    Case 14  'Batch No.
        '        FindNow(FromDate, ToDate, "", 0, "", "", "", "", CInt(StatusId), "", 0, "", 0, "", "", 1, , "", "", Description:="", IsForPrint:=IsForPrint, BatchNo:=Name)
        'End Select
        dgReceiptList.PageIndex = 0   'Added Code on MAy,2007  
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal PeriodIndex As Int32 = 0, Optional ByVal RectTxt As Int32 = 0, Optional ByVal Ordtxt As Int32 = 0)
        cmbPeriod.Visible = CBool(IIf(SearchIndex = 1, True, False))
        lbl.Visible = CBool(IIf(SearchIndex = 1 And PeriodIndex <> 0, True, False)) 'From Date
        'lbl.Visible = CBool(IIf(SearchIndex = 1 And PeriodIndex <> 0, True, False))
        'lblToDate.Visible = CBool(IIf(SearchIndex = 1 And PeriodIndex <> 0, True, False)) 'To Date
        lblTo.Visible = CBool(IIf(PeriodIndex <> 0, True, False))
        'cmbOrderText.Visible = CBool(IIf(SearchIndex = 6, True, False))
        'cmbReceiptText.Visible = CBool(IIf(SearchIndex = 2, True, False))

        'Added by Saylee on 16-June 2007**************
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
        '************************************************
        'lblNo.Visible = IIf(((SearchIndex = 2 And cmbReceiptText.SelectedIndex > 0) Or (SearchIndex = 6 And cmbOrderText.SelectedIndex > 0)), True, False)
        'txtReceiptNo.Visible = IIf((SearchIndex = 2 And cmbReceiptText.SelectedIndex > 0), True, False)
        'txtOrderNo.Visible = IIf((SearchIndex = 6 And cmbOrderText.SelectedIndex > 0), True, False)
        'txtSearchFor.Visible = CBool(IIf((SearchIndex >= 3 And SearchIndex <= 5) Or _
        ' (SearchIndex >= 7 And SearchIndex <= 8) Or (SearchIndex = 10) Or (SearchIndex = 12) Or (SearchIndex = 13) Or (SearchIndex = 14), True, False))
        'Me.cmbSearchReceiptType.Visible = CBool(IIf(SearchIndex = 9, True, False))
        'cmbStatus.Visible = CBool(IIf(SearchIndex = 11, True, False))
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
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearchCriteria.SelectedIndex < 0, 0, cmbSearchCriteria.SelectedIndex)
        DateIndex = IIf(cmbPeriod.SelectedIndex < 0, 0, cmbPeriod.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        ReceiptOrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue)
        ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)

        ReceiptOrderNo = txtOrderNo.Text.Trim
        ReceiptNo = txtReceiptNo.Text.Trim

        ReceiptTypeID = IIf(cmbSearchReceiptType.SelectedIndex <= 0, 0, cmbSearchReceiptType.SelectedValue)
        ReceiptInternalReceiptNoSearch = txtInternalReceiptNoSearch.Text
        ReceiptDCNoSearch = txtDCNoSearch.Text
        ReceiptPartNoSearch = txtPartNoSearch.Text
        ReceiptDescriptionSearch = txtDescriptionSearch.Text
        ReceiptReleaseNoteNoSearch = txtReleaseNoteNoSearch.Text
        ReceiptCustomBillofEntrySearch = txtCustomBillofEntrySearch.Text
        ReceiptSerialNoSearch = txtSerialNoSearch.Text
        ReceiptBatchNoSearch = txtBatchNoSearch.Text
        ReceiptSupplier = txtSupplierSearch.Text
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text) 'Ajay 18-01-2023

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("ReceiptOrderText") = ReceiptOrderText
        Session("ReceiptText") = ReceiptText
        Session("ReceiptOrderNo") = ReceiptOrderNo
        Session("ReceiptNo") = ReceiptNo
        Session("Name") = Name

        Session("ReceiptInternalReceiptNoSearch") = ReceiptInternalReceiptNoSearch
        Session("ReceiptDCNoSearch") = ReceiptDCNoSearch
        Session("ReceiptPartNoSearch") = ReceiptPartNoSearch
        Session("ReceiptDescriptionSearch") = ReceiptDescriptionSearch
        Session("ReceiptReleaseNoteNoSearch") = ReceiptReleaseNoteNoSearch
        Session("ReceiptCustomBillofEntrySearch") = ReceiptCustomBillofEntrySearch
        Session("ReceiptSerialNoSearch") = ReceiptSerialNoSearch
        Session("ReceiptBatchNoSearch") = ReceiptBatchNoSearch
        Session("ReceiptSupplier") = ReceiptSupplier
        Session("SearchText") = SearchText 'Ajay 18-01-2023
    End Sub
    Private Sub addAttributes()
        txtReceiptNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReceiptNo').value,event)")
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOrderNo').value,event)")
    End Sub
    Private Sub SetGrid()
        'btnBottomPrint.Enabled = IIf(dgReceiptList.Rows.Count = 0, False, True) ''Ajay 30-01-2023
        btnPrintTop.Enabled = IIf(dgReceiptList.Rows.Count = 0, False, True)
        'ajay 07-02-2023
        Dim P As Boolean
        'For j As Integer = 0 To dgReceiptList.Rows.Count - 1
        '    P = CType(Me.dgReceiptList.Rows.Item(j).Cells(14).Text, Boolean)  
        '    If P = False Then
        '        dgReceiptList.Rows.Item(j).Cells(13).Enabled = False  
        '    End If
        ''Ajay 10-02-2023
        For j As Integer = 0 To dgReceiptList.Rows.Count - 1
            P = CType(Me.dgReceiptList.Rows.Item(j).Cells(12).Text, Boolean)
            If P = False Then
                dgReceiptList.Rows.Item(j).Cells(11).Enabled = True
            End If
        Next
    End Sub
    Private Sub SetTitle() 'Added By Utkarsh On 21-Jul-2011 For All19072011
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        mModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeId).ToString
        Session("mModuleName") = mModuleName
        'lblList.Text = "List of Receipts" + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"  'Added by shweta on 23-12-11
        lblList.Text = "List of Receipts"   'Added by Ajay on 08-02-2023
        upnlTitle.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        ReceiptOrderText = Session("ReceiptOrderText")
        ReceiptText = Session("ReceiptText")
        Name = Session("Name")
        ReceiptTypeID = Session("ReceiptTypeID") 'Changes by Kalpesh Shah as on 23-01-2008

        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("2", , True, "(All)")
        cmbReceiptText.DataSource = mDistinctTextListForReceipt
        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder

        mTransactionListCount = TransactionListCount.GetTransactionListCountt(6)
        Session("mTransactionListCount") = mTransactionListCount 'End

        'mSearchReceiptTypeList = ReceiptTypeList.GetSimpleReciptTypeList()
        'cmbSearchReceiptType.DataSource = mSearchReceiptTypeList

        mReceiptTypeList = ReceiptTypeList.GetSimpleReciptTypeList()
        cmbReceiptType.DataSource = mReceiptTypeList
        DataBind()
    End Sub
    Private Sub GridBind()
        dgReceiptList.DataSource = mReceiptList
        dgReceiptList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = "As per criteria:" & totalCount & " Record(s) found."

        Else
            'lblResult.Text = "List of Receipts as per criteria:" & currentrow + 1 & " to " & currentrow + mReceiptList.Count & " of " & totalCount & " Record(s) found."
            lblResult.Text = "As per criteria:" & totalCount & "  Record(s) found." ''Ajay 08-02-2023

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
        dgReceiptList.DataBind()
        SetGrid()
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added By Utkarsh On 20-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbPeriod.Enabled = True Then
                setFocus(cmbPeriod)
            End If
            'Added By Utkarsh On 21-Jul-2011 For All19072011
            mTransTypeId = Request.QueryString("TransTypeId")
            Session("mTransTypeId") = mTransTypeId
            'End
            Session("MiddleFrame") = "wfReceiptList_Ajax.aspx?" 'TransTypeId=" & mTransTypeId

            'Ajay 07-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            cmbShowE.SelectedIndex = "4" 'Ajay 18-Jan-2023
            DataFieldBind()
            SetControl()
            SetGrid()
            SetTitle()
        End If
    End Sub
    Private Sub btnAddNewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click ', btnBottomAddNew.Click ''Ajay 30-01-2023
        'Ajay 07-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
        NewRecord()
        If (Not User.IsInRole("ReceiptPONew")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'Changed By Utkarsh On 20-Jul-2011 For All19072011
        SetTitle()
        MarkLog(Util.Action.[New], TransactionList.GetTransactionList().GetTransactionTypeName(mReceipt.TransTypeID).ToString, "", Util.ErrorType.NoError, mReceipt.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceipt.TransTypeID).ToString)
        'End
        mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceipt.ID)
        Session("mFileAttach") = mFileAttach
        Dim str As String
        If CType(mTransTypeId, Trans) = Util.Trans.RCIFromSupplierAsNone Then 'Added by Prashant 5-Dec-2018 ALL05122018 
            str = "openledgersame('wfReceipt_Ajax.aspx?BackPage=wfReceiptList_Ajax.aspx');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        Else
            Dim mPrevTransID As Guid = Guid.Empty
            Dim mPrimaryOrderType As Integer
            Dim mTransaction As Integer
            Dim mFromPartList As Boolean

            If (mReceipt.ReceiptItems.Count = 0) Or (mReceipt.ReceiptItems.Count = 1 And mReceipt.ReceiptItems.CurrentItem.IsNew) Then
                mPrevTransID = Guid.Empty
            Else
                mPrevTransID = mReceipt.ReceiptItems.Item(mReceipt.ReceiptItems.Count - 2).OrderItemDetailForReceipt.OrderID
            End If

            If CType(mTransTypeId, Trans) = Util.Trans.ReceiptAgainstPuchaseOrder Then 'mPrimaryOrderType = 3 'TransListOf.Order_Outright'Changes by Kalpesh Shah
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
            str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType=1');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End If
    End Sub
    Private Sub dgReceiptList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                'Ajay 07-Nov-2022
                If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO") Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
                End If
                '--------------------------
                Dim index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgReceiptList.PageIndex * dgReceiptList.PageSize
                Dim mID As Guid = mReceiptList(index).ID
                If (Not User.IsInRole("ReceiptPOView") And Not User.IsInRole("ReceiptPOEdit")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                EditRecord(mID)
                'Changed By Utkarsh On 20-Jul-2011 For All19072011
                mTransTypeId = mReceipt.TransTypeID
                If mReceipt.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachmentChild(mReceipt.ID)
                Else
                    mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceipt.ID)
                End If
                Session("mFileAttach") = mFileAttach
                SetTitle()
                mReceiptDetails = mReceipt.ReceiptNo + " Dated : " + mReceipt.RecdDateFormatted + " from " + mReceiptList(mReceipt.ID).VendorName
                MarkLog(Util.Action.Edit, TransactionList.GetTransactionList().GetTransactionTypeName(mReceipt.TransTypeID).ToString, mReceiptDetails, Util.ErrorType.NoError, mReceipt.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceipt.TransTypeID).ToString)
                'End
                Dim str As String
                str = "openledgersame('wfReceipt_Ajax.aspx?BackPage=wfReceiptList_Ajax.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                Dim index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgReceiptList.PageIndex * dgReceiptList.PageSize
                Dim mID As Guid = mReceiptList(index).ID
                If (Not User.IsInRole("ReceiptPODelete")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
            Case "ViewRec" '=====================Added By Saylee on 8th Aug 2007=================
                If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
                    GridBind()
                    SetGrid()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgReceiptList.PageIndex * dgReceiptList.PageSize
                Dim mID As Guid = mReceiptList(index).ID
                GridBind()
                mReceipt = Receipt.GetReceipt(mID)
                GetAttachment(mReceipt.ID, mReceipt.IsAttachmentAdded)
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If
        End Select
        SetGrid()
    End Sub
    Private Sub dgReceiptList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReceiptList.PageIndexChanging
        'dgReceiptList.PageIndex = e.NewPageIndex
        'mCurrentpage = e.NewPageIndex
        'GridBind()
        'UpdateItemGridView()
        'Session("mReceiptList") = mReceiptList
        'dgReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 18-Jan-2023
        '' Ajay 07-02-2023
        dgReceiptList.PageIndex = e.NewPageIndex
        dgReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgReceiptList.DataSource = mReceiptList
        Session("mReceiptList") = mReceiptList
        dgReceiptList.DataBind()
        SetGrid()

        '-----------------
        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgReceiptList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = e.NewPageIndex
        pagecount = CInt(Session("pagecount"))


    End Sub

    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        'Ajay 07-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
        setVariables()
        dgReceiptList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex)
        dgReceiptList.DataBind()

        SetGrid()
        upnlGridView.Update()
        upnlTitle.Update()
        upnlTitleNew.Update()
        upnTopButtons.Update()
        upnBottomButtons.Update()
        upnlResult.Update()

    End Sub
    'Private Sub cmbSearchCriteria_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchCriteria.SelectedIndexChanged
    '    cmbPeriod.SelectedIndex = 0
    '    cmbReceiptText.SelectedIndex = 0
    '    cmbOrderText.SelectedIndex = 0
    '    cmbReceiptType.SelectedIndex = 0
    '    cmbStatus.SelectedIndex = 0

    '    ClearControl()
    '    Dim DateIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0 And cmbPeriod.Visible, cmbPeriod.SelectedIndex, 0)
    '    ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)
    '    SetPeriod(DateIndex)
    '    '' ControlVisibility(cmbSearchCriteria.SelectedIndex, 0, 0, 0)
    '    If cmbSearchCriteria.Enabled = True Then
    '        setFocus(cmbSearchCriteria)
    '    End If
    'End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged
         Dim PeriodIndex As Int32 = CInt(IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0))
        ControlVisibility(1, PeriodIndex, 0, 0)
        SetPeriod(PeriodIndex)
        If cmbPeriod.Enabled = True Then
            setFocus(cmbPeriod)
        End If
    End Sub
    Private Sub cmbReceiptText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptText.SelectedIndexChanged, cmbOrderText.SelectedIndexChanged, cmbReceiptType.SelectedIndexChanged, cmbSearchReceiptType.SelectedIndexChanged
        If sender.ID = "cmbReceiptText" Then
            txtReceiptNo.Text = "0"
            If cmbReceiptText.Enabled = True Then
                setFocus(cmbReceiptText)
            End If
        ElseIf sender.ID = "cmbOrderText" Then
            txtOrderNo.Text = "0"
            If cmbOrderText.Enabled = True Then
                setFocus(cmbOrderText)
            End If
        ElseIf sender.ID = "cmbReceiptType" Then
            mTransTypeId = CType(cmbReceiptType.SelectedValue, Util.Trans)
            Session("mTransTypeId") = mTransTypeId
        ElseIf sender.ID = "cmbSearchReceiptType" Then
            ReceiptTypeID = cmbSearchReceiptType.SelectedIndex
            Session("ReceiptTypeID") = ReceiptTypeID
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click '', btnBottomClose.Click ''Ajay 30-01-2023
        Session("MiddleFrame") = ""
        Session("mCount") = Nothing
        mReceipt = Nothing
        mDistinctTextListForOrder = Nothing
        mDistinctTextListForReceipt = Nothing
        mReceiptList = Nothing
        RemoveSessions()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgReceiptList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgReceiptList.Sorting
        mReceiptList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mReceiptList") = mReceiptList
        GridBind()
        UpdateItemGridView()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub


    Private Sub btnGridPaging_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        dgReceiptList.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(1)
        upnlResult.Update()
    End Sub
 
    'Ajay 07-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 07-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 07-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO")
    End Sub
    '-----
#End Region
    'Ajay 18-Jan-2023 // 07-02-2023
    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        ''Dim ExpiryDateList = ((From res In mWOList).ToList.Take(CInt(DropDownList1.SelectedItem.ToString))).ToList
        'dgReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        'dgReceiptList.DataSource = mReceiptList
        'dgReceiptList.DataBind()
        'SetControl()
        'upnlGridView.Update()
        'upnlTitleNew.Update()    
        'upnlResult.Update()
        setVariables()
        dgReceiptList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgReceiptList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        pagecount = CInt(Session("pagecount"))
        SetControl()
        upnlGridView.Update()
        upnlTitleNew.Update()
        upnlResult.Update()

    End Sub

    'Ajay 11-Jan-2023
    Private Sub txtSearchBox_TextChanged(sender As Object, e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        dgReceiptList.DataBind()

        SetControl()
       
        upnlGridView.Update()
        upnlTitleNew.Update()
        upnlResult.Update()
    End Sub
    '-----
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
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
        'For Receipt List
        Dim Rpt As New crReceiptList
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
        '    'Receipt 
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbReceiptText.SelectedItem.Text + "-" + txtReceiptNo.Text
        'ElseIf (cmbSearchCriteria.SelectedIndex = 3 Or cmbSearchCriteria.SelectedIndex = 4 Or cmbSearchCriteria.SelectedIndex = 5 Or cmbSearchCriteria.SelectedIndex = 7 Or cmbSearchCriteria.SelectedIndex = 8 Or cmbSearchCriteria.SelectedIndex = 10 Or cmbSearchCriteria.SelectedIndex = 12 Or cmbSearchCriteria.SelectedIndex = 13 Or cmbSearchCriteria.SelectedIndex = 14) Then
        '    'Internal Receipt No., Part Number, Vendor, DC No., Release Note No.,  AWBNo, Serial No., Description
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtSearchFor.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 6 Then
        '    'Order 
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbOrderText.SelectedItem.Text + "-" + txtOrderNo.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 9 Then
        '    'Receip Type
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbSearchReceiptType.SelectedItem.Text
        'ElseIf cmbSearchCriteria.SelectedIndex = 11 Then
        '    'Status
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        'End If

        ReportDetails.Add(New rptStatus(, 0, , _
              dgReceiptList.Columns.Item(1).HeaderText, dgReceiptList.Columns.Item(2).HeaderText, dgReceiptList.Columns.Item(3).HeaderText, _
              dgReceiptList.Columns.Item(4).HeaderText, dgReceiptList.Columns.Item(5).HeaderText, dgReceiptList.Columns.Item(6).HeaderText, _
              dgReceiptList.Columns.Item(7).HeaderText, dgReceiptList.Columns.Item(8).HeaderText, dgReceiptList.Columns.Item(9).HeaderText))
        Dim I As Integer
        For I = 0 To mReceiptList.Count - 1
            ReportDetails.Add(New rptStatus(, 1, , mReceiptList(I).RecdDateFormatted.ToString, _
                mReceiptList(I).ReceiptNo, mReceiptList(I).ReceiptType.ToString, mReceiptList(I).IntReceiptNo, mReceiptList(I).VendorName.ToString, IIf(mReceiptList(I).DCNO = "", "", mReceiptList(I).DCNO), mReceiptList(I).DCDateFormatted.ToString, mReceiptList(I).Status.ToString, mReceiptList(I).UserName.ToString))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Receipt List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mReceiptList.Count = 0 Then
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