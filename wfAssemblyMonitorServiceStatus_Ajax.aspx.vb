Imports System.Linq

Public Class wfAssemblyMonitorServiceStatus_Ajax
    Inherits System.Web.UI.Page

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
    Public mIsSpareAssembly As Integer 'Added By Saylee On 27-Jul-2020 For ALL27072020
    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 20-07-2023
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
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added By Saylee On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorServiceStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 13th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 13th-Oct-2009
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgAssemblyValues.Rows.Count - 1
            txtDnOnDate = CType(Me.dgAssemblyValues.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 And txtDoneOnDate.Text <> "" Then
                    txtDnOnDate.Enabled = False
                Else
                    txtDnOnDate.Enabled = True
                End If
            End With
        Next j
    End Sub
    Private Sub ControlVisibility()
        If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 3 Then 'for MonitorTypeID=NoFrequency 
            REM:1)for elapsed value
            dgElapsedRemainingValues.Columns(2).Visible = False
            REM:2)for Remaining Value
            dgElapsedRemainingValues.Columns(3).Visible = False
            REM:3) for Due On VAlue
            dgAssemblyValues.Columns(5).Visible = False
            REM:3) for Extension VAlue
            dgAssemblyValues.Columns(4).Visible = False
        End If
        'Added By Utkarsh ON 28-Jun-2013 FOR ALL28062013
        dgAssemblyValues.Columns(6).Visible = (mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Saylee On 27-Jul-2020 For ALL27072020
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
        End If
        If txtRemark.ReadOnly Then txtRemark.BackColor = Color.Gainsboro
        If txtWorkOrNo.ReadOnly Then txtWorkOrNo.BackColor = Color.Gainsboro
        If mAssemblyMonitorServiceStatus.EnableDoneOn = False Then txtDoneOnDate.Enabled = False
        If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If
        ControlVisibilityForAttachment()
    End Sub
    Private Sub SetCaption()

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "Maintenance Events"
            lblMonitorServiceType.InnerText = "Task Type"
            btnSelectMonitoringService.ToolTip = "Click to open MPD List screen"
            btnSelectMonitoringService.Text = "Select Monitoring MPD"
        Else
            ServiceMPDTitle = "Assembly Service"
            btnSelectMonitoringService.ToolTip = "Click to open Model Service List screen"
            btnSelectMonitoringService.Text = "Select Monitoring Service"
        End If

        If mAssemblyMonitorServiceStatus.IsNew Then
            lblTitle.Text = ServiceMPDTitle + " Status [Model : " & mAssemblyStatus.ModelName & " Serial No. :" & mAssemblyStatus.Assembly.SerialNo & "] [New]"
        Else
            lblTitle.Text = ServiceMPDTitle + " Status [Model : " & mAssemblyStatus.ModelName & " Serial No. :" & mAssemblyStatus.Assembly.SerialNo & "] "
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
            .IsApplicable = chkApplicable.Checked
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
        End With
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
    End Sub
    Private Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, calDoneOn, txtDueOnValue, txtExtensionValue As TextBox
        If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID <> 3 Then
            For i As Integer = 0 To CShort(dgElapsedRemainingValues.Rows.Count - 1)
                txtElapsedValue = CType(Me.dgElapsedRemainingValues.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgElapsedRemainingValues.Rows(i).FindControl("txtRemainingValue"), TextBox)
                With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                    .Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
                    .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
                End With
            Next i
        End If
        For j As Integer = 0 To Me.dgAssemblyValues.Rows.Count - 1
            calDoneOn = CType(Me.dgAssemblyValues.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            txtDueOnValue = CType(Me.dgAssemblyValues.Rows(j).FindControl("txtDueOnValue"), TextBox)
            txtExtensionValue = CType(Me.dgAssemblyValues.Rows(j).FindControl("txtExtensionValue"), TextBox) 'Added By Saylee on 22-07-2008
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(j).DoneOnValue = ""
                    Else
                        .Item(j).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(j).DoneOnValue = Trim(calDoneOn.Text)
                End If
                .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)
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
                                SetCaption()
                                UpdatePanel()
                                'Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                            End If
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("Sender") = ""
                        UpdatePanel()
                        'Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                    End If
            End Select
        End If
    End Sub

    Private Function Save() As Boolean

        Dim cln As AssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 13th-Oct-2009

        If mAssemblyMonitorServiceStatus.IsValid Then

            Try

                If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then

                    MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.PeriodUnitRequired,
                                    MessageText:=MSGBox.Message_text.PeriodUnitRequired,
                                    ExtraMessage:="You are trying to save Assembly Service. Record cannot save without Periods.",
                                    ButtonToShow:=MsgBoxStyle.OkOnly,
                                    Sender:="")

                    Return False

                    Exit Function

                End If

                'aded By Deven on 24-Sep-2009 ------
                If Session("IsOpenFromMPD") IsNot Nothing AndAlso Not Session("IsOpenFromMPD").ToString().ToLower() = "true" Then

                    If mAssemblyMonitorServiceStatusList IsNot Nothing AndAlso
                       mAssemblyMonitorServiceStatusList.Contains(ModelMonitorServiceID:=mAssemblyMonitorServiceStatus.ModelMonitorServiceID) And
                       mAssemblyMonitorServiceStatus.IsNew = True Then

                        MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.DataBaseError,
                                        MessageText:=MSGBox.Message_text.Duplicate,
                                        ExtraMessage:="Assembly Service Status.",
                                        ButtonToShow:=MsgBoxStyle.OkOnly,
                                        Sender:="")

                        Return False

                        Exit Function

                    End If

                End If

                '-------------------------------------------------
                'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                If Not mAssemblyMonitorServiceStatus.DoneByID.Equals(Guid.Empty) AndAlso mAssemblyMonitorServiceStatus.DoneOn.ToString.Length > 0 Then

                    Dim Title As String = "Save Alert !"
                    Dim Message As String = ""
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorServiceStatus.DoneByID.ToString,
                                                                                                    mAssemblyMonitorServiceStatus.DoneOn.ToString)

                    If mEmployeeStatus(0).Information <> "" Then

                        Message = mEmployeeStatus(0).Information

                        ScriptManager.RegisterStartupScript(page:=Me,
                                                            type:=[GetType],
                                                            key:="OpenScript",
                                                            script:=MessageBox.Show(Title, Message, , False),
                                                            addScriptTags:=True)

                        Return False

                        Exit Function

                    End If

                End If

                'End
                mAssemblyMonitorServiceStatus.ApplyEdit()
                mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
                SaveAttachment()
                Session("mAircraftInformationBoardList") = Nothing 'Added by Saylee on 16-July-2009
                SaveMachineMaintenance()  'Added by Saylee on 13th-Oct-2009
                Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                ControlVisibility()

                Return True

            Catch ex As SqlException

                Session("cln") = cln

                If ex.Number = 8145 Then

                    MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.DataBaseError,
                                    MessageText:=MSGBox.Message_text.ProcedureError,
                                    ExtraMessage:=ex.Procedure,
                                    ButtonToShow:=MsgBoxStyle.OkOnly,
                                    Sender:="")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.DataBaseError,
                                    MessageText:=MSGBox.Message_text.Duplicate,
                                    ExtraMessage:=ex.Procedure,
                                    ButtonToShow:=MsgBoxStyle.OkOnly,
                                    Sender:="")

                End If

                Return False

            Finally

                cln = Nothing

                Dim mRegNo As String = ""

                If mAssemblyStatus.IsSpareAssembly = False Then
                    mRegNo = "Reg No. : " & mMachine.RegNo
                End If

                MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " +
                                mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " &
                                mAssemblyMonitorServiceStatus.ModelMonitorService.Description & " Monitor Type : " &
                                mAssemblyMonitorServiceStatus.ModelMonitorService.ModelMonitorServiceTypeName

                MarkLog(Util.Action.Save,
                        "Assembly Service Status",
                        MachineDetail,
                        Util.ErrorType.NoError,
                        mAssemblyMonitorServiceStatus.ID,
                        EventLogID)

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
        'Added by Saylee on 13th-Oct-2009
        If Not (mMachineMaintenanceList.Contains(mAssemblyMonitorServiceStatus.ID, 5, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 5, txtDoneOnDate.Text, mAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatus.ID, 5)
        End If
        With mMachineMaintenance
            .MaintenanceID = mAssemblyMonitorServiceStatus.ID 'TransactionID
            If txtDoneOnDate.Text <> "" Then
                .Date = txtDoneOnDate.Text
            Else
                .Date = System.DBNull.Value
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
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
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
    Private Sub SetRights() 'Added By Utkarsh On 14-Mar-2011
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyServicePrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyServicePrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    Private Sub UpdatePanel()
        upnlMonitoringStatusDetails.Update()
        upnlElapsedRemainingValues.Update()
        upnlAssemblyValues.Update()
        upnlDocumentDetails.Update()
        upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        upnlSelectMonitoringService.Update()
    End Sub
    Private Sub SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
        If Not mAssemblyMonitorServiceStatus Is Nothing Then
            If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is System.DBNull.Value Then
                Dim txtdueOnValue As TextBox
                For i As Integer = 0 To dgAssemblyValues.Rows.Count - 1
                    txtdueOnValue = CType(dgAssemblyValues.Rows(i).FindControl("txtDueOnValue"), TextBox)
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
    End Sub 'End
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
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgElapsedRemainingValues.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        dgAssemblyValues.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        'Added on 28-05-2007 by Saylee
        txtDoneOnDate.Text = mAssemblyMonitorServiceStatus.DoneOnFormatted.ToString
        txtExtensionDate.Text = mAssemblyMonitorServiceStatus.ExtensionDateFormatted.ToString
        'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours + ")"
        BindLicenceNo() 'MLNo

		'Added by Ajay 21-01-2023
		If Not mMachine Is Nothing Then
			mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mMachine.ID)
			Session("mLastAMPRef") = mLastAMPRef
			If (mLastAMPRef.AMPNo = "") Then

			Else
				lblAMPNo.Text = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
			End If
		End If


		DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        dgElapsedRemainingValues.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        dgAssemblyValues.DataSource = mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
        dgElapsedRemainingValues.DataBind()
        dgAssemblyValues.DataBind()
        SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                CustValidator.ErrorMessage = "Max. Length of Remark is 500 char."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Prashant On 12-Jun-2012 FOR ALL08062012
        ElseIf CustValidator.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                CustValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End
        End If
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim CustValidator As CustomValidator = CType(s, CustomValidator)

        SetGridObject() REM: this is for grid validation
        SetObject()
        Dim str As String = ""
        If Not mAssemblyMonitorServiceStatus.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1)
            If Not mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).IsValid Then
                For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                    str = str + mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            CustValidator.ErrorMessage = str
            e.IsValid = False
        Else
            e.IsValid = True
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean REM: THIS IS FOR SHOWING THE BROKEN RULES
        Dim str As String = ""
        For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1)
            If Not mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).IsValid Then
                For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                    str = str + mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvRemark.ErrorMessage = str
            cvRemark.IsValid = False
            Return False
        Else
            cvRemark.IsValid = True
            Return True
        End If
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM: put here the code to initialize the page
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011
        If Not IsPostBack Then
            If btnSelectMonitoringService.Enabled = True Then
                setFocus(btnSelectMonitoringService)
            End If
            DataFieldBind()
            SetCaption()
            ControlVisibility()
            SetRights()  'Added By Utkarsh On 14-Mar-2011
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
        For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
            txtElapsedValue = CType(Me.dgElapsedRemainingValues.Rows(i).FindControl("txtElapsedValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                .Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
            End With
        Next i
        DataBindGrid()
        upnlElapsedRemainingValues.Update()
        upnlAssemblyValues.Update()
    End Sub
    Protected Sub txtRemainingValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemainingValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
            txtRemainingValue = CType(Me.dgElapsedRemainingValues.Rows(i).FindControl("txtRemainingValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
            End With
        Next i
        DataBindGrid()
        upnlElapsedRemainingValues.Update()
        upnlAssemblyValues.Update()
    End Sub
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim calDoneOn As TextBox = CType(Me.dgAssemblyValues.Rows(i).FindControl("txtDoneOnValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text) Then
                        .Item(i).DoneOnValueFormatted = ""
                    Else
                        .Item(i).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(i).DoneOnValue = Trim(calDoneOn.Text)
                End If
            End With
        Next i
        DataBindGrid()
        upnlElapsedRemainingValues.Update()
        upnlAssemblyValues.Update()
    End Sub
    Protected Sub txtDueOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim txtDueOnValue As TextBox = CType(Me.dgAssemblyValues.Rows(i).FindControl("txtDueOnValue"), TextBox)
            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(txtDueOnValue.Text) Then
                        .Item(i).DueOnValueFormatted = ""
                    Else
                        .Item(i).DueOnValueFormatted = Trim(txtDueOnValue.Text)
                    End If
                Else
                    .Item(i).DueOnValue = Trim(txtDueOnValue.Text)
                End If
            End With
        Next i
        DataBindGrid()
        upnlElapsedRemainingValues.Update()
        upnlAssemblyValues.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgAssemblyValues.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        DataBindGrid()
        upnlElapsedRemainingValues.Update()
        upnlAssemblyValues.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If CheckPeriods() = False Then
                'Added By Utkarsh On 16-May-2012 FOR ALL15052012
                If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is System.DBNull.Value Then
                    MSGBoxCtrl.show("Save Alert!", "You are about to comply one time service status.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    Exit Sub
                End If
                'End
                If Save() = True Then
                    SetCaption()
                    UpdatePanel()
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    upnlValidationSummary.Update()
                    'Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
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
        Response.Redirect("wfModelMonitorServiceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfAssemblyMonitorServiceStatus_Ajax.aspx")
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        If IsPostBack Then
            SetObject()
            DataBindGrid()
            upnlRedLabel.Update()
            upnlElapsedRemainingValues.Update()
            upnlAssemblyValues.Update()
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim mRegNo As String = ""
        If mAssemblyStatus.IsSpareAssembly = False Then
            mRegNo = "Reg No. : " & mMachine.RegNo
        End If

        MachineDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & " Monitor Type : " & mAssemblyMonitorServiceStatus.ModelMonitorService.ModelMonitorServiceTypeName
        If Not mAssemblyMonitorServiceStatus.IsNew Then
            MarkLog(Util.Action.Close, "Assembly Service Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
        Else
            MarkLog(Util.Action.Close, "Assembly Service Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
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
#End Region

#Region " Report "
    'Created By :- Pallavi , Date -09/08/2006
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
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
                      dgElapsedRemainingValues.Columns.Item(0).HeaderText, dgElapsedRemainingValues.Columns.Item(1).HeaderText,
                    , dgElapsedRemainingValues.Columns.Item(2).HeaderText, , dgElapsedRemainingValues.Columns.Item(3).HeaderText))
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
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
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
                           CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
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
            ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Date",
            New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
            dgAssemblyValues.Columns.Item(0).HeaderText, dgAssemblyValues.Columns.Item(1).HeaderText, ,
            dgAssemblyValues.Columns.Item(2).HeaderText, , dgAssemblyValues.Columns.Item(3).HeaderText,
            dgAssemblyValues.Columns.Item(4).HeaderText, dgAssemblyValues.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Date",
                                New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                                      "", "", , "", , "", ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Work Order No.",
                    txtWorkOrNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Work Order No.",
                        txtWorkOrNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "License No.",
                    mAssemblyMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "License No.",
                        mAssemblyMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Place",
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Place",
                        txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Actual Man Hours ",
                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Actual Man Hours ",
                        txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Remark",
                    txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "Remark",
                        txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "",
                    "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "",
                                          "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                                                 "", "", , "", , "", "", , lblNote1.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Compliance Details", "",
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText,
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).FrequencyValueFormatted, String), ,
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DoneOnValueFormatted, String), ,
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).CurrentValueFormatted, String),
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String),
                CType(Me.mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), lblNote1.Text))
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
            dgAssemblyValues.Columns.Item(0).HeaderText, dgAssemblyValues.Columns.Item(1).HeaderText, "Extension Date ", _
            dgAssemblyValues.Columns.Item(2).HeaderText, txtExtensionDate.Text, dgAssemblyValues.Columns.Item(3).HeaderText, _
            dgAssemblyValues.Columns.Item(4).HeaderText, dgAssemblyValues.Columns.Item(5).HeaderText))
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