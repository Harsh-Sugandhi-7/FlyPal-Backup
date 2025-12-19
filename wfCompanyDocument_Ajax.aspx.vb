Partial Class wfCompanyDocument_Ajax
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
    Public mCompanyDocument As CompanyDocument
    Public mCompanyDocumentList As CompanyDocumentList
    Public mDocumentList As DocumentList
    Public mIsRenew As Boolean = False
    Public BackPage As String
    Public mDocument As Document
    Public mCalibrationPeriodInList As CalibrationPeriodInList
    'Public mVendorList As VendorList
    Dim EventLogID As Guid
    Public mIssuingAuthority As IssuingAuthority
    Public mIssuingAuthorityList As IssuingAuthorityList
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mCompanyDocument = Session("mCompanyDocument")
        mCompanyDocumentList = Session("mCompanyDocumentList")
        mDocumentList = Session("mDocumentList")
        mIsRenew = Session("IsRenew")
        mDocument = Session("mDocument")
        mIssuingAuthorityList = Session("mIssuingAuthorityList")
        mIssuingAuthority = Session("mIssuingAuthority")
    End Sub
    Private Sub SetSession()
        Session("mCompanyDocument") = mCompanyDocument
        Session("mDocumentList") = mDocumentList
        Session("mIssuingAuthorityList") = mIssuingAuthorityList
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        'Validity
        'txtValidity.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtValidity').value)")
        'Warning Days
        txtWarningDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtWarningDays').value)")
    End Sub
    Private Sub DataFieldBind()
        mDocumentList = DocumentList.GetDocumentList(, "(SELECT)")
        cmbDocumentList.DataSource = mDocumentList
        Session("mDocumentList") = mDocumentList

        mIssuingAuthorityList = IssuingAuthorityList.GetIssuingAuthorityList(AddTopItem:="(SELECT)")
        cmbIssuingAuthority.DataSource = mIssuingAuthorityList
        Session("mIssuingAuthorityList") = mIssuingAuthorityList

        txtIssueDate.Text = mCompanyDocument.DateOfIssueFormatted.ToString
        txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString

        'mCalibrationPeriodInList = CalibrationPeriodInList.GetCalibrationPeriodInList()
        'Session("mCalibrationPeriodInList") = mCalibrationPeriodInList
        'cmbDocumentValidityIn.DataSource = mCalibrationPeriodInList

        'mVendorList = VendorList.GetVendortList(0, , , , , , True, True, True, True)
        'cmbVendorList.DataSource = mVendorList

        upnlDocumentDetails.DataBind()
    End Sub
    Public Sub Save()
        SetObject()
        If Not mCompanyDocument.IsValid Then Exit Sub
        Try
            If (mCompanyDocumentList.Contains(mCompanyDocument.DocumentID, mCompanyDocument.ID, mIsRenew.ToString)) Then
                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry. ", MsgBoxStyle.OkOnly, "")
            Else
                mCompanyDocument.Save()
                SetSession()
                lblTitle.Text = "Organisation Approval Information [New]"
                MarkLog(Flypal.Util.Action.Save, "CompanyDocument", "Doc No. : " + mCompanyDocument.DocNo + " Document : " + mCompanyDocument.DocumentName, Flypal.Util.ErrorType.NoError, mCompanyDocument.ID, EventLogID)
                Session.Remove("mCompanyDocumentList")
                Session.Remove("mIsRenew")
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                'If Request.QueryString("ChildPage1") = "wfCompanyDetails.aspx" Then
                '    Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                'Else
                '    Response.Redirect(Request.QueryString("BackPage") & "?ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                'End If
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 50000 Then
                MSGBoxCtrl.Show("Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mCompanyDocument.ImageSize > 0 Then
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
    Private Sub NewRecordIssuingAuthorityMaster()
        mIssuingAuthority = IssuingAuthority.NewIssuingAuthority
        Session("mIssuingAuthority") = mIssuingAuthority
    End Sub
    Private Sub EditRecordIssuingAuthorityMaster(ByVal mID As Guid)
        mIssuingAuthority = IssuingAuthority.GetIssuingAuthority(mID)
        Session("mIssuingAuthority") = mIssuingAuthority
    End Sub
    Private Sub DeleteRecordIssuingAuthorityMaster(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteIssuingAuthorityMaster")
        mIssuingAuthority = IssuingAuthority.GetIssuingAuthority(mID)
        Session("mIssuingAuthority") = mIssuingAuthority
    End Sub
    Private Sub SetObjectIssuingAuthorityMaster()
        mIssuingAuthority.Name = txtIssuingAuthorityName.Text
    End Sub
    Private Sub DataFieldBindIssuingAuthorityMaster()
        mIssuingAuthorityList = IssuingAuthorityList.GetIssuingAuthorityList()
        dgIssuingAuthority.DataSource = mIssuingAuthorityList
        Session("mIssuingAuthorityList") = mIssuingAuthorityList
        dgIssuingAuthority.DataBind()
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
                    If MSGBoxCtrl.Sender = "DeleteIssuingAuthorityMaster" Then
                        Try
                            Session("sender") = ""
                            mIssuingAuthority = Session("mIssuingAuthority")
                            IssuingAuthority.DeleteIssuingAuthority(mIssuingAuthority.ID)
                            NewRecordIssuingAuthorityMaster()
                            DataFieldBindIssuingAuthorityMaster()
                            txtIssuingAuthorityName.Text = ""
                            txtIssuingAuthorityName.DataBind()
                            lblTitleIssuingAuthorityMaster.Text = "Issuing Authority Information [New]"
                            upnlIssuingAuthorityMaster.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "IssuingAuthority", "Can't delete : " + mIssuingAuthority.Name + "  is Currently in use", Flypal.Util.ErrorType.NoError, mIssuingAuthority.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordIssuingAuthorityMaster()
                            DataFieldBindIssuingAuthorityMaster()
                            txtIssuingAuthorityName.Text = ""
                            txtIssuingAuthorityName.DataBind()
                            lblTitleIssuingAuthorityMaster.Text = "Issuing Authority Information [New]"
                            upnlIssuingAuthorityMaster.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "IssuingAuthority", mIssuingAuthority.Name, Flypal.Util.ErrorType.NoError, mIssuingAuthority.ID, EventLogID)
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
                    If MSGBoxCtrl.Sender = "DeleteIssuingAuthorityMaster" Then
                        NewRecordIssuingAuthorityMaster()
                        txtIssuingAuthorityName.DataBind()
                        lblTitleIssuingAuthorityMaster.Text = "Issuing Authority Information [New]"
                        dgIssuingAuthority.DataSource = mIssuingAuthorityList
                        upnlIssuingAuthorityMaster.DataBind()
                        upnlIssuingAuthorityMaster.Update()
                    End If
                    Session("sender") = ""
                    'Response.Redirect("wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'DataFieldBind()
            'Response.Redirect("wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mCompanyDocument.IsNew Then
            lblTitle.Text = "Organisation Approval Information [New]"
        Else
            If Len(mCompanyDocument.DocumentName) > 15 Then
                lblTitle.Text = "Organisation Approval Information [" & mCompanyDocument.DocumentName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Organisation Approval Information [" & mCompanyDocument.DocumentName & "]"
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
    Public Sub Customvalidate2(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "txtIssuingAuthorityName" Then
            If Len(Trim(txtDocumentName.Text)) > 200 Then
                CustValid.ErrorMessage = " Issuing Authority Name too long "
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
        ElseIf CustValid.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 255 Then
                CustValid.ErrorMessage = "Remark should not be more than 255 characters."
                e.IsValid = False
            End If
            'ElseIf CustValid.ControlToValidate = "txtWarningDays" Then
            '    Dim ValidityInDays As String
            '    ValidityInDays = IIf(cmbDocumentValidityIn.SelectedValue = 1, txtValidity.Text, IIf(cmbDocumentValidityIn.SelectedValue = 2, (Val(txtValidity.Text) * 30).ToString, (Val(txtValidity.Text) * 365).ToString))
            '    If Val(txtWarningDays.Text) <= 0 Then
            '        CustValid.ErrorMessage = "Warning days required."
            '        e.IsValid = False
            '    ElseIf Val(ValidityInDays) <= Val(txtWarningDays.Text) Then
            '        CustValid.ErrorMessage = "Warning days should be less than Validity Period."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        End If
    End Sub
    Private Sub SetObject()
        mCompanyDocument.DocumentOrContractID = 1
        'mCompanyDocument.VendorID = New Guid(cmbVendorList.SelectedValue)
        mCompanyDocument.DocumentID = New Guid(cmbDocumentList.SelectedValue)
        mCompanyDocument.DocNo = Trim(txtDocumentNo.Text)
        'mCompanyDocument.DateOfSigning = CType(txtDateOfSigning.Text, Object)
        mCompanyDocument.DateOfIssue = CType(txtIssueDate.Text, Object)
        'mCompanyDocument.PlaceOfIssue = Trim(txtPlaceOfIssue.Text)
        'mCompanyDocument.Validity = txtValidity.Text
        mCompanyDocument.DateOfExpiry = CType(txtExpiryDate.Text, Object)
        mCompanyDocument.IssuingAuthorityID = New Guid(cmbIssuingAuthority.SelectedValue)
        If cmbIssuingAuthority.SelectedIndex > 0 Then
            mCompanyDocument.IssuingAuthority = cmbIssuingAuthority.SelectedItem.ToString
        Else
            mCompanyDocument.IssuingAuthority = ""
        End If
        mCompanyDocument.WarningDays = txtWarningDays.Text
        'mCompanyDocument.DoneStatus = chkDoneStatus.Checked
        mCompanyDocument.Remark = Trim(txtRemark.Text)
        'mCompanyDocument.DocumentValidityInID = CInt(cmbDocumentValidityIn.SelectedValue)
        'AttachMyFile()
    End Sub
    Private Sub AttachMyFile()
        Try
            mCompanyDocument.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mCompanyDocument.ImageSize = Session("FileUpload.FileSize")
            mCompanyDocument.FileExtension = Session("FileUpload.FileExtension")
            Session("mCompanyDocument") = mCompanyDocument
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Private Sub DisableName(mID As Guid) 'Added by : Prashant 09-Jul-2020, MRO09072020

        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerDocument(mID)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtDocumentName.Enabled = mTransCountAsPerMasters.Count = 0
        End If

    End Sub
    Private Sub DisableIssuingAuthorityName(mID As Guid)

        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerDocument(mID)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtIssuingAuthorityName.Enabled = mTransCountAsPerMasters.Count = 0
        End If

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
            '    MyFile.Value = mCompanyDocument.FileExtension
            '    AttachMyFile()
            'Else
            If mIsRenew = True Then
                'txtValidity.Text = 0
                'mCompanyDocument.DateOfExpiry = CType(txtIssueDate.Text, Object)
                'txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
                'mCompanyDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
                cmbDocumentList.Enabled = False
                txtDocumentNo.Enabled = False
                imgDocument.Visible = False
                txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString

                Dim fileSize1 As Integer = 0
                Dim file1(fileSize1) As Byte
                mCompanyDocument.ImageFile = file1
                mCompanyDocument.ImageSize = 0
                ImageButton1.Visible = False
                btnDelAttach.Enabled = False
                cmbIssuingAuthority.Enabled = False
                imgIssuingAuthority.Visible = False
            End If
            ControlVisibilityForAttachment()
        End If
        'AttachMyFile()
        'End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If (Not User.IsInRole("CompanyNew") And mCompanyDocument.IsNew) Or (Not User.IsInRole("CompanyEdit") And Not mCompanyDocument.IsNew) Then
        '    SetObject()
        '    SetSession()
        '    'MarkLog(Flypal.Util.Action.Save, "CompanyService", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        '    'MarkLog(Flypal.Util.Action.Save, "CompanyDocument", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mCompany.EmpNoName + " Document : " + mCompanyDocument.DocumentName, Flypal.Util.ErrorType.HandledError, mCompanyDocument.ID, EventLogID)
        '    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    'msg.ReplacePage = "wfCompanyDocument_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
        '    'Session("sender") = "Authorization"
        '    'msg.Show()
        '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
        '    Exit Sub
        'End If
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
        'Response.Redirect("wfDocument.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfCompanyDocument_Ajax.aspx")
    End Sub
    Private Sub imgIssuingAuthority_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgIssuingAuthority.Click
        SetObject() 'Added Code
        NewRecordIssuingAuthorityMaster()
        DataFieldBindIssuingAuthorityMaster()
        mdlPopUpIssuingAuthorityMaster.Show()
        upnlIssuingAuthorityMaster.Update()
        'Response.Redirect("wfDocument.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfCompanyDocument_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        Session.Remove("mCompanyDocumentList")
        Session.Remove("IsRenew") 'Added By Vikrant on 11-Oct-2012 For ALL11102012
        If Not mCompanyDocument.IsNew Then
            'MarkLog(Flypal.Util.Action.Close, "CompanyDocument", "Emp : " + mCompany.EmpNoName + " Document : " + mCompanyDocument.DocumentName, Flypal.Util.ErrorType.NoError, mCompanyDocument.ID, EventLogID)
        End If
        'Added by Vikrant on 28-nov-2013 for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        If Request.QueryString("ChildPage1") = "wfCompanyDetails_Ajax.aspx" Then
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
        If mCompanyDocument.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mCompanyDocument.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mCompanyDocument.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mCompanyDocument.ImageFile, 0, mCompanyDocument.ImageFile.Length)
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
        mCompanyDocument.ImageFile = file1
        mCompanyDocument.ImageSize = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    'Private Sub calDateOfIssue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIssueDate.TextChanged
    '    If txtIssueDate.Text <> "" Then
    '        mCompanyDocument.DateOfExpiry = CDate(mCompanyDocument.DateOfIssue).AddMonths(txtValidity.Text)
    '        txtExpiryDate.Value = mCompanyDocument.DateOfExpiry
    '    End If
    'End Sub
    'Private Sub txtValidity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtValidity.TextChanged
    '    If txtValidity.Text <> "" Then
    '        If txtIssueDate.Text <> "" Then
    '            'mCompanyDocument.DateOfExpiry = CDate(mCompanyDocument.DateOfIssue).AddMonths(txtValidity.Text)
    '            mCompanyDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
    '            txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '        End If
    '    Else
    '        txtValidity.Text = 0
    '        mCompanyDocument.DateOfExpiry = txtIssueDate.Text
    '        txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '    End If
    'End Sub
    'Private Sub txtIssueDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIssueDate.TextChanged
    '    If IsDate(txtIssueDate.Text) Or (txtIssueDate.Text = "") Then
    '        If txtIssueDate.Text = "" Then
    '            mCompanyDocument.DateOfIssue = System.DBNull.Value
    '            txtIssueDate.Text = mCompanyDocument.DateOfIssueFormatted.ToString
    '            mCompanyDocument.DateOfExpiry = System.DBNull.Value
    '            txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '        Else
    '            mCompanyDocument.DateOfIssue = txtIssueDate.Text
    '            txtIssueDate.Text = mCompanyDocument.DateOfIssueFormatted.ToString
    '            mCompanyDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
    '            txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '        End If
    '    Else
    '        mCompanyDocument.DateOfIssue = System.DBNull.Value
    '        txtIssueDate.Text = mCompanyDocument.DateOfIssueFormatted.ToString
    '        mCompanyDocument.DateOfExpiry = System.DBNull.Value
    '        txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '    End If
    'End Sub
    'Private Sub txtExpiryDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpiryDate.TextChanged
    '    If IsDate(txtExpiryDate.Text) Or (txtExpiryDate.Text = "") Then
    '        If txtExpiryDate.Text = "" Then
    '            mCompanyDocument.DateOfExpiry = System.DBNull.Value
    '            txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '        Else
    '            mCompanyDocument.DateOfExpiry = txtExpiryDate.Text
    '            txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '        End If
    '    Else
    '        txtExpiryDate.Text = ""
    '    End If
    'End Sub
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
        txtDocumentName.Enabled = True
        txtDocumentName.DataBind()
        DataFieldBindDocumentMaster()
        lblTitleDocumentMaster.Text = "Document Information [New]"
    End Sub
    Private Sub btnNewIssuingAuthorityMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewIssuingAuthorityMaster.Click
        If txtIssuingAuthorityName.Enabled = True Then
            setFocus(txtIssuingAuthorityName)
        End If
        MarkLog(Flypal.Util.Action.[New], "IssuingAuthority", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecordIssuingAuthorityMaster()
        txtIssuingAuthorityName.Text = ""
        txtIssuingAuthorityName.Enabled = True
        txtIssuingAuthorityName.DataBind()
        DataFieldBindIssuingAuthorityMaster()
        lblTitleIssuingAuthorityMaster.Text = "Issuing Authority Information [New]"
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
                DisableName(mID)
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
    Private Sub dgIssuingAuthority_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssuingAuthority.RowCommand
        Dim mID As Guid
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgIssuingAuthority.PageIndex * dgIssuingAuthority.PageSize
                mID = CType(dgIssuingAuthority.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                'If (Not User.IsInRole("DocumentView") And Not User.IsInRole("DocumentEdit")) Then
                '    SetObjectDocumentMaster()
                'End If
                EditRecordIssuingAuthorityMaster(mID)
                DisableIssuingAuthorityName(mID)
                txtIssuingAuthorityName.DataBind()
                MarkLog(Flypal.Util.Action.Edit, "IssuingAuthority", mIssuingAuthority.Name, Flypal.Util.ErrorType.NoError, mIssuingAuthority.ID, EventLogID)
                If Len(mIssuingAuthority.Name) > 15 Then
                    lblTitleIssuingAuthorityMaster.Text = "Issuing Authority [" & mIssuingAuthority.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleIssuingAuthorityMaster.Text = "Issuing Authority [" & mIssuingAuthority.Name & " ]"
                End If
                If txtIssuingAuthorityName.Enabled = True Then
                    setFocus(txtIssuingAuthorityName)
                End If
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgIssuingAuthority.PageIndex * dgIssuingAuthority.PageSize
                mID = CType(dgIssuingAuthority.DataKeys(CInt(e.CommandArgument)).Value, Guid)

                'If (Not User.IsInRole("DocumentDelete")) Then
                '    SetObjectDocumentMaster()
                'End If
                DeleteRecordIssuingAuthorityMaster(mID)
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
                txtDocumentName.Enabled = True
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
        mdlPopUpDocumentMaster.Hide()
        upnlDocumentDetails.Update()
    End Sub
    Private Sub btnSaveIssuingAuthorityMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveIssuingAuthorityMaster.Click
        If IsValid Then
            SetObjectIssuingAuthorityMaster()
            If Not mIssuingAuthority.IsValid Then Exit Sub

            Try
                mIssuingAuthority.Save()
                If txtIssuingAuthorityName.Enabled = True Then
                    txtIssuingAuthorityName.Focus()
                End If
                MarkLog(Flypal.Util.Action.Save, "IssuingAuthority", mIssuingAuthority.Name, Flypal.Util.ErrorType.HandledError, mIssuingAuthority.ID, EventLogID)
                NewRecordIssuingAuthorityMaster()
                txtIssuingAuthorityName.Enabled = True
                txtIssuingAuthorityName.DataBind()
                DataFieldBindIssuingAuthorityMaster()
                lblTitleIssuingAuthorityMaster.Text = "Issuing Authority Information [New]"
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    Private Sub btnCloseIssuingAuthorityMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseIssuingAuthorityMaster.Click
        Session.Remove("mIssuingAuthority")
        DataFieldBind()
        mdlPopUpIssuingAuthorityMaster.Hide()
        upnlDocumentDetails.Update()
    End Sub
    'Private Sub cmbDocumentValidityIn_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDocumentValidityIn.SelectedIndexChanged
    '    If IsDate(txtIssueDate.Text) And txtIssueDate.Text <> "" And Val(txtValidity.Text) > 0 Then
    '        mCompanyDocument.DateOfExpiry = IIf(cmbDocumentValidityIn.SelectedValue = 1, CDate(CType(txtIssueDate.Text, Object)).AddDays(txtValidity.Text), IIf(cmbDocumentValidityIn.SelectedValue = 2, CDate(CType(txtIssueDate.Text, Object)).AddMonths(txtValidity.Text), CDate(CType(txtIssueDate.Text, Object)).AddYears(txtValidity.Text)))
    '        txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '    Else
    '        mCompanyDocument.DateOfExpiry = System.DBNull.Value
    '        txtExpiryDate.Text = mCompanyDocument.DateOfExpiryFormatted.ToString
    '    End If
    'End Sub
#End Region

End Class
