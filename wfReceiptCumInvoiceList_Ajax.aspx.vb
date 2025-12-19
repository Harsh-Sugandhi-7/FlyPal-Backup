Imports System.Linq
Imports System.Collections.Generic
Imports System.Text


Public Class wfReceiptCumInvoiceList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        Authorized = 7 'Added By Prashant 17-Aug-2011
    End Enum
#End Region

#Region " Variable Declaration "

	Public mReceiptCumInvoiceList, mtempReceiptCumInvoiceList As ReceiptCumInvoiceList
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mDistinctTextListForIssue As DistinctTextListForIssue
    Public mDistinctTextListforInvoice As DistinctTextListForInvoice
    Dim mDistinctWOText As nDistinctWOText
    Dim objSearch As rptSearchingCriteriaForReceipt
	Dim objReg As rptReceiptCumInvReg
	Public RedirectToNewUIHelper As New RedirectToNewUIHelper
	Public AttachmentHelper As New AttachmentHelper

	Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, OrderText, ReceiptText, IssueText, _
        InvoiceText, Name, OrderNo, ReceiptNo, IssueNo, InvoiceNo, WOText, WoNo, ReceivedFromType, _
        InternalReceiptNoSearch, DCNoSearch, PartNoSearch, ReleaseNoteNoSearch, CustomBillofEntrySearch, _
        SerialNoSearch, BatchNoSearch, GSENoSearch, SearchText As String 'Ajay
    Public mTransTypeID As Trans
    Public mModuleName As String
    Public Tital As String
    Public mDocumentTypeForID As Integer
    Public mAttachToID As Guid
    Public mName As String
    Public mReceiptTypeList As ReceiptTypeList
    Dim mReceivedFrom, mReceivedAs As String
    Dim EventLogID As Guid                              'Added By Utkarsh On 20-Jul-2011 For All19072011
    Dim mRCIDetail As String                            'Added By Utkarsh On 20-Jul-2011 For All19072011
    Dim mTransactionListCount As TransactionListCount   'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Private SerialNo As String = String.Empty
    Dim mFileAttach As FileAttach 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
    Dim VendorName As String = String.Empty
    Dim AircraftName As String = String.Empty
    Dim StoreName As String = String.Empty
    Dim CustomerName As String = String.Empty
    Dim WorkShopName As String = String.Empty
	Dim ListofReceiptItems As New StringBuilder

#End Region

#Region " Business Methods "
	Private Sub GetSession()
        mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
        mReceiptCumInvoiceList = CType(Session("mReceiptCumInvoiceList"), ReceiptCumInvoiceList)
        mDistinctTextListForReceipt = CType(Session("mDistinctTextListForReceipt"), DistinctTextListForReceipt)
        mDistinctTextListForOrder = CType(Session("mDistinctTextListForOrder"), DistinctTextListForOrder)
        mDistinctTextListForIssue = CType(Session("mDistinctTextListForIssue"), DistinctTextListForIssue)
        mDistinctTextListforInvoice = CType(Session("mDistinctTextListforInvoice"), DistinctTextListForInvoice)
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        OrderText = Session("OrderText")
        ReceiptText = Session("ReceiptText")
        IssueText = Session("IssueText")
        InvoiceText = Session("InvoiceText")
        'IssueNo = Session("IssueNo")
        'InvoiceNo = Session("InvoiceNo")
        'WoNo = Session("WoNo")
        Name = Session("Name")
        OrderNo = IIf(IsNothing(Session("OrderNo")), 0, Session("OrderNo"))
        ReceiptNo = IIf(IsNothing(Session("ReceiptNo")), 0, Session("ReceiptNo"))
        IssueNo = IIf(IsNothing(Session("IssueNo")), 0, Session("IssueNo"))
        InvoiceNo = IIf(IsNothing(Session("InvoiceNo")), 0, Session("InvoiceNo"))
        WoNo = IIf(IsNothing(Session("WoNo")), 0, Session("WoNo"))
        mTransTypeID = Session("mTransTypeId")
        mModuleName = Session("mModuleName")
        mReceivedFrom = Session("mReceivedFrom")
        mReceivedAs = Session("mReceivedAs")
        mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
        WOText = Session("WOText")
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        SerialNo = Session("SerialNo")
        mFileAttach = Session("mFileAttach") 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
        ReceivedFromType = Session("ReceivedFromType")
        VendorName = Session("VendorName")
        AircraftName = Session("AircraftName")
        StoreName = Session("StoreName")
        CustomerName = Session("CustomerName")
        WorkShopName = Session("WorkShopName")
        InternalReceiptNoSearch = Session("InternalReceiptNoSearch")
        DCNoSearch = Session("DCNoSearch")
        PartNoSearch = Session("PartNoSearch")
        ReleaseNoteNoSearch = Session("ReleaseNoteNoSearch")
        CustomBillofEntrySearch = Session("CustomBillofEntrySearch")
        SerialNoSearch = Session("SerialNoSearch")
        BatchNoSearch = Session("BatchNoSearch")
        GSENoSearch = Session("GSENoSearch")
        SearchText = Session("SearchText") 'Ajay 19-Jan-2023
    End Sub
    Private Sub SetSession()
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        Session("mDistinctTextListForReceipt") = mDistinctTextListForReceipt
        Session("mDistinctTextListForOrder") = mDistinctTextListForOrder
        Session("mDistinctTextListForIssue") = mDistinctTextListForIssue
        Session("mDistinctTextListforInvoice") = mDistinctTextListforInvoice
        Session("mModuleName") = mModuleName
        Session("mReceivedFrom") = mReceivedFrom
        Session("mReceivedAs") = mReceivedAs
        Session("SerialNo") = SerialNo
        Session("mFileAttach") = mFileAttach 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
        SearchText = Session("SearchText") 'Ajay 19-Jan-2023
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mReceiptCumInvoice")
        Session.Remove("mReceiptCumInvoiceList")
        Session.Remove("mDistinctTextListForReceipt")
        Session.Remove("mDistinctTextListForOrder")
        Session.Remove("mDistinctTextListForIssue")
        Session.Remove("mDistinctTextListforInvoice")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("OrderText")
        Session.Remove("ReceiptText")
        Session.Remove("IssueText")
        Session.Remove("InvoiceText")
        Session.Remove("Name")
        Session.Remove("OrderNo")
        Session.Remove("ReceiptNo")
        Session.Remove("IssueNo")
        Session.Remove("InvoiceNo")
        Session.Remove("mTransTypeId")
        Session.Remove("mReceivedFrom")
        Session.Remove("mReceivedAs")
        Session.Remove("mTransactionListCount") 'Added By Vikrant On 20-Aug-2013 For ALL16082013-1
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        Session.Remove("SerialNo")
        Session.Remove("mFileAttach") 'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
        Session.Remove("WoNo")
        Session.Remove("WOText")
        Session.Remove("ReceivedFromType")
        Session.Remove("VendorName")
        Session.Remove("AircraftName")
        Session.Remove("StoreName")
        Session.Remove("CustomerName")
        Session.Remove("WorkShopName")
        Session.Remove("InternalReceiptNoSearch")
        Session.Remove("DCNoSearch")
        Session.Remove("PartNoSearch")
        Session.Remove("ReleaseNoteNoSearch")
        Session.Remove("CustomBillofEntrySearch")
        Session.Remove("SerialNoSearch")
        Session.Remove("BatchNoSearch")
        Session.Remove("GSENoSearch")
        Session.Remove("SearchText") 'Ajay 19-Jan-2023
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfReceiptCumInvoiceList_Ajax.aspx?") <= 0 Then
            RemoveSessions()
        End If
    End Sub
    Private Sub ClearTextBoxs()
        txtOrderNo.Text = ""
        txtReceiptNo.Text = ""
        txtIssueNo.Text = ""
        txtInvoiceNo.Text = ""
        txtWONo.Text = ""
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        mReceiptTypeList = Session("mReceiptTypeList")
        mTransTypeID = mReceiptTypeList.Item(cmbReceivedAs.SelectedIndex).ID
        mReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(mTransTypeID)
        If mTransTypeID = 7 Or mTransTypeID = 8 Or mTransTypeID = 10 Or mTransTypeID = 11 Or mTransTypeID = 12 Or mTransTypeID = 13 Or mTransTypeID = 27 _
            Or mTransTypeID = 28 Or mTransTypeID = 47 Or mTransTypeID = 54 Or mTransTypeID = 61 Or mTransTypeID = 62 Or mTransTypeID = 66 Or mTransTypeID = 73 Then  'ALL12102012-1   '73 Added By Prashant 10-Sep-2014 'ALL10092014
            mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
            mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConversionFactor = mReceiptCumInvoice.ConversionFactor
        End If
        mReceivedFrom = cmbReceivedFrom.SelectedIndex
        mReceivedAs = cmbReceivedAs.SelectedIndex
        Session("mTransTypeID") = mTransTypeID
        Session("mReceivedFrom") = mReceivedFrom
        Session("mReceivedAs") = mReceivedAs
        Session("mReceipt") = mReceiptCumInvoice.Receipt
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Sub EditRecord(ByVal mReceiptID As Guid, ByVal mInvoiceID As Guid)
        mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptID, mInvoiceID)
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        mReceivedFrom = cmbReceivedFrom.SelectedIndex
        mReceivedAs = cmbReceivedAs.SelectedIndex
        Session("mReceivedFrom") = mReceivedFrom
        Session("mReceivedAs") = mReceivedAs
    End Sub
    Private Sub DeleteRecord(ByVal mReceiptID As Guid, ByVal mInvoiceID As Guid)
        GridBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptID, mInvoiceID)
        Session("mReceiptID") = mReceiptID
        Session("mInvoiceID") = mInvoiceID
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            'mFileAttach = FileAttach.GetAttachment(ID)
            mFileAttach = FileAttach.GetAttachmentChild(ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded) 'Sort = 2 - Removal
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
    End Sub
    'End
    Private Sub SetControl()
        SetPeriod(DateIndex)

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgReceiptCumInvoiceList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgReceiptCumInvoiceList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        CallFindNow(SearchIndex, , ReceivedFromType)
        dgReceiptCumInvoiceList.DataBind()
        'cmbSearchCriteria.SelectedIndex = SearchIndex
        cmbPeriod.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
        cmbReceivedFromType.SelectedValue = ReceivedFromType

        'Changes made by Kalpesh as per - Aircraft Removed_62)
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
        If cmbIssueText.Items.Contains(New System.Web.UI.WebControls.ListItem(IssueText)) Then
            cmbIssueText.SelectedValue = IssueText
        Else
            cmbIssueText.SelectedValue = "(All)"
        End If
        If cmbInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(InvoiceText)) Then
            cmbInvoiceText.SelectedValue = InvoiceText
        Else
            cmbInvoiceText.SelectedValue = "(All)"
        End If
        If cmbWoText.Items.Contains(New System.Web.UI.WebControls.ListItem(WOText)) Then
            cmbWoText.SelectedValue = WOText
        Else
            cmbWoText.SelectedValue = "(All)"
        End If
        '--------------------------------------------------------------------------

        cmbReceivedFrom.SelectedIndex = mReceivedFrom

        mReceiptTypeList = ReceiptTypeList.GetReciptTypeList(cmbReceivedFrom.SelectedIndex)
        cmbReceivedAs.Enabled = IIf(mReceiptTypeList.Count = 0, False, True)
        btnAddNewTop.Enabled = IIf(mReceiptTypeList.Count = 0, False, True)
        'btnBottomAddNew.Enabled = IIf(mReceiptTypeList.Count = 0, False, True) Ajay

        cmbReceivedAs.DataSource = mReceiptTypeList
        cmbReceivedAs.DataBind()
        Session("mReceiptTypeList") = mReceiptTypeList
        cmbReceivedAs.SelectedIndex = mReceivedAs
        'txtSearchFor.Text = Name
        Select Case ReceivedFromType 'Received From
            Case "0"
                VendorName = ""
                AircraftName = ""
                StoreName = ""
                CustomerName = ""
                WorkShopName = ""
            Case "1"  'Supplier
                txtSearchFor.Text = VendorName
                AircraftName = ""
                StoreName = ""
                CustomerName = ""
                WorkShopName = ""
            Case "2"  'Aircraft
                VendorName = ""
                txtSearchFor.Text = AircraftName
                StoreName = ""
                CustomerName = ""
                WorkShopName = ""
            Case "3"  'Store
                VendorName = ""
                AircraftName = ""
                txtSearchFor.Text = StoreName
                CustomerName = ""
                WorkShopName = ""
            Case "4" 'Customer
                VendorName = ""
                AircraftName = ""
                StoreName = ""
                txtSearchFor.Text = CustomerName
                WorkShopName = ""
            Case "5" 'WorkShop
                VendorName = ""
                AircraftName = ""
                StoreName = ""
                CustomerName = ""
                txtSearchFor.Text = WorkShopName
            Case "6" 'Work Order
        End Select
        txtOrderNo.Text = OrderNo
        txtReceiptNo.Text = ReceiptNo
        txtIssueNo.Text = IssueNo
        txtInvoiceNo.Text = InvoiceNo
        txtWONo.Text = WoNo

        txtInternalReceiptNoSearch.Text = InternalReceiptNoSearch
        txtDCNoSearch.Text = DCNoSearch
        txtPartNoSearch.Text = PartNoSearch
        txtReleaseNoteNoSearch.Text = ReleaseNoteNoSearch
        txtCustomBillofEntrySearch.Text = CustomBillofEntrySearch
        txtSerialNoSearch.Text = SerialNoSearch
        txtBatchNoSearch.Text = BatchNoSearch
        txtGSENoSearch.Text = GSENoSearch

        ControlVisibility(SearchIndex, DateIndex, cmbOrderText.SelectedIndex, cmbReceiptText.SelectedIndex, cmbIssueText.SelectedIndex, cmbInvoiceText.SelectedIndex)
        'If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
        '    lblResult.Text = "List of Goods Receipt as per criteria :" & mReceiptCumInvoiceList.Count & " Record(s) found."
        'Else
        '    lblResult.Text = "List of Receipt-Cum-Invoice as per criteria :" & mReceiptCumInvoiceList.Count & " Record(s) found."
        'End If

        'Ajay 19-Jan-2023
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
                        Dim ReceiptCumInvoiceDetail As String
                        Dim mDetails As String = String.Empty
                        Try
                            Dim mReceiptId As New Guid
                            Dim mInvoiceID As New Guid
                            Session("sender") = ""
                            mReceiptId = Session("mReceiptID")
                            mInvoiceID = Session("mInvoiceID")
                            mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
                            mDetails = mReceiptCumInvoiceList(mReceiptCumInvoice.ID).Name
                            mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptId, mInvoiceID)
                            'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
                            If mReceiptCumInvoice.IsAttachmentAdded Then
                                'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoice.ID) 'Sort = 2: for Removal 
                                mFileAttach = FileAttach.GetAttachmentChild(mReceiptCumInvoice.ID) 'Sort = 2: for Removal 
                            End If
                            'End
                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
                                If (mReceiptCumInvoice.IsSync = 1 Or mReceiptCumInvoice.IsSync = 2) Then
                                    msgCount = 1
                                    MSGBoxCtrl.show("Alert!", " <BR> This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    ReceiptCumInvoice.DeleteReceiptInvoice(mReceiptId, mInvoiceID)
                                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                                    DataFieldBind()
                                    SetControl()
                                    UpdateItemGridView()
                                    GridBind()
                                End If
                            Else
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
                                                MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), Month(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                                Exit Sub
                                            End If
                                        Else
                                            msgCount = 1
                                            MSGBoxCtrl.Show("Delete Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), Month(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                            Exit Sub
                                        End If
                                    End If
                                End If
                                'End
                                ReceiptCumInvoice.DeleteReceiptInvoice(mReceiptId, mInvoiceID)
                                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                                'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
                                If Not mFileAttach Is Nothing Then
                                    If mFileAttach.Size > 0 Then
                                        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                    End If
                                End If
                                'End
                                DataFieldBind()
                                SetControl()
                                UpdateItemGridView()
                            End If
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Message.Contains("tabInvoiceItem") Then
                                stringInfo = "Invoice."
                            ElseIf ex.Message.Contains("tabIssueItem") Then
                                stringInfo = "Issue."
                            ElseIf ex.Message.Contains("tabOrderItem") Then
                                stringInfo = "Order."
                            ElseIf ex.Message.Contains("tabConditionCheckItem") Then
                                stringInfo = "Equipment Maintenance."
                            ElseIf ex.Message.Contains("tabCalibrationItem") Then
                                stringInfo = "Calibration."
                            ElseIf ex.Message.Contains("tabOtherChargeInvoices") Then
                                stringInfo = "Docket Charge."
                            ElseIf ex.Message.Contains("tabComponentReservation") Then
                                stringInfo = "Component Reservation."
                            ElseIf ex.Message.Contains("Can not delete record") Then
                                If User.Identity.Name.ToUpper = "BTPLAdmin".ToUpper Then
                                    stringInfo = ex.Message.Substring(ex.Message.IndexOf("use") + 3)
                                Else
                                    stringInfo = "Issue."
                                End If
                            End If
                            If ex.Number = 547 Then
                                mModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mReceiptCumInvoice.TransTypeID).ToString
                                Session("mModuleName") = mModuleName
                                ReceiptCumInvoiceDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mDetails
                                MarkLog(Util.Action.Delete, mModuleName, "Can't delete : " & ReceiptCumInvoiceDetail & " is Currently in use", Util.ErrorType.HandledError, mReceiptCumInvoice.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            SetTitle()
                            upnlResult.Update()
                            If msgCount = 0 Then
                                mModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mReceiptCumInvoice.TransTypeID).ToString
                                Session("mModuleName") = mModuleName
                                ReceiptCumInvoiceDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mDetails
                                ListofReceiptItems.Append(ReceiptCumInvoiceDetail)
                                If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then
                                    For l As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1
                                        ListofReceiptItems.Append(mReceiptCumInvoice.ReceiptCumInvoiceItems(l).ItemName + " " + mReceiptCumInvoice.ReceiptCumInvoiceItems(l).PartCategory + " Qty:- " + mReceiptCumInvoice.ReceiptCumInvoiceItems(l).Qty.ToString + " Rate:- " + mReceiptCumInvoice.ReceiptCumInvoiceItems(l).EffRate.ToString + ", ")
                                    Next
                                End If
                                'MarkLog(Util.Action.Delete, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString, ReceiptCumInvoiceDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                                MarkLog(Util.Action.Delete, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString, ListofReceiptItems.ToString, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                            End If
                            Session("ForEventLog") = "For Event Log"
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                        SetGrid()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()
                    SetGrid()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Fromdate As String = "1/1/1900", _
    Optional ByVal ToDate As String = "1/1/2200", Optional ByVal Text As String = "", _
    Optional ByVal No As Integer = 0, Optional ByVal IntReceiptNo As String = "", _
    Optional ByVal VendorName As String = "", Optional ByVal AircraftName As String = "", _
    Optional ByVal StoreName As String = "", Optional ByVal DCNo As String = "", _
    Optional ByVal StatusID As Integer = 0, Optional ByVal ItemName As String = "", _
    Optional ByVal OrderText As String = "", Optional ByVal OrderNo As Integer = 0, _
    Optional ByVal IssueText As String = "", Optional ByVal IssueNo As Integer = 0, _
    Optional ByVal ReleaseNoteNo As String = "", Optional ByVal Type As Integer = 0, _
    Optional ByVal InvoiceText As String = "", Optional ByVal InvoiceNo As Integer = 0, _
    Optional ByVal CustomerName As String = "", Optional ByVal AWBNo As String = "", _
    Optional ByVal SerialNo As String = "", Optional ByVal Description As String = "", _
    Optional ByVal IsForPrint As Boolean = False, Optional ByVal ReceivedFromType As Integer = 0, _
    Optional ByVal WorkShopName As String = "", Optional ByVal WOText As String = "", Optional ByVal WONo As Integer = 0, _
    Optional ByVal BatchNo As String = "", Optional ByVal CodeNo As String = "", Optional ByVal SearchText As String = "") 'Ajay SearchText 19-Jan-2023
        'clear the obj and grid
        mReceiptCumInvoiceList = Nothing
        dgReceiptCumInvoiceList.DataSource = Nothing
        'get the list
        If IsForPrint = True Then
            mReceiptCumInvoiceList = ReceiptCumInvoiceList.GetReceiptCumInvoiceList(Fromdate, ToDate, Text, No, _
                              IntReceiptNo, VendorName, AircraftName, StoreName, DCNo, StatusID, _
                              ItemName, OrderText, OrderNo, IssueText, IssueNo, _
                              ReleaseNoteNo, Type, InvoiceText, InvoiceNo, 0, CustomerName, AWBNo, False, CurrentPage:=mpageindex, _
                              PageSize:=mpageSize, SerialNo:=SerialNo, Description:=Description, ReceivedFromType:=ReceivedFromType, _
                              WorkShopName:=WorkShopName, WOText:=WOText, WONo:=WONo, BatchNo:=BatchNo, CodeNo:=CodeNo, SearchText:=SearchText)
            Exit Sub
        Else
            mReceiptCumInvoiceList = ReceiptCumInvoiceList.GetReceiptCumInvoiceList(Fromdate, ToDate, Text, No, _
                              IntReceiptNo, VendorName, AircraftName, StoreName, DCNo, StatusID, _
                              ItemName, OrderText, OrderNo, IssueText, IssueNo, _
                              ReleaseNoteNo, Type, InvoiceText, InvoiceNo, 0, CustomerName, AWBNo, IsCustomPaging:=True, _
                              CurrentPage:=mpageindex, PageSize:=mpageSize, SerialNo:=SerialNo, Description:=Description, _
                              ReceivedFromType:=ReceivedFromType, WorkShopName:=WorkShopName, WOText:=WOText, WONo:=WONo, BatchNo:=BatchNo, CodeNo:=CodeNo, SearchText:=SearchText)
        End If
      

        'bind the list to the datagrid
        totalCount = mReceiptCumInvoiceList.TotalRecords
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount
        dgReceiptCumInvoiceList.DataSource = mReceiptCumInvoiceList
        dgReceiptCumInvoiceList.DataBind()
        Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        UpdateItemGridView()
    End Sub
    Private Sub CallFindNow(ByVal Indx As Int32, Optional ByVal IsForPrint As Boolean = False, Optional ReceivedFromType As String = "0")

        FindNow(Fromdate:=txtFromDate.Text, ToDate:=txtToDate.Text, Text:=Trim(ReceiptText), No:=CInt(Val(ReceiptNo)), IntReceiptNo:=Trim(InternalReceiptNoSearch), _
                VendorName:=Trim(VendorName), AircraftName:=Trim(AircraftName), StoreName:=Trim(StoreName), DCNo:=Trim(DCNoSearch), _
                StatusID:=CInt(StatusId), ItemName:=PartNoSearch, OrderText:=Trim(OrderText), OrderNo:=CInt(Val(OrderNo)), _
                IssueText:=Trim(IssueText), IssueNo:=CInt(Val(IssueNo)), ReleaseNoteNo:=Trim(ReleaseNoteNoSearch), Type:=0, InvoiceText:=Trim(InvoiceText), _
                InvoiceNo:=CInt(Val(InvoiceNo)), CustomerName:=Trim(CustomerName), AWBNo:=Trim(CustomBillofEntrySearch), SerialNo:=Trim(SerialNoSearch), Description:="", _
                IsForPrint:=IsForPrint, ReceivedFromType:=ReceivedFromType, WorkShopName:=Trim(WorkShopName), WOText:=Trim(WOText), WONo:=CInt(Val(WoNo)), _
                BatchNo:=Trim(BatchNoSearch), CodeNo:=Trim(GSENoSearch), SearchText:=SearchText) 'Ajay


        dgReceiptCumInvoiceList.PageIndex = 0 'Added Code  on MAy,25,2007
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal PeriodIndex As Int32 = 0, _
                                    Optional ByVal OrdTxt As Int32 = 0, Optional ByVal RectTxt As Int32 = 0, _
                                    Optional ByVal IssTxt As Int32 = 0, Optional ByVal InvTxt As Int32 = 0)
        'cmbPeriod.Visible = CBool(IIf(SearchIndex = 1, True, False))
        lblFromDate.Visible = CBool(IIf(PeriodIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(PeriodIndex <> 0, True, False))
        'cmbReceiptText.Visible = CBool(IIf(SearchIndex = 2, True, False))
        'cmbOrderText.Visible = CBool(IIf(SearchIndex = 3, True, False))
        'cmbIssueText.Visible = CBool(IIf(SearchIndex = 4, True, False))
        'cmbInvoiceText.Visible = CBool(IIf(SearchIndex = 5, True, False))
        'txtReceiptNo.Visible = (SearchIndex = 2 And RectTxt > 0)
        'txtOrderNo.Visible = (SearchIndex = 3 And OrdTxt > 0)
        'txtIssueNo.Visible = (SearchIndex = 4 And IssTxt > 0)
        'txtInvoiceNo.Visible = (SearchIndex = 5 And InvTxt > 0)
        'cmbWoText.Visible = CBool(IIf(SearchIndex = 14 And cmbReceivedFromType.SelectedIndex = 6, True, False))
        ''If SearchIndex = 14 Then
        ''    lblNo.Visible = CBool(IIf(cmbReceivedFromType.SelectedIndex = 6 And cmbWoText.SelectedIndex > 0, True, False))
        ''Else
        ''    lblNo.Visible = (SearchIndex >= 2 And SearchIndex <= 5) And (OrdTxt > 0 Or RectTxt > 0 Or IssTxt > 0 Or InvTxt > 0)
        ''End If
        'cmbReceivedFromType.Visible = CBool(IIf(SearchIndex = 14, True, False))
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
        'If SearchIndex = 14 Then
        '    txtSearchFor.Visible = CBool(IIf(cmbReceivedFromType.SelectedIndex = 0 Or cmbReceivedFromType.SelectedIndex = 6, False, True))
        '    txtWONo.Visible = CBool(IIf(cmbReceivedFromType.SelectedIndex = 6 And cmbWoText.SelectedIndex > 0, True, False))
        'Else
        '    txtSearchFor.Visible = CBool(IIf(((SearchIndex >= 6 And SearchIndex <= 12) Or SearchIndex = 15 Or SearchIndex = 16), True, False))
        '    txtWONo.Visible = False
        'End If
        'cmbStatus.Visible = CBool(IIf(SearchIndex = 13, True, False))
        txtSearchBox.Visible = True 'Ajay 19-Jan-2023
    End Sub
    Private Sub SetPeriod(ByVal index As Int32)
        ''Last 1 Week
        'If FromDate = "1/1/1900" Then
        '    txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
        'Else
        '    txtFromDate.Text = FromDate
        'End If
        'If ToDate = "1/1/2200" Then
        '    txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        'Else
        '    txtToDate.Text = ToDate
        'End If
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
        OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue)
        ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)
        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedValue)
        InvoiceText = IIf(cmbInvoiceText.SelectedIndex <= 0, "", cmbInvoiceText.SelectedValue)
        WOText = IIf(cmbWoText.SelectedIndex <= 0, "", cmbWoText.SelectedValue)
        ReceivedFromType = IIf(cmbReceivedFromType.SelectedIndex <= 0, "0", cmbReceivedFromType.SelectedValue)
        Name = txtSearchFor.Text.Trim
        OrderNo = txtOrderNo.Text.Trim
        ReceiptNo = txtReceiptNo.Text.Trim
        IssueNo = txtIssueNo.Text.Trim
        InvoiceNo = txtInvoiceNo.Text.Trim
        InternalReceiptNoSearch = txtInternalReceiptNoSearch.Text.Trim
        DCNoSearch = txtDCNoSearch.Text.Trim
        PartNoSearch = txtPartNoSearch.Text.Trim
        ReleaseNoteNoSearch = txtReleaseNoteNoSearch.Text.Trim
        CustomBillofEntrySearch = txtCustomBillofEntrySearch.Text.Trim
        SerialNoSearch = txtSerialNoSearch.Text.Trim
        BatchNoSearch = txtBatchNoSearch.Text.Trim
        GSENoSearch = txtGSENoSearch.Text.Trim
        WoNo = txtWONo.Text.Trim
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text) 'Ajay 19-01-2023
        Select Case ReceivedFromType 'Received From
            Case "0"
                VendorName = ""
                AircraftName = ""
                StoreName = ""
                CustomerName = ""
                WorkShopName = ""
            Case "1"  'Supplier
                VendorName = txtSearchFor.Text.Trim
                AircraftName = ""
                StoreName = ""
                CustomerName = ""
                WorkShopName = ""
            Case "2"  'Aircraft
                VendorName = ""
                AircraftName = txtSearchFor.Text.Trim
                StoreName = ""
                CustomerName = ""
                WorkShopName = ""
            Case "3"  'Store
                VendorName = ""
                AircraftName = ""
                StoreName = txtSearchFor.Text.Trim
                CustomerName = ""
                WorkShopName = ""
            Case "4" 'Customer
                VendorName = ""
                AircraftName = ""
                StoreName = ""
                CustomerName = txtSearchFor.Text.Trim
                WorkShopName = ""
            Case "5" 'WorkShop
                VendorName = ""
                AircraftName = ""
                StoreName = ""
                CustomerName = ""
                WorkShopName = txtSearchFor.Text.Trim
            Case "6" 'Work Order
        End Select
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("OrderText") = OrderText
        Session("ReceiptText") = ReceiptText
        Session("IssueText") = IssueText
        Session("InvoiceText") = InvoiceText
        Session("Name") = Name
        Session("OrderNo") = OrderNo
        Session("ReceiptNo") = ReceiptNo
        Session("IssueNo") = IssueNo
        Session("InvoiceNo") = InvoiceNo
        Session("WoNo") = WoNo
        Session("WOText") = WOText
        Session("ReceivedFromType") = ReceivedFromType
        Session("VendorName") = VendorName
        Session("AircraftName") = AircraftName
        Session("StoreName") = StoreName
        Session("CustomerName") = CustomerName
        Session("WorkShopName") = WorkShopName
        Session("InternalReceiptNoSearch") = InternalReceiptNoSearch
        Session("DCNoSearch") = DCNoSearch
        Session("PartNoSearch") = PartNoSearch
        Session("ReleaseNoteNoSearch") = ReleaseNoteNoSearch
        Session("CustomBillofEntrySearch") = CustomBillofEntrySearch
        Session("SerialNoSearch") = SerialNoSearch
        Session("BatchNoSearch") = BatchNoSearch
        Session("GSENoSearch") = GSENoSearch
        Session("SearchText") = SearchText 'Ajay 19-01-2023
    End Sub
    Private Sub ClearControls()
        'cmbPeriod.SelectedIndex = 0
        cmbOrderText.SelectedIndex = 0
        cmbReceiptText.SelectedIndex = 0
        cmbIssueText.SelectedIndex = 0
        cmbInvoiceText.SelectedIndex = 0
        cmbStatus.SelectedIndex = 0
        cmbReceivedFromType.SelectedIndex = 0
        cmbWoText.SelectedIndex = 0
        txtSearchFor.Text = ""
        txtOrderNo.Text = ""
        txtReceiptNo.Text = ""
        txtIssueNo.Text = ""
        txtInvoiceNo.Text = ""
        txtWONo.Text = ""
    End Sub
    Private Sub SetTitle()
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        ' lblList.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        lblList.Text = "List of Goods Receipt"
        mModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        Session("mModuleName") = mModuleName
        If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
            'lblList.Text = "List of Goods Receipt " + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]" 'Added by shweta on 22-12-11
            lblList.Text = "List of Goods Receipt "   'Added by Ajay on 08-02-2023
            'btnBottomAddNew.ToolTip = "Click to Add New Goods Receipt" Ajay
            'btnBottomPrint.ToolTip = "Click to Print List of Goods Receipt" Ajay
            'btnBottomClose.ToolTip = "Click to close List of Goods Receipt" Ajay
            btnAddNewTop.ToolTip = "Click to Add New Goods Receipt"
            btnPrintTop.ToolTip = "Click to Print List of Goods Receipt"
            btnCloseTop.ToolTip = "Click to close List of Goods Receipt"
        Else
            'lblList.Text = "List of Goods Receipt " + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]" 'Added by shweta on 22-12-11
            lblList.Text = "List of Goods Receipt " 'Added by Ajay on 08-02-2023
        End If
        upnlTitle.Update()
    End Sub
    Private Sub addAttributes()
        txtReceiptNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReceiptNo').value,event)")
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOrderNo').value,event)")
        txtIssueNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtIssueNo').value,event)")
        txtInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtInvoiceNo').value,event)")
        txtWONo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtWONo').value,event)")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        Select Case mTransTypeID
            Case Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder
                IsInRoleString = "RCIFromPO"
            Case Util.Trans.ReceiptAgainstLoanIssueToVendor
                IsInRoleString = "RCIFromVendorForLoanReturn"
            Case Util.Trans.ExchangeRepairReceivedFromVendor
                IsInRoleString = "RCIFromVendor"
            Case Util.Trans.ReceivedFromAircraft
                IsInRoleString = "RCIFromAircraft"
            Case Util.Trans.ReceiptAgainstLoanIssuedToAircraft
                IsInRoleString = "RCIFromAircraftForLoanReturn"
            Case Util.Trans.ReceivedFromOtherStore
                IsInRoleString = "RCIFromStore"
            Case Util.Trans.LoanTakenFromStore
                IsInRoleString = "RCIFromStoreForLoan"
            Case Util.Trans.ReceiptAgainstLoanIssuedToStore
                IsInRoleString = "RCIFromStoreForLoanReturn"
            Case Util.Trans.ReceiptAgainstLoanIssueToCustomer
                IsInRoleString = "RCIFromCustomerForLoanReturn"
            Case Util.Trans.AssembledFromWorkShop
                IsInRoleString = "AssembledFromWorkShop"
            Case Util.Trans.ReceiptAgainstLoanIssuedToWorkShop
                IsInRoleString = "RCIFromWorkShopForLoanReturn"
            Case Util.Trans.RCIFromWorkOrderAsReturn
                IsInRoleString = "RCIFromWorkOrderReturn"
            Case Util.Trans.RCIFromAircraftAsCoreUnitReturn
                IsInRoleString = "RCIFromAircraftAsCoreUnitReturn"
            Case Util.Trans.RCIFromSupplierAsNone
                IsInRoleString = "RCIFromSupplierAsNone"
            Case Util.Trans.DisassembledFromWorkShop
                IsInRoleString = "DisassembledFromWorkShop"
            Case Util.Trans.ReceivedfromSupplierRentalLease
                IsInRoleString = "ReceivedfromSupplierRentalLease"
            Case Util.Trans.ReceiptasLoanFromSupplier
                IsInRoleString = "ReceiptasLoanFromSupplier"
            Case Util.Trans.ReceiptasLoanFromCustomer
                IsInRoleString = "ReceiptasLoanFromCustomer"
            Case Util.Trans.ReceiptFromCustomer
                IsInRoleString = "RCIFromCustomer"
            Case Util.Trans.ReceivedFromCustomerAsForRepair
                IsInRoleString = "ReceivedFromCustomerAsForRepair"
            Case Util.Trans.RCIFromWorkOrder
                IsInRoleString = "RCIFromWorkOrder"
            Case Util.Trans.ReceivedFromWorkShopAsServiceableReturned        'Added By Prashant 10-Sep-2014 'ALL10092014
                IsInRoleString = "ReceivedFromWorkShopAsServiceablReturned"
        End Select
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
            Case Rights.Authorized                              'Added By Prashant 17-Aug-2011
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
        Return True
    End Function
    Private Sub SetGrid()
        'Ajay
        'Dim P As Boolean
        'For j As Integer = 0 To dgReceiptCumInvoiceList.Rows.Count - 1
        '    P = CType(Me.dgReceiptCumInvoiceList.Rows.Item(j).Cells(21).Text, Boolean)
        '    If P = False Then
        '        dgReceiptCumInvoiceList.Rows.Item(j).Cells(18).Enabled = False
        '    End If
        'Next
        dgReceiptCumInvoiceList.Columns(4).Visible = IIf(AppSettings("ClientCode") = "Taj", True, False)
        'btnBottomPrint.Enabled = IIf(dgReceiptCumInvoiceList.Rows.Count = 0, False, True) Ajay
        btnPrintTop.Enabled = IIf(dgReceiptCumInvoiceList.Rows.Count = 0, False, True)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("13", , True, "(All)")
        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
        mDistinctTextListforInvoice = DistinctTextListForInvoice.GetDistinctTextListForInvoice("4", , True, "(All)")
        mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
        cmbReceiptText.DataSource = mDistinctTextListForReceipt
        cmbOrderText.DataSource = mDistinctTextListForOrder
        cmbIssueText.DataSource = mDistinctTextListForIssue
        cmbInvoiceText.DataSource = mDistinctTextListforInvoice
        cmbWoText.DataSource = mDistinctWOText
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(7)
        Session("mTransactionListCount") = mTransactionListCount
        mReceiptTypeList = ReceiptTypeList.GetReciptTypeList(0)
        cmbReceivedAs.DataSource = mReceiptTypeList
        Session("mReceiptTypeList") = mReceiptTypeList
        DataBind()
    End Sub
    Private Sub GridBind()
        dgReceiptCumInvoiceList.DataSource = mReceiptCumInvoiceList
        dgReceiptCumInvoiceList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = IIf(AppSettings("ClientCode") = "CE", "As per criteria : ", "As per criteria : ") & totalCount & " Record(s) found."
        Else
            'lblResult.Text = IIf(AppSettings("ClientCode") = "CE", "List of Goods Receipt as per criteria : ", "List of Goods Receipt as per criteria : ") & currentrow + 1 & " to " & currentrow + mReceiptCumInvoiceList.Count & " of " & totalCount & " Record(s) found."
            lblResult.Text = IIf(AppSettings("ClientCode") = "CE", "As per criteria : ", "As per criteria : ") & totalCount & " Record(s) found." ''Ajay 08-02-2023
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
        dgReceiptCumInvoiceList.DataBind()
        SetGrid()
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try

			ClearAll()
			addAttributes()
			GetSession()

			EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 20-Jul-2011 For All19072011

			If Not IsPostBack And Session("sender") = "" Then

				mTransTypeID = Request.QueryString("TransTypeId")
				Session("mTransTypeId") = mTransTypeID
				Session("MiddleFrame") = "wfReceiptCumInvoiceList_Ajax.aspx?" 'TransTypeId=" & mTransTypeID

				'Ajay 07-Nov-2022
				If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "RCIFromPO") Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
				Else
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
				End If
				'--------------------------

				cmbShowE.SelectedValue = "4" 'Ajay 18-Jan-2023
				DataFieldBind()
				SetControl()
				SetGrid()
				SetTitle()

				If Session("RCICreatedFromNewApplication") Is Nothing Then

					If CBool(AppSettings("NewUi")) Then
						CreateRCIFromNewApplication(sender:=sender, e:=e)
						Session("RCICreatedFromNewApplication") = True
					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
    Private Sub dgReceiptCumInvoiceList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptCumInvoiceList.RowCommand

		Try

			Dim index As Integer = CInt(e.CommandArgument)
			Dim mID As Guid = mReceiptCumInvoiceList(index).ReceiptID

			'Ajay 07-Nov-2022
			If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO") Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav()Then;", True)
			Else
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
			End If

			Select Case e.CommandName
				Case "EditView"

					Dim mReceiptID As Guid = mReceiptCumInvoiceList(index).ReceiptID
					Dim mInvoiceID As Guid = mReceiptCumInvoiceList(index).InvoiceID
					mTransTypeID = mReceiptCumInvoiceList(index).TransID
					If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
						GridBind()
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
						Exit Sub
					End If
					EditRecord(mReceiptID, mInvoiceID)
					mTransTypeID = mReceiptCumInvoice.TransTypeID 'Changed By Utkarsh On 20-Jul-2011 For All19072011

					UpdateItemGridView()
					GridBind()
					SetTitle()
					mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mReceiptCumInvoiceList(mReceiptCumInvoice.ID).Name
					MarkLog(Util.Action.Edit, TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID) 'End
					Dim str As String
					str = "openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=wfReceiptCumInvoiceList_Ajax.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				Case "DeleteRecord"
					Dim mReceiptID As Guid = mReceiptCumInvoiceList(index).ReceiptID
					Dim mInvoiceID As Guid = mReceiptCumInvoiceList(index).InvoiceID
					mTransTypeID = mReceiptCumInvoiceList(index).TransID
					If (Not IsInRole(Rights.Delete)) Then
						GridBind()
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
						Exit Sub
					End If
					DeleteRecord(mReceiptID, mInvoiceID)
				Case "ViewRec"

					Dim mFileAttachments As New FileAttachments
					mFileAttachments = FileAttachments.GetChildFileAttachments(ReferenceID:=mID)
					Dim AttachmentCount As Integer = mFileAttachments.Count

					If AttachmentCount > 1 Then

						Session("mFileAttachments") = mFileAttachments
						ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "OpenAttachWindow();", True)

					Else

						Dim FileAttach As FileAttach
						FileAttach = FileAttach.GetAttachment(ReferenceID:=mID)

						AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

						ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

					End If
					dgReceiptCumInvoiceList.DataSource = mReceiptCumInvoiceList
					dgReceiptCumInvoiceList.DataBind()
			End Select
			SetGrid()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
    Private Sub dgReceiptCumInvoiceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReceiptCumInvoiceList.PageIndexChanging
        'dgReceiptCumInvoiceList.PageIndex = e.NewPageIndex
        'mCurrentpage = e.NewPageIndex
        'GridBind()
        'UpdateItemGridView()
        'Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        'dgReceiptCumInvoiceList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 18-Jan-2023

        '' Ajay 08-02-2023
        dgReceiptCumInvoiceList.PageIndex = e.NewPageIndex
        dgReceiptCumInvoiceList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgReceiptCumInvoiceList.DataSource = mReceiptCumInvoiceList
        Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        dgReceiptCumInvoiceList.DataBind()
        SetGrid()

        '-----------------
        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgReceiptCumInvoiceList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = e.NewPageIndex
        pagecount = CInt(Session("pagecount"))
    End Sub
    Private Sub dgReceiptCumInvoiceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgReceiptCumInvoiceList.Sorting
        mReceiptCumInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mReceiptCumInvoiceList") = mReceiptCumInvoiceList
        GridBind()
        UpdateItemGridView()
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
        dgReceiptCumInvoiceList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex, , ReceivedFromType)
        dgReceiptCumInvoiceList.DataBind()


        SetGrid()
        upnlGridView.Update()
        upnlTitle.Update()
        upnBottomButtons.Update()
        upnlResult.Update()


    End Sub
    'Private Sub cmbSearchCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchCriteria.SelectedIndexChanged
    '    cmbPeriod.SelectedIndex = 0
    '    cmbReceivedFrom.SelectedIndex = 0
    '    ClearControls()
    '    Dim PeriodIndex As Int32 = CInt(IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0))
    '    ControlVisibility(cmbSearchCriteria.SelectedIndex, PeriodIndex, 0, 0, 0, 0)
    '    SetPeriod(DateIndex)
    '    If cmbSearchCriteria.Enabled = True Then
    '        SetFocus(cmbSearchCriteria)
    '    End If
    'End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged
        Dim PeriodIndex As Int32 = CInt(IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0))
        ControlVisibility(1, PeriodIndex, 0, 0, 0, 0)
        SetPeriod(PeriodIndex)
        If cmbPeriod.Enabled = True Then
            setFocus(cmbPeriod)
        End If
    End Sub
    Private Sub cmbOrderText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOrderText.SelectedIndexChanged, cmbReceiptText.SelectedIndexChanged, cmbIssueText.SelectedIndexChanged, cmbInvoiceText.SelectedIndexChanged, cmbWoText.SelectedIndexChanged
        If sender.ID = "cmbOrderText" Then
            txtOrderNo.Text = "0"
            If cmbOrderText.Enabled = True Then
                setFocus(cmbOrderText)
            End If
        ElseIf sender.ID = "cmbReceiptText" Then
            txtReceiptNo.Text = "0"
            If cmbReceiptText.Enabled = True Then
                setFocus(cmbReceiptText)
            End If
        ElseIf sender.ID = "cmbIssueText" Then
            txtIssueNo.Text = "0"
            If cmbIssueText.Enabled = True Then
                setFocus(cmbIssueText)
            End If
        ElseIf sender.ID = "cmbInvoiceText" Then
            txtInvoiceNo.Text = "0"
            If cmbInvoiceText.Enabled = True Then
                setFocus(cmbInvoiceText)
            End If
        ElseIf sender.ID = "cmbWoText" Then
            txtWONo.Text = "0"
            If cmbWoText.Enabled = True Then
                setFocus(cmbWoText)
            End If
        End If
    End Sub
    Private Sub cmbReceivedFrom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReceivedFrom.SelectedIndexChanged
        mReceiptTypeList = ReceiptTypeList.GetReciptTypeList(cmbReceivedFrom.SelectedIndex)

        cmbReceivedAs.Enabled = IIf(mReceiptTypeList.Count = 0, False, True)
        btnAddNewTop.Enabled = IIf(mReceiptTypeList.Count = 0, False, True)
        'btnBottomAddNew.Enabled = IIf(mReceiptTypeList.Count = 0, False, True) Ajay

        cmbReceivedAs.DataSource = mReceiptTypeList
        cmbReceivedAs.DataBind()
        If cmbReceivedFrom.Enabled = True Then
            setFocus(cmbReceivedFrom)
        End If
        Session("mReceiptTypeList") = mReceiptTypeList
    End Sub
    Private Sub cmbReceivedFromType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbReceivedFromType.SelectedIndexChanged
        txtSearchFor.Text = ""
    End Sub
    'btnBottomAddNew.Click, Ajay
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click

        'Ajay 07-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "ReceiptPO") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
        Dim str As String
        NewRecord()
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Session("RCIItem") = True
        'Changed By Utkarsh On 20-Jul-2011 For All19072011
        SetTitle()
        MarkLog(Util.Action.[New], TransactionList.GetTransactionList().GetModuleName(mReceiptCumInvoice.TransTypeID).ToString, "", Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
        'End
        Session("mFromToTypeID") = CInt(IIf(mReceiptCumInvoice.FromTypeID = 14, 1, mReceiptCumInvoice.FromTypeID))  '8  'Store

        'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
        'mFileAttach = FileAttach.NewAttachment(Guid.Empty, mReceiptCumInvoice.ID)
        'commented on 29-jun-2020
        mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceiptCumInvoice.ID)
        Session("mFileAttach") = mFileAttach
        'End

        Select Case mTransTypeID
            'Coad Added 
            'DEVEN 19/03/2008
            Case 7
                Dim mPrevTransID As Guid = Guid.Empty
                Dim mPrimaryOrderType As Integer
                Dim mTransaction As Integer
                Dim mFromPartList As Boolean
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    mPrevTransID = Guid.Empty
                Else
                    mPrevTransID = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).OrderItemDetailForReceipt.OrderID
                End If
                If CType(mTransTypeID, Trans) = Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder Then
                    mPrimaryOrderType = 6 'TransListOf.Order_Outright
                ElseIf CType(mTransTypeID, Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Then
                    mPrimaryOrderType = 4 'TransListOf.Order_ExchangeRepair
                End If
                mTransaction = 3 'Transaction.Order
                mFromPartList = False
                Session("OpenFrom") = 1
                Session("mPrevTransID") = mPrevTransID
                Session("mPrimaryOrderType") = mPrimaryOrderType
                Session("mTransaction") = mTransaction
                Session("mFromPartList") = mFromPartList
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType=2');"
            Case 8    'ReceivedFromOtherStore
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
                End If
                Session("OpenFrom") = 1
                'Session("mFromToTypeID") = 8
                Session("mPrimaryOrderType") = 4
                Session("mTransaction") = 4
                Session("mFromPartList") = False
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 9
                str = "openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 10
                'Session("mFromToTypeID") = 1   'Vendor
                Session("mPrimaryOrderType") = 4 'TransListOf.Order_Replaced
                Session("mTransaction") = 3 'Transaction.Order
                Session("mFromPartList") = False 'True
                Session("OpenFrom") = 1
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).OrderItemDetailForReceipt.OrderID
                End If
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 11
                str = "openledgersame('wfPendingLoanToRecover_Ajax.aspx?BackPage=index.aspx');"
            Case 12    'LoanTaken
                Session("mPrimaryOrderType") = 4  'TransListOf.Issue_LoanTaken
                Session("mTransaction") = 4   'Transaction.Issue
                Session("mFromPartList") = False
                Session("OpenFrom") = 1
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
                End If
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 13
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
                End If
                Session("mPrimaryOrderType") = 4  'TransListOf.Order_LoanRecovery
                Session("mTransaction") = 4  'Transaction.Issue
                Session("mFromPartList") = False
                Session("OpenFrom") = 1
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 27, 28
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
                End If
                Session("mPrimaryOrderType") = 4 'TransListOf.Order_LoanRecovery
                Session("mTransaction") = 4 'Transaction.Issue
                Session("mFromPartList") = False
                Session("OpenFrom") = 1
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 46, 56
                str = "openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 47
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
                End If
                Session("mPrimaryOrderType") = 4  'TransListOf.Order_LoanRecovery
                Session("mTransaction") = 4  'Transaction.Issue
                Session("mFromPartList") = False
                Session("OpenFrom") = 1
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 48, 50, 57   'Added By Prashant 21-May-2010  57
                str = "openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 52  'Received from Customer
                str = "openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx&mType= 2');"
                'Added By Prashant 5-Jan-2009 ****************************************************************************************************************
            Case 54
                Dim mPrevTransID As Guid = Guid.Empty
                Dim mPrimaryOrderType As Integer
                Dim mTransaction As Integer
                Dim mFromPartList As Boolean
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    mPrevTransID = Guid.Empty
                Else
                    mPrevTransID = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).OrderItemDetailForReceipt.OrderID
                End If
                If CType(mTransTypeID, Trans) = Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder Then
                    mPrimaryOrderType = 3 'TransListOf.Order_Outright
                ElseIf CType(mTransTypeID, Trans) = Util.Trans.ReceivedfromSupplierRentalLease Then   'Added By Prashant 6-Jan-2009
                    mPrimaryOrderType = 5 'TransListOf.Order_Rental / Lease
                ElseIf CType(mTransTypeID, Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Then
                    mPrimaryOrderType = 4 'TransListOf.Order_ExchangeRepair
                End If
                mTransaction = 3 'Transaction.Order
                mFromPartList = False
                Session("OpenFrom") = 1
                Session("mPrevTransID") = mPrevTransID
                Session("mPrimaryOrderType") = mPrimaryOrderType
                Session("mTransaction") = mTransaction
                Session("mFromPartList") = mFromPartList
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType=2');"
            Case 61                   'Added By Utkarsh 09-Dec-2010
                Dim mPrevTransID As Guid = Guid.Empty
                Dim mPrimaryOrderType As Integer
                Dim mTransaction As Integer
                Dim mFromPartList As Boolean
                mPrevTransID = Guid.Empty
                mPrimaryOrderType = 3
                mTransaction = 3 'Transaction.Order
                mFromPartList = False
                Session("OpenFrom") = 1
                Session("mPrevTransID") = mPrevTransID
                Session("mPrimaryOrderType") = mPrimaryOrderType
                Session("mTransaction") = mTransaction
                Session("mFromPartList") = mFromPartList
                str = "openledgersame('wfnPendingWOListForRemoveComp_Ajax.aspx?BackPage=index.aspx&mType= 2');"
            Case 62 'Added by Saylee  'Worked order Return
                Session("mPrimaryOrderType") = 3 'TransListOf.Order_Replaced
                Session("mTransaction") = 4 'Transaction.Issue
                Session("mFromPartList") = False 'True
                Session("OpenFrom") = 1
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
                End If
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=index.aspx&mType= 2');"
                ''Added By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1
            Case 66
                str = "openledgersame('wfPartListForRCIFromAircraftAsCoreUnitReturn_Ajax.aspx?BackPage=index.aspx&mType=2');"
                ''Added By Prashant 10-Sep-2014 'ALL10092014
            Case 73
                str = "openledgersame('wfReceivedFromWorkShopAsServiceablReturned_Ajax.aspx?BackPage=index.aspx&mType=2');"
                'End
            Case Else
                str = "openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=wfReceiptCumInvoiceList_Ajax.aspx');"
        End Select
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    'btnBottomClose.Click, Ajay
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Session("mCount") = Nothing
        mDistinctTextListForOrder = Nothing
        mDistinctTextListForReceipt = Nothing
        mDistinctTextListForIssue = Nothing
        mDistinctTextListforInvoice = Nothing
        mReceiptCumInvoice = Nothing
        mModuleName = Nothing
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnGridPaging_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        dgReceiptCumInvoiceList.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(1, , ReceivedFromType)
        upnlResult.Update()
    End Sub

    'Ajay 07-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 07-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "RCIFromPO")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 07-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "RCIFromPO")
    End Sub
	'-----

	Private Sub CreateRCIFromNewApplication(sender As Object, e As EventArgs) Handles btnCheckoutNewApplication.Click

		Try

			Dim NewUrl As String = RedirectToNewUIHelper.NavigationLinkForNewUI(Request:=Request,
																				NavigationLink:="store/InventoryManagement?tab=material-in")

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Open in New Tab",
												$"window.open('{NewUrl}', '_blank');",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

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
    Private Function GetTitle() As String               'New Addition
        'By :- Jyoti
        'Dated On :- 11/5/2007
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString

        If mTitle = "" Or mTitle = "(SELECT)" Or mTitle = "<SELECT>" Then
            If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
                Return "Goods Receipt List Report"
            Else
                Return "Goods Receipt List Report"
            End If
        Else
            Return mTitle + " List Report"
        End If
    End Function
    'btnBottomPrint.Click, Ajay
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
        'For Receipt-Cum-Invoice List
        Dim Rpt As New crReceiptCumInvoiceList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        Dim Title As String = GetTitle()
        CallFindNow(1, True, ReceivedFromType)
        SearchStr1 = "" '"The report shows records filtered by the following criteria"
        SearchStr2 = ""
		'If cmbSearchCriteria.SelectedIndex = 0 Then
		'    'All
		'    SearchStr1 = "The report shows all records till date."
		'    SearchStr2 = ""
		'ElseIf cmbSearchCriteria.SelectedIndex = 1 Then
		'    'Date
		'    If cmbPeriod.SelectedIndex = 0 Then
		'        SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text
		'    ElseIf cmbPeriod.SelectedIndex = 6 Then
		'        SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text + "    " + lblFromDate.Text + ":" + "  " + New SmartDate(txtFromDate.Text).FormattedText + "    " + lblToDate.Text + ":" + " " + New SmartDate(txtToDate.Text).FormattedText
		'    Else
		'        SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text + "    " + lblFromDate.Text + ":" + " " + New SmartDate(txtFromDate.Text).FormattedText + "    " + lblToDate.Text + ":" + " " + New SmartDate(txtToDate.Text).FormattedText
		'    End If
		'ElseIf cmbSearchCriteria.SelectedIndex = 2 Then
		'    'Receipt No.
		'    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbReceiptText.SelectedItem.Text + "    " + lblNo.Text + " " + txtReceiptNo.Text
		'ElseIf cmbSearchCriteria.SelectedIndex = 3 Then
		'    'Order No.
		'    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbOrderText.SelectedItem.Text + "    " + lblNo.Text + " " + txtOrderNo.Text
		'ElseIf cmbSearchCriteria.SelectedIndex = 4 Then
		'    'Issue No.
		'    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbIssueText.SelectedItem.Text + "    " + lblNo.Text + " " + txtIssueNo.Text
		'ElseIf cmbSearchCriteria.SelectedIndex = 5 Then
		'    'Invoice No.
		'    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbInvoiceText.SelectedItem.Text + "    " + lblNo.Text + " " + txtInvoiceNo.Text
		'ElseIf (cmbSearchCriteria.SelectedIndex = 6 Or cmbSearchCriteria.SelectedIndex = 7 Or cmbSearchCriteria.SelectedIndex = 8 Or _
		'        cmbSearchCriteria.SelectedIndex = 9 Or cmbSearchCriteria.SelectedIndex = 10 Or cmbSearchCriteria.SelectedIndex = 11 Or _
		'        cmbSearchCriteria.SelectedIndex = 12 Or cmbSearchCriteria.SelectedIndex = 15 Or cmbSearchCriteria.SelectedIndex = 16) Then
		'    'Internal Receipt No., Release Note No., DC No., Part Number, Custom Bill of Entry, Serial No., Description, Batch No
		'    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtSearchFor.Text
		' ElseIf cmbSearchCriteria.SelectedIndex = 13 Then
		'    'Status
		'    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
		'ElseIf cmbSearchCriteria.SelectedIndex = 14 Then
		'    'Received From
		'    SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbReceivedFromType.SelectedItem.Text + ":" + " " + IIf(cmbReceivedFromType.SelectedIndex = 6, cmbWoText.SelectedItem.Text + "    " + lblNo.Text + " " + txtWONo.Text, txtSearchFor.Text)
		'End If

		'ReportDetails.Add(New rptStatus(, 0, , _
		'                dgReceiptCumInvoiceList.Columns.Item(2).HeaderText, dgReceiptCumInvoiceList.Columns.Item(3).HeaderText, dgReceiptCumInvoiceList.Columns.Item(4).HeaderText, _
		'                dgReceiptCumInvoiceList.Columns.Item(5).HeaderText, dgReceiptCumInvoiceList.Columns.Item(6).HeaderText, dgReceiptCumInvoiceList.Columns.Item(7).HeaderText, _
		'                dgReceiptCumInvoiceList.Columns.Item(8).HeaderText, dgReceiptCumInvoiceList.Columns.Item(9).HeaderText, dgReceiptCumInvoiceList.Columns.Item(10).HeaderText, _
		'                 dgReceiptCumInvoiceList.Columns.Item(11).HeaderText, dgReceiptCumInvoiceList.Columns.Item(12).HeaderText, dgReceiptCumInvoiceList.Columns.Item(13).HeaderText, _
		'                 dgReceiptCumInvoiceList.Columns.Item(14).HeaderText, dgReceiptCumInvoiceList.Columns.Item(15).HeaderText)) '', dgReceiptCumInvoiceList.Columns.Item(16).HeaderText))	'Comment by Sankalp to added ABWno

		ReportDetails.Add(New rptStatus(, 0, ,
						LHLabel:=dgReceiptCumInvoiceList.Columns.Item(2).HeaderText,
						LHData:=dgReceiptCumInvoiceList.Columns.Item(3).HeaderText,
						LHLabel1:=dgReceiptCumInvoiceList.Columns.Item(4).HeaderText,
						LHData1:=dgReceiptCumInvoiceList.Columns.Item(5).HeaderText,
						LHLabel2:=dgReceiptCumInvoiceList.Columns.Item(6).HeaderText,
						LHData2:=dgReceiptCumInvoiceList.Columns.Item(7).HeaderText,
						LHData3:=dgReceiptCumInvoiceList.Columns.Item(8).HeaderText,
						LHData4:=dgReceiptCumInvoiceList.Columns.Item(9).HeaderText,
						LHData5:=dgReceiptCumInvoiceList.Columns.Item(10).HeaderText,
						 LHData6:=dgReceiptCumInvoiceList.Columns.Item(12).HeaderText,
						 LHData7:=dgReceiptCumInvoiceList.Columns.Item(13).HeaderText,
						 LHData8:=dgReceiptCumInvoiceList.Columns.Item(14).HeaderText,
						 LHData9:=dgReceiptCumInvoiceList.Columns.Item(15).HeaderText,
						 LHData10:=dgReceiptCumInvoiceList.Columns.Item(16).HeaderText,
						LHData11:=dgReceiptCumInvoiceList.Columns.Item(11).HeaderText)) '', dgReceiptCumInvoiceList.Columns.Item(16).HeaderText)) 'Sankalp 



		Dim I As Integer
        For I = 0 To mReceiptCumInvoiceList.Count - 1
			'ReportDetails.Add(New rptStatus(, 1, , mReceiptCumInvoiceList(I).RecCumInvDateFormatted, mReceiptCumInvoiceList(I).ReceiptNo, mReceiptCumInvoiceList(I).IntReceiptNo, mReceiptCumInvoiceList(I).RCIType, mReceiptCumInvoiceList(I).Name, mReceiptCumInvoiceList(I).VendorInvoiceNo, mReceiptCumInvoiceList(I).VendorInvoiceDateFormatted.ToString, _
			'               IIf(mReceiptCumInvoiceList(I).DCNO = "", "", mReceiptCumInvoiceList(I).DCNO), mReceiptCumInvoiceList(I).DCDateFormatted.ToString, mReceiptCumInvoiceList(I).CurrencyName, mReceiptCumInvoiceList(I).CGrantTotal.ToString, mReceiptCumInvoiceList(I).StatusName, mReceiptCumInvoiceList(I).UserName, mReceiptCumInvoiceList(I).AuthorizedBy)) 'Comment by Sankalp to added ABWno
			ReportDetails.Add(New rptStatus(,
								1,
                                             ,
								mReceiptCumInvoiceList(I).RecCumInvDateFormatted,
								mReceiptCumInvoiceList(I).ReceiptNo,
								mReceiptCumInvoiceList(I).IntReceiptNo,
								mReceiptCumInvoiceList(I).RCIType,
								mReceiptCumInvoiceList(I).Name,
								mReceiptCumInvoiceList(I).VendorInvoiceNo,
								mReceiptCumInvoiceList(I).VendorInvoiceDateFormatted.ToString,
								IIf(mReceiptCumInvoiceList(I).DCNO = "", "", mReceiptCumInvoiceList(I).DCNO),
								mReceiptCumInvoiceList(I).DCDateFormatted.ToString,
								mReceiptCumInvoiceList(I).CurrencyName,
								mReceiptCumInvoiceList(I).CGrantTotal.ToString,
								mReceiptCumInvoiceList(I).StatusName,
								mReceiptCumInvoiceList(I).UserName,
								mReceiptCumInvoiceList(I).AuthorizedBy,
								LHData11:=mReceiptCumInvoiceList(I).AWBNo,
))
		Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, Title, SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mReceiptCumInvoiceList.Count = 0 Then
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
    'Ajay 19-Jan-2023 // 08-02-2023
    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        ''Dim ExpiryDateList = ((From res In mWOList).ToList.Take(CInt(DropDownList1.SelectedItem.ToString))).ToList
        'dgReceiptCumInvoiceList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        'dgReceiptCumInvoiceList.DataSource = mReceiptCumInvoiceList
        'dgReceiptCumInvoiceList.DataBind()
        'SetControl()
        'upnlGridView.Update()
        'upnlResult.Update(
        setVariables()
        dgReceiptCumInvoiceList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgReceiptCumInvoiceList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        pagecount = CInt(Session("pagecount"))
        SetControl()
        upnlGridView.Update()
        upnlResult.Update()
    End Sub
    'Ajay 19-Jan-2023
    Private Sub txtSearchBox_TextChanged(sender As Object, e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        dgReceiptCumInvoiceList.DataBind()

        SetControl()
        SetControl()
        upnlGridView.Update()
        upnlResult.Update()
    End Sub
    '-----
#End Region

#End Region

End Class