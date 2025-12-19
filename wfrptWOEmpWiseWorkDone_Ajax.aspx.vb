Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Public Class wfrptWOEmpWiseWorkDone_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim FromDate As String = ""
    Dim ToDate As String = ""
    Dim RegNo As String = ""
    Dim Model As String = ""
    Dim Employee As String = ""

    Public mEmployeeWiseWorkDoneInWO As EmployeeWiseWorkDoneInWO
    Dim SearchStr1 As String
    Dim SearchStr3 As String
    Dim SearchStr6 As String
    Dim EventLogDetail As String = String.Empty
    Public mWOJobTypeList As nWOJobTypeList
    Public mEmployeeListForCombo As EmployeeListForCombo
    'Added By Abhishek on 10-OCT-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim ds As New dsnWOSummary
    Dim mCompanyDetail As New CompanyDetail
    Dim ReportName As String
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mEmployeeListForCombo = CType(Session("mEmployeeListForCombo"), EmployeeListForCombo)
    End Sub
    Private Sub SetSession()
        Session("mEmployeeListForCombo") = mEmployeeListForCombo
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeListForCombo")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
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
        lblEmployee1.Visible = True
        lblRegNo1.Visible = True
        lblModel1.Visible = True
        lblJobType1.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = CDate(Today.AddDays(1).AddYears(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = New SmartDate("01-01-1900").FormattedText
            ToDate = New SmartDate("01-01-2200").FormattedText
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range     : " & FromDate & " To " & ToDate & " ( " & cmbDateRange.SelectedItem.Text & ")"
        End If

        If cmbEmployee.SelectedIndex = 0 Then
            Employee = ""
        Else
            Employee = cmbEmployee.SelectedItem.Text
        End If
        lblEmployee1.Text = "Employee  : " & Employee

        RegNo = txtRegNo.Text.Trim
        lblRegNo1.Text = "Reg. No.  :" & RegNo

        Model = txtModelList.Text.Trim
        lblModel1.Text = "Model  : " & Model

        lblJobType1.Text = "Job Type  : " & IIf(cmbWOJobType.SelectedIndex > 0, cmbWOJobType.SelectedItem.Text, "")

        If cmbDateRange.SelectedIndex = 0 Then
            SearchStr1 = ""
        Else
            SearchStr1 = cmbDateRange.SelectedItem.Text + " : " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
        End If

        EventLogDetail = lblDateRangeFrom.Text + ", " + lblEmployee1.Text + ", " + lblRegNo1.Text + ", " + lblModel1.Text + ", " + lblJobType1.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        'Session("IsExcel") = IsExcel
        SetValues()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsnWOSummary
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String
        If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
            If cmbFormat.SelectedIndex = 0 Then 'Format 1
                myReport = New crptEmpwiseWorkDoneInWO
                ReportName = "Employee Wise Work Done"
            Else 'Format 2
                myReport = New crptEmpwiseWorkDoneInWOFormat2
                ReportName = "Employee Log Book"
            End If
        Else
            myReport = New crptEmpwiseWorkDoneInWO
            ReportName = "Employee Wise Work Done"
        End If




        mEmployeeWiseWorkDoneInWO = EmployeeWiseWorkDoneInWO.GetEmployeeWiseWorkDone(EmployeeID:=New Guid(cmbEmployee.SelectedValue.ToString), Text:="", No:=0, FromDate:=FromDate, ToDate:=ToDate, RegNo:=RegNo, ModelName:=Model, StatusID:=0, WOStatusID:=0, CustomerID:="{00000000-0000-0000-0000-000000000000}", SerialNo:="", WOJobTypeID:=cmbWOJobType.SelectedValue, Format:=IIf(cmbFormat.Visible, cmbFormat.SelectedIndex, 0))
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, ReportName:=ReportName, SearchStr1:=SearchStr1, SearchStr2:=Employee, SearchStr3:=SearchStr3, SearchStr4:=RegNo, SearchStr5:=Model, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=SearchStr6, SearchStr7:="", SearchStr8:=IIf(cmbWOJobType.SelectedIndex > 0, cmbWOJobType.SelectedItem.Text, ""), SearchStr9:="", SearchStr10:=AppSettings("Logo"))

        If mEmployeeWiseWorkDoneInWO.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mEmployeeWiseWorkDoneInWO.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1349)
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)

        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        da.Fill(ds, mEmployeeWiseWorkDoneInWO)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "EmployeeWiseWorkDoneInWO", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub addAttributes()
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        'Employee
        mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(SELECT)")
        cmbEmployee.DataSource = mEmployeeListForCombo
        Session("mEmployeeListForCombo") = mEmployeeListForCombo

        mWOJobTypeList = nWOJobTypeList.GetWOJobTypeList("All")
        cmbWOJobType.DataSource = mWOJobTypeList
        Session("mWOJobTypeList") = mWOJobTypeList
        DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            lblStep7.Text = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "Step VII. Display Report", "Step VI. Display Report")
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        Else
            upnlValidations.Update()
        End If
    End Sub
    'Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
    '    SetReport(True) 'Export button added by Saylee on 19-July-2012
    'End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
#End Region
    'Added By Abhishek on 10-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()

            mEmployeeWiseWorkDoneInWO = EmployeeWiseWorkDoneInWO.GetEmployeeWiseWorkDone(EmployeeID:=New Guid(cmbEmployee.SelectedValue.ToString), Text:="", No:=0, FromDate:=FromDate, ToDate:=ToDate, RegNo:=RegNo, ModelName:=Model, StatusID:=0, WOStatusID:=0, CustomerID:="{00000000-0000-0000-0000-000000000000}", SerialNo:="", WOJobTypeID:=cmbWOJobType.SelectedValue, Format:=IIf(cmbFormat.Visible, cmbFormat.SelectedIndex, 0))
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                    mCompanyDetail.WebSite, ReportName:=ReportName, SearchStr1:=SearchStr1, SearchStr2:=Employee, SearchStr3:=SearchStr3, SearchStr4:=RegNo, SearchStr5:=Model, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=SearchStr6, SearchStr7:="", SearchStr8:=IIf(cmbWOJobType.SelectedIndex > 0, cmbWOJobType.SelectedItem.Text, ""), SearchStr9:="", SearchStr10:=AppSettings("Logo"))

            If mEmployeeWiseWorkDoneInWO.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf mEmployeeWiseWorkDoneInWO.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1349)
            End If


            If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
                If cmbFormat.SelectedIndex = 0 Then 'Format 1
                    da.Fill(ds, "ExcelnrptWOSummary", mEmployeeWiseWorkDoneInWO)
                    da.Fill(ds, "ReportData", Report)
                    Dim columnToRemove1 As String() = {"WOText", "WOJobTypeID", "WOJobStatusID", "MonitorTypeID", "WORemark", "MonitorInfoType", "OnTypeID", "WONo", "WOStartDate", "WOStatusID", "HourType", "LogID", "WOTotalActualTime", "LogText", "LogNo", "WOPlanedDate", "WOCloseDate", "TotalActualTime", "TotalEstimatedTime", "CustomerID", "MachineID", "StatusID", "WODate", "WOJobAction", "EmpCategory", "ID", "WOPlanedDateFormatted", "WOBy", "RegNo", "ModelName", "SerialNo", "LogNumber", "WOActualTime", "IsInHouse", "WOStatusName", "StatusName", "InHouseThirdParty", "IsThirdParty", "WOJobTypeName", "WOJobDescription", "DueAsOf", "WOJobAction", "WOJobEstimatedTime", "WOJobActualTime", "WOJobStatusName", "WOJobSrNo"}
                    For i As Integer = 0 To columnToRemove1.Length - 1
                        If ds.Tables("ExcelnrptWOSummary").Columns.Contains(columnToRemove1(i)) Then
                            ds.Tables("ExcelnrptWOSummary").Columns.Remove(columnToRemove1(i))
                        End If
                    Next
                    Dim columnToRemove2 As String() = {"ReportName", "SearchStr10", "SearchStr3", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "CurrencySymbol", "CurrencyName", "SearchStr7", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr6", "SearchStr9", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                        End If
                    Next
                    If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                        ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Employee"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                        ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Between Dates"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                        ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Reg No."
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                        ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Model"
                    End If

                    If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
                        ds.Tables("ReportData").Columns("SearchStr8").ColumnName = "JobType"
                    End If


                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WODateFormatted") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("WODateFormatted").ColumnName = "Date"
                    End If

                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WONumber") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("WONumber").ColumnName = "Work Order No."
                    End If
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("ExcelModelAircraftSerialNo") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("ExcelModelAircraftSerialNo").ColumnName = "Reg No./Serial No./Model"
                    End If

                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOStartDateFormatted") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("WOStartDateFormatted").ColumnName = "Start Date"
                    End If
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOCloseDateFormatted") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("WOCloseDateFormatted").ColumnName = "End Date"
                    End If
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("EmpEstimatedTime") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("EmpEstimatedTime").ColumnName = "Estimated Time"
                    End If
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("EmpActualTime") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("EmpActualTime").ColumnName = "Actual Time"
                    End If
                    Dim dsNew As New DataSet
                    dsNew.Clear()

                    dsNew.Merge(ds.Tables("ReportData"))
                    dsNew.Merge(ds.Tables("ExcelnrptWOSummary"))

                    dsNew.Tables("ReportData").TableName = "Searching Criteria"
                    dsNew.Tables("ExcelnrptWOSummary").TableName = "Employee Wise WorkDone"
					Session("ExcelFileName") = "Employee Wise WorkDone"
					Session("dsNew") = dsNew
					'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
					'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
					'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
					'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    ReportName = "Employee Wise Work Done"
                Else 'Format 2
                    da.Fill(ds, "ExcelnrptWOSummary", mEmployeeWiseWorkDoneInWO)
                    da.Fill(ds, "ReportData", Report)

                    Dim columnToRemove1 As String() = {"WODateFormatted", "WOStartDateFormatted", "WOCloseDate", "ReportName", "WONumber", "CustomerName", "EmpActualTime", "EmpEstimatedTime", "WOText", "WOJobTypeID", "WOJobStatusID", "MonitorTypeID", "WORemark", "MonitorInfoType", "OnTypeID", "WONo", "WOStartDate", "WOStatusID", "HourType", "LogID", "WOTotalActualTime", "LogText", "LogNo", "WOPlanedDate", "TotalActualTime", "TotalEstimatedTime", "CustomerID", "MachineID", "StatusID", "WODate", "ID", "WOPlanedDateFormatted", "WOBy", "RegNo", "ModelName", "SerialNo", "LogNumber", "WOActualTime", "IsInHouse", "WOStatusName", "StatusName", "InHouseThirdParty", "IsThirdParty", "WOJobTypeName", "WOJobDescription", "DueAsOf", "WOJobEstimatedTime", "WOJobActualTime", "WOJobStatusName", "WOJobSrNo"}
                    For i As Integer = 0 To columnToRemove1.Length - 1
                        If ds.Tables("ExcelnrptWOSummary").Columns.Contains(columnToRemove1(i)) Then
                            ds.Tables("ExcelnrptWOSummary").Columns.Remove(columnToRemove1(i))
                        End If
                    Next
                    Dim columnToRemove2 As String() = {"ReportName", "SearchStr10", "SearchStr3", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "CurrencySymbol", "CurrencyName", "SearchStr11", "SearchStr7", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr6", "SearchStr9", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                        End If
                    Next

                    If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                        ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Between Dates"
                    End If

                    If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                        ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Employee"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                        ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Reg No."
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                        ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Model"
                    End If

                    If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
                        ds.Tables("ReportData").Columns("SearchStr8").ColumnName = "JobType"
                    End If
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("ExcelModelAircraftSerialNo") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("ExcelModelAircraftSerialNo").ColumnName = "Aircraft Reg No. & Type"
                    End If

                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("EmpCategory") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("EmpCategory").ColumnName = "Category"
                    End If
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobAction") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("WOJobAction").ColumnName = "Typical Maintenance Tasks"
                    End If
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOCloseDateFormatted") Then
                        ds.Tables("ExcelnrptWOSummary").Columns("WOCloseDateFormatted").ColumnName = "Date On Which Performed"
                    End If

                    Dim dsNew As New DataSet
                    dsNew.Clear()

                    dsNew.Merge(ds.Tables("ReportData"))
                    dsNew.Merge(ds.Tables("ExcelnrptWOSummary"))

                    dsNew.Tables("ReportData").TableName = "Searching Criteria"
                    dsNew.Tables("ExcelnrptWOSummary").TableName = "Employee Log Book"
					Session("ExcelFileName") = "Employee Log Book"
					Session("dsNew") = dsNew
					'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
					'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
					'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
					'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)

                    ReportName = "Employee Log Book"
                End If
            Else
                da.Fill(ds, "ExcelnrptWOSummary", mEmployeeWiseWorkDoneInWO)
                da.Fill(ds, "ReportData", Report)
                Dim columnToRemove1 As String() = {"WOText", "WOJobTypeID", "WOJobStatusID", "MonitorTypeID", "WORemark", "MonitorInfoType", "OnTypeID", "WONo", "WOStartDate", "WOStatusID", "HourType", "LogID", "WOTotalActualTime", "LogText", "LogNo", "WOPlanedDate", "WOCloseDate", "TotalActualTime", "TotalEstimatedTime", "CustomerID", "MachineID", "StatusID", "WODate", "WOJobAction", "EmpCategory", "ID", "WOPlanedDateFormatted", "WOBy", "RegNo", "ModelName", "SerialNo", "LogNumber", "WOActualTime", "IsInHouse", "WOStatusName", "StatusName", "InHouseThirdParty", "IsThirdParty", "WOJobTypeName", "WOJobDescription", "DueAsOf", "WOJobAction", "WOJobEstimatedTime", "WOJobActualTime", "WOJobStatusName", "WOJobSrNo"}
                For i As Integer = 0 To columnToRemove1.Length - 1
                    If ds.Tables("ExcelnrptWOSummary").Columns.Contains(columnToRemove1(i)) Then
                        ds.Tables("ExcelnrptWOSummary").Columns.Remove(columnToRemove1(i))
                    End If
                Next


                Dim columnToRemove2 As String() = {"ReportName", "SearchStr10", "SearchStr3", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "CurrencySymbol", "CurrencyName", "SearchStr11", "SearchStr7", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr6", "SearchStr9", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                    End If
                Next
                If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                    ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Between Dates"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                    ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Employee"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                    ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Reg No."
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                    ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Model"
                End If

                If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
                    ds.Tables("ReportData").Columns("SearchStr8").ColumnName = "JobType"
                End If


                If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WODateFormatted") Then
                    ds.Tables("ExcelnrptWOSummary").Columns("WODateFormatted").ColumnName = "Date"
                End If

                If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WONumber") Then
                    ds.Tables("ExcelnrptWOSummary").Columns("WONumber").ColumnName = "Work Order No."
                End If
                If ds.Tables("ExcelnrptWOSummary").Columns.Contains("ExcelModelAircraftSerialNo") Then
                    ds.Tables("ExcelnrptWOSummary").Columns("ExcelModelAircraftSerialNo").ColumnName = "Reg No./Serial No./Model"
                End If

                If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOStartDateFormatted") Then
                    ds.Tables("ExcelnrptWOSummary").Columns("WOStartDateFormatted").ColumnName = "Start Date"
                End If
                If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOCloseDateFormatted") Then
                    ds.Tables("ExcelnrptWOSummary").Columns("WOCloseDateFormatted").ColumnName = "End Date"
                End If
                If ds.Tables("ExcelnrptWOSummary").Columns.Contains("EmpEstimatedTime") Then
                    ds.Tables("ExcelnrptWOSummary").Columns("EmpEstimatedTime").ColumnName = "Estimated Time"
                End If
                If ds.Tables("ExcelnrptWOSummary").Columns.Contains("EmpActualTime") Then
                    ds.Tables("ExcelnrptWOSummary").Columns("EmpActualTime").ColumnName = "Actual Time"
                End If
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("ReportData"))
                dsNew.Merge(ds.Tables("ExcelnrptWOSummary"))

                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Tables("ExcelnrptWOSummary").TableName = "Employee Wise WorkDone"
				Session("ExcelFileName") = "Employee Wise WorkDone"
				Session("dsNew") = dsNew
				'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
				'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
				'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
				'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                ReportName = "Employee Wise Work Done"

            End If
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "EmployeeWiseWorkDoneInWO", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
#Region " Service "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
               Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                   Select c.Name).Take(count).ToList
        End If
    End Function
#End Region
    
End Class