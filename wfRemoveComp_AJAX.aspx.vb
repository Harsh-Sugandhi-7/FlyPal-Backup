
'AJAX Conversion By: Saylee on 26-Mar-2015 : ModuleID:307


Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Public Class wfRemoveComp_AJAX
    Inherits System.Web.UI.Page

#Region " Enum "
    Public Enum From
        NewRemove = 1
        EditRemove = 2
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
    Public mCompStatus As CompStatus
    Public mPrevCompStatus As CompStatus
    Public mRemovalReasonList As RemovalReasonList
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mFrom As From
    Public Flag As Boolean
    Dim LogID As String

    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 8th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 8th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Public mEmployeeList As EmployeeList
    Public mManufacturerList As ManufacturerList 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
    Public mSubATAList As SubATAList 'Added By Utkarsh On 02-Apr-2013 For ALL01042013
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013

    'Added By Saylee On 27-Nov-2014 
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mRemovalReasonList = CType(Session("mRemovalReasonList"), RemovalReasonList)
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mFrom = CType(Session("From"), From)
        mPrevCompStatus = CType(Session("mPrevCompStatus"), CompStatus)
        LogID = CType(Session("LogID"), String)

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 8th-Oct-2009
        mManufacturerList = Session("mManufacturerList") 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        mSubATAList = Session("mSubATAList") 'Added By Utkarsh On 02-Apr-2013 For ALL01042013
        'Added By Saylee On 27-Nov-2014 
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
    End Sub
    Private Sub setSession()
        Session("mCompStatus") = mCompStatus
        Session("mRemovalReasonList") = mRemovalReasonList
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mPrevCompStatus") = mPrevCompStatus
        Session("From") = mFrom

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 8th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList            'Added by Saylee on 8th-Oct-2009
        Session("mManufacturerList") = mManufacturerList 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        Session("mSubATAList") = mSubATAList 'Added By Utkarsh On 02-Apr-2013 For ALL01042013

        'Added By Saylee On 27-Nov-2014 
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRemovalReasonList")

        Session.Remove("mMachineMaintenance")       'Added by Saylee on 8th-Oct-2009
        Session.Remove("mMachineMaintenanceList")       'Added by Saylee on 8th-Oct-2009
        Session.Remove("mManufacturerList") 'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        Session.Remove("mSubATAList") 'Added By Utkarsh On 02-Apr-2013 For ALL01042013

        'Added By Saylee On 27-Nov-2014 
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End

        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetObject()
        With mCompStatus
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

            'Added By Saylee on 24-Apr-2009
            mCompStatus.RemDoneBy = Trim(txtRemDoneBy.Text)
            '==================================
            .RemPlace = txtPlace.Text.Trim

            '.RemDoneByID = New Guid(cmbRemovedBy.SelectedValue)
            '.RemLicenseNo = txtLicenceNo.Text.Trim


            'Added By Shweta On 12-Jun-2012 FOR ALL08062012

            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .RemLicenseNo = LicenseNo
            .RemDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            'End
            .IsRemUnschedule = chkIsRemUnscheduled.Checked 'Added By Vikrant On 22-Aug-2012 For ALL20082012

            .ManufacturerID = New Guid(cmbManufacturerList.SelectedValue)
            .SubATAID = New Guid(cmbSubATAChpater.SelectedValue) 'Added By Utkarsh On 02-Apr-2013 For ALL01042013

            'Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394
            .IsFanBladeDistribution = False
            .FanBladePosition = 0
            .MomentWeight = 0
            .BalanceScrew = 0
            'End of Added by Prashant on 13-Oct-2022 for Fan Blade Distribution  FLYPAL-394

            'Added By Saylee On 27-Nov-2014 
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsRemAttachmentAdded = True
                Else
                    .IsRemAttachmentAdded = False
                End If
            End If
            'End
        End With
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub SetPage()
        lbltitle.Text = "Removal of Component " & " from " & mAssemblyStatus.AssemblyTypeName
        lblEngineInfo.InnerText = "Part and Serial No. of the Component"

        lblRemovalInfo.InnerText = "Removal Information of the Component"

        If Session("From") = 2 Then
            btnTechDirection.Enabled = True
            calRemove.Enabled = False
            btnSelectLog.Enabled = False
            lblScrollNote.Visible = True
            lnkPrintLogBookEntry.Enabled = True   'Added By Prashant 7-May-20201 ALL07052021
        Else
            If Session("Saved") = True Then
                btnTechDirection.Enabled = True
                calRemove.Enabled = False
                btnSelectLog.Enabled = False
                lblScrollNote.Visible = True
                lnkPrintLogBookEntry.Enabled = True   'Added By Prashant 7-May-20201 ALL07052021
            Else
                btnTechDirection.Enabled = False
                calRemove.Enabled = True
                btnSelectLog.Enabled = True
                lblScrollNote.Visible = False
                lnkPrintLogBookEntry.Enabled = False  'Added By Prashant 7-May-20201 ALL07052021
            End If

        End If
    End Sub
    Private Sub SetFormClone(ByVal clnCompStatus As CompStatus)
        mCompStatus.RemovalWONo = clnCompStatus.RemovalWONo
        mCompStatus.RemovalReasonID = clnCompStatus.RemovalReasonID
        mCompStatus.RemovalReasonName = clnCompStatus.RemovalReasonName
        mCompStatus.RemovalRemark = clnCompStatus.RemovalRemark
        mCompStatus.RemovedOn = clnCompStatus.RemovedOn

        mCompStatus.RemDoneByID = clnCompStatus.RemDoneByID
        mCompStatus.RemLicenseNo = clnCompStatus.RemLicenseNo
        mCompStatus.RemPlace = clnCompStatus.RemPlace
        'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompStatus.MaintenanceDoneByEmployees
            If Not mCompStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            Else
                If Not mCompStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                    mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                    'mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                    mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                End If
            End If
        Next
        'End
        clnCompStatus = Nothing
    End Sub
    Private Sub SetLog()
        'If Val(Request.QueryString("Type")) = -1 Then
        '    Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
        '    Dim LogDate = Request.QueryString("LogDate")
        '    If DateDiff(DateInterval.Day, SmartDate.StringToDate(mCompStatus.RemovedOn.ToString), SmartDate.StringToDate(calRemove.Text)) <> 0 Then
        '        Dim clnCompStatus As CompStatus = mCompStatus.Clone
        '        If mFrom = From.NewRemove Then
        '            mCompStatus = CompStatus.NewRemovalCompStatus(mPrevCompStatus.ID, calRemove.Text, mAssemblyStatus.ID, Guid.Empty.ToString)
        '        Else
        '            mCompStatus = CompStatus.GetRemovalCompStatus(mPrevCompStatus.ID, mAssemblyStatus.ID, calRemove.Text, Guid.Empty.ToString)
        '        End If
        '        SetFormClone(clnCompStatus)
        '    End If

        '    'Added by Saylee on 8th-Oct-2009
        '    Dim mLog As Log
        '    mLog = Log.GetLog(New Guid(LogId.ToString))
        '    Session("mLog") = mLog
        '    '===================================
        'End If

        If CType(Session("FromLog"), Boolean) = True Then
            Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
            Dim LogDate = CType(Session("LogDate"), String)
            Dim tmpCompStatus As CompStatus
            If Not mCompStatus Is Nothing Then
                tmpCompStatus = mCompStatus
            End If
            If mFrom = From.NewRemove Then
                mCompStatus = CompStatus.NewRemovalCompStatus(mPrevCompStatus.ID, calRemove.Text, mAssemblyStatus.ID, LogId.ToString)
            Else
                mCompStatus = CompStatus.GetRemovalCompStatus(mPrevCompStatus.ID, mAssemblyStatus.ID, calRemove.Text, LogId.ToString)
            End If
            If Not tmpCompStatus Is Nothing Then
                mCompStatus.RemovalReasonID = tmpCompStatus.RemovalReasonID
                mCompStatus.RemovalReasonName = tmpCompStatus.RemovalReasonName
                mCompStatus.RemovalWONo = tmpCompStatus.RemovalWONo
                mCompStatus.RemovalRemark = tmpCompStatus.RemovalRemark
                mCompStatus.IsExpired = tmpCompStatus.IsExpired

                mCompStatus.RemDoneByID = tmpCompStatus.RemDoneByID
                mCompStatus.RemLicenseNo = tmpCompStatus.RemLicenseNo
                mCompStatus.RemPlace = tmpCompStatus.RemPlace
                'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
                For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In tmpCompStatus.MaintenanceDoneByEmployees
                    If Not mCompStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                        mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                    Else
                        If Not mCompStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                            mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                            mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                            mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                            mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                        End If
                    End If
                Next
                'End
            End If
            'DataFieldBind()
            dgRemovalValue.DataSource = mCompStatus.CompStatusPeriods
            dgRemovalValue.DataBind()
            'Added Code By Saylee 
            calRemove.Text = mCompStatus.RemovedOnFormatted
            Session("mCompStatus") = mCompStatus
            Session.Remove("FromLog")


            'Added by Saylee on 8th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog
            '===========================================
            'Else
            'If Not IsPostBack And CType(Session("sender"), String) = "" Then Session.Remove("mLog") 'Added by Saylee on 10-Jan-2014
        End If
    End Sub
    Private Function Save() As Boolean
        If Not IsValid Then Exit Function
        Dim CompStatusClone As CompStatus
        CompStatusClone = CType(mCompStatus.Clone, CompStatus)
        SetObject()
        SetMachineMaintenanceObject()  'Added by Saylee on 8th-Oct-2009
        If mCompStatus.IsValid = True And mCompStatus.IsDirty = True Then
            Try
                'Added By Shweta On 07-Aug-2013 For ALL01082013
                If Not mCompStatus.RemDoneByID.Equals(Guid.Empty) AndAlso Not mCompStatus.RemovedOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompStatus.RemDoneByID.ToString, mCompStatus.RemovedOn.ToString)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ' ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message))
                        '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, , False), True)
                        MSGBoxCtrl.show(title, message, "", MsgBoxStyle.OkOnly, "")
                        Return False
                    End If
                End If
                'End
                mCompStatus.ApplyEdit()
                mCompStatus = CType(mCompStatus.Save(), CompStatus)
                If Not mFileAttach Is Nothing Then
                    SaveAttachment() 'Added By Vikrant On 01-Dec-2014
                End If

                SaveMachineMaintenance()  'Added by Saylee on 8th-Oct-2009
                Session("mCompStatus") = mCompStatus
                Return True
            Catch ex As SqlException
                Session("CompStatusClone") = CompStatusClone
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
            Finally
                CompStatusClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        Save()
                        'Response.Redirect("wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        DataFieldBind()
                        GetAttachment()
                        SetPage()
                        ControlVisibilityForAttachment()
                        upnlEngineInfo.Update()
                        upnlRemovalInfo.Update()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        ' Response.Redirect("wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        'Response.Redirect("wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    upnlEngineInfo.Update()
                    upnlRemovalInfo.Update()
                    'Response.Redirect("wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    upnlEngineInfo.Update()
                    upnlRemovalInfo.Update()
                    'Response.Redirect("wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 8th-Oct-2009
        If (mFrom = From.NewRemove) And Not (mMachineMaintenanceList.Contains(mCompStatus.ID, 4, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 4, calRemove.Text, mCompStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompStatus.ID, 4)
            Session("mMachineMaintenance") = mMachineMaintenance
        End If

        With mMachineMaintenance
            .MachineID = mAssemblyStatus.MachineID
            .MaintenanceActivityTypeID = 4
            .MaintenanceID = mCompStatus.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatus.ID

            .Date = calRemove.Text

            Dim mLog As Log = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                'Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(calRemove.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
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
                'Session("mMachineMaintenance") = mMachineMaintenance
                Session.Remove("mMachineMaintenance")
            Catch ex As Exception

            End Try
        End If
        ''End If
    End Sub
    REM:-Restore the values of the variable.
    Private Sub CopyFromClone(ByVal ClonedCompStatus As CompStatus)
        mCompStatus.RemovalWONo = ClonedCompStatus.RemovalWONo
        mCompStatus.RemovalReasonID = ClonedCompStatus.RemovalReasonID
        mCompStatus.RemovalReasonName = ClonedCompStatus.RemovalReasonName
        mCompStatus.RemovalRemark = ClonedCompStatus.RemovalRemark
        mCompStatus.RemDoneByID = ClonedCompStatus.RemDoneByID
        mCompStatus.RemLicenseNo = ClonedCompStatus.RemLicenseNo
        mCompStatus.RemPlace = ClonedCompStatus.RemPlace
        'Commented and Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
        'For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In ClonedCompStatus.MaintenanceDoneByEmployees
        '    mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
        'Next
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In ClonedCompStatus.MaintenanceDoneByEmployees
            If Not mCompStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            Else
                If Not mCompStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                    mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                    'mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                    mCompStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                End If
            End If

        Next
        'End
        'MLNo

        'End
    End Sub
    'Added By Vikrant On 01-Dec-2014
    Private Sub ControlVisibilityForAttachment()
        'If mFileAttach.Size > 0 Then
        If mCompStatus.IsRemAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mCompStatus.IsRemAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 2) 'Sort = 2 : Removal
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '
        mFileAttach.ReferenceID = mCompStatus.ID
        If mFileAttach.Size > 0 Then
            Try
                mFileAttach.Save()
                'mFileAttach = Nothing
                'Session("mFileAttach") = mFileAttach
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), False)
            End Try
        Else
            If (Not mCompStatus.IsNew) And IsAttachmentDeleted Then
                FileAttach.DeleteAttachment(mFileAttach.ID, mCompStatus.ID, 2)
            End If
            IsAttachmentDeleted = False
            Session("IsAttachmentDeleted") = IsAttachmentDeleted
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mCompStatus.IsRemAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 2)
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
    'End 'MLNo
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
    'added by Saylee on 16-Feb-2017 to show proper PeriodUnit for Technical Direction
    Public Function GetPeriodUnitID(PeriodID As Integer) As Integer
        Select Case PeriodID
            Case 1
                Return 1
            Case 2
                Return 0
            Case 3
                Return 6
            Case 4
                Return 7
            Case 5
                Return 8
            Case 6
                Return 9
            Case 7
                Return 10
            Case 8
                Return 11
            Case 9
                Return 12
            Case 10
                Return 13
            Case 11
                Return 14
            Case 12
                Return 15
            Case 13
                Return 16
            Case 14
                Return 17
            Case 15
                Return 18
        End Select
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        cmbReason.DataSource = mRemovalReasonList
        Session("mRemovalReasonList") = mRemovalReasonList
        dgRemovalValue.DataSource = mCompStatus.CompStatusPeriods
        'Added Code By Saylee 
        calRemove.Text = mCompStatus.RemovedOnFormatted

        'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        '========================================
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(SELECT)")
        'cmbRemovedBy.DataSource = mEmployeeList
        Session("mEmployeeList") = mEmployeeList

        'Added By Utkarsh On 31-Jan-2013 For ALL30122013
        mManufacturerList = ManufacturerList.GetManufacturerList(, "(SELECT)")
        cmbManufacturerList.DataSource = mManufacturerList
        Session("mManufacturerList") = mManufacturerList
        'End
        'Added By Utkarsh On 02-Apr-2013 For ALL01042013
        mSubATAList = SubATAList.GetSubATAList(mCompStatus.ATAID, "", "(SELECT)")
        cmbSubATAChpater.DataSource = mSubATAList
        Session("mSubATAList") = mSubATAList
        'End

        BindLicenceNo() 'MLNo

        DataBind()

        If Not mCompStatus.IsNew Then
            If Not mCompStatus.SubATAID.Equals(Guid.Empty) Then cmbSubATAChpater.SelectedValue = mCompStatus.SubATAID.ToString
        End If
        '=============Added by Saylee on 11th-Jan-2008 (Maintenance)==============================
        If cmbReason.Items.Contains(New System.Web.UI.WebControls.ListItem(mCompStatus.RemovalReasonName, mCompStatus.RemovalReasonID.ToString)) Then
            cmbReason.SelectedValue = mCompStatus.RemovalReasonID.ToString
        Else
            cmbReason.SelectedValue = Guid.Empty.ToString
        End If
    End Sub
    Private Sub DataGridBind()
        Session("mCompStatus") = mCompStatus
        dgRemovalValue.DataSource = mCompStatus.CompStatusPeriods
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
        ElseIf custValidator.ControlToValidate = "cmbReason" Then
            If cmbReason.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please select Reason from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

            'Added By Shweta On 12-Jun-2012 FOR ALL08062012
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
        SetObject()
        Dim str As String = ""
        If Not mCompStatus.IsValid Then
            For i As Integer = 0 To mCompStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgRemovalValue.Rows.Count - 1)
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
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack Then
            setFocus(txtWorkOrderNo)
            DataFieldBind()
            '  GetAttachment()
            SetLog()
            SetPage()
            ControlVisibilityForAttachment() 'Added by Vikrant On 02-Dec-2014

            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If


    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("AssemblyRemovalNew") And mCompStatus.IsNew) Or (Not User.IsInRole("AssemblyRemovalEdit") And Not mCompStatus.IsNew) Then
            SetObject()
            setSession()
            'Changed By Utkarsh On 26-Jul-2011 For All19072011
            MaintDetail = "Reg No. : " & Machine.GetMachine(mAssemblyStatus.MachineID).RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo
            MarkLog(Util.Action.Save, "ComponentRemoval", User.Identity.Name & " is not Authorized User to save " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End 'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            If Save() Then
                'Added by Saylee on 14-July-2009
                Session("mAircraftInformationBoardList") = Nothing
                Session("Saved") = True
                '**********************************************
                DataFieldBind()
                SetPage()
                ControlVisibilityForAttachment()
                upnlEngineInfo.Update()
                upnlRemovalInfo.Update()
                upnlScrollNote.Update()
                'MLNo
                Session.Remove("mMaintenanceDoneByEmployees")
                Session.Remove("UserNameForLicenceList")
                'End
                'Response.Redirect("wfRemovedAssembly.aspx?BackPage=" & Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If

    End Sub
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        SetObject()
        Session.Remove("FromLog")
        Session.Remove("mLogList")

        Session("mFromType") = 4
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mAssemblyStatus.ID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(calRemove.Text = "", Today.Date.ToShortDateString, calRemove.Text))
        ' Response.Redirect("wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage6=wfRemoveComp.aspx&FromType=4&DoneOn=" & calRemove.Text & "&MachineId=" & mAssemblyStatus.MachineID.ToString & "&AssemblyStatusID=" & mAssemblyStatus.ID.ToString & "&AssemblyID=" & mAssemblyStatus.AssemblyID.ToString)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
    End Sub
    '  Private Sub imgbtnReason_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnReason.Click
    Private Sub imgReason_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReason.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemovalReasonWindow", "OpenRemovalReasonWindow()", True)
        'Response.Redirect("wfRemovalReason_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfRemoveComp_AJAX.aspx&Type=" & mCompStatus.AssemblyTypeID)
    End Sub
    Private Sub imgManufacturer_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgManufacturer.Click
        'Response.Redirect("wfManufacturer_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfRemoveComp.aspx&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManufacturerWindow", "OpenManufacturerWindow()", True)
    End Sub
    Private Sub calRemove_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calRemove.TextChanged
        Try
            If DateDiff(DateInterval.Day, SmartDate.StringToDate(mCompStatus.RemovedOn.ToString), SmartDate.StringToDate(calRemove.Text)) <> 0 Then
                SetObject()
                setSession()
                Dim clnCompStatus As CompStatus = mCompStatus.Clone
                If mFrom = From.NewRemove Then
                    mCompStatus = CompStatus.NewRemovalCompStatus(mPrevCompStatus.ID, calRemove.Text, mAssemblyStatus.ID, Guid.Empty.ToString)
                Else
                    mCompStatus = CompStatus.GetRemovalCompStatus(mPrevCompStatus.ID, mAssemblyStatus.ID, calRemove.Text, Guid.Empty.ToString)
                End If
                Session.Remove("mLog")
                SetFormClone(clnCompStatus)
                DataGridBind()
            End If
        Catch ex As Exception

        Finally
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        SetObject()
        'Changed By Utkarsh On 26-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "ComponentRemoval", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        Session.Remove("FromLog")
        Session.Remove("mLog")
        Session.Remove("Saved")
        'Added By Saylee On 27-Nov-2014 
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    'Added By Saylee On 27-Nov-2014 
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCompStatus.IsRemAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlAttach.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        GetAttachment()
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

    'Added by utkarsh on 07-Jan-2014
    Private Sub btnTechDirection_Click(sender As Object, e As System.EventArgs) Handles btnTechDirection.Click
        If IsValid Then
            Dim mtechDirection As rptTechDirection = rptTechDirection.GetTechDirection(mCompStatus.ID, 2, mCompStatus.RemovedOn.ToString) '2 for compoenent
            If mtechDirection.IsNew Then 'there is no entry for current component.
                mtechDirection = rptTechDirection.NewTechDirection(mCompStatus.ID, 2, mCompStatus.RemovedOn.ToString)
            End If
            If mCompStatus.RemovalReasonName = "(SELECT)" Then
                mtechDirection.RemovalReason = ""
            Else
                mtechDirection.RemovalReason = mCompStatus.RemovalReasonName
            End If
            '  mtechDirection.Date = mCompStatus.RemovedOn  'Commented by Saylee on 27-Mar-2017 as date should be TDdate and not Removal date
            mtechDirection.RemovalDate = mCompStatus.RemovedOn

            Dim mAssemblyList As AssemblyList = AssemblyList.GetAssemblyList(1, mAssemblyStatus.MachineID.ToString, calRemove.Text)
            mtechDirection.ATA = mCompStatus.ATAChapter
            mtechDirection.PartNo = mCompStatus.PartName
            mtechDirection.Description = mCompStatus.Description
            mtechDirection.SerialNo = mCompStatus.SerialNo
            mtechDirection.ModelName = mAssemblyList(0).ModelName 'mAssemblyStatus.ModelName
            mtechDirection.AircaftName = mAssemblyList(0).RegNo 'MachineNameValueList.GetMachineList(mCompStatus.RemovedOn, mAssemblyStatus.MachineID.ToString)(0).RegNo
            mtechDirection.AircaftSrNo = mAssemblyList(0).SerialNo 'mAssemblyStatus.Assembly.SerialNo
            mtechDirection.IsRemUnschedule = mCompStatus.IsRemUnschedule
            mtechDirection.Position = mCompStatus.Position 'Added By Prashant 3-Jun-2022
            'mtechDirection.TimeSinceNew = String.Join(", ", From c In mCompStatus.CompStatusPeriods Select c.CompRemovalValueFormatted)
            mtechDirection.TimeSinceNew = String.Join(", ", From c As CompStatusPeriod In mCompStatus.CompStatusPeriods Select New Period(c.PeriodID, c.CompRemovalValue, GetPeriodUnitID(c.PeriodID), CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)
            Session("mrptTechDirection") = mtechDirection

            'Added By Saylee on 10-July-2015
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompStatus.ID, 4)
            Session("TechLog") = mMachineMaintenance.LogID.ToString
            '******************************
            Response.Redirect("wfTechDirection.aspx?BackPage=wfRemoveComp_Ajax.aspx&BackPage1=" & Request.QueryString("BackPage"))
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnSelectLog_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSelectLog.Click
        SetLog()
        upnlRemovalInfo.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompStatus.IsRemAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompStatus.ID, 2)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompStatus.ID, Sort:=2) 'Sort = 2 : Removal
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub hdnBtnRemovalReason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemovalReason.Click
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        cmbReason.DataSource = mRemovalReasonList
        cmbReason.DataBind()
        Session("mRemovalReasonList") = mRemovalReasonList
        If Not mCompStatus.RemovalReasonID.Equals(Guid.Empty) Then
            cmbReason.SelectedValue = mCompStatus.RemovalReasonID.ToString
        End If
        upnlRemovalInfo.Update()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompStatus.ID
            Session("MaintenanceDoneOnDate") = mCompStatus.RemovedOn.ToString
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
                mCompStatus.MaintenanceDoneByEmployees.Add(mCompStatus.ID, MaintActivityTypeID.ComponentRemoval, DoneByID, LicenseNo, "", EmpName)
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

        Dim EventLogDetail As String = "Printed From Component Removal through maintenance with As On Date: " + New SmartDate(Today.Date.ToString, False).FormattedText + " , Part: " + txtDescription.Text + " , Serial No.: " + txtSerialNo.Text.Trim
        Dim ReportData As Flypal.ReportData
        If ObjHistoryCard.Count > 0 Then
            ReportData = New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             "", "Component History Card Report", New SmartDate(Today.Date.ToString, False).FormattedText, "", txtPart.Text, txtSerialNo.Text, ObjHistoryCard(0).ATA, AppSettings("Product Version"), AppSettings("SINote"), txtDescription.Text, "", "", "Assembly", AppSettings("Logo"))

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

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mCompStatus.RemovedOn, mCompStatus.RemovedOn, "", mAssemblyStatus.AssemblyTypeName, _
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "", _
                                                        mAssemblyStatus.MachineID.ToString, True, True, IsRemoved:=True, IsInstalled:=False, _
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True, _
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False, _
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mCompStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "OpenFromAssemblyRemovalInstallationComponentRemovalInstallation", mCompStatus.RemovedOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo, _
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo, IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper), _
           AppSettings("Product Version"), AppSettings("SINote"), _
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.", _
           "True", mCompStatus.RemovedOnFormatted, "", AppSettings("Logo"))

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
#End Region


#Region " Report "
    'Created By :- Pallavi , Date -10/08/2006
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        If (Not User.IsInRole("ComponentRemovalPrint")) Then
            'Commented By Utkarsh On 26-Jul-2011 For All19072011
            'MarkLog(Util.Action.Print, "CompRemoval", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'End
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            msg.ReplacePage = "wfRemoveComp.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            msg.Show()
            Exit Sub
        End If

        Rpt = New crDetInstallRemoveComp
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Assembly and Component Info
        Dim LHCount As Integer
        'Commented And Added By Utkarsh On 02-Apr-2013 For ALL01042013
        'LHCount = 5
        LHCount = 6
        'End
        ReportDetails.Add(New rptStatus(, 0, lblEngineInfo.InnerText))
        Dim I As Integer
        For I = 0 To LHCount - 1
            'Added By Utkarsh On 02-Apr-2013 For ALL01042013
            If I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, , "ATA Chapter", _
    txtATAChapter.Text, , , , , , , , , , , , , , , , , "", _
    "", "", , ""))
                'End
            ElseIf I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, , lblPart.Text, _
    txtPart.Text, , , , , , , , , , , , , , , , , "", _
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
        'Commented And Added By Utkarsh On 02-Apr-2013 For ALL01042013
        ' LHCount1 = 4
        LHCount1 = 5
        'End
        RHCount1 = Me.mCompStatus.CompStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If
        ReportDetails.Add(New rptStatus(, 2, , , , , , lblRemovalInfo.InnerText, , , , , , , , , , , , , , lblValuesAtRemoval.Text))
        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 3, , , , lblEngineInfo.InnerText, _
             New SmartDate(calRemove.Text).FormattedText, , , , , , , , , , , , , , , , _
            dgRemovalValue.Columns.Item(0).HeaderText, dgRemovalValue.Columns.Item(1).HeaderText, _
            , dgRemovalValue.Columns.Item(2).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 3, , , , lblEngineInfo.InnerText, _
                            New SmartDate(calRemove.Text).FormattedText, , , , , , , , , , , , , , , , "", "", , ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            'Changed by By Utkarsh On 02-Apr-2013 For ALL01042013 'Added coding for Sub ATA chapter
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblSubATA.Text, _
                     IIf(cmbSubATAChpater.SelectedIndex <= 0, "", cmbSubATAChpater.SelectedItem.Text), , , , , , , , , , , , , , , , _
                CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), _
                CType(Me.mCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , , , lblSubATA.Text, _
                   IIf(cmbSubATAChpater.SelectedIndex <= 0, "", cmbSubATAChpater.SelectedItem.Text), , , , , , , , , , , , , , , , "", "", , ""))
                End If
                'End
            ElseIf m = 1 Then 'Changed by By Utkarsh from (m=0 to m=1) On 02-Apr-2013 For ALL01042013 
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text, _
                     txtWorkOrderNo.Text, , , , , , , , , , , , , , , , _
                CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), _
                CType(Me.mCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text, _
                   txtWorkOrderNo.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 2 Then 'Changed by By Utkarsh from (m=1 to m=2) On 02-Apr-2013 For ALL01042013 
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblReason.Text, _
                     cmbReason.SelectedItem.Text, , , , , , , , , , , , , , , , _
                    CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), _
                    CType(Me.mCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                    CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblReason.Text, _
                                     cmbReason.SelectedItem.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 3 Then 'Changed by By Utkarsh from (m=2 to m=3) On 02-Apr-2013 For ALL01042013 
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text, _
                     txtNote.Text, , , , , , , , , , , , , , , , _
                    CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), _
                    CType(Me.mCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                    CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text, _
                                    txtNote.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 4 Then 'Changed by By Utkarsh from (m=3 to m=4) On 02-Apr-2013 For ALL01042013 
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , "", _
                    "", "", "", , , , , , , , , , , , , , , , _
                    CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), _
                    CType(Me.mCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                    CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , "", _
                                          "", "", "", , , , , , , , , , , , , , , , "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 4, , "", _
                 "", "", "", , , , , , , , , , , , , , , , _
                 CType(Me.mCompStatus.CompStatusPeriods(m).PeriodName, String), _
                 CType(Me.mCompStatus.CompStatusPeriods(m).CompRemovalValueFormatted, String), , _
                CType(Me.mCompStatus.CompStatusPeriods(m).AssemblyRemovalValueFormatted, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Remove Component Status Detail Report", lbltitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)

        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 26-Jul-2011 For All19072011
        ' MarkLog(Util.Action.Print, "CompRemoval", RemovalComp + " -> " + "Remove Component Status Detail Report", Util.ErrorType.NoError, mCompStatus.ID)
        'End
        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetLicenceList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        'Dim itemlist As ItemListAutoComplete
        'itemlist = ItemListAutoComplete.GetItemList(prefixText, False)

        Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, "", , , False)
        If count = 0 Then
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
        Else
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
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

End Class