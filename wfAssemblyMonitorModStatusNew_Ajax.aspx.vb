Imports System.Linq
Public Class wfAssemblyMonitorModStatusNew_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Private Flag As Int16
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 13th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 13th-Oct-2009
    Dim EventLogID As Guid
    Public mDirectiveDetail As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mDirectiveNo As String
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
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
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
    Private Sub ControlToVisibility()
        btnPrint.Enabled = Not mAssemblyMonitorModStatus.IsNew
        btnSelect.Enabled = mAssemblyMonitorModStatus.IsNew
        REM: For No Frequency
        dgCurrentValue.Columns(3).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        dgCurrentValue.Columns(4).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(4).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(5).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3)
        'Added By Shweta ON 28-Jun-2013 FOR ALL28062013
        dgDoneOnValue.Columns(6).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        'end
        If mAssemblyMonitorModStatus.ModelMonitorModID.Equals(Guid.Empty) Then
            txtDoneOnDate.BackColor = Color.Gainsboro
            txtDoneOnDate.Enabled = False
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True
            txtWorkOrdNo.BackColor = Color.Gainsboro
            txtWorkOrdNo.ReadOnly = True
            chkApplicable.Enabled = False 'Added By Rajnish 22-12-2007
        End If
        If txtRemark.ReadOnly Then txtRemark.BackColor = Color.Gainsboro
        If txtWorkOrdNo.ReadOnly Then txtWorkOrdNo.BackColor = Color.Gainsboro
        If Not (mAssemblyMonitorModStatus.ModelMonitorMod.ReadOnlyFrequencyColumn) = False Then txtDoneOnDate.Enabled = False
        If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If
        'Revise Activity
        btnRevise.Enabled = (mAssemblyMonitorModStatus.IsApplicable And Not mAssemblyMonitorModStatus.IsNew)
        'End
        txtEffectiveFromDate.Enabled = IIf(txtDoneOnDate.Text = "", True, False)
        btnSelectLog.Visible = (mIsSpareAssembly <> 1) ' Added By Vikrant On 27-Jul-2020 For ALL27072020
        ControlVisibilityForAttachment()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorModStatus")
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
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()

        Try

            With mAssemblyMonitorModStatus

                Dim LicenseNo As String = String.Empty ' Added By Utkarsh On 12-Jun-2012 FOR ALL08062012
                Dim EmpName As String = String.Empty

                If txtDoneOnDate.Text = "" Then
                    .DoneOn = System.DBNull.Value
                Else
                    .DoneOn = txtDoneOnDate.Text
                End If

                .DoneWONo = Trim(txtWorkOrdNo.Text)
                .DoneRemark = Trim(txtRemark.Text)
                .IsApplicable = chkApplicable.Checked
                .SourceDoc = Trim(txtSourceDoc.Text)
                .RevisionNo = Trim(txtRevisionNo.Text)
                .BookNo = Trim(txtBookNo.Text)
                .PageNo = Trim(txtPageNo.Text)
                .RequiredManHours = Trim(txtRequiredManHours.Text)
                .IsLater = chkIsLater.Checked          'Added By Prashant 17-Aug-2010

                If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                    LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                    EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2,
                                  txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                Else
                    LicenseNo = Trim(txtLicenceNo.Text)
                End If

                .LicenseNo = LicenseNo
                .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID 'End 
                .Place = txtPlace.Text.Trim  'Added By Shweta 06-04-2012

                If Not mFileAttach Is Nothing Then

                    If mFileAttach.Size > 0 Then
                        .IsAttachmentAdded = True
                    Else
                        .IsAttachmentAdded = False
                    End If

                End If

                'Added By Vikrant on 11-Dec-2019 For ALL11122019-1
                If txtEffectiveFromDate.Text = "" Then
                    .AsOnDate = System.DBNull.Value
                Else
                    .AsOnDate = txtEffectiveFromDate.Text
                End If
                'End

                .MethodOfCompliance = Trim(txtMethodOfCompliance.Text) 'Added By Harsh on 10-Oct-2024

            End With

            If mAssemblyMonitorModStatus.IsNew Then mAssemblyMonitorModStatus.IsMaster = False 'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)

            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, txtDoneOnDate, txtDueOnValue, txtExtensionValue As TextBox
        If mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3 Then
            For i As Integer = 0 To Me.dgCurrentValue.Rows.Count - 1
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            Next i
        End If
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDoneOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox) 'Added By Shital on 25-Jan-2021
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(txtDoneOnDate.Text.Trim) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(txtDoneOnDate.Text)
                    End If
                Else
                    .Item(j).CurrentValue = Trim(txtDoneOnDate.Text)
                End If
                'Added By Shital on 25-Jan-2021
                If txtExtensionValue Is Nothing Then
                    .Item(j).ExtensionValue = ""
                Else
                    .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)  'Added By Saylee on 28-07-2008 Shital on 25-Jan-2021
                End If
            End With
        Next j
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    Private Function Save() As Boolean
        Dim AssemblyMonitorModStatusClone As AssemblyMonitorModStatus
        AssemblyMonitorModStatusClone = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mAssemblyMonitorModStatus.IsValid = True Then
            If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Assembly Directives Status.Assembly Directives Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Dim mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList 'aded By Deven on 24-Sep-2009 ------
            mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString, ModelMonitorModID:=mAssemblyMonitorModStatus.ModelMonitorModID.ToString)
            If mAssemblyMonitorModStatusList.Contains(mAssemblyMonitorModStatus.ModelMonitorModID) And mAssemblyMonitorModStatus.IsNew = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Assembly Directive Status", MsgBoxStyle.OkOnly, "")
                Return False
            End If '---------------------------------------
            Try
                If Not mAssemblyMonitorModStatus.DoneByID.Equals(Guid.Empty) AndAlso mAssemblyMonitorModStatus.DoneOn.ToString.Length > 0 Then 'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                    Dim Title As String = "Save Alert !"
                    Dim Message As String = ""
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorModStatus.DoneByID.ToString, mAssemblyMonitorModStatus.DoneOn.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
                        Return False
                    End If
                End If 'End
                mAssemblyMonitorModStatus.ApplyEdit()
                mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
                'Revise Activity
                If Not Session("mPrevAssemblyMonitorModStatusForRevise") Is Nothing Then
                    Dim mPrevAssemblyMonitorModStatusForRevise As AssemblyMonitorModStatus
                    mPrevAssemblyMonitorModStatusForRevise = Session("mPrevAssemblyMonitorModStatusForRevise")
                    mPrevAssemblyMonitorModStatusForRevise.IsApplicable = False
                    mPrevAssemblyMonitorModStatusForRevise.Save()
                    Session.Remove("mPrevAssemblyMonitorModStatusForRevise")
                End If
                'End
                SaveAttachment()
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                ControlToVisibility()
                mDirectiveDetail = "Model : " + mAssemblyStatus.ModelName + " Serial No. : " + mAssemblyStatus.Assembly.SerialNo + "Directive No. : " + txtModNumber.Text.Trim + " Monitor Type : " + txtModelMonitorModTypeName.Text
                MarkLog(Util.Action.Save, "Assembly Directive Status", mDirectiveDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("AssemblyMonitorModStatusClone") = AssemblyMonitorModStatusClone
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                AssemblyMonitorModStatusClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        Dim AssemblyInfo As String = "[Model: " & mAssemblyStatus.ModelName & " SerialNo: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        If mAssemblyMonitorModStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Directive Status " & AssemblyInfo & " [New]"
        Else
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Directive Status" & AssemblyInfo
        End If
        lblAssemblyValues.InnerText = mAssemblyStatus.AssemblyTypeName & " Values"
        upnlTitle.Update()
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
                    End If
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.[New], "Model Directive", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mModelMonitorMod As ModelMonitorMod
                        Dim ID As Guid = Guid.NewGuid 'Revise Activity
                        mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(mAssemblyMonitorModStatus.ModelMonitorMod, mMachine.HourType)
                        'New
                        Dim tmpModelMonitorMod As ModelMonitorMod
                        tmpModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mAssemblyMonitorModStatus.ModelMonitorMod.ID)
                        'If mAssemblyMonitorModStatus.DoneOnFormatted.ToString = "" Then
                        '    mModelMonitorMod.IssueDate = mAssemblyMonitorModStatus.AsOnDateFormatted.ToString
                        'Else
                        '    mModelMonitorMod.IssueDate = mAssemblyMonitorModStatus.DoneOnFormatted.ToString
                        'End If
                        If Not tmpModelMonitorMod.IssueDateFormatted.ToString = "" Then
                            mModelMonitorMod.IssueDate = tmpModelMonitorMod.IssueDate
                        Else
                            mModelMonitorMod.IssueDate = System.DBNull.Value
                        End If
                        'End
                        Session("mModelMonitorMod") = mModelMonitorMod
                        RemoveSession()
                        mModelMonitorMod.BeginEdit()
                        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                        Session("mPrevAssemblyMonitorModStatusForRevise") = mAssemblyMonitorModStatus

                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow();", True)
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("Sender") = ""
                        UpdatePanel()
                    End If
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.Close, "Assembly Directive Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        RemoveSession()
                        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        Response.Redirect(Request.QueryString("BackPage"))
                    End If
                    'End
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                    End If
            End Select
        End If
    End Sub
    Public Function CheckPeriods() As Boolean 'Added by Saylee on 21-Aug-2008
        SetObject()
        SetGridObject()
        Dim mAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriod
        For Each mAssemblyMonitorModStatusPeriod In mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
            If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mAssemblyMonitorModStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        If Not (mMachineMaintenanceList.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then 'Added by Saylee on 9th-Oct-2009
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 7, txtDoneOnDate.Text.ToString, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
        End If
        With mMachineMaintenance
            .MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID
			' .Date = txtDoneOnDate.Text
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
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtDoneOnDate.Text.ToString, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
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
        If mAssemblyMonitorModStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID)
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
                If (Not mAssemblyMonitorModStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyMonitorModStatus.ID)
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
    Private Sub SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
        If Not mAssemblyMonitorModStatus Is Nothing Then
            If mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is System.DBNull.Value Then
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
    End Sub 'End
    Private Sub UpdatePanel()
        upnlDoneOnDetails.Update()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
        upnlDocumentDetails.Update()
        upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        upnlSelectMonitoringService.Update()
        upnlRevisedDetails.Update() 'Revise Activity
    End Sub
    Private Sub SetSession()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 9th-Oct-2009
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        txtDoneOnDate.Text = mAssemblyMonitorModStatus.DoneOnFormatted.ToString  'Added on 28-05-2007 by Saylee
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList() 'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours + ")"
        BindLicenceNo() 'MLNo
        txtEffectiveFromDate.Text = mAssemblyMonitorModStatus.AsOnDateFormatted.ToString 'Added By Vikrant on 11-Dec-2019 For ALL11122019-1
        DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        dgCurrentValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "txtRemark" Then
        '    If Len(txtRemark.Text) > 500 Then
        '        custValidator.ErrorMessage = "Max. length of Remark should be 500 char"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        If custValidator.ControlToValidate = "txtlicenceno" Then 'Added By Utkarsh On 13-Jun-2012 FOR ALL08062012
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If 'End
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtElapsedValue As TextBox
        Dim txtRemainingValue As TextBox
        If Not mAssemblyMonitorModStatus.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorModStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyMonitorModStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by vikrant on 27-July-2011
        If Not IsPostBack Then
            If btnSelect.Enabled = True Then
                setFocus(btnSelect)
            End If
            DataFieldBind()
            ControlToVisibility()
            SetTitle()
            SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If
    End Sub
    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        SetObject()
        SetGridObject()
        Response.Redirect("wfModelMonitorModList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfAssemblyMonitorModStatus_Ajax.aspx")
    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtElapsedValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                '  .Item(i).IsCalculate = chkcalValues.Checked
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
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                '  .Item(i).IsCalculate = chkcalValues.Checked
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
        For i As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        DataBindGrid()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
    End Sub
    '--------------End
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDoneOnDate As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtDoneOnDate = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                ' .Item(i).IsCalculate = chkcalValues.Checked
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(txtDoneOnDate.Text.Trim) Then
                        .Item(i).CurrentValueFormatted = ""
                    Else
                        .Item(i).CurrentValueFormatted = Trim(txtDoneOnDate.Text)
                    End If
                Else
                    .Item(i).CurrentValue = Trim(txtDoneOnDate.Text)
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
            mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
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
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged, txtEffectiveFromDate.TextChanged 'txtEffectiveFromDate.TextChanged Added By Vikrant on 11-Dec-2019 For ALL11122019-1
        'If IsPostBack Then
        '    SetObject()
        '    DataBindGrid()
        '    upnlRedLabel.Update()
        '    upnlCurrentValue.Update()
        '    upnlDoneOnValue.Update()
        'End If

        'Added By Vikrant on 11-Dec-2019 For ALL11122019-1
        Dim txtBox As TextBox
        txtBox = CType(sender, TextBox)
        If txtBox.ID = "txtDoneOnDate" Then
            If Not txtBox.Text = "" AndAlso IsDate(txtBox.Text) Then
                txtEffectiveFromDate.Text = txtDoneOnDate.Text
                txtEffectiveFromDate.Enabled = False
            Else
                txtEffectiveFromDate.Enabled = True
                GoTo NextStatement
            End If
        End If
        'End

        'Added by Saylee on 11-Jul-2018 for ALL21062018, to show current values as per Done On Date selection
        If IsPostBack Then
            Dim tmpmAssemblyMonitorModStatus As AssemblyMonitorModStatus = mAssemblyMonitorModStatus.Clone
            If tmpmAssemblyMonitorModStatus.IsNew Then
                If txtBox.Text <> "" Then
                    mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, txtBox.Text, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                Else
                    'Revise Activity
                    'mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Session("mIssueDate"), mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                    If tmpmAssemblyMonitorModStatus.ModelMonitorMod.ReviseRemark <> "" Then
                        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Today.Date.ToString, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                    Else
                        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Session("mIssueDate"), mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                    End If
                    'End

                End If
                With mAssemblyMonitorModStatus
                    Dim mModelMonitorMod As ModelMonitorMod = CType(Session("mModelMonitorMod"), ModelMonitorMod)
                    .ModelMonitorModID(True) = mModelMonitorMod.ID
                    '.ModelMonitorMod.Code = mModelMonitorMod.Code
                    .ModelMonitorMod.Reference = mModelMonitorMod.Reference
                    .ModelMonitorMod.Description = mModelMonitorMod.Description
                    .ModelMonitorMod.RequiredManHours = mModelMonitorMod.RequiredManHours

                End With
            Else

                If txtBox.Text = "" Then
                    mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(tmpmAssemblyMonitorModStatus.ID, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mMachine.HourType, tmpmAssemblyMonitorModStatus.AsOnDateFormatted, mAssemblyStatus.Assembly.ModelID, False)
                Else
                    mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(tmpmAssemblyMonitorModStatus.ID, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mMachine.HourType, txtBox.Text, mAssemblyStatus.Assembly.ModelID, True)
                End If
            End If
            'Added By Vikrant on 11-Dec-2019 For ALL11122019-1
            If txtBox.ID = "txtEffectiveFromDate" Then
                mAssemblyMonitorModStatus.AsOnDate = System.DBNull.Value 'Explicitly set DBNull to make object dirty.In AssemblyMonitorModStatus.vb DataPortal_Fetch AsOnDate is set so obj is marked as dirty
                If txtEffectiveFromDate.Text = "" Then
                    mAssemblyMonitorModStatus.AsOnDate = System.DBNull.Value
                Else
                    mAssemblyMonitorModStatus.AsOnDate = txtEffectiveFromDate.Text
                End If
            End If
            'End
NextStatement:
            DataBindGrid()
            upnlRedLabel.Update()
            upnlCurrentValue.Update()
            upnlDoneOnValue.Update()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If ((mAssemblyStatus.IsMaster) And ((Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew))) Or ((Not mAssemblyStatus.IsMaster) And ((Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew And Not User.IsInRole("AssemblyModificationsNew") And mAssemblyMonitorModStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew And (Not User.IsInRole("AssemblyModificationsEdit") And Not mAssemblyMonitorModStatus.IsNew)) Or (Not User.IsInRole("AssemblyModificationsNew") And mAssemblyMonitorModStatus.IsNew) Or (Not User.IsInRole("AssemblyModificationsEdit") And Not mAssemblyMonitorModStatus.IsNew))) Then
            'AssemblyModificationsEdit line in above if condition, Added by Saylee on 8-Jul-2020 for  All08072020 
            SetObject()
            SetSession()
            mDirectiveDetail = "Model : " + mMachineMaintenanceList.Item(mMachineMaintenanceList.CurrentIndex).ModelName + " Serial No. : " + mMachineMaintenanceList.Item(mMachineMaintenanceList.CurrentIndex).SerialNo + "Directive No. : " + mMachineMaintenanceList.Item(mMachineMaintenanceList.CurrentIndex).ModNumber + " Monitor Type : " + txtModelMonitorModTypeName.Text
            MarkLog(Util.Action.Save, "Assembly Directive Status", User.Identity.Name & " is not Authorized User to save " & mDirectiveDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
            Exit Sub
        End If
        If IsValid Then
            If CheckPeriods() = False Then
                If mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is System.DBNull.Value Then 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
                    MSGBoxCtrl.show("Save Alert!", "You are about to comply one time directive status.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    Exit Sub
                End If 'End
                If Save() = True Then
                    SetTitle()
                    UpdatePanel()
                    If Not txtDoneOnDate.Text = "" Then
                        btnBack_Click(sender, e)
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
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Session("NewPage") = "True" Or mAssemblyMonitorModStatus.ModelMonitorMod.ReviseRemark <> "" Then
            Session("NewPage") = "False"
            MarkLog(Util.Action.Close, "Assembly Directive Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Changed by Vikrant on 29-July-2011
            RemoveSession()
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            MarkLog(Util.Action.Close, "Assembly Directive Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Changed by Vikrant on 29-July-2011
            RemoveSession()
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mAssemblyMonitorModStatus.IsAttachmentAdded = True
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
        mAssemblyMonitorModStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorModStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorModStatus.ID)
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
            Session("mMaintenanceID") = mAssemblyMonitorModStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyMonitorModStatus.DoneOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtRequiredManHours.DataBind()
        upnlDoneOnDetails.Update()
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
            If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, 7, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If
        Else
            If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlDoneOnDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            upnlDoneOnDetails.Update()
        End If
    End Sub
    'End
    'Revise Activity
    Private Sub btnRevise_Click(sender As Object, e As System.EventArgs) Handles btnRevise.Click
        MSGBoxCtrl.show("Alert!", "You are about to Revise Model Activity.After revision of model activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
    End Sub
    'End
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        SetObject()
        SetGridObject()
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mAssemblyMonitorModStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", mAssemblyMonitorModStatus.AsOnDate.ToString, txtDoneOnDate.Text))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            Dim LogID As String
            LogID = CType(Session("LogID"), String)
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            If mAssemblyMonitorModStatus.IsNew = False Then 'Edit record
                'Commented & added By Vikrant On 14-Dec-2020 to solve issue:after select log done on date was also getting changed,which shouldnt happen
                'mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mAssemblyMonitorModStatus.ID, _
                '                                                                                 mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, _
                '                                                                                 mMachine.HourType, mLog.Date.ToString, _
                '                                                                                 mAssemblyStatus.Assembly.ModelID, True, _
                '                                                                                 mLogID:=mLog.ID.ToString, IsFromMain:=True)
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mAssemblyMonitorModStatus.ID, _
                                                                                                 mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, _
                                                                                                 mMachine.HourType, txtEffectiveFromDate.Text, _
                                                                                                 mAssemblyStatus.Assembly.ModelID, True, _
                                                                                                 mLogID:=mLog.ID.ToString, IsFromMain:=True)
            Else
                mAssemblyMonitorModStatus.LogID(LogID, mLog.Date.ToString, True, CType(Session("mModelMonitorMod"), ModelMonitorMod)) = New Guid(LogID)
            End If
            Session.Remove("FromLog")
            DataBindGrid()
            ControlToVisibility()
            SetTitle()
            upnlCurrentValue.Update()
            upnlDoneOnValue.Update()
        End If
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
        Rpt = New crDetAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList
        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Type ", _
                  txtModelMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                      dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, _
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Type ", _
                            txtModelMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                             "", "", , "", , ""))
                End If

            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                            txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number", _
                             txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number", _
                            txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Note", _
                                   txtDirNote.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Note", _
                                    txtDirNote.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , lblNote.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 6
        RHCount1 = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
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
            'dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText, _
            'dgDoneOnValue.Columns.Item(5).HeaderText))
            ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Done On", _
            New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
            dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, , _
            dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText, _
            dgDoneOnValue.Columns.Item(6).HeaderText))
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
                    txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    ))
                    ' CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), _
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                        txtWorkOrdNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                    mAssemblyMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                        mAssemblyMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                   CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                        txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours ", _
                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Actual Man Hours ", _
                        txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                    txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                   CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Remark", _
                        txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                    "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                     CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                                          "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                                 "", "", , "", , "", "", , lblNote1.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    , lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
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
            dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, "Extension Date ", _
            dgDoneOnValue.Columns.Item(3).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(4).HeaderText, _
            dgDoneOnValue.Columns.Item(5).HeaderText))
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
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Assembly Directive Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

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