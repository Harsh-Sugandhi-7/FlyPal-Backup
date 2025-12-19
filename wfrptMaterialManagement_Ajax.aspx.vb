'Added By Vikrant On 02-Apr-2018 For BA02042018

Imports System.Collections.Generic
Imports Flypal.ItemListAutoComplete
Imports System.Linq

Public Class wfrptMaterialManagement_Ajax
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
    Public mMachineNameValueList As MachineNameValueList
    Public mPartList As PartList
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Public mPartID, mPartName As String
    Public mMachineList As MachineList
    Public PeriodLimt As String = String.Empty
    Public searchstr2 As String = String.Empty

    Public ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Public ReportStatusList As New rptStatusList
    Public mPerDayLimits As PerDayLimits

    Dim FromDate, ToDate As String
    Dim Periodcount As Integer
    Dim AssemblyID As Guid
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
    Private Freq1, Freq2, Freq3 As String
    Private ElapsedTime, ElapsedTime1, ElapsedTime2 As String
    Private RemainingTime, RemainingTime1, RemainingTime2 As String
    Private DueAsof, DueAsof1, DueAsof2 As String
    Private AssemblyModel As String
    Private ATACode As Integer = 0
    Private InstalledAt, InstalledAt1, InstalledAt2 As String
    Private TSN, TSO, TSO1, TSO2 As String
    Private RemoveAt, RemoveAt1, RemoveAt2, SerialNoPostion, DoneRemrk As String
    Private InstalledAtDate As SmartDate = New SmartDate(True)
    Private RemoveAtDate As SmartDate = New SmartDate(True)
    Private DoneOnValue As String
    Private DoneOnDate As SmartDate = New SmartDate(True)
    Private CompStatusID As Guid
    Public EventLogID As Guid
    Dim EventLogDetail As String = String.Empty
    Dim ServiceTypeID As Integer()
    Public mPartNo As String = String.Empty
    Public mDescription As String = String.Empty
#End Region

#Region " Business Methods "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        custValidator.ControlToValidate = "txtSearch"
        If txtSearch.Text = "" Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            mPartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            mDescription = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
            If mPartNo = "" Or mDescription = "" Then
                e.IsValid = False
            End If
            mPartList = PartList.GetPartList(mPartNo)
            If mPartList.Count <= 0 Then
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptMaterialManagement_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPerDayLimits")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub GetSession()
        mPerDayLimits = Session("mPerDayLimits")
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(txtFromDate.Text, , , , , , , True, "(All)", , True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataSource = mMachineNameValueList

        If (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            mPartNo = txtSearch.Text.Trim
            mDescription = txtSearch.Text.Trim
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            mPartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            mDescription = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        End If
        mPartList = PartList.GetPartList(mPartNo)
        If mPartList.Count > 0 Then
            mPartID = mPartList(0).ID.ToString
            mPartName = mPartNo
        Else
            mPartID = Guid.Empty.ToString
            mPartName = ""
        End If

        'mPartID = IIf(PartID.Value.Length > 0, PartID.Value, Guid.Empty.ToString)
        'mPartName = IIf(PartName.Value.Length > 0, PartName.Value, "")

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
            PeriodLimt = PeriodLimt + ", " + Trim(txtPerDatLimit.Text) + " " + Me.gdPerDayLimit.Rows(i1).Cells(1).Text
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
    Private Sub Display()
        lblDateRange.Visible = True
        lblComponent1.Visible = True
        lblAC.Visible = True
        lblEstimated.Visible = True
        upnlSearchingCriteria.Update()
    End Sub
    Public Sub SetValues()
        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString

        If Not IsDate(txtFromDate.Text.Trim) Then
            FromDate = ""
        Else
            FromDate = txtFromDate.Text.Trim
        End If

        If Not IsDate(txtToDate.Text.Trim) Then
            ToDate = ""
        Else
            ToDate = txtToDate.Text.Trim
        End If

        lblDateRange.Text = "From Date : " & txtFromDate.Text.Trim & " & To Date : " & txtToDate.Text.Trim
        lblComponent1.Text = "Part : " & txtSearch.Text.Trim
        lblAC.Text = "Aircraft : " & IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.ToString, "All")

        SetGridObject()
        lblEstimated.Text = "Estimated as per day flying : " & PeriodLimt.Trim(",")
        'End
        EventLogDetail = lblDateRange.Text + "," + lblComponent1.Text + "," + lblAC.Text + "," + lblEstimated.Text
    End Sub
    Public Sub SetReport()
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList

        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMaterialManagementReport

        Dim rptCompStatus As New CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim mCompanyDetail As New CompanyDetail

        rptCompStatus = New crptMaterialMgmtReportBA

        SetPerDayLimitValues()
        SetValues()
        ReportDetail()

        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1393)
        End If

        Dim ShortFall As Decimal = ReportMaintenanceDetails.Count - (CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).ServiceableStockQty + CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).UnserviceableStockQty + CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).EROQtyForMaterialMgmtReport + CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).POQtyForMaterialMgmtReport)
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    mCompanyDetail.WebSite, "Material Management For Lifed Items", txtFromDate.Text.Trim, searchstr2, txtToDate.Text, _
    IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, ""), txtSearch.Text.Trim, AppSettings("Product Version"), AppSettings("SINote"), , _
    "Total Required : " & ReportMaintenanceDetails.Count.ToString, "Total SV : " & CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).ServiceableStockQty.ToString, _
    "Total US : " & CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).UnserviceableStockQty.ToString, AppSettings("Logo"), _
    "On RO : " & CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).EROQtyForMaterialMgmtReport.ToString, _
    "On PO : " & CType(ReportMaintenanceDetails(0), ReportMaintenanceDetail).POQtyForMaterialMgmtReport.ToString, _
    "Shortfall for this period : " & IIf(ShortFall > 0, ShortFall, 0).ToString, IIf(chkCheckForAlternatePart.Checked, mPartList(0).PartNameWithAlternateParts.Replace(mPartList(0).Name + ",", "").Replace(mPartList(0).Name, ""), ""))

        

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
        MarkLog(Util.Action.Print, "MaterialManagementForLifedItems", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Public Function CompareEstimatedDate(ByVal EstimatedDate As String) As Boolean
        If IsDate(EstimatedDate) And IsDate(txtToDate.Text) Then
            If CDate(EstimatedDate) <= CDate(txtToDate.Text) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Public Function FindMatchInStringArray(ByVal StrArray As String(), ByVal strToCompare As String) As Boolean
        For i As Integer = 0 To StrArray.Length - 1
            If strToCompare.Equals(StrArray(i)) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim mItem As Item
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo
        Dim mPartHistoryBinCardServiceable As rptPartHistoryBinCardServiceableUnserviceableList
        'Dim mPartHistoryBinCardUnServiceable As rptPartHistoryBinCardServiceableUnserviceableList


        If (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            mPartNo = txtSearch.Text.Trim
            mDescription = txtSearch.Text.Trim
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            mPartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            mDescription = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        End If


        mPartList = PartList.GetPartList(mPartNo)
        If mPartList.Count > 0 Then
            mPartID = mPartList(0).ID.ToString
            mPartName = mPartNo
        Else
            mPartID = Guid.Empty.ToString
            mPartName = ""
        End If

        ServiceTypeID = {1, 2}

        mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(txtFromDate.Text, cmbAircraft.SelectedValue.ToString, , , , , , , , , , True, True, , , , , , , mPartNo, "", , , , , , , , , , , , , , , , False, , False, , True, , , , , , IsAverageRequired:=True, AverageMonths:=0, CompMonitoringServiceRequired:=True, ByPerDayLimit:=True, PerdayLimits:=mPerDayLimits, MonitorServiceTypeIDs:="1,2", PartNames:=IIf(chkCheckForAlternatePart.Checked, mPartList(0).PartNameWithAlternateParts, ""))

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
                                If InstalledAt1 = "" Then
                                    InstalledAt1 = ""
                                Else
                                    InstalledAt1 = InstalledAt1 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompStartValueFormatted
                                End If
                            End If
                        Next
                        ObjCompStatus.CompMonitorServiceStatusList.Sort("MinimumRemainingValue", ComponentModel.ListSortDirection.Ascending)
                        For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                            If ServiceTypeID.Contains(ObjCompMonitorServiceStatus.PartMonitorServiceTypeID) And Not CType(ReportMaintenanceDetails, ReportMaintenanceDetailList).Contains(ObjCompStatus.ID) And CompareEstimatedDate(ObjCompMonitorServiceStatus.EstimatedDateFormatted.ToString) Then
                                ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                ATACode = ObjCompMonitorServiceStatus.ATACode
                                Description = ObjCompMonitorServiceStatus.Description
                                PartNo = ObjCompStatus.PartName
                                mDescription = ObjCompStatus.PartDescription
                                CompSerialNo = ObjCompStatus.CompSerialNo
                                Position = ObjCompStatus.Position
                                CompStatusID = ObjCompStatus.ID
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
                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, mDescription, _
                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , _
                                , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g")))
                            End If
                        Next
                    End If
                    
                Next
            Next
        Next

        If ReportMaintenanceDetails.Count > 0 Then
            Dim PartToCompare As String = ""
            Dim recieptqty As Decimal = 0.0
            Dim issqty As Decimal = 0.0
            Dim RCIItemID1 As Guid = Guid.Empty
            Dim RCIItemID As Guid = Guid.Empty

            CType(ReportMaintenanceDetails, ReportMaintenanceDetailList).Sort("ATACode", ComponentModel.ListSortDirection.Ascending)
            CType(ReportMaintenanceDetails, ReportMaintenanceDetailList).Sort("PartNo", ComponentModel.ListSortDirection.Ascending)


            For i As Integer = 0 To ReportMaintenanceDetails.Count - 1
                If Not PartToCompare.Equals(CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).PartNo) Then
                    PartToCompare = CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).PartNo
                    mPartHistoryBinCardServiceable = rptPartHistoryBinCardServiceableUnserviceableList.GetPartHistoryBinCardServiceableUnserviceableList(CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).PartNo, CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).Description, "", Guid.Empty, Guid.Empty, False, chkIsValued.Checked, 0, WithAlternatePatrs:=chkCheckForAlternatePart.Checked)
                    'mPartHistoryBinCardServiceable = rptPartHistoryBinCardServiceableUnserviceableList.GetPartHistoryBinCardServiceableUnserviceableList(CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).PartNo, CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).Description, "", Guid.Empty, Guid.Empty, False, chkIsValued.Checked, 2, WithAlternatePatrs:=chkCheckForAlternatePart.Checked)
                    mItem = Item.GetItemByName(CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).PartNo)

                    Dim ServCount As Decimal = (From StatusInfo As rptPartHistoryBinCardServiceableUnserviceableList.PartHistoryBinCardServiceableUnserviceableListInfo In mPartHistoryBinCardServiceable
                                                    Where StatusInfo.PartStatusID = 1 And StatusInfo.IssueQty <= 0
                                                    Select StatusInfo.ReceiptQty).Sum()
                    Dim UnServCount As Decimal = (From StatusInfo As rptPartHistoryBinCardServiceableUnserviceableList.PartHistoryBinCardServiceableUnserviceableListInfo In mPartHistoryBinCardServiceable
                                                    Where StatusInfo.PartStatusID = 2 And StatusInfo.IssueQty <= 0
                                                    Select StatusInfo.ReceiptQty).Sum()
                    'recieptqty = (From StatusInfo As rptPartHistoryBinCardServiceableUnserviceableList.PartHistoryBinCardServiceableUnserviceableListInfo In mPartHistoryBinCardServiceable
                    '                                Where (StatusInfo.PartStatusID = 1 Or StatusInfo.PartStatusID = 2) And StatusInfo.ReceiptTransTypeID = 6 Or StatusInfo.ReceiptTransTypeID = 7 Or StatusInfo.ReceiptTransTypeID = 22 Or _
                    '                                      ((StatusInfo.ReceiptTransTypeID = 67 Or StatusInfo.ReceiptTransTypeID = 10) And StatusInfo.IsConsiderAsAsset = True) Or _
                    '                                      (StatusInfo.ReceiptTransTypeID = 9 And StatusInfo.IsConsiderAsAsset = True)
                    '                                      Select StatusInfo.ReceiptQty).Sum()
                    'issqty = (From StatusInfo As rptPartHistoryBinCardServiceableUnserviceableList.PartHistoryBinCardServiceableUnserviceableListInfo In mPartHistoryBinCardServiceable
                    '                                Where (StatusInfo.PartStatusID = 1 Or StatusInfo.PartStatusID = 2) And _
                    '                                (StatusInfo.IssueTransTypeID = 19 Or StatusInfo.IssueTransTypeID = 25 Or ((StatusInfo.IssueTransTypeID = 14 Or StatusInfo.IssueTransTypeID = 44) And StatusInfo.IsCapitalize = True))
                    '                                      Select StatusInfo.IssueQty).Sum()
                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                        For k As Integer = 0 To mPartHistoryBinCardServiceable.Count - 1
                            If mPartHistoryBinCardServiceable(k).ReceiptTransTypeID = 6 Or _
                            mPartHistoryBinCardServiceable(k).ReceiptTransTypeID = 7 Or _
                            mPartHistoryBinCardServiceable(k).ReceiptTransTypeID = 22 Or _
                           ((mPartHistoryBinCardServiceable(k).ReceiptTransTypeID = 67 Or mPartHistoryBinCardServiceable(k).ReceiptTransTypeID = 10) And mPartHistoryBinCardServiceable(k).IsConsiderAsAsset = True) Or _
                           (mPartHistoryBinCardServiceable(k).ReceiptTransTypeID = 9 And mPartHistoryBinCardServiceable(k).IsConsiderAsAsset = True And mItem.PrimaryCategoryID = 1) Then
                                If mPartHistoryBinCardServiceable(k).PartStatusID = 1 Then
                                    If Not RCIItemID1.Equals(mPartHistoryBinCardServiceable(k).RCIItemID) Then
                                        RCIItemID1 = mPartHistoryBinCardServiceable(k).RCIItemID
                                        recieptqty = recieptqty + mPartHistoryBinCardServiceable(k).ReceiptQty
                                    End If
                                End If
                                If mPartHistoryBinCardServiceable(k).PartStatusID = 2 Then
                                    If Not RCIItemID.Equals(mPartHistoryBinCardServiceable(k).RCIItemID) Then
                                        RCIItemID = mPartHistoryBinCardServiceable(k).RCIItemID
                                        recieptqty = recieptqty + mPartHistoryBinCardServiceable(k).ReceiptQty
                                    End If
                                End If
                            End If
                            If (mPartHistoryBinCardServiceable(k).IssueTransTypeID = 19 Or mPartHistoryBinCardServiceable(k).IssueTransTypeID = 25 Or ((mPartHistoryBinCardServiceable(k).IssueTransTypeID = 14 Or mPartHistoryBinCardServiceable(k).IssueTransTypeID = 44) And mPartHistoryBinCardServiceable(k).IsCapitalize = True)) Then
                                issqty = issqty + mPartHistoryBinCardServiceable(k).IssueQty
                            End If
                        Next
                    Else
                        For j As Integer = 0 To mPartHistoryBinCardServiceable.Count - 1
                            If mPartHistoryBinCardServiceable(j).PartStatusID = 1 Then
                                If Not RCIItemID1.Equals(mPartHistoryBinCardServiceable(j).RCIItemID) Then
                                    RCIItemID1 = mPartHistoryBinCardServiceable(j).RCIItemID
                                    recieptqty = recieptqty + mPartHistoryBinCardServiceable(j).ReceiptQty
                                End If
                            End If
                            If mPartHistoryBinCardServiceable(j).PartStatusID = 2 Then
                                If Not RCIItemID.Equals(mPartHistoryBinCardServiceable(j).RCIItemID) Then
                                    RCIItemID = mPartHistoryBinCardServiceable(j).RCIItemID
                                    recieptqty = recieptqty + mPartHistoryBinCardServiceable(j).ReceiptQty
                                End If
                            End If
                            If mPartHistoryBinCardServiceable(j).IssueStatusID = 2 Then
                                issqty = issqty + mPartHistoryBinCardServiceable(j).IssueQty
                            End If
                        Next
                    End If
                    CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).ServiceableStockQty = ServCount
                    CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).UnserviceableStockQty = UnServCount
                    CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).BinCardTotalQty = IIf(recieptqty - issqty > 0, Format(recieptqty - issqty, "##0.##"), 0)
                    If mPartHistoryBinCardServiceable.Count > 0 Then
                        CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).EROQtyForMaterialMgmtReport = mPartHistoryBinCardServiceable(0).EROQtyForMaterialMgmtReport
                        CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).ERONosForMaterialMgmtReport = mPartHistoryBinCardServiceable(0).ERONosForMaterialMgmtReport
                        CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).POQtyForMaterialMgmtReport = mPartHistoryBinCardServiceable(0).POQtyForMaterialMgmtReport
                        CType(ReportMaintenanceDetails(i), ReportMaintenanceDetail).PONosForMaterialMgmtReport = mPartHistoryBinCardServiceable(0).PONosForMaterialMgmtReport
                    End If
                End If
                
            Next


        End If
        Return ReportMaintenanceDetails
    End Function
#End Region

#Region " Events "
    Private Sub wfrptMaterialManagement_Ajax_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            ClearAll()
            Session("MiddleFrame") = "wfrptMaterialManagement_Ajax.aspx?"
            SetFocus(txtSearch)
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.AddMonths(6).ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        'If (PartID.Value.Length > 0 And PartName.Value.Length > 0) Then

        'mPartID = IIf(PartID.Value.Length > 0, PartID.Value, Guid.Empty.ToString)
        'mPartName = IIf(PartName.Value.Length > 0, PartName.Value.Substring(0, PartName.Value.ToString.IndexOf("[") - 1), "")
        If (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            mPartNo = txtSearch.Text.Trim
            mDescription = txtSearch.Text.Trim
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            mPartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            mDescription = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        End If
        mPartList = PartList.GetPartList(mPartNo)
        If mPartList.Count > 0 Then
            mPartID = mPartList(0).ID.ToString
            mPartName = mPartNo
        Else
            mPartID = Guid.Empty.ToString
            mPartName = ""
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        Else
            upnlValidationSummary.Update()
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
        Dim partlist As ItemListAutoComplete
        partlist = ItemListAutoComplete.GetItemList(prefixText, PrimaryCategoryID:=1, IsLifeComponent:=1)
        If count = 0 Then
            Return (From c As ItemListAutoCompleteInfo In partlist
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoCompleteInfo In partlist
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region
End Class