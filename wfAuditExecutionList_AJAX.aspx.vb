
'AJAX Created By     :   Saylee
'Dated               :   27-Aug-2015


Public Class wfAuditExecutionList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditExecutionList As AuditExecutionList
    Public mAuditExecution As AuditExecution
    Dim SearchIdx, DateIdx, FromDate, ToDate, SearchTxt As String     'A1
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Dim mExecutionDetail As String
    Dim mFileAttach As FileAttach

    Dim ShowOpenClosed As Boolean  'Added by Ajay 16-Nov-2022  
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditExecutionList = Session("mAuditExecutionList")
        mAuditExecution = Session("mAuditExecution")
        SearchIdx = Session("SearchIdx")
        DateIdx = Session("DateIdx")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchTxt = Session("SearchTxt")
        mFileAttach = Session("mFileAttach")
        ShowOpenClosed = Session("ShowOpenClosed") 'Added by Ajay 16-Nov-2022
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditExecutionList")
        Session.Remove("mAuditExecution")
        Session.Remove("SearchIdx")
        Session.Remove("DateIdx")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("SearchTxt")
        Session.Remove("mFileAttach")
        Session.Remove("ShowOpenClosed") 'Added by Ajay 16-Nov-2022
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfAuditExecutionList_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
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
    Private Sub NewRecord()
        mAuditExecution = AuditExecution.NewAuditExecution
        Session("mAuditExecution") = mAuditExecution
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mAuditExecution = AuditExecution.GetAuditExecution(mID)
        Session("mAuditExecution") = mAuditExecution
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAuditExecution = AuditExecution.GetAuditExecution(mID)
        Session("mAuditExecution") = mAuditExecution
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "", Optional ByVal ShowOpenClosed As Boolean = False) 'Ajay Added by ShowOpenClosed 17-Nov-2022

        'Get List From the Database as per Criteria  
        mAuditExecutionList = AuditExecutionList.GetAuditExecutionList(SearchTxt, , FromDate, ToDate, , ShowOpenClosed) 'Ajay Added by ShowOpenClosed 17-Nov-2022
        'Set DataSource of the Grid
        dgAuditExecution.DataSource = mAuditExecutionList
        Session("mAuditExecutionList") = mAuditExecutionList
        dgAuditExecution.DataBind()
        SetGrid()
        lblResult.Text = "List of Audit Compliances as per criteria : " & mAuditExecutionList.Count & " Record(s) found."
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
                            mAuditExecution = Session("mAuditExecution")
                            mExecutionDetail = "Audit No : " + mAuditExecution.AuditNo + " Start Date : " + mAuditExecution.StartDateFormatted + " Lead Auditor : " + mAuditExecution.AuditorName
                            GetAttachment(mAuditExecution.ID, mAuditExecution.IsAttachmentAdded)
                            AuditExecution.DeleteAuditExecution(mAuditExecution.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            MarkLog(Util.Action.Delete, "Audit Compliances", mExecutionDetail, Util.ErrorType.NoError, mAuditExecution.ID, EventLogID) 'Changed by Vikrant on 22-July-2011
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
                                MarkLog(Util.Action.Delete, "Audit Compliances", "Can't delete :" & mExecutionDetail & " is Currently in use", Util.ErrorType.NoError, mAuditExecution.ID, EventLogID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            'If msgCount = 0 Then
                            '    MarkLog(Util.Action.Delete, "AuditExecution", mAuditExecution.Name, Util.ErrorType.NoError, mAuditExecution.ID)
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

        FindNow(txtFromDate.Text, txtToDate.Text, SearchTxt, ShowOpenClosed) 'Ajay 18-Nov-2022 ShowOpenClosed
        dgAuditExecution.DataBind()
        cmbSearch.SelectedIndex = SearchIdx
        cmbDateRange.SelectedIndex = DateIdx
        txtSearchText.Text = SearchTxt      
        ChkOpenColsed.Checked = ShowOpenClosed 'Ajay 18-Nov-2022
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
            'txtFromDate.ReadOnly = False
            'txtToDate.ReadOnly = False
            'txtFromDate.BackColor = Color.FromKnownColor(KnownColor.White)
            'txtToDate.BackColor = Color.FromKnownColor(KnownColor.White)
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIdx = 1 Or DateIdx = 2 Or DateIdx = 3 Or DateIdx = 4 Or DateIdx = 5) Then
            'txtFromDate.ReadOnly = True
            'txtToDate.ReadOnly = True
            'txtFromDate.BackColor = Color.FromKnownColor(KnownColor.Silver)
            'txtToDate.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
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
        'Dim B As Boolean
        'For j As Integer = 0 To dgAuditExecution.Rows.Count - 1
        '    B = CType(Me.dgAuditExecution.Rows.Item(j).Cells(13).Text, Boolean)
        '    If B = False Then
        '        dgAuditExecution.Rows.Item(j).Cells(12).Enabled = False
        '    End If
        ' Next
        'Ajay H 29-09-2023
        'btnAddTop.Visible = mAuditExecutionList.Count > 10
        'btnCloseTop.Visible = mAuditExecutionList.Count > 10
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIdx = IIf(IsNothing(SearchIdx), 0, SearchIdx)
        DateIdx = IIf(IsNothing(DateIdx), 0, DateIdx)
        SearchTxt = IIf(IsNothing(Session("SearchTxt")), "", Session("SearchTxt")) 'Session("SearchTxt")

        'ShowOpenClosed = (ChkOpenColsed.Checked) 'Ajay 18-Nov-2022

        mAuditExecutionList = AuditExecutionList.GetAuditExecutionList(SearchTxt, , FromDate, ToDate)
        dgAuditExecution.DataSource = mAuditExecutionList
        Session("mAuditExecutionList") = mAuditExecutionList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)      'Added by Vikrant on 22-July-2011
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfAuditExecutionList_Ajax.aspx?"
            setPeriod(0)
            DataFieldBind()
            SetControl()
            If cmbDateRange.Enabled = True Then
                cmbSearch.Focus()
            End If
            lblResult.Text = "List of Audit Compliances as per criteria : " & mAuditExecutionList.Count & " Record(s) found."
            SetGrid()

        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgAuditExecution.PageIndex = 0

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
    Private Sub dgAuditExecution_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditExecution.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("AuditExecutionView") And Not User.IsInRole("AuditExecutionEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Idx As Int32 = CInt(e.CommandArgument) + dgAuditExecution.PageIndex * dgAuditExecution.PageSize
                Dim mID As Guid = mAuditExecutionList(Idx).ID
                EditRecord(mID)

                mExecutionDetail = "Audit No : " + mAuditExecution.AuditNo + " Start Date : " + mAuditExecution.StartDateFormatted + " Lead Auditor : " + mAuditExecution.AuditorName
                MarkLog(Util.Action.Edit, "Audit Complaince", mExecutionDetail, Util.ErrorType.NoError, mAuditExecution.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAuditExecution_Ajax.aspx?BackPage=index.aspx');", True)
            Case "DeleteRec"
                If (Not User.IsInRole("AuditExecutionDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Idx As Int32 = CInt(e.CommandArgument) + dgAuditExecution.PageIndex * dgAuditExecution.PageSize
                Dim mID As Guid = mAuditExecutionList(Idx).ID
                DeleteRecord(mID)
            Case "View"
                Dim Idx As Int32 = e.CommandArgument.ToString + dgAuditExecution.PageIndex * dgAuditExecution.PageSize
                Dim mID As Guid = mAuditExecutionList(Idx).ID
                Dim mIsAttachemntAdded As Boolean = mAuditExecutionList(mID).IsAttachmentAdded
                SetGrid()
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub
    Private Sub dgAuditExecution_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAuditExecution.PageIndexChanging
        dgAuditExecution.PageIndex = e.NewPageIndex
        dgAuditExecution.DataSource = mAuditExecutionList
        Session("mAuditExecutionList") = mAuditExecutionList
        dgAuditExecution.DataBind()
        SetGrid()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        If (Not User.IsInRole("AuditExecutionNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewRecord()
        MarkLog(Util.Action.[New], "Audit Complaince", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingAuditScheduleListForExecution_AJAX.aspx?BackPage=Index.aspx&');", True)
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

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)
        ShowOpenClosed = (ChkOpenColsed.Checked) 'Ajay 18-Nov-2022

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt
        Session("ShowOpenClosed") = ShowOpenClosed 'Ajay 18-Nov-2022

        FindNow(FromDate, ToDate, SearchTxt, ShowOpenClosed)
        upnlGrid.Update()
    End Sub
    Private Sub dgAuditExecution_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditExecution.Sorting
        mAuditExecutionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgAuditExecution.DataSource = mAuditExecutionList
        Session("mAuditExecutionList") = mAuditExecutionList
        dgAuditExecution.DataBind()
        SetGrid()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged  'A2
        cmbDateRange.SelectedIndex = 0
        txtSearchText.Text = ""

        Dim DateIdx As Int32 = IIf(cmbDateRange.SelectedIndex >= 0 And cmbDateRange.Visible, cmbDateRange.SelectedIndex, 0)
        ControlVisibility1(cmbSearch.SelectedIndex, DateIdx)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)
        ShowOpenClosed = (ChkOpenColsed.Checked) 'Ajay 18-Nov-2022

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt
        Session("ShowOpenClosed") = ShowOpenClosed 'Ajay 18-Nov-2022

        FindNow(FromDate, ToDate, SearchTxt, ShowOpenClosed)
        SetGrid()
        upnlGrid.Update()
    End Sub

    Protected Sub txtSearchText_TextChanged(sender As Object, e As EventArgs) Handles txtSearchText.TextChanged, txtFromDate.TextChanged, txtToDate.TextChanged, ChkOpenColsed.CheckedChanged
        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)
        ShowOpenClosed = (ChkOpenColsed.Checked) 'Ajay 18-Nov-2022


        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt
        Session("ShowOpenClosed") = ShowOpenClosed 'Ajay 18-Nov-2022

        FindNow(FromDate, ToDate, SearchTxt, ShowOpenClosed)
        SetGrid()
        upnlGrid.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    ''Added by Ajay 16-Nov-2022

    'Protected Sub ChkOpenColsed_CheckedChanged(sender As Object, e As EventArgs) Handles ChkOpenColsed.CheckedChanged

    '    ShowOpenClosed = (ChkOpenColsed.Checked)
    '    Session("ShowOpenClosed") = ShowOpenClosed
    '    FindNow(, , , ShowOpenClosed)
    '    upnlGrid.Update()
    'End Sub
#End Region


 
End Class