Imports System.IO.FileStream
Imports System.Web.UI.HtmlControls.HtmlGenericControl
Partial Class wfAttachFiles
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable declaration"
    Public mAttachFileDetail As AttachFileDetail
    Public mAttachFiles As AttachFiles
    Public mAttachFileDetailList As AttachFileDetailList
    Public mAttachFile As AttachFile
    Public mDocumentTypeList As DocumentTypeList
    Public mCurrentControl As Control

    Public mAttachFileDetail1 As AttachFileDetail

    'Common
    Public mAttachToID As Guid
    Public mName As String
    Public mPath As String
    Public IsSelected As Boolean = False
    Public CurrentRowIndex As Integer = 0
    Public mCurrentRow As Integer
    Public mDocumentTypeForID As Integer

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDocumentTypeList = DocumentTypeList.GetDocumentTypeList(, mDocumentTypeForID, "<SELECT>")
        Session("mDocumentTypeList") = mDocumentTypeList
        cmbDocumentType.DataSource = mDocumentTypeList
        DataBind()
    End Sub
#End Region

#Region " Business Methods  "
    Private Sub GetSession()
        mAttachFileDetail = CType(Session("mAttachFileDetail"), AttachFileDetail)
        mAttachFileDetail1 = CType(Session("mAttachFileDetail1"), AttachFileDetail)
        mAttachFiles = CType(Session("mAttachFiles"), AttachFiles)
        mAttachFile = CType(Session("mAttachFile"), AttachFile)
        mDocumentTypeList = CType(Session("mDocumentTypeList"), DocumentTypeList)
        mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
        mAttachToID = CType(Session("mAttachToID"), Guid)
        mName = CType(Session("mName"), String)
        mPath = CType(Session("mPath"), String)
    End Sub
    Private Sub SetSession()
        Session("mAttachFileDetail") = mAttachFileDetail
        Session("mAttachFileDetail1") = mAttachFileDetail1
        Session("mAttachFiles") = mAttachFiles
        Session("mAttachFile") = mAttachFile
        Session("mDocumentTypeList") = mDocumentTypeList
        Session("mDocumentTypeForID") = mDocumentTypeForID
        Session("mAttachToID") = mAttachToID
        Session("mName") = mName
        Session("mPath") = mPath
    End Sub
    Private Sub NewRecord()
        mAttachToID = CType(Session("mAttachToID"), Guid)
        mAttachFileDetail = AttachFileDetail.NewAttachFileDetail(mAttachToID)
        Session("mAttachFileDetail") = mAttachFileDetail
        EnableDisableButtons()
        'MarkLog(Util.Action.[New], "AttachFileDetail", "", Util.ErrorType.NoError, mAttachFileDetail.ID)
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
    End Sub
    Private Sub setObject(Optional ByVal BackupPath As String = "")
        mAttachFileDetail = CType(Session("mAttachFileDetail"), AttachFileDetail)

        mAttachFileDetail.Name = Trim(txtName.Text)

        If BackupPath = "" Then
            If txtPath.Text <> "" Then
                BackupPath = txtPath.Text
            Else
                BackupPath = MyFile.Value
            End If
        End If
        If txtPath.Text <> "" Then
            mAttachFileDetail.Path = BackupPath
        ElseIf MyFile.Value <> "" Then
            mAttachFileDetail.Path = BackupPath
            txtPath.Text = BackupPath
        End If
        If (cmbDocumentType.SelectedValue.ToString) <> "" Then
            mAttachFileDetail.DocumentTypeID = New Guid(cmbDocumentType.SelectedValue.ToString)
        End If
        mAttachFileDetail.Remark = Trim(txtRemark.Text)
        Session("mAttachFileDetail") = mAttachFileDetail
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub EnableDisableButtons()
        If (mAttachFileDetail.IsNew) Then
            btnAttach.Enabled = True
            cmbAttach.Enabled = True
            txtName.Enabled = True
            txtPath.Visible = True
            MyFile.Visible = False
            btnAttach.Visible = True
        Else
            btnAttach.Enabled = False
            cmbAttach.Enabled = False
            txtName.Enabled = False
            MyFile.Visible = False
        End If
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mAttachFileDetail = AttachFileDetail.GetAttachFileDetail(mId)
        Session("mAttachFileDetail") = mAttachFileDetail
        txtName.DataBind()
        cmbDocumentType.DataBind()
        txtRemark.DataBind()
        EnableDisableButtons()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbDocumentType" Then
            If cmbDocumentType.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Document Type from the list."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub Save()
        Try
            Dim BackupPath As String = ""
            Dim RefPath As String = ""
            Dim isNew As Boolean = False
            mAttachFileDetail = CType(Session("mAttachFileDetail"), AttachFileDetail)
            If mAttachFileDetail.IsNew Then
                If txtPath.Text <> "" Then
                    RefPath = txtPath.Text
                    'ElseIf MyFile.Value <> "" Then
                    '    RefPath = MyFile.Value
                    isNew = True 'To set variable
                End If
                'BackupPath = AppSettings("DOCPath") & mAttachFileDetail.Name & ".PDF"
                BackupPath = AppSettings("DOCPath") & mAttachFileDetail.ID.ToString & ".PDF"
                If (txtPath.Text <> "") Or (MyFile.Value <> "") Then mAttachFileDetail.Path = BackupPath
            End If
            setObject(BackupPath)
            mAttachFileDetail = CType(mAttachFileDetail.Save(), AttachFileDetail)
            'MarkLog(Util.Action.Save, "AttachFileDetail", mAttachFileDetail.Name, Util.ErrorType.NoError, mAttachFileDetail.ID)
            FindNow()
            NewRecord()
            DataFieldBind()
            SetSession()
            EnableDisableButtons()
            SetTitle()
            If cmbAttach.SelectedIndex = 0 Then
                If isNew And RefPath <> "" Then FileCopy(RefPath, BackupPath)
            ElseIf cmbAttach.SelectedIndex = 1 Then
                ' Let us recover only the file name from its fully qualified path at client 
                ' ''Dim strFileName As String
                ' ''strFileName = MyFile.PostedFile.FileName
                Try
                    If MyFile.Value <> "" Then
                        MyFile.PostedFile.SaveAs(BackupPath)
                        cmbAttach.SelectedIndex = 0
                    End If
                Catch Exp As Exception
                End Try
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                msg1.Show()
            ElseIf ex.Number = 2627 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.AttachmentAlert, SIMsgBox.Message_text.AttachmentAlert, "", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                msg1.Show()
            ElseIf ex.Number = 547 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                'MarkLog(Util.Action.Delete, "AttachFileDetail", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mAttachFileDetail.ID)
                msg1.Show()
            End If
        End Try
    End Sub
    Private Sub FindNow()
        GetSession()
        mAttachFiles = AttachFiles.GetAttachFiles(mAttachToID, Guid.Empty)
        'Set DataSource of the Grid
        Me.dgAttachFileList.DataSource = mAttachFiles
        dgAttachFileList.DataBind()
        lblResult.Text = "Attach File List: " & mAttachFiles.Count & " record(s) found."
        'Set Buttons.
        EnableDisableButtons()
        Session("mAttachFiles") = mAttachFiles
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfAttachFiles.aspx?BackPage=" & Session("BackPage") & "&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        Session("sender") = "Delete"
        msg1.Show()
        mAttachFile = AttachFile.GetAttachFile(mAttachToID, mID)
        Session("mAttachFile") = mAttachFile
    End Sub
    Private Sub ClearAll()
        Session.Remove("mAttachFileDetail")
        Session.Remove("mAttachFileDetail1")
        Session.Remove("mAttachFiles")
        Session.Remove("mAttachFile")
        Session.Remove("mDocumentTypeList")
        Session.Remove("mDocumentTypeForID")
        Session.Remove("mAttachToID")
        Session.Remove("BackPage")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Session("sender") = ""
                            mAttachFile = CType(Session("mAttachFile"), AttachFile)
                            AttachFile.DeleteAttachFile(mAttachFile.ID)
                            Response.Redirect("wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                'msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&BackPage=" & Session("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Request.QueryString("Type") & "&ChildPage=" & Request.QueryString("ChildPage")
                                msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                                'MarkLog(Util.Action.Delete, "AttachFileDetail", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mAttachFileDetail.ID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "AttachFileDetail", mAttachFileDetail.Name, Util.ErrorType.NoError, mAttachFileDetail.ID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfAttachFiles.aspx?MsgResult=0&BackPage=" & Session("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Request.QueryString("Type") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Response.Redirect("wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub ExistingFileSelected()

        mAttachFileDetail1 = CType(Session("mAttachFileDetail1"), AttachFileDetail)
        mAttachFiles = CType(Session("mAttachFiles"), AttachFiles)
        IsSelected = False
        Session("IsSelected") = IsSelected
        If (Not mAttachFileDetail1 Is Nothing) Then
            If (Not mAttachFiles.Contains(mAttachFileDetail1.ID)) Then
                mAttachFileDetail = AttachFileDetail.NewAttachFileDetail(mAttachToID, mAttachFileDetail1)
                Session("mAttachFileDetail") = mAttachFileDetail
                Session("mAttachFileDetail1") = mAttachFileDetail1
                FindNow()
                DataFieldBind()
                EnableDisableButtons()
            Else
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.AttachmentAlert, SIMsgBox.Message_text.AttachmentAlert, "Please select another File", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                FindNow()
                DataFieldBind()
                EnableDisableButtons()
                msg1.Show()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub SetTitle()
        mName = CType(Session("mName"), String)
        If mAttachFileDetail.IsNew Then
            lbltitle.Text = "Attach Files [" & mName & "]"
        Else
            If Len(mAttachFileDetail.Name) > 15 Then
                lbltitle.Text = "Attach Files [" & mName & "]"
            Else
                lbltitle.Text = "Attach Files [" & mName & "]"
            End If
        End If
        lblResult.Text = "Attach Files List: " & mAttachFiles.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Request.QueryString("BackPage") Is Nothing AndAlso Request.QueryString("BackPage").ToString.Length > 0 Then
            Session("BackPage") = Request.QueryString("BackPage").ToString.Replace(",", "")
        Else
            Session("BackPage") = Request.QueryString("BackPage")
        End If

        GetSession()
        IsSelected = CType(Session("IsSelected"), Boolean)
        '-----------------------------------------------------------
        Dim MyPath, MyName As String
        MyPath = "C:\Temp"                  ' Set the path.
        MyName = Dir(MyPath, vbDirectory)   ' Retrieve the first entry.
        If MyName = "" Then                 ' The folder is not there & to be created
            MkDir("C:\Temp\")               ' Folder created
        End If
        '-----------------------------------------------------------
        If Not IsPostBack And Session("sender") = "" And IsSelected = False Then
            MyFile.Visible = False
            'If IsNothing(Request.QueryString("BackPage1")) Or Request.QueryString("BackPage1") = "" Then
            '    Session("MiddleFrame") = "wfAttachFiles.aspx?MainBackPage=" & Request.QueryString("MainBackPage")
            'End If
            NewRecord()
            FindNow()
            DataFieldBind()
            EnableDisableButtons()
        ElseIf CType(Session("sender"), String) = "Existing" And IsSelected = False Then
            FindNow()
            DataFieldBind()
            EnableDisableButtons()
            Session("sender") = ""
        ElseIf IsSelected = True Then
            ExistingFileSelected()
        End If
        SetTitle()
        MessageBoxResult()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        FindNow()
        DataFieldBind()
        SetTitle()
        cmbAttach.SelectedIndex = 0
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
    End Sub
    Private Sub imgbtnDocumentType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnDocumentType.Click
        mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
        setObject()
        Session("mDocumentTypeForID") = mDocumentTypeForID
        'Dim str As String
        ' str = "<script language='javascript'>openledgersame('wfDocumentType.aspx?BackPage1=wfAttachFiles.aspx&MainBackPage=" & Request.QueryString("MainBackPage") & "');</script>"
        '  ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", str)
        Response.Redirect("wfDocumentType.aspx?BackPage1=wfAttachFiles.aspx&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            Save()
        End If
    End Sub
    Private Sub btnAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttach.Click
        setObject()
        If cmbAttach.SelectedIndex = 0 Then
            mDocumentTypeForID = CType(Session("mDocumentTypeForID"), Integer)
            SetSession()
            'Dim str As String
            'str = "<script language='javascript'>openledgersame('wfAttachFileDetailList.aspx?BackPage2=wfAttachFiles.aspx&MainBackPage=" & Request.QueryString("MainBackPage") & "');</script>"
            ' ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", str)
            Response.Redirect("wfAttachFileDetailList.aspx?BackPage2=wfAttachFiles.aspx&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
        ElseIf cmbAttach.SelectedIndex = 1 Then
            If MyFile.Value <> "" And txtPath.Text = "" Then
                txtPath.Text = MyFile.Value
                mAttachFileDetail.Path = MyFile.Value
            End If
        End If
    End Sub
    Private Sub dgAttachFileList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAttachFileList.ItemCommand

        Dim mID As New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "Edit"
                ' ''If (Not User.IsInRole("AttachFileDetailView") And Not User.IsInRole("AttachFileDetailEdit")) Then
                ' ''    setObject()
                ' ''    SetSession()
                ' ''    MarkLog(Util.Action.Edit, "AttachFileDetail", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ' ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ' ''    msg.ReplacePage = "wfAttachFiles.aspx?MainBackPage=" & Request.QueryString("MainBackPage")
                ' ''    Session("sender") = "Authorization"
                ' ''    msg.Show()
                ' ''    Exit Sub
                ' ''End If
                EditRecord(mID)
                'MarkLog(Util.Action.Edit, "AttachFileDetail", mAttachFileDetail.Name, Util.ErrorType.NoError, mAttachFileDetail.ID)
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
            Case "Delete"
                ' ''If (Not User.IsInRole("AttachFileDetailDelete")) Then
                ' ''    setObject()
                ' ''    SetSession()
                ' ''    MarkLog(Util.Action.Delete, "AttachFileDetail", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ' ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ' ''    msg.ReplacePage = "wfAttachFiles.aspx?MainBackPage=" & Request.QueryString("MainBackPage")
                ' ''    Session("sender") = "Authorization"
                ' ''    msg.Show()
                ' ''    Exit Sub
                ' ''End If
                DeleteRecord(mID)

            Case "View"
                mAttachFileDetail = AttachFileDetail.GetAttachFileDetail(mID)
                Session("FilePath") = AppSettings("DOCPath") & mAttachFileDetail.ID.ToString & ".PDF"
                If mAttachFileDetail.Path <> "" Then
                    Dim Str As String
                    Str = "<script language=Javascript>openFile();</script>"
                    ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)
                Else
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoAttachmentAlert, SIMsgBox.Message_text.NoAttachmentAlert, "", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfAttachFiles.aspx?MsgResult=0&MainBackPage=" & Request.QueryString("MainBackPage") & "&BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                    FindNow()
                    DataFieldBind()
                    EnableDisableButtons()
                    msg1.Show()
                    Exit Sub
                End If
        End Select
    End Sub
    Private Sub cmbAttach_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAttach.SelectedIndexChanged
        setObject()
        If cmbAttach.SelectedIndex = 0 Then
            txtPath.Visible = True
            MyFile.Visible = False
            btnAttach.Visible = True
        Else
            txtPath.Visible = False
            MyFile.Visible = True
            btnAttach.Visible = False
        End If
        If cmbAttach.Enabled = True Then
            setFocus(cmbAttach)
        End If
        'DataFieldBind()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ClearAll()
        'MarkLog(Util.Action.Close, "AttachFiles", "", Util.ErrorType.NoError, Guid.Empty)
        Session("sender") = ""
        If Session("NewPage") = "True" Then
            Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
        Else
            Response.Redirect(Request.QueryString("MainBackPage") & "?BackPage=" & Session("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
        End If

    End Sub
#End Region


End Class
