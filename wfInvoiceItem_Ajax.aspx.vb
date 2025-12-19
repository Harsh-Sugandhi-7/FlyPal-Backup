Public Class wfInvoiceItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mInvoice As Invoice             'Code 18-FR03-VA06
    Public mPendingReceiptItemList As PendingInvoiceList
    Public mSelectList() As Boolean
    Public BackPage As String
    Public mCurrency As Currency
    Public IItemCounter As Integer
    Public tmpIICounter As Integer
    Public IsExitSelected As Boolean
    Public mGSTPercentage As GSTPercentage
    Public mVendor As Vendor
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mInvoice = CType(Session("mInvoice"), Invoice)
        mPendingReceiptItemList = CType(Session("mPendingReceiptItemList"), PendingInvoiceList)
        mSelectList = Session("mSelectList")
        tmpIICounter = Session("tmpIICounter")
        IItemCounter = Session("IItemCounter")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQty').value,event)")
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then
            txtAmount.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
            'txtDisplayCAmount.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        Else
            txtRate.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
            'txtDisplayRate.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
        End If
        txtCommercialRate.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
        'txtDisplayCommercialRate.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtDisplayCommercialRate').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Invoice Item [" & mInvoice.InvoiceItems.CurrentItem.ItemName & "]"
            imgbtnPartNo.BackColor = Color.Silver
            txtPartNo.BackColor = Color.Silver
        Else
            lblTitle.Text = "Purchase Invoice Item [New]"
        End If
    End Sub
    Private Function setObject() As Boolean
        mInvoice.BeginEdit()
        mInvoice.InvoiceItems.CurrentItem.SrNo = mInvoice.InvoiceItems.CurrentIndex + 1
        mInvoice.InvoiceItems.CurrentItem.Qty = Val(txtQty.Text)
        'Commneted and added On 30-Jan-2020
        'mInvoice.InvoiceItems.CurrentItem.DisplayQty = Val(txtDisplayQty.Text)
        'End of Commneted and added On 30-Jan-2020
        'Added By Vikrant on 02-Aug-2012 For BA01082012
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then
            mInvoice.InvoiceItems.CurrentItem.CAmount = Val(txtAmount.Text)
            If mInvoice.InvoiceItems.CurrentItem.Qty > 0 Then
                If mInvoice.TransTypeID = 10 Then
                    mInvoice.InvoiceItems.CurrentItem.GROCRate = mInvoice.InvoiceItems.CurrentItem.CAmount / mInvoice.InvoiceItems.CurrentItem.Qty
                Else
                    mInvoice.InvoiceItems.CurrentItem.CRate = mInvoice.InvoiceItems.CurrentItem.CAmount / mInvoice.InvoiceItems.CurrentItem.Qty
                End If
            End If
        Else 'Old Condition
            If mInvoice.TransTypeID = 10 Then
                'mInvoice.InvoiceItems.CurrentItem.GROCRate = Val(txtRate.Text)
                'Commneted and added On 30-Jan-2020
                'mInvoice.InvoiceItems.CurrentItem.GROCRate = Val(txtDisplayRate.Text)
                mInvoice.InvoiceItems.CurrentItem.GROCRate = Val(txtRate.Text)
                'End of Commneted and added On 30-Jan-2020
            Else
                'mInvoice.InvoiceItems.CurrentItem.CRate = Val(txtRate.Text)
                'Commneted and added On 30-Jan-2020
                'mInvoice.InvoiceItems.CurrentItem.CRate = Val(txtDisplayRate.Text) * mInvoice.InvoiceItems.CurrentItem.Factor
                mInvoice.InvoiceItems.CurrentItem.CRate = Val(txtRate.Text)
                'mInvoice.InvoiceItems.CurrentItem.DisplayCRate = Val(txtDisplayRate.Text)
                'End of Commneted and added On 30-Jan-2020
            End If
        End If
        'End
        mInvoice.InvoiceItems.CurrentItem.ConversionFactor = mInvoice.ConversionFactor
        mInvoice.InvoiceItems.CurrentItem.COtherCharges = Val(txtOtherCharges.Text)
        'mInvoice.InvoiceItems.CurrentItem.CCommercialRate = Val(txtCommercialRate.Text) 'Code Added By Girish on July,19,2007
        'Commneted and added On 30-Jan-2020
        mInvoice.InvoiceItems.CurrentItem.CCommercialRate = Val(txtCommercialRate.Text) 'Code Added By Girish on July,19,2007
        'mInvoice.InvoiceItems.CurrentItem.DisplayCCommercialRate = Val(txtDisplayCommercialRate.Text)
        'End of Commneted and added On 30-Jan-2020
        mInvoice.InvoiceItems.CurrentItem.Remark = txtRemark.Text
        mInvoice.InvoiceItems.CurrentItem.Note = txtNote.Text

        If mInvoice.InvoiceItems.Contains(mInvoice.InvoiceItems.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Invoice Item", MsgBoxStyle.OkOnly, "Duplicate")
            mInvoice.CancelEdit()
            Exit Function
        Else
            '----GST--------------------------------------------------------------
            Dim mtmpItem As Item
            mtmpItem = Item.GetItem(mInvoice.InvoiceItems.CurrentItem.ItemID)
            If AppSettings("IsGSTApplicable") = "True" Then
                mVendor = Vendor.GetVendor(mInvoice.VendorID)
                If mVendor.ClientCountryName.ToUpper = "INDIA" Then
                    If mVendor.CountryName.ToUpper = "INDIA" And mInvoice.InvoiceDate >= CDate("01-Jul-2017") Then
                        mGSTPercentage = GSTPercentage.GetPercentage(mInvoice.InvoiceDate, 1, mInvoice.InvoiceItems.CurrentItem.ItemID.ToString)
                        If Not mGSTPercentage Is Nothing Then
                            If Len(mVendor.StateCode) > 0 Then
                                If mVendor.StateCode = mVendor.ClientStateCode Then
                                    If mInvoice.InvoiceItems.CurrentItem.CGSTPercentage = 0 Then
                                        mInvoice.InvoiceItems.CurrentItem.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                        mInvoice.InvoiceItems.CurrentItem.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                    Else
                                        'Do nothing  Already CGST/SGST percentage set from Order Item
                                    End If
                                    mInvoice.InvoiceItems.CurrentItem.CGSTCAmount = ((mInvoice.InvoiceItems.CurrentItem.CGSTPercentage * mInvoice.InvoiceItems.CurrentItem.CAmount) / 100)
                                    mInvoice.InvoiceItems.CurrentItem.SGSTCAmount = ((mInvoice.InvoiceItems.CurrentItem.SGSTPercentage * mInvoice.InvoiceItems.CurrentItem.CAmount) / 100)

                                    mInvoice.InvoiceItems.CurrentItem.TotalCAmount = mInvoice.InvoiceItems.CurrentItem.CAmount + mInvoice.InvoiceItems.CurrentItem.CGSTCAmount + mInvoice.InvoiceItems.CurrentItem.SGSTCAmount
                                    mInvoice.InvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

                                    mInvoice.InvoiceItems.CurrentItem.DisplayCGSTCAmount = ((mInvoice.InvoiceItems.CurrentItem.CGSTPercentage * mInvoice.InvoiceItems.CurrentItem.DisplayCAmount) / 100)
                                    mInvoice.InvoiceItems.CurrentItem.DisplaySGSTCAmount = ((mInvoice.InvoiceItems.CurrentItem.SGSTPercentage * mInvoice.InvoiceItems.CurrentItem.DisplayCAmount) / 100)
                                    mInvoice.InvoiceItems.CurrentItem.DisplayTotalCAmount = mInvoice.InvoiceItems.CurrentItem.DisplayCAmount + mInvoice.InvoiceItems.CurrentItem.DisplayCGSTCAmount + mInvoice.InvoiceItems.CurrentItem.DisplaySGSTCAmount

                                    mInvoice.StateCode = mVendor.StateCode
                                    mInvoice.ClientStateCode = mVendor.ClientStateCode
                                    mInvoice.VendorCountry = mVendor.CountryName
                                    mInvoice.Visibility = 1
                                Else
                                    If mInvoice.InvoiceItems.CurrentItem.IGSTPercentage = 0 Then
                                        mInvoice.InvoiceItems.CurrentItem.IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                    Else
                                        'Do nothing  Already IGST percentage set from Order Item
                                    End If
                                    mInvoice.InvoiceItems.CurrentItem.IGSTCAmount = ((mInvoice.InvoiceItems.CurrentItem.IGSTPercentage * mInvoice.InvoiceItems.CurrentItem.CAmount) / 100)

                                    mInvoice.InvoiceItems.CurrentItem.TotalCAmount = mInvoice.InvoiceItems.CurrentItem.CAmount + mInvoice.InvoiceItems.CurrentItem.IGSTCAmount
                                    mInvoice.InvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode

                                    mInvoice.StateCode = mVendor.StateCode
                                    mInvoice.ClientStateCode = mVendor.ClientStateCode
                                    mInvoice.VendorCountry = mVendor.CountryName
                                    mInvoice.Visibility = 2
                                End If
                            Else
                                mInvoice.StateCode = mVendor.StateCode
                                mInvoice.ClientStateCode = mVendor.ClientStateCode
                                mInvoice.VendorCountry = mVendor.CountryName
                                mInvoice.Visibility = 3
                            End If
                        End If
                        mtmpItem = Nothing
                    Else
                        mInvoice.StateCode = mVendor.StateCode
                        mInvoice.ClientStateCode = mVendor.ClientStateCode
                        mInvoice.VendorCountry = mVendor.CountryName
                        mInvoice.Visibility = 3
                    End If
                Else
                    mInvoice.StateCode = mVendor.StateCode
                    mInvoice.ClientStateCode = mVendor.ClientStateCode
                    mInvoice.VendorCountry = mVendor.CountryName
                    mInvoice.Visibility = 3
                End If
            Else
                mInvoice.Visibility = 3
            End If
            mInvoice.InvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
            '------------------------------------------------------------------
            mInvoice.ApplyEdit()
            mInvoice.CalculateTotal()               'Added By Saylee on 8-Sep-2007
            If mInvoice.IsRoundOff = True Then      'Added By Prashant on 21-May-2012 ALL25102012
                mInvoice.RoundCGrandTotal()
            End If
        End If
        Return True
    End Function
    'Added By Vikrant on 01-Aug-2012 For BA01082012
    Private Sub ControlVisibility()
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then
            txtRate.ReadOnly = True
            txtRate.BackColor = System.Drawing.Color.Gainsboro
            'Commneted and added On 30-Jan-2020
            'txtDisplayRate.ReadOnly = True
            'txtDisplayRate.BackColor = System.Drawing.Color.Gainsboro
            'End of Commneted and added On 30-Jan-2020
            txtOtherCharges.Visible = False
            lblOtherCharges.Visible = False
            txtAmount.ReadOnly = False
            'Commneted and added On 30-Jan-2020
            'txtDisplayCAmount.ReadOnly = False 'Added By Prashant 5-Feb-2019 ALL04022019 
            'End of Commneted and added On 30-Jan-2020
        Else
            txtRate.ReadOnly = False
            'Commneted and added On 30-Jan-2020
            'txtDisplayRate.ReadOnly = False
            'End of Commneted and added On 30-Jan-2020
            If AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
                txtOtherCharges.Visible = False
                lblOtherCharges.Visible = False
            Else
                txtOtherCharges.Visible = True
                lblOtherCharges.Visible = True
            End If
            txtAmount.ReadOnly = True
            txtAmount.BackColor = System.Drawing.Color.Gainsboro
            'Commneted and added On 30-Jan-2020
            'txtDisplayCAmount.ReadOnly = True 'Added By Prashant 5-Feb-2019 ALL04022019 
            'txtDisplayCAmount.BackColor = System.Drawing.Color.Gainsboro
            'If mInvoice.TransTypeID = 21 Then  'Added By Prashant 5-Feb-2019 ALL04022019 
            '    txtDisplayCAmount.Visible = True 'Added By Prashant 5-Feb-2019 ALL04022019 
            'Else
            '    txtAmount.Visible = True
            'End If
            'End Commneted and added On 30-Jan-2020
        End If
    End Sub
    'End
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Duplicate" Then


                    End If
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Not (mInvoice.CurrencyID.Equals(Guid.Empty)) Then
            mCurrency = Currency.GetCurrency(mInvoice.CurrencyID)
            mInvoice.InvoiceItems.CurrentItem.Currency = mCurrency.Name
        Else
            mInvoice.InvoiceItems.CurrentItem.Currency = txtRateCurrency.Text
        End If

        If Not mInvoice.InvoiceItems.CurrentItem.ItemDetailForInvoice Is Nothing Then
            If mInvoice.InvoiceItems.CurrentItem.ItemDetailForInvoice.FromItemTypeID = 3 Then
                txtOrderIssueDate.Text = mInvoice.InvoiceItems.CurrentItem.OrderDateFormatted.ToString
            ElseIf mInvoice.InvoiceItems.CurrentItem.ItemDetailForInvoice.FromItemTypeID = 12 Then
                txtOrderIssueDate.Text = mInvoice.InvoiceItems.CurrentItem.ReceiptDateFormatted.ToString
            Else
                txtOrderIssueDate.Text = mInvoice.InvoiceItems.CurrentItem.IssueDateFormatted.ToString
            End If
        End If

        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            Dim mPendingInvoiceList As PendingInvoiceList = PendingInvoiceList.GetPendingToInvoiceList(mInvoice.VendorID, mInvoice.InvoiceItems.CurrentItem.ItemName, mInvoice.InvoiceDate.ToString)
            Dim mBalanceQty As Decimal = 0D
            Dim MaxQty As Decimal = 0D
            For I As Integer = 0 To mPendingInvoiceList.Count - 1
                If mPendingInvoiceList(I).ReceiptItemID.Equals(mInvoice.InvoiceItems.CurrentItem.ReceiptItemID) And Not mInvoice.InvoiceItems.CurrentItem.ReceiptItemID.Equals(Guid.Empty) Then
                    mBalanceQty = mPendingInvoiceList(I).BalanceQty
                    Exit For
                End If
            Next
            If Session("Edit") And Not mInvoice.InvoiceItems.CurrentItem.IsNew Then
                MaxQty = mBalanceQty + mInvoice.InvoiceItems.CurrentItem.Qty
            ElseIf mInvoice.InvoiceItems.CurrentItem.IsNew Then
                MaxQty = mBalanceQty
            End If
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
            Else
                If Val(txtQty.Text) > MaxQty Then
                    custValidator.ErrorMessage = "Quantity can't be greater than Balance Qty."
                    e.IsValid = False
                End If
            End If
        ElseIf custValidator.ControlToValidate = "txtRate" Then
            If Val(txtRate.Text) < 0 Then
                custValidator.ErrorMessage = "Rate must be positive number."
                e.IsValid = False
            End If
            If Val(txtOtherCharges.Text) < 0 Then
                custValidator.ErrorMessage = "Other charges can't be negative."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        If Not IsPostBack Then
            If txtPartNo.Enabled = True Then
                setFocus(txtPartNo)
            End If
            If Session("OpenFrom") = "Pending" Then
                IItemCounter = 1
                Session("IItemCounter") = IItemCounter
                Session("OpenFrom") = ""
            End If
            DataFieldBind()
        End If
        If Session("Edit") = True Then
            lbliRecNo.Text = ""
            lblMessage.Text = ""
        Else
            lbliRecNo.Text = (IItemCounter).ToString + " / " + GetSelectedCount().ToString
            lblMessage.Text = "Invoicing Part : "
        End If
        'MessageBoxResult()
        SetPage()
        ControlVisibility()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mInvoice.InvoiceItems.CurrentItem.IsNew And Not Session("Edit") = True Then mInvoice.InvoiceItems.Remove(mInvoice.InvoiceItems.CurrentItem)
        Session.Remove("Edit")
        Session.Remove("mSelectList")
        Response.Redirect("wfInvoice_Ajax.aspx?BackPage=wfInvoiceItem_Ajax.aspx")
    End Sub
    Private Sub imgbtnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnPartNo.Click
        setObject()
        Session("mInvoice") = mInvoice
        Session("PartNo") = txtPartNo.Text
        'If (mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 1 And mInvoice.IsNew) Then
        If (mInvoice.InvoiceItems.Count = 0) Then
            Session("mPrevTransID") = Guid.Empty
        Else
            Session("mPrevTransID") = mInvoice.InvoiceItems.Item(mInvoice.InvoiceItems.Count - 2).ItemDetailForInvoice.ReceiptID
            Session("mOrderTranstypeID") = mInvoice.InvoiceItems.Item(mInvoice.InvoiceItems.Count - 2).ItemDetailForInvoice.OrderTranstypeID
        End If
        Session("mTransaction") = 5  'Transaction.Receipt
        Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=wfInvoiceItem_Ajax.aspx&mType=3")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            MakeInvoiceItem()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Function GetSelectedCount() As Integer
        Dim Count As Integer = 0
        For i As Integer = 0 To UBound(mSelectList)
            If mSelectList(i) = True Then
                Count = Count + 1
            End If
        Next
        Return Count
    End Function
    Private Sub MakeInvoiceItem()
        If Session("PendingReceipt") = "True" Then
            Dim ind As Integer
            tmpIICounter = Session("tmpIICounter")
            For ind = tmpIICounter To mPendingReceiptItemList.Count - 1
                If mSelectList(ind) = True Then
                    tmpIICounter = ind
                    Session("tmpIICounter") = tmpIICounter
                    Exit For
                End If
            Next
            If IItemCounter < GetSelectedCount() Then
                If setObject() Then
                    If mSelectList(tmpIICounter) Then
                        mInvoice.InvoiceItems.Add(mInvoice.ID)
                        mInvoice.InvoiceItems.CurrentIndex = mInvoice.InvoiceItems.Count - 1
                        mInvoice.InvoiceItems.CurrentItem.ReceiptItemID = mPendingReceiptItemList(tmpIICounter).ReceiptItemID
                        'Added By Prashant 5-Feb-2019 ALL04022019
                        mInvoice.InvoiceItems.CurrentItem.DisplayUnitName = mPendingReceiptItemList(tmpIICounter).DisplayUnitName
                        'mInvoice.InvoiceItems.CurrentItem.BaseUnitID = mPendingReceiptItemList(tmpIICounter).BaseUnitID
                        'mInvoice.InvoiceItems.CurrentItem.DisplayUnitID = mPendingReceiptItemList(tmpIICounter).DisplayUnitID
                        If mPendingReceiptItemList(tmpIICounter).IsSerialized = True Then
                            mInvoice.InvoiceItems.CurrentItem.Qty = 1
                            'mInvoice.InvoiceItems.CurrentItem.DisplayQty = 1
                        Else
                            mInvoice.InvoiceItems.CurrentItem.Qty = mPendingReceiptItemList(tmpIICounter).BalanceQty
                            'mInvoice.InvoiceItems.CurrentItem.DisplayQty = mPendingReceiptItemList(tmpIICounter).DisplayQty
                        End If
                        If mInvoice.TransTypeID = 10 Then
                            mInvoice.InvoiceItems.CurrentItem.GROCRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                        Else
                            'mInvoice.InvoiceItems.CurrentItem.CRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                            'mInvoice.InvoiceItems.CurrentItem.CRate = (mPendingReceiptItemList(tmpIICounter).OrderRate * mPendingReceiptItemList(tmpIICounter).Factor)
                            mInvoice.InvoiceItems.CurrentItem.CRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                            'mInvoice.InvoiceItems.CurrentItem.DisplayCRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                        End If
                        mInvoice.InvoiceItems.CurrentItem.CGSTPercentage = mPendingReceiptItemList(tmpIICounter).CGSTPercentage
                        mInvoice.InvoiceItems.CurrentItem.SGSTPercentage = mPendingReceiptItemList(tmpIICounter).SGSTPercentage
                        mInvoice.InvoiceItems.CurrentItem.IGSTPercentage = mPendingReceiptItemList(tmpIICounter).IGSTPercentage
                        'mInvoice.InvoiceItems.CurrentItem.CCommercialRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                        IItemCounter = IItemCounter + 1
                        tmpIICounter = tmpIICounter + 1
                        Session("IItemCounter") = IItemCounter
                        Session("tmpIICounter") = tmpIICounter
                        Session.Remove("Edit")
                        DataBind()
                        Session("mInvoice") = mInvoice
                        lbliRecNo.Text = (IItemCounter).ToString + " / " + GetSelectedCount().ToString
                        upnlValidationSummary.Update()
                        upnlInvoiceItemStatus.Update()
                        upnlItemIformation.Update()
                        upnlOrderIssueInformation.Update()
                        upnlRateValues.Update()
                        upnlRemarkNote.Update()
                    Else
                        Session("mInvoice") = mInvoice
                        Session.Remove("mSelectList")
                        Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & BackPage)
                    End If
                Else
                    If mSelectList(tmpIICounter) Then
                        mInvoice.InvoiceItems.CurrentItem.ReceiptItemID = mPendingReceiptItemList(tmpIICounter).ReceiptItemID
                        If mPendingReceiptItemList(tmpIICounter).IsSerialized = True Then
                            mInvoice.InvoiceItems.CurrentItem.Qty = 1
                        Else
                            mInvoice.InvoiceItems.CurrentItem.Qty = mPendingReceiptItemList(tmpIICounter).BalanceQty
                        End If
                        If mInvoice.TransTypeID = 10 Then
                            mInvoice.InvoiceItems.CurrentItem.GROCRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                        Else
                            mInvoice.InvoiceItems.CurrentItem.CRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                        End If
                        'mInvoice.InvoiceItems.CurrentItem.CCommercialRate = mPendingReceiptItemList(tmpIICounter).OrderRate
                        IItemCounter = IItemCounter + 1
                        tmpIICounter = tmpIICounter + 1
                        Session("IItemCounter") = IItemCounter
                        Session("tmpIICounter") = tmpIICounter
                        Session.Remove("Edit")
                        DataBind()
                        Session("mInvoice") = mInvoice
                    Else
                        'Close form
                        mInvoice.InvoiceItems.Remove(mInvoice.InvoiceItems.CurrentItem)
                        Session("mInvoice") = mInvoice
                        Session.Remove("mSelectList")
                        Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & BackPage)
                    End If
                End If
            Else
                'Close form
                If setObject() Then
                Else
                    ' mInvoice.InvoiceItems.Remove(mInvoice.InvoiceItems.CurrentItem)
                    Exit Sub
                End If
                Session.Remove("Edit")
                Session("mInvoice") = mInvoice
                Session.Remove("mSelectList")
                Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & BackPage)
            End If
        Else
            'Close form
            If setObject() Then
            Else
                Exit Sub
            End If
            Session.Remove("Edit")
            Session("mInvoice") = mInvoice
            Session.Remove("mSelectList")
            Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & BackPage)
        End If
    End Sub
    'Private Sub txtDisplayCAmount_TextChanged(sender As Object, e As System.EventArgs) Handles txtDisplayCAmount.TextChanged
    '    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
    '        Dim Factor As Decimal
    '        Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mInvoice.InvoiceItems.CurrentItem.ItemID)
    '        If Not mUnitConverterList Is Nothing Then
    '            Factor = mUnitConverterList.UnitConverterFactor(mInvoice.InvoiceItems.CurrentItem.BaseUnitID, mInvoice.InvoiceItems.CurrentItem.DisplayUnitID)
    '        End If
    '        mInvoice.InvoiceItems.CurrentItem.DisplayCAmount = CDec(Val(txtDisplayCAmount.Text))
    '        mInvoice.InvoiceItems.CurrentItem.CAmount = CDec(Val(txtDisplayCAmount.Text))
    '        txtAmount.DataBind()
    '        If CDec(Val(txtDisplayQty.Text)) > 0 Then
    '            If (mInvoice.TransTypeID = 10) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
    '                'Do Nothing
    '            Else
    '                mInvoice.InvoiceItems.CurrentItem.DisplayCRate = mInvoice.InvoiceItems.CurrentItem.DisplayCAmount / (CDec(Val(txtDisplayQty.Text)))
    '                mInvoice.InvoiceItems.CurrentItem.CRate = mInvoice.InvoiceItems.CurrentItem.CAmount / (CDec(Val(txtDisplayQty.Text)) / Factor)
    '                'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = 0 'Added By Prashant 28-Oct-2013 --ALL25102013-1	
    '            End If
    '            txtRate.DataBind()
    '        End If
    '    End If
    'End Sub
    'Private Sub txtDisplayRate_TextChanged(sender As Object, e As System.EventArgs) Handles txtDisplayRate.TextChanged
    '    If (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo") Then
    '        Dim Factor As Decimal
    '        Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mInvoice.InvoiceItems.CurrentItem.ItemID)
    '        If Not mUnitConverterList Is Nothing Then
    '            Factor = mUnitConverterList.UnitConverterFactor(mInvoice.InvoiceItems.CurrentItem.BaseUnitID, mInvoice.InvoiceItems.CurrentItem.DisplayUnitID)
    '        End If
    '        If mInvoice.TransTypeID = 10 Then
    '            mInvoice.InvoiceItems.CurrentItem.GROCRate = Val(txtDisplayRate.Text)
    '        Else
    '            mInvoice.InvoiceItems.CurrentItem.DisplayCRate = CDec(Val(txtDisplayRate.Text))
    '            'Commneted and added On 30-Jan-2020
    '            'mInvoice.InvoiceItems.CurrentItem.CRate = CDec(Val(txtDisplayRate.Text)) * Factor
    '            mInvoice.InvoiceItems.CurrentItem.CRate = CDec(Val(txtDisplayRate.Text))
    '            'End of Commneted and added On 30-Jan-2020
    '        End If
    '        txtRate.DataBind()
    '        upnlRateValues.Update()
    '    End If
    'End Sub
    'Private Sub txtDisplayCommercialRate_TextChanged(sender As Object, e As System.EventArgs) Handles txtDisplayCommercialRate.TextChanged
    '    Dim Factor As Decimal
    '    Dim mUnitConverterList As UnitConverterList = UnitConverterList.GetUnitConverterList(mInvoice.InvoiceItems.CurrentItem.ItemID)
    '    If Not mUnitConverterList Is Nothing Then
    '        Factor = mUnitConverterList.UnitConverterFactor(mInvoice.InvoiceItems.CurrentItem.BaseUnitID, mInvoice.InvoiceItems.CurrentItem.DisplayUnitID)
    '    End If
    '    'Commneted and added On 30-Jan-2020
    '    'mInvoice.InvoiceItems.CurrentItem.CCommercialRate = CDec(Val(txtDisplayCommercialRate.Text)) * Factor
    '    mInvoice.InvoiceItems.CurrentItem.CCommercialRate = CDec(Val(txtDisplayCommercialRate.Text))
    '    'End of Commneted and added On 30-Jan-2020

    '    txtCommercialRate.DataBind()
    '    upnlRateValues.Update()
    'End Sub
#End Region
End Class