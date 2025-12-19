Public Class wfOtherChargeDetails_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mOtherCharge As OtherCharge
    Private mChargeList As ChargeList
    Private mOtherChargeTypeList As OtherChargeTypeList
    Private mCurrencyList As CurrencyList
    Public mVendorList As VendorList
    'Added By Vikrant On 24-Sep-2020 For ALL24092020
    Dim mFileAttach As FileAttach
    'End
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mOtherCharge = Session("mOtherCharge")
        mChargeList = Session("mChargeList")
        mOtherChargeTypeList = Session("mOtherChargeTypeList")
        mCurrencyList = Session("mCurrencyList")
        mVendorList = Session("mVendorList")
        'Added By Vikrant On 24-Sep-2020 For ALL24092020
        mFileAttach = Session("mFileAttach")
        'End
    End Sub
    Private Sub SetSession()
        Session("mOtherCharge") = mOtherCharge
        Session("mChargeList") = mChargeList
        Session("mOtherChargeTypeList") = mOtherChargeTypeList
        Session("mCurrencyList") = mCurrencyList
        Session("mVendorList") = mVendorList
        'Added By Vikrant On 24-Sep-2020 For ALL24092020
        Session("mFileAttach") = mFileAttach
        'End
    End Sub
    Private Function Setobject() As Boolean
        mOtherCharge.BeginEdit()
        Dim ChargeID As New Guid(cmbCharge.SelectedValue.ToString)
        Dim VendorID As New Guid(cmbVendorList.SelectedValue.ToString)
        Dim CurrencyID As New Guid(cmbCurrencyList.SelectedValue.ToString)

        mOtherCharge.OtherChargeDetails.CurrentItem.SrNo = mOtherCharge.OtherChargeDetails.CurrentIndex + 1
        mOtherCharge.OtherChargeDetails.CurrentItem.ChargeID = ChargeID
        mOtherCharge.OtherChargeDetails.CurrentItem.VendorID = VendorID
        mOtherCharge.OtherChargeDetails.CurrentItem.CurrencyID = CurrencyID
        mOtherCharge.OtherChargeDetails.CurrentItem.OtherChargeTypeID = cmbChargeType.SelectedValue
        mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceNo = txtInvNo.Text
        If Not IsDate(txtInvDate.Text) Then
            mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDate = System.DBNull.Value
        Else
            mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDate = CDate(txtInvDate.Text)
        End If
        ''mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDate = txtInvDate.Text
        mOtherCharge.OtherChargeDetails.CurrentItem.ConversionFactor = Val(txtConversionFactor.Text)
        mOtherCharge.OtherChargeDetails.CurrentItem.CServiceCharges = Val(txtCSeriveCharge.Text)

        mOtherCharge.OtherChargeDetails.CurrentItem.CAmount = Val(txtChargeAmount.Text)

        ' ''If mOtherCharge.OtherChargeDetails.CurrentItem.Sign = 1 Then
        ' ''    mOtherCharge.OtherChargeDetails.CurrentItem.CAmount = -1 * System.Math.Abs(Val(txtChargeAmount.Text))
        ' ''End If

        If mOtherCharge.OtherChargeDetails.Contains(mOtherCharge.OtherChargeDetails.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, " Other Charge.", MsgBoxStyle.OkOnly, "")
            mOtherCharge.CancelEdit()
            Exit Function
        Else
            mOtherCharge.ApplyEdit()
            Return True
        End If
        txtGrandTotal.DataBind()
        Session("mOtherCharge") = mOtherCharge
    End Function
    Private Sub addAttributes()
        txtCSeriveCharge.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCSeriveCharge').value,event)")
        txtChargeAmount.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChargeAmount').value,event)")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " Binding Methods "
    Public Sub DataFieldBind()
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
        cmbCharge.DataSource = mChargeList

        mOtherChargeTypeList = OtherChargeTypeList.GetOtherChargeTypeList()
        Session("mOtherChargeTypeList") = mOtherChargeTypeList
        cmbChargeType.DataSource = mOtherChargeTypeList

		mVendorList = VendorList.GetVendortList(0, , , , , , True, , IsSupplier:=True, IsServiceProvider:=True)
		Session("mVendorList") = mVendorList
        cmbVendorList.DataSource = mVendorList

        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        Session("mCurrencyList") = mCurrencyList
        cmbCurrencyList.DataSource = mCurrencyList
        If Not mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDate.Equals(System.DBNull.Value) Then
            txtInvDate.Text = mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceDateFormatted
        End If

        DataBind()

        'Code Added by DEVEN On 28/12/2007 --------------------------------------
        If cmbCharge.Items.Contains(New System.Web.UI.WebControls.ListItem(mOtherCharge.OtherChargeDetails.CurrentItem.ChargeName, mOtherCharge.OtherChargeDetails.CurrentItem.ChargeID.ToString)) Then
            cmbCharge.SelectedValue = mOtherCharge.OtherChargeDetails.CurrentItem.ChargeID.ToString
        Else
            cmbCharge.SelectedValue = Guid.Empty.ToString
        End If
        '------------------------------------------------------------------------
    End Sub
    'Added By Vikrant On 24-Sep-2020 For ALL24092020
    Private Sub ControlVisibilityForFileAttachment()
        If mOtherCharge.OtherChargeDetails.CurrentItem.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
        upnlAttachFile.Update()
    End Sub
    'End
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        Dim Index As Int32 = IIf(cmbCharge.SelectedIndex <= 0, 0, cmbCharge.SelectedIndex)
        CustValidator = CType(s, CustomValidator)

        If CustValidator.ControlToValidate = "txtChargeAmount" Then
            If IsNumeric(txtChargeAmount.Text) Then
                If CDbl(txtChargeAmount.Text) <= 0 And mChargeList(Index).Sign <> 1 Then  ''And mChargeList(Index).PercentageTypeID = 1 Then
                    CustValidator.ErrorMessage = "Charge Amount should be Greater than Zero."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "txtCSeriveCharge" Then
            If IsNumeric(txtCSeriveCharge.Text) Then
                If CDbl(txtCSeriveCharge.Text) < 0 Then
                    CustValidator.ErrorMessage = "Service Charge should be Positive."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "txtConversionFactor" Then
            If Val(txtConversionFactor.Text) <= 0 Then
                CustValidator.ErrorMessage = "Currency factor must be greater than zero."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub SetTitle()
        If Session("EditCharge") Then
            lblTitle.Text = " Charge [ " & mOtherCharge.OtherChargeDetails.CurrentItem.ChargeName & " ]"
        Else
            lblTitle.Text = " Charge [ New ]"
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbVendorList.Enabled = True Then
                cmbVendorList.Focus()
            End If
            DataFieldBind()
            SetTitle()
            ControlVisibilityForFileAttachment() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
            'Session("mOtherCharge") = mOtherCharge
        End If
    End Sub
    Private Sub imgbtnCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCharge.Click
        Response.Redirect("wfCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfOtherChargeDetails_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mOtherCharge.OtherChargeDetails.CurrentItem.IsNew And Not Session("EditCharge") = True Then mOtherCharge.OtherChargeDetails.Remove(mOtherCharge.OtherChargeDetails.CurrentItem)
        'If mOrder.OrderItems.CurrentItem.IsNew And mOrder.OrderItems.CurrentItem.IsDirty Then mOrder.OrderItems.Remove(mOrder.OrderItems.CurrentItem)
        Session.Remove("EditCharge")
        Session.Remove("mChargeList")
        Session.Remove("mOtherChargeTypeList")
        Session.Remove("mCurrencyList")
        Session.Remove("mVendorList")
        Response.Redirect("wfOtherCharge_Ajax.aspx")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            '=============Commented by Saylee on 5th-Feb-2008 suggested by Kalpesh Sir.
            'Dim Id As New Guid(cmbCharge.SelectedValue)
            'If mOtherCharge.OtherChargeDetails.CurrentItem.IsNew And Not Session("EditCharge") Then
            '    mOtherCharge.OtherChargeDetails.Remove(mOtherCharge.OtherChargeDetails.CurrentItem)
            '    mOtherCharge.OtherChargeDetails.Add(Id)
            'End If
            '================Commented Code End============================================
            If Setobject() Then
                Session.Remove("EditCharge")
                Session.Remove("mChargeList")
                Session.Remove("mOtherChargeTypeList")
                Session.Remove("mCurrencyList")
                Session.Remove("mVendorList")
                Response.Redirect("wfOtherCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub cmbCharge_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCharge.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbCharge.SelectedIndex <= 0, 0, Val(cmbCharge.SelectedIndex))
        Setobject()
        If cmbCharge.Enabled = True Then
            cmbCharge.Focus()
        End If
    End Sub
    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        If cmbCurrencyList.Enabled = True Then
            cmbCurrencyList.Focus()
        End If
    End Sub
    'Added By Vikrant On 24-Sep-2020 For ALL24092020
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mOtherCharge.OtherChargeDetails.CurrentItem.IsAttachmentAdded = True Then
            'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoice.ID)
            mFileAttach = FileAttach.GetAttachmentChild(mOtherCharge.OtherChargeDetails.CurrentItem.ID)
        Else
            'mFileAttach = FileAttach.NewAttachment(Guid.Empty, mReceiptCumInvoice.ID)
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mOtherCharge.OtherChargeDetails.CurrentItem.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mOtherCharge.OtherChargeDetails.CurrentItem.IsAttachmentAdded Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments(0).Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments(0).ImageFile, 0, mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            Else
                MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                ControlVisibilityForFileAttachment()
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mOtherCharge.OtherChargeDetails.CurrentItem.IsAttachmentAdded = False
        mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments.RemoveAt(0)
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If mOtherCharge.OtherChargeDetails.CurrentItem.IsAttachmentAdded Then
            mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments(0).Size = mFileAttach.Size
            mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments(0).ImageFile = mFileAttach.ImageFile
            mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments(0).Extension = mFileAttach.Extension
        Else
            mOtherCharge.OtherChargeDetails.CurrentItem.IsAttachmentAdded = True
            mOtherCharge.OtherChargeDetails.CurrentItem.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
        End If
        Session("mOtherCharge") = mOtherCharge
        ControlVisibilityForFileAttachment()
    End Sub
    'End

#End Region

End Class