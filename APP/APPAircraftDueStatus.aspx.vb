

'Created By : Saylee 
'Dated: 7-Jun-2021


Imports System.Collections.Generic
Imports System.Linq



Public Class APPAircraftDueStatus
    Inherits System.Web.UI.Page



#Region "Variable Declaration"
    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    Public mrptDueReport As rptDueReport

    Public mMachineNameValueList As MachineNameValueList
    Dim EventLogID As Guid
#End Region

#Region "Helper Method"

    Private Sub GetSession()
        mUser = Session("User")
        mGBUser = Session("GBUser")
        'mEventLogSession = Session("EventLogSession")
    End Sub

    Private Sub ApplyRights()
        Try

            'BottomMenu Rights
            '          
            If User.IsInRole("AircraftCurrentStatusView") = False Then '
                hrefTimeline.Attributes("style") = "pointer-events: none"
                iTimeline.Attributes.Remove("style")
            End If

            ''Flights
            If User.IsInRole("LogView") = False Then
                hrefFlights.Attributes("style") = "pointer-events: none"
                iFlights.Attributes.Remove("style")
            End If

            ''Availability
            ''
            ''
            'hrefAvailability.Attributes("style") = "pointer-events: none"
            'iAvailability.Attributes.Remove("style")

            ''Profile
            'If (mUser.IsInRole("CrewNew") Or mUser.IsInRole("CrewEdit") Or mUser.IsInRole("CrewDelete") Or mUser.IsInRole("CrewView") Or mUser.IsInRole("CrewPrint")) = False Then
            '    hrefProfile.Attributes("style") = "pointer-events: none"
            '    iProfile.Attributes.Remove("style")
            'End If

            ''-----
            'Profile
            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")
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
#End Region

#Region "Data Bind"
    Private Sub BindGrid()
        Try



            mrptDueReport = rptDueReport.GetList(Today.Date.ToString, IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.ToString, ""))
            mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)

            Dim List = (From c As rptDueReport.rptDueReportInfo In mrptDueReport
                   Select c).ToList.Take(10)

            grdAircraftCurrentStatusList.DataSource = List
            grdAircraftCurrentStatusList.DataBind()

                'Session("APPRosterList.App_CrewRosterList") = mApp_CrewRosterList
            If grdAircraftCurrentStatusList.Rows.Count = 10 Then
                lblTotalrecordCount.Text = "Top 10 Records displayed"
            Else
                lblTotalrecordCount.Text = "Total records found: " + grdAircraftCurrentStatusList.Rows.Count.ToString
            End If

            upnlRosterList.Update()
            Session("mrptDueReport") = mrptDueReport
        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try

    End Sub
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        ' mMachineList = MachineList.GetMachineListMonitoringStatus(AOnDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(SELECT)", SkipIsForInventoryAircarft:=True)
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Private Sub Loadcombos()
        SetComboOfMachine(Today.Date.ToString)
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

    'Private Sub grdAircraftCurrentStatusList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAircraftCurrentStatusList.RowDataBound
    '    mrptDueReport = Session("mrptDueReport")
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        Dim txt As HtmlGenericControl = New HtmlGenericControl("span")
    '        txt = e.Row.FindControl("txtDue")
    '        Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))

    '        If mrptDueReport(ID).DueStatus = 1 Then
    '            txt.Attributes.Add("bgcolor", "red")
    '            '  txt.DataBind()
    '        ElseIf mrptDueReport(ID).DueStatus = 2 Then
    '            txt.Attributes.Add("bgcolor", "green")
    '            '   txt.DataBind()
    '        ElseIf mrptDueReport(ID).DueStatus = 3 Then
    '            txt.Attributes.Add("bgcolor", "yellow")
    '            '  txt.DataBind()
    '        End If

    '    End If
    'End Sub
    Protected Sub lnkHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHome.Click
        Try
            Response.Redirect("APPMenu.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub
#End Region


#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As ItemListAutoComplete
        itemlist = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

   
End Class