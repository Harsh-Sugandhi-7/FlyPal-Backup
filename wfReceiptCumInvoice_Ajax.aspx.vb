Imports System.Linq
Imports System.Text
Imports System.Security.Cryptography
Imports System.Collections.Generic
Imports System.Web.Script.Serialization


Public Class wfReceiptCumInvoice_Ajax
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
    Private Enum RequstFor
        Supplier = 0
        Customer = 1
    End Enum
#End Region

#Region " Variable Declaration "
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
    Public mTypeList As TypeListForReceipt
    Public mVendorList As VendorList
    Public mMachineNameValueList As MachineNameValueList
    Public mStoreList As StoreList
    Public mCurrencyList As CurrencyList
    Public mTransTypeID As Trans   'Added Code
    Public mModuleName As String
    Public mWorkShopList As WorkShopList

    Public mnWOListForCombo As nWOListForCombo  '------Added By Utkarsh 10-Dec-2010

    Dim EventLogID As Guid 'Added By Utkarsh On 20-Jul-2011 For All19072011
    Dim mRCIDetail As String 'Added By Utkarsh On 20-Jul-2011 For All19072011
    Dim mMachineID As Guid 'Added by Vikrant on 7.3.12 FORALL03052012

    Dim mOtherCharge As OtherCharge   'Added By Prashant 26-Jul-2012
    Dim mOtherChargeListByInvoiceID As OtherChargeListByInvoiceID 'Added By Prashant 26-Jul-2012
    Dim mOpenFrom As String 'Added By Prashant 3-Apr-2014 ALL03042014
    Dim ExtraMessage As String = ""
    'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mFileAttachments As FileAttachments
    'End
    Dim ItemsComply As StringBuilder = New StringBuilder
    Dim ConditionalItemsComply As StringBuilder = New StringBuilder
    Dim mEmployeeEmailID As EmployeeEmailID
    Dim mEmployeeEmailIDs As String = String.Empty
    Dim mUser As User

    Public mGSTPercentage As GSTPercentage
    Public mVendor As Vendor
    Public mIsAttachmentNotSave As Boolean = True
    Dim email As Thread
    Dim mTransactionList As TransactionList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    Dim ReceiptitemRemovedDetails As String = ""
	Dim mModuleList As ModuleList
	Public AttachmentHelper As New AttachmentHelper
	Private ReportHelper As New ReportHelper


#End Region

#Region " Business Methods "
	Private Sub GetSession()
        mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
        mVendorList = CType(Session("mVendorList"), VendorList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        mCurrencyList = CType(Session("mCurrencyList"), CurrencyList)
        mTypeList = CType(Session("mTypeList"), TypeListForReceipt)
        mTransTypeID = CType(Session("mTransTypeID"), Integer)
        mModuleName = Session("mModuleName")
        mWorkShopList = CType(Session("mWorkShopList"), WorkShopList)
        mnWOListForCombo = CType(Session("mnWOListForCombo"), nWOListForCombo) '--Added By Utkarsh 10-Dec-2010
        mMachineID = Session("MachineID") 'Added by Vikrant on 7.3.12 FORALL03052012
        'Added By Vikrant On 01-Dec-2014
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
        mTransactionList = Session("mTransactionList")  'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        mModuleList = Session("mModuleList")
    End Sub
    Private Sub SetSession()
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Session("mVendorList") = mVendorList
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mStoreList") = mStoreList
        Session("mCurrencyList") = mCurrencyList
        Session("mTypeList") = mTypeList
        Session("mModuleName") = mModuleName
        Session("mWorkShopList") = mWorkShopList
        Session("mnWOListForCombo") = mnWOListForCombo   ' ------Added By Utkarsh 10-Dec-2010
        Session("MachineID") = mMachineID 'Added by Vikrant on 7.3.12 FORALL03052012
        'Added By Vikrant On 01-Dec-2014
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mReceiptCumInvoice")
        Session.Remove("mVendorList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mStoreList")
        Session.Remove("mCurrencyList")
        Session.Remove("mTypeList")
        Session.Remove("mStatusist")
        Session.Remove("mWorkShopList")
        Session.Remove("mnWOListForCombo") ' ------Added By Utkarsh 10-Dec-2010
        'Added By Vikrant On 01-Dec-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
    End Sub
    Private Sub SetPage()
        If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
            lblTitle.Text = "Goods Receipt Details"
            lblReceiptCumInvoiceDetails.Text = "Goods Receipt Details"
        Else
            lblTitle.Text = "Goods Receipt Details"
        End If
    End Sub
    Private Sub Disable()
        txtReceiptCumInvoiceDate.Enabled = False
    End Sub
    Private Sub Enable()
        txtReceiptCumInvoiceDate.Enabled = True
    End Sub
    Private Function IsRecQtyExceedsOrderQty() As Boolean
        ' Group by OrderItemID once
        Dim groupedItems = From item In mReceiptCumInvoice.ReceiptCumInvoiceItems
                           Group item By item.OrderItemID Into Group
                           Select New With {
                           .OrderItemID = OrderItemID,
                           .TotalDisplayQty = Group.Sum(Function(x) x.DisplayQty),
                           .FirstItemID = Group.First().ID, 'use any ID from group
                           .Items = Group.ToList()
                       }
        Dim variable
        For Each variable In groupedItems
            ' Get order item detail
            Dim mOrderItemDetailForReceipt As OrderItemDetailForReceipt =
            OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(variable.OrderItemID)

			Dim TotalReceiptCount As Decimal = Order.GetTotalReceiptCountAgainstOrderItem(variable.OrderItemID,
																						  SkipRecItemID:=variable.FirstItemID.ToString,
																						  SkipReceiptID:=mReceiptCumInvoice.ID.ToString)

			' Get already saved receipt qty (excluding current batch)
			Dim TotalRecQty As Decimal = 0

			If TotalReceiptCount = 0 Then
				TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(variable.OrderItemID,
																	   SkipRecItemID:=variable.FirstItemID.ToString,
																	   SkipReceiptID:=mReceiptCumInvoice.ID.ToString)
			Else
				TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(variable.OrderItemID,
																	   SkipRecItemID:=variable.FirstItemID.ToString)
			End If

            If variable.TotalDisplayQty > mOrderItemDetailForReceipt.Qty Then
                ' case 1: current receipt exceeds order qty
                Return True
            ElseIf (TotalRecQty + variable.TotalDisplayQty) > mOrderItemDetailForReceipt.Qty Then
                ' case 2: total receipts exceed order qty
                Return True
            Else
                ' case 3: within limit, safe to save
                Return False
            End If
        Next

        Return False
    End Function
    'ALL30082018
    'Private Function IsRecQtyExceedsOrderQty() As Boolean
    '	'For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1
    '	'    'Dim mOrder As OrderItem = Order.GetOrder( mReceiptCumInvoice.ReceiptCumInvoiceItems(i).OrderItemID)
    '	'    Dim mOrderItemDetailForReceipt As OrderItemDetailForReceipt = OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(mReceiptCumInvoice.ReceiptCumInvoiceItems(i).OrderItemID)
    '	'    Dim TotalRecQty As Decimal
    '	'    TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(mReceiptCumInvoice.ReceiptCumInvoiceItems(i).OrderItemID, mReceiptCumInvoice.ReceiptCumInvoiceItems(i).ID.ToString)
    '	'    'Commented and Added By Prashant 5-Feb-2019 ALL04022019
    '	'    'If mReceiptCumInvoice.ReceiptCumInvoiceItems(i).Qty > mOrderItemDetailForReceipt.Qty - TotalRecQty And Not mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IsSerialized Then
    '	'    'If mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayQty > mOrderItemDetailForReceipt.Qty - TotalRecQty And Not mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IsSerialized Then
    '	'    If mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayQty > CDec(Format(mOrderItemDetailForReceipt.Qty - TotalRecQty, "##0.00##")) And Not mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IsSerialized Then
    '	'        Return True
    '	'    End If
    '	'Next
    '	'Return False

    '	Dim groupedItems = From item In mReceiptCumInvoice.ReceiptCumInvoiceItems
    '					   Group item By item.OrderItemID Into Group
    '					   Select New With {
    '									   .OrderItemID = OrderItemID,
    '									   .TotalDisplayQty = Group.Sum(Function(x) x.DisplayQty),
    '									   .Items = Group.ToList()
    '									   }
    '	Dim mOrder As Order
    '	Dim variable
    '	For Each variable In groupedItems
    '		Dim TotalRecQty As Decimal
    '		Dim mOrderItemDetailForReceipt As OrderItemDetailForReceipt = OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(variable.OrderItemID)
    '		mOrder = Order.GetOrder(mOrderItemDetailForReceipt.OrderID)
    '		Dim receiptitemchildcol
    '           For Each receiptitemchildcol In variable.Items
    '               TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(variable.OrderItemID, receiptitemchildcol.ID.ToString)
    '           Next

    '           If variable.TotalDisplayQty > CDec(Format(mOrderItemDetailForReceipt.Qty - TotalRecQty, "##0.00##")) And Not mOrder.OrderItems(variable.OrderItemID).IsSerializedPart Then
    '                   Return True
    '               End If
    '           'If variable.TotalDisplayQty > CDec(Format(mOrder.OrderItems(variable.OrderItemID).Qty - TotalRecQty, "##0.00##")) And Not mOrder.OrderItems(variable.OrderItemID).IsSerializedPart Then
    '           '    Return True
    '           'End If
    '       Next
    '	Return False
    'End Function
    'End
    'Private Function IsRecQtyExceedsOrderQty() As Boolean
    '    For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1
    '        'Dim mOrder As OrderItem = Order.GetOrder( mReceiptCumInvoice.ReceiptCumInvoiceItems(i).OrderItemID)
    '        Dim mOrderItemDetailForReceipt As OrderItemDetailForReceipt = OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(mReceiptCumInvoice.ReceiptCumInvoiceItems(i).OrderItemID)
    '        Dim TotalRecQty As Decimal
    '        TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(mReceiptCumInvoice.ReceiptCumInvoiceItems(i).OrderItemID, mReceiptCumInvoice.ReceiptCumInvoiceItems(i).ID.ToString)
    '        'Commented and Added By Prashant 5-Feb-2019 ALL04022019
    '        'If mReceiptCumInvoice.ReceiptCumInvoiceItems(i).Qty > mOrderItemDetailForReceipt.Qty - TotalRecQty And Not mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IsSerialized Then
    '        'If mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayQty > mOrderItemDetailForReceipt.Qty - TotalRecQty And Not mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IsSerialized Then
    '        If mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayQty > CDec(Format(mOrderItemDetailForReceipt.Qty - TotalRecQty, "##0.00##")) And Not mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IsSerialized Then
    '            Return True
    '        End If
    '    Next
    '    Return False
    'End Function
    'Private Sub Save()
    '    'Authentication
    '    If Not mReceiptCumInvoice.RecCumInvDate Is System.DBNull.Value Then
    '        Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
    '        If mCheck.WebAuthentication = True Then
    '            Dim mDays As Integer = 0
    '            mDays = mCheck.Number("Days")
    '            Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
    '            If DateDiff(DateInterval.Day, CDate(mReceiptCumInvoice.RecCumInvDate), maxAllowableDate) < 0 Then
    '                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            End If

    '            'Added By Vikrant On 05-Nov-2015 For All05112015
    '            If mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 13 Or mReceiptCumInvoice.TransTypeID = 66 Then 'Aircraft Related Transactions
    '                If mMachineNameValueList(mReceiptCumInvoice.AircraftID).IsReadOnly Then
    '                    MSGBoxCtrl.show("Alert!", "<b>" & cmbAircraft.SelectedItem.ToString & "</b> is marked <b>ReadOnly</b>", "You cannot save Goods Receipt.", MsgBoxStyle.OkOnly, "")
    '                    Exit Sub
    '                End If
    '            End If
    '            'End
    '            'Added By Vikrant On 24-July-2014 For BA24072014
    '            If AppSettings("LockBackDatedTransaction") = "True" And (mReceiptCumInvoice.TransTypeID <> 9 And mReceiptCumInvoice.TransTypeID <> 66) Then
    '                If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
    '                    'Do nothing
    '                Else
    '                    If mReceiptCumInvoice.StatusID <> 2 Then
    '                        If CheckDateForTransactionLock(mReceiptCumInvoice.RecCumInvDate) Then
    '                            MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), Month(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
    '                            Exit Sub
    '                        End If
    '                    End If
    '                End If
    '            End If
    '            'End
    '        End If
    '    End If
    '    Dim ReceiptCumInvoiceClone As ReceiptCumInvoice
    '    ReceiptCumInvoiceClone = mReceiptCumInvoice.Clone
    '    Try

    '        'check whether min. one item & charge is present while saving
    '        If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0 Then
    '            'save the object
    '            SetObject()
    '            If mReceiptCumInvoice.IsValid Then
    '                Dim i As Integer
    '                While i < mReceiptCumInvoice.ReceiptCumInvoiceItems.Count
    '                    If mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).IsSerialized = True Then
    '                        If mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateSerialNo() = True Then
    '                            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Serial number already exist. You can not add Duplicate.", MsgBoxStyle.OkOnly, "Status")
    '                            Exit Sub
    '                        End If

    '                        If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).PrimaryCategoryID = 2 And AppSettings("CodeNo") = "True") Then
    '                            If (mReceiptCumInvoice.TransTypeID = 7) Then
    '                                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(1) = True) Then 'Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(2) = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(3) = True) Then '1 Duplication checking with CodeNo only
    '                                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. entered for item  " + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).ItemName + " (" + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).SerialNo + ") " + " already exist.  Please enter another Code No.", MsgBoxStyle.OkOnly, "Status")
    '                                    Exit Sub
    '                                End If
    '                            Else
    '                                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(2) = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(3) = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(4) = True) Then '2 Duplication checking with CodeNo,ItemID,Serail No.
    '                                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. entered for item  " + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).ItemName + " (" + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).SerialNo + ") " + " already exist.  Please enter another Code No.", MsgBoxStyle.OkOnly, "Status")
    '                                    Exit Sub
    '                                Else
    '                                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. already exist for item. " + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).ItemName + " You can not add Duplicate Code No.", MsgBoxStyle.OkOnly, "")
    '                                    'Exit Sub
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                    i = i + 1
    '                End While
    '                mReceiptCumInvoice.ApplyEdit()
    '                Dim mReceiptCumInvoiceCharge As InvoiceCharge
    '                For Each mReceiptCumInvoiceCharge In mReceiptCumInvoice.Invoice.InvoiceCharges
    '                    If (mReceiptCumInvoiceCharge.Sign <> 1 And mReceiptCumInvoiceCharge.CChargeAmount <= 0) Or (Not (mReceiptCumInvoiceCharge.IsValid)) Then
    '                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Other Charge(s) are not allowed if Goods Receipt Amount Is Zero ", MsgBoxStyle.OkOnly, "")
    '                        mReceiptCumInvoice.CancelEdit()
    '                        Exit Sub
    '                    End If
    '                Next
    '                If chkIsRoundOff.Checked = True Then
    '                    mReceiptCumInvoice.Invoice.RoundCGrandTotal()
    '                End If
    '                'Added by Utkarsh on 19-Nov-2013 FOr TransTextSeries 
    '                'Check if ReceiptCumInvoiceText is blank then call TransTextSeries UI

    '                If (mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.InvText = "") Then

    '                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mReceiptCumInvoice.TransTypeID, mReceiptCumInvoice.RecCumInvDateFormatted)

    '                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID).TransText = "")) Then

    '                        Dim str = "<script language='javascript'>openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx');</script>"

    '                        Session("BackPagestr_ForTransSeries") = str

    '                        Session("TransName_ForTransSeries") = "Receipt Cum Invoice"
    '                        Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
    '                        Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted
    '                        MSGBoxCtrl.show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RCITransTextSeriesAlert")
    '                        Exit Sub
    '                    Else
    '                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

    '                        If mAutoRenewTransTextSeries.IsRenewed Then
    '                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID)
    '                                mReceiptCumInvoice.InvText = .TransText
    '                                mReceiptCumInvoice.InvNo = .StartingTransNo
    '                            End With
    '                        Else
    '                            Dim str = "<script language='javascript'>openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx');</script>"

    '                            Session("BackPagestr_ForTransSeries") = str

    '                            Session("TransName_ForTransSeries") = "Receipt Cum Invoice"
    '                            Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
    '                            Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted
    '                            MSGBoxCtrl.show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RCITransTextSeriesAlert")
    '                            Exit Sub
    '                        End If
    '                    End If

    '                End If
    '                'End
    '                'Added on 08-May-2018
    '                mReceiptCumInvoice.IsForAttachmentAfterAuthorized = True
    '                'ALL30082018
    '                If mReceiptCumInvoice.TransTypeID = Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder Then
    '                    If IsRecQtyExceedsOrderQty() Then
    '                        MSGBoxCtrl.show("Pending Quantity Alert!", "Receipt Qty. is greater than Order Qty.<BR>If you click Yes, existing Order alongwith Order Amount will get updated.<BR><BR>Do you want to continue?", "", MsgBoxStyle.YesNo, "ExcessQtyHandle")
    '                        Exit Sub
    '                    End If

    '                End If
    '                'End
    '                mReceiptCumInvoice.Save()
    '                If mReceiptCumInvoice.IsAttachmentAdded Then
    '                    If mReceiptCumInvoice.FileAttachments(0).Size > 0 Then
    '                        ImageButton1.Visible = True
    '                    End If

    '                End If

    '                'Added By Utkarsh On 20-Jul-2011 For All19072011

    '                Dim mFrom As String
    '                Select Case mReceiptCumInvoice.FromTypeID
    '                    Case 14  'Vendor
    '                        mFrom = cmbVendor.SelectedItem.ToString
    '                    Case 2   'Aircraft
    '                        mFrom = cmbAircraft.SelectedItem.ToString
    '                    Case 8   'Store
    '                        mFrom = cmbStore.SelectedItem.ToString
    '                        'btnAddItem.Enabled = True
    '                    Case 16  'WorkShop
    '                        mFrom = cmbWorkShop.SelectedItem.ToString
    '                    Case 17  'WorkOrder                                
    '                        mFrom = cmbWorkOrder.SelectedItem.ToString
    '                End Select
    '                'End
    '                'Changed By Utkarsh On 20-Jul-2011 For All19072011
    '                'mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mFrom
    '                'Added by Prashant  16-Jul-2013 'ALL15072013
    '                If Session("Note") <> "" Then
    '                    mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mFrom + " Note:- " + Session("Note")
    '                    Session.Remove("Note")
    '                Else
    '                    mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mFrom
    '                End If
    '                '-------------------------------------------

    '                Select Case mReceiptCumInvoice.StatusID
    '                    Case 1
    '                        MarkLog(Util.Action.Save, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
    '                    Case 2
    '                        If (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10) Then
    '                            SendMailIfAlternateReceive()
    '                        End If
    '                        SendReqPartsMail() 'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
    '                        SendMail()
    '                        'Added by Prashant on 26-Oct-2021. To check is this receipt against requisition, checked employee id, if employee id is not blank notification will push in table
    '                        If mReceiptCumInvoice.ReceiptCumInvoiceItems(0).ReqEmployeeID.Equals(Guid.Empty) Then
    '                            'Do not push Notification 
    '                        Else
    '                            SendPUSHNotification(mReceiptCumInvoice)
    '                        End If
    '                        'End of Added by Prashant on 26-Oct-2021
    '                        MarkLog(Util.Action.Authorize, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
    '                    Case 3
    '                        MarkLog(Util.Action.Amend, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
    '                    Case 4
    '                        MarkLog(Util.Action.Cancel, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
    '                End Select
    '                'End
    '                mReceiptCumInvoice.MarkClean()

    '                mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptCumInvoice.Receipt.ID, mReceiptCumInvoice.InvoiceID)
    '                Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '                DataFieldBind()
    '                ControlVisibility()
    '                SetChargeGrid()
    '                SetControlStatus(mReceiptCumInvoice.StatusID)
    '                upnlTitle.Update()
    '                upnlStatusName.Update()
    '                upnlReceiptCumInvoiceDetails.Update()
    '                upnlReceivedFrom.Update()
    '                upnlReceiptCumInvItems.Update()
    '                upnlRCICharges.Update()
    '                upnlOtherDetails.Update()
    '                upnlButtons.Update()

    '                If mReceiptCumInvoice.StatusID = 2 Then
    '                    MSGBoxCtrl.show(MSGBox.Message_title.AuthorizedSuccessFully, MSGBox.Message_text.AuthorizedSuccessFully, "", MsgBoxStyle.OkOnly, "")
    '                ElseIf mReceiptCumInvoice.StatusID = 4 Then
    '                    MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")
    '                Else
    '                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
    '                End If
    '            Else
    '                'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Enter valid Other Charge.", MsgBoxStyle.OkOnly, "")
    '                Dim mRule As String = ""
    '                If mReceiptCumInvoice.GetBrokenRulesCollection.Count > 0 Then
    '                    mRule = mReceiptCumInvoice.GetBrokenRulesCollection(0).Description
    '                ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GetBrokenRulesCollection.Count > 0 Then
    '                    mRule = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GetBrokenRulesCollection(0).Description
    '                End If
    '                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, mRule, MsgBoxStyle.OkOnly, "")
    '                mRule = ""
    '                mReceiptCumInvoice = ReceiptCumInvoiceClone
    '                SetObject()
    '                Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '                DataFieldBind()
    '                Exit Sub
    '            End If
    '        Else
    '            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Goods Receipt can not be saved without Part.", MsgBoxStyle.OkOnly, "")
    '            mReceiptCumInvoice = ReceiptCumInvoiceClone
    '            SetObject()
    '            Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '            DataFieldBind()
    '            Exit Sub
    '        End If
    '    Catch ex As SqlException
    '        Session("ReceiptCumInvoiceClone") = ReceiptCumInvoiceClone
    '        If ex.Number = 8114 Or ex.Number = 8115 Then
    '            MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
    '            Exit Sub
    '        ElseIf ex.Number = 8145 Then
    '            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
    '            Exit Sub
    '        ElseIf ex.Number = 2627 Then
    '            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
    '            Exit Sub
    '        ElseIf ex.Number = 547 Then
    '            If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
    '                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
    '                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br></br><b>Please amend Purchase Order for Receipt of excess quantity.</b>", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
    '                MSGBoxCtrl.show("Alert!", "Other Charge Deleted ! ", "Other charge Not Available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            Else
    '                MSGBoxCtrl.show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            End If
    '        End If
    '    Catch ex1 As Exception
    '        If InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex1.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
    '            MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "Status")
    '            mReceiptCumInvoice = ReceiptCumInvoiceClone
    '            SetObject()
    '            Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '            DataFieldBind()
    '            Exit Sub
    '        ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
    '            MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br><b>Please amend Purchase Order Quantity & make Goods Receipt again.</b>", MsgBoxStyle.OkOnly, "")
    '        Else
    '            MSGBoxCtrl.show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "Status")
    '            mReceiptCumInvoice = ReceiptCumInvoiceClone
    '            SetObject()
    '            Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '            DataFieldBind()
    '            Exit Sub
    '        End If
    '        mReceiptCumInvoice = ReceiptCumInvoiceClone
    '        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '    Finally
    '        ReceiptCumInvoiceClone = Nothing
    '    End Try
    'End Sub
    Public Function Save() As Boolean
        'Authentication
        If Not mReceiptCumInvoice.RecCumInvDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mReceiptCumInvoice.RecCumInvDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Function
                End If

                'Added By Vikrant On 05-Nov-2015 For All05112015
                If mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 13 Or mReceiptCumInvoice.TransTypeID = 66 Then 'Aircraft Related Transactions
                    If mMachineNameValueList(mReceiptCumInvoice.AircraftID).IsReadOnly Then
                        MSGBoxCtrl.Show("Alert!", "<b>" & cmbAircraft.SelectedItem.ToString & "</b> is marked <b>ReadOnly</b>", "You cannot save Goods Receipt.", MsgBoxStyle.OkOnly, "")
                        Exit Function
                    End If
                End If
                'End
                'Added By Vikrant On 24-July-2014 For BA24072014
                If AppSettings("LockBackDatedTransaction") = "True" And (mReceiptCumInvoice.TransTypeID <> 9 And mReceiptCumInvoice.TransTypeID <> 66) Then
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        If mReceiptCumInvoice.StatusID <> 2 Then
                            If CheckDateForTransactionLock(mReceiptCumInvoice.RecCumInvDate) Then
                                MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), Month(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                                Exit Function
                            End If
                        End If
                    End If
                End If
                'End
            End If
        End If
        Dim ReceiptCumInvoiceClone As ReceiptCumInvoice
        ReceiptCumInvoiceClone = mReceiptCumInvoice.Clone
        Try

            'check whether min. one item & charge is present while saving
            If Not mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0 Then
                'save the object
                SetObject()
                If mReceiptCumInvoice.IsValid Then
                    Dim i As Integer
                    While i < mReceiptCumInvoice.ReceiptCumInvoiceItems.Count
                        If mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).IsSerialized = True Then
                            If mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateSerialNo() = True Then
                                MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Serial number already exist. You can not add Duplicate.", MsgBoxStyle.OkOnly, "Status")
                                Exit Function
                            End If

                            If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).PrimaryCategoryID = 2 And AppSettings("CodeNo") = "True") Then
                                If (mReceiptCumInvoice.TransTypeID = 7) Then
                                    If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(1) = True) Then 'Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(2) = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(3) = True) Then '1 Duplication checking with CodeNo only
                                        MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. entered for item  " + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).ItemName + " (" + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).SerialNo + ") " + " already exist.  Please enter another Code No.", MsgBoxStyle.OkOnly, "Status")
                                        Exit Function
                                    End If
                                Else
                                    If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(2) = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(3) = True Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).DuplicateCodeNo(4) = True) Then '2 Duplication checking with CodeNo,ItemID,Serail No.
                                        MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. entered for item  " + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).ItemName + " (" + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).SerialNo + ") " + " already exist.  Please enter another Code No.", MsgBoxStyle.OkOnly, "Status")
                                        Exit Function
                                    Else
                                        'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. already exist for item. " + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).ItemName + " You can not add Duplicate Code No.", MsgBoxStyle.OkOnly, "")
                                        'Exit Function
                                    End If
                                End If
                            End If
                        End If
                        i = i + 1
                    End While
                    mReceiptCumInvoice.ApplyEdit()
                    Dim mReceiptCumInvoiceCharge As InvoiceCharge
                    For Each mReceiptCumInvoiceCharge In mReceiptCumInvoice.Invoice.InvoiceCharges
                        If (mReceiptCumInvoiceCharge.Sign <> 1 And mReceiptCumInvoiceCharge.CChargeAmount <= 0) Or (Not (mReceiptCumInvoiceCharge.IsValid)) Then
                            MSGBoxCtrl.Show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Other Charge(s) are not allowed if Goods Receipt Amount Is Zero ", MsgBoxStyle.OkOnly, "")
                            mReceiptCumInvoice.CancelEdit()
                            Exit Function
                        End If
                    Next
                    If chkIsRoundOff.Checked = True Then
                        mReceiptCumInvoice.Invoice.RoundCGrandTotal()
                    End If
                    'Added by Utkarsh on 19-Nov-2013 FOr TransTextSeries 
                    'Check if ReceiptCumInvoiceText is blank then call TransTextSeries UI

                    If (mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.InvText = "") Then

                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mReceiptCumInvoice.TransTypeID, mReceiptCumInvoice.RecCumInvDateFormatted)

                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID).TransText = "")) Then

                            Dim str = "<script language='javascript'>openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Receipt Cum Invoice"
                            Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted
                            MSGBoxCtrl.Show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RCITransTextSeriesAlert")
                            Exit Function
                        Else
                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then
                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID)
                                    mReceiptCumInvoice.InvText = .TransText
                                    mReceiptCumInvoice.InvNo = .StartingTransNo
                                End With
                            Else
                                Dim str = "<script language='javascript'>openledgersame('wfReceiptCumInvoice_Ajax.aspx?BackPage=index.aspx');</script>"

                                Session("BackPagestr_ForTransSeries") = str

                                Session("TransName_ForTransSeries") = "Receipt Cum Invoice"
                                Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                                Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted
                                MSGBoxCtrl.Show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RCITransTextSeriesAlert")
                                Exit Function
                            End If
                        End If

                    End If
                    'End
                    'Added on 08-May-2018
                    mReceiptCumInvoice.IsForAttachmentAfterAuthorized = True
                    'ALL30082018
                    If mReceiptCumInvoice.TransTypeID = Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder Then
                        If IsRecQtyExceedsOrderQty() Then
							MSGBoxCtrl.Show("Pending Quantity Alert!", "Receipt quantity exceeds Order quantity.<BR>If you proceed the Order details including the total Order amount will be updated.<BR><BR>Do you want to continue & update ?", "", MsgBoxStyle.YesNo, "ExcessQtyHandle")
							Return False
                            Exit Function
                        End If

                    End If
                    'End
                    mReceiptCumInvoice.Save()
                    'Comment Sankalp 26-09-25
                    'If mReceiptCumInvoice.IsAttachmentAdded Then
                    '    If mReceiptCumInvoice.FileAttachments(0).Size > 0 Then
                    '        ImageButton1.Visible = True
                    '    End If

                    'End If

                    'Added By Utkarsh On 20-Jul-2011 For All19072011

                    Dim mFrom As String
                    Select Case mReceiptCumInvoice.FromTypeID
                        Case 14  'Vendor
                            mFrom = cmbVendor.SelectedItem.ToString
                        Case 2   'Aircraft
                            mFrom = cmbAircraft.SelectedItem.ToString
                        Case 8   'Store
                            mFrom = cmbStore.SelectedItem.ToString
                            'btnAddItem.Enabled = True
                        Case 16  'WorkShop
                            mFrom = cmbWorkShop.SelectedItem.ToString
                        Case 17  'WorkOrder                                
                            mFrom = cmbWorkOrder.SelectedItem.ToString
                    End Select
                    'End
                    'Changed By Utkarsh On 20-Jul-2011 For All19072011
                    'mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mFrom
                    'Added by Prashant  16-Jul-2013 'ALL15072013
                    If Session("Note") <> "" Then
                        mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mFrom + " Note:- " + Session("Note")
                        Session.Remove("Note")
                    Else
                        mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted + " from " + mFrom
                    End If
                    '-------------------------------------------

                    Select Case mReceiptCumInvoice.StatusID
                        Case 1
                            MarkLog(Util.Action.Save, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                        Case 2
                            If (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10) Then
                                SendMailIfAlternateReceive()
                            End If
                            SendReqPartsMail() 'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
                            SendMail()
                            'Added by Prashant on 26-Oct-2021. To check is this receipt against requisition, checked employee id, if employee id is not blank notification will push in table
                            If mReceiptCumInvoice.ReceiptCumInvoiceItems(0).ReqEmployeeID.Equals(Guid.Empty) Then
                                'Do not push Notification 
                            Else
                                SendPUSHNotification(mReceiptCumInvoice)
                            End If
                            'End of Added by Prashant on 26-Oct-2021
                            MarkLog(Util.Action.Authorize, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                        Case 3
                            MarkLog(Util.Action.Amend, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                        Case 4
                            MarkLog(Util.Action.Cancel, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                    End Select
                    'End
                    mReceiptCumInvoice.MarkClean()

                    mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptCumInvoice.Receipt.ID, mReceiptCumInvoice.InvoiceID)
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                    DataFieldBind()
                    ControlVisibility()
                    SetChargeGrid()
                    SetControlStatus(mReceiptCumInvoice.StatusID)
                    upnlTitle.Update()
                    upnlStatusName.Update()
                    upnlReceiptCumInvoiceDetails.Update()
                    upnlReceivedFrom.Update()
                    upnlReceiptCumInvItems.Update()
                    upnlRCICharges.Update()
                    upnlOtherDetails.Update()
                    upnlButtons.Update()
                    'Sankalp 26-09-25
                    dgItemAttachment.DataSource = mReceiptCumInvoice.FileAttachments
                    dgItemAttachment.DataBind()

                    If mReceiptCumInvoice.StatusID = 2 Then
                        MSGBoxCtrl.Show(MSGBox.Message_title.AuthorizedSuccessFully, MSGBox.Message_text.AuthorizedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    ElseIf mReceiptCumInvoice.StatusID = 4 Then
                        MSGBoxCtrl.Show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    Else
                        MSGBoxCtrl.Show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    End If
                Else
                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Enter valid Other Charge.", MsgBoxStyle.OkOnly, "")
                    Dim mRule As String = ""
                    If mReceiptCumInvoice.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mReceiptCumInvoice.GetBrokenRulesCollection(0).Description
                    ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GetBrokenRulesCollection(0).Description
                    End If
                    MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, mRule, MsgBoxStyle.OkOnly, "")
                    mRule = ""
                    mReceiptCumInvoice = ReceiptCumInvoiceClone
                    SetObject()
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                    DataFieldBind()
                    Exit Function
                End If
            Else
                MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Goods Receipt can not be saved without Part.", MsgBoxStyle.OkOnly, "")
                mReceiptCumInvoice = ReceiptCumInvoiceClone
                SetObject()
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                DataFieldBind()
                Exit Function
            End If
        Catch ex As SqlException
            Session("ReceiptCumInvoiceClone") = ReceiptCumInvoiceClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Exit Function
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                Exit Function
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Function
            ElseIf ex.Number = 547 Then
				If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
					MSGBoxCtrl.Show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
					Exit Function
				ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Receipt quantity exceeds Order quantity.</br></br><b>Please Amend the Purchase Order used in Receipt for excess quantity.</b>", MsgBoxStyle.OkOnly, "")
					Exit Function
				ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
					MSGBoxCtrl.Show("Alert!", "Other Charge Deleted ! ", "The selected Charge can’t be found.<Br><BR>It may have been removed or is no longer available. Please delete it from your selection and choose a new charge to continue.", MsgBoxStyle.OkOnly, "")
					Exit Function
				ElseIf InStr(ex.Message, "FKtabConditionCheckItemtabReceiptItem", CompareMethod.text) Then
					MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "Record cannot be deleted because it is currently used in other records in the system.", "", MsgBoxStyle.OkOnly, "")
					Exit Function
				Else
					MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR> Please check the Entry and try again.", "", MsgBoxStyle.OkOnly, "")
					Exit Function
                End If
            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex1.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                MSGBoxCtrl.Show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "Status")
                mReceiptCumInvoice = ReceiptCumInvoiceClone
                SetObject()
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                DataFieldBind()
                Exit Function
            ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
				MSGBoxCtrl.Show(MSGBox.Message_Title.PendingQty, MSGBox.Message_Text.PendingQty, "<br>" + ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Receipt quantity exceeds Order quantity.</br><b>Please Amend the Purchase Order used in Receipt for excess quantity.</b>", MsgBoxStyle.OkOnly, "")
			Else
				MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR> Please check the Entry and try again.", "", MsgBoxStyle.OkOnly, "")
				mReceiptCumInvoice = ReceiptCumInvoiceClone
				SetObject()
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                DataFieldBind()
                Exit Function
            End If
            mReceiptCumInvoice = ReceiptCumInvoiceClone
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Finally
            ReceiptCumInvoiceClone = Nothing
        End Try
    End Function
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()
        'mReceiptCumInvoice.RecCumInvDate = txtReceiptCumInvoiceDate.Text
        If txtReceiptCumInvoiceDate.Text = "" Then
            mReceiptCumInvoice.RecCumInvDate = Today.Date
        Else
            mReceiptCumInvoice.RecCumInvDate = CDate(txtReceiptCumInvoiceDate.Text)
        End If
        mReceiptCumInvoice.InvText = txtInvoiceText.Text
        mReceiptCumInvoice.InvNo = Val(txtInvoiceNo.Text)
        mReceiptCumInvoice.IntReceiptNo = Trim(txtInternalReceiptNo.Text)
        mReceiptCumInvoice.FromTypeID = CInt(Val(cmbReceivedFrom.SelectedValue))
        mReceiptCumInvoice.VendorID = New Guid(cmbVendor.SelectedValue)
        mReceiptCumInvoice.AircraftID = New Guid(cmbAircraft.SelectedValue)
        mReceiptCumInvoice.StoreID = New Guid(cmbStore.SelectedValue)
        mReceiptCumInvoice.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
        If txtDCDate.Text = "" Then
            mReceiptCumInvoice.DCDate = System.DBNull.Value
        Else
            mReceiptCumInvoice.DCDate = CDate(txtDCDate.Text)
        End If
        mReceiptCumInvoice.DCNO = Trim(txtDCNo.Text)
        mReceiptCumInvoice.VendorInvoiceNo = Trim(txtVendorInvNo.Text)
        If txtVendorInvDate.Text = "" Then
            mReceiptCumInvoice.VendorInvoiceDate = System.DBNull.Value
        Else
            mReceiptCumInvoice.VendorInvoiceDate = CDate(txtVendorInvDate.Text)
        End If
        mReceiptCumInvoice.CurrencyID = New Guid(cmbCurrency.SelectedValue)
        mReceiptCumInvoice.ConversionFactor = CDec(Val(txtFactor.Text))
        mReceiptCumInvoice.Remark = Trim(txtRemark.Text)
        mReceiptCumInvoice.UserName = User.Identity.Name
        mReceiptCumInvoice.RegNo = txtRegNo.Text
        mReceiptCumInvoice.AWBNo = txtAWBNo.Text
        mReceiptCumInvoice.Remark = Trim(txtRemark.Text)
        mReceiptCumInvoice.ReturnInDays = Val(txtReturnInDays.Text) 'Added By Prashant 4-Jun-2010
        mReceiptCumInvoice.IsRoundOff = chkIsRoundOff.Checked ' Added by Prashant 29-Oct-2012
        'Added by Prashant 29-Jan-2018 Deccan29012018
        'Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
        Dim txtValue As TextBox
        Dim i As Integer = 0
        Try
            '------------------------------------------------------------------
            If AppSettings("IsGSTApplicable") = "True" Then
                For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                    With mReceiptCumInvoiceItem
                        If (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 27 Or _
                            mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or mReceiptCumInvoice.TransTypeID = 67 Or _
                            mReceiptCumInvoice.TransTypeID = 28 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or _
                            mReceiptCumInvoice.TransTypeID = 57) Then
                            Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                            mVendor = Vendor.GetVendor(mReceiptCumInvoice.VendorID)
                            If mVendor.ClientCountryName.ToUpper = "INDIA" Then
                                If mVendor.CountryName.ToUpper = "INDIA" And mReceiptCumInvoice.RecCumInvDate >= CDate("01-Jul-2017") Then
                                    mGSTPercentage = GSTPercentage.GetPercentage(mReceiptCumInvoice.RecCumInvDate, 1, .ItemID.ToString)
                                    If Not mGSTPercentage Is Nothing Then
                                        If Len(mVendor.StateCode) > 0 Then
                                            If mVendor.StateCode = mVendor.ClientStateCode Then
                                                txtValue = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtCGSTPer"), TextBox)
                                                .CGSTPercentage = CDec(Val(txtValue.Text))

                                                txtValue = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtSGSTPer"), TextBox)
                                                .SGSTPercentage = CDec(Val(txtValue.Text))

                                                .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                                .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)

                                                .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

                                                .IGSTPercentage = 0
                                                .IGSTCAmount = 0
                                                .HSNACSCode = mtmpItem.HSNACSCode
                                                mReceiptCumInvoice.StateCode = mVendor.StateCode
                                                mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
                                                mReceiptCumInvoice.VendorCountry = mVendor.CountryName
                                                mReceiptCumInvoice.Visibility = 1
                                            Else
                                                '.IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                                txtValue = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtIGSTPer"), TextBox)
                                                .IGSTPercentage = CDec(Val(txtValue.Text))
                                                .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

                                                .CGSTPercentage = 0
                                                .SGSTPercentage = 0
                                                .CGSTCAmount = 0
                                                .SGSTCAmount = 0

                                                .TotalCAmount = .CAmount + .IGSTCAmount
                                                .HSNACSCode = mtmpItem.HSNACSCode
                                                mReceiptCumInvoice.StateCode = mVendor.StateCode
                                                mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
                                                mReceiptCumInvoice.VendorCountry = mVendor.CountryName
                                                mReceiptCumInvoice.Visibility = 2
                                            End If
                                        Else
                                            .CGSTPercentage = 0
                                            .SGSTPercentage = 0
                                            .CGSTCAmount = 0
                                            .SGSTCAmount = 0
                                            .IGSTPercentage = 0
                                            .IGSTCAmount = 0
                                            .TotalCAmount = 0
                                            .HSNACSCode = ""
                                            mReceiptCumInvoice.StateCode = mVendor.StateCode
                                            mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
                                            mReceiptCumInvoice.VendorCountry = mVendor.CountryName
                                            mReceiptCumInvoice.Visibility = 3
                                        End If
                                    End If
                                Else
                                    .CGSTPercentage = 0
                                    .SGSTPercentage = 0
                                    .CGSTCAmount = 0
                                    .SGSTCAmount = 0
                                    .IGSTPercentage = 0
                                    .IGSTCAmount = 0
                                    .TotalCAmount = 0
                                    .HSNACSCode = ""
                                    mReceiptCumInvoice.StateCode = mVendor.StateCode
                                    mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
                                    mReceiptCumInvoice.VendorCountry = mVendor.CountryName
                                    mReceiptCumInvoice.Visibility = 3
                                End If
                            Else
                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .TotalCAmount = 0
                                .HSNACSCode = ""
                                mReceiptCumInvoice.StateCode = mVendor.StateCode
                                mReceiptCumInvoice.ClientStateCode = mVendor.ClientStateCode
                                mReceiptCumInvoice.VendorCountry = mVendor.CountryName
                                mReceiptCumInvoice.Visibility = 3
                            End If
                        End If
                    End With
                    i = i + 1
                Next
            Else
                mReceiptCumInvoice.Visibility = 3
            End If
            '------------------------------------------------------------------
        Catch ex As Exception
            Dim a As Integer = 0
        End Try
        '--------------------------------------------
        mReceiptCumInvoice.Invoice.CalculateTotal()
        mReceiptCumInvoice.WOID = New Guid(cmbWorkOrder.SelectedValue)
        mReceiptCumInvoice.IsReturnFromOHRepair = ChkIsReturnFromOHRepair.Checked 'Added By Utkarsh ON 31-May-2013 FOR ALL30052013
        'Added By Sankalp on 26-09-25
        If Not mReceiptCumInvoice.FileAttachments Is Nothing Then
            If mReceiptCumInvoice.FileAttachments.Count > 0 Then
                mReceiptCumInvoice.IsAttachmentAdded = True
            Else
                mReceiptCumInvoice.IsAttachmentAdded = False
            End If
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex = Index
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Sub DeleteChargeRecord(ByVal indx As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
        mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentIndex = indx
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
                            mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
                            mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex = mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1
                            For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1
                                mReceiptCumInvoice.ReceiptCumInvoiceItems(i).SrNo = i + 1
                            Next
                            dgReceiptCumInvoiceItem.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems
                            dgReceiptCumInvoiceItem.DataBind()
                            SetGrid()
                            upnlReceiptCumInvItems.Update()
                            mReceiptCumInvoice.Invoice.CalculateTotal()
                            If mReceiptCumInvoice.IsRoundOff = True Then 'Added By Prashant on 29-Oct-2012 ALL25102012
                                mReceiptCumInvoice.Invoice.RoundCGrandTotal()
                            End If
                            upnlOtherDetails.Update()
                            ReceiptitemRemovedDetails = mReceiptCumInvoice.ReceiptNo + " Dated: " + mReceiptCumInvoice.RecCumInvDateFormatted.ToString + " Part No. " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemName + " Category " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartCategory + " Qty:- " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Qty.ToString + " Rate:- " + mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.EffRate.ToString
                            Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        Finally
                            MarkLog(Util.Action.Remove, "ReceiptCumInvoice", ReceiptitemRemovedDetails, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCharge" Then
                        Try
                            mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
                            mReceiptCumInvoice.ReceiptCumInvoiceCharges.Remove(mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem)
                            dgReceiptCumInvoiceCharge.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceCharges
                            dgReceiptCumInvoiceCharge.DataBind()
                            upnlRCICharges.Update()
                            mReceiptCumInvoice.Invoice.CalculateTotal()
                            If mReceiptCumInvoice.IsRoundOff = True Then 'Added By Prashant on 29-Oct-2012 ALL25102012
                                SetChargeGrid()
                                mReceiptCumInvoice.Invoice.RoundCGrandTotal()
                            End If
                            upnlOtherDetails.Update()
                            Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If mReceiptCumInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            If Save() = False Then
                                If MSGBoxCtrl.Sender = "ExcessQtyHandle" Then
                                    Exit Sub
                                End If
                            Else
                                Save()
                            End If
                            Else
                            Session.Remove("IsValid")
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mReceiptCumInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            mReceiptCumInvoice.StatusID = 2
                            DataFieldBind()
                            Save()
                            '-----------------------------------------------------------
                            If (mReceiptCumInvoice.StatusID = 2 And mReceiptCumInvoice.TransTypeID = 10) Then
                                Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                                For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                                    'If mReceiptCumInvoiceItem.CalibrationDoneOnDate <> "" Then
                                    If Not IsDBNull(mReceiptCumInvoiceItem.CalibrationDoneOnDate) Then
                                        If AppSettings("ClientCode") = "STR" Then 'Added by Prashant 24-Sep-2020 STR24092020 Calibration Remove yes no message do automatic compliance with ok message
                                            Session("ShowedMSGForCalibration") = "Showed MSG For Calibration"
                                            CalibratedItemComply()
                                            Exit Sub
                                        Else
											ExtraMessage = $"Receipt contains Calibrated Items.{Environment.NewLine} Do you wish to Comply ?"
											MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "CalibratedItemComply")
                                            Session("ShowedMSGForCalibration") = "Showed MSG For Calibration"
                                            Exit Sub
                                        End If
                                    End If
                                Next
                            End If
                            If (mReceiptCumInvoice.StatusID = 2 And mReceiptCumInvoice.TransTypeID = 10) Then
                                Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                                Dim mReceiptItemServiceInspection As ReceiptItemServiceInspection  'Added by Prashant 0n 9-Oct-2019
                                For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                                    For Each mReceiptItemServiceInspection In mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections 'Added by Prashant 0n 9-Oct-2019
                                         If Not IsDBNull(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate) Then 'Added by Prashant 0n 9-Oct-2019
                                            If AppSettings("ClientCode") = "STR" Then 'Added by Prashant 24-Sep-2020 STR24092020 Equipment Maintenance Remove yes no message do automatic compliance with ok message
                                                Session("ShowedMSGForConditionCheck") = "Showed MSG For Condition Check"
                                                ConditionCheckItemComply()
                                                Exit Sub
                                            Else
												ExtraMessage = "Receipt contains Equipment Maintenance Parts. Do you wish to Comply ?"
												MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                                                Session("ShowedMSGForConditionCheck") = "Showed MSG For Condition Check"
                                                Exit Sub
                                            End If
                                            
                                        End If
                                    Next
                                Next
                            End If
                            Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    ElseIf MSGBoxCtrl.Sender = "ExcessQtyHandle" Then 'ALL30082018
                        If mReceiptCumInvoice.IsValid = True Then
                            If (Not User.IsInRole("OrderNew")) And (Not User.IsInRole("OrderEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user to update Purchase Order", False), True)
                                Exit Sub
                            End If
                            'WORKING CODE IN VB.NET For LINQ GROUP & SUM
                            'Dim SumOfDisplayQty = From c In mReceiptCumInvoice.ReceiptCumInvoiceItems
                            '                      Group c By OrderItemID = c.OrderItemID Into Group
                            '                      Select New With {Key .OrderItemID = OrderItemID, Key .DisplayQty = Group.Sum(Function(x) x.DisplayQty)}


                            Dim groupedItems = From item In mReceiptCumInvoice.ReceiptCumInvoiceItems
                                               Group item By item.OrderItemID Into Group
                                               Select New With {
                                                               .OrderItemID = OrderItemID,
                                                               .TotalDisplayQty = Group.Sum(Function(x) x.DisplayQty),
                                                               .Items = Group.ToList()
                                                               }
                            Dim mOrder As Order
                            Dim variable
                            For Each variable In groupedItems
                                Dim TotalRecQty As Decimal
                                Dim mOrderItemDetailForReceipt As OrderItemDetailForReceipt = OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(variable.OrderItemID)
                                mOrder = Order.GetOrder(mOrderItemDetailForReceipt.OrderID)
                                Dim receiptitemchildcol
                                For Each receiptitemchildcol In variable.Items
                                    TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(variable.OrderItemID, receiptitemchildcol.ID.ToString)
                                Next
                                If variable.TotalDisplayQty > mOrder.OrderItems(variable.OrderItemID).Qty - TotalRecQty Then
                                    Dim OldOrderItemQty As Decimal = mOrder.OrderItems(variable.OrderItemID).Qty
                                    Dim NewOrderItemQty As Decimal = OldOrderItemQty + (variable.TotalDisplayQty - (mOrder.OrderItems(variable.OrderItemID).Qty - TotalRecQty))
                                    mOrder.OrderItems(variable.OrderItemID).Qty = NewOrderItemQty
                                    If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 7 Then
                                        mOrder.OrderItems(variable.OrderItemID).OrderItemQuotationItems(0).Qty = NewOrderItemQty
                                    End If
									mOrder.OrderItems(variable.OrderItemID).Note = $"Order Item quantity updated to  {NewOrderItemQty} from {OldOrderItemQty} by automatic process from Goods Receipt."
									receiptitemchildcol.ExcessQty = NewOrderItemQty - OldOrderItemQty

                                    If AppSettings("IsGSTApplicable") = "True" Then
                                        Dim mtmpItem As ItemByID = ItemByID.GetItemByID(mOrder.OrderItems(variable.OrderItemID).ItemID)
                                        mVendor = Vendor.GetVendor(mOrder.VendorID)
                                        If mVendor.ClientCountryName.ToUpper = "INDIA" Then
                                            If mVendor.CountryName.ToUpper = "INDIA" And mOrder.OrderDate >= CDate("01-Jul-2017") Then
                                                mGSTPercentage = GSTPercentage.GetPercentage(mOrder.OrderDate, 1, mOrder.OrderItems(variable.OrderItemID).ItemID.ToString)
                                                If Not mGSTPercentage Is Nothing Then
                                                    If Len(mVendor.StateCode) > 0 Then
                                                        If mVendor.StateCode = mVendor.ClientStateCode Then
                                                            mOrder.OrderItems(variable.OrderItemID).CGSTCAmount = ((mOrder.OrderItems(variable.OrderItemID).CGSTPercentage * mOrder.OrderItems(variable.OrderItemID).CRate * mOrder.OrderItems(variable.OrderItemID).Qty) / 100)
                                                            mOrder.OrderItems(variable.OrderItemID).SGSTCAmount = ((mOrder.OrderItems(variable.OrderItemID).SGSTPercentage * mOrder.OrderItems(variable.OrderItemID).CRate * mOrder.OrderItems(variable.OrderItemID).Qty) / 100)

                                                            mOrder.OrderItems(variable.OrderItemID).TotalCAmount = (mOrder.OrderItems(variable.OrderItemID).CRate * mOrder.OrderItems(variable.OrderItemID).Qty) + mOrder.OrderItems(variable.OrderItemID).CGSTCAmount + mOrder.OrderItems(variable.OrderItemID).SGSTCAmount

                                                            mOrder.OrderItems(variable.OrderItemID).IGSTPercentage = 0
                                                            mOrder.OrderItems(variable.OrderItemID).IGSTCAmount = 0
                                                        Else
                                                            mOrder.OrderItems(variable.OrderItemID).IGSTCAmount = ((mOrder.OrderItems(variable.OrderItemID).IGSTPercentage * mOrder.OrderItems(variable.OrderItemID).CRate * mOrder.OrderItems(variable.OrderItemID).Qty) / 100)
                                                            mOrder.OrderItems(variable.OrderItemID).CGSTCAmount = 0
                                                            mOrder.OrderItems(variable.OrderItemID).SGSTCAmount = 0
                                                            mOrder.OrderItems(variable.OrderItemID).TotalCAmount = (mOrder.OrderItems(variable.OrderItemID).CRate * mOrder.OrderItems(variable.OrderItemID).Qty) + mOrder.OrderItems(variable.OrderItemID).IGSTCAmount
                                                        End If
                                                    Else
                                                        mOrder.OrderItems(variable.OrderItemID).CGSTCAmount = 0
                                                        mOrder.OrderItems(variable.OrderItemID).SGSTCAmount = 0
                                                        mOrder.OrderItems(variable.OrderItemID).IGSTCAmount = 0
                                                        mOrder.OrderItems(variable.OrderItemID).TotalCAmount = 0
                                                    End If
                                                End If
                                            Else
                                                mOrder.OrderItems(variable.OrderItemID).CGSTCAmount = 0
                                                mOrder.OrderItems(variable.OrderItemID).SGSTCAmount = 0
                                                mOrder.OrderItems(variable.OrderItemID).IGSTCAmount = 0
                                                mOrder.OrderItems(variable.OrderItemID).TotalCAmount = 0
                                            End If
                                        Else
                                            mOrder.OrderItems(variable.OrderItemID).CGSTCAmount = 0
                                            mOrder.OrderItems(variable.OrderItemID).SGSTCAmount = 0
                                            mOrder.OrderItems(variable.OrderItemID).IGSTCAmount = 0
                                            mOrder.OrderItems(variable.OrderItemID).TotalCAmount = 0
                                        End If
                                    End If
                                    mOrder.CalculateTotal()
                                    mOrder.Save()
                                    MarkLog(Util.Action.Save, "Order", "Order Qty. Updated by " + User.Identity.Name + " on " + Today.Date.ToString, Util.ErrorType.NoError, mOrder.ID, EventLogID)
                                End If
                            Next

                            Session.Remove("IsValid")
                            'DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
                        Else
                            Session.Remove("IsValid")
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If 'End 'End
                    End If
                    If MSGBoxCtrl.Sender = "CalibratedItemComply" Then
                        CalibratedItemComply()
                        If CheckForConditionCheckItemComply() = True Then
							ExtraMessage = "Receipt contains Equipment Maintenance Parts. Do you wish to Comply ?"
							MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                            Exit Sub
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "ConditionCheckItemComply" Then
                        ConditionCheckItemComply()
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mReceiptCumInvoice.StatusID = 4
                        DataFieldBind()
                        Save()
                    End If
                    If MSGBoxCtrl.Sender = "SaveAttachment" Then
                        mReceiptCumInvoice.UpdateReceiptAttachment()
                        mIsAttachmentNotSave = False
                        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
                    End If


                    'End If


                    'Added by Shital on 17-May-2021
                    If MSGBoxCtrl.Sender = "DifferCurrency" Then
                        'Save()
                        If Save() = False Then
                            If MSGBoxCtrl.Sender = "ExcessQtyHandle" Then
                                Exit Sub
                            End If
                        Else
                            Save()
                        End If
                        MarkLog(Action.Save, mModuleName, "Receipt Currency is different from order currency", ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                    End If
                    'Sankalp 25-05-25
                    If MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
                            mReceiptCumInvoice.FileAttachments.Remove(mReceiptCumInvoice.FileAttachments.CurrentItem)
                            dgItemAttachment.DataSource = mReceiptCumInvoice.FileAttachments
                            dgItemAttachment.DataBind()
                            upnldgItemAttachment.Update()
                            upnlItemAttachment.Update()
                            Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        Catch ex As SqlException

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Or MSGBoxCtrl.Sender = "ExcessQtyHandle" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then

                    End If
                    If MSGBoxCtrl.Sender = "CalibratedItemComply" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If MSGBoxCtrl.Sender = "ConditionCheckItemComply" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If MSGBoxCtrl.Sender = "SaveAttachment" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    'Added by Shital on 17-May-2021
                    If MSGBoxCtrl.Sender = "DifferCurrency" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        Session("sender") = ""
                        If mReceiptCumInvoice.StatusID = 2 Then
                            mReceiptCumInvoice.StatusID = 1
                        ElseIf mReceiptCumInvoice.StatusID = 4 Then
                            mReceiptCumInvoice.StatusID = 2
                        End If
                        Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        DataFieldBind()
                    ElseIf MSGBoxCtrl.Sender = "RCITransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    ElseIf MSGBoxCtrl.Sender = "CalibrationItemComply" Then
                        If Session("ShowedMSGForConditionCheck") = "" Then
                            Session("ShowedMSGForConditionCheck") = ""
                            If (mReceiptCumInvoice.StatusID = 2 And mReceiptCumInvoice.TransTypeID = 10) Then
                                 Dim mReceiptItemServiceInspection As ReceiptItemServiceInspection  'Added by Prashant 0n 9-Oct-2019
                                For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                                    For Each mReceiptItemServiceInspection In mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections 'Added by Prashant 0n 9-Oct-2019
                                        If Not IsDBNull(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate) Then 'Added by Prashant 0n 9-Oct-2019
                                            If AppSettings("ClientCode") = "STR" Then 'Added by Prashant 24-Sep-2020 STR24092020 Equipment Maintenance Remove yes no message do automatic compliance with ok message
                                                ConditionCheckItemComply()
                                                Exit Sub
                                            Else
												ExtraMessage = "Receipt contains Equipment Maintenance Parts. Do you wish to Comply ?"
												MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                                                Exit Sub
                                            End If
                                        End If
                                    Next
                                Next
                            End If
                        End If
                    ElseIf MSGBoxCtrl.Sender = "ConditionCheckItemComplied" Then
                        If Session("ShowedMSGForCalibration") = "" Then
                            Session("ShowedMSGForCalibration") = ""
                            If (mReceiptCumInvoice.StatusID = 2 And mReceiptCumInvoice.TransTypeID = 10) Then
                                 For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                                     If Not IsDBNull(mReceiptCumInvoiceItem.CalibrationDoneOnDate) Then
                                        If AppSettings("ClientCode") = "STR" Then 'Added by Prashant 24-Sep-2020 STR24092020 Calibrated Remove yes no message do automatic compliance with ok message
                                            CalibratedItemComply()
                                            Exit Sub
                                        Else
											ExtraMessage = $"Receipt contains Calibrated Items.{Environment.NewLine} Do you wish to Comply ?"
											MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "CalibratedItemComply")
                                            Exit Sub
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
            End Select
        End If
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)
        btnAddItem.Enabled = IIf(StatusId > 1, False, True)
        btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True)

        ''Code shifted to SetGris Function
        'If (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport") Then
        '    dgReceiptCumInvoiceItem.Columns(30).Visible = False 'Edit  ''Ajay 32=>30
        'End If
        'dgReceiptCumInvoiceItem.Columns(33).Visible = IIf(StatusId > 1, False, True) 'Remove
        ' dgReceiptCumInvoiceItem.Columns(30).Visible = IIf(StatusId > 1, False, True) 'Remove ''Ajay 32=>30
        ''*************************************************************


        dgReceiptCumInvoiceCharge.Columns(4).Visible = IIf(StatusId > 1, False, True) 'Ord/Issue No ''delete & Edit 'Ajay
        'dgReceiptCumInvoiceCharge.Columns(5).Visible = IIf(StatusId > 1, False, True) 'Ord/Issue Date 'Ajay

        If mReceiptCumInvoice.TransTypeID = Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder Or mReceiptCumInvoice.TransTypeID = Util.Trans.ExchangeRepairReceivedFromVendor Or mReceiptCumInvoice.TransTypeID = Util.Trans.ReceivedfromSupplierRentalLease Then
            dgReceiptCumInvoiceItem.Columns(5).HeaderText = "Order Info." ''Order No./Date Ajay
            'dgReceiptCumInvoiceItem.Columns(5).HeaderText = "Order Date"
        ElseIf mReceiptCumInvoice.TransTypeID = Util.Trans.ReceivedFromAircraft Or mReceiptCumInvoice.TransTypeID = Util.Trans.ReceiptasLoanFromSupplier Or mReceiptCumInvoice.TransTypeID = Util.Trans.ReceiptasLoanFromCustomer Or mReceiptCumInvoice.TransTypeID = Util.Trans.ReceiptFromCustomer Or mReceiptCumInvoice.TransTypeID = Util.Trans.ReceivedFromCustomerAsForRepair Then
            dgReceiptCumInvoiceItem.Columns(5).Visible = False
            'dgReceiptCumInvoiceItem.Columns(5).Visible = False
        Else
            dgReceiptCumInvoiceItem.Columns(5).HeaderText = "Issue Info." ''Issue No./Date
            'dgReceiptCumInvoiceItem.Columns(5).HeaderText = "Issue Date"
        End If
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "TAAL" Then
            lblInternalReceiptNo.Text = "GIR No. & DT."
            dgReceiptCumInvoiceItem.Columns(14).HeaderText = "RNN No."
        Else
            lblInternalReceiptNo.Text = "Int. Recpt. No."
            dgReceiptCumInvoiceItem.Columns(14).HeaderText = "Batch No."
        End If
        If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
            dgReceiptCumInvoiceItem.Columns(24).HeaderText = "GSE No." ''24=>22
        End If
        If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
            btnAddItem.ToolTip = "Click To Add New Item."
            btnAddCharge.ToolTip = "Click To Add Other Charge"
            btnDocketCharge.ToolTip = "Click To Add Docket Charge"
            btnCancel.ToolTip = "Click to Cancel the Goods Receipt"
            btnBack.ToolTip = "Click to close Goods Receipt Details screen"
            btnPrint.ToolTip = "Click to print Goods Receipt"
            btnAuthorized.ToolTip = "Click to Authorize the Goods Receipt"
            ' lblReceiptCumInvItemCaption.Text = "Receipt Item(s):"
            ' lblRCIChargeCaption.Text = "Goods Receipt Charge(s):"
        End If
    End Sub
    Private Sub SetReceivedFromDetails(ByVal ToType As Int16)
        Select Case ToType
            Case 0
                lblSelectDetails.Visible = False
                cmbVendor.Visible = False
                cmbAircraft.Visible = False
                cmbStore.Visible = False
                btnAddItem.Enabled = False
                cmbWorkShop.Visible = False
            Case 14  'Vendor
                lblSelectDetails.Visible = True
                cmbVendor.Visible = True
                cmbAircraft.Visible = False
                cmbStore.Visible = False
                btnAddItem.Enabled = True
                cmbWorkShop.Visible = False
            Case 2   'Aircraft
                lblSelectDetails.Visible = True
                cmbVendor.Visible = False
                cmbAircraft.Visible = True
                cmbStore.Visible = False
                btnAddItem.Enabled = True
                cmbWorkShop.Visible = False
            Case 8   'Store
                lblSelectDetails.Visible = True
                cmbVendor.Visible = False
                cmbAircraft.Visible = False
                cmbStore.Visible = True
                btnAddItem.Enabled = True
                cmbWorkShop.Visible = False
            Case 16  'WorkShop
                lblSelectDetails.Visible = True
                cmbVendor.Visible = False
                cmbAircraft.Visible = False
                cmbStore.Visible = False
                btnAddItem.Enabled = True
                cmbWorkShop.Visible = True
            Case 17  'WorkOrder       ' ------Added By Utkarsh 10-Dec-2010
                lblSelectDetails.Visible = True
                cmbVendor.Visible = False
                cmbAircraft.Visible = False
                cmbStore.Visible = False
                btnAddItem.Enabled = True
                cmbWorkShop.Visible = False
                cmbWorkOrder.Visible = True
        End Select
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        Dim txtCGSTPer As TextBox
        Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
        Dim i As Integer = 0
        For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
            With mReceiptCumInvoiceItem
                Try
                    txtCGSTPer = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtCGSTPer"), TextBox)
                    txtCGSTPer.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtCGSTPer.ClientID + "').value,event)")

                    txtValue = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtSGSTPer"), TextBox)
                    txtValue.Text = Val(txtCGSTPer.Text)

                    txtValue = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtIGSTPer"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
                Catch ex As Exception
                End Try
            End With
            i = i + 1
        Next
        upnlReceiptCumInvItems.Update()
    End Sub
    Private Sub addAttributes()
        txtFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtFactor').value,event)")
        txtInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtInvoiceNo').value,event)")
        txtReturnInDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtReturnInDays').value,event)")
    End Sub
    Private Sub ControlVisibility()
        cmbStore.Enabled = ((mReceiptCumInvoice.IsNew And (CType(mTransTypeID, Util.Trans) = Util.Trans.ReceivedFromOtherStore Or CType(mTransTypeID, Util.Trans) = Util.Trans.ReceiptAgainstLoanIssuedToStore Or CType(mTransTypeID, Util.Trans) = Util.Trans.LoanTakenFromStore Or CType(mTransTypeID, Util.Trans) = Util.Trans.LoanReturnToStore) And mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0))
        cmbVendor.Enabled = ((mReceiptCumInvoice.IsNew And (CType(mTransTypeID, Util.Trans) = Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder Or CType(mTransTypeID, Util.Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Or CType(mTransTypeID, Util.Trans) = Util.Trans.ReceiptAgainstLoanIssueToVendor Or CType(mTransTypeID, Util.Trans) = Util.Trans.ReceiptAgainstLoanIssueToCustomer Or CType(mTransTypeID, Util.Trans) = Util.Trans.ReceiptFromCustomer) And mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0))
        cmbAircraft.Enabled = ((mReceiptCumInvoice.IsNew And (CType(mTransTypeID, Util.Trans) = Util.Trans.ReceivedFromAircraft Or CType(mTransTypeID, Util.Trans) = Util.Trans.ReceiptAgainstLoanIssuedToAircraft) Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0))
        txtDCDate.Enabled = (mReceiptCumInvoice.StatusID = 1)
        'cmbCurrency.Enabled = (Not (mReceiptCumInvoice.FromTypeID = 8 Or mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 54)) And (Not mReceiptCumInvoice.StatusID <> 1)   'If RCI is Store to store 'mReceiptCumInvoice.TransTypeID = 7,10,54 Added By Vikrant on 12-Jun-2020 For ALL12062020
        'txtFactor.Enabled = (Not (mReceiptCumInvoice.FromTypeID = 8 Or mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 54)) And (Not mReceiptCumInvoice.StatusID <> 1)  'If RCI is Store to store 'mReceiptCumInvoice.TransTypeID = 7,10,54 Added By Vikrant on 12-Jun-2020 For ALL12062020
        cmbCurrency.Enabled = (Not (mReceiptCumInvoice.FromTypeID = 8)) And (Not mReceiptCumInvoice.StatusID <> 1)   ''mReceiptCumInvoice.TransTypeID = 7,10,54 removed by vikrant on 10-Aug-2020 as per Heligo requirement as per maill discussed in meeting which was added for ALL12062020
        txtFactor.Enabled = (Not (mReceiptCumInvoice.FromTypeID = 8)) And (Not mReceiptCumInvoice.StatusID <> 1)  'mReceiptCumInvoice.TransTypeID = 7,10,54 removed by vikrant on 10-Aug-2020 as per Heligo requirement as per maill discussed in meeting which was added for ALL12062020

        txtVendorInvDate.Enabled = (mReceiptCumInvoice.StatusID = 1)
        btnAuthorized.Visible = (Not mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.StatusID = 1)
        btnCancel.Visible = (Not mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.StatusID = 2) And (mReceiptCumInvoice.Receipt.IsSync = 0) 'One Condition Added by Saylee on 2-June-2010
        cmbWorkShop.Enabled = ((mReceiptCumInvoice.IsNew And (CType(mTransTypeID, Util.Trans) = Util.Trans.AssembledFromWorkShop Or CType(mTransTypeID, Util.Trans) = Util.Trans.ReceiptAgainstLoanIssuedToWorkShop Or CType(mTransTypeID, Util.Trans) = Util.Trans.ReceivedFromWorkShopAsServiceableReturned) Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0))
        txtInvoiceText.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
        txtInvoiceNo.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
        txtReceiptCumInvoiceDate.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean) And mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0)
        cmbWorkOrder.Enabled = ((mReceiptCumInvoice.IsNew And (CType(mTransTypeID, Util.Trans) = Util.Trans.RCIFromWorkOrderAsReturn Or CType(mTransTypeID, Util.Trans) = Util.Trans.RCIFromWorkOrder) Or mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0))
        'txtRemark.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
        btnSentToBill.Visible = ((mReceiptCumInvoice.FromTypeID = 2 Or mReceiptCumInvoice.FromTypeID = 14)) And (Not mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.StatusID = 2) And ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer")   'Added by Saylee on 2-June-2010
        btnSentToBill.Enabled = (mReceiptCumInvoice.FromTypeID = 2 Or mReceiptCumInvoice.FromTypeID = 14) And (Not mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.Receipt.IsSync = 0) And ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer")   'Added by Saylee on 2-June-2010
        chkIsRoundOff.Enabled = (mReceiptCumInvoice.StatusID = 1)
        'Added By Prashant 17-Aug-2011
        If Not IsInRole(Rights.Authorized) Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
            btnSaveAttachment.Enabled = False
            btnSaveAttachment.ToolTip = "You are not authorized user "
        End If
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            'btnSelectFile.Disabled = True
            'btnDelAttach.Enabled = False
            'btnDelAttach.ToolTip = "You are not authorized user "
            'ImageButton1.Enabled = False
            'ImageButton1.ToolTip = "You are not authorized user "
        End If
        'Added by Prashant on 25-Jul-2012----------------
        btnDocketCharge.Visible = (mReceiptCumInvoice.StatusID = 2) 'And AppSettings("OtherChargeDocket") = "True")
        '------------------------------------------------
        'Other Charge
        dgReceiptCumInvoiceItem.Columns(20).Visible = IIf((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA"), False, True) 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

        'Code No 24=> 22 ''Ajay
        dgReceiptCumInvoiceItem.Columns(22).Visible = IIf((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA"), True, False)
        'Added By Prashant 2-Dec-2013 --ALL25102013-1
        If (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0) Then
            ChkIsReturnFromOHRepair.Enabled = False
        End If
        'Added By Prashant 28-Oct-2013 --ALL25102013-1
        If (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then
            dgReceiptCumInvoiceItem.Columns(17).Visible = True
            dgReceiptCumInvoiceItem.Columns(18).Visible = True
            dgReceiptCumInvoiceItem.Columns(15).Visible = False
            'dgReceiptCumInvoiceItem.Columns(16).Visible = False
        Else
            dgReceiptCumInvoiceItem.Columns(17).Visible = False
            dgReceiptCumInvoiceItem.Columns(18).Visible = False
            dgReceiptCumInvoiceItem.Columns(15).Visible = True
            dgReceiptCumInvoiceItem.Columns(16).Visible = True
        End If
        'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
        If (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport") Then
            dgReceiptCumInvoiceItem.Columns(32).Visible = False 'Attach 'Ajay 27-Jan-23 Cells(35) 34=>32
            dgReceiptCumInvoiceItem.Columns(33).Visible = False 'Remove Attachment 'Ajay 27-Jan-23 Cells(36) 35=> 33
        Else
            dgReceiptCumInvoiceItem.Columns(32).Visible = IIf(mReceiptCumInvoice.StatusID = 2, True, False) 'Ajay 27-Jan-23 Cells(35) 34=>32
            dgReceiptCumInvoiceItem.Columns(33).Visible = IIf(mReceiptCumInvoice.StatusID = 2, True, False)  'Ajay 27-Jan-23 Cells(36) 35=>33
        End If

        'btnSelectFile.Disabled = IIf(mReceiptCumInvoice.StatusID > 2, True, False) 'Comment by Sankalp
        'btnSelectFiles.Visible = IIf(mReceiptCumInvoice.StatusID > 2, True, False) 'Added by sankalp
        If mReceiptCumInvoice.StatusID = 2 Then
            btnSaveAttachment.Visible = True
        Else
            btnSaveAttachment.Visible = False
        End If
        'End
        '---------------------------------------------	

        '---------------------------------------------------------------------------
        Dim txtCGSTPer As TextBox
        Dim txtIGSTPer As TextBox
        For i As Integer = 0 To dgReceiptCumInvoiceItem.Rows.Count - 1
            txtCGSTPer = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtCGSTPer"), TextBox)
            txtCGSTPer.Enabled = IIf(mReceiptCumInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mReceiptCumInvoice.ReceiptCumInvoiceItems(i).HSNACSCode = "", False, True)
            txtIGSTPer = CType(Me.dgReceiptCumInvoiceItem.Rows(i).FindControl("txtIGSTPer"), TextBox)
            txtIGSTPer.Enabled = IIf(mReceiptCumInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mReceiptCumInvoice.ReceiptCumInvoiceItems(i).HSNACSCode = "", False, True)
        Next
        If mReceiptCumInvoice.Visibility = 1 Then
            dgReceiptCumInvoiceItem.Columns(24).Visible = True 'CGSTPercentage   ''Ajay 26=>24
            dgReceiptCumInvoiceItem.Columns(25).Visible = True 'CGSTCAmount      ''Ajay 27=>25
            dgReceiptCumInvoiceItem.Columns(26).Visible = True 'SGSTPercentage   ''Ajay 28=>26
            dgReceiptCumInvoiceItem.Columns(27).Visible = True 'SGSTCAmount      ''Ajay 29=>27
            dgReceiptCumInvoiceItem.Columns(28).Visible = False 'IGSTPercentage  ''Ajay 30=>28
            dgReceiptCumInvoiceItem.Columns(29).Visible = False 'IGSTCAmount     ''Ajay 31=>29

            lblTotalCGST.Visible = True
            txtTotalCGST.Visible = True
            lblTotalSGST.Visible = True
            txtTotalSGST.Visible = True

            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False
        ElseIf mReceiptCumInvoice.Visibility = 2 Then
            dgReceiptCumInvoiceItem.Columns(24).Visible = False 'CGSTPercentage  ''Ajay 26=>24
            dgReceiptCumInvoiceItem.Columns(25).Visible = False 'CGSTCAmount     ''Ajay 27=>25
            dgReceiptCumInvoiceItem.Columns(26).Visible = False 'SGSTPercentage  ''Ajay 28=>26
            dgReceiptCumInvoiceItem.Columns(27).Visible = False 'SGSTCAmount     ''Ajay 29=>27
            dgReceiptCumInvoiceItem.Columns(28).Visible = True  'IGSTPercentage  ''Ajay 30=>28
            dgReceiptCumInvoiceItem.Columns(29).Visible = True 'IGSTCAmount      ''Ajay 31=>29

            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False

            lblTotalIGST.Visible = True
            txtTotalIGST.Visible = True
        ElseIf mReceiptCumInvoice.Visibility = 3 Then
            If AppSettings("HSNACSCodeVisibleInPartMaster") = "True" Then
                dgReceiptCumInvoiceItem.Columns(23).Visible = True 'HSNACSCode   ''Ajay 25=>23
            Else
                dgReceiptCumInvoiceItem.Columns(23).Visible = False 'HSNACSCode  ''Ajay 25=>23
            End If
            dgReceiptCumInvoiceItem.Columns(24).Visible = False 'CGSTPercentage  ''Ajay 26=>24
            dgReceiptCumInvoiceItem.Columns(25).Visible = False 'CGSTCAmount     ''Ajay 27=>25
            dgReceiptCumInvoiceItem.Columns(26).Visible = False 'SGSTPercentage  ''Ajay 28=>26
            dgReceiptCumInvoiceItem.Columns(27).Visible = False 'SGSTCAmount     ''Ajay 29=>27
            dgReceiptCumInvoiceItem.Columns(28).Visible = False  'IGSTPercentage ''Ajay 30=>28
            dgReceiptCumInvoiceItem.Columns(29).Visible = False 'IGSTCAmount     ''Ajay 31=>29
            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False
            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False
        End If
        '---------------------------------------------------------------------------
        btnPrintTag.Enabled = (Not mReceiptCumInvoice.IsNew) AndAlso (AppSettings("ToAllowPrintTagForOpenReceipt") = "True" Or (AppSettings("ToAllowPrintTagForOpenReceipt") = "False" And mReceiptCumInvoice.StatusID = 2))
    End Sub
    Private Function ISInDate() As Boolean 'Added by Saylee on 15-July-2010
        Dim dueDay As Integer = CType(AppSettings("dueDay"), Integer)
        Dim tmpDate As Date = New Date(Year(mReceiptCumInvoice.RecCumInvDate), Month(mReceiptCumInvoice.RecCumInvDate) + 1, dueDay)    ''New Date(2010, 7,  7)
        Dim PrevtmpDate As Date = New Date(Year(mReceiptCumInvoice.RecCumInvDate), Month(mReceiptCumInvoice.RecCumInvDate), 1)

        If Today.Date >= PrevtmpDate And (Today.Date <= tmpDate) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        '        Select Case mTransTypeID                             'Commented By Prashant 17-Aug-2011
        Select Case mReceiptCumInvoice.TransTypeID                    'Added By Prashant 17-Aug-2011
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
        'IsInRoleString = "ReceiptCumInvoice"
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
            Case Rights.Authorized                              'Added By Prashant 17-Aug-2011
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Function getVendorStatus(ByVal TransTypeID As Integer, ByVal Type As RequstFor) As Boolean
        If Type = RequstFor.Supplier Then                                  'Receipt Cum Invoice 
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.ReceiptAgainstPuchaseOrder
                    Return True
                Case Util.Trans.ExchangeRepairReceivedFromVendor
                    Return True
                Case Util.Trans.ReceiptAgainstLoanIssueToVendor
                    Return True
                Case Util.Trans.ReceiptcumInvoiceAgainstPuchaseOrder
                    Return True
                Case Util.Trans.ReceiptasLoanFromSupplier
                    Return True
                    'Added By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1
                Case Util.Trans.RCIFromSupplierAsNone
                    Return True
                    'End
                Case Else
                    Return False
            End Select
        ElseIf Type = RequstFor.Customer Then                              'Receipt Cum Invoice      
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.ReceiptAgainstLoanIssueToCustomer
                    Return True
                Case Util.Trans.ReceiptasLoanFromCustomer
                    Return True
                Case Util.Trans.ReceiptFromCustomer
                    Return True
                Case Util.Trans.ReceivedFromCustomerAsForRepair
                    Return True
                Case Else
                    Return False
            End Select
        End If
    End Function
    Private Sub SetGrid()
        Dim P As Boolean
        Dim deletebtn As ImageButton
        Dim Editbtn As ImageButton
        For j As Integer = 0 To dgReceiptCumInvoiceItem.Rows.Count - 1
            P = CType(Me.dgReceiptCumInvoiceItem.Rows.Item(j).Cells(31).Text, Boolean) 'Ajay 27-Jan-23 Cells(34) 33=>31


            deletebtn = CType(Me.dgReceiptCumInvoiceItem.Rows(j).Cells(30).FindControl("DeleteRecord"), ImageButton)
            Editbtn = CType(Me.dgReceiptCumInvoiceItem.Rows(j).Cells(30).FindControl("EditView"), ImageButton)

            If P Then
                dgReceiptCumInvoiceItem.Rows(j).Cells(0).Visible = True
                dgReceiptCumInvoiceItem.Rows(j).Cells(33).Enabled = True 'Ajay 27-Jan-23 Cells(36) 35=>33
            Else
                Dim img As ImageButton = dgReceiptCumInvoiceItem.Rows(j).Cells(0).FindControl("ViewAttachment")
                img.Visible = False
                dgReceiptCumInvoiceItem.Rows(j).Cells(33).Enabled = False 'Ajay 27-Jan-23 Cells(36) 35=>33
            End If


            ''Aded by Saylee on 1-Mar-2023, to make visiblility as per StatusID
            deletebtn.Visible = IIf(mReceiptCumInvoice.StatusID > 1, False, True) 'Remove 

            If (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport") Then
                Editbtn.Visible = False
            End If
        Next


    End Sub
    Private Sub CGrandTotal()
        Dim SumCEffectiveAmount As Decimal
        Dim mInvoiceDocketCharge As Decimal
        For i As Integer = 0 To mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1
            If (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 54 Or (mReceiptCumInvoice.TransTypeID = 67 And mReceiptCumInvoice.IsReturnFromOHRepair = True)) Then
                SumCEffectiveAmount = SumCEffectiveAmount + (mReceiptCumInvoice.ReceiptCumInvoiceItems(i).GROCEffRate * mReceiptCumInvoice.ReceiptCumInvoiceItems(i).Qty) - (mReceiptCumInvoice.ReceiptCumInvoiceItems(i).CGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems(i).SGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IGSTCAmount)
            Else
                'SumCEffectiveAmount = SumCEffectiveAmount + (mReceiptCumInvoice.ReceiptCumInvoiceItems(i).CEffRate * mReceiptCumInvoice.ReceiptCumInvoiceItems(i).Qty) - (mReceiptCumInvoice.ReceiptCumInvoiceItems(i).CGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems(i).SGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems(i).IGSTCAmount)
                SumCEffectiveAmount = SumCEffectiveAmount + (mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayCEffRate * mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayQty) - (mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayCGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplaySGSTCAmount + mReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayIGSTCAmount)
            End If
        Next
        mInvoiceDocketCharge = SumCEffectiveAmount - mReceiptCumInvoice.CTotalAmount - mReceiptCumInvoice.CTotalCharges 'This is to show Docket charges per invoice.
        mOtherChargeListByInvoiceID = OtherChargeListByInvoiceID.GetOtherChargeListByInvoiceID(mReceiptCumInvoice.InvoiceID.ToString)
        If mOtherChargeListByInvoiceID.Count <> 0 Then
            txtInvoiceDocketCharge.Visible = True
            lblInvoiceDocketCharge.Visible = True
            txtInvoiceDocketCharge.Text = CDec(Format(mInvoiceDocketCharge, "##0.00##")).ToString
            lblTotalDocketCharge.Visible = True
            lblTotalDocketCharge.Text = "Total Docket Charge : " + mOtherChargeListByInvoiceID.Item(0).CGrandTotal.ToString + " in " + cmbCurrency.SelectedItem.Text
        Else
            txtInvoiceDocketCharge.Visible = False
            lblInvoiceDocketCharge.Visible = False
            txtInvoiceDocketCharge.Text = ""
            lblTotalDocketCharge.Visible = False
            lblTotalDocketCharge.Text = ""
        End If
    End Sub
    Private Sub SetChargeGrid()
        For j As Integer = 0 To dgReceiptCumInvoiceCharge.Rows.Count - 1
            If (Me.dgReceiptCumInvoiceCharge.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or Me.dgReceiptCumInvoiceCharge.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then
                dgReceiptCumInvoiceCharge.Rows.Item(j).Cells(4).Visible = False
                'dgReceiptCumInvoiceCharge.Rows.Item(j).Cells(5).Enabled = False
            End If
        Next
    End Sub
    'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
    Private Sub SendReqPartsMail()
        If AppSettings("MailsRequire") = "True" Then
            If Thread.CurrentPrincipal.Identity.Name.ToUpper = "BTPLADMIN" Or Thread.CurrentPrincipal.Identity.Name.ToUpper = "BYTZADMIN" Then
                'Do nothing
                Exit Sub
            End If
            Dim RCIItems
            If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then
                RCIItems = (From c As ReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                          Where Not c.ReqEmployeeEmailIDs = ""
                          Select c).ToList
            End If
            If RCIItems.count > 0 Then
                Dim strGeneratedReport As String = ""
                Dim EmailIDs As New StringBuilder
                For i As Integer = 0 To RCIItems.count - 1
                    If Not EmailIDs.ToString.Contains(RCIItems(i).ReqEmployeeEmailIDs) Then
                        EmailIDs.Append(RCIItems(i).ReqEmployeeEmailIDs + ",")
                    End If

                Next
                strGeneratedReport = GenerateReportBodyForReqParts(RCIItems)
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Requested Part(s) received", mReceiptCumInvoice.ReceiptNo, ToMailID:=EmailIDs.ToString.TrimEnd(","), Info:=strGeneratedReport, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                        SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
            End If
        End If
    End Sub
    Private Function GenerateReportBodyForReqParts(ByVal RCIItems) As String
        Dim str As String = ""
        str = str + ("<p><font face=""Calibri"">Following Requested Part(s) received in <b> " + mReceiptCumInvoice.ReceiptNo + "</b> Dated <b> " + mReceiptCumInvoice.RecCumInvDateFormatted + "</b></font></p>")
        str = str + ("<p><font face=""Calibri"">by User : <b>" + Thread.CurrentPrincipal.Identity.Name + " </b></font></p>")
        str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
        str = str + ("<tr>" & "<td align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requested Part No.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Serial No.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requisition No.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requisition Date</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requested Qty.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Receipt Qty.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requested By</b>" & "</font>" & "</td></tr>")

        Dim srNo As Integer = 1
        Dim i As Integer = 0
        'Dim RCIItem
        For Each RCIItem As ReceiptCumInvoiceItem In RCIItems
            str = str + ("<TR>")
            str = str + ("<TD WIDTH=20px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + (srNo.ToString)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=80px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RCIItem.ItemName
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RCIItem.SerialNo
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RCIItem.ReqNo
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RCIItem.ReqDate.ToString
            str = str + ("</font>")
            str = str + ("</TD>")

            'str = str + ("<TD WIDTH=70px align=""left"">")
            'str = str + ("<font face=""Calibri"">")
            'str = str + RCIItem.ReqQty.ToString
            'str = str + ("</font>")
            'str = str + ("</TD>")
            Dim RowSpanCount As Integer = 0

            RowSpanCount = (From RCIItemInfo As ReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                                                                    Where RCIItemInfo.ItemID = RCIItems(i).ItemID
                                                                    Select RCIItemInfo).Count()
            If i = 0 Then
                str = str + ("<TD WIDTH=70px align=""left"" rowspan=" + RowSpanCount.ToString + ">")
                str = str + ("<font face=""Calibri"">")
                str = str + RCIItems(i).ReqQty.ToString
                str = str + ("</font>")
                str = str + ("</TD>")
            Else
                If RCIItems(i).ReqItemID.Equals(RCIItems(i - 1).ReqItemID) Then
                    'str = str + ("<TD id=""tdReqQty""" + (i + 1).ToString + " WIDTH=70px align=""left"">")
                    'str = str + ("<font face=""Calibri"">")
                    'str = str + ""
                    'str = str + ("</font>")
                    'str = str + ("</TD>")
                Else
                    str = str + ("<TD WIDTH=70px align=""left"" rowspan=" + RowSpanCount.ToString + ">")
                    str = str + ("<font face=""Calibri"">")
                    str = str + RCIItems(i).ReqQty.ToString
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If
            End If


            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + CDec(Format(RCIItem.Qty, "##0.00##")).ToString
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RCIItem.ReqEmployeeName
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("</TR>")
            srNo = srNo + 1
            i = i + 1
        Next
        str = str + ("</TABLE>")
        Return str
    End Function
    'End
    Public Sub SendMailIfAlternateReceive()
        If AppSettings("MailsRequire") = "True" Then
            If Thread.CurrentPrincipal.Identity.Name.ToUpper = "BTPLADMIN" Or Thread.CurrentPrincipal.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
                'Do nothing
                Exit Sub
            End If
            Dim Alternate
            If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then
                Alternate = (From c As ReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                          Where Not c.AlternateItemID.Equals(Guid.Empty)
                          Select c).ToList
            End If
            If Alternate.count > 0 Then
                Dim strGeneratedReport As String = ""
                strGeneratedReport = GenerateReportBody(Alternate)
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Alternate Part(s) received", mReceiptCumInvoice.ReceiptNo, Info:=strGeneratedReport, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                        SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
            End If
        End If
    End Sub
    Private Function GenerateReportBody(ByVal Alternate) As String 'Added by utkarsh on 16-sep-2013
        Dim str As String = ""
        str = str + ("<p><font face=""Calibri"">Following Alternate Part(s) received in <b> " + mReceiptCumInvoice.ReceiptNo + "</b> Dated <b> " + mReceiptCumInvoice.RecCumInvDateFormatted + "</b></font></p>")
        str = str + ("<p><font face=""Calibri"">by User : <b>" + Thread.CurrentPrincipal.Identity.Name + " </b>Last Modified Dated : <b>" + New SmartDate(Today.Date).FormattedText + "</b></font></p>")
        str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
        str = str + ("<tr>" & "<td align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Ordered Part #</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Received Part #</b>" & "</font>" & "</td></tr>")

        Dim srNo As Integer = 1
        Dim i As Integer = 0
        Dim Alternateitem
        For Each Alternateitem In Alternate
            Dim S As String = ""
            S = Alternateitem.OrderItemDetailForReceipt.ItemName
            'Dim Note As String = "Ordered Part " + S + " was amended to  " + Alternateitem.ItemName + ", as Alternate Part  " + Alternateitem.ItemName + " has been received."

            str = str + ("<TR>")
            str = str + ("<TD WIDTH=20px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + (srNo.ToString)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + S
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + Alternateitem.ItemName
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("</TR>")
            srNo = srNo + 1
            i = i + 1
        Next
        str = str + ("</TABLE>")
        Return str
    End Function 'End
    Private Function CheckDateForTransactionLock(ByVal TransDate As Date) As Boolean 'Added By Vikrant On 24-July-2014 For BA24072014
        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
        If (TransDate >= FirstDayofLastMonth) Then
            If (TransDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                If mReceiptCumInvoice.StatusID = 4 Then
                    mReceiptCumInvoice.StatusID = 2
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                End If
                Return True
            Else
                Return False
            End If
        Else
            If mReceiptCumInvoice.StatusID = 4 Then
                mReceiptCumInvoice.StatusID = 2
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
            End If
            Return True
        End If
    End Function
    'Private Sub ControlVisibilityForFileAttachment()
    '    If mReceiptCumInvoice.IsAttachmentAdded Then
    '        ImageButton1.Visible = True
    '        btnDelAttach.Enabled = IIf(mReceiptCumInvoice.StatusID > 2, False, True)
    '        'btnDelAttach.Enabled = True
    '    Else
    '        ImageButton1.Visible = False
    '        btnDelAttach.Enabled = False
    '    End If
    'End Sub
    Private Sub CalibratedItemComply()
        Dim mCalibrationItemChildList As CalibrationItemChildList
        Dim mOldCalibrationItemChild As CalibrationItemChild
        Dim mCalibrationItem As CalibrationItem
        Dim mCalibrationItemChild As CalibrationItemChild
        Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
        For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
             If Not IsDBNull(mReceiptCumInvoiceItem.CalibrationDoneOnDate) Then
                mCalibrationItemChildList = CalibrationItemChildList.GetCalibrationChildList(FromDate:="1/1/1900", ToDate:="1/1/3300", ItemName:=mReceiptCumInvoiceItem.ItemName, Description:=mReceiptCumInvoiceItem.ItemDescription, SerialNo:=mReceiptCumInvoiceItem.SerialNo)
                mCalibrationItem = CalibrationItem.GetCalibrationItem(mCalibrationItemChildList(0).CalibrationItemID)
                mOldCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(mCalibrationItemChildList(0).ID)
                If mOldCalibrationItemChild.IsApplicable = True Then
                    If CDate(mOldCalibrationItemChild.DoneOnDate) < CDate(mReceiptCumInvoiceItem.CalibrationDoneOnDate) Then
                        mCalibrationItemChild = CalibrationItemChild.NewComplyCalibrationItemChild(CalibrationItemID:=mCalibrationItem.ID, CalDoneOnDate:=mReceiptCumInvoiceItem.CalibrationDoneOnDate.ToString, PreviousCalibrationItemChildID:=mOldCalibrationItemChild.ID)
                        mCalibrationItemChild.ItemName = mOldCalibrationItemChild.ItemName
                        mCalibrationItemChild.Description = mOldCalibrationItemChild.Description
                        mCalibrationItemChild.SerialNo = mOldCalibrationItemChild.SerialNo
                        mCalibrationItemChild.Frequency = mOldCalibrationItemChild.CalibrationItemChildFrequency
                        mCalibrationItemChild.CalibrationPeriodInID = mOldCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID
                        mCalibrationItemChild.CalibrationItemChildFrequency = mOldCalibrationItemChild.CalibrationItemChildFrequency
                        mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = mOldCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID
                        mCalibrationItemChild.DoneOnDate = mReceiptCumInvoiceItem.CalibrationDoneOnDate
                        mCalibrationItemChild.Location = mOldCalibrationItemChild.Location
                        'If mCalibrationItemChild.CalibrationPeriodInID = 1 Then
                        If mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 1 Then
                            mCalibrationItemChild.NextDueDate = CDate(mReceiptCumInvoiceItem.CalibrationDoneOnDate).AddDays(mOldCalibrationItemChild.CalibrationItemChildFrequency)
                        ElseIf mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 2 Then
                            mCalibrationItemChild.NextDueDate = CDate(mReceiptCumInvoiceItem.CalibrationDoneOnDate).AddMonths(mOldCalibrationItemChild.CalibrationItemChildFrequency)
                        ElseIf mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 3 Then
                            mCalibrationItemChild.NextDueDate = CDate(mReceiptCumInvoiceItem.CalibrationDoneOnDate).AddYears(mOldCalibrationItemChild.CalibrationItemChildFrequency)
                        End If
                        ItemsComply.Append("Part No. : " + mCalibrationItemChild.ItemName + " Serial No. : " + mCalibrationItemChild.SerialNo + "<BR>")
                        mCalibrationItemChild = mCalibrationItemChild.Save()
                    End If
                End If
            End If
        Next
        If ItemsComply.Length = 0 Then
            'Do nothing
        Else
            ShowMessage(ItemsComply:=ItemsComply.ToString)
        End If
    End Sub
    Private Function CheckForConditionCheckItemComply() As Boolean
         If Session("ShowedMSGForConditionCheck") = "" Then
            Session("ShowedMSGForConditionCheck") = ""
            If (mReceiptCumInvoice.StatusID = 2 And mReceiptCumInvoice.TransTypeID = 10) Then
                Dim mReceiptItemServiceInspection As ReceiptItemServiceInspection  'Added by Prashant 0n 9-Oct-2019
                For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                    For Each mReceiptItemServiceInspection In mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections 'Added by Prashant 0n 9-Oct-2019
                         If Not IsDBNull(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate) Then 'Added by Prashant 0n 9-Oct-2019
                            Return True
                            Exit Function
                        End If
                    Next
                Next
            End If
        End If
        Return False
    End Function
    Private Sub ConditionCheckItemComply()
        Dim mConditionCheckItemChildList As ConditionCheckItemChildList
        Dim mOldConditionCheckItemChild As ConditionCheckItemChild
        Dim mConditionCheckItem As ConditionCheckItem
        Dim mConditionCheckItemChild As ConditionCheckItemChild
        Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
        Dim mReceiptItemServiceInspection As ReceiptItemServiceInspection 'Added By Prashant 9-Oct-2019
        For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
            For Each mReceiptItemServiceInspection In mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections 'Added By Prashant 9-Oct-2019
                 If Not IsDBNull(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate) Then
                     mConditionCheckItemChildList = ConditionCheckItemChildList.GetConditionCheckItemChildList(FromDate:="1/1/1900", ToDate:="1/1/3300", _
                                                                                                              ItemName:=mReceiptCumInvoiceItem.ItemName, _
                                                                                                              Description:=mReceiptCumInvoiceItem.ItemDescription, _
                                                                                                              SerialNo:=mReceiptCumInvoiceItem.SerialNo, _
                                                                                                              ItemServiceInspectionsID:=mReceiptItemServiceInspection.ItemServiceInspectionsID.ToString)
                    mConditionCheckItem = ConditionCheckItem.GetConditionCheckItem(mConditionCheckItemChildList(0).ConditionCheckItemID)
                    mOldConditionCheckItemChild = ConditionCheckItemChild.GetConditionCheckItemChild(mConditionCheckItemChildList(0).ID)
                    If mOldConditionCheckItemChild.IsApplicable = True Then
                          If CDate(mOldConditionCheckItemChild.DoneOnDate) < CDate(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate) Then
                            mConditionCheckItemChild = ConditionCheckItemChild.NewComplyConditionCheckItemChild(ConditionCheckItemID:=mConditionCheckItem.ID, _
                                                                                                                DoneOnDate:=New SmartDate(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate.ToString, False), _
                                                                                                                PreviousConditionCheckItemChildID:=mOldConditionCheckItemChild.ID)
                            mConditionCheckItemChild.ItemName = mOldConditionCheckItemChild.ItemName
                            mConditionCheckItemChild.Description = mOldConditionCheckItemChild.Description
                            mConditionCheckItemChild.SerialNo = mOldConditionCheckItemChild.SerialNo
                            mConditionCheckItemChild.Frequency = mOldConditionCheckItemChild.Frequency
                            mConditionCheckItemChild.ConditionCheckIntervalIn = mOldConditionCheckItemChild.ConditionCheckIntervalIn
                             mConditionCheckItemChild.DoneOnDate = mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate
                            mConditionCheckItemChild.Location = mOldConditionCheckItemChild.Location
                            If mReceiptItemServiceInspection.ItemServiceInspectionFrequencyPeriod = 1 Then
                                mConditionCheckItemChild.NextDueDate = CDate(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate).AddDays(mOldConditionCheckItemChild.Frequency)
                            ElseIf mReceiptItemServiceInspection.ItemServiceInspectionFrequencyPeriod = 2 Then
                                mConditionCheckItemChild.NextDueDate = CDate(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate).AddMonths(mOldConditionCheckItemChild.Frequency)
                            ElseIf mReceiptItemServiceInspection.ItemServiceInspectionFrequencyPeriod = 3 Then
                                mConditionCheckItemChild.NextDueDate = CDate(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate).AddYears(mOldConditionCheckItemChild.Frequency)
                            End If
                            ConditionalItemsComply.Append("Part No. : " + mConditionCheckItemChild.ItemName + " Serial No. : " + mConditionCheckItemChild.SerialNo + " Description : " + mReceiptItemServiceInspection.ItemServiceInspectionDescription + "<BR>")
                            mConditionCheckItemChild = mConditionCheckItemChild.Save()
                        End If
                    End If
                End If
            Next 'End Added By Prashant 9-Oct-2019
        Next
        If ConditionalItemsComply.Length = 0 Then
            If Session("ShowedMSGForCalibration") = "" Then
                Session("ShowedMSGForCalibration") = ""
                If (mReceiptCumInvoice.StatusID = 2 And mReceiptCumInvoice.TransTypeID = 10) Then
                      For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                        If Not IsDBNull(mReceiptCumInvoiceItem.CalibrationDoneOnDate) Then
                            If AppSettings("ClientCode") = "STR" Then 'Added by Prashant 24-Sep-2020 STR24092020 Calibration Remove yes no message do automatic compliance with ok message
                                CalibratedItemComply()
                                Exit Sub
                            Else
								ExtraMessage = $"Receipt contains Calibrated Items.{Environment.NewLine} Do you wish to Comply ?"
								MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "CalibratedItemComply")
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            End If
        Else
            ShowMessageForConditionCheck(ConditionalItemsComply:=ConditionalItemsComply.ToString)
        End If
    End Sub
    Private Sub ShowMessage(Optional ByVal ItemsComply As String = "")
        Dim str1 As String = ""
        str1 = str1 + ("<span class=""clsLabelAuto"">Following Item(s) Comply Successfully! <BR><BR>" + ItemsComply + "</BR></span>")
        MSGBoxCtrl.show("Notify!", str1, "", MsgBoxStyle.OkOnly, "CalibrationItemComply")
        Exit Sub
    End Sub
    Private Sub ShowMessageForConditionCheck(Optional ByVal ConditionalItemsComply As String = "")
        Dim str1 As String = ""
        str1 = str1 + ("<span class=""clsLabelAuto"">Following Item(s) Comply Successfully! <BR><BR>" + ConditionalItemsComply + "</BR></span>")
        MSGBoxCtrl.show("Notify!", str1, "", MsgBoxStyle.OkOnly, "ConditionCheckItemComplied")
        Exit Sub
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False)
        If Not IsInRole(Rights.Print) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If

        Dim da As New CSLA.Data.ObjectAdapter
        'Dim rpt As New crptReceiptCumInvoice

        Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

        If CDate(mReceiptCumInvoice.RecCumInvDate) <= CDate("30-Jun-2017") Or mReceiptCumInvoice.Visibility = 3 Then
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                rpt = New crptReceiptCumInvoiceIndamar  'For Indamar
                'Added by Archana on 3-Nov-2009
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "TAAL" Then
                rpt = New crptReceiptCumInvoiceDetailPortraitTAAL
                'Added by Prashant on 21-Jun-2011
            ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "RAL") And (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10) Then
                rpt = New crptGoodReceiptNote
                '-------------------------------
                'Added by Shweta on 25-Jul-2012
            ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                rpt = New crptReceiptCumInvoiceDetailPortraitForBuddhaAir
                '-------------------------------
            ElseIf (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022  'Added By Prashant 17-Dec-2014 ALL17122014
                rpt = New crptReceiptCumInvoiceDetailPortraitDeccan
            ElseIf AppSettings("ClientCode") = "GEP" Then
                rpt = New crptReceiptCumInvoiceDetailPortraitForGEPL
            ElseIf AppSettings("ClientCode") = "HSC" Then 'HeliStar Added by Prashant HSC22082019
                rpt = New crptReceiptCumInvoiceDetailPortraitForHeliStar
            Else
                'rpt = New crptReceiptCumInvoice
                rpt = New crptReceiptCumInvoiceDetailPortrait
            End If
        Else
            rpt = New crptReceiptCumInvoiceGSTDetail
        End If

        Dim obj As rptReceiptCumInvoice
        Dim objChilds As rptReceiptCumInvoiceChildList

        Dim mCompanyInfo As rptSearchingCriteriaForReceipt

        Dim ds As New dsRecCumInvReg

        If mReceiptCumInvoice.FromTypeID = 14 Then
            If mReceiptCumInvoice.VendorName = "" Then mReceiptCumInvoice.VendorName = mVendorList.Item(mReceiptCumInvoice.VendorID).Name
        ElseIf mReceiptCumInvoice.FromTypeID = 2 Then
            If mReceiptCumInvoice.AircraftName = "" Then mReceiptCumInvoice.AircraftName = mMachineNameValueList.Item(mReceiptCumInvoice.AircraftID).RegNo 'AircraftName
        ElseIf mReceiptCumInvoice.FromTypeID = 8 Then
            If mReceiptCumInvoice.StoreName = "" Then mReceiptCumInvoice.StoreName = mStoreList.Item(mReceiptCumInvoice.StoreID).Name
        End If

        obj = rptReceiptCumInvoice.GetReceiptCumInvoice(mReceiptCumInvoice)
        ''objChilds = rptReceiptCumInvoiceChildList.GetReceiptCumInvoiceChild(mReceiptCumInvoice)
        SetUserEmailID()


        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "RAL") And (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10) Then
            objChilds = rptReceiptCumInvoiceChildList.GetReceiptCumInvoiceChild(mReceiptCumInvoice, "RAL")
            mCompanyInfo = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "", "", AppSettings("ClientCode"), "", AppSettings("Barcode") = "True", cmbCurrency.SelectedItem.Text, mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(0).OrderItemDetailForReceipt.OrderDateFormatted.ToString, "", "", mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(0).OrderItemDetailForReceipt.OrderNumber, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, AppSettings("Logo"))
        Else
            objChilds = rptReceiptCumInvoiceChildList.GetReceiptCumInvoiceChild(mReceiptCumInvoice)
            mCompanyInfo = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), FromDate:="", ToDate:="", _
                                                                                         InternalReceiptNo:=AppSettings("ClientCode"), ReleaseNoteNo:=AppSettings("IssueDate"), _
                                                                                         RecText:=AppSettings("Barcode") = "True", IssText:=Session("FormRevisionNo"), OrdText:=Session("FormRevisionDate"), _
                                                                                         RecNo:=AppSettings("FormNumberOnReceipt"), IssNo:=AppSettings("IssueNumber"), OrdNo:=AppSettings("HSNACSCodeVisibleInPartMaster"), Aircraft:="", Supplier:="", _
                                                                                         Store:="", Status:="", DCNo:="", PartNo:="", Description:="", _
                                                                                         InvText:="", InvNo:="", FromStore:="", Amend:="", QuotationNo:="", _
                                                                                         IntOrderNo:="", SerialNo:="", Charge:="", SuppInvNo:="", FromInvDate:="", ToInvDate:="", _
                                                                                         TransTypeID:=0, WorkShop:=AppSettings("Logo"), _
                                                                                         WorkOrderText:=AppSettings("PrintBarCodeOnItemDetail"), WorkOrderNo:="")
            'Replace AppSettings("RevisionNumber") ,AppSettings("RevisionDate") with Session("FormRevisionNo"), & Session("FormRevisionDate") by Shital
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, objChilds)
        da.Fill(ds, mCompanyInfo)
        da.Fill(ds, mrptImage)
        rpt.SetDataSource(ds)
		Session("CrystalReport") = rpt

		If ByMail Then

			Dim rciDetails As New Dictionary(Of String, String) From {
				{"RCI No", $"{mReceiptCumInvoice.ReceiptNo}"},
				{"RCI Date", mReceiptCumInvoice.RecCumInvDateFormatted}
			}

			Dim MailBody As String = ReportHelper.GenerateEmailBody(Details:=rciDetails,
																	ModuleName:="Receipt Cum Invoice",
																	AuthorizedBy:=Thread.CurrentPrincipal.Identity.Name,
																	AuthorizationDate:=New SmartDate(Today.Date).FormattedText)

			SendMailFile.SendMailFile(rpt:=Session("CrystalReport"),
									  UserName:=Thread.CurrentPrincipal.Identity.Name,
									  Subject:=$"{mCompanyInfo(0).CompanyName} Receipt No:- {mReceiptCumInvoice.ReceiptNo}",
									  Text:=$"{mReceiptCumInvoice.ReceiptNo}",
									  Info:=MailBody, VendorEmailID:="",
									  ToMailID:=Session("ToSendMailIDs"),
									  CCMailID:=Session("CcSendMailIDs"),
									  ReportPath:="",
									  ReportByMail:=False,
									  Remark:=Session("SendMailRemark"),
									  ReportGeneratedBy:=Session("ReportGenratedBy"),
									  SmtpHost:=Session("SmtpHost"),
									  SmtpPort:=Session("SmtpPort"),
									  SmtpUser:=Session("SmtpUser"),
									  SmtpPassword:=Session("SmtpPassword"))

		End If

	End Sub
	Public Sub SendMail()
		If AppSettings("MailsRequire") = "True" Then
			If Thread.CurrentPrincipal.Identity.Name.ToUpper = "BTPLADMIN" Or Thread.CurrentPrincipal.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
				'Do nothing
				Exit Sub
			End If
			SetReport()
			Dim str As String
			mUser = SI.UTILITY.User.GetUser(Thread.CurrentPrincipal.Identity.Name)
			mEmployeeEmailID = EmployeeEmailID.GetEmployeeEmailID(mReceiptCumInvoice.ID.ToString)
			If mEmployeeEmailID.Count > 0 Then
				If mEmployeeEmailID(0).EmployeeEmailID <> "" Then
					mEmployeeEmailIDs = mUser.UserEmail + "," + mEmployeeEmailID(0).EmployeeEmailID
				End If
			End If
			str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Receipt No.: <b> " & mReceiptCumInvoice.RecText + "-" + mReceiptCumInvoice.RecNo.ToString & "</b> Dated: <b> " + mReceiptCumInvoice.RecCumInvDateFormatted + "</b> has been Authorized By User: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>,</font></P> ")
			str = str + ("</body></html>")
			SendMailFile.SendMailFile(rpt:=Session("CrystalReport"), UserName:=Thread.CurrentPrincipal.Identity.Name, Subject:="Goods Receipt Details", Text:=mReceiptCumInvoice.RecText + "-" + mReceiptCumInvoice.RecNo.ToString, Info:=str, VendorEmailID:="", ToMailID:=mEmployeeEmailIDs, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
										SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
		End If
	End Sub
	Private Sub Print(Optional ByVal obj As rptStoresAcceptanceTag = Nothing)
		Dim pdfList As New System.Collections.ArrayList
		Dim pageCount As Integer = 0
		Dim PDFNo As Integer = 1
		Dim tmp As Integer
		Dim da As New CSLA.Data.ObjectAdapter
		Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim letter As rptLetterHead
		Dim ds As New dsStoresAcceptanceTag
		Dim mrptImage As rptImage

		Dim mmrptStoresAcceptanceTag = (From c In obj
										Where c.PartStatusID = 2
										Select c).ToList
		If mmrptStoresAcceptanceTag.Count > 0 Then

			If AppSettings("ClientCode") = "IRM" Then
				myReport = New crptUnserviceableTagForIRM
			ElseIf AppSettings("ClientCode") = "STR" Then
				myReport = New crptUnserviceableTagForStarAir 'crptQUARANTINETagForStarAir '
			ElseIf AppSettings("ClientCode") = "BAP" Then
				myReport = New crptStoreAcceptanceTagBharatAviation
			End If
			letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
			ds.Clear()
			da.Fill(ds, "rptStoresAcceptanceTag", mmrptStoresAcceptanceTag)
			da.Fill(ds, letter)
			mrptImage = rptImage.GetImage(ds)
			da.Fill(ds, "rptImage", mrptImage)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport

			Dim a As New Random
			tmp = a.Next

			Dim MyFile1 = "C:\Temp\" & "Unserviceable" & tmp & PDFNo.ToString & ".pdf"

			myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

			Dim myExportOption As CrystalDecisions.Shared.ExportOptions
			Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

			myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
			myDiskOption.DiskFileName = MyFile1
			myExportOption = myReport.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
			End With
			myReport.Export()
			myReport.Close()
			myReport.Dispose()
			GC.Collect()

			pdfList.Add(MyFile1)
			PDFNo = PDFNo + 1
		End If

		Dim mmrptSERVICEABLETag = Nothing

		If AppSettings("ClientCode") = "IRM" Then 'For IRM if item is Serviceable and Not primary category is tool i.e. 2 then
			mmrptSERVICEABLETag = (From c In obj
								   Where c.PartStatusID = 1 And (c.PrimaryCategoryID <> 2 Or c.StatusEquipment = False)
								   Select c).ToList
		Else
			mmrptSERVICEABLETag = (From c In obj
								   Where c.PartStatusID = 1
								   Select c).ToList
		End If
		If mmrptSERVICEABLETag.Count > 0 Then

			If AppSettings("ClientCode") = "IRM" Then
				myReport = New crptStoreAcceptanceTagIRM
			ElseIf AppSettings("ClientCode") = "STR" Then
				myReport = New crptStoreAcceptanceTag1  'crptQUARANTINETagForStarAir
			ElseIf AppSettings("ClientCode") = "BAP" Then
				myReport = New crptStoreAcceptanceServiceableTagBharatAviation
			End If

			letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
			ds.Clear()
			da.Fill(ds, "rptStoresAcceptanceTag", mmrptSERVICEABLETag)
			da.Fill(ds, letter)
			mrptImage = rptImage.GetImage(ds)
			da.Fill(ds, "rptImage", mrptImage)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport

			Dim a As New Random

			tmp = a.Next
			Dim MyFile1 = "C:\Temp\" & "Serviceable" & tmp & PDFNo.ToString & ".pdf"

			myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

			Dim myExportOption As CrystalDecisions.Shared.ExportOptions
			Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

			myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
			myDiskOption.DiskFileName = MyFile1
			myExportOption = myReport.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
			End With
			myReport.Export()
			myReport.Close()
			myReport.Dispose()
			GC.Collect()

			pdfList.Add(MyFile1)
			PDFNo = PDFNo + 1
		End If
		Dim mmrptRotableTag = (From c In obj
							   Where c.PartStatusID = 3
							   Select c).ToList
		If mmrptRotableTag.Count > 0 Then
			myReport = New crptRotableTagForStarAir
			letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
			ds.Clear()
			da.Fill(ds, "rptStoresAcceptanceTag", mmrptRotableTag)
			da.Fill(ds, letter)
			mrptImage = rptImage.GetImage(ds)
			da.Fill(ds, "rptImage", mrptImage)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport

			Dim a As New Random

			tmp = a.Next
			Dim MyFile1 = "C:\Temp\" & "RotableTServiceable" & tmp & PDFNo.ToString & ".pdf"

			myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

			Dim myExportOption As CrystalDecisions.Shared.ExportOptions
			Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

			myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
			myDiskOption.DiskFileName = MyFile1
			myExportOption = myReport.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
			End With
			myReport.Export()
			myReport.Close()
			myReport.Dispose()
			GC.Collect()

			pdfList.Add(MyFile1)
			PDFNo = PDFNo + 1
		End If

		Dim mmrptQUARANTINETag = (From c In obj
								  Where c.PartStatusID = 4
								  Select c).ToList
		If mmrptQUARANTINETag.Count > 0 Then
			'myReport = New crptQUARANTINETagForStarAir
			If AppSettings("ClientCode") = "IRM" Then
				myReport = New crptQuarantineTagIRM
			ElseIf AppSettings("ClientCode") = "STR" Then
				myReport = New crptQUARANTINETagForStarAir  'crptQUARANTINETagForStarAir
			End If
			letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
			ds.Clear()
			da.Fill(ds, "rptStoresAcceptanceTag", mmrptQUARANTINETag)
			da.Fill(ds, letter)
			mrptImage = rptImage.GetImage(ds)
			da.Fill(ds, "rptImage", mrptImage)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport

			Dim a As New Random

			tmp = a.Next
			Dim MyFile1 = "C:\Temp\" & "QUARANTINETServiceable" & tmp & PDFNo.ToString & ".pdf"

			myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

			Dim myExportOption As CrystalDecisions.Shared.ExportOptions
			Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

			myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
			myDiskOption.DiskFileName = MyFile1
			myExportOption = myReport.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
			End With
			myReport.Export()
			myReport.Close()
			myReport.Dispose()
			GC.Collect()

			pdfList.Add(MyFile1)
			PDFNo = PDFNo + 1
		End If

		Dim mmrptSCRAPTag = (From c In obj
							 Where c.PartStatusID = 5
							 Select c).ToList
		If mmrptSCRAPTag.Count > 0 Then
			myReport = New crptSCRAPTagForStarAir
			letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
			ds.Clear()
			da.Fill(ds, "rptStoresAcceptanceTag", mmrptSCRAPTag)
			da.Fill(ds, letter)
			mrptImage = rptImage.GetImage(ds)
			da.Fill(ds, "rptImage", mrptImage)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport

			Dim a As New Random

			tmp = a.Next
			Dim MyFile1 = "C:\Temp\" & "SCRAPTServiceable" & tmp & PDFNo.ToString & ".pdf"

			myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

			Dim myExportOption As CrystalDecisions.Shared.ExportOptions
			Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

			myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
			myDiskOption.DiskFileName = MyFile1
			myExportOption = myReport.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
			End With
			myReport.Export()
			myReport.Close()
			myReport.Dispose()
			GC.Collect()

			pdfList.Add(MyFile1)
			PDFNo = PDFNo + 1
		End If
		If AppSettings("ClientCode") = "IRM" Then
			Dim mmServiceableTagToolsEquipment = Nothing  'For IRM if item is Serviceable and primary category is tool i.e. 2 and marked as calibrated i.e. Status Equipment=1 then
			mmServiceableTagToolsEquipment = (From c In obj
											  Where c.PartStatusID = 1 And c.PrimaryCategoryID = 2 And c.StatusEquipment = True
											  Select c).ToList
			If mmServiceableTagToolsEquipment.Count > 0 Then
				If AppSettings("ClientCode") = "IRM" Then
					myReport = New crptTagServiceableTagToolsEquipment
				End If
				letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))
				ds.Clear()
				da.Fill(ds, "rptStoresAcceptanceTag", mmServiceableTagToolsEquipment)
				da.Fill(ds, letter)
				mrptImage = rptImage.GetImage(ds)
				da.Fill(ds, "rptImage", mrptImage)
				myReport.SetDataSource(ds)
				Session("CrystalReport") = myReport

				Dim a As New Random

				tmp = a.Next
				Dim MyFile1 = "C:\Temp\" & "Serviceable" & tmp & PDFNo.ToString & ".pdf"

				myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

				Dim myExportOption As CrystalDecisions.Shared.ExportOptions
				Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

				myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
				myDiskOption.DiskFileName = MyFile1
				myExportOption = myReport.ExportOptions
				With myExportOption
					.DestinationOptions = myDiskOption
					.ExportDestinationType = ExportDestinationType.DiskFile
					.ExportFormatType = ExportFormatType.PortableDocFormat
				End With
				myReport.Export()
				myReport.Close()
				myReport.Dispose()
				GC.Collect()

				pdfList.Add(MyFile1)
				PDFNo = PDFNo + 1
			End If
		End If

		Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"

		Dim filesByte As New List(Of Byte())()
		For Each file__1 As String In pdfList 'files
			filesByte.Add(File.ReadAllBytes(file__1))
		Next

		File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

		Session("CrystalReport") = MergedPath
		Session("PrintReportWithAttachment") = "True"

		Dim Files As String() = Directory.GetFiles("C:\Temp\")
		For Each file__1 As String In Files
			If file__1.ToUpper().Contains("serviceable".ToUpper()) Then
				File.Delete(file__1)
			End If
		Next
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
	End Sub
	'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
	Public Sub SetUserEmailID()
		Session("UserEmailID") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).SendToMailID
		Session("UserCcEmailID") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).SendCCMailID
		Session("MailsRequire") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).MailsRequire
		Session("SmtpHost") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).SmtpHost
		Session("SmtpPort") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).SmtpPort
		Session("SmtpUser") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).SmtpUser
		Session("SmtpPassword") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).SmtpPassword
		Session("FormRevisionNo") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).FormRevisionNo
		Session("FormRevisionDate") = mTransactionList.Item(mReceiptCumInvoice.TransTypeID).FormRevisionDate
	End Sub
	'-End
	Public Sub SendPUSHNotification(ByVal tmpReceiptCumInvoice As ReceiptCumInvoice)
		Dim PreviousStepStatus As Boolean = False

		'Step # 1: Get User Devices

		Dim mUserDeviceList As APP_UserDeviceList = APP_UserDeviceList.GetUserDeviceList(6) '6:Receipt

		If mUserDeviceList.Count = 0 Then
			PreviousStepStatus = False
		Else
			PreviousStepStatus = True
		End If

		If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------

		'Step # 2: Record PUSH Notification in the table

		Dim UserIDs(50) As Guid
		UserIDs = (From c As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList
				   Select (c.UserID)).Distinct().ToArray()

		Dim Notifications(UserIDs.Count - 1) As APP_UserNotification

		For i As Integer = 0 To UserIDs.Count - 1

			If UserIDs(i).Equals(Guid.Empty) Then Exit For

			Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)

			Try
				With mAPP_UserNotification
					.UserID = UserIDs(i)
					.SentOn = Now
					.Message = "Requested Part(s) received in:- " + tmpReceiptCumInvoice.ReceiptNo + " Dated:- " + tmpReceiptCumInvoice.RecCumInvDateFormatted + " By User:- " + Thread.CurrentPrincipal.Identity.Name '"Parts(s) has been requested by " + User.Identity.Name + " in Requisition " + mRequisitionNew.RequisitionNo + " ,Created on " + New SmartDate(mRequisitionNew.ReqDateFormatted.ToString).FormattedText + " in FlyPal System."
					.ModuleType = 6 'Requisition-Order-Receipt
					.ModuleID = mReceiptCumInvoice.ID
				End With

				mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

				Notifications(i) = mAPP_UserNotification

				PreviousStepStatus = True
			Catch ex As Exception
				PreviousStepStatus = False
			End Try
		Next

		'Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)

		If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------

		For Each Notification As APP_UserNotification In Notifications

			Dim errorcount As Integer = 0

StartStep3:

			'Step # 3: Trigger PUSH Notification

			errorcount = errorcount + 1

			System.Net.ServicePointManager.Expect100Continue = True
			System.Net.ServicePointManager.SecurityProtocol = 3072 'System.Net.SecurityProtocolType.Tls

			Dim request = TryCast(System.Net.WebRequest.Create("https://onesignal.com/api/v1/notifications"), System.Net.HttpWebRequest)

			request.KeepAlive = True
			request.Method = "POST"
			request.ContentType = "application/json; charset=utf-8"

			request.Headers.Add("authorization", "Basic YmE0YTUwZDgtMmJkYS00MjMzLWI5NjgtZTkxZmE5MzQ0NzMw")

			Dim serializer = New JavaScriptSerializer()

			'Forming Notification Detail URL
			'
			'
			Dim index As Integer = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("wfReceiptCumInvoice_Ajax.aspx")
			Dim urlNotificationDetail As String = ""
			urlNotificationDetail = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, index) + "APP/Launcher.aspx?NotificationID=" + Notification.ID.ToString + "&ModuleID=" + tmpReceiptCumInvoice.ID.ToString + "&username=" + Notification.UserName + "&EventLogSessionID=" + Guid.NewGuid.ToString + "&ModuleTypeID=5"


			Dim filterObject As Object()
			ReDim filterObject(((mUserDeviceList.Count - 1) * 2) + 1)

			Dim idx As Integer = 0
			Dim Ridx As Integer = 0
			For Each info As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList

				If Notification.UserID.Equals(info.UserID) Then


					If idx = 0 Then
						filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(0).DeviceID.ToString}
						idx = idx + 1
					Else
						Ridx = Ridx + 1

						filterObject(idx) = New With {Key .[operator] = "OR"}
						idx = idx + 1

						filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(Ridx).DeviceID.ToString}
						idx = idx + 1
					End If

				End If

			Next

			Dim obj = New With {Key .app_id = "f877b4d2-b6e5-4595-a381-87165f6e46a0", Key .contents = New With {Key .en = Notification.Message}, Key .headings = New With {Key .en = "FlyPal"}, Key .filters = filterObject, Key .data = New With {Key .url = urlNotificationDetail.ToString}}

			'---------------------

			Dim param = serializer.Serialize(obj)
			Dim byteArray As Byte() = Encoding.UTF8.GetBytes(param)

			Dim responseContent As String = Nothing

			Try

				Using writer = request.GetRequestStream()
					writer.Write(byteArray, 0, byteArray.Length)
				End Using

				Using response As System.Net.HttpWebResponse = request.GetResponse()

					Using reader = New System.IO.StreamReader(response.GetResponseStream())

						responseContent = reader.ReadToEnd()

					End Using

				End Using

			Catch ex As System.Net.WebException
				System.Diagnostics.Debug.WriteLine(ex.Message)
				System.Diagnostics.Debug.WriteLine(New System.IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd())

				If errorcount <= 3 Then GoTo StartStep3

			End Try

			System.Diagnostics.Debug.WriteLine(responseContent)
		Next

	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		If mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 57 Then   'Added By Prashant 21-May-2010 '57'
			mTypeList = TypeListForReceipt.GetTypeList("5", mReceiptCumInvoice.TransTypeID)
		Else
			mTypeList = TypeListForReceipt.GetTypeList("5", mReceiptCumInvoice.TransTypeID)
		End If

		If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "7AR") Then 'Added By Saylee 14-Oct-2024
			mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mReceiptCumInvoice.TransTypeID, RequstFor.Customer), getVendorStatus(mReceiptCumInvoice.TransTypeID, RequstFor.Supplier), IsServiceProvider:=getVendorStatus(mReceiptCumInvoice.TransTypeID, RequstFor.Supplier))
		Else
			mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mReceiptCumInvoice.TransTypeID, RequstFor.Customer), getVendorStatus(mReceiptCumInvoice.TransTypeID, RequstFor.Supplier))
		End If

		mMachineNameValueList = MachineNameValueList.GetMachineList(New SmartDate(mReceiptCumInvoice.RecCumInvDate.ToString).FormattedText, , , , , , , True, "(SELECT)".Trim, ForInventory:=True)


		mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
		cmbWorkShop.DataSource = mWorkShopList
		Session("mWorkShopList") = mWorkShopList
		mStoreList = StoreList.GetStoreList(0, "", True)
		mCurrencyList = CurrencyList.GetCurrencyList("", "", True)
		'---------Added By Utkarsh 10-Dec-2010
		mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1800").FormattedText, New SmartDate(mReceiptCumInvoice.RecCumInvDate.ToString).FormattedText, , , 2)
		'---------
		cmbReceivedFrom.DataSource = mTypeList
		cmbVendor.DataSource = mVendorList
		cmbAircraft.DataSource = mMachineNameValueList
		cmbStore.DataSource = mStoreList
		cmbCurrency.DataSource = mCurrencyList
		' ------Added By Utkarsh 10-Dec-2010
		cmbWorkOrder.DataSource = mnWOListForCombo
		'--------
		Session("mTypeList") = mTypeList
		Session("mVendorList") = mVendorList
		Session("mMachineNameValueList") = mMachineNameValueList
		Session("mStoreList") = mStoreList
		Session("mCurrencylist") = mCurrencyList
		dgReceiptCumInvoiceItem.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems
        dgReceiptCumInvoiceCharge.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceCharges
        dgItemAttachment.DataSource = mReceiptCumInvoice.FileAttachments     'Sankalp 26-09-25
        upnldgItemAttachment.DataBind() 'Sankalp 25-08-25
        Session("mReceiptCumInvoice") = mReceiptCumInvoice

		Session("mnWOListForCombo") = mnWOListForCombo

		Select Case mReceiptCumInvoice.FromTypeID
			Case 1  'Vendor 
				cmbVendor.Visible = True
				cmbAircraft.Visible = False
			Case 14
				cmbVendor.Visible = True
			Case 2 'Aircraft
				cmbAircraft.Visible = True
			Case 8 'Store
				cmbStore.Visible = True
				Dim mBaseCurrency As Currency
				mBaseCurrency = Currency.GetBaseCurrency()
				mReceiptCumInvoice.CurrencyID = mBaseCurrency.ID
				mReceiptCumInvoice.ConversionFactor = mBaseCurrency.ConversionFactor
			Case 16 'WorkShop
				cmbWorkShop.Visible = True
			Case 17 'Work Order              '-------Added By Utkarsh 09-Dec-2010
				cmbWorkOrder.Visible = True
		End Select

		txtReceiptCumInvoiceDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted.ToString
		txtDCDate.Text = mReceiptCumInvoice.DCDateFormatted.ToString
		txtVendorInvDate.Text = mReceiptCumInvoice.VendorInvoiceDateFormatted.ToString

		'cmbReceivedFrom.DataBind()
		'cmbAircraft.DataBind()
		'cmbStore.DataBind()
		'cmbCurrency.DataBind()
		DataBind()
        'cmbWorkOrder.DataBind() '--------
        SetGrid()

        cmbReceivedFrom.SelectedValue = mReceiptCumInvoice.FromTypeID
		cmbVendor.SelectedValue = mReceiptCumInvoice.VendorID.ToString
		cmbAircraft.SelectedValue = mReceiptCumInvoice.AircraftID.ToString
		cmbStore.SelectedValue = mReceiptCumInvoice.StoreID.ToString
		cmbCurrency.SelectedValue = mReceiptCumInvoice.CurrencyID.ToString
		cmbWorkShop.SelectedValue = mReceiptCumInvoice.WorkShopID.ToString
        cmbWorkOrder.SelectedValue = mReceiptCumInvoice.WOID.ToString '-- Added By Utkarsh 10-Dec-2010

    End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim CustValid As CustomValidator
		CustValid = CType(s, CustomValidator)
		If CustValid.ControlToValidate = "cmbWorkShop" Then
			If cmbReceivedFrom.SelectedIndex = 3 And cmbWorkShop.Enabled = True And cmbWorkShop.SelectedIndex <= 0 Then
				CustValid.ErrorMessage = "Select WorkShop from List"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "cmbReceivedFrom" Then
			If cmbReceivedFrom.Enabled = True And cmbReceivedFrom.SelectedIndex < 0 Then
				CustValid.ErrorMessage = "Please select 'Received From' Field."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "txtFactor" Then
			If Val(txtFactor.Text) = 0 Then
				CustValid.ErrorMessage = "Conversion factor Required."
				e.IsValid = False
			ElseIf Not IsNumeric(Val(txtFactor.Text)) And Val(txtFactor.Text) <> 0 Then
				CustValid.ErrorMessage = "Conversion factor must be numeric."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "cmbCurrency" Then
			If cmbCurrency.SelectedIndex <= 0 Then
				CustValid.ErrorMessage = "Please select Currency."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "cmbVendor" Then
			If cmbReceivedFrom.SelectedIndex = 2 And cmbVendor.Visible = True And cmbVendor.SelectedIndex <= 0 Then
				CustValid.ErrorMessage = "Please select Vendor."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "cmbAircraft" Then
			If cmbReceivedFrom.SelectedIndex = 0 And cmbAircraft.Visible = True And cmbAircraft.SelectedIndex <= 0 Then
				CustValid.ErrorMessage = "Please select Aircraft."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "cmbStore" Then
			If cmbReceivedFrom.SelectedIndex = 1 And cmbStore.Visible = True And cmbStore.SelectedIndex <= 0 Then
				CustValid.ErrorMessage = "Please select Store."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "txtRemark" Then
			If Len(Trim(txtRemark.Text)) > 100 Then
				CustValid.ErrorMessage = "Max. Length of Remark should be 100."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
			'------------------------------------------------------------' ------Added By Utkarsh 09-Dec-2010
		ElseIf CustValid.ControlToValidate = "cmbWorkOrder" Then
			If cmbWorkOrder.SelectedIndex < 0 And cmbWorkOrder.Visible = True Then
				CustValid.ErrorMessage = "Select WorkOrder from the list."
				e.IsValid = False
			Else
				e.IsValid = True
				'----------------------------------------------------------------- 
			End If
		End If
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 20-Jul-2011 For All19072011
		mOpenFrom = Request.QueryString("Type") 'Added By Prashant 3-Apr-2014 ALL03042014
		addAttributes()
		SetControlStatus(mReceiptCumInvoice.StatusID)

		If Not IsPostBack And Session("Sender") = "" Then
			'Added by Utkarsh on 19-Nov-2013 for Trans Text Series
			If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
				If mReceiptCumInvoice.IsNew Then
					mReceiptCumInvoice.InvText = Session("TransText_ForTransSeries")
					txtInvoiceText.Text = mReceiptCumInvoice.InvText
					Session("mReceiptCumInvoice") = mReceiptCumInvoice
					Session("AddTransTextSeries") = "False"
					Session.Remove("TransName_ForTransSeries")
					Session.Remove("TransText_ForTransSeries")
					Session.Remove("TransNo_ForTransSeries")
				End If

			End If
			'End
			DataFieldBind()
		End If
		SetPage()
		ControlVisibility()
		If mReceiptCumInvoice.IsRoundOff = True Then
			SetChargeGrid()
		End If
		SetGrid()
		If mReceiptCumInvoice.IsNew Then
			lblStatus.Text = "OPEN"
		End If
		'If AppSettings("OtherChargeDocket") = "True" Then
		CGrandTotal()
        'End If
        'ControlVisibilityForFileAttachment()
        TextChanged(sender, e)
	End Sub
	Private Sub btnAddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
		If IsValid Then
			Session("MachineID") = New Guid(cmbAircraft.SelectedValue) 'Added by Vikrant on 7.3.12 FORALL03052012
			Session("RCIItem") = True
			SetObject()
			Session("OpenFrom") = "1"
			mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConversionFactor = mReceiptCumInvoice.ConversionFactor
			Session("mReceiptCumInvoice") = mReceiptCumInvoice
			Session("mFromToTypeID") = CInt(IIf(mReceiptCumInvoice.FromTypeID = 14, 1, mReceiptCumInvoice.FromTypeID)) '8  'Store

			'mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID)
			'Commented on 29-jun-2020
			mFileAttach = FileAttach.NewAttachmentChild(Guid.NewGuid, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID)
			Session("mFileAttach") = mFileAttach

			Select Case mReceiptCumInvoice.TransTypeID

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
					If CType(mTransTypeID, Trans) = Util.Trans.ReceiptAgainstPuchaseOrder Then
						mPrimaryOrderType = 6 'TransListOf.Order_Outright
					ElseIf CType(mTransTypeID, Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Then
						mPrimaryOrderType = 4 'TransListOf.Order_ExchangeRepair
					End If
					mTransaction = 3 'Transaction.Order
					mFromPartList = False
					Session("mPrevTransID") = mPrevTransID
					Session("mPrimaryOrderType") = mPrimaryOrderType
					Session("mTransaction") = mTransaction
					Session("mFromPartList") = mFromPartList
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType=2")
				Case 8    'ReceivedFromOtherStore
					If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
						Session("mPrevTransID") = Guid.Empty
					Else
						Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
					End If
					Session("mPrimaryOrderType") = 4
					Session("mTransaction") = 4
					Session("mFromPartList") = False
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType= 2")
				Case 9
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 12
					Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
				Case 10
					If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
						Session("mPrevTransID") = Guid.Empty
					Else
						Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).OrderItemDetailForReceipt.OrderID
					End If
					Session("mPrimaryOrderType") = 4
					Session("mTransaction") = 3
					Session("mFromPartList") = False
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType= 2")
				Case 11
					Response.Redirect("wfPendingLoanToRecover_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
				Case 12    'LoanTaken
					If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
						Session("mPrevTransID") = Guid.Empty
					Else
						Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
					End If
					Session("mPrimaryOrderType") = 4
					Session("mTransaction") = 4
					Session("mFromPartList") = False
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType= 2")
				Case 13
					If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
						Session("mPrevTransID") = Guid.Empty
					Else
						Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
					End If
					Session("mPrimaryOrderType") = 4
					Session("mTransaction") = 4
					Session("mFromPartList") = False
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType= 2")
				Case 27, 28
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 4

					If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
						Session("mPrevTransID") = Guid.Empty
					Else
						Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
					End If
					Session("mPrimaryOrderType") = 4 'TransListOf.Order_LoanRecovery
					Session("mTransaction") = 4 'Transaction.Issue
					Session("mFromPartList") = False
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType= 2")
				Case 46, 56
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 16
					Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
				Case 47
					If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
						Session("mPrevTransID") = Guid.Empty
					Else
						Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
					End If
					Session("mPrimaryOrderType") = 4
					Session("mTransaction") = 4
					Session("mFromPartList") = False
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType= 2")
				Case 48, 50, 57 '57 Added By Prashant 21-May-2010
					Session("RCIItem") = False
					Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
				Case 53
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 14
					Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
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
					If CType(mTransTypeID, Trans) = Util.Trans.ReceiptAgainstPuchaseOrder Then
						mPrimaryOrderType = 3 'TransListOf.Order_Outright
					ElseIf CType(mReceiptCumInvoice.TransTypeID, Trans) = Util.Trans.ReceivedfromSupplierRentalLease Then   'Added By Prashant 6-Jan-2009
						mPrimaryOrderType = 5 'TransListOf.Order_Rental / Lease
					ElseIf CType(mTransTypeID, Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Then
						mPrimaryOrderType = 4 'TransListOf.Order_ExchangeRepair
					End If
					mTransaction = 3 'Transaction.Order
					mFromPartList = False
					Session("mPrevTransID") = mPrevTransID
					Session("mPrimaryOrderType") = mPrimaryOrderType
					Session("mTransaction") = mTransaction
					Session("mFromPartList") = mFromPartList
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType=2")
				Case 61        'Added by Utkarsh 10-Dec-2010
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
					Response.Redirect("wfnPendingWOListForRemoveComp_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType=2")
				Case 62 'Added by Saylee
					Session("mPrimaryOrderType") = 3
					Session("mTransaction") = 4 'Transaction.Issue
					Session("mFromPartList") = False 'True
					Session("OpenFrom") = 1
					If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
						Session("mPrevTransID") = Guid.Empty
					Else
						Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
					End If
					Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType=2")
				Case 66 ''Added By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1
					Response.Redirect("wfPartListForRCIFromAircraftAsCoreUnitReturn_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType=2")
				Case 67
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 14
					Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
				Case 73 ''Added By Prashant 10-Sep-2014 'ALL10092014
					Response.Redirect("wfReceivedFromWorkShopAsServiceablReturned_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&mType=2")
			End Select
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub btnAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
		If IsValid Then
			SetObject()
			mReceiptCumInvoice.Invoice.InvoiceCharges.Add(mReceiptCumInvoice.Invoice.ID)
			Session("mReceiptCumInvoice") = mReceiptCumInvoice
			Response.Redirect("wfInvoiceChargeRCI_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
		End If
	End Sub
	Private Sub dgReceiptCumInvoiceItem_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptCumInvoiceItem.RowCommand
		Dim mQtyBalReceived As Decimal = 0
		Select Case e.CommandName
			Case "EditView"
				Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptCumInvoiceItem.PageIndex * dgReceiptCumInvoiceItem.PageSize
				SetObject()
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex = index
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Currency = cmbCurrency.SelectedItem.Text
				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized Then
					Session("mTotalPendingItemQty") = 1
					Session("mQtyBalReceived") = 1
				Else
					If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 3 Then 'Order
						mQtyBalReceived = CDec(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.Qty)
					ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 4 Then 'Issue
						mQtyBalReceived = CDec(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.Qty)
					End If
					Session("mTotalPendingItemQty") = mQtyBalReceived
					Session("mQtyBalReceived") = mQtyBalReceived
				End If
				Session("TotalCount") = 1
				Session("RCIItem") = False    'Added By Saylee to solve DBNULL bug given by Mangal on 8th Dec-07.
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				Session("Edit") = True
				Dim tmpReceiptCumInvoice As ReceiptCumInvoice = mReceiptCumInvoice.Clone        'Added by Saylee on 7-Jun-2011
				Session("tmpReceiptCumInvoice") = tmpReceiptCumInvoice
				Session("ItemIndex") = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex   '*********************************
				'Commented by Shital on 20-Jul-2020 for multiple Attachment in RCI Item
				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAttachmentAdded Then
					' mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID)
					mFileAttach = FileAttach.GetAttachmentChild(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID)
					Session("mFileAttach") = mFileAttach
				Else
					'mFileAttach = FileAttach.NewAttachment(Guid.Empty, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID)
					mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID)
					Session("mFileAttach") = mFileAttach
				End If
				Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
			Case "DeleteRecord"
				Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptCumInvoiceItem.PageIndex * dgReceiptCumInvoiceItem.PageSize
				DeleteRecord(index)
			Case "ViewRec"
				If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
					Exit Sub
				End If
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptCumInvoiceItem.PageIndex * dgReceiptCumInvoiceItem.PageSize
				mReceiptCumInvoiceItem = mReceiptCumInvoice.ReceiptCumInvoiceItems(index)

				'Added by Shital on 29-Jun-2020
				mFileAttachments = FileAttachments.GetChildFileAttachments(mReceiptCumInvoiceItem.ID)
				Dim AttachmentCount As Integer = mFileAttachments.Count
				If AttachmentCount > 1 Then

					Session("mFileAttachments") = mFileAttachments
					Session("TransactionNameMarkLog") = "Receipt Cum Invoice Item"
					Session("TransactionName") = "Receipt Cum Invoice No.and Date"
					Session("TransactionDetails") = mReceiptCumInvoice.ReceiptNo + " & " + mReceiptCumInvoice.RecCumInvDateFormatted.ToString
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

				Else
					'------

					If mReceiptCumInvoiceItem.IsAttachmentAdded Then
						'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoiceItem.ID)
						mFileAttach = FileAttach.GetAttachmentChild(mReceiptCumInvoiceItem.ID)
						If mFileAttach.Size > 0 Then
							Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
								Dim Str1 As String
								Str1 = "openFile();"
								ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str1, True)
							End If
						End If
					Else
						MSGBoxCtrl.Show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
				End If
				'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
			Case "Attach"
				If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
					Exit Sub
				End If
				Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptCumInvoiceItem.PageIndex * dgReceiptCumInvoiceItem.PageSize
				Session("index") = index
				'If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAttachmentAdded = True Then

				'Commented on 17-jul-2020 by Shital
				'If mReceiptCumInvoice.ReceiptCumInvoiceItems(index).IsAttachmentAdded = True Then
				'    'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoice.ReceiptCumInvoiceItems(index).ID)
				'    mFileAttach = FileAttach.GetAttachmentChild(mReceiptCumInvoice.ReceiptCumInvoiceItems(index).ID)
				'Else
				'    'mFileAttach = FileAttach.NewAttachment(Guid.Empty, mReceiptCumInvoice.ReceiptCumInvoiceItems(index).ID)
				'    mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceiptCumInvoice.ReceiptCumInvoiceItems(index).ID)
				'End If
				'Session("mFileAttach") = mFileAttach
				'----
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
			Case "Remove"
				If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
					Exit Sub
				End If
				Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptCumInvoiceItem.PageIndex * dgReceiptCumInvoiceItem.PageSize
				mReceiptCumInvoice.ReceiptCumInvoiceItems(index).IsAttachmentAdded = False
				mReceiptCumInvoice.ReceiptCumInvoiceItems(index).ReceiptItem.FileAttachments.RemoveAt(0)
				dgReceiptCumInvoiceItem.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems
				dgReceiptCumInvoiceItem.DataBind()
				SetGrid()
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				'End
		End Select
	End Sub
	Private Sub dgReceiptCumInvoiceCharge_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptCumInvoiceCharge.RowCommand
		Select Case e.CommandName
			Case "EditCharge"
				Dim indx As Int32 = CInt(e.CommandArgument) + dgReceiptCumInvoiceCharge.PageIndex * dgReceiptCumInvoiceCharge.PageSize
				SetObject()
				mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentIndex = indx
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				Session("Edit") = "Edit"
				Response.Redirect("wfInvoiceChargeRCI_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
			Case "DeleteCharge"
				Dim indx As Int32 = CInt(e.CommandArgument) + dgReceiptCumInvoiceCharge.PageIndex * dgReceiptCumInvoiceCharge.PageSize
				DeleteChargeRecord(indx)
		End Select
	End Sub
	Private Sub cmbReceivedFrom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReceivedFrom.SelectedIndexChanged
		SetReceivedFromDetails(CInt(cmbReceivedFrom.SelectedValue))
		If cmbReceivedFrom.Enabled = True Then
			setFocus(cmbReceivedFrom)
		End If
	End Sub
	Private Sub cmbVendorName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles 'cmbVendorName.SelectedIndexChanged
		btnAddItem.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
		btnAddCharge.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
	End Sub
	Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
		btnAddItem.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
		btnAddCharge.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
		If cmbAircraft.Enabled = True Then
			setFocus(cmbAircraft)
		End If
		If mReceiptCumInvoice Is Nothing And cmbAircraft.SelectedIndex > 0 Then mReceiptCumInvoice.AircraftName = cmbAircraft.SelectedItem.Text
	End Sub
	Private Sub cmbWorkShop_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbWorkShop.SelectedIndexChanged
		btnAddItem.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
		btnAddCharge.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
		If cmbWorkShop.Enabled = True Then
			setFocus(cmbWorkShop)
		End If
		If mReceiptCumInvoice Is Nothing And cmbWorkShop.SelectedIndex > 0 Then mReceiptCumInvoice.WorkShopName = cmbWorkShop.SelectedItem.Text
	End Sub
	Private Sub cmbStore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStore.SelectedIndexChanged
		btnAddItem.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
		btnAddCharge.Enabled = CBool(IIf(mReceiptCumInvoice.StatusID <> 2 Or mReceiptCumInvoice.StatusID <> 4, True, False))
		If cmbStore.Enabled = True Then
			setFocus(cmbStore)
		End If
		If mReceiptCumInvoice Is Nothing And cmbStore.SelectedIndex > 0 Then mReceiptCumInvoice.StoreName = cmbStore.SelectedItem.Text
	End Sub
	Private Sub cmbCurrency_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrency.SelectedIndexChanged
		txtFactor.Text = mCurrencyList(cmbCurrency.SelectedIndex).ConversionFactor.ToString
		If cmbCurrency.Enabled = True Then
			setFocus(cmbCurrency)
		End If
		upnlValidationsummary.Update()
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		'Changed By Utkarsh On 20-Jul-2011 For All19072011
		MarkLog(Util.Action.Close, mModuleName, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		'End
		If Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromwfStockCard" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		ElseIf Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromReqItemStatusReport" Then 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
			RemoveSessions()
			mTypeList = Nothing
			mVendorList = Nothing
			mMachineNameValueList = Nothing
			mStoreList = Nothing
			mCurrencyList = Nothing
			mReceiptCumInvoice = Nothing
			mModuleName = Nothing
			Response.Redirect("Index.aspx")
		End If
		SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
		Session("IsValid") = IsValid
		'Added on 10-May-2018
		If mReceiptCumInvoice.StatusID <> 2 Then
			If mReceiptCumInvoice.IsDirty Then
				MSGBoxCtrl.Show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
				If IsValid Then
					SetObject()
				End If
			Else
				RemoveSessions()
				mTypeList = Nothing
				mVendorList = Nothing
				mMachineNameValueList = Nothing
				mStoreList = Nothing
				mCurrencyList = Nothing
				mReceiptCumInvoice = Nothing
				mModuleName = Nothing
				Response.Redirect("Index.aspx")
			End If
		Else
			If Session("IsAttachmentNotSave") = True Then
				If mReceiptCumInvoice.IsDirty And mReceiptCumInvoice.StatusID = 2 Then
					ExtraMessage = "As their is change in Attachment.Do you want to save Attchament?"
					MSGBoxCtrl.Show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveAttachment")
					If IsValid Then
						SetObject()
					End If
				Else
					RemoveSessions()
					mTypeList = Nothing
					mVendorList = Nothing
					mMachineNameValueList = Nothing
					mStoreList = Nothing
					mCurrencyList = Nothing
					mReceiptCumInvoice = Nothing
					mModuleName = Nothing
					Response.Redirect("Index.aspx")
				End If
			Else
				RemoveSessions()
				mTypeList = Nothing
				mVendorList = Nothing
				mMachineNameValueList = Nothing
				mStoreList = Nothing
				mCurrencyList = Nothing
				mReceiptCumInvoice = Nothing
				mModuleName = Nothing
				Response.Redirect("Index.aspx")
			End If
		End If
	End Sub
	Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
			Exit Sub
		End If
		SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
		Session("EditForExpiryInfo") = "True" 'Added by Vikrant FOR ALL11052012-13
        If IsValid Then
            'Added by Shital on 17-May-2021
            If mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10 Then
                If mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(0).OrderItemDetailForReceipt.OrderCurrencyID <> mReceiptCumInvoice.CurrencyID Then
                    ExtraMessage = "As Order currency is different from Receipt Currency. Do you want to continue?"
                    MSGBoxCtrl.Show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "DifferCurrency")
                Else
                    Save()
                End If
            Else
                Save()
            End If
        Else
            upnlValidationsummary.Update()
        End If

    End Sub
	Private Sub txtReceiptCumInvoiceDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReceiptCumInvoiceDate.TextChanged
		mReceiptCumInvoice.RecCumInvDate = CType(Trim(txtReceiptCumInvoiceDate.Text), Object)
		txtInvoiceText.Text = mReceiptCumInvoice.InvText
	End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
		SetReport()
		Dim Str1 As String
		Str1 = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
	End Sub
    Private Sub btnPrintTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTag.Click
        If Not IsInRole(Rights.Print) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim rpt As New crptStoreAcceptanceTag1
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As rptStoresAcceptanceTag
        Dim letter As rptLetterHead

        Dim ds As New dsStoresAcceptanceTag
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        obj = rptStoresAcceptanceTag.GetStoresAcceptanceTag(mReceiptCumInvoice.Receipt.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
                                                 "", AppSettings("WODocumentNo"),
                                                 AppSettings("WORevisionNo"), AppSettings("Barcode"),
                                                 AppSettings("ClientCode"),
                                                 SearchString4:=mModuleList.Item("Acceptance Tag").FormRevisionNo)


        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Taj" Or AppSettings("ClientCode") = "HSC" Then
            If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
                myReport = New crptStoreAcceptanceTag6
            Else
                myReport = New crptStoreAcceptanceTag6WithoutBarcode
            End If
        ElseIf AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "Heligo" Then
            'If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
            '    myReport = New crptServiceableUnserviceableTag
            'Else
            'myReport = New crptServiceableUnserviceableTag
            myReport = New crptServiceableUnserviceableTagForCE
            'End If
        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
            myReport = New crptStoreAcceptanceTagYATA
            'ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "LAMA" Then
            '    myReport = New crptServiceableUnserviceableTagForLama
        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Novo" Then
            myReport = New crptStoreAcceptanceTagNOVO
        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IRM" Or AppSettings("ClientCode") = "BAP") Then
            If AppSettings("ClientCode") = "IRM" Then
                myReport = New crptStoreAcceptanceTagIRM
            Else
                Print(obj)
                Exit Sub
            End If

        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "IND" Then
            myReport = New crptStoreAcceptanceTagIND
            'Print(obj)
            'Exit Sub
        ElseIf AppSettings("ClientCode") = "PTW" Then
            myReport = New crptStoreAcceptanceTagForPattaya
        ElseIf AppSettings("ClientCode") = "7AR" Then
            myReport = New crptStoreAcceptanceTagWithoutBarcodeFor7Air
        Else
            If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
                myReport = New crptStoreAcceptanceTag1
            Else
                myReport = New crptStoreAcceptanceTag1WithoutBarcode
            End If
        End If

        da.Fill(ds, obj)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    'Sankalp Comment 26-09-25
    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
    '        Exit Sub
    '    End If
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString
    '    If mReceiptCumInvoice.IsAttachmentAdded Then
    '        'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoice.ID)
    '        'mFileAttach = FileAttach.GetAttachmentChild(mReceiptCumInvoice.ID)
    '        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mReceiptCumInvoice.FileAttachments(0).Extension 'mFileAttach.Extension
    '        Dim fs As FileStream
    '        If File.Exists(AppSettings("DOCPath")) = False Then
    '            'Delete File if exist
    '            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mReceiptCumInvoice.FileAttachments(0).Extension)
    '            ' Create the file.
    '            fs = File.Create(path)
    '            '' Add some information to the file.
    '            fs.Write(mReceiptCumInvoice.FileAttachments(0).ImageFile, 0, mReceiptCumInvoice.FileAttachments(0).ImageFile.Length)
    '            fs.Close()
    '            Session("DOCPath") = path
    '            Dim Str As String
    '            Str = "openFile();"
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
    '        End If
    '    Enda If
    'End Sub
    'Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
    '    If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
    '        Exit Sub
    '    End If
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte
    '    mReceiptCumInvoice.IsAttachmentAdded = False
    '    mReceiptCumInvoice.FileAttachments.Remove(mReceiptCumInvoice.ID)
    '    Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '    Session("IsAttachmentNotSave") = mIsAttachmentNotSave
    '    'ControlVisibilityForFileAttachment()
    'End Sub
    Private Sub btnSentToBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSentToBill.Click 'Added by Saylee on 2-June-2010
        If (Not User.IsInRole("SentToBillView")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If ISInDate() = True Then
            If IsValid Then
                Session("IsValid") = IsValid
                mReceiptCumInvoice.IsSync = 1
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                If Session("IsValid") Then
                    Session.Remove("IsValid")
                    DataFieldBind()
                    Save()
                End If
            End If
        Else
            Dim ToDate As String
            ToDate = (New SmartDate(DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, -(Day(mReceiptCumInvoice.RecCumInvDate)), mReceiptCumInvoice.RecCumInvDate)))).FormattedText
            MSGBoxCtrl.show("Alert!", "This Transaction cannot be sent for billing. Accounts are closed upto " + ToDate, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub btnDocketCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocketCharge.Click
        mOtherChargeListByInvoiceID = OtherChargeListByInvoiceID.GetOtherChargeListByInvoiceID(mReceiptCumInvoice.InvoiceID.ToString)
        If mOtherChargeListByInvoiceID.Count = 0 Then  'Then Add new docket    'New
            mOtherCharge = OtherCharge.NewOtherCharge
            mOtherCharge.Date = Today.Date

            mOtherCharge.OtherChargeInvoices.Add(mOtherCharge.ID)
            mOtherCharge.OtherChargeInvoices.CurrentItem.InvoiceID = mReceiptCumInvoice.InvoiceID  'mOtherChargeInvoiceList(Recordno).ID
            MarkLog(Util.Action.[New], "Other Charge Docket", "", Util.ErrorType.NoError, mOtherCharge.ID, EventLogID)
        Else                                            'Then Only add new  charges for docket   'Edit
            mOtherCharge = OtherCharge.GetOtherCharge(mOtherChargeListByInvoiceID.Item(0).ID)
            mOtherCharge.MarkClean()
        End If

        Session("mOtherCharge") = mOtherCharge
        Session("mReceiptCumInvoice") = mReceiptCumInvoice  'Added By Prashant 5-Mar-2014

        Response.Redirect("wfOtherChargeDocket_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx")
    End Sub
    Private Sub chkIsRoundOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsRoundOff.CheckedChanged
        Dim Child As InvoiceCharge
        For i As Integer = mReceiptCumInvoice.ReceiptCumInvoiceCharges.Count - 1 To 0 Step -1
            Child = mReceiptCumInvoice.ReceiptCumInvoiceCharges(i)
            If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
                mReceiptCumInvoice.ReceiptCumInvoiceCharges.Remove(Child)
            End If
        Next
        dgReceiptCumInvoiceCharge.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceCharges
        dgReceiptCumInvoiceCharge.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'Coment Sankalp 26-09-25
    'Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
    '    'mFileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)

    '    ''Coommented by Shital on 17-JUl-2020
    '    'If mFileAttach.ReferenceID.Equals(mReceiptCumInvoice.ID) Then
    '    '    If mReceiptCumInvoice.IsAttachmentAdded Then
    '    '        mReceiptCumInvoice.FileAttachments(0).Size = mFileAttach.Size
    '    '        mReceiptCumInvoice.FileAttachments(0).ImageFile = mFileAttach.ImageFile
    '    '        mReceiptCumInvoice.FileAttachments(0).Extension = mFileAttach.Extension
    '    '    Else
    '    '        mReceiptCumInvoice.IsAttachmentAdded = True
    '    '        mReceiptCumInvoice.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
    '    '    End If
    '    '    'Added on 08-May-2018
    '    '    mReceiptCumInvoice.IsForAttachmentAfterAuthorized = True
    '    'Else
    '    '    If mReceiptCumInvoice.ReceiptCumInvoiceItems(mFileAttach.ReferenceID).IsAttachmentAdded Then
    '    '        mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments(0).Size = mFileAttach.Size
    '    '        mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments(0).ImageFile = mFileAttach.ImageFile
    '    '        mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments(0).Extension = mFileAttach.Extension
    '    '    Else
    '    '        mReceiptCumInvoice.ReceiptCumInvoiceItems(mFileAttach.ReferenceID).IsAttachmentAdded = True
    '    '        mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
    '    '    End If
    '    '    'Added on 08-May-2018
    '    '    mReceiptCumInvoice.IsForAttachmentAfterAuthorized = True
    '    'End If
    '    Try
    '        If mFileAttach.ReferenceID.Equals(mReceiptCumInvoice.ID) Then
    '            If mReceiptCumInvoice.IsAttachmentAdded Then
    '                mReceiptCumInvoice.FileAttachments(0).Size = mFileAttach.Size
    '                mReceiptCumInvoice.FileAttachments(0).ImageFile = mFileAttach.ImageFile
    '                mReceiptCumInvoice.FileAttachments(0).Extension = mFileAttach.Extension
    '            Else
    '                mReceiptCumInvoice.IsAttachmentAdded = True
    '                mReceiptCumInvoice.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
    '            End If
    '            'Added on 08-May-2018
    '            mReceiptCumInvoice.IsForAttachmentAfterAuthorized = True
    '        Else
    '            If Not mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.Contains(mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ID, CType(Session("FileUpload.FileName"), String)) Then

    '                mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.Add(mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ID, CType(Session("FileUpload.FileName"), String))
    '                'mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
    '                'mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.CurrentItem.Size = Session("Size")
    '                'mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.CurrentItem.Extension = Session("Extension")
    '                mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.CurrentItem.ImageFile = mFileAttach.ImageFile
    '                mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.CurrentItem.Size = mFileAttach.Size
    '                mReceiptCumInvoice.ReceiptCumInvoiceItems(CType(Session("index"), Integer)).ReceiptItem.FileAttachments.CurrentItem.Extension = mFileAttach.Extension
    '                mReceiptCumInvoice.IsForAttachmentAfterAuthorized = True

    '                Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '                Session.Remove("Size")
    '                Session.Remove("ImageFile")
    '                Session.Remove("Extension")
    '                Session.Remove("FileUpload.FileName")

    '            Else
    '                Session("mReceiptCumInvoice") = mReceiptCumInvoice
    '                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            End If
    '        End If
    '    Catch ex As Exception
    '    End Try

    '    'ControlVisibilityForFileAttachment()
    '    dgReceiptCumInvoiceItem.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems
    '    dgReceiptCumInvoiceItem.DataBind()
    '    SetGrid()
    '    Session("IsAttachmentNotSave") = True
    'End Sub
    'Added By Vikrant On 09-Dec-2014 For ALL09122014-1
    'Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
    '    If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
    '        Exit Sub
    '    End If
    '    If mReceiptCumInvoice.IsAttachmentAdded = True Then
    '        'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoice.ID)
    '        mFileAttach = FileAttach.GetAttachmentChild(mReceiptCumInvoice.ID)
    '    Else
    '        'mFileAttach = FileAttach.NewAttachment(Guid.Empty, mReceiptCumInvoice.ID)
    '        mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceiptCumInvoice.ID)
    '    End If

    '    Session("mFileAttach") = mFileAttach
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    'End Sub
    Private Sub btnSaveAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAttachment.Click
        mReceiptCumInvoice.UpdateReceiptAttachment(txtRemark.Text.Trim)
        'mReceiptCumInvoice.UserName = User.Identity.Name
        '-----------
        'mReceiptCumInvoice.Save() 'Commented on 10-May-2018
        mIsAttachmentNotSave = False
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
        'AttachMyFile()
        'mReceiptCumInvoice.UpdateReceiptAttachment(txtRemark.Text.Trim) 'Comment 26-09-25

        'mReceiptCumInvoice.Remark = txtRemark.Text.Trim
        'If IsValid Then
        '    mReceiptCumInvoice.Save()
        'Else
        '    upnlValidationsummary.Update()
        'End If
        '--------
        SetObject()
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        'UpdateReceiptAttachment()
        MarkLog(Action.Save, mModuleName, "Attachment", ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
        SetGrid()
        upnlReceiptCumInvItems.Update()
        MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully, MSGBox.Message_Text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
    End Sub
    'End
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '   Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        SetUserEmailID()
        '--------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub

#End Region

#Region " Status "
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If IsValid Then
            If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
                ExtraMessage = "<Strong> Goods Receipt </Strong>"
            Else
                ExtraMessage = "<Strong> Goods Receipt </Strong>"
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, ExtraMessage, MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If IsValid Then
            Dim IsInUse As IsInUse = IsInUse.GetIsInUseReceiptINIssue(mReceiptCumInvoice.ID)
            If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
                ExtraMessage = "<Strong>Goods Receipt, It is used in Issue</Strong>"
            Else
                ExtraMessage = "<Strong>Goods Receipt, It is used in Issue</Strong>"
            End If
            If IsInUse.IsInUse Then
                MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, ExtraMessage, MsgBoxStyle.OkOnly, "StatusCancel")
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                Exit Sub
            End If

            If AppSettings("ClientCode") = "CE" Then 'Added By Prashant 15-Apr-2014  'ALL15042014
                ExtraMessage = "<Strong> Goods Receipt </Strong>"
            Else
                ExtraMessage = "<Strong> Goods Receipt </Strong>"
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, ExtraMessage, MsgBoxStyle.YesNo, "StatusCancel")
            Session("IsValid") = IsValid
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
        End If
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        SetObject()
        If mReceiptCumInvoice.IsValid = False Then
            For i As Integer = 0 To mReceiptCumInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mReceiptCumInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
        If mReceiptCumInvoice.ReceiptCumInvoiceItems.IsValid = False Then
            For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
                For i As Integer = 0 To mReceiptCumInvoiceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mReceiptCumInvoiceItem.ItemName + " : " + mReceiptCumInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
        Dim str As String() = contextKey.Split("¿")
        Dim mTransTypeID As Integer = CInt(str(0).Substring(str(0).IndexOf("=") + 1))
        Dim mOrderDate As String = str(1).Substring(str(1).IndexOf("=") + 1)
        mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, , True, mTransTypeID, mOrderDate)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
#End Region

#Region "MultipleAttachment"
    'Sankalp 26-09-25
    Private Sub btnSelectFiles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        SetObject()
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
	Private Sub Attachments_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgItemAttachment.RowCommand

		Try

			Dim Index As Integer = CInt(e.CommandArgument) + dgItemAttachment.PageSize * dgItemAttachment.PageIndex

			Select Case e.CommandName
				Case "View"

					mFileAttachments = mReceiptCumInvoice.FileAttachments

					AttachmentHelper.DownloadAttachmentWithName(Index:=Index,
													   ModuleName:="Multiple Attachments",
													   AttachmentObject:=mFileAttachments)

					ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

					dgItemAttachment.DataSource = mReceiptCumInvoice.FileAttachments
					dgItemAttachment.DataBind()
					upnlItemAttachment.Update()
					upnldgItemAttachment.Update()

				Case "Remove"

					mFileAttachments = mReceiptCumInvoice.FileAttachments

					If mFileAttachments.Count = 1 Then

						DeleteAttachment(0)
						mReceiptCumInvoice.IsAttachmentAdded = False
						Session("IsAttachmentDeleted") = IsAttachmentDeleted

					Else
						DeleteAttachment(Index - 1)
					End If

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem, MSGBox.Message_Text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mReceiptCumInvoice.FileAttachments.CurrentIndex = Index
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
    End Sub

    Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        mReceiptCumInvoice.IsAttachmentAdded = True
        upnlItemAttachment.Update()
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
    End Sub
    Private Sub AttachMyFile()

		Try

			If Not mReceiptCumInvoice.FileAttachments.Contains(mReceiptCumInvoice.ID, CType(Session("FileUpload.FileName"), String)) Then

				mReceiptCumInvoice.FileAttachments.Add(mReceiptCumInvoice.ID, CType(Session("FileUpload.FileName"), String))
				mReceiptCumInvoice.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
				mReceiptCumInvoice.FileAttachments.CurrentItem.Size = Session("Size")
				mReceiptCumInvoice.FileAttachments.CurrentItem.Extension = Session("Extension")
				mReceiptCumInvoice.FileAttachments.CurrentItem.FileName = CType(Session("FileUpload.FileName"), String)

				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				Session("AttachmentName") = CType(Session("FileUpload.FileName"), String)

				dgItemAttachment.DataSource = mReceiptCumInvoice.FileAttachments
				dgItemAttachment.DataBind()

				For i As Integer = 0 To mReceiptCumInvoice.FileAttachments.Count - 1

					Dim txtValue As TextBox
					txtValue = CType(Me.dgItemAttachment.Rows(i).FindControl("txtFileName"), TextBox)
					txtValue.Text = mReceiptCumInvoice.FileAttachments(i).FileName

				Next

				Session.Remove("Size")
				Session.Remove("ImageFile")
				Session.Remove("Extension")
				Session.Remove("FileUpload.FileName")
				upnlItemAttachment.Update()
				upnldgItemAttachment.Update()

			Else
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class