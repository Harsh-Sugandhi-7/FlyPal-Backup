Imports System.Collections.Generic
Imports System.Linq

Public Class wfMultiComplanceListPartII_Ajax
    Inherits System.Web.UI.Page

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

    Dim mtmpInstalledCompList As tmpInstalledCompList
    Dim mtmpRemovedCompList As tmpRemovedCompList

    Dim mTmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
    Dim mTmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
    Dim mTmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList

    Dim mTmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
    Dim mTmpComplyCompMonitorInspStatusList As tmpComplyCompMonitorInspStatusList
    Dim mTmpComplyCompMonitorModStatusList As tmpComplyCompMonitorModStatusList

    Private checkedIds As New List(Of String)()

    Public mLinkMaintenanceList As LinkMaintenanceList
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

        Assembly1 = Session("Assembly1")
        mtmpInstalledCompList = Session("mtmpInstalledCompList")
        mtmpRemovedCompList = Session("mtmpRemovedCompList")
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

        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        Session("mtmpRemovedCompList") = mtmpRemovedCompList
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
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
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfMultiComplanceListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfMultiComplanceListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        End If
    End Sub
    Private Sub Controltovisibility()
        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp
                If Not mtmpInstalledCompList Is Nothing Then
                    btnAddToCartTop.Visible = mtmpInstalledCompList.Count > 10
                    btnNextTop.Visible = mtmpInstalledCompList.Count > 10
                    btnCloseTop.Visible = mtmpInstalledCompList.Count > 10
                End If
            Case MaintenanceActivityTypes.InstallComp
                If Not mtmpRemovedCompList Is Nothing Then
                    btnAddToCartTop.Visible = mtmpRemovedCompList.Count > 10
                    btnNextTop.Visible = mtmpRemovedCompList.Count > 10
                    btnCloseTop.Visible = mtmpRemovedCompList.Count > 10
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
        If Not mMultiComplianceList Is Nothing Then
            btnNextTop.Enabled = mMultiComplianceList.Count > 0
            btnNext.Enabled = mMultiComplianceList.Count > 0
        End If

    End Sub

    Private Sub AddComplaince(ID As String)
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.AssemblyService
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MachineID)
                    Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID, mMachine.HourType)
                    If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.OneTimeMonitoring, MSGBox.Message_Text.OneTimeMonitoring, "Assembly Service -> " + mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.Expiry, MSGBox.Message_Text.Expiry, "Assembly Service -> " + mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")

                        Exit Sub
                    Else
                        'If Len(Remark) > 200 Then
                        '    str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Service" + "-> " + mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).Desc + "<BR>"
                        'Else
                        mMultiComplianceList.Add(mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID, MaintenanceActivityTypes.AssemblyService, True, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).Reference, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorInfo, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MonitorType, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).Code_Desc, AsonDate, txtWorkOrderNo.Text, "", mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MachineInfo, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyType, mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).MachineID.ToString, , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ATA, , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).AssemblyMonitorServiceStatusID.ToString, , , , , , mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ModelSerialNo, AsonDate, ModelMonitorTypeCode:=mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ModelMonitorCode, ATACode:=mTmpComplyAssemblyMonitorServiceStatusList(New Guid(ID)).ATA.ToString, Place:=txtPlace.Text.Trim)
                        'End If
                    End If


                End If
            Case MaintenanceActivityTypes.AssemblyInspection
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MachineID)
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyStatusID, mMachine.HourType)
                    If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.OneTimeMonitoring, MSGBox.Message_Text.OneTimeMonitoring, "Assembly Inspection -> " + mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.Expiry, MSGBox.Message_Text.Expiry, "Assembly Inspection -> " + mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")

                        Exit Sub
                    Else
                        'If Len(Remark) > 200 Then
                        '    str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Inspection" + "-> " + mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).Desc + "<BR>"
                        'Else
                        mMultiComplianceList.Add(mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID, MaintenanceActivityTypes.AssemblyInspection, True, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).Reference, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorInfo, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MonitorType, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).Code_Desc, AsonDate, txtWorkOrderNo.Text, , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MachineInfo, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyType, mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).MachineID.ToString, , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ATA, , , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).AssemblyMonitorInspStatusID.ToString, , , , , mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ModelSerialNo, AsonDate, ModelMonitorTypeCode:=mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ModelMonitorCode, ATACode:=mTmpComplyAssemblyMonitorInspStatusList(New Guid(ID)).ATA.ToString, Place:=txtPlace.Text.Trim)
                        ' End If
                    End If
                End If
            Case MaintenanceActivityTypes.AssemblyDirective
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MachineID)
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyStatusID, mMachine.HourType)
                    If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.OneTimeMonitoring, MSGBox.Message_Text.OneTimeMonitoring, "Assembly Directives -> " + mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.Expiry, MSGBox.Message_Text.Expiry, "Assembly Directives -> " + mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")

                        Exit Sub
                    Else
                        'If Len(Remark) > 200 Then
                        '    str = str + "Comply Remark should not be greater than 200 characters" + " Assembly Directive" + "-> " + mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).Desc + "<BR>"
                        'Else
                        mMultiComplianceList.Add(mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID, MaintenanceActivityTypes.AssemblyDirective, True, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).Reference, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorInfo, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MonitorType, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).Code_Desc, AsonDate, txtWorkOrderNo.Text, , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).RemainingValueFormatted, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ModNumber, , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MachineInfo, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyType, mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyInfo, , , , , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).MachineID.ToString, , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ModelID.ToString, , , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ATA, , , , , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).AssemblyMonitorModStatusID.ToString, , , , mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ModelSerialNo, AsonDate, ModelMonitorTypeCode:=mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ModelMonitorCode, ATACode:=mTmpComplyAssemblyMonitorModStatusList(New Guid(ID)).ATA.ToString, Place:=txtPlace.Text.Trim)
                        ' End If
                    End If
                End If
            Case MaintenanceActivityTypes.ComponentService
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompStatusID, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID.ToString)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MachineID)
                    Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompStatusID, mMachine.HourType)
                    If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.OneTimeMonitoring, MSGBox.Message_Text.OneTimeMonitoring, "Component Service -> " + mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.Expiry, MSGBox.Message_Text.Expiry, "Compoenent Service -> " + mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")

                        Exit Sub
                    Else
                        'If Len(Remark) > 200 Then
                        '    str = str + "Comply Remark should not be greater than 200 characters" + " Component Service" + "-> " + mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).Description + "<BR>"
                        'Else
                        mMultiComplianceList.Add(mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID, MaintenanceActivityTypes.ComponentService, True, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).Reference, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorInfo, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MonitorType, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).Code_Desc, AsonDate, txtWorkOrderNo.Text, , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MachineInfo, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyType, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyInfo, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompInfo, , , , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompStatusID.ToString, mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).MachineID.ToString, , , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).PartSerialNo, , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).ATA, , , , , , , , mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).CompMonitorServiceStatusID.ToString, , , , AsonDate, ModelMonitorTypeCode:=mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).PartMonitorCode, ATACode:=mTmpComplyCompMonitorServiceStatusList(New Guid(ID)).ATA.ToString, Place:=txtPlace.Text.Trim)
                        ' End If
                    End If
                End If
            Case MaintenanceActivityTypes.ComponentInspection
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompStatusID, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID.ToString)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MachineID)
                    Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompStatusID, mMachine.HourType)
                    If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.OneTimeMonitoring, MSGBox.Message_Text.OneTimeMonitoring, "Compoenent Inspection -> " + mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.Expiry, MSGBox.Message_Text.Expiry, "Compoenent Inspection -> " + mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")

                        Exit Sub
                    Else
                        'If Len(Remark) > 200 Then
                        '    str = str + "Comply Remark should not be greater than 200 characters" + " Component Inspection" + "-> " + mTmpComplyCompMonitorInspStatusList(New Guid(ID)).Description + "<BR>"
                        'Else
                        mMultiComplianceList.Add(mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID, MaintenanceActivityTypes.ComponentInspection, True, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).Reference, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorInfo, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MonitorType, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).Code_Desc, AsonDate, txtWorkOrderNo.Text, , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).RemainingValueFormatted, , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MachineInfo, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyType, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyInfo, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompInfo, , , , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompStatusID.ToString, mTmpComplyCompMonitorInspStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).MachineID.ToString, , , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).PartSerialNo, , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).ATA, , , , , , , , , mTmpComplyCompMonitorInspStatusList(New Guid(ID)).CompMonitorInspStatusID.ToString, , ModelMonitorTypeCode:=mTmpComplyCompMonitorInspStatusList(New Guid(ID)).PartMonitorCode, ATACode:=mTmpComplyCompMonitorInspStatusList(New Guid(ID)).ATA.ToString, Place:=txtPlace.Text.Trim)
                        ' End If
                    End If
                End If
            Case MaintenanceActivityTypes.ComponentDirective
                If (Not mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompStatusID, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID.ToString)) Then
                    Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(New Guid(ID)).MachineID)
                    Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyStatusID, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompStatusID, mMachine.HourType)
                    If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.OneTimeMonitoring, MSGBox.Message_Text.OneTimeMonitoring, "Component Modification -> " + mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.Show(MSGBox.Message_Title.Expiry, MSGBox.Message_Text.Expiry, "Compoenent Modification -> " + mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorInfo + " ", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        'If Len(Remark) > 200 Then
                        '    str = str + "Comply Remark should not be greater than 200 characters" + " Component Directive" + "-> " + mTmpComplyCompMonitorModStatusList(New Guid(ID)).Description + "<BR>"
                        'Else
                        mMultiComplianceList.Add(mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID, MaintenanceActivityTypes.ComponentDirective, True, mTmpComplyCompMonitorModStatusList(New Guid(ID)).Reference, mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorInfo, mTmpComplyCompMonitorModStatusList(New Guid(ID)).MonitorType, mTmpComplyCompMonitorModStatusList(New Guid(ID)).Code_Desc, AsonDate, txtWorkOrderNo.Text, , mTmpComplyCompMonitorModStatusList(New Guid(ID)).PeriodUnitNameForWeb, mTmpComplyCompMonitorModStatusList(New Guid(ID)).FrequencyValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).DoneOnValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CurrentValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).ElapsedValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).ExtensionValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).DueOnValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).RemainingValueFormatted, mTmpComplyCompMonitorModStatusList(New Guid(ID)).ModNumber, , mTmpComplyCompMonitorModStatusList(New Guid(ID)).MachineInfo, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyType, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyInfo, mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompInfo, , , , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompStatusID.ToString, mTmpComplyCompMonitorModStatusList(New Guid(ID)).AssemblyStatusID.ToString, , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).MachineID.ToString, , , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).PartSerialNo, , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).ATA, , , , , , , , , , mTmpComplyCompMonitorModStatusList(New Guid(ID)).CompMonitorModStatusID.ToString, , AsonDate, ModelMonitorTypeCode:=mTmpComplyCompMonitorModStatusList(New Guid(ID)).PartMonitorCode, ATACode:=mTmpComplyCompMonitorModStatusList(New Guid(ID)).ATA.ToString, Place:=txtPlace.Text.Trim)
                        'End If
                    End If
                End If
        End Select
        Session("mMultiComplianceList") = mMultiComplianceList

    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)

    End Sub
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
                For i = 0 To Me.dgInstalledList.Rows.Count - 1
                    chkSelectInstalledList = CType(Me.dgInstalledList.Rows(i).FindControl("chkSelectInstalledList"), CheckBox)
                    cmbReason = CType(Me.dgInstalledList.Rows(i).FindControl("cmbReason"), DropDownList)
                    txtInstalledNote = CType(Me.dgInstalledList.Rows(i).FindControl("txtInstalledNote"), TextBox)
                    chkIsExpired = CType(Me.dgInstalledList.Rows(i).FindControl("chkIsExpired"), CheckBox)
                    txtInstalledDoneByAgency = CType(Me.dgInstalledList.Rows(i).FindControl("txtInstalledDoneByAgency"), TextBox)
                    If chkSelectInstalledList.Checked = True Then
                        IsNotSelected = False
                        If Not cmbReason.SelectedIndex > 0 Then
                            str = str + "Removal Reason Required for " + mtmpInstalledCompList(i).CompInfo + "<BR>"
                        End If
                    End If
                Next

                '2: Install Comp
            Case MaintenanceActivityTypes.InstallComp
                For i = 0 To Me.dgRemovedList.Rows.Count - 1
                    chkSelectRemovedList = CType(Me.dgRemovedList.Rows(i).FindControl("chkSelectRemovedList"), CheckBox)
                    txtRemovedDoneByAgency = CType(Me.dgRemovedList.Rows(i).FindControl("txtRemovedDoneByAgency"), TextBox)
                    If chkSelectRemovedList.Checked = True Then
                        IsNotSelected = False
                    End If
                Next
            Case 5, 6, 7

                Dim ActivityTypeName As String
                If MaintenanceActivityTypeID = MaintenanceActivityTypes.AssemblyService Then
                    ActivityTypeName = "Assembly Service"
                ElseIf MaintenanceActivityTypeID = MaintenanceActivityTypes.AssemblyInspection Then
                    ActivityTypeName = "Assembly Inspection"
                ElseIf MaintenanceActivityTypeID = MaintenanceActivityTypes.AssemblyDirective Then
                    ActivityTypeName = "Assembly Directive"
                End If

                Dim checkString = Request.Form("chkSelectAssemblyList")

                If checkString Is Nothing Then
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    'msg.Show()
                    MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne, MSGBox.Message_Text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    Dim values = checkString.Split(","c)
                    For Each value As String In values
                        checkedIds.Add(value)
                    Next
                    'For i = 0 To Me.dgDueMonitoringList.Rows.Count - 1
                    '    If checkedIds.Contains(dgDueMonitoringList.Rows(i).Cells(2).Text) Then
                    '        Dim ID As String = dgDueMonitoringList.Rows(i).Cells(2).Text
                    '        txtAssemblyRemark = CType(Me.dgDueMonitoringList.Rows(i).FindControl("txtAssemblyRemark"), TextBox)
                    '        If Len(txtAssemblyRemark.Text) > 200 Then
                    '            str = str + "Comply Remark should not be greater than 200 characters " + ActivityTypeName + "-> " + dgDueMonitoringList.Rows(i).Cells(9).Text + "<BR>"
                    '        End If
                    '    End If
                    'Next
                    values = ""
                End If

                checkString = Nothing
            Case 8, 9, 10

                Dim ActivityTypeName As String
                If MaintenanceActivityTypeID = MaintenanceActivityTypes.ComponentService Then
                    ActivityTypeName = "Component Service"
                ElseIf MaintenanceActivityTypeID = MaintenanceActivityTypes.ComponentInspection Then
                    ActivityTypeName = "Component Inspection"
                ElseIf MaintenanceActivityTypeID = MaintenanceActivityTypes.ComponentDirective Then
                    ActivityTypeName = "Component Directive"
                End If

                Dim checkString = Request.Form("chkSelectCompList")

                If checkString Is Nothing Then
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    'msg.Show()
                    MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne, MSGBox.Message_Text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    Dim values = checkString.Split(","c)
                    For Each value As String In values
                        checkedIds.Add(value)
                    Next
                    'For i = 0 To Me.dgDueMonitoringList.Rows.Count - 1
                    '    If checkedIds.Contains(dgDueMonitoringCompList.Rows(i).Cells(2).Text) Then
                    '        Dim ID As String = dgDueMonitoringCompList.Rows(i).Cells(2).Text
                    '        txtCompRemark = CType(Me.dgDueMonitoringCompList.Rows(i).FindControl("txtCompRemark"), TextBox)
                    '        If Len(txtCompRemark.Text) > 200 Then
                    '            str = str + "Comply Remark should not be greater than 200 characters " + ActivityTypeName + "-> " + mTmpComplyCompMonitorModStatusList(i).Description + "<BR>"
                    '        End If
                    '    End If
                    'Next
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
    Public Sub DataFieldBind()


        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp '1. Removal Comp
                Dim i As Integer
                dgInstalledList.Visible = True
                mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "<SELECT>")
                Dim cmbReason As DropDownList
                Session("mRemovalReasonList") = mRemovalReasonList
                If Not mtmpInstalledCompList Is Nothing Then
                    dgInstalledList.DataSource = mtmpInstalledCompList
                    dgInstalledList.DataBind()
                    For i = 0 To Me.dgInstalledList.Rows.Count - 1
                        cmbReason = CType(Me.dgInstalledList.Rows(i).FindControl("cmbReason"), DropDownList)
                        cmbReason.DataSource = mRemovalReasonList
                        cmbReason.DataBind()
                    Next
                End If
            Case MaintenanceActivityTypes.InstallComp  '2. Install Comp
                dgRemovedList.Visible = True
                If Not mtmpRemovedCompList Is Nothing Then
                    dgRemovedList.DataSource = mtmpRemovedCompList
                    dgRemovedList.DataBind()
                End If
            Case MaintenanceActivityTypes.AssemblyService '5. Assembly Service
                dgDueMonitoringList.Visible = True
                mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, Session("ModelName"), Session("SerialNo"), , , , , , , , True, , True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
                    For i As Integer = 0 To mTmpComplyAssemblyMonitorServiceStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorServiceStatusList(i).ID.ToString)
                        End If
                    Next

                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                    dgDueMonitoringList.DataBind()
                End If
            Case MaintenanceActivityTypes.AssemblyInspection  '6. Assembly Inspection 
                dgDueMonitoringList.Visible = True
                mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, Session("ModelName"), Session("SerialNo"), , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyAssemblyMonitorInspStatusList Is Nothing Then

                    For i As Integer = 0 To mTmpComplyAssemblyMonitorInspStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorInspStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
                    dgDueMonitoringList.DataBind()
                End If
            Case MaintenanceActivityTypes.AssemblyDirective   '7. Assembly Directive 
                dgDueMonitoringList.Visible = True
                'mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, Session("ModelName"), Session("SerialNo"), , , , , , , , , True, True)

                mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, Session("ModelName"), Session("SerialNo"), , , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
                    For i As Integer = 0 To mTmpComplyAssemblyMonitorModStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(i).ID) Then
                            checkedIds.Add(mTmpComplyAssemblyMonitorModStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
                    dgDueMonitoringList.DataBind()
                End If
            Case MaintenanceActivityTypes.ComponentService    '8. Component Service 
                dgDueMonitoringCompList.Visible = True
                mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , True, , True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
                    For i As Integer = 0 To mTmpComplyCompMonitorServiceStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mTmpComplyCompMonitorServiceStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorServiceStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorServiceStatusList
                    dgDueMonitoringCompList.DataBind()
                End If
            Case MaintenanceActivityTypes.ComponentInspection    '9. Component Inspection 

                dgDueMonitoringCompList.Visible = True
                mTmpComplyCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyCompMonitorInspStatusList Is Nothing Then
                    For i As Integer = 0 To mTmpComplyCompMonitorInspStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mTmpComplyCompMonitorInspStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorInspStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorInspStatusList
                    dgDueMonitoringCompList.DataBind()
                End If
            Case MaintenanceActivityTypes.ComponentDirective     '10. Component Directive


                dgDueMonitoringCompList.Visible = True
                mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyCompMonitorModStatusList Is Nothing Then
                    For i As Integer = 0 To mTmpComplyCompMonitorModStatusList.Count - 1
                        If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mTmpComplyCompMonitorModStatusList(i).ID.ToString) Then
                            checkedIds.Add(mTmpComplyCompMonitorModStatusList(i).ID.ToString)
                        End If
                    Next
                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorModStatusList
                    dgDueMonitoringCompList.DataBind()
                End If
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
        txtAsOnDate.Text = AsonDate
        SetCaption()

    End Sub
    Private Sub SetCaption()
        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp           '1. Removal Comp
                lblResult.Text = "List of Installed components as per selected criteria : " & mtmpInstalledCompList.Count & " Record(s) found."
            Case MaintenanceActivityTypes.InstallComp           '2. Install Comp
                lblResult.Text = "List of Removed components as per selected criteria : " & mtmpRemovedCompList.Count & " Record(s) found."
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

        If Not mMultiComplianceList Is Nothing Then
            If mMultiComplianceList.Count > 0 Then
                lblCart.Text = "Your Cart contains " & mMultiComplianceList.Count & " item(s)"
            Else
                lblCart.Text = "Your Cart contains 0 item(s)"
            End If
        End If
        If Assembly1 <> "" Then lbltitle.Text = "Multi Compliance List [ " & Assembly1 + " ]"
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
            txtAsOnDate.Text = AsonDate
            Session("mLogList") = Nothing
            rdbAssemblyService.Checked = True
            Session("MaintenanceActivityTypeID") = 5
            DataFieldBind()
            Controltovisibility()
            ''SetLog()

            rdbAssemblyService.DataBind()
            rdbComponentService.DataBind()
            rdbAssemblyInspection.DataBind()
            rdbComponentInspection.DataBind()
            rdbAssemblyDirective.DataBind()
            rdbComponentDirective.DataBind()
        End If
        SetSession()
        ' SetCaption()
    End Sub
    Private Sub btnAddToCart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddToCart.Click, btnAddToCartTop.Click
        If IsValid Then
            Dim i As Integer
            Dim IsNotSelected As Boolean = True
            'Dim txtAssemblyRemark As TextBox
            'Dim txtCompRemark As TextBox
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
                    For i = 0 To Me.dgInstalledList.Rows.Count - 1
                        chkSelectInstalledList = CType(Me.dgInstalledList.Rows(i).FindControl("chkSelectInstalledList"), CheckBox)
                        cmbReason = CType(Me.dgInstalledList.Rows(i).FindControl("cmbReason"), DropDownList)
                        txtInstalledNote = CType(Me.dgInstalledList.Rows(i).FindControl("txtInstalledNote"), TextBox)
                        chkIsExpired = CType(Me.dgInstalledList.Rows(i).FindControl("chkIsExpired"), CheckBox)
                        txtInstalledDoneByAgency = CType(Me.dgInstalledList.Rows(i).FindControl("txtInstalledDoneByAgency"), TextBox)
                        If chkSelectInstalledList.Checked = True Then
                            If cmbReason.SelectedIndex > 0 Then
                                IsNotSelected = False
                                mMultiComplianceList.Add(Guid.NewGuid, MaintenanceActivityTypes.RemovalComp, True, , , , , , txtWorkOrderNo.Text, txtInstalledNote.Text, , , , , , , , , , , mtmpInstalledCompList(i).MachineInfo, mtmpInstalledCompList(i).AssemblyType, mtmpInstalledCompList(i).AssemblyInfo, mtmpInstalledCompList(i).CompInfo, mtmpInstalledCompList(i).InstalledOn.ToString, mtmpInstalledCompList(i).PeriodName, mtmpInstalledCompList(i).Value, mtmpInstalledCompList(i).ValueFormatted, mtmpInstalledCompList(i).CompStatusID.ToString, mtmpInstalledCompList(i).AssemblyStatusID.ToString, mtmpInstalledCompList(i).AssemblyTypeID, AsonDate, mtmpInstalledCompList(i).MachineID.ToString, mtmpInstalledCompList(i).IsMaster, mtmpInstalledCompList(i).ModelID.ToString, mtmpInstalledCompList(i).PartID.ToString, mtmpInstalledCompList(i).CompSerialNo, mtmpInstalledCompList(i).IsRemoved, mtmpInstalledCompList(i).Code, mtmpInstalledCompList(i).ATAChapter, cmbReason.SelectedValue.ToString, cmbReason.SelectedItem.Text, txtInstalledDoneByAgency.Text, , , , , , , , , , , Place:=txtPlace.Text.Trim)
                            Else
                                str = str + "Removal Reason Required for " + mtmpInstalledCompList(i).CompInfo + "<BR>"
                            End If
                        End If
                    Next

                    '2: Install Comp
                Case MaintenanceActivityTypes.InstallComp
                    For i = 0 To Me.dgRemovedList.Rows.Count - 1
                        chkSelectRemovedList = CType(Me.dgRemovedList.Rows(i).FindControl("chkSelectRemovedList"), CheckBox)
                        txtRemovedDoneByAgency = CType(Me.dgRemovedList.Rows(i).FindControl("txtRemovedDoneByAgency"), TextBox)
                        If chkSelectRemovedList.Checked = True Then
                            IsNotSelected = False
                            mMultiComplianceList.Add(Guid.NewGuid, MaintenanceActivityTypes.InstallComp, True, , , , , , txtWorkOrderNo.Text, , , , , , , , , , , , mtmpRemovedCompList(i).MachineInfo, mtmpRemovedCompList(i).AssemblyType, mtmpRemovedCompList(i).AssemblyInfo, mtmpRemovedCompList(i).CompInfo, AsonDate, mtmpRemovedCompList(i).PeriodName, mtmpRemovedCompList(i).Value, mtmpRemovedCompList(i).ValueFormatted, mtmpRemovedCompList(i).CompStatusID.ToString, mtmpRemovedCompList(i).AssemblyStatusID.ToString, , mtmpRemovedCompList(i).RemovedOn, mtmpRemovedCompList(i).MachineID.ToString, , mtmpRemovedCompList(i).ModelID.ToString, mtmpRemovedCompList(i).PartID.ToString, mtmpRemovedCompList(i).CompSerialNo, , mtmpRemovedCompList(i).Code, mtmpRemovedCompList(i).ATAChapter, , , txtRemovedDoneByAgency.Text, Place:=txtPlace.Text.Trim)
                        End If
                    Next
                Case 5, 6, 7
                    Dim checkString = Request.Form("chkSelectAssemblyList")

                    If checkString Is Nothing Then
                        'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                        'msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        'msg.Show()
                        MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne, MSGBox.Message_Text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        Dim values = checkString.Split(","c)
                        For Each value As String In values
                            checkedIds.Add(value)
                        Next
                        For i = 0 To Me.dgDueMonitoringList.Rows.Count - 1
                            If checkedIds.Contains(dgDueMonitoringList.Rows(i).Cells(2).Text) Then
                                Dim ID As String = dgDueMonitoringList.Rows(i).Cells(2).Text
                                'txtAssemblyRemark = CType(Me.dgDueMonitoringList.Rows(i).FindControl("txtAssemblyRemark"), TextBox)
                                AddComplaince(ID)
                            End If
                        Next
                        values = ""
                    End If
                    checkString = Nothing

                Case 8, 9, 10
                    Dim checkString = Request.Form("chkSelectCompList")

                    If checkString Is Nothing Then
                        'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                        'msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                        'msg.Show()
                        MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne, MSGBox.Message_Text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        Dim values = checkString.Split(","c)
                        For Each value As String In values
                            checkedIds.Add(value)
                        Next
                        For i = 0 To Me.dgDueMonitoringCompList.Rows.Count - 1
                            If checkedIds.Contains(dgDueMonitoringCompList.Rows(i).Cells(2).Text) Then
                                Dim ID As String = dgDueMonitoringCompList.Rows(i).Cells(2).Text
                                'txtCompRemark = CType(Me.dgDueMonitoringCompList.Rows(i).FindControl("txtCompRemark"), TextBox)
                                AddComplaince(ID)
                            End If
                        Next
                        values = ""
                    End If
                    checkString = Nothing
            End Select


            If Not mMultiComplianceList Is Nothing And mMultiComplianceList.Count > 0 Then
                Response.Redirect("wfMultiComplianceCartListPartII_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMultiComplanceListPartII_Ajax.aspx" & "&DoneOn=" & AsonDate & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString)
            Else
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly)
                'msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                'msg.Show()
                MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne, MSGBox.Message_Text.SelectAtleastOne, "Please select atleast one item to add into the Cart", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click, btnNextTop.Click
        If IsValid Then
            If Not Session("mMultiComplianceList") Is Nothing Then mMultiComplianceList = Session("mMultiComplianceList")

            If Not mMultiComplianceList Is Nothing And mMultiComplianceList.Count > 0 Then
                Response.Redirect("wfMultiComplianceCartListPartII_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMultiComplanceListPartII_Ajax.aspx" & "&DoneOn=" & AsonDate & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString)
            Else
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please add some Items into the Cart", MsgBoxStyle.OkOnly)
                'msg.ReplacePage = "wfMultiComplanceListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                'msg.Show()
                MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne, MSGBox.Message_Text.SelectAtleastOne, "There are no Items in the cart. Please add some Items into the Cart", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub dgDueMonitoringList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDueMonitoringList.RowDataBound
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
            Dim grdLinkActivity As GridView = DirectCast(e.Row.FindControl("grdLinkActivity"), GridView)
            ' Dim Image As String = Request.Form("imageID" + ID.ToString) 'CType(e.Row.FindControl("imageID" + ID.ToString), ImageButton)

            Select Case MaintenanceActivityTypeID
                Case MaintenanceActivityTypes.AssemblyService
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorServiceStatusList(ID).ModelMonitorServiceID.ToString)
                Case MaintenanceActivityTypes.AssemblyInspection
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorInspStatusList(ID).ModelMonitorInspID.ToString)
                Case MaintenanceActivityTypes.AssemblyDirective
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyAssemblyMonitorModStatusList(ID).ModelMonitorModID.ToString)
            End Select

            If mLinkMaintenanceList.Count > 0 Then
                e.Row.Cells(1).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
            Else
                ' Image.Visible = False
            End If

            grdLinkActivity.DataSource = mLinkMaintenanceList
            grdLinkActivity.DataBind()
        End If



    End Sub
    Private Sub dgDueMonitoringList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueMonitoringList.Sorting
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.AssemblyService
                mTmpComplyAssemblyMonitorServiceStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                dgDueMonitoringList.DataBind()
            Case MaintenanceActivityTypes.AssemblyInspection
                mTmpComplyAssemblyMonitorInspStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
                dgDueMonitoringList.DataBind()
            Case MaintenanceActivityTypes.AssemblyDirective
                mTmpComplyAssemblyMonitorModStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
                dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
                dgDueMonitoringList.DataBind()
        End Select
    End Sub

    Private Sub dgDueMonitoringCompList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDueMonitoringCompList.RowDataBound
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
            Dim grdLinkActivityComp As GridView = DirectCast(e.Row.FindControl("grdLinkActivityComp"), GridView)

            Select Case MaintenanceActivityTypeID
                Case MaintenanceActivityTypes.ComponentService
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyCompMonitorServiceStatusList(ID).PartMonitorServiceID.ToString)
                Case MaintenanceActivityTypes.ComponentInspection
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyCompMonitorInspStatusList(ID).PartMonitorInspID.ToString)
                Case MaintenanceActivityTypes.ComponentDirective
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mTmpComplyCompMonitorModStatusList(ID).PartMonitorModID.ToString)
            End Select

            If mLinkMaintenanceList.Count > 0 Then
                e.Row.Cells(1).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
            End If

            grdLinkActivityComp.DataSource = mLinkMaintenanceList
            grdLinkActivityComp.DataBind()
        End If

    End Sub
    Private Sub dgDueMonitoringCompList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueMonitoringCompList.Sorting
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.ComponentService
                mTmpComplyCompMonitorServiceStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorServiceStatusList
                dgDueMonitoringCompList.DataBind()
            Case MaintenanceActivityTypes.ComponentInspection
                mTmpComplyCompMonitorInspStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyCompMonitorInspStatusList") = mTmpComplyCompMonitorInspStatusList
                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorInspStatusList
                dgDueMonitoringCompList.DataBind()
            Case MaintenanceActivityTypes.ComponentDirective
                mTmpComplyCompMonitorModStatusList.Sort(IIf(e.SortExpression = "RemainingValueFormatted", "MinimumRemainingValue", e.SortExpression), ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
                dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorModStatusList
                dgDueMonitoringCompList.DataBind()
        End Select
    End Sub
    Private Sub dgInstalledList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInstalledList.Sorting
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.RemovalComp
                mtmpInstalledCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mtmpInstalledCompList") = mtmpInstalledCompList
                dgInstalledList.DataSource = mtmpInstalledCompList
                dgInstalledList.DataBind()
        End Select
    End Sub
    Private Sub dgRemovedList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedList.Sorting
        Dim MaintenanceActivityTypeID As Integer = CType(Session("MaintenanceActivityTypeID"), Integer)
        Select Case MaintenanceActivityTypeID
            Case MaintenanceActivityTypes.InstallComp
                mtmpRemovedCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mtmpRemovedCompList") = mtmpRemovedCompList
                dgRemovedList.DataSource = mtmpRemovedCompList
                dgRemovedList.DataBind()
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        mMachineList = Nothing
        mAssemblyStatusList = Nothing
        Session.Remove("mMultiComplianceList")

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

        'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Response.Redirect("index.aspx")
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click
        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp '1. Removal Comp

            Case MaintenanceActivityTypes.InstallComp  '2. Install Comp

            Case MaintenanceActivityTypes.AssemblyService '5. Assembly Service
                dgDueMonitoringList.Visible = True
                If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
                    'Dim ComplyList = (From c In mTmpComplyAssemblyMonitorServiceStatusList
                    '                  Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper OrElse c.Reference.ToUpper().Contains(txtNote.Text.ToUpper)
                    '    Select c).ToList
                    Dim ComplyList = (From c In mTmpComplyAssemblyMonitorServiceStatusList
                                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Reference.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Zone.ToUpper().Contains(txtNote.Text.ToUpper)
                                      Select c).ToList()

                    dgDueMonitoringList.DataSource = ComplyList
                    dgDueMonitoringList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyAssemblyMonitorServiceStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(i).ID) Then
                                checkedIds.Add(mTmpComplyAssemblyMonitorServiceStatusList(i).ID.ToString)
                            End If
                        Next
                    End If

                    lblResult.Text = "List of Assembly Services as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.AssemblyInspection  '6. Assembly Inspection 
                dgDueMonitoringList.Visible = True
                If Not mTmpComplyAssemblyMonitorInspStatusList Is Nothing Then

                    Dim ComplyList = (From c In mTmpComplyAssemblyMonitorInspStatusList
                                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Reference.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Zone.ToUpper().Contains(txtNote.Text.ToUpper)
                                      Select c).ToList()

                    dgDueMonitoringList.DataSource = ComplyList
                    dgDueMonitoringList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyAssemblyMonitorInspStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).ID) Then
                                checkedIds.Add(mTmpComplyAssemblyMonitorInspStatusList(i).ID.ToString)
                            End If
                        Next
                    End If

                    lblResult.Text = "List of Assembly Inspections as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.AssemblyDirective   '7. Assembly Directive 
                dgDueMonitoringList.Visible = True
                If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyAssemblyMonitorModStatusList
                                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Reference.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Zone.ToUpper().Contains(txtNote.Text.ToUpper)
                                      Select c).ToList()

                    dgDueMonitoringList.DataSource = ComplyList
                    dgDueMonitoringList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyAssemblyMonitorModStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(i).ID) Then
                                checkedIds.Add(mTmpComplyAssemblyMonitorModStatusList(i).ID.ToString)
                            End If
                        Next
                    End If

                    lblResult.Text = "List of Assembly Directives as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentService    '8. Component Service 
                dgDueMonitoringCompList.Visible = True
                If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyCompMonitorServiceStatusList
                                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Reference.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Zone.ToUpper().Contains(txtNote.Text.ToUpper)
                                      Select c).ToList()

                    dgDueMonitoringCompList.DataSource = ComplyList
                    dgDueMonitoringCompList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyCompMonitorServiceStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mTmpComplyCompMonitorServiceStatusList(i).ID.ToString) Then
                                checkedIds.Add(mTmpComplyCompMonitorServiceStatusList(i).ID.ToString)
                            End If
                        Next
                    End If

                    lblResult.Text = "List of Component Services as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentInspection    '9. Component Inspection 
                dgDueMonitoringCompList.Visible = True
                If Not mTmpComplyCompMonitorInspStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyCompMonitorInspStatusList
                                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Reference.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Zone.ToUpper().Contains(txtNote.Text.ToUpper)
                                      Select c).ToList()

                    dgDueMonitoringCompList.DataSource = ComplyList
                    dgDueMonitoringCompList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyCompMonitorInspStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mTmpComplyCompMonitorInspStatusList(i).ID.ToString) Then
                                checkedIds.Add(mTmpComplyCompMonitorInspStatusList(i).ID.ToString)
                            End If
                        Next
                    End If


                    lblResult.Text = "List of Component Inspections as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentDirective     '10. Component Directive
                dgDueMonitoringCompList.Visible = True
                If Not mTmpComplyCompMonitorModStatusList Is Nothing Then
                    Dim ComplyList = (From c In mTmpComplyCompMonitorModStatusList
                                      Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Reference.ToUpper().Contains(txtNote.Text.ToUpper) _
                                            OrElse c.Zone.ToUpper().Contains(txtNote.Text.ToUpper)
                                      Select c).ToList()

                    dgDueMonitoringCompList.DataSource = ComplyList
                    dgDueMonitoringCompList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyCompMonitorModStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mTmpComplyCompMonitorModStatusList(i).ID.ToString) Then
                                checkedIds.Add(mTmpComplyCompMonitorModStatusList(i).ID.ToString)
                            End If
                        Next
                    End If

                    lblResult.Text = "List of Component Modifications as per selected criteria : " & ComplyList.Count & " Record(s) found."
                End If
        End Select

        upnlDueList.Update()
        upnlDueCompList.Update()
        upnlResult.Update()
        Controltovisibility()
        upnlButtonsTop.Update()
    End Sub
    Private Sub rdbAssemblyService_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdbAssemblyService.CheckedChanged, rdbAssemblyDirective.CheckedChanged, rdbAssemblyInspection.CheckedChanged, rdbComponentDirective.CheckedChanged, rdbComponentInspection.CheckedChanged, rdbComponentService.CheckedChanged
        If rdbAssemblyService.Checked = True Then
            Session("MaintenanceActivityTypeID") = MaintenanceActivityTypes.AssemblyService
        ElseIf rdbAssemblyDirective.Checked = True Then
            Session("MaintenanceActivityTypeID") = MaintenanceActivityTypes.AssemblyDirective
        ElseIf rdbAssemblyInspection.Checked = True Then
            Session("MaintenanceActivityTypeID") = MaintenanceActivityTypes.AssemblyInspection
        ElseIf rdbComponentDirective.Checked = True Then
            Session("MaintenanceActivityTypeID") = MaintenanceActivityTypes.ComponentDirective
        ElseIf rdbComponentInspection.Checked = True Then
            Session("MaintenanceActivityTypeID") = MaintenanceActivityTypes.ComponentInspection
        ElseIf rdbComponentService.Checked = True Then
            Session("MaintenanceActivityTypeID") = MaintenanceActivityTypes.ComponentService
        End If

        Select Case CType(Session("MaintenanceActivityTypeID"), Integer)
            Case MaintenanceActivityTypes.RemovalComp '1. Removal Comp
                dgInstalledList.Visible = True
                Dim i As Integer
                mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "<SELECT>")
                Dim cmbReason As DropDownList
                Session("mRemovalReasonList") = mRemovalReasonList
                If Not mtmpInstalledCompList Is Nothing Then
                    dgInstalledList.DataSource = mtmpInstalledCompList
                    dgInstalledList.DataBind()
                    For i = 0 To Me.dgInstalledList.Rows.Count - 1
                        cmbReason = CType(Me.dgInstalledList.Rows(i).FindControl("cmbReason"), DropDownList)
                        cmbReason.DataSource = mRemovalReasonList
                        cmbReason.DataBind()
                    Next
                End If
            Case MaintenanceActivityTypes.InstallComp  '2. Install Comp
                dgRemovedList.Visible = True
                If Not mtmpRemovedCompList Is Nothing Then
                    dgRemovedList.DataSource = mtmpRemovedCompList
                    dgRemovedList.DataBind()
                End If
            Case MaintenanceActivityTypes.AssemblyService '5. Assembly Service
                dgDueMonitoringList.Visible = True
                dgDueMonitoringCompList.Visible = False

                mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, Session("ModelName"), Session("SerialNo"), , , , , , , , True, , True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyAssemblyMonitorServiceStatusList Is Nothing Then
                    ''Dim ComplyList = (From c In mTmpComplyAssemblyMonitorServiceStatusList
                    ''     Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                    ''     Select c).ToList

                    ''dgDueMonitoringList.DataSource = ComplyList
                    ''dgDueMonitoringList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyAssemblyMonitorServiceStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyService, mTmpComplyAssemblyMonitorServiceStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorServiceStatusList(i).ID) Then
                                checkedIds.Add(mTmpComplyAssemblyMonitorServiceStatusList(i).ID.ToString)
                            End If
                        Next
                    End If
                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                    dgDueMonitoringList.DataBind()

                    Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList

                    lblResult.Text = "List of Assembly Services as per selected criteria : " & mTmpComplyAssemblyMonitorServiceStatusList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.AssemblyInspection  '6. Assembly Inspection 
                dgDueMonitoringList.Visible = True
                dgDueMonitoringCompList.Visible = False

                mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, Session("ModelName"), Session("SerialNo"), , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyAssemblyMonitorInspStatusList Is Nothing Then
                    'Dim ComplyList = (From c In mTmpComplyAssemblyMonitorInspStatusList
                    '    Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                    '    Select c).ToList

                    'dgDueMonitoringList.DataSource = ComplyList
                    'dgDueMonitoringList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyAssemblyMonitorInspStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyInspection, mTmpComplyAssemblyMonitorInspStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorInspStatusList(i).ID) Then
                                checkedIds.Add(mTmpComplyAssemblyMonitorInspStatusList(i).ID.ToString)
                            End If
                        Next
                    End If
                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
                    dgDueMonitoringList.DataBind()
                    Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList

                    lblResult.Text = "List of Assembly Inspections as per selected criteria : " & mTmpComplyAssemblyMonitorInspStatusList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.AssemblyDirective   '7. Assembly Directive 
                dgDueMonitoringList.Visible = True
                dgDueMonitoringCompList.Visible = False

                mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, Session("ModelName"), Session("SerialNo"), , , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyAssemblyMonitorModStatusList Is Nothing Then
                    'Dim ComplyList = (From c In mTmpComplyAssemblyMonitorModStatusList
                    '    Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                    '    Select c).ToList

                    'dgDueMonitoringList.DataSource = ComplyList
                    'dgDueMonitoringList.DataBind()



                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyAssemblyMonitorModStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.AssemblyDirective, mTmpComplyAssemblyMonitorModStatusList(i).AssemblyStatusID, mTmpComplyAssemblyMonitorModStatusList(i).ID) Then
                                checkedIds.Add(mTmpComplyAssemblyMonitorModStatusList(i).ID.ToString)
                            End If
                        Next
                    End If
                    dgDueMonitoringList.DataSource = mTmpComplyAssemblyMonitorModStatusList
                    dgDueMonitoringList.DataBind()
                    Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList

                    lblResult.Text = "List of Assembly Directives as per selected criteria : " & mTmpComplyAssemblyMonitorModStatusList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentService    '8. Component Service 
                dgDueMonitoringCompList.Visible = True
                dgDueMonitoringList.Visible = False

                mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , True, , True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyCompMonitorServiceStatusList Is Nothing Then
                    'Dim ComplyList = (From c In mTmpComplyCompMonitorServiceStatusList
                    '  Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                    '  Select c).ToList

                    'dgDueMonitoringCompList.DataSource = ComplyList
                    'dgDueMonitoringCompList.DataBind()


                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyCompMonitorServiceStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentService, mTmpComplyCompMonitorServiceStatusList(i).CompStatusID, mTmpComplyCompMonitorServiceStatusList(i).ID.ToString) Then
                                checkedIds.Add(mTmpComplyCompMonitorServiceStatusList(i).ID.ToString)
                            End If
                        Next
                    End If

                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorServiceStatusList
                    dgDueMonitoringCompList.DataBind()

                    Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList

                    lblResult.Text = "List of Component Services as per selected criteria : " & mTmpComplyCompMonitorServiceStatusList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentInspection    '9. Component Inspection 
                dgDueMonitoringCompList.Visible = True
                dgDueMonitoringList.Visible = False

                mTmpComplyCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyCompMonitorInspStatusList Is Nothing Then
                    'Dim ComplyList = (From c In mTmpComplyCompMonitorInspStatusList
                    ' Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                    ' Select c).ToList

                    'dgDueMonitoringCompList.DataSource = ComplyList
                    'dgDueMonitoringCompList.DataBind()


                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyCompMonitorInspStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentInspection, mTmpComplyCompMonitorInspStatusList(i).CompStatusID, mTmpComplyCompMonitorInspStatusList(i).ID.ToString) Then
                                checkedIds.Add(mTmpComplyCompMonitorInspStatusList(i).ID.ToString)
                            End If
                        Next
                    End If
                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorInspStatusList
                    dgDueMonitoringCompList.DataBind()

                    Session("mTmpComplyCompMonitorInspStatusList") = mTmpComplyCompMonitorInspStatusList

                    lblResult.Text = "List of Component Inspections as per selected criteria : " & mTmpComplyCompMonitorInspStatusList.Count & " Record(s) found."
                End If
            Case MaintenanceActivityTypes.ComponentDirective     '10. Component Directive
                dgDueMonitoringCompList.Visible = True
                dgDueMonitoringList.Visible = False
                mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, "", "", New Guid(AssemblyName), , , , , , , , , , True, True, SortBy:="MinimumRemainingValue")

                If Not mTmpComplyCompMonitorModStatusList Is Nothing Then
                    'Dim ComplyList = (From c In mTmpComplyCompMonitorModStatusList
                    'Where c.Note.ToUpper().Contains(txtNote.Text.ToUpper)
                    'Select c).ToList

                    'dgDueMonitoringCompList.DataSource = ComplyList
                    'dgDueMonitoringCompList.DataBind()

                    If mMultiComplianceList.Count <> 0 Then
                        For i As Integer = 0 To mTmpComplyCompMonitorModStatusList.Count - 1
                            If mMultiComplianceList.Contains(MaintenanceActivityTypes.ComponentDirective, mTmpComplyCompMonitorModStatusList(i).CompStatusID, mTmpComplyCompMonitorModStatusList(i).ID.ToString) Then
                                checkedIds.Add(mTmpComplyCompMonitorModStatusList(i).ID.ToString)
                            End If
                        Next
                    End If
                    dgDueMonitoringCompList.DataSource = mTmpComplyCompMonitorModStatusList
                    dgDueMonitoringCompList.DataBind()

                    Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
                    lblResult.Text = "List of Component Modifications as per selected criteria : " & mTmpComplyCompMonitorModStatusList.Count & " Record(s) found."
                End If
        End Select
        Controltovisibility()
        upnlDueList.Update()
        upnlDueCompList.Update()
        upnlResult.Update()
        upnlButtonsTop.Update()
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