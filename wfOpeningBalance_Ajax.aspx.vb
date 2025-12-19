Imports System.Collections.Generic
Imports System.Linq

Public Class wfOpeningBalance_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mCurrencyList As CurrencyList
    Public mStoreList As StoreList
    Public mFromStoreList As StoreList
    Public mVendorList As VendorList
    Public mMachineNameValueList As MachineNameValueList
    Public mWorkShopList As WorkShopList
    Public mPartTypeList As PartTypeList
    Public mTypeListReceipt As TypeListForReceipt
    Dim EventLogID As Guid
    Public mItemTypeList As PartTypeList
    Dim IssueCount As Integer = 0
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItem = Session("mItem")
        mCurrencyList = Session("mCurrencyList")
        mStoreList = Session("mStoreList")
        mFromStoreList = Session("mFromStoreList")
        mVendorList = Session("mVendorList")
        mMachineNameValueList = Session("mMachineNameValueList")
        mPartTypeList = Session("mPartTypeList")
        mTypeListReceipt = Session("mTypeListReceipt")
        mWorkShopList = Session("mWorkShopList")
        mItemTypeList = CType(Session("mItemTypeList"), PartTypeList)
    End Sub
    Private Sub SetSession()
        Session("mItem") = mItem
        Session("mCurrencyList") = mCurrencyList
        Session("mStoreList") = mStoreList
        Session("mFromStoreList") = mFromStoreList
        Session("mVendorList") = mVendorList
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mPartTypeList") = mPartTypeList
        Session("mTypeListReceipt") = mTypeListReceipt
        Session("mWorkShopList") = mWorkShopList
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
        txtLandingRates.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLandingRates').value,event)")
        txtLandingCost.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLandingCost').value,event)")
        txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
        txtOtherCharge.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOtherCharge').value,event)")

        txtCureQtrs.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCureQtrs').value,event)")
        txtCureYear.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCureYear').value,event)")
        txtExpQrts.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExpQrts').value,event)")
        txtExpYear.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExpYear').value,event)")
        txtCommercialRate.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCommercialRate').value,event)")
    End Sub
    Private Sub SetPage()
        lblTitle.Text = "Opening Stock For Part No [" & mItem.Name & "]"
        txtReceiptDate.Enabled = Not Session("EditItem")
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblBatchNo.Text = "RNN No."
        Else
            lblBatchNo.Text = "Batch No."
        End If
    End Sub
    Private Function setObject() As Boolean
        'mItem.BeginEdit()
        mItem.OpeningBalances.CurrentItem.InvoiceText = txtReceiptText.Text
        mItem.OpeningBalances.CurrentItem.InvoiceNo = Val(txtReceiptNo.Text)
        mItem.OpeningBalances.CurrentItem.COtherCharges = Val(txtOtherCharge.Text)
        mItem.OpeningBalances.CurrentItem.AsOnDate = mItem.AsOnDate
        mItem.OpeningBalances.CurrentItem.VendorInvoiceNo = txtInvoiceNo.Text
        If txtReceiptDate.Text.ToString <> "" Then
            mItem.OpeningBalances.CurrentItem.InvoiceDate = txtReceiptDate.Text
        Else
            mItem.OpeningBalances.CurrentItem.InvoiceDate = System.DBNull.Value
        End If
        If txtInvoiceDate.Text.ToString <> "" Then
            mItem.OpeningBalances.CurrentItem.VendorInvoiceDate = txtInvoiceDate.Text
        Else
            mItem.OpeningBalances.CurrentItem.VendorInvoiceDate = System.DBNull.Value
        End If
        mItem.OpeningBalances.CurrentItem.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mItem.OpeningBalances.CurrentItem.ConversionFactor = Val(txtConversionFactor.Text)
        mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.BaseUnitID = mItem.UnitID
        mItem.OpeningBalances.CurrentItem.DisplayQty = Val(txtQty.Text)
        mItem.OpeningBalances.CurrentItem.CRate = Val(txtLandingRates.Text)
        mItem.OpeningBalances.CurrentItem.LandingCost = Val(txtLandingCost.Text)
        mItem.OpeningBalances.CurrentItem.ReleaseNoteNo = txtReleaseNoteNo.Text.Trim
        If txtReleaseNoteDate.Text.ToString <> "" Then
            mItem.OpeningBalances.CurrentItem.ReleaseNoteDate = txtReleaseNoteDate.Text.ToString
        Else
            mItem.OpeningBalances.CurrentItem.ReleaseNoteDate = System.DBNull.Value
        End If
        If txtStartDate.Text.ToString <> "" Then
            mItem.OpeningBalances.CurrentItem.StartDate = txtStartDate.Text.ToString
        Else
            mItem.OpeningBalances.CurrentItem.StartDate = System.DBNull.Value
        End If
        If txtExpiryDate.Text.ToString <> "" Then
            mItem.OpeningBalances.CurrentItem.ExpiryDate = txtExpiryDate.Text.ToString
        Else
            mItem.OpeningBalances.CurrentItem.ExpiryDate = System.DBNull.Value
        End If
        If cmbReceivedFrom.SelectedItem.Text = "Supplier" Then
            mItem.OpeningBalances.CurrentItem.VendorID = New Guid(cmbVendor.SelectedValue)
        ElseIf cmbReceivedFrom.SelectedItem.Text = "Aircraft" Then
            mItem.OpeningBalances.CurrentItem.MachineID = New Guid(cmbAircraft.SelectedValue)
        ElseIf cmbReceivedFrom.SelectedItem.Text = "Store" Then
            mItem.OpeningBalances.CurrentItem.FromStoreID = New Guid(cmbFromStore.SelectedValue)
        ElseIf cmbReceivedFrom.SelectedItem.Text = "WorkShop" Then
            mItem.OpeningBalances.CurrentItem.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
        End If
        mItem.OpeningBalances.CurrentItem.ItemTypeID = Val(cmbPartType.SelectedValue)
        mItem.OpeningBalances.CurrentItem.Returnable = chkReturnable.Checked
        mItem.OpeningBalances.CurrentItem.TypeID = Val(cmbReceivedFrom.SelectedValue)
        mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreID = New Guid(cmbStoreList.SelectedValue)
        If cmbStoreList.SelectedIndex > 0 Then
            Dim info As New StoreList.StoreInfo
            info = mStoreList.Item(cmbStoreList.SelectedIndex)
            mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreName = info.Name
        Else
            mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreName = ""
        End If
        mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.SerialNo = Trim(txtSerialNo.Text)
        mItem.OpeningBalances.CurrentItem.Location = txtLocation.Text.Trim
        mItem.OpeningBalances.CurrentItem.Remark = txtRemark.Text.Trim
        mItem.OpeningBalances.CurrentItem.Note = txtNote.Text.Trim
        mItem.OpeningBalances.CurrentItem.CureQtrs = Val(txtCureQtrs.Text)
        mItem.OpeningBalances.CurrentItem.CureYear = Val(txtCureYear.Text)
        mItem.OpeningBalances.CurrentItem.ExpQtrs = Val(txtExpQrts.Text)
        mItem.OpeningBalances.CurrentItem.ExpYear = Val(txtExpYear.Text)
        mItem.OpeningBalances.CurrentItem.BatchNo = Trim(txtBatchNo.Text)
        mItem.OpeningBalances.CurrentItem.Receipt.IntReceiptNo = Trim(txtInternalReceiptNo.Text)
        mItem.OpeningBalances.CurrentItem.CCommercialRate = Val(txtCommercialRate.Text)
        If txtCalibrationDoneOnDate.Text.ToString <> "" Then
            mItem.OpeningBalances.CurrentItem.CalibrationDoneOnDate = txtCalibrationDoneOnDate.Text.ToString
        Else
            mItem.OpeningBalances.CurrentItem.CalibrationDoneOnDate = System.DBNull.Value
        End If
        mItem.OpeningBalances.CurrentItem.IsExpiryNA = chkIsExpiryNA.Checked
        mItem.OpeningBalances.CurrentItem.IsExpiryUnlimited = chkIsExpiryUnlimited.Checked
        mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.CodeNo = Trim(txtCodeNo.Text) 'Added By Prashant On 07-Oct-2015 For ALL06102015
        'If txtConditionCheckDoneOnDate.Text.ToString <> "" Then
        '    mItem.OpeningBalances.CurrentItem.ConditionCheckDoneOnDate = txtConditionCheckDoneOnDate.Text.ToString
        'Else
        '    mItem.OpeningBalances.CurrentItem.ConditionCheckDoneOnDate = System.DBNull.Value
        'End If
    End Function
    Private Sub controlvisibility()
        'If chkIsExpiryNA.Checked Or chkIsExpiryUnlimited.Checked Then
        If (chkIsExpiryNA.Checked Or chkIsExpiryUnlimited.Checked) And (AppSettings("ClientCode") <> "IND") Then 'IND'Commneted and Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
            If chkIsExpiryNA.Checked Then
                chkIsExpiryUnlimited.Checked = False
                chkIsExpiryUnlimited.Enabled = False
            ElseIf chkIsExpiryUnlimited.Checked Then
                chkIsExpiryNA.Checked = False
                chkIsExpiryNA.Enabled = False
            End If
            txtStartDate.Enabled = False
            txtExpiryDate.Enabled = False
            txtCureQtrs.Enabled = False
            txtCureYear.Enabled = False
            txtExpQrts.Enabled = False
            txtExpYear.Enabled = False

            txtStartDate.Text = ""
            txtExpiryDate.Text = ""
            txtCureQtrs.Text = "0"
            txtCureYear.Text = "0"
            txtExpQrts.Text = "0"
            txtExpYear.Text = "0"
        Else
            txtStartDate.Enabled = True
            txtExpiryDate.Enabled = True
            txtCureQtrs.Enabled = True
            txtCureYear.Enabled = True
            txtExpQrts.Enabled = True
            txtExpYear.Enabled = True
        End If
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            lblOtheCharge.Visible = False
            txtOtherCharge.Visible = False
        Else
            lblOtheCharge.Visible = True
            txtOtherCharge.Visible = True
        End If
        IssueCount = ReceiptItems.GetIssueCount(mItem.OpeningBalances.CurrentItem.ReceiptItemID).CurrentItem.IssueCount
        If IssueCount > 0 Then
            txtSerialNo.Enabled = False
            cmbStoreList.Enabled = False
        Else
            txtSerialNo.Enabled = True
            cmbStoreList.Enabled = True
        End If
        If (AppSettings("CodeNo") = "True" And mItem.SerialisedStatus = True And mItem.PrimaryCategoryID = 2) Then
            lblCodeNo.Visible = True
            txtCodeNo.Visible = True
            'Added By Vikrant On 21-Dec-2016 For ALL21122016-1
            lblCodeNo.Text = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "GSE No.", "Code No.")
            txtCodeNo.ToolTip = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "Enter GSE No.", "Enter Code No.")
            'End
        Else
            lblCodeNo.Visible = False
            txtCodeNo.Visible = False
        End If



    End Sub
    Private Sub ControlvisibilityForExpiryInfo()
        'If txtStartDate.Text <> "" Or txtExpiryDate.Text <> "" Or txtCureQtrs.Text <> "0" Or txtCureYear.Text <> "0" Or txtExpQrts.Text <> "0" Or txtExpYear.Text <> "0" Then
        'If txtStartDate.Text <> "" Or txtExpiryDate.Text <> "" Or (txtCureQtrs.Text <> "0" And txtCureQtrs.Text <> "") Or (txtCureYear.Text <> "0" And txtCureYear.Text <> "") Or (txtExpQrts.Text <> "0" And txtExpQrts.Text <> "") Or (txtExpYear.Text <> "0" And txtExpYear.Text <> "") Then
        If (
            (txtStartDate.Text <> "" Or txtExpiryDate.Text <> "") Or (txtCureQtrs.Text <> "0" And txtCureQtrs.Text <> "") Or
            (txtCureYear.Text <> "0" And txtCureYear.Text <> "") Or (txtExpQrts.Text <> "0" And txtExpQrts.Text <> "") Or
            (txtExpYear.Text <> "0" And txtExpYear.Text <> "")
           ) And (AppSettings("ClientCode") <> "IND") Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
            chkIsExpiryNA.Enabled = False
            chkIsExpiryUnlimited.Enabled = False
        ElseIf Session("EditForExpiryInfo") = "True" Then
            Session("EditForExpiryInfo") = "False"
        Else
            chkIsExpiryNA.Enabled = True
            chkIsExpiryUnlimited.Enabled = True
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "OpeningStockTransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    End If
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCurrencyList = CurrencyList.GetCurrencyList("", "", True)
        mStoreList = StoreList.GetStoreList(0, "", True)
        Session("mCurrencyList") = mCurrencyList
        Session("mStoreList") = mStoreList
        cmbCurrencyList.DataSource = mCurrencyList
        cmbStoreList.DataSource = mStoreList

        mTypeListReceipt = TypeListForReceipt.GetTypeList("5", "[NONE]", Util.Trans.OpeningStock)        'ReceiptCumInvoice
        cmbReceivedFrom.DataSource = mTypeListReceipt
        Session("mTypeListReceipt") = mTypeListReceipt

        mVendorList = VendorList.GetVendortList(0, , , , , , True, , True)
        cmbVendor.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        mMachineNameValueList = MachineNameValueList.GetMachineList(mItem.OpeningBalances.CurrentItem.InvoiceDate.ToString, , , , , , , True, "(SELECT)", ForInventory:=True)

        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        mFromStoreList = StoreList.GetStoreList(0, , True)
        cmbFromStore.DataSource = mFromStoreList
        Session("mFromStoreList") = mFromStoreList
        mPartTypeList = PartTypeList.GetPartTypeList(True)
        cmbPartType.DataSource = mPartTypeList
        Session("mPartTypeList") = mPartTypeList

        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
        cmbWorkShop.DataSource = mWorkShopList
        Session("mWorkShopList") = mWorkShopList
        txtReceiptDate.Text = mItem.OpeningBalances.CurrentItem.InvoiceDate.ToString
        txtReleaseNoteDate.Text = mItem.OpeningBalances.CurrentItem.ReleaseNoteDate.ToString
        txtStartDate.Text = mItem.OpeningBalances.CurrentItem.StartDate.ToString
        txtExpiryDate.Text = mItem.OpeningBalances.CurrentItem.ExpiryDate.ToString
        txtInvoiceDate.Text = mItem.OpeningBalances.CurrentItem.VendorInvoiceDate.ToString
        lblExpPeriod.Text = mItem.OpeningBalances.CurrentItem.ExpiryPeriod
        txtCalibrationDoneOnDate.Text = mItem.OpeningBalances.CurrentItem.CalibrationDoneOnDate.ToString 'Added By Prashant 25-Sep-2009
        'txtConditionCheckDoneOnDate.Text = mItem.OpeningBalances.CurrentItem.ConditionCheckDoneOnDate.ToString
        mItemTypeList = PartTypeList.GetPartTypeList(True)
        Session("mItemTypeList") = mItemTypeList
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtReceiptDate" Then
            If txtReceiptDate.Text.ToString <> "" Then
                If DateDiff(DateInterval.Day, CDate(txtReceiptDate.Text), mItem.AsOnDate) < 0 Then
                    custValidator.ErrorMessage = "Receipt date must be prior or equals to As On Date."
                    e.IsValid = False
                End If
            End If
        ElseIf custValidator.ControlToValidate = "cmbCurrencyList" Then
            If cmbCurrencyList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select currency from the list."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtConversionFactor" Then
            If Val(txtConversionFactor.Text) <= 0 Then
                custValidator.ErrorMessage = "Conversion factor must be greater than zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtQty" Then
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
                Exit Sub
            End If
            If mItem.OpeningBalances.CurrentItem.Serialized Then
                If txtSerialNo.Text.Trim = "" And Val(txtQty.Text) > 1 Then
                    custValidator.ErrorMessage = "Serial No required." & vbCrLf & " Serialized Part's quantity must not be greater than one."
                    e.IsValid = True
                ElseIf txtSerialNo.Text.Trim = "" Then
                    custValidator.ErrorMessage = "Serial No Required."
                    e.IsValid = False
                ElseIf Val(txtQty.Text) > 1 Then
                    custValidator.ErrorMessage = " Serialized Part's quantity must not be greater than one."
                    e.IsValid = False
                End If
            End If
        ElseIf custValidator.ControlToValidate = "txtExpiryDate" Then
            If (Not txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And ((mItem.OpeningBalances.CurrentItem.ExpiryMonth <> 0 Or mItem.OpeningBalances.CurrentItem.ExpiryQuarter <> 0)) Then
                custValidator.ErrorMessage = "Select Cure Date "
                e.IsValid = False
            ElseIf Not txtExpiryDate.Text.ToString = "" And Not txtStartDate.Text.ToString = "" Then
                If CDate(txtExpiryDate.Text) < CDate(txtStartDate.Text) Then
                    custValidator.ErrorMessage = "Expiry Date should be Later to Cure Date."
                    e.IsValid = False
                End If
                'ElseIf (Not txtExpiryDate.Text = "" And txtStartDate.Text = "") And _
                '       ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And _
                '       ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And _
                '       (mItem.IsExpiryItem = True) Then  'Added by Prashant On 10-Aug-2020 All10082020
                '    custValidator.ErrorMessage = "Enter Expiry Information"
                '    e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbStoreList" Then
            If cmbStoreList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Store from the list."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtStartDate" Then
            If (txtExpiryDate.Text.ToString = "" And Not txtStartDate.Text.ToString = "") And
               ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And
               ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And
               ((mItem.OpeningBalances.CurrentItem.ExpiryMonth <> 0 Or mItem.OpeningBalances.CurrentItem.ExpiryQuarter <> 0)) _
               And Not (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "IND") Then
                custValidator.ErrorMessage = "Select Expiry Date "
                mItem = Session("mItem")
                mItem.OpeningBalances.CurrentIndex = CType(Session("ItemIndex"), Integer)
                Session("mItem") = mItem
                e.IsValid = False
            ElseIf (txtExpiryDate.Text.ToString = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) _
                And AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                custValidator.ErrorMessage = "Select Expiry Date "
                mItem = Session("mItem")
                mItem.OpeningBalances.CurrentIndex = CType(Session("ItemIndex"), Integer)
                Session("mItem") = mItem
                e.IsValid = False
                'ElseIf (txtExpiryDate.Text = "" And Not txtStartDate.Text = "") And _
                '      ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And _
                '    ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And _
                '     (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo") And (mItem.IsExpiryItem = True) Then 'Added by Prashant On 10-Aug-2020 All10082020
                '    mItem = Session("mItem")
                '    mItem.OpeningBalances.CurrentIndex = CType(Session("ItemIndex"), Integer)
                '    Session("mItem") = mItem
                '    custValidator.ErrorMessage = "Select Expiry Date "
                '    e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtInternalReceiptNo" Then
            If Len(txtInternalReceiptNo.Text) > 50 Then
                custValidator.ErrorMessage = " Max Length of Internal Receipt No should be 50. "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbFromStore" Then
            If cmbFromStore.Visible = True And cmbFromStore.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Store From List"
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.Visible = True And cmbAircraft.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Aircraft From List "
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbWorkShop" Then
            If cmbWorkShop.Visible = True And cmbWorkShop.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select WorkShop From List "
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbVendor" Then
            If cmbVendor.Visible = True And cmbVendor.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Supplier From List "
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbPartType" Then
            If cmbPartType.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select the Part Type"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtCureQtrs" Then
            If (Not txtExpiryDate.Text.ToString = "" Or Not txtStartDate.Text.ToString = "") And (Val(txtCureQtrs.Text) <> 0 Or Val(txtCureYear.Text) <> 0 Or Val(txtExpQrts.Text) <> 0 Or Val(txtExpYear.Text) <> 0) Then
                custValidator.ErrorMessage = "Enter either Cure/Expiry Date or Cure/Expiry Quarters."
                mItem = Session("tmpItem")
                mItem.OpeningBalances.CurrentIndex = CType(Session("ItemIndex"), Integer)
                Session("mItem") = mItem
                e.IsValid = False
            ElseIf Val(txtCureQtrs.Text) < 0 Or Val(txtCureQtrs.Text) > 4 Then
                custValidator.ErrorMessage = "Cure Quarters should be between 1 to 4"
                e.IsValid = False
            ElseIf (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0") Then
                custValidator.ErrorMessage = "Cure Year also required with Cure Qtrs."
                e.IsValid = False
            ElseIf ((txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "")) And
                   ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
                   ((txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0")) And
                   ((mItem.OpeningBalances.CurrentItem.ExpiryMonth <> 0 Or mItem.OpeningBalances.CurrentItem.ExpiryQuarter <> 0)) Then
                custValidator.ErrorMessage = "Expiry Year and Expiry Quarters required."
                e.IsValid = False
                'ElseIf ((txtExpiryDate.Text = "" And txtStartDate.Text = "")) And _
                '    ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And _
                '    ((txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0")) And _
                '    (mItem.IsExpiryItem = True) Then 'Added by Prashant On 10-Aug-2020 All10082020

                '    custValidator.ErrorMessage = "Enter Expiry Information"
                '    e.IsValid = False
            ElseIf ((
                    (mItem.OpeningBalances.CurrentItem.ExpiryMonth <> 0 Or mItem.OpeningBalances.CurrentItem.ExpiryQuarter <> 0) And
                    Not (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "IND")
                    ) And
                   ((txtStartDate.Text = "" And txtExpiryDate.Text = "") And
                   ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
                   ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")))) Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
                custValidator.ErrorMessage = "As " & mItem.OpeningBalances.CurrentItem.ExpiryPeriod & ". Enter Expiry Information"
                e.IsValid = False
                'ElseIf (((mItem.IsExpiryItem = True) And (AppSettings("ClientCode") <> "BA" Or AppSettings("ClientCode") <> "Novo")) And _
                '        ((txtStartDate.Text = "" And txtExpiryDate.Text = "") And _
                '        ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And _
                '        ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")))) Then  'Added by Prashant On 10-Aug-2020 All10082020
                '    custValidator.ErrorMessage = "Enter Expiry Information"
                '    e.IsValid = False
            ElseIf ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or
                    (AppSettings("ClientCode") = "IND" And mItem.IsExpiryItem = True)) And
                   ((txtStartDate.Text = "" And txtExpiryDate.Text = "") And
                   ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And
                   ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")) And
                   (chkIsExpiryNA.Checked = False) And (chkIsExpiryUnlimited.Checked = False))) Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
                custValidator.ErrorMessage = "Enter Expiry Information"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtOtherCharge" Then   '
            If txtCalibrationDoneOnDate.Text.ToString = "" And (mItem.StatusEquipment = True And mItem.BenchmarkMonths > 0 And mItem.CalibrationPeriodInID > 0) Then
                custValidator.ErrorMessage = "Part is Calibrated so Start Date required"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf custValidator.ControlToValidate = "txtInvoiceNo" Then   '
            '    If (txtConditionCheckDoneOnDate.Text.ToString = "" And _
            '        ((mItem.IsConditionCheck = True And mItem.ConditionCheckInterval > 0 And mItem.ConditionCheckIntervalIn > 0) Or (mItem.IsServicedInspected = True And mItem.ServicedInspectedInterval > 0 And mItem.ServicedInspectedIntervalIn > 0))) Then
            '        If mItem.IsConditionCheck = True Then
            '            custValidator.ErrorMessage = "Part is Condition Checked so Condition Check Start Date required"
            '        Else
            '            custValidator.ErrorMessage = "Part is Serviced/Inspected so Serviced Inspected Start Date required"
            '        End If
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        ElseIf custValidator.ControlToValidate = "txtExpQrts" Then
            If Val(txtExpQrts.Text) < 0 Or Val(txtExpQrts.Text) > 4 Then
                custValidator.ErrorMessage = "Expiry Quarters should be between 1 to 4"
                e.IsValid = False
            ElseIf (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0") Then
                custValidator.ErrorMessage = "Expiry Year also required with Expiry Qtrs."
                e.IsValid = False
            ElseIf ((txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "")) And ((txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0")) And ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")) And ((mItem.OpeningBalances.CurrentItem.ExpiryMonth <> 0 Or mItem.OpeningBalances.CurrentItem.ExpiryQuarter <> 0)) Then
                custValidator.ErrorMessage = "Cure Year and Cure Quarters required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtExpYear" Then
            If (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0") Then
                custValidator.ErrorMessage = "Expiry Qtrs also required with Expiry Year."
                e.IsValid = False
            ElseIf txtExpYear.Text <> "0" And Len(txtExpYear.Text) < 4 Then
                custValidator.ErrorMessage = "Expiry Year should be not be less than 4 digits"
                e.IsValid = False
            ElseIf txtExpYear.Text <> "0" And Val(txtExpYear.Text) < 1753 Or Val(txtExpYear.Text) > 3030 Then
                custValidator.ErrorMessage = "Enter valid Expiry Year"
                e.IsValid = False
            ElseIf (txtCureYear.Text <> "0" And txtExpYear.Text <> "0") And (Val(txtCureYear.Text) > Val(txtExpYear.Text)) Then
                custValidator.ErrorMessage = "Expiry Year should be Later to Cure Year."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            '------------------------------
        ElseIf custValidator.ControlToValidate = "txtCureYear" Then
            If (txtExpiryDate.Text.ToString = "" And txtStartDate.Text.ToString = "") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") Then
                custValidator.ErrorMessage = "Cure Qtrs also required with Cure Year."
                e.IsValid = False
            ElseIf txtCureYear.Text <> "0" And Len(txtCureYear.Text) < 4 Then
                custValidator.ErrorMessage = "Cure Year should be not be less than 4 digits"
                e.IsValid = False
            ElseIf txtCureYear.Text <> "0" And Val(txtCureYear.Text) < 1753 Or Val(txtCureYear.Text) > 3030 Then
                custValidator.ErrorMessage = "Enter valid Cure Year"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtCodeNo" Then
            If (AppSettings("CodeNo") = "True" And mItem.PrimaryCategoryID = 2 And mItem.SerialisedStatus = True) Then
                If (txtCodeNo.Text.Length = 0 Or txtCodeNo.Text.Trim = "") Then
                    custValidator.ErrorMessage = "Code No. Required"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If txtReceiptDate.Enabled = True Then
                setFocus(txtReceiptText)
            End If
            'Added by Utkarsh on 16-Dec-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mItem.OpeningBalances.CurrentItem.IsNew Then
                    mItem.OpeningBalances.CurrentItem.InvoiceText = Session("TransText_ForTransSeries")
                    txtReceiptText.Text = mItem.OpeningBalances.CurrentItem.InvoiceText
                    Session("mItem") = mItem
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind()
            If cmbReceivedFrom.SelectedItem.Text = "Aircraft" Then cmbAircraft.Visible = True
            If cmbReceivedFrom.SelectedItem.Text = "Store" Then cmbFromStore.Visible = True
            If cmbReceivedFrom.SelectedItem.Text = "Supplier" Then cmbVendor.Visible = True
            If cmbReceivedFrom.SelectedItem.Text = "WorkShop" Then cmbWorkShop.Visible = True
            If cmbReceivedFrom.SelectedIndex = 0 Or cmbReceivedFrom.SelectedIndex = 5 Then
                btnSelectDetails.Visible = False
            Else
                btnSelectDetails.Visible = True
            End If
            Call cmbPartType_SelectedIndexChanged(Nothing, Nothing)
        End If
        ControlvisibilityForExpiryInfo()
        controlvisibility()
        SetPage()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
            setObject()
            SetSession()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If cmbVendor.SelectedIndex > 0 Then
            If mVendorList(New Guid(cmbVendor.SelectedValue)).NotInUse = True Then
                If CDate(mVendorList(New Guid(cmbVendor.SelectedValue)).NotInUseDate) <= CDate(txtReceiptDate.Text) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Supplier is not applicable since " + mVendorList(New Guid(cmbVendor.SelectedValue)).NotInUseDateFormatted + "\n" + "Select another Supplier from list or select date before " + mVendorList(New Guid(cmbVendor.SelectedValue)).NotInUseDateFormatted + " & try again", False), True)
                    Exit Sub
                End If
            End If
        End If
        If IsValid Then
            Try
                setObject()
                If (mItem.OpeningBalances.CurrentItem.IsNew = True And (mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.DuplicateSerialNo() = True Or mItem.OpeningBalances.Contains(mItem.OpeningBalances.CurrentItem, "") = True)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Duplicate Item", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf (mItem.OpeningBalances.CurrentItem.IsNew = True And (mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.DuplicateCodeNo(1) = True Or mItem.OpeningBalances.ContainsCodeNo(mItem.OpeningBalances.CurrentItem) = True)) Then
                    If (mItem.SerialisedStatus = True And mItem.PrimaryCategoryID = 2 And AppSettings("CodeNo") = "True") Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You can not add duplicate Code No.", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                Else
                    '  mItem.ApplyEdit()
                    If (mItem.OpeningBalances.CurrentItem.IsNew) And (mItem.OpeningBalances.CurrentItem.InvoiceText = "") Then
                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(Trans.OpeningStock, mItem.OpeningBalances.CurrentItem.InvoiceDateFormatted)
                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.OpeningStock) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.OpeningStock) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.OpeningStock).TransText = "")) Then
                            Dim str = "<script language='javascript'>openledgersame('wfOpeningBalance_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "');</script>"
                            Session("BackPagestr_ForTransSeries") = str
                            Session("TransName_ForTransSeries") = "Opening Stock"
                            Session("TransTypeID_ForTransSeries") = Trans.OpeningStock
                            Session("TransDate_ForTransSeries") = mItem.OpeningBalances.CurrentItem.InvoiceDateFormatted

                            'Dim msg1 As New SIMsgBox(Page, "Opening Stock Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly)
                            'msg1.ReplacePage = "wfOpeningBalance_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                            'msg1.Show()
                            'Session("sender") = "OpeningStockTransTextSeriesAlert"
                            'Exit Sub
                            MSGBoxCtrl.Show("Opening Stock Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "OpeningStockTransTextSeriesAlert")
                            Exit Sub
                        Else
                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then
                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.OpeningStock)
                                    mItem.OpeningBalances.CurrentItem.InvoiceText = .TransText
                                    mItem.OpeningBalances.CurrentItem.InvoiceNo = .StartingTransNo
                                End With
                            Else
                                Dim str = "<script language='javascript'>openledgersame('wfOpeningBalance_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "');</script>"
                                Session("BackPagestr_ForTransSeries") = str

                                Session("TransName_ForTransSeries") = "Opening Stock"
                                Session("TransTypeID_ForTransSeries") = Trans.OpeningStock
                                Session("TransDate_ForTransSeries") = mItem.OpeningBalances.CurrentItem.InvoiceDateFormatted

                                'Dim msg1 As New SIMsgBox(Page, "Opening Stock Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfOpeningBalance_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                                'msg1.Show()
                                'Session("sender") = "OpeningStockTransTextSeriesAlert"
                                MSGBoxCtrl.Show("Opening Stock Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "OpeningStockTransTextSeriesAlert")
                                Exit Sub

                            End If
                        End If

                    End If
                End If
                Session("mItem") = mItem
                Session.Remove("EditItem")
                Session.Remove("tmpItem")
                Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

            Catch ex As SqlClient.SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Catch ex As Exception
                MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.Exception, ex.Message, MsgBoxStyle.OkOnly, "")
                Exit Sub
            Finally

            End Try
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
    End Sub
    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        '' txtConversionFactor.Text = Val(mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor)
        txtConversionFactor.Text = CDec(Format(mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor, "##0.00##"))
        If cmbCurrencyList.Enabled = True Then
            setFocus(cmbCurrencyList)
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        If Not Session("tmpItem") Is Nothing Then
            mItem = CType(Session("tmpItem"), Item)
            mItem.OpeningBalances.CurrentIndex = CType(Session("ItemIndex"), Integer)
            Session("mItem") = mItem
            Session.Remove("tmpItem")
        End If
        If mItem.OpeningBalances.CurrentItem.IsNew And Not Session("EditItem") = True Then mItem.OpeningBalances.Remove(mItem.OpeningBalances.CurrentItem)
        Session.Remove("EditItem")
        Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub txtStartDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStartDate.TextChanged

        If Not IsDate(txtStartDate.Text.Trim) Then
            txtStartDate.Text = ""
            Exit Sub
        End If

        If txtStartDate.Text.ToString <> mItem.OpeningBalances.CurrentItem.StartDate.ToString Then
            Dim tmpItem As Item = mItem.Clone
            If Session("tmpItem") Is Nothing Then
                Session("tmpItem") = tmpItem
                Session("ItemIndex") = mItem.OpeningBalances.CurrentIndex
            End If
            mItem.OpeningBalances.CurrentItem.StartDate = txtStartDate.Text.ToString
            txtStartDate.Text = New SmartDate(mItem.OpeningBalances.CurrentItem.StartDate.ToString).FormattedText
            txtExpiryDate.Text = New SmartDate(mItem.OpeningBalances.CurrentItem.ExpiryDate.ToString).FormattedText
            ControlvisibilityForExpiryInfo()
        End If
    End Sub
    Private Sub cmbReceivedFrom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReceivedFrom.SelectedIndexChanged
        cmbAircraft.Visible = False
        cmbVendor.Visible = False
        cmbFromStore.Visible = False
        cmbWorkShop.Visible = False
        If cmbReceivedFrom.SelectedItem.Text = "Supplier" Then cmbVendor.Visible = True
        If cmbReceivedFrom.SelectedItem.Text = "Aircraft" Then
            cmbAircraft.Visible = True
            btnSelectDetails.Enabled = False
        Else
            btnSelectDetails.Enabled = True
        End If
        If cmbReceivedFrom.SelectedItem.Text = "Store" Then cmbFromStore.Visible = True
        If cmbReceivedFrom.SelectedIndex = 0 Or cmbReceivedFrom.SelectedIndex = 5 Then
            btnSelectDetails.Visible = False
        Else
            btnSelectDetails.Visible = True
        End If
        If cmbReceivedFrom.SelectedItem.Text = "WorkShop" Then cmbWorkShop.Visible = True
        If cmbReceivedFrom.Enabled = True Then
            setFocus(cmbReceivedFrom)
        End If
    End Sub
    Private Sub btnSelectDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectDetails.Click
        If cmbReceivedFrom.SelectedItem.Text = "Supplier" Then
            Session("mItem") = mItem
            Session("OpType") = ""
            Response.Redirect("wfVendorList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&BackPage1=wfOpeningBalance_Ajax.aspx")
        ElseIf cmbReceivedFrom.SelectedItem.Text = "Store" Then
            Session("mItem") = mItem
            Session("OpType") = ""
            Response.Redirect("wfStore_Ajax.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=wfOpeningBalance_Ajax.aspx")
        ElseIf cmbReceivedFrom.SelectedItem.Text = "WorkShop" Then
            Session("mItem") = mItem
            Session("OpType") = ""
            Response.Redirect("wfWorkShop_Ajax.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=wfOpeningBalance_Ajax.aspx")
        End If
    End Sub
    Private Sub txtLandingCost_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLandingCost.TextChanged
        If Not mItem.OpeningBalances.CurrentItem Is Nothing Then
            setObject()
            DataBind()
            '     mItem.ApplyEdit()
        End If
    End Sub
    Private Sub txtLandingRates_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLandingRates.TextChanged
        If Not mItem.OpeningBalances.CurrentItem Is Nothing Then
            setObject()
            DataBind()
            '  mItem.ApplyEdit()
        End If
    End Sub
    Private Sub txtCureQtrs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCureQtrs.TextChanged
        If Val(txtCureQtrs.Text) >= 0 And Val(txtCureQtrs.Text) <= 4 Then
            If Val(txtCureYear.Text) = 0 And Session("tmpItem") Is Nothing Then
                Dim tmpItem As Item = mItem.Clone
                Session("tmpItem") = tmpItem
                Session("ItemIndex") = mItem.OpeningBalances.CurrentIndex
            End If
            mItem.OpeningBalances.CurrentItem.CureQtrs = Val(txtCureQtrs.Text)
            txtExpQrts.DataBind()
            txtExpYear.DataBind()
            If Val(txtCureQtrs.Text) = 0 Then txtCureQtrs.Text = "0"
            ControlvisibilityForExpiryInfo()
        End If
    End Sub
    Private Sub txtCureYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCureYear.TextChanged
        If Val(txtCureQtrs.Text) >= 0 And Val(txtCureQtrs.Text) <= 4 Then
            If Val(txtCureQtrs.Text) = 0 And Session("tmpItem") Is Nothing Then
                Dim tmpItem As Item = mItem.Clone
                Session("tmpItem") = tmpItem
                Session("ItemIndex") = mItem.OpeningBalances.CurrentIndex
            End If
            mItem.OpeningBalances.CurrentItem.CureYear = Val(txtCureYear.Text)
            txtExpQrts.DataBind()
            txtExpYear.DataBind()
            ControlvisibilityForExpiryInfo()
        End If
    End Sub
    Private Sub ImgbtnPartTypeNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgbtnPartTypeNew.Click
        setObject()
        Response.Redirect("wfItemType_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&BackPage1=wfOpeningBalance_Ajax.aspx&OpenFrom=3")
    End Sub
    Private Sub cmbPartType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPartType.SelectedIndexChanged
        If cmbPartType.SelectedIndex > 0 Then
            lblColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#" & mPartTypeList(cmbPartType.SelectedIndex).Color)
        Else
            lblColor.BackColor = System.Drawing.Color.WhiteSmoke
        End If
        lblPartStatus.Text = IIf(cmbPartType.SelectedIndex > 0, mItemTypeList(cmbPartType.SelectedIndex).PartStatusName, "")
    End Sub
    Private Sub txtExpQrts_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpQrts.TextChanged
        ControlvisibilityForExpiryInfo()
    End Sub
    Private Sub txtExpYear_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpYear.TextChanged
        ControlvisibilityForExpiryInfo()
    End Sub
    Private Sub txtExpiryDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpiryDate.TextChanged
        If Not IsDate(txtExpiryDate.Text.Trim) Then
            txtExpiryDate.Text = ""
            Exit Sub
        End If

        ControlvisibilityForExpiryInfo()
    End Sub
    Private Sub chkIsExpiryNA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsExpiryNA.CheckedChanged
        If chkIsExpiryNA.Checked Then
            If AppSettings("ClientCode") = "IND" Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
                'Do nothing 
            Else
                chkIsExpiryUnlimited.Enabled = False
            End If
        End If
    End Sub
    Private Sub chkIsExpiryUnlimited_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsExpiryUnlimited.CheckedChanged
        If chkIsExpiryUnlimited.Checked Then
            If AppSettings("ClientCode") = "IND" Then 'IND'Added by Prashant On 29-Oct-2020 change of 10-Aug-2020 All10082020
                'Do nothing 
            Else
                chkIsExpiryNA.Enabled = False
            End If
        End If
    End Sub
    Private Sub txtReceiptDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptDate.TextChanged
        mItem = Session("mItem")
        mItem.OpeningBalances.CurrentItem.InvoiceDate = txtReceiptDate.Text
        txtReceiptText.Text = mItem.OpeningBalances.CurrentItem.InvoiceText
        Session("mItem") = mItem
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub Print(Optional ByVal obj As rptStoresAcceptanceTag = Nothing) 'Added By Prashant 26-Feb-2021 IND26022021
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
            ''myReport = New crptUnserviceableTagForStarAir 'crptQUARANTINETagForStarAir '
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
            '' myReport = New crptStoreAcceptanceTag1 'crptQUARANTINETagForStarAir
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
    Private Sub btnPrintTag_Click(sender As Object, e As System.EventArgs) Handles btnPrintTag.Click 'Added By Prashant 26-Feb-2021 IND26022021
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As rptStoresAcceptanceTag
        Dim letter As rptLetterHead

        Dim ds As New dsStoresAcceptanceTag
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        obj = rptStoresAcceptanceTag.GetStoresAcceptanceTag(mItem.OpeningBalances.CurrentItem.ReceiptID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", AppSettings("WODocumentNo"), AppSettings("WORevisionNo"), AppSettings("Barcode"), AppSettings("ClientCode"))


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
		ElseIf AppSettings("ClientCode") = "PTW" Then
			myReport = New crptStoreAcceptanceTagForPattaya
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

    Private Sub txtReleaseNoteDate_TextChanged(sender As Object, e As EventArgs) Handles txtReleaseNoteDate.TextChanged
        If Not IsDate(txtReleaseNoteDate.Text.Trim) Then
            txtReleaseNoteDate.Text = ""
        End If
    End Sub
    Private Sub txtInvoiceDate_TextChanged(sender As Object, e As EventArgs) Handles txtInvoiceDate.TextChanged
        If Not IsDate(txtInvoiceDate.Text.Trim) Then
            txtInvoiceDate.Text = ""
        End If
    End Sub
    Private Sub txtCalibrationDoneOnDate_TextChanged(sender As Object, e As EventArgs) Handles txtCalibrationDoneOnDate.TextChanged
        If Not IsDate(txtCalibrationDoneOnDate.Text.Trim) Then
            txtCalibrationDoneOnDate.Text = ""
        End If
    End Sub
#End Region

End Class