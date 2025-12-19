'AJAX Conversion by vikrant on 24-Apr-2015
Imports System.Linq
Public Class wfInstallAssembly_Ajax
    Inherits System.Web.UI.Page

#Region " Enum "
    Public Enum From
        NewInstall = 1
        EditInstall = 2
    End Enum
#End Region

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mRemovedAssemblyStatus As AssemblyStatus
    Public mMachineNameValueList As MachineNameValueList
    Public mModelList As ModelList
    Public mSelectPeriods As SelectPeriods
    Public mSelectPeriod As SelectPeriod
    Public mPeriodList As PeriodList
    Public mCurrentDate As String
    Public mFromType As From
    Dim Flag As Integer
    Public mATAList As ATAList
    Public mInstallAssemblyStatusInfo As String   'Code Added 29,Jan,2007
    Public mInstallAssemblyStatus As AssemblyStatus 'Code Added 29,Jan,2007

    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 6th-Oct-2009

    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009
    'Added by Vikrant on 26-July-2011
    Dim EventLogID As Guid
    Public mAssemblyDetail As String
    Public mEmployeeList As EmployeeList
    Public mEmployeeStatus As EmployeeStatus 'Added By Shweta On 07-Aug-2013 For ALL01082013
    'Added By Vikrant On 01-Dec-2014
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End
    Public mtmpInstalledCompList As tmpInstalledCompList
    Public mCompStatus As CompStatus
    Public mEvtLogID As Guid
    Public mAircraft As String
    Public mAssemblyType As String
    Public mAssemblyInfo As String
    Public mDetail As String
    Public TabIndex As Integer = 0
    Public mBoardInfo As AircraftInformationBoard.BoardInfo
    Dim mModelMaintenanceActivityListCount As ModelMaintenanceActivityListCount
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
#Region "Service Tab"
    Public mModelMonitorServiceTypeList As ModelMonitorServiceTypeList
    Public mComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
    Public LookInCombo, TextFor, TextCode, SearchForCombo As String
#End Region
#Region "Inspection Tab"
    Public mModelMonitorInspTypeList As ModelMonitorInspTypeList
    Public mComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
#End Region
#Region "Directive Tab"
    Public mModelMonitorModTypeList As ModelMonitorModTypeList
    Public mComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList
#End Region
#Region "Parameters Tab"
    Public mParameterList As ParameterList
#End Region

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mRemovedAssemblyStatus = CType(Session("mRemovedAssemblyStatus"), AssemblyStatus)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mModelList = CType(Session("mModelList"), ModelList)
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        mSelectPeriod = CType(Session("mSelectPeriod"), SelectPeriod)
        mPeriodList = CType(Session("mPeriodList"), PeriodList)
        mFromType = CType(Session("FromType"), From)
        mATAList = CType(Session("mATAList"), ATAList)
        mInstallAssemblyStatus = CType(Session("mInstallAssemblyStatus"), AssemblyStatus)   'Code Added 29,Jan,2007

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 6th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 6th-Oct-2009
        'Added By Vikrant On 01-Dec-2014
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
        mtmpInstalledCompList = Session("mtmpInstalledCompList")
        TabIndex = Session("AssemblyInstTabIndex")
        mComplyAssemblyMonitorServiceStatusList = CType(Session("mComplyAssemblyMonitorServiceStatusList"), tmpComplyAssemblyMonitorServiceStatusList)
        mComplyAssemblyMonitorInspStatusList = CType(Session("mComplyAssemblyMonitorInspStatusList"), tmpComplyAssemblyMonitorInspStatusList)
        mComplyAssemblyMonitorModStatusList = CType(Session("mComplyAssemblyMonitorModStatusList"), tmpComplyAssemblyMonitorModStatusList)
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
    End Sub
    Private Sub SetSession()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mRemovedAssemblyStatus") = mRemovedAssemblyStatus
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mModelList") = mModelList
        Session("mSelectPeriods") = mSelectPeriods
        Session("mSelectPeriod") = mSelectPeriod
        Session("mPeriodList") = mPeriodList
        Session("mMachine") = mMachine
        Session("FromType") = mFromType
        Session("mATAList") = mATAList
        Session("mInstallAssemblyStatus") = mInstallAssemblyStatus   'Code Added 29,Jan,2007
        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 6th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList            'Added by Saylee on 6th-Oct-2009
        'Added By Vikrant On 01-Dec-2014
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSession()
        mRemovedAssemblyStatus = Nothing
        mAssemblyStatus = Nothing
        mMachineNameValueList = Nothing
        mModelList = Nothing
        mPeriodList = Nothing
        mFromType = Nothing
        Session.Remove("mRemovedAssemblyStatus")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mModelList")
        Session.Remove("mSelectPeriods")
        Session.Remove("mSelectPeriod")
        Session.Remove("mPeriodList")
        Session.Remove("mMachine")
        Session.Remove("FromType")
        Session.Remove("IsExistingAssembly")
        Session.Remove("mATAList")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 6th-Oct-2009
        Session.Remove("mMachineMaintenanceList")       'Added by Saylee on 6th-Oct-2009
        'Added By Vikrant On 01-Dec-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'MLNo
    Public Sub SetLicenceCount()
        If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteCompList" Then
                        Try
                            Session("sender") = ""
                            'Added by Vikrant on 1-Aug-2011
                            mEvtLogID = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).CompStatusID
                            mAircraft = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).MachineInfo
                            mAssemblyType = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).AssemblyType
                            mAssemblyInfo = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).AssemblyInfo
                            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
                            'End
                            mtmpInstalledCompList = CType(Session("mtmpInstalledCompList"), tmpInstalledCompList)
                            CompStatus.DeleteCompStatus(mtmpInstalledCompList(mtmpInstalledCompList.CurrentIndex).CompStatusID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate)
                            DataFieldBindComponentList()
                            upnlGridComponentList.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Assembly Installation", "Can't delete :" & mDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'mEnquiry.ID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Assembly Installation", mDetail, Util.ErrorType.NoError, mEvtLogID, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteDirective" Then
                        Try
                            Session("sender") = ""
                            'Added by Vikrant on 1-Aug-2011
                            mEvtLogID = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID
                            mAircraft = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).MachineInfo
                            mAssemblyType = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyType
                            mAssemblyInfo = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyInfo
                            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
                            'End

                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mComplyAssemblyMonitorModStatusList(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, 7)
                            '=============================
                            'Added By Vikrant On 25-Nov-2014
                            If mComplyAssemblyMonitorModStatusList(mComplyAssemblyMonitorModStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mComplyAssemblyMonitorModStatusList(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID)
                            End If
                            'End
                            AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(mComplyAssemblyMonitorModStatusList(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            'Added By Vikrant On 25-Nov-2014
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'End
                            Session("mMachineMaintenance") = mMachineMaintenance
                            FindNowDirective()
                            SetGridDirective()
                            SetPageDirective()
                            ControlVisibilityDirective()
                            upnlGridDirective.Update()
                            upnlActionBtnDirective.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Assembly Installation", "Can't delete :" & mDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Assembly Installation", mDetail, Util.ErrorType.NoError, mEvtLogID, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteService" Then
                        Try
                            Session("sender") = ""
                            'Added by Vikrant on 1-Aug-2011
                            mEvtLogID = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID
                            mAircraft = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).MachineInfo
                            mAssemblyType = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyType
                            mAssemblyInfo = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyInfo
                            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
                            'End
                            mComplyAssemblyMonitorServiceStatusList = CType(Session("mComplyAssemblyMonitorServiceStatusList"), tmpComplyAssemblyMonitorServiceStatusList)
                            'Added by Saylee on 13th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mComplyAssemblyMonitorServiceStatusList(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID, 5)
                            '=============================
                            'Added By Vikrant On 25-Nov-2014
                            If mComplyAssemblyMonitorServiceStatusList(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mComplyAssemblyMonitorServiceStatusList(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID)
                            End If
                            'End

                            AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(mComplyAssemblyMonitorServiceStatusList(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            'Added By Vikrant On 25-Nov-2014
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'End
                            Session("mMachineMaintenance") = mMachineMaintenance
                            FindNowService()
                            SetGridService()
                            SetPageService()
                            ControlVisibilityService()
                            upnlGridService.Update()
                            upnlActionBtnService.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Assembly Installation", "Can't delete :" & mDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Assembly Installation", mDetail, Util.ErrorType.NoError, mEvtLogID, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteInspection" Then
                        Try
                            Session("sender") = ""
                            'Added by Vikrant on 1-Aug-2011
                            mEvtLogID = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID
                            mAircraft = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).MachineInfo
                            mAssemblyType = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyType
                            mAssemblyInfo = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyInfo
                            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
                            'End
                            'Added by Saylee on 12th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mComplyAssemblyMonitorInspStatusList(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID, 6)
                            '=============================
                            'Added By Vikrant On 25-Nov-2014
                            If mComplyAssemblyMonitorInspStatusList(mComplyAssemblyMonitorInspStatusList.CurrentIndex).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mComplyAssemblyMonitorInspStatusList(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID)
                            End If
                            'End
                            AssemblyMonitorInspStatus.DeleteAssemblyMonitorInspStatus(mComplyAssemblyMonitorInspStatusList(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            'Added By Vikrant On 25-Nov-2014
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'End
                            Session("mMachineMaintenance") = mMachineMaintenance
                            FindNowInspection()
                            SetGridInspection()
                            SetPageInspection()
                            ControlVisibilityInspection()
                            upnlGridInspection.Update()
                            upnlActionBtnInspection.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "InstallAssemblyMonitorModStatusList", "Can't delete :" + mDetail + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Assembly Installation", mDetail, Util.ErrorType.NoError, mEvtLogID, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "InstallExistingAssemblyWithNewValue" Then
                        mAssemblyStatus.Assembly.InstallExistingAssemblyWithNewValue = True
                        Session("mAssemblyStatus") = mAssemblyStatus
                        Page.Validate("1")
                        If Save() Then

                            'Added by Saylee on 14-July-2009
                            Session("mAircraftInformationBoardList") = Nothing
                            '*********************************
                            SetCaptions()
                            ControlVisibility()
                            ControlVisibilityForTabs()
                            upnlTitle.Update()
                            upnlInstallationDetails.Update()
                            upnlInstallationValues.Update()
                            upnlActionBtn.Update()
                            upnlContainer.Update()
                        End If
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If

    End Sub
    'Added By Vikrant On 01-Dec-2014
    Private Sub ControlVisibilityForAttachment()
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                ImageButton1.Visible = True
                btnDelAttach.Enabled = True
            Else
                ImageButton1.Visible = False
            End If
        End If
    End Sub
    Private Sub ControlVisibilityForTabs()
        tbPnlComponent.Visible = IIf(mAssemblyStatus.IsNew, False, True)
        tbPnlService.Visible = IIf(mAssemblyStatus.IsNew, False, True)
        tbPnlInspection.Visible = IIf(mAssemblyStatus.IsNew, False, True)
        tbPnlDirective.Visible = IIf(mAssemblyStatus.IsNew, False, True)
        tbPnlParameters.Visible = IIf(mAssemblyStatus.IsNew, False, True)
    End Sub
    Private Sub GetAttachment()
        If mAssemblyStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyStatus.ID, 1)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAssemblyStatus.ID
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                    'mFileAttach = Nothing
                    'Session("mFileAttach") = mFileAttach
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mAssemblyStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyStatus.ID, 1)
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
    'End
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mAssemblyStatus.IsNew
        lnkPrintLogBookEntry.Enabled = Not mAssemblyStatus.IsNew 'Added By Prashant 7-May-20201 ALL07052021
        cmbModelList.Enabled = (From.NewInstall And Session("IsExistingAssembly") = False) Or (From.EditInstall And mAssemblyStatus.Sort = 1)
        ' txtDispSerialNo.ReadOnly = (mFromType.NewInstall And Session("IsExistingAssembly") = False) Or (mFromType.EditInstall And mAssemblyStatus.Sort = 1)
        'Added by Saylee on 2-Nov-2009
        txtSerialNo.Enabled = (From.NewInstall And Session("IsExistingAssembly") = False) Or (From.EditInstall And mAssemblyStatus.Sort = 1)
        '*****************************
        If (From.NewInstall And Session("IsExistingAssembly") = False) Then
            txtSerialNo.Enabled = True
            cmbModelList.Enabled = True
        ElseIf (From.EditInstall And mAssemblyStatus.Sort = 1) Then
            txtSerialNo.Enabled = False
            cmbModelList.Enabled = False
        End If
        If Not mAssemblyStatus.IsNew And mAssemblyStatus.Sort >= 1 And IsDate(mAssemblyStatus.LogDate) And IsDate(mAssemblyStatus.RemovedOn) Then
            txtInstalledOnDate.Enabled = DateDiff(DateInterval.Day, CType(mAssemblyStatus.LogDate, Object), CType(mAssemblyStatus.RemovedOn, Object)) < 0
        End If
        REM:-Enabling the +/- periods buttons, only if the AssemblyType is Engine and no log entry exist for this assembly as on date.

        'Commented and added by Saylee on 11-Mar-2013 for ALL11032013 - 1
        'btnAddPeriod.Enabled = (mAssemblyStatus.AssemblyTypeID = 2 Or mAssemblyStatus.AssemblyTypeID = 4) And (mAssemblyStatus.HasLogCount = False) And _
        '                       ((mFromType.NewInstall And Session("IsExistingAssembly") = False) Or (mFromType.EditInstall And mAssemblyStatus.Sort = 1))

        ''REM: Enabling (now open) the +/- preiods button, for all assemblies and no log entry exist for this assembly as on date.
        btnAddPeriod.Enabled = (Not mAssemblyStatus.AssemblyTypeID = 1) And (mAssemblyStatus.HasLogCount = False) And
                             ((From.NewInstall And Session("IsExistingAssembly") = False) Or (From.EditInstall And mAssemblyStatus.Sort = 1))


        'Commented and added by Saylee on 14-Mar-2013 for ALL14032013-1
        ''With dgInstallationValue
        ''    .Columns(4).Visible = (mAssemblyStatus.AssemblyTypeID = 2 Or mAssemblyStatus.AssemblyTypeID = 4) And (mAssemblyStatus.HasLogCount = False) And _
        ''                       ((mFromType.NewInstall And Session("IsExistingAssembly") = False) Or (mFromType.EditInstall And mAssemblyStatus.Sort = 1))
        ''End With
        With dgInstallationValue
            .Columns(4).Visible = (mAssemblyStatus.AssemblyTypeID <> 1) And (mAssemblyStatus.HasLogCount = False) And
                               ((From.NewInstall And Session("IsExistingAssembly") = False) Or (From.EditInstall And mAssemblyStatus.Sort = 1))
        End With


        'Clear date is not expected on this form 
        'Because of DataBinding is not available to this Control we have to write this line.
        'calInstalledOn.ShowClearButton = False


        If (Not mRemovedAssemblyStatus Is Nothing AndAlso Not mRemovedAssemblyStatus.ID.Equals(Guid.Empty)) And mFromType = From.NewInstall Then
            txtSerialNo.Enabled = False
            cmbModelList.Enabled = False
        ElseIf (Not mRemovedAssemblyStatus Is Nothing AndAlso mRemovedAssemblyStatus.ID.Equals(Guid.Empty)) And mFromType = From.NewInstall Then
            txtSerialNo.Enabled = True
            cmbModelList.Enabled = True
        ElseIf mFromType = From.EditInstall And (Not mInstallAssemblyStatus Is Nothing AndAlso mInstallAssemblyStatus.Sort = 1) Then
            txtSerialNo.Enabled = True
            cmbModelList.Enabled = True
        ElseIf mFromType = From.EditInstall And (Not mInstallAssemblyStatus Is Nothing AndAlso mInstallAssemblyStatus.Sort > 1) Then
            txtSerialNo.Enabled = False
            cmbModelList.Enabled = False
        End If


        '********************************************
        'Added by Saylee on 4-Nov-2009
        'to lock calInstalledOn and AssemblyInstallationValue if any log or Monitor entry is done
        If Not mAssemblyStatus.IsNew Then
            ''Dim HasFoundMonitorEntry As Boolean = False
            ''For i As Integer = 0 To mAssemblyStatus.AssemblyStatusPeriods.Count - 1
            ''    '' If mAssemblyStatus.AssemblyStatusPeriods.Item(i).HasMonitorCount(mAssemblyStatus.ID, mAssemblyStatus.AssemblyStatusPeriods.Item(i).PeriodID) = True Then
            ''    If mAssemblyStatus.AssemblyStatusPeriods.Item(i).HasMonitor = True Then
            ''        HasFoundMonitorEntry = True
            ''        Exit For
            ''    End If
            ''Next
            'Dim mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
            'mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString)

            'Dim mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
            'mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString)

            'Dim mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
            'mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString)

            Dim mMaintenanceActivityCountOfAircraft As MaintenanceActivityCountOfAircraft
            mMaintenanceActivityCountOfAircraft = MaintenanceActivityCountOfAircraft.GetCount(mMachine.ID, 4, mMachine.AssemblyStatus.AsOnDateFormatted.ToString)

            ''If HasFoundMonitorEntry = True or mAssemblyStatus.HasLogCount = True Then
            If (mMaintenanceActivityCountOfAircraft.MaintActivityCount > 0) Or (mAssemblyStatus.HasLogCount = True) Then
                txtInstalledOnDate.Enabled = False

                For i As Integer = 0 To dgInstallationValue.Rows.Count - 1
                    Dim txtAssemblyInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)
                    CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox).ReadOnly = True
                Next
            End If
            'mAssemblyMonitorServiceStatusList = Nothing
            'mAssemblyMonitorInspStatusList = Nothing
            'mAssemblyMonitorModStatusList = Nothing
        End If
        '********************************************
        ControlVisibilityForAttachment() 'Added By Vikrant On 01-Dec-2014
        ControlVisibilityForTabs()
    End Sub
    Private Sub SetObject()
        With mAssemblyStatus
            .Assembly.ModelID = New Guid(cmbModelList.SelectedValue)
            .ATAID = New Guid(cmbATAChapter.SelectedValue)
            .MachineID = New Guid(cmbMachineList.SelectedValue) '' Added New On 15-09-2006 Rajnish
            .Position = txtPosition.Text.Trim
            .InstallationWONo = txtWorkOrNo.Text.Trim
            .InstallationRemark = txtNote.Text.Trim
            .Assembly.SerialNo = txtSerialNo.Text.Trim
            If txtInstalledOnDate.Text = "" Then
                .InstalledOn = DBNull.Value
            Else
                .InstalledOn = txtInstalledOnDate.Text
            End If
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
            .InstallationReason = Trim(txtInstallationReason.Text) 'Added By Vikrant On 09-Apr-2014 For ALL09042014-1
            'Added By Vikrant On 01-Dec-2014
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If
            'End
        End With
        Session("mAssemblyStatus") = mAssemblyStatus

    End Sub

    Private Sub SetGridObject()
        For i As Integer = 0 To dgInstallationValue.Rows.Count - 1
            Dim txtAssemblyInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)
            If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And txtAssemblyInstallationValue.Text.Trim = "" Then 'This If Condition added by vikrant on 19-Jun-2020 to save 0 instead of null if nothing enetered in TextBox
                mAssemblyStatus.AssemblyStatusPeriods(i).AssemblyInstallationValueFormatted = New Period(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID, 0).Value
            Else
                mAssemblyStatus.AssemblyStatusPeriods(i).AssemblyInstallationValueFormatted = txtAssemblyInstallationValue.Text.Trim
            End If
        Next
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub

    Private Function Save() As Boolean
        If Not IsValid Then Exit Function
        Dim clnAssemblyStatus As AssemblyStatus = mAssemblyStatus.Clone
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 6th-Oct-2009
        If mAssemblyStatus.IsValid = True Then
            Try
                'Added By Shweta On 07-Aug-2013 For ALL01082013
                If Not mAssemblyStatus.InstDoneByID.Equals(Guid.Empty) AndAlso Not mAssemblyStatus.InstalledOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyStatus.InstDoneByID.ToString, mAssemblyStatus.InstalledOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, , False), True)
                        Return False
                    End If
                End If
                'End
                mAssemblyStatus.ApplyEdit()
                mAssemblyStatus = CType(mAssemblyStatus.Save, AssemblyStatus)
                SaveAttachment() 'Added By Vikrant On 01-Dec-2014
                SaveMachineMaintenance()  'Added by Saylee on 6th-Oct-2009
                Session("mAssemblyStatus") = mAssemblyStatus
                'mMachineMaintenance.RegNo 
                'AssemblyTypeName: mAssemblyStatus.Assembly.AssemblyTypeName 
                'mAssemblyStatus.Assembly.ModelName + mAssemblyStatus.Assembly.SerialNo 

                'Added by Vikrant
                mAssemblyDetail = "Reg No. : " + cmbMachineList.SelectedItem.Text + " Model : " + cmbModelList.SelectedItem.Text + " Serial No. : " + txtSerialNo.Text & " Installed On :" & txtInstalledOnDate.Text
                MarkLog(Util.Action.Save, "AssemblyInstallation", mAssemblyDetail, Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                mAssemblyStatus = clnAssemblyStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    Dim tmpRemovedAssemblyStatusList As tmpRemovedAssemblyList = Session("mRemovedAssemblyStatusList")
                    'Dim mtmpAssemblyStatus As AssemblyStatus = AssemblyStatus

                    If AppSettings("InstallExistingAssemblyWithNewValue") = "True" And tmpRemovedAssemblyStatusList.Contains(mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.Assembly.SerialNo) = True Then
                        MSGBoxCtrl.Show("Alert!!", "This Serial No. is already maintained in the system.", "Do you want to replace it?", MsgBoxStyle.YesNo, "InstallExistingAssemblyWithNewValue")
                    Else
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If

                ElseIf InStr(ex.Message, "FKtabAssemblyStatustabAssembly", CompareMethod.Text) Or InStr(ex.Message, "Installation of Assembly is not possible as you can not change No. of assemblies of this type on this aircraft", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, "Installation of Assembly is not possible as you can not change No. of assemblies of this type on this aircraft", MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                clnAssemblyStatus = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetCaptions()
        If IsDate(mAssemblyStatus.InstalledOn) Then
            'Code Commented and newly Added on 28-05-2007 by Kalpesh Shah -------- 
            ''calInstalledOn.SelectedDate = CDate(mAssemblyStatus.InstalledOn)
            ''calInstalledOn.TitleText = CDate(mAssemblyStatus.InstalledOn)
            ''calInstalledOn.DateToday = CDate(mAssemblyStatus.InstalledOn)
            ''calInstalledOn.Text = CDate(mAssemblyStatus.InstalledOn).ToShortDateString
            '---------------------------------------------------------------------
            'Else
            '    'Code Commented and newly Added on 28-05-2007 by Kalpesh Shah -------- 
            '    ''calInstalledOn.SelectedDate = Today.Date
            '    ''calInstalledOn.TitleText = Today.Date.ToShortDateString
            '    ''calInstalledOn.DateToday = Today.Date
            '    calInstalledOn.Text = Today.Date.ToShortDateString
            '---------------------------------------------------------------------
        End If
        lblInstallationInfo.InnerText = "Installation Information of the " & mAssemblyStatus.AssemblyTypeName
        If Not mAssemblyStatus.IsNew Then
            lblTitle.Text = "Install information of the " & mAssemblyStatus.AssemblyTypeName & "[Model:" & mAssemblyStatus.ModelName & " Serial No. :" & mAssemblyStatus.Assembly.SerialNo & "]"
        Else
            lblTitle.Text = "Install information of the " & mAssemblyStatus.AssemblyTypeName & "[New]"
        End If
        REM: Set the Grid header aas per Assembly Name

    End Sub
    'Commented and added by Saylee on 14-Mar-2013 for ALL14032013-1
    ''Private Sub SetPeriods()
    ''    mSelectPeriods = SelectPeriods.NewSelectPeriods()
    ''    mPeriodList = PeriodList.GetPeriodList(PeriodList.SelectType.WithoutSelect)
    ''    Dim i As Integer
    ''    REM:-Display only those periods which are not existed in AssemblyStatusPeriods datagrid 
    ''    While i <= mPeriodList.Count - 1
    ''        If mAssemblyStatus.AssemblyStatusPeriods.Contains(mPeriodList(i).ID) = False Then
    ''            mSelectPeriods.Add(mPeriodList(i).ID, mPeriodList(i).PeriodName)
    ''        End If
    ''        i = i + 1
    ''    End While
    ''    Session("mSelectPeriods") = mSelectPeriods
    ''End Sub

    'Added by Saylee on 14-Mar-2013 for ALL14032013-1
    Private Sub SetPeriods()
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        Dim i As Integer
        Dim mPeriodList As PeriodList
        mPeriodList = PeriodList.GetPeriodList
        If mAssemblyStatus.AssemblyTypeID = 1 Or mAssemblyStatus.AssemblyTypeID = 2 Or mAssemblyStatus.AssemblyTypeID = 4 Then
            While i <= mPeriodList.Count - 1
                If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mPeriodList(i).ID) Then
                    mSelectPeriods.Add(mPeriodList(i).ID, mPeriodList(i).PeriodName)
                End If
                i = i + 1
            End While
            Session("mSelectPeriods") = mSelectPeriods
        Else
            While i <= mMachine.AssemblyStatus.AssemblyStatusPeriods.Count - 1
                If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mMachine.AssemblyStatus.AssemblyStatusPeriods(i).PeriodID) Then
                    mSelectPeriods.Add(mMachine.AssemblyStatus.AssemblyStatusPeriods(i).PeriodID, mMachine.AssemblyStatus.AssemblyStatusPeriods(i).PeriodName)
                End If
                i = i + 1
            End While
            Session("mSelectPeriods") = mSelectPeriods
        End If
    End Sub

    Private Sub AddSelectedPeriods()
        Dim mSelectPeriod As SelectPeriod
        If IsNothing(mSelectPeriods) Then
            mSelectPeriods = SelectPeriods.NewSelectPeriods
        End If
        'this is to add the selected periods from the SelectPeriod page
        For Each mSelectPeriod In mSelectPeriods
            If mSelectPeriod.IsSelected = True Then
                mAssemblyStatus.AssemblyStatusPeriods.Add(AssemblyStatusPeriod.NewChildAssemblyStatusPeriod(mAssemblyStatus.ID, mAssemblyStatus.MachineID, CType(mAssemblyStatus.InstalledOn, Object), mAssemblyStatus.Assembly.Model.AssemblyTypeID, mSelectPeriod.PeriodID, False, mAssemblyStatus.InstalledOn.ToString))
            End If
        Next
        Session("mAssemblyStatus") = mAssemblyStatus
        Session.Remove("mSelectPeriods")
        mSelectPeriods = Nothing
    End Sub
    Private Sub CopyFromClone(ByVal clnAssemblyStatus As AssemblyStatus, ByVal IsNewInstallation As Boolean)
        mAssemblyStatus.Assembly.ModelID = clnAssemblyStatus.Assembly.ModelID
        mAssemblyStatus.Assembly.SerialNo = clnAssemblyStatus.Assembly.SerialNo
        mAssemblyStatus.Position = clnAssemblyStatus.Position
        mAssemblyStatus.InstallationWONo = clnAssemblyStatus.InstallationWONo
        mAssemblyStatus.InstallationRemark = clnAssemblyStatus.InstallationRemark
        mAssemblyStatus.InstDoneByID = clnAssemblyStatus.InstDoneByID
        mAssemblyStatus.InstLicenseNo = clnAssemblyStatus.InstLicenseNo
        mAssemblyStatus.InstPlace = clnAssemblyStatus.InstPlace
        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyStatus.MaintenanceDoneByEmployees
            If IsNewInstallation Then 'New Record
                mAssemblyStatus.MaintenanceDoneByEmployees.Add(mAssemblyStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            ElseIf Not IsNewInstallation Then 'Edit Record
                If Not mAssemblyStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mAssemblyStatus.MaintenanceDoneByEmployees.Add(mAssemblyStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                End If
            End If
        Next
        'End
    End Sub
    Private Sub NewRecord()
        If Not IsNothing(mRemovedAssemblyStatus) Then
            mRemovedAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mRemovedAssemblyStatus.ID)
            mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbMachineList.SelectedValue), txtInstalledOnDate.Text, mRemovedAssemblyStatus.AssemblyTypeID, True, mRemovedAssemblyStatus.ID.ToString)
        Else
            REM:-If new assembly is installed
            mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbMachineList.SelectedValue), txtInstalledOnDate.Text, mAssemblyStatus.AssemblyTypeID, False)
        End If
    End Sub
    Private Sub EditRecord()
        mAssemblyStatus = AssemblyStatus.GetInstallAssemblyStatus(mAssemblyStatus.ID, txtInstalledOnDate.Text)
    End Sub
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 6th-Oct-2009
        If mFromType = From.NewInstall And Not (mMachineMaintenanceList.Contains(mAssemblyStatus.ID, 1, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mMachine.ID, 1, txtInstalledOnDate.Text, mAssemblyStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else  ''If mFromType = From.EditInstall Then
            mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyStatus.ID, 1)
            Session("mMachineMaintenance") = mMachineMaintenance
        End If

        With mMachineMaintenance
            .MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID = 1
            .MaintenanceID = mAssemblyStatus.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtInstalledOnDate.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(txtInstalledOnDate.Text, mAssemblyStatus.MachineID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
            End If
        End With

        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub SaveMachineMaintenance()
        'Added by Saylee on 6th-Oct-2009
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
    Private Sub GetAssemblyStatusForModel(ByVal PartIndex As Integer) 'Added by Saylee on 25-Aug-2009

        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        Dim mtmpAssemblyListOnModelSelection As tmpAssemblyListOnModelSelection = tmpAssemblyListOnModelSelection.GetAssemblyListOnModelSelection(mAssemblyStatus.AssemblyTypeName, Guid.Empty.ToString, mModelList(New Guid(cmbModelList.SelectedValue)).ModelName)
        If mtmpAssemblyListOnModelSelection.Count > 0 Then
            'Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetInstallAssemblyStatus(mtmpAssemblyListOnModelSelection(0).ID, mtmpAssemblyListOnModelSelection(0).InstalledOn.ToString)
            Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mtmpAssemblyListOnModelSelection(0).ID)
            mAssemblyStatus.ATAID = tmpAssemblyStatus.ATAID
            mAssemblyStatus.Assembly.Model.ManufacturerID = tmpAssemblyStatus.Assembly.Model.ManufacturerID
            mAssemblyStatus.Assembly.ModelID = tmpAssemblyStatus.Assembly.ModelID

            If mAssemblyStatus.AssemblyStatusPeriods.Count > 0 Then
                For i As Integer = mAssemblyStatus.AssemblyStatusPeriods.Count - 1 To 0 Step -1
                    ''If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 Then
                    ''    mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                    ''End If
                    '-----------ClientCode Checked By Vikrant on 19 Dec 2011 for Buddha Air-------------------
                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 3 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    Else
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    End If
                    '--------------------------------------------------------------------------------------------
                Next
                dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
                dgInstallationValue.DataBind()
                upnlInstallationValues.Update()
            End If

            Dim tmpAssemblyStatusPeriod As AssemblyStatusPeriod
            For Each tmpAssemblyStatusPeriod In tmpAssemblyStatus.AssemblyStatusPeriods
                If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(tmpAssemblyStatusPeriod.PeriodID) Then
                    'mAssemblyStatus.AsOnDate.ToString()

                    'Added by Saylee on 19-Mar-2013 for ALL14032013-1
                    'mAssemblyStatus.AssemblyStatusPeriods.Add(AssemblyStatusPeriod.NewChildAssemblyStatusPeriod(mAssemblyStatus.ID, mAssemblyStatus.MachineID, CType(mAssemblyStatus.InstalledOn, Object), mAssemblyStatus.AssemblyTypeID, tmpAssemblyStatusPeriod.PeriodID, , mtmpAssemblyListOnModelSelection(0).InstalledOn.ToString))
                    mAssemblyStatus.AssemblyStatusPeriods.Add(AssemblyStatusPeriod.NewChildAssemblyStatusPeriod(mAssemblyStatus.ID, mAssemblyStatus.MachineID, CType(mAssemblyStatus.InstalledOn, Object), mAssemblyStatus.AssemblyTypeID, tmpAssemblyStatusPeriod.PeriodID, , mAssemblyStatus.InstalledOn.ToString))
                    '***********************

                    ''mAssemblyStatus.AssemblyStatusPeriods.Item(tmpAssemblyStatusPeriod.PeriodID, "").CompCurrentValueFormatted = ""
                    mAssemblyStatus.AssemblyStatusPeriods.Item(tmpAssemblyStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted = ""
                    ''mAssemblyStatus.AssemblyStatusPeriods.Item(tmpAssemblyStatusPeriod.PeriodID, "").MachineInstallationValueFormatted = "" ''Commented by Saylee on 19-Dec-2018, as MachineInstall values was getting blank when model selected,even if period was present in its Airframe 
                End If
            Next
            Session("mAssemblyStatus") = mAssemblyStatus
            dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
            dgInstallationValue.DataBind()
            tmpAssemblyStatus = Nothing
        Else
            If mAssemblyStatus.AssemblyStatusPeriods.Count > 0 Then
                For i As Integer = mAssemblyStatus.AssemblyStatusPeriods.Count - 1 To 0 Step -1
                    'If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 Then
                    '    mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                    'End If
                    '-----------ClientCode Checked By Vikrant on 19 Dec 2011 for Buddha Air-------------------
                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 3 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    Else
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    End If
                    '--------------------------------------------------------------------------------------------
                Next
                dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
                dgInstallationValue.DataBind()
            End If
        End If
        upnlInstallationValues.Update()
    End Sub
#Region "Component List Tab"
    Private Sub EditRecordComponentList(ByVal Index As Int32)
        Dim mId As Guid = mtmpInstalledCompList(Index).CompStatusID
        If mtmpInstalledCompList(Index).IsMaster Then
            'If mAssemblyStatus.Sort > 1 Then 'At present messagebox removed by Saylee and allowed to edit comp as master record can be edited even if old assembly 

            '    MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit component.This is master record and can not be edited from here", MsgBoxStyle.OkOnly, "")
            '    Exit Sub
            'Else
            mCompStatus = CompStatus.GetCompStatus(mtmpInstalledCompList(Index).CompStatusID, mAssemblyStatus.ID, mAssemblyStatus.InstalledOn.ToString)
            Session("mCompStatus") = mCompStatus
            ''Changed By Utkarsh ON 24-Apr-2012 For ALL23042012 (For Buddha Air)
            'If (AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            '    Response.Redirect("wfCompStatusBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
            'Else
            '    Response.Redirect("wfCompStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
            'End If
            Session("IsOpenedFromAssembly") = "True"
            Response.Redirect("wfCompStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
            ' End If
        Else
            mCompStatus = CompStatus.GetInstallCompStatusFromEntry(mtmpInstalledCompList(Index).CompStatusID, mAssemblyStatus.ID, mtmpInstalledCompList(Index).InstalledOnDBValue.ToString)
            Session("mCompStatus") = mCompStatus
            Session("From") = 2 'EditInstall
            Session("mRemovedCompStatus") = Nothing
            'Added by Vikrant on 1-Aug-2011
            mAircraft = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).MachineInfo
            mAssemblyType = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).AssemblyType
            mAssemblyInfo = mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).AssemblyInfo
            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
            MarkLog(Util.Action.Edit, "Assembly Installation", mDetail, Util.ErrorType.NoError, mtmpInstalledCompList.Item(mtmpInstalledCompList.CurrentIndex).CompStatusID, EventLogID)



            'Changed By Utkarsh ON 24-Apr-2012 For ALL23042012 (For Buddha Air)
            'If (AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            '    Response.Redirect("wfInstallComp_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
            'Else
            '    Response.Redirect("wfInstallComp_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
            'End If
            Session("IsOpenedFromAssembly") = "True"
            Response.Redirect("wfInstallComp_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
            'End
        End If
    End Sub
    Private Sub DeleteRecordComponentList(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteCompList")
        mtmpInstalledCompList.CurrentIndex = Index
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
    End Sub
    Private Sub RemoveSessionComponentList()
        mtmpInstalledCompList = Nothing
        Session.Remove("mtmpInstalledCompList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub NewRecordComponentList()
        mCompStatus = CompStatus.NewCompStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.AsOnDate.ToString, mMachine.HourType)
        Session("mCompStatus") = mCompStatus
        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        ''Changed By Utkarsh ON 24-Apr-2012 For ALL23042012 (For Buddha Air)
        'If (AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
        '    Response.Redirect("wfCompStatusBA.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        'Else
        '    Response.Redirect("wfCompStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        'End If
        ''End
        Session("IsOpenedFromAssembly") = "True"
        Response.Redirect("wfCompStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
    End Sub
    Private Sub FindNowComponentList()
        'Binding CompStatus Grid
        Select Case cmbLookInComponentList.SelectedIndex
            Case 0
                REM ALL
                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", "", mAssemblyStatus.AssemblyID)
            Case 1
                REM ATACode
                If txtCodeComponentList.Text = "" Then
                    mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", "", mAssemblyStatus.AssemblyID)
                Else
                    mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", "", mAssemblyStatus.AssemblyID, Val(txtCodeComponentList.Text))
                End If
            Case 2
                REM ATA nomenclature
                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", "", mAssemblyStatus.AssemblyID, , txtForComponentList.Text.Trim)
            Case 3
                REM Part Name
                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, txtForComponentList.Text.Trim, "", mAssemblyStatus.AssemblyID)
            Case 4
                REM Part Desription
                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", "", mAssemblyStatus.AssemblyID, , , txtForComponentList.Text.Trim)
            Case 5
                REM Part Serial No
                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", txtForComponentList.Text.Trim, mAssemblyStatus.AssemblyID)
            Case Else
                REM ALL
                mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", "", mAssemblyStatus.AssemblyID)
        End Select
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        dgCompStatusList.DataSource = mtmpInstalledCompList
        dgCompStatusList.DataBind()
    End Sub
    Private Sub SetPageComponentList()
        lblComponentsCaption.Text = "List of all the Components on the " & mAssemblyStatus.AssemblyTypeName & " as of " & mAssemblyStatus.InstalledOnFormatted & ". The Time Since New values of all the Components will be as of " & mAssemblyStatus.InstalledOnFormatted
    End Sub
    Private Sub ControlVisibilityComponentList()
        btnAddTopComponentList.Visible = mtmpInstalledCompList.Count > 25
        'btnBackTopDirective.Visible = mComplyAssemblyMonitorModStatusList.Count > 25
        btnPrintTopComponentList.Visible = mtmpInstalledCompList.Count > 25
        btnPrintComponentList.Enabled = mtmpInstalledCompList.Count > 0
        btnPrintTopDirective.Enabled = mtmpInstalledCompList.Count > 0
    End Sub
    Private Sub DataFieldBindComponentList()
        mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(CDate(mAssemblyStatus.InstalledOn).ToShortDateString, mAssemblyStatus.MachineID.ToString, "", "", mAssemblyStatus.AssemblyID)
        dgCompStatusList.DataSource = mtmpInstalledCompList
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        upnlComponentList.DataBind()
        'btnPrintComponentList.Enabled = mCompStatusList.Count > 0   'Code Shifted from ControlVisiblity()
        'btnPrintTopComponentList.Enabled = mCompStatusList.Count > 0
        lblResultComponentList.Text = "List of Component: " & mtmpInstalledCompList.Count & " Record(s)."  'Code Shifted from SetPage()
    End Sub
    Private Sub DisplayControlsComponentList(ByVal Index As Integer)
        txtForComponentList.Text = ""
        txtCodeComponentList.Text = ""
        txtCodeComponentList.Visible = IIf(Index = 1, True, False)
        txtForComponentList.Visible = IIf(Index = 2 Or Index = 3 Or Index = 4 Or Index = 5, True, False)
        lblForComponentList.Visible = (Index > 0 And Index < 6)
        If cmbLookInComponentList.Enabled = True Then
            cmbLookInComponentList.Focus()
        End If
    End Sub
#End Region

#Region "Service Tab"
    Private Sub DataFieldBindService()
        mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("(All)")
        cmbSearchForService.DataSource = mModelMonitorServiceTypeList
        Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList

        mComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), ShowNotApplicable:=chkApplicableService.Checked)
        dgMonitorServiceStatusList.DataSource = mComplyAssemblyMonitorServiceStatusList
        Session("mComplyAssemblyMonitorServiceStatusList") = mComplyAssemblyMonitorServiceStatusList

        cmbSearchForService.DataBind()
        dgMonitorServiceStatusList.DataBind()
        chkApplicableService.Checked = False
    End Sub
    Private Sub SetPageService()

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "Maintenance Event"
        Else
            ServiceMPDTitle = "Service"
        End If

        lblServiceText.Text = "List of all the  " + ServiceMPDTitle + "(s) on the " & mAssemblyStatus.AssemblyTypeName & " as of " & New SmartDate(mAssemblyStatus.InstalledOn.ToString).FormattedText & ". All the values will be as of " & New SmartDate(mAssemblyStatus.InstalledOn.ToString).FormattedText
        If Not IsNothing(mComplyAssemblyMonitorServiceStatusList) Then
            lblResultService.Text = "List of Assembly " + ServiceMPDTitle + " Status: " & mComplyAssemblyMonitorServiceStatusList.Count & " Record(s)"
        End If
    End Sub
    Private Sub SetGridService()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorServiceStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(25).Text, Boolean)
            If B = False Then
                dgMonitorServiceStatusList.Rows(j).Cells(24).Enabled = False
            End If
        Next
    End Sub
    Private Sub ControlVisibilityService()
        btnAddTopService.Visible = mComplyAssemblyMonitorServiceStatusList.Count > 25
        'btnBackTopService.Visible = mComplyAssemblyMonitorServiceStatusList.Count > 25
        btnPrintTopService.Visible = mComplyAssemblyMonitorServiceStatusList.Count > 25
        btnPrintService.Enabled = mComplyAssemblyMonitorServiceStatusList.Count > 0
        btnPrintTopService.Enabled = mComplyAssemblyMonitorServiceStatusList.Count > 0
        dgMonitorServiceStatusList.Columns(19).Visible = IIf(chkApplicableService.Checked, False, True)
    End Sub
    Private Sub FindNowService()
        Session("LookInCombo") = cmbLookInService.SelectedValue
        Session("TextFor") = txtForService.Text
        Session("TextCode") = txtCodeService.Text
        Session("SearchForCombo") = cmbSearchForService.SelectedValue

        dgMonitorServiceStatusList.PageIndex = 0
        Select Case cmbLookInService.SelectedIndex
            Case 0, -1  'All
                mComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), ShowNotApplicable:=chkApplicableService.Checked)
            Case 1  'ATA Code
                mComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), Val(txtCodeService.Text), ShowNotApplicable:=chkApplicableService.Checked)
            Case 2  'Description
                mComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , txtForService.Text.Trim, ShowNotApplicable:=chkApplicableService.Checked)
            Case 3  'Service Type ID
                mComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , CInt(cmbSearchForService.SelectedValue), ShowNotApplicable:=chkApplicableService.Checked)
            Case 4 ' Work Order No.
                mComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , , txtForService.Text.Trim, ShowNotApplicable:=chkApplicableService.Checked)
            Case 5  'Show In C of A
                mComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , , , True, ShowNotApplicable:=chkApplicableService.Checked)
        End Select
        Session("mComplyAssemblyMonitorServiceStatusList") = mComplyAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mComplyAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
    End Sub
    Private Sub SetControlService()
        'Function added by Saylee on 11th-Jan-2008 to keep Searching criteia as it is
        cmbLookInService.SelectedValue = LookInCombo 'IIf(LookIn = "", "(All)", LookIn)
        txtForService.Text = TextFor
        txtCodeService.Text = TextCode
        cmbSearchForService.SelectedValue = SearchForCombo 'IIf(SearchFor = "", "(All)", SearchFor)
        DisplayControlsService(cmbLookInService.SelectedIndex)
        FindNowService()
    End Sub
    Private Sub DisplayControlsService(ByVal Index As Integer)
        txtForService.Text = IIf(Index = 2 Or Index = 4, txtForService.Text, "")
        txtCodeService.Text = IIf(Index = 1, txtCodeService.Text, "")
        '=========================================================
        txtCodeService.Visible = IIf(Index = 1, True, False)
        txtForService.Visible = IIf(Index = 2 Or Index = 4, True, False)
        lblForService.Visible = (Index > 0 And Index <> 5)
        cmbSearchForService.Visible = (Index = 3)
        'New addition By Yogita on 9-Jan-2008
        If cmbLookInService.Enabled = True Then
            cmbLookInService.Focus()
        End If
    End Sub
    Private Sub EditMasterRecordService(ByVal mMasterId As Guid, ByVal mId As Guid, ByVal Index As Integer)

        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim objAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mAssemblyMonitorServiceStatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo = mComplyAssemblyMonitorServiceStatusList(Index)
        objAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mComplyAssemblyMonitorServiceStatusList(Index).AssemblyMonitorServiceStatusID, mComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID, mMachine.HourType)
        Dim mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
        mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        '----------------------------------

        If mAssemblyMonitorServiceStatusInfo.IsMaster Then
            Session("mAssemblyMonitorServiceStatus") = objAssemblyMonitorServiceStatus
        Else
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(objAssemblyMonitorServiceStatus.ID, objAssemblyMonitorServiceStatus.AssemblyStatusID, objAssemblyMonitorServiceStatus.DoneOnFormatted.ToString, mMachine.HourType)
            Session("mPrevAssemblyMonitorServiceStatus") = objAssemblyMonitorServiceStatus
            Session("From") = 1 'Edit record
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        End If

        ''***
        Dim mModelMonitorService As ModelMonitorService
        mModelMonitorService = ModelMonitorService.GetModelMonitorService(mMasterId, mMachine.HourType)
        ''Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        ''mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mId, mAssemblyStatus.ID, mMachine.HourType)

        Session("mMachine") = mMachine
        Session("mModelMonitorService") = mModelMonitorService
        'RemoveSession()
        'Added by Vikrant on 1-Aug-2011
        mAircraft = mComplyAssemblyMonitorServiceStatusList(mId).MachineInfo
        mAssemblyType = mComplyAssemblyMonitorServiceStatusList(mId).AssemblyType
        mAssemblyInfo = mComplyAssemblyMonitorServiceStatusList(mId).AssemblyInfo
        mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
        MarkLog(Util.Action.Edit, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorServiceStatusList(mId).AssemblyMonitorServiceStatusID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelServiceMasterWindow", "OpenModelServiceMasterWindow()", True)
        'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallAssembly_Ajax.aspx")
    End Sub
    Private Sub RemoveSessionService()
        mComplyAssemblyMonitorServiceStatusList = Nothing
        Session.Remove("From")
        Session.Remove("mPrevAssemblyMonitorServiceStatus")
        Session.Remove("mComplyAssemblyMonitorServiceStatusList")
        'Session.Remove("mFileAttach") 'Added By Vikrant On 25-Nov-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub NewRecordService()
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'MarkLog(Util.Action.[New], "InstallAssemblyMonitorServiceStatusList", massemblyinfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
        'Code added By Deven on 1/4/2008
        'Response.Redirect("wfAssemblyMonitorServiceStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssemblyMonitorServiceStatusList.aspx")

        'Code added By Deven on 25/09/2009
        Dim mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
        mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        '----------------------------------
        mModelMaintenanceActivityListCount = ModelMaintenanceActivityListCount.GetModelMaintenanceActivityListCount(mAssemblyStatus.Assembly.ModelID)
        If mModelMaintenanceActivityListCount.ModelServiceListCount > 0 Then
            Response.Redirect("wfModelMonitorServiceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx" & "&GChildPage3=wfInstallAssembly_Ajax.aspx")
        Else
            Dim mModelMonitorService As ModelMonitorService

            Dim ID As Guid = Guid.NewGuid
            mModelMonitorService = ModelMonitorService.NewModelMonitorService(ID:=ID,
                                                                              ModelID:=mAssemblyStatus.Assembly.ModelID,
                                                                              HourType:=mMachine.HourType,
                                                                              PreviousRefID:=ID)
            Session("mModelMonitorService") = mModelMonitorService
            mModelMonitorService.BeginEdit()
            MarkLog(Util.Action.[New], "Model Service", " Model : " & mAssemblyStatus.Assembly.ModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorServiceList_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelServiceMasterWindow", "OpenModelServiceMasterWindow()", True)
        End If
    End Sub
    Private Sub EditRecordService(ByVal Index As Integer)
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim objAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mAssemblyMonitorServiceStatusInfo As tmpComplyAssemblyMonitorServiceStatusList.tmpComplyAssemblyMonitorServiceStatusInfo = mComplyAssemblyMonitorServiceStatusList(Index)
        objAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mComplyAssemblyMonitorServiceStatusList(Index).AssemblyMonitorServiceStatusID, mComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID, mMachine.HourType)
        'Code added By Deven on 25/09/2009
        Dim mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
        mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        '----------------------------------

        If mAssemblyMonitorServiceStatusInfo.IsMaster Then
            Session("mAssemblyMonitorServiceStatus") = objAssemblyMonitorServiceStatus
            Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        Else
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(objAssemblyMonitorServiceStatus.ID, objAssemblyMonitorServiceStatus.AssemblyStatusID, objAssemblyMonitorServiceStatus.DoneOnFormatted.ToString, mMachine.HourType, True)
            Session("mPrevAssemblyMonitorServiceStatus") = objAssemblyMonitorServiceStatus
            Session("From") = 1 'Edit record
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            'Added by Vikrant on 1-Aug-2011
            mAircraft = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).MachineInfo
            mAssemblyType = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyType
            mAssemblyInfo = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyInfo
            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
            MarkLog(Util.Action.Edit, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID, EventLogID)

            'Added by Saylee on 17-Jun-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(objAssemblyMonitorServiceStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            If mAssemblyMonitorServiceStatus.IsAttachmentAdded Then
                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            Response.Redirect("wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        End If
    End Sub
    Private Sub DeleteRecordService(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteService")
        mComplyAssemblyMonitorServiceStatusList.CurrentIndex = Index
        Session("mComplyAssemblyMonitorServiceStatusList") = mComplyAssemblyMonitorServiceStatusList
    End Sub
    Private Sub ComplyRecordService(ByVal Index As Integer)
        Dim objAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mComplyAssemblyMonitorServiceStatusList(Index).AssemblyMonitorServiceStatusID, mComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID, mMachine.HourType)
        If objAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And objAssemblyMonitorServiceStatus.IsCompleted = True Then
            MSGBoxCtrl.Show("Compliance Alert!", "", "One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus

        'Added by Saylee on 17-Jun-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(objAssemblyMonitorServiceStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************

        Session("mPrevAssemblyMonitorServiceStatus") = objAssemblyMonitorServiceStatus
        Session("From") = 0 'New record
        'Commented and changed by Saylee on 28-Oct-2009 Instead of AsOnDate,InstalledOn Date is passed as CurrentDate
        ''mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, objAssemblyMonitorServiceStatus.AssemblyID, objAssemblyMonitorServiceStatus.AssemblyStatusID, objAssemblyMonitorServiceStatus.AsOnDate.ToString, objAssemblyMonitorServiceStatus.ModelMonitorService.ModelID, objAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, objAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, objAssemblyMonitorServiceStatus.AssemblyID, objAssemblyMonitorServiceStatus.AssemblyStatusID, mAssemblyStatus.InstalledOn.ToString, objAssemblyMonitorServiceStatus.ModelMonitorService.ModelID, objAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, objAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        'Added by Vikrant on 1-Aug-2011
        mAircraft = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).MachineInfo
        mAssemblyType = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyType
        mAssemblyInfo = mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyInfo
        mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
        MarkLog(Util.Action.Comply, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorServiceStatusList.Item(mComplyAssemblyMonitorServiceStatusList.CurrentIndex).AssemblyMonitorServiceStatusID, EventLogID)

        'Added By Vikrant On 25-Nov-2014
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID)
        Session("mFileAttach") = mFileAttach
        'End

        Response.Redirect("wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
    End Sub
#End Region

#Region "Inspection Tab"
    Private Sub SetGridInspection()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorInspStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorInspStatusList.Rows(j).Cells(24).Text, Boolean)
            If B = False Then
                dgMonitorInspStatusList.Rows(j).Cells(23).Enabled = False
            End If
        Next
    End Sub
    Private Sub FindNowInspection()
        dgMonitorInspStatusList.PageIndex = 0
        Session("LookInCombo") = cmbLookInInspection.SelectedValue
        Session("TextFor") = txtForInspection.Text
        Session("TextCode") = txtCodeInspection.Text
        Session("SearchForCombo") = cmbSearchForInspection.SelectedValue
        Select Case cmbLookInInspection.SelectedIndex
            Case 0, -1  'All
                mComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 1  'ATA Code
                mComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), Val(txtCodeInspection.Text), ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 2  'Description
                mComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , txtForInspection.Text.Trim, ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 3  'Insp Type ID
                mComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , CInt(cmbSearchForInspection.SelectedValue), ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 4 ' Work Order No.
                mComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , , txtForInspection.Text.Trim, ShowNotApplicable:=chkApplicableInspection.Checked)
            Case 5  'Show In C of A
                mComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , , , True, ShowNotApplicable:=chkApplicableInspection.Checked)
        End Select
        Session("mComplyAssemblyMonitorInspStatusList") = mComplyAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mComplyAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
    End Sub
    Private Sub SetPageInspection()
        lblInspectionsCaption.Text = "List of all the Inspection on the " & mAssemblyStatus.AssemblyTypeName & " as of " & mAssemblyStatus.InstalledOnFormatted & ". All the values will be as of " & mAssemblyStatus.InstalledOnFormatted
        lblResultInspection.Text = "List of Inspection: " & mComplyAssemblyMonitorInspStatusList.Count & " Record(s)."
    End Sub
    Private Sub ControlVisibilityInspection()
        btnAddTopInspection.Visible = mComplyAssemblyMonitorInspStatusList.Count > 25
        'btnBackTopInspection.Visible = mComplyAssemblyMonitorInspStatusList.Count > 25
        btnPrintTopInspection.Visible = mComplyAssemblyMonitorInspStatusList.Count > 25
        btnPrintInspection.Enabled = mComplyAssemblyMonitorInspStatusList.Count > 0
        btnPrintTopInspection.Enabled = mComplyAssemblyMonitorInspStatusList.Count > 0
        dgMonitorInspStatusList.Columns(19).Visible = IIf(chkApplicableInspection.Checked, False, True)
    End Sub
    Private Sub DisplayControlsInspection(ByVal Index As Integer)
        txtForInspection.Text = IIf(Index = 2 Or Index = 4, txtForInspection.Text, "")
        txtCodeInspection.Text = IIf(Index = 1, txtCodeInspection.Text, "")
        '=========================================================
        txtCodeInspection.Visible = IIf(Index = 1, True, False)
        txtForInspection.Visible = IIf(Index = 2 Or Index = 4, True, False)
        lblForInspection.Visible = (Index > 0 And Index <> 5)
        cmbSearchForInspection.Visible = (Index = 3)
        'New Addition By Yogita on 9-Jan-2008 
        If cmbLookInInspection.Enabled = True Then
            cmbLookInInspection.Focus()
        End If
    End Sub
    Private Sub NewRecordInspection()
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        'Changed by Vikrant on 1-Aug-2011
        'MarkLog(Util.Action.[New], "Assembly Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'Code added By Deven on 1/4/2008
        'Response.Redirect("wfAssemblyMonitorInspStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssemblyMonitorInspStatusList.aspx")

        'Code added By Deven on 25/09/2009
        Dim mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
        mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        '----------------------------------
        mModelMaintenanceActivityListCount = ModelMaintenanceActivityListCount.GetModelMaintenanceActivityListCount(mAssemblyStatus.Assembly.ModelID)
        If mModelMaintenanceActivityListCount.ModelInspListCount > 0 Then
            Response.Redirect("wfModelMonitorInspList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx" & "&GChildPage3=wfInstallAssembly_Ajax.aspx")
        Else
            Dim mModelMonitorInsp As ModelMonitorInsp
            ''mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(Guid.NewGuid, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(ID, mAssemblyStatus.Assembly.ModelID, mMachine.HourType, ID) 'For new records ID,PrevRefID are same
            Session("mModelMonitorInsp") = mModelMonitorInsp
            MarkLog(Util.Action.[New], "Model Inspection", " Model : " & mAssemblyStatus.Assembly.ModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorServiceList_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow()", True)
        End If


        '------------------------------------------------
    End Sub
    Private Sub EditMasterRecordInspection(ByVal mMasterId As Guid, ByVal mId As Guid, ByVal index As Integer)
        Dim mModelMonitorInsp As ModelMonitorInsp
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim objAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim AssemblyMonitorInspStatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo = mComplyAssemblyMonitorInspStatusList(index)
        objAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mComplyAssemblyMonitorInspStatusList(index).AssemblyMonitorInspStatusID, mComplyAssemblyMonitorInspStatusList(index).AssemblyStatusID, mMachine.HourType)

        Dim mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
        mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        '----------------------------------
        If AssemblyMonitorInspStatusInfo.IsMaster Then
            Session("mAssemblyMonitorInspStatus") = objAssemblyMonitorInspStatus
        Else
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(objAssemblyMonitorInspStatus.ID, objAssemblyMonitorInspStatus.AssemblyStatusID, objAssemblyMonitorInspStatus.DoneOnFormatted.ToString, mMachine.HourType)
            Session("mPrevAssemblyMonitorInspStatus") = objAssemblyMonitorInspStatus
            Session("From") = 1 'Edit record
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        End If

        '*****
        mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mMasterId, mMachine.HourType)
        ' Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mMachine") = mMachine
        Session("mModelMonitorInsp") = mModelMonitorInsp
        'Added by Vikrant on 1-Aug-2011
        mAircraft = mComplyAssemblyMonitorInspStatusList(mId).MachineInfo
        mAssemblyType = mComplyAssemblyMonitorInspStatusList(mId).AssemblyType
        mAssemblyInfo = mComplyAssemblyMonitorInspStatusList(mId).AssemblyInfo
        mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
        MarkLog(Util.Action.Edit, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorInspStatusList(mId).AssemblyMonitorInspStatusID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow()", True)
        'Response.Redirect("wfModelMonitorInspection_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallAssembly_Ajax.aspx")
    End Sub
    Private Sub DataFieldBindInspection()
        mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(All)")
        Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
        cmbSearchForInspection.DataSource = mModelMonitorInspTypeList

        mComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(CDate(mAssemblyStatus.InstalledOn).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), ShowNotApplicable:=chkApplicableInspection.Checked)
        dgMonitorInspStatusList.DataSource = mComplyAssemblyMonitorInspStatusList
        Session("mComplyAssemblyMonitorInspStatusList") = mComplyAssemblyMonitorInspStatusList

        cmbSearchForInspection.DataBind()
        dgMonitorInspStatusList.DataBind()
        chkApplicableInspection.Checked = False
    End Sub
    Private Sub SetControlInspection()
        'Fuction added by Saylee on 11th-Jan-2008 to keep Searching criteia as it is
        cmbLookInInspection.SelectedValue = LookInCombo 'IIf(LookIn = "", "(All)", LookIn)
        txtForInspection.Text = TextFor
        txtCodeInspection.Text = TextCode
        cmbSearchForInspection.SelectedValue = SearchForCombo 'IIf(SearchFor = "", "(All)", SearchFor)
        DisplayControlsInspection(cmbLookInInspection.SelectedIndex)
        FindNowInspection()
    End Sub
    Private Sub ComplyRecordInspection(ByVal Index As Integer)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim objAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        If objAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And objAssemblyMonitorInspStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.MonitorExist, MSGBox.Message_text.MonitorExist, "One time monitoring already done. Can not be complied again.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        '        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, objAssemblyMonitorInspStatus.AssemblyID, objAssemblyMonitorInspStatus.AssemblyStatusID, objAssemblyMonitorInspStatus.AsOnDate.ToString, objAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, objAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, objAssemblyMonitorInspStatus.DoneOn)

        'Added by Saylee on 17-Jun-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(objAssemblyMonitorInspStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************

        'Commented and changed by Saylee on 28-Oct-2009 Instead of AsOnDate,InstalledOn Date is passed as CurrentDate
        ''mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, objAssemblyMonitorInspStatus.AssemblyID, objAssemblyMonitorInspStatus.AssemblyStatusID, objAssemblyMonitorInspStatus.AsOnDate.ToString, objAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, objAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, objAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, objAssemblyMonitorInspStatus.AssemblyID, objAssemblyMonitorInspStatus.AssemblyStatusID, mAssemblyStatus.InstalledOn.ToString, objAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, objAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, objAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mPrevAssemblyMonitorInspStatus") = objAssemblyMonitorInspStatus
        'Added by Vikrant on 1-Aug-2011
        mAircraft = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).MachineInfo
        mAssemblyType = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyType
        mAssemblyInfo = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyInfo
        mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
        MarkLog(Util.Action.Comply, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)

        Session("From") = 0 'New record
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

        'Added By Vikrant On 25-Nov-2014
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorInspStatus.ID)
        Session("mFileAttach") = mFileAttach
        'End

        Response.Redirect("wfComplyAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
    End Sub
    Private Sub DeleteRecordInspection(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteInspection")
        mComplyAssemblyMonitorInspStatusList.CurrentIndex = Index
        Session("mComplyAssemblyMonitorInspStatusList") = mComplyAssemblyMonitorInspStatusList
    End Sub
    Private Sub EditRecordInspection(ByVal Index As Integer)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim objAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim AssemblyMonitorInspStatusInfo As tmpComplyAssemblyMonitorInspStatusList.tmpComplyAssemblyMonitorInspStatusInfo = mComplyAssemblyMonitorInspStatusList(Index)
        objAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mComplyAssemblyMonitorInspStatusList(Index).AssemblyMonitorInspStatusID, mComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID, mMachine.HourType)
        'Code added By Deven on 25/09/2009
        Dim mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
        mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList

        '----------------------------------
        If AssemblyMonitorInspStatusInfo.IsMaster Then
            Session("mAssemblyMonitorInspStatus") = objAssemblyMonitorInspStatus
            Response.Redirect("wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        Else
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(objAssemblyMonitorInspStatus.ID, objAssemblyMonitorInspStatus.AssemblyStatusID, objAssemblyMonitorInspStatus.DoneOnFormatted.ToString, mMachine.HourType, True)
            Session("mPrevAssemblyMonitorInspStatus") = objAssemblyMonitorInspStatus
            Session("From") = 1 'Edit record
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            'Added by Vikrant on 1-Aug-2011
            mAircraft = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).MachineInfo
            mAssemblyType = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyType
            mAssemblyInfo = mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyInfo
            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
            MarkLog(Util.Action.Edit, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorInspStatusList.Item(mComplyAssemblyMonitorInspStatusList.CurrentIndex).AssemblyMonitorInspStatusID, EventLogID)

            'Added by Saylee on 17-Jun-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(objAssemblyMonitorInspStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            If mAssemblyMonitorInspStatus.IsAttachmentAdded Then
                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorInspStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            Response.Redirect("wfComplyAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        End If
    End Sub
    Private Sub RemoveSessionInspection()
        Session.Remove("mPrevAssemblyMonitorInspStatus")
        Session.Remove("From")
        mComplyAssemblyMonitorInspStatusList = Nothing
        Session.Remove("mComplyAssemblyMonitorInspStatusList")
        'Session.Remove("mFileAttach") 'Added By Vikrant On 25-Nov-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
#End Region

#Region "Directive Tab"
    Private Sub SetGridDirective()
        Dim B As Boolean
        For j As Integer = 0 To dgMonitorModStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorModStatusList.Rows(j).Cells(25).Text, Boolean)
            If B = False Then
                dgMonitorModStatusList.Rows(j).Cells(24).Enabled = False
            End If
        Next
    End Sub
    Private Sub FindNowDirective()
        dgMonitorModStatusList.PageIndex = 0
        Session("LookInCombo") = cmbLookInDirective.SelectedValue
        Session("TextFor") = txtForDirective.Text
        Session("TextCode") = txtCodeDirective.Text
        Session("SearchForCombo") = cmbSearchForDirective.SelectedValue

        Select Case cmbLookInDirective.SelectedIndex
            Case 0, -1  'All
                mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 1  'ATA Code
                mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), Val(txtCodeDirective.Text), ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 2 ' Directive No.
                mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , DirectiveNo:=txtForDirective.Text.Trim, ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 3  'Description
                mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , txtForDirective.Text.Trim, ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 4  'Mod Type ID
                mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , CInt(cmbSearchForDirective.SelectedValue), ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 5 ' Work Order No.
                mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , , txtForDirective.Text.Trim, ShowNotApplicable:=chkApplicableDirective.Checked)
            Case 6  'Show In C of A
                mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString).ToShortDateString, mAssemblyStatus.MachineID.ToString, Trim(mAssemblyStatus.ModelName), Trim(mAssemblyStatus.Assembly.SerialNo), , , , , , True, ShowNotApplicable:=chkApplicableDirective.Checked)
        End Select
        Session("mComplyAssemblyMonitorModStatusList") = mComplyAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataSource = mComplyAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataBind()
    End Sub
    Private Sub SetPageDirective()
        lblModifications.Text = "List of all the Directives on the " & mAssemblyStatus.AssemblyTypeName & " as of " & mAssemblyStatus.InstalledOnFormatted & ". All the values will be as of " & mAssemblyStatus.InstalledOnFormatted
        lblResultDirective.Text = "List of Directives: " & mComplyAssemblyMonitorModStatusList.Count & " Record(s)."
    End Sub
    Private Sub ControlVisibilityDirective()
        btnAddTopDirective.Visible = mComplyAssemblyMonitorModStatusList.Count > 25
        'btnBackTopDirective.Visible = mComplyAssemblyMonitorModStatusList.Count > 25
        btnPrintTopDirective.Visible = mComplyAssemblyMonitorModStatusList.Count > 25
        btnPrintDirective.Enabled = mComplyAssemblyMonitorModStatusList.Count > 0
        btnPrintTopDirective.Enabled = mComplyAssemblyMonitorModStatusList.Count > 0
        dgMonitorModStatusList.Columns(20).Visible = IIf(chkApplicableDirective.Checked, False, True)
    End Sub
    Private Sub DisplayControlsDirective(ByVal Index As Integer)
        txtForDirective.Text = IIf(Index = 2 Or Index = 3 Or Index = 5, txtForDirective.Text, "")
        txtCodeDirective.Text = IIf(Index = 1, txtCodeDirective.Text, "")
        '=========================================================
        txtCodeDirective.Visible = IIf(Index = 1, True, False)
        txtForDirective.Visible = IIf(Index = 2 Or Index = 3 Or Index = 5, True, False)
        lblForDirective.Visible = (Index > 0 And Index <> 6)
        cmbSearchForDirective.Visible = (Index = 4)
        'New addition by Amrita on 9-Jan-08 for solving Bug No:-ML1 given by Pramod
        If cmbLookInDirective.Enabled = True Then
            cmbLookInDirective.Focus()
        End If
    End Sub
    Private Sub NewRecordDirective()
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'MarkLog(Util.Action.[New], "AssemblyMonitorModStatus", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID)
        'Code added By Saylee on 1/4/2008 suggested by Deven sir
        'Response.Redirect("wfAssemblyMonitorModStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssemblyMonitorModStatusList.aspx")

        'Code added By Deven on 25/09/2009
        Dim mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
        mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        '----------------------------------
        mModelMaintenanceActivityListCount = ModelMaintenanceActivityListCount.GetModelMaintenanceActivityListCount(mAssemblyStatus.Assembly.ModelID)
        If mModelMaintenanceActivityListCount.ModelModListCount > 0 Then
            Response.Redirect("wfModelMonitorModList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx" & "&GChildPage3=wfInstallAssembly_Ajax.aspx")
        Else
            Dim mModelMonitorMod As ModelMonitorMod
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, mAssemblyStatus.Assembly.ModelID, mMachine.HourType, ID)
            Session("mModelMonitorMod") = mModelMonitorMod
            'RemoveSessionDirective()
            MarkLog(Util.Action.[New], "Model Directive", " Model : " & mAssemblyStatus.Assembly.ModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorServiceList_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow()", True)
        End If

        '------------------------------------------------
    End Sub
    Private Sub EditMasterRecordDirective(ByVal mMasterId As Guid, ByVal mId As Guid, ByVal Index As Integer)
        Dim mModelMonitorMod As ModelMonitorMod
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim objAssemblyMonitorModStatus As AssemblyMonitorModStatus
        objAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mComplyAssemblyMonitorModStatusList(Index).AssemblyMonitorModStatusID, mAssemblyStatus.ID, mMachine.HourType)
        Dim mAssemblyMonitorModStatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo = mComplyAssemblyMonitorModStatusList(Index)

        Dim mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
        mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        '----------------------------------
        If mAssemblyMonitorModStatusInfo.IsMaster Then
            mAssemblyMonitorModStatus = objAssemblyMonitorModStatus
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(objAssemblyMonitorModStatus.ID, objAssemblyMonitorModStatus.AssemblyStatusID, mAssemblyStatus.InstalledOnFormatted.ToString, mMachine.HourType)
            Session("mPrevAssemblyMonitorModStatus") = objAssemblyMonitorModStatus
            Session("From") = 1 'Edit record
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        End If

        ''***
        mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mMasterId, mMachine.HourType)
        '' mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mId, mAssemblyStatus.ID, mMachine.HourType)
        '' Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mMachine") = mMachine
        Session("mModelMonitorMod") = mModelMonitorMod
        'RemoveSession()
        'Added by Vikrant on 1-Aug-2011
        mAircraft = mComplyAssemblyMonitorModStatusList(mId).MachineInfo
        mAssemblyType = mComplyAssemblyMonitorModStatusList(mId).AssemblyType
        mAssemblyInfo = mComplyAssemblyMonitorModStatusList(mId).AssemblyInfo
        mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
        MarkLog(Util.Action.Edit, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorModStatusList(mId).AssemblyMonitorModStatusID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow()", True)
        'Response.Redirect("wfModelMonitorMod_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfInstallAssembly_Ajax.aspx")
    End Sub
    Private Sub DataFieldBindDirective()
        mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(All)")
        cmbSearchForDirective.DataSource = mModelMonitorModTypeList
        Session("mModelMonitorModTypeList") = mModelMonitorModTypeList

        mComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(mAssemblyStatus.InstalledOnFormatted.ToString, mAssemblyStatus.MachineID.ToString, mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, ShowNotApplicable:=chkApplicableDirective.Checked)
        dgMonitorModStatusList.DataSource = mComplyAssemblyMonitorModStatusList
        Session("mComplyAssemblyMonitorModStatusList") = mComplyAssemblyMonitorModStatusList

        cmbSearchForDirective.DataBind()
        dgMonitorModStatusList.DataBind()
        chkApplicableDirective.Checked = False
    End Sub
    Private Sub SetControlDirective()
        'Fuction added by Saylee on 11th-Jan-2008 to keep Searching criteia as it is
        cmbLookInDirective.SelectedValue = LookInCombo 'IIf(LookIn = "", "(All)", LookIn)
        txtForDirective.Text = TextFor
        txtCodeDirective.Text = TextCode
        cmbSearchForDirective.SelectedValue = SearchForCombo 'IIf(SearchFor = "", "(All)", SearchFor)
        DisplayControlsDirective(cmbLookInDirective.SelectedIndex)
        FindNowDirective()
    End Sub
    Private Sub ComplyRecordDirective(ByVal Index As Integer)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim objAssemblyMonitorModStatus As AssemblyMonitorModStatus
        objAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)
        If objAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And objAssemblyMonitorModStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not mComplyAssemblyMonitorModStatusList.Item(Index).IsApplicable Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "Modification monitoring is not applicable, can not be complied", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            'Commented and changed by Saylee on 28-Oct-2009 Instead of AsOnDate,InstalledOn Date is passed as CurrentDate
            ''mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, objAssemblyMonitorModStatus.AssemblyID, objAssemblyMonitorModStatus.AssemblyStatusID, objAssemblyMonitorModStatus.AsOnDate.ToString, objAssemblyMonitorModStatus.ModelMonitorMod.ModelID, objAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, objAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, objAssemblyMonitorModStatus.AssemblyID, objAssemblyMonitorModStatus.AssemblyStatusID, mAssemblyStatus.InstalledOn.ToString, objAssemblyMonitorModStatus.ModelMonitorMod.ModelID, objAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, objAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            'Added by Saylee on 17-Jun-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(objAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            Session("mPrevAssemblyMonitorModStatus") = objAssemblyMonitorModStatus
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("From") = 0 'New record
            'Added by Vikrant on 1-Aug-2011
            mAircraft = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).MachineInfo
            mAssemblyType = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyType
            mAssemblyInfo = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyInfo
            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
            MarkLog(Util.Action.Comply, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, EventLogID)

            'Added By Vikrant On 25-Nov-2014
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorModStatus.ID)
            Session("mFileAttach") = mFileAttach
            'End

            Response.Redirect("wfComplyAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        End If
    End Sub
    Private Sub DeleteRecordDirective(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDirective")
        mComplyAssemblyMonitorModStatusList.CurrentIndex = Index
        Session("mComplyAssemblyMonitorModStatusList") = mComplyAssemblyMonitorModStatusList
    End Sub
    Private Sub EditRecordDirective(ByVal Index As Integer)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim objAssemblyMonitorModStatus As AssemblyMonitorModStatus
        objAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mComplyAssemblyMonitorModStatusList(Index).AssemblyMonitorModStatusID, mAssemblyStatus.ID, mMachine.HourType)
        Dim mAssemblyMonitorModStatusInfo As tmpComplyAssemblyMonitorModStatusList.tmpComplyAssemblyMonitorModStatusInfo = mComplyAssemblyMonitorModStatusList(Index)
        'Code added By Deven on 25/09/2009
        Dim mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
        mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True)
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        '----------------------------------
        If mAssemblyMonitorModStatusInfo.IsMaster Then
            mAssemblyMonitorModStatus = objAssemblyMonitorModStatus
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Response.Redirect("wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(objAssemblyMonitorModStatus.ID, objAssemblyMonitorModStatus.AssemblyStatusID, mAssemblyStatus.InstalledOnFormatted.ToString, mMachine.HourType, True)
            Session("mPrevAssemblyMonitorModStatus") = objAssemblyMonitorModStatus
            Session("From") = 1 'Edit record
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            'Added by Vikrant on 1-Aug-2011
            mAircraft = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).MachineInfo
            mAssemblyType = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyType
            mAssemblyInfo = mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyInfo
            mDetail = "Aircraft : " + mAircraft + " Assembly Type : " + mAssemblyType + " Assembly Info. : " + mAssemblyInfo
            MarkLog(Util.Action.Edit, "Assembly Installation", mDetail, Util.ErrorType.NoError, mComplyAssemblyMonitorModStatusList.Item(mComplyAssemblyMonitorModStatusList.CurrentIndex).AssemblyMonitorModStatusID, EventLogID)

            'Added by Saylee on 17-Jun-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(objAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            If mAssemblyMonitorModStatus.IsAttachmentAdded Then
                mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID) 'Sort = 1 - Installation
                Session("mFileAttach") = mFileAttach
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorModStatus.ID)
                Session("mFileAttach") = mFileAttach
            End If
            'End

            Response.Redirect("wfComplyAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
        End If
    End Sub
    Private Sub RemoveSessionDirective()
        mComplyAssemblyMonitorModStatusList = Nothing
        Session.Remove("From")
        Session.Remove("mPrevAssemblyMonitorModStatus")
        Session.Remove("mComplyAssemblyMonitorModStatusList")
        'Session.Remove("mFileAttach") 'Added By Vikrant On 25-Nov-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
#End Region

#Region "Parameters Tab"
    Private Sub DataFieldBindParameters()
        mParameterList = ParameterList.GetParameterList("(SELECT)")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
        Session("mAssemblyStatus") = mAssemblyStatus
        upnlParameters.DataBind()
        txtMin.Text = ""
        txtMax.Text = ""
    End Sub
    Private Sub SetPageParameters()
        lblResultParameters.Text = "List of Parameters: " & mAssemblyStatus.AssemblyParameters.Count & " Record(s)."
    End Sub
    Private Sub ControlVisibilityParameters()
        dgParameterList.Columns(6).Visible = Not mMachine.AssemblyStatus.HasLogCount
    End Sub
    Private Sub EditRecordParameters(ByVal Index As Int32)
        mAssemblyStatus.AssemblyParameters.CurrentIndex = Index
        txtMin.Text = mAssemblyStatus.AssemblyParameters.Item(Index).MinValue
        txtMax.Text = mAssemblyStatus.AssemblyParameters.Item(Index).MaxValue
        cmbParameterList.SelectedValue = mAssemblyStatus.AssemblyParameters.Item(Index).ParameterID.ToString
        cmbParameterList.Enabled = False
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub NewRecordParameter()
        Dim mParameter As Parameter
        mParameter = Parameter.NewParameter(Guid.NewGuid)
        Session("mParameter") = mParameter
    End Sub
    Private Sub RemoveSessionParameter()
        Session.Remove("mParameterList")
        'Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
#End Region

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "(SELECT)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        mModelList = ModelList.GetModelList(mAssemblyStatus.AssemblyTypeID, , , , "(SELECT)")
        cmbModelList.DataSource = mModelList
        Session("mModelList") = mModelList

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, IsTagRequired:=True, TagText:="(SELECT)", SkipIsForInventoryAircarft:=True)
        cmbMachineList.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods

        'Commeneted by Saylee on 15-Feb-2013 for UHPL15022013
        'dgInstallationValue.Columns(2).HeaderText = mAssemblyStatus.AssemblyTypeName

        SetGridHeader()  'Added by Saylee on 15-Feb-2013 for UHPL15022013

        'Added on 28-05-2007 by Kalpesh Shah
        If IsDate(mAssemblyStatus.InstalledOn) Then
            txtInstalledOnDate.Text = CDate(mAssemblyStatus.InstalledOn).ToString(AppSettings("DateFormat"))
        End If


        'Added by Saylee on 6th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        '=======================================================
        BindLicenceNo() 'MLNo

        DataBind()

        '=============Added by Saylee on 11th-Jan-2008 for bug-IIA14 (Maintenance)==============================
        If cmbATAChapter.Items.Contains(New System.Web.UI.WebControls.ListItem(mAssemblyStatus.ATAChapter, mAssemblyStatus.ATAID.ToString)) Then
            cmbATAChapter.SelectedValue = mAssemblyStatus.ATAID.ToString
        Else
            cmbATAChapter.SelectedValue = Guid.Empty.ToString
        End If
        '======================================================================================

        If mFileAttach Is Nothing Then
            If mAssemblyStatus.IsAttachmentAdded = True Then
                mFileAttach = FileAttach.GetAttachment(mAssemblyStatus.ID, 1) 'Sort = 1 - Installation
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1)
            End If
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SetGridHeader()
        Select Case mAssemblyStatus.AssemblyTypeID
            Case 4
                dgInstallationValue.Columns(1).HeaderText = "A.P.U."
            Case 5
                If (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
                    dgInstallationValue.Columns(2).HeaderText = "A.C."
                ElseIf AppSettings("ClientCode") = "Indamer" Then 'Added by Vikrant on 26-sept-2011 For ALL26092011-1
                    dgInstallationValue.Columns(2).HeaderText = "Air-Conditioning"
                Else
                    dgInstallationValue.Columns(2).HeaderText = "C.G.B."
                End If
            Case 8
                If (AppSettings("ClientCode") = "UHPL") Then
                    dgInstallationValue.Columns(2).HeaderText = "M.R.H."
                End If
            Case 9
                If (AppSettings("ClientCode") = "UHPL") Then
                    dgInstallationValue.Columns(2).HeaderText = "S.P.S."
                End If
            Case 10
                If (AppSettings("ClientCode") = "UHPL") Then
                    dgInstallationValue.Columns(2).HeaderText = "S.S.A."
                End If
            Case Else
                dgInstallationValue.Columns(2).HeaderText = mAssemblyStatus.AssemblyTypeName
        End Select
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyStatus") = mAssemblyStatus
        dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
        dgInstallationValue.DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "cmbModelList" Then
            If cmbModelList.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Please select Model from the list"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "txtSerialNo" Then
            If txtSerialNo.Text = "" Then
                CustValid.ErrorMessage = "Serial No required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Changed By Yogita on 8-Jan-2008 bcaz it fires two times from object & from here also. Plz give "Please select the Machine from the list." this msg in object
            'ElseIf CustValid.ControlToValidate = "cmbMachineList" Then
            '    If cmbMachineList.SelectedIndex = 0 Then
            '        CustValid.ErrorMessage = "Please select the Machine from the list."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        ElseIf CustValid.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 200 Then
                CustValid.ErrorMessage = "Max length of Note should be 200 char."
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
            'Added By Vikrant On 11-Nov-2020 For ALL27072020
        ElseIf CustValid.ControlToValidate = "txtInstalledOnDate" Then
            If txtInstalledOnDate.Text <> "" And Not mRemovedAssemblyStatus Is Nothing Then
                If mRemovedAssemblyStatus.IsSpareAssembly And mRemovedAssemblyStatus.AsOnDateFormatted <> "" AndAlso New SmartDate(txtInstalledOnDate.Text).CompareTo(New SmartDate(mRemovedAssemblyStatus.AsOnDateFormatted.ToString)) < 0 Then
                    CustValid.ErrorMessage = "Installation date should be later to Stock Assembly Built date: " & mRemovedAssemblyStatus.AsOnDateFormatted.ToString
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
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
        Dim txtAssemblyInstallationValue As TextBox   'Added Code   Jan-12,2007
        If Not mAssemblyStatus.IsValid Then
            For i As Integer = 0 To mAssemblyStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgInstallationValue.Rows.Count - 1)
            txtAssemblyInstallationValue = CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)   'Added Code Jan-12,2007
            If Not mAssemblyStatus.AssemblyStatusPeriods.Item(i).IsValid Then
                For x As Integer = 0 To mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    'Added by Saylee on 19-Mar-2013 for ALL14032013-1
    Public Function CheckPeriodsForRemovedAssemblyStatus(ByVal RemovedAssemblyStatus As AssemblyStatus) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        If RemovedAssemblyStatus.AssemblyTypeID = 2 Or RemovedAssemblyStatus.AssemblyTypeID = 4 Then Return True
        mMachine = Machine.GetMachine(New Guid(cmbMachineList.SelectedValue))
        While i <= RemovedAssemblyStatus.AssemblyStatusPeriods.Count - 1
            If mMachine.AssemblyStatus.AssemblyStatusPeriods.Contains(RemovedAssemblyStatus.AssemblyStatusPeriods(i).PeriodID) Then
                tmpIsPeriodExists = True
            Else
                tmpIsPeriodExists = False
                Exit While
            End If
            i = i + 1
        End While
        Return tmpIsPeriodExists
    End Function
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        For i As Integer = 0 To CShort(dgInstallationValue.Rows.Count - 1)
            If Not mAssemblyStatus.AssemblyStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next

        If str <> "" Then
            cvInstallationRemark.ErrorMessage = str
            cvInstallationRemark.IsValid = False
            Return False
        Else
            Return True
        End If
    End Function

#End Region

#Region " Events "

#Region "Common"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If cmbATAChapter.Enabled = True Then
                cmbATAChapter.Focus()
            End If
            AddSelectedPeriods()
            DataFieldBind()
            SetCaptions()
            ControlVisibility()
            ControlVisibilityForTabs()
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
            tabContainer.ActiveTabIndex = IIf(CType(Session("AssemblyInstTabIndex"), Integer) > 0, CType(Session("AssemblyInstTabIndex"), Integer), 0)
            If CType(Session("AssemblyInstTabIndex"), Integer) > 0 Then
                Call tabContainer_ActiveTabChanged(Nothing, Nothing)
            End If

            'Added by Saylee on 24-apr-2023
            Dim lblServiceTitle As Label

            lblServiceTitle = tabContainer.Tabs(2).FindControl("lblServiceListTitle")
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then

                ' tbPnlServiceList.HeaderTemplate = "MPD List"
                lblServiceTitle.Text = "Maintenance Event(s)"
                tabContainer.Tabs(3).Visible = False
            Else

                'tbPnlServiceList.HeaderTemplate = "Service List"
                lblServiceTitle.Text = "Service(s)"
                tabContainer.Tabs(3).Visible = Not mAssemblyStatus.IsNew
            End If
            '**************************
            upnlContainer.Update()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            'Added by Vikrant on 28-July-2011
            mAssemblyDetail = "Reg No. : " + cmbMachineList.SelectedItem.Text + " Model : " + cmbModelList.SelectedItem.Text + " Serial No. : " + txtSerialNo.Text & " Installed On : " + txtInstalledOnDate.Text
            MarkLog(Util.Action.Save, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to save " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If IsValid Then
            If Not CustomValidate2() Then
                upnlValidationSummary.Update()
                Exit Sub
            End If

            'Added by Saylee on 18-Jul-2018 for ALL18072018-1 : Locking backdated installations on Comp and Assembly
            If mFromType = From.EditInstall And (mAssemblyStatus.IsRemoved = True) Then
                MSGBoxCtrl.Show("Installation Alert!", "Assembly detail(s) cannot be modified as it is removed. " & " Revert the Removal and then modify.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            '*******************************************************************

            'Added by Saylee on 19-Mar-2013 for ALL14032013-1
            If CheckPeriodsForRemovedAssemblyStatus(mAssemblyStatus) = False Then
                'Str = Str() + "Periods for selected " & mAssemblyStatus.AssemblyTypeName & " are mismatching with selected Installed On " & cmbMachineList.SelectedItem.Text & " Aircraft.Can not be installed."
                'Dim msg1 As New SIMsgBox(Page, "<BR>Assembly Status Installation Alert!", "<BR><BR>Periods for selected " & mAssemblyStatus.AssemblyTypeName & " are mismatching with selected Installed On " & cmbMachineList.SelectedItem.Text & " Aircraft.Can not be installed.", "", MsgBoxStyle.OKOnly)
                MSGBoxCtrl.Show("Assembly Status Installation Alert!", "Periods for selected " & mAssemblyStatus.AssemblyTypeName & " are mismatching with selected Installed On " & cmbMachineList.SelectedItem.Text & " Aircraft.Can not be installed.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            '***********************************

            If Save() = True Then
                'Added by Saylee on 14-July-2009
                Session("mAircraftInformationBoardList") = Nothing
                '*********************************

                Try
                    'Added by Vikrant on 28-July-2011
                    'mRegNo = cmbMachineList.SelectedItem.Text
                    'mModelName = cmbModelList.SelectedItem.Text
                    'mSerialNo = txtSerialNo.Text
                    'mAssemblyDetail = "Reg No. : " + mRegNo + " Model : " + mModelName + " Serial No. : " + mSerialNo
                    'MarkLog(Util.Action.Save, "Assembly Installation", mAssemblyDetail, Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
                Catch ex As Exception
                    '
                End Try
                'DataFieldBind()
                SetCaptions()
                ControlVisibility()
                ControlVisibilityForTabs()
                upnlTitle.Update()
                upnlInstallationDetails.Update()
                upnlInstallationValues.Update()
                upnlActionBtn.Update()
                upnlContainer.Update()
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgInstallationValue_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstallationValue.RowCommand
        Dim index As Integer
        Select Case e.CommandName
            Case "AssemblyInstallationValue"

            Case "DeleteRec"
                index = CInt(e.CommandArgument) + dgInstallationValue.PageIndex * dgInstallationValue.PageSize
                REM-If Monitoring of the assembly is done, then do not allow to remove periods
                '  If mAssemblyStatus.AssemblyStatusPeriods.Item(index).HasMonitor = True Then
                If mAssemblyStatus.AssemblyStatusPeriods.Item(index).HasMonitor Then       'Added Code
                    MSGBoxCtrl.show(MSGBox.Message_title.MonitorExist, MSGBox.Message_text.MonitorExist, "Selected " & mAssemblyStatus.AssemblyTypeName & " Period cannot be removed as monitor entry exist", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    REM-If component is added for the assembly, then do not allow to remove periods
                    'ElseIf mAssemblyStatus.AssemblyStatusPeriods.Item(index).HasCompStatusPeriod = True Then
                ElseIf mAssemblyStatus.AssemblyStatusPeriods.Item(index).HasCompStatusPeriod Then   'Added Code
                    MSGBoxCtrl.show(MSGBox.Message_title.ComponentPeriodExist, MSGBox.Message_text.ComponentPeriodExist, "Selected " & mAssemblyStatus.AssemblyTypeName & " Period cannot be removed as Component Period exist", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    'Added Code  jan-12.2007
                ElseIf mAssemblyStatus.AssemblyStatusPeriods(index).PeriodID = 1 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.HoursRemove, MSGBox.Message_text.HoursRemove, "Selected " & mAssemblyStatus.AssemblyTypeName & " period can not be removed.Hours Cannot Removed", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mAssemblyStatus.AssemblyStatusPeriods(index).PeriodID = 2 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.StartDateRemove, MSGBox.Message_text.StartDateRemove, "Selected " & mAssemblyStatus.AssemblyTypeName & " period can not be removed.Start date Cannot Removed", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    'End of Added Code
                Else
                    mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods.Item(index))
                    dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
                    SetGridHeader()
                    ControlVisibility()
                    dgInstallationValue.DataBind()
                End If
        End Select
    End Sub
    Protected Sub txtAssemblyInstallationValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyStatus.AssemblyStatusPeriods.Count - 1
            Dim txtAssemblyInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)

            'Added Code Jan-12,2007
            If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID = 2 Then
                If Period.IsDate(txtAssemblyInstallationValue.Text) Then
                    mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyCurrentValueFormatted = Trim(txtAssemblyInstallationValue.Text)
                Else
                    mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyCurrentValueFormatted = ""
                End If
            Else
                mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyCurrentValueFormatted = Trim(txtAssemblyInstallationValue.Text)
            End If
            'End of Added Code

            'mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyInstallationValue = txtAssemblyInstallationValue.Text.Trim

        Next i
        DataBindGrid()
    End Sub
    Private Sub btnAddPeriod_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPeriod.Click
        SetObject()
        SetGridObject()
        SetPeriods()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
    End Sub
    Private Sub hdnAddPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnAddPeriod.Click
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        AddSelectedPeriods()
        dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
        dgInstallationValue.DataBind()
        upnlInstallationValues.Update()
    End Sub
    Private Sub cmbModelList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbModelList.SelectedIndexChanged
        GetAssemblyStatusForModel(cmbModelList.SelectedIndex)  'Added by Saylee on 25-Aug-2009
        If cmbModelList.SelectedIndex > 0 Then
            Dim mModel As Model = Model.GetModel(New Guid(cmbModelList.SelectedValue))
            txtManufacturer.Text = mModel.Manufacturer.Name
        Else
            txtManufacturer.Text = ""
        End If
        'New addition By Yogita on 9-Jan-2008
        If cmbModelList.Enabled = True Then
            cmbModelList.Focus()
        End If
        cmbATAChapter.DataBind()
        upnlATADetails.Update()
    End Sub
    REM:-This event is used to compare the InstalledOn date and dtpInstalledOn date value, before it binds to the object
    Private Sub txtInstalledOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInstalledOnDate.TextChanged
        'If IsPostBack Then
        If mFromType = From.NewInstall Then
            If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString), SmartDate.StringToDate(txtInstalledOnDate.Text)) <> 0 Then
                SetObject()
                REM: Clone the object
                Dim clnAssemblyStatus As AssemblyStatus
                REM:-Restore the variable values before creating new record.
                clnAssemblyStatus = CType(mAssemblyStatus.Clone, AssemblyStatus)
                NewRecord()
                REM: Copy from Clone
                CopyFromClone(clnAssemblyStatus, True)
            End If
        End If
        If mFromType = From.EditInstall Then
            If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyStatus.InstalledOn.ToString), SmartDate.StringToDate(txtInstalledOnDate.Text)) <> 0 Then
                SetObject()
                REM: Clone the object
                Dim clnAssemblyStatus As AssemblyStatus
                REM:-Restore the modified values of the variables before EditRecord call
                clnAssemblyStatus = CType(mAssemblyStatus.Clone, AssemblyStatus)
                EditRecord()
                REM: Copy from Clone
                CopyFromClone(clnAssemblyStatus, False)
            End If
        End If
        DataBindGrid()
        upnlInstallationValues.Update()
        'End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "AssemblyInstallation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        'Session.Remove("mFileAttach") 'Added By Vikrant On 01-Dec-2014
        'Response.Redirect(Request.QueryString("BackPage"))
        Response.Redirect("index.aspx")
    End Sub
    Private Sub ImgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnATAChapter.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenATAWindow", "OpenATAWindow()", True)
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATADetails.Update()
    End Sub
    'Added by Vikrant On 01-Dec-2014
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
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
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Public Sub tabContainer_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabContainer.ActiveTabChanged
        Select Case Session("AssemblyInstTabIndex")
            Case 0
                'RemoveSession()
            Case 1 'Remove Component List Session
                RemoveSessionComponentList()
            Case 2 'Remove Service List Session
                RemoveSessionService()
            Case 3 'Remove Inspection List Session
                RemoveSessionInspection()
            Case 4 'Remove Directive List Session
                RemoveSessionDirective()
            Case 5 'Remove Parameter List Session
                RemoveSessionParameter()

        End Select
        Session("AssemblyInstTabIndex") = tabContainer.ActiveTabIndex
        Session.Remove("mMaintenanceDoneByEmployees")
        Select Case tabContainer.ActiveTabIndex
            Case 0
                'If (Session("From") = 1) Then
                '    If (Session("InstallSelected") = 1) Then
                '        cmbATAChapter.Enabled = False
                '        txtPartDescription.Enabled = False
                '        chkByModel.Enabled = False
                '        btnPartNo.Enabled = False
                '        ImgBtnATAChapter.Enabled = False

                '        If btnSelectLog.Enabled = True Then
                '            setFocus(btnSelectLog)
                '        End If

                '        Session("mInstallSelected") = mInstallSelected

                '        ' ''Session.Remove("InstallSelected")
                '    Else
                '        If cmbATAChapter.Enabled = True Then
                '            setFocus(cmbATAChapter)
                '        End If
                '    End If
                'ElseIf (Session("From") = 2) Then
                '    If btnSelectLog.Enabled = True Then
                '        setFocus(btnSelectLog)
                '    End If
                'End If
                ''===============================================================
                'SetLog()
                'AddSelectedPeroids()
                'DataFieldBind()

                ''---------- 28-Apr-2009
                'If Session("IsAdded") = "False" Then
                '    Call cmbAssemblyList_SelectedIndexChanged(Nothing, Nothing)
                '    SetPeroids()
                '    For i As Integer = 0 To mSelectPeriods.Count - 1
                '        mSelectPeriods(i).IsSelected = True
                '    Next
                '    AddSelectedPeroids()

                '    Session("IsAdded") = "True"
                'End If
                ''----------
                'GetAttachment()  'Added By Saylee On 27-Nov-2014 
                'SetPage()
                'ControlVisibility()
                'ControlVisiblity1() 'Added By Prashant 26-Aug-2010
                'If (AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                '    txtPartDescription.Visible = True
                '    cmbPartNo.Visible = False
                'Else
                '    cmbPartNo.Visible = True
                '    txtPartDescription.Visible = False
                'End If
            Case 1 'Component List Tab
                txtCodeComponentList.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeComponentList').value,event)")
                If cmbLookInComponentList.Enabled = True Then
                    cmbLookInComponentList.Focus()
                End If
                DataFieldBindComponentList()
                DisplayControlsComponentList(0)
                SetPageComponentList()
                ControlVisibilityComponentList()
                upnlComponentList.Update()
            Case 2 'Service Tab
                txtCodeService.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeService').value,event)")
                If cmbLookInService.Enabled = True Then
                    cmbLookInService.Focus()
                End If
                DataFieldBindService()
                SetControlService()
                SetPageService()
                SetGridService()
                ControlVisibilityService()
                upnlService.Update()
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Description"
                Else
                    dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Code/Form No./Description"
                End If
            Case 3 'Inspection Tab
                txtCodeInspection.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeInspection').value,event)")
                If cmbLookInInspection.Enabled = True Then
                    cmbLookInInspection.Focus()
                End If
                DataFieldBindInspection()
                SetControlInspection()
                SetPageInspection()
                SetGridInspection()
                ControlVisibilityInspection()
                upnlInspection.Update()

            Case 4 'Directive Tab
                txtCodeDirective.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeDirective').value,event)")
                If cmbLookInDirective.Enabled = True Then
                    cmbLookInDirective.Focus()
                End If
                DataFieldBindDirective()
                SetControlDirective()
                SetPageDirective()
                SetGridDirective()
                ControlVisibilityDirective()
                upnlDirective.Update()
            Case 5 'Parameters Tab
                txtMin.Attributes.Add("onKeyPress", "validateText(('ND'),document.getElementById('txtMin').value,event)")
                txtMax.Attributes.Add("onKeyPress", "validateText(('ND'),document.getElementById('txtMax').value,event)")
                cmbParameterList.Focus()
                DataFieldBindParameters()
                SetPageParameters()
                ControlVisibilityParameters()
                upnlParameters.Update()
        End Select
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mAssemblyStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyStatus.InstalledOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                'mAssemblyStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyStatus.MaintenanceDoneByEmployees.Remove(mAssemblyStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyStatus") = mAssemblyStatus
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
            If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyStatus.MaintenanceDoneByEmployees.Add(mAssemblyStatus.ID, 1, DoneByID, LicenseNo, "", EmpName)
            End If

        Else
            If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyStatus") = mAssemblyStatus
        BindLicenceNo()
        SetLicenceCount()
    End Sub
    'End
    Private Sub lnkPrintLogBookEntry_Click(sender As Object, e As System.EventArgs) Handles lnkPrintLogBookEntry.Click  'Added By Prashant On 7-May-2021 ALL07052021
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mLogEntryFormat As New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        RptCommonHistory = New crptLogEntryFormat

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mAssemblyStatus.InstalledOn, mAssemblyStatus.InstalledOn, "", mAssemblyStatus.AssemblyTypeName,
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "",
                                                        mAssemblyStatus.MachineID.ToString, True, True, IsRemoved:=False, IsInstalled:=True,
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True,
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False,
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mAssemblyStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "OpenFromAssemblyRemovalInstallationComponentRemovalInstallation", mAssemblyStatus.InstalledOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo,
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo,
           IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper),
           AppSettings("Product Version"), AppSettings("SINote"),
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.",
           "True", mAssemblyStatus.InstalledOnFormatted, "", AppSettings("Logo"))

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

#Region "Component List Tab"
    Private Sub dgCompStatusList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCompStatusList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgCompStatusList.PageIndex * dgCompStatusList.PageSize
                EditRecordComponentList(Index)
            Case "DeleteRec"
                If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgCompStatusList.PageIndex * dgCompStatusList.PageSize
                DeleteRecordComponentList(Index)
        End Select
    End Sub
    Private Sub dgCompStatusList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCompStatusList.Sorting
        mtmpInstalledCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        dgCompStatusList.DataSource = mtmpInstalledCompList
        dgCompStatusList.DataBind()
    End Sub
    Private Sub btnPrintTopComponentList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintTopComponentList.Click, btnPrintComponentList.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstallAssemblyMonitorComp
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
       mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation",
       "Period", "Assembly", , "Airframe"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
                     mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation",
                      "", "", , ""))
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                          mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                           mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                          "", "", , "", , , , , ))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                          mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                         mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo. :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "",
                   "", , , , , , , , , , , , , , , , , ,
                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
            End If
        Next

        'For Install Assembly Component List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblComponentsCaption.Text))

        'For Install Assembly Component List
        ReportDetails.Add(New rptStatus(, 2, ,
              , , , dgCompStatusList.Columns.Item(1).HeaderText, , dgCompStatusList.Columns.Item(2).HeaderText, dgCompStatusList.Columns.Item(3).HeaderText, dgCompStatusList.Columns.Item(4).HeaderText,
              dgCompStatusList.Columns.Item(5).HeaderText, dgCompStatusList.Columns.Item(6).HeaderText, dgCompStatusList.Columns.Item(7).HeaderText, , , , , , , , , , , , , , dgCompStatusList.Columns.Item(8).HeaderText))
        Dim TotalCount1 As Integer
        TotalCount1 = Me.mtmpInstalledCompList.Count
        Dim m As Integer

        For m = 0 To TotalCount1 - 1
            Dim str(7) As String
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""

            If Me.dgCompStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgCompStatusList.Rows(m).Cells(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgCompStatusList.Rows(m).Cells(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgCompStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgCompStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgCompStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgCompStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgCompStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(7) = Me.dgCompStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
       , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , , str(7)))
        Next
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite, "Component List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "InstallCompStatusList", "Component List Report", Util.ErrorType.NoError, Guid.Empty)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnAddComponentList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddComponentList.Click, btnAddTopComponentList.Click
        MarkLog(Util.Action.[New], "Assembly Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionComponentList()
        Session("From") = 3 'FromInstallAssembly
        NewRecordComponentList()
    End Sub
    Private Sub btnFindNowComponentList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNowComponentList.Click
        FindNowComponentList()
        SetPageComponentList()
        ControlVisibilityComponentList()
        upnlGridComponentList.Update()
        upnlActionBtnComponentList.Update()
    End Sub
    Private Sub cmbLookInComponentList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInComponentList.SelectedIndexChanged
        DisplayControlsComponentList(cmbLookInComponentList.SelectedIndex)
    End Sub
#End Region

#Region "Service Tab"
    Private Sub btnFindNowService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNowService.Click
        FindNowService()
        SetPageService()
        SetGridService()
        ControlVisibilityService()
        upnlGridService.Update()
        upnlActionBtnService.Update()
    End Sub
    Private Sub dgMonitorServiceStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorServiceStatusList.Sorting
        mComplyAssemblyMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mComplyAssemblyMonitorServiceStatusList") = mComplyAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mComplyAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
        SetGridService()
    End Sub
    Private Sub cmbLookInService_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInService.SelectedIndexChanged
        DisplayControlsService(cmbLookInService.SelectedIndex)
    End Sub
    Private Sub dgMonitorServiceStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorServiceStatusList.RowCommand
        Dim Index As Int32
        Dim mId As Guid
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Description"
        Else
            dgMonitorServiceStatusList.HeaderRow.Cells(6).Text = "Code/Form No./Description"
        End If
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecordService(Index)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordService(Index)
            Case "Comply"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ComplyRecordService(Index)
                'Added by Saylee on 6-Aug-2010
            Case "EditMaster"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mMasterId As Guid = New Guid(dgMonitorServiceStatusList.Rows(Index).Cells(19).Text)
                mId = New Guid(dgMonitorServiceStatusList.Rows(Index).Cells(0).Text)
                Session("EditMasterRecord") = "True"
                EditMasterRecordService(mMasterId, mId, Index)
                '---------------------------------------------------------------------------------
            Case "View"
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageIndex * dgMonitorServiceStatusList.PageSize
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mComplyAssemblyMonitorServiceStatusList(Index).ID)
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
    Private Sub btnPrintService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintService.Click, btnPrintTopService.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstallAssemblyMonitor
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
       mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation",
       "Period", "Assembly", , "Airframe"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
                     mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Value at Installation",
                      "", "", , ""))
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                           mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                           mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                          "", "", , "", , , , , ))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                          mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                         mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                         mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "",
                   "", , , , , , , , , , , , , , , , , ,
                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
            End If
        Next
        'For Install Assembly Service List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , lblServiceText.Text))

        'For Install Assembly service List
        ReportDetails.Add(New rptStatus(, 2, ,
              , , , dgMonitorServiceStatusList.Columns.Item(4).HeaderText, , dgMonitorServiceStatusList.Columns.Item(5).HeaderText, dgMonitorServiceStatusList.Columns.Item(6).HeaderText,
              dgMonitorServiceStatusList.Columns.Item(7).HeaderText, dgMonitorServiceStatusList.Columns.Item(8).HeaderText, dgMonitorServiceStatusList.Columns.Item(9).HeaderText,
               dgMonitorServiceStatusList.Columns.Item(10).HeaderText, , dgMonitorServiceStatusList.Columns.Item(11).HeaderText, dgMonitorServiceStatusList.Columns.Item(12).HeaderText,
                dgMonitorServiceStatusList.Columns.Item(13).HeaderText, dgMonitorServiceStatusList.Columns.Item(14).HeaderText, dgMonitorServiceStatusList.Columns.Item(15).HeaderText,
                dgMonitorServiceStatusList.Columns.Item(16).HeaderText, , , , dgMonitorServiceStatusList.Columns.Item(17).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mComplyAssemblyMonitorServiceStatusList.Count
        Dim m As Integer

        For m = 0 To TotalCount1 - 1
            Dim str(13) As String
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

            If Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgMonitorServiceStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgMonitorServiceStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgMonitorServiceStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgMonitorServiceStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(12).Text <> "&nbsp;" Then str(8) = Me.dgMonitorServiceStatusList.Rows(m).Cells(12).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(13).Text <> "&nbsp;" Then str(9) = Me.dgMonitorServiceStatusList.Rows(m).Cells(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(14).Text <> "&nbsp;" Then str(10) = Me.dgMonitorServiceStatusList.Rows(m).Cells(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(15).Text <> "&nbsp;" Then str(11) = Me.dgMonitorServiceStatusList.Rows(m).Cells(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(16).Text <> "&nbsp;" Then str(12) = Me.dgMonitorServiceStatusList.Rows(m).Cells(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(17).Text <> "&nbsp;" Then str(13) = Me.dgMonitorServiceStatusList.Rows(m).Cells(17).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
                   , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), , str(7),
                       str(8), str(9), str(10), str(11), str(12), , , , str(13)))
        Next
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite, "Assembly Service Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If m = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "InstallAssemblyMonitorServiceStatusList", "Assembly Service Status List Report", Util.ErrorType.NoError, Guid.Empty)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnAddService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddService.Click
        MarkLog(Util.Action.[New], "Assembly Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionService()
        NewRecordService()
    End Sub
    Private Sub hdnBtnModelServiceMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelServiceMaster.Click
        SetControlService()
        SetPageService()
        SetGridService()
        upnlGridService.Update()
    End Sub
#End Region

#Region "Inspection Tab"
    Private Sub dgMonitorInspStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Dim Index As Int32
        Dim mId As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecordInspection(Index)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordInspection(Index)
            Case "Comply"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ComplyRecordInspection(Index)
            Case "EditMaster"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mMasterId As Guid = New Guid(dgMonitorInspStatusList.Rows(Index).Cells(18).Text)
                Session("EditMasterRecord") = "True"
                mId = New Guid(dgMonitorInspStatusList.Rows(Index).Cells(0).Text)
                EditMasterRecordInspection(mMasterId, mId, Index)
                '---------------------------------------------------------------------------------
            Case "View"
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageIndex * dgMonitorInspStatusList.PageSize
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mComplyAssemblyMonitorInspStatusList(Index).ID)
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
    Private Sub btnAddInspection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddInspection.Click, btnAddTopInspection.Click
        MarkLog(Util.Action.[New], "Assembly Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionInspection()
        NewRecordInspection()
    End Sub
    Private Sub btnFindNowInspection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowInspection.Click
        FindNowInspection()
        SetPageInspection()
        SetGridInspection()
        ControlVisibilityInspection()
        upnlActionBtnInspection.Update()
        upnlGridInspection.Update()
    End Sub
    Private Sub cmbLookInInspection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInInspection.SelectedIndexChanged
        DisplayControlsInspection(cmbLookInInspection.SelectedIndex)
    End Sub
    Private Sub dgMonitorInspStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorInspStatusList.Sorting
        mComplyAssemblyMonitorInspStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mComplyAssemblyMonitorInspStatusList") = mComplyAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mComplyAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
        SetGridInspection()
    End Sub
    Private Sub btnPrintInspection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintInspection.Click, btnPrintTopInspection.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstallAssemblyMonitor
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
       mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Values at Installation",
        "Period", "Assembly", , "Airframe"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
                      mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Values at Installation",
                      "", "", , ""))
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                          mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                            mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                          "", "", , "", , , , , ))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                          mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                         mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                        CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                         mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                         mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "",
                   "", , , , , , , , , , , , , , , , , ,
                   CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
            End If
        Next

        'For Install Assembly Inspection List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , lblInspectionsCaption.Text))

        'For Install Assembly Inspection List
        ReportDetails.Add(New rptStatus(, 2, ,
              , , , dgMonitorInspStatusList.Columns.Item(4).HeaderText, , dgMonitorInspStatusList.Columns.Item(5).HeaderText, dgMonitorInspStatusList.Columns.Item(6).HeaderText,
              dgMonitorInspStatusList.Columns.Item(7).HeaderText, dgMonitorInspStatusList.Columns.Item(8).HeaderText, dgMonitorInspStatusList.Columns.Item(9).HeaderText,
                dgMonitorInspStatusList.Columns.Item(10).HeaderText, , dgMonitorInspStatusList.Columns.Item(11).HeaderText, dgMonitorInspStatusList.Columns.Item(12).HeaderText,
                dgMonitorInspStatusList.Columns.Item(13).HeaderText, dgMonitorInspStatusList.Columns.Item(14).HeaderText, dgMonitorInspStatusList.Columns.Item(15).HeaderText,
                dgMonitorInspStatusList.Columns.Item(16).HeaderText, , , , dgMonitorInspStatusList.Columns.Item(17).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mComplyAssemblyMonitorInspStatusList.Count
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            Dim str(13) As String
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

            If Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(0) = Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(1) = Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgMonitorInspStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgMonitorInspStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgMonitorInspStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgMonitorInspStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(12).Text <> "&nbsp;" Then str(8) = Me.dgMonitorInspStatusList.Rows(m).Cells(12).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(13).Text <> "&nbsp;" Then str(9) = Me.dgMonitorInspStatusList.Rows(m).Cells(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(14).Text <> "&nbsp;" Then str(10) = Me.dgMonitorInspStatusList.Rows(m).Cells(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(15).Text <> "&nbsp;" Then str(11) = Me.dgMonitorInspStatusList.Rows(m).Cells(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(16).Text <> "&nbsp;" Then str(12) = Me.dgMonitorInspStatusList.Rows(m).Cells(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(17).Text <> "&nbsp;" Then str(13) = Me.dgMonitorInspStatusList.Rows(m).Cells(17).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
                   , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), , str(7),
                       str(8), str(9), str(10), str(11), str(12), , , , str(13)))
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite, "Assembly Inspection Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 10-May-2012
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage) 'Added by Shweta on 10-May-2012
        da.Fill(ds, Report)

        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "InstallAssemblyMonitorInspStatusList", "Assembly Inspection Status List Report", Util.ErrorType.NoError, Guid.Empty)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub hdnBtnModelInspMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelInspMaster.Click
        SetControlInspection()
        SetPageInspection()
        SetGridInspection()
        upnlGridInspection.Update()
    End Sub
#End Region

#Region "Directive Tab"
    Private Sub dgMonitorModStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorModStatusList.RowCommand
        Dim Index As Int32
        Dim mId As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecordDirective(Index)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordDirective(Index)
            Case "Comply"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                ComplyRecordDirective(Index)
                'Added by Saylee on 1/04/2008 suggested by Deven Sir
            Case "EditMaster"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("EditMasterRecord") = "True"
                Dim mMasterId As Guid = New Guid(dgMonitorModStatusList.Rows(Index).Cells(19).Text)
                mId = New Guid(dgMonitorModStatusList.Rows(Index).Cells(0).Text)
                EditMasterRecordDirective(mMasterId, mId, Index)
                '---------------------------------------------------------------------------------
            Case "View"
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageIndex * dgMonitorModStatusList.PageSize
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mComplyAssemblyMonitorModStatusList(Index).ID)
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
    Private Sub btnAddDirective_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddDirective.Click, btnAddTopDirective.Click
        MarkLog(Util.Action.[New], "Assembly Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionDirective()
        NewRecordDirective()
    End Sub
    Private Sub cmbLookInDirective_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInDirective.SelectedIndexChanged
        DisplayControlsDirective(cmbLookInDirective.SelectedIndex)
    End Sub
    Private Sub btnFindNowDirective_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowDirective.Click
        FindNowDirective()
        SetPageDirective()
        SetGridDirective()
        ControlVisibilityDirective()
        upnlGridDirective.Update()
        upnlActionBtnDirective.Update()
    End Sub
    Private Sub dgMonitorModStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorModStatusList.Sorting
        mComplyAssemblyMonitorModStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mComplyAssemblyMonitorModStatusList") = mComplyAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataSource = mComplyAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataBind()
        SetGridDirective()
    End Sub
    Private Sub btnPrintTopDirective_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintDirective.Click, btnPrintTopDirective.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstallAssemblyMonitor
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
      mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Values at Installation",
       "Period", "Assembly", , "Airframe"))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "ATA Chapter :",
                     mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Values at Installation",
                      "", "", , ""))
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                           mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Manufacturer :",
                           mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , ,
                          "", "", , "", , , , , ))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                          mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model :",
                                        mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "SerialNo :",
                                          mAssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , ,
                                          "", "", , "", , , , , ))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position :",
                                                          mAssemblyStatus.Position, , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                         CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "",
                                                          "", , , , , , , , , , , , , , , , , ,
                                                          "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "",
                   "", , , , , , , , , , , , , , , , , ,
                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyInstallationValueFormatted, String),
                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineInstallationValueFormatted, String)))
            End If
        Next


        'For Install Assembly Modification List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , lblModifications.Text))

        'For Install Assembly Modification List
        ReportDetails.Add(New rptStatus(, 2, ,
              , , , dgMonitorModStatusList.Columns.Item(4).HeaderText, , dgMonitorModStatusList.Columns.Item(5).HeaderText, dgMonitorModStatusList.Columns.Item(6).HeaderText,
              dgMonitorModStatusList.Columns.Item(7).HeaderText, dgMonitorModStatusList.Columns.Item(8).HeaderText, dgMonitorModStatusList.Columns.Item(9).HeaderText,
               dgMonitorModStatusList.Columns.Item(10).HeaderText, , dgMonitorModStatusList.Columns.Item(11).HeaderText, dgMonitorModStatusList.Columns.Item(13).HeaderText,
               dgMonitorModStatusList.Columns.Item(14).HeaderText, dgMonitorModStatusList.Columns.Item(15).HeaderText, dgMonitorModStatusList.Columns.Item(16).HeaderText,
                dgMonitorModStatusList.Columns.Item(17).HeaderText, , , , dgMonitorModStatusList.Columns.Item(18).HeaderText))
        Dim TotalCount1 As Integer
        TotalCount1 = Me.mComplyAssemblyMonitorModStatusList.Count
        Dim m As Integer

        For m = 0 To TotalCount1 - 1
            Dim str(13) As String
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

            If Me.dgMonitorModStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(0) = Me.dgMonitorModStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(1) = Me.dgMonitorModStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(2) = Me.dgMonitorModStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(3) = Me.dgMonitorModStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(4) = Me.dgMonitorModStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(5) = Me.dgMonitorModStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(6) = Me.dgMonitorModStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(7) = Me.dgMonitorModStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(13).Text <> "&nbsp;" Then str(8) = Me.dgMonitorModStatusList.Rows(m).Cells(13).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(14).Text <> "&nbsp;" Then str(9) = Me.dgMonitorModStatusList.Rows(m).Cells(14).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(15).Text <> "&nbsp;" Then str(10) = Me.dgMonitorModStatusList.Rows(m).Cells(15).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(16).Text <> "&nbsp;" Then str(11) = Me.dgMonitorModStatusList.Rows(m).Cells(16).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(17).Text <> "&nbsp;" Then str(12) = Me.dgMonitorModStatusList.Rows(m).Cells(17).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(18).Text <> "&nbsp;" Then str(13) = Me.dgMonitorModStatusList.Rows(m).Cells(18).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, ,
                   , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), , str(7), str(8), str(9),
                       str(10), str(11), str(12), , , , str(13)))
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite, "Assembly Directives Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "InstallAssemblyMonitorModStatusList", "Assembly Directives Status List Report", Util.ErrorType.NoError, Guid.Empty)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub hdnBtnModelModMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelModMaster.Click
        SetControlDirective()
        SetPageDirective()
        SetGridDirective()
        ControlVisibilityDirective()
        upnlDirective.Update()
    End Sub
#End Region

#Region "Parameters Tab"
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not IsValid Then Exit Sub
        Dim ParameterID As New Guid(cmbParameterList.SelectedValue.ToString)
        ''If mAssemblyParameterList.Contains(ParameterID) = False Then
        ''    MarkLog(Util.Action.[New], "Assembly", " Parameter ->  " + cmbParameterList.SelectedItem.Text, Util.ErrorType.NoError, ParameterID)
        ''    'mAssemblyParameter = AssemblyParameter.NewChildAssemblyParameter(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString))
        ''    mAssemblyParameterList.Add(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString))
        ''    dgParameterList.DataSource = mAssemblyParameterList
        ''    dgParameterList.DataBind()

        ''    Session("mAssemblyParameter") = mAssemblyParameter
        ''    Session("mAssemblyParameterList") = mAssemblyParameterList
        ''    'Response.Redirect("wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ''Else
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Parameter already exists, can not be added.", MsgBoxStyle.OKOnly)
        ''    '   msg.ReplacePage = "wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
        ''    msg.ReplacePage = "wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")

        ''    Session("sender") = "Delete"
        ''    msg.Show()
        ''End If

        If Session("mInstallAssemblyParametersEdit") = False Then
            If mAssemblyStatus.AssemblyParameters.Contains(ParameterID, mAssemblyStatus.AssemblyID) = False Then
                'Changed by Vikrant on 26-July-2011
                MarkLog(Util.Action.[New], "Assembly Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                'mAssemblyParameter = AssemblyParameter.NewChildAssemblyParameter(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString))
                mAssemblyStatus.AssemblyParameters.Add(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString), Val(txtMin.Text), Val(txtMax.Text)) '$$$$$$$$
                dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                dgParameterList.DataBind()
                Session("mAssemblyStatus") = mAssemblyStatus

                ''Session("mAssemblyParameter") = mAssemblyParameter
                ''Session("mAssemblyParameterList") = mAssemblyParameterList
                'Response.Redirect("wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Parameter already exists, can not be added.", MsgBoxStyle.OkOnly, "")
            End If

        Else
            mAssemblyStatus.AssemblyParameters.CurrentItem.MinValue = Val(txtMin.Text)
            mAssemblyStatus.AssemblyParameters.CurrentItem.MaxValue = Val(txtMax.Text)

            If mAssemblyStatus.AssemblyParameters.CurrentItem.IsDirty Then
                dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                dgParameterList.DataBind()
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mInstallAssemblyParametersEdit") = False
            End If
        End If
        mParameterList = ParameterList.GetParameterList("(SELECT)")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        cmbParameterList.DataBind()
        cmbParameterList.Enabled = True
        txtMin.Text = ""
        txtMax.Text = ""

    End Sub
    Private Sub hdnBtnParameter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnParameter.Click
        mParameterList = ParameterList.GetParameterList("(SELECT)")
        cmbParameterList.DataSource = mParameterList
        cmbParameterList.DataBind()
        upnlParamterCombo.Update()
    End Sub
    Private Sub dgParameterList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgParameterList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgParameterList.PageIndex * dgParameterList.PageSize
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteParameters")
                mAssemblyStatus.AssemblyParameters.CurrentIndex = Index
                Session("mAssemblyStatus") = mAssemblyStatus
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgParameterList.PageIndex * dgParameterList.PageSize
                Session("mInstallAssemblyParametersEdit") = True
                EditRecordParameters(Index)
        End Select
    End Sub
    Private Sub imgbtnParameter1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnParameter1.Click
        NewRecordParameter()
        Session("mAssemblyStatus") = mAssemblyStatus  '$$$$$$$$$
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenParameterWindow", "OpenParameterWindow()", True)
        'Response.Redirect("wfParameter_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
    End Sub
    Private Sub dgParameterList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgParameterList.Sorting
        mAssemblyStatus.AssemblyParameters.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyStatus") = mAssemblyStatus
        dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
        dgParameterList.DataBind()
    End Sub

#End Region

#End Region

#Region " Report "
    ' Creted  by - Rajnish on 23-09-2006
#Region " Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            'MarkLog(Util.Action.Print, "InstallAssembly", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crDetInstallRemoveAssembly
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList



        'For Installation Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 4
        RHCount1 = Me.mAssemblyStatus.AssemblyStatusPeriods.Count ' mInstallAssembly
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If
        ReportDetails.Add(New rptStatus(, 2, , , , , , lblInstallationInfo.InnerText, , , , , , , , , , , , , , "Note"))
        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 3, , , , "Aircraft",
                                             cmbMachineList.SelectedItem.Text, , , , , , , , , , , , , , , ,
                                             dgInstallationValue.Columns.Item(1).HeaderText, dgInstallationValue.Columns.Item(2).HeaderText,
                                          , dgInstallationValue.Columns.Item(3).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 3, , , , "Aircraft", cmbMachineList.SelectedItem.Text, , , , , , , , , , , , , , , , "", "", , ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , "Installed On",
                     txtInstalledOnDate.Text, , , , , , , , , , , , , , , ,
                             CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String),
                             , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , , , "Installed On",
                 txtInstalledOnDate.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , "Work Order No.",
                                           txtWorkOrNo.Text, , , , , , , , , , , , , , , ,
                                                  CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String),
                             , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , "Work Order No.", txtWorkOrNo.Text, , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , "Note", txtNote.Text, , , , , , , , , , , , , , , ,
                                                  CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String),
                             , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineInstallationValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , "Note", txtNote.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , "", "", "", "", , , , , , , , , , , , , , , ,
                                                 CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String),
                             , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineInstallationValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , "", "", "", "", , , , , , , , , , , , , , , , "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 4, , "", "", "", "", , , , , , , , , , , , , , , , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String),
                             , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineInstallationValueFormatted, String)))
            End If
        Next

        'For Model and Serial No. Info
        Dim LHCount As Integer
        LHCount = 8
        ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName))
        Dim I As Integer
        For I = 0 To LHCount - 1
            If I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, , "ATA Chapter", cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "", "", "", , ""))
            ElseIf I = 1 Then
                ReportDetails.Add(New rptStatus(, 1, , "Manufacturer", Trim(txtManufacturer.Text), , , , , , , , , , , , , , , , , "", "", "", , ""))
            ElseIf I = 2 Then
                ReportDetails.Add(New rptStatus(, 1, , "Model", cmbModelList.SelectedItem.Text, , , , , , , , , , , , , , , , , "", "", "", , ""))
            ElseIf I = 3 Then
                ReportDetails.Add(New rptStatus(, 1, , "Serial No.", Trim(txtSerialNo.Text), , , , , , , , , , , , , , , , , "", "", "", , ""))
            ElseIf I = 4 Then
                ReportDetails.Add(New rptStatus(, 1, , "Position", Trim(txtPosition.Text), , , , , , , , , , , , , , , , , "", "", "", , ""))
            ElseIf I = 5 Then
                ReportDetails.Add(New rptStatus(, 1, , "Installation Reason", Trim(txtInstallationReason.Text), , , , , , , , , , , , , , , , , "", "", "", , ""))
            ElseIf I = 6 Then
                ReportDetails.Add(New rptStatus(, 1, , "License No.", mAssemblyStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "", "", "", , ""))
            ElseIf I = 7 Then
                ReportDetails.Add(New rptStatus(, 1, , "Place", Trim(txtPlace.Text), , , , , , , , , , , , , , , , , "", "", "", , ""))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Install Assembly Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented For a wile MarkLog
        ' MarkLog(Util.Action.Print, "AssemblyInstall", mInstallAssemblyStatusInfo + " ->  Install Assembly Status Detail Report", Util.ErrorType.NoError, mInstallAssemblyStatus.ID)
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
#End Region


End Class