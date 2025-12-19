
'Added By : Saylee 
'Dated    : 29-Jun-2016


Public Class wfCWPFunctionMaster_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mFunction As FunctionName
    Public mFunctionList As FunctionNameList

    Dim EventLogID As Guid 'Added By Utkarsh On 19-Jul-2011 For All19072011

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFunction = CType(Session("mFunction"), FunctionName)
        mFunctionList = CType(Session("mFunctionList"), FunctionNameList)
    End Sub
    Private Sub SetSession()
        Session("mFunction") = mFunction
        Session("mFunctionList") = mFunctionList
    End Sub
    Private Sub NewRecord()
        mFunction = FunctionName.NewFunctionName()
        Session("mFunction") = mFunction
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mFunction = FunctionName.GetFunctionName(mId)
        Session("mFunction") = mFunction
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
        '''''msg1.ReplacePage = "wfFunction.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
        'End
        'Session("sender") = "Delete"
        '''''msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mFunction = FunctionName.GetFunctionName(mId)
        Session("mFunction") = mFunction
    End Sub
    Private Sub setObject()
        mFunction.Name = Trim(txtName.Text)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mFunction = CType(Session("mFunction"), FunctionName)
                            FunctionName.DeleteFunctionName(mFunction.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                            'Response.Redirect("wfFunction.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                            'End
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                   MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                 MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                If InStr(ex.Message, "FKtabCWPTaskSheettabCWPFunctionMaster", CompareMethod.Text) Then
                                    MSGBoxCtrl.show("Reference !", "You cannot delete this fuction as it is used in Component Task Sheet", "", MsgBoxStyle.OkOnly, "")
                                    MarkLog(Util.Action.Delete, "Function", "Can't delete : " & mFunction.Name & " is Currently used in Component Task Sheet", Util.ErrorType.NoError, mFunction.ID, EventLogID)
                                Else
                                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                    MarkLog(Util.Action.Delete, "Function", "Can't delete : " & mFunction.Name & " is Currently in use", Util.ErrorType.NoError, mFunction.ID, EventLogID)
                                End If

                                'End
                                ' msg1.Show()
                                End If
                                NewRecord()
                                DataFieldBind()
                                SetTitle()
                                msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011

                                MarkLog(Util.Action.Delete, "Function", mFunction.Name, Util.ErrorType.NoError, mFunction.ID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                      If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        SetTitle()
                    End If
                    Session("sender") = ""
                    SetTitle()
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                  Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            DataFieldBind()
        End If
        upnlFunction.Update()
    End Sub
    Private Sub SetTitle()
        If mFunction.IsNew Then
            lbltitle.Text = "Function [New]"
        Else
            If Len(mFunction.Name) > 15 Then
                lbltitle.Text = "Function [" & mFunction.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Function [" & mFunction.Name & "]"
            End If
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        lblResult.Text = "Function List: " & mFunctionList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mFunctionList = FunctionNameList.GetFunctionNameList("", "")
        Session("mFunctionList") = mFunctionList
        dgFunction.DataSource = mFunctionList
        dgFunction.DataBind() '''''DataBind()

        txtName.Text = mFunction.Name
        upnlFunction.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
       
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            NewRecord()
            DataFieldBind()
        End If
        SetTitle()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Function", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mFunction")
            Session.Remove("mFunctionList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If (Not User.IsInRole("FunctionNew") And mFunction.IsNew) Or (Not User.IsInRole("FunctionEdit") And Not mFunction.IsNew) Then
        '    setObject()
        '    SetSession()
        '    MarkLog(Util.Action.Save, "Function", User.Identity.Name & " is not Authorized User to save " & mFunction.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
        '    Exit Sub
        'End If
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mFunction.Save()
            'Changed By Utkarsh On 19-Jul-2011 For All19072011
            MarkLog(Util.Action.Save, "Function", mFunction.Name, Util.ErrorType.NoError, mFunction.ID, EventLogID)
            'End

            NewRecord()
            DataFieldBind()
            SetSession()
            SetTitle()
            'If txtName.Enabled = True Then
            '    setFocus(txtName)
            'End If

        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            End If
            DataFieldBind()
        End Try
    End Sub
    Private Sub dgFunction_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFunction.RowCommand
        Dim mId As Guid


        Select Case e.CommandName
            Case "ViewRec"
                ' Idx = CInt(e.CommandArgument) + dgFunction.PageIndex * dgFunction.PageSize
                mId = New Guid(e.CommandArgument.ToString) ' mFunctionList(Idx).ID
                Dim mName As String = mFunctionList(mId).Name
                'If (Not User.IsInRole("FunctionView") And Not User.IsInRole("FunctionEdit")) Then
                '    setObject()
                '    SetSession()
                '     MarkLog(Util.Action.Edit, "Function", User.Identity.Name & " is not Authorized User to Edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                EditRecord(mId)
                txtName.Text = mFunction.Name '''''txtName.DataBind()
                SetTitle()
              
                MarkLog(Util.Action.Edit, "Function", mFunction.Name, Util.ErrorType.NoError, mFunction.ID, EventLogID)
                upnlFunction.Update()
            Case "DeleteRec"
                ' Idx = CInt(e.CommandArgument) + dgFunction.PageIndex * dgFunction.PageSize
                mId = New Guid(e.CommandArgument.ToString) ' mId = mFunctionList(Idx).ID
                Dim mName As String = mFunctionList(mId).Name
                'If (Not User.IsInRole("FunctionDelete")) Then
                '    setObject()
                '    SetSession()
                '    MarkLog(Util.Action.Delete, "Function", User.Identity.Name & " is not Authorized User to Delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                DeleteRecord(mId)
        End Select
    End Sub

    Private Sub dgFunction_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgFunction.PageIndexChanging
        dgFunction.PageIndex = e.NewPageIndex
        dgFunction.DataSource = mFunctionList
        Session("mFunctionList") = mFunctionList
        dgFunction.DataBind()
        upnlFunction.Update()
    End Sub

    Private Sub dgFunction_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgFunction.Sorting
        mFunctionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mFunctionList") = mFunctionList
        dgFunction.DataSource = mFunctionList
        dgFunction.DataBind()
        upnlFunction.Update()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "Function", "", Util.ErrorType.NoError, mFunction.ID, EventLogID)

        DataFieldBind()
        SetTitle()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class