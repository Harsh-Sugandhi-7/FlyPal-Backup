'AJAX Conversion By Vikrant On 22-Aug-2014

Public Class wfRequisitionItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mRequisitionNew As RequisitionNew
    Public mPriorityList As PriorityList
    Public mMachineNameValueList As MachineNameValueList
    Public mRequisitionItemTypeList As RequisitionItemTypeList
    Public OpeningFor As Integer
    Public RegNo As String = String.Empty
    Public WOText As String = String.Empty
    Public mWorkShopList As WorkShopList
    Public mDistinctOverhaulRepairOrderText As DistinctOverhaulRepairOrderText  'Added by Vikrant On 24-Jul-2015 For BA24072015 
    Dim mTransTypeID As Integer
    Public mUnitConverterList As UnitConverterList 'Added By Prashant On 07-May-2019 BA07052019
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mRequisitionNew = Session("mRequisitionNew")
        mPriorityList = Session("mPriorityList")
        mRequisitionItemTypeList = Session("mRequisitionItemTypeList")
        OpeningFor = Session("OpeningFor")
        mMachineNameValueList = Session("mMachineListForRequisitionItem")
        'mItemList = Session("mItemList")
        mWorkShopList = Session("mWorkShopList")
        mDistinctOverhaulRepairOrderText = Session("mDistinctOverhaulRepairOrderText")
        mTransTypeID = Session("TransTypeID")
    End Sub
    Private Sub setSession()
        Session("mRequisitionNew") = mRequisitionNew
        Session("mPriorityList") = mPriorityList
        Session("mRequisitionItemTypeList") = mRequisitionItemTypeList
        Session("mMachineListForRequisitionItem") = mMachineNameValueList
        'Session("mItemList") = mItemList
        Session("mWorkShopList") = mWorkShopList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPriorityList")
        Session.Remove("mMachineListForRequisitionItem")
        Session.Remove("mWorkShopList")
        Session.Remove("mDistinctOverhaulRepairOrderText")
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
        txtDays.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtDays').value,event)")
        'Added By Vikrant On 30-Aug-2016 For ALL30082016
        txtMaxStockLevel.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtMaxStockLevel.ClientID + "').value,event)")
        txtMinStockLevel.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtMinStockLevel.ClientID + "').value,event)")
        txtMinReOrderLevel.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtMinReOrderLevel.ClientID + "').value,event)")
        'End
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Requisition Item [" & mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo & "]"
            txtPartNo.BackColor = Color.Silver
            txtDescription.BackColor = Color.Silver
        End If
        If Session("Edit") Or (Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty)) Then
            If ((Session("Edit") And (Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty))) Or (Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty))) Then
                txtPartNo.BackColor = System.Drawing.Color.Gainsboro
                txtDescription.BackColor = System.Drawing.Color.Gainsboro
            Else
                txtPartNo.BackColor = System.Drawing.Color.White
                txtDescription.BackColor = System.Drawing.Color.White
            End If
        End If
    End Sub
    Private Function setObject(Optional ByVal IfOpenFromSave As Boolean = False) As Boolean 'Used to Set ItemID only for typed parts
        mRequisitionNew.RequisitionItemsNew.CurrentItem.SrNo = mRequisitionNew.RequisitionItemsNew.CurrentIndex + 1
        'mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = Guid.Empty 'check
        mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = Trim(txtWONo.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.NRCNo = Trim(txtNRCNo.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = New Guid(cmbMachine.SelectedValue)
        'mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = IIf(cmbMachine.SelectedIndex > 0, cmbMachine.SelectedItem.Text, "")
        mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = Trim(txtCostCenter.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForRequest = Trim(txtReasonForRequest.Text)
        'If Session("ItemID") <> "True" Then
        '    Session("ItemID") = "True"
        '    Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(txtPartNo.Text.Trim)
        '    If mFetchItemByName.Count > 0 Then
        '        If Not (mFetchItemByName(0).ID.Equals(Guid.Empty)) Then
        '            mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mFetchItemByName(0).ID
        '        End If
        '    End If
        'End If
        If ((mRequisitionNew.TransTypeID = 65 Or mRequisitionNew.TransTypeID = 72) And mRequisitionNew.ReqTypeID = 2) Or mRequisitionNew.TransTypeID = 77 Then
            If mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty) And IfOpenFromSave Then
                Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(txtPartNo.Text.Trim)
                If mFetchItemByName.Count > 0 Then
                    If Not (mFetchItemByName(0).ID.Equals(Guid.Empty)) Then
                        mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mFetchItemByName(0).ID
                    End If
                End If
            End If
        End If
        'End
        mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = Trim(txtPartNo.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = Trim(txtDescription.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = Trim(txtReference.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = CDec(Val(txtQty.Text))
        mRequisitionNew.RequisitionItemsNew.CurrentItem.PriorityID = CInt(cmbPriority.SelectedValue)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.ReasonForPurchase = Trim(txtReasonForPurchase.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Remark = Trim(txtRemark.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Note = Trim(txtNote.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Days = Val(txtDays.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.WorkShopName = IIf(cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.ToString, "")
        mRequisitionNew.RequisitionItemsNew.CurrentItem.OrderID = New Guid(cmbOrder.SelectedValue) 'Added by Vikrant On 24-Jul-2015 For BA24072015
        'Added By Vikrant On 30-Aug-2016 For ALL30082016
        mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = chkOneTimePurchase.Checked
        mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = CDec(Val(txtMinStockLevel.Text))
        mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = CDec(Val(txtMaxStockLevel.Text))
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
            If MaxMinQtyDiffForReOrder >= 0 Then
                txtMinReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
            End If
        End If

        mRequisitionNew.RequisitionItemsNew.CurrentItem.IsExchangePurchase = chkExchangePurchase.Checked            'Added by Shital on 18-Oct-2019

        mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = CDec(Val(txtMinReOrderLevel.Text))
        'End
        mRequisitionNew.RequisitionItemsNew.CurrentItem.DueDate = CDate(mRequisitionNew.ReqDate).AddDays(Val(txtDays.Text)) 'Added By Prashant On 16-Oct-2019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.ManualReference = Trim(txtManualRef.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.RequisitionItemTypeID = CInt(cmbRequisitionItemTypeList.SelectedValue)  'Added By Prashant On 23-jan-2019 For ALL22012019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.RequisitionItemTypeName = cmbRequisitionItemTypeList.SelectedItem.Text   'Added By Prashant On 23-jan-2019 For ALL22012019

        '' '--Added by Saylee on 8-Apr-2021
        mRequisitionNew.RequisitionItemsNew.CurrentItem.TSNValue = Trim(txtTSN.Text)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.CSNValue = Trim(txtCSN.Text)
        '''''


        If cmbUnitConverterList.SelectedIndex >= 0 Then
            mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = New Guid(cmbUnitConverterList.SelectedValue) 'Added By Prashant On 07-May-2019 BA07052019
            mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = cmbUnitConverterList.SelectedItem.Text     'Added By Prashant On 07-May-2019 BA07052019
        End If

        Dim mtmpItem As Item = Item.GetItem(mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID)                             'Added by Saylee on 24-Jul-2012

        If (mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID.Equals(Guid.Empty) And Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty)) Then
            mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mtmpItem.UnitID  'Added By Prashant On 07-May-2019 BA07052019
            mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mtmpItem.UnitName      'Added By Prashant On 07-May-2019 BA07052019
        End If
        If mRequisitionNew.RequisitionItemsNew.Contains(mRequisitionNew.RequisitionItemsNew.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Requisition Item", MsgBoxStyle.OkOnly, "")
            mRequisitionNew.CancelEdit()
            Exit Function
        ElseIf mtmpItem.NotInUse = True Then 'Added by Saylee on 24-Jul-2012
            If CDate(mtmpItem.NotInUseDate) <= CDate(mRequisitionNew.ReqDate) Then
                MSGBoxCtrl.Show("Save Alert!", "Part is not applicable since " + mtmpItem.NotInUseDateFormatted + " <br><br> Select another Part from list & try again", "", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
        Else
            mRequisitionNew.ApplyEdit()
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
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPriorityList = PriorityList.GetPriorityList(, , "")
        Session("mPriorityList") = mPriorityList
        cmbPriority.DataSource = mPriorityList

        mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=mRequisitionNew.ReqDateFormatted, IsTagRequired:=True, TagText:="(SELECT)", ForInventory:=True)
        Session("mMachineListForRequisitionItem") = mMachineNameValueList
        cmbMachine.DataSource = mMachineNameValueList

        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
        Session("mWorkShopList") = mWorkShopList
        cmbWorkShop.DataSource = mWorkShopList

        'Added by Vikrant On 24-Jul-2015 For BA24072015 
        mDistinctOverhaulRepairOrderText = DistinctOverhaulRepairOrderText.GetOrderList(True, "(SELECT)", "01-Jan-1900", mRequisitionNew.ReqDateFormatted, mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew)
        cmbOrder.DataSource = mDistinctOverhaulRepairOrderText
        Session("mDistinctOverhaulRepairOrderText") = mDistinctOverhaulRepairOrderText
        'End
        mRequisitionItemTypeList = RequisitionItemTypeList.GetRequisitionItemTypeList()
        cmbRequisitionItemTypeList.DataSource = mRequisitionItemTypeList

        mUnitConverterList = UnitConverterList.GetUnitConverterList(mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID)
        cmbUnitConverterList.DataSource = mUnitConverterList

        If Not mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty) And mUnitConverterList.Count <> 0 And
            Not mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID.Equals(Guid.Empty) Then
            cmbUnitConverterList.SelectedValue = mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID.ToString
        End If

        upnlReqItemDetails.DataBind()

    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQty" Then
            If Val(txtQty.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbMachine" Then

            If ((mRequisitionNew.TransTypeID = Trans.EngineeringRequisition And (mRequisitionNew.RequisitionEngineeringBrancheID = 1 Or mRequisitionNew.RequisitionEngineeringBrancheID = 2) And AppSettings("ClientCode") = "APFT") Or
               AppSettings("ClientCode") = "AAP" Or
               (
               (AppSettings("ClientCode") = "KAS" Or AppSettings("ClientCode") = "CE") And (mRequisitionNew.TransTypeID = Trans.EngineeringRequisition Or mRequisitionNew.TransTypeID = Trans.PlanningRequisition)
               ) Or
               (AppSettings("ClientCode") = "Heligo" And mRequisitionNew.TransTypeID = Trans.EngineeringRequisition)) Then ''CE Added By Prashant on 18-May-2022 CE1852022

                If cmbMachine.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Aircraft Required."
                    e.IsValid = False
                End If

            End If
        ElseIf custValidator.ControlToValidate = "cmbWorkShop" Then
            If mRequisitionNew.TransTypeID = Trans.WorkShopRequisition Then
                If cmbWorkShop.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "WorkShop Required."
                    e.IsValid = False
                End If
            End If
            'Added By Vikrant On 30-Aug-2016 For ALL30082016
        ElseIf custValidator.ControlToValidate = "txtMinStockLevel" Then
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" And mRequisitionNew.ReqTypeID = 2 Then
                If mRequisitionNew.TransTypeID <> 77 Then 'TransTypeID=77 : Planning Requisition
                    If chkOneTimePurchase.Checked Or (CDec(Val(txtMinStockLevel.Text)) > 0 Or CDec(Val(txtMinReOrderLevel.Text)) > 0 Or CDec(Val(txtMaxStockLevel.Text)) > 0) Then
                        'Do Nothing
                    Else
                        custValidator.ErrorMessage = "Either mark Requisition Item as One Time Purchase or enter either of the Min. Stock Level,Max. Stock Level,Min. Re-Order Level Quantities."
                        e.IsValid = False
                    End If
                End If
            End If
            'End
        ElseIf custValidator.ControlToValidate = "txtMaxStockLevel" Then
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                If Not chkOneTimePurchase.Checked And mRequisitionNew.TransTypeID <> 77 Then 'TransTypeID=77 : Planning Requisition
                    If (CDec(Val(txtMaxStockLevel.Text)) > 0) Then
                        If CDec(Val(txtMaxStockLevel.Text)) - CDec(Val(txtMinStockLevel.Text)) < 0 Then
                            custValidator.ErrorMessage = "Max Stock Level quantity should be greater than Min Stock Level quantity."
                            e.IsValid = False
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub AddSingleParts()
        With mRequisitionNew.RequisitionItemsNew.CurrentItem
            If Session("ItemName") <> "" Then
                'If Not mRequisitionNew.RequisitionItemsNew.Contains((CType(Session("ItemName"), RequisitionItemNew).ItemID), mRequisitionNew.RequisitionItemsNew.CurrentItem.ID) Then
                .ItemID = Guid.Empty
                ''.ReqItemID = Guid.Empty
                Try
                    ''.ReqPartNo = Session("ItemName")
                    .PartNo = Session("ItemName")

                Catch ex As Exception
                End Try
                .Description = Session("Description")
                ''.PartNo = Session("ItemName")
                ''.ReqDescription = Session("Description")
                ''.ReqPartNo = Session("ItemName")
                ''.IPCReference = ""
                ''.RequestedQty = 0
                'End 'If
            Else
                If Not mRequisitionNew.RequisitionItemsNew.Contains(CType(Session("SelectedRequisitionItem"), RequisitionItemNew).ItemID) Then
                    .ItemID = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).ItemID
                    .PartNo = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).PartNo
                    .Description = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).Description
                    .IPCReference = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).IPCReference
                    .RequestedQty = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).RequestedQty
                    .UnitID = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).UnitID
                    .Unit = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).Unit 'Added By Vikrant On 04-Nov-2014 For All04112014-1
                    'Added By Vikrant On 30-Aug-2016 For ALL30082016
                    .IsOneTimePurchase = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).IsOneTimePurchase
                    If Not CType(Session("SelectedRequisitionItem"), RequisitionItemNew).IsOneTimePurchase Then
                        .MinStockLevel = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).MinStockLevel
                        .MaxStockLevel = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).MaxStockLevel
                        .MinReOrderLevel = CType(Session("SelectedRequisitionItem"), RequisitionItemNew).MinReOrderLevel
                    Else
                        .MinStockLevel = 0
                        .MaxStockLevel = 0
                        .MinReOrderLevel = 0
                    End If
                    'End
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Part : '" + CType(Session("SelectedRequisitionItem"), RequisitionItemNew).PartNo.ToString + "' already taken for Requisition.", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
            End If
        End With
    End Sub
    Private Sub controlvisibility()
        'If Not mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID.Equals(Guid.Empty) Then
        '    cmbMachine.Enabled = False
        '    txtCostCenter.Enabled = False
        'End If
        If Not mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID.Equals(Guid.Empty) Then
            txtWONo.Enabled = False
            cmbMachine.Enabled = False
            txtCostCenter.Enabled = False
        Else
            If Not mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID.Equals(Guid.Empty) Then
                txtCostCenter.Enabled = False
            End If
        End If

        'If Session("Edit") Then
        If AppSettings("ClientCode") = "BA" Then
            If cmbPriority.SelectedItem.ToString.Equals("Other") Then
                lblInDays.Visible = True
                txtDays.Visible = True

            Else
                lblInDays.Visible = False
                txtDays.Visible = False
                txtDays.Text = "0"
            End If
        End If
        'If AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
        '    If cmbPriority.SelectedIndex = 5 And cmbPriority.SelectedItem.Equals("Other") Then
        '        txtDays.Enabled = True
        '    Else
        '        txtDays.Enabled = False
        '    End If
        'End If
        'If cmbPriority.SelectedItem.Equals("Other") Then
        '    If AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
        '        txtDays.Enabled = True
        '    Else
        '        txtDays.Enabled = False
        '    End If
        'Else
        '    txtDays.Enabled = False
        '    txtDays.Text = "0"
        'End If
        'End If
        'Added By Vikrant On 30-Aug-2016 For ALL30082016
        If CDec(Val(txtMinStockLevel.Text)) > 0 Or CDec(Val(txtMinReOrderLevel.Text)) > 0 Or CDec(Val(txtMaxStockLevel.Text)) > 0 Then
            chkOneTimePurchase.Enabled = False
        Else
            chkOneTimePurchase.Enabled = True
        End If
        'End
        If AppSettings("ClientCode") = "IND" Then
            lblNRCNo.Text = "OJS No."
            txtNRCNo.ToolTip = "Enter OJS No."
        Else
            lblNRCNo.Text = "NRC No."
            txtNRCNo.ToolTip = "Enter NRC No."
        End If


        If chkExchangePurchase.Checked = True Then
            plTSO.Visible = True
        Else
            plTSO.Visible = False
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        If CType(Session("AddSingleParts"), String) = "True" Then
            AddSingleParts()
            Session("AddSingleParts") = "False"
        Else
            Session("AddSingleParts") = "False"
        End If

        If Not IsPostBack Then
            If txtPartNo.Enabled = True Then
                txtPartNo.Focus()
            End If
            DataFieldBind()
            controlvisibility()
            SetPage()
        End If
    End Sub
    Private Sub hdnimgBtnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnPartNo.Click
        Session("AddMultipleParts") = "False"
        Session("Add") = True
        setObject()
        If mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID.Equals(Guid.Empty) Then 'v'
            Session("ItemName") = mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo '.ItemName
            Session("Description") = mRequisitionNew.RequisitionItemsNew.CurrentItem.Description '.ItemDescription
        Else
            Session("ItemName") = ""
            Session("Description") = ""
        End If
        Session("mRequisitionNew") = mRequisitionNew
        Session("mPriorityList") = mPriorityList
        Session("PartNo") = Trim(txtPartNo.Text)
        Session("mRequisitionItemTypeList") = mRequisitionItemTypeList
        Response.Redirect("wfRequisitionItemSearch_Ajax.aspx?BackPage=wfRequisitionItem_Ajax.aspx&ChildPage=wfRequisition_Ajax.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If setObject(True) Then
                RemoveSession()
                Session("mRequisitionNew") = mRequisitionNew
                Session.Remove("mRequisitionItemTypeList")
                Session.Remove("ItemID")
                Session.Remove("Edit")
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew And Not Session("Edit") = True Then mRequisitionNew.RequisitionItemsNew.Remove(mRequisitionNew.RequisitionItemsNew.CurrentItem)
        RemoveSession()
        Session.Remove("Edit")
        Session.Remove("mRequisitionItemTypeList")
        Session.Remove("ItemID")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub hdnbtnSelectWONo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hdnbtnSelectWONo.Click
        RegNo = IIf(cmbMachine.SelectedIndex > 0, cmbMachine.SelectedItem.Text, "")
        WOText = Trim(txtWONo.Text)
        Session("RegNo") = RegNo
        Session("WOText") = WOText
        setObject()
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub cmbPriority_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPriority.SelectedIndexChanged
        If AppSettings("ClientCode") = "BA" Then
            If cmbPriority.SelectedItem.ToString.Equals("Other") Then
                lblInDays.Visible = True
                txtDays.Visible = True
            Else
                lblInDays.Visible = False
                txtDays.Visible = False
                txtDays.Text = "0"
            End If
        Else
            txtDays.Text = mPriorityList.Item(cmbPriority.SelectedItem.ToString).Days
        End If
        If cmbPriority.SelectedItem.ToString.Equals("Other") Then
            txtDays.Enabled = True
        Else
            txtDays.Enabled = False
        End If
        upnlPriority.Update()
        'If AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
        '    If cmbPriority.SelectedIndex = 5 And cmbPriority.SelectedItem.Equals("Other") Then
        '        txtDays.Enabled = True
        '    Else
        '        txtDays.Enabled = False
        '    End If
        'End If
        'If cmbPriority.SelectedIndex = 5 Then
        '    If AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
        '        txtDays.Enabled = True
        '    Else
        '        txtDays.Enabled = False
        '    End If
        'Else
        '    txtDays.Enabled = False
        '    txtDays.Text = "0"
        'End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnWOList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnWOList.Click
        If Not Session("ID") Is Nothing Then
            mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = New Guid(Session("ID").ToString)
            mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = Session("WONo")
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = Session("WOMachineID")
            mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mMachineNameValueList(CType(Session("WOMachineID"), Guid)).RegNo
            Session.Remove("ID")
            Session.Remove("WONo")
            Session.Remove("WOMachineID")

            DataFieldBind()
            controlvisibility()
            SetPage()
            upnlReqItemDetails.Update()
        End If
    End Sub
    Private Sub hdnimgBtnRequisitionItemSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnRequisitionItemSearch.Click
        If CType(Session("AddSingleParts"), String) = "True" Then
            AddSingleParts()
            Session("AddSingleParts") = "False"
        Else
            Session("AddSingleParts") = "False"
        End If
        DataFieldBind()
        controlvisibility()
        SetPage()
        upnlReqItemDetails.Update()
    End Sub
    Private Sub cmbMachine_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMachine.SelectedIndexChanged
        If Not mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID.Equals(Guid.Empty) And Not txtWONo.Enabled Then
            txtWONo.Text = ""
            txtWONo.Enabled = True
            'txtWONo.DataBind()
        End If
    End Sub
    Private Sub txtMaxStockLevel_TextChanged(sender As Object, e As System.EventArgs) Handles txtMaxStockLevel.TextChanged
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
            If MaxMinQtyDiffForReOrder >= 0 Then
                txtMinReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
            End If
        End If
        controlvisibility()
        upnlReqItemDetails.Update()
    End Sub
    Private Sub txtMinStockLevel_TextChanged(sender As Object, e As System.EventArgs) Handles txtMinStockLevel.TextChanged
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
            If MaxMinQtyDiffForReOrder >= 0 Then
                txtMinReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
            End If
        End If
        controlvisibility()
        upnlReqItemDetails.Update()
    End Sub



    Private Sub chkExchangePurchase_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkExchangePurchase.CheckedChanged
        If chkExchangePurchase.Checked Then
            plTSO.Visible = True
        Else
            plTSO.Visible = False
        End If
    End Sub
#End Region



End Class