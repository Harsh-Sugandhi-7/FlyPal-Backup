
Imports System.Collections.Generic
Imports System.Linq
Imports java.util

Public Class wfMPDAMPRef
    Inherits System.Web.UI.Page


#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Variables and Declarations "
    Public mMPDRef As MPDAMPRef
    Public mMPDRefList As MPDAMPRefList

    Dim mFileAttach As FileAttach
    Dim mFileAttachAMP As FileAttach
    Dim EventLogID As Guid
    Dim IsAttachmentDeleted As Boolean = False
    Dim IsAttachmentDeletedAMP As Boolean = False
    Dim mLastMPDRef As LastMPDAMPRef
    Dim mLastAMPRef As LastMPDAMPRef
    Dim mMPDRefDetailForEventLog As String = ""
    Dim mAMPRefDetailForEventLog As String = ""
    Protected mtmpLastMPDRef As MPDAMPRef
    Protected mtmpLastAMPRef As MPDAMPRef
    Private checkedIds As New List(Of String)()
    Dim mMachine As Machine

    Public mAMPRefList As MPDAMPRefList
    Public mAMPRef As MPDAMPRef
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMPDRef = Session("mMPDRef")
        mMPDRefList = Session("mMPDRefList")

        mAMPRef = Session("mAMPRef")
        mAMPRefList = Session("mAMPRefList")

        mFileAttach = Session("mFileAttach")
        mFileAttachAMP = Session("mFileAttachAMP")
        mLastMPDRef = Session("mLastMPDRef")
        mMachine = Session("mMachine")
    End Sub
    Private Sub SetSession()
        mMPDRef = Session("mMPDRef")
        mMPDRefList = Session("mMPDRefList")
        mAMPRef = Session("mAMPRef")
        mAMPRefList = Session("mAMPRefList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMPDRef")
        Session.Remove("mMPDRefList")
        Session.Remove("mAMPRef")
        Session.Remove("mAMPRefList")
        Session.Remove("mLastMPDRef")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "MPDAMPRef"
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
        End Select
    End Function
    Private Sub setMPDObject()

        mMPDRef.ModelID = mMachine.AssemblyStatus.Assembly.ModelID
        mMPDRef.RevNo = Trim(txtMPDRevisionNo.Text)
        mMPDRef.MPDNo = Trim(txtMPDNo.Text)
        mMPDRef.FromDate = Trim(txtMPDFromDate.Text)
        If mMPDRef.IsNew Then
            mMPDRef.IsRevised = False
            mMPDRef.ToDate = System.DBNull.Value
        End If

        Session("mMPDRef") = mMPDRef

        mLastMPDRef = LastMPDAMPRef.GetLastMPDAMPRefForModel(mMachine.AssemblyStatus.Assembly.ModelID, mMPDRef.ID.ToString)
        Session("mLastMPDRef") = mLastMPDRef

        If Not mLastMPDRef.MPDNo = "" And Not mLastMPDRef.ID.Equals(mMPDRef.ID) Then
            mtmpLastMPDRef = MPDAMPRef.GetMPDAMPRef(mLastMPDRef.ID)
            mtmpLastMPDRef.ToDate = DateAdd(DateInterval.Day, -1, CType(mMPDRef.FromDate, Date))
            mtmpLastMPDRef.IsRevised = True
            Session("mtmpLastMPDRef") = mtmpLastMPDRef
        Else
            Session.Remove("mtmpLastMPDRef")
        End If

        For i As Integer = 0 To mMPDRef.FileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgMPDAttachment1.Rows(i).FindControl("txtMPDFileName"), TextBox)
            mMPDRef.FileAttachments(i).FileName = txtValue.Text.Trim
        Next
    End Sub
    Private Sub setAMPObject()

        mAMPRef.MachineID = mMachine.ID
        mAMPRef.RevNo = Trim(TxtAMPRevisionNo.Text)
        mAMPRef.AMPNo = Trim(txtAmpNo.Text)
        mAMPRef.FromDate = Trim(txtAMPFromDate.Text)
        If mAMPRef.IsNew Then
            mAMPRef.IsRevised = False
            mAMPRef.ToDate = System.DBNull.Value
        End If

        Session("mAMPRef") = mAMPRef

        mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mMachine.ID, mAMPRef.ID.ToString)
        Session("mLastAMPRef") = mLastAMPRef

        If Not mLastAMPRef.AMPNo = "" And Not mLastAMPRef.ID.Equals(mAMPRef.ID) Then
            mtmpLastAMPRef = MPDAMPRef.GetMPDAMPRef(mLastAMPRef.ID)
            mtmpLastAMPRef.ToDate = DateAdd(DateInterval.Day, -1, CType(mAMPRef.FromDate, Date))
            mtmpLastAMPRef.IsRevised = True
            Session("mtmpLastAMPRef") = mtmpLastAMPRef
        Else
            Session.Remove("mtmpLastAMPRef")
        End If
        For i As Integer = 0 To mAMPRef.FileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgASPAttachment.Rows(i).FindControl("txtFileName"), TextBox)
            mAMPRef.FileAttachments(i).FileName = txtValue.Text.Trim
        Next
    End Sub
    Private Function SaveMPD(Optional ByVal CreatNewRecordAfterSave As Boolean = False, Optional ByVal ClosePageAfterSave As Boolean = False) As Boolean
        Try
            mMPDRef.ApplyEdit()
            mMPDRef.Save()
            MarkLog(Util.Action.Save, "PartCMMRef", "Part No. : " + txtModel.Text.ToString + " Ref No. : " + txtMPDNo.Text.Trim, Util.ErrorType.NoError, mMPDRef.ID, EventLogID)

            mtmpLastMPDRef = Session("mtmpLastMPDRef")
            'If Not mtmpLastMPDRef Is Nothing Then mtmpLastMPDRef.Save()

            Return True
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")

            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "UK_tabMPDRef", CompareMethod.Text) Then
                Dim DuplicateEntryMessage As String = ex1.Message.Substring(ex1.Message.IndexOf("SerialNo.:"))
                'MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + DuplicateEntryMessage, MsgBoxStyle.OkOnly, "")
                MSGBoxCtrl.Show("Duplicate Alert!", "You are trying to save the duplicate entry for" + DuplicateEntryMessage, "", MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
    Private Function SaveAMP(Optional ByVal CreatNewRecordAfterSave As Boolean = False, Optional ByVal ClosePageAfterSave As Boolean = False) As Boolean
        Try
            mAMPRef.ApplyEdit()
            mAMPRef.Save()
            mtmpLastAMPRef = Session("mtmpLastAMPRef")
            ' If Not mtmpLastAMPRef Is Nothing Then mtmpLastAMPRef.Save()

            Return True
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")

            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "UK_tabMPDRef", CompareMethod.Text) Then
                Dim DuplicateEntryMessage As String = ex1.Message.Substring(ex1.Message.IndexOf("SerialNo.:"))
                'MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + DuplicateEntryMessage, MsgBoxStyle.OkOnly, "")
                MSGBoxCtrl.Show("Duplicate Alert!", "You are trying to save the duplicate entry for" + DuplicateEntryMessage, "", MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        If CustomValidate() = True Then
                            If SaveMPD() Then
                                SetPage()
                                'MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")

                                RemoveSession()
                                Dim mopenas As String = Request.QueryString("Type")
                                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                    Exit Sub
                                End If
                            End If
                        Else
                            upnlValidationSummary.Update()
                        End If
                    ElseIf MSGBoxCtrl.Sender = "Delete" Then

                        Try
                            Session("sender") = ""
                            mMPDRef = CType(Session("mMPDRef"), MPDAMPRef)
                            MPDAMPRef.DeleteMPDAMPRef(mMPDRef.ID)
                            DataFieldBind()
                            upnldgMPDRefList.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "zza")
                                'MarkLog(Util.Action.Delete, "", ex.Message, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If

                            DataFieldBind()
                            upnldgMPDRefList.Update()

                        Finally

                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteAMP" Then

                        Try
                            Session("sender") = ""
                            mAMPRef = CType(Session("mAMPRef"), MPDAMPRef)
                            MPDAMPRef.DeleteMPDAMPRef(mAMPRef.ID)
                            DataFieldBindAMP()

                            upnDgAMP.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "ad")
                                'MarkLog(Util.Action.Delete, "", ex.Message, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If

                            DataFieldBindAMP()
                            upnDgAMP.Update()

                        Finally

                        End Try
                    ElseIf MSGBoxCtrl.Sender = "RemoveAttachmentAMP" Then

                        Try
                            Session("Sender") = ""
                            Dim mAMPRef As MPDAMPRef
                            mAMPRef = CType(Session("mAMPRef"), MPDAMPRef)
                            ' mMPDRefDetailForEventLogAMP = "Reference No : " + mAMPRef.RefNo.ToString + " Part Name : " + mAMPRef.ModelName
                            Dim FileName As String = mAMPRef.FileAttachments.CurrentItem.FileName
                            mAMPRef.FileAttachments.Remove(mAMPRef.FileAttachments.CurrentItem)
                            dgASPAttachment.DataSource = mAMPRef.FileAttachments
                            dgASPAttachment.DataBind()
                            'If mMPDRef.FileAttachments.Count = 0 Then
                            '    If mMPDRef.FileAttachments.IsDirty Then
                            '        mMPDRef.Save()
                            '    End If
                            'End If


                            upnlAMPAttachment.Update()
                            upnldgAMPAttachment.Update()
                            Session("mMPDRef") = mMPDRef
                            Session.Remove("Size")
                            Session.Remove("ImageFile")
                            Session.Remove("Extension")
                            Session.Remove("FileUpload.FileName")

                            MarkLog(Util.Action.Remove, "PartCMMRef", "Attachment : " + FileName + " removed for " + mAMPRefDetailForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            'MSGBoxCtrl.show("Removed Successfully!", "Attachment Removed Successfully!", "")
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try


                    ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mMPDRef As MPDAMPRef
                            mMPDRef = CType(Session("mMPDRef"), MPDAMPRef)
                            mMPDRefDetailForEventLog = "Reference No : " + mMPDRef.RefNo.ToString + " Part Name : " + mMPDRef.ModelName
                            Dim FileName As String = mMPDRef.FileAttachments.CurrentItem.FileName
                            mMPDRef.FileAttachments.Remove(mMPDRef.FileAttachments.CurrentItem)
                            dgMPDAttachment1.DataSource = mMPDRef.FileAttachments
                            dgMPDAttachment1.DataBind()
                            'If mMPDRef.FileAttachments.Count = 0 Then
                            '    If mMPDRef.FileAttachments.IsDirty Then
                            '        mMPDRef.Save()
                            '    End If
                            'End If

                            upnlGridMPDAttachment.Update()
                            Session("mMPDRef") = mMPDRef

                            MarkLog(Util.Action.Remove, "PartCMMRef", "Attachment : " + FileName + " removed for " + mMPDRefDetailForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            '  MSGBoxCtrl.show("Removed Successfully!", "Attachment Removed Successfully!", "")
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





                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        RemoveSession()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        Response.Redirect("index.aspx")
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Function CustomValidate() As Boolean
        Dim strMsg As String = ""
        setMPDObject()

        If Not mMPDRef.IsValid Then
            For i As Integer = 0 To mMPDRef.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMPDRef.GetBrokenRulesCollection(i).Description + "<Br>"
            Next

        End If

        'If mLastMPDRef.RefNo <> "" And mMPDRef.IsNew Then
        '    If Not mLastMPDRef.ID.Equals(mMPDRef.ID) And CDate(Trim(txtFromDate.Text)) <= CDate(mLastMPDRef.FromDateFormatted.ToString) Then
        '        strMsg = strMsg + "Revision Date should be greater than Last Revision Date " + mLastMPDRef.FromDateFormatted
        '    End If

        'End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        If strMsg <> "" Then
            cvValidator.IsValid = False
            cvValidator.ErrorMessage = strMsg
            Return False
        End If

        Return True
    End Function
    Private Function CustomValidateAMP() As Boolean
        Dim strMsg As String = ""
        setAMPObject()

        If Not mAMPRef.IsValid Then
            For i As Integer = 0 To mAMPRef.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mAMPRef.GetBrokenRulesCollection(i).Description + "<Br>"
            Next

        End If

        'If mLastMPDRef.RefNo <> "" And mMPDRef.IsNew Then
        '    If Not mLastMPDRef.ID.Equals(mMPDRef.ID) And CDate(Trim(txtFromDate.Text)) <= CDate(mLastMPDRef.FromDateFormatted.ToString) Then
        '        strMsg = strMsg + "Revision Date should be greater than Last Revision Date " + mLastMPDRef.FromDateFormatted
        '    End If

        'End If

        If strMsg <> "" Then
            CustomValidator1.IsValid = False
            CustomValidator1.ErrorMessage = strMsg
            Return False
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        Return True
    End Function
    Private Sub AttachFile()
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        If Not Session("Extension") = ".pdf" Then
            MSGBoxCtrl.Show("Alert!", "Please attach in PDF only!", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Try
            If Not mMPDRef.FileAttachments.Contains(mMPDRef.ID, CType(Session("FileUpload.FileName"), String)) Then
                mMPDRef.FileAttachments.Add(mMPDRef.ID, CType(Session("FileUpload.FileName"), String)) 'Added By Vikrant On 17-Apr-2013 For ALL17042013
                ' mMPDRef.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mMPDRef.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mMPDRef.FileAttachments.CurrentItem.Size = Session("Size")
                mMPDRef.FileAttachments.CurrentItem.Extension = Session("Extension")
                '   mMPDRef.FileAttachments.CurrentItem.SrNo = (mMPDRef.FileAttachments.Count - 1) + 1


                dgMPDAttachment1.DataSource = mMPDRef.FileAttachments
                dgMPDAttachment1.DataBind()

                For i As Integer = 0 To mMPDRef.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgMPDAttachment1.Rows(i).FindControl("txtMPDFileName"), TextBox)
                    txtValue.Text = mMPDRef.FileAttachments(i).FileName
                Next
                Session("mMPDRef") = mMPDRef
                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlGridMPDAttachment.Update()

            Else
                Session("mMPDRef") = mMPDRef
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
            End If
        Catch ex As Exception
            Throw ex
        Finally
        End Try
    End Sub
    Private Sub AttachFileAMP()
        '  If MyFile1.Value <> "" Then
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        If Not Session("Extension") = ".pdf" Then
            MSGBoxCtrl.Show("Alert!", "Please attach in PDF only!", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Try
            If Not mAMPRef.FileAttachments.Contains(mAMPRef.ID, CType(Session("FileUpload.FileName"), String)) Then
                mAMPRef.FileAttachments.Add(mAMPRef.ID, CType(Session("FileUpload.FileName"), String)) 'Added By Vikrant On 17-Apr-2013 For ALL17042013
                ' mAMPRef.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mAMPRef.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mAMPRef.FileAttachments.CurrentItem.Size = Session("Size")
                mAMPRef.FileAttachments.CurrentItem.Extension = Session("Extension")
                '   mAMPRef.FileAttachments.CurrentItem.SrNo = (mAMPRef.FileAttachments.Count - 1) + 1


                dgASPAttachment.DataSource = mAMPRef.FileAttachments
                dgASPAttachment.DataBind()

                For i As Integer = 0 To mAMPRef.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgASPAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mAMPRef.FileAttachments(i).FileName
                Next
                Session("mAMPRef") = mAMPRef
                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlAMPAttachment.Update()
                upnldgAMPAttachment.Update()

            Else
                Session("mAMPRef") = mAMPRef
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
            End If
        Catch ex As Exception
        Finally
        End Try
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
                If (Not mMPDRef.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mMPDRef.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub SaveAttachmentAMP() '
        If Not mFileAttachAMP Is Nothing Then
            If mFileAttachAMP.Size > 0 Then
                Try
                    mFileAttachAMP.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mAMPRef.IsNew) And IsAttachmentDeletedAMP Then
                    FileAttach.DeleteAttachment(mFileAttachAMP.ID, mAMPRef.ID)
                End If
                IsAttachmentDeletedAMP = False
                Session("IsAttachmentDeletedAMP") = IsAttachmentDeletedAMP
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" '& No.next.ToString

        If mMPDRef.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mMPDRef.ID)
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
    Private Sub ViewImageAMP()
        Dim No As New Random
        Dim StrName As String = "abc" '& No.next.ToString

        If mAMPRef.IsAttachmentAdded And mFileAttachAMP Is Nothing Then
            mFileAttachAMP = FileAttach.GetAttachment(mAMPRef.ID)
        End If
        If mFileAttachAMP.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttachAMP.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachAMP.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttachAMP.ImageFile, 0, mFileAttachAMP.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub SetPage()
        'If mMPDRef.IsNew Then
        '    lblStatus.Text = "OPEN"
        'Else
        '    lblStatus.Text = mMPDRef.StatusName
        'End If
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mMPDRef.FileAttachments.CurrentIndex = Index
        Session("mMPDRef") = mMPDRef
    End Sub
    Private Sub DeleteAttachmentAMP(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachmentAMP")
        mAMPRef.FileAttachments.CurrentIndex = Index
        Session("mAMPRef") = mAMPRef
    End Sub
    Private Sub controlvisibilityDateWiseFalse()
        If mMPDRef.IsRevised Then
            'txtMPDFromDate.ReadOnly = True
            txtMPDFromDate.Enabled = False
            txtMPDFromDate.BackColor = Color.Gainsboro
            txtMPDNo.ReadOnly = True
            txtMPDNo.BackColor = Color.Gainsboro
            txtMPDRevisionNo.ReadOnly = True
            txtMPDRevisionNo.BackColor = Color.Gainsboro
            txtModel.ReadOnly = True
            txtModel.BackColor = Color.Gainsboro
            btnMPDSelectFiles.Enabled = False
            btnMPDSave.Visible = False
            ' btnNewMPD.Visible = False
            For i As Integer = 0 To mMPDRef.FileAttachments.Count - 1
                Dim btnDel As ImageButton
                btnDel = CType(Me.dgMPDAttachment1.Rows(i).FindControl("RemoveRec"), ImageButton)
                btnDel.Visible = False
            Next
        Else
            'txtMPDFromDate.ReadOnly = False
            txtMPDFromDate.Enabled = True
            txtMPDFromDate.BackColor = Color.White
            txtMPDNo.ReadOnly = False
            txtMPDNo.BackColor = Color.White
            txtMPDRevisionNo.ReadOnly = False
            txtMPDRevisionNo.BackColor = Color.White
            txtModel.ReadOnly = False
            txtModel.BackColor = Color.White
            btnMPDSelectFiles.Enabled = True
            btnMPDSave.Visible = True
            ' btnNewMPD.Visible = True
            For i As Integer = 0 To mMPDRef.FileAttachments.Count - 1
                Dim btnDel As ImageButton
                btnDel = CType(Me.dgMPDAttachment1.Rows(i).FindControl("RemoveRec"), ImageButton)
                btnDel.Visible = True
            Next
        End If
    End Sub
    Private Sub controlvisibilityDateWiseFalseAMP()
        If mAMPRef.IsRevised Then
            'txtAMPFromDate.ReadOnly = True
            txtAMPFromDate.Enabled = False
            txtAMPFromDate.BackColor = Color.Gainsboro
            txtAmpNo.ReadOnly = True
            txtAmpNo.BackColor = Color.Gainsboro
            txtAMPRevisionNo.ReadOnly = True
            txtAMPRevisionNo.BackColor = Color.Gainsboro
            txtAMP.ReadOnly = True
            txtAMP.BackColor = Color.Gainsboro
            btnASPSelectFiles.Enabled = False
            btnAMPSave.Visible = False
            ' btnNewAMP.Visible = False
            For i As Integer = 0 To mAMPRef.FileAttachments.Count - 1
                Dim btnDel As ImageButton
                btnDel = CType(Me.dgASPAttachment.Rows(i).FindControl("Remove"), ImageButton)
                btnDel.Visible = False
            Next
        Else
            'txtAMPFromDate.ReadOnly = False
            txtAMPFromDate.Enabled = True
            txtAMPFromDate.BackColor = Color.White
            txtAmpNo.ReadOnly = False
            txtAmpNo.BackColor = Color.White
            txtAMPRevisionNo.ReadOnly = False
            txtAMPRevisionNo.BackColor = Color.White
            txtAMP.ReadOnly = False
            txtAMP.BackColor = Color.White
            btnASPSelectFiles.Enabled = True
            btnAMPSave.Visible = True
            ' btnNewAMP.Visible = True
            For i As Integer = 0 To mAMPRef.FileAttachments.Count - 1
                Dim btnDel As ImageButton
                btnDel = CType(Me.dgASPAttachment.Rows(i).FindControl("Remove"), ImageButton)
                btnDel.Visible = True
            Next
        End If
    End Sub
    Private Sub ControlVisibility()

        txtModel.Enabled = False
        txtAMP.Enabled = False
    End Sub
    Private Sub UpdatePanelMPD()
        upnldgMPDRefList.Update()
        upnlMPDDetails.Update()
    End Sub
    Private Sub UpdatePanelAMP()
        upnDgAMP.Update()
        upnAMPDetails.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()



        mMPDRefList = MPDAMPRefList.GetMPDAMPRefList_ForModel(mMachine.AssemblyStatus.Assembly.ModelName)
        dgMPDRefList.DataSource = mMPDRefList
        dgMPDRefList.DataBind()
        Session("mMPDRefList") = mMPDRefList



        txtModel.Text = mMachine.AssemblyStatus.Assembly.ModelName
        'txtMPDRevisionNo.Text = mMPDRef.RevNo
        'txtMPDNo.Text = mMPDRef.MPDNo

        'txtMPDFromDate.Text = mMPDRef.FromDateFormatted.ToString
        upnlMPDDetails.Update()
        If Not mMPDRef Is Nothing Then
            Session("mMPDRef") = mMPDRef
            dgMPDAttachment1.DataSource = mMPDRef.FileAttachments
            dgMPDAttachment1.DataBind()
            mLastMPDRef = LastMPDAMPRef.GetLastMPDAMPRefForModel(mMachine.AssemblyStatus.Assembly.ModelID, mMPDRef.ID.ToString)
            Session("mLastMPDRef") = mLastMPDRef
            If mLastMPDRef.RefNo <> "" And Not mMPDRef.IsNew Then
                If Not mLastMPDRef.ID.Equals(mMPDRef.ID) And CDate(Trim(txtMPDFromDate.Text)) <= CDate(mLastMPDRef.FromDateFormatted.ToString) Then
                    txtMPDFromDate.Enabled = False
                    txtModel.Enabled = False
                Else
                    txtMPDFromDate.Enabled = True
                    txtModel.Enabled = True
                End If
            End If

        End If

    End Sub
    Private Sub DataFieldBindAMP()


        mAMPRefList = MPDAMPRefList.GetMPDAMPRefList_ForMachine(mMachine.RegNo)
        dgAMP.DataSource = mAMPRefList
        dgAMP.DataBind()
        Session("mAMPRefList") = mAMPRefList


        txtAMP.Text = mMachine.RegNo

        upnAMPDetails.Update()

        If Not mAMPRef Is Nothing Then
            Session("mAMPRef") = mAMPRef
            dgASPAttachment.DataSource = mAMPRef.FileAttachments
            dgASPAttachment.DataBind()

            mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mMachine.ID, mAMPRef.ID.ToString)
            Session("mLastAMPRef") = mLastAMPRef

            If mLastAMPRef.RefNo <> "" And Not mAMPRef.IsNew Then
                If Not mLastAMPRef.ID.Equals(mAMPRef.ID) And CDate(Trim(txtAMPFromDate.Text)) <= CDate(mLastAMPRef.FromDateFormatted.ToString) Then
                    txtAMPFromDate.Enabled = False
                    txtAMP.Enabled = False
                Else
                    txtAMPFromDate.Enabled = True
                    txtAMP.Enabled = True
                End If
            End If

        End If

    End Sub
    Private Sub GridBind()
        dgMPDRefList.DataSource = mMPDRefList
        dgMPDRefList.DataBind()
        '  SetGrid()
        upnldgMPDRefList.Update()
    End Sub
    Private Sub GridBindAMP()
        dgAMP.DataSource = mAMPRefList
        dgAMP.DataBind()
        '  SetGrid()
        upnDgAMP.Update()
    End Sub
    Private Sub NewModelRecord()
        mMPDRef = MPDAMPRef.NewMPDRef(mMachine.AssemblyStatus.Assembly.ModelID)
        txtMPDRevisionNo.Text = mMPDRef.RevNo
        txtMPDNo.Text = mMPDRef.MPDNo

        txtMPDFromDate.Text = mMPDRef.FromDateFormatted.ToString
        Session("mMPDRef") = mMPDRef
    End Sub
    Private Sub NewAMPRecord()
        mAMPRef = MPDAMPRef.NewAMPRef(mMachine.ID)
        txtAMPRevisionNo.Text = mAMPRef.RevNo
        txtAmpNo.Text = mAMPRef.AMPNo

        txtAMPFromDate.Text = mAMPRef.FromDateFormatted.ToString
        Session("mAMPRef") = mAMPRef
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mMPDRef = MPDAMPRef.GetMPDAMPRef(mId)
        Session("mMPDRef") = mMPDRef
    End Sub
    Private Sub EditRecordAMP(ByVal mId As Guid)
        mAMPRef = MPDAMPRef.GetMPDAMPRef(mId)
        Session("mAMPRef") = mAMPRef
    End Sub

    Private Sub DeleteRecord(ByVal mId As Guid)

        mMPDRef = MPDAMPRef.GetMPDAMPRef(mId)
        Session("mMPDRef") = mMPDRef
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        controlvisibilityDateWiseFalse()
        ' UpdatePanelMPD()
    End Sub
    Private Sub DeleteRecordAMP(ByVal mId As Guid)

        mAMPRef = MPDAMPRef.GetMPDAMPRef(mId)
        Session("mAMPRef") = mAMPRef
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteAMP")
        controlvisibilityDateWiseFalseAMP()
        ' DataFieldBindAMP()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtModel.Focus()
            '' TxtAMP.Focus()
            'If mMPDRef Is Nothing Then
            mMPDRef = MPDAMPRef.NewMPDRef(mMachine.AssemblyStatus.Assembly.ModelID)
            Session("mMPDRef") = mMPDRef
            'End If
            'If mAMPRef Is Nothing Then
            mAMPRef = MPDAMPRef.NewAMPRef(mMachine.ID)
            Session("mAMPRef") = mAMPRef
            'End If
            DataFieldBind()
            DataFieldBindAMP()
            SetPage()
            ControlVisibility()
            controlvisibilityDateWiseFalse()
            controlvisibilityDateWiseFalseAMP()

        End If


        ''  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub btnMPDSave_Click(sender As Object, e As System.EventArgs) Handles btnMPDSave.Click
        If IsValid Then

            setMPDObject()

            If Not CustomValidate() = True Then upnlValidationSummary.Update() : Exit Sub



            If mMPDRef.IsDirty Then
                If SaveMPD() Then
                    NewModelRecord()
                    DataFieldBind()
                    SaveAttachment()
                    UpdatePanelMPD()
                    upnlGridMPDAttachment.Update()
                    SetPage()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                End If
            End If
        Else
            upnlValidationSummary.Update()
        End If

        UpdatePanelMPD()
        upnlGridMPDAttachment.Update()
    End Sub
    Private Sub btnAMPSave_Click(sender As Object, e As System.EventArgs) Handles btnAMPSave.Click
        If IsValid Then

            setAMPObject()

            If Not CustomValidateAMP() = True Then upnlValidationSummary1.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True) : Exit Sub

            'For i As Integer = 0 To mAMPRef.FileAttachments.Count - 1
            '    Dim txtValue As TextBox
            '    txtValue = CType(Me.dgMPDAttachment1.Rows(i).FindControl("txtFileName"), TextBox)
            '    mAMPRef.FileAttachments(i).FileName = txtValue.Text.Trim
            'Next 

            If mAMPRef.IsDirty Then
                If SaveAMP() Then
                    NewAMPRecord()
                    DataFieldBindAMP()
                    SaveAttachment()
                    UpdatePanelAMP()
                    upnlGridMPDAttachment.Update()
                    SetPage()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                End If
            End If
        Else
            upnlValidationSummary1.Update()
        End If

        UpdatePanelAMP()
        upnldgAMPAttachment.Update()
    End Sub
    Private Sub SetGridView()
        dgMPDRefList.DataSource = mMPDRefList
        dgMPDRefList.DataBind()
        ' SetGrid()
    End Sub
    Private Sub SetGridViewAMP()
        dgAMP.DataSource = mAMPRefList
        dgAMP.DataBind()
        ' SetGrid()
    End Sub
    Private Sub dgMPDRefList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMPDRefList.RowCommand
        Dim mId As Guid
        Select Case e.CommandName
            Case "ViewRec"
                mId = New Guid(e.CommandArgument.ToString)
                EditRecord(mId)
                txtMPDNo.Text = mMPDRef.MPDNo
                txtMPDRevisionNo.Text = mMPDRef.RevNo
                txtMPDFromDate.Text = mMPDRef.FromDateFormatted

                dgMPDAttachment1.DataSource = mMPDRef.FileAttachments
                dgMPDAttachment1.DataBind()
                controlvisibilityDateWiseFalse()
                upnlMPDDetails.Update()
                upnlGridMPDAttachment.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
            Case "Remove"
                mId = New Guid(e.CommandArgument.ToString)
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgAMP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAMP.RowCommand
        Dim mId As Guid
        Select Case e.CommandName
            Case "ViewRec"
                mId = New Guid(e.CommandArgument.ToString)
                EditRecordAMP(mId)
                txtAmpNo.Text = mAMPRef.AMPNo
                txtAMPRevisionNo.Text = mAMPRef.RevNo
                txtAMPFromDate.Text = mAMPRef.FromDateFormatted

                dgASPAttachment.DataSource = mAMPRef.FileAttachments
                dgASPAttachment.DataBind()
                controlvisibilityDateWiseFalseAMP()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                upnldgAMPAttachment.Update()
                upnAMPDetails.Update()

            Case "Remove"
                mId = New Guid(e.CommandArgument.ToString)
                DeleteRecordAMP(mId)
        End Select
    End Sub
    Private Sub dgMPDAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMPDAttachment1.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgMPDAttachment1.PageSize * dgMPDAttachment1.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" '& No.next.ToString
                mFileAttachments = mMPDRef.FileAttachments
                mFileAttachments.CurrentIndex = Index - 1
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
                dgMPDAttachment1.DataSource = mMPDRef.FileAttachments
                dgMPDAttachment1.DataBind()

                upnlGridMPDAttachment.Update()
                controlvisibilityDateWiseFalse()
            Case "RemoveRec"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgMPDAttachment1.PageSize * dgMPDAttachment1.PageIndex

                DeleteAttachment(Index - 1)
        End Select

    End Sub
    Private Sub dgASPAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgASPAttachment.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgMPDAttachment1.PageSize * dgMPDAttachment1.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" '& No.next.ToString
                mFileAttachments = mAMPRef.FileAttachments
                mFileAttachments.CurrentIndex = Index - 1
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
                dgASPAttachment.DataSource = mAMPRef.FileAttachments
                dgASPAttachment.DataBind()
                upnlAMPAttachment.Update()
                upnldgAMPAttachment.Update()
                controlvisibilityDateWiseFalseAMP()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgMPDAttachment1.PageSize * dgMPDAttachment1.PageIndex

                DeleteAttachmentAMP(Index - 1)
        End Select

    End Sub
    Private Sub btnMPDCloseTop_Click(sender As Object, e As System.EventArgs) Handles btnMPDCloseTop.Click
        setMPDObject()
        If mMPDRef.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Save")
            Exit Sub
        Else
            RemoveSession()
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
            Response.Redirect("index.aspx")
        End If
    End Sub

    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If Session("OpenAttachementForMPD") = "True" Then
            AttachFile()
            Session.Remove("OpenAttachementForMPD")
        ElseIf Session("OpenAttachementForAMP") = "True" Then
            AttachFileAMP()
            Session.Remove("OpenAttachementForAMP")
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnMPDSelectFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMPDSelectFiles.Click
        Session("OpenAttachementForMPD") = "True"
        Session("OpenAttachementForAMP") = "False"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub btnASPSelectFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnASPSelectFiles.Click
        Session("OpenAttachementForMPD") = "False"
        Session("OpenAttachementForAMP") = "True"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub btnNewMPD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewMPD.Click
        NewModelRecord()

        DataFieldBind()
        controlvisibilityDateWiseFalse()
    End Sub
    Private Sub btnNewAMP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewAMP.Click
        NewAMPRecord()

        DataFieldBindAMP()
        controlvisibilityDateWiseFalseAMP()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        'MarkLog(Util.Action.Close, "Machine", "", Util.ErrorType.NoError, Guid.Empty)
        RemoveSession()

        '  Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
#End Region

End Class