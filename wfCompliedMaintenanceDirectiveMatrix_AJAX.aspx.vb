
'Created by Saylee on 29-Aug-2025

Imports System.Collections.Generic
Public Class wfCompliedMaintenanceDirectiveMatrix_AJAX
    Inherits System.Web.UI.Page



#Region " Variable Declaration "
    Dim mCompliedDirectiveStatusActivityList As CompliedDirectiveStatusActivityList


    Dim FromDate As String
    Dim ToDate As String
    Dim Directive As String
    Dim Aircraft As String
    Dim AircraftIndex As Integer
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim AssemblyType As String
    Dim ModelName As String
    Dim ModTypeName As String
    Dim MachineName As String
    Dim ModelID As String

    Private mMachineList As MachineList
    Dim mMachineNameValueList As MachineNameValueList

    Private mModTypeList As ModTypeList
    Private mAssemblyList As AssemblyList


    Dim ModTypeID As String

    Dim ArrCnt As Integer = 0

    Public EventLogID As Guid
    Public EventLogDetail As String = ""
    Dim ModificationTypeName(50) As String
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Private DirectiveName As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mModTypeList = CType(Session("mModTypeList"), ModTypeList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mAssemblyList") = mAssemblyList
        Session("mModTypeList") = mModTypeList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfCompliedMaintenanceDirectiveMatrix_AJAX.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblyList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType, "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblAssembly1.Visible = True
    End Sub
    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            If cmbAssembly.SelectedItem.Text = "<SELECT>" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : All"
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1
            End If
        End If

        If Not IsDate(txtFromDate.Text) Then
            FromDate = ""
        Else
            FromDate = txtFromDate.Text.ToString
        End If
        If Not IsDate(txtToDate.Text) Then
            ToDate = ""
        Else
            ToDate = txtToDate.Text.ToString
        End If

        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        lblDateRangeFrom.Text = "From Date : " & IIf(FromDate <> "", New SmartDate(FromDate).FormattedText, "") & "   To Date : " & IIf(ToDate <> "", New SmartDate(ToDate).FormattedText, "")


        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")

        Dim Type As String = ""

        'Set Service/Inspection/Directive checkbox list values
        'Service

        If cmbType.SelectedItem.Text = "<SELECT>" Then     'Directive
            Directive = ""
            lblType1.Text = ""
            lblType1.Visible = False
        Else
            DirectiveName = mModTypeList(cmbType.SelectedIndex).Name
            Directive = cmbType.SelectedItem.Text
            lblType1.Text = "Directive Name : " & Directive
        End If


        EventLogDetail = lblDateRangeFrom.Text + " , " + lblAircraft1.Text + " , " + lblAssembly1.Text + Type
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        ModTypeID = ""
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Private Sub SetReport(Optional ByVal ByExcel As Boolean = False)
		Dim CrystalReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ds As New dsCompliedActivityList
        Dim ReportName As String
        Dim Operatorname As String = ""
        SetValues()


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")



        mCompliedDirectiveStatusActivityList = CompliedDirectiveStatusActivityList.GetCompliedModActivityList(FromDate,
                                                                                                              ToDate,
                                                                                                              New Guid(MachineName),
                                                                                                              "",
                                                                                                              cmbAssembly.SelectedValue,
                                                                                                              0,
                                                                                                              chkApplicable.Checked,
                                                                                                              ModTypeIDs:=cmbType.SelectedValue.ToString)




        ReportName = cmbType.SelectedItem.ToString & " Compliance Matrix"
		CrystalReport = New crptDirectiveComplianceMatrix

		If mCompliedDirectiveStatusActivityList.Count <= 0 Then
            MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mCompliedDirectiveStatusActivityList.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1271)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                 mCompanyDetail.WebSite, ReportName, New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, Aircraft, Assembly1, Operatorname, mModuleList.Item("CompliedMaintenanceActivities").FormRevisionNo, AppSettings("SINote"), , , , AppSettings("MELSnagNomenclature").ToString, AppSettings("Logo"))
        'Replace AppSettings("Product Version") with mModuleList.Item("CompliedMaintenanceActivities").FormRevisionNo for  SUHAN

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mCompliedDirectiveStatusActivityList)
        da.Fill(ds, "CompliedStatusActivityList", mCompliedDirectiveStatusActivityList)
        If ByExcel = True Then SetDirectiveExcel(mCompliedDirectiveStatusActivityList, Report, ReportName) : Exit Sub



        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
		CrystalReport.SetDataSource(ds)
		Session("CrystalReport") = CrystalReport
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        ' MarkLog(Util.Action.Print, "CompliedMaintenanceActivities", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        MarkLog(Util.Action.Print, "CompliedMaintenanceActivities", IIf(ByExcel = True, "Export To excel ", "") + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)  'Added by Shital on 18-Jan-2021 Export To excel
        ResetValues()
    End Sub
    '***************************************************************************************
    Private Sub SetDirectiveExcel(ByVal ExCompliedDirectiveStatusActivityList As CompliedDirectiveStatusActivityList, ByVal ExReport As ReportData, ByVal ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsExcel As New dsCompliedActivityList
        da.Fill(dsExcel, ExCompliedDirectiveStatusActivityList)
        da.Fill(dsExcel, ExReport)

        Dim columnToRemove As String() = {
                                                "AssemblyCompID",
                                                "AssemblyID",
                                                "AsOnDate",
                                                "ApprovalRemark",
                                                "AssemblyCompSerialNo",
                                                "AssemblyName",
                                                "AssemblyPosition",
                                                "AssemblySerialNo",
                                                "AssemblyStatusID",
                                                "AssemblyType",
                                                "AssemblyTypeID",
                                                "CompName",
                                                "CompPosition",
                                                "CompSerialNo",
                                                "CompStatusID",
                                                "DoneOn",
                                                "DueOnValueFormattedForGrid",
                                                "DueStatus",
                                                "ExtensionDate",
                                                "ExtensionDateFormatted",
                                                "HourType",
                                                "IsApplicable",
                                                "IsCompleted",
                                                "IsMaster",
                                                "MachineID",
                                                "MachineInfo",
                                                "MinimumPercentLife",
                                                "ModelPartMonitorModCode",
                                                "ModelPartMonitorModID",
                                                "ModelPartMonitorModTypeCode",
                                                "MonitorModStatusID",
                                                "MonitorModStatusPeriodID",
                                                "MonitorTypeID",
                                                "Note",
                                                "PeriodID",
                                                "PeriodUnitID",
                                                "PeriodUnitName",
                                                "PeriodUnitNameForWeb",
                                                "PreviousDueOnValue",
                                                "RequiredManHours",
                                                "DirectiveType",
                                                "WasteLife",
                                                "FrequencyValue",
                                                "DoneOnValue",
                                                "DueOnValue",
                                                "AssemblyInfo",
                                                "CompInfo",
                                                "DoneOnWONO",
                                                "Description",
                                                "CurrentValue",
                                                "MonitorStatusID",
                                                "MonitorStatusPeriodID",
                                                "ModelPartMonitorID",
                                                "ATA",
                                                "WasteLifeExcel",
                                                "WastePercentLife",
                                                "DoneRemark",
                                                "AssemblyInfoExcel", "WorkDoneInfo", "FrequencyValueExcel",
                                                "DoneOnValueExcel", "MonitorType", "ModelPartMonitorDirectiveTypeName", "DoneBy"
                                            }

        For i As Integer = 0 To columnToRemove.Length - 1
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Contains(columnToRemove(i)) Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Remove(columnToRemove(i))
            End If
        Next

        'set Column Sequence
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DirectiveNo").SetOrdinal(1)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("Reference").SetOrdinal(2)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("AsOnDateFormatted").SetOrdinal(3)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("ModDescription").SetOrdinal(4)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("AssemblyInfoExcel").SetOrdinal(5)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("CompInfoExcel").SetOrdinal(6)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DoneOnFormatted").SetOrdinal(7)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("MethodOfCompliance").SetOrdinal(8)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DueOnValueExcel").SetOrdinal(9)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("EmpName").SetOrdinal(10)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("Place").SetOrdinal(11)






        Dim ColumnName As String = String.Empty

        For i As Integer = 0 To dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Count - 1

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DirectiveNo" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "AD #"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "ModDescription" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Description"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "AsOnDateFormatted" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Effective Date"
            End If


            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "AssemblyInfoExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Assembly Info"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "CompInfoExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Comp Info"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "EmpName" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Work Done By"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DoneOnFormatted" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Done On"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DueOnValueExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Due On"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "MethodOfCompliance" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Method Of Compliance"
            End If

        Next

        Dim columnToRemoveCriteria As String() = {
                                                "ReportDate",
                                                "ID",
                                                "CompanyName",
                                                "Address",
                                                "Tel1",
                                                "Tel2",
                                                "Fax",
                                                "Email",
                                                "WebSite",
                                                "ReportName",
                                                "SearchStr7",
                                                "SearchStr8",
                                                "SearchStr9",
                                                "ProductVersion",
                                                "SINote",
                                                "CurrencyName",
                                                "CurrencySymbol",
                                                "SearchStr10",
                                                "SearchStr11",
                                                 "SearchStr12",
                                                 "SearchStr13",
                                                "SearchStr14",
                                                "ShortName",
                                                "SearchStr15",
                                               "SearchStr16",
                                               "SearchStr17",
                                               "SearchStr18",
                                               "SearchStr19",
                                               "SearchStr20",
                                               "SearchStr21",
                                               "SearchStr22",
                                               "SearchStr23",
                                               "SearchStr24",
                                               "SearchStr25",
                                               "SearchStr5", "SearchStr6"
                                                       }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If dsExcel.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                dsExcel.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next
        dsExcel.Tables("ReportData").Columns("SearchStr1").SetOrdinal(0)
        dsExcel.Tables("ReportData").Columns("SearchStr2").SetOrdinal(1)
        dsExcel.Tables("ReportData").Columns("SearchStr3").SetOrdinal(2)
        dsExcel.Tables("ReportData").Columns("SearchStr4").SetOrdinal(3)

        dsExcel.Tables("ReportData").Columns("SearchStr6").SetOrdinal(4)

        For i As Integer = 0 To dsExcel.Tables("ReportData").Columns.Count - 1
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "From Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "To Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Assembly"
            End If
            'If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
            '    dsExcel.Tables("ReportData").Columns(i).ColumnName = "Operator"
            'End If

        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsExcel.Tables("ReportData"))
        dsNew.Merge(dsExcel.Tables("CompliedDirectiveStatusActivityList"))

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("CompliedDirectiveStatusActivityList").TableName = ReportName
        Session("ExcelFileName") = ReportName

        Session("DataTableToBeFormattedForExportToExcel") = ReportName
        PeriodColumnsForExportToExcel.AddRange(New String() {"Due On"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
    '*******************************************************************************************
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    ''Response.Redirect("wfCompliedMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            ''Response.Redirect("wfCompliedMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
        ElseIf custValidator.ControlToValidate = "txtFromDate" Then
            If txtFromDate.Text = "" Or txtToDate.Text = "" Then
                custValidator.ErrorMessage = "Please Enter Valid Date."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbType" Then                          'Aircraft
            If cmbType.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Directive"
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        End If
    End Sub


    Public Sub SetCombo()
        ''GetSession()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        'cmbAircraft.DataBind()
        Session("mMachineNameValueList") = mMachineNameValueList

        mModTypeList = ModTypeList.GetModelTypeList(IsSelectTagRequired:=True)     'Modification Type
        cmbType.DataSource = mModTypeList
        Session("mModTypeList") = mModTypeList
        DataBind()
    End Sub



#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfCompliedMaintenanceDirectiveMatrix_AJAX.aspx?"
            ResetValues()
            SetCombo()
            cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            setFocus(cmbAircraft)
            SetSession()

        End If

        '  MessageBoxResult()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
            upnlCriteria.Update()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid = True Then

            SetReport()
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyList = Nothing
        mModTypeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            cmbAssembly.Enabled = False
            cmbAssembly.SelectedIndex = 0
        Else
            cmbAssembly.Enabled = True

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
        End If

        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        upnlAssembly.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnByExcel_Click(sender As Object, e As System.EventArgs) Handles btnByExcel.Click
        If IsValid = True Then
            SetReport(True)

        End If
    End Sub
#End Region

End Class