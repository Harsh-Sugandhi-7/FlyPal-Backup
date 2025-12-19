Public Class wfMachineMaintenanceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineMaintenanceList As MachineMaintenanceList
    'Private mMachineNameValueList As MachineList

    Private mMachineNameValueList As MachineNameValueList

    Private mMaintenanceActivityTypeList As MaintenanceActivityType
    Private mAssemblylist As AssemblyList

    Public AircraftId As String
    Public AssemblyId As String

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList)
    End Sub
    Private Sub SetSession()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineMaintenanceList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfMachineMaintenanceList.aspx?" Then
            Session.Remove("mMachineMaintenanceList")
        End If
    End Sub
    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub SetPage()
        lblResult.Text = "List of Aircraft Maintenance : " & mMachineMaintenanceList.Count & " Record(s) found"
    End Sub
    Private Sub FindNow()
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList(txtFromDate.Text.ToString, txtToDate.Text.ToString, , cmbAircraftList.SelectedValue.ToString, cmbMaintenanceActivity.SelectedValue, cmbAssembly.SelectedValue.ToString)
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        dgMachineMaintenanceList.DataSource = mMachineMaintenanceList
        dgMachineMaintenanceList.DataBind()

    End Sub
    Private Sub ControlVisibility()
        'btnBackTop.Visible = mMachineMaintenanceList.Count > 25
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        Dim MachineId As String, AssemId As Guid
        'Verify change
        'mMachineNameValueList = MachineList.GetMachineListMonitoringStatus(Today.Date.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(ALL)")
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, , , , , , , True, "(ALL)", , True)

        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraftList.DataSource = mMachineNameValueList
        cmbAircraftList.DataBind()

        If mMachineNameValueList.Count > 1 And (IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString) Then
            'Verify change
            MachineId = mMachineNameValueList(1).ID.ToString
            AssemblyId = Guid.Empty.ToString
        Else
            MachineId = AircraftId
        End If

        mAssemblylist = AssemblyList.GetAssemblyList(0, MachineId, Today.Date.ToShortDateString, "(ALL)")
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        cmbAssembly.DataBind()

        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then AssemId = mAssemblylist(0).ID Else AssemId = New Guid(AssemblyId)

        AssemblyId = AssemId.ToString

        ''mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList(txtFromDate.Text.ToString, txtToDate.Text.ToString, , MachineId, , AssemId.ToString)
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        dgMachineMaintenanceList.DataSource = mMachineMaintenanceList
        dgMachineMaintenanceList.DataBind()

        mMaintenanceActivityTypeList = MaintenanceActivityType.GetMaintenanceActivityTypeList(True, "(ALL)")
        cmbMaintenanceActivity.DataSource = mMaintenanceActivityTypeList
        cmbMaintenanceActivity.DataBind()

        If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then cmbAircraftList.SelectedIndex = 1 Else cmbAircraftList.SelectedValue = AircraftId

        If IsNothing(AssemblyId) Or AssemblyId = Guid.Empty.ToString Then cmbAssembly.SelectedIndex = 0 Else cmbAssembly.SelectedValue = AssemblyId

        Session("MachineId") = cmbAircraftList.SelectedValue
        Session("AssemblyId") = cmbAssembly.SelectedValue
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            txtFromDate.Text = Format(CDate(Today.Date.ToString), AppSettings("DateFormat"))  'Today.Date.ToString
            txtToDate.Text = Format(CDate(Today.Date.ToString), AppSettings("DateFormat"))    'Today.Date.ToString

            DataFieldBind()
            SetFocus(cmbAircraftList)
            Session("MiddleFrame") = "wfMachineMaintenanceList_Ajax.aspx?"
        End If
        SetPage()
        ControlVisibility()
        'MessageBoxResult()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If IsValid Then
            FindNow()
            SetFocus(cmbAircraftList)
            ControlVisibility()
            SetPage()
        End If

    End Sub
    Private Sub cmbAircraftList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
        mAssemblylist = AssemblyList.GetAssemblyList(0, New Guid(cmbAircraftList.SelectedValue.ToString).ToString, Today.Date.ToString, "(ALL)")
        cmbAssembly.DataSource = mAssemblylist
        Session("mAssemblylist") = mAssemblylist
        cmbAssembly.DataBind()
        If cmbAircraftList.Enabled = True Then
            SetFocus(cmbAircraftList)
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click ', btnBackTop.Click
        SetSession()
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub dgMachineMaintenanceList_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgMachineMaintenanceList.SortCommand
    '    mMachineMaintenanceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    '    Session("mMachineMaintenanceList") = mMachineMaintenanceList
    '    dgMachineMaintenanceList.DataSource = mMachineMaintenanceList
    '    dgMachineMaintenanceList.DataBind()
    'End Sub
    'Private Sub dgMachineMaintenanceList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgMachineMaintenanceList.PageIndexChanged
    '    dgMachineMaintenanceList.CurrentPageIndex = e.NewPageIndex

    '    dgMachineMaintenanceList.DataSource = mMachineMaintenanceList
    '    dgMachineMaintenanceList.DataBind()

    '    Session("mMachineMaintenanceList") = mMachineMaintenanceList
    '    SetFocus(cmbAircraftList)

    'End Sub

    Private Sub dgMachineMaintenanceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMachineMaintenanceList.PageIndexChanging
        dgMachineMaintenanceList.PageIndex = e.NewPageIndex

        dgMachineMaintenanceList.DataSource = mMachineMaintenanceList
        dgMachineMaintenanceList.DataBind()

        Session("mMachineMaintenanceList") = mMachineMaintenanceList
    End Sub

    Private Sub dgMachineMaintenanceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMachineMaintenanceList.Sorting
        mMachineMaintenanceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        dgMachineMaintenanceList.DataSource = mMachineMaintenanceList
        dgMachineMaintenanceList.DataBind()
    End Sub
#End Region
End Class