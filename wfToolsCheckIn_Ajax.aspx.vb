Imports System.Text
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic

Public Class wfToolsCheckIn_Ajax
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
    '    Private Enum RequstFor
    '        Supplier = 0
    '        Customer = 1
    '    End Enum
#End Region

#Region " Variable Declaration "
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public EventLogID As Guid
    Protected mStoreList As StoreList
    Public mEmployeeStatus As EmployeeStatus
    Dim mPendingItemList As PendingToolsToReceiveFromEmployee
    Public ModuleName As String

    Public mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
    'Added By Utkarsh On 20-Jul-2011 For All19072011
    Dim mRCIDetail As String 'Added By Utkarsh On 20-Jul-2011 For All19072011
    Dim mOtherCharge As OtherCharge   'Added By Prashant 26-Jul-2012
    Dim mOtherChargeListByInvoiceID As OtherChargeListByInvoiceID 'Added By Prashant 26-Jul-2012
    Dim mOpenFrom As String 'Added By Prashant 3-Apr-2014 ALL03042014
    Dim ExtraMessage As String = ""
    Dim ItemsComply As StringBuilder = New StringBuilder
    Dim mEmployeeEmailID As EmployeeEmailID
    Dim mEmployeeEmailIDs As String = String.Empty
    Dim mUser As User
    Public mEmployeeList As EmployeeList
#End Region


#Region " Business Methods "
    Private Sub GetSession()
        mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
        mStoreList = CType(Session("mStoreList"), StoreList)
        ModuleName = Session("ModuleName")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReceiptCumInvoice")
        Session.Remove("mStoreList")
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtReceivedFromEmp" Then
            If txtReceivedFromEmp.Text = "" Or mReceiptCumInvoice.ToolsReceivedByEmployeeID.Equals(Guid.Empty) Then
                e.IsValid = False
                custValidator.ErrorMessage = "Select Returned By Employee Name"
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Public Function CustomValidate2() As Boolean

        Dim strMsg As String = ""
        If txtReceivedFromEmp.Text = "" Or mReceiptCumInvoice.ToolsReceivedByEmployeeID.Equals(Guid.Empty) Then
            strMsg = "Select Returned By Employee Name"
        End If

        If strMsg <> "" Then
            cvControlValidator.ErrorMessage = strMsg
            cvControlValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub addattributes()
        txtInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtInvoiceNo').value,event)")
    End Sub
    Private Sub SetPage()
        If mReceiptCumInvoice.InvNo > 0 Then
            lblTitle.Text = Session("ModuleName") + " [" + mReceiptCumInvoice.InvText + "-" + CType(mReceiptCumInvoice.InvNo, String) + "]"
        Else
            lblTitle.Text = Session("ModuleName") + " [ New ]"
        End If
    End Sub
    Private Sub SetObject()
        Dim mBaseCurrency As Currency
        mBaseCurrency = Currency.GetBaseCurrency()
        mReceiptCumInvoice.CurrencyID = mBaseCurrency.ID
        mReceiptCumInvoice.ConversionFactor = mBaseCurrency.ConversionFactor

        mReceiptCumInvoice.RecCumInvDate = CDate(txtReceiptCumInvoiceDate.Text)
        mReceiptCumInvoice.InvText = txtInvoiceText.Text
        mReceiptCumInvoice.InvNo = Val(txtInvoiceNo.Text)
        mReceiptCumInvoice.Remark = Trim(txtRemark.Text)
        mReceiptCumInvoice.FromTypeID = 19 'Employee
        mReceiptCumInvoice.UserName = User.Identity.Name
        Dim cmbValue As DropDownList
        Dim txtRCIItemRemark As TextBox
        Dim txtNote As TextBox  'Here Note we are taking for Phy. Condition . Added by Prashant 28-Jan-2019 'ALL28012019
        Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
        Dim i As Integer = 0
        For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems
            With mReceiptCumInvoiceItem
                Try
                    cmbValue = CType(Me.dgToolsReceipt.Rows(i).FindControl("cmbStore"), DropDownList)
                    .StoreID = New Guid(cmbValue.SelectedValue)
                    txtRCIItemRemark = CType(Me.dgToolsReceipt.Rows(i).FindControl("txtRCIItemRemark"), TextBox)
                    .Remark = txtRCIItemRemark.Text.Trim
                    txtNote = CType(Me.dgToolsReceipt.Rows(i).FindControl("txtNote"), TextBox)
                    .Note = txtNote.Text.Trim
                Catch ex As Exception
                End Try
            End With
            i = i + 1
        Next
        mReceiptCumInvoice.Invoice.CalculateTotal()  'Added By Prashant 1-Apr-2019 ALL01042019 
    End Sub
    '    Private Sub DeleteRecord(ByVal Index As Int32)
    '        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Remove")
    '        mIssue.IssueItems.CurrentIndex = Index
    '        Session("mIssue") = mIssue
    '    End Sub
    '    Private Sub ReturnWOQty()
    '        Dim mtmpIssueItem As IssueItem
    '        For Each mtmpIssueItem In mIssue.IssueItems
    '            mtmpIssueItem.DisplayQty = mtmpIssueItem.DisplayQty - mtmpIssueItem.WOReturnQty
    '            mtmpIssueItem.WOReturnQty = 0

    '        Next
    '        mIssue.RemoveQtyZeroItems()
    '        dgIssueItems.DataSource = mIssue.IssueItems
    '        dgIssueItems.DataBind()
    '        Session("mIssue") = mIssue
    '    End Sub
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
    Private Sub Save()
        'Authentication
        If Not mReceiptCumInvoice.RecCumInvDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mReceiptCumInvoice.RecCumInvDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If


                'Added By Vikrant On 24-July-2014 For BA24072014
                If AppSettings("LockBackDatedTransaction") = "True" And (mReceiptCumInvoice.TransTypeID <> 9 And mReceiptCumInvoice.TransTypeID <> 66) Then
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        If mReceiptCumInvoice.StatusID <> 2 Then
                            If CheckDateForTransactionLock(mReceiptCumInvoice.RecCumInvDate) Then
                                MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), Month(CDate(mReceiptCumInvoice.RecCumInvDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                                Exit Sub
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
                                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Serial number already exist. You can not add Duplicate.", MsgBoxStyle.OkOnly, "Status")
                                Exit Sub
                            End If
                        End If
                        i = i + 1
                    End While
                    mReceiptCumInvoice.ApplyEdit()
                    Dim mReceiptCumInvoiceCharge As InvoiceCharge
                    For Each mReceiptCumInvoiceCharge In mReceiptCumInvoice.Invoice.InvoiceCharges
                        If (mReceiptCumInvoiceCharge.Sign <> 1 And mReceiptCumInvoiceCharge.CChargeAmount <= 0) Or (Not (mReceiptCumInvoiceCharge.IsValid)) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Goods Receipt Charge(s) are not allowed if Goods Receipt Amount Is Zero ", MsgBoxStyle.OkOnly, "")
                            mReceiptCumInvoice.CancelEdit()
                            Exit Sub
                        End If
                    Next
                    'Added by Utkarsh on 19-Nov-2013 FOr TransTextSeries 
                    'Check if ReceiptCumInvoiceText is blank then call TransTextSeries UI

                    If (mReceiptCumInvoice.IsNew) And (mReceiptCumInvoice.InvText = "") Then

                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mReceiptCumInvoice.TransTypeID, mReceiptCumInvoice.RecCumInvDateFormatted)

                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mReceiptCumInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID).TransText = "")) Then

                            Dim str = "<script language='javascript'>openledgersame('wfToolsCheckIn_Ajax.aspx?BackPage=index.aspx');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Tools Check In"
                            Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted
                            If mReceiptCumInvoice.StatusID = 2 Then
                                mReceiptCumInvoice.StatusID = 1
                                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                            End If
                            MSGBoxCtrl.show("Tools Check In", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RCITransTextSeriesAlert")
                            Exit Sub
                        Else
                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then
                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mReceiptCumInvoice.TransTypeID)
                                    mReceiptCumInvoice.InvText = .TransText
                                    mReceiptCumInvoice.InvNo = .StartingTransNo
                                End With
                            Else
                                Dim str = "<script language='javascript'>openledgersame('wfToolsCheckIn_Ajax.aspx?BackPage=index.aspx');</script>"
                                Session("BackPagestr_ForTransSeries") = str
                                Session("TransName_ForTransSeries") = "Receipt Cum Invoice"
                                Session("TransTypeID_ForTransSeries") = mReceiptCumInvoice.TransTypeID
                                Session("TransDate_ForTransSeries") = mReceiptCumInvoice.RecCumInvDateFormatted
                                If mReceiptCumInvoice.StatusID = 2 Then
                                    mReceiptCumInvoice.StatusID = 1
                                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                                End If
                                MSGBoxCtrl.show("Tools Check In", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RCITransTextSeriesAlert")
                                Exit Sub
                            End If
                        End If

                    End If
                    'End
                    mReceiptCumInvoice.Save()



                    'End
                    mReceiptCumInvoice.MarkClean()
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                    DataFieldBind()
                    ControlVisibility()
                    SetPage()
                    upnlReceiveDetails.Update()
                    upnlRecItem.Update()
                    upnlActionBtn.Update()
                    upnlTitle.Update()
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Please add at least One Item.", MsgBoxStyle.OkOnly, "")
                If mReceiptCumInvoice.StatusID = 2 Then
                    mReceiptCumInvoice.StatusID = 1
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                End If
                'mReceiptCumInvoice = ReceiptCumInvoiceClone
                'SetObject()
                'Session("mReceiptCumInvoice") = mReceiptCumInvoice
                'DataFieldBind()
                Exit Sub
            End If
        Catch ex As SqlException
            Session("ReceiptCumInvoiceClone") = ReceiptCumInvoiceClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br></br><b>Please amend Purchase Order for Receipt of excess quantity.</b>", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.show("Alert!", "Goods Receipt Charge Deleted ! ", "Goods Receipt charge Not Available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    MSGBoxCtrl.show("Alert!", "Goods Receipt is not Saved !", ex.Message, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex1.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "Status")
                mReceiptCumInvoice = ReceiptCumInvoiceClone
                SetObject()
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                DataFieldBind()
                Exit Sub
            ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br><b>Please amend Purchase Order Quantity & make Goods Receipt again.</b>", MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "Status")
                mReceiptCumInvoice = ReceiptCumInvoiceClone
                SetObject()
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                DataFieldBind()
                Exit Sub
            End If
            mReceiptCumInvoice = ReceiptCumInvoiceClone
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Finally
            ReceiptCumInvoiceClone = Nothing
        End Try
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
                            dgToolsReceipt.Columns(1).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
                            dgToolsReceipt.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems
                            dgToolsReceipt.DataBind()
                            ControlVisibility()
                            upnlRecItem.Update()
                            upnlReceiveDetails.Update()
                            upnlActionBtn.Update()
                            Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then
                        If mReceiptCumInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            'mReceiptCumInvoice.StatusID = 2
                            Session("mReceiptCumInvoice") = mReceiptCumInvoice
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
                            mReceiptCumInvoice.StatusID = 2
                            Session("mReceiptCumInvoice") = mReceiptCumInvoice
                            Save()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                            Exit Sub
                        End If

                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            mReceiptCumInvoice.StatusID = 2
                            Session("mReceiptCumInvoice") = mReceiptCumInvoice
                            Save()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        If mReceiptCumInvoice.StatusID = 2 Then
                            mReceiptCumInvoice.StatusID = 1
                        End If
                        Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        ControlVisibility()
                        upnlReceiveDetails.Update()
                        'Response.Redirect("wfToolsCheckOut_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        '------------------------------------------------------------------------------------------
                        '-----------------Added by Vikrant on 26-aug-2011--------------------------
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "ResetReceivedFromEmployee" Then
                        txtReceivedFromEmp.Text = ""
                        txtReceivedFromEmp.DataBind()
                        mReceiptCumInvoice.ToolsReceivedByEmployeeID = Guid.Empty
                        mReceiptCumInvoice.ToolsReceivedByEmployeeName = ""
                        upnlReceiveDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "ResetSubmittedByEmployee" Then
                        txtSubmittedByEmp.Text = ""
                        mReceiptCumInvoice.ToolsSubmittedByEmployeeID = Guid.Empty
                        mReceiptCumInvoice.ToolsSubmittedByEmployeeName = ""
                        upnlReceiveDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        If mReceiptCumInvoice.StatusID = 2 Then
                            mReceiptCumInvoice.StatusID = 1
                        End If
                        Session("sender") = ""
                        Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        ''========================================
                        DataFieldBind()
                        upnlReceiveDetails.Update()
                        upnlActionBtn.Update()
                    ElseIf MSGBoxCtrl.Sender = "DuplicateBarcode" Then
                        txtBarcodeItem.Text = ""
                        txtBarcodeItem.DataBind()
                        upnlReceiveDetails.Update()
                    End If '
            End Select
        End If
    End Sub
    Private Sub ControlVisibility()
        dgToolsReceipt.Columns(11).Visible = IIf(mReceiptCumInvoice.StatusID = 1, True, False)
        txtInvoiceText.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
        txtInvoiceNo.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
        txtReceiptCumInvoiceDate.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean) And mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0)
        txtReceivedFromEmp.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
        txtSubmittedByEmp.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
        txtRemark.Enabled = (CType(IIf(mReceiptCumInvoice.StatusID >= 2, False, True), Boolean))
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
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Public Sub SetReport()
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

        rpt = New crptToolsCheckIn

        Dim obj As rptReceiptCumInvoice
        Dim objChilds As rptReceiptCumInvoiceChildList

        Dim mCompanyInfo As rptSearchingCriteriaForReceipt

        Dim ds As New dsRecCumInvReg



        obj = rptReceiptCumInvoice.GetReceiptCumInvoice(mReceiptCumInvoice)
        objChilds = rptReceiptCumInvoiceChildList.GetReceiptCumInvoiceChild(mReceiptCumInvoice)
        mCompanyInfo = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "", "", "", AppSettings("ClientCode"), AppSettings("Barcode") = "True", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, AppSettings("Logo"), AppSettings("PrintBarCodeOnItemDetail"))


        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, objChilds)
        da.Fill(ds, mCompanyInfo)
        da.Fill(ds, mrptImage)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(0, "", False)
        Session("mStoreList") = mStoreList

        dgToolsReceipt.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems
        Session("mReceiptCumInvoice") = mReceiptCumInvoice

        txtReceiptCumInvoiceDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted.ToString
        txtReceivedFromEmp.Text = mReceiptCumInvoice.ToolsReceivedByEmployeeName
        txtSubmittedByEmp.Text = mReceiptCumInvoice.ToolsSubmittedByEmployeeName

        dgToolsReceipt.Columns(1).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
        DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addattributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
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
            DataFieldBind()
            ControlVisibility()
            SetPage()
        End If
    End Sub
    Private Sub txtReceiptCumInvoiceDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReceiptCumInvoiceDate.TextChanged
        mReceiptCumInvoice.RecCumInvDate = CType(Trim(txtReceiptCumInvoiceDate.Text), Object)
        txtInvoiceText.Text = mReceiptCumInvoice.InvText
    End Sub
    Private Sub btnAddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        If IsValid Then
            SetObject()
            mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
            Session("OpenFromCheckInDetailPage") = True
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
            Response.Redirect("wfPendingToolsToReceiveFromEmployee_Ajax.aspx?BackPage=index.aspx&ChildPage=wfToolsCheckIn_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgToolsReceipt_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgToolsReceipt.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex = CInt(e.CommandArgument) - 1
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
        End Select
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'MarkLog(Util.Action.Close, ModuleName, "", Util.ErrorType.NoError, Guid.Empty)
        SetObject()
        mRCIDetail = mReceiptCumInvoice.ReceiptNo + " Dated : " + mReceiptCumInvoice.RecCumInvDateFormatted.ToString
        MarkLog(Util.Action.Close, "ToolsCheckIn", mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)

        Session("IsValid") = IsValid
        If mReceiptCumInvoice.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                SetObject()
            End If
        Else
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Protected Sub txtReceivedFromEmp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'SetEmpID()
        Dim message As String = ""
        If IsNumeric(txtReceivedFromEmp.Text) Then
            Dim mEmployeeListForCombo As EmployeeListForCombo
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtReceivedFromEmp.Text)
            If mEmployeeListForCombo.Count > 0 Then
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeListForCombo(0).ID.ToString, mReceiptCumInvoice.RecCumInvDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetReceivedFromEmployee")
                        Exit Sub
                    End If
                    txtReceivedFromEmp.Text = mEmployeeListForCombo(0).LicenceNoName
                    txtReceivedFromEmp.DataBind()
                    mReceiptCumInvoice.ToolsReceivedByEmployeeID = New Guid(mEmployeeListForCombo(0).ID.ToString)
                    mReceiptCumInvoice.ToolsReceivedByEmployeeName = mEmployeeListForCombo(0).LicenceNoName
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                End If
                Exit Sub
            End If

        End If

        If hdnReceivedFromEmpId.Value <> "" Then
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnReceivedFromEmpId.Value.ToString, mReceiptCumInvoice.RecCumInvDateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetReceivedFromEmployee")
                    Exit Sub
                End If
                mReceiptCumInvoice.ToolsReceivedByEmployeeID = New Guid(hdnReceivedFromEmpId.Value)
                mReceiptCumInvoice.ToolsReceivedByEmployeeName = txtReceivedFromEmp.Text
            Else
                txtReceivedFromEmp.Text = ""
                mReceiptCumInvoice.ToolsReceivedByEmployeeID = Guid.Empty
                mReceiptCumInvoice.ToolsReceivedByEmployeeName = ""
            End If
        Else
            'txtReceivedFromEmp.Text = ""
            'mReceiptCumInvoice.ToolsReceivedByEmployeeID = Guid.Empty
            'mReceiptCumInvoice.ToolsReceivedByEmployeeName = ""
            If txtReceivedFromEmp.Text <> "" Then
                mEmployeeList = EmployeeList.GetEmployeeList()
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtReceivedFromEmp.Text, "").ID.ToString, mReceiptCumInvoice.RecCumInvDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetReceivedFromEmployee")
                        Exit Sub
                    End If
                    mReceiptCumInvoice.ToolsReceivedByEmployeeID = mEmployeeList(txtReceivedFromEmp.Text, "").ID
                    mReceiptCumInvoice.ToolsReceivedByEmployeeName = txtReceivedFromEmp.Text
                Else
                    txtReceivedFromEmp.Text = ""
                    mReceiptCumInvoice.ToolsReceivedByEmployeeID = Guid.Empty
                    mReceiptCumInvoice.ToolsReceivedByEmployeeName = ""
                End If
            End If
        End If
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Protected Sub txtSubmittedByEmp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim message As String = ""
        If IsNumeric(txtSubmittedByEmp.Text) Then
            Dim mEmployeeListForCombo As EmployeeListForCombo
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtSubmittedByEmp.Text)
            If mEmployeeListForCombo.Count > 0 Then
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeListForCombo(0).ID.ToString, mReceiptCumInvoice.RecCumInvDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetReceivedFromEmployee")
                        Exit Sub
                    End If
                    txtSubmittedByEmp.Text = mEmployeeListForCombo(0).LicenceNoName
                    mReceiptCumInvoice.ToolsSubmittedByEmployeeID = New Guid(mEmployeeListForCombo(0).ID.ToString)
                    mReceiptCumInvoice.ToolsSubmittedByEmployeeName = mEmployeeListForCombo(0).LicenceNoName
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                End If
                Exit Sub
            End If
        End If

        If hdnSubmittedByEmpId.Value <> "" Then
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnSubmittedByEmpId.Value.ToString, mReceiptCumInvoice.RecCumInvDateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetSubmittedByEmployee")
                    Exit Sub
                End If
                mReceiptCumInvoice.ToolsSubmittedByEmployeeID = New Guid(hdnSubmittedByEmpId.Value)
                mReceiptCumInvoice.ToolsSubmittedByEmployeeName = txtSubmittedByEmp.Text
            Else
                txtSubmittedByEmp.Text = ""
                mReceiptCumInvoice.ToolsSubmittedByEmployeeID = Guid.Empty
                mReceiptCumInvoice.ToolsSubmittedByEmployeeName = ""
            End If
        Else
            'txtSubmittedByEmp.Text = ""
            'mReceiptCumInvoice.ToolsSubmittedByEmployeeID = Guid.Empty
            'mReceiptCumInvoice.ToolsSubmittedByEmployeeName = ""
            If txtSubmittedByEmp.Text <> "" Then
                mEmployeeList = EmployeeList.GetEmployeeList()
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtSubmittedByEmp.Text, "").ID.ToString, mReceiptCumInvoice.RecCumInvDateFormatted.ToString)
                If mEmployeeStatus.Count > 0 Then
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetSubmittedByEmployee")
                        Exit Sub
                    End If
                    mReceiptCumInvoice.ToolsSubmittedByEmployeeID = mEmployeeList(txtSubmittedByEmp.Text, "").ID
                    mReceiptCumInvoice.ToolsSubmittedByEmployeeName = txtSubmittedByEmp.Text
                Else
                    txtSubmittedByEmp.Text = ""
                    mReceiptCumInvoice.ToolsSubmittedByEmployeeID = Guid.Empty
                    mReceiptCumInvoice.ToolsSubmittedByEmployeeName = ""
                End If
            End If
        End If
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        SetObject()
        If Not mReceiptCumInvoice.IsValid Then
            'ValidationCode()
            'CustomValidate1()
            upnlValidationSummary.Update()
            Exit Sub
        End If

        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            MSGBoxCtrl.show("Save Alert!", "You are about to Receive Tool(s).</br></br>Do you want to continue?", "", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        SetReport()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#Region " Barcode "
    Private Sub btnAddBarcodeItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBarcodeItem.Click
        If IsValid Then
            If txtBarcodeItem.Text <> "" Then '--
                mPendingItemList = PendingToolsToReceiveFromEmployee.GetPendingTools(EmployeeName:=mReceiptCumInvoice.ToolsReceivedByEmployeeName, BarcodeNo:=txtBarcodeItem.Text.Trim)

                If mPendingItemList.Count > 0 Then '3
                    If mReceiptCumInvoice.ReceiptCumInvoiceItems.Contains(mPendingItemList(0).IssueItemID, "") Then '4
                        MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Receipt Item", MsgBoxStyle.OkOnly, "DuplicateBarcode")
                        txtBarcodeItem.Text = ""
                        Exit Sub
                    Else '4
                        AddItemByBarcode(mPendingItemList)
                    End If '5
                Else
                    MSGBoxCtrl.show("Add alert !", "Tool can not be added <br> Tool not present in Stock or Wrong Employee Name selected", "", MsgBoxStyle.OkOnly, "")
                    txtBarcodeItem.Text = ""
                    Exit Sub
                End If '4
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Invalid Barcode Number.", False), True)
                txtBarcodeItem.Text = ""
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Public Sub AddItemByBarcode(ByVal mPendingItemList As PendingToolsToReceiveFromEmployee)
        mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(Guid.NewGuid)

        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 19 'From Employee
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingItemList(0).IssueItemID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPendingItemList(0).ItemID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPendingItemList(0).ItemName
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPendingItemList(0).Description
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mPendingItemList(0).FromStoreID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreName = mPendingItemList(0).FromStoreName
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingItemList(0).UnitID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = 1
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mPendingItemList(0).ItemTypeID

        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        dgToolsReceipt.Columns(1).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
        dgToolsReceipt.DataSource = mReceiptCumInvoice.ReceiptCumInvoiceItems
        dgToolsReceipt.DataBind()

        txtBarcodeItem.Text = ""
        txtBarcodeItem.Focus()
        ControlVisibility()
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