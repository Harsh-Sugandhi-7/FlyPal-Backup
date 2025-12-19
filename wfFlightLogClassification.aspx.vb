Partial Class wfFlightLogClassification
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
    Public mFlightLogClassification As FlightLogClassification
    Public mFlightLogClassificationList As FlightLogClassificationList
    Public SortFlag As Boolean
    Dim Name As String
    Public SearchFor As String
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFlightLogClassification = CType(Session("mFlightLogClassification"), FlightLogClassification)
        mFlightLogClassificationList = CType(Session("mFlightLogClassificationList"), FlightLogClassificationList)

    End Sub
    Private Sub SetSession()
        Session("mFlightLogClassification") = mFlightLogClassification
        Session("mFlightLogClassificationList") = mFlightLogClassificationList

    End Sub
    Private Sub ClearAll()
        Session.Remove("mFlightLogClassification")
        Session.Remove("mFlightLogClassificationList")
    End Sub
    Private Sub OpenList(Optional ByVal Name As String = "")
        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList(Name)
        gvFlightLogClassificationList.DataSource = mFlightLogClassificationList
        gvFlightLogClassificationList.DataBind()
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        DataBind()
    End Sub
    Private Sub NewRecord()
        mFlightLogClassification = FlightLogClassification.NewFlightLogClassification(Guid.NewGuid)
        'mFlightLogClassificationList = mFlightLogClassificationList.GetFlightLogClassificationList()
        SetSession()
        Session("mFlightLogClassification") = mFlightLogClassification
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mFlightLogClassification = FlightLogClassification.GetFlightLogClassification(mId)
        Session("mFlightLogClassification") = mFlightLogClassification

    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfFlightLogClassification.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1")
        Session("sender") = "Delete"
        msg1.Show()
        mFlightLogClassification = FlightLogClassification.GetFlightLogClassification(mId)
        Session("mFlightLogClassification") = mFlightLogClassification
    End Sub
    Private Sub setObject()
        mFlightLogClassification.Name = Trim(txtName.Text)
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtName" Then
            If Len(txtName.Text.Trim) > 50 Then
                custValidator.ErrorMessage = "Name too long."
                e.IsValid = False
            End If
        End If
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        'Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mFlightLogClassification As FlightLogClassification
                            Session("sender") = ""
                            mFlightLogClassification = CType(Session("mFlightLogClassification"), FlightLogClassification)
                            FlightLogClassification.DeleteFlightLogClassification(mFlightLogClassification.ID)
                            DataFieldBind()
                            upnlMain.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfFlightLogClassification.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.DatabaseException, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfFlightLogClassification.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfFlightLogClassification.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Flight Log Classification", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mFlightLogClassification.ID, EventLogID)
                                Exit Sub
                            End If
                            DataFieldBind()
                            ' msgCount = ex.Errors.Count
                        Finally
                            'If msgCount = 0 Then
                            'MarkLog(Util.Action.Delete, "Flight Log Classification", mFlightLogClassification.Name, Util.ErrorType.NoError, mFlightLogClassification.ID, EventLogID)
                            'End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""

                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()

                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If Not mFlightLogClassification.IsNew Then
            If Len(mFlightLogClassification.Name) > 15 Then
                lbltitle.Text = "Flight Log Classification [" & mFlightLogClassification.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Flight Log Classification [" & mFlightLogClassification.Name & "]"
            End If
        Else
            lbltitle.Text = "Flight Log Classification [New]"
        End If
        lblResult.Text = "As per criteria " & mFlightLogClassificationList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList()
        gvFlightLogClassificationList.DataSource = mFlightLogClassificationList
        'gvFlightLogClassificationList.DataBind()
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        DataBind()
        lblResult.Text = "As per criteria " & mFlightLogClassificationList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            NewRecord()
            OpenList()
            SetTitle()
            DataFieldBind()
        End If

    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ClearAll()
        MarkLog(Util.Action.Close, "Flight Log Classification", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""

        Dim mopenas As String = Request.QueryString("Typepup")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        'Commented and added by Saylee on 14-Aug-2013 for ALL14082013 (Switch Log functionality)
        Response.Redirect(Request.QueryString("BackPage1"))

        '-------CHANGED BY VIKRANT---------------
        'If mLog.IsTLP = "True" Then
        '    Response.Redirect("wfTLP.aspx")
        'Else
        '    If AppSettings("LogDetailPage") = "NewPage" Then
        '        Response.Redirect("wfLogSOP.aspx")
        '    Else
        '        Response.Redirect("wfLogDetail.aspx")
        '    End If
        'End If

        '-----------------------------------------

        'Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            Try
                GetSession()
                setObject()
                mFlightLogClassification.Save()
                MarkLog(Util.Action.Save, "Flight Log Classification", mFlightLogClassification.Name, Util.ErrorType.NoError, mFlightLogClassification.ID, EventLogID)
                NewRecord()
                txtName.DataBind()
                OpenList()
                SetSession()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                SetTitle()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End Try
        End If
    End Sub
    Private Sub gvFlightLogClassificationList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvFlightLogClassificationList.RowCommand
        Select Case e.CommandName
            Case "EditRecord"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim mId As Guid = mFlightLogClassificationList(index).ID
                EditRecord(mId)
                txtName.DataBind()
                setFocus(txtName)
                MarkLog(Util.Action.Edit, "Flight Log Classification", mFlightLogClassification.Name, Util.ErrorType.NoError, mFlightLogClassification.ID, EventLogID)
                SetTitle()
            Case "DeleteRecord"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim mId As Guid = mFlightLogClassificationList(index).ID
                'Dim mID As New Guid(e.row.Cells(0).Text)
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                'msg.ReplacePage = "wfFlightLogClassification.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                'Session("sender") = "Delete"
                'msg.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                mFlightLogClassification = FlightLogClassification.GetFlightLogClassification(mID)
                Session("mFlightLogClassification") = mFlightLogClassification
                DataFieldBind()
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImgFindNow.Click
        OpenList(Trim(txtSearch.Text))
        lblResult.Text = "As per criteria " & mFlightLogClassificationList.Count & " Record(s) Found."
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        GetSession()
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        NewRecord()
        MarkLog(Util.Action.[New], "Flight Log Classification", "", Util.ErrorType.NoError, mFlightLogClassification.ID, EventLogID)
        'OpenList()
        gvFlightLogClassificationList.DataSource = mFlightLogClassificationList
        gvFlightLogClassificationList.DataBind()
        txtName.DataBind()
        SetTitle()
        SetSession()
    End Sub

#End Region

End Class
