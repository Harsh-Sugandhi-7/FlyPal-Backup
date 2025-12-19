Public Class wfQuotationItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mQuotation As Quotation
    Public mModelList As ModelList
    Public mPriorityList As PriorityList
    Public mQuotationItemRequisitionItems As QuotationItemRequisitionItems
    Dim mItemTypeQuoList As ItemTypeList
    Public mRequisitionItemQuotationItemsNew As RequisitionItemQuotationItemsNew 'Added by vikrant For New Requisition
    'GST Changes
    Public mGSTPercentage As GSTPercentage
    Public mVendor As Vendor
    'End
    Public mUnitConverterList As UnitConverterList 'Added By Vikrant On 22-Nov-2019 For ALL22112019
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mQuotation = Session("mQuotation")
        mModelList = Session("mModelList")
        mPriorityList = Session("mPriorityList")
        mItemTypeQuoList = Session("mItemTypeQuoList")
        mUnitConverterList = Session("mUnitConverterList") 'Added By Vikrant On 22-Nov-2019 For ALL22112019
    End Sub
    Private Sub setSession()
        Session("mQuotation") = mQuotation
        Session("mModelList") = mModelList
        Session("mPriorityList") = mPriorityList
        Session("mItemTypeQuoList") = mItemTypeQuoList
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
        txtRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRate').value,event)")
        txtOtherCharges.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOtherCharges').value,event)")
        txtEOQ.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEOQ').value,event)")
        txtEOQCRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEOQCRate').value,event)")
        txtCBillBackRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCBillBackRate').value,event)")
        txtDeliveryInDays.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtDeliveryInDays').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Quotation Item [" & mQuotation.QuotationItems.CurrentItem.ItemName & "]"
            txtPartNo.BackColor = Color.Silver
            If AppSettings("NewRequisition") <> "True" Then  'Changed by vikrant For New Requisition
                dgRequisitionItemList.Columns(5).Visible = (mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems.Count > 0) And CType(mQuotation.TransTypeID, Trans) = Util.Trans.PurchaseQuotation
                dgRequisitionItemList.Columns(6).Visible = (mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems.Count > 0) And CType(mQuotation.TransTypeID, Trans) = Util.Trans.PurchaseQuotation
                dgRequisitionItemList.Columns(7).Visible = (mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems.Count > 0) And CType(mQuotation.TransTypeID, Trans) = Util.Trans.PurchaseQuotation
            End If
            txtPartNo.ToolTip = "Part No."
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
    Private Function setObject() As Boolean
        mQuotation.QuotationItems.CurrentItem.ItemTypeID = CInt(cmbPartTypeList.SelectedValue)
        mQuotation.QuotationItems.CurrentItem.SrNo = mQuotation.QuotationItems.CurrentIndex + 1
        mQuotation.QuotationItems.CurrentItem.Qty = Val(txtQty.Text)
        mQuotation.QuotationItems.CurrentItem.CRate = Val(txtRate.Text)
        mQuotation.QuotationItems.CurrentItem.COtherCharges = Val(txtOtherCharges.Text)
        mQuotation.QuotationItems.CurrentItem.ModelID = New Guid(cmbApplicable.SelectedValue)
        mQuotation.QuotationItems.CurrentItem.ModelName = cmbApplicable.SelectedItem.Text
        mQuotation.QuotationItems.CurrentItem.Remark = Trim(txtRemark.Text)
        mQuotation.QuotationItems.CurrentItem.Note = Trim(txtNote.Text)
        mQuotation.QuotationItems.CurrentItem.AltPartNo = txtAltPartNo.Text         '==================== By Saylee on 18/07/07 =============================
        mQuotation.QuotationItems.CurrentItem.EOQ = Val(txtEOQ.Text)
        mQuotation.QuotationItems.CurrentItem.EOQCRate = Val(txtEOQCRate.Text)
        mQuotation.QuotationItems.CurrentItem.DeliveryInDays = Val(txtDeliveryInDays.Text)
        mQuotation.QuotationItems.CurrentItem.IPCReference = txtIPCReference.Text
        mQuotation.QuotationItems.CurrentItem.CBillBackRate = Val(txtCBillBackRate.Text)
        mQuotation.QuotationItems.CurrentItem.PaymentTerm = txtPaymentTerms.Text    '========================================================================
        mQuotation.QuotationItems.CurrentItem.PriorityID = CInt(cmbPriority.SelectedValue)
        'Added By Vikrant On 22-Nov-2019 For ALL22112019
        mQuotation.QuotationItems.CurrentItem.UnitID = New Guid(cmbUnitConverterList.SelectedValue)
        mQuotation.QuotationItems.CurrentItem.UnitName = cmbUnitConverterList.SelectedItem.Text
        'End
        Dim mQuotationItemRequisitionItem As QuotationItemRequisitionItem
        Dim txtValue As TextBox
        Dim i As Integer = 0
        For Each mQuotationItemRequisitionItem In mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems
            With mQuotationItemRequisitionItem
                txtValue = CType(Me.dgRequisitionItemList.Rows(i).FindControl("txtReqQty"), TextBox)
                .Qty = CDec(Val(txtValue.Text))
            End With
            i = i + 1
        Next
        txtQty.DataBind()

        Dim mtmpItem As Item = Item.GetItem(mQuotation.QuotationItems.CurrentItem.ItemID)   'Added by Saylee on 24-Jul-2012

        If mQuotation.QuotationItems.Contains(mQuotation.QuotationItems.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Quotation Item", MsgBoxStyle.OkOnly, "")
            mQuotation.CancelEdit()
            Exit Function
        ElseIf mtmpItem.NotInUse = True Then 'Added by Saylee on 24-Jul-2012

            If CDate(mtmpItem.NotInUseDate) <= CDate(mQuotation.Date) Then
                Session("ItemNotInUse") = True
                MSGBoxCtrl.show("Save Alert!", "Part is not applicable since " + mtmpItem.NotInUseDateFormatted + " <br><br> Select another Part from list & try again", "", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            'Else
            '    mQuotation.ApplyEdit()
        End If
        'GST Changes
        'If AppSettings("IsGSTApplicable") = "True" Then
        '    mVendor = Vendor.GetVendor(mQuotation.VendorID)
        '    If mVendor.CountryName.ToUpper.Equals("INDIA") And CDate(mQuotation.DateFormatted.ToString) >= CDate("01-Jul-2017") And mVendor.ClientCountryName.ToUpper.Equals("INDIA") Then
        '        mGSTPercentage = GSTPercentage.GetPercentage(mQuotation.DateFormatted.ToString, 1, mQuotation.QuotationItems.CurrentItem.ItemID.ToString)
        '        If Not mGSTPercentage Is Nothing Then
        '            If Len(mVendor.StateCode) > 0 Then
        '                If mVendor.StateCode = mVendor.ClientStateCode Then
        '                    mQuotation.QuotationItems.CurrentItem.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
        '                    mQuotation.QuotationItems.CurrentItem.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
        '                    mQuotation.QuotationItems.CurrentItem.CGSTCAmount = ((mQuotation.QuotationItems.CurrentItem.CGSTPercentage * mQuotation.QuotationItems.CurrentItem.CAmount) / 100)
        '                    mQuotation.QuotationItems.CurrentItem.SGSTCAmount = ((mQuotation.QuotationItems.CurrentItem.SGSTPercentage * mQuotation.QuotationItems.CurrentItem.CAmount) / 100)

        '                    mQuotation.QuotationItems.CurrentItem.TotalCAmount = mQuotation.QuotationItems.CurrentItem.CAmount + mQuotation.QuotationItems.CurrentItem.CGSTCAmount + mQuotation.QuotationItems.CurrentItem.SGSTCAmount

        '                    SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 1)
        '                Else
        '                    mQuotation.QuotationItems.CurrentItem.IGSTPercentage = (mGSTPercentage.GSTPercentage)
        '                    mQuotation.QuotationItems.CurrentItem.IGSTCAmount = ((mQuotation.QuotationItems.CurrentItem.IGSTPercentage * mQuotation.QuotationItems.CurrentItem.CAmount) / 100)

        '                    mQuotation.QuotationItems.CurrentItem.TotalCAmount = mQuotation.QuotationItems.CurrentItem.CAmount + mQuotation.QuotationItems.CurrentItem.IGSTCAmount

        '                    SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 2)
        '                End If
        '            Else
        '                SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 3)
        '            End If
        '        End If
        '    Else
        '        SetQuotationDetails(mVendor.StateCode, mVendor.ClientStateCode, mVendor.CountryName, 3)
        '    End If
        'Else
        '    mQuotation.Visibility = 3
        'End If
        
        'End
        mQuotation.CalculateTotal()   'Added By Saylee on 10-Sep-2007
        If mQuotation.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
            mQuotation.RoundCGrandTotal()
        End If
        Return True
    End Function
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
                            mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems.Remove(mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems.CurrentItem.ID)
                            Session("mQuotation") = mQuotation
                            mQuotationItemRequisitionItems = mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems
                            dgRequisitionItemList.DataSource = mQuotationItemRequisitionItems
                            dgRequisitionItemList.DataBind()
                            upnlRequisitionItems.Update()
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("Sender") = ""
                    End If
            End Select
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "Delete")
        mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems.CurrentIndex = Index
        Session("mQuotation") = mQuotation
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModelList = ModelList.GetModelList(mQuotation.QuotationItems.CurrentItem.ItemID, True)
        Session("mModelList") = mModelList
        cmbApplicable.DataSource = mModelList
        mPriorityList = PriorityList.GetPriorityList(, , "")
        Session("mPriorityList") = mPriorityList
        cmbPriority.DataSource = mPriorityList
        mItemTypeQuoList = ItemTypeList.GetItemTypeList
        Session("mItemTypeQuoList") = mItemTypeQuoList
        cmbPartTypeList.DataSource = mItemTypeQuoList

        mQuotationItemRequisitionItems = mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems
        dgRequisitionItemList.DataSource = mQuotationItemRequisitionItems

        'Added By Vikrant On 22-Nov-2019 For ALL22112019
        mUnitConverterList = UnitConverterList.GetUnitConverterList(mQuotation.QuotationItems.CurrentItem.ItemID)
        cmbUnitConverterList.DataSource = mUnitConverterList
        Session("mUnitConverterList") = mUnitConverterList
        'End

        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            If mQuotation.ValidDays <= 0 And Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than Zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRate" Then
            If mQuotation.ValidDays <= 0 And Val(txtRate.Text) <= 0 Then
                custValidator.ErrorMessage = "Rate must be greater than zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtOtherCharges" Then
            If Val(txtOtherCharges.Text) < 0 Then
                custValidator.ErrorMessage = "Other Charge must be greater than zero."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub AddPart()
        'Added by vikrant For New Requisition
        If AppSettings("NewRequisition") = "True" Then
            Dim mRequisitionItemNew As RequisitionItemNew
            Dim mRequisitionItemsNew As RequisitionItemsNew = Session("mRequisitionItemsNew")
            If mRequisitionItemsNew Is Nothing Then Exit Sub
            For Each mRequisitionItemNew In mRequisitionItemsNew
                If mRequisitionItemNew.IsSelect Then
                    With mQuotation.QuotationItems.CurrentItem
                        .IPCReference = mRequisitionItemNew.IPCReference
                        .PriorityID = mRequisitionItemNew.PriorityID
                        'Check is Requisition Part is present ?
                        If Not .RequisitionItemQuotationItemsNew.Contains(mRequisitionItemNew.ID) Then ''
                            'if NOT then add
                            '.RequisitionItemQuotationItemsNew.Add(.ID, mRequisitionItemNew.ID, mRequisitionItemNew.PurchaseQty, mRequisitionItemNew.RequisitionNo, mQuotation.ValidDays) ''
                        Else
                            'if YES fire Message
                            MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for enquiry", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End With
                End If
            Next
        Else 'AppSettings("NewRequisition") = "False" 'End
            Dim mRequisitionItem As RequisitionItem
            Dim mRequisitionItems As RequisitionItems = Session("mRequisitionItems")
            If mRequisitionItems Is Nothing Then Exit Sub
            For Each mRequisitionItem In mRequisitionItems
                If mRequisitionItem.IsSelect Then
                    With mQuotation.QuotationItems.CurrentItem
                        .IPCReference = mRequisitionItem.IPCReference
                        .PriorityID = mRequisitionItem.PriorityID
                        'Check is Requisition Part is present ?
                        If Not .QuotationItemRequisitionItems.Contains(mRequisitionItem.ID) Then
                            'if NOT then add
                            .QuotationItemRequisitionItems.Add(.ID, mRequisitionItem.ID, mRequisitionItem.PurchaseQty, mRequisitionItem.RequisitionNo, mQuotation.ValidDays)
                        Else
                            'if YES fire Message
                             MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for enquiry", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End With
                End If
            Next
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        If CType(Session("AddPart"), String) = "True" Then
            'Add selected part(s) to Enquiry Items
            AddPart()
            Session("AddPart") = "False"
            Session("AddRequisitionParts") = "False"
        Else
            Session("AddPart") = "False"
            Session("AddRequisitionParts") = "False"
        End If
        If Not IsPostBack Then
            If txtPartNo.Enabled = True Then
                setFocus(txtPartNo)
            End If
            DataFieldBind()
        End If
        SetPage()
        ControlVisibility()
        If mQuotation.QuotationItems.CurrentItem.EnquiryItemID.Equals(Guid.Empty) Then 'Added By Rajnish On 03-01-2008
            pnlEnquiryItemInformation.Visible = False
        End If
    End Sub
    Private Sub ControlVisibility()
        'Added by vikrant For New Requisition
        If AppSettings("NewRequisition") = "True" Then
            If mQuotation.QuotationItems.CurrentItem.EnquiryItemID.Equals(Guid.Empty) And mQuotation.QuotationItems.CurrentItem.RequisitionItemQuotationItemsNew.Count = 0 Then ''
                cmbPriority.Enabled = True
            Else
                cmbPriority.Enabled = False
            End If
            btnAdd.Visible = False
            'Label2.Visible = False
            dgRequisitionItemList.Visible = False
        Else 'End
            If mQuotation.QuotationItems.CurrentItem.EnquiryItemID.Equals(Guid.Empty) And mQuotation.QuotationItems.CurrentItem.QuotationItemRequisitionItems.Count = 0 Then
                cmbPriority.Enabled = True
            Else
                cmbPriority.Enabled = False
            End If
            btnAdd.Visible = (Not mQuotation.QuotationItems.CurrentItem.ItemID.Equals(Guid.Empty)) And (CType(mQuotation.TransTypeID, Flypal.Util.Trans) = Flypal.Util.Trans.PurchaseQuotation Or CType(mQuotation.TransTypeID, Flypal.Util.Trans) = Flypal.Util.Trans.Quotation)
            'Label2.Visible = (Not mQuotation.QuotationItems.CurrentItem.ItemID.Equals(Guid.Empty)) And (CType(mQuotation.TransTypeID, Flypal.Util.Trans) = Flypal.Util.Trans.PurchaseQuotation Or CType(mQuotation.TransTypeID, Flypal.Util.Trans) = Flypal.Util.Trans.Quotation)
            dgRequisitionItemList.Visible = (Not mQuotation.QuotationItems.CurrentItem.ItemID.Equals(Guid.Empty)) And (CType(mQuotation.TransTypeID, Flypal.Util.Trans) = Flypal.Util.Trans.PurchaseQuotation Or CType(mQuotation.TransTypeID, Flypal.Util.Trans) = Flypal.Util.Trans.Quotation)
        End If
        'Added By Vikrant On 22-Nov-2019 For ALL22112019
        If mQuotation.TransTypeID = 33 And mUnitConverterList.Count > 1 And mQuotation.QuotationItems.CurrentItem.IsSerializedPart = False Then 'mOrder.AgainstTypeID = 1 New purchase against item i.e none
            cmbUnitConverterList.Enabled = True
        Else
            cmbUnitConverterList.Enabled = False
        End If
        'End
    End Sub
    Private Sub imgbtnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnPartNo.Click
        setObject()
        mQuotation.QuotationItems.CurrentItem.ModelID = Guid.Empty  '--------14-12-2006
        Session("mQuotation") = mQuotation
        Session("PartNo") = txtPartNo.Text
        Response.Redirect("wfQuotationPartStockStatus_Ajax.aspx?BackPage=wfQuotation_Ajax.aspx&ChildPage=wfQuotationItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(txtPartNo.Text))
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If setObject() Then
                Session("mQuotation") = mQuotation
                Session.Remove("mModelList")
                Session.Remove("Edit")
                Session.Remove("mUnitConverterList") 'Added By Vikrant On 22-Nov-2019 For ALL22112019
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim mtmpItem As Item = Item.GetItem(mQuotation.QuotationItems.CurrentItem.ItemID) 'Added by Saylee on 24-Jul-2012
        If mtmpItem.NotInUse = True Then 'Added by Saylee on 24-Jul-2012
            If CDate(mtmpItem.NotInUseDate) < CDate(mQuotation.Date) Then
                mQuotation.QuotationItems.CurrentItem.AlternateItemID = Guid.Empty
                mQuotation.QuotationItems.CurrentItem.ItemID = CType(Session("PrevItemID"), Guid)
                Session("mQuotation") = mQuotation
            End If
        End If

        If mQuotation.QuotationItems.CurrentItem.IsNew And Not Session("Edit") = True Then mQuotation.QuotationItems.Remove(mQuotation.QuotationItems.CurrentItem)

        Session.Remove("Edit")
        Session.Remove("mModelList")
        Session.Remove("mUnitConverterList") 'Added By Vikrant On 22-Nov-2019 For ALL22112019
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub dgRequisitionItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionItemList.RowCommand
        Dim Index As Integer = CInt(e.CommandArgument) + dgRequisitionItemList.PageIndex * dgRequisitionItemList.PageSize
        Select Case e.CommandName
            Case "ForDelete"
                DeleteRecord(Index)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        setObject()
        Session("mQuotation") = mQuotation
        Session("PartNo") = txtPartNo.Text
        Session("mPriorityList") = mPriorityList
        If Not mQuotation.QuotationItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
            Session("StoreApprovalList") = "True"
            Session("TransDate") = mQuotation.Date.ToString
            Session("QuotationItem") = mQuotation.QuotationItems.CurrentItem.ItemID
            Session("ListFor") = 1
            Response.Redirect("wfStoreApprovalList.aspx?BackPage=wfQuotation_Ajax.aspx&ChildPage=wfQuotationItem_Ajax.aspx")
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

#Region "Alternate Part List"
    Private Sub btnAlternatePart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlternatePart.Click 'Added By Kalpesh on 02-Jun-2008
        setObject()
        txtAlternatePartNo.Text = Item.GetItem(mQuotation.QuotationItems.CurrentItem.ItemID).Name
        txtAlternateDescription.Text = Item.GetItem(mQuotation.QuotationItems.CurrentItem.ItemID).Description
        lblResult.Text = "List of alternate parts For : " + txtAlternatePartNo.Text
        dgAlternatePartList.DataSource = Item.GetItem(mQuotation.QuotationItems.CurrentItem.ItemID).AlternatePartNos
        dgAlternatePartList.DataBind()
        upnlAlternatePartList.Update()
        mdeAlternatePartList.Show()
    End Sub
    Private Sub dgAlternatePartList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAlternatePartList.RowCommand
        Select Case e.CommandName
            Case "SelectPart"
                Dim index As Integer = CInt(e.CommandArgument) + dgAlternatePartList.PageIndex * dgAlternatePartList.PageSize
                mQuotation.QuotationItems.CurrentItem.AlternateItemID = Item.GetItem(mQuotation.QuotationItems.CurrentItem.ItemID).AlternatePartNos(index).AlternatePartID
                Session("mQuotation") = mQuotation
                txtPartNo.Text = mQuotation.QuotationItems.CurrentItem.ItemName
                txtDescription.Text = mQuotation.QuotationItems.CurrentItem.ItemDescription
                txtIPCReference.Text = mQuotation.QuotationItems.CurrentItem.IPCReference
                mModelList = ModelList.GetModelList(mQuotation.QuotationItems.CurrentItem.ItemID, True)
                Session("mModelList") = mModelList
                mQuotation.QuotationItems.CurrentItem.ModelID = Guid.Empty
                mQuotation.QuotationItems.CurrentItem.ModelName = ""
                cmbApplicable.DataSource = mModelList
                cmbApplicable.DataBind()
                upnlApplicableTo.Update()
                upnlQuotationItem.Update()
                upnlPartType.Update()
                mdeAlternatePartList.Hide()
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mdeAlternatePartList.Hide()
    End Sub
#End Region

End Class