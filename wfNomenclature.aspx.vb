Partial Class wfNomenclature
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
    Public mNomenclature As NomenClature
    Public mNomenclatureList As NomenclatureList

    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mNomenclature = Session("mNomenclature")
        mNomenclatureList = Session("mNomenclatureList")
    End Sub
    Private Sub SetSession()
        Session("mNomenclature") = mNomenclature
        Session("mNomenclatureList") = mNomenclatureList
    End Sub
    Private Sub NewRecord()
        mNomenclature = NomenClature.NewNomenClature()
        Session("mNomenclature") = mNomenclature
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mNomenclature = NomenClature.GetNomenclature(mId)
        Session("mNomenclature") = mNomenclature
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfNomenclature.aspx?BackPage=" & Request.QueryString("BackPage")
        Session("sender") = "Delete"
        msg1.Show()
        mNomenclature = NomenClature.GetNomenclature(mId)
        Session("mNomenclature") = mNomenclature
    End Sub
    Private Sub setObject()
        mNomenclature.Name = Trim(txtName.Text)
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
                            mNomenclature = Session("mNomenclature")
                            NomenClature.DeleteNomenclature(mNomenclature.ID)
                            Response.Redirect("wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfNomenclature.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfNomenclature.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfNomenclature.aspx?BackPage=" & Request.QueryString("BackPage")
                                MarkLog(Util.Action.Delete, "Nomenclature", "Can't delete : " & mNomenclature.Name & " is Currently in use", Util.ErrorType.NoError, mNomenclature.ID, EventLogID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Nomenclature", mNomenclature.Name, Util.ErrorType.NoError, mNomenclature.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            Response.Redirect("wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Saylee 17-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerNomenclature(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mNomenclatureList = NomenclatureList.GetNomenclatureList
        dgNomenclature.DataSource = mNomenclatureList
        Session("mNomenclatureList") = mNomenclatureList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            NewRecord()
            DataFieldBind()
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        lblResult.Text = "Nomenclature List: " & mNomenclatureList.Count & " Record(s) Found."

        MessageBoxResult()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBacktop.Click
        MarkLog(Util.Action.Close, "Nomenclature", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("PartNew") And mNomenclature.IsNew) Or (Not User.IsInRole("PartEdit") And Not mNomenclature.IsNew) Then
            setObject()
            SetSession()
            'MarkLog(Util.Action.Save, "Nomencalture", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            MarkLog(Util.Action.Save, "Nomencalture", User.Identity.Name & " is not Authorized User to save ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            Session("sender") = "Authorization"
            msg.Show()
            Exit Sub
        End If
        Try
            setObject()
            mNomenclature.Save()
            MarkLog(Util.Action.Save, "Nomenclature", mNomenclature.Name, Util.ErrorType.HandledError, mNomenclature.ID, EventLogID)
            mNomenclature = NomenClature.NewNomenClature
            DataFieldBind()
            SetSession()
            lblTitle.Text = "Nomenclature [New]"
            'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
            lblResult.Text = "Nomenclature List: " & mNomenclatureList.Count & " Record(s) Found."

        Catch ex As SqlException
            If ex.Number = 8145 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfNomenclature.aspx?BackPage=" & Request.QueryString("BackPage")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 2627 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfNomenclature.aspx?BackPage=" & Request.QueryString("BackPage")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 547 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfNomenclature.aspx?BackPage=" & Request.QueryString("BackPage")
                Session("sender") = "Delete"
                msg1.Show()
            End If
        End Try
    End Sub
    Private Sub dgNomenclature_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgNomenclature.ItemCommand
        Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
        Dim mName As String = New String(e.Item.Cells(1).Text)
        Select Case e.CommandName
            Case "Edit"
                If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
                    setObject()
                    SetSession()
                    'MarkLog(Util.Action.Edit, "Nomenclature", "Not Authorized User", Util.ErrorType.HandledError, mId, EventLogID)
                    MarkLog(Util.Action.Edit, "Nomenclature", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, mId, EventLogID)
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    msg.ReplacePage = "wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                    Session("sender") = "Authorization"
                    msg.Show()
                    Exit Sub
                End If
                EditRecord(mId)
                txtName.DataBind()
                DisableName(mId) 'Added by : Saylee 17-Jun-2020, ALL16062020
                MarkLog(Util.Action.Edit, "Nomenclature", mName, Util.ErrorType.NoError, mId, EventLogID)
                If Len(mNomenclature.Name) > 15 Then
                    lblTitle.Text = "Nomenclature [" & mNomenclature.Name.Substring(0, 15) & "...]"
                Else
                    lblTitle.Text = "Nomenclature [" & mNomenclature.Name & "]"
                End If
                'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
                lblResult.Text = "Nomenclature List: " & mNomenclatureList.Count & " Record(s) Found."

            Case "Delete"
                If (Not User.IsInRole("PartDelete")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Delete, "Nomenclature", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, mId, EventLogID)
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    msg.ReplacePage = "wfNomenclature.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                    Session("sender") = "Authorization"
                    msg.Show()
                    Exit Sub
                End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        MarkLog(Util.Action.[New], "Nomenclature", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
        txtName.Text = ""
        DataFieldBind()
        lblTitle.Text = "Nomenclature [New]"
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        lblResult.Text = "Nomenclature List: " & mNomenclatureList.Count & " Record(s) Found."
    End Sub
#End Region

End Class
