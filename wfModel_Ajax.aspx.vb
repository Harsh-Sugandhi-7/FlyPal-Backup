'Added by Bhushan

Public Class wfModel_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine   'Code Added
    Public mItem As Item         'Code Added
    Public mModel As Model
    Public mModelList As ModelList
    Public mAssemblyTypeList As AssemblyTypeList
    Public mManufacturerList As ManufacturerList
    Public mAssemblyTypeId As Integer
    Public Type As Boolean = False
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Public mPrimaryModelList As PrimaryModelList
    Public mCompanyDetail As New CompanyDetail
#End Region

#Region " Business Methods "

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mMachine = Session("mMachine") 'Code added
        mItem = Session("mItem")       'Code Added   
        mModel = Session("mModel")
        mModelList = Session("mModelList")
        mAssemblyTypeList = Session("mAssemblyTypeList")
        mManufacturerList = Session("mManufacturerList")
        mAssemblyTypeId = CType(Session("AssemblyTypeId"), Integer)
        Type = CType(Session("Type"), Boolean)
        mPrimaryModelList = Session("mPrimaryModelList")
        mCompanyDetail = Session("mCompanyDetail")
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine   'Code Added
        Session("mItem") = mItem         'Code Added 
        Session("mModel") = mModel
        Session("mModelList") = mModelList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mManufacturerList") = mManufacturerList
        Session("mAssemblyTypeId") = mAssemblyTypeId
        Session("Type") = Type
        Session("mPrimaryModelList") = mPrimaryModelList
        Session("mCompanyDetail") = mCompanyDetail
    End Sub
    Private Sub NewRecord()
        mModel = Model.NewModel(Guid.NewGuid, CInt(IIf(Type = True, 1, mAssemblyTypeId)))
        Session("mModel") = mModel
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mModel = Model.GetModel(mId)
        Session("mModel") = mModel
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfModel.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")        
        'msg1.Show()
        'Session("sender") = "Delete"
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mModel = Model.GetModel(mId)
        Session("mModel") = mModel
    End Sub
    Private Sub setObject()
        mModel.ManufacturerID = New Guid(cmbManufacturerList.SelectedValue)
        mModel.Name = txtName.Text
        mModel.AssemblyTypeID = Val(cmbForAssemblyList.SelectedValue)
        'Ajay 30-Nov-2022
        'mModel.FixedWing = rdbFixedWing.Checked
        'mModel.RotaryWing = rdbRotaryWing.Checked
        '------------------
        If cmbPrimaryModelList.SelectedIndex = 0 Then
            'Do notning
        Else
            mModel.PrimaryModelID = New Guid(cmbPrimaryModelList.SelectedValue)
            mModel.PrimaryModelName = cmbPrimaryModelList.SelectedItem.Text
        End If
        Session("mModel") = mModel
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'If CType(Session("sender"), String) = "Delete" Then
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            'Session("sender") = ""
                            mModel = Session("mModel")
                            Model.DeleteModel(mModel.ID)
                            Session.Remove("mModel")
                            NewRecord()
                            DataFieldBind()
                            lblTitle.Text = "Model Information [New]"

                            'Response.Redirect("wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfModel.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfModel.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfModel.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Changed by Vikrant on 22-July-2011
                                MarkLog(Util.Action.Delete, "Model", "Can't delete :" & mModel.Name & " is Currently in use", Util.ErrorType.NoError, mModel.ID, EventLogID)

                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed by Vikrant on 22-July-2011
                                MarkLog(Util.Action.Delete, "Model", mModel.Name, Util.ErrorType.NoError, mModel.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    'Session("sender") = ""
                    ' Response.Redirect("wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    'Session("sender") = ""
                    DataFieldBind()
                    ' Response.Redirect("wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    'Session("sender") = ""
                    DataFieldBind()
                    ' Response.Redirect("wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
            End Select
        ElseIf Result1 = -1 Then
            'Session("sender") = ""
            DataFieldBind()
            ' Response.Redirect("wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            '  DataFieldBind()
        End If
        upnlModel.Update()
    End Sub
    Private Sub ControlVisibility()
        'If Not Type Then imgbtnModel.BackColor = Color.Gainsboro
        'Ajay 30-Nov-2022
        'If mModel.AssemblyTypeID = 1 And mCompanyDetail.IsSyncApplication = True Then
        If mModel.AssemblyTypeID = 1 Then
            'rdbFixedWing.Visible = False
            'rdbRotaryWing.Visible = False
            PrimaryModelPlaceHolder.Visible = True
            dgModel.Columns(1).Visible = True
        Else
            'rdbFixedWing.Visible = False
            'rdbRotaryWing.Visible = False
            PrimaryModelPlaceHolder.Visible = False
            dgModel.Columns(1).Visible = False
        End If
        '-----------------
    End Sub
    Private Sub SetTitle()

    End Sub
    Private Sub DisableName(mID As Guid) 'Added by : Saylee 17-Jun-2020, ALL16062020

        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerModel(mID)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModelList = ModelList.GetModelList(IIf(Type = True, 0, mAssemblyTypeId))
        mManufacturerList = ManufacturerList.GetManufacturerList(, "(SELECT)")
        mPrimaryModelList = PrimaryModelList.GetPrimaryModelList(AddTopItem:="(SELECT)")
        cmbPrimaryModelList.DataSource = mPrimaryModelList
        cmbManufacturerList.DataSource = mManufacturerList
        dgModel.DataSource = mModelList
        Session("mManufacturerList") = mManufacturerList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mModelList") = mModelList
        Session("mPrimaryModelList") = mPrimaryModelList
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Session("mCompanyDetail") = mCompanyDetail
        If Session("Save") = False Then       'Done this By Saylee on 11th Dec-07 to solve bug-MD1(comboBox getting refreshed after saving) given By Pramod
            mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
            cmbForAssemblyList.DataSource = mAssemblyTypeList
        End If
        DataBind()
        lblSearch.Text = "Model List: " & mModelList.Count & " Record(s) Found."


    End Sub

    'Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)

    '    If custValidator.ControlToValidate = "cmbModelType" Then
    '        If cmbForAssemblyList.SelectedIndex <= 0 Then
    '            custValidator.ErrorMessage = "Select Model Type from the list."
    '            e.IsValid = False
    '        End If
    '    ElseIf custValidator.ControlToValidate = "cmbManufacturerList" Then
    '        If cmbManufacturerList.SelectedIndex <= 0 Then
    '            custValidator.ErrorMessage = "Select Manufacturer from the list."
    '            e.IsValid = False
    '        End If
    '    End If

    'End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)           'Added by Vikrant on 22-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbManufacturerList.Enabled = True Then
                setFocus(cmbManufacturerList)
            End If
            Session("Save") = False 'Done this By Saylee on 11th Dec-07 to solve bug-MD1(comboBox getting refreshed after saving) given By Pramod
            Type = CType(Session("Type"), Boolean)
            mAssemblyTypeId = CType(Session("AssemblyTypeId"), Integer)
            Session("Type") = Type
            Session("mAssemblyTypeId") = mAssemblyTypeId
            NewRecord()
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed by Vikrant on 22-July-2011
        MarkLog(Util.Action.Close, "Model", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        'Added by utkarsh for Model Master as Popup
        Dim mopenas As String = Request.QueryString("OpenAs")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mModel")
            Session.Remove("mModelList")
            Session.Remove("mCompanyDetail")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Type = False And Session("FromCopyUtility") <> "True" Then
            If Session("IsForSpareAssembly") = False Then
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    setObject()
                    SetSession()
                    'Changed by Vikrant on 22-July-2011
                    MarkLog(Util.Action.Save, "Model", User.Identity.Name & " is not Authorized User to save " & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")               
                    'msg.Show()
                    'Session("sender") = "Authorization"
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
            End If
        End If
        If Type = True And Session("FromCopyUtility") <> "True" Then
            If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
                setObject()
                SetSession()
                'Changed by Vikrant on 22-July-2011
                MarkLog(Util.Action.Save, "Model", User.Identity.Name & " is not Authorized User to save " & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                
                'msg.Show()
                'Session("sender") = "Authorization"
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                Exit Sub
            End If
        End If

        If cmbForAssemblyList.SelectedIndex = 0 Then
            If cmbPrimaryModelList.SelectedIndex = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mModel.Save()
            If cmbManufacturerList.Enabled = True Then
                setFocus(cmbManufacturerList)
            End If
            'Changed by Vikrant on 22-July-2011
            MarkLog(Util.Action.Save, "Model", mModel.Name, Util.ErrorType.HandledError, mModel.ID, EventLogID)
            NewRecord()
            Session("Save") = True 'Done this By Saylee on 11th Dec-07 to solve bug-MD1(comboBox getting refreshed after saving) given By Pramod
            DataFieldBind()
            SetSession()
            lblTitle.Text = "Model Information [New]"
            upnlModel.Update()
        Catch ex As SqlException
            If ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfModel.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                'msg1.Show()
                'Session("sender") = "Delete"
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfModel.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                
                'msg1.Show()
                'Session("sender") = "Delete"
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 547 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfModel.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                
                'msg1.Show()
                'Session("sender") = "Delete"
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            End If
        End Try
    End Sub
    '''''Private Sub dgModel_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgModel.ItemCommand
    '''''    If e.Item.Cells(0).Text = "ID" Or e.Item.Cells(0).Text = "" Then Exit Sub
    '''''    Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
    '''''    Select Case e.CommandName
    '''''        Case "Edit"
    '''''            If Type = False And Session("FromCopyUtility") <> "True" Then
    '''''                If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
    '''''                    setObject()
    '''''                    SetSession()
    '''''                    'Changed by Vikrant on 22-July-2011
    '''''                    MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit" & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
    '''''                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '''''                    'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                        
    '''''                    'msg.Show()
    '''''                    'Session("sender") = "Authorization"
    '''''                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
    '''''                    Exit Sub
    '''''                End If
    '''''            End If

    '''''            If Type = True And Session("FromCopyUtility") <> "True" Then
    '''''                If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
    '''''                    setObject()
    '''''                    SetSession()
    '''''                    'Changed by Vikrant on 22-July-2011
    '''''                    MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit " & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
    '''''                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '''''                    'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                       
    '''''                    'msg.Show()
    '''''                    'Session("sender") = "Authorization"
    '''''                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
    '''''                    Exit Sub
    '''''                End If
    '''''            End If

    '''''            EditRecord(mId)
    '''''            txtName.Text = mModel.Name 'txtName.DataBind()
    '''''            cmbForAssemblyList.DataBind()
    '''''            cmbManufacturerList.DataBind()
    '''''            'Changed by Vikrant on 22-July-2011
    '''''            MarkLog(Util.Action.Edit, "Model", mModel.Name, Util.ErrorType.NoError, mModel.ID, EventLogID)
    '''''            If Len(mModel.Name) > 15 Then
    '''''                lblTitle.Text = "Model Information [" & mModel.Name.Substring(0, 15) & "...]"
    '''''            Else
    '''''                lblTitle.Text = "Model Information [" & mModel.Name & "]"
    '''''            End If
    '''''            If cmbManufacturerList.Enabled = True Then
    '''''                setFocus(cmbManufacturerList)
    '''''            End If
    '''''        Case "Delete"
    '''''            If Type = False And Session("FromCopyUtility") <> "True" Then
    '''''                ''If (Not User.IsInRole("MachineDelete")) Then
    '''''                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
    '''''                    setObject()
    '''''                    SetSession()
    '''''                    'Changed by Vikrant on 22-July-2011
    '''''                    MarkLog(Util.Action.Delete, "Model", User.Identity.Name & " is not Authorized User to delete " & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
    '''''                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '''''                    'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                       
    '''''                    'msg.Show()
    '''''                    'Session("sender") = "Authorization"
    '''''                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
    '''''                    Exit Sub
    '''''                End If
    '''''            End If
    '''''            If Type = True And Session("FromCopyUtility") <> "True" Then
    '''''                ''If (Not User.IsInRole("PartDelete")) Then
    '''''                If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
    '''''                    setObject()
    '''''                    SetSession()
    '''''                    'Changed by Vikrant on 22-July-2011
    '''''                    MarkLog(Util.Action.Delete, "Model", User.Identity.Name & " is not Authorized User to delete" & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
    '''''                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '''''                    'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                      
    '''''                    'msg.Show()
    '''''                    'Session("sender") = "Authorization"
    '''''                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
    '''''                    Exit Sub
    '''''                End If
    '''''            End If
    '''''            DeleteRecord(mId)
    '''''            'Changed by Vikrant on 22-July-2011
    '''''            MarkLog(Util.Action.Delete, "Model", mModel.Name, Util.ErrorType.HandledError, mModel.ID, EventLogID)

    '''''    End Select
    '''''End Sub
    Private Sub dgModel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModel.RowCommand

        Dim Idx As Int32
        Dim mId As Guid

        Select Case e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgModel.PageIndex * dgModel.PageSize
                mId = mModelList(Idx).ID
                If Type = False And Session("FromCopyUtility") <> "True" Then
                    If Session("IsForSpareAssembly") = False Then

                        If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                            setObject()
                            SetSession()
                            'Changed by Vikrant on 22-July-2011
                            MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit" & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                            'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                        
                            'msg.Show()
                            'Session("sender") = "Authorization"
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                            Exit Sub
                        End If
                    End If
                End If

                If Type = True And Session("FromCopyUtility") <> "True" Then
                    If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
                        setObject()
                        SetSession()
                        'Changed by Vikrant on 22-July-2011
                        MarkLog(Util.Action.Edit, "Model", User.Identity.Name & " is not Authorized User to edit " & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                        'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                        'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                       
                        'msg.Show()
                        'Session("sender") = "Authorization"
                        MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                        Exit Sub
                    End If
                End If

                EditRecord(mId)
                txtName.Text = mModel.Name 'txtName.DataBind()
                cmbForAssemblyList.DataBind()
                cmbManufacturerList.DataBind()
                cmbPrimaryModelList.DataBind()
                'Ajay 30-Nov-2022
                'rdbFixedWing.Checked = mModel.FixedWing
                'rdbRotaryWing.Checked = mModel.RotaryWing
                '---------------
                'Changed by Vikrant on 22-July-2011
                MarkLog(Util.Action.Edit, "Model", mModel.Name, Util.ErrorType.NoError, mModel.ID, EventLogID)
                If Len(mModel.Name) > 15 Then
                    lblTitle.Text = "Model Information [" & mModel.Name.Substring(0, 15) & "...]"
                Else
                    lblTitle.Text = "Model Information [" & mModel.Name & "]"
                End If
                If cmbManufacturerList.Enabled = True Then
                    setFocus(cmbManufacturerList)
                End If
                If cmbForAssemblyList.SelectedIndex = 0 Then
                    PrimaryModelPlaceHolder.Visible = True
                Else
                    PrimaryModelPlaceHolder.Visible = False
                End If


                DisableName(mId)
            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgModel.PageIndex * dgModel.PageSize
                mId = mModelList(Idx).ID
                If Type = False And Session("FromCopyUtility") <> "True" Then
                    ''If (Not User.IsInRole("MachineDelete")) Then
                    If Session("IsForSpareAssembly") = False Then
                        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                            setObject()
                            SetSession()
                            'Changed by Vikrant on 22-July-2011
                            MarkLog(Util.Action.Delete, "Model", User.Identity.Name & " is not Authorized User to delete " & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                            'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                       
                            'msg.Show()
                            'Session("sender") = "Authorization"
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                            Exit Sub
                        End If
                    End If
                End If
                If Type = True And Session("FromCopyUtility") <> "True" Then
                    ''If (Not User.IsInRole("PartDelete")) Then
                    If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
                        setObject()
                        SetSession()
                        'Changed by Vikrant on 22-July-2011
                        MarkLog(Util.Action.Delete, "Model", User.Identity.Name & " is not Authorized User to delete" & mModel.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                        'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                        'msg.ReplacePage = "wfModel.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")                      
                        'msg.Show()
                        'Session("sender") = "Authorization"
                        MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                        Exit Sub
                    End If
                End If
                DeleteRecord(mId)
                'Changed by Vikrant on 22-July-2011
                MarkLog(Util.Action.Delete, "Model", mModel.Name, Util.ErrorType.HandledError, mModel.ID, EventLogID)

        End Select


    End Sub

    Private Sub dgModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgModel.PageIndexChanging
        dgModel.PageIndex = e.NewPageIndex
        dgModel.DataSource = mModelList
        Session("mModelList") = mModelList
        dgModel.DataBind()

    End Sub

    Private Sub dgModel_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModel.Sorting
        mModelList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mModelList") = mModelList
        dgModel.DataSource = mModelList
        dgModel.DataBind()
    End Sub
    Private Sub imgbtnManufacturer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnManufacturer.Click
        ' Response.Redirect("wfManufacturer_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfModel_Ajax.aspx&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
        Session("mAssemblyTypeId") = mAssemblyTypeId
        Session("Type") = Type
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManufacturerWindow", "OpenManufacturerWindow()", True)
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If cmbManufacturerList.Enabled = True Then
            setFocus(cmbManufacturerList)
        End If
        'Changed by Vikrant on 22-July-2011
        MarkLog(Util.Action.[New], "Model", "", Util.ErrorType.NoError, mModel.ID, EventLogID)
        NewRecord()
        cmbManufacturerList.SelectedIndex = 0
        ''''' cmbForAssemblyList.SelectedIndex = 0
        txtName.Text = ""
        'DataFieldBind()
        txtName.Enabled = True
        'Ajay 30-Nov-2022
        'rdbFixedWing.Checked = True
        'rdbRotaryWing.Checked = False
        '---------------
        lblTitle.Text = "Model Information [New]"
    End Sub
    Private Sub imgbtnPrimaryModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnPrimaryModel.Click
        Session("mAssemblyTypeId") = mAssemblyTypeId
        Session("Type") = Type
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPrimaryModelWindow", "OpenPrimaryModelWindow()", True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub htnBtnManufacturer_Click(sender As Object, e As System.EventArgs) Handles htnBtnManufacturer.Click, htnBtnPrimaryModel.Click
        DataFieldBind()
    End Sub

    Private Sub cmbForAssemblyList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbForAssemblyList.SelectedIndexChanged
        Try

            If cmbForAssemblyList.SelectedIndex = 0 Then
                PrimaryModelPlaceHolder.Visible = True
            Else
                PrimaryModelPlaceHolder.Visible = False
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
#End Region

End Class