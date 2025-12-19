'Added By Vikrant On 25-Feb-2016
Imports System.Text
Public Class wfOpenAuthorizedTransactions
    Inherits System.Web.UI.Page
#Region "Enumeration"
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
    Public mReceipt As Receipt
    Public mIssue As Issue
    Public mOrder As Order
    Public mRequisitionNew As RequisitionNew 'Added By Vikrant On 19-Dec-2016 For ALL19122016
    Public mnWO As nWO
    Public mFetchTransactionToReOpen As FetchTransactionToReOpen
    Public mTransactionID As Guid
    Dim EventLogID As Guid                              'Added By Utkarsh On 20-Jul-2011 For All19072011
    Public mInvoice As Invoice 'Added By Vikrant On 15-May-2019 For ALL15052019
    Public mEnquiry As Enquiry
    Public mQuotation As Quotation
    Public mExportInvoice As ExportInvoice  'Ajay 29-Nov-2022
    Public mAuditExecution As AuditExecution
    Public mMELSnagCorrectiveActionFromOpenAuthorizedTransactions As MELSnagCorrectiveAction
    Dim mTransTypeName As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReceipt = CType(Session("mReceipt"), Receipt)
        mIssue = CType(Session("mIssue"), Issue)
        mFetchTransactionToReOpen = CType(Session("mFetchTransactionToReOpen"), FetchTransactionToReOpen)
        mTransactionID = CType(Session("mTransactionID"), Guid)
        mnWO = CType(Session("mnWO"), nWO)
        mExportInvoice = CType(Session("mExportInvoice"), ExportInvoice)  'Ajay 29-Nov-2022
    End Sub

    Private Sub RemoveSessions()
        Session.Remove("mFetchTransactionToReOpen")
        Session.Remove("mReceipt")
        Session.Remove("mIssue")
        Session.Remove("mnWO")
        Session.Remove("mTransactionID")
        Session.Remove("mExportInvoice") 'Ajay 29-Nov-2022
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfOpenAuthorizedTransactions.aspx?") <= 0 Then
            RemoveSessions()
        End If
    End Sub
    'Added By Vikrant On 19-Dec-2016 For ALL19122016
    Private Function SetTransMadeAgainstText(ByVal strToConvert As String) As String
        Dim Text As New StringBuilder
        Dim Arr As String() = strToConvert.Split(",")
        For i As Integer = 0 To Arr.Length - 1
            If Arr(i) <> "" Then
                Text.Append(Arr(i))
                Text.Append("</br>")
            End If
        Next
        Return Text.ToString.TrimEnd("</br>")
    End Function
    'End
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "ReOpen" Then
                        Try

                            Session("sender") = ""

                            mFetchTransactionToReOpen = Session("mFetchTransactionToReOpen")
                            If cmbTransactionType.SelectedIndex = 1 Then 'Receipt
                                mReceipt = Receipt.GetReceipt(mFetchTransactionToReOpen(0).TransactionID)
                                If mFetchTransactionToReOpen(0).TransactionMadeAgainstText <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Receipt No. : " + mReceipt.ReceiptNo + ",Receipt Date : " + mReceipt.RecdDateFormatted.ToString + " is used in " + mFetchTransactionToReOpen(0).TransactionMadeAgainstText + " Transaction(s)", ErrorType.NoError, mReceipt.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Receipt Status can not be changed as it is being used in following Issue/Purchase Inoice/Component Reservation Transactions : " + mFetchTransactionToReOpen(0).TransactionMadeAgainstText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mReceipt.UpdateReceiptStatus(mReceipt.ID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Receipt No. : " + mReceipt.ReceiptNo + ",Receipt Date : " + mReceipt.RecdDateFormatted.ToString, ErrorType.NoError, mReceipt.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "Receipt Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                            ElseIf cmbTransactionType.SelectedIndex = 2 Then 'Issue
                                mIssue = Issue.GetIssue(mFetchTransactionToReOpen(0).TransactionID)
                                If mFetchTransactionToReOpen(0).TransactionMadeAgainstText <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Issue No. : " + mIssue.IssueNo + ",Issue Date : " + mIssue.IDateFormatted.ToString + " is used in " + mFetchTransactionToReOpen(0).TransactionMadeAgainstText + " Transaction(s)", ErrorType.NoError, mIssue.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Issue Status can not be changed as it is being used in following Receipt/Export Invoice Transactions : " + mFetchTransactionToReOpen(0).TransactionMadeAgainstText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If

                                mIssue.UpdateIssueStatus(mIssue.ID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Issue No. : " + mIssue.IssueNo + ",Issue Date : " + mIssue.IDateFormatted.ToString, ErrorType.NoError, mIssue.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "Issue Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                            ElseIf cmbTransactionType.SelectedIndex = 3 Then 'Order
                                mOrder = Order.GetOrder(mFetchTransactionToReOpen(0).TransactionID)
                                'Added By Vikrant On 19-Dec-2016 For ALL19122016
                                Dim TransText As String
                                TransText = SetTransMadeAgainstText(mFetchTransactionToReOpen(0).TransactionMadeAgainstText)
                                'End
                                If Trim(TransText) <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Order No. : " + mOrder.OrderNo + ",Order Date : " + mOrder.OrderDateFormatted.ToString + " is used in " + TransText.Replace("</br>", ", ").Trim.TrimEnd(",") + " Transaction(s)", ErrorType.NoError, mOrder.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Order Status can not be changed as it is being used in following Receipt/Issue/Requisition Item Transactions : " + TransText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mOrder.UpdateOrderStatus(mOrder.ID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Order No. : " + mOrder.OrderNo + ",Order Date : " + mOrder.OrderDateFormatted.ToString, ErrorType.NoError, mOrder.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "Order Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                                'Added By Vikrant On 19-Dec-2016 For ALL19122016
                            ElseIf cmbTransactionType.SelectedIndex = 4 Then
                                mRequisitionNew = RequisitionNew.GetRequisition(mFetchTransactionToReOpen(0).TransactionID)
                                'Added By Vikrant On 19-Dec-2016 For ALL19122016
                                Dim TransText As String
                                TransText = SetTransMadeAgainstText(mFetchTransactionToReOpen(0).TransactionMadeAgainstText)
                                'End
                                If Trim(TransText) <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Requisition No. : " + mRequisitionNew.RequisitionNo + ",Requisition Date : " + mRequisitionNew.ReqDateFormatted.ToString + " is used in " + TransText.Replace("</br>", ", ").Trim.TrimEnd(",") + " Transaction(s)", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Requisition Status can not be changed as it is being used in following Enquiry/Quotation/Order/Issue Transactions : " + TransText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mRequisitionNew.UpdateReqStatus(mFetchTransactionToReOpen(0).TransactionID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Requisition No. : " + mRequisitionNew.RequisitionNo + ",Requisition Date : " + mRequisitionNew.ReqDateFormatted.ToString, ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "Requisition Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                                'End
                            ElseIf cmbTransactionType.SelectedIndex = 5 Then 'Work Order
                                mnWO = nWO.GetWO(mFetchTransactionToReOpen(0).TransactionID)
                                Dim TransText As String
                                TransText = SetTransMadeAgainstText(mFetchTransactionToReOpen(0).TransactionMadeAgainstText)
                                If Trim(TransText) <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",WO No. : " + mnWO.WONumber + ",WO Date : " + mnWO.WODateFormatted.ToString + " is used in " + TransText.Replace("</br>", ", ").Trim.TrimEnd(",") + " Transaction(s)", ErrorType.NoError, mnWO.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Work Order Status can not be changed as it is being used in following Receipt/Issue Transactions : " + TransText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mnWO.UpdateWOStatus(mFetchTransactionToReOpen(0).TransactionID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",WO No. : " + mnWO.WONumber + ",WO Date : " + mnWO.WODateFormatted.ToString, ErrorType.NoError, mnWO.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "WO Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                            ElseIf cmbTransactionType.SelectedIndex = 6 Then 'Added By Vikrant On 15-May-2019 For ALL15052019
                                mInvoice = Invoice.GetInvoice(mFetchTransactionToReOpen(0).TransactionID)
                                Dim TransText As String
                                TransText = SetTransMadeAgainstText(mFetchTransactionToReOpen(0).TransactionMadeAgainstText)
                                If Trim(TransText) <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Invoice No. : " + mInvoice.InvoiceNo + ",Invoice Date : " + mInvoice.InvoiceDateFormatted.ToString + " is used in " + " Payment Transaction(s) dated : " + TransText.Replace("</br>", ", ").Trim.TrimEnd(","), ErrorType.NoError, mInvoice.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Invoice Status can not be changed as it is being used in following Payment Transactions Dated : " + TransText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mInvoice.UpdateInvStatus(mFetchTransactionToReOpen(0).TransactionID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Invoice No. : " + mInvoice.InvoiceNo + ",Invoice Date : " + mInvoice.InvoiceDateFormatted.ToString, ErrorType.NoError, mInvoice.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "Invoice Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                                'End
                            ElseIf cmbTransactionType.SelectedIndex = 7 Then
                                mEnquiry = Enquiry.GetEnquiry(mFetchTransactionToReOpen(0).TransactionID)
                                Dim TransText As String
                                TransText = SetTransMadeAgainstText(mFetchTransactionToReOpen(0).TransactionMadeAgainstText)
                                If Trim(TransText) <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Enquiry No. : " + mEnquiry.EnquiryNo + ",Enquiry Date : " + mEnquiry.DateFormatted.ToString + " is used in " + " Quotation Transaction(s) : " + TransText.Replace("</br>", ", ").Trim.TrimEnd(","), ErrorType.NoError, mEnquiry.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Enquiry Status can not be changed as it is being used in following Quotation Transactions : " + TransText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mEnquiry.UpdateEnquiryStatus(mFetchTransactionToReOpen(0).TransactionID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Enquiry No. : " + mEnquiry.EnquiryNo + ",Enquiry Date : " + mEnquiry.DateFormatted.ToString, ErrorType.NoError, mEnquiry.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "Enquiry Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                            ElseIf cmbTransactionType.SelectedIndex = 8 Then
                                mQuotation = Quotation.GetQuotation(mFetchTransactionToReOpen(0).TransactionID)
                                Dim TransText As String
                                TransText = SetTransMadeAgainstText(mFetchTransactionToReOpen(0).TransactionMadeAgainstText)
                                If Trim(TransText) <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Quotation No. : " + mQuotation.QuotationNo + ",Quotation Date : " + mQuotation.DateFormatted.ToString + " is used in " + " Order Transaction(s) : " + TransText.Replace("</br>", ", ").Trim.TrimEnd(","), ErrorType.NoError, mQuotation.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Quotation Status can not be changed as it is being used in following Order Transactions : " + TransText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mQuotation.UpdateQuotationStatus(mFetchTransactionToReOpen(0).TransactionID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Quotation No. : " + mQuotation.QuotationNo + ",Quotation Date : " + mQuotation.DateFormatted.ToString, ErrorType.NoError, mQuotation.ID, EventLogID)
                                MSGBoxCtrl.show("Status Changed!", "Quotation Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")

                                'Ajay 29-Nov-2022
                            ElseIf cmbTransactionType.SelectedIndex = 9 Then
                                mExportInvoice = ExportInvoice.GetExportInvoice(mFetchTransactionToReOpen(0).TransactionID)
                                Dim TransText As String
                                TransText = SetTransMadeAgainstText(mFetchTransactionToReOpen(0).TransactionMadeAgainstText)
                                If Trim(TransText) <> "" Then
                                    MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Export Invoice No. : " + mExportInvoice.ExportInvoiceTextNo + ",Export Invoice Date : " + mExportInvoice.ExportInvoiceDateFormatted.ToString + " is used in " + " Export Invoice Transaction(s) : " + TransText.Replace("</br>", ", ").Trim.TrimEnd(","), ErrorType.NoError, mExportInvoice.ID, EventLogID)
                                    MSGBoxCtrl.show("Alert", "Export Invoice can not be changed as it is being used in following Export Invoice Transactions : " + TransText, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mExportInvoice.UpdateExportInvoiceStatus(mFetchTransactionToReOpen(0).TransactionID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Export Invoice No. : " + mExportInvoice.ExportInvoiceTextNo + ",Export Invoice Date : " + mExportInvoice.ExportInvoiceDateFormatted.ToString, ErrorType.NoError, mExportInvoice.ID, EventLogID)
                                MSGBoxCtrl.Show("Status Changed!", "Export Invoice Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                            ElseIf cmbTransactionType.SelectedIndex = 10 Then 'Audit
                                mAuditExecution = AuditExecution.GetAuditExecution(mFetchTransactionToReOpen(0).TransactionID)

                                mAuditExecution.UpdateAuditExecutionStatus(mFetchTransactionToReOpen(0).TransactionID)
                                MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Audit No. : " + mAuditExecution.AuditNo + ", Date : " + mAuditExecution.AuditScheduleDateFormatted.ToString, ErrorType.NoError, mAuditExecution.ID, EventLogID)
                                MSGBoxCtrl.Show("Status Changed!", "Audit Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                            ElseIf cmbTransactionType.SelectedIndex = 11 Then 'Discrepancy
                                mMELSnagCorrectiveActionFromOpenAuthorizedTransactions = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=mFetchTransactionToReOpen(0).TransactionID)

                                mMELSnagCorrectiveActionFromOpenAuthorizedTransactions.UpdateMELSnagCorrectiveActionStatus(mFetchTransactionToReOpen(0).TransactionID)
								MarkLog(Action.Amend, "ReOpenAuthorizedTransactions", "UserName : " + HttpContext.Current.User.Identity.Name + ",Discrepancy No. : " + mMELSnagCorrectiveActionFromOpenAuthorizedTransactions.DefectNo + ", Date : " + mMELSnagCorrectiveActionFromOpenAuthorizedTransactions.DateOfOccurrenceFormatted.ToString, ErrorType.NoError, mMELSnagCorrectiveActionFromOpenAuthorizedTransactions.ID, EventLogID)
								MSGBoxCtrl.Show("Status Changed!", "Discrepancy Status changed successfully!!!", "", MsgBoxStyle.OkOnly, "OKStatus")
                            End If
                            '--------------------------------------


                        Catch ex As Exception
                            MSGBoxCtrl.show(MSGBox.Message_title.ErrorMessage, MSGBox.Message_text.ErrorMessage, ex.InnerException.ToString, MsgBoxStyle.OkOnly, "")
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "OKStatus" Then
                        ClearControls()
                        ControlVisibility()
                        upnlSearch.Update()
                        TransTypeName()
                        lblResultReceipt.Text = "List of " + mTransTypeName + " as per criteria : 0 Record(s) found."
                        upnlGridView.Update()
                    End If
            End Select
        End If
    End Sub
    Private Sub ClearControls()
        If cmbTransactionType.SelectedIndex = 10 Then
            txtTransactionNo.Text = "0"
        Else
            txtTransactionNo.Text = String.Empty
        End If
        txtTransactionText.Text = String.Empty
        'cmbTransactionType.ClearSelection()
        dgReceiptList.DataSource = Nothing
        dgReceiptList.DataBind()
    End Sub
    Private Sub FindNow(ByVal TransactionType As Integer, ByVal TransactionText As String, ByVal TransactionNo As Integer)
        If TransactionType = 1 Or TransactionType = 2 Or TransactionType = 4 Or TransactionType = 5 Or TransactionType = 6 Or TransactionType = 7 Or TransactionType = 8 Or TransactionType = 9 Or TransactionType = 10 Or TransactionType = 11 Then  '1 Receipt 2 Issue 4 Requisition 5 Work Order (Ajay TransactionType = 9 29-Nov-2022)
            mFetchTransactionToReOpen = FetchTransactionToReOpen.GetTransaction(TransactionType, TransactionText, TransactionNo)
            dgReceiptList.DataSource = mFetchTransactionToReOpen
            dgReceiptList.DataBind()
        ElseIf TransactionType = 3 Then 'Order
            mFetchTransactionToReOpen = FetchTransactionToReOpen.GetTransaction(TransactionType, TransactionText, TransactionNo, txtAmend.Text)
            dgReceiptList.DataSource = mFetchTransactionToReOpen
            dgReceiptList.DataBind()
            'ElseIf TransactionType = 4 Then 'Added By Vikrant On 19-Dec-2016 For ALL19122016
            '    mFetchTransactionToReOpen = FetchTransactionToReOpen.GetTransaction(TransactionType, TransactionText, TransactionNo)
            '    dgReceiptList.DataSource = mFetchTransactionToReOpen
            '    dgReceiptList.DataBind()
            '    'End
        End If
        TransTypeName()
        lblResultReceipt.Text = "List of " + mTransTypeName + " as per criteria : " & mFetchTransactionToReOpen.Count.ToString & " Record(s) found."
        Session("mFetchTransactionToReOpen") = mFetchTransactionToReOpen
    End Sub
    Private Sub ControlVisibility()
        lblResultReceipt.Visible = IIf(dgReceiptList.Rows.Count > 0, True, False)
        If cmbTransactionType.SelectedIndex = 1 Then
            dgReceiptList.Columns(5).Visible = False
            dgReceiptList.Columns(7).Visible = False
            dgReceiptList.Columns(8).Visible = False
            dgReceiptList.Columns(3).Visible = True
            dgReceiptList.Columns(4).Visible = True
            dgReceiptList.Columns(9).Visible = False
            'Added By Vikrant On 19-Dec-2016 For ALL19122016
            dgReceiptList.Columns(10).Visible = False
            dgReceiptList.Columns(11).Visible = False
            dgReceiptList.Columns(12).Visible = True
            dgReceiptList.Columns(13).Visible = True
            'End
            dgReceiptList.Columns(14).Visible = False
            dgReceiptList.Columns(15).Visible = False
            dgReceiptList.Columns(16).Visible = False
            dgReceiptList.Columns(17).Visible = False
            dgReceiptList.Columns(18).Visible = False
            dgReceiptList.Columns(6).Visible = False 'Added By Vikrant On 15-May-2019 For ALL15052019
        ElseIf cmbTransactionType.SelectedIndex = 2 Then
            dgReceiptList.Columns(5).Visible = True
            dgReceiptList.Columns(7).Visible = True
            dgReceiptList.Columns(8).Visible = True
            dgReceiptList.Columns(3).Visible = False
            dgReceiptList.Columns(4).Visible = False
            dgReceiptList.Columns(9).Visible = False
            'Added By Vikrant On 19-Dec-2016 For ALL19122016
            dgReceiptList.Columns(10).Visible = False
            dgReceiptList.Columns(11).Visible = False
            dgReceiptList.Columns(12).Visible = True
            dgReceiptList.Columns(13).Visible = True
            'End
            dgReceiptList.Columns(14).Visible = False
            dgReceiptList.Columns(15).Visible = False
            dgReceiptList.Columns(16).Visible = False
            dgReceiptList.Columns(17).Visible = False
            dgReceiptList.Columns(18).Visible = False
            dgReceiptList.Columns(6).Visible = False 'Added By Vikrant On 15-May-2019 For ALL15052019
        ElseIf cmbTransactionType.SelectedIndex = 3 Then
            dgReceiptList.Columns(5).Visible = False
            dgReceiptList.Columns(7).Visible = False
            dgReceiptList.Columns(8).Visible = False
            dgReceiptList.Columns(3).Visible = False
            dgReceiptList.Columns(4).Visible = False
            dgReceiptList.Columns(9).Visible = True
            'Added By Vikrant On 19-Dec-2016 For ALL19122016
            dgReceiptList.Columns(10).Visible = False
            dgReceiptList.Columns(11).Visible = False
            dgReceiptList.Columns(12).Visible = True
            dgReceiptList.Columns(13).Visible = True
            'End
            dgReceiptList.Columns(14).Visible = False
            dgReceiptList.Columns(15).Visible = False
            dgReceiptList.Columns(16).Visible = False
            dgReceiptList.Columns(17).Visible = False
            dgReceiptList.Columns(18).Visible = False
            dgReceiptList.Columns(6).Visible = False 'Added By Vikrant On 15-May-2019 For ALL15052019
            'Added By Vikrant On 19-Dec-2016 For ALL19122016
        ElseIf cmbTransactionType.SelectedIndex = 4 Then
            dgReceiptList.Columns(5).Visible = False
            dgReceiptList.Columns(7).Visible = False
            dgReceiptList.Columns(8).Visible = False
            dgReceiptList.Columns(3).Visible = False
            dgReceiptList.Columns(4).Visible = False
            dgReceiptList.Columns(9).Visible = False
            dgReceiptList.Columns(10).Visible = True
            dgReceiptList.Columns(11).Visible = True
            dgReceiptList.Columns(12).Visible = False
            dgReceiptList.Columns(13).Visible = False
            dgReceiptList.Columns(14).Visible = False
            dgReceiptList.Columns(15).Visible = False
            dgReceiptList.Columns(16).Visible = False
            dgReceiptList.Columns(17).Visible = False
            dgReceiptList.Columns(18).Visible = False
            dgReceiptList.Columns(6).Visible = False 'Added By Vikrant On 15-May-2019 For ALL15052019
        ElseIf cmbTransactionType.SelectedIndex = 5 Then
            dgReceiptList.Columns(5).Visible = False
            dgReceiptList.Columns(7).Visible = False
            dgReceiptList.Columns(8).Visible = False
            dgReceiptList.Columns(3).Visible = False
            dgReceiptList.Columns(4).Visible = False
            dgReceiptList.Columns(9).Visible = False
            dgReceiptList.Columns(10).Visible = False
            dgReceiptList.Columns(11).Visible = False
            dgReceiptList.Columns(12).Visible = False
            dgReceiptList.Columns(13).Visible = False
            dgReceiptList.Columns(14).Visible = True
            dgReceiptList.Columns(15).Visible = True
            dgReceiptList.Columns(16).Visible = True
            dgReceiptList.Columns(17).Visible = True
            dgReceiptList.Columns(18).Visible = True
            dgReceiptList.Columns(6).Visible = False 'Added By Vikrant On 15-May-2019 For ALL15052019
            'Added By Vikrant On 15-May-2019 For ALL15052019
        ElseIf cmbTransactionType.SelectedIndex = 6 Then
            dgReceiptList.Columns(5).Visible = False
            dgReceiptList.Columns(7).Visible = False
            dgReceiptList.Columns(8).Visible = False
            dgReceiptList.Columns(3).Visible = False
            dgReceiptList.Columns(4).Visible = False
            dgReceiptList.Columns(9).Visible = True
            dgReceiptList.Columns(10).Visible = False
            dgReceiptList.Columns(11).Visible = False
            dgReceiptList.Columns(12).Visible = True
            dgReceiptList.Columns(13).Visible = True
            dgReceiptList.Columns(14).Visible = False
            dgReceiptList.Columns(15).Visible = False
            dgReceiptList.Columns(16).Visible = False
            dgReceiptList.Columns(17).Visible = False
            dgReceiptList.Columns(18).Visible = False
        ElseIf cmbTransactionType.SelectedIndex = 7 Or cmbTransactionType.SelectedIndex = 8 Then
            dgReceiptList.Columns(5).Visible = False
            dgReceiptList.Columns(7).Visible = False
            dgReceiptList.Columns(8).Visible = False
            dgReceiptList.Columns(3).Visible = False
            dgReceiptList.Columns(4).Visible = False
            dgReceiptList.Columns(9).Visible = True
            dgReceiptList.Columns(10).Visible = False
            dgReceiptList.Columns(11).Visible = False
            dgReceiptList.Columns(12).Visible = True
            dgReceiptList.Columns(13).Visible = True
            dgReceiptList.Columns(14).Visible = False
            dgReceiptList.Columns(15).Visible = False
            dgReceiptList.Columns(16).Visible = False
            dgReceiptList.Columns(17).Visible = False
            dgReceiptList.Columns(18).Visible = False
            dgReceiptList.Columns(6).Visible = False
            'Ajay 29-Nov-2022
        ElseIf cmbTransactionType.SelectedIndex = 8 Or cmbTransactionType.SelectedIndex = 9 Or cmbTransactionType.SelectedIndex = 10 Or cmbTransactionType.SelectedIndex = 11 Then
            dgReceiptList.Columns(5).Visible = False
            dgReceiptList.Columns(7).Visible = False
            dgReceiptList.Columns(8).Visible = False
            dgReceiptList.Columns(3).Visible = False
            dgReceiptList.Columns(4).Visible = False
            dgReceiptList.Columns(9).Visible = False
            dgReceiptList.Columns(10).Visible = False
            dgReceiptList.Columns(11).Visible = False
            If cmbTransactionType.SelectedIndex = 10 Or cmbTransactionType.SelectedIndex = 11 Then
                dgReceiptList.Columns(12).Visible = False  'Created By
                dgReceiptList.Columns(13).Visible = False  'Authorized By
            Else
                dgReceiptList.Columns(12).Visible = True 'Created By
                dgReceiptList.Columns(13).Visible = True 'Authorized By
            End If
            dgReceiptList.Columns(14).Visible = False
            dgReceiptList.Columns(15).Visible = False
            dgReceiptList.Columns(16).Visible = False
            dgReceiptList.Columns(17).Visible = False
            dgReceiptList.Columns(18).Visible = False
            dgReceiptList.Columns(6).Visible = False
            '--------------------------------------
        End If

        lblAmend.Visible = (cmbTransactionType.SelectedIndex = 3)
        txtAmend.Visible = (cmbTransactionType.SelectedIndex = 3)
    End Sub
    Private Sub addAttributes()
        txtTransactionNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtTransactionNo').value,event)")
    End Sub
    Private Sub SetComBox() 'Added by Prashant 18-Nov-2019 ALL18112019

        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("(SELECT)", "0"))
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Receipt", "1"))
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Issue", "2"))
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Order", "3"))
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition", "4"))
        If AppSettings("ShowNewWOFlow") = "True" Then 'If this is true then do not show Work Order for selection
            'Do nothing
        Else
            cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Work Order", "5"))
        End If
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Purchase Invoice", "6"))
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Purchase Enquiry", "7"))
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Purchase Quotation", "8"))
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Export Invoice", "9")) 'Ajay 29-Nov-2022
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Audit", "10")) 'Added By Prashant on 11-Sep-2023 as per Kasas mail
        cmbTransactionType.Items.Add(New System.Web.UI.WebControls.ListItem("Discrepancy", "11")) 'Added By Prashant on 9-Jan-2025 for Afcom
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            SetComBox()
            If cmbTransactionType.Enabled = True Then
                cmbTransactionType.Focus()
            End If
            DataBind()
        End If
        ControlVisibility()
    End Sub
    Private Sub TransTypeName()
        If cmbTransactionType.SelectedIndex = 1 Then
            mTransTypeName = "Receipt"
        ElseIf cmbTransactionType.SelectedIndex = 2 Then
            mTransTypeName = "Issue"
        ElseIf cmbTransactionType.SelectedIndex = 3 Then
            mTransTypeName = "Order"
        ElseIf cmbTransactionType.SelectedIndex = 4 Then
            mTransTypeName = "Requisition"
        ElseIf cmbTransactionType.SelectedIndex = 5 Then
            mTransTypeName = "Work Order"
        ElseIf cmbTransactionType.SelectedIndex = 6 Then
            mTransTypeName = "Invoice"
        ElseIf cmbTransactionType.SelectedIndex = 7 Then
            mTransTypeName = "Purchase Enquiry"
        ElseIf cmbTransactionType.SelectedIndex = 8 Then
            mTransTypeName = "Purchase Quotation"
        ElseIf cmbTransactionType.SelectedIndex = 9 Then
            mTransTypeName = "Export Invoice"
        ElseIf cmbTransactionType.SelectedIndex = 10 Then
            mTransTypeName = "Audit"
        ElseIf cmbTransactionType.SelectedIndex = 11 Then
            mTransTypeName = "Discrepancy"
        End If
    End Sub

    Private Sub dgReceiptList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptList.RowCommand
        Select Case e.CommandName
            Case "ReOpen"
                Dim mTransactionID As Guid = New Guid(dgReceiptList.DataKeys(CInt(e.CommandArgument)).Values("TransactionID").ToString)
                Session("mTransactionID") = mTransactionID
                TransTypeName()
                If AppSettings("LockBackDatedTransaction") = "True" Then
                    If mTransTypeName = "Issue" Then
                        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
                        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
                        If (CDate(dgReceiptList.DataKeys(CInt(e.CommandArgument)).Values("TransactionDateFormatted").ToString) >= FirstDayofLastMonth) Then
                            If (CDate(dgReceiptList.DataKeys(CInt(e.CommandArgument)).Values("TransactionDateFormatted").ToString) < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                                MSGBoxCtrl.Show("Alert!", "Previous Months transactions can only be open until " & DateSerial(Year(CDate(dgReceiptList.DataKeys(CInt(e.CommandArgument)).Values("TransactionDateFormatted").ToString).AddMonths(1)), Month(CDate(dgReceiptList.DataKeys(CInt(e.CommandArgument)).Values("TransactionDateFormatted").ToString).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Else
                            MSGBoxCtrl.Show("Alert!", "Previous Months transactions can only be open until " & DateSerial(Year(CDate(dgReceiptList.DataKeys(CInt(e.CommandArgument)).Values("TransactionDateFormatted").ToString).AddMonths(1)), Month(CDate(dgReceiptList.DataKeys(CInt(e.CommandArgument)).Values("TransactionDateFormatted").ToString).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If
                End If
                MSGBoxCtrl.Show("Alert", mTransTypeName + " Status will get changed<BR>", "Do you want to continue??", MsgBoxStyle.YesNo, "ReOpen")
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow(cmbTransactionType.SelectedIndex, txtTransactionText.Text.Trim, CInt(txtTransactionNo.Text))
        ControlVisibility()
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Response.Redirect("index.aspx")
    End Sub
    Private Sub cmbTransactionType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTransactionType.SelectedIndexChanged
        If cmbTransactionType.SelectedIndex = 10 Then
            txtTransactionNo.Text = "0"
            txtTransactionText.Text = ""
            txtTransactionNo.Enabled = False
        Else
            txtTransactionNo.Enabled = True
            txtTransactionNo.Text = ""
            txtTransactionText.Text = ""
        End If
        upnlSearch.Update()
        dgReceiptList.DataSource = Nothing
        dgReceiptList.DataBind()
        TransTypeName()
        lblResultReceipt.Text = "List of " + mTransTypeName + " as per criteria : 0 Record(s) found."
        upnlGridView.Update()
    End Sub
#End Region

End Class