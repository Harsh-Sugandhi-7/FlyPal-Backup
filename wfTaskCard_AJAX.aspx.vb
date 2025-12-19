Imports System.Web.Services

'AJAX Conversion By : Saylee on 12-Sep-2014

Public Class wfTaskCard_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mTaskCard As TaskCard
    Protected mTaskCardList As TaskCardList
    Protected mTaskStep As TaskStep
    Dim mflag As Integer
    Dim Content As Integer
    'Added by Vikrant on 20-July-2011
    Dim EventLogID As Guid
    Public mATAList As ATAList 'Added By Shweta

    Public mModelList As ModelList 'Added By Saylee on 7-Nov-2013 for ALL07112013
#End Region

#Region "Methods"
    Private Sub GetSession()

        mflag = CInt(Session("flag"))

        mModelList = Session("mModelList") 'Added By Saylee on 7-Nov-2013 for ALL07112013
    End Sub
    Public Sub settitle()
        If mTaskCard.IsNew Then
            lblTitle.Text = "Task Card [New]"
        Else
            lblTitle.Text = "Task Card Detail " & "[" & mTaskCard.TaskCardNo & "]"
        End If
    End Sub
    Private Sub ControlVisibility()
        btnAddWorkSpares.Visible = IIf(dgTaskCardSteps.Rows.Count = 0, False, True)
        lblTaskCardWorkSpares.Visible = IIf(dgTaskCardSteps.Rows.Count = 0, False, True)
        dgTaskCardWorkSpares.Visible = IIf(dgTaskCardSteps.Rows.Count = 0, False, True)

        'Added By Saylee ON 4-Feb-2013 for BA04022013
        If (AppSettings("ClientCode") = "BSA") Then 'Added By Saylee On 15-Oct-2014 For BSA15102014
            lblRelatedTaskCardNo.Text = "Other References"
        Else
            lblRelatedTaskCardNo.Text = "Related Task Card No."
        End If
    End Sub
    Private Sub AttachFile()
        '  If MyFile1.Value <> "" Then
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"

        Try
            If Not mTaskCard.TaskCardAttachments.Contains(mTaskCard.ID, CType(Session("FileUpload.FileName"), String)) Then

                mTaskCard.TaskCardAttachments.Add(mTaskCard.ID, CType(Session("FileUpload.FileName"), String)) 'Added By Vikrant On 17-Apr-2013 For ALL17042013
                mTaskCard.TaskCardAttachments.CurrentItem.FileName = Session("FileUpload.FileName")
                mTaskCard.TaskCardAttachments.CurrentItem.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
                mTaskCard.TaskCardAttachments.CurrentItem.ImageSize = Session("FileUpload.FileSize")
                mTaskCard.TaskCardAttachments.CurrentItem.FileExtension = Session("FileUpload.FileExtension")

                Session("mTaskCard") = mTaskCard
                dgTaskCardAttachment.DataSource = mTaskCard.TaskCardAttachments
                dgTaskCardAttachment.DataBind()

                For i As Integer = 0 To mTaskCard.TaskCardAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgTaskCardAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mTaskCard.TaskCardAttachments(i).FileName
                Next

                Session.Remove("FileUpload.FileSize")
                Session.Remove("FileUpload.FileContent")
                Session.Remove("FileUpload.FileExtension")
                Session.Remove("FileUpload.FileName")
                GetTaskCardChilds()
                upnlAttachFile.Update()
                upnldgTaskCardAttachment.Update()
            Else
                Session("mTaskCard") = mTaskCard
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub DisableName(mID As Guid) 'Added by : Saylee 17-Jun-2020, ALL16062020
        'Commented by Vikrant On 26-Aug-2021 as per BA requirements,discussed with Sir,Abhijit
        'Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerTaskCard(mID)
        'If Not mTransCountAsPerMasters Is Nothing Then
        '    txtCardNo.Enabled = mTransCountAsPerMasters.Count = 0
        'End If
        'End
    End Sub
#End Region

#Region " Page Load "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Set the Task Card currently available.
        GetSession()
        addAttributes() 'Added By Shweta

        mTaskCardList = Session("mTaskCardList")
        mTaskCard = Session("wfTaskCard.TaskCard")
        ' new Added by Vikrant on 20-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)


        If Not Page.IsPostBack Then
            setFocus(txtCardNo)
            If mTaskCard Is Nothing Then
                setFocus(txtCardNo)
                If New Guid(CType(Session("ID"), String)).Equals(Guid.Empty) Then
                    lblTitle.Text = "Task Card [New]"
                    mTaskCard = TaskCard.NewTaskCard()
                    MarkLog(Util.Action.[New], "TaskCard", "", Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
                    Session("wfTaskCard.TaskCard") = mTaskCard
                    btnRefresh.Visible = False 'Added by Ajay on 12-09-23
                Else
                    mTaskCard = TaskCard.GetTaskCard(New Guid(CType(Session("ID"), String)))
                    settitle()
                    DisableName(mTaskCard.ID)
                    MarkLog(Util.Action.Edit, "TaskCard", "Task Card No. : " + mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
                    Session("wfTaskCard.TaskCard") = mTaskCard

                End If
                mflag = 0
            End If
            GetTaskCardChilds()
            DataFieldBind()
            Try
                settitle()
                dgTaskCardSteps.DataBind()
                'DataBind()
            Catch ex As Exception

            End Try
            DataFieldBind1()
            ControlVisibility()

        End If

    End Sub

#End Region

#Region " Helper Methods "
    Private Sub DataFieldBind1()
        txtRevDate.Text = mTaskCard.RevDate
        txtIssueDate.Text = mTaskCard.IssueDate
    End Sub
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "<SELECT>")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList

        'Added By Saylee on 7-Nov-2013 for ALL07112013
        mModelList = ModelList.GetModelList(0, "", , , "(SELECT)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList
        DataBind()
        'End
    End Sub
    Private Sub SaveFormtoObject()
        If mTaskCard.TaskCardNo = "" Then
        Else
            lblTitle.Text = "Task Card Detail " & "[" & mTaskCard.TaskCardNo & "]"
        End If
        mTaskCard.TaskCardNo = txtCardNo.Text
        mTaskCard.TaskDesc = txtDescription.Text
        mTaskCard.RevNo = txtRevNo.Text
        If txtRevDate.Text = "" Then
            mTaskCard.RevDate = ""
        Else
            mTaskCard.RevDate = Format(CDate(txtRevDate.Text), AppSettings("DateFormat"))
        End If
        If txtIssueDate.Text = "" Then
            mTaskCard.IssueDate = ""
        Else
            mTaskCard.IssueDate = Format(CDate(txtIssueDate.Text), AppSettings("DateFormat"))
        End If
        mTaskCard.Reference = txtReference.Text
        mTaskCard.Equipment = txtEquipment.Text
        mTaskCard.Material = txtMaterial.Text
        mTaskCard.EstimatedHours = txtEstimatedHr.Text
        mTaskCard.Check = txtCheck.Text
        mTaskCard.RelatedTaskCardsNo = txtRelatedTaskCardNo.Text
        'Added by Shweta on 11-Jan-2013 
        mTaskCard.AMPIssueRev = txtAMPIssueRev.Text
        mTaskCard.INSPTypeInterval = txtINSPTypeInterval.Text
        mTaskCard.Zone = txtZone.Text
        mTaskCard.Area = txtArea.Text
        mTaskCard.Category = txtCategory.Text
        mTaskCard.ATAChapterID = New Guid(cmbATAChapter.SelectedValue.ToString)
        'Added By Shweta  on 18-Jan-2012 for   BA17012013
        mTaskCard.IsRII = chkIsRII.Checked 'If Checked IsRII=True
        mTaskCard.InspCode = txtInspCode.Text
        mTaskCard.Publication = txtPublication.Text
        mTaskCard.Skill = txtSkill.Text
        'End
        mTaskCard.Panels = txtPanels.Text
        'Added By Shweta  on 15-March-2013 for  BA14032013d
        mTaskCard.TaskHeading = txtHeading.Text
        mTaskCard.TaskSubject = txtSubject.Text
        '
        mTaskCard.ModelID = New Guid(cmbModel.SelectedValue.ToString) 'Added By Saylee on 7-Nov-2013 for ALL07112013

        mTaskCard.Remark = Trim(txtRemark.Text) 'Added By Saylee on 7-Feb-2013 for BA28032014-1

        'Added By Vikrant On 23-Apr-2013 For ALL17042013
        For i As Integer = 0 To mTaskCard.TaskCardAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgTaskCardAttachment.Rows(i).FindControl("txtFileName"), TextBox)
            mTaskCard.TaskCardAttachments(i).FileName = txtValue.Text.Trim
        Next
        'End

        mTaskCard.TallySequenceNo = txtTallySequenceNo.Text
        mTaskCard.CMR = chkCMR.Checked
        mTaskCard.CPCP = chkCPCP.Checked
        mTaskCard.CDCCL = chkCDCCL.Checked
        mTaskCard.AD = chkAD.Checked
        mTaskCard.ALI = chkALI.Checked
        mTaskCard.ETO = chkETO.Checked

        Session("mTaskCard") = mTaskCard 'Added By Vikrant On 22-Jan-2013 For ALL21012013

        mTaskCard.AccessHours = txtAccessHr.Text '- Ajay- 09-Jan-2023
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetTaskCardChilds()
        If Not mTaskCard.TaskSteps Is Nothing Then
            dgTaskCardSteps.DataSource = mTaskCard.TaskSteps
            'upnldgTaskCardSteps.Update()
        End If
        'Added By Vikrant On 22-Jan-2013 For ALL21012013
        If Not mTaskCard.TaskCardTools Is Nothing Then
            dgTaskCardTools.DataSource = mTaskCard.TaskCardTools
            'upnldgTaskCardTools.Update()
        End If
        'End
        'Added By Shweta On 23-Jan-2013 For ALL21012013
        If Not mTaskCard.TaskCardSpares Is Nothing Then
            dgTaskCardSpares.DataSource = mTaskCard.TaskCardSpares
            'upnldgTaskCardSpares.Update()
        End If
        'End
        'Added By Vikrant On 17-Apr-2013 For ALL17042013
        If Not mTaskCard.TaskCardAttachments Is Nothing Then
            dgTaskCardAttachment.DataSource = mTaskCard.TaskCardAttachments
            'upnldgTaskCardAttachment.Update()
        End If
        'End

        'Added By Shweta On 5-Sep-2013 For BA04092013
        If Not mTaskCard.TaskCardStepsSpares Is Nothing Then
            dgTaskCardWorkSpares.DataSource = mTaskCard.TaskCardStepsSpares
            'upnldgTaskCardWorkSpares.Update()
        End If
        'End

        'Added by Shital on 18-Aug-2016
        If Not mTaskCard.TaskCardSkills Is Nothing Then
            gdSkillList.DataSource = mTaskCard.TaskCardSkills
        End If
        'Added By Vikrant on 03-Mar-2020 For ALL03032020
        If Not mTaskCard.TaskCardPartRemovals Is Nothing Then
            dgTaskCardPartRemovals.DataSource = mTaskCard.TaskCardPartRemovals
        End If
        'End
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "RemoveStep" Then
                        Session("Sender") = ""
                        'Added By Vikrant On 02-Jan-2014 For All02012014
                        Dim Description As String = mTaskCard.TaskSteps.CurrentItem.Description
                        'End
                        mTaskCard.TaskSteps.Remove(mTaskCard.TaskSteps.CurrentItem)
                        'Added By Vikrant On 02-Jan-2014 For All02012014
                        Try
                            mTaskCard.Save()
                            MarkLog(Util.Action.Delete, "TaskCard", "Additional Work : " + Chr(13) + "Description : " + Description, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID)
                        Catch ex As Exception
                            MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
                        End Try
                        'End
                        'dgTaskCardSteps.DataSource = mTaskCard.TaskSteps
                        GetTaskCardChilds()
                        DataBind()
                        upnldgTaskCardSteps.Update()
                        ControlVisibility()
                        upnldgTaskCardSteps.Update()
                        upnldgTaskCardWorkSpares.Update()
                        upnlAddWorkSpares.Update()

                        'Added by Shital on 18-Aug-2016
                        'upnldgSkillList.Update()
                        '-----------

                        Session("wfTaskCard.TaskCard") = mTaskCard
                        'Response.Redirect("wfTaskCard.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                        'Added By Vikrant On 22-Jan-2013 For ALL21012013
                    ElseIf MSGBoxCtrl.Sender = "RemoveTool" Then
                        Try
                            Session("Sender") = ""
                            Dim mTaskCard As TaskCard
                            mTaskCard = CType(Session("mTaskCard"), TaskCard)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Dim PartNo As String = mTaskCard.TaskCardTools.CurrentItem.PartNo
                            Dim Description As String = mTaskCard.TaskCardTools.CurrentItem.Description
                            Dim Qty As String = mTaskCard.TaskCardTools.CurrentItem.RequiredQty.ToString
                            'End
                            mTaskCard.TaskCardTools.Remove(mTaskCard.TaskCardTools.CurrentItem)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Try
                                mTaskCard.Save()
                                MarkLog(Util.Action.Delete, "TaskCard", "Task Card Tool : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Description + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID)
                            Catch ex As Exception
                                MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
                            End Try
                            'End
                            'dgTaskCardTools.DataSource = mTaskCard.TaskCardTools
                            GetTaskCardChilds()
                            DataBind()
                            upnldgTaskCardTools.Update()
                            Session("mTaskCard") = mTaskCard
                            'Response.Redirect("wfTaskCard.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                        'End
                        'Added By Shweta On 23-Jan-2013 For ALL21012013
                    ElseIf MSGBoxCtrl.Sender = "RemoveSpare" Then
                        Try
                            Session("Sender") = ""
                            Dim mTaskCard As TaskCard
                            mTaskCard = CType(Session("mTaskCard"), TaskCard)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Dim PartNo As String = mTaskCard.TaskCardSpares.CurrentItem.PartNo
                            Dim Description As String = mTaskCard.TaskCardSpares.CurrentItem.Description
                            Dim Qty As String = mTaskCard.TaskCardSpares.CurrentItem.RequiredQty.ToString
                            'End
                            mTaskCard.TaskCardSpares.Remove(mTaskCard.TaskCardSpares.CurrentItem)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Try
                                mTaskCard.Save()
                                MarkLog(Util.Action.Delete, "TaskCard", "Task Card Spare : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Description + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID)
                            Catch ex As Exception
                                MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
                            End Try
                            'End
                            'dgTaskCardSpares.DataSource = mTaskCard.TaskCardSpares
                            GetTaskCardChilds()
                            DataBind()
                            upnldgTaskCardSpares.Update()
                            Session("mTaskCard") = mTaskCard
                            'Response.Redirect("wfTaskCard.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                        'End

                        'Added By Shweta On 5-Sep-2013 For BA04092013
                    ElseIf MSGBoxCtrl.Sender = "RemoveStepSpare" Then
                        Try
                            Session("Sender") = ""
                            Dim mTaskCard As TaskCard
                            mTaskCard = CType(Session("mTaskCard"), TaskCard)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Dim PartNo As String = mTaskCard.TaskCardStepsSpares.CurrentItem.PartNo
                            Dim Description As String = mTaskCard.TaskCardStepsSpares.CurrentItem.Description
                            Dim Qty As String = mTaskCard.TaskCardStepsSpares.CurrentItem.RequiredQty.ToString
                            'End
                            mTaskCard.TaskCardStepsSpares.Remove(mTaskCard.TaskCardStepsSpares.CurrentItem)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Try
                                mTaskCard.Save()
                                MarkLog(Util.Action.Delete, "TaskCard", "Additional Work Spare : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Description + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID)
                            Catch ex As Exception
                                MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
                            End Try
                            'End
                            'dgTaskCardWorkSpares.DataSource = mTaskCard.TaskCardStepsSpares
                            GetTaskCardChilds()
                            DataBind()
                            upnldgTaskCardWorkSpares.Update()
                            Session("mTaskCard") = mTaskCard
                            'Response.Redirect("wfTaskCard.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                        'End
                        'Added By Vikrant On 17-Apr-2013 For ALL17042013
                    ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mTaskCard As TaskCard
                            mTaskCard = CType(Session("mTaskCard"), TaskCard)
                            mTaskCard.TaskCardAttachments.Remove(mTaskCard.TaskCardAttachments.CurrentItem)
                            'dgTaskCardAttachment.DataSource = mTaskCard.TaskCardAttachments
                            GetTaskCardChilds()
                            DataBind()
                            upnldgTaskCardAttachment.Update()
                            Session("mTaskCard") = mTaskCard
                            'Response.Redirect("wfTaskCard.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                        'End


                        'Added by Shital on 18-Aug-2016
                    ElseIf MSGBoxCtrl.Sender = "RemoveSkill" Then
                        Session("Sender") = ""

                        Dim SkillName As String = mTaskCard.TaskCardSkills.CurrentItem.SkillName

                        mTaskCard.TaskCardSkills.Remove(mTaskCard.TaskCardSkills.CurrentItem)

                        Try
                            mTaskCard.Save()
                            MarkLog(Util.Action.Delete, "TaskCard", "Task Card Skill : " + Chr(13) + "SkillName : " + SkillName, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID)
                        Catch ex As Exception
                            MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
                        End Try

                        GetTaskCardChilds()
                        DataBind()
                        upnldgSkillList.Update()
                        ControlVisibility()
                        Session("wfTaskCard.TaskCard") = mTaskCard
                        '--------------------
                        'Added By Vikrant on 03-Mar-2020 For ALL03032020
                    ElseIf MSGBoxCtrl.Sender = "RemovePartRemoval" Then
                        Try
                            Session("Sender") = ""
                            Dim mTaskCard As TaskCard
                            mTaskCard = CType(Session("mTaskCard"), TaskCard)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Dim PartNo As String = mTaskCard.TaskCardPartRemovals.CurrentItem.PartNo
                            Dim Description As String = mTaskCard.TaskCardPartRemovals.CurrentItem.Description
                            Dim Qty As String = mTaskCard.TaskCardPartRemovals.CurrentItem.RequiredQty.ToString
                            'End
                            mTaskCard.TaskCardPartRemovals.Remove(mTaskCard.TaskCardPartRemovals.CurrentItem)
                            'Added By Vikrant On 02-Jan-2014 For All02012014
                            Try
                                mTaskCard.Save()
                                MarkLog(Util.Action.Delete, "TaskCard", "Task Card Part Removal : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Description + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID)
                            Catch ex As Exception
                                MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
                            End Try
                            'End
                            'dgTaskCardSpares.DataSource = mTaskCard.TaskCardSpares
                            GetTaskCardChilds()
                            DataBind()
                            upnldgTaskCardPartRemovals.Update()
                            Session("mTaskCard") = mTaskCard
                            'Response.Redirect("wfTaskCard.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                        'End
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        'Added Code
                        Session("sender") = ""
                        Session.Remove("IsValid")
                        mTaskCard = Session("wfTaskCard.TaskCard")
                        SaveFormtoObject()

                        'Added By Vikrant On 06-Jan-2014 For All02012014
                        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
                            SaveFormtoObject()
                            MarkLog(Util.Action.Save, "TaskCard", User.Identity.Name & " is not Authorized User to save " & mTaskCard.TaskCardNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                            Exit Sub
                        End If
                        'End
                        mTaskCard.ApplyEdit()

                        If mTaskCard.IsSavable Then
                            Try
                                mTaskCard = mTaskCard.Save
                                'Added by Vikrant On 21-july-2011
                                MarkLog(Util.Action.Save, "TaskCard", "TaskCard No : " + mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)

                                Session("wfTaskCard.TaskCard") = mTaskCard
                                GetTaskCardChilds()
                                DataBind()
                                ControlVisibility()
                                Session("wfTaskCard.TaskCard") = mTaskCard
                                Session("mTaskCardList") = mTaskCardList
                                Session.Remove("wfTaskCard.TaskCard")
                                upnldgTaskCardSteps.Update()
                                upnldgTaskCardWorkSpares.Update()
                                upnlAddWorkSpares.Update()


                                Dim mopenas As String = Request.QueryString("Type")
                                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                    Exit Sub
                                End If

                                Response.Redirect(BackPage.Pop(Session("TaskBackPage")) & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                                ' End If

                            Catch ex As Exception
                                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry in Task Card.", MsgBoxStyle.OkOnly, "")
                            End Try
                        Else
                            cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
                            cvControlValidator.IsValid = mTaskCard.IsSavable
                            upnlValidationSummary.Update()
                        End If

                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "RemoveStep" Then
                        Session("Sender") = ""
                        Session("wfTaskCard.TaskCard") = mTaskCard
                    ElseIf MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        If mTaskCard.IsNew Then
                            Session.Remove("wfTaskCard.TaskCard")
                        End If
                        Session("Sender") = ""
                        Session.Remove("wfTaskCard.TaskCard")
                        Session("mTaskCardList") = mTaskCardList

                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        Response.Redirect(BackPage.Pop(Session("TaskBackPage")) & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                    End If
                    Session("Sender") = ""
                    GetTaskCardChilds()
                    DataBind()
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    mTaskCard = Session("wfTaskCard.TaskCard")
                    'Added By Vikrant On 02-Jan-2014 For All02012014
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfTaskCard.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                    'End
            End Select
        End If
    End Sub
    Private Sub Deletestep(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "RemoveStep")
        mTaskCard.TaskSteps.CurrentIndex = Index
        Session("wfTaskCard.TaskCard") = mTaskCard
    End Sub
    'Added By Vikrant On 22-Jan-2013 For ALL21012013
    Private Sub DeleteToolRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveTool")
        mTaskCard.TaskCardTools.CurrentIndex = Index
        Session("mTaskCard") = mTaskCard
    End Sub
    'End
    'Added By Shweta On 23-Jan-2013 For ALL21012013
    Private Sub DeleteSpareRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveSpare")
        mTaskCard.TaskCardSpares.CurrentIndex = Index
        Session("mTaskCard") = mTaskCard
    End Sub
    'Added By Vikrant On 17-Apr-2013 For ALL17042013
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mTaskCard.TaskCardAttachments.CurrentIndex = Index
        Session("mTaskCard") = mTaskCard
    End Sub
    'End
    'Added By Shweta On 5-Sep-2013 For BA04092013
    Private Sub DeleteStepSpareRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveStepSpare")
        mTaskCard.TaskCardStepsSpares.CurrentIndex = Index
        Session("mTaskCard") = mTaskCard
    End Sub
    Private Sub addAttributes()
        '    txtEstimatedHr.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEstimatedHr').value)")
        'Commented and Added By Saylee on 21-Feb-2013
        If (Not AppSettings("nWOShowHrsInDecimal") Is Nothing) AndAlso (AppSettings("nWOShowHrsInDecimal") = "True") Then
            txtEstimatedHr.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEstimatedHr').value,event)")
        Else
            txtEstimatedHr.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEstimatedHr').value,event)")
        End If
        'Ajay 09-Jan-2023
        If (Not AppSettings("nWOShowHrsInDecimal") Is Nothing) AndAlso (AppSettings("nWOShowHrsInDecimal") = "True") Then
            txtAccessHr.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAccessHr').value,event)")
        Else
            txtAccessHr.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtAccessHr').value,event)")
        End If

    End Sub
    'End

    'Added by Shital on 18-Aug-2016
    Private Sub DeleteSkillRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveSkill")
        mTaskCard.TaskCardSkills.CurrentIndex = Index
        Session("mTaskCard") = mTaskCard
    End Sub
    'Added By Vikrant on 03-Mar-2020 For ALL03032020
    Private Sub DeletePartRemovalRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemovePartRemoval")
        mTaskCard.TaskCardPartRemovals.CurrentIndex = Index
        Session("mTaskCard") = mTaskCard
    End Sub
    'End
#End Region

#Region " Events "

    Private Sub dgTaskCardSteps_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTaskCardSteps.PageIndexChanging
        dgTaskCardSteps.PageIndex = e.NewPageIndex
        dgTaskCardSteps.DataSource = mTaskCard.TaskSteps
        Session("mTaskCard") = mTaskCard
        dgTaskCardSteps.DataBind()
    End Sub
    Private Sub dgTaskCardSteps_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskCardSteps.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardSteps.PageSize * dgTaskCardSteps.PageIndex
                Dim mId As Guid = mTaskCard.TaskSteps(Index).ID
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardView") And Not User.IsInRole("TaskCardEdit")) Then
                    MarkLog(Util.Action.Edit, "TaskCard", User.Identity.Name & " is not Authorized User to edit Additional Work : " & mTaskCard.TaskSteps(Index).Description, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End

                Session("StepEdit") = True
                SaveFormtoObject()
                mTaskCard.TaskSteps.CurrentIndex = Index
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardWorkSpares.Update()
                upnlAddWorkSpares.Update()
                Session("wfTaskCard.TaskCard") = mTaskCard
                Session("mTaskCardListt") = mTaskCardList
                BackPage.Push(Session("BackPage1"), "wfTaskCard_AJAX.aspx")
                MarkLog(Util.Action.Edit, "TaskCard", "Additional Work : " + Chr(13) + "Description : " + mTaskCard.TaskSteps(Index).Description, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID) 'Added By Vikrant On 02-Jan-2014 For All02012014
                'Response.Redirect("wfTaskCardStep.aspx?Index=" & Index & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardStepWindow", "OpenTaskCardStepWindow()", True)
                'Added by Saylee on 5-sep-2013
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardSteps.PageSize * dgTaskCardSteps.PageIndex
                Dim mId As Guid = mTaskCard.TaskSteps(Index).ID
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardDelete")) Then
                    MarkLog(Util.Action.Delete, "TaskCard", User.Identity.Name & " is not Authorized User to delete Additional Work : " & mTaskCard.TaskSteps(Index).Description, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                If Index = 0 And mTaskCard.TaskSteps.Count = 1 And mTaskCard.TaskCardStepsSpares.Count > 0 Then
                    MSGBoxCtrl.show("Alert!", "You cannot delete this record,as its last record and there are additional spares. You need to first delete additional spares.", "", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If
                'end
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardWorkSpares.Update()
                upnlAddWorkSpares.Update()
                Deletestep(Index)
        End Select
    End Sub
    Private Sub btnAddStep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddStep.Click
        'Added By Vikrant On 02-Jan-2014 For All02012014
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        SaveFormtoObject()
        Dim mIsValid As Boolean = mTaskCard.IsSavable
        'End

        If mTaskCard.IsValid Then
            SaveFormtoObject()
            mTaskCard.TaskSteps.Add(mTaskCard.ID)
            Session("wfTaskCard.TaskCard") = mTaskCard
            Session("StepEdit") = False
            BackPage.Push(Session("BackPage1"), "wfTaskCard_Ajax.aspx")
            'Response.Redirect("wfTaskCardStep.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=wfTaskCard.aspx" & "&Index=-1")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardStepWindow", "OpenTaskCardStepWindow()", True)
        Else 'Added By Vikrant On 02-Jan-2014 For All02012014
            cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
            cvControlValidator.IsValid = mTaskCard.IsSavable
            upnlValidationSummary.Update()
        End If
        upnlValidationSummary.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        mTaskCard = Session("wfTaskCard.TaskCard")
        SaveFormtoObject()
        'Added By Vikrant On 02-Jan-2014 For All02012014
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            SaveFormtoObject()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        'End
        Try
            If mTaskCard.IsSavable Then
                mTaskCard.ApplyEdit()
                mTaskCard.Save()
                MarkLog(Util.Action.Save, "TaskCard", "TaskCard No : " & mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
                Session("wfTaskCard.TaskCard") = mTaskCard
                Session("mTaskCardList") = mTaskCardList
                GetTaskCardChilds()
                DataBind()
                settitle()
                upnlTitle.Update()
                ControlVisibility()
                upnlValidationSummary.Update()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardWorkSpares.Update()
                upnlAddWorkSpares.Update()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Else
                cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
                cvControlValidator.IsValid = mTaskCard.IsSavable
                upnlValidationSummary.Update()
            End If
        Catch ex As Exception
            MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry in Task Card.", MsgBoxStyle.OkOnly, "")
        End Try

    End Sub
    Private Sub btnSaveNew_Click(sender As Object, e As System.EventArgs) Handles btnSaveNew.Click
        mTaskCard = Session("wfTaskCard.TaskCard")

        btnRefresh.Visible = False 'Added by Ajay on 12-09-23
        UpdatePanel2.Update()

        SaveFormtoObject()
        'Added By Vikrant On 02-Jan-2014 For All02012014
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            SaveFormtoObject()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        'End
        Try
            If mTaskCard.IsSavable Then
                mTaskCard.ApplyEdit()
                mTaskCard.Save()
                MarkLog(Util.Action.Save, "TaskCard", "TaskCard No : " & mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
                Session("wfTaskCard.TaskCard") = mTaskCard
                Session("mTaskCardList") = mTaskCardList

                mTaskCard = TaskCard.NewTaskCard()
                MarkLog(Util.Action.[New], "TaskCard", "", Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
                Session("wfTaskCard.TaskCard") = mTaskCard
                GetTaskCardChilds()
                DataBind()
                settitle()
                upnlTitle.Update()
                DataFieldBind1()
                ControlVisibility()
                upnlValidationSummary.Update()

                upnldgTaskCardAttachment.Update()
                upnldgTaskCardSpares.Update()
                upnldgTaskCardTools.Update()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardWorkSpares.Update()
                UpnlTaskCardDetail.Update()
                upnlTaskCardHeader.Update()
                upnlAddWorkSpares.Update()

                'Added by Shital on 18-Aug-2016
                upnldgSkillList.Update()
                '-----------
                upnlEnclosure.Update()
                upnldgTaskCardPartRemovals.Update() 'Added By Vikrant on 03-Mar-2020 For ALL03032020
                UpnlTargetOtherDet.Update() 'Added by Sachin on 14-Sept-2023
            Else
                cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
                cvControlValidator.IsValid = mTaskCard.IsSavable
                If mTaskCard.IsValid Then NewRecord()
                upnlValidationSummary.Update()

                Exit Sub
            End If

            NewRecord()

        Catch ex As Exception
            MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry in Task Card.", MsgBoxStyle.OkOnly, "")
        End Try
    End Sub
    Private Sub NewRecord()
        mTaskCard = TaskCard.NewTaskCard()
        MarkLog(Util.Action.[New], "TaskCard", "", Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
        Session("wfTaskCard.TaskCard") = mTaskCard
        GetTaskCardChilds()
        DataBind()
        settitle()
        upnlTitle.Update()
        DataFieldBind1()
        ControlVisibility()
        upnlValidationSummary.Update()
        txtIssueDate.DataBind()
        txtRevDate.DataBind()
        upnldgTaskCardAttachment.Update()
        upnldgTaskCardSpares.Update()
        upnldgTaskCardTools.Update()
        upnldgTaskCardSteps.Update()
        upnldgTaskCardWorkSpares.Update()
        UpnlTaskCardDetail.Update()
        upnlTaskCardHeader.Update()
        upnlAddWorkSpares.Update()
        'Added by Shital on 18-Aug-2016
        upnldgSkillList.Update()
        '------------
        upnldgTaskCardPartRemovals.Update() 'Added By Vikrant on 03-Mar-2020 For ALL03032020
        UpnlTargetOtherDet.Update() 'Added by Sachin on 14-Sept-2023

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Added by Vikrant on 20-July-2011
        MarkLog(Util.Action.Close, "TaskCard", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        mTaskCard = Session("wfTaskCard.TaskCard")
        SaveFormtoObject()
        If mTaskCard.IsSavable Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            SaveFormtoObject()
            Session("wfTaskCard.TaskCard") = mTaskCard
            Session.Remove("wfTaskCard.TaskCard")
            Session("mTaskCardList") = mTaskCardList


            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If


            Response.Redirect(BackPage.Pop(Session("TaskBackPage")) & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8"))
            Session("Content") = Content
        End If
    End Sub
    'Added By Vikrant On 22-Jan-2013 For ALL21012013
    Private Sub btnAddTaskTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTaskTools.Click
        'Added By Vikrant On 02-Jan-2014 For All02012014
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        SaveFormtoObject()
        Dim mIsValid As Boolean = mTaskCard.IsSavable
        'End

        If mTaskCard.IsValid Then
            SaveFormtoObject()
            mTaskCard.TaskCardTools.Add(mTaskCard.ID)
            Session("mTaskCard") = mTaskCard
            Session("ToolEdit") = False
            'Response.Redirect("wfTaskCardTools.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=wfTaskCard.aspx" & "&Index=-1")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardToolWindow", "OpenTaskCardToolWindow()", True)

        Else 'Added By Vikrant On 02-Jan-2014 For All02012014
            cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
            cvControlValidator.IsValid = mTaskCard.IsSavable
        End If
        'End
        upnlValidationSummary.Update()
    End Sub

    'Added by Shital on 18-Aug-2016
    Private Sub btnAddSkill_Click(sender As Object, e As System.EventArgs) Handles btnAddSkill.Click
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        SaveFormtoObject()
        Dim mIsValid As Boolean = mTaskCard.IsSavable

        If mTaskCard.IsValid Then
            ' mTaskCard.TaskCardSkills.Add(mTaskCard.ID)
            Session("mTaskCard") = mTaskCard
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardSkillWindow", "OpenTaskCardSkillWindow()", True)

        Else
            cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
            cvControlValidator.IsValid = mTaskCard.IsSavable
        End If

        upnlValidationSummary.Update()
    End Sub
    '-------
    'Added By Vikrant on 03-Mar-2020 For ALL03032020
    Private Sub btnAddPartRemovals_Click(sender As Object, e As System.EventArgs) Handles btnAddPartRemovals.Click
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        SaveFormtoObject()
        Dim mIsValid As Boolean = mTaskCard.IsSavable

        If mTaskCard.IsValid Then
            mTaskCard.TaskCardPartRemovals.Add(mTaskCard.ID)
            Session("mTaskCard") = mTaskCard
            Session("PartRemovalEdit") = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardPartRemovalWindow", "OpenTaskCardPartRemovalWindow();", True)
        Else
            cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
            cvControlValidator.IsValid = mTaskCard.IsSavable
        End If

        upnlValidationSummary.Update()
    End Sub
    Private Sub dgTaskCardPartRemovals_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskCardPartRemovals.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardPartRemovals.PageSize * dgTaskCardPartRemovals.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardPartRemovals(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardPartRemovals(Index).Description
                Dim Qty As String = CType(mTaskCard.TaskCardPartRemovals(Index).RequiredQty, String)
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardView") And Not User.IsInRole("TaskCardEdit")) Then
                    MarkLog(Util.Action.Edit, "TaskCard", User.Identity.Name & " is not Authorized User to edit Task Card Spare : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                Session("PartNo") = PartNo   'Added by Sachin 12-09-23
                Session("PartRemovalEdit") = True
                SaveFormtoObject()
                mTaskCard.TaskCardPartRemovals.CurrentIndex = Index
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                Session("mTaskCard") = mTaskCard
                MarkLog(Util.Action.Edit, "TaskCard", "Task Card Part Removal : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Desc + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID) 'Added By Vikrant On 02-Jan-2014 For All02012014
                'Response.Redirect("wfTaskCardSpares.aspx?BackPage1=wfTaskCard.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardPartRemovalWindow", "OpenTaskCardPartRemovalWindow();", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardPartRemovals.PageSize * dgTaskCardPartRemovals.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardPartRemovals(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardPartRemovals(Index).Description
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardDelete")) Then
                    MarkLog(Util.Action.Delete, "TaskCard", User.Identity.Name & " is not Authorized User to delete Task Card Spare : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardPartRemovals.Update()
                DeletePartRemovalRecord(Index)
        End Select

    End Sub
    Private Sub dgTaskCardPartRemovals_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTaskCardPartRemovals.PageIndexChanging
        dgTaskCardPartRemovals.PageIndex = e.NewPageIndex
        dgTaskCardPartRemovals.DataSource = mTaskCard.TaskCardPartRemovals
        Session("mTaskCard") = mTaskCard
        dgTaskCardPartRemovals.DataBind()
    End Sub
    Private Sub hdnBtnTaskCardPartRemoval_Click(sender As Object, e As System.EventArgs) Handles hdnBtnTaskCardPartRemoval.Click
        If Not mTaskCard.TaskCardPartRemovals Is Nothing Then
            dgTaskCardPartRemovals.DataSource = mTaskCard.TaskCardPartRemovals
            dgTaskCardPartRemovals.DataBind()
        End If
        upnldgTaskCardPartRemovals.Update()
    End Sub
    'End
    Private Sub dgTaskCardTools_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTaskCardTools.PageIndexChanging
        dgTaskCardTools.PageIndex = e.NewPageIndex
        dgTaskCardTools.DataSource = mTaskCard.TaskCardTools
        Session("mTaskCard") = mTaskCard
        dgTaskCardTools.DataBind()
    End Sub
    Private Sub dgTaskCardTools_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskCardTools.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardTools.PageSize * dgTaskCardTools.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardTools(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardTools(Index).Description
                Dim Qty As String = CType(mTaskCard.TaskCardTools(Index).RequiredQty, String)
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardView") And Not User.IsInRole("TaskCardEdit")) Then
                    MarkLog(Util.Action.Edit, "TaskCard", User.Identity.Name & " is not Authorized User to edit Task Card Tool : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                Session("PartNo") = PartNo 'Added by Sachin 08-09-2023
                Session("ToolEdit") = True
                SaveFormtoObject()
                mTaskCard.TaskCardTools.CurrentIndex = Index
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardTools.Update()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardWorkSpares.Update()
                upnlAddWorkSpares.Update()

                'Added by Shital on 18-Aug-2016
                ' upnldgSkillList.Update()
                '-----------------

                Session("mTaskCard") = mTaskCard
                MarkLog(Util.Action.Edit, "TaskCard", "Task Card Tool : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Desc + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID) 'Added By Vikrant On 02-Jan-2014 For All02012014
                'Response.Redirect("wfTaskCardTools.aspx?BackPage1=wfTaskCard.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardToolWindow", "OpenTaskCardToolWindow()", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardTools.PageSize * dgTaskCardTools.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardTools(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardTools(Index).Description
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardDelete")) Then
                    MarkLog(Util.Action.Delete, "TaskCard", User.Identity.Name & " is not Authorized User to delete Task Card Tool : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                GetTaskCardChilds()
                DataBind()
                upnldgTaskCardSpares.Update()
                DeleteToolRecord(Index)
        End Select
    End Sub
    Private Sub btnAddTaskCardSpare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTaskCardSpare.Click
        'Added By Vikrant On 02-Jan-2014 For All02012014
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        SaveFormtoObject()
        Dim mIsValid As Boolean = mTaskCard.IsSavable
        'End

        If mTaskCard.IsValid Then
            SaveFormtoObject()
            mTaskCard.TaskCardSpares.Add(mTaskCard.ID)
            Session("mTaskCard") = mTaskCard
            Session("SpareEdit") = False
            'Response.Redirect("wfTaskCardSpares.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=wfTaskCard.aspx" & "&Index=-1")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardSpareWindow", "OpenTaskCardSpareWindow()", True)
        Else 'Added By Vikrant On 02-Jan-2014 For All02012014
            cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
            cvControlValidator.IsValid = mTaskCard.IsSavable
        End If
        upnlValidationSummary.Update()
    End Sub
    'End
    Private Sub btnAddWorkSpares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddWorkSpares.Click 'Added By Shweta
        'Added By Vikrant On 02-Jan-2014 For All02012014
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        SaveFormtoObject()
        Dim mIsValid As Boolean = mTaskCard.IsSavable
        'End

        If mTaskCard.IsValid Then
            SaveFormtoObject()
            mTaskCard.TaskCardStepsSpares.AddForSteps(mTaskCard.ID)
            Session("mTaskCard") = mTaskCard
            Session("WorkSpareEdit") = False
            'Response.Redirect("wfTaskCardStepSpares.aspx?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=wfTaskCard.aspx" & "&Index=-1")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardStepSparesWindow", "OpenTaskCardStepSparesWindow()", True)
        Else 'Added By Vikrant On 02-Jan-2014 For All02012014
            cvControlValidator.ErrorMessage = mTaskCard.GetBrokenRulesString
            cvControlValidator.IsValid = mTaskCard.IsSavable
        End If
        upnlValidationSummary.Update()
    End Sub

    Private Sub dgTaskCardSpares_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTaskCardSpares.PageIndexChanging
        dgTaskCardSpares.PageIndex = e.NewPageIndex
        dgTaskCardSpares.DataSource = mTaskCard.TaskCardSpares
        Session("mTaskCard") = mTaskCard
        dgTaskCardSpares.DataBind()
    End Sub
    Private Sub dgTaskCardSpares_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskCardSpares.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardSpares.PageSize * dgTaskCardSpares.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardSpares(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardSpares(Index).Description
                Dim Qty As String = CType(mTaskCard.TaskCardSpares(Index).RequiredQty, String)
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardView") And Not User.IsInRole("TaskCardEdit")) Then
                    MarkLog(Util.Action.Edit, "TaskCard", User.Identity.Name & " is not Authorized User to edit Task Card Spare : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                Session("PartNo") = PartNo 'Added by Sachin 08-09-2023
                Session("SpareEdit") = True
                SaveFormtoObject()
                mTaskCard.TaskCardSpares.CurrentIndex = Index
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                Session("mTaskCard") = mTaskCard
                MarkLog(Util.Action.Edit, "TaskCard", "Task Card Spare : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Desc + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID) 'Added By Vikrant On 02-Jan-2014 For All02012014
                'Response.Redirect("wfTaskCardSpares.aspx?BackPage1=wfTaskCard.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardSpareWindow", "OpenTaskCardSpareWindow()", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardSpares.PageSize * dgTaskCardSpares.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardSpares(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardSpares(Index).Description
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardDelete")) Then
                    MarkLog(Util.Action.Delete, "TaskCard", User.Identity.Name & " is not Authorized User to delete Task Card Spare : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardSpares.Update()
                DeleteSpareRecord(Index)
        End Select
    End Sub

    Private Sub dgTaskCardWorkSpares_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTaskCardWorkSpares.PageIndexChanging
        dgTaskCardWorkSpares.PageIndex = e.NewPageIndex
        dgTaskCardWorkSpares.DataSource = mTaskCard.TaskCardStepsSpares
        Session("mTaskCard") = mTaskCard
        dgTaskCardWorkSpares.DataBind()
    End Sub

    'Added By Shweta On 5-Sep-2013 For BA04092013
    Private Sub dgTaskCardWorkSpares_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskCardWorkSpares.RowCommand
        'Dim Index As Int32 = e.Item.ItemIndex + dgTaskCardSpares.CurrentPageIndex * dgTaskCardSpares.PageSize
        Select Case e.CommandName
            Case "EditRec"

                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardWorkSpares.PageSize * dgTaskCardWorkSpares.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardStepsSpares(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardStepsSpares(Index).Description
                Dim Qty As String = CType(mTaskCard.TaskCardStepsSpares(Index).RequiredQty, String)
                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardView") And Not User.IsInRole("TaskCardEdit")) Then
                    MarkLog(Util.Action.Edit, "TaskCard", User.Identity.Name & " is not Authorized User to edit Additional Work Spare : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                Session("PartNo") = PartNo 'Added by Sachin 08-09-2023
                Session("WorkSpareEdit") = True
                SaveFormtoObject()
                ' mTaskCard.TaskCardSpares.CurrentIndex = Index
                mTaskCard.TaskCardStepsSpares.CurrentIndex = Index
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardSpares.Update()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardWorkSpares.Update()
                upnlAddWorkSpares.Update()
                Session("mTaskCard") = mTaskCard
                MarkLog(Util.Action.Edit, "TaskCard", "Additional Work Spare : " + Chr(13) + "Part No. : " + PartNo + "," + "Description : " + Desc + "," + "Qty. : " + Qty, Util.ErrorType.HandledError, mTaskCard.ID, EventLogID) 'Added By Vikrant On 02-Jan-2014 For All02012014
                'Response.Redirect("wfTaskCardStepSpares.aspx?BackPage1=wfTaskCard.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage5=" & Request.QueryString("BackPage5") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskCardStepSparesWindow", "OpenTaskCardStepSparesWindow()", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardWorkSpares.PageSize * dgTaskCardWorkSpares.PageIndex
                Dim PartNo As String = mTaskCard.TaskCardStepsSpares(Index).PartNo
                Dim Desc As String = mTaskCard.TaskCardStepsSpares(Index).Description

                'Added By Vikrant On 03-Jan-2014 For All02012014 
                If (Not User.IsInRole("TaskCardDelete")) Then
                    MarkLog(Util.Action.Delete, "TaskCard", User.Identity.Name & " is not Authorized User to delete Additional Work Spare : " & PartNo + "," + Desc, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardSpares.Update()
                upnldgTaskCardSteps.Update()
                upnldgTaskCardWorkSpares.Update()
                upnlAddWorkSpares.Update()
                DeleteStepSpareRecord(Index)
        End Select
    End Sub

    Private Sub dgTaskCardAttachment_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTaskCardAttachment.PageIndexChanging
        dgTaskCardAttachment.PageIndex = e.NewPageIndex
        dgTaskCardAttachment.DataSource = mTaskCard.TaskCardAttachments
        Session("mTaskCard") = mTaskCard
        dgTaskCardAttachment.DataBind()
    End Sub
    'Added By Vikrant On 17-Apr-2013 For ALL17042013
    Private Sub dgTaskCardAttachment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskCardAttachment.RowCommand
        Dim mTaskCardAttachments As TaskCardAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardAttachment.PageSize * dgTaskCardAttachment.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                'mTaskCardAttachments = mTaskCard.TaskCardAttachments.GetTaskCardAttachment(New Guid(e.Item.Cells(0).Text))
                mTaskCardAttachments = mTaskCard.TaskCardAttachments
                mTaskCardAttachments.CurrentIndex = Index
                If mTaskCardAttachments.CurrentItem.ImageSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mTaskCardAttachments.CurrentItem.FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mTaskCardAttachments.CurrentItem.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mTaskCardAttachments.CurrentItem.ImageFile, 0, mTaskCardAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
                GetTaskCardChilds()
                DataBind()
                ControlVisibility()
                upnldgTaskCardAttachment.Update()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTaskCardAttachment.PageSize * dgTaskCardAttachment.PageIndex

                DeleteAttachment(Index)
        End Select

    End Sub

    'Added by Shital on 18-Aug-2016
    Private Sub gdSkillList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdSkillList.RowCommand
        Select Case e.CommandName

            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + gdSkillList.PageSize * gdSkillList.PageIndex
                Dim SkillName As String = mTaskCard.TaskCardSkills(Index).SkillName

                If (Not User.IsInRole("TaskCardDelete")) Then
                    MarkLog(Util.Action.Delete, "TaskCard", User.Identity.Name & " is not Authorized User to delete Task Card Tool : " & SkillName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'End
                GetTaskCardChilds()
                DataBind()
                upnldgSkillList.Update()
                DeleteSkillRecord(Index)

        End Select
    End Sub

    'End
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachFile()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnTaskCardSpare_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTaskCardSpare.Click
        If Not mTaskCard.TaskCardSpares Is Nothing Then
            dgTaskCardSpares.DataSource = mTaskCard.TaskCardSpares
            dgTaskCardSpares.DataBind()
        End If
        upnldgTaskCardSpares.Update()
    End Sub
    Private Sub hdnBtnTaskCardTool_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTaskCardTool.Click

        If Not mTaskCard.TaskCardTools Is Nothing Then
            dgTaskCardTools.DataSource = mTaskCard.TaskCardTools
            dgTaskCardTools.DataBind()
        End If
        upnldgTaskCardTools.Update()
    End Sub
    'Added by Shital on 18-Aug-2016
    Private Sub hdnBtnTaskCardSkill_Click(sender As Object, e As System.EventArgs) Handles hdnBtnTaskCardSkill.Click
        If Not mTaskCard.TaskCardSkills Is Nothing Then
            gdSkillList.DataSource = mTaskCard.TaskCardSkills
            gdSkillList.DataBind()
        End If
        upnldgSkillList.Update()
    End Sub

    Private Sub hdnBtnTaskCardStep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTaskCardStep.Click
        If Not mTaskCard.TaskSteps Is Nothing Then
            dgTaskCardSteps.DataSource = mTaskCard.TaskSteps
            dgTaskCardSteps.DataBind()
        End If
        ControlVisibility()
        upnldgTaskCardSteps.Update()
        upnldgTaskCardWorkSpares.Update()
        upnlAddWorkSpares.Update()
    End Sub
    Private Sub hdnBtnTaskCardStepSpares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTaskCardStepSpares.Click
        If Not mTaskCard.TaskCardStepsSpares Is Nothing Then
            dgTaskCardWorkSpares.DataSource = mTaskCard.TaskCardStepsSpares
            dgTaskCardWorkSpares.DataBind()
        End If
        upnldgTaskCardWorkSpares.Update()
    End Sub
#End Region


#Region "Drag n Drop" 'Added by Saylee on 30-Sep-2013 for ALL03102013

    <WebMethod()>
    Public Shared Sub GetTableIDs(ByVal Ids As DragDrop)
        Dim mDragDrop As New DragDrop

        mDragDrop = Ids
        Dim mTaskCard As TaskCard

        mTaskCard = HttpContext.Current.Session("wfTaskCard.TaskCard")
        For i As Integer = 0 To mDragDrop.SrNo.Length - 1
            If mTaskCard.TaskCardAttachments.Contains(mDragDrop.SrNo(i)) Then
                mTaskCard.TaskCardAttachments.Item(CInt(mDragDrop.SrNo(i)), False).TempSrNo = CInt(mDragDrop.index(i)) + 1
            End If
        Next

        HttpContext.Current.Session("wfTaskCard.TaskCard") = mTaskCard
    End Sub
    Private Sub btnRefresh_Click(sender As Object, e As System.EventArgs) Handles btnRefresh.Click
        Try
            mTaskCard.TaskCardAttachments.UpdateSrNo()
            mTaskCard.ApplyEdit()
            mTaskCard.Save()
            mTaskCard = TaskCard.GetTaskCard(New Guid(CType(Session("ID"), String)))
            dgTaskCardAttachment.DataSource = mTaskCard.TaskCardAttachments
            dgTaskCardAttachment.DataBind()
            HttpContext.Current.Session("wfTaskCard.TaskCard") = mTaskCard
            upnldgTaskCardAttachment.Update()
        Catch ex As Exception

        End Try
    End Sub
    Public Class DragDrop
        Private mIndex(10) As String
        Private mSrno(10) As String
        Public Property index() As String()
            Get
                Return mIndex
            End Get
            Set(ByVal value As String())
                ReDim mIndex(value.Length)
                mIndex = value
            End Set
        End Property
        Public Property SrNo() As String()
            Get
                Return mSrno
            End Get
            Set(ByVal value As String())
                ReDim mSrno(value.Length)
                mSrno = value
            End Set
        End Property
        Public Sub New()

        End Sub
    End Class
#End Region



    
End Class