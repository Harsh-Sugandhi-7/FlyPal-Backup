Public Class wfModelList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mModelList As ModelList
    Public mModel As Model
    Public SearchForText As String
    Dim Index As Int32
    Dim EventLogID As Guid
    Public mMachineNameValueList As MachineNameValueList
    Public mMachine As Machine

    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyStatusList As tmpAssemblyStatusList

    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList

    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList

    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mModelList = CType(Session("mModelList"), ModelList)
        mModel = CType(Session("mModel"), Model)
        SearchForText = CType(Session("SearchForText"), String)
        Session("NewPage") = "False"
    End Sub
    Private Sub SetSession()
        Session("mModelList") = mModelList
        Session("mModel") = mModel
        Session("SearchForText") = SearchForText
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfModelList_Ajax.aspx?" Then
            Session.Remove("mModelList")
            Session.Remove("mModel")
            Session.Remove("SearchForText")
            Session.Remove("mMachine")
            Session.Remove("mAssemblyStatus")
            Session.Remove("mAssemblyMonitorServiceStatus")
            Session.Remove("mAssemblyMonitorInspStatus")
            Session.Remove("mAssemblyMonitorModStatus")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub FindNow(Optional ByVal mModelName As String = "")
        'clear the obj and grid for new search
        mModel = Nothing
        dgModelList.DataSource = Nothing
        'get the new list
        mModelList = ModelList.GetModelList(0, mModelName)
        'bind the list to the grid
        dgModelList.DataSource = mModelList
        Session("mModelList") = mModelList
        dgModelList.DataBind()
        lblResult.Text = "List of Model as per criteria: " & mModelList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlTitle.Update()
        upnlResult.Update()
    End Sub
    Private Sub CallFindNow(ByVal Index As Int32)
        FindNow(txtFor.Text.Trim)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim Name As String
                        Try
                            Dim mModel As Model
                            Session("sender") = ""
                            mModel = CType(Session("mModel"), Model)
                            Name = mModel.Name
                            Model.DeleteModel(mModel.ID)
                            DatafieldBind()
                            SetControl()
                            upnlGrid.Update()
                            upnlResult.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                Dim mService As String = ""
                                If mModelList(Name).ModelMonitorServiceCount > 0 Or mModelList(Name).ModelMonitorInspCount > 0 Or mModelList(Name).ModelMonitorModCount > 0 Then
                                    mService = "Service/Insp/Directive"
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, mService, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Model", "Can't delete : " + Name + " is Currently in use", Util.ErrorType.NoError, mModel.ID, EventLogID)
                            End If
                            DatafieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Model", Name, Util.ErrorType.NoError, mModel.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetControl()
        txtFor.Text = Session("SearchForText")
        dgModelList.DataBind()
    End Sub

#End Region

#Region " DataBinding "
    Private Sub DatafieldBind()
        mModelList = ModelList.GetModelList()
        dgModelList.DataSource = mModelList
        Session("mModelList") = mModelList
        lblResult.Text = "List of Model as per criteria: " & mModelList.Count & " Record(s) found."
        DataBind()
        If IsNothing(Session("SearchForText")) Then txtFor.Text = "" Else txtFor.Text = CType(Session("SearchForText"), String)

        txtFor.Text = Session("SearchForText")
        Session("SearchForText") = txtFor.Text
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            Session("MiddleFrame") = "wfModelList_Ajax.aspx?"
            DatafieldBind()
            SetControl()

            'Added by Harsh on 15th July 2024 for FLYPAL 1757
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Model") Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Mark As Favourite",
                                                    "MarkAsFavourite();",
                                                    True)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Remove From Favourite",
                                                    "RemoveFromFavourite();",
                                                    True)

            End If

        End If

    End Sub

    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgModelList.PageIndex = 0
        Session("SearchForText") = txtFor.Text
        CallFindNow(Index)
    End Sub
    Private Sub txtFor_TextChanged(sender As Object, e As System.EventArgs) Handles txtFor.TextChanged
        dgModelList.PageIndex = 0
        Session("SearchForText") = txtFor.Text
        CallFindNow(Index)
    End Sub
    Private Sub dgModelList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgModelList.PageSize * dgModelList.PageIndex
                Dim mId As Guid = mModelList(Index).ID
                Dim mName As String = mModelList(mID).ModelName
                If (Not User.IsInRole("ModelView") And Not User.IsInRole("ModelEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit " + mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                Session("ActiveTabIndex") = 0
                mModel = Model.GetModel(mID)
                'mModel.BeginEdit()
                Session("mModel") = mModel
                Dim ModelDetail As String = "Name. : " + mModel.Name
                MarkLog(Util.Action.Edit, "Model", ModelDetail, Util.ErrorType.NoError, mModel.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfModelCreation_Ajax.aspx?BackPage=Index.aspx');", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgModelList.PageSize * dgModelList.PageIndex
                Dim mId As Guid = mModelList(Index).ID
                Dim mName As String = mModelList(mID).ModelName
                If (Not User.IsInRole("ModelDelete")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to delete " + mName, Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    Exit Sub
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                    mModel = Model.GetModel(mID)
                    Session("mModel") = mModel
                End If
            Case "ModelMonitorServiceClick"
                Dim Idx As Int32
                Idx = CInt(e.CommandArgument) + dgModelList.PageIndex * dgModelList.PageSize
                Dim mID As Guid = mModelList(Idx).ID
                Dim mName As String = mModelList(Idx).ModelName
                If (Not User.IsInRole("ModelView") And Not User.IsInRole("ModelEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit " + mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                mModel = Model.GetModel(mID)
                Session("mModel") = mModel
                Session("OpenFromModelCreation") = "True"
                Session("ModelIDFromModelCreation") = mModel.ID
                Session("ModelNameFromModelCreation") = mModel.Name

                mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString)
                If mMachineNameValueList.Count > 0 Then
                    mMachine = Machine.GetMachine(mMachineNameValueList(0).ID)
                    Session("mMachine") = mMachine

                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMachine.AssemblyStatus.ID)
                    Session("mAssemblyStatus") = mAssemblyStatus

                    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mModel.ID, mMachine.HourType)
                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                    '''Else
                    '''    MSGBoxCtrl.show(MSGBox.Message_title.AircraftNotConfigured, MSGBox.Message_text.AircraftNotConfigured, "", MsgBoxStyle.OkOnly, "")
                    '''    Exit Sub

                End If

                Dim ModelDetail As String = "Name. : " + mModel.Name
                MarkLog(Util.Action.Edit, "Model", ModelDetail, Util.ErrorType.NoError, mModel.ID, EventLogID)

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModelMonitorServiceListWindow", "OpenModelMonitorServiceListWindow();", True)
            Case "ModelMonitorInspClick"
                Dim Idx As Int32
                Idx = CInt(e.CommandArgument) + dgModelList.PageIndex * dgModelList.PageSize
                Dim mID As Guid = mModelList(Idx).ID
                Dim mName As String = mModelList(Idx).ModelName
                If (Not User.IsInRole("ModelView") And Not User.IsInRole("ModelEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit " + mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                mModel = Model.GetModel(mID)
                Session("mModel") = mModel
                Session("OpenFromModelCreation") = "True"
                Session("ModelIDFromModelCreation") = mModel.ID
                Session("ModelNameFromModelCreation") = mModel.Name

                mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString)
                If mMachineNameValueList.Count > 0 Then
                    mMachine = Machine.GetMachine(mMachineNameValueList(0).ID)
                    Session("mMachine") = mMachine

                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMachine.AssemblyStatus.ID)
                    Session("mAssemblyStatus") = mAssemblyStatus

                    mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate.ToString, mModel.ID, mMachine.HourType)
                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                    '''Else
                    '''    MSGBoxCtrl.show(MSGBox.Message_title.AircraftNotConfigured, MSGBox.Message_text.AircraftNotConfigured, "", MsgBoxStyle.OkOnly, "")
                    '''    Exit Sub
                End If

                Dim ModelDetail As String = "Name. : " + mModel.Name
                MarkLog(Util.Action.Edit, "Model", ModelDetail, Util.ErrorType.NoError, mModel.ID, EventLogID)
               
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModelMonitorInspListWindow", "OpenModelMonitorInspListWindow();", True)
            Case "ModelMonitorModClick"
                Dim Idx As Int32
                Idx = CInt(e.CommandArgument) + dgModelList.PageIndex * dgModelList.PageSize
                Dim mID As Guid = mModelList(Idx).ID
                Dim mName As String = mModelList(Idx).ModelName
                If (Not User.IsInRole("ModelView") And Not User.IsInRole("ModelEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit " + mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                mModel = Model.GetModel(mID)
                Session("mModel") = mModel
                Session("OpenFromModelCreation") = "True"
                Session("ModelIDFromModelCreation") = mModel.ID
                Session("ModelNameFromModelCreation") = mModel.Name

                mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString)
                If mMachineNameValueList.Count > 0 Then
                    mMachine = Machine.GetMachine(mMachineNameValueList(0).ID)
                    Session("mMachine") = mMachine

                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMachine.AssemblyStatus.ID)
                    Session("mAssemblyStatus") = mAssemblyStatus

                    mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mModel.ID, mMachine.HourType)
                    Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                    '''Else
                    '''    MSGBoxCtrl.show(MSGBox.Message_title.AircraftNotConfigured, MSGBox.Message_text.AircraftNotConfigured, "", MsgBoxStyle.OkOnly, "")
                    '''    Exit Sub
                End If

                Dim ModelDetail As String = "Name. : " + mModel.Name
                MarkLog(Util.Action.Edit, "Model", ModelDetail, Util.ErrorType.NoError, mModel.ID, EventLogID)
               
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModelMonitorModListWindow", "OpenModelMonitorModListWindow();", True)
        End Select
    End Sub
    Private Sub dgModelList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgModelList.PageIndexChanging
        dgModelList.PageIndex = e.NewPageIndex
        dgModelList.DataSource = mModelList
        Session("mModelList") = mModelList
        dgModelList.DataBind()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        SetSession()
        mModel = Model.NewModel(Guid.NewGuid, 1)
        If (Not User.IsInRole("ModelNew") And mModel.IsNew) Or (Not User.IsInRole("ModelEdit") And Not mModel.IsNew) Then
            SetSession()
            MarkLog(Util.Action.[New], "Model", User.Identity.Name & " is not Authorized User to add ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Session("mModel") = mModel
        MarkLog(Util.Action.[New], "Model", "", Util.ErrorType.NoError, mModel.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfModelCreation_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Session.Remove("ModelIDFromModelCreation")
        Session.Remove("ModelNameFromModelCreation")
        Session.Remove("SearchForText")
        Session.Remove("OpenFromModelCreation")
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub hdnBtnModelMonitorInspList_Click(sender As Object, e As System.EventArgs) Handles hdnBtnModelMonitorInspList.Click,
                                                                                                  hdnBtnModelMonitorModList.Click,
                                                                                                  hdnBtnModelMonitorServiceList.Click
        DatafieldBind()
        upnlGrid.Update()
        upnlTitle.Update()
        upnlResult.Update()
        CallFindNow(Index)

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1757
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "Model")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "Model")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    'End

#End Region

End Class