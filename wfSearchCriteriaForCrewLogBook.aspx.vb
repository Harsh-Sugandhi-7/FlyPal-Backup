Partial Class wfSearchCriteriaForCrewLogBook
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cvAircraft As System.Web.UI.WebControls.CustomValidator


    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar
    Protected WithEvents rfvCrew As System.Web.UI.WebControls.RequiredFieldValidator

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
    Dim mMachineNameValueList As MachineNameValueList
    Dim mAssemblylist As AssemblyList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As String
    Dim AssemblyID As String
    Dim Aircraft As String
    'Dim AssemblyType As String
    'Dim AssemblyText As String
    Dim Model As String
    Dim SerialNo As String
    Dim RegNo As String

    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail

    'Dim objCrewLogRegister As ReportCrewLogRegister
    Dim objCrewLogRegister As ReportCrewLogRegisterNew
    Dim dsLogRegister As New dsLogRegister

    Dim LogType As Integer
    Dim mEmployeeList As EmployeeList
    Dim CrewID As String
    Dim crew As String
    Dim CrewName As String
    Dim AllAircraft As Boolean = False

    Dim mDutyTypeList As DutyTypeList
    Public mDutyAs As String

    Dim CoPilotID As String 'Added By Prashant 18-Jun-2013  ALL18062013
    Dim CoPilot As String 'Added By Prashant 18-Jun-2013  ALL18062013
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mEmployeeList = CType(Session("mPilotList"), EmployeeList)
        mDutyTypeList = CType(Session("mDutyTypeList"), DutyTypeList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForCrewLogBook.aspx" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblylist")
            Session.Remove("mPilotList")
            Session.Remove("mDutyTypeList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDutyAs1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblPilot1.Visible = True
        lblCopilot.Visible = True 'Added By Prashant 18-Jun-2013  ALL18062013
    End Sub
    Private Sub SetValues()
        If Not (txtFromDate.IsDateValue) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Value.ToString
        End If
        If Not (txtToDate.IsDateValue) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Value.ToString
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If cmbAircraft.SelectedIndex > 0 Then
            RegNo = mMachineNameValueList(cmbAircraft.SelectedIndex).RegNo
        End If
        MachineID = cmbAircraft.SelectedValue.ToString
        'CrewID = cmbPilotList.SelectedValue.ToString
        'crew = cmbPilotList.SelectedItem.Text

        CrewID = mEmployeeList.Item(txtSearch.Text.Trim, "").ID.ToString

        CoPilotID = mEmployeeList.Item(txtCoPilot.Text.Trim, "").ID.ToString 'Added By Prashant 18-Jun-2013  ALL18062013
        If txtSearch.Text.Trim = "" Then
            lblPilot1.Text = "Pilot In Command : (All)"
            'CrewName = "Crew Name : (All)"
            CrewName = "Pilot In Command : (All)" 'Added By Prashant 18-Jun-2013  ALL18062013"
        Else
            crew = mEmployeeList(txtSearch.Text.Trim, "").Name
            'CrewName = "Crew Name : " & crew  'Commented By Prashant 18-Jun-2013  ALL18062013
            CrewName = "Pilot In Command : " & crew    'Added By Prashant 18-Jun-2013  ALL18062013"
        End If

        If txtCoPilot.Text.Trim = "" Then
            lblCopilot.Text = "Co-Pilot Name : (All)"
            CoPilot = "Co-Pilot : (All)" 'Added By Prashant 18-Jun-2013  ALL18062013"
        Else
            CoPilot = "Co-Pilot : " & txtCoPilot.Text.Trim    'Added By Prashant 18-Jun-2013  ALL18062013"
        End If

        mDutyAs = IIf(cmbDutyAs.SelectedIndex > 0, "On Duty As: " & cmbDutyAs.SelectedItem.Text, "On Duty As: (All)")
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "(All)")
        lblPilot1.Text = CrewName
        lblDutyAs1.Text = "Duty As : " & cmbDutyAs.SelectedItem.Text
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Value.ToString
        EndDate = txtToDate.Value.ToString
        MachineID = "{00000000-0000-0000-0000-000000000000}"
        Aircraft = ""
        crew = ""
        CrewID = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim serchstr7 As String
        Dim str1 As String = ""
        If chkLogNo.Checked Then
            str1 = "Log No."
        Else
            str1 = ""
        End If
        If chkLogPageNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Log Page No."
        Else
            str1 = str1 + "/" + "Log Page No."
        End If

        If chkFlightNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Flight No."
        Else
            str1 = str1 + "/" + "Flight No."
        End If

        Dim mCheckedDetail As Boolean = True
        'myReport = New crCrewLogRegister
        If optDetail.Checked Then
            myReport = New crCrewLogRegisterDetail
            mCheckedDetail = True
        ElseIf optSummary.Checked Then
            myReport = New crCrewLogRegisterSummary
            mCheckedDetail = False
        End If


        ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "   " + "To" + "   " + New SmartDate(EndDate).FormattedText, , , , _
        cmbAircraft.SelectedItem.Text, , "", "", crew, , , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))

        'objCrewLogRegister = ReportCrewLogRegister.GetCrewLogRegister(StartDate, EndDate, MachineID, , , CrewID, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked, cmbDutyAs.SelectedValue)
        objCrewLogRegister = ReportCrewLogRegisterNew.GetCrewLogRegister(StartDate, EndDate, MachineID, True, , CrewID, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked, cmbDutyAs.SelectedValue, , mCheckedDetail, CoPilotID)
        'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 


        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
            If cmbAircraft.SelectedIndex > 0 Then
                serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
            Else
                serchstr7 = ""
            End If
        Else
            serchstr7 = ""
        End If


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
             mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Crew Log Register", "From Date: " + New SmartDate(StartDate).FormattedText, "To Date: " + New SmartDate(EndDate).FormattedText, "Aircraft: " + cmbAircraft.SelectedItem.Text, CrewName, mDutyAs, AppSettings("Product Version"), AppSettings("SINote"), CoPilot, serchstr7, IIf(cmbAircraft.SelectedIndex > 0, "False", "True"), str1, AppSettings("Logo"))

        If objCrewLogRegister.Count = 0 Then
            ResetValues()
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfSearchCriteriaForCrewLogBook.aspx?Backpage="
            msg1.Show()
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011

        ElseIf objCrewLogRegister.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1141)

            '*******************************
        End If
        da.Fill(dsLogRegister, objCrewLogRegister)
        da.Fill(dsLogRegister, Report)
        da.Fill(dsLogRegister, ReportStatusList)
        Dim mrptImage As rptImage = rptImage.GetImage(dsLogRegister)
        da.Fill(dsLogRegister, mrptImage)
        myReport.SetDataSource(dsLogRegister)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ResetValues()
    End Sub

#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        ''If custValidator.ControlToValidate = "cmbPilotList" Then
        ''    If cmbPilotList.SelectedIndex = 0 Then
        ''        custValidator.ErrorMessage = "Please select the Crew."
        ''        e.IsValid = False
        ''    Else
        ''        e.IsValid = True
        ''    End If
        ''End If
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , 0, 0, "", "", "", True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)", , , False)
        'cmbPilotList.DataSource = mEmployeeList
        Session("mPilotList") = mEmployeeList
        'cmbPilotList.DataBind()
        mDutyTypeList = DutyTypeList.GetDutyTypeList(True, "(All)")
        cmbDutyAs.DataSource = mDutyTypeList
        cmbDutyAs.DataBind()
        Session("mDutyTypeList") = mDutyTypeList
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForCrewLogBook.aspx"
            ResetValues()
            txtFromDate.Value = Now.Date
            txtToDate.Value = Now.Date
            optDetail.Checked = True
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        'If IsValid = True Then
        SetReport()
        'End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        mAssemblylist = Nothing
        mEmployeeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            mAssemblylist = Nothing
            Session("mAssemblylist") = mAssemblylist
        Else
            MachineName = cmbAircraft.SelectedValue.ToString

            'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(Now.ToShortDateString, MachineName, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , , True).Item(0), MachineInfo).AssemblyStatusList
            'Session("mAssemblyStatusList") = mAssemblyStatusList

            Dim mAssemblylist As AssemblyList
            'mAssemblylist = AssemblyList.GetAssemblyList(0, cmbAircraft.SelectedValue, txtFromDate.Value.ToString)
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Value.ToString, , True)
            Session("mAssemblyList") = mAssemblylist
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
    End Sub
    Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
        Me.cmbAircraft.Visible = Not CType(sender, Boolean)
        'Me.cmbPilotList.Visible = Not CType(sender, Boolean)
    End Sub
#End Region


End Class
