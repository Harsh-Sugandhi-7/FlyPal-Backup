
'Created by : Saylee
'Date       : 15-Sep-2009

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq

Imports System
Imports System.IO
Partial Class wfMultiComplanceListPartII
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtAsOnDate As SIControls.SICalendar

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Enumeration "
    Enum MaintenanceActivityTypes
        RemovalComp = 1
        InstallComp = 2
        RemovalAssembly = 3
        InstallAssembly = 4
        AssemblyService = 5
        AssemblyInspection = 6
        AssemblyDirective = 7
        ComponentService = 8
        ComponentInspection = 9
        ComponentDirective = 10
    End Enum
#End Region

#Region " Variable Declaration "

    Dim mMachineList As MachineList

    Dim mtmpMachineList As tmpMachineList

    Private Flag As Int16
    Dim AOdate As String
    Dim AOnDate As String
    Dim Average As String
    Dim Aircraft As String
    Dim Periodcount As Integer
    Dim MachineName As String
    Dim AsonDate As String
    Dim Type As Integer = 1
    Dim AssemblyID As Guid
    Private AssemblyType As String
    ''  Private DueType As Integer
    Dim AircraftIndex As Integer
    Dim mAssemblyStatusList As AssemblyStatusList
    Dim AssemblyName As String
    Dim Assembly1 As String
    Private AssemblyStatusID As String
    Private ModelID As String
    Dim LogId As String
    Dim LogDate As String
    Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
    Dim tmpAssemblyStatusID As Guid
    Dim HourType As String

    Dim mMultiComplianceList As New MultiComplianceList
    Dim mRemovalReasonList As RemovalReasonList
    Dim str As String = ""

    Dim mInstalledCompList As tmpInstalledCompList
    Dim mRemovedCompList As tmpRemovedCompList

    Dim mTmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
    Dim mTmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
    Dim mTmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList

    Dim mTmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
    Dim mTmpComplyCompMonitorInspStatusList As tmpComplyCompMonitorInspStatusList
    Dim mTmpComplyCompMonitorModStatusList As tmpComplyCompMonitorModStatusList

    Private checkedIds As New List(Of String)()
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineListForCompliance"), MachineList)

        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)

        AsonDate = Session("AsonDate")
        MachineName = Session("AircraftId")
        AssemblyName = Session("AssemblyId")
        AssemblyType = Session("AssemblyType")

        HourType = Session("HourType")
        AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")
        Aircraft = Session("Aircraft")
        LogId = CType(Session("LogId"), String)

        mInstalledCompList = Session("mInstalledCompList")
        mRemovedCompList = Session("mRemovedCompList")
        mRemovalReasonList = Session("mRemovalReasonList")
        mMultiComplianceList = IIf(Not Session("mMultiComplianceList") Is Nothing, Session("mMultiComplianceList"), mMultiComplianceList)

        mTmpComplyAssemblyMonitorServiceStatusList = Session("mTmpComplyAssemblyMonitorServiceStatusList")
        mTmpComplyAssemblyMonitorInspStatusList = Session("mTmpComplyAssemblyMonitorInspStatusList")
        mTmpComplyAssemblyMonitorModStatusList = Session("mTmpComplyAssemblyMonitorModStatusList")

        mTmpComplyCompMonitorServiceStatusList = Session("mTmpComplyCompMonitorServiceStatusList")
        mTmpComplyCompMonitorInspStatusList = Session("mTmpComplyCompMonitorInspStatusList")
        mTmpComplyCompMonitorModStatusList = Session("mTmpComplyCompMonitorModStatusList")
    End Sub
    Private Sub SetSession()
        Session("mMachineListForCompliance") = mMachineList
        ''  Session("DueType") = DueType
        Session("AssemblyType") = AssemblyType
        Session("mAssemblyStatusList") = mAssemblyStatusList
        Session("HourType") = HourType
        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
        Session("Aircraft") = Aircraft

        Session("LogId") = LogId
        Session("AsonDate") = AsonDate
        Session("AircraftId") = MachineName
        Session("HourType") = HourType
        Session("AssemblyId") = AssemblyName

        Session("mInstalledCompList") = mInstalledCompList
        Session("mRemovedCompList") = mRemovedCompList
        Session("mRemovalReasonList") = mRemovalReasonList

        Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
        Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
        Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList

        Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
        Session("mTmpComplyCompMonitorInspStatusList") = mTmpComplyCompMonitorInspStatusList
        Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList

    End Sub
    Private Sub ClearAll()
        ''DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfMultiComplanceListListPartII.aspx?" Then
            Session.Remove("mMachineListForCompliance")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Type")
            Session.Remove("AvgMnths")

            Session.Remove("mAssemblyStatusList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")
            Session.Remove("OpenFindNowSelectLogForm")
            Session.Remove("AssemblyStatusPeriodList")
            Session.Remove("HourType")
            Session.Remove("mRemovalReasonList")
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfMultiComplanceListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfMultiComplanceListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        End If
    End Sub
    Private Sub Controltovisibility()
        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp
                If Not mInstalledCompList Is Nothing Then
                    btnAddToCartTop.Visible = mInstalledCompList.Count > 10
                    btnNextTop.Visible = mInstalledCompList.Count > 10
                    btnCloseTop.Visible = mInstalledCompList.Count > 10
                End If
            Case MaintenanceActivityTypes.InstallComp
                If Not mRemovedCompList Is Nothing Then
                    btnAddToCartTop.Visible = mRemovedCompList.Count > 10
                    btnNextTop.Visible = mRemovedCompList.Count > 10
                    btnCloseTop.Visible = mRemovedCompList.Count > 10
                End If
            Case MaintenanceActivityTypes.AssemblyService  '5. Assembly Service
                If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
                    btnAddToCartTop.Visible = mTmpComplyAssemblyMonitorServiceStatusList.Count > 10
                    btnNextTop.Visible = mTmpComplyAssemblyMonitorServiceStatusList.Count > 10
                    btnCloseTop.Visible = mTmpComplyAssemblyMonitorServiceStatusList.Count > 10
                End If
            Case MaintenanceActivityTypes.AssemblyInspection   '6. Assembly Inspection
                If Not mTmpComplyAssemblyMonitorInspStatusList Is Nothing Then
                    btnAddToCartTop.Visible = mTmpComplyAssemblyMonitorInspStatusList.Count > 10
                    btnNextTop.Visible = mTmpComplyAssemblyMonitorInspStatusList.Count > 10
                    btnCloseTop.Visible = mTmpComplyAssemblyMonitorInspStatusList.Count > 10
                End If
            Case MaintenanceActivityTypes.AssemblyDirective    '7. Assembly Directive
                If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
                    btnAddToCartTop.Visible = mTmpComplyAssemblyMonitorModStatusList.Count > 10
                    btnNextTop.Visible = mTmpComplyAssemblyMonitorModStatusList.Count > 10
                    btnCloseTop.Visible = mTmpComplyAssemblyMonitorModStatusList.Count > 10
                End If
            Case MaintenanceActivityTypes.ComponentService    '8. Component Service
                If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
                    btnAddToCartTop.Visible = mTmpComplyCompMonitorServiceStatusList.Count > 10
                    btnNextTop.Visible = mTmpComplyCompMonitorServiceStatusList.Count > 10
                    btnCloseTop.Visible = mTmpComplyCompMonitorServiceStatusList.Count > 10
                End If
            Case MaintenanceActivityTypes.ComponentInspection     '9. Component Inspection
                If Not mTmpComplyCompMonitorInspStatusList Is Nothing Then
                    btnAddToCartTop.Visible = mTmpComplyCompMonitorInspStatusList.Count > 10
                    btnNextTop.Visible = mTmpComplyCompMonitorInspStatusList.Count > 10
                    btnCloseTop.Visible = mTmpComplyCompMonitorInspStatusList.Count > 10
                End If
            Case MaintenanceActivityTypes.ComponentDirective     '10. Component Directive
                If Not mTmpComplyCompMonitorModStatusList Is Nothing Then
                    btnAddToCartTop.Visible = mTmpComplyCompMonitorModStatusList.Count > 10
                    btnNextTop.Visible = mTmpComplyCompMonitorModStatusList.Count > 10
                    btnCloseTop.Visible = mTmpComplyCompMonitorModStatusList.Count > 10
                End If
        End Select

    End Sub
    Private Sub AddComplaince(ID As String, Remark As String)
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.AssemblyService
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MachineID)
                    Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID, mMachine.HourType)
                    If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record. Assembly Service -> " + mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Assembly Service -> " + mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        If Len(Remark) > 200 Then
                            str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Service" + "-> " + mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).Desc + "<BR>"
                        Else
                            mMultiComplianceList.Add(mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID, MaintenanceActivityTypes.AssemblyService, True, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).Reference, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorInfo, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorType, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).Desc, AsonDate, txtWorkOrderNo.Text, Remark, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MachineInfo, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyType, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MachineID.ToString, , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ATA, , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID.ToString, , , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ModelSerialNo, AsonDate)
                        End If
                    End If


                End If
            Case MaintenanceActivityTypes.AssemblyInspection
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MachineID)
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyStatusID, mMachine.HourType)
                    If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record. Assembly Inspection -> " + mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorInfo + " One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Assembly Inspection -> " + mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        If Len(Remark) > 200 Then
                            str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Inspection" + "-> " + mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).Desc + "<BR>"
                        Else
                            mMultiComplianceList.Add(mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID, MaintenanceActivityTypes.AssemblyInspection, True, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).Reference, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorInfo, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorType, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).Desc, AsonDate, txtWorkOrderNo.Text, Remark, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MachineInfo, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyType, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MachineID.ToString, , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ATA, , , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID.ToString, , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ModelSerialNo, AsonDate)
                        End If
                    End If
                End If
            Case MaintenanceActivityTypes.AssemblyDirective
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MachineID)
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyStatusID, mMachine.HourType)
                    If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record. Assembly Directives -> " + mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorInfo + " One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Assembly Directives -> " + mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        If Len(Remark) > 200 Then
                            str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Directive" + "-> " + mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).Desc + "<BR>"
                        Else
                            mMultiComplianceList.Add(mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID, MaintenanceActivityTypes.AssemblyDirective, True, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).Reference, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorInfo, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorType, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).Desc, AsonDate, txtWorkOrderNo.Text, Remark, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).RemainingValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ModNumber, , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MachineInfo, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyType, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MachineID.ToString, , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ATA, , , , , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID.ToString, , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ModelSerialNo, AsonDate)
                        End If
                    End If
                End If
            Case MaintenanceActivityTypes.ComponentService
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompStatusID, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID.ToString)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MachineID)
                    Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompStatusID, mMachine.HourType)
                    If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record. Compoenent Service -> " + mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Compoenent Service -> " + mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        If Len(Remark) > 200 Then
                            str = str + "Comply Remark should not be greater than 200 characters" + " Component Service" + "-> " + mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).Description + "<BR>"
                        Else
                            mMultiComplianceList.Add(mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID, MaintenanceActivityTypes.ComponentService, True, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).Reference, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorInfo, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorType, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).Description, AsonDate, txtWorkOrderNo.Text, Remark, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MachineInfo, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyType, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyInfo, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompInfo, , , , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompStatusID.ToString, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MachineID.ToString, , , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).PartSerialNo, , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).ATA, , , , , , , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID.ToString, , , , AsonDate)
                        End If
                    End If
                End If
            Case MaintenanceActivityTypes.ComponentInspection
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompStatusID, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID.ToString)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MachineID)
                    Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompStatusID, mMachine.HourType)
                    If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record. Compoenent Inspection -> " + mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorInfo + " One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Compoenent Inspection -> " + mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        If Len(Remark) > 200 Then
                            str = str + "Comply Remark should not be greater than 200 characters" + " Component Inspection" + "-> " + mTmpComplyCompMonitorInspStatusList(New Guid(ID)).Description + "<BR>"
                        Else
                            mMultiComplianceList.Add(mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID, MaintenanceActivityTypes.ComponentInspection, True, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).Reference, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorInfo, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorType, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).Description, AsonDate, txtWorkOrderNo.Text, Remark, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MachineInfo, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyType, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyInfo, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompInfo, , , , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompStatusID.ToString, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MachineID.ToString, , , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).PartSerialNo, , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).ATA, , , , , , , , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID.ToString)
                        End If
                    End If
                End If
            Case MaintenanceActivityTypes.ComponentDirective
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompStatusID, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID.ToString)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(New Guid(ID)).MachineID)
                    Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompStatusID, mMachine.HourType)
                    If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Component Modification -> " + mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Compoenent Modification -> " + mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        If Len(Remark) > 200 Then
                            str = str + "Comply Remark should not be greater than 200 characters" + " Component Directive" + "-> " + mTmpComplyCompMonitorModStatusList(New Guid(ID)).Description + "<BR>"
                        Else
                            mMultiComplianceList.Add(mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID, MaintenanceActivityTypes.ComponentDirective, True, mTmpComplyCompMonitorModStatusList(New Guid(ID)).Reference, mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorInfo, mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorType, mTmpComplyCompMonitorModStatusList(New Guid(ID)).Description, AsonDate, txtWorkOrderNo.Text, Remark, mTmpComplyCompMonitorModStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyCompMonitorModStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).RemainingValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).ModNumber, , mTmpComplyCompMonitorModStatusList(New Guid(ID)).MachineInfo, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyType, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyInfo, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompInfo, , , , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompStatusID.ToString, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).MachineID.ToString, , , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).PartSerialNo, , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).ATA, , , , , , , , , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID.ToString, , AsonDate)
                        End If
                    End If
                End If
        End Select
        Session("mMultiComplianceList") = mMultiComplianceList

    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)

    End Sub
    'Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    If Flag = 1 Then Exit Sub
    '    Dim i As Integer
    '    Dim IsNotSelected As Boolean = True

    '    '1 Removal Comp
    '    Dim chkSelectInstalledList As New CheckBox
    '    Dim txtInstalledNote As New TextBox
    '    Dim chkIsExpired As New CheckBox
    '    Dim txtInstalledDoneByAgency As New TextBox
    '    Dim cmbReason As New DropDownList

    '    '2: Install Comp
    '    Dim chkSelectRemovedList As New CheckBox
    '    Dim txtRemovedDoneByAgency As New TextBox

    '    'case 5,6,7: Assembly 
    '    Dim chkSelectAssemblyList As CheckBox
    '    Dim txtAssemblyRemark As TextBox

    '    'case 8,9,10: Component 
    '    Dim chkSelectCompList As CheckBox
    '    Dim txtCompRemark As TextBox
    '    str = ""
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)


    '    Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
    '    Select Case MaintenanceActivityTypeID
    '        '1 Removal Comp
    '        Case MaintenanceActivityTypes.RemovalComp
    '            For i = 0 To Me.dgInstalledList.Items.Count - 1
    '                chkSelectInstalledList = CType(Me.dgInstalledList.Items(i).FindControl("chkSelectInstalledList"), CheckBox)
    '                cmbReason = CType(Me.dgInstalledList.Items(i).FindControl("cmbReason"), DropDownList)
    '                txtInstalledNote = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledNote"), TextBox)
    '                chkIsExpired = CType(Me.dgInstalledList.Items(i).FindControl("chkIsExpired"), CheckBox)
    '                txtInstalledDoneByAgency = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledDoneByAgency"), TextBox)
    '                If chkSelectInstalledList.Checked = True Then
    '                    IsNotSelected = False
    '                    If Not cmbReason.SelectedIndex > 0 Then
    '                        str = str + "Removal Reason Required for " + mInstalledCompList(i).CompInfo + "<BR>"
    '                    End If
    '                End If
    '            Next

    '            '2: Install Comp
    '        Case MaintenanceActivityTypes.InstallComp
    '            For i = 0 To Me.dgRemovedList.Items.Count - 1
    '                chkSelectRemovedList = CType(Me.dgRemovedList.Items(i).FindControl("chkSelectRemovedList"), CheckBox)
    '                txtRemovedDoneByAgency = CType(Me.dgRemovedList.Items(i).FindControl("txtRemovedDoneByAgency"), TextBox)
    '                If chkSelectRemovedList.Checked = True Then
    '                    IsNotSelected = False
    '                End If
    '            Next

    '            '5. Assembly Service
    '        Case MaintenanceActivityTypes.AssemblyService
    '            For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
    '                chkSelectAssemblyList = CType(Me.dgDueMonitoringList.Items(i).FindControl("chkSelectAssemblyList"), CheckBox)
    '                txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
    '                If chkSelectAssemblyList.Checked = True Then
    '                    IsNotSelected = False
    '                    If Len(txtAssemblyRemark.Text) > 200 Then
    '                        str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Service" + "-> " + mTmpComplyAssemblyMonitorServiceStatusList(i).Desc + "<BR>"
    '                    End If
    '                End If
    '            Next
    '            '6. Assembly Service
    '        Case MaintenanceActivityTypes.AssemblyInspection
    '            For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
    '                chkSelectAssemblyList = CType(Me.dgDueMonitoringList.Items(i).FindControl("chkSelectAssemblyList"), CheckBox)
    '                txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
    '                If chkSelectAssemblyList.Checked = True Then ''And (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyMonitorInspStatusID)) Then
    '                    IsNotSelected = False
    '                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(i).MachineID)
    '                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mMachine.HourType)
    '                    If Len(txtAssemblyRemark.Text) > 200 Then
    '                        str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Inspection" + "-> " + mTmpComplyAssemblyMonitorInspStatusList(i).Desc + "<BR>"
    '                    End If
    '                End If
    '            Next
    '        Case MaintenanceActivityTypes.AssemblyDirective
    '            For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
    '                chkSelectAssemblyList = CType(Me.dgDueMonitoringList.Items(i).FindControl("chkSelectAssemblyList"), CheckBox)
    '                txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
    '                If chkSelectAssemblyList.Checked = True Then ''And (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyMonitorModStatusID)) Then
    '                    IsNotSelected = False
    '                    If Len(txtAssemblyRemark.Text) > 200 Then
    '                        str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Directive" + "-> " + mTmpComplyAssemblyMonitorModStatusList(i).Desc + "<BR>"
    '                    End If
    '                End If
    '            Next
    '        Case MaintenanceActivityTypes.ComponentService
    '            For i = 0 To Me.dgDueMonitoringCompList.Items.Count - 1
    '                chkSelectCompList = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("chkSelectCompList"), CheckBox)
    '                txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtCompRemark"), TextBox)
    '                If (chkSelectCompList.Checked = True) Then ''And (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mTmpComplyCompMonitorServiceStatusList(i).CompMonitorServiceStatusID.ToString)) Then
    '                    IsNotSelected = False
    '                    If Len(txtCompRemark.Text) > 200 Then
    '                        str = str + "Comply Remark should not be greater than 200 characters" + " Component Service" + "-> " + mTmpComplyCompMonitorServiceStatusList(i).Description + "<BR>"
    '                    End If
    '                End If
    '            Next
    '        Case MaintenanceActivityTypes.ComponentInspection
    '            For i = 0 To Me.dgDueMonitoringCompList.Items.Count - 1
    '                chkSelectCompList = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("chkSelectCompList"), CheckBox)
    '                txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtCompRemark"), TextBox)
    '                If (chkSelectCompList.Checked = True) Then ''And (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mTmpComplyCompMonitorInspStatusList(i).CompMonitorInspStatusID.ToString)) Then
    '                    IsNotSelected = False
    '                    If Len(txtCompRemark.Text) > 200 Then
    '                        str = str + "Comply Remark should not be greater than 200 characters" + " Component Inspection" + "-> " + mTmpComplyCompMonitorInspStatusList(i).Description + "<BR>"
    '                    End If
    '                End If
    '            Next
    '        Case MaintenanceActivityTypes.ComponentDirective
    '            For i = 0 To Me.dgDueMonitoringCompList.Items.Count - 1
    '                chkSelectCompList = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("chkSelectCompList"), CheckBox)
    '                txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtCompRemark"), TextBox)
    '                If (chkSelectCompList.Checked = True) Then ''And (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mTmpComplyCompMonitorModStatusList(i).CompMonitorModStatusID.ToString)) Then
    '                    IsNotSelected = False
    '                    If Len(txtCompRemark.Text) > 200 Then
    '                        str = str + "Comply Remark should not be greater than 200 characters" + " Component Directive" + "-> " + mTmpComplyCompMonitorModStatusList(i).Description + "<BR>"
    '                    End If
    '                End If
    '            Next
    '    End Select

    '    If IsNotSelected = True Then
    '        str = str + "Please select atleast one item to add into the Cart"
    '    End If

    '    Session("str") = str
    '    str = Session("str")
    '    If str <> "" Then
    '        custValidator.ErrorMessage = str
    '        e.IsValid = False
    '    End If
    '    Flag = 1
    'End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim i As Integer
        Dim IsNotSelected As Boolean = True
        '    '1 Removal Comp
        Dim chkSelectInstalledList As New CheckBox
        Dim txtInstalledNote As New TextBox
        Dim chkIsExpired As New CheckBox
        Dim txtInstalledDoneByAgency As New TextBox
        Dim cmbReason As New DropDownList

        '    '2: Install Comp
        Dim chkSelectRemovedList As New CheckBox
        Dim txtRemovedDoneByAgency As New TextBox
        'case 5,6,7: Assembly 
        Dim txtAssemblyRemark As TextBox

        'case 8,9,10: Component 
        Dim txtCompRemark As TextBox

        str = ""
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)


        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)

        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.RemovalComp
                For i = 0 To Me.dgInstalledList.Items.Count - 1
                    chkSelectInstalledList = CType(Me.dgInstalledList.Items(i).FindControl("chkSelectInstalledList"), CheckBox)
                    cmbReason = CType(Me.dgInstalledList.Items(i).FindControl("cmbReason"), DropDownList)
                    txtInstalledNote = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledNote"), TextBox)
                    chkIsExpired = CType(Me.dgInstalledList.Items(i).FindControl("chkIsExpired"), CheckBox)
                    txtInstalledDoneByAgency = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledDoneByAgency"), TextBox)
                    If chkSelectInstalledList.Checked = True Then
                        IsNotSelected = False
                        If Not cmbReason.SelectedIndex > 0 Then
                            str = str + "Removal Reason Required for " + mInstalledCompList(i).CompInfo + "<BR>"
                        End If
                    End If
                Next

                '2: Install Comp
            Case MaintenanceActivityTypes.InstallComp
                For i = 0 To Me.dgRemovedList.Items.Count - 1
                    chkSelectRemovedList = CType(Me.dgRemovedList.Items(i).FindControl("chkSelectRemovedList"), CheckBox)
                    txtRemovedDoneByAgency = CType(Me.dgRemovedList.Items(i).FindControl("txtRemovedDoneByAgency"), TextBox)
                    If chkSelectRemovedList.Checked = True Then
                        IsNotSelected = False
                    End If
                Next
            Case 5, 6, 7

                Dim ActivityTypeName As String
                If MaintenanceActivityTypeID = 5 Then
                    ActivityTypeName = "Assembly Service"
                ElseIf MaintenanceActivityTypeID = 6 Then
                    ActivityTypeName = "Assembly Inspection"
                ElseIf MaintenanceActivityTypeID = 7 Then
                    ActivityTypeName = "Assembly Directive"
                End If

                Dim checkString = Request.Form("chkSelectAssemblyList")
               
                If checkString Is Nothing Then
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                    msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    msg.Show()
                    Exit Sub
                Else
                    Dim values = checkString.Split(","c)
                    For Each value As String In values
                        checkedIds.Add(value)
                    Next
                    For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
                        If checkedIds.Contains(dgDueMonitoringList.Items(i).Cells(1).Text) Then
                            Dim ID As String = dgDueMonitoringList.Items(i).Cells(1).Text
                            txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
                            If Len(txtAssemblyRemark.Text) > 200 Then
                                str = str + "Comply Remark should not be greater than 200 characters " + ActivityTypeName + "-> " + dgDueMonitoringList.Items(i).Cells(9).Text + "<BR>"
                            End If
                        End If
                    Next
                    values = ""
                End If

                checkString = Nothing
            Case 8, 9, 10

                Dim ActivityTypeName As String
                If MaintenanceActivityTypeID = 5 Then
                    ActivityTypeName = "Component Service"
                ElseIf MaintenanceActivityTypeID = 6 Then
                    ActivityTypeName = "Component Inspection"
                ElseIf MaintenanceActivityTypeID = 7 Then
                    ActivityTypeName = "Component Directive"
                End If

                Dim checkString = Request.Form("chkSelectCompList")
               
                If checkString Is Nothing Then
                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                    msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    msg.Show()
                    Exit Sub
                Else
                    Dim values = checkString.Split(","c)
                    For Each value As String In values
                        checkedIds.Add(value)
                    Next
                    For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
                        If checkedIds.Contains(dgDueMonitoringCompList.Items(i).Cells(1).Text) Then
                            Dim ID As String = dgDueMonitoringCompList.Items(i).Cells(1).Text
                            txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
                            If Len(txtCompRemark.Text) > 200 Then
                                str = str + "Comply Remark should not be greater than 200 characters " + ActivityTypeName + "-> " + mTmpComplyCompMonitorModStatusList(i).Description + "<BR>"
                            End If
                        End If
                    Next
                    values = ""
                End If

                checkString = Nothing
        End Select

        Session("str") = str
        str = Session("str")
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    'Public Sub DataFieldBind()
    '    Dim i As Integer

    '    Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
    '        Case MaintenanceActivityTypes.RemovalComp '1. Removal Comp
    '            dgInstalledList.Visible = True
    '            mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "<SELECT>")
    '            Dim cmbReason As DropDownList
    '            Session("mRemovalReasonList") = mRemovalReasonList
    '            If Not mInstalledCompList Is Nothing Then
    '                dgInstalledList.DataSource = mInstalledCompList
    '                dgInstalledList.DataBind()
    '                For i = 0 To Me.dgInstalledList.Items.Count - 1
    '                    cmbReason = CType(Me.dgInstalledList.Items(i).FindControl("cmbReason"), DropDownList)
    '                    cmbReason.DataSource = mRemovalReasonList
    '                    cmbReason.DataBind()
    '                Next
    '            End If
    '        Case MaintenanceActivityTypes.InstallComp  '2. Install Comp
    '            dgRemovedList.Visible = True
    '            If Not mRemovedCompList Is Nothing Then
    '                dgRemovedList.DataSource = mRemovedCompList
    '                dgRemovedList.DataBind()
    '            End If
    '        Case MaintenanceActivityTypes.AssemblyService '5. Assembly Service
    '            dgDueMonitoringList.Visible = True
    '            If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
    '                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
    '                dgDueMonitoringList.DataBind()
    '            End If
    '        Case MaintenanceActivityTypes.AssemblyInspection  '6. Assembly Inspection 
    '            dgDueMonitoringList.Visible = True
    '            If Not mTmpComplyAssemblyMonitorInspStatusList Is Nothing Then
    '                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
    '                dgDueMonitoringList.DataBind()
    '            End If
    '        Case MaintenanceActivityTypes.AssemblyDirective   '7. Assembly Directive 
    '            dgDueMonitoringList.Visible = True
    '            If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
    '                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
    '                dgDueMonitoringList.DataBind()
    '            End If
    '        Case MaintenanceActivityTypes.ComponentService    '8. Component Service 
    '            dgDueMonitoringCompList.Visible = True
    '            If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
    '                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorServiceStatusList
    '                dgDueMonitoringCompList.DataBind()
    '            End If
    '        Case MaintenanceActivityTypes.ComponentInspection    '9. Component Inspection 
    '            dgDueMonitoringCompList.Visible = True
    '            If Not mTmpComplyCompMonitorInspStatusList Is Nothing Then
    '                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorInspStatusList
    '                dgDueMonitoringCompList.DataBind()
    '            End If
    '        Case MaintenanceActivityTypes.ComponentDirective     '10. Component Directive
    '            dgDueMonitoringCompList.Visible = True
    '            If Not mTmpComplyCompMonitorModStatusList Is Nothing Then
    '                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorModStatusList
    '                dgDueMonitoringCompList.DataBind()
    '            End If
    '    End Select

    '    If Not Session("LogId") Is Nothing Then
    '        Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId).Item(0), MachineInfo).AssemblyStatusList
    '        AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
    '        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
    '        tmpAssemblyStatusList = Nothing
    '    End If

    '    dgDoneOnValue.DataSource = AssemblyStatusPeriodList
    '    dgDoneOnValue.DataBind()

    '    txtAircraft.Text = Aircraft
    '    txtAssembly.Text = AssemblyType

    '    txtAsOnDate.Enabled = False
    '    txtAsOnDate.Value = AsonDate


    'End Sub
    Public Sub DataFieldBind()

        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp '1. Removal Comp
                Dim i As Integer
                dgInstalledList.Visible = True
                mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "<SELECT>")
                Dim cmbReason As DropDownList
                Session("mRemovalReasonList") = mRemovalReasonList
                If Not mInstalledCompList Is Nothing Then
                    dgInstalledList.DataSource = mInstalledCompList
                    dgInstalledList.DataBind()
                    For i = 0 To Me.dgInstalledList.Items.Count - 1
                        cmbReason = CType(Me.dgInstalledList.Items(i).FindControl("cmbReason"), DropDownList)
                        cmbReason.DataSource = mRemovalReasonList
                        cmbReason.DataBind()
                    Next
                End If
            Case MaintenanceActivityTypes.InstallComp  '2. Install Comp
                dgRemovedList.Visible = True
                If Not mRemovedCompList Is Nothing Then
                    dgRemovedList.DataSource = mRemovedCompList
                    dgRemovedList.DataBind()
                End If
            Case MaintenanceActivityTypes.AssemblyService '5. Assembly Service
                dgDueMonitoringList.Visible = True
                If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
                    For i As Integer = 0 To mTmpComplyAssemblyMonitorServiceStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorServiceStatusList(i).ID.ToString)
                        End If
                    Next

                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                    dgDueMonitoringList.DataBind()
                End If
                Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
                Session("MaintenanceActivityTypeID") = 5
            Case MaintenanceActivityTypes.AssemblyInspection  '6. Assembly Inspection 
                dgDueMonitoringList.Visible = True
                
                If Not mTmpComplyAssemblyMonitorInspStatusList Is Nothing Then

                    For i As Integer = 0 To mTmpComplyAssemblyMonitorInspStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorInspStatusList(i).ID.ToString)
                        End If
                    Next

                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
                    dgDueMonitoringList.DataBind()
                End If
                Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
                Session("MaintenanceActivityTypeID") = 6
            Case MaintenanceActivityTypes.AssemblyDirective   '7. Assembly Directive 
                dgDueMonitoringList.Visible = True
                
                If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
                    For i As Integer = 0 To mTmpComplyAssemblyMonitorModStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorModStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
                    dgDueMonitoringList.DataBind()
                    Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
                    Session("MaintenanceActivityTypeID") = 7
                End If
            Case MaintenanceActivityTypes.ComponentService    '8. Component Service 
                dgDueMonitoringCompList.Visible = True
                Dim mTmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList

                If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then

                    For i As Integer = 0 To mTmpComplyCompMonitorServiceStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mTmpComplyCompMonitorServiceStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorServiceStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorServiceStatusList
                    dgDueMonitoringCompList.DataBind()
                End If
                Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
                Session("MaintenanceActivityTypeID") = 8
            Case MaintenanceActivityTypes.ComponentInspection    '9. Component Inspection 
                dgDueMonitoringCompList.Visible = True
                
                If Not mTmpComplyCompMonitorInspStatusList Is Nothing Then

                    For i As Integer = 0 To mTmpComplyCompMonitorInspStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mTmpComplyCompMonitorInspStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorInspStatusList(i).ID.ToString)
                        End If
                    Next

                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorInspStatusList
                    dgDueMonitoringCompList.DataBind()
                End If
                Session("mTmpComplyCompMonitorInspStatusList") = mTmpComplyCompMonitorInspStatusList
                Session("MaintenanceActivityTypeID") = 9
            Case MaintenanceActivityTypes.ComponentDirective     '10. Component Directive
                dgDueMonitoringCompList.Visible = True
                
                If Not mTmpComplyCompMonitorModStatusList Is Nothing Then

                    For i As Integer = 0 To mTmpComplyCompMonitorModStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mTmpComplyCompMonitorModStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorModStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorModStatusList
                    dgDueMonitoringCompList.DataBind()
                End If
                Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
                Session("MaintenanceActivityTypeID") = 10
        End Select

        If Not Session("LogId") Is Nothing Then
            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId).Item(0), MachineInfo).AssemblyStatusList
            AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            tmpAssemblyStatusList = Nothing
        End If

        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
        dgDoneOnValue.DataBind()

        txtAircraft.Text = Aircraft
        txtAssembly.Text = AssemblyType

        txtAsOnDate.Enabled = False
        txtAsOnDate.Value = AsonDate



    End Sub
    Private Sub SetCaption()
        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp           '1. Removal Comp
                lblResult.Text = "List of Installed components as per selected criteria : " & mInstalledCompList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.InstallComp           '2. Install Comp
                lblResult.Text = "List of Removed components as per selected criteria : " & mRemovedCompList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.AssemblyService       '5. Assembly Service 
                lblResult.Text = "List of Assembly Services as per selected criteria : " & mTmpComplyAssemblyMonitorServiceStatusList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.AssemblyInspection    '6. Assembly Inspection  
                lblResult.Text = "List of Assembly Inspections as per selected criteria : " & mTmpComplyAssemblyMonitorInspStatusList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.AssemblyDirective     '7. Assembly Directive  
                lblResult.Text = "List of Assembly Directives as per selected criteria : " & mTmpComplyAssemblyMonitorModStatusList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.ComponentService      '8. Component Service  
                lblResult.Text = "List of Component Services as per selected criteria : " & mTmpComplyCompMonitorServiceStatusList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.ComponentInspection   '9. Component Inspection
                lblResult.Text = "List of Component Inspections as per selected criteria : " & mTmpComplyCompMonitorInspStatusList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.ComponentDirective    '10. Component Directive  
                lblResult.Text = "List of Component Modifications as per selected criteria : " & mTmpComplyCompMonitorModStatusList.Count & " Record(s) found."
        End Select
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        '' ClearAll()
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            ''DueType = 1
            If AsonDate Is Nothing Then AsonDate = Request.QueryString("DoneOn")
            If MachineName Is Nothing Then MachineName = Request.QueryString("MachineId")
            If HourType Is Nothing Then HourType = Request.QueryString("HourType")
            If AssemblyName Is Nothing Then AssemblyName = Request.QueryString("AssemblyID")
            setFocus(txtWorkOrderNo)
            txtAsOnDate.Enabled = False
            txtAsOnDate.Value = AsonDate
            Session("mLogList") = Nothing
            DataFieldBind()
            Controltovisibility()
            ''SetLog()
        End If
        SetSession()
        SetCaption()
        MessageBoxResult()
    End Sub
    ''Private Sub btnAddToCart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddToCart.Click, btnAddToCartTop.Click
    ''    If IsValid Then
    ''        Dim i As Integer
    ''        Dim IsNotSelected As Boolean = True

    ''        '1 Removal Comp
    ''        Dim chkSelectInstalledList As New CheckBox
    ''        Dim txtInstalledNote As New TextBox
    ''        Dim chkIsExpired As New CheckBox
    ''        Dim txtInstalledDoneByAgency As New TextBox
    ''        Dim cmbReason As New DropDownList

    ''        '2: Install Comp
    ''        Dim chkSelectRemovedList As New CheckBox
    ''        Dim txtRemovedDoneByAgency As New TextBox

    ''        '5,6,7: Assembly 
    ''        Dim chkSelectAssemblyList As CheckBox
    ''        Dim txtAssemblyRemark As TextBox

    ''        '8,9,10: Component 
    ''        Dim chkSelectCompList As CheckBox
    ''        Dim txtCompRemark As TextBox


    ''        mRemovalReasonList = Session("mRemovalReasonList")
    ''        str = ""
    ''        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
    ''        Select Case MaintenanceActivityTypeID

    ''            '1 Removal Comp
    ''            Case MaintenanceActivityTypes.RemovalComp
    ''                For i = 0 To Me.dgInstalledList.Items.Count - 1
    ''                    chkSelectInstalledList = CType(Me.dgInstalledList.Items(i).FindControl("chkSelectInstalledList"), CheckBox)
    ''                    cmbReason = CType(Me.dgInstalledList.Items(i).FindControl("cmbReason"), DropDownList)
    ''                    txtInstalledNote = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledNote"), TextBox)
    ''                    chkIsExpired = CType(Me.dgInstalledList.Items(i).FindControl("chkIsExpired"), CheckBox)
    ''                    txtInstalledDoneByAgency = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledDoneByAgency"), TextBox)
    ''                    If chkSelectInstalledList.Checked = True Then
    ''                        If cmbReason.SelectedIndex > 0 Then
    ''                            IsNotSelected = False
    ''                            mMultiComplianceList.Add(Guid.NewGuid, MaintenanceActivityTypes.RemovalComp, True, , , , , , txtWorkOrderNo.Text, txtInstalledNote.Text, , , , , , , , , , , mInstalledCompList(i).MachineInfo, mInstalledCompList(i).AssemblyType, mInstalledCompList(i).AssemblyInfo, mInstalledCompList(i).CompInfo, mInstalledCompList(i).InstalledOn.ToString, mInstalledCompList(i).PeriodName, mInstalledCompList(i).Value, mInstalledCompList(i).ValueFormatted, mInstalledCompList(i).CompStatusID.ToString, mInstalledCompList(i).AssemblyStatusID.ToString, mInstalledCompList(i).AssemblyTypeID, AsonDate, mInstalledCompList(i).MachineID.ToString, mInstalledCompList(i).IsMaster, mInstalledCompList(i).ModelID.ToString, mInstalledCompList(i).PartID.ToString, mInstalledCompList(i).CompSerialNo, mInstalledCompList(i).IsRemoved, mInstalledCompList(i).Code, mInstalledCompList(i).ATAChapter, cmbReason.SelectedValue.ToString, cmbReason.SelectedItem.Text, txtInstalledDoneByAgency.Text, , , , , , , , , , , )
    ''                        Else
    ''                            str = str + "Removal Reason Required for " + mInstalledCompList(i).CompInfo + "<BR>"
    ''                        End If
    ''                    End If
    ''                Next

    ''                '2: Install Comp
    ''            Case MaintenanceActivityTypes.InstallComp
    ''                For i = 0 To Me.dgRemovedList.Items.Count - 1
    ''                    chkSelectRemovedList = CType(Me.dgRemovedList.Items(i).FindControl("chkSelectRemovedList"), CheckBox)
    ''                    txtRemovedDoneByAgency = CType(Me.dgRemovedList.Items(i).FindControl("txtRemovedDoneByAgency"), TextBox)
    ''                    If chkSelectRemovedList.Checked = True Then
    ''                        IsNotSelected = False
    ''                        mMultiComplianceList.Add(Guid.NewGuid, MaintenanceActivityTypes.InstallComp, True, , , , , , txtWorkOrderNo.Text, , , , , , , , , , , , mRemovedCompList(i).MachineInfo, mRemovedCompList(i).AssemblyType, mRemovedCompList(i).AssemblyInfo, mRemovedCompList(i).CompInfo, AsonDate, mRemovedCompList(i).PeriodName, mRemovedCompList(i).Value, mRemovedCompList(i).ValueFormatted, mRemovedCompList(i).CompStatusID.ToString, mRemovedCompList(i).AssemblyStatusID.ToString, , mRemovedCompList(i).RemovedOn, mRemovedCompList(i).MachineID.ToString, , mRemovedCompList(i).ModelID.ToString, mRemovedCompList(i).PartID.ToString, mRemovedCompList(i).CompSerialNo, , mRemovedCompList(i).Code, mRemovedCompList(i).ATAChapter, , , txtRemovedDoneByAgency.Text)
    ''                    End If
    ''                Next

    ''                '5. Assembly Service
    ''            Case MaintenanceActivityTypes.AssemblyService
    ''                For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
    ''                    chkSelectAssemblyList = CType(Me.dgDueMonitoringList.Items(i).FindControl("chkSelectAssemblyList"), CheckBox)
    ''                    txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
    ''                    If chkSelectAssemblyList.Checked = True Then
    ''                        IsNotSelected = False
    ''                        If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyMonitorServiceStatusID)) Then
    ''                            Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(i).MachineID)
    ''                            Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID, mMachine.HourType)
    ''                            If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record Assembly Service -> " + mTmpComplyAssemblyMonitorServiceStatusList(i).MonitorInfo + " One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiry compliance already done. Assembly Service -> " + mTmpComplyAssemblyMonitorServiceStatusList(i).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            Else
    ''                                If Len(txtAssemblyRemark.Text) > 200 Then
    ''                                    str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Service" + "-> " + mTmpComplyAssemblyMonitorServiceStatusList(i).Desc + "<BR>"
    ''                                Else
    ''                                    mMultiComplianceList.Add(mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyMonitorServiceStatusID, MaintenanceActivityTypes.AssemblyService, True, mTmpComplyAssemblyMonitorServiceStatusList(i).Reference, mTmpComplyAssemblyMonitorServiceStatusList(i).MonitorInfo, mTmpComplyAssemblyMonitorServiceStatusList(i).MonitorType, mTmpComplyAssemblyMonitorServiceStatusList(i).Desc, AsonDate, txtWorkOrderNo.Text, txtAssemblyRemark.Text, mTmpComplyAssemblyMonitorServiceStatusList(i).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorServiceStatusList(i).FrequencyValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(i).DoneOnValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(i).CurrentValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(i).ElapsedValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(i).ExtensionValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(i).DueOnValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(i).RemainingValueFormatted, , , mTmpComplyAssemblyMonitorServiceStatusList(i).MachineInfo, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyType, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorServiceStatusList(i).MachineID.ToString, , mTmpComplyAssemblyMonitorServiceStatusList(i).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorServiceStatusList(i).ATA, , , , , mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyMonitorServiceStatusID.ToString, , , , , , mTmpComplyAssemblyMonitorServiceStatusList(i).ModelSerialNo, AsonDate)
    ''                                End If
    ''                            End If
    ''                        End If
    ''                    End If
    ''                Next
    ''                '6. Assembly Service
    ''            Case MaintenanceActivityTypes.AssemblyInspection
    ''                For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
    ''                    chkSelectAssemblyList = CType(Me.dgDueMonitoringList.Items(i).FindControl("chkSelectAssemblyList"), CheckBox)
    ''                    txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
    ''                    If chkSelectAssemblyList.Checked = True Then
    ''                        IsNotSelected = False
    ''                        If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyMonitorInspStatusID)) Then
    ''                            Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(i).MachineID)
    ''                            Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mMachine.HourType)
    ''                            If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record. Assembly Inspection -> " + mTmpComplyAssemblyMonitorInspStatusList(i).MonitorInfo + " One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Assembly Inspection -> " + mTmpComplyAssemblyMonitorInspStatusList(i).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            Else
    ''                                If Len(txtAssemblyRemark.Text) > 200 Then
    ''                                    str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Inspection" + "-> " + mTmpComplyAssemblyMonitorInspStatusList(i).Desc + "<BR>"
    ''                                Else
    ''                                    mMultiComplianceList.Add(mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyMonitorInspStatusID, MaintenanceActivityTypes.AssemblyInspection, True, mTmpComplyAssemblyMonitorInspStatusList(i).Reference, mTmpComplyAssemblyMonitorInspStatusList(i).MonitorInfo, mTmpComplyAssemblyMonitorInspStatusList(i).MonitorType, mTmpComplyAssemblyMonitorInspStatusList(i).Desc, AsonDate, txtWorkOrderNo.Text, txtAssemblyRemark.Text, mTmpComplyAssemblyMonitorInspStatusList(i).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorInspStatusList(i).FrequencyValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(i).DoneOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(i).CurrentValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(i).ElapsedValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(i).ExtensionValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(i).DueOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(i).RemainingValueFormatted, , , mTmpComplyAssemblyMonitorInspStatusList(i).MachineInfo, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyType, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorInspStatusList(i).MachineID.ToString, , mTmpComplyAssemblyMonitorInspStatusList(i).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorInspStatusList(i).ATA, , , , , , mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyMonitorInspStatusID.ToString, , , , , mTmpComplyAssemblyMonitorInspStatusList(i).ModelSerialNo, AsonDate)
    ''                                End If
    ''                            End If
    ''                        End If
    ''                    End If
    ''                Next
    ''            Case MaintenanceActivityTypes.AssemblyDirective
    ''                For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
    ''                    chkSelectAssemblyList = CType(Me.dgDueMonitoringList.Items(i).FindControl("chkSelectAssemblyList"), CheckBox)
    ''                    txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
    ''                    If chkSelectAssemblyList.Checked = True Then
    ''                        IsNotSelected = False
    ''                        If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyMonitorModStatusID)) Then
    ''                            Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(i).MachineID)
    ''                            Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList(i).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mMachine.HourType)
    ''                            If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Assembly Modification -> " + mTmpComplyAssemblyMonitorModStatusList(i).ModNumber + " " + mTmpComplyAssemblyMonitorModStatusList(i).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Assembly Modification -> " + mTmpComplyAssemblyMonitorModStatusList(i).ModNumber + " " + mTmpComplyAssemblyMonitorModStatusList(i).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            Else
    ''                                If Len(txtAssemblyRemark.Text) > 200 Then
    ''                                    str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Directive" + "-> " + mTmpComplyAssemblyMonitorModStatusList(i).Desc + "<BR>"
    ''                                Else
    ''                                    mMultiComplianceList.Add(mTmpComplyAssemblyMonitorModStatusList(i).AssemblyMonitorModStatusID, MaintenanceActivityTypes.AssemblyDirective, True, mTmpComplyAssemblyMonitorModStatusList(i).Reference, mTmpComplyAssemblyMonitorModStatusList(i).MonitorInfo, mTmpComplyAssemblyMonitorModStatusList(i).MonitorType, mTmpComplyAssemblyMonitorModStatusList(i).Desc, AsonDate, txtWorkOrderNo.Text, txtAssemblyRemark.Text, mTmpComplyAssemblyMonitorModStatusList(i).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorModStatusList(i).FrequencyValueFormatted, mTmpComplyAssemblyMonitorModStatusList(i).DoneOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(i).CurrentValueFormatted, mTmpComplyAssemblyMonitorModStatusList(i).ElapsedValueFormatted, mTmpComplyAssemblyMonitorModStatusList(i).ExtensionValueFormatted, mTmpComplyAssemblyMonitorModStatusList(i).DueOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(i).RemainingValueFormatted, mTmpComplyAssemblyMonitorModStatusList(i).ModNumber, , mTmpComplyAssemblyMonitorModStatusList(i).MachineInfo, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyType, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorModStatusList(i).MachineID.ToString, , mTmpComplyAssemblyMonitorModStatusList(i).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorModStatusList(i).ATA, , , , , , , mTmpComplyAssemblyMonitorModStatusList(i).AssemblyMonitorModStatusID.ToString, , , , mTmpComplyAssemblyMonitorModStatusList(i).ModelSerialNo, AsonDate)
    ''                                End If
    ''                            End If
    ''                        End If
    ''                    End If
    ''                Next
    ''            Case MaintenanceActivityTypes.ComponentService
    ''                For i = 0 To Me.dgDueMonitoringCompList.Items.Count - 1
    ''                    chkSelectCompList = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("chkSelectCompList"), CheckBox)
    ''                    txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtCompRemark"), TextBox)
    ''                    If (chkSelectCompList.Checked = True) Then
    ''                        IsNotSelected = False
    ''                        If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mTmpComplyCompMonitorServiceStatusList(i).CompMonitorServiceStatusID.ToString)) Then
    ''                            Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(i).MachineID)
    ''                            Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(i).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(i).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mMachine.HourType)
    ''                            If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Component Service -> " + mTmpComplyCompMonitorServiceStatusList(i).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Component Service -> " + mTmpComplyCompMonitorServiceStatusList(i).MonitorInfo + "  Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            Else
    ''                                If Len(txtCompRemark.Text) > 200 Then
    ''                                    str = str + "Comply Remark should not be greater than 200 characters" + " Component Service" + "-> " + mTmpComplyCompMonitorServiceStatusList(i).Description + "<BR>"
    ''                                Else
    ''                                    mMultiComplianceList.Add(mTmpComplyCompMonitorServiceStatusList(i).CompMonitorServiceStatusID, MaintenanceActivityTypes.ComponentService, True, mTmpComplyCompMonitorServiceStatusList(i).Reference, mTmpComplyCompMonitorServiceStatusList(i).MonitorInfo, mTmpComplyCompMonitorServiceStatusList(i).MonitorType, mTmpComplyCompMonitorServiceStatusList(i).Description, AsonDate, txtWorkOrderNo.Text, txtCompRemark.Text, mTmpComplyCompMonitorServiceStatusList(i).PeriodUnitNameForWeb, mTmpComplyCompMonitorServiceStatusList(i).FrequencyValueFormatted, mTmpComplyCompMonitorServiceStatusList(i).DoneOnValueFormatted, mTmpComplyCompMonitorServiceStatusList(i).CurrentValueFormatted, mTmpComplyCompMonitorServiceStatusList(i).ElapsedValueFormatted, mTmpComplyCompMonitorServiceStatusList(i).ExtensionValueFormatted, mTmpComplyCompMonitorServiceStatusList(i).DueOnValueFormatted, mTmpComplyCompMonitorServiceStatusList(i).RemainingValueFormatted, , , mTmpComplyCompMonitorServiceStatusList(i).MachineInfo, mTmpComplyCompMonitorServiceStatusList(i).AssemblyType, mTmpComplyCompMonitorServiceStatusList(i).AssemblyInfo, mTmpComplyCompMonitorServiceStatusList(i).CompInfo, , , , , mTmpComplyCompMonitorServiceStatusList(i).CompStatusID.ToString, mTmpComplyCompMonitorServiceStatusList(i).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorServiceStatusList(i).MachineID.ToString, , , , mTmpComplyCompMonitorServiceStatusList(i).PartSerialNo, , , mTmpComplyCompMonitorServiceStatusList(i).ATA, , , , , , , , mTmpComplyCompMonitorServiceStatusList(i).CompMonitorServiceStatusID.ToString, , , , AsonDate)
    ''                                End If
    ''                            End If
    ''                        End If
    ''                    End If
    ''                Next
    ''            Case MaintenanceActivityTypes.ComponentInspection
    ''                For i = 0 To Me.dgDueMonitoringCompList.Items.Count - 1
    ''                    chkSelectCompList = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("chkSelectCompList"), CheckBox)
    ''                    txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtCompRemark"), TextBox)
    ''                    If (chkSelectCompList.Checked = True) Then
    ''                        IsNotSelected = False
    ''                        If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mTmpComplyCompMonitorInspStatusList(i).CompMonitorInspStatusID.ToString)) Then
    ''                            Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorInspStatusList(i).MachineID)
    ''                            Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mTmpComplyCompMonitorInspStatusList(i).CompMonitorInspStatusID, mTmpComplyCompMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mMachine.HourType)
    ''                            If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Component Inspection -> " + mTmpComplyCompMonitorInspStatusList(i).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Component Inspection -> " + mTmpComplyCompMonitorInspStatusList(i).MonitorInfo + "  Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            Else
    ''                                If Len(txtCompRemark.Text) > 200 Then
    ''                                    str = str + "Comply Remark should not be greater than 200 characters" + " Component Inspection" + "-> " + mTmpComplyCompMonitorInspStatusList(i).Description + "<BR>"
    ''                                Else
    ''                                    mMultiComplianceList.Add(mTmpComplyCompMonitorInspStatusList(i).CompMonitorInspStatusID, MaintenanceActivityTypes.ComponentInspection, True, mTmpComplyCompMonitorInspStatusList(i).Reference, mTmpComplyCompMonitorInspStatusList(i).MonitorInfo, mTmpComplyCompMonitorInspStatusList(i).MonitorType, mTmpComplyCompMonitorInspStatusList(i).Description, AsonDate, txtWorkOrderNo.Text, txtCompRemark.Text, mTmpComplyCompMonitorInspStatusList(i).PeriodUnitNameForWeb, mTmpComplyCompMonitorInspStatusList(i).FrequencyValueFormatted, mTmpComplyCompMonitorInspStatusList(i).DoneOnValueFormatted, mTmpComplyCompMonitorInspStatusList(i).CurrentValueFormatted, mTmpComplyCompMonitorInspStatusList(i).ElapsedValueFormatted, mTmpComplyCompMonitorInspStatusList(i).ExtensionValueFormatted, mTmpComplyCompMonitorInspStatusList(i).DueOnValueFormatted, mTmpComplyCompMonitorInspStatusList(i).RemainingValueFormatted, , , mTmpComplyCompMonitorInspStatusList(i).MachineInfo, mTmpComplyCompMonitorInspStatusList(i).AssemblyType, mTmpComplyCompMonitorInspStatusList(i).AssemblyInfo, mTmpComplyCompMonitorInspStatusList(i).CompInfo, , , , , mTmpComplyCompMonitorInspStatusList(i).CompStatusID.ToString, mTmpComplyCompMonitorInspStatusList(i).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorInspStatusList(i).MachineID.ToString, , , , mTmpComplyCompMonitorInspStatusList(i).PartSerialNo, , , mTmpComplyCompMonitorInspStatusList(i).ATA, , , , , , , , , mTmpComplyCompMonitorInspStatusList(i).CompMonitorInspStatusID.ToString, , , AsonDate)
    ''                                End If
    ''                            End If
    ''                        End If
    ''                    End If
    ''                Next
    ''            Case MaintenanceActivityTypes.ComponentDirective
    ''                For i = 0 To Me.dgDueMonitoringCompList.Items.Count - 1
    ''                    chkSelectCompList = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("chkSelectCompList"), CheckBox)
    ''                    txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtCompRemark"), TextBox)
    ''                    If (chkSelectCompList.Checked = True) Then
    ''                        IsNotSelected = False
    ''                        If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mTmpComplyCompMonitorModStatusList(i).CompMonitorModStatusID.ToString)) Then
    ''                            Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(i).MachineID)
    ''                            Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mTmpComplyCompMonitorModStatusList(i).CompMonitorModStatusID, mTmpComplyCompMonitorModStatusList(i).AssemblyStatusID, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mMachine.HourType)
    ''                            If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Component Modification -> " + mTmpComplyCompMonitorModStatusList(i).MonitorInfo + " Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted Then
    ''                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Component Modification -> " + mTmpComplyCompMonitorModStatusList(i).ModNumber + " " + mTmpComplyCompMonitorModStatusList(i).MonitorInfo + "  Can not be complied again.", MsgBoxStyle.OKOnly)
    ''                                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''                                msg.Show()
    ''                                Exit Sub
    ''                            Else
    ''                                If Len(txtCompRemark.Text) > 200 Then
    ''                                    str = str + "Comply Remark should not be greater than 200 characters" + " Component Directive" + "-> " + mTmpComplyCompMonitorModStatusList(i).Description + "<BR>"
    ''                                Else
    ''                                    mMultiComplianceList.Add(mTmpComplyCompMonitorModStatusList(i).CompMonitorModStatusID, MaintenanceActivityTypes.ComponentDirective, True, mTmpComplyCompMonitorModStatusList(i).Reference, mTmpComplyCompMonitorModStatusList(i).MonitorInfo, mTmpComplyCompMonitorModStatusList(i).MonitorType, mTmpComplyCompMonitorModStatusList(i).Description, AsonDate, txtWorkOrderNo.Text, txtCompRemark.Text, mTmpComplyCompMonitorModStatusList(i).PeriodUnitNameForWeb, mTmpComplyCompMonitorModStatusList(i).FrequencyValueFormatted, mTmpComplyCompMonitorModStatusList(i).DoneOnValueFormatted, mTmpComplyCompMonitorModStatusList(i).CurrentValueFormatted, mTmpComplyCompMonitorModStatusList(i).ElapsedValueFormatted, mTmpComplyCompMonitorModStatusList(i).ExtensionValueFormatted, mTmpComplyCompMonitorModStatusList(i).DueOnValueFormatted, mTmpComplyCompMonitorModStatusList(i).RemainingValueFormatted, mTmpComplyCompMonitorModStatusList(i).ModNumber, , mTmpComplyCompMonitorModStatusList(i).MachineInfo, mTmpComplyCompMonitorModStatusList(i).AssemblyType, mTmpComplyCompMonitorModStatusList(i).AssemblyInfo, mTmpComplyCompMonitorModStatusList(i).CompInfo, , , , , mTmpComplyCompMonitorModStatusList(i).CompStatusID.ToString, mTmpComplyCompMonitorModStatusList(i).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorModStatusList(i).MachineID.ToString, , , , mTmpComplyCompMonitorModStatusList(i).PartSerialNo, , , mTmpComplyCompMonitorModStatusList(i).ATA, , , , , , , , , , mTmpComplyCompMonitorModStatusList(i).CompMonitorModStatusID.ToString, , AsonDate)
    ''                                End If
    ''                            End If
    ''                        End If
    ''                    End If
    ''                Next
    ''        End Select
    ''        Session("str") = str
    ''        Session("mMultiComplianceList") = mMultiComplianceList

    ''        If IsNotSelected = True Then
    ''            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OKOnly)
    ''            msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    ''            msg.Show()
    ''            Exit Sub
    ''        Else
    ''            Response.Redirect("wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMultiComplanceListPartII.aspx" & "&DoneOn=" & AsonDate & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString)
    ''        End If
    ''    End If
    ''End Sub
  
    Private Sub btnAddToCart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddToCart.Click, btnAddToCartTop.Click
        If IsValid Then
            Dim i As Integer
            Dim IsNotSelected As Boolean = True
            Dim txtAssemblyRemark As TextBox
            Dim txtCompRemark As TextBox
            '1:          Removal(Comp)
            Dim chkSelectInstalledList As New CheckBox
            Dim txtInstalledNote As New TextBox
            Dim chkIsExpired As New CheckBox
            Dim txtInstalledDoneByAgency As New TextBox
            Dim cmbReason As New DropDownList

            '2: Install Comp
            Dim chkSelectRemovedList As New CheckBox
            Dim txtRemovedDoneByAgency As New TextBox
            str = ""

            Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)

            Select Case MaintenanceActivityTypeID
                '1: Removal Comp
                Case MaintenanceActivityTypes.RemovalComp
                    For i = 0 To Me.dgInstalledList.Items.Count - 1
                        chkSelectInstalledList = CType(Me.dgInstalledList.Items(i).FindControl("chkSelectInstalledList"), CheckBox)
                        cmbReason = CType(Me.dgInstalledList.Items(i).FindControl("cmbReason"), DropDownList)
                        txtInstalledNote = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledNote"), TextBox)
                        chkIsExpired = CType(Me.dgInstalledList.Items(i).FindControl("chkIsExpired"), CheckBox)
                        txtInstalledDoneByAgency = CType(Me.dgInstalledList.Items(i).FindControl("txtInstalledDoneByAgency"), TextBox)
                        If chkSelectInstalledList.Checked = True Then
                            If cmbReason.SelectedIndex > 0 Then
                                IsNotSelected = False
                                mMultiComplianceList.Add(Guid.NewGuid, MaintenanceActivityTypes.RemovalComp, True, , , , , , txtWorkOrderNo.Text, txtInstalledNote.Text, , , , , , , , , , , mInstalledCompList(i).MachineInfo, mInstalledCompList(i).AssemblyType, mInstalledCompList(i).AssemblyInfo, mInstalledCompList(i).CompInfo, mInstalledCompList(i).InstalledOn.ToString, mInstalledCompList(i).PeriodName, mInstalledCompList(i).Value, mInstalledCompList(i).ValueFormatted, mInstalledCompList(i).CompStatusID.ToString, mInstalledCompList(i).AssemblyStatusID.ToString, mInstalledCompList(i).AssemblyTypeID, AsonDate, mInstalledCompList(i).MachineID.ToString, mInstalledCompList(i).IsMaster, mInstalledCompList(i).ModelID.ToString, mInstalledCompList(i).PartID.ToString, mInstalledCompList(i).CompSerialNo, mInstalledCompList(i).IsRemoved, mInstalledCompList(i).Code, mInstalledCompList(i).ATAChapter, cmbReason.SelectedValue.ToString, cmbReason.SelectedItem.Text, txtInstalledDoneByAgency.Text, , , , , , , , , , , )
                            Else
                                str = str + "Removal Reason Required for " + mInstalledCompList(i).CompInfo + "<BR>"
                            End If
                        End If
                    Next

                    '2: Install Comp
                Case MaintenanceActivityTypes.InstallComp
                    For i = 0 To Me.dgRemovedList.Items.Count - 1
                        chkSelectRemovedList = CType(Me.dgRemovedList.Items(i).FindControl("chkSelectRemovedList"), CheckBox)
                        txtRemovedDoneByAgency = CType(Me.dgRemovedList.Items(i).FindControl("txtRemovedDoneByAgency"), TextBox)
                        If chkSelectRemovedList.Checked = True Then
                            IsNotSelected = False
                            mMultiComplianceList.Add(Guid.NewGuid, MaintenanceActivityTypes.InstallComp, True, , , , , , txtWorkOrderNo.Text, , , , , , , , , , , , mRemovedCompList(i).MachineInfo, mRemovedCompList(i).AssemblyType, mRemovedCompList(i).AssemblyInfo, mRemovedCompList(i).CompInfo, AsonDate, mRemovedCompList(i).PeriodName, mRemovedCompList(i).Value, mRemovedCompList(i).ValueFormatted, mRemovedCompList(i).CompStatusID.ToString, mRemovedCompList(i).AssemblyStatusID.ToString, , mRemovedCompList(i).RemovedOn, mRemovedCompList(i).MachineID.ToString, , mRemovedCompList(i).ModelID.ToString, mRemovedCompList(i).PartID.ToString, mRemovedCompList(i).CompSerialNo, , mRemovedCompList(i).Code, mRemovedCompList(i).ATAChapter, , , txtRemovedDoneByAgency.Text)
                        End If
                    Next
                Case 5, 6, 7
                    Dim checkString = Request.Form("chkSelectAssemblyList")
                  
                    If checkString Is Nothing Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        Dim values = checkString.Split(","c)
                        For Each value As String In values
                            checkedIds.Add(value)
                        Next
                        For i = 0 To Me.dgDueMonitoringList.Items.Count - 1
                            If checkedIds.Contains(dgDueMonitoringList.Items(i).Cells(1).Text) Then
                                Dim ID As String = dgDueMonitoringList.Items(i).Cells(1).Text
                                txtAssemblyRemark = CType(Me.dgDueMonitoringList.Items(i).FindControl("txtAssemblyRemark"), TextBox)
                                AddComplaince(ID, txtAssemblyRemark.Text)
                            End If
                        Next
                        values = ""
                    End If
                    checkString = Nothing

                Case 8, 9, 10
                    Dim checkString = Request.Form("chkSelectCompList")
                   
                    If checkString Is Nothing Then
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                        msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        msg.Show()
                        Exit Sub
                    Else
                        Dim values = checkString.Split(","c)
                        For Each value As String In values
                            checkedIds.Add(value)
                        Next
                        For i = 0 To Me.dgDueMonitoringCompList.Items.Count - 1
                            If checkedIds.Contains(dgDueMonitoringCompList.Items(i).Cells(1).Text) Then
                                Dim ID As String = dgDueMonitoringCompList.Items(i).Cells(1).Text
                                txtCompRemark = CType(Me.dgDueMonitoringCompList.Items(i).FindControl("txtCompRemark"), TextBox)
                                AddComplaince(ID, txtCompRemark.Text)
                            End If
                        Next
                        values = ""
                    End If
                    checkString = Nothing
            End Select

        
            If Not mMultiComplianceList Is Nothing And mMultiComplianceList.Count > 0 Then
                Response.Redirect("wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMultiComplanceListPartII.aspx" & "&DoneOn=" & AsonDate & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString)
            Else
                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                msg.Show()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click, btnNextTop.Click
        If IsValid Then
            If Not Session("mMultiComplianceList") Is Nothing Then mMultiComplianceList = Session("mMultiComplianceList")

            If Not mMultiComplianceList Is Nothing And mMultiComplianceList.Count > 0 Then
                Response.Redirect("wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMultiComplanceListPartII.aspx" & "&DoneOn=" & AsonDate & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString)
            Else
                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please add some Items into the Cart", MsgBoxStyle.OkOnly)
                msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                msg.Show()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub dgDueMonitoringList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgDueMonitoringList.SortCommand
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.AssemblyService
                mTmpComplyAssemblyMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                dgDueMonitoringList.DataBind()
            Case MaintenanceActivityTypes.AssemblyInspection
                mTmpComplyAssemblyMonitorInspStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
                dgDueMonitoringList.DataBind()
            Case MaintenanceActivityTypes.AssemblyDirective
                mTmpComplyAssemblyMonitorModStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
                dgDueMonitoringList.DataBind()
        End Select
    End Sub
    Private Sub dgDueMonitoringCompList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgDueMonitoringCompList.SortCommand
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.ComponentService
                mTmpComplyCompMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorServiceStatusList
                dgDueMonitoringCompList.DataBind()
            Case MaintenanceActivityTypes.ComponentInspection
                mTmpComplyCompMonitorInspStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyCompMonitorInspStatusList") = mTmpComplyCompMonitorInspStatusList
                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorInspStatusList
                dgDueMonitoringCompList.DataBind()
            Case MaintenanceActivityTypes.ComponentDirective
                mTmpComplyCompMonitorModStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorModStatusList
                dgDueMonitoringCompList.DataBind()
        End Select
    End Sub
    Private Sub dgInstalledList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgInstalledList.SortCommand
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.RemovalComp
                mInstalledCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mInstalledCompList") = mInstalledCompList
                dgInstalledList.DataSource = mInstalledCompList
                dgInstalledList.DataBind()
        End Select
    End Sub
    Private Sub dgRemovedList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgRemovedList.SortCommand
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.InstallComp
                mRemovedCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mRemovedCompList") = mRemovedCompList
                dgRemovedList.DataSource = mRemovedCompList
                dgRemovedList.DataBind()
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        mMachineList = Nothing
        mAssemblyStatusList = Nothing
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Protected Sub btnFindNow_Click(sender As Object, e As EventArgs) Handles btnFindNow.Click
        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp '1. Removal Comp

            Case MaintenanceActivityTypes.InstallComp  '2. Install Comp

            Case MaintenanceActivityTypes.AssemblyService '5. Assembly Service
                dgDueMonitoringList.Visible = True
                If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyAssemblyMonitorServiceStatusList
                        Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                        Select c).ToList

                    dgDueMonitoringList.DataSource = ComplyList
                    dgDueMonitoringList.DataBind()

                    For i As Integer = 0 To mTmpComplyAssemblyMonitorServiceStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorServiceStatusList(i).ID.ToString)
                        End If
                    Next
                    lblResult.Text = "List of Assembly Services as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.AssemblyInspection  '6. Assembly Inspection 
                dgDueMonitoringList.Visible = True
                If Not mTmpComplyAssemblyMonitorInspStatusList Is Nothing Then

                    Dim ComplyList = (From c In mTmpComplyAssemblyMonitorInspStatusList
                        Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                        Select c).ToList

                    dgDueMonitoringList.DataSource = ComplyList
                    dgDueMonitoringList.DataBind()

                    For i As Integer = 0 To mTmpComplyAssemblyMonitorInspStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorInspStatusList(i).ID.ToString)
                        End If
                    Next
                    lblResult.Text = "List of Assembly Inspections as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.AssemblyDirective   '7. Assembly Directive 
                dgDueMonitoringList.Visible = True
                If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyAssemblyMonitorModStatusList
                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                      Select c).ToList

                    dgDueMonitoringList.DataSource = ComplyList
                    dgDueMonitoringList.DataBind()
                    For i As Integer = 0 To mTmpComplyAssemblyMonitorModStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorModStatusList(i).ID.ToString)
                        End If
                    Next
                    lblResult.Text = "List of Assembly Directives as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentService    '8. Component Service 
                dgDueMonitoringCompList.Visible = True
                If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyCompMonitorServiceStatusList
                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                      Select c).ToList

                    dgDueMonitoringCompList.DataSource = ComplyList
                    dgDueMonitoringCompList.DataBind()

                    For i As Integer = 0 To mTmpComplyCompMonitorServiceStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mTmpComplyCompMonitorServiceStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorServiceStatusList(i).ID.ToString)
                        End If
                    Next
                    lblResult.Text = "List of Component Services as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentInspection    '9. Component Inspection 
                dgDueMonitoringCompList.Visible = True
                If Not mTmpComplyCompMonitorInspStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyCompMonitorInspStatusList
                       Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                       Select c).ToList

                    dgDueMonitoringCompList.DataSource = ComplyList
                    dgDueMonitoringCompList.DataBind()

                    For i As Integer = 0 To mTmpComplyCompMonitorInspStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mTmpComplyCompMonitorInspStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorInspStatusList(i).ID.ToString)
                        End If
                    Next

                    lblResult.Text = "List of Component Inspections as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentDirective     '10. Component Directive
                dgDueMonitoringCompList.Visible = True
                If Not mTmpComplyCompMonitorModStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyCompMonitorModStatusList
                        Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                        Select c).ToList

                    dgDueMonitoringCompList.DataSource = ComplyList
                    dgDueMonitoringCompList.DataBind()

                    For i As Integer = 0 To mTmpComplyCompMonitorModStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mTmpComplyCompMonitorModStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorModStatusList(i).ID.ToString)
                        End If
                    Next
                    lblResult.Text = "List of Component Modifications as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
        End Select
    End Sub
#End Region

#Region "Checked Selection"

    Public Function NumeroChequeInclus(ByVal numero As String) As String

        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
#End Region
End Class
