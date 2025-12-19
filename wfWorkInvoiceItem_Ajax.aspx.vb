Public Class wfWorkInvoiceItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mWorkInvoice As WorkInvoice
    Public mWorkInvoiceItems As WorkInvoiceItems
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub FillUnitCombo()
        cmbUnit.Items.Add(New ListItem("Days", 1))
        cmbUnit.Items.Add(New ListItem("Hours", 2))
    End Sub
    Private Sub getSession()
        mWorkInvoice = Session("mWorkInvoice")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtAMEQty.Attributes.Add("onkeypress", "validateText(('NUM'),document.getElementById('txtAMEQty').value,event)")
        txtAMERate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAMERate').value,event)")
        txtHelperQty.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtHelperQty').value,event)")
        txtHelperRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtHelperRate').value,event)")
        txtTechQty.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtTechQty').value,event)")
        txtTechRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtTechRate').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Work Invoice Item"
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mWorkInvoice.WorkInvoiceItems.CurrentItem.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = IIf(mWorkInvoice.StatusID > 1, False, True)
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mWorkInvoice.WorkInvoiceItems.CurrentItem.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mWorkInvoice.WorkInvoiceItems.CurrentItem.ID)
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mWorkInvoice.WorkInvoiceItems.CurrentItem.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mWorkInvoice.WorkInvoiceItems.CurrentItem.ID)
        End If
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                Dim fs As FileStream
                If File.Exists(AppSettings("DOCPath")) = False Then
                    'Delete File if exist
                    System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                    ' Create the file.
                    fs = File.Create(path)
                    '' Add some information to the file.
                    fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                    fs.Close()
                    Session("DOCPath") = path
                    Dim Str As String
                    Str = "openFile();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                End If
            End If
        End If
    End Sub
    Private Function setObject() As Boolean
        mWorkInvoice.BeginEdit()
        mWorkInvoice.WorkInvoiceItems.CurrentItem.SrNo = mWorkInvoice.WorkInvoiceItems.CurrentIndex + 1
        mWorkInvoice.WorkInvoiceItems.CurrentItem.TaskDescription = Trim(txtDescription.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.UnitID = cmbUnit.SelectedValue
        mWorkInvoice.WorkInvoiceItems.CurrentItem.AMEQty = Val(txtAMEQty.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.AMECRate = Val(txtAMERate.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.TechQty = Val(txtTechQty.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.TechCRate = Val(txtTechRate.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.HelperQty = Val(txtHelperQty.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.HelperCRate = Val(txtHelperRate.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.Remark = Trim(txtRemark.Text)
        mWorkInvoice.WorkInvoiceItems.CurrentItem.Note = Trim(txtNote.Text)

        mWorkInvoice.ApplyEdit()
        Return True
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        txtSrNo.DataBind()
        txtDescription.DataBind()
        cmbUnit.DataBind()
        txtAMEQty.DataBind()
        txtAMERate.DataBind()
        txtTechQty.DataBind()
        txtTechRate.DataBind()
        txtHelperQty.DataBind()
        txtHelperRate.DataBind()
        txtRemark.DataBind()
        txtNote.DataBind()
        txtTotalRate.DataBind()
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custvalidator As CustomValidator
        custvalidator = CType(s, CustomValidator)

        If custvalidator.ControlToValidate = "txtAMEQty" Then
            If (Val(txtAMEQty.Text) <= 0.0 And Val(txtHelperQty.Text) <= 0.0 And Val(txtTechQty.Text) <= 0.0) Then
                custvalidator.ErrorMessage = "Enter at least one of the AME, Technician, Helper No."
                e.IsValid = False
                'ElseIf (Val(txtAMEQty.Text) > 0) Then
                '    If (Val(txtAMERate.Text) <= 0.0) Then
                '        custvalidator.ErrorMessage = "Enter AME Rate."
                '        e.IsValid = False
                '    End If
            End If
            'ElseIf custvalidator.ControlToValidate = "txtTechQty" Then
            '    If (Val(txtTechQty.Text) > 0) Then
            '        If (Val(txtTechRate.Text) <= 0.0) Then
            '            custvalidator.ErrorMessage = "Enter Technician Rate."
            '            e.IsValid = False
            '        End If
            '    End If
            'ElseIf custvalidator.ControlToValidate = "txtHelperQty" Then
            '    If (Val(txtHelperQty.Text) > 0) Then
            '        If (Val(txtHelperRate.Text) <= 0.0) Then
            '            custvalidator.ErrorMessage = "Enter Helper Rate."
            '            e.IsValid = False
            '        End If
            '    End If
        ElseIf custvalidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text.Trim) > 1999 Then
                custvalidator.ErrorMessage = "Description should not more than 2000 Charcters."
                e.IsValid = False
            End If
        ElseIf custvalidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text.Trim) > 1999 Then
                custvalidator.ErrorMessage = "Remark should not more than 2000 Charcters."
                e.IsValid = False
            End If
        ElseIf custvalidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text.Trim) > 1999 Then
                custvalidator.ErrorMessage = "Note should not more than 2000 Charcters."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        addAttributes()
        FillUnitCombo()
        If Not IsPostBack Then
            If txtDescription.Enabled = True Then
                setFocus(txtDescription)
            End If
            DataFieldBind()
            SetPage()
        End If
        ControlVisibilityForAttachment()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If setObject() Then
                Session("mWorkInvoice") = mWorkInvoice
                Session.Remove("Edit")
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
       If mWorkInvoice.WorkInvoiceItems.CurrentItem.IsAttachmentAdded = True Then
            mWorkInvoice.WorkInvoiceItems.CurrentItem.FileAttachments(0).Size = mFileAttach.Size
            mWorkInvoice.WorkInvoiceItems.CurrentItem.FileAttachments(0).ImageFile = mFileAttach.ImageFile
            mWorkInvoice.WorkInvoiceItems.CurrentItem.FileAttachments(0).Extension = mFileAttach.Extension
        Else
            mWorkInvoice.WorkInvoiceItems.CurrentItem.IsAttachmentAdded = True
            mWorkInvoice.WorkInvoiceItems.CurrentItem.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
        End If
        ControlVisibilityForAttachment()
        upnlAttachFile.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mWorkInvoice.WorkInvoiceItems.CurrentItem.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachmentChild(mWorkInvoice.WorkInvoiceItems.CurrentItem.ID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mWorkInvoice.WorkInvoiceItems.CurrentItem.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        mWorkInvoice.WorkInvoiceItems.CurrentItem.IsAttachmentAdded = False
        mWorkInvoice.WorkInvoiceItems.CurrentItem.FileAttachments.Remove(mWorkInvoice.WorkInvoiceItems.CurrentItem.ID)
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mWorkInvoice.WorkInvoiceItems.CurrentItem.IsNew And Not Session("Edit") = True Then mWorkInvoice.WorkInvoiceItems.Remove(mWorkInvoice.WorkInvoiceItems.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region

End Class