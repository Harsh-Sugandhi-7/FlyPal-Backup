'Created By     :   Saylee
'Dated          :   19-Aug-2015



Public Class wfAuditList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditList As AuditList
    Public mAudit As Audit
    Public BackPage As String
    Dim DateIndex, FromDate, ToDate As String
    Dim SearchIndex, SearchText, AuditTypeID As String
    Protected mAuditSchedule As AuditSchedule
    Dim mFileAttach As FileAttach
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditList = Session("mAuditList")
        mAudit = Session("Audit")
        mAuditSchedule = Session("mAuditSchedule")

        SearchIndex = Session("SearchIndex")
        AuditTypeID = Session("AuditTypeID")
        SearchText = Session("SearchText")
    End Sub
    Private Sub SetSession()
        Session("mAuditList") = mAuditList
        Session("mAudit") = mAudit
        Session("mAuditSchedule") = mAuditSchedule

        Session("SearchIndex") = SearchIndex
        Session("AuditTypeID") = AuditTypeID
        Session("SearchText") = SearchText
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfAuditList_AJAX.aspx?" Then
            Session.Remove("mAuditList")
            Session.Remove("mAudit")
            Session.Remove("SearchIndex")
            Session.Remove("AuditTypeID")
            Session.Remove("SearchText")
        End If
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "List of Audit as per criteria :" & mAuditList.Count & " Record(s) found."
    End Sub
    Private Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        Page.RegisterStartupScript("FocusScript", str)
    End Sub
    Private Sub NewRecord()
        mAudit = Audit.NewAudit()
        Session("mAudit") = mAudit
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mAudit = Audit.GetChildAudit(mID)
        Session("mAudit") = mAudit
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfAuditList.aspx?BackPage=" & Request.QueryString("BackPage")
        'Session("sender") = "Delete"
        'msg1.Show()
        SetGrid()
        upnldgAuditList.Update()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAudit = Audit.GetChildAudit(mID)
        Session("mAudit") = mAudit
    End Sub

    Private Sub FindNow(Optional ByVal AuditTypeID As Integer = 0, Optional ByVal SearchText As String = "")
        'Get List From the Database as per Criteria  
        mAuditList = AuditList.GetAuditList(AuditTypeID, SearchText)
        'Set DataSource of the Grid
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()
        SetTitle()
    End Sub

    Private Sub SetGrid()
        'Ajay H 26-09-2023
        'Dim B As Boolean
        'For j As Integer = 0 To dgAuditList.Rows.Count - 1
        '    B = CType(Me.dgAuditList.Rows.Item(j).Cells(13).Text, Boolean)
        '    If B = False Then
        '        dgAuditList.Rows.Item(j).Cells(12).Enabled = False
        '    End If
        'Next
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean) 'Added By Vikrant On 01-Dec-2014
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded)
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0


        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mAudit = Session("mAudit")
                            Audit.DeleteAudit(mAudit.ID)
                            DataFieldBind()
                            ControlVisibility(SearchIndex, DateIndex)
                            SetFocus(cmbSearch)
                            SetTitle()
                            SetGrid()
                            upnldgAuditList.Update()
                            upnlResult.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            SetGrid()
                            upnldgAuditList.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            'If msgCount = 0 Then
                            '    MarkLog(Util.Action.Delete, "AuditList", Audit.Name, Util.ErrorType.NoError, Audit.ID)
                            'End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetControl()
        '''FindNow(txtFromDate.Value.ToString, txtToDate.Value.ToString)
        '''dgAuditList.DataBind()

        '        setPeriod(DateIndex)

        cmbSearch.SelectedIndex = SearchIndex
        cmbAuditType.SelectedValue = AuditTypeID

        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If

        FindNow(AuditTypeID, SearchText)
        dgAuditList.DataBind()
        dgAuditList.PageIndex = 0
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"

    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        'btnAddTop.Visible = mAuditList.Count > 10
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
        ' Session("LeadAuditorID") = LeadAuditorID
        Session("SearchText") = SearchText
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        mAuditList = AuditList.GetAuditList()
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()

        cmbAuditType.DataSource = AuditTypeList.GetAuditTypeList("(All)")
        cmbAuditType.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfAuditList_AJAX.aspx?"
            'setPeriod(0)
            DataFieldBind()
            SetControl()
            ControlVisibility(SearchIndex, DateIndex)
            SetFocus(cmbSearch)
            SetTitle()
            SetGrid()
        End If
      
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
        dgAuditList.DataBind()
    End Sub
    Private Sub dgAuditList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Idx As Int32 = e.CommandArgument.ToString + dgAuditList.PageIndex * dgAuditList.PageSize
                Dim mID As Guid = mAuditList(Idx).ID
                If (Not User.IsInRole("AuditView") And Not User.IsInRole("AuditEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "AuditList", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty.ToString)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfAuditList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

                    Exit Sub
                End If
                EditRecord(mID)
                setVariables()
                ' MarkLog(Util.Action.Edit, "AuditList", Audit.Name, Util.ErrorType.NoError, Audit.ID)
                'Dim str As String
                'str = "<script language='javascript'>openledgersame('wfAudit.aspx?BackPage=index.aspx&" & "');</script>"
                'Page.RegisterStartupScript("OpenScript", str)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAudit_Ajax.aspx?BackPage=index.aspx&" & "');", True)

            Case "DeleteRec"
                Dim Idx As Int32 = e.CommandArgument.ToString + dgAuditList.PageIndex * dgAuditList.PageSize
                Dim mID As Guid = mAuditList(Idx).ID

                If (Not User.IsInRole("AuditDelete")) Then
                    SetSession()
                    ' MarkLog(Util.Action.Delete, "AuditList", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfAudit.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

                    Exit Sub
                End If
                DeleteRecord(mID)
            Case "ViewRec"
                Dim Idx As Int32 = e.CommandArgument.ToString + dgAuditList.PageIndex * dgAuditList.PageSize
                Dim mID As Guid = mAuditList(Idx).ID
                Dim mIsAttachemntAdded As Boolean = mAuditList(mID).IsAttachmentAdded
                SetGrid()
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub
  
    Private Sub dgAuditList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgAuditList.PageIndexChanged
        dgAuditList.PageIndex = e.NewPageIndex
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        NewRecord()
        If (Not User.IsInRole("AuditNew") And mAudit.IsNew) Or (Not User.IsInRole("AuditEdit") And Not mAudit.IsNew) Then
            SetSession()
            ' Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfAuditList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

            Exit Sub
        End If
        ' MarkLog(Util.Action.[New], "AuditList", "", Util.ErrorType.NoError, Audit.ID)
        setVariables()
        'Dim str As String
        'str = "<script language='javascript'>openledgersame('wfAudit.aspx?BackPage=Index.aspx&');</script>"
        'Page.RegisterStartupScript("OpenScript", str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAudit_Ajax.aspx?BackPage=index.aspx&" & "');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbAuditType.SelectedIndex = 0
        txtSearchText.Text = ""
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        'setPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            SetFocus(cmbSearch)
        End If

        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
        dgAuditList.DataBind()
        SetGrid()
        upnldgAuditList.Update()
        upnlTitle.Update()
        upnlResult.Update()
        upnlButtonsTop.Update()
    End Sub
    Private Sub txtSearchText_TextChanged(sender As Object, e As System.EventArgs) Handles txtSearchText.TextChanged
        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
        dgAuditList.DataBind()
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        SetGrid()
        upnldgAuditList.Update()
        upnlTitle.Update()
        upnlResult.Update()
        upnlButtonsTop.Update()
    End Sub
    Private Sub cmbAuditType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAuditType.SelectedIndexChanged
        dgAuditList.PageIndex = 0
        setVariables()
        FindNow(AuditTypeID, SearchText)
        dgAuditList.DataBind()
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        SetGrid()
        upnldgAuditList.Update()
        upnlTitle.Update()
        upnlResult.Update()
        upnlButtonsTop.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgAuditList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditList.Sorting
        mAuditList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgAuditList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgAuditList.DataBind()
        SetGrid()
        upnldgAuditList.Update()
    End Sub
#End Region


  
   

End Class