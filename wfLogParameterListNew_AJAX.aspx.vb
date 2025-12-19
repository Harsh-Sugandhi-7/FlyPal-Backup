

'AJAX Conversion By Saylee On 26-Sep-2014

Public Class wfLogParameterListNew_AJAX
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mLogParameterList As LogFuelAndOilList
    Dim mMachineNameValueList As MachineNameValueList
    Dim FromDate As String
    Dim ToDate As String
    Dim Engine As String
    Dim MachineName As String
    Dim MachineID As String
    Public AircraftId As String

    Dim EventLogID As Guid
    Dim mLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mLogParameterList = CType(Session("mLogParameterList"), LogFuelAndOilList)
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        AircraftId = CType(Session("AircraftId"), String)
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mLogParameterList") = mLogParameterList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("AircraftId") = AircraftId
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mLogParameterList")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("AircraftId")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfLogParameterListNew_Ajax.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mLogParameterList")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("AircraftId")
        End If
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "1-1-1900", Optional ByVal ToDate As String = "1-1-3300", Optional ByVal MachineID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal Show_100_Records As Boolean = False)

        mLogParameterList = LogFuelAndOilList.GetLogFuelAndOilList(MachineID, FromDate, ToDate, , Show_100_Records)
        'Set DataSource of the Grid
        dgLogParameterList.DataSource = mLogParameterList
        Session("mLogParameterList") = mLogParameterList
        dgLogParameterList.DataBind()
        lblResult.Text = "As per criteria :" & mLogParameterList.Count & " Record(s) found."
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
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    GetSession()
                    DataFieldBind()
                Case MsgBoxResult.OK And Session("sender") = "Authorization"
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetControl()
        dgLogParameterList.DataBind()
    End Sub
    Public Sub ControlVisibility()
        For j As Integer = 0 To dgLogParameterList.Rows.Count - 1
            Dim P As New Integer
            If mLogParameterList(j).LogTypeID <> 1 Then
                dgLogParameterList.Rows(j).Cells(12).Enabled = False
            End If
        Next
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        Dim mLog As Log
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
        Dim mMachine As Machine = Machine.GetMachine(mMachineID)
        ''   Session("mLogList") = mLogList
        Session("mLogList") = Nothing
        Session("mMachine") = mMachine
        mLog = Log.GetLog(mID)
        mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
        Session("mLog") = mLog
        mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
        MarkLog(Util.Action.Edit, "Log Fuel Oil", mLogDetail, Util.ErrorType.NoError, mLog.ID, EventLogID)

        AircraftId = Session("MachineID")
        Session("mOpenFromParameterListNew") = True
        Session("OpenFromWO") = False

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLogParameterWindow", "OpenLogParameterWindow()", True)
    End Sub
    Private Sub DataFieldBind()

        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)

        If (Not IsDate(FromDate) Or Not IsDate(ToDate)) Or (FromDate = "1/1/1900" Or ToDate = "1/1/2200") Then
            txtFromDate.Text = ""
            txtToDate.Text = ""
        Else
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        End If

        txtFromDate.DataBind()
        txtToDate.DataBind()

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        If mMachineNameValueList.Count <> 0 Then
            If IsNothing(AircraftId) Then AircraftId = mMachineNameValueList(0).ID.ToString Else AircraftId = AircraftId
        Else
            AircraftId = "00000000-0000-0000-0000-000000000000"
        End If
        Session("AircraftId") = AircraftId
        cmbAircraft.DataBind()


        mLogParameterList = LogFuelAndOilList.GetLogFuelAndOilList(AircraftId, FromDate, ToDate, Guid.Empty.ToString, True)
        dgLogParameterList.DataSource = mLogParameterList
        dgLogParameterList.DataBind()
        Session("mLogParameterList") = mLogParameterList

        If mMachineNameValueList.Count > 1 And IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 1 Else cmbAircraft.SelectedValue = AircraftId
        AircraftId = cmbAircraft.SelectedValue
        Session("AircraftId") = AircraftId
        lblResult.Text = "As per criteria :" & mLogParameterList.Count & " Record(s) found."
        'DataBind()
    End Sub
#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfLogParameterListNew_Ajax.aspx?"
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Session("AircraftId") = cmbAircraft.SelectedValue
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        dgLogParameterList.PageIndex = 0

        If chkShowAll.Checked = True Then
            FindNow(FromDate, ToDate, mMachineID.ToString)
        Else
            FindNow(FromDate, ToDate, mMachineID.ToString, True)
        End If
        ControlVisibility()
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        Session("AircraftId") = cmbAircraft.SelectedValue
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        dgLogParameterList.PageIndex = 0

        If chkShowAll.Checked = True Then
            FindNow(FromDate, ToDate, mMachineID.ToString)
        Else
            FindNow(FromDate, ToDate, mMachineID.ToString, True)
        End If
        ControlVisibility()
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub

    Private Sub dgLogParameterList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLogParameterList.PageIndexChanging
        dgLogParameterList.PageIndex = e.NewPageIndex
        dgLogParameterList.DataSource = mLogParameterList
        Session("mLogParameterList") = mLogParameterList
        dgLogParameterList.DataBind()
    End Sub
    Private Sub dgLogParameterList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLogParameterList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgLogParameterList.PageIndex * dgLogParameterList.PageSize
                Dim mID As Guid = mLogParameterList(Index).ID
                Dim mLog As Log
                mLog = Log.GetLog(mID)
                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                'Added by Saylee on 8-Apr-2014 for ALL08042014
                If (Not User.IsInRole("LogParameterListNew") And mLog.IsNew) Or (Not User.IsInRole("LogParameterListEdit") And Not mLog.IsNew) Then
                    'setObject()
                    SetSession()
                    MarkLog(Util.Action.Edit, "LogParameterList", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DataFieldBind()
                SetControl()
                EditRecord(mID)
                ControlVisibility()
                upnlGridView.Update()
                upnlActionBtnTop.Update()
                upnlActionBtnBottom.Update()
                upnlResult.Update()
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnLogParameter_Click(sender As Object, e As System.EventArgs) Handles hdnBtnLogParameter.Click
        DataFieldBind()
        SetControl()
        ControlVisibility()
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub
#End Region

End Class