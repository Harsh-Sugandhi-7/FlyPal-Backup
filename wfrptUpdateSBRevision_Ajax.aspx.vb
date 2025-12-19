Imports System.Text
Public Class wfrptUpdateSBRevision_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mModelMonitorModList As ModelMonitorModList
    Public mModelMonitorMod As ModelMonitorMod
    Public mModelList As ModelList
    Dim mDirectiveDetail, ConfigDetail As New StringBuilder
    Dim EventLogID As Guid
    Public PeriodValues(,) As String
    Public mModelMonitorModTypeList As ModelMonitorModTypeList 'Added By Vikrant On 04-Dec-2018 For ALL04122018
    Dim mModuleList As ModuleList

    'Added By Saylee on 13-May-2020, LockDown 3.0
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    '***********************************
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mModelMonitorModList = CType(Session("mModelMonitorModList"), ModelMonitorModList)
        mModelMonitorMod = CType(Session("mModelMonitorMod"), ModelMonitorMod)
        mModelList = CType(Session("mModelList"), ModelList)
        mModelMonitorModTypeList = Session("mModelMonitorModTypeList") 'Added By Vikrant On 04-Dec-2018 For ALL04122018
        mModuleList = Session("mModuleList")
        'Added By Saylee on 13-May-2020, LockDown 3.0
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        '***********************************
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelMonitorModList")
        Session.Remove("mModelMonitorMod")
        Session.Remove("mModelList")
        Session.Remove("mModelMonitorModTypeList") 'Added By Vikrant On 04-Dec-2018 For ALL04122018
        Session.Remove("mFileAttach") 'Added By Saylee on 13-May-2020, LockDown 3.0
        Session.Remove("IsAttachmentDeleted") 'Added By Saylee on 13-May-2020, LockDown 3.0
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptUpdateSBRevision_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub FindNow()
        dgModelMonitorModList.PageIndex = 0
        Dim SearchIndex As Integer = cmbSearch.SelectedIndex
        Select Case SearchIndex
            Case 0, -1  'All
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(Guid.Empty)
            Case 1  'Model
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(cmbModel.SelectedValue))
            Case 2  'ATA Code
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(Guid.Empty, 0, Val(txtCode.Text))
            Case 3  'Description
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(Guid.Empty, 0, , , txtSearchFor.Text.Trim)
            Case 4  'Reference
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(Guid.Empty, 0, , , , txtSearchFor.Text.Trim)
            Case 5  'Directive No.
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(Guid.Empty, 0, , , , , txtSearchFor.Text.Trim)
                'Added By Vikrant On 04-Dec-2018 For ALL04122018
            Case 6  'Directive Type
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(Guid.Empty, cmbDirectiveType.SelectedValue, , , , , txtSearchFor.Text.Trim)
                'End
        End Select

        dgModelMonitorModList.DataSource = mModelMonitorModList
        dgModelMonitorModList.DataBind()
        Session("mModelMonitorModList") = mModelMonitorModList
        lblResult.Text = "List Of Directives : " & mModelMonitorModList.Count & " Record(s)"
    End Sub
    Public Sub SetControl()
        FindNow()
        upnlGrid.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SaveConfig" Then
                        Try
                            mModelMonitorMod = Session("mModelMonitorMod")
                            mDirectiveDetail.Append("Master Details : ")
                            mDirectiveDetail.Append(Environment.NewLine)
                            mDirectiveDetail.Append("SB Directive No : " & mModelMonitorMod.Number & ", Model : " & mModelMonitorMod.Model.Name)
                            mDirectiveDetail.Append(Environment.NewLine)
                            mDirectiveDetail.Append("Old Directive No. : " & txtOldDirectiveNo.Text & ", New Directive No. : " & txtNewDirectiveNo.Text)
                            mDirectiveDetail.Append(Environment.NewLine)
                            mDirectiveDetail.Append("Old Note. : " & txtOldNote.Text & ", New Rev. No. : " & txtNewNote.Text)
                            mDirectiveDetail.Append(Environment.NewLine)
                            mDirectiveDetail.Append("Old Issue. Date : " & txtOldIssueDate.Text & ", New Issue. Date : " & txtNewIssueDate.Text)
                            mDirectiveDetail.Append(Environment.NewLine)
                            mDirectiveDetail.Append("Old Note. : " & txtOldNote.Text & ", New Rev. No. : " & txtNewNote.Text)

                            If txtNewIssueDate.Text = "" Then
                                mModelMonitorMod.IssueDate = System.DBNull.Value
                            Else
                                mModelMonitorMod.IssueDate = txtNewIssueDate.Text
                            End If
                            mModelMonitorMod.Number = txtNewDirectiveNo.Text.Trim
                            mModelMonitorMod.Note = Trim(txtNewNote.Text)

                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    mModelMonitorMod.IsAttachmentAdded = True
                                Else
                                    mModelMonitorMod.IsAttachmentAdded = False
                                End If
                            End If

                            mModelMonitorMod.Save()
                            MarkLog(Util.Action.Save, "UpdateSBRevision", mDirectiveDetail.ToString, Util.ErrorType.NoError, mModelMonitorMod.ID, EventLogID)
                            Dim mtmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList
                            mtmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(Today.Date.ToString, Guid.Empty.ToString, mModelMonitorMod.Model.Name, "", , , , , , , mModelMonitorMod.Number, , , , mModelMonitorMod.ID.ToString, SortBy:="MinimumRemainingValue")
                            For i As Integer = 0 To mtmpComplyAssemblyMonitorModStatusList.Count - 1
                                If mtmpComplyAssemblyMonitorModStatusList(i).DoneOnFormatted.ToString <> "" And DateDiff(DateInterval.Day, SmartDate.StringToDate(mtmpComplyAssemblyMonitorModStatusList(i).DoneOnFormatted.ToString), SmartDate.StringToDate(txtNewIssueDate.Text)) <> 0 Then
                                    ConfigDetail.Append("Config. Details : ")
                                    mDirectiveDetail.Append(Environment.NewLine)
                                    ConfigDetail.Append("Reg No. : " & mtmpComplyAssemblyMonitorModStatusList(i).MachineInfo & " Assembly Info : " & mtmpComplyAssemblyMonitorModStatusList(i).AssemblyInfo + " Old Done On : " & mtmpComplyAssemblyMonitorModStatusList(i).DoneOnFormatted.ToString & " Old Done On Values : " & mtmpComplyAssemblyMonitorModStatusList(i).DoneOnValueFormatted.Replace("<BR>", " "))
                                    Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
                                    Dim mMachine As Machine = Machine.GetMachine(mtmpComplyAssemblyMonitorModStatusList(i).MachineID)
                                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mtmpComplyAssemblyMonitorModStatusList.Item(i).AssemblyMonitorModStatusID, mtmpComplyAssemblyMonitorModStatusList.Item(i).AssemblyStatusID, mMachine.HourType)
                                    mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType, True)
                                    'mAssemblyMonitorModStatus.DoneOn = mModelMonitorMod.IssueDateFormatted.ToString
                                    UpdateModPeriods(mAssemblyMonitorModStatus, txtNewIssueDate.Text, mMachine)
                                    mAssemblyMonitorModStatus = Session("mAssemblyMonitorModStatus")
                                    mAssemblyMonitorModStatus.Save()

                                    ConfigDetail.Append(" New Done On : " & mAssemblyMonitorModStatus.DoneOnFormatted.ToString)
                                    If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count > 0 Then
                                        ConfigDetail.Append(" New Done On Values : ")
                                        For j As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
                                            ConfigDetail.Append(" " & mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted)
                                        Next
                                    End If
                                    MarkLog(Util.Action.Save, "UpdateSBRevision", ConfigDetail.ToString, Util.ErrorType.NoError, mtmpComplyAssemblyMonitorModStatusList(i).AssemblyMonitorModStatusID, EventLogID)
                                End If
                            Next
                            Session("mModelMonitorModtmp") = mModelMonitorMod
                            'RemoveSessionForExpiryInfo()
                            '''''' mdlPopUpChangeExpiryInfo.Hide()
                            ''''pnlExpiryInfo.Visible = False
                            upnlChangeExpiryInfo.Update()
                            SetControl()
                            SaveAttachment()
                            btnSendMail.Visible = True
                            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                        Catch ex As Exception
                            ex.GetBaseException()
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Public Sub SetUserMailIDs()
        Session("UserEmailID") = mModuleList.Item("UpdateSBRevision").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("UpdateSBRevision").SendCCMailID
        Session("MailsRequire") = mModuleList.Item("UpdateSBRevision").MailsRequire
        Session("SmtpHost") = mModuleList.Item("UpdateSBRevision").SmtpHost
        Session("SmtpPort") = mModuleList.Item("UpdateSBRevision").SmtpPort
        Session("SmtpUser") = mModuleList.Item("UpdateSBRevision").SmtpUser
        Session("SmtpPassword") = mModuleList.Item("UpdateSBRevision").SmtpPassword
    End Sub
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        Dim Str As String

        SetUserMailIDs()

        Session("btnSendMail") = "btnSendMail"
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Dim email As Thread
        Try
            Dim mModelMonitorModtmp As ModelMonitorMod
            mModelMonitorModtmp = Session("mModelMonitorModtmp")
            Dim str As String
            Dim mSendMailFile As New SendMailFile

            'Added by Saylee on 7-Sep-2020 for APFT07092020
            Dim ModName As String = "AD/SB(s)"
            Dim StrName As String = "SBRevision"
            If AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then
                ModName = "ACMD"
                'StrName = "ACMDRevision"
                StrName = "ACMD Revision" 'Added By Prashant On 5-Oct-2020 APFT05102020
            End If
            '*************************

            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following " & ModName & " has been Updated / Revised in FlyPal System and need your attentions." + "</font></P></br> ")
            str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")

            str = str + ("<p><font face=""Calibri"">")
            str = str + ("<b>Type: " + "</b>" + mModelMonitorModtmp.ModelMonitorModTypeName + "<b> Number:</b> " + mModelMonitorModtmp.Number + "<b>" + " Description: " + "</b>" + mModelMonitorModtmp.Description)
            str = str + ("</font></p>")

            str = str + ("<p><font face=""Calibri"">")


            str = str + ("<b>" + " Issue Date: " + "</b>" + txtNewIssueDate.Text)
            str = str + ("</font></p>")
          

            str = str + ("<p><font face=""Calibri"">")
            str = str + "<b>Note: " + "</b>" + txtNewNote.Text
            str = str + ("</font></p>")


            str = str + ("</body></html>")


            'Attachement Present
            Dim No As New Random

            Dim Attachmentpath As String = String.Empty
            '----------------------------------------------------------------------
            If mModelMonitorModtmp.IsAttachmentAdded And mFileAttach Is Nothing Then
                mFileAttach = FileAttach.GetAttachment(mModelMonitorModtmp.ID)
                Session("mFileAttach") = mFileAttach
            End If

            If Not mFileAttach Is Nothing Then

                If mFileAttach.Size > 0 Then
                    Attachmentpath = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(Attachmentpath)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = Attachmentpath

                    End If
                End If
            End If
            '******************************
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ModName + " Revision Notification", , str, _
                                    "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), Attachmentpath, False, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                     SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

            Dim mDirectiveDetail As String = ModName + " Revision Notification sent successfully to " + Session("ToSendMailIDs") + " by " + User.Identity.Name
            MarkLog(Util.Action.SendMail, "UpdateSBRevision", mDirectiveDetail, Util.ErrorType.HandledError, mModelMonitorModtmp.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)


        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        Finally
            ' Session.Remove("mModelMonitorModtmp")
        End Try

    End Sub
    Private Sub SaveAttachment() '  'Added By Saylee on 13-May-2020, LockDown 3.0
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mModelMonitorMod.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mModelMonitorMod.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If

    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mModelMonitorMod.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(Guid.Empty, 0)
        dgModelMonitorModList.DataSource = mModelMonitorModList
        Session("mModelMonitorModList") = mModelMonitorModList

        mModelList = ModelList.GetModelList(0, "", , , "(All)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList

        'Added By Vikrant On 04-Dec-2018 For ALL04122018
        mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(ALL)")
        cmbDirectiveType.DataSource = mModelMonitorModTypeList
        'End
        DataBind()
        lblResult.Text = "List of Directives : " & mModelMonitorModList.Count & " Record(s) found. "
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptUpdateSBRevision_Ajax.aspx"
            If cmbSearch.Enabled = True Then
                SetFocus(cmbSearch)
            End If
            DataFieldBind()
            'SetControl()

        End If
    End Sub
    Private Sub dgPartSearch_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelMonitorModList.RowCommand
        Select Case e.CommandName
            Case "ChangeExpiryInfo"    'Added Code
                dgModelMonitorModList.DataSource = mModelMonitorModList
                dgModelMonitorModList.DataBind()
                mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(New Guid(dgModelMonitorModList.DataKeys(CInt(e.CommandArgument)).Value.ToString()))
                Session("mModelMonitorMod") = mModelMonitorMod
                BindValueForChangeExpiryInfo()
                pnlExpiryInfo.Visible = True
                upnlChangeExpiryInfo.Update()
                mdlPopUpChangeExpiryInfo.Show()
                ControlVisibilityForAttachment()
                'BindGrid()
        End Select
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click
        dgModelMonitorModList.PageIndex = 0
        FindNow()
        upnlGrid.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "UpdateSBRevision", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgModelMonitorModList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModelMonitorModList.Sorting
        mModelMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorModList") = mModelMonitorModList
        dgModelMonitorModList.DataSource = mModelMonitorModList
        dgModelMonitorModList.DataBind()
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgModelMonitorModList.PageIndexChanging
        dgModelMonitorModList.PageIndex = e.NewPageIndex
        Session("mModelMonitorModList") = mModelMonitorModList
        dgModelMonitorModList.DataSource = mModelMonitorModList
        dgModelMonitorModList.DataBind()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mModelMonitorMod.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mModelMonitorMod.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mModelMonitorMod.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mModelMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mModelMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mModelMonitorMod.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
#End Region

#Region "Expiry Info"

#Region "Methods"
    Private Sub BindValueForChangeExpiryInfo()

        txtOldIssueDate.Text = mModelMonitorMod.IssueDateFormatted.ToString
        txtOldDirectiveNo.Text = mModelMonitorMod.Number
        txtOldNote.Text = mModelMonitorMod.Note

        txtNewIssueDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        txtNewDirectiveNo.Text = mModelMonitorMod.Number ' Old has set default user can change it also 
        txtNewNote.Text = ""

        upnlChangeExpiryInfo.DataBind()
    End Sub
    Public Sub SetGridFromObject(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus)
        Dim j As Int32
        ReDim PeriodValues(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1, 1)             'Actual Size   (dgDoneOnValue.Rows.Count , 2)'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        For j = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID      'To Check same Period'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 1) = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted) 'Period Value 'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID          'To Check same Period'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 1) = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)     'Period Value 'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                End If
                .Item(j).ExtensionValue = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted)  'Added By Saylee on 28-07-2008
            End With
        Next j
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    Private Sub UpdateModPeriods(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal NewDoneOnDate As String, ByVal mMachine As Machine)
        If CStr(mAssemblyMonitorModStatus.DoneOn.ToString) <> "" And NewDoneOnDate <> "" Then
            If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyMonitorModStatus.DoneOn.ToString), SmartDate.StringToDate(NewDoneOnDate)) <> 0 Then
                If Not IsDate(NewDoneOnDate) Then
                    mAssemblyMonitorModStatus.DoneOn = System.DBNull.Value
                Else
                    mAssemblyMonitorModStatus.DoneOn = NewDoneOnDate
                End If
                Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus = mAssemblyMonitorModStatus.Clone
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatus(mAssemblyMonitorModStatus.ID, mAssemblyMonitorModStatus.AssemblyStatusID, NewDoneOnDate, Guid.Empty, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
                SetGridFromObject(mAssemblyMonitorModStatus)
            End If
        End If
    End Sub
#End Region
#Region "Events"
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            MSGBoxCtrl.show("Save Alert!", "All configured directive(s) will also get updated along with Master", "Do you want to continue?", MsgBoxStyle.YesNo, "SaveConfig")
            Exit Sub
        End If
    End Sub
    Private Sub RemoveSessionForExpiryInfo()
        Session.Remove("mModelMonitorMod")
        Session.Remove("mFileAttach")
    End Sub
    Private Sub btnCloseChangeExpiryInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseChangeExpiryInfo.Click
        RemoveSessionForExpiryInfo()
        mdlPopUpChangeExpiryInfo.Hide()
        pnlExpiryInfo.Visible = False
        btnSendMail.Visible = False
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
#End Region

End Class