Imports System.Linq

Public Class wfAssemblyMonitorServiceStatusNew_Ajax
    Inherits Page

#Region " Variable Declaration "
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Public Flag As Int16
    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 13th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 13th-Oct-2009
    Dim EventLogID As Guid 'Added by Saylee on 22-July-2011
    Dim MachineDetail As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public mIsSpareAssembly As Integer 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 22-07-2023
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyMonitorServiceStatus = CType(Session("mAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorServiceStatusList = CType(Session("mAssemblyMonitorServiceStatusList"), tmpAssemblyMonitorServiceStatusList)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 13th-Oct-2009
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorServiceStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 9th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 9th-Oct-2009
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'MLNo
    Public Sub SetLicenceCount()
        If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility()
        If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 3 Then 'for MonitorTypeID=NoFrequency 
            REM:1)for elapsed value
            dgCurrentValue.Columns(2).Visible = False
            REM:2)for Remaining Value
            dgCurrentValue.Columns(3).Visible = False
            REM:3) for Due On VAlue
            ' dgDoneOnValue.Columns(4).Visible = False
            dgDoneOnValue.Columns(5).Visible = False
            REM:3) for Due At Airframe VAlue
            'dgDoneOnValue.Columns(5).Visible = False
            dgDoneOnValue.Columns(6).Visible = False
        End If
        'Added By Utkarsh ON 28-Jun-2013 FOR ALL28062013
        ' dgDoneOnValue.Columns(5).Visible = (mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        dgDoneOnValue.Columns(6).Visible = (mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        'End
        btnPrint.Enabled = Not mAssemblyMonitorServiceStatus.IsNew
        btnSelectMonitoringService.Enabled = mAssemblyMonitorServiceStatus.IsNew

        If mAssemblyMonitorServiceStatus.ModelMonitorService.ID.Equals(Guid.Empty) Then
            txtDoneOnDate.BackColor = Color.Gainsboro
            txtDoneOnDate.Enabled = False
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True
            txtWorkOrNo.BackColor = Color.Gainsboro
            txtWorkOrNo.ReadOnly = True
            chkboxApplicable.Enabled = False
        End If

        If txtRemark.ReadOnly Then txtRemark.BackColor = Color.Gainsboro
        If txtWorkOrNo.ReadOnly Then txtWorkOrNo.BackColor = Color.Gainsboro
        If mAssemblyMonitorServiceStatus.EnableDoneOn = False Then txtDoneOnDate.Enabled = False

        If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If

        btnSelectLog.Visible = (mIsSpareAssembly <> 1) ' Added By Vikrant On 27-Jul-2020 For ALL27072020

        'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
        btnRevise.Enabled = (mAssemblyMonitorServiceStatus.IsApplicable And Not mAssemblyMonitorServiceStatus.IsNew)

        ControlVisibilityForAttachment()
    End Sub
    Private Sub SetTitle()

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "Maintenance Event"
            lblMonitorServiceType.InnerText = "Task Type"
        Else
            ServiceMPDTitle = "Assembly Service Status"
        End If


        If mAssemblyMonitorServiceStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + ServiceMPDTitle & " [Model : " & mAssemblyStatus.ModelName & " Serial No. :" & mAssemblyStatus.Assembly.SerialNo & "] [New]"
        Else
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + ServiceMPDTitle & " [Model : " & mAssemblyStatus.ModelName & " Serial No. :" & mAssemblyStatus.Assembly.SerialNo & "] "
        End If
        lblAssemblyValues.InnerText = mAssemblyStatus.AssemblyTypeName + " Values"
        upnlTitle.Update()
    End Sub
    Private Sub SetObject()
        With mAssemblyMonitorServiceStatus
            If txtDoneOnDate.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = txtDoneOnDate.Text
            End If
            .DoneWONo = Trim(txtWorkOrNo.Text)
            .DoneRemark = Trim(txtRemark.Text)
            .SourceDoc = Trim(txtSourceDoc.Text)
            .RevisionNo = Trim(txtRevisionNo.Text)
            .BookNo = Trim(txtBookNo.Text)
            .PageNo = Trim(txtPageNo.Text)
            .RequiredManHours = Trim(txtRequiredManHours.Text)
            If txtExtensionDate.Text = "" Then
                .ExtensionDate = System.DBNull.Value
            Else
                .ExtensionDate = txtExtensionDate.Text
            End If
            .ApprovalRemark = txtApprovalRemark.Text
            .IsApplicable = chkboxApplicable.Checked
            .IsLater = chkIsLater.Checked          'Added By Prashant 17-Aug-2010
            Dim LicenseNo As String = String.Empty 'Added By Prashant On 12-Jun-2012 FOR ALL08062012
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .LicenseNo = LicenseNo
            .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            .Place = txtPlace.Text.Trim 'End
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If
            If mAssemblyMonitorServiceStatus.IsNew Then mAssemblyMonitorServiceStatus.IsMaster = False 'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        End With

        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, calDoneOn, txtDueOnValue, txtExtensionValue As TextBox
        If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3 Then
            For i As Integer = 0 To Me.dgCurrentValue.Rows.Count - 1
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            Next i
        End If
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            calDoneOn = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox) 'Added By Shital on 25-Jan-2021
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(j).CurrentValue = Trim(calDoneOn.Text)
                End If
                'Added By Shital on 25-Jan-2021
                If txtExtensionValue Is Nothing Then
                    .Item(j).ExtensionValue = ""
                Else
                    .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)  'Added By Saylee on 28-07-2008 Shital on 25-Jan-2021
                End If
            End With
        Next j
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
    End Sub
    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then

            Select Case Result1

                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then

                        Try

                            If Save() = True Then

                                SetTitle()
                                UpdatePanel()

                            End If

                        Catch ex As SqlException

                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub

                        End Try

                    ElseIf MSGBoxCtrl.Sender = "ReviseActivity" Then     'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity

                        MarkLog(Action:=Action.[New],
                                ModuleName:="Model Service",
                                Detail:="",
                                ErrorType:=ErrorType.NoError,
                                TransID:=Guid.Empty,
                                EventLogID)

                        Dim ID As Guid = Guid.NewGuid
                        Dim mModelMonitorService As ModelMonitorService
                        Dim mModelMonitorServiceList As ModelMonitorServiceList

                        mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(ModelID:=mAssemblyMonitorServiceStatus.
                                                                                                                    ModelMonitorService.ModelID,
                                                                                                      GetRecordsByPreviousRefID:=True,
                                                                                                      PreviousRefID:=mAssemblyMonitorServiceStatus.
                                                                                                                        ModelMonitorService.
                                                                                                                        PreviousRefID.ToString())

                        If mModelMonitorServiceList.Count > 1 Then

                            For i As Integer = mModelMonitorServiceList.Count - 1 To 0 Step -1

                                If mModelMonitorServiceList(i).ID.Equals(mAssemblyMonitorServiceStatus.ModelMonitorService.ID) Then
                                    Exit For
                                Else
                                    Session("ModelIDFromModelCreation") = mAssemblyMonitorServiceStatus.ModelMonitorService.ModelID
                                    Session("ModelNameFromModelCreation") = mAssemblyMonitorServiceStatus.ModelMonitorService.Model.Name
                                    Session("mModelMonitorServiceList") = mModelMonitorServiceList
                                    Session("ModelMonitorServiceIDToBeLinked") = mModelMonitorServiceList(i).ID.ToString
                                    Session("ModelMonitorServicePreviousRefIDToBeLinked") = mModelMonitorServiceList(i).
                                                                                                    PreviousRefID.ToString()
                                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                                    Session("PreviousAssemblyMonitorServiceStatusForRevise") = mAssemblyMonitorServiceStatus

                                    ScriptManager.RegisterStartupScript(page:=Me,
                                                                        type:=[GetType],
                                                                        key:="OpenScript",
                                                                        script:="openledgersame('wfModelMonitorServiceList_Ajax.aspx?BackPage=Index.aspx');",
                                                                        addScriptTags:=True)

                                    Exit Sub

                                End If

                            Next

                        End If

                        mModelMonitorService = ModelMonitorService.NewModelMonitorService(OldModelMonitorService:=mAssemblyMonitorServiceStatus.
                                                                                                                    ModelMonitorService,
                                                                                          HourType:=mMachine.HourType)

                        Session("mModelMonitorService") = mModelMonitorService
                        RemoveSession()
                        mModelMonitorService.BeginEdit()
                        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                        Session("PreviousAssemblyMonitorServiceStatusForRevise") = mAssemblyMonitorServiceStatus
                        Session("IsLinkedActivitySelected") = True

                        ScriptManager.RegisterStartupScript(page:=Me,
                                                            type:=Me.GetType,
                                                            key:="ModelMonitorMasterWindow",
                                                            script:="OpenModelMonitorMasterWindow();",
                                                            addScriptTags:=True)

                    End If

                Case MsgBoxResult.No

                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("Sender") = ""
                        UpdatePanel()
                        'Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    End If

                    If MSGBoxCtrl.Sender = "ReviseActivity" Then

                        MarkLog(Action.Close,
                                "Assembly Service Status",
                                "",
                                ErrorType.NoError,
                                Guid.Empty,
                                EventLogID)

                        RemoveSession()
                        Response.Redirect(Request.QueryString("BackPage"))

                    End If

                Case MsgBoxResult.Ok

                    If MSGBoxCtrl.Sender = "Status" Then
                    End If

            End Select

        End If

    End Sub
    Private Function Save() As Boolean
        Dim AssemblyMonitorServiceStatusClone As AssemblyMonitorServiceStatus
        AssemblyMonitorServiceStatusClone = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mAssemblyMonitorServiceStatus.IsValid = True Then
            If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Assembly Service.Record cannot save without Periods.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            'aded By Deven on 24-Sep-2009 ------
            Dim mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
            mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString, ModelMonitorServiceID:=mAssemblyMonitorServiceStatus.ModelMonitorServiceID.ToString)
            If mAssemblyMonitorServiceStatusList.Contains(mAssemblyMonitorServiceStatus.ModelMonitorServiceID) And mAssemblyMonitorServiceStatus.IsNew = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Assembly Service Status.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            '---------------------------------------
            Try
                'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                If Not mAssemblyMonitorServiceStatus.DoneByID.Equals(Guid.Empty) AndAlso mAssemblyMonitorServiceStatus.DoneOn.ToString.Length > 0 Then
                    Dim Title As String = "Save Alert !"
                    Dim Message As String = ""
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorServiceStatus.DoneByID.ToString, mAssemblyMonitorServiceStatus.DoneOn.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
                        Return False
                    End If
                End If
                'End
                mAssemblyMonitorServiceStatus.ApplyEdit()
                mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)

                'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                If Not Session("PreviousAssemblyMonitorServiceStatusForRevise") Is Nothing Then

                    Dim IsLinkedActivitySelected As Boolean = IIf(Session("IsLinkedActivitySelected") Is Nothing,
                                                                  False,
                                                                  Session("IsLinkedActivitySelected"))
                    If IsLinkedActivitySelected Then

                        Dim mPreviousAssemblyMonitorServiceStatusForRevise As AssemblyMonitorServiceStatus

                        mPreviousAssemblyMonitorServiceStatusForRevise = Session("PreviousAssemblyMonitorServiceStatusForRevise")
                        mPreviousAssemblyMonitorServiceStatusForRevise.IsApplicable = False

                        mPreviousAssemblyMonitorServiceStatusForRevise.Save()
                        Session.Remove("PreviousAssemblyMonitorServiceStatusForRevise")

                    End If

                End If

                    SaveAttachment()
                Session("mAircraftInformationBoardList") = Nothing 'Added by Saylee on 16-July-2009
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                ControlVisibility()
                MachineDetail = "Model : " + mAssemblyStatus.ModelName + " Serial No : " + mAssemblyStatus.Assembly.SerialNo + "Service Type : " + txtMonitorServiceType.Text
                MarkLog(Action.Save, "Assembly Service Status", MachineDetail, ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("AssemblyMonitorServiceStatusClone") = AssemblyMonitorServiceStatusClone
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                AssemblyMonitorServiceStatusClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Public Function CheckPeriods() As Boolean 'Added by Saylee on 21-Aug-2008
        SetObject()
        SetGridObject()
        Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod
        For Each mAssemblyMonitorServiceStatusPeriod In mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
            If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mAssemblyMonitorServiceStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 9th-Oct-2009
        If Not (mMachineMaintenanceList.Contains(mAssemblyMonitorServiceStatus.ID, 5, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 5, txtDoneOnDate.Text, mAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatus.ID, 5)
        End If
        With mMachineMaintenance
            .MaintenanceID = mAssemblyMonitorServiceStatus.ID 'TransactionID
			'  .Date = txtDoneOnDate.Text
			If txtDoneOnDate.Text = "" Then
				.Date = System.DBNull.Value
			Else
				.Date = txtDoneOnDate.Text
			End If
			Dim mLog As Log
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtDoneOnDate.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(txtDoneOnDate.Text, mAssemblyStatus.MachineID)
                    If mMaxLogNo.Count <> 0 Then
                        .LogNo = mMaxLogNo(0).LogNo
                        .LogID = mMaxLogNo(0).LogId
                        .LogPageNo = mMaxLogNo(0).LogPageNo
                    End If
                End If
                'End
            End If
        End With
        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub SaveMachineMaintenance()
        If mMachineMaintenance.IsValid = True Then 'Added by Saylee on 9th-Oct-2009
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If ''  End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mAssemblyMonitorServiceStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mAssemblyMonitorServiceStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyMonitorServiceStatus.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment()
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
    Private Sub UpdatePanel()
        upnlMonitoringStatusDetails.Update()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
        upnlDocumentDetails.Update()
        upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        upnlSelectMonitoringService.Update()
        upnlRevisionDetails.Update()
    End Sub
    Private Sub SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
        If Not mAssemblyMonitorServiceStatus Is Nothing Then
            If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is System.DBNull.Value Then
                Dim txtdueOnValue As TextBox
                For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
                    txtdueOnValue = CType(dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
                    txtdueOnValue.BackColor = System.Drawing.Color.Red
                    txtdueOnValue.ForeColor = System.Drawing.Color.White
                Next
                lblRed.Visible = True
                lblInfo.Visible = True
            Else
                lblRed.Visible = False
                lblInfo.Visible = False
            End If
        End If
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        'Added on 28-05-2007 by Saylee
        txtDoneOnDate.Text = mAssemblyMonitorServiceStatus.DoneOnFormatted.ToString
        txtExtensionDate.Text = mAssemblyMonitorServiceStatus.ExtensionDateFormatted.ToString
        'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours + ")"

        BindLicenceNo() 'MLNo
        DataBind()
        'Added by Ajay 22-01-2023
        mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mMachine.ID)
        Session("mLastAMPRef") = mLastAMPRef
        If (mLastAMPRef.AMPNo <> "") Then lblAMPNo.Text = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        dgCurrentValue.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 500 char"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Utkarsh On 13-Jun-2012 FOR ALL08062012
        ElseIf custValidator.ControlToValidate = "txtlicenceno" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetGridObject()
        SetObject()
        Dim str As String = ""
        Dim txtElapsedValue As TextBox
        Dim txtRemainingValue As TextBox
        If Not mAssemblyMonitorServiceStatus.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            If Not mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvRemark.ErrorMessage = str
            cvRemark.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack Then
            If btnSelectMonitoringService.Enabled = True Then
                setFocus(btnSelectMonitoringService)
            End If
            DataFieldBind()
            SetTitle()
            ControlVisibility()
            SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If
    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtElapsedValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                .Item(I).ElapsedValue = txtElapsedValue.Text.Trim
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Protected Sub txtRemainingValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemainingValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtRemainingValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                .Item(I).RemainingValue = txtRemainingValue.Text
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    'Added By Shital on 25-Jan-2021
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    '--------------End
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim calDoneOn As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            calDoneOn = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(i).CurrentValueFormatted = ""
                    Else
                        .Item(i).CurrentValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(i).CurrentValue = Trim(calDoneOn.Text)
                End If
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Protected Sub txtDueOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDueOnValue As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
            mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                ' .Item(i).IsCalculate = chkcalValues.Checked
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(txtDueOnValue.Text.Trim) Then
                        .Item(i).DueOnValueFormatted = ""
                    Else
                        .Item(i).DueOnValueFormatted = Trim(txtDueOnValue.Text)
                    End If
                Else
                    .Item(i).DueOnValue = Trim(txtDueOnValue.Text)
                End If
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If ((mAssemblyStatus.IsMaster) And ((Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew))) Or ((Not mAssemblyStatus.IsMaster) And ((Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew))) Then
        If ((mAssemblyStatus.IsMaster) And ((Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew))) Or ((Not mAssemblyStatus.IsMaster) And ((Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew And Not User.IsInRole("AssemblyServiceMonitorNew") And mAssemblyMonitorServiceStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew And (Not User.IsInRole("AssemblyServiceMonitorEdit") And Not mAssemblyMonitorServiceStatus.IsNew)) Or (Not User.IsInRole("AssemblyServiceMonitorNew") And mAssemblyMonitorServiceStatus.IsNew) Or (Not User.IsInRole("AssemblyServiceMonitorEdit") And Not mAssemblyMonitorServiceStatus.IsNew))) Then
            '''AssemblyServiceMonitorEdit line in above if condition, Added by Saylee on 8-Jul-2020 for  All08072020 
            SetObject()
            MachineDetail = "Model : " + mMachineMaintenanceList.Item(mMachineMaintenanceList.CurrentIndex).ModelName + " Serial No : " + mMachineMaintenanceList.Item(mMachineMaintenanceList.CurrentIndex).SerialNo + "Service Type : " + txtMonitorServiceType.Text
            MarkLog(Action.Save, "Assembly Service Status ", User.Identity.Name & " is not Authorized User to save " & MachineDetail, ErrorType.HandledError, Guid.Empty, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
            Exit Sub
        End If
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If CheckPeriods() = False Then
                'Added By Utkarsh On 16-May-2012 FOR ALL15052012
                If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is System.DBNull.Value Then
                    MSGBoxCtrl.Show("Save Alert!", "You are about to comply one time service status.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    Exit Sub
                End If
                'End
                If Save() = True Then
                    SetTitle()
                    UpdatePanel()
                    If Not txtDoneOnDate.Text = "" Then
                        Back(sender, e)
                    End If
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    upnlValidationSummary.Update()
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodNotPresent, MSGBox.Message_text.PeriodNotPresent, "Period used to monitor this maintenance activity is not present in Assembly Status", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnSelectMonitoringService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectMonitoringService.Click
        SetObject()
        SetGridObject()
        Response.Redirect("wfModelMonitorServiceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfAssemblyMonitorServiceStatusNew_Ajax.aspx")
    End Sub

    Private Sub DoneOnDateChanged(sender As Object, e As EventArgs) Handles txtDoneOnDate.TextChanged

        'Added by Saylee on 11-Jul-2018 for ALL21062018, to show current values as per Done On Date selection
        If IsPostBack Then

            Dim tmpmAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = mAssemblyMonitorServiceStatus.Clone

            If tmpmAssemblyMonitorServiceStatus.IsNew Then

                If txtDoneOnDate.Text <> "" Then
                    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid,
                                                                                                                 mAssemblyStatus.AssemblyID,
                                                                                                                 mAssemblyStatus.ID,
                                                                                                                 txtDoneOnDate.Text,
                                                                                                                 mAssemblyStatus.
                                                                                                                            Assembly.ModelID,
                                                                                                                 mMachine.HourType)
                Else

                    'mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Session("mIssueDate"), mAssemblyStatus.Assembly.ModelID, mMachine.HourType)

                    If tmpmAssemblyMonitorServiceStatus.ModelMonitorService.ReviseRemark <> "" Then
                        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid,
                                                                                                                     mAssemblyStatus.
                                                                                                                                AssemblyID,
                                                                                                                     mAssemblyStatus.ID,
                                                                                                                     Today.Date.ToString,
                                                                                                                     mAssemblyStatus.
                                                                                                                                Assembly.ModelID,
                                                                                                                     mMachine.HourType)
                    Else
                        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid,
                                                                                                                     mAssemblyStatus.
                                                                                                                                AssemblyID,
                                                                                                                     mAssemblyStatus.ID,
                                                                                                                     Session("mIssueDate"),
                                                                                                                     mAssemblyStatus.
                                                                                                                                Assembly.ModelID,
                                                                                                                     mMachine.HourType)
                    End If

                End If

                With mAssemblyMonitorServiceStatus

                    Dim mModelMonitorService As ModelMonitorService = CType(Session("mModelMonitorService"), ModelMonitorService)
                    .ModelMonitorServiceID(True) = mModelMonitorService.ID
                    .ModelMonitorService.Reference = mModelMonitorService.Reference
                    .ModelMonitorService.Description = mModelMonitorService.Description
                    .ModelMonitorService.RequiredManHours = mModelMonitorService.RequiredManHours

                End With

            Else

                'Added by Saylee on 11-Jul-2018, to show current values as per Done On Date selection
                If txtDoneOnDate.Text = "" Then
                    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(tmpmAssemblyMonitorServiceStatus.ID,
                                                                                                                 mAssemblyStatus.AssemblyID,
                                                                                                                 mAssemblyStatus.ID,
                                                                                                                 mMachine.HourType,
                                                                                                                 tmpmAssemblyMonitorServiceStatus.
                                                                                                                                AsOnDateFormatted,
                                                                                                                 mAssemblyStatus.Assembly.ModelID,
                                                                                                                 False)
                Else
                    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(tmpmAssemblyMonitorServiceStatus.ID,
                                                                                                                 mAssemblyStatus.AssemblyID,
                                                                                                                 mAssemblyStatus.ID,
                                                                                                                 mMachine.HourType,
                                                                                                                 txtDoneOnDate.Text,
                                                                                                                 mAssemblyStatus.Assembly.ModelID,
                                                                                                                 True)
                End If

            End If

            DataBindGrid()
            upnlRedLabel.Update()
            upnlCurrentValue.Update()
            upnlDoneOnValue.Update()

        End If

    End Sub

    'Modified by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
    Private Sub Back(sender As Object, e As EventArgs) Handles btnBack.Click

        If Session("NewPage") = "True" Or mAssemblyMonitorServiceStatus.ModelMonitorService.ReviseRemark <> "" Then

            Session("NewPage") = "False"

            MarkLog(Action.Close,
                    "Assembly Service Status",
                    "",
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

            RemoveSession()
            Response.Redirect(Request.QueryString("BackPage"))

        Else

            MarkLog(Action.Close,
                    "Assembly Service Status",
                    "",
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

            RemoveSession()
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" &
                                  Request.QueryString("BackPage") & "&ChildPage=" &
                                  Request.QueryString("ChildPage") & "&GChildPage=" &
                                  Request.QueryString("GChildPage") & "&GChildPage1=" &
                                  Request.QueryString("GChildPage1"))

        End If

    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mAssemblyMonitorServiceStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mAssemblyMonitorServiceStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorServiceStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorServiceStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mAssemblyMonitorServiceStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyMonitorServiceStatus.DoneOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
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
            If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatus.ID, 5, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If
        Else
            If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        SetObject()
        SetGridObject()
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mAssemblyMonitorServiceStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", mAssemblyMonitorServiceStatus.AsOnDate.ToString, txtDoneOnDate.Text))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            Dim LogID As String
            LogID = CType(Session("LogID"), String)
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            If mAssemblyMonitorServiceStatus.IsNew = False Then 'Edit record 
                mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatus.ID,
                                                                                                             mAssemblyStatus.AssemblyID,
                                                                                                             mAssemblyStatus.ID,
                                                                                                             mMachine.HourType,
                                                                                                             mLog.Date.ToString,
                                                                                                             mAssemblyStatus.Assembly.ModelID,
                                                                                                             True,
                                                                                                             mLog.ID.ToString,
                                                                                                             True)
            Else
                mAssemblyMonitorServiceStatus.LogID(LogID,
                                                    mLog.Date.ToString,
                                                    True,
                                                    CType(Session("mModelMonitorService"), ModelMonitorService)) = New Guid(LogID)
            End If
            Session.Remove("FromLog")
            DataBindGrid()
            ControlVisibility()
            SetTitle()
            upnlCurrentValue.Update()
            upnlDoneOnValue.Update()
        End If
    End Sub

    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
    Private Sub ReviseActivity(sender As Object, e As EventArgs) Handles btnRevise.Click

        Try

            MSGBoxCtrl.Show(MessageTitle:="Alert!",
                            MessageText:="You are about to Revise Model Activity.
                                          After revision of model activity this Status will become Not Applicable.",
                            ExtraMessage:="Do you want to continue?",
                            ButtonToShow:=MsgBoxStyle.YesNo,
                            Sender:="ReviseActivity")

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

#Region " Report "
    '    'Created By :- Pallavi , Date -09/08/2006
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
            Exit Sub
        End If
        Rpt = New crDetAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim MPDType As String = ""
        Dim ReportName As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            MPDType = "MPD Type"
            ReportName = "Maintenance Events Detail Report"
        Else
            MPDType = "Service Type"
            ReportName = "Assembly Service Status Detail Report"
        End If


        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", MPDType,
                   txtMonitorServiceType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                       dgCurrentValue.Columns.Item(0).HeaderText, dgCurrentValue.Columns.Item(1).HeaderText, ,
                    dgCurrentValue.Columns.Item(2).HeaderText, , dgCurrentValue.Columns.Item(3).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", MPDType,
                             txtMonitorServiceType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                   "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                             "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                            txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , lblNote.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 6
        RHCount1 = Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            'ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
            'New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
            'dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, , _
            'dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText))
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
          New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
          dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, , _
          dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
                                New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                      "", "", , "", , "", ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                    txtWorkOrNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                        txtWorkOrNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                    mAssemblyMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), , _
                     CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                        mAssemblyMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), , _
                     CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                        txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours", _
                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), , _
                     CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours", _
                        txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                    txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), , _
                     CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                        txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                    "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), , _
                   CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), , , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                                          "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                                 "", "", , "", , "", "", , lblNote1.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), _
                , lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        If temp2 < RHCount2 Then
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
            txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
            dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, "Extension Date ", _
            dgDoneOnValue.Columns.Item(2).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(3).HeaderText, _
            dgDoneOnValue.Columns.Item(4).HeaderText, dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
                                txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                                      "", txtExtensionDate.Text, , "", , "", ""))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                    txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark", _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DoneOnValueFormatted, String), , _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), _
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ReportName, lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
#End Region
#End Region

#Region "Service Methods"
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

End Class