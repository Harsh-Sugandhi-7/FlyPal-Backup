Imports System.Web.Services.Description

Partial Class wfEmployeeDocument_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents chkDoneStatus As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblDoneStatus As System.Web.UI.WebControls.Label

    'Protected WithEvents txtIssueDate As SIControls.SICalendar
    'Protected WithEvents txtExpiryDate As SIControls.SICalendar

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeDocument As EmployeeDocument
    Public mEmployeeDocumentList As EmployeeDocumentList
    Public mDocumentList As DocumentList
    Public mIsRenew As Boolean = False

    Public BackPage As String
    Public mDocument As Document
    Public mCalibrationPeriodInList As CalibrationPeriodInList 'Added By Vikrant On 11-July-2016 For ALL11072016

    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeDocument = Session("mEmployeeDocument")
        mEmployeeDocumentList = Session("mEmployeeDocumentList")
        mDocumentList = Session("mDocumentList")
        mIsRenew = Session("IsRenew")
        mDocument = Session("mDocument")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDocument") = mEmployeeDocument
        'Session("mEmployeeSkillList") = mEmployeeSkillList
        Session("mDocumentList") = mDocumentList
        Session("mEmployee") = mEmployee
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        'Validity
        txtValidity.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtValidity').value,event)")
        'Warning Days
        txtWarningDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtWarningDays').value,event)")
    End Sub
    Private Sub DataFieldBind()
        mDocumentList = DocumentList.GetDocumentList(, "<SELECT>")
        cmbDocumentList.DataSource = mDocumentList
        Session("mDocumentList") = mDocumentList

        txtIssueDate.Text = mEmployeeDocument.DateOfIssueFormatted.ToString
        txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString

        'Added By Vikrant On 11-July-2016 For ALL11072016
        mCalibrationPeriodInList = CalibrationPeriodInList.GetCalibrationPeriodInList()
        Session("mCalibrationPeriodInList") = mCalibrationPeriodInList
        cmbDocumentValidityIn.DataSource = mCalibrationPeriodInList
        'End

        upnlDocumentDetails.DataBind()
    End Sub

    Public Sub Save()
        SetObject()
        If Not mEmployeeDocument.IsValid Then Exit Sub

        Try
            If ((mEmployeeDocumentList.Contains(mEmployeeDocument.EmployeeID, mEmployeeDocument.DocumentID, mEmployeeDocument.ReferenceID)) And mEmployeeDocument.IsNew) Then
                MSGBoxCtrl.Show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry. ", MsgBoxStyle.OkOnly, "")
            Else
                mEmployeeDocument.Save()
                SetSession()
                lblTitle.Text = "Employee Document Information [New]"
                MarkLog(Flypal.Util.Action.Save, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mDocumentList(mEmployeeDocument.DocumentID).Name, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
                Session.Remove("mEmployeeDocumentList")
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                'If Request.QueryString("ChildPage1") = "wfEmployeeDetails.aspx" Then
                '    Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                'Else
                '    Response.Redirect(Request.QueryString("BackPage") & "?ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                'End If
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mEmployeeDocument.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
        upnlDocumentDetails.Update()
    End Sub
    Private Sub NewRecordDocumentMaster()
        mDocument = Document.NewDocument
        Session("mDocument") = mDocument
    End Sub
    Private Sub EditRecordDocumentMaster(ByVal mID As Guid)
        mDocument = Document.GetDocument(mID)
        Session("mDocument") = mDocument
    End Sub
    Private Sub DeleteRecordDocumentMaster(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDocumentMaster")
        mDocument = Document.GetDocument(mID)
        Session("mDocument") = mDocument
    End Sub
    Private Sub SetObjectDocumentMaster()
        mDocument.Name = txtDocumentName.Text
    End Sub
    Private Sub DataFieldBindDocumentMaster()
        mDocumentList = DocumentList.GetDocumentList()
        dgDocument.DataSource = mDocumentList
        Session("mDocumentList") = mDocumentList
        dgDocument.DataBind()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteDocumentMaster" Then
                        Try
                            Session("sender") = ""
                            mDocument = Session("mDocument")
                            Document.DeleteDocument(mDocument.ID)
                            NewRecordDocumentMaster()
                            DataFieldBindDocumentMaster()
                            txtDocumentName.Text = ""
                            txtDocumentName.DataBind()
                            lblTitleDocumentMaster.Text = "Document Information [New]"
                            upnlDocumentMaster.Update()
                            'Response.Redirect("wfDocument.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Document", "Can't delete : " + mDocument.Name + "  is Currently in use", Flypal.Util.ErrorType.NoError, mDocument.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordDocumentMaster()
                            DataFieldBindDocumentMaster()
                            txtDocumentName.Text = ""
                            txtDocumentName.DataBind()
                            lblTitleDocumentMaster.Text = "Document Information [New]"
                            upnlDocumentMaster.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Document", mDocument.Name, Flypal.Util.ErrorType.NoError, mDocument.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteDocumentMaster" Then
                        NewRecordDocumentMaster()
                        txtDocumentName.DataBind()
                        lblTitleDocumentMaster.Text = "Document Information [New]"
                        dgDocument.DataSource = mDocumentList
                        upnlDocumentMaster.DataBind()
                        upnlDocumentMaster.Update()
                    End If
                    Session("sender") = ""
                    'Response.Redirect("wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'DataFieldBind()
            'Response.Redirect("wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mEmployeeDocument.IsNew Then
            lblTitle.Text = "Employee Document Information [New]"
        Else
            If Len(mEmployeeDocument.DocumentName) > 15 Then
                lblTitle.Text = "Employee Document Information [" & mEmployeeDocument.DocumentName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Document Information [" & mEmployeeDocument.DocumentName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Public Sub Customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtName" Then
            If Len(Trim(txtDocumentName.Text)) > 50 Then
                CustValid.ErrorMessage = " Document Name too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "cmbDocumentList" Then
            If cmbDocumentList.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Please select the Document."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf CustValid.ControlToValidate = "txtExpiryDate" Then
            '    If txtExpiryDate.Value.ToString = "" Then
            '        CustValid.ErrorMessage = "Date Of Expiry should not be blank."
            '        e.IsValid = False
            '    ElseIf txtExpiryDate.Value < txtIssueDate.Value Then
            '        CustValid.ErrorMessage = "Date Of Expiry should be greater than Date Of Issue."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
            'ElseIf CustValid.ControlToValidate = "txtIssueDate" Then
            '    If txtIssueDate.Value.ToString = "" Then
            '        CustValid.ErrorMessage = "Date Of Issue should not be blank."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        ElseIf CustValid.ControlToValidate = "txtValidity" Then
            'If Val(txtValidity.Text) <= 0 Then
            If Val(txtValidity.Text) <= 0 And chkOneTimeDocument.Checked = False Then ' And chkOneTimeDocument.Checked = False Added by Prashant 0n 24-Nov-2020 ALL24112020
                CustValid.ErrorMessage = "Validity required."
                e.IsValid = False
            ElseIf Val(txtValidity.Text) > 0 And chkOneTimeDocument.Checked = False And txtExpiryDate.Text = "" Then ' And chkOneTimeDocument.Checked = False Added by Prashant 0n 24-Nov-2020 ALL24112020
                CustValid.ErrorMessage = "Date of Expiry Required"
                e.IsValid = False
            End If
        ElseIf CustValid.ControlToValidate = "txtWarningDays" Then
            Dim ValidityInDays As String
            ValidityInDays = IIf(cmbDocumentValidityIn.SelectedValue = 1, txtValidity.Text, IIf(cmbDocumentValidityIn.SelectedValue = 2, (Val(txtValidity.Text) * 30).ToString, (Val(txtValidity.Text) * 365).ToString))
            'If Val(txtWarningDays.Text) <= 0 Then
            If Val(txtWarningDays.Text) <= 0 And chkOneTimeDocument.Checked = False Then ' And chkOneTimeDocument.Checked = False Added by Prashant 0n 24-Nov-2020 ALL24112020
                CustValid.ErrorMessage = "Warning days required."
                e.IsValid = False
            ElseIf Val(ValidityInDays) <= Val(txtWarningDays.Text) And chkOneTimeDocument.Checked = False Then ' And chkOneTimeDocument.Checked = False Added by Prashant 0n 24-Nov-2020 ALL24112020
                CustValid.ErrorMessage = "Warning days should be less than Validity Period."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "txtApplicabilityRemark" Then
            If chkApplicable.Checked Then
                If Len(Trim(txtApplicabilityRemark.Text)) > 500 Then
                    e.IsValid = False
                    CustValid.ErrorMessage = "Not Applicable Remark should not be greater than 500 characters."
                Else
                    e.IsValid = True
                End If
            Else

                If Len(Trim(txtApplicabilityRemark.Text)) = 0 Then
                    e.IsValid = False
                    CustValid.ErrorMessage = "Pleae enter Not Applicable Remark."
                ElseIf Len(Trim(txtApplicabilityRemark.Text)) > 500 Then
                    e.IsValid = False
                    CustValid.ErrorMessage = "Not Applicable Remark should not be greater than 500 characters."
                Else
                    e.IsValid = True
                End If
            End If

        End If
    End Sub
    Private Sub SetObject()
        mEmployeeDocument.EmployeeID = mEmployee.ID
        mEmployeeDocument.DocumentID = New Guid(cmbDocumentList.SelectedValue)
        mEmployeeDocument.DocNo = Trim(txtDocumentNo.Text)
        mEmployeeDocument.DateOfIssue = CType(txtIssueDate.Text, Object)
        mEmployeeDocument.PlaceOfIssue = Trim(txtPlaceOfIssue.Text)
        mEmployeeDocument.Validity = txtValidity.Text
        mEmployeeDocument.DateOfExpiry = CType(txtExpiryDate.Text, Object)
        mEmployeeDocument.IssuingAuthority = Trim(txtIssuingAuthority.Text)
        mEmployeeDocument.WarningDays = txtWarningDays.Text
        'mEmployeeDocument.DoneStatus = chkDoneStatus.Checked
        mEmployeeDocument.Remark = Trim(txtRemark.Text)
        mEmployeeDocument.DocumentValidityInID = CInt(cmbDocumentValidityIn.SelectedValue) 'Added By Vikrant On 11-July-2016 For ALL11072016
        mEmployeeDocument.IsApplicable = chkApplicable.Checked
        mEmployeeDocument.ApplicabilityRemark = Trim(txtApplicabilityRemark.Text)
        mEmployeeDocument.OneTimeDocument = chkOneTimeDocument.Checked 'Added by Prashant 0n 24-Nov-2020 ALL24112020
        'AttachMyFile()
    End Sub
    Private Sub AttachMyFile()
        Try
            mEmployeeDocument.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeDocument.ImageSize = Session("FileUpload.FileSize")
            mEmployeeDocument.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeDocument") = mEmployeeDocument
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForAttachment()
        Catch ex As Exception
            MSGBoxCtrl.Show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Private Sub controlVisibility()
        lblApplicabilityStar.Visible = (Not chkApplicable.Checked)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        SetTitle()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            setFocus(cmbDocumentList)

            'If mIsRenew = True Then
            '    MyFile.Value = mEmployeeDocument.FileExtension
            '    AttachMyFile()
            'Else
            If mIsRenew = True Then 'Added by Archana on Dec,09,2009 - reported bug by Pramod
                'txtValidity.Text = 0
                'mEmployeeDocument.DateOfExpiry = CType(txtIssueDate.Text, Object)
                'txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
                If chkOneTimeDocument.Checked = True Then 'Added by Prashant 0n 24-Nov-2020 ALL24112020
                    'Do nothing
                Else
                    mEmployeeDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
                    txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
                End If
                Dim fileSize1 As Integer = 0
                Dim file1(fileSize1) As Byte
                mEmployeeDocument.ImageFile = file1
                mEmployeeDocument.ImageSize = 0
                ImageButton1.Visible = False
                btnDelAttach.Enabled = False
            End If
            ControlVisibilityForAttachment()
            controlVisibility()
        End If
        'Added By Vikrant on 15-Oct-2012 For ALL11102012
        If mIsRenew = False And mEmployeeDocument.ReferenceID.Equals(Guid.Empty) Then
            cmbDocumentList.Enabled = True
        Else
            cmbDocumentList.Enabled = False
        End If
        'End
        'AttachMyFile()
        'End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeDocumentsNew") And mEmployeeDocument.IsNew) Or (Not User.IsInRole("EmployeeDocumentsEdit") And Not mEmployeeDocument.IsNew) Then
            SetObject()
            SetSession()
            'MarkLog(Flypal.Util.Action.Save, "EmployeeService", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MarkLog(Flypal.Util.Action.Save, "Employee Document", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.HandledError, mEmployeeDocument.ID, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfEmployeeDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgDocument.Click
        SetObject() 'Added Code
        NewRecordDocumentMaster()
        DataFieldBindDocumentMaster()
        mdlPopUpDocumentMaster.Show()
        upnlDocumentMaster.Update()
        'Response.Redirect("wfDocument.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeDocument_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        Session.Remove("mEmployeeDocumentList")
        Session.Remove("IsRenew") 'Added By Vikrant on 11-Oct-2012 For ALL11102012
        If Not mEmployeeDocument.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
        End If
        'Added by Vikrant on 28-nov-2013 for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        If Request.QueryString("ChildPage1") = "wfEmployeeDetails_Ajax.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Else
            Response.Redirect(Request.QueryString("BackPage") & "?ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
        End If
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployeeDocument.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeDocument.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDocument.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployeeDocument.ImageFile, 0, mEmployeeDocument.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mEmployeeDocument.ImageFile = file1
        mEmployeeDocument.ImageSize = 0
        mEmployeeDocument.FileExtension = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    'Private Sub calDateOfIssue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIssueDate.TextChanged
    '    If txtIssueDate.Text <> "" Then
    '        mEmployeeDocument.DateOfExpiry = CDate(mEmployeeDocument.DateOfIssue).AddMonths(txtValidity.Text)
    '        txtExpiryDate.Value = mEmployeeDocument.DateOfExpiry
    '    End If
    'End Sub
    Private Sub txtValidity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtValidity.TextChanged
        If txtValidity.Text <> "" Then
            If txtIssueDate.Text <> "" Then
                'mEmployeeDocument.DateOfExpiry = CDate(mEmployeeDocument.DateOfIssue).AddMonths(txtValidity.Text)
                mEmployeeDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
                txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
            End If
        Else
            txtValidity.Text = 0
            mEmployeeDocument.DateOfExpiry = txtIssueDate.Text
            txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
        End If
    End Sub
    Private Sub txtIssueDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIssueDate.TextChanged
        If IsDate(txtIssueDate.Text) Or (txtIssueDate.Text = "") Then
            If txtIssueDate.Text = "" Then
                mEmployeeDocument.DateOfIssue = System.DBNull.Value
                txtIssueDate.Text = mEmployeeDocument.DateOfIssueFormatted.ToString
                mEmployeeDocument.DateOfExpiry = System.DBNull.Value
                txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
            Else
                mEmployeeDocument.DateOfIssue = txtIssueDate.Text
                txtIssueDate.Text = mEmployeeDocument.DateOfIssueFormatted.ToString
                mEmployeeDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
                txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
            End If
        Else
            mEmployeeDocument.DateOfIssue = System.DBNull.Value
            txtIssueDate.Text = mEmployeeDocument.DateOfIssueFormatted.ToString
            mEmployeeDocument.DateOfExpiry = System.DBNull.Value
            txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
        End If

        If chkOneTimeDocument.Checked Then
            mEmployeeDocument.DateOfExpiry = System.DBNull.Value
            txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
        End If
    End Sub
    Private Sub txtExpiryDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpiryDate.TextChanged
        If IsDate(txtExpiryDate.Text) Or (txtExpiryDate.Text = "") Then
            If txtExpiryDate.Text = "" Then
                mEmployeeDocument.DateOfExpiry = System.DBNull.Value
                txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
            Else
                mEmployeeDocument.DateOfExpiry = txtExpiryDate.Text
                txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
            End If
        Else
            txtExpiryDate.Text = ""
        End If
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlDocumentDetails.Update()
    End Sub
    Private Sub btnNewDocumentMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewDocumentMaster.Click
        If txtDocumentName.Enabled = True Then
            setFocus(txtDocumentName)
        End If
        MarkLog(Flypal.Util.Action.[New], "Document", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecordDocumentMaster()
        txtDocumentName.Text = ""
        txtDocumentName.DataBind()
        DataFieldBindDocumentMaster()
        lblTitleDocumentMaster.Text = "Document Information [New]"
    End Sub
    Private Sub dgDocument_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDocument.RowCommand
        Dim mID As Guid
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgDocument.PageIndex * dgDocument.PageSize
                mID = CType(dgDocument.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                If (Not User.IsInRole("DocumentView") And Not User.IsInRole("DocumentEdit")) Then
                    SetObjectDocumentMaster()
                End If
                EditRecordDocumentMaster(mID)
                txtDocumentName.DataBind()
                MarkLog(Flypal.Util.Action.Edit, "Document", mDocument.Name, Flypal.Util.ErrorType.NoError, mDocument.ID, EventLogID)
                If Len(mDocument.Name) > 15 Then
                    lblTitleDocumentMaster.Text = "Document [" & mDocument.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleDocumentMaster.Text = "Document [" & mDocument.Name & " ]"
                End If
                If txtDocumentName.Enabled = True Then
                    setFocus(txtDocumentName)
                End If
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgDocument.PageIndex * dgDocument.PageSize
                mID = CType(dgDocument.DataKeys(CInt(e.CommandArgument)).Value, Guid)

                If (Not User.IsInRole("DocumentDelete")) Then
                    SetObjectDocumentMaster()
                End If
                DeleteRecordDocumentMaster(mID)
        End Select
    End Sub
    Private Sub btnSaveDocumentMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDocumentMaster.Click
        If (Not User.IsInRole("DocumentNew") And mDocument.IsNew) Or (Not User.IsInRole("DocumentEdit") And Not mDocument.IsNew) Then
            SetObjectDocumentMaster()
            MarkLog(Flypal.Util.Action.Save, "Document", User.Identity.Name & " is not Authorized User to save " + mDocument.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        End If
        If IsValid Then
            SetObjectDocumentMaster()
            If Not mDocument.IsValid Then Exit Sub

            Try
                mDocument.Save()
                If txtDocumentName.Enabled = True Then
                    setFocus(txtDocumentName)
                End If
                MarkLog(Flypal.Util.Action.Save, "Document", mDocument.Name, Flypal.Util.ErrorType.HandledError, mDocument.ID, EventLogID)
                NewRecordDocumentMaster()
                txtDocumentName.DataBind()
                DataFieldBindDocumentMaster()
                lblTitleDocumentMaster.Text = "Document Information [New]"
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfDocument.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfDocument.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfDocument.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfDocument.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    Private Sub btnCloseDocumentMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseDocumentMaster.Click
        Session.Remove("mDocument")
        DataFieldBind()
        controlVisibility()
        txtDocumentName.Text = ""
        mdlPopUpDocumentMaster.Hide()
        upnlDocumentDetails.Update()
    End Sub
    Private Sub cmbDocumentValidityIn_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDocumentValidityIn.SelectedIndexChanged
        If IsDate(txtIssueDate.Text) And txtIssueDate.Text <> "" And Val(txtValidity.Text) > 0 Then
            mEmployeeDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
            txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
        Else
            mEmployeeDocument.DateOfExpiry = System.DBNull.Value
            txtExpiryDate.Text = mEmployeeDocument.DateOfExpiryFormatted.ToString
        End If
    End Sub
    Private Sub chkApplicable_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkApplicable.CheckedChanged, chkOneTimeDocument.CheckedChanged
        If sender.id = "chkApplicable" Then
            controlVisibility()
        ElseIf sender.id = "chkOneTimeDocument" Then
            If chkOneTimeDocument.Checked = True Then
                lblWarningDaysStar.Visible = False
                lblDateOfExpiryStar.Visible = False
                lblValidityStar.Visible = False

                mEmployeeDocument.DateOfExpiry = System.DBNull.Value
                mEmployeeDocument.Validity = 0
                mEmployeeDocument.WarningDays = 0
                txtExpiryDate.Text = ""
                txtValidity.Text = "0"
                txtWarningDays.Text = "0"

                txtValidity.Enabled = False
                cmbDocumentValidityIn.Enabled = False
                txtExpiryDate.Enabled = False
                txtWarningDays.Enabled = False
            Else
                lblWarningDaysStar.Visible = True
                lblDateOfExpiryStar.Visible = True
                lblValidityStar.Visible = True
                txtValidity.Enabled = True
                cmbDocumentValidityIn.Enabled = True
                txtExpiryDate.Enabled = True
                txtWarningDays.Enabled = True
            End If
        End If
        upnlDocumentDetails.Update()
    End Sub

    Private Sub dgDocument_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgDocument.PageIndexChanging
        dgDocument.PageIndex = e.NewPageIndex
        dgDocument.DataSource = mDocumentList
        Session("mDocumentList") = mDocumentList
        dgDocument.DataBind()
    End Sub
#End Region





End Class
