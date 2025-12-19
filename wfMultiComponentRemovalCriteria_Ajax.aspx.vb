'Created By: Saylee
'Date      : 16-Aug-2016

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Imports System
Imports System.IO
Imports System.Linq


Public Class wfMultiComponentRemovalCriteria_Ajax
    Inherits System.Web.UI.Page
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
    Public AssemblyId As String
    Private AssemblyType As String
    Private DueType As Integer
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

    Public mMachineNameValueList As MachineNameValueList 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Dim IsReadOnly As Boolean = False  'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
    Private checkedIds As New List(Of String)()

    Public mtmpInstalledCompList As tmpInstalledCompList
    Public mRemovalReasonListForMultiComponenet As RemovalReasonList
    Public mAssemblylist As AssemblyList
    Dim LicenseNo As String = String.Empty
    Dim EmployeeName As String = String.Empty
    Dim EmployeeID As String
    Dim DoneByID As Guid = Guid.Empty
    Shared UserNameForLicenceList As String
    Dim mMaintenanceID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineListForCompliance"), MachineList)

        AOnDate = Session("AOnDate")
        Type = Session("Type")
        DueType = Session("DueType")

        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AsonDate = Session("AsonDate")
        MachineName = Session("AircraftId")
        AssemblyName = Session("AssemblyId")
        AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mtmpInstalledCompList = CType(Session("mtmpInstalledCompList"), tmpInstalledCompList)
        mMaintenanceID = Session("mMaintenanceID")
        mRemovalReasonListForMultiComponenet = Session("mRemovalReasonListForMultiComponenet")
    End Sub
    Private Sub SetSession()
        Session("mMachineListForCompliance") = mMachineList
        Session("AOnDate") = AOnDate
        Session("Type") = Type
        Session("DueType") = DueType
        'Added by Saylee on 12-Feb-2009
        Session("mAssemblyStatusList") = mAssemblyStatusList
        Session("mAssemblylist") = mAssemblylist
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        Session("mRemovalReasonListForMultiComponenet") = mRemovalReasonListForMultiComponenet
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
            Session.Remove("mtmpInstalledCompList")
            Session.Remove("mAssemblylist")

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
                    Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(0), MachineInfo).AssemblyStatusList
                    AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
                    Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                    tmpAssemblyStatusList = Nothing
                End If

            Else
                '  AssemblyType = mAssemblyStatusList(New Guid(cmbAssembly.SelectedValue.ToString)).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , cmbAssembly.SelectedValue.ToString, , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(New Guid(cmbAircraft.SelectedValue)), MachineInfo).AssemblyStatusList

                AssemblyStatusID = (tmpAssemblyStatusList(1).ID).ToString
                ModelID = (tmpAssemblyStatusList(1).ModelID).ToString
                Session("ModelName") = (tmpAssemblyStatusList(1).Model).ToString
                Session("SerialNo") = (tmpAssemblyStatusList(1).SerialNo).ToString

                If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then
                    'do nothing
                Else
                    AssemblyStatusPeriodList = mAssemblyStatusList(1).AssemblyStatusPeriodList
                    Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                End If

            End If
            Session("Assembly1") = Assembly1
            'dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            'dgDoneOnValue.DataBind()

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

            LogId = CType(Session("LogId"), String)
            Session("LogId") = CType(Session("LogId"), String)

            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog


            If Not LogId.Equals(Guid.Empty) Then
                Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId.ToString, SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(0), MachineInfo).AssemblyStatusList
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
        Dim mloglist As LogList
        mloglist = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), , AsonDate)

        Dim x As String
        If mloglist.Count > 0 Then
            x = mloglist(0).LogDate.ToShortDateString
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
                    SetFocus(cmbAircraft)
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
    Private Function RemoveCompStatus(CompStatusID As Guid, ReasonID As Guid, RemovalReasonName As String, AssemblyStatusID As Guid, _
                                      LicenceNo As String, EmployeeID As String, EmployeeName As String, Optional ByVal WorkOrderNo As String = "") As Boolean
        'WorkOrderNo Added by Prashant 22-Jul-2020 All22072020
        Dim mCompStatus As CompStatus
        If CType(Session("FromLog"), Boolean) = True Then
            mCompStatus = CompStatus.NewRemovalCompStatus(mtmpInstalledCompList(CompStatusID).CompStatusID, txtAsOnDate.Text, mtmpInstalledCompList(CompStatusID).AssemblyStatusID, LogID:=CType(Session("LogID"), String))
        Else
            mCompStatus = CompStatus.NewRemovalCompStatus(mtmpInstalledCompList(CompStatusID).CompStatusID, txtAsOnDate.Text, mtmpInstalledCompList(CompStatusID).AssemblyStatusID, Guid.Empty.ToString)
        End If

        SetObject(mCompStatus, ReasonID, RemovalReasonName, LicenceNo, EmployeeID, EmployeeName, WorkOrderNo)
        mCompStatus = Session("mCompStatus")
        If mCompStatus.IsValid = True And mCompStatus.IsDirty = True Then
            Try
                mCompStatus.ApplyEdit()
                mCompStatus = CType(mCompStatus.Save(), CompStatus)
                SaveMachineMaintenance(mCompStatus, AssemblyStatusID)
                Session("mCompStatus") = mCompStatus
                Return True
            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            End Try


            Session.Remove("mCompStatus")
        Else
            UpnlInstalledCompList.Update()
            upnlValidationSummary.Update()
        End If
    End Function
    Private Sub SetObject(mCompStatus As CompStatus, ReasonID As Guid, RemovalReasonName As String, LicenceNo As String, EmployeeID As String, _
                          EmployeeName As String, Optional ByVal WorkOrderNo As String = "")
        With mCompStatus
            .RemovalReasonID = ReasonID
            .RemovalReasonName = RemovalReasonName
            '.Comp.SerialNo = Trim(txtSerialNo.Text)
            '.Position = Trim(txtPosition.Text)

            If txtAsOnDate.Text = "" Then
                .RemovedOn = System.DBNull.Value
            Else
                .RemovedOn = txtAsOnDate.Text
            End If

            '.RemovalWONo = Trim(txtWorkOrderNo.Text)
            .RemovalWONo = WorkOrderNo

            .RemPlace = txtPlace.Text.Trim

            'Dim LicenseNo As String = String.Empty
            'Dim EmpName As String = String.Empty
            'If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            '    LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            '    EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            'Else
            '    LicenseNo = Trim(txtLicenceNo.Text)
            'End If
            '.RemLicenseNo = LicenseNo
            '.RemDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            ''End


            Dim Licenses() As String
            Dim EmpID() As String
            Dim EmpName() As String
            If LicenceNo <> "" Then
                If .MaintenanceDoneByEmployees.Count > 0 Then
                    .MaintenanceDoneByEmployees.Remove(mCompStatus.ID)
                End If

                Licenses = LicenceNo.Split(",")
                EmpID = EmployeeID.Split(",")
                EmpName = EmployeeName.Split(",")

                For i As Integer = 0 To EmpID.Length - 1
                    .MaintenanceDoneByEmployees.Add(mCompStatus.ID, 4, Guid.Empty, Licenses(i), "", EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                Next
            End If
        End With
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub SaveMachineMaintenance(mCompStatus As CompStatus, AssemblyStatusID As Guid)
        Dim mMachineMaintenance As MachineMaintenance
        mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompStatus.ID, 4)

        If Not mMachineMaintenance Is Nothing Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(New Guid(cmbAircraft.SelectedValue.ToString), 4, txtAsOnDate.Text, mCompStatus.ID, Guid.Empty, 0, 0, AssemblyStatusID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompStatus.ID, 4)
            Session("mMachineMaintenance") = mMachineMaintenance
        End If

        With mMachineMaintenance
            .MachineID = New Guid(cmbAircraft.SelectedValue.ToString)
            .MaintenanceActivityTypeID = 4
            .MaintenanceID = mCompStatus.ID 'TransactionID
            .AssemblyStatusID = AssemblyStatusID

            .Date = txtAsOnDate.Text

            Dim mLog As Log = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                'Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text, New Guid(cmbAircraft.SelectedValue.ToString), mCompStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If
        End With

        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub FillGrid(Optional ByVal LogID As String = "{00000000-0000-0000-0000-000000000000}")
        mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtAsOnDate.Text, cmbAircraft.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue), LogID:=LogID) 'LogID Added By Vikrant On 10-Nov-2020 to solve issue,grid values were not changing on selection of log
        dgInstalledList.DataSource = mtmpInstalledCompList
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        dgInstalledList.DataBind()

        lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mtmpInstalledCompList.Count & " Record(s) found."
        upnlRemoveEntry.Visible = True
        upnlRemoveEntry.Update()

        UpnlInstalledCompList.Update()
        lblSelection.Text = "0"
        lblSelection.DataBind()
        imgChecked.Visible = True
        upnlCheckedSelection.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)

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
    Public Function CustomValidate2() As Boolean

        Dim str As String = String.Empty
        If cmbAircraft.SelectedIndex <= 0 Then
            str = "Aircraft Required"
        End If
        If str <> "" Then
            cvValidator.ErrorMessage = str
            cvValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Sub DataFieldBind()
        If CType(Session("OpenFindNowSelectLogForm"), Boolean) = True Then
            If Not IsNothing(MachineName) Or Not MachineName = Guid.Empty.ToString Then
                cmbAircraft.SelectedValue = MachineName
                mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue.ToString, txtAsOnDate.Text, "(All)", True)
                cmbAssembly.DataSource = mAssemblylist
                Session("mAssemblyList") = mAssemblylist
                cmbAssembly.DataBind()

                If IsNothing(AssemblyId) Then AssemblyId = Guid.Empty.ToString Else AssemblyId = AssemblyId

                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtAsOnDate.Text, cmbAircraft.SelectedValue.ToString, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(AssemblyId))
                dgInstalledList.DataSource = mtmpInstalledCompList
                Session("mtmpInstalledCompList") = mtmpInstalledCompList

                lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mtmpInstalledCompList.Count & " Record(s) found."
                upnlRemoveEntry.Visible = True
                upnlRemoveEntry.Update()

                UpnlInstalledCompList.Update()
                lblSelection.Text = "0"
                lblSelection.DataBind()
                imgChecked.Visible = True
                upnlCheckedSelection.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)

            End If
            If Not IsNothing(AssemblyName) Then
                If (Not New Guid(AssemblyName).Equals(Guid.Empty)) Then cmbAssembly.SelectedValue = AssemblyName
            End If
            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

            txtAsOnDate.Text = AsonDate
        End If

        mRemovalReasonListForMultiComponenet = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        Session("mRemovalReasonListForMultiComponenet") = mRemovalReasonListForMultiComponenet

        DataBind()
        'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        'Disable AddNew buttons if Aircraft is ReadOnly
        If IsReadOnly = True Then
            btnRemove.Enabled = False
            lblReadOnly.Visible = True
        Else
            btnRemove.Enabled = True
            lblReadOnly.Visible = False
        End If
        'mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        'Session("mRemovalReasonList") = mRemovalReasonList
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
    'Protected Sub chkSelect_Change(sender As Object, e As System.EventArgs)
    '    Dim builder = New StringBuilder()
    '    Dim checkString = Request.Form("chkSelect")
    '    If Not checkString Is Nothing Then
    '        Dim values = checkString.Split(","c)
    '        For Each value As String In values
    '            builder.Append("<br/>")
    '            builder.Append(value)
    '            checkedIds.Add(value)
    '        Next
    '        lblSelection.Text = checkedIds.Count.ToString
    '        If checkedIds.Count > 0 Then
    '            btnRemove.Visible = True
    '            imgChecked.Visible = True
    '        Else
    '            btnRemove.Visible = False
    '            imgChecked.Visible = False
    '        End If

    '        upnlCheckedSelection.Update()
    '        lblSelection.DataBind()
    '        'imgChecked.DataBind()
    '    Else
    '        lblSelection.Text = "0"
    '        lblSelection.DataBind()
    '        imgChecked.Visible = True
    '        upnlCheckedSelection.Update()
    '    End If
    'End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyStatusList = Nothing
        AssemblyStatusPeriodList = Nothing
        Session("mMultiComplianceList") = Nothing
        Session("MiddleFrame") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentCallback", "CallParentCallback()", True)
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

            dgDoneOnValue.DataSource = Nothing
            dgDoneOnValue.DataBind()
            upnlValues.Update()


            dgInstalledList.DataSource = Nothing
            dgInstalledList.DataBind()

            lblInstalledComponents.Text = ""
            upnlRemoveEntry.Visible = False
            upnlRemoveEntry.Update()

            UpnlInstalledCompList.Update()
            lblSelection.Text = "0"
            lblSelection.DataBind()
            imgChecked.Visible = False
            upnlCheckedSelection.Update()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OnDateChange", "OnDateChange()", True)
        End If
    End Sub

    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged

        '   If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            cmbAssembly.SelectedIndex = 0
            dgInstalledList.DataSource = Nothing
            dgInstalledList.DataBind()

            dgDoneOnValue.DataSource = Nothing
            dgDoneOnValue.DataBind()
            upnlValues.Update()

            lblInstalledComponents.Text = ""
            upnlRemoveEntry.Visible = False
            upnlRemoveEntry.Update()

            UpnlInstalledCompList.Update()
            lblSelection.Text = "0"
            lblSelection.DataBind()
            imgChecked.Visible = False
            upnlCheckedSelection.Update()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OnDateChange", "OnDateChange()", True)
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtAsOnDate.Text, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()

            cmbAssembly.SelectedValue = mAssemblylist(1).ID.ToString
            Session("mAssemblylist") = mAssemblylist

            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , cmbAssembly.SelectedValue.ToString, , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(New Guid(cmbAircraft.SelectedValue)), MachineInfo).AssemblyStatusList
            Session("mAssemblyStatusList") = mAssemblyStatusList

            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(0), MachineInfo).AssemblyStatusList
            AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

            FillGrid()

            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
            Session("IsReadOnly") = IsReadOnly
            'SetValues\
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)
        End If
        Session.Remove("OpenFindNowSelectLogForm")
        If cmbAircraft.Enabled = True Then
            SetFocus(cmbAircraft)
        End If
        DataFieldBind()
        upnlSearchCriteria.Update()
        upnlValues.Update()

    End Sub

    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        If cmbAssembly.SelectedIndex = 0 Then
            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(0), MachineInfo).AssemblyStatusList
            AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            tmpAssemblyStatusList = Nothing
        Else
            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , cmbAssembly.SelectedValue.ToString, , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(New Guid(cmbAircraft.SelectedValue)), MachineInfo).AssemblyStatusList
            Session("mAssemblyStatusList") = mAssemblyStatusList
            AssemblyStatusPeriodList = mAssemblyStatusList(1).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
        End If
        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
        dgDoneOnValue.DataBind()
        upnlValues.Update()

        FillGrid()
    End Sub
    Private Sub txtPart_TextChanged(sender As Object, e As System.EventArgs) Handles txtPart.TextChanged, txtSerialNo.TextChanged
        '   If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() Then upnlValidationSummary.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True) : Exit Sub

        mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtAsOnDate.Text, cmbAircraft.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue))
        dgInstalledList.DataSource = mtmpInstalledCompList
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        dgInstalledList.DataBind()

        lblInstalledComponents.Text = "List of Installed components as per selected criteria : " & mtmpInstalledCompList.Count & " Record(s) found."
        upnlRemoveEntry.Visible = True
        upnlRemoveEntry.Update()

        UpnlInstalledCompList.Update()
        lblSelection.Text = "0"
        lblSelection.DataBind()
        imgChecked.Visible = True
        upnlCheckedSelection.Update()
        UpnlInstalledCompList.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)
    End Sub
    Private Function CheckValidation() As Boolean
        mRemovalReasonListForMultiComponenet = Session("mRemovalReasonListForMultiComponenet")
        Dim strError As String = String.Empty
        Dim builder = New StringBuilder()
        Dim checkString = Request.Form("chkSelect")
        If Not checkString Is Nothing Then
            Dim values = checkString.Split(","c)
            If checkedIds.Count = 0 Then
                For Each value As String In values
                    builder.Append("<br/>")
                    builder.Append(value)
                    checkedIds.Add(value)
                Next
                Session("checkString") = checkString
            End If


            For i As Integer = 0 To checkedIds.Count - 1

                'Dim cmbReason As DropDownList
                Dim txtReason As TextBox
                Dim txtLicenceNo As TextBox
                Dim cvValidator As RequiredFieldValidator
                Dim rfLicenceValidator As RequiredFieldValidator

                Dim upnlReasonValidate As UpdatePanel
                Dim upnlLicenceValidate As UpdatePanel
                Dim LicenceNo As String = String.Empty
                Dim EmployeeID As String = Guid.Empty.ToString
                Dim EmployeeName As String = String.Empty

                For j As Integer = 0 To mtmpInstalledCompList.Count - 1
                    If mtmpInstalledCompList(j).CompStatusID = New Guid(checkedIds(i)) Then

                        cvValidator = CType(Me.dgInstalledList.Rows(j).FindControl("rfReason"), RequiredFieldValidator)
                        upnlReasonValidate = CType(Me.dgInstalledList.Rows(j).FindControl("upnlReasonValidate"), UpdatePanel)

                        rfLicenceValidator = CType(Me.dgInstalledList.Rows(j).FindControl("rfLicenceValidator"), RequiredFieldValidator)
                        upnlLicenceValidate = CType(Me.dgInstalledList.Rows(j).FindControl("upnlLicenceValidate"), UpdatePanel)

                        txtReason = CType(Me.dgInstalledList.Rows(j).FindControl("txtReason"), TextBox)
                        txtLicenceNo = CType(Me.dgInstalledList.Rows(j).FindControl("txtLicenceNo"), TextBox)


                        'If cmbReason.SelectedIndex = 0 Then
                        If txtReason.Text = "" Then
                            cvValidator.IsValid = False
                            cvValidator.Text = "* Reason Required"
                            strError = "* Reason Required"
                            upnlReasonValidate.Update()
                        ElseIf Not mRemovalReasonListForMultiComponenet.Contains(txtReason.Text) Then
                            cvValidator.IsValid = False
                            cvValidator.Text = "Invalid Reason"
                            strError = "Invalid Reason"
                            upnlReasonValidate.Update()
                        End If
                        If txtLicenceNo.Text <> "" Then
                            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                                EmployeeName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                            Else
                                LicenseNo = Trim(txtLicenceNo.Text)
                            End If
                            EmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenceNo, EmployeeName).EmpID.ToString
                            If EmployeeID = Guid.Empty.ToString Then
                                rfLicenceValidator.IsValid = False
                                rfLicenceValidator.Text = "Invalid Licence No"
                                strError = "Invalid Licence No"
                                upnlLicenceValidate.Update()
                            ElseIf (Not EmployeeID.Equals(Guid.Empty)) AndAlso (txtAsOnDate.Text <> "") Then
                                Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EmployeeID.ToString, txtAsOnDate.Text)
                                If (mEmployeeStatus(0).Information <> "") Then
                                    rfLicenceValidator.IsValid = False
                                    rfLicenceValidator.Text = mEmployeeStatus(0).Information
                                    strError = mEmployeeStatus(0).Information
                                    upnlLicenceValidate.Update()
                                End If

                            End If
                        End If
                        Exit For
                    End If
                Next
            Next
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckChange", "CheckChange()", True)

            If strError <> "" Then
                Return False
            End If
        End If
        Return True
    End Function
    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If IsValid = True Then
            If CheckValidation() = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)
                Exit Sub
            End If
            SetReport()
            Session("LogId") = CType(Session("LogId"), String)
            mMachineList = Session("mMachineListForCompliance")
            Session("OpenFindNowSelectLogForm") = True
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            Session("ActiveTabIndex") = 0
            '  ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckReasonSelection", "CheckReasonSelection()", True)
            Dim mRemovalReasonListForMultiComponenet As RemovalReasonList
            mRemovalReasonListForMultiComponenet = RemovalReasonList.GetRemovalReasonList("", )
            Session("mRemovalReasonListForMultiComponenet") = mRemovalReasonListForMultiComponenet

            Dim IsRemoved As Boolean
            Dim builder = New StringBuilder()
            Dim checkString = Request.Form("chkSelect")
            'checkedIds = ""
            If Not checkString Is Nothing Then
                ' Dim values = checkString.Split(","c)
                'For Each value As String In values
                '    builder.Append("<br/>")
                '    builder.Append(value)
                '    checkedIds.Add(value)
                'Next

                For i As Integer = 0 To checkedIds.Count - 1
                    Dim ReasonID As Guid = Guid.Empty
                    Dim RemovalReasonName As String = String.Empty
                    Dim LicenceEmpNo As String = String.Empty

                    Dim txtReason As TextBox
                    Dim txtLicenceNo As TextBox
                    Dim hdnLicenceNo As HiddenField

                    Dim hdnEmployeeID As HiddenField
                    Dim hdnEmployeeName As HiddenField

                    Dim LicenceNo As String = String.Empty
                    Dim EmployeeID As String = String.Empty
                    Dim EmployeeName As String = String.Empty

                    Dim cvValidator As RequiredFieldValidator
                    Dim upnlReasonValidate As UpdatePanel
                    Dim txtWorkOrderNo As TextBox 'Added By Prashant 22-Jul-2020 All22072020
                    Dim WorkOrderNo As String = String.Empty 'Added By Prashant 22-Jul-2020 All22072020
                    For j As Integer = 0 To mtmpInstalledCompList.Count - 1
                        If mtmpInstalledCompList(j).CompStatusID = New Guid(checkedIds(i)) Then
                            cvValidator = CType(Me.dgInstalledList.Rows(j).FindControl("rfReason"), RequiredFieldValidator)
                            upnlReasonValidate = CType(Me.dgInstalledList.Rows(j).FindControl("upnlReasonValidate"), UpdatePanel)

                            txtReason = CType(Me.dgInstalledList.Rows(j).FindControl("txtReason"), TextBox)
                            hdnLicenceNo = CType(Me.dgInstalledList.Rows(j).FindControl("hdnLicenceNo"), HiddenField)
                            hdnEmployeeID = CType(Me.dgInstalledList.Rows(j).FindControl("hdnEmployeeID"), HiddenField)
                            hdnEmployeeName = CType(Me.dgInstalledList.Rows(j).FindControl("hdnEmployeeName"), HiddenField)

                            ReasonID = mRemovalReasonListForMultiComponenet(txtReason.Text).ID
                            RemovalReasonName = txtReason.Text 'cmbReason.SelectedItem.Text

                            txtWorkOrderNo = CType(Me.dgInstalledList.Rows(j).FindControl("txtWorkOrderNo"), TextBox) 'Added By Prashant 22-Jul-2020 All22072020
                            WorkOrderNo = txtWorkOrderNo.Text.Trim

                            txtLicenceNo = CType(Me.dgInstalledList.Rows(j).FindControl("txtLicenceNo"), TextBox)

                            If hdnLicenceNo.Value <> "" Then
                                LicenceNo = hdnLicenceNo.Value
                                EmployeeID = hdnEmployeeID.Value
                                EmployeeName = hdnEmployeeName.Value
                            ElseIf txtLicenceNo.Text <> "" Then
                                If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                                    LicenceNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                                    EmployeeName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                                Else
                                    LicenceNo = Trim(txtLicenceNo.Text)
                                End If
                                EmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenceNo, EmployeeName).EmpID.ToString
                            End If
                            Exit For
                        End If
                    Next
                    '  Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mtmpInstalledCompList(New Guid(checkedIds(i))).AssemblyStatusID)
                    Dim AssemblyStatusID As Guid = mtmpInstalledCompList(New Guid(checkedIds(i))).AssemblyStatusID
                    If RemovalReasonName <> "" Then
                        If RemoveCompStatus(New Guid(checkedIds(i)), ReasonID, RemovalReasonName, AssemblyStatusID, LicenceNo, EmployeeID, _
                                            EmployeeName, WorkOrderNo) Then
                            'mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtAsOnDate.Text, cmbAircraft.SelectedValue, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(cmbAssembly.SelectedValue))
                            'dgInstalledList.DataSource = mtmpInstalledCompList
                            'Session("mtmpInstalledCompList") = mtmpInstalledCompList
                            'dgInstalledList.DataBind()
                            'UpnlInstalledCompList.Update()
                            IsRemoved = True
                            'Comps successfully removed
                        Else
                            IsRemoved = False
                        End If
                    End If
                Next

                If IsRemoved = True Then
                    FillGrid()
                    'txtWorkOrderNo.Text = ""
                    txtPlace.Text = ""
                    Session.Remove("FromLog")
                    Session.Remove("mLog")
                End If
            End If
        Else
            upnlValidationSummary.Update()
            UpnlInstalledCompList.Update()
            Exit Sub
        End If
    End Sub
    Public Sub GetLicenceNos()

    End Sub
    Private Sub dgInstalledList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstalledList.RowCommand
        Dim mCompStatusID As Guid

        Select Case e.CommandName
            Case "EmployeeLicence"
                Dim mMaintenanceDoneByEmployees As MaintenanceDoneByEmployees = New MaintenanceDoneByEmployees

                Dim builder = New StringBuilder()
                builder.Append("You have selected the following checks :<br/>")
                ' get the selected checkboxes from the form data
                Dim checkString = Request.Form("chkSelect")
                If checkString Is Nothing Then
                    FillGrid()
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Dim values = checkString.Split(","c)
                If checkedIds.Count = 0 Then
                    For Each value As String In values
                        builder.Append("<br/>")
                        builder.Append(value)
                        checkedIds.Add(value)
                    Next
                    Session("checkString") = checkString
                End If


                Dim rowIndex As String = e.CommandArgument
                mCompStatusID = mtmpInstalledCompList(CInt(rowIndex)).CompStatusID  'New Guid(e.CommandArgument.ToString)
                Session("mMaintenanceID") = mCompStatusID 'mtmpInstalledCompList(mID).CompStatusID
                Session("MaintenanceDoneOnDate") = txtAsOnDate.Text.ToString

                Dim hdnLicenceNo As HiddenField
                Dim hdnEmployeeID As HiddenField
                Dim hdnEmployeeName As HiddenField
                Dim txtLicenceNo As TextBox

                hdnLicenceNo = CType(Me.dgInstalledList.Rows(CInt(rowIndex)).FindControl("hdnLicenceNo"), HiddenField)
                hdnEmployeeID = CType(Me.dgInstalledList.Rows(CInt(rowIndex)).FindControl("hdnEmployeeID"), HiddenField)
                hdnEmployeeName = CType(Me.dgInstalledList.Rows(CInt(rowIndex)).FindControl("hdnEmployeeName"), HiddenField)
                txtLicenceNo = CType(Me.dgInstalledList.Rows(CInt(rowIndex)).FindControl("txtLicenceNo"), TextBox)

                Dim Licenses() As String
                Dim EmpID() As String
                Dim EmpName() As String


                If mMaintenanceDoneByEmployees.Count > 0 Then
                    mMaintenanceDoneByEmployees.Remove(mCompStatusID)
                End If

                If hdnLicenceNo.Value <> "" Then
                    Licenses = hdnLicenceNo.Value.Split(",")
                    EmpID = hdnEmployeeID.Value.Split(",")
                    EmpName = hdnEmployeeName.Value.Split(",")

                    If txtLicenceNo.Text <> "" Then
                        For i As Integer = 0 To EmpID.Length - 1
                            mMaintenanceDoneByEmployees.Add(mCompStatusID, 4, Guid.Empty, Licenses(i), "", EmpName(i))
                            mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                        Next
                    Else
                        For i As Integer = 1 To EmpID.Length - 1 'Skip first record as txtLicenceNo is cleared
                            mMaintenanceDoneByEmployees.Add(mCompStatusID, 4, Guid.Empty, Licenses(i), "", EmpName(i))
                            mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                        Next
                    End If

                ElseIf txtLicenceNo.Text <> "" Then
                    If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                        LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                        EmployeeName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                    Else
                        LicenseNo = Trim(txtLicenceNo.Text)
                    End If
                    EmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmployeeName).EmpID.ToString
                    mMaintenanceDoneByEmployees.Add(mCompStatusID, 4, Guid.Empty, LicenseNo, "", EmployeeName)
                    mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmployeeID)
                End If

                Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)


                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetCheckBox()", "SetCheckBox('" + checkString + "');", True)

                ' Case Added on 18-Mar-2019 for New Reason 
            Case "Reason"
                Dim builder = New StringBuilder()
                builder.Append("You have selected the following checks :<br/>")
                ' get the selected checkboxes from the form data
                Dim checkString = Request.Form("chkSelect")
                If checkString Is Nothing Then
                    FillGrid()
                    'MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                    'Exit Sub
                End If
                Dim values = checkString.Split(","c)
                If checkedIds.Count = 0 Then
                    For Each value As String In values
                        builder.Append("<br/>")
                        builder.Append(value)
                        checkedIds.Add(value)
                    Next
                    Session("checkString") = checkString
                End If


                Dim rowIndex As String = e.CommandArgument
                mCompStatusID = mtmpInstalledCompList(CInt(rowIndex)).CompStatusID  'New Guid(e.CommandArgument.ToString)
                Session("mMaintenanceID") = mCompStatusID 'mtmpInstalledCompList(mID).CompStatusID
                Session("MaintenanceDoneOnDate") = txtAsOnDate.Text.ToString


                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemovalReasonWindow", "OpenRemovalReasonWindow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetCheckBox()", "SetCheckBox('" + checkString + "');", True)
        End Select
    End Sub
    'Private Sub dgInstalledList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgInstalledList.RowDataBound
    '    mRemovalReasonList = Session("mRemovalReasonList")

    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        'Find the DropDownList in the Row
    '        Dim cmbReason As DropDownList = CType(e.Row.FindControl("cmbReason"), DropDownList)
    '        cmbReason.DataSource = mRemovalReasonList
    '        cmbReason.DataTextField = "Name"
    '        cmbReason.DataValueField = "ID"
    '        cmbReason.DataBind()
    '    End If
    'End Sub
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        '''Dim builder = New StringBuilder()
        '''builder.Append("You have selected the following checks :<br/>")
        '''' get the selected checkboxes from the form data
        '''Dim checkString = Request.Form("chkSelect")
        '''If checkString Is Nothing Then
        '''    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
        '''    Exit Sub
        '''End If
        Session.Remove("FromLog")
        SetSession()
        Session("OpenFindNowSelectLogForm") = True
        SetValues()
        '' Dim mtmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , ).Item(cmbAssembly.SelectedIndex), MachineInfo).AssemblyStatusList
        If cmbAssembly.SelectedIndex = 0 Then
            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe", SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(0), MachineInfo).AssemblyStatusList
            'Dim str As String
            ' str = "openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=index.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & tmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & tmpAssemblyStatusList(0).AssemblyID.ToString & "');"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

            Session("mFromType") = 3
            Session("mMachineId") = MachineName
            Session("mAssemblyStatusId") = tmpAssemblyStatusList(0).ID.ToString
            Session("mAssemblyID") = tmpAssemblyStatusList(0).AssemblyID.ToString
            Session("mDoneOn") = CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate))
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
        Else
            'Dim str As String
            'str = "openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=index.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & mAssemblyStatusList(cmbAssembly.SelectedIndex).ID.ToString & "&AssemblyID=" & mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyID.ToString & "');"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , cmbAssembly.SelectedValue.ToString, , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(New Guid(cmbAircraft.SelectedValue)), MachineInfo).AssemblyStatusList

            Session("mFromType") = 3
            Session("mMachineId") = MachineName
            Session("mAssemblyStatusId") = tmpAssemblyStatusList(1).ID.ToString
            Session("mAssemblyID") = tmpAssemblyStatusList(1).AssemblyID.ToString
            Session("mDoneOn") = CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate))
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
        End If

    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        'Added By Vikrant On 10-Nov-2020 to solve issue
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , False, , , True)
        Session("mMachineNameValueList") = mMachineNameValueList
        'End
        LogId = CType(Session("LogID"), String)
        Dim LogDate = CType(Session("mDoneOn"), String)
        If cmbAircraft.SelectedIndex > 0 Then
            SetLog()
            FillGrid(LogId) 'Added By Vikrant On 10-Nov-2020 to solve issue
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click, hdnBtnRemovalReason.Click
        Dim mMaintenanceDoneByEmployees As MaintenanceDoneByEmployees
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        Dim checkString = Session("checkString")
        mMaintenanceID = Session("mMaintenanceID") 'mMaintenanceDoneByEmployees(0).MaintenanceID
        Session.Remove("mMaintenanceID")
        ' If mMaintenanceDoneByEmployees.Count > 0 Then

        mRemovalReasonListForMultiComponenet = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        Session("mRemovalReasonListForMultiComponenet") = mRemovalReasonListForMultiComponenet

        Dim hdnLicenceNo As HiddenField
        Dim hdnEmployeeID As HiddenField
        Dim hdnEmployeeName As HiddenField

        Dim LicenceNo As String = String.Empty
        Dim EmployeeID As String = String.Empty
        Dim EmployeeName As String = String.Empty
        Dim txtLicenceNo As TextBox
        Dim lblLicenceCount As Label

        If Not checkString Is Nothing Then

            Dim values = checkString.Split(","c)
            If checkedIds.Count = 0 Then
                For Each value As String In values
                    checkedIds.Add(value)
                Next
                mtmpInstalledCompList = Session("mtmpInstalledCompList")
                'dgInstalledList.DataSource = mtmpInstalledCompList
                'dgInstalledList.DataBind()

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)
                Session("checkString") = checkString
            End If
        End If

        For j As Integer = 0 To mtmpInstalledCompList.Count - 1
            If mtmpInstalledCompList(j).CompStatusID = mMaintenanceID Then
                If mMaintenanceDoneByEmployees Is Nothing Then Exit For
                For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
                    If LicenceNo = "" Then
                        LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                        EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID.ToString
                        EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
                    Else
                        LicenceNo = LicenceNo + "," + mMaintenanceDoneByEmployees(i).LicenceNo
                        EmployeeID = EmployeeID + "," + mMaintenanceDoneByEmployees(i).EmployeeID.ToString
                        EmployeeName = EmployeeName + "," + mMaintenanceDoneByEmployees(i).EmployeeName
                    End If

                Next

                hdnLicenceNo = CType(Me.dgInstalledList.Rows(j).FindControl("hdnLicenceNo"), HiddenField)
                hdnEmployeeID = CType(Me.dgInstalledList.Rows(j).FindControl("hdnEmployeeID"), HiddenField)
                hdnEmployeeName = CType(Me.dgInstalledList.Rows(j).FindControl("hdnEmployeeName"), HiddenField)
                txtLicenceNo = CType(Me.dgInstalledList.Rows(j).FindControl("txtLicenceNo"), TextBox)
                lblLicenceCount = CType(Me.dgInstalledList.Rows(j).FindControl("lblLicenceCount"), Label)

                hdnLicenceNo.Value = LicenceNo
                hdnEmployeeID.Value = EmployeeID
                hdnEmployeeName.Value = EmployeeName

                If mMaintenanceDoneByEmployees.Count > 0 Then
                    txtLicenceNo.Text = mMaintenanceDoneByEmployees(0).LicenceNo + " [" + mMaintenanceDoneByEmployees(0).EmployeeName + "]"
                Else
                    txtLicenceNo.Text = String.Empty
                End If
                txtLicenceNo.DataBind()

                If mMaintenanceDoneByEmployees.Count > 1 Then
                    lblLicenceCount.Text = "and " + (mMaintenanceDoneByEmployees.Count - 1).ToString + " more"
                End If
                lblLicenceCount.DataBind()
                lblLicenceCount.Visible = mMaintenanceDoneByEmployees.Count > 1

                Exit For
            End If
        Next

        UpnlInstalledCompList.Update()
        UpnldgInstalledCompList.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetCheckBox()", "SetCheckBox('" + checkString + "');", True)

    End Sub
    'End
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        Dim txtLicenceNo As TextBox
        Dim lblLicenceCount As Label
        Dim hdnLicenceNo As HiddenField
        Dim hdnEmployeeID As HiddenField
        Dim hdnEmployeeName As HiddenField
        Dim EmpName() As String
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent

        Dim Licences() As String
        Dim EmpID() As String

        txtLicenceNo = CType(currentRow.FindControl("txtLicenceNo"), TextBox)
        lblLicenceCount = CType(currentRow.FindControl("lblLicenceCount"), Label)
        hdnLicenceNo = CType(currentRow.FindControl("hdnLicenceNo"), HiddenField)
        hdnEmployeeID = CType(currentRow.FindControl("hdnEmployeeID"), HiddenField)
        hdnEmployeeName = CType(currentRow.FindControl("hdnEmployeeName"), HiddenField)


        LicenseNo = ""
        EmployeeName = ""
        EmployeeID = ""

        If txtLicenceNo.Text = "" Then 'used when record deleted by backspace in txtLicenceNo
            Licences = hdnLicenceNo.Value.Split(",")
            EmpName = hdnEmployeeName.Value.Split(",")
            EmpID = hdnEmployeeID.Value.Split(",")


            For i As Integer = 1 To Licences.Length - 1
                If LicenseNo = "" Then
                    LicenseNo = Licences(i)
                    EmployeeName = EmpName(i)
                    EmployeeID = EmpID(i)
                Else
                    LicenseNo = LicenseNo + "," + Licences(i)
                    EmployeeName = EmployeeName + "," + EmpName(i)
                    EmployeeID = EmployeeID + "," + EmpID(i)
                End If

            Next
            hdnLicenceNo.Value = LicenseNo
            hdnEmployeeName.Value = EmployeeName
            hdnEmployeeID.Value = EmployeeID

            Licences = hdnLicenceNo.Value.Split(",")
            EmpName = hdnEmployeeName.Value.Split(",")
            EmpID = hdnEmployeeID.Value.Split(",")

            If LicenseNo <> "" Then txtLicenceNo.Text = Licences(0) + " [" + EmpName(0) + "]"
            txtLicenceNo.DataBind()

            If Licences.Length > 1 Then
                lblLicenceCount.Text = "and " + (Licences.Length - 1).ToString + " more"
            End If
            lblLicenceCount.DataBind()
            lblLicenceCount.Visible = Licences.Length > 1
        End If

        UpnlInstalledCompList.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)
        Dim checkString = Request.Form("chkSelect")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetCheckBox()", "SetCheckBox('" + checkString + "');", True)
    End Sub
    Private Sub dgInstalledList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInstalledList.Sorting
        mtmpInstalledCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        dgInstalledList.DataSource = mtmpInstalledCompList
        dgInstalledList.DataBind()
        UpnlInstalledCompList.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentRemAutoResizeFunction", "CallParentRemAutoResizeFunction()", True)
    End Sub
    'Protected Sub txtReason_TextChanged(sender As Object, e As System.EventArgs)
    '    Dim txtReason As TextBox
    '    Dim hdnReason As HiddenField

    '    Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
    '    hdnReason = CType(currentRow.FindControl("hdnReason"), HiddenField)
    '    txtReason = CType(currentRow.FindControl("txtReason"), TextBox)
    '    Dim ReasonID As Guid
    '    mRemovalReasonList = Session("mRemovalReasonList")

    '    ReasonID = mRemovalReasonList(txtReason.Text).ID

    '    If Not ReasonID.Equals(Guid.Empty) Then
    '        hdnReason.Value = txtReason.Text
    '    Else
    '        txtReason.Text = ""
    '        txtReason.DataBind()
    '    End If

    ' UpnlInstalledCompList.Update()
    'End Sub
#End Region

#Region " Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
            '  SetFocus(txtAsOnDate)
            txtAsOnDate.Focus()
            DataFieldBind()
            Session("mLogList") = Nothing
            SetLog()
        End If
        SetSession()
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

#Region "WebService Methods"
    'MLNo
    <System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
    Public Shared Function GetLicenseNoList(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim list As LicenseNoListWithEmployee
        list = LicenseNoListWithEmployee.GetLicenseNoList(prefixText)

        If count = 0 Then
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In list
               Select c.LicenseNoEmpName).ToList
        Else
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In list
                   Select c.LicenseNoEmpName).Take(count).ToList
        End If

    End Function
    <System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
    Public Shared Function GetReasonList(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim mRemovalReasonList As RemovalReasonList
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList(prefixText)

        If count = 0 Then
            Return (From c As RemovalReasonList.RemovalReasonInfo In mRemovalReasonList
               Select c.Name).ToList
        Else
            Return (From c As RemovalReasonList.RemovalReasonInfo In mRemovalReasonList
                   Select c.Name).Take(count).ToList
        End If
    End Function
#End Region

   
   
End Class