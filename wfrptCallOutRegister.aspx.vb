Partial Class wfrptCallOutRegister
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'Added by Saylee om 19-june 2007					
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

#Region " Variable Declarations "
    Public mQcCallOutTextList As NextCalloutText
    Public mCalloutTextList As DistinctTextListForCallout
    Public mQcCallOutList As QCCallOutList
    Public mJobTypeList As JobTypeList
    Public mWoStatusList As WOStatusList
    Public mCustomerList As VendorList
    Dim CallOutText As String = ""
    Dim CallOutNo As Integer
    Dim FromDate As String = ""
    Dim ToDate As String = ""
    Dim RegNo As String = ""
    Dim Model As String = ""
    Dim SerialNo As String = ""
    Dim Supplier As String = ""
    Dim JobType As String = ""
    Dim Status As String = ""
    Dim CompPartNo As String = ""
    Dim CompSerialNo As String = ""
    Dim ReportType As String = ""
    Public mSearchingCriteria As New rptReportSearchingCriteria
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        mQcCallOutList = CType(Session("mQcCallOutList"), QCCallOutList)
        mQcCallOutTextList = CType(Session("mQcCallOutTextList"), NextCalloutText)
        mCalloutTextList = CType(Session("mCalloutTextList"), DistinctTextListForCallout)
        mJobTypeList = CType(Session("mJobTypeList"), JobTypeList)
        mWoStatusList = CType(Session("mWoStatusList"), WOStatusList)
    End Sub
    Private Sub SetSession()
        Session("mQcCallOutList") = mQcCallOutList
        Session("mQcCallOutTextList") = mQcCallOutTextList
        Session("mCalloutTextList") = mCalloutTextList
        Session("mJobTypeList") = mJobTypeList
        Session("mWoStatusList") = mWoStatusList
        Session("mCustomerList") = mCustomerList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mQcCallOutList")
        Session.Remove("mQcCallOutTextList")
        Session.Remove("mCalloutTextList")
        Session.Remove("mJobTypeList")
        Session.Remove("mWoStatusList")
        Session.Remove("mCustomerList")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub addAttributes()
        txtCallOutNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCallOutNo').value)")
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        ' lblToDate.Visible = True
        lblCallOutNo1.Visible = True
        lblReportType1.Visible = True
        lblVendor.Visible = True
        lblRegNo1.Visible = True
        lblModel1.Visible = False
        lblSerialNo1.Visible = False
        lblCompPartNo1.Visible = False
        lblCompSerialNo1.Visible = False
        lblStatus1.Visible = True
        lblJobType1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblCallOutNo1.Visible = False
        lblReportType1.Visible = False
        lblVendor.Visible = False
        lblRegNo1.Visible = False
        lblModel1.Visible = False
        lblSerialNo1.Visible = False
        lblCompPartNo1.Visible = False
        lblCompSerialNo1.Visible = False
        lblStatus1.Visible = False
        lblJobType1.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Value = CDate("01-01-1900")
                txtToDate.Value = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Value = CDate(Today.AddDays(-6))
                txtToDate.Value = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Value = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Value = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Value = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Value = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Value = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Value = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Value = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Value = Today.AddDays(1).AddYears(-1)
                txtToDate.Value = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Value = Today.Date
            Case 6 'Between Dates
                txtFromDate.Value = Today.Date
                txtToDate.Value = Today.Date
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Value.ToString
            ToDate = txtToDate.Value.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If
        Supplier = txtVendor.Text.Trim
        'If cmbCustomer.SelectedIndex = 0 Then
        '    Supplier = ""
        'Else
        '    Supplier = cmbCustomer.SelectedItem.Text
        'End If
        lblVendor.Text = "Customer  : " & Supplier
        CallOutText = IIf(cmbCallOutText.SelectedIndex > 0, Trim(cmbCallOutText.SelectedItem.Text), "")
        CallOutNo = CInt(Val(txtCallOutNo.Text))
        If CallOutText <> "" Then
            If CallOutNo <> 0 Then
                lblCallOutNo1.Text = "CallOut No. : " & CallOutText + "-" + CallOutNo.ToString
            Else
                lblCallOutNo1.Text = "CallOut No. : " & CallOutText
            End If
        Else
            lblCallOutNo1.Text = "CallOut No. : " & "All"
        End If
        RegNo = txtRegNo.Text.Trim
        lblRegNo1.Text = "Reg. No.  :" & RegNo
        Model = txtModel.Text.Trim
        lblModel1.Text = "Model  : " & Model
        SerialNo = txtSerialNo.Text.Trim
        lblSerialNo1.Text = "Serial No.  : " & SerialNo
        CompPartNo = txtComppartNo.Text.Trim
        lblCompPartNo1.Text = "Comp Part No.  : " & CompPartNo
        CompSerialNo = txtCompSerialNo.Text.Trim
        lblCompSerialNo1.Text = "Comp Serial No.  : " & CompSerialNo
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        lblStatus1.Text = "Status :" & IIf(Status <> "", Status, "All")
        JobType = IIf(cmbJobType.SelectedIndex > 0, cmbJobType.SelectedItem.Text, "")
        lblJobType1.Text = "Job Type :" & IIf(JobType <> "", JobType, "All")
        ReportType = cmbReportType.SelectedItem.Text
        lblReportType1.Text = "Report Type  : " & ReportType
    End Sub
    Private Sub ReportobjectSearching()
        mSearchingCriteria.FromDate.Text = FromDate
        mSearchingCriteria.ToDate.Text = ToDate
        mSearchingCriteria.Vendor = Supplier

        mSearchingCriteria.Text = CallOutText
        If CallOutNo = 0 Then
            mSearchingCriteria.ParentID = Guid.Empty
        End If
        mSearchingCriteria.No = CallOutNo
        mSearchingCriteria.RegNo = RegNo
        mSearchingCriteria.ReportNo = "QC Callout No. :"

        If cmbStatus.SelectedIndex = 0 Then
            mSearchingCriteria.StatusID = 0
        Else
            mSearchingCriteria.StatusID = CInt(Val(cmbStatus.SelectedValue))
            mSearchingCriteria.StatusName = cmbStatus.SelectedItem.Text
        End If
        mSearchingCriteria.Vendor = Supplier
        mSearchingCriteria.JobName = JobType
        If cmbJobType.SelectedIndex = 0 Then
            mSearchingCriteria.JobTypeID = 0
        Else
            mSearchingCriteria.JobTypeID = CInt(Val(cmbJobType.SelectedValue))
        End If
        mSearchingCriteria.MachineModelNo = Model
        'mSearchingCriteria.MachineSerialNo = SerialNo
        mSearchingCriteria.CompPartNo = CompPartNo
        mSearchingCriteria.CompSerialNo = CompSerialNo
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        Supplier = ""
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCallOut
        Dim rpt As rptCalloutList
        Dim rptChild As rptCalloutJobList
        Dim mCompanyDetail As New CompanyDetail
        Dim String1 As String
        SetValues()
        ReportobjectSearching()
        mSearchingCriteria.SetLines()
        If (cmbReportType.SelectedIndex = 0) Then
            String1 = "CallOut Register (Detail Report)"  'New Addition on 16/1/2007
        ElseIf (cmbReportType.SelectedIndex = 1) Then
            String1 = "CallOut Register (Summary Report)" 'New Addition on 16/1/2007
        ElseIf (cmbReportType.SelectedIndex = 2) Then
            String1 = "CallOut Register (Detail Report)"  'New Addition on 16/1/2007
        ElseIf (cmbReportType.SelectedIndex = 3) Then
            String1 = "CallOut Register (Summary Report)" 'New Addition on 16/1/2007
        End If

        Dim mReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
                 mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, String1, "", "", "", "", "", AppSettings("ProductVersion"), _
                AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        rpt = rptCalloutList.GetCalloutList(mSearchingCriteria)
        rptChild = rptCalloutJobList.GetrptCalloutJobList(mSearchingCriteria)
        If rpt.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfrptCallOutRegister.aspx?Backpage="
            msg1.Show()
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011

        ElseIf rpt.Count > 0 Then
            
           RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 722)

            '*******************************
        End If
        ds.Clear()
        da.Fill(ds, rpt)
        da.Fill(ds, rptChild)
        da.Fill(ds, mSearchingCriteria)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mReportData)
        '************************Report Show ***************************
        If (cmbReportType.SelectedIndex = 0) Then
            myReport = New crCallOutRegDetailP
        ElseIf (cmbReportType.SelectedIndex = 1) Then
            myReport = New crCallOutRegSummaryP
        ElseIf (cmbReportType.SelectedIndex = 2) Then
            myReport = New crCallOutRegDetailL
        ElseIf (cmbReportType.SelectedIndex = 3) Then
            myReport = New crCallOutRegSummaryL
        End If
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCalloutTextList = DistinctTextListForCallout.GetDistinctTextList(1, False, )
        cmbCallOutText.DataSource = mCalloutTextList
        Session("mCalloutTextList") = mCalloutTextList
        'Job Type List
        mJobTypeList = JobTypeList.GetJobTypeList("<SELECT>")
        cmbJobType.DataSource = mJobTypeList
        Session("mJobTypeList") = mJobTypeList
        'Status List
        mWoStatusList = WOStatusList.GetWOStatusList(0, 1, True)
        cmbStatus.DataSource = mWoStatusList
        'Customer
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, False, False)
        cmbCustomer.DataSource = mCustomerList
        Session("mCustomerList") = mCustomerList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                SetFocus(cmbDateRange)
            End If
            DataFieldBind()
            ControlVisibility(2)
            setDatePeroid(2)
            cmbDateRange.SelectedIndex = 2
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbCallOutText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCallOutText.SelectedIndexChanged
        txtCallOutNo.Text = ""
        txtCallOutNo.Visible = IIf(cmbCallOutText.SelectedIndex > 0, True, False)
        If cmbCallOutText.Enabled = True Then
            SetFocus(cmbCallOutText)
        End If
    End Sub
    Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
        Me.cmbReportType.Visible = Not CType(sender, Boolean)
        Me.cmbJobType.Visible = Not CType(sender, Boolean)
        Me.cmbCustomer.Visible = Not CType(sender, Boolean)
    End Sub
    Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
        Me.cmbReportType.Visible = Not CType(sender, Boolean)
        Me.cmbJobType.Visible = Not CType(sender, Boolean)
        Me.cmbCustomer.Visible = Not CType(sender, Boolean)
    End Sub
#End Region
End Class
