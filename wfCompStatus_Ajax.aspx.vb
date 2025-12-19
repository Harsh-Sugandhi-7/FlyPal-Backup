'AJAX Conversion by vikrant on 02-Jun-2015
Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.PartListAutoComplete

Public Class wfCompStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Install Component "
#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mPeriodListForCompStatus As PeriodListForCompStatus
    Public mPartlist As PartList
    Public mSelectPeriods As SelectPeriods
    Public mATAList As ATAList
    Private Flag As Int16
    Public mCompDetail As String     'Code Added 30,Jan,2007
    Public mtmpInstalledCompList As tmpInstalledCompList  'AC
    Dim index As Int32   'AC

    Dim EventLogID As Guid      'Added By Utkarsh On 1-Aug-2011 For All19072011
    Dim MachineDetail As String 'Added By Utkarsh On 1-Aug-2011 For All19072011
    Public mEmployeeList As EmployeeList
    Public mManufacturerList As ManufacturerList 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
    Dim mInstallationStatusList As InstallationStatusList
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty

    Public mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
    Public mPartMonitorServiceTypeList As PartMonitorServiceTypeList
    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Public mMachineMaintenance As MachineMaintenance
    Public SearchFor As String = String.Empty

    Public mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
    Public mPartMonitorInspTypeList As PartMonitorInspTypeList
    Public mCompMonitorInspStatus As CompMonitorInspStatus

    Public mCompMonitorModStatusList As tmpCompMonitorModStatusList
    Public mPartMonitorModTypeList As PartMonitorModTypeList
    Public mCompMonitorModStatus As CompMonitorModStatus
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End

    Dim mFirstThrustCompStatus As FirstThrustCompStatus 'Added by Saylee on 7-Oct-2017 for Thrust
    Public IsSLLExists As Boolean
    Dim mThrustTypeList As ThrustTypeList 'Added by Saylee on 25-May-2018 for Thrust
    Public mIsSpareAssembly As Integer 'Added By Saylee On 27-Jul-2020 For ALL27072020
#End Region

#Region " Enum "
    Public Enum From
        FromMaster = 0
        FromInstallAssembly = 1
    End Enum
    Public Enum MaintActivityTypeID
        AssemblyInstallation = 1
        AssemblyRemoval = 2
        ComponentInstallation = 3
        ComponentRemoval = 4
        AssemblyService = 5
        AssemblyInspection = 6
        AssemblyDirective = 7
        ComponentService = 8
        ComponentInspection = 9
        ComponentModification = 10
    End Enum
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus) 'required to retrieve the AssemblyStatusID to the future forms
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mPeriodListForCompStatus = CType(Session("mPeriodListForCompStatus"), PeriodListForCompStatus)
        mPartlist = CType(Session("mPartlist"), PartList)
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        mATAList = CType(Session("mATAList"), ATAList)
        mtmpInstalledCompList = CType(Session("mtmpInstalledCompList"), tmpInstalledCompList)
        mManufacturerList = Session("mManufacturerList") 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        mInstallationStatusList = Session("mInstallationStatusList")
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mFirstThrustCompStatus = Session("mFirstThrustCompStatus") 'Added by Saylee on 7-Oct-2017 for Thrust
        mThrustTypeList = Session("mThrustTypeList") 'Added by Saylee on 25-May-2018 for Thrust
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added By Saylee On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
        Session("mPartlist") = mPartlist
        Session("mSelectPeriods") = mSelectPeriods
        Session("mATAList") = mATAList
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        Session("mManufacturerList") = mManufacturerList 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        Session("mInstallationStatusList") = mInstallationStatusList
        Session("mFirstThrustCompStatus") = mFirstThrustCompStatus 'Added by Saylee on 7-Oct-2017 for Thrust
        Session("mThrustTypeList") = mThrustTypeList 'Added by Saylee on 25-May-2018 for Thrust
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPeriodListForCompStatus")
        Session.Remove("mPartlist")
        Session.Remove("mSelectPeriods")
        Session.Remove("mATAList")
        Session.Remove("mManufacturerList") 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        Session.Remove("mInstallationStatusList")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
        Session.Remove("mFirstThrustCompStatus")  'Added by Saylee on 7-Oct-2017 for Thrust
        Session.Remove("mThrustTypeList")  'Added by Saylee on 25-May-2018 for Thrust
    End Sub

    Private Sub NewRecord()
        mCompStatus = CompStatus.NewCompStatus(Guid.NewGuid, mCompStatus.AssemblyID, mAssemblyStatus.AsOnDate, mMachine.HourType)
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub MessageBoxResult()
        Dim msgCount As Integer = 0
        Dim Mtype As String
        Dim id As Guid
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "ReqServ" Then
                        Dim URLForCompInst As New Stack
                        URLForCompInst.Push(Request.Url)
                        Session("URLForCompInst") = URLForCompInst
                        Session("CompInstTabIndex") = TbContInst.ActiveTabIndex
                        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
                        If mComponentMaintananceListCount.MaintenanceServiceListCount = 0 And mComponentMaintananceListCount.MaintenanceInspListCount > 0 Then
                            NewRecordInspection()
                        Else
                            NewRecordService()
                        End If

                    ElseIf MSGBoxCtrl.Sender = "DeleteService" Then
                        Try
                            Session("sender") = ""
                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatusList.CurrentItem.ID, 8)
                            '=============================
                            'Added By Utkarsh On 1-Aug-2011 For All19072011
                            Mtype = mCompMonitorServiceStatusList(mCompMonitorServiceStatusList.CurrentIndex).PartMonitorServiceTypeName
                            id = CType(mCompMonitorServiceStatusList(mCompMonitorServiceStatusList.CurrentIndex).ID, Guid)
                            'End

                            If mCompMonitorServiceStatusList(mCompMonitorServiceStatusList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatusList(mCompMonitorServiceStatusList.CurrentIndex).ID)
                            End If

                            CompMonitorServiceStatus.DeleteCompMonitorServiceStatus(mCompMonitorServiceStatusList.CurrentItem.ID)

                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            Session("mMachineMaintenance") = mMachineMaintenance
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            DataFieldBindService()
                            SetControlsService()
                            SetPageService()
                            SetGridService()
                            ControlVisibilityService()
                            upnlService.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Changed By Utkarsh On 1-Aug-2011 For All19072011
                                MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & Mtype
                                MarkLog(Util.Action.Delete, "Component Service Status", "Can't delete : " & MachineDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                                'End
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 1-Aug-2011 For All19072011
                                MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & Mtype
                                MarkLog(Util.Action.Delete, "Component Service Status", MachineDetail, Util.ErrorType.NoError, id, EventLogID)
                                'End
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteInspection" Then
                        Try
                            Session("sender") = ""
                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorInspStatusList.CurrentItem.ID, 9)
                            '=============================
                            If mCompMonitorInspStatusList(mCompMonitorInspStatusList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mCompMonitorInspStatusList(mCompMonitorInspStatusList.CurrentIndex).ID)
                            End If
                            CompMonitorInspStatus.DeleteCompMonitorInspStatus(mCompMonitorInspStatusList.CurrentItem.ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            Session("mMachineMaintenance") = mMachineMaintenance
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            FindNowInspection()
                            SetPageInspection()
                            ControlVisibilityInspection()
                            upnlInspection.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Component Inspection Status", "Can't delete : " + "Reg No. : " + mMachine.RegNo & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Mod Type : " + mCompMonitorInspStatusList.Item(mCompMonitorInspStatusList.CurrentIndex).MonitorType + " Description : " + mCompMonitorInspStatusList.Item(mCompMonitorInspStatusList.CurrentIndex).Description + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count

                        End Try

                    ElseIf MSGBoxCtrl.Sender = "DeleteModification" Then

                        Try
                            Session("sender") = ""
                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatusList.CurrentItem.ID, 10)
                            '=============================
                            If mCompMonitorModStatusList(mCompMonitorModStatusList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatusList(mCompMonitorModStatusList.CurrentIndex).ID)
                            End If
                            CompMonitorModStatus.DeleteCompMonitorModStatus(mCompMonitorModStatusList.CurrentItem.ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            Session("mMachineMaintenance") = mMachineMaintenance
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            FindNowModification()
                            SetPageModification()
                            ControlVisibilityModification()
                            upnlModification.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Component Mod Status", "Can't delete : " + "Reg No. : " + mMachine.RegNo & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Mod Type : " + mCompMonitorModStatusList.Item(mCompMonitorModStatusList.CurrentIndex).MonitorType + " Description : " + mCompMonitorModStatusList.Item(mCompMonitorModStatusList.CurrentIndex).Description + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        End Try

                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                    Else
                        Session("sender") = ""
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "ReqServ" Then
                        Session("sender") = ""
                        'Changed By Utkarsh On 1-Aug-2011 For All19072011
                        MarkLog(Util.Action.Close, "Assembly Component Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        'End
                        RemoveSession()
                        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    End If
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "ReqServ" Then
                        Session("sender") = ""
                    End If
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
    Private Sub SetObject()
        mCompStatus.Comp.Code = CInt(Trim(txtCode.Text))
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
            mCompStatus.Comp.PartID = mPartlist(txtPartDescription.Text.Trim).ID
        Else
            mCompStatus.Comp.PartID = New Guid(cmbPartNo.SelectedValue.ToString)
        End If
        mCompStatus.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mCompStatus.Comp.SerialNo = Trim(txtSerialNo.Text)
        'Code Added By Deven on 14-11-2008 For Accumulated Cycles calculation
        mCompStatus.Comp.ACF = IIf(txtACF.Text <> "", CDec(txtACF.Text), 0)
        mCompStatus.Comp.ECF = IIf(txtECF.Text <> "", CDec(txtECF.Text), 0)
        mCompStatus.Comp.FCF = IIf(txtFCF.Text <> "", CDec(txtFCF.Text), 0)
        '********************************************************************

        mCompStatus.Comp.RTCF = IIf(txtRTCF.Text <> "", CDec(txtRTCF.Text), 0) ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor

        mCompStatus.Position = Trim(txtPostion.Text)
        If Not IsDate(txtInstalledOnDate.Text) Then
            mCompStatus.InstalledOn = System.DBNull.Value
        Else
            mCompStatus.InstalledOn = txtInstalledOnDate.Text
        End If
        mCompStatus.InstallationWONo = Trim(txtWorkOrderNo.Text)
        mCompStatus.InstallationRemark = Trim(txtRemark.Text)
        mCompStatus.SourceDoc = Trim(txtSourceDoc.Text)
        mCompStatus.RevisionNo = Trim(txtRevisionNo.Text)
        mCompStatus.BookNo = Trim(txtBookNo.Text)
        mCompStatus.PageNo = Trim(txtPageNo.Text)
        'mCompStatus.InstDoneByID = New Guid(cmbDoneBy.SelectedValue)
        'mCompStatus.InstLicenseNo = txtLicenceNo.Text.Trim
        'mCompStatus.InstPlace = txtPlace.Text.Trim

        'Added By Prashant On 12-Jun-2012 FOR ALL08062012
        Dim LicenseNo As String = String.Empty
        Dim EmpName As String = String.Empty
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        mCompStatus.InstLicenseNo = LicenseNo
        mCompStatus.InstDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        mCompStatus.InstPlace = txtPlace.Text.Trim
        'End

        'Added By Saylee on 24-Apr-2009
        mCompStatus.InstDoneBy = Trim(txtInstDoneBy.Text)
        '==================================
        mCompStatus.ManufacturerID = New Guid(cmbManufacturerList.SelectedValue) 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        mCompStatus.InstallationReason = Trim(txtInstallationReason.Text) 'Added By Vikrant On 09-Apr-2014 For ALL09042014-1
        mCompStatus.InstallationStatusID = CInt(cmbInstallationStatus.SelectedValue) 'Added By Vikrant On 31-Mar-2015 For All31032015

        'Added By Saylee on 6-Oct-2017 for Thrust
        mCompStatus.IsThrustMonitoringComp = chkIsThrustComp.Checked
        If chkIsThrustComp.Checked Then
            mCompStatus.B22CurrentValue = CDec(txtB22Current.Text)
            mCompStatus.B22LifeLimit = CDec(txtB22LifeLimit.Text)
            mCompStatus.B22IsCurrentThrust = chkB22IsCurrent.Checked

            mCompStatus.B24CurrentValue = CDec(txtB24Current.Text)
            mCompStatus.B24LifeLimit = CDec(txtB24LifeLimit.Text)
            mCompStatus.B24IsCurrentThrust = chkB24IsCurrent.Checked

            mCompStatus.B26CurrentValue = CDec(txtB26Current.Text)
            mCompStatus.B26LifeLimit = CDec(txtB26LifeLimit.Text)
            mCompStatus.B26IsCurrentThrust = chkB26IsCurrent.Checked

        End If
        '***********************************************
        'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        If chkFanBladeMonitoring.Checked Then
            mCompStatus.IsFanBladeDistribution = chkFanBladeMonitoring.Checked
            mCompStatus.FanBladePosition = Val(txtFanBladePosition.Text)
            mCompStatus.MomentWeight = CDec(txtMomentWeight.Text)
            mCompStatus.BalanceScrew = Val(txtBalanceScrew.Text)
        End If
        'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        Session("mCompStatus") = mCompStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtCurrentCompValue, txtCompInstallationValue, txtAssemblyInstallationValue As TextBox
        For i As Integer = 0 To mCompStatus.CompStatusPeriods.Count - 1

            txtCurrentCompValue = CType(Me.dgCurrentCompValue.Rows(i).FindControl("txtCurrentCompValue"), TextBox)
            txtCompInstallationValue = CType(Me.dgInstallationValues.Rows(i).FindControl("txtCompInstallationValue"), TextBox)
            txtAssemblyInstallationValue = CType(Me.dgInstallationValues.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)

            If mCompStatus.CompStatusPeriods(i).PeriodID = 2 And Not Period.IsDate(txtCurrentCompValue.Text) Then
                mCompStatus.CompStatusPeriods.Item(i).CompCurrentValueFormatted = ""
                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = Trim(txtCompInstallationValue.Text)
                mCompStatus.CompStatusPeriods.Item(i).AssemblyInstallationValueFormatted = Trim(txtAssemblyInstallationValue.Text)
            ElseIf mCompStatus.CompStatusPeriods(i).PeriodID = 2 And Not Period.IsDate(txtCompInstallationValue.Text) Then
                mCompStatus.CompStatusPeriods.Item(i).CompCurrentValueFormatted = txtCurrentCompValue.Text.Trim
                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = ""
                mCompStatus.CompStatusPeriods.Item(i).AssemblyInstallationValueFormatted = Trim(txtAssemblyInstallationValue.Text)
            ElseIf mCompStatus.CompStatusPeriods(i).PeriodID = 2 And Not Period.IsDate(txtAssemblyInstallationValue.Text) Then
                mCompStatus.CompStatusPeriods.Item(i).CompCurrentValueFormatted = txtCurrentCompValue.Text.Trim
                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = txtCompInstallationValue.Text.Trim
                mCompStatus.CompStatusPeriods.Item(i).AssemblyInstallationValueFormatted = ""
            Else
                mCompStatus.CompStatusPeriods.Item(i).CompCurrentValueFormatted = Trim(txtCurrentCompValue.Text)
                If mCompStatus.CompStatusPeriods.Item(i).PeriodID <> 2 And txtCompInstallationValue.Text.Trim = "" Then 'This If Condition added by vikrant on 19-Jun-2020 to save 0 instead of null if nothing enetered in TextBox
                    mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = New Period(mCompStatus.CompStatusPeriods.Item(i).PeriodID, 0).Value
                Else
                    mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = Trim(txtCompInstallationValue.Text)
                End If
                mCompStatus.CompStatusPeriods.Item(i).AssemblyInstallationValueFormatted = Trim(txtAssemblyInstallationValue.Text)
            End If
        Next i
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub AddSelectedPeroids()
        Dim mSelectPeriod As SelectPeriod
        If IsNothing(mSelectPeriods) Then
            mSelectPeriods = SelectPeriods.NewSelectPeriods
        End If
        For Each mSelectPeriod In mSelectPeriods
            If mSelectPeriod.IsSelected Then
                mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewChildCompStatusPeriod(mCompStatus.ID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mSelectPeriod.PeriodID, txtInstalledOnDate.Text))
            End If
        Next
        Session("mCompStatus") = mCompStatus
        Session.Remove("mSelectPeriods")
        mSelectPeriods = Nothing
    End Sub
    Private Sub SetPeroids()
        Dim mPeriodlist As PeriodList
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        mPeriodlist = PeriodList.GetPeriodList
        For i As Integer = 0 To mPeriodListForCompStatus.Count - 1
            If Not mCompStatus.CompStatusPeriods.Contains(mPeriodListForCompStatus(i).PeriodID) Then
                mSelectPeriods.Add(mPeriodListForCompStatus(i).PeriodID, mPeriodListForCompStatus(i).PeriodName)
            End If
        Next
        Session("mSelectPeriods") = mSelectPeriods
    End Sub
    Private Sub SetPage()
        If mCompStatus.IsNew Then
            lblTitle.Text = "Component Status [New]"
        Else
            lblTitle.Text = "Component Status [Part: " & mCompStatus.PartName & " Serial No:" & mCompStatus.SerialNo & " ]"
        End If
        'Commented and Added By Vikrant On 31-Mar-2015 For All31032015
        'lblModuleTSNCaption.Text = "Since New Value as on " & mCompStatus.AsOnDateFormatted
        lblModuleTSNCaption.InnerText = cmbInstallationStatus.SelectedItem.ToString + " Value as on " & mCompStatus.AsOnDateFormatted
        'End

    End Sub
    Private Sub ControlVisibility()
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            txtPartDescription.Visible = True
            cmbPartNo.Visible = False
        Else
            cmbPartNo.Visible = True
            txtPartDescription.Visible = False
        End If

        'Added by Saylee on 7-Oct-2017 for Thrust
        If mAssemblyStatus.AssemblyTypeID = 2 And mCompStatus.CompStatusPeriods.Contains(3) And AppSettings("ShowThrustMonitoring") = True Then
            IsSLLExists = mCompStatus.IsSLLServiceExists
            Label1.Visible = True
            upnlIsThrustComp.Visible = True
            'upnlIsThrustComp.Update()
            If chkIsThrustComp.Checked Then
                pnlThrustyComponentDet.Visible = True

                mThrustTypeList = ThrustTypeList.GetThrustTypeList()
                Session("mThrustTypeList") = mThrustTypeList

                lblB22.InnerText = mThrustTypeList(0).Name
                lblB24.InnerText = mThrustTypeList(1).Name
                lblB26.InnerText = mThrustTypeList(2).Name

                mFirstThrustCompStatus = Session("mFirstThrustCompStatus")

                If mFirstThrustCompStatus Is Nothing Then
                    mFirstThrustCompStatus = FirstThrustCompStatus.GetFirstThrustCompStatusList(mAssemblyStatus.AssemblyID)
                    Session("mFirstThrustCompStatus") = mFirstThrustCompStatus
                End If

                If Not mFirstThrustCompStatus Is Nothing And mFirstThrustCompStatus.Count > 0 Then
                    chkB22IsCurrent.Enabled = False
                    chkB24IsCurrent.Enabled = False
                    chkB26IsCurrent.Enabled = False
                Else
                    chkB22IsCurrent.Enabled = Not IsSLLExists Or mCompStatus.IsNew
                    chkB24IsCurrent.Enabled = Not IsSLLExists Or mCompStatus.IsNew
                    chkB26IsCurrent.Enabled = Not IsSLLExists Or mCompStatus.IsNew
                End If

                txtB22Current.Enabled = Not IsSLLExists Or mCompStatus.IsNew
                txtB24Current.Enabled = Not IsSLLExists Or mCompStatus.IsNew
                txtB26Current.Enabled = Not IsSLLExists Or mCompStatus.IsNew

                txtB22LifeLimit.Enabled = Not IsSLLExists Or mCompStatus.IsNew
                txtB24LifeLimit.Enabled = Not IsSLLExists Or mCompStatus.IsNew
                txtB26LifeLimit.Enabled = Not IsSLLExists Or mCompStatus.IsNew



            Else
                pnlThrustyComponentDet.Visible = False

            End If
            chkIsThrustComp.Enabled = Not IsSLLExists
            pnlThrustyComponentOnOFF.Enabled = Not IsSLLExists Or mCompStatus.IsNew
            upnlThrustyComponentDet.Update()
            chkIsThrustComp.DataBind()
            upnlIsThrustCompOuter.Update()
        Else
            pnlThrustyComponentDet.Visible = False
            Label1.Visible = False
            upnlIsThrustComp.Visible = False
            upnlIsThrustCompOuter.Update()
            upnlThrustyComponentDet.Update()
        End If
        '*******************************************************
        'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394 
        If mAssemblyStatus.AssemblyTypeID = 2 And AppSettings("ShowFanBladeDistributionMonitoring") = "True" Then
            FieldsetFanBladeDistribution.Visible = True
            If chkFanBladeMonitoring.Checked = True Then
                txtFanBladePosition.Enabled = True
                txtMomentWeight.Enabled = True
                txtBalanceScrew.Enabled = True
            Else
                txtFanBladePosition.Enabled = False
                txtMomentWeight.Enabled = False
                txtBalanceScrew.Enabled = False
            End If
            upnlIsFanBladeDistribution.Update()
            chkFanBladeMonitoring.DataBind()
        Else
            FieldsetFanBladeDistribution.Visible = False
            upnlIsFanBladeDistribution.Update()
        End If
        'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394 

        'Added by Saylee on 24-apr-2023
        Dim lblServiceTitle As Label

        lblServiceTitle = TbContInst.Tabs(1).FindControl("lblServiceListTitle")
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then

            ' tbPnlServiceList.HeaderTemplate = "MPD List"
            lblServiceTitle.Text = "Maintenance Event(s)"
            TbContInst.Tabs(2).Visible = False
            dgMonitorServiceStatusList.Columns(3).Visible = True
            If Not cmbLookInService.Items.Contains(New ListItem("Task Type", "2")) Then
                cmbLookInService.Items.Add(New ListItem("Task Type", "2"))
                cmbLookInService.Items.Add(New ListItem("Work Order No.", "3"))
            End If

        Else

            'tbPnlServiceList.HeaderTemplate = "Service List"
            lblServiceTitle.Text = "Service(s)"
            TbContInst.Tabs(2).Visible = Not (mCompStatus.IsNew)
            dgMonitorServiceStatusList.Columns(3).Visible = False
            If Not cmbLookInService.Items.Contains(New ListItem("Service Type", "2")) Then
                cmbLookInService.Items.Add(New ListItem("Service Type", "2"))
                cmbLookInService.Items.Add(New ListItem("Work Order No.", "3"))
            End If
        End If
        '**************************
    End Sub
    Private Sub ControlVisiblity1() 'Added By Prashant 26-Aug-2010
        If mCompStatus.CompStatusPeriods.Count > 0 Then
            'For p = 0 To mCompStatus.CompStatusPeriods.Count - 1
            '    If (mCompStatus.CompStatusPeriods.Item(p).PeriodID = 9 Or mCompStatus.CompStatusPeriods.Item(p).PeriodID = 10) Then
            '        lblACF.Visible = True
            '        txtACF.Visible = True
            '        lblECF.Visible = True
            '        txtECF.Visible = True
            '        lblFCF.Visible = True
            '        txtFCF.Visible = True
            '    Else
            '        lblACF.Visible = False
            '        txtACF.Visible = False
            '        lblECF.Visible = False
            '        txtECF.Visible = False
            '        lblFCF.Visible = False
            '        txtFCF.Visible = False
            '    End If
            'Next
            'Check if PeriodID = 9 or PeriodID = 10 exists
            If mCompStatus.CompStatusPeriods.Contains(9) Or mCompStatus.CompStatusPeriods.Contains(10) Or mCompStatus.CompStatusPeriods.Contains(16) Then
                lblACF.Visible = True
                txtACF.Visible = True
                lblECF.Visible = True
                txtECF.Visible = True
                lblFCF.Visible = True
                txtFCF.Visible = True
                lblRTCF.Visible = True
                txtRTCF.Visible = True  ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor
            Else
                lblACF.Visible = False
                txtACF.Visible = False
                lblECF.Visible = False
                txtECF.Visible = False
                lblFCF.Visible = False
                txtFCF.Visible = False
                lblRTCF.Visible = False
                txtRTCF.Visible = False  ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor
            End If
        Else
            lblACF.Visible = False
            txtACF.Visible = False
            lblECF.Visible = False
            txtECF.Visible = False
            lblFCF.Visible = False
            txtFCF.Visible = False
            lblRTCF.Visible = False
            txtRTCF.Visible = False  ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor
        End If
        '-----------------------------
        'Added By Vikrant On 26-Jun-2014
        If Session("IsOpenFromMaster") = True And mAssemblyStatus.IsSpareAssembly = False Then
            imgHome.Visible = True
        End If
        'End
        btnPrint.Enabled = Not mCompStatus.IsNew
        ' If mCompStatus.IsThrustMonitoringComp Then lblThrustyComponentDet.Visible = True
        '    ControlVisibility()
    End Sub
    Private Sub SetPartNoDescription()
        PartNo = txtPartDescription.Text.Trim

        If mPartlist.Contains(PartNo) Then
            Description = mPartlist(PartNo).Description
        Else
            Description = ""
        End If
    End Sub
    'Added by Saylee on 19-Mar-2013 for ALL14032013-1
    Public Function CheckPeriodsForCompStatus(ByVal tmpCompStatus As CompStatus) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        While i <= tmpCompStatus.CompStatusPeriods.Count - 1
            If mAssemblyStatus.AssemblyStatusPeriods.Contains(tmpCompStatus.CompStatusPeriods(i).PeriodID) Then
                tmpIsPeriodExists = True
            Else
                tmpIsPeriodExists = False
                Exit While
            End If
            i = i + 1
        End While

        Return tmpIsPeriodExists
    End Function
    Public Function SumOfThrust(ByVal tmpCompStatus As CompStatus) As Boolean 'Added by Saylee on 7-Oct-2017 for Thrust
        Dim sum As Decimal
        sum = tmpCompStatus.B22CurrentValue + tmpCompStatus.B24CurrentValue + tmpCompStatus.B26CurrentValue

        If tmpCompStatus.CompStatusPeriods(3, "").CompCurrentValue <> "" Then
            If CDec(tmpCompStatus.CompStatusPeriods(3, "").CompCurrentValue) = sum Then
                Return True
            End If
        End If

        Return False
    End Function

    Private Function Save() As Boolean
        Dim CompStatusClone As CompStatus
        CompStatusClone = CType(mCompStatus, CompStatus)
        SetObject()
        SetGridObject()
        If mCompStatus.IsValid Then
            If mCompStatus.CompStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Component. Component can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
            If Not mCompStatus.InstDoneByID.Equals(Guid.Empty) AndAlso mCompStatus.InstalledOn.ToString.Length > 0 Then
                Dim Title As String = "Save Alert !"
                Dim Message As String = ""
                Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompStatus.InstDoneByID.ToString, mCompStatus.InstalledOn.ToString)
                If mEmployeeStatus(0).Information <> "" Then
                    Message = mEmployeeStatus(0).Information
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(Title, Message, , False), True)
                    Return False
                End If
            End If
            'End
            'Added by Saylee on 19-Mar-2013 for ALL14032013-1
            If CheckPeriodsForCompStatus(mCompStatus) = False Then
                MSGBoxCtrl.Show("Component Installation Alert!", "Periods for selected " & mCompStatus.PartNameSerialNo & " are mismatching with selected Installed On Assembly " & mAssemblyStatus.AssemblyTypeName & " .Can not be installed.", "", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            '***********************************

            'Added by Saylee on 9-Oct-2017 for Thrust Monitoring
            If mCompStatus.CompStatusPeriods.Contains(3) And mCompStatus.IsThrustMonitoringComp Then
                If SumOfThrust(mCompStatus) = False Then
                    MSGBoxCtrl.Show("Component Installation Alert!", "Summation of Thrust Monitoring values are mismatching with Current values. ", "", MsgBoxStyle.OkOnly, "")
                    Return False
                End If
            End If
            Dim ThrustLabels As String = lblB22.InnerText + " , " + lblB24.InnerText + " & " + lblB26.InnerText
            If mCompStatus.IsThrustMonitoringComp And (chkB22IsCurrent.Checked = False And chkB24IsCurrent.Checked = False And chkB26IsCurrent.Checked = False) Then
                MSGBoxCtrl.Show("Component Installation Alert!", "Thrust Monitoring (either Monitor with " + ThrustLabels + ") required.", "", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            '***********************************


            If mCompStatus.IsThrustMonitoringComp And (CDec(txtB22LifeLimit.Text) = 0 Or CDec(txtB24LifeLimit.Text) = 0 Or CDec(txtB24LifeLimit.Text) = 0) Then
                MSGBoxCtrl.Show("Component Installation Alert!", "Please Enter Life Limit for all " + ThrustLabels, "", MsgBoxStyle.OkOnly, "")
                Return False
            End If


            Try
                mCompStatus.ApplyEdit()
                mCompStatus = CType(mCompStatus.Save(), CompStatus)

                'Commented By Utkarsh On 1-Aug-2011 For All19072011

                '   mCompDetail = "ATAChapter -> " + mCompStatus.ATAChapter + " Part -> " + mCompStatus.PartNameSerialNo    'Code Added 30,Jan,2007
                '    MarkLog(Util.Action.Save, "CompStatus", mCompDetail, Util.ErrorType.NoError, mCompStatus.ID)            'Code Added 30,Jan,2007 
                'End

                Return True
            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
                ElseIf ex.Number = 50000 Then
                    NewRecord()
                    SetPartNoDescription()
                    GetCompStatusForPart(cmbPartNo.SelectedIndex)
                    SetObject()
                    SetGridObject()
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                    'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
                End If
                Return False
            Finally
                'Added by Saylee on 10-Feb-2020,  All27072020
                Dim mRegNo As String = ""
                If mAssemblyStatus.IsSpareAssembly = False Then
                    mRegNo = "Reg No. : " & mMachine.RegNo
                End If
                '***********************
                MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo
                MarkLog(Util.Action.Save, "Assembly Component Status", MachineDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function

    Private Sub GetCompStatusForPart(ByVal PartIndex As Integer) 'Added by Saylee on 25-Aug-2009
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        Dim SearchPartNo As String
        Dim PartID As Guid 'Added By Utkarsh On 09-May-2013 FOR ALL09052013-1
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            If PartNo.Length = 0 Then
                SearchPartNo = "(SELECT)"
                PartID = Guid.Empty 'Added By Utkarsh On 09-May-2013 FOR ALL09052013-1
            Else
                SearchPartNo = PartNo
                PartID = mPartlist(PartNo).ID 'Added By Utkarsh On 09-May-2013 FOR ALL09052013-1
            End If
        Else
            SearchPartNo = mPartlist(New Guid(cmbPartNo.SelectedValue)).Name
            PartID = New Guid(cmbPartNo.SelectedValue.ToString)
        End If

        'Commented and Added by Saylee on 17-Nov-2010
        ''Dim mtmpCompStatusList As tmpCompStatusList = tmpCompStatusList.GetCompStatusList(Guid.Empty, mPartlist(PartIndex).Name, "", mPartlist(PartIndex).Description)
        'Added By Utkarsh(cmbPartNo.SelectedValue.ToString Criteria) On 09-May-2013 FOR ALL09052013-1
        Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(PartID.ToString, SearchPartNo, Description)
        If mtmpCompListOnPartSelection.Count > 0 Then
            Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, mAssemblyStatus.ID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)

            txtCode.Text = tmpCompStatus.Comp.Code
            mCompStatus.Comp.PartID = tmpCompStatus.Comp.PartID
            mCompStatus.ATAID = tmpCompStatus.ATAID

            If mCompStatus.CompStatusPeriods.Count > 0 Then
                For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                    mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
                Next
                dgCurrentCompValue.DataSource = mCompStatus.CompStatusPeriods
                dgInstallationValues.DataSource = mCompStatus.CompStatusPeriods
                dgCurrentCompValue.DataBind()
                dgInstallationValues.DataBind()
            End If

            Dim tmpCompStatusPeriod As CompStatusPeriod
            For Each tmpCompStatusPeriod In tmpCompStatus.CompStatusPeriods
                If Not mCompStatus.CompStatusPeriods.Contains(tmpCompStatusPeriod.PeriodID) Then
                    mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewChildCompStatusPeriod(mCompStatus.ID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, tmpCompStatusPeriod.PeriodID, txtInstalledOnDate.Text))
                    ''mCompStatus.CompStatusPeriods.Item(tmpCompStatusPeriod.PeriodID, "").CompCurrentValueFormatted = ""
                    mCompStatus.CompStatusPeriods.Item(tmpCompStatusPeriod.PeriodID, "").CompInstallationValueFormatted = ""
                    mCompStatus.CompStatusPeriods.Item(tmpCompStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted = ""
                End If
            Next
            Session("mCompStatus") = mCompStatus
            dgCurrentCompValue.DataSource = mCompStatus.CompStatusPeriods
            dgInstallationValues.DataSource = mCompStatus.CompStatusPeriods
            DataBind()
            tmpCompStatus = Nothing
        Else
            If mCompStatus.CompStatusPeriods.Count > 0 Then
                For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                    mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
                Next
                dgCurrentCompValue.DataSource = mCompStatus.CompStatusPeriods
                dgInstallationValues.DataSource = mCompStatus.CompStatusPeriods
                dgCurrentCompValue.DataBind()
                dgInstallationValues.DataBind()
            End If
        End If
        ''Dim mCompStatusList As CompStatusList = CompStatusList.GetCompStatusList(mCompStatus.AssemblyID, , mCompStatus.CompID.ToString, mPartlist(PartIndex).ID, mPartlist(PartIndex).Name, mPartlist(PartIndex).Description, , , , True, True, True, , , , , mCompStatus)
    End Sub
    Private Sub SetRights()

        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineComponentNew") Or User.IsInRole("MachineComponentEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineComponentNew") Or User.IsInRole("MachineComponentEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    'MLNo
    Public Sub SetLicenceCount()
        If mCompStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "(SELECT)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        mPartlist = PartList.GetPartList("", "", "(SELECT)")
        Session("mPartList") = mPartlist
        cmbPartNo.DataSource = mPartlist

        mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mAssemblyStatus.ID)
        Session("mPeriodListForCompStatus") = mPeriodListForCompStatus

        ''mCompStatus = CompStatus.GetCompStatus(mCompStatusList(index).CompStatusID, mAssemblyStatus.ID, mAssemblyStatus.InstalledOn.ToString) 'AC
        ''Session("mCompStatus") = mCompStatus  'AC

        dgCurrentCompValue.DataSource = mCompStatus.CompStatusPeriods
        dgInstallationValues.DataSource = mCompStatus.CompStatusPeriods

        'Added Code By Prashant on May,28,2007
        txtInstalledOnDate.Text = mCompStatus.InstalledOnFormatted.ToString

        Session("PartNo") = mCompStatus.PartName
        Session("Description") = mCompStatus.Description

        'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        mManufacturerList = ManufacturerList.GetManufacturerList(, "(SELECT)")
        cmbManufacturerList.DataSource = mManufacturerList
        Session("mManufacturerList") = mManufacturerList
        'End

        mInstallationStatusList = InstallationStatusList.GetInstallationStatusList()
        cmbInstallationStatus.DataSource = mInstallationStatusList
        Session("mInstallationStatusList") = mInstallationStatusList

        BindLicenceNo() 'MLNo

        'Added by Saylee on 7-Oct-2017 for Thrust
        If mAssemblyStatus.AssemblyTypeID = 2 And mCompStatus.CompStatusPeriods.Contains(3) And AppSettings("ShowThrustMonitoring") = True Then
            mFirstThrustCompStatus = FirstThrustCompStatus.GetFirstThrustCompStatusList(mAssemblyStatus.AssemblyID)
            Session("mFirstThrustCompStatus") = mFirstThrustCompStatus

            mThrustTypeList = ThrustTypeList.GetThrustTypeList()
            Session("mThrustTypeList") = mThrustTypeList

            lblB22.InnerText = mThrustTypeList(0).Name
            lblB24.InnerText = mThrustTypeList(1).Name
            lblB26.InnerText = mThrustTypeList(2).Name

            If Not mFirstThrustCompStatus Is Nothing Then
                If mCompStatus.IsNew And mCompStatus.IsThrustMonitoringComp Then
                    mCompStatus.B22IsCurrentThrust = mFirstThrustCompStatus(0).B22IsCurrentThrust
                    mCompStatus.B24IsCurrentThrust = mFirstThrustCompStatus(0).B24IsCurrentThrust
                    mCompStatus.B26IsCurrentThrust = mFirstThrustCompStatus(0).B26IsCurrentThrust
                End If

            End If
        End If
        '******************************************************

        DataBind()
        'upnlComponentDetails.DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mCompStatus") = mCompStatus
        dgCurrentCompValue.DataSource = mCompStatus.CompStatusPeriods
        dgCurrentCompValue.DataBind()
        dgInstallationValues.DataSource = mCompStatus.CompStatusPeriods
        dgInstallationValues.DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        SetObject()
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtDescription" Then
            If Len(Trim(txtDescription.Text)) > 200 Then
                custValidator.ErrorMessage = "Max. length of Description should be 200 char long."
                txtDescription.Text = txtDescription.Text.Substring(0, 199)
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(Trim(txtRemark.Text)) > 1000 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 1000 char long."
                txtRemark.Text = txtRemark.Text.Substring(0, 199)
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbPartNo" Then
            If ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA")) Then
                If txtPartDescription.Text = "" Then
                    custValidator.ErrorMessage = "Please Enter Part No."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                If cmbPartNo.SelectedIndex = 0 Then
                    custValidator.ErrorMessage = "Please select the Part No. from the list."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        ElseIf custValidator.ControlToValidate = "cmbATAChapter" Then
            If cmbATAChapter.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select ATAChapter from List."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtInstalledOnDate" Then
            Dim i As Integer
            For i = 0 To mCompStatus.CompStatusPeriods.Count - 1
                If mCompStatus.CompStatusPeriods(i).PeriodID = 2 Then
                    If IsDate(txtInstalledOnDate.Text) And IsDate(mCompStatus.CompStatusPeriods(2, "").CompCurrentValue) Then
                        If (txtInstalledOnDate.Text) < CDate(mCompStatus.CompStatusPeriods(2, "").CompCurrentValue) Then
                            custValidator.ErrorMessage = "Installation date should be later to Start date.."
                            e.IsValid = False
                        ElseIf (txtInstalledOnDate.Text) > CDate(mCompStatus.AsOnDate) Then
                            custValidator.ErrorMessage = "Installation date should be prior to As on date.."
                            e.IsValid = False
                        Else
                            e.IsValid = True
                        End If
                    End If
                End If
            Next
            'Added By Prashant On 12-Jun-2012 FOR ALL08062012
        ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End

        End If
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetGridObject()
        Dim str As String = ""
        Dim txtCurrentCompValue As TextBox 'Code Added
        'If Not mAssemblyStatus.IsValid Then

        '    For i As Integer = 0 To mAssemblyStatus.GetBrokenRulesCollection.Count - 1
        '        str = str + mAssemblyStatus.GetBrokenRulesCollection(i).Description + "<BR>"

        '    Next
        'End If
        For i As Integer = 0 To CShort(dgCurrentCompValue.Rows.Count - 1)
            txtCurrentCompValue = CType(Me.dgCurrentCompValue.Rows(i).FindControl("txtcvCurrentValue"), TextBox) 'Code Added
            If Not mCompStatus.CompStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompStatus.CompStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompStatus.CompStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        SetObject()
        If Not mCompStatus.IsValid Then
            Dim x As Integer
            For x = 0 To mCompStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompStatus.GetBrokenRulesCollection(x).Description + "<BR>"
            Next
        End If

        For i As Integer = 0 To CShort(dgCurrentCompValue.Rows.Count - 1)
            If Not mCompStatus.CompStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompStatus.CompStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompStatus.CompStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvPartNo.ErrorMessage = str
            cvPartNo.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub addAttributes()
        txtACF.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtACF').value,event)")
        txtECF.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtECF').value,event)")
        txtFCF.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtFCF').value,event)")
        txtRTCF.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRTCF').value,event)")

        'Added By Saylee on 6-Oct-2017
        txtB22Current.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtB22Current').value,event)")
        txtB22LifeLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtB22LifeLimit').value,event)")

        txtB24Current.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtB24Current').value,event)")
        txtB24LifeLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtB24LifeLimit').value,event)")

        txtB26Current.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtB26Current').value,event)")
        txtB26LifeLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtB26LifeLimit').value,event)")

        txtFanBladePosition.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtFanBladePosition').value,event)")
        txtMomentWeight.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtMomentWeight').value,event)")
        txtBalanceScrew.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtBalanceScrew').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        GetSessionService()
        GetSessionInspection()
        GetSessionModification()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 1-Aug-2011 For All19072011
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            cmbATAChapter.Focus()
            AddSelectedPeroids()
            DataFieldBind()
            SetPage()
            ControlVisiblity1() 'Added By Prashant 26-Aug-2010
            SetRights()
            ControlVisibility()
            TbContInst.ActiveTabIndex = IIf(CType(Session("CompInstTabIndex"), Integer) > 0, CType(Session("CompInstTabIndex"), Integer), 0)
            If CType(Session("CompInstTabIndex"), Integer) > 0 Then
                Call TbContInst_ActiveTabChanged(Nothing, Nothing)
            End If
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End

            '''''Added by Saylee on 24-apr-2023
            ''''Dim lblServiceTitle As Label

            ''''lblServiceTitle = TbContInst.Tabs(1).FindControl("lblServiceListTitle")
            ''''If AppSettings("ShowMaintenanceForNewClients") = "True" Then

            ''''    ' tbPnlServiceList.HeaderTemplate = "MPD List"
            ''''    lblServiceTitle.Text = "Maintenance Event(s)"
            ''''    TbContInst.Tabs(2).Visible = False
            ''''    dgMonitorServiceStatusList.Columns(3).Visible = True
            ''''    If Not cmbLookInService.Items.Contains(New ListItem("Task Type", "2")) Then
            ''''        cmbLookInService.Items.Add(New ListItem("Task Type", "2"))
            ''''        cmbLookInService.Items.Add(New ListItem("Work Order No.", "3"))
            ''''    End If

            ''''Else

            ''''    'tbPnlServiceList.HeaderTemplate = "Service List"
            ''''    lblServiceTitle.Text = "Service(s)"
            ''''    TbContInst.Tabs(2).Visible = Not (mCompStatus.IsNew)
            ''''    dgMonitorServiceStatusList.Columns(3).Visible = False
            ''''    If Not cmbLookInService.Items.Contains(New ListItem("Service Type", "2")) Then
            ''''        cmbLookInService.Items.Add(New ListItem("Service Type", "2"))
            ''''        cmbLookInService.Items.Add(New ListItem("Work Order No.", "3"))
            ''''    End If
            ''''End If
            '''''**************************

        End If

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If Save() = True Then
                DataFieldBind()
                SetPage()
                ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                SetRights()
                ControlVisibility()
                upnlComponentDetails.Update()
                upnlContainer.Update()
                upnlTitle.Update()
                upnlPartInfo.Update()
                'MLNo
                Session.Remove("mMaintenanceDoneByEmployees")
                Session.Remove("UserNameForLicenceList")
                'End
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Else
                upnlValidationSummary.Update()
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgCurrentCompValue_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCurrentCompValue.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgCurrentCompValue.PageIndex * dgCurrentCompValue.PageSize
                'Commented and added by Saylee on 24-Aug-2009
                ''If mCompStatus.CompStatusPeriods.Item(Index).HasMonitor = True Then
                If mCompStatus.CompStatusPeriods.Item(Index).HasMonitorCount(mCompStatus.ID, mCompStatus.CompStatusPeriods.Item(Index).PeriodID) = True Then
                    MSGBoxCtrl.show(MSGBox.Message_title.MachineMonitor, MSGBox.Message_text.MachineMonitor, "Selected " & mCompStatus.CompStatusPeriods.Item(Index).PeriodName & " period can not be removed as monitor entry exist", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    'Added By Prashant 03-Sep-2010
                    mCompStatus.InstallationWONo = Trim(txtWorkOrderNo.Text)
                    mCompStatus.InstallationRemark = Trim(txtRemark.Text)
                    mCompStatus.SourceDoc = Trim(txtSourceDoc.Text)
                    mCompStatus.RevisionNo = Trim(txtRevisionNo.Text)
                    mCompStatus.BookNo = Trim(txtBookNo.Text)
                    mCompStatus.PageNo = Trim(txtPageNo.Text)
                    mCompStatus.InstDoneBy = Trim(txtInstDoneBy.Text)
                    'mCompStatus.InstDoneByID = New Guid(cmbDoneBy.SelectedValue)
                    'mCompStatus.InstLicenseNo = txtLicenceNo.Text.Trim
                    'mCompStatus.InstPlace = txtPlace.Text.Trim
                    '-----------------------------
                    'Added By Prashant On 12-Jun-2012 FOR ALL08062012
                    Dim LicenseNo As String = String.Empty
                    Dim EmpName As String = String.Empty
                    If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                        LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                        EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                    Else
                        LicenseNo = Trim(txtLicenceNo.Text)
                    End If
                    mCompStatus.InstLicenseNo = LicenseNo
                    mCompStatus.InstDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
                    mCompStatus.InstPlace = txtPlace.Text.Trim
                    'End

                    mCompStatus.CompStatusPeriods.RemoveAt(Index)
                    ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                    If (Not mCompStatus.CompStatusPeriods.Contains(9) And Not mCompStatus.CompStatusPeriods.Contains(10) And Not mCompStatus.CompStatusPeriods.Contains(16)) Then
                        mCompStatus.Comp.ACF = 0D
                        mCompStatus.Comp.ECF = 0D
                        mCompStatus.Comp.FCF = 0D
                        mCompStatus.Comp.RTCF = 0D ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor
                    End If
                    DataBindGrid()
                    ControlVisiblity1()
                    ControlVisibility()
                    upnlPartInfo.Update()
                    upnlInstallationValues.Update()
                End If
        End Select
    End Sub
    'Private Sub btnAddPeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriod.Click
    '    SetObject()
    '    SetPeroids()
    '    SetGridObject()
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
    '    'Response.Redirect("wfSelectPeriod.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage2=wfCompStatus.aspx")
    'End Sub
    Private Sub btnAddPeriod_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPeriod.Click
        SetObject()
        SetPeroids()
        SetGridObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow();", True)
    End Sub
    Private Sub cmbPartNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPartNo.SelectedIndexChanged
        txtDescription.Text = IIf(cmbPartNo.SelectedIndex > 0, mPartlist(cmbPartNo.SelectedIndex).Description, "")
        GetCompStatusForPart(cmbPartNo.SelectedIndex)  'Added by Saylee on 25-Aug-2009
        ControlVisiblity1() 'Added By Prashant 26-Aug-2010
        If cmbPartNo.Enabled = True Then
            cmbPartNo.Focus()
        End If

        If cmbPartNo.SelectedIndex = 0 Then
            cmbATAChapter.SelectedIndex = 0
            'cmbATAChapter.DataBind()
        End If
        upnlPartInfo.Update()
        upnlInstallationValues.Update()
        upnlTSNValues.Update()
    End Sub
    Private Sub btnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartNo.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPartWindow", "OpenPartWindow();", True)
        'Response.Redirect("wfPart.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfCompStatus.aspx")
    End Sub
    Private Sub ImgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnATAChapter.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenATAWindow", "OpenATAWindow();", True)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        If Not mCompStatus.IsNew Then
            'Dim mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
            'mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp) 'ture is for mCompStatus.IsMaster 

            'Dim mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
            'mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            If IsDate(mCompStatus.AsOnDateFormatted.ToString) And IsDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
                If CDate(mCompStatus.AsOnDateFormatted.ToString) > CDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
                    mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp) 'ture is for mCompStatus.IsMaster 
                    mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)

                Else
                    mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mAssemblyStatus.AsOnDateFormatted.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp) 'ture is for mCompStatus.IsMaster 
                    mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mAssemblyStatus.AsOnDateFormatted.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)

                End If
            End If

            If mCompMonitorServiceStatusList.Count <= 0 And mCompMonitorInspStatusList.Count <= 0 Then
                MSGBoxCtrl.Show("Monitoring Service / Inspection not added", "Monitoring Service / Inspection is not Added in this Installed Component.<BR><BR> Do you want to Configure them?", "", MsgBoxStyle.YesNoCancel, "ReqServ")
                Exit Sub
            End If
        End If

        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MarkLog(Util.Action.Close, "Assembly Component Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End

        RemoveSession()
        If Session("IsOpenFromMaster") = True Then
            Session.Remove("IsOpenFromMaster") 'Added By Vikrant On 26-Jun-2014
            If mAssemblyStatus.IsSpareAssembly = True Then  'Added by Saylee on 10-Feb-2020,  All27072020
                Response.Redirect("wfSpareAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
            Else
                Response.Redirect("wfAssemblyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
            End If

        Else
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        End If


    End Sub
    Private Sub txtInstalledOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInstalledOnDate.TextChanged
        If IsPostBack Then
            SetGridObject()           'Added Code on 1st June,2007
            SetObject()
            DataBindGrid()
            upnlInstallationValues.Update()
            upnlTSNValues.Update()
        End If
    End Sub
    Private Sub chkByModel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkByModel.CheckedChanged
        mCompStatus.Comp.PartID = Guid.Empty 'Code Added By Rajnish On 09-04-2008
        If chkByModel.Checked Then
            mPartlist = PartList.GetPartList(mAssemblyStatus.Assembly.ModelID, , , "(SELECT)")
            Session("mPartlist") = mPartlist
        Else
            mPartlist = PartList.GetPartList("", "", "(SELECT)")
            Session("mPartList") = mPartlist

        End If

        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
            PartNo = IIf(mPartlist.Contains(mCompStatus.Comp.PartName), mCompStatus.Comp.PartName, "")
            Description = IIf(txtPartDescription.Text.Length <> 0, mPartlist(PartNo).Description, "")
            txtPartDescription.Text = PartNo
            txtDescription.Text = Description
        Else
            cmbPartNo.DataSource = mPartlist
            cmbPartNo.DataBind()
            cmbPartNo.SelectedValue = IIf(mPartlist.Contains(mCompStatus.Comp.PartName), mCompStatus.Comp.PartID.ToString, Guid.Empty.ToString)
            txtDescription.Text = IIf(cmbPartNo.SelectedIndex > 0, mPartlist(cmbPartNo.SelectedIndex).Description, "")
        End If
        upnlPartNoDetails.Update()
        upnlPartNoModelDetails.Update()
        upnlbtnPartNo.Update()
    End Sub
    'Added By Utkarsh On 31-Jan-2013 For ALL30122013
    Private Sub imgbtnModel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnModel1.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManufacturerWindow", "OpenManufacturerWindow();", True)
    End Sub
    'end
    'Added By Vikrant On 26-Jun-2014
    'Private Sub imgHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHome.Click
    '    MarkLog(Util.Action.Close, "Assembly Component Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
    '    RemoveSession()
    '    RemoveAllSessionValues()
    '    Response.Redirect("wfMachine.aspx?BackPage=Index.aspx")
    'End Sub
    'End
    'Added By Vikrant On 31-Mar-2015 For All31032015
    Private Sub cmbInstallationStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbInstallationStatus.SelectedIndexChanged
        lblModuleTSNCaption.InnerText = cmbInstallationStatus.SelectedItem.ToString + " Value as on " & mCompStatus.AsOnDateFormatted
        upnlTSNValues.Update()
    End Sub
    'End
    Protected Sub txtCompInstallationValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtCompInstallationValue As TextBox
        With mCompStatus.CompStatusPeriods
            For I As Integer = 0 To .Count - 1
                txtCompInstallationValue = CType(Me.dgInstallationValues.Rows(I).FindControl("txtCompInstallationValue"), TextBox)
                If .Item(I).PeriodID = 2 Then
                    If Period.IsDate(txtCompInstallationValue.Text) Then
                        .Item(I).CompInstallationValueFormatted = Trim(txtCompInstallationValue.Text)
                    Else
                        .Item(I).CompInstallationValueFormatted = ""
                    End If
                Else
                    .Item(I).CompInstallationValue = Trim(txtCompInstallationValue.Text)
                End If
            Next
            DataBindGrid()
            upnlTSNValues.Update()
        End With
    End Sub
    Protected Sub txtAssemblyInstallationValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtAssemblyInstallationValue As TextBox
        With mCompStatus.CompStatusPeriods
            For I As Integer = 0 To .Count - 1
                txtAssemblyInstallationValue = CType(Me.dgInstallationValues.Rows(I).FindControl("txtAssemblyInstallationValue"), TextBox)
                If .Item(I).PeriodID = 2 Then
                    If Period.IsDate(txtAssemblyInstallationValue.Text) Then
                        .Item(I).AssemblyInstallationValueFormatted = Trim(txtAssemblyInstallationValue.Text)
                    Else
                        .Item(I).AssemblyInstallationValueFormatted = ""
                    End If
                Else
                    .Item(I).AssemblyInstallationValue = Trim(txtAssemblyInstallationValue.Text)
                End If
            Next
            DataBindGrid()
            upnlTSNValues.Update()
        End With
    End Sub
    Protected Sub txtCurrentCompValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtCompValue As TextBox
        With mCompStatus.CompStatusPeriods
            For I As Integer = 0 To .Count - 1
                txtCompValue = CType(Me.dgCurrentCompValue.Rows(I).FindControl("txtCurrentCompValue"), TextBox)
                If .Item(I).PeriodID = 2 Then
                    If Period.IsDate(txtCompValue.Text) Then
                        .Item(I).CompCurrentValueFormatted = Trim(txtCompValue.Text)
                    Else
                        .Item(I).CompCurrentValueFormatted = ""
                    End If
                Else
                    .Item(I).CompCurrentValue = Trim(txtCompValue.Text)
                End If
            Next
            DataBindGrid()
            upnlInstallationValues.Update()
        End With

    End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
    Private Sub hdnBtnManufacturer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnManufacturer.Click
        mManufacturerList = ManufacturerList.GetManufacturerList(, "(SELECT)")
        cmbManufacturerList.DataSource = mManufacturerList
        Session("mManufacturerList") = mManufacturerList
        cmbManufacturerList.DataBind()
        upnlManufacturerMaster.Update()
    End Sub
    Private Sub hdnAddPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnAddPeriod.Click
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        AddSelectedPeroids()
        ControlVisiblity1()
        SetObject()
        dgCurrentCompValue.DataSource = mCompStatus.CompStatusPeriods
        dgInstallationValues.DataSource = mCompStatus.CompStatusPeriods
        dgCurrentCompValue.DataBind()
        dgInstallationValues.DataBind()
        upnlPartInfo.Update()
        upnlTSNValues.Update()
        upnlInstallationValues.Update()
    End Sub
    Private Sub hdnBtnPart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnPart.Click
        mPartlist = PartList.GetPartList("", "", "(SELECT)")
        Session("mPartList") = mPartlist
        cmbPartNo.DataSource = mPartlist
        cmbPartNo.DataBind()
        txtDescription.DataBind()
        upnlPartNoDetails.Update()
        upnlPartNoModelDetails.Update()
        'upnlbtnPartNo.Update()
    End Sub
    Private Sub txtPartDescription_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPartDescription.TextChanged
        SetPartNoDescription()
        txtDescription.Text = Description
        txtPartDescription.Text = PartNo
        GetCompStatusForPart(0)
        ControlVisiblity1()
        ControlVisibility()
        If PartNo.Length = 0 Then
            cmbATAChapter.SelectedIndex = 0
            'cmbATAChapter.DataBind()
        End If
        upnlInstallationValues.Update()
        upnlTSNValues.Update()
        upnlPartNoDetails.Update()
        upnlPartInfo.Update()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompStatus.ID
            Session("MaintenanceDoneOnDate") = mCompStatus.InstalledOn.ToString
            mMaintenanceDoneByEmployees = mCompStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                ' mCompStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompStatus.MaintenanceDoneByEmployees.Remove(mCompStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompStatus") = mCompStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        upnlLicenceNo.Update()
    End Sub
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        'SetObject()
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        Session("LicenseNo") = LicenseNo
        Session("EmployeeID") = DoneByID
        If Not DoneByID.Equals(Guid.Empty) Then
            If mCompStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mCompStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, MaintActivityTypeID.ComponentInstallation, DoneByID, LicenseNo, "", EmpName)
            End If

        Else
            If mCompStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompStatus") = mCompStatus
        BindLicenceNo()
        SetLicenceCount()
    End Sub
    'End

    'Added by Saylee on 7-Oct-2017 for Thrust
    Private Sub hdnThrustValue_Click(sender As Object, e As System.EventArgs) Handles hdnThrustValue.Click
        If chkIsThrustComp.Checked Then

            If mAssemblyStatus.AssemblyTypeID = 2 And mCompStatus.CompStatusPeriods.Contains(3) Then

                mThrustTypeList = ThrustTypeList.GetThrustTypeList()
                Session("mThrustTypeList") = mThrustTypeList

                lblB22.InnerText = mThrustTypeList(0).Name
                lblB24.InnerText = mThrustTypeList(1).Name
                lblB26.InnerText = mThrustTypeList(2).Name
                mFirstThrustCompStatus = Session("mFirstThrustCompStatus")
                If mFirstThrustCompStatus Is Nothing Then mFirstThrustCompStatus = FirstThrustCompStatus.GetFirstThrustCompStatusList(mAssemblyStatus.AssemblyID)
                If Not mFirstThrustCompStatus Is Nothing Then
                    If mFirstThrustCompStatus.Count > 0 Then
                        mCompStatus.B22IsCurrentThrust = mFirstThrustCompStatus(0).B22IsCurrentThrust
                        mCompStatus.B24IsCurrentThrust = mFirstThrustCompStatus(0).B24IsCurrentThrust
                        mCompStatus.B26IsCurrentThrust = mFirstThrustCompStatus(0).B26IsCurrentThrust

                        chkB22IsCurrent.Checked = mCompStatus.B22IsCurrentThrust
                        chkB24IsCurrent.Checked = mCompStatus.B24IsCurrentThrust
                        chkB26IsCurrent.Checked = mCompStatus.B26IsCurrentThrust

                        chkB22IsCurrent.Enabled = False
                        chkB24IsCurrent.Enabled = False
                        chkB26IsCurrent.Enabled = False
                    End If
                End If
            End If

            pnlThrustyComponentDet.Visible = True
        Else
            pnlThrustyComponentDet.Visible = False
            ''txtB22Current.Text = "0"
            ''txtB22LifeLimit.Text = "0"

            ''txtB24Current.Text = "0"
            ''txtB24LifeLimit.Text = "0"

            ''txtB26Current.Text = "0"
            ''txtB26LifeLimit.Text = "0"

            ''chkB22IsCurrent.Checked = False
            ''chkB24IsCurrent.Checked = False
            ''chkB26IsCurrent.Checked = False
        End If
        upnlThrustyComponentDet.Update()
    End Sub
    '****************************************************************
    Private Sub lnkHistoryCard_Click(sender As Object, e As System.EventArgs) Handles lnkHistoryCard.Click 'Added by Saylee on 12-Jan-2018 for ALL12012018
        Dim Rpt As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCompHistory 'dsCompHistoryList
        Dim ObjHistoryCard As ComponentHistory ''CompHistoryCardList
        Dim mCompanyDetail As New CompanyDetail


        If AppSettings("ClientCode") = "Indamer" Then
            Rpt = New crptComponentHistoryInd 'crptCompHistoryCardListForIndamer
        ElseIf AppSettings("ClientCode") = "STR" Then 'Added By Vikrant On 14-Aug-2018 For StarAir14082018
            Rpt = New crptComponentHistoryStarAir
        Else
            Rpt = New crptComponentHistory 'crptCompHistoryCardList
        End If

        '********************************

        ObjHistoryCard = ComponentHistory.GetComponentHistory(New SmartDate(Today.Date.ToString, False), mCompStatus.CompID)
        Session("ObjHistoryCard") = ObjHistoryCard
        If ObjHistoryCard.Count = 0 Then
            ''Dim msg1 As New SIMsgBox(Page, " Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly)
            ''msg1.ReplacePage = "wfrptComponentHistoryCard.aspx?BackPage=" & Request.QueryString("BackPage")
            ''msg1.Show()
            MSGBoxCtrl.Show(" Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim EventLogDetail As String = "Printed From Component Installation with As On Date: " + New SmartDate(Today.Date.ToString, False).FormattedText + " , Part: " + txtPartDescription.Text + " , Serial No.: " + txtSerialNo.Text.Trim
        Dim ReportData As Flypal.ReportData
        If ObjHistoryCard.Count > 0 Then
            ReportData = New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
             "", "Component History Card Report", New SmartDate(Today.Date.ToString, False).FormattedText, "", txtPartDescription.Text, txtSerialNo.Text, ObjHistoryCard(0).ATA, AppSettings("Product Version"), AppSettings("SINote"), txtDescription.Text, "", "", "Assembly", AppSettings("Logo"))

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1135)

            '*******************************
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ObjHistoryCard)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportData)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "Component History Card", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    ' Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
    Private Sub chkFanBladeMonitoring_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFanBladeMonitoring.CheckedChanged
        If chkFanBladeMonitoring.Checked = True Then
            txtFanBladePosition.Enabled = True
            txtMomentWeight.Enabled = True
            txtBalanceScrew.Enabled = True
            mCompStatus.IsFanBladeDistribution = chkFanBladeMonitoring.Checked
            mCompStatus.FanBladePosition = Val(txtFanBladePosition.Text)
            mCompStatus.MomentWeight = CDec(txtMomentWeight.Text)
            mCompStatus.BalanceScrew = Val(txtBalanceScrew.Text)
        ElseIf chkFanBladeMonitoring.Checked = False Then
            txtFanBladePosition.Text = "0"
            txtMomentWeight.Text = "0"
            txtBalanceScrew.Text = "0"
            txtFanBladePosition.Enabled = False
            txtMomentWeight.Enabled = False
            txtBalanceScrew.Enabled = False
            mCompStatus.IsFanBladeDistribution = chkFanBladeMonitoring.Checked
            mCompStatus.FanBladePosition = Val(txtFanBladePosition.Text)
            mCompStatus.MomentWeight = CDec(txtMomentWeight.Text)
            mCompStatus.BalanceScrew = Val(txtBalanceScrew.Text)
        End If
    End Sub
    ' End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394 
#End Region

#Region " Report "
#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Rpt = New crDetComponentStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList
        Dim PartNumber As String = String.Empty
        PartNumber = IIf((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA"), txtPartDescription.Text, cmbPartNo.SelectedItem.Text)


        'For Current Component Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 7
        RHCount = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                  Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                  cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                  dgCurrentCompValue.Columns.Item(1).HeaderText, dgCurrentCompValue.Columns.Item(2).HeaderText,
                    , dgCurrentCompValue.Columns.Item(3).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                                Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                                cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                                      "", "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "ATA Chapter",
                                mCompStatus.ATAChapter, , , , , , , , , , , , , , , , ,
                                cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                                CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "ATA Chapter",
                            mCompStatus.ATAChapter, , , , , , , , , , , , , , , , ,
                            cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                                                   "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                                PartNumber, , , , , , , , , , , , , , , , ,
                                cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                                CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                            PartNumber, , , , , , , , , , , , , , , , ,
                            cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                                                   "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                    Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                    cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                      Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                      cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                              "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                    Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                    cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                     Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                     cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                    Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                    cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                     Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                     cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Manufacturer",
                                    IIf(cmbManufacturerList.SelectedIndex > 0, cmbManufacturerList.SelectedItem.ToString, ""), , , , , , , , , , , , , , , , ,
                                    cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Manufacturer",
                                     IIf(cmbManufacturerList.SelectedIndex > 0, cmbManufacturerList.SelectedItem.ToString, ""), , , , , , , , , , , , , , , , ,
                                     cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 6 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                     "", , , , , , , , , , , , , , , , ,
                     cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                    CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                    CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                    , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         cmbInstallationStatus.SelectedItem.Text + " Value as on " & mCompStatus.AsOnDateFormatted,
                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                        CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
            End If
        Next

        'For Installation Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Installed On",
                                             Me.mCompStatus.InstalledOnFormatted.ToString, , , , , , , , , , , , , , , , ,
                                             "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                             dgInstallationValues.Columns.Item(1).HeaderText,
                                             dgInstallationValues.Columns.Item(2).HeaderText,
                                          , dgInstallationValues.Columns.Item(3).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Installed On",
                                Me.mCompStatus.InstalledOnFormatted.ToString, , , , , , , , , , , , , , , , ,
                                "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                      "", "", , ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Installation Remark",
                     Me.mCompStatus.InstallationReason, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                             CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                             CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                             , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Installation Remark",
                 Me.mCompStatus.InstallationReason, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                          "", "", , ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Work Order No.",
                     Me.mCompStatus.InstallationWONo, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                             CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                             CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                             , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Work Order No.",
                 Me.mCompStatus.InstallationWONo, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                          "", "", , ""))
                End If

            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Remark",
                                           Me.mCompStatus.InstallationRemark, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Remark",
                                    Me.mCompStatus.InstallationRemark, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                         "", "", , ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Done By Agency",
                                           Me.mCompStatus.InstDoneBy, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Done By Agency",
                                    Me.mCompStatus.InstDoneBy, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                         "", "", , ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "License No.",
                                           Me.mCompStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "License No.",
                                    Me.mCompStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                         "", "", , ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Place",
                                           Me.mCompStatus.InstPlace, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                                                 CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "Place",
                                    Me.mCompStatus.InstPlace, , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                         "", "", , ""))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "",
                                          "", , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                                  CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                                                  CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "",
                                          "", , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                                 "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Installation Information of Component", "",
                                           "", , , , , , , , , , , , , , , , , "Values at " + cmbInstallationStatus.SelectedItem.Text,
                                                  CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                                                   CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                   , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
            End If
        Next

        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 1
        RHCount2 = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        If temp2 < RHCount2 Then
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc.", txtSourceDoc.Text, , , , , , , , , , , , , , , , , "", , , "Revision No.", , txtRevisionNo.Text))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc.", txtSourceDoc.Text, , , , , , , , , , , , , , , , , "", "", txtRevisionNo.Text))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", txtBookNo.Text, , , , , , , , , , , , , , , , , "", , , "Page No.", , txtPageNo.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", txtBookNo.Text, , , , , , , , , , , , , , , , , "", "", txtPageNo.Text))
                End If
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Component Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 1-Aug-2011 For All19072011
        '    MarkLog(Util.Action.Print, "CompStatus", "CompStatus Report", Util.ErrorType.NoError, Guid.Empty)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region
#End Region

    'Added By Utkarsh ON 10-Jun-2013 FOR BA10062013
#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim str As String() = contextKey.Split("¿") 'Holds the parameters to filter criteria..
        Dim isModel As Boolean = CBool(str(0).Substring(str(0).IndexOf("=") + 1))
        Dim ModelID As String = str(1).Substring(str(1).IndexOf("=") + 1)
        Dim partlist As PartListAutoComplete
        If isModel Then
            partlist = PartListAutoComplete.GetPartList(prefixText, ModelID)
        Else
            partlist = PartListAutoComplete.GetPartList(prefixText)
        End If
        If count = 0 Then
            Return (From c As PartListAutoCompleteInfo In partlist
                    Select c.Name).ToList
        Else
            Return (From c As PartListAutoCompleteInfo In partlist
                    Select c.Name).Take(count).ToList
        End If

    End Function
    'MLNo
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetLicenseNoList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mLicenses As LicenseNoListWithEmployee
        mLicenses = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, UserNameForLicenceList, , , False)

        If count = 0 Then
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
        Else
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region
    'End
#End Region

#Region "Common Events"

#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region
    Protected Sub ScriptManager1_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs)
        If (e.Exception.Data("ExtraInfo") <> Nothing) Then
            ScriptManager1.AsyncPostBackErrorMessage =
               e.Exception.Message &
               e.Exception.Data("ExtraInfo").ToString()
        Else
            ScriptManager1.AsyncPostBackErrorMessage =
               "An unspecified error occurred."
        End If
    End Sub
    Private Sub TbContInst_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbContInst.ActiveTabChanged
        Select Case Session("CompInstTabIndex")
            Case 0
                'RemoveSession()
            Case 1 'Remove Service List Session
                RemoveSessionService()
            Case 2 'Remove Inspection List Session
                RemoveSessionInspection()
            Case 3 'Remove Modification List Session
                RemoveSessionModification()
        End Select
        Session("CompInstTabIndex") = TbContInst.ActiveTabIndex
        Session("mIsSpareAssembly") = mIsSpareAssembly  'Added By Saylee On 27-Jul-2020 For ALL27072020
        Select Case TbContInst.ActiveTabIndex
            Case 0
            Case 1 'Service Tab
                addAttributesService()
                GetSessionService()
                cmbLookInService.Focus()
                DataFieldBindService()
                SetControlsService() 'Added By Saylee on 28-th-Jan-2008 for bug-Component Service List
                SetPageService()
                ControlVisibilityService()
                SetRightsService()
                SetGridService()
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Description"
                    dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Task Type"
                Else
                    dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Code/Form No./Description"
                    dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Service Type"
                End If
                upnlService.Update()
            Case 2 'Inspection Tab
                GetSessionInspection()
                addAttributesInspection()
                cmbLookInInspection.Focus()
                DataFieldBindInspection()
                SetControlsInspection()
                SetPageInspection()
                ControlVisibilityInspection() 'Added By Utkarsh On 21-Mar-2011
                SetRightsInspection()
                SetGridInspection()
                upnlInspection.Update()
            Case 3 'Modification Tab
                GetSessionModification()
                addAttributesModification()
                cmbLookInModification.Focus()
                DataFieldBindModification()
                SetControlsModification() 'Added By Saylee on 29-th-Jan-2008 for bug-Component Inspection List (CIL1)
                SetPageModification()
                ControlVisibilityModification() 'Added By Utkarsh On 21-Mar-2011
                SetRightsModification()
                SetGridDirective()
                upnlModification.Update()
        End Select
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region " Service Tab "

#Region " Business methods "
    Private Sub GetSessionService()
        mCompMonitorServiceStatusList = CType(Session("mCompMonitorServiceStatusList"), tmpCompMonitorServiceStatusList)
        mPartMonitorServiceTypeList = CType(Session("mPartMonitorServiceTypeList"), PartMonitorServiceTypeList)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub SetSessionService()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        Session("mPartMonitorServiceTypeList") = mPartMonitorServiceTypeList
        Session("mMachineMaintenance") = mMachineMaintenance 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub RemoveSessionService()
        Session.Remove("mCompMonitorServiceStatusList")
        Session.Remove("mPartMonitorServiceTypeList")
        mPartMonitorServiceTypeList = Nothing
        mCompMonitorServiceStatusList = Nothing
        Session.Remove("LookIn")
        Session.Remove("txtFor")
        Session.Remove("txtCode")
        Session.Remove("SearchFor")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 13th-Oct-2009
        Session.Remove("mFileAttach")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Sub SetGridDirective()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorModStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorModStatusList.Rows(j).Cells(20).Text, Boolean)
            If B = False Then
                dgMonitorModStatusList.Rows(j).Cells(19).Enabled = False
            End If
        Next
    End Sub
    Private Sub NewRecordService()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mHourType, mCompStatus)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

        Dim mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
        mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MarkLog(Util.Action.[New], "Component Service Status", "", Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
        'End
        'Code  Added By Saylee on 1/4/2008 Suggested by Deven Sir
        Session("EditMasterRecord") = "False"

        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
        If mComponentMaintananceListCount Is Nothing Or mComponentMaintananceListCount.MaintenanceServiceListCount = 0 Then
            Dim mPartMonitorService As PartMonitorService
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mPartMonitorService = PartMonitorService.NewPartMonitorService(ID, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mHourType, ID)
            Session.Remove("mPartMonitorServiceList")
            Session("mPartMonitorService") = mPartMonitorService
            MarkLog(Util.Action.[New], "Part Service", "", Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow();", True)
        ElseIf mComponentMaintananceListCount.MaintenanceServiceListCount > 0 Then
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage6=wfAssemblyStatus_Ajax.aspx');", True)
        End If
    End Sub
    Private Sub EditRecordService(ByVal mId As Guid)
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
            mRegNo = "Reg No. : " & mMachine.RegNo
        End If
        '*********************

        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mHourType, , mCompStatus)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
        MarkLog(Util.Action.Edit, "Component Service Status", MachineDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
        'End
        Session.Remove("mFileAttach")
        Response.Redirect("wfCompMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&GChildPage4=wfCompStatus_Ajax.aspx")
    End Sub
    'code added by Saylee on 1/04/2008 Suggested by Deven Sir
    Private Sub EditMasterRecordService(ByVal mMasterId As Guid, ByVal mId As Guid)
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
            mRegNo = "Reg No. : " & mMachine.RegNo
            Session("mMachine") = mMachine
        End If
        '*********************
        Dim mPartMonitorService As PartMonitorService
        mPartMonitorService = PartMonitorService.GetPartMonitorService(mMasterId, mHourType)
        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mHourType, , mCompStatus)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

        Session("mPartMonitorService") = mPartMonitorService
        'RemoveSession()
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mPartMonitorService.PartMonitorServiceTypeName
        MarkLog(Util.Action.Edit, "Component Service Status", MachineDetail, Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
        'End
        Session.Remove("mFileAttach")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow();", True)
    End Sub
    Private Sub DeleteRecordService(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteService")
        mCompMonitorServiceStatusList.CurrentIndex = Index
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
    End Sub
    Private Sub SetGridService()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorServiceStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(20).Text, Boolean)
            If B = False Then
                dgMonitorServiceStatusList.Rows(j).Cells(19).Enabled = False
            End If
        Next
    End Sub
    Private Sub SetPageService()
        If mCompStatus.IsNew Then
            lblTitle.Text = "Component Status [New]"
        Else
            lblTitle.Text = "Component Status [Part: " & mCompStatus.PartName & " SerialNo:" & mCompStatus.SerialNo & " ]"
        End If
        'set the title
        'CNDC
        'lblInfo.Text = "List of all the Servicings on the Component as of Date: " & CStr(mCompStatus.AsOnDate) & ". All the values of all the Services will be as of Date: " & CStr(mCompStatus.AsOnDate)


        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "Maintenance Event"
        Else
            ServiceMPDTitle = "Component Service"
        End If

        lblInfoService.Text = "List of all the " + ServiceMPDTitle + "(s) on the Component as of Date: " & mCompStatus.AsOnDateFormatted & ". All the values of all the " + ServiceMPDTitle + "(s) will be as of Date: " & mCompStatus.AsOnDateFormatted
        lblCountService.Text = "List of " + ServiceMPDTitle + " Status: " & dgMonitorServiceStatusList.Rows.Count & " Record(s)"
    End Sub
    Private Sub addAttributesService()
        txtCodeService.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeService').value,event)")
    End Sub
    Private Sub DisplayControlsService(ByVal Index As Integer)
        txtForService.Text = IIf(Index = 3, txtForService.Text, "")
        txtCodeService.Text = IIf(Index = 1, txtCodeService.Text, "")
        txtCodeService.Visible = IIf(Index = 1, True, False)
        txtForService.Visible = IIf(Index = 3, True, False)
        lblForService.Visible = (Index > 0 And Index <> 4)
        cmbSearchForService.Visible = (Index = 2)
    End Sub
    Private Sub SetControlsService()
        '======Function added By Saylee on 28-th-Jan-2008 for bug-Component Service List 
        txtForService.Text = Session("txtFor")
        txtCodeService.Text = Session("txtCode")
        cmbLookInService.SelectedIndex = Session("LookIn")
        '==========================================================================
        cmbSearchForService.SelectedValue = IIf(SearchFor = "", 0, SearchFor)
        DisplayControlsService(cmbLookInService.SelectedIndex)
        'FindNow()
    End Sub
    Private Sub SetRightsService()

        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentServicePrint")) Then
                btnPrintService.Enabled = False
                btnPrintService.ToolTip = "You are not authorized user"
                btnPrintTopService.Enabled = False
                btnPrintTopService.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineComponentServiceNew")) = False Then
                btnAddNewService.Enabled = False
                btnAddNewTopService.Enabled = False
                btnAddNewService.ToolTip = "You are not Authorized user"
                btnAddNewTopService.ToolTip = "You are not Authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentServicePrint")) Then
                btnPrintService.Enabled = False
                btnPrintService.ToolTip = "You are not authorized user"
                btnPrintTopService.Enabled = False
                btnPrintTopService.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineComponentServiceNew")) = False Then
                btnAddNewService.Enabled = False
                btnAddNewTopService.Enabled = False
                btnAddNewService.ToolTip = "You are not Authorized user"
                btnAddNewTopService.ToolTip = "You are not Authorized user"
            End If
        End If
    End Sub
    Private Sub ControlVisibilityService() 'Added By Utkarsh On 21-Mar-2011
        btnPrintService.Enabled = IIf(mCompMonitorServiceStatusList.Count > 0, True, False)
        btnPrintTopService.Enabled = IIf(mCompMonitorServiceStatusList.Count > 0, True, False)
        btnAddNewTopService.Visible = (mCompMonitorServiceStatusList.Count > 10)
        btnPrintTopService.Visible = (mCompMonitorServiceStatusList.Count > 10)
        btnCloseTopService.Visible = (mCompMonitorServiceStatusList.Count > 10)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub FindNowService()
        dgMonitorServiceStatusList.PageIndex = 0
        Dim TransDate As String
        If IsDate(mCompStatus.AsOnDateFormatted.ToString) And IsDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
            If CDate(mCompStatus.AsOnDateFormatted.ToString) > CDate(mCompStatus.AsOnDateFormatted.ToString) Then
                TransDate = mCompStatus.AsOnDateFormatted.ToString
            Else
                TransDate = mAssemblyStatus.AsOnDateFormatted.ToString
            End If
        End If
        Select Case cmbLookInService.SelectedIndex
            Case 0, -1  'All
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(TransDate, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 1  'ATA Code
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(TransDate, mCompStatus.CompID, True, Val(txtCodeService.Text), , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 2  'Service Type ID
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(TransDate, mCompStatus.CompID, True, , , , CInt(cmbSearchForService.SelectedValue), , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 3 ' Work Order No.
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(TransDate, mCompStatus.CompID, True, , , , , txtForService.Text.Trim, , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 4  'Show In C of A
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(TransDate, mCompStatus.CompID, True, , , , , , True, mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
        End Select
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()


        'Added By Saylee on 28th-Jan-2008===============
        Session("LookIn") = cmbLookInService.SelectedIndex
        Session("txtFor") = txtForService.Text
        Session("txtCode") = txtCodeService.Text
        SearchFor = IIf(cmbSearchForService.SelectedIndex <= 0, "", cmbSearchForService.SelectedValue)
        Session("SearchFor") = SearchFor
        '==================================================
        SetGridService()
    End Sub
    Private Sub DataFieldBindService()
        If IsDate(mCompStatus.AsOnDateFormatted.ToString) And IsDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
            If CDate(mCompStatus.AsOnDateFormatted.ToString) > CDate(mCompStatus.AsOnDateFormatted.ToString) Then
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDateFormatted.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp) 'ture is for mCompStatus.IsMaster 
            Else
                mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mAssemblyStatus.AsOnDateFormatted.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp) 'ture is for mCompStatus.IsMaster 
            End If
        End If
        dgMonitorServiceStatusList.DataSource = mCompMonitorServiceStatusList
        mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList("(All)")
        Session("mPartMonitorServiceTypeList") = mPartMonitorServiceTypeList
        cmbSearchForService.DataSource = mPartMonitorServiceTypeList
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        upnlService.DataBind()
        SearchFor = Session("SearchFor")
    End Sub
#End Region

#Region "Events"
    Private Sub btnAddNewService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewService.Click, btnAddNewTopService.Click
        NewRecordService()
    End Sub
    Private Sub dgMonitorServiceStatusList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorServiceStatusList.RowCommand
        Dim Index As Integer
        Dim mId As Guid
        Dim mMasterId As Guid
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = False Then

            mRegNo = "Reg No. : " & mMachine.RegNo
        End If
        '*********************

        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Description"
        Else
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Code/Form No./Description"
        End If
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                mId = New Guid(dgMonitorServiceStatusList.Rows(Index).Cells(0).Text)
                'Added By Prashant 14-Mar-2011
                If (User.IsInRole("MachineComponentServiceView") Or User.IsInRole("MachineComponentServiceEdit")) = False Then
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorServiceStatusList(mId).PartMonitorServiceTypeName
                    MarkLog(Util.Action.Edit, "Component Service Status", User.Identity.Name & " is not Authorized User to edit " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecordService(mId)
                'Added by Saylee on 1/04/2008
            Case "EditMaster"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                mId = New Guid(dgMonitorServiceStatusList.Rows(Index).Cells(0).Text)
                mMasterId = New Guid(dgMonitorServiceStatusList.Rows(Index).Cells(1).Text)
                'Added By Prashant 14-Mar-2011
                If (User.IsInRole("MachineComponentServiceView") Or User.IsInRole("MachineComponentServiceEdit")) = False Then
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorServiceStatusList(mId).PartMonitorServiceTypeName
                    MarkLog(Util.Action.Edit, "Component Service Status", User.Identity.Name & " is not Authorized User to edit " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("EditMasterRecord") = "True"
                EditMasterRecordService(mMasterId, mId)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                mId = New Guid(dgMonitorServiceStatusList.Rows(Index).Cells(0).Text)
                If (User.IsInRole("MachineComponentServiceDelete")) = False Then
                    'Added By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorServiceStatusList(mId).PartMonitorServiceTypeName
                    MarkLog(Util.Action.Edit, "Component Service Status", User.Identity.Name & " is not Authorized User to edit master " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordService(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(New Guid(dgMonitorServiceStatusList.Rows(Index).Cells(0).Text))
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub cmbLookInService_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLookInService.SelectedIndexChanged
        cmbSearchForService.SelectedIndex = 0
        DisplayControlsService(cmbLookInService.SelectedIndex)
        If cmbLookInService.Enabled = True Then
            cmbLookInService.Focus()
        End If
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Description"
            dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Task Type"
        Else
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Code/Form No./Description"
            dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Service Type"
        End If
    End Sub
    Private Sub btnFindNowService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowService.Click
        FindNowService()
        SetPageService()
        ControlVisibilityService()
        upnlActionBtnModification.Update()
        upnlActionBtnModificationTop.Update()
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Description"
        Else
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Code/Form No./Description"
        End If
    End Sub
    Private Sub dgMonitorServiceStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorServiceStatusList.Sorting
        mCompMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
        SetGridService()
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Description"
        Else
            dgMonitorServiceStatusList.HeaderRow.Cells(5).Text = "Code/Form No./Description"
        End If
    End Sub
    'Added By Vikrant On 26-Jun-2014
    Private Sub imgHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHome.Click
        MarkLog(Util.Action.Close, "Component Service Status", " Part : " & mCompStatus.PartName & " Serial No. : " & mCompStatus.Comp.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        RemoveAllSessionValues()
        Response.Redirect("wfMachine_Ajax.aspx?BackPage=Index.aspx")
    End Sub
    'End
    Private Sub btnPrintService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintService.Click, btnPrintTopService.Click
        Rpt = New crListComponentMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                  Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                  "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                 "Period", "Component",
                    , "Assembly"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                                Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                                "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                      "", "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                                Me.mCompStatus.Comp.PartName, , , , , , , , , , , , , , , , ,
                                "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                            Me.mCompStatus.Comp.PartName, , , , , , , , , , , , , , , , ,
                            "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                                   "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                    Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                      Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                      "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                              "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                    Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                     Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                    Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                     Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                     "", , , , , , , , , , , , , , , , ,
                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                    CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                    CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                    , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                        CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
            End If
        Next

        'For Component Service Status List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblInfoService.Text))


        'For Component Service Status List
        ReportDetails.Add(New rptStatus(, 2, ,
              , , , dgMonitorServiceStatusList.Columns.Item(3).HeaderText, dgMonitorServiceStatusList.Columns.Item(11).HeaderText, dgMonitorServiceStatusList.Columns.Item(4).HeaderText,
              dgMonitorServiceStatusList.Columns.Item(5).HeaderText, dgMonitorServiceStatusList.Columns.Item(6).HeaderText,
              dgMonitorServiceStatusList.Columns.Item(7).HeaderText, dgMonitorServiceStatusList.Columns.Item(8).HeaderText,
                dgMonitorServiceStatusList.Columns.Item(9).HeaderText, , , , , , , , , , , , , , , dgMonitorServiceStatusList.Columns.Item(10).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mCompMonitorServiceStatusList.Count
        Dim m As Integer

        Dim str(8) As String

        For m = 0 To TotalCount1 - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            'Commented by Saylee on 1/04/2008 Suggested by Deven sir
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceStatusList.Rows(m).Cells(1).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceStatusList.Rows(m).Cells(2).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text

            'Added by Saylee on 1/04/2008 Suggested by Deven sir
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(3) = Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(4) = Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(5) = Me.dgMonitorServiceStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgMonitorServiceStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(7) = Me.dgMonitorServiceStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(8) = Me.dgMonitorServiceStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
                   , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , , , str(7), str(8)))
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Component Service Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptimage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptimage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 1-Aug-2011 For All19072011
        '    MarkLog(Util.Action.Print, "CompMonitorServiceStatus", "Component Service Status List Report", Util.ErrorType.NoError, Guid.Empty)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnCloseService_Click(sender As Object, e As System.EventArgs) Handles btnCloseService.Click, btnCloseTopService.Click
        MarkLog(Util.Action.Close, "Component Service Status", " Part : " & mCompStatus.PartName & " Serial No. : " & mCompStatus.Comp.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionService()
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlContainer.Update()
    End Sub
#End Region

#End Region

#Region " Insp Tab "
#Region " Business Methods "
    Private Sub GetSessionInspection()
        mCompMonitorInspStatusList = CType(Session("mCompMonitorInspStatusList"), tmpCompMonitorInspStatusList)
        mPartMonitorInspTypeList = CType(Session("mPartMonitorInspTypeList"), PartMonitorInspTypeList)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub SetSessionInspection()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
        Session("mPartMonitorInspTypeList") = mPartMonitorInspTypeList
        Session("mMachine") = mMachine
        Session("mMachineMaintenance") = mMachineMaintenance 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub RemoveSessionInspection()
        mCompMonitorInspStatusList = Nothing
        mPartMonitorInspTypeList = Nothing
        Session.Remove("mPartMonitorInspTypeList")
        Session.Remove("mCompMonitorInspStatusList")
        Session.Remove("LookIn")
        Session.Remove("txtFor")
        Session.Remove("txtCode")
        Session.Remove("SearchFor")
        Session.Remove("mFileAttach")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Sub SetGridInspection()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorInspStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorInspStatusList.Rows(j).Cells(19).Text, Boolean)
            If B = False Then
                dgMonitorInspStatusList.Rows(j).Cells(18).Enabled = False
            End If
        Next
    End Sub
    Private Sub NewRecordInspection()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, CStr(mAssemblyStatus.AsOnDate), mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mHourType)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MarkLog(Util.Action.[New], "Component Inspection Status", "", Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
        'End
        'Code  Added By Saylee on 1/4/2008 Suggested by Deven Sir
        Session("EditMasterRecord") = "False"
        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
        If mComponentMaintananceListCount Is Nothing Or mComponentMaintananceListCount.MaintenanceInspListCount = 0 Then
            Dim mPartMonitorInsp As PartMonitorInsp
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mPartMonitorInsp = PartMonitorInsp.NewPartMonitorInsp(ID, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mHourType, ID)
            Session.Remove("mPartMonitorInspList")
            Session("mPartMonitorInsp") = mPartMonitorInsp
            MarkLog(Util.Action.[New], "Part Insp", "", Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow();", True)
        ElseIf mComponentMaintananceListCount.MaintenanceInspListCount > 0 Then
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage6=wfAssemblyStatus_Ajax.aspx');", True)
        End If

    End Sub
    Private Sub EditRecordInspection(ByVal mId As Guid)
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
            mRegNo = "Reg No. : " & mMachine.RegNo
        End If
        '*********************

        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        mCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mHourType, IsForSpareComp:=mCompStatus.IsSpareComp)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
        MarkLog(Util.Action.Edit, "Component Inspection Status", MachineDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
        'End
        Session.Remove("mFileAttach")
        Response.Redirect("wfCompMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&GChildPage4=wfCompStatus_Ajax.aspx")
    End Sub
    'code added by Saylee on 1/04/2008 Suggested by Deven Sir
    Private Sub EditMasterRecordInspection(ByVal mMasterId As Guid, ByVal mId As Guid)
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
            mRegNo = "Reg No. : " & mMachine.RegNo
            Session("mMachine") = mMachine
        End If
        '*********************
        Dim mPartMonitorInsp As PartMonitorInsp
        mPartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(mMasterId, mHourType)
        mCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mHourType)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mMachine") = mMachine
        Session("mPartMonitorInsp") = mPartMonitorInsp
        'RemoveSession()
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mPartMonitorInsp.PartMonitorInspTypeName
        MarkLog(Util.Action.Edit, "Component Inspection Status", MachineDetail, Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)
        'End
        Session.Remove("mFileAttach")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow();", True)
    End Sub
    Private Sub DeleteRecordInspection(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteInspection")
        mCompMonitorInspStatusList.CurrentIndex = Index
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
    End Sub
    Private Sub SetPageInspection()
        If mCompStatus.IsNew Then
            lblTitle.Text = "Component Status [New]"
        Else
            lblTitle.Text = "Component Status [Part: " & mCompStatus.PartName & " SerialNo:" & mCompStatus.SerialNo & " ]"
        End If
        'set the title 
        'CNDC
        'lblInfo.Text = "List of all the Inspections on the Component as of Date: " & CStr(mCompStatus.AsOnDate) & ". All the values of all the Inspections will be as of Date: " & CStr(mCompStatus.AsOnDate)
        lblInfoInspection.Text = "List of all the Inspections on the Component as of Date: " & mCompStatus.AsOnDateFormatted & ". All the values of all the Inspections will be as of Date: " & mCompStatus.AsOnDateFormatted
        lblCountInspection.Text = "List of Component Inspection Status: " & dgMonitorInspStatusList.Rows.Count & " Record(s)"
    End Sub
    Private Sub addAttributesInspection()
        txtCodeInspection.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeInspection').value,event)")
    End Sub
    Private Sub DisplayControlsInspection(ByVal Index As Integer)
        txtForInspection.Text = IIf(Index = 3, txtForInspection.Text, "")
        txtCodeInspection.Text = IIf(Index = 1, txtCodeInspection.Text, "")
        txtCodeInspection.Visible = IIf(Index = 1, True, False)
        txtForInspection.Visible = IIf(Index = 3, True, False)
        lblForInspection.Visible = (Index > 0 And Index <> 4)
        cmbSearchForInspection.Visible = (Index = 2)
    End Sub
    Private Sub SetControlsInspection()
        '======Function added By Saylee on 29-th-Jan-2008 for bug-Component Inspection List (CIL1)
        txtForInspection.Text = Session("txtFor")
        txtCodeInspection.Text = Session("txtCode")
        cmbLookInInspection.SelectedIndex = Session("LookIn")
        cmbSearchForInspection.SelectedValue = IIf(SearchFor = "", 0, SearchFor)
        DisplayControlsInspection(cmbLookInInspection.SelectedIndex)
        'FindNow()
    End Sub
    Private Sub SetRightsInspection()

        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentInspectionPrint")) Then
                btnPrintInspection.Enabled = False
                btnPrintInspection.ToolTip = "You are not authorized user"
                btnPrintTopInspection.Enabled = False
                btnPrintTopInspection.ToolTip = "You are not Authorized user"
            End If
            If (User.IsInRole("MachineComponentInspectionNew")) = False Then
                btnAddNewInspection.Enabled = False
                btnAddNewInspection.ToolTip = "You are not authorized user"
                btnAddNewTopInspection.Enabled = False
                btnAddNewTopInspection.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentInspectionPrint")) Then
                btnPrintInspection.Enabled = False
                btnPrintInspection.ToolTip = "You are not authorized user"
                btnPrintTopInspection.Enabled = False
                btnPrintTopInspection.ToolTip = "You are not Authorized user"
            End If
            If (User.IsInRole("MachineComponentInspectionNew")) = False Then
                btnAddNewInspection.Enabled = False
                btnAddNewInspection.ToolTip = "You are not authorized user"
                btnAddNewTopInspection.Enabled = False
                btnAddNewTopInspection.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    Private Sub ControlVisibilityInspection() 'Added By Utkarsh On 21-Mar-2011
        btnPrintInspection.Enabled = IIf(mCompMonitorInspStatusList.Count > 0, True, False)
        btnPrintTopInspection.Enabled = IIf(mCompMonitorInspStatusList.Count > 0, True, False)
        btnAddNewTopInspection.Visible = (mCompMonitorInspStatusList.Count > 10)
        btnPrintTopInspection.Visible = (mCompMonitorInspStatusList.Count > 10)
        btnCloseTopInsp.Visible = (mCompMonitorInspStatusList.Count > 10)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub FindNowInspection()
        dgMonitorInspStatusList.PageIndex = 0
        Dim TransDate As String
        If IsDate(mCompStatus.AsOnDateFormatted.ToString) And IsDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
            If CDate(mCompStatus.AsOnDateFormatted.ToString) > CDate(mCompStatus.AsOnDateFormatted.ToString) Then
                TransDate = mCompStatus.AsOnDateFormatted.ToString
            Else
                TransDate = mAssemblyStatus.AsOnDateFormatted.ToString
            End If
        End If
        Select Case cmbLookInInspection.SelectedIndex
            Case 0, -1  'All
                mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(TransDate, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 1  'ATA Code
                mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(TransDate, mCompStatus.CompID, True, Val(txtCodeInspection.Text), , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 2  'Insp Type ID
                mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(TransDate, mCompStatus.CompID, True, , , , CInt(cmbSearchForInspection.SelectedValue), , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 3 ' Work Order No.
                mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(TransDate, mCompStatus.CompID, True, , , , , txtForInspection.Text.Trim, , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 4  'Show In C of A
                mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(TransDate, mCompStatus.CompID, True, , , , , , True, mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
        End Select
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mCompMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()

        'Added By Saylee on 29th-Jan-2008===============
        Session("LookIn") = cmbLookInInspection.SelectedIndex
        Session("txtFor") = txtForInspection.Text
        Session("txtCode") = txtCodeInspection.Text
        SearchFor = IIf(cmbSearchForInspection.SelectedIndex <= 0, "", cmbSearchForInspection.SelectedValue)
        Session("SearchFor") = SearchFor
        '==================================================
        SetGridInspection()
    End Sub
    Private Sub DataFieldBindInspection()
        If IsDate(mCompStatus.AsOnDateFormatted.ToString) And IsDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
            If CDate(mCompStatus.AsOnDateFormatted.ToString) > CDate(mCompStatus.AsOnDateFormatted.ToString) Then
                mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Else
                mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mAssemblyStatus.AsOnDateFormatted.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            End If
        End If

        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
        mPartMonitorInspTypeList = PartMonitorInspTypeList.GetPartMonitorInspTypeList("(All)")
        cmbSearchForInspection.DataSource = mPartMonitorInspTypeList
        Session("mPartMonitorInspTypeList") = mPartMonitorInspTypeList
        dgMonitorInspStatusList.DataSource = mCompMonitorInspStatusList
        upnlInspection.DataBind()
        SearchFor = Session("SearchFor")
    End Sub
#End Region

#Region " Events "
    Private Sub btnAddNewInspection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewInspection.Click, btnAddNewTopInspection.Click
        NewRecordInspection()
    End Sub
    Private Sub dgMonitorInspStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Dim Index As Integer
        Dim mId As Guid
        Dim mMasterId As Guid
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = False Then

            mRegNo = "Reg No. : " & mMachine.RegNo
        End If
        '*********************
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                mId = New Guid(dgMonitorInspStatusList.Rows(Index).Cells(0).Text)
                'Added By Prashant 15-Mar-2011
                If (User.IsInRole("MachineComponentInspectionView") Or User.IsInRole("MachineComponentInspectionEdit")) = False Then
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorInspStatusList(mId).MonitorType
                    MarkLog(Util.Action.Edit, "Component Inspection Status", User.Identity.Name & " is not Authorized User to edit " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '---------------------------------
                EditRecordInspection(mId)
                'Added by Saylee on 1/04/2008
            Case "EditMaster"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                mId = New Guid(dgMonitorInspStatusList.Rows(Index).Cells(0).Text)
                mMasterId = New Guid(dgMonitorInspStatusList.Rows(Index).Cells(1).Text)
                'Added By Prashant 15-Mar-2011
                If (User.IsInRole("MachineComponentInspectionView") Or User.IsInRole("MachineComponentInspectionEdit")) = False Then
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorInspStatusList(mId).MonitorType
                    MarkLog(Util.Action.Edit, "Component Inspection Status", User.Identity.Name & " is not Authorized User to edit master " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'End
                Session("EditMasterRecord") = "True"
                EditMasterRecordInspection(mMasterId, mId)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                mId = New Guid(dgMonitorInspStatusList.Rows(Index).Cells(0).Text)
                'Added By Prashant 15-Mar-2011
                If (User.IsInRole("MachineComponentInspectionDelete")) = False Then
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorInspStatusList(mId).MonitorType
                    MarkLog(Util.Action.Delete, "Component Inspection Status", User.Identity.Name & " is not Authorized User to delete " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '---------------------------------
                DeleteRecordInspection(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(New Guid(dgMonitorInspStatusList.Rows(Index).Cells(0).Text))
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub cmbLookInInspection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLookInInspection.SelectedIndexChanged
        cmbSearchForInspection.SelectedIndex = 0
        DisplayControlsInspection(cmbLookInInspection.SelectedIndex)
        If cmbLookInInspection.Enabled = True Then
            cmbLookInInspection.Focus()
        End If
    End Sub
    Private Sub btnFindNowInspection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowInspection.Click
        FindNowInspection()
        SetPageInspection()
        ControlVisibilityInspection()
        upnlActionBtnModification.Update()
        upnlActionBtnModificationTop.Update()
    End Sub
    Private Sub dgMonitorInspStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorInspStatusList.Sorting
        mCompMonitorInspStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mCompMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
        SetGridInspection()
    End Sub
    Private Sub btnCloseInsp_Click(sender As Object, e As System.EventArgs) Handles btnCloseInsp.Click, btnCloseTopInsp.Click
        MarkLog(Util.Action.Close, "Component Inspection Status", " Part : " & mCompStatus.PartName & " Serial No. : " & mCompStatus.Comp.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionInspection()
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlContainer.Update()
    End Sub
#End Region

#Region " Report "
    '    'Created By :- Jyoti
#Region " Event "
    Private Sub btnPrintInspection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintInspection.Click, btnPrintTopInspection.Click
        Rpt = New crListComponentMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                  Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                  "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                 "Period", "Component",
                    , "Assembly"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                                Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                                "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                      "", "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                                Me.mCompStatus.Comp.PartName, , , , , , , , , , , , , , , , ,
                                "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                            Me.mCompStatus.Comp.PartName, , , , , , , , , , , , , , , , ,
                            "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                                   "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                    Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                      Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                      "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                              "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                    Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                     Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                    Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                     Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                     "", , , , , , , , , , , , , , , , ,
                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                    CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                    CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                    , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                        CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
            End If
        Next

        'For Component Inspection Status List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblInfoInspection.Text))

        'For Component Inspection Status List
        ReportDetails.Add(New rptStatus(, 2, , , , , dgMonitorInspStatusList.Columns.Item(3).HeaderText, dgMonitorInspStatusList.Columns.Item(11).HeaderText, dgMonitorInspStatusList.Columns.Item(4).HeaderText,
              dgMonitorInspStatusList.Columns.Item(5).HeaderText, dgMonitorInspStatusList.Columns.Item(6).HeaderText,
             dgMonitorInspStatusList.Columns.Item(7).HeaderText, dgMonitorInspStatusList.Columns.Item(8).HeaderText,
             dgMonitorInspStatusList.Columns.Item(9).HeaderText, , , , , , , , , , , , , , , dgMonitorInspStatusList.Columns.Item(10).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mCompMonitorInspStatusList.Count
        Dim m As Integer

        Dim str(8) As String

        For m = 0 To TotalCount1 - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""

            'Commented by Saylee on 1/04/2008 Suggested by Deven sir
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorInspStatusList.Rows(m).Cells(1).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorInspStatusList.Rows(m).Cells(2).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text

            'Added by Saylee on 1/04/2008 Suggested by Deven sir
            If Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(0) = Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(2) = Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(3) = Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(4) = Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(5) = Me.dgMonitorInspStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgMonitorInspStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(7) = Me.dgMonitorInspStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(8) = Me.dgMonitorInspStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)


            ReportDetails.Add(New rptStatus(, 3, ,
                   , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , , , str(7), str(8)))
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Component Inspection Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 1-March-2012
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage) 'Added by Shweta on 1-March-2012
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 1-Aug-2011 For All19072011
        '   MarkLog(Util.Action.Print, "CompMonitorInspStatus", "Component Inspection Status List Report", Util.ErrorType.NoError, Guid.Empty)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region
#End Region

#Region " Mod Tab "
#Region " Business Methods "
    Private Sub GetSessionModification()
        mCompMonitorModStatusList = CType(Session("mCompMonitorModStatusList"), tmpCompMonitorModStatusList)
        mPartMonitorModTypeList = CType(Session("mPartMonitorModTypeList"), PartMonitorModTypeList)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub SetSessionModification()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
        Session("mPartMonitorModTypeList") = mPartMonitorModTypeList
        Session("mMachineMaintenance") = mMachineMaintenance 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub RemoveSessionModification()
        mCompMonitorModStatusList = Nothing
        mPartMonitorModTypeList = Nothing
        Session.Remove("mPartMonitorModTypeList")
        Session.Remove("mCompMonitorModStatusList")
        Session.Remove("LookIn")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 13th-Oct-2009
        Session.Remove("txtFor")
        Session.Remove("txtCode")
        Session.Remove("SearchFor")
        Session.Remove("mFileAttach")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'Added By Vikrant On 26-Jun-2014
    Private Sub RemoveAllSessionValues()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
        Session.Remove("mCompStatus")
        Session.Remove("mtmpInstalledCompList")
        Session.Remove("LookIn")
        Session.Remove("txtFor")
        Session.Remove("txtCode")
        Session.Remove("SearchFor")
        Session.Remove("mPeriodListForCompStatus")
        Session.Remove("mPartlist")
        Session.Remove("mSelectPeriods")
        Session.Remove("mManufacturerList")
        Session.Remove("IsOpenFromMaster")
        Session.Remove("myReport")
        Session.Remove("mFirstThrustCompStatus")
    End Sub
    'End
    Private Sub NewRecordModification()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Dim mCompMonitorModStatus As CompMonitorModStatus
        mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, CStr(mAssemblyStatus.AsOnDate), mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mHourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MarkLog(Util.Action.[New], "Component Modification Status", "", Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
        'End

        'Code  Added By Saylee on 1/4/2008 Suggested by Deven Sir
        Session("EditMasterRecord") = "False"
        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
        If mComponentMaintananceListCount Is Nothing Or mComponentMaintananceListCount.MaintenanceModListCount = 0 Then
            Dim mPartMonitorMod As PartMonitorMod
            mPartMonitorMod = PartMonitorMod.NewPartMonitorMod(Guid.NewGuid, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mHourType)
            Session.Remove("mPartMonitorModList")
            Session("mPartMonitorMod") = mPartMonitorMod
            MarkLog(Util.Action.[New], "Part Modification", "", Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow();", True)
        ElseIf mComponentMaintananceListCount.MaintenanceModListCount > 0 Then
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorModList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorModList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage6=wfAssemblyStatus_Ajax.aspx');", True)
        End If

    End Sub
    Private Sub EditRecordModification(ByVal mId As Guid)
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
            mRegNo = "Reg No. : " & mMachine.RegNo
        End If
        '*********************


        Dim mCompMonitorModStatus As CompMonitorModStatus
        mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mHourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
        MarkLog(Util.Action.Edit, "Component Modification Status", MachineDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
        'End
        Session.Remove("mFileAttach")
        Response.Redirect("wfCompMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&GChildPage4=wfCompStatus_Ajax.aspx")
    End Sub
    'code added by Saylee on 1/04/2008 Suggested by Deven Sir
    Private Sub EditMasterRecordModification(ByVal mMasterId As Guid, ByVal mId As Guid)
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
            mRegNo = "Reg No. : " & mMachine.RegNo
            Session("mMachine") = mMachine
        End If
        '*********************
        Dim mPartMonitorMod As PartMonitorMod
        mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mMasterId, mHourType)
        mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mHourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mMachine") = mMachine
        Session("mPartMonitorMod") = mPartMonitorMod
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mPartMonitorMod.PartMonitorModTypeName
        MarkLog(Util.Action.Edit, "Component Modification Status", MachineDetail, Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
        'End
        'RemoveSession()
        Session.Remove("mFileAttach")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow();", True)
    End Sub
    Private Sub DeleteRecordModification(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteModification")
        mCompMonitorModStatusList.CurrentIndex = Index
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
    End Sub
    Private Sub SetPageModification()
        If mCompStatus.IsNew Then
            lblTitle.Text = "Component Status [New]"
        Else
            lblTitle.Text = "Component Status [Part: " & mCompStatus.PartName & " SerialNo:" & mCompStatus.SerialNo & " ]"
        End If
        'set the title
        'CNDC
        'lblInfo.Text = "List of all the Modifications on the Component as of Date: " & CStr(mCompStatus.AsOnDate) & ". All the values of all the Modifications will be as of Date: " & CStr(mCompStatus.AsOnDate)
        lblInfoModification.Text = "List of all the Modifications on the Component as of Date: " & mCompStatus.AsOnDateFormatted & ". All the values of all the Modifications will be as of Date: " & mCompStatus.AsOnDateFormatted
        lblCountModification.Text = "List of Component Modification Status: " & dgMonitorModStatusList.Rows.Count & " Record(s)"
    End Sub
    Private Sub addAttributesModification()
        txtCodeModification.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeModification').value,event)")
    End Sub
    Private Sub DisplayControlsModification(ByVal Index As Integer)
        txtForModification.Text = IIf(Index = 2 Or Index = 4, txtForModification.Text, "")
        txtCodeModification.Text = IIf(Index = 1, txtCodeModification.Text, "")
        txtCodeModification.Visible = IIf(Index = 1, True, False)
        txtForModification.Visible = IIf(Index = 2 Or Index = 4, True, False)
        lblForModification.Visible = (Index > 0 And Index <> 5)
        cmbSearchForModification.Visible = (Index = 3)
    End Sub
    Private Sub SetControlsModification()
        '======Function added By Saylee on 29-th-Jan-2008 for bug-Component Modification List (CIL1)
        txtForModification.Text = Session("txtFor")
        txtCodeModification.Text = Session("txtCode")
        cmbLookInModification.SelectedIndex = Session("LookIn")
        cmbSearchForModification.SelectedValue = IIf(SearchFor = "", 0, SearchFor)
        DisplayControlsModification(cmbLookInModification.SelectedIndex)
        'FindNow()
    End Sub
    Private Sub SetRightsModification()
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentModificationPrint")) Then
                btnPrintModification.Enabled = False
                btnPrintModification.ToolTip = "You are not authorized user"
                btnPrintTopModification.Enabled = False
                btnPrintTopModification.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineComponentModificationNew")) = False Then
                btnAddNewModification.Enabled = False
                btnAddNewTopModification.Enabled = False
                btnAddNewModification.ToolTip = "You are not Authorized user"
                btnAddNewTopModification.ToolTip = "You are not Authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentModificationPrint")) Then
                btnPrintModification.Enabled = False
                btnPrintModification.ToolTip = "You are not authorized user"
                btnPrintTopModification.Enabled = False
                btnPrintTopModification.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineComponentModificationNew")) = False Then
                btnAddNewModification.Enabled = False
                btnAddNewTopModification.Enabled = False
                btnAddNewModification.ToolTip = "You are not Authorized user"
                btnAddNewTopModification.ToolTip = "You are not Authorized user"
            End If
        End If
    End Sub
    Private Sub ControlVisibilityModification() 'Added By Utkarsh On 21-Mar-2011
        btnPrintModification.Enabled = IIf(mCompMonitorModStatusList.Count > 0, True, False)
        btnPrintTopModification.Enabled = IIf(mCompMonitorModStatusList.Count > 0, True, False)
        btnAddNewTopModification.Visible = (mCompMonitorModStatusList.Count > 10)
        btnPrintTopModification.Visible = (mCompMonitorModStatusList.Count > 10)
        btnCloseTopMod.Visible = (mCompMonitorModStatusList.Count > 10)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub FindNowModification()
        dgMonitorModStatusList.PageIndex = 0
        Dim TransDate As String
        If IsDate(mCompStatus.AsOnDateFormatted.ToString) And IsDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
            If CDate(mCompStatus.AsOnDateFormatted.ToString) > CDate(mCompStatus.AsOnDateFormatted.ToString) Then
                TransDate = mCompStatus.AsOnDateFormatted.ToString
            Else
                TransDate = mAssemblyStatus.AsOnDateFormatted.ToString
            End If
        End If
        Select Case cmbLookInModification.SelectedIndex
            Case 0, -1  'All
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(TransDate, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 1  'ATA Code
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(TransDate, mCompStatus.CompID, True, Val(txtCodeModification.Text), , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 2 ' Mod No
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(TransDate, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, DirectiveNo:=txtForModification.Text.Trim, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 3  'Mod Type ID
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(TransDate, mCompStatus.CompID, True, , , , CInt(cmbSearchForModification.SelectedValue), , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 4 ' Work Order No.
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(TransDate, mCompStatus.CompID, True, , , , , txtForModification.Text.Trim, , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Case 5  'Show In C of A
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(TransDate, mCompStatus.CompID, True, , , , , , True, mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
        End Select

        dgMonitorModStatusList.DataSource = mCompMonitorModStatusList
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
        dgMonitorModStatusList.DataBind()
        'Added By Saylee on 29th-Jan-2008===============
        Session("LookIn") = cmbLookInModification.SelectedIndex
        Session("txtFor") = txtForModification.Text
        Session("txtCode") = txtCodeModification.Text
        SearchFor = IIf(cmbSearchForModification.SelectedIndex <= 0, "", cmbSearchForModification.SelectedValue)
        Session("SearchFor") = SearchFor
        '==================================================
        SetGridDirective()
    End Sub
    Private Sub DataFieldBindModification()
        If IsDate(mCompStatus.AsOnDateFormatted.ToString) And IsDate(mAssemblyStatus.AsOnDateFormatted.ToString) Then
            If CDate(mCompStatus.AsOnDateFormatted.ToString) > CDate(mCompStatus.AsOnDateFormatted.ToString) Then
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(mCompStatus.AsOnDateFormatted.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            Else
                mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
            End If
        End If

        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
        mPartMonitorModTypeList = PartMonitorModTypeList.GetPartMonitorModTypeList("(All)")
        cmbSearchForModification.DataSource = mPartMonitorModTypeList
        dgMonitorModStatusList.DataSource = mCompMonitorModStatusList
        upnlModification.DataBind()
        SearchFor = Session("SearchFor")
    End Sub

#End Region

#Region " Events "
    Private Sub btnAddNewModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewModification.Click, btnAddNewTopModification.Click
        NewRecordModification()
    End Sub
    Private Sub dgMonitorModStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorModStatusList.RowCommand
        Dim Index As Integer
        Dim mId As Guid
        Dim mMasterId As Guid
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = False Then

            mRegNo = "Reg No. : " & mMachine.RegNo
        End If
        '*********************
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                mId = New Guid(dgMonitorModStatusList.Rows(Index).Cells(0).Text)
                'Added By Prashant 15-Mar-2011
                If (User.IsInRole("MachineComponentModificationView") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorModStatusList(mId).MonitorType
                    MarkLog(Util.Action.Edit, "Component Modification Status", User.Identity.Name & " is not Authorized User to edit " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'End
                EditRecordModification(mId)
            Case "EditMaster"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                mId = New Guid(dgMonitorModStatusList.Rows(Index).Cells(0).Text)
                mMasterId = New Guid(dgMonitorModStatusList.Rows(Index).Cells(1).Text)
                'Added By Prashant 15-Mar-2011
                If (User.IsInRole("MachineComponentModificationView") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                    'Added By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorModStatusList(mId).MonitorType
                    MarkLog(Util.Action.Edit, "Component Modification Status", User.Identity.Name & " is not Authorized User to edit master " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'End
                Session("EditMasterRecord") = "True"
                EditMasterRecordModification(mMasterId, mId)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                mId = New Guid(dgMonitorModStatusList.Rows(Index).Cells(0).Text)
                If (User.IsInRole("MachineComponentModificationDelete")) = False Then
                    'Added By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo & " Monitor Info : " & mCompMonitorModStatusList(mId).MonitorType
                    MarkLog(Util.Action.Delete, "Component Modification Status", User.Identity.Name & " is not Authorized User to delete " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordModification(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(New Guid(dgMonitorModStatusList.Rows(Index).Cells(0).Text))
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub cmbLookInModification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInModification.SelectedIndexChanged
        cmbSearchForModification.SelectedIndex = 0
        DisplayControlsModification(cmbLookInModification.SelectedIndex)
        cmbLookInModification.Focus()
    End Sub
    Private Sub btnFindNowModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowModification.Click
        FindNowModification()
        SetPageModification()
        ControlVisibilityModification()
        upnlActionBtnModification.Update()
        upnlActionBtnModificationTop.Update()
    End Sub
    Private Sub dgMonitorModStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorModStatusList.Sorting
        mCompMonitorModStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
        dgMonitorModStatusList.DataSource = mCompMonitorModStatusList
        dgMonitorModStatusList.DataBind()
        SetGridDirective()
    End Sub
    Private Sub btnCloseMod_Click(sender As Object, e As System.EventArgs) Handles btnCloseMod.Click, btnCloseTopMod.Click
        MarkLog(Util.Action.Close, "Component Modification Status", " Part : " & mCompStatus.PartName & " Serial No. : " & mCompStatus.Comp.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionModification()
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlContainer.Update()
    End Sub
#End Region

#Region " Report "
    '    'Created By :- Jyoti
#Region " Event "
    Private Sub btnPrintModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintModification.Click, btnPrintTopModification.Click
        Rpt = New crListComponentMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                  Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                  "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                 "Period", "Component",
                    , "Assembly"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Code",
                                Me.mCompStatus.Comp.Code, , , , , , , , , , , , , , , , ,
                                "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                      "", "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                                Me.mCompStatus.Comp.PartName, , , , , , , , , , , , , , , , ,
                                "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Part No.",
                            Me.mCompStatus.Comp.PartName, , , , , , , , , , , , , , , , ,
                            "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                                   "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                    Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Description",
                                      Me.mCompStatus.Comp.Description, , , , , , , , , , , , , , , , ,
                                      "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                              "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                    Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Serial No.",
                                     Me.mCompStatus.Comp.SerialNo, , , , , , , , , , , , , , , , ,
                                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                    Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                    "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                            CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                            , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "Position",
                                     Me.mCompStatus.Position, , , , , , , , , , , , , , , , ,
                                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                     "", , , , , , , , , , , , , , , , ,
                     "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                    CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                    CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                    , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                             "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of Component", "",
                                         "", , , , , , , , , , , , , , , , ,
                                         "Since New Value as on " & mCompStatus.AsOnDateFormatted,
                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String),
                                        CType(Me.mCompStatus.CompStatusPeriods(I).CompCurrentValueFormatted, String),
                                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
            End If
        Next

        'For Component Modification Status List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblInfoModification.Text))

        'For Component Modification Status List
        'ReportDetails.Add(New rptStatus(, 2, , , , , dgMonitorModStatusList.Columns.Item(3).HeaderText, dgMonitorModStatusList.Columns.Item(4).HeaderText, dgMonitorModStatusList.Columns.Item(5).HeaderText, _
        '     dgMonitorModStatusList.Columns.Item(6).HeaderText, dgMonitorModStatusList.Columns.Item(7).HeaderText, _
        '     dgMonitorModStatusList.Columns.Item(8).HeaderText, dgMonitorModStatusList.Columns.Item(9).HeaderText, _
        '        dgMonitorModStatusList.Columns.Item(10).HeaderText, , , , , , , , , , , , , , , dgMonitorModStatusList.Columns.Item(11).HeaderText))
        ReportDetails.Add(New rptStatus(, 2, ,
                    , , , dgMonitorModStatusList.Columns.Item(3).HeaderText, dgMonitorModStatusList.Columns.Item(11).HeaderText, dgMonitorModStatusList.Columns.Item(4).HeaderText,
                    dgMonitorModStatusList.Columns.Item(5).HeaderText, dgMonitorModStatusList.Columns.Item(6).HeaderText,
                    dgMonitorModStatusList.Columns.Item(7).HeaderText, dgMonitorModStatusList.Columns.Item(8).HeaderText,
                      dgMonitorModStatusList.Columns.Item(9).HeaderText, , , , , , , , , , , , , , , dgMonitorModStatusList.Columns.Item(10).HeaderText, RHData6:=dgMonitorModStatusList.Columns.Item(12).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mCompMonitorModStatusList.Count
        Dim m As Integer

        Dim str(9) As String

        For m = 0 To TotalCount1 - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            str(9) = ""

            'Commented by Saylee on 1/04/2008 Suggested by Deven sir
            'If Me.dgMonitorModStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorModStatusList.Rows(m).Cells(1).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorModStatusList.Rows(m).Cells(2).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorModStatusList.Rows(m).Cells(3).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMonitorModStatusList.Rows(m).Cells(4).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMonitorModStatusList.Rows(m).Cells(5).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgMonitorModStatusList.Rows(m).Cells(6).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgMonitorModStatusList.Rows(m).Cells(7).Text

            'Added by Saylee on 1/04/2008 Suggested by Deven sir
            'If Me.dgMonitorModStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(0) = Me.dgMonitorModStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If mCompMonitorModStatusList(m).ModNumber <> "&nbsp;" Then str(0) = mCompMonitorModStatusList(m).ModNumber.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgMonitorModStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(2) = Me.dgMonitorModStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(3) = Me.dgMonitorModStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(4) = Me.dgMonitorModStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(5) = Me.dgMonitorModStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgMonitorModStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(7) = Me.dgMonitorModStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(8) = Me.dgMonitorModStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(12).Text <> "&nbsp;" Then str(9) = Me.dgMonitorModStatusList.Rows(m).Cells(12).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
                   , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , str(9), , str(7), str(8)))
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Component Modification Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 1-March-2012
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage) 'Added by Shweta on 1-March-2012
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 1-Aug-2011 For All19072011
        '   MarkLog(Util.Action.Print, "CompMonitorInspStatus", "Component Modification Status List Report", Util.ErrorType.NoError, Guid.Empty)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region
#End Region




End Class