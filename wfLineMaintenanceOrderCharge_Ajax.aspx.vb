Public Class wfLineMaintenanceOrderCharge_Ajax
    Inherits Web.UI.Page

#Region " Variable Declaration "
    Public mLineMaintenanceOrder As LineMaintenanceOrder
    Public mLineMaintenanceOrderCharge As LineMaintenanceOrderCharge
    Private mChargeList As ChargeList
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mLineMaintenanceOrder = Session("mLineMaintenanceOrder")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
        Session("mChargeList") = mChargeList
    End Sub
    Private Function Setobject() As Boolean
        Dim Id As New Guid(cmbCharge.SelectedValue.ToString)
        If Not Id.Equals(Guid.Empty) Then
            Dim mLineMaintenanceOrderClone As LineMaintenanceOrder
            mLineMaintenanceOrderClone = mLineMaintenanceOrder.Clone
            mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.SrNo = mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentIndex + 1
            mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeID = Id
            mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ConversionFactor = mLineMaintenanceOrder.ConversionFactor
            mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ConversionFactor = mLineMaintenanceOrder.ConversionFactor
            If mLineMaintenanceOrder.LineMaintenanceOrderCharges.Contains(mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Service Order Charge.", MsgBoxStyle.OkOnly, "")
                mLineMaintenanceOrder.CancelEdit()
                Return False
                Exit Function
            Else
                mLineMaintenanceOrder.ApplyEdit()
                mLineMaintenanceOrder.CalculateTotal()
                If mLineMaintenanceOrder.IsRoundOff = True Then
                    mLineMaintenanceOrder.RoundCGrandTotal()
                End If
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
            Return True
        Else
            mLineMaintenanceOrder.CancelEdit()
            Return False
        End If
    End Function
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
        txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
    End Sub
    Private Sub setControl(Index As Int32)
        txtPercentage.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 3)
        txtChargeAmount.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 1)
        txtPercentage.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, mChargeList(Index).Percentage)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, txtChargeAmount.Text, 0)
        txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, txtChargeAmount.Text)
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
    Private Overloads Sub setFocus(cntrl As WebControl)
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

        If cmbCharge.Items.Contains(New Web.UI.WebControls.ListItem(mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeName, mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If

        If Session("Edit") Then
            If cmbCharge.Items.Contains(New Web.UI.WebControls.ListItem(mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeName, mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeID)
                txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
                txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
                txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
                txtPercentage.ToolTip = IIf(Not txtPercentage.ReadOnly, "Enter Percentage", "Percentage")
                txtChargeAmount.ToolTip = IIf(Not txtChargeAmount.ReadOnly, "Enter Charge Amount", "Charge Amount")
            End If
        End If
    End Sub
    Public Sub customvalidate(s As Object, e As ServerValidateEventArgs)
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
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
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
            lblTitle.Text = "Service Order Charge [ " & mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "Service Order Charge [ New ]"
        End If
        Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
    End Sub
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If IsValid Then
            If Setobject() Then
                If (mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.Sign <> 1 And mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.CChargeAmount <= 0) Or (Not (mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.IsValid)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session.Remove("Edit")
                Response.Redirect("wfLineMaintenanceOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnCharge_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLineMaintenanceOrderCharge_Ajax.aspx")
    End Sub
    Private Sub cmbCharge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCharge.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
        setControl(Index)
        upnlOtherChargeDetails.Update()
        If cmbCharge.Enabled = True Then
            setFocus(cmbCharge)
        End If
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem.IsNew And Not Session("Edit") = True Then mLineMaintenanceOrder.LineMaintenanceOrderCharges.Remove(mLineMaintenanceOrder.LineMaintenanceOrderCharges.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect("wfLineMaintenanceOrder_Ajax.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region


End Class