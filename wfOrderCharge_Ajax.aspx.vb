Public Class wfOrderCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mOrder As Order
    Public mOrderCharge As OrderCharge
    Private mChargeList As ChargeList
    Dim mModuleName As String
    Dim mChargeInfo As String = ""
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mOrder = Session("mOrder")
        mChargeList = Session("mChargeList")
        mModuleName = Session("mModuleName")
    End Sub
    Private Sub SetSession()
        Session("mOrder") = mOrder
        Session("mChargeList") = mChargeList
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Function Setobject() As Boolean
        Dim Id As New Guid(cmbCharge.SelectedValue.ToString)
        If Not Id.Equals(Guid.Empty) Then
            mOrder.OrderCharges.CurrentItem.SrNo = mOrder.OrderCharges.CurrentIndex + 1
            mOrder.OrderCharges.CurrentItem.ChargeID = Id
            mOrder.OrderCharges.CurrentItem.ConversionFactor = mOrder.ConversionFactor
            mOrder.OrderCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mOrder.OrderCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            mOrder.OrderCharges.CurrentItem.ConversionFactor = mOrder.ConversionFactor
            If mOrder.OrderItems.Count > 0 Then
                mOrder.OrderCharges.CurrentItem.BasicAmount = mOrder.OrderItems.CTotalAmount
            End If
            If mOrder.OrderCharges.Contains(mOrder.OrderCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Order Charge.", MsgBoxStyle.OkOnly, "")
                mOrder.CancelEdit()
                Return False
                Exit Function
            Else
                mOrder.ApplyEdit()
                mOrder.CalculateTotal()
                If mOrder.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
                    mOrder.RoundCGrandTotal()
                End If
                If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
                    mChargeInfo = "After Change Info. Charge Name " + mOrder.OrderCharges.CurrentItem.ChargeName + " Of Amount " + mOrder.OrderCharges.CurrentItem.CChargeAmount.ToString + " Added"
                Else
                    mChargeInfo = "Charge Name " + mOrder.OrderCharges.CurrentItem.ChargeName + " Of Amount " + mOrder.OrderCharges.CurrentItem.CChargeAmount.ToString + " Added"
                End If
                MarkLog(Util.Action.[New], mModuleName, mChargeInfo, Util.ErrorType.NoError, mOrder.ID, EventLogID)
                Return True
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mOrder") = mOrder
        Else
            mOrder.CancelEdit()
            If Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Then
                mChargeInfo = "After Change Info. Charge Name " + mOrder.OrderCharges.CurrentItem.ChargeName + " Of Amount " + mOrder.OrderCharges.CurrentItem.CChargeAmount.ToString + " Edited"
            Else
                mChargeInfo = "Charge Name " + mOrder.OrderCharges.CurrentItem.ChargeName + " Of Amount " + mOrder.OrderCharges.CurrentItem.CChargeAmount.ToString + " Edited"
            End If
            MarkLog(Util.Action.Edit, mModuleName, mChargeInfo, Util.ErrorType.NoError, mOrder.ID, EventLogID)
        End If
    End Function
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
        If (txtChargeAmount.ReadOnly = True Or txtChargeAmount.Enabled = False) Then
            '
        Else
            txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
        End If
    End Sub
    Private Sub setControl(ByVal Index As Int32)
        txtPercentage.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 3)
        txtChargeAmount.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 1)
        txtPercentage.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, mChargeList(Index).Percentage)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, txtChargeAmount.Text, 0)
        txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, txtChargeAmount.Text)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region " Binding Methods "
    Private Sub GetList()
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
    End Sub
    Public Sub DataFieldBind()
        cmbCharge.DataSource = mChargeList
        txtPercentage.DataBind()
        txtChargeAmount.DataBind()
        DataBind()
        'Code Added by DEVEN On 29/12/2007 --------------------------------------
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mOrder.OrderCharges.CurrentItem.ChargeName, mOrder.OrderCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mOrder.OrderCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        If Session("Edit") Then
            'Condation Added by DEVEN On 28/12/2007 --------------------------------------
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mOrder.OrderCharges.CurrentItem.ChargeName, mOrder.OrderCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mOrder.OrderCharges.CurrentItem.ChargeID)
                txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
                txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
                txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
                txtPercentage.ToolTip = IIf(Not txtPercentage.ReadOnly, "Enter Percentage", "Percentage") 'Code Added by DEVEN On 28/12/2007 --------------------------------------
                txtChargeAmount.ToolTip = IIf(Not txtChargeAmount.ReadOnly, "Enter Charge Amount", "Charge Amount")
            End If
        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        Dim Index As Int32 = IIf(cmbCharge.SelectedIndex <= 0, 0, cmbCharge.SelectedIndex)
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "cmbCharge" Then
            If cmbCharge.SelectedIndex = 0 Then
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If txtPercentage.Enabled = True Then
            If CustValidator.ControlToValidate = "txtPercentage" Then
                If IsNumeric(txtPercentage.Text) Then
                    If CDbl(txtPercentage.Text) <= 0 And mChargeList(Index).PercentageTypeID = 3 Then
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                Else
                    e.IsValid = False
                End If
            End If
        End If
        If CustValidator.ControlToValidate = "txtChargeAmount" Then
            If IsNumeric(txtChargeAmount.Text) Then
                If CDbl(txtChargeAmount.Text) <= 0 And mChargeList(Index).PercentageTypeID = 1 Then
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbCharge.Enabled = True Then
                setFocus(cmbCharge)
            End If
            GetList()
            DataFieldBind()
        End If
        If Session("Edit") Then
            lblTitle.Text = "Order Charge [ " & mOrder.OrderCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "Order Charge [ New ]"
        End If
        Session("mOrder") = mOrder
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If Setobject() = True Then
                If (mOrder.OrderCharges.CurrentItem.Sign <> 1 And mOrder.OrderCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mOrder.OrderCharges.CurrentItem.IsValid)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Order Charge(s) are not allowed if Order Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                    mOrder.CancelEdit()
                    Exit Sub
                End If
                Session.Remove("Edit")
                Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
            Else
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfOrderCharge_Ajax.aspx")
    End Sub
    Private Sub cmbCharge_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCharge.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
        setControl(Index)
        upnlOtherChargeDetails.Update()
        If cmbCharge.Enabled = True Then
            setFocus(cmbCharge)
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mOrder.OrderCharges.CurrentItem.IsNew And Not Session("Edit") = True Then mOrder.OrderCharges.Remove(mOrder.OrderCharges.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect("wfPurchaseOrder_Ajax.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
     End Sub
#End Region

End Class