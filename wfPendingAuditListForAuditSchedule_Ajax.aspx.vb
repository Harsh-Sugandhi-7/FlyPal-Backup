'Added by vikrant on 24-Aug-2015

Public Class wfPendingAuditListForAuditSchedule_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mAuditList As AuditList
    Dim mAuditTypeList As AuditTypeList
    Public BackPage As String
    Dim DateIndex, FromDate, ToDate As String
    Dim SearchIndex, SearchText, AuditTypeID As String
    Dim mAuditSchedule As AuditSchedule
    Dim mAuditScheduleList As AuditScheduleList
    Dim mFileAttach As FileAttach
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditList = Session("mAuditList")
        mAuditSchedule = Session("mAuditSchedule")
        SearchIndex = Session("SearchIndex")
        AuditTypeID = Session("AuditTypeID")
        SearchText = Session("SearchText")
        mAuditTypeList = Session("mAuditTypeList")
        mAuditScheduleList = CType(Session("mAuditScheduleList"), AuditScheduleList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditList")
        Session.Remove("SearchIndex")
        Session.Remove("AuditTypeID")
        Session.Remove("SearchText")
        Session.Remove("mAuditTypeList")
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "List of Audit as per criteria :" & mAuditList.Count & " Record(s) found."
    End Sub
    Private Sub FindNow(Optional ByVal AuditTypeID As Integer = 0, Optional ByVal SearchText As String = "")
        mAuditList = AuditList.GetAuditList(AuditTypeID, SearchText)
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()
        SetTitle()
    End Sub
    Private Sub SetObject(ByVal Index As Int32)
        mAuditSchedule.AuditID = mAuditList(Index).ID
        If mAuditList(Index).IsNextSchedule = True Then
            Dim mPreviousAuditSchedule As PreviousAuditSchedule
            mPreviousAuditSchedule = PreviousAuditSchedule.GetPreviousAuditSchedule(mAuditList(Index).ID)
            If Not mPreviousAuditSchedule.AuditNo Is Nothing Then
                If mPreviousAuditSchedule.Frequency = 0 And mPreviousAuditSchedule.NextSchedule = True Then
                    mAuditSchedule.ScheduleDate = Today.Date
                Else
                    mAuditSchedule.ScheduleDate = mPreviousAuditSchedule.NextAuditDate
                End If
            Else
                mAuditSchedule.ScheduleDate = Today.Date
            End If
            mAuditSchedule.NextAuditDate = DateAdd(DateInterval.Month, mAuditList(Index).Frequency, mAuditSchedule.ScheduleDate)
        End If

        If mAuditList(Index).IsAttachmentAdded Then
            mAuditSchedule.IsAttachmentAdded = True
            Dim tmpFileAttach As FileAttach = FileAttach.GetAttachment(mAuditList(Index).ID)
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAuditSchedule.ID, tmpFileAttach.ImageFile, tmpFileAttach.Size, tmpFileAttach.Extension)
            Session("mFileAttach") = mFileAttach
        End If
        'mAuditSchedule.OtherInformation = mAuditList(Index).OtherInformation
        'mAuditSchedule.ImageFile = mAuditList(Index).ImageFile
        'mAuditSchedule.ImageSize = mAuditList(Index).ImageSize
        'mAuditSchedule.FileExtension = mAuditList(Index).FileExtension
        Dim mAudit As Audit = Audit.GetChildAudit(mAuditList(Index).ID)
        For Each AuditMasterTask As AuditMasterTask In mAudit.AuditMasterTasks
            mAuditSchedule.AuditScheduleTasks.Add(mAuditSchedule.ID)
            mAuditSchedule.AuditScheduleTasks.CurrentItem.AuditTaskID = AuditMasterTask.AuditTaskID
        Next
        Session("mAuditSchedule") = mAuditSchedule
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetControl()
        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"

    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        'btnCloseTop.Visible = mAuditList.Count > 10
        txtSearchText.Visible = IIf(SearchIndex = 1, True, False)
        cmbAuditType.Visible = IIf(SearchIndex = 2, True, False)
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        AuditTypeID = IIf(cmbAuditType.SelectedIndex <= 0, 0, cmbAuditType.SelectedValue)
        SearchText = IIf(txtSearchText.Text = "", "", txtSearchText.Text)

        Session("SearchIndex") = SearchIndex
        Session("AuditTypeID") = AuditTypeID
        Session("SearchText") = SearchText
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        mAuditList = AuditList.GetAuditList()
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()

        mAuditTypeList = AuditTypeList.GetAuditTypeList("(All)")
        cmbAuditType.DataSource = mAuditTypeList
        Session("mAuditTypeList") = mAuditTypeList
        cmbAuditType.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            cmbSearch.Focus()
            DataFieldBind()
            SetControl()
            ControlVisibility(SearchIndex, DateIndex)
        End If
    End Sub
    Private Sub dgAuditList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim index As Integer = CInt(e.CommandArgument) + dgAuditList.PageIndex * dgAuditList.PageSize
                If mAuditList(index).IsNextSchedule = False And mAuditScheduleList.Contains(mAuditList(index).ID) Then
                    MSGBoxCtrl.show("One Time!", "This audit is already scheduled.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mAuditScheduleList.Contains(mAuditList(index).ID) Then
                    If mAuditList(index).IsNextSchedule = True And CDate(mAuditScheduleList.Item(mAuditList(index).ID, "").ScheduleDate) = Today.Date And mAuditList(index).Frequency = 0 Then
                        MSGBoxCtrl.show("Alert!", "This audit is already scheduled for today date.", "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If
                SetObject(index)
                Response.Redirect("wfAuditSchedule_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfPendingAuditListForAuditSchedule_Ajax.aspx&Type=1")
        End Select
    End Sub
    Private Sub dgAuditList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAuditList.PageIndexChanging
        dgAuditList.PageIndex = e.NewPageIndex
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbAuditType.SelectedIndex = 0
        txtSearchText.Text = ""
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
        upnlGrid.Update()
        upnlActionButton.Update()
    End Sub
    Private Sub txtSearchText_TextChanged(sender As Object, e As System.EventArgs) Handles txtSearchText.TextChanged
        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        upnlGrid.Update()
        upnlActionButton.Update()
    End Sub
    Private Sub cmbAuditType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAuditType.SelectedIndexChanged
        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        upnlGrid.Update()
        upnlActionButton.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgAuditList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditList.Sorting
        mAuditList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()
    End Sub
#End Region

End Class