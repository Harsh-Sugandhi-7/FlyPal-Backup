Public Class wfRemInstComp_AJAX
    Inherits System.Web.UI.Page


#Region " Enum "
    Public Enum From_Remove
        NewRemove = 1
        EditRemove = 2
    End Enum

    Public Enum From_Inst
        NewInstall = 1
        EditInstall = 2
        FromInstallAssembly = 3
    End Enum
#End Region

#Region " Variable Declaration"
    Public mRemCompStatus As CompStatus
    Public mPrevCompStatus As CompStatus
    Public mRemovalReasonList As RemovalReasonList
    Public mMachine As Machine
    Public mRemAssemblyStatus As AssemblyStatus
    Public mFrom_Remove As From_Remove
    Public Flag As Boolean
    Dim LogID As String
    Public mMachineMaintenance As MachineMaintenance

    Public mMachineMaintenanceList As MachineMaintenanceList
    'Public mRemAssemblyStatus As AssemblyStatus
    'Install
    Public mRemovedCompStatus As CompStatus
    Public mInstCompStatus As CompStatus
    Public mAssemblyList As AssemblyList
    Public mPartList As PartList
    Public mSelectPeriods As SelectPeriods
    ' Public mSelectPeriod As SelectPeriod           
    Public mPeriodListForCompStatus As PeriodListForCompStatus
    Public mATAList As ATAList
    'Dim mID As Guid
    Public mCompInstallInfo As String
    Dim mInstallSelected As Integer
    Public mFrom_Inst As From_Inst
    Dim mInstAssemblyStatus As AssemblyStatus

    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Public mComplyMachineMaintenance As MachineMaintenance

    Public mIsRemoval As Boolean
    Public mIsInstall As Boolean

    Dim EventLogID As Guid
    Dim MaintDetail As String
    Public mtmpInstalledCompList As tmpInstalledCompList
    Dim IsValidRemovalInstallation As String = "True" 'Change
    Dim IsExpiryServicePresent As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRemCompStatus = CType(Session("mRemCompStatus"), CompStatus)
        mRemovalReasonList = CType(Session("mRemovalReasonList"), RemovalReasonList)
        mMachine = CType(Session("mMachine"), Machine)
        mRemAssemblyStatus = CType(Session("mRemAssemblyStatus"), AssemblyStatus)
        mFrom_Remove = CType(Session("From_Remove"), From_Remove)
        mPrevCompStatus = CType(Session("mPrevCompStatus"), CompStatus)
        LogID = CType(Session("LogID"), String)

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 8th-Oct-2009

        mRemAssemblyStatus = Session("mRemAssemblyStatus")
        mIsRemoval = CType(Session("IsRemoval"), Boolean)
        'Install()
        mFrom_Inst = CType(Session("From_Inst"), From_Inst)
        mInstCompStatus = CType(Session("mInstCompStatus"), CompStatus)
        mRemovedCompStatus = CType(Session("mRemovedCompStatus"), CompStatus)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mPartList = CType(Session("mPartList"), PartList)
        mInstAssemblyStatus = CType(Session("mInstAssemblyStatus"), AssemblyStatus)
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        mPeriodListForCompStatus = CType(Session("mPeriodListForCompStatus"), PeriodListForCompStatus)
        mATAList = CType(Session("mATAList"), ATAList)
        mIsInstall = CType(Session("IsInstall"), Boolean)

        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        mComplyMachineMaintenance = CType(Session("mComplyMachineMaintenance"), MachineMaintenance)

        mtmpInstalledCompList = CType(Session("mtmpInstalledCompList"), tmpInstalledCompList)
        IsExpiryServicePresent = Session("IsExpiryServicePresent")
    End Sub
    Private Sub setSession()
        Session("mRemCompStatus") = mRemCompStatus
        Session("mRemovalReasonList") = mRemovalReasonList
        Session("mMachine") = mMachine
        Session("mRemAssemblyStatus") = mRemAssemblyStatus
        Session("mPrevCompStatus") = mPrevCompStatus
        Session("From_Remove") = mFrom_Remove

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 8th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList            'Added by Saylee on 8th-Oct-2009
        Session("mRemAssemblyStatus") = mRemAssemblyStatus
        Session("IsRemoval") = mIsRemoval
        'Install
        Session("mFrom_Inst") = mFrom_Inst
        Session("mInstCompStatus") = mInstCompStatus
        Session("mRemovedCompStatus") = mRemovedCompStatus
        Session("mAssemblyList") = mAssemblyList
        Session("mPartList") = mPartList
        Session("mInstAssemblyStatus") = mInstAssemblyStatus
        Session("mSelectPeriods") = mSelectPeriods
        Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
        Session("mATAList") = mATAList
        Session("IsInstall") = mIsInstall
        Session("mtmpInstalledCompList") = mtmpInstalledCompList

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRemovalReasonList")

        Session.Remove("mMachineMaintenance")       'Added by Saylee on 8th-Oct-2009
        Session.Remove("mMachineMaintenanceList")       'Added by Saylee on 8th-Oct-2009

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SaveComplyRecord()
        Dim mCompInfo As String
        If Not mCompMonitorServiceStatus Is Nothing Then
            mComplyMachineMaintenance = Session("mComplyMachineMaintenance")
            mCompMonitorServiceStatus.ApplyEdit()
            mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            If mComplyMachineMaintenance.IsValid = True Then
                Try
                    mComplyMachineMaintenance.ApplyEdit()
                    mComplyMachineMaintenance.Save()
                    Session("mComplyMachineMaintenance") = mComplyMachineMaintenance
                Catch ex As Exception

                End Try
            End If
            mCompInfo = Session("mCompInfo")
            MarkLog(Util.Action.Comply, "Component Service Status", mCompInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
        End If
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        mInstCompStatus = Session("mInstCompStatus")
        If mInstCompStatus.CompStatusPeriods.Count = 0 Then
            strMSG = strMSG + "Installation Component Status can not be saved without periods" + "<BR>"
        End If

        If Not mInstCompStatus.IsValid Then
            For i As Integer = 0 To mInstCompStatus.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mInstCompStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
            If strMSG.Trim <> "" Then
                cvCompValue.ErrorMessage = strMSG
            End If
        End If
        If strMSG.Trim <> "" Then
            cvCompValue.ErrorMessage = strMSG
            cvCompValue.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub SetInstObject()
        With mInstCompStatus
            .ATAID = New Guid(cmbInstATAChapter.SelectedValue)
            .Comp.PartID = New Guid(cmbInstPartNo.SelectedValue.ToString)
            .Comp.SerialNo = txtInstSerialNo.Text.Trim

            .Position = txtInstPosition.Text.Trim
            .AssemblyID = New Guid(cmbInstAssemblyList.SelectedValue.ToString)
            If calInstalledOn.Text = "" Then
                .InstalledOn = System.DBNull.Value
            Else
                .InstalledOn = calInstalledOn.Text
            End If
            .InstallationWONo = txtInstWorkOrderNo.Text.Trim
            .InstallationRemark = txtInstNote.Text.Trim
            .InstDoneBy = txtInstDoneBy.Text

            .Comp.PartID = New Guid(cmbInstPartNo.SelectedValue.ToString)

            'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
            .IsFanBladeDistribution = chkInstCompStatusFanBladeMonitoring.Checked
            .FanBladePosition = Val(txtInstCompStatusFanBladePosition.Text)
            .MomentWeight = CDec(txtInstCompStatusMomentWeight.Text)
            .BalanceScrew = Val(txtInstCompStatusBalanceScrew.Text)
            'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        End With

        Session("mInstCompStatus") = mInstCompStatus

    End Sub
    Private Sub SetInstGridObject()
        For i As Integer = 0 To dgInstallationValue.Rows.Count - 1
            Dim txtCompInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtCompInstallationValue"), TextBox)
            mInstCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = txtCompInstallationValue.Text.Trim
        Next
        Session("mInstCompStatus") = mInstCompStatus
    End Sub
    Private Sub SetInstPeroids()
        Dim mPeriodlist As PeriodList
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        mPeriodlist = PeriodList.GetPeriodList
        If Not mPeriodListForCompStatus Is Nothing Then
            For i As Integer = 0 To mPeriodListForCompStatus.Count - 1
                If Not mInstCompStatus.CompStatusPeriods.Contains(mPeriodListForCompStatus(i).PeriodID) Then
                    mSelectPeriods.Add(mPeriodListForCompStatus(i).PeriodID, mPeriodListForCompStatus(i).PeriodName)
                End If
            Next
        End If
        Session("mSelectPeriods") = mSelectPeriods
    End Sub
    Private Sub NewRecord(ByVal LogID As Guid, ByVal mCurrentDate As String)
        ''Dim mRemovedCompList As tmpRemovedCompList
        ''mRemovedCompList = tmpRemovedCompList.GetRemovedCompList(mRemCompStatus.RemovedOn.ToString, mMachine.ID.ToString, mRemCompStatus.PartName, "", mRemCompStatus.AssemblyID)
        Dim mRemovedCompStatus As CompStatus = CompStatus.GetCompStatus(mRemCompStatus.ID, mRemAssemblyStatus.ID, mCurrentDate)

        'mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mRemovedCompStatus.AssemblyID, mRemAssemblyStatus.ID, mCurrentDate,  True, mRemCompStatus.ID.ToString, LogID.ToString)
        mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mRemCompStatus.AssemblyID, mRemAssemblyStatus.ID, mCurrentDate, False, Guid.Empty.ToString, LogID.ToString)

        mInstCompStatus.InstallationWONo = mPrevCompStatus.RemovalWONo  'Added By Vikrant On 05-Oct-2021 for ALL05102021-1

        Session("mInstCompStatus") = mInstCompStatus

        mInstAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mRemAssemblyStatus.ID)
        Session("mInstAssemblyStatus") = mInstAssemblyStatus

        If mInstCompStatus.CompStatusPeriods.Count = 0 Then
            mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mInstCompStatus.AssemblyID, "")
            'Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(Guid.Empty.ToString, mRemovedCompStatus.PartName)
            Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(mRemovedCompStatus.Comp.PartID.ToString, mRemovedCompStatus.PartName)
            If mtmpCompListOnPartSelection.Count > 0 Then
                Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mInstCompStatus.AssemblyID, "")
                Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)

                If LogID.Equals(Guid.Empty) Then
                    mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, calInstalledOn.Text, True, calInstalledOn.Text)
                Else
                    mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, mCurrentDate, False, mCurrentDate, LogID.ToString)
                End If

                dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                dgInstallationValue.DataBind()
            End If

        End If

        ''dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
        ''dgInstallationValue.DataBind()

    End Sub
    Private Sub NewInstRecord(ByVal LogID As Guid, ByVal mCurrentDate As String)
        Dim mAssemblyID As Guid
        If cmbInstAssemblyList.SelectedValue = "" Then
            mAssemblyID = Guid.Empty
        Else
            mAssemblyID = New Guid(cmbInstAssemblyList.SelectedValue.ToString)
        End If

        'mRemAssemblyStatus = Session("mRemAssemblyStatus")
        'code added By Deven On 24/04/2008---------------------------------
        mInstAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mPeriodListForCompStatus(0).AssemblyStatusID)
        Session("mInstAssemblyStatus") = mInstAssemblyStatus
        '------------------------------------------------------------------

        If Not IsNothing(mRemovedCompStatus) Then
            Dim clnRemovedCompStatus As CompStatus = mRemovedCompStatus.Clone
            mRemovedCompStatus = CompStatus.GetCompStatus(clnRemovedCompStatus.ID, mRemAssemblyStatus.ID, mCurrentDate)
            mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, clnRemovedCompStatus.AssemblyID, mRemAssemblyStatus.ID, mCurrentDate, True, clnRemovedCompStatus.ID.ToString, LogID.ToString)
            clnRemovedCompStatus = Nothing
            Session("mRemovedCompStatus") = mRemovedCompStatus
        Else
            If mFrom_Inst = From_Inst.FromInstallAssembly Then
                mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mInstAssemblyStatus.AssemblyID, mInstAssemblyStatus.ID, mCurrentDate, False)
            Else
                mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mAssemblyID, mRemAssemblyStatus.ID, mCurrentDate, False, Guid.Empty.ToString, LogID.ToString)
            End If
        End If
        Session("mInstCompStatus") = mInstCompStatus
    End Sub

    Private Sub CopyFromClone(ByVal cln As CompStatus)
        REM: to recover from object when there is change in data or log 
        mInstCompStatus.Comp.PartID = cln.Comp.PartID
        mInstCompStatus.Comp.SerialNo = cln.Comp.SerialNo
        mInstCompStatus.Position = cln.Position
        mInstCompStatus.InstallationWONo = cln.InstallationWONo
        mInstCompStatus.InstallationRemark = cln.InstallationRemark
        mInstCompStatus.InstalledOn = cln.InstalledOn
        mInstCompStatus.AssemblyID = cln.AssemblyID
        mInstCompStatus.ATAID = cln.ATAID

        Session("mInstCompStatus") = mInstCompStatus
    End Sub

    Public Sub SetAssemblyPeriod()
        If cmbInstPartNo.SelectedIndex > 0 Then
            If Not New Guid(cmbInstPartNo.SelectedValue).Equals(Guid.Empty) Then

                'Dim mtmpCompStatusList As tmpCompStatusList = tmpCompStatusList.GetCompStatusList(Guid.Empty, mPartList(New Guid(cmbPartNo.SelectedValue)).Name, "", mPartList(New Guid(cmbPartNo.SelectedValue)).Description)
                Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(cmbInstPartNo.SelectedValue.ToString, mPartList(New Guid(cmbInstPartNo.SelectedValue)).Name, mPartList(New Guid(cmbInstPartNo.SelectedValue)).Description)

                If mtmpCompListOnPartSelection.Count > 0 Then
                    Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mInstCompStatus.AssemblyID, "")

                    Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)
                    If cmbInstAssemblyList.SelectedIndex > 0 Then
                        'If mFrom.NewInstall Then
                        If mFrom_Inst = From_Inst.NewInstall Or mFrom_Inst = From_Inst.EditInstall Then
                            If Not New Guid(cmbInstAssemblyList.SelectedValue).Equals(Guid.Empty) Then
                                mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbInstAssemblyList.SelectedValue), "")
                                Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
                                If mFrom_Inst = From_Inst.NewInstall Then
                                    If Not IsNothing(mRemovedCompStatus) Then
                                        Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                        NewInstRecord(Guid.Empty, calInstalledOn.Text)
                                        CopyFromClone(clnCompStatus)
                                    Else
                                        Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                        Dim tmp As CompStatusPeriods = mInstCompStatus.CompStatusPeriods
                                        NewInstRecord(Guid.Empty, calInstalledOn.Text)
                                        If mInstCompStatus.CompStatusPeriods.Count > 0 Then
                                            For i As Integer = mInstCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                                                mInstCompStatus.CompStatusPeriods.Remove(mInstCompStatus.CompStatusPeriods(i).ID)
                                            Next
                                            dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                                            dgInstallationValue.DataBind()
                                        End If
                                        If CType(Session("FromLog"), Boolean) = True Then
                                            Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
                                            Dim LogDate As String = mInstCompStatus.InstalledOn.ToString 'Request.QueryString("LogDate")
                                            mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, LogDate, False, LogDate, LogId.ToString)
                                        Else
                                            mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, calInstalledOn.Text, True, calInstalledOn.Text)
                                        End If
                                        CopyFromClone(clnCompStatus)
                                    End If
                                Else
                                    Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                    mInstCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mRemAssemblyStatus.ID, calInstalledOn.Text, Guid.Empty.ToString)
                                    CopyFromClone(clnCompStatus)
                                End If

                                ''----------'28-Apr-2009
                                ''If Session("From_Inst") = 1 And Session("mInstallSelected") <> 1 Then
                                ''    SetPeroids()
                                ''    For i As Integer = 0 To mSelectPeriods.Count - 1
                                ''        mSelectPeriods(i).IsSelected = True
                                ''    Next
                                ''    'AddSelectedPeroids()
                                ''    Dim mSelectPeriod As SelectPeriod
                                ''    If IsNothing(mSelectPeriods) Then
                                ''        mSelectPeriods = SelectPeriods.NewSelectPeriods
                                ''    End If
                                ''    For Each mSelectPeriod In mSelectPeriods
                                ''        If mSelectPeriod.IsSelected Then
                                ''            mInstCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mInstCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mInstCompStatus.InstalledOn.ToString, mSelectPeriod.PeriodID, False, mInstCompStatus.InstalledOn.ToString))
                                ''        End If
                                ''    Next
                                ''    Session("mInstCompStatus") = mInstCompStatus
                                ''    Session.Remove("mSelectPeriods")
                                ''    mSelectPeriods = Nothing
                                ''End If
                                ''---------

                                dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                                dgInstallationValue.DataBind()
                                '' ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                                If cmbInstAssemblyList.Enabled = True Then
                                    setFocus(cmbInstAssemblyList)
                                End If
                            End If
                        End If
                    Else
                        Dim str As String = "abc"
                        If mInstCompStatus.CompStatusPeriods.Count > 0 Then
                            For i As Integer = mInstCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                                mInstCompStatus.CompStatusPeriods.Remove(mInstCompStatus.CompStatusPeriods(i).ID)
                            Next
                            dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                            dgInstallationValue.DataBind()
                        End If
                    End If
                    tmpPeriodListForCompStatus = Nothing
                Else
                    If cmbInstAssemblyList.SelectedIndex > 0 Then
                        'If mFrom.NewInstall Then
                        If mFrom_Inst = From_Inst.NewInstall Then
                            If Not New Guid(cmbInstAssemblyList.SelectedValue).Equals(Guid.Empty) Then
                                mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbInstAssemblyList.SelectedValue), "")
                                Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
                                If mFrom_Inst = From_Inst.NewInstall Then
                                    If Not IsNothing(mRemovedCompStatus) Then
                                        Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                        NewInstRecord(Guid.Empty, calInstalledOn.Text)
                                        CopyFromClone(clnCompStatus)
                                    Else
                                        Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                        Dim tmp As CompStatusPeriods = mInstCompStatus.CompStatusPeriods
                                        NewInstRecord(Guid.Empty, calInstalledOn.Text)
                                        If CType(Session("FromLog"), Boolean) = True Then
                                            Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
                                            Dim LogDate As String = mInstCompStatus.InstalledOn.ToString 'Request.QueryString("LogDate")
                                            mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, LogDate, False, LogDate, LogId.ToString)
                                        Else
                                            mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, calInstalledOn.Text, True, calInstalledOn.Text)
                                        End If
                                        CopyFromClone(clnCompStatus)
                                    End If
                                Else
                                    Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                    mInstCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mRemAssemblyStatus.ID, calInstalledOn.Text, Guid.Empty.ToString)
                                    CopyFromClone(clnCompStatus)
                                End If

                                '----------'28-Apr-2009
                                If Session("From_Inst") = 1 And Session("mInstallSelected") <> 1 Then
                                    SetInstPeroids()
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
                                            mInstCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mInstCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mInstCompStatus.InstalledOn.ToString, mSelectPeriod.PeriodID, False, mInstCompStatus.InstalledOn.ToString))
                                        End If
                                    Next
                                    Session("mInstCompStatus") = mInstCompStatus
                                    Session.Remove("mSelectPeriods")
                                    mSelectPeriods = Nothing
                                End If
                                '---------

                                dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                                dgInstallationValue.DataBind()
                                ''ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                                If cmbInstAssemblyList.Enabled = True Then
                                    setFocus(cmbInstAssemblyList)
                                End If
                            End If
                        End If
                    Else
                        Dim str As String = "abc"
                        If mInstCompStatus.CompStatusPeriods.Count > 0 Then
                            For i As Integer = mInstCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                                mInstCompStatus.CompStatusPeriods.Remove(mInstCompStatus.CompStatusPeriods(i).ID)
                            Next
                            dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                            dgInstallationValue.DataBind()
                        End If
                    End If
                End If
            End If
        Else
            If cmbInstAssemblyList.SelectedIndex > 0 Then
                'If mFrom.NewInstall Then
                If mFrom_Inst = From_Inst.NewInstall Then
                    If Not New Guid(cmbInstAssemblyList.SelectedValue).Equals(Guid.Empty) Then
                        mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbInstAssemblyList.SelectedValue), "")
                        Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
                        If mFrom_Inst = From_Inst.NewInstall Then
                            If Not IsNothing(mRemovedCompStatus) Then
                                Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                NewInstRecord(Guid.Empty, calInstalledOn.Text)
                                CopyFromClone(clnCompStatus)
                            Else
                                Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                                Dim tmp As CompStatusPeriods = mInstCompStatus.CompStatusPeriods
                                NewInstRecord(Guid.Empty, calInstalledOn.Text)
                                If CType(Session("FromLog"), Boolean) = True Then
                                    Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
                                    Dim LogDate As String = mInstCompStatus.InstalledOn.ToString 'Request.QueryString("LogDate")
                                    mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, LogDate, False, LogDate, LogId.ToString)
                                Else
                                    mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, calInstalledOn.Text, True, calInstalledOn.Text)
                                End If
                                CopyFromClone(clnCompStatus)
                            End If
                        Else
                            Dim clnCompStatus As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
                            mInstCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mRemAssemblyStatus.ID, calInstalledOn.Text, Guid.Empty.ToString)
                            CopyFromClone(clnCompStatus)
                        End If

                        '----------'28-Apr-2009
                        If Session("From_Inst") = 1 And Session("mInstallSelected") <> 1 Then
                            SetInstPeroids()
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
                                    mInstCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mInstCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mInstCompStatus.InstalledOn.ToString, mSelectPeriod.PeriodID, False, mInstCompStatus.InstalledOn.ToString))
                                End If
                            Next
                            Session("mInstCompStatus") = mInstCompStatus
                            Session.Remove("mSelectPeriods")
                            mSelectPeriods = Nothing
                        End If
                        '---------

                        dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                        dgInstallationValue.DataBind()
                        '' ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                        If cmbInstAssemblyList.Enabled = True Then
                            setFocus(cmbInstAssemblyList)
                        End If
                    End If
                End If
                Dim str As String = "abc"
                If mInstCompStatus.CompStatusPeriods.Count > 0 Then
                    For i As Integer = mInstCompStatus.CompStatusPeriods.Count - 1 To 0 Step -1
                        mInstCompStatus.CompStatusPeriods.Remove(mInstCompStatus.CompStatusPeriods(i).ID)
                    Next
                    dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                    dgInstallationValue.DataBind()
                End If
            End If
        End If
    End Sub
    Private Sub SetRemObject()
        With mRemCompStatus
            .RemovalReasonID = New Guid(cmbReason.SelectedValue)
            .RemovalReasonName = cmbReason.SelectedItem.Text
            .Comp.SerialNo = Trim(txtSerialNo.Text)
            .Position = Trim(txtPosition.Text)
            If calRemove.Text = "" Then
                .RemovedOn = System.DBNull.Value
            Else
                .RemovedOn = calRemove.Text
            End If
            .RemovalWONo = Trim(txtWorkOrderNo.Text)
            .RemovalRemark = Trim(txtNote.Text)
            .IsExpired = chkExpired.Checked
			.IsRemUnschedule = chkIsRemUnscheduled.Checked 'Sankalp
			'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
			.IsFanBladeDistribution = False
            .FanBladePosition = 0
            .MomentWeight = 0
            .BalanceScrew = 0
            'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
            'Added By Saylee on 24-Apr-2009
            mRemCompStatus.RemDoneBy = Trim(txtRemDoneBy.Text)
            '==================================
        End With
        Session("mRemCompStatus") = mRemCompStatus
    End Sub
    Private Sub SetFormClone(ByVal clnCompStatus As CompStatus)
        mRemCompStatus.RemovalWONo = clnCompStatus.RemovalWONo
        mRemCompStatus.RemovalReasonID = clnCompStatus.RemovalReasonID
        mRemCompStatus.RemovalReasonName = clnCompStatus.RemovalReasonName
        mRemCompStatus.RemovalRemark = clnCompStatus.RemovalRemark
        mRemCompStatus.RemovedOn = clnCompStatus.RemovedOn
        clnCompStatus = Nothing
    End Sub
    Private Sub SetFormInstClone(ByVal clnCompStatus As CompStatus)
        mInstCompStatus.InstallationWONo = clnCompStatus.RemovalWONo
        mInstCompStatus.InstallationReason = clnCompStatus.InstallationReason
        mInstCompStatus.InstallationRemark = clnCompStatus.RemovalRemark
        mInstCompStatus.InstalledOn = clnCompStatus.InstalledOn
        clnCompStatus = Nothing
    End Sub
    Private Sub SetPage()
        lblTitle.Text = "Removal/Installation of Component " & " from/On " & mRemAssemblyStatus.AssemblyTypeName
        lblCompRemInfo.Text = "Part and Serial No. of the Component"
        lblRemovalInfo.Text = "Removal Information of the Component"

        lblCompInstInfo.Text = "Part and Serial No. of the Component"
        lbInstallationInfo.Text = "Installation Information of the Component"
    End Sub
    Public Sub ControlVisibility(ByVal IsForRemoval, ByVal IsForInstall)
        If Session("IsFromSpareWO") = "True" Then
            btnSelectLog.Visible = False
        Else
            btnSelectLog.Visible = True
        End If
        If IsForRemoval = True Then
            btnSelectLog.Enabled = True
            calRemove.Enabled = True
            txtWorkOrderNo.Enabled = True
            cmbReason.Enabled = True
            imgbtnReason.Enabled = True
            txtNote.Enabled = True
            chkExpired.Enabled = True
            txtRemDoneBy.Enabled = True

            dgRemovalValue.Enabled = True
        Else
            btnSelectLog.Enabled = False
            calRemove.Enabled = False
            txtWorkOrderNo.Enabled = False
            cmbReason.Enabled = False
            imgbtnReason.Enabled = False
            txtNote.Enabled = False
            chkExpired.Enabled = False
            txtRemDoneBy.Enabled = False

            dgRemovalValue.Enabled = False
        End If

        If IsForInstall = True Then
            txtInstSerialNo.Enabled = True
            txtInstPosition.Enabled = True
            calInstalledOn.Enabled = True
            txtInstWorkOrderNo.Enabled = True
            txtInstNote.Enabled = True
            txtInstDoneBy.Enabled = True
            '' cmbInstAssemblyList.Enabled = True
            cmbInstPartNo.Enabled = True
            cmbInstATAChapter.Enabled = True
            btnAddPeriod.Enabled = True

            dgInstallationValue.Enabled = True
            'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
            lnkInstallSelected.Enabled = True
            lnkInstallSpareComponent.Enabled = True
            'End
            'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
            chkInstCompStatusFanBladeMonitoring.Enabled = True
            'txtInstCompStatusFanBladePosition.Enabled = True
            'txtInstCompStatusMomentWeight.Enabled = True
            'txtInstCompStatusBalanceScrew.Enabled = True
            'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        Else
            txtInstSerialNo.Enabled = False
            txtInstPosition.Enabled = False
            calInstalledOn.Enabled = False
            txtInstWorkOrderNo.Enabled = False
            txtInstNote.Enabled = False
            txtInstDoneBy.Enabled = False
            '' cmbInstAssemblyList.Enabled = False
            cmbInstPartNo.Enabled = False
            cmbInstATAChapter.Enabled = False
            btnAddPeriod.Enabled = False

            dgInstallationValue.Enabled = False
            'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
            lnkInstallSelected.Enabled = False
            lnkInstallSpareComponent.Enabled = False
            'End
            'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
            chkInstCompStatusFanBladeMonitoring.Checked = False
            chkInstCompStatusFanBladeMonitoring.Enabled = False
            txtInstCompStatusFanBladePosition.Text = "0"
            txtInstCompStatusFanBladePosition.Enabled = False
            txtInstCompStatusMomentWeight.Text = "0"
            txtInstCompStatusMomentWeight.Enabled = False
            txtInstCompStatusBalanceScrew.Text = "0"
            txtInstCompStatusBalanceScrew.Enabled = False
            'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        End If
        'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
        If (User.IsInRole("BuildSpareCompNew") = True And User.IsInRole("BuildSpareCompEdit") = True) Then
            lnkInstallSpareComponent.Visible = True
        End If
        'End
        'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        If (mRemAssemblyStatus.AssemblyTypeName = "Engine" And mRemAssemblyStatus.AssemblyTypeID = 2 And AppSettings("ShowFanBladeDistributionMonitoring") = "True") Then
            lblRemovalFanBladeMonitoring.Visible = True
            chkRemCompStatusFanBladeMonitoring.Visible = True
            lblRemCompStatusFanBladePosition.Visible = True
            txtRemCompStatusFanBladePosition.Visible = True
            lblRemCompStatusMomentWeight.Visible = True
            txtRemCompStatusMomentWeight.Visible = True
            lblRemCompStatusBalanceScrew.Visible = True
            txtRemCompStatusBalanceScrew.Visible = True

            lblInstCompStatusFanBladeMonitoring.Visible = True
            chkInstCompStatusFanBladeMonitoring.Visible = True
            lblInstCompStatusPosition.Visible = True
            txtInstCompStatusFanBladePosition.Visible = True
            lblInstCompStatusMomentWeight.Visible = True
            txtInstCompStatusMomentWeight.Visible = True
            lblInstCompStatusBalanceScrew.Visible = True
            txtInstCompStatusBalanceScrew.Visible = True
        Else
            lblRemovalFanBladeMonitoring.Visible = False
            chkRemCompStatusFanBladeMonitoring.Visible = False
            lblRemCompStatusFanBladePosition.Visible = False
            txtRemCompStatusFanBladePosition.Visible = False
            lblRemCompStatusMomentWeight.Visible = False
            txtRemCompStatusMomentWeight.Visible = False
            lblRemCompStatusBalanceScrew.Visible = False
            txtRemCompStatusBalanceScrew.Visible = False

            lblInstCompStatusFanBladeMonitoring.Visible = False
            chkInstCompStatusFanBladeMonitoring.Visible = False
            lblInstCompStatusPosition.Visible = False
            txtInstCompStatusFanBladePosition.Visible = False
            lblInstCompStatusMomentWeight.Visible = False
            txtInstCompStatusMomentWeight.Visible = False
            lblInstCompStatusBalanceScrew.Visible = False
            txtInstCompStatusBalanceScrew.Visible = False
        End If
        'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
    End Sub
    Private Sub SetLog()
        If CType(Session("FromLog"), Boolean) = True Then
            'Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
            'Dim LogDate As String = mInstCompStatus.InstalledOn.ToString 'Request.QueryString("LogDate")
            Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
            Dim LogDate = CType(Session("mDoneOn"), String)
            If mFrom_Inst = From_Inst.NewInstall Or mFrom_Inst = From_Inst.FromInstallAssembly Then
                If Not IsNothing(mInstCompStatus) Then
                    Dim clnCompStatus As CompStatus = mInstCompStatus.Clone
                    NewRecord(LogId, LogDate)
                    CopyFromClone(clnCompStatus)
                Else
                    Dim clnCompStatus As CompStatus = mInstCompStatus.Clone
                    Dim tmp As CompStatusPeriods = mInstCompStatus.CompStatusPeriods
                    NewRecord(LogId, LogDate)
                    mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmp, LogDate, False, LogDate, LogId.ToString)
                    CopyFromClone(clnCompStatus)
                    clnCompStatus = Nothing
                End If
            Else
                Dim clnCompStatus As CompStatus = mInstCompStatus.Clone
                mInstCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mRemAssemblyStatus.ID, LogDate, LogId.ToString)
                CopyFromClone(clnCompStatus)
                clnCompStatus = Nothing
            End If
        End If
    End Sub
    Private Sub FromLog()
        If CType(Session("FromLog"), Boolean) = True Then

            'Remove
            Dim tmpCompStatus As CompStatus
            If Not mRemCompStatus Is Nothing Then
                tmpCompStatus = mRemCompStatus
            End If
            If mFrom_Remove = From_Remove.NewRemove Then
                mRemCompStatus = CompStatus.NewRemovalCompStatus(mPrevCompStatus.ID, calRemove.Text, mRemAssemblyStatus.ID, LogID.ToString)
            Else
                mRemCompStatus = CompStatus.GetRemovalCompStatus(mPrevCompStatus.ID, mRemAssemblyStatus.ID, calRemove.Text, LogID.ToString)
            End If
            If Not tmpCompStatus Is Nothing Then
                mRemCompStatus.RemovalReasonID = tmpCompStatus.RemovalReasonID
                mRemCompStatus.RemovalReasonName = tmpCompStatus.RemovalReasonName
                mRemCompStatus.RemovalWONo = tmpCompStatus.RemovalWONo
                mRemCompStatus.RemovalRemark = tmpCompStatus.RemovalRemark
                mRemCompStatus.IsExpired = tmpCompStatus.IsExpired
            End If

            dgRemovalValue.DataSource = mRemCompStatus.CompStatusPeriods
            dgRemovalValue.DataBind()
            calRemove.Text = mRemCompStatus.RemovedOnFormatted.ToString
            Session("mRemCompStatus") = mRemCompStatus

            'Install
            Dim mAssemblyID As Guid
            If cmbInstAssemblyList.SelectedValue = "" Then
                mAssemblyID = Guid.Empty
            Else
                mAssemblyID = New Guid(cmbInstAssemblyList.SelectedValue.ToString)
            End If
            SetLog()
            dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
            dgInstallationValue.DataBind()


            Session.Remove("FromLog")
            'Added by Saylee on 8th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            Session("mLog") = mLog
            '===========================================
        Else
            Session.Remove("mLog")
        End If
        '**************************************************************
    End Sub
    Private Function SaveInstall() As Boolean
        If chkInstallation.Checked = True Then
            mInstCompStatus = Session("mInstCompStatus")

            If Not IsValid Then Exit Function
            Dim mClnInsComp As CompStatus = CType(mInstCompStatus.Clone, CompStatus)
            SetInstObject()
            SetInstGridObject()
            SetInstMachineMaintenanceObject()
            If mInstCompStatus.IsValid = True Then
                Try
                    If mInstCompStatus.CompStatusPeriods.Count = 0 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Installation Component Status can not be saved without periods", MsgBoxStyle.OKOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Installation Component Status can not be saved without periods", MsgBoxStyle.OkOnly, "")
                        Return False
                    End If
                    mInstCompStatus.ApplyEdit()
                    mInstCompStatus = CType(mInstCompStatus.Save, CompStatus)
                    SaveMachineMaintenance()

                    mCompInstallInfo = "ATAChapter -> " + mInstCompStatus.ATAChapter + " Part -> " + mInstCompStatus.PartNameSerialNo + " -> " + " InstallOn -> " + mInstCompStatus.InstalledOn.ToString   'Code Added Jan,30,2007
                    'Added By Vikrant On 27-Jul-2020 For ALL27072020
                    Dim mRegNo As String = ""
                    If mInstAssemblyStatus.IsSpareAssembly = False Then
                        mRegNo = "Reg No. : " & mMachine.RegNo
                    End If
                    'End
                    'MaintDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mInstAssemblyStatus.ModelName + " " + mInstAssemblyStatus.Assembly.SerialNo & " Part Info : " & mInstCompStatus.Comp.PartName + " " + mInstCompStatus.Comp.Description + " " + mInstCompStatus.Comp.SerialNo
                    MaintDetail = "Reg No. : " & mRegNo & " Assembly Info : " & mInstAssemblyStatus.ModelName + " " + mInstAssemblyStatus.Assembly.SerialNo & " Part Info : " & mInstCompStatus.Comp.PartName + " " + mInstCompStatus.Comp.Description + " " + mInstCompStatus.Comp.SerialNo

                    MarkLog(Util.Action.Install, "Component Install", MaintDetail, Util.ErrorType.NoError, mInstCompStatus.ID, EventLogID)
                    Return True
                Catch ex As SqlException
                    Session("mClnInsComp") = mClnInsComp
                    If ex.Number = 8114 Or ex.Number = 8115 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 8145 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "Part No. With Same Serial No. Already Exist!!", MsgBoxStyle.OKOnly) 'Change
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show("Installation Save Alert!", "Part No. With Same Serial No. Already Exist!!", "", MsgBoxStyle.OkOnly, "Dup")
                    ElseIf ex.Number = 547 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Return False
                Finally
                    mClnInsComp = Nothing
                End Try
            Else
                Session("mInstCompStatus") = mInstCompStatus
                CustomValidate1()
                Return False
            End If
        End If
    End Function
    Private Function SaveRemoval() As Boolean
        If chkRemoval.Checked = True Then
            If Not IsValid Then Exit Function

            ' SaveComplyRecord() ''Not used as no need to comply record as component is getting removed

            Dim CompStatusClone As CompStatus
            CompStatusClone = CType(mRemCompStatus.Clone, CompStatus)
            SetRemObject()
            SetRemMachineMaintenanceObject()
            If mRemCompStatus.IsValid = True Then
                Try
                    mRemCompStatus.ApplyEdit()
                    mRemCompStatus = CType(mRemCompStatus.Save(), CompStatus)
                    
                    Session("mRemCompStatus") = mRemCompStatus
                    SaveMachineMaintenance()  'Added by Saylee on 8th-Oct-2009

                    'MarkLog(Util.Action.Save, "CompRemoval", RemovalComp, Util.ErrorType.NoError, mRemCompStatus.ID)
                    SaveComplyRecord() 'Added by Saylee on 20-Jul-2018 for ALL20072018, as Compliance will be saved only if Component is removed & saved
                    Return True
                Catch ex As SqlException
                    Session("CompStatusClone") = CompStatusClone
                    If ex.Number = 8114 Or ex.Number = 8115 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 8145 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Part No. With Same Serial No. Already Exist!!", MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Return False
                Finally
                    CompStatusClone = Nothing
                End Try
            Else
                Return False
            End If
        End If

    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'If MSGBoxCtrl.Sender = "Save" Then
                    '    Session("sender") = ""
                    '    SaveRemoval()
                    '    'Response.Redirect("wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    'End If
                Case MsgBoxResult.No
                    'If MSGBoxCtrl.Sender = "Save" Then
                    '    Session("sender") = ""
                    '    ' Response.Redirect("wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    'End If
                Case MsgBoxResult.Cancel
                    'If MSGBoxCtrl.Sender = "Save" Then
                    '    Session("sender") = ""
                    '    '   Response.Redirect("wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    'End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    If MSGBoxCtrl.Sender = "ExServiceConfig" Then
                        If IsValidRemovalInstallation = "True" Then 'Change
                            Session.Remove("IsExpiryServicePresent")
                            'Added by vikrant on 06-Sep-2019 for ALL06092019
                            Dim URLForWOCompliance As Stack = CType(Session("URLForWOCompliance"), Stack)
                            If Not URLForWOCompliance Is Nothing Then
                                If URLForWOCompliance.Count > 0 Then

                                    Response.Redirect(URLForWOCompliance.Peek.ToString)
                                    Exit Sub
                                End If
                            End If
                            'End
                            Response.Redirect(Request.QueryString("BackPage"))
                        End If
                    End If

                    'Session("sender") = ""
                    'DataFieldBind()
                    '  Response.Redirect("wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    '  Response.Redirect("wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            '  Response.Redirect("wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub SetInstMachineMaintenanceObject()
        mInstAssemblyStatus = Session("mInstAssemblyStatus")
        mInstCompStatus = Session("mInstCompStatus")

        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        'Added by Saylee on 8th-Oct-2009
        If mFrom_Inst = From_Inst.NewInstall And (Not mMachineMaintenanceList.Contains(mInstCompStatus.ID, 3, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mInstAssemblyStatus.MachineID, 3, calInstalledOn.Text, mInstCompStatus.ID, Guid.Empty, 0, 0, mInstAssemblyStatus.ID)
        Else
            mInstCompStatus = CType(Session("mInstCompStatus"), CompStatus)
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mInstCompStatus.ID, 3)
            Session("mMachineMaintenance") = mMachineMaintenance
        End If


        With mMachineMaintenance
            .MachineID = mInstAssemblyStatus.MachineID
            .MaintenanceActivityTypeID = 3
            .MaintenanceID = mInstCompStatus.ID 'TransactionID
            .AssemblyStatusID = mInstAssemblyStatus.ID

            .Date = calInstalledOn.Text

            Dim mLog As Log = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(calInstalledOn.Text, mInstAssemblyStatus.MachineID, mInstAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If
        End With

        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub SetRemMachineMaintenanceObject()
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        'Added by Saylee on 8th-Oct-2009
        If (mFrom_Remove = From_Remove.NewRemove) And Not (mMachineMaintenanceList.Contains(mRemCompStatus.ID, 4, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mRemAssemblyStatus.MachineID, 4, calRemove.Text, mRemCompStatus.ID, Guid.Empty, 0, 0, mRemAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mRemCompStatus.ID, 4)
            Session("mMachineMaintenance") = mMachineMaintenance
        End If

        With mMachineMaintenance
            .MachineID = mRemAssemblyStatus.MachineID
            .MaintenanceActivityTypeID = 4
            .MaintenanceID = mRemCompStatus.ID 'TransactionID
            .AssemblyStatusID = mRemAssemblyStatus.ID

            .Date = calRemove.Text

            Dim mLog As Log = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(calRemove.Text, mRemAssemblyStatus.MachineID, mRemAssemblyStatus.AssemblyID)
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
        '' mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        ''If Not mMachineMaintenanceList.Contains(mMachineMaintenance.MaintenanceID, "") Then
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
    Private Sub AddSelectedPeroids()

        Dim mSelectPeriod As SelectPeriod
        If IsNothing(mSelectPeriods) Then
            mSelectPeriods = SelectPeriods.NewSelectPeriods
        End If
        For Each mSelectPeriod In mSelectPeriods
            If mSelectPeriod.IsSelected Then
                mInstCompStatus.CompStatusPeriods.Add(CompStatusPeriod.NewInstallChildCompStatusPeriod(mInstCompStatus.ID, mPeriodListForCompStatus(0).AssemblyStatusID, mInstCompStatus.InstalledOn.ToString, mSelectPeriod.PeriodID, False, mInstCompStatus.InstalledOn.ToString))
            End If
        Next
        '   SetAssemblyPeriod()
        Session("mInstCompStatus") = mInstCompStatus
        Session.Remove("mSelectPeriods")
        mSelectPeriods = Nothing
    End Sub
    Private Sub SetMachineMaintenanceObject(ByVal CompMonitorStatusID As Guid, ByVal mAssemblyStatus As AssemblyStatus)

        If Not (mMachineMaintenanceList.Contains(CompMonitorStatusID, 9, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 9, "", CompMonitorStatusID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(CompMonitorStatusID, 9)
        End If

        With mMachineMaintenance

            .MaintenanceID = CompMonitorStatusID 'TransactionID
            Dim mLog As Log
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo("", mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                End If
            End If

        End With

        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub CopyServices()
        Dim mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
        mCompMonitorServiceStatusList = tmpCompMonitorServiceStatusList.GetCompMonitorServiceStatusList(mRemCompStatus.AsOnDate.ToString, mRemCompStatus.CompID, False, , , , , , , mRemAssemblyStatus.MachineID.ToString, mRemAssemblyStatus.AssemblyID.ToString, mRemAssemblyStatus.ID.ToString)
        Dim mInstCompMonitorServiceStatus As CompMonitorServiceStatus

        For i As Integer = 0 To mCompMonitorServiceStatusList.Count - 1
            If Not mCompMonitorServiceStatusList(i).PartMonitorServiceTypeID = 5 Then 'skip expiry service
                Dim mRemCompMonitorServiceStatus As CompMonitorServiceStatus
                'Commented and Added By Vikrant On 27-Jul-2020 For ALL27072020
                'mRemCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mCompMonitorServiceStatusList(i).ID, mRemAssemblyStatus.ID, mRemCompStatus.ID, mMachine.HourType)
                'mInstCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mInstCompStatus.CompID, mInstAssemblyStatus.ID, CStr(mInstAssemblyStatus.AsOnDate), mInstCompStatus.Comp.PartID, mInstAssemblyStatus.Assembly.ModelID, mInstCompStatus.ID, mMachine.HourType, mInstCompStatus)
                mRemCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mCompMonitorServiceStatusList(i).ID, mRemAssemblyStatus.ID, mRemCompStatus.ID, mRemAssemblyStatus.HourType)
                mInstCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mInstCompStatus.CompID, mInstAssemblyStatus.ID, CStr(mInstAssemblyStatus.AsOnDate), mInstCompStatus.Comp.PartID, mInstAssemblyStatus.Assembly.ModelID, mInstCompStatus.ID, mRemAssemblyStatus.HourType, mInstCompStatus)
                'End
                ''SetMachineMaintenanceObject(mInstCompMonitorServiceStatus.ID, mInstAssemblyStatus)

                mInstCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.CopyCompMonitorServiceStatusPeriod(mCompMonitorServiceStatusList.Count - 1, mInstCompMonitorServiceStatus.ID, mInstCompMonitorServiceStatus.AssemblyStatusID, mRemCompMonitorServiceStatus.PartMonitorService, Today.Date.ToString, mInstCompStatus.ID, mInstCompMonitorServiceStatus.IsMaster)
                With mInstCompMonitorServiceStatus
                    mInstCompMonitorServiceStatus.PartMonitorServiceID(False) = mRemCompMonitorServiceStatus.PartMonitorServiceID
                    .SourceDoc = mRemCompMonitorServiceStatus.SourceDoc
                    .RevisionNo = mRemCompMonitorServiceStatus.RevisionNo
                    .BookNo = mRemCompMonitorServiceStatus.BookNo
                    .PageNo = mRemCompMonitorServiceStatus.PageNo
                    .RequiredManHours = mRemCompMonitorServiceStatus.RequiredManHours
                    .ExtensionDate = mRemCompMonitorServiceStatus.ExtensionDate
                    .ApprovalRemark = mRemCompMonitorServiceStatus.ApprovalRemark
                    .IsApplicable = mRemCompMonitorServiceStatus.IsApplicable
                    .DoneBy = mRemCompMonitorServiceStatus.DoneBy
                    .IsLater = mRemCompMonitorServiceStatus.IsLater

                    ''For j As Integer = 0 To mRemCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
                    ''    mInstCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).RemainingValue = mRemCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).RemainingValue
                    ''    mInstCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).ElapsedValue = mRemCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).ElapsedValue
                    ''    mInstCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).ExtensionValue = mRemCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).ExtensionValue
                    ''Next

                End With

                Try
                    If mInstCompMonitorServiceStatus.IsValid = True Then
                        mInstCompMonitorServiceStatus.ApplyEdit()
                        mInstCompMonitorServiceStatus = CType(mInstCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
                        SaveMachineMaintenance()
                        'MaintDetail = "Reg No. : " + mtmpInstalledCompList(mInstCompMonitorServiceStatus.CompStatusID).MachineInfo & " Assembly Info : " & mtmpInstalledCompList(mInstCompMonitorServiceStatus.CompStatusID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mtmpInstalledCompList(mInstCompMonitorServiceStatus.CompStatusID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstCompMonitorServiceStatus.PartMonitorService.MonitorTypeName.Replace(Environment.NewLine, " ")
                        'MarkLog(Util.Action.Save, "CompMonitorServiceStatus", " Part: " & mInstCompStatus.PartName & " Serial No.: " & mInstCompStatus.SerialNo, Util.ErrorType.NoError, mInstCompMonitorServiceStatus.ID, EventLogID)
                        MarkLog(Util.Action.Save, "Component Service Status", MaintDetail, Util.ErrorType.NoError, mInstCompMonitorServiceStatus.ID, EventLogID)
                    Else
                        Dim strMSG As String = ""
                        For k As Integer = 0 To mInstCompMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                            strMSG = strMSG + mInstCompMonitorServiceStatus.GetBrokenRulesCollection(k).Description + "<BR>"
                        Next
                        For m As Integer = 0 To mInstCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
                            For k As Integer = 0 To mInstCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).GetBrokenRulesCollection.Count - 1
                                strMSG = strMSG + mInstCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).GetBrokenRulesCollection(k).Description + "<BR>"
                            Next
                        Next
                    End If
                Catch ex As SqlException
                    If ex.Number = 8114 Or ex.Number = 8115 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 8145 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Service Already Exist!!", MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                End Try
            Else
                If mCompMonitorServiceStatusList(i).PartMonitorServiceTypeID = 5 And Not IsExpiryServicePresent Then
                    IsExpiryServicePresent = True
                    Session("IsExpiryServicePresent") = IsExpiryServicePresent
                End If
            End If
        Next
    End Sub
    Private Sub CopyInspections()
        Dim mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
        mCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mRemCompStatus.AsOnDate.ToString, mRemCompStatus.CompID, False, , , , , , , mRemAssemblyStatus.MachineID.ToString, mRemAssemblyStatus.AssemblyID.ToString, mRemAssemblyStatus.ID.ToString)
        Dim mInstCompMonitorInspStatus As CompMonitorInspStatus

        For i As Integer = 0 To mCompMonitorInspStatusList.Count - 1
            Dim mRemCompMonitorInspStatus As CompMonitorInspStatus
            'Commented and Added By Vikrant On 27-Jul-2020 For ALL27072020
            'mRemCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mCompMonitorInspStatusList(i).ID, mRemAssemblyStatus.ID, mRemCompStatus.ID, mMachine.HourType)
            'mInstCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mInstCompStatus.CompID, mInstAssemblyStatus.ID, CStr(mInstAssemblyStatus.AsOnDate), mInstCompStatus.Comp.PartID, mInstAssemblyStatus.Assembly.ModelID, mInstCompStatus.ID, mMachine.HourType)
            mRemCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mCompMonitorInspStatusList(i).ID, mRemAssemblyStatus.ID, mRemCompStatus.ID, mRemAssemblyStatus.HourType)
            mInstCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mInstCompStatus.CompID, mInstAssemblyStatus.ID, CStr(mInstAssemblyStatus.AsOnDate), mInstCompStatus.Comp.PartID, mInstAssemblyStatus.Assembly.ModelID, mInstCompStatus.ID, mRemAssemblyStatus.HourType)
            'End
            'SetMachineMaintenanceObject(mInstCompMonitorInspStatus.ID, mInstAssemblyStatus)

            mInstCompMonitorInspStatus.CompMonitorInspStatusPeriods.CopyCompMonitorInspStatusPeriod(mCompMonitorInspStatusList.Count - 1, mInstCompMonitorInspStatus.ID, mInstCompMonitorInspStatus.AssemblyStatusID, mRemCompMonitorInspStatus.PartMonitorInsp, Today.Date.ToString, mInstCompStatus.ID, mInstCompMonitorInspStatus.IsMaster)
            With mInstCompMonitorInspStatus
                mInstCompMonitorInspStatus.PartMonitorInspID(False) = mRemCompMonitorInspStatus.PartMonitorInspID
                .SourceDoc = mRemCompMonitorInspStatus.SourceDoc
                .RevisionNo = mRemCompMonitorInspStatus.RevisionNo
                .BookNo = mRemCompMonitorInspStatus.BookNo
                .PageNo = mRemCompMonitorInspStatus.PageNo
                .RequiredManHours = mRemCompMonitorInspStatus.RequiredManHours
                .ExtensionDate = mRemCompMonitorInspStatus.ExtensionDate
                .ApprovalRemark = mRemCompMonitorInspStatus.ApprovalRemark
                .IsApplicable = mRemCompMonitorInspStatus.IsApplicable
                .DoneBy = mRemCompMonitorInspStatus.DoneBy
                .IsLater = mRemCompMonitorInspStatus.IsLater

                ''For j As Integer = 0 To mRemCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
                ''    mInstCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).RemainingValue = mRemCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).RemainingValue
                ''    mInstCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).ElapsedValue = mRemCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).ElapsedValue
                ''    mInstCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).ExtensionValue = mRemCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).ExtensionValue
                ''Next

            End With

            Try
                If mInstCompMonitorInspStatus.IsValid = True Then
                    mInstCompMonitorInspStatus.ApplyEdit()
                    mInstCompMonitorInspStatus = CType(mInstCompMonitorInspStatus.Save(), CompMonitorInspStatus)
                    SaveMachineMaintenance()
                    'MarkLog(Util.Action.Save, "Install Component Inspection Status", " Part: " & mInstCompStatus.PartName & " Serial No.: " & mInstCompStatus.SerialNo, Util.ErrorType.NoError, mInstCompMonitorInspStatus.ID, EventLogID)
                    'MaintDetail = "Reg No. : " + mtmpInstalledCompList(mInstCompMonitorInspStatus.CompStatusID).MachineInfo & " Assembly Info : " & mtmpInstalledCompList(mInstCompMonitorInspStatus.CompStatusID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mtmpInstalledCompList(mInstCompMonitorInspStatus.CompStatusID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstCompMonitorInspStatus.PartMonitorInsp.MonitorTypeName.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Save, "Component Inspection Status", MaintDetail, Util.ErrorType.NoError, mInstCompMonitorInspStatus.ID, EventLogID)

                Else
                    Dim strMSG As String = ""
                    For k As Integer = 0 To mInstCompMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                        strMSG = strMSG + mInstCompMonitorInspStatus.GetBrokenRulesCollection(k).Description + "<BR>"
                    Next
                    For m As Integer = 0 To mInstCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
                        For k As Integer = 0 To mInstCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).GetBrokenRulesCollection.Count - 1
                            strMSG = strMSG + mInstCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).GetBrokenRulesCollection(k).Description + "<BR>"
                        Next
                    Next
                End If
            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Inspection Already Exist!!", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try


        Next
    End Sub
    Private Sub CopyMods()
        Dim mCompMonitorModStatusList As tmpCompMonitorModStatusList
        mCompMonitorModStatusList = tmpCompMonitorModStatusList.GetCompMonitorModStatusList(mRemCompStatus.AsOnDate.ToString, mRemCompStatus.CompID, False, , , , , , , mRemAssemblyStatus.MachineID.ToString, mRemAssemblyStatus.AssemblyID.ToString, mRemAssemblyStatus.ID.ToString)
        Dim mInstCompMonitorModStatus As CompMonitorModStatus

        For i As Integer = 0 To mCompMonitorModStatusList.Count - 1
            Dim mRemCompMonitorModStatus As CompMonitorModStatus
            'Commented and Added By Vikrant On 27-Jul-2020 For ALL27072020
            'mRemCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mCompMonitorModStatusList(i).ID, mRemAssemblyStatus.ID, mRemCompStatus.ID, mMachine.HourType)
            'mInstCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mInstCompStatus.CompID, mInstAssemblyStatus.ID, CStr(mInstAssemblyStatus.AsOnDate), mInstCompStatus.Comp.PartID, mInstAssemblyStatus.Assembly.ModelID, mInstCompStatus.ID, mMachine.HourType)
            mRemCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mCompMonitorModStatusList(i).ID, mRemAssemblyStatus.ID, mRemCompStatus.ID, mRemAssemblyStatus.HourType)
            mInstCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mInstCompStatus.CompID, mInstAssemblyStatus.ID, CStr(mInstAssemblyStatus.AsOnDate), mInstCompStatus.Comp.PartID, mInstAssemblyStatus.Assembly.ModelID, mInstCompStatus.ID, mRemAssemblyStatus.HourType)
            'End
            ''SetMachineMaintenanceObject(mInstCompMonitorModStatus.ID, mInstAssemblyStatus)

            mInstCompMonitorModStatus.CompMonitorModStatusPeriods.CopyCompMonitorModStatusPeriod(mCompMonitorModStatusList.Count - 1, mInstCompMonitorModStatus.ID, mInstCompMonitorModStatus.AssemblyStatusID, mRemCompMonitorModStatus.PartMonitorMod, Today.Date.ToString, mInstCompStatus.ID, mInstCompMonitorModStatus.IsMaster)
            With mInstCompMonitorModStatus
                mInstCompMonitorModStatus.PartMonitorModID(False) = mRemCompMonitorModStatus.PartMonitorModID
                .SourceDoc = mRemCompMonitorModStatus.SourceDoc
                .RevisionNo = mRemCompMonitorModStatus.RevisionNo
                .BookNo = mRemCompMonitorModStatus.BookNo
                .PageNo = mRemCompMonitorModStatus.PageNo
                .RequiredManHours = mRemCompMonitorModStatus.RequiredManHours
                .ExtensionDate = mRemCompMonitorModStatus.ExtensionDate
                .ApprovalRemark = mRemCompMonitorModStatus.ApprovalRemark
                .IsApplicable = mRemCompMonitorModStatus.IsApplicable
                .DoneBy = mRemCompMonitorModStatus.DoneBy
                .IsLater = mRemCompMonitorModStatus.IsLater

                ''For j As Integer = 0 To mRemCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
                ''    mInstCompMonitorModStatus.CompMonitorModStatusPeriods(j).RemainingValue = mRemCompMonitorModStatus.CompMonitorModStatusPeriods(j).RemainingValue
                ''    mInstCompMonitorModStatus.CompMonitorModStatusPeriods(j).ElapsedValue = mRemCompMonitorModStatus.CompMonitorModStatusPeriods(j).ElapsedValue
                ''    mInstCompMonitorModStatus.CompMonitorModStatusPeriods(j).ExtensionValue = mRemCompMonitorModStatus.CompMonitorModStatusPeriods(j).ExtensionValue
                ''Next

            End With

            Try
                If mInstCompMonitorModStatus.IsValid = True Then
                    mInstCompMonitorModStatus.ApplyEdit()
                    mInstCompMonitorModStatus = CType(mInstCompMonitorModStatus.Save(), CompMonitorModStatus)
                    SaveMachineMaintenance()
                    'MaintDetail = "Reg No. : " + mtmpInstalledCompList(mInstCompMonitorModStatus.CompStatusID).MachineInfo & " Assembly Info : " & mtmpInstalledCompList(mInstCompMonitorModStatus.CompStatusID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mtmpInstalledCompList(mInstCompMonitorModStatus.CompStatusID).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mInstCompMonitorModStatus.PartMonitorMod.MonitorTypeName.Replace(Environment.NewLine, " ")
                    MarkLog(Util.Action.Save, "Component Modification Status", MaintDetail, Util.ErrorType.NoError, mInstCompMonitorModStatus.ID, EventLogID)
                Else
                    Dim strMSG As String = ""
                    For k As Integer = 0 To mInstCompMonitorModStatus.GetBrokenRulesCollection.Count - 1
                        strMSG = strMSG + mInstCompMonitorModStatus.GetBrokenRulesCollection(k).Description + "<BR>"
                    Next
                    For m As Integer = 0 To mInstCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
                        For k As Integer = 0 To mInstCompMonitorModStatus.CompMonitorModStatusPeriods(m).GetBrokenRulesCollection.Count - 1
                            strMSG = strMSG + mInstCompMonitorModStatus.CompMonitorModStatusPeriods(m).GetBrokenRulesCollection(k).Description + "<BR>"
                        Next
                    Next
                End If
            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Directive Already Exist!!", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Next
    End Sub
    'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
    Private Sub addAttributes()
        txtInstCompStatusFanBladePosition.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtInstCompStatusFanBladePosition').value,event)")
        txtInstCompStatusMomentWeight.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtInstCompStatusMomentWeight').value,event)")
        txtInstCompStatusBalanceScrew.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtInstCompStatusBalanceScrew').value,event)")
    End Sub
    'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Remove
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        cmbReason.DataSource = mRemovalReasonList
        Session("mRemovalReasonList") = mRemovalReasonList
        dgRemovalValue.DataSource = mRemCompStatus.CompStatusPeriods
        'Added Code By Saylee 
        calRemove.Text = mRemCompStatus.RemovedOnFormatted.ToString

        'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        '========================================

        'Install

        mATAList = ATAList.GetATAList("", "(SELECT)")
        Session("mATAList") = mATAList
        cmbInstATAChapter.DataSource = mATAList
        mPartList = PartList.GetPartList(, , "(SELECT)")
        cmbInstPartNo.DataSource = mPartList
        Session("mPartList") = mPartList
        'Added By Vikrant On 27-Jul-2020 For ALL27072020
        Dim IsSpareAssembly As Boolean = False
        If Not Session("IsFromSpareWO") Is Nothing Then
            IsSpareAssembly = IIf(Session("IsFromSpareWO") = "True", True, False)
        End If
        'End
        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, , mRemCompStatus.RemovedOn.ToString, "(SELECT)", IsForSpareAssembly:=IsSpareAssembly)

        cmbInstAssemblyList.DataSource = mAssemblyList
        'cmbAssemblyList.DataSource = Flypal.MachineReadOnly.AssemblyStatusList.GetAssemblyStatusList(Guid.Empty, , , , , , , , , , , , , , , True)
        Session("mAssemblyList") = mAssemblyList

        If Not mRemCompStatus.AssemblyID.Equals(Guid.Empty) Then
            mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mRemCompStatus.AssemblyID, "")
            Session("mPeriodListForCompStatus") = mPeriodListForCompStatus

            mRemAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mPeriodListForCompStatus(0).AssemblyStatusID)
            Session("mRemAssemblyStatus") = mRemAssemblyStatus
        End If
        If Not mInstCompStatus Is Nothing Then
            dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
            calInstalledOn.Text = mInstCompStatus.InstalledOnFormatted.ToString
        End If
        'Added on 28-05-2007 by Kalpesh Shah


        ''Added by Saylee on 8th-Oct-2009
        'mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        'Session("mMachineMaintenanceList") = mMachineMaintenanceList

        DataBind()

        '=============Added by Saylee on 11th-Jan-2008 (Maintenance)==============================
        If cmbReason.Items.Contains(New System.Web.UI.WebControls.ListItem(mRemCompStatus.RemovalReasonName, mRemCompStatus.RemovalReasonID.ToString)) Then
            cmbReason.SelectedValue = mRemCompStatus.RemovalReasonID.ToString
        Else
            cmbReason.SelectedValue = Guid.Empty.ToString
        End If

        If cmbInstPartNo.Items.Contains(New System.Web.UI.WebControls.ListItem(mRemCompStatus.Comp.PartName, mRemCompStatus.Comp.PartID.ToString)) Then
            cmbInstPartNo.SelectedValue = mRemCompStatus.Comp.PartID.ToString
        Else
            cmbInstPartNo.SelectedValue = Guid.Empty.ToString
        End If

        If cmbInstATAChapter.Items.Contains(New System.Web.UI.WebControls.ListItem(mRemCompStatus.ATAChapter, mRemCompStatus.ATAID.ToString)) Then
            cmbInstATAChapter.SelectedValue = mRemCompStatus.ATAID.ToString
        Else
            cmbInstATAChapter.SelectedValue = Guid.Empty.ToString
        End If


        cmbInstAssemblyList.SelectedValue = mRemCompStatus.AssemblyID.ToString
        chkInstallation.Checked = mIsInstall

    End Sub
    Private Sub DataGridBind()
        Session("mRemCompStatus") = mRemCompStatus
        dgRemovalValue.DataSource = mRemCompStatus.CompStatusPeriods
        dgRemovalValue.DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 200 Then
                custValidator.ErrorMessage = "Max. length of Note should be 200 char."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf custValidator.ControlToValidate = "cmbReason" Then
            '    If cmbReason.SelectedIndex <= 0 Then
            '        custValidator.ErrorMessage = "Please select Reason from the list."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            ''    End If
        ElseIf custValidator.ControlToValidate = "cmbInstAssemblyList" Then
            If cmbInstAssemblyList.SelectedIndex = 0 And chkInstallation.Checked = True Then
                custValidator.ErrorMessage = "Please select the Assembly from the list."
                e.IsValid = False
            ElseIf cmbInstAssemblyList.SelectedIndex = 0 And chkInstallation.Checked = True Then

            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "calInstalledOn" Then
            If calInstalledOn.Text = "" And chkInstallation.Checked = True Then
                custValidator.ErrorMessage = "Installation date required"
                e.IsValid = False
            ElseIf calInstalledOn.Text <> "" And chkInstallation.Checked = True And calRemove.Text <> "" Then
                If CDate(calRemove.Text) > CDate(calInstalledOn.Text) Then
                    custValidator.ErrorMessage = "Installation Date should be later to Removal date"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtInstNote" Then
            If Len(txtInstNote.Text) > 200 Then
                custValidator.ErrorMessage = "Max length of Note should be 200 character."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation

        Dim str As String = ""
        If chkRemoval.Checked = True Then
            SetRemObject()
            If Not mRemCompStatus.IsValid Then
                For i As Integer = 0 To mRemCompStatus.GetBrokenRulesCollection.Count - 1
                    str = str + mRemCompStatus.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
            For i As Integer = 0 To CShort(dgRemovalValue.Rows.Count - 1) 'Change
                If Not mRemCompStatus.CompStatusPeriods.Item(i).IsValid Then
                    Dim x As Integer
                    For x = 0 To mRemCompStatus.CompStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                        str = str + mRemCompStatus.CompStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                    Next
                End If
            Next
        End If

        If chkInstallation.Checked = True Then
            SetInstObject()
            SetInstGridObject()
            If Not mInstCompStatus.IsValid Then
                For i As Integer = 0 To mInstCompStatus.GetBrokenRulesCollection.Count - 1
                    str = str + mInstCompStatus.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
            If mInstCompStatus.CompStatusPeriods.Count = 0 Then
                str = str + "Installation Component Status can not be saved without periods" + "<BR>"
            Else
                ' For i As Integer = 0 To CShort(dgRemovalValue.Items.Count - 1)
                For i As Integer = 0 To CShort(dgInstallationValue.Rows.Count - 1) 'Change
                    If Not mInstCompStatus.CompStatusPeriods.Item(i).IsValid Then
                        Dim x As Integer
                        For x = 0 To mInstCompStatus.CompStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                            str = str + mInstCompStatus.CompStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                        Next
                    End If
                Next
            End If
        End If
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes() 'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            'setFocus(btnSelectLog)
            calRemove.Focus()
            Session("mLogList") = Nothing
            chkRemoval.Checked = True
            mIsRemoval = chkRemoval.Checked
            Session("IsRemoval") = mIsRemoval
            calInstalledOn.Text = mRemCompStatus.RemovedOnFormatted.ToString
            If Session("IsFromAddPeriod") = False And CType(Session("FromLog"), Boolean) = False Then
                ' NewRecord(Guid.Empty, Today.Date.ToString)
                NewRecord(Guid.Empty, mRemCompStatus.RemovedOnFormatted.ToString)
            Else
                Session("IsFromAddPeriod") = False
            End If

            AddSelectedPeroids()
            DataFieldBind()
            FromLog()

            ControlVisibility(mIsRemoval, mIsInstall)

            SetPage()
        End If


    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ComponentRemovalNew") And mRemCompStatus.IsNew) Or (Not User.IsInRole("ComponentRemovalEdit") And Not mRemCompStatus.IsNew) Then
            SetRemObject()
            setSession()
            MarkLog(Util.Action.Save, "Component", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If Page.IsValid Then
            Try
                If chkRemoval.Checked And SaveRemoval() = True Then
                    Session.Remove("FromLog")
                    'MarkLog(Util.Action.Save, "ComponentRemoval", "( " + mMachine.RegNo + " / " + mRemAssemblyStatus.ModelName + " / " + mRemAssemblyStatus.Assembly.SerialNo + " ) " + mRemCompStatus.Comp.PartName + " / " + mRemCompStatus.Comp.SerialNo, Util.ErrorType.NoError, mRemCompStatus.ID)
                    'Added By Vikrant On 27-Jul-2020 For ALL27072020
                    Dim mRegNo As String = ""
                    If mInstAssemblyStatus.IsSpareAssembly = False Then
                        mRegNo = "Reg No. : " & mMachine.RegNo
                    End If
                    'End
                    MaintDetail = "Reg No. : " & mRegNo & " Assembly Info : " & mRemAssemblyStatus.ModelName + " " + mRemAssemblyStatus.Assembly.SerialNo & " Part Info : " & mRemCompStatus.Comp.PartName + " " + mRemCompStatus.Comp.Description + " " + mRemCompStatus.Comp.SerialNo
                    MarkLog(Util.Action.Remove, "Component Removal", MaintDetail, Util.ErrorType.NoError, mRemCompStatus.ID, EventLogID)
                    ''NewRecord(Guid.Empty, calInstalledOn.Text)
                    If chkInstallation.Checked = True And SaveInstall() = True Then
                        Try
                            If mInstCompStatus.Comp.PartID.Equals(mRemCompStatus.Comp.PartID) Then
                                CopyServices() 'copy Services from Removal comp to new Installed comp
                                CopyInspections() 'copy Inspections from Removal comp to new Installed comp
                                CopyMods() 'copy Mods from Removal comp to new Installed comp
                            End If
                            If IsExpiryServicePresent Then
                                MSGBoxCtrl.show("Alert!", "Expiry Service are not configred on newly installed component.", "Please configure Expiry service manually", MsgBoxStyle.OkOnly, "ExServiceConfig")
                                Exit Sub
                            End If
                           
                        Catch ex As Exception
                            'Throw ex
                        End Try
                    ElseIf SaveInstall() = False Then 'Change
                        IsValidRemovalInstallation = "False"
                    End If
                End If
            Catch ex As Exception

            Finally
                Session.Remove("FromLog")
                Session.Remove("IsInstall")
                'Response.Redirect(Request.QueryString("BackPage"))
            End Try
            If IsValidRemovalInstallation = "True" Then 'Change
                Session.Remove("IsExpiryServicePresent")
                'Added by vikrant on 06-Sep-2019 for ALL06092019
                Dim URLForWOCompliance As Stack = CType(Session("URLForWOCompliance"), Stack)
                If Not URLForWOCompliance Is Nothing Then
                    If URLForWOCompliance.Count > 0 Then
                        Response.Redirect(URLForWOCompliance.Peek.ToString)
                        Exit Sub
                    End If
                End If
                'End
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        Else
            upnlValidation.Update()
        End If

    End Sub
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        'SetRemObject()
        'Session.Remove("FromLog")
        ''FromCompRemovalInstall=4
        'Session.Remove("mLogList")
        'Response.Redirect("wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage6=wfRemInstComp.aspx&FromType=4&DoneOn=" & calRemove.Value.ToString & "&MachineId=" & mRemAssemblyStatus.MachineID.ToString & "&AssemblyStatusID=" & mRemAssemblyStatus.ID.ToString & "&AssemblyID=" & mRemAssemblyStatus.AssemblyID.ToString)
        SetRemObject()

        Session("mFromType") = 4
        Session("mMachineId") = mRemAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mRemAssemblyStatus.ID.ToString
        Session("mAssemblyID") = mRemAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(calRemove.Text = "", Today.Date.ToShortDateString, calRemove.Text))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
    End Sub
    'Private Sub imgbtnReason_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnReason.Click
    Private Sub imgbtnReason_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnReason.Click
        SetRemObject()
        Session.Remove("FromLog")
        ' Response.Redirect("wfRemovalReason_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfRemInstComp_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemovalReasonWindow", "OpenRemovalReasonWindow()", True)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetRemObject()
        ' MarkLog(Util.Action.Close, "CompRemoval", "", Util.ErrorType.NoError, Guid.Empty)
        Session.Remove("FromLog")
        Session.Remove("IsInstall")
        Session.Remove("IsExpiryServicePresent")
        ''If Request.QueryString("BackPage6") = "wfComplyCompMonitorServiceStatus.aspx" Then
        ''    Response.Redirect(Request.QueryString("BackPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        ''Else
        Response.Redirect(Request.QueryString("BackPage"))
        ''End If


    End Sub
    Private Sub calRemove_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calRemove.TextChanged
        If DateDiff(DateInterval.Day, SmartDate.StringToDate(mRemCompStatus.RemovedOn.ToString), SmartDate.StringToDate(calRemove.Text)) <> 0 Then
            SetRemObject()
            setSession()
            Dim clnCompStatus As CompStatus = mRemCompStatus.Clone
            If mFrom_Remove = From_Remove.NewRemove Then
                mRemCompStatus = CompStatus.NewRemovalCompStatus(mPrevCompStatus.ID, calRemove.Text, mRemAssemblyStatus.ID, Guid.Empty.ToString)
            Else
                mRemCompStatus = CompStatus.GetRemovalCompStatus(mPrevCompStatus.ID, mRemAssemblyStatus.ID, calRemove.Text, Guid.Empty.ToString)
            End If
            Session.Remove("mLog")
            SetFormClone(clnCompStatus)
            DataGridBind()
        End If
    End Sub
    Private Sub calInstalledOn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calInstalledOn.TextChanged
        If DateDiff(DateInterval.Day, SmartDate.StringToDate(mInstCompStatus.InstalledOn.ToString), SmartDate.StringToDate(calInstalledOn.Text)) <> 0 Then
            Dim LogID As Guid = Guid.Empty
            SetInstObject()
            setSession()
            Dim clnCompStatus As CompStatus = mInstCompStatus.Clone
            If mFrom_Inst = From_Inst.NewInstall Then
                mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mRemCompStatus.AssemblyID, mRemAssemblyStatus.ID, calInstalledOn.Text, False, Guid.Empty.ToString, Guid.Empty.ToString)
            Else
                mInstCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mRemAssemblyStatus.ID, calInstalledOn.Text, Guid.Empty.ToString)
            End If


            If mInstCompStatus.CompStatusPeriods.Count = 0 Then
                mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mInstCompStatus.AssemblyID, "")
                'Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(Guid.Empty.ToString, mRemovedCompStatus.PartName)
                Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(mRemCompStatus.Comp.PartID.ToString, mRemCompStatus.PartName)
                If mtmpCompListOnPartSelection.Count > 0 Then
                    Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mInstCompStatus.AssemblyID, "")
                    Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)

                    If CType(Session("FromLog"), Boolean) = True Then
                        LogID = New Guid(Request.QueryString("LogId"))
                        Dim LogDate As String = mInstCompStatus.InstalledOn.ToString 'Request.QueryString("LogDate")
                    End If
                    If LogID.Equals(Guid.Empty) Then
                        mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, calInstalledOn.Text, True, calInstalledOn.Text)
                    Else
                        mInstCompStatus.CompStatusPeriods.Add(mPeriodListForCompStatus, tmpCompStatus.CompStatusPeriods, calInstalledOn.Text, False, calInstalledOn.Text, LogID.ToString)
                    End If

                    dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                    dgInstallationValue.DataBind()
                End If

            End If
            Session.Remove("mLog")
            SetFormInstClone(clnCompStatus)
            Session("mInstCompStatus") = mInstCompStatus
            dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
            dgInstallationValue.DataBind()
            upnlInstallationDetail.Update()
        End If
    End Sub
    Private Sub chkInstallation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInstallation.CheckedChanged
        mIsInstall = chkInstallation.Checked
        Session("IsInstall") = mIsInstall
        ControlVisibility(mIsRemoval, mIsInstall)
    End Sub
    Private Sub chkRemoval_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mIsRemoval = chkRemoval.Checked
        Session("IsRemoval") = mIsRemoval
        ControlVisibility(mIsRemoval, mIsInstall)
    End Sub
    Private Sub cmbInstAssemblyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetAssemblyPeriod()
    End Sub

    Private Sub cmbInstPartNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInstPartNo.SelectedIndexChanged
        txtInstDescription.Text = IIf(cmbInstPartNo.SelectedIndex > 0, mPartList(cmbInstPartNo.SelectedIndex).Description, "")
        'txtInstDescription.DataBind()

        Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(cmbInstPartNo.SelectedValue.ToString, mPartList(New Guid(cmbInstPartNo.SelectedValue)).Name, mPartList(New Guid(cmbInstPartNo.SelectedValue)).Description)

        If mtmpCompListOnPartSelection.Count > 0 Then
            Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mInstCompStatus.AssemblyID, "")
            Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)
            txtCode.Text = tmpCompStatus.Comp.Code
            mInstCompStatus.Comp.PartID = tmpCompStatus.Comp.PartID
            mInstCompStatus.ATAID = tmpCompStatus.ATAID

            cmbInstATAChapter.SelectedValue = mInstCompStatus.ATAID.ToString
            cmbInstPartNo.SelectedValue = mInstCompStatus.Comp.PartID.ToString

            mtmpCompListOnPartSelection = Nothing
            tmpCompStatus = Nothing
        Else
            cmbInstATAChapter.SelectedIndex = 0
            cmbInstATAChapter.DataBind()
        End If
        SetAssemblyPeriod()
        If cmbInstPartNo.Enabled = True Then
            setFocus(cmbInstPartNo)
        End If
        If cmbInstPartNo.SelectedIndex = 0 Then
            cmbInstATAChapter.SelectedIndex = 0
            cmbInstATAChapter.DataBind()
        End If
        upnlInstallationDetail.Update()
    End Sub
    Private Sub btnAddPeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriod.Click

        If cmbInstAssemblyList.SelectedIndex = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Select an Assembly from the list.", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Select an Assembly from the list.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            Session("IsFromAddPeriod") = True
            Session("IsInstall") = chkInstallation.Checked
            Session("IsRemoval") = chkRemoval.Checked
            SetRemObject()
            SetInstObject()
            SetInstGridObject()
            SetInstPeroids()
            ' Response.Redirect("wfSelectPeriod.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage2=wfRemInstComp.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
        End If
    End Sub
    Protected Sub txtCompInstallationValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mInstCompStatus.CompStatusPeriods.Count - 1
            Dim txtCompInstVal As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtCompInstallationValue"), TextBox)
            If mInstCompStatus.CompStatusPeriods.Item(i).PeriodID = 2 Then
                If Period.IsDate(txtCompInstVal.Text) Then
                    mInstCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = txtCompInstVal.Text.Trim
                Else
                    mInstCompStatus.CompStatusPeriods.Item(i).CompInstallationValueFormatted = ""
                End If
            Else
                mInstCompStatus.CompStatusPeriods.Item(i).CompInstallationValue = txtCompInstVal.Text.Trim
            End If
        Next i
        dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
        dgInstallationValue.DataBind()
        Session("mInstCompStatus") = mInstCompStatus

        ControlVisibility(False, True)

        upnlInstallationDetail.Update()
    End Sub
    Private Sub dgInstallationValue_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstallationValue.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgInstallationValue.PageSize * dgInstallationValue.PageIndex

                ' If (Not User.IsInRole("ComponentInstallationEdit")) Then
                If (Not User.IsInRole("AssemblyInstallationDelete")) Then
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                REM: If monitoring entry is present for that particualr period then that period can not be deleted
                ' If mInstCompStatus.CompStatusPeriods.Item(index).HasMonitor = True Then
                If mInstCompStatus.CompStatusPeriods.Item(Index).HasMonitorCount(mInstCompStatus.ID, mInstCompStatus.CompStatusPeriods.Item(Index).PeriodID) = True Or mInstCompStatus.CompStatusPeriods.Item(Index).IsPeriodMonitored(mInstCompStatus.CompID, mInstCompStatus.CompStatusPeriods.Item(Index).PeriodID) = True Then
                    'Dim msg1 As New SIMsgBox(Page, "Removal Alert!", "Selected Component Period cannot be removed as monitor entry exist", "", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfRemInstComp.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    'msg1.Show()
                    MSGBoxCtrl.show("Removal Alert!", "Selected Component Period cannot be removed as monitor entry exist", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    SetInstGridObject()
                    mInstCompStatus.CompStatusPeriods.Remove(mInstCompStatus.CompStatusPeriods.Item(Index))
                    dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods
                    dgInstallationValue.DataBind()
                    Session("mInstCompStatus") = mInstCompStatus
                    If (Not mInstCompStatus.CompStatusPeriods.Contains(9) And Not mInstCompStatus.CompStatusPeriods.Contains(10)) Then
                        mInstCompStatus.Comp.ACF = 0D
                        mInstCompStatus.Comp.ECF = 0D
                        mInstCompStatus.Comp.FCF = 0D
                        ''txtACF.DataBind()
                        ''txtECF.DataBind()
                        ''txtFCF.DataBind()
                    End If
                    upnlInstallationDetail.Update()
                End If
        End Select
    End Sub
    Private Sub chkByModel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkByModel.CheckedChanged
        If chkByModel.Checked Then
            mPartList = PartList.GetPartList(mInstCompStatus.ModelID, , , "(SELECT)")
            Session("mPartlist") = mPartList
            cmbInstPartNo.DataSource = mPartList
        Else
            mPartList = PartList.GetPartList("", "", "(SELECT)")
            Session("mPartList") = mPartList
            cmbInstPartNo.DataSource = mPartList
        End If
        dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods

        '=============Added by Saylee for bug-IIC21 (Maintenance)==============================
        If cmbInstPartNo.Items.Contains(New System.Web.UI.WebControls.ListItem(mInstCompStatus.Comp.PartName, mInstCompStatus.Comp.PartID.ToString)) Then
            cmbInstPartNo.SelectedValue = mInstCompStatus.Comp.PartID.ToString
        Else
            cmbInstPartNo.SelectedValue = Guid.Empty.ToString
        End If

        If cmbInstATAChapter.Items.Contains(New System.Web.UI.WebControls.ListItem(mInstCompStatus.ATAChapter, mInstCompStatus.ATAID.ToString)) Then
            cmbInstATAChapter.SelectedValue = mInstCompStatus.ATAID.ToString
        Else
            cmbInstATAChapter.SelectedValue = Guid.Empty.ToString
        End If
        '=======================================================================================
        DataBind()
        upnlInstallationDetail.Update()
    End Sub
    Private Sub hdnAddPeriod_Click(sender As Object, e As System.EventArgs) Handles hdnAddPeriod.Click
        AddSelectedPeroids()
        DataFieldBind()
        FromLog()

        ControlVisibility(mIsRemoval, mIsInstall)

        SetPage()
        upnlInstallationDetail.Update()
        upnlRemovalDetail.Update()
    End Sub
    Private Sub hdnBtnSelectLog_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSelectLog.Click
        FromLog()
        ControlVisibility(chkRemoval.Checked, chkInstallation.Checked)
        upnlInstallationDetail.Update()
        upnlRemovalDetail.Update()
    End Sub
    Private Sub hdnBtnRemovalReason_Click(sender As Object, e As System.EventArgs) Handles hdnBtnRemovalReason.Click
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        cmbReason.DataSource = mRemovalReasonList
        Session("mRemovalReasonList") = mRemovalReasonList
        cmbReason.DataBind()
        If cmbReason.Items.Contains(New System.Web.UI.WebControls.ListItem(mRemCompStatus.RemovalReasonName, mRemCompStatus.RemovalReasonID.ToString)) Then
            cmbReason.SelectedValue = mRemCompStatus.RemovalReasonID.ToString
        Else
            cmbReason.SelectedValue = Guid.Empty.ToString
        End If
        upnlRemovalDetail.Update()
    End Sub
    'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
    Private Sub hdnBtnInstallSelected_Click(sender As Object, e As System.EventArgs) Handles hdnBtnInstallSelected.Click, hdnBtnSpareCompInstallList.Click
        cmbInstATAChapter.SelectedValue = mInstCompStatus.ATAID.ToString
        cmbInstPartNo.SelectedValue = mInstCompStatus.Comp.PartID.ToString
        txtInstDescription.Text = mInstCompStatus.Description
        txtInstSerialNo.Text = mInstCompStatus.SerialNo
        txtInstPosition.Text = mInstCompStatus.Position
        dgInstallationValue.DataSource = mInstCompStatus.CompStatusPeriods

        cmbInstATAChapter.DataBind()
        cmbInstPartNo.DataBind()
        txtCode.DataBind()
        txtInstDescription.DataBind()
        txtInstSerialNo.DataBind()
        txtInstPosition.DataBind()
        dgInstallationValue.DataBind()

        cmbInstPartNo.Focus()
        upnlRemovalDetail.Update()
        upnlInstallationDetail.Update()
    End Sub
    Private Sub lnkInstallSelected_Click(sender As Object, e As System.EventArgs) Handles lnkInstallSelected.Click
        Session("InstalledOnMachineID") = mAssemblyList(New Guid(cmbInstAssemblyList.SelectedValue)).MachineID.ToString
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenInstallSelectedWindow", "OpenInstallSelectedWindow();", True)
    End Sub
    Private Sub lnkInstallSpareComponent_Click(sender As Object, e As System.EventArgs) Handles lnkInstallSpareComponent.Click
        Session("IsOpenFromWO") = "True"
        Session("InstalledOnAssembly") = mAssemblyList(New Guid(cmbInstAssemblyList.SelectedValue)).ID.ToString
        Session("InstalledOnDate") = calInstalledOn.Text
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenSpareCompInstallListWindow", "OpenSpareCompInstallListWindow();", True)
    End Sub
    'End
    'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
    Private Sub chkInstCompStatusFanBladeMonitoring_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInstCompStatusFanBladeMonitoring.CheckedChanged
        If chkInstCompStatusFanBladeMonitoring.Checked = True Then
            txtInstCompStatusFanBladePosition.Enabled = True
            txtInstCompStatusFanBladePosition.Text = txtRemCompStatusFanBladePosition.Text
            txtInstCompStatusMomentWeight.Enabled = True
            txtInstCompStatusMomentWeight.Text = txtRemCompStatusMomentWeight.Text
            txtInstCompStatusBalanceScrew.Enabled = True
            txtInstCompStatusBalanceScrew.Text = txtRemCompStatusBalanceScrew.Text
            mInstCompStatus.IsFanBladeDistribution = chkInstCompStatusFanBladeMonitoring.Checked
            mInstCompStatus.FanBladePosition = Val(txtInstCompStatusFanBladePosition.Text)
            mInstCompStatus.MomentWeight = CDec(txtInstCompStatusMomentWeight.Text)
            mInstCompStatus.BalanceScrew = Val(txtInstCompStatusBalanceScrew.Text)
        ElseIf chkInstCompStatusFanBladeMonitoring.Checked = False Then
            txtInstCompStatusFanBladePosition.Text = "0"
            txtInstCompStatusMomentWeight.Text = "0"
            txtInstCompStatusBalanceScrew.Text = "0"
            txtInstCompStatusFanBladePosition.Enabled = False
            txtInstCompStatusMomentWeight.Enabled = False
            txtInstCompStatusBalanceScrew.Enabled = False
            mInstCompStatus.IsFanBladeDistribution = chkInstCompStatusFanBladeMonitoring.Checked
            mInstCompStatus.FanBladePosition = Val(txtInstCompStatusFanBladePosition.Text)
            mInstCompStatus.MomentWeight = CDec(txtInstCompStatusMomentWeight.Text)
            mInstCompStatus.BalanceScrew = Val(txtInstCompStatusBalanceScrew.Text)
        End If
    End Sub
    'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
#End Region

#Region " Report "
    'Created By :- Pallavi , Date -10/08/2006
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If (Not User.IsInRole("ComponentRemovalPrint")) Then
            'MarkLog(Util.Action.Print, "CompRemoval", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfRemInstComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Rpt = New crDetInstallRemoveComp
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Assembly and Component Info
        Dim LHCount As Integer
        LHCount = 5
        ReportDetails.Add(New rptStatus(, 0, lblCompRemInfo.Text))
        Dim I As Integer
        For I = 0 To LHCount - 1
            If I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, , lblPartNo.Text, _
    txtPartNo.Text, , , , , , , , , , , , , , , , , "", _
    "", "", , ""))
            ElseIf I = 1 Then
                ReportDetails.Add(New rptStatus(, 1, , lblDescription.Text, _
      txtDescription.Text, , , , , , , , , , , , , , , , , "", _
      "", "", , ""))
            ElseIf I = 2 Then
                ReportDetails.Add(New rptStatus(, 1, , lblSerialNo.Text, _
   txtSerialNo.Text, , , , , , , , , , , , , , , , , "", _
   "", "", , ""))
            ElseIf I = 3 Then
                ReportDetails.Add(New rptStatus(, 1, , lblCode.Text, _
    txtCode.Text, , , , , , , , , , , , , , , , , "", _
    "", "", , ""))
            ElseIf I = 4 Then
                ReportDetails.Add(New rptStatus(, 1, , lblPosition.Text, _
   txtPosition.Text, , , , , , , , , , , , , , , , , "", _
   "", "", , ""))
            End If
        Next

        'For Removal Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 4
        RHCount1 = Me.mRemCompStatus.CompStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If
        ReportDetails.Add(New rptStatus(, 2, , , , , , lblRemovalInfo.Text, , , , , , , , , , , , , , lblRemovalValues.Text))
        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 3, , , , lblAssembly.Text, _
             New SmartDate(calRemove.Text.ToString).FormattedText, , , , , , , , , , , , , , , , _
            dgRemovalValue.Columns.Item(0).HeaderText, dgRemovalValue.Columns.Item(1).HeaderText, _
            , dgRemovalValue.Columns.Item(2).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 3, , , , lblAssembly.Text, _
                            New SmartDate(calRemove.Text.ToString).FormattedText, , , , , , , , , , , , , , , , "", "", , ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text, _
                     txtWorkOrderNo.Text, , , , , , , , , , , , , , , , _
                CType(Me.mRemCompStatus.CompStatusPeriods(m).PeriodName, String), _
                CType(Me.mRemCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                CType(Me.mRemCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text, _
                  txtWorkOrderNo.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblReason.Text, _
                     cmbReason.SelectedItem.Text, , , , , , , , , , , , , , , , _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).PeriodName, String), _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblReason.Text, _
                                     cmbReason.SelectedItem.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text, _
                     txtNote.Text, , , , , , , , , , , , , , , , _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).PeriodName, String), _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text, _
                                    txtNote.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , "", _
                    "", "", "", , , , , , , , , , , , , , , , _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).PeriodName, String), _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                    CType(Me.mRemCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , "", _
                                          "", "", "", , , , , , , , , , , , , , , , "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 4, , "", _
                 "", "", "", , , , , , , , , , , , , , , , _
                 CType(Me.mRemCompStatus.CompStatusPeriods(m).PeriodName, String), _
                 CType(Me.mRemCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                CType(Me.mRemCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Remove Component Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "CompRemoval", RemovalComp + " -> " + "Remove Component Status Detail Report", Util.ErrorType.NoError, mRemCompStatus.ID)
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    End Sub
#End Region

#End Region

    
    
    
    
End Class