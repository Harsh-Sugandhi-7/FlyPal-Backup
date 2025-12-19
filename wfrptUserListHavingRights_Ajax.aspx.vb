Imports SI.UTILITY.UserListHavingRights

Public Class wfrptUserListHavingRights_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    Public mUserList As UserList
    Public mByMachineUserList As ByMachineUserList
    Public mByStoreUserList As ByStoreUserList
    Public mByDepartmentUserList As ByDepartmentUserList
    Dim EventLogID As Guid
    Dim templist As New System.Collections.ArrayList
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
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(All)")
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()

        mUserList = UserList.GetListofUser("", AddTopItem:="(All)")
        cmbUserList.DataSource = mUserList
        cmbUserList.DataBind()

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptUserListHavingRights_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptUserListHavingRights_Ajax.aspx?"
            DataFieldBind()
        End If
    End Sub

    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub

#End Region
    Private Sub rbAircraft_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbAircraft.CheckedChanged, rbUserwise.CheckedChanged
        If rbAircraft.Checked Then
            cmbUserList.Enabled = False
            cmbAircraft.Enabled = True
        ElseIf rbUserwise.Checked Then
            cmbUserList.Enabled = True
            cmbAircraft.Enabled = False
        End If
        upnlSearch.Update()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        If rbAircraft.Checked = True Then
            mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, cmbAircraft.SelectedValue)
        ElseIf rbUserwise.Checked = True Then
            mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, _
                                                                        Username:=IIf(cmbUserList.SelectedIndex = 0, "", cmbUserList.SelectedItem.Text))
        End If
        'mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, cmbAircraft.SelectedValue)

        For i As Integer = 0 To mMachineNameValueList.Count - 1
            mByMachineUserList = ByMachineUserList.GetByMachineUserList(mMachineNameValueList(i).ID)

            Dim variable As Object

            Dim Info As New UserListHavingRightsInfo

            For Each variable In mByMachineUserList
                Info = New UserListHavingRightsInfo
                Info.ID = New Guid(variable.ID.ToString)
                Info.UserID = New Guid(variable.UserID.ToString)
                Info.UserName = variable.UserName
                Info.MachineID = New Guid(variable.MachineID.ToString)
                Info.RegNo = mMachineNameValueList(i).RegNo
                Info.IsSelected = variable.IsSelected
                templist.Add(Info)
            Next
        Next

        'dgUserList.DataSource = templist
        'dgUserList.DataBind()
        'upnlMachine.Update()

        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsUserListHavingRights As New dsUserListHavingRights
        myReport = New crUserListHavingRights
        If rbAircraft.Checked = True Then
            myReport = New crUserListHavingRights
        ElseIf rbUserwise.Checked = True Then
            myReport = New crUserListHavingAircraftRights
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                                     mCompanyDetail.WebSite, "Audit Findings Report", _
                                     SearchStr1:=IIf(cmbUserList.SelectedIndex = 0, "", cmbUserList.SelectedItem.Text), SearchStr2:="", SearchStr3:="", _
                                     SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
                                     SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(dsUserListHavingRights)
        da.Fill(dsUserListHavingRights, "UserListHavingRights", templist)
        da.Fill(dsUserListHavingRights, Report)
        da.Fill(dsUserListHavingRights, mrptImage)
        myReport.SetDataSource(dsUserListHavingRights)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
End Class