'AJAX Conversion By : Saylee on 1-Sep-2014
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfTaskCardList_AJAX
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Public Enum UserRightsFor
        urfNew = 1
        urfEdit = 2
        urfDelete = 3
        urfView = 4
        urfPrint = 5
        urfSave = 6
    End Enum
#End Region

#Region " Variable Declaration "
    Dim mTaskCard As TaskCard
    Dim mTaskCardList As TaskCardList
    Dim mTaskStep As TaskStep
    Dim TaskCardNo, TaskDesc As String
    Dim InspTypeIntervalSearch As String

    Dim ModelID As String
    Dim mModelList As ModelList

    'Added by Vikrant on 20-July-2011
    Dim EventLogID As Guid
    Dim mIsRII As Boolean
#End Region

#Region " Business Methods "
    Private Sub ClearAll()
        Session.Remove("mTaskCardList")
        Session.Remove("mTaskCard")
        Session.Remove("SearchIndex")
        Session.Remove("TaskCardNo")
        Session.Remove("TaskDesc")
        Session.Remove("InspTypeIntervalSearch")
        Session.Remove("mModelList")
        Session.Remove("ModelID")
        Session.Remove("ISRII")
    End Sub
    Private Sub addAttributes()

    End Sub
    Private Sub GetSession()
        mTaskCardList = Session("mTaskCardList")
        mTaskCard = Session("mTaskCard")
        TaskCardNo = Session("TaskCardNo")
        TaskDesc = Session("TaskDesc")
        InspTypeIntervalSearch = Session("InspTypeIntervalSearch")
        mModelList = Session("mModelList")
        ModelID = Session("wfTaskCardListModelID")
        mIsRII = Session("ISRII")
    End Sub
    Private Sub SetSession()
        Session("mTaskCard") = mTaskCard
        Session("mTaskCardList") = mTaskCardList
        Session("TaskCardNo") = TaskCardNo
        Session("TaskDesc") = TaskDesc
        Session("InspTypeIntervalSearch") = InspTypeIntervalSearch

        Session("mModelList") = mModelList
        Session("wfTaskCardListModelID") = ModelID
        Session("ISRII") = mIsRII
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mTaskCardList")
        Session.Remove("TaskCardNo")
        Session.Remove("TaskDesc")

        Session.Remove("ModelID")
        Session.Remove("ISRII")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetControl()
        setVariables()
        FindNow(TaskCardNo, ModelID.ToString, TaskDesc)
        dgTaskCardList.PageIndex = 0
        dgTaskCardList.DataBind()
        If cmbModelList.Items.Contains(New System.Web.UI.WebControls.ListItem(ModelID)) Then
            cmbModelList.SelectedValue = ModelID
        End If
        txtDesc.Text = TaskDesc
        txtTaskNo.Text = TaskCardNo
        chkIsRII.Checked = mIsRII
        lblResult.Text = "List of Task Cards as per criteria : " & "" & mTaskCardList.Count & " Record(s) found."
    End Sub
    Private Sub ClearControl()
        txtDesc.Text = ""
        txtTaskNo.Text = ""
    End Sub
    Private Sub NewRecord()
        Dim ID As Guid = Guid.Empty



        Session("Add") = "1"
        SetSession()
        Session("ID") = ID.ToString

        If (Not Session("POPUpPage") Is Nothing) And (Session("POPUpPage") = "wfSelectTaskCardList_Ajax.aspx") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskMasterWindow", "OpenTaskMasterWindow()", True)
        Else
            If IsNothing(Request.QueryString("GChildPage7")) Then
                BackPage.Push(Session("TaskBackPage"), "index.aspx")
            Else
                BackPage.Push(Session("TaskBackPage"), "wfTaskCardList_AJAX.aspx")
            End If

            Dim str As String
            str = "openledgersame('wfTaskCard_Ajax.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&TaskBackPage=index.aspx" & "&ID=" & ID.ToString & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End If

    End Sub
    Private Sub EditRecord(ByVal mID As Guid)


        SetSession()
        Session("Edit") = "1"
        dgTaskCardList.DataSource = mTaskCardList
        DataBind()
        Session("ID") = mID.ToString
        ' Response.Redirect("wfTaskCard.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&TaskBackPage=wfTaskCardList.aspx" & "&ID=" & mID.ToString)

        If (Not Session("POPUpPage") Is Nothing) And (Session("POPUpPage") = "wfSelectTaskCardList_Ajax.aspx") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskMasterWindow", "OpenTaskMasterWindow()", True)
        Else

            If IsNothing(Request.QueryString("GChildPage7")) Then
                BackPage.Push(Session("TaskBackPage"), "index.aspx")
            Else
                BackPage.Push(Session("TaskBackPage"), "wfTaskCardList_AJAX.aspx")
            End If

            Dim str As String
            str = "openledgersame('wfTaskCard_Ajax.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&TaskBackPage=index.aspx" & "&ID=" & mID.ToString & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End If
    End Sub
    Public Sub GridBind()
        dgTaskCardList.DataSource = mTaskCardList
        dgTaskCardList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, " ", MsgBoxStyle.YesNo, "Delete")
        mTaskCard = TaskCard.GetTaskCard(mID)
        Session("mTaskCard") = mTaskCard
        GridBind()
    End Sub
    'Added By Vikrant On 02-Jan-2014 For All02012014
    Private Function IsInRole(ByVal CheckFor As UserRightsFor) As Boolean
        Dim IsInRoleString As String = "TaskCard"

        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case UserRightsFor.urfView
                Return User.IsInRole(IsInRoleString + "View")
            Case UserRightsFor.urfNew
                Return User.IsInRole(IsInRoleString + "New")
            Case UserRightsFor.urfEdit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case UserRightsFor.urfSave
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case UserRightsFor.urfDelete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case UserRightsFor.urfPrint
                Return User.IsInRole(IsInRoleString + "Print")
                'Case Rights.FindNow
                '   Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        End Select
    End Function
    'End
    Private Sub setVariables()
        TaskCardNo = txtTaskNo.Text
        Session("TaskCardNo") = TaskCardNo
        TaskDesc = txtDesc.Text
        Session("TaskDesc") = TaskDesc

        InspTypeIntervalSearch = txtInspTypeIntervalSearch.Text
        Session("InspTypeIntervalSearch") = InspTypeIntervalSearch

        ModelID = IIf(cmbModelList.SelectedIndex <= 0, "", cmbModelList.SelectedValue)
        Session("wfTaskCardListModelID") = ModelID

        mIsRII = chkIsRII.Checked
    End Sub
    Private Sub FindNow(Optional ByVal TaskCardNo As String = "", Optional ByVal ModelID As String = "", Optional ByVal Description As String = "", Optional ByVal InspTypeIntervalSearch As String = "")
        'clear the obj and grid
        If ModelID = "" Then
            ModelID = "00000000-0000-0000-0000-000000000000"
        End If

        mTaskCardList = Nothing
        dgTaskCardList.DataSource = Nothing
        mTaskCardList = TaskCardList.GetTaskCardList("", "", "", TaskCardNo, ModelID, "", Description, InspTypeIntervalSearch, IsRII:=chkIsRII.Checked)

        'bind the list to the datagrid
        dgTaskCardList.DataSource = mTaskCardList
        'set the session
        SetSession()
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
                            Dim mTaskCard As TaskCard
                            Session("sender") = ""
                            mTaskCard = CType(Session("mTaskCard"), TaskCard)
                            TaskCard.DeleteTaskCard(mTaskCard.ID)
                            'Response.Redirect("wfTaskCardList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                            DataFieldBind()
                            SetControl()
                            upnlGridView.Update()
                            upnlActionBtnTop.Update()
                            upnlActionBtnBottom.Update()
                            upnlResult.Update()

                            'Catch ex As ThreadAbortException
                            '    'Must Catch the Exception But Dont Throw Anything

                        Catch ex As Exception
                            If (InStr(ex.InnerException.InnerException.ToString, "FKtabWOJobTasktabTaskCard", CompareMethod.Text) > 0) Or (InStr(ex.InnerException.InnerException.ToString, "FKtabMaintenanceTaskDetailstabTaskCard", CompareMethod.Text) > 0) Then
                                'Throw New Exception(FlyPal22.Resources.Strings.GetResourceString("RecordInUse"), ex)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Message, MsgBoxStyle.OkOnly, "")
                                DataFieldBind()
                                SetControl()
                                MarkLog(Util.Action.Delete, "TaskCard", "Can't delete :" & mTaskCard.TaskCardNo & " is Currently in use", Util.ErrorType.HandledError, mTaskCard.ID, EventLogID)
                                msgCount = 1
                            Else
                                Throw ex
                            End If

                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "TaskCard", "Task Card No. : " + mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok
                    GetSession()
                    DataFieldBind()
                    'SetControl()
                    dgTaskCardList.DataBind()

                    If cmbModelList.Items.Contains(New System.Web.UI.WebControls.ListItem(ModelID)) Then
                        cmbModelList.SelectedValue = ModelID
                    End If

                    txtDesc.Text = TaskDesc
                    txtTaskNo.Text = TaskCardNo
                    lblResult.Text = "List of Task Cards as per criteria : " & "" & mTaskCardList.Count & " Record(s) found."

                    DataFieldBind()
                    SetControl()
                    upnlSearchCriteria.Update()
                    upnlResult.Update()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            DataFieldBind()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        ModelID = Session("wfTaskCardListModelID")
        mModelList = ModelList.GetModelList(0, , , , "(All)")
        Session("mModelList") = mModelList
        cmbModelList.DataSource = mModelList


        dgTaskCardList.DataSource = mTaskCardList
        Session("mTaskCardList") = mTaskCardList
        DataBind()

        If ModelID <> "" Then cmbModelList.SelectedValue = ModelID.ToString
        txtDesc.Text = TaskDesc
        txtTaskNo.Text = TaskCardNo
        txtInspTypeIntervalSearch.Text = InspTypeIntervalSearch
        chkIsRII.Checked = mIsRII
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'ClearAll()
        addAttributes()
        GetSession()
        ' new Added by Vikrant on 20-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            SetControl()
            '  If IsNothing(Request.QueryString("GChildPage7")) Then Session("MiddleFrame") = "wfTaskCardList_AJAX.aspx"
            If Session("GChildPage7") Is Nothing Then Session("MiddleFrame") = "wfTaskCardList_AJAX.aspx"
        End If

        SetSession()
        If CType(Session("Add"), String) = "1" Then
            txtDesc.Text = TaskDesc
            txtTaskNo.Text = TaskCardNo
            If ModelID <> "" Then
                cmbModelList.SelectedValue = ModelID
            End If
            chkIsRII.Checked = mIsRII
            Session("Add") = ""
        End If
        If CType(Session("Edit"), String) = "1" Then
            dgTaskCardList.DataSource = mTaskCardList
            DataBind()

            If ModelID <> "" Then
                cmbModelList.SelectedValue = ModelID
            Else
                mModelList = ModelList.GetModelList(0, , , , "(All)")
                cmbModelList.DataSource = mModelList
            End If

            txtDesc.Text = TaskDesc
            txtTaskNo.Text = TaskCardNo
            chkIsRII.Checked = mIsRII
            lblResult.Text = "List of Task Cards as per criteria : " & "" & mTaskCardList.Count & " Record(s) found."
            Session("Edit") = ""
        End If

    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        'Added By Vikrant On 02-Jan-2014 For All02012014
        If (Not IsInRole(UserRightsFor.urfNew)) Or (Not IsInRole(UserRightsFor.urfEdit)) Then
            MarkLog(Util.Action.[New], "TaskCard", User.Identity.Name & " is not Authorized User to add Task Card ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'End
        NewRecord()

    End Sub
    Private Sub imgFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgFindNow.Click
        setVariables()
        FindNow(TaskCardNo, ModelID, TaskDesc, InspTypeIntervalSearch) 'changed by Saylee for ALL03042014 for multiple criteria
        dgTaskCardList.PageIndex = 0
        dgTaskCardList.DataBind()
        'SetGrid()
        'set result label
        lblResult.Text = "List of Task Cards as per criteria : " & "" & mTaskCardList.Count & " Record(s) found."
        upnlResult.Update()
        upnlGridView.Update()
    End Sub

    Private Sub dgTaskCardList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTaskCardList.PageIndexChanging
        dgTaskCardList.PageIndex = e.NewPageIndex
        dgTaskCardList.DataSource = mTaskCardList
        Session("mTaskCardList") = mTaskCardList
        dgTaskCardList.DataBind()
    End Sub
    Private Sub dgTaskCardList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskCardList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardList.PageSize * dgTaskCardList.PageIndex
                Dim mId As Guid = mTaskCardList(Index).ID

                'Added By Vikrant On 02-Jan-2014 For All02012014 
                If (Not IsInRole(UserRightsFor.urfView) And Not IsInRole(UserRightsFor.urfEdit)) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "TaskCard", User.Identity.Name & " is not Authorized User to edit " & mTaskCardList(Index).TaskCardNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    Session("sender") = "Authorization"
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If

                EditRecord(mId)
                ''If IsNothing(Request.QueryString("GChildPage7")) Then
                ''    Dim str As String
                ''    str = "openledgersame('wfTaskCard.aspx?TaskBackPage=index.aspx');"
                ''    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
                ''Else
                ''    Response.Redirect("wfTaskCard.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&TaskBackPage=index.aspx" & "&ID=" & ID.ToString)
                ''End If

            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardList.PageSize * dgTaskCardList.PageIndex
                Dim mId As Guid = mTaskCardList(Index).ID

                'Added By Vikrant On 02-Jan-2014 For All02012014
                If (Not IsInRole(UserRightsFor.urfDelete)) Then
                    SetSession()
                    MarkLog(Util.Action.Delete, "TaskCard", User.Identity.Name & " is not Authorized User to delete " & mTaskCardList(Index).TaskCardNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(mId)

        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        'Added by Vikrant on 20-July-2011
        MarkLog(Util.Action.Close, "TaskCard", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        Session.Remove("wfTaskCardList.TaskCard")


        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If


        If IsNothing(Request.QueryString("GChildPage7")) Then
            ClearAll()
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Else
            ClearAll()
            Response.Redirect(Request.QueryString("GChildPage7") & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnTaskMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnTaskMaster.Click
        DataFieldBind()
        SetControl()
        upnlGridView.Update()
        upnlResult.Update()
    End Sub
#End Region


#Region "Service Methods"
    '<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    'Public Shared Function GetInspTypeIntervalSearchList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
    '    Dim itemlist As InspTypeIntervalAutoComplete
    '    itemlist = InspTypeIntervalAutoComplete.GeInspTypeIntervalList(prefixText)
    '    If count = 0 Then
    '        Return (From c As InspTypeIntervalAutoComplete.InspTypeIntervalAutoCompleteInfo In itemlist
    '           Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.InspTypeInterval, "")).ToArray
    '    Else
    '        Return (From c As InspTypeIntervalAutoComplete.InspTypeIntervalAutoCompleteInfo In itemlist
    '           Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.InspTypeInterval, "")).Take(count).ToArray
    '    End If
    'End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetInspTypeIntervalSearchList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mInspTypeIntervalAutoComplete As InspTypeIntervalAutoComplete = InspTypeIntervalAutoComplete.GeInspTypeIntervalList(prefixText)
        If count = 0 Then
            Return (From c As InspTypeIntervalAutoComplete.InspTypeIntervalAutoCompleteInfo In mInspTypeIntervalAutoComplete
               Select c.InspTypeInterval).ToArray
        Else
            Return (From c As InspTypeIntervalAutoComplete.InspTypeIntervalAutoCompleteInfo In mInspTypeIntervalAutoComplete
               Select c.InspTypeInterval).Take(count).ToArray
        End If
    End Function
#End Region

   
End Class