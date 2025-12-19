
'AJAX Created By: Saylee on 11-May-2015

Imports System.Linq
Imports System.Collections.Generic
Public Class wfCompMonitorInspStatus_AJAX
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
    Public mCompMonitorInspStatus As CompMonitorInspStatus
    Private Flag As Int16
    Public mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
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
    Public mIsSpareComp As Boolean 'Added by Shital on 30-Sep-2020 for All27072020
#End Region

#Region " Busines Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorInspStatus = CType(Session("mCompMonitorInspStatus"), CompMonitorInspStatus)
        mCompMonitorInspStatusList = CType(Session("mCompMonitorInspStatusList"), tmpCompMonitorInspStatusList)

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 13th-Oct-2009

        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")

        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareComp = Session("IsSpareComp") 'Added by Shital on 30-Sep-2020 for All27072020
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 13th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 13th-Oct-2009

        Session("mFileAttach") = mFileAttach 'Added By Prashant  On 27-Nov-2014
        Session("IsAttachmentDeleted") = IsAttachmentDeleted 'Added By Prashant  On 27-Nov-2014
    End Sub
    Private Sub GetAttachment()
        If mCompMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorInspStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCompMonitorInspStatus")

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
        'mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
        If mIsSpareComp = False Then 'If Condition Added by Shital for All27072020
            mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
        Else
            mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, Guid.Empty, mCompStatus.ID, 0)
        End If
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mCompMonitorInspStatus.IsNew
        btnSelect.Enabled = mCompMonitorInspStatus.IsNew
        dgCurrentValue.Columns(2).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 4)
        dgCurrentValue.Columns(3).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3)
        dgCurrentValue.Columns(4).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3)

        dgDoneOnValue.Columns(2).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 4)
        dgDoneOnValue.Columns(3).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 4)
        dgDoneOnValue.Columns(6).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3)
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        'dgDoneOnValue.Columns(7).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3)
        If mIsSpareComp = False Then 'mIsSpareComp Added By Shital On 1-OCt-2020 For ALL27072020
            dgDoneOnValue.Columns(7).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3)
            dgDoneOnValue.Columns(8).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3) AndAlso mAssemblyStatus.IsSpareAssembly = False AndAlso mIsSpareComp = False
        Else
            dgDoneOnValue.Columns(7).Visible = False
            dgDoneOnValue.Columns(8).Visible = False
        End If

        'Added By Saylee on 23-07-2008
        dgDoneOnValue.Columns(5).Visible = ((mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3) And (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 4))

        'If Not mCompMonitorInspStatus.EnableDoneOn Then   'previos condn of added code
        If mCompMonitorInspStatus.PartMonitorInsp.ID.Equals(Guid.Empty) Then   'Added Code
            calDoneOn.BackColor = Color.Gainsboro
            calDoneOn.Enabled = False               'Added Code 
            txtWorkOrderNo.BackColor = Color.Gainsboro
            txtWorkOrderNo.ReadOnly = True         'Added Rajnish on 22-12-2007
            txtRemark.BackColor = Color.Gainsboro
            txtRemark.ReadOnly = True               'Added Rajnish on 22-12-2007
        End If
        If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count > 1 Then     'Added By Prashant 17-Aug-2010
            chkIsLater.Enabled = True
        Else
            chkIsLater.Enabled = False
        End If

        ControlVisibilityForAttachment()
        'Commented by Rajnish on 22-12-2007
        'If mCompMonitorInspStatus.EnableDoneOn = False Then calDoneOn.Enabled = False 'Added Code
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
                        'Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
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
                            'Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
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
                        'Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                        'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                    ElseIf MSGBoxCtrl.Sender = "SaveWithDoneOnDate" Then
                        Session("sender") = ""
                        SetPage()
                        upnlTitle.Update()
                        upnlDoneOnValueGrid.Update()
                        upnlCurrentValueGrid.Update()
                        'Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    End If
                    'End
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    ControlVisibilityForDatePeriod()
                    'Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    ControlVisibilityForDatePeriod()
                    'Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetObject()
        With mCompMonitorInspStatus
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
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
                'Else
                '    .IsAttachmentAdded = False
            End If
        End With
    End Sub
    Public Sub SetGridObject()
        Dim txtElapsedValue, txtRemainingValue, txtDoneOnValue, txtDueOnValue, txtExtensionValue As TextBox
        With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
                txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3 Then
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
                If mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 4 Then
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
                'If mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3 Then
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
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
    End Sub
    Private Function Save() As Boolean
        Dim CompMonitorInspStatusClone As CompMonitorInspStatus
        CompMonitorInspStatusClone = CType(mCompMonitorInspStatus.Clone, CompMonitorInspStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 13th-Oct-2009
        If mCompMonitorInspStatus.IsValid = True Then
            If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count = 0 Then
                'MessageBox.Show("Component Insp Status can not be saved without period units.", "Comp Monitor Insp Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodUnitRequired, SIMsgBox.Message_text.PeriodUnitRequired, "You are trying to save Component Insp Status. Component Insp Status can not be saved without period units.", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Component Insp Status. Component Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If

            'Added By Vikrant On 06-Aug-2013 For ALL01082013
            If Not mCompMonitorInspStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorInspStatus.DoneOn.Equals(System.DBNull.Value) Then
                Dim title As String = "Save Alert !"
                Dim message As String = ""
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorInspStatus.DoneByID.ToString, mCompMonitorInspStatus.DoneOn)
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message))
                    Return False
                End If
            End If
            'End

            'aded By Deven on 24-Sep-2009 ------
            If Not Session("IsOpenFromMPD") = "True" Then 'Condition Added By Vikrant For MPD
                If mCompMonitorInspStatusList.Contains(mCompMonitorInspStatus.PartMonitorInspID) And mCompMonitorInspStatus.IsNew = True Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "Component Insp Status.", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Component Insp Status.", MsgBoxStyle.OkOnly, "")
                    Return False
                End If
            End If


            Try
                mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
                SaveMachineMaintenance()  'Added by Saylee on 13th-Oct-2009
                SaveAttachment()
                'Commented By Utkarsh On 28-Jul-2011 For All19072011

                '     MarkLog(Util.Action.Save, "CompMonitorSerStatus", " Part: " & mCompStatus.PartName & " Serial No.: " & mCompStatus.SerialNo, Util.ErrorType.NoError, mCompMonitorInspStatus.ID)

                'End

                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                Return True
            Catch ex As SqlException
                Session("CompMonitorInspStatusClone") = CompMonitorInspStatusClone
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
                CompMonitorInspStatusClone = Nothing

                'Added By Utkarsh On 28-Jul-2011 For All19072011
                'Added by Saylee on 10-Feb-2020,  All27072020
                Dim mRegNo As String = ""
                If mIsSpareComp = False Then   'Added by Shital on 05-Oct-2020,  All27072020
                    If mAssemblyStatus.IsSpareAssembly = False Then
                        mRegNo = "Reg No. : " & mMachine.RegNo
                    End If

                End If
                If mIsSpareComp = False Then   'Added by Shital on 05-Oct-2020,  All27072020
                    MaintDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
                Else
                    MaintDetail = " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
                End If

                MarkLog(Util.Action.Save, "Component Insp Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)

                'End

            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetPage()
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " SerialNo: " & mCompStatus.Comp.SerialNo & " ]"
        If mCompMonitorInspStatus.IsNew Then
            lblTitle.Text = "Component Inspection Status " & CompInfo & " [New]"
        Else
            lblTitle.Text = "Component Inspection Status" & CompInfo
        End If
    End Sub
    Public Function CheckPeriods() As Boolean 'Added by Saylee on 21-Aug-2008
        SetObject()
        SetGridObject()
        Dim mCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriod
        For Each mCompMonitorInspStatusPeriod In mCompMonitorInspStatus.CompMonitorInspStatusPeriods
            If Not mCompStatus.CompStatusPeriods.Contains(mCompMonitorInspStatusPeriod.PeriodID) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 13th-Oct-2009

        If Not (mMachineMaintenanceList.Contains(mCompMonitorInspStatus.ID, MaintenanceType.ComponentInspection, "")) Then
            'mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, MaintenanceType.ComponentInspection, calDoneOn.Text, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
            If mIsSpareComp = False Then 'mIsSpareComp Added By Shital On 1-OCt-2020 For ALL27072020
                mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, MaintenanceType.ComponentInspection, calDoneOn.Text, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
            Else
                mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(Guid.Empty, MaintenanceType.ComponentInspection, calDoneOn.Text, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, Guid.Empty)
            End If
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorInspStatus.ID, MaintenanceType.ComponentInspection)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mCompMonitorInspStatus.ID 'TransactionID
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
                ' mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mIsSpareComp = False Then
                    mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                Else
                    mMaxLogNo = MaxLogNo.GetMaxLogNo(calDoneOn.Text, Guid.Empty, Guid.Empty)
                End If
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    ' mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(calDoneOn.Text, mAssemblyStatus.MachineID)
                    If mIsSpareComp = False Then
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
        If mIsSpareComp = False Then 'If Condition added by shitalon 30-sep-2020 for ALL27072020

            If mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentInspectionPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentInspectionNew") Or User.IsInRole("MachineComponentInspectionEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                End If
            ElseIf Not mAssemblyStatus.IsMaster Then
                If (Not User.IsInRole("MachineComponentInspectionPrint")) Then
                    btnPrint.Enabled = False
                    btnPrint.ToolTip = "You are not authorized user"
                End If
                If (User.IsInRole("MachineComponentInspectionNew") Or User.IsInRole("MachineComponentInspectionEdit")) = False Then
                    btnSave.Enabled = False
                    btnSave.ToolTip = "You are not authorized user"
                End If
            End If

        End If
    End Sub
    'Added By Utkarsh On 17-May-2012 FOR ALL15052012
    Private Sub SetColor()
        If Not mCompMonitorInspStatus Is Nothing Then
            If mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And Not mCompMonitorInspStatus.DoneOn Is System.DBNull.Value Then
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
    'End
    Private Sub ControlVisibilityForAttachment()
        ' If mFileAttach.Size > 0 Then 'change from  to current condition
        If mCompMonitorInspStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                If mFileAttach.Size > 0 Then
                    Try
                        mFileAttach.Save()
                        'mEmployee.IsAttachmentAdded = True
                    Catch ex As Exception
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                    End Try
                Else
                    If (Not mCompMonitorInspStatus.IsNew) And IsAttachmentDeleted Then
                        FileAttach.DeleteAttachment(mFileAttach.ID, mCompMonitorInspStatus.ID)
                    End If
                    IsAttachmentDeleted = False
                    Session("IsAttachmentDeleted") = IsAttachmentDeleted
                End If
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
        If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDoneOnValue"), TextBox)
            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
        dgCurrentValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
        dgDoneOnValue.DataBind()



        'Added by Saylee on 13th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        BindLicenceNo() 'MLNo

        DataBind()
        calDoneOn.Text = mCompMonitorInspStatus.DoneOnFormatted.ToString


        'Added By Saylee on 23-07-2008=======================
        txtExtensionDate.Text = mCompMonitorInspStatus.ExtensionDateFormatted.ToString
        If Val(mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours + ")"
        End If

    End Sub
    Private Sub DataBindGrid()
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        dgCurrentValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
        If Not mCompMonitorInspStatus.IsValid Then
            For i As Integer = 0 To mCompMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentValue.Rows.Count - 1)
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)
            If Not mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
    Private Sub NewRecordInsp()
        Dim mCompMonitorInspStatus As CompMonitorInspStatus

        'mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mMachine.HourType)
        If mIsSpareComp = False Then 'If Condition Added by Shital for All27072020
            mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mMachine.HourType)
        Else
            mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, Guid.Empty, mCompStatus.AsOnDateFormatted.ToString, mCompStatus.Comp.PartID, mCompStatus.ModelID, mCompStatus.ID, mCompStatus.HourType)
        End If

        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Install Component Insp Status", "", Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
        'End
    End Sub
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
        For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
            txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
            mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
            Dim a As String = mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).AssemblyDueOnValueFormatted
        Next
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorInspStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorInspStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorInspStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Protected Sub txtRemaining_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemainingValue As TextBox
        For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
            txtRemainingValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtRemainingValue"), TextBox)

            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
                .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
                Dim a As String = .Item(i).AssemblyDueOnValueFormatted
            End With
        Next
        DataBindGrid()
        ControlVisibility()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCompMonitorInspStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    'Private Sub dgCurrentValue_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCurrentValue.ItemCommand
    '    Select Case e.CommandName
    '        Case "ElapsedValue"
    '            Dim txtElapsedValue As TextBox
    '            For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
    '                txtElapsedValue = CType(Me.dgCurrentValue.Rows(i).FindControl("txtElapsedValue"), TextBox)
    '                'If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).PeriodID = 2 Then
    '                '    mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).ElapsedValueFormatted = Trim(txtElapsedValue.Text)
    '                'Else
    '                mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
    '                'End If
    '            Next
    '            DataBindGrid()
    '        Case "RemainingValue"
    '            Dim txtRemainingValue As TextBox
    '            For j As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
    '                txtRemainingValue = CType(Me.dgCurrentValue.Rows(j).FindControl("txtRemainingValue"), TextBox)
    '                'If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(j).PeriodID = 2 Then
    '                '    mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(j).RemainingValueFormatted = Trim(txtRemainingValue.Text)
    '                'Else
    '                mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(j).RemainingValue = Trim(txtRemainingValue.Text)
    '                'End If
    '            Next
    '            DataBindGrid()
    '    End Select
    'End Sub
    Protected Sub txtDoneOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDoneOnValue As TextBox
        For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
            txtDoneOnValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtDoneOnValue"), TextBox)

            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub txtDueOnValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtDueOnValue As TextBox
        For j As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
            txtDueOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtDueOnValue"), TextBox)

            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
        For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
        '    msg.ReplacePage = "wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        '---------------------------------
        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If CheckPeriods() = False Then
                'Added By Utkarsh On 17-May-2012 FOR ALL15052012
                If mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And Not mCompMonitorInspStatus.DoneOn Is System.DBNull.Value Then
                    'Dim msg As New SIMsgBox(Page, "Save Alert !", "Component Insp Status is one time and you have entered Done On date.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo)
                    'msg.ReplacePage = "wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
                    'Session("sender") = "SaveWithDoneOnDate"
                    'msg.Show()
                    MSGBoxCtrl.show("Save Alert !", "Component Insp Status is one time and you have entered Done On date.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "SaveWithDoneOnDate")
                    Exit Sub
                End If
                'End
                If Save() = True Then
                    'Response.Redirect("wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    SetPage()
                    ControlVisibility()
                    upnlActionBtn.Update()
                    upnlMonitoringStatusDetails.Update()
                    upnlDoneOnValueGrid.Update()
                    upnlCurrentValueGrid.Update()
                    upnlDocument.Update()
                    upnlTitle.Update()
                    upnlMonitoringSelect.Update()
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    upnlValidationSummary.Update()
                End If
            Else
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodNotPresent, SIMsgBox.Message_text.PeriodNotPresent, "Period used to monitor this maintenance activity is not present in Component Status", MsgBoxStyle.OKOnly)
                'msg.ReplacePage = "wfCompMonitorInspStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4")
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
        Response.Redirect("wfPartMonitorInspList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=wfCompMonitorInspStatus_AJAX.aspx")
    End Sub
    Private Sub calDoneOn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDoneOn.TextChanged
        If IsPostBack Then      'Added Code on May,29,2007
            SetObject()
            DataBindGrid()
            SetColor() 'Added By Utkarsh On 17-May-2012 FOR ALL15052012
            upnlRedLabel.Update()
            upnlDoneOnValueGrid.Update()
            upnlCurrentValueGrid.Update()
        End If
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
        mCompMonitorInspStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        Dim mCompanyDetail As New CompanyDetail
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

        Rpt = New crDetComponentMonitorInspStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Insp Type", _
                  txtPartMonitorInspTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                    dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, _
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Insp Type", _
                            txtPartMonitorInspTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                        CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                        CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                        CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                        CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                             "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                   CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                   CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                   CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                   CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
               txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                    "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                      CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                      CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                      CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                      CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                 "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
     CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
     CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
     CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
     CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , _
     , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , _
     , "Please Note: Elapsed/Remaining values for Days/Months/Years will be in days"))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
                   New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , _
                    dgDoneOnValue.Columns.Item(8).HeaderText, , , , , , , , , "Component Values", _
                   dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, _
                 , dgDoneOnValue.Columns.Item(3).HeaderText, , dgDoneOnValue.Columns.Item(4).HeaderText, _
                  dgDoneOnValue.Columns.Item(5).HeaderText, dgDoneOnValue.Columns.Item(6).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
                            New SmartDate(calDoneOn.Text).FormattedText, , , , , , , , "", , , , , , , , , "Component Values", _
                                  "", "", , "", , "", ""))
        End If

        'LHData6:= CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame , String)

        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No. ", _
                    txtWorkOrderNo.Text, , , , , , , , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), _
                     ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.", _
                        txtWorkOrderNo.Text, , , , , , , , "", , , , , , , , , "Component Values", _
                        "", "", , "", , "", ""))
                End If

            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency", _
                    txtDoneBy.Text, , , , , , , , _
                     CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency", _
                        txtDoneBy.Text, , , , , , , , "", , , , , , , , , "Component Values", _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No. ", _
                    mCompMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No. ", _
                        mCompMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , "", , , , , , , , , "Component Values", _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place", _
                    txtPlace.Text, , , , , , , , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place", _
                        txtPlace.Text, , , , , , , , "", , , , , , , , , "Component Values", _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                    "", , , , , , , , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), _
                    "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                                          "", , , , , , , , "", , , , , , , , , "Component Values", _
                                                 "", "", , "", , "", "", , "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                    txtActualManHours.Text, , , , , , , , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                        txtActualManHours.Text, , , , , , , , "", , , , , , , , , "Component Values", _
                    "", "", , "", , "", ""))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                    txtRemark.Text, , , , , , , , _
                     CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                        txtRemark.Text, , , , , , , , "", , , , , , , , , "Component Values", _
                    "", "", , "", , "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                                   "", , , , , , , , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , , , , , , , , "Component Values", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), _
                      "Please Note: Started On/Current Values/Due On Values for Days/Months/Years will be in days"))
            End If
        Next


        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count
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
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Service", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Component Insp Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        'Commented By Utkarsh On 28-Jul-2011 For All19072011
        '       MarkLog(Util.Action.Print, "CompMonitorSerStatus", "Comp Insp Report", Util.ErrorType.NoError, Guid.Empty)
        'End

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mRegNo As String = ""
        If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020


            If mAssemblyStatus.IsSpareAssembly = False Then

                mRegNo = "Reg No. : " & mMachine.RegNo
            End If
        End If
        '*********************
        'Changed By Utkarsh On 28-Jul-2011 For All19072011
        If Not mCompMonitorInspStatus.IsNew Then
            'MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
            'MaintDetail = "Reg No. : " & mMachine.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName

            If mIsSpareComp = False Then  'if condition Added by Shital fro All27072020
                MaintDetail = mRegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
            Else
                MaintDetail = " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
            End If
            MarkLog(Util.Action.Close, "Component Insp Status", MaintDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
        Else
            MarkLog(Util.Action.Close, "Component Insp Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

        'End

        RemoveSession()
        If Not Session("URLForCompInst") Is Nothing Then
            Dim mtmpCompMonitorInspStatusList As tmpCompMonitorInspStatusList
            Dim mPartMonitorInspList As PartMonitorInspList

            Dim IsAllInspAddedForComp As Boolean = True

            mPartMonitorInspList = PartMonitorInspList.GetPartMonitorInspList(mCompStatus.Comp.PartID, Guid.Empty)
            mtmpCompMonitorInspStatusList = tmpCompMonitorInspStatusList.GetCompMonitorInspStatusList(mCompStatus.AsOnDate.ToString, mCompStatus.CompID, True, , , , , , , mAssemblyStatus.MachineID.ToString, mAssemblyStatus.AssemblyID.ToString) 'ture is for mCompStatus.IsMaster 

            For i As Integer = 0 To mPartMonitorInspList.Count - 1
                If Not mtmpCompMonitorInspStatusList.Contains(mPartMonitorInspList(i).ID) Then
                    IsAllInspAddedForComp = False
                    Exit For
                End If
            Next

            If Not Session("StatusPageOpenFrom") Is Nothing And Not IsAllInspAddedForComp Then
                NewRecordInsp()
                'Dim URLForPartInspList As Stack = CType(Session("URLForPartInspList"), Stack)


                'Dim UrlToSearch As String = URLForPartInspList.Peek.ToString
                'Dim GChildPage2 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage2=") + 12, UrlToSearch.IndexOf("&GChildPage4=") - UrlToSearch.IndexOf("GChildPage2=") - 12)
                'Dim GChildPage4 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage4=") + 12, UrlToSearch.IndexOf("&GChildPage5=") - UrlToSearch.IndexOf("GChildPage4=") - 12)
                'Dim GChildPage5 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage5=") + 12, UrlToSearch.IndexOf("&GChildPage6=") - UrlToSearch.IndexOf("GChildPage5=") - 12)
                'Dim GChildPage6 As String = UrlToSearch.Substring(UrlToSearch.IndexOf("GChildPage6=") + 12, UrlToSearch.Length - UrlToSearch.IndexOf("GChildPage6=") - 12)
                'Response.Redirect(URLForPartInspList.Peek.ToString)
                If Session("StatusPageOpenFrom") = "" Then 'Open From Comp Installation
                    Session.Remove("StatusPageOpenFrom")
                    Response.Redirect("wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfInstallComp_AJAX.aspx & &GChildPage5=wfInstallComp_AJAX.aspx")
                Else
                    If Session("StatusPageOpenFrom") = "wfAssemblyStatus_Ajax.aspx" Then 'Open From Comp Installation Aircarft Master
                        Session.Remove("StatusPageOpenFrom")
                        Response.Redirect("wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfAssemblyStatus_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                    Else 'Open From Assembly Installation In Maint
                        Session.Remove("StatusPageOpenFrom")
                        Response.Redirect("wfPartMonitorInspList_Ajax.aspx?GChildPage4=wfCompStatus_Ajax.aspx&GChildPage5=wfCompStatus_Ajax.aspx&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage6=wfAssemblyStatus_Ajax.aspx")
                    End If
                End If
            End If
            Dim URLForCompInst As Stack = CType(Session("URLForCompInst"), Stack)
            Session.Remove("URLForCompInst")
            Response.Redirect(URLForCompInst.Peek.ToString)
        End If
        Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompMonitorInspStatus.ID
            Session("MaintenanceDoneOnDate") = mCompMonitorInspStatus.DoneOn.ToString
            mMaintenanceDoneByEmployees = mCompMonitorInspStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompMonitorInspStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.Remove(mCompMonitorInspStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
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
            If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, MaintenanceType.ComponentInspection, DoneByID, LicenseNo, txtActualManHours.Text, EmpName)
            End If

        Else
            If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        BindLicenceNo()
        SetLicenceCount()
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtActualManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
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