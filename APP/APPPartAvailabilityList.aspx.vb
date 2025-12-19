'Created By : Prashant 
'Dated: 7-Jun-2021
Imports System.Collections.Generic
Imports System.Linq
Public Class APPPartAvailabilityList
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    Dim mAPPPartAvailability As APPPartAvailability

    Dim ItemDescription As String = ""
    Dim ItemName As String = ""
    'Dim mEventLogSession As EventLogSetSession
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
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            ItemName = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            ItemDescription = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            ItemName = Trim(txtSearch.Text)
            ItemDescription = Trim(txtSearch.Text)
        End If

        mAPPPartAvailability = APPPartAvailability.GetAPPPartAvailability(PartName:=ItemName.Trim, Description:=ItemDescription.Trim, _
                                                                          ToDate:=Date.Today.ToString("dd-MMM-yyyy"), User:=mGBUser.Name)

        grdRosterList.DataSource = mAPPPartAvailability
        grdRosterList.DataBind()

        'Session("APPRosterList.App_CrewRosterList") = mApp_CrewRosterList

        lblTotalrecordCount.Text = "Total records found: " + mAPPPartAvailability.Count.ToString

        upnlRosterList.Update()
    End Sub

    Private Sub Loadcombos()

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

            End If
            ApplyRights()
        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try
    End Sub
    Protected Sub lnkSearch_Click(sender As Object, e As System.EventArgs) Handles lnkSearch.Click
        Try
            If txtSearch.Text.Trim = "" Then
                ShowAlertMsg("Alert ! Please select part no.", "Select part no.")
                Exit Sub
            End If
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