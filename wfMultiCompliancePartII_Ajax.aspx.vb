'AJAX Created by :   Saylee
'Date            :   10-Nov-2014

Imports System.Collections.Generic
Imports System.Text
Public Class wfMultiCompliancePartII_Ajax
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

#Region " Page Load "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'MultiCompliance tab
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            DueType = 1
            Session("DueType") = DueType
            Session("MiddleFrame") = "wfMultiCompliancePartII_Ajax.aspx?"
            If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then
                ResetValues()
                lblAssembly.Enabled = False
                cmbAssembly.Enabled = False
                txtAsOnDate.Text = New SmartDate(Today.Date.ToString).FormattedText
                AOnDate = Today.Date
            End If
            SetComboOfMachine(AOnDate)
            setFocus(cmbAircraft)
            DataFieldBind()
            Session("mLogList") = Nothing
            SetLog()
        End If
        SetSession()

        ''Work Order tab
        'GetSessionWO()

        'If Not IsPostBack Then

        '    If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then
        '        ResetValuesWO()
        '        txtWOAsOnDate.Text = Today.Date
        '        AOnDateWO = Today.Date
        '    End If

        '    txtWOAsOnDate.Text = New SmartDate(Today.Date.ToString).FormattedText
        '    AsonDateWO = New SmartDate(Today.Date.ToString).FormattedText
        '    DataFieldBindWO()
        'End If
        'setFocus(cmbWOList)
        'ControlVisibilityWO()

    End Sub
#End Region

    'Multicompliance Tab
#Region " Multi Complaince "

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
    Private DueType As Integer
    Dim AircraftIndex As Integer
    Dim mAssemblyStatusList As AssemblyStatusList
    Dim AssemblyName As String
    Dim Assembly1 As String
    Private AssemblyStatusID As String
    Private ModelID As String
    Dim LogId As Guid
    Dim LogDate As String
    Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
    Dim tmpAssemblyStatusID As Guid
    Dim mAssemblyList As AssemblyList
    Public mMachineNameValueList As MachineNameValueList 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Dim IsReadOnly As Boolean = False  'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Private checkedIds As New List(Of String)()
    Dim tmpAssemblyStatusList As AssemblyStatusList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineListForCompliance"), MachineList)

        AOnDate = Session("AOnDate")
        Type = Session("Type")
        DueType = Session("DueType")

        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)

        AsonDate = Session("AsonDate")
        MachineName = Session("AircraftId")
        AssemblyName = Session("AssemblyId")
        AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblyList = Session("mAssemblyList")
        tmpAssemblyStatusList = CType(Session("tmpAssemblyStatusList"), AssemblyStatusList)
    End Sub
    Private Sub SetSession()
        Session("mMachineListForCompliance") = mMachineList
        Session("AOnDate") = AOnDate
        Session("Type") = Type
        Session("DueType") = DueType
        'Added by Saylee on 12-Feb-2009
        Session("mAssemblyStatusList") = mAssemblyStatusList
        Session("tmpAssemblyStatusList") = tmpAssemblyStatusList
        Session("mAssemblyList") = mAssemblyList
    End Sub
    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfMultiCompliancePartII_Ajax.aspx?" Then
            Session.Remove("mMachineListForCompliance")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("Type")
            Session.Remove("AvgMnths")

            Session.Remove("mAssemblyStatusList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")
            Session.Remove("LogId")
            Session.Remove("LogIdWO")
            Session.Remove("OpenFindNowSelectLogForm")
            Session.Remove("AssemblyStatusPeriodList")
            Session.Remove("AircraftId")
            Session.Remove("mLogList")
            Session.Remove("OpenFindNowSelectLogForm")
            Session.Remove("AOnDateWO")
            Session.Remove("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblyStatusList")
            Session.Remove("tmpAssemblyStatusList")
            Session.Remove("mAssemblyList")
            RemoveSessionWO()
        End If
    End Sub
    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            'AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = Guid.Empty.ToString
            Assembly1 = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString

            If cmbAssembly.SelectedItem.Text = "(All)" Or (cmbAssembly.SelectedItem.Text = "<SELECT>") Then
                'AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                AssemblyName = Guid.Empty.ToString
                Assembly1 = ""
                AssemblyType = "(All)"
                AssemblyStatusID = "{00000000-0000-0000-0000-000000000000}"

                Session("ModelName") = ""
                Session("SerialNo") = ""

                If CType(Session("LogId"), String) <> "" Or Not Session("LogId") Is Nothing Then
                    '' SetLog()
                    'do nothing
                Else
                    'Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
                    'AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
                    'Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList.
                    tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(cmbAircraft.SelectedValue.ToString),
                                                                             Guid.Empty.ToString, AssemblyType:="Airframe",
                                                                             IsAssemblyInstalled:=True,
                                                                             CurrentDate:=txtAsOnDate.Text.ToString)
                    AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
                    Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                    Session("tmpAssemblyStatusList") = tmpAssemblyStatusList
                    tmpAssemblyStatusList = Nothing
                End If

            Else
                ''AssemblyType = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyType
                ''AssemblyName = cmbAssembly.SelectedValue.ToString
                ''Assembly1 = cmbAssembly.SelectedItem.Text
                ''AssemblyStatusID = (mAssemblyStatusList(cmbAssembly.SelectedIndex).ID).ToString
                ''ModelID = (mAssemblyStatusList(cmbAssembly.SelectedIndex).ModelID).ToString
                ''Session("ModelName") = (mAssemblyStatusList(cmbAssembly.SelectedIndex).Model).ToString
                ''Session("SerialNo") = (mAssemblyStatusList(cmbAssembly.SelectedIndex).SerialNo).ToString
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                AssemblyStatusID = (mAssemblyList(cmbAssembly.SelectedIndex).ID).ToString
                ModelID = (mAssemblyList(cmbAssembly.SelectedIndex).ModelID).ToString
                Session("ModelName") = (mAssemblyList(cmbAssembly.SelectedIndex).ModelName).ToString
                Session("SerialNo") = (mAssemblyList(cmbAssembly.SelectedIndex).ModelSerialNo).ToString

                If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then
                    'do nothing
                Else
                    'AssemblyStatusPeriodList = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyStatusPeriodList
                    Dim tmpAssemblyStatusList As AssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(cmbAircraft.SelectedValue.ToString),
                                                                                                               cmbAssembly.SelectedValue.ToString,
                                                                                                               IsAssemblyInstalled:=True,
                                                                                                               CurrentDate:=txtAsOnDate.Text.ToString)
                    AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList 'tmpAssemblyStatusList(New Guid(cmbAssembly.SelectedValue.ToString)).AssemblyStatusPeriodList
                    Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                End If

            End If
            Session("Assembly1") = Assembly1
            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

        End If
        ''Average = txtAvgMnths.Text
        'If Not (txtAsOnDate.IsDateValue) Then
        '    AsonDate = ""
        '    AOnDate = ""
        'Else
        '    AsonDate = txtAsOnDate.Text.ToString
        '    AOnDate = txtAsOnDate.Text.ToString
        'End If
        AsonDate = txtAsOnDate.Text.ToString
        AOnDate = txtAsOnDate.Text.ToString
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        Session("AsonDate") = AsonDate
        Session("AonDate") = AOnDate
        Session("AircraftId") = MachineName
        Session("AssemblyId") = AssemblyName
        Session("AssemblyType") = AssemblyType
        Session("Aircraft") = Aircraft
    End Sub
    Private Sub SetLog()
        'If Val(Request.QueryString("Type")) = -1 Then
        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then

            LogId = New Guid(CType(Session("LogId"), String))
            Session("LogId") = CType(Session("LogId"), String)

            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog


            If Not LogId.Equals(Guid.Empty) Then
                ''Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId.ToString, SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
                tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(MachineName),
                                                                             AssemblyName,
                                                                             IsAssemblyInstalled:=True,
                                                                             LogID:=LogId.ToString,
                                                                             CurrentDate:=AsonDate)
                AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
                Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                dgDoneOnValue.DataSource = AssemblyStatusPeriodList
                dgDoneOnValue.DataBind()
                upnlValues.Update()
                tmpAssemblyStatusList = Nothing
            End If

        Else
        End If
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        If AsonDate <> "" Then
            txtAsOnDate.Text = AsonDate
        End If
        AsonDate = ""
        AssemblyName = Guid.Empty.ToString
    End Sub
    Private Sub SetReport()

        SetValues()
        'Dim mloglist As LogList
        'mloglist = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), , AsonDate)
        Dim x As String
        'If mloglist.Count > 0 Then
        '    x = mloglist(0).LogDate.ToShortDateString
        'Else
        '    x = txtAsOnDate.Text.ToString
        'End If
        Dim mMaxLogOfAircraft As MaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(New Guid(cmbAircraft.SelectedValue.ToString), AsonDate)
        If Not mMaxLogOfAircraft.LogDate.Equals(Guid.Empty) Then
            x = mMaxLogOfAircraft.LogDateFormatted
        Else
            x = txtAsOnDate.Text.ToString
        End If

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
                    SetComboOfMachine(AOnDate)
                    setFocus(cmbAircraft)
                    DataFieldBind()
                    upnlSearchCriteria.Update()
                    upnlValues.Update()
                    'Response.Redirect("wfMultiCompliancePartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfMultiCompliancePartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If DueType = 1 Then
            If custValidator.ControlToValidate = "cmbAircraft" Then
                If cmbAircraft.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Aircraft Required"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        End If
    End Sub
    Public Sub DataFieldBind()
        If CType(Session("OpenFindNowSelectLogForm"), Boolean) = True Then
            If Not IsNothing(MachineName) Or Not MachineName = Guid.Empty.ToString Then
                cmbAircraft.SelectedValue = MachineName
                'cmbAssembly.DataSource = mAssemblyStatusList
                'Session("mAssemblyStatusList") = mAssemblyStatusList
                cmbAssembly.DataSource = mAssemblyList
                Session("mAssemblyList") = mAssemblyList
                cmbAssembly.DataBind()
            End If
            If Not IsNothing(AssemblyName) Then
                If (Not New Guid(AssemblyName).Equals(Guid.Empty)) Then cmbAssembly.SelectedValue = AssemblyName
            End If
            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

            txtAsOnDate.Text = AsonDate
        End If

        'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        'Disable AddNew buttons if Aircraft is ReadOnly
        If IsReadOnly = True Then
            btnNext.Enabled = False
            lblReadOnly.Visible = True
        Else
            btnNext.Enabled = True
            lblReadOnly.Visible = False
        End If
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        ''If DueType = 1 Then
        mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(SELECT)", SkipIsForInventoryAircarft:=True)
        'Else
        '    mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        ''End If

        cmbAircraft.DataSource = mMachineList
        Session("mMachineListForCompliance") = mMachineList
        cmbAircraft.DataBind()

        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , False, , , True)
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim str As String = ""
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
#End Region

#Region " Events "

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyStatusList = Nothing
        AssemblyStatusPeriodList = Nothing
        Session("mMultiComplianceList") = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtAsOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsOnDate.TextChanged
        AOdate = txtAsOnDate.Text.ToString
        If AOnDate = AOdate Then
        Else
            SetComboOfMachine(AOdate)
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            'Changed by Vikrant ON 18-Jun-2013 FOR ALL17062013
            cmbAssembly.ClearSelection()
        End If
    End Sub

    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            cmbAssembly.SelectedIndex = 0
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True).Item(New Guid(cmbAircraft.SelectedValue)), MachineInfo).AssemblyStatusList
            'cmbAssembly.DataSource = mAssemblyStatusList
            'cmbAssembly.DataBind()
            'Session("mAssemblyStatusList") = mAssemblyStatusList

            mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0,
                                                                    cmbAircraft.SelectedValue,
                                                                    txtAsOnDate.Text.ToString,
                                                                    "(All)",
                                                                    True)
            cmbAssembly.DataSource = mAssemblyList
            cmbAssembly.DataBind()
            Session("mAssemblyList") = mAssemblyList

            tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(cmbAircraft.SelectedValue.ToString),
                                                                             mAssemblyList(1).ID.ToString,
                                                                             IsAssemblyInstalled:=True,
                                                                             CurrentDate:=txtAsOnDate.Text.ToString)
            AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
            Session("tmpAssemblyStatusList") = tmpAssemblyStatusList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

            'Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
            'AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
            'Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            Session("IsReadOnly") = IsReadOnly


            Session("mMultiComplianceList") = Nothing
            SetValues()
        End If
        Session.Remove("OpenFindNowSelectLogForm")
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        DataFieldBind()
        upnlSearchCriteria.Update()
        upnlValues.Update()
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        If cmbAssembly.SelectedIndex = 0 Then
            'Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
            'AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList

            tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(cmbAircraft.SelectedValue.ToString),
                                                                             mAssemblyList(1).ID.ToString, IsAssemblyInstalled:=True,
                                                                             CurrentDate:=txtAsOnDate.Text.ToString)
            AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            Session("tmpAssemblyStatusList") = tmpAssemblyStatusList
            tmpAssemblyStatusList = Nothing
        Else
            ' AssemblyStatusPeriodList = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyStatusPeriodList
            tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(cmbAircraft.SelectedValue.ToString),
                                                                             cmbAssembly.SelectedValue.ToString, IsAssemblyInstalled:=True,
                                                                             CurrentDate:=txtAsOnDate.Text.ToString)
            AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

            Session("tmpAssemblyStatusList") = tmpAssemblyStatusList
        End If
        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
        dgDoneOnValue.DataBind()
        upnlValues.Update()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If IsValid = True Then
            'SetReport()
            Session("LogId") = CType(Session("LogId"), String)
            mMachineList = Session("mMachineListForCompliance")
            Session("OpenFindNowSelectLogForm") = True
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            Session("ActiveTabIndex") = 0
            Dim str As String
            str = "openledgersame('wfMultiComplanceListPartII_Ajax.aspx?BackPage=Index.aspx" & "&DoneOn=" & CStr(IIf(txtAsOnDate.Text.ToString = "", Today.Date.ToShortDateString, txtAsOnDate.Text)) & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)


            'Session("AsOnDate") = CStr(IIf(txtAsOnDate.Text.ToString = "", Today.Date.ToShortDateString, txtAsOnDate.Text))
            'Session("MachineName") = MachineName
            'Session("HoutType") = CType(mMachineList(New Guid(MachineName)).HourType, String)
            'Session("AssemblyID") = AssemblyName
            '  ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMaintenanceActivityWindow", "OpenMaintenanceActivityWindow()", True)

        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        SetSession()
        Session("OpenFindNowSelectLogForm") = True
        SetValues()
        tmpAssemblyStatusList = Session("tmpAssemblyStatusList")
        '' Dim mtmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , ).Item(cmbAssembly.SelectedIndex), MachineInfo).AssemblyStatusList
        If cmbAssembly.SelectedIndex = 0 Then
            'tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList

            'Dim str As String
            ' str = "openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=index.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & tmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & tmpAssemblyStatusList(0).AssemblyID.ToString & "');"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

            Session("mFromType") = 3
            Session("mMachineId") = MachineName
            Session("mAssemblyStatusId") = tmpAssemblyStatusList(0).ID.ToString
            Session("mAssemblyID") = mAssemblyList(1).ID.ToString
            Session("mDoneOn") = CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate))
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
        Else
            'Dim str As String
            'str = "openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=index.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & mAssemblyStatusList(cmbAssembly.SelectedIndex).ID.ToString & "&AssemblyID=" & mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyID.ToString & "');"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

            Session("mFromType") = 3
            Session("mMachineId") = MachineName
            Session("mAssemblyStatusId") = tmpAssemblyStatusList.FirstItem.ID.ToString
            Session("mAssemblyID") = mAssemblyList(New Guid(cmbAssembly.SelectedValue)).ID.ToString
            Session("mDoneOn") = CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate))
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
        End If

    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Private Sub btnWO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWO.Click
    '    '' If IsValid = True Then
    '    'SetReport()
    '    '' Session("LogId") = CType(Session("LogId"), String)
    '    ''  mMachineList = Session("mMachineListForCompliance")
    '    Session("OpenFindNowSelectLogForm") = False
    '    ''Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
    '    'Dim str As String
    '    'str = "<script language='javascript'>openledgersame('wfSelectWO.aspx?BackPage=Index.aspx" & "&DoneOn=" & CStr(IIf(txtAsOnDate.Text = "", Today.Date.ToShortDateString, txtAsOnDate.Text)) & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString & "'); </script>"
    '    ' ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", str)

    '    Response.Redirect("wfSelectWOForMulticompliance.aspx??BackPage=Index.aspx")
    '    '' End If
    'End Sub
#End Region
#End Region

#Region " Checked Selection "

    Public Function NumeroChequeInclus(ByVal numero As String) As String

        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
#End Region

    'Link Activity
#Region " Link Maintenance "

#Region " Variable Declaration "

    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance
    Public mMultiComplianceList As New MultiComplianceList
    Public mAssemblyMonitorServiceStatusForLM As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorInspStatusForLM As AssemblyMonitorInspStatus
    Public mAssemblyMonitorModStatusForLM As AssemblyMonitorModStatus
    Public mLinkMaintenanceMonitorStatus As LinkMaintenaceMonitorStatus
    Public PeriodValues(,) As String
    Dim message As String = ""
    Dim mDetail As String = ""
#End Region

    Private Sub LinkMaintenance(MaintenanceActivityID As Guid, mMachine As Machine, Detail As String, DoneWONo As String, AssemblyId As Guid, MaintenanceActivity As String, mMachineMaintenance As MachineMaintenance, ByVal DoneRemark As String, ByVal LicenceNo As String, ByVal EmployeeID As String, ByVal Place As String)
        If AppSettings("LinkMaintenance") = "True" Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(MaintenanceActivityID.ToString)
            Session("mLinkMaintenanceList") = mLinkMaintenanceList
            If mLinkMaintenanceList.Count > 0 Then

                ShowLinkedMaintenaceActivity(mMachine, AssemblyId)

                'Save Link Activities
                If Not mMultiComplianceList Is Nothing Then
                    If mMultiComplianceList.Count > 0 Then
                        Dim Result As Boolean
                        SetLinkGridObject()
                        Dim LinkMaintenanceEvents As LinkedMaintenanceActivityEvents = New LinkedMaintenanceActivityEvents
                        LinkMaintenanceEvents.AssemblyLogInfo = MaintenanceActivity & ": " & Detail 'setting Mark Log Detail ...
                        Result = LinkMaintenanceEvents.SaveLinkedMaintenanceActivies(mMultiComplianceList, DoneWONo, AsonDateWO, mMachineMaintenance.LogID, mMachine.HourType, mMachine.ID, AssemblyId, PeriodValues, DoneRemark, LicenceNo:=LicenceNo, EmployeeID:=EmployeeID, Place:=Place)

                        If LinkMaintenanceEvents.ErrorStr.Length > 0 Then
                            Dim title As String = "Link Maintenance Alert !"
                            Dim message As String = LinkMaintenanceEvents.ErrorStr
                            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                        End If
                        Session.Remove("mMultiComplianceList")
                        mMultiComplianceList = Nothing
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ShowLinkedMaintenaceActivity(mMachine As Machine, AssemblyID As Guid)

        mMultiComplianceList = New MultiComplianceList

        Dim mPeriodUnitName As String
        Dim mFrequencyValue As String
        Dim mDoneOnValue As String
        Dim mCurrentValue As String
        Dim mDueOnValue As String
        Dim mElapsedValue As String
        Dim mRemainingValue As String
        Dim mDoneOn As String
        Dim mExtensionValue As String

        Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()

        For i As Integer = 0 To mLinkMaintenanceList.Count - 1

            If Not i = 0 Then

                mPeriodUnitName = String.Empty
                mFrequencyValue = String.Empty
                mDoneOnValue = String.Empty
                mCurrentValue = String.Empty
                mDueOnValue = String.Empty
                mElapsedValue = String.Empty
                mRemainingValue = String.Empty
                mDoneOn = String.Empty
                mExtensionValue = String.Empty
            End If

            Select Case mLinkMaintenanceList(i).LinkedMaintenanceTypeID

                Case 1 'Assembly Service

                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(New Guid(MachineName), mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorSeviceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorServiceStatusForLM = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusForLinkMaintenance(mPrevAssemblyMonitorSeviceStatus.ID, mPrevAssemblyMonitorSeviceStatus.AssemblyStatusID, AsonDateWO, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

                    Dim mAssemblyInfo As String = mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

                    For j As Integer = 0 To mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods.Count - 1

                        If mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodID = 2 Then

                            Dim PeriodCode As String = mPeriodUnitList(3, "").Code

                            If j = 0 Then

                                mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If

                        Else

                            Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code

                            If j = 0 Then

                                mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If


                        End If
                    Next
                    mMultiComplianceList.Add(mAssemblyMonitorServiceStatusForLM.ID, MaintenanceActivityTypes.AssemblyService, True, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Reference, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.MonitorTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelMonitorServiceTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Description, mAssemblyMonitorServiceStatusForLM.DoneOn.ToString, mAssemblyMonitorServiceStatusForLM.DoneWONo, mAssemblyMonitorServiceStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ATAChapter, , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 2 'Assembly Inspection

                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorInspStatusForLM = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusForLinkMaintenance(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDateWO, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

                    Dim mAssemblyInfo As String = mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

                    For j As Integer = 0 To mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods.Count - 1

                        If mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodID = 2 Then

                            Dim PeriodCode As String = mPeriodUnitList(3, "").Code

                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If

                        Else
                            Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code

                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If

                        End If

                    Next
                    mMultiComplianceList.Add(mAssemblyMonitorInspStatusForLM.ID, MaintenanceActivityTypes.AssemblyInspection, True, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Reference, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.MonitorTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelMonitorInspTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Description, mAssemblyMonitorInspStatusForLM.DoneOn.ToString, mAssemblyMonitorInspStatusForLM.DoneWONo, mAssemblyMonitorInspStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ATAChapter, , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 3 'Assembly Directive
                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorModStatusForLM = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusForLinkMaintenance(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDateWO, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

                    Dim mAssemblyInfo As String = mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

                    For j As Integer = 0 To mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods.Count - 1

                        If mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodID = 2 Then

                            Dim PeriodCode As String = mPeriodUnitList(3, "").Code

                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If

                        Else
                            Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code

                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If
                        End If


                    Next
                    mMultiComplianceList.Add(mAssemblyMonitorModStatusForLM.ID, MaintenanceActivityTypes.AssemblyDirective, True, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Reference, mAssemblyMonitorModStatusForLM.ModelMonitorMod.MonitorTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelMonitorModTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Description, mAssemblyMonitorModStatusForLM.DoneOn.ToString, mAssemblyMonitorModStatusForLM.DoneWONo, mAssemblyMonitorModStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ATAChapter, , , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing
            End Select
        Next
        Session("mMultiComplianceList") = mMultiComplianceList
    End Sub
#End Region

#Region "Common Events "
    Private Sub hdnBtnSelectLog_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSelectLog.Click
		If Session("mMachineNameValueList") Is Nothing Then
			mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , False, , , True)
			Session("mMachineNameValueList") = mMachineNameValueList
		End If

		If cmbAircraft.SelectedIndex > 0 Then
			SetLog()
		Else
			SetLogWO()
        End If
    End Sub

    Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TabContainer1.ActiveTabChanged
        Select Case TabContainer1.ActiveTabIndex
            Case 0
                If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then
                    ResetValues()
                    lblAssembly.Enabled = False
                    cmbAssembly.Enabled = False
                    txtAsOnDate.Text = New SmartDate(Today.Date.ToString).FormattedText
                    AOnDate = Today.Date
                End If
                SetComboOfMachine(AOnDate)
                setFocus(cmbAircraft)
                Session.Remove("IsReadOnly")
                IsReadOnly = Session("IsReadOnly")
                DataFieldBind()
                Session.Remove("IsReadOnly")
                mSelectDueJobsForWO = SelectDueJobsFornWO.NewSelectDueJobsFornWO()
                dgDueJob.DataSource = mSelectDueJobsForWO
                dgDueJob.DataBind()
                upnlWOGrid.Update()

                Session("mLogList") = Nothing
                upnlSearchCriteria.Update()
                upnlValues.Update()
            Case 1
                'Work Order tab
                GetSessionWO()
                Session.Remove("IsReadOnly")
                If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then
                    ResetValuesWO()
                    txtWOAsOnDate.Text = Today.Date
                    AOnDateWO = Today.Date
                End If

                txtWOAsOnDate.Text = New SmartDate(Today.Date.ToString).FormattedText
                AsonDateWO = New SmartDate(Today.Date.ToString).FormattedText
                txtSearch.Text = ""
                AssemblyStatusPeriodList = Nothing
                'mSelectDueJobsForWO = Nothing
                mSelectDueJobsForWO = SelectDueJobsFornWO.NewSelectDueJobsFornWO()
                dgDueJob.DataSource = mSelectDueJobsForWO
                dgDueJob.DataBind()

                DataFieldBindWO1()



                dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
                dgDoneOnValuesWO.DataBind()
                lblResult.Text = "List of Due Jobs as per selected criteria : " & "0 Record(s) found."
                ControlVisibilityWO()

                upnlbuttonsSave.Update()
                upnlButtonsSaveTop.Update()
                upnlWOGrid.Update()
                upnlComplianceValues.Update()
                upnlResult.Update()
            Case 2
                'If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then
                '    ResetValues()
                '    lblAssembly.Enabled = False
                '    cmbAssembly.Enabled = False
                '    txtAsOnDate.Text = New SmartDate(Today.Date.ToString).FormattedText
                '    AOnDate = Today.Date
                'End If
                Session.Remove("OpenFindNowSelectLogForm")
                Session("mLogList") = Nothing
                Session.Remove("IsReadOnly")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallRemovalCompList", "CallRemovalCompList();", True)



        End Select

    End Sub
    Public Sub SetLinkGridObject()
        Dim j As Int32

        ReDim PeriodValues(dgDoneOnValuesWO.Rows.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)

        For j = 0 To Me.dgDoneOnValuesWO.Rows.Count - 1

            PeriodValues(j, 0) = Me.dgDoneOnValuesWO.Rows(j).Cells(0).Text 'To Check same Period
            PeriodValues(j, 1) = Me.dgDoneOnValuesWO.Rows(j).Cells(1).Text 'Period Value 
        Next j

    End Sub
#End Region


    'Work Order Tab
#Region " Work Order "

#Region " Variable Declaration "
    Public mWOListForCombo As nWOListForCombo

    Public mSelectDueJobForWO As SelectDueJobFornWO
    Public mSelectDueJobsForWO As SelectDueJobsFornWO

    Public mWO As nWO
    Public mDueLimits As DueLimits

    Dim mLog As Log
    Dim AsonDateWO As String = ""
    Dim AOnDateWO As String = ""
    Public mAssemblyInfo As String
    Public mCompInfo As String

    Dim LogIdWO As String
    Dim WOName As String

    Public mBoardInfo As AircraftInformationBoard.BoardInfo

    Public mMachineMaintenanceForAssemblyService As MachineMaintenance
    Public mMachineMaintenanceListForAssemblyService As MachineMaintenanceList

    Public mMachineMaintenanceForAssemblyInsp As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenanceListForAssemblyInsp As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

    Public mMachineMaintenanceForAssemblyMod As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenanceListForAssemblyMod As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

    Public mMachineMaintenanceForCompService As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenanceListForCompService As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

    Public mMachineMaintenanceForCompInsp As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenanceListForCompInsp As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

    Public mMachineMaintenanceForCompMod As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenanceListForCompMod As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mAssemblyInfoDetail As String
    Dim strMSG As String = ""
    Dim IsSavedSuccessfully As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSessionWO()
        mWOListForCombo = Session("mWOListForCombo")
        AsonDate = Session("AsonDate")
        MachineName = Session("AircraftId")
        WOName = Session("WOId")
        AsonDateWO = Session("AsonDateWO")
        LogIdWO = CType(Session("LogIdWO"), String)
        mSelectDueJobForWO = Session("mSelectDueJobForWO")
        mSelectDueJobsForWO = Session("mSelectDueJobsForWO")
        AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")
        mWO = Session("mWO")
        mMachineMaintenanceForAssemblyService = CType(Session("mMachineMaintenanceForAssemblyService"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
        mMachineMaintenanceListForAssemblyService = CType(Session("mMachineMaintenanceListForAssemblyService"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

        mMachineMaintenanceForAssemblyInsp = CType(Session("mMachineMaintenanceForAssemblyInsp"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
        mMachineMaintenanceListForAssemblyInsp = CType(Session("mMachineMaintenanceListForAssemblyInsp"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

        mMachineMaintenanceForAssemblyMod = CType(Session("mMachineMaintenanceForAssemblyMod"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
        mMachineMaintenanceListForAssemblyMod = CType(Session("mMachineMaintenanceListForAssemblyMod"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

        mMachineMaintenanceForCompService = CType(Session("mMachineMaintenanceForCompService"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
        mMachineMaintenanceListForCompService = CType(Session("mMachineMaintenanceListForCompService"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

        mMachineMaintenanceForCompInsp = CType(Session("mMachineMaintenanceForCompInsp"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
        mMachineMaintenanceListForCompInsp = CType(Session("mMachineMaintenanceListForCompInsp"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

        mMachineMaintenanceForCompMod = CType(Session("mMachineMaintenanceForCompMod"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
        mMachineMaintenanceListForCompMod = CType(Session("mMachineMaintenanceListForCompMod"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub

    Private Sub SetSessionWO()
        Session("mWOListForCombo") = mWOListForCombo
        Session("LogIdWO") = LogIdWO
        Session("AsonDateWO") = AsonDateWO

        Session("AOnDateWO") = AOnDateWO
        Session("AircraftId") = MachineName

        Session("mSelectDueJobForWO") = mSelectDueJobForWO
        Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

        Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService 'Added by Saylee on 28th-Oct-2009
        Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceListForAssemblyService 'Added by Saylee on 28th-Oct-2009

        Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp 'Added by Saylee on 28th-Oct-2009
        Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceListForAssemblyInsp 'Added by Saylee on 28th-Oct-2009

        Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod 'Added by Saylee on 28th-Oct-2009
        Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceListForAssemblyMod 'Added by Saylee on 28th-Oct-2009

        Session("mMachineMaintenanceForCompService") = mMachineMaintenanceForCompService 'Added by Saylee on 28th-Oct-2009
        Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceListForCompService 'Added by Saylee on 28th-Oct-2009

        Session("mMachineMaintenanceForCompInsp") = mMachineMaintenanceForCompInsp 'Added by Saylee on 28th-Oct-2009
        Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceListForCompInsp 'Added by Saylee on 28th-Oct-2009

        Session("mMachineMaintenanceForCompMod") = mMachineMaintenanceForCompMod 'Added by Saylee on 28th-Oct-2009
        Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceListForCompMod 'Added by Saylee on 28th-Oct-2009
    End Sub

    Private Sub RemoveSessionWO()
        Session.Remove("mWOListForCombo")
        Session.Remove("AsonDateWO")
        Session.Remove("AOnDateWO")
        Session.Remove("AircraftId")

        Session.Remove("mSelectDueJobForWO")
        Session.Remove("mSelectDueJobsForWO")
        Session.Remove("mWO")
        Session.Remove("mDueLimits")

        Session.Remove("mLog")

        Session.Remove("MachineName")
        Session.Remove("mAssemblyInfo")
        Session.Remove("mCompInfo")
        Session.Remove("LogIdWO")

        Session.Remove("mMachineMaintenanceForAssemblyService")
        Session.Remove("mMachineMaintenanceListForAssemblyService")

        Session.Remove("mMachineMaintenanceForAssemblyInsp")
        Session.Remove("mMachineMaintenanceListForAssemblyInsp")

        Session.Remove("mMachineMaintenanceForAssemblyMod")
        Session.Remove("mMachineMaintenanceListForAssemblyMod")

        Session.Remove("mMachineMaintenanceForCompService")
        Session.Remove("mMachineMaintenanceListForCompService")

        Session.Remove("mMachineMaintenanceForCompInsp")
        Session.Remove("mMachineMaintenanceListForCompInsp")

        Session.Remove("mMachineMaintenanceForCompMod")
        Session.Remove("mMachineMaintenanceListForCompMod")

        Session.Remove("WOId")

        Session.Remove("OpenFindNowSelectLogForm")

    End Sub

    Private Sub AddJobs()
        'Dim item As GridViewRow
        '
        'Dim Recordno, PageItems As Integer
        'Dim i As Integer
        'PageItems = dgDueJob.Rows.Count - 1
        '' Set Selected Notes value  
        'For i = 0 To PageItems
        '    Recordno = i + dgDueJob.PageSize * dgDueJob.PageIndex
        '    item = dgDueJob.Rows(i)
        '    chkBox = CType(item.FindControl("chkSelect"), CheckBox)
        '    txtComplyRemark = CType(item.FindControl("txtAssemblyRemark"), TextBox)
        '    mSelectDueJobsForWOForWO(Recordno).IsSelected = chkBox.Checked
        '    mSelectDueJobsForWOForWO(Recordno).DoneRemark = txtComplyRemark.Text
        'Next
        Dim txtComplyRemark As TextBox
        Dim builder = New StringBuilder()
        builder.Append("You have selected the following checks :<br/>")
        ' get the selected checkboxes from the form data
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne, MSGBox.Message_Text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            ' we'll need a split to get the individual ids
            Dim values = checkString.Split(","c)
            For Each value As String In values
                builder.Append("<br/>")
                builder.Append(value)
                checkedIds.Add(value)
                'If mSelectDueJobsForWO.Contains(New Guid(value)) Then
                '    mSelectDueJobsForWO(New Guid(value)).IsSelected = True
                'End If
            Next

            Dim i As Integer
            For i = 0 To Me.dgDueJob.Rows.Count - 1
                If checkedIds.Contains(dgDueJob.Rows(i).Cells(2).Text) Then
                    Dim ID As String = dgDueJob.Rows(i).Cells(2).Text
                    txtComplyRemark = CType(Me.dgDueJob.Rows(i).FindControl("txtComplyRemark"), TextBox)
                    If mSelectDueJobsForWO.Contains(New Guid(ID)) Then
                        mSelectDueJobsForWO(New Guid(ID)).IsSelected = True
                        mSelectDueJobsForWO(New Guid(ID)).DoneRemark = txtComplyRemark.Text
                    End If
                End If
            Next
            values = ""
            values = ""
            checkString = Nothing
        End If


        Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
    End Sub
    'Private Sub SetValuesWO()
    '    If (cmbWOList.SelectedItem.Text = "(SELECT)") Then
    '        MachineName = "{00000000-0000-0000-0000-000000000000}"

    '    Else
    '        mWO = Session("mWO")
    '        MachineName = mWO.MachineID.ToString
    '        WOName = mWO.ID.ToString

    '        If CType(Session("LogId"), String) <> "" Or Not Session("LogId") Is Nothing Then
    '            '' SetLog()
    '            'do nothing
    '        Else
    '            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWOAsOnDate.Text.ToString, MachineName.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
    '            AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
    '            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
    '            tmpAssemblyStatusList = Nothing
    '        End If

    '        dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
    '        dgDoneOnValuesWO.DataBind()

    '    End If

    '    If txtWOAsOnDate.Text = "" Then
    '        AsonDateWO = ""
    '        AsonDateWO = ""
    '    Else
    '        AsonDateWO = txtWOAsOnDate.Text
    '        AOnDateWO = txtWOAsOnDate.Text
    '    End If

    '    Session("AsonDateWO") = AsonDateWO
    '    Session("AOnDateWO") = AOnDateWO
    '    Session("AircraftId") = MachineName
    '    Session("WOId") = WOName
    'End Sub
    Private Sub SetLogWO()
        If Session("LogIdWO") <> "" Or Not Session("LogIdWO") Is Nothing Then

            LogIdWO = CType(Session("LogIdWO"), String)
            Dim LogDate = Session("LogDate")
            Session("LogIdWO") = CType(Session("LogIdWO"), String)

            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogIdWO.ToString))
            Session("mLog") = mLog

            If Not LogIdWO = Guid.Empty.ToString Then
                '  tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDateWO.ToString, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogIdWO.ToString, SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
                tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(MachineName),
                                                                             AssemblyName,
                                                                             IsAssemblyInstalled:=True,
                                                                             LogID:=LogIdWO.ToString,
                                                                             CurrentDate:=AsonDateWO.ToString)

                AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
                Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
                dgDoneOnValuesWO.DataBind()
                upnlComplianceValues.Update()
                tmpAssemblyStatusList = Nothing
            End If


        Else
        End If
    End Sub
    Private Sub ControlVisibilityWO()
        If Not mSelectDueJobsForWO Is Nothing Then
            If mSelectDueJobsForWO.Count > 0 Then
                btnSave.Enabled = True
                If mSelectDueJobsForWO.Count > 10 Then
                    btnSaveTop.Visible = True
                    btnCloseTop.Visible = True
                Else
                    btnSaveTop.Visible = False
                    btnCloseTop.Visible = False
                End If
                lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
            Else
                btnSaveTop.Visible = False
                btnCloseTop.Visible = False
            End If
        Else
            btnSave.Enabled = False
            btnSaveTop.Visible = False
            btnCloseTop.Visible = False
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ResetValuesWO()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        If AsonDateWO <> "" Then
            txtWOAsOnDate.Text = AsonDateWO
        End If
        AsonDateWO = ""
        AssemblyName = Guid.Empty.ToString
        mSelectDueJobsForWO = Nothing
    End Sub
    Private Sub SetWOGrid()
        For j As Integer = 0 To dgWOList.Rows.Count - 1
            If mMachineNameValueList.Contains(dgWOList.Rows(j).Cells(3).Text) Then
                If mMachineNameValueList(dgWOList.Rows(j).Cells(3).Text).IsReadOnly = True Then
                    dgWOList.Rows(j).Cells(6).Enabled = False
                Else
                    dgWOList.Rows(j).Cells(6).Enabled = True
                End If
            End If
        Next
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidateWO(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "WOIDValue" Then
        '    If WOIDValue.Text = "" Then
        '        custValidator.ErrorMessage = "Work Order Required"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
    End Sub
    Private Sub DataFieldBindWO()
        mWOListForCombo = nWOListForCombo.GetnWOListForComplaince()
        dgWOList.DataSource = mWOListForCombo
        dgWOList.DataBind()

        Dim mMachineMaintenanceList As MachineMaintenanceList
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceList


        IIf(AsonDateWO <> "", txtWOAsOnDate.Text = CDate(New SmartDate(AsonDateWO.ToString).FormattedText), txtWOAsOnDate.Text = New SmartDate(Today.Date.ToString).FormattedText)
        'If WOName <> "" Then
        '    cmbWOList.SelectedValue = WOName
        'Else
        '    cmbWOList.SelectedIndex = 0
        'End If

        If Not mSelectDueJobsForWO Is Nothing Then
            dgDueJob.DataSource = mSelectDueJobsForWO
            dgDueJob.DataBind()
        End If

        If Not AssemblyStatusPeriodList Is Nothing Then
            dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
            dgDoneOnValuesWO.DataBind()
        End If

        If CType(Session("OpenFindNowSelectLogForm"), Boolean) = True Then
            dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
            dgDoneOnValuesWO.DataBind()
            txtWOAsOnDate.Text = AsonDateWO
        End If
    End Sub
    Private Sub DataFieldBindWO1()
        mWOListForCombo = nWOListForCombo.GetnWOListForComplaince()
        dgWOList.DataSource = mWOListForCombo
        dgWOList.DataBind()
        Session("mWOListForCombo") = mWOListForCombo

        Dim mMachineMaintenanceList As MachineMaintenanceList
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceList


        IIf(AsonDateWO <> "", txtWOAsOnDate.Text = CDate(New SmartDate(AsonDateWO.ToString).FormattedText), txtWOAsOnDate.Text = New SmartDate(Today.Date.ToString).FormattedText)
        'If WOName <> "" Then
        '    cmbWOList.SelectedValue = WOName
        'Else
        '    cmbWOList.SelectedIndex = 0
        'End If


        If CType(Session("OpenFindNowSelectLogForm"), Boolean) = True Then
            dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
            dgDoneOnValuesWO.DataBind()
            txtWOAsOnDate.Text = AsonDateWO
        End If
        SetWOGrid()
    End Sub
#End Region

#Region " Machine Maintenance "
    Private Sub SaveMachineMaintenance(ByVal mMachineMaintenance As MachineMaintenance)
        'Added by Saylee on 9th-Oct-2009
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
        ''  End If
    End Sub
#End Region

#Region " Save Status "
#Region "Assembly Service Status"
    Private Sub SaveAssemblyMonitorServiceStatusBoardInfo(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus)
        Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod
        Dim DueOnValue As String

        If (mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorServiceStatus.IsApplicable = False) Then
            DueOnValue = ""
        Else
            For Each mAssemblyMonitorServiceStatusPeriod In mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If mAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                Else
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueTextFormatted
                End If
            Next
        End If

        mBoardInfo = Session("mBoardInfo")
        If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
            mBoardInfo.MonitorID = mAssemblyMonitorServiceStatus.ID
            mBoardInfo.DueOnValue = DueOnValue
            mBoardInfo.ApplyEdit()
            mBoardInfo.Save()
            Session("mBoardInfo") = mBoardInfo
        End If
        Session("mAircraftInformationBoardList") = Nothing
    End Sub
    Public Function SaveAssemblyMonitorServiceStatus(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
        Dim clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        clnAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

        SetAssemblyMonitorServiceStatusObject(mAssemblyMonitorServiceStatus, mSelectDueJobForWO)

        If mAssemblyMonitorServiceStatus.IsValid Then
            If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Monitor Service Status.Assembly Monitor Service Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Service Status.Assembly Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Try
                mAssemblyMonitorServiceStatus.ApplyEdit()
                mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
                SaveAssemblyMonitorServiceStatusBoardInfo(mAssemblyMonitorServiceStatus)
                SaveMachineMaintenance(mMachineMaintenanceForAssemblyService)
                Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                mAssemblyInfo = Session("mAssemblyInfo")
                MarkLog(Util.Action.Save, "AssemblyServiceMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
                mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
                mDetail = "Model : " + mSelectDueJobForWO.Model + " Serial No : " + mSelectDueJobForWO.SerialNo + " Monitor Info : " + mSelectDueJobForWO.ModelMonitorCode + " Monitor Type : " + mSelectDueJobForWO.MonitorType
                MarkLog(Util.Action.Save, "Assembly Service Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("mAssemblyMonitorServiceStatus") = clnAssemblyMonitorServiceStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Finally
                clnAssemblyMonitorServiceStatus = Nothing
            End Try
        Else
            If Not mAssemblyMonitorServiceStatus.IsValid Then
                For i As Integer = 0 To mAssemblyMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mAssemblyMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            End If
        End If
    End Function
    Private Sub SetAssemblyMonitorServiceStatusObject(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
        mAssemblyMonitorServiceStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
        mAssemblyMonitorServiceStatus.DoneWONo = mWO.WONumber

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForAssemblyService.Contains(mAssemblyMonitorServiceStatus.ID, 5, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForAssemblyService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 5, txtWOAsOnDate.Text, mAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorServiceStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForAssemblyService = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatus.ID, 5)
        End If

        With mMachineMaintenanceForAssemblyService
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mAssemblyMonitorServiceStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtWOAsOnDate.Text
            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtWOAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService
    End Sub
#End Region

#Region "Assembly Inspection Status"
    Private Sub SaveAssemblyMonitorInspStatusBoardInfo(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus)
        Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod
        Dim DueOnValue As String

        If (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And Not mAssemblyMonitorInspStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorInspStatus.IsApplicable = False) Then
            DueOnValue = ""
        Else
            For Each mAssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                If mAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                Else
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueTextFormatted
                End If
            Next
        End If

        mBoardInfo = Session("mBoardInfo")
        If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
            mBoardInfo.MonitorID = mAssemblyMonitorInspStatus.ID
            mBoardInfo.DueOnValue = DueOnValue
            mBoardInfo.ApplyEdit()
            mBoardInfo.Save()
            Session("mBoardInfo") = mBoardInfo
        End If
        Session("mAircraftInformationBoardList") = Nothing
    End Sub
    Public Function SaveAssemblyMonitorInspStatus(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
        Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)

        SetAssemblyMonitorInspStatusObject(mAssemblyMonitorInspStatus, mSelectDueJobForWO)

        If mAssemblyMonitorInspStatus.IsValid Then
            If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Insp Status.Assembly Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                msg1.Show()
            End If
            Try
                mAssemblyMonitorInspStatus.ApplyEdit()
                mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
                SaveAssemblyMonitorInspStatusBoardInfo(mAssemblyMonitorInspStatus)
                SaveMachineMaintenance(mMachineMaintenanceForAssemblyInsp)
                Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                mAssemblyInfo = Session("mAssemblyInfo")
                mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
                mDetail = "Model : " + mSelectDueJobForWO.Model + " Serial No : " + mSelectDueJobForWO.SerialNo + " Monitor Info : " + mSelectDueJobForWO.ModelMonitorCode + " Monitor Type : " + mSelectDueJobForWO.MonitorType
                MarkLog(Util.Action.Save, "Assembly Inspection Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("mAssemblyMonitorInspStatus") = clnAssemblyMonitorInspStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Finally
                clnAssemblyMonitorInspStatus = Nothing
            End Try
        Else
            If Not mAssemblyMonitorInspStatus.IsValid Then
                For i As Integer = 0 To mAssemblyMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mAssemblyMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            End If
        End If
    End Function
    Private Sub SetAssemblyMonitorInspStatusObject(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
        mAssemblyMonitorInspStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
        mAssemblyMonitorInspStatus.DoneWONo = mWO.WONumber

        If Not (mMachineMaintenanceListForAssemblyInsp.Contains(mAssemblyMonitorInspStatus.ID, 6, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForAssemblyInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 6, txtWOAsOnDate.Text, mAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorInspStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForAssemblyInsp = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatus.ID, 6)
        End If

        With mMachineMaintenanceForAssemblyInsp
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mAssemblyMonitorInspStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtWOAsOnDate.Text
            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtWOAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp
    End Sub
#End Region

#Region "Assembly Modification Status"
    Private Sub SaveAssemblyMonitorModStatusBoardInfo(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus)
        Dim mAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriod
        Dim DueOnValue As String

        If (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorModStatus.IsApplicable = False) Then
            DueOnValue = ""
        Else
            For Each mAssemblyMonitorModStatusPeriod In mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If mAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                Else
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueTextFormatted
                End If
            Next
        End If

        mBoardInfo = Session("mBoardInfo")
        If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
            mBoardInfo.MonitorID = mAssemblyMonitorModStatus.ID
            mBoardInfo.DueOnValue = DueOnValue
            mBoardInfo.ApplyEdit()
            mBoardInfo.Save()
            Session("mBoardInfo") = mBoardInfo
        End If
        Session("mAircraftInformationBoardList") = Nothing
    End Sub
    Public Function SaveAssemblyMonitorModStatus(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
        Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus
        clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)

        SetAssemblyMonitorModStatusObject(mAssemblyMonitorModStatus, mSelectDueJobForWO)

        If mAssemblyMonitorModStatus.IsValid Then
            If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Mod Status.Assembly Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Try
                mAssemblyMonitorModStatus.ApplyEdit()
                mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
                SaveAssemblyMonitorModStatusBoardInfo(mAssemblyMonitorModStatus)
                SaveMachineMaintenance(mMachineMaintenanceForAssemblyMod)
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                mAssemblyInfo = Session("mAssemblyInfo")
                'MarkLog(Util.Action.Save, "AssemblyModMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID)
                mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
                mDetail = "Model : " + mSelectDueJobForWO.Model + " Serial No : " + mSelectDueJobForWO.SerialNo + " Monitor Info : " + mSelectDueJobForWO.ModelMonitorCode + " Monitor Type : " + mSelectDueJobForWO.MonitorType
                MarkLog(Util.Action.Save, "Assembly Modification Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("mAssemblyMonitorModStatus") = clnAssemblyMonitorModStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Finally
                clnAssemblyMonitorModStatus = Nothing
            End Try
        End If
    End Function
    Private Sub SetAssemblyMonitorModStatusObject(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
        mAssemblyMonitorModStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
        mAssemblyMonitorModStatus.DoneWONo = mWO.WONumber

        If Not (mMachineMaintenanceListForAssemblyMod.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForAssemblyMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 7, txtWOAsOnDate.Text, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorModStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForAssemblyMod = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
        End If

        With mMachineMaintenanceForAssemblyMod
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtWOAsOnDate.Text
            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtWOAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod
    End Sub
#End Region

#Region "Component Service Status"
    Private Sub SetCompMonitorServiceStatusObject(ByVal mCompMonitorServiceStatus As CompMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
        mCompMonitorServiceStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
        mCompMonitorServiceStatus.DoneWONo = mWO.WONumber

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForCompService.Contains(mCompMonitorServiceStatus.ID, 8, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForCompService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 8, txtWOAsOnDate.Text, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mCompMonitorServiceStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForCompService = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatus.ID, 8)
        End If

        With mMachineMaintenanceForCompService
            ''.MachineID = mCompStatus.MachineID
            ''.MaintenanceActivityTypeID =8
            .MaintenanceID = mCompMonitorServiceStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtWOAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtWOAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForCompService") = mMachineMaintenanceForCompService
    End Sub
    Public Function SaveCompMonitorServiceStatus(ByVal mCompMonitorServiceStatus As CompMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
        Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus
        clnCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)

        SetCompMonitorServiceStatusObject(mCompMonitorServiceStatus, mSelectDueJobForWO)
        If mCompMonitorServiceStatus.IsValid Then
            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Service Status.Component Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Try
                mCompMonitorServiceStatus.ApplyEdit()
                mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                SaveMachineMaintenance(mMachineMaintenanceForCompService)
                mCompInfo = Session("mCompInfo")
                'MarkLog(Util.Action.Save, "CompServiceMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
                mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
                MarkLog(Util.Action.Save, "Component Service Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)

                Return True
            Catch ex As SqlException
                Session("mCompMonitorServiceStatus") = clnCompMonitorServiceStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Finally
                clnCompMonitorServiceStatus = Nothing
            End Try
        End If
    End Function
#End Region

#Region "Component Insp Status"
    Private Sub SetCompMonitorInspStatusObject(ByVal mCompMonitorInspStatus As CompMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
        mCompMonitorInspStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
        mCompMonitorInspStatus.DoneWONo = mWO.WONumber

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForCompInsp.Contains(mCompMonitorInspStatus.ID, 9, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForCompInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 9, txtWOAsOnDate.Text, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, mCompMonitorInspStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForCompInsp = MachineMaintenance.GetMachineMaintenance(mCompMonitorInspStatus.ID, 9)
        End If

        With mMachineMaintenanceForCompInsp
            ''.MachineID = mCompStatus.MachineID
            ''.MaintenanceActivityTypeID =8
            .MaintenanceID = mCompMonitorInspStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtWOAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtWOAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForCompInsp") = mMachineMaintenanceForCompInsp
    End Sub
    Public Function SaveCompMonitorInspStatus(ByVal mCompMonitorInspStatus As CompMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
        Dim clnCompMonitorInspStatus As CompMonitorInspStatus
        clnCompMonitorInspStatus = CType(mCompMonitorInspStatus.Clone, CompMonitorInspStatus)

        SetCompMonitorInspStatusObject(mCompMonitorInspStatus, mSelectDueJobForWO)
        If mCompMonitorInspStatus.IsValid Then
            If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Insp Status.Component Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Try
                mCompMonitorInspStatus.ApplyEdit()
                mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                SaveMachineMaintenance(mMachineMaintenanceForCompInsp)
                mCompInfo = Session("mCompInfo")
                'MarkLog(Util.Action.Save, "CompInspMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorInspStatus.ID)
                mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
                MarkLog(Util.Action.Save, "Component Inspection Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("mCompMonitorInspStatus") = clnCompMonitorInspStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Finally
                clnCompMonitorInspStatus = Nothing
            End Try
        End If
    End Function
#End Region

#Region "Component Mod Status"
    Private Sub SetCompMonitorModStatusObject(ByVal mCompMonitorModStatus As CompMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
        mCompMonitorModStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
        mCompMonitorModStatus.DoneWONo = mWO.WONumber

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForCompMod.Contains(mCompMonitorModStatus.ID, 10, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForCompMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 10, txtWOAsOnDate.Text, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, mCompMonitorModStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForCompMod = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatus.ID, 10)
        End If

        With mMachineMaintenanceForCompMod
            ''.MachineID = mCompStatus.MachineID
            ''.MaintenanceActivityTypeID =8
            .MaintenanceID = mCompMonitorModStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtWOAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtWOAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForCompMod") = mMachineMaintenanceForCompMod
    End Sub
    Public Function SaveCompMonitorModStatus(ByVal mCompMonitorModStatus As CompMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
        Dim clnCompMonitorModStatus As CompMonitorModStatus
        clnCompMonitorModStatus = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)

        SetCompMonitorModStatusObject(mCompMonitorModStatus, mSelectDueJobForWO)
        If mCompMonitorModStatus.IsValid Then
            If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Mod Status.Component Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Try
                mCompMonitorModStatus.ApplyEdit()
                mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                SaveMachineMaintenance(mMachineMaintenanceForCompMod)
                mCompInfo = Session("mCompInfo")
                'MarkLog(Util.Action.Save, "CompModMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)
                mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
                MarkLog(Util.Action.Save, "Component Modification Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("mCompMonitorModStatus") = clnCompMonitorModStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Finally
                clnCompMonitorModStatus = Nothing
            End Try
        End If
    End Function
#End Region
#End Region

#Region " Events "
    'Private Sub cmbWOList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWOList.SelectedIndexChanged
    '    mDueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))
    '    If cmbWOList.SelectedIndex > 0 Then
    '        mWO = nWO.GetWO(New Guid(cmbWOList.SelectedValue))
    '        mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtWOAsOnDate.Text, mDueLimits, mWO.MachineID.ToString, 0, mWO)

    '        If mSelectDueJobsForWO.Count = 0 Then
    '            'Dim msg1 As New SIMsgBox(Page, "Monitoring Services / Inspections / Directives not available", "<BR><BR> All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OkOnly)
    '            'msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
    '            'msg1.Show()
    '            dgDueJob.DataSource = mSelectDueJobsForWO
    '            Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
    '            Session("mWO") = mWO
    '            dgDueJob.DataBind()
    '            ControlVisibilityWO()
    '            upnlbuttonsSave.Update()
    '            upnlButtonsSaveTop.Update()
    '            upnlWOGrid.Update()
    '            upnlComplianceValues.Update()
    '            upnlResult.Update()
    '            MSGBoxCtrl.show("Jobs not available!", "All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OkOnly, "")
    '            Exit Sub

    '        End If
    '        dgDueJob.DataSource = mSelectDueJobsForWO
    '        Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
    '        Session("mWO") = mWO
    '        dgDueJob.DataBind()

    '        Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWOAsOnDate.Text, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
    '        AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
    '        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

    '        dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
    '        dgDoneOnValuesWO.DataBind()
    '        If mSelectDueJobsForWO.Count > 0 Then
    '            btnSave.Enabled = True
    '            If mSelectDueJobsForWO.Count > 10 Then btnSaveTop.Visible = True
    '            If mSelectDueJobsForWO.Count > 10 Then btnCloseTop.Visible = True

    '        Else
    '            btnSave.Enabled = False
    '        End If
    '        lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
    '    Else
    '        mSelectDueJobsForWO = Nothing
    '        dgDueJob.DataSource = mSelectDueJobsForWO
    '        dgDueJob.DataBind()
    '        btnSave.Enabled = False
    '        lblResult.Text = "List of Due Jobs as per selected criteria : " & "0 Record(s) found."
    '    End If
    '    ControlVisibilityWO()
    '    upnlbuttonsSave.Update()
    '    upnlButtonsSaveTop.Update()
    '    upnlWOGrid.Update()
    '    upnlComplianceValues.Update()
    '    upnlResult.Update()
    'End Sub
    Private Sub dgWOList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
                mWOListForCombo = Session("mWOListForCombo")
                Dim mID As Guid = mWOListForCombo(Index).ID
                mDueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))

                mWO = nWO.GetWO(mID)

                mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtWOAsOnDate.Text, mDueLimits, mWO.MachineID.ToString, 0, mWO)
                MachineName = mWO.MachineID.ToString
                WOName = mWO.ID.ToString

                Session("AircraftId") = MachineName
                Session("WOId") = WOName

                If mSelectDueJobsForWO.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, "Monitoring Services / Inspections / Directives not available", "<BR><BR> All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    'msg1.Show()
                    dgDueJob.DataSource = mSelectDueJobsForWO
                    Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
                    Session("mWO") = mWO
                    dgDueJob.DataBind()
                    SetWOGrid()
                    ControlVisibilityWO()
                    upnlbuttonsSave.Update()
                    upnlButtonsSaveTop.Update()
                    upnlWOGrid.Update()
                    upnlComplianceValues.Update()
                    upnlResult.Update()
                    MSGBoxCtrl.show("Jobs not available!", "All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub

                End If
                dgDueJob.DataSource = mSelectDueJobsForWO
                Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
                Session("mWO") = mWO
                dgDueJob.DataBind()

                '  tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWOAsOnDate.Text, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
                tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mWO.MachineID,
                                                                             Guid.Empty.ToString, AssemblyType:="Airframe",
                                                                             IsAssemblyInstalled:=True,
                                                                             CurrentDate:=txtWOAsOnDate.Text.ToString)
                AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
                Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

                dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
                dgDoneOnValuesWO.DataBind()
                If mSelectDueJobsForWO.Count > 0 Then
                    btnSave.Enabled = True
                    If mSelectDueJobsForWO.Count > 10 Then btnSaveTop.Visible = True
                    If mSelectDueJobsForWO.Count > 10 Then btnCloseTop.Visible = True

                Else
                    btnSave.Enabled = False
                End If
                lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
                'Else
                'mSelectDueJobsForWO = Nothing
                'dgDueJob.DataSource = mSelectDueJobsForWO
                'dgDueJob.DataBind()
                'btnSave.Enabled = False
                'lblResult.Text = "List of Due Jobs as per selected criteria : " & "0 Record(s) found."
                'End If
                SetWOGrid()
                ControlVisibilityWO()
                upnlbuttonsSave.Update()
                upnlButtonsSaveTop.Update()
                upnlWOGrid.Update()
                upnlComplianceValues.Update()
                upnlResult.Update()
        End Select
    End Sub
    Private Sub btnClose1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose1.Click, btnCloseTop.Click
        RemoveSessionWO()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub btnSelectLogWO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLogWO.Click
        If Not IsValid Then upnlWOValidationSummary.Update() : Exit Sub

        WOName = Session("WOId")
        If (dgWOList.Rows.Count = 0) Or WOName = "" Then
            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Please select Work Order from the List.", False), True)
            MSGBoxCtrl.show("Work Order not available!", "Please select Work Order from the List.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        GetSessionWO()
        Dim checkString = Request.Form("chkSelect")
        If Not checkString Is Nothing Then AddJobs()

        'SetValuesWO()
        SetSessionWO()
        Session("OpenFindNowSelectLogForm") = True
        mWO = Session("mWO")

        'tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
        tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mWO.MachineID,
                                                                             Guid.Empty.ToString, AssemblyType:="Airframe",
                                                                             IsAssemblyInstalled:=True,
                                                                             CurrentDate:=txtAsOnDate.Text.ToString)
        ' Response.Redirect("wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=wfSelectWOForMulticompliance.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & tmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & tmpAssemblyStatusList(0).AssemblyID.ToString)
        Session("mFromType") = 3
        Session("mMachineId") = MachineName
        Session("mAssemblyStatusId") = tmpAssemblyStatusList(0).ID.ToString
        Session("mAssemblyID") = tmpAssemblyStatusList(0).AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(AsonDateWO = "", Today.Date.ToShortDateString, AsonDateWO))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
        'SetValuesWO()
        GetSessionWO()
        mSelectDueJobsForWO = Session("mSelectDueJobsForWO")
        AddJobs()

        If txtWOAsOnDate.Text = "" Then
            AsonDateWO = ""
            AsonDateWO = ""
        Else
            AsonDateWO = txtWOAsOnDate.Text
            AOnDateWO = txtWOAsOnDate.Text
        End If

        Session("AsonDateWO") = AsonDateWO
        Session("AOnDateWO") = AOnDateWO

        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            ' we'll need a split to get the individual ids
            Dim values = checkString.Split(","c)
            For Each value As String In values
                If mSelectDueJobsForWO.Item(New Guid(value)).IsSelected = True Then
                    Dim mMachine As Machine = Machine.GetMachine(mSelectDueJobsForWO.Item(New Guid(value)).MachineID)
                    If mSelectDueJobsForWO(New Guid(value)).OnAssemblyOrComponent = "Assembly" Then
                        Select Case mSelectDueJobsForWO(New Guid(value)).DataType
                            Case "Servicing" 'Service

                                Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
                                Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mSelectDueJobsForWO.Item(New Guid(value)).ID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, mMachine.HourType)
                                If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    If CType(Session("FromLog"), Boolean) = True Then
                                        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDateWO, mSelectDueJobsForWO(New Guid(value)).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, New Guid(LogIdWO), mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
                                    Else
                                        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDateWO, mSelectDueJobsForWO(New Guid(value)).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
                                    End If

                                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                                    Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
                                    Session("From") = 0 'New record
                                    ''
                                    mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
                                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

                                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(New Guid(value)).AssemblyStatusID)
                                    Session("mMachine") = mMachine
                                    Session("mAssemblyStatus") = mAssemblyStatus


                                    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
                                    Session("mBoardInfo") = mBoardInfo

                                    Session("mAssemblyInfo") = ""
                                    ''Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(New Guid(value)).MachineInfo+ "->" + mSelectDueJobsForWO.Item(New Guid(value)).ModelSerialNo + "->" + mSelectDueJobsForWO.Item(New Guid(value)).Reference + "->" + mSelectDueJobsForWO.Item(New Guid(value)).MonitorInfo + "->" + mSelectDueJobsForWO.Item(New Guid(value)).MonitorType + "->" + mSelectDueJobsForWO.Item(New Guid(value)).ATA + "->" + mSelectDueJobsForWO.Item(New Guid(value)).Description
                                    Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(New Guid(value)).LogBook

                                    If SaveAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatus, mSelectDueJobsForWO.Item(New Guid(value))) = True Then

                                        LinkMaintenance(mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine, mDetail, mWO.WONumber, mAssemblyMonitorServiceStatus.AssemblyID, "Assembly Servicing", mMachineMaintenanceForAssemblyService, mSelectDueJobsForWO.Item(New Guid(value)).DoneRemark, mSelectDueJobsForWO(New Guid(value)).LicenseNo, mSelectDueJobsForWO(New Guid(value)).DoneByID.ToString, mSelectDueJobsForWO(New Guid(value)).Place)
                                        If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID) Then
                                            Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID)
                                            mWOJob.IsComplied = True
                                            mWOJob.Save()
                                        End If
                                        IsSavedSuccessfully = True
                                    Else
                                        'If strMSG <> "" Then
                                        '    Session("strMSG") = strMSG
                                        '    cvControlValidator.ErrorMessage = strMSG
                                        '    cvControlValidator.IsValid = False
                                        'End If
                                        IsSavedSuccessfully = False
                                    End If
                                    Session("MaintenanceActivityTypeID") = 5
                                End If
                            Case "Inspection" 'Inspection

                                Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
                                Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mSelectDueJobsForWO.Item(New Guid(value)).ID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, mMachine.HourType)
                                If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    If CType(Session("FromLog"), Boolean) = True Then
                                        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDateWO, mSelectDueJobsForWO(New Guid(value)).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, New Guid(LogIdWO), mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                                    Else
                                        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDateWO, mSelectDueJobsForWO(New Guid(value)).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                                    End If

                                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                                    Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
                                    Session("From") = 0 'New record
                                    ''
                                    mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
                                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

                                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(New Guid(value)).AssemblyStatusID)
                                    Session("mMachine") = mMachine
                                    Session("mAssemblyStatus") = mAssemblyStatus


                                    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
                                    Session("mBoardInfo") = mBoardInfo

                                    Session("mAssemblyInfo") = ""
                                    ''Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(New Guid(value)).MachineInfo+ "->" + mSelectDueJobsForWO.Item(New Guid(value)).ModelSerialNo + "->" + mSelectDueJobsForWO.Item(New Guid(value)).Reference + "->" + mSelectDueJobsForWO.Item(New Guid(value)).MonitorInfo + "->" + mSelectDueJobsForWO.Item(New Guid(value)).MonitorType + "->" + mSelectDueJobsForWO.Item(New Guid(value)).ATA + "->" + mSelectDueJobsForWO.Item(New Guid(value)).Description
                                    Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(New Guid(value)).LogBook

                                    If SaveAssemblyMonitorInspStatus(mAssemblyMonitorInspStatus, mSelectDueJobsForWO.Item(New Guid(value))) = True Then
                                        LinkMaintenance(mAssemblyMonitorInspStatus.ModelMonitorInspID, mMachine, mDetail, mWO.WONumber, mAssemblyMonitorInspStatus.AssemblyID, "Assembly Inspection", mMachineMaintenanceForAssemblyInsp, mSelectDueJobsForWO.Item(New Guid(value)).DoneRemark, mSelectDueJobsForWO(New Guid(value)).LicenseNo, mSelectDueJobsForWO(New Guid(value)).DoneByID.ToString, mSelectDueJobsForWO(New Guid(value)).Place)

                                        If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID) Then
                                            Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID)
                                            mWOJob.IsComplied = True
                                            mWOJob.Save()
                                        End If
                                        IsSavedSuccessfully = True
                                    Else
                                        'If strMSG <> "" Then
                                        '    Session("strMSG") = strMSG
                                        '    cvControlValidator.ErrorMessage = strMSG
                                        '    cvControlValidator.IsValid = False
                                        'End If
                                        IsSavedSuccessfully = False
                                    End If
                                    Session("MaintenanceActivityTypeID") = 6
                                End If
                            Case "Modification" 'Modification

                                Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
                                Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mSelectDueJobsForWO.Item(New Guid(value)).ID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, mMachine.HourType)
                                If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    If CType(Session("FromLog"), Boolean) = True Then
                                        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDateWO, mSelectDueJobsForWO(New Guid(value)).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, New Guid(LogIdWO), mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                                    Else
                                        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDateWO, mSelectDueJobsForWO(New Guid(value)).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                                    End If

                                    Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                                    Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
                                    Session("From") = 0 'New record
                                    ''
                                    mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
                                    Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

                                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(New Guid(value)).AssemblyStatusID)
                                    Session("mMachine") = mMachine
                                    Session("mAssemblyStatus") = mAssemblyStatus


                                    mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
                                    Session("mBoardInfo") = mBoardInfo

                                    Session("mAssemblyInfo") = ""
                                    ''Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(New Guid(value)).MachineInfo+ "->" + mSelectDueJobsForWO.Item(New Guid(value)).ModelSerialNo + "->" + mSelectDueJobsForWO.Item(New Guid(value)).Reference + "->" + mSelectDueJobsForWO.Item(New Guid(value)).MonitorInfo + "->" + mSelectDueJobsForWO.Item(New Guid(value)).MonitorType + "->" + mSelectDueJobsForWO.Item(New Guid(value)).ATA + "->" + mSelectDueJobsForWO.Item(New Guid(value)).Description
                                    Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(New Guid(value)).LogBook


                                    If SaveAssemblyMonitorModStatus(mAssemblyMonitorModStatus, mSelectDueJobsForWO.Item(New Guid(value))) = True Then

                                        LinkMaintenance(mAssemblyMonitorModStatus.ModelMonitorModID, mMachine, mDetail, mWO.WONumber, mAssemblyMonitorModStatus.AssemblyID, "Assembly Directives", mMachineMaintenanceForAssemblyMod, mSelectDueJobsForWO.Item(New Guid(value)).DoneRemark, mSelectDueJobsForWO(New Guid(value)).LicenseNo, mSelectDueJobsForWO(New Guid(value)).DoneByID.ToString, mSelectDueJobsForWO(New Guid(value)).Place)
                                        If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID) Then
                                            Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID)
                                            mWOJob.IsComplied = True
                                            mWOJob.Save()
                                        End If
                                        IsSavedSuccessfully = True
                                    Else
                                        IsSavedSuccessfully = False
                                    End If
                                    Session("MaintenanceActivityTypeID") = 7
                                End If
                        End Select
                    ElseIf mSelectDueJobsForWO(New Guid(value)).OnAssemblyOrComponent = "Component" Then
                        Select Case mSelectDueJobsForWO(New Guid(value)).DataType
                            Case "Servicing"

                                Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
                                Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mSelectDueJobsForWO.Item(New Guid(value)).ID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, mSelectDueJobsForWO.Item(New Guid(value)).CompStatusID, mMachine.HourType)
                                If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    If CType(Session("FromLog"), Boolean) = True Then
                                        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDateWO, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, New Guid(LogIdWO), mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
                                    Else
                                        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDateWO, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
                                    End If

                                    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                                    Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
                                    Session("From") = 0 'NewRecord

                                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(New Guid(value)).AssemblyStatusID)
                                    Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(New Guid(value)).CompStatusID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, AsonDateWO)
                                    Session("mMachine") = mMachine
                                    Session("mCompStatus") = mCompStatus
                                    Session("mAssemblyStatus") = mAssemblyStatus
                                    mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
                                    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

                                    Session("mCompInfo") = ""
                                    Session("mCompInfo") = mSelectDueJobsForWO.Item(New Guid(value)).LogBook

                                    If SaveCompMonitorServiceStatus(mCompMonitorServiceStatus, mSelectDueJobsForWO.Item(New Guid(value))) = True Then
                                        If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID) Then
                                            Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID)
                                            mWOJob.IsComplied = True
                                            mWOJob.Save()
                                        End If
                                        IsSavedSuccessfully = True
                                    Else
                                        IsSavedSuccessfully = False
                                    End If
                                    Session("MaintenanceActivityTypeID") = 8
                                End If
                            Case "Inspection" 'Inspection

                                Dim mCompMonitorInspStatus As CompMonitorInspStatus
                                Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mSelectDueJobsForWO.Item(New Guid(value)).ID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, mSelectDueJobsForWO.Item(New Guid(value)).CompStatusID, mMachine.HourType)
                                If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    If CType(Session("FromLog"), Boolean) = True Then
                                        mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDateWO, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, New Guid(LogIdWO), mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                                    Else
                                        mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDateWO, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                                    End If

                                    Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                                    Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
                                    Session("From") = 0 'NewRecord

                                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(New Guid(value)).AssemblyStatusID)
                                    Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(New Guid(value)).CompStatusID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, AsonDateWO)
                                    Session("mMachine") = mMachine
                                    Session("mCompStatus") = mCompStatus
                                    Session("mAssemblyStatus") = mAssemblyStatus
                                    mCompMonitorInspStatus.RequiredManHours = mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours
                                    Session("mCompMonitorInspStatus") = mCompMonitorInspStatus

                                    Session("mCompInfo") = ""
                                    Session("mCompInfo") = mSelectDueJobsForWO.Item(New Guid(value)).LogBook

                                    If SaveCompMonitorInspStatus(mCompMonitorInspStatus, mSelectDueJobsForWO.Item(New Guid(value))) = True Then
                                        If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID) Then
                                            Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID)
                                            mWOJob.IsComplied = True
                                            mWOJob.Save()
                                        End If
                                        IsSavedSuccessfully = True
                                    Else
                                        IsSavedSuccessfully = False
                                    End If
                                    Session("MaintenanceActivityTypeID") = 8
                                End If
                            Case "Modification" 'Modification

                                Dim mCompMonitorModStatus As CompMonitorModStatus
                                Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mSelectDueJobsForWO.Item(New Guid(value)).ID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, mSelectDueJobsForWO.Item(New Guid(value)).CompStatusID, mMachine.HourType)
                                If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    If CType(Session("FromLog"), Boolean) = True Then
                                        mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDateWO, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, New Guid(LogIdWO), mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                                    Else
                                        mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDateWO, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                                    End If

                                    Session("mCompMonitorModStatus") = mCompMonitorModStatus
                                    Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
                                    Session("From") = 0 'NewRecord

                                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(New Guid(value)).AssemblyStatusID)
                                    Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(New Guid(value)).CompStatusID, mSelectDueJobsForWO.Item(New Guid(value)).AssemblyStatusID, AsonDateWO)
                                    Session("mMachine") = mMachine
                                    Session("mCompStatus") = mCompStatus
                                    Session("mAssemblyStatus") = mAssemblyStatus
                                    mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
                                    Session("mCompMonitorModStatus") = mCompMonitorModStatus

                                    Session("mCompInfo") = ""
                                    Session("mCompInfo") = mSelectDueJobsForWO.Item(New Guid(value)).LogBook

                                    If SaveCompMonitorModStatus(mCompMonitorModStatus, mSelectDueJobsForWO.Item(New Guid(value))) = True Then
                                        If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID) Then
                                            Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(New Guid(value)).WOJobID)
                                            mWOJob.IsComplied = True
                                            mWOJob.Save()
                                        End If
                                        IsSavedSuccessfully = True
                                    Else
                                        IsSavedSuccessfully = False
                                    End If
                                    Session("MaintenanceActivityTypeID") = 8
                                End If
                        End Select
                    End If
                End If
            Next
        End If


        If Not mWO Is Nothing Then
            mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Text, mDueLimits, mWO.MachineID.ToString, 0, mWO)

            lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."

            dgDueJob.DataSource = mSelectDueJobsForWO
            Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
            Session("mWO") = mWO
            dgDueJob.DataBind()

            'tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList
            tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mWO.MachineID,
                                                                             Guid.Empty.ToString, AssemblyType:="Airframe",
                                                                             IsAssemblyInstalled:=True,
                                                                             CurrentDate:=txtAsOnDate.Text.ToString)
            AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

            dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
            dgDoneOnValuesWO.DataBind()

            If IsSavedSuccessfully = True Then
                MSGBoxCtrl.show("Successfull!!", "Multiple Compliances has been done successfully!", "", MsgBoxStyle.OkOnly, "Successfull")
            Else
                MSGBoxCtrl.show("Failed!!", "Multiple Compliances has been failed!", "Please verify again", MsgBoxStyle.OkOnly, "")
            End If

        End If
        SetWOGrid()
        ControlVisibilityWO()
        upnlbuttonsSave.Update()
        upnlButtonsSaveTop.Update()
        upnlWOGrid.Update()
        upnlComplianceValues.Update()
        upnlResult.Update()
    End Sub

    ''Private Sub txtWOAsOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWOAsOnDate.TextChanged
    ''    If cmbWOList.SelectedIndex > 0 Then
    ''        mWO = FlyPal22.Maintain.WO.GetWO(New Guid(cmbWOList.SelectedValue))
    ''        mSelectDueJobsForWO = SelectDueJobsForWO.GetSelectDueJobsForWO(txtAsOnDate.Text, mDueLimits, mWO.MachineID.ToString, 0, mWO, chkShowAll.Checked)

    ''        dgDueJob.DataSource = mSelectDueJobsForWO
    ''        Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
    ''        Session("mWO") = mWO
    ''        dgDueJob.DataBind()

    ''        Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
    ''        AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
    ''        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

    ''        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
    ''        dgDoneOnValue.DataBind()
    ''    End If
    ''End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            mDueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))
            mWOListForCombo = nWOListForCombo.GetnWOListForComplaince(, , , , , , , txtSearch.Text)
            dgWOList.DataSource = mWOListForCombo
            dgWOList.DataBind()
            Session("mWOListForCombo") = mWOListForCombo
            If Not mWO Is Nothing Then
                mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtWOAsOnDate.Text, mDueLimits, mWO.MachineID.ToString, 0, mWO)
                If mSelectDueJobsForWO.Count = 0 Then
                    ''Dim msg1 As New SIMsgBox(Page, "Monitoring Services / Inspections / Directives not available", "<BR><BR> All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OKOnly)
                    ''msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    ''msg1.Show()
                    MSGBoxCtrl.show("Jobs not available!", "All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OkOnly, "")
                    dgDueJob.DataSource = mSelectDueJobsForWO
                    Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
                    Session("mWO") = mWO
                    dgDueJob.DataBind()
                    Exit Sub
                End If
                dgDueJob.DataSource = mSelectDueJobsForWO
                Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
                Session("mWO") = mWO
                dgDueJob.DataBind()

                'tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWOAsOnDate.Text,
                '                                                                         mWO.MachineID.ToString, , , , , , , , , ,
                '                                                                         True, , , ,
                '                                                                         "Airframe", SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList

                tmpAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mWO.MachineID,
                                                                             Guid.Empty.ToString,
                                                                             IsAssemblyInstalled:=True, AssemblyType:="Airframe",
                                                                             CurrentDate:=txtWOAsOnDate.Text.ToString)
                AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
                Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

                dgDoneOnValuesWO.DataSource = AssemblyStatusPeriodList
                dgDoneOnValuesWO.DataBind()
                If mSelectDueJobsForWO.Count > 0 Then
                    btnSave.Enabled = True
                    If mSelectDueJobsForWO.Count > 10 Then btnSaveTop.Visible = True
                    If mSelectDueJobsForWO.Count > 10 Then btnCloseTop.Visible = True

                Else
                    btnSave.Enabled = False
                End If
                lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
            Else
                mSelectDueJobsForWO = Nothing
                dgDueJob.DataBind()
                btnSave.Enabled = False
                lblResult.Text = "List of Due Jobs as per selected criteria : " & "0 Record(s) found."
            End If
        End If
        SetWOGrid()
        ControlVisibilityWO()
        upnlbuttonsSave.Update()
        upnlButtonsSaveTop.Update()
        upnlWOGrid.Update()
        upnlComplianceValues.Update()
        upnlResult.Update()
    End Sub

    Private Sub dgDueJob_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDueJob.RowDataBound
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
            mSelectDueJobsForWO = Session("mSelectDueJobsForWO")
            Dim mMachine As Machine = Machine.GetMachine(mSelectDueJobsForWO(ID).MachineID)

            Dim grdLinkActivity As GridView = DirectCast(e.Row.FindControl("grdLinkActivity"), GridView)

            If mSelectDueJobsForWO(ID).OnAssemblyOrComponent = "Assembly" Then
                Select Case mSelectDueJobsForWO(ID).DataType
                    Case "Servicing" '5. Assembly Service
                        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mSelectDueJobsForWO.Item(ID).ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mMachine.HourType)
                        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorServiceStatus.ModelMonitorServiceID.ToString)

                    Case "Inspection"    '6. Assembly Inspection
                        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mSelectDueJobsForWO.Item(ID).ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mMachine.HourType)
                        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorInspStatus.ModelMonitorInspID.ToString)

                    Case "Modification"    '7. Assembly Directive
                        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mSelectDueJobsForWO.Item(ID).ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mMachine.HourType)
                        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorModStatus.ModelMonitorModID.ToString)
                End Select
            ElseIf mSelectDueJobsForWO(ID).OnAssemblyOrComponent = "Component" Then
                Select Case mSelectDueJobsForWO(ID).DataType
                    Case "Servicing"  '8. Comp Service
                        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mSelectDueJobsForWO.Item(ID).ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mSelectDueJobsForWO.Item(ID).CompStatusID, mMachine.HourType)
                        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorServiceStatus.PartMonitorServiceID.ToString)

                    Case "Inspection"   '9. Component Inspection
                        Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mSelectDueJobsForWO.Item(ID).ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mSelectDueJobsForWO.Item(ID).CompStatusID, mMachine.HourType)
                        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorInspStatus.PartMonitorInspID.ToString)

                    Case "Modification"    '10. Component Directive
                        Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mSelectDueJobsForWO.Item(ID).ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mSelectDueJobsForWO.Item(ID).CompStatusID, mMachine.HourType)
                        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorModStatus.PartMonitorModID.ToString)

                End Select
            End If

            If mLinkMaintenanceList.Count > 0 Then
                e.Row.Cells(1).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
            End If

            grdLinkActivity.DataSource = mLinkMaintenanceList
            grdLinkActivity.DataBind()

        End If
    End Sub

#End Region
#End Region

End Class