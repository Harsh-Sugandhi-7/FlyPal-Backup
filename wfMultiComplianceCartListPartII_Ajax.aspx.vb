
'AJAX Created by :   Saylee
'Date            :   04-Dec-2014

Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Text
Imports System.Globalization

Imports System.Web.Services
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Imports System
Imports System.IO
Imports System.Linq

Public Class wfMultiComplianceCartListPartII_Ajax
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
    '' Private DueType As Integer
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
    Dim mLog As Log
    Public Shared mMultiComplianceList As New MultiComplianceList
    Public mBoardInfo As AircraftInformationBoard.BoardInfo
    Public mAssemblyInfo As String
    Public mCompInfo As String

    Public mMachineMaintenanceForAssemblyService As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
    Public mMachineMaintenanceListForAssemblyService As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

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
    Dim mMaintenanceOnDetail As String

    Dim IsSavedSuccessfully As Boolean = True

    Dim LicenseNo As String = String.Empty
    Dim EmployeeName As String = String.Empty
    Dim EmployeeID As String
    Dim DoneByID As Guid = Guid.Empty
    Dim ActualManHrs As String = String.Empty

    Shared UserNameForLicenceList As String
    Dim mMaintenanceID As Guid
    Dim IsCompliedOnSameDate As Boolean = False
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
        mMultiComplianceList = Session("mMultiComplianceList")

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

        mMaintenanceID = Session("mMaintenanceID")
    End Sub
    Private Sub SetSession()
        Session("mMachineListForCompliance") = mMachineList

        '' Session("DueType") = DueType
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

        Session("mMultiComplianceList") = mMultiComplianceList

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
    Private Sub RemoveSession()
        Session.Remove("mMachineMaintenanceForAssemblyService") 'Added by Saylee on 28th-Oct-2009
        Session.Remove("mMachineMaintenanceListForAssemblyService") 'Added by Saylee on 28th-Oct-2009

        Session.Remove("mMachineMaintenanceForAssemblyInsp")  'Added by Saylee on 28th-Oct-2009
        Session.Remove("mMachineMaintenanceListForAssemblyInsp")  'Added by Saylee on 28th-Oct-2009

        Session.Remove("mMachineMaintenanceForAssemblyMod") 'Added by Saylee on 28th-Oct-2009
        Session.Remove("mMachineMaintenanceListForAssemblyMod")  'Added by Saylee on 28th-Oct-2009

        Session.Remove("mMachineMaintenanceForCompService")  'Added by Saylee on 28th-Oct-2009
        Session.Remove("mMachineMaintenanceListForCompService")  'Added by Saylee on 28th-Oct-2009

        Session.Remove("mMachineMaintenanceForCompInsp")  'Added by Saylee on 28th-Oct-2009
        Session.Remove("mMachineMaintenanceListForCompInsp")  'Added by Saylee on 28th-Oct-2009

        Session.Remove("mMachineMaintenanceForCompMod") 'Added by Saylee on 28th-Oct-2009
        Session.Remove("mMachineMaintenanceListForCompMod") 'Added by Saylee on 28th-Oct-2009

        Session.Remove("mLog")


    End Sub

    Private Sub ClearAll()
        ''DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfMultiComplanceCartListPartII.aspx?" Then
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
            Session.Remove("LogId")
            Session.Remove("OpenFindNowSelectLogForm")
            Session.Remove("AssemblyStatusPeriodList")
            Session.Remove("HourType")
            Session.Remove("mLog")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Remove" Then
                        Session("Sender") = ""
                        Dim index As Integer = CType(Session("Index"), Integer)
                        mMultiComplianceList.RemoveAt(index)
                        dgMultiComplianceList.DataSource = mMultiComplianceList
                        dgMultiComplianceList.DataBind()
                        Session("mMultiComplianceList") = mMultiComplianceList
                        SetCaption()
                        Controltovisibility()
                        upnlResult.Update()
                        upnlGrid.Update()
                        upnlButtonsTop.Update()
                        upnlButtons.Update()

                        'Response.Redirect("wfMultiComplianceCartListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    DataFieldBind()
                    SetCaption()

                    'Response.Redirect("wfMultiComplianceCartListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    DataFieldBind()
                    SetCaption()
                    If MSGBoxCtrl.Sender = "Successfull" Then
                        Session.Remove("mMultiComplianceList")
                        mMultiComplianceList = Nothing
                        RemoveSession()
                        Response.Redirect(Request.QueryString("BackPage") & "?ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                    End If
                    'Response.Redirect("wfMultiComplianceCartListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfMultiComplianceCartListPartII.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        End If
    End Sub
    Private Sub Controltovisibility()
        If mMultiComplianceList.Count > 0 Then
            btnSaveTop.Enabled = True
            btnSave.Enabled = True
        Else
            btnSaveTop.Enabled = False
            btnSave.Enabled = False
        End If

        btnAddMoreTop.Visible = mMultiComplianceList.Count > 8
        btnSaveTop.Visible = mMultiComplianceList.Count > 8
        btnCloseTop.Visible = mMultiComplianceList.Count > 8


    End Sub
    Private Sub SetCompObject(ByVal mCompStatus As CompStatus, ByVal mMultiCompliance As MultiCompliance, LicenceNo As String, EmployeeID As String, EmployeeName As String)
        With mCompStatus
            Select Case mMultiCompliance.MaintenanceActivity
                Case MaintenanceActivityTypes.RemovalComp
                    .RemovalReasonID = mMultiCompliance.RemovalReasonID
                    .RemovalReasonName = mMultiCompliance.RemovalReasonName
                    .RemovedOn = mMultiCompliance.RemovedOn
                    .RemovalWONo = mMultiCompliance.DoneOnWONo
                    .RemovalRemark = mMultiCompliance.DoneRemark
                    If mCompStatus.IsExpiredEnabled Then
                        .IsExpired = mMultiCompliance.IsExpired
                    End If
                    .RemDoneBy = mMultiCompliance.DoneByAgency
                Case MaintenanceActivityTypes.InstallComp
                    .InstalledOn = mMultiCompliance.InstalledOn
                    .InstallationWONo = mMultiCompliance.DoneOnWONo
                    .InstallationRemark = mMultiCompliance.DoneRemark
                    .InstDoneBy = mMultiCompliance.DoneByAgency
            End Select

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
                    .MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMultiCompliance.MaintenanceActivity, Guid.Empty, Licenses(i), "", EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                Next
            End If
        End With
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub SaveComp(ByVal mCompStatus As CompStatus, ByVal mMultiCompliance As MultiCompliance, Optional ByVal Type As String = "", Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "")
        SetCompObject(mCompStatus, mMultiCompliance, LicenceNo, EmployeeID, EmployeeName)
        If mCompStatus.IsValid = True Then
            Try
                mCompStatus.ApplyEdit()
                mCompStatus = CType(mCompStatus.Save(), CompStatus)
                Session("mCompStatus") = mCompStatus
                mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString + "Description : " + mMultiCompliance.Description + " ATA Chapter : " + mMultiCompliance.ATAChapter + IIf(mMultiCompliance.DirectiveNumber <> "", " Directive No. : " + mMultiCompliance.DirectiveNumber, "")
                MarkLog(Util.Action.Save, Type, mMaintenanceOnDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)

            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Finally

            End Try

        End If
    End Sub
    Private Sub SetAssemblyMonitorServiceStatusObject(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mMultiCompliance As MultiCompliance, LicenceNo As String, EmployeeID As String, EmployeeName As String, DoneRemark As String, ActualManHrsPar As String)
        mAssemblyMonitorServiceStatus.DoneRemark = DoneRemark
        mAssemblyMonitorServiceStatus.DoneWONo = mMultiCompliance.DoneOnWONo

        mAssemblyMonitorServiceStatus.Place = mMultiCompliance.Place
        ' mAssemblyMonitorServiceStatus.LicenseNo = mMultiCompliance.LicenseNo


        With mAssemblyMonitorServiceStatus
            Dim Licenses() As String
            Dim EmpID() As String
            Dim EmpName() As String
            Dim ActManHrsArray() As String

            If LicenceNo <> "" Then
                If .MaintenanceDoneByEmployees.Count > 0 Then
                    .MaintenanceDoneByEmployees.Remove(mAssemblyMonitorServiceStatus.ID)
                End If

                Licenses = LicenceNo.Split(",")
                EmpID = EmployeeID.Split(",")
                EmpName = EmployeeName.Split(",")
                ActManHrsArray = ActualManHrsPar.Split(",")


                For i As Integer = 0 To EmpID.Length - 1
                    .MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatus.ID, mMultiCompliance.MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrsArray(i), EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                    .MaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrsArray(i)
                Next

                mAssemblyMonitorServiceStatus.LicenseNo = Licenses(0)
                mAssemblyMonitorServiceStatus.DoneByID = New Guid(EmpID(0))
                mAssemblyMonitorServiceStatus.RequiredManHours = ActManHrsArray(0)
            End If
        End With

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForAssemblyService.Contains(mAssemblyMonitorServiceStatus.ID, 5, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForAssemblyService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 5, txtAsOnDate.Text.ToString, mAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorServiceStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForAssemblyService = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatus.ID, 5)
        End If

        With mMachineMaintenanceForAssemblyService
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mAssemblyMonitorServiceStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtAsOnDate.Text
            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text.ToString, New Guid(MachineName), New Guid(AssemblyName))
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService
    End Sub
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
    Private Function SaveAssemblyMonitorServiceStatus(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mMultiCompliance As MultiCompliance, Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "", Optional DoneRemark As String = "", Optional ByVal ActualManHrsPar As String = "") As Boolean
        Dim clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        clnAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

        SetAssemblyMonitorServiceStatusObject(mAssemblyMonitorServiceStatus, mMultiCompliance, LicenceNo, EmployeeID, EmployeeName, DoneRemark, ActualManHrsPar)

        If mAssemblyMonitorServiceStatus.IsValid Then
            If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Monitor Service Status.Assembly Monitor Service Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
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
                IsSavedSuccessfully = True
                mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString + "Description : " + mMultiCompliance.Description + " ATA Chapter : " + mMultiCompliance.ATAChapter

                MarkLog(Util.Action.Save, "Assembly Service Status", mMaintenanceOnDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                IsSavedSuccessfully = False

                Session("mAssemblyMonitorServiceStatus") = clnAssemblyMonitorServiceStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                End If
            Finally
                clnAssemblyMonitorServiceStatus = Nothing
            End Try
        End If
        Return False
    End Function
    Private Sub SetAssemblyMonitorInspStatusObject(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mMultiCompliance As MultiCompliance, LicenceNo As String, EmployeeID As String, EmployeeName As String, DoneRemark As String, ActualManHrsPar As String)
        mAssemblyMonitorInspStatus.DoneRemark = DoneRemark
        mAssemblyMonitorInspStatus.DoneWONo = mMultiCompliance.DoneOnWONo


        mAssemblyMonitorInspStatus.Place = mMultiCompliance.Place
        mAssemblyMonitorInspStatus.LicenseNo = mMultiCompliance.LicenseNo


        With mAssemblyMonitorInspStatus
            Dim Licenses() As String
            Dim EmpID() As String
            Dim EmpName() As String
            Dim ActManHrsArray() As String

            If LicenceNo <> "" Then
                If .MaintenanceDoneByEmployees.Count > 0 Then
                    .MaintenanceDoneByEmployees.Remove(mAssemblyMonitorInspStatus.ID)
                End If

                Licenses = LicenceNo.Split(",")
                EmpID = EmployeeID.Split(",")
                EmpName = EmployeeName.Split(",")
                ActManHrsArray = ActualManHrsPar.Split(",")

                For i As Integer = 0 To EmpID.Length - 1
                    .MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, mMultiCompliance.MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrsArray(i), EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                    .MaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrsArray(i)
                Next
                mAssemblyMonitorInspStatus.LicenseNo = Licenses(0)
                mAssemblyMonitorInspStatus.DoneByID = New Guid(EmpID(0))
                mAssemblyMonitorInspStatus.RequiredManHours = ActManHrsArray(0)
            End If
        End With

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForAssemblyInsp.Contains(mAssemblyMonitorInspStatus.ID, 6, "")) Then   ''Session("From") = 0 And
            mMachineMaintenanceForAssemblyInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 6, txtAsOnDate.Text.ToString, mAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorInspStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForAssemblyInsp = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatus.ID, 6)
        End If

        With mMachineMaintenanceForAssemblyInsp
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mAssemblyMonitorInspStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text.ToString, New Guid(MachineName), New Guid(AssemblyName))
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp
    End Sub
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
    Private Function SaveAssemblyMonitorInspStatus(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mMultiCompliance As MultiCompliance, Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "", Optional DoneRemark As String = "", Optional ByVal ActualManHrsPar As String = "") As Boolean
        Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)

        SetAssemblyMonitorInspStatusObject(mAssemblyMonitorInspStatus, mMultiCompliance, LicenceNo, EmployeeID, EmployeeName, DoneRemark, ActualManHrsPar)
        If mAssemblyMonitorInspStatus.IsValid Then
            If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Service Status.Assembly Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Try
                mAssemblyMonitorInspStatus.ApplyEdit()
                mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
                SaveAssemblyMonitorInspStatusBoardInfo(mAssemblyMonitorInspStatus)
                SaveMachineMaintenance(mMachineMaintenanceForAssemblyInsp)
                Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                mAssemblyInfo = Session("mAssemblyInfo")
                IsSavedSuccessfully = True
                mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString + "Description : " + mMultiCompliance.Description + " ATA Chapter : " + mMultiCompliance.ATAChapter
                MarkLog(Util.Action.Save, "Assembly Inspection Status", mMaintenanceOnDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                IsSavedSuccessfully = False
                Session("mAssemblyMonitorInspStatus") = clnAssemblyMonitorInspStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                End If
            Finally
                clnAssemblyMonitorInspStatus = Nothing
            End Try
        End If
        Return False
    End Function

    Private Sub SetAssemblyMonitorModStatusObject(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mMultiCompliance As MultiCompliance, LicenceNo As String, EmployeeID As String, EmployeeName As String, DoneRemark As String, ActualManHrsPar As String)
        mAssemblyMonitorModStatus.DoneRemark = DoneRemark
        mAssemblyMonitorModStatus.DoneWONo = mMultiCompliance.DoneOnWONo


        mAssemblyMonitorModStatus.Place = mMultiCompliance.Place



        With mAssemblyMonitorModStatus
            Dim Licenses() As String
            Dim EmpID() As String
            Dim EmpName() As String
            Dim ActManHrsArray() As String

            If LicenceNo <> "" Then
                If .MaintenanceDoneByEmployees.Count > 0 Then
                    .MaintenanceDoneByEmployees.Remove(mAssemblyMonitorModStatus.ID)
                End If

                Licenses = LicenceNo.Split(",")
                EmpID = EmployeeID.Split(",")
                EmpName = EmployeeName.Split(",")
                ActManHrsArray = ActualManHrsPar.Split(",")

                For i As Integer = 0 To EmpID.Length - 1
                    .MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, mMultiCompliance.MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrsArray(0), EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                    .MaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrsArray(i)
                Next
                mAssemblyMonitorModStatus.LicenseNo = Licenses(0)
                mAssemblyMonitorModStatus.DoneByID = New Guid(EmpID(0))
                mAssemblyMonitorModStatus.RequiredManHours = ActManHrsArray(0)
            End If
        End With

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForAssemblyMod.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then  ''Session("From") = 0 And
            mMachineMaintenanceForAssemblyMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 7, txtAsOnDate.Text.ToString, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorModStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForAssemblyMod = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
        End If

        With mMachineMaintenanceForAssemblyMod
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text.ToString, New Guid(MachineName), New Guid(AssemblyName))
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod
    End Sub
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
    Private Function SaveAssemblyMonitorModStatus(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mMultiCompliance As MultiCompliance, Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "", Optional DoneRemark As String = "", Optional ByVal ActualManHrsPar As String = "") As Boolean
        Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus
        clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)

        SetAssemblyMonitorModStatusObject(mAssemblyMonitorModStatus, mMultiCompliance, LicenceNo, EmployeeID, EmployeeName, DoneRemark, ActualManHrsPar)
        If mAssemblyMonitorModStatus.IsValid Then
            If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Directive Status.Assembly Directive Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                msg1.Show()
            End If
            Try
                mAssemblyMonitorModStatus.ApplyEdit()
                mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
                SaveAssemblyMonitorModStatusBoardInfo(mAssemblyMonitorModStatus)
                SaveMachineMaintenance(mMachineMaintenanceForAssemblyMod)
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                mAssemblyInfo = Session("mAssemblyInfo")
                IsSavedSuccessfully = True
                mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString + "Description : " + mMultiCompliance.Description + " ATA Chapter : " + mMultiCompliance.ATAChapter + IIf(mMultiCompliance.DirectiveNumber <> "", " Directive No. : " + mMultiCompliance.DirectiveNumber, "")
                MarkLog(Util.Action.Save, "Assembly Modification Status", mMaintenanceOnDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                IsSavedSuccessfully = False
                Session("mAssemblyMonitorModStatus") = clnAssemblyMonitorModStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                End If
            Finally
                clnAssemblyMonitorModStatus = Nothing
            End Try
        End If
        Return False
    End Function
    Private Sub SetCompMonitorServiceStatusObject(ByVal mCompMonitorServiceStatus As CompMonitorServiceStatus, ByVal mMultiCompliance As MultiCompliance, LicenceNo As String, EmployeeID As String, EmployeeName As String, DoneRemark As String, ActualManHrsPar As String)
        mCompMonitorServiceStatus.DoneRemark = DoneRemark
        mCompMonitorServiceStatus.DoneWONo = mMultiCompliance.DoneOnWONo

        'Added on 27-03-2019 by Shital
        mCompMonitorServiceStatus.Place = mMultiCompliance.Place
        mCompMonitorServiceStatus.LicenseNo = mMultiCompliance.LicenseNo
        '-------
        With mCompMonitorServiceStatus
            Dim Licenses() As String
            Dim EmpID() As String
            Dim EmpName() As String
            Dim ActManHrsArray() As String

            If LicenceNo <> "" Then
                If .MaintenanceDoneByEmployees.Count > 0 Then
                    .MaintenanceDoneByEmployees.Remove(mCompMonitorServiceStatus.ID)
                End If

                Licenses = LicenceNo.Split(",")
                EmpID = EmployeeID.Split(",")
                EmpName = EmployeeName.Split(",")
                ActManHrsArray = ActualManHrsPar.Split(",")

                For i As Integer = 0 To EmpID.Length - 1
                    .MaintenanceDoneByEmployees.Add(mCompMonitorServiceStatus.ID, mMultiCompliance.MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrsArray(i), EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                    .MaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrsArray(i)
                Next
                mCompMonitorServiceStatus.LicenseNo = Licenses(0)
                mCompMonitorServiceStatus.DoneByID = New Guid(EmpID(0))
                mCompMonitorServiceStatus.RequiredManHours = ActManHrsArray(0)
            End If
        End With

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForCompService.Contains(mCompMonitorServiceStatus.ID, 8, "")) Then  '' Session("From") = 0 And
            mMachineMaintenanceForCompService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 8, txtAsOnDate.Text.ToString, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mCompMonitorServiceStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForCompService = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatus.ID, 8)
        End If

        With mMachineMaintenanceForCompService
            ''.MachineID = mCompStatus.MachineID
            ''.MaintenanceActivityTypeID =8
            .MaintenanceID = mCompMonitorServiceStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text.ToString, New Guid(MachineName), New Guid(AssemblyName))
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForCompService") = mMachineMaintenanceForCompService
    End Sub
    Private Sub SaveCompMonitorServiceStatus(ByVal mCompMonitorServiceStatus As CompMonitorServiceStatus, ByVal mMultiCompliance As MultiCompliance, Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "", Optional DoneRemark As String = "", Optional ByVal ActualManHrsPar As String = "")
        Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus
        clnCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)

        SetCompMonitorServiceStatusObject(mCompMonitorServiceStatus, mMultiCompliance, LicenceNo, EmployeeID, EmployeeName, DoneRemark, ActualManHrsPar)
        If mCompMonitorServiceStatus.IsValid Then
            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Component Service Status.Component Service Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                msg1.Show()
            End If
            Try
                mCompMonitorServiceStatus.ApplyEdit()
                mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                SaveMachineMaintenance(mMachineMaintenanceForCompService)
                mCompInfo = Session("mCompInfo")
                IsSavedSuccessfully = True
                mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString + "Description : " + mMultiCompliance.Description + " ATA Chapter : " + mMultiCompliance.ATAChapter
                MarkLog(Util.Action.Save, "Component Service Status", mMaintenanceOnDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
            Catch ex As SqlException
                Session("mCompMonitorServiceStatus") = clnCompMonitorServiceStatus
                IsSavedSuccessfully = False
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Finally
                clnCompMonitorServiceStatus = Nothing
            End Try
        End If
    End Sub
    Private Sub SetCompMonitorInspStatusObject(ByVal mCompMonitorInspStatus As CompMonitorInspStatus, ByVal mMultiCompliance As MultiCompliance, LicenceNo As String, EmployeeID As String, EmployeeName As String, DoneRemark As String, ActualManHrsPar As String)
        mCompMonitorInspStatus.DoneRemark = DoneRemark
        mCompMonitorInspStatus.DoneWONo = mMultiCompliance.DoneOnWONo

        'Added on 27-03-2019 by Shital
        mCompMonitorInspStatus.Place = mMultiCompliance.Place
        ' mCompMonitorInspStatus.LicenseNo = mMultiCompliance.LicenseNo
        '-------

        With mCompMonitorInspStatus
            Dim Licenses() As String
            Dim EmpID() As String
            Dim EmpName() As String
            Dim ActManHrsArray() As String

            If LicenceNo <> "" Then
                If .MaintenanceDoneByEmployees.Count > 0 Then
                    .MaintenanceDoneByEmployees.Remove(mCompMonitorInspStatus.ID)
                End If

                Licenses = LicenceNo.Split(",")
                EmpID = EmployeeID.Split(",")
                EmpName = EmployeeName.Split(",")
                ActManHrsArray = ActualManHrsPar.Split(",")

                For i As Integer = 0 To EmpID.Length - 1
                    .MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, mMultiCompliance.MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrsArray(i), EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                    .MaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrsArray(i)
                Next
                mCompMonitorInspStatus.LicenseNo = Licenses(0)
                mCompMonitorInspStatus.DoneByID = New Guid(EmpID(0))
                mCompMonitorInspStatus.RequiredManHours = ActManHrsArray(0)
            End If
        End With

        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForCompInsp.Contains(mCompMonitorInspStatus.ID, 9, "")) Then   ''Session("From") = 0 And
            mMachineMaintenanceForCompInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 9, txtAsOnDate.Text.ToString, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, mCompMonitorInspStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForCompInsp = MachineMaintenance.GetMachineMaintenance(mCompMonitorInspStatus.ID, 9)
        End If

        With mMachineMaintenanceForCompInsp
            ''.MachineID = mCompStatus.MachineID
            ''.MaintenanceActivityTypeID =9
            .MaintenanceID = mCompMonitorInspStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text.ToString, New Guid(MachineName), New Guid(AssemblyName))
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp
    End Sub
    Private Sub SaveCompMonitorInspStatus(ByVal mCompMonitorInspStatus As CompMonitorInspStatus, ByVal mMultiCompliance As MultiCompliance, Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "", Optional DoneRemark As String = "", Optional ByVal ActualManHrsPar As String = "")
        Dim clnCompMonitorInspStatus As CompMonitorInspStatus
        clnCompMonitorInspStatus = CType(mCompMonitorInspStatus.Clone, CompMonitorInspStatus)

        SetCompMonitorInspStatusObject(mCompMonitorInspStatus, mMultiCompliance, LicenceNo, EmployeeID, EmployeeName, DoneRemark, ActualManHrsPar)
        If mCompMonitorInspStatus.IsValid Then
            If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Component Inspection Status.Component Inspection Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                msg1.Show()
            End If
            Try
                mCompMonitorInspStatus.ApplyEdit()
                mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                SaveMachineMaintenance(mMachineMaintenanceForCompInsp)
                mCompInfo = Session("mCompInfo")
                IsSavedSuccessfully = True
                mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString + "Description : " + mMultiCompliance.Description + " ATA Chapter : " + mMultiCompliance.ATAChapter
                MarkLog(Util.Action.Save, "Component Inspection Status", mMaintenanceOnDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
            Catch ex As SqlException
                Session("mCompMonitorInspStatus") = clnCompMonitorInspStatus
                IsSavedSuccessfully = False
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Finally
                clnCompMonitorInspStatus = Nothing
            End Try
        End If
    End Sub
    Private Sub SetCompMonitorModStatusObject(ByVal mCompMonitorModStatus As CompMonitorModStatus, ByVal mMultiCompliance As MultiCompliance, LicenceNo As String, EmployeeID As String, EmployeeName As String, DoneRemark As String, ActualManHrsPar As String)
        mCompMonitorModStatus.DoneRemark = DoneRemark
        mCompMonitorModStatus.DoneWONo = mMultiCompliance.DoneOnWONo

        'Added on 27-03-2019 by Shital
        mCompMonitorModStatus.Place = mMultiCompliance.Place
        ' mCompMonitorModStatus.LicenseNo = mMultiCompliance.LicenseNo
        '-------
        With mCompMonitorModStatus
            Dim Licenses() As String
            Dim EmpID() As String
            Dim EmpName() As String
            Dim ActManHrsArray() As String

            If LicenceNo <> "" Then
                If .MaintenanceDoneByEmployees.Count > 0 Then
                    .MaintenanceDoneByEmployees.Remove(mCompMonitorModStatus.ID)
                End If

                Licenses = LicenceNo.Split(",")
                EmpID = EmployeeID.Split(",")
                EmpName = EmployeeName.Split(",")
                ActManHrsArray = ActualManHrsPar.Split(",")

                For i As Integer = 0 To EmpID.Length - 1
                    .MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, mMultiCompliance.MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrsArray(i), EmpName(i))
                    .MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                    .MaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrsArray(i)
                Next
                mCompMonitorModStatus.LicenseNo = Licenses(0)
                mCompMonitorModStatus.DoneByID = New Guid(EmpID(0))
                mCompMonitorModStatus.RequiredManHours = ActManHrsArray(0)
            End If
        End With


        'Added by Saylee on 28th-Oct-2009
        If Not (mMachineMaintenanceListForCompMod.Contains(mCompMonitorModStatus.ID, 10, "")) Then  ''Session("From") = 0 And
            mMachineMaintenanceForCompMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 10, txtAsOnDate.Text.ToString, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, mCompMonitorModStatus.AssemblyStatusID)
        Else
            mMachineMaintenanceForCompMod = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatus.ID, 10)
        End If

        With mMachineMaintenanceForCompMod
            ''.MachineID = mCompStatus.MachineID
            ''.MaintenanceActivityTypeID =10
            .MaintenanceID = mCompMonitorModStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtAsOnDate.Text

            mLog = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text.ToString, New Guid(MachineName), New Guid(AssemblyName))
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod
    End Sub
    Private Sub SaveCompMonitorModStatus(ByVal mCompMonitorModStatus As CompMonitorModStatus, ByVal mMultiCompliance As MultiCompliance, Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "", Optional DoneRemark As String = "", Optional ByVal ActualManHrsPar As String = "")
        Dim clnCompMonitorModStatus As CompMonitorModStatus
        clnCompMonitorModStatus = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)

        SetCompMonitorModStatusObject(mCompMonitorModStatus, mMultiCompliance, LicenceNo, EmployeeID, EmployeeName, DoneRemark, ActualManHrsPar)
        If mCompMonitorModStatus.IsValid Then
            If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Component Modification Status.Component Modification Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplianceCartListPartII.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                msg1.Show()
            End If
            Try
                mCompMonitorModStatus.ApplyEdit()
                mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                SaveMachineMaintenance(mMachineMaintenanceForCompMod)
                mCompInfo = Session("mCompInfo")
                IsSavedSuccessfully = True
                mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString + "Description : " + mMultiCompliance.Description + " ATA Chapter : " + mMultiCompliance.ATAChapter + IIf(mMultiCompliance.DirectiveNumber <> "", " Directive No. : " + mMultiCompliance.DirectiveNumber, "")
                MarkLog(Util.Action.Save, "Component Modification Status", mMaintenanceOnDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
            Catch ex As SqlException
                Session("mCompMonitorModStatus") = clnCompMonitorModStatus
                IsSavedSuccessfully = False
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Finally
                clnCompMonitorModStatus = Nothing
            End Try
        End If
    End Sub

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

#Region " Data Binding "
    Public Function CustomValidate1() As Boolean
        Dim txtRemark, txtActualManHrs As TextBox
        Dim mActualManHours As New Period(1, DBNull.Value, 0, True, False)
        Dim str As String = ""
        Dim str1 As String = ""
        Dim strbuilder As StringBuilder = New StringBuilder()
        Dim cvValidator, cvValidator1 As CustomValidator
        Dim validationcontrol, validationcontrol1 As ValidationSummary
        Dim upnlValidationSummary, upnlValidationSummary1 As UpdatePanel

        For i As Integer = 0 To dgMultiComplianceList.Rows.Count - 1
            cvValidator = CType(Me.dgMultiComplianceList.Rows(i).FindControl("cvRemark"), CustomValidator)
            txtRemark = CType(Me.dgMultiComplianceList.Rows(i).FindControl("txtRemark"), TextBox)
            validationcontrol = CType(Me.dgMultiComplianceList.Rows(i).FindControl("Validationsummary2"), ValidationSummary)
            upnlValidationSummary = CType(Me.dgMultiComplianceList.Rows(i).FindControl("upnlValidationSummary"), UpdatePanel)

            txtActualManHrs = CType(Me.dgMultiComplianceList.Rows(i).FindControl("txtActualManHrs"), TextBox)
            mActualManHours.Value = Trim(txtActualManHrs.Text)
            upnlValidationSummary1 = CType(Me.dgMultiComplianceList.Rows(i).FindControl("upnlValidationSummary1"), UpdatePanel)
            cvValidator1 = CType(Me.dgMultiComplianceList.Rows(i).FindControl("cvActManHrs"), CustomValidator)
            validationcontrol1 = CType(Me.dgMultiComplianceList.Rows(i).FindControl("Validationsummary21"), ValidationSummary)

            If Len(txtRemark.Text) > 500 Then
                'If str = "" Then
                '    str = "Comply Remark should not be greater than 500 characters " + mMultiComplianceList(i).MaintenanceActivityName + "-> " + dgMultiComplianceList.Rows(i).Cells(8).Text + "<BR>"
                'Else
                '    str = str + "Comply Remark should not be greater than 500 characters " + mMultiComplianceList(i).MaintenanceActivityName + "-> " + dgMultiComplianceList.Rows(i).Cells(8).Text + "<BR>"
                'End If
                cvValidator.IsValid = False
                cvValidator.Text = "Remark should be less than 500 characters."
                str = "Remark should be less than 500 characters."
                strbuilder.Append(str)
                upnlValidationSummary.Update()
            Else
                cvValidator.IsValid = True
                cvValidator.Text = ""
                str = ""
                upnlValidationSummary.Update()
            End If

            If (Not mActualManHours.IsValid And mActualManHours.Value <> "") Then
                cvValidator1.IsValid = False
                cvValidator1.ErrorMessage = "Actual Man Hours : " & mActualManHours.ErrMsg
                str1 = "Actual Man Hours : " & mActualManHours.ErrMsg
                strbuilder.Append(str1)
                upnlValidationSummary1.Update()
            Else
                cvValidator1.IsValid = True
                cvValidator1.Text = ""
                str1 = ""
                upnlValidationSummary1.Update()
            End If
            'Session("str") = str
            'str = Session("str")
        Next

        If strbuilder.Length <> 0 Then
            Return False
        End If
        Return True
    End Function
    Public Sub DataFieldBind()
        dgMultiComplianceList.DataSource = mMultiComplianceList
        dgMultiComplianceList.DataBind()

        If Not Session("LogId") Is Nothing Then
            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId).Item(0), MachineInfo).AssemblyStatusList
            AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            tmpAssemblyStatusList = Nothing
        End If

        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
        dgDoneOnValue.DataBind()

        'Added by Saylee on 6th-Oct-2009
        Dim mMachineMaintenanceList As MachineMaintenanceList
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceList
        Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceList

        txtAircraft.Text = Aircraft
        txtAssembly.Text = AssemblyType

        mMachineMaintenanceListForAssemblyService = Session("mMachineMaintenanceListForAssemblyService")
        mMachineMaintenanceListForAssemblyInsp = Session("mMachineMaintenanceListForAssemblyInsp")
        mMachineMaintenanceListForAssemblyMod = Session("mMachineMaintenanceListForAssemblyMod")
        mMachineMaintenanceListForCompService = Session("mMachineMaintenanceListForCompService")
        mMachineMaintenanceListForCompInsp = Session("mMachineMaintenanceListForCompInsp")
        mMachineMaintenanceListForCompMod = Session("mMachineMaintenanceListForCompMod")

        txtAsOnDate.Enabled = False
        txtAsOnDate.Text = AsonDate
    End Sub
    Public Sub SetCaption()
        lblResult.Text = "List of records to be Complied : " & mMultiComplianceList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '' ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And Session("Sender") = "" Then
            ''DueType = 1
            If AsonDate Is Nothing Then AsonDate = Request.QueryString("DoneOn")
            If MachineName Is Nothing Then MachineName = Request.QueryString("MachineId")
            If HourType Is Nothing Then HourType = Request.QueryString("HourType")
            If AssemblyName Is Nothing Then AssemblyName = Request.QueryString("AssemblyID")

            ''SetFocus(txtWorkOrderNo)
            txtAsOnDate.Enabled = False
            txtAsOnDate.Text = AsonDate
            Session("mLogList") = Nothing
            DataFieldBind()
            Controltovisibility()
            SetCaption()
            ''SetLog()
        End If
        If Not Session("LogId") Is Nothing Then
            If (Not New Guid(CType(Session("LogId"), String)).Equals(Guid.Empty)) Then
                mLog = Log.GetLog(New Guid(CType(Session("LogId"), String)))
                Session("mLog") = mLog
            End If
        End If
    End Sub
    Private Sub dgMultiComplianceList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMultiComplianceList.RowCommand

        Select Case e.CommandName()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMultiComplianceList.PageSize * dgMultiComplianceList.PageIndex
                mMultiComplianceList = Session("mMultiComplianceList")
                Session("Index") = Index
                ' msg.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "Remove")
                Exit Sub
            Case "EmployeeLicence"
                Dim mRowMaintenanceID As Guid


                Dim rowIndex As String = e.CommandArgument
                mRowMaintenanceID = mMultiComplianceList(CInt(rowIndex)).ID  'New Guid(e.CommandArgument.ToString)
                Session("mMaintenanceID") = mRowMaintenanceID 'mtmpInstalledCompList(mID).CompStatusID
                Session("MaintenanceDoneOnDate") = txtAsOnDate.Text.ToString

                Dim hdnLicenceNo As HiddenField
                Dim hdnEmployeeID As HiddenField
                Dim hdnEmployeeName As HiddenField
                Dim hdnActualManHrs As HiddenField
                Dim txtLicenceNo As TextBox
                Dim txtActualManHrs As TextBox

                hdnLicenceNo = CType(Me.dgMultiComplianceList.Rows(CInt(rowIndex)).FindControl("hdnLicenceNo"), HiddenField)
                hdnEmployeeID = CType(Me.dgMultiComplianceList.Rows(CInt(rowIndex)).FindControl("hdnEmployeeID"), HiddenField)
                hdnEmployeeName = CType(Me.dgMultiComplianceList.Rows(CInt(rowIndex)).FindControl("hdnEmployeeName"), HiddenField)
                hdnActualManHrs = CType(Me.dgMultiComplianceList.Rows(CInt(rowIndex)).FindControl("hdnActualManHrs"), HiddenField)
                txtActualManHrs = CType(Me.dgMultiComplianceList.Rows(CInt(rowIndex)).FindControl("txtActualManHrs"), TextBox)

                txtLicenceNo = CType(Me.dgMultiComplianceList.Rows(CInt(rowIndex)).FindControl("txtLicenceNo"), TextBox)

                If txtActualManHrs.Text <> "" AndAlso Not hdnActualManHrs.Value.Split(",").Length > 1 Then
                    hdnActualManHrs.Value = txtActualManHrs.Text
                End If

                Dim Licenses() As String
                Dim EmpID() As String
                Dim EmpName() As String
                Dim ActManHrs() As String
                Dim mMaintenanceDoneByEmployees As MaintenanceDoneByEmployees = New MaintenanceDoneByEmployees

                If mMaintenanceDoneByEmployees.Count > 0 Then
                    mMaintenanceDoneByEmployees.Remove(mRowMaintenanceID)
                End If

                If hdnLicenceNo.Value <> "" Then
                    Licenses = hdnLicenceNo.Value.Split(",")
                    EmpID = hdnEmployeeID.Value.Split(",")
                    EmpName = hdnEmployeeName.Value.Split(",")
                    ActManHrs = hdnActualManHrs.Value.Split(",")

                    If txtLicenceNo.Text <> "" Then
                        For i As Integer = 0 To EmpID.Length - 1
                            mMaintenanceDoneByEmployees.Add(mRowMaintenanceID, mMultiComplianceList(CInt(rowIndex)).MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrs(i), EmpName(i))
                            mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                            mMaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrs(i)
                        Next
                    Else
                        For i As Integer = 1 To EmpID.Length - 1 'Skip first record as txtLicenceNo is cleared
                            mMaintenanceDoneByEmployees.Add(mRowMaintenanceID, mMultiComplianceList(CInt(rowIndex)).MaintenanceActivity, Guid.Empty, Licenses(i), ActManHrs(i), EmpName(i))
                            mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
                            mMaintenanceDoneByEmployees.CurrentItem.RequiredManHours = ActManHrs(i)
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
                    mMaintenanceDoneByEmployees.Add(mRowMaintenanceID, mMultiComplianceList(CInt(rowIndex)).MaintenanceActivity, Guid.Empty, LicenseNo, "", EmployeeName)
                    mMaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmployeeID)
                    mMaintenanceDoneByEmployees.CurrentItem.RequiredManHours = txtActualManHrs.Text

                End If

                Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo(" + mMultiComplianceList(CInt(rowIndex)).MaintenanceActivity.ToString + ");", True)
        End Select
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
        Dim index As Integer

        Dim ReasonID As Guid = Guid.Empty
        Dim RemovalReasonName As String = String.Empty
        Dim LicenceEmpNo As String = String.Empty

        Dim txtReason As TextBox
        Dim txtLicenceNo As TextBox
        Dim hdnLicenceNo As HiddenField

        Dim hdnEmployeeID As HiddenField
        Dim hdnEmployeeName As HiddenField
        Dim hdnActualManHrs As HiddenField

        Dim LicenceNo As String = String.Empty
        Dim EmployeeID As String = String.Empty
        Dim EmployeeName As String = String.Empty

        Dim cvValidator As RequiredFieldValidator
        Dim upnlReasonValidate As UpdatePanel

        Dim txtRemark, txtActualManHrs As TextBox

        If CustomValidate1() = False Then Exit Sub


        For index = 0 To mMultiComplianceList.Count - 1
            hdnLicenceNo = CType(Me.dgMultiComplianceList.Rows(index).FindControl("hdnLicenceNo"), HiddenField)
            hdnEmployeeID = CType(Me.dgMultiComplianceList.Rows(index).FindControl("hdnEmployeeID"), HiddenField)
            hdnEmployeeName = CType(Me.dgMultiComplianceList.Rows(index).FindControl("hdnEmployeeName"), HiddenField)
            hdnActualManHrs = CType(Me.dgMultiComplianceList.Rows(index).FindControl("hdnActualManHrs"), HiddenField)


            txtLicenceNo = CType(Me.dgMultiComplianceList.Rows(index).FindControl("txtLicenceNo"), TextBox)
            txtRemark = CType(Me.dgMultiComplianceList.Rows(index).FindControl("txtRemark"), TextBox)
            txtActualManHrs = CType(Me.dgMultiComplianceList.Rows(index).FindControl("txtActualManHrs"), TextBox)

            If hdnLicenceNo.Value <> "" Then
                LicenceNo = hdnLicenceNo.Value
                EmployeeID = hdnEmployeeID.Value
                EmployeeName = hdnEmployeeName.Value
                ActualManHrs = hdnActualManHrs.Value
            ElseIf txtLicenceNo.Text <> "" Then
                If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                    LicenceNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                    EmployeeName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                Else
                    LicenceNo = Trim(txtLicenceNo.Text)
                End If
                EmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenceNo, EmployeeName).EmpID.ToString
            End If

            Select Case mMultiComplianceList(index).MaintenanceActivity
                Case MaintenanceActivityTypes.RemovalComp   '1. Removal Comp
                    Dim mCompStatus As CompStatus

                    Session("From") = 1 'NewRemove
                    Session("mCompStatus") = mCompStatus
                    Dim mPrevCompStatus As CompStatus = CompStatus.GetCompStatus(mMultiComplianceList(index).CompStatusID, mMultiComplianceList(index).AssemblyStatusID, mMultiComplianceList(index).InstalledOnDBValue)
                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                    If CType(Session("FromLog"), Boolean) = True Then
                        mCompStatus = CompStatus.NewRemovalCompStatus(mPrevCompStatus.ID, mMultiComplianceList(index).RemovedOn.ToString, mAssemblyStatus.ID, LogId)
                    Else
                        mCompStatus = CompStatus.NewRemovalCompStatus(mMultiComplianceList(index).CompStatusID, mMultiComplianceList(index).RemovedOn.ToString, mMultiComplianceList(index).AssemblyStatusID, Guid.Empty.ToString)
                    End If
                    SaveComp(mCompStatus, mMultiComplianceList.Item(index), "Component Removal", LicenceNo, EmployeeID, EmployeeName)

                    mCompStatus = Nothing
                    mPrevCompStatus = Nothing
                    mAssemblyStatus = Nothing
                Case MaintenanceActivityTypes.InstallComp   '2. Install Comp
                    Dim mCompStatus As CompStatus
                    Dim mRemovedCompStatus As CompStatus = CompStatus.GetCompStatus(mMultiComplianceList(index).CompStatusID, mMultiComplianceList(index).AssemblyStatusID, mMultiComplianceList(index).InstalledOnDBValue)
                    If CType(Session("FromLog"), Boolean) = True Then
                    Else
                        mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mRemovedCompStatus.AssemblyID, mMultiComplianceList(index).AssemblyStatusID, mMultiComplianceList(index).InstalledOnDBValue, True, mRemovedCompStatus.ID.ToString, Guid.Empty.ToString)
                    End If

                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                    Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(index).MachineID)

                    Session("From") = 1 'NewInstall
                    Session("InstallSelected") = 1
                    Session("mCompStatus") = mCompStatus
                    Session("mRemovedCompStatus") = mRemovedCompStatus
                    Session("mAssemblyStatus") = mAssemblyStatus
                    Session("mMachine") = mMachine

                    SaveComp(mCompStatus, mMultiComplianceList.Item(index), "Component Install", LicenceNo, EmployeeID, EmployeeName)

                Case MaintenanceActivityTypes.AssemblyService  '5. Assembly Service
                    Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(index).MachineID)
                    Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
                    Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mMultiComplianceList.Item(index).AssemblyMonitorServiceStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMachine.HourType)

                    'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
                    If mPrevAssemblyMonitorServiceStatus.DoneOn.ToString <> "" Then
                        If (CDate(txtAsOnDate.Text) <= CDate(mPrevAssemblyMonitorServiceStatus.DoneOn)) Then
                            IsCompliedOnSameDate = True
                        End If
                    End If
                    If (CDate(txtAsOnDate.Text) > CDate(Today.Date)) Then
                        IsCompliedOnSameDate = True
                    End If
                    'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

                    If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        If CType(Session("FromLog"), Boolean) = True Then
                            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mMultiComplianceList.Item(index).CurrentDate.ToString, mMultiComplianceList(index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, New Guid(LogId), mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
                        Else
                            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mMultiComplianceList.Item(index).CurrentDate.ToString, mMultiComplianceList(index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
                        End If

                        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                        Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
                        Session("From") = 0 'New record
                        ''
                        'mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
                        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

                        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                        Session("mMachine") = mMachine
                        Session("mAssemblyStatus") = mAssemblyStatus


                        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
                        Session("mBoardInfo") = mBoardInfo

                        Session("mAssemblyInfo") = ""
                        Session("mAssemblyInfo") = mMultiComplianceList.Item(index).MachineInfo + "->" + mMultiComplianceList.Item(index).ModelSerialNo + "->" + mMultiComplianceList.Item(index).Reference + "->" + mMultiComplianceList.Item(index).MonitorInfo + "->" + mMultiComplianceList.Item(index).MonitorType + "->" + mMultiComplianceList.Item(index).ATA + "->" + mMultiComplianceList.Item(index).Description

                        If SaveAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatus, mMultiComplianceList.Item(index), LicenceNo, EmployeeID, EmployeeName, Trim(txtRemark.Text), ActualManHrs) = True Then
                            LinkMaintenance(mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine, mMaintenanceOnDetail, mMultiComplianceList.Item(index).DoneOnWONo, mAssemblyMonitorServiceStatus.AssemblyID, "Assembly Servicing", mMachineMaintenanceForAssemblyService, mMultiComplianceList.Item(index).DoneOn.ToString, Trim(txtRemark.Text), LicenceNo, EmployeeID, EmployeeName)
                        End If

                        Session("MaintenanceActivityTypeID") = 5
                    End If
                Case MaintenanceActivityTypes.AssemblyInspection   '6. Assembly Inspection
                    Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(index).MachineID)
                    Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mMultiComplianceList.Item(index).AssemblyMonitorInspStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMachine.HourType)

                    'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
                    If mPrevAssemblyMonitorInspStatus.DoneOn.ToString <> "" Then
                        If (CDate(txtAsOnDate.Text) <= CDate(mPrevAssemblyMonitorInspStatus.DoneOn)) Then
                            IsCompliedOnSameDate = True
                        End If
                    End If
                    If (CDate(txtAsOnDate.Text) > CDate(Today.Date)) Then
                        IsCompliedOnSameDate = True
                    End If
                    'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

                    If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        If CType(Session("FromLog"), Boolean) = True Then
                            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mMultiComplianceList.Item(index).CurrentDate.ToString, mMultiComplianceList(index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, New Guid(LogId), mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                        Else
                            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mMultiComplianceList.Item(index).CurrentDate.ToString, mMultiComplianceList(index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                        End If

                        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
                        Session("From") = 0 'New record
                        ''
                        mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
                        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

                        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                        Session("mMachine") = mMachine
                        Session("mAssemblyStatus") = mAssemblyStatus


                        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
                        Session("mBoardInfo") = mBoardInfo

                        Session("mAssemblyInfo") = ""
                        Session("mAssemblyInfo") = mMultiComplianceList.Item(index).MachineInfo + "->" + mMultiComplianceList.Item(index).ModelSerialNo + "->" + mMultiComplianceList.Item(index).Reference + "->" + mMultiComplianceList.Item(index).MonitorInfo + "->" + mMultiComplianceList.Item(index).MonitorType + "->" + mMultiComplianceList.Item(index).ATA + "->" + mMultiComplianceList.Item(index).Description

                        If SaveAssemblyMonitorInspStatus(mAssemblyMonitorInspStatus, mMultiComplianceList.Item(index), LicenceNo, EmployeeID, EmployeeName, Trim(txtRemark.Text), ActualManHrs) = True Then
                            LinkMaintenance(mAssemblyMonitorInspStatus.ModelMonitorInspID, mMachine, mMaintenanceOnDetail, mMultiComplianceList.Item(index).DoneOnWONo, mAssemblyMonitorInspStatus.AssemblyID, "Assembly Inspection", mMachineMaintenanceForAssemblyInsp, mMultiComplianceList.Item(index).DoneOn.ToString, Trim(txtRemark.Text), LicenceNo, EmployeeID, EmployeeName)
                        End If

                        Session("MaintenanceActivityTypeID") = 6
                    End If
                Case MaintenanceActivityTypes.AssemblyDirective    '7. Assembly Directive
                    Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(index).MachineID)
                    Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mMultiComplianceList.Item(index).AssemblyMonitorDirectiveStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMachine.HourType)

                    'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
                    If mPrevAssemblyMonitorModStatus.DoneOn.ToString <> "" Then
                        If (CDate(txtAsOnDate.Text) <= CDate(mPrevAssemblyMonitorModStatus.DoneOn)) Then
                            IsCompliedOnSameDate = True
                        End If
                    End If
                    If (CDate(txtAsOnDate.Text) > CDate(Today.Date)) Then
                        IsCompliedOnSameDate = True
                    End If
                    'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

                    If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        If CType(Session("FromLog"), Boolean) = True Then
                            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mMultiComplianceList.Item(index).CurrentDate.ToString, mMultiComplianceList(index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, New Guid(LogId), mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                        Else
                            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mMultiComplianceList.Item(index).CurrentDate.ToString, mMultiComplianceList(index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                        End If

                        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                        Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
                        Session("From") = 0 'New record
                        ''
                        mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
                        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

                        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                        Session("mMachine") = mMachine
                        Session("mAssemblyStatus") = mAssemblyStatus


                        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
                        Session("mBoardInfo") = mBoardInfo

                        Session("mAssemblyInfo") = ""
                        Session("mAssemblyInfo") = mMultiComplianceList.Item(index).MachineInfo + "->" + mMultiComplianceList.Item(index).ModelSerialNo + "->" + mMultiComplianceList.Item(index).Reference + "->" + mMultiComplianceList.Item(index).MonitorInfo + "->" + mMultiComplianceList.Item(index).MonitorType + "->" + mMultiComplianceList.Item(index).ATA + "->" + mMultiComplianceList.Item(index).Description

                        If SaveAssemblyMonitorModStatus(mAssemblyMonitorModStatus, mMultiComplianceList.Item(index), LicenceNo, EmployeeID, EmployeeName, Trim(txtRemark.Text), ActualManHrs) = True Then
                            LinkMaintenance(mAssemblyMonitorModStatus.ModelMonitorModID, mMachine, mMaintenanceOnDetail, mMultiComplianceList.Item(index).DoneOnWONo, mAssemblyMonitorModStatus.AssemblyID, "Assembly Directives", mMachineMaintenanceForAssemblyMod, mMultiComplianceList.Item(index).DoneOn.ToString, Trim(txtRemark.Text), LicenceNo, EmployeeID, EmployeeName)
                        End If

                        Session("MaintenanceActivityTypeID") = 7

                    End If

                Case MaintenanceActivityTypes.ComponentService  '8. Comp Service
                    Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(index).MachineID)
                    Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
                    Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mMultiComplianceList.Item(index).CompMonitorServiceStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMultiComplianceList.Item(index).CompStatusID, mMachine.HourType)

                    'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
                    If mPrevCompMonitorServiceStatus.DoneOn.ToString <> "" Then
                        If (CDate(txtAsOnDate.Text) <= CDate(mPrevCompMonitorServiceStatus.DoneOn)) Then
                            IsCompliedOnSameDate = True
                        End If
                    End If
                    If (CDate(txtAsOnDate.Text) > CDate(Today.Date)) Then
                        IsCompliedOnSameDate = True
                    End If
                    'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

                    If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        If CType(Session("FromLog"), Boolean) = True Then
                            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, New Guid(LogId), mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
                        Else
                            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
                        End If

                        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                        Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
                        Session("From") = 0 'NewRecord

                        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                        Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mMultiComplianceList.Item(index).CompStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMultiComplianceList.Item(index).DoneOn.ToString)
                        Session("mMachine") = mMachine
                        Session("mCompStatus") = mCompStatus
                        Session("mAssemblyStatus") = mAssemblyStatus
                        mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
                        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

                        Session("mCompInfo") = ""
                        Session("mCompInfo") = mMultiComplianceList.Item(index).MachineInfo + "->" + mMultiComplianceList.Item(index).CompSerialNo + "->" + mMultiComplianceList.Item(index).Reference + "->" + mMultiComplianceList.Item(index).MonitorInfo + "->" + mMultiComplianceList.Item(index).CompInfo + "->" + mMultiComplianceList.Item(index).MonitorType + "->" + mMultiComplianceList.Item(index).ATA + "->" + mMultiComplianceList.Item(index).Description

                        SaveCompMonitorServiceStatus(mCompMonitorServiceStatus, mMultiComplianceList.Item(index), LicenceNo, EmployeeID, EmployeeName, Trim(txtRemark.Text), ActualManHrs)

                        Session("MaintenanceActivityTypeID") = 8
                    End If
                    '***************
                Case MaintenanceActivityTypes.ComponentInspection   '9. Component Inspection
                    Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(index).MachineID)
                    Dim mCompMonitorInspStatus As CompMonitorInspStatus
                    Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mMultiComplianceList.Item(index).CompMonitorInspStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMultiComplianceList.Item(index).CompStatusID, mMachine.HourType)

                    'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
                    If mPrevCompMonitorInspStatus.DoneOn.ToString <> "" Then
                        If (CDate(txtAsOnDate.Text) <= CDate(mPrevCompMonitorInspStatus.DoneOn)) Then
                            IsCompliedOnSameDate = True
                        End If
                    End If
                    If (CDate(txtAsOnDate.Text) > CDate(Today.Date)) Then
                        IsCompliedOnSameDate = True
                    End If
                    'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

                    If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        If CType(Session("FromLog"), Boolean) = True Then
                            mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, New Guid(LogId), mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                        Else
                            mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
                        End If

                        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                        Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
                        Session("From") = 0 'NewRecord

                        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                        Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mMultiComplianceList.Item(index).CompStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMultiComplianceList.Item(index).DoneOn.ToString)
                        Session("mMachine") = mMachine
                        Session("mCompStatus") = mCompStatus
                        Session("mAssemblyStatus") = mAssemblyStatus
                        mCompMonitorInspStatus.RequiredManHours = mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours
                        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus

                        Session("mCompInfo") = ""
                        Session("mCompInfo") = mMultiComplianceList.Item(index).MachineInfo + "->" + mMultiComplianceList.Item(index).CompSerialNo + "->" + mMultiComplianceList.Item(index).Reference + "->" + mMultiComplianceList.Item(index).MonitorInfo + "->" + mMultiComplianceList.Item(index).CompInfo + "->" + mMultiComplianceList.Item(index).MonitorType + "->" + mMultiComplianceList.Item(index).ATA + "->" + mMultiComplianceList.Item(index).Description

                        SaveCompMonitorInspStatus(mCompMonitorInspStatus, mMultiComplianceList.Item(index), LicenceNo, EmployeeID, EmployeeName, Trim(txtRemark.Text), ActualManHrs)

                        Session("MaintenanceActivityTypeID") = 9
                    End If
                Case MaintenanceActivityTypes.ComponentDirective    '10. Component Directive
                    Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(index).MachineID)
                    Dim mCompMonitorModStatus As CompMonitorModStatus
                    Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mMultiComplianceList.Item(index).CompMonitorDirectiveStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMultiComplianceList.Item(index).CompStatusID, mMachine.HourType)

                    'Added By Prashant 19-Nov-2019 Alert if user is complying on same date  ALL19112019
                    If mPrevCompMonitorModStatus.DoneOn.ToString <> "" Then
                        If (CDate(txtAsOnDate.Text) <= CDate(mPrevCompMonitorModStatus.DoneOn)) Then
                            IsCompliedOnSameDate = True
                        End If
                    End If
                    If (CDate(txtAsOnDate.Text) > CDate(Today.Date)) Then
                        IsCompliedOnSameDate = True
                    End If
                    'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

                    If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    Else
                        If CType(Session("FromLog"), Boolean) = True Then
                            mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, New Guid(LogId), mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                        Else
                            mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
                        End If
                        Session("mCompMonitorModStatus") = mCompMonitorModStatus
                        Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
                        Session("From") = 0 'NewRecord

                        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMultiComplianceList(index).AssemblyStatusID)
                        Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mMultiComplianceList.Item(index).CompStatusID, mMultiComplianceList.Item(index).AssemblyStatusID, mMultiComplianceList.Item(index).DoneOn.ToString)
                        Session("mMachine") = mMachine
                        Session("mCompStatus") = mCompStatus
                        Session("mAssemblyStatus") = mAssemblyStatus
                        mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
                        Session("mCompMonitorModStatus") = mCompMonitorModStatus

                        Session("mCompInfo") = ""
                        Session("mCompInfo") = mMultiComplianceList.Item(index).MachineInfo + "->" + mMultiComplianceList.Item(index).CompSerialNo + "->" + mMultiComplianceList.Item(index).Reference + "->" + mMultiComplianceList.Item(index).MonitorInfo + "->" + mMultiComplianceList.Item(index).CompInfo + "->" + mMultiComplianceList.Item(index).MonitorType + "->" + mMultiComplianceList.Item(index).ATA + "->" + mMultiComplianceList.Item(index).Description

                        SaveCompMonitorModStatus(mCompMonitorModStatus, mMultiComplianceList.Item(index), LicenceNo, EmployeeID, EmployeeName, Trim(txtRemark.Text), ActualManHrs)

                        Session("MaintenanceActivityTypeID") = 10
                    End If
            End Select
        Next
        SetCaption()
        SetSession()

        If IsSavedSuccessfully = True Then
            If IsCompliedOnSameDate = True Then
                MSGBoxCtrl.show("Successful!!", "Multiple Compliances has been done successfully! Some of Compliances done on Same Date or less than last Compliance date Or greater than today date", "", MsgBoxStyle.OkOnly, "Successfull")
            Else
                MSGBoxCtrl.show("Successful!!", "Multiple Compliances has been done successfully!", "", MsgBoxStyle.OkOnly, "Successfull")
            End If
        Else
            MSGBoxCtrl.show("Failed!!", "Multiple Compliances has been failed!", "Please verify again", MsgBoxStyle.OkOnly, "")
        End If
        upnlDet.Update()
        upnlGrid.Update()
        upnlResult.Update()
    End Sub

    Private Sub btnAddMore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMore.Click, btnAddMoreTop.Click
        mMachineList = Nothing
        mAssemblyStatusList = Nothing
        ' Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        mMachineList = Nothing
        mAssemblyStatusList = Nothing
        RemoveSession()
        Session.Remove("mMultiComplianceList")

        'Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Response.Redirect("index.aspx")
    End Sub

    Private Sub dgMultiComplianceList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgMultiComplianceList.RowDataBound
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
            mMultiComplianceList = Session("mMultiComplianceList")
            Dim mMachine As Machine = Machine.GetMachine(mMultiComplianceList(ID).MachineID)

            Dim grdLinkActivity As GridView = DirectCast(e.Row.FindControl("grdLinkActivity"), GridView)

            Select Case mMultiComplianceList(ID).MaintenanceActivity()
                Case MaintenanceActivityTypes.AssemblyService
                    Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mMultiComplianceList.Item(ID).AssemblyMonitorServiceStatusID, mMultiComplianceList.Item(ID).AssemblyStatusID, mMachine.HourType)
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorServiceStatus.ModelMonitorServiceID.ToString)

                Case MaintenanceActivityTypes.AssemblyInspection   '6. Assembly Inspection
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mMultiComplianceList.Item(ID).AssemblyMonitorInspStatusID, mMultiComplianceList.Item(ID).AssemblyStatusID, mMachine.HourType)
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorInspStatus.ModelMonitorInspID.ToString)

                Case MaintenanceActivityTypes.AssemblyDirective    '7. Assembly Directive
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mMultiComplianceList.Item(ID).AssemblyMonitorDirectiveStatusID, mMultiComplianceList.Item(ID).AssemblyStatusID, mMachine.HourType)
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorModStatus.ModelMonitorModID.ToString)
                Case MaintenanceActivityTypes.ComponentService  '8. Comp Service
                    Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mMultiComplianceList.Item(ID).CompMonitorServiceStatusID, mMultiComplianceList.Item(ID).AssemblyStatusID, mMultiComplianceList.Item(ID).CompStatusID, mMachine.HourType)
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorServiceStatus.PartMonitorServiceID.ToString)
                Case MaintenanceActivityTypes.ComponentInspection   '9. Component Inspection
                    Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mMultiComplianceList.Item(ID).CompMonitorInspStatusID, mMultiComplianceList.Item(ID).AssemblyStatusID, mMultiComplianceList.Item(ID).CompStatusID, mMachine.HourType)
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorInspStatus.PartMonitorInspID.ToString)
                Case MaintenanceActivityTypes.ComponentDirective    '10. Component Directive
                    Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mMultiComplianceList.Item(ID).CompMonitorDirectiveStatusID, mMultiComplianceList.Item(ID).AssemblyStatusID, mMultiComplianceList.Item(ID).CompStatusID, mMachine.HourType)
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorModStatus.PartMonitorModID.ToString)

            End Select

            If mLinkMaintenanceList.Count > 0 Then
                e.Row.Cells(0).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
            End If
            grdLinkActivity.DataSource = mLinkMaintenanceList
            grdLinkActivity.DataBind()

        End If
    End Sub
    Private Sub dgMultiComplianceList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMultiComplianceList.Sorting
        mMultiComplianceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMultiComplianceList") = mMultiComplianceList
        dgMultiComplianceList.DataSource = mMultiComplianceList
        dgMultiComplianceList.DataBind()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        Dim mMaintenanceID As Guid
        Dim mMaintenanceDoneByEmployees As MaintenanceDoneByEmployees
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")

        mMaintenanceID = Session("mMaintenanceID") 'mMaintenanceDoneByEmployees(0).MaintenanceID
        Session.Remove("mMaintenanceID")
        ' If mMaintenanceDoneByEmployees.Count > 0 Then

        Dim hdnLicenceNo As HiddenField
        Dim hdnEmployeeID As HiddenField
        Dim hdnEmployeeName As HiddenField
        Dim hdnActualManHrs As HiddenField

        Dim LicenceNo As String = String.Empty
        Dim EmployeeID As String = String.Empty
        Dim EmployeeName As String = String.Empty
        Dim ActualManHrs As String = String.Empty
        Dim txtLicenceNo, txtActualManHrs As TextBox
        Dim lblLicenceCount As Label



        For j As Integer = 0 To mMultiComplianceList.Count - 1
            If mMultiComplianceList(j).ID = mMaintenanceID Then
                For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
                    If LicenceNo = "" Then
                        LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                        EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID.ToString
                        EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
                        ActualManHrs = mMaintenanceDoneByEmployees(i).RequiredManHours
                    Else
                        LicenceNo = LicenceNo + "," + mMaintenanceDoneByEmployees(i).LicenceNo
                        EmployeeID = EmployeeID + "," + mMaintenanceDoneByEmployees(i).EmployeeID.ToString
                        EmployeeName = EmployeeName + "," + mMaintenanceDoneByEmployees(i).EmployeeName
                        ActualManHrs = ActualManHrs + "," + mMaintenanceDoneByEmployees(i).RequiredManHours
                    End If

                Next

                hdnLicenceNo = CType(Me.dgMultiComplianceList.Rows(j).FindControl("hdnLicenceNo"), HiddenField)
                hdnEmployeeID = CType(Me.dgMultiComplianceList.Rows(j).FindControl("hdnEmployeeID"), HiddenField)
                hdnEmployeeName = CType(Me.dgMultiComplianceList.Rows(j).FindControl("hdnEmployeeName"), HiddenField)
                hdnActualManHrs = CType(Me.dgMultiComplianceList.Rows(j).FindControl("hdnActualManHrs"), HiddenField)

                txtLicenceNo = CType(Me.dgMultiComplianceList.Rows(j).FindControl("txtLicenceNo"), TextBox)
                txtActualManHrs = CType(Me.dgMultiComplianceList.Rows(j).FindControl("txtActualManHrs"), TextBox)
                lblLicenceCount = CType(Me.dgMultiComplianceList.Rows(j).FindControl("lblLicenceCount"), Label)

                hdnLicenceNo.Value = LicenceNo
                hdnEmployeeID.Value = EmployeeID
                hdnEmployeeName.Value = EmployeeName
                hdnActualManHrs.Value = ActualManHrs
                Dim mTotalHrs1 As Decimal
                If mMaintenanceDoneByEmployees.Count > 0 Then
                    For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mMaintenanceDoneByEmployees
                        mTotalHrs1 = mTotalHrs1 + New Period(1, mMaintenanceDoneByEmployee.RequiredManHours, 0, True, False).DbValueDec
                    Next
                    txtLicenceNo.Text = mMaintenanceDoneByEmployees(0).LicenceNo + " [" + mMaintenanceDoneByEmployees(0).EmployeeName + "]"
                    txtActualManHrs.Text = New Period(1, mTotalHrs1, 0, True, False).Value

                Else
                    txtLicenceNo.Text = String.Empty
                    'txtActualManHrs.Text = String.Empty
                End If
                txtLicenceNo.DataBind()
                txtActualManHrs.DataBind()

                If mMaintenanceDoneByEmployees.Count > 1 Then
                    lblLicenceCount.Text = "and " + (mMaintenanceDoneByEmployees.Count - 1).ToString + " more"
                    lblLicenceCount.ToolTip = LicenceNo
                    txtActualManHrs.Enabled = False
                Else
                    txtActualManHrs.Enabled = True
                End If
                lblLicenceCount.DataBind()
                lblLicenceCount.Visible = mMaintenanceDoneByEmployees.Count > 1

                Exit For
            End If
        Next

        upnlGrid.Update()


    End Sub
    'End
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        Dim txtLicenceNo, txtActualManHrs As TextBox
        Dim lblLicenceCount As Label
        Dim hdnLicenceNo As HiddenField
        Dim hdnEmployeeID As HiddenField
        Dim hdnEmployeeName As HiddenField
        Dim hdnActualManHrs As HiddenField

        Dim EmpName() As String
        Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent

        Dim Licences() As String
        Dim EmpID() As String
        Dim ActManHrs() As String

        txtLicenceNo = CType(currentRow.FindControl("txtLicenceNo"), TextBox)
        lblLicenceCount = CType(currentRow.FindControl("lblLicenceCount"), Label)
        hdnLicenceNo = CType(currentRow.FindControl("hdnLicenceNo"), HiddenField)
        hdnEmployeeID = CType(currentRow.FindControl("hdnEmployeeID"), HiddenField)
        hdnEmployeeName = CType(currentRow.FindControl("hdnEmployeeName"), HiddenField)
        hdnActualManHrs = CType(currentRow.FindControl("hdnActualManHrs"), HiddenField)
        txtActualManHrs = CType(currentRow.FindControl("txtActualManHrs"), TextBox)
        If txtActualManHrs.Text <> "" AndAlso Not hdnActualManHrs.Value.Split(",").Length > 1 Then
            hdnActualManHrs.Value = txtActualManHrs.Text
        End If


        LicenseNo = ""
        EmployeeName = ""
        EmployeeID = ""

        Dim mActManHrsInDec As Decimal

        If txtLicenceNo.Text = "" Then 'used when record deleted by backspace in txtLicenceNo
            Licences = hdnLicenceNo.Value.Split(",")
            EmpName = hdnEmployeeName.Value.Split(",")
            EmpID = hdnEmployeeID.Value.Split(",")
            ActManHrs = hdnActualManHrs.Value.Split(",")

            For i As Integer = 1 To Licences.Length - 1
                If LicenseNo = "" Then
                    LicenseNo = Licences(i)
                    EmployeeName = EmpName(i)
                    EmployeeID = EmpID(i)
                    ActualManHrs = ActManHrs(i)
                    mActManHrsInDec = mActManHrsInDec + New Period(1, ActManHrs(i), 0, True, False).DbValueDec
                Else
                    LicenseNo = LicenseNo + "," + Licences(i)
                    EmployeeName = EmployeeName + "," + EmpName(i)
                    EmployeeID = EmployeeID + "," + EmpID(i)
                    ActualManHrs = ActualManHrs + "," + ActManHrs(i)
                    mActManHrsInDec = mActManHrsInDec + New Period(1, ActManHrs(i), 0, True, False).DbValueDec
                End If

            Next
            hdnLicenceNo.Value = LicenseNo
            hdnEmployeeName.Value = EmployeeName
            hdnEmployeeID.Value = EmployeeID
            hdnActualManHrs.Value = ActualManHrs

            Licences = hdnLicenceNo.Value.Split(",")
            EmpName = hdnEmployeeName.Value.Split(",")
            EmpID = hdnEmployeeID.Value.Split(",")
            ActManHrs = hdnActualManHrs.Value.Split(",")

            If LicenseNo <> "" Then txtLicenceNo.Text = Licences(0) + " [" + EmpName(0) + "]"
            txtLicenceNo.DataBind()
            txtActualManHrs.Text = New Period(1, mActManHrsInDec, 0, True, False).Value
            txtActualManHrs.DataBind()

            If Licences.Length > 1 Then
                lblLicenceCount.Text = "and " + (Licences.Length - 1).ToString + " more"
                lblLicenceCount.ToolTip = LicenseNo
            End If
            lblLicenceCount.DataBind()
            lblLicenceCount.Visible = Licences.Length > 1
        End If

        upnlGrid.Update()
    End Sub
#End Region

#Region " Link Maintenance "

#Region " Variable Declaration "

    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance
    Public mMultiComplianceLinkList As New MultiComplianceList
    Public mAssemblyMonitorServiceStatusForLM As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorInspStatusForLM As AssemblyMonitorInspStatus
    Public mAssemblyMonitorModStatusForLM As AssemblyMonitorModStatus
    Public mLinkMaintenanceMonitorStatus As LinkMaintenaceMonitorStatus
    Public PeriodValues(,) As String
    Dim message As String = ""
    Dim mDetail As String = ""
#End Region

    Private Sub LinkMaintenance(MaintenanceActivityID As Guid, mMachine As Machine, Detail As String, DoneWONo As String, AssemblyId As Guid, MaintenanceActivity As String, mMachineMaintenance As MachineMaintenance, DoneOnDate As String, ByVal DoneRemark As String, Optional LicenceNo As String = "", Optional ByVal EmployeeID As String = "", Optional ByVal EmployeeName As String = "")
        If AppSettings("LinkMaintenance") = "True" Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(MaintenanceActivityID.ToString)
            Session("mLinkMaintenanceList") = mLinkMaintenanceList
            If mLinkMaintenanceList.Count > 0 Then

                ShowLinkedMaintenaceActivity(mMachine, DoneOnDate, AssemblyId)

                'Save Link Activities
                If Not mMultiComplianceLinkList Is Nothing Then
                    If mMultiComplianceLinkList.Count > 0 Then
                        Dim Result As Boolean
                        SetLinkGridObject()
                        Dim LinkMaintenanceEvents As LinkedMaintenanceActivityEvents = New LinkedMaintenanceActivityEvents
                        LinkMaintenanceEvents.AssemblyLogInfo = MaintenanceActivity & ": " & Detail 'setting Mark Log Detail ...
                        Result = LinkMaintenanceEvents.SaveLinkedMaintenanceActivies(mMultiComplianceLinkList, DoneWONo, DoneOnDate, mMachineMaintenance.LogID, mMachine.HourType, mMachine.ID, AssemblyId, PeriodValues, DoneRemark, LicenceNo, EmployeeID, EmployeeName, isFromMulticomplianceForm:=True)

                        If LinkMaintenanceEvents.ErrorStr.Length > 0 Then
                            Dim title As String = "Link Maintenance Alert !"
                            Dim message As String = LinkMaintenanceEvents.ErrorStr
                            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                        End If
                        Session.Remove("mMultiComplianceLinkList")
                        mMultiComplianceLinkList = Nothing
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ShowLinkedMaintenaceActivity(mMachine As Machine, DoneOnDate As String, AssemblyID As Guid)

        mMultiComplianceLinkList = New MultiComplianceList

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

                    mAssemblyMonitorServiceStatusForLM = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusForLinkMaintenance(mPrevAssemblyMonitorSeviceStatus.ID, mPrevAssemblyMonitorSeviceStatus.AssemblyStatusID, DoneOnDate, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

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
                    mMultiComplianceLinkList.Add(mAssemblyMonitorServiceStatusForLM.ID, MaintenanceActivityTypes.AssemblyService, True, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Reference, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.MonitorTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelMonitorServiceTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Description, mAssemblyMonitorServiceStatusForLM.DoneOn.ToString, mAssemblyMonitorServiceStatusForLM.DoneWONo, mAssemblyMonitorServiceStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ATAChapter, , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 2 'Assembly Inspection

                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorInspStatusForLM = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusForLinkMaintenance(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, DoneOnDate, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

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
                    mMultiComplianceLinkList.Add(mAssemblyMonitorInspStatusForLM.ID, MaintenanceActivityTypes.AssemblyInspection, True, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Reference, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.MonitorTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelMonitorInspTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Description, mAssemblyMonitorInspStatusForLM.DoneOn.ToString, mAssemblyMonitorInspStatusForLM.DoneWONo, mAssemblyMonitorInspStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ATAChapter, , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 3 'Assembly Directive
                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorModStatusForLM = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusForLinkMaintenance(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, DoneOnDate, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

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
                    mMultiComplianceLinkList.Add(mAssemblyMonitorModStatusForLM.ID, MaintenanceActivityTypes.AssemblyDirective, True, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Reference, mAssemblyMonitorModStatusForLM.ModelMonitorMod.MonitorTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelMonitorModTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Description, mAssemblyMonitorModStatusForLM.DoneOn.ToString, mAssemblyMonitorModStatusForLM.DoneWONo, mAssemblyMonitorModStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ATAChapter, , , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing
            End Select
        Next
        Session("mMultiComplianceLinkList") = mMultiComplianceLinkList
    End Sub

    Public Sub SetLinkGridObject()
        Dim j As Int32

        ReDim PeriodValues(dgDoneOnValue.Rows.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)

        For j = 0 To Me.dgDoneOnValue.Rows.Count - 1

            PeriodValues(j, 0) = Me.dgDoneOnValue.Rows(j).Cells(0).Text 'To Check same Period
            PeriodValues(j, 1) = Me.dgDoneOnValue.Rows(j).Cells(1).Text 'Period Value 
        Next j

    End Sub

#Region " View Link Activity "


#End Region
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

#End Region
End Class