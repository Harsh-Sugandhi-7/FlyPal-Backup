'Added by vikrant For AD/SB

Public Class wfNewADSBList_Ajax
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
    Public mModelMonitorModList As ModelMonitorModList
    Public mATAList As ATAList
    Public mModelMonitorModTypeList As ModelMonitorModTypeList
    Dim EventLogID As Guid
    Public mDirectiveDetail As String
    Public mATA As String
    Public mModelName As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim SelectedModelIndex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA As Integer
    Dim Description As String = String.Empty
    Dim NewADSBTabIndex As Integer
    Dim ModNo As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelList = CType(Session("mModelList"), ModelList)
        mModelMonitorModList = CType(Session("mModelMonitorModList"), ModelMonitorModList)
        SelectedModelIndex = IIf(Session("SelectedModelIndex") Is Nothing, 0, Session("SelectedModelIndex"))
        SelectedAssemblyTypeIndex = IIf(Session("SelectedAssemblyTypeIndex") Is Nothing, 0, Session("SelectedAssemblyTypeIndex"))
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorModTypeList = CType(Session("mModelMonitorModTypeList"), ModelMonitorModTypeList)
        SelectedMonitorType = IIf(Session("SelectedMonitorType") Is Nothing, 0, Session("SelectedMonitorType"))
        ATA = IIf(Session("ATA") Is Nothing, 0, Session("ATA"))
        Description = IIf(Session("ModelDescription") Is Nothing, String.Empty, Session("ModelDescription"))
        ModNo = IIf(Session("ModNo") Is Nothing, String.Empty, Session("ModNo"))
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "NewADSB"


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
        If InStr(Session("MiddleFrame"), "wfNewADSBList_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub FindNow()
        mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(ModelID:=mModelList(SelectedModelIndex).ID, ModificationType:=mModelMonitorModTypeList(SelectedMonitorType, "").ID, ATACode:=mATAList(ATA).ATACode, ATANomenclature:=String.Empty, Description:=Description, DirectiveNo:=ModNo)
        Session("mModelMonitorModList") = mModelMonitorModList
        dgModelMonitorDirectiveList.DataSource = mModelMonitorModList
        dgModelMonitorDirectiveList.DataBind()

        lblResult.Text = "List Of AD/SB: " & mModelMonitorModList.Count.ToString & " Record(s)"
        SetGrid()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelList")
        Session.Remove("mModelMonitorModList")
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorModTypeList")
        Session.Remove("ModNo")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModelMonitorModList.CurrentIndex = Index
        Session("mModelMonitorModList") = mModelMonitorModList
    End Sub
    Private Sub NewRecord()
        Dim mModelMonitorMod As ModelMonitorMod
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, New Guid(cmbModel.SelectedValue), 1, ID) 'HardFix HourType=1 as diff is only show purpose H OR HD  'For new records ID,PrevRefID are same
        Session("mModelMonitorMod") = mModelMonitorMod
        RemoveSession()
        Session("ModelIDForADSB") = New Guid(cmbModel.SelectedValue)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewADSB_Ajax.aspx?BackPage=wfNewADSBList_Ajax.aspx');", True)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mModelMonitorMod As ModelMonitorMod
        mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mId, 1) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mModelMonitorMod") = mModelMonitorMod
        mDirectiveDetail = "Model : " & mModelMonitorModList(mId).ModelName & " Model Directive Type : " & mModelMonitorMod.ModelMonitorModTypeName & " Directive No: " & mModelMonitorMod.Number & " Description : " & mModelMonitorMod.Description

        MarkLog(Util.Action.Edit, "Model Directive", mDirectiveDetail, Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
        RemoveSession()
        Session("ModelIDForADSB") = New Guid(cmbModel.SelectedValue)
        Session("ModelName") = cmbModel.SelectedItem.ToString
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewADSB_Ajax.aspx?BackPage=wfNewADSBList_Ajax.aspx');", True)
    End Sub
    Private Sub ControlVisibility()
        If Not mModelMonitorModList Is Nothing Then
            btnAddNewTop.Visible = (mModelMonitorModList.Count > 15)
            btnBackTop.Visible = (mModelMonitorModList.Count > 15)
            'btnPrintTop.Visible = (mModelMonitorModList.Count > 15)
            'btnPrint.Enabled = IIf(mModelMonitorModList.Count > 0, True, False)
            'btnPrintTop.Enabled = IIf(mModelMonitorModList.Count > 0, True, False)
        Else
            btnAddNewTop.Visible = False
            btnBackTop.Visible = False
            'btnPrintTop.Visible = False
            'btnPrint.Visible = False
        End If

        If Not mModelList Is Nothing Then
            btnAddNew.Enabled = (mModelList.Count > 0)
            btnAddNewTop.Enabled = (mModelList.Count > 0)
        Else
            btnAddNew.Enabled = False
            btnAddNewTop.Enabled = False
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
                            If mModelMonitorModList(mModelMonitorModList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mModelMonitorModList(mModelMonitorModList.CurrentIndex).ID)
                            End If
                            ModelMonitorMod.DeleteModelMonitorMod(mModelMonitorModList.CurrentItem.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'Added by Vikrant on 28-July-2011
                            mModelName = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelName
                            mATA = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ATAChapter

                            mDirectiveDetail = "Model : " + mModelName + " Directive Type : " + mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelMonitorModTypeName + " Directive No. : " + mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).Number
                            MarkLog(Util.Action.Delete, "Model Directive", mDirectiveDetail, Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)
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
                                mModelName = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelName
                                mATA = mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ATAChapter

                                mDirectiveDetail = "Model : " + mModelName + " Directive Type : " + mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ModelMonitorModTypeName + " Directive No. : " + mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).Number
                                MarkLog(Util.Action.Delete, "Model Directive", "Can't Delete : " & mDirectiveDetail & " is already in use", Util.ErrorType.NoError, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID, EventLogID)

                                'Added by saylee on 1-Jun-2016
                                Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
                                mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mModelList(SelectedModelIndex).ID, mModelMonitorModList.Item(mModelMonitorModList.CurrentIndex).ID.ToString)

                                If mModelMonitorModConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mModelMonitorModConfiguredList.Count - 1
                                        If i = mModelMonitorModConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected Directive is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
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
        lblResult.Text = "List Of AD/SB: " & mModelMonitorModList.Count.ToString & " Record(s)"
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgModelMonitorDirectiveList.Rows.Count - 1
            P = CType(Me.dgModelMonitorDirectiveList.Rows(j).Cells(13).Text, Boolean)
            If P = False Then
                dgModelMonitorDirectiveList.Rows(j).Cells(12).Enabled = False
            End If
        Next
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

        mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(All)")
        cmbMonitorType.DataSource = mModelMonitorModTypeList
        cmbMonitorType.DataBind()
        Session("mModelMonitorModTypeList") = mModelMonitorModTypeList

        If mAssemblyTypeList.Count > 0 Then
            mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
            cmbModel.DataSource = mModelList
            cmbModel.DataBind()
            Session("mModelList") = mModelList

            setModelCombo()
            cmbAssemblyType.SelectedIndex = SelectedAssemblyTypeIndex
            cmbModel.SelectedIndex = SelectedModelIndex
        End If
        cmbATAChapter.SelectedIndex = ATA
        cmbMonitorType.SelectedIndex = SelectedMonitorType
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
            mModelMonitorModList = Nothing
            Session("mModelMonitorModList") = mModelMonitorModList
            dgModelMonitorDirectiveList.DataSource = mModelMonitorModList
            dgModelMonitorDirectiveList.DataBind()
            btnAddNewTop.Visible = False
            btnBackTop.Visible = False
            btnAddNew.Enabled = False
            lblResult.Text = "List Of AD/SB: 0 Record(s)"
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        If Session("MiddleFrame") <> "wfNewADSBList_Ajax.aspx?" Then
            Session.Remove("ModNo")
            Session.Remove("mCompMonitorModStatusList")
            Session.Remove("mAssemblyMonitorModStatusList")
        End If
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            TbContInst.ActiveTabIndex = IIf(CType(Session("NewADSBTabIndex"), Integer) > 0, CType(Session("NewADSBTabIndex"), Integer), 0)
            Session("MiddleFrame") = "wfNewADSBList_Ajax.aspx?"
            If TbContInst.ActiveTabIndex = 1 Then 'Comp Tab
                TbContInst_ActiveTabChanged(sender, e)
            Else
                DataFieldBind()
                ControlVisibility()
            End If
        End If
    End Sub
    Private Sub dgModelMonitorDirectiveList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModelMonitorDirectiveList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgModelMonitorDirectiveList.PageIndex * dgModelMonitorDirectiveList.PageSize
                mID = New Guid(dgModelMonitorDirectiveList.DataKeys(Index).Value.ToString)
                EditRecord(mID)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgModelMonitorDirectiveList.PageIndex * dgModelMonitorDirectiveList.PageSize
                DeleteRecord(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgModelMonitorDirectiveList.PageIndex * dgModelMonitorDirectiveList.PageSize
                mID = New Guid(dgModelMonitorDirectiveList.DataKeys(Index).Value.ToString)
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
        MarkLog(Util.Action.[New], "Model Directive", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Session.Remove("SelectedModelIndex")
        Session.Remove("SelectedAssemblyTypeIndex")
        Session.Remove("SelectedMonitorType")
        Session.Remove("ATA")
        Session.Remove("ModelDescription")
        Session.Remove("NewADSBTabIndex")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgModelMonitorDirectiveList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModelMonitorDirectiveList.Sorting
        mModelMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelMonitorModList") = mModelMonitorModList
        dgModelMonitorDirectiveList.DataSource = mModelMonitorModList
        dgModelMonitorDirectiveList.DataBind()
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
    End Sub
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
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged, txtModNo.TextChanged
        Description = txtDescription.Text.Trim
        Session("ModelDescription") = Description

        ModNo = txtModNo.Text.Trim
        Session("ModNo") = ModNo

        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub TbContInst_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbContInst.ActiveTabChanged
        NewADSBTabIndex = TbContInst.ActiveTabIndex
        Session("NewADSBTabIndex") = NewADSBTabIndex
        Select Case NewADSBTabIndex
            Case 0 'Assembly New ADSB
                Session.Remove("SelectedMonitorTypeForNewCompADSB")
                Session.Remove("ATAForNewCompADSB")
                Session.Remove("DescriptionForNewCompADSB")
                Session.Remove("mPartID")
                Session.Remove("mPartMonitorModListForNewCompADSB")
                Session.Remove("mATAListForNewCompADSB")
                Session.Remove("mPartMonitorModTypeListForNewCompADSB")
                Session.Remove("mAssemblyTypeListForNewCompADSB")
                Session.Remove("mModelListForNewCompADSB")
                DataFieldBind()
                ControlVisibility()
            Case 1 'Comp New ADSB
                Session.Remove("mAssemblyTypeList")
                Session.Remove("mModelList")
                Session.Remove("mModelMonitorModList")
                Session.Remove("mATAList")
                Session.Remove("mModelMonitorModTypeList")
                Session.Remove("SelectedModelIndex")
                Session.Remove("SelectedAssemblyTypeIndex")
                Session.Remove("SelectedMonitorType")
                Session.Remove("ATA")
                Session.Remove("ModelDescription")
                Session("IsTabIndexChaged") = True
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCompADSBList", "CallCompADSBList();", True)
        End Select
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        ''For Issue List
        'Dim Rpt As New crptAssemblyMPDList
        'Dim da As New CSLA.Data.ObjectAdapter
        'Dim ds As New dsMPD
        'Dim mCompanyDetail As New CompanyDetail

        'mModelMonitorInspList = Session("mModelMonitorInspList")

        'mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, "Assembly MPD List", cmbAssemblyType.SelectedItem.ToString, cmbModel.SelectedItem.ToString, cmbMonitorType.SelectedItem.ToString, cmbATAChapter.SelectedItem.ToString, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        'Dim Reccount As Integer = 0
        'If AppSettings("ClientCode") = "IAT" Then
        '    Reccount = mModelMonitorInspList.RecordCount
        'Else
        '    Reccount = mModelMonitorInspList.Count
        'End If
        'If Reccount = 0 Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        'Dim mrptImage As rptImage = rptImage.GetImage(ds)
        'da.Fill(ds, mrptImage)
        'da.Fill(ds, mModelMonitorInspList)
        'da.Fill(ds, Report)
        'Rpt.SetDataSource(ds)
        'Session("CrystalReport") = Rpt

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region



End Class