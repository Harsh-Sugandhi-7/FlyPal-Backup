
'Created By     :   Saylee
'Dated          :   5-Feb-2010
'Modified By    :   6-Apr-2010

Partial Class wfAuditInfoListForAuditSchedule
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ' Protected WithEvents txtAuditSearchText As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mAuditList As AuditList
    Public mAudit As Audit
    Protected mAuditSchedule As AuditSchedule
    Dim mAuditScheduleList As AuditScheduleList
    Dim SearchIndex, AuditSearchText, AuditTypeID, DateIndex As String
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Public mAuditScheduleNo As String
    Dim mScheduleDetail As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAuditSchedule = Session("mAuditSchedule")
        mAuditList = Session("mAuditList")
        mAudit = Session("mAudit")
        mAuditScheduleList = CType(Session("mAuditScheduleList"), AuditScheduleList)

        SearchIndex = Session("SearchIndex")
        AuditTypeID = Session("AuditTypeID")
        AuditSearchText = Session("AuditSearchText")
    End Sub
    Private Sub SetSession()
        Session("mAuditSchedule") = mAuditSchedule
        Session("mAuditList") = mAuditList
        Session("mAudit") = mAudit
        Session("SearchIndex") = SearchIndex
        Session("AuditTypeID") = AuditTypeID
        Session("AuditSearchText") = AuditSearchText
        Session("mAuditScheduleList") = mAuditScheduleList
    End Sub
    Private Sub NewRecord()
        mAudit = Audit.NewAudit()
        Session("mAudit") = mAudit
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mAudit = Audit.GetChildAudit(mID)
        Session("mAudit") = mAudit
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = 0
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Session("sender") = ""
                            mAudit = Session("mAudit")
                            Audit.DeleteAudit(mAudit.ID)
                            'Changed by Vikrant on 22-July-2011
                            mAuditScheduleNo = mAuditList.Item(mAuditList.CurrentIndex).AuditNo
                            mScheduleDetail = "Audit No. :" + mAuditScheduleNo + " Audit Type : " + mAuditList.Item(mAuditList.CurrentIndex).AuditTypeName
                            MarkLog(Util.Action.Delete, "Audit Schedule", mScheduleDetail, Util.ErrorType.NoError, mAuditList.Item(mAuditList.CurrentIndex).ID, EventLogID)
                            Response.Redirect("wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                'Changed by Vikrant on 22-July-2011
                                mAuditScheduleNo = mAuditList.Item(mAuditList.CurrentIndex).AuditNo
                                mScheduleDetail = "Audit No. :" + mAuditScheduleNo + " Audit Type : " + mAuditList.Item(mAuditList.CurrentIndex).AuditTypeName
                                MarkLog(Util.Action.Delete, "Audit Schedule", "Can't delete :" & mScheduleDetail & " is Currently in use", Util.ErrorType.NoError, mAuditSchedule.ID, EventLogID)

                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            'If msgCount = 0 Then
                            '    MarkLog(Util.Action.Delete, "AuditList", Audit.Name, Util.ErrorType.NoError, Audit.ID)
                            'End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfAuditInfoListForAuditSchedule.aspx?BackPage=" & Request.QueryString("BackPage")
        Session("sender") = "Delete"
        msg1.Show()
        mAudit = Audit.GetChildAudit(mID)
        Session("mAudit") = mAudit
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        txtAuditSearchText.Visible = IIf(SearchIndex = 1, True, False)
        cmbAuditType.Visible = IIf(SearchIndex = 2, True, False)
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetObject(ByVal Index As Int32)
        mAuditSchedule.AuditID = mAuditList(Index).ID

        If mAuditList(Index).IsNextSchedule = True Then
            Dim mPreviousAuditSchedule As PreviousAuditSchedule
            mPreviousAuditSchedule = PreviousAuditSchedule.GetPreviousAuditSchedule(mAuditList(Index).ID)
            If Not mPreviousAuditSchedule.AuditNo Is Nothing Then
                mAuditSchedule.ScheduleDate = mPreviousAuditSchedule.NextAuditDate
            Else
                mAuditSchedule.ScheduleDate = Today.Date
            End If
            mAuditSchedule.NextAuditDate = DateAdd(DateInterval.Month, mAuditList(Index).Frequency, mAuditSchedule.ScheduleDate)
        End If

        mAuditSchedule.OtherInformation = mAuditList(Index).OtherInformation
        mAuditSchedule.ImageFile = mAuditList(Index).ImageFile
        mAuditSchedule.ImageSize = mAuditList(Index).ImageSize
        mAuditSchedule.FileExtension = mAuditList(Index).FileExtension

        Session("mAuditSchedule") = mAuditSchedule
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "List of Audit as per criteria :" & mAuditList.Count & " Record(s) found."
    End Sub
    Private Sub FindNow(Optional ByVal AuditTypeID As Integer = 0, Optional ByVal AuditSearchText As String = "")
        'Get List From the Database as per Criteria  
        mAuditList = AuditList.GetAuditList(AuditTypeID, AuditSearchText)
        'Set DataSource of the Grid
        dgPendingList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgPendingList.DataBind()
        SetTitle()
    End Sub
    Private Sub setVariables()

        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        AuditTypeID = IIf(cmbAuditType.SelectedIndex <= 0, 0, cmbAuditType.SelectedValue)
        AuditSearchText = IIf((txtAuditSearchText.Text Is Nothing) Or (txtAuditSearchText.Text = ""), "", txtAuditSearchText.Text)

        Session("SearchIndex") = SearchIndex
        Session("AuditTypeID") = AuditTypeID
        Session("AuditSearchText") = AuditSearchText
    End Sub
    Private Sub SetControl()

        cmbSearch.SelectedIndex = SearchIndex
        cmbAuditType.SelectedValue = AuditTypeID
        txtAuditSearchText.Text = AuditSearchText

        FindNow(AuditTypeID, AuditSearchText)
        dgPendingList.DataBind()
        dgPendingList.CurrentPageIndex = 0
    End Sub
    Private Sub SetGrid()
        Dim P As Integer
        Dim lb As LinkButton 'ButtonColumn 
        For j As Integer = 0 To dgPendingList.Items.Count - 1
            P = CType(Me.dgPendingList.Items.Item(j).Cells(10).Text, Integer)
            If P <= 0 Then
                lb = CType(dgPendingList.Items.Item(j).Cells(8).FindControl("LinkButton1"), LinkButton)
                lb.Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAuditList = AuditList.GetAuditList()
        dgPendingList.DataSource = mAuditList
        Session("mAuditList") = mAuditList

        cmbAuditType.DataSource = AuditTypeList.GetAuditTypeList("(All)")
        cmbAuditType.DataBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)      'Added by Vikrant on 22-July-2011
        If Not IsPostBack Then
            DataFieldBind()
            SetControl()
            ControlVisibility(SearchIndex, DateIndex)
        End If
        setFocus(cmbSearch)
        SetGrid()
        MessageBoxResult()
        lblResult.Text = "Audit List : " & mAuditList.Count & " Record(s) found."
    End Sub
    Private Sub dgPendingList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPendingList.ItemCommand
        Dim Index As Int32 = e.Item.ItemIndex + dgPendingList.CurrentPageIndex * dgPendingList.PageSize
        Select Case e.CommandName
            Case "Edit"
                If (Not User.IsInRole("AuditScheduleView") And Not User.IsInRole("AuditScheduleEdit")) Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Idx As Int32 = e.Item.ItemIndex + dgPendingList.CurrentPageIndex * dgPendingList.PageSize
                Dim mID As New Guid(e.Item.Cells(0).Text)
                EditRecord(mID)
                'Changed by Vikrant on 22-July-2011
                mAuditScheduleNo = CStr(e.Item.Cells(1).Text)
                mScheduleDetail = "Audit No. :" + mAuditScheduleNo + " Audit Type : " + mAuditList(mID).AuditTypeName
                MarkLog(Util.Action.Edit, "Audit Schedule", mScheduleDetail, Util.ErrorType.HandledError, mAuditList.Item(mAuditList.CurrentIndex).ID, EventLogID)
                setVariables()
                Response.Redirect("wfAudit.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfAuditInfoListForAuditSchedule.aspx")

            Case "Delete"
                If (Not User.IsInRole("AuditScheduleDelete")) Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Idx As Int32 = e.Item.ItemIndex + dgPendingList.CurrentPageIndex * dgPendingList.PageSize
                Dim mID As New Guid(e.Item.Cells(0).Text)
                'If (Not User.IsInRole("AuditDelete")) Then
                '    'setObject()
                '    SetSession()
                '    ' MarkLog(Util.Action.Delete, "AuditList", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                DeleteRecord(mID)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mAudit = Audit.GetChildAudit(New Guid(e.Item.Cells(0).Text))
                If mAudit.ImageSize > 0 Then

                    Dim path As String = AppSettings("DOCPath") & StrName & mAudit.FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mAudit.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mAudit.ImageFile, 0, mAudit.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "<script language=Javascript>openFile();</script>"
                        ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)
                    End If
                Else
                    'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                End If

            Case "Select"
                If mAuditList(Index).IsNextSchedule = False And mAuditScheduleList.Contains(mAuditList(Index).ID) Then
                    SetSession()
                    Dim msg1 As New SIMsgBox(Page, "One Time!", "This audit is already scheduled.", "", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfAuditInfoListForAuditSchedule.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                    Exit Sub
                End If
                SetObject(Index)
                Response.Redirect("wfAuditSchedule.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfAuditInfoListForAuditSchedule.aspx" & "&AuditNo=" & mAuditList(Index).AuditNo)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Session.Remove("SearchIndex")
        Session.Remove("AuditTypeID")
        Session.Remove("AuditSearchText")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnAddTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        NewRecord()
        'If (Not User.IsInRole("AuditNew") And mAudit.IsNew) Or (Not User.IsInRole("AuditEdit") And Not mAudit.IsNew) Then
        '    'setObject()
        '    SetSession()
        '    'MarkLog(Util.Action.Save, "AuditList", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfAuditList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        ' MarkLog(Util.Action.[New], "AuditList", "", Util.ErrorType.NoError, Audit.ID)
        setVariables()
        Response.Redirect("wfAudit.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfAuditInfoListForAuditSchedule.aspx")
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPendingList.CurrentPageIndex = 0
        setVariables()
        FindNow(AuditTypeID, AuditSearchText)
        lblResult.Text = "Audit List : " & mAuditList.Count & " Record(s) found."
        dgPendingList.DataSource = mAuditList
        dgPendingList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgPendingList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPendingList.SortCommand
        mAuditList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgPendingList.DataSource = mAuditList
        Session("mAuditList") = mAuditList
        dgPendingList.DataBind()
        SetGrid()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbAuditType.SelectedIndex = 0
        txtAuditSearchText.Text = ""
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        'setPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
#End Region

End Class



