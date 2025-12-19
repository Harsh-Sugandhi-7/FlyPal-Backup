
'Added By Vikrant On 26-May-2014 For ALL02062014

Imports System.Collections.Generic
Imports Flypal.PartListAutoComplete
Imports System.Linq

Public Class wfrptPartWiseComponentStatus_Ajax
    Inherits System.Web.UI.Page

#Region "Enum"
    Private Enum ErrorMessage
        HourMinuteFormat = 0
        MinutesInDecimal = 1
        MinutesFormat = 2
        HourDecimalFormat = 3
        WholeNumber = 4
        ValidDate = 5
        DecimalValue = 6
        ValidLenght = 7
        ValidDays = 8
        ValidMonths = 9
        ValidYears = 10
    End Enum
#End Region
    
#Region " Variable Declarations "
    Public mPartListForSerialNos As PartListForSerialNos
    Public mPartList As PartList
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Public mPartID, mPartName As String
    Public mPartMonitorServiceTypeList As PartMonitorServiceTypeList
    Public mMachineList As MachineList
    Public PeriodLimt As String = String.Empty
    Public searchstr2 As String = String.Empty

    Public ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Public ReportStatusList As New rptStatusList
    Public mPerDayLimits As PerDayLimits

    Dim Aircraft As String
    Dim Assembly1, Component1, Component, SerialNo, SerialNo1 As String
    Dim CheckService As Boolean
    Dim ServiceType As String
    'End of Added Code
    Dim Report As Integer
    Dim ShowCofA As Boolean
    Dim AsonDate As String
    Dim IsSerSelect As Boolean
    Dim IsInsSelect As Boolean
    Dim ServiceTypeID(50), InspTypeID(50) As Integer
    Dim ReportLabel As String
    Dim ReportType As String
    Dim x As Integer
    Dim Periodcount As Integer
    Dim Count As Integer
    Dim AssemblyName As String
    Dim MachineName As String
    Dim Machine1 As String
    Dim AssemblyID As Guid
    Dim AircraftIndex As Integer   'Added Code 

    Private ATAChapter As String = ""
    Private RegNo As String
    Private AssemblyType As String
    Private Model As String
    Private AssemblySerialNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String = ""
    Private MonitorType As String = ""
    Private Note As String = ""
    Private EstimatedDate As String = ""
    Private Freq1 As String
    Private Freq2 As String
    Private Freq3 As String
    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String
    Private RemainingTime As String
    Private RemainingTime1 As String
    Private RemainingTime2 As String
    Private DueAsof As String
    Private DueAsof1 As String
    Private DueAsof2 As String
    Private AssemblyModel As String
    Private ATACode As Integer = 0
    Private InstalledAt As String
    Private InstalledAt1 As String
    Private InstalledAt2 As String
    Private TSO As String
    Private TSN As String
    Private TSO1 As String
    Private TSO2 As String
    Private RemoveAt As String
    Private RemoveAt1 As String
    Private RemoveAt2, SerialNoPostion, DoneRemrk As String
    Private InstalledAtDate As SmartDate = New SmartDate(True)
    Private RemoveAtDate As SmartDate = New SmartDate(True)
    Private DoneOnValue As String
    Private DoneOnDate As SmartDate = New SmartDate(True)

    Public EventLogID As Guid
    Dim EventLogDetail As String = String.Empty
    'Added By Vikrant On 04-Jan-2018 For ALL04012019
    Public mPartMonitorInspTypeList As PartMonitorInspTypeList
    Private MaintenanceActivityTypeID As Integer = 0
    'End
#End Region

#Region " Business Methods "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        custValidator.ControlToValidate = "txtsearch"
        If txtSearch.Text = "" Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
            If PartNo = "" Or Description = "" Then
                e.IsValid = False
            End If
            mPartList = PartList.GetPartList(PartNo)
            If mPartList.Count <= 0 Then
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptPartWiseComponentStatus_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPerDayLimits")
        Session.Remove("mPartMonitorServiceTypeList")
        Session.Remove("mPartMonitorInspTypeList") 'Added By Vikrant On 04-Jan-2018 For ALL04012019
    End Sub
    Private Sub GetSession()
        mPerDayLimits = Session("mPerDayLimits")
        mPartMonitorServiceTypeList = Session("mPartMonitorServiceTypeList")
        mPartMonitorInspTypeList = Session("mPartMonitorInspTypeList") 'Added By Vikrant On 04-Jan-2018 For ALL04012019
    End Sub
    Private Sub DataFieldBind()
        'mPartID = IIf(PartID.Value.Length > 0, PartID.Value, Guid.Empty.ToString)
        'mPartName = IIf(PartName.Value.Length > 0, PartName.Value, "")
        If (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            PartNo = txtSearch.Text.Trim
            Description = txtSearch.Text.Trim
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        End If
        mPartList = PartList.GetPartList(PartNo)
        If mPartList.Count > 0 Then
            mPartID = mPartList(0).ID.ToString
            mPartName = PartNo
        Else
            mPartID = Guid.Empty.ToString
            mPartName = ""
        End If

        mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, False, mPartID)
        chkListServiceType.DataSource = mPartMonitorServiceTypeList
        Session("mPartMonitorServiceTypeList") = mPartMonitorServiceTypeList
        'Added By Vikrant On 04-Jan-2018 For ALL04012019
        mPartMonitorInspTypeList = PartMonitorInspTypeList.GetPartMonitorInspTypeList(, mPartID)
        chkListInspType.DataSource = mPartMonitorInspTypeList
        Session("mPartMonitorInspTypeList") = mPartMonitorInspTypeList
        'End
        mPerDayLimits = PerDayLimits.GetPerDayLimits(Guid.Empty, True, mPartID)
        gdPerDayLimit.DataSource = mPerDayLimits
        Session("mPerDayLimits") = mPerDayLimits

        DataBind()
    End Sub
    Private Sub SetGridObject()
        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdPerDayLimit.Rows.Count - 1
            txtPerDatLimit = CType(Me.gdPerDayLimit.Rows(i1).FindControl("txtLimitPerDay"), TextBox)
            'mPerDayLimits.Item(i1).PeriodLimit = CDec(Val(Trim(txtPerDatLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mPerDayLimits.Item(i1).PeriodLimit = Trim(txtPerDatLimit.Text)  'Added by Saylee on 12-Nov-2012
            PeriodLimt = PeriodLimt + ", " + Trim(txtPerDatLimit.Text)
        Next i1
        'Session("mPerDayLimits") = mPerDayLimits
    End Sub
    Private Sub SetPerDayLimitValues()
        searchstr2 = ""
        Dim mPerDayLimit As PerDayLimit
        For Each mPerDayLimit In mPerDayLimits
            If CDec(Val(mPerDayLimit.PeriodLimit)) >= 0 Then
                If searchstr2 = "" Then
                    searchstr2 = "Note :- The Tentative Date of Removal has been calculated on the basis of A/C utilization" & " " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                Else
                    searchstr2 = searchstr2 & ", " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                End If
            End If
        Next
        If searchstr2 <> "" Then searchstr2 = searchstr2 & " per Day "
    End Sub
    Private Sub ControlVisibility()
        If PartID.Value.Length > 0 Then

        End If
    End Sub
    Private Sub Display()
        lblDateRange.Visible = True
        lblComponent1.Visible = True
        upnlSearchingCriteria.Update()
    End Sub
    Public Sub SetValues()
        AsonDate = txtAsOnDate.Text.ToString                                     'Date

        If Not IsDate(txtAsOnDate.Text.Trim) Then            'Date  
            AsonDate = ""
        Else
            AsonDate = txtAsOnDate.Text.Trim
            lblDateRange.Text = "AsonDate : " & txtAsOnDate.Text.Trim
        End If


        If cmbSerialNo.SelectedIndex > 0 Then
            lblComponent1.Text = "Part No : " + txtSearch.Text + " Serial No : " + cmbSerialNo.SelectedItem.ToString
        Else
            lblComponent1.Text = "Part No : " + txtSearch.Text + " Serial No : (ALL)"
        End If
        SetGridObject()
        'End
        EventLogDetail = lblDateRange.Text + "," + lblComponent1.Text + ","
    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList

        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail

        Dim rptCompStatus As New CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim mCompanyDetail As New CompanyDetail


        rptCompStatus = New crptPartWiseComponentStatus

        SetPerDayLimitValues()
        SetValues()
        ReportDetail()
        ReportLabel = "Part wise Lifed Component Status"

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    mCompanyDetail.WebSite, ReportLabel, txtAsOnDate.Text.Trim, searchstr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), , , _
    txtSearch.Text.Trim, IIf(chkCheckForAlternatePart.Checked, mPartList(0).PartNameWithAlternateParts.Replace(mPartList(0).Name + ",", "").Replace(mPartList(0).Name, ""), ""), _
    AppSettings("Logo"))

        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1290)
        End If
        If IsExcel = False Then 'If PDF format  
            ds.Clear()
            '-----------Added by vikrant for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, ReportMaintenanceDetails)
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, mrptImage) 'Added by vikrant for Report Logo

            rptCompStatus.SetDataSource(ds)
            Session("CrystalReport") = rptCompStatus
            Try
                ReportMaintenanceDetails = Nothing
                Report = Nothing
                ReportStatusList = Nothing
            Catch ex As Exception
                Throw ex
            End Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "PartWiseComponentStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            ds.Clear()
            Dim objsearch As rptSearchingCriteria
            da.Fill(ds, ReportStatusList)

            Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)

            reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
                                     Order By c.MaintenanceTypeID, c.PartNo, c.CompSerialNo
                                     Select c).ToList

            da.Fill(ds, "ExcelReportMaintenanceDetailList", reportmaintdetailslist)
            ' da.Fill(ds, "ExcelReport", Report)

            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", txtAsOnDate.Text, PartNo, cmbSerialNo.SelectedItem.ToString, "", "", "", "", "", Description, ReportLabel, "", 0, "", "", "", "", "")
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            Dim columnToRemove As String() = {"ID", "Code", "Name", "Model", "EstDate", "SerialNo", "MonitorType", "Freq2", "Freq3", "ElapsedTime1", _
                                              "ElapsedTime2", "RemainingTime1", "RemainingTime2", "DueAsof1", "DueAsof2", "AssemblySerialNo", _
                                              "ComponentInfo", "AssemblyType", "SinceNew", "SinceNew1", "DoneAt", "DoneAt1", "AssemblyModel", _
                                               "MinimumRemainingValue", "AssemblyTypeID", "MaintenanceEvent", "InstalledAt1", _
                                                "InstalledAt2", "TSO2", "RemoveAt1", "RemoveAt2", "ModificationNumber", "DoneWONo", "DetailID", _
                                                "Applicability", "ApplicabilityForExcel", "ComplianceRequirement", "AssemblyDueAsof", "AssemblyDueAsof1", _
                                                "AssemblyDueAsof2", "Extension1", "Extension2", "ExtensionDate", "ApprovalRemark", "RequiredManHours", _
                                                "Customer", "SupersededByADNumber", "IssueDate", "IsApplicable", "MaintenanceTypeID", "MaintenanceTypeName", _
                                                "IsLater", "DueStatus", "ModelMonitorModCode", "StatusTypeName", "WONumber", "StatusMasterID", "StatusID", _
                                                "TypeID", "CompStatusID", "AssemblyStatusID", "DocumentTypeForID", "MaintenanceOn", "MaintenanceInformation", _
                                                "MaintenanceInfo", "Frequency", "SinceNewAll", "ElapsedAll", "DoneAtAll", "ExtensionAll", "DueAsofAll", _
                                                "AssDueAsofAll", "RemainingTimeAll", "LogBook", "DoneOnDate", "RemoveAt", "ATAChapter", _
                                                "RemoveAtDate", "DoneONValueForAssembly", "RecordID", "MachineID", "ModelID", "IsMaster", _
                                                "DiffCompInstDoneOnValue", "EffectiveFromAll", "MaintenanceOnExcel", "ReferenceForExcel", "MaintenanceInformationForExcel", _
                                                "Description", "MaintenanceInfoExcel", "FrequencyExcel", "SinceNewAllExcel", "ElapsedAllExcel", "EffectiveFromAllExcel", _
                                                "DoneAtAllExcel", "ExtensionAllExcel", "DueAsofAllExcel", "AssDueAsofAllExcel", "DataColumn1", _
                                                "RemainingTimeAllExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport", _
                                                "PONosForMaterialMgmtReport", "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport", "MonitorTypeWithCode", _
                                                "EROQtyForMaterialMgmtReport", "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area", _
                                                "Zone", "NoteForExcel", "MaintenanceActivityType", "LinkedMaintenanceActivityCount", "ModelEstimatedManHours", _
                                                "SourceDoc", "IsRII", "ReqNumber", "MaintenanceInformationExcel", _
                                                "ElapsedTime", "TimeSinceNew", "SinceNew2", "Extension", "FrequencyAccordingToTypeIDForExcel", "WONoExcel", "Note", "DoneAt2", "PartDesc", "Reference", _
                                              "PartMonitorCode", "AssemblyInstCycles", "AssemblyInstLandings", "AssemblyInstStartDate", "AssemblyInstHours", "InstCompCycles", "InstCompLandings", "InstCompStartDate", "InstCompHours", "CSNCycles", _
                                              "SinceNewLandings", "SinceNewDate", "TSNHours", "InstPlace", "InstallationDoneBy", "InstallationRemark", "InstallationWONo", "Manufacturer", "Remark", _
                                              "ThresholdAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel", "RemainingForExcel", "DaysMnthsYrsName", "DaysMnthsYrsValue", "LandingsFreq", _
                                              "HoursDoneOnValue", "CyclesDoneOnValue", "DaysMnthsYrsDoneOnValue", "LandingsDoneOnValue", "PartNo", "CompSerialNo", "HoursFreq", "CyclesFreq", "TSO1", "TSO", "InstalledAt", "Freq1", "TSN", "DoneOnValue", _
                                              "RemainingTime", "DueAsOf"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                End If
            Next


            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ATACode") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATACode").ColumnName = "ATA"
            End If

            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DescriptionForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").ColumnName = "Nomenclature"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("PartNoSerialNoforExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartNoSerialNoforExcel").ColumnName = "Part # &  Serial #"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("RegNo") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("RegNo").ColumnName = "A/C Regd."
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("Position") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Position").ColumnName = "Position"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MonitorTypeCode") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorTypeCode").ColumnName = "Service Type"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("InstalledAtDate") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstalledAtDate").ColumnName = "Date"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("TSOForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSOForExcel").ColumnName = "TSO/CSO"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("InstalledAtForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstalledAtForExcel").ColumnName = "TSN/CSN"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("TSO1ForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSO1ForExcel").ColumnName = "A/F Hrs./CYC"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("Freq1ForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Freq1ForExcel").ColumnName = "Time b/w OH /SLL"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("TSNForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSNForExcel").ColumnName = "Component Current"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DoneOnValueForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneOnValueForExcel").ColumnName = "DSO/MF"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("RemainingTimeForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTimeForExcel").ColumnName = "Component Remaining"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DueAsOfForExcel") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsOfForExcel").ColumnName = "Removal at Airframe"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("EstimatedDate") Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("EstimatedDate").ColumnName = "Tentative Date Of Removal"
            End If


            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATA").SetOrdinal(0)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Nomenclature").SetOrdinal(1)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Part # &  Serial #").SetOrdinal(2)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("A/C Regd.").SetOrdinal(3)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Position").SetOrdinal(4)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Service Type").SetOrdinal(5)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Date").SetOrdinal(6)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSO/CSO").SetOrdinal(7)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSN/CSN").SetOrdinal(8)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("A/F Hrs./CYC").SetOrdinal(9)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Time b/w OH /SLL").SetOrdinal(10)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Component Current").SetOrdinal(11)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DSO/MF").SetOrdinal(12)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Component Remaining").SetOrdinal(13)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Removal at Airframe").SetOrdinal(14)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Tentative Date Of Removal").SetOrdinal(15)

            Dim columnToRemove2 As String() = {"CompanyName", "BranchName", "Store", "KitName", "CurrencySymbol", "currencyName", "ProductVersion", _
                                               "SINote", "TransTypeID", "FromStore", "WorkOrderText", "WorkOrderNo", "Search2", "Search3", _
                                               "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "FromDate", "ToDate", _
                                               "Aircraft", "WorkShop", "SupplierName", "Category", "Nomenclature", "Description", "RelNoteNo", "Search1"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next



            Dim dsNew As New DataSet
            dsNew.Clear()
            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))
            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = "Part Wise Component Status Report"
			Session("ExcelFileName") = "Part Wise Component Status Report"

			' dsNew.Tables("rptParts").TableName = "Part List Report"
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "PartList", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Public Function FindMatchInStringArray(ByVal StrArray As String(), ByVal strToCompare As String) As Boolean
        For i As Integer = 0 To StrArray.Length - 1
            If strToCompare.Equals(StrArray(i)) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo
        'Added By Vikrant On 04-Jan-2018 For ALL04012019
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        InspTypeID = (From c As System.Web.UI.WebControls.ListItem In chkListInspType.Items
                        Where c.Selected = True
                        Select CInt(c.Value)).ToArray
        'End

        Dim ComponentSerialNo As String

        If cmbSerialNo.SelectedIndex > 0 Then
            ComponentSerialNo = cmbSerialNo.SelectedItem.ToString
        Else
            ComponentSerialNo = ""
        End If
        'mPartID = IIf(PartID.Value.Length > 0, PartID.Value, Guid.Empty.ToString)
        If (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            PartNo = txtSearch.Text.Trim
            Description = txtSearch.Text.Trim
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        End If
        mPartList = PartList.GetPartList(PartNo)
        If mPartList.Count > 0 Then
            mPartID = mPartList(0).ID.ToString
            mPartName = PartNo
        Else
            mPartID = Guid.Empty.ToString
            mPartName = ""
        End If

        ServiceTypeID = (From c As System.Web.UI.WebControls.ListItem In chkListServiceType.Items
                        Where c.Selected = True
                        Select CInt(c.Value)).ToArray

        mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(txtAsOnDate.Text, , , , , , , , , , , True, True, , , , , , , PartNo, , cmbSerialNo.SelectedValue.ToString, , , , , , , , , , , , ComponentSerialNo, , , False, , False, IIf(chkCheckForAlternatePart.Checked, Guid.Empty.ToString, mPartID), True, , , , , , IsAverageRequired:=True, AverageMonths:=0, CompMonitoringServiceRequired:=True, ByPerDayLimit:=True, PerdayLimits:=mPerDayLimits, PartNames:=IIf(chkCheckForAlternatePart.Checked, mPartList(0).PartNameWithAlternateParts, ""), CompMonitoringInspRequired:=True)

        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim LHLabel3 As String = ""
        Dim LHData3 As String = ""
        Dim LHLabel4 As String = ""
        Dim LHData4 As String = ""

        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                LHLabel2 = ""
                LHData2 = ""
                LHLabel3 = ""
                LHData3 = ""

                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                End If
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                    "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, LHLabel4, LHData4, , , , , , , , , , , LHLabel2, LHData2, LHLabel3, LHData3))

            Next
        Next

         For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                    If chkCheckForAlternatePart.Checked And FindMatchInStringArray(mPartList(0).PartNameWithAlternateParts.Split(","), ObjCompStatus.PartName) Or Not chkCheckForAlternatePart.Checked Then
                        InstalledAt = ""
                        InstalledAt1 = ""
                        TSO1 = ""
                        TSN = ""
                        For Each ObjCompStatusPeriod In ObjCompStatus.CompStatusPeriodList
                            If Not ObjCompStatusPeriod.PeriodID = 2 Then
                                If InstalledAt = "" Then
                                    InstalledAt = ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                                Else
                                    InstalledAt = InstalledAt & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                                End If

                                If TSO1 = "" Then
                                    TSO1 = ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").AirframeCurrentValueAtCompInstallationFormatted
                                Else
                                    TSO1 = TSO1 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").AirframeCurrentValueAtCompInstallationFormatted
                                End If

                                If TSN = "" Then
                                    TSN = ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompCurrentValueFormatted
                                Else
                                    TSN = TSN & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompCurrentValueFormatted
                                End If

                                If InstalledAt1 = "" Then
                                    InstalledAt1 = ""
                                Else
                                    InstalledAt1 = InstalledAt1 & vbCrLf & ""
                                End If

                            Else

                                'If InstalledAt = "" Then
                                '    InstalledAt = ""
                                'Else
                                '    InstalledAt = InstalledAt & vbCrLf & ""
                                'End If

                                'If TSO1 = "" Then
                                '    TSO1 = ""
                                'Else
                                '    TSO1 = TSO1 & vbCrLf & ""
                                'End If

                                'If TSN = "" Then
                                '    TSN = ""
                                'Else
                                '    TSN = TSN & vbCrLf & AsonDate
                                'End If

                                If InstalledAt1 = "" Then
                                    InstalledAt1 = ""
                                Else
                                    InstalledAt1 = InstalledAt1 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompStartValueFormatted
                                End If
                            End If
                        Next
                        If ObjCompStatus.CompMonitorServiceStatusList.Count = 0 Then
                            'ATAChapter = ObjCompStatus.ATACode.ToString + " " + "-" + " " + ObjCompStatus.ATANomenclature
                            'ATACode = ObjCompStatus.ATACode
                            'Description = ObjCompStatus.PartDescription
                            'PartNo = ObjCompStatus.PartName
                            'CompSerialNo = ObjCompStatus.CompSerialNo
                            'Position = ObjCompStatus.Position
                            'MonitorTypeCode = ""
                            'AssemblyModel = ObjAssemblyStatus.Model
                            'AssemblySerialNo = ObjAssemblyStatus.SerialNo
                            'Freq1 = ""
                            'Freq2 = ""
                            'Freq3 = ""
                            'ElapsedTime = ""
                            'ElapsedTime1 = ""
                            'ElapsedTime2 = ""
                            'RemainingTime = ""
                            'RemainingTime1 = ""
                            'RemainingTime2 = ""
                            'DueAsof = ""
                            'DueAsof1 = ""
                            'DueAsof2 = ""
                            'ATACode = ObjCompStatus.ATACode
                            'InstalledAt1 = ""
                            'InstalledAt2 = ""
                            'TSN = ""
                            ''TSO1 = ""
                            'TSO = ""
                            'TSO2 = ""
                            'RemoveAt = ""
                            'RemoveAt1 = ""
                            'RemoveAt2 = ""
                            'InstalledAtDate.Text = ObjCompStatus.InstalledOn
                            'RemoveAtDate.Text = ""
                            'DoneRemrk = ""
                            'AssemblyID = ObjAssemblyStatus.AssemblyID
                            'DoneOnValue = ""
                            'DoneOnDate.Text = ""
                            'ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, , Description, _
                            ', EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , _
                            ', , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g")))
                            'Return ReportMaintenanceDetails
                            'Exit For
                        End If

                        For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                            If (chkApplicable.Checked = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (chkApplicable.Checked = False) Then
                                If ServiceTypeID.Contains(ObjCompMonitorServiceStatus.PartMonitorServiceTypeID) Then
                                    MaintenanceActivityTypeID = 1 'Added By Vikrant On 04-Jan-2018 For ALL04012019
                                    ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                    ATACode = ObjCompMonitorServiceStatus.ATACode
                                    Description = ObjCompMonitorServiceStatus.Description
                                    PartNo = ObjCompStatus.PartName
                                    CompSerialNo = ObjCompStatus.CompSerialNo
                                    Position = ObjCompStatus.Position
                                    MonitorTypeCode = ObjCompMonitorServiceStatus.Code

                                    If ObjCompMonitorServiceStatus.MonitorTypeID = 3 Then 'Added by Saylee on 15-May-2015 for No-Freq record
                                        EstimatedDate = ""
                                    Else
                                        EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                    End If

                                    MonitorType = ObjCompMonitorServiceStatus.Type
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    Freq1 = ""
                                    Freq2 = ""
                                    Freq3 = ""
                                    ElapsedTime = ""
                                    ElapsedTime1 = ""
                                    ElapsedTime2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    ATACode = ObjCompMonitorServiceStatus.ATACode
                                    'InstalledAt1 = ""
                                    InstalledAt2 = ""
                                    TSN = ""
                                    'TSO1 = ""
                                    TSO = ""
                                    TSO2 = ""
                                    RemoveAt = ""
                                    RemoveAt1 = ""
                                    RemoveAt2 = ""
                                    InstalledAtDate.Text = ObjCompStatus.InstalledOn
                                    RemoveAtDate.Text = ""
                                    DoneRemrk = ObjCompMonitorServiceStatus.DoneRemark
                                    DoneOnValue = ""
                                    DoneOnDate.Text = ""
                                    For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                        If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                            Freq1 = Freq1 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                ElapsedTime = ""
                                                RemainingTime = ""
                                                DueAsof = ""
                                            Else
                                                ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame '
                                            End If
                                            TSN = TSN & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
                                            If ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" And ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 Then
                                                TSO = "N/A"
                                            Else
                                                TSO = TSO & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                            End If
                                            'TSO1 = TSO1 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDoneOnValueTextFormattedByAirFrame
                                            RemoveAt = RemoveAt & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValue
                                            If DoneOnValue = "" Then
                                                DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                            Else
                                                DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                            End If

                                        End If
                                        If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                            If Freq1 = "" Then
                                                Freq1 = Freq1 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    RemoveAtDate.Text = ""
                                                    DoneOnDate.Text = ""
                                                Else
                                                    ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    'DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                        DueAsof = DueAsof & vbCrLf & ""
                                                    Else
                                                        DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame '
                                                    End If
                                                    RemoveAtDate.Text = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                End If

                                                'TSN = TSN & vbCrLf & ""


                                                If ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" And ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 Then
                                                    TSO = "N/A"
                                                Else
                                                    TSO = TSO & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                                End If
                                                'TSO1 = TSO1 & vbCrLf & ""
                                                RemoveAt = RemoveAt & vbCrLf & ""
                                                'commented and chenged by Saylee on 31-Dec-2015
                                                'DoneOnValue = DoneOnValue & vbCrLf & " "

                                                If DoneOnValue = "" Then
                                                    DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                End If
                                            Else
                                                Freq1 = Freq1 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    RemoveAtDate.Text = ""
                                                    DoneOnDate.Text = ""
                                                Else
                                                    ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    'DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                        DueAsof = DueAsof & vbCrLf & ""
                                                    Else
                                                        DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame '
                                                    End If
                                                    RemoveAtDate.Text = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                End If
                                                ' TSN = TSN & vbCrLf & " "
                                                If ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" And ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 Then
                                                    TSO = "N/A"
                                                Else
                                                    TSO = TSO & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                                End If
                                                'TSO1 = TSO1 & vbCrLf & " "
                                                RemoveAt = RemoveAt & vbCrLf & " "
                                                'commented and chenged by Saylee on 31-Dec-2015
                                                'DoneOnValue = DoneOnValue & vbCrLf & " "
                                                If DoneOnValue = "" Then
                                                    DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                End If
                                            End If
                                        End If
										'Added PeriodID=11,15 By Vikrant For ALL 21062012
										'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 10 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Then
										'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
										If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
											If Freq1 = "" Then
												Freq1 = Freq1 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
												If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
													ElapsedTime = ""
													RemainingTime = ""
													DueAsof = ""
												Else
													ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
													RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
													If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
													Else
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
													End If
												End If
												TSN = TSN & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
												If ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" And ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 Then
													TSO = "N/A"
												Else
													TSO = TSO & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
												End If
												'TSO1 = TSO1 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
												RemoveAt = RemoveAt & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValue

												If DoneOnValue = "" Then
													DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue
												Else
													DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
												End If
											Else
												Freq1 = Freq1 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
												If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
													ElapsedTime = ""
													RemainingTime = ""
													DueAsof = ""
												Else
													ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.AllElapsedValue

													RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
													If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
													Else
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
													End If
												End If
												TSN = TSN & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
												If ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" And ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 Then
													TSO = "N/A"
												Else
													TSO = TSO & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
												End If
												'TSO1 = TSO1 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
												RemoveAt = RemoveAt & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValue

												If DoneOnValue = "" Then
													DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue
												Else
													DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
												End If
											End If
										End If
									Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjCompMonitorServiceStatus.Notes
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, ObjMachine.RegNo, , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                    , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , _
                                    , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g"), MaintenanceTypeID:=MaintenanceActivityTypeID))
                                End If
                            End If
                        Next

                        'Added By Vikrant On 04-Jan-2018 For ALL04012019
                        For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                            If (chkApplicable.Checked = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (chkApplicable.Checked = False) Then
                                If InspTypeID.Contains(ObjCompMonitorInspStatus.PartMonitorInspTypeID) Then
                                    MaintenanceActivityTypeID = 2
                                    ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                    ATACode = ObjCompMonitorInspStatus.ATACode
                                    Description = ObjCompMonitorInspStatus.Description
                                    PartNo = ObjCompStatus.PartName
                                    CompSerialNo = ObjCompStatus.CompSerialNo
                                    Position = ObjCompStatus.Position
                                    MonitorTypeCode = ObjCompMonitorInspStatus.Code

                                    If ObjCompMonitorInspStatus.MonitorTypeID = 3 Then 'Added by Saylee on 15-May-2015 for No-Freq record
                                        EstimatedDate = ""
                                    Else
                                        EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                    End If

                                    MonitorType = ObjCompMonitorInspStatus.Type
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    Freq1 = ""
                                    Freq2 = ""
                                    Freq3 = ""
                                    ElapsedTime = ""
                                    ElapsedTime1 = ""
                                    ElapsedTime2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    'InstalledAt1 = ""
                                    InstalledAt2 = ""
                                    TSN = ""
                                    'TSO1 = ""
                                    TSO = ""
                                    TSO2 = ""
                                    RemoveAt = ""
                                    RemoveAt1 = ""
                                    RemoveAt2 = ""
                                    InstalledAtDate.Text = ObjCompStatus.InstalledOn
                                    RemoveAtDate.Text = ""
                                    DoneRemrk = ObjCompMonitorInspStatus.DoneRemark
                                    DoneOnValue = ""
                                    DoneOnDate.Text = ""
                                    For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                        If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
                                            Freq1 = Freq1 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                            If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime = ""
                                                RemainingTime = ""
                                                DueAsof = ""
                                            Else
                                                ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
                                                DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame '
                                            End If
                                            TSN = TSN & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
                                            'If ObjCompMonitorInspStatusPeriod.DoneOnValue = "" And ObjCompMonitorInspStatus.PartMonitorInspTypeID = 1 Then
                                            '    TSO = "N/A"
                                            'Else
                                            '    TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
                                            'End If
                                            TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
                                            'TSO1 = TSO1 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDoneOnValueTextFormattedByAirFrame
                                            RemoveAt = RemoveAt & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
                                            If DoneOnValue = "" Then
                                                DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                            Else
                                                DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
                                            End If

                                        End If
                                        If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                            If Freq1 = "" Then
                                                Freq1 = Freq1 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    RemoveAtDate.Text = ""
                                                    DoneOnDate.Text = ""
                                                Else
                                                    ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    'DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                        DueAsof = DueAsof & vbCrLf & ""
                                                    Else
                                                        DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame '
                                                    End If
                                                    RemoveAtDate.Text = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                    DoneOnDate.Text = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                End If

                                                'TSN = TSN & vbCrLf & ""


                                                'If ObjCompMonitorInspStatusPeriod.DoneOnValue = "" And ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 Then
                                                '    TSO = "N/A"
                                                'Else
                                                '    TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
                                                'End If
                                                TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
                                                'TSO1 = TSO1 & vbCrLf & ""
                                                RemoveAt = RemoveAt & vbCrLf & ""
                                                'commented and chenged by Saylee on 31-Dec-2015
                                                'DoneOnValue = DoneOnValue & vbCrLf & " "

                                                If DoneOnValue = "" Then
                                                    DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                End If
                                            Else
                                                Freq1 = Freq1 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    RemoveAtDate.Text = ""
                                                    DoneOnDate.Text = ""
                                                Else
                                                    ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    'DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                        DueAsof = DueAsof & vbCrLf & ""
                                                    Else
                                                        DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame '
                                                    End If
                                                    RemoveAtDate.Text = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                    DoneOnDate.Text = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                End If
                                                ' TSN = TSN & vbCrLf & " "
                                                'If ObjCompMonitorInspStatusPeriod.DoneOnValue = "" And ObjCompMonitorInspStatus.PartMonitorServiceTypeID = 1 Then
                                                '    TSO = "N/A"
                                                'Else
                                                '    TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
                                                'End If
                                                TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
                                                'TSO1 = TSO1 & vbCrLf & " "
                                                RemoveAt = RemoveAt & vbCrLf & " "
                                                'commented and chenged by Saylee on 31-Dec-2015
                                                'DoneOnValue = DoneOnValue & vbCrLf & " "
                                                If DoneOnValue = "" Then
                                                    DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                End If
                                            End If
                                        End If
										'Added PeriodID=11,15 By Vikrant For ALL 21062012
										'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 10 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Then
										'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
										If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then
											If Freq1 = "" Then
												Freq1 = Freq1 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
												If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
													ElapsedTime = ""
													RemainingTime = ""
													DueAsof = ""
												Else
													ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValue
													RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
													If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
													Else
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
													End If
												End If
												TSN = TSN & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
												'If ObjCompMonitorInspStatusPeriod.DoneOnValue = "" And ObjCompMonitorInspStatus.PartMonitorServiceTypeID = 1 Then
												'    TSO = "N/A"
												'Else
												'    TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
												'End If
												TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
												'TSO1 = TSO1 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
												RemoveAt = RemoveAt & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue

												If DoneOnValue = "" Then
													DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue
												Else
													DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
												End If
											Else
												Freq1 = Freq1 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
												If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
													ElapsedTime = ""
													RemainingTime = ""
													DueAsof = ""
												Else
													ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValue

													RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
													If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
													Else
														DueAsof = DueAsof & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
													End If
												End If
												TSN = TSN & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
												'If ObjCompMonitorInspStatusPeriod.DoneOnValue = "" And ObjCompMonitorInspStatus.PartMonitorServiceTypeID = 1 Then
												'    TSO = "N/A"
												'Else
												'    TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
												'End If
												TSO = TSO & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedAtInstall
												'TSO1 = TSO1 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
												RemoveAt = RemoveAt & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue

												If DoneOnValue = "" Then
													DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue
												Else
													DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
												End If
											End If
										End If
									Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjCompMonitorInspStatus.Notes
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, ObjMachine.RegNo, , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                    , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , _
                                    , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g"), MaintenanceTypeID:=MaintenanceActivityTypeID))
                                End If
                            End If
                        Next
                        'End
                    End If
                    
                Next
            Next
        Next
        CType(ReportMaintenanceDetails, ReportMaintenanceDetailList).Sort("PartNo", ComponentModel.ListSortDirection.Ascending)
        Return ReportMaintenanceDetails
    End Function
#End Region

#Region " Events "
    Private Sub wfrptPartWiseComponentStatus_Ajax_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            ClearAll()
            Session("MiddleFrame") = "wfrptPartWiseComponentStatus_Ajax.aspx?"
            SetFocus(txtSearch)
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            cmbSerialNo.Enabled = False
            chkListServiceType.Enabled = False
            chkListServiceType.Visible = False
            'Added By Vikrant On 04-Jan-2018 For ALL04012019
            chkListInspType.Enabled = False
            chkListInspType.Visible = False
            'End
            DataFieldBind()
        End If
    End Sub
    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        'If (PartID.Value.Length > 0 And PartName.Value.Length > 0) Then

        'mPartID = IIf(PartID.Value.Length > 0, PartID.Value, Guid.Empty.ToString)
        'mPartName = IIf(PartName.Value.Length > 0, PartName.Value.Substring(0, PartName.Value.ToString.IndexOf("[") - 1), "")

        If (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            PartNo = txtSearch.Text.Trim
            Description = txtSearch.Text.Trim
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        End If
        mPartList = PartList.GetPartList(PartNo)
        If mPartList.Count > 0 Then
            mPartID = mPartList(0).ID.ToString
            mPartName = PartNo
        Else
            mPartID = Guid.Empty.ToString
            mPartName = ""
        End If

        If mPartID = Guid.Empty.ToString Then
            cmbSerialNo.Enabled = False
        Else
            mPartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(mPartName, "", txtAsOnDate.Text, , "(All)")

            If mPartListForSerialNos.Count = 0 Then
                cmbSerialNo.Enabled = False
                cmbSerialNo.ClearSelection()
            Else
                cmbSerialNo.DataSource = mPartListForSerialNos
                cmbSerialNo.DataBind()
                cmbSerialNo.Enabled = True
                Session("mCompList") = mPartListForSerialNos
                SetFocus(txtSearch)
                DataFieldBind()
                chkListServiceType.Enabled = IIf(chkListServiceType.Items.Count > 0, True, False)
                chkListServiceType.Visible = IIf(chkListServiceType.Items.Count > 0, True, False)
                'Added By Vikrant On 04-Jan-2018 For ALL04012019
                chkListInspType.Enabled = IIf(chkListInspType.Items.Count > 0, True, False)
                chkListInspType.Visible = IIf(chkListInspType.Items.Count > 0, True, False)
                'End
            End If
        End If
        'End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim partlist As PartListAutoComplete
        partlist = PartListAutoComplete.GetPartList(prefixText)
        If count = 0 Then
            Return (From c As PartListAutoCompleteInfo In partlist
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Part, c.ID.ToString())).ToArray
        Else
            Return (From c As PartListAutoCompleteInfo In partlist
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Part, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region
End Class