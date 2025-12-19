
Partial Class wfEquipment
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

#Region " Variable Declaration "
    Protected mEquipment As Equipment
    Protected mEquipmentList As EquipmentList
    Public BackPage As String
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mEquipment = Session("mEquipment")
        mEquipmentList = Session("mEquipmentList")
    End Sub
    Private Sub SetSession()
        Session("mEquipment") = mEquipment
        Session("mEquipmentList") = mEquipmentList
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub NewRecord()
        mEquipment = Equipment.NewEquipment
        Session("mEquipment") = mEquipment
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mEquipment = Equipment.GetEquipment(mID)
        Session("mEquipment") = mEquipment
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
        Session("sender") = "Delete"
        msg1.Show()
        mEquipment = Equipment.GetEquipment(mID)
        Session("mEquipment") = mEquipment
    End Sub
    Private Sub SetObject()
        mEquipment.Name = Trim(txtEquipment.Text)
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
                            mEquipment = Session("mEquipment")
                            Equipment.DeleteEquipment(mEquipment.ID)
                            Response.Redirect("wfEquipment.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                                MarkLog(Flypal.Util.Action.Delete, "Equipment", "Can't delete : " + mEquipment.Name + "  is Currently in use", Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Equipment", mEquipment.Name, Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfEquipment.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                Case MsgBoxResult.OK
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfEquipment.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfEquipment.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            Response.Redirect("wfEquipment.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind()
        mEquipmentList = EquipmentList.GetEquipmentList()
        dgEquipmentList.DataSource = mEquipmentList
        Session("mEquipmentList") = mEquipmentList
        dgEquipmentList.DataBind()
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtEquipment" Then
            If Len(Trim(txtEquipment.Text)) = 0 Then
                CustValid.ErrorMessage = " Equipment name required. "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If txtEquipment.Enabled = True Then
            setFocus(txtEquipment)
        End If
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then

            BackPage = Request.QueryString("Backpage")
            Session("BackPage") = BackPage
            NewRecord()
            Session("mEquipment") = mEquipment
            DataFieldBind()
        Else
            dgEquipmentList.DataSource = mEquipmentList
            dgEquipmentList.DataBind()
        End If
        If mEquipmentList.Count > 25 Then
            btnBackTop.Visible = True
        Else
            btnBackTop.Visible = False
        End If
        MessageBoxResult()
        SetSession()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If txtEquipment.Enabled = True Then
            setFocus(txtEquipment)
        End If
        MarkLog(Flypal.Util.Action.[New], "Equipment", "", Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
        NewRecord()
        txtEquipment.Text = ""
        DataFieldBind()
        lbltitle.Text = "Equipment [New]"
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        MarkLog(Flypal.Util.Action.Close, "Equipment", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("mEquipmentList") = mEquipmentList
        Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeNew") And mEquipment.IsNew) Or (Not User.IsInRole("EmployeeEdit") And Not mEquipment.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "Equipment", User.Identity.Name & " is not Authorized User to save " + mEquipment.Name, Flypal.Util.ErrorType.HandledError, mEquipment.ID, EventLogID)
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfEquipment.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
            Session("sender") = "Authorization"
            msg.Show()
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                mEquipment.Save()
                If txtEquipment.Enabled = True Then
                    setFocus(txtEquipment)
                End If
                MarkLog(Flypal.Util.Action.Save, "Equipment", mEquipment.Name, Flypal.Util.ErrorType.HandledError, mEquipment.ID, EventLogID)
                NewRecord()
                txtEquipment.DataBind()
                DataFieldBind()
                SetSession()
                lbltitle.Text = "Equipment Information [New]"
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    Session("sender") = "Delete"
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "Equipment", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    Session("sender") = "Delete"
                    msg1.Show()
                ElseIf ex.Number = 2601 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "Equipment", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    Session("sender") = "Delete"
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    Session("sender") = "Delete"
                    msg1.Show()
                End If
            End Try
        End If
    End Sub
    Private Sub dgEquipmentList_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEquipmentList.ItemCommand
        Dim mID As New Guid(e.Item.Cells(0).Text)
        Dim mName As String = New String(e.Item.Cells(1).Text)
        Select Case e.CommandName
            Case "Edit"
                If (Not User.IsInRole("EmployeeView") And Not User.IsInRole("EmployeeEdit")) Then
                    SetObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Edit, "Equipment", User.Identity.Name & " is not Authorized User to edit " + mName, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    msg.ReplacePage = "wfEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
                    Session("sender") = "Authorization"
                    msg.Show()
                    Exit Sub
                End If
                EditRecord(mID)
                txtEquipment.DataBind()
                MarkLog(Flypal.Util.Action.Edit, "Equipment", mEquipment.Name, Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
                If Len(mEquipment.Name) > 15 Then
                    lbltitle.Text = "Equipment [" & mEquipment.Name.Substring(0, 15) & "... ]"
                Else
                    lbltitle.Text = "Equipment [" & mEquipment.Name & " ]"
                End If
                If txtEquipment.Enabled = True Then
                    setFocus(txtEquipment)
                End If
            Case "Delete"
                If (Not User.IsInRole("EmployeeDelete")) Then
                    SetObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Delete, "Equipment", User.Identity.Name & " is not Authorized User to delete " + mName, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    msg.ReplacePage = "wfEquipment.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    Session("sender") = "Authorization"
                    msg.Show()
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select
    End Sub
#End Region


End Class
