

'Created by : Saylee
'Dated      : 5-Sep-2022



Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System.Linq.Enumerable
Imports System
Imports System.IO
Imports System.Text


Public Class wfADSBTechRecordingList
    Inherits System.Web.UI.Page

#Region " Variable Declaration "

    Private mADSBTechRecording As ADSBTechRecording
    Private mADSBReviewMeeting As ADSBReviewMeeting
    Private mADSBPlanningSupport As ADSBPlanningSupport
    Private mADSBSamplingClosure As ADSBSamplingClosure
    Private mADSBTechRecordingList As ADSBTechRecordingList
    Dim mDistinctADSBText As DistinctADSBText
    Dim DateIndex, FromDate, ToDate, Text, StatusID, No, ADSBNumber, Subject As String
    Dim EventLogID As Guid
    Dim totcnt As Integer
    Dim mFileAttach As FileAttach
    Dim mTransactionListCount As TransactionListCount
    Dim mStatusList As StatusList
    Protected mtmpWOList As nWOList
    Public mEventLog As EventLog
    Public mUser As User
    Dim OpenFromLink As Integer
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mADSBTechRecording = Session("mADSBTechRecording")
        mADSBTechRecordingList = Session("mADSBTechRecordingList")
        Text = Session("Text")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        ADSBNumber = IIf(IsNothing(Session("ADSBNumber")), "", Session("ADSBNumber"))
        Subject = IIf(IsNothing(Session("Subject")), "", Session("Subject"))

        DateIndex = Session("DateIndex")
        StatusID = Session("StatusID")
        mUser = Session("mUser")

    End Sub
    Private Sub SetSession()
        Session("mADSBTechRecording") = mADSBTechRecording
        Session("mADSBTechRecordingList") = mADSBTechRecordingList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID

        Session("No") = No
        Session("mDistinctADSBText") = mDistinctADSBText
        Session("Text") = Text

        Session("ADSBNumber") = ADSBNumber
        Session("Subject") = Subject
        Session("OpenFromLink") = OpenFromLink
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mADSBTechRecording")
        Session.Remove("mADSBTechRecordingList")

        Session.Remove("FromDate")
        Session.Remove("ToDate")

        Session.Remove("DateIndex")
        Session.Remove("StatusID")

        Session.Remove("No")
        Session.Remove("Text")

        Session.Remove("ADSBNumber")
        Session.Remove("Subject")

        Session.Remove("mUser")
        Session.Remove("mDistinctADSBText")
     
    End Sub
    Private Sub ClearAll()
        OpenFromLink = Session("OpenFromLink")
        If InStr(Session("MiddleFrame"), "wfADSBTechRecordingList.aspx?OpenFromLink=" & OpenFromLink) <= 0 Then
            RemoveSession()
            Session.Remove("mADSBTechRecordingList")
            Session.Remove("IsPageLoadedForFirstTime")
            Session.Remove("OpenFromLink")
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetGrid()
        ' btnAddNew.Visible = IIf(AppSettings("ClientCode") <> "A3S", True, False)
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub setVariables()

        DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        Text = IIf(cmbADSBRecording.SelectedIndex <= 0, "", cmbADSBRecording.SelectedValue)

        StatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)


        No = txtNo.Text.Trim
        ADSBNumber = txtADSBNumber.Text.Trim
        Subject = txtSubject.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID

        Session("Text") = Text
        Session("No") = No

        Session("ADSBNumber") = ADSBNumber
        Session("Subject") = Subject
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", _
                        Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "", Optional ByVal ADSBNumber As String = "", _
                        Optional ByVal Subject As String = "", Optional ByVal OpenFromLink As Integer = 0)
        mADSBTechRecordingList = Nothing
        dgADSBRecordingList.DataSource = Nothing

        mADSBTechRecordingList = ADSBTechRecordingList.GetADSBTechRecordingList(FromDate, ToDate, Text, No, StatusID, ADSBNumber, Subject, OpenFromLink:=OpenFromLink)
        dgADSBRecordingList.DataSource = mADSBTechRecordingList
        Session("mADSBTechRecordingList") = mADSBTechRecordingList
    End Sub
    Private Sub SetControl(Optional ByVal OpenFromLink As Integer = 0)
        setPeriod(DateIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        No = IIf(No Is Nothing, txtNo.Text.Trim, No)

        Text = IIf(Text Is Nothing, IIf(cmbADSBRecording.SelectedIndex <= 0, "", cmbADSBRecording.SelectedValue), Text)

        ADSBNumber = IIf(ADSBNumber Is Nothing, txtADSBNumber.Text.Trim, ADSBNumber)
        Subject = IIf(Subject Is Nothing, txtSubject.Text.Trim, Subject)

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID


        Session("StatusId") = StatusID

        Session("No") = No
        Session("Text") = Text
        Session("ADSBNumber") = ADSBNumber

        Session("Subject") = Subject


        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate


        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate

        txtNo.Text = No

        txtADSBNumber.Text = ADSBNumber

        txtSubject.Text = Subject

        cmbStatus.SelectedValue = StatusID

        If mDistinctADSBText.Contains(Text) Then
            cmbADSBRecording.SelectedValue = IIf(Text = "", "(ALL)", Text)
        Else
            cmbADSBRecording.SelectedValue = "(ALL)"
        End If

        txtNo.Text = No


        mUser = CType(Session("mUser"), User)
        mEventLog = Session("mEventLog")
        If mUser Is Nothing Then mUser = SI.UTILITY.User.GetUser(mEventLog.UserID)


        Session("mUser") = mUser

        FindNow(Text, Val(No), FromDate, ToDate, Val(StatusID), "", ADSBNumber:=ADSBNumber, Subject:=Subject, OpenFromLink:=OpenFromLink)
        dgADSBRecordingList.DataBind()

        cmbDate.SelectedIndex = DateIndex
        cmbADSBRecording.SelectedValue = IIf(Text = "", "(ALL)", Text)
        txtNo.Text = No


        ControlVisibility(DateIndex)
        dgADSBRecordingList.DataBind()
        If mADSBTechRecordingList.Count > 0 And mADSBTechRecordingList.Count <> mADSBTechRecordingList.TotalRecords Then
            lblResult.Text = "List of AD/SB(s) as per criteria : Recent " & mADSBTechRecordingList.Count & " of " & mADSBTechRecordingList.TotalRecords.ToString & " Record(s)."
        Else
            lblResult.Text = "List of AD/SB(s) as per criteria : " & mADSBTechRecordingList.Count & " Record(s)."
        End If
    End Sub
    Private Sub ControlVisibility(Optional ByVal DateIndex As Int32 = 0)
        lblFromDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub Visibility()
        If OpenFromLink = 1 Then ' from Review Meeting link
            dgADSBRecordingList.Columns(7).Visible = False 'ApprovedStatus
        ElseIf OpenFromLink = 2 Then ' from Participant Approval link
            btnAddNew.Visible = False
            btnAddNewTop.Visible = False
            cmbStatus.Visible = False
            lblStatus.Visible = False
            btnApplicability1.Visible = False
            lblApplicabilityButton.Visible = False
            btnReview1.Visible = False
            lblMeetingReviewButton.Visible = False
            btnPlanned1.Visible = False
            lblPlannedButton.Visible = False
            btnMonitoring1.Visible = False
            lblMonitoringButton.Visible = False
            dgADSBRecordingList.Columns(5).Visible = False 'Grid Buttons
            dgADSBRecordingList.Columns(6).Visible = False 'Delete
        End If
    End Sub
    Private Sub SetTitle()
        lbltitle.InnerText = "List of AD/SB(s)  [Total No of Record(s):-" + mADSBTechRecordingList.TotalRecords.ToString() + "]"
        upnltitle.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TextNo As String
                        Try
                            Dim mADSBTechRecording As ADSBTechRecording
                            Session("sender") = ""
                            mADSBTechRecording = CType(Session("mADSBTechRecording"), ADSBTechRecording)


                            TextNo = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted.ToString
                            ADSBTechRecording.DeleteADSBTechRecording(mADSBTechRecording.ID)

                            DataFieldBind()
                            SetControl(OpenFromLink:=OpenFromLink)
                            SetGrid()

                            upnlGrid.Update()
                            upnlResult.Update()
                        Catch ex As SqlException
                            Dim UseInstr As String = String.Empty
                            If ex.Message.Contains("FKtabReqtabCWP") Then
                                UseInstr = "Requisition"
                            ElseIf ex.Message.Contains("FKtabMROInvoicetabCWP") Then
                                UseInstr = "Invoice"
                            End If
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, UseInstr, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "ADSBTechRecording", "Can't delete : " + TextNo + " is Currently in use", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                            End If
                            DataFieldBind()
                            SetControl(OpenFromLink:=OpenFromLink)
                            SetGrid()
                            upnlGrid.Update()
                            upnlResult.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DeletedSuccessFully, MSGBox.Message_text.DeletedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "ADSBTechRecording", TextNo, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
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
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region "DataFieldBind"
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

        mDistinctADSBText = DistinctADSBText.GetDistinctTextList(IsSelectTagRequired:=True, Tag:="(ALL)")
        cmbADSBRecording.DataSource = mDistinctADSBText
        Session("mDistinctADSBText") = mDistinctADSBText

        mStatusList = StatusList.GetStatusList(0, IsSelectTagRequired:=True)
        cmbStatus.DataSource = mStatusList
        Session("mStatusList") = mStatusList


        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            OpenFromLink = Request.QueryString("OpenFromLink") '1 from Review Meeting 2 from Participant Approval links 
            Session("OpenFromLink") = OpenFromLink
            Session("MiddleFrame") = "wfADSBTechRecordingList.aspx?OpenFromLink=" & OpenFromLink
            mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))
            Session("mEventLog") = mEventLog
            DataFieldBind()
            SetControl(OpenFromLink:=OpenFromLink)
            Visibility()
        End If
        SetGrid()
        SetTitle()
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If

        setVariables()

        SetGrid()
        ControlVisibility(DateIndex)

        upnlGrid.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
        upnlADSBRecording.Update()
        upnlADSBRecordingNo.Update()
        upnlADSBRecordinglblNo.Update()
    End Sub
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFindNow.Click

        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setVariables()

        FindNow(Text, Val(No), FromDate, ToDate, Val(StatusID), "", ADSBNumber:=ADSBNumber, Subject:=Subject, OpenFromLink:=OpenFromLink)

        dgADSBRecordingList.DataBind()
        SetGrid()
        ControlVisibility(DateIndex)
        lblResult.Text = "List of AD/SB(s) as per criteria : " & mADSBTechRecordingList.Count & " Record(s)."
        upnlGrid.Update()
        upnlResult.Update()
        upnlADSBRecording.Update()
        upnlADSBRecordingNo.Update()
        upnlADSBRecordinglblNo.Update()

        If mADSBTechRecordingList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub dgADSBRecordingList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgADSBRecordingList.PageIndexChanging
        dgADSBRecordingList.PageIndex = e.NewPageIndex
        dgADSBRecordingList.DataSource = mADSBTechRecordingList
        Session("mADSBTechRecordingList") = mADSBTechRecordingList
        dgADSBRecordingList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgADSBRecordingList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgADSBRecordingList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                If (Not User.IsInRole("ADSBTechRecordingView") And Not User.IsInRole("ADSBTechRecordingEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "ADSBTechRecording", User.Identity.Name & " is not Authorized User to edit " + mADSBTechRecording.ADSBNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

             

                Session("mADSBTechRecording") = mADSBTechRecording
                Dim mADSBTechRecordingDetail As String = "ADSBTechRecording : " + mADSBTechRecording.ADSBNo + " dated : " + mADSBTechRecording.ADSBDateFormatted
                MarkLog(Util.Action.Edit, "ADSBTechRecording", mADSBTechRecordingDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBTechRecording.aspx?BackPage=Index.aspx');", True)
            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                If (Not User.IsInRole("ADSBTechRecordingDelete")) Then
                    SetSession()
                    MarkLog(Util.Action.Delete, "ADSBTechRecording", User.Identity.Name & " is not Authorized User to delete " + mADSBTechRecording.ADSBNo, Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    '************************************
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

                    Session("mADSBTechRecording") = mADSBTechRecording
                End If
                '1: Screen
            Case "Applicability"
                ''Applicability
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                If (Not User.IsInRole("ADSBTechRecordingView") And Not User.IsInRole("ADSBTechRecordingEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "ADSBTechRecording", User.Identity.Name & " is not Authorized User to edit " + mADSBTechRecording.ADSBNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Session("mADSBTechRecording") = mADSBTechRecording
                Dim mADSBTechRecordingDetail As String = "ADSBTechRecording : " + mADSBTechRecording.ADSBNo + " dated : " + mADSBTechRecording.ADSBDateFormatted
                MarkLog(Util.Action.Edit, "ADSBTechRecording", mADSBTechRecordingDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBApplicability.aspx?BackPage=Index.aspx');", True)

                '2: Screen
            Case "Review"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ADSBReviewMeetingNew")) Then
                    MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name & " is not Authorized User to set AD-SB Review Meeting", Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mADSBReviewMeeting = ADSBReviewMeeting.GetADSBReviewMeeting(ID:="{00000000-0000-0000-0000-000000000000}", ADSBTechRecordingID:=mID.ToString, From:=1)
                If mADSBReviewMeeting.ADSBTechRecordingID.Equals(Guid.Empty) Then
                    mADSBReviewMeeting = ADSBReviewMeeting.NewADSBReviewMeeting()
                    mADSBReviewMeeting.ADSBTechRecordingID = mID
                End If
                Session("mADSBReviewMeeting") = mADSBReviewMeeting
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                Session("mADSBTechRecordingForADSBReviewMeetingPage") = mADSBTechRecording
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBReviewMeeting.aspx?BackPage=Index.aspx');", True)
                '3 : Planned
            Case "Planned"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ADSBPlanningSupportNew")) Then
                    MarkLog(Util.Action.Save, "ADSBPlanningSupport", User.Identity.Name & " is not Authorized User to set AD-SB Review Meeting", Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mADSBPlanningSupport = ADSBPlanningSupport.GetADSBPlanningSupport(ID:="{00000000-0000-0000-0000-000000000000}", ADSBTechRecordingID:=mID.ToString, From:=1)
                If mADSBPlanningSupport.ADSBTechRecordingID.Equals(Guid.Empty) Then
                    mADSBPlanningSupport = ADSBPlanningSupport.NewADSBPlanningSupport()
                    mADSBPlanningSupport.ADSBTechRecordingID = mID
                End If
                Session("mADSBPlanningSupport") = mADSBPlanningSupport
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                mADSBReviewMeeting = ADSBReviewMeeting.GetADSBReviewMeeting(ID:="{00000000-0000-0000-0000-000000000000}", ADSBTechRecordingID:=mID.ToString, From:=1)
                Session("mADSBTechRecordingForADSBPlanningSupportgPage") = mADSBTechRecording
                Session("mADSBReviewMeetingForADSBPlanningSupportgPage") = mADSBReviewMeeting
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBPlanningSupport.aspx?OpenFromLink=1&BackPage=Index.aspx');", True)
                '4 : Screen
            Case "Monitoring"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ADSBMonitoringNew")) Then
                    MarkLog(Util.Action.Save, "ADSBMonitoring", User.Identity.Name & " is not Authorized User to monitor AD/SB(s)", Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                Session("mADSBTechRecording") = mADSBTechRecording

             

                If mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0 Then
                    MSGBoxCtrl.show("Alert..!!", "No Monitoring details available for this AD/SB", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Dim mADSBMonitoring As ADSBMonitoring
                mADSBMonitoring = ADSBMonitoring.NewADSBMonitoring(mADSBTechRecording.ID, Guid.Empty)
                Session("mADSBMonitoring") = mADSBMonitoring

                Dim mADSBTechRecordingDetail As String = "ADSBTechRecording : " + mADSBTechRecording.ADSBNo + " dated : " + mADSBTechRecording.ADSBDateFormatted
                MarkLog(Util.Action.Edit, "ADSBTechRecording", mADSBTechRecordingDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBMonitoring.aspx?BackPage=Index.aspx');", True)
                '5 : Sampling Closure
            Case "SamplingClosure"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ADSBSamplingClosureNew")) Then
                    MarkLog(Util.Action.Save, "ADSBSamplingClosure", User.Identity.Name & " is not Authorized User to set AD-SB Review Meeting", Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mADSBSamplingClosure = ADSBSamplingClosure.GetADSBSamplingClosure(ID:="{00000000-0000-0000-0000-000000000000}", ADSBTechRecordingID:=mID.ToString, From:=1)
                If mADSBSamplingClosure.ADSBTechRecordingID.Equals(Guid.Empty) Then
                    mADSBSamplingClosure = ADSBSamplingClosure.NewADSBSamplingClosure()
                    mADSBSamplingClosure.ADSBTechRecordingID = mID
                End If
                Session("mADSBSamplingClosure") = mADSBSamplingClosure
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                mADSBReviewMeeting = ADSBReviewMeeting.GetADSBReviewMeeting(ID:="{00000000-0000-0000-0000-000000000000}", ADSBTechRecordingID:=mID.ToString, From:=1)
                Session("mADSBTechRecordingForADSBSamplingClosuregPage") = mADSBTechRecording
                Session("mADSBReviewMeetingForADSBSamplingClosuregPage") = mADSBReviewMeeting
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBSamplingClosure.aspx?BackPage=Index.aspx');", True)

                ': Approval
            Case "Approval"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ADSBReviewMeetingParticipantApprovalView")) Then
                    MarkLog(Util.Action.Save, "ADSBPlanningSupport", User.Identity.Name & " is not Authorized User to set AD-SB Review Meeting", Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mADSBPlanningSupport = ADSBPlanningSupport.GetADSBPlanningSupport(ID:="{00000000-0000-0000-0000-000000000000}", ADSBTechRecordingID:=mID.ToString, From:=1)
                'If mADSBPlanningSupport.ADSBTechRecordingID.Equals(Guid.Empty) Then
                '    mADSBPlanningSupport = ADSBPlanningSupport.NewADSBPlanningSupport()
                '    mADSBPlanningSupport.ADSBTechRecordingID = mID
                'End If
                Session("mADSBPlanningSupport") = mADSBPlanningSupport
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mID)
                mADSBReviewMeeting = ADSBReviewMeeting.GetADSBReviewMeeting(ID:="{00000000-0000-0000-0000-000000000000}", ADSBTechRecordingID:=mID.ToString, From:=1, _
                                                                            UserID:=SI.UTILITY.User.GetUser(User.Identity.Name).UserID.ToString)
                Session("mADSBTechRecordingForADSBPlanningSupportgPage") = mADSBTechRecording
                Session("mADSBReviewMeetingForADSBPlanningSupportgPage") = mADSBReviewMeeting
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBPlanningSupport.aspx?OpenFromLink=2&BackPage=Index.aspx');", True)
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        mADSBTechRecording = ADSBTechRecording.NewADSBTechRecording()
        MarkLog(Util.Action.[New], "ADSBTechRecording", "", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)

        Session("mADSBTechRecording") = mADSBTechRecording
        SetGrid()
        upnlGridView.Update()

        '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfWOInvoice_Ajax.aspx?BackPage=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfADSBTechRecording.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        RemoveSession()

        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgADSBRecordingList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgADSBRecordingList.Sorting
        mADSBTechRecordingList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgADSBRecordingList.DataSource = mADSBTechRecordingList
        Session("mADSBTechRecordingList") = mADSBTechRecordingList
        dgADSBRecordingList.DataBind()
        SetGrid()
    End Sub
#End Region
End Class