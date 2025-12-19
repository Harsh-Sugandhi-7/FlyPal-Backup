'AJAX Conversion by vikrant on 20-May-2015

Public Class wfModelMonitorServiceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Public mModelMonitorService As ModelMonitorService
    Public mModelMonitorServiceList As ModelMonitorServiceList
    Public mModel As Model
    Public mModelID As New Guid
    'Dim Type As Int32

    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList

    Dim EventLogID As Guid 'Added by Saylee on 22-July-2011
    Dim mFileAttach As FileAttach

    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
    Dim mPreviousAssemblyMonitorServiceStatusForRevise As AssemblyMonitorServiceStatus
    Public mIssueDate As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyMonitorServiceStatus = CType(Session("mAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        mModelMonitorService = CType(Session("mModelMonitorService"), ModelMonitorService)
        mModelMonitorServiceList = CType(Session("mModelMonitorServiceList"), ModelMonitorServiceList)
        mAssemblyMonitorServiceStatusList = CType(Session("mAssemblyMonitorServiceStatusList"), tmpAssemblyMonitorServiceStatusList)
        mPreviousAssemblyMonitorServiceStatusForRevise = Session("PreviousAssemblyMonitorServiceStatusForRevise") 'Revise Activity
    End Sub
    Private Sub SetSession()
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mModelMonitorService") = mModelMonitorService
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("Edit")
        Session.Remove("mModelMonitorServiceList")
        Session.Remove("FromModelMonitorServiceList")
    End Sub
    Private Sub DeleteRecord(ByVal index As Int32)
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModelMonitorServiceList.CurrentIndex = index
        mModelMonitorService = ModelMonitorService.GetModelMonitorService(mModelMonitorServiceList.Item(index).ID, mHourType)
        Session("mModelMonitorService") = mModelMonitorService
    End Sub
    Private Sub ControlVisibility()
        btnPrintTop.Visible = (mModelMonitorServiceList.Count > 15)
        btnAddNewTop.Visible = (mModelMonitorServiceList.Count > 15)
        btnBackTop.Visible = (mModelMonitorServiceList.Count > 15)

        btnPrint.Enabled = (mModelMonitorServiceList.Count > 0)
        btnPrintTop.Enabled = (mModelMonitorServiceList.Count > 0)
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
                            SetSession()
                            If mAssemblyMonitorServiceStatus.ModelMonitorServiceID.Equals(mModelMonitorService.ID) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "Record is currently in use, can not delete", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            If mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).ID)
                            End If
                            ModelMonitorService.DeleteModelMonitorService(mModelMonitorServiceList.CurrentItem.id)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'Added by vikrant 
                            If Session("ModelNameFromModelCreation") = Nothing Then
                                MarkLog(Util.Action.Delete, "Model Service", " Model : " & mAssemblyStatus.Assembly.ModelName & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description, Util.ErrorType.NoError, mModelMonitorService.ID, EventLogID)
                            Else
                                MarkLog(Util.Action.Delete, "Model Service", " Model : " & Session("ModelNameFromModelCreation") & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description, Util.ErrorType.NoError, mModelMonitorService.ID, EventLogID)
                            End If
                            DataFieldBind()
                            SetGrid()
                            ControlVisibility()
                            upnlDetails.Update()
                            upnlActionBtn.Update()
                            upnlActionBtnTop.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                ' MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Added by vikrant 
                                If Session("ModelNameFromModelCreation") = Nothing Then
                                    MarkLog(Util.Action.Delete, "Model Service", "Can't Delete : " & " Model : " & mAssemblyStatus.Assembly.ModelName & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description & " is already in use", Util.ErrorType.NoError, mModelMonitorService.ID, EventLogID)
                                Else
                                    MarkLog(Util.Action.Delete, "Model Service", "Can't Delete : " & " Model : " & Session("ModelNameFromModelCreation") & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description & " is already in use", Util.ErrorType.NoError, mModelMonitorService.ID, EventLogID)
                                End If
                                'Added by saylee on 1-Jun-2016
                                Dim mModelMonitorServiceConfiguredList As ModelMonitorConfiguredList
                                If Session("ModelIDFromModelCreation") = Nothing Then
                                    mModelMonitorServiceConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mAssemblyStatus.Assembly.ModelID, mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ID.ToString)
                                Else
                                    mModelMonitorServiceConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(New Guid(Session("ModelIDFromModelCreation").ToString), mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ID.ToString)
                                End If
                                If mModelMonitorServiceConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mModelMonitorServiceConfiguredList.Count - 1
                                        If i = mModelMonitorServiceConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                                        MSGBoxCtrl.Show("Deletion Alert!", "Selected MPD is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")
                                    Else
                                        MSGBoxCtrl.Show("Deletion Alert!", "Selected Service is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                    End If


                                End If
                            End If
                        End Try
                    End If

                    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                    If MSGBoxCtrl.Sender = "DifferentReviseActivity" Then

                        Dim mID As Guid = Session("ID")

                        If Session("NewPage") = "True" Then

                            Dim mHourType As Integer = 0

                            mIssueDate = Session("mIssueDate")

                            If mAssemblyStatus.IsSpareAssembly = True Then
                                mHourType = mAssemblyStatus.HourType
                            Else
                                mHourType = mMachine.HourType
                            End If

                            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid,
                                                                                                                         mAssemblyStatus.
                                                                                                                                    AssemblyID,
                                                                                                                         mAssemblyStatus.
                                                                                                                                        ID,
                                                                                                                         mIssueDate,
                                                                                                                         mAssemblyStatus.
                                                                                                                                    Assembly.ModelID,
                                                                                                                         mHourType)
                            mAssemblyMonitorServiceStatus.ModelMonitorServiceID(True) = mID
                            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                            RemoveSession()

                            mModelMonitorService = ModelMonitorService.GetModelMonitorService(mID, mHourType)
                            Session("mModelMonitorService") = mModelMonitorService

                            Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
                            Session.Remove("ID")
                            Session.Remove("ModelMonitorServiceIDToBeLinked") 'Revise Activity New
                            Response.Redirect("wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        Else

                            mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = mID
                            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                            RemoveSession()

                            Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
                            Session.Remove("ID")
                            Session.Remove("ModelMonitorServiceIDToBeLinked") 'Revise Activity New
                            Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" &
                                              Request.QueryString("BackPage") & "&ChildPage=" &
                                              Request.QueryString("ChildPage") & "&GChildPage=" &
                                              Request.QueryString("GChildPage") & "&GChildPage1=" &
                                              Request.QueryString("GChildPage1") & "&GChildPage2=" &
                                              Request.QueryString("GChildPage2"))

                        End If

                    End If

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
        End If
    End Sub
    Private Sub SetObject()
        mModelMonitorService = CType(Session("mModelMonitorService"), ModelMonitorService)
        With mAssemblyMonitorServiceStatus
            .ModelMonitorServiceID(False) = mModelMonitorService.ID
            '.ModelMonitorService.Code = mModelMonitorService.Code
            .ModelMonitorService.Reference = mModelMonitorService.Reference
            .ModelMonitorService.Description = mModelMonitorService.Description
            .ModelMonitorService.RequiredManHours = mModelMonitorService.RequiredManHours
            '.ModelMonitorService.ModelMonitorServiceTypeID = mModelMonitorService.ModelMonitorServiceTypeID
        End With
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
    End Sub
    Private Sub SetCaption()
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
        Else
            ServiceMPDTitle = "Services"
        End If


        If Session("ModelNameFromModelCreation") = Nothing Then
            lblTitle.Text = ServiceMPDTitle + " [ " & "Model: " & mAssemblyStatus.Assembly.ModelName & "]"
        Else
            lblTitle.Text = "Model " + ServiceMPDTitle + " List of - [ " & "Model: " & Session("ModelNameFromModelCreation").ToString & "]"
        End If
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
        Dim ID As Guid = Guid.NewGuid
        If Session("ModelIDFromModelCreation") = Nothing Then

            mModelMonitorService = ModelMonitorService.NewModelMonitorService(ID:=ID,
                                                                              ModelID:=mAssemblyStatus.Assembly.ModelID,
                                                                              HourType:=mHourType,
                                                                              PreviousRefID:=ID)
        ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
            mModelMonitorService = ModelMonitorService.NewModelMonitorService(ID:=ID, New Guid(Session("ModelIDFromModelCreation").ToString), 1, PreviousRefID:=ID)
        End If
        Session("mModelMonitorService") = mModelMonitorService
        mModelMonitorService.BeginEdit()
        'If mAssemblyStatus.IsMaster Then
        '    If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
        '        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '        msg.ReplacePage = "wfModelMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
        '        Session("sender") = "Authorization"
        '        msg.Show()
        '        Exit Sub
        '    End If
        'ElseIf Not mAssemblyStatus.IsMaster Then
        '    If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
        '        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '        msg.ReplacePage = "wfModelMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
        '        Session("sender") = "Authorization"
        '        msg.Show()
        '        Exit Sub
        '    End If
        'End If
        If Session("ModelNameFromModelCreation") = Nothing Then
            MarkLog(Util.Action.[New], "Model Service", " Model : " & mAssemblyStatus.Assembly.ModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            MarkLog(Util.Action.[New], "Model Service", " Model : " & Session("ModelNameFromModelCreation"), Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorServiceList_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelServiceMasterWindow", "OpenModelServiceMasterWindow()", True)
    End Sub
    Private Sub SetGrid()

        Dim P As Boolean

        For j As Integer = 0 To dgMonitorServiceList.Rows.Count - 1

            P = CType(Me.dgMonitorServiceList.Rows(j).Cells(14).Text, Boolean)

            If P = False Then
                dgMonitorServiceList.Rows(j).Cells(13).Enabled = False
            End If

            'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
            If Session("ModelMonitorServiceIDToBeLinked") IsNot Nothing Then

                Dim mID, PreviousRefID, PreviousID As Guid

                mID = New Guid(dgMonitorServiceList.DataKeys(j).Value.ToString)
                PreviousRefID = mModelMonitorServiceList(mAssemblyMonitorServiceStatus.ModelMonitorServiceID).PreviousRefID
                PreviousID = mModelMonitorServiceList(mAssemblyMonitorServiceStatus.ModelMonitorServiceID).ID

                If mModelMonitorServiceList(mID).PreviousRefID.Equals(PreviousRefID) And Not mID.Equals(PreviousID) Then

                    Me.dgMonitorServiceList.Rows.Item(j).BackColor = Color.OrangeRed
                    Me.dgMonitorServiceList.Rows.Item(j).ToolTip = "Revised Activity"
                    Me.dgMonitorServiceList.Rows.Item(j).ForeColor = Color.White

                End If

            End If

        Next

        If Not Session("OpenFromModelCreation") Is Nothing Then
            dgMonitorServiceList.Columns(10).Visible = False
        End If

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Session("ModelMonitorServicePreviousRefIDToBeLinked") Is Nothing Then
            If Session("ModelIDFromModelCreation") = Nothing Then
                mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(mAssemblyStatus.Assembly.ModelID)
            ElseIf Not Session("ModelIDFromModelCreation") Is Nothing Then
                mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(New Guid(Session("ModelIDFromModelCreation").ToString))
            End If
        End If

        dgMonitorServiceList.DataSource = mModelMonitorServiceList
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        dgMonitorServiceList.DataBind()



        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
            dgMonitorServiceList.HeaderRow.Cells(1).Text = "Task No."
        Else
            ServiceMPDTitle = "Services"
            dgMonitorServiceList.HeaderRow.Cells(1).Text = "Code/Form No."
        End If
        lblResult.Text = "List Of " + ServiceMPDTitle + " : " & mModelMonitorServiceList.Count & " Record(s) found."
        upnlDetails.Update()

    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM:put here the code to initialize the page
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            SetCaption()
            SetGrid()
            ControlVisibility()
        End If
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
            dgMonitorServiceList.HeaderRow.Cells(1).Text = "Task No."
            dgMonitorServiceList.ToolTip = "MPD List"
        Else
            ServiceMPDTitle = "Services"
            dgMonitorServiceList.HeaderRow.Cells(1).Text = "Code/Form No."
            dgMonitorServiceList.ToolTip = "Model Service List"
        End If
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        NewRecord()
    End Sub

    Private Sub GridViewRowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgMonitorServiceList.RowCommand

        Dim Index As Integer
        Dim mID As Guid
        Dim PreviousRefID, PreviousID 'Revise Activity

        Select Case e.CommandName

            Case "EditRec"

                Index = CInt(e.CommandArgument) + dgMonitorServiceList.PageIndex * dgMonitorServiceList.PageSize
                mID = New Guid(dgMonitorServiceList.DataKeys(Index).Value.ToString)

                'Added by Saylee on 10-Feb-2020,  All27072020
                Dim mHourType As Integer = 0
                If mAssemblyStatus.IsSpareAssembly = True Then
                    mHourType = mAssemblyStatus.HourType
                Else
                    mHourType = mMachine.HourType
                End If
                '*********************

                If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019 
                    mModelMonitorService = ModelMonitorService.GetModelMonitorService(mID, mHourType)
                Else
                    mModelMonitorService = ModelMonitorService.GetModelMonitorService(mID, 1)
                End If

                mModelMonitorService.BeginEdit()
                SetSession()
                Session("Edit") = True
                If Session("ModelNameFromModelCreation") = Nothing Then
                    MarkLog(Util.Action.Edit, "Model Service", "Model : " & mAssemblyStatus.Assembly.ModelName & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description, Util.ErrorType.NoError, mModelMonitorService.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Edit, "Model Service", "Model : " & Session("ModelNameFromModelCreation") & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description, Util.ErrorType.NoError, mModelMonitorService.ID, EventLogID)
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelServiceMasterWindow", "OpenModelServiceMasterWindow()", True)
                'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorServiceList_Ajax.aspx")

            Case "DeleteRec"

                Index = CInt(e.CommandArgument) + dgMonitorServiceList.PageIndex * dgMonitorServiceList.PageSize
                DeleteRecord(Index)

            Case "Select"

                Index = CInt(e.CommandArgument) + dgMonitorServiceList.PageIndex * dgMonitorServiceList.PageSize
                mID = New Guid(dgMonitorServiceList.DataKeys(Index).Value.ToString)
                'Added by Saylee on 10-Feb-2020,  All27072020

                'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                PreviousRefID = mModelMonitorServiceList(mAssemblyMonitorServiceStatus.ModelMonitorServiceID).PreviousRefID
                PreviousID = mModelMonitorServiceList(mAssemblyMonitorServiceStatus.ModelMonitorServiceID).ID

                If Not Session("ModelMonitorServiceIDToBeLinked") Is Nothing Then

                    If mModelMonitorServiceList(mID).PreviousRefID.Equals(PreviousRefID) And Not mID.Equals(PreviousID) Then
                        Session("IsLinkedActivitySelected") = True
                    Else

                        Session("ID") = mID
                        MSGBoxCtrl.Show("Alert!",
                                        "Activity you have selected is different than Revised Activity.
                                        <BR>If you select different activity,it will not be treated as Revised Activity & previous configuration will not become not applicable.",
                                        "Do you want to continue?",
                                        MsgBoxStyle.YesNo,
                                        "DifferentReviseActivity")

                        Session("IsLinkedActivitySelected") = False

                        Exit Sub

                    End If

                End If

                Dim mHourType As Integer = 0
                If mAssemblyStatus.IsSpareAssembly = True Then
                    mHourType = mAssemblyStatus.HourType
                Else
                    mHourType = mMachine.HourType
                End If
                '*********************

                If Session("NewPage") = "True" Or mPreviousAssemblyMonitorServiceStatusForRevise IsNot Nothing Then

                    'Added by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
                    mIssueDate = Session("mIssueDate")
                    mModelMonitorService = ModelMonitorService.GetModelMonitorService(mID)

                    If mPreviousAssemblyMonitorServiceStatusForRevise IsNot Nothing And mModelMonitorService.ReviseRemark <> "" Then

                        If mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString = "" Then
                            mIssueDate = mPreviousAssemblyMonitorServiceStatusForRevise.AsOnDateFormatted.ToString
                        Else
                            mIssueDate = mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString
                        End If

                    End If

                    If mPreviousAssemblyMonitorServiceStatusForRevise IsNot Nothing Then

                        If mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString = "" Then
                            mAssemblyMonitorServiceStatus.DoneOn = DBNull.Value
                        Else
                            mAssemblyMonitorServiceStatus.DoneOn = mPreviousAssemblyMonitorServiceStatusForRevise.DoneOnFormatted.ToString
                        End If

                    End If

                    'Saylee 29-Sep-2008
                    mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Session("mIssueDate"), mAssemblyStatus.Assembly.ModelID, mHourType)
                    '--------------------
                    mAssemblyMonitorServiceStatus.ModelMonitorServiceID(True) = mID
                    Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                    RemoveSession()

                    mModelMonitorService = ModelMonitorService.GetModelMonitorService(mModelMonitorServiceList.Item(Index).ID, mHourType)
                    Session("mModelMonitorService") = mModelMonitorService

                    'Code added by Saylee on 1/4/2008 Suggested by Deven sir
                    Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
                    Session.Remove("ModelMonitorServiceIDToBeLinked") 'Revise Activity
                    Response.Redirect("wfAssemblyMonitorServiceStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    '--------------------------------
                Else
                    'pls check this
                    mModelMonitorService = ModelMonitorService.GetModelMonitorService(mModelMonitorServiceList.Item(Index).ID, mHourType)
                    Session("mModelMonitorService") = mModelMonitorService
                    SetObject()
                    SetSession()
                    Session.Remove("Edit")
                    Session.Remove("mModelMonitorServiceList")
                    Session("FromModelMonitorServiceList") = True

                    'Code added by DEven on 14/2008
                    Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
                    Session.Remove("ModelMonitorServiceIDToBeLinked") 'Revise Activity
                    Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    '--------------------------------
                End If

            Case "View"

                Index = CInt(e.CommandArgument) + dgMonitorServiceList.PageIndex * dgMonitorServiceList.PageSize
                mID = New Guid(dgMonitorServiceList.DataKeys(Index).Value.ToString)

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

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
                End If

        End Select

    End Sub
    Private Sub Back(sender As Object, e As EventArgs) Handles btnBack.Click, btnBackTop.Click
        'SetSession()
        RemoveSession()

        '---- Open From Model Creation ----
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            'Session.Remove("mManufacturer")
            'Session.Remove("mManufacturerList")
            Session("ActiveTabIndex") = 0
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        '---- End Open From Model Creation ----

        MarkLog(Util.Action.Close,
                "Model Service",
                "Model : " & mAssemblyStatus.Assembly.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo,
                Util.ErrorType.NoError,
                Guid.Empty,
                EventLogID)

        'Modified by Harsh on 27th May 2024 for FLYPAL-1659 Revise Activity
        If Session("NewPage") = "True" Or Session("PreviousAssemblyMonitorServiceStatusForRevise") IsNot Nothing Then

            Session("NewPage") = "False"
            Session.Remove("ModelMonitorServiceIDToBeLinked")
            Response.Redirect(Request.QueryString("BackPage"))

        Else

            Session.Remove("ModelMonitorServiceIDToBeLinked")
            Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" &
                                  Request.QueryString("BackPage") & "&ChildPage=" &
                                  Request.QueryString("ChildPage") & "&GChildPage=" &
                                  Request.QueryString("GChildPage") & "&GChildPage1=" &
                                  Request.QueryString("GChildPage1") & "&GChildPage2=" &
                                  Request.QueryString("GChildPage2"))

        End If

    End Sub
    Private Sub dgMonitorServiceList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorServiceList.Sorting
        mModelMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        dgMonitorServiceList.DataSource = mModelMonitorServiceList
        dgMonitorServiceList.DataBind()
        SetGrid()
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
            dgMonitorServiceList.HeaderRow.Cells(1).Text = "Task No."
            dgMonitorServiceList.ToolTip = "MPD List"
        Else
            ServiceMPDTitle = "Services"
            dgMonitorServiceList.HeaderRow.Cells(1).Text = "Code/Form No."
            dgMonitorServiceList.ToolTip = "Model Service List"
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnModelServiceMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModelServiceMaster.Click
        DataFieldBind()
        SetGrid()
        ControlVisibility()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
        upnlDetails.Update()
    End Sub
#End Region

#Region " Report "
    'Created By :- Jyoti
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

        'Model Service List
        ' ReportDetails.Add(New rptStatus(, 0, ,
        ' , , , dgMonitorServiceList.HeaderRow.Cells(1).Text, , dgMonitorServiceList.Columns(2).HeaderText,
        ' dgMonitorServiceList.Columns(3).HeaderText, dgMonitorServiceList.Columns(4).HeaderText,
        'dgMonitorServiceList.Columns(5).HeaderText, "",
        ' "", dgMonitorServiceList.Columns(8).HeaderText))

        ReportDetails.Add(New rptStatus(, 0, ,
         , , , dgMonitorServiceList.HeaderRow.Cells(1).Text, , dgMonitorServiceList.Columns(2).HeaderText,
        dgMonitorServiceList.Columns(3).HeaderText, dgMonitorServiceList.Columns(4).HeaderText,
       dgMonitorServiceList.Columns(5).HeaderText, dgMonitorServiceList.Columns(9).HeaderText,
        dgMonitorServiceList.Columns(8).HeaderText))
        Dim TotalCount As Integer
        TotalCount = Me.mModelMonitorServiceList.Count
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
            If Me.dgMonitorServiceList.Rows(I).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceList.Rows(I).Cells(1).Text
            If Me.dgMonitorServiceList.Rows(I).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceList.Rows(I).Cells(2).Text
            If Me.dgMonitorServiceList.Rows(I).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceList.Rows(I).Cells(3).Text
            If Me.dgMonitorServiceList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMonitorServiceList.Rows(I).Cells(4).Text
            If Me.dgMonitorServiceList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMonitorServiceList.Rows(I).Cells(5).Text
            ' If CType(Me.mModelMonitorServiceList.Item(I).ShowInCofA, String) <> "&nbsp;" Then str(5) = CType(Me.mModelMonitorServiceList.Item(I).ShowInCofA, String)
            If Me.dgMonitorServiceList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgMonitorServiceList.Rows(I).Cells(9).Text '.Replace("<BR>", "Chr(10)")
            If Me.dgMonitorServiceList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(7) = Me.dgMonitorServiceList.Rows(I).Cells(8).Text

            'ReportDetails.Add(New rptStatus(, 1, ,
            '         , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7)))
            ReportDetails.Add(New rptStatus(, 1, ,
                     , , , str(0), , str(1), str(2), str(3), str(4), str(6), str(7), ""))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Model Service List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

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