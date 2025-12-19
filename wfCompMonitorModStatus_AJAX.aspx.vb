

'AJAX Created By: Saylee on 11-May-2015
Imports System.Linq

Public Class wfCompMonitorModStatus_AJAX
    Inherits System.Web.UI.Page

#Region "Enumeration"
    Private Enum MaintenanceType
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
    Public mCompStatus As CompStatus
    Public mCompMonitorModStatus As CompMonitorModStatus
    Private Flag As Int16
    Public mCompMonitorModStatusList As tmpCompMonitorModStatusList
    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 13th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 13th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Dim MaintDetail As String 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013

    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public mIsSpareComp As Boolean 'Added By Prashant 1-Oct-2020 for SpareComp All27072020
#End Region


#Region " Busines Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mCompMonitorModStatusList = CType(Session("mCompMonitorModStatusList"), tmpCompMonitorModStatusList)

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 13th-Oct-2009

        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")

        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareComp = Session("IsSpareComp") 'Added By Prashant 1-Oct-2020 for SpareComp
    End Sub
    Private Sub GetAttachment()
        If mCompMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 13th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 13th-Oct-2009

        Session("mFileAttach") = mFileAttach 'Added By Prashant  On 27-Nov-2014
        Session("IsAttachmentDeleted") = IsAttachmentDeleted 'Added By Prashant  On 27-Nov-2014
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCompMonitorModStatus")

        Session.Remove("mMachineMaintenance")       'Added by Saylee on 13th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 13th-Oct-2009

        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")

        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Sub NewRecord()
        If mIsSpareComp = False Then 'If Condition 'Added By Prashant 1-Oct-2020 for SpareComp
            mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, CStr(mAssemblyStatus.AsOnDate), mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
        Else
            Dim mModelList As ModelList
            mModelList = ModelList.GetModelList(1, , , , )
            mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty,
                                                                                  mCompStatus.AsOnDateFormatted, mCompStatus.Comp.PartID,
                                                                                 mModelList.Item(0).ID, mCompStatus.ID, mCompStatus.HourType)
        End If

        Session("mCompMonitorModStatus") = mCompMonitorModStatus
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mCompMonitorModStatus.IsNew
        btnSelect.Enabled = mCompMonitorModStatus.IsNew
        dgCurrentValue.Columns(2).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 4)
        dgCurrentValue.Columns(3).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)
        dgCurrentValue.Columns(4).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)

        dgDoneOnValue.Columns(2).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 4)
        dgDoneOnValue.Columns(3).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 4)
        dgDoneOnValue.Columns(6).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        'dgDoneOnValue.Columns(7).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)
        If mIsSpareComp = False Then 'mIsSpareComp 'Added By Prashant 1-Oct-2020 for SpareComp All27072020
            dgDoneOnValue.Columns(7).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3)
            dgDoneOnValue.Columns(8).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) AndAlso mAssemblyStatus.IsSpareAssembly = False AndAlso mIsSpareComp = False
        Else
            dgDoneOnValue.Columns(7).Visible = False  ' Added By Saylee On 27-Jul-2020 For ALL27072020
            dgDoneOnValue.Columns(8).Visible = False
        End If

        'End
        'Added By Saylee on 23-07-2008
        dgDoneOnValue.Columns(5).Visible = ((mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) And (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 4))

        'If Not mCompMonitorModStatus.EnableDoneOn Then   'previos condn of added code
        If mCompMonitorModStatus.PartMonitorMod.ID.Equals(Guid.Empty) Then   'Added Code
            calDoneOn.BackColor = Color.Gainsboro
            calDoneOn.Enabled = False               'Added Code 
            txtWorkOrderNo.BackColor = Color.Gainsboro
            txtWorkOrderNo.ReadOnly = True         'Added Rajnish on 22-12-2007
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True               'Added Rajnish on 22-12-2007
        End If
        If mCompMonitorModStatus.PartMonitorMod.IsApplicable = False Then calDoneOn.Enabled = False '22-12-2007
        If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If
        'Commented by Rajnish on 22-12-2007
        'If mCompMonitorModStatus.EnableDoneOn = False Then calDoneOn.Enabled = False 'Added Code

        ControlVisibilityForAttachment()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
        'Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        Save()
                        NewRecord()
                        DataFieldBind()
                        SetPage()
                        ControlVisibilityForDatePeriod()
                        upnlMonitoringStatusDetails.Update()
                        upnlDoneOnValueGrid.Update()
                        upnlCurrentValueGrid.Update()
                        upnlDocument.Update()
                        upnlTitle.Update()
                        'Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                        'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                    ElseIf MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("sender") = ""
                        If Save() = True Then
                            SetPage()
                            ControlVisibility()
                            upnlActionBtn.Update()
                            upnlMonitoringStatusDetails.Update()
                            upnlDoneOnValueGrid.Update()
                            upnlCurrentValueGrid.Update()
                            upnlDocument.Update()
                            upnlTitle.Update()
                            upnlMonitoringSelect.Update()
                            'Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                        End If
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        NewRecord()
                        DataFieldBind()
                        SetPage()
                        ControlVisibilityForDatePeriod()
                        upnlMonitoringStatusDetails.Update()
                        upnlDoneOnValueGrid.Update()
                        upnlCurrentValueGrid.Update()
                        upnlDocument.Update()
                        upnlTitle.Update()
                        'Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                        'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                    ElseIf MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("sender") = ""
                        'Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    End If
                    'End
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    ControlVisibilityForDatePeriod()
                    'Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    ControlVisibilityForDatePeriod()
                    'Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetObject()
        With mCompMonitorModStatus
            If calDoneOn.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = calDoneOn.Text
            End If
            .DoneWONo = txtWorkOrderNo.Text
            .DoneRemark = txtRemark.Text
            .SourceDoc = Trim(txtSourceDoc.Text)
            .RevisionNo = Trim(txtRevisionNo.Text)
            .BookNo = Trim(txtBookNo.Text)
            .PageNo = Trim(txtPageNo.Text)
            .RequiredManHours = Trim(txtActualManHours.Text)

            'Added By Saylee on 23-07-2008=======================
            'CNDC
            If txtExtensionDate.Text = "" Then
                .ExtensionDate = System.DBNull.Value
            Else
                .ExtensionDate = txtExtensionDate.Text
            End If

            .ApprovalRemark = Trim(txtApprovalRemark.Text)
            '====================================================
            .IsApplicable = chkApplicable.Checked   'Added By Saylee on 10-Sep-2008
            .DoneBy = Trim(txtDoneBy.Text)          'Added by Saylee On 23-Apr-2009
            .IsLater = chkIsLater.Checked           'Added By Prashant 17-Aug-2010
            .MethodOfCompliance = Trim(txtMethodOfCompliance.Text)  'Added By Saylee on 10-Oct-2024

            'Added By Prashant On 12-Jun-2012 FOR ALL08062012
            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .LicenseNo = LicenseNo
            .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            .Place = txtPlace.Text.Trim
            'End

            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    mCompMonitorModStatus.IsAttachmentAdded = True
                Else
                    mCompMonitorModStatus.IsAttachmentAdded = False
                End If
            End If


        End With
    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, txtDoneOnValue, txtDueOnValue, txtExtensionValue As TextBox
        With mCompMonitorModStatus.CompMonitorModStatusPeriods
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3 Then
                    'If .Item(i).PeriodID = 2 Then
                    '    .Item(i).ElapsedValueFormatted = Trim(txtElapsedValue.Text)
                    '    .Item(i).RemainingValueFormatted = Trim(txtRemainingValue.Text)
                    'Else
                    .Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
                    .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
                    'End If
                End If
            Next i
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtDoneOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)
                txtDueOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
                ''Added By Saylee on 23-07-2008
                txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 4 Then
                    If .Item(i).PeriodID = 2 Then
                        If Not Period.IsDate(txtDoneOnValue.Text.Trim) Then
                            .Item(i).DoneOnValueFormatted = ""
                        Else
                            .Item(i).DoneOnValueFormatted = Trim(txtDoneOnValue.Text)
                        End If
                    Else
                        .Item(i).DoneOnValue = Trim(txtDoneOnValue.Text)
                    End If
                End If

                ''Commented By Saylee on 23-07-2008
                'If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3 Then
                '    If .Item(i).PeriodID = 2 Then
                '        If Not Period.IsDate(txtDueOnValue.Text.Trim) Then
                '            .Item(i).DueOnValueFormatted = ""
                '        Else
                '            .Item(i).DueOnValueFormatted = txtDueOnValue.Text.Trim
                '        End If
                '    Else
                '        .Item(i).DueOnValue = txtDueOnValue.Text.Trim
                '    End If
                'End If
                'Added By Saylee on 23-07-2008
                'ExtensionValue
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            Next i
        End With
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
    End Sub
    Private Function Save() As Boolean
        Dim CompMonitorModStatusClone As CompMonitorModStatus
        CompMonitorModStatusClone = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 13th-Oct-2009
        If mCompMonitorModStatus.IsValid = True Then
            If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
                'MessageBox.Show("Component Mod Status can not be saved without period units.", "Comp Monitor Mod Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodUnitRequired, SIMsgBox.Message_text.PeriodUnitRequired, "You are trying to save Component Mod Status. Component Mod Status can not be saved without period units.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Component Mod Status. Component Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If

            'Added By Vikrant On 06-Aug-2013 For ALL01082013
            If Not mCompMonitorModStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorModStatus.DoneOn.Equals(System.DBNull.Value) Then
                Dim title As String = "Save Alert !"
                Dim message As String = ""
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorModStatus.DoneByID.ToString, mCompMonitorModStatus.DoneOn)
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message))
                    Return False
                End If
            End If
            'End

            'aded By Deven on 24-Sep-2009 ------
            If Not Session("IsOpenFromADSB") = "True" Then 'Added By Vikrant For ADSBConfig
                If mCompMonitorModStatusList.Contains(mCompMonitorModStatus.PartMonitorModID) And mCompMonitorModStatus.IsNew = True Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "Component Mod Status.", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Component Mod Status.", MsgBoxStyle.OkOnly, "")
                    Return False
                End If
            End If
            '-----------------------------------


            Try
                mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
                SaveMachineMaintenance()  'Added by Saylee on 13th-Oct-2009
                SaveAttachment()
                'Commented By Utkarsh On 28-Jul-2011 For All19072011

                '     MarkLog(Util.Action.Save, "CompMonitorSerStatus", " Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)

                'End

                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                Return True
            Catch ex As SqlException
                Session("CompMonitorModStatusClone") = CompMonitorModStatusClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                CompMonitorModStatusClone = Nothing

                'Added by Saylee on 10-Feb-2020,  All27072020
                Dim mRegNo As String = ""
                If mIsSpareComp = False Then   'Added by Shital on 05-Oct-2020,  All27072020
                    If mAssemblyStatus.IsSpareAssembly = False Then
                        mRegNo = "Reg No. : " & mMachine.RegNo
                    End If
                End If

                'Added By Utkarsh On 28-Jul-2011 For All19072011
                If mIsSpareComp = True Then 'Added By Prashant 1-Oct-2020 for SpareComp
                    MaintDetail = " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
                Else
                    MaintDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
                End If

                MarkLog(Util.Action.Save, "Component Mod Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)

                'End

            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetPage()
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " SerialNo: " & mCompStatus.Comp.SerialNo & " ]"
        If mCompMonitorModStatus.IsNew Then
            lblTitle.Text = "Component Modification Status " & CompInfo & " [New]"
        Else
            lblTitle.Text = "Component Modification Status" & CompInfo
        End If
    End Sub
    Public Function CheckPeriods() As Boolean 'Added by Saylee on 21-Aug-2008
        SetObject()
        SetGridObject()
        Dim mCompMonitorModStatusPeriod As CompMonitorModStatusPeriod
        For Each mCompMonitorModStatusPeriod In mCompMonitorModStatus.CompMonitorModStatusPeriods
            If Not mCompStatus.CompStatusPeriods.Contains(mCompMonitorModStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 13th-Oct-2009

        If Not (mMachineMaintenanceList.Contains(mCompMonitorModStatus.ID, MaintenanceType.ComponentModification, "")) Then
            If mIsSpareComp = False Then 'Added By Prashant 1-Oct-2020 for SpareComp All27072020
                mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, MaintenanceType.ComponentModification, calDoneOn.Text, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
            Else
                mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(Guid.Empty, MaintenanceType.ComponentModification, calDoneOn.Text, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, Guid.Empty)
            End If
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatus.ID, MaintenanceType.ComponentModification)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mCompMonitorModStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            If calDoneOn.Text <> "" Then
                .Date = calDoneOn.Text
            Else
                .Date = System.DBNull.Value
            End If

            '' Dim mLog As Log = CType(Session("mLog"), Log)
            Dim mLog As Log
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                If mIsSpareComp = False Then 'Added By Prashant 1-Oct-2020 for SpareComp All27072020
                    mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                Else
                    mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, Guid.Empty, Guid.Empty)
                End If

                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    If mIsSpareComp = False Then 'Added By Prashant 1-Oct-2020 for SpareComp All27072020
                        mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(calDoneOn.Text, mAssemblyStatus.MachineID)
                    Else
                        mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(calDoneOn.Text, Guid.Empty)
                    End If

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
        'Added by Saylee on 13th-Oct-2009
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        End If
        ''  End If
    End Sub
    Private Sub SetRights()
        If mIsSpareComp = False Then 'If Condition Added By Prashant 1-Oct-2020 for SpareComp All27072020
            If mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentModificationPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentModificationNew") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                End If
            ElseIf Not mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentModificationPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentModificationNew") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                End If
            End If
        End If
    End Sub
    'Added By Utkarsh On 17-May-2012 FOR ALL15052012
    Private Sub SetColor()
        If Not mCompMonitorModStatus Is Nothing Then
            If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And Not mCompMonitorModStatus.DoneOn Is System.DBNull.Value Then
                Dim txtdueOnValue As TextBox
                For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
                    txtdueOnValue = CType(dgDoneOnValue.Rows(i).FindControl("txtDueOnValue"), TextBox)
                    txtdueOnValue.BackColor = System.Drawing.Color.Red
                    txtdueOnValue.ForeColor = System.Drawing.Color.White
                Next
            End If
        End If
    End Sub
    'End

    'Added By Prashant On 27-Nov-2014
    Private Sub ControlVisibilityForAttachment()
        If mCompMonitorModStatus.IsAttachmentAdded = True Then 'change from  to current condition
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                    'mEmployee.IsAttachmentAdded = True
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mCompMonitorModStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCompMonitorModStatus.ID)
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
    'MLNo
    Public Sub SetLicenceCount()
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 And calDoneOn.Text <> "" Then
                    txtDnOnDate.Enabled = False
                Else
                    txtDnOnDate.Enabled = True
                End If
            End With
        Next j
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgDoneOnValue.DataBind()


        calDoneOn.Text = mCompMonitorModStatus.DoneOnFormatted.ToString


        'Added By Saylee on 23-07-2008=======================
        txtExtensionDate.Text = mCompMonitorModStatus.ExtensionDateFormatted.ToString

        'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        If Val(mCompMonitorModStatus.PartMonitorMod.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorModStatus.PartMonitorMod.RequiredManHours + ")"
        End If
        BindLicenceNo() 'MLNo
        DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        dgCurrentValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataBind()
        SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
        ControlVisibilityForDatePeriod()
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
            'Added By Prashant On 121-Jun-2012 FOR ALL08062012
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
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtElapsedValue As TextBox
        Dim txtRemainingValue As TextBox
        If Not mCompMonitorModStatus.IsValid Then
            For i As Integer = 0 To mCompMonitorModStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompMonitorModStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 28-Jul-2011 For All19072011
        If Not IsPostBack Then
            If btnSelect.Enabled = True Then
                setFocus(btnSelect)
            End If
            Session("mLogList") = Nothing
            DataFieldBind()
            ControlVisibility()
            ControlVisibilityForDatePeriod()
            SetRights()
            SetPage()
            SetColor()


            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If

    End Sub
    Protected Sub txtElapsedValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtElapsedValue As TextBox
        For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
            Dim a As String = mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).AssemblyDueOnValueFormatted
        Next
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtRemaining_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemainingValue As TextBox
        For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
                Dim a As String = .Item(i).AssemblyDueOnValueFormatted
            End With
        Next
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    'Private Sub dgCurrentValue_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCurrentValue.ItemCommand
    '    Select Case e.CommandName
    '        Case "ElapsedValue"
    '            Dim txtElapsedValue As TextBox
    '            For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
    '                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
    '                'If mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).PeriodID = 2 Then
    '                '    mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).ElapsedValueFormatted = Trim(txtElapsedValue.Text)
    '                'Else
    '                mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
    '                'End If
    '            Next
    '            DataBindGrid()
    '        Case "RemainingValue"
    '            Dim txtRemainingValue As TextBox
    '            For j As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
    '                txtRemainingValue = CType(Me.dgCurrentValue.Rows(j).FindControl("txtRemainingValue"), TextBox)
    '                'If mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(j).PeriodID = 2 Then
    '                '    mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(j).RemainingValueFormatted = Trim(txtRemainingValue.Text)
    '                'Else
    '                mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(j).RemainingValue = Trim(txtRemainingValue.Text)
    '                'End If
    '            Next
    '            DataBindGrid()
    '    End Select
    'End Sub
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDoneOnValue As TextBox
        For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            txtDoneOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)

            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Period.IsDate(txtDoneOnValue.Text.Trim) Then
                        .Item(i).DoneOnValueFormatted = Trim(txtDoneOnValue.Text)
                    Else
                        .Item(i).DoneOnValueFormatted = ""
                    End If
                Else
                    .Item(i).DoneOnValue = Trim(txtDoneOnValue.Text)
                End If
                Dim a As String = .Item(i).AssemblyDueOnValueFormatted
            End With
        Next
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtDueOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDueOnValue As TextBox
        For j As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)

            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Period.IsDate(txtDueOnValue.Text.Trim) Then
                        .Item(j).DueOnValueFormatted = txtDueOnValue.Text.Trim
                    Else
                        .Item(j).DueOnValueFormatted = ""
                    End If
                Else
                    .Item(j).DueOnValue = Trim(txtDueOnValue.Text)
                End If
                Dim a As String = .Item(j).AssemblyDueOnValueFormatted
            End With
        Next
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
                Dim a As String = .Item(i).AssemblyDueOnValueFormatted
            End With
        Next

        DataBindGrid()
        ControlVisibility()

        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Commented By Prashant 15-Mar-2011
        'If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
        '    SetObject()
        '    SetSession()
        '    MarkLog(Util.Action.Save, "CompMonitorSerStatus", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        '---------------------------------
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If CheckPeriods() = False Then
                'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                If mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And Not mCompMonitorModStatus.DoneOn Is System.DBNull.Value Then
                    'Dim msg As New SIMsgBox(Page, "Save Alert !", "Component Mod Status is one time and you have entered Done On date.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo)
                    'msg.ReplacePage = "wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                    'Session("sender") = "SaveWithDoneOnDate"
                    MSGBoxCtrl.Show("Save Alert !", "Component Mod Status is one time and you have entered Done On date.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    'msg.Show()
                    Exit Sub
                End If
                'End
                If Save() = True Then
                    'Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    SetPage()
                    ControlVisibility()
                    upnlActionBtn.Update()
                    upnlMonitoringStatusDetails.Update()
                    upnlDoneOnValueGrid.Update()
                    upnlCurrentValueGrid.Update()
                    upnlDocument.Update()
                    upnlTitle.Update()
                    upnlMonitoringSelect.Update()

                    'MLNo
                    Session.Remove("mMaintenanceDoneByEmployees")
                    Session.Remove("UserNameForLicenceList")
                    'End
                    ''  MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    'Added by Saylee on 28-Sep-2022 for Review Meeting
                    Dim mADSBConfiguration As ADSBConfiguration
                    mADSBConfiguration = Session("mADSBConfiguration")

                    If Not mADSBConfiguration Is Nothing Then
                        mADSBConfiguration.PartMonitorModID = mCompMonitorModStatus.PartMonitorMod.ID
                        mADSBConfiguration.CompStatusID = mCompStatus.ID
                        mADSBConfiguration.AssemblyStatusID = mAssemblyStatus.ID

                        Try
                            mADSBConfiguration.Save()
                            MSGBoxCtrl.Show("AD/SB Configuration..!!!", "SuccessFully Configured..!!!", "", MsgBoxStyle.OkOnly, "")
                        Catch ex As Exception
                            MSGBoxCtrl.Show("AD/SB Configuration..!!!", "Configuration Failed..!!!", "", MsgBoxStyle.OkOnly, "")
                        End Try
                    Else
                        MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                    End If
                    ''*****************************************************************

                Else
                    upnlValidationSummary.Update()
                End If
            Else
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodNotPresent, SIMsgBox.Message_text.PeriodNotPresent, "Period used to monitor this maintenance activity is not present in Component Status", MsgBoxStyle.OKOnly)
                'msg.ReplacePage = "wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                'Session("sender") = ""
                'msg.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodNotPresent, MSGBox.Message_text.PeriodNotPresent, "Period used to monitor this maintenance activity is not present in Component Status", MsgBoxStyle.OkOnly, "")
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        SetObject()
        SetGridObject()
        Response.Redirect("wfPartMonitorModList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=wfCompMonitorModStatus_AJAX.aspx")
    End Sub
    Private Sub calDoneOn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDoneOn.TextChanged
        If IsPostBack Then      'Added Code on May,29,2007
            SetObject()
            DataBindGrid()
            SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
            upnlDoneOnValueGrid.Update()
            upnlCurrentValueGrid.Update()
        End If
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        Dim mCompanyDetail As New CompanyDetail
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

        Rpt = New crDetComponentMonitorModStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Mod Type",
                  txtPartMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                    dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText,
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Mod Type",
                            txtPartMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter",
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                        CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                        CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                        CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                        CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter",
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                             "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference",
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                   CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                   CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                   CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                   CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference",
               txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                    "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description",
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                      CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                      CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                      CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                      CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description",
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
     CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
     CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
     CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
     CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String), ,
     , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , "", , , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String), ,
     , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
                   New SmartDate(calDoneOn.Text).FormattedText, , , , , , , ,
                   dgDoneOnValue.Columns.Item(8).HeaderText, , , , , , , , , "Component Values",
                   dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText,
                 , dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText,
                  dgDoneOnValue.Columns.Item(5).HeaderText, dgDoneOnValue.Columns.Item(6).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
                            New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , , , , , , , , , , "Component Values",
                                  "", "", , "", , "", ""))
        End If

        'LHData6:= CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame , String)

        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No. ",
                    txtWorkOrderNo.Text, , , , , , , ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String),
))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.",
                        txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values",
                        "", "", , "", , "", ""))
                End If

            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                    txtDoneBy.Text, , , , , , , ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                        txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No. ",
                    mCompMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No. ",
                        mCompMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                    txtPlace.Text, , , , , , , ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                        txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                    "", , , , , , , ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String),
                    "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                                          "", , , , , , , , , , , , , , , , , "Component Values",
                                                 "", "", , "", , "", "", , "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                    txtActualManHours.Text, , , , , , , ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                        txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                    txtRemark.Text, , , , , , , ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                        txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values",
                    "", "", , "", , "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                                   "", , , , , , , ,
                                   CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String),
                      "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
            End If
        Next

        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        If temp2 < RHCount2 Then
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.",
            txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
            dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, "Extension Date ",
            dgDoneOnValue.Columns.Item(2).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(3).HeaderText,
            dgDoneOnValue.Columns.Item(4).HeaderText, dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.",
                                txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                                      "", txtExtensionDate.Text, , "", , "", ""))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                    txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).FrequencyValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DoneOnValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "",
                "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Service",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DoneOnValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Component Mod Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        'Commented By Utkarsh On 28-Jul-2011 For All19072011
        '       MarkLog(Util.Action.Print, "CompMonitorSerStatus", "Comp Monitor Mod Report", Util.ErrorType.NoError, Guid.Empty)
        'End

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCompMonitorModStatus.IsAttachmentAdded = True
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
        mCompMonitorModStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorModStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorModStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added By Prashant 1-Oct-2020 for SpareComp All27072020
        Dim mRegNo As String = ""
        If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020
            If mAssemblyStatus.IsSpareAssembly = False Then
                mRegNo = "Reg No. : " & mMachine.RegNo
            End If
        End If
        'End of Added By Prashant 1-Oct-2020 for SpareComp All27072020
        'Changed By Utkarsh On 28-Jul-2011 For All19072011
        If Not mCompMonitorModStatus.IsNew Then
            'MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
            'MaintDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
            If mIsSpareComp = False Then  'if condition Added By Prashant 1-Oct-2020 for SpareComp All27072020
                MaintDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
            Else
                MaintDetail = " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
            End If
            MarkLog(Util.Action.Close, "Component Mod Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
        Else
            MarkLog(Util.Action.Close, "Component Mod Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

        'End

        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompMonitorModStatus.ID
            Session("MaintenanceDoneOnDate") = mCompMonitorModStatus.DoneOn.ToString
            mMaintenanceDoneByEmployees = mCompMonitorModStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompMonitorModStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Remove(mCompMonitorModStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
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
            If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, MaintenanceType.ComponentModification, DoneByID, LicenseNo, txtActualManHours.Text, EmpName)
            End If

        Else
            If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        BindLicenceNo()
        SetLicenceCount()
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtActualManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mCompMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
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