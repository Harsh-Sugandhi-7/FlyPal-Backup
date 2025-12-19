
'Created By Utkarsh On 11-Nov-2013

Public Class wfApplicableFor_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mModelList As ModelList
    Public mAssemblyTypeList As AssemblyTypeList
    Public mModelID As Guid
    Public mModelName As String

    Dim EventLogID As Guid 'Added By Utkarsh On 20-Jul-2011 For All19072011
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mItem = Session("mItem")
        mModelList = Session("mModelList")
        mAssemblyTypeList = Session("mAssemblyTypeList")
        mModelID = Session("mModelID")
        mModelName = Session("mModelName")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelList")
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelID")
        Session.Remove("mModelName")
    End Sub
    Private Sub SetSession()
        Session("mItem") = mItem
        Session("mModelList") = mModelList
        Session("mAssemblyTypeList") = mAssemblyTypeList
    End Sub
    Private Sub NewRecord()
        mItem.ItemApplicables.Add(mItem.ID)
        'mItem.ItemApplicables.CurrentIndex = mItem.ItemApplicables.Count - 1
        mItem.ItemApplicables.CurrentItem.SrNo = mItem.ItemApplicables.Count
        mItem.ItemApplicables.CurrentItem.ModelName = ""
        For i As Integer = 0 To mItem.ItemApplicables.Count - 1
            mItem.ItemApplicables(i).SrNo = i + 1
        Next
        Session("mItem") = mItem
        ClearControls()
    End Sub
    Private Function setObject() As Boolean
        SetModelIDName()
        mItem.ItemApplicables.CurrentItem.ModelType = cmbTypeList.SelectedItem.Text
        mItem.ItemApplicables.CurrentItem.ModelID = mModelID
        mItem.ItemApplicables.CurrentItem.ModelName = mModelName
        mItem.ItemApplicables.CurrentItem.GroundSupportEquipment = chkGroundSupportEquipment.Checked 'Added by Kalpesh as 14-Feb-2008

        If mItem.ItemApplicables.Contains(mItem.ItemApplicables.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Applicable Model", MsgBoxStyle.Information, "")
            Return False
        End If
        Return True
    End Function
    Private Sub DeleteRecord(ByVal Idx As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mItem.ItemApplicables.CurrentIndex = Idx
        Session("mItem") = mItem
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
                            mItem = Session("mItem")
                            mItem.ItemApplicables.Remove(mItem.ItemApplicables.CurrentItem)
                            mItem.ItemApplicables.CurrentIndex = mItem.ItemApplicables.Count - 1
                            Session("mItem") = mItem
                            BindGrid()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
                            End If
                            BindGrid()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        mItem.ItemApplicables.CurrentIndex = mItem.ItemApplicables.Count - 1
                        Session("mItem") = mItem
                        Session("sender") = ""
                        BindGrid()
                    End If
                Case MsgBoxResult.Ok ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            '    DataFieldBind()
        End If
    End Sub
    Private Sub ControlVisibility()
        If mItem.ItemApplicables.Count > 0 Then
            For i As Integer = 0 To mItem.ItemApplicables.Count - 1
                If mItem.ItemApplicables(i).ModelName = "" Then
                    gdvItemApplicables.Rows(i).Visible = False
                End If
            Next
        End If
    End Sub
    Private Sub TypeChanged()
        Dim ModelTypeId As Int16 = Val(cmbTypeList.SelectedValue)
        BindCombo(ModelTypeId, True, False)
    End Sub
    Private Sub SetModelIDName(Optional ByVal OnPageLoad As Boolean = False)
        If ModelIDValue.Value.Length = 0 Then
            If OnPageLoad Then
                mModelID = Guid.Empty
                Session("mModelID") = mModelID
            Else
                mModelID = Session("mModelID")
            End If
        Else
            mModelID = New Guid(ModelIDValue.Value)
            Session("mModelID") = mModelID
        End If

        If ModelNameValue.Value.Length = 0 Then
            If OnPageLoad Then
                mModelName = String.Empty
                Session("mModelName") = mModelName
            Else
                mModelName = Session("mModelName")
            End If
        Else
            mModelName = ModelNameValue.Value
            Session("mModelName") = mModelName
        End If

    End Sub
    Private Sub ClearControls()
        chkGroundSupportEquipment.Checked = False
        ModelIDValue.Value = ""
        ModelNameValue.Value = ""
        SetModelIDName(True)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind(Optional ByVal ModelTypeId As Int16 = 0, Optional ByVal FetchFromDatabase As Boolean = False, Optional ByVal OnPageLoad As Boolean = False)
        BindCombo(ModelTypeId, FetchFromDatabase, OnPageLoad)
        BindGrid()
    End Sub
    Private Sub BindGrid()
        lblResult.Text = "List of Applicable Models :" + CType(mItem.ItemApplicables.Count - 1, String) + " Record(s)."
        gdvItemApplicables.DataSource = mItem.ItemApplicables
        gdvItemApplicables.DataBind()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub BindCombo(Optional ByVal ModelTypeId As Int16 = 0, Optional ByVal FetchFromDatabase As Boolean = False, Optional ByVal OnPageLoad As Boolean = False)
        If FetchFromDatabase Then
            'Added by Kalpesh Shah as on 14-Feb-2008 
            If Not AppSettings("ClientCode") Is Nothing Then
                Select Case AppSettings("ClientCode").ToString
                    Case "AWI"
                        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeListForItemApplcable("", False)
                    Case "DEC"
                        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeListForItemApplcable("", True)
                    Case Else
                        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeListForItemApplcable("", True)
                End Select
            Else
                mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeListForItemApplcable("", True)
            End If
            Session("mAssemblyTypeList") = mAssemblyTypeList
            If OnPageLoad Then
                ModelTypeId = mAssemblyTypeList(0).ID
                cmbTypeList.DataSource = mAssemblyTypeList
                cmbTypeList.DataBind()
            End If
            mModelList = ModelList.GetModelList(ModelTypeId, ModelList.IsSelectTagRequired.True)
            Session("mModelList") = mModelList

        End If
        cmbModelList.DataSource = mModelList
        cmbModelList.DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 20-Jul-2011 For All19072011
        If Not IsPostBack Then
            If cmbTypeList.Enabled = True Then
                setFocus(cmbTypeList)
            End If
            SetModelIDName(True)
            DataFieldBind(FetchFromDatabase:=True, OnPageLoad:=True)
            'If Session("PartInfo") = "True" Then 'Added by Prashant 22-Aug-2018 ALL22082018
            '    btnOpeningStock.Visible = False
            '    btnAlternatePart.Visible = False
            'End If
        End If
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        If IsValid Then
            If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
                BindCombo()
                Exit Sub
            End If
            If setObject() Then
                NewRecord()
                DataFieldBind(FetchFromDatabase:=True, OnPageLoad:=True)
            Else
                BindCombo()
            End If
        End If

    End Sub
    Private Sub cmbTypeList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTypeList.SelectedIndexChanged
        TypeChanged()
        setFocus(cmbTypeList)
    End Sub
    Private Sub gdvItemApplicables_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvItemApplicables.RowCommand
        Select Case e.CommandName
            Case "Remove"
                Dim index As Integer = CInt(e.CommandArgument) + gdvItemApplicables.PageIndex * gdvItemApplicables.PageSize
                If (Not User.IsInRole("PartDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
                    BindGrid()
                    Exit Sub
                End If
                DeleteRecord(index)
                BindGrid()
        End Select
    End Sub
    Private Sub imgbtnModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnModelsNew.Click
        'If mItem.ItemApplicables.CurrentItem.ModelName = "" Then mItem.ItemApplicables.Remove(mItem.ItemApplicables.CurrentItem)
        SetSession()
        Session("Type") = True
        Session("AssemblyTypeId") = cmbTypeList.SelectedValue.ToString
        BindCombo()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelWindow", "OpenModelWindow();", True)
        'Response.Redirect("wfModel.aspx?BackPage1=wfApplicableFor.aspx&BackPage=" & Request.QueryString("BackPage"))
        'Response.Redirect("wfModel_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage1=wfApplicableFor_Ajax.aspx&Type=True&AssemblyTypeId=0")
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnbtnModel_Click(sender As Object, e As System.EventArgs) Handles hdnbtnModel.Click
        ClearControls()
        BindCombo(FetchFromDatabase:=True, OnPageLoad:=True)
    End Sub
#End Region

#Region " Navigation "
    Private Sub btnPartInformation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click ''btnPartInformation.Click,
        'Changed By Saylee on 12-Dec-2007 to solved Bug No:-PD6
        If mItem.ItemApplicables.Count > 0 Then
            For i As Integer = 0 To mItem.ItemApplicables.Count - 1
                If mItem.ItemApplicables(i).ModelName = "" Then
                    mItem.ItemApplicables.Remove(mItem.ItemApplicables(i))
                End If
            Next
        End If
        Session("mItem") = mItem
        RemoveSession()
        'Response.Redirect(Request.QueryString("BackPage") & "?")
        Response.Redirect(Request.QueryString("BackPage")) ' & "?")
    End Sub
    'Private Sub btnAlternatePart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlternatePart.Click
    '    If mItem.ItemApplicables.Count > 0 Then
    '        For i As Integer = 0 To mItem.ItemApplicables.Count - 1
    '            If mItem.ItemApplicables(i).ModelName = "" Then
    '                mItem.ItemApplicables.Remove(mItem.ItemApplicables(i))
    '            End If
    '        Next
    '    End If
    '    Session("mItem") = mItem
    '    RemoveSession()
    '    Response.Redirect("wfAlternatePartChild_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    'End Sub
    'Private Sub btnOpeningStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpeningStock.Click
    '    If mItem.ItemApplicables.Count > 0 Then
    '        For i As Integer = 0 To mItem.ItemApplicables.Count - 1
    '            If mItem.ItemApplicables(i).ModelName = "" Then
    '                mItem.ItemApplicables.Remove(mItem.ItemApplicables(i))
    '            End If
    '        Next
    '    End If
    '    Session("mItem") = mItem
    '    RemoveSession()
    '    'Response.Redirect("wfOpeningBalanceList.aspx?ChildPage=wfApplicableFor.aspx&BackPage=" & Request.QueryString("BackPage"))
    '    Response.Redirect("wfOpeningBalanceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    'End Sub
#End Region


End Class