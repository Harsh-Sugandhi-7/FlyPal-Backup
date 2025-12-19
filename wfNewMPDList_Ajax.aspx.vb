'Added by vikrant For MPD

Public Class wfNewMPDList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Variable Declaration "
    Public mAssemblyTypeList As AssemblyTypeList
    Public mModelList As ModelList
    Public mModelMonitorInspList As ModelMonitorInspList
    Public mModelMonitorInsp As ModelMonitorInsp
    Public mATAList As ATAList
    Public mModelMonitorInspTypeList As ModelMonitorInspTypeList
    Dim EventLogID As Guid
    Public mInspectionDetail, mServiceDetail As String
    Public mATA As String
    Public mModelName As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim SelectedModelIndex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA, SelectedMonitorServiceType As Integer
    Dim Description As String = String.Empty
    Dim NewMPDTabIndex As Integer
    Dim MPDNo As String = String.Empty

    Public mModelMonitorServiceList As ModelMonitorServiceList
    Public mModelMonitorServiceTypeList As ModelMonitorServiceTypeList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelList = CType(Session("mModelList"), ModelList)
        mModelMonitorInspList = CType(Session("mModelMonitorInspList"), ModelMonitorInspList)
        SelectedModelIndex = IIf(Session("SelectedModelIndex") Is Nothing, 0, Session("SelectedModelIndex"))
        SelectedAssemblyTypeIndex = IIf(Session("SelectedAssemblyTypeIndex") Is Nothing, 0, Session("SelectedAssemblyTypeIndex"))
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorInspTypeList = CType(Session("mModelMonitorInspTypeList"), ModelMonitorInspTypeList)
        SelectedMonitorType = IIf(Session("SelectedMonitorType") Is Nothing, 0, Session("SelectedMonitorType"))
        ATA = IIf(Session("ATA") Is Nothing, 0, Session("ATA"))
        Description = IIf(Session("ModelDescription") Is Nothing, String.Empty, Session("ModelDescription"))
        MPDNo = IIf(Session("MPDNo") Is Nothing, String.Empty, Session("MPDNo"))

        mModelMonitorServiceList = CType(Session("mModelMonitorServiceListForNewMPD"), ModelMonitorServiceList)
        mModelMonitorServiceTypeList = CType(Session("mModelMonitorServiceTypeListForNewMPD"), ModelMonitorServiceTypeList)
        SelectedMonitorServiceType = IIf(Session("SelectedMonitorServiceTypeForNewMPD") Is Nothing, 0, Session("SelectedMonitorServiceTypeForNewMPD"))
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "NewMPD"


        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
        End Select
    End Function
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfNewMPDList_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub FindNow()
        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(ModelID:=mModelList(SelectedModelIndex).ID, InspectionType:=mModelMonitorInspTypeList(SelectedMonitorType, "").ID, ATACode:=mATAList(ATA).ATACode, ATANomenclature:=String.Empty, Description:=Description, MPDNO:=MPDNo)
        Session("mModelMonitorInspList") = mModelMonitorInspList
        dgModelMonitorInspList.DataSource = mModelMonitorInspList
        dgModelMonitorInspList.DataBind()

        Dim Reccount As Integer = 0
        If AppSettings("ClientCode") = "IAT" Then
            Reccount = mModelMonitorInspList.RecordCount
        Else
            Reccount = mModelMonitorInspList.Count
        End If

        lblResult.Text = "List Of MPD: " & Reccount & " Record(s)"


        mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(ModelID:=mModelList(SelectedModelIndex).ID, ServiceType:=mModelMonitorServiceTypeList(SelectedMonitorServiceType, "").ID, ATACode:=mATAList(ATA).ATACode, ATANomenclature:=String.Empty, Description:=Description, TaskCardNo:=MPDNo)
        Session("mModelMonitorServiceListForNewMPD") = mModelMonitorServiceList
        dgModelMonitorServiceList.DataSource = mModelMonitorServiceList
        dgModelMonitorServiceList.DataBind()
        lblResultService.Text = "List Of MPD: " & mModelMonitorServiceList.Count & " Record(s)"

        upnlGridService.Update()


        SetGrid()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelList")
        Session.Remove("mModelMonitorInspList")
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorInspTypeList")
        Session.Remove("MPDNo")
        Session.Remove("mModelMonitorServiceListForNewMPD")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModelMonitorInspList.CurrentIndex = Index
        Session("mModelMonitorInspList") = mModelMonitorInspList
    End Sub
    Private Sub NewRecord()
        Dim mModelMonitorInsp As ModelMonitorInsp
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(ID, New Guid(cmbModel.SelectedValue), 1, ID) 'HardFix HourType=1 as diff is only show purpose H OR HD  'For new records ID,PrevRefID are same
        Session("mModelMonitorInsp") = mModelMonitorInsp
        RemoveSession()
        Session("ModelIDForMPD") = New Guid(cmbModel.SelectedValue)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewMPD_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mModelMonitorInsp As ModelMonitorInsp

        mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mId, 1) 'HardFix HourType=1 as diff is only show purpose H OR HD
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
        Session("ModelIDForMPD") = New Guid(cmbModel.SelectedValue)
        Session("ModelName") = cmbModel.SelectedItem.ToString
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewMPD_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub DeleteRecordService(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteService")
        mModelMonitorServiceList.CurrentIndex = Index
        Session("mModelMonitorServiceListForNewMPD") = mModelMonitorServiceList
    End Sub
    Private Sub NewRecordService()
        Dim mModelMonitorService As ModelMonitorService
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        mModelMonitorService = ModelMonitorService.NewModelMonitorService(ID, New Guid(cmbModel.SelectedValue), 1, ID) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mModelMonitorService") = mModelMonitorService
        RemoveSession()
        Session("ModelIDForNewMPD") = New Guid(cmbModel.SelectedValue)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewMPDService_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub EditRecordService(ByVal mId As Guid)
        Dim mModelMonitorService As ModelMonitorService
        mModelMonitorService = ModelMonitorService.GetModelMonitorService(mId, 1) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mModelMonitorService") = mModelMonitorService
        Session("ModelIDForNewMPD") = mModelMonitorServiceList(mId).ModelID
        mServiceDetail = "Model : " & mModelMonitorService.Model.Name & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description
        MarkLog(Util.Action.Edit, "Model Insp", mServiceDetail, Util.ErrorType.NoError, mModelMonitorService.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewMPDService_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub ControlVisibility()
        If Not mModelMonitorInspList Is Nothing Then
            btnAddNewTop.Visible = (mModelMonitorInspList.Count > 15)
            btnBackTop.Visible = (mModelMonitorInspList.Count > 15)
            btnPrintTop.Visible = (mModelMonitorInspList.Count > 15)
            btnPrint.Enabled = IIf(mModelMonitorInspList.Count > 0, True, False)
            btnPrintTop.Enabled = IIf(mModelMonitorInspList.Count > 0, True, False)
        Else
            btnAddNewTop.Visible = False
            btnBackTop.Visible = False
            btnPrintTop.Visible = False
            btnPrint.Visible = False
        End If

        If Not mModelMonitorServiceList Is Nothing Then
            btnAddNewTopService.Visible = (mModelMonitorServiceList.Count > 15)
            btnBackTopService.Visible = (mModelMonitorServiceList.Count > 15)
            btnPrintTopService.Visible = (mModelMonitorServiceList.Count > 15)
            btnPrintService.Enabled = IIf(mModelMonitorServiceList.Count > 0, True, False)
            btnPrintTopService.Enabled = IIf(mModelMonitorServiceList.Count > 0, True, False)
        Else
            btnAddNewTopService.Visible = False
            btnBackTopService.Visible = False
            btnPrintTopService.Visible = False
            btnPrintService.Visible = False
        End If

        If Not mModelList Is Nothing Then
            btnAddNew.Enabled = (mModelList.Count > 0)
            btnAddNewTop.Enabled = (mModelList.Count > 0)
            btnAddNewService.Enabled = (mModelList.Count > 0)
            btnAddNewTopService.Enabled = (mModelList.Count > 0)
        Else
            btnAddNew.Enabled = False
            btnAddNewTop.Enabled = False
            btnAddNewService.Enabled = False
            btnAddNewTopService.Enabled = False

        End If


        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            spnService.Text = "Maintenance Event"
            lblTbInspServiceMPD.Text = "Maintenance Event"
            phInsp.Visible = False
            dgModelMonitorServiceList.Columns(1).HeaderText = "Task No."
            spMPDNo.InnerText = "Task No."
            tbpnlInsp.Visible = False
            upnlGridService.Update()
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim mId As Guid
        Dim msgCount As Integer = 0
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
                            ControlVisibility()
                            upnlGrid.Update()
                            upnlActionBtn.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "") 'Added by Vikrant on 28-July-2011
                                mModelName = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ModelName
                                mATA = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ATAChapter
                                mMonitorDesc = mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).Description
                                mInspectionDetail = "Model : " + mModelName + " ATA : " + mATA + " Description : " + mMonitorDesc
                                MarkLog(Util.Action.Delete, "Model Inspection", "Can't Delete:" & mInspectionDetail & " is already in use", Util.ErrorType.NoError, mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID, EventLogID)
                                'End
                                'Added by saylee on 1-Jun-2016
                                Dim mModelMonitorInspConfiguredList As ModelMonitorConfiguredList
                                mModelMonitorInspConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelList(SelectedModelIndex).ID, mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID.ToString)

                                If mModelMonitorInspConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mModelMonitorInspConfiguredList.Count - 1
                                        If i = mModelMonitorInspConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mModelMonitorInspConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mModelMonitorInspConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected MPD is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master MPD record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
                                MarkLog(Util.Action.Delete, "Model Inspection", "Can't Delete:" & mInspectionDetail & " is already in use", Util.ErrorType.NoError, mModelMonitorInspList.Item(mModelMonitorInspList.CurrentIndex).ID, EventLogID)
                                'End
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteService" Then
                        Try

                            Session("sender") = ""
                            mServiceDetail = "Service Type : " + mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).ModelMonitorServiceTypeName + " Description : " + mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).Description

                            Mid = mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).ID
                            If mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mModelMonitorServiceList(mModelMonitorServiceList.CurrentIndex).ID)
                            End If
                            ModelMonitorService.DeleteModelMonitorService(mModelMonitorServiceList.CurrentItem.id)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            FindNow()
                            ControlVisibility()
                            upnlGrid.Update()
                            upnlActionBtn.Update()
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                ' MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "") 'Added by Vikrant on 28-July-2011
                                MarkLog(Util.Action.Delete, "Model Service", "Can't Delete:" & mServiceDetail & " is already in use", Util.ErrorType.NoError, Mid, EventLogID)
                                'End
                                'Added by saylee on 1-Jun-2016
                                Dim mModelMonitorServiceConfiguredList As ModelMonitorConfiguredList
                                mModelMonitorServiceConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ModelID, mModelMonitorServiceList.Item(mModelMonitorServiceList.CurrentIndex).ID.ToString)

                                If mModelMonitorServiceConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mModelMonitorServiceConfiguredList.Count - 1
                                        If i = mModelMonitorServiceConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.Show("Deletion Alert!", "Selected MPD is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 26-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Model Service", mServiceDetail, Util.ErrorType.NoError, Mid, EventLogID)
                                'End
                            End If
                        End Try
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
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        Dim Reccount As Integer = 0
        If AppSettings("ClientCode") = "IAT" Then
            Reccount = mModelMonitorInspList.RecordCount
        Else
            Reccount = mModelMonitorInspList.Count
        End If

        lblResult.Text = "List Of MPD: " & Reccount & " Record(s)"
        lblResultService.Text = "List Of MPD: " & mModelMonitorServiceList.Count & " Record(s)"
    End Sub
    Private Sub SetGrid()
        'Dim P As Boolean
        'For j As Integer = 0 To dgModelMonitorInspList.Rows.Count - 1
        '    P = CType(Me.dgModelMonitorInspList.Rows(j).Cells(13).Text, Boolean)
        '    If P = False Then
        '        dgModelMonitorInspList.Rows(j).Cells(12).Enabled = False
        '    End If
        'Next

        'Dim Q As Boolean
        'For j As Integer = 0 To dgModelMonitorServiceList.Rows.Count - 1
        '    Q = CType(Me.dgModelMonitorServiceList.Rows(j).Cells(13).Text, Boolean)
        '    If Q = False Then
        '        dgModelMonitorServiceList.Rows(j).Cells(12).Enabled = False
        '    End If
        'Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        cmbAssemblyType.DataBind()

        mATAList = ATAList.GetATAList("", "(All)") 'Added By Saylee on 12-Aug-2010
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(All)")
        cmbMonitorType.DataSource = mModelMonitorInspTypeList
        cmbMonitorType.DataBind()
        Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList

        mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("(All)")
        cmbMonitorServiceType.DataSource = mModelMonitorServiceTypeList
        cmbMonitorServiceType.DataBind()
        Session("mModelMonitorServiceTypeListForNewMPD") = mModelMonitorServiceTypeList

        If mAssemblyTypeList.Count > 0 Then
            mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
            cmbModel.DataSource = mModelList
            cmbModel.DataBind()
            Session("mModelList") = mModelList

            setModelCombo()
            cmbAssemblyType.SelectedIndex = SelectedAssemblyTypeIndex
            cmbModel.SelectedIndex = SelectedModelIndex
        End If
        DataBind()
        cmbATAChapter.SelectedIndex = ATA
        cmbMonitorType.SelectedIndex = SelectedMonitorType
        cmbMonitorServiceType.SelectedIndex = SelectedMonitorServiceType
        txtDescription.Text = Description
    End Sub
    Private Sub setModelCombo()
        If mModelList.Count > 0 Then
            cmbModel.Enabled = True
            FindNow()
            ControlVisibility()
            SetPage()
        Else
            cmbModel.Enabled = False
            mModelMonitorInspList = Nothing
            Session("mModelMonitorInspList") = mModelMonitorInspList
            dgModelMonitorInspList.DataSource = mModelMonitorInspList
            dgModelMonitorInspList.DataBind()
            btnAddNewTop.Visible = False
            btnBackTop.Visible = False
            btnAddNew.Enabled = False
            lblResult.Text = "List Of MPD: 0 Record(s)"

            mModelMonitorServiceList = Nothing
            Session("mModelMonitorServiceListForNewpMPD") = mModelMonitorServiceList
            dgModelMonitorServiceList.DataSource = mModelMonitorServiceList
            dgModelMonitorServiceList.DataBind()
            btnAddNewTopService.Visible = False
            btnBackTopService.Visible = False
            btnAddNewService.Enabled = False
            lblResultService.Text = "List Of MPD: 0 Record(s)"
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        If Session("MiddleFrame") <> "wfNewMPDList_Ajax.aspx?" Then
            Session.Remove("MPDNo")
            Session.Remove("mCompMonitorInspStatusList")
            Session.Remove("mAssemblyMonitorInspStatusList")
            Session.Remove("IsTabIndexChaged")
            Session.Remove("NewMPDTabIndex")
            Session.Remove("SelectedModelIndex")
            Session.Remove("SelectedAssemblyTypeIndex")
            Session.Remove("SelectedMonitorType")
            Session.Remove("ATA")
            Session.Remove("ModelDescription")
        End If
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            TbContInst.ActiveTabIndex = IIf(CType(Session("NewMPDTabIndex"), Integer) > 0, CType(Session("NewMPDTabIndex"), Integer), 0)
            Session("MiddleFrame") = "wfNewMPDList_Ajax.aspx?"
            If TbContInst.ActiveTabIndex = 1 Then 'Comp Tab
                TbContInst_ActiveTabChanged(sender, e)
            Else
                DataFieldBind()
                ControlVisibility()
            End If
        End If
    End Sub
    Private Sub dgModelMonitorInspList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelMonitorInspList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgModelMonitorInspList.PageIndex * dgModelMonitorInspList.PageSize
                mID = New Guid(dgModelMonitorInspList.DataKeys(Index).Value.ToString)
                EditRecord(mID)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgModelMonitorInspList.PageIndex * dgModelMonitorInspList.PageSize
                DeleteRecord(Index)
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
                End If
        End Select
    End Sub
    Private Sub dgModelMonitorServiceList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelMonitorServiceList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgModelMonitorServiceList.PageIndex * dgModelMonitorServiceList.PageSize
                mID = New Guid(dgModelMonitorServiceList.DataKeys(Index).Value.ToString)
                EditRecordService(mID)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgModelMonitorServiceList.PageIndex * dgModelMonitorServiceList.PageSize
                DeleteRecordService(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgModelMonitorServiceList.PageIndex * dgModelMonitorServiceList.PageSize
                mID = New Guid(dgModelMonitorServiceList.DataKeys(Index).Value.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttachForNewMPD") = mFileAttach
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
                End If
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'If Not Guid.Empty.Equals(cmbModel.SelectedValue.ToString) Then
        Dim mMachineNameValueList As MachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, ModelID:=cmbModel.SelectedValue.ToString)
        If mMachineNameValueList.Count = 0 Then
            MSGBoxCtrl.show("Add Alert!!!", "Alert", "Aircraft not present for selected Model.<BR><BR>Please select different Model", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'End If
        MarkLog(Util.Action.[New], "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
    End Sub
    Private Sub btnAddNewService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewService.Click, btnAddNewTopService.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "Model Service", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecordService()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click, btnBackTopService.Click, btnBackService.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Session.Remove("SelectedModelIndex")
        Session.Remove("SelectedAssemblyTypeIndex")
        Session.Remove("SelectedMonitorType")
        Session.Remove("ATA")
        Session.Remove("ModelDescription")
        Session.Remove("NewMPDTabIndex")
        Session.Remove("SelectedMonitorServiceTypeForNewMPD")

        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgModelMonitorInspList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModelMonitorInspList.Sorting
        mModelMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorInspList") = mModelMonitorInspList
        dgModelMonitorInspList.DataSource = mModelMonitorInspList
        dgModelMonitorInspList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgModelMonitorServiceList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModelMonitorServiceList.Sorting
        mModelMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorServiceListForNewCompMPD") = mModelMonitorServiceList
        dgModelMonitorServiceList.DataSource = mModelMonitorServiceList
        dgModelMonitorServiceList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        SelectedModelIndex = 0
        Session("SelectedModelIndex") = SelectedModelIndex
        SelectedAssemblyTypeIndex = cmbAssemblyType.SelectedIndex
        Session("SelectedAssemblyTypeIndex") = SelectedAssemblyTypeIndex
        mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()
        Session("mModelList") = mModelList
        setModelCombo()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbModel_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbModel.SelectedIndexChanged
        SelectedModelIndex = cmbModel.SelectedIndex
        Session("SelectedModelIndex") = SelectedModelIndex
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub

    Private Sub cmbATAChapter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        ATA = cmbATAChapter.SelectedIndex
        Session("ATA") = ATA
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        SelectedMonitorType = cmbMonitorType.SelectedIndex
        Session("SelectedMonitorType") = SelectedMonitorType
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbMonitorServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorServiceType.SelectedIndexChanged
        SelectedMonitorServiceType = cmbMonitorServiceType.SelectedIndex
        Session("SelectedMonitorServiceTypeForNewMPD") = SelectedMonitorServiceType
        FindNow()
        ControlVisibility()
        upnlGridService.Update()
        upnlActionBtnService.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged, txtMPDNo.TextChanged
        Description = txtDescription.Text.Trim
        Session("ModelDescription") = Description

        MPDNo = txtMPDNo.Text.Trim
        Session("MPDNo") = MPDNo

        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub TbContInst_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbContInst.ActiveTabChanged
        NewMPDTabIndex = TbContInst.ActiveTabIndex
        Session("NewMPDTabIndex") = NewMPDTabIndex
        Select Case NewMPDTabIndex
            Case 0 'Assembly New MPD
                Session.Remove("SelectedMonitorTypeForNewCompMPD")
                Session.Remove("ATAForNewCompMPD")
                Session.Remove("DescriptionForNewCompMPD")
                Session.Remove("mPartID")
                Session.Remove("mPartMonitorInspListForNewCompMPD")
                Session.Remove("mATAListForNewCompMPD")
                Session.Remove("mPartMonitorInspTypeListForNewCompMPD")
                Session.Remove("mAssemblyTypeListForNewCompMPD")
                Session.Remove("mModelListForNewCompMPD")
                DataFieldBind()
                ControlVisibility()
            Case 1 'Comp New MPD
                Session.Remove("mAssemblyTypeList")
                Session.Remove("mModelList")
                Session.Remove("mModelMonitorInspList")
                Session.Remove("mATAList")
                Session.Remove("mModelMonitorInspTypeList")
                Session.Remove("SelectedModelIndex")
                Session.Remove("SelectedAssemblyTypeIndex")
                Session.Remove("SelectedMonitorType")
                Session.Remove("ATA")
                Session.Remove("ModelDescription")
                Session("IsTabIndexChaged") = True
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCompMPDList", "CallCompMPDList();", True)
        End Select
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        'For Issue List
        Dim Rpt As New crptAssemblyMPDList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMPD
        Dim mCompanyDetail As New CompanyDetail

        mModelMonitorInspList = Session("mModelMonitorInspList")

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Assembly MPD List", cmbAssemblyType.SelectedItem.ToString, cmbModel.SelectedItem.ToString, cmbMonitorType.SelectedItem.ToString, cmbATAChapter.SelectedItem.ToString, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim Reccount As Integer = 0
        If AppSettings("ClientCode") = "IAT" Then
            Reccount = mModelMonitorInspList.RecordCount
        Else
            Reccount = mModelMonitorInspList.Count
        End If
        If Reccount = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mModelMonitorInspList)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region



End Class