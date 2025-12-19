'Added by vikrant For AD/SB
Imports System.Collections.Generic
Imports Flypal.PartListAutoComplete
Imports System.Linq

Public Class wfNewCompADSBList_Ajax
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
    Public mPartMonitorModList As PartMonitorModList
    Public mATAList As ATAList
    Public mPartMonitorModTypeList As PartMonitorModTypeList
    Dim EventLogID As Guid
    Public mDirectiveDetail As String
    Public mATA As String
    Public mModelName As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim SelectedModelIndex, SelectedAssemblyTypeIndex, SelectedMonitorType, ATA As Integer
    Dim Description As String = String.Empty
    Dim ModNo As String = String.Empty
    Shared mPartID, mModelIDForNewCompADSB As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyTypeList = CType(Session("mAssemblyTypeListForNewCompADSB"), AssemblyTypeList)
        mModelList = CType(Session("mModelListForNewCompADSB"), ModelList)
        mPartMonitorModList = CType(Session("mPartMonitorModListForNewCompADSB"), PartMonitorModList)
        SelectedModelIndex = IIf(Session("SelectedModelIndexForNewCompADSB") Is Nothing, 0, Session("SelectedModelIndexForNewCompADSB"))
        mATAList = CType(Session("mATAListForNewCompADSB"), ATAList)
        mPartMonitorModTypeList = CType(Session("mPartMonitorModTypeListForNewCompADSB"), PartMonitorModTypeList)
        SelectedMonitorType = IIf(Session("SelectedMonitorTypeForNewCompADSB") Is Nothing, 0, Session("SelectedMonitorTypeForNewCompADSB"))
        ATA = IIf(Session("ATAForNewCompADSB") Is Nothing, 0, Session("ATAForNewCompADSB"))
        Description = IIf(Session("DescriptionForNewCompADSB") Is Nothing, String.Empty, Session("DescriptionForNewCompADSB"))
        ModNo = IIf(Session("ModNoForNewCompADSB") Is Nothing, String.Empty, Session("ModNoForNewCompADSB"))
        If PartID.Value <> "" Then
            mPartID = New Guid(PartID.Value)
        Else
            mPartID = Guid.Empty
        End If
        mModelIDForNewCompADSB = Session("mModelIDForNewCompADSB")
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
    'Private Sub ClearAll()
    '    If InStr(Session("MiddleFrame"), "wfNewCompADSBList_Ajax.aspx?") <= 0 Then
    '        RemoveSession()
    '    End If
    'End Sub
    Private Sub FindNow()
        mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(mPartID, ModelID:=mModelList(SelectedModelIndex).ID, ModType:=mPartMonitorModTypeList(SelectedMonitorType, "").ID, ATACode:=mATAList(ATA).ATACode, ATANomenclature:=String.Empty, Description:=Description, DirectiveNo:=ModNo)
        Session("mPartMonitorModListForNewCompADSB") = mPartMonitorModList
        dgPartMonitorModList.DataSource = mPartMonitorModList
        dgPartMonitorModList.DataBind()
        lblResult.Text = "List Of AD/SB: " & mPartMonitorModList.Count & " Record(s)"
        SetGrid()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartMonitorModListForNewCompADSB")
        Session.Remove("mATAListForNewCompADSB")
        Session.Remove("mPartMonitorModTypeListForNewCompADSB")
        Session.Remove("mAssemblyTypeListForNewCompADSB")
        Session.Remove("mModelListForNewCompADSB")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartMonitorModList.CurrentIndex = Index
        Session("mPartMonitorModListForNewCompADSB") = mPartMonitorModList
    End Sub
    Private Sub NewRecord()
        Dim mPartMonitorMod As PartMonitorMod
        mPartMonitorMod = PartMonitorMod.NewPartMonitorMod(Guid.NewGuid, mPartID, New Guid(cmbModel.SelectedValue), 1)
        Session("mPartMonitorMod") = mPartMonitorMod
        RemoveSession()
        Session("ModelIDForNewCompADSB") = New Guid(cmbModel.SelectedValue)
        Session("PartIDForNewCompADSB") = mPartID
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompADSB_Ajax.aspx?BackPage=wfNewADSBList_Ajax.aspx');", True)
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mPartMonitorMod As PartMonitorMod
        mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mId, 1) 'HardFix HourType=1 as diff is only show purpose H OR HD
        Session("mPartMonitorMod") = mPartMonitorMod
        Session("PartIDForNewCompADSB") = mPartMonitorModList(mId).PartID
        mDirectiveDetail = "Part : " & mPartMonitorMod.Part.Name & " Part Modification Type : " & mPartMonitorMod.PartMonitorModTypeName & " Mod No. : " & mPartMonitorMod.Number & " Description : " & mPartMonitorMod.Description
        MarkLog(Util.Action.Edit, "Part Mod", mDirectiveDetail, Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNewCompADSB_Ajax.aspx?BackPage=wfNewADSBList_Ajax.aspx');", True)
    End Sub
    Private Sub ControlVisibility()
        If Not mPartMonitorModList Is Nothing Then
            btnAddNewTop.Visible = (mPartMonitorModList.Count > 15)
            btnBackTop.Visible = (mPartMonitorModList.Count > 15)
            'btnPrintTop.Visible = (mPartMonitorModList.Count > 15)
            'btnPrint.Enabled = IIf(mPartMonitorModList.Count > 0, True, False)
            'btnPrintTop.Enabled = IIf(mPartMonitorModList.Count > 0, True, False)
        Else
            btnAddNewTop.Visible = False
            btnBackTop.Visible = False
            'btnPrintTop.Visible = False
            'btnPrint.Visible = False
        End If
        btnAddNew.Enabled = Not mPartID.Equals(Guid.Empty)
        btnAddNewTop.Enabled = Not mPartID.Equals(Guid.Empty)
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
                            mDirectiveDetail = "Mod Type : " + mPartMonitorModList(mPartMonitorModList.CurrentIndex).PartMonitorModTypeName + " Description : " + mPartMonitorModList(mPartMonitorModList.CurrentIndex).Description + " Mod No. : " + mPartMonitorModList(mPartMonitorModList.CurrentIndex).Number

                            mId = mPartMonitorModList(mPartMonitorModList.CurrentIndex).ID
                            If mPartMonitorModList(mPartMonitorModList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorModList(mPartMonitorModList.CurrentIndex).ID)
                            End If
                            PartMonitorMod.DeletePartMonitorMod(mPartMonitorModList.CurrentItem.id)
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
                                MarkLog(Util.Action.Delete, "Part Mod", "Can't Delete:" & mDirectiveDetail & " is already in use", Util.ErrorType.NoError, mId, EventLogID)
                                'End
                                'Added by saylee on 1-Jun-2016
                                Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
                                mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).PartID, mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).ID.ToString)

                                If mPartMonitorModConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mPartMonitorModConfiguredList.Count - 1
                                        If i = mPartMonitorModConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected Modification is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 26-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Part Mod", mDirectiveDetail, Util.ErrorType.NoError, mId, EventLogID)
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
        lblResult.Text = "List Of AD/SB: " & mPartMonitorModList.Count & " Record(s)"
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgPartMonitorModList.Rows.Count - 1
            P = CType(Me.dgPartMonitorModList.Rows(j).Cells(14).Text, Boolean)
            If P = False Then
                dgPartMonitorModList.Rows(j).Cells(13).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAListForNewCompADSB") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeListForNewCompADSB") = mAssemblyTypeList
        cmbAssemblyType.DataBind()

        mPartMonitorModTypeList = PartMonitorModTypeList.GetPartMonitorModTypeList("(All)")
        cmbMonitorType.DataSource = mPartMonitorModTypeList
        cmbMonitorType.DataBind()
        Session("mPartMonitorModTypeListForNewCompADSB") = mPartMonitorModTypeList

        If mAssemblyTypeList.Count > 0 Then
            mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
            cmbModel.DataSource = mModelList
            cmbModel.DataBind()
            Session("mModelListForNewCompADSB") = mModelList
            cmbAssemblyType.SelectedIndex = SelectedAssemblyTypeIndex
            cmbModel.SelectedIndex = SelectedModelIndex
            setModelCombo()
        End If
        cmbModel.SelectedIndex = SelectedModelIndex
        cmbATAChapter.SelectedIndex = ATA
        cmbMonitorType.SelectedIndex = SelectedMonitorType
        txtDescription.Text = Description
        txtModNo.Text = ModNo
    End Sub
    Private Sub setModelCombo()
        If mModelList.Count > 0 Then
            txtPartDescription.Enabled = True
            txtPartDescription.BackColor = Color.White
            cmbModel.Enabled = True
            mModelIDForNewCompADSB = New Guid(cmbModel.SelectedValue)
            Session("mModelIDForNewCompADSB") = mModelIDForNewCompADSB
            FindNow()
            ControlVisibility()
            SetPage()
        Else
            cmbModel.Enabled = False
            txtPartDescription.Enabled = False
            txtPartDescription.BackColor = Color.Gainsboro
            mPartMonitorModList = Nothing
            Session("mPartMonitorModListForNewCompADSB") = mPartMonitorModList
            dgPartMonitorModList.DataSource = mPartMonitorModList
            dgPartMonitorModList.DataBind()
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
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Or Session("IsTabIndexChaged") = True Then
            'Session("MiddleFrame") = "wfNewCompADSBList_Ajax.aspx"
            Session.Remove("IsTabIndexChaged")
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgPartMonitorModList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartMonitorModList.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgPartMonitorModList.PageIndex * dgPartMonitorModList.PageSize
                mID = New Guid(dgPartMonitorModList.DataKeys(Index).Value.ToString)
                EditRecord(mID)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Index = CInt(e.CommandArgument) + dgPartMonitorModList.PageIndex * dgPartMonitorModList.PageSize
                DeleteRecord(Index)
            Case "View"
                Index = CInt(e.CommandArgument) + dgPartMonitorModList.PageIndex * dgPartMonitorModList.PageSize
                mID = New Guid(dgPartMonitorModList.DataKeys(Index).Value.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttachForNewCompADSB") = mFileAttach
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
        MarkLog(Util.Action.[New], "Model Mod", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        NewRecord()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Session.Remove("SelectedMonitorTypeForNewCompADSB")
        Session.Remove("ATAForNewCompADSB")
        Session.Remove("DescriptionForNewCompADSB")
        Session.Remove("mPartID")
        Session.Remove("NewADSBTabIndex")
        'Response.Redirect("index.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgPartMonitorModList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartMonitorModList.Sorting
        mPartMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorModListForNewCompADSB") = mPartMonitorModList
        dgPartMonitorModList.DataSource = mPartMonitorModList
        dgPartMonitorModList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        ATA = cmbATAChapter.SelectedIndex
        Session("ATAForNewCompADSB") = ATA
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbMonitorType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonitorType.SelectedIndexChanged
        SelectedMonitorType = cmbMonitorType.SelectedIndex
        Session("SelectedMonitorTypeForNewCompADSB") = SelectedMonitorType
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As System.EventArgs) Handles txtDescription.TextChanged
        Description = txtDescription.Text.Trim
        Session("DescriptionForNewCompADSB") = Description
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub txtModNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtModNo.TextChanged
        ModNo = txtModNo.Text.Trim
        Session("ModNoForNewCompADSB") = ModNo
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
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbModel_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbModel.SelectedIndexChanged
        mModelIDForNewCompADSB = New Guid(cmbModel.SelectedValue)
        SelectedModelIndex = cmbModel.SelectedIndex
        txtPartDescription.Text = ""
        PartID.Value = ""
        mPartID = Guid.Empty
        Session("mModelIDForNewCompADSB") = mModelIDForNewCompADSB
        Session("SelectedModelIndexForNewCompADSB") = SelectedModelIndex
        FindNow()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        SelectedModelIndex = 0
        Session("SelectedModelIndexForNewCompADSB") = SelectedModelIndex
        SelectedAssemblyTypeIndex = cmbAssemblyType.SelectedIndex
        Session("SelectedAssemblyTypeIndexForNewCompADSB") = SelectedAssemblyTypeIndex
        txtPartDescription.Text = ""
        PartID.Value = ""
        mPartID = Guid.Empty
        mModelList = ModelList.GetModelList(mAssemblyTypeList(SelectedAssemblyTypeIndex).ID, "", , )
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()

        Session("mModelListForNewCompADSB") = mModelList
        setModelCombo()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        ''For Issue List
        'Dim Rpt As New crptCompMPDList
        'Dim da As New CSLA.Data.ObjectAdapter
        'Dim ds As New dsMPD
        'Dim mCompanyDetail As New CompanyDetail

        'mPartMonitorModList = Session("mPartMonitorInspListForNewCompMPD")

        'mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, "Part MPD List", cmbAssemblyType.SelectedItem.ToString, cmbModel.SelectedItem.ToString, cmbMonitorType.SelectedItem.ToString, cmbATAChapter.SelectedItem.ToString, txtPartDescription.Text.Trim, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        'If mPartMonitorModList.Count = 0 Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        'Dim mrptImage As rptImage = rptImage.GetImage(ds)
        'da.Fill(ds, mrptImage)
        'da.Fill(ds, mPartMonitorModList)
        'da.Fill(ds, Report)
        'Rpt.SetDataSource(ds)
        'Session("CrystalReport") = Rpt

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim partlist As PartListAutoComplete
        partlist = PartListAutoComplete.GetPartList(prefixText, mModelIDForNewCompADSB.ToString)
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