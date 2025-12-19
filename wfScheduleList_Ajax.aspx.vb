Public Class wfScheduleList_Ajax
    Inherits System.Web.UI.Page

#Region "variable declaration"
    Public mRoute As Route
    Public mRouteList As RouteList
    Public mTransTypeID As Trans
    Dim SearchIndex, DateIndex, FromDate, ToDate, RouteName As String
#End Region

#Region "Business Properties and Methods"

    Private Sub getSession()
        mRouteList = Session("mRouteList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        RouteName = Session("RouteName")
    End Sub

    Private Sub setSession()
        Session("mRouteList") = mRouteList
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("RouteName") = RouteName
    End Sub

    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        RouteName = txtRouteName.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("RouteName") = RouteName
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
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub

    Private Sub SetControl()
        setPeriod(DateIndex)
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        RouteName = txtRouteName.Text
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        CallFindNow(SearchIndex, 1)
        dgScheduleList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
        txtRouteName.Text = RouteName
        ControlVisibility(SearchIndex)

        lblResult.Text = "List of Schedule as per criteria :" & mRouteList.Count & " Record(s) found."

    End Sub

    Private Sub CallFindNow(ByVal Index As Integer, Optional ByVal IsForPrint As Boolean = False)
        Select Case Index
            Case -1 'all
                FindNow()
            Case 0   'all
                FindNow()
            Case 1  'Schedule date
                FindNow(, FromDate, ToDate)
            Case 2 'Schedule
                FindNow(RouteName)
        End Select
        dgScheduleList.PageIndex = 0   'Added Code on May,25,2007
    End Sub

    Private Sub FindNow(Optional ByVal RouteName As String = "", Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/3300")
        mRouteList = RouteList.GetRouteList(RouteName, FromDate, ToDate)
        Session("mRouteList") = mRouteList
        dgScheduleList.DataSource = mRouteList
        dgScheduleList.DataBind()
        lblResult.Text = "List of Schedule as per criteria :" & mRouteList.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub

    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = CBool(IIf(SearchIndex = 1, True, False))
        lblFromDate.Visible = CBool(IIf(SearchIndex = 1 And DateIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(SearchIndex = 1 And DateIndex <> 0, True, False))
        txtFromDate.Visible = False
        txtToDate.Visible = False
        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        ElseIf SearchIndex = 1 And (DateIndex = 0) Then
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        txtRouteName.Visible = CBool(IIf(SearchIndex = 2, True, False))
    End Sub

    'Private Sub NewRecord()
    '    'mRoute = Route.NewRoute(mTransTypeID)
    '    'mEnquiry.Date = Today.Date
    '    'Session("mEnquiry") = mEnquiry
    '    'Session("mTransTypeID") = mTransTypeID
    'End Sub

    Private Sub EditRecord(ByVal mId As Guid)
        mRoute = Route.GetRoute(mId)
        mRoute.MarkClean()
        Session("mRoute") = mRoute
    End Sub

    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mRoute = Route.GetRoute(mId)
        Session("mRoute") = mRoute
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
                            mroute.Delete()
                            mroute.Save()
                            DatafieldBind()
                            upnlRouteScheduleList.Update()
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
                        Response.Redirect("wfScheduleList_Ajax.aspx")
                        ''==========================================WO - 2006-2007-1-17.doc
                    Else
                        Session("Sender") = ""

                    End If

            End Select
        End If
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DatafieldBind()
        mRouteList = RouteList.GetRouteList()
        Session("mRouteList") = mRouteList
        dgScheduleList.DataSource = mRouteList
        dgScheduleList.DataBind()
        upnlGridView.Update()
    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfScheduleList_Ajax.aspx"
            DatafieldBind()
            SearchIndex = 1
            DateIndex = 1
            SetControl()
        End If
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        mRoute = Route.NewRoute()
        Session("mRoute") = mRoute
        ' Response.Redirect("wfSchedule_Ajax.aspx")
        Dim str As String
        str = "openledgersame('wfSchedule_Ajax.aspx?BackPage=index.aspx&mType=1');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Protected Sub dgScheduleList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgScheduleList.RowCommand
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                'dgScheduleList.DataSource = mRouteList
                'dgScheduleList.DataBind()
                'Index = CInt(e.CommandArgument) + dgScheduleList.PageSize * dgScheduleList.PageIndex
                'mID = mRouteList(Index).ID
                'EditRecord(mID)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScheduleListWindow", "OpenScheduleListWindow();", True)
                mID = New Guid(e.CommandArgument.ToString)
                mRoute = Route.GetRoute(mID)
                mRoute.MarkClean()
                Session("mRoute") = mRoute
                Dim str As String
                str = "openledgersame('wfSchedule_Ajax.aspx?BackPage=index.aspx&mType=1');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRec"
                mID = New Guid(e.CommandArgument.ToString)
                'dgScheduleList.DataSource = mRouteList
                'dgScheduleList.DataBind()
                'Index = CInt(e.CommandArgument) + dgScheduleList.PageSize * dgScheduleList.PageIndex
                'mID = mRouteList(Index).ID
                DeleteRecord(mID)
        End Select
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

#End Region
    
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(cmbSearch.SelectedIndex)
        ControlVisibility(cmbSearch.SelectedIndex)
    End Sub

    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDate.SelectedIndex = 0
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        '' ControlVisibility(cmbSearchCriteria.SelectedIndex, 0, 0, 0)
        If cmbSearch.Enabled = True Then
            SetFocus(cmbSearch)
        End If
    End Sub

    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        'ClearControl()
        Dim DateIndex As Int32 = CInt(IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0))
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If
    End Sub
   
    Protected Sub dgScheduleList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgScheduleList.PageIndexChanging
        dgScheduleList.PageIndex = e.NewPageIndex
        dgScheduleList.DataSource = mRouteList
        Session("mRouteList") = mRouteList
        dgScheduleList.DataBind()
    End Sub

    Protected Sub dgScheduleList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgScheduleList.Sorting
        mRouteList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRouteList") = mRouteList
        dgScheduleList.DataSource = mRouteList
        dgScheduleList.DataBind()
    End Sub

End Class