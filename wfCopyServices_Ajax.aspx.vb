Imports System.Collections.Generic
Public Class wfCopyServices_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAssemblyTypeList As AssemblyTypeList
    Public mCopyModelList As ModelList
    Public mServiceTypeList As ModelMonitorServiceTypeList
    Public mModelMonitorServiceList As ModelMonitorServiceList
    Public mtmpModelMonitorServiceList As ModelMonitorServiceList
    Public mErrorString As String

    Public AssemblyTypeId As Integer
    Public mSourceModel As String
    Public mDestinationModel As String
    Public mServiceType As Integer

    Public mSourceModelID As Guid
    Public mDestinationModelID As Guid

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
        mServiceTypeList = CType(Session("mServiceTypeList"), ModelMonitorServiceTypeList)
        mModelMonitorServiceList = CType(Session("mModelMonitorServiceList"), ModelMonitorServiceList)
        mErrorString = Session("mErrorString")

        AssemblyTypeId = Session("AssemblyTypeId")
        mSourceModel = Session("mSourceModel")
        mDestinationModel = Session("mDestinationModel")
        mServiceType = Session("mServiceType")

        mSourceModelID = CType(Session("mSourceModelID"), Guid)
        mDestinationModelID = CType(Session("mDestinationModelID"), Guid)
    End Sub
    Private Sub SetSession()
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mCopyModelList") = mCopyModelList
        Session("mServiceTypeList") = mServiceTypeList
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        Session("mErrorString") = mErrorString

        Session("AssemblyTypeId") = AssemblyTypeId
        Session("mSourceModel") = mSourceModel
        Session("mDestinationModel") = mDestinationModel
        Session("mServiceType") = mServiceType

        Session("mSourceModelID") = mSourceModelID
        Session("mDestinationModelID") = mDestinationModelID

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mCopyModelList")
        Session.Remove("mServiceTypeList")
        Session.Remove("mModelMonitorServiceList")
        Session.Remove("mErrorString")

        Session.Remove("AssemblyTypeId")
        Session.Remove("mSourceModel")
        Session.Remove("mDestinationModel")
        Session.Remove("mServiceType")

        Session.Remove("mSourceModelID")
        Session.Remove("mDestinationModelID")
        Session.Remove("Copied")
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
        If Session("MiddleFrame") <> "wfCopyServices_Ajax.aspx" Then
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
                    'Response.Redirect("wfCopyServices.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfCopyServices.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetControl()
        GetSession()
        If Session("AssemblyTypeId") <> Nothing Then cmbAssemblyType.SelectedValue = AssemblyTypeId
        If Session("mServiceType") <> Nothing Then cmbServiceType.SelectedIndex = mServiceType
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

        If cmbServiceType.SelectedIndex = 0 Then
            mServiceType = 0
        Else
            mServiceType = cmbServiceType.SelectedIndex
        End If

        If cmbAssemblyType.SelectedIndex = 0 Then
            AssemblyTypeId = 0
        Else
            AssemblyTypeId = CInt(cmbAssemblyType.SelectedValue)
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
        If mModelMonitorServiceList Is Nothing Then
            lblModelServiceList.Text = "Service List : 0 Record(s)"
        Else
            lblModelServiceList.Text = "Service List for Model  ' " + cmbSourceModel.SelectedItem.ToString + " '  : " + mModelMonitorServiceList.Count.ToString + " Record(s)"
        End If
        txtListError.Text = ""
    End Sub
    Private Sub SetGrid()
        Dim IsAttachmentAdded As Boolean
        For j As Integer = 0 To dgModelServiceList.Rows.Count - 1
            IsAttachmentAdded = CType(Me.dgModelServiceList.Rows(j).Cells(12).Text, Boolean)
            If IsAttachmentAdded = False Then
                dgModelServiceList.Rows(j).Cells(11).Enabled = False 'View
            End If
        Next
    End Sub
    'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        AssemblyTypeId = IIf(IsNothing(AssemblyTypeId), 0, AssemblyTypeId)
        mServiceType = IIf(IsNothing(mServiceType), 0, mServiceType)
        mSourceModel = Session("mSourceModel")
        mDestinationModel = Session("mDestinationModel")
        mSourceModelID = IIf(IsNothing(mSourceModelID), Guid.Empty, mSourceModelID)
        mDestinationModelID = IIf(IsNothing(mDestinationModelID), Guid.Empty, mDestinationModelID)

        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList("(SELECT)")
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList

        mServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("(All)")
        cmbServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

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

        dgModelServiceList.DataSource = mModelMonitorServiceList
        dgModelServiceList.DataBind()

        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mServiceTypeList") = mServiceTypeList
        DataBind()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by vikrant on 03-Aug-2011
        If Not IsPostBack And Session("sender") = "" Then

            Session("MiddleFrame") = "wfCopyServices_Ajax.aspx"
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
    Private Sub btnNewModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewModel.Click
        Dim AssemblyTypeID As Integer = CInt(cmbAssemblyType.SelectedValue)
        SetVariables()
        Session("MiddleFrame") = "wfCopyServices_Ajax.aspx"
        Session("FromCopyUtility") = "True"
        ControlVisibility()
        Dim str As String
        'str = "<script language='javascript'>openledgersame('wfModel.aspx?ChildPage1=Index.aspx&Type=False&AssemblyTypeId=" & AssemblyTypeID & "');</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)

        str = "openledgersame('wfModel_Ajax.aspx?ChildPage1=Index.aspx&Type=False&AssemblyTypeId=" & AssemblyTypeID & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click, btnCopyTop.Click
        txtListError.Text = ""
        SetVariables()

        If Not IsValid Then Exit Sub
        'ALL04032019
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one Service to copy.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'End
        Session("Copied") = ""
        mErrorString = ""
        Session("mErrorString") = ""

        If Not mCopyModelList.Item(cmbSourceModel.SelectedIndex).ID.Equals(mCopyModelList.Item(cmbDestinationModel.SelectedIndex).ID) Then
            'ALL04032019 mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(New Guid(cmbSourceModel.SelectedValue.ToString), CInt(cmbServiceType.SelectedValue))
            mtmpModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(New Guid(cmbDestinationModel.SelectedValue.ToString), CInt(cmbServiceType.SelectedValue))
            Dim i As Integer
            If mModelMonitorServiceList.Count > 0 Then
                If Not checkString Is Nothing Then
                    Dim values = checkString.Split(","c)
                    For Each value As String In values
                        Dim oldModelService As ModelMonitorService
                        Dim mModelMonitorService As ModelMonitorService

                        oldModelService = ModelMonitorService.GetModelMonitorService(New Guid(value), 1)
                        If Not mtmpModelMonitorServiceList.Contains(oldModelService) Then

                            Dim ID As Guid = Guid.NewGuid
                            mModelMonitorService = ModelMonitorService.NewModelMonitorService(ID:=ID,
                                                                                              ModelID:=mCopyModelList.Item(cmbDestinationModel.SelectedIndex).ID,
                                                                                              HourType:=1,
                                                                                              PreviousRefID:=ID)
                            mModelMonitorService.Code = oldModelService.Code
                            mModelMonitorService.ATAID = oldModelService.ATAID
                            mModelMonitorService.Reference = oldModelService.Reference
                            mModelMonitorService.Description = oldModelService.Description
                            mModelMonitorService.ModelMonitorServiceTypeID = oldModelService.ModelMonitorServiceTypeID
                            mModelMonitorService.Note = oldModelService.Note
                            mModelMonitorService.ShowInCofA = oldModelService.ShowInCofA
                            mModelMonitorService.RequiredManHours = oldModelService.RequiredManHours
                            mModelMonitorService.ImageFile = oldModelService.ImageFile
                            mModelMonitorService.Size = oldModelService.Size
                            mModelMonitorService.Extension = oldModelService.Extension
                            'ALL04032019
                            mModelMonitorService.IsAttachmentAdded = oldModelService.IsAttachmentAdded
                            If mModelMonitorService.IsAttachmentAdded Then
                                Dim tempFileAttach As FileAttach = FileAttach.GetAttachment(oldModelService.ID)
                                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mModelMonitorService.ID, tempFileAttach.ImageFile, tempFileAttach.Size, tempFileAttach.Extension, tempFileAttach.Sort)
                            End If
                            'End
                            For j As Integer = 0 To oldModelService.ModelMonitorServicePeriods.Count - 1
                                mModelMonitorService.ModelMonitorServicePeriods.Add(mModelMonitorService.ID, oldModelService.ModelMonitorServicePeriods.Item(j).PeriodUnitID, oldModelService.ModelMonitorServicePeriods.Item(j).PeriodID, 1)
                                mModelMonitorService.ModelMonitorServicePeriods(j).MonitorTypeID = mServiceTypeList(oldModelService.ModelMonitorServiceTypeID).MonitorTypeID
                                If mServiceTypeList(oldModelService.ModelMonitorServiceTypeID).MonitorTypeID = 3 Then
                                    mModelMonitorService.ModelMonitorServicePeriods.Item(j).FrequencyValue = CStr(0)
                                Else
                                    mModelMonitorService.ModelMonitorServicePeriods.Item(j).FrequencyValue = oldModelService.ModelMonitorServicePeriods.Item(j).FrequencyValue
                                End If
                            Next j


                            'Added byb Saylee on 23-Jul-2018 for ALL23072018,  to give facility for copying  Spare/Tools/Task cards also
                            'Tools,Spare & Tasks
                            Dim mOldMaintenanceTaskAndKit As MaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(oldModelService)

                            If Not mOldMaintenanceTaskAndKit Is Nothing Then


                                Dim mNewMaintenanceTaskAndKit As MaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)
                                If Not mNewMaintenanceTaskAndKit Is Nothing Then
                                    mModelMonitorService.MaintenanceKitID = mNewMaintenanceTaskAndKit.MaintenanceKitID
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

                                If mModelMonitorService.IsValid Then
                                    mModelMonitorService = CType(mModelMonitorService.Save(), ModelMonitorService)
                                    Session("Copied") = "True"
                                End If
                            Catch ex As SqlException
                                If ex.Number = 2627 Or ex.Number = 50000 Then
                                    mErrorString = mErrorString + vbNewLine + "Description : " + mModelMonitorService.Description + " Service Type : " + mModelMonitorService.ModelMonitorServiceTypeName + " - Duplicate"
                                    Session("Copied") = "False"
                                Else
                                    mErrorString = mErrorString + vbNewLine + "Description : " + mModelMonitorService.Description + " Service Type : " + mModelMonitorService.ModelMonitorServiceTypeName + " - " + ex.Message
                                End If
                            Finally

                            End Try
                            mModelMonitorService = Nothing
                        Else
                            mErrorString = mErrorString + vbNewLine + "Description : " + oldModelService.Description + " Service Type : " + oldModelService.ModelMonitorServiceTypeName + " - Duplicate"
                            Session("Copied") = "False"
                        End If
                    Next
                End If
                Dim mDetail As String = "Assembly Type : " + cmbAssemblyType.SelectedItem.Text + ", " + "Source Model : " + cmbSourceModel.SelectedItem.Text + ", " + "Destination Model : " + cmbDestinationModel.SelectedItem.Text + ", " + "Service Type : " + IIf(cmbServiceType.SelectedIndex <= 0, "All", cmbServiceType.SelectedItem.Text)
                MarkLog(Util.Action.Save, "CopyServices", mDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, " ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Session("mErrorString") = mErrorString
            txtListError.Text = mErrorString

            If Session("Copied") = "True" Then
                MSGBoxCtrl.Show("Copied Successfully", "Model Services(s) has been copied successfully.", "", MsgBoxStyle.OkOnly, "")
            ElseIf Session("Copied") = "False" Then
                MSGBoxCtrl.Show("Duplication Alert !!", "Model Services(s) has been copied successfully excluding duplicate Services.", "", MsgBoxStyle.OkOnly, "")
            End If
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.SelectRestriction, MSGBox.Message_text.SelectRestriction, "Destination Model different from the Source Model.", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        'Added by vikrant on 3-Aug-2011
        MarkLog(Util.Action.Close, "CopyServices", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        txtListError.Text = ""
        RemoveSession()
        Session("MiddleFrame") = ""
        ' '' FileClose(1)
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'ALL04032019
    Private Sub cmbSourceModel_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbSourceModel.SelectedIndexChanged
        If cmbSourceModel.SelectedIndex > 0 Then
            mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(New Guid(cmbSourceModel.SelectedValue.ToString), CInt(cmbServiceType.SelectedValue))
            dgModelServiceList.DataSource = mModelMonitorServiceList
            dgModelServiceList.DataBind()
            Session("mModelMonitorServiceList") = mModelMonitorServiceList
        Else
            mModelMonitorServiceList = Nothing
            dgModelServiceList.DataSource = mModelMonitorServiceList
            dgModelServiceList.DataBind()
            Session("mModelMonitorServiceList") = mModelMonitorServiceList
        End If
        setTitle()
        SetGrid()
    End Sub
    Private Sub dgPartList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgModelServiceList.PageIndexChanging
        'If Not mModelMonitorServiceList Is Nothing Then

        '    For Each Child As ModelMonitorInspList.ModelMonitorInspInfo In mModelMonitorServiceList
        '        Child.IsSelect = mRequisitionNew.RequisitionItemsNew.Contains(Child.ItemID)
        '        If mRequisitionNew.RequisitionItemsNew.Contains(Child.ItemID) Then
        '            checkedIds.Add(Child.ID.ToString)
        '        End If
        '    Next
        'End If
        dgModelServiceList.PageIndex = e.NewPageIndex
        dgModelServiceList.DataSource = mModelMonitorServiceList
        Session("mModelMonitorServiceList") = mModelMonitorServiceList
        dgModelServiceList.DataBind()
        setTitle()
        SetGrid()
    End Sub
    Private Sub dgModelServiceList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgModelServiceList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(New Guid(dgModelServiceList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, [GetType], "openFile", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub cmbServiceType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbServiceType.SelectedIndexChanged
        If cmbSourceModel.SelectedIndex > 0 Then
            mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(New Guid(cmbSourceModel.SelectedValue.ToString), CInt(cmbServiceType.SelectedValue))
            dgModelServiceList.DataSource = mModelMonitorServiceList
            dgModelServiceList.DataBind()
            Session("mModelMonitorServiceList") = mModelMonitorServiceList
        Else
            mModelMonitorServiceList = Nothing
            dgModelServiceList.DataSource = mModelMonitorServiceList
            dgModelServiceList.DataBind()
            Session("mModelMonitorServiceList") = mModelMonitorServiceList
        End If
        setTitle()
        SetGrid()
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