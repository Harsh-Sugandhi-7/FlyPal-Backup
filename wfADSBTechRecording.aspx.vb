


'Created by : Saylee
'Dated      : 5-Sep-2022

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text



Public Class wfADSBTechRecording
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
    Dim mUser As User
   
#End Region

#Region "Helper Methods"

    Private Sub addAttributes()
        txtADSBTechRecordingText.Attributes.Add("onblur", "WaterMark(this, event);")
        txtADSBTechRecordingText.Attributes.Add("onfocus", "WaterMark(this, event);")
    End Sub
    Private Sub GetSession()
        mADSBTechRecording = Session("mADSBTechRecording")
    End Sub

    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "ADSBTechRecording"
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
        If txtADSBTechRecordingDate.Text.ToString <> "" Then
            mADSBTechRecording.Date = CDate(txtADSBTechRecordingDate.Text)
        Else
            mADSBTechRecording.Date = System.DBNull.Value
        End If

        ''mADSBTechRecording.Text = txtADSBTechRecordingText.Text.Trim
        If txtADSBTechRecordingText.Text.Trim = "Select your prefix" Then
            mADSBTechRecording.Text = ""
        Else
            mADSBTechRecording.Text = Trim(txtADSBTechRecordingText.Text.Trim)
        End If


        mADSBTechRecording.No = Val(txtADSBTechRecordingNo.Text.Trim)

        mADSBTechRecording.ADSBNo = txtADSBNO.Text.Trim
        mADSBTechRecording.ADSBSubject = txtSubject.Text.Trim
        mADSBTechRecording.Description = txtDescription.Text.Trim
        mADSBTechRecording.RevNo = txtRevNo.Text.Trim
        mADSBTechRecording.MethodOfCompliance = txtCompliance.Text.Trim
        mADSBTechRecording.RevChangeInBrief = txtRevChange.Text.Trim
        ''  mADSBTechRecording.Applicability = chkApplicability.checked

        'IssueDate
        If txtIssueDate.Text.ToString <> "" Then
            mADSBTechRecording.IssueDate = CDate(txtIssueDate.Text)
        Else
            mADSBTechRecording.IssueDate = System.DBNull.Value
        End If

        'EffectiveDate
        If txtEffectiveDate.Text.ToString <> "" Then
            mADSBTechRecording.EffectiveDate = CDate(txtEffectiveDate.Text)
        Else
            mADSBTechRecording.EffectiveDate = System.DBNull.Value
        End If

        'RevDate
        If txtRevDate.Text.ToString <> "" Then
            mADSBTechRecording.RevDate = CDate(txtRevDate.Text)
        Else
            mADSBTechRecording.RevDate = System.DBNull.Value
        End If

        '******************************
        '''''''AttachMyFile()

        Dim mADSBTechRecordingClone As ADSBTechRecording
        mADSBTechRecordingClone = mADSBTechRecording.Clone

        For j As Integer = 0 To mADSBTechRecording.FileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgAttachment.Rows(j).FindControl("txtFileName"), TextBox)
            mADSBTechRecording.FileAttachments(j).FileName = txtValue.Text.Trim




        Next

        Session("mADSBTechRecording") = mADSBTechRecording
    End Sub
    Private Function Save() As Boolean
        Try
            setObject()

            If mADSBTechRecording.FileAttachments.Count > 1 Then
                Dim FileNameWiseItem = (From c In mADSBTechRecording.FileAttachments
                               Where c.FileName <> "" _
                             Group By FileName = c.FileName Into Group
                             Select New With {.FileName = FileName, .ReceiptItemCollection = Group, .InstanceCount = Group.Count()})

                Dim FileNameCount
                For Each FileNameCount In FileNameWiseItem
                    If FileNameCount.InstanceCount > 1 Then
                        '' MSGBoxCtrl.show("Alert!", "Can Not Save/Authorized !" + " <BR> Attached File Name " + FileNameCount.FileName + "Same.", "", MsgBoxStyle.OkOnly, "")
                        MSGBoxCtrl.show("Duplicate Alert!", "You are trying to add same filename. Only unique filename is allowed", "", MsgBoxStyle.OkOnly, "")
                        Return False
                        Exit Function
                    End If
                Next
            End If

            mADSBTechRecording.ApplyEdit()


            If mADSBTechRecording.IsValid Then
                mADSBTechRecording.Save()
            Else
                upnlValidationsummary.Update()
            End If
            DataFieldBind()
            ControlVisibility()
            SetPage()

            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
            MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)

            
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
            lblTitle.InnerText = "AD/SB For " + mADSBTechRecording.ADSBRecordingText.ToString + " [ NEW ]"
        Else
            lblTitle.InnerText = "AD/SB For " + mADSBTechRecording.ADSBRecordingText.ToString + " [" + mADSBTechRecording.ADSBNo + "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub AttachMyFile()

        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        mADSBTechRecording = Session("mADSBTechRecording")
        Try
            If Not mADSBTechRecording.FileAttachments.Contains(mADSBTechRecording.ID, CType(Session("FileUpload.FileName"), String)) Then

                mADSBTechRecording.FileAttachments.Add(mADSBTechRecording.ID, CType(Session("FileUpload.FileName"), String))
                ' mADSBTechRecording.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mADSBTechRecording.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mADSBTechRecording.FileAttachments.CurrentItem.Size = Session("Size")
                mADSBTechRecording.FileAttachments.CurrentItem.Extension = Session("Extension")
                '   mADSBTechRecording.FileAttachments.CurrentItem.SrNo = (mADSBTechRecording.FileAttachments.Count - 1) + 1

                Session("mADSBTechRecording") = mADSBTechRecording
                dgAttachment.DataSource = mADSBTechRecording.FileAttachments
                dgAttachment.DataBind()

                For i As Integer = 0 To mADSBTechRecording.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mADSBTechRecording.FileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Else
                Session("mADSBTechRecording") = mADSBTechRecording
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
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
                            Dim mADSBTechRecording As ADSBTechRecording
                            mADSBTechRecording = CType(Session("mADSBTechRecording"), ADSBTechRecording)
                            mADSBTechRecording.FileAttachments.Remove(mADSBTechRecording.FileAttachments.CurrentItem)
                            mADSBTechRecording.TempStatusID = 1
                            dgAttachment.DataSource = mADSBTechRecording.FileAttachments
                            dgAttachment.DataBind()
                            upnldgAttachment.Update()
                            upnlAttachment.Update()
                            Session("mADSBTechRecording") = mADSBTechRecording

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
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mADSBTechRecording.IsValid = True Then
                            mADSBTechRecording.StatusID = 2
                            Save()
                            DataFieldBind()
                            ControlVisibility()
                            lblStatus.DataBind()
                            UpdatePanel()
                            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                            MarkLog(Util.Action.Authorize, "ADSBTechRecording", User.Identity.Name + " Authorized AD/SB : " + ADSBDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                            MSGBoxCtrl.show("Authorized!", "Authorized SuccessFully", "", MsgBoxStyle.OkOnly, "")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        If mADSBTechRecording.IsValid = True Then
                            mADSBTechRecording.StatusID = 4
                            Save()
                            DataFieldBind()
                            lblStatus.DataBind()
                            UpdatePanel()
                            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                            MarkLog(Util.Action.Cancel, "ADSBTechRecording", User.Identity.Name + " Canceled Invoice : " + ADSBDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                            MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")

                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If Not CustomValidate1() Then
                            upnlValidationsummary.Update()
                            Exit Sub
                        End If
                      

                        If Save() Then
                            SetPage()
                            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                            MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved Invoice : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, ADSBDetail.ID, EventLogID)
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

        If Not mADSBTechRecording.IssueDateFormatted Is System.DBNull.Value Then
            txtIssueDate.Text = Format(CDate(mADSBTechRecording.IssueDateFormatted), AppSettings("DateFormat"))
        Else
            txtIssueDate.Text = ""
        End If

        If Not mADSBTechRecording.EffectiveDate Is System.DBNull.Value Then
            txtEffectiveDate.Text = Format(CDate(mADSBTechRecording.EffectiveDateFormatted), AppSettings("DateFormat"))
        Else
            txtEffectiveDate.Text = ""
        End If

        If Not mADSBTechRecording.RevDate Is System.DBNull.Value Then
            txtRevDate.Text = Format(CDate(mADSBTechRecording.RevDateFormatted), AppSettings("DateFormat"))
        Else
            txtRevDate.Text = ""
        End If

        dgAttachment.DataSource = mADSBTechRecording.FileAttachments

        DataBind()

       
    End Sub
   
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mADSBTechRecording.IsValid = False Then
            For i As Integer = 0 To mADSBTechRecording.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mADSBTechRecording.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If txtIssueDate.Text = "" Then
            ''  MSGBoxCtrl.show("Alert..!!", "Issue Date Required.", "", MsgBoxStyle.OkOnly, "")
            ''Exit Function
            strMsg = strMsg + "Issue Date Required." + "<Br>"
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
        If custValidator.ControlToValidate = "txtADSBTechRecordingDate" Then
            If txtADSBTechRecordingDate.Text = "" Then
                custValidator.ErrorMessage = "Select Date."
                e.IsValid = False
            End If

        End If
        If custValidator.ControlToValidate = "txtADSBNO" Then
            If txtIssueDate.Text = "" Then
                custValidator.ErrorMessage = "Issue Date Required."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region "Buissness Methods"
    Private Sub ControlVisibility()
        txtADSBTechRecordingDate.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)
        txtADSBTechRecordingText.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)
        txtADSBTechRecordingNo.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)

        txtADSBNO.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtSubject.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtDescription.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtIssueDate.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtEffectiveDate.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtRevDate.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtRevNo.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtCompliance.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtRevChange.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)



        btnCancel.Visible = (Not mADSBTechRecording.IsNew) And (mADSBTechRecording.StatusID = 2)
        btnAuthorized.Visible = (Not mADSBTechRecording.IsNew) And (mADSBTechRecording.StatusID = 1)
        btnSave.Visible = (Not mADSBTechRecording.StatusID >= 2)
        ''btnPrint.Visible = (Not mADSBTechRecording.IsNew)
        btnSelectFiles.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        dgAttachment.Columns(5).Visible = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        UpdatePanel()
    End Sub
    Private Sub UpdatePanel()
        upnlADSBTechRecordingDetails.Update()
        upnlStatusName.Update()
        upnlTitle.Update()
        upnlButtons.Update()
        upnlAttachment.Update()
        upnlStatusName.Update()
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mADSBTechRecording.FileAttachments.CurrentIndex = Index
        Session("mADSBTechRecording") = mADSBTechRecording
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
            ControlVisibility()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        If CustomValidate1() Then

            If (Not IsInRole(Rights.[New]) And mADSBTechRecording.IsNew) Or (Not IsInRole(Rights.Edit) And Not mADSBTechRecording.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If Save() Then
                SetPage()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                'Added on 21-May-2018 by Shital
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mADSBTechRecording.ID)
                Session("mADSBTechRecording") = mADSBTechRecording
                '----------
            End If
        Else
            upnlValidationsummary.Update()
        End If

    End Sub
    Private Sub btnAuthorized_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click

        If (Not IsInRole(Rights.Authorized)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If

        If IsValid Then
            Session("mADSBTechRecording") = mADSBTechRecording
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>AD/SB</strong>", MsgBoxStyle.YesNo, "Status")
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click ''===============================WO - 2006-2007-1-19

        If (Not IsInRole(Rights.Authorized)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user to cancel this AD/SB", False), True)
            Exit Sub
        End If

        If IsValid Then

            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> AD/SB </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
            Session("mADSBTechRecording") = mADSBTechRecording
        End If
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click

        Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
        MarkLog(Util.Action.Close, "WOInvoice", ADSBDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
        setObject()

        If mADSBTechRecording.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnSelectFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        SetObject()
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
                mFileAttachments = mADSBTechRecording.FileAttachments
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
                dgAttachment.DataSource = mADSBTechRecording.FileAttachments
                dgAttachment.DataBind()
                ControlVisibility()
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Case "Remove"

                Dim Index As Integer = CInt(e.CommandArgument) + dgAttachment.PageSize * dgAttachment.PageIndex
                mFileAttachments = mADSBTechRecording.FileAttachments
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

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mDistinctADSBText As DistinctADSBText
        mDistinctADSBText = DistinctADSBText.GetDistinctTextList(prefixText:=prefixText)

        If count = 0 Then
            Return (From c As DistinctADSBText.TextInfo In mDistinctADSBText
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctADSBText.TextInfo In mDistinctADSBText
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
#End Region
End Class