Imports System.Collections.Generic
Public Class wfCopyModification_Ajax
    Inherits Page

#Region " Variable Declaration "
    Public mAssemblyTypeList As AssemblyTypeList
    Public mCopyModelList As ModelList
    Public mModificationTypeList As ModelMonitorModTypeList
    Public mModelMonitorModList As ModelMonitorModList
    Public mErrorString As String

    Public AssemblyTypeId As Integer
    Public mSourceModel As String
    Public mDestinationModel As String
    Public mModificationType As Integer

    Public mSourceModelID As Guid
    Public mDestinationModelID As Guid
    Public Path As String = AppSettings("DOCPath") & "LOG"

    Dim EventLogID As Guid
    'ALL04032019
    Private checkedIds As New List(Of String)()
    Dim mFileAttach As FileAttach
    'End
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mCopyModelList = CType(Session("mCopyModelList"), ModelList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mModelMonitorModList = CType(Session("mModelMonitorModList"), ModelMonitorModList)
        mErrorString = Session("mErrorString")

        AssemblyTypeId = Session("AssemblyTypeId")
        mSourceModel = Session("mSourceModel")
        mDestinationModel = Session("mDestinationModel")
        mModificationType = Session("mModificationType")

        mSourceModelID = CType(Session("mSourceModelID"), Guid)
        mDestinationModelID = CType(Session("mDestinationModelID"), Guid)
    End Sub
    Private Sub SetSession()
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mCopyModelList") = mCopyModelList
        Session("mModificationTypeList") = mModificationTypeList
        Session("mModelMonitorModList") = mModelMonitorModList
        Session("mErrorString") = mErrorString

        Session("AssemblyTypeId") = AssemblyTypeId
        Session("mSourceModel") = mSourceModel
        Session("mDestinationModel") = mDestinationModel
        Session("mModificationType") = mModificationType

        Session("mSourceModelID") = mSourceModelID
        Session("mDestinationModelID") = mDestinationModelID
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mCopyModelList")
        Session.Remove("mModificationTypeList")
        Session.Remove("mModelMonitorModList")
        Session.Remove("mErrorString")

        Session.Remove("AssemblyTypeId")
        Session.Remove("mSourceModel")
        Session.Remove("mDestinationModel")
        Session.Remove("mModificationType")

        Session.Remove("mSourceModelID")
        Session.Remove("mDestinationModelID")
        Session.Remove("Copied")
        Session.Remove("FromCopyModification")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    'Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "cmbSourceModel" Then
    '        If cmbSourceModel.SelectedIndex <= 0 Then
    '            custValidator.ErrorMessage = "Select the Source Model from the list"
    '            e.IsValid = False
    '        End If
    '    ElseIf custValidator.ControlToValidate = "cmbDestinationModel" Then
    '        If cmbDestinationModel.SelectedIndex <= 0 Then
    '            custValidator.ErrorMessage = "Select Destination Model from the list."
    '            e.IsValid = False
    '        End If
    '    End If
    'End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfCopyModification_Ajax.aspx" Then
            txtListError.Text = ""
            RemoveSession()
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

                Case MsgBoxResult.Cancel

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    SetControl()
                    'Response.Redirect("wfCopyModification.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfCopyModification.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetControl()
        GetSession()
        If Session("AssemblyTypeId") <> Nothing Then cmbAssemblyType.SelectedValue = AssemblyTypeId
        If Session("mModificationType") <> Nothing Then cmbModificationType.SelectedIndex = mModificationType
        If Session("mErrorString") <> Nothing Then txtListError.Text = mErrorString
    End Sub
    Private Sub SetVariables()

        If Not mCopyModelList.Item(cmbSourceModel.SelectedIndex).ID.Equals(Guid.Empty) Then
            mSourceModelID = New Guid(cmbSourceModel.SelectedValue.ToString)
        Else
            mSourceModelID = Guid.Empty
        End If

        If Not mCopyModelList.Item(cmbDestinationModel.SelectedIndex).ID.Equals(Guid.Empty) Then
            mDestinationModelID = New Guid(cmbDestinationModel.SelectedValue.ToString)
        Else
            mDestinationModelID = Guid.Empty
        End If

        If cmbModificationType.SelectedIndex = 0 Then
            mModificationType = 0
        Else
            mModificationType = cmbModificationType.SelectedIndex
        End If

        If cmbAssemblyType.SelectedIndex = 0 Then
            AssemblyTypeId = 0
        Else
            AssemblyTypeId = CInt(cmbAssemblyType.SelectedValue) 'cmbAssemblyType.SelectedIndex
        End If
        SetSession()
    End Sub
    Private Sub ControlVisibility()
        cmbSourceModel.Enabled = IIf(cmbAssemblyType.SelectedIndex > 0, True, False)
        cmbDestinationModel.Enabled = IIf(cmbAssemblyType.SelectedIndex > 0, True, False)
        btnCopyTop.Enabled = IIf(cmbAssemblyType.SelectedIndex > 0, True, False)
        btnNewModel.Enabled = IIf(cmbAssemblyType.SelectedIndex > 0, True, False)
    End Sub
    'ALL04032019
    Private Sub setTitle()
        If mModelMonitorModList Is Nothing Then
            lblModelModList.Text = "Directive List : 0 Record(s)"
        Else
            lblModelModList.Text = "Directive List for Model  ' " + cmbSourceModel.SelectedItem.ToString + " '  : " + mModelMonitorModList.Count.ToString + " Record(s)"
        End If
        txtListError.Text = ""
    End Sub
    Private Sub SetGrid()
        Dim IsAttachmentAdded As Boolean
        For j As Integer = 0 To dgModelModList.Rows.Count - 1
            IsAttachmentAdded = CType(Me.dgModelModList.Rows(j).Cells(13).Text, Boolean)
            If IsAttachmentAdded = False Then
                dgModelModList.Rows(j).Cells(11).Enabled = False 'View
            End If
        Next
    End Sub
    'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        AssemblyTypeId = IIf(IsNothing(AssemblyTypeId), 0, AssemblyTypeId)
        mModificationType = IIf(IsNothing(mModificationType), 0, mModificationType)
        mSourceModel = Session("mSourceModel")
        mDestinationModel = Session("mDestinationModel")
        mSourceModelID = IIf(IsNothing(mSourceModelID), Guid.Empty, mSourceModelID)   'Session("mSourceModelID")
        mDestinationModelID = IIf(IsNothing(mDestinationModelID), Guid.Empty, mDestinationModelID) 'Session("mDestinationModelID")
        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList("(SELECT)")
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(All)")
        cmbModificationType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList

        If Not mCopyModelList Is Nothing Then
            mCopyModelList = ModelList.GetModelList(CInt(AssemblyTypeId), "", , , "(SELECT)")

            If mCopyModelList.Count > 0 Then
                cmbSourceModel.DataSource = mCopyModelList
                cmbDestinationModel.DataSource = mCopyModelList
                Session("mCopyModelList") = mCopyModelList
                DataBind()
                setFocus(cmbAssemblyType)
                ControlVisibility()
            End If
        End If

        dgModelModList.DataSource = mModelMonitorModList
        dgModelModList.DataBind()
        Session("mModelMonitorModList") = mModelMonitorModList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by vikrant on 03-Aug-2011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfCopyModification_Ajax.aspx"
            If cmbAssemblyType.Enabled = True Then
                setFocus(cmbAssemblyType)
            End If
            txtListError.Text = ""
            DataFieldBind()
            SetControl()
            cmbSourceModel.SelectedValue = mSourceModelID.ToString
            cmbDestinationModel.SelectedValue = mDestinationModelID.ToString
            'ALL04032019
            setTitle()
            SetGrid()
            'End
        End If
        ControlVisibility()
        'MessageBoxResult()
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged

        mCopyModelList = ModelList.GetModelList(CInt(mAssemblyTypeList(cmbAssemblyType.SelectedIndex).ID), "", , , "(SELECT)")
        If mCopyModelList.Count > 0 Then
            cmbSourceModel.DataSource = mCopyModelList
            cmbDestinationModel.DataSource = mCopyModelList
            Session("mCopyModelList") = mCopyModelList
            DataBind()
            setFocus(cmbAssemblyType)
            ControlVisibility()
            'ALL04032019
            setTitle()
            SetGrid()
            'End
        End If
    End Sub
    Private Sub btnNewModel_Click(sender As Object, e As EventArgs) Handles btnNewModel.Click
        Dim AssemblyTypeID As Integer = CInt(cmbAssemblyType.SelectedValue)
        SetVariables()
        Session("MiddleFrame") = "wfCopyModification_Ajax.aspx"
        Session("FromCopyUtility") = "True"
        ControlVisibility()
        Dim str As String
        str = "openledgersame('wfModel_Ajax.aspx?ChildPage1=Index.aspx&Type=False&AssemblyTypeId=" & AssemblyTypeID & "');"
        ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", str, True)
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click, btnCopyTop.Click
        txtListError.Text = ""
        SetVariables()

        If Not IsValid Then Exit Sub
        'ALL04032019
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one Directive to copy.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'End
        Session("Copied") = ""
        mErrorString = ""
        Session("mErrorString") = ""

        If Not mCopyModelList.Item(cmbSourceModel.SelectedIndex).ID.Equals(mCopyModelList.Item(cmbDestinationModel.SelectedIndex).ID) Then
            'ALL04032019 mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(cmbSourceModel.SelectedValue.ToString), CInt(cmbModificationType.SelectedValue))
            If mModelMonitorModList.Count > 0 Then
                If Not checkString Is Nothing Then
                    Dim values = checkString.Split(","c)
                    For Each value As String In values
                        Dim oldModelMod As ModelMonitorMod
                        Dim mModelMonitorMod As ModelMonitorMod
                        oldModelMod = ModelMonitorMod.GetModelMonitorMod(New Guid(value), 1)
                        Dim ID As Guid = Guid.NewGuid 'Revise Activity
                        mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, New Guid(cmbDestinationModel.SelectedValue.ToString), 1, ID)
                        mModelMonitorMod.Code = oldModelMod.Code
                        mModelMonitorMod.Reference = oldModelMod.Reference
                        mModelMonitorMod.Description = oldModelMod.Description
                        mModelMonitorMod.Number = oldModelMod.Number
                        mModelMonitorMod.IssueDate = oldModelMod.IssueDate

                        mModelMonitorMod.Note = oldModelMod.Note
                        mModelMonitorMod.ATAID = oldModelMod.ATAID
                        mModelMonitorMod.ModelMonitorModTypeID = oldModelMod.ModelMonitorModTypeID
                        mModelMonitorMod.ShowInCofA = oldModelMod.ShowInCofA
                        mModelMonitorMod.Applicability = oldModelMod.Applicability
                        mModelMonitorMod.ComplianceRequirement = oldModelMod.ComplianceRequirement
                        mModelMonitorMod.RequiredManHours = oldModelMod.RequiredManHours
                        mModelMonitorMod.ImageFile = oldModelMod.ImageFile
                        mModelMonitorMod.Size = oldModelMod.Size
                        mModelMonitorMod.Extension = oldModelMod.Extension
                        'ALL04032019
                        mModelMonitorMod.IsAttachmentAdded = oldModelMod.IsAttachmentAdded
                        If mModelMonitorMod.IsAttachmentAdded Then
                            Dim tempFileAttach As FileAttach = FileAttach.GetAttachment(oldModelMod.ID)
                            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mModelMonitorMod.ID, tempFileAttach.ImageFile, tempFileAttach.Size, tempFileAttach.Extension, tempFileAttach.Sort)
                        End If
                        'End
                        For j As Integer = 0 To oldModelMod.ModelMonitorModPeriods.Count - 1
                            mModelMonitorMod.ModelMonitorModPeriods.Add(mModelMonitorMod.ID, oldModelMod.ModelMonitorModPeriods.Item(j).PeriodUnitID, oldModelMod.ModelMonitorModPeriods.Item(j).PeriodID, 1)
                            mModelMonitorMod.ModelMonitorModPeriods(j).MonitorTypeID = mModificationTypeList(oldModelMod.ModelMonitorModTypeID).MonitorTypeID
                            If mModificationTypeList(mModelMonitorMod.ModelMonitorModTypeID).MonitorTypeID = 3 Then
                                mModelMonitorMod.ModelMonitorModPeriods.Item(j).FrequencyValue = CStr(0)
                            Else
                                mModelMonitorMod.ModelMonitorModPeriods.Item(j).FrequencyValue = oldModelMod.ModelMonitorModPeriods.Item(j).FrequencyValue
                            End If
                        Next j


                        'Added byb Saylee on 23-Jul-2018 for ALL23072018,  to give facility for copying  Spare/Tools/Task cards also
                        'Tools,Spare & Tasks
                        Dim mOldMaintenanceTaskAndKit As MaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(oldModelMod)

                        If Not mOldMaintenanceTaskAndKit Is Nothing Then


                            Dim mNewMaintenanceTaskAndKit As MaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)
                            If Not mNewMaintenanceTaskAndKit Is Nothing Then
                                mModelMonitorMod.MaintenanceKitID = mNewMaintenanceTaskAndKit.MaintenanceKitID
                            End If

                            'Tools
                            Try
                                If Not mOldMaintenanceTaskAndKit.MaintenanceToolID.Equals(Guid.Empty) Then
                                    Dim mToolsOldMaintenanceKit As MaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mOldMaintenanceTaskAndKit.ID, True)
                                    If mToolsOldMaintenanceKit.MaintenanceKitDetails.Count > 0 Then
                                        Dim mToolsNewMaintenanceKit As MaintenanceKit = MaintenanceKit.NewMaintenanceKit(mNewMaintenanceTaskAndKit.MaintenanceTypeID, mNewMaintenanceTaskAndKit.ID, mNewMaintenanceTaskAndKit.IsAssembly, True)
                                        For j As Integer = 0 To mToolsOldMaintenanceKit.MaintenanceKitDetails.Count - 1
                                            mToolsNewMaintenanceKit.MaintenanceKitDetails.Add(mToolsNewMaintenanceKit.ID)
                                            mToolsNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = mToolsNewMaintenanceKit.MaintenanceKitDetails.CurrentIndex + 1
                                            mToolsNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = mToolsOldMaintenanceKit.MaintenanceKitDetails(j).ItemID
                                            mToolsNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = mToolsOldMaintenanceKit.MaintenanceKitDetails(j).Qty
                                            mToolsNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.Note = mToolsOldMaintenanceKit.MaintenanceKitDetails(j).Note
                                            mToolsNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = mToolsOldMaintenanceKit.MaintenanceKitDetails(j).Remark
                                        Next
                                        mToolsNewMaintenanceKit.Save()
                                        mNewMaintenanceTaskAndKit.MaintenanceToolID = mToolsNewMaintenanceKit.ID
                                    End If
                                End If

                            Catch ex As SqlException

                            End Try

                            'Spares
                            Try
                                If Not mOldMaintenanceTaskAndKit.MaintenanceKitID.Equals(Guid.Empty) Then
                                    Dim mSparesOldMaintenanceKit As MaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mOldMaintenanceTaskAndKit.ID, False)
                                    If mSparesOldMaintenanceKit.MaintenanceKitDetails.Count > 0 Then
                                        Dim mSparesNewMaintenanceKit As MaintenanceKit = MaintenanceKit.NewMaintenanceKit(mNewMaintenanceTaskAndKit.MaintenanceTypeID, mNewMaintenanceTaskAndKit.ID, mNewMaintenanceTaskAndKit.IsAssembly, False)
                                        For j As Integer = 0 To mSparesOldMaintenanceKit.MaintenanceKitDetails.Count - 1
                                            mSparesNewMaintenanceKit.MaintenanceKitDetails.Add(mSparesNewMaintenanceKit.ID)
                                            mSparesNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = mSparesNewMaintenanceKit.MaintenanceKitDetails.CurrentIndex + 1
                                            mSparesNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = mSparesOldMaintenanceKit.MaintenanceKitDetails(j).ItemID
                                            mSparesNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = mSparesOldMaintenanceKit.MaintenanceKitDetails(j).Qty
                                            mSparesNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.Note = mSparesOldMaintenanceKit.MaintenanceKitDetails(j).Note
                                            mSparesNewMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = mSparesOldMaintenanceKit.MaintenanceKitDetails(j).Remark
                                        Next
                                        Try
                                            mSparesNewMaintenanceKit.Save()
                                            mNewMaintenanceTaskAndKit.MaintenanceKitID = mSparesNewMaintenanceKit.ID
                                        Catch ex As Exception

                                        End Try
                                    End If
                                End If

                            Catch ex As SqlException

                            End Try


                            'Tasks
                            Try
                                If Not mOldMaintenanceTaskAndKit.MaintenanceTaskID.Equals(Guid.Empty) Then
                                    Dim mOLDMaintenanceTask As MaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mOldMaintenanceTaskAndKit.ID)
                                    If mOLDMaintenanceTask.MaintenanceTaskDetails.Count > 0 Then
                                        Dim mNEWMaintenanceTask As MaintenanceTask = MaintenanceTask.NewMaintenanceTask(mNewMaintenanceTaskAndKit.MaintenanceTypeID, mNewMaintenanceTaskAndKit.ID, mNewMaintenanceTaskAndKit.IsAssembly)
                                        For j As Integer = 0 To mOLDMaintenanceTask.MaintenanceTaskDetails.Count - 1
                                            mNEWMaintenanceTask.MaintenanceTaskDetails.Add(mNEWMaintenanceTask.ID)
                                            mNEWMaintenanceTask.MaintenanceTaskDetails.CurrentItem.SrNo = mNEWMaintenanceTask.MaintenanceTaskDetails.CurrentIndex + 1
                                            mNEWMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardID = mOLDMaintenanceTask.MaintenanceTaskDetails(j).TaskCardID
                                            mNEWMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardNo = mOLDMaintenanceTask.MaintenanceTaskDetails(j).TaskCardNo
                                            mNEWMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task = mOLDMaintenanceTask.MaintenanceTaskDetails(j).Task
                                            mNEWMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Note = ""

                                        Next
                                        Try
                                            mNEWMaintenanceTask.Save()
                                            mNewMaintenanceTaskAndKit.MaintenanceTaskID = mNEWMaintenanceTask.ID
                                        Catch ex As Exception

                                        End Try
                                    End If

                                End If
                            Catch ex As Exception

                            End Try

                        End If
                        '********************************************************************************************************
                        Try
                            If mModelMonitorMod.IsValid Then
                                mModelMonitorMod = CType(mModelMonitorMod.Save(), ModelMonitorMod)
                                Session("Copied") = "True"
                            End If
                        Catch ex As SqlException
                            If ex.Number = 2627 Or ex.Number = 50000 Then
                                mErrorString = mErrorString + vbNewLine + mModelMonitorMod.Number + " - Duplicate"
                                Session("Copied") = "False"
                            Else
                                mErrorString = mErrorString + vbNewLine + mModelMonitorMod.Number + " - " + ex.Message
                            End If
                        Finally

                        End Try
                        mModelMonitorMod = Nothing
                    Next
                End If
                Dim mDetail As String = "Assembly Type : " + cmbAssemblyType.SelectedItem.Text + ", " + "Source Model : " + cmbSourceModel.SelectedItem.Text + ", " + "Destination Model : " + cmbDestinationModel.SelectedItem.Text + ", " + "Directive Type : " + IIf(cmbModificationType.SelectedIndex <= 0, "All", cmbModificationType.SelectedItem.Text)
                MarkLog(Action.Save, "CopyModifications", mDetail, ErrorType.NoError, Guid.Empty, EventLogID)
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, " ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Session("mErrorString") = mErrorString
            txtListError.Text = mErrorString
            If Session("Copied") = "True" Then
                MSGBoxCtrl.Show("Copied Successfully", "Model Directive(s) has been copied successfully.", "", MsgBoxStyle.OkOnly, "")
            ElseIf Session("Copied") = "False" Then
                MSGBoxCtrl.Show("Duplication Alert !!", "Model Directive(s) has been copied successfully excluding duplicate Directives.", "", MsgBoxStyle.OkOnly, "")
            End If
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.SelectRestriction, MSGBox.Message_text.SelectRestriction, "Destination Model different from the Source Model.", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        'Added by vikrant on 3-Aug-2011
        MarkLog(Util.Action.Close, "CopyModifications", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        txtListError.Text = ""
        RemoveSession()
        Session("MiddleFrame") = ""
        'FileClose(1)
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSaveLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveLog.Click

    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'ALL04032019
    Private Sub cmbSourceModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSourceModel.SelectedIndexChanged
        If cmbSourceModel.SelectedIndex > 0 Then
            mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(cmbSourceModel.SelectedValue.ToString), CInt(cmbModificationType.SelectedValue))
            dgModelModList.DataSource = mModelMonitorModList
            dgModelModList.DataBind()
            Session("mModelMonitorModList") = mModelMonitorModList
        Else
            mModelMonitorModList = Nothing
            dgModelModList.DataSource = mModelMonitorModList
            dgModelModList.DataBind()
            Session("mModelMonitorModList") = mModelMonitorModList
        End If
        setTitle()
        SetGrid()
    End Sub
    Private Sub dgModelModList_PageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles dgModelModList.PageIndexChanging
        dgModelModList.PageIndex = e.NewPageIndex
        dgModelModList.DataSource = mModelMonitorModList
        Session("mModelMonitorModList") = mModelMonitorModList
        dgModelModList.DataBind()
        setTitle()
        SetGrid()
    End Sub
    Private Sub cmbModificationType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbModificationType.SelectedIndexChanged
        If cmbSourceModel.SelectedIndex > 0 Then
            mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(cmbSourceModel.SelectedValue.ToString), CInt(cmbModificationType.SelectedValue))
            dgModelModList.DataSource = mModelMonitorModList
            dgModelModList.DataBind()
            Session("mModelMonitorModList") = mModelMonitorModList
        Else
            mModelMonitorModList = Nothing
            dgModelModList.DataSource = mModelMonitorModList
            dgModelModList.DataBind()
            Session("mModelMonitorModList") = mModelMonitorModList
        End If
        setTitle()
        SetGrid()
    End Sub
    Private Sub dgModelInspList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelModList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(New Guid(dgModelModList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
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
        End Select
    End Sub
    'End
#End Region

#Region "Checked Selection"
    'ALL04032019
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
    'End
#End Region

End Class