'AJAX Conversion by vikrant on 21-May-2015

Public Class wfModelMonitorInspList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mModelMonitorInspList As ModelMonitorInspList
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mModelMonitorInsp As ModelMonitorInsp
    ''Added By Saylee on 9th Aug 2007
    Public mIssueDate As String

    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
    'Added by Vikrant on 28-July-2011
    Dim EventLogID As Guid
    Public mInspectionDetail As String
    Public mATA As String
    Public mModelName As String
    Public mMonitorDesc As String
    'Added By Vikrant on 13-Aug-2012 For ALL-13082012
    Public mModelMonitorInspTypeList As ModelMonitorInspTypeList
    Public SearchFor As String
    'End
    Dim mFileAttach As FileAttach
    Dim mPrevAssemblyMonitorInspStatusForRevise As AssemblyMonitorInspStatus  'Revise Activity New
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        mModelMonitorInspList = CType(Session("mModelMonitorInspList"), ModelMonitorInspList)
        mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
        mModelMonitorInspTypeList = Session("mModelMonitorInspTypeList") 'Added By Vikrant on 13-Aug-2012 For ALL-13082012
        mPrevAssemblyMonitorInspStatusForRevise = Session("mPrevAssemblyMonitorInspStatusForRevise") 'Revise Activity New
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
    End Sub
    Private Sub RemoveSession()
        If Session("ModelMonitorInspPrevRefIDToBeLinked") Is Nothing Then Session.Remove("mModelMonitorInspList")
        Session.Remove("mModelMonitorInspTypeList") 'Added By Vikrant on 13-Aug-2012 For ALL-13082012
        Session.Remove("LookIn")
        Session.Remove("txtFor")
        Session.Remove("txtCode")
        Session.Remove("SearchFor")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModelMonitorInspList.CurrentIndex = Index
        Session("mModelMonitorInspList") = mModelMonitorInspList
    End Sub
    Private Sub NewRecord()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Dim mModelMonitorInsp As ModelMonitorInsp
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        If Session("ModelIDFromModelCreation") = Nothing Then
            mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(ID, mAssemblyStatus.Assembly.ModelID, mHourType, ID) 'For new records ID,PrevRefID are same
        ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
            mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(ID, New Guid(Session("ModelIDFromModelCreation").ToString), 1, ID) 'For new records ID,PrevRefID are same
        End If
        Session("mModelMonitorInsp") = mModelMonitorInsp
        RemoveSession()
        'Response.Redirect("wfModelMonitorInspection_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorInspList_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow()", True)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)

        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************

        Dim mModelMonitorInsp As ModelMonitorInsp
        If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
            mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mId, mHourType)
        Else
            mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mId, 1)
        End If

        Session("mModelMonitorInsp") = mModelMonitorInsp
        'Added by Vikrant on 28-July-2011
        mModelName = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ModelName
        mATA = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ATAChapter
        ' mMonitorDesc = mModelMonitorInsp.Description 'mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).Description
        'mInspectionDetail = "Model : " + mModelName + " ATA : " + mATA + " Description : " + mMonitorDesc
        mInspectionDetail = "Model : " & mModelName & " Model Inspection Type : " & mModelMonitorInsp.ModelMonitorInspTypeName & " Description : " & mModelMonitorInsp.Description
        MarkLog(Util.Action.Edit, "Model Inspection", mInspectionDetail, Util.ErrorType.NoError, mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID, EventLogID)
        'End
        RemoveSession()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow()", True)
        'Response.Redirect("wfModelMonitorInspection_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorInspList_Ajax.aspx")
    End Sub
    Private Sub ControlVisibility()
        btnPrintTop.Visible = (mModelMonitorInspList.Count > 15)
        btnAddNewTop.Visible = (mModelMonitorInspList.Count > 15)
        btnBackTop.Visible = (mModelMonitorInspList.Count > 15)

        btnPrint.Enabled = (mModelMonitorInspList.Count > 0)
        btnPrintTop.Enabled = (mModelMonitorInspList.Count > 0)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            If mModelMonitorInspList(mModelMonitorInspList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mModelMonitorInspList(mModelMonitorInspList.CurrentIndex).ID)
                            End If
                            ModelMonitorInsp.DeleteModelMonitorInsp(mModelMonitorInspList.CurrentItem.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'Added by Vikrant on 28-July-2011
                            mModelName = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ModelName
                            mATA = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ATAChapter
                            mMonitorDesc = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).Description
                            mInspectionDetail = "Model : " + mModelName + " ATA : " + mATA + " Description : " + mMonitorDesc
                            MarkLog(Util.Action.Delete, "Model Inspection", mInspectionDetail, Util.ErrorType.NoError, mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID, EventLogID)
                            'End
                            FindNow()
                            DataFieldBind()
                            SetGrid()
                            ControlVisibility()
                            upnlGrid.Update()
                            upnlActionBtn.Update()
                            upnlActionBtnTop.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")'Added by Vikrant on 28-July-2011
                                mATA = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ATAChapter
                                mMonitorDesc = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).Description
                                mInspectionDetail = "Model : " + mModelName + " ATA : " + mATA + " Description : " + mMonitorDesc
                                mModelName = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ModelName

                                'Added by saylee on 1-Jun-2016
                                Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
                                If Session("ModelIDFromModelCreation") = Nothing Then
                                    mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mAssemblyStatus.Assembly.ModelID, mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID.ToString)
                                ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                                    mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(New Guid(Session("ModelIDFromModelCreation").ToString), mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID.ToString)
                                End If
                                If mModelMonitorConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                                        If i = mModelMonitorConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected Inspection is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
                                MarkLog(Util.Action.Delete, "Model Inspection", "Can't Delete:" & mInspectionDetail & " is already in use", Util.ErrorType.NoError, mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                    'Revise Activity New
                    If MSGBoxCtrl.Sender = "DifferentReviseActivity" Then
                        Dim mID As Guid = Session("ID")
                        If Session("NewPage") = "True" Then
                            'Saylee 29-Sep-2008
                            mIssueDate = Session("mIssueDate")

                            'Added by Saylee on 10-Feb-2020,  All27072020
                            Dim mHourType As Integer = 0
                            If mAssemblyStatus.IsSpareAssembly = True Then
                                mHourType = mAssemblyStatus.HourType
                            Else
                                mHourType = mMachine.HourType
                            End If
                            '*********************

                            'mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID,  mModelMonitorInsp., mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mIssueDate, mAssemblyStatus.Assembly.ModelID, mHourType)
                            mAssemblyMonitorInspStatus.ModelMonitorInspID(True) = mID
                            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                            RemoveSession()
                            'Code added by Saylee on 1/4/2008 Suggested by Deven sir
                            'Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))

                            mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mID, mHourType)
                            Session("mModelMonitorInsp") = mModelMonitorInsp

                            Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
                            Session.Remove("ID")
                            Session.Remove("ModelMonitorInspIDToBeLinked") 'Revise Activity New
                            Response.Redirect("wfAssemblyMonitorInspStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                            '--------------------------------
                        Else
                            mAssemblyMonitorInspStatus.ModelMonitorInspID(False) = mID
                            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                            RemoveSession()

                            'Code added by DEven on 14/2008
                            'Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))

                            Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
                            Session.Remove("ID")
                            Session.Remove("ModelMonitorInspIDToBeLinked") 'Revise Activity New
                            Response.Redirect("wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                            '--------------------------------
                        End If
                    End If
                    'End
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
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        If Session("ModelNameFromModelCreation") = Nothing Then
            lblTitle.Text = "Model Inspection List of - [ " & "Model: " & mAssemblyStatus.Assembly.ModelName & "]"
        Else
            lblTitle.Text = "Model Inspection List of - [ " & "Model: " & Session("ModelNameFromModelCreation").ToString & "]"
        End If
        lblResult.Text = "List Of Model Inspection: " & mModelMonitorInspList.Count & " Record(s)"
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgModelMonitorInspList.Rows.Count - 1
            P = CType(Me.dgModelMonitorInspList.Rows(j).Cells(16).Text, Boolean)
            If P = False Then
                dgModelMonitorInspList.Rows(j).Cells(15).Enabled = False
            End If
            'Revise Activity New
            If Not Session("ModelMonitorInspIDToBeLinked") Is Nothing Then

                Dim mID, PrevRefID, PrevID As Guid
                mID = New Guid(dgModelMonitorInspList.DataKeys(j).Value.ToString)
                PrevRefID = mModelMonitorInspList(mAssemblyMonitorInspStatus.ModelMonitorInspID).PrevRefID
                PrevID = mModelMonitorInspList(mAssemblyMonitorInspStatus.ModelMonitorInspID).ID

                If mModelMonitorInspList(mID).PrevRefID.Equals(PrevRefID) And Not mID.Equals(PrevID) Then
                    Me.dgModelMonitorInspList.Rows.Item(j).BackColor = Color.OrangeRed
                    Me.dgModelMonitorInspList.Rows.Item(j).ToolTip = "Revised Activity"
                    Me.dgModelMonitorInspList.Rows.Item(j).ForeColor = Color.White
                End If
            End If
            'End
        Next
        If Not Session("OpenFromModelCreation") Is Nothing Then
            dgModelMonitorInspList.Columns(11).Visible = False
        End If
    End Sub
    'Added By Vikrant on 13-Aug-2012 For ALL-13082012
    Private Sub FindNow()
        dgModelMonitorInspList.PageIndex = 0
        If Session("ModelMonitorInspPrevRefIDToBeLinked") Is Nothing Then



            Select Case cmbLookIn.SelectedIndex
                Case 0 'All
                    If Session("ModelIDFromModelCreation") = Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyStatus.Assembly.ModelID, IsRII:=chkIsRII.Checked)
                    ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(New Guid(Session("ModelIDFromModelCreation").ToString), IsRII:=chkIsRII.Checked)
                    End If
                Case 1 'ATA Code
                    If Session("ModelIDFromModelCreation") = Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyStatus.Assembly.ModelID, 0, Val(Trim(txtCode.Text)), IsRII:=chkIsRII.Checked)
                    ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(New Guid(Session("ModelIDFromModelCreation").ToString), 0, Val(Trim(txtCode.Text)), IsRII:=chkIsRII.Checked)
                    End If
                Case 2 'Description 
                    If Session("ModelIDFromModelCreation") = Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyStatus.Assembly.ModelID, 0, 0, "", Trim(txtFor.Text), IsRII:=chkIsRII.Checked)
                    ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(New Guid(Session("ModelIDFromModelCreation").ToString), 0, 0, "", Trim(txtFor.Text), IsRII:=chkIsRII.Checked)
                    End If
                Case 3 'Inspection Type 
                    If Session("ModelIDFromModelCreation") = Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyStatus.Assembly.ModelID, CInt(cmbSearchFor.SelectedValue), 0, "", "", IsRII:=chkIsRII.Checked)
                    ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(New Guid(Session("ModelIDFromModelCreation").ToString), CInt(cmbSearchFor.SelectedValue), 0, "", "", IsRII:=chkIsRII.Checked)
                    End If
                Case 4 'Reference
                    If Session("ModelIDFromModelCreation") = Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyStatus.Assembly.ModelID, 0, 0, "", "", Trim(txtFor.Text), IsRII:=chkIsRII.Checked)
                    ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(New Guid(Session("ModelIDFromModelCreation").ToString), 0, 0, "", "", Trim(txtFor.Text))
                    End If
            End Select
        End If
        dgModelMonitorInspList.DataSource = mModelMonitorInspList
        Session("mModelMonitorInspList") = mModelMonitorInspList
        dgModelMonitorInspList.DataBind()

        Session("LookIn") = cmbLookIn.SelectedIndex
        Session("txtFor") = txtFor.Text
        Session("txtCode") = txtCode.Text
        SearchFor = IIf(cmbSearchFor.SelectedIndex <= 0, "", cmbSearchFor.SelectedValue) 'cmbSearchFor.SelectedIndex
        Session("SearchFor") = SearchFor
    End Sub
    Private Sub ControlVisibility(ByVal Index As Integer)
        txtFor.Text = IIf(Index = 2 Or Index = 4, txtFor.Text, "")
        txtCode.Text = IIf(Index = 1, txtCode.Text, "")
        lblFor.Visible = IIf(Index > 0, True, False)
        txtFor.Visible = IIf((Index = 2 Or Index = 4), True, False)
        txtCode.Visible = IIf(Index = 1, True, False)
        cmbSearchFor.Visible = IIf(Index = 3, True, False)
    End Sub
    Private Sub SetControls()
        txtFor.Text = Session("txtFor")
        txtCode.Text = Session("txtCode")
        cmbLookIn.SelectedIndex = Session("LookIn")
        cmbSearchFor.SelectedValue = IIf(SearchFor = "", 0, SearchFor)
        ControlVisibility(cmbLookIn.SelectedIndex)
        'FindNow()
    End Sub
    'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        '''If Session("ModelIDFromModelCreation") = Nothing Then
        '''    mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyStatus.Assembly.ModelID, IsRII:=chkIsRII.Checked)
        '''ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
        '''    mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(New Guid(Session("ModelIDFromModelCreation").ToString), IsRII:=chkIsRII.Checked)
        '''End If
        dgModelMonitorInspList.DataSource = mModelMonitorInspList
        Session("mModelMonitorInspList") = mModelMonitorInspList
        'Added By Vikrant on 13-Aug-2012 For ALL-13082012
        mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(ALL)")
        cmbSearchFor.DataSource = mModelMonitorInspTypeList
        Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
        'End
        dgModelMonitorInspList.DataBind()
        cmbSearchFor.DataBind()
        lblResult.Text = "List Of Model Inspection: " & mModelMonitorInspList.Count & " Record(s)"
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            'Added By Vikrant on 13-Aug-2012 For ALL-13082012
            SetControls()
            FindNow()
            DataFieldBind()
            'End
            SetPage()
            SetGrid()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgModelMonitorInspList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelMonitorInspList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Dim PrevRefID, PrevID 'Revise Activity New
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgModelMonitorInspList.PageIndex * dgModelMonitorInspList.PageSize
                mID = New Guid(dgModelMonitorInspList.DataKeys(Index).Value.ToString)
                EditRecord(mID)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgModelMonitorInspList.PageIndex * dgModelMonitorInspList.PageSize
                DeleteRecord(Index)
            Case "Select"
                Index = CInt(e.CommandArgument) + dgModelMonitorInspList.PageIndex * dgModelMonitorInspList.PageSize
                mID = New Guid(dgModelMonitorInspList.DataKeys(Index).Value.ToString)

                'Revise Activity New
                PrevRefID = mModelMonitorInspList(mAssemblyMonitorInspStatus.ModelMonitorInspID).PrevRefID
                PrevID = mModelMonitorInspList(mAssemblyMonitorInspStatus.ModelMonitorInspID).ID
                If Not Session("ModelMonitorInspIDToBeLinked") Is Nothing Then
                    If mModelMonitorInspList(mID).PrevRefID.Equals(PrevRefID) And Not mID.Equals(PrevID) Then
                        Session("IsLinkedActivitySelected") = True
                    Else
                        Session("ID") = mID
                        MSGBoxCtrl.show("Alert!", "Activity you have selected is different than Revised Activity.<BR>If you select different activity,it will not be treated as Revised Activity & previous configuration will not become not applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "DifferentReviseActivity")
                        Session("IsLinkedActivitySelected") = False
                        Exit Sub
                    End If
                End If
                'End
                'Added by Saylee on 10-Feb-2020,  All27072020
                Dim mHourType As Integer = 0
                If mAssemblyStatus.IsSpareAssembly = True Then
                    mHourType = mAssemblyStatus.HourType
                Else
                    mHourType = mMachine.HourType
                End If
                '*********************
                If Session("NewPage") = "True" Or Not mPrevAssemblyMonitorInspStatusForRevise Is Nothing Then 'Revise Activity New OR Condition 
                    'Saylee 29-Sep-2008
                    mIssueDate = Session("mIssueDate")
                    'Revise Activity New
                    mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mID)
                    If Not mPrevAssemblyMonitorInspStatusForRevise Is Nothing And mModelMonitorInsp.ReviseRemark <> "" Then
                        If mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString = "" Then
                            mIssueDate = mPrevAssemblyMonitorInspStatusForRevise.AsOnDateFormatted.ToString
                        Else
                            mIssueDate = mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString
                        End If
                    End If
                    'End
                    'mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID,  mModelMonitorInsp., mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
                    mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mIssueDate, mAssemblyStatus.Assembly.ModelID, mHourType)
                    mAssemblyMonitorInspStatus.ModelMonitorInspID(True) = mID
                    'Revise Activity New
                    If Not mPrevAssemblyMonitorInspStatusForRevise Is Nothing Then
                        If mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString = "" Then
                            mAssemblyMonitorInspStatus.DoneOn = System.DBNull.Value
                        Else
                            mAssemblyMonitorInspStatus.DoneOn = mPrevAssemblyMonitorInspStatusForRevise.DoneOnFormatted.ToString
                        End If
                    End If
                    'End
                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                    RemoveSession()
                    'Code added by Saylee on 1/4/2008 Suggested by Deven sir
                    'Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))

                    mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mID, mHourType)
                    Session("mModelMonitorInsp") = mModelMonitorInsp

                    Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
                    Session.Remove("ModelMonitorInspIDToBeLinked") 'Revise Activity New
                    Response.Redirect("wfAssemblyMonitorInspStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    '--------------------------------
                Else
                    mAssemblyMonitorInspStatus.ModelMonitorInspID(False) = mID
                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                    RemoveSession()

                    'Code added by DEven on 14/2008
                    'Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))

                    Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
                    Session.Remove("ModelMonitorInspIDToBeLinked") 'Revise Activity New
                    Response.Redirect("wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    '--------------------------------
                End If
                '=======================Saylee on 9 Aug 2007==========================
            Case "View"
                Index = CInt(e.CommandArgument) + dgModelMonitorInspList.PageIndex * dgModelMonitorInspList.PageSize
                mID = New Guid(dgModelMonitorInspList.DataKeys(Index).Value.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mManual.FileExtension
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                Else
                    'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfModelMonitorInspList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                    'msg1.Show()
                End If
                ''If (Not User.IsInRole("FileAttach")) Then
                ''    SetSession()
                ''    MarkLog(Util.Action.Attach, "PartMonitorInsp", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                'mDocumentTypeForID = 9   'In csDocumentTypeFor Table, Model Monitor Inspection has ID=9
                'mAttachToID = Id
                'mName = "Model:" & mModelMonitorInspList.Item(Id).ModelName
                'Session("mDocumentTypeForID") = mDocumentTypeForID
                'Session("mAttachToID") = mAttachToID
                'Session("mName") = mName
                ''Dim str As String
                ''str = "<script language='javascript'>openledgersame('wfAttachFiles.aspx?MainBackPage=wfModelMonitorInspList.aspx');</script>"
                '' ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", str)

                ''Changed By Saylee on 12th Dec 2007 to solv bug-MMI1 of Aircraft Master given By Pramod
                'Response.Redirect("wfAttachFiles.aspx?MainBackPage=wfModelMonitorInspList.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))

                '=====================================================================
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        'Changed by Vikrant on 28-July-2011
        MarkLog(Util.Action.[New], "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            'Session.Remove("mManufacturer")
            'Session.Remove("mManufacturerList")
            Session("ActiveTabIndex") = 0
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        'Changed by Vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Model Inspection", "Model : " & mAssemblyStatus.Assembly.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)

        If Session("NewPage") = "True" Or Not Session("mPrevAssemblyMonitorInspStatusForRevise") Is Nothing Then  'Added By Saylee on 1st Oct-2008  'Revise Activity New
            Session("NewPage") = "False"
            Session.Remove("ModelMonitorInspIDToBeLinked") 'Revise Activity New
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            Session.Remove("ModelMonitorInspIDToBeLinked") 'Revise Activity New
            Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        End If
    End Sub
    Private Sub dgModelMonitorInspList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModelMonitorInspList.Sorting
        mModelMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorInspList") = mModelMonitorInspList
        dgModelMonitorInspList.DataSource = mModelMonitorInspList
        dgModelMonitorInspList.DataBind()
        SetGrid()
    End Sub
    'Added By Vikrant on 13-Aug-2012 For ALL-13082012
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        SetPage()
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub cmbLookIn_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLookIn.SelectedIndexChanged
        cmbSearchFor.SelectedIndex = 0
        ControlVisibility(cmbLookIn.SelectedIndex)
        If cmbLookIn.Enabled = True Then
            cmbLookIn.Focus()
        End If
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnModelInspMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelInspMaster.Click
        FindNow()
        DataFieldBind()
        SetGrid()
        ControlVisibility()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
        upnlGrid.Update()
    End Sub
#End Region

#Region " Report "
    '    'Created By :- Jyoti
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        Rpt = New crListModelMonitor
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        'Model Monitor Inspection List
        'Commented By Saylee for hiding Show in C Of A Column  on 22-08-2008
        'ReportDetails.Add(New rptStatus(, 0, , _
        ', , , dgModelMonitorInspList.Columns.Item(1).HeaderText, , dgModelMonitorInspList.Columns.Item(3).HeaderText, _
        'dgModelMonitorInspList.Columns.Item(4).HeaderText, dgModelMonitorInspList.Columns.Item(5).HeaderText, _
        'dgModelMonitorInspList.Columns.Item(6).HeaderText, dgModelMonitorInspList.Columns.Item(7).HeaderText, _
        'dgModelMonitorInspList.Columns.Item(8).HeaderText, dgModelMonitorInspList.Columns.Item(9).HeaderText))

        ReportDetails.Add(New rptStatus(, 0, , _
               , , , dgModelMonitorInspList.Columns.Item(1).HeaderText, , dgModelMonitorInspList.Columns.Item(3).HeaderText, _
               dgModelMonitorInspList.Columns.Item(4).HeaderText, dgModelMonitorInspList.Columns.Item(5).HeaderText, _
               dgModelMonitorInspList.Columns.Item(6).HeaderText, _
               dgModelMonitorInspList.Columns.Item(7).HeaderText, dgModelMonitorInspList.Columns.Item(9).HeaderText))
        Dim TotalCount As Integer
        TotalCount = Me.mModelMonitorInspList.Count
        Dim I As Integer

        Dim str(7) As String

        For I = 0 To TotalCount - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            If Me.dgModelMonitorInspList.Rows(I).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgModelMonitorInspList.Rows(I).Cells(1).Text
            If Me.dgModelMonitorInspList.Rows(I).Cells(3).Text <> "&nbsp;" Then str(1) = Me.dgModelMonitorInspList.Rows(I).Cells(3).Text
            If Me.dgModelMonitorInspList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(2) = Me.dgModelMonitorInspList.Rows(I).Cells(4).Text
            If Me.dgModelMonitorInspList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(3) = Me.dgModelMonitorInspList.Rows(I).Cells(5).Text
            If Me.dgModelMonitorInspList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(4) = Me.dgModelMonitorInspList.Rows(I).Cells(6).Text
            If CType(Me.mModelMonitorInspList.Item(I).IsRII, String) <> "&nbsp;" Then str(5) = IIf(CType(mModelMonitorInspList.Item(I).IsRII, String) = "True", "Yes", "")
            'If Me.dgModelMonitorInspList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(6) = Me.dgModelMonitorInspList.Rows(I).Cells(8).Text
            If Me.dgModelMonitorInspList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgModelMonitorInspList.Rows(I).Cells(9).Text

            'ReportDetails.Add(New rptStatus(, 1, , _
            '     , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7)))

            ReportDetails.Add(New rptStatus(, 1, , _
                     , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Model Inspection List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptimage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptimage)
        da.Fill(ds, Report)

        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region


End Class