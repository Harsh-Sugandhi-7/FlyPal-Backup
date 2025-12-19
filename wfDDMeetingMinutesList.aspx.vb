

'Created By Saylee on 4-Jul-2017

Imports System.Linq
Imports System.Collections.Generic

Public Class wfDDMeetingMinutesList
    Inherits System.Web.UI.Page

#Region " Variable Declaration"
    Private mMeeting As Meeting
    Private mMeetingList As MeetingList
    Dim mTitle As String = String.Empty
    Dim IDate As String = String.Empty
    Dim IDForEventLog As Guid
    Dim EventLogID As Guid
    Dim RecordsToShow As Integer
    Dim DateIndex As String = ""
    Public FromDate As String = "1-1-1900"
    Public ToDate As String = "1-1-2200"
#End Region

#Region " Methods"
    
    Private Sub EnableLinks()
        'If Not mMeetingList Is Nothing Then
        '    If RecordsToShow < mMeetingList.Count Then
        '        lnkShowAllRecordsTop.Enabled = True
        '        lnkShowAllRecordsTop.ForeColor = Color.Red
        '    Else
        '        lnkShowAllRecordsTop.Enabled = False
        '        lnkShowAllRecordsTop.ForeColor = Color.Gray
        '    End If
        'End If
    End Sub
    Public Sub GetSession()
        mMeetingList = Session("mMeetingList")
        mTitle = Session("mTitle")
        IDate = Session("IDate")
        RecordsToShow = Session("RecordsToShow")

        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        DateIndex = Session("DateIndex")
       

    End Sub
    Public Sub RemoveSession()
        Session.Remove("mMeetingList")
        Session.Remove("mTitle")
        Session.Remove("IDate")
        Session.Remove("RecordsToShow")

        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("DateIndex")
        mTitle = Nothing
        IDate = Nothing

    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If

    End Sub
    Private Sub SetControl()
        SetPeriod(DateIndex)
       
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
      
        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
      

        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate

      
        mMeetingList = MeetingList.GetMeetingList(FromDate, ToDate)
        cmbDate.SelectedIndex = DateIndex

        'Dim List = (From StatusInfo As Meeting In mMeetingList
        '                                           Select StatusInfo).ToList.Take(RecordsToShow)
        dgMeetingList.DataSource = mMeetingList
        dgMeetingList.DataBind()
        Session("mMeetingList") = mMeetingList
        ControlVisibility(DateIndex)

        lblResult.Text = "List of Meeting(s) as per criteria :" & mMeetingList.Count.ToString & " Record(s) found."
        'If RecordsToShow < mMeetingList.Count Then
        '    lblResult.Text = "List of Meeting(s) as per criteria : " & RecordsToShow.ToString & " of " & mMeetingList.Count & " Record(s) shown."
        'Else
        '    lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
        'End If
    End Sub
    Private Sub SetTitle()
        ' lbltitle.Text = "List of Meeting(s) [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"
        upnlTitle.Update()
    End Sub
    Private Sub SetPeriod(ByVal index As Int32) 'CNDC
        Select Case index
            Case 0 ' All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
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
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
        Session("FromDate") = txtFromDate.Text
        Session("ToDate") = txtToDate.Text
    End Sub
    Public Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex

        DataBind()
        cmbDate.SelectedIndex = DateIndex
     
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        Dim MeetingDetail As String = ""
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mMeeting = Session("mMeeting")
                            IDForEventLog = mMeeting.ID
                            MeetingDetail = "Meeting" + mMeeting.Title
                            Meeting.DeleteMeeting(mMeeting.ID)
                            DataFieldBind()
                            mMeetingList = MeetingList.GetMeetingList(FromDate, ToDate)
                            cmbDate.SelectedIndex = DateIndex

                            'Dim List = (From StatusInfo As Meeting In mMeetingList
                            '                                           Select StatusInfo).ToList.Take(RecordsToShow)
                            dgMeetingList.DataSource = mMeetingList
                            dgMeetingList.DataBind()
                            Session("mMeetingList") = mMeetingList
                            lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
                            'If RecordsToShow < mMeetingList.Count Then
                            '    lblResult.Text = "List of Meeting(s) as per criteria : " & RecordsToShow.ToString & " of " & mMeetingList.Count & " Record(s) shown."
                            'Else
                            '    lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
                            'End If

                            upnlgrid.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "Meeting", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Meeting", "Can't delete : " & MeetingDetail & " is Currently used in Meeting", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                                'End
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Meeting", MeetingDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Public Sub DeletedRecord(ID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mMeeting = Meeting.GetMeeting(ID)
        Session("mMeeting") = mMeeting
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfDDMeetingMinutesList.aspx?"
            RecordsToShow = dgMeetingList.PageSize
            Session("RecordsToShow") = RecordsToShow
            DataFieldBind()
            SetControl()
            cmbDate.Focus()
        End If
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        SetPeriod(DateIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex

        mMeetingList = MeetingList.GetMeetingList(FromDate, ToDate)
        Session("mMeetingList") = mMeetingList
        dgMeetingList.DataSource = mMeetingList
        dgMeetingList.DataBind()
        lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found."
        upnlgrid.Update()

    End Sub
    Protected Sub txtTodate_TextChanged(sender As Object, e As System.EventArgs)

        RecordsToShow = dgMeetingList.PageSize
        Session("RecordsToShow") = RecordsToShow

        dgMeetingList.PageIndex = 0


        mMeetingList = MeetingList.GetMeetingList(txtFromDate.Text, txtToDate.Text)
        Session("mMeetingList") = mMeetingList

        'Dim List = (From StatusInfo As Meeting In mMeetingList
        '                                               Select StatusInfo).ToList.Take(RecordsToShow)
        dgMeetingList.DataSource = mMeetingList
        DataBind()
        lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
        'If RecordsToShow < mMeetingList.Count Then
        '    lblResult.Text = "List of Meeting(s) as per criteria : " & RecordsToShow.ToString & " of " & mMeetingList.Count & " Record(s) shown."
        'Else
        '    lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
        'End If
        upnlgrid.Update()
        EnableLinks()
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click, btnAddNewTop.Click
        mMeeting = Meeting.NewMeeting()
        Session("mMeeting") = mMeeting
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMeetingonent_Ajax.aspx?BackPage=wfMeetingonentList_Ajax.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenMeetingWindow", "OpenMeetingWindow();", True)
    End Sub
    Private Sub dgMeetingList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMeetingList.Sorting
        'RecordsToShow = dgMeetingList.PageSize
        RecordsToShow = Session("RecordsToShow")
        mMeetingList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMeeting") = mMeetingList
        'Dim List = (From StatusInfo As Meeting In mMeetingList
        '                                              Select StatusInfo).ToList.Take(RecordsToShow)
        dgMeetingList.DataSource = mMeetingList
        dgMeetingList.DataBind()
    End Sub
    Private Sub dgMeetingList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMeetingList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mMeeting = Meeting.GetMeeting(ID)
                Session("mMeeting") = mMeeting
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenMeetingWindow", "OpenMeetingWindow();", True)
            Case "Remove"
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                DeletedRecord(ID)
        End Select
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub hdnBtnMeeting_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMeeting.Click
        ' RecordsToShow = dgMeetingList.PageSize
        RecordsToShow = Session("RecordsToShow")

        dgMeetingList.PageIndex = 0

        mMeetingList = MeetingList.GetMeetingList(txtFromDate.Text, txtToDate.Text)
        cmbDate.SelectedIndex = DateIndex

        'Dim List = (From StatusInfo As Meeting In mMeetingList
        '                                           Select StatusInfo).ToList.Take(RecordsToShow)
        dgMeetingList.DataSource = mMeetingList
        dgMeetingList.DataBind()
        Session("mMeetingList") = mMeetingList

        lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
        

        'If RecordsToShow < mMeetingList.Count Then
        '    lblResult.Text = "List of Meeting(s) as per criteria : " & RecordsToShow.ToString & " of " & mMeetingList.Count & " Record(s) shown."
        'Else
        '    lblResult.Text = "List of Meeting(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
        'End If

        upnlgrid.Update()
        EnableLinks()
    End Sub
    Private Sub lnkShowAllRecordsTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAllRecordsTop.Click
        RecordsToShow = mMeetingList.Count
        Session("RecordsToShow") = RecordsToShow
        dgMeetingList.DataSource = mMeetingList
        dgMeetingList.DataBind()
        lblResult.Text = "List of Component(s) as per criteria : " & mMeetingList.Count.ToString & " Record(s) found. "
        lnkShowAllRecordsTop.Enabled = False
        lnkShowAllRecordsTop.ForeColor = Color.Gray

        upnlgrid.Update()
        upnlActionBtn.Update()
    End Sub
#End Region

End Class