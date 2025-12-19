Partial Class wfrptLogParameterGraphList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar
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
    Public mtmpMachineList As tmpMachineList
    Public mMachine As Machine
    Public mParameter As Parameter
    Public mParameterList As ParameterList
    Dim FromDate As String = "1-1-1900"
    Dim ToDate As String = "1-1-2200"
    Dim Aircraft As String = ""
    Dim Parameter1 As String = ""
    Dim Description As String = ""
    Dim AircraftIndex As Integer
    Dim MachineName As String
    Dim AssemblyName As String
    Dim AssemblyType As String
    Dim Assembly1 As String
    Dim mAssemblyList As AssemblyList
    Public mAssemblyParameterList As AssemblyParameterList
    Dim Count As Integer = 0
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mtmpMachineList = CType(Session("mtmpMachineList"), tmpMachineList)
        mParameterList = CType(Session("mParameterList"), ParameterList)
        mMachine = CType(Session("mMachine"), Machine)
        mParameter = CType(Session("mParameter"), Parameter)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mAssemblyParameterList = CType(Session("mAssemblyParameterList"), AssemblyParameterList)

    End Sub
    Private Sub SetSession()
        Session("mtmpMachineList") = mtmpMachineList
        Session("mParameterList") = mParameterList
        Session("mMachine") = mMachine
        Session("mParameter") = mParameter
        Session("mAssemblyList") = mAssemblyList
        Session("mAssemblyParameterList") = mAssemblyParameterList

    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mtmpMachineList")
        Session.Remove("mParameterList")
        Session.Remove("mMachine")
        Session.Remove("mParameter")
        Session.Remove("mAssemblyList")
        Session.Remove("mAssemblyParameterList")

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub PageInitialization()
        txtFromDate.Value = Today.Date
        txtToDate.Value = Today.Date
    End Sub
    Private Sub ResetValues()
        ToDate = Format(CDate(Today.Date).Year, "")
    End Sub
    Private Sub SetValues()
        If txtToDate.Value.ToString = "" Or txtFromDate.Value.ToString = "" Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            ToDate = txtToDate.Value.ToString
            FromDate = txtFromDate.Value.ToString
            lblDateRangeFrom.Text = "From Date : " & New SmartDate(txtFromDate.Value.ToString).FormattedText & " To Date : " & New SmartDate(txtToDate.Value.ToString).FormattedText
        End If

        If cmbAircraft.SelectedIndex = 0 Then       'Aircraft
            Aircraft = ""
            lblAircraft.Text = "Aircraft : All"
        Else
            Aircraft = Machine.GetMachine(New Guid(cmbAircraft.SelectedValue)).Name
            lblAircraft.Text = "Aircraft : " & Aircraft
        End If

        'Assembly
        If cmbAircraft.SelectedItem.Text = "(SELECT)" Then
            Aircraft = ""
        Else
            If cmbAssembly.SelectedItem.Text = "(All)" Or cmbAssembly.SelectedItem.Text = "<All>" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : All"          'Added Code
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
            End If

            MachineName = cmbAircraft.SelectedValue.ToString
            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft.Text = "Aircraft Name : " & Aircraft
        End If

        'Parameter
        Parameter1 = Parameter.GetParameter(New Guid(cmbParameter.SelectedValue.ToString), New Guid(cmbAssembly.SelectedValue.ToString).ToString).Name
        Description = Parameter.GetParameter(New Guid(cmbParameter.SelectedValue.ToString), New Guid(cmbAssembly.SelectedValue.ToString).ToString).Description
        lblDesc.Text = "        Description : " & Description
        lblParameter.Text = "Parameter : " & Parameter1
        lblMinValue.Text = "Min : " & txtMin.Text
        lblMaxValue.Text = "Max : " & txtMax.Text


        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("Aircraft") = Aircraft
        Session("Parameter1") = Parameter1
        Session("Description") = Description
    End Sub
    Private Sub ControlVisibility()
        lblSummary.Visible = False
        lblDateRangeFrom.Visible = False
        lblAircraft.Visible = False
        lblParameter.Visible = False
        lblDesc.Visible = False
        lblAssembly1.Visible = False
        lblMinValue.Visible = False
        lblMaxValue.Visible = False
    End Sub
    Private Sub ControlVisibility1()
        lblSummary.Visible = True
        lblDateRangeFrom.Visible = True
        lblAircraft.Visible = True
        lblParameter.Visible = True
        lblDesc.Visible = True
        lblAssembly1.Visible = True
        lblMinValue.Visible = True
        lblMaxValue.Visible = True
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)

        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbParameter" Then
            If (cmbParameter.SelectedIndex < 0) Then
                custValidator.ErrorMessage = "Parameter Required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Aircraft Required."
                e.IsValid = False

            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbAssembly" Then
            If cmbAssembly.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Assembly Required."
                e.IsValid = False
            ElseIf cmbParameter.Items.Count = 0 Then
                custValidator.ErrorMessage = "Select another Assembly."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtMin" Then
            If Val(txtMin.Text) > Val(txtMax.Text) Then
                custValidator.ErrorMessage = "Max value should be greater than min value "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf custValidator.ControlToValidate = "txtMax" Then
            '    If Val(txtMin.Text) > Val(txtMax.Text) Then
            '        custValidator.ErrorMessage = "Min value should be less than max value "
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        End If
    End Sub
    Private Sub SetParameterValues()
        For i As Integer = 0 To cmbParameter.Items.Count - 1
            'mMachine.MachineParameters.Item(i).IsSelect = cmbParameter.Items.Item(i).Selected
            mAssemblyParameterList.Item(i).IsSelect = cmbParameter.Items.Item(i).Selected
            If cmbParameter.Items.Item(i).Selected = True Then
                Count = Count + 1
            End If
        Next
        Session("mAssemblyParameterList") = mAssemblyParameterList
        'If Count > 5 And rdoPortrait.Checked = True Then
        '    Dim msg1 As New SIMsgBox(Page, "<BR>Too many selection", "<BR><BR>Portrait option does not allow more than 5 parameters, use landscape option", "Portrait option does not allow more than 5 parameters, use landscape option.", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfrptLogParameterReport.aspx?"
        '    msg1.Show()
        '    Exit Sub
        'ElseIf Count > 9 And rdoLandScape.Checked = True Then
        '    Dim msg1 As New SIMsgBox(Page, "<BR>Too many selection", "<BR><BR>Landscape option does not allow more than 9 parameters, please break parameters into multiple report prints.", "Landscape option does not allow more than 9 parameters, please break parameters into multiple report prints", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfrptLogParameterReport.aspx?"
        '    msg1.Show()
        '    Exit Sub
        'End If
    End Sub
    Public Sub SetReport()
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim objReg As rptLogParameterGraph

        'Code Added By Deven ------------on 09-04-08
        Dim mCompanyDetail As New CompanyDetail
        '__________________________________________________

        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsLogParameter As New dsLogParameter
        'myReport = New crptLogParameterGraph
        myReport = New crptLogParameterGraphList

        'Code Added By Deven ------------on 09-04-08
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Log Parameter Graph", New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, cmbAircraft.SelectedItem.Text, cmbAssembly.SelectedItem.Text, "", AppSettings("Product Version"), AppSettings("SINote"), Val(txtMin.Text), Val(txtMax.Text))
        '------------------------------------------------
        mAssemblyParameterList = Session("mAssemblyParameterList")

        'objReg = rptLogParameterGraph.GetParameterValue(New Guid("{132E2858-2B28-49B9-9A45-3A676F0B5E2F}"), New Guid("{8885C2BF-49F6-4ABF-BAE9-AB0BB9573839}"), CDate("12/1/2007"), CDate("12/15/2007"))
        objReg = rptLogParameterGraph.GetParameterValue(New Guid(cmbAssembly.SelectedValue.ToString), mParameter.ID, FromDate, ToDate, Val(txtMin.Text), Val(txtMax.Text), mAssemblyParameterList)
        If objReg.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfrptLogParameterGraphList.aspx?Backpage="
            msg1.Show()
            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 914)
        End If

        'Dim objChart As Object
        'Dim objAxis As Object
        'objChart = CType(myReport, Object)

        'objAxis = objChart.Axes(2)

        'If objAxis.Type = 2 Then 'Type 2 = Value
        '    objAxis.MaximumScale = 100
        '    objAxis.MinimumScale = 0
        'End If
        'myReport = objAxis

        dsLogParameter.Clear()
        da.Fill(dsLogParameter, objReg)

        'Code Added By Deven ------------on 09-04-08
        da.Fill(dsLogParameter, Report)
        '__________________________________________________

        myReport.SetDataSource(dsLogParameter)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub
    Private Sub addAttributes()
        txtMin.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtMin').value,event)")
        txtMax.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtMax').value,event)")
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DatafieldBind()
        mtmpMachineList = tmpMachineList.GetMachineList(, , , , , "(SELECT)")
        cmbAircraft.DataSource = mtmpMachineList
        Session("mtmpMachineList") = mtmpMachineList
        cmbAircraft.DataBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            If cmbAircraft.Enabled = True Then
                SetFocus(cmbAircraft)
            End If
            DatafieldBind()
            PageInitialization()
        End If
        ControlVisibility()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Value.ToString, "(SELECT)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()

            ''mMachine = Machine.GetMachine(mtmpMachineList.Item(cmbAircraft.SelectedIndex).ID)
            ''cmbParameter.DataSource = mMachine.MachineParameters
            ''If mMachine.MachineParameters.Count > 0 Then
            ''    cmbParameter.Enabled = True
            ''    cmbParameter.DataBind()
            ''    If Not mMachine.MachineParameters(cmbParameter.SelectedIndex) Is Nothing Then
            ''        txtDescription.Text = mMachine.MachineParameters(cmbParameter.SelectedIndex).ParameterDescription
            ''    End If
            ''Else
            ''    cmbParameter.Enabled = False
            ''    cmbParameter.Items.Clear()
            ''    txtDescription.Text = ""
            ''End If
            ''Session("mMachine") = mMachine
        End If
        If cmbAircraft.SelectedIndex = 0 Then
            cmbParameter.Enabled = False
            cmbParameter.Items.Clear()
            txtDescription.Text = ""
        End If
        If cmbAircraft.Enabled = True Then
            SetFocus(cmbAircraft)
        End If
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        Dim mAssemblyParameterList As AssemblyParameterList = AssemblyParameterList.GetChildAssemblyParameterList(New Guid(cmbAssembly.SelectedValue.ToString))
        Session("mAssemblyParameterList") = mAssemblyParameterList

        cmbParameter.DataSource = mAssemblyParameterList

        If mAssemblyParameterList.Count > 0 Then
            cmbParameter.Enabled = True
            cmbParameter.DataBind()
            'If Not mAssemblyParameterList(cmbParameter.SelectedIndex) Is Nothing Then
            '    txtDescription.Text = mAssemblyParameterList(cmbParameter.SelectedIndex).ParameterDescription
            '    txtMin.Text = mAssemblyParameterList(cmbParameter.SelectedIndex).MinValue
            '    txtMax.Text = mAssemblyParameterList(cmbParameter.SelectedIndex).MaxValue
            'End If
        Else
            cmbParameter.Enabled = False
            cmbParameter.Items.Clear()
            txtDescription.Text = ""
            txtMin.Text = ""
            txtMax.Text = ""
        End If

        If cmbAssembly.SelectedIndex = 0 Then
            cmbParameter.Enabled = False
            cmbParameter.Items.Clear()
            txtDescription.Text = ""
            txtMin.Text = ""
            txtMax.Text = ""
        End If
        If cmbAssembly.Enabled = True Then
            setFocus(cmbAssembly)
        End If
    End Sub
    Private Sub cmbParameter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mParameter = Parameter.GetParameter(New Guid(cmbParameter.SelectedValue), New Guid(cmbAssembly.SelectedValue.ToString).ToString)
        txtDescription.Text = mParameter.Description
        txtMin.Text = mParameter.MinValue
        txtMax.Text = mParameter.MaxValue
        Session("mParameter") = mParameter
        If cmbParameter.Enabled = True Then
            setFocus(cmbParameter)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid() Then
            ControlVisibility1()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            mParameter = Parameter.GetParameter(New Guid(cmbParameter.SelectedValue), New Guid(cmbAssembly.SelectedValue.ToString).ToString)
            txtDescription.Text = mParameter.Description
            'txtMin.Text = mParameter.MinValue
            'txtMax.Text = mParameter.MaxValue
            Session("mParameter") = mParameter
            SetValues()
            If DateDiff(DateInterval.Day, CDate(txtFromDate.Value), CDate(txtToDate.Value)) > 183 Or DateDiff(DateInterval.Month, CDate(txtToDate.Value), CDate(txtFromDate.Value)) > 183 Then
                Dim msg1 As New SIMsgBox(Page, "Alert!", "Date range should be one month or less than one month.", "", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfrptLogParameterGraphList.aspx?Backpage="
                msg1.Show()
                Exit Sub
            Else
                SetParameterValues()
                SetReport()
            End If

        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
        cmbParameter.Visible = Not CType(sender, Boolean)
        cmbAircraft.Visible = Not CType(sender, Boolean)
        cmbAssembly.Visible = Not CType(sender, Boolean)
        txtToDate.Enabled = Not CType(sender, Boolean)
    End Sub
    Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
        txtFromDate.Enabled = Not CType(sender, Boolean)
        cmbAssembly.Visible = Not CType(sender, Boolean)
    End Sub
    'Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
    '    If txtFromDate.Text <> "" Then
    '        Dim mMonth As Double
    '        Dim mYear As Integer
    '        mMonth = Month(CDate(txtFromDate.Text))
    '        mYear = Year(CDate(txtFromDate.Text))
    '        txtToDate.Value = CDate(txtFromDate.Value).AddDays(-1).AddMonths(1)
    '        'txtToDate.Value = DateDiff(DateInterval.Month, (CDate(txtToDate.Value)), (CDate(txtFromDate.Value)))   'CDate(txtFromDate.Value).AddDays(-1).AddMonths(1).ToShortDateString
    '    Else
    '        txtToDate.Value = Today.Date
    '    End If
    'End Sub
    'Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
    '    If txtToDate.Text <> "" Then
    '        Dim mMonth As Double
    '        Dim mYear As Integer
    '        mMonth = Month(CDate(txtToDate.Text))
    '        mYear = Year(CDate(txtToDate.Text))
    '        txtFromDate.Value = CDate(txtToDate.Value).AddDays(1).AddMonths(-1)
    '        'txtFromDate.Value = DateDiff(DateInterval.Month, (CDate(txtFromDate.Value)), (CDate(txtToDate.Value)))  'CDate(txtToDate.Value).AddDays(1).AddMonths(-1).ToShortDateString
    '    Else
    '        txtFromDate.Value = Today.Date
    '    End If
    'End Sub
#End Region

End Class
