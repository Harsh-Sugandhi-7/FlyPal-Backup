Imports System.IO
Public Class wfSchedule_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "

    Public mRoute As New Route
    Public mRouteSchedule As RouteSchedule
    Public FromPlace As Guid
    Public Toplace As Guid
    Public Departure As String
    Public Arrival As String
#End Region

#Region " Business Methods "

    Private Sub getSession()
        mRoute = Session("mRoute")
        mRouteSchedule = Session("mRouteSchedules")
    End Sub

    Private Sub setSession()
        Session("mRoute") = mRoute
        Session("mRouteSchedules") = mRouteSchedule
    End Sub

    Private Sub SetTitle()
        If mRoute.IsNew Then
            lblTitle.Text = "Schedule [New]"
        Else
            If Len(mRoute.RouteName) > 15 Then
                lblTitle.Text = "Schedule [" & mRoute.RouteName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Schedule [" & mRoute.RouteName & "]"
            End If
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        'lblResult.Text = "Aircraft List: " & mAirCraftMasterList.Count & " Record(s) Found."
    End Sub

    Private Sub setObject()
        mRoute.RouteName = txtRouteName.Text
        If Not IsDate(txtValidFrom.Text) Then
            mRoute.ValidFrom = Today.Date
        Else
            mRoute.ValidFrom = (CDate(txtValidFrom.Text))
        End If

        If Not IsDate(txtValidTo.Text) Then
            mRoute.ValidTo = Today.Date
        Else
            mRoute.ValidTo = CDate(txtValidTo.Text)
        End If
        mRoute.Note = TxtNote.Text

        Dim firstchild As Integer
        Dim totalWeeklyTime As Decimal = 0.0
        If mRoute.RouteSchedules.Count > 0 Then
            firstchild = mRoute.RouteSchedules(0).WeekDaysID
            For i As Integer = 0 To mRoute.RouteSchedules.Count - 1
                If i = 0 Then
                    totalWeeklyTime = mRoute.RouteSchedules(0).FlightTime
                Else
                    If firstchild = mRoute.RouteSchedules(i).WeekDaysID Then
                        Exit For
                    End If
                    totalWeeklyTime = totalWeeklyTime + mRoute.RouteSchedules(i).FlightTime
                End If
            Next

        End If
        mRoute.TotalWeeklyTime = totalWeeklyTime
        Session("mRoute") = mRoute
    End Sub

    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mRoute.RouteSchedules.CurrentIndex = Index
        Session("mRoute") = mRoute
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("Sender") = ""
                            Dim mroute As Route
                            mroute = CType(Session("mroute"), Route)
                        

                            'For i As Integer = 0 To mroute.RouteSchedules.Count - 1
                            '    If mroute.RouteSchedules.CurrentItem.FromPlaceID = mroute.RouteSchedules(i).FromPlaceID And mroute.RouteSchedules.CurrentItem.ToPlaceID = mroute.RouteSchedules(i).ToPlaceID And mroute.RouteSchedules.CurrentItem.DepartureTimeUTC = mroute.RouteSchedules(i).DepartureTimeUTC And mroute.RouteSchedules.CurrentItem.ArrivalTimeUTC = mroute.RouteSchedules(i).ArrivalTimeUTC Then
                            '        mroute.RouteSchedules.Remove(mroute.RouteSchedules.CurrentItem)
                            '    End If
                            'Next
                            FromPlace = mroute.RouteSchedules.CurrentItem.FromPlaceID
                            Toplace = mroute.RouteSchedules.CurrentItem.ToPlaceID
                            Departure = mroute.RouteSchedules.CurrentItem.DepartureTimeUTC
                            Arrival = mroute.RouteSchedules.CurrentItem.ArrivalTimeUTC

                            For i As Integer = mroute.RouteSchedules.Count - 1 To 0 Step -1
                                If FromPlace = mroute.RouteSchedules(i).FromPlaceID And Toplace = mroute.RouteSchedules(i).ToPlaceID And Format(CDate(Departure), AppSettings("TimeFormat")) = Format(CDate(mroute.RouteSchedules(i).DepartureTimeUTC), AppSettings("TimeFormat")) And Format(CDate(Arrival), AppSettings("TimeFormat")) = Format(CDate(mroute.RouteSchedules(i).ArrivalTimeUTC), AppSettings("TimeFormat")) Then
                                    mroute.RouteSchedules.Remove(mroute.RouteSchedules(i))
                                End If
                            Next

                            Session("mroute") = mroute
                            dgScheduleDetailList.DataSource = mroute.RouteSchedules
                            dgScheduleDetailList.DataBind()
                            upnlSchedule.Update()
                            lblResult.Text = "List of Schedule Details as per criteria :" & mroute.RouteSchedules.Count & " Record(s) found."
                            DataBind()
                            upnlRouteScheduleDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""

                        Response.Redirect("index.aspx")
                    Else
                        Session("Sender") = ""

                    End If

            End Select
        End If
    End Sub

    Private Sub Save()
        setObject()
        mRoute.Save()
        mRoute.MarkClean()
        dgScheduleDetailList.DataSource = mRoute.RouteSchedules
        dgScheduleDetailList.DataBind()
        upnlSchedule.Update()
        lblResult.Text = "List of Schedule Details as per criteria :" & mRoute.RouteSchedules.Count & " Record(s) found."
        DataBind()
        upnlRouteScheduleDetails.Update()
        'lblTitle.Text = "Route ( Saved ...)"
        ' upnlSchedule.Update()
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DatafieldBind()
        'mRoute.RouteSchedules.CurrentItem.DepartureTimeUTC
        dgScheduleDetailList.DataSource = mRoute.RouteSchedules
        dgScheduleDetailList.DataBind()
        'txtValidFrom.Text = mRoute.ValidFrom.ToString(AppSettings("DateFormat"))
        ' txtValidTo.Text = mRoute.ValidTo.ToString(AppSettings("DateFormat"))
        lblResult.Text = "List of Schedule Details as per criteria :" & mRoute.RouteSchedules.Count & " Record(s) found."
        DataBind()
        upnlRouteScheduleDetails.Update()
    End Sub
#End Region

#Region " Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        If Not IsPostBack Then
            DatafieldBind()
        End If
        SetTitle()    
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If (Not IsInRole(Rights.New) And Not IsInRole(Rights.Edit)) Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        If IsValid Then
            Save()
            Session("mRoute") = mRoute
            SetTitle()
            upnlActionBtn.DataBind()
            upnlActionBtn.Update()
            '' upnlSchedule.Update()
            upnlRouteScheduleDetails.Update()
        Else
            upnlValidationSAummary.Update()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("IsValid") = IsValid
        If mRoute.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                setObject()
            Else
                'upnlValidationSAummary.Update()
            End If
        Else
            mRoute = Nothing
            'Request.QueryString("BackPage") = "wfScheduleList_Ajax.aspx"
            Response.Redirect("index.aspx")
        End If
    End Sub

    'Private Sub hdnimgBtnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnSchedule.Click
    'End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Protected Sub btnAddScheduleDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddScheduleDetail.Click
        setObject()
        mRoute.RouteSchedules.Add(mRoute.ID)
        mRoute.RouteSchedules.CurrentIndex = mRoute.RouteSchedules.Count - 1
        Session("mRoute") = mRoute
        Session("EditmRoute") = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScheduleWindow", "OpenScheduleWindow();", True)
    End Sub

    Private Sub hdnBtnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSchedule.Click
        dgScheduleDetailList.DataSource = mRoute.RouteSchedules
        dgScheduleDetailList.DataBind()
        lblResult.Text = "List of Schedule Details as per criteria :" & mRoute.RouteSchedules.Count & " Record(s) found."
        DataBind()
        upnlRouteScheduleDetails.Update()
    End Sub

    Protected Sub dgScheduleDetailList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgScheduleDetailList.RowCommand
        Dim Index As Int32
        ' Dim Index1 As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument)
                Session("EditmRoute") = True
                setObject()
                mRoute.RouteSchedules.CurrentIndex = Index - 1
                Session("mRoute") = mRoute
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScheduleWindow", "OpenScheduleWindow();", True)
            Case "DeleteRec"
                'Index1 = New Guid(e.CommandArgument.ToString)
                ' DeleteRecord(Index1)
                Index = CInt(e.CommandArgument)
                DeleteRecord(Index - 1)
        End Select
    End Sub

    Protected Sub dgScheduleDetailList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgScheduleDetailList.PageIndexChanging
        dgScheduleDetailList.PageIndex = e.NewPageIndex
        dgScheduleDetailList.DataSource = mRoute.RouteSchedules
        Session("mRouteSchedule") = mRoute.RouteSchedules
        dgScheduleDetailList.DataBind()
    End Sub

    Protected Sub dgScheduleDetailList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgScheduleDetailList.Sorting
        mRoute.RouteSchedules.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRouteSchedule") = mRoute.RouteSchedules
        dgScheduleDetailList.DataSource = mRoute.RouteSchedules
        dgScheduleDetailList.DataBind()
    End Sub

    Private Sub dgScheduleDetailList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgScheduleDetailList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Cells(1).Text = e.Row.Cells(1).Text
            e.Row.Cells(2).Text = e.Row.Cells(2).Text
            e.Row.Cells(3).Text = e.Row.Cells(3).Text
            e.Row.Cells(4).Text = e.Row.Cells(4).Text
            e.Row.Cells(5).Text = DateTime.Parse(e.Row.Cells(5).Text).ToString(AppSettings("DateTimeFormatForImport"))
            e.Row.Cells(6).Text = DateTime.Parse(e.Row.Cells(6).Text).ToString(AppSettings("DateTimeFormatForImport"))
            e.Row.Cells(7).Text = e.Row.Cells(7).Text
        End If
    End Sub
#End Region
    
   
End Class