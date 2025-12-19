Public Class wfUsermappingwithAircraftStoreDepartment_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    Public mStoreList As StoreList
    Public mByMachineUserList As ByMachineUserList
    Public mByStoreUserList As ByStoreUserList
    Public mByDepartmentUserList As ByDepartmentUserList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mByMachineUserList = Session("mByMachineUserList")
        mByStoreUserList = Session("mByStoreUserList")
        mByDepartmentUserList = Session("mByDepartmentUserList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("MiddleFrame")
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString)
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()

        mStoreList = StoreList.GetStoreList(0, "")
        cmbStore.DataSource = mStoreList
        cmbStore.DataBind()

        cmbDepartmentList.DataSource = EmployeeDepartmentList.GetEmployeeDepartmentList()
        cmbDepartmentList.DataBind()

        mByMachineUserList = ByMachineUserList.GetByMachineUserList(New Guid(cmbAircraft.SelectedValue))
        dgUserList.DataSource = mByMachineUserList
        dgUserList.DataBind()
        Session("mByMachineUserList") = mByMachineUserList
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUsermappingwithAircraftStoreDepartment_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub setGridObject()
        For i As Integer = 0 To dgUserList.Rows.Count - 1
            Dim chkselect As CheckBox
            chkselect = CType(dgUserList.Rows(i).FindControl("chkSelect"), CheckBox)
            mByMachineUserList.Item(i).IsSelected = chkselect.Checked
        Next
        Session("mByMachineUserList") = mByMachineUserList
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfUsermappingwithAircraftStoreDepartment_Ajax.aspx?"
            DataFieldBind()
        End If
    End Sub
    Private Sub btnSaveTop_Click(sender As Object, e As System.EventArgs) Handles btnSaveTop.Click, btnSaveBottom.Click
        If rbAircraft.Checked Then
            For i As Integer = 0 To dgUserList.Rows.Count - 1
                Dim chkselect As CheckBox
                chkselect = CType(dgUserList.Rows(i).FindControl("chkSelect"), CheckBox)
                mByMachineUserList.Item(i).IsSelected = chkselect.Checked
                If mByMachineUserList.Item(i).IsSelected = False Then
                    ByMachineUserList.AddUM_tabUserMachine(New Guid(cmbAircraft.SelectedValue), mByMachineUserList.Item(i).UserID)
                Else
                    ByMachineUserList.DeleteUM_tabUserMachine(New Guid(cmbAircraft.SelectedValue), mByMachineUserList.Item(i).UserID)
                End If
            Next
            mByMachineUserList = ByMachineUserList.GetByMachineUserList(New Guid(cmbAircraft.SelectedValue))
            dgUserList.DataSource = mByMachineUserList
            dgUserList.DataBind()
        ElseIf rbStore.Checked Then
            For i As Integer = 0 To dgUserList.Rows.Count - 1
                Dim chkselect As CheckBox
                chkselect = CType(dgUserList.Rows(i).FindControl("chkSelect"), CheckBox)
                mByStoreUserList.Item(i).IsSelected = chkselect.Checked
                If mByStoreUserList.Item(i).IsSelected = False Then
                    ByStoreUserList.AddUM_tabUserStore(New Guid(cmbStore.SelectedValue), mByStoreUserList.Item(i).UserID)
                Else
                    ByStoreUserList.DeleteUM_tabUserStore(New Guid(cmbStore.SelectedValue), mByStoreUserList.Item(i).UserID)
                End If
            Next
            mByStoreUserList = ByStoreUserList.GetUserStores(New Guid(cmbStore.SelectedValue))
            dgUserList.DataSource = mByStoreUserList
            dgUserList.DataBind()
        ElseIf rbDepartment.Checked Then
            For i As Integer = 0 To dgUserList.Rows.Count - 1
                Dim chkselect As CheckBox
                chkselect = CType(dgUserList.Rows(i).FindControl("chkSelect"), CheckBox)
                mByDepartmentUserList.Item(i).IsSelected = chkselect.Checked
                If mByDepartmentUserList.Item(i).IsSelected = False Then
                    ByDepartmentUserList.AddUM_tabUserEmployeeDepartment(New Guid(cmbDepartmentList.SelectedValue), mByDepartmentUserList.Item(i).UserID)
                Else
                    ByDepartmentUserList.DeleteUM_tabUserEmployeeDepartment(New Guid(cmbDepartmentList.SelectedValue), mByDepartmentUserList.Item(i).UserID)
                End If
            Next
            mByDepartmentUserList = ByDepartmentUserList.GetUserEmployeeDepartments(New Guid(cmbDepartmentList.SelectedValue))
            dgUserList.DataSource = mByDepartmentUserList
            dgUserList.DataBind()
        End If

        upnlMachine.Update()
        Session("mByMachineUserList") = mByMachineUserList
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseBottom.Click, btnCloseTop.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged, cmbStore.SelectedIndexChanged, cmbDepartmentList.SelectedIndexChanged
        If rbAircraft.Checked Then
            cmbStore.Enabled = False
            cmbDepartmentList.Enabled = False
            cmbAircraft.Enabled = True
            mByMachineUserList = ByMachineUserList.GetByMachineUserList(New Guid(cmbAircraft.SelectedValue))
            dgUserList.DataSource = mByMachineUserList
            dgUserList.DataBind()
            Session("mByMachineUserList") = mByMachineUserList
        ElseIf rbStore.Checked Then
            cmbStore.Enabled = True
            cmbDepartmentList.Enabled = False
            cmbAircraft.Enabled = False
            mByStoreUserList = ByStoreUserList.GetUserStores(New Guid(cmbStore.SelectedValue))
            dgUserList.DataSource = mByStoreUserList
            dgUserList.DataBind()
            Session("mByStoreUserList") = mByStoreUserList
        ElseIf rbDepartment.Checked Then
            cmbStore.Enabled = False
            cmbDepartmentList.Enabled = True
            cmbAircraft.Enabled = False
            mByDepartmentUserList = ByDepartmentUserList.GetUserEmployeeDepartments(New Guid(cmbDepartmentList.SelectedValue))
            dgUserList.DataSource = mByDepartmentUserList
            dgUserList.DataBind()
            Session("mByDepartmentUserList") = mByDepartmentUserList
        End If
        upnlSearch.Update()
        upnlMachine.Update()
    End Sub
#End Region

    Private Sub rbAircraft_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbAircraft.CheckedChanged, rbStore.CheckedChanged, rbDepartment.CheckedChanged
        If rbAircraft.Checked Then
            cmbStore.Enabled = False
            cmbDepartmentList.Enabled = False
            cmbAircraft.Enabled = True
            mByMachineUserList = ByMachineUserList.GetByMachineUserList(New Guid(cmbAircraft.SelectedValue))
            dgUserList.DataSource = mByMachineUserList
            dgUserList.DataBind()
            Session("mByMachineUserList") = mByMachineUserList
        ElseIf rbStore.Checked Then
            cmbStore.Enabled = True
            cmbDepartmentList.Enabled = False
            cmbAircraft.Enabled = False
            mByStoreUserList = ByStoreUserList.GetUserStores(New Guid(cmbStore.SelectedValue))
            dgUserList.DataSource = mByStoreUserList
            dgUserList.DataBind()
            Session("mByStoreUserList") = mByStoreUserList
        ElseIf rbDepartment.Checked Then
            cmbStore.Enabled = False
            cmbDepartmentList.Enabled = True
            cmbAircraft.Enabled = False
            mByDepartmentUserList = ByDepartmentUserList.GetUserEmployeeDepartments(New Guid(cmbDepartmentList.SelectedValue))
            dgUserList.DataSource = mByDepartmentUserList
            dgUserList.DataBind()
            Session("mByDepartmentUserList") = mByDepartmentUserList
        End If
        upnlSearch.Update()
        upnlMachine.Update()
    End Sub
End Class