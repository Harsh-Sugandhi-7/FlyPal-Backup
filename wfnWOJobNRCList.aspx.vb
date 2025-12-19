Public Class wfnWOJobNRCList
    Inherits Page

#Region " Variable Declaration "

    Public mnWOJob As nWOJob
    Protected mnWO As nWO
    Dim mWOJobTypeID As Integer
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mWODetail As String
    'Added By Vikrant For WO NRC
    Dim mWOJobNRCList As WOJobNRCList
    Dim mnWOJobNRC As nWOJob
    'End
    'Added By Saylee On 27-Dec-2018
    Dim mFileJobAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End
    Dim OpenAs As String

#End Region

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

#Region " Business Methods "
    Private Sub GetSession()
        mnWOJob = Session("mnWOJob")
        mnWO = Session("mnWO")
        mWOJobTypeID = CType(Session("WOJobTypeID"), Integer)
        mWOJobNRCList = CType(Session("mWOJobNRCList"), WOJobNRCList) 'Added By Vikrant For WO NRC
    End Sub
    Private Sub SetSession()
        Session("WOJobTypeID") = mWOJobTypeID
        'Added By Saylee On 27-Dec-2018
        Session("mFileAttach") = mFileJobAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub

    Private Function IsInRole(CheckFor As Rights) As Boolean

        Dim IsInRoleString As String = ""

        If AppSettings("ShowNewWOFlow") = "True" Then

            If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then

                If mnWO.TransTypeID = Trans.WO145 Then

                    IsInRoleString = "WOCreate"

                Else

                    IsInRoleString = "CAMOWOCreate"

                End If

            ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then

                IsInRoleString = "WOPlanning"

            ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then

                IsInRoleString = "WOExecution"

            ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then

                IsInRoleString = "WOCompletion"

            ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then

                IsInRoleString = "WOQCApproval"

            ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then

                IsInRoleString = "WOCAMOUpdate"

            ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then

                IsInRoleString = "WOBilling"

            End If

        Else

            If mnWO.TransTypeID = Trans.WO145 Then

                If Session("MiddleFrame") = "wfnWOJobListToComplete_AJAX.aspx" Then

                    IsInRoleString = "WOJobListToComplete"

                Else

                    IsInRoleString = "WorkOrder"

                End If

            ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then

                IsInRoleString = "SpareAssemblyWO"

            ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then

                IsInRoleString = "SpareComponentWO"

            ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then

                IsInRoleString = "EngineeringOrder"

            Else

                IsInRoleString = "CAMOWO"
            End If

        End If

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
    Private Sub CallUpdatePanels()
        ' upnlWOJobDetails.Update()
        'upnlTitle.Update()
        upnlJobNRC.Update()
    End Sub
    Private Sub ControlVisibility()

        btnRaiseNRC.Enabled = IIf(mnWO.IsThirdParty, False, True) And mnWO.WOStatusID <> 3
        dgWOJobNRC.Columns(8).Visible = IIf(mnWO.IsThirdParty, False, True)
        dgWOJobNRC.Columns(9).Visible = IIf(mnWO.IsThirdParty, False, True) And mnWO.WOStatusID <> 3
        OpenAs = Request.QueryString("Type")

        If AppSettings("ClientCode") = "IND" Then

            lblTitle.Text = "W.O. JOB OJS List"
            lblJobNRC.Text = "List of W.O. OJS JOBS"
            btnRaiseNRC.Text = "Raise OJS"
            btnRaiseNRC.ToolTip = "Raise a new OJS"
            dgWOJobNRC.ToolTip = "List of W.O. OJS JOBS"

        Else

            lblTitle.Text = "W.O. JOB NRC List"
            lblJobNRC.Text = "List of W.O. NRC JOBS"
            btnRaiseNRC.Text = "Raise NRC"
            btnRaiseNRC.ToolTip = "Raise a new NRC"
            dgWOJobNRC.ToolTip = "List of W.O. NRC JOBS"

        End If

    End Sub

    Private Sub DeleteWOJobNRC(Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveWOJobNRC")
        Session("JobNRCIndex") = Index
    End Sub

    Private Overloads Sub SetFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript([GetType], "focusscript", str)
    End Sub

    Private Sub SetJobNRCGrid()

        Dim P As Boolean

        For j As Integer = 0 To dgWOJobNRC.Rows.Count - 1

            P = CType(Me.dgWOJobNRC.Rows.Item(j).Cells(12).Text, Boolean) 'Ajay 14=>12

        Next

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "RemoveWOJobNRC" Then
                        Dim mFileAttachments As New FileAttachments
                        Dim index As Integer = CType(Session("JobNRCIndex"), Integer)
                        Session.Remove("JobNRCIndex")
                        nWOJob.DeleteWOJobNRC(mWOJobNRCList(index).ID)
                        mFileAttachments.DeleteAllByRefID(mWOJobNRCList(index).ID)
                        mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mnWO.ID, mnWO.WOJobs.CurrentItem.ID)
                        Session("mWOJobNRCList") = mWOJobNRCList
                        dgWOJobNRC.DataSource = mWOJobNRCList
                        dgWOJobNRC.DataBind()
                        SetJobNRCGrid()
                        upnlJobNRC.Update()
                        ScriptManager.RegisterStartupScript(Me, [GetType], "SetTabCount", "SetTabCount('" + mWOJobNRCList.Count.ToString + "');", True)
                        'End
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        If mnWO.WOJobs.CurrentItem.IsValid = True Then
                            Session.Remove("IsValid")
                        Else
                            Session.Remove("IsValid")
                            ''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
                            ControlVisibility()
                            SetGrid()
                            DataFieldBind()
                            CallUpdatePanels()
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        If Session("Edit") = True Then
                            mnWO = Session("mnWOClone")
                        End If
                        Session("mnWO") = mnWO
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Session.Remove("Edit")
                        Session.Remove("mnWOClone")
                        If mnWO.WOJobs.CurrentItem.IsNew And mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 Then
                            mnWO.WOJobs.Remove(mnWO.WOJobs.CurrentItem)
                        End If
                        OpenAs = Request.QueryString("Type")
                        If Not OpenAs Is Nothing AndAlso OpenAs = "pup" Then
                            'Session.Remove("MiddleFrame")
                            ScriptManager.RegisterStartupScript(Me, [GetType], "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
                    ElseIf MSGBoxCtrl.Sender = "RemoveWOJobNRC" Then
                        'Do Nothing
                        Session.Remove("JobNRCIndex")
                        'End
                    Else
                        Session("sender") = ""
                        ControlVisibility()
                        SetGrid()
                        DataFieldBind()
                        ''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
                    End If

            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ControlVisibility()
            SetGrid()
            DataFieldBind()
            CallUpdatePanels()
            ''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetGrid()
        'Dim P As Integer
        'Dim lnkWOJobTaskView As LinkButton
        'For j As Integer = 0 To dgWOJobNRC.Rows.Count - 1
        '    If Me.dgWOJobNRC.Rows.Item(j).Cells(11).Text = "" Then
        '        P = 0
        '    Else
        '        P = CType(Me.dgWOJobNRC.Rows.Item(j).Cells(11).Text, Integer)
        '    End If

        '    If P <= 0 Then
        '        lnkWOJobTaskView = CType(dgWOJobNRC.Rows.Item(j).Cells(10).FindControl("lnkWOJobTaskView"), LinkButton)
        '        lnkWOJobTaskView.Enabled = False
        '    End If
        'Next
    End Sub
    Private Sub AddMultipleTaskCards()
        Dim tmpTaskCard As TaskCard
        Dim mTaskCardList As TaskCardList = Session("mSelectTaskCardList")
        For Each tmpTaskCard In mTaskCardList
            If tmpTaskCard.IsSelect Then
                If Not mnWOJob.WOJobTasks.Contains(tmpTaskCard.ID, "") Then
                    Dim mTaskCard As TaskCard
                    mTaskCard = TaskCard.GetTaskCard(tmpTaskCard.ID)
                    mnWOJob.WOJobTasks.Add(mnWOJob.ID, mTaskCard.ID.ToString)
                    With mnWOJob.WOJobTasks.CurrentItem
                        mnWOJob.WOJobTasks.CurrentItem.SrNo = mnWOJob.WOJobTasks.CurrentIndex + 1
                        mnWOJob.WOJobTasks.CurrentItem.TaskCardNo = mTaskCard.TaskCardNo
                        mnWOJob.WOJobTasks.CurrentItem.EstimatedHours = mTaskCard.EstimatedHours
                        mnWOJob.WOJobTasks.CurrentItem.Reference = mTaskCard.Reference
                        mnWOJob.WOJobTasks.CurrentItem.Equipment = mTaskCard.Equipment
                        mnWOJob.WOJobTasks.CurrentItem.Material = mTaskCard.Material
                        mnWOJob.WOJobTasks.CurrentItem.TaskDescription = mTaskCard.TaskDesc
                        mnWOJob.WOJobTasks.CurrentItem.RevNo = mTaskCard.RevNo
                        mnWOJob.WOJobTasks.CurrentItem.RevDate = mTaskCard.RevDate
                        mnWOJob.WOJobTasks.CurrentItem.IssueDate = mTaskCard.IssueDate
                        mnWOJob.WOJobTasks.CurrentItem.checks = mTaskCard.Check
                        mnWOJob.WOJobTasks.CurrentItem.RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo

                        Dim mTaskCardSpare As TaskCardSpare
                        Dim mTaskCardStepsSpare As TaskCardSpare

                        For Each mTaskCardSpare In mTaskCard.TaskCardSpares
                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                            With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem
                                .ItemID = mTaskCardSpare.ItemID
                                .RequiredQty = mTaskCardSpare.RequiredQty
                                .PartNo = mTaskCardSpare.PartNo
                                .Description = mTaskCardSpare.Description
                                .Remark = mTaskCardSpare.Remark
                                .OnSerialNo = mTaskCardSpare.OnSerialNo
                                .OffSerialNo = mTaskCardSpare.OffSerialNo
                                .IsForSteps = False
                            End With

                        Next

                        For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                            With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem
                                .ItemID = mTaskCardStepsSpare.ItemID
                                .RequiredQty = mTaskCardStepsSpare.RequiredQty
                                .PartNo = mTaskCardStepsSpare.PartNo
                                .Description = mTaskCardStepsSpare.Description
                                .Remark = mTaskCardStepsSpare.Remark
                                .OnSerialNo = mTaskCardStepsSpare.OnSerialNo
                                .OffSerialNo = mTaskCardStepsSpare.OffSerialNo
                                .IsForSteps = True
                            End With
                        Next
                        'Added By Vikrant on 03-Mar-2020 For ALL03032020
                        For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals
                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                            With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem
                                .ItemID = mTaskCardSpare.ItemID
                                .RequiredQty = mTaskCardSpare.RequiredQty
                                .PartNo = mTaskCardSpare.PartNo
                                .Description = mTaskCardSpare.Description
                                .Remark = mTaskCardSpare.Remark
                                .OnSerialNo = mTaskCardSpare.OnSerialNo
                                .OffSerialNo = mTaskCardSpare.OffSerialNo
                                .IsForSteps = False
                                .IsPartRemoval = True
                                .Position = mTaskCardSpare.Position
                            End With

                        Next
                        'End
                    End With
                    'Else
                End If
            Else
                If mnWO.WOJobs.CurrentItem.WOJobTasks.Contains(tmpTaskCard.ID, "") Then
                    mnWO.WOJobs.CurrentItem.WOJobTasks.Remove(tmpTaskCard.ID, "")
                End If
            End If
        Next
        Session("TaskCards") = "False"
        Session.Remove("mTaskCard")
        Session.Remove("mTaskCardList")
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()
        mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mnWO.ID, mnWO.WOJobs.CurrentItem.ID)
        Session("mWOJobNRCList") = mWOJobNRCList
        dgWOJobNRC.DataSource = mWOJobNRCList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not Page.IsPostBack Then
            DataFieldBind()
        End If
        ControlVisibility()
        SetGrid()
    End Sub
    Private Sub GV_WOJobNRC_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgWOJobNRC.RowCommand

        Select Case e.CommandName
            Case "EditRecord"

                Dim Index As Integer = CType(e.CommandArgument, Integer)

                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                Session("Edit") = True
                mnWOJobNRC = nWOJob.GetWOJobNRC(mWOJobNRCList(Index).ID)
                Session("nWOJobNRC") = mnWOJobNRC
                Session("mnWO") = mnWO
                Session("mnWOJobParent") = mnWO.WOJobs.CurrentItem
                'Added By Saylee On 27-Dec-2018 
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")

                OpenAs = Request.QueryString("Type")
                If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenToAddWOJobNRCDetail",
                                                        "OpenToAddWOJobNRCDetail();",
                                                        True)

                ElseIf OpenAs IsNot Nothing AndAlso OpenAs = "childpup" Then

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "CallParentOpenToAddNRCJobDetail",
                                                        "CallParentOpenToAddNRCJobDetail();",
                                                        True)

                End If

            Case "DeleteRecord"

                'Added by Saylee on 7-Mar-2014 for ALL07032014
                If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
                   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

                    SetSession()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                Dim Index As Integer = CType(e.CommandArgument, Integer)

                DeleteWOJobNRC(Index)

            Case "View"

                'Added by Saylee on 7-Mar-2014 for ALL07032014
                If (Not IsInRole(Rights.View)) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                Dim Index As Integer
                Index = rowIndex
                '----------------------------------------------------------------------
                mnWOJobNRC = nWOJob.GetWOJobNRC(mWOJobNRCList(Index).ID)

                If mnWOJobNRC.IsAttachmentAdded Then

                    mFileJobAttach = FileAttach.GetAttachment(mnWOJobNRC.ID) 'Sort = 2 : Removal
                    Session("mFileAttach") = mFileJobAttach

                End If

                If mFileJobAttach.Size > 0 Then

                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileJobAttach.Extension
                    Dim fs As FileStream

                    If File.Exists(AppSettings("DOCPath")) = False Then

                        'Delete File if exist
                        File.Delete(AppSettings("DOCPath") & StrName & mFileJobAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileJobAttach.ImageFile, 0, mFileJobAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "openFile",
                                                            "openFile();",
                                                            True)

                    End If

                End If

			Case "PrintRec" 'Added By Prashant 6-OCT2025
				Dim Index As Integer = CType(e.CommandArgument, Integer)
				mnWOJobNRC = nWOJob.GetWOJobNRC(mWOJobNRCList(Index).ID)

				Dim da As New ObjectAdapter
                Dim mCompanyDetail As New CompanyDetail
                Dim mnWOTools As nWOTools
                Dim mnWOPeriods As nWOPeriods
                Dim mnWOJobTasks As nWOJobTasks
                Dim mnrptWOJobResourceDetails As nrptWOJobResourceDetails
                Dim mnWOJobSpares As nWOJobSpares
                Dim mnWOJobComps As nWOJobComps
                Dim mnWOJobs As nWOJobs
                Dim SearchStr1 As String = New SmartDate(Today.Date).FormattedText
                Dim rpt As New crnWOJobDetail
                Dim ds As New dsnWODetail

                Dim myReport As Engine.ReportClass
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
                    myReport = New crnWOJobDetailTAAL
                ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
                    myReport = New crnWOJobDetailNOVO
                ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then 'Added by Saylee on 13-Aug-2018  for StarAir13082018-1
                    myReport = New crnWOJobDetailSTR
                ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "IND") Then
                    myReport = New crnOffJobSheet
                ElseIf AppSettings("ClientCode") = "AFC" Then 'Added by Saylee on 11-Jun-2025 for FLYPAL-2484 ( W.O. Report for Afcom ) 
                    myReport = New crnWODetailForAfcom
                Else
                    myReport = New crnWOJobDetail
                End If

                Dim mnWOJobParent As nWOJob = mnWO.WOJobs.CurrentItem 'Session("mnWOJobParent")
				Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
				mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
				mCompanyDetail.WebSite, "NRC Details", SearchStr1, AppSettings("WO-NRCIssueRev"),
				mnWO.WONumber + "-" + mnWOJobParent.SrNo.ToString + "-" + mnWOJob.SrNo.ToString, mnWOJobParent.WorkPACKREF,
				SearchStr5:="OpenFromJobNRCPage", AppSettings("Product Version"), AppSettings("SINote"), "", "",
				"", "", AppSettings("Logo"))

				mnWO = Session("mnWO")

                mnWOJobs = mnWO.WOJobs
				mnWOJob = mnWOJobNRC
				mnWOTools = mnWO.WOTools
                mnWOPeriods = mnWO.WOPeriods

                mnWOJobTasks = mnWOJob.WOJobTasks
                mnrptWOJobResourceDetails = nrptWOJobResourceDetails.GetrptWOJobResourceDetails(mnWOJob.ID.ToString)
                mnWOJobSpares = mnWOJob.WOJobSpares
                mnWOJobComps = mnWOJob.WOJobComps

                da.Fill(ds, mnWO)

                If AppSettings("ClientCode") = "AFC" Then
                    da.Fill(ds, "nWOJobs", mnWOJob)
                Else
                    da.Fill(ds, mnWOJob)
                    da.Fill(ds, mnWOJobs)
                End If

                da.Fill(ds, mnWOTools)
                da.Fill(ds, mnWOPeriods)
                da.Fill(ds, mnWOJobTasks)
                da.Fill(ds, mnrptWOJobResourceDetails)
                da.Fill(ds, mnWOJobSpares)
                da.Fill(ds, mnWOJobComps)
                da.Fill(ds, Report)
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, mrptImage)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", Str, True)

        End Select

    End Sub
    Private Sub btnRaiseNRC_Click(sender As Object, e As System.EventArgs) Handles btnRaiseNRC.Click

        If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
            SetSession()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        mnWOJobNRC = nWOJob.NewWOJobNRC(mnWO.ID, mnWO.WOJobs.CurrentItem.ID)
        mnWOJobNRC.WOJobTypeID = 5
        mnWOJobNRC.SrNo = mWOJobNRCList.Count + 1
        Session("nWOJobNRC") = mnWOJobNRC
        Session("mnWO") = mnWO
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'Response.Redirect("wfnWOJobNRC_Ajax.aspx?CPage1=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
        OpenAs = Request.QueryString("Type")
        If Not OpenAs Is Nothing AndAlso OpenAs = "pup" Then
            ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddWOJobNRCDetail", "OpenToAddWOJobNRCDetail();", True)
        ElseIf Not OpenAs Is Nothing AndAlso OpenAs = "childpup" Then
            ScriptManager.RegisterStartupScript(Me, [GetType], "CallParentOpenToAddNRCJobDetail", "CallParentOpenToAddNRCJobDetail();", True)
        End If
    End Sub
    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click

        If mnWO.WOJobs.CurrentItem.IsNew And mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 Then
            mnWO.WOJobs.Remove(mnWO.WOJobs.CurrentItem)
        End If

        OpenAs = Request.QueryString("Type")
        ScriptManager.RegisterStartupScript(Me, [GetType], "SetTabCount", "SetTabCount('" + mWOJobNRCList.Count.ToString + "');", True)

        If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then

            ScriptManager.RegisterStartupScript(Me, [GetType], "onclose", "CallParentCallback();", True)
            Exit Sub

        ElseIf Not OpenAs Is Nothing AndAlso OpenAs = "childpup" Then

            ScriptManager.RegisterStartupScript(Me, [GetType], "CallCloseChildPage", "CallCloseChildPage();", True)
            Exit Sub

        End If


        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

    End Sub

    Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnAddWOJobNRCDetail_Click(sender As Object, e As System.EventArgs) Handles hdnBtnAddWOJobNRCDetail.Click
        mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mnWO.ID, mnWO.WOJobs.CurrentItem.ID)
        Session("mWOJobNRCList") = mWOJobNRCList
        dgWOJobNRC.DataSource = mWOJobNRCList
        dgWOJobNRC.DataBind()
        SetGrid()
        upnlJobNRC.Update()
    End Sub
#End Region

End Class