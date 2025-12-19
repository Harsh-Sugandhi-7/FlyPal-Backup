'Created by :   Saylee
'Date       :   15-Sep-2009

Partial Class wfSelectMaintenanceActivity
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Dim mMachineList As MachineList
    Dim MachineName As String
    Dim AsonDate As String
    Dim HourType As String
    Dim AssemblyName As String
    Dim LogId As String
    Dim mMultiComplianceList As MultiComplianceList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        AsonDate = Session("AsonDate")
        MachineName = Session("AircraftId")
        HourType = Session("HourType")
        AssemblyName = Session("AssemblyId")
        mMachineList = Session("mMachineListForCompliance")
        LogId = CType(Session("LogId"), String)
        mMultiComplianceList = CType(Session("mMultiComplianceList"), MultiComplianceList)
    End Sub
    Private Sub SetSession()
        Session("AsonDate") = AsonDate
        Session("AircraftId") = MachineName
        Session("HourType") = HourType
        Session("AssemblyId") = AssemblyName
        Session("mMachineListForCompliance") = mMachineList
        Session("LogId") = LogId
        Session("mMultiComplianceList") = mMultiComplianceList
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            If AsonDate Is Nothing Then AsonDate = Request.QueryString("DoneOn")
            If MachineName Is Nothing Then MachineName = Request.QueryString("MachineId")
            If HourType Is Nothing Then HourType = Request.QueryString("HourType")
            If AssemblyName Is Nothing Then AssemblyName = Request.QueryString("AssemblyID")
            Session("mLogList") = Nothing
            ''rdbRemovalComp.Checked = True
            rdbAssemblyService.Checked = True
            SetFocus(rdbAssemblyService)
        End If
        SetSession()
        ''MessageBoxResult()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click

        If rdbRemovalComp.Checked Then '1. Removal Comp
            Dim mInstalledCompList As tmpInstalledCompList
            mInstalledCompList = tmpInstalledCompList.GetInstalledCompList(AsonDate, MachineName, "", "", IIf(AssemblyName = "", Guid.Empty, New Guid(AssemblyName)))
            Session("mInstalledCompList") = mInstalledCompList
            Session("MaintenanceActivityTypeID") = 1
        End If

        If rdbInstallComp.Checked Then '2. Install Comp
            Dim mRemovedCompList As tmpRemovedCompList
            ' mInstalledCompList = tmpInstalledCompList.GetInstalledCompList(AsonDate, MachineName, "", "", IIf(AssemblyName = "", Guid.Empty, New Guid(AssemblyName)))
            mRemovedCompList = tmpRemovedCompList.GetRemovedCompList(AsonDate, MachineName, "", "", IIf(AssemblyName = "", Guid.Empty, New Guid(AssemblyName)))
            Session("mRemovedCompList") = mRemovedCompList
            Session("MaintenanceActivityTypeID") = 2
        End If

        If rdbAssemblyService.Checked Then '5. Assembly Service
            Dim mTmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
            mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, "", "", , , , , , , , True, , True)
            Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
            Session("MaintenanceActivityTypeID") = 5
        End If
        If rdbAssemblyInspection.Checked Then '6. Assembly Inspection
            Dim mTmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
            mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, "", "", , , , , , , , True, True)
            Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
            Session("MaintenanceActivityTypeID") = 6
        End If

        If rdbAssemblyDirective.Checked Then '7. Assembly Directive
            Dim mTmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList
            mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, "", "", , , , , , , , , True, True)
            Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
            Session("MaintenanceActivityTypeID") = 7
        End If

        If rdbComponentService.Checked Then '8. Component Service
            Dim mTmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
            mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , True, , True)
            Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
            Session("MaintenanceActivityTypeID") = 8
        End If
        If rdbComponentInspection.Checked Then '9. Component Inspection
            Dim mTmpComplyCompMonitorInspStatusList As tmpComplyCompMonitorInspStatusList
            mTmpComplyCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , True, True)
            Session("mTmpComplyCompMonitorInspStatusList") = mTmpComplyCompMonitorInspStatusList
            Session("MaintenanceActivityTypeID") = 9
        End If
        If rdbComponentDirective.Checked Then '10. Component Directive
            Dim mTmpComplyCompMonitorModStatusList As tmpComplyCompMonitorModStatusList
            mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , , True, True)
            Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
            Session("MaintenanceActivityTypeID") = 10
        End If

        Dim str As String
        str = "<script language='javascript'>openledgersame('wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfSelectMaintenanceActivity.aspx" & "&DoneOn=" & AsonDate & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString & "'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        mMachineList = Nothing
        Response.Redirect("index.aspx")
    End Sub
#End Region

End Class
