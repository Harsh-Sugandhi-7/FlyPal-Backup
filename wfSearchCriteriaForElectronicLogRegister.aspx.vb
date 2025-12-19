Partial Class wfSearchCriteriaForElectronicLogRegister
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
    Dim ReportStatusList As New rptStatusList
    Dim mAssemblyList As ReportFillAssemblyStatus
    Dim mMachineList As MachineList
    Dim mAssemblyStatusList As AssemblyStatusList
    Dim StartDate As String
    Dim EndDate As String
    Dim Engine As String
    Dim MachineName As String
    Dim MachineID As String
    Dim Aircraft As String
    Dim AssemblyID As String
    Dim Model As String
    Dim SerialNo As String
    Dim RegNo As String
    Dim Company As String
    Dim AssemblyType As String
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail
    Dim dsFuelOil As New dsFuelOilRegister
    Dim objFuelOil As ReportFuelandOilRegister
    Dim da As New CSLA.Data.ObjectAdapter

#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mAssemblyStatusList") = mAssemblyStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mMachineList")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblAssembly1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
    End Sub
    Public Sub SetComboOfAssembly(ByVal MachineName As String)
        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(Now.ToShortDateString, MachineName, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , , True).Item(0), MachineInfo).AssemblyStatusList
        cmbAssembly.DataSource = mAssemblyStatusList
        cmbAssembly.SelectedIndex = 0
        lblAssembly1.Text = "Assembly : " & cmbAssembly.SelectedItem.Text
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text.Trim) Then
            StartDate = ""
        Else
            StartDate = CDate(txtFromDate.Text.Trim)
        End If
        If Not IsDate(txtToDate.Text.Trim) Then
            EndDate = ""
        Else
            EndDate = CDate(txtToDate.Text.Trim)
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If cmbAircraft.SelectedIndex > 0 Then
            Engine = IIf(cmbAssembly.SelectedIndex > -1, cmbAssembly.SelectedItem.Text, "")
        Else
            Engine = ""
        End If
        'lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", StartDate, "")
        'lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", EndDate, "")
        If StartDate <> "" Then
            lblDateRangeFrom.Text = "From Date : " & New SmartDate(StartDate).FormattedText
        Else
            lblDateRangeFrom.Text = "From Date : "
        End If
        If EndDate <> "" Then
            lblDateRangeTo.Text = "To Date : " & New SmartDate(EndDate).FormattedText
        Else
            lblDateRangeTo.Text = "To Date : "
        End If
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblAssembly1.Text = "Assembly : " & IIf(Engine <> "", Engine, "")

        MachineID = cmbAircraft.SelectedValue.ToString
        AssemblyID = cmbAssembly.SelectedValue.ToString
        AssemblyType = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyType
        SerialNo = mAssemblyStatusList(cmbAssembly.SelectedIndex).SerialNo
        Model = mAssemblyStatusList(cmbAssembly.SelectedIndex).Model
        RegNo = mMachineList(cmbAircraft.SelectedIndex).RegNo
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text
        MachineID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyType = ""
        SerialNo = ""
        Model = ""
        RegNo = ""
        Engine = ""
        Aircraft = ""
    End Sub
    Private Sub SetReport()
        'Dim objLogRegister As New ReportLogRegister
        'Dim objLogDetail As AssemblyLogDifferncePeriodList
        'Dim RptLogRegister As New crLogRegister
        'Dim dsLogRegister As New dsLogRegister

        ' myReport = New crLogRegister
        Dim objHistoryCumLogRegister As New ReportHistoryCumLogRegister
        Dim objLogDetail As AssemblyLogDifferncePeriodList
        Dim RptHistoryCumLogRegister As New crHistoryCumLogRegister
        Dim dsHistoryCumLogRegister As New dsHistoryCumLogRegister
        myReport = New crHistoryCumLogRegister
        SetValues()
        objLogDetail = AssemblyLogDifferncePeriodList.GetAssemblyLogDifferencePeriodList(StartDate, EndDate, New Guid(AssemblyID), True)
        'objHistoryCumLogRegister = ReportHistoryCumLogRegister.GetHistoryCumLogRegister(StartDate, EndDate, "", _
        '     "Airframe", Model, SerialNo, "", "", "", "", MachineID, True, True, True, False, True, AssemblyID)
        objHistoryCumLogRegister = ReportHistoryCumLogRegister.GetHistoryCumLogRegister(StartDate, EndDate, "", _
             "Airframe", Model, SerialNo, "", "", "", "", MachineID, True, True, True, True, True, AssemblyID)
        ReportStatusList.Add(New rptStatus(, 0, StartDate + " " + "To" + " " + EndDate, _
            AssemblyType + " " + "Details", , , RegNo, , Model, SerialNo, , _
            , , , _
            , , , _
            , , , , "Period", "Before" + " " + StartDate, , "Total Diff.", , _
            "After" + " " + EndDate))
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Electronic Log Book of" + " " + AssemblyType, "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))
        If objHistoryCumLogRegister.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfSearchCriteriaForElectronicLogRegister.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If
        'objLogDetail = objLogDetail.GetAssemblyLogDifferencePeriodList(StartDate, EndDate, New Guid(AssemblyID), True)
        'objLogRegister = ReportLogRegister.GetLogRegister(StartDate, EndDate, AssemblyID, MachineID)
        'ReportStatusList.Add(New rptStatus(, 0, StartDate + " " + "To" + " " + EndDate, AssemblyType + " " + "Details", , , _
        'RegNo, , Model, SerialNo, , , , , , , , , , , , "Period", "Before" + " " + StartDate, , "Total Diff.", , "After" + " " + EndDate))

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        '     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, "Electronic Log Register of" + " " + AssemblyType, "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))
        'If objLogRegister.Count = 0 Then
        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfSearchCriteriaForElectronicLogRegister.aspx?Backpage="
        '    msg1.Show()
        '    Exit Sub
        'End If
        da.Fill(dsHistoryCumLogRegister, objLogDetail)
        da.Fill(dsHistoryCumLogRegister, objHistoryCumLogRegister)
        da.Fill(dsHistoryCumLogRegister, Report)
        da.Fill(dsHistoryCumLogRegister, ReportStatusList)
        RptHistoryCumLogRegister.SetDataSource(dsHistoryCumLogRegister)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    Response.Redirect("wfSearchCriteriaForElectronicLogRegister.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfSearchCriteriaForElectronicLogRegister.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        mMachineList = MachineList.GetMachineListMonitoringStatus(Now.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(Select)")
        cmbAircraft.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        cmbAircraft.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            Session("MiddleFrame") = "wfSearchCriteriaForFuelAndOil.aspx?"
            ResetValues()
            txtFromDate.Text = Now.Date
            txtToDate.Text = Now.Date
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            SetFocus(txtFromDate)
            DataFieldBind()
        End If
        MessageBoxResult()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True
            MachineName = cmbAircraft.SelectedValue.ToString
            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(Now.ToShortDateString, MachineName, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , , True).Item(0), MachineInfo).AssemblyStatusList
            cmbAssembly.DataSource = mAssemblyStatusList
            Session("mAssemblyStatusList") = mAssemblyStatusList
            cmbAssembly.DataBind()
        End If
    End Sub
#End Region

End Class
