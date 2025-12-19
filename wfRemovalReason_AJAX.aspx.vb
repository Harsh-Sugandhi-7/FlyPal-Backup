
'AJAX Conversion By: Saylee on 19-Mar-2015 : ModuleID:302
Public Class wfRemovalReason_AJAX
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Public mRemovalReason As RemovalReason
    Public mRemovalReasonList As RemovalReasonList
    'Added by Vikrant on 26-July-2011
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRemovalReason = CType(Session("mRemovalReason"), RemovalReason)
        mRemovalReasonList = CType(Session("mRemovalReasonList"), RemovalReasonList)
    End Sub
    Private Sub SetSession()
        Session("mRemovalReason") = mRemovalReason
        Session("mRemovalReasonList") = mRemovalReasonList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRemovalReasonList")
    End Sub
    Private Sub NewRecord()
        mRemovalReason = RemovalReason.NewRemovalReason
        Session("mRemovalReason") = mRemovalReason
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mRemovalReason = RemovalReason.GetRemovalReason(mId)
        Session("mRemovalReason") = mRemovalReason
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        ''msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage")
        'msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mRemovalReason = RemovalReason.GetRemovalReason(mId)
        Session("mRemovalReason") = mRemovalReason
    End Sub
    Private Sub setObject()
        mRemovalReason.Name = txtReason.Text
        Session("mRemovalReason") = mRemovalReason
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
                            mRemovalReason = CType(Session("mRemovalReason"), RemovalReason)
                            RemovalReason.DeleteRemovalReason(mRemovalReason.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlRemovalReason.Update()
                            'Response.Redirect("wfRemovalReason.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                ''msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage")
                                'msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                ''msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage")
                                'msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                ''msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage")
                                'msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                MarkLog(Util.Action.Delete, "Removal Reason", "Can't delete : " & mRemovalReason.Name & " is Currently in use", Util.ErrorType.NoError, mRemovalReason.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""

                    'Response.Redirect("wfRemovalReason.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        SetTitle()
                        upnlRemovalReason.Update()
                    End If
                    Session("sender") = ""
                    SetTitle()
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    '    DataFieldBind()
                    '    Response.Redirect("wfRemovalReason.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    'Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfRemovalReason.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfRemovalReason.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mRemovalReason.IsNew Then
            lbltitle.Text = "Removal Reason [New]"
        Else
            'lbltitle.Text = "Removal Reason [" & mRemovalReason.Name & "]"
            If Len(mRemovalReason.Name) > 15 Then
                lbltitle.Text = "Removal Reason [" & mRemovalReason.Name.Substring(0, 15) & "... ]"
            Else
                lbltitle.Text = "Removal Reason [" & mRemovalReason.Name & " ]"
            End If
        End If
        lblResult.Text = "Removal Reason List"
        upnlRemovalReason.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList
        Session("mRemovalReasonList") = mRemovalReasonList
        dgRemovalReason.DataSource = mRemovalReasonList
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtReason" Then
            If Len(txtReason.Text) > 250 Then
                custValidator.ErrorMessage = "Max. length of Removal Reason should be 250 char."
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
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If txtReason.Enabled = True Then
                setFocus(txtReason)
            End If
            NewRecord()
            DataFieldBind()
        End If
        SetTitle()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Added by Vikrant on 26-July-2011
        MarkLog(Util.Action.[New], "Removal Reason", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
        DataFieldBind()
        SetTitle()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            Try
                setObject()
                mRemovalReason.Save()
                MarkLog(Util.Action.Save, "Removal Reason : " + mRemovalReason.Name, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
                SetTitle()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfRemovalReason.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                End If
            End Try
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Added by Vikrant on 26-July-2011
        MarkLog(Util.Action.Close, "Removal Reason", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session("sender") = ""

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            'Session.Remove("MiddleFrame")
            Session.Remove("mRemovalReason")
            Session.Remove("mRemovalReasonList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))

    End Sub
    Private Sub dgRemovalReason_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovalReason.RowCommand

        Select Case e.CommandName
            Case "EditRec"
                Dim mIndex As Integer = CInt(e.CommandArgument) + dgRemovalReason.PageIndex * dgRemovalReason.PageSize
                Dim mID As Guid = mRemovalReasonList(mIndex).ID
                EditRecord(mID)
                txtReason.DataBind()
                'Added by Vikrant on 26-July-2011
                MarkLog(Util.Action.Edit, "Removal Reason", txtReason.Text, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                SetTitle()

                upnlRemovalReason.Update()

            Case "DeleteRec"
                Dim mIndex As Integer = CInt(e.CommandArgument) + dgRemovalReason.PageIndex * dgRemovalReason.PageSize
                Dim mID As Guid = mRemovalReasonList(mIndex).ID
                DeleteRecord(mID)
                'Added by Vikrant on 26-July-2011
                MarkLog(Util.Action.Delete, "Removal Reason", txtReason.Text, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End Select
    End Sub
    Private Sub dgRemovalReason_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRemovalReason.PageIndexChanging
        dgRemovalReason.PageIndex = e.NewPageIndex
        dgRemovalReason.DataSource = mRemovalReasonList
        'Session("mRequisitionListNew") = mRequisitionListNew
        Session("mRemovalReasonList") = mRemovalReasonList
        dgRemovalReason.DataBind()
        'dgRemovalReason.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 11-Jan-2023
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

   
End Class