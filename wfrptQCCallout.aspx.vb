Partial Class wfrptQCCallout
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
    Public mVendor As Vendor
    ''Public mQcCallOutTextList As DistinctTextListForQC
    Public mSupplierList As VendorList
    Public mQcCallOutList As QCCallOutList
    ''  Private mSearchingCriteria As New rptReportSearchingCriteria
    Public mMachine As Machine
    Public mtmpMachineList As tmpMachineList
    Public mArrivalPlaceList As PlaceList
    Public mDepartPlaceList As PlaceList
    Public mJobTypeList As JobTypeList
    Public mQcCallStatusList As QCStatusList

    Public FromDate As String = ""
    Public ToDate As String = ""
    Public Supplier As String = ""
    Public ReportType As String = ""
    Public QCCallText As String = ""
    Public QCCallNo As String = ""
    Public RegNo As String = ""
    Public Model As String = ""
    Public Arrival As String = ""
    Public Depart As String = ""
    Public JobType As String = ""
    Public Status As String = ""
#End Region

#Region " Business Proerties and Methods "
    Private Sub GetSession()
        mSupplierList = CType(Session("mSupplierList"), VendorList)
        mtmpMachineList = CType(Session("mtmpMachineList"), tmpMachineList)
        mArrivalPlaceList = CType(Session("mArrivalPlaceList"), PlaceList)
        mDepartPlaceList = CType(Session("mDepartPlaceList"), PlaceList)
        mJobTypeList = CType(Session("mJobTypeList"), JobTypeList)
        mQcCallStatusList = CType(Session("mQcCallStatusList"), QCStatusList)
    End Sub
    Private Sub SetSession()
        Session("mSupplierList") = mSupplierList
        Session("mtmpMachineList") = mtmpMachineList
        Session("mArrivalPlaceList") = mArrivalPlaceList
        Session("mDepartPlaceList") = mDepartPlaceList
        Session("mJobTypeList") = mJobTypeList
        Session("mQcCallStatusList") = mQcCallStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSupplierList")
        Session.Remove("mtmpMachineList")
        Session.Remove("mArrivalPlaceList")
        Session.Remove("mDepartPlaceList")
        Session.Remove("mJobTypeList")
        Session.Remove("mQcCallStatusList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)
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
        lblVendor1.Visible = True
        lblReportType1.Visible = True
        lblType1.Visible = True
        lblRegNo1.Visible = True
        lblArrival1.Visible = True
        lblDeparture1.Visible = True
        lblJobType1.Visible = True
        lblStatus1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor1.Visible = False
        lblReportType1.Visible = False
        lblType1.Visible = False
        lblRegNo1.Visible = True
        lblArrival1.Visible = False
        lblDeparture1.Visible = False
        lblJobType1.Visible = False
        lblStatus1.Visible = False
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
        If cmbVendor.SelectedIndex = 0 Then
            Supplier = ""
            lblVendor1.Text = "Vendor : All"
        Else
            mVendor = Vendor.GetVendor(New Guid(cmbVendor.SelectedValue))
            Supplier = mVendor.Name.Trim
            lblVendor1.Text = "Vendor :  " & Supplier
        End If
        QCCallText = IIf(cmbQcCalloutText.SelectedIndex > 0, Trim(cmbQcCalloutText.SelectedItem.Text), "")
        QCCallNo = txtQcCalloutNo.Text
        RegNo = IIf(cmbRegNo.SelectedIndex > 0, Trim(cmbRegNo.SelectedItem.Text), "")
        Model = txtModel.Text.Trim
        Arrival = IIf(cmbArrivalAt.SelectedIndex > 0, Trim(cmbArrivalAt.SelectedItem.Text), "")
        Depart = IIf(cmbDepartureFrom.SelectedIndex > 0, Trim(cmbDepartureFrom.SelectedItem.Text), "")
        JobType = IIf(cmbJobType.SelectedIndex > 0, Trim(cmbJobType.SelectedItem.Text), "")
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")

        lblType1.Text = "QCCall No.: " & IIf(QCCallText + QCCallNo <> "", QCCallText + "-" + QCCallNo, "All")
        lblRegNo1.Text = "Registration No. : " & IIf(RegNo + Model <> "", RegNo + "-" + Model, "All")
        lblArrival1.Text = "Arrival " & IIf(Arrival <> "", Arrival, "All")
        lblDeparture1.Text = "Departure " & IIf(Depart <> "", Depart, "All")
        lblJobType1.Text = "Job Type :" & IIf(JobType <> "", JobType, "All")
        lblStatus1.Text = "Status :" & IIf(Status <> "", Status, "All")
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        Supplier = ""
    End Sub
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        mSupplierList = VendorList.GetVendortList(0, , , , , , True)
        cmbVendor.DataSource = mSupplierList
        Session("mSupplierList") = mSupplierList
        '' mQcCallOutTextList = DistinctTextListForQC.GetDistinctTextList(1, , "<SELECT>")
        '' cmbQcCalloutText.DataSource = mQcCallOutTextList
        ''   Session("mQcCallOutTextList") = mQcCallOutTextList
        mtmpMachineList = tmpMachineList.GetMachineList(, , , , , "<SELECT>")
        cmbRegNo.DataSource = mtmpMachineList
        Session("mtmpMachineList") = mtmpMachineList
        mArrivalPlaceList = PlaceList.GetPlaceList(, , "<SELECT>")
        cmbArrivalAt.DataSource = mArrivalPlaceList
        Session("mArrivalPlaceList") = mArrivalPlaceList
        mDepartPlaceList = PlaceList.GetPlaceList(, , "<SELECT>")
        cmbDepartureFrom.DataSource = mDepartPlaceList
        Session("mDepartPlaceList") = mDepartPlaceList
        mJobTypeList = JobTypeList.GetJobTypeList("<SELECT>")
        cmbJobType.DataSource = mJobTypeList
        Session("mJobTypeList") = mJobTypeList
        mQcCallStatusList = QCStatusList.GetQCStatusList(0, 1, True)
        cmbStatus.DataSource = mQcCallOutList
        Session("mQcCallOutList") = mQcCallOutList
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
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
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbQcCalloutText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQcCalloutText.SelectedIndexChanged
        txtQcCalloutNo.Text = ""
        txtQcCalloutNo.Visible = IIf(cmbQcCalloutText.SelectedIndex > 0, True, False)
        If cmbQcCalloutText.Enabled = True Then
            SetFocus(cmbQcCalloutText)
        End If
    End Sub
    Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
        Me.cmbReportType.Visible = Not CType(sender, Boolean)

    End Sub
    Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
        Me.cmbReportType.Visible = Not CType(sender, Boolean)

    End Sub
#End Region

End Class
