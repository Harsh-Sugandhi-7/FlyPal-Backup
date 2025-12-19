'Added By Vikrant On 20-Aug-2015

Public Class wfAuditStandard_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditStandard As AuditStandard
    Public mAuditStandardList As AuditStandardList
    Dim EventLogID As Guid          'Added by Vikrant on 25-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAuditStandard = CType(Session("mAuditStandard"), AuditStandard)
        mAuditStandardList = CType(Session("mAuditStandardList"), AuditStandardList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditStandard")
        Session.Remove("mAuditStandardList")
    End Sub
    Private Sub NewRecord()
        mAuditStandard = AuditStandard.NewAuditStandard(Guid.NewGuid)
        Session("mAuditStandard") = mAuditStandard
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mAuditStandard = AuditStandard.GetChildAuditStandard(mId)
        Session("mAuditStandard") = mAuditStandard
        txtAuditStandardName.Focus()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAuditStandard = AuditStandard.GetChildAuditStandard(mId)
        Session("mAuditStandard") = mAuditStandard
    End Sub
    Private Sub SetObject()
        mAuditStandard.Name = Trim(txtAuditStandardName.Text)
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
                            mAuditStandard = CType(Session("mAuditStandard"), AuditStandard)
                            AuditStandard.DeleteAuditStandard(mAuditStandard.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            txtAuditStandardName.Text = ""
                            upnlAuditStandard.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                           
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
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
    Private Sub SetTitle()
        If mAuditStandard.IsNew Then
            lbltitle.Text = "Audit Standard [New]"
        Else
            If Len(mAuditStandard.Name) > 15 Then
                lbltitle.Text = "Audit Standard [" & mAuditStandard.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Audit Standard [" & mAuditStandard.Name & "]"
            End If
        End If
        lblResult.Text = "Audit Standard List : " & mAuditStandardList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAuditStandardList = AuditStandardList.GetAuditStandardList()
        Session("mAuditStandardList") = mAuditStandardList
        dgAuditStandardList.DataSource = mAuditStandardList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)          'Added by Vikrant on 25-July-2011
        If Not IsPostBack Then
            If txtAuditStandardName.Enabled Then
                txtAuditStandardName.Focus()
            End If
          
            NewRecord()
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            Try
                SetObject()
                mAuditStandard.Save()
                If txtAuditStandardName.Enabled = True Then
                    txtAuditStandardName.Focus()
                End If
                ''Changed by Vikrant on 25-July-2011
                MarkLog(Util.Action.Save, "Audit Standard", mAuditStandard.Name, Util.ErrorType.HandledError, mAuditStandard.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetTitle()
                upnlAuditStandard.Update()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    Private Sub dgAuditStandardList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditStandardList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mId As Guid = mAuditStandardList(CInt(e.CommandArgument)).ID
                EditRecord(mId)
                txtAuditStandardName.DataBind()
                'Changed by Vikrant on 25-July-2011
                MarkLog(Util.Action.Edit, "Audit Standard", mAuditStandard.Name, Util.ErrorType.NoError, mAuditStandard.ID, EventLogID)
                SetTitle()
            Case "DeleteRec"
                Dim mId As Guid = mAuditStandardList(CInt(e.CommandArgument)).ID
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        txtAuditStandardName.Text = ""
        'Changed by Vikrant on 25-July-2011
        MarkLog(Util.Action.[New], "Audit Standard", "", Util.ErrorType.NoError, mAuditStandard.ID, EventLogID)
        If txtAuditStandardName.Enabled Then
            txtAuditStandardName.Focus()
        End If
        SetTitle()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Changed by Vikrant on 25-July-2011
        MarkLog(Flypal.Util.Action.Close, "Audit Standard", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        'Added by vikrant for popup
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        If Request.QueryString("ChildPage3") <> "" Then
            Response.Redirect(Request.QueryString("ChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage=" & Request.QueryString("ChildPage"))
            'ElseIf Request.QueryString("BackPage2") <> "" Then
            '  Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
        Else
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    Private Sub dgAuditStandardList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditStandardList.Sorting
        mAuditStandardList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAuditStandardList") = mAuditStandardList
        dgAuditStandardList.DataSource = mAuditStandardList
        dgAuditStandardList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

 
End Class