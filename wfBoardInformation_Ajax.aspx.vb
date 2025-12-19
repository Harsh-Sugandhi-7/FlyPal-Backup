'AJAX Conversion By Vikrant On 30-Jun-2015

Public Class wfBoardInformation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mBoardInfoList As AircraftInformationBoard.BoardInfoList
    Private mBoardTypeList As AircraftInformationBoard.BoardTypeList

    Public mMachine As Machine
    Dim EventLogID As Guid
    Public detail As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mBoardInfoList = Session("mBoardInfoList")
        mMachine = Session("mMachine")
        mBoardTypeList = Session("mBoardTypeList")
    End Sub
    Private Sub SetSession()
        Session("mBoardInfoList") = mBoardInfoList
        Session("mMachine") = mMachine
        Session("mBoardTypeList") = mBoardTypeList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mBoardInfoList")
        Session.Remove("mBoardTypeList")
        Session.Remove("BoardType")
        Session.Remove("Index")
        Session.Remove("FromSelectInfo")
    End Sub
    Private Sub DeleteBoardInfo(ByVal Index As Int32)
        Session("Index") = Index
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim ID As Guid
                        Dim Description As String
                        Try

                            Session("sender") = ""
                            Dim Index As Integer = CType(Session("Index"), Integer)
                            ID = mBoardInfoList(Index).ID
                            Description = mBoardInfoList(Index).Description
                            mBoardInfoList.Remove(Index)

                            dgBoardInfoList.DataSource = mBoardInfoList
                            dgBoardInfoList.DataBind()
                            lblResult.Text = "List of Board Info. : " & mBoardInfoList.Count & " Record(s) found."
                            Session("mBoardInfoList") = mBoardInfoList
                            Session("FromSelectInfo") = "FromSelectInfo"
                            upnlGrid.Update()
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Board Information", "Can't delete :" & mBoardInfoList.Item(mBoardInfoList.CurrentIndex).Description & " is Currently in use", Util.ErrorType.NoError, ID, EventLogID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Board Information", Description, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mBoardTypeList = AircraftInformationBoard.BoardTypeList.GetBoardTypeList()
        Session("mBoardTypeList") = mBoardTypeList
        cmbBoardType.DataSource = mBoardTypeList

        If Session("FromSelectInfo") = "FromSelectInfo" Then
            mBoardInfoList = Session("mBoardInfoList")
            Session("FromSelectInfo") = ""
        Else
            If mBoardInfoList Is Nothing Then
                mBoardInfoList = AircraftInformationBoard.BoardInfoList.GetBoardInfoList(mMachine.ID)
            Else
                mBoardInfoList = Session("mBoardInfoList")
            End If
        End If

        dgBoardInfoList.DataSource = mBoardInfoList
        Session("mBoardInfoList") = mBoardInfoList
        Session("mBoardTypeList") = mBoardTypeList
        DataBind()
        lblResult.Text = "List of Board Info. : " & mBoardInfoList.Count & " Record(s) found."
        If CType(Session("BoardType"), Integer) <> 0 Then cmbBoardType.SelectedValue = CType(Session("BoardType"), Integer)


    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            cmbBoardType.Focus()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Added by Vikrant on 1-Aug-2011
        MarkLog(Util.Action.[New], "Board Information", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("mMachine") = mMachine
        Session("BoardType") = cmbBoardType.SelectedValue
        Session("mBoardInfoList") = mBoardInfoList
        ' Response.Redirect("wfSelectInformationBoard_Ajax.aspx?ChildPage1=wfBoardInformation_Ajax.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentSelectInfoBoardFunction", "CallParentSelectInfoBoardFunction();", True)

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        mBoardInfoList = Session("mBoardInfoList")

        Try
            If (Not User.IsInRole("AircraftInformationBoardNew")) Then 'Or (Not User.IsInRole("AircraftInformationBoardEdit")) Then
                SetSession()
                MarkLog(Util.Action.Save, "Board Information", User.Identity.Name & " is not Authorized User to save " & mBoardInfoList.Item(mBoardInfoList.CurrentIndex).Description, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If mBoardInfoList.Count <= 8 Then
                mBoardInfoList.Save()
                '---------------------------------------
                If mBoardInfoList.CurrentIndex = 0 Then
                    detail = ""
                Else
                    detail = mBoardInfoList.Item(mBoardInfoList.CurrentIndex).Description
                End If
                '----------------------------------------
                MarkLog(Util.Action.Save, "Board Information", detail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                Session("mAircraftInformationBoardList") = Nothing
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
            Else
                Session("mBoardInfoList") = mBoardInfoList
                Session("FromSelectInfo") = "FromSelectInfo"
                MSGBoxCtrl.show("Save Alert!", "You cannot Save more than 8 records", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub dgBoardInfoList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgBoardInfoList.RowCommand
        Dim Index As Int16
        Select Case e.CommandName
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgBoardInfoList.PageSize * dgBoardInfoList.PageIndex
                If (Not User.IsInRole("AircraftInformationBoardDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    DeleteBoardInfo(Index)
                End If
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Added by Vikrant on 1-Aug-2011
        MarkLog(Util.Action.Close, "Board Information", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()

        'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    'New addition by Saylee on 14-July-09 for Sorting Order
    Private Sub dgBoardInfoList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgBoardInfoList.Sorting
        mBoardInfoList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mBoardInfoList") = mBoardInfoList
        dgBoardInfoList.DataSource = mBoardInfoList
        dgBoardInfoList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class