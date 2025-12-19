

'AJAX Created By     :   Saylee
'Dated               :   27-Aug-2015


Public Class wfPendingAuditScheduleListForExecution_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditScheduleListForExecution As AuditScheduleListForExecution
    Protected mAuditExecution As AuditExecution
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAuditExecution = Session("mAuditExecution")
        mAuditScheduleListForExecution = Session("mAuditScheduleListForExecution")
    End Sub
    Private Sub SetSession()
        Session("mAuditExecution") = mAuditExecution
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "Audit Schedule List : " & mAuditScheduleListForExecution.Count & " Record(s) found."
        btnBackTop.Visible = mAuditScheduleListForExecution.Count > 25
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetObject(ByVal Index As Int32)
        mAuditExecution.AuditScheduleID = mAuditScheduleListForExecution(Index).ID
        mAuditExecution.StartDate = txtAuditDate.Text
        mAuditExecution.AuditNo = mAuditScheduleListForExecution(Index).AuditText
        mAuditExecution.Reference = mAuditScheduleListForExecution(Index).Reference
        mAuditExecution.Description = mAuditScheduleListForExecution(Index).Description
        mAuditExecution.OtherInformation = mAuditScheduleListForExecution(Index).OtherInformation
        mAuditExecution.ToMailIDs = mAuditScheduleListForExecution(Index).ToMailIDs
        mAuditExecution.CCMailIDs = mAuditScheduleListForExecution(Index).CCMailIDs

        For i As Integer = 0 To mAuditScheduleListForExecution(Index).AuditScheduleTasks.Count - 1
            mAuditExecution.AuditExecutionTasks.Add(mAuditExecution.ID)
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditTaskID = mAuditScheduleListForExecution(Index).AuditScheduleTasks(i).AuditTaskID
            If AppSettings("ClientCode") = "SAA" Or AppSettings("ClientCode") = "ABD" Then 'Added By Prashant on 28-Jun-2022, ABD code addedby saylee on 28-Sep-2022 as they need satisfactory
                mAuditExecution.AuditExecutionTasks.CurrentItem.TaskStatusID = 1
            End If
        Next

        mAuditExecution.IsAttachmentAdded = mAuditScheduleListForExecution(Index).IsAttachmentAdded

        If mAuditExecution.IsAttachmentAdded = True Then
            Dim tmpFileAttach As FileAttach = FileAttach.GetAttachment(mAuditScheduleListForExecution(Index).ID)
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAuditExecution.ID, tmpFileAttach.ImageFile, tmpFileAttach.Size, tmpFileAttach.Extension)
            'Session("mFileAttach") = mFileAttach 'Commented by Sachin file Attachment
            Session("mFileAttachOnAuditExecution") = mFileAttach 'Added by Sachin file Attachment
        End If


        Session("mAuditExecution") = mAuditExecution
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAuditScheduleListForExecution = AuditScheduleListForExecution.GetAuditScheduleListForExecution(txtAuditDate.Text.ToString, CInt(txtUpcomingDays.Text))
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
        dgAuditScheduleList.DataSource = mAuditScheduleListForExecution
        DataBind()


    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            txtAuditDate.Text = New SmartDate(Today.Date.ToString).FormattedText
            DataFieldBind()
            SetTitle()
        End If
    End Sub

    Private Sub dgAuditScheduleList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAuditScheduleList.PageIndexChanging
        dgAuditScheduleList.PageIndex = e.NewPageIndex
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
        dgAuditScheduleList.DataSource = mAuditScheduleListForExecution
        dgAuditScheduleList.DataBind()
    End Sub
    Private Sub dgAuditScheduleList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditScheduleList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgAuditScheduleList.PageIndex * dgAuditScheduleList.PageSize
                Dim mID As Guid = mAuditScheduleListForExecution(Index).ID
                SetObject(Index)
                Response.Redirect("wfAuditExecution_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfAuditScheduleListForExecution.aspx" & "&AuditNo=" & mAuditScheduleListForExecution(Index).AuditNo)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        dgAuditScheduleList.PageIndex = 0
        mAuditScheduleListForExecution = AuditScheduleListForExecution.GetAuditScheduleListForExecution(txtAuditDate.Text.ToString, CInt(txtUpcomingDays.Text))
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
        lblResult.Text = "Audit Schedule List : " & mAuditScheduleListForExecution.Count & " Record(s) found."
        dgAuditScheduleList.DataSource = mAuditScheduleListForExecution
        dgAuditScheduleList.DataBind()

        upnlResult.Update()
        upnlGrid.Update()

    End Sub

    Private Sub dgAuditScheduleList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditScheduleList.Sorting
        mAuditScheduleListForExecution.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgAuditScheduleList.DataSource = mAuditScheduleListForExecution
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
        dgAuditScheduleList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region

   
End Class