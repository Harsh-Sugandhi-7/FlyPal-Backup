
Partial Class wfMachineParameterList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnContactInfo As System.Web.UI.WebControls.Button
    Protected WithEvents btnBankInfo As System.Web.UI.WebControls.Button
    Protected WithEvents btnTaxInfo As System.Web.UI.WebControls.Button
    
    

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
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mParameterList = CType(Session("mParameterList"), ParameterList)
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mParameterList") = mParameterList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mParameterList")
    End Sub
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
                            mMachine.MachineParameters.Remove(mMachine.MachineParameters(mMachine.MachineParameters.CurrentIndex))
                            Session("mMachine") = mMachine
                            Response.Redirect("wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                                'MarkLog(Util.Action.Delete, "Machine", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mMachine.ID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Machine", " Aircraft Name ->" + mMachine.RegNo + " Parameter Name -> " + mMachine.MachineParameters.Item(mParameterList.CurrentIndex).ParameterName, Util.ErrorType.NoError, mMachine.MachineParameters.Item(mParameterList.CurrentIndex).ParameterID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfMachineParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfMachineParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfMachineParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfMachineParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '   DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        If mMachine.IsNew Then
            lblTitle.Text = "Aircraft [New]"
        Else
            lblTitle.Text = "Aircraft [" & mMachine.RegNo & "]"
        End If
        lblResult.Text = "List of Parameters: " & mMachine.MachineParameters.Count & " Record(s)found"
    End Sub
    Private Sub ControlVisibility()
        'enabledisable buttons
        '  btnAdd.Enabled = Not mMachine.AssemblyStatus.HasLogCount   'Had Commented this line
        dgParameterList.Columns(3).Visible = Not mMachine.AssemblyStatus.HasLogCount
    End Sub
#End Region

#Region " Data Binding "
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbParameterList" Then
            If cmbParameterList.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Select Parameters form List."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        mParameterList = ParameterList.GetParameterList("<SELECT>")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        dgParameterList.DataSource = mMachine.MachineParameters
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
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
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            'MarkLog(Util.Action.[New], "Machine", "", Util.ErrorType.NoError, mMachine.ID)
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMachineParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
            Session("sender") = "Authorization"
            msg.Show()
            Exit Sub
        End If
        If Not IsValid Then Exit Sub
        Dim ParameterID As New Guid(cmbParameterList.SelectedValue.ToString)
        If mMachine.MachineParameters.Contains(ParameterID, "") = False Then
            'MarkLog(Util.Action.[New], "Machine", " Parameter ->  " + cmbParameterList.SelectedItem.Text, Util.ErrorType.NoError, ParameterID)
            mMachine.MachineParameters.Add(mMachine.ID, ParameterID)
            Session("mMachine") = mMachine
            Response.Redirect("wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Else
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Parameter already exists, can not be added.", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
            Session("sender") = "Delete"
            msg.Show()
        End If
    End Sub
    Private Sub dgParameterList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgParameterList.ItemCommand
        Dim Index As Int32 = dgParameterList.CurrentPageIndex * dgParameterList.PageSize + e.Item.ItemIndex
        Select Case e.CommandName
            Case "Delete"
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    'MarkLog(Util.Action.Delete, "Machine", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMachineCertificateList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    msg1.Show()
                    Exit Sub
                End If
                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                msg.ReplacePage = "wfMachineParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                Session("sender") = "Delete"
                msg.Show()
                mMachine.MachineParameters.CurrentIndex = Index
                Session("mMachine") = mMachine
        End Select
    End Sub
    Private Sub btnAirCraftStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAirCraftStatus.Click
        SetSession()
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnAssemblyList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssemblyList.Click
        SetSession()
        Response.Redirect("wfAssemblyStatusList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub imgbtnParameter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnParameter.Click
        NewRecord()
        Response.Redirect("wfParameter.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMachineParameterList.aspx")
    End Sub
    Private Sub btnTankList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTankList.Click
        SetSession()
        Response.Redirect("wfMachineTankList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub btnFeatureList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFeatureList.Click
        SetSession()
        Response.Redirect("wfMachineFeatureList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub btnCertificateList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCertificateList.Click
        SetSession()
        Response.Redirect("wfMachineCertificateList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub btnMELList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMELList.Click
        SetSession()
        Response.Redirect("wfMEL.aspx?ChildPage=wfMEL.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub btnBoardInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBoardInfo.Click
        SetSession()
        'Added by Saylee on 14-July-2009 for User Rights
        If (Not User.IsInRole("AircraftInformationBoardNew")) Then ' Or (Not User.IsInRole("AircraftInformationBoardEdit")) Then
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMachineParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
            Session("sender") = "Authorization"
            msg.Show()
            Exit Sub
        End If
        '****************************s
        Response.Redirect("wfBoardInformation.aspx?ChildPage=wfMachine.aspx&BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        'MarkLog(Util.Action.Close, "Machine", "", Util.ErrorType.NoError, Guid.Empty)
        RemoveSession()
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    'Added By Prashant 19-June-2009 for grid sorting
    Private Sub dgParameterList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgParameterList.SortCommand
        mMachine.MachineParameters.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mParameterList") = mParameterList
        dgParameterList.DataSource = mMachine.MachineParameters
        dgParameterList.DataBind()
    End Sub
    '-----------------------------------------------
#End Region

#Region " Report "
    'Created By :- Jyoti
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

    '#Region "Event"
    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

    'If (Not User.IsInRole("MachinePrint")) Then
    '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '            msg.ReplacePage = "wfMachineParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    '          msg.Show()
    '            Exit Sub
    '        End If

    '    Rpt = New crListAssemblyStatus
    '    Dim da As New CSLA.Data.ObjectAdapter
    '    Dim ds As New dsCommon
    '    Dim ReportDetails As New rptStatusList

    '    'For Detail Section
    '    ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Reg No.", _
    '       Me.mMachine.RegNo, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft" + "  " + Me.mMachine.AssemblyStatus.AsOnDate, _
    '       "Periods", "Value"))

    '    Dim TotalCount As Integer
    '    TotalCount = Me.mMachine.AssemblyStatus.AssemblyStatusPeriods.Count
    '    Dim I As Integer

    '    For I = 0 To TotalCount - 1
    '        If I = 0 Then
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Manufacturer", _
    '                   Me.mMachine.Owner, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                   CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        ElseIf I = 1 Then
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Model", _
    '                                      Me.mMachine.AssemblyStatus.Assembly.ModelName, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                                      CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        ElseIf I = 2 Then
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Serial No", _
    '                                  Me.mMachine.AssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                                  CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        Else
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "", _
    '                                   "", , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                                   CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        End If
    '    Next

    '    'For Assembly List Caption
    '    ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , , , lblAssemblyListInfo.Text))


    '    'For Assembly Status List
    '    ReportDetails.Add(New rptStatus(, 2, , , _
    '   , , dgAssemblyStatusList.Columns.Item(1).HeaderText, , dgAssemblyStatusList.Columns.Item(2).HeaderText, dgAssemblyStatusList.Columns.Item(3).HeaderText, _
    '   dgAssemblyStatusList.Columns.Item(4).HeaderText, dgAssemblyStatusList.Columns.Item(5).HeaderText, _
    '    dgAssemblyStatusList.Columns.Item(6).HeaderText, dgAssemblyStatusList.Columns.Item(7).HeaderText, dgAssemblyStatusList.Columns.Item(8).HeaderText, _
    '    dgAssemblyStatusList.Columns.Item(9).HeaderText))

    '    Dim TotalCount1 As Integer
    '    TotalCount1 = Me.mAssemblyStatusList.Count
    '    Dim m As Integer
    '    Dim str(8) As String
    '    For m = 0 To TotalCount1 - 1
    '        str(0) = ""
    '        str(1) = ""
    '        str(2) = ""
    '        str(3) = ""
    '        str(4) = ""
    '        str(5) = ""
    '        str(6) = ""
    '        str(7) = ""
    '        str(8) = ""
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgAssemblyStatusList.Items(m).Cells.Item(1).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgAssemblyStatusList.Items(m).Cells.Item(2).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgAssemblyStatusList.Items(m).Cells.Item(3).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgAssemblyStatusList.Items(m).Cells.Item(4).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgAssemblyStatusList.Items(m).Cells.Item(5).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgAssemblyStatusList.Items(m).Cells.Item(6).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgAssemblyStatusList.Items(m).Cells.Item(7).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgAssemblyStatusList.Items(m).Cells.Item(8).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgAssemblyStatusList.Items(m).Cells.Item(9).Text

    '        ReportDetails.Add(New rptStatus(, 3, , , , , str(0), , _
    '                      str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8)))
    '    Next

    '    mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
    '    Dim Report As New ReportData(mCompanyDetail.CompanyName, _
    '    mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
    '    mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
    '    " Assembly List Report", "All the Assembly data is as on " & Me.mMachine.AssemblyStatus.AsOnDate, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))
    '    da.Fill(ds, ReportDetails)
    '    da.Fill(ds, Report)
    '    Rpt.SetDataSource(ds)
    '    Session("CrystalReport") = Rpt

    '    Dim Str1 As String
    '    Str1 = "<script language=Javascript>openTranDetail();</script>"
    '     ClientScript.RegisterStartupScript(Me.GetType(),"openTranDetail", Str1)
    'End Sub
    '#End Region
#End Region

End Class
