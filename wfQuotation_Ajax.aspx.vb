Public Class wfQuotation_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum
    Private Enum RequstFor
        Supplier = 0
        Customer = 1
    End Enum
#End Region

#Region " Variable Declaration "
    Public mQuotation As Quotation
    Public mVendorList As VendorList
    Public mStatusList As StatusList
    Public mCurrencyList As CurrencyList
    Public mCustomerList As VendorList
    Public mPriorityList As PriorityList        'By Saylee on 11/10/07==========
    Public Flag As Integer                      'Kalpesh 
    Public mTransTypeID As Trans
    Dim mVendorTerms As VendorTerms             'Added By Prashant 26-Apr-2010
    Dim mModuleName As String                   'Added by Prashant on 20-July-2011
    Dim EventLogID As Guid                      'Added by Prashant on 20-July-2011
    Dim mPendingTransactionCount As PendingTransactionCount
    'GST Changes
    Public mGSTPercentage As GSTPercentage
    Public mVendor As Vendor
    Public mCompanyDetail As CompanyDetail
    'End
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mQuotation = Session("mQuotation")
        mVendorList = Session("mVendorList")
        mStatusList = Session("mStatusList")
        mCurrencyList = Session("mCurrencyList")
        mCustomerList = Session("mCustomerList")
        mTransTypeID = Session("mTransTypeId")
        mVendorTerms = Session("mVendorTerms")
        mModuleName = Session("mModuleName")    'Added by Prashant on 20-July-2011
        mPriorityList = Session("mPriorityList")
    End Sub
    Private Sub SetTransactionDate()
        Dim TransDate As String = IIf(Session("TransactionDate") Is Nothing, "", Session("TransactionDate"))
        If TransDate <> "" Then
            mQuotation.Date = CDate(TransDate)
            calQuotationDate.Text = mQuotation.DateFormatted.ToString
        End If
        Session("mQuotation") = mQuotation
        Session.Remove("TransactionDate")
    End Sub
    Private Sub setSession()
        Session("mQuotation") = mQuotation
        Session("mVendorList") = mVendorList
        Session("mStatusList") = mStatusList
        Session("mCurrencyList") = mCurrencyList
        Session("mCustomerList") = mCustomerList
        Session("mVendorTerms") = mVendorTerms
        Session("mModuleName") = mModuleName    'Added by Prashant on 20-July-2011
    End Sub
    Public Sub PendingTransCount()
        mPendingTransactionCount = PendingTransactionCount.GetCount(Today.Date.ToString, IIf(mTransTypeID = 33, 32, IIf(mTransTypeID = 36, 34, 35)))
        cmbAdd.Items.Clear()
        cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Multiple Parts", 1))
        'cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Add Parts From Enquiry (" + mPendingTransactionCount.EnquiryCountForQuotation.ToString + ")", 2))
        cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Multiple Parts From Enquiry", 2))
        If (AppSettings("NewRequisition") = "True" And mTransTypeID = Util.Trans.PurchaseQuotation) Then
            cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Add Requisition Items (" + mPendingTransactionCount.ReqItemCountForQuotation.ToString + ")", 3))
        ElseIf AppSettings("NewRequisition") = "False" Then 'End
            If (mTransTypeID = Util.Trans.PurchaseQuotation Or mTransTypeID = Util.Trans.Quotation) Then
                cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Store Approved part List", 3))
            End If '======================================================================
        End If
    End Sub
    Private Sub setObject()
        mQuotation.Date = CDate(calQuotationDate.Text)
        If txtValidUpToDate.Text = "" Then
            mQuotation.ValidDate = System.DBNull.Value
        Else
            mQuotation.ValidDate = CDate(txtValidUpToDate.Text)
        End If
        mQuotation.Text = txtText.Text
        mQuotation.No = Val(txtNo.Text)
        mQuotation.ValidDays = Val(txtValidDays.Text)
        mQuotation.UserName = User.Identity.Name
        mQuotation.Amend = txtAmend.Text 'Added By Vikrant On 22-Nov-2019 For ALL22112019

        Dim txtValue As TextBox
        Dim cmbValue As DropDownList
        Dim mQuotationItem As QuotationItem
        Dim i As Integer = 0
        For Each mQuotationItem In mQuotation.QuotationItems
            With mQuotationItem

                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtQty"), TextBox)
                .Qty = CDec(Val(txtValue.Text))

                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtRate"), TextBox)
                .CRate = CDec(Val(txtValue.Text))

                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtOtherCharges"), TextBox)
                .COtherCharges = CDec(Val(txtValue.Text))

                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtEOQ"), TextBox)
                .EOQ = CDec(Val(txtValue.Text))

                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtEOQCRate"), TextBox)
                .EOQCRate = CDec(Val(txtValue.Text))

                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtBillBackRate"), TextBox)
                .CBillBackRate = CDec(Val(txtValue.Text))

                cmbValue = CType(Me.dgQuotationItems.Rows(i).FindControl("cmbPriority"), DropDownList) '=======Added by Saylee on 11-Oct-2007=============
                .PriorityID = CInt(cmbValue.SelectedValue)

                'Added By Vikrant On 21-Dec-2016 For ALL21122016
                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtRemark"), TextBox)
                .Remark = Trim(txtValue.Text)

                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtNote"), TextBox)
                .Note = Trim(txtValue.Text)
                'End

                'GST Changes
                'mVendor = mVendorList(New Guid(cmbVendorList.SelectedValue)).StateName
                If AppSettings("IsGSTApplicable") = "True" And Not mQuotation.VendorID.Equals(Guid.Empty) Then
                    If mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName.ToUpper = "INDIA" And CDate(mQuotation.DateFormatted.ToString) >= CDate("01-Jul-2017") And mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientCountryName.ToUpper.Equals("INDIA") Then
                        'mGSTPercentage = GSTPercentage.GetPercentage(mQuotation.DateFormatted.ToString, 1, .ItemID.ToString)
                        'If Not mGSTPercentage Is Nothing Then
                        Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                        If Len(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode) > 0 Then
                            If mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode = mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode Then
                                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWCGST"), TextBox)
                                .CGSTPercentage = CDec(Val(txtValue.Text))

                                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWSGST"), TextBox)
                                .SGSTPercentage = Val(txtValue.Text.Trim)

                                .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .HSNACSCode = mtmpItem.HSNACSCode
                                .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
                                SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 1)
                            Else
                                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWIGST"), TextBox)
                                .IGSTPercentage = CDec(Val(txtValue.Text))
                                .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0

                                .HSNACSCode = mtmpItem.HSNACSCode
                                .TotalCAmount = .CAmount + .IGSTCAmount
                                SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 2)
                            End If
                        Else
                            .CGSTPercentage = 0
                            .SGSTPercentage = 0
                            .CGSTCAmount = 0
                            .SGSTCAmount = 0
                            .IGSTPercentage = 0
                            .IGSTCAmount = 0
                            .HSNACSCode = ""
                            SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                        End If
                        'End If
                    Else
                        .CGSTPercentage = 0
                        .SGSTPercentage = 0
                        .CGSTCAmount = 0
                        .SGSTCAmount = 0
                        .IGSTPercentage = 0
                        .IGSTCAmount = 0
                        .HSNACSCode = ""
                        SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                    End If
                Else
                    .CGSTPercentage = 0
                    .SGSTPercentage = 0
                    .CGSTCAmount = 0
                    .SGSTCAmount = 0
                    .IGSTPercentage = 0
                    .IGSTCAmount = 0
                    .HSNACSCode = ""
                    mQuotation.Visibility = 3
                End If
                'End
            End With
            i = i + 1
        Next

        mQuotation.VendorQuoteNo = txtCustQuoteNo.Text  '==========================By Saylee on 18/07/07===============================
        If txtCustQuoteDate.Text = "" Then
            mQuotation.VendorQuoteDate = System.DBNull.Value
        Else
            mQuotation.VendorQuoteDate = CDate(txtCustQuoteDate.Text)
        End If
        mQuotation.IsCustomer = chkIsCustomer.Checked  '==============================================================================

        mQuotation.OpeningLine = txtOpeningLine.Text    'Added By Saylee on 4-Oct-2007
        mQuotation.IsRoundOff = chkIsRoundOff.Checked   'Added by Prashant 25-Oct-2012
        mQuotation.CalculateTotal()                     'Added By Saylee on 10-Sep-2007
        mQuotation.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mQuotation.ConversionFactor = Val(txtConversionFactor.Text)
    End Sub
    Private Sub setVendorDetails()
        mQuotation.VendorID = New Guid(cmbVendorList.SelectedValue)
        mQuotation.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mQuotation.ConversionFactor = Val(txtConversionFactor.Text)
        mQuotation.IsCustomer = chkIsCustomer.Checked
        mQuotation.CustomerID = New Guid(cmbCustomer.SelectedValue) ' Saylee on 18/07/07
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mQuotation.QuotationItems.CurrentIndex = Index
        Session("mQuotation") = mQuotation
    End Sub
    Private Sub DeleteChargeRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
        mQuotation.QuotationCharges.CurrentIndex = Index
        Session("mQuotation") = mQuotation
    End Sub
    Private Sub DeleteQuotationTerms(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveTerm, MSGBox.Message_text.RemoveTerm, "", MsgBoxStyle.YesNo, "DeleteQuotationTerms")
        mQuotation.QuotationTerms.CurrentIndex = Index
        Session("mQuotation") = mQuotation
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("Sender") = ""
                            mQuotation = CType(Session("mQuotation"), Quotation)
                            mQuotation.QuotationItems.Remove(mQuotation.QuotationItems.CurrentItem)
                            mQuotation.CalculateTotal()             'Added By Saylee on 10-Sep-2007
                            If mQuotation.IsRoundOff = True Then    'ALL25102012
                                mQuotation.RoundCGrandTotal()
                            End If
                            Session("mQuotation") = mQuotation
                            cmbVendorList.Enabled = ((CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)) And mQuotation.VendorID.Equals(Guid.Empty)) Or mQuotation.QuotationItems.Count = 0
                            upnlQuotationDetails.Update()
                            upnlSupplierDetails.Update()
                            QuotationItemDataGrid()
                            ControlVisibility()
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCharge" Then
                        Try
                            Session("Sender") = ""
                            mQuotation = CType(Session("mQuotation"), Quotation)
                            mQuotation.QuotationCharges.Remove(mQuotation.QuotationCharges.CurrentItem)
                            mQuotation.CalculateTotal()             'Added By Saylee on 10-Sep-2007
                            If mQuotation.IsRoundOff = True Then    'ALL25102012
                                mQuotation.RoundCGrandTotal()
                            End If
                            Session("mQuotation") = mQuotation
                            QuotationChargesGrid()
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteQuotationTerms" Then
                        Try
                            Session("Sender") = ""
                            mQuotation = CType(Session("mQuotation"), Quotation)
                            mQuotation.QuotationTerms.Remove(mQuotation.QuotationTerms.CurrentItem)
                            Session("mQuotation") = mQuotation
                            dgQuotationTerms.DataSource = mQuotation.QuotationTerms
                            dgQuotationTerms.DataBind()
                            upnlQuotationTerms.Update()
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        If mQuotation.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
                            PendingTransCount()
                            Session.Remove("ItemsCount") 'Added by Prashant 30-Jul-2020 As once a Quo against Req item done then for new quo against req date was remaine disabled so removed session
                            Response.Redirect("Index.aspx")
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
                        If mQuotation.IsValid = True Then
                            mQuotation.StatusID = 2
                            'DataFieldBind()
                            Save()
                            DataFieldBind()
                            PendingTransCount()
                            ControlVisibility()
                            upnlQuotationTerms.Update()
                            upnlQuotationItems.Update()

                            'Added by Prashant On 26-Feb-2019 BA25022019
                            'If only one quoation is there then we are not geting SumOfQuotationBalanceQty so saved quoation fetched again for following msg
                            'other wise 
                            Dim mTempQuotation As Quotation = Nothing
                            mTempQuotation = Quotation.GetQuotation(mQuotation.ID)
                            If mTempQuotation.QuotationItems.Count > 0 Then 'Added by Prashant On 26-Feb-2019 BA25022019
                                For i As Integer = 0 To mTempQuotation.QuotationItems.Count - 1
                                    If mTempQuotation.QuotationItems(i).RequisitionItemQuotationItemsNew.Count > 0 Then
                                        For j As Integer = 0 To mTempQuotation.QuotationItems(i).RequisitionItemQuotationItemsNew.Count - 1
                                            If mTempQuotation.QuotationItems(i).RequisitionItemQuotationItemsNew(j).SumOfQuotationBalanceQty = 0 Then
                                                'Do nothing 
                                            Else
                                                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Do you want to remove Requisition items from Pending list.", MsgBoxStyle.YesNo, "SetQuotationBalQtyOfReqItemToZero")
                                                Exit Sub
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                            'End Added by Prashant On 26-Feb-2019 BA25022019
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "SetQuotationBalQtyOfReqItemToZero" Then 'Added by Prashant On 26-Feb-2019 BA25022019
                        If mQuotation.QuotationItems.Count > 0 Then
                            For i As Integer = 0 To mQuotation.QuotationItems.Count - 1
                                If mQuotation.QuotationItems(i).RequisitionItemQuotationItemsNew.Count > 0 Then
                                    mQuotation.UpdateSetQuotationBalQtyOfReqItemToZero(mQuotation.QuotationItems(i).RequisitionItemQuotationItemsNew(0).RequisitionItemID.ToString)
                                End If
                            Next
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mQuotation.StatusID = 4
                        DataFieldBind()
                        Save()
                        PendingTransCount()
                    End If
                    'Added By Vikrant On 22-Nov-2019 For ALL22112019
                    If MSGBoxCtrl.Sender = "AmendStatus" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If SaveAmendQuote() = True Then
                                'SendMail() Add Send Mail Button on 16-Jan-2017
                                'Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                                UpdatePanel()
                                upnlQuotationItems.Update()
                                upnlQuotationCharges.Update()
                                upnlQuotationTerms.Update()
                            End If
                        Else
                            Session.Remove("IsValid")
                        End If
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session("mQuotation") = mQuotation
                        DataFieldBind()
                        PendingTransCount()
                        UpdatePanel()
                        upnlQuotationItems.Update()
                        upnlQuotationCharges.Update()
                        upnlQuotationTerms.Update()
                    End If
                    'Added By Vikrant On 22-Nov-2019 For ALL22112019
                    If MSGBoxCtrl.Sender = "AmendStatus" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        If mQuotation.StatusID = 2 Then
                            mQuotation.StatusID = 1
                        ElseIf mQuotation.StatusID = 3 Or mQuotation.StatusID = 4 Then
                            mQuotation.StatusID = 2
                        End If
                        Session("mQuotation") = mQuotation
                        DataFieldBind()
                        UpdatePanel()
                    End If
                    'End
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "vendornotvalid" Then
                        cmbVendorList.ClearSelection()
                        upnlSupplierDetails.Update()
                    End If
            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
        txtValidDays.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtValidDays').value,event)")
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        Dim mQuotationItem As QuotationItem
        Dim i As Integer = 0
        For Each mQuotationItem In mQuotation.QuotationItems
            With mQuotationItem
                Try
                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtQty"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtRate"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtOtherCharges"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtEOQ"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtEOQCRate"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtBillBackRate"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWCGST"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWIGST"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWSGST"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                Catch ex As Exception
                End Try
            End With
            i = i + 1
        Next
        upnlQuotationItems.Update()
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)
        btnAdd.Enabled = IIf(StatusId > 1, False, True)
        cmbAdd.Enabled = IIf(StatusId > 1, False, True)
        btnAddTerm.Enabled = IIf(StatusId > 1, False, True)
        btnAddSupplierSpecificTerms.Enabled = IIf(StatusId > 1, False, True)
        btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True)
        dgQuotationItems.Columns(28).Visible = IIf(StatusId > 1, False, True)
        'dgQuotationItems.Columns(28).Visible = IIf(StatusId > 1, False, True)
        'dgQuotationItems.Columns(29).Visible = IIf(StatusId > 1, False, True)
        dgChargeList.Columns(4).Visible = IIf(StatusId > 1, False, True)
        'dgChargeList.Columns(5).Visible = IIf(StatusId > 1, False, True)
        dgQuotationTerms.Columns(2).Visible = IIf(StatusId > 1, False, True)
    End Sub
    Private Sub SetPage()
        If mTransTypeID = 33 Then                  'Added By Prashant 26/12/2007
            If mQuotation.No > 0 Then
                lblTitle.Text = "Outright Quotation [" & mQuotation.Text + "-" + CType(mQuotation.No, String) + IIf(mQuotation.Amend = "", "", " (" & mQuotation .Amend & ") ").ToString + "]"
            Else
                lblTitle.Text = "Outright Quotation [New]"
            End If
        ElseIf mTransTypeID = 36 Then
            If mQuotation.No > 0 Then
                lblTitle.Text = "Repair / Overhaul Quotation [" & mQuotation.Text + "-" + CType(mQuotation.No, String) + IIf(mQuotation.Amend = "", "", " (" & mQuotation.Amend & ") ").ToString + "]"
            Else
                lblTitle.Text = "Repair / Overhaul Quotation [New]"
            End If
        ElseIf mTransTypeID = 37 Then
            If mQuotation.No > 0 Then
                lblTitle.Text = "Rental / Lease Quotation [" & mQuotation.Text + "-" + CType(mQuotation.No, String) + IIf(mQuotation.Amend = "", "", " (" & mQuotation.Amend & ") ").ToString + "]"
            Else
                lblTitle.Text = "Rental / Lease Quotation [New]"
            End If
        ElseIf mTransTypeID = 2 Then
            If mQuotation.No > 0 Then
                lblTitle.Text = "Sales Quotation [" & mQuotation.Text + "-" + CType(mQuotation.No, String) + IIf(mQuotation.Amend = "", "", " (" & mQuotation.Amend & ") ").ToString + "]"
            Else
                lblTitle.Text = "Sales Quotation [New]"
            End If
        End If

        If mQuotation.TransTypeID = 2 Then
            lblVendorDetail.InnerText = "Customer Details"
        Else
            lblVendorDetail.InnerText = "Supplier Details"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub ControlVisibilityForGrid()
        Dim txtValue As TextBox
        Dim cmbValue As DropDownList
        For i As Integer = 0 To dgQuotationItems.Rows.Count - 1
            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtQty"), TextBox)
            'txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)
            If AppSettings("NewRequisition") = "True" Then  'Added by vikrant For New Requisition
                txtValue.Enabled = (mQuotation.StatusID = 1 And mQuotation.QuotationItems(i).RequisitionItemQuotationItemsNew.Count = 0)
            End If


            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtRate"), TextBox)
            txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)

            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtOtherCharges"), TextBox)
            txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)

            '==================Saylee on 18/07/07================================
            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtEOQ"), TextBox)
            txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)

            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtEOQCRate"), TextBox)
            txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)

            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtBillBackRate"), TextBox)
            txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)

            'Added By Vikrant On 21-Dec-2016 For ALL21122016
            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtRemark"), TextBox)
            txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)

            txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtNote"), TextBox)
            txtValue.Enabled = CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)
            'End
            '=====================================================================
            '============Added By Saylee on 11-Oct-2007============
            cmbValue = CType(Me.dgQuotationItems.Rows(i).FindControl("cmbPriority"), DropDownList)

            If mQuotation.QuotationItems.Item(i).EnquiryItemID.Equals(Guid.Empty) And mQuotation.QuotationItems.Item(i).QuotationItemRequisitionItems.Count = 0 Then
                cmbValue.Enabled = (True And CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))
            Else
                cmbValue.Enabled = False
            End If

        Next
    End Sub
    'Added By Vikrant On 22-Nov-2019 For ALL22112019
    Private Function SaveAmendQuote() As Boolean
        If mQuotation.StatusID = 3 And CType(Session("Amend"), String) = "Yes" Then
            Dim mAmendQuote As Quotation

            mQuotation.StatusID = 1
            mQuotation.AmendedStatus = True
            mQuotation.AmendCount = mQuotation.AmendCount + 1

            mAmendQuote = Quotation.GetAmendedQuote(mQuotation)
            mQuotation = CType(mQuotation.Save(), Quotation)

            mAmendQuote = CType(mAmendQuote.Save(), Quotation)
            'To make StatusID  as MarkDirty  again set it to 2 and then 1 
            mQuotation.StatusID = 2
            mQuotation.StatusID = 1
            mQuotation.AmendedStatus = False
            mQuotation = CType(mQuotation.Save(), Quotation)
            mQuotation = Quotation.GetQuotation(mQuotation.ID) 'We are not geting ERo Qty first time so to get it fetch order again
            '''SetControlStatusAfterAmendOrder(mOrder.StatusID)

            Dim QuotationDetail As String = mQuotation.QuotationNo + " Dated : " + mQuotation.DateFormatted.ToString + " from " + mVendorList(mQuotation.VendorID).Name & " Created By : " & mQuotation.UserName
            MarkLog(Util.Action.Amend, mModuleName, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID)
            Session("Amend") = ""
        End If
        Session("mQuotation") = mQuotation
        SetPage()
        Return True
    End Function
    Private Sub ControlVisibility()
        txtText.Enabled = IIf(mQuotation.StatusID >= 2, False, True)                                ' Added By Prashant 20/12/2007
        txtNo.Enabled = IIf(mQuotation.StatusID >= 2, False, True)                                  'Added By Prashant 20/12/2007
        '''txtAmend.Enabled = CType(IIf(mQuotation.StatusID >= 2 Or mQuotation.ReceiptCount > 0 Or ((mQuotation.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.IssueCount > 0) Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True), Boolean)
        'cmbVendorList.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean) And mQuotation.QuotationItems.Count = 0) Or (mQuotation.QuotationItems.Count = 0)
        cmbVendorList.Enabled = ((CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)) And mQuotation.VendorID.Equals(Guid.Empty)) Or mQuotation.QuotationItems.Count = 0
        cmbCurrencyList.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))
        txtConversionFactor.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))
        calQuotationDate.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean) And mQuotation.QuotationItems.Count = 0) Or (mQuotation.QuotationItems.Count = 0)
        btnAuthorized.Visible = (Not mQuotation.IsNew) And (mQuotation.StatusID = 1)
        btnCancel.Visible = (Not mQuotation.IsNew) And (mQuotation.StatusID = 2)
        cmbCurrencyList.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))
        chkIsCustomer.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))
        If (mQuotation.StatusID >= 2) Then
            cmbCustomer.Enabled = False
        Else
            If chkIsCustomer.Checked = False Then
                cmbCustomer.Enabled = False
            Else
                cmbCustomer.Enabled = True
            End If
        End If
        ControlVisibilityForGrid()
        txtCustQuoteNo.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))       '================== By Saylee on 18/07/07=================================
        txtCustQuoteDate.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))     '=====================================================================
        lblCustomerQuoteNo.Visible = IIf((CType(mQuotation.TransTypeID, Trans) = Util.Trans.Quotation), False, True) '===========Added By Saylee on 10th-Sep-2007================
        txtCustQuoteNo.Visible = IIf((CType(mQuotation.TransTypeID, Trans) = Util.Trans.Quotation), False, True)
        txtCustQuoteDate.Visible = IIf((CType(mQuotation.TransTypeID, Trans) = Util.Trans.Quotation), False, True)
        lblCustomerQuoteDate.Visible = IIf((CType(mQuotation.TransTypeID, Trans) = Util.Trans.Quotation), False, True)
        txtValidDays.Enabled = IIf(mQuotation.StatusID >= 2, False, True)
        txtValidUpToDate.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))     '==========================================================
        chkIsRoundOff.Enabled = (CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean))
        If Not IsInRole(Rights.Authorized) Then                                                     'Added By Prashant 17-Aug-2011
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
        End If
        'GST Changes
        If mQuotation.Visibility = 1 Or mQuotation.Visibility = 2 Then
            Dim txtCGSTPercentage As TextBox
            Dim txtSGSTPercentage As TextBox
            Dim txtIGSTPercentage As TextBox

            For i As Integer = 0 To dgQuotationItems.Rows.Count - 1
                txtCGSTPercentage = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWCGST"), TextBox)
                txtSGSTPercentage = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWSGST"), TextBox)
                txtIGSTPercentage = CType(Me.dgQuotationItems.Rows(i).FindControl("txtWIGST"), TextBox)

                txtCGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mQuotation.StatusID >= 2 Or mQuotation.QuotationItems(i).HSNACSCode = "", True, False) 'CGSTPercentage 
                'txtSGSTPercentage.Enabled = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mQuotation.StatusID = 2 Or mQuotation.StatusID = 4, True, False ) 'SGSTPercentage 
                txtIGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mQuotation.StatusID >= 2 Or mQuotation.QuotationItems(i).HSNACSCode = "", True, False) 'IGSTPercentage 

                txtCGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mQuotation.StatusID >= 2 Or mQuotation.QuotationItems(i).HSNACSCode = "", Color.Gainsboro, Color.White) 'CGSTPercentage 
                'txtSGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mQuotation.StatusID = 2 Or mQuotation.StatusID = 4, Color.Gainsboro, Color.White ) 'SGSTPercentage 
                txtIGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mQuotation.StatusID >= 2 Or mQuotation.QuotationItems(i).HSNACSCode = "", Color.Gainsboro, Color.White) 'IGSTPercentage 
            Next
        End If
        If mQuotation.Visibility = 1 Then
            dgQuotationItems.Columns(11).Visible = True 'HSNACSCode 
            dgQuotationItems.Columns(12).Visible = True 'CGSTPercentage 
            dgQuotationItems.Columns(13).Visible = True 'CGSTCAmount 
            dgQuotationItems.Columns(14).Visible = True 'SGSTPercentage 
            dgQuotationItems.Columns(15).Visible = True 'SGSTCAmount 
            dgQuotationItems.Columns(16).Visible = False 'IGSTPercentage 
            dgQuotationItems.Columns(17).Visible = False 'IGSTCAmount 

            lblTotalCGST.Visible = True
            txtTotalCGST.Visible = True
            lblTotalSGST.Visible = True
            txtTotalSGST.Visible = True

            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False
        ElseIf mQuotation.Visibility = 2 Then
            dgQuotationItems.Columns(11).Visible = True  'HSNACSCode 
            dgQuotationItems.Columns(12).Visible = False 'CGSTPercentage 
            dgQuotationItems.Columns(13).Visible = False 'CGSTCAmount 
            dgQuotationItems.Columns(14).Visible = False 'SGSTPercentage 
            dgQuotationItems.Columns(15).Visible = False 'SGSTCAmount 
            dgQuotationItems.Columns(16).Visible = True  'IGSTPercentage 
            dgQuotationItems.Columns(17).Visible = True 'IGSTCAmount 

            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False

            lblTotalIGST.Visible = True
            txtTotalIGST.Visible = True
        ElseIf mQuotation.Visibility = 3 Then
            dgQuotationItems.Columns(11).Visible = False 'HSNACSCode 
            dgQuotationItems.Columns(12).Visible = False 'CGSTPercentage 
            dgQuotationItems.Columns(13).Visible = False 'CGSTCAmount 
            dgQuotationItems.Columns(14).Visible = False 'SGSTPercentage 
            dgQuotationItems.Columns(15).Visible = False 'SGSTCAmount 
            dgQuotationItems.Columns(16).Visible = False  'IGSTPercentage 
            dgQuotationItems.Columns(17).Visible = False 'IGSTCAmount 
            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False
            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False
        End If
        'End
        'Added By Vikrant On 22-Nov-2019 For ALL22112019
        Dim mShowTopAmendedOrderNo As ShowTopAmendedOrderNo
        mShowTopAmendedOrderNo = ShowTopAmendedOrderNo.GetTopAmendedOrderNo(mQuotation.Text, mQuotation.No, 1)
        btnAmend.Visible = Not mQuotation.IsNew And mQuotation.OrderCount <= 0 And (mQuotation.StatusID = 2 Or (mQuotation.StatusID = 3 And mShowTopAmendedOrderNo.ID.Equals(mQuotation.ID))) And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))
        txtAmend.Enabled = CType(IIf(mQuotation.StatusID >= 2 Or mQuotation.OrderCount > 0, False, True), Boolean)
        'End
    End Sub
    Private Sub Save()
        'Authentication
        If Not mQuotation.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------
                If DateDiff(DateInterval.Day, CDate(mQuotation.Date), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Quotation. <br> Quotation Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        'Authentication
        Dim QuotationClone As Quotation
        QuotationClone = mQuotation.Clone
        Try
            If Not mQuotation.QuotationItems.Count = 0 Then
                setObject()
                setVendorDetails()

                If mQuotation.VendorID.Equals(mQuotation.CustomerID) Then 'Added By Rajnish On 16-01-2008
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Supplier & Customer are same. <br><br> Select another Customer from list.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                If mVendorList(mQuotation.VendorID).NotInUse = True Then 'Added by Saylee on 24-Jul-2012
                    If CDate(mVendorList(mQuotation.VendorID).NotInUseDate) <= CDate(mQuotation.Date) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Supplier is not applicable since " + mVendorList(mQuotation.VendorID).NotInUseDateFormatted.ToString + " <br><br> Select another Supplier from list or select date before " + mVendorList(mQuotation.VendorID).NotInUseDateFormatted.ToString + " & try again", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If

                If mQuotation.IsCustomer = True Then
                    If mVendorList(mQuotation.CustomerID).NotInUse = True Then
                        If CDate(mVendorList(mQuotation.CustomerID).NotInUseDate) <= CDate(mQuotation.Date) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Customer is not applicable since " + mVendorList(mQuotation.CustomerID).NotInUseDateFormatted.ToString + " <br><br> Select another Customer from list or select date before " + mVendorList(mQuotation.CustomerID).NotInUseDateFormatted.ToString + " & try again", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If
                End If '*************************

                Session("mQuotation") = mQuotation

                Dim mQuotationCharge As QuotationCharge '============= Added By Rajnish On 01-01-2008
                For Each mQuotationCharge In mQuotation.QuotationCharges
                    If (mQuotationCharge.Sign <> 1 And mQuotationCharge.CChargeAmount <= 0) Or (Not (mQuotationCharge.IsValid)) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Quotation Charge(s) are not allowed if Quotation Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                        mQuotation.CancelEdit()
                        Exit Sub
                    End If
                Next                                    '=============

                '' mQuotation.ApplyEdit()
                If mQuotation.IsRoundOff = True Then    'ALL25102012
                    mQuotation.RoundCGrandTotal()
                End If

                'Added by Utkarsh ON 21-Nov-2013 FOr TransTextSeries
                'Check if Quotation is blank then call TransTextSeries UI

                If (mQuotation.IsNew) And (mQuotation.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mQuotation.TransTypeID, mQuotation.DateFormatted.ToString)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mQuotation.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mQuotation.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mQuotation.TransTypeID).TransText = "")) Then

                        Dim str = "<script language='javascript'>openledgersame('wfQuotation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "Quotation"
                        Session("TransTypeID_ForTransSeries") = mQuotation.TransTypeID
                        Session("TransDate_ForTransSeries") = mQuotation.DateFormatted.ToString
                        Session("AddTransTextSeries") = "True"
                        'Dim msg1 As New SIMsgBox(Page, "Quotation Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfQuotation.aspx?BackPage=" & Request.QueryString("BackPage")
                        'msg1.Show()
                        'Session("sender") = "QuotationTransTextSeriesAlert"
                        'Exit Sub
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    Else
                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then
                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mQuotation.TransTypeID)
                                mQuotation.Text = .TransText
                                mQuotation.No = .StartingTransNo
                            End With
                        Else
                            Dim str = "<script language='javascript'>openledgersame('wfQuotation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Quotation"
                            Session("TransTypeID_ForTransSeries") = mQuotation.TransTypeID
                            Session("TransDate_ForTransSeries") = mQuotation.DateFormatted.ToString
                            Session("AddTransTextSeries") = "True"
                            'Dim msg1 As New SIMsgBox(Page, "Quotation Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly)
                            'msg1.ReplacePage = "wfQuotation.aspx?BackPage=" & Request.QueryString("BackPage")
                            'msg1.Show()
                            'Session("sender") = "QuotationTransTextSeriesAlert"
                            'Exit Sub
                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                        End If
                    End If
                End If
                mQuotation.Save()
                Dim QuotationDetail As String = mQuotation.QuotationNo + " Dated : " + mQuotation.DateFormatted.ToString + " from " + mVendorList(mQuotation.VendorID).Name

                If mQuotation.StatusID = 2 Then
                    MarkLog(Util.Action.Authorize, mModuleName, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID)
                ElseIf mQuotation.StatusID = 3 Then
                    MarkLog(Util.Action.Amend, mModuleName, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID)
                ElseIf mQuotation.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, mModuleName, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, mModuleName, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID)
                End If

                mQuotation.MarkClean()
                Session("mQuotation") = mQuotation
                SetPage()
                UpdatePanel()
                QuotationItemDataGrid()
                ControlVisibilityForGrid()
                QuotationChargesGrid()
                QuotationTermsGrid()
                SetChargeGrid()
                If mQuotation.StatusID = 2 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.AuthorizedSuccessFully, MSGBox.Message_text.AuthorizedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                ElseIf mQuotation.StatusID = 4 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                End If

            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Quotation can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As SqlClient.SqlException
            Session("QuotationClone") = QuotationClone
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
                If InStr(ex.Message, "CCtabRequisitionItemQuotationBalQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabRequisitionItemQuotationBalQty", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Quotation Qty can not be greater than Requisition Qty.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "FKtabQuotationTermtabTerm", CompareMethod.Text) Then
                    MSGBoxCtrl.show("Alert!", "Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "FKtabQuotationChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.show("Alert!", "Other Charge Deleted! ", "Other Charge Not Avalable<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "CCtabQuotationItemRate", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Rate Required if Stored Approval Qty. greater than Zero.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        Finally
            QuotationClone = Nothing
        End Try
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        Select Case mQuotation.TransTypeID
            Case Util.Trans.Quotation
                IsInRoleString = "Quotation"
            Case Util.Trans.PurchaseQuotation
                IsInRoleString = "PurchaseQuotation"
            Case Util.Trans.OverHaulRepairQuotation
                IsInRoleString = "PurchaseQuotationRepairOverHaul"
            Case Util.Trans.RentialLeaseQuotation
                IsInRoleString = "PurchaseQuotationRentalLease"
        End Select
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Authorized 'Added By Prashant 17-Aug-2011
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Function getVendorStatus(ByVal TransTypeID As Integer, ByVal Type As RequstFor) As Boolean
        If Type = RequstFor.Supplier Then                                  'Purchase Quotation
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.PurchaseQuotation
                    Return True
                Case Util.Trans.OverHaulRepairQuotation
                    Return True
                Case Util.Trans.RentialLeaseQuotation
                    Return True
                Case Else
                    Return False
            End Select
        ElseIf Type = RequstFor.Customer Then                              'Sales Quotaion
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.Quotation
                    Return True
                Case Else
                    Return False
            End Select
        End If
    End Function
    Private Sub SetChargeGrid()
        For j As Integer = 0 To dgChargeList.Rows.Count - 1
            If (Me.dgChargeList.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or Me.dgChargeList.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then
                dgChargeList.Rows.Item(j).Cells(4).Visible = False
                'dgChargeList.Rows.Item(j).Cells(4).Enabled = False
                'dgChargeList.Rows.Item(j).Cells(5).Enabled = False
            End If
        Next
    End Sub
    Private Sub UpdatePanel()
        ControlsDataBind()
        upnlStatusName.Update()
        upnlQuotationDetails.Update()
        upnlSupplierDetails.Update()
        upnlOtherDetails.Update()
        upnlButtons.Update()
        SetControlStatus(mQuotation.StatusID)
        ControlVisibility()
    End Sub
    Private Sub ControlVisibilityForFileAttachment()
        If mQuotation.Size > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = IIf(mQuotation.StatusID > 1, False, True)
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
        btnSelectFile.Disabled = IIf(mQuotation.StatusID > 1, True, False)
    End Sub
    Private Sub AttachMyFile()
        Try
            mQuotation.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mQuotation.Size = Session("FileUpload.FileSize")
            mQuotation.Extension = Session("FileUpload.FileExtension")
            Session("mQuotation") = mQuotation
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForFileAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendortList(0, , , , , , True, getVendorStatus(mQuotation.TransTypeID, RequstFor.Customer), getVendorStatus(mQuotation.TransTypeID, RequstFor.Supplier))
        Session("mVendorList") = mVendorList
        cmbVendorList.DataSource = mVendorList
        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        Session("mCurrencyList") = mCurrencyList
        cmbCurrencyList.DataSource = mCurrencyList
        mStatusList = StatusList.GetStatusList(mQuotation.StatusID, 1, True)
        Session("mStatusList") = mStatusList
        dgQuotationItems.DataSource = mQuotation.QuotationItems
        dgChargeList.DataSource = mQuotation.QuotationCharges
        dgQuotationTerms.DataSource = mQuotation.QuotationTerms
        calQuotationDate.Text = mQuotation.DateFormatted.ToString
        If txtValidUpToDate.Text = "" Then
            txtValidUpToDate.Text = ""
        Else
            txtValidUpToDate.Text = mQuotation.ValidDateFormatted.ToString
        End If
        mCustomerList = VendorList.GetVendortList(0, , , , , , True, True, , )  '=============================By Saylee 17/07/07============================
        Session("mCustomerList") = mCustomerList
        cmbCustomer.DataSource = mCustomerList
        If txtCustQuoteDate.Text = "" Then
            txtCustQuoteDate.Text = ""
        Else
            txtCustQuoteDate.Text = mQuotation.VendorQuoteDateFormatted.ToString
        End If                                                                  '==========================================================================
        mPriorityList = PriorityList.GetPriorityList(, , "")
        Session("mPriorityList") = mPriorityList
        DataBind()
    End Sub
    Private Sub ControlsDataBind()
        dgQuotationItems.DataBind()
        dgQuotationTerms.DataBind()
        dgChargeList.DataBind()
        upnlStatusName.DataBind()
        upnlQuotationDetails.DataBind()
        upnlSupplierDetails.DataBind()
        upnlOtherDetails.DataBind()
        upnlButtons.DataBind()
    End Sub
    Private Sub QuotationItemDataGrid()
        mPriorityList = PriorityList.GetPriorityList(, , "")
        Session("mPriorityList") = mPriorityList
        dgQuotationItems.DataSource = mQuotation.QuotationItems
        dgQuotationItems.DataBind()
        upnlQuotationItems.Update()
        upnlOtherDetails.Update()
        upnlOtherDetails.DataBind()
    End Sub
    Private Sub QuotationChargesGrid()
        dgChargeList.DataSource = mQuotation.QuotationCharges
        dgChargeList.DataBind()
        upnlQuotationCharges.Update()
        upnlOtherDetails.Update()
        upnlOtherDetails.DataBind()
    End Sub
    Private Sub QuotationTermsGrid()
        dgQuotationTerms.DataSource = mQuotation.QuotationTerms
        dgQuotationTerms.DataBind()
        upnlQuotationTerms.Update()
        upnlOtherDetails.Update()
        upnlOtherDetails.DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQuotationDate" Then
            If calQuotationDate.Text = "" Then
                custValidator.ErrorMessage = "Select Quotation Date."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 Then
                If mQuotation.TransTypeID = 2 Then
                    custValidator.ErrorMessage = "Select Customer from the list."
                Else
                    custValidator.ErrorMessage = "Select Vendor from the list."
                End If
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbCurrencyList" Then
            If cmbCurrencyList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Currency from the List."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtConversionFactor" Then
            If Val(txtConversionFactor.Text) <= 0 Then
                custValidator.ErrorMessage = "Currency factor must be greater than zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbCustomer" Then
            If (cmbCustomer.SelectedIndex <= 0) And (chkIsCustomer.Checked = True) Then
                custValidator.ErrorMessage = "Select Customer from the list."
                e.IsValid = False
            End If
            If (chkIsCustomer.Checked = True) Then
                cmbCustomer.Enabled = True
            End If
        End If
    End Sub
    'GST Changes
    Private Sub SetQuotationDetails(ByVal stateCode As String, ByVal ClientStateCode As String, ByVal CountryName As String, ByVal Visibility As Integer)
        mQuotation.StateCode = stateCode
        mQuotation.ClientStateCode = ClientStateCode
        mQuotation.VendorCountry = CountryName
        mQuotation.Visibility = Visibility
    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        SetControlStatus(mQuotation.StatusID)
        EventLogID = CType(Session("EventLogID"), Guid)                 'Added by Prashant   on 20-July-2011

        If CType(Session("AddEnquiryParts"), String) = "True" Then      'Add selected part(s) to Quotation Items
            SetTransactionDate()
            AddEnquiryParts()
            Session("AddEnquiryParts") = "False"
        Else
            Session("AddEnquiryParts") = "False"
        End If                                                          '--------------------------------------------------------------------------------

        If CType(Session("AddRequisitionParts"), String) = "True" Then  'added by Prashant 07/08/07 'Add selected part(s) to Enquiry Items
            SetTransactionDate()
            AddRequisitionParts()
            Session("AddRequisitionParts") = "False"
            Session("AddPart") = "False"
        Else
            Session("AddRequisitionParts") = "False"
            Session("AddPart") = "False"
        End If                                                          '-----------------------------------------------------

        If CType(Session("AddParts"), String) = "True" Then
            SetTransactionDate()
            AddMultipleParts()
            Session("AddParts") = "False"
        Else
            Session("AddParts") = "False"
        End If

        If Not IsPostBack And Session("sender") = "" Then
            If AppSettings("AutoCompleteTransText") = "False" Then 'Added By Utkarsh ON 23-May-2012 FOR 23052012 
                If txtText.Enabled = True Then
                    setFocus(txtText)
                End If
            End If

            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then 'Added by Utkarsh on 21-Nov-2013 for Trans Text Series
                If mQuotation.IsNew Then
                    mQuotation.Text = Session("TransText_ForTransSeries")
                    txtText.Text = mQuotation.Text
                    Session("mEnquiry") = mQuotation
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If 'End

            DataFieldBind()
            PendingTransCount()

        End If
        SetPage()
        ControlVisibility()
        If mQuotation.IsRoundOff = True Then  'Added By Prashant on 21-May-2012
            SetChargeGrid()
        End If
        ControlVisibilityForFileAttachment()
        TextChanged(sender, e)
        Session("ItemsCount") = mQuotation.QuotationItems.Count
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If IsValid = False Then upnlValidationsummary.Update() : Exit Sub
        If cmbAdd.SelectedValue = "0" Then
            setObject()
            setVendorDetails()
            mQuotation.QuotationItems.Add(mQuotation.ID, mQuotation.ValidDays)
            mQuotation.QuotationItems.CurrentItem.Currency = cmbCurrencyList.SelectedItem.Text
            mQuotation.QuotationItems.CurrentItem.ConversionFactor = txtConversionFactor.Text
            Session("mQuotation") = mQuotation
            Response.Redirect("wfQuotationItem_Ajax.aspx?BackPage=wfQuotation_Ajax.aspx")
        End If
        If cmbAdd.SelectedValue = "1" Then
            setObject()
            setVendorDetails()
            setSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWindow", "OpenPartsWindow('" + mQuotation.QuotationItems.Count.ToString + "', '" + mQuotation.DateFormatted.ToString + "');", True)
        End If
        If cmbAdd.SelectedValue = "2" Then
            setVendorDetails()
            setObject()
            setSession()
            Dim str As String
            str = "openledgersame('wfEnquiriesForQuotation_Ajax.aspx?BackPage=wfQuotation_Ajax.aspx&BackPage1=wfQuotation_Ajax.aspx&Date=" & mQuotation.DateFormatted.ToString & "&VendorID = " & mQuotation.VendorID.ToString & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End If
        If cmbAdd.SelectedValue = "3" Then  'Store Approval List
            setObject()
            setVendorDetails()
            setSession()
            Dim str As String
            Session("TransDate") = mQuotation.DateFormatted.ToString
            Session("QuotationItem") = Guid.Empty
            Session("ListFor") = 1

            Session("TransTypeID") = mQuotation.TransTypeID
            If mQuotation.TransTypeID = Util.Trans.PurchaseQuotation Then
                Session("CustomerID") = Guid.Empty
            ElseIf mQuotation.TransTypeID = Util.Trans.Quotation Then
                Session("CustomerID") = mQuotation.VendorID
            End If

            If AppSettings("NewRequisition") = "True" Then   'Changed by vikrant For New Requisition
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenWindow", "OpenWindow(" + mQuotation.QuotationItems.Count.ToString + "," + mQuotation.DateFormatted.ToString + ");", True)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenWindow", "OpenWindow(" + mQuotation.QuotationItems.Count.ToString + ",'" + mQuotation.DateFormatted.ToString + "');", True)
                Session("RequisitionItemSelected") = "RequisitionItemSelected"  'Added by Prashant On 26-Feb-2019 BA25022019
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWindow", "OpenReqPartsWindow('" + mQuotation.QuotationItems.Count.ToString + "', '" + mQuotation.DateFormatted.ToString + "');", True)
            Else 'End
                str = "openledgersame('wfStoreApprovalList.aspx?BackPage=wfQuotation_Ajax.aspx&LookinTypeID=1 &Name=');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            End If
        End If
    End Sub
    Private Sub btnAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            mQuotation.QuotationCharges.Add(mQuotation.ID)
            Session("mQuotation") = mQuotation
            'Response.Redirect("wfQuotationCharge_Ajax.aspx?BackPage=wfQuotation_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenQuotationChargeWindow", "OpenQuotationChargeWindow();", True)
        End If
    End Sub
    Private Sub btnAddTerm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTerm.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            Session("mQuotation") = mQuotation
            'Response.Redirect("wfQuotationTerm_Ajax.aspx?BackPage=wfQuotation_Ajax.aspx&Type=4")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTermWindow", "OpenTermWindow()", True)
        End If
    End Sub
    Private Sub btnAddSupplierSpecificTerms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSupplierSpecificTerms.Click
        mVendorTerms = VendorTerms.GetVendorTerms(New Guid(cmbVendorList.SelectedValue), mQuotation.TransTypeID, mQuotation.ID.ToString, 4)
        Dim i As Integer = 0
        While i < mVendorTerms.Count
            If mQuotation.QuotationTerms.Contains(mVendorTerms.Item(i).TermID) = False Then
                mQuotation.QuotationTerms.Add(mQuotation.ID)
                mQuotation.QuotationTerms.CurrentItem.Terms = mVendorTerms.Item(i).Terms
                mQuotation.QuotationTerms.CurrentItem.TermID = mVendorTerms.Item(i).TermID
            End If
            i = i + 1
        End While
        dgQuotationTerms.DataSource = mVendorTerms
        dgQuotationTerms.DataBind()
    End Sub
    Private Sub dgQuotationItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgQuotationItems.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgQuotationItems.PageIndex * dgQuotationItems.PageSize
                Session("Edit") = True
                setObject()
                setVendorDetails()
                mQuotation.QuotationItems.CurrentIndex = Index
                Session("mQuotation") = mQuotation
                Response.Redirect("wfQuotationItem_Ajax.aspx?BackPage=wfQuotation_Ajax.aspx")
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgQuotationItems.PageIndex * dgQuotationItems.PageSize
                DeleteRecord(Index)
        End Select
    End Sub
    Private Sub dgQuotationTerms_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgQuotationTerms.RowCommand
        Select Case e.CommandName
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgQuotationTerms.PageIndex * dgQuotationTerms.PageSize
                DeleteQuotationTerms(Index)
        End Select
    End Sub
    Private Sub dgChargeList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgChargeList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim Index As Integer = CInt(e.CommandArgument) + dgChargeList.PageIndex * dgChargeList.PageSize
                Session("EditCharge") = True
                setObject()
                setVendorDetails()
                mQuotation.QuotationCharges.CurrentIndex = Index
                Session("mQuotation") = mQuotation
                Response.Redirect("wfQuotationCharge_Ajax.aspx?BackPage=wfQuotation_Ajax.aspx")
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) + dgChargeList.PageIndex * dgChargeList.PageSize
                DeleteChargeRecord(Index)
        End Select
    End Sub
    Private Sub cmbVendorList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVendorList.SelectedIndexChanged
        Dim mVendorApprovalListForDue As VendorApprovalListForDue
        mVendorApprovalListForDue = VendorApprovalListForDue.GetVendorApprovalListForDue(mQuotation.Date.ToString, cmbVendorList.SelectedValue.ToString)
        For g As Integer = 0 To mVendorApprovalListForDue.Count - 1
            If (mVendorApprovalListForDue(g).RemainingDays < 0) Then
                Dim str As String = mVendorApprovalListForDue(g).Name
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Approval Document(s) " & str & " of selected vendor not valid for the Quotation Date.", MsgBoxStyle.OkOnly, "vendornotvalid")
                mVendorApprovalListForDue = Nothing
                Exit Sub
            End If
        Next
        setVendorDetails()
        'mPriorityList = PriorityList.GetPriorityList(, , "")
        'Session("mPriorityList") = mPriorityList
        Dim mQuotationItem As QuotationItem
        Dim i As Integer = 0
        For Each mQuotationItem In mQuotation.QuotationItems
            With mQuotationItem
                'GST Changes
                'mVendor = mVendorList(New Guid(cmbVendorList.SelectedValue)).StateName
                If AppSettings("IsGSTApplicable") = "True" And Not mQuotation.VendorID.Equals(Guid.Empty) Then
                    If mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName.ToUpper = "INDIA" And CDate(mQuotation.DateFormatted.ToString) >= CDate("01-Jul-2017") And mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientCountryName.ToUpper.Equals("INDIA") Then
                        mGSTPercentage = GSTPercentage.GetPercentage(mQuotation.DateFormatted.ToString, 1, .ItemID.ToString)
                        If Not mGSTPercentage Is Nothing Then
                            Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                            If Len(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode) > 0 Then
                                If mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode = mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode Then
                                    .CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                    .SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                    .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                    .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
                                    .IGSTPercentage = 0
                                    .IGSTCAmount = 0
                                    .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
                                    SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 1)
                                Else
                                    .IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                    .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
                                    .CGSTPercentage = 0
                                    .SGSTPercentage = 0
                                    .CGSTCAmount = 0
                                    .SGSTCAmount = 0
                                    .TotalCAmount = .CAmount + .IGSTCAmount
                                    SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 2)
                                End If
                                .HSNACSCode = mtmpItem.HSNACSCode
                            Else
                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .HSNACSCode = ""
                                SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                            End If
                        End If
                    Else
                        .CGSTPercentage = 0
                        .SGSTPercentage = 0
                        .CGSTCAmount = 0
                        .SGSTCAmount = 0
                        .IGSTPercentage = 0
                        .IGSTCAmount = 0
                        .HSNACSCode = ""
                        SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                    End If
                Else
                    .CGSTPercentage = 0
                    .SGSTPercentage = 0
                    .CGSTCAmount = 0
                    .SGSTCAmount = 0
                    .IGSTPercentage = 0
                    .IGSTCAmount = 0
                    .HSNACSCode = ""
                    mQuotation.Visibility = 3
                End If
                'End
            End With
            i = i + 1
        Next
        txtAddress.Text = mVendorList(cmbVendorList.SelectedIndex).Address
        cmbVendorList.Enabled = ((CType(IIf(mQuotation.StatusID >= 2, False, True), Boolean)) And mQuotation.VendorID.Equals(Guid.Empty)) Or mQuotation.QuotationItems.Count = 0
        If cmbVendorList.Enabled = True Then
            setFocus(cmbVendorList)
        End If
        ControlVisibility()
        dgQuotationItems.DataSource = mQuotation.QuotationItems
        dgQuotationItems.DataBind()
        ControlVisibility()
        upnlQuotationItems.Update()
        upnlValidationsummary.Update()
    End Sub
    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        setVendorDetails()
        If cmbCurrencyList.Enabled = True Then
            setFocus(cmbCurrencyList)
        End If
        upnlValidationsummary.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If chkIsCustomer.Checked = True And cmbCustomer.SelectedIndex <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Select Customer From List", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If chkIsCustomer.Checked = True Then
            cmbCustomer.Enabled = True
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim QuotationDetail As String
        If cmbVendorList.SelectedIndex = 0 Then
            QuotationDetail = mQuotation.QuotationNo + " Dated : " + mQuotation.DateFormatted.ToString
        Else
            QuotationDetail = mQuotation.QuotationNo + " Dated : " + mQuotation.DateFormatted.ToString + " from " + mVendorList(mQuotation.VendorID).Name
        End If
        MarkLog(Util.Action.Close, mModuleName, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID)
        'Session("IsValid") = IsValid
        setObject()
        setVendorDetails()
        If mQuotation.IsDirty Then
            'If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
            'End If
        Else
            Session.Remove("ItemsCount") 'Added by Prashant 30-Jul-2020 As once a Quo against Req item done then for new quo against req date was remaine disabled so removed session
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        'GST Changes
        Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        If CDate(calQuotationDate.Text) < CDate("01-Jul-2017") Or mQuotation.Visibility = 3 Then
            rpt = New crptQuotationDetailPortrait
        Else
            rpt = New crptQuotationDetailPortraitGST
        End If
        'End

        Dim obj As rptQuotation
        Dim objChilds As rptQuotationChilds
        Dim letter As rptLetterHead
        Dim ds As New dsQuotation
        obj = rptQuotation.GetQuotation(mQuotation.ID)
        objChilds = rptQuotationChilds.GetQuotationChilds(mQuotation.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", AppSettings("Logo"))
        Dim mrptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    'Added on 25-Jul-2016
    Protected Sub btnPerformInvoice_Click(sender As System.Object, e As EventArgs) Handles btnPerformInvoice.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As New crptPerformaInvoice 'crptQuotation
        Dim obj As rptQuotation
        Dim objChilds As rptQuotationChilds
        Dim letter As rptLetterHead
        Dim ds As New dsQuotation
        obj = rptQuotation.GetQuotation(mQuotation.ID)
        objChilds = rptQuotationChilds.GetQuotationChilds(mQuotation.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", AppSettings("Logo"))
        Dim mrptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub chkIsCustomer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsCustomer.CheckedChanged '=============================By Saylee 17/07/07============================
        If chkIsCustomer.Checked = True Then
            cmbCustomer.Enabled = True
        Else
            cmbCustomer.Enabled = False
            cmbCustomer.SelectedIndex = 0
        End If
        If chkIsCustomer.Enabled = True Then
            setFocus(chkIsCustomer)
        End If
    End Sub
    Private Sub calQuotationDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calQuotationDate.TextChanged
        If Not (New SmartDate(mQuotation.Date.ToString, True).Text = New SmartDate(CType(Trim(calQuotationDate.Text), Object).ToString, True).Text) Then
            ''============================WO - 2006-2007-1-24
            '' mQuotation.Date = CType(Trim(txtQuotationDate.Text), Object)'' Commented by Rajnish on 30-01-2008
            If calQuotationDate.Text = "" Then
                mQuotation.Date = System.DBNull.Value
            Else
                mQuotation.Date = CDate(calQuotationDate.Text)
            End If
            txtText.Text = mQuotation.Text                    '==============================
            txtValidDays.Text = mQuotation.ValidDays.ToString 'Kalpesh: ----------
            If txtValidUpToDate.Text = "" Then
                mQuotation.ValidDate = System.DBNull.Value
            Else
                mQuotation.ValidDate = mQuotation.ValidDateFormatted.ToString
            End If
            txtValidUpToDate.Text = mQuotation.ValidDateFormatted.ToString
            If mQuotation.TransTypeID = 2 Then
                cmbCustomer.Visible = False
            End If
        End If
        Session("mQuotation") = mQuotation
        upnlQuotationDetails.Update()
    End Sub
    'Kalpesh: ----------
    Private Sub txtValidUpToDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtValidUpToDate.TextChanged
        If (Not New SmartDate(mQuotation.ValidDate.ToString, True).Text = New SmartDate(CType(Trim(txtValidUpToDate.Text), Object).ToString, True).Text) And (Not (calQuotationDate.Text = "")) Then
            If txtValidUpToDate.Text = "" Then
                mQuotation.ValidDate = System.DBNull.Value
            Else
                mQuotation.ValidDate = CDate(txtValidUpToDate.Text)
            End If
        End If
        txtValidDays.Text = mQuotation.ValidDays.ToString
        calQuotationDate.Text = mQuotation.DateFormatted.ToString
    End Sub
    'Kalpesh: ----------
    Private Sub txtValidDays_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtValidDays.TextChanged
        If Not (calQuotationDate.Text = "") Then
            mQuotation.ValidDays = CInt(Val(txtValidDays.Text))
            If IsDBNull(mQuotation.ValidDate) = True Then
                txtValidUpToDate.Text = ""
            Else
                txtValidUpToDate.Text = mQuotation.ValidDateFormatted.ToString
            End If
        End If
    End Sub
    Private Sub txtCustQuoteDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtCustQuoteDate.TextChanged
        If Not IsDate(txtCustQuoteDate.Text) Then
            txtCustQuoteDateWatermarkExtender.WatermarkText = AppSettings("DateFormat")
        End If
    End Sub
    Private Sub chkIsRoundOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsRoundOff.CheckedChanged
        Dim Child As QuotationCharge
        For i As Integer = mQuotation.QuotationCharges.Count - 1 To 0 Step -1
            Child = mQuotation.QuotationCharges(i)
            If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
                mQuotation.QuotationCharges.Remove(Child)
            End If
        Next
        mQuotation.IsRoundOff = chkIsRoundOff.Checked
        dgChargeList.DataSource = mQuotation.QuotationCharges
        dgChargeList.DataBind()
    End Sub
    Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlAttachFile.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim mID As Guid
        mID = mQuotation.ID
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mQuotation.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mQuotation.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mQuotation.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mQuotation.ImageFile, 0, mQuotation.ImageFile.Length)
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
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mQuotation.ImageFile = file1
        mQuotation.Size = 0
        mQuotation.Extension = ""
        Session("mQuotation") = mQuotation
        ControlVisibilityForFileAttachment()
    End Sub
    Private Sub hdnimgBtnCommonPartList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCommonPartList.Click
        'DataFieldBind()
        'setObject()
        If CType(Session("AddParts"), String) = "True" Then
            AddMultipleParts()
            Session("AddParts") = "False"
        Else
            Session("AddParts") = "False"
        End If
        setVendorDetails()
        SetTransactionDate()
        mPriorityList = PriorityList.GetPriorityList(, , "")
        Session("mPriorityList") = mPriorityList
        dgQuotationItems.DataSource = mQuotation.QuotationItems
        dgQuotationItems.DataBind()
        calQuotationDate.DataBind()
        ControlVisibility()
        upnlQuotationDetails.Update()
        upnlQuotationItems.Update()
        upnlSupplierDetails.Update()
    End Sub
    Private Sub hdnimgBtnReqPartList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnReqPartList.Click
        setVendorDetails()
        mPriorityList = PriorityList.GetPriorityList(, , "")
        Session("mPriorityList") = mPriorityList
        dgQuotationItems.DataSource = mQuotation.QuotationItems
        dgQuotationItems.DataBind()
        ControlVisibilityForGrid()
        SetTransactionDate()
        ControlVisibility()
        upnlQuotationDetails.Update()
        upnlQuotationItems.Update()
        upnlSupplierDetails.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'Added By Vikrant On 22-Nov-2019 For ALL22112019
    Private Sub btnAmend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAmend.Click
        If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAmended, MSGBox.Message_text.StatusAmended, "<Strong> Quotation </Strong>", MsgBoxStyle.YesNo, "AmendStatus")
            Session("IsValid") = IsValid
            Session("Amend") = "Yes"
            mQuotation.StatusID = 3
            Session("mQuotation") = mQuotation
        End If
    End Sub
    'End
    Private Sub hdnimgBtnQuotationTerm_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnQuotationTerm.Click
        dgQuotationTerms.DataSource = mQuotation.QuotationTerms
        dgQuotationTerms.DataBind()
        upnlQuotationTerms.Update()
    End Sub
    Private Sub hdnBtnQuotationCharge_Click(sender As Object, e As System.EventArgs) Handles hdnBtnQuotationCharge.Click
        dgChargeList.DataSource = mQuotation.QuotationCharges
        dgChargeList.DataBind()
        mQuotation.CalculateTotal()
        SetChargeGrid()
        upnlQuotationCharges.Update()
        upnlOtherDetails.DataBind()
        upnlOtherDetails.Update()
    End Sub
#End Region

#Region " Status "
    ''====================================WO - 2006-2007-1-19
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If IsValid And CustomValidateForRateBeforeAuthorizeQuotation() = True Then
            setVendorDetails()
            If mVendorList(mQuotation.VendorID).NotInUse = True Then  'Added by Saylee on 24-Jul-2012
                If CDate(mVendorList(mQuotation.VendorID).NotInUseDate) <= CDate(mQuotation.Date) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Record can not be saved. <br><br> Supplier is not applicable since " + mVendorList(mQuotation.VendorID).NotInUseDateFormatted.ToString + " <br><br> Select another Supplier from list or select date before " + mVendorList(mQuotation.VendorID).NotInUseDateFormatted.ToString + " & try again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
            If mQuotation.IsCustomer = True Then
                If mVendorList(mQuotation.CustomerID).NotInUse = True Then
                    If CDate(mVendorList(mQuotation.CustomerID).NotInUseDate) <= CDate(mQuotation.Date) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Record can not be saved. <br><br> Customer is not applicable since " + mVendorList(mQuotation.CustomerID).NotInUseDateFormatted.ToString + " <br><br> Select another Customer from list or select date before " + mVendorList(mQuotation.CustomerID).NotInUseDateFormatted.ToString + " & try again", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> Quotation </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("mQuotation") = mQuotation
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click ''===============================WO - 2006-2007-1-19
        If IsValid Then
            Dim IsInUse As IsInUse = IsInUse.GetIsInUseQuotationINSalesOrder(mQuotation.ID)
            If IsInUse.IsInUse Then
                MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, "<Strong> Quotation, It is used in Sales Order .</Strong>", MsgBoxStyle.OkOnly, "StatusCancel")
                Session("mQuotation") = mQuotation
                Exit Sub
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Quotation </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
            Session("mQuotation") = mQuotation
        End If
    End Sub
#End Region

#Region " Add Multiple Parts "
    Private Sub AddMultipleParts()
        Dim mItem As Item
        Dim mItems As Items = Session("mItems")
        For Each mItem In mItems
            If mItem.IsSelected Then
                If Not mQuotation.QuotationItems.Contains(mItem.ID) Then
                    mQuotation.QuotationItems.Add(mQuotation.ID, mQuotation.ValidDays)
                    With mQuotation.QuotationItems.CurrentItem
                        .ItemID = mItem.ID
                        .Qty = 0 'frm.Qty
                        .CRate = mItem.Rate 'Add this line in ASP
                        .ModelName = ""
                        .IPCReference = mItem.IPCReference
                        'Added By Vikrant On 22-Nov-2019 For ALL22112019
                        .UnitID = mItem.UnitID
                        .UnitName = mItem.UnitName
                        .IsSerializedPart = mItem.SerialisedStatus
                        'End
                        'GST Changes
                        'mVendor = mVendorList(New Guid(cmbVendorList.SelectedValue)).StateName
                        If AppSettings("IsGSTApplicable") = "True" And Not mQuotation.VendorID.Equals(Guid.Empty) Then
                            If mVendorList(mQuotation.VendorID).CountryName.ToUpper = "INDIA" And CDate(mQuotation.DateFormatted.ToString) >= CDate("01-Jul-2017") And mVendorList(mQuotation.VendorID).ClientCountryName.ToUpper.Equals("INDIA") Then
                                mGSTPercentage = GSTPercentage.GetPercentage(mQuotation.DateFormatted.ToString, 1, .ItemID.ToString)
                                If Not mGSTPercentage Is Nothing Then
                                    Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                                    If Len(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode) > 0 Then
                                        If mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode = mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode Then
                                            .CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                            .SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                            .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                            .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
                                            .IGSTPercentage = 0
                                            .IGSTCAmount = 0
                                            .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
                                            SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 1)
                                        Else
                                            .IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                            .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
                                            .CGSTPercentage = 0
                                            .SGSTPercentage = 0
                                            .CGSTCAmount = 0
                                            .SGSTCAmount = 0
                                            .TotalCAmount = .CAmount + .IGSTCAmount
                                            SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 2)
                                        End If
                                        .HSNACSCode = mtmpItem.HSNACSCode
                                    Else
                                        .CGSTPercentage = 0
                                        .SGSTPercentage = 0
                                        .CGSTCAmount = 0
                                        .SGSTCAmount = 0
                                        .IGSTPercentage = 0
                                        .IGSTCAmount = 0
                                        .HSNACSCode = ""
                                        SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                                    End If

                                End If
                            Else
                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .HSNACSCode = ""
                                SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                            End If
                        Else
                            .CGSTPercentage = 0
                            .SGSTPercentage = 0
                            .CGSTCAmount = 0
                            .SGSTCAmount = 0
                            .IGSTPercentage = 0
                            .IGSTCAmount = 0
                            .HSNACSCode = ""
                            mQuotation.Visibility = 3
                        End If
                        'End
                    End With
                End If
            End If
        Next
        Session("mQuotation") = mQuotation
        Session("ItemsCount") = mQuotation.QuotationItems.Count
        Session.Remove("mItems")
    End Sub
    Private Sub AddEnquiryParts()
        Dim mEnquiry As Enquiry
        Dim mEnquiryItem As EnquiryItem

        mEnquiry = Session("mEnquiry")

        If Not mEnquiry Is Nothing Then

            For Each mEnquiryItem In mEnquiry.EnquiryItems
                If mEnquiryItem.IsSelect Then
                    If Not mQuotation.QuotationItems.Contains(mEnquiryItem.ItemID) Then

                        mQuotation.QuotationItems.Add(mQuotation.ID, mQuotation.ValidDays)
                        With mQuotation.QuotationItems.CurrentItem

                            mQuotation.QuotationItems.CurrentItem.ItemID = mEnquiryItem.ItemID
                            mQuotation.QuotationItems.CurrentItem.Qty = mEnquiryItem.Qty  'frm.Qty
                            mQuotation.QuotationItems.CurrentItem.EnquiryItemID = mEnquiryItem.ID  ' frm.FromItemID
                            mQuotation.QuotationItems.CurrentItem.EnquiryNo = mEnquiry.EnquiryNo    'frm.FromNo
                            mQuotation.QuotationItems.CurrentItem.EnquiryDate = mEnquiry.Date    'frm.FromDate
                            'mQuotation.QuotationItems.CurrentItem.ItemFrom = FromQuotation.PreviousTrans.Enquiry
                            mQuotation.QuotationItems.CurrentItem.CRate = mQuotation.QuotationItems.CurrentItem.CRate  'Add this line in ASP
                            mQuotation.QuotationItems.CurrentItem.ModelName = ""

                            mQuotation.QuotationItems.CurrentItem.IPCReference = mEnquiryItem.IPCReference
                            mQuotation.QuotationItems.CurrentItem.PriorityID = mEnquiryItem.PriorityID
                            mQuotation.QuotationItems.CurrentItem.DeliveryInDays = mEnquiryItem.RequiredInDays
                            Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                            'Added By Vikrant On 22-Nov-2019 For ALL22112019
                            If mEnquiryItem.ReqItemUnitID.Equals(Guid.Empty) Then
                                mQuotation.QuotationItems.CurrentItem.UnitID = mtmpItem.UnitID
                                mQuotation.QuotationItems.CurrentItem.UnitName = mtmpItem.UnitName
                            Else
                                mQuotation.QuotationItems.CurrentItem.UnitID = mEnquiryItem.ReqItemUnitID
                                mQuotation.QuotationItems.CurrentItem.UnitName = mEnquiryItem.ReqItemUnitName
                            End If
                            mQuotation.QuotationItems.CurrentItem.IsSerializedPart = mtmpItem.SerialisedStatus
                            'End
                            '.ItemID = mEnquiryItem.ItemID
                            'mQuotation.QuotationItems.CurrentItem.RequisitionItemID = mEnquiryItem.RequisitionItemID

                            'GST Changes
                            If AppSettings("IsGSTApplicable") = "True" And Not mQuotation.VendorID.Equals(Guid.Empty) Then
                                mVendor = Vendor.GetVendor(mQuotation.VendorID)
                                If mVendor.CountryName.ToUpper = "INDIA" And CDate(mQuotation.DateFormatted.ToString) >= CDate("01-Jul-2017") And mVendor.ClientCountryName.ToUpper.Equals("INDIA") Then
                                    mGSTPercentage = GSTPercentage.GetPercentage(mQuotation.DateFormatted.ToString, 1, .ItemID.ToString)
                                    If Not mGSTPercentage Is Nothing Then

                                        If Len(mVendor.StateCode) > 0 Then
                                            If mVendor.StateCode = mVendor.ClientStateCode Then
                                                .CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                                .SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                                .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                                .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
                                                .IGSTPercentage = 0
                                                .IGSTCAmount = 0
                                                .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

                                                SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 1)
                                            Else
                                                .IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                                .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
                                                .CGSTPercentage = 0
                                                .SGSTPercentage = 0
                                                .CGSTCAmount = 0
                                                .SGSTCAmount = 0
                                                .TotalCAmount = .CAmount + .IGSTCAmount
                                                SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 2)
                                            End If
                                            .HSNACSCode = mtmpItem.HSNACSCode
                                        Else
                                            .CGSTPercentage = 0
                                            .SGSTPercentage = 0
                                            .CGSTCAmount = 0
                                            .SGSTCAmount = 0
                                            .IGSTPercentage = 0
                                            .IGSTCAmount = 0
                                            .HSNACSCode = ""
                                            SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 3)
                                        End If

                                    End If
                                Else
                                    .CGSTPercentage = 0
                                    .SGSTPercentage = 0
                                    .CGSTCAmount = 0
                                    .SGSTCAmount = 0
                                    .IGSTPercentage = 0
                                    .IGSTCAmount = 0
                                    .HSNACSCode = ""
                                    SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 3)
                                End If
                            Else
                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .HSNACSCode = ""
                                mQuotation.Visibility = 3
                            End If
                            'End

                            Dim mEnquiryItemRequisitionItem As EnquiryItemRequisitionItem
                            For Each mEnquiryItemRequisitionItem In mEnquiryItem.EnquiryItemRequisitionItems
                                'Check if Requisition Part is present ?
                                If Not .QuotationItemRequisitionItems.Contains(mEnquiryItemRequisitionItem.RequisitionItemID) Then
                                    'if NOT then add
                                    .QuotationItemRequisitionItems.Add(.ID, mEnquiryItemRequisitionItem.RequisitionItemID, mEnquiryItemRequisitionItem.Qty, mEnquiryItemRequisitionItem.RequisitionNo, mQuotation.ValidDays)
                                    mQuotation.ApplyEdit()
                                Else
                                    'if YES fire Message
                                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for Quotation.", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                            Next

                        End With
                    End If
                End If
            Next
        End If
        Session("mQuotation") = mQuotation
        Session.Remove("mEnquiry")
    End Sub
    Private Sub AddRequisitionParts()
        If AppSettings("NewRequisition") = "True" Then 'Added by vikrant For New Requisition
            Dim mRequisitionItemNew As RequisitionItemNew
            Dim mRequisitionItemsNew As RequisitionItemsNew = Session("mRequisitionItemsNew")
            If mRequisitionItemsNew Is Nothing Then Exit Sub
            For Each mRequisitionItemNew In mRequisitionItemsNew
                If mRequisitionItemNew.IsSelect Then
                    If mQuotation.QuotationItems.Contains(mRequisitionItemNew.ItemID) Then
                        'With mQuotation.QuotationItems.Item(mRequisitionItemNew.ItemID, "")
                        '    'Check if Requisition Part is present ?
                        '    If Not .RequisitionItemQuotationItemsNew.Contains(mRequisitionItemNew.ID) Then
                        '        'if NOT then add
                        '        .RequisitionItemQuotationItemsNew.Add(.ID, mRequisitionItemNew.ID, mRequisitionItemNew.PurchaseQty, mRequisitionItemNew.RequisitionNo, mQuotation.ValidDays)
                        '    Else
                        'if YES fire Message
                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Quotation,Part " + mRequisitionItemNew.PartNo + " already taken for Quotation", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        'If NOT
                        mQuotation.BeginEdit()

                        mQuotation.QuotationItems.Add(mQuotation.ID, mQuotation.ValidDays)
                        With mQuotation.QuotationItems.CurrentItem
                            .ItemID = mRequisitionItemNew.ItemID
                            .IPCReference = mRequisitionItemNew.IPCReference
                            .PriorityID = mRequisitionItemNew.PriorityID
                            .RequisitionNo = mRequisitionItemNew.RequisitionNo

                            .ReqItemUnitID = mRequisitionItemNew.UnitID
                            .ReqItemUnitName = mRequisitionItemNew.Unit
                            .ReqItemID = mRequisitionItemNew.ID
                            Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                            'Added By Vikrant On 22-Nov-2019 For ALL22112019
                            .UnitID = mRequisitionItemNew.UnitID
                            .UnitName = mRequisitionItemNew.Unit
                            .IsSerializedPart = mtmpItem.SerialisedStatus
                            'End
                            'GST Changes
                            'mVendor = mVendorList(New Guid(cmbVendorList.SelectedValue)).StateName
                            If AppSettings("IsGSTApplicable") = "True" And Not mQuotation.VendorID.Equals(Guid.Empty) Then
                                If mVendorList(mQuotation.VendorID).CountryName.ToUpper = "INDIA" And CDate(mQuotation.DateFormatted.ToString) >= CDate("01-Jul-2017") And mVendorList(mQuotation.VendorID).ClientCountryName.ToUpper.Equals("INDIA") Then
                                    mGSTPercentage = GSTPercentage.GetPercentage(mQuotation.DateFormatted.ToString, 1, .ItemID.ToString)
                                    If Not mGSTPercentage Is Nothing Then

                                        If Len(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode) > 0 Then
                                            If mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode = mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode Then
                                                .CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                                .SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
                                                .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                                .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
                                                .IGSTPercentage = 0
                                                .IGSTCAmount = 0
                                                .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

                                                SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 1)
                                            Else
                                                .IGSTPercentage = (mGSTPercentage.GSTPercentage)
                                                .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
                                                .CGSTPercentage = 0
                                                .SGSTPercentage = 0
                                                .CGSTCAmount = 0
                                                .SGSTCAmount = 0
                                                .TotalCAmount = .CAmount + .IGSTCAmount

                                                SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 2)
                                            End If
                                            .HSNACSCode = mtmpItem.HSNACSCode
                                        Else
                                            .CGSTPercentage = 0
                                            .SGSTPercentage = 0
                                            .CGSTCAmount = 0
                                            .SGSTCAmount = 0
                                            .IGSTPercentage = 0
                                            .IGSTCAmount = 0
                                            .HSNACSCode = ""
                                            SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                                        End If

                                    End If
                                Else
                                    .CGSTPercentage = 0
                                    .SGSTPercentage = 0
                                    .CGSTCAmount = 0
                                    .SGSTCAmount = 0
                                    .IGSTPercentage = 0
                                    .IGSTCAmount = 0
                                    .HSNACSCode = ""
                                    SetQuotationDetails(mVendorList(New Guid(cmbVendorList.SelectedValue)).StateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).ClientStateCode, mVendorList(New Guid(cmbVendorList.SelectedValue)).CountryName, 3)
                                End If
                            Else
                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .HSNACSCode = ""
                                mQuotation.Visibility = 3
                            End If
                            'End

                            'Check if Requisition Part is present ?
                            If Not .RequisitionItemQuotationItemsNew.Contains(mRequisitionItemNew.ID) Then
                                'if NOT then add
                                .RequisitionItemQuotationItemsNew.Add(.ID, mRequisitionItemNew.ID, mRequisitionItemNew.QuotationBalQty, mRequisitionItemNew.RequisitionNo, mQuotation.ValidDays)
                                mQuotation.ApplyEdit()
                            Else
                                'if YES fire Message
                                MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for Quotation.", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        End With
                    End If
                End If
            Next
        Else   'AppSettings("NewRequisition") = "False" Then ' 'End
            Dim mRequisitionItem As RequisitionItem
            Dim mRequisitionItems As RequisitionItems = Session("mRequisitionItems")
            If mRequisitionItems Is Nothing Then Exit Sub
            For Each mRequisitionItem In mRequisitionItems
                If mRequisitionItem.IsSelect Then
                    '' If Not mQuotation.QuotationItems.Contains(mRequisitionItem.ID, "") Then

                    'Check if Part is present ?

                    'If YES
                    If mQuotation.QuotationItems.Contains(mRequisitionItem.ItemID) Then
                        With mQuotation.QuotationItems.Item(mRequisitionItem.ItemID, "")

                            'Check if Requisition Part is present ?
                            If Not .QuotationItemRequisitionItems.Contains(mRequisitionItem.ID) Then
                                'if NOT then add
                                .QuotationItemRequisitionItems.Add(.ID, mRequisitionItem.ID, mRequisitionItem.PurchaseQty, mRequisitionItem.RequisitionNo, mQuotation.ValidDays)
                            Else
                                'if YES fire Message
                                MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for Quotation.", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        End With
                    Else
                        'If NOT
                        mQuotation.BeginEdit()

                        mQuotation.QuotationItems.Add(mQuotation.ID, mQuotation.ValidDays)
                        With mQuotation.QuotationItems.CurrentItem
                            .ItemID = mRequisitionItem.ItemID
                            .IPCReference = mRequisitionItem.IPCReference '    Added new Rajnish - 3-11-2007
                            .PriorityID = mRequisitionItem.PriorityID      '    Added new Rajnish - 3-11-2007
                            '.RequisitionItemID = mRequisitionItem.ID
                            '.Qty = mRequisitionItem.PurchaseQty
                            'Check if Requisition Part is present ?
                            If Not .QuotationItemRequisitionItems.Contains(mRequisitionItem.ID) Then
                                'if NOT then add
                                .QuotationItemRequisitionItems.Add(.ID, mRequisitionItem.ID, mRequisitionItem.PurchaseQty, mRequisitionItem.RequisitionNo, mQuotation.ValidDays)
                                mQuotation.ApplyEdit()
                            Else
                                'if YES fire Message
                                MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for Quotation.", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        End With
                    End If
                End If
            Next
        End If
        Session("mQuotation") = mQuotation
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mQuotation.IsValid = False Then
            For i As Integer = 0 To mQuotation.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mQuotation.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mQuotationItem As QuotationItem
        If mQuotation.QuotationItems.IsValid = False Then
            For Each mQuotationItem In mQuotation.QuotationItems
                For i As Integer = 0 To mQuotationItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mQuotationItem.ItemName + " : " + mQuotationItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            cvCustomer.ErrorMessage = strMsg
            cvCustomer.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub

        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()

        If mQuotation.IsValid = False Then
            For i As Integer = 0 To mQuotation.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mQuotation.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        Dim mQuotationItem As QuotationItem
        If mQuotation.QuotationItems.IsValid = False Then
            For Each mQuotationItem In mQuotation.QuotationItems
                For i As Integer = 0 To mQuotationItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mQuotationItem.ItemName + " : " + mQuotationItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
        Flag = 1
    End Sub

    Public Function CustomValidateForRateBeforeAuthorizeQuotation()
        Dim txtValue As TextBox
        Dim mQuotationItem As QuotationItem
        Dim strMsg As String = ""
        Dim i As Integer = 0
        For Each mQuotationItem In mQuotation.QuotationItems
            With mQuotationItem
                txtValue = CType(Me.dgQuotationItems.Rows(i).FindControl("txtRate"), TextBox)
                If Val(txtValue.Text) = 0 Or txtValue.Text = "" Then
                    strMsg = mQuotationItem.ItemName + " : " + "Rate is required"
                    cvCustomer.ErrorMessage = strMsg
                    cvCustomer.IsValid = False
                    Return False
                    Exit Function
                End If
            End With
            i = i + 1
        Next
        Return True
    End Function
#End Region




End Class