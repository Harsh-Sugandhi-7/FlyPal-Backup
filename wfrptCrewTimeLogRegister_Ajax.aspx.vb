'Ajax Conversion By Vikrant On 30-Jan-2014

Imports System.Linq
Public Class wfrptCrewTimeLogRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList
    'Dim mAssemblylist As AssemblyList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineID As String
    Dim Aircraft As String

    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail

    'Dim objCrewLogRegister As ReportCrewLogRegister
    Dim objCrewLogTimeRegister As CrewLogTimeRegister
    Dim dsCrewTime As New dsCrewTimeLogRegister
    'Dim mEmployeeList As EmployeeList
    Dim CrewID As String
    Dim crew As String
    Dim CrewName As String

    'Dim mDutyTypeList As DutyTypeList
    Public mDutyAs As String

    'Dim CoPilotID As String 'Added By Prashant 18-Jun-2013  ALL18062013
    Dim CoPilot As String 'Added By Prashant 18-Jun-2013  ALL18062013
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        'mEmployeeList = CType(Session("mPilotList"), EmployeeList)
        'mDutyTypeList = CType(Session("mDutyTypeList"), DutyTypeList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptCrewTimeLogRegister_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
            'Session.Remove("mAssemblylist")
            'Session.Remove("mPilotList")
            'Session.Remove("mDutyTypeList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        ' lblDutyAs1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblPilot1.Visible = True
        ' lblCopilot.Visible = True 'Added By Prashant 18-Jun-2013  ALL18062013
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        MachineID = cmbAircraft.SelectedValue.ToString
        'CrewID = cmbPilotList.SelectedValue.ToString
        'crew = cmbPilotList.SelectedItem.Text

        CrewID = IIf(SelectedCrewID.Value.Length > 0, SelectedCrewID.Value, Guid.Empty.ToString) 'CrewID = mEmployeeList.Item(txtSearch.Text.Trim, "").ID.ToString

        'CoPilotID = mEmployeeList.Item(txtCoPilot.Text.Trim, "").ID.ToString 'Added By Prashant 18-Jun-2013  ALL18062013
        If txtSearch.Text.Trim = "" Then
            lblPilot1.Text = "All"
            'CrewName = "Crew Name : (All)"
            CrewName = "All" 'Added By Prashant 18-Jun-2013  ALL18062013"
        Else
            'crew = mEmployeeList(txtSearch.Text.Trim, "").Name
            'CrewName = "Crew Name : " & crew  'Commented By Prashant 18-Jun-2013  ALL18062013
            crew = txtSearch.Text.Trim
            CrewName = crew
        End If

        'If txtCoPilot.Text.Trim = "" Then
        '    lblCopilot.Text = "Co-Pilot Name : (All)"
        '    CoPilot = "Co-Pilot : (All)" 'Added By Prashant 18-Jun-2013  ALL18062013"
        'Else
        '    CoPilot = "Co-Pilot : " & txtCoPilot.Text.Trim    'Added By Prashant 18-Jun-2013  ALL18062013"
        'End If

        '  mDutyAs = IIf(cmbDutyAs.SelectedIndex > 0, "On Duty As: " & cmbDutyAs.SelectedItem.Text, "On Duty As: (All)")
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "(All)")
        lblPilot1.Text = "Crew : " & CrewName
        ' lblDutyAs1.Text = "Duty As : " & cmbDutyAs.SelectedItem.Text

        EventLogDetail = lblDateRangeFrom.Text + "," + lblDateRangeTo.Text + "," + lblAircraft1.Text + "," + lblPilot1.Text
    End Sub
    Private Sub SetReport()
        Dim serchstr7 As String
        SetValues()
        myReport = New crptCrewTimeLogRegister
        objCrewLogTimeRegister = CrewLogTimeRegister.GetCrewLogTimeRegister(StartDate, EndDate, CrewID, MachineID)
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
        mCompanyDetail.WebSite, "Crew Log Time Register", New SmartDate(StartDate).FormattedText, New SmartDate(EndDate).FormattedText, cmbAircraft.SelectedItem.Text, CrewName, mDutyAs, AppSettings("Product Version"), AppSettings("SINote"), CoPilot, serchstr7, IIf(cmbAircraft.SelectedIndex > 0, "False", "True"), "", AppSettings("Logo"))

        If objCrewLogTimeRegister.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objCrewLogTimeRegister.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1279)
        End If
        da.Fill(dsCrewTime, objCrewLogTimeRegister)
        da.Fill(dsCrewTime, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(dsCrewTime)
        da.Fill(dsCrewTime, mrptImage)
        myReport.SetDataSource(dsCrewTime)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "Crew Time Register", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , 0, 0, "", "", "", True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        'mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)", , , False)
        'cmbPilotList.DataSource = mEmployeeList
        'Session("mPilotList") = mEmployeeList
        'cmbPilotList.DataBind()
        'mDutyTypeList = DutyTypeList.GetDutyTypeList(True, "(All)")
        ' cmbDutyAs.DataSource = mDutyTypeList
        ' cmbDutyAs.DataBind()
        'Session("mDutyTypeList") = mDutyTypeList
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptCrewTimeLogRegister_Ajax.aspx"
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))

            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCrewListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mEmpNoNameList As EmpNoNameAutoComplete = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In mEmpNoNameList
             Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In mEmpNoNameList
            Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region
End Class