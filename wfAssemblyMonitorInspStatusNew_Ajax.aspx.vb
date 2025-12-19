Imports System.Linq
Public Class wfAssemblyMonitorInspStatusNew_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mMachine As Machine
    Private Flag As Int16
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 12th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 12th-Oct-2009
    Dim EventLogID As Guid      'Added By Utkarsh On 1-Aug-2011 For All19072011
    Dim MachineDetail As String 'Added By Utkarsh On 1-Aug-2011 For All19072011
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
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
        mMachine = CType(Session("mMachine"), Machine)
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 12th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 12th-Oct-2009
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorInspStatus")
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
        If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub ControlToVisibility()
        btnPrint.Enabled = Not mAssemblyMonitorInspStatus.IsNew
        btnSelectMonitoringInspection.Enabled = mAssemblyMonitorInspStatus.IsNew
        REM: For No Frequency
        dgCurrentValue.Columns(3).Visible = (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3)
        dgCurrentValue.Columns(4).Visible = (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(4).Visible = (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(5).Visible = (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3)
        'Added By Shweta ON 28-Jun-2013 FOR ALL28062013
        dgDoneOnValue.Columns(6).Visible = (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        'End
        If mAssemblyMonitorInspStatus.ModelMonitorInspID.Equals(Guid.Empty) Then
            txtDoneOnDate.BackColor = Color.Gainsboro
            txtDoneOnDate.Enabled = False
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True
            txtWorkOrderNo.BackColor = Color.Gainsboro
            txtWorkOrderNo.ReadOnly = True
            chkApplicable.Enabled = False
        End If
        If txtRemark.ReadOnly Then txtRemark.BackColor = Color.Gainsboro
        If txtWorkOrderNo.ReadOnly Then txtWorkOrderNo.BackColor = Color.Gainsboro
        If Not (mAssemblyMonitorInspStatus.ModelMonitorInsp.ReadOnlyFrequencyColumn) = False Then txtDoneOnDate.Enabled = False
        If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If
        'Revise Activity
        btnRevise.Enabled = (mAssemblyMonitorInspStatus.IsApplicable And Not mAssemblyMonitorInspStatus.IsNew)
        'End
        btnSelectLog.Visible = (mIsSpareAssembly <> 1) ' Added By Vikrant On 27-Jul-2020 For ALL27072020
        ControlVisibilityForAttachment()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()

    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean, ByVal AssemblyMonitorInspStatusID As Guid)
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(AssemblyMonitorInspStatusID, mAssemblyStatus.ID, mAssemblyStatus.HourType, IIf(Session("IsOpenFromMPD") = "True", True, False), DoneOnDate)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
    End Sub
    Private Sub SetObject()
        With mAssemblyMonitorInspStatus
            If txtDoneOnDate.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = txtDoneOnDate.Text
            End If
            .DoneWONo = Trim(txtWorkOrderNo.Text)
            .DoneRemark = Trim(txtRemark.Text)
            .IsApplicable = chkApplicable.Checked
            .SourceDoc = Trim(txtSourceDoc.Text)
            .RevisionNo = Trim(txtRevisionNo.Text)
            .BookNo = Trim(txtBookNo.Text)
            .PageNo = Trim(txtPageNo.Text)
            .RequiredManHours = Trim(txtRequiredManHours.Text)
            .IsLater = chkIsLater.Checked          'Added By Prashant 17-Aug-2010
            Dim LicenseNo As String = String.Empty ' Added By Utkarsh On 12-Jun-2012 FOR ALL08062012
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
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
        End With
        If mAssemblyMonitorInspStatus.IsNew Then mAssemblyMonitorInspStatus.IsMaster = False 'Added by Saylee on 25-Jun-2018 for ALL21062018, to edit master record added in Maintenance section (after AsOnDate)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, txtDoneOnDate, txtDueOnValue, txtExtensionValue As TextBox
        If mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3 Then
            For i As Integer = 0 To Me.dgCurrentValue.Rows.Count - 1
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            Next i
        End If
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDoneOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox) 'Added By Shital on 25-Jan-2021
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
    End Sub
    Private Function Save() As Boolean
        Dim AssemblyMonitorInspStatusClone As AssemblyMonitorInspStatus
        AssemblyMonitorInspStatusClone = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mAssemblyMonitorInspStatus.IsValid = True Then
            If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Assembly Inspection.Assembly Inspection Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Dim mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList 'aded By Deven on 24-Sep-2009 ------
            mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString, ModelMonitorInspID:=mAssemblyMonitorInspStatus.ModelMonitorInspID.ToString)
            If mAssemblyMonitorInspStatusList.Contains(mAssemblyMonitorInspStatus.ModelMonitorInspID) And mAssemblyMonitorInspStatus.IsNew = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Assembly Inspection Status.", MsgBoxStyle.OkOnly, "")
                Return False
            End If  '---------------------------------------
            Try
                If Not mAssemblyMonitorInspStatus.DoneByID.Equals(Guid.Empty) AndAlso mAssemblyMonitorInspStatus.DoneOn.ToString.Length > 0 Then 'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                    Dim Title As String = "Save Alert !"
                    Dim Message As String = ""
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorInspStatus.DoneByID.ToString, mAssemblyMonitorInspStatus.DoneOn.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
                        Return False
                    End If
                End If 'End
                mAssemblyMonitorInspStatus.ApplyEdit()
                mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)

                'Revise Activity
                If Not Session("mPrevAssemblyMonitorInspStatusForRevise") Is Nothing Then
                    Dim IsLinkedActivitySelected As Boolean = IIf(Session("IsLinkedActivitySelected") Is Nothing, False, Session("IsLinkedActivitySelected")) 'Revise Activity New
                    If IsLinkedActivitySelected Then
                        Dim mPrevAssemblyMonitorInspStatusForRevise As AssemblyMonitorInspStatus
                        mPrevAssemblyMonitorInspStatusForRevise = Session("mPrevAssemblyMonitorInspStatusForRevise")
                        mPrevAssemblyMonitorInspStatusForRevise.IsApplicable = False
                        mPrevAssemblyMonitorInspStatusForRevise.Save()
                        Session.Remove("mPrevAssemblyMonitorInspStatusForRevise")
                    End If

                End If
                'End

                SaveAttachment()
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                ControlToVisibility()
                MachineDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- Description : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & " Monitor Type : " & mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelMonitorInspTypeName
                MarkLog(Util.Action.Save, "Assembly Inspection Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("AssemblyMonitorInspStatusClone") = AssemblyMonitorInspStatusClone
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                AssemblyMonitorInspStatusClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub ControlVisibilityForAttachment()
        If mAssemblyMonitorInspStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
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
                If (Not mAssemblyMonitorInspStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyMonitorInspStatus.ID)
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
    Private Sub SetTitle()
        Dim AssemblyInfo As String = "[Model: " & mAssemblyStatus.ModelName & " SerialNo: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        If mAssemblyMonitorInspStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Inspection Status " & AssemblyInfo & " [New]"
        Else
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Inspection Status" & AssemblyInfo
        End If
        lblAssemblyValues.InnerText = mAssemblyStatus.AssemblyTypeName & " Values"
        upnlTitle.Update()
    End Sub
    Private Sub UpdatePanel()
        upnlMonitoringStatusDetails.Update()
        upnlCurrentValue.Update()
        upnlDoneOnValue.Update()
        upnlDocumentDetails.Update()
        upnlExtensionDetails.Update()
        upnlActionBtn.Update()
        upnlSelectMonitoringInspection.Update()
        upnlRevisedDetails.Update() 'Revise Activity
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
                        MarkLog(Util.Action.[New], "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mModelMonitorInsp As ModelMonitorInsp
                        Dim ID As Guid = Guid.NewGuid
                        'Revise Activity New
                        Dim mModelMonitorInspList As ModelMonitorInspList
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, GetRecordsByPrevRefID:=True, PrevRefID:=mAssemblyMonitorInspStatus.ModelMonitorInsp.PrevRefID.ToString)
                        If mModelMonitorInspList.Count > 1 Then
                            For i As Integer = mModelMonitorInspList.Count - 1 To 0 Step -1
                                If mModelMonitorInspList(i).ID.Equals(mAssemblyMonitorInspStatus.ModelMonitorInsp.ID) Then
                                    Exit For
                                Else
                                    Session("ModelMonitorInspIDToBeLinked") = mModelMonitorInspList(i).ID.ToString
                                    Session("ModelMonitorInspPrevRefIDToBeLinked") = mModelMonitorInspList(i).PrevRefID.ToString
                                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                                    Session("mPrevAssemblyMonitorInspStatusForRevise") = mAssemblyMonitorInspStatus
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfModelMonitorInspList_Ajax.aspx?BackPage=Index.aspx');", True)
                                    Exit Sub
                                End If
                            Next
                        End If
                        'END
                        mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(mAssemblyMonitorInspStatus.ModelMonitorInsp, mMachine.HourType)
                        Session("mModelMonitorInsp") = mModelMonitorInsp
                        RemoveSession()
                        mModelMonitorInsp.BeginEdit()
                        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                        Session("mPrevAssemblyMonitorInspStatusForRevise") = mAssemblyMonitorInspStatus
                        Session("IsLinkedActivitySelected") = True 'Revise Activity New
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow();", True)
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("Sender") = ""
                        UpdatePanel()
                    End If
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.Close, "Assembly Inspection Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod
        For Each mAssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
            If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mAssemblyMonitorInspStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        If Not (mMachineMaintenanceList.Contains(mAssemblyMonitorInspStatus.ID, 6, "")) Then 'Added by Saylee on 9th-Oct-2009
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 6, txtDoneOnDate.Text, mAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatus.ID, 6)
        End If
        With mMachineMaintenance
            .MaintenanceID = mAssemblyMonitorInspStatus.ID 'TransactionID
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
    Private Sub SetColor() 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
        If Not mAssemblyMonitorInspStatus Is Nothing Then
            If mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And Not mAssemblyMonitorInspStatus.DoneOn Is System.DBNull.Value Then
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
    Private Sub SetSession()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 9th-Oct-2009
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
        txtDoneOnDate.Text = mAssemblyMonitorInspStatus.DoneOnFormatted.ToString  'Added on 28-05-2007 by Saylee
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList() 'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours + ")"
        BindLicenceNo() 'MLNo
        DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        dgCurrentValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
            'Added By Utkarsh On 12-Jun-2012 FOR ALL08062012
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
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtElapsedValue As TextBox
        Dim txtRemainingValue As TextBox
        If Not mAssemblyMonitorInspStatus.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 28-July-2011
        If Not IsPostBack Then
            If btnSelectMonitoringInspection.Enabled = True Then
                setFocus(btnSelectMonitoringInspection)
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
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For I As Integer = 0 To dgCurrentValue.Rows.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(I).FindControl("txtElapsedValue"), TextBox)
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
        For i As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
            mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
        '  If ((mAssemblyStatus.IsMaster) And ((Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew))) Or ((Not mAssemblyStatus.IsMaster) And ((Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew))) Then
        If ((mAssemblyStatus.IsMaster) And ((Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew))) Or ((Not mAssemblyStatus.IsMaster) And ((Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew And Not User.IsInRole("AssemblyInspectionsNew") And mAssemblyMonitorInspStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew And (Not User.IsInRole("AssemblyInspectionsEdit") And Not mAssemblyMonitorInspStatus.IsNew)) Or (Not User.IsInRole("AssemblyInspectionsNew") And mAssemblyMonitorInspStatus.IsNew) Or (Not User.IsInRole("AssemblyInspectionsEdit") And Not mAssemblyMonitorInspStatus.IsNew))) Then
            '''AssemblyModificationsEdit line in above if condition, Added by Saylee on 8-Jul-2020 for  All08072020   SetObject()
            SetSession()
            MachineDetail = "Model : " + mMachineMaintenanceList.Item(mMachineMaintenanceList.CurrentIndex).RegNo + " Serial No : " + mMachineMaintenanceList.Item(mMachineMaintenanceList.CurrentIndex).SerialNo + " Description : " + txtDescription.Text.Trim + " Inspection Type : " + txtMonitorInspectionType.Text
            MarkLog(Util.Action.Save, "Assembly Inspection Status", User.Identity.Name & " is not Authorized User to save " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
            Exit Sub
        End If
        If IsValid Then
            If CheckPeriods() = False Then
                If mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And Not mAssemblyMonitorInspStatus.DoneOn Is System.DBNull.Value Then 'Added By Utkarsh On 16-May-2012 FOR ALL15052012
                    MSGBoxCtrl.show("Save Alert!", "You are about to comply one time inspection status.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
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
    Private Sub btnSelectMonitoringInspection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectMonitoringInspection.Click
        SetObject()
        SetGridObject()
        Response.Redirect("wfModelMonitorInspList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=wfAssemblyMonitorInspStatus_Ajax.aspx")
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        'If IsPostBack Then
        '    SetObject()
        '    DataBindGrid()
        '    upnlRedLabel.Update()
        '    upnlCurrentValue.Update()
        '    upnlDoneOnValue.Update()
        'End If

        'Added by Saylee on 11-Jul-2018 for ALL21062018, to show current values as per Done On Date selection
        If IsPostBack Then
            Dim tmpmAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = mAssemblyMonitorInspStatus.Clone
            If tmpmAssemblyMonitorInspStatus.IsNew Then
                If txtDoneOnDate.Text <> "" Then
                    mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, txtDoneOnDate.Text, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                Else
                    'Revise Activity
                    'mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Session("mIssueDate"), mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                    If tmpmAssemblyMonitorInspStatus.ModelMonitorInsp.ReviseRemark <> "" Then
                        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Today.Date.ToString, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                    Else
                        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Session("mIssueDate"), mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                    End If
                    'End
                End If
                With mAssemblyMonitorInspStatus
                    Dim mModelMonitorInsp As ModelMonitorInsp = CType(Session("mModelMonitorInsp"), ModelMonitorInsp)
                    .ModelMonitorInspID(True) = mModelMonitorInsp.ID
                    '.ModelMonitorInsp.Code = mModelMonitorInsp.Code
                    .ModelMonitorInsp.Reference = mModelMonitorInsp.Reference
                    .ModelMonitorInsp.Description = mModelMonitorInsp.Description
                    .ModelMonitorInsp.RequiredManHours = mModelMonitorInsp.RequiredManHours

                End With
            Else
                'Added by Saylee on 11-Jul-2018, to show current values as per Done On Date selection
                If txtDoneOnDate.Text = "" Then
                    mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(tmpmAssemblyMonitorInspStatus.ID, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mMachine.HourType, tmpmAssemblyMonitorInspStatus.AsOnDateFormatted, mAssemblyStatus.Assembly.ModelID, False)
                Else
                    mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(tmpmAssemblyMonitorInspStatus.ID, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mMachine.HourType, txtDoneOnDate.Text, mAssemblyStatus.Assembly.ModelID, True)
                End If
            End If

            DataBindGrid()
            upnlRedLabel.Update()
            upnlCurrentValue.Update()
            upnlDoneOnValue.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Session("NewPage") = "True" Or mAssemblyMonitorInspStatus.ModelMonitorInsp.ReviseRemark <> "" Then 'Revise Activity
            Session("NewPage") = "False"
            MarkLog(Util.Action.Close, "Assembly Inspection Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            RemoveSession()
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            MarkLog(Util.Action.Close, "Assembly Inspection Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            RemoveSession()
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mAssemblyMonitorInspStatus.IsAttachmentAdded = True
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
        mAssemblyMonitorInspStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorInspStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorInspStatus.ID)
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
            Session("mMaintenanceID") = mAssemblyMonitorInspStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyMonitorInspStatus.DoneOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
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
            If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, 6, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If
        Else
            If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            upnlMonitoringStatusDetails.Update()
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
        Session("mAssemblyStatusId") = mAssemblyMonitorInspStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", mAssemblyMonitorInspStatus.AsOnDate.ToString, txtDoneOnDate.Text))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            Dim LogID As String
            LogID = CType(Session("LogID"), String)
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            If mAssemblyMonitorInspStatus.IsNew = False Then 'Edit record 
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mAssemblyMonitorInspStatus.ID, _
                                                                                                   mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, _
                                                                                                   mMachine.HourType, mLog.Date.ToString, _
                                                                                                   mAssemblyStatus.Assembly.ModelID, True, _
                                                                                                   mLogID:=mLog.ID.ToString, IsFromMain:=True)
            Else
                mAssemblyMonitorInspStatus.LogID(LogID, mLog.Date.ToString, True, CType(Session("mModelMonitorInsp"), ModelMonitorInsp)) = New Guid(LogID)
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
        LHCount = 4
        RHCount = Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Insp. Type ", _
                  txtMonitorInspectionType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                      dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, _
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Insp. Type ", _
                            txtMonitorInspectionType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                             "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                            txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , lblNote.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                           CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , , lblNote.Text))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 6
        RHCount1 = Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count
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
                    txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    ))
                    ' CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), _
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Work Order No.", _
                        txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                        "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                    mAssemblyMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                   CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "License No.", _
                        mAssemblyMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "Place", _
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                   CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
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
                  CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
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
                   CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
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
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                                          "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                                 "", "", , "", , "", "", , lblNote1.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Done On Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
               CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), _
                    , lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count
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
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark", _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Assembly Inspection Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

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