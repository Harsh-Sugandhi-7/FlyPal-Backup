

'Created by : Saylee
'Dated      : 20-Sep-2022


Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class wfADSBMonitoring
    Inherits System.Web.UI.Page


#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum

#End Region

#Region "Variables and Declarations"
    Public mADSBTechRecording As ADSBTechRecording
    Public mADSBMonitoring As ADSBMonitoring


    Dim mUser As User
    Dim mMachine As Machine
    Public mAssemblyStatusModelWise As AssemblyStatusModelWise
    Public mMachineNameValueList As MachineNameValueList
    Public mAssemblyStatus As AssemblyStatus
    Public mModelMonitorMod As ModelMonitorMod
    Public mPartMonitorMod As PartMonitorMod
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mCompMonitorModStatus As CompMonitorModStatus
    Public mADSBConfiguration As ADSBConfiguration
    Public mCompStatus As CompStatus
#End Region

#Region "Helper Methods"

    Private Sub addAttributes()
        txtADSBTechRecordingText.Attributes.Add("onblur", "WaterMark(this, event);")
        txtADSBTechRecordingText.Attributes.Add("onfocus", "WaterMark(this, event);")
    End Sub
    Private Sub GetSession()
        mADSBTechRecording = Session("mADSBTechRecording")
        mADSBMonitoring = Session("mADSBMonitoring")
        mAssemblyStatusModelWise = Session("mAssemblyStatusModelWise")
        mMachineNameValueList = Session("mMachineNameValueList")
        mAssemblyStatus = Session("mAssemblyStatus")
        mModelMonitorMod = Session("mModelMonitorMod")
        mAssemblyMonitorModStatus = Session("mAssemblyMonitorModStatus")
        mADSBConfiguration = Session("mADSBConfiguration")

        mPartMonitorMod = Session("mPartMonitorMod")
        mCompMonitorModStatus = Session("mCompMonitorModStatus")
        mCompStatus = Session("mCompStatus")
    End Sub

    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "ADSBMonitoring"
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Sub setObject()
       
        mADSBMonitoring.EONO = txtEONo.Text.Trim
        If txtEODate.Text.ToString <> "" Then
            mADSBMonitoring.EODate = CDate(txtEODate.Text)
        Else
            mADSBMonitoring.EODate = System.DBNull.Value
        End If
        mADSBMonitoring.FacilityName = txtFacility.Text.Trim
        mADSBMonitoring.Location = txtLocation.Text.Trim

        If txtPlannedDate.Text.ToString <> "" Then
            mADSBMonitoring.PlannedDate = CDate(txtPlannedDate.Text)
        Else
            mADSBMonitoring.PlannedDate = System.DBNull.Value
        End If

        If txtComplianceDate.Text.ToString <> "" Then
            mADSBMonitoring.DateOfCompliance = CDate(txtComplianceDate.Text)
        Else
            mADSBMonitoring.DateOfCompliance = System.DBNull.Value
        End If

        mADSBMonitoring.IsPartially = rdbPartially.Checked
        mADSBMonitoring.AircraftHours = txtHours.Text.Trim
        mADSBMonitoring.Landings = txtLandings.Text.Trim
        mADSBMonitoring.Cycles = txtCycles.Text.Trim
        mADSBMonitoring.IsAuditRequired = chkAuditRequired.Checked
        If txtAuditDate.Text.ToString <> "" Then
            mADSBMonitoring.AuditDueDate = CDate(txtAuditDate.Text)
        Else
            mADSBMonitoring.AuditDueDate = System.DBNull.Value
        End If
        mADSBMonitoring.ReInspection = chkReInspection.Checked


        '******************************
        '''''AttachMyFile()
        For j As Integer = 0 To mADSBMonitoring.FileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgAttachment.Rows(j).FindControl("txtFileName"), TextBox)
            mADSBMonitoring.FileAttachments(j).FileName = txtValue.Text.Trim
        Next


        Session("mADSBMonitoring") = mADSBMonitoring
    End Sub
    Private Function Save() As Boolean
        Try
            setObject()
            mADSBMonitoring.ApplyEdit()


            If mADSBMonitoring.IsValid Then
                mADSBMonitoring.Save()
            Else
                upnlValidationsummary.Update()
            End If
            DataFieldBind()
            ControlVisibility()
            SetPage()

            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
            MarkLog(Util.Action.Save, "ADSBMonitoring", User.Identity.Name + " Saved AD/SB Monitoring : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBMonitoring.ID, EventLogID)

            Return True
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
    Private Sub SetPage()

        If mADSBTechRecording.IsNew = True Then
            lblTitle.InnerText = "Monitoring For " + mADSBTechRecording.ADSBRecordingText.ToString + " [ NEW ]"
        Else
            lblTitle.InnerText = "Monitoring For " + mADSBTechRecording.ADSBRecordingText.ToString + " [" + mADSBTechRecording.ADSBNo + "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result


        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    
                   If MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mADSBMonitoring As ADSBMonitoring
                            mADSBMonitoring = CType(Session("mADSBMonitoring"), ADSBMonitoring)
                            mADSBMonitoring.FileAttachments.Remove(mADSBMonitoring.FileAttachments.CurrentItem)
                            dgAttachment.DataSource = mADSBMonitoring.FileAttachments
                            dgAttachment.DataBind()
                            upnldgAttachment.Update()
                            upnlAttachment.Update()
                            Session("mADSBMonitoring") = mADSBMonitoring

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If Not CustomValidate1() Then
                            upnlValidationsummary.Update()
                        End If

                        If Save() Then
                            SetPage()
                            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                            MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved Invoice : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                            Response.Redirect("Index.aspx")
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session("mADSBTechRecording") = mADSBTechRecording
                        DataFieldBind()

                    End If
                Case MsgBoxResult.Ok

            End Select

        End If
    End Sub
#End Region


#Region "Data Binding"
    Private Sub DataFieldBind()


        If Not mADSBTechRecording.ADSBDateFormatted Is System.DBNull.Value Then
            txtADSBTechRecordingDate.Text = Format(CDate(mADSBTechRecording.ADSBDateFormatted), AppSettings("DateFormat"))
        Else
            txtADSBTechRecordingDate.Text = ""
        End If

        If Not mADSBMonitoring.EODate Is System.DBNull.Value Then
            txtEODate.Text = Format(CDate(mADSBMonitoring.EODateFormatted), AppSettings("DateFormat"))
        Else
            txtEODate.Text = ""
        End If


        If Not Session("IsOpenFromADSB") Is Nothing Then
            If Session("IsOpenFromADSB") = "True" Then
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mADSBTechRecording.ID)
                Session("mADSBTechRecording") = mADSBTechRecording
            End If
        End If

        dgEffectivityDetails.DataSource = mADSBTechRecording.ADSBTechRecordingApplicableOns
        dgAttachment.DataSource = mADSBMonitoring.FileAttachments


        DataBind()


        If mADSBTechRecording.ADSBStepsID <> 5 And mADSBTechRecording.IsFullyConfigured Then
            mADSBTechRecording.ADSBStepsID = 5
            mADSBTechRecording.Save()
        End If

    End Sub
    Public Sub ClearControl()
        txtEONo.Text = ""
        txtEODate.Text = ""
        txtFacility.Text = ""
        txtLocation.Text = ""
        txtPlannedDate.Text = ""
        txtComplianceDate.Text = ""
        rdbFully.Checked = True
        rdbPartially.Checked = False
        rdbPartially.Checked = False
        txtHours.Text = "0:00"
        txtLandings.Text = "0"
        txtCycles.Text = "0"
        chkAuditRequired.Checked = False
        txtAuditDate.Text = ""
        chkReInspection.Checked = False
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mADSBTechRecording.IsValid = False Then
            For i As Integer = 0 To mADSBTechRecording.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mADSBTechRecording.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

       
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False

        End If
        Return True
    End Function

    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtEONo" Then
            If txtADSBTechRecordingDate.Text = "" Then
                custValidator.ErrorMessage = "Select EO Date."
                e.IsValid = False
            End If

        End If
    End Sub
#End Region


#Region "Business Methods"
    Private Sub ControlVisibility()
        txtADSBTechRecordingDate.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)
        txtADSBTechRecordingText.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)
        txtADSBTechRecordingNo.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)

        txtADSBNO.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtSubject.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)

        dgEffectivityDetails.Columns(1).Visible = mADSBTechRecording.ApplicableToPart
        dgEffectivityDetails.Columns(2).Visible = mADSBTechRecording.ApplicableToModel

        Dim P As Boolean = False
        Dim lb As LinkButton 'ButtonColumn 
        For j As Integer = 0 To dgEffectivityDetails.Rows.Count - 1
            P = CType(Me.dgEffectivityDetails.Rows.Item(j).Cells(8).Text, Boolean)
            If P = True Then
                lb = CType(dgEffectivityDetails.Rows.Item(j).Cells(7).FindControl("lnkConfigure"), LinkButton)
                lb.Enabled = False
                lb.ToolTip = "Already Configured"
            End If
            dgEffectivityDetails.Rows(j).Cells(9).Enabled = IIf(P = False, False, True) 'History

        Next
        UpdatePanel()
    End Sub
    Private Sub ControlVisibilityPOPUP()
        If mADSBTechRecording.ApplicableToPart Then
            lblPartModelNo.InnerText = "Part No."
            txtPartModelNo.Text = mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentItem.PartName
        Else
            lblPartModelNo.InnerText = "Model No."
            txtPartModelNo.Text = mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentItem.ModelName
        End If
        txtSerialNo.Text = mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentItem.SerialNo

        If Not mADSBMonitoring.EODate Is System.DBNull.Value Then
            txtEODate.Text = Format(CDate(mADSBMonitoring.EODateFormatted), AppSettings("DateFormat"))
        Else
            txtEODate.Text = ""
        End If

        If Not mADSBMonitoring.PlannedDate Is System.DBNull.Value Then
            txtPlannedDate.Text = Format(CDate(mADSBMonitoring.PlannedDateFormatted), AppSettings("DateFormat"))
        Else
            txtPlannedDate.Text = ""
        End If

        If Not mADSBMonitoring.DateOfCompliance Is System.DBNull.Value Then
            txtComplianceDate.Text = Format(CDate(mADSBMonitoring.DateOfComplianceFormatted), AppSettings("DateFormat"))
        Else
            txtComplianceDate.Text = ""
        End If

        If Not mADSBMonitoring.AuditDueDate Is System.DBNull.Value Then
            txtAuditDate.Text = Format(CDate(mADSBMonitoring.AuditDueDateFormatted), AppSettings("DateFormat"))
        Else
            txtAuditDate.Text = ""
        End If
    End Sub
    Private Sub UpdatePanel()
        upnlADSBTechRecordingDetails.Update()
        upnlStatusName.Update()
        upnlTitle.Update()
        upnlADSBMonitoring.Update()
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mADSBMonitoring.FileAttachments.CurrentIndex = Index
        Session("mADSBMonitoring") = mADSBMonitoring
    End Sub
    Private Sub AttachMyFile()

        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        mADSBMonitoring = Session("mADSBMonitoring")
        Try
            If Not mADSBMonitoring.FileAttachments.Contains(mADSBMonitoring.ID, CType(Session("FileUpload.FileName"), String)) Then

                mADSBMonitoring.FileAttachments.Add(mADSBMonitoring.ID, CType(Session("FileUpload.FileName"), String))
                ' mADSBMonitoring.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mADSBMonitoring.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mADSBMonitoring.FileAttachments.CurrentItem.Size = Session("Size")
                mADSBMonitoring.FileAttachments.CurrentItem.Extension = Session("Extension")
                '   mADSBMonitoring.FileAttachments.CurrentItem.SrNo = (mADSBMonitoring.FileAttachments.Count - 1) + 1

                Session("mADSBMonitoring") = mADSBMonitoring
                dgAttachment.DataSource = mADSBMonitoring.FileAttachments
                dgAttachment.DataBind()

                For i As Integer = 0 To mADSBMonitoring.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mADSBMonitoring.FileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Else
                Session("mADSBMonitoring") = mADSBMonitoring
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack Then
            DataFieldBind()
            SetPage()
        End If
        ControlVisibility()
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMonitoringOk.Click

        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        If CustomValidate1() Then

            If (Not IsInRole(Rights.[New]) And mADSBMonitoring.IsNew) Or (Not IsInRole(Rights.Edit) And Not mADSBMonitoring.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If Save() Then
                SetPage()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mADSBTechRecording.ID)
                Session("mADSBTechRecording") = mADSBTechRecording
                mADSBMonitoring = Nothing
                Session("mADSBMonitoring") = mADSBMonitoring
                dgAttachment.DataSource = Nothing
                dgAttachment.DataBind()
                ClearControl()
            End If
        Else
            upnlValidationsummary.Update()
        End If

    End Sub
 
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click

        Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
        MarkLog(Util.Action.Close, "WOInvoice", ADSBDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)


        If mADSBTechRecording.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub dgEffectivityDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEffectivityDetails.RowCommand
        Select Case e.CommandName
            Case "ComplyRec"
                Dim index As Integer = CInt(e.CommandArgument) - 1
                mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentIndex = index
                Dim mADSBTechRecordingApplicableOnID As Guid = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).ID
                mADSBMonitoring = ADSBMonitoring.GetADSBMonitoring(mADSBTechRecordingApplicableOnID, "")
                If mADSBMonitoring.ADSBTechRecordingID.Equals(Guid.Empty) Then
                    mADSBMonitoring = ADSBMonitoring.NewADSBMonitoring(mADSBTechRecording.ID, mADSBTechRecordingApplicableOnID)
                End If

                Session("mADSBMonitoring") = mADSBMonitoring
                Session("mADSBTechRecording") = mADSBTechRecording

                ControlVisibility()
                ControlVisibilityPOPUP()
                dgAttachment.DataSource = mADSBMonitoring.FileAttachments
                upnlMonitoring.DataBind()
                upnlMonitoring.Update()
                mdlPopUpChangeMonitoring.Show()
                upnlAttachment.Update()
                UpdatePanel()
                upnlEffectivityDet.Update()
            Case "ConfigureRec"
                Dim index As Integer = CInt(e.CommandArgument) - 1
                mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentIndex = index
                Dim mADSBTechRecordingApplicableOnID As Guid = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).ID
                
                Dim mSerialNo As String = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).SerialNo

                Session("IsOpenFromADSB") = "True"
                Session("OpenFromModelCreation") = "True"
                Session("OpenFromADSBReviewMeeting") = "True"

                Session("DirectiveNoFromModelCreation") = mADSBTechRecording.ADSBNo

                mADSBConfiguration = ADSBConfiguration.NewADSBConfiguration(mADSBTechRecordingApplicableOnID)
                Session("mADSBConfiguration") = mADSBConfiguration
                'Model
                If mADSBTechRecording.ApplicableToModel = True Then
                    Dim mModelID As Guid = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).ModelID
                    Dim mModelName As String = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).ModelName

                    Session("ModelIDFromModelCreation") = mModelID
                    Session("ModelNameFromModelCreation") = mModelName
                      Dim mAssemblyStatusList As AssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(MachineID:=Guid.Empty, ModelId:=mModelID.ToString, AssemblySerialNo:=mSerialNo, IsAssemblyInstalled:=True)


                    If mAssemblyStatusList.Count > 0 Then
                        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusList(0).ID)
                        mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                        Session("mMachine") = mMachine


                        Session("mAssemblyStatus") = mAssemblyStatus

                        mADSBConfiguration.AssemblyStatusID = mAssemblyStatus.ID
                        Session("mADSBConfiguration") = mADSBConfiguration

                        Dim mModelMonitorModList As ModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(ModelID:=mModelID, DirectiveNo:=mADSBTechRecording.ADSBNo)

                        If mModelMonitorModList.Count > 0 Then
                            Session("NewPage") = "false"
                            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mModelID, mMachine.HourType)
                            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModelMonitorModListWindow", "OpenModelMonitorModListWindow();", True)
                        Else

                            Dim ID As Guid = Guid.NewGuid
                            mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, mModelID, 1, ID)
                            mModelMonitorMod.Number = mADSBTechRecording.ADSBNo
                            mModelMonitorMod.Description = mADSBTechRecording.Description
                            mModelMonitorMod.ComplianceRequirement = mADSBTechRecording.MethodOfCompliance
                            mModelMonitorMod.IssueDate = mADSBTechRecording.IssueDate


                            mModelMonitorMod.BeginEdit()
                            MarkLog(Util.Action.[New], "Model Monitor Mod", " Model : " & mModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)



                            mADSBConfiguration.ModelMonitorModID = mModelMonitorMod.ID
                            Session("mADSBConfiguration") = mADSBConfiguration
                            Session("mModelMonitorMod") = mModelMonitorMod



                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow()", True)
                        End If
                    Else
                        mAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(MachineID:=Guid.Empty, ModelId:=mModelID.ToString, AssemblySerialNo:=mSerialNo, IsAssemblyRemoved:=True)
                        If mAssemblyStatusList.Count > 0 Then
                            'MSGBoxCtrl.show("Alert..!!!", "This Assembly is Removed on " + mAssemblyStatusList(0).RemovedOnFormatted + " Configuration is not possible on this Assembly.", "", MsgBoxStyle.OkOnly)
                            MSGBoxCtrl.show("AD/SB Configuration Alert..!!!", "This Assembly is Removed on " + mAssemblyStatusList(0).RemovedOnFormatted + " .So configuration is not possible on this Assembly.", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If



                ElseIf mADSBTechRecording.ApplicableToPart = True Then
                    Dim mPartID As Guid = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).PartID
                    Dim mPartName As String = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).PartName

                    Dim mCompStatusList As CompStatusList = CompStatusList.GetCompStatusList(Guid.Empty, PartID:=mPartID.ToString, CompSerialNo:=mSerialNo, IsCompInstalled:=True, IsCompPeriodsRequired:=False, CurrentDate:=Today.Date.ToString)

                    If mCompStatusList.Count > 0 Then
                        mCompStatus = CompStatus.GetCompStatus(mCompStatusList(0).ID, mCompStatusList(0).AssemblyStatusID, Today.Date.ToString)
                        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusList(0).AssemblyStatusID)
                        mADSBConfiguration.AssemblyStatusID = mAssemblyStatus.ID
                        mADSBConfiguration.CompStatusID = mCompStatus.ID
                        Session("mADSBConfiguration") = mADSBConfiguration


                        'mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString, ModelID:=mAssemblyStatus.Assembly.ModelID.ToString)
                        'Session("mMachineNameValueList") = mMachineNameValueList

                        mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                        Session("mMachine") = mMachine

                        mADSBConfiguration.AssemblyStatusID = mAssemblyStatus.ID
                        Session("mADSBConfiguration") = mADSBConfiguration

                        Session("mCompStatus") = mCompStatus
                        Session("mAssemblyStatus") = mAssemblyStatus

                        Dim mPartMonitorModList As PartMonitorModList = PartMonitorModList.GetPartMonitorModList(mPartID, Guid.Empty, DirectiveNo:=mADSBTechRecording.ADSBNo)
                        '' Dim mComponentMaintananceListCount As ComponentMaintananceListCount = ComponentMaintananceListCount.GetComponentMaintananceListCount(mCompStatus.Comp.PartID)

                        If mPartMonitorModList Is Nothing Or mPartMonitorModList.Count > 0 Then
                            Session("NewPage") = "false"
                            mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.Comp.ID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDateFormatted, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, 1)
                            Session("mCompMonitorModStatus") = mCompMonitorModStatus
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenPartMonitorModListWindow", "OpenPartMonitorModListWindow();", True)
                        Else
                            Dim ID As Guid = Guid.NewGuid
                            mPartMonitorMod = PartMonitorMod.NewPartMonitorMod(ID, mPartID, mAssemblyStatus.Assembly.ModelID, 1)
                            mPartMonitorMod.Number = mADSBTechRecording.ADSBNo
                            mPartMonitorMod.Description = mADSBTechRecording.Description
                            mPartMonitorMod.ComplianceRequirement = mADSBTechRecording.MethodOfCompliance
                            mPartMonitorMod.IssueDate = mADSBTechRecording.IssueDate
                          
                            mPartMonitorMod.BeginEdit()
                            MarkLog(Util.Action.[New], "Part Monitor Mod", " Model : " & mPartName, Util.ErrorType.NoError, Guid.Empty, EventLogID)



                            mADSBConfiguration.PartMonitorModID = mPartMonitorMod.ID
                            Session("mADSBConfiguration") = mADSBConfiguration
                            Session("mPartMonitorMod") = mPartMonitorMod

                            mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.Comp.ID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDateFormatted, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, 1)

                            Session("mAssemblyStatus") = mAssemblyStatus
                            Session("mCompMonitorModStatus") = mCompMonitorModStatus

                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow();", True)
                        End If
                    Else
                        mCompStatusList = CompStatusList.GetCompStatusList(Guid.Empty, PartID:=mPartID.ToString, CompSerialNo:=mSerialNo, IsCompRemoved:=True, IsCompPeriodsRequired:=False, CurrentDate:=Today.Date.ToString)
                        If mCompStatusList.Count > 0 Then
                            MSGBoxCtrl.show("AD/SB Configuration Alert..!!!", "This Component is Removed on " + mCompStatusList(0).RemovedOnFormatted + " .So configuration is not possible on this Component.", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If

                    End If
                End If
            Case "History"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument)
                mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentIndex = index
                Dim mADSBTechRecordingApplicableOnID As Guid = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).ID

                Dim mSerialNo As String = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).SerialNo


                mADSBConfiguration = ADSBConfiguration.GetADSBConfiguration(mADSBTechRecordingApplicableOnID, "")
                Session("mADSBConfiguration") = mADSBConfiguration

                If mADSBTechRecording.ApplicableToModel = True Then
                    Dim mModelID As Guid = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).ModelID
                    Dim mModelName As String = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).ModelName

                    Session("ModelIDFromModelCreation") = mModelID
                    Session("ModelNameFromModelCreation") = mModelName
                    Dim mAssemblyStatusList As AssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(MachineID:=Guid.Empty, ModelId:=mModelID.ToString, AssemblySerialNo:=mSerialNo, IsAssemblyInstalled:=True)


                    If mAssemblyStatusList.Count > 0 Then
                        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusList(0).ID)
                        mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                        Session("mMachine") = mMachine

                    End If
                    Session("mAssemblyStatus") = mAssemblyStatus

                    Dim mUpdateComplyHistoryAssemblyMonitorModStatusList As UpdateComplyHistoryAssemblyMonitorModStatusList

                    mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mADSBConfiguration.ModelMonitorModID, mMachine.HourType)
                    Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList

                    If mUpdateComplyHistoryAssemblyMonitorModStatusList.Count = 0 Then
                        MSGBoxCtrl.show("History Alert..!!", "No Status found.Status must have been deleted from outside. ", "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If

                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModHistoryWindow", "OpenModHistoryWindow();", True)

                ElseIf mADSBTechRecording.ApplicableToPart = True Then
                    Dim mPartID As Guid = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).PartID
                    Dim mPartName As String = mADSBTechRecording.ADSBTechRecordingApplicableOns(index).PartName

                    Dim mCompStatusList As CompStatusList = CompStatusList.GetCompStatusList(Guid.Empty, PartID:=mPartID.ToString, CompSerialNo:=mSerialNo, IsCompInstalled:=True, IsCompPeriodsRequired:=False, CurrentDate:=Today.Date.ToString)

                    If mCompStatusList.Count > 0 Then

                        mCompStatus = CompStatus.GetCompStatus(mCompStatusList(0).ID, mCompStatusList(0).AssemblyStatusID, Today.Date.ToString)
                        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusList(0).AssemblyStatusID)

                        mMachine = Machine.GetMachine(mAssemblyStatus.MachineID)
                        Session("mMachine") = mMachine

                        Dim mUpdateComplyHistoryCompMonitorModStatusList As UpdateComplyHistoryCompMonitorModStatusList

                        mUpdateComplyHistoryCompMonitorModStatusList = UpdateComplyHistoryCompMonitorModStatusList.GetComplyHistoryCompMonitorModStatusList(mCompStatus.CompID, mADSBConfiguration.PartMonitorModID, mMachine.HourType)
                        Session("mUpdateComplyHistoryCompMonitorModStatusList") = mUpdateComplyHistoryCompMonitorModStatusList

                        If mUpdateComplyHistoryCompMonitorModStatusList.Count = 0 Then
                            MSGBoxCtrl.show("History Alert..!!", "No Status found.Status must have been deleted from outside. ", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If

                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDirectiveHistoryWindow", "OpenDirectiveHistoryWindow();", True)
                    End If
                End If


        End Select
    End Sub
    
    Private Sub hdnBtnModelModMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelModMaster.Click
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Today.Date.ToString, mAssemblyStatus.Assembly.ModelID, 1)
        mAssemblyMonitorModStatus.ModelMonitorModID(False) = mModelMonitorMod.ID
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("IsOpenFromADSB") = "True"
        Session("mADSBConfiguration") = mADSBConfiguration
        Response.Redirect("wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfADSBMonitoring.aspx")

    End Sub
    Private Sub hdnBtnModMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModMaster.Click
        '  mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.Comp.ID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDateFormatted, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, 1)
        mCompMonitorModStatus.PartMonitorModID(False) = mADSBConfiguration.PartMonitorModID
        Session("IsOpenFromADSB") = "True"
        Session("mADSBConfiguration") = mADSBConfiguration
        'Response.Redirect("wfCompMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfADSBMonitoring.aspx")
        Response.Redirect("wfCompMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfADSBMonitoring.aspx")
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMonitoringClose.Click
        mdlPopUpChangeMonitoring.Hide()
        mADSBTechRecording = Session("mADSBTechRecording")
        mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mADSBTechRecording.ID)
        Session("mADSBTechRecording") = mADSBTechRecording
        dgEffectivityDetails.DataSource = mADSBTechRecording.ADSBTechRecordingApplicableOns
        dgEffectivityDetails.DataBind()
        ControlVisibility()
    End Sub
    Private Sub btnSelectFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        setObject()
        Session("mADSBTechRecording") = mADSBTechRecording
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub

    Private Sub dgAttachment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAttachment.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachments = mADSBMonitoring.FileAttachments
                'mFileAttachments.CurrentIndex = Index - 1

                If mFileAttachments.Count = 1 Then
                    mFileAttachments.CurrentIndex = 0
                Else
                    mFileAttachments.CurrentIndex = Index - 1
                End If

                If mFileAttachments.CurrentItem.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
                dgAttachment.DataSource = mADSBMonitoring.FileAttachments
                dgAttachment.DataBind()
                ControlVisibility()
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Case "Remove"

                Dim Index As Integer = CInt(e.CommandArgument) + dgAttachment.PageSize * dgAttachment.PageIndex
                mFileAttachments = mADSBMonitoring.FileAttachments
                If mFileAttachments.Count = 1 Then
                    DeleteAttachment(0)
                Else
                    DeleteAttachment(Index - 1)
                End If
        End Select

    End Sub

    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlAttachment.Update()
    End Sub
#End Region


End Class