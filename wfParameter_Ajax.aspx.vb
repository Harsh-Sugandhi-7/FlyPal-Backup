'AJAX Conversion by Vikrant on 11-May-2015

Public Class wfParameter_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mParameterList As ParameterList
    Public mParameter As Parameter
    'Added by Vikrant on 26-July-2011
    Dim EventLogID As Guid
    '-----------------------------
    Public mAssemblyStatus As AssemblyStatus
    '-----------------------------
#End Region

#Region " Business Methods "
    Private Sub SetSession()
        Session("mParameter") = mParameter
        Session("mParameterList") = mParameterList
        'Session("mAssemblyStatus.AssemblyParameters") = mAssemblyStatus.AssemblyParameters
        Session("mAssemblyStatus") = mAssemblyStatus '$$$$$$$$
    End Sub
    Private Sub GetSession()
        mParameter = Session("mParameter")
        mParameterList = Session("mParameterList")
        'mAssemblyStatus.AssemblyParameters = Session("mAssemblyStatus.AssemblyParameters")
        mAssemblyStatus = Session("mAssemblyStatus") '$$$$$$$$$$$$$
    End Sub
    Private Sub NewRecord()
        mParameter = Parameter.NewParameter(Guid.NewGuid)
        Session("mParameter") = mParameter
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mParameter = Parameter.GetParameter(mId)
        Session("mParameter") = mParameter
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mParameter = Parameter.GetParameter(mId)
        Session("mParameter") = mParameter
    End Sub
    Private Sub SetObject()
        mParameter.Name = txtName.Text.Trim
        mParameter.Description = txtDescription.Text.Trim
        Session("mParameter") = mParameter
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
                            '------------------------------------------------------
                            If mAssemblyStatus.AssemblyParameters.Contains(mParameter.ID, mAssemblyStatus.AssemblyID) = True Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Openscript", MessageBox.Show("You can not delete this entry...", False), True)
                                NewRecord()
                                setFocus(txtName)
                                DataFieldBind()
                                Exit Sub
                            End If
                            '-------------------------------------------------------
                            Parameter.DeleteParameter(mParameter.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlParameterDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlParameterDetails.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Parameter", mParameter.Name, Util.ErrorType.NoError, mParameter.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    NewRecord()
                    DataFieldBind()
                    SetTitle()
                    upnlParameterDetails.Update()
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
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
        If mParameter.IsNew Then
            lbltitle.Text = "Parameter [New]"
        Else
            If Len(mParameter.Name) > 15 Then
                lbltitle.Text = "Parameter [" & mParameter.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Parameter [" & mParameter.Name & "]"
            End If
        End If
        lblResult.Text = "Parameter List: " & mParameterList.Count & "Records Found"
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mParameterList = ParameterList.GetParameterList
        Session("mParameterList") = mParameterList
        dgParameter.DataSource = mParameterList
        upnlParameterDetails.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtName.Enabled = True Then
                SetFocus(txtName)
            End If
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Parameter", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            Try
                SetObject()
                mParameter.Save()
                If txtName.Enabled = True Then
                    txtName.Focus()
                End If
                MarkLog(Util.Action.Save, "Parameter", mParameter.Name, Util.ErrorType.HandledError, mParameter.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetTitle()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    Private Sub dgParameter_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgParameter.RowCommand
        Dim mId As Guid
        Select Case e.CommandName
            Case "EditRec"

                'Dim Index As Integer = CInt(e.CommandArgument) + dgParameter.PageSize * dgParameter.PageIndex
                'Dim mId As Guid = mWOList(Index).ID

                mId = New Guid(dgParameter.Rows(e.CommandArgument).Cells(0).Text)
                EditRecord(mId)
                txtName.DataBind()
                txtDescription.DataBind()
                txtName.Focus()
                MarkLog(Util.Action.Edit, "Parameter", mParameter.Name, Util.ErrorType.NoError, mParameter.ID, EventLogID)
                SetTitle()
            Case "DeleteRec"
                mId = New Guid(dgParameter.Rows(e.CommandArgument).Cells(0).Text)
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Changed by Vikrant on 28-July-2011
        MarkLog(Util.Action.[New], "Parameter", "", Util.ErrorType.NoError, mParameter.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        SetTitle()
    End Sub
    'Added By Prashant 19-June-2009 for grid sorting
    Private Sub dgParameter_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgParameter.Sorting
        mParameterList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mParameterList") = mParameterList
        dgParameter.DataSource = mParameterList
        dgParameter.DataBind()
    End Sub
    '-----------------------------------------------
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

   
End Class