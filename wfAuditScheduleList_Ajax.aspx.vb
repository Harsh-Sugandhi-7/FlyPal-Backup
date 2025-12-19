'AJAX Conversion by vikrant on 19-Aug-2015

Public Class wfAuditScheduleList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditScheduleList As AuditScheduleList
    Public mAuditSchedule As AuditSchedule
    Dim SearchIdx, DateIdx, FromDate, ToDate, SearchTxt As String
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Dim mScheduleDetail As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditScheduleList = Session("mAuditScheduleList")
        mAuditSchedule = Session("mAuditSchedule")
        SearchIdx = Session("SearchIdx")
        DateIdx = Session("DateIdx")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchTxt = Session("SearchTxt")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditScheduleList")
        Session.Remove("mAuditSchedule")
        Session.Remove("SearchIdx")
        Session.Remove("DateIdx")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("SearchTxt")
        Session.Remove("mFileAttach")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfAuditScheduleList_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub NewRecord()
        mAuditSchedule = AuditSchedule.NewAuditSchedule
        Session("mAuditSchedule") = mAuditSchedule
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mAuditSchedule = AuditSchedule.GetAuditSchedule(mID)
        Session("mAuditSchedule") = mAuditSchedule
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAuditSchedule = AuditSchedule.GetAuditSchedule(mID)
        Session("mAuditSchedule") = mAuditSchedule
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "")
        'Get List From the Database as per Criteria  
        mAuditScheduleList = AuditScheduleList.GetAuditScheduleList(FromDate, ToDate, SearchTxt)
        'Set DataSource of the Grid
        dgAuditSchedule.DataSource = mAuditScheduleList
        Session("mAuditScheduleList") = mAuditScheduleList
        dgAuditSchedule.DataBind()
        SetGrid()
        lblResult.Text = "List of Audit Schedule as per criteria :" & mAuditScheduleList.Count & " Record(s) found."
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
                            mAuditSchedule = Session("mAuditSchedule")
                            mScheduleDetail = "Audit No. :" + mAuditSchedule.AuditNo + " Dated : " + mAuditSchedule.ScheduleDateFormatted
                            GetAttachment(mAuditSchedule.ID, mAuditSchedule.IsAttachmentAdded)
                            AuditSchedule.DeleteAuditSchedule(mAuditSchedule.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            MarkLog(Util.Action.Delete, "Audit Schedule", mScheduleDetail, Util.ErrorType.NoError, mAuditSchedule.ID, EventLogID) 'Changed by Vikrant on 22-July-2011
                            SetControl()
                            setPeriod(SearchIdx)
                            SetGrid()
                            upnlGrid.Update()
                            upnlActionBtn.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Audit Schedule", "Can't delete :" & mScheduleDetail & " is Currently in use", Util.ErrorType.NoError, mAuditSchedule.ID, EventLogID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            'If msgCount = 0 Then
                            '    MarkLog(Util.Action.Delete, "AuditSchedule", mAuditSchedule.Name, Util.ErrorType.NoError, mAuditSchedule.ID)
                            'End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIdx)

        FindNow(txtFromDate.Text, txtToDate.Text, SearchTxt)
        dgAuditSchedule.DataBind()
        cmbSearch.SelectedIndex = SearchIdx
        cmbDateRange.SelectedIndex = DateIdx
        txtSearchText.Text = SearchTxt

        ControlVisibility1(cmbSearch.SelectedIndex, DateIdx)
    End Sub
    Private Sub ControlVisibility1(ByVal SearchIdx As Int32, Optional ByVal DateIdx As Int32 = 0)
        cmbDateRange.Visible = IIf(SearchIdx = 1, True, False)
        If SearchIdx = 1 And DateIdx = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIdx = 1 And (DateIdx = 1 Or DateIdx = 2 Or DateIdx = 3 Or DateIdx = 4 Or DateIdx = 5 Or DateIdx = 7) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            'txtFromDate.Text = ""    'Commented by sachin on 21-Nov-2023 as debugger hits on textchenge event of txtfromdate (txtSearchText_TextChanged) handles txtfromdate. 
            'txtToDate.Text = ""      'Commented by sachin on 21-Nov-2023 as debugger hits on textchenge event of txtfromdate (txtSearchText_TextChanged) handles txtfromdate. 
        End If
        cmbDateRange.Visible = IIf(cmbSearch.SelectedIndex = 1, True, False)
        txtSearchText.Visible = IIf(cmbSearch.SelectedIndex = 2, True, False)
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"

    End Sub
    Private Sub ControlVisibility(ByVal DateIdx As Int32)
        If DateIdx = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIdx = 1 Or DateIdx = 2 Or DateIdx = 3 Or DateIdx = 4 Or DateIdx = 5) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-1-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("1-1-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIdx = 6 And FromDate <> "", FromDate, Today.Date)
                ToDate = IIf(DateIdx = 6 And ToDate <> "", ToDate, Today.Date)
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetGrid()
        'Ajay H 29-09-2023
        'Dim P As Integer
        'For j As Integer = 0 To dgAuditSchedule.Rows.Count - 1
        '    P = CType(Me.dgAuditSchedule.Rows(j).Cells(14).Text, Boolean)
        '    If P = False Then
        '        dgAuditSchedule.Rows(j).Cells(13).Enabled = False
        '    End If
        'Next
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIdx = IIf(IsNothing(SearchIdx), 0, SearchIdx)
        DateIdx = IIf(IsNothing(DateIdx), 0, DateIdx)
        SearchTxt = Session("SearchTxt")

        mAuditScheduleList = AuditScheduleList.GetAuditScheduleList(FromDate, ToDate, SearchTxt)
        dgAuditSchedule.DataSource = mAuditScheduleList
        Session("mAuditScheduleList") = mAuditScheduleList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)      'Added by Vikrant on 22-July-2011
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfAuditScheduleList_Ajax.aspx?"
            setPeriod(0)
            DataFieldBind()
            SetControl()
            If cmbDateRange.Enabled = True Then
                cmbSearch.Focus()
            End If
            lblResult.Text = "List of Audit Schedule as per criteria :" & mAuditScheduleList.Count & " Record(s) found."
            SetGrid()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgAuditSchedule.PageIndex = 0

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt

        FindNow(FromDate, ToDate, SearchTxt)
        upnlGrid.Update()
    End Sub
    Private Sub dgAuditSchedule_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditSchedule.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("AuditScheduleView") And Not User.IsInRole("AuditScheduleEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Idx As Int32 = CInt(e.CommandArgument) + dgAuditSchedule.PageIndex * dgAuditSchedule.PageSize
                Dim mID As Guid = mAuditScheduleList(Idx).ID
                EditRecord(mID)

                'Changed by Vikrant on 22-July-2011
                mScheduleDetail = "Audit No. :" + mAuditScheduleList(mID).AuditText + " Dated : " + mAuditScheduleList(mID).ScheduleDateFormatted
                MarkLog(Util.Action.Edit, "Audit Schedule", mScheduleDetail, Util.ErrorType.NoError, mAuditSchedule.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAuditSchedule_Ajax.aspx?BackPage=index.aspx');", True)
            Case "DeleteRec"
                If (Not User.IsInRole("AuditScheduleDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Idx As Int32 = CInt(e.CommandArgument) + dgAuditSchedule.PageIndex * dgAuditSchedule.PageSize
                Dim mID As Guid = mAuditScheduleList(Idx).ID
                DeleteRecord(mID)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                Dim Idx As Int32 = CInt(e.CommandArgument) + dgAuditSchedule.PageIndex * dgAuditSchedule.PageSize
                Dim mID As Guid = mAuditScheduleList(Idx).ID
                Dim IsAttachmentAdded As Boolean = mAuditScheduleList(Idx).IsAttachmentAdded
                GetAttachment(mID, IsAttachmentAdded)

                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgAuditSchedule_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAuditSchedule.PageIndexChanging
        dgAuditSchedule.PageIndex = e.NewPageIndex
        dgAuditSchedule.DataSource = mAuditScheduleList
        Session("mAuditScheduleList") = mAuditScheduleList
        dgAuditSchedule.DataBind()
        SetGrid()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        If (Not User.IsInRole("AuditScheduleNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewRecord()
        MarkLog(Util.Action.[New], "Audit Schedule", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingAuditListForAuditSchedule_Ajax.aspx?BackPage=Index.aspx&');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim DateIdx As Int32 = IIf(cmbDateRange.SelectedIndex >= 0, cmbDateRange.SelectedIndex, 0)
        ControlVisibility(DateIdx)
        setPeriod(DateIdx)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If

        dgAuditSchedule.PageIndex = 0

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt

        FindNow(FromDate, ToDate, SearchTxt)
        upnlGrid.Update()
    End Sub
    Private Sub dgAuditSchedule_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditSchedule.Sorting
        mAuditScheduleList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgAuditSchedule.DataSource = mAuditScheduleList
        Session("mAuditScheduleList") = mAuditScheduleList
        dgAuditSchedule.DataBind()
        SetGrid()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDateRange.SelectedIndex = 0
        txtSearchText.Text = ""

        Dim DateIdx As Int32 = IIf(cmbDateRange.SelectedIndex >= 0 And cmbDateRange.Visible, cmbDateRange.SelectedIndex, 0)
        ControlVisibility1(cmbSearch.SelectedIndex, DateIdx)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If

        dgAuditSchedule.PageIndex = 0

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt

        FindNow(FromDate, ToDate, SearchTxt)
        upnlGrid.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Session.Remove("mFileAttach")
    End Sub
    Private Sub txtSearchText_TextChanged(sender As Object, e As System.EventArgs) Handles txtSearchText.TextChanged, txtFromDate.TextChanged, txtToDate.TextChanged
        dgAuditSchedule.PageIndex = 0

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt

        FindNow(FromDate, ToDate, SearchTxt)
        upnlGrid.Update()
    End Sub
#End Region

    
  
End Class