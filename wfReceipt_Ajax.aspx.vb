Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class wfReceipt_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mReceipt As Receipt
    Public mVendorList As VendorList
    Public mStatusList As StatusList
    Private mReceiptItem As ReceiptItem
    Public mDocumentTypeForID As Integer ''Added By Saylee on 4th July 2007
    Public mAttachToID As Guid
    Public mName As String
    Public mIsNew As Boolean
    Public mPrevTransID As Guid = Guid.Empty
    Public mPrimaryOrderType As Integer
    Public mTransaction As Integer
    Public mFromPartList As Boolean
    Public Flag As Integer
    Dim EventLogID As Guid 'Added By Utkarsh On 20-Jul-2011 For All19072011
    Dim mReceiptDetails As String
    Public mTransTypeID As Trans
    Public mModuleName As String 'End
    Dim mOpenFrom As String 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mFileAttachments As FileAttachments
    Dim ExtraMessage As String = ""
    Dim ItemsComply As StringBuilder = New StringBuilder
    Dim ConditionalItemsComply As StringBuilder = New StringBuilder
    Dim mEmployeeEmailID As EmployeeEmailID
    Dim mEmployeeEmailIDs As String = String.Empty
    Dim mUser As User
    Public mIsAttachmentNotSave As Boolean = True
    Dim email As Thread
	Dim mTransactionList As TransactionList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
	Dim mModuleList As ModuleList
#End Region

#Region " Business Methods "
	Private Sub GetSession()
        mReceipt = CType(Session("mReceipt"), Receipt)
        mVendorList = CType(Session("mVendorList"), VendorList)
        mStatusList = CType(Session("mStatusList"), StatusList)
        mTransTypeID = CType(Session("mTransTypeID"), Integer) 'Added By Utkarsh On 21-Jul-2011 For All19072011
        mModuleName = Session("mModuleName") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
		mTransactionList = Session("mTransactionList")  'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
		mModuleList = Session("mModuleList")
	End Sub
    Private Sub SetSession()
        Session("mReceipt") = mReceipt
        Session("mVendorList") = mVendorList
        Session("mStatusList") = mStatusList
        Session("mModuleName") = mModuleName   'Added By Utkarsh On 21-Jul-2011 For All19072011
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReceipt")
        Session.Remove("mVendorList")
        Session.Remove("mStatusList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub SetPage()
        If mReceipt.IsNew Then
            If mReceipt.TransTypeID = 67 Then
                lblReceipt.Text = "Receipt against Supplier-None  [New]"
            Else
                lblReceipt.Text = "Receipt against Purchase Order [New]"
            End If
        Else
            If mReceipt.TransTypeID = 67 Then
                lblReceipt.Text = "Receipt against Supplier-None [" & mReceipt.Text & "-" & mReceipt.No & "]"
            Else
                lblReceipt.Text = "Receipt against Purchase Order [" & mReceipt.Text & "-" & mReceipt.No & "]"
            End If
        End If
    End Sub
    Private Function IsRecQtyExceedsOrderQty() As Boolean 'ALL30082018
        'Dim mOrder As Order = Order.GetOrder(mReceipt.OrderID)
        For i As Integer = 0 To mReceipt.ReceiptItems.Count - 1
            Dim mOrderItemDetailForReceipt As OrderItemDetailForReceipt = OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(mReceipt.ReceiptItems(i).OrderItemID)
            Dim TotalRecQty As Decimal
            TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(mReceipt.ReceiptItems(i).OrderItemID, mReceipt.ReceiptItems(i).ID.ToString)
            'Commented and Added By Prashant 5-Feb-2019 ALL04022019
            'If mReceipt.ReceiptItems(i).Qty > mOrderItemDetailForReceipt.Qty - TotalRecQty And Not mReceipt.ReceiptItems(i).IsSerialized Then
            ' If mReceipt.ReceiptItems(i).DisplayQty > mOrderItemDetailForReceipt.Qty - TotalRecQty And Not mReceipt.ReceiptItems(i).IsSerialized Then
            If mReceipt.ReceiptItems(i).DisplayQty > CDec(Format(mOrderItemDetailForReceipt.Qty - TotalRecQty, "##0.00##")) And Not mReceipt.ReceiptItems(i).IsSerialized Then
                Return True
            End If
        Next
        Return False
    End Function 'End
    Private Sub Save()
        'Authentication
        If Not mReceipt.RecdDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mReceipt.RecdDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Receipt-Cum-Invoice. <br> Receipt-Cum-Invoice Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If AppSettings("LockBackDatedTransaction") = "True" Then 'Added By Vikrant On 24-July-2014 For BA24072014
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        If mReceipt.StatusID <> 2 Then
                            If CheckDateForTransactionLock(mReceipt.RecdDate) Then
                                MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mReceipt.RecdDate).AddMonths(1)), Month(CDate(mReceipt.RecdDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        End If
                    End If
                End If 'End
            End If
        End If
        Dim ReceiptClone As Receipt
        ReceiptClone = mReceipt.Clone
        Try
            'check whether min. one item & charge is present while saving
            If Not mReceipt.ReceiptItems.Count = 0 Then
                'save the object
                SetObject()
                If mReceipt.IsValid Then
                    Dim i As Integer
                    While i < mReceipt.ReceiptItems.Count
                        If mReceipt.ReceiptItems.Item(i).IsSerialized = True Then
                            If mReceipt.ReceiptItems.Item(i).DuplicateSerialNo() = True Then
                                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Serial number already exist. You can not add Duplicate.", MsgBoxStyle.OkOnly, "Status")
                                Exit Sub
                            End If
                            If (mReceipt.ReceiptItems.Item(i).PrimaryCategoryID = 2 And AppSettings("CodeNo") = "True") Then
                                If (mReceipt.TransTypeID = 6) Then
                                    If (mReceipt.ReceiptItems.Item(i).DuplicateCodeNo(1) = True) Then 'Or mReceipt.ReceiptItems.Item(i).DuplicateCodeNo(2) = True) Then '1 Duplication checking with CodeNo only
                                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. entered for item  " + mReceipt.ReceiptItems.Item(i).ItemName + " (" + mReceipt.ReceiptItems.Item(i).SerialNo + ") " + " already exist.  Please enter another Code No.", MsgBoxStyle.OkOnly, "Status")
                                        Exit Sub
                                    End If
                                Else
                                    If (mReceipt.ReceiptItems.Item(i).DuplicateCodeNo(2) = True Or mReceipt.ReceiptItems.Item(i).DuplicateCodeNo(3) = True Or mReceipt.ReceiptItems.Item(i).DuplicateCodeNo(4) = True) Then '2 Duplication checking with CodeNo,ItemID,Serail No.
                                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. entered for item  " + mReceipt.ReceiptItems.Item(i).ItemName + " (" + mReceipt.ReceiptItems.Item(i).SerialNo + ") " + " already exist.  Please enter another Code No.", MsgBoxStyle.OkOnly, "Status")
                                        Exit Sub
                                    Else
                                        'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Code No. already exist for item. " + mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(i).ItemName + " You can not add Duplicate Code No.", MsgBoxStyle.OkOnly, "")
                                        'Exit Sub
                                    End If
                                End If
                            End If
                        End If
                        i = i + 1
                    End While
                    mReceipt.ApplyEdit()

                    'Added by Utkarsh on 19-Nov-2013 FOr TransTextSeries 
                    'Check if ReceiptCumInvoiceText is blank then call TransTextSeries UI

                    If (mReceipt.IsNew) And (mReceipt.Text = "") Then

                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mReceipt.TransTypeID, mReceipt.RecdDateFormatted)

                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceipt.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceipt.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceipt.TransTypeID).TransText = "")) Then

                            Dim str = "<script language='javascript'>openledgersame('wfReceipt_Ajax.aspx?BackPage=index.aspx');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Receipt"
                            Session("TransTypeID_ForTransSeries") = mReceipt.TransTypeID
                            Session("TransDate_ForTransSeries") = mReceipt.RecdDateFormatted
                            MSGBoxCtrl.show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "ReceiptTransTextSeriesAlert")
                            Exit Sub
                        Else
                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then
                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceipt.TransTypeID)
                                    mReceipt.Text = .TransText
                                    mReceipt.No = .StartingTransNo
                                End With
                            Else
                                Dim str = "<script language='javascript'>openledgersame('wfReceipt_Ajax.aspx?BackPage=index.aspx');</script>"

                                Session("BackPagestr_ForTransSeries") = str

                                Session("TransName_ForTransSeries") = "Receipt"
                                Session("TransTypeID_ForTransSeries") = mReceipt.TransTypeID
                                Session("TransDate_ForTransSeries") = mReceipt.RecdDateFormatted
                                MSGBoxCtrl.show("RCI Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "ReceiptTransTextSeriesAlert")
                                Exit Sub
                            End If
                        End If

                    End If
                    'End
                    'ALL30082018
                    If mReceipt.TransTypeID = Util.Trans.ReceiptAgainstPuchaseOrder Then
                        If IsRecQtyExceedsOrderQty() Then
                            MSGBoxCtrl.show("Pending Quantity Alert!", "Receipt Qty. is greater than Order Qty.<BR>If you click Yes, existing Order alongwith Order Amount will get updated.<BR><BR>Do you want to continue?", "", MsgBoxStyle.YesNo, "ExcessQtyHandle")
                            Exit Sub
                        End If

                    End If
                    'End
                    mReceipt.Save()
                    '------------------------------------------
                    If mReceipt.IsAttachmentAdded Then
                        If mReceipt.FileAttachments(0).Size > 0 Then
                            ImageButton1.Visible = True
                        End If

                    End If
                    '------------------------------------------
                    'Changed By Utkarsh On 20-Jul-2011 For All19072011
                    'mReceiptDetails = mReceipt.ReceiptNo + " Dated : " + mReceipt.RecdDateFormatted + " from " + mVendorList(mReceipt.VendorID).Name
                    'Added by Prashant  16-Jul-2013 'ALL15072013
                    If Session("Note") <> "" Then
                        mReceiptDetails = mReceipt.ReceiptNo + " Dated : " + mReceipt.RecdDateFormatted + " from " + mVendorList(mReceipt.VendorID).Name + " Note:- " + Session("Note")
                        Session.Remove("Note")
                    Else
                        mReceiptDetails = mReceipt.ReceiptNo + " Dated : " + mReceipt.RecdDateFormatted + " from " + mVendorList(mReceipt.VendorID).Name
                    End If
                    '-------------------------------------------
                    Select Case mReceipt.StatusID
                        Case 1
                            MarkLog(Util.Action.Save, mModuleName, mReceiptDetails, Util.ErrorType.NoError, mReceipt.ID, EventLogID)
                        Case 2
                            SendMailIfAlternateReceive()
                            SendReqPartsMail() 'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
                            SendMail()
                            MarkLog(Util.Action.Authorize, mModuleName, mReceiptDetails, Util.ErrorType.NoError, mReceipt.ID, EventLogID)
                        Case 3
                            MarkLog(Util.Action.Amend, mModuleName, mReceiptDetails, Util.ErrorType.NoError, mReceipt.ID, EventLogID)
                        Case 4
                            MarkLog(Util.Action.Cancel, mModuleName, mReceiptDetails, Util.ErrorType.NoError, mReceipt.ID, EventLogID)

                    End Select
                    'End
                    mReceipt.MarkClean()
                    'lblReceipt.Text = "Receipt (saved.....)"
                    Session("mReceipt") = mReceipt
                    DataFieldBind()
                    ControlVisibility()
                    SetControlStatus(mReceipt.StatusID)
                    ControlVisibilityForFileAttachment()
                    SetPage()
                    upnlTitle.Update()
                    upnlStatusName.Update()
                    upnlReceiptCumInvoiceDetails.Update()
                    upnlReceivedFrom.Update()
                    upnlReceiptItems.Update()
                    upnlButtons.Update()
                    If mReceipt.StatusID = 2 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.AuthorizedSuccessFully, MSGBox.Message_text.AuthorizedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    ElseIf mReceipt.StatusID = 4 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    Else
                        MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    End If
                Else
                    mReceipt = ReceiptClone
                    SetObject()
                    Session("mReceipt") = mReceipt
                    DataFieldBind()
                    Exit Sub
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Receipt can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                mReceipt = ReceiptClone
                SetObject()
                Session("mReceipt") = mReceipt
                DataFieldBind()
                Exit Sub
            End If
        Catch ex As SqlClient.SqlException
            mReceipt = ReceiptClone
            Session("mReceipt") = mReceipt
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    If InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Receipt Qty can not be greater than Order Qty.</br></br><b>Please amend Purchase Order for Receipt of excess quantity.</b>", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If
            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex1.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "Status")
                mReceipt = ReceiptClone
                SetObject()
                Session("mReceipt") = mReceipt
                DataFieldBind()
                Exit Sub
            ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Receipt Qty can not be greater than Order Qty.</br><b>Please amend Purchase Order Quantity & make Receipt again.</b>", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Finally
            ReceiptClone = Nothing
        End Try
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()
        If txtReceiptDate.Text = "" Then
            mReceipt.RecdDate = Today.Date
        Else
            mReceipt.RecdDate = CDate(txtReceiptDate.Text)
        End If
        mReceipt.Text = txtText.Text
        mReceipt.No = Val(txtNo.Text)
        mReceipt.IntReceiptNo = Trim(txtIntReceiptNo.Text)
        mReceipt.FromTypeID = 1
        If txtDCDate.Text = "" Then
            mReceipt.DCDate = System.DBNull.Value
        Else
            mReceipt.DCDate = CDate(txtDCDate.Text)
        End If
        mReceipt.DCNO = Trim(txtDCNo.Text)
        mReceipt.AWBNo = txtAWBNo.Text
        mReceipt.UserName = User.Identity.Name
        If mReceipt.TransTypeID = 67 Then 'Added by Prashant 5-Dec-2018 ALL05122018 
            mReceipt.VendorID = New Guid(cmbVendorName.SelectedValue)
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mReceipt.ReceiptItems.CurrentIndex = Index
        Session("mReceipt") = mReceipt
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mReceipt = CType(Session("mReceipt"), Receipt)
                            mReceipt.ReceiptItems.Remove(mReceipt.ReceiptItems.CurrentItem)
                            mReceipt.ReceiptItems.CurrentIndex = mReceipt.ReceiptItems.Count - 1
                            For i As Integer = 0 To mReceipt.ReceiptItems.Count - 1
                                mReceipt.ReceiptItems(i).SrNo = i + 1
                            Next
                            dgReceiptItems.DataSource = mReceipt.ReceiptItems
                            dgReceiptItems.DataBind()
                            SetGrid()
                            upnlReceiptItems.Update()
                            Session("mReceipt") = mReceipt
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If mReceipt.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("ReceiptPONew")) And (Not User.IsInRole("ReceiptPOEdit")) Then
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
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mReceipt.IsValid = True Then
                            Session.Remove("IsValid")
                            mReceipt.StatusID = 2
                            DataFieldBind()
                            Save()
                            '-----------------------------------------------------------
                            If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                                Dim mReceiptItem As ReceiptItem
                                For Each mReceiptItem In mReceipt.ReceiptItems
                                    If Not IsDBNull(mReceiptItem.CalibrationDoneOnDate) Then
                                        ExtraMessage = "As Receipt Contains Calibrated Items. Do you want to comply it?"
                                        MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "CalibratedItemComply")
                                        Session("ShowedMSGForCalibrationFormReceipt") = "Showed MSG For Calibration Form Receipt"
                                        Exit Sub
                                    End If
                                Next
                            End If
                            'If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                            '    Dim mReceiptItem As ReceiptItem
                            '    For Each mReceiptItem In mReceipt.ReceiptItems
                            '        If Not IsDBNull(mReceiptItem.ConditionCheckDoneOnDate) Then
                            '            ExtraMessage = "As Receipt Contains Condition Check Items. Do you want to comply it?"
                            '            MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                            '            Session("ShowedMSGForConditionCheckFormReceipt") = "Showed MSG For Condition Check Form Receipt"
                            '            Exit Sub
                            '        End If
                            '    Next
                            'End If
                            ''Added by Shital on 13-Sep-2019
                            'If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                            '    Dim mReceiptItem As ReceiptItem
                            '    For Each mReceiptItem In mReceipt.ReceiptItems
                            '        If Not IsDBNull(mReceiptItem.ServiedInspectedCheckDoneOnDate) Then
                            '            ExtraMessage = "As Receipt Contains Serviced Inspected Items. Do you want to comply it?"
                            '            MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                            '            Session("ShowedMSGForConditionCheckFormReceipt") = "Showed MSG For Condition Check Form Receipt"
                            '            Exit Sub
                            '        End If
                            '    Next
                            'End If
                            '-----------------------------------------------------------
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    ElseIf MSGBoxCtrl.Sender = "ExcessQtyHandle" Then 'ALL30082018
                        If mReceipt.IsValid = True Then
                            If (Not User.IsInRole("OrderNew")) And (Not User.IsInRole("OrderEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user to update Purchase Order", False), True)
                                Exit Sub
                            End If
                            Dim mOrder As Order '= Order.GetOrder(mReceipt.OrderID)
                            For i As Integer = 0 To mReceipt.ReceiptItems.Count - 1
                                Dim mOrderItemDetailForReceipt As OrderItemDetailForReceipt = OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(mReceipt.ReceiptItems(i).OrderItemID)
                                mOrder = Order.GetOrder(mOrderItemDetailForReceipt.OrderID)
                                Dim TotalRecQty As Decimal
                                TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(mReceipt.ReceiptItems(i).OrderItemID, mReceipt.ReceiptItems(i).ID.ToString)
                                'Commented and Added By Prashant 5-Feb-2019 ALL04022019
                                'If mReceipt.ReceiptItems(i).Qty > mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).Qty - TotalRecQty Then
                                If mReceipt.ReceiptItems(i).DisplayQty > mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).Qty - TotalRecQty Then
                                    Dim OldOrderItemQty As Decimal = mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).Qty
                                    'Commented and Added By Prashant 5-Feb-2019 ALL04022019
                                    'Dim NewOrderItemQty As Decimal = OldOrderItemQty + (mReceipt.ReceiptItems(i).Qty - (mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).Qty - TotalRecQty))
                                    Dim NewOrderItemQty As Decimal = OldOrderItemQty + (mReceipt.ReceiptItems(i).DisplayQty - (mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).Qty - TotalRecQty))
                                    mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).Qty = NewOrderItemQty
                                    If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 7 Then
                                        mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).OrderItemQuotationItems(0).Qty = NewOrderItemQty
                                    End If
                                    mOrder.OrderItems(mReceipt.ReceiptItems(i).OrderItemID).Note = "Order Item Qty. updated to " + NewOrderItemQty.ToString + " from " + OldOrderItemQty.ToString + " by automatic process through Receipt" 'on " + Today.Date.ToString(AppSettings("DateFormat"))
                                    mReceipt.ReceiptItems(i).ExcessQty = NewOrderItemQty - OldOrderItemQty
                                End If
                            Next
                            mOrder.CalculateTotal()
                            mOrder.Save()
                            MarkLog(Util.Action.Save, "Order", "Order Qty. Updated by " + User.Identity.Name + " on " + Today.Date.ToString, Util.ErrorType.NoError, mOrder.ID, EventLogID)

                            Session.Remove("IsValid")
                            'DataFieldBind()
                            If (Not User.IsInRole("ReceiptPONew")) And (Not User.IsInRole("ReceiptPOEdit")) Then
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
                        End If 'End
                    End If
                    If MSGBoxCtrl.Sender = "CalibratedItemComply" Then
                        CalibratedItemComply()
                    End If
                    If MSGBoxCtrl.Sender = "ConditionCheckItemComply" Then
                        ConditionCheckItemComply()
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mReceipt.StatusID = 4
                        DataFieldBind()
                        Save()
                    End If
                    If MSGBoxCtrl.Sender = "SaveAttachment" Then
                        mReceipt.UpdateReceiptAttachment(mReceipt.FileAttachments)
                        mIsAttachmentNotSave = False
                        Session("ReceiptIsAttachmentNotSave") = mIsAttachmentNotSave
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then

                    End If
                    If MSGBoxCtrl.Sender = "SaveAttachment" Then
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
                        If mReceipt.StatusID = 2 Then
                            mReceipt.StatusID = 1
                        ElseIf mReceipt.StatusID = 4 Then
                            mReceipt.StatusID = 2
                        End If
                        Session("mReceipt") = mReceipt
                        DataFieldBind()
                    ElseIf MSGBoxCtrl.Sender = "ReceiptTransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    ElseIf MSGBoxCtrl.Sender = "CalibrationItemComply" Then
                        If Session("ShowedMSGForConditionCheckFormReceipt") = "" Then
                            Session("ShowedMSGForConditionCheckFormReceipt") = ""
                            If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                                'Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                                For Each mReceiptItem In mReceipt.ReceiptItems
                                    If Not IsDBNull(mReceiptItem.ConditionCheckDoneOnDate) Then
                                        ExtraMessage = "As Receipt Contains Condition Check Items. Do you want to comply it?"
                                        MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                                        Exit Sub
                                    End If
                                Next
                            End If
                            'Added by Shital on 13-Sep-2019
                            If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                                'Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                                For Each mReceiptItem In mReceipt.ReceiptItems
                                    If Not IsDBNull(mReceiptItem.ServiedInspectedCheckDoneOnDate) Then
                                        ExtraMessage = "As Receipt Contains Serviced Inspected Items. Do you want to comply it?"
                                        MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                                        Exit Sub
                                    End If
                                Next
                            End If
                        End If
                    ElseIf MSGBoxCtrl.Sender = "ConditionCheckItemComplied" Then
                        If Session("ShowedMSGForCalibrationFormReceipt") = "" Then
                            Session("ShowedMSGForCalibrationFormReceipt") = ""
                            If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                                'Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                                For Each mReceiptItem In mReceipt.ReceiptItems
                                    'If mReceiptCumInvoiceItem.CalibrationDoneOnDate <> "" Then
                                    If Not IsDBNull(mReceiptItem.CalibrationDoneOnDate) Then
                                        ExtraMessage = "As Receipt Contains Calibrated Items. Do you want to comply it?"
                                        MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "CalibratedItemComply")
                                        Exit Sub
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
        btnSave.Visible = IIf(StatusId > 1, False, True)
        If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
            dgReceiptItems.Columns(16).HeaderText = "GSE No." '' Ajay 21 => 20 | 20 => 19 | 19 => 18 | 18 => 16 | 16 => 14
        End If
        If (mOpenFrom = "FromwfStockCard" Or mOpenFrom = "FromReqItemStatusReport") Then
            'dgReceiptItems.Columns(22).Visible = False  'Edit
            'dgReceiptItems.Columns(24).Visible = False  'Attach
            'dgReceiptItems.Columns(25).Visible = False 'Remove Attachment
            'Ajay
            dgReceiptItems.Columns(15).Visible = False  'Edit/Delete        22 => 21 | 21 => 20 | 20 => 19 | 19 => 17 | 17 => 15
            dgReceiptItems.Columns(16).Visible = False  'Attach             23 => 22 | 22 => 21 | 21 => 20 | 20 => 18 | 18 => 16
            dgReceiptItems.Columns(17).Visible = False 'Remove Attachment   24 => 23 | 23 => 22 | 22 => 21 | 21 => 19 | 19 => 17
        Else
            'dgReceiptItems.Columns(24).Visible = IIf(StatusId = 2, True, False) 'Attach
            'dgReceiptItems.Columns(25).Visible = IIf(StatusId = 2, True, False) 'Remove Attachment
            'Ajay
            dgReceiptItems.Columns(16).Visible = IIf(StatusId = 2, True, False) 'Attach            23 => 22 | 22 => 21 | 21 => 20 | 20 => 18 | 18 => 16
            dgReceiptItems.Columns(17).Visible = IIf(StatusId = 2, True, False) 'Remove Attachment 24 => 23 | 23 => 22 | 22 => 21 | 21 => 19 | 19 => 17
        End If
        'dgReceiptItems.Columns(23).Visible = IIf(StatusId > 1, False, True) 'Remove
        'Ajay
        'dgReceiptItems.Columns(15).Visible = IIf(StatusId > 1, False, True) 'Remove 22 => 21 | 21 => 20 | 20 => 19 | 19 => 17 | 17 => 15
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub ControlVisibility()
        btnAddItem.Enabled = IIf(mReceipt.StatusID > 1, False, True)
        txtDCDate.Enabled = (mReceipt.StatusID = 1)
        btnAuthorized.Visible = (Not mReceipt.ReceiptItems.Count = 0) And (Not mReceipt.IsNew) And (mReceipt.StatusID = 1)
        btnCancel.Visible = (Not mReceipt.IsNew) And (mReceipt.StatusID = 2)
        txtText.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean)) ''And mEnquiry.EnquiryItems.Count = 0) Or (mEnquiry.EnquiryItems.Count = 0)
        txtNo.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean)) '' And mEnquiry.EnquiryItems.Count = 0) Or (mEnquiry.EnquiryItems.Count = 0)
        txtReceiptDate.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean) And mReceipt.ReceiptItems.Count = 0) Or (mReceipt.ReceiptItems.Count = 0)
        txtDCDate.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean))
        txtDCNo.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean))
        txtIntReceiptNo.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean))
        cmbVendorName.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean) And mReceipt.ReceiptItems.Count = 0) Or (mReceipt.ReceiptItems.Count = 0)
        txtAWBNo.Enabled = (CType(IIf(mReceipt.StatusID >= 2, False, True), Boolean))
        btnPrint.Enabled = Not mReceipt.IsNew
        'btnPrintTag.Enabled = Not mReceipt.IsNew
        'Added By Prashant 17-Aug-2011
        If Not User.IsInRole("ReceiptPOAuthorized") Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
            btnSaveAttachment.Enabled = False
            btnSaveAttachment.ToolTip = "You are not authorized user "
        End If
        'If (Not User.IsInRole("ReceiptPOAuthorized") And AppSettings("ClientCode") = "Deccan") Then
        '    btnSelectFile.Disabled = True
        '    btnDelAttach.Enabled = False
        '    btnDelAttach.ToolTip = "You are not authorized user "
        '    ImageButton1.Enabled = False
        '    ImageButton1.ToolTip = "You are not authorized user "
        'End If
        '-----------------------------
        btnSaveAttachment.Visible = (mReceipt.StatusID = 2)
        btnSendMail.Visible = (mReceipt.StatusID = 2)
        'Code No Ajay 21 => 20 | 20 => 19 | 19 => 18 | 18 => 16 | 16 => 14
        dgReceiptItems.Columns(14).Visible = IIf((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA"), True, False)
        btnPrintTag.Enabled = (Not mReceipt.IsNew) AndAlso (AppSettings("ToAllowPrintTagForOpenReceipt") = "True" Or (AppSettings("ToAllowPrintTagForOpenReceipt") = "False" And mReceipt.StatusID = 2))
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        Dim deletebtn As ImageButton
        Dim Editbtn As ImageButton
        For j As Integer = 0 To dgReceiptItems.Rows.Count - 1
            P = CType(Me.dgReceiptItems.Rows.Item(j).Cells(18).Text, Boolean) ''   Ajay 25 => 24 | 24 => 23 | 23 => 22 | 22 => 20 | 20 => 18

            deletebtn = CType(Me.dgReceiptItems.Rows(j).Cells(15).FindControl("ImgDeleteRecord"), ImageButton)
            Editbtn = CType(Me.dgReceiptItems.Rows(j).Cells(15).FindControl("ImgEditView"), ImageButton)

            If P Then
                dgReceiptItems.Rows(j).Cells(17).Enabled = True  'Remove Attachment     24 => 23 | 23 => 22 | 22 => 21 | 21 => 19 | 19 => 17
            Else
                dgReceiptItems.Rows(j).Cells(17).Enabled = False  '' Ajay               24 => 23 | 23 => 22 | 22 => 21 | 21 => 19 | 19 => 17
            End If

            ''Aded by Ajay on 1-Mar-2023, to make visiblility as per StatusID
            deletebtn.Visible = IIf(mReceipt.StatusID > 1, False, True)
        Next
        upnlReceiptItems.Update()
    End Sub
    'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
    Private Sub SendReqPartsMail()
        If AppSettings("MailsRequire") = "True" Then
            If Thread.CurrentPrincipal.Identity.Name.ToUpper = "BTPLADMIN" Or Thread.CurrentPrincipal.Identity.Name.ToUpper = "BYTZADMIN" Then
                'Do nothing
                Exit Sub
            End If
            Dim RecItems
            If mReceipt.ReceiptItems.Count > 0 Then
                RecItems = (From c As ReceiptItem In mReceipt.ReceiptItems
                          Where Not c.ReqEmployeeEmailIDs = ""
                          Select c).ToList
            End If
            If RecItems.count > 0 Then
                Dim strGeneratedReport As String = ""
                Dim EmailIDs As New StringBuilder
                For i As Integer = 0 To RecItems.Count - 1
                    If Not EmailIDs.ToString.Contains(RecItems(i).ReqEmployeeEmailIDs) Then
                        EmailIDs.Append(RecItems(i).ReqEmployeeEmailIDs + ",")
                    End If

                Next
                strGeneratedReport = GenerateReportBodyForReqParts(RecItems)
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Requested Part(s) received", mReceipt.ReceiptNo, ToMailID:=EmailIDs.ToString.TrimEnd(","), Info:=strGeneratedReport, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                        SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
            End If
        End If
    End Sub
    Private Function GenerateReportBodyForReqParts(ByVal RecItems) As String
        Dim str As String = ""
        str = str + ("<p><font face=""Calibri"">Following Requested Part(s) received in <b> " + mReceipt.ReceiptNo + "</b> Dated <b> " + mReceipt.RecdDateFormatted.ToString + "</b></font></p>")
        str = str + ("<p><font face=""Calibri"">by User : <b>" + Thread.CurrentPrincipal.Identity.Name + " </b></font></p>")
        str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
        str = str + ("<tr>" & "<td align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requested Part No.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Serial No.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requisition No.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requisition Date</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requested Qty.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Receipt Qty.</b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Requested By</b>" & "</font>" & "</td></tr>")

        Dim srNo As Integer = 1
        Dim i As Integer = 0
        'Dim ReceiptItem
        For Each RecItem As ReceiptItem In RecItems
            str = str + ("<TR>")
            str = str + ("<TD WIDTH=20px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + (srNo.ToString)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=80px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RecItem.ItemName
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RecItem.SerialNo
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RecItem.ReqNo
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RecItem.ReqDate.ToString
            str = str + ("</font>")
            str = str + ("</TD>")

            'str = str + ("<TD WIDTH=70px align=""left"">")
            'str = str + ("<font face=""Calibri"">")
            'str = str + RecItem.ReqQty.ToString
            'str = str + ("</font>")
            'str = str + ("</TD>")
            Dim RowSpanCount As Integer = 0

            RowSpanCount = (From RCIItemInfo As ReceiptItem In mReceipt.ReceiptItems
                                                                    Where RCIItemInfo.ItemID = RecItems(i).ItemID
                                                                    Select RCIItemInfo).Count()
            If i = 0 Then
                str = str + ("<TD WIDTH=70px align=""left"" rowspan=" + RowSpanCount.ToString + ">")
                str = str + ("<font face=""Calibri"">")
                str = str + RecItems(i).ReqQty.ToString
                str = str + ("</font>")
                str = str + ("</TD>")
            Else
                If RecItems(i).ReqItemID.Equals(RecItems(i - 1).ReqItemID) Then
                    'str = str + ("<TD id=""tdReqQty""" + (i + 1).ToString + " WIDTH=70px align=""left"">")
                    'str = str + ("<font face=""Calibri"">")
                    'str = str + ""
                    'str = str + ("</font>")
                    'str = str + ("</TD>")
                Else
                    str = str + ("<TD WIDTH=70px align=""left"" rowspan=" + RowSpanCount.ToString + ">")
                    str = str + ("<font face=""Calibri"">")
                    str = str + RecItems(i).ReqQty.ToString
                    str = str + ("</font>")
                    str = str + ("</TD>")
                End If
            End If

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + CDec(Format(RecItem.Qty, "##0.00##")).ToString
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=70px align=""left"">")
            str = str + ("<font face=""Calibri"">")
            str = str + RecItem.ReqEmployeeName
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
            Dim Alternate
            If mReceipt.ReceiptItems.Count > 0 Then
                Alternate = (From c As ReceiptItem In mReceipt.ReceiptItems
                          Where Not c.AlternateItemID.Equals(Guid.Empty)
                          Select c).ToList
            End If

            If Alternate.count > 0 Then
                Dim strGeneratedReport As String = ""
                strGeneratedReport = GenerateReportBody(Alternate)
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Alternate Part(s) received", mReceipt.ReceiptNo, Info:=strGeneratedReport, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
            End If
        End If
    End Sub
    Private Function GenerateReportBody(ByVal Alternate) As String 'Added by utkarsh on 16-sep-2013
        Dim str As String = ""
        str = str + ("<p><font face=""Calibri"">Following Alternate Part(s) received in <b> " + mReceipt.ReceiptNo + "</b> Dated <b> " + mReceipt.RecdDateFormatted + "</b></font></p>")
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
    End Function
    Private Function CheckDateForTransactionLock(ByVal TransDate As Date) As Boolean 'Added By Vikrant On 24-July-2014 For BA24072014
        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
        If (TransDate >= FirstDayofLastMonth) Then
            If (TransDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                If mReceipt.StatusID = 4 Then
                    mReceipt.StatusID = 2
                    Session("mReceipt") = mReceipt
                End If
                Return True
            Else
                Return False
            End If
        Else
            If mReceipt.StatusID = 4 Then
                mReceipt.StatusID = 2
                Session("mReceipt") = mReceipt
            End If
            Return True
        End If
    End Function
    Private Sub ControlVisibilityForFileAttachment()
        If mReceipt.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = IIf(mReceipt.StatusID > 2, False, True)
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
        upnlAttachFile.Update()
    End Sub
    Private Sub CalibratedItemComply()
        Dim mCalibrationItemChildList As CalibrationItemChildList
        Dim mOldCalibrationItemChild As CalibrationItemChild
        Dim mCalibrationItem As CalibrationItem
        Dim mCalibrationItemChild As CalibrationItemChild

        Dim mReceiptItem As ReceiptItem
        For Each mReceiptItem In mReceipt.ReceiptItems
            If Not IsDBNull(mReceiptItem.CalibrationDoneOnDate) Then
                mCalibrationItemChildList = CalibrationItemChildList.GetCalibrationChildList(FromDate:="1/1/1900", ToDate:="1/1/3300", ItemName:=mReceiptItem.ItemName, Description:=mReceiptItem.ItemDescription, SerialNo:=mReceiptItem.SerialNo)
                mCalibrationItem = CalibrationItem.GetCalibrationItem(mCalibrationItemChildList(0).CalibrationItemID)
                mOldCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(mCalibrationItemChildList(0).ID)
                If mOldCalibrationItemChild.IsApplicable = True Then
                    If CDate(mOldCalibrationItemChild.DoneOnDate) < CDate(mReceiptItem.CalibrationDoneOnDate) Then
                        mCalibrationItemChild = CalibrationItemChild.NewComplyCalibrationItemChild(CalibrationItemID:=mCalibrationItem.ID, CalDoneOnDate:=mReceiptItem.CalibrationDoneOnDate.ToString, PreviousCalibrationItemChildID:=mOldCalibrationItemChild.ID)
                        mCalibrationItemChild.ItemName = mOldCalibrationItemChild.ItemName
                        mCalibrationItemChild.Description = mOldCalibrationItemChild.Description
                        mCalibrationItemChild.SerialNo = mOldCalibrationItemChild.SerialNo
                        mCalibrationItemChild.Frequency = mOldCalibrationItemChild.CalibrationItemChildFrequency
                        mCalibrationItemChild.CalibrationPeriodInID = mOldCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID
                        mCalibrationItemChild.CalibrationItemChildFrequency = mOldCalibrationItemChild.CalibrationItemChildFrequency
                        mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = mOldCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID
                        mCalibrationItemChild.DoneOnDate = mReceiptItem.CalibrationDoneOnDate
                        mCalibrationItemChild.Location = mOldCalibrationItemChild.Location
                        'If mCalibrationItemChild.CalibrationPeriodInID = 1 Then
                        If mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 1 Then
                            mCalibrationItemChild.NextDueDate = CDate(mReceiptItem.CalibrationDoneOnDate).AddDays(mOldCalibrationItemChild.CalibrationItemChildFrequency)
                        ElseIf mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 2 Then
                            mCalibrationItemChild.NextDueDate = CDate(mReceiptItem.CalibrationDoneOnDate).AddMonths(mOldCalibrationItemChild.CalibrationItemChildFrequency)
                        ElseIf mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 3 Then
                            mCalibrationItemChild.NextDueDate = CDate(mReceiptItem.CalibrationDoneOnDate).AddYears(mOldCalibrationItemChild.CalibrationItemChildFrequency)
                        End If
                        ItemsComply.Append("Part No. : " + mCalibrationItemChild.ItemName + " Serial No. : " + mCalibrationItemChild.SerialNo + "<BR>")
                        mCalibrationItemChild = mCalibrationItemChild.Save()
                    End If
                End If
            End If
        Next
        If ItemsComply.Length = 0 Then
            If Session("ShowedMSGForConditionCheckFormReceipt") = "" Then
                Session("ShowedMSGForConditionCheckFormReceipt") = ""
                If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                    'Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                    For Each mReceiptItem In mReceipt.ReceiptItems
                        If Not IsDBNull(mReceiptItem.ConditionCheckDoneOnDate) Then
                            ExtraMessage = "As Receipt Contains Condition Check Items. Do you want to comply it?"
                            MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                            Exit Sub
                        End If
                    Next
                End If

                'Added by Shital on 13-Sep-2019
                If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
                    'Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
                    For Each mReceiptItem In mReceipt.ReceiptItems
                        If Not IsDBNull(mReceiptItem.ServiedInspectedCheckDoneOnDate) Then
                            ExtraMessage = "As Receipt Contains Serviced Inspected Items. Do you want to comply it?"
                            MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "ConditionCheckItemComply")
                            Exit Sub
                        End If
                    Next
                End If
            End If
        Else
            ShowMessage(ItemsComply:=ItemsComply.ToString)
        End If
    End Sub
    Private Sub ConditionCheckItemComply()
        Dim mConditionCheckItemChildList As ConditionCheckItemChildList
        Dim mOldConditionCheckItemChild As ConditionCheckItemChild
        Dim mConditionCheckItem As ConditionCheckItem
        Dim mConditionCheckItemChild As ConditionCheckItemChild

        Dim mReceiptItem As ReceiptItem
        For Each mReceiptItem In mReceipt.ReceiptItems
            If Not IsDBNull(mReceiptItem.ConditionCheckDoneOnDate) Then
                mConditionCheckItemChildList = ConditionCheckItemChildList.GetConditionCheckItemChildList(FromDate:="1/1/1900", ToDate:="1/1/3300", ItemName:=mReceiptItem.ItemName, Description:=mReceiptItem.ItemDescription, SerialNo:=mReceiptItem.SerialNo)
                mConditionCheckItem = ConditionCheckItem.GetConditionCheckItem(mConditionCheckItemChildList(0).ConditionCheckItemID)
                mOldConditionCheckItemChild = ConditionCheckItemChild.GetConditionCheckItemChild(mConditionCheckItemChildList(0).ID)
                If mOldConditionCheckItemChild.IsApplicable = True Then
                    If CDate(mOldConditionCheckItemChild.DoneOnDate) < CDate(mReceiptItem.ConditionCheckDoneOnDate) Then
                        mConditionCheckItemChild = ConditionCheckItemChild.NewComplyConditionCheckItemChild(ConditionCheckItemID:=mConditionCheckItem.ID, DoneOnDate:=New SmartDate(mReceiptItem.ConditionCheckDoneOnDate.ToString, False), PreviousConditionCheckItemChildID:=mOldConditionCheckItemChild.ID)
                        mConditionCheckItemChild.ItemName = mOldConditionCheckItemChild.ItemName
                        mConditionCheckItemChild.Description = mOldConditionCheckItemChild.Description
                        mConditionCheckItemChild.SerialNo = mOldConditionCheckItemChild.SerialNo
                        mConditionCheckItemChild.Frequency = mOldConditionCheckItemChild.Frequency
                        mConditionCheckItemChild.ConditionCheckIntervalIn = mOldConditionCheckItemChild.ConditionCheckIntervalIn
                        mConditionCheckItemChild.DoneOnDate = mReceiptItem.ConditionCheckDoneOnDate
                        mConditionCheckItemChild.Location = mOldConditionCheckItemChild.Location
                        If mConditionCheckItemChild.ConditionCheckIntervalIn = 1 Then
                            mConditionCheckItemChild.NextDueDate = CDate(mReceiptItem.ConditionCheckDoneOnDate).AddDays(mOldConditionCheckItemChild.Frequency)
                        ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 2 Then
                            mConditionCheckItemChild.NextDueDate = CDate(mReceiptItem.ConditionCheckDoneOnDate).AddMonths(mOldConditionCheckItemChild.Frequency)
                        ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 3 Then
                            mConditionCheckItemChild.NextDueDate = CDate(mReceiptItem.ConditionCheckDoneOnDate).AddYears(mOldConditionCheckItemChild.Frequency)
                        End If
                        ConditionalItemsComply.Append("Part No. : " + mConditionCheckItemChild.ItemName + " Serial No. : " + mConditionCheckItemChild.SerialNo + "<BR>")
                        mConditionCheckItemChild = mConditionCheckItemChild.Save()
                    End If
                End If
            End If
        Next
        If ConditionalItemsComply.Length = 0 Then
            'If Session("ShowedMSGForCalibrationFormReceipt") = "" Then
            '    Session("ShowedMSGForCalibrationFormReceipt") = ""
            '    If (mReceipt.StatusID = 2 And mReceipt.TransTypeID = 10) Then
            '        'Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
            '        For Each mReceiptItem In mReceipt.ReceiptItems
            '            'If mReceiptCumInvoiceItem.CalibrationDoneOnDate <> "" Then
            '            If Not IsDBNull(mReceiptItem.CalibrationDoneOnDate) Then
            '                ExtraMessage = "As Receipt Contains Calibrated Items. Do you want to comply it?"
            '                MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "CalibratedItemComply")
            '                Exit Sub
            '            End If
            '        Next
            '    End If
            'End If
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
        If Not User.IsInRole("ReceiptPOPrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

        If AppSettings("ClientCode") = "RAL" Then
            rpt = New crptReceiptDetailPortraitInd
            'Added by Shweta on 25-Jul-2012
        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            rpt = New crptReceiptDetailPortraitForBuddhaAir
            '-------------------------------
        Else
            rpt = New crptReceiptDetailPortrait
        End If

        Dim obj As rptReceipts
        Dim objChilds As rptReceiptParts
        Dim letter As rptLetterHead

        Dim ds As New dsReceipt
        obj = rptReceipts.GetReceipts(mReceipt.ID)
        objChilds = rptReceiptParts.GetReceiptParts(mReceipt.ID)
        Dim mSearchstring As String         'Addded by vikrant on 5-sept-2011
        If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
            mSearchstring = "True"
        Else
            mSearchstring = "False"
        End If
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", mSearchstring, AppSettings("Logo"), _
                                                 AppSettings("PrintBarCodeOnItemDetail"), ClientCode:=AppSettings("ClientCode"))
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        If ByMail = True Then
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Receipt No.: <b> " & mReceipt.ReceiptNo & "</b> Dated: <b> " + mReceipt.RecdDateFormatted + "</b> has been Authorized By User: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>.</font></P> ")
            str = str + ("<p></b> Copy is attached for your information and planning.</font></p>")
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Goods Receipt Details", Text:=mReceipt.ReceiptNo, Info:=str, _
                                      VendorEmailID:="", ToMailID:=Session("ToSendMailIDs"), CCMailID:=Session("CcSendMailIDs"), ReportPath:="", _
                                      ReportByMail:=False, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
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
            mEmployeeEmailID = EmployeeEmailID.GetEmployeeEmailID(mReceipt.ID.ToString)
            If mEmployeeEmailID.Count > 0 Then
                If mEmployeeEmailID(0).EmployeeEmailID <> "" Then
                    mEmployeeEmailIDs = mUser.UserEmail + "," + mEmployeeEmailID(0).EmployeeEmailID
                End If
            End If
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Receipt No.: <b> " & mReceipt.ReceiptNo & "</b> Dated: <b> " + mReceipt.RecdDateFormatted + "</b> has been Authorized By User: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>,</font></P> ")
            str = str + ("</body></html>")
            SendMailFile.SendMailFile(rpt:=Session("CrystalReport"), UserName:=Thread.CurrentPrincipal.Identity.Name, Subject:="Goods Receipt Details", Text:=mReceipt.ReceiptNo, Info:=str, VendorEmailID:="", ToMailID:=mEmployeeEmailIDs, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
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
            ' myReport = New crptUnserviceableTagForStarAir 'crptQUARANTINETagForStarAir '
            If AppSettings("ClientCode") = "IRMI" Then
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

        If AppSettings("ClientCode") = "IRMI" Then 'For IRM if item is Serviceable and Not primary category is tool i.e. 2 then
            mmrptSERVICEABLETag = (From c In obj
                           Where c.PartStatusID = 1 And (c.PrimaryCategoryID <> 2 Or c.StatusEquipment = False)
                           Select c).ToList
        Else
            mmrptSERVICEABLETag = (From c In obj
                           Where c.PartStatusID = 1
                           Select c).ToList
        End If

        If mmrptSERVICEABLETag.Count > 0 Then
            'myReport = New crptStoreAcceptanceTag1 'crptQUARANTINETagForStarAir
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
            myReport = New crptQUARANTINETagForStarAir
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

        If AppSettings("ClientCode") = "IRMI" Then
            Dim mmServiceableTagToolsEquipment = Nothing  'For IRM if item is Serviceable and primary category is tool i.e. 2 and marked as calibrated i.e. Status Equipment=1 then
            mmServiceableTagToolsEquipment = (From c In obj
                               Where c.PartStatusID = 1 And c.PrimaryCategoryID = 2 And c.StatusEquipment = True
                               Select c).ToList
            If mmServiceableTagToolsEquipment.Count > 0 Then
                If AppSettings("ClientCode") = "IRMI" Then
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
    Public Sub SetUserMailIDs()
        Session("UserEmailID") = mTransactionList.Item(mReceipt.TransTypeID).SendToMailID
        Session("UserCcEmailID") = mTransactionList.Item(mReceipt.TransTypeID).SendCCMailID
        Session("MailsRequire") = mTransactionList.Item(mReceipt.TransTypeID).MailsRequire
        Session("SmtpHost") = mTransactionList.Item(mReceipt.TransTypeID).SmtpHost
        Session("SmtpPort") = mTransactionList.Item(mReceipt.TransTypeID).SmtpPort
        Session("SmtpUser") = mTransactionList.Item(mReceipt.TransTypeID).SmtpUser
        Session("SmtpPassword") = mTransactionList.Item(mReceipt.TransTypeID).SmtpPassword
        Session("FormRevisionNo") = mTransactionList.Item(mReceipt.TransTypeID).FormRevisionNo
        Session("FormRevisionDate") = mTransactionList.Item(mReceipt.TransTypeID).FormRevisionDate
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "7AR") Then 'Added By Saylee 14-Oct-2024
            mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", True, IsCustomer:=False, IsSupplier:=True, IsServiceProvider:=True)
        Else
            mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", True, IsCustomer:=False, IsSupplier:=True)
        End If

        cmbVendorName.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        dgReceiptItems.DataSource = mReceipt.ReceiptItems
        Session("mReceipt") = mReceipt
        txtReceiptDate.Text = mReceipt.RecdDateFormatted.ToString
        txtDCDate.Text = mReceipt.DCDateFormatted.ToString

        txtReceiptDate.DataBind()
        txtText.DataBind()
        txtNo.DataBind()
        txtIntReceiptNo.DataBind()
        cmbVendorName.DataBind()
        txtDCNo.DataBind()
        txtDCDate.DataBind()
        txtAWBNo.DataBind()
        dgReceiptItems.DataBind()
        lblStatus.DataBind()
        SetGrid()
        cmbVendorName.SelectedValue = mReceipt.VendorID.ToString
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "txtReceiptDate" Then
            If Len(txtReceiptDate.Text) = 0 Then
                CustValid.ErrorMessage = " Please Select Receipt Date "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "txtInternalReceiptNo" Then
            If Len(txtIntReceiptNo.Text) > 50 Then
                CustValid.ErrorMessage = " Max Length of Internal Receipt No should be 50. "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "txtDCNo" Then
            If Len(txtDCNo.Text) > 25 Then
                CustValid.ErrorMessage = " Max Length of DC No should be 25."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf CustValid.ControlToValidate = "cmbVendorName" Then
            If cmbVendorName.SelectedIndex <= 0 Then
                CustValid.ErrorMessage = "Please Select Supplier"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 20-Jul-2011 For All19072011
        addAttributes()
        SetControlStatus(mReceipt.StatusID)
        mOpenFrom = Request.QueryString("Type") 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report

        If Not IsPostBack And Session("Sender") = "" Then
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mReceipt.IsNew Then
                    mReceipt.Text = Session("TransText_ForTransSeries")
                    txtText.Text = mReceipt.Text
                    Session("mReceipt") = mReceipt
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
        SetGrid()
        SetSession()
        If mReceipt.IsNew Then
            lblStatus.Text = "OPEN"
        End If
        ControlVisibilityForFileAttachment()
    End Sub
    Private Sub btnAddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        If IsValid Then
            SetObject()
            mReceipt.ReceiptItems.Add(mReceipt.ID, mReceipt.TransTypeID)
            mReceipt.ReceiptItems.CurrentIndex = mReceipt.ReceiptItems.Count - 1
            Session("mReceipt") = mReceipt

            mFileAttach = FileAttach.NewAttachmentChild(Guid.NewGuid, mReceipt.ReceiptItems.CurrentItem.ID)
            Session("mFileAttach") = mFileAttach

            If CType(mReceipt.TransTypeID, Trans) = Util.Trans.RCIFromSupplierAsNone Then 'Added by Prashant 5-Dec-2018 ALL05122018 
                Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx")
            Else
                If (mReceipt.ReceiptItems.Count = 0) Or (mReceipt.ReceiptItems.Count = 1 And mReceipt.ReceiptItems.CurrentItem.IsNew) Then
                    mPrevTransID = Guid.Empty
                Else
                    mPrevTransID = mReceipt.ReceiptItems.Item(mReceipt.ReceiptItems.Count - 2).OrderItemDetailForReceipt.OrderID
                End If
                If CType(mReceipt.TransTypeID, Trans) = Util.Trans.ReceiptAgainstPuchaseOrder Then 'mPrimaryOrderType = 3 'TransListOf.Order_Outright'Changes by Kalpesh Shah
                    mPrimaryOrderType = 3 'TransListOf.Order_Outright
                ElseIf CType(mReceipt.TransTypeID, Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Then
                    mPrimaryOrderType = 4 'TransListOf.Order_ExchangeRepair
                End If
                mTransaction = 3 'Transaction.Order
                mFromPartList = False
                Session("OpenFrom") = "1"
                Session("mPrevTransID") = mPrevTransID
                Session("mPrimaryOrderType") = mPrimaryOrderType
                Session("mTransaction") = mTransaction
                Session("mFromPartList") = mFromPartList
                Dim str As String
                str = "openledgersame('wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx&mType= 1');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgReceiptItems_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptItems.RowCommand
        Dim mQtyBalReceived As Decimal = 0
        Select Case e.CommandName
            Case "EditView"
                'Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptItems.PageIndex * dgReceiptItems.PageSize
                'mReceipt.ReceiptItems.CurrentIndex = index
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Dim index As Int32 = gvr.RowIndex
                mReceipt.ReceiptItems.CurrentIndex = index

                SetObject()
                If mReceipt.ReceiptItems.CurrentItem.IsSerialized Then
                    Session("mTotalPendingItemQty") = 1
                    Session("mQtyBalReceived") = 1
                Else
                    If mReceipt.ReceiptItems.CurrentItem.FromItemTypeID = 3 Then 'Order
                        mQtyBalReceived = CDec(mReceipt.ReceiptItems.CurrentItem.OrderItemDetailForReceipt.Qty)
                    ElseIf mReceipt.ReceiptItems.CurrentItem.FromItemTypeID = 4 Then 'Issue
                        mQtyBalReceived = CDec(mReceipt.ReceiptItems.CurrentItem.IssueItemDetailForReceipt.Qty)
                    End If
                    Session("mTotalPendingItemQty") = mQtyBalReceived
                    Session("mQtyBalReceived") = mQtyBalReceived
                End If
                Session("TotalCount") = 1
                Session("mReceipt") = mReceipt
                Session("Edit") = True
                Dim tmpReceipt As Receipt = mReceipt.Clone
                Session("tmpReceipt") = tmpReceipt
                Session("ItemIndex") = mReceipt.ReceiptItems.CurrentIndex
                If mReceipt.ReceiptItems.CurrentItem.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachmentChild(mReceipt.ReceiptItems.CurrentItem.ID)
                    Session("mFileAttach") = mFileAttach
                Else
                    mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceipt.ReceiptItems.CurrentItem.ID)
                    Session("mFileAttach") = mFileAttach
                End If
                Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx")
            Case "DeleteRecord"
                'Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptItems.PageIndex * dgReceiptItems.PageSize
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Dim index As Int32 = gvr.RowIndex
                DeleteRecord(index)
            Case "ViewRec"
                If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                ' Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptItems.PageIndex * dgReceiptItems.PageSize
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Dim index As Int32 = gvr.RowIndex
                mReceiptItem = mReceipt.ReceiptItems(index)

                'Added by Shital on 26-Oct-2020
                mFileAttachments = FileAttachments.GetChildFileAttachments(mReceiptItem.ID)
                Dim AttachmentCount As Integer = mFileAttachments.Count
                If AttachmentCount > 1 Then

                    Session("mFileAttachments") = mFileAttachments
                    Session("TransactionNameMarkLog") = "Receipt Item"
                    Session("TransactionName") = "Receipt No.and Date"
                    Session("TransactionDetails") = mReceipt.ReceiptNo + " & " + mReceipt.RecdDateFormatted.ToString
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

                Else
                    '------
                    If mReceiptItem.IsAttachmentAdded Then
                        'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoiceItem.ID)
                        mFileAttach = FileAttach.GetAttachmentChild(mReceiptItem.ID)
                        'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mManual.FileExtension
                        Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
                        If mFileAttach.Size > 0 Then
                            If File.Exists(AppSettings("DOCPath")) = False Then
                                Dim fs As FileStream
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
                        MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If
            Case "Attach"
                If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptItems.PageIndex * dgReceiptItems.PageSize
                Session("index") = index
                If mReceipt.ReceiptItems(index).IsAttachmentAdded = True Then
                    mFileAttach = FileAttach.GetAttachmentChild(mReceipt.ReceiptItems(index).ID)
                Else
                    mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceipt.ReceiptItems(index).ID)
                End If
                Session("mFileAttach") = mFileAttach
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
            Case "Remove"
                If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim index As Int32 = CInt(e.CommandArgument) + dgReceiptItems.PageIndex * dgReceiptItems.PageSize
                mReceipt.ReceiptItems(index).IsAttachmentAdded = False
                mReceipt.ReceiptItems(index).FileAttachments.RemoveAt(0)
                dgReceiptItems.DataSource = mReceipt.ReceiptItems
                dgReceiptItems.DataBind()
                SetGrid()
                Session("mReceipt") = mReceipt
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 20-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, mModuleName, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        If Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromwfStockCard" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        ElseIf Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromReqItemStatusReport" Then 'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
            RemoveSession()
            mVendorList = Nothing
            mStatusList = Nothing
            mReceipt = Nothing
            Response.Redirect("Index.aspx")
        End If
        SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Session("IsValid") = IsValid
        'Added on 10-May-2018 If Condition
        If mReceipt.StatusID <> 2 Then
            If mReceipt.IsDirty Then
                MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
                If IsValid Then
                    SetObject()
                End If
            Else
                RemoveSession()
                mVendorList = Nothing
                mStatusList = Nothing
                mReceipt = Nothing
                Response.Redirect("index.aspx")
            End If
        Else
            If Session("ReceiptIsAttachmentNotSave") = True Then
                If mReceipt.IsDirty And mReceipt.StatusID = 2 Then
                    ExtraMessage = "As their is change in Attachment.Do you want to save Attchament?"
                    MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, ExtraMessage, MsgBoxStyle.YesNo, "SaveAttachment")
                    If IsValid Then
                        SetObject()
                    End If
                Else
                    RemoveSession()
                    mVendorList = Nothing
                    mStatusList = Nothing
                    mReceipt = Nothing
                    Response.Redirect("index.aspx")
                End If
            Else
                RemoveSession()
                mVendorList = Nothing
                mStatusList = Nothing
                mReceipt = Nothing
                Response.Redirect("index.aspx")
            End If
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ReceiptPONew")) And (Not User.IsInRole("ReceiptPOEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Session("EditForExpiryInfo") = "True" 'Added by Vikrant FOR ALL11052012-13
        If IsValid Then
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub txtReceiptDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReceiptDate.TextChanged
        mReceipt = Session("mReceipt")
        If Not IsDate(txtReceiptDate.Text) Then
            mReceipt.RecdDate = System.DBNull.Value
        Else
            mReceipt.RecdDate = txtReceiptDate.Text
        End If
        txtText.Text = mReceipt.Text
        Session("mReceipt") = mReceipt
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        SetReport()
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub btnPrintTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTag.Click
        If Not User.IsInRole("ReceiptPOPrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim rpt As New crptStoreAcceptanceTag1
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As rptStoresAcceptanceTag
        Dim letter As rptLetterHead
        Dim ds As New dsStoresAcceptanceTag
        obj = rptStoresAcceptanceTag.GetStoresAcceptanceTag(mReceipt.ID)
		letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
												 "", AppSettings("WODocumentNo"),
												 AppSettings("WORevisionNo"),
												 AppSettings("Barcode"),
												 AppSettings("ClientCode"),
												 SearchString4:=mModuleList.Item("Acceptance Tag").FormRevisionNo)

		If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Taj" Or AppSettings("ClientCode") = "HSC" Then
			If (Not AppSettings("Barcode") Is Nothing) AndAlso AppSettings("Barcode") = "True" Then
				myReport = New crptStoreAcceptanceTag6
			Else
				myReport = New crptStoreAcceptanceTag6WithoutBarcode
			End If
		ElseIf AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "Heligo" Then
			myReport = New crptServiceableUnserviceableTagForCE
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
			myReport = New crptStoreAcceptanceTagYATA
			'ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "LAMA" Then
			'    myReport = New crptServiceableUnserviceableTagForLama
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Novo" Then
			myReport = New crptStoreAcceptanceTagNOVO
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IRM") Then
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
        'rpt.SetDataSource(ds)
        'Session("CrystalReport") = rpt
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mReceipt.IsAttachmentAdded Then
            'mFileAttach = FileAttach.GetAttachment(mReceipt.ID)
            'mFileAttach = FileAttach.GetAttachmentChild(mReceipt.ID)
            'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mReceipt.FileAttachments(0).Extension 'mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mReceipt.FileAttachments(0).Extension) 'mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                'fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Write(mReceipt.FileAttachments(0).ImageFile, 0, mReceipt.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        Else
            MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
            ControlVisibilityForFileAttachment()
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mReceipt.IsAttachmentAdded = False
        mReceipt.FileAttachments.Remove(mReceipt.ID)
        Session("mReceipt") = mReceipt
        Session("ReceiptIsAttachmentNotSave") = mIsAttachmentNotSave
        ControlVisibilityForFileAttachment()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If mFileAttach.ReferenceID.Equals(mReceipt.ID) Then
            If mReceipt.IsAttachmentAdded Then
                mReceipt.FileAttachments(0).Size = mFileAttach.Size
                mReceipt.FileAttachments(0).ImageFile = mFileAttach.ImageFile
                mReceipt.FileAttachments(0).Extension = mFileAttach.Extension
            Else
                mReceipt.IsAttachmentAdded = True
                mReceipt.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
            End If
        Else
            If mReceipt.ReceiptItems(mFileAttach.ReferenceID).IsAttachmentAdded Then
                mReceipt.ReceiptItems(CType(Session("index"), Integer)).FileAttachments(0).Size = mFileAttach.Size
                mReceipt.ReceiptItems(CType(Session("index"), Integer)).FileAttachments(0).ImageFile = mFileAttach.ImageFile
                mReceipt.ReceiptItems(CType(Session("index"), Integer)).FileAttachments(0).Extension = mFileAttach.Extension
            Else
                mReceipt.ReceiptItems(mFileAttach.ReferenceID).IsAttachmentAdded = True
                mReceipt.ReceiptItems(CType(Session("index"), Integer)).FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
            End If
        End If
        ControlVisibilityForFileAttachment()
        dgReceiptItems.DataSource = mReceipt.ReceiptItems
        dgReceiptItems.DataBind()
        SetGrid()
        Session("ReceiptIsAttachmentNotSave") = True
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If mReceipt.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachmentChild(mReceipt.ID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mReceipt.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub btnSaveAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAttachment.Click
        'mReceipt.UserName = User.Identity.Name
        '--------
        'Commented 0n 10-May-2018
        'mReceipt.Save()
        mIsAttachmentNotSave = False
        Session("ReceiptIsAttachmentNotSave") = mIsAttachmentNotSave

        If IsValid Then
            mReceipt.Save()  ''APFT : ALL18012018 Added by Saylee on 18-Jan-2019 to open button after authorization ,to save rematk and note
        Else
            upnlValidationsummary.Update()
        End If

        mReceipt.UpdateReceiptAttachment(mReceipt.FileAttachments)

       
        '--------------------
        SetGrid()
        MarkLog(Action.Save, mModuleName, "Attachment", ErrorType.NoError, mReceipt.ID, EventLogID)
        upnlReceiptItems.Update()
        MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
    End Sub
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click

        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '  Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        SetUserMailIDs()
        '-------


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
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> Receipt </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            Session("mReceipt") = mReceipt
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If IsValid Then
            Dim IsInUse As IsInUse = IsInUse.GetIsInUseReceiptINIssue(mReceipt.ID)
            If IsInUse.IsInUse Then
                MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, "<Strong> Receipt </Strong>", MsgBoxStyle.OkOnly, "StatusCancel")
                Session("mReceipt") = mReceipt
                Exit Sub
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Receipt </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
            Session("IsValid") = IsValid
            Session("mReceipt") = mReceipt
        End If
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        SetObject()
        If mReceipt.IsValid = False Then
            For i As Integer = 0 To mReceipt.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mReceipt.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mReceiptItem As ReceiptItem
        If mReceipt.ReceiptItems.IsValid = False Then
            For Each mReceiptItem In mReceipt.ReceiptItems
                For i As Integer = 0 To mReceiptItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mReceiptItem.ItemName + " : " + mReceiptItem.GetBrokenRulesCollection(i).Description + "<Br>"
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

#Region " Service Methods "
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

End Class