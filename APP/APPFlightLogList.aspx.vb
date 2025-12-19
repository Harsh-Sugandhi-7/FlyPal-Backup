'Created By : Saylee 
'Dated: 26-Dec-2023

Public Class APPFlightLogList
    Inherits System.Web.UI.Page



#Region "Variable Declaration"
    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    Dim mAPPAircraftLogList As LogList

    ''  Dim mMachineList As MachineList
    Public mMachineNameValueList As MachineNameValueList
    Dim EventLogID As Guid
    Dim mLogTypeList As LogTypeList
    Public mLog As Log
#End Region

#Region "Helper Method"

    Private Sub GetSession()
        mUser = Session("User")
        mGBUser = Session("GBUser")
        'mEventLogSession = Session("EventLogSession")
        mAPPAircraftLogList = Session("mAPPAircraftLogList")
    End Sub

    Private Sub ApplyRights()
        Try

            'BottomMenu Rights
            '
            'TimeLine
            If (mUser.IsInRole("CrewRosterNew") Or mUser.IsInRole("CrewRosterEdit") Or mUser.IsInRole("CrewRosterDelete") Or mUser.IsInRole("CrewRosterView") Or mUser.IsInRole("CrewRosterPrint")) = False Then
                hrefTimeline.Attributes("style") = "pointer-events: none"
                iTimeline.Attributes.Remove("style")
            End If

            'Flights
            If (mUser.IsInRole("FlightScheduleNew") Or mUser.IsInRole("FlightScheduleEdit") Or mUser.IsInRole("FlightScheduleDelete") Or mUser.IsInRole("FlightScheduleView") Or mUser.IsInRole("FlightSchedulePrint")) = False Then
                hrefFlights.Attributes("style") = "pointer-events: none"
                iFlights.Attributes.Remove("style")
            End If

            'Availability
            '
            '
            hrefAvailability.Attributes("style") = "pointer-events: none"
            iAvailability.Attributes.Remove("style")

            'Profile
            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")


            '-----
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ShowAlertMsg(ByVal Msg As String, ByVal MsgTitle As String)
        Dim str As String
        str = "opennotificationpopup('" & Msg & "','" & MsgTitle & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
    End Sub

    Private Sub DisableButtons()
        Try

            hrefTimeline.Attributes("style") = "pointer-events: none"
            iTimeline.Attributes.Remove("style")

            hrefFlights.Attributes("style") = "pointer-events: none"
            iFlights.Attributes.Remove("style")

            hrefAvailability.Attributes("style") = "pointer-events: none"
            iAvailability.Attributes.Remove("style")

            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")

        Catch ex As Exception

        End Try

    End Sub
    Private Function CHECK_isRequiredAssembliesInstalled(ByVal mLog As Log) As Boolean
        If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved Then
            ShowAlertMsg("You are trying To create New log. Selected machine does Not have required assemblies installed. ", "Entry Restriction..!!")
            Return False
            ' Exit Function
        End If
        ' Dim tmpAssemblyStatusList As tmpAssemblyStatusList = tmpAssemblyStatusList.GetAssemblyStatusList(Now.ToShortDateString, New Guid(cmbAircraft.SelectedValue),  True)
        Dim mLogAssemblyInstalledList As LogAssemblyInstalledList = LogAssemblyInstalledList.GetLogAssemblyInstalledList(MachineID:=New Guid(cmbAircraft.SelectedValue), CurrentDate:=Now.ToShortDateString)

        Dim IsAirFrameAvailable As Boolean = False
        Dim IsEngineAvailable As Boolean = False
        Dim AssembliesNotFound As String = ""
        ' Dim Obj As tmpAssemblyStatusList.tmpAssemblyStatusInfo
        Dim obj As LogAssemblyInstalledList.LogAssemblyInstalledListInfo

        For Each obj In mLogAssemblyInstalledList
            If obj.AssemblyTypeID = 1 Then IsAirFrameAvailable = True
            If obj.AssemblyTypeID = 2 Then IsEngineAvailable = True
        Next

        If (Not (IsAirFrameAvailable And IsEngineAvailable)) Then
            If IsEngineAvailable = False Then AssembliesNotFound = "Engine"
            If IsAirFrameAvailable = False Then AssembliesNotFound = AssembliesNotFound + IIf(AssembliesNotFound = "", "Machine", ", Machine").ToString

            ShowAlertMsg("Assembly Required for Selected Aircraft", "Entry Restriction..!!")
            Return False
            ' Exit Sub
        End If
        Return True
    End Function
    Private Sub EditRecord(ByVal Id As Guid)
        Dim mLog As Log
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
        Dim mMachine As Machine = Machine.GetMachine(mMachineID)

        ''   Session("mLogList") = mLogList
        Session("mLogList") = Nothing
        Session("LogListCount") = mAPPAircraftLogList.Count
        Session("mMachine") = mMachine
        Session("mAPPAircraftLogList") = mAPPAircraftLogList

        mLog = Log.GetLog(Id)
        mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
        mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
        mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
        Session("mLog") = mLog
        Dim mLogDetail As String
        'MarkLog(Util.Action.Edit, "Log", "Aircraft Name -> " + mMachine.RegNo + "Log TextNo -> " + mLog.LogTextNo, Util.ErrorType.NoError, mLog.ID)
        mLogDetail = mLog.LogTextNo.ToString + " Dated : " + mLog.DateFormatted
        MarkLog(Util.Action.Edit, "Flight Log", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)

        Dim str As String
        Session("mIsLastLog") = IIf((MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID, True).LogID).Equals(mLog.ID), True, False)

        If mLog.LogTypeID = 1 Then

            If mLog.IsTLP = True Then
                str = "APPFlightLogEntryTLP.aspx"

            Else
                str = "APPFlightLogEntry.aspx"
            End If
            '******************************************************************************************************
            Session("mLog") = mLog
            '--------------------------------------------------

            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Response.Redirect(str)
        Else
            ' ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVoidLogWindow", "OpenVoidLogWindow()", True)
            Response.Redirect("APPFlightLogEntryVoidMaint.aspx")
        End If
    End Sub
#End Region

#Region "Data Bind"
    Private Sub BindGrid()
        Try

            mAPPAircraftLogList = LogList.GetLogList(MachineID:=New Guid(cmbAircraft.SelectedValue.ToString), SouLocalDateTime:=txtFromDate.Text, DesLocalDateTime:=txtToDate.Text) ' APPPartAvailability.GetAPPPartAvailability(ItemName.Trim, ItemDescription.Trim, Date.Today.ToString("dd-MMM-yyyy"))

            grdAircraftCurrentStatusList.DataSource = mAPPAircraftLogList
            grdAircraftCurrentStatusList.DataBind()

            'Session("APPRosterList.App_CrewRosterList") = mApp_CrewRosterList
            Session("mAPPAircraftLogList") = mAPPAircraftLogList
            lblTotalrecordCount.Text = "Total records found:  " + mAPPAircraftLogList.Count.ToString

            upnlRosterList.Update()
        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try

    End Sub
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        'mMachineList = MachineList.GetMachineListMonitoringStatus(AOnDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(SELECT)", SkipIsForInventoryAircarft:=True)
        mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "<SELECT>", , SkipIsForInventoryAircarft:=True)
        Session("mMachineNameValueList") = mMachineNameValueList

        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()

    End Sub
    Private Sub Loadcombos()
        SetComboOfMachine(Today.Date.ToString)
        mLogTypeList = LogTypeList.GetLogTypeList()
        cmbLogType.DataSource = mLogTypeList
        cmbLogType.DataBind()


        If mMachineNameValueList.Count > 1 Then cmbAircraft.SelectedIndex = 1
    End Sub

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)
            If mGBUser Is Nothing Then
                ShowAlertMsg("Session expired ! Please click on Home button on left side menu.", "Session expired..")
                DisableButtons()
                Exit Sub
            End If

            If Not IsPostBack Then
                Loadcombos()
            End If
            ApplyRights()
        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try
    End Sub
    Protected Sub lnkSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSearch.Click
        Try
            BindGrid()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        Try
            BindGrid()
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub lnkHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHome.Click
        Try
            Response.Redirect("APPMenu.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        If (Not User.IsInRole("LogNew")) Then
            MarkLog(Util.Action.[New], "Flight Log", User.Identity.Name & " is not Authorized User to add ", Util.ErrorType.NoError, Guid.Empty, EventLogID)   'Added By Prashant 20-Jul-2011
            ' MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            ShowAlertMsg("User not authorized. Please contact the Administrator.", "Access Denied")
            Exit Sub
        End If


        Session.Remove("isvaluezero")
        Session.Remove("mFileAttach")

        Dim mMachine As Machine = Machine.GetMachine(New Guid(cmbAircraft.SelectedValue.ToString))

        Session("mMachine") = mMachine
        Session("LogListCount") = mAPPAircraftLogList.Count

        If mMachine.IsReadOnly Then
            ShowAlertMsg("As <b>" & cmbAircraft.SelectedItem.ToString & "</b> is marked as ReadOnly,You can not add new Flight Log Entry.", "Alert..!!")
            Exit Sub
        End If
        Dim str As String
        Dim str1 As String
        str1 = "delete_cookie();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)

        If cmbLogType.SelectedValue = 1 Then
            mLog = Log.NewLog(mMachine, Today.ToShortDateString, , , cmbLogType.SelectedValue)
            mLog.IsTLP = mMachine.IsTLP
            If mLog.IsTLP = True Then
                str = "APPFlightLogEntryTLP.aspx"

            Else
                str = "APPFlightLogEntry.aspx"
            End If

            mLog.IsUTC = mMachine.IsUTC


            mLog.LogTypeID = cmbLogType.SelectedValue
            Session("mLog") = mLog
            '--------------------------------------------------

            If CHECK_isRequiredAssembliesInstalled(mLog) = True Then
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", Str, True)
                Response.Redirect(str)
            End If
        Else
            Dim LogType As String = String.Empty

            If cmbLogType.SelectedValue = 2 Then
                LogType = "MAINT. LOG"
            Else
                LogType = "VOID LOG"
            End If
            Dim LogDate As String
            If (mAPPAircraftLogList(0).DesUniverseDateTime).ToString = "" Then
                LogDate = mAPPAircraftLogList(0).Date.ToString
            Else
                If mMachine.IsUTC Then
                    LogDate = (mAPPAircraftLogList(0).DesUniverseDateTime)
                Else
                    LogDate = (mAPPAircraftLogList(0).DesLocalDateTime)
                End If

            End If
            mLog = Log.NewLog(mMachine, LogDate, , , cmbLogType.SelectedValue)
            mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
            mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
            mLog.LogTypeID = cmbLogType.SelectedValue
            Session("mLog") = mLog
            ' MSGBoxCtrl.Show("Alert!!", "You are about to enter " + LogType + " and last TLP No. is " + mLogList(0).LogPageNo, "Please enter Next TLP No.", MsgBoxStyle.OkOnly, "NextTLP")
            Response.Redirect("APPFlightLogEntryVoidMaint.aspx")
        End If
        MarkLog(Util.Action.[New], "Flight Log", "User clicked on ADD New button from APP", Util.ErrorType.NoError, mLog.ID, EventLogID)
    End Sub

    Private Sub grdAircraftCurrentStatusList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdAircraftCurrentStatusList.RowCommand
        Dim Index As Int32
        Dim ID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + gdvLogList.PageIndex * gdvLogList.PageSize
                Session("Index") = Index
                Session.Remove("isvaluezero")
                Session.Remove("mFileAttach")
                ID = mAPPAircraftLogList(Index).ID
                Dim mLogDetail As String
                mLogDetail = mAPPAircraftLogList(Index).LogTextNo + " Dated : " + mAPPAircraftLogList(Index).DateFormatted

                If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
                    MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ShowAlertMsg("User not authorized. Please contact the Administrator.", "Access Denied")
                    Exit Sub
                End If
                EditRecord(ID)
        End Select
    End Sub
#End Region

End Class