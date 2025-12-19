'AJAX Conversion by Vikrant on 25-May-2015

Public Class wfAssemblyMonitorInspStatusListNew_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyList As AssemblyList
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mMachine As Machine
    'Public SearchFor As String
    Public AircraftIdForInsp As String
    Public mAssemblyStatus As AssemblyStatus
    Public mIssueDate As String
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        AircraftIdForInsp = CType(Session("AircraftIdForInsp"), String)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
    End Sub
    Private Sub SetSession()
        Session("AircraftIdForInsp") = AircraftIdForInsp
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mAssemblyList") = mAssemblyList
        Session("mMachine") = mMachine
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
    End Sub
    Private Sub RemoveSession()
        ' Session.Remove("mAssemblyList")
        Session.Remove("mAssemblyMonitorInspStatusList")
        Session.Remove("mMachine")
        Session.Remove("AircraftIdForInsp")
        Session.Remove("mAssemblyStatus")
    End Sub
    Private Sub NewRecord()
        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(New Guid(cmbAssembly.SelectedValue.ToString))
        mIssueDate = txtIssueDate.Text
        Session("mAssemblyStatus") = mAssemblyStatus  'mAssemblyStatus.AsOnDate
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, New Guid(cmbAssembly.SelectedValue.ToString), txtIssueDate.Text, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            'MarkLog(Util.Action.[New], "AssemblyMonitorInspStatus", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            MarkLog(Util.Action.[New], "Assembly Inspection Status", User.Identity.Name & " is not Authorized User to add ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "Assembly Inspection Status", " Model : " & mMachine.AssemblyStatus.ModelName & " Serial No.: " & mMachine.AssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
        'Code  Added By Saylee on 1/4/2008 suggested by Deven sir
        Session("EditMasterRecord") = "False"
        Session("mIssueDate") = mIssueDate
        'Response.Redirect("wfModelMonitorInspList.aspx?BackPage=wfAssemblyMonitorInspStatusListNew.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfModelMonitorInspList_Ajax.aspx?BackPage=Index.aspx');", True)
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
            lblTitle.Text = "Add New Inspections for Aircraft " + mMachine.RegNo
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Previous this List was Binded
        'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(Today.Date, AircraftIdForInsp, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , ).Item(0), MachineInfo).AssemblyStatusList
        'End
        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, AircraftIdForInsp, Today.Date.ToString, , True)
        cmbAssembly.DataSource = mAssemblyList
        cmbAssembly.DataBind()
        Session("mAssemblyList") = mAssemblyList

        mMachine = Machine.GetMachine(New Guid(AircraftIdForInsp))
        Session("mMachine") = mMachine
        'mAssemblyStatusList = tmpAssemblyStatusList.GetAssemblyStatusList(mMachine.AssemblyStatus.AsOnDate.ToString, mMachine.ID, mMachine.AssemblyStatus.IsMaster)New Guid("{00000000-0000-0000-0000-000000000000}")'
        'mMachine.AssemblyStatus.AsOnDate
        ''''''mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mMachine.AssemblyStatus.AsOnDate, mMachine.AssemblyStatus.AssemblyID, New Guid(AircraftIdForInsp), True)
        ''''''Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        'DataBind()
        'SearchFor = Session("SearchFor")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            cmbAssembly.Focus()
            DataFieldBind()
            SetPage()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If IsValid Then
            NewRecord()
            btnAdd.Enabled = False
        End If
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