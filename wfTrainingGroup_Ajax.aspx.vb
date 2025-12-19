'Added by Shital
'Dated 8-Oct-2021

Partial Class wfTrainingGroup_Ajax
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Protected mDesignationList As DesignationList
    Protected mDesignation As Designation
    Public mTrainingList As TrainingList
    Public mTrainingGroup As GroupTraining
    Public mTrainingGroupList As GroupTrainingList
    Public mTrainingGroupListByName As GroupTrainingList
    Public Type As Int16 = 0
    Dim EventLogID As Guid

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTrainingGroup = CType(Session("mTrainingGroup"), GroupTraining)
        mTrainingGroupList = CType(Session("mTrainingGroupList"), GroupTrainingList)
        mTrainingList = Session("mTrainingList")
        mTrainingGroupListByName = Session("mTrainingGroupListByName")
        Type = Session("Type")
    End Sub
    Private Sub SetSession()
        Session("mTrainingGroup") = mTrainingGroup
        Session("mTrainingGroupList") = mTrainingGroupList
        Session("Type") = Type
    End Sub
    Private Sub NewRecord()
        mTrainingGroup = GroupTraining.NewGroupTraining(Guid.NewGuid)
        mTrainingGroupList = GroupTrainingList.GetGroupTrainingList()
        SetSession()
        txtGroupName.Enabled = True
        txtGroupName.Text = ""
        txtGroupName.DataBind()
        cmbDesignation.SelectedIndex = 0
        cmbDesignation.DataBind()
        chkTrainingList.DataBind()
        chkTrainingList.DataSource = mTrainingList
        chkTrainingList.DataBind()
        upnlGroupDetails.Update()
        Session("Edit") = False
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        SetFocus(txtGroupName)
        txtGroupName.Enabled = True
    End Sub
    Private Sub DeleteRecord(ByVal mName As String)
        Session("mGroupName") = mName
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    
    Public Function Save() As Boolean
        Try
            If Session("Edit") = False Then
                mTrainingGroupList = GroupTrainingList.GetGroupTrainingList()
                If Not mTrainingGroupList Is Nothing Then
                    If mTrainingGroupList.Contains(txtGroupName.Text.ToString) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, "can not add duplicate Group Name", MsgBoxStyle.OkOnly, "")
                        Exit Function
                    End If
                End If
            End If

            For i As Integer = 0 To chkTrainingList.Items.Count - 1
                If chkTrainingList.Items(i).Selected Then
                    mTrainingGroup.TrainingID = New Guid(chkTrainingList.Items(i).Value)
                    mTrainingGroup.GroupName = txtGroupName.Text
                    mTrainingGroup.DesignationID = New Guid(cmbDesignation.SelectedValue)

                    If mTrainingGroup.IsValid Then
                        mTrainingGroup.Save()

                        If Not mTrainingGroupListByName Is Nothing Then
                            If Not mTrainingGroupListByName.Contains(New Guid(chkTrainingList.Items(i).Value), mTrainingGroup.GroupName) Then
                                mTrainingGroup = GroupTraining.NewGroupTraining(Guid.NewGuid)
                            End If
                        Else
                            mTrainingGroup = GroupTraining.NewGroupTraining(Guid.NewGuid)
                        End If


                        If txtGroupName.Enabled = True Then
                            SetFocus(txtGroupName)
                        End If
                        MarkLog(Util.Action.Save, "Group", mTrainingGroup.GroupName, Util.ErrorType.HandledError, mTrainingGroup.ID, EventLogID)

                    Else

                        Dim str As String = ""
                        For j As Integer = 0 To mTrainingGroup.GetBrokenRulesCollection.Count - 1
                            str = str + mTrainingGroup.GetBrokenRulesCollection(j).Description + "<Br>"
                        Next
                        Return False
                    End If
                Else
                    If Not mTrainingGroupListByName Is Nothing Then
                        If mTrainingGroupListByName.Contains(New Guid(chkTrainingList.Items(i).Value), txtGroupName.Text) Then
                            GroupTraining.DeleteGroupTraining(Name:=mTrainingGroupListByName.Item(0).GroupName, TrainingID:=mTrainingGroupListByName.Item(New Guid(chkTrainingList.Items(i).Value)).TrainingID.ToString)
                        End If
                    End If
                End If
            Next


            DataFieldBind()

        Catch ex As SqlException
            If ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 50000 Then
                MSGBoxCtrl.show("Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Return True
        End Try
    End Function
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
                            mTrainingGroup = Session("mTrainingGroup")
                            '  mGroupName = Session("mGroupName")
                            GroupTraining.DeleteGroupTraining(Name:=Session("mGroupName"))
                            NewRecord()
                            DataFieldBind()
                            lblResult.Text = "Training Group List: " & mTrainingGroupList.Count & " Record(s) Found."
                            lblTitle.Text = "Training Group  [New]"
                            upnlTitle.Update()
                            upnlGroupDetails.Update()
                            upnlGridView.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                Dim stringInfo As String = ""
                                If ex.Message.Contains("tabIssue") Then
                                    stringInfo = "Issue"
                                ElseIf ex.Message.Contains("tabReceiptItem") Then
                                    stringInfo = "Receipt Item."
                                ElseIf ex.Message.Contains("tabReceipt") Then
                                    stringInfo = "Receipt."
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            upnlGroupDetails.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Group", mTrainingGroup.GroupName, Util.ErrorType.NoError, mTrainingGroup.ID, EventLogID)
                            End If
                        End Try
                    End If

                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        NewRecord()
                        DataFieldBind()
                        upnlGroupDetails.Update()
                    End If

                Case MsgBoxResult.Ok
                    Session("sender") = ""

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""

            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfTrainingGroup_Ajax.aspx?") <= 0 Then
            Session.Remove("mTrainingGroup")
            Session.Remove("mTrainingGroupList")
            Session.Remove("mLocationList")

            Session.Remove("Type")
            Session.Remove("mVendor")
            Session.Remove("mCitylist")
            Session.Remove("mCity")
            Session.Remove("New")
        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "txtGroupName" Then
            If txtGroupName.Text = "" Then
                CustValidator.ErrorMessage = "Group Name Required."
                e.IsValid = False
            Else
                Dim IsSelected As Boolean = False
                For i As Integer = 0 To chkTrainingList.Items.Count - 1
                    If chkTrainingList.Items(i).Selected Then
                        IsSelected = True
                        Exit For
                    End If
                Next
                If IsSelected = False Then
                    CustValidator.ErrorMessage = "Select At least one Training from the List."
                    e.IsValid = False
                End If
            End If
        ElseIf CustValidator.ControlToValidate = "cmbDesignation" Then
            If cmbDesignation.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Select Designation from the List."
                e.IsValid = False
            End If


        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub setObject()
        txtGroupName.Text = mTrainingGroupListByName.Item(0).GroupName
        cmbDesignation.SelectedValue = mTrainingGroupListByName.Item(0).DesignationID.ToString

        'For j As Integer = 0 To mTrainingGroupListByName.Count - 1
        For i As Integer = 0 To chkTrainingList.Items.Count - 1
            If mTrainingGroupListByName.Contains(New Guid(chkTrainingList.Items(i).Value), mTrainingGroupListByName.Item(0).GroupName) Then
                chkTrainingList.Items(i).Selected = True
            Else
                chkTrainingList.Items(i).Selected = False
            End If
        Next
        ' Next
    End Sub
    Private Sub DataFieldBind()
        mTrainingList = TrainingList.GetTrainingList()
        Session("mTrainingList") = mTrainingList
        chkTrainingList.DataSource = mTrainingList
        chkTrainingList.DataBind()
        mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
        Session("mDesignationList") = mDesignationList
        cmbDesignation.DataSource = mDesignationList
        cmbDesignation.DataBind()

        mTrainingGroupList = GroupTrainingList.GetGroupTrainingList()
        Session("mTrainingGroupList") = mTrainingGroupList
        dgTrainingGroupList.DataSource = mTrainingGroupList
        dgTrainingGroupList.DataBind()

        lblResult.Text = "Training Group List: " & mTrainingGroupList.Count & " Record(s) Found."

        upnlGridView.Update()
        DataBind()

    End Sub

#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()
        Type = Val(Request.QueryString("Type"))

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            If txtGroupName.Enabled = True Then
                SetFocus(txtGroupName)
            End If
            If Session("sender") = "" And Session("New") <> "True" Then

                Session("MiddleFrame") = "wfTrainingGroup_Ajax.aspx?"
                NewRecord()
            Else

            End If
            DataFieldBind()

            'Added by Harsh on 15th July 2024 for FLYPAL 1757
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "TrainingGroup") Then

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
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("TrainingGroupNew") And mTrainingGroup.IsNew) Or (Not User.IsInRole("TrainingGroupEdit") And Not mTrainingGroup.IsNew) Then
            setObject()
            SetSession()
            MarkLog(Util.Action.Save, "Group", User.Identity.Name & " is not Authorized User to save " & mTrainingGroup.GroupName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            If Save() Then


                DataFieldBind()
                lblTitle.Text = "Training Group [New]"
                lblResult.Text = "Training Group  List: " & mTrainingGroupList.Count & " Record(s) Found."
                upnlTitle.Update()
                upnlGroupDetails.Update()
                upnlGridView.Update()
            End If
            NewRecord()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgTrainingGroupList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingGroupList.PageIndexChanging
        dgTrainingGroupList.PageIndex = e.NewPageIndex
        dgTrainingGroupList.DataSource = mTrainingGroupList
        Session("mTrainingGroupList") = mTrainingGroupList
        dgTrainingGroupList.DataBind()
    End Sub
    Private Sub dgTrainingGroupList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingGroupList.RowCommand

        Dim Idx As Int32
        Dim mName As String
        Select Case e.CommandName
            Case "EditRec"
            
                Idx = CInt(e.CommandArgument) + dgTrainingGroupList.PageIndex * dgTrainingGroupList.PageSize
                mName = mTrainingGroupList(Idx).GroupName

                If (Not User.IsInRole("TrainingGroupView") And Not User.IsInRole("TrainingGroupEdit")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Edit, "Group", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                mTrainingGroupListByName = GroupTrainingList.GetGroupTrainingList(Name:=mName.ToString, TrainingsRequired:=1)
                Session("mTrainingGroupListByName") = mTrainingGroupListByName
                setObject()
                Session("Edit") = True
                txtGroupName.DataBind()
                chkTrainingList.DataBind()
                cmbDesignation.DataBind()

                MarkLog(Util.Action.Edit, "Group", mTrainingGroup.GroupName, Util.ErrorType.NoError, mTrainingGroup.ID, EventLogID)

                lblTitle.Text = "Training Group [" & mName & "...]"


                lblResult.Text = "Training Group List: " & mTrainingGroupList.Count & " Record(s) Found."
                upnlGroupDetails.Update()
                upnlTitle.Update()
                upnlGridView.Update()

            Case "DeleteRec"

                Idx = CInt(e.CommandArgument) + dgTrainingGroupList.PageIndex * dgTrainingGroupList.PageSize
                mName = mTrainingGroupList(Idx).GroupName


                If (Not User.IsInRole("TrainingGroupDelete")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Delete, "Group", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                End If
                DeleteRecord(mName)

        End Select
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtGroupName.Enabled = True Then
            SetFocus(txtGroupName)
        End If
        MarkLog(Util.Action.[New], "Group", "", Util.ErrorType.NoError, mTrainingGroup.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        lblTitle.Text = " Training Group [New]"
        'New Addition By Yogita on 10-Dec-2007
        lblResult.Text = "Training Group List: " & mTrainingGroupList.Count & " Record(s) Found."
        upnlGroupDetails.Update()
        upnlTitle.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1757
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "TrainingGroup")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "TrainingGroup")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    'End

#End Region

    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click

        Session("sender") = ""
        MarkLog(Util.Action.Close, "Training Group", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")

    End Sub
End Class
