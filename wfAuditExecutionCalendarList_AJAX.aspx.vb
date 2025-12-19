
Imports System
Imports System.IO
Imports System.Data
Imports DayPilot.Utils
Imports DayPilot.Web.Ui.Events
Imports DayPilot.Web.Ui.Enums
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Configuration.ConfigurationManager
Imports DayPilot.Web.Ui.Events.Scheduler

Public Class wfAuditExecutionCalendarList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditCalendar As AuditCalandar

    Dim SearchIdx, DateIdx, FromDate, ToDate, SearchTxt, AuditStatusName, AuditStatusNameIdx As String
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Dim mExecutionDetail As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditCalendar = Session("mAuditCalendar")

        SearchIdx = Session("SearchIdx")
        DateIdx = Session("DateIdx")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchTxt = Session("SearchTxt")
        AuditStatusName = Session("AuditStatusName")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditCalendar")

        Session.Remove("SearchIdx")
        Session.Remove("DateIdx")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("SearchTxt")
        Session.Remove("AuditStatusName")
        Session.Remove("mFileAttach")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfAuditExecutionCalendarList_AJAX.aspx?" Then
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
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

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

        FindNow(txtFromDate.Text, txtToDate.Text, SearchTxt, AuditStatusName)
        cmbSearch.SelectedIndex = SearchIdx
        cmbDateRange.SelectedIndex = DateIdx
        txtSearchText.Text = SearchTxt
        cmbAuditStatusName.SelectedIndex = AuditStatusNameIdx

        ControlVisibility1(cmbSearch.SelectedIndex, DateIdx)
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "", Optional ByVal AuditStatusName As String = "")
        BindDayPilot(FromDate, ToDate, SearchTxt, AuditStatusName)
    End Sub
    Private Sub ControlVisibility1(ByVal SearchIdx As Int32, Optional ByVal DateIdx As Int32 = 0)
        cmbDateRange.Visible = IIf(SearchIdx = 3, True, False)
        If SearchIdx = 3 And DateIdx = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIdx = 3 And (DateIdx = 1 Or DateIdx = 2 Or DateIdx = 3 Or DateIdx = 4 Or DateIdx = 5 Or DateIdx = 7) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Text = ""
            txtToDate.Text = ""
        End If
        cmbDateRange.Visible = IIf(cmbSearch.SelectedIndex = 3, True, False)
        txtSearchText.Visible = IIf(cmbSearch.SelectedIndex = 1, True, False)
        cmbAuditStatusName.Visible = IIf(cmbSearch.SelectedIndex = 2, True, False)
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        SearchTxt = ""
        AuditStatusName = ""
    End Sub
    Private Sub ControlVisibility(ByVal DateIdx As Int32)
        If DateIdx = 6 Then
            txtFromDate.ReadOnly = False
            txtToDate.ReadOnly = False
            txtFromDate.BackColor = Color.FromKnownColor(KnownColor.White)
            txtToDate.BackColor = Color.FromKnownColor(KnownColor.White)

        ElseIf (DateIdx = 1 Or DateIdx = 2 Or DateIdx = 3 Or DateIdx = 4 Or DateIdx = 5) Then
            txtFromDate.ReadOnly = True
            txtToDate.ReadOnly = True
            txtFromDate.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtToDate.BackColor = Color.FromKnownColor(KnownColor.Silver)
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
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIdx = IIf(IsNothing(SearchIdx), 0, SearchIdx)
        DateIdx = IIf(IsNothing(DateIdx), 0, DateIdx)
        SearchTxt = IIf(IsNothing(Session("SearchTxt")), "", Session("SearchTxt")) 'Session("SearchTxt")

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt
        Session("AuditStatusNameIdx") = AuditStatusNameIdx
        Session("AuditStatusName") = AuditStatusName
        'mAuditCalendar = AuditCalandar.GetAuditCalandarList(FromDate, ToDate, SearchTxt, AuditStatusName)
        ''dgAuditExecution.DataSource = mAuditCalendar
        'Session("mAuditCalendar") = mAuditCalendar

        BindDayPilot(FromDate, ToDate, SearchTxt, AuditStatusName)
        DayPilotMonth1.Update(2)

        '  DataBind()

        upnlcontrol.Update()
    End Sub
    Private Sub BindDayPilot(Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "", Optional ByVal AuditStatusName As String = "")

        DayPilotMonth1.DataSource = GetData(FromDate, ToDate, SearchTxt, AuditStatusName)
        DayPilotMonth1.DataBind()

    End Sub
    Private Function GetData(Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "", Optional ByVal AuditStatusName As String = "") As DataTable
        Dim dt As DataTable

        dt = New DataTable()
        dt.Columns.Add("id", GetType(String))
        dt.Columns.Add("FromDateTime", GetType(DateTime))
        dt.Columns.Add("ToDateTime", GetType(DateTime))
        dt.Columns.Add("name", GetType(String))
        dt.Columns.Add("column", GetType(String))

        Dim dr As DataRow
        Dim EndDateStr As String = ""
        'mAuditCalendar = AuditCalandar.GetAuditCalandarList(DayPilotMonth1.VisibleStart.ToString, DayPilotMonth1.VisibleEnd.ToString, SearchTxt, AuditStatusName)
        If FromDate = "" Then FromDate = DayPilotMonth1.VisibleStart.ToString
        If ToDate = "" Then ToDate = DayPilotMonth1.VisibleEnd.ToString

        mAuditCalendar = AuditCalandar.GetAuditCalandarList(FromDate.ToString, ToDate.ToString, SearchTxt, AuditStatusName)
        For i As Integer = 0 To mAuditCalendar.Count - 1
            dr = dt.NewRow()
            dr("id") = mAuditCalendar(i).ID.ToString

            dr("FromDateTime") = CDate(mAuditCalendar(i).StartDate).Date

            'If mAuditCalendar(i).EndDateFormatted.ToString <> "" Then
            '    dr("ToDateTime") = CDate(mAuditCalendar(i).EndDate).Date.AddDays(1)
            'Else
            '    dr("ToDateTime") = CDate(mAuditCalendar(i).StartDate).Date
            'End If
            dr("ToDateTime") = CDate(mAuditCalendar(i).StartDate).Date
            EndDateStr = ""
            If mAuditCalendar(i).EndDateFormatted.ToString <> "" Then EndDateStr = "Audit Closing Date - " + mAuditCalendar(i).EndDateFormatted.ToString

            'dr("name") = mAuditCalendar(i).AuditNo & " : Audit Standard - " & mAuditCalendar(i).AuditStandard & " : Audit Status - " & mAuditCalendar(i).AuditStatusName & EndDateStr
            dr("name") = mAuditCalendar(i).AuditNo & vbCrLf & "Audit Standard - " & mAuditCalendar(i).AuditStandard & vbCrLf & "Audit Status - " & mAuditCalendar(i).AuditStatusName & vbCrLf & EndDateStr
            dr("column") = "D"
            dt.Rows.Add(dr)
        Next

        Return dt
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)      'Added by Vikrant on 22-July-2011
        If Not IsPostBack Then
            DayPilotMonth1.ToolTip = True
            DayPilotMonth1.StartDate = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)

            Session("MiddleFrame") = "wfAuditExecutionCalendarList_AJAX.aspx?"
            setPeriod(0)
            Session("FromDate") = DayPilotMonth1.StartDate.ToString
            FromDate = DayPilotMonth1.StartDate.ToString
            DataFieldBind()
            SetControl()
            If cmbDateRange.Enabled = True Then
                cmbSearch.Focus()
            End If
            lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString("MMM-yyyy")
            ' upnlShowDate.Update()
        End If
    End Sub
    Protected Sub DayPilotMonth1_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
        Select Case e.Command
            Case "next"
                DayPilotMonth1.StartDate = DayPilotMonth1.StartDate.AddMonths(1)
            Case ("previous")
                DayPilotMonth1.StartDate = DayPilotMonth1.StartDate.AddMonths(-1)

            Case "today"
                DayPilotMonth1.StartDate = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
        End Select

        Session("StartDate") = DayPilotMonth1.StartDate
        Session("FromDate") = DayPilotMonth1.StartDate.ToString
        FromDate = DayPilotMonth1.StartDate.ToString

        DataFieldBind()
       
        lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString("MMM-yyyy")
        'upnlShowDate.Update()
        Dim data As Hashtable = New Hashtable()
        data("label") = DayPilotMonth1.StartDate.ToString("MMM-yyyy")
        DayPilotMonth1.Update(data, CallBackUpdateType.Full)
        upnlCurrentDate.Update()
        upnlGrid.Update()
    End Sub

    Protected Sub DayPilotMonth1_TimeRangeSelected(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.TimeRangeSelectedEventArgs) Handles DayPilotMonth1.TimeRangeSelected
        DataFieldBind()
    End Sub
    Protected Sub DayPilotMonth1_BeforeEventRender(sender As Object, e As DayPilot.Web.Ui.Events.Month.BeforeEventRenderEventArgs) Handles DayPilotMonth1.BeforeEventRender
        Try
            ' Dim index As Integer
            Dim Status As String

            'index = e.Text.IndexOf(":")
            'Status = e.Text.Substring(index)
            Status = e.Text

            e.InnerHTML = e.Text
            e.ToolTip = "Audit No - " & e.InnerHTML


            If Status.Contains("Open") Then
                e.BackgroundColor = "LightBlue"
            ElseIf Status.Contains("Close") Then
                e.BackgroundColor = "Green"
            ElseIf Status.Contains("Approaching") Then
                e.BackgroundColor = "Yellow"
            ElseIf Status.Contains("Forecasting") Then
                e.BackgroundColor = "LightPink"
            ElseIf Status.Contains("Pending") Then
                e.BackgroundColor = "RosyBrown"
            ElseIf Status.Contains("CrossDue") Then  'Added By Saylee on 27-Apr-2016 as per Deven Sir suggestion
                e.BackgroundColor = "Red"
            End If
        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        'dgAuditExecution.PageIndex = 0

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        'FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        'ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt

        ' FindNow(FromDate, ToDate, SearchTxt)
        upnlGrid.Update()
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
        'FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        'ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt

        DayPilotMonth1.StartDate = New DateTime(CDate(FromDate).Year, CDate(FromDate).Month, 1)



        FindNow(FromDate, ToDate, SearchTxt, AuditStatusName)
        DayPilotMonth1.Update(2)
        lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString("MMM-yyyy")
        'upnlShowDate.Update()
        upnlCurrentDate.Update()
        upnlcontrol.Update()
        upnlGrid.Update()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDateRange.SelectedIndex = 0
        txtSearchText.Text = ""
        cmbAuditStatusName.SelectedIndex = 0

        Dim DateIdx As Int32 = IIf(cmbDateRange.SelectedIndex >= 0 And cmbDateRange.Visible, cmbDateRange.SelectedIndex, 0)
        Dim AuditStatusNameIdx As Int32 = IIf(cmbAuditStatusName.SelectedIndex >= 0 And cmbAuditStatusName.Visible, cmbAuditStatusName.SelectedIndex, 0)

        ControlVisibility1(cmbSearch.SelectedIndex, DateIdx)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        'FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, CType(New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), String))
        'ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)
        AuditStatusNameIdx = IIf(cmbAuditStatusName.SelectedIndex < 0, 0, cmbAuditStatusName.SelectedIndex)
        AuditStatusName = IIf(cmbAuditStatusName.SelectedIndex = 0, "", cmbAuditStatusName.SelectedItem.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt
        Session("AuditStatusNameIdx") = AuditStatusNameIdx
        Session("AuditStatusName") = AuditStatusName

        DayPilotMonth1.StartDate = New DateTime(CDate(FromDate).Year, CDate(FromDate).Month, 1)
        FindNow(FromDate, ToDate, SearchTxt, AuditStatusName)

        lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString("MMM-yyyy")
        'upnlShowDate.Update()
        upnlCurrentDate.Update()
        upnlGrid.Update()
    End Sub
    Private Sub cmbAuditStatusName_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAuditStatusName.SelectedIndexChanged
        cmbDateRange.SelectedIndex = 0
        txtSearchText.Text = ""

        Dim DateIdx As Int32 = IIf(cmbDateRange.SelectedIndex >= 0 And cmbDateRange.Visible, cmbDateRange.SelectedIndex, 0)
        Dim AuditStatusNameIdx As Int32 = IIf(cmbAuditStatusName.SelectedIndex >= 0 And cmbAuditStatusName.Visible, cmbAuditStatusName.SelectedIndex, 0)

        ControlVisibility1(cmbSearch.SelectedIndex, DateIdx)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If

        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        'FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, CType(New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), String))
        'ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)
        AuditStatusNameIdx = IIf(cmbAuditStatusName.SelectedIndex < 0, 0, cmbAuditStatusName.SelectedIndex)
        AuditStatusName = IIf(cmbAuditStatusName.SelectedIndex = 0, "", cmbAuditStatusName.SelectedItem.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt
        Session("AuditStatusNameIdx") = AuditStatusNameIdx
        Session("AuditStatusName") = AuditStatusName

        DayPilotMonth1.StartDate = New DateTime(CDate(FromDate).Year, CDate(FromDate).Month, 1)
        FindNow(FromDate, ToDate, SearchTxt, AuditStatusName)

        lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString("MMM-yyyy")
        upnlCurrentDate.Update()
        'upnlShowDate.Update()
        DayPilotMonth1.Update(2)

        upnlcontrol.Update()
        upnlGrid.Update()
    End Sub
    Protected Sub DayPilotMonth1_BeforeCellRender(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.Month.BeforeCellRenderEventArgs) Handles DayPilotMonth1.BeforeCellRender
        '
    End Sub

    Protected Sub DayPilotMonth1_EventMove(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventMoveEventArgs) Handles DayPilotMonth1.EventMove

    End Sub

    Protected Sub DayPilotMonth1_EventResize(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventResizeEventArgs) Handles DayPilotMonth1.EventResize

    End Sub
    Protected Sub DayPilotMonth1_EventClick(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventClickEventArgs) Handles DayPilotMonth1.EventClick

        Dim str As String = e.Text
        DataFieldBind()

        If str.Contains("Open") Or str.Contains("Close") Then
            Dim mAuditExecution As AuditExecution = AuditExecution.GetAuditExecution(New Guid(e.Value))
            Session("mAuditExecution") = mAuditExecution
            'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAuditExecutionWindow", "OpenAuditExecutionWindow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAuditExecution_Ajax.aspx?BackPage=index.aspx');", True)

        ElseIf str.Contains("Approaching") Or str.Contains("Forecasting") Or str.Contains("Pending") Then
            Dim mAuditSchedule As AuditSchedule = AuditSchedule.GetAuditSchedule(New Guid(e.Value))
            Session("mAuditSchedule") = mAuditSchedule
            ' ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAuditScheduleWindow", "OpenAuditScheduleWindow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfAuditSchedule_Ajax.aspx?BackPage=index.aspx');", True)

        End If


        'DayPilotMonth1.Update(2)

        'upnlcontrol.Update()
    End Sub
    Protected Sub txtSearchText_TextChanged(sender As Object, e As EventArgs) Handles txtSearchText.TextChanged, txtFromDate.TextChanged, txtToDate.TextChanged
        SearchIdx = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIdx = IIf(cmbDateRange.SelectedIndex < 0, 0, cmbDateRange.SelectedIndex)
        'FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        'ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchTxt = IIf(txtSearchText.Text = "", "", txtSearchText.Text)
        AuditStatusNameIdx = IIf(cmbAuditStatusName.SelectedIndex < 0, 0, cmbAuditStatusName.SelectedIndex)
        AuditStatusName = IIf(cmbAuditStatusName.SelectedIndex = 0, "", cmbAuditStatusName.SelectedItem.Text)

        Session("SearchIdx") = SearchIdx
        Session("DateIdx") = DateIdx
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchTxt") = SearchTxt
        Session("AuditStatusNameIdx") = AuditStatusNameIdx
        Session("AuditStatusName") = AuditStatusName

        FindNow(FromDate, ToDate, SearchTxt, AuditStatusName)
        DayPilotMonth1.Update(2)

        upnlcontrol.Update()
        upnlGrid.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

   
   
End Class