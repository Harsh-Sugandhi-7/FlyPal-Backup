'Created By : Saylee 
'Date       : 21-Apr-2010

Partial Class wfAssemblyParameterList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mParameterList As ParameterList
    Public mAssemblyStatus As AssemblyStatus
    'Public mAssemblyParameter As AssemblyParameter
    'Public mAssemblyParameterList As AssemblyParameterList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mParameterList = CType(Session("mParameterList"), ParameterList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        ' mAssemblyParameterList = CType(Session("mAssemblyParameterList"), AssemblyParameterList)
        ' mAssemblyParameter = CType(Session("mAssemblyParameter"), AssemblyParameter)
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mParameterList") = mParameterList
        Session("mAssemblyStatus") = mAssemblyStatus
        'Session("mAssemblyParameterList") = mAssemblyParameterList
        'Session("mAssemblyParameter") = mAssemblyParameter
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mParameterList")
    End Sub
    'Added By Vikrant On 25-Jun-2014
    Private Sub RemoveAllSessionValues()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
    End Sub
    'End
    Private Sub NewRecord()
        Dim mParameter As Parameter
        mParameter = Parameter.NewParameter(Guid.NewGuid)
        Session("mParameter") = mParameter
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Session("sender") = ""
                            ''mAssemblyParameter.DeleteAssemblyParameter(mAssemblyParameterList(mAssemblyParameterList.CurrentIndex).ID)
                            ''Session("mAssemblyParameterList") = mAssemblyParameterList
                            mAssemblyStatus.AssemblyParameters.Remove(mAssemblyStatus.AssemblyParameters(mAssemblyStatus.AssemblyParameters.CurrentIndex))
                            dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                            Session("mAssemblyStatus") = mAssemblyStatus
                            dgParameterList.DataBind()
                            Response.Redirect("wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                '    msg1.ReplacePage = "wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                                msg1.ReplacePage = "wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")
                                'MarkLog(Util.Action.Delete, "Machine", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mMachine.ID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Machine", " Aircraft Name ->" + mMachine.RegNo + " Parameter Name -> " + mAssemblyStatus.AssemblyParameters.Item(mParameterList.CurrentIndex).ParameterName, Util.ErrorType.NoError, mAssemblyStatus.AssemblyParameters.Item(mParameterList.CurrentIndex).ParameterID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))

                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '   DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        If mAssemblyStatus.IsNew Then
            lblTitle.Text = mAssemblyStatus.AssemblyTypeName & " Status of " & mMachine.RegNo & " [New]"
        Else
            lblTitle.Text = mAssemblyStatus.AssemblyTypeName & " Status of " & mMachine.RegNo & " [Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        End If
        '  lblResult.Text = "List of Parameters: " & mAssemblyParameterList.Count & " Record(s)found"
        lblResult.Text = "List of Parameters: " & mAssemblyStatus.AssemblyParameters.Count & " Record(s)."

    End Sub
    Private Sub ControlVisibility()
        dgParameterList.Columns(6).Visible = Not mMachine.AssemblyStatus.HasLogCount
    End Sub
    Private Sub addAttributes()
        txtMin.Attributes.Add("onKeyPress", "validateText(('ND'),document.getElementById('txtMin').value,event)")
        txtMax.Attributes.Add("onKeyPress", "validateText(('ND'),document.getElementById('txtMax').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbParameterList" Then
            If cmbParameterList.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Select Parameters from List."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            '    Commented by Shweta
            '------------------
            'ElseIf custValidator.ControlToValidate = "txtMin" Then
            '    If Val(txtMin.Text) > Val(txtMax.Text) Then
            '        custValidator.ErrorMessage = "Max value should be greater than min value "
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
            '-------------------
            'ElseIf custValidator.ControlToValidate = "txtMax" Then
            '    If Val(txtMin.Text) < Val(txtMax.Text) Then
            '        custValidator.ErrorMessage = "Min value should be less than max value "
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        End If
    End Sub
    Private Sub DataFieldBind()
        mParameterList = ParameterList.GetParameterList("<SELECT>")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        ''If mAssemblyParameterList Is Nothing Then
        ''    mAssemblyParameterList = AssemblyParameterList.GetChildAssemblyParameterList(mAssemblyStatus.AssemblyID)
        ''End If

        dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
        Session("mAssemblyStatus") = mAssemblyStatus
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbParameterList.Enabled = True Then
                SetFocus(cmbParameterList)
            End If
            DataFieldBind()
        End If
        SetPage()
        ControlVisibility()
        MessageBoxResult()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not IsValid Then Exit Sub
        Dim ParameterID As New Guid(cmbParameterList.SelectedValue.ToString)
        If Session("mAssemblyParametersEdit") = False Then
            If mAssemblyStatus.AssemblyParameters.Contains(ParameterID, mAssemblyStatus.AssemblyID) = False Then
                'MarkLog(Util.Action.[New], "Assembly", " Parameter ->  " + cmbParameterList.SelectedItem.Text, Util.ErrorType.NoError, ParameterID)
                mAssemblyStatus.AssemblyParameters.Add(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString), Val(txtMin.Text), Val(txtMax.Text))
                dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                dgParameterList.DataBind()
                Session("mAssemblyStatus") = mAssemblyStatus

            Else
                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Parameter already exists, can not be added.", MsgBoxStyle.OKOnly)
                msg.ReplacePage = "wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")

                Session("sender") = "Delete"
                msg.Show()
            End If
        Else
            mAssemblyStatus.AssemblyParameters.CurrentItem.MinValue = Val(txtMin.Text)
            mAssemblyStatus.AssemblyParameters.CurrentItem.MaxValue = Val(txtMax.Text)

            If mAssemblyStatus.AssemblyParameters.CurrentItem.IsDirty Then
                dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                dgParameterList.DataBind()
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mAssemblyParametersEdit") = False
            End If
        End If
        mParameterList = ParameterList.GetParameterList("<SELECT>")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        cmbParameterList.DataBind()
        cmbParameterList.Enabled = True
        txtMin.Text = ""
        txtMax.Text = ""
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)
        mAssemblyStatus.AssemblyParameters.CurrentIndex = Index
        txtMin.Text = mAssemblyStatus.AssemblyParameters.Item(Index).MinValue
        txtMax.Text = mAssemblyStatus.AssemblyParameters.Item(Index).MaxValue
        cmbParameterList.SelectedValue = mAssemblyStatus.AssemblyParameters.Item(Index).ParameterID.ToString
        cmbParameterList.Enabled = False
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub dgParameterList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgParameterList.ItemCommand
        Dim Index As Int32 = dgParameterList.CurrentPageIndex * dgParameterList.PageSize + e.Item.ItemIndex
        Select Case e.CommandName
            Case "Delete"
                'If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                '    MarkLog(Util.Action.Delete, "Machine", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg1.ReplacePage = "wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")
                '    msg1.Show()
                '    Exit Sub
                'End If
                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                msg.ReplacePage = "wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")
                Session("sender") = "Delete"
                msg.Show()
                mAssemblyStatus.AssemblyParameters.CurrentIndex = Index
                Session("mAssemblyStatus") = mAssemblyStatus
            Case "Edit"
                Session("mAssemblyParametersEdit") = True
                EditRecord(Index)
        End Select
    End Sub
    Private Sub imgbtnParameter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnParameter.Click
        NewRecord()
        Session("mAssemblyStatus") = mAssemblyStatus  '$$$$$$$$$
        'Response.Redirect("wfParameter.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfAssemblyParameterList.aspx")
        Response.Redirect("wfParameter.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfAssemblyParameterList.aspx")
    End Sub
    Private Sub btnComponentList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnComponentList.Click
        RemoveSession()
        Response.Redirect("wfAssemblyCompStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub btnServiceList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServiceList.Click
        RemoveSession()
        Response.Redirect("wfAssemblyMonitorServiceStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub btnModificationList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificationList.Click
        RemoveSession()
        Response.Redirect("wfAssemblyMonitorModStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub btnAssemblyDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssemblyDetails.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ''SetSession()
        ''MarkLog(Util.Action.Close, "Assembly", "", Util.ErrorType.NoError, Guid.Empty)
        ''RemoveSession()
        ''Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))

        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))

    End Sub
    Private Sub dgParameterList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgParameterList.SortCommand
        ' mMachine.MachineParameters.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        mAssemblyStatus.AssemblyParameters.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyStatus") = mAssemblyStatus
        dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
        dgParameterList.DataBind()
    End Sub
    '-----------------------------------------------
    'Added By Vikrant On 25-Jun-2014
    Private Sub imgHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHome.Click
        RemoveSession()
        RemoveAllSessionValues()
        Response.Redirect("wfMachine.aspx?BackPage=Index.aspx")
    End Sub
    'End
#End Region


End Class
