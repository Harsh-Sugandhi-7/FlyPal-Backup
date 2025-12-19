Imports System
Imports System.Data
Imports System.IO
Imports DayPilot.Web.Ui.Enums
Imports DayPilot.Web.Ui.Events
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Configuration.ConfigurationManager
Imports DayPilot.Web.Ui.Events.Scheduler
Public Class wfhangarPlanningCalendarList
    Inherits System.Web.UI.Page



#Region " Variable Declaration "
    Public mhanger As Hanger
    'Public mAuditCalendar As AuditCalandar
    Public mhangarlist As HangarList
    Public mDistinctGood As DistinctGood
    Dim SearchIdx, DateIdx, mFromDate, mToDate, SearchTxt, mHangarStr, mHangarIdx As String
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Dim mExecutionDetail As String
    Dim mFileAttach As FileAttach
    Public a As Date
#End Region

#Region "Helper Methods"
    Private Sub GetSession()
        mDistinctGood = Session("mDistinctGood")
        mhangarlist = Session("mhangarlist")
        mFromDate = Session("mFromDate")
        mToDate = Session("mToDate")
        mHangarStr = CType(Session("mHangarStr"), String)
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mhangarlist")
        Session.Remove("mFromDate")
        Session.Remove("mToDate")
        Session.Remove("mFileAttach")
        Session.Remove("mHangar")
    End Sub
    Private Sub ClearAll()
        'If Session("MiddleFrame") <> "wfhangarPlanningCalendarList.aspx?" Then
        '    RemoveSession()
        'End If
        If InStr(Session("MiddleFrame"), "wfhangarPlanningCalendarList.aspx?") <= 0 Then
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
        ' setPeriod(DateIdx)
        'FindNow(, , , mHangarStr)
        ' FindNow(cmbMonth.SelectedValue, , , mHangarStr)
        FindNow(, , , mHangarStr)
        cmbMonth.SelectedIndex = DateIdx
        cmbModel.SelectedIndex = mHangarIdx
    End Sub
    Private Sub FindNow(Optional ByVal mFromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "", Optional ByVal mHangarStr As String = "{00000000-0000-0000-0000-000000000000}")
        BindDayPilot(, , , mHangarStr)
    End Sub

#End Region

#Region " DataBinding "
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If
        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
        cmbMonth.SelectedValue = MonthName(Now.Month, False)
        cmbMonth.DataBind()
    End Sub
    Public Sub DataFieldBind()

        mFromDate = IIf(IsNothing(mFromDate), "1/1/1900", mFromDate)
        mToDate = IIf(IsNothing(mToDate), "1/1/2200", mToDate)     
        mHangarStr = IIf(IsNothing(mHangarStr), 0, cmbModel.SelectedItem.Text)
        Session("mFromDate") = mFromDate
        Session("ToDate") = mToDate
        Session("mHangarStr") = mHangarStr
        BindDayPilot(, , SearchTxt, mHangarStr)
        DayPilotMonth1.Update()
        upnlcontrol.Update()

    End Sub
    Private Sub DataFieldBinding()

        mDistinctGood = DistinctGood.GetDistinctText("3", 0, True, AddTopItem:="(ALL)")
        cmbModel.DataSource = mDistinctGood
        Session("mDistinctGood") = mDistinctGood
        cmbModel.DataBind()

    End Sub
    Private Sub BindDayPilot(Optional ByVal mFromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "", Optional ByVal mHangarStr As String = "{00000000-0000-0000-0000-000000000000}")

        DayPilotMonth1.DataSource = GetData(, , , cmbModel.SelectedItem.Text)
        DayPilotMonth1.DataBind()

    End Sub
    Private Function GetData(Optional ByVal mFromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal SearchTxt As String = "", Optional ByVal mHangarStr As String = "{00000000-0000-0000-0000-000000000000}") As DataTable
        Dim dt As DataTable

        dt = New DataTable()
        dt.Columns.Add("id", GetType(String))
        dt.Columns.Add("FromDateTime", GetType(DateTime))
        dt.Columns.Add("ToDateTime", GetType(DateTime))
        dt.Columns.Add("name", GetType(DateTime))
        dt.Columns.Add("maircraft", GetType(String))
        dt.Columns.Add("column", GetType(String))

        Dim dr As DataRow
        Dim EndDateStr As String = ""
        mhangarlist = HangarList.GetHangarList(, changarID:=cmbModel.SelectedValue, cdatetimefrom:="1/1/1900", cdatetimeto:="1/1/3300", cRemark:="", Text:="", No:=0, year:=CType(cmbYear.SelectedItem.Text, Integer), month:=cmbMonth.SelectedIndex + 1, graph:=1)
        If mFromDate = "" Then mFromDate = DayPilotMonth1.VisibleStart.ToString
        If mToDate = "" Then ToDate = DayPilotMonth1.VisibleEnd.ToString

        For i As Integer = 0 To mhangarlist.Count - 1
            dr = dt.NewRow()
            dr("id") = mhangarlist(i).ID.ToString
            dr("FromDateTime") = CDate(mhangarlist(i).Hdatetimefrom).Date
            'dr("ToDateTime") = CDate(mhangarlist(i).Hdatetimeto).Date.AddDays(1)
            dr("ToDateTime") = CDate(mhangarlist(i).Hdatetimeto).Date.AddDays(1)
            EndDateStr = ""
            If mhangarlist(i).Hdatetimeto.ToString <> "" Then EndDateStr = "Hangar Closing Date - " + mhangarlist(i).Hdatetimeto.ToString
            dr("maircraft") = mhangarlist(i).Haircraft.ToString
            'dr("name") = mAuditCalendar(i).AuditNo & " : Audit Standard - " & mAuditCalendar(i).AuditStandard & " : Audit Status - " & mAuditCalendar(i).AuditStatusName & EndDateStr
            ' dr("name") = mAuditCalendar(i).AuditNo & vbCrLf & "Audit Standard - " & mAuditCalendar(i).AuditStandard & vbCrLf & "Audit Status - " & mAuditCalendar(i).AuditStatusName & vbCrLf & EndDateStr
            dr("column") = "D"
            dt.Rows.Add(dr)
        Next

        Return dt
    End Function
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfhangarPlanningCalendarList.aspx?"

            SetCombo()
            DataFieldBinding()
            ''new add
            DataFieldBind()
            upnlcontrol.Update()
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
        Session("mFromDate") = DayPilotMonth1.StartDate.ToString
        mFromDate = DayPilotMonth1.StartDate.ToString

        DataFieldBind()
        lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString(AppSettings("DateFormat"))
        'upnlShowDate.Update()
        Dim data As Hashtable = New Hashtable()
        data("label") = DayPilotMonth1.StartDate.ToString(AppSettings("DateFormat"))
        DayPilotMonth1.Update(data, CallBackUpdateType.Full)
        ' upnlCurrentDate.Update()
        'upnlGrid.Update()

    End Sub



    'Private Sub GetColorForTextbox(ByVal DutyStatusID As Integer)
    '    Select Case DutyStatusID
    '        Case 0  'Select
    '            txtDutyStatusColor.BackColor = Drawing.Color.White
    '        Case 1  'Rest
    '            txtDutyStatusColor.BackColor = Drawing.Color.Green
    '        Case 2  'StandBy
    '            txtDutyStatusColor.BackColor = Drawing.Color.Yellow
    '        Case 3  'Leave
    '            txtDutyStatusColor.BackColor = Drawing.Color.Orange
    '        Case 4  'Training
    '            txtDutyStatusColor.BackColor = Drawing.Color.Pink
    '        Case 5  'FlightDuty
    '            txtDutyStatusColor.BackColor = Drawing.Color.Red
    '        Case 6  'Duty
    '            txtDutyStatusColor.BackColor = Drawing.Color.Maroon
    '        Case 7  'WeeklyOff
    '            txtDutyStatusColor.BackColor = Drawing.Color.GreenYellow
    '        Case 8  'Positioning
    '            txtDutyStatusColor.BackColor = Drawing.Color.Magenta
    '        Case 9  'Transportation
    '            txtDutyStatusColor.BackColor = Drawing.Color.RosyBrown
    '        Case 10  'Split Duty
    '            txtDutyStatusColor.BackColor = Drawing.Color.LemonChiffon
    '        Case 11 'Travelled
    '            txtDutyStatusColor.BackColor = Drawing.Color.LightGreen
    '        Case 12 'Planned Flight Duty
    '            txtDutyStatusColor.BackColor = Drawing.Color.Brown
    '            ' Miscellaneous Duty
    '        Case 15
    '            txtDutyStatusColor.BackColor = Drawing.Color.Bisque
    '    End Select
    '    upnlCrewRosterWin.Update()
    'End Sub
    Protected Sub DayPilotMonth1_BeforeEventRender(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.Month.BeforeEventRenderEventArgs) Handles DayPilotMonth1.BeforeEventRender
        Try
            'e.InnerHTML = e.Text.Replace("*", "") ' e.Text & vbCrLf & "(" & e.Start.ToString("dd-MMM-yyyy HH:mm") & " - " & e.End.ToString("dd-MMM-yyyy HH:mm") & ")"
            'e.ToolTip = "(" & e.Start.ToString("dd-MMM-yyyy HH:mm") & " - " & e.End.ToString("dd-MMM-yyyy HH:mm") & ")"
            'e.BackgroundColor = "Black"
            ' Dim index As Integer
            '  Dim Status As String
            'index = e.Start.IndexOf(":")
            'Status = e.Text.Substring(index)

            'Status = e.Text
            e.InnerHTML = e.Text.Replace("*", "")
            'e.End.ToString("dd-MMM-yyyy").Length(-1)

            e.ToolTip = "(" & e.Start.ToString("dd-MMM-yyyy") & " - " & e.End.AddDays(-1).ToString("dd-MMM-yyyy") & ")"
            '  e.InnerHTML = e.Text
            '  e.ToolTip = "Hangar - " & e.InnerHTML
            e.BackgroundColor = "Black"
            ' mhangarlist = HangarList.GetHangarList(, changarID:=cmbhangar.SelectedValue, cdatetimefrom:=mFromDate, cdatetimeto:=mToDate)
            ' Status = (mhangarlist(i).HHangerWithCity.ToString())
            ''For i As Integer = 0 To mhangarlist.Count - 1
            'Dim (3) As Object = {e.BackgroundColor = "LightBlue",
            '                     e.BackgroundColor = "Green",
            '                          e.BackgroundColor = "Yellow"}
            ''Dim colors As VariantType
            'colors = Array(e.BackgroundColor = "LightBlue", e.BackgroundColor = "Green", e.BackgroundColor = "Yellow", )
            '    e.BackgroundColor = "LightBlue"

            'ElseIf If strData () Then
            '    e.BackgroundColor = "Green"
            'ElseIf Status.Contains("Approaching") Then
            '    e.BackgroundColor = "Yellow"
            'ElseIf Status.Contains("Forecasting") Then
            '    e.BackgroundColor = "LightPink"
            'ElseIf Status.Contains("Pending") Then
            '    e.BackgroundColor = "RosyBrown"
            'ElseIf Status.Contains("CrossDue") Then  'Added By Saylee on 27-Apr-2016 as per Deven Sir suggestion
            '    e.BackgroundColor = "Red"
        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub
    'Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
    '    'dgAuditExecution.PageIndex = 0

    'End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("~/Dashboard.aspx")
    End Sub
  
    Protected Sub DayPilotMonth1_BeforeCellRender(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.Month.BeforeCellRenderEventArgs) Handles DayPilotMonth1.BeforeCellRender
        '
    End Sub

    Protected Sub DayPilotMonth1_EventMove(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventMoveEventArgs) Handles DayPilotMonth1.EventMove
    End Sub

    Protected Sub DayPilotMonth1_EventResize(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventResizeEventArgs) Handles DayPilotMonth1.EventResize
    End Sub
    Protected Sub DayPilotMonth1_EventClick(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventClickEventArgs) Handles DayPilotMonth1.EventClick

        Dim mDistinctTextListForHangar As DistinctTextListForHangar
        Dim mDistinctHangarListForHangar As DistinctHangarListForHangar
        Dim mDistinctAircraftListForHangar As DistinctAircraftListForHangar
        mDistinctAircraftListForHangar = DistinctAircraftListForHangar.GetDistinctText("2", 0, True)
        BindDayPilot()
        Dim mID As Guid = New Guid(e.Value)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAuditExecutionWindow", "OpenAuditExecutionWindow('wfHangar.aspx');", True)
        mhanger = Hanger.GetHangar(mID)
        'mhanger.BeginEdit()
        Session("mhanger") = mhanger      
        '' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAuditExecutionWindow", "OpenAuditExecutionWindow('wfHangar.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHangerWindow", "OpenHangerWindow();", True)

    End Sub
    Protected Sub DayPilotMonth1_TimeRangeSelected(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.TimeRangeSelectedEventArgs) Handles DayPilotMonth1.TimeRangeSelected

        Dim mDistinctTextListForHangar As DistinctTextListForHangar
        Dim mDistinctHangarListForHangar As DistinctHangarListForHangar
        Dim mDistinctAircraftListForHangar As DistinctAircraftListForHangar
        '  Dim mdistinctGood As DistinctGood
        ' mDistinctGood = DistinctGood.GetDistinctText("3", 0, True)
        ' mDistinctGood = DistinctGood.GetDistinctText("3", 0)
        ' cmbModel.DataSource = mDistinctGood
        ' cmbModel.DataBind()
        mDistinctAircraftListForHangar = DistinctAircraftListForHangar.GetDistinctText("2", 0, True)
        BindDayPilot()
        mhanger = Hanger.NewHangar()
        Session("mhanger") = mhanger
        '' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAuditExecutionWindow", "OpenAuditExecutionWindow('wfHangar.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHangerWindow", "OpenHangerWindow();", True)

    End Sub


    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisplay.Click

        DayPilotMonth1.ToolTip = True
        DayPilotMonth1.StartDate = DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1) ' CDate(New DateTime(CType(cmbYear.SelectedItem.Text, Integer), , 1))
        Session("mFromDate") = DayPilotMonth1.StartDate.ToString
        mFromDate = DayPilotMonth1.StartDate.ToString

        'SetCombo()
        DataFieldBind()
        ' mFromDate = IIf(cmbYear.SelectedValue <> "" And cmbMonth.SelectedValue <> "", cmbYear.SelectedValue And cmbMonth.SelectedValue, "1/1/1900")
        ' Session("mFromDate") = mFromDate
        ' Session("ToDate") = mToDate
        SetControl()
        lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString(AppSettings("DateFormat"))
        'upnlcontrol.Update()

    End Sub
    Private Sub hdnGraph_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnGraph.Click
       
        DayPilotMonth1.ToolTip = True
        DayPilotMonth1.StartDate = mFromDate
        DataFieldBind()
        SetControl()
        lblCurrentDate.Text = DayPilotMonth1.StartDate.ToString(AppSettings("DateFormat"))
        upnlcontrol.Update()
    End Sub
#End Region
    'Protected Sub DayPilotMonth1_EventRightClick(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventRightClickEventArgs) Handles DayPilotMonth1.EventRightClick
    'End Sub
End Class