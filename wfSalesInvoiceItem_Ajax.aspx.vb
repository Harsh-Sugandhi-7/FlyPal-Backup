Public Class wfSalesInvoiceItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mSalesInvoice As SalesInvoice
    Public BackPage As String
    Public mCurrency As Currency
    Dim mGSTPercentage As GSTPercentage
    Dim mVendor As Vendor
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mSalesInvoice = Session("mSalesInvoice")
    End Sub
    Private Sub setSession()
        Session("mSalesInvoice") = mSalesInvoice
    End Sub
    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQty').value,event)")
        txtRate.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtRate').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Sales Invoice Item [ " & mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemName & " ]"
            imgbtnPartNo.BackColor = Color.Silver
            txtPartNo.BackColor = Color.Silver
        Else
            lblTitle.Text = "Sales Invoice Item [New]"
        End If
    End Sub
    Private Function setObject() As Boolean
        mSalesInvoice.BeginEdit()
        mSalesInvoice.SalesInvoiceItems.CurrentItem.SrNo = mSalesInvoice.SalesInvoiceItems.CurrentIndex + 1
        mSalesInvoice.SalesInvoiceItems.CurrentItem.Qty = Val(txtQty.Text)
        mSalesInvoice.SalesInvoiceItems.CurrentItem.CRate = Val(txtRate.Text)
        mSalesInvoice.SalesInvoiceItems.CurrentItem.ConversionFactor = mSalesInvoice.ConversionFactor
        mSalesInvoice.SalesInvoiceItems.CurrentItem.COtherCharges = Val(txtOtherCharges.Text)
        mSalesInvoice.SalesInvoiceItems.CurrentItem.Remark = txtRemark.Text
        mSalesInvoice.SalesInvoiceItems.CurrentItem.Note = txtNote.Text

        '------------------------------------------------------------------
        If AppSettings("IsGSTApplicable") = "True" Then
            mVendor = Vendor.GetVendor(mSalesInvoice.VendorID)
            If mVendor.ClientCountryName.ToUpper = "INDIA" Then
                If mVendor.CountryName.ToUpper = "INDIA" And mSalesInvoice.SalesInvoiceDate >= CDate("01-Jul-2017") Then
                    mGSTPercentage = GSTPercentage.GetPercentage(mSalesInvoice.SalesInvoiceDate, 1, mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemID.ToString)
                    If Not mGSTPercentage Is Nothing Then
                        Dim mtmpItem As Item = Item.GetItem(mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemID)
                        If Len(mVendor.StateCode) > 0 Then
                            If mVendor.StateCode = mVendor.ClientStateCode Then
                                If mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTPercentage = 0 Then
                                    mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                    mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                End If
                                mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTCAmount = ((mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTPercentage * mSalesInvoice.SalesInvoiceItems.CurrentItem.CAmount) / 100)
                                mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTCAmount = ((mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTPercentage * mSalesInvoice.SalesInvoiceItems.CurrentItem.CAmount) / 100)
                                mSalesInvoice.StateCode = mVendor.StateCode
                                mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                                mSalesInvoice.VendorCountry = mVendor.CountryName
                                mSalesInvoice.Visibility = 1
                            Else
                                If mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTPercentage = 0 Then
                                    mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                End If
                                mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTCAmount = ((mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTPercentage * mSalesInvoice.SalesInvoiceItems.CurrentItem.CAmount) / 100)
                                mSalesInvoice.StateCode = mVendor.StateCode
                                mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                                mSalesInvoice.VendorCountry = mVendor.CountryName
                                mSalesInvoice.Visibility = 2
                            End If
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.HSNACSCode = mtmpItem.HSNACSCode
                        Else
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTPercentage = 0
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTPercentage = 0
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTCAmount = 0
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTCAmount = 0
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTPercentage = 0
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTCAmount = 0
                            mSalesInvoice.SalesInvoiceItems.CurrentItem.HSNACSCode = ""
                            mSalesInvoice.StateCode = mVendor.StateCode
                            mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                            mSalesInvoice.VendorCountry = mVendor.CountryName
                            mSalesInvoice.Visibility = 3
                        End If
                    End If
                Else
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTPercentage = 0
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTPercentage = 0
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTCAmount = 0
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTCAmount = 0
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTPercentage = 0
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTCAmount = 0
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.HSNACSCode = ""
                    mSalesInvoice.StateCode = mVendor.StateCode
                    mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                    mSalesInvoice.VendorCountry = mVendor.CountryName
                    mSalesInvoice.Visibility = 3
                End If
            Else
                mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTPercentage = 0
                mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTPercentage = 0
                mSalesInvoice.SalesInvoiceItems.CurrentItem.CGSTCAmount = 0
                mSalesInvoice.SalesInvoiceItems.CurrentItem.SGSTCAmount = 0
                mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTPercentage = 0
                mSalesInvoice.SalesInvoiceItems.CurrentItem.IGSTCAmount = 0
                mSalesInvoice.SalesInvoiceItems.CurrentItem.HSNACSCode = ""
                mSalesInvoice.StateCode = mVendor.StateCode
                mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                mSalesInvoice.VendorCountry = mVendor.CountryName
                mSalesInvoice.Visibility = 3
            End If
        Else
            mSalesInvoice.Visibility = 3
        End If
        '------------------------------------------------------------------

        If mSalesInvoice.SalesInvoiceItems.Contains(mSalesInvoice.SalesInvoiceItems.CurrentItem, mSalesInvoice.TransTypeID) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Sales Invoice Item", MsgBoxStyle.OkOnly, "")
            mSalesInvoice.CancelEdit()
            Return False
            Exit Function
        Else
            mSalesInvoice.ApplyEdit()
            mSalesInvoice.CalculateTotal()     'Added By Saylee on 10-Sep-2007
            If mSalesInvoice.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
                mSalesInvoice.RoundCGrandTotal()
            End If
        End If
        Return True
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No
                    Session("Sender") = ""
                    Response.Redirect("wfSalesInvoiceItem_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfSalesInvoiceItem_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfSalesInvoiceItem_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfSalesInvoiceItem_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Not (mSalesInvoice.CurrencyID.Equals(Guid.Empty)) Then
            mCurrency = Currency.GetCurrency(mSalesInvoice.CurrencyID)
            mSalesInvoice.SalesInvoiceItems.CurrentItem.Currency = mCurrency.Name
        Else
            mSalesInvoice.SalesInvoiceItems.CurrentItem.Currency = txtRateCurrency.Text
        End If
        DataBind()
    End Sub
    Public Sub customvalidate(s As Object, e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            Dim mPendingSalesInvoiceList As PendingSalesInvoiceList = PendingSalesInvoiceList.GetPendingToSalesInvoiceList(mSalesInvoice.VendorID, mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemName, mSalesInvoice.SalesInvoiceDate)
            Dim mBalanceQty As Decimal = 0D
            Dim MaxQty As Decimal = 0D
            For I As Integer = 0 To mPendingSalesInvoiceList.Count - 1
                If mPendingSalesInvoiceList(I).IssueItemID.Equals(mSalesInvoice.SalesInvoiceItems.CurrentItem.IssueItemID) And Not mSalesInvoice.SalesInvoiceItems.CurrentItem.IssueItemID.Equals(Guid.Empty) Then
                    mBalanceQty = mPendingSalesInvoiceList(I).BalanceQty
                    Exit For
                End If
            Next
            If Session("Edit") And Not mSalesInvoice.SalesInvoiceItems.CurrentItem.IsNew Then
                MaxQty = mBalanceQty + mSalesInvoice.SalesInvoiceItems.CurrentItem.Qty
            ElseIf mSalesInvoice.SalesInvoiceItems.CurrentItem.IsNew Then
                MaxQty = mBalanceQty
            End If
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
            Else
                If mSalesInvoice.TransTypeID = 23 Then
                    If Val(txtQty.Text) > MaxQty Then
                        custValidator.ErrorMessage = "Quantity can't be greater than Balance Qty."
                        e.IsValid = False
                    End If
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
    Private Sub Page_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        If Not IsPostBack Then
            If txtPartNo.Enabled = True Then
                setFocus(txtPartNo)
            End If
            DataFieldBind()
        End If
        SetPage()
    End Sub
    Private Sub btnBack_Click(sender As System.Object, e As System.EventArgs) Handles btnBack.Click
        If mSalesInvoice.SalesInvoiceItems.CurrentItem.IsNew And Not Session("Edit") = True Then mSalesInvoice.SalesInvoiceItems.Remove(mSalesInvoice.SalesInvoiceItems.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect("wfSalesInvoice_Ajax.aspx?BackPage=wfSalesInvoiceItem_Ajax.aspx")
    End Sub
    Private Sub imgbtnPartNo_Click(sender As System.Object, e As System.EventArgs) Handles imgbtnPartNo.Click
        setObject()
        Session("mSalesInvoice") = mSalesInvoice
        Session("PartNo") = txtPartNo.Text
        'Response.Redirect("wfPendingSalesInvoiceItem.aspx?BackPage=wfSalesInvoice.aspx&ChildPage=wfSalesInvoiceItem.aspx")
        If mSalesInvoice.TransTypeID = 23 Then  'Against  Issue
            Response.Redirect("wfPendingSalesInvoiceItem_Ajax.aspx?BackPage=wfSalesInvoice_Ajax.aspx&ChildPage=wfSalesInvoiceItem_Ajax.aspx")
        ElseIf mSalesInvoice.TransTypeID = 74 Then 'Against  None
            Response.Redirect("wfSearchPartListForSalesInvoice_Ajax.aspx?BackPage=wfSalesInvoice_Ajax.aspx&ChildPage=wfSalesInvoiceItem_Ajax.aspx")
        End If
    End Sub
    Private Sub SaveRecord(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

        If IsValid Then

            If setObject() Then

                Session("mSalesInvoice") = mSalesInvoice
                Session.Remove("Edit")

                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
                                MSGBox.Message_text.SavedSuccessFully,
                                "",
                                MsgBoxStyle.OkOnly, "")

                Response.Redirect("wfSalesInvoice_Ajax.aspx?BackPage=" & BackPage)

            End If

        End If

    End Sub

#End Region

End Class