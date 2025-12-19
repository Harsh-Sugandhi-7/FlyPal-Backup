'=============================================
' Created By : Saylee
' Create date: 3-Jun-2024
'=============================================  
Imports System.Linq
Imports System.Text
'Added
Public Class wfMPDMasterList
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
    'Public mAssemblyTypeList As AssemblyTypeList
    Public mPrimaryModelList As PrimaryModelList
    Public mMPDMasterList As MPDMasterList
    Public mMPDMaster As MPDMaster
    Public mATAList As ATAList
    Public mServiceTypeList As ServiceTypeList
    Dim EventLogID As Guid
    Public mDetail As String
    Public mATA As String
    Public mModelName As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim SelectedModelIndex, SelectedMonitorType, ATA As Integer
    Dim Description As String = String.Empty
    Dim NewMPDTabIndex As Integer
    Dim MPDTaskNumber As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        ' mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mPrimaryModelList = CType(Session("mPrimaryModelList"), PrimaryModelList)
        mMPDMasterList = CType(Session("mMPDMasterList"), MPDMasterList)
        SelectedModelIndex = IIf(Session("SelectedModelIndex") Is Nothing, 0, Session("SelectedModelIndex"))
        ' SelectedAssemblyTypeIndex = IIf(Session("SelectedAssemblyTypeIndex") Is Nothing, 0, Session("SelectedAssemblyTypeIndex"))
        mATAList = CType(Session("mATAList"), ATAList)
        mServiceTypeList = CType(Session("mServiceTypeList"), ServiceTypeList)
        SelectedMonitorType = IIf(Session("SelectedMonitorType") Is Nothing, 0, Session("SelectedMonitorType"))
        ATA = IIf(Session("ATA") Is Nothing, 0, Session("ATA"))
        Description = IIf(Session("ModelDescription") Is Nothing, String.Empty, Session("ModelDescription"))
        MPDTaskNumber = IIf(Session("MPDTaskNumber") Is Nothing, String.Empty, Session("MPDTaskNumber"))

    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "NewMPDMaster"


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
        If InStr(Session("MiddleFrame"), "wfMPDMasterList.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub FindNow()
        mMPDMasterList = MPDMasterList.GetMPDMasterList(PrimaryModelID:=mPrimaryModelList(SelectedModelIndex).ID, ServiceTypeID:=mServiceTypeList(SelectedMonitorType).ID, ATACode:=mATAList(ATA).ATACode, ATANomenclature:=String.Empty, Description:=Description, MPDTaskNumber:=MPDTaskNumber)
        Session("mMPDMasterList") = mMPDMasterList
        dgMPDMasterList.DataSource = mMPDMasterList
        dgMPDMasterList.DataBind()

        lblResult.Text = "List Of MPD: " & mMPDMasterList.Count.ToString & " Record(s)"
        SetGrid()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelList")
        Session.Remove("mMPDMasterList")
        Session.Remove("mATAList")
        Session.Remove("mServiceTypeList")

    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mMPDMasterList.CurrentIndex = Index
        Session("mMPDMasterList") = mMPDMasterList
        mMPDMaster = MPDMaster.GetMPDMaster(mMPDMasterList(Index).ID)
        Session("mMPDMaster") = mMPDMaster
    End Sub
    Private Sub NewRecord()
        Session("ModelIDForMPD") = New Guid(cmbModel.SelectedValue)
        mMPDMaster = MPDMaster.NewMPDMaster(Guid.NewGuid)
        Session("mMPDMaster") = mMPDMaster
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMPDMaster.aspx?BackPage=wfMPDMasterList.aspx');", True)

    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mMPDMaster As MPDMaster

        mMPDMaster = MPDMaster.GetMPDMaster(mId)
        Session("mMPDMaster") = mMPDMaster

        mModelName = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).PrimaryModelName
        mATA = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ATAChapter
        mDetail = "MPD Task number : " & mMPDMaster.MPDTaskNumber & " Primary Model : " & mModelName & " Type : " & mMPDMaster.ServiceTypeName & " Description : " & mMPDMaster.Description
        MarkLog(Util.Action.Edit, "NewMPDMaster", mDetail, Util.ErrorType.NoError, mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ID, EventLogID)
        'End
        RemoveSession()
        Session("ModelIDForMPD") = New Guid(cmbModel.SelectedValue)
        Session("ModelName") = cmbModel.SelectedItem.ToString
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMPDMaster.aspx?BackPage=wfMPDMasterList.aspx');", True)
    End Sub
    Private Sub ControlVisibility()
        'If Not mMPDMasterList Is Nothing Then
        '    btnAddNewTop.Visible = (mMPDMasterList.Count > 15)
        '    btnBackTop.Visible = (mMPDMasterList.Count > 15)
        '    btnPrintTop.Visible = (mMPDMasterList.Count > 15)
        '    btnPrint.Enabled = IIf(mMPDMasterList.Count > 0, True, False)
        '    btnPrintTop.Enabled = IIf(mMPDMasterList.Count > 0, True, False)
        'Else
        '    btnAddNewTop.Visible = False
        '    btnBackTop.Visible = False
        '    btnPrintTop.Visible = False
        '    btnPrint.Visible = False
        'End If

        If Not mPrimaryModelList Is Nothing Then
            btnAddNew.Enabled = (mPrimaryModelList.Count > 0)
            '   btnAddNewTop.Enabled = (mPrimaryModelList.Count > 0)
        Else
            btnAddNew.Enabled = False
            ' btnAddNewTop.Enabled = False
        End If
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
                            mMPDMaster = CType(Session("mMPDMaster"), MPDMaster)
                            MPDMaster.DeleteMPDMaster(mMPDMaster.ID)
                            mModelName = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).PrimaryModelName
                            mATA = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ATAChapter
                            mMonitorDesc = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).Description
                            mDetail = "Model : " + mModelName + " ATA : " + mATA + " Description : " + mMonitorDesc
                            MarkLog(Util.Action.Delete, "NewMPDMaster", mDetail, Util.ErrorType.NoError, mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ID, EventLogID)
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
                                mModelName = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).PrimaryModelName
                                mATA = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ATAChapter
                                mMonitorDesc = mMPDMasterList.Item(mMPDMasterList.CurrentIndex).Description
                                mDetail = "Model : " + mModelName + " ATA : " + mATA + " Description : " + mMonitorDesc
                                MarkLog(Util.Action.Delete, "NewMPDMaster", "Can't Delete:" & mDetail & " is already in use", Util.ErrorType.NoError, mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ID, EventLogID)
                                'End
                                'Added by saylee on 1-Jun-2016
                                'Dim mModelMonitorInspConfiguredList As ModelMonitorConfiguredList
                                'mModelMonitorInspConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mPrimaryModelList(SelectedModelIndex).ID, mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ID.ToString)
                                'Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
                                'mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mPrimaryModelList(SelectedModelIndex).ID, mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ID.ToString)

                                'If mModelMonitorConfiguredList.Count > 0 Then
                                '    Dim SerialNos As String = String.Empty

                                '    For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                                '        If i = mModelMonitorConfiguredList.Count - 1 Then
                                '            SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                                '        Else
                                '            SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                                '        End If
                                '    Next

                                '    MSGBoxCtrl.Show("Deletion Alert!", "Selected MPD is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master MPD record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                'End If
                                MSGBoxCtrl.Show("Deletion Alert!", "Selected MPD is already configured on Assembly. So cannot be deleted", "To delete master MPD record please delete all configured status first", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "NewMPDMaster", "Can't Delete:" & mDetail & " is already in use", Util.ErrorType.NoError, mMPDMasterList.Item(mMPDMasterList.CurrentIndex).ID, EventLogID)
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
        lblResult.Text = "List Of MPD: " & mMPDMasterList.Count & " Record(s)"
    End Sub
    Private Sub SetGrid()
        'Dim P As Boolean
        'For j As Integer = 0 To dgMPDMasterList.Rows.Count - 1
        '    P = CType(Me.dgMPDMasterList.Rows(j).Cells(12).Text, Boolean)
        '    If P = False Then
        '        dgMPDMasterList.Rows(j).Cells(11).Enabled = False
        '    End If
        'Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        ''mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
        ''cmbAssemblyType.DataSource = mAssemblyTypeList
        ''Session("mAssemblyTypeList") = mAssemblyTypeList
        ''cmbAssemblyType.DataBind()

        mATAList = ATAList.GetATAList("", "(ALL)") 'Added By Saylee on 12-Aug-2010
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        mServiceTypeList = ServiceTypeList.GetServiceTypeList(True, SelectTagText:="(ALL)")
        cmbMonitorType.DataSource = mServiceTypeList
        cmbMonitorType.DataBind()
        Session("mServiceTypeList") = mServiceTypeList

        mPrimaryModelList = PrimaryModelList.GetPrimaryModelList(AddTopItem:="(ALL)")
        cmbModel.DataSource = mPrimaryModelList
        Session("mPrimaryModelList") = mPrimaryModelList
        cmbModel.DataBind()

        cmbATAChapter.SelectedIndex = ATA
        cmbMonitorType.SelectedIndex = SelectedMonitorType
        txtDescription.Text = Description
        cmbModel.SelectedIndex = SelectedModelIndex
        txtMPDNo.Text = MPDTaskNumber
    End Sub
    Private Sub setModelCombo()
        If mPrimaryModelList.Count > 0 Then
            cmbModel.Enabled = True
            FindNow()
            ControlVisibility()
            SetPage()
        Else
            cmbModel.Enabled = False
            mMPDMasterList = Nothing
            Session("mMPDMasterList") = mMPDMasterList
            dgMPDMasterList.DataSource = mMPDMasterList
            dgMPDMasterList.DataBind()
            '  btnAddNewTop.Visible = False
            '  btnBackTop.Visible = False
            btnAddNew.Enabled = False
            lblResult.Text = "List Of MPD: 0 Record(s)"
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        If Session("MiddleFrame") <> "wfMPDMasterList.aspx?" Then
            Session.Remove("MPDTaskNumber")

            Session.Remove("IsTabIndexChaged")
            Session.Remove("NewMPDTabIndex")
            Session.Remove("SelectedModelIndex")
            Session.Remove("ATA")
            Session.Remove("ModelDescription")
            Session.Remove("mPrimaryModelList")
        End If
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            TbContInst.ActiveTabIndex = IIf(CType(Session("NewMPDTabIndex"), Integer) > 0, CType(Session("NewMPDTabIndex"), Integer), 0)
            Session("MiddleFrame") = "wfMPDMasterList.aspx?"
            If TbContInst.ActiveTabIndex = 1 Then 'Comp Tab
                TbContInst_ActiveTabChanged(sender, e)
            Else
                DataFieldBind()
                FindNow()
                ControlVisibility()
            End If
        End If
    End Sub
    Private Sub dgMPDMasterList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMPDMasterList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgMPDMasterList.PageIndex * dgMPDMasterList.PageSize
                '  mID = New Guid(dgMPDMasterList.DataKeys(Index).Value.ToString)
                mID = mMPDMasterList(Index).ID

                EditRecord(mID)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgMPDMasterList.PageIndex * dgMPDMasterList.PageSize
                DeleteRecord(Index)
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgMPDMasterList.PageIndex * dgMPDMasterList.PageSize
                ' mID = New Guid(dgMPDMasterList.DataKeys(Index).Value.ToString)
                mID = mMPDMasterList(Index).ID
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
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click ', btnAddNewTop.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        ''''''''If Not Guid.Empty.Equals(cmbModel.SelectedValue.ToString) Then
        '''''''Dim mMachineNameValueList As MachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, ModelID:=cmbModel.SelectedValue.ToString)
        '''''''If mMachineNameValueList.Count = 0 Then
        '''''''    MSGBoxCtrl.Show("Add Alert!!!", "Alert", "Aircraft not present for selected Model.<BR><BR>Please select different Model", MsgBoxStyle.OkOnly, "")
        '''''''    Exit Sub
        '''''''End If
        ''''''''End If

        If Not Guid.Empty.Equals(cmbModel.SelectedValue.ToString) Then
            Dim mModelList As ModelList = ModelList.GetModelList(PrimaryModelID:=cmbModel.SelectedValue.ToString)
            Dim ChkModelIDs As String()
            ChkModelIDs = (From c As ModelList.ModelInfo In mModelList
                           Select (c.ID.ToString)).ToArray

            Dim ModelIDs As New StringBuilder
            If ChkModelIDs.Length > 0 Then
                ModelIDs.Append("<ModelID>")
                For i As Integer = 0 To ChkModelIDs.Count - 1
                    ModelIDs.Append("<id>")
                    ModelIDs.Append(ChkModelIDs(i))
                    ModelIDs.Append("</id>")
                Next
                ModelIDs.Append("</ModelID>")
            End If

            Dim mMachineNameValueList As MachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, ModelIDStr:=ModelIDs.ToString)
            If mMachineNameValueList.Count = 0 Then
                MSGBoxCtrl.Show("Add Alert!!!", "Alert", "Aircraft not present for selected Model.<BR><BR>Please select different Model", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If

        MarkLog(Util.Action.[New], "NewMPDMaster", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click ', btnBackTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Session.Remove("SelectedModelIndex")
        Session.Remove("SelectedAssemblyTypeIndex")
        Session.Remove("SelectedMonitorType")
        Session.Remove("ATA")
        Session.Remove("ModelDescription")
        Session.Remove("NewMPDTabIndex")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgMPDMasterList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgMPDMasterList.PageIndexChanging
        dgMPDMasterList.PageIndex = e.NewPageIndex
        dgMPDMasterList.DataSource = mMPDMasterList
        dgMPDMasterList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgMPDMasterList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMPDMasterList.Sorting
        mMPDMasterList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMPDMasterList") = mMPDMasterList
        dgMPDMasterList.DataSource = mMPDMasterList
        dgMPDMasterList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
    '    SelectedModelIndex = 0
    '    Session("SelectedModelIndex") = SelectedModelIndex
    '    SelectedAssemblyTypeIndex = cmbAssemblyType.SelectedIndex
    '    Session("SelectedAssemblyTypeIndex") = SelectedAssemblyTypeIndex
    '    mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
    '    cmbModel.DataSource = mModelList
    '    cmbModel.DataBind()
    '    Session("mModelList") = mModelList
    '    setModelCombo()
    '    upnlGrid.Update()
    '    upnlActionBtn.Update()
    '    upnlActionBtnTop.Update()
    'End Sub
    Private Sub cmbModel_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbModel.SelectedIndexChanged
        SelectedModelIndex = cmbModel.SelectedIndex
        Session("SelectedModelIndex") = SelectedModelIndex
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        ATA = cmbATAChapter.SelectedIndex
        Session("ATA") = ATA
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        SelectedMonitorType = cmbMonitorType.SelectedIndex
        Session("SelectedMonitorType") = SelectedMonitorType
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged, txtMPDNo.TextChanged
        Description = txtDescription.Text.Trim
        Session("ModelDescription") = Description

        MPDTaskNumber = txtMPDNo.Text.Trim
        Session("MPDTaskNumber") = MPDTaskNumber

        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
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
                Session.Remove("mMPDMasterList")
                Session.Remove("mATAList")
                Session.Remove("mServiceTypeList")
                Session.Remove("SelectedModelIndex")
                Session.Remove("SelectedAssemblyTypeIndex")
                Session.Remove("SelectedMonitorType")
                Session.Remove("ATA")
                Session.Remove("ModelDescription")
                Session("IsTabIndexChaged") = True
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCompMPDList", "CallCompMPDList();", True)
        End Select
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click ', btnPrintTop.Click
        'For Issue List
        Dim Rpt As New crptAssemblyMPDList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMPD
        Dim mCompanyDetail As New CompanyDetail

        mMPDMasterList = Session("mMPDMasterList")

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Assembly MPD List", "", cmbModel.SelectedItem.ToString, cmbMonitorType.SelectedItem.ToString, cmbATAChapter.SelectedItem.ToString, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim Reccount As Integer = 0

        Reccount = mMPDMasterList.Count

        If Reccount = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mMPDMasterList)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub


#End Region


End Class