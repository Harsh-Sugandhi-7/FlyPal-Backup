
'AJAX Created By Saylee On 24-Apr-2015

Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.PartListAutoComplete

Public Class wfInstallComp_AJAX
    Inherits System.Web.UI.Page

#Region " Install Component "

#Region " Enum "
    Public Enum From
        NewInstall = 1
        EditInstall = 2
        FromInstallAssembly = 3
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

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mRemovedCompStatus As CompStatus
    Public mCompStatus As CompStatus
    Public mAssemblyList As AssemblyList
    Public mPartList As PartList
    Public mSelectPeriods As SelectPeriods

    Public mPeriodListForCompStatus As PeriodListForCompStatus
    Public mFrom As From
    Dim Flag As Integer
    Public mATAList As ATAList

    Public mCompInstallInfo As String

    Dim LogID As String

    Dim mInstallSelected As Integer

    Public mMachineMaintenance As MachineMaintenance
    Public mMachineMaintenanceList As MachineMaintenanceList

    Dim EventLogID As Guid
    Dim MaintDetail As String
    Public mEmployeeList As EmployeeList

    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Public mManufacturerList As ManufacturerList
    Public mEmployeeStatus As EmployeeStatus

    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End
    Dim mInstallationStatusList As InstallationStatusList
    Public Shared ModelID As Guid

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
    Public mSpareAssemblyComponent As Integer 'Added by Shital on 23-Dec-2020 
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFrom = CType(Session("From"), From)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mRemovedCompStatus = CType(Session("mRemovedCompStatus"), CompStatus)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mPartList = CType(Session("mPartList"), PartList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        '  mSelectPeriod = CType(Session("mSelectPeriod"), SelectPeriod)   'AC
        mPeriodListForCompStatus = CType(Session("mPeriodListForCompStatus"), PeriodListForCompStatus)
        mATAList = CType(Session("mATAList"), ATAList)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)   'Code Added 30,Jan,2007
        LogID = CType(Session("LogID"), String)

        mMachine = Session("mMachine")

        mInstallSelected = Session("mInstallSelected") '28-Apr-2009

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 8th-Oct-2009
        mManufacturerList = Session("mManufacturerList") 'Added By Utkarsh On 31-Jan-2013 For ALL30122013

        'Added By Saylee On 27-Nov-2014 
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
        mInstallationStatusList = Session("mInstallationStatusList")
        ModelID = CType(Session("ModelID"), Guid)

        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mFirstThrustCompStatus = Session("mFirstThrustCompStatus") 'Added by Saylee on 7-Oct-2017 for Thrust
        mThrustTypeList = Session("mThrustTypeList") 'Added by Saylee on 25-May-2018 for Thrust
        mSpareAssemblyComponent = CType(Session("mSpareAssemblyComponent"), Integer) 'Added By Shital On 23-Dec-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("From") = mFrom
        Session("mCompStatus") = mCompStatus
        Session("mRemovedCompStatus") = mRemovedCompStatus
        Session("mAssemblyList") = mAssemblyList
        Session("mPartList") = mPartList
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mSelectPeriods") = mSelectPeriods

        Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
        Session("mATAList") = mATAList
        Session("mCompStatus") = mCompStatus
        Session("mMachine") = mMachine

        Session("mInstallSelected") = mInstallSelected

        Session("mMachineMaintenance") = mMachineMaintenance
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        Session("mManufacturerList") = mManufacturerList


        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
        Session("mInstallationStatusList") = mInstallationStatusList
        Session("mFirstThrustCompStatus") = mFirstThrustCompStatus 'Added by Saylee on 7-Oct-2017 for Thrust
        Session("mThrustTypeList") = mThrustTypeList 'Added by Saylee on 25-May-2018 for Thrust
    End Sub
    Private Sub RemoveSession()
        mAssemblyList = Nothing
        mPartList = Nothing
        mSelectPeriods = Nothing
        mPeriodListForCompStatus = Nothing

        Session.Remove("mAssemblyList")
        Session.Remove("mPartList")
        Session.Remove("mSelectPeriods")
        'AC
        Session.Remove("mPeriodListForCompStatus")
        Session.Remove("mATAList")

        Session.Remove("mInstallSelected")
        Session.Remove("InstallSelected")
        Session.Remove("mMachineMaintenance")
        Session.Remove("mMachineMaintenanceList")
        Session.Remove("mManufacturerList")


        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
        Session.Remove("mInstallationStatusList")

        Session.Remove("mInstallCompMonitorServiceStatusList")
        mPartMonitorServiceTypeList = Nothing

        mInstallCompMonitorInspStatusList = Nothing
        mPartMonitorInspTypeList = Nothing
        Session.Remove("mInstallCompMonitorInspStatusList")

        Session.Remove("mFileAttach")

        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
        Session.Remove("mFirstThrustCompStatus") 'Added by Saylee on 7-Oct-2017 for Thrust
        Session.Remove("mThrustTypeList")  'Added by Saylee on 25-May-2018 for Thrust
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False OrElse cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub DataGridBind()
        Session("mCompStatus") = mCompStatus
        dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
        dgInstallationValue.DataBind()
    End Sub
    Private Sub SetFormClone(ByVal clnCompStatus As CompStatus)
        mCompStatus.InstallationWONo = clnCompStatus.InstallationWONo
        mCompStatus.InstallationReason = clnCompStatus.InstallationReason
        mCompStatus.InstallationRemark = clnCompStatus.InstallationRemark
        mCompStatus.InstalledOn = clnCompStatus.InstalledOn

        mCompStatus.InstDoneByID = clnCompStatus.InstDoneByID
        mCompStatus.InstLicenseNo = clnCompStatus.InstLicenseNo
        mCompStatus.InstPlace = clnCompStatus.InstPlace
        clnCompStatus = Nothing
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        DataFieldBind()
                        Save()
                        'Response.Redirect("wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                        DataFieldBind()
                        GetAttachment()
                        SetPage()
                        ControlVisibilityForAttachment()
                        upnlPartOnfo.Update()
                        upnlInstInfo.Update()
                    End If

                    If MSGBoxCtrl.Sender = "ReqServ" Then
                        'Session("sender") = ""
                        ''Changed By Utkarsh On 26-Jul-2011 For All19072011
                        'MarkLog(Util.Action.Close, "Component Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        ''End
                        'RemoveSession()
                        'Session.Remove("FromLog")
                        'Session.Remove("TabIndex")
                        ''Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        'If Request.QueryString("GChildPage2") = "wfInstallAssembly_Ajax.aspx" Then
                        '    Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        'Else
                        '    Response.Redirect("index.aspx")
                        'End If
                        Dim URLForCompInst As New Stack
                        URLForCompInst.Push(Request.Url)
                        Session("URLForCompInst") = URLForCompInst
                        Session("TabIndex") = TbContInst.ActiveTabIndex
                        mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                        Session("mMachine") = mMachine
                        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
                        If mComponentMaintananceListCount.MaintenanceServiceListCount = 0 And mComponentMaintananceListCount.MaintenanceInspListCount > 0 Then
                            NewRecordInsp()
                        Else
                            NewRecordService()
                        End If

                    End If

                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                    ElseIf MSGBoxCtrl.Sender = "ReqServ" Then
                        Session("sender") = ""
                        'Changed By Utkarsh On 26-Jul-2011 For All19072011
                        MarkLog(Util.Action.Close, "Component Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        'End
                        RemoveSession()
                        Session.Remove("FromLog")
                        Session.Remove("TabIndex")
                        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        If Request.QueryString("GChildPage2") = "wfInstallAssembly_Ajax.aspx" Then
                            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        Else
                            Response.Redirect("index.aspx")
                        End If
                        ' Response.Redirect("wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    ElseIf MSGBoxCtrl.Sender = "NextPage" Then
                        Session("sender") = ""
                        Session.Remove("Flag")
                        Session("mAssemblyStatus") = mAssemblyStatus
                        'Response.Redirect("wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    End If
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "ReqServ" Then
                        Session("sender") = ""
                        'Response.Redirect("wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    upnlPartOnfo.Update()
                    upnlInstInfo.Update()
                    ' Response.Redirect("wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    upnlPartOnfo.Update()
                    upnlInstInfo.Update()
                    'Response.Redirect("wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ' Response.Redirect("wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If

    End Sub
    Private Sub SetObject()
        With mCompStatus
            .ATAID = New Guid(cmbATAChapter.SelectedValue)

            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                .Comp.PartID = mPartList(txtPartDescription.Text.Trim).ID
            Else
                .Comp.PartID = New Guid(cmbPartNo.SelectedValue.ToString)
            End If

            .Comp.SerialNo = txtSerialNo.Text.Trim
            .Comp.Code = Val(txtCode.Text)

            'Added By Deven on 22-11-2008 for Accumulated Cycles
            .Comp.ACF = Val(txtACF.Text)
            .Comp.ECF = Val(txtECF.Text)
            .Comp.FCF = Val(txtFCF.Text)
            '******************************************************
            mCompStatus.Comp.RTCF = IIf(txtRTCF.Text <> "", CDec(txtRTCF.Text), 0) ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor

            .Position = txtPosition.Text.Trim
            .AssemblyID = New Guid(cmbAssemblyList.SelectedValue.ToString)
            If calInstalledOn.Text = "" Then
                .InstalledOn = System.DBNull.Value
            Else
                .InstalledOn = calInstalledOn.Text
            End If
            .InstallationWONo = txtWorkOrderNo.Text.Trim
            .InstallationRemark = txtNote.Text.Trim
            .InstDoneBy = txtDoneBy.Text

            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .InstLicenseNo = LicenseNo
            .InstDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            .InstPlace = txtPlace.Text.Trim
            'End
            .ManufacturerID = New Guid(cmbManufacturerList.SelectedValue) 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
            .InstallationReason = Trim(txtInstallationReason.Text) 'Added By Vikrant On 10-Apr-2014 For ALL09042014-1

            'Added By Saylee On 27-Nov-2014  
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If

            'End
            .InstallationStatusID = CInt(cmbInstallationStatus.SelectedValue) 'Added By Vikrant On 31-Mar-2015 For All31032015

            'Added By Saylee on 6-Oct-2017 for Thrust
            .IsThrustMonitoringComp = chkIsThrustComp.Checked
            If chkIsThrustComp.Checked Then
                .B22CurrentValue = CDec(txtB22Current.Text)
                .B22LifeLimit = CDec(txtB22LifeLimit.Text)
                .B22IsCurrentThrust = chkB22IsCurrent.Checked

                .B24CurrentValue = CDec(txtB24Current.Text)
                .B24LifeLimit = CDec(txtB24LifeLimit.Text)
                .B24IsCurrentThrust = chkB24IsCurrent.Checked

                .B26CurrentValue = CDec(txtB26Current.Text)
                .B26LifeLimit = CDec(txtB26LifeLimit.Text)
                .B26IsCurrentThrust = chkB26IsCurrent.Checked

            End If
            '***********************************************
        End With
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
    Private Sub SetObjectForPart()
        'Added By Saylee on 8th-Jan-2008 for bug-IIC21 (Maintenance)
        With mCompStatus
            .ATAID = New Guid(cmbATAChapter.SelectedValue)
            .Comp.PartID = Guid.Empty 'New Guid(cmbPartNo.SelectedValue.Tostring)
            .Comp.SerialNo = txtSerialNo.Text.Trim
            .Comp.Code = Val(txtCode.Text)

            'Added By Deven on 22-11-2008 for Accumulated Cycles
            .Comp.ACF = Val(txtACF.Text)
            .Comp.ECF = Val(txtECF.Text)
            .Comp.FCF = Val(txtFCF.Text)
            '******************************************************

            .Position = txtPosition.Text.Trim
            .AssemblyID = New Guid(cmbAssemblyList.SelectedValue.ToString)
            If calInstalledOn.Text = "" Then
                .InstalledOn = System.DBNull.Value
            Else
                .InstalledOn = calInstalledOn.Text
            End If
            .InstallationWONo = txtWorkOrderNo.Text.Trim
            .InstallationRemark = txtNote.Text.Trim

            '.InstDoneByID = New Guid(cmbDoneBy.SelectedValue)
            '.InstLicenseNo = txtLicenceNo.Text.Trim
            '.InstPlace = txtPlace.Text.Trim
            'Added By Prashant On 12-Jun-2012 FOR ALL08062012
            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .InstLicenseNo = LicenseNo
            .InstDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            .InstPlace = txtPlace.Text.Trim
            'End

            'Added By Saylee On 27-Nov-2014 
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If

            'End
        End With
        Session("mCompStatus") = mCompStatus
    End Sub
    Public Sub SetAssemblyPeriod()
        If PartNo.Length > 0 Then
            If Not mPartList Is Nothing Then 'Added by Saylee on 7-May-2012 for BA07052012
                If Not mPartList(PartNo).ID.Equals(Guid.Empty) Then

                    'Dim mtmpCompStatusList As tmpCompStatusList = tmpCompStatusList.GetCompStatusList(Guid.Empty, mPartList(New Guid(cmbPartNo.SelectedValue)).Name, "", mPartList(New Guid(cmbPartNo.SelectedValue)).Description)
                    Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(mPartList(PartNo).ID.ToString, PartNo, Description)

                    If mtmpCompListOnPartSelection.Count > 0 Then
                        Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mCompStatus.AssemblyID, "")
                        'Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)
                        Dim tmpCompStatus As CompStatus
                        If mInstallSelected = 1 Then
                            If mRemovedCompStatus.IsSpareComp Then
                                tmpCompStatus = CompStatus.GetSpareCompStatus(mRemovedCompStatus.ID, True)
                                'End
                            Else
                                tmpCompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)
                            End If
                        Else
                            tmpCompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)

                        End If
                        If cmbAssemblyList.SelectedIndex > 0 Then
                            'If mFrom.NewInstall Then
                            If mFrom = From.NewInstall Or mFrom = From.EditInstall Then
                                If Not New Guid(cmbAssemblyList.SelectedValue).Equals(Guid.Empty) Then
                                    mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbAssemblyList.SelectedValue), "")
                                    Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
                                    If mCompStatus.IsNew Then 'If mFrom = From.NewInstall Then
                                        If Not IsNothing(mRemovedCompStatus) Then
                                            Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                            NewRecord(Guid.Empty, calInstalledOn.Text)
                                            CopyFromClone(clnCompStatus)
                                        Else
                                            Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                            Dim tmp As CompStatusPeriods = mCompStatus.CompStatusPeriods
                                            NewRecord(Guid.Empty, calInstalledOn.Text)
                                            If CType(Session("FromLog"), Boolean) = True Then
                                                Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
                                                Dim LogDate = CType(Session("LogDate"), String) 'Request.QueryString("LogDate")
                                                mCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, LogDate, False, LogDate, LogId.ToString)
                                            Else
                                                mCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, calInstalledOn.Text, True, calInstalledOn.Text)
                                            End If
                                            CopyFromClone(clnCompStatus)
                                        End If
                                    Else
                                        Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                        mCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mAssemblyStatus.ID, calInstalledOn.Text, Guid.Empty.ToString)
                                        CopyFromClone(clnCompStatus)
                                    End If


                                    dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                                    dgInstallationValue.DataBind()
                                    ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                                    If cmbAssemblyList.Enabled = True Then
                                        setFocus(cmbAssemblyList)
                                    End If
                                End If
                            End If
                        Else
                            Dim str As String = "abc"
                            If mCompStatus.CompStatusPeriods.Count > 0 Then
                                For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                                    mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
                                Next
                                dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                                dgInstallationValue.DataBind()
                            End If
                        End If
                        tmpPeriodListForCompStatus = Nothing
                    Else
                        If cmbAssemblyList.SelectedIndex > 0 Then
                            'If mFrom.NewInstall Then
                            If mFrom = From.NewInstall Then
                                If Not New Guid(cmbAssemblyList.SelectedValue).Equals(Guid.Empty) Then
                                    mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbAssemblyList.SelectedValue), "")
                                    Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
                                    If mFrom = From.NewInstall Then
                                        If Not IsNothing(mRemovedCompStatus) Then
                                            Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                            NewRecord(Guid.Empty, calInstalledOn.Text)
                                            CopyFromClone(clnCompStatus)
                                        Else
                                            Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                            Dim tmp As CompStatusPeriods = mCompStatus.CompStatusPeriods
                                            NewRecord(Guid.Empty, calInstalledOn.Text)
                                            If CType(Session("FromLog"), Boolean) = True Then
                                                Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
                                                Dim LogDate As String = mCompStatus.InstalledOn.ToString 'Request.QueryString("LogDate")
                                                mCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, LogDate, False, LogDate, LogId.ToString)
                                            Else
                                                mCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, calInstalledOn.Text, True, calInstalledOn.Text)
                                            End If
                                            CopyFromClone(clnCompStatus)
                                        End If
                                    Else
                                        Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                        mCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mAssemblyStatus.ID, calInstalledOn.Text, Guid.Empty.ToString)
                                        CopyFromClone(clnCompStatus)
                                    End If

                                    '----------'28-Apr-2009
                                    If Session("From") = 1 And Session("mInstallSelected") <> 1 Then
                                        SetPeroids()
                                        For i As Integer = 0 To mSelectPeriods.Count - 1
                                            mSelectPeriods(i).IsSelected = True
                                        Next
                                        'AddSelectedPeroids()
                                        Dim mSelectPeriod As SelectPeriod
                                        If IsNothing(mSelectPeriods) Then
                                            mSelectPeriods = SelectPeriods.NewSelectPeriods
                                        End If
                                        For Each mSelectPeriod In mSelectPeriods
                                            If mSelectPeriod.IsSelected Then
                                                mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mCompStatus.InstalledOn.ToString, mSelectPeriod.PeriodID, False, mCompStatus.InstalledOn.ToString))
                                            End If
                                        Next
                                        Session("mCompStatus") = mCompStatus
                                        Session.Remove("mSelectPeriods")
                                        mSelectPeriods = Nothing
                                    End If
                                    '---------

                                    dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                                    dgInstallationValue.DataBind()
                                    ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                                    If cmbAssemblyList.Enabled = True Then
                                        setFocus(cmbAssemblyList)
                                    End If
                                End If
                            End If
                        Else
                            Dim str As String = "abc"
                            If mCompStatus.CompStatusPeriods.Count > 0 Then
                                For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                                    mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
                                Next
                                dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                                dgInstallationValue.DataBind()
                            End If
                        End If
                    End If
                End If
            End If
        Else
            If cmbAssemblyList.SelectedIndex > 0 Then
                'If mFrom.NewInstall Then
                If mFrom = From.NewInstall Then
                    If Not New Guid(cmbAssemblyList.SelectedValue).Equals(Guid.Empty) Then
                        mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbAssemblyList.SelectedValue), "")
                        Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
                        If mFrom = From.NewInstall Then
                            If Not IsNothing(mRemovedCompStatus) Then
                                Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                NewRecord(Guid.Empty, calInstalledOn.Text)
                                CopyFromClone(clnCompStatus)
                            Else
                                Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                                Dim tmp As CompStatusPeriods = mCompStatus.CompStatusPeriods
                                NewRecord(Guid.Empty, calInstalledOn.Text)
                                If CType(Session("FromLog"), Boolean) = True Then
                                    Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
                                    Dim LogDate As String = mCompStatus.InstalledOn.ToString 'Request.QueryString("LogDate")
                                    mCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, LogDate, False, LogDate, LogId.ToString)
                                Else
                                    mCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, calInstalledOn.Text, True, calInstalledOn.Text)
                                End If
                                CopyFromClone(clnCompStatus)
                            End If
                        Else
                            Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                            mCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mAssemblyStatus.ID, calInstalledOn.Text, Guid.Empty.ToString)
                            CopyFromClone(clnCompStatus)
                        End If

                        '----------'28-Apr-2009
                        If Session("From") = 1 And Session("mInstallSelected") <> 1 Then
                            SetPeroids()
                            For i As Integer = 0 To mSelectPeriods.Count - 1
                                mSelectPeriods(i).IsSelected = True
                            Next
                            'AddSelectedPeroids()
                            Dim mSelectPeriod As SelectPeriod
                            If IsNothing(mSelectPeriods) Then
                                mSelectPeriods = SelectPeriods.NewSelectPeriods
                            End If
                            For Each mSelectPeriod In mSelectPeriods
                                If mSelectPeriod.IsSelected Then
                                    mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mCompStatus.InstalledOn.ToString, mSelectPeriod.PeriodID, False, mCompStatus.InstalledOn.ToString))
                                End If
                            Next
                            Session("mCompStatus") = mCompStatus
                            Session.Remove("mSelectPeriods")
                            mSelectPeriods = Nothing
                        End If
                        '---------

                        dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                        dgInstallationValue.DataBind()
                        ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                        If cmbAssemblyList.Enabled = True Then
                            setFocus(cmbAssemblyList)
                        End If
                    End If
                End If

                Dim str As String = "abc"
                If mCompStatus.CompStatusPeriods.Count > 0 Then
                    For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                        mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
                    Next
                    dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                    dgInstallationValue.DataBind()
                End If
            End If
        End If
    End Sub

    Private Sub SetGridObject()
        For i As Integer = 0 To dgInstallationValue.Rows.Count - 1
            Dim txtCompInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtCompInstallationValue"), TextBox)
            If mCompStatus.CompStatusPeriods.Item(i).PeriodID <> 2 And txtCompInstallationValue.Text.Trim = "" Then 'This If Condition added by vikrant on 19-Jun-2020 to save 0 instead of null if nothing enetered in TextBox
                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = New Period(mCompStatus.CompStatusPeriods.Item(i).PeriodID, 0).Value
            Else
                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = txtCompInstallationValue.Text.Trim
            End If
        Next
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub AddSelectedPeroids()

        Dim mSelectPeriod As SelectPeriod
        If IsNothing(mSelectPeriods) Then
            mSelectPeriods = SelectPeriods.NewSelectPeriods
        End If
        For Each mSelectPeriod In mSelectPeriods
            If mSelectPeriod.IsSelected Then
                mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mCompStatus.InstalledOn.ToString, mSelectPeriod.PeriodID, False, mCompStatus.InstalledOn.ToString))
                'Commented by Rajnish On 14-02-2008
                ''mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewChildCompStatusPeriod(mCompStatus.ID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mSelectPeriod.PeriodID, calInstalledOn.Text))
            End If
        Next
        ''SetAssemblyPeriod()
        Session("mCompStatus") = mCompStatus
        Session.Remove("mSelectPeriods")
        mSelectPeriods = Nothing
    End Sub
    Private Sub SetPeroids()
        Dim mPeriodlist As PeriodList
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        mPeriodlist = PeriodList.GetPeriodList
        If Not mPeriodListForCompStatus Is Nothing Then
            For i As Integer = 0 To mPeriodListForCompStatus.Count - 1
                If Not mCompStatus.CompStatusPeriods.Contains(mPeriodListForCompStatus(i).PeriodID) Then
                    mSelectPeriods.Add(mPeriodListForCompStatus(i).PeriodID, mPeriodListForCompStatus(i).PeriodName)
                End If
            Next
        End If
        Session("mSelectPeriods") = mSelectPeriods
    End Sub
    Private Sub ControlVisiblity1() 'Added By Prashant 26-Aug-2010
        'Dim p As Integer
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
        '-----------------------------
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

            phAC.Visible = True
        Else
            lblACF.Visible = False
            txtACF.Visible = False
            lblECF.Visible = False
            txtECF.Visible = False
            lblFCF.Visible = False
            txtFCF.Visible = False
            lblRTCF.Visible = False
            txtRTCF.Visible = False  ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor
            phAC.Visible = False
        End If
        ControlVisibility()
    End Sub
    Private Sub ControlVisibility()
        'btnSave.Enabled = mCompStatus.IsDirty
        btnPrint.Enabled = Not mCompStatus.IsNew
        lnkPrintLogBookEntry.Enabled = Not mCompStatus.IsNew 'Added By Prashant 7-May-20201 ALL07052021
        cmbAssemblyList.Enabled = (Not (mFrom = From.FromInstallAssembly)) And (mCompStatus.IsNew)

        If (Not mRemovedCompStatus Is Nothing AndAlso Not mRemovedCompStatus.ID.Equals(Guid.Empty)) And mFrom = From.NewInstall Then
            txtSerialNo.Enabled = False
        ElseIf (Not mRemovedCompStatus Is Nothing AndAlso mRemovedCompStatus.ID.Equals(Guid.Empty)) And mFrom = From.NewInstall Then
            txtSerialNo.Enabled = True
        ElseIf mFrom = From.EditInstall And (Not mCompStatus Is Nothing AndAlso mCompStatus.Sort = 1 AndAlso mCompStatus.IsRemoved = False) Then
            txtSerialNo.Enabled = True
        ElseIf mFrom = From.EditInstall And (Not mCompStatus Is Nothing AndAlso mCompStatus.Sort = 1 AndAlso mCompStatus.IsRemoved = True) Then
            txtSerialNo.Enabled = False
        ElseIf (Not mCompStatus Is Nothing AndAlso mFrom = From.EditInstall) And (Not mCompStatus Is Nothing AndAlso mCompStatus.Sort > 1) Then
            txtSerialNo.Enabled = False
        End If

        upnlHistoryCard.Visible = Not (txtSerialNo.Text = "") 'Added by Saylee on 12-Jan-2018 for ALL12012018


        'Added by Saylee on 7-Oct-2017 for Thrust
        If Not mAssemblyStatus Is Nothing Then
            If mAssemblyStatus.AssemblyTypeID = 2 And mCompStatus.CompStatusPeriods.Contains(3) And AppSettings("ShowThrustMonitoring") = True Then
                IsSLLExists = mCompStatus.IsSLLServiceExists
                Label1.Visible = True
                upnlIsThrustComp.Visible = True
                upnlIsThrustCompOuter.Update()
                If chkIsThrustComp.Checked Then
                    pnlThrustyComponentDet.Visible = True

                    mFirstThrustCompStatus = Session("mFirstThrustCompStatus")
                    mThrustTypeList = ThrustTypeList.GetThrustTypeList()
                    Session("mThrustTypeList") = mThrustTypeList

                    lblB22.InnerText = mThrustTypeList(0).Name
                    lblB24.InnerText = mThrustTypeList(1).Name
                    lblB26.InnerText = mThrustTypeList(2).Name

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
                upnlThrustyComponentDet.Update()
            Else
                pnlThrustyComponentDet.Visible = False
                Label1.Visible = False
                upnlIsThrustComp.Visible = False
                upnlIsThrustCompOuter.Update()
                upnlThrustyComponentDet.Update()
            End If

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

        End If
        '*******************************************************
        'Added by Saylee on 24-apr-2023
        Dim lblServiceTitle As Label

        lblServiceTitle = TbContInst.Tabs(1).FindControl("lblServiceListTitle")
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then

            ' tbPnlServiceList.HeaderTemplate = "MPD List"
            lblServiceTitle.Text = "Maintenance Event(s)"
            TbContInst.Tabs(2).Visible = False
        Else

            'tbPnlServiceList.HeaderTemplate = "Service List"
            lblServiceTitle.Text = "Service(s)"
            TbContInst.Tabs(2).Visible = Not (mCompStatus.IsNew)
        End If
        '**************************

        ControlVisibilityForAttachment() 'Added By Saylee On 27-Nov-2014 
    End Sub
    Private Sub SetPage()
        If IsDate(mCompStatus.InstalledOn) Then
            'Code Commented and newly Added on 28-05-2007 by Kalpesh Shah -------- 
            ''calInstalledOn.TitleText = CDate(mCompStatus.InstalledOn)
            ''calInstalledOn.DateToday = CDate(mCompStatus.InstalledOn)
            ''calInstalledOn.SelectedDate = CDate(mCompStatus.InstalledOn)
            ' ''calInstalledOn.Text = CDate(mCompStatus.InstalledOn)
            '---------------------------------------------------------------------
            'ElseIf IsDate(mAssemblyStatus.AsOnDate) Then
            '    'Code Commented and newly Added on 28-05-2007 by Kalpesh Shah -------- 
            '    ''calInstalledOn.TitleText = CDate(mAssemblyStatus.AsOnDate)
            '    ''calInstalledOn.DateToday = CDate(mAssemblyStatus.AsOnDate)
            '    ''calInstalledOn.SelectedDate = CDate(mAssemblyStatus.AsOnDate)
            '    calInstalledOn.Text = CDate(mAssemblyStatus.AsOnDate)
            '---------------------------------------------------------------------
        End If
        lblPartInfo.InnerText = "Part and Serial No. of the Component"
        lbInstallationInfo.InnerText = "Installation Information of the Component"


        If Not mCompStatus.IsNew Then
            lblTitle.Text = "Installation Information of the Component [ Part:" & mCompStatus.PartName & " Serial No. : " & mCompStatus.SerialNo & " ]"
        Else
            lblTitle.Text = "Installation Information of the Component [ New ]"
        End If
    End Sub
    Private Sub CopyFromClone(ByVal cln As CompStatus)
        REM: to recover from object when there is change in data or log 
        mCompStatus.Comp.PartID = cln.Comp.PartID
        mCompStatus.Comp.SerialNo = cln.Comp.SerialNo
        mCompStatus.Position = cln.Position
        mCompStatus.InstallationWONo = cln.InstallationWONo
        mCompStatus.InstallationRemark = cln.InstallationRemark
        mCompStatus.InstalledOn = cln.InstalledOn
        mCompStatus.AssemblyID = cln.AssemblyID
        mCompStatus.ATAID = cln.ATAID

        mCompStatus.InstDoneByID = cln.InstDoneByID
        mCompStatus.InstLicenseNo = cln.InstLicenseNo
        mCompStatus.InstPlace = cln.InstPlace
        mCompStatus.ModelID = mAssemblyStatus.Assembly.ModelID
        'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        If chkFanBladeMonitoring.Checked Then
            mCompStatus.IsFanBladeDistribution = chkFanBladeMonitoring.Checked
            mCompStatus.FanBladePosition = Val(txtFanBladePosition.Text)
            mCompStatus.MomentWeight = CDec(txtMomentWeight.Text)
            mCompStatus.BalanceScrew = Val(txtBalanceScrew.Text)
        End If
        'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        'MLNo
        'For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In cln.MaintenanceDoneByEmployees
        '    mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
        'Next

        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In cln.MaintenanceDoneByEmployees
            If Session("From") = 1 Then 'New Record
                mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            ElseIf Session("From") = 2 Then 'Edit Record
                If Not mCompStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                End If
            End If
        Next
        'End
        Session("mCompStatus") = mCompStatus
    End Sub
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
        ''    If Not IsValid Then Exit Function
        Dim mClnInsComp As CompStatus = CType(mCompStatus.Clone, CompStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 8th-Oct-2009
        If mCompStatus.IsValid = True Then
            Try
                If mCompStatus.CompStatusPeriods.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Installation Component Status can not be saved without periods", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfInstallCompBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Installation Component Status can not be saved without periods", MsgBoxStyle.OkOnly, "")
                    Return False
                End If
                'Added By Shweta On 07-Aug-2013 For ALL01082013
                If Not mCompStatus.InstDoneByID.Equals(Guid.Empty) AndAlso Not mCompStatus.InstalledOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompStatus.InstDoneByID.ToString, mCompStatus.InstalledOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ' ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message))
                        MSGBoxCtrl.Show(title, message, "", MsgBoxStyle.OkOnly, "")
                        Return False
                    End If
                End If
                'End

                'Added by Saylee on 9-Oct-2017 for Thrust Monitoring
                If mCompStatus.CompStatusPeriods.Contains(3) And mCompStatus.IsThrustMonitoringComp Then
                    If SumOfThrust(mCompStatus) = False Then
                        MSGBoxCtrl.Show("Installation Alert!", "Summation of Thrust Monitoring values are mismatching with Current values. ", "", MsgBoxStyle.OkOnly, "")
                        Return False
                    End If
                End If
                Dim ThrustLabels As String = lblB22.InnerText + " , " + lblB24.InnerText + " & " + lblB26.InnerText

                If mCompStatus.IsThrustMonitoringComp And (chkB22IsCurrent.Checked = False And chkB24IsCurrent.Checked = False And chkB26IsCurrent.Checked = False) Then
                    MSGBoxCtrl.Show("Component Installation Alert!", "Thrust Monitoring (either Monitor with " + ThrustLabels + ") required.", "", MsgBoxStyle.OkOnly, "")
                    Return False
                End If
                '**********************************************************
                If mCompStatus.IsThrustMonitoringComp And (CDec(txtB22LifeLimit.Text) = 0 Or CDec(txtB24LifeLimit.Text) = 0 Or CDec(txtB24LifeLimit.Text) = 0) Then
                    MSGBoxCtrl.Show("Component Installation Alert!", "Please Enter Life Limit for all " + ThrustLabels, "", MsgBoxStyle.OkOnly, "")
                    Return False
                End If

                mCompStatus.ApplyEdit()
                mCompStatus = CType(mCompStatus.Save, CompStatus)
                SaveMachineMaintenance()  'Added by Saylee on 8th-Oct-2009
                SaveAttachment() 'Added By Saylee On 27-Nov-2014 

                If (Session("InstallSelected") <> 1) Then Session("From") = 2
                'REM: If we are coming by new component from Remove Comp Status then modelID and MachineID is not set
                ''   After Saving the record we have to set it to mModelID and mMachineID
                'If mCompStatus.ModelID.Equals(Guid.Empty) Then  'And Session("NewInstall") = 2
                '    mAssemblyStatus.Assembly.ModelID = mAssemblyList(mCompStatus.AssemblyID).ModelID
                'End If
                ''mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbAssemblyList.SelectedValue.Tostring))
                ''******pls check
                'If Not cmbAssemblyList.SelectedIndex < 0 And Not IsNothing("mCompStatus") Then
                '    If Not mPeriodListForCompStatus(0).AssemblyStatusID.Equals(Guid.Empty) Then     'And Session("NewInstall")= 1   'this is in case if coming from Removed CompList
                '        REM: AssemblyStatusID is retreived from mPeriodListForCompStatus
                '        Dim tmpAssemblyStatus As AssemblyStatus
                '        tmpAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mPeriodListForCompStatus(0).AssemblyStatusID)
                '        mAssemblyStatus.MachineID = tmpAssemblyStatus.MachineID
                '    End If
                'End If
                ''**********
                'Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompStatus") = mCompStatus
                mCompInstallInfo = "ATAChapter -> " + mCompStatus.ATAChapter + " Part -> " + mCompStatus.PartNameSerialNo + " -> " + " InstallOn -> " + mCompStatus.InstalledOn.ToString   'Code Added Jan,30,2007

                'Commented By Utkarsh On 26-Jul-2011 For All19072011
                '   MarkLog(Util.Action.Save, "CompInstall", mCompInstallInfo, Util.ErrorType.NoError, mCompStatus.ID) 'Code Added Jan,30,2007
                'End
                Return True
            Catch ex As SqlException
                Session("mClnInsComp") = mClnInsComp
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfInstallCompBA.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfInstallCompBA.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfInstallCompBA.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfInstallCompBA.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
                ElseIf ex.Number = 50000 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                    'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
                End If
                Return False
            Finally
                mClnInsComp = Nothing
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                If mSpareAssemblyComponent = 0 Then    'If Condition Added by Shital on 23-Dec-2020
                    MaintDetail = "Reg No. : " & Machine.GetMachine(mAssemblyStatus.MachineID).RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo
                    MarkLog(Util.Action.Save, "Component Installation", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
                    MarkLog(Util.Action.Save, "Machine Maintenance", User.Identity.Name & " saved maintenance activity " & IIf(mMachineMaintenance.LogID.Equals(Guid.Empty), "", " with log " & Machine.GetMachine(mAssemblyStatus.MachineID).RegNo & mMachineMaintenance.LogNo), Util.ErrorType.NoError, mMachineMaintenance.LogID, EventLogID)
                Else
                    MaintDetail = " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo
                    MarkLog(Util.Action.Save, "Component Installation", MaintDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
                    MarkLog(Util.Action.Save, "Machine Maintenance", User.Identity.Name & " saved maintenance activity ", Util.ErrorType.NoError, mMachineMaintenance.LogID, EventLogID)
                End If
                'End

            End Try
        Else
            Return False
        End If
    End Function
    Private Sub NewRecord(ByVal LogID As Guid, ByVal mCurrentDate As String, Optional ConsiderAssemblyInstValue As Boolean = False)
        Dim mAssemblyID As Guid
        If cmbAssemblyList.SelectedValue = "" Then
            mAssemblyID = Guid.Empty
        Else
            mAssemblyID = New Guid(cmbAssemblyList.SelectedValue.ToString)
        End If

        'mAssemblyStatus = Session("mAssemblyStatus")
        'code added By Deven On 24/04/2008---------------------------------
        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mPeriodListForCompStatus(0).AssemblyStatusID)
        Session("mAssemblyStatus") = mAssemblyStatus
        '------------------------------------------------------------------

        If Not IsNothing(mRemovedCompStatus) Then
            Dim clnRemovedCompStatus As CompStatus = mRemovedCompStatus.Clone
            If clnRemovedCompStatus.IsSpareComp Then 'If condition added by vikrant on 17-Nov-2020 for ALL27072020
                mRemovedCompStatus = CompStatus.GetSpareCompStatus(clnRemovedCompStatus.ID, True)
                'End
            Else
                mRemovedCompStatus = CompStatus.GetCompStatus(clnRemovedCompStatus.ID, mAssemblyStatus.ID, mCurrentDate)
            End If

            mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, clnRemovedCompStatus.AssemblyID, mAssemblyStatus.ID, mCurrentDate, True, clnRemovedCompStatus.ID.ToString, LogID.ToString, ConsiderAssemblyInstValue)
            clnRemovedCompStatus = Nothing
            Session("mRemovedCompStatus") = mRemovedCompStatus
        Else
            If mFrom = From.FromInstallAssembly Then
                mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mCurrentDate, False)
            Else
                mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mAssemblyID, mAssemblyStatus.ID, mCurrentDate, False, Guid.Empty.ToString, LogID.ToString)
            End If
        End If

        mCompStatus.ModelID = mAssemblyStatus.Assembly.ModelID
        ModelID = mAssemblyStatus.Assembly.ModelID
        Session("ModelID") = ModelID
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub SetLog()
        ' Code Added by DEVEN ***********************************************23-08-2007******************************************************************
        If CType(Session("FromLog"), Boolean) = True Then
            Dim mAssemblyID As Guid
            If cmbAssemblyList.SelectedValue = "" Then
                mAssemblyID = Guid.Empty
            Else
                mAssemblyID = New Guid(cmbAssemblyList.SelectedValue.ToString)
            End If
            'mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mAssemblyID, mAssemblyStatus.ID, calInstalledOn.Text, False, , LogID.ToString)
            dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
            dgInstallationValue.DataBind()
            'DataFieldBind()


            'Added by Saylee on 8th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            Session("mLog") = mLog
            '===========================================
            'Else
            'If Not IsPostBack And CType(Session("sender"), String) = "" Then Session.Remove("mLog")
        End If

        If CType(Session("FromLog"), Boolean) = True Then
            Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
            Dim LogDate = CType(Session("LogDate"), String)
            If mFrom = From.NewInstall Or mFrom = From.FromInstallAssembly Then
                If Not IsNothing(mRemovedCompStatus) Then
                    Dim clnCompStatus As CompStatus = mCompStatus.Clone
                    NewRecord(LogId, LogDate, CType(Session("ConsiderAssemblyInstValue"), Boolean))
                    CopyFromClone(clnCompStatus)
                Else
                    Dim clnCompStatus As CompStatus = mCompStatus.Clone
                    Dim tmp As CompStatusPeriods = mCompStatus.CompStatusPeriods
                    NewRecord(LogId, LogDate)
                    mCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, LogDate, False, LogDate, LogId.ToString, CType(Session("ConsiderAssemblyInstValue"), Boolean))
                    CopyFromClone(clnCompStatus)
                    clnCompStatus = Nothing
                    Session.Remove("ConsiderAssemblyInstValue")
                End If

                'Added by Saylee on 8th-Oct-2009
                Dim mLog As Log
                mLog = Log.GetLog(New Guid(LogId.ToString))
                Session("mLog") = mLog
                '===================================
            Else
                Dim clnCompStatus As CompStatus = mCompStatus.Clone
                mCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mAssemblyStatus.ID, LogDate, LogId.ToString, CType(Session("ConsiderAssemblyInstValue"), Boolean))
                CopyFromClone(clnCompStatus)
                clnCompStatus = Nothing
                Session.Remove("ConsiderAssemblyInstValue")
            End If
        End If
        Session.Remove("FromLog")
    End Sub
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 8th-Oct-2009
        If mFrom = From.NewInstall And (Not mMachineMaintenanceList.Contains(mCompStatus.ID, 3, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 3, calInstalledOn.Text, mCompStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mCompStatus = CType(Session("mCompStatus"), CompStatus)
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompStatus.ID, 3)
            Session("mMachineMaintenance") = mMachineMaintenance
        End If


        With mMachineMaintenance
            .MachineID = mAssemblyStatus.MachineID
            .MaintenanceActivityTypeID = 3
            .MaintenanceID = mCompStatus.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatus.ID

            .Date = calInstalledOn.Text

            Dim mLog As Log = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                'Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(calInstalledOn.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If
        End With

        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub SaveMachineMaintenance()
        'Added by Saylee on 8th-Oct-2009
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
        ''End If
    End Sub
    Private Sub SetPartNoDescription()
        'If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
        '    PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
        '    Description = Microsoft.VisualBasic.Strings.Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        'Else
        '    PartNo = Trim(txtPartDescription.Text)
        '    Description = Trim(txtPartDescription.Text)
        'End If
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

            PartNo = txtPartDescription.Text.Trim

            If mPartList.Contains(PartNo) Then
                Description = mPartList(PartNo).Description
            Else
                Description = ""
            End If
        Else
            PartNo = IIf(mPartList(New Guid(cmbPartNo.SelectedValue.ToString)).Name = "(SELECT)", "", mPartList(New Guid(cmbPartNo.SelectedValue.ToString)).Name)

            If mPartList.Contains(PartNo) Then
                Description = mPartList(PartNo).Description
            Else
                Description = ""
            End If
        End If
    End Sub

    'Added by Saylee on 19-Mar-2013 for ALL14032013-1
    Public Function CheckPeriodsForRemovedCompStatus(ByVal RemovedCompStatus As CompStatus) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = True
        Dim mAssemblyStatusList As AssemblyStatusList

        'Commented and Added by Shital on 23-Dec-2020
        ' mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(calInstalledOn.Text, mAssemblyList(New Guid(cmbAssemblyList.SelectedValue.ToString)).MachineID.ToString, , , , , , , , , , True, , , cmbAssemblyList.SelectedValue.ToString, , , , , , , , , , , , , , , , ).Item(0), MachineInfo).AssemblyStatusList()

        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(calInstalledOn.Text, mAssemblyList(New Guid(cmbAssemblyList.SelectedValue.ToString)).MachineID.ToString, , , , , , , , , , True, , , cmbAssemblyList.SelectedValue.ToString, , , ,
                                                                               , , , , , , , , , , , , , , , IsForSpareAssembly:=True).Item(0), MachineInfo).AssemblyStatusList()



        '   mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(calInstalledOn.Text, mAssemblyStatus.MachineID.ToString, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
        If mAssemblyStatusList.Count > 0 Then 'Added by Saylee on 8-May-2013 for ALL08052013-2 
            If mAssemblyStatusList(0).AssemblyID.Equals(New Guid(cmbAssemblyList.SelectedValue.ToString)) Then
                Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusList(0).ID)
                While i <= RemovedCompStatus.CompStatusPeriods.Count - 1
                    If tmpAssemblyStatus.AssemblyStatusPeriods.Contains(RemovedCompStatus.CompStatusPeriods(i).PeriodID) Then
                        tmpIsPeriodExists = True
                    Else
                        tmpIsPeriodExists = False
                        Exit While
                    End If
                    i = i + 1
                    Session("AssemblyNotExists") = "" 'Added by Saylee on 8-May-2013 for ALL08052013-2 
                End While
            End If
        Else
            Session("AssemblyNotExists") = "AssemblyNotExists" 'Added by Saylee on 8-May-2013 for ALL08052013-2
        End If

        Return tmpIsPeriodExists
    End Function
    ''Added By Saylee On 27-Nov-2014 
    Private Sub NewRecordAttachment()
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1)
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ControlVisibilityForAttachment()

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then 'change from  to current condition
                ImageButton1.Visible = True
                btnDelAttach.Enabled = True
            Else
                ImageButton1.Visible = False
            End If
        Else
            ImageButton1.Visible = False
        End If

    End Sub
    Private Sub GetAttachment()
        If mCompStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 1) 'Sort = 1 - Installation
            Session("mFileAttach") = mFileAttach
        End If

        'If mFileAttach Is Nothing Then
        '    NewRecordAttachment()
        'End If
    End Sub
    Private Sub SaveAttachment() '

        If mFileAttach Is Nothing And mCompStatus.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 1)
            Session("mFileAttach") = mFileAttach
        End If

        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mCompStatus.ID
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                    mFileAttach = Nothing
                    Session("mFileAttach") = mFileAttach
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), False)
                End Try
            Else
                If (Not mCompStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCompStatus.ID, Sort:=1)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mCompStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 1)
            Session("mFileAttach") = mFileAttach
        End If

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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    'End
    'Added by Saylee on 24-Feb-2015
    Public Function CheckForInstValue(ByVal CompStatus As CompStatus) As Boolean
        Dim i As Integer = 0
        For i = 0 To CompStatus.CompStatusPeriods.Count - 1
            If CompStatus.CompStatusPeriods(i).CheckForInstValue = True Then
                Return True
            End If
        Next
        Return False
    End Function
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
        mPartList = PartList.GetPartList(, , "(SELECT)")
        Session("mPartList") = mPartList
        cmbPartNo.DataSource = mPartList

        'Commented ans Added by Saylee on 4-Sep-2009
        '' mAssemblyList = AssemblyList.GetAssemblyList(0, , , "<SELECT>") 


        If mSpareAssemblyComponent = 0 Then  'If Condition Added by Shital on 23-Dec-2020
            mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, , mCompStatus.InstalledOn.ToString, "(SELECT)", True, , , True)
        Else
            mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, , mCompStatus.InstalledOn.ToString, "(SELECT)", True, , , True, IsForSpareAssembly:=True) '  
        End If

        cmbAssemblyList.DataSource = mAssemblyList
        'cmbAssemblyList.DataSource = Flypal.MachineReadOnly.AssemblyStatusList.GetAssemblyStatusList(Guid.Empty, , , , , , , , , , , , , , , True)
        Session("mAssemblyList") = mAssemblyList

        If Not mCompStatus.AssemblyID.Equals(Guid.Empty) And mCompStatus.IsNew Then
            mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mCompStatus.AssemblyID, "")
            Session("mPeriodListForCompStatus") = mPeriodListForCompStatus

            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mPeriodListForCompStatus(0).AssemblyStatusID)
            Session("mAssemblyStatus") = mAssemblyStatus
            Session("AircraftId") = mAssemblyStatus.MachineID.ToString
        End If
        dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
        'Added on 28-05-2007 by Kalpesh Shah
        calInstalledOn.Text = mCompStatus.InstalledOnFormatted

        'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        '=======================================================
        'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        mManufacturerList = ManufacturerList.GetManufacturerList(, "<SELECT>")
        cmbManufacturerList.DataSource = mManufacturerList
        Session("mManufacturerList") = mManufacturerList
        'End
        'Added by Saylee on 7-Apr-2015
        If Not mAssemblyList.Contains(mCompStatus.AssemblyID) Then
            mCompStatus.AssemblyID = Guid.Empty
        End If

        mInstallationStatusList = InstallationStatusList.GetInstallationStatusList()
        cmbInstallationStatus.DataSource = mInstallationStatusList
        Session("mInstallationStatusList") = mInstallationStatusList

        BindLicenceNo() 'MLNo

        'Added by Saylee on 7-Oct-2017 for Thrust
        If Not mAssemblyStatus Is Nothing Then
            If mAssemblyStatus.AssemblyTypeID = 2 And mCompStatus.CompStatusPeriods.Contains(3) Then
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
        End If
        '******************************************************


        DataBind()

        If cmbATAChapter.Items.Contains(New System.Web.UI.WebControls.ListItem(mCompStatus.ATAChapter, mCompStatus.ATAID.ToString)) Then
            cmbATAChapter.SelectedValue = mCompStatus.ATAID.ToString
        Else
            cmbATAChapter.SelectedValue = Guid.Empty.ToString
        End If
        Session("PartNo") = mCompStatus.PartName
        Session("Description") = mCompStatus.Description

        If cmbPartNo.Items.Contains(New System.Web.UI.WebControls.ListItem(mCompStatus.Comp.PartName, mCompStatus.Comp.PartID.ToString)) Then
            cmbPartNo.SelectedValue = mCompStatus.Comp.PartID.ToString
        Else
            cmbPartNo.SelectedValue = Guid.Empty.ToString
        End If

        If mFileAttach Is Nothing Then
            If mCompStatus.IsAttachmentAdded = True Then
                mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 1) 'Sort = 1 - Installation
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1)
            End If
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub DataBindGrid()
        dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
        dgInstallationValue.DataBind()
        Session("mCompStatus") = mCompStatus
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "cmbATAChapter" Then
            'Commented by Saylee on 8th-Jan-2008 as its displaying brokenrule twice
            'If cmbATAChapter.SelectedIndex = 0 Then
            '    CustValid.ErrorMessage = "Please select ATA Chapter from the list"
            '    e.IsValid = False
            'Else
            '    e.IsValid = True
            'End If
        ElseIf (CustValid.ControlToValidate = "cmbPartNo") Then
            If ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA")) Then
                If txtPartDescription.Text = "" Then
                    CustValid.ErrorMessage = "Please Enter Part No."
                    e.IsValid = False
                ElseIf txtPartDescription.Text <> "" And Not mPartList.Contains(txtPartDescription.Text) Then
                    CustValid.ErrorMessage = "Please select proper Part No."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                If cmbPartNo.SelectedIndex = 0 Then
                    CustValid.ErrorMessage = "Please select the Part No. from the list."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        ElseIf CustValid.ControlToValidate = "txtSerialNo" Then
            If txtSerialNo.Text = "" Then
                CustValid.ErrorMessage = "Serial No required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "cmbAssemblyList" Then
            'Commented by Saylee on 8th-Jan-2008 as its displaying brokenrule twice
            If cmbAssemblyList.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Please select the Assembly from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 500 Then
                CustValid.ErrorMessage = "Max length of Note should not be greater than 500 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Prashant On 121-Jun-2012 FOR ALL08062012
        ElseIf CustValid.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                CustValid.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End

        End If
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator = CType(s, CustomValidator)
        REM:this is for grid validation
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtInstallCompValue As TextBox
        If Not mCompStatus.IsValid Then
            For i As Integer = 0 To mCompStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgInstallationValue.Rows.Count - 1)
            txtInstallCompValue = CType(Me.dgInstallationValue.Rows(i).FindControl("txtCompInstallationValue"), TextBox)
            If Not mCompStatus.CompStatusPeriods.Item(i).IsValid Then
                For x As Integer = 0 To mCompStatus.CompStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompStatus.CompStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        If Not mCompStatus.IsValid Then
            Dim x As Integer
            For x = 0 To mCompStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompStatus.GetBrokenRulesCollection(x).Description + "<BR>"
            Next
        End If

        For i As Integer = 0 To CShort(dgInstallationValue.Rows.Count - 1)
            If Not mCompStatus.CompStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompStatus.CompStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompStatus.CompStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvNote.ErrorMessage = str
            cvNote.IsValid = False
            Return False
        Else
            Return True
        End If
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
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM: put here the code to initialize the page
        GetSession()
        GetSessionService()
        GetSessionInsp()
        GetSessionMod()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 26-Jul-2011 For All19072011
        addAttributes()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            '===============================================================
            'Commented and added by Saylee on 8th-Jan-2008 for bug-IIC1 (Maintenance)
            'If cmbATAChapter.Enabled = True Then
            '    SetFocus(cmbATAChapter)
            'End If
            If (Session("From") = 1) Then
                If (Session("InstallSelected") = 1) Then
                    cmbATAChapter.Enabled = False
                    txtPartDescription.Enabled = False
                    chkByModel.Enabled = False
                    btnPartNo.Enabled = False
                    ImgBtnATAChapter.Enabled = False

                    If btnSelectLog.Enabled = True Then
                        setFocus(btnSelectLog)
                    End If

                    Session("mInstallSelected") = mInstallSelected

                    ' ''Session.Remove("InstallSelected")
                Else
                    If cmbATAChapter.Enabled = True Then
                        setFocus(cmbATAChapter)
                    End If
                End If
            ElseIf (Session("From") = 2) Then
                If btnSelectLog.Enabled = True Then
                    setFocus(btnSelectLog)
                End If
            End If
            '===============================================================
            SetLog()
            AddSelectedPeroids()
            DataFieldBind()
            'DataFieldBindInsp()
            'DataFieldBindMod()
            'DataFieldBindService()
            '---------- 28-Apr-2009
            If Session("IsAdded") = "False" Then

                SetPeroids()
                For i As Integer = 0 To mSelectPeriods.Count - 1
                    mSelectPeriods(i).IsSelected = True
                Next
                AddSelectedPeroids()
                Call cmbAssemblyList_SelectedIndexChanged(Nothing, Nothing)
                dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                dgInstallationValue.DataBind()
                Session("IsAdded") = "True"
            End If
            '----------
            GetAttachment()  'Added By Saylee On 27-Nov-2014 
            SetPage()
            'SetPageInsp()
            'SetPageMod()
            'SetPageService()
            ControlVisibility()
            'ControlVisibilityInsp()
            'ControlVisibilityMod()
            'ControlVisibilityService()
            ControlVisiblity1() 'Added By Prashant 26-Aug-2010
            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                txtPartDescription.Visible = True
                cmbPartNo.Visible = False
            Else
                cmbPartNo.Visible = True
                txtPartDescription.Visible = False
            End If
            If CType(Session("TabIndex"), Integer) > 0 Then
                If Not Session("TabIndex") Is Nothing Then TbContInst.ActiveTabIndex = CType(Session("TabIndex"), Integer) : Session.Remove("TabIndex")
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
            ''''Else

            ''''    'tbPnlServiceList.HeaderTemplate = "Service List"
            ''''    lblServiceTitle.Text = "Service(s)"
            ''''    TbContInst.Tabs(2).Visible = Not (mCompStatus.IsNew)
            ''''End If
            '''''**************************
        End If
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 1)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompStatus.ID, Sort:=1)
        End If
        Session("mFileAttach") = mFileAttach
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectFileWindow", "OpenSelectFileWindow()", True)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
        If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
            SetObject()
            SetSession()

            'Changed By Utkarsh On 26-Jul-2011 For All19072011
            If mSpareAssemblyComponent = 0 Then
                MaintDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo
            Else
                MaintDetail = " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo
            End If
            MarkLog(Util.Action.Save, "Component Installation", User.Identity.Name & " is not Authorized User to save " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            If Not CustomValidate2() Then Exit Sub

            'Added by Saylee on 18-Jul-2018 for ALL18072018-1 : Locking backdated installations on Comp and Assembly
            If mFrom = From.EditInstall And (mCompStatus.IsRemoved = True) Then
                MSGBoxCtrl.Show("Installation Alert!", "Component detail(s) cannot be modified as it is removed." & " Revert the Removal and then modify.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            '*******************************************************************

            'Added by Saylee on 19-Mar-2013 for ALL14032013-1
            If CheckPeriodsForRemovedCompStatus(mCompStatus) = False Then
                'Str = Str() + "Periods for selected " & mAssemblyStatus.AssemblyTypeName & " are mismatching with selected Installed On " & cmbMachineList.SelectedItem.Text & " Aircraft.Can not be installed."
                'Dim msg1 As New SIMsgBox(Page, "Component Status Installation Alert!", "Periods for selected " & mCompStatus.PartNameSerialNo & " are mismatching with selected Installed On Assembly" & mAssemblyStatus.AssemblyTypeName & " .Can not be installed.", "", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                'Session("sender") = "Delete"
                MSGBoxCtrl.Show("Installation Alert!", "Periods for selected " & mCompStatus.PartNameSerialNo & " are mismatching with selected Installed On Assembly" & mAssemblyStatus.AssemblyTypeName & " .Can not be installed.", "", MsgBoxStyle.OkOnly, "Delete")
                'msg1.Show()
                Exit Sub
            End If
            '***********************************

            If Session("AssemblyNotExists") = "AssemblyNotExists" Then  'Added by Saylee on 08-May-2013 for ALL08052013-2
                Session("AssemblyNotExists") = ""
                'Dim msg1 As New SIMsgBox(Page, "Component Status Installation Alert!", "Assembly does not exists on selected Installed On date. Can not be installed.", "", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                'Session("sender") = "Delete"
                'msg1.Show()
                Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus) 'Added by Saylee on 8-May-2013 for ALL08052013-2 
                Session("tmpclnCompStatus") = clnCompStatus 'Added by Saylee on 8-May-2013 for ALL08052013-2
                CopyFromClone(CType(Session("tmpclnCompStatus"), CompStatus))
                SetGridObject()
                MSGBoxCtrl.Show("Installation Alert!", "Assembly does not exists on selected Installed On date. Can not be installed.", "", MsgBoxStyle.OkOnly, "Delete")
                Exit Sub
            End If

            'Added by Saylee on 24-Feb-2015 to check whether inst value is less than ite removal value
            If mFrom = From.NewInstall And Session("InstallSelected") = 1 And CheckForInstValue(mCompStatus) Then

                'Dim msg1 As New SIMsgBox(Page, "Component Status Installation Alert!", " You are about to save Component Installation value less than its Removal Value. ", "Do you want to continue?", MsgBoxStyle.YesNo)
                'msg1.ReplacePage = "wfInstallCompBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                'DataFieldBind()
                'Session("sender") = "Save"
                'msg1.Show()
                MSGBoxCtrl.Show("Installation Alert!", " You are about to save Component Installation value less than its Removal Value. ", "Do you want to continue?", MsgBoxStyle.YesNo, "Save")
                Exit Sub
            End If

            If Save() = True Then
                Session("FromLog") = False
                DataFieldBind()
                GetAttachment()
                SetPage()

                ControlVisibility()
                upnlTabs.Update()
                upnlPartOnfo.Update()
                upnlInstInfo.Update()
                upnlTitle.Update()

                'MLNo
                Session.Remove("mMaintenanceDoneByEmployees")
                Session.Remove("UserNameForLicenceList")
                'End
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                '' Response.Redirect("wfInstallCompBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
            Else
                If Not CustomValidate2() Then upnlValidationSummary.Update()
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Protected Sub txtCompInstallationValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        For i As Integer = 0 To mCompStatus.CompStatusPeriods.Count - 1
            Dim txtCompInstVal As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtCompInstallationValue"), TextBox)
            If mCompStatus.CompStatusPeriods.Item(i).PeriodID = 2 Then
                If Period.IsDate(txtCompInstVal.Text) Then
                    mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = txtCompInstVal.Text.Trim
                Else
                    mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = ""
                End If
            Else
                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValue = txtCompInstVal.Text.Trim
            End If
        Next i
        DataBindGrid()
        upnlInstallationValue.Update()
    End Sub
    Private Sub dgInstallationValue_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstallationValue.RowCommand

        Select Case e.CommandName
            'Case "CompInstallationValue"
            '    Dim Index As Integer = CInt(e.CommandArgument) + dgInstallationValue.PageSize * dgInstallationValue.PageIndex

            '    For i As Integer = 0 To mCompStatus.CompStatusPeriods.Count - 1
            '        Dim txtCompInstVal As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtCompInstallationValue"), TextBox)
            '        If mCompStatus.CompStatusPeriods.Item(i).PeriodID = 2 Then
            '            If Period.IsDate(txtCompInstVal.Text) Then
            '                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = txtCompInstVal.Text.Trim
            '            Else
            '                mCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = ""
            '            End If
            '        Else
            '            mCompStatus.CompStatusPeriods.Item(i).CompInstallationValue = txtCompInstVal.Text.Trim
            '        End If
            '    Next i
            '    DataBindGrid()
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgInstallationValue.PageSize * dgInstallationValue.PageIndex

                ' If (Not User.IsInRole("ComponentInstallationEdit")) Then
                If (Not User.IsInRole("AssemblyInstallationDelete")) Then
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")

                    Exit Sub
                End If

                REM: If monitoring entry is present for that particualr period then that period can not be deleted
                ' If mCompStatus.CompStatusPeriods.Item(index).HasMonitor = True Then
                If mCompStatus.CompStatusPeriods.Item(Index).HasMonitorCount(mCompStatus.ID, mCompStatus.CompStatusPeriods.Item(Index).PeriodID) = True Or mCompStatus.CompStatusPeriods.Item(Index).IsPeriodMonitored(mCompStatus.CompID, mCompStatus.CompStatusPeriods.Item(Index).PeriodID) = True Then
                    'Dim msg1 As New SIMsgBox(Page, "Removal Alert!", "Selected Component Period cannot be removed as monitor entry exist", "", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfInstallCompBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()

                    MSGBoxCtrl.Show("Removal Alert!", "Selected Component Period cannot be removed as monitor entry exist", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    SetGridObject()
                    mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods.Item(Index))
                    'Commneted By Prashant 27-July-2009 because it was refresing cmbAssemblyList Combobox
                    'Response.Redirect("wfInstallCompBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    '---------------------------------
                    'Added By Prashant 27-July-2009
                    DataBindGrid()
                    '---------------------------------
                    ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                    upnlPartOnfo.Update()
                    If (Not mCompStatus.CompStatusPeriods.Contains(9) And Not mCompStatus.CompStatusPeriods.Contains(10) And Not mCompStatus.CompStatusPeriods.Contains(16)) Then
                        mCompStatus.Comp.ACF = 0D
                        mCompStatus.Comp.ECF = 0D
                        mCompStatus.Comp.FCF = 0D
                        mCompStatus.Comp.RTCF = 0D ''Added by Saylee on 31-Oct-2022 for Rapid Take Off Cycle Factor
                        txtACF.DataBind()
                        txtECF.DataBind()
                        txtFCF.DataBind()
                        txtRTCF.DataBind()
                    End If
                End If
        End Select
    End Sub
    Private Sub btnAddPeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriod.Click
        If cmbAssemblyList.SelectedIndex = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Select an Assembly from the list.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            SetObject()
            SetGridObject()
            SetPeroids()
            ' Response.Redirect("wfSelectPeriod.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage2=wfInstallCompBA.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
        End If
    End Sub
    Private Sub cmbAssemblyList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssemblyList.SelectedIndexChanged
        SetPartNoDescription()
        SetAssemblyPeriod()
        upnlPartOnfo.Update()
        upnlInstInfo.Update()
    End Sub
    Private Sub calInstalledOn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calInstalledOn.TextChanged
        ' If IsPostBack Then
        Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus) 'Added by Saylee on 8-May-2013 for ALL08052013-2 
        Session("tmpclnCompStatus") = clnCompStatus 'Added by Saylee on 8-May-2013 for ALL08052013-2
        SetPartNoDescription()
        SetObject()
        SetGridObject()
        SetAssemblyPeriod()
        Session.Remove("mLog")
        DataGridBind()
        upnlInstallationValue.Update()
        ' End If
    End Sub

    Private Sub SelectLog(sender As Object, e As EventArgs) Handles btnSelectLog.Click

        If IsValid Then

            Session.Remove("mLogList") 'Added Code 'The previous value coming from loglist in session will be cleared and fwded
            SetPartNoDescription()
            SetObject()
            SetGridObject()
            Session.Remove("FromLog")

            If mPeriodListForCompStatus Is Nothing Then

                mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mCompStatus.AssemblyID, "")
                Session("mPeriodListForCompStatus") = mPeriodListForCompStatus

            End If

            If Not mPeriodListForCompStatus(0).AssemblyStatusID.Equals(Guid.Empty) Then

                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mPeriodListForCompStatus(0).AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus

                If mCompStatus.InstalledOn.Equals(mAssemblyStatus.InstalledOn) Then

                    Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation =
                        FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)

                    Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation

                End If

                Session("ForCompInstall") = True
                Session.Remove("ConsiderAssemblyInstValue")
                Session.Remove("FromLog")
                Session.Remove("mLogList")
                Session("mFromType") = 4
                Session("mMachineId") = mAssemblyStatus.MachineID.ToString
                Session("mAssemblyStatusId") = mAssemblyStatus.ID.ToString
                Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
                Session("mDoneOn") = CStr(IIf(calInstalledOn.Text = "",
                                              Today.Date.ToShortDateString,
                                              calInstalledOn.Text))

                MarkLog(Action.View,
                        "Select Log From Comp Installation",
                        User.Identity.Name,
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID)

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenSelectLogWindow",
                                                    "OpenSelectLogWindow()",
                                                    True)

            End If

        Else

            upnlValidationSummary.Update()
            Exit Sub

        End If

    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not mCompStatus.IsNew Then
            Dim mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
            'mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.InstalledOn.ToString, mCompStatus.CompID, False)
            mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString) 'ture is for mCompStatus.IsMaster 

            Dim mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
            'mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.InstalledOn.ToString, mCompStatus.CompID, False)
            mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)

            If mCompMonitorServiceStatusList.Count <= 0 And mCompMonitorInspStatusList.Count <= 0 Then
                MSGBoxCtrl.Show("Monitoring Service / Inspection not added", "Monitoring Service / Inspection is not Added in this Installed Component.<BR><BR> Do you want to Configure them?", "", MsgBoxStyle.YesNoCancel, "ReqServ")
                Exit Sub
            End If
        End If
        'Changed By Utkarsh On 26-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Component Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        RemoveSession()
        Session.Remove("FromLog")
        Session.Remove("mLog")
        Session.Remove("ConsiderAssemblyInstValue")
        Session.Remove("TabIndex")

        If Request.QueryString("GChildPage2") = "wfInstallAssembly_Ajax.aspx" And Session("IsOpenedFromAssembly") = "True" Then
            Session.Remove("IsOpenedFromAssembly")
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        Else
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub ImgBtnATAChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgBtnATAChapter.Click
        SetPartNoDescription()
        SetObject()
        SetGridObject()
        'SetAssemblyPeriod()
        Session.Remove("FromLog")
        ' Response.Redirect("wfATA_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage3=wfInstallComp_AJAX.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenATAWindow", "OpenATAWindow()", True)
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "<SELECT>")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
    Private Sub btnPartNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartNo.Click
        SetPartNoDescription()
        SetObject()
        SetGridObject()
        'SetAssemblyPeriod()
        Session.Remove("FromLog")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPartWindow", "OpenPartWindow()", True)
        'Response.Redirect("wfPart_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfInstallCompBA.aspx")
    End Sub
    Private Sub chkByModel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkByModel.CheckedChanged


        If cmbAssemblyList.SelectedIndex = 0 Then
            MSGBoxCtrl.Show("Alert!", "You have not selected Assembly.", "To see Model wise Part List, you need to select assembly first. Please select Assembly", MsgBoxStyle.OkOnly, "")
            chkByModel.Checked = False
            Exit Sub
        End If

        If chkByModel.Checked Then
            mPartList = PartList.GetPartList(mCompStatus.ModelID, , , "(SELECT)")
            Session("mPartlist") = mPartList
            cmbPartNo.DataSource = mPartList
        Else
            mPartList = PartList.GetPartList("", "", "(SELECT)")
            Session("mPartList") = mPartList
            cmbPartNo.DataSource = mPartList
        End If
        PartNo = IIf(mPartList.Contains(mCompStatus.Comp.PartName), mCompStatus.Comp.PartName, "")
        Description = IIf(txtPartDescription.Text.Length <> 0, mPartList(PartNo).Description, "")

        txtPartDescription.Text = PartNo
        txtDescription.Text = Description

        dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods

        mCompStatus.Comp.PartID = IIf(mPartList.Contains(mCompStatus.Comp.PartName), mCompStatus.Comp.PartID, Guid.Empty)

        cmbPartNo.DataBind()

        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            'txtPartDescription.Focus()
        Else
            cmbPartNo.Focus()
        End If


        If cmbATAChapter.Items.Contains(New System.Web.UI.WebControls.ListItem(mCompStatus.ATAChapter, mCompStatus.ATAID.ToString)) Then
            cmbATAChapter.SelectedValue = mCompStatus.ATAID.ToString
        Else
            cmbATAChapter.SelectedValue = Guid.Empty.ToString
        End If
        '=======================================================================================
        '  DataBind()
        dgInstallationValue.DataBind()

        upnlPartOnfo.Update()
        upnlInstallationValue.Update()
    End Sub
    Private Sub GetCompStatusForPart(ByVal PartIndex As Integer) 'Added by Saylee on 25-Aug-2009

        mCompStatus = CType(Session("mCompStatus"), CompStatus)


        'Dim mtmpCompStatusList As tmpCompStatusList = tmpCompStatusList.GetCompStatusList(Guid.Empty, mPartList(PartIndex).Name, "", mPartList(PartIndex).Description)
        Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(Guid.Empty.ToString, mPartList(PartNo).Name, mPartList(PartNo).Description)

        If mtmpCompListOnPartSelection.Count > 0 Then
            Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, mAssemblyStatus.ID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)

            txtCode.Text = tmpCompStatus.Comp.Code
            mCompStatus.Comp.PartID = tmpCompStatus.Comp.PartID
            mCompStatus.ATAID = tmpCompStatus.ATAID

            ''If mCompStatus.CompStatusPeriods.Count > 0 Then
            ''    For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
            ''        mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
            ''    Next
            ''    dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
            ''    dgInstallationValue.DataBind()
            ''End If

            Dim tmpCompStatusPeriod As CompStatusPeriod
            For Each tmpCompStatusPeriod In tmpCompStatus.CompStatusPeriods
                If mCompStatus.CompStatusPeriods.Count > 0 And Not mCompStatus.CompStatusPeriods.Contains(tmpCompStatusPeriod.PeriodID) Then
                    'mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewChildCompStatusPeriod(mCompStatus.ID, New Guid(cmbAssemblyList.SelectedValue), mAssemblyStatus.AsOnDate, tmpCompStatusPeriod.PeriodID, calInstalledOn.Text))
                    mCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mCompStatus.InstalledOn.ToString, tmpCompStatusPeriod.PeriodID, False, mCompStatus.InstalledOn.ToString))
                End If
            Next
            Session("mCompStatus") = mCompStatus
            dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
            DataBind()
            tmpCompStatus = Nothing
            ''  Else
            ''If mCompStatus.CompStatusPeriods.Count > 0 Then
            ''    For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
            ''        mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
            ''    Next
            ''    dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
            ''    dgInstallationValue.DataBind()
            ''End If
        End If
    End Sub
    Private Sub cmbPartNo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbPartNo.SelectedIndexChanged
        SetPartNoDescription()
        txtDescription.Text = IIf(cmbPartNo.SelectedIndex > 0, mPartList(cmbPartNo.SelectedIndex).Description, "")
        txtPartDescription.Text = IIf(cmbPartNo.SelectedIndex > 0, mPartList(cmbPartNo.SelectedIndex).Name, "")
        SetObject()
        txtDescription.DataBind()
        Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(cmbPartNo.SelectedValue.ToString, mPartList(New Guid(cmbPartNo.SelectedValue)).Name, mPartList(New Guid(cmbPartNo.SelectedValue)).Description)
        'End
        If mtmpCompListOnPartSelection.Count > 0 Then
            Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mCompStatus.AssemblyID, "")
            'Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)
            Dim tmpCompStatus As CompStatus
            If mInstallSelected = 1 Then
                If mRemovedCompStatus.IsSpareComp Then
                    tmpCompStatus = CompStatus.GetSpareCompStatus(mRemovedCompStatus.ID, True)
                    'End
                Else
                    tmpCompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)
                End If
            Else
                tmpCompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)

            End If
            txtCode.Text = tmpCompStatus.Comp.Code
            mCompStatus.Comp.PartID = tmpCompStatus.Comp.PartID
            mCompStatus.ATAID = tmpCompStatus.ATAID

            cmbATAChapter.SelectedValue = mCompStatus.ATAID.ToString
            cmbPartNo.SelectedValue = mCompStatus.Comp.PartID.ToString
            SetPartNoDescription()
            txtDescription.Text = IIf(cmbPartNo.SelectedIndex > 0, mPartList(cmbPartNo.SelectedIndex).Description, "")
            txtPartDescription.Text = IIf(cmbPartNo.SelectedIndex > 0, mPartList(cmbPartNo.SelectedIndex).Name, "")
            txtDescription.DataBind()

            mtmpCompListOnPartSelection = Nothing
            tmpCompStatus = Nothing

            If mCompStatus.CompStatusPeriods.Count > 0 Then
                For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                    If mCompStatus.CompStatusPeriods(i).PeriodID <> 1 And mCompStatus.CompStatusPeriods(i).PeriodID <> 2 Then
                        mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
                    End If
                Next
                dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                dgInstallationValue.DataBind()
            End If
        Else
            cmbATAChapter.SelectedIndex = 0
            'cmbATAChapter.DataBind()
        End If
        SetAssemblyPeriod()
        If cmbPartNo.Enabled = True Then
            setFocus(cmbPartNo)
        End If
        If cmbPartNo.SelectedIndex = 0 Then
            cmbATAChapter.SelectedIndex = 0
            ' cmbATAChapter.DataBind()
        End If
        ControlVisiblity1()
        upnlPartOnfo.Update()
        upnlInstInfo.Update()
        upnlInstallationValue.Update()
        upnlPartOnfo.Update()

    End Sub
    Private Sub txtPartDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPartDescription.TextChanged
        SetPartNoDescription()
        txtDescription.Text = Description
        txtPartDescription.Text = PartNo
        SetObject()
        txtDescription.DataBind()
        Dim SearchPartNo As String
        Dim PartID As Guid 'Added By Utkarsh On 09-May-2013 FOR ALL09052013-1
        If PartNo.Length = 0 Then
            SearchPartNo = "(SELECT)"
            PartID = Guid.Empty 'Added By Utkarsh On 09-May-2013 FOR ALL09052013-1
        Else
            SearchPartNo = PartNo
            PartID = mPartList(PartNo).ID 'Added By Utkarsh On 09-May-2013 FOR ALL09052013-1
        End If
        'Added By Utkarsh(PartID Criteria) On 09-May-2013 FOR ALL09052013-1
        Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(PartID.ToString, SearchPartNo, Description)
        'End
        If mtmpCompListOnPartSelection.Count > 0 Then
            Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mCompStatus.AssemblyID, "")
            Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)
            txtCode.Text = tmpCompStatus.Comp.Code
            mCompStatus.Comp.PartID = tmpCompStatus.Comp.PartID
            mCompStatus.ATAID = tmpCompStatus.ATAID

            cmbATAChapter.SelectedValue = mCompStatus.ATAID.ToString
            txtPartDescription.Text = mCompStatus.Comp.PartName.ToString
            SetPartNoDescription()
            txtDescription.Text = Description
            txtPartDescription.Text = PartNo
            mtmpCompListOnPartSelection = Nothing
            tmpCompStatus = Nothing

            If mCompStatus.CompStatusPeriods.Count > 0 Then
                For i As Integer = mCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                    If mCompStatus.CompStatusPeriods(i).PeriodID <> 1 And mCompStatus.CompStatusPeriods(i).PeriodID <> 2 Then
                        mCompStatus.CompStatusPeriods.Remove(mCompStatus.CompStatusPeriods(i).ID)
                    End If
                Next
                dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
                dgInstallationValue.DataBind()
            End If
        Else
            cmbATAChapter.SelectedIndex = 0
            'cmbATAChapter.DataBind()
        End If
        SetAssemblyPeriod()
        If PartNo.Length = 0 Then
            cmbATAChapter.SelectedIndex = 0
            'cmbATAChapter.DataBind()
        End If
        upnlInstInfo.Update()
        upnlPartOnfo.Update()
        upnlInstallationValue.Update()
    End Sub
    Private Sub imgbtManufacturer1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtManufacturer1.Click
        'Response.Redirect("wfManufacturer_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfInstallComp_AJAX.aspx&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManufacturerWindow", "OpenManufacturerWindow()", True)
    End Sub
    'Private Sub imgbtManufacturer_Click(sender As Object, e As System.EventArgs) Handles imgbtManufacturer.Click
    '    Dim Str As String
    '    Str = "OpenManufacturerWindow();"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenManufacturerWindow", Str, True)
    '    'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManufacturerWindow", "OpenManufacturerWindow()", True)
    'End Sub

    ''Added By Saylee On 27-Nov-2014 
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte


        If mCompStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 1)
            Session("mFileAttach") = mFileAttach
        End If

        'mEmployee.ImageFile = file1
        'mEmployee.ImageSize = 0
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        If TbContInst.ActiveTabIndex = 0 Then
            MessageBoxResult()
        ElseIf TbContInst.ActiveTabIndex = 1 Then
            MessageBoxResultService()
        ElseIf TbContInst.ActiveTabIndex = 2 Then
            MessageBoxResultInsp()
        ElseIf TbContInst.ActiveTabIndex = 3 Then
            MessageBoxResultMod()
        End If

    End Sub
    Private Sub hdnBtnPart_Click(sender As Object, e As System.EventArgs) Handles hdnBtnPart.Click
        mPartList = PartList.GetPartList(, , "(SELECT)")
        Session("mPartList") = mPartList
        cmbPartNo.DataSource = mPartList
        cmbPartNo.DataBind()
        upnlPartNo.Update()
    End Sub

    Private Sub hdnBtnManufacturer_Click(sender As Object, e As System.EventArgs) Handles hdnBtnManufacturer.Click
        mManufacturerList = ManufacturerList.GetManufacturerList(, "<SELECT>")
        cmbManufacturerList.DataSource = mManufacturerList
        Session("mManufacturerList") = mManufacturerList
        cmbManufacturerList.DataBind()
        upnlManufacturer.Update()
    End Sub
    Private Sub hdnBtnSelectLog_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSelectLog.Click
        SetLog()
        DataFieldBind()
        upnlInstInfo.Update()
        upnlInstallationValue.Update()
    End Sub
    Private Sub hdnAddPeriod_Click(sender As Object, e As System.EventArgs) Handles hdnAddPeriod.Click
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        AddSelectedPeroids()
        dgInstallationValue.DataSource = mCompStatus.CompStatusPeriods
        dgInstallationValue.DataBind()
        ControlVisiblity1()
        upnlPartOnfo.Update()
        upnlInstallationValue.Update()
    End Sub
    Protected Sub btnAddPeriod_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddPeriod.Click
        If cmbAssemblyList.SelectedIndex = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Select an Assembly from the list.", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfInstallCompBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            msg1.Show()
            Exit Sub
        Else
            SetObject()
            SetGridObject()
            SetPeroids()
            '''Response.Redirect("wfSelectPeriod.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage2=wfInstallCompBA.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)

        End If
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
            If Not mAssemblyStatus Is Nothing Then


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
            End If
            pnlThrustyComponentDet.Visible = True
        Else
            pnlThrustyComponentDet.Visible = False
            'txtB22Current.Text = "0"
            'txtB22LifeLimit.Text = "0"

            'txtB24Current.Text = "0"
            'txtB24LifeLimit.Text = "0"

            'txtB26Current.Text = "0"
            'txtB26LifeLimit.Text = "0"

            'chkB22IsCurrent.Checked = False
            'chkB24IsCurrent.Checked = False
            'chkB26IsCurrent.Checked = False

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
            MSGBoxCtrl.show(" Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        If (txtPartDescription.Text.Trim.IndexOf("[") >= 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text.Trim)
            Description = Trim(txtPartDescription.Text.Trim)
        End If


        Dim EventLogDetail As String = "Printed From Component Installation through maintenance with As On Date: " + New SmartDate(Today.Date.ToString, False).FormattedText + " , Part: " + txtPartDescription.Text + " , Serial No.: " + txtSerialNo.Text.Trim
        Dim ReportData As Flypal.ReportData
        If ObjHistoryCard.Count > 0 Then
            ReportData = New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             "", "Component History Card Report", New SmartDate(Today.Date.ToString, False).FormattedText, "", PartNo, txtSerialNo.Text, ObjHistoryCard(0).ATA, AppSettings("Product Version"), AppSettings("SINote"), txtDescription.Text.Trim, "", "", "Assembly", AppSettings("Logo"))

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
    Private Sub lnkPrintLogBookEntry_Click(sender As Object, e As System.EventArgs) Handles lnkPrintLogBookEntry.Click  'Added By Prashant On 7-May-2021 ALL07052021
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mLogEntryFormat As New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        RptCommonHistory = New crptLogEntryFormat

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mCompStatus.InstalledOn, mCompStatus.InstalledOn, "", mAssemblyStatus.AssemblyTypeName, _
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "", _
                                                        mAssemblyStatus.MachineID.ToString, True, True, IsRemoved:=False, IsInstalled:=True, _
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True, _
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False, _
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mCompStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "", mCompStatus.InstalledOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo,
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo,
           IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper),
           AppSettings("Product Version"), AppSettings("SINote"),
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.",
           "True", mCompStatus.InstalledOnFormatted, "", AppSettings("Logo"))

        'here above removed by saylee'SearchStr1:="OpenFromAssemblyRemovalInstallationComponentRemovalInstallation" 

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, "LogEntryFormat", mLogEntryFormat)      'This is direct from object records 

        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        RptCommonHistory.SetDataSource(ds)
        Session("CrystalReport") = RptCommonHistory
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "LogEntryFormat", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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

#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
#End Region

#Region " Events "
    Private Sub btnPrintService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintService.Click
        If (Not User.IsInRole("ComponentInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstallComponentMonitor
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of the Component", "ATA Chapter :", _
       mCompStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation", _
       "Period", "Component", , "Assembly"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of the Component", "ATA Chapter :", _
                      mCompStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation", _
                      "", "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Part No. :", _
                           mCompStatus.PartName, , , , , , , , , , , , , , , , , , _
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String), _
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Part No. :", _
                          mCompStatus.PartName, , , , , , , , , , , , , , , , , , _
                          "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Description :", _
                                           mCompStatus.Description, , , , , , , , , , , , , , , , , , _
                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String), _
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Description :", _
                                          mCompStatus.Description, , , , , , , , , , , , , , , , , , _
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :", _
                                                         mCompStatus.SerialNo, , , , , , , , , , , , , , , , , , _
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String), _
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :", _
                                                          mCompStatus.SerialNo, , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))

                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Code :", _
                                                         mCompStatus.Comp.Code, , , , , , , , , , , , , , , , , , _
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String), _
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Code :", _
                                                          mCompStatus.Comp.Code, , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))

                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position :", _
                                                          mCompStatus.Position, , , , , , , , , , , , , , , , , , _
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String), _
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position :", _
                                                          mCompStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                                          "", , , , , , , , , , , , , , , , , , _
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String), _
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                                          "", , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "", _
                   "", , , , , , , , , , , , , , , , , , _
                    CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String), _
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
            End If
        Next

        'For Install Component Service List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , lblServiceInfo.Text))

        'For Install Component service List
        'ReportDetails.Add(New rptStatus(, 2, , _
        '      , , , dgMonitorServiceStatusList.Columns.Item(4).HeaderText, , dgMonitorServiceStatusList.Columns.Item(5).HeaderText, dgMonitorServiceStatusList.Columns.Item(6).HeaderText, _
        '      dgMonitorServiceStatusList.Columns.Item(7).HeaderText, dgMonitorServiceStatusList.Columns.Item(8).HeaderText, dgMonitorServiceStatusList.Columns.Item(9).HeaderText, _
        '      dgMonitorServiceStatusList.Columns.Item(10).HeaderText, , dgMonitorServiceStatusList.Columns.Item(11).HeaderText, dgMonitorServiceStatusList.Columns.Item(13).HeaderText, dgMonitorServiceStatusList.Columns.Item(14).HeaderText, _
        '      dgMonitorServiceStatusList.Columns.Item(15).HeaderText, dgMonitorServiceStatusList.Columns.Item(16).HeaderText, dgMonitorServiceStatusList.Columns.Item(17).HeaderText, _
        '      , , , dgMonitorServiceStatusList.Columns.Item(18).HeaderText, , ))
        ReportDetails.Add(New rptStatus(, 2, ,
            , , , dgMonitorServiceStatusList.Columns.Item(4).HeaderText, , dgMonitorServiceStatusList.Columns.Item(5).HeaderText, dgMonitorServiceStatusList.Columns.Item(6).HeaderText,
            dgMonitorServiceStatusList.Columns.Item(7).HeaderText, dgMonitorServiceStatusList.Columns.Item(8).HeaderText, dgMonitorServiceStatusList.Columns.Item(9).HeaderText,
            dgMonitorServiceStatusList.Columns.Item(10).HeaderText, , dgMonitorServiceStatusList.Columns.Item(11).HeaderText, dgMonitorServiceStatusList.Columns.Item(13).HeaderText, dgMonitorServiceStatusList.Columns.Item(14).HeaderText,
            dgMonitorServiceStatusList.Columns.Item(15).HeaderText, dgMonitorServiceStatusList.Columns.Item(16).HeaderText, dgMonitorServiceStatusList.Columns.Item(17).HeaderText,
            , , , dgMonitorServiceStatusList.Columns.Item(18).HeaderText, , ))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mInstallCompMonitorServiceStatusList.Count
        Dim m As Integer

        For m = 0 To TotalCount1 - 1
            Dim str(14) As String
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
            str(10) = ""
            str(11) = ""
            str(12) = ""
            str(13) = ""
            str(14) = ""
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(4).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(5).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(6).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(7).Text <> "&nbsp;" Then str(3) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(8).Text <> "&nbsp;" Then str(4) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(9).Text <> "&nbsp;" Then str(5) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(10).Text <> "&nbsp;" Then str(6) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(11).Text <> "&nbsp;" Then str(7) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(11).Text.Replace("<BR>", vbCrLf)
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(12).Text <> "&nbsp;" Then str(8) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(12).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(13).Text <> "&nbsp;" Then str(9) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(14).Text <> "&nbsp;" Then str(10) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(15).Text <> "&nbsp;" Then str(11) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(16).Text <> "&nbsp;" Then str(12) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(17).Text <> "&nbsp;" Then str(13) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(17).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(18).Text <> "&nbsp;" Then str(14) = Me.dgMonitorServiceStatusList.Rows(m).Cells.Item(18).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
                   , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), , str(7),
                       str(9), str(10), str(11), str(12), str(13), , , , str(14), , ))

        Next

        Dim MPDType As String = ""
        Dim ReportName As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ReportName = "Maintenance Event List Report"
        Else

            ReportName = "Component Service Status List Report"
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite, ReportName, lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
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
        'Dim ModelID As String = ModelID
        Dim partlist As PartListAutoComplete
        If isModel Then
            partlist = PartListAutoComplete.GetPartList(prefixText, ModelID.ToString)
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

#End Region

#Region " Service Tab "

#Region " Variable Declarations "
    Public mInstallCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList

    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Public mPartMonitorServiceTypeList As PartMonitorServiceTypeList

    Public mInstallCompStatus As CompStatus                                          'Code Added 30,Jan,2007
    'Public mCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList    'Code Added 30,Jan,2007
    Public mCompInfo As String 'Code Added 30,Jan,2007
    Public LookIn, TextFor, Code, SearchFor As String 'Code Added 9th-Jan-2008 by Saylee
    Public mBoardInfo As AircraftInformationBoard.BoardInfo  'Added by Saylee on 17-June-2009
#End Region

#Region " Business Methods "
    Private Sub GetSessionService()
        mInstallCompMonitorServiceStatusList = CType(Session("mInstallCompMonitorServiceStatusList"), tmpComplyCompMonitorServiceStatusList)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mRemovedCompStatus = CType(Session("mRemovedCompStatus"), CompStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        mPartMonitorServiceTypeList = CType(Session("mPartMonitorServiceTypeList"), PartMonitorServiceTypeList)
        mMachine = CType(Session("mMachine"), Machine)
        '===Added by Saylee on 9th-Jan-2008===============
        LookIn = Session("LookIn")
        TextFor = Session("TextFor")
        Code = Session("Code")
        SearchFor = Session("SearchFor")
        '=================================================

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub addAttributesService()
        txtCode.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCode').value,event)")
    End Sub
    Private Sub GridBindService()
        dgMonitorServiceStatusList.DataSource = mInstallCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
        SetGridService()
    End Sub
    Private Sub DataFieldBindService()
        mInstallCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(mCompStatus.InstalledOn.ToString, mAssemblyStatus.MachineID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableService.Checked)
        dgMonitorServiceStatusList.DataSource = mInstallCompMonitorServiceStatusList
        Session("mInstallCompMonitorServiceStatusList") = mInstallCompMonitorServiceStatusList
        mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList("(ALL)")
        Session("mPartMonitorServiceTypeList") = mPartMonitorServiceTypeList
        cmbSearchFor.DataSource = mPartMonitorServiceTypeList
        'DataBind()
        dgMonitorServiceStatusList.DataBind()
        cmbSearchFor.DataBind()
        chkApplicableService.Checked = False
    End Sub
    Private Sub ControlVisibilityService()
        btnAddTopService.Visible = (mInstallCompMonitorServiceStatusList.Count > 5)
        btnPrintService.Enabled = (Not mInstallCompMonitorServiceStatusList Is Nothing And mInstallCompMonitorServiceStatusList.Count <> 0)
        dgMonitorServiceStatusList.Columns(20).Visible = IIf(chkApplicableService.Checked, False, True)
    End Sub
    Private Sub NewRecordService()
        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.InstalledOn.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mAssemblyStatus.HourType, mCompStatus)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then

            'Changed By Utkarsh On 27-Jul-2011 For All19072011
            MarkLog(Util.Action.[New], "Install Component Service Status", User.Identity.Name & " is not Authorized User to add new ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End

            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfInstallCompMonitorServiceStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If


        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Install Component Service Status", "", Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
        'End

        'Code added By Deven on 1/4/2008
        'Response.Redirect("wfCompMonitorServiceStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallCompMonitorServiceStatusList.aspx")
        Session("mInstallCompMonitorServiceStatusList") = mInstallCompMonitorServiceStatusList

        'Code added By Deven on 25/09/2009
        Dim mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
        mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString)
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
        '----------------------------------
        Session("TabIndex") = TbContInst.ActiveTabIndex
        ' Response.Redirect("wfPartMonitorServiceList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage5=wfInstallComp_AJAX.aspx")
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorServiceList_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)

        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
        If mComponentMaintananceListCount Is Nothing Or mComponentMaintananceListCount.MaintenanceServiceListCount = 0 Then
            Dim mPartMonitorService As PartMonitorService
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mPartMonitorService = PartMonitorService.NewPartMonitorService(ID, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType, ID)
            Session.Remove("mPartMonitorServiceList")
            Session("mPartMonitorService") = mPartMonitorService

            MarkLog(Util.Action.[New], "Part Service", "", Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)

            'Response.Redirect("wfPartMonitorService_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage6=wfInstallComp_AJAX.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow();", True)

        ElseIf mComponentMaintananceListCount.MaintenanceServiceListCount > 0 Then
            'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterListWindow", "OpenSeriviceMasterListWindow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorServiceList_Ajax.aspx?GChildPage2=&GChildPage4=wfInstallComp_AJAX.aspx & &GChildPage5=wfInstallComp_AJAX.aspx');", True)

        End If
        '------------------------------------------------
    End Sub
    Private Sub FindNowService()
        'Added by Saylee on 9th-Jan-2007 to keep Searching criteria as it is
        Session("LookIn") = cmbLookIn.SelectedIndex
        Session("TextFor") = txtFor.Text
        Session("Code") = txtCode1.Text
        Session("SearchFor") = cmbSearchFor.SelectedIndex
        '=================================================================
        Select Case cmbLookIn.SelectedIndex
            Case 0, -1  'All
                mInstallCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableService.Checked)
            Case 1  'ATA Code
                mInstallCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, Val(txtCode1.Text), , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableService.Checked)
            Case 2  'Service Type ID
                mInstallCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , CInt(cmbSearchFor.SelectedValue), , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableService.Checked)
            Case 3 ' Work Order No.
                mInstallCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , txtFor.Text.Trim, , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableService.Checked)
            Case 4  'Show In C of A
                mInstallCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , True, mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableService.Checked)
        End Select

        Session("mInstallCompMonitorServiceStatusList") = mInstallCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mInstallCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
    End Sub
    Private Sub Setcontrol()
        'Fuction added by Saylee on 9th-Jan-2008 to keep Searching criteia as it is
        cmbLookIn.SelectedValue = LookIn 'IIf(LookIn = "", "(All)", LookIn)
        txtFor.Text = TextFor
        txtCode1.Text = Code
        cmbSearchFor.SelectedIndex = IIf(SearchFor Is Nothing, 0, SearchFor) ' 'IIf(SearchFor = "", "(All)", SearchFor)
        DisplayControls(cmbLookIn.SelectedIndex)
        FindNowService()
    End Sub
    Private Sub DisplayControls(ByVal Index As Integer)
        'Commented and Added by Saylee on 9th-Jan-2008 to keep Searching criteia as it is
        'txtFor.Text = ""
        'txtCode.Text = ""
        txtFor.Text = IIf(Index = 3, txtFor.Text, "")
        txtCode1.Text = IIf(Index = 1, txtCode1.Text, "")
        '=========================================================
        txtCode1.Visible = IIf(Index = 1, True, False)
        txtFor.Visible = IIf(Index = 3, True, False)
        lblFor.Visible = (Index > 0 And Index <> 4)
        cmbSearchFor.Visible = (Index = 2)
        If cmbLookIn.Enabled = True Then
            setFocus(cmbLookIn)
        End If
    End Sub
    Private Sub SetPageService()
        If Not mCompStatus.IsNew Then
            lblTitle.Text = "Installation Information of the Component [Part:" & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & "]"
        Else
            lblTitle.Text = "Installation Information of the Component [New]"
        End If
        'CNDC
        'lblServiceInfo.Text = "List of all the Servicing on the Component as of " & mCompStatus.InstalledOn.ToString & ". All the values of all the Services will be as of " & mAssemblyStatus.InstalledOn.ToString
        'lblServiceInfo.Text = "List of all the Servicing on the Component as of " & mCompStatus.InstalledOnFormatted & ". All the values of all the Services will be as of " & mAssemblyStatus.InstalledOnFormatted

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "Maintenance Events"
        Else
            ServiceMPDTitle = "Service"
        End If


        lblServiceInfo.Text = "List of all the Component " + ServiceMPDTitle + "(s) on the Component and values of all the " + ServiceMPDTitle + "(s) will be as of " & mCompStatus.InstalledOnFormatted & ""
        lblCaption.Text = "List of " + ServiceMPDTitle + " Status: " & mInstallCompMonitorServiceStatusList.Count & " Record(s) found."

    End Sub
    Private Sub SetGridService()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorServiceStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorServiceStatusList.Rows.Item(j).Cells(25).Text, Boolean)
            If B = False Then
                'lb = CType(dgMonitorServiceStatusList.Rows.Item(j).Cells(21).FindControl("lnkView"), LinkButton)
                'lb.Enabled = False
                dgMonitorServiceStatusList.Rows.Item(j).Cells(24).Enabled = False
            End If

        Next
    End Sub
    Private Sub DeleteServiceRecord(ByVal Index As Integer)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "Do you want to Delete the record?", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfInstallCompMonitorServiceStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "Do you want to Delete the record?", MsgBoxStyle.YesNo, "Delete")
        mInstallCompMonitorServiceStatusList.CurrentIndex = Index
        Session("mInstallCompMonitorServiceStatusList") = mInstallCompMonitorServiceStatusList
    End Sub
    Private Sub EditMasterRecordService(ByVal mMasterId As Guid, ByVal mId As Guid, ByVal Index As Integer)
        Dim CompMonitorServiceStatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo = mInstallCompMonitorServiceStatusList.Item(Index)
        REM: if selected record is Master record then master form is opened
        '    else entry form is opened
        If CompMonitorServiceStatusInfo.IsMaster = True Then
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mInstallCompMonitorServiceStatusList.Item(Index).CompMonitorServiceStatusID, mAssemblyStatus.ID, mCompStatus.ID, mAssemblyStatus.HourType)
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Else
            Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus
            mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mInstallCompMonitorServiceStatusList.Item(Index).CompMonitorServiceStatusID, mInstallCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorServiceStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType)
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mAssemblyStatus.HourType)
            Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Session("EnFrom") = 1 'EditRecord
            If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then

                'Added By Utkarsh On 27-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mId).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mId).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mId).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mId).MonitorInfo.Replace(Environment.NewLine, " ")
                MarkLog(Util.Action.Edit, "Install Component Service Status", User.Identity.Name & " is not Authorized User to edit " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                'End
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                'msg.ReplacePage = "wfInstallCompMonitorServiceStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
                'msg.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        ''***
        Dim mPartMonitorService As PartMonitorService
        mPartMonitorService = PartMonitorService.GetPartMonitorService(mMasterId, mAssemblyStatus.HourType)
        ''mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mMachine.HourType)
        ''Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mMachine") = mMachine
        Session("mPartMonitorService") = mPartMonitorService
        'RemoveSession()

        'Added By Utkarsh On 27-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mId).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mId).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mId).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mId).MonitorInfo.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.Edit, "Install Component Service Status", MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        'End

        'Response.Redirect("wfPartMonitorService_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage6=wfInstallComp_AJAX.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow();", True)

    End Sub
    Private Sub MessageBoxResultService()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0
        Dim mCompMonitorServiceStatusID As Guid

        If Result1 > 0 Then
            GetSession()
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mCompMonitorServiceStatusID = mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID
                            MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID, 8)
                            '=============================
                            'Added By Vikrant On 25-Nov-2014
                            If mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID)
                            End If

                            CompMonitorServiceStatus.DeleteCompMonitorServiceStatus(mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).CompMonitorServiceStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mFileAttach = Nothing
                            Session("mMachineMaintenance") = mMachineMaintenance
                            ' Response.Redirect("wfInstallCompMonitorServiceStatusList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                            DataFieldBindService()
                            SetPageService()
                            SetGridService()
                            ControlVisibility()

                            upnlCaption.Update()
                            upnlServiceGrid.Update()
                            upnlServiceButtons.Update()
                            upnlServiceInfo.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                                MarkLog(Util.Action.Delete, "Install Component Service Status", "Can't delete : " & MaintDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 27-Jul-2011 For All19072011
                                'MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mInstallCompMonitorServiceStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                                MarkLog(Util.Action.Delete, "Install Component Service Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorServiceStatusID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    FindNowService()
                    SetPageService()
                    SetGridService()

                    upnlServiceGrid.Update()
                    upnlTitle.Update()
                    upnlServiceInfo.Update()
                    upnlCaption.Update()
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    FindNowService()
                    SetPageService()
                    upnlServiceGrid.Update()
                    upnlTitle.Update()
                    upnlServiceInfo.Update()
                    upnlCaption.Update()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    SetPageService()
                    SetGridService()
                    ControlVisibility()

                    upnlServiceGrid.Update()
                    upnlServiceButtons.Update()
                    upnlServiceInfo.Update()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
#End Region

#Region " Service Events "
    Private Sub btnAddService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddService.Click, btnAddTopService.Click
        Session("TabIndex") = TbContInst.ActiveTabIndex
        NewRecordService()
    End Sub
    Private Sub cmbLookIn_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookIn.SelectedIndexChanged
        DisplayControls(cmbLookIn.SelectedIndex)
    End Sub
    Private Sub hdnBtnSeriviceMasterList_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSeriviceMasterList.Click
        DataFieldBindService()
        SetPageService()
        SetGridService()
        upnlServiceGrid.Update()
        upnlServiceInfo.Update()
        upnlCaption.Update()
    End Sub
    Private Sub dgMonitorServiceStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorServiceStatusList.RowCommand
        Session("TabIndex") = TbContInst.ActiveTabIndex
        Select Case e.CommandName
            Case "Comply"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                Dim mID = mInstallCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID
                If (Not User.IsInRole("ComponentInstallationNew")) Then
                    MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Comply, "Install Component Service Status", User.Identity.Name & " is not Authorized User to comply " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus
                mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mInstallCompMonitorServiceStatusList.Item(Index).CompMonitorServiceStatusID, mInstallCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorServiceStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType)
                REM: Complance of one time monitoring is done only once.
                If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "You are trying to comply component.One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
                    REM: Complance of expiery monitoring is done only once.
                    MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mCompStatus.InstalledOn.ToString, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
                Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                Session("EnFrom") = 0 'NewRecord

                'Added by Saylee on 17-Jun-2009
                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevCompMonitorServiceStatus.ID)
                Session("mBoardInfo") = mBoardInfo
                '**************************************

                'Added By Vikrant On 25-Nov-2014
                Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach
                'End

                'Added By Utkarsh On 27-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                MarkLog(Util.Action.Comply, "Install Component Service Status", MaintDetail, Util.ErrorType.NoError, mID, EventLogID)
                'End

                'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallCompMonitorServiceStatusList.aspx")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                Dim mID = mInstallCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID
                Dim CompMonitorServiceStatusInfo As tmpComplyCompMonitorServiceStatusList.tmpComplyCompMonitorServiceStatusInfo = mInstallCompMonitorServiceStatusList.Item(Index)
                REM: if selected record is Master record then master form is opened
                '    else entry form is opened
                If CompMonitorServiceStatusInfo.IsMaster = True Then
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mInstallCompMonitorServiceStatusList.Item(Index).CompMonitorServiceStatusID, mAssemblyStatus.ID, mCompStatus.ID, mAssemblyStatus.HourType, True, mCompStatus)
                    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

                    'Added by Saylee on 17-Feb-2011
                    Dim mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
                    mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString)
                    Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList

                    'Added By Vikrant On 25-Nov-2014
                    If mCompMonitorServiceStatus.IsAttachmentAdded Then
                        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID) 'Sort = 1 - Installation
                        Session("mFileAttach") = mFileAttach
                    Else
                        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID)
                        Session("mFileAttach") = mFileAttach
                    End If
                    'End

                    Response.Redirect("wfCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx")
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorServiceStatus.aspx?BackPage='& Request.QueryString('BackPage') & '&ChildPage=' & Request.QueryString('ChildPage') & '&GChildPage=' & Request.QueryString('GChildPage') & '&GChildPage1=' & Request.QueryString('GChildPage1') & '&GChildPage2=' & Request.QueryString('GChildPage2') & '&GChildPage3=' & Request.QueryString('GChildPage3') & '&GChildPage4=wfInstallCompMonitorServiceStatusList.aspx'); ", True)
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame(wfCompMonitorServiceStatus.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
                Else
                    Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus
                    mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mInstallCompMonitorServiceStatusList.Item(Index).CompMonitorServiceStatusID, mInstallCompMonitorServiceStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorServiceStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType, , mCompStatus)
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mAssemblyStatus.HourType, True)
                    Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
                    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                    Session("EnFrom") = 1 'EditRecord
                    If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then

                        'Added By Utkarsh On 27-Jul-2011 For All19072011
                        MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                        MarkLog(Util.Action.Edit, "Install Component Service Status", User.Identity.Name & " is not Authorized User to edit " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                        'End
                        MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                    'Added By Utkarsh On 27-Jul-2011 For All19072011
                    MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Edit, "Install Component Service Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
                    'End

                    'Added By Vikrant On 25-Nov-2014
                    If mCompMonitorServiceStatus.IsAttachmentAdded Then
                        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID) 'Sort = 1 - Installation
                        Session("mFileAttach") = mFileAttach
                    Else
                        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID)
                        Session("mFileAttach") = mFileAttach
                    End If
                    'End

                    'Response.Redirect("wfComplyCompMonitorServiceStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
                End If
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                Dim mID = mInstallCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID
                GridBindService()
                SetGridService()
                ControlVisibility()
                If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
                    'Added By Utkarsh On 27-Jul-2011 For All19072011
                    MaintDetail = "Reg No. : " + mInstallCompMonitorServiceStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorServiceStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorServiceStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorServiceStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Delete, "Install Component Service Status", User.Identity.Name & " is not Authorized User to delete " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteServiceRecord(Index)
            Case "EditMaster"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                Dim mID = mInstallCompMonitorServiceStatusList(Index).CompMonitorServiceStatusID
                Dim mMasterId As Guid = mInstallCompMonitorServiceStatusList(Index).PartMonitorServiceID
                Session("EditMasterRecord") = "True"
                EditMasterRecordService(mMasterId, mID, Index)
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mInstallCompMonitorServiceStatusList(Index).ID)
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNowService()
        SetPageService()
        SetGridService()
        ControlVisibilityService()
        upnlServiceGrid.Update()
        upnlTitle.Update()
        upnlServiceInfo.Update()
        upnlCaption.Update()
        upnlServiceTopButtons.Update()
        upnlServiceButtons.Update()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click

        If (Not User.IsInRole("ComponentInstallationPrint")) Then

            'Commented By Utkarsh On 26-Jul-2011 For All19072011
            'MarkLog(Util.Action.Print, "InstallComp", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'End

            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfInstallCompBA.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crDetInstallRemoveComp
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Part and Serial No. Info
        Dim LHCount As Integer
        LHCount = 6
        ReportDetails.Add(New rptStatus(, 0, PartNo))
        Dim I As Integer
        For I = 0 To LHCount - 1
            If I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, , "ATA Chapter :",
    cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "",
    "", "", , ""))
            ElseIf I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, , "Part No. :",
   PartNo, , , , , , , , , , , , , , , , , "",
    "", "", , ""))
            ElseIf I = 1 Then
                ReportDetails.Add(New rptStatus(, 1, , lblDescription.Text,
      txtDescription.Text, , , , , , , , , , , , , , , , , "",
      "", "", , ""))
            ElseIf I = 2 Then
                ReportDetails.Add(New rptStatus(, 1, , lblSerialNo.Text,
   txtSerialNo.Text, , , , , , , , , , , , , , , , , "",
   "", "", , ""))
            ElseIf I = 3 Then
                ReportDetails.Add(New rptStatus(, 1, , lblCode.Text,
    txtCode.Text, , , , , , , , , , , , , , , , , "",
    "", "", , ""))
            ElseIf I = 4 Then
                ReportDetails.Add(New rptStatus(, 1, , lblPosition.Text,
   txtPosition.Text, , , , , , , , , , , , , , , , , "",
   "", "", , ""))
            End If
        Next

        'For Removal Value Grid
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
        ReportDetails.Add(New rptStatus(, 2, , , , , , lbInstallationInfo.InnerText, , , , , , , , , , , , , , lblAssemblyValues.Text))
        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 3, , , , lblAssembly.Text,
                                             cmbAssemblyList.SelectedItem.Text, , , , , , , , , , , , , , , , dgInstallationValue.Columns.Item(0).HeaderText,
                                              dgInstallationValue.Columns.Item(1).HeaderText, , dgInstallationValue.Columns.Item(2).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 3, , , , lblAssembly.Text,
                            cmbAssemblyList.SelectedItem.Text, , , , , , , , , , , , , , , , "", "", , ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblInstalledOn.Text,
                     New SmartDate(calInstalledOn.Text).FormattedText, , , , , , , , , , , , , , , , CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String),
                     CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String), , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , , , lblInstalledOn.Text,
                  New SmartDate(calInstalledOn.Text).FormattedText, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text,
                                           txtWorkOrderNo.Text, , , , , , , , , , , , , , , ,
                                          CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String), ,
                                                   CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text,
                                    txtWorkOrderNo.Text, , , , , , , , , , , , , , , ,
                                         "", "", , ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text,
                                           txtNote.Text, , , , , , , , , , , , , , , ,
                                                  CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text,
                                    txtNote.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , tbtDoneby.Text,
                                           txtDoneBy.Text, , , , , , , , , , , , , , , ,
                                                  CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , tbtDoneby.Text,
                                    txtDoneBy.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblLicenceNo.Text,
                                           mCompStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , ,
                                                  CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblLicenceNo.Text,
                                    mCompStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblPlace.Text,
                                           txtPlace.Text, , , , , , , , , , , , , , , ,
                                                  CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblPlace.Text,
                                    txtPlace.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , "",
                                          "", "", "", , , , , , , , , , , , , , , , CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                  , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , "",
                                          "", "", "", , , , , , , , , , , , , , , , "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 4, , "",
                                           "", "", "", , , , , , , , , , , , , , , , CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(m).CompInstallationValueFormatted, String),
                                                   , CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyInstallationValueFormatted, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Install Component Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)

        Dim mrptImage As rptImage = rptImage.GetImage(ds)

        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        '   MarkLog(Util.Action.Print, "InstallCompMonitorInspStatusList", "Component Monitor Insp Status List Report", Util.ErrorType.HandledError, mInstallCompStatus.ID)

        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub dgMonitorServiceStatusList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorServiceStatusList.Sorting
        mInstallCompMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInstallCompMonitorServiceStatusList") = mInstallCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mInstallCompMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
        SetGridService()
    End Sub
    Private Sub btnCloseService_Click(sender As Object, e As System.EventArgs) Handles btnCloseService.Click, btnCloseTopService.Click
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Install Component Service Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        SetSession()
        ' RemoveSession()
        Session.Remove("mInstallCompMonitorServiceStatusList")
        mPartMonitorServiceTypeList = Nothing

        Session.Remove("mMachineMaintenance") 'Added by Saylee on 13th-Oct-2009
        Session.Remove("mFileAttach")
        mInstallCompMonitorServiceStatusList = Nothing
        Session("FromInstallCompMonitorServiceStatusList") = True
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
#End Region

#End Region

#Region "Common Events "
    Private Sub TbContInst_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbContInst.ActiveTabChanged
        'If Not Session("TabIndex") Is Nothing Then TbContInst.ActiveTabIndex = CType(Session("TabIndex"), Integer) : Session.Remove("TabIndex")
        Select Case TbContInst.ActiveTabIndex
            Case 0
                DataBindGrid()
            Case 1
                lblTitle.Text = "Install Component Service Status List"
                upnlTitle.Update()

                addAttributesService()
                GetSessionService()
                EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 27-Jul-2011 For All19072011
                mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                Session("mMachine") = mMachine
                DataFieldBindService()
                Setcontrol()   'Added by Saylee on 9th-Jan-2008
                ControlVisibilityService()
                SetPageService()
                btnPrintService.Enabled = (Not mInstallCompMonitorServiceStatusList Is Nothing And mInstallCompMonitorServiceStatusList.Count <> 0)
                SetGridService()
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
            Case 2
                lblTitle.Text = "Install Component Inspection Status List"
                upnlTitle.Update()

                addAttributesInsp()
                GetSessionInsp()
                EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 27-Jul-2011 For All19072011
                DataFieldBindInsp()
                mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                Session("mMachine") = mMachine
                SetcontrolInsp()   'Added by Saylee on 9th-Jan-2008
                ControlVisibilityInsp()
                SetPageInsp()
                btnPrintInsp.Enabled = (Not mInstallCompMonitorInspStatusList Is Nothing And mInstallCompMonitorInspStatusList.Count <> 0)
                SetGridInsp()
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
            Case 3
                lblTitle.Text = "Install Component Modification Status List"
                upnlTitle.Update()

                addAttributesMod()
                GetSessionMod()
                EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 27-Jul-2011 For All19072011
                DataFieldBindMod()
                mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                Session("mMachine") = mMachine
                SetcontrolMod()   'Added by Saylee on 9th-Jan-2008
                ControlVisibilityMod()
                SetPageMod()
                btnPrintMod.Enabled = (Not mInstallCompMonitorModStatusList Is Nothing And mInstallCompMonitorModStatusList.Count <> 0)
                SetGridMod()
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
        End Select
    End Sub
#End Region

#Region " Insp Tab "

#Region " Variable Declarations "
    Public mInstallCompMonitorInspStatusList As tmpComplyCompMonitorInspStatusList

    Public mCompMonitorInspStatus As CompMonitorInspStatus
    Public mPartMonitorInspTypeList As PartMonitorInspTypeList

    ' Public mCompMonitorInspStatusList As tmpComplyCompMonitorInspStatusList
    Public LookInInsp, TextForInsp, CodeInsp, SearchForInsp As String

#End Region

#Region " Business Methods "
    Private Sub GetSessionInsp()
        mInstallCompMonitorInspStatusList = CType(Session("mInstallCompMonitorInspStatusList"), tmpComplyCompMonitorInspStatusList)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mRemovedCompStatus = CType(Session("mRemovedCompStatus"), CompStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompMonitorInspStatus = CType(Session("mCompMonitorInspStatus"), CompMonitorInspStatus)
        mPartMonitorInspTypeList = CType(Session("mPartMonitorInspTypeList"), PartMonitorInspTypeList)
        mMachine = CType(Session("mMachine"), Machine)
        '===Added by Saylee on 9th-Jan-2008===============
        LookInInsp = Session("LookInInsp")
        TextForInsp = Session("TextForInsp")
        CodeInsp = Session("CodeInsp")
        SearchForInsp = Session("SearchForInsp")
        '=================================================

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub addAttributesInsp()
        txtCode1Insp.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCode1Insp').value,event)")
    End Sub
    Private Sub GridBindInsp()
        dgMonitorInspStatusList.DataSource = mInstallCompMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
        SetGridInsp()
    End Sub
    Private Sub ControlVisibilityInsp()
        btnAddTopInsp.Visible = (mInstallCompMonitorInspStatusList.Count > 5)
        btnPrintTopInsp.Visible = (mInstallCompMonitorInspStatusList.Count > 5)
        btnCloseTopInsp.Visible = (mInstallCompMonitorInspStatusList.Count > 5)
        btnPrintInsp.Enabled = (Not mInstallCompMonitorInspStatusList Is Nothing And mInstallCompMonitorInspStatusList.Count <> 0)
        dgMonitorInspStatusList.Columns(20).Visible = IIf(chkApplicableInspection.Checked, False, True)
    End Sub
    Private Sub DataFieldBindInsp()
        mInstallCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(mCompStatus.InstalledOn.ToString, mAssemblyStatus.MachineID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableInspection.Checked)
        dgMonitorInspStatusList.DataSource = mInstallCompMonitorInspStatusList
        Session("mInstallCompMonitorInspStatusList") = mInstallCompMonitorInspStatusList
        mPartMonitorInspTypeList = PartMonitorInspTypeList.GetPartMonitorInspTypeList("(ALL)")
        Session("mPartMonitorInspTypeList") = mPartMonitorInspTypeList
        cmbSearchForInsp.DataSource = mPartMonitorInspTypeList
        'DataBind()
        dgMonitorInspStatusList.DataBind()
        cmbSearchForInsp.DataBind()
        chkApplicableInspection.Checked = False
    End Sub
    Private Sub NewRecordInsp()
        Session("TabIndex") = TbContInst.ActiveTabIndex
        mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.InstalledOn.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mAssemblyStatus.HourType)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then

            'Changed By Utkarsh On 27-Jul-2011 For All19072011
            MarkLog(Util.Action.[New], "Install Component Insp Status", User.Identity.Name & " is not Authorized User to add new ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End

            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfInstallCompMonitorInspStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If


        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Install Component Insp Status", "", Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
        'End

        'Code added By Deven on 1/4/2008
        'Response.Redirect("wfCompMonitorInspStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallCompMonitorInspStatusList.aspx")
        Session("mInstallCompMonitorInspStatusList") = mInstallCompMonitorInspStatusList

        'Code added By Deven on 25/09/2009
        Dim mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
        mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
        '----------------------------------

        ' Response.Redirect("wfPartMonitorInspList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage5=wfInstallComp_AJAX.aspx")
        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
        If mComponentMaintananceListCount Is Nothing Or mComponentMaintananceListCount.MaintenanceInspListCount = 0 Then

            Dim mPartMonitorInsp As PartMonitorInsp
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mPartMonitorInsp = PartMonitorInsp.NewPartMonitorInsp(ID, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType, ID)
            Session.Remove("mPartMonitorInspList")
            Session("mPartMonitorInsp") = mPartMonitorInsp

            MarkLog(Util.Action.[New], "Part Insp", "", Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)

            'Response.Redirect("wfPartMonitorInsp_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage6=wfInstallComp_AJAX.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow();", True)

        ElseIf mComponentMaintananceListCount.MaintenanceInspListCount > 0 Then
            'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterListWindow", "OpenSeriviceMasterListWindow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorInspList_Ajax.aspx?GChildPage2=&GChildPage4=wfInstallComp_AJAX.aspx & &GChildPage5=wfInstallComp_AJAX.aspx');", True)
        End If
        '------------------------------------------------
    End Sub
    Private Sub FindNowInsp()
        'Added by Saylee on 9th-Jan-2007 to keep Searching criteria as it is
        Session("LookInInsp") = cmbLookInInsp.SelectedIndex
        Session("TextForInsp") = txtForInsp.Text
        Session("CodeInsp") = txtCode1Insp.Text
        Session("SearchForInsp") = cmbSearchForInsp.SelectedIndex
        '=================================================================
        Select Case cmbLookInInsp.SelectedIndex
            Case 0, -1  'All
                mInstallCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 1  'ATA Code
                mInstallCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, Val(txtCode1Insp.Text), , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 2  'Insp Type ID
                mInstallCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , CInt(cmbSearchForInsp.SelectedValue), , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 3 ' Work Order No.
                mInstallCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , txtForInsp.Text.Trim, , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 4  'Show In C of A
                mInstallCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , True, mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableInspection.Checked)
        End Select

        Session("mInstallCompMonitorInspStatusList") = mInstallCompMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mInstallCompMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
    End Sub
    Private Sub SetcontrolInsp()
        'Fuction added by Saylee on 9th-Jan-2008 to keep Searching criteia as it is
        cmbLookInInsp.SelectedValue = LookInInsp 'IIf(LookIn = "", "(All)", LookInInsp)
        txtForInsp.Text = TextForInsp
        txtCode1Insp.Text = CodeInsp
        cmbSearchForInsp.SelectedIndex = IIf(SearchForInsp Is Nothing, 0, SearchForInsp) 'SearchForInsp 'IIf(SearchFor = "", "(All)", SearchFor)
        DisplayControlsInsp(cmbLookInInsp.SelectedIndex)
        FindNowInsp()
    End Sub
    Private Sub DisplayControlsInsp(ByVal Index As Integer)
        'Commented and Added by Saylee on 9th-Jan-2008 to keep Searching criteia as it is
        'txtFor.Text = ""
        'txtCode.Text = ""
        txtForInsp.Text = IIf(Index = 3, txtForInsp.Text, "")
        txtCode1Insp.Text = IIf(Index = 1, txtCode1Insp.Text, "")
        '=========================================================
        txtCode1Insp.Visible = IIf(Index = 1, True, False)
        txtForInsp.Visible = IIf(Index = 3, True, False)
        lblForInsp.Visible = (Index > 0 And Index <> 4)
        cmbSearchForInsp.Visible = (Index = 2)
        If cmbLookInInsp.Enabled = True Then
            setFocus(cmbLookInInsp)
        End If
    End Sub
    Private Sub SetPageInsp()
        If Not mCompStatus.IsNew Then
            lblTitle.Text = "Installation Information of the Component [Part:" & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & "]"
        Else
            lblTitle.Text = "Installation Information of the Component [New]"
        End If
        'CNDC
        'lblInspInfo.Text = "List of all the Inspection on the Component as of " & mCompStatus.InstalledOnFormatted & ". All the values of all the Insps will be as of " & mAssemblyStatus.InstalledOnFormatted
        lblInspInfo.Text = "List of all the Inspections on the Component and values of all the Insps will be as of " & mCompStatus.InstalledOnFormatted
        lblCaptionInsp.Text = "List of Component Insp Status: " & mInstallCompMonitorInspStatusList.Count & " Record(s) found."

    End Sub
    Private Sub SetGridInsp()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorInspStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorInspStatusList.Rows.Item(j).Cells(25).Text, Boolean)
            If B = False Then
                'lb = CType(dgMonitorInspStatusList.Rows.Item(j).Cells(21).FindControl("lnkView"), LinkButton)
                'lb.Enabled = False
                dgMonitorInspStatusList.Rows.Item(j).Cells(24).Enabled = False
            End If

        Next
    End Sub
    Private Sub DeleteInspRecord(ByVal Index As Integer)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "Do you want to Delete the record?", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfInstallCompMonitorInspStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "Do you want to Delete the record?", MsgBoxStyle.YesNo, "Delete")
        mInstallCompMonitorInspStatusList.CurrentIndex = Index
        Session("mInstallCompMonitorInspStatusList") = mInstallCompMonitorInspStatusList
    End Sub
    Private Sub EditMasterRecordInsp(ByVal mMasterId As Guid, ByVal mId As Guid, ByVal Index As Integer)
        Dim CompMonitorInspStatusInfo As tmpComplyCompMonitorInspStatusList.tmpComplyCompMonitorInspStatusInfo = mInstallCompMonitorInspStatusList.Item(Index)
        REM: if selected record is Master record then master form is opened
        '    else entry form is opened
        If CompMonitorInspStatusInfo.IsMaster = True Then
            mCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mInstallCompMonitorInspStatusList.Item(Index).CompMonitorInspStatusID, mAssemblyStatus.ID, mCompStatus.ID, mAssemblyStatus.HourType)
            Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Else
            Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus
            mPrevCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mInstallCompMonitorInspStatusList.Item(Index).CompMonitorInspStatusID, mInstallCompMonitorInspStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorInspStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType, IsForSpareComp:=mSpareAssemblyComponent)
            mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatusFromEntry(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mAssemblyStatus.HourType, IsForSpareComp:=mSpareAssemblyComponent)
            Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
            Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
            Session("EnFrom") = 1 'EditRecord
            If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then

                'Added By Utkarsh On 27-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mId).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mId).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mId).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mId).MonitorInfo.Replace(Environment.NewLine, " ")
                MarkLog(Util.Action.Edit, "Install Component Insp Status", User.Identity.Name & " is not Authorized User to edit " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                'End
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                'msg.ReplacePage = "wfInstallCompMonitorInspStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
                'msg.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        ''***
        Dim mPartMonitorInsp As PartMonitorInsp
        mPartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(mMasterId, mAssemblyStatus.HourType)
        ''mCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mMachine.HourType)
        ''Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mMachine") = mMachine
        Session("mPartMonitorInsp") = mPartMonitorInsp
        'RemoveSession()

        'Added By Utkarsh On 27-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mId).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mId).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mId).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mId).MonitorInfo.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.Edit, "Install Component Insp Status", MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        'End

        ' Response.Redirect("wfPartMonitorInsp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage6=wfInstallComp_AJAX.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow();", True)
    End Sub
    Private Sub MessageBoxResultInsp()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0
        Dim mCompMonitorInspStatusID As Guid

        If Result1 > 0 Then
            GetSession()
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mCompMonitorInspStatusID = mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).CompMonitorInspStatusID
                            MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).CompMonitorInspStatusID, 8)
                            '=============================
                            'Added By Vikrant On 25-Nov-2014
                            If mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).CompMonitorInspStatusID)
                            End If

                            CompMonitorInspStatus.DeleteCompMonitorInspStatus(mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).CompMonitorInspStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mFileAttach = Nothing
                            Session("mMachineMaintenance") = mMachineMaintenance
                            ' Response.Redirect("wfInstallCompMonitorInspStatusList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                            DataFieldBindInsp()
                            SetPageInsp()
                            SetGridInsp()
                            ControlVisibility()

                            upnlCaptionInsp.Update()
                            upnlInspGrid.Update()
                            upnlInspButtons.Update()
                            upnlInspInfo.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                ' MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                                MarkLog(Util.Action.Delete, "Install Component Insp Status", "Can't delete : " & MaintDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 27-Jul-2011 For All19072011
                                ' MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mInstallCompMonitorInspStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                                MarkLog(Util.Action.Delete, "Install Component Insp Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorInspStatusID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBindInsp()
                    SetPageInsp()
                    SetGridInsp()
                    ControlVisibilityInsp()

                    upnlInspGrid.Update()
                    upnlInspButtons.Update()
                    upnlInspInfo.Update()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBindInsp()
                    SetPageInsp()
                    SetGridInsp()
                    ControlVisibility()

                    upnlInspGrid.Update()
                    upnlInspButtons.Update()
                    upnlInspInfo.Update()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
#End Region

#Region " Insp Events "
    Private Sub btnAddInsp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddInsp.Click, btnAddTopInsp.Click
        Session("TabIndex") = TbContInst.ActiveTabIndex
        NewRecordInsp()
    End Sub
    Private Sub cmbLookInInsp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInInsp.SelectedIndexChanged
        DisplayControlsInsp(cmbLookInInsp.SelectedIndex)
    End Sub
    Private Sub hdnBtnInspMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnInspMaster.Click
        DataFieldBindInsp()
        SetPageInsp()
        SetGridInsp()
        upnlInspGrid.Update()
        upnlInspInfo.Update()
        upnlCaptionInsp.Update()
    End Sub
    Private Sub dgMonitorInspStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Session("TabIndex") = TbContInst.ActiveTabIndex
        Select Case e.CommandName
            Case "Comply"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                Dim mID = mInstallCompMonitorInspStatusList(Index).CompMonitorInspStatusID
                If (Not User.IsInRole("ComponentInstallationNew")) Then
                    MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Comply, "Install Component Insp Status", User.Identity.Name & " is not Authorized User to comply " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus
                mPrevCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mInstallCompMonitorInspStatusList.Item(Index).CompMonitorInspStatusID, mInstallCompMonitorInspStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorInspStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType, IsForSpareComp:=mCompStatus.IsSpareComp)
                REM: Complance of one time monitoring is done only once.
                If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted = True Then
                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, mCompStatus.InstalledOn.ToString, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mAssemblyStatus.HourType)
                Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                Session("EnFrom") = 0 'NewRecord

                'Added by Saylee on 17-Jun-2009
                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevCompMonitorInspStatus.ID)
                Session("mBoardInfo") = mBoardInfo
                '**************************************

                'Added By Vikrant On 25-Nov-2014
                Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorInspStatus.ID) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach
                'End

                'Added By Utkarsh On 27-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                MarkLog(Util.Action.Comply, "Install Component Insp Status", MaintDetail, Util.ErrorType.NoError, mID, EventLogID)
                'End

                'Response.Redirect("wfComplyCompMonitorInspStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallCompMonitorInspStatusList.aspx")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorInspStatus_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                Dim mID = mInstallCompMonitorInspStatusList(Index).CompMonitorInspStatusID
                Dim CompMonitorInspStatusInfo As tmpComplyCompMonitorInspStatusList.tmpComplyCompMonitorInspStatusInfo = mInstallCompMonitorInspStatusList.Item(Index)
                REM: if selected record is Master record then master form is opened
                '    else entry form is opened
                If CompMonitorInspStatusInfo.IsMaster = True Then
                    mCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mInstallCompMonitorInspStatusList.Item(Index).CompMonitorInspStatusID, mAssemblyStatus.ID, mCompStatus.ID, mAssemblyStatus.HourType, True)
                    Session("mCompMonitorInspStatus") = mCompMonitorInspStatus

                    'Added by Saylee on 17-Feb-2011
                    Dim mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
                    mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly, IsSpareComponent:=mCompStatus.IsSpareComp)
                    Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList

                    'Added By Vikrant On 25-Nov-2014
                    If mCompMonitorInspStatus.IsAttachmentAdded Then
                        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorInspStatus.ID) 'Sort = 1 - Installation
                        Session("mFileAttach") = mFileAttach
                    Else
                        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorInspStatus.ID)
                        Session("mFileAttach") = mFileAttach
                    End If
                    'End

                    Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx")
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorInspStatus.aspx?BackPage='& Request.QueryString('BackPage') & '&ChildPage=' & Request.QueryString('ChildPage') & '&GChildPage=' & Request.QueryString('GChildPage') & '&GChildPage1=' & Request.QueryString('GChildPage1') & '&GChildPage2=' & Request.QueryString('GChildPage2') & '&GChildPage3=' & Request.QueryString('GChildPage3') & '&GChildPage4=wfInstallCompMonitorInspStatusList.aspx'); ", True)
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame(wfCompMonitorInspStatus.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
                Else
                    Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus
                    mPrevCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mInstallCompMonitorInspStatusList.Item(Index).CompMonitorInspStatusID, mInstallCompMonitorInspStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorInspStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType, IsForSpareComp:=mSpareAssemblyComponent)
                    mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatusFromEntry(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mAssemblyStatus.HourType, True, IsForSpareComp:=mSpareAssemblyComponent)
                    Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
                    Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                    Session("EnFrom") = 1 'EditRecord
                    If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then

                        'Added By Utkarsh On 27-Jul-2011 For All19072011
                        MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                        MarkLog(Util.Action.Edit, "Install Component Insp Status", User.Identity.Name & " is not Authorized User to edit " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                        'End
                        MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                    'Added By Utkarsh On 27-Jul-2011 For All19072011
                    MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Edit, "Install Component Insp Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
                    'End

                    'Added By Vikrant On 25-Nov-2014
                    If mCompMonitorInspStatus.IsAttachmentAdded Then
                        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorInspStatus.ID) 'Sort = 1 - Installation
                        Session("mFileAttach") = mFileAttach
                    Else
                        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorInspStatus.ID)
                        Session("mFileAttach") = mFileAttach
                    End If
                    'End

                    'Response.Redirect("wfComplyCompMonitorInspStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorInspStatus_AJAX.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
                End If
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                Dim mID = mInstallCompMonitorInspStatusList(Index).CompMonitorInspStatusID
                GridBindInsp()
                SetGridInsp()
                ControlVisibility()
                If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
                    'Added By Utkarsh On 27-Jul-2011 For All19072011
                    MaintDetail = "Reg No. : " + mInstallCompMonitorInspStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorInspStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorInspStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorInspStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Delete, "Install Component Insp Status", User.Identity.Name & " is not Authorized User to delete " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteInspRecord(Index)
            Case "EditMaster"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                Dim mID = mInstallCompMonitorInspStatusList(Index).CompMonitorInspStatusID
                Dim mMasterId As Guid = mInstallCompMonitorInspStatusList(Index).PartMonitorInspID
                Session("EditMasterRecord") = "True"
                EditMasterRecordInsp(mMasterId, mID, Index)
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mInstallCompMonitorInspStatusList(Index).ID)
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgMonitorInspStatusList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorInspStatusList.Sorting
        mInstallCompMonitorInspStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInstallCompMonitorInspStatusList") = mInstallCompMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mInstallCompMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
        SetGridInsp()
    End Sub
    Private Sub btnFindNowInsp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowInsp.Click
        FindNowInsp()
        SetPageInsp()
        SetGridInsp()
        ControlVisibilityInsp()
        upnlInspGrid.Update()
        upnlTitle.Update()
        upnlInspInfo.Update()
        upnlCaption.Update()
        upnlInspButtons.Update()
        upnlInspButtons.Update()
    End Sub
    Private Sub btnPrintInsp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintInsp.Click, btnPrintTopInsp.Click

        If (Not User.IsInRole("ComponentInstallationPrint")) Then
            'Commented By Utkarsh On 27-Jul-2011 For All19072011
            '    MarkLog(Util.Action.Print, "InstallCompMonitorInspStatusList", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'End

            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfInstallCompMonitorInspStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstallComponentMonitor
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList
        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If
        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of the Component", "ATA Chapter :",
       mCompStatus.ATAChapter, , , , , , , , , , , , , , , , , "Values at Installation",
       "Period", "Component", , "Assembly"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of the Component", "ATA Chapter :",
                      mCompStatus.ATAChapter, , , , , , , , , , , , , , , , , "Values at Installation",
                      "", "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Part No. :",
                           mCompStatus.PartName, , , , , , , , , , , , , , , , , ,
                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Part No. :",
                          mCompStatus.PartName, , , , , , , , , , , , , , , , , ,
                          "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Description :",
                                           mCompStatus.Description, , , , , , , , , , , , , , , , , ,
                                          CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Description :",
                                          mCompStatus.Description, , , , , , , , , , , , , , , , , ,
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :",
                                                        mCompStatus.SerialNo, , , , , , , , , , , , , , , , , ,
                                                          CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :",
                                                          mCompStatus.SerialNo, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))

                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Code :",
                                                         mCompStatus.Comp.Code, , , , , , , , , , , , , , , , , ,
                                                          CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Code :",
                                                          mCompStatus.Comp.Code, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))

                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mCompStatus.Position, , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                         mCompStatus.Position, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "",
                   "", , , , , , , , , , , , , , , , , ,
                    CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
            End If
        Next
        'For Install Component Inspection List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , lblInspInfo.Text))
        'For Install Component Inspection List
        'ReportDetails.Add(New rptStatus(, 2, , _
        '      , , , dgMonitorInspStatusList.Columns.Item(1).HeaderText, , dgMonitorInspStatusList.Columns.Item(2).HeaderText, dgMonitorInspStatusList.Columns.Item(3).HeaderText, _
        '      dgMonitorInspStatusList.Columns.Item(4).HeaderText, dgMonitorInspStatusList.Columns.Item(5).HeaderText, dgMonitorInspStatusList.Columns.Item(6).HeaderText, _
        '      dgMonitorInspStatusList.Columns.Item(7).HeaderText, , dgMonitorInspStatusList.Columns.Item(8).HeaderText, dgMonitorInspStatusList.Columns.Item(9).HeaderText, dgMonitorInspStatusList.Columns.Item(10).HeaderText, _
        '      dgMonitorInspStatusList.Columns.Item(11).HeaderText, dgMonitorInspStatusList.Columns.Item(13).HeaderText, dgMonitorInspStatusList.Columns.Item(14).HeaderText, _
        '      , , , dgMonitorInspStatusList.Columns.Item(15).HeaderText, , ))
        ReportDetails.Add(New rptStatus(, 2, ,
              , , , dgMonitorInspStatusList.Columns.Item(4).HeaderText, , dgMonitorInspStatusList.Columns.Item(5).HeaderText, dgMonitorInspStatusList.Columns.Item(6).HeaderText,
              dgMonitorInspStatusList.Columns.Item(7).HeaderText, dgMonitorInspStatusList.Columns.Item(8).HeaderText, dgMonitorInspStatusList.Columns.Item(9).HeaderText,
              dgMonitorInspStatusList.Columns.Item(10).HeaderText, , dgMonitorInspStatusList.Columns.Item(11).HeaderText, dgMonitorInspStatusList.Columns.Item(13).HeaderText, dgMonitorInspStatusList.Columns.Item(14).HeaderText,
              dgMonitorInspStatusList.Columns.Item(15).HeaderText, dgMonitorInspStatusList.Columns.Item(16).HeaderText, dgMonitorInspStatusList.Columns.Item(17).HeaderText,
              , , , dgMonitorInspStatusList.Columns.Item(18).HeaderText, , ))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mInstallCompMonitorInspStatusList.Count
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            Dim str(14) As String
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
            str(10) = ""
            str(11) = ""
            str(12) = ""
            str(13) = ""
            str(14) = ""
            '  If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            '   If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            '   If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(4).Text <> "&nbsp;" Then str(0) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(5).Text <> "&nbsp;" Then str(1) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(6).Text <> "&nbsp;" Then str(2) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(7).Text <> "&nbsp;" Then str(3) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(8).Text <> "&nbsp;" Then str(4) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(9).Text <> "&nbsp;" Then str(5) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(10).Text <> "&nbsp;" Then str(6) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(11).Text <> "&nbsp;" Then str(7) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(11).Text.Replace("<BR>", vbCrLf)
            ' If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(12).Text <> "&nbsp;" Then str(8) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(12).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(13).Text <> "&nbsp;" Then str(9) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(14).Text <> "&nbsp;" Then str(10) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(15).Text <> "&nbsp;" Then str(11) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(16).Text <> "&nbsp;" Then str(12) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(17).Text <> "&nbsp;" Then str(13) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(17).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells.Item(18).Text <> "&nbsp;" Then str(14) = Me.dgMonitorInspStatusList.Rows(m).Cells.Item(18).Text.Replace("<BR>", vbCrLf)


            ReportDetails.Add(New rptStatus(, 3, ,
                 , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), , str(7), str(9), str(10),
                      str(11), str(12), str(13), , , , str(14), , ))
        Next
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite, "Component Inspection Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        If m = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            'msg1.ReplacePage = "wfInstallCompMonitorInspStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
            'msg1.Show()
            Exit Sub
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        '   MarkLog(Util.Action.Print, "InstallCompMonitorServiceStatusList", "Component Service Status List Report", Util.ErrorType.HandledError, mInstallCompStatus.ID)

        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

    Private Sub btnCloseInsp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseInsp.Click, btnCloseTopInsp.Click
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Install Component Insp Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        SetSession()
        'RemoveSession()
        mInstallCompMonitorInspStatusList = Nothing
        mPartMonitorInspTypeList = Nothing
        Session.Remove("mInstallCompMonitorInspStatusList")

        Session.Remove("mFileAttach")
        Session("FromInstallCompMonitorInspStatusList") = True
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
#End Region

#End Region

#Region " Mod Tab "

#Region " Variable Declarations "
    Public mInstallCompMonitorModStatusList As tmpComplyCompMonitorModStatusList

    Public mCompMonitorModStatus As CompMonitorModStatus
    Public mPartMonitorModTypeList As PartMonitorModTypeList

    'Public mCompMonitorModStatusList As tmpComplyCompMonitorModStatusList
    Public LookInMod, TextForMod, CodeMod, SearchForMod As String

#End Region

#Region " Business Methods "
    Private Sub GetSessionMod()
        mInstallCompMonitorModStatusList = CType(Session("mInstallCompMonitorModStatusList"), tmpComplyCompMonitorModStatusList)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mRemovedCompStatus = CType(Session("mRemovedCompStatus"), CompStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mPartMonitorModTypeList = CType(Session("mPartMonitorModTypeList"), PartMonitorModTypeList)
        mMachine = CType(Session("mMachine"), Machine)
        '===Added by Saylee on 9th-Jan-2008===============
        LookInMod = Session("LookInMod")
        TextForMod = Session("TextForMod")
        CodeMod = Session("CodeMod")
        SearchForMod = Session("SearchForMod")
        '=================================================

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub addAttributesMod()
        txtCode1Mod.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCode1Mod').value,event)")
    End Sub
    Private Sub GridBindMod()
        dgMonitorModStatusList.DataSource = mInstallCompMonitorModStatusList
        dgMonitorModStatusList.DataBind()
        SetGridMod()
    End Sub
    Private Sub ControlVisibilityMod()
        btnAddTopMod.Visible = (mInstallCompMonitorModStatusList.Count > 5)
        btnPrintTopMod.Visible = (mInstallCompMonitorModStatusList.Count > 5)
        btnCloseTopMod.Visible = (mInstallCompMonitorModStatusList.Count > 5)
        btnPrintMod.Enabled = (Not mInstallCompMonitorModStatusList Is Nothing And mInstallCompMonitorModStatusList.Count <> 0)
        dgMonitorModStatusList.Columns(21).Visible = IIf(chkApplicableDirective.Checked, False, True)
    End Sub
    Private Sub DataFieldBindMod()
        mInstallCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(mCompStatus.InstalledOn.ToString, mAssemblyStatus.MachineID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableDirective.Checked)
        dgMonitorModStatusList.DataSource = mInstallCompMonitorModStatusList
        Session("mInstallCompMonitorModStatusList") = mInstallCompMonitorModStatusList
        mPartMonitorModTypeList = PartMonitorModTypeList.GetPartMonitorModTypeList("(ALL)")
        Session("mPartMonitorModTypeList") = mPartMonitorModTypeList
        cmbSearchForMod.DataSource = mPartMonitorModTypeList
        'DataBind()
        dgMonitorModStatusList.DataBind()
        cmbSearchForMod.DataBind()
        chkApplicableDirective.Checked = False
    End Sub
    Private Sub NewRecordMod()
        Session("TabIndex") = TbContInst.ActiveTabIndex
        mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.InstalledOn.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mAssemblyStatus.HourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then

            'Changed By Utkarsh On 27-Jul-2011 For All19072011
            MarkLog(Util.Action.[New], "Install Component Mod Status", User.Identity.Name & " is not Authorized User to add new ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End

            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfInstallCompMonitorModStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If


        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Install Component Mod Status", "", Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
        'End

        'Code added By Deven on 1/4/2008
        'Response.Redirect("wfCompMonitorModStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallCompMonitorModStatusList.aspx")
        Session("mInstallCompMonitorModStatusList") = mInstallCompMonitorModStatusList

        'Code added By Deven on 25/09/2009
        Dim mCompMonitorModStatusList As tmpCompMonitorModStatusList
        mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString)
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
        '----------------------------------

        'Response.Redirect("wfPartMonitorModList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage5=wfInstallComp_AJAX.aspx")
        Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)
        If mComponentMaintananceListCount Is Nothing Or mComponentMaintananceListCount.MaintenanceModListCount = 0 Then

            Dim mPartMonitorMod As PartMonitorMod
            mPartMonitorMod = PartMonitorMod.NewPartMonitorMod(Guid.NewGuid, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType)
            Session.Remove("mPartMonitorModList")
            Session("mPartMonitorMod") = mPartMonitorMod

            MarkLog(Util.Action.[New], "Part Modification", "", Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)

            'Response.Redirect("wfPartMonitorMod_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage6=wfInstallComp_AJAX.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow();", True)

        ElseIf mComponentMaintananceListCount.MaintenanceModListCount > 0 Then
            'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterListWindow", "OpenSeriviceMasterListWindow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfPartMonitorModList_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx & &GChildPage5=wfInstallComp_AJAX.aspx');", True)
        End If

        '------------------------------------------------
    End Sub
    Private Sub FindNowMod()
        'Added by Saylee on 9th-Jan-2007 to keep Searching criteria as it is
        Session("LookInMod") = cmbLookInMod.SelectedIndex
        Session("TextForMod") = txtForMod.Text
        Session("CodeMod") = txtCode1Mod.Text
        Session("SearchForMod") = cmbSearchForMod.SelectedIndex
        '=================================================================
        Select Case cmbLookInMod.SelectedIndex
            Case 0, -1  'All
                mInstallCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 1  'ATA Code
                mInstallCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, Val(txtCode1Mod.Text), , , , , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 2  'Mod Type ID
                mInstallCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , CInt(cmbSearchForMod.SelectedValue), , , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 3 ' Work Order No.
                mInstallCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , txtForMod.Text.Trim, , , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 4  'Show In C of A
                mInstallCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(mCompStatus.InstalledOn, mMachine.ID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, , , , , , True, , mCompStatus.CompID.ToString, ShowNotApplicable:=chkApplicableDirective.Checked)
        End Select

        Session("mInstallCompMonitorModStatusList") = mInstallCompMonitorModStatusList
        dgMonitorModStatusList.DataSource = mInstallCompMonitorModStatusList
        dgMonitorModStatusList.DataBind()
    End Sub
    Private Sub SetcontrolMod()
        'Fuction added by Saylee on 9th-Jan-2008 to keep Searching criteia as it is
        cmbLookInMod.SelectedValue = LookInMod 'IIf(LookIn = "", "(All)", LookInMod)
        txtForMod.Text = TextForMod
        txtCode1Mod.Text = CodeMod
        cmbSearchForMod.SelectedIndex = IIf(SearchForMod Is Nothing, 0, SearchForMod) 'IIf(SearchFor = "", "(All)", SearchFor)
        DisplayControlsMod(cmbLookInMod.SelectedIndex)
        FindNowMod()
    End Sub
    Private Sub DisplayControlsMod(ByVal Index As Integer)
        'Commented and Added by Saylee on 9th-Jan-2008 to keep Searching criteia as it is
        'txtFor.Text = ""
        'txtCode.Text = ""
        txtForMod.Text = IIf(Index = 3, txtForMod.Text, "")
        txtCode1Mod.Text = IIf(Index = 1, txtCode1Mod.Text, "")
        '=========================================================
        txtCode1Mod.Visible = IIf(Index = 1, True, False)
        txtForMod.Visible = IIf(Index = 3, True, False)
        lblForMod.Visible = (Index > 0 And Index <> 4)
        cmbSearchForMod.Visible = (Index = 2)
        If cmbLookInMod.Enabled = True Then
            setFocus(cmbLookInMod)
        End If
    End Sub
    Private Sub SetPageMod()
        If Not mCompStatus.IsNew Then
            lblTitle.Text = "Installation Information of the Component [Part:" & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo & "]"
        Else
            lblTitle.Text = "Installation Information of the Component [New]"
        End If
        'CNDC

        ' lblModInfo.Text = "List of all the Modification on the Component as of " & mCompStatus.InstalledOnFormatted & ". All the values of all the Mods will be as of " & mAssemblyStatus.InstalledOnFormatted
        lblModInfo.Text = "List of all the Modification on the Component and values of all the Mods will be as of " & mCompStatus.InstalledOnFormatted
        lblCaptionMod.Text = "List of Component Mod Status: " & mInstallCompMonitorModStatusList.Count & " Record(s) found."

    End Sub
    Private Sub SetGridMod()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorModStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorModStatusList.Rows.Item(j).Cells(26).Text, Boolean)
            If B = False Then
                'lb = CType(dgMonitorModStatusList.Rows.Item(j).Cells(21).FindControl("lnkView"), LinkButton)
                'lb.Enabled = False
                dgMonitorModStatusList.Rows.Item(j).Cells(25).Enabled = False
            End If

        Next
    End Sub
    Private Sub DeleteModRecord(ByVal Index As Integer)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "Do you want to Delete the record?", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfInstallCompMonitorModStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "Do you want to Delete the record?", MsgBoxStyle.YesNo, "Delete")
        mInstallCompMonitorModStatusList.CurrentIndex = Index
        Session("mInstallCompMonitorModStatusList") = mInstallCompMonitorModStatusList
    End Sub
    Private Sub EditMasterRecordMod(ByVal mMasterId As Guid, ByVal mId As Guid, ByVal Index As Integer)
        Dim CompMonitorModStatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo = mInstallCompMonitorModStatusList.Item(Index)
        REM: if selected record is Master record then master form is opened
        '    else entry form is opened
        If CompMonitorModStatusInfo.IsMaster = True Then
            mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mInstallCompMonitorModStatusList.Item(Index).CompMonitorModStatusID, mAssemblyStatus.ID, mCompStatus.ID, mAssemblyStatus.HourType)
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Else
            Dim mPrevCompMonitorModStatus As CompMonitorModStatus
            mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mInstallCompMonitorModStatusList.Item(Index).CompMonitorModStatusID, mInstallCompMonitorModStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorModStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType)
            mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mAssemblyStatus.HourType)
            Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            Session("EnFrom") = 1 'EditRecord
            If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then

                'Added By Utkarsh On 27-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mId).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mId).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mId).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mId).MonitorInfo.Replace(Environment.NewLine, " ")
                MarkLog(Util.Action.Edit, "Install Component Mod Status", User.Identity.Name & " is not Authorized User to edit " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                'End
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                'msg.ReplacePage = "wfInstallCompMonitorModStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
                'msg.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        ''***
        Dim mPartMonitorMod As PartMonitorMod
        mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mMasterId, mAssemblyStatus.HourType)
        ''mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mId, mAssemblyStatus.ID, mCompStatus.ID, mMachine.HourType)
        ''Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mMachine") = mMachine
        Session("mPartMonitorMod") = mPartMonitorMod
        'RemoveSession()

        'Added By Utkarsh On 27-Jul-2011 For All19072011
        MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mId).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mId).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mId).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mId).MonitorInfo.Replace(Environment.NewLine, " ")
        MarkLog(Util.Action.Edit, "Install Component Mod Status", MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        'End

        ' Response.Redirect("wfPartMonitorMod.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx" & "&GChildPage6=wfInstallComp_AJAX.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow();", True)
    End Sub
    Private Sub MessageBoxResultMod()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0
        Dim mCompMonitorModStatusID As Guid

        If Result1 > 0 Then
            GetSession()
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mCompMonitorModStatusID = mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID
                            MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID, 8)
                            '=============================
                            'Added By Vikrant On 25-Nov-2014
                            If mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID)
                            End If

                            CompMonitorModStatus.DeleteCompMonitorModStatus(mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).CompMonitorModStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mFileAttach = Nothing
                            Session("mMachineMaintenance") = mMachineMaintenance
                            ' Response.Redirect("wfInstallCompMonitorModStatusList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                            DataFieldBindMod()
                            SetPageMod()
                            SetGridMod()
                            ControlVisibility()

                            upnlCaptionMod.Update()
                            upnlModGrid.Update()
                            upnlModButtons.Update()
                            upnlModInfo.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                                MarkLog(Util.Action.Delete, "Install Component Mod Status", "Can't delete : " & MaintDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 27-Jul-2011 For All19072011
                                'MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mInstallCompMonitorModStatusList.CurrentIndex).MonitorInfo.Replace(Environment.NewLine, " ")
                                MarkLog(Util.Action.Delete, "Install Component Mod Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorModStatusID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    DataFieldBindMod()
                    SetPageMod()
                    SetGridMod()
                    ControlVisibility()

                    upnlModGrid.Update()
                    upnlModButtons.Update()
                    upnlModInfo.Update()
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBindMod()
                    SetPageMod()
                    SetGridMod()
                    ControlVisibility()

                    upnlModGrid.Update()
                    upnlModButtons.Update()
                    upnlModInfo.Update()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBindMod()
                    SetPageMod()
                    SetGridMod()
                    ControlVisibility()

                    upnlModGrid.Update()
                    upnlModButtons.Update()
                    upnlModInfo.Update()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
#End Region

#Region " Mod Events "
    Private Sub btnAddMod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMod.Click, btnAddTopMod.Click
        Session("TabIndex") = TbContInst.ActiveTabIndex
        NewRecordMod()
    End Sub
    Private Sub cmbLookInMod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInMod.SelectedIndexChanged
        DisplayControlsMod(cmbLookInMod.SelectedIndex)
    End Sub
    Private Sub hdnBtnModMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnModMaster.Click
        DataFieldBindMod()
        SetPageMod()
        SetGridMod()
        upnlModGrid.Update()
        upnlModInfo.Update()
        upnlCaptionMod.Update()
    End Sub
    Private Sub dgMonitorModStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorModStatusList.RowCommand
        Session("TabIndex") = TbContInst.ActiveTabIndex
        Select Case e.CommandName
            Case "Comply"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                Dim mID = mInstallCompMonitorModStatusList(Index).CompMonitorModStatusID
                If (Not User.IsInRole("ComponentInstallationNew")) Then
                    MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Comply, "Install Component Mod Status", User.Identity.Name & " is not Authorized User to comply " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mPrevCompMonitorModStatus As CompMonitorModStatus
                mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mInstallCompMonitorModStatusList.Item(Index).CompMonitorModStatusID, mInstallCompMonitorModStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorModStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType)
                REM: Complance of one time monitoring is done only once.
                If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted = True Then
                    MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, mCompStatus.InstalledOn.ToString, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mAssemblyStatus.HourType)
                Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                Session("EnFrom") = 0 'NewRecord

                'Added by Saylee on 17-Jun-2009
                mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevCompMonitorModStatus.ID)
                Session("mBoardInfo") = mBoardInfo
                '**************************************

                'Added By Vikrant On 25-Nov-2014
                Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach
                'End

                'Added By Utkarsh On 27-Jul-2011 For All19072011
                MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                MarkLog(Util.Action.Comply, "Install Component Mod Status", MaintDetail, Util.ErrorType.NoError, mID, EventLogID)
                'End

                'Response.Redirect("wfComplyCompMonitorModStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallCompMonitorModStatusList.aspx")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorModStatus_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                Dim mID = mInstallCompMonitorModStatusList(Index).CompMonitorModStatusID
                Dim CompMonitorModStatusInfo As tmpComplyCompMonitorModStatusList.tmpComplyCompMonitorModStatusInfo = mInstallCompMonitorModStatusList.Item(Index)
                REM: if selected record is Master record then master form is opened
                '    else entry form is opened
                If CompMonitorModStatusInfo.IsMaster = True Then
                    mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mInstallCompMonitorModStatusList.Item(Index).CompMonitorModStatusID, mAssemblyStatus.ID, mCompStatus.ID, mAssemblyStatus.HourType, True)
                    Session("mCompMonitorModStatus") = mCompMonitorModStatus

                    'Added by Saylee on 17-Feb-2011
                    Dim mCompMonitorModStatusList As tmpCompMonitorModStatusList
                    mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString, mAssemblyStatus.ID.ToString)
                    Session("mCompMonitorModStatusList") = mCompMonitorModStatusList

                    'Added By Vikrant On 25-Nov-2014
                    If mCompMonitorModStatus.IsAttachmentAdded Then
                        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID) 'Sort = 1 - Installation
                        Session("mFileAttach") = mFileAttach
                    Else
                        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID)
                        Session("mFileAttach") = mFileAttach
                    End If
                    'End

                    Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx")
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfCompMonitorModStatus.aspx?BackPage='& Request.QueryString('BackPage') & '&ChildPage=' & Request.QueryString('ChildPage') & '&GChildPage=' & Request.QueryString('GChildPage') & '&GChildPage1=' & Request.QueryString('GChildPage1') & '&GChildPage2=' & Request.QueryString('GChildPage2') & '&GChildPage3=' & Request.QueryString('GChildPage3') & '&GChildPage4=wfInstallCompMonitorModStatusList.aspx'); ", True)
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame(wfCompMonitorModStatus.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
                Else
                    Dim mPrevCompMonitorModStatus As CompMonitorModStatus
                    mPrevCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mInstallCompMonitorModStatusList.Item(Index).CompMonitorModStatusID, mInstallCompMonitorModStatusList.Item(Index).AssemblyStatusID, mInstallCompMonitorModStatusList.Item(Index).CompStatusID, mAssemblyStatus.HourType)
                    mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mAssemblyStatus.HourType, True)
                    Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
                    Session("mCompMonitorModStatus") = mCompMonitorModStatus
                    Session("EnFrom") = 1 'EditRecord
                    If (Not User.IsInRole("ComponentInstallationView") And Not User.IsInRole("ComponentInstallationEdit")) Then

                        'Added By Utkarsh On 27-Jul-2011 For All19072011
                        MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                        MarkLog(Util.Action.Edit, "Install Component Mod Status", User.Identity.Name & " is not Authorized User to edit " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                        'End
                        MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                    'Added By Utkarsh On 27-Jul-2011 For All19072011
                    MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Edit, "Install Component Mod Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
                    'End

                    'Added By Vikrant On 25-Nov-2014
                    If mCompMonitorModStatus.IsAttachmentAdded Then
                        Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID) 'Sort = 1 - Installation
                        Session("mFileAttach") = mFileAttach
                    Else
                        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID)
                        Session("mFileAttach") = mFileAttach
                    End If
                    'End

                    ' Response.Redirect("wfComplyCompMonitorModStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallComp_AJAX.aspx")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorModStatus_AJAX.aspx?GChildPage4=wfInstallComp_AJAX.aspx');", True)
                End If
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                Dim mID = mInstallCompMonitorModStatusList(Index).CompMonitorModStatusID
                GridBindMod()
                SetGridMod()
                ControlVisibility()
                If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
                    'Added By Utkarsh On 27-Jul-2011 For All19072011
                    MaintDetail = "Reg No. : " + mInstallCompMonitorModStatusList(mID).MachineInfo & " Assembly Info : " & mInstallCompMonitorModStatusList(mID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mInstallCompMonitorModStatusList(mID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstallCompMonitorModStatusList(mID).MonitorInfo.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Delete, "Install Component Mod Status", User.Identity.Name & " is not Authorized User to delete " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteModRecord(Index)
            Case "EditMaster"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                Dim mID = mInstallCompMonitorModStatusList(Index).CompMonitorModStatusID
                Dim mMasterId As Guid = mInstallCompMonitorModStatusList(Index).PartMonitorModID
                Session("EditMasterRecord") = "True"
                EditMasterRecordMod(mMasterId, mID, Index)
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mInstallCompMonitorModStatusList(Index).ID)
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgMonitorModStatusList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorModStatusList.Sorting
        mInstallCompMonitorModStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInstallCompMonitorModStatusList") = mInstallCompMonitorModStatusList
        dgMonitorModStatusList.DataSource = mInstallCompMonitorModStatusList
        dgMonitorModStatusList.DataBind()
        SetGridMod()
    End Sub
    Private Sub btnFindNowMod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowMod.Click
        FindNowMod()
        SetPageMod()
        SetGridMod()
        ControlVisibilityMod()
        upnlModGrid.Update()
        upnlTitle.Update()
        upnlModInfo.Update()
        upnlCaptionMod.Update()
        upnlModButtons.Update()
        upnlModTopButtons.Update()
    End Sub
    Private Sub btnPrintMod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintMod.Click, btnPrintTopMod.Click

        If (Not User.IsInRole("ComponentInstallationPrint")) Then
            'Cmmented By Utkarsh On 27-Jul-2011 For All19072011
            '       MarkLog(Util.Action.Print, "InstallCompMonitorModStatusList", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'End
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfInstallCompMonitorModStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstallComponentMonitor
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of the Component", "ATA Chapter :",
       mCompStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation",
       "Period", "Component", , "Assembly"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Part and Serial No. of the Component", "ATA Chapter :",
                      mCompStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation",
                      "", "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Part No. :",
                           mCompStatus.PartName, , , , , , , , , , , , , , , , , ,
                            CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Part No. :",
                          mCompStatus.PartName, , , , , , , , , , , , , , , , , ,
                          "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Description :",
                                           mCompStatus.Description, , , , , , , , , , , , , , , , , ,
                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Description :",
                                          mCompStatus.Description, , , , , , , , , , , , , , , , , ,
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :",
                                                         mCompStatus.SerialNo, , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :",
                                                          mCompStatus.SerialNo, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))

                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Code :",
                                                         mCompStatus.Comp.Code, , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Code :",
                                                          mCompStatus.Comp.Code, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))

                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mCompStatus.Position, , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mCompStatus.Position, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "",
                   "", , , , , , , , , , , , , , , , , ,
                    CType(Me.mCompStatus.CompStatusPeriods(I).PeriodName, String), CType(Me.mCompStatus.CompStatusPeriods(I).CompInstallationValueFormatted, String),
                           , CType(Me.mCompStatus.CompStatusPeriods(I).AssemblyInstallationValueFormatted, String)))
            End If
        Next

        'For Install Component Mod List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , lblModInfo.Text))

        'For Install Component Mod List
        'ReportDetails.Add(New rptStatus(, 2, , _
        '      , , , dgMonitorModStatusList.Columns.Item(1).HeaderText, , dgMonitorModStatusList.Columns.Item(2).HeaderText, dgMonitorModStatusList.Columns.Item(3).HeaderText, _
        '      dgMonitorModStatusList.Columns.Item(4).HeaderText, dgMonitorModStatusList.Columns.Item(5).HeaderText, dgMonitorModStatusList.Columns.Item(6).HeaderText, _
        '      dgMonitorModStatusList.Columns.Item(7).HeaderText, , dgMonitorModStatusList.Columns.Item(8).HeaderText, dgMonitorModStatusList.Columns.Item(9).HeaderText, dgMonitorModStatusList.Columns.Item(10).HeaderText, _
        '      dgMonitorModStatusList.Columns.Item(11).HeaderText, dgMonitorModStatusList.Columns.Item(13).HeaderText, dgMonitorModStatusList.Columns.Item(14).HeaderText, _
        '      , , , dgMonitorModStatusList.Columns.Item(15).HeaderText, , ))
        ReportDetails.Add(New rptStatus(, 2, ,
            , , , dgMonitorModStatusList.Columns.Item(4).HeaderText, , dgMonitorModStatusList.Columns.Item(5).HeaderText, dgMonitorModStatusList.Columns.Item(6).HeaderText,
            dgMonitorModStatusList.Columns.Item(7).HeaderText, dgMonitorModStatusList.Columns.Item(8).HeaderText, dgMonitorModStatusList.Columns.Item(9).HeaderText,
            dgMonitorModStatusList.Columns.Item(10).HeaderText, , dgMonitorModStatusList.Columns.Item(11).HeaderText, dgMonitorModStatusList.Columns.Item(12).HeaderText, dgMonitorModStatusList.Columns.Item(14).HeaderText,
            dgMonitorModStatusList.Columns.Item(15).HeaderText, dgMonitorModStatusList.Columns.Item(16).HeaderText, dgMonitorModStatusList.Columns.Item(17).HeaderText,
            , , , dgMonitorModStatusList.Columns.Item(18).HeaderText, RHLabel2:=dgMonitorModStatusList.Columns.Item(19).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mInstallCompMonitorModStatusList.Count
        Dim m As Integer

        For m = 0 To TotalCount1 - 1
            Dim str(15) As String
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
            str(10) = ""
            str(11) = ""
            str(12) = ""
            str(13) = ""
            str(14) = ""
            str(15) = ""

            'If Me.dgMonitorModStatusList.Rows(m).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            'If Me.dgMonitorModStatusList.Rows(m).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            'If Me.dgMonitorModStatusList.Rows(m).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(4).Text <> "&nbsp;" Then str(0) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(5).Text <> "&nbsp;" Then str(1) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(6).Text <> "&nbsp;" Then str(2) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(7).Text <> "&nbsp;" Then str(3) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(8).Text <> "&nbsp;" Then str(4) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(9).Text <> "&nbsp;" Then str(5) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(10).Text <> "&nbsp;" Then str(6) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(11).Text <> "&nbsp;" Then str(7) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(11).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(12).Text <> "&nbsp;" Then str(8) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(12).Text.Replace("<BR>", vbCrLf)
            'If Me.dgMonitorModStatusList.Rows(m).Cells.Item(13).Text <> "&nbsp;" Then str(9) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(14).Text <> "&nbsp;" Then str(10) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(15).Text <> "&nbsp;" Then str(11) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(16).Text <> "&nbsp;" Then str(12) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(17).Text <> "&nbsp;" Then str(13) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(17).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(18).Text <> "&nbsp;" Then str(14) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(18).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells.Item(19).Text <> "&nbsp;" Then str(15) = Me.dgMonitorModStatusList.Rows(m).Cells.Item(19).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
                , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), , str(7), str(8), str(10),
                     str(11), str(12), str(13), , , , str(14), RHLabel2:=str(15)))
        Next
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Component Mod Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        If m = 0 Then
            ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            '''msg1.ReplacePage = "wfInstallCompMonitorModStatusList.aspx?Backpage="
            ''msg1.ReplacePage = "wfInstallCompMonitorModStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
            ''msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        '   MarkLog(Util.Action.Print, "InstallCompMonitorModStatusList", "Component Monitor Mod Status List Report", Util.ErrorType.HandledError, mInstallCompStatus.ID)

        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnCloseMod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseMod.Click, btnCloseTopMod.Click
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Install Component Mod Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        SetSession()
        ' RemoveSession()
        mInstallCompMonitorModStatusList = Nothing
        mPartMonitorModTypeList = Nothing
        Session.Remove("mInstallCompMonitorModStatusList")

        Session.Remove("mFileAttach")
        Session("FromInstallCompMonitorModStatusList") = True
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
#End Region

#End Region

End Class