'Added By Vikrant On 27-Jul-2020 For ALL27072020
Public Class wfSpareCompListForInstallation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mSpareCompList As SpareCompList
    Public mAssemblyStatus As AssemblyStatus
    Dim EventLogID As Guid
    Public mAssemblyType As String
    Dim mFileAttach As FileAttach
    Dim mMachine As Machine
    Dim mInstalledAssemblyStatusList As tmpInstalledAssemblyList
    Public mAssemblyList As AssemblyList
    Dim mCompStatus As CompStatus
    Dim ModelID As Guid
    Dim mRemovedCompStatus As CompStatus
    Dim IsOpenFromWO As String = "" 'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mSpareCompList = CType(Session("mSpareCompList"), SpareCompList)
        mAssemblyList = Session("mAssemblyList")
        IsOpenFromWO = IIf(Session("IsOpenFromWO") Is Nothing, "", Session("IsOpenFromWO")) 'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
    End Sub
    Public Sub SetAssemblyPeriod()
        Dim mPeriodListForCompStatus As PeriodListForCompStatus
        Dim mtmpCompListOnPartSelection As tmpCompListOnPartSelection = tmpCompListOnPartSelection.GetCompListOnPartSelection(mCompStatus.Comp.PartID.ToString, mCompStatus.Comp.PartName, mCompStatus.Comp.Description)

        If mtmpCompListOnPartSelection.Count > 0 Then
            Dim tmpPeriodListForCompStatus As PeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mCompStatus.AssemblyID, "")
            Dim tmpCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpCompListOnPartSelection(0).ID, tmpPeriodListForCompStatus(0).AssemblyStatusID, mtmpCompListOnPartSelection(0).InstalledOn.ToString)

            mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).AssemblyStatusID, "")
            Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
            If mCompStatus.IsNew Then 'If mFrom = From.NewInstall Then
                Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                NewRecord(Guid.Empty, txtInstallationDate.Text, , mPeriodListForCompStatus)
                CopyFromClone(clnCompStatus, mCompStatus)
            Else
                Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
                mCompStatus = CompStatus.GetInstallCompStatus(clnCompStatus.ID, mAssemblyStatus.ID, txtInstallationDate.Text, Guid.Empty.ToString)
                CopyFromClone(clnCompStatus, mCompStatus)
            End If
            tmpPeriodListForCompStatus = Nothing
        Else
            mPeriodListForCompStatus = PeriodListForCompStatus.GetPeriodListForCompStatus(New Guid(cmbAssemblyList.SelectedValue), "")
            Session("mPeriodListForCompStatus") = mPeriodListForCompStatus
            Dim clnCompStatus As CompStatus = CType(mCompStatus.Clone, CompStatus)
            NewRecord(Guid.Empty, txtInstallationDate.Text)
            CopyFromClone(clnCompStatus, mCompStatus)
        End If
    End Sub
    Private Sub NewRecord(ByVal LogID As Guid, ByVal mCurrentDate As String, Optional ConsiderAssemblyInstValue As Boolean = False, Optional ByVal mPeriodListForCompStatus As PeriodListForCompStatus = Nothing)
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

        Dim clnRemovedCompStatus As CompStatus = mRemovedCompStatus.Clone
        mRemovedCompStatus = CompStatus.GetCompStatus(clnRemovedCompStatus.ID, mAssemblyStatus.ID, mCurrentDate)
        mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, clnRemovedCompStatus.AssemblyID, mAssemblyStatus.ID, mCurrentDate, True, clnRemovedCompStatus.ID.ToString, LogID.ToString, ConsiderAssemblyInstValue)
        clnRemovedCompStatus = Nothing
        Session("mRemovedCompStatus") = mRemovedCompStatus
        mCompStatus.ModelID = mAssemblyStatus.Assembly.ModelID
        ModelID = mAssemblyStatus.Assembly.ModelID
        Session("ModelID") = ModelID
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub CopyFromClone(ByVal cln As CompStatus, ByVal mCompStatus As CompStatus)
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
    Public Function CheckPeriodsForRemovedCompStatus(ByVal RemovedCompStatus As CompStatus) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        Dim mAssemblyStatusList As AssemblyStatusList
        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtInstallationDate.Text, mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).MachineID.ToString, , , , , , , , , , True, , , mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).ID.ToString, , , , , , , , , , , , , , , , , MonitoringInspRequired:=False, MonitoringServiceRequired:=False, MonitoringModRequired:=False).Item(0), MachineInfo).AssemblyStatusList()

        '  For j As Integer = 0 To mAssemblyStatusList.Count - 1
        If mAssemblyStatusList(0).AssemblyID.Equals(mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).ID) Then
            Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusList(0).ID)
            While i <= RemovedCompStatus.CompStatusPeriods.Count - 1
                If tmpAssemblyStatus.AssemblyStatusPeriods.Contains(RemovedCompStatus.CompStatusPeriods(i).PeriodID) Then
                    tmpIsPeriodExists = True
                Else
                    tmpIsPeriodExists = False
                    Exit While
                End If
                i = i + 1
            End While
        End If
        '   Next

        Return tmpIsPeriodExists
    End Function
    Private Sub RemoveSession()
        Session.Remove("mSpareCompList")
        'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
        If Not Session("IsOpenFromWO") Is Nothing Then
            If Session("IsOpenFromWO") = "True" Then
                cmbAssemblyList.Enabled = False
                txtInstallationDate.Enabled = False
            Else 'Existing Condition
                Session.Remove("mAssemblyList")
            End If
        Else 'Existing Condition
            Session.Remove("mAssemblyList")
        End If
        'End

    End Sub
    Private Sub ControlVisibility()
        'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
        If Not Session("IsOpenFromWO") Is Nothing Then
            If Session("IsOpenFromWO") = "True" Then
                cmbAssemblyList.Enabled = False
                txtInstallationDate.Enabled = False
            End If
        End If
        'End
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes


                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '   DataFieldBind()
        End If
    End Sub
    Public Function CheckPeriodsForRemovedAssemblyStatus(ByVal RemovedAssemblyStatus As AssemblyStatus, ByVal Mac As Machine) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        If RemovedAssemblyStatus.AssemblyTypeID = 2 Or RemovedAssemblyStatus.AssemblyTypeID = 4 Then Return True
        While i <= RemovedAssemblyStatus.AssemblyStatusPeriods.Count - 1
            If Mac.AssemblyStatus.AssemblyStatusPeriods.Contains(RemovedAssemblyStatus.AssemblyStatusPeriods(i).PeriodID) Then
                tmpIsPeriodExists = True
            Else
                tmpIsPeriodExists = False
                Exit While
            End If
            i = i + 1
        End While
        Return tmpIsPeriodExists
    End Function
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        txtInstallationDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

        mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0, , Today.Date.ToString, "", True, , , True)
        cmbAssemblyList.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList

        mSpareCompList = SpareCompList.GetSparedCompList(IsPeriodValuesRequired:=True)
        Session("mSpareCompList") = mSpareCompList
        dgBuiltSpareList.DataSource = mSpareCompList

        DataBind()
        'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
        If Not Session("IsOpenFromWO") Is Nothing Then
            If Session("IsOpenFromWO") = "True" Then
                cmbAssemblyList.SelectedValue = Session("InstalledOnAssembly").ToString
                txtInstallationDate.Text = Session("InstalledOnDate").ToString
            End If
        End If
        'End
        lblBuiltSpareComp.Text = "List of Built Component " & " : " & mSpareCompList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgBuiltSpareList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgBuiltSpareList.PageIndexChanging
        dgBuiltSpareList.PageIndex = e.NewPageIndex
        dgBuiltSpareList.DataSource = mSpareCompList
        Session("mSpareCompList") = mSpareCompList
        dgBuiltSpareList.DataBind()
    End Sub
    Private Sub dgBuiltSpareList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgBuiltSpareList.RowCommand
        Dim Index As Int32
        Dim mAssemblyDetail As String
        Dim mRemovedAssemblyStatus As AssemblyStatus

        Select Case e.CommandName
            Case "InstallSelected"
                Index = CInt(e.CommandArgument) + dgBuiltSpareList.PageSize * dgBuiltSpareList.PageIndex
                Dim Id As Guid = New Guid(dgBuiltSpareList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                If (Not User.IsInRole("ComponentInstallationNew")) Then
                     MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '''mCompStatusInfo = mCompStatusList(New Guid(dgRemovedList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
                'Added By Utkarsh ON 04-Apr-2013 FOR ALL04042013
                mRemovedCompStatus = CompStatus.GetSpareCompStatus(Id, True)
                'End
                'Added by Saylee on 19-Mar-2013 for ALL14032013-1
                'Changed By Utkarsh ON 04-Apr-2013 FOR ALL04042013 (if condition & Message text)
                If CheckPeriodsForRemovedCompStatus(mRemovedCompStatus) = False Then
                    MSGBoxCtrl.show("Component Status Installation Alert!", "Periods for " & mRemovedCompStatus.PartNameSerialNo & " are mismatching with selected " & " Assembly on " & cmbAssemblyList.SelectedItem.Text & " .Can not be installed.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).ID, mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).AssemblyStatusID, txtInstallationDate.Text, True, Id.ToString, Guid.Empty.ToString)
                    '---
                    Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).AssemblyStatusID)
                    Dim mMachine As Machine = Machine.GetMachine(mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).MachineID)

                    Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1) 'Sort = 1 : Installation
                    Session("mFileAttach") = mFileAttach
                    '---28-Apr-2009
                    Session("IsAdded") = "False"
                    Session("InstallOnId") = mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).MachineID.ToString
                    Session("mInstallOnAssemblyID") = mAssemblyList(New Guid(cmbAssemblyList.SelectedValue)).ID.ToString
                    '---28-Apr-2009

                    Session("From") = 1 'NewInstall
                    Session("InstallSelected") = 1
                    Session("mCompStatus") = mCompStatus
                    Session("mRemovedCompStatus") = mRemovedCompStatus
                    Session("mAssemblyStatus") = mAssemblyStatus
                    Session("mMachine") = mMachine

                    ''NewMachineMaintenance() 'Added by Saylee on 8th-Oct-2009

                    'Changed By Utkarsh On 26-Jul-2011 For All19072011
                    Dim MaintDetail As String
                    MaintDetail = "Compnent Info. : " + mSpareCompList(Id).ItemNameDescriptionSerialNo
                    MarkLog(Util.Action.Install, "Component Installation", MaintDetail, Util.ErrorType.NoError, mRemovedCompStatus.ID, EventLogID)
                    'End
                    'SetAssemblyPeriod()

                    'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
                    If Not Session("IsOpenFromWO") Is Nothing Then
                        If Session("IsOpenFromWO") = "True" Then
                            Dim mInstCompStatus As CompStatus
                            mInstCompStatus = CType(Session("mInstCompStatus"), CompStatus)

                            mInstCompStatus.ATAID = mCompStatus.ATAID
                            mInstCompStatus.Comp.PartID = mCompStatus.Comp.PartID
                            mInstCompStatus.Position = mCompStatus.Position
                            mInstCompStatus.CompID = mCompStatus.CompID
                            Session("mInstCompStatus") = mInstCompStatus
                            RemoveSession()
                            Dim mopenas As String = Request.QueryString("Type")
                            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                Exit Sub
                            End If
                        Else 'Existing Condition
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfInstallComp_AJAX.aspx?GChildPage2=Index.aspx');", True)
                        End If
                    Else 'Existing Condition
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfInstallComp_AJAX.aspx?GChildPage2=Index.aspx');", True)
                    End If
                    'End



                End If
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgBuiltSpareList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgBuiltSpareList.Sorting
        mSpareCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mSpareCompList") = mSpareCompList
        dgBuiltSpareList.DataSource = mSpareCompList
        dgBuiltSpareList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub dgBuiltSpareList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgBuiltSpareList.Columns(i).HeaderText
            Next
        End If
    End Sub
#End Region

End Class