'AJAX Conversion by vikrant on 26-May-2015

Public Class wfCompMonitorServiceStatusListNew_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyList As AssemblyList
    Public mCompStatus As CompStatus
    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Public AircraftIdForService As String
    Private AssemblyID As String
    Private mCompCurrentStatusList As tmpCompCurrentStatusList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        AircraftIdForService = CType(Session("AircraftIdForService"), String)
        mAssemblyStatus = Session("mAssemblyStatus")
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompCurrentStatusList = CType(Session("mCompCurrentStatusList"), tmpCompCurrentStatusList)
        mMachine = CType(Session("mMachine"), Machine)
        AssemblyID = Session("AssemblyID1")
    End Sub
    Private Sub SetSession()
        Session("AircraftIdForService") = AircraftIdForService
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mMachine") = mMachine
        Session("AssemblyID1") = AssemblyID
        Session("mCompCurrentStatusList") = mCompCurrentStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("AircraftIdForService")
        Session.Remove("AssemblyID1")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mCompMonitorServiceStatus")
        Session.Remove("mCompCurrentStatusList")
        Session.Remove("mCompStatus")
        Session.Remove("mAssemblyList")
    End Sub
    Private Sub NewRecord()
        '    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(New Guid(cmbAssembly.SelectedValue.ToString))
        Dim CompStatusID As Guid
        If hdnCompStatusID.Value = String.Empty Or hdnCompStatusID.Value Is Nothing Then
            If mCompCurrentStatusList.Count > 0 Then
                CompStatusID = mCompCurrentStatusList(0).CompStatusID
            Else
                CompStatusID = Guid.Empty
            End If
        Else
            CompStatusID = New Guid(hdnCompStatusID.Value)
        End If
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate.ToString)
        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, txtIssueDate.Text, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)

        ' Session("mAssemblyStatus") = mAssemblyStatus  'mAssemblyStatus.AsOnDate
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mCompStatus") = mCompStatus
        Session("mIssueDate") = txtIssueDate.Text

        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "Component Service Status", " Part: " & mCompStatus.Comp.PartName & " Serial No.: " & mMachine.AssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
        'Response.Redirect("wfPartMonitorServiceList.aspx?BackPage=wfCompMonitorServiceStatusListNew.aspx")
        Session.Remove("mCompMonitorServiceStatusList")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPartMonitorServiceList_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetPage()
        If Not mMachine Is Nothing Then
            Dim ServiceMPDTitle As String = ""
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                ServiceMPDTitle = "Maintenance Event(s)"
            Else
                ServiceMPDTitle = "Services"
            End If

            lblTitle.Text = "Add New " + ServiceMPDTitle + " for Aircraft " + mMachine.RegNo
            lblServices.InnerText = "Select the Issue Date and Select Assembly from the list and then select Component for that Assembly and then click on OK button to add " + ServiceMPDTitle + "  to that Component."
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        Dim AssemId As Guid
        'Previous this List was Binded
        'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(Today.Date, AircraftIdForService, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , ).Item(0), MachineInfo).AssemblyStatusList
        'End
        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, AircraftIdForService, Today.Date.ToString, , True)
        cmbAssembly.DataSource = mAssemblyList
        If IsNothing(AssemblyID) Or AssemblyID = Guid.Empty.ToString Or AssemblyID = "" Then AssemId = mAssemblyList(0).AssemblyStatusID Else AssemId = New Guid(AssemblyID)

        AssemblyID = AssemId.ToString

        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyList(0).AssemblyStatusID)
        'mCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatusList(0).AssemblyID, "", "")
        'cmbComponent.DataSource = mCompStatusList
        mCompCurrentStatusList = tmpCompCurrentStatusList.GetCompCurrentStatusList(Today.Date.ToString, mAssemblyStatus.MachineID.ToString, mAssemblyStatus.ID.ToString)
        cmbComponent.DataSource = mCompCurrentStatusList

        cmbComponent.Enabled = mCompCurrentStatusList.Count > 0
        btnOk.Enabled = mCompCurrentStatusList.Count > 0

        mMachine = Machine.GetMachine(New Guid(AircraftIdForService))
        Session("mMachine") = mMachine
        If IsNothing(AssemblyID) Or AssemblyID = Guid.Empty.ToString Then cmbAssembly.SelectedIndex = 1 Else cmbAssembly.SelectedValue = AssemblyID

        Session("AssemblyID1") = cmbAssembly.SelectedValue
        Session("mAssemblyList") = mAssemblyList
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompCurrentStatusList") = mCompCurrentStatusList

        cmbAssembly.DataBind()
        cmbComponent.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            cmbAssembly.Focus()
            DataFieldBind()
            SetPage()
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            NewRecord()
        End If
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(New Guid(cmbAssembly.SelectedValue))
        'mCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", "", )

        'cmbComponent.DataSource = mCompStatusList
        'cmbComponent.Enabled = mCompStatusList.Count > 0
        'btnOk.Enabled = mCompStatusList.Count > 0
        'cmbComponent.DataBind()
        mCompCurrentStatusList = tmpCompCurrentStatusList.GetCompCurrentStatusList(Today.Date.ToString, mAssemblyStatus.MachineID.ToString, mAssemblyStatus.ID.ToString)

        cmbComponent.DataSource = mCompCurrentStatusList
        cmbComponent.Enabled = mCompCurrentStatusList.Count > 0
        btnOk.Enabled = mCompCurrentStatusList.Count > 0
        cmbComponent.DataBind()

        Session("mAssemblyStatus") = mAssemblyStatus
        'Session("mCompStatusList") = mCompStatusList
        Session("mCompCurrentStatusList") = mCompCurrentStatusList
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        If Session("NewPage") = "True" Then
            Session("NewPage") = "False"
        End If
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect("Index.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region


End Class