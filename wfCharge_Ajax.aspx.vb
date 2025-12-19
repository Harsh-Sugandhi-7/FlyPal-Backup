Public Class wfCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Enum "
    Private Enum Rights
        [New] = 0
        Edit = 1
        Delete = 2
        View = 3
        Print = 4
        Authorized = 5
    End Enum
#End Region

#Region " Variable Declaration "
    Public mCharge As Charge
    Dim mChargeList As ChargeList
    Public mPercentTypeList As PercentTypeList
    Public mChargeTypeList As ChargeTypeList
    Dim Type As Int16
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mCharge = Session("mCharge")
        mChargeList = Session("mChargeList")
        mPercentTypeList = Session("mPercentTypeList")
        mChargeTypeList = Session("mChargeTypeList")
        Type = Session("Type")
    End Sub
    Private Sub SetSession()
        Session("mCharge") = mCharge
        Session("mChargeList") = mChargeList
        Session("mPercentTypeList") = mPercentTypeList
        Session("mChargeTypeList") = mChargeTypeList
    End Sub
    Private Sub SetTitle()
        If mCharge.IsNew = True Then
            lbltitle.Text = "Charge Information [New]"
        Else
            If Len(mCharge.Name) > 15 Then
                lbltitle.Text = "Charge Information [" & mCharge.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Charge Information [" & mCharge.Name & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub NewRecord()
        mCharge = Charge.NewCharge
        Session("mCharge") = mCharge
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        mCharge = Charge.GetCharge(ID)
        Session("mCharge") = mCharge
        SetTitle()
        DataBind()
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        If Not ID.Equals(Session("ChargeID")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            EditRecord(ID)
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        End If
    End Sub
    Public Function Save() As Boolean
        Try
            setObject()
            If mCharge.IsDirty Then mCharge.IsSync = 0
            If mCharge.IsValid Then
                mCharge.Save()
                NewRecord()
                GetList()
                DataFieldBind()
                txtChargeName.Text = ""
                Return True
            Else
                Return False
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Function
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Function
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Return False
        End Try
    End Function
    Private Sub GetList()
        mChargeList = ChargeList.GetChargeList("", -1)
        Session("mChargeList") = mChargeList
        mPercentTypeList = PercentTypeList.GetPercentTypeList()
        Session("mPercentTypeList") = mPercentTypeList
        mChargeTypeList = ChargeTypeList.GetChargeTypeList()
        Session("mChargeTypeList") = mChargeTypeList
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mCharge = Session("mCharge")
                            Charge.DeleteCharge(mCharge.ID)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.DeleteAlert, ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                Session.Remove("mUnitDetail")
                                NewRecord()
                                GetList()
                                DataFieldBind()
                                UpdatePanel()
                            Else
                                NewRecord()
                                GetList()
                                DataFieldBind()
                                UpdatePanel()
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        NewRecord()
                        DataFieldBind()
                        UpdatePanel()
                    End If
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
    End Sub
    Private Function IsInRole(ByVal mRights As Rights) As Boolean
        With User
            Select Case mRights
                Case Rights.[New]
                    Return (.IsInRole("QuotationNew") Or .IsInRole("PurchaseQuotationNew") Or .IsInRole("SalesOrderNew") Or _
                            .IsInRole("InvoiceNew") Or .IsInRole("OtherChargeNew") Or .IsInRole("SalesInvoiceNew") Or .IsInRole("OrderNew") Or _
                            .IsInRole("OrderForExchangeNew") Or .IsInRole("PurchaseOrderRepairOverHaulNew") Or _
                            .IsInRole("PurchaseOrderRentalLeaseNew"))
                Case Rights.Edit
                    Return (.IsInRole("QuotationEdit") Or .IsInRole("PurchaseQuotationEdit") Or .IsInRole("SalesOrderEdit") Or _
                            .IsInRole("InvoiceEdit") Or .IsInRole("OtherChargeEdit") Or .IsInRole("SalesInvoiceEdit") Or .IsInRole("OrderEdit") Or _
                            .IsInRole("OrderForExchangeEdit") Or .IsInRole("PurchaseOrderRepairOverHaulEdit") Or _
                            .IsInRole("PurchaseOrderRentalLeaseEdit"))
                Case Rights.Delete
                    Return (.IsInRole("QuotationDelete") Or .IsInRole("PurchaseQuotationDelete") Or .IsInRole("SalesOrderDelete") Or _
                            .IsInRole("InvoiceDelete") Or .IsInRole("OtherChargeDelete") Or .IsInRole("SalesInvoiceDelete") Or .IsInRole("OrderDelete") Or _
                            .IsInRole("OrderForExchangeDelete") Or .IsInRole("PurchaseOrderRepairOverHaulDelete") Or _
                            .IsInRole("PurchaseOrderRentalLeaseDelete"))
                Case Rights.View
                    Return (.IsInRole("QuotationView") Or .IsInRole("PurchaseQuotationView") Or .IsInRole("SalesOrderView") Or _
                            .IsInRole("InvoiceView") Or .IsInRole("OtherChargeView") Or .IsInRole("SalesInvoiceView") Or .IsInRole("OrderView") Or _
                            .IsInRole("OrderForExchangeView") Or .IsInRole("PurchaseOrderRepairOverHaulView") Or _
                            .IsInRole("PurchaseOrderRentalLeaseView"))
                Case Rights.Print
                    Return (.IsInRole("QuotationPrint") Or .IsInRole("PurchaseQuotationPrint") Or .IsInRole("SalesOrderPrint") Or _
                            .IsInRole("InvoicePrint") Or .IsInRole("OtherChargePrint") Or .IsInRole("SalesInvoicePrint") Or .IsInRole("OrderPrint") Or _
                            .IsInRole("OrderForExchangePrint") Or .IsInRole("PurchaseOrderRepairOverHaulPrint") Or _
                            .IsInRole("PurchaseOrderRentalLeasePrint"))
                Case Rights.Authorized
                    Return (.IsInRole("QuotationAuthorized") Or .IsInRole("PurchaseQuotationAuthorized") Or .IsInRole("SalesOrderAuthorized") Or _
                            .IsInRole("InvoiceAuthorized") Or .IsInRole("OtherChargeAuthorized") Or .IsInRole("SalesInvoiceAuthorized") Or .IsInRole("OrderAuthorized") Or _
                             .IsInRole("OrderForExchangeAuthorized") Or .IsInRole("PurchaseOrderRepairOverHaulAuthorized") Or _
                             .IsInRole("PurchaseOrderRentalLeaseAuthorized"))

            End Select
        End With
    End Function
    Private Sub UpdatePanel()
        upnlChargeDetails.Update()
        upnlGridView.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub setObject()
        mCharge.Name = Trim(txtChargeName.Text)
        mCharge.PercentageTypeID = CType(cmbPercentType.SelectedValue, Int16)
        mCharge.ChargeTypeID = CType(cmbChargeType.SelectedValue, Int16)
        mCharge.Sign = CType(cmbSign.SelectedValue, Int16)
        If txtPercentage.Text = "" Then txtPercentage.Text = 0
        mCharge.Percentage = CType(txtPercentage.Text, Decimal)
        mCharge.GLCode = txtGLCode.Text.Trim
    End Sub
    Private Sub DataFieldBind()
        txtChargeName.DataBind()
        txtPercentage.DataBind()
        txtGLCode.DataBind()
        cmbSign.DataBind()
        cmbPercentType.DataSource = mPercentTypeList
        cmbPercentType.DataBind()
        cmbChargeType.DataSource = mChargeTypeList
        cmbChargeType.DataBind()
        dgCharge.DataSource = mChargeList
        dgCharge.DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "txtPercentage" Then
            If cmbPercentType.SelectedValue = 2 Or cmbPercentType.SelectedValue = 3 Then
                If txtPercentage.Text = 0 Or txtPercentage.Text = "" Then
                    CustValidator.ErrorMessage = "Charge Percentage Required."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        End If
        If CustValidator.ControlToValidate = "txtChargeName" Then
            If Len(txtChargeName.Text) >= 50 Then
                CustValidator.ErrorMessage = "Charge Name can not greater than 50 characters."
                e.IsValid = False
            ElseIf Len(txtGLCode.Text) = 0 And AppSettings("ClientCode") = "Indamer" Then
                CustValidator.ErrorMessage = "GLCode required"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValidator.ControlToValidate = "cmbPercentType" Then
            If cmbPercentType.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Select Percentage Type from the list."
                e.IsValid = False
            End If
        End If
        If CustValidator.ControlToValidate = "cmbChargeType" Then
            If cmbPercentType.SelectedIndex > 0 And cmbChargeType.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Select Charge Type from the list."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            Type = CType(Request.QueryString("Type"), Int16)
            Session("Type") = Type
            If txtChargeName.Enabled = True Then
                setFocus(txtChargeName)
            End If
            If Session("sender") = "" And Session("New") <> "True" Then
                Session("PageHead") = Request.QueryString("PageHead")
                NewRecord()
            Else
                Session("New") = ""
            End If
            GetList()
            DataFieldBind()
        Else
            cmbPercentType.DataSource = mPercentTypeList
            cmbChargeType.DataSource = mChargeTypeList
            dgCharge.DataSource = mChargeList
            dgCharge.DataBind()
        End If
        Session("mCharge") = mCharge
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Dim mopenas As String = Request.QueryString("Typepup")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New]) And mCharge.IsNew) Or (Not IsInRole(Rights.Edit) And Not mCharge.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Save()
            If txtChargeName.Enabled = True Then
                setFocus(txtChargeName)
            End If
            UpdatePanel()
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtChargeName.Enabled = True Then
            setFocus(txtChargeName)
        End If
        NewRecord()
        upnlValidations.Update()
        txtChargeName.Text = ""
        txtPercentage.Text = ""
        DataFieldBind()
    End Sub
    Private Sub dgCharge_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCharge.RowCommand
        Dim index As Integer
        Dim ID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'index = CInt(e.CommandArgument) + dgCharge.PageIndex * dgCharge.PageSize
                'Session("index") = index

                
                ID = New Guid(e.CommandArgument.ToString) 'mChargeList(index).ID

                EditRecord(ID)

                txtChargeName.DataBind()
                txtPercentage.DataBind()
                cmbPercentType.SelectedValue = mCharge.PercentageTypeID.ToString()
                cmbChargeType.SelectedValue = mCharge.ChargeTypeID.ToString()
                cmbSign.SelectedValue = mCharge.Sign.ToString()
                txtPercentage.ReadOnly = (mCharge.PercentageTypeID = 1)
                txtPercentage.BackColor = IIf(mCharge.PercentageTypeID <> 1, Color.White, Color.Silver)
                If txtChargeName.Enabled = True Then
                    setFocus(txtChargeName)
                End If
                upnlChargeDetails.Update()
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'index = CInt(e.CommandArgument) + dgCharge.PageIndex * dgCharge.PageSize
                'Session("index") = index
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 15-Feb-2023
                index = gvr.RowIndex
                'ID = mChargeList(index).ID
                ID = New Guid(e.CommandArgument.ToString) 'mChargeList(index).ID Ajay 15-Feb-2023
                DeleteRecord(ID)
        End Select
    End Sub
    Private Sub cmbPercentType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPercentType.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbPercentType.SelectedIndex <= 0, 0, cmbPercentType.SelectedIndex)
        txtPercentage.ReadOnly = (mPercentTypeList(Index).PercentName = "(None)")
        txtPercentage.BackColor = IIf(mPercentTypeList(Index).PercentName <> "(None)", Color.White, Color.Silver)
        txtPercentage.Text = IIf(mPercentTypeList(Index).PercentName = "(None)", 0, txtPercentage.Text)
        If cmbPercentType.Enabled = True Then
            setFocus(cmbPercentType)
        End If
    End Sub
    Private Sub dgCharge_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCharge.PageIndexChanging
        dgCharge.PageIndex = e.NewPageIndex
        dgCharge.DataSource = mChargeList
        Session("mChargeList") = mChargeList
        dgCharge.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub dgCharge_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCharge.Sorting
        mChargeList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mChargeList") = mChargeList
        dgCharge.DataSource = mChargeList
        dgCharge.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class