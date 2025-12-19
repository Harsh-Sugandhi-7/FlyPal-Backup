Public Class wfSalesInvoiceCharge_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mSalesInvoice As SalesInvoice
    Private mChargeList As ChargeList

#End Region

#Region " Buisness Method And Properties "

    Private Sub GetSession()
        mSalesInvoice = Session("mSalesInvoice")
        mChargeList = Session("mChargeList")
    End Sub

    Private Sub SetSession()
        Session("mSalesInvoice") = mSalesInvoice
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
            mSalesInvoice.SalesInvoiceCharges.CurrentItem.SrNo = mSalesInvoice.SalesInvoiceCharges.CurrentIndex + 1
            mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeID = Id
            mSalesInvoice.SalesInvoiceCharges.CurrentItem.ConversionFactor = mSalesInvoice.ConversionFactor
            mSalesInvoice.SalesInvoiceCharges.CurrentItem.Percentage = Val(txtPercentage.Text)
            mSalesInvoice.SalesInvoiceCharges.CurrentItem.CChargeAmount = Val(txtChargeAmount.Text)
            mSalesInvoice.SalesInvoiceCharges.CurrentItem.ConversionFactor = mSalesInvoice.ConversionFactor
            If mSalesInvoice.SalesInvoiceItems.Count > 0 Then
                mSalesInvoice.SalesInvoiceCharges.CurrentItem.BasicAmount = mSalesInvoice.SalesInvoiceItems.CGrandTotalAmountItem
            End If
            If mSalesInvoice.SalesInvoiceCharges.Contains(mSalesInvoice.SalesInvoiceCharges.CurrentItem) = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "SalesInvoice Charge.", MsgBoxStyle.OkOnly, "")
                mSalesInvoice.CancelEdit()
                Return False
                Exit Function
            Else
                mSalesInvoice.ApplyEdit()
                mSalesInvoice.CalculateTotal()
                If mSalesInvoice.IsRoundOff = True Then 'Added By Prashant on 21-May-2012 ALL25102012
                    mSalesInvoice.RoundCGrandTotal()
                End If
                Return True
            End If
            txtPercentage.DataBind()
            txtChargeAmount.DataBind()
            Session("mSalesInvoice") = mSalesInvoice
        Else
            mSalesInvoice.CancelEdit()
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

    Private Sub setControl(Index As Int32)
        txtPercentage.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 3)
        txtChargeAmount.ReadOnly = Not (mChargeList(Index).PercentageTypeID = 1)
        txtPercentage.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, mChargeList(Index).Percentage)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, txtChargeAmount.Text, 0)
        txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
        txtChargeAmount.Text = IIf(mChargeList(Index).PercentageTypeID = 1, 0, txtChargeAmount.Text)
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
        'Code Added by DEVEN On 29/12/2007 --------------------------------------
        If cmbCharge.Items.Contains(New ListItem(mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeName, mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        If Session("Edit") Then
            'Condation Added by DEVEN On 28/12/2007 --------------------------------------
            If cmbCharge.Items.Contains(New ListItem(mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeName, mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeID.ToString)) Then
                Dim mCharge As Charge = Charge.GetCharge(mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeID)
                txtPercentage.ReadOnly = Not (mCharge.PercentageTypeID = 3)
                txtChargeAmount.ReadOnly = Not (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(Not txtPercentage.ReadOnly, Color.White, Color.Silver)
                txtChargeAmount.BackColor = IIf(Not txtChargeAmount.ReadOnly, Color.White, Color.Silver)
                txtPercentage.ToolTip = IIf(Not txtPercentage.ReadOnly, "Enter Percentage", "Percentage") 'Code Added by DEVEN On 28/12/2007 --------------------------------------
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
            lblTitle.Text = "Sales Invoice Charge [ " & mSalesInvoice.SalesInvoiceCharges.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = "Sales Invoice Charge [ New ]"
        End If
        Session("mSalesInvoice") = mSalesInvoice
    End Sub

    Private Sub SaveCharges(sender As Object, e As EventArgs) Handles btnOK.Click

        If IsValid Then

            If Setobject() Then

                If (mSalesInvoice.SalesInvoiceCharges.CurrentItem.Sign <> 1 And mSalesInvoice.SalesInvoiceCharges.CurrentItem.CChargeAmount <= 0) Or
                   (Not (mSalesInvoice.SalesInvoiceCharges.CurrentItem.IsValid)) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert,
                                    MSGBox.Message_text.ValidationAlert,
                                    "Percentage SalesInvoice Charge(s) are not allowed if SalesInvoice Amount Is Zero. ",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    mSalesInvoice.CancelEdit()

                    Exit Sub

                Else

                    Session.Remove("EditCharge")

                    Dim openAs As String = Request.QueryString("Typepup")

                    If openAs IsNot Nothing AndAlso openAs = "pup" Then

                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "On Okay",
                                                            "CallParentCallback();",
                                                            True)
                        Exit Sub

                    End If

                End If

            Else
                Exit Sub
            End If

        Else
            upnlValidationSummary.Update()
        End If

    End Sub

    Private Sub ReturnToInvoicePage(sender As Object, e As EventArgs) Handles btnBack.Click

        If mSalesInvoice.SalesInvoiceCharges.CurrentItem.IsNew And
           Not Session("EditCharge") = True Then mSalesInvoice.SalesInvoiceCharges.Remove(mSalesInvoice.SalesInvoiceCharges.CurrentItem)

        Session.Remove("EditCharge")

        Dim openAs As String = Request.QueryString("Typepup")

        If openAs IsNot Nothing AndAlso openAs = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "On Close",
                                                "CallParentCallback();",
                                                True)

            Exit Sub

        End If

    End Sub

    Private Sub imgbtnCharge_Click(sender As Object, e As EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfSalesInvoiceCharge_Ajax.aspx")
    End Sub

    Private Sub cmbCharge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCharge.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
        setControl(Index)
        upnlOtherChargeDetails.Update()
        If cmbCharge.Enabled = True Then
            setFocus(cmbCharge)
        End If
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region

End Class