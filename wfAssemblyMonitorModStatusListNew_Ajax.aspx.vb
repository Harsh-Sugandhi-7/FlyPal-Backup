'AJAX Conversion by Vikrant on 25-May-2015

Public Class wfAssemblyMonitorModStatusListNew_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyList As AssemblyList
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mMachine As Machine
    'Public SearchFor As String
    Public AircraftIdForMod As String
    Public mAssemblyStatus As AssemblyStatus
    Public EventLogID As Guid

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        AircraftIdForMod = CType(Session("AircraftIdForMod"), String)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
    End Sub
    Private Sub SetSession()
        Session("AircraftIdForMod") = AircraftIdForMod
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mAssemblyList") = mAssemblyList
        Session("mMachine") = mMachine
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
    End Sub
    Private Sub RemoveSession()
        'Session.Remove("mAssemblyList")
        Session.Remove("mAssemblyMonitorModStatusList")
        Session.Remove("mMachine")
        Session.Remove("AircraftIdForMod")
        Session.Remove("mAssemblyStatus")
    End Sub
    Private Sub NewRecord()
        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(New Guid(cmbAssembly.SelectedValue.ToString))
        Session("mAssemblyStatus") = mAssemblyStatus  'mAssemblyStatus.AsOnDate
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, New Guid(cmbAssembly.SelectedValue.ToString), mAssemblyStatus.AsOnDate, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            MarkLog(Util.Action.[New], "Assembly Directive Status", User.Identity.Name & " is not Authorized User to add ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "Assembly Directive Status", " Model: " & mMachine.AssemblyStatus.ModelName & " Serial No.: " & mMachine.AssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
        'Code  Added By Saylee on 1/4/2008 suggested by Deven sir
        Session("EditMasterRecord") = "False"
        'Response.Redirect("wfModelMonitorModList.aspx?BackPage=wfAssemblyMonitorModStatusListNew.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfModelMonitorModList_Ajax.aspx?BackPage=Index.aspx');", True)
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
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '  DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        If Not mMachine Is Nothing Then
            lblTitle.Text = "Add New Directives for Aircraft " + mMachine.RegNo
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Previous this List was Binded
        'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(Today.Date, AircraftIdForMod, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , ).Item(0), MachineInfo).AssemblyStatusList
        'End
        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, AircraftIdForMod, Today.Date.ToString, , True)
        cmbAssembly.DataSource = mAssemblyList
        cmbAssembly.DataBind()
        Session("mAssemblyList") = mAssemblyList

        mMachine = Machine.GetMachine(New Guid(AircraftIdForMod))
        Session("mMachine") = mMachine
        '---------
        'mAssemblyStatusList = tmpAssemblyStatusList.GetAssemblyStatusList(mMachine.AssemblyStatus.AsOnDate.ToString, mMachine.ID, mMachine.AssemblyStatus.IsMaster)New Guid("{00000000-0000-0000-0000-000000000000}")'
        '--------
        'mMachine.AssemblyStatus.AsOnDate
        '''''''''mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mMachine.AssemblyStatus.AsOnDate, mMachine.AssemblyStatus.AssemblyID, New Guid(AircraftIdForMod), True)
        '''''''''Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        'DataBind()
        'SearchFor = Session("SearchFor")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            cmbAssembly.Focus()
            DataFieldBind()
            SetPage()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
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