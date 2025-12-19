'Added by vikrant For MPD
Imports System.Collections.Generic
Imports Flypal.PartListAutoComplete
Imports System.Linq

Public Class wfNewCompMPDList_Ajax
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
    Public mPartMonitorInspList As PartMonitorInspList
    Public mModelMonitorInsp As ModelMonitorInsp
    Public mATAList As ATAList
    Public mPartMonitorInspTypeList As PartMonitorInspTypeList
    Dim EventLogID As Guid
    Public mInspectionDetail, mServiceDetail As String
    Public mATA As String
    Public mModelName As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim SelectedModelIndex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA, SelectedMonitorServiceType As Integer
    Dim Description As String = String.Empty
    Shared mPartID, mModelIDForNewCompMPD As Guid

    Public mPartMonitorServiceList As PartMonitorServiceList
    Public mPartMonitorServiceTypeList As PartMonitorServiceTypeList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyTypeList = CType(Session("mAssemblyTypeListForNewCompMPD"), AssemblyTypeList)
        mModelList = CType(Session("mModelListForNewCompMPD"), ModelList)
        mPartMonitorInspList = CType(Session("mPartMonitorInspListForNewCompMPD"), PartMonitorInspList)
        SelectedModelIndex = IIf(Session("SelectedModelIndexForNewCompMPD") Is Nothing, 0, Session("SelectedModelIndexForNewCompMPD"))
        mATAList = CType(Session("mATAListForNewCompMPD"), ATAList)
        mPartMonitorInspTypeList = CType(Session("mPartMonitorInspTypeListForNewCompMPD"), PartMonitorInspTypeList)
        SelectedMonitorType = IIf(Session("SelectedMonitorTypeForNewCompMPD") Is Nothing, 0, Session("SelectedMonitorTypeForNewCompMPD"))
        ATA = IIf(Session("ATAForNewCompMPD") Is Nothing, 0, Session("ATAForNewCompMPD"))
        Description = IIf(Session("DescriptionForNewCompMPD") Is Nothing, String.Empty, Session("DescriptionForNewCompMPD"))
        If PartID.Value <> "" Then
            mPartID = New Guid(PartID.Value)
        Else
            mPartID = Guid.Empty
        End If
        mModelIDForNewCompMPD = Session("mModelIDForNewCompMPD")

        mPartMonitorServiceList = CType(Session("mPartMonitorServiceListForNewCompMPD"), PartMonitorServiceList)
        mPartMonitorServiceTypeList = CType(Session("mPartMonitorServiceTypeListForNewCompMPD"), PartMonitorServiceTypeList)
        SelectedMonitorServiceType = IIf(Session("SelectedMonitorServiceTypeForNewCompMPD") Is Nothing, 0, Session("SelectedMonitorServiceTypeForNewCompMPD"))
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
    'Private Sub ClearAll()
    '    If InStr(Session("MiddleFrame"), "wfNewCompMPDList_Ajax.aspx?") <= 0 Then
    '        RemoveSession()
    '    End If
    'End Sub
    Private Sub FindNow()
        mPartMonitorInspList = PartMonitorInspList.GetPartMonitorInspList(mPartID, ModelID:=mModelList(SelectedModelIndex).ID, InspectionType:=mPartMonitorInspTypeList(SelectedMonitorType, "").ID, ATACode:=mATAList(ATA).ATACode, ATANomenclature:=String.Empty, Description:=Description)
        Session("mPartMonitorInspListForNewCompMPD") = mPartMonitorInspList
        dgPartMonitorInspList.DataSource = mPartMonitorInspList
        dgPartMonitorInspList.DataBind()
        lblResult.Text = "List Of MPD: " & mPartMonitorInspList.Count & " Record(s)"

        mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(mPartID, ModelID:=mModelList(SelectedModelIndex).ID, ServiceType:=mPartMonitorServiceTypeList(SelectedMonitorServiceType, "").ID, ATACode:=mATAList(ATA).ATACode, ATANomenclature:=String.Empty, Description:=Description)
        Session("mPartMonitorServiceListForNewCompMPD") = mPartMonitorServiceList
        dgPartMonitorServiceList.DataSource = mPartMonitorServiceList
        dgPartMonitorServiceList.DataBind()
        lblResultService.Text = "List Of MPD: " & mPartMonitorServiceList.Count & " Record(s)"

        upnlGridService.Update()
        upnlTabs.Update()
        SetGrid()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartMonitorInspListForNewCompMPD")
        Session.Remove("mATAListForNewCompMPD")
        Session.Remove("mPartMonitorInspTypeListForNewCompMPD")
        Session.Remove("mAssemblyTypeListForNewCompMPD")
        Session.Remove("mModelListForNewCompMPD")
        Session.Remove("mPartMonitorServiceListForNewCompMPD")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartMonitorInspList.CurrentIndex = Index
        Session("mPartMonitorInspListForNewCompMPD") = mPartMonitorInspList
    End Sub
    Private Sub NewRecord()
        Dim mPartMonitorInsp As PartMonitorInsp
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        mPartMonitorInsp = PartMonitorInsp.NewPartMonitorInsp(ID, mPartID, New Guid(cmbModel.SelectedValue), 1, ID) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mPartMonitorInsp") = mPartMonitorInsp
        RemoveSession()
        Session("ModelIDForNewCompMPD") = New Guid(cmbModel.SelectedValue)
        Session("PartIDForNewCompMPD") = mPartID
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompMPD_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub

    Private Sub EditRecord(ByVal mId As Guid)
        Dim mPartMonitorInsp As PartMonitorInsp
        mPartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(mId, 1) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mPartMonitorInsp") = mPartMonitorInsp
        Session("PartIDForNewCompMPD") = mPartMonitorInspList(mId).PartID
        mInspectionDetail = "Part : " & mPartMonitorInsp.Part.Name & " Part Inspection Type : " & mPartMonitorInsp.PartMonitorInspTypeName & " Description : " & mPartMonitorInsp.Description
        MarkLog(Util.Action.Edit, "Part Insp", mInspectionDetail, Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompMPD_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub

    Private Sub DeleteRecordService(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteService")
        mPartMonitorServiceList.CurrentIndex = Index
        Session("mPartMonitorServiceListForNewCompMPD") = mPartMonitorServiceList
    End Sub
    Private Sub NewRecordService()
        Dim mPartMonitorService As PartMonitorService
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        mPartMonitorService = PartMonitorService.NewPartMonitorService(ID, mPartID, New Guid(cmbModel.SelectedValue), 1, ID) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mPartMonitorService") = mPartMonitorService
        RemoveSession()
        Session("ModelIDForNewCompMPD") = New Guid(cmbModel.SelectedValue)
        Session("PartIDForNewCompMPD") = mPartID
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompMPDService_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub EditRecordService(ByVal mId As Guid)
        Dim mPartMonitorService As PartMonitorService
        mPartMonitorService = PartMonitorService.GetPartMonitorService(mId, 1) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mPartMonitorService") = mPartMonitorService
        Session("PartIDForNewCompMPD") = mPartMonitorServiceList(mId).PartID
        mServiceDetail = "Part : " & mPartMonitorService.Part.Name & " Part Service Type : " & mPartMonitorService.PartMonitorServiceTypeName & " Description : " & mPartMonitorService.Description
        MarkLog(Util.Action.Edit, "Part Insp", mServiceDetail, Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompMPDService_Ajax.aspx?BackPage=wfNewMPDList_Ajax.aspx');", True)
    End Sub
    Private Sub ControlVisibility()
        If Not mPartMonitorInspList Is Nothing Then
            btnAddNewTop.Visible = (mPartMonitorInspList.Count > 15)
            btnBackTop.Visible = (mPartMonitorInspList.Count > 15)
            btnPrintTop.Visible = (mPartMonitorInspList.Count > 15)
            btnPrint.Enabled = IIf(mPartMonitorInspList.Count > 0, True, False)
            btnPrintTop.Enabled = IIf(mPartMonitorInspList.Count > 0, True, False)
        Else
            btnAddNewTop.Visible = False
            btnBackTop.Visible = False
            btnPrintTop.Visible = False
            btnPrint.Visible = False
        End If

        If Not mPartMonitorServiceList Is Nothing Then
            btnAddNewTopService.Visible = (mPartMonitorServiceList.Count > 15)
            btnBackTopService.Visible = (mPartMonitorServiceList.Count > 15)
            btnPrintTopService.Visible = (mPartMonitorServiceList.Count > 15)
            btnPrintService.Enabled = IIf(mPartMonitorServiceList.Count > 0, True, False)
            btnPrintTopService.Enabled = IIf(mPartMonitorServiceList.Count > 0, True, False)
        Else
            btnAddNewTopService.Visible = False
            btnBackTopService.Visible = False
            btnPrintTopService.Visible = False
            btnPrintService.Visible = False
        End If

        btnAddNew.Enabled = Not mPartID.Equals(Guid.Empty)
        btnAddNewTop.Enabled = Not mPartID.Equals(Guid.Empty)
        btnAddNewService.Enabled = Not mPartID.Equals(Guid.Empty)
        btnAddNewTopService.Enabled = Not mPartID.Equals(Guid.Empty)

        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            spnService.Text = "Maintenance Event"
            lblTbCompInspServiceMPD.Text = "Maintenance Event"
            phInsp.Visible = False
            tbpnlInsp.Visible = False
            dgPartMonitorServiceList.Columns(2).HeaderText = "Task No."
            ' spMPDNo.InnerText = "Task No."
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
                            mInspectionDetail = "Insp Type : " + mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).PartMonitorInspTypeName + " Description : " + mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).Description

                            mId = mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).ID
                            If mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).ID)
                            End If
                            PartMonitorInsp.DeletePartMonitorInsp(mPartMonitorInspList.CurrentItem.id)
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
                                MarkLog(Util.Action.Delete, "Part Inspection", "Can't Delete:" & mInspectionDetail & " is already in use", Util.ErrorType.NoError, mId, EventLogID)
                                'End
                                'Added by saylee on 1-Jun-2016
                                Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
                                mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInspList.Item(mPartMonitorInspList.CurrentIndex).PartID, mPartMonitorInspList.Item(mPartMonitorInspList.CurrentIndex).ID.ToString)

                                If mPartMonitorInspConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mPartMonitorInspConfiguredList.Count - 1
                                        If i = mPartMonitorInspConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected MPD is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 26-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Part Inspection", mInspectionDetail, Util.ErrorType.NoError, mId, EventLogID)
                                'End
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteService" Then
                        Try
                            Session("sender") = ""
                            mServiceDetail = "Service Type : " + mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).PartMonitorServiceTypeName + " Description : " + mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).Description

                            mId = mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).ID
                            If mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).ID)
                            End If
                            PartMonitorService.DeletePartMonitorService(mPartMonitorServiceList.CurrentItem.id)
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
                                MarkLog(Util.Action.Delete, "Part Service", "Can't Delete:" & mServiceDetail & " is already in use", Util.ErrorType.NoError, mId, EventLogID)
                                'End
                                'Added by saylee on 1-Jun-2016
                                Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
                                mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).PartID, mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).ID.ToString)

                                If mPartMonitorServiceConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mPartMonitorServiceConfiguredList.Count - 1
                                        If i = mPartMonitorServiceConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected MPD is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 26-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Part Service", mServiceDetail, Util.ErrorType.NoError, mId, EventLogID)
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
        lblResult.Text = "List Of MPD: " & mPartMonitorInspList.Count & " Record(s)"
        lblResultService.Text = "List Of MPD: " & mPartMonitorServiceList.Count & " Record(s)"
    End Sub
    Private Sub SetGrid()
        'Dim P As Boolean
        'For j As Integer = 0 To dgPartMonitorInspList.Rows.Count - 1
        '    P = CType(Me.dgPartMonitorInspList.Rows(j).Cells(13).Text, Boolean)
        '    If P = False Then
        '        dgPartMonitorInspList.Rows(j).Cells(12).Enabled = False
        '    End If
        'Next

        'Dim Q As Boolean
        'For j As Integer = 0 To dgPartMonitorServiceList.Rows.Count - 1
        '    Q = CType(Me.dgPartMonitorServiceList.Rows(j).Cells(13).Text, Boolean)
        '    If Q = False Then
        '        dgPartMonitorServiceList.Rows(j).Cells(12).Enabled = False
        '    End If
        'Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAListForNewCompMPD") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeListForNewCompMPD") = mAssemblyTypeList
        cmbAssemblyType.DataBind()

        mPartMonitorInspTypeList = PartMonitorInspTypeList.GetPartMonitorInspTypeList("(All)")
        cmbMonitorType.DataSource = mPartMonitorInspTypeList
        cmbMonitorType.DataBind()
        Session("mPartMonitorInspTypeListForNewCompMPD") = mPartMonitorInspTypeList

        mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList("(All)")
        cmbMonitorServiceType.DataSource = mPartMonitorServiceTypeList
        cmbMonitorServiceType.DataBind()
        Session("mPartMonitorServiceTypeListForNewCompMPD") = mPartMonitorServiceTypeList

        If mAssemblyTypeList.Count > 0 Then
            mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
            cmbModel.DataSource = mModelList
            cmbModel.DataBind()
            Session("mModelListForNewCompMPD") = mModelList

            setModelCombo()
            cmbAssemblyType.SelectedIndex = SelectedAssemblyTypeIndex
            cmbModel.SelectedIndex = SelectedModelIndex

        End If
        DataBind()
        cmbModel.SelectedIndex = SelectedModelIndex
        cmbATAChapter.SelectedIndex = ATA
        cmbMonitorType.SelectedIndex = SelectedMonitorType
        cmbMonitorServiceType.SelectedIndex = SelectedMonitorServiceType
        txtDescription.Text = Description
    End Sub
    Private Sub setModelCombo()
        If mModelList.Count > 0 Then
            txtPartDescription.Enabled = True
            txtPartDescription.BackColor = Color.White
            cmbModel.Enabled = True
            mModelIDForNewCompMPD = New Guid(cmbModel.SelectedValue)
            Session("mModelIDForNewCompMPD") = mModelIDForNewCompMPD
            FindNow()
            ControlVisibility()
            SetPage()
        Else
            cmbModel.Enabled = False
            txtPartDescription.Enabled = False
            txtPartDescription.BackColor = Color.Gainsboro
            mPartMonitorInspList = Nothing
            Session("mPartMonitorInspListForNewCompMPD") = mPartMonitorInspList
            dgPartMonitorInspList.DataSource = mPartMonitorInspList
            dgPartMonitorInspList.DataBind()
            btnAddNewTop.Visible = False
            btnBackTop.Visible = False
            btnAddNew.Enabled = False
            lblResult.Text = "List Of MPD: 0 Record(s)"

            mPartMonitorServiceList = Nothing
            Session("mPartMonitorServiceListForNewCompMPD") = mPartMonitorServiceList
            dgPartMonitorServiceList.DataSource = mPartMonitorServiceList
            dgPartMonitorServiceList.DataBind()
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
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Or Session("IsTabIndexChaged") = True Then
            'Session("MiddleFrame") = "wfNewCompMPDList_Ajax.aspx"
            Session.Remove("IsTabIndexChaged")
            DataFieldBind()
            ControlVisibility()

        End If

    End Sub
    Private Sub dgPartMonitorInspList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartMonitorInspList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgPartMonitorInspList.PageIndex * dgPartMonitorInspList.PageSize
                mID = New Guid(dgPartMonitorInspList.DataKeys(Index).Value.ToString)
                EditRecord(mID)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgPartMonitorInspList.PageIndex * dgPartMonitorInspList.PageSize
                DeleteRecord(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgPartMonitorInspList.PageIndex * dgPartMonitorInspList.PageSize
                mID = New Guid(dgPartMonitorInspList.DataKeys(Index).Value.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttachForNewCompMPD") = mFileAttach
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
    Private Sub dgPartMonitorServiceList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartMonitorServiceList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgPartMonitorServiceList.PageIndex * dgPartMonitorServiceList.PageSize
                mID = New Guid(dgPartMonitorServiceList.DataKeys(Index).Value.ToString)
                EditRecordService(mID)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgPartMonitorServiceList.PageIndex * dgPartMonitorServiceList.PageSize
                DeleteRecordService(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgPartMonitorServiceList.PageIndex * dgPartMonitorServiceList.PageSize
                mID = New Guid(dgPartMonitorServiceList.DataKeys(Index).Value.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttachForNewCompMPD") = mFileAttach
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
        MarkLog(Util.Action.[New], "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
    End Sub
    Private Sub btnAddNewService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewService.Click, btnAddNewTopService.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "Part Service", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecordService()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click, btnBackService.Click, btnBackTopService.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Session.Remove("SelectedMonitorTypeForNewCompMPD")
        Session.Remove("ATAForNewCompMPD")
        Session.Remove("DescriptionForNewCompMPD")
        Session.Remove("mPartID")
        Session.Remove("NewMPDTabIndex")
        Session.Remove("SelectedMonitorServiceTypeForNewCompMPD")
        'Response.Redirect("index.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgPartMonitorInspList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartMonitorInspList.Sorting
        mPartMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorInspListForNewCompMPD") = mPartMonitorInspList
        dgPartMonitorInspList.DataSource = mPartMonitorInspList
        dgPartMonitorInspList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgPartMonitorServiceList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartMonitorServiceList.Sorting
        mPartMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorServiceListForNewCompMPD") = mPartMonitorServiceList
        dgPartMonitorServiceList.DataSource = mPartMonitorServiceList
        dgPartMonitorServiceList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        ATA = cmbATAChapter.SelectedIndex
        Session("ATAForNewCompMPD") = ATA
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        SelectedMonitorType = cmbMonitorType.SelectedIndex
        Session("SelectedMonitorTypeForNewCompMPD") = SelectedMonitorType
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbMonitorServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorServiceType.SelectedIndexChanged
        SelectedMonitorServiceType = cmbMonitorServiceType.SelectedIndex
        Session("SelectedMonitorServiceTypeForNewCompMPD") = SelectedMonitorServiceType
        FindNow()
        ControlVisibility()
        upnlGridService.Update()
        upnlActionBtnService.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged
        Description = txtDescription.Text.Trim
        Session("DescriptionForNewCompMPD") = Description
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub txtPartDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtPartDescription.TextChanged
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlGridService.Update()
        upnlActionBtnTopService.Update()
        upnlTabs.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbModel_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbModel.SelectedIndexChanged
        mModelIDForNewCompMPD = New Guid(cmbModel.SelectedValue)
        SelectedModelIndex = cmbModel.SelectedIndex
        txtPartDescription.Text = ""
        PartID.Value = ""
        mPartID = Guid.Empty
        Session("mModelIDForNewCompMPD") = mModelIDForNewCompMPD
        Session("SelectedModelIndexForNewCompMPD") = SelectedModelIndex
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        SelectedModelIndex = 0
        Session("SelectedModelIndexForNewCompMPD") = SelectedModelIndex
        SelectedAssemblyTypeIndex = cmbAssemblyType.SelectedIndex
        Session("SelectedAssemblyTypeIndexForNewCompMPD") = SelectedAssemblyTypeIndex
        txtPartDescription.Text = ""
        PartID.Value = ""
        mPartID = Guid.Empty
        mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()

        Session("mModelListForNewCompMPD") = mModelList
        setModelCombo()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub TbInspServiceMPD_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TbInspServiceMPD.ActiveTabChanged
        'Select Case TbInspServiceMPD.ActiveTabIndex
        '    Case 0
        '    Case 1

        'End Select
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        'For Issue List
        Dim Rpt As New crptCompMPDList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMPD
        Dim mCompanyDetail As New CompanyDetail

        mPartMonitorInspList = Session("mPartMonitorInspListForNewCompMPD")

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Part MPD List", cmbAssemblyType.SelectedItem.ToString, cmbModel.SelectedItem.ToString, cmbMonitorType.SelectedItem.ToString, cmbATAChapter.SelectedItem.ToString, txtPartDescription.Text.Trim, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mPartMonitorInspList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mPartMonitorInspList)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnPrintService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintService.Click, btnPrintTopService.Click
        'For Issue List
        Dim Rpt As New crptCompMPDList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMPD
        Dim mCompanyDetail As New CompanyDetail

        mPartMonitorServiceList = Session("mPartMonitorServiceListForNewCompMPD")

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Part MPD List", cmbAssemblyType.SelectedItem.ToString, cmbModel.SelectedItem.ToString, cmbMonitorType.SelectedItem.ToString, cmbATAChapter.SelectedItem.ToString, txtPartDescription.Text.Trim, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mPartMonitorServiceList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, "PartMonitorInspList", mPartMonitorServiceList)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub imgbtPart_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtPart.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPartWindow", "OpenPartWindow()", True)
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim partlist As PartListAutoComplete
        'partlist = PartListAutoComplete.GetPartList(prefixText, mModelIDForNewCompMPD.ToString)
        partlist = PartListAutoComplete.GetPartList(prefixText)
        If count = 0 Then
            Return (From c As PartListAutoCompleteInfo In partlist
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Part, c.ID.ToString())).ToArray
        Else
            Return (From c As PartListAutoCompleteInfo In partlist
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Part, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

  
  
End Class