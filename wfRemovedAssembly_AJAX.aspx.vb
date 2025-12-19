

'AJAX Conversion By: Saylee on 17-Mar-2015 : ModuleID:302


Imports System.Linq
Imports System.Collections.Generic
Imports System.Text

Public Class wfRemovedAssembly_AJAX
    Inherits System.Web.UI.Page


#Region " Enum "
    Public Enum From
        NewRemove = 1
        EditRemove = 2
    End Enum
#End Region

#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mPrevAssemblyStatus As AssemblyStatus
    Public mRemovalReasonList As RemovalReasonList
    Public mMachine As Machine
    Public mRemovedOn As String
    Public mFrom As From
    Public Flag As Boolean = False

    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 8th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 8th-Oct-2009

    Dim EventLogID As Guid  'Added by Vikrant on 26-July-2011
    Dim mAssemblyDetail As String
    Public mEmployeeList As EmployeeList
    Public mEmployeeStatus As EmployeeStatus 'Added By Shweta On 07-Aug-2013 For ALL01082013
    'Added By Vikrant On 01-Dec-2014
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
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mPrevAssemblyStatus = CType(Session("mPrevAssemblyStatus"), AssemblyStatus)
        mRemovalReasonList = CType(Session("mRemovalReasonList"), RemovalReasonList)
        mMachine = CType(Session("mMachine"), Machine)
        mFrom = CType(Session("From"), From)

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 8th-Oct-2009
        'Added By Vikrant On 01-Dec-2014
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
    End Sub
    Private Sub setSession()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mRemovalReasonList") = mRemovalReasonList
        Session("mPrevAssemblyStatus") = mPrevAssemblyStatus
        Session("mMachine") = mMachine
        Session("From") = mFrom

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 8th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList            'Added by Saylee on 8th-Oct-2009
        'Added By Vikrant On 01-Dec-2014
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRemovalReasonList")

        Session.Remove("mMachineMaintenance")       'Added by Saylee on 8th-Oct-2009
        Session.Remove("mMachineMaintenanceList")       'Added by Saylee on 8th-Oct-2009
        'Added By Vikrant On 01-Dec-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetObject()
        With mAssemblyStatus
            .RemovalReasonID = New Guid(cmbReason.SelectedValue)
            .RemovalReasonName = cmbReason.SelectedItem.Text
            .Assembly.SerialNo = Trim(txtSerialNo.Text)
            .Position = Trim(txtPosition.Text)
            'If Not (calStartDate.IsDateValue) Then
            '    .RemovedOn = System.DBNull.Value
            'Else
            '    .RemovedOn = calStartDate.Value.ToString
            'End If
            If calStartDate.Text <> "" Then
                .RemovedOn = calStartDate.Text
            Else
                .RemovedOn = System.DBNull.Value
            End If
            .RemovalWONO = Trim(txtWorkOrderNo.Text)
            .RemovalRemark = Trim(txtNote.Text)
            'Added By Vikrant On 12-Jun-2012 FOR ALL08062012
            '.RemDoneByID = New Guid(cmbRemovedBy.SelectedValue)
            '.RemLicenseNo = txtLicenceNo.Text.Trim
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

            .RemPlace = txtPlace.Text.Trim
            .IsRemUnschedule = chkIsRemUnscheduled.Checked 'Added By Vikrant On 22-Aug-2012 For ALL20082012
            'Added By Vikrant On 01-Dec-2014
            If mFileAttach.Size > 0 Then
                .IsAttachmentAdded = True
            Else
                .IsAttachmentAdded = False
            End If
            'End
        End With
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub SetPage()
        lbltitle.Text = "Remove " & mAssemblyStatus.AssemblyTypeName & " of aircraft " & mMachine.RegNo
        lblEngineInfo.InnerText = "Model & Serial No. of the " & mAssemblyStatus.AssemblyTypeName

        lblRemovalInfo.InnerText = "Removal Information of the " & mAssemblyStatus.AssemblyTypeName

        If Session("From") = 2 Then
            btnTechDirection.Enabled = True
            lnkPrintLogBookEntry.Enabled = True 'Added By Prashant 7-May-20201 ALL07052021
        Else
            If Session("Saved") = True Then
                btnTechDirection.Enabled = True
                lnkPrintLogBookEntry.Enabled = True 'Added By Prashant 7-May-20201 ALL07052021
            Else
                btnTechDirection.Enabled = False
                lnkPrintLogBookEntry.Enabled = False  'Added By Prashant 7-May-20201 ALL07052021
            End If
        End If
    End Sub
    Private Function Save() As Boolean
        Dim AssemblyStatusClone As AssemblyStatus
        AssemblyStatusClone = CType(mAssemblyStatus.Clone, AssemblyStatus)
        SetObject()
        SetMachineMaintenanceObject()  'Added by Saylee on 8th-Oct-2009
        If mAssemblyStatus.IsValid = True Then
            Try
                'Added By Shweta On 07-Aug-2013 For ALL01082013
                If Not mAssemblyStatus.RemDoneByID.Equals(Guid.Empty) AndAlso Not mAssemblyStatus.RemovedOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyStatus.RemDoneByID.ToString, mAssemblyStatus.RemovedOn.ToString)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                        Return False
                    End If
                End If
                'End
                mAssemblyStatus.ApplyEdit()
                mAssemblyStatus = CType(mAssemblyStatus.Save(), AssemblyStatus)
                SaveAttachment() 'Added By Vikrant On 01-Dec-2014
                SaveMachineMaintenance()  'Added by Saylee on 8th-Oct-2009
                Session("mAssemblyStatus") = mAssemblyStatus
                Return True
            Catch ex As SqlException
                Session("AssemblyStatusClone") = AssemblyStatusClone
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
                AssemblyStatusClone = Nothing
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
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    upnlEngineInfo.Update()
                    upnlRemovalInfo.Update()
                    'Response.Redirect("wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
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
    Private Sub SetFromClone(ByVal clnAssemblyStatus As AssemblyStatus)
        mAssemblyStatus.RemovedOn = clnAssemblyStatus.RemovedOn
        mAssemblyStatus.RemovalReasonID = clnAssemblyStatus.RemovalReasonID
        mAssemblyStatus.RemovalRemark = clnAssemblyStatus.RemovalRemark
        mAssemblyStatus.RemDoneByID = clnAssemblyStatus.RemDoneByID
        mAssemblyStatus.RemLicenseNo = clnAssemblyStatus.RemLicenseNo
        mAssemblyStatus.RemPlace = clnAssemblyStatus.RemPlace
    End Sub

    Private Sub SetMachineMaintenanceObject()

        'Added by Saylee on 8th-Oct-2009
        If mFrom = From.NewRemove And Not (mMachineMaintenanceList.Contains(mAssemblyStatus.ID, 2, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 2, calStartDate.Text, mAssemblyStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else ''If mFrom = From.EditRemove Then
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyStatus.ID, 2)
            Session("mMachineMaintenance") = mMachineMaintenance
        End If

        With mMachineMaintenance
            .MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID = 2
            .MaintenanceID = mAssemblyStatus.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatus.ID

            .Date = calStartDate.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo(calStartDate.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
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
    'Added By Vikrant On 01-Dec-2014
    Private Sub ControlVisibilityForAttachment()
        If mFileAttach.Size > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyStatus.ID, 2) 'Sort = 2 : Removal
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '
        mFileAttach.ReferenceID = mAssemblyStatus.ID
        If mFileAttach.Size > 0 Then
            Try
                mFileAttach.Save()
                'mFileAttach = Nothing
                'Session("mFileAttach") = mFileAttach
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), False)
            End Try
        Else
            If (Not mAssemblyStatus.IsNew) And IsAttachmentDeleted Then
                FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyStatus.ID, 2)
            End If
            IsAttachmentDeleted = False
            Session("IsAttachmentDeleted") = IsAttachmentDeleted
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
        dgRemovalValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods

        'Added on 28-05-2007 by Saylee
        calStartDate.Text = mAssemblyStatus.RemovedOnFormatted

        'Added by Saylee on 8th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        '====================================================================
        BindLicenceNo() 'MLNo
        DataBind()

        '=============Added by Saylee on 11th-Jan-2008 for bug-RA7 (Maintenance)==============================
        If cmbReason.Items.Contains(New System.Web.UI.WebControls.ListItem(mAssemblyStatus.RemovalReasonName, mAssemblyStatus.RemovalReasonID.ToString)) Then
            cmbReason.SelectedValue = mAssemblyStatus.RemovalReasonID.ToString
        Else
            cmbReason.SelectedValue = Guid.Empty.ToString
        End If
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyStatus") = mAssemblyStatus
        mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
        mAssemblyStatus.BeginEdit()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 200 Then
                custValidator.ErrorMessage = "Max. length of Note should be 200 char"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Vikrant On 12-Jun-2012 FOR ALL08062012
        ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End
            'Commented by Amrita on 8-Jan-08 for Solving Bug No : - RA6 given by Pramod
            'ElseIf custValidator.ControlToValidate = "cmbReason" Then
            '    If cmbReason.SelectedIndex <= 0 Then
            '        custValidator.ErrorMessage = "Please select Reason from the list"
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        End If
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetObject()
        Dim str As String = ""
        If Not mAssemblyStatus.IsValid Then
            For i As Integer = 0 To mAssemblyStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgRemovalValue.Rows.Count - 1)
            If Not mAssemblyStatus.AssemblyStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        If Not IsPostBack And MSGBoxCtrl.Sender = "" Then
            setFocus(txtWorkOrderNo)
            DataFieldBind()
            GetAttachment()
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If
        SetPage()
        ControlVisibilityForAttachment() 'Added by Vikrant On 02-Dec-2014
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("AssemblyRemovalNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyRemovalEdit") And Not mAssemblyStatus.IsNew) Then
            SetObject()
            setSession()
            'Changed by Vikrant on 26-July-2011
            mAssemblyDetail = "Reg No. : " & mMachine.RegNo & " Model : " & mAssemblyStatus.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo
            MarkLog(Util.Action.Save, "AssemblyRemoval", User.Identity.Name & " is not Authorized User to save" & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
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
                SetPage()
                ControlVisibilityForAttachment()
                upnlEngineInfo.Update()
                upnlRemovalInfo.Update()
                mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
                Session("mMachineMaintenanceList") = mMachineMaintenanceList
                Try
                    'Changed by Vikrant on 26-July-2011
                    mAssemblyDetail = "Reg No. : " & mMachine.RegNo & " Model : " & mAssemblyStatus.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo & " Removed On : " & calStartDate.Text & " Removal Reason : " & cmbReason.SelectedItem.Text
                    MarkLog(Util.Action.Save, "AssemblyRemoval", mAssemblyDetail, Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
                Catch ex As Exception
                    '
                End Try
                'Response.Redirect("wfRemovedAssembly.aspx?BackPage=" & Request.QueryString("BackPage"))
            End If
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If

    End Sub
    '  Private Sub imgbtnReason_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnReason.Click
    Private Sub imgReason_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReason.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemovalReasonWindow", "OpenRemovalReasonWindow()", True)
        'Response.Redirect("wfRemovalReason_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfRemovedAssembly_Ajax.aspx&Type=" & mAssemblyStatus.AssemblyTypeID)
    End Sub
    Private Sub calStartDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calStartDate.TextChanged
        Try
            If Not mAssemblyStatus Is Nothing Then
                If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyStatus.RemovedOn.ToString), SmartDate.StringToDate(calStartDate.Text)) <> 0 Then
                    SetObject()
                    setSession()
                    REM: Clone the object
                    Dim clnAssemblyStatus As AssemblyStatus
                    clnAssemblyStatus = CType(mAssemblyStatus.Clone, AssemblyStatus)

                    EditRecord(calStartDate.Text)

                    REM: Copy from Clone
                    CopyFromClone(clnAssemblyStatus, IIf(mFrom = From.NewRemove, True, False))
                    DataFieldBind()

                    Session("mAssemblyStatus") = mAssemblyStatus
                End If
            End If
        Catch ex As Exception

        Finally
        End Try
    End Sub

    Private Sub EditRecord(ByVal RemovedOn As String)
        REM:-if we r removing the assembly
        If mFrom = From.NewRemove Then
            mAssemblyStatus = AssemblyStatus.NewRemovalAssemblyStatus(mPrevAssemblyStatus.ID, RemovedOn)
        Else
            REM:-If we r editing the removal details of an assembly.
            mAssemblyStatus = AssemblyStatus.GetRemovalAssemblyStatus(mPrevAssemblyStatus.ID, RemovedOn)
        End If
        mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
        mAssemblyStatus.BeginEdit()
    End Sub
    REM:-Restore the values of the variable.
    Private Sub CopyFromClone(ByVal ClonedAssemblyStatus As AssemblyStatus, ByVal IsNewInstallation As Boolean)
        mAssemblyStatus.RemovalWONO = ClonedAssemblyStatus.RemovalWONO
        mAssemblyStatus.RemovalReasonID = ClonedAssemblyStatus.RemovalReasonID
        mAssemblyStatus.RemovalReasonName = ClonedAssemblyStatus.RemovalReasonName
        mAssemblyStatus.RemovalRemark = ClonedAssemblyStatus.RemovalRemark
        mAssemblyStatus.RemDoneByID = ClonedAssemblyStatus.RemDoneByID
        mAssemblyStatus.RemLicenseNo = ClonedAssemblyStatus.RemLicenseNo
        mAssemblyStatus.RemPlace = ClonedAssemblyStatus.RemPlace
        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In ClonedAssemblyStatus.MaintenanceDoneByEmployees
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

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Changed by Vikrant on 26-July-2011
        MarkLog(Util.Action.Close, "AssemblyRemoval", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session.Remove("Saved")
        'Added By Saylee On 27-Nov-2014 
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    'Added By Saylee On 27-Nov-2014 
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
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
        Dim mtechDirection As rptTechDirection = rptTechDirection.GetTechDirection(mAssemblyStatus.ID, 1, mAssemblyStatus.RemovedOn.ToString) '2 for Assembly
        If mtechDirection.IsNew Then 'there is no entry for current component.
            mtechDirection = rptTechDirection.NewTechDirection(mAssemblyStatus.ID, 1, mAssemblyStatus.RemovedOn.ToString)
        End If
        If mAssemblyStatus.RemovalReasonName = "(SELECT)" Then
            mtechDirection.RemovalReason = ""
        Else
            mtechDirection.RemovalReason = mAssemblyStatus.RemovalReasonName
        End If
        ' mtechDirection.Date = mAssemblyStatus.RemovedOn 'Commented by Saylee on 27-Mar-2017 as date should be TDdate and not Removal date
        mtechDirection.RemovalDate = mAssemblyStatus.RemovedOn
        mtechDirection.Position = mAssemblyStatus.Position 'Added By Prashant 3-Jun-2022
        'Dim mMachineinfo As ListOfAircraftCurrentStatus = ListOfAircraftCurrentStatus.GetListOfAircraftCurrentStatus("", mMachine.RegNo, "", "", "", mAssemblyStatus.RemovedOn.ToString)
        Dim mAssemblyList As AssemblyList = AssemblyList.GetAssemblyList(1, mAssemblyStatus.MachineID.ToString, calStartDate.Text)
        mtechDirection.ATA = mAssemblyStatus.ATAChapter
        mtechDirection.PartNo = mAssemblyStatus.ModelName
        mtechDirection.Description = ""
        mtechDirection.SerialNo = mAssemblyStatus.Assembly.SerialNo
        mtechDirection.ModelName = mAssemblyList(0).ModelName 'mMachineinfo(0).ModelName
        mtechDirection.AircaftName = mMachine.RegNo
        mtechDirection.AircaftSrNo = mAssemblyList(0).SerialNo
        mtechDirection.IsRemUnschedule = mAssemblyStatus.IsRemUnschedule
        'mtechDirection.TimeSinceNew = String.Join(", ", From c In mAssemblyStatus.AssemblyStatusPeriods Select c.AssemblyRemovalValueFormatted)
        mtechDirection.TimeSinceNew = String.Join(", ", From c As AssemblyStatusPeriod In mAssemblyStatus.AssemblyStatusPeriods Select New Period(c.PeriodID, c.AssemblyRemovalValue, GetPeriodUnitID(c.PeriodID), CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)
        'mtechDirection.TimeSinceOverhaul = String.Join(", ", From c As AssemblyStatusPeriod In mAssemblyStatus.AssemblyStatusPeriods Select New Period(c.PeriodID, c.AssemblyRemovalValue, 0, CBool(IIf(c.PeriodID = 2, True, False)), False, c.HourType).TextFormatted)
        Session("mrptTechDirection") = mtechDirection

        'Added By Saylee on 10-July-2015
        mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyStatus.ID, 2)
        Session("TechLog") = mMachineMaintenance.LogID.ToString
        '******************************
        Response.Redirect("wfTechDirection.aspx?BackPage=wfRemovedAssembly_Ajax.aspx&BackPage1=" & Request.QueryString("BackPage"))
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnRemovalReason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemovalReason.Click
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        cmbReason.DataSource = mRemovalReasonList
        cmbReason.DataBind()
        Session("mRemovalReasonList") = mRemovalReasonList
        If Not mAssemblyStatus.RemovalReasonID.Equals(Guid.Empty) Then
            cmbReason.SelectedValue = mAssemblyStatus.RemovalReasonID.ToString
        End If
        upnlRemovalInfo.Update()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mAssemblyStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyStatus.RemovedOn.ToString
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
                mAssemblyStatus.MaintenanceDoneByEmployees.Add(mAssemblyStatus.ID, 2, DoneByID, LicenseNo, "", EmpName)
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

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mAssemblyStatus.RemovedOn, mAssemblyStatus.RemovedOn, "", mAssemblyStatus.AssemblyTypeName, _
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "", _
                                                        mAssemblyStatus.MachineID.ToString, True, True, IsRemoved:=True, IsInstalled:=False, _
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True, _
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False, _
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mAssemblyStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "OpenFromAssemblyRemovalInstallationComponentRemovalInstallation", mAssemblyStatus.RemovedOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo, _
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo, _
           IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper), _
           AppSettings("Product Version"), AppSettings("SINote"), _
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.", _
           "True", mAssemblyStatus.RemovedOnFormatted, "", AppSettings("Logo"))

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

#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
#End Region


#Region " Event "

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("AssemblyRemovalPrint")) Then
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim Rpt As New crDetInstallRemoveAssembly
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Model and Serial No. Info
        Dim LHCount As Integer
        LHCount = 5

        ReportDetails.Add(New rptStatus(, 0, lblEngineInfo.InnerText))
        Dim I As Integer
        For I = 0 To LHCount - 1
            If I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, , lblATAChapter.Text, _
    txtATAChapter.Text, , , , , , , , , , , , , , , , , "", _
    "", "", , "", , , , , ))
            ElseIf I = 1 Then
                ReportDetails.Add(New rptStatus(, 1, , lblManufacturer.Text, _
    txtManufacturer.Text, , , , , , , , , , , , , , , , , "", _
    "", "", , "", , , , , ))
            ElseIf I = 2 Then
                ReportDetails.Add(New rptStatus(, 1, , lblModel.Text, _
     txtModel.Text, , , , , , , , , , , , , , , , , "", _
      "", "", , "", , , , , ))
            ElseIf I = 3 Then
                ReportDetails.Add(New rptStatus(, 1, , lblSerialNo.Text, _
   txtSerialNo.Text, , , , , , , , , , , , , , , , , "", _
   "", "", , "", , , , , ))
            ElseIf I = 4 Then
                ReportDetails.Add(New rptStatus(, 1, , lblPosition.Text, _
     txtPosition.Text, , , , , , , , , , , , , , , , , "", _
    "", "", , "", , , , , ))
            End If
        Next

        'For Removal Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 4
        RHCount1 = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        ReportDetails.Add(New rptStatus(, 2, , , , , , lblRemovalInfo.InnerText, , , , , , , , , , , , , , lblValuesAtRemoval.Text))
        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 3, , , , lblRemovedOn.Text, _
                                             New SmartDate(calStartDate.Text).FormattedText, , , , , , , , , , , , , , , , _
                                              dgRemovalValue.Columns.Item(0).HeaderText, dgRemovalValue.Columns.Item(1).HeaderText, _
                                          , dgRemovalValue.Columns.Item(2).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 3, , , , lblRemovedOn.Text, _
                            New SmartDate(calStartDate.Text).FormattedText, , , , , , , , , , , , , , , , "", "", , ""))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text, _
                      txtWorkOrderNo.Text, , , , , , , , , , , , , , , , _
                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyRemovalValueFormatted, String), _
                             , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineRemovalValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , , , lblWorkOrderNo.Text, _
                  txtWorkOrderNo.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 1 Then
                Dim mReasonOfRemoval As String = ""
                If cmbReason.SelectedIndex > 0 Then
                    mReasonOfRemoval = cmbReason.SelectedItem.Text
                End If
                If m < RHCount1 Then

                    ReportDetails.Add(New rptStatus(, 4, , , , "Reason :", _
                                           mReasonOfRemoval, , , , , , , , , , , , , , , , _
                                                  CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyRemovalValueFormatted, String), _
                                                  , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineRemovalValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , "Reason :", _
                                    mReasonOfRemoval, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text, _
                                       txtNote.Text, , , , , , , , , , , , , , , , _
                                                 CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyRemovalValueFormatted, String), _
                                                  , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineRemovalValueFormatted, String)))
                Else

                    ReportDetails.Add(New rptStatus(, 4, , , , lblNote.Text, _
                                    txtNote.Text, , , , , , , , , , , , , , , , "", "", , ""))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 4, , "", _
                                          "", "", "", , , , , , , , , , , , , , , , _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyRemovalValueFormatted, String), _
                                                  , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineRemovalValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 4, , "", _
                                          "", "", "", , , , , , , , , , , , , , , , "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 4, , "", _
                                           "", "", "", , , , , , , , , , , , , , , , _
                                                   CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyRemovalValueFormatted, String), _
                                                   , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).MachineRemovalValueFormatted, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Remove Assembly Status Detail Report", lbltitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "AssemblyRemoval", "Remove Assembly Report", Util.ErrorType.NoError, Guid.Empty)
        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
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