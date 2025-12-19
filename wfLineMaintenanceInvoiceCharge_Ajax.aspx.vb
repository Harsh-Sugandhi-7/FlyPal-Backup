Public Class wfLineMaintenanceInvoiceCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mLineMaintInvoice As LineMaintenanceInvoice
    Public mLineMaintInvoiceCharge As LineMaintenanceInvoiceCharge
    Private mChargeList As ChargeList
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mLineMaintInvoice = Session("mLineMaintInvoice")
        mChargeList = Session("mChargeList")
    End Sub
    Private Sub SetSession()
        Session("mLineMaintInvoice") = mLineMaintInvoice
        Session("mChargeList") = mChargeList
    End Sub
    Private Function Setobject() As Boolean
        Dim Id As New Guid(cmbCharge.SelectedValue.ToString)
        If Not Id.Equals(Guid.Empty) Then
            Dim invoiceclone As LineMaintenanceInvoice
            invoiceclone = mLineMaintInvoice.Clone
            mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.SrNo = mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentIndex + 1
            mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeID = Id
            mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ConversionFactor = mLineMaintInvoice.ConversionFactor
            mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.Percentage = Val(txtPercentage.Text.Trim)
            mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text.Trim)
            mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ConversionFactor = mLineMaintInvoice.ConversionFactor
            If mLineMaintInvoice.LineMaintenanceInvoiceCharges.Contains(mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Service Invoice Charge.", MsgBoxStyle.OkOnly, "")
                mLineMaintInvoice = invoiceclone
                Session("mLineMaintInvoice") = mLineMaintInvoice
                invoiceclone = Nothing
                Return False
            Else
                mLineMaintInvoice.CalculateTotal()
                If mLineMaintInvoice.IsRoundOff = True Then
                    mLineMaintInvoice.RoundCGrandTotal()
                End If
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mLineMaintInvoice") = mLineMaintInvoice
            Return True
        Else
            mLineMaintInvoice.CancelEdit()
            Return False
        End If
    End Function
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
        txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
    End Sub
    Private Sub setControl(ByVal Index As Int32)
        txtPercentage.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 3)
        txtChargeAmount.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 1)
        txtPercentage.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, mChargeList(Index).Percentage)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, txtChargeAmount.Text, 0)
        txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, txtChargeAmount.Text)
        'Setobject()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    Session("Sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
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
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeName, mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        If Session("Edit") Then
            If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeName, mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeID)
                txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
                txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
                txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)

                txtPercentage.ToolTip = IIf(Not txtPercentage.ReadOnly, "Enter Percentage", "Percentage")
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

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbCharge.Enabled = True Then
                cmbCharge.Focus()
            End If
            GetList()
            DataFieldBind()
            If Session("Edit") Then
                lblTitle.Text = "Service Invoice Charge [ " & mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.ChargeName & " ]"
            Else
                lblTitle.Text = "Service Invoice Charge [ New ]"
            End If
            Session("mLineMaintInvoice") = mLineMaintInvoice
        End If
      
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        'Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLineMaintenanceInvoiceCharge_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenChargeWindow", "OpenChargeWindow();", True)
    End Sub
    Private Sub cmbCharge_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCharge.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
        setControl(Index)
        If cmbCharge.Enabled = True Then
            cmbCharge.Focus()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.IsNew And Not Session("Edit") = True Then mLineMaintInvoice.LineMaintenanceInvoiceCharges.Remove(mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect("wfLineMaintenanceInvoice_Ajax.aspx")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            If Setobject() Then
                If (mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.Sign <> 1 And mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.CChargeAmount <= 0) Or Not (mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem.IsValid) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Service Invoice Charge(s) are not allowed if Service Invoice Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                    mLineMaintInvoice.CancelEdit()
                    Exit Sub
                End If
                Session.Remove("Edit")
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                'Response.Redirect("wfLineMaintenanceInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                'End If
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnimgBtnChargeList_Click(sender As Object, e As EventArgs) Handles hdnimgBtnChargeList.Click
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
        cmbCharge.DataSource = mChargeList
        cmbCharge.DataBind()
        upnlOtherChargeDetails.Update()
    End Sub
#End Region

End Class