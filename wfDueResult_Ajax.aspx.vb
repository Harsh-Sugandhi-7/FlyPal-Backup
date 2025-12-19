'Added By Vikrant On 28-Mar-2014
Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.DashBoardWO

Public Class wfDueResult_Ajax
    Inherits Page

#Region " Variable Declaration "

    Dim ReportMaintenanceDetails As ReportMaintenanceDetailList
    Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)
    'Added By Vikrant On 03-Jun-2016 For ALL03062016
    Dim mMachine As Machine
    Dim mBoardInfo As AircraftInformationBoard.BoardInfo
    Dim SearchCriteriaValues As New Hashtable
    Dim Periodcount, Count, DueStatus, AvgMnths, AssemblyTypeID, DueType, DocumentTypeForID, ServiceTypeID(50), InspectionTypeID(50), ModificationTypeID(50) As Integer
    Dim Type As Integer = 1
    Dim AssemblyID, StatusMasterID, StatusID As Guid
    Dim MachineName, mMonitorDetail, mAircraft, mMonitorInfo, mMonitorType, mMonitorDesc, percent, searchstr7, DoneOnDate, AsonDate, nWONumber, ATAChapter, RegNo, Extension, Extension1, Remark, Customer, Code, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, AssemblyType, AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Model, AssemblySerialNo, PartNo, CompSerialNo, Position, MonitorTypeCode, Note, Description, SerialNo, EstimatedDate, Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, SinceNew, SinceNew1, SinceNew2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, DoneAt, DoneAt1, DoneAt2, AssemblyModel, MaintenanceEvent As String
    Dim MinimumRemainingValue As Decimal
    Dim mtmpMachineList As tmpMachineList
    Dim Report As Integer = 1
    Dim mnWOListForDueJobs As nWOListForDueJobs
    Dim ReportStatusList As New rptStatusList
    Dim IschkwithWONoChecked As Boolean = True
    Dim mUpdateComplyHistoryAssemblyMonitorInspStatusList As UpdateComplyHistoryAssemblyMonitorInspStatusList
    Dim mUpdateComplyHistoryAssemblyMonitorServiceStatusList As UpdateComplyHistoryAssemblyMonitorServiceStatusList
    Dim mUpdateComplyHistoryAssemblyMonitorModStatusList As UpdateComplyHistoryAssemblyMonitorModStatusList
    Dim mUpdateComplyHistoryCompMonitorInspStatusList As UpdateComplyHistoryCompMonitorInspStatusList
    Dim mUpdateComplyHistoryCompMonitorModStatusList As UpdateComplyHistoryCompMonitorModStatusList
    Dim mUpdateComplyHistoryCompMonitorServiceStatusList As UpdateComplyHistoryCompMonitorServiceStatusList
    Dim EventLogID As Guid
    'End
    'Added By Vikrant on 14-Jun-2018 For ALL14062018
    Private mnWO As nWO
    Private checkedIds As New List(Of String)()
    Dim mMachineNameValueList As MachineNameValueList
    Dim AsOnDateForWOCreation, MachineIDForWOCreation As String
    'End
    Dim mSpareListByMaintenanceActivity As SpareListByMaintenanceActivity
    Public mMaintenanceKit As MaintenanceKit
    Public mMaintenanceTask As MaintenanceTask
    Public mLinkMaintenanceList As LinkMaintenanceList
    Dim rptMachineCertificates As MachineCertificateList  'Added by Shital on 09-Nov-2020 For Add Print in Preview Button
    Dim rptSnagCorrectiveActionListForDue As MELSnagCorrectiveActionListForDue
    Dim mModuleList As ModuleList
    Dim mTransactionList As TransactionList
    Dim CompletedByUserLicenceNos As String = String.Empty
    Dim AirframeHrsAsOnCompletionDate As String = String.Empty
    Dim AFAllPeriodsAsOnCompletionDate As String = String.Empty
    Dim mFileAttachnWO As FileAttach
    Dim chkMaintenanceTypeList As Object
    Dim MaintenanceTypeValues As String()
    Dim ServiceCount As Integer
    Dim InspectionCount As Integer
    Dim ModificationCount As Integer

#End Region

#Region " Enumeration "

    Private Enum Rights

        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        Authorized = 7
        Completed = 8 'Added By Vikrant on 30-Jun-2021 For ALL30062021

    End Enum

#End Region

#Region "Business Methods"

    Private Sub GetSession()

        SearchCriteriaValues = CType(Session("SearchCriteriaValues"), Hashtable) 'Added By Vikrant On 03-Jun-2016 For ALL03062016
        ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
        reportmaintdetailslist = Session("reportmaintdetailslist")
        'Added By Vikrant on 14-Jun-2018 For ALL14062018
        mMachineNameValueList = Session("mMachineNameValueList")
        AsOnDateForWOCreation = Session("AsOnDateForWOCreation")
        MachineIDForWOCreation = Session("MachineIDForWOCreation")
        'End
        'Added by Shital on 09-Nov-2020 For Add Print in Preview Button
        rptMachineCertificates = Session("rptMachineCertificates")
        mModuleList = Session("mModuleList")
        rptSnagCorrectiveActionListForDue = Session("rptSnagCorrectiveActionListForDue")
        ReportStatusList = Session("ReportStatusList")
        '----
        mTransactionList = Session("mTransactionList")

    End Sub

    'Added By Vikrant On 03-Jun-2016 For ALL03062016
    Private Sub RemoveSession()

        Session.Remove("ReportMaintenanceDetails")
        Session.Remove("reportmaintdetailslist")

    End Sub

    Public Function ReportDetail() As ReportMaintenanceDetailList

        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
        Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
        Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
        Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
        Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
        Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
        Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo
        Dim mDueLimits As DueLimits
        Dim mMachineList As MachineList

        Try

            mDueLimits = CType(SearchCriteriaValues("DueLimitObj"), DueLimits)

            If CBool(SearchCriteriaValues("IsrbdPercentChecked")) Then mDueLimits.SetPercentageWise(True, CDec(SearchCriteriaValues("Percentage")))

            mMachineList = MachineList.GetMachineListDueMonitoringStatus(CStr(SearchCriteriaValues("AsonDate")), mDueLimits, SearchCriteriaValues("MachineID").ToString, SearchCriteriaValues("AssemblyID").ToString, CInt(SearchCriteriaValues("AverageMonths")), CBool(SearchCriteriaValues("IsSpecifyValuesChecked")), CType(SearchCriteriaValues("PerDayLimitsObj"), PerDayLimits), , CBool(SearchCriteriaValues("IsServiceRequired")), CBool(SearchCriteriaValues("IsInspRequired")), CBool(SearchCriteriaValues("IsModRequired")), , , , CBool(SearchCriteriaValues("IsServiceRequired")), CBool(SearchCriteriaValues("IsInspRequired")), CBool(SearchCriteriaValues("IsModRequired")), CInt(SearchCriteriaValues("ForDueStatus")), True, SkipIsForInventoryAircarft:=True)
            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""

            If Not CStr(SearchCriteriaValues("SelectedAircraftText")) = "(All)" Then
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                        LHLabel2 = ""
                        LHData2 = ""
                        For Count = 0 To Periodcount - 1
                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                                LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                            End If
                        Next
                        AssemblyID = ObjAssemblyStatus.AssemblyID
                    Next
                Next
            End If

            If Not CStr(SearchCriteriaValues("SelectedAircraftText")) = "(All)" Then
                mtmpMachineList = tmpMachineList.GetMachineList(, CStr(SearchCriteriaValues("Aircraft")), , , , , True, AsonDate)
                Dim mOtherPeriodExists As String = "False"

                For i As Integer = 0 To mtmpMachineList.Count - 1
                    If mtmpMachineList(i).AllPeriods <> "" Then
                        mOtherPeriodExists = "True"
                        Exit For
                    End If
                Next

                For i As Integer = 0 To mtmpMachineList.Count - 1
                    searchstr7 = mtmpMachineList(i).Owner.ToString  ' Changed By Utkarsh On 11-Apr-2011 '"Owner/Operator :- " +
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , mtmpMachineList(i).TSO, , mtmpMachineList(i).CSO, , , , , , , , , mtmpMachineList(i).Cycles, mtmpMachineList(i).AllPeriods.Replace("<BR>", vbCrLf), mOtherPeriodExists, Year(CDate(SearchCriteriaValues("AsonDate"))).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                    Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted
                Next

            End If

            If CBool(SearchCriteriaValues("IsServiceRequired")) Then
                ServiceTypeID = DirectCast(SearchCriteriaValues("ServiceTypeID"), Integer())
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        If Not Session("IsBackFromWO") Is Nothing Then
                            IschkwithWONoChecked = True
                        Else
                            IschkwithWONoChecked = CBool(SearchCriteriaValues("IschkwithWONoChecked"))
                        End If

                        For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList
                            'loop through selected monitory types
                            If ServiceTypeID.Contains(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceTypeID) Then
                                If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then
                                    If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorServiceStatus.Description
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorServiceStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID  '11-Sep-2008
                                        DueStatus = ObjAssemblyMonitorServiceStatus.DueStatus
                                        DocumentTypeForID = 0
                                        '  Remark = ObjAssemblyMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorServiceStatus.DoneRemark
                                        Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode  'Added By Saylee on 28-08-2008
                                        DoneOnDate = ObjAssemblyMonitorServiceStatus.DoneOn  'Added By Saylee 2-Aug-2012
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

                                        AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                        AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                        AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                        SinceNew = ""
                                        SinceNew1 = ""
                                        SinceNew2 = ""
                                        DoneAt = ""
                                        DoneAt1 = ""
                                        DoneAt2 = ""
                                        MaintenanceEvent = ""

                                        'Added by Saylee 04-08-2008
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""

                                        For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList
                                            If Report = 1 Then  'Portarait
                                                If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                                    RemainingTime = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                    DueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                    'Added By Shweta 7-June-2012
                                                    'AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                    DoneAt = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                    If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                        AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                    ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                        AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    Else
                                                        AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                    End If
                                                    '**********************************
                                                    SinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue

                                                    'Added by Saylee 04-08-2008
                                                    Extension = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                                End If
                                                If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    DueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    AssemblyDueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted  'Added By DEVEN On 14/06/2008
                                                    SinceNew1 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt1 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    'Added by Saylee 04-08-2008
                                                    Extension1 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                End If
												'Added PeriodID=11 By Vikrant For ALL 21062012
												'If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjAssemblyMonitorServiceStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
														RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
														DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														'Added By Prashant 26-Jun-2013 BA26062013
														If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
															AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
														Else
															AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue  'Added By DEVEN On 14/06/2008
														End If
														SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
														DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
														'Added by Saylee 04-08-2008
														Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														'Added By Prashant 26-Jun-2013 BA26062013
														If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
														Else
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														End If

														SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
														'Added by Saylee 04-08-2008
														Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
													End If
												End If
											End If
                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        Note = ObjAssemblyMonitorServiceStatus.Notes
                                        RegNo = ObjMachine.RegNo
                                        'Rajnish 08-08-2008
                                        RequiredManHours = ModelMonitorService.GetModelMonitorService(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID).RequiredManHours
                                        Customer = ObjMachine.Customer
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                        'MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type

                                        Dim TaskNo As String = ""
                                        Dim TaskNoMaint As String = ""
                                        If AppSettings("ShowMaintenanceForNewClients") = True And ObjAssemblyMonitorServiceStatus.TaskNo <> "" Then
                                            ' TaskNo = IIf(IsExcel, Chr(10), vbCrLf) & "Task No. : " & ObjAssemblyMonitorServiceStatus.TaskNo
                                            TaskNo = ObjAssemblyMonitorServiceStatus.TaskNo
                                            TaskNoMaint = "Task No. : " & ObjAssemblyMonitorServiceStatus.TaskNo
                                        End If

                                        'If ObjAssemblyMonitorServiceStatus.Reference <> "" Then
                                        '    MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")" & vbCrLf & ObjAssemblyMonitorServiceStatus.Reference
                                        'Else
                                        '    MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")"
                                        'End If

                                        If ObjAssemblyMonitorServiceStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")" & TaskNoMaint & vbCrLf & ObjAssemblyMonitorServiceStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode <> "", vbCrLf & " (" & ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode & ")", ""), "")
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")" & TaskNoMaint & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode <> "", vbCrLf & " (" & ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode & ")", ""), "")
                                        End If

                                        'Added by Saylee 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark
                                        StatusID = ObjAssemblyMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        'If IschkwithWONoChecked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End If





                                        'If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorServiceStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                  SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1,
                  AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorServiceStatus.IsApplicable, StatusID.ToString _
                  , AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=5, IsMaster:=ObjAssemblyMonitorServiceStatus.IsMaster, TaskNo:=TaskNo))
                                    End If
                                End If
                            End If
                        Next

                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                                If ServiceTypeID.Contains(ObjCompMonitorServiceStatus.PartMonitorServiceTypeID) Then
                                    If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then
                                        If (ObjCompMonitorServiceStatus.IsApplicable = True) And (Not (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True)) Then
                                            ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                            Description = ObjCompMonitorServiceStatus.Description
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                            EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorServiceStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                            StatusMasterID = ObjCompMonitorServiceStatus.PartMonitorServiceID  '11-Sep-2008
                                            DueStatus = ObjCompMonitorServiceStatus.DueStatus
                                            DocumentTypeForID = 0
                                            'Remark = ObjCompMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                            DoneOnDate = ObjCompMonitorServiceStatus.DoneOn  'Added By Saylee 2-Aug-2012
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

                                            AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                            SinceNew = ""
                                            SinceNew1 = ""
                                            SinceNew2 = ""
                                            DoneAt = ""
                                            DoneAt1 = ""
                                            DoneAt2 = ""
                                            MaintenanceEvent = ""

                                            'Added by Saylee 04-08-2008
                                            Extension = ""
                                            Extension1 = ""
                                            Extension2 = ""

                                            For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                                If Report = 1 Then 'Portarait
                                                    If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                        Freq1 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                        ElapsedTime = ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                        RemainingTime = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                        'AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                        'Added By Shweta 7-June-2012
                                                        DoneAt = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                            AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            If DoneOnDate <> "" Then DoneAt = ObjCompMonitorServiceStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                            AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        Else
                                                            AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                                        End If
                                                        '**********************************
                                                        DueAsof = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                        SinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValue

                                                        'Added by Saylee 04-08-2008
                                                        Extension = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                    End If
                                                    If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                        Freq2 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime1 = ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime1 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                        DueAsof1 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        SinceNew1 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt1 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        'Added by Saylee 04-08-2008
                                                        Extension1 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    End If
													'Added PeriodID=11 By Vikrant For ALL 21062012
													'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValue
															RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
															'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
															'AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
																AssemblyDueAsof2 = "" 'ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															Else
																'Added By Prashant 26-Jun-2013 BA26062013
																If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																	AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
																Else
																	AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																End If
															End If
															'***************
															DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
															SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
															DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
															'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
															'AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ""  'AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															Else
																'Added By Prashant 26-Jun-2013 BA26062013
																If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																	AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
																Else
																	AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																End If
															End If
															'****************************
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
															SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValue
															DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValue
														End If
													End If
												End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            'Rajnish 08-08-2008
                                            RequiredManHours = PartMonitorService.GetPartMonitorService(ObjCompMonitorServiceStatus.PartMonitorServiceID).RequiredManHours
                                            Customer = ObjMachine.Customer
                                            Note = ObjCompMonitorServiceStatus.Notes
                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjCompMonitorServiceStatus.Type

                                            Dim TaskNo As String = ""
                                            Dim TaskNoMaint As String = ""
                                            If AppSettings("ShowMaintenanceForNewClients") = True And ObjCompMonitorServiceStatus.TaskNo <> "" Then
                                                ' TaskNo = IIf(IsExcel, Chr(10), vbCrLf) & "Task No. : " & ObjCompMonitorServiceStatus.TaskNo
                                                TaskNo = ObjCompMonitorServiceStatus.TaskNo
                                                TaskNoMaint = "Task No. : " & ObjCompMonitorServiceStatus.TaskNo
                                            End If

                                            'If ObjCompMonitorServiceStatus.Reference <> "" Then
                                            '    MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")" & vbCrLf & ObjCompMonitorServiceStatus.Reference
                                            'Else
                                            '    MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")"
                                            'End If
                                            If ObjCompMonitorServiceStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")" & TaskNoMaint & vbCrLf & ObjCompMonitorServiceStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorServiceStatus.PartMonitorServiceCode <> "", vbCrLf & " (" & ObjCompMonitorServiceStatus.PartMonitorServiceCode & ")", ""), "")
                                            Else
                                                MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")" & TaskNoMaint & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorServiceStatus.PartMonitorServiceCode <> "", vbCrLf & " (" & ObjCompMonitorServiceStatus.PartMonitorServiceCode & ")", ""), "")
                                            End If


                                            'Added by Saylee 04-08-2008
                                            ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark

                                            StatusID = ObjCompMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1

                                            'If IschkwithWONoChecked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                            mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                            If mnWOListForDueJobs.Count > 0 Then
                                                nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                            Else
                                                nWONumber = ""
                                            End If
                                            'End If



                                            'If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorServiceStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                  AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorServiceStatus.IsApplicable, StatusID.ToString, CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=8, IsMaster:=ObjCompMonitorServiceStatus.IsMaster, TaskNo:=TaskNo))
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            End If

            If CBool(SearchCriteriaValues("IsInspRequired")) Then
                InspectionTypeID = DirectCast(SearchCriteriaValues("InspectionTypeID"), Integer())
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                            If InspectionTypeID.Contains(ObjAssemblyMonitorInspStatus.ModelMonitorInspTypeID) Then
                                If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                    If (ObjAssemblyMonitorInspStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorInspStatus.Description
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorInspStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                        StatusMasterID = ObjAssemblyMonitorInspStatus.ModelMonitorInspID  '11-Sep-2008
                                        DueStatus = ObjAssemblyMonitorInspStatus.DueStatus
                                        DocumentTypeForID = 9
                                        DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOn  'Added By Saylee 2-Aug-2012
                                        Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                                        'Remark = ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008

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

                                        AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                        AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                        AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                        SinceNew = ""
                                        SinceNew1 = ""
                                        SinceNew2 = ""
                                        DoneAt = ""
                                        DoneAt1 = ""
                                        DoneAt2 = ""

                                        'Added by Saylee 04-08-2008
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""
                                        MaintenanceEvent = ""
                                        For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                            If Report = 1 Then 'Portarait
                                                If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                    RemainingTime = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                    DueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                    'AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                    'Added By Shweta 7-June-2012

                                                    DoneAt = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                        AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorInspStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                    ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                        AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame

                                                    Else
                                                        AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                    End If
                                                    '**********************************

                                                    SinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue

                                                    'Added by Saylee 04-08-2008
                                                    Extension = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                                End If
                                                If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                    DueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                    AssemblyDueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                    SinceNew1 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt1 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    'Added by Saylee 04-08-2008
                                                    Extension1 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                                End If
												'Added PeriodID=11 By Vikrant For ALL 21062012
												'If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjAssemblyMonitorInspStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
														RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
														DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														'Added By Prashant 26-Jun-2013 BA26062013
														If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
															AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame 'AssemblyDueOnValueByAirFrame
														Else
															AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														End If

														SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
														DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
														'Added by Saylee 04-08-2008
														Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														'Added By Prashant 26-Jun-2013 BA26062013
														If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueByAirFrame
														Else
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														End If

														SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
														'Added by Saylee 04-08-2008
														Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
													End If
												End If
											End If
                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        'Rajnish 08-08-2008
                                        RequiredManHours = ModelMonitorInsp.GetModelMonitorInsp(ObjAssemblyMonitorInspStatus.ModelMonitorInspID).RequiredManHours
                                        Customer = ObjMachine.Customer
                                        Note = ObjAssemblyMonitorInspStatus.Notes
                                        'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                        'MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type
                                        If ObjAssemblyMonitorInspStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")" & vbCrLf & ObjAssemblyMonitorInspStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")"
                                        End If


                                        'Added by Saylee 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark

                                        StatusID = ObjAssemblyMonitorInspStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1

                                        'If IschkwithWONoChecked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End If

                                        'If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                           , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                           SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                           AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorInspStatus.IsApplicable, StatusID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=6, IsMaster:=ObjAssemblyMonitorInspStatus.IsMaster))
                                    End If
                                End If
                            End If
                        Next
                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                                If InspectionTypeID.Contains(ObjCompMonitorInspStatus.PartMonitorInspTypeID) Then
                                    If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                        If (ObjCompMonitorInspStatus.IsApplicable = True) And (Not (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True)) Then
                                            ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                            Description = ObjCompMonitorInspStatus.Description
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorInspStatus.Code
                                            EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorInspStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                            StatusMasterID = ObjCompMonitorInspStatus.PartMonitorInspID  '11-Sep-2008
                                            DueStatus = ObjCompMonitorInspStatus.DueStatus
                                            DocumentTypeForID = 11

                                            'Remark = ObjCompMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Code = ObjCompMonitorInspStatus.PartMonitorInspCode
                                            DoneOnDate = ObjCompMonitorInspStatus.DoneOn  'Added By Saylee 2-Aug-2012

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

                                            AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                            SinceNew = ""
                                            SinceNew1 = ""
                                            SinceNew2 = ""
                                            DoneAt = ""
                                            DoneAt1 = ""
                                            DoneAt2 = ""
                                            MaintenanceEvent = ""

                                            'Added By Saylee on 04-08-2008
                                            Extension = ""
                                            Extension1 = ""
                                            Extension2 = ""

                                            For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                                If Report = 1 Then 'Portarait
                                                    If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
                                                        Freq1 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                        ElapsedTime = ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                        RemainingTime = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                        'AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                        'Added By Shweta 7-June-2012

                                                        DoneAt = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                            AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame   'Added By Saylee 2-Aug-2012
                                                            If DoneOnDate <> "" Then DoneAt = ObjCompMonitorInspStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                            AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        Else
                                                            AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                                        End If
                                                        '**********************************

                                                        DueAsof = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                        SinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValue

                                                        'Added By Saylee on 04-08-2008
                                                        Extension = ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                    End If
                                                    If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                        Freq2 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime1 = ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime1 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                        DueAsof1 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        SinceNew1 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt1 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        'Added By Saylee on 04-08-2008
                                                        Extension1 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                    End If
													'Added PeriodID=11 By Vikrant For ALL 21062012
													'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValue
															RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
															'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
															'AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
																AssemblyDueAsof2 = "" 'ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															Else
																'Added By Prashant 26-Jun-2013 BA26062013
																If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																	AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
																Else
																	AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																End If

															End If
															'*****************
															DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
															SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValue
															DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
															'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
															'AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & "" 'AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															Else
																'Added By Prashant 26-Jun-2013 BA26062013
																If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																	AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
																Else
																	AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText  'Added By DEVEN On 14/06/2008
																End If

															End If
															'**********************
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
															SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValue
															DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValue
														End If
													End If

												End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            'Rajnish 08-08-2008
                                            RequiredManHours = PartMonitorInsp.GetPartMonitorInsp(ObjCompMonitorInspStatus.PartMonitorInspID).RequiredManHours
                                            Customer = ObjMachine.Customer

                                            Note = ObjCompMonitorInspStatus.Notes

                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjCompMonitorInspStatus.Type
                                            If ObjCompMonitorInspStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")" & vbCrLf & ObjCompMonitorInspStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")"
                                            End If

                                            '*********************************
                                            'Added By Saylee on 04-08-2008
                                            ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark

                                            StatusID = ObjCompMonitorInspStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1

                                            'If IschkwithWONoChecked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                            mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                            If mnWOListForDueJobs.Count > 0 Then
                                                nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                            Else
                                                nWONumber = ""
                                            End If
                                            'End If

                                            'If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                                 , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                 AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorInspStatus.IsApplicable, StatusID.ToString, CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=9, IsMaster:=ObjCompMonitorInspStatus.IsMaster))
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            End If

            If CBool(SearchCriteriaValues("IsModRequired")) Then
                ModificationTypeID = DirectCast(SearchCriteriaValues("ModificationTypeID"), Integer())
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList
                            If ModificationTypeID.Contains(ObjAssemblyMonitorModStatus.ModelMonitorModTypeID) Then
                                If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                    If (ObjAssemblyMonitorModStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                                        'Commented and changed by Saylee on 10-Oct-2013 for ALL10102013
                                        'Description = ObjAssemblyMonitorModStatus.Description & vbCrLf & ObjAssemblyMonitorModStatus.ModificationNumber & vbCrLf & ObjAssemblyMonitorModStatus.Reference
                                        Description = ObjAssemblyMonitorModStatus.Description & vbCrLf & ObjAssemblyMonitorModStatus.Number
                                        '****************************
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorModStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                        StatusMasterID = ObjAssemblyMonitorModStatus.ModelMonitorModID  '11-Sep-2008                        
                                        DueStatus = ObjAssemblyMonitorModStatus.DueStatus
                                        DocumentTypeForID = 8

                                        'Remark = ObjAssemblyMonitorModStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorModStatus.DoneRemark 'Added By Saylee on 20-08-2008
                                        Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode
                                        DoneOnDate = ObjAssemblyMonitorModStatus.DoneOn  'Added By Saylee 2-Aug-2012
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

                                        AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                        AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                        AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                        SinceNew = ""
                                        SinceNew1 = ""
                                        SinceNew2 = ""
                                        DoneAt = ""
                                        DoneAt1 = ""
                                        DoneAt2 = ""
                                        MaintenanceEvent = ""

                                        'Added By Saylee on 04-08-2008
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""

                                        For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                                            If Report = 1 Then 'Portarait
                                                If ObjAssemblyMonitorModStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                                    RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                    DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                    'AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                    'Added By Shweta 7-June-2012

                                                    DoneAt = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                    If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                        AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorModStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                    ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                        AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    Else
                                                        AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue

                                                    End If
                                                    '**********************************
                                                    SinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue

                                                    'Added By Saylee on 04-08-2008
                                                    Extension = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                                End If
                                                If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                    DueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                    AssemblyDueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                    SinceNew1 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt1 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                    'Added By Saylee on 04-08-2008
                                                    Extension1 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                                End If
												'Added PeriodID=11 By Vikrant For ALL 21062012
												'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjAssemblyMonitorModStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
														RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
														DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
														'Added By Prashant 26-Jun-2013 BA26062013
														If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
															AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueByAirFrame
														Else
															AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														End If

														SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
														DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
														'Added By Saylee on 04-08-2008
														Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValue
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
														'Added By Prashant 26-Jun-2013 BA26062013
														If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueByAirFrame
														Else
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														End If

														SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
														'Added By Saylee on 04-08-2008
														Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ExtensionValue
													End If
												End If
											End If
                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        'Rajnish 08-08-2008
                                        RequiredManHours = ModelMonitorMod.GetModelMonitorMod(ObjAssemblyMonitorModStatus.ModelMonitorModID).RequiredManHours
                                        Customer = ObjMachine.Customer

                                        Note = ObjAssemblyMonitorModStatus.Notes
                                        'Added by Saylee on 10-Oct-2013 for ALL10102013
                                        'MaintenanceEvent = ObjAssemblyMonitorModStatus.Type 
                                        If ObjAssemblyMonitorModStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")" & vbCrLf & ObjAssemblyMonitorModStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")"
                                        End If


                                        '*************************
                                        'Added By Saylee on 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorModStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorModStatus.ApprovalRemark

                                        StatusID = ObjAssemblyMonitorModStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1


                                        'If IschkwithWONoChecked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End If

                                        'If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                           , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                           SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , ObjAssemblyMonitorModStatus.Number, , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorModStatus.IsApplicable, StatusID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=7, IsMaster:=ObjAssemblyMonitorModStatus.IsMaster))
                                    End If
                                End If

                            End If
                        Next
                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                                If ModificationTypeID.Contains(ObjCompMonitorModStatus.PartMonitorModTypeID) Then
                                    If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                        If (ObjCompMonitorModStatus.IsApplicable = True) And (Not (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted)) Then
                                            ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'Description = ObjCompMonitorModStatus.Description & vbCrLf & ObjCompMonitorModStatus.Number & vbCrLf & ObjCompMonitorModStatus.Reference
                                            Description = ObjCompMonitorModStatus.Description & vbCrLf & ObjCompMonitorModStatus.Number
                                            '**********************************
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorModStatus.Code
                                            EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorModStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                            StatusMasterID = ObjCompMonitorModStatus.PartMonitorModID  '11-Sep-2008                        
                                            DueStatus = ObjCompMonitorModStatus.DueStatus
                                            DocumentTypeForID = 10

                                            'Remark = ObjCompMonitorModStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorModStatus.DoneRemark    'Added By Saylee on 20-08-2008
                                            Code = ObjCompMonitorModStatus.PartMonitorModCode
                                            DoneOnDate = ObjCompMonitorModStatus.DoneOn  'Added By Saylee 2-Aug-2012
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

                                            AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                            SinceNew = ""
                                            SinceNew1 = ""
                                            SinceNew2 = ""
                                            DoneAt = ""
                                            DoneAt1 = ""
                                            DoneAt2 = ""
                                            MaintenanceEvent = ""

                                            'Added By Saylee on 04-08-2008
                                            Extension = ""
                                            Extension1 = ""
                                            Extension2 = ""
                                            For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                                If Report = 1 Then 'Portarait
                                                    If ObjCompMonitorModStatusPeriod.PeriodID = 1 Then
                                                        Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                                        ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue
                                                        RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                                        'AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                        'Added By Shweta 7-June-2012
                                                        DoneAt = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then    'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                            AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            If DoneOnDate <> "" Then DoneAt = ObjCompMonitorModStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                            AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        Else
                                                            AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                        End If
                                                        '**********************************

                                                        DueAsof = ObjCompMonitorModStatusPeriod.DueOnValue
                                                        SinceNew = ObjCompMonitorModStatusPeriod.CompCurrentValue

                                                        'Added By Saylee on 04-08-2008
                                                        Extension = ObjCompMonitorModStatusPeriod.ExtensionValue
                                                    End If
                                                    If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                        Freq2 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime1 = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime1 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof1 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                        DueAsof1 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                        SinceNew1 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt1 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                        'Added By Saylee on 04-08-2008
                                                        Extension1 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                    End If
													'Added PeriodID=11 By Vikrant For ALL 21062012
													'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 9 Or ObjCompMonitorModStatusPeriod.PeriodID = 12 Or ObjCompMonitorModStatusPeriod.PeriodID = 13 Or ObjCompMonitorModStatusPeriod.PeriodID = 14 Or ObjCompMonitorModStatusPeriod.PeriodID = 15 Or ObjCompMonitorModStatusPeriod.PeriodID = 11 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjCompMonitorModStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValue
															RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
															'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
															'AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
																AssemblyDueAsof2 = ""  'ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															Else
																'Added By Prashant 26-Jun-2013 BA26062013
																If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																	AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
																Else
																	AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																End If

															End If
															'******************
															DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
															SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValue
															DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValue
															'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
															'AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & "" 'AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															Else
																'Added By Prashant 26-Jun-2013 BA26062013
																If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																	AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
																Else
																	AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																End If

															End If
															'***********************
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
															SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorModStatusPeriod.CompCurrentValue
															DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = Extension2 & vbCrLf & ObjCompMonitorModStatusPeriod.ExtensionValue
														End If
													End If

												End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            'Rajnish 08-08-2008
                                            RequiredManHours = PartMonitorMod.GetPartMonitorMod(ObjCompMonitorModStatus.PartMonitorModID).RequiredManHours
                                            Customer = ObjMachine.Customer

                                            Note = ObjCompMonitorModStatus.Notes

                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjCompMonitorModStatus.Type
                                            If ObjCompMonitorModStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")" & vbCrLf & ObjCompMonitorModStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")"
                                            End If

                                            '***************************************
                                            'Added By Saylee on 04-08-2008
                                            ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark

                                            StatusID = ObjCompMonitorModStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1


                                            'If IschkwithWONoChecked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                            mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                            If mnWOListForDueJobs.Count > 0 Then
                                                nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                            Else
                                                nWONumber = ""
                                            End If
                                            'End If

                                            'If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                  AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , ObjCompMonitorModStatus.Number, , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorModStatus.IsApplicable, StatusID.ToString, , CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=10, IsMaster:=ObjCompMonitorModStatus.IsMaster))
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            End If
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (ReportDetail): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
        Return ReportMaintenanceDetails
    End Function

    Private Sub RefreshGrid()
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList

        ReportDetail()
        reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
                                  Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                  Select c).ToList
        Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
        Session("reportmaintdetailslist") = reportmaintdetailslist
    End Sub

    Private Sub GridBind()
        If cmbAircraft.SelectedValue.Equals(Guid.Empty.ToString) Then
            dgDueJob.DataSource = reportmaintdetailslist
            dgDueJob.DataBind()
            lblDuePeriodList.Text = "Due Job List : " & reportmaintdetailslist.Count.ToString & " record(s)"
            SetGrid()
        Else
            Dim mJobs = (From c As ReportMaintenanceDetail In reportmaintdetailslist
                         Where (c.RegNo.ToUpper().Contains(cmbAircraft.SelectedItem.ToString.ToUpper))
                         Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                         Select c).ToList
            dgDueJob.DataSource = mJobs
            dgDueJob.DataBind()
            lblDuePeriodList.Text = "Due Job List : " & mJobs.Count.ToString & " record(s)"
            SetGrid()
        End If
    End Sub

    Private Sub ComplyAssemblyInspection(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal ModelID As Guid, ByVal MonitorDetail As String)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(MaintID, AssemblyStatusID, mMachine.HourType)
        If (mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And (mPrevAssemblyMonitorInspStatus.IsCompleted Or mPrevAssemblyMonitorInspStatus.FetchRecordCount(mPrevAssemblyMonitorInspStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, SearchCriteriaValues("AsonDate").ToString, ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
            Session("From") = 0 'New record

            mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            'RemoveSession()

            'Added by Saylee on 22-May-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            '''Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).Description
            MarkLog(Util.Action.Comply, "AssemblyInspections", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
            Session("IsBackFromCompliance") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorInspStatus_Ajax.aspx?GChildPage2=wfDueResult_Ajax.aspx');", True)
        End If
    End Sub

    Private Sub ComplyAssemblyService(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal ModelID As Guid, ByVal MonitorDetail As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(MaintID, AssemblyStatusID, mMachine.HourType)
        If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
            MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, SearchCriteriaValues("AsonDate").ToString, ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
            Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
            Session("From") = 0 'New record
            ''
            mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
            Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            ''NewMachineMaintenance(mAssemblyStatus, mAssemblyMonitorServiceStatus.ID)

            'Added by Saylee on 22-May-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End

            '''Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).Description

            'RemoveSession()
            Session("IsBackFromCompliance") = "True"
            MarkLog(Util.Action.Comply, "AssemblyServiceMonitor", MonitorDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyAssemblyMonitorServiceStatus_Ajax.aspx?GChildPage2=wfDueResult_Ajax.aspx'); ", True)
        End If
    End Sub

    Private Sub ComplyAssemblyDirective(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal ModelID As Guid, ByVal MonitorDetail As String, ByVal IsApplicable As Boolean)
        mMachine = Machine.GetMachine(MachineID)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(MaintID, AssemblyStatusID, mMachine.HourType)
        If (mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And (mPrevAssemblyMonitorModStatus.IsCompleted Or mPrevAssemblyMonitorModStatus.FetchRecordCount(mPrevAssemblyMonitorModStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Not IsApplicable Then
            MSGBoxCtrl.show(MSGBox.Message_title.MonitoringNotApplicable, MSGBox.Message_text.MonitoringNotApplicable, "You are trying to comply the record.Directives monitoring is not applicable, can not be complied.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, SearchCriteriaValues("AsonDate").ToString, ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus

            MarkLog(Util.Action.Comply, "AssemblyModifications", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)

            Session("From") = 0 'New record
            ''
            mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus

            'Added by Saylee on 22-May-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            'Added By Vikrant On 25-Nov-2014
            'Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorModStatus.ID) 'Sort = 1 : Installation
            'Session("mFileAttach") = mFileAttach
            'End

            '''Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).Description
            'RemoveSession()
            Session("IsBackFromCompliance") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyAssemblyMonitorModStatus_Ajax.aspx?GChildPage2=wfDueResult_Ajax.aspx');", True)
        End If
    End Sub

    Private Sub ComplyCompInspection(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal ModelID As Guid, ByVal MonitorDetail As String, ByVal CompStatusID As Guid, ByVal DoneOn As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(MaintID, AssemblyStatusID, CompStatusID, mMachine.HourType)
        If (mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And (mPrevCompMonitorInspStatus.IsCompleted Or mPrevCompMonitorInspStatus.FetchRecordCount(mPrevCompMonitorInspStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, SearchCriteriaValues("AsonDate").ToString, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
            Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
            Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
            Session("EnFrom") = 0 'New record

            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
            Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn)
            Session("mMachine") = mMachine
            Session("mCompStatus") = mCompStatus
            Session("mAssemblyStatus") = mAssemblyStatus
            'Rajnish 21-07-2008
            mCompMonitorInspStatus.RequiredManHours = mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours
            Session("mCompMonitorInspStatus") = mCompMonitorInspStatus

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorInspStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End

            'Added by Saylee on 5-Aug-2009
            '''mCompInfo = mTmpComplyCompMonitorInspStatusList.Item(index).MachineInfo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).AssemblyInfo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).PartSerialNo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).Reference + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).PartMonitorInspInfo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).ATA.ToString + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).Description
            '''Session("mCompInfo") = mTmpComplyCompMonitorInspStatusList.Item(index).MachineInfo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).AssemblyInfo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).PartSerialNo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).Reference + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).PartMonitorInspInfo + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).ATA.ToString + "->" + mTmpComplyCompMonitorInspStatusList.Item(index).Description

            'Added By Utkarsh On 28-Jul-2011 For All19072011
            '''MaintDetail = "Reg No. : " + mTmpComplyCompMonitorInspStatusList(index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorInspStatusList(index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorInspStatusList(index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorInspStatusList(index).TypeDet & " Done On Date : " & mTmpComplyCompMonitorInspStatusList(index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorInspStatusList(index).DoneOnValueFormatted
            MarkLog(Util.Action.Comply, "ComponentInspections", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
            'End

            'RemoveSession()
            Session("IsBackFromCompliance") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfComplyCompMonitorInspStatus_AJAX.aspx?GChildPage2=wfDueResult_Ajax.aspx');", True)
        End If
    End Sub

    Private Sub ComplyCompService(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal ModelID As Guid, ByVal MonitorDetail As String, ByVal CompStatusID As Guid, ByVal DoneOn As String)
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(MaintID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, SearchCriteriaValues("AsonDate").ToString, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
            Session("EnFrom") = 0 'NewRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorServiceStatusList(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
            Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn)
            Session("mMachine") = mMachine
            Session("mCompStatus") = mCompStatus
            Session("mAssemblyStatus") = mAssemblyStatus
            'Rajnish 21-07-2008
            mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End

            'RemoveSession()
            'Added by Saylee on 5-Aug-2009
            'mCompInfo = mTmpComplyCompMonitorServiceStatusList.Item(index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).Description
            'Session("mCompInfo") = mTmpComplyCompMonitorServiceStatusList.Item(index).MachineInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).AssemblyInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).PartSerialNo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).Reference + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).PartMonitorServiceInfo + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).ATA.ToString + "->" + mTmpComplyCompMonitorServiceStatusList.Item(index).Description
            ''*****************************************

            'Added By Utkarsh On 28-Jul-2011 For All19072011

            '''MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(index).DoneOnValueFormatted
            MarkLog(Util.Action.Comply, "ComponentServiceMonitor", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
            Session("IsBackFromCompliance") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorServiceStatus_AJAX.aspx?GChildPage2=wfDueResult_Ajax.aspx');", True)
        End If
    End Sub

    Private Sub ComplyCompModification(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal ModelID As Guid, ByVal MonitorDetail As String, ByVal CompStatusID As Guid, ByVal DoneOn As String, ByVal IsApplicable As Boolean)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorModStatus As CompMonitorModStatus
        Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(MaintID, AssemblyStatusID, CompStatusID, mMachine.HourType)
        If (mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And (mPrevCompMonitorModStatus.IsCompleted Or mPrevCompMonitorModStatus.FetchRecordCount(mPrevCompMonitorModStatus.ID) > 1)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf IsApplicable = False Then
            REM: If IsApplicable of Part Monitor Mod is not checked then it can not be complied
            MSGBoxCtrl.show(MSGBox.Message_title.MonitoringNotApplicable, MSGBox.Message_text.MonitoringNotApplicable, "Monitoring modification is not applicable, can not be complied.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, SearchCriteriaValues("AsonDate").ToString, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
            Session("EnFrom") = 0 'ComplyRecord
            'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
            Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn)
            Session("mMachine") = mMachine
            Session("mCompStatus") = mCompStatus
            Session("mAssemblyStatus") = mAssemblyStatus
            'Rajnish 21-07-2008
            mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
            Session("mCompMonitorModStatus") = mCompMonitorModStatus

            'Added By Vikrant On 25-Nov-2014
            Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID) 'Sort = 1 : Installation
            Session("mFileAttach") = mFileAttach
            'End

            'RemoveSession()
            'mCompInfo = mTmpComplyCompMonitorModStatusList.Item(index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(index).Description
            'Session("mCompInfo") = mTmpComplyCompMonitorModStatusList.Item(index).MachineInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).AssemblyInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).PartSerialNo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).ModNumber + "->" + mTmpComplyCompMonitorModStatusList.Item(index).Reference + "->" + mTmpComplyCompMonitorModStatusList.Item(index).PartMonitorModInfo + "->" + mTmpComplyCompMonitorModStatusList.Item(index).ATA.ToString + "->" + mTmpComplyCompMonitorModStatusList.Item(index).Description

            'Added By Utkarsh On 28-Jul-2011 For All19072011
            MarkLog(Util.Action.Comply, "ComponentModifications", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
            'End

            ''MarkLog(Util.Action.[New], "ComplyCompMonitorModStatus", mCompInfo + "   " + ComplyCompMonitorModInfo, Util.ErrorType.NoError, Guid.Empty)
            Session("IsBackFromCompliance") = "True"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfComplyCompMonitorModStatus_AJAX.aspx?GChildPage2=wfDueResult_Ajax.aspx');", True)
        End If
    End Sub

    Private Sub AssInspHistoryRecord(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal MonitorDetail As String) 'Added by Saylee on 09-Sep-2009
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(MaintID, AssemblyStatusID, mMachine.HourType)

        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)

        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorInspStatusList.Item(index).MachineInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).Reference + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorInspStatusList.Item(index).Description


        mUpdateComplyHistoryAssemblyMonitorInspStatusList = UpdateComplyHistoryAssemblyMonitorInspStatusList.GetComplyHistoryAssemblyMonitorInspStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorInspStatus.ModelMonitorInspID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList

        'RemoveSession()
        'Added by Vikrant on 3-Aug-2011
        MarkLog(Util.Action.View, "AssemblyInspections", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionHistoryWindow", "OpenInspectionHistoryWindow();", True)
        'End If
    End Sub

    Private Sub AssServiceHistoryRecord(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal MonitorDetail As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(MaintID, AssemblyStatusID, mMachine.HourType)

        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        'Added by Saylee on 29-June-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************
        'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorServiceStatusList.Item(index).MachineInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).Reference + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorServiceStatusList.Item(index).Description

        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList


        RemoveSession()
        'Added by Vikrant on 3-Aug-2011
        MarkLog(Util.Action.View, "AssemblyServiceMonitor", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow();", True)
        'End If
    End Sub

    Private Sub AssDirectiveHistoryRecord(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal MonitorDetail As String)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(MaintID, AssemblyStatusID, mMachine.HourType)
        If mPrevAssemblyMonitorModStatus.IsMaster Then
            MSGBoxCtrl.Show("Master Record!", "There is no history for this record", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
            Session("From") = 1 'Edit record
            ''
            ' Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
            Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
            Session("mMachine") = mMachine
            Session("mAssemblyStatus") = mAssemblyStatus
            'Added by Saylee on 29-June-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************
            'Session("mAssemblyInfo") = mTmpComplyAssemblyMonitorModStatusList.Item(index).MachineInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).ModelSerialNo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).Reference + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).ModNumber + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).MonitorInfo + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).ATA.ToString + "->" + mTmpComplyAssemblyMonitorModStatusList.Item(index).Description

            mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorModStatus.ModelMonitorModID, mMachine.HourType)
            Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList

            RemoveSession()
            'Added by Vikrant on 3-Aug-2011
            MarkLog(Util.Action.View, "AssemblyModifications", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDirectiveHistoryWindow", "OpenDirectiveHistoryWindow();", True)
        End If
    End Sub

    Private Sub CompServiceHistoryRecord(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal CompStatusID As Guid, ByVal DoneOn As String, ByVal MonitorDetail As String)
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(MaintID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        Session("EnFrom") = 1 'EditRecord

        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        RemoveSession()

        mUpdateComplyHistoryCompMonitorServiceStatusList = UpdateComplyHistoryCompMonitorServiceStatusList.GetComplyHistoryCompMonitorServiceStatusList(mCompStatus.CompID, mCompMonitorServiceStatus.PartMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorServiceStatusList") = mUpdateComplyHistoryCompMonitorServiceStatusList

        'Added By Utkarsh On 28-Jul-2011 For All19072011
        '''MaintDetail = "Reg No. : " + mTmpComplyCompMonitorServiceStatusList(index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorServiceStatusList(index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorServiceStatusList(index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorServiceStatusList(index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorServiceStatusList(index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorServiceStatusList(index).DoneOnValueFormatted
        MarkLog(Util.Action.View, "ComponentServiceMonitor", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompServiceHistoryWindow", "OpenCompServiceHistoryWindow();", True)
        'End If
    End Sub

    Private Sub CompInspHistoryRecord(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal CompStatusID As Guid, ByVal DoneOn As String, ByVal MonitorDetail As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(MaintID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatusFromEntry(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
        Session("EnFrom") = 1 'EditRecord
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorInspStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        RemoveSession()

        mUpdateComplyHistoryCompMonitorInspStatusList = UpdateComplyHistoryCompMonitorInspStatusList.GetComplyHistoryCompMonitorInspStatusList(mCompStatus.CompID, mCompMonitorInspStatus.PartMonitorInspID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorInspStatusList") = mUpdateComplyHistoryCompMonitorInspStatusList

        'Added By Utkarsh On 28-Jul-2011 For All19072011
        '''MaintDetail = "Reg No. : " + mTmpComplyCompMonitorInspStatusList(index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorInspStatusList(index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorInspStatusList(index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorInspStatusList(index).TypeDet & " Done On Date : " & mTmpComplyCompMonitorInspStatusList(index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorInspStatusList(index).DoneOnValueFormatted
        MarkLog(Util.Action.View, "ComponentInspections", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
        'End

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenHistoryWindow", "OpenHistoryWindow();", True)
        'End If
    End Sub

    Private Sub CompModHistoryRecord(ByVal MachineID As Guid, ByVal AssemblyStatusID As Guid, ByVal MaintID As Guid, ByVal CompStatusID As Guid, ByVal DoneOn As String, ByVal MonitorDetail As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorModStatus As CompMonitorModStatus
        Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(MaintID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
        Session("EnFrom") = 1 'EditRecord
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyCompMonitorModStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        RemoveSession()
        mUpdateComplyHistoryCompMonitorModStatusList = UpdateComplyHistoryCompMonitorModStatusList.GetComplyHistoryCompMonitorModStatusList(mCompStatus.CompID, mCompMonitorModStatus.PartMonitorModID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorModStatusList") = mUpdateComplyHistoryCompMonitorModStatusList

        'Added By Utkarsh On 28-Jul-2011 For All19072011
        '''MaintDetail = "Reg No. : " + mTmpComplyCompMonitorModStatusList(index).MachineInfo & " Assembly Info : " & mTmpComplyCompMonitorModStatusList(index).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mTmpComplyCompMonitorModStatusList(index).CompInfo.Replace(Environment.NewLine, " ") & " Monitor Info : " & mTmpComplyCompMonitorModStatusList(index).MonitorInfo.Replace(Environment.NewLine, " ") & " Done On Date : " & mTmpComplyCompMonitorModStatusList(index).DoneOnFormatted & " Done On Value : " & mTmpComplyCompMonitorModStatusList(index).DoneOnValueFormatted
        MarkLog(Util.Action.View, "ComponentModifications", MonitorDetail, Util.ErrorType.NoError, MaintID, EventLogID)
        ''''End

        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfUpdateComplyHistoryCompMonitorModStatusList.aspx?GChildPage2=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompDirectiveHistoryWindow", "OpenCompDirectiveHistoryWindow();", True)
        'End If
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SignatureRequired" Then
                        PrintWO(SignatureRequired:=True, ByMail:=IIf(Session("btnSendMail") = "btnSendMail", True, False))

                        Dim Text As String = ""
                        If AppSettings("ClientCode") = "APFT" Or
                           AppSettings("ClientCode") = "AAP" Then
                            Text = " CALL-OUT/Work Order - " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
                        Else
                            Text = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
                        End If

                        If Session("btnSendMail") = "btnSendMail" Then
                            Session.Remove("btnSendMail")
                            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Work Order Details", Text.ToString, "",
                         "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                          SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
                        End If
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    If MSGBoxCtrl.Sender = "SignatureRequired" Then
                        PrintWO(SignatureRequired:=False, ByMail:=IIf(Session("btnSendMail") = "btnSendMail", True, False))

                        Dim Text As String = ""
                        If AppSettings("ClientCode") = "APFT" Or
                           AppSettings("ClientCode") = "AAP" Then
                            Text = " CALL-OUT/Work Order - " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
                        Else
                            Text = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
                        End If
                        If Session("btnSendMail") = "btnSendMail" Then
                            Session.Remove("btnSendMail")
                            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Work Order Details", Text.ToString, "",
                             "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                              SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
                        End If
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""
                    GridBind()

                    'Added By Vikrant on 14-Jun-2018 For ALL14062018
                    If MSGBoxCtrl.Sender = "WODateAlert" Then
                        txtFromDate.Text = AsOnDateForWOCreation
                        txtFromDate.DataBind()
                        upnlCreateWO.Update()
                    End If
                    'End
                    upnlGrid.Update()
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"

                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub

    Private Sub SetGrid()
        Dim B As Boolean

        For j As Integer = 0 To dgDueJob.Rows.Count - 1
            B = CType(Me.dgDueJob.Rows(j).Cells(18).Text, Boolean)
            If B = True Then
                dgDueJob.Rows(j).Cells(16).Enabled = False
            End If
        Next
    End Sub
    'End
    'Added By Vikrant on 14-Jun-2018 For ALL14062018

    Private Sub AddJobs(Optional ByVal ServiceCount As Integer = 0,
                        Optional ByVal InspectionCount As Integer = 0,
                        Optional ByVal ModificationCount As Integer = 0)
        Dim mReportMaintenanceDetail As ReportMaintenanceDetail
        Dim WOJobDescription As String
        Dim mATAList As ATAList
        Dim tmpAssemblyStatusList As AssemblyStatusList
        Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
        Dim DataType As String

        ' get the selected checkboxes from the form data
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            If AppSettings("IsEngineeringWORequired").ToLower = "true" Then
                If ModificationCount > 0 Then
                    mnWO = nWO.NewWO(TransTypeID:=Trans.EngineeringWO)
                ElseIf (ServiceCount > 0 Or InspectionCount > 0) Then
                    mnWO = nWO.NewWO(TransTypeID:=Trans.WOCAMO)
                End If
            Else
                mnWO = nWO.NewWO(TransTypeID:=Trans.WOCAMO)
            End If

            mATAList = ATAList.GetATAList()
            ' we'll need a split to get the individual ids
            Dim values = checkString.Split(","c)
            'Added By Vikrant On 27-Nov-2020
            'mnWO.WODate = txtFromDate.Text
            If AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR" Then
                mnWO.WODate = CType(txtFromDate.Text.ToString.Trim + " " + txtWOTime.Text.ToString.Trim, DateTime)
            Else
                mnWO.WODate = CDate(txtFromDate.Text)
            End If
            'End

            For Each value As String In values
                mReportMaintenanceDetail = ReportMaintenanceDetails(New Guid(value))

                DataType = IIf(mReportMaintenanceDetail.MaintenanceTypeID = 5 Or mReportMaintenanceDetail.MaintenanceTypeID = 8, "Servicing", IIf(mReportMaintenanceDetail.MaintenanceTypeID = 6 Or mReportMaintenanceDetail.MaintenanceTypeID = 9, "Inspection", "Modification"))
                mnWO.MachineID = New Guid(cmbAircraft.SelectedValue.ToString)
                If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan") Then
                    Dim TempRegNo As String = ""
                    TempRegNo = cmbAircraft.SelectedItem.Text
                    mnWO.WOText = Replace(TempRegNo, "VT-", "")
                    If AppSettings("ClientCode") = "ADeccan" Then 'ADeccan Code Added by Saylee on 11-May-2018 for ADeccan11052018
                        mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yy")
                    End If
                ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                    mnWO.WOText = "MJO# " & CStr(CDate(txtFromDate.Text).Date.Year) & " - " & mnWO.ModelName
                ElseIf AppSettings("ClientCode") = "TP" Then
                    mnWO.WOText = Replace(cmbAircraft.SelectedItem.Text, "VT-", "") & "/" & CStr(CDate(txtFromDate.Text).Date.Year)
                End If

                tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtFromDate.Text.ToString, cmbAircraft.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
                AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

                If mnWO.WOPeriods.Count <> 0 Then
                    For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
                        mnWO.WOPeriods.RemoveAt(i)
                    Next
                End If

                mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

                If mReportMaintenanceDetail.PartNo = "" Then
                    With mReportMaintenanceDetail
                        'Description = .DataType & " on Assembly-" & .MaintenanceEvent & "<BR>" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & "<BR>" & "Directive No.:" & .Number.ToString & " Ref.:" & .Reference.ToString
                        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
                            WOJobDescription = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & " Position:" & .Position & vbCrLf & DataType & " on Assembly-" & .MaintenanceEvent & CStr(IIf(.ModificationNumber.ToString <> "", "Directive No.:" & .ModificationNumber.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                            'Added By VIkrant On 05-June-2013 For FGA05062013
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
                            WOJobDescription = DataType & " on Assembly-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.ModificationNumber.ToString <> "", "Directive No.:" & .ModificationNumber.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                            'End
                        Else
                            'Description = .DataType & " on Assembly-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.ModificationNumber.ToString <> "", "Directive No.:" & .ModificationNumber.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, "")) & CStr(IIf(.SinceNew.ToString <> "", " Current Values:" & .SinceNew.ToString, ""))        '' & CStr(IIf(.DueAsof2.ToString <> "",  & " Due As Of:" & .DueAsof2.ToString, ""))
                            WOJobDescription = DataType & " on Assembly - " & .Code & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.ModificationNumber.ToString <> "", vbCrLf & "Directive No.: " & .ModificationNumber.ToString, ""))

                        End If

                    End With
                ElseIf mReportMaintenanceDetail.PartNo <> "" Then
                    With mReportMaintenanceDetail
                        'Description = .DataType & " on Component-" & .MaintenanceEvent & "<BR>" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & "<BR>" & "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString & "<BR>" & "Directive No.:" & .ModificationNumber.ToString & " Ref.:" & .Reference.ToString
                        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
                            WOJobDescription = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & " Position:" & .Position & vbCrLf & DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.PartNo <> "", "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.ModificationNumber.ToString <> "", "Directive No.:" & .ModificationNumber.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))     '' & CStr(IIf(.DueAsof2.ToString <> "", "<BR>" & " Due As Of:" & .DueAsof2.ToString, ""))
                            'Added By VIkrant On 05-June-2013 For FGA05062013
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
                            WOJobDescription = DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.PartNo <> "", "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.ModificationNumber.ToString <> "", "Directive No.:" & .ModificationNumber.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                            'End
                        Else
                            'Description = .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.PartNo <> "", "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.ModificationNumber.ToString <> "", "Directive No.:" & .ModificationNumber.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, "")) & CStr(IIf(.SinceNew.ToString <> "", " Current Values:" & .SinceNew.ToString, ""))          '' & CStr(IIf(.DueAsof2.ToString <> "",  & " Due As Of:" & .DueAsof2.ToString, ""))
                            WOJobDescription = DataType & " on Component - " & .MaintenanceOn & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartNo <> "", vbCrLf & "Part: " & .PartNo & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.ModificationNumber.ToString <> "", vbCrLf & "Directive No.: " & .ModificationNumber.ToString, ""))
                        End If



                    End With
                End If
                'Commented and Added By Saylee On 05-June-2013 For BA07082013
                '''''''''Description = Description & CStr(IIf(mReportMaintenanceDetail.JobDescription <> "", mReportMaintenanceDetail.JobDescription, "")) & CStr(IIf(mReportMaintenanceDetail.Note <> "", mReportMaintenanceDetail.Note, ""))
                'Here BA needs only description fro Master so directly assigned JobDescription
                Dim TempDescription As String = ""
                If mReportMaintenanceDetail.Description <> "" Then
                    TempDescription = mReportMaintenanceDetail.Description.Replace("<br>", " ")
                    TempDescription = TempDescription.Replace("<b>", "")
                    TempDescription = TempDescription.Replace("</b>", "")
                Else
                    TempDescription = ""
                End If
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then ' Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "YA") Then
                    WOJobDescription = TempDescription.ToString  'CStr(IIf(mReportMaintenanceDetail.Description <> "", mReportMaintenanceDetail.Description.Replace("<br>", vbCrLf), ""))
                Else
                    WOJobDescription = WOJobDescription & CStr(IIf(mReportMaintenanceDetail.Description.ToString <> "", vbCrLf & "Description: " & TempDescription.ToString, "")) & CStr(IIf(mReportMaintenanceDetail.Note <> "", vbCrLf & "Note: " & mReportMaintenanceDetail.Note, ""))
                End If
                '------------------------------

                mnWO.WOJobs.Add(mnWO.ID, 2)
                mnWO.WOJobs.CurrentItem.PreviousTransID = mReportMaintenanceDetail.StatusID
                mnWO.WOJobs.CurrentItem.WOJobDescription = WOJobDescription
                'mnWO.WOJobs.CurrentItem.DueAsOf = mReportMaintenanceDetail.DueAsofAll
                mnWO.WOJobs.CurrentItem.DueAsOf = mReportMaintenanceDetail.AssDueAsofAllExcel

                ''If Not mReportMaintenanceDetail.StartDate Is DBNull.Value Then mnWO.WOJobs.CurrentItem.WOJobStartDate = mReportMaintenanceDetail.StartDate
                mnWO.WOJobs.CurrentItem.TSNCSN = mReportMaintenanceDetail.TSN 'CHK
                mnWO.WOJobs.CurrentItem.SBADNO = mReportMaintenanceDetail.ModificationNumber
                If mATAList.Contains(mReportMaintenanceDetail.ATACode) Then
                    mnWO.WOJobs.CurrentItem.ATAChapterID = mATAList(mReportMaintenanceDetail.ATACode).ID
                End If


                'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
                If mReportMaintenanceDetail.PartNo = "" Then
                    mnWO.WOJobs.CurrentItem.OnTypeID = 1
                ElseIf mReportMaintenanceDetail.PartNo <> "" Then
                    mnWO.WOJobs.CurrentItem.OnTypeID = 2
                End If
                If mReportMaintenanceDetail.MaintenanceTypeID = 5 Or mReportMaintenanceDetail.MaintenanceTypeID = 8 Then 'CHK
                    mnWO.WOJobs.CurrentItem.MonitorTypeID = 1
                ElseIf mReportMaintenanceDetail.MaintenanceTypeID = 6 Or mReportMaintenanceDetail.MaintenanceTypeID = 9 Then
                    mnWO.WOJobs.CurrentItem.MonitorTypeID = 2
                ElseIf mReportMaintenanceDetail.MaintenanceTypeID = 7 Or mReportMaintenanceDetail.MaintenanceTypeID = 10 Then
                    mnWO.WOJobs.CurrentItem.MonitorTypeID = 3
                End If
                '-----------------------------------------------------------------------
                mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = mReportMaintenanceDetail.RequiredManHours

                mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = mReportMaintenanceDetail.Description  'Added By Vikrant On 19-Dec-2012 For ALL19122012

                'Added by Saylee on 23-July-2013 for BA22072013 	
                mnWO.WOJobs.CurrentItem.Zone = mReportMaintenanceDetail.Zone
                mnWO.WOJobs.CurrentItem.AREA = mReportMaintenanceDetail.Area
                mnWO.WOJobs.CurrentItem.IsRII = mReportMaintenanceDetail.IsRII
                'End
                If mReportMaintenanceDetail.AssemblyTypeID = 1 Then
                    mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mReportMaintenanceDetail.AssemblyType
                Else
                    mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mReportMaintenanceDetail.AssemblyType + IIf(mReportMaintenanceDetail.Position = "", "", "(" + mReportMaintenanceDetail.Position + ")")
                End If

                If AppSettings("ShowCAMOOnlyForNewClients") = "True" And mnWO.WOJobs.CurrentItem.MonitorTypeID = 1 Then
                    mnWO.WOJobs.CurrentItem.TaskCardNo = mReportMaintenanceDetail.TaskNo
                    mnWO.WOJobs.CurrentItem.TaskSourceRef = mReportMaintenanceDetail.SourceDoc
                    mnWO.WOJobs.CurrentItem.Publication = mReportMaintenanceDetail.Reference
                    mnWO.WOJobs.CurrentItem.Skill = mReportMaintenanceDetail.Skill
                    mnWO.WOJobs.CurrentItem.SkillID = mReportMaintenanceDetail.SkillID
                ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And mnWO.WOJobs.CurrentItem.MonitorTypeID = 3 Then
                    mnWO.WOJobs.CurrentItem.TaskCardNo = mReportMaintenanceDetail.ModificationNumber
                    mnWO.WOJobs.CurrentItem.InspCode = mReportMaintenanceDetail.ModelMonitorModCode
                    mnWO.WOJobs.CurrentItem.TaskSourceRef = mReportMaintenanceDetail.Reference
                Else
                    If AppSettings("ShowNewDiscrepancyFlow") = "True" Then
                        mnWO.WOJobs.CurrentItem.TaskCardNo = mReportMaintenanceDetail.Code
                    End If
                    mnWO.WOJobs.CurrentItem.InspCode = mReportMaintenanceDetail.ModificationNumber 'Added by Saylee on 18-Feb-2018 for ASH18022019 
                    mnWO.WOJobs.CurrentItem.TaskSourceRef = mReportMaintenanceDetail.Reference
                End If

                With mnWO.WOJobs.CurrentItem
                    'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
                    'TASK(s):
                    Dim mMaintenanceTask As MaintenanceTask
                    Dim mMaintenanceTaskDetail As MaintenanceTaskDetail

                    If .OnTypeID = 1 Then        'Assembly
                        mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, True)
                    ElseIf .OnTypeID = 2 Then    'Componant
                        mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, False)
                    End If

                    For Each mMaintenanceTaskDetail In mMaintenanceTask.MaintenanceTaskDetails
                        mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

                        With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem
                            '.TaskAction = "No action taken." 'mMaintenanceTaskDetail.Task 'Commented By Prashant 12-Mar-2010
                            .TaskAction = ""  'Added By Prashant 12-Mar-2010
                            .ActualStartDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
                            .ActualEndDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
                            .IsDone = False
                            .TaskCardID = mMaintenanceTaskDetail.TaskCardID  'Added By Prashant 29-Dec-2008

                            'Added By Utkarsh On 27-Apr-2011

                            Dim mTaskCard As TaskCard
                            mTaskCard = TaskCard.GetTaskCard(mMaintenanceTaskDetail.TaskCardID)
                            .TaskCardNo = mTaskCard.TaskCardNo
                            .TaskDescription = mTaskCard.TaskDesc
                            .RevNo = mTaskCard.RevNo
                            .RevDate = mTaskCard.RevDate
                            .IssueDate = mTaskCard.IssueDate

                            'Commentedby Saylee on 15-Feb-2013
                            .Reference = mTaskCard.Reference

                            .Equipment = mTaskCard.Equipment
                            .Material = mTaskCard.Material
                            .EstimatedHours = mTaskCard.EstimatedHours
                            .checks = mTaskCard.Check
                            .RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo
                            .ImageSize = mTaskCard.ImageSize
                            .ImageFile = mTaskCard.ImageFile
                            .FileExtension = mTaskCard.FileExtension

                            'Added by Vikrant on 06-Sept-2013 For BA04092013
                            Dim mTaskCardSpare As TaskCardSpare
                            Dim mTaskCardStepsSpare As TaskCardSpare

                            For Each mTaskCardSpare In mTaskCard.TaskCardSpares
                                mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem
                                    .ItemID = mTaskCardSpare.ItemID
                                    .RequiredQty = mTaskCardSpare.RequiredQty
                                    .PartNo = mTaskCardSpare.PartNo
                                    .Description = mTaskCardSpare.Description
                                    .Remark = mTaskCardSpare.Remark
                                    .OnSerialNo = mTaskCardSpare.OnSerialNo
                                    .OffSerialNo = mTaskCardSpare.OffSerialNo
                                    .IsForSteps = False
                                End With

                            Next

                            For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
                                mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem
                                    .ItemID = mTaskCardStepsSpare.ItemID
                                    .RequiredQty = mTaskCardStepsSpare.RequiredQty
                                    .PartNo = mTaskCardStepsSpare.PartNo
                                    .Description = mTaskCardStepsSpare.Description
                                    .Remark = mTaskCardStepsSpare.Remark
                                    .OnSerialNo = mTaskCardStepsSpare.OnSerialNo
                                    .OffSerialNo = mTaskCardStepsSpare.OffSerialNo
                                    .IsForSteps = True
                                End With
                            Next
                            'End
                            'Added By Vikrant on 03-Mar-2020 For ALL03032020
                            For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals
                                mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem
                                    .ItemID = mTaskCardSpare.ItemID
                                    .RequiredQty = mTaskCardSpare.RequiredQty
                                    .PartNo = mTaskCardSpare.PartNo
                                    .Description = mTaskCardSpare.Description
                                    .Remark = mTaskCardSpare.Remark
                                    .OnSerialNo = mTaskCardSpare.OnSerialNo
                                    .OffSerialNo = mTaskCardSpare.OffSerialNo
                                    .IsForSteps = False
                                    .IsPartRemoval = True
                                    .Position = mTaskCardSpare.Position
                                End With

                            Next
                            'End
                        End With
                    Next

                    'KIT(s):
                    Dim mMaintenanceKit As MaintenanceKit

                    If .OnTypeID = 1 Then        'Assembly
                        mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True)
                    ElseIf .OnTypeID = 2 Then    'Componant
                        mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False)
                    End If

                    'Added by Saylee on 23-July-2013 for BA22072013 	
                    Dim mMaintenanceSpares As MaintenanceKit
                    Dim mMaintenanceSparesDetail As MaintenanceKitDetail

                    Dim mMaintenanceTools As MaintenanceKit
                    Dim mMaintenanceToolsDetail As MaintenanceKitDetail

                    If .OnTypeID = 1 Then        'Assembly
                        mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, False)
                        mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, True)
                    ElseIf .OnTypeID = 2 Then    'Componant
                        mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, False)
                        mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, True)
                    End If

                    For Each mMaintenanceSparesDetail In mMaintenanceSpares.MaintenanceKitDetails
                        mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

                        With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
                            .ItemID = mMaintenanceSparesDetail.ItemID
                            .RequiredQty = mMaintenanceSparesDetail.Qty
                            Dim mItem As Item = Item.GetItem(mMaintenanceSparesDetail.ItemID)
                            .PartNo = mItem.Name
                            .Description = mItem.Description
                            mItem = Nothing
                            .Remark = mMaintenanceSparesDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                        End With
                    Next

                    For Each mMaintenanceToolsDetail In mMaintenanceTools.MaintenanceKitDetails
                        If Not mnWO.WOTools.Contains(mMaintenanceToolsDetail.ItemID) Then
                            mnWO.WOTools.Add(mnWO.ID)

                            With mnWO.WOTools.CurrentItem
                                .ItemID = mMaintenanceToolsDetail.ItemID
                                .RequiredQty = mMaintenanceToolsDetail.Qty
                                Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
                                .PartNo = mItem.Name
                                .Description = mItem.Description
                                mItem = Nothing
                                .WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                            End With
                        Else
                            mnWO.WOTools.CurrentIndex = mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").SrNo - 1
                            If mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty = 0 Then

                            Else
                                If (mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty <= mMaintenanceToolsDetail.Qty) Or (mMaintenanceToolsDetail.Qty = 0) Then
                                    With mnWO.WOTools.CurrentItem
                                        .ItemID = mMaintenanceToolsDetail.ItemID
                                        .RequiredQty = mMaintenanceToolsDetail.Qty
                                        Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
                                        .PartNo = mItem.Name
                                        .Description = mItem.Description
                                        mItem = Nothing
                                        .WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                                    End With
                                End If
                            End If
                        End If
                    Next
                    '-----------------------------------------------------------------------
                End With
            Next


            values = ""
            checkString = Nothing
        End If
        Session("mnWO") = mnWO
    End Sub

    Public Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()

        txtFromDate.Text = AsOnDateForWOCreation
        'Added By Vikrant On 27-Nov-2020
        If AsOnDateForWOCreation <> "" OrElse Not AsOnDateForWOCreation Is Nothing Then
            If IsDate(AsOnDateForWOCreation) Then
                txtWOTime.Text = Format(CDate(DateTime.UtcNow.ToString), AppSettings("TimeFormat"))
            Else
                txtWOTime.Text = ""
            End If
        End If
        'End
        cmbAircraft.SelectedValue = MachineIDForWOCreation
        txtFromDate.DataBind()
        cmbAircraft.Enabled = IIf(MachineIDForWOCreation.Equals(Guid.Empty.ToString), True, False)
    End Sub
    'End

    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'If AppSettings("ShowNewWOFlow") = "True" Then

        '    ' If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then
        '    If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mtmpTransTypeID Then
        '        If mnWO.TransTypeID = Trans.WO145 Then
        '            IsInRoleString = "WOCreate"
        '        ElseIf mnWO.TransTypeID = Trans.OJS145 Then
        '            IsInRoleString = "OJSWorkOrder"
        '        ElseIf mnWO.TransTypeID = Trans.OJSCAMO Then
        '            IsInRoleString = "OJSCAMOWorkOrder"
        '        Else
        '            IsInRoleString = "CAMOWOCreate"
        '        End If
        '    ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then
        '        IsInRoleString = "WOPlanning"
        '    ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
        '        IsInRoleString = "WOExecution"
        '    ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then
        '        IsInRoleString = "WOCompletion"
        '    ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then
        '        IsInRoleString = "WOQCApproval"
        '    ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then
        '        IsInRoleString = "WOCAMOUpdate"
        '    ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then
        '        IsInRoleString = "WOBilling"
        '    End If

        'Else
        If mnWO.TransTypeID = Trans.WO145 Then
            IsInRoleString = "WorkOrder"
        ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then
            IsInRoleString = "SpareAssemblyWO"
        ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
            IsInRoleString = "SpareComponentWO"
        ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then
            IsInRoleString = "EngineeringOrder"
        Else
            IsInRoleString = "CAMOWO"
        End If


        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
                'Added By Vikrant on 30-Jun-2021 For ALL30062021 
            Case Rights.Completed
                Return User.IsInRole(IsInRoleString + "Completed")
                'End
        End Select
    End Function

    Public Sub PrintWO(Optional ByVal ByMail As Boolean = False, Optional ByVal SignatureRequired As Boolean = False, Optional ByVal HeligoCallOutPrint As Boolean = False)
        'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'Added by Saylee on 7-Mar-2014 for ALL07032014
        mnWO = Session("mnWO")

        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Dim FormRevisionNo As String = ""
        Dim FormRevisionDate As String = ""
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportDocument 'CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsnWODetail
        Dim mnWOJobs As nWOJobs
        Dim mnWOJobComps As nWOJobComps
        Dim mnWOJobSpares As nWOJobSpares 'Added By Saylee on 20-Sep-2019 HSC20092019
        Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations 'Added By Vikrant On 24-June-2013 For Indamer21062013
        Dim mnWONRCJobs As nWOJobs

        Dim WODocumentNo As String = ""
        Dim WORevisionNo As String = ""
        Dim FormNo As String = ""
        Dim IssueNo As String = ""
        Dim IssueDate As String = ""

        Dim Searchstr7 As String = ""
        Dim LastLogDate As String = ""
        Dim LastLogDateHavingAPUValues As String = ""

        Dim ReportTitle As String = "AIRCRAFT WORK ORDER"
        If AppSettings("ShowCAMOOnlyForNewClients") = "False" And AppSettings("ShowAMOOnlyForNewClients") = "False" Then
            ReportTitle = "AIRCRAFT WORK ORDER"
        Else
            ReportTitle = "WORK ORDER"
        End If

        Dim EOFooterLine As String = ""

        Dim mnWORegisterList As nWORegisterList

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then EOFooterLine = CType(AppSettings("EOFooterLine"), String)


		If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
			''myReport = New crnWorkOrder
			myReport = New crnWODetailForIndamar 'added By Saylee ON 05-April-2013 FOR Indamar04104013
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

			If mnWO.WOJobs.IsScheduledJobExists Then
				myReport = New crnWODetailForTAALLandscapeSch
			ElseIf mnWO.WOJobs.IsUnScheduledJobExists Then

				myReport = New crnWODetailForTAALLandscapeUnSch
			Else
				myReport = New crnWODetailForTAALLandscape
			End If

		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "DOL" Then
			myReport = New crnWODetailForDolphin
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "HL" Then
			If mnWO.EngCount > 2 Then
				myReport = New crnWODetailForHeavyLiftFormat2
			Else
				myReport = New crnWODetailForHeavyLiftFormat1
			End If
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
			LastLogDate = MaxLogOfAircraft.GetMaxLogOfAircraft(mnWO.MachineID, IsLastLogWithValuesRequired:=True, AssemblyTypeID:=1, WODate:=mnWO.WODateFormatted.ToString).LogDateFormatted.ToString
			LastLogDateHavingAPUValues = MaxLogOfAircraft.GetMaxLogOfAircraft(mnWO.MachineID, IsLastLogWithValuesRequired:=True, AssemblyTypeID:=4, WODate:=mnWO.WODateFormatted.ToString).LogDateFormatted.ToString

			myReport = New crnWOIssueDetail 'New addition by Saylee on 25-July-2011
			WODocumentNo = AppSettings("WODocumentNo")
			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
			' WORevisionNo = AppSettings("WORevisionNo")
			WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
			'-----
		ElseIf (AppSettings("ClientCode") = "BA") Then 'Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
			'Commented and Added By Vikrant On 13-May-2013 For BA13052013
			'If Session("IsAdditionalWO") = "True" Then 'added By Prashant ON 08-Nov-2012 FOR ALL06112012-2
			''myReport = New crAdditionalWorkCard
			''ReportTitle = "ADDITIONAL WORK CARD"
			'Else
			'myReport = New crAdditionalWorkCard
			'ReportTitle = "ADDITIONAL WORK CARD"
			''myReport = New crRoutineWorkOrder
			''ReportTitle = "Routine Work Order"
			'End If
			myReport = New crnWODetailForBA
		ElseIf (AppSettings("ClientCode") = "Novo") Then 'Added by Saylee on 23-Jan-2018 for NOVO23012018
			myReport = New crnWODetailForNOVO 'Added by Saylee on 23-Jan-2018 for NOVO23012018
		ElseIf AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
			myReport = New crnWODetailReportYATA
			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
			' WORevisionNo = AppSettings("WORevisionNo")
			'WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo

			FormRevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo '21
			FormRevisionDate = mTransactionList.Item(mnWO.TransTypeID).FormRevisionDate ''22
			IssueNo = AppSettings("WOIssueNo")
			WORevisionNo = AppSettings("RevisionNumber")
			IssueDate = AppSettings("IssueDate")
			'-----
			FormNo = AppSettings("WoNo")
			'End
			'Added By Utkarsh ON 30-Nov-2012 FOR ALL30112012-1
			'ElseIf AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Then
		ElseIf AppSettings("ClientCode") = "IIC" Then
			myReport = New crnWODetailForDeccan
		ElseIf AppSettings("ClientCode") = "Deccan" Then ' SPZ Code added by Saylee on 13-Jun-2022 
			myReport = New crnWOIssueDetailForDeccan 'Added by Vikrant For Deccan03022021
			'End
			'Added by ajay 13-09-2023
			Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(mnWO.MachineID)
			If mMachineOperatorName.OperatorName <> "" Then Searchstr7 = mMachineOperatorName.OperatorName
			'----
		ElseIf AppSettings("ClientCode") = "ADeccan" Then
			myReport = New crnWODetailForAirDeccan
		ElseIf AppSettings("ClientCode") = "FG" Or AppSettings("ClientCode") = "JA" Then
			myReport = New crnWODetailForFG  'Added By Vikrant On 15-May-2013 FOR FGA15052013
			'End
			'Added By Shweta on 11-Sep-2013 For UHPL11092013-1
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "UHPL" Then
			myReport = New crnWODetailForUHPL
			'End
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then  'Added By Prashant 9-Jun-2014 HELIGO09062014
			'myReport = New crnWODetailForHeligo

			'Added by Saylee on 20-Nov-2020 for Heligo20112020
			'If HeligoCallOutPrint = True Then
			If HeligoCallOutPrint = True And mnWO.TransTypeID = 89 Then ' CAMO-Work Order
				myReport = New crnWODetailForHeligoCallOut
				FormNo = AppSettings("WoNo")
			Else
				myReport = New crnWODetailForHeligo
			End If
			WODocumentNo = AppSettings("WODocumentNo")
			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
			' WORevisionNo = AppSettings("WORevisionNo")
			WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
			'-----

		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "TP" Then 'Added By Vikrant On 06-Jun-2016 For TP06062016
			myReport = New crnWODetailForTP
			'End
		ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then 'Added by saylee on 13-Jun-2016
			myReport = New crnWODetailForBIRD
			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
			' WORevisionNo = AppSettings("WORevisionNo")
			WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
			'-----
			FormNo = AppSettings("WoNo")
			IssueNo = AppSettings("WOIssueNo")
			da.Fill(ds, mnWO.WOTools)
			'End
		ElseIf AppSettings("ClientCode") = "GEP" Then
			myReport = New crnWODetailGEP
		ElseIf AppSettings("ClientCode") = "RBH" Then 'Added by Saylee on 17-Nov-2017 for RBH
			mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)

			da.Fill(ds, mnWORegisterList)
			da.Fill(ds, mnWO.WOTools)
			da.Fill(ds, nWOJobSpares.GetWOSpares(mnWO.ID, ""))
			myReport = New crnWODetailForRBH
		ElseIf AppSettings("ClientCode") = "STR" Then 'Added by Vikrant On 09-May-2018 For STR09052018
			'FormNo = AppSettings("WoNo")
			'WORevisionNo = AppSettings("WORevisionNo")
			'IssueNo = AppSettings("WOIssueNo")
			myReport = New crnWOIssueDetailForStarAir
			da.Fill(ds, "nIssuedWOSpares", Session("mnIssuedWOSpareswfnWODetail")) 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020
			da.Fill(ds, "nIssuedWOTools", Session("mIssuedWOToolswfnWODetail")) 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020
		ElseIf AppSettings("ClientCode") = "DHL" Then 'Added By Prashant 27-Sep-2018 DHILLON27092018
			myReport = New crnWODetailForDhillon
		ElseIf AppSettings("ClientCode") = "APFT" Or AppSettings("ClientCode") = "AAP" Then 'Added by Saylee on 29-Nov-2018 for APFT
			myReport = New crnWODetailForAPFT
		ElseIf AppSettings("ClientCode") = "ASH" Then 'Added by Saylee on 18-Feb-2019 for ASHLEY for ASH18022019
			myReport = New crnWODetailForASHLEY
			WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
			mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)
			da.Fill(ds, mnWORegisterList)
		ElseIf AppSettings("ClientCode") = "KLP" Then 'Added by Saylee on 4-APR-2019 for Kelachandra Logistics Private Limited for KLP04042019
			myReport = New crnWODetailForKLP
		ElseIf AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "UYA" Then 'Added by Saylee on 17-JUN-2019 for Indamar for IND14062019 'UYA Added By Vikrant On 14-Jul-2020 For ALL14072020 UYA needs same values like IND so used sama patch
			If AppSettings("ClientCode") = "IND" Then
				myReport = New crnWODetailForIND
			ElseIf AppSettings("ClientCode") = "UYA" Then 'Added By Vikrant On 14-Jul-2020 For ALL14072020
				myReport = New crnWODetailForUYA
			End If
			'IND
			If mnWO.StatusID = 2 And mnWO.WOStatusID = 3 Then 'Only Completed WO
				Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
				CompletedByUserLicenceNos = mUser.LicenseNo

				Dim mAssemblyStatusList As AssemblyStatusList
				mnWO = nWO.GetWO(mnWO.ID, AllWOJobType:=False, getAircraftValuesAsOnCompletionDate:=True)
				mAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mnWO.MachineID, AssemblyType:="Airframe", CurrentDate:=mnWO.WOCloseDateFormatted.ToString, IsAssemblyInstalled:=True)
				If mAssemblyStatusList.Count > 0 Then
					If Not mAssemblyStatusList(0).AssemblyStatusPeriodList(1, "") Is Nothing Then
						AirframeHrsAsOnCompletionDate = mAssemblyStatusList(0).AssemblyStatusPeriodList(1, "").AssemblyCurrentValue
					End If
					If mnWO.Cycles <> "" Then 'Same Formula used here as that of IND crystal rpeort
						If Not mAssemblyStatusList(0).AssemblyStatusPeriodList(3, "") Is Nothing Then
							AFAllPeriodsAsOnCompletionDate = mAssemblyStatusList(0).AssemblyStatusPeriodList(3, "").AssemblyCurrentValue + " C" + IIf(mnWO.AFAllPeriodsAsOnWOCompletionDate = "", "", Chr(13) + mnWO.AFAllPeriodsAsOnWOCompletionDate)
						End If
					Else
						If Not mAssemblyStatusList(0).AssemblyStatusPeriodList(7, "") Is Nothing Then
							AFAllPeriodsAsOnCompletionDate = mAssemblyStatusList(0).AssemblyStatusPeriodList(7, "").AssemblyCurrentValue + " L" + IIf(mnWO.AFAllPeriodsAsOnWOCompletionDate = "", "", Chr(13) + mnWO.AFAllPeriodsAsOnWOCompletionDate)
						End If
					End If
				End If
				Session("mnWO") = mnWO
			End If
			'End
		ElseIf AppSettings("ClientCode") = "LNT" Then 'Added by Saylee on 17-JUN-2019 for LNT for LNT17062019
			myReport = New crnWODetailForLNT
		ElseIf AppSettings("ClientCode") = "HSC" Then
			WODocumentNo = AppSettings("WODocumentNo")
			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
			' WORevisionNo = AppSettings("WORevisionNo")
			WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
			'-----
			FormNo = AppSettings("FormNo")
			IssueNo = AppSettings("WOIssueNo")
			myReport = New crnWOIssueDetailForHeliStar
		ElseIf AppSettings("ClientCode") = "IIC" Then 'Added by Saylee on 16-JUL-2019 for LNT for IIC16072019
			myReport = New crnWODetailForIIC
		ElseIf AppSettings("ClientCode") = "PAS" Then 'Added by Prashant on 22-Aug-2019 for Passion for PAS22082019
			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
			' WORevisionNo = AppSettings("WORevisionNo")
			WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
			'-----
			IssueNo = AppSettings("WOIssueNo")
			myReport = New crnWODetailPassion
		ElseIf AppSettings("ClientCode") = "SUH" Then
			myReport = New crnWODetailForSUH
		ElseIf AppSettings("ClientCode") = "FAP" Then 'Added by Prashant on 10-Sep-2019 for Fiducia Aviation Pvt. Ltd. for Fiducia10092019
			myReport = New crnWODetailFAP
		ElseIf AppSettings("ClientCode") = "PNW" Then 'Added by Prashant on 2-Dec-2019 for poonawalla aviation
			myReport = New crnWODetailForPNW
		ElseIf AppSettings("ClientCode") = "HNS" Then
			myReport = New crnWODetailForSAFAL
		ElseIf AppSettings("ClientCode") = "Dana" Then 'Added By Prashant on 20-May-2021 DANA20052021
			myReport = New crnWODetailForDana
		ElseIf AppSettings("ClientCode") = "GMP" Then 'Added By Saylee on 26-Aug-2021 GMP26082021
			myReport = New crnWODetailForGMP
		ElseIf AppSettings("ClientCode") = "BLUE" Then 'Added By Saylee on 24-Sep-2021 BLUE24092021
			myReport = New crnWODetailForBlueRay
		ElseIf AppSettings("ClientCode") = "IRM" Then 'Added By Saylee on 22-Oct-2021 IRM22102021
			IssueNo = AppSettings("WOIssueNo")
			myReport = New crnWODetailForIRM
		ElseIf AppSettings("ClientCode") = "FBW" Then
			myReport = New crnWODetailForFBW
		ElseIf AppSettings("ClientCode") = "IPA" Then '''Added By Saylee - Indo Pacific
			myReport = New crnWODetailForIPA
		ElseIf AppSettings("ClientCode") = "TSL" Then
			myReport = New crnWODetailForTSL
			IssueNo = AppSettings("WOIssueNo")
		ElseIf AppSettings("ClientCode") = "SAA" Then '''Added By Prashant 28-Mar-2022
			myReport = New crnWODetailForSaurya
		ElseIf AppSettings("ClientCode") = "SPZ" Then '''Added By Prashant 7-Jun-2022
			myReport = New crnWODetailForSparzana
		ElseIf AppSettings("ClientCode") = "SHN" Then '''Added By Ajay 6-Sep-2022
			'myReport = New crnWODetailForShivan
			myReport = New crnWODetailForSHN
		ElseIf AppSettings("ClientCode") = "RAJ" Then '''Added Sankalp
			myReport = New crnWODetailForRAJ 'Added by Sankalp
		ElseIf AppSettings("ClientCode") = "SIT" Then '''Added Sankalp
			myReport = New crnWODetailForSIT 'Added by Sankalp
		ElseIf AppSettings("ClientCode") = "GUN" Then '''Added By Prashant 19-Jan-2023
            myReport = New crnWODetailForGuna
        ElseIf AppSettings("ClientCode") = "MEL" Then '''Added By Ajay 3-May-2023
            myReport = New crnWODetailForMEL
        ElseIf AppSettings("ClientCode") = "ACI" Then '''Added By Prashant 25-Jan-2023
            myReport = New crnWOWorkPackForACI
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(mnWO.MachineID)
            If mMachineOperatorName.OperatorName <> "" Then Searchstr7 = mMachineOperatorName.OperatorName
            FormRevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
            FormRevisionDate = mTransactionList.Item(mnWO.TransTypeID).FormRevisionDate
            IssueNo = AppSettings("WOIssueNo")
        Else
            myReport = New crnWODetail
        End If

        mnWO = Session("mnWO")
        If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "SHN" Then
            mnWO = nWO.GetWO(mnWO.ID, getAircraftValuesAsOnCompletionDate:=True)
        End If

        mnWOJobs = mnWO.WOJobs
        mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
        mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "") 'Added By Saylee on 20-Sep-2019 HSC20092019
        mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")  'Added By Vikrant On 24-June-2013 For Indamer21062013
        FormNo = AppSettings("WoNo")

        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            ''Dim mnWOJobSpares As nWOJobSpares
            ''Dim mnWOTools As nWOTools
            ''Dim mnrptRoutineWorkOrderList As nrptRoutineWorkOrderList
            ''mnrptRoutineWorkOrderList = nrptRoutineWorkOrderList.GetRoutineWorkOrderList(mnWO.ID)

            ''mnWOTools = mnWO.WOTools
            ''mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "")
            ''da.Fill(ds, mnWOTools)
            ''da.Fill(ds, mnWOJobSpares)
            ''da.Fill(ds, mnrptRoutineWorkOrderList)

            ' Added By Vikrant On 13-May-2013 For BA13052013

            mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)
            da.Fill(ds, mnWORegisterList)
            'End
        ElseIf AppSettings("ClientCode") = "Indamer" Then  'Added By Vikrant On 14-May-2013 For IND14052013
            Dim mtmpMachineList As tmpMachineList
            Dim ReportStatusList As New rptStatusList
            mtmpMachineList = tmpMachineList.GetMachineList(, mnWO.RegNo, , , , , True, mnWO.WODate.ToString)
            For i As Integer = 0 To mtmpMachineList.Count - 1
                ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , Year(New SmartDate(mnWO.WODate.ToString).FormattedText).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
            Next
            da.Fill(ds, ReportStatusList)

            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(mnWO.MachineID)
            If mMachineOperatorName.OperatorName <> "" Then Searchstr7 = mMachineOperatorName.OperatorName
        End If

        'Added by Saylee on 11-Oct-2018 for ALL11102018
        If mnWO.IsDigitalSignatureAdded Then
            mFileAttachnWO = FileAttach.GetAttachment(mnWO.ID, , "DigitalSignatureWO", ds, AppSettings("DOCPath"))
            da.Fill(ds, "FileAttach", mFileAttachnWO)
        End If
        '***************************
        Dim EmpName As String = ""

        Dim mEmployee As Employee
        If Not mnWO.EmployeeID.Equals(Guid.Empty) Then
            mEmployee = Employee.GetEmployee(mnWO.EmployeeID)
            EmpName = mEmployee.Name
        End If

        Dim EmployeeName As String = ""

        If mnWO.AuthorizedBy = "" Then
            EmployeeName = ""
        Else
            EmployeeName = mnWO.AuthorizedBy 'SI.UTILITY.User.GetUser(mnWO.AuthorizedBy).EmpNoName
        End If

        If AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "SUH" Or AppSettings("ClientCode") = "LNT" _
            Or AppSettings("ClientCode") = "UYA" Or AppSettings("ClientCode") = "FBW" Or AppSettings("ClientCode") = "IPA" _
            Or AppSettings("ClientCode") = "IRM" Or AppSettings("ClientCode") = "SPZ" Or AppSettings("ClientCode") = "MEL" Then 'UYA Added By Vikrant On 14-Jul-2020 For ALL14072020 
            Dim mnWOTaskParameterList As nWOParameterList
            Dim mnWORequestsParameterList As nWOParameterList
            Dim mnWOStatisticsParameterList As nWOParameterList

            Dim tmpLog As Log 'Added by Saylee 16-Sep-2019

            If Not mnWO.LogID.Equals(Guid.Empty) Then
                tmpLog = Log.GetLog(mnWO.LogID)
                LastLogDate = tmpLog.DateFormatted
            Else
                LastLogDate = ""
            End If

            mnWOTaskParameterList = nWOParameterList.GetWOParameterList(mnWO.ID, "Tasks", IsForReport:=True)
            mnWORequestsParameterList = nWOParameterList.GetWOParameterList(mnWO.ID, "Requests", IsForReport:=True)
            mnWOStatisticsParameterList = nWOParameterList.GetWOParameterList(mnWO.ID, "Statistics", IsForReport:=True)

            da.Fill(ds, "mnWOTaskParameterList", mnWOTaskParameterList)
            da.Fill(ds, "mnWORequestsParameterList", mnWORequestsParameterList)
            da.Fill(ds, "mnWOStatisticsParameterList", mnWOStatisticsParameterList)
            tmpLog = Nothing
        End If

        'Added By Vikrant On 14-Jul-2020 For ALL14072020

        If mnWO.TransTypeID = 88 Then ' Third Party-Work Order
            FormRevisionNo = mTransactionList.Item(Trans.WO145).FormRevisionNo
            FormRevisionDate = mTransactionList.Item(Trans.WO145).FormRevisionDate
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" And HeligoCallOutPrint = False Then 'added by Prashant on 11-Jan-2023 as Per mail According to work order type
                FormNo = "HCPL/QC/21"
            End If
        ElseIf mnWO.TransTypeID = 89 Then ' CAMO-Work Order
            FormRevisionNo = mTransactionList.Item(Trans.WOCAMO).FormRevisionNo
            FormRevisionDate = mTransactionList.Item(Trans.WOCAMO).FormRevisionDate
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" And HeligoCallOutPrint = False Then 'added by Prashant on 11-Jan-2023 as Per mail According to work order type
                FormNo = "HCPL/CAME/02"
            End If
            'Added By Vikrant On 27-Jul-2020 For ALL27072020
        ElseIf mnWO.TransTypeID = 92 Then
            FormRevisionNo = mTransactionList.Item(Trans.SpareAssemblyWO).FormRevisionNo
            FormRevisionDate = mTransactionList.Item(Trans.SpareAssemblyWO).FormRevisionDate
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" And HeligoCallOutPrint = False Then 'added by Prashant on 11-Jan-2023 as Per mail According to work order type
                FormNo = "HCPL/QC/21"
            End If
        ElseIf mnWO.TransTypeID = 93 Then
            FormRevisionNo = mTransactionList.Item(Trans.SpareComponentWO).FormRevisionNo
            FormRevisionDate = mTransactionList.Item(Trans.SpareComponentWO).FormRevisionDate
            'End
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" And HeligoCallOutPrint = False Then 'added by Prashant on 11-Jan-2023 as Per mail According to work order type
                FormNo = "HCPL/QC/21"
            End If
        End If
        'End

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            mCompanyDetail.WebSite, ReportTitle, EOFooterLine, WODocumentNo, WORevisionNo, AppSettings("ClientCode"), FormNo, AppSettings("Product Version"),
            AppSettings("SINote"), SearchStr6:=IssueNo, SearchStr7:=Searchstr7, SearchStr8:=EmployeeName, SearchStr9:=AppSettings("Government Authority"), SearchStr10:=AppSettings("Logo"), SearchStr11:=LastLogDate,
            SearchStr12:=LastLogDateHavingAPUValues, SearchStr13:=AppSettings("CRS"),
            SearchStr14:=mnWO.WOJobs(0).WOJobTypeID.ToString, SearchStr15:=EmpName, SearchStr16:=IIf(SignatureRequired = True, "True", "False"),
            SearchStr17:=AirframeHrsAsOnCompletionDate, SearchStr18:=AFAllPeriodsAsOnCompletionDate, SearchStr19:=CompletedByUserLicenceNos,
            SearchStr21:=FormRevisionNo, SearchStr22:=FormRevisionDate, SearchStr23:=mnWO.WOJobs(0).OtherJob.ToString,
            SearchStr24:=mnWO.WOJobs(0).OtherJobSpecification.ToString, SearchStr25:=mnWO.LogNo, SearchStr26:=AppSettings("ShowMaintenanceForNewClients"),
            SearchStr27:=AppSettings("ShowCAMOOnlyForNewClients"), SearchStr28:=AppSettings("ShowAMOOnlyForNewClients"), SearchStr29:=mnWO.TransTypeID.ToString, SearchStr30:=IssueDate) 'Dont Use SearchStr20 

        Dim mrptImage As rptImage = rptImage.GetImage(ds, , "rptImage")


        da.Fill(ds, mnWO)
        da.Fill(ds, mnWOJobs)
        da.Fill(ds, mnWOJobComps)
        da.Fill(ds, mnWOJobDesignationAllocations) 'Added By Vikrant On 24-June-2013 For Indamer21062013
        da.Fill(ds, Report)
        da.Fill(ds, mnWOJobSpares)
        da.Fill(ds, mnWO.WOTools) 'Added By Prashant 13-Oct-2020 STR12102020
        da.Fill(ds, mrptImage)


        If AppSettings("ClientCode") = "RAL" Then
            mnWONRCJobs = mnWO.WONRCJobs
            da.Fill(ds, mnWONRCJobs)
        End If

        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        ''Dim str As String
        ''str = "<script language=Javascript>openTranDetail();</script>"
        ''ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", str)
        If ByMail = True Then 'Added By Prashant 1-Nov-2018  StarAir1112018
            'Do nothing 
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If
        Dim mWODetail As String
        mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
        MarkLog(Util.Action.Print, "Work Order", "Work Order Detail Print : " + mWODetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            'Added By Vikrant On 03-Jun-2016 For ALL03062016
            If Session("IsBackFromCompliance") = "True" Or Session("IsBackFromWO") = "True" Then
                Session.Remove("IsBackFromCompliance")
                RefreshGrid()
                Session.Remove("IsBackFromWO")
            End If
            'End
            If AppSettings("IsEngineeringWORequired").ToLower = "true" Then
                lblNote.Visible = True
                If AppSettings("ShowMaintenanceForNewClients").ToLower = "false" And AppSettings("ShowCAMOOnlyForNewClients").ToLower = "false" And AppSettings("ShowAMOOnlyForNewClients").ToLower = "false" Then
                    lblNote.Text = "For CAMO WO: Select Service, Inspection." + "<br />" + "For Engineering Order: Select Directive"
                Else
                    lblNote.Text = "For CAMO WO: Select AMP Tasks." + "<br />" + "For Engineering Order: Select Directive"
                End If
            Else
                lblNote.Visible = False
                lblNote.Text = ""
            End If
            DataFieldBind() 'Added By Vikrant on 14-Jun-2018 For ALL14062018
            GridBind()
            Session("MiddleFrame") = "wfDueResult_Ajax.aspx?"
        End If
        btnPrintTop.Visible = IIf(AppSettings("ClientCode") = "Heligo", True, False)
        btnPrint.Visible = IIf(AppSettings("ClientCode") = "Heligo", True, False)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnClseTop.Click
        'Added By Vikrant on 14-Jun-2018 For ALL14062018
        Session.Remove("mMachineNameValueList")
        Session.Remove("AsOnDateForWOCreation")
        Session.Remove("MachineIDForWOCreation")
        Session("mMachineList") = Nothing
        Session.Remove("URLFromDueReportPreview")
        'End
        If Session("wfSearchCriteriaForMaintenanceAdviceFromQC") = "wfSearchCriteriaForMaintenanceAdviceFromQC" Then
            Session.Remove("wfSearchCriteriaForMaintenanceAdviceFromQC")
            Session("MiddleFrame") = "wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax.aspx?DueType=" & Session("DueType").ToString
            Response.Redirect("Index.aspx")
        ElseIf Session("wfSearchCriteriaForDueWithAircraftSelection") = "wfSearchCriteriaForDueWithAircraftSelection" Then
            Session.Remove("wfSearchCriteriaForDueWithAircraftSelection")
            Session("MiddleFrame") = "wfSearchCriteriaForDueWithAircraftSelection.aspx?DueType=" & Session("DueType").ToString
            Response.Redirect("Index.aspx")
        Else
            Session("MiddleFrame") = "wfSearchCriteriaForDue_Ajax.aspx?DueType=" & Session("DueType").ToString
            Response.Redirect("Index.aspx")
        End If
    End Sub

    'Added By Vikrant On 03-Jun-2016 For ALL03062016
    Private Sub dgDueJob_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueJob.RowCommand
        Dim MainTypeID As Integer
        Dim MachineID, AssemblyStatusID, MaintID, ModelID, CompStatusID As Guid
        Dim MonitorDetail, DoneOnDate As String
        Dim IsApplicable As Boolean
        Dim mReportMaintenanceDetail As ReportMaintenanceDetail
        Select Case e.CommandName
            Case "Comply"
                mReportMaintenanceDetail = ReportMaintenanceDetails(New Guid(dgDueJob.DataKeys(CInt(e.CommandArgument)).Values(0).ToString))
                MainTypeID = mReportMaintenanceDetail.MaintenanceTypeID 'reportmaintdetailslist(CInt(e.CommandArgument)).MaintenanceTypeID
                MachineID = mReportMaintenanceDetail.MachineID  'reportmaintdetailslist(CInt(e.CommandArgument)).MachineID
                AssemblyStatusID = mReportMaintenanceDetail.AssemblyStatusID 'reportmaintdetailslist(CInt(e.CommandArgument)).AssemblyStatusID
                MaintID = mReportMaintenanceDetail.StatusID  'reportmaintdetailslist(CInt(e.CommandArgument)).StatusID
                ModelID = mReportMaintenanceDetail.ModelID 'reportmaintdetailslist(CInt(e.CommandArgument)).ModelID
                IsApplicable = mReportMaintenanceDetail.IsApplicable  'reportmaintdetailslist(CInt(e.CommandArgument)).IsApplicable
                CompStatusID = mReportMaintenanceDetail.CompStatusID 'reportmaintdetailslist(CInt(e.CommandArgument)).CompStatusID
                DoneOnDate = mReportMaintenanceDetail.DoneOnDate.ToString   'reportmaintdetailslist(CInt(e.CommandArgument)).DoneOnDate.ToString
                MonitorDetail = "Aircraft : " + mReportMaintenanceDetail.RegNo + " Assembly Info. : " + mReportMaintenanceDetail.AssemblySerialNo + " Monitor Info. : " + mReportMaintenanceDetail.MaintenanceTypeName + " Monitor Type : " + mReportMaintenanceDetail.MonitorType + " Description : " + mReportMaintenanceDetail.Description & " Done On Date : " & mReportMaintenanceDetail.DoneOnDate.ToString & " Done On Value : " & mReportMaintenanceDetail.DoneAt1

                Select Case MainTypeID
                    Case 5 'Assembly Service
                        GridBind()
                        If Not User.IsInRole("AssemblyServiceMonitorNew") Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        ComplyAssemblyService(MachineID, AssemblyStatusID, MaintID, ModelID, MonitorDetail)
                    Case 6 'Assembly Inspections
                        GridBind()
                        If Not User.IsInRole("AssemblyInspectionsNew") Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        ComplyAssemblyInspection(MachineID, AssemblyStatusID, MaintID, ModelID, MonitorDetail)
                    Case 7 'Assembly Directive
                        GridBind()
                        If Not User.IsInRole("AssemblyModificationsNew") Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Directive No. : " & mReportMaintenanceDetail.ModificationNumber
                        ComplyAssemblyDirective(MachineID, AssemblyStatusID, MaintID, ModelID, MonitorDetail, IsApplicable)
                    Case 8 'Component Service
                        GridBind()
                        If Not User.IsInRole("ComponentServiceMonitorNew") Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Part Info : " & mReportMaintenanceDetail.CompSerialNo
                        ComplyCompService(MachineID, AssemblyStatusID, MaintID, ModelID, MonitorDetail, CompStatusID, DoneOnDate)
                    Case 9 'Component Inspection
                        GridBind()
                        If Not User.IsInRole("ComponentInspectionsNew") Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Part Info : " & mReportMaintenanceDetail.CompSerialNo
                        ComplyCompInspection(MachineID, AssemblyStatusID, MaintID, ModelID, MonitorDetail, CompStatusID, DoneOnDate)
                    Case 10 'Component Modification
                        GridBind()
                        If Not User.IsInRole("ComponentModificationsNew") Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Part Info : " & mReportMaintenanceDetail.CompSerialNo & " Mod No. : " & mReportMaintenanceDetail.ModificationNumber
                        ComplyCompModification(MachineID, AssemblyStatusID, MaintID, ModelID, MonitorDetail, CompStatusID, DoneOnDate, IsApplicable)
                    Case Else

                End Select
            Case "History"
                mReportMaintenanceDetail = ReportMaintenanceDetails(New Guid(dgDueJob.DataKeys(CInt(e.CommandArgument)).Values(0).ToString))
                MainTypeID = mReportMaintenanceDetail.MaintenanceTypeID
                MachineID = mReportMaintenanceDetail.MachineID
                AssemblyStatusID = mReportMaintenanceDetail.AssemblyStatusID
                MaintID = mReportMaintenanceDetail.StatusID
                ModelID = mReportMaintenanceDetail.ModelID
                CompStatusID = mReportMaintenanceDetail.CompStatusID
                DoneOnDate = mReportMaintenanceDetail.DoneOnDate.ToString
                MonitorDetail = "Aircraft : " + mReportMaintenanceDetail.RegNo + " Assembly Info. : " + mReportMaintenanceDetail.AssemblySerialNo + " Monitor Info. : " + mReportMaintenanceDetail.MaintenanceTypeName + " Monitor Type : " + mReportMaintenanceDetail.MonitorType + " Description : " + mReportMaintenanceDetail.Description & " Done On Date : " & mReportMaintenanceDetail.DoneOnDate.ToString & " Done On Value : " & mReportMaintenanceDetail.DoneAt1

                Select Case MainTypeID
                    Case 5 'Assembly Service
                        GridBind()
                        If (Not User.IsInRole("AssemblyServiceMonitorView") And Not User.IsInRole("AssemblyServiceMonitorEdit")) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        AssServiceHistoryRecord(MachineID, AssemblyStatusID, MaintID, MonitorDetail)
                    Case 6 'Assembly Inspections
                        GridBind()
                        If (Not User.IsInRole("AssemblyInspectionsView") And Not User.IsInRole("AssemblyInspectionsEdit")) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        AssInspHistoryRecord(MachineID, AssemblyStatusID, MaintID, MonitorDetail)
                    Case 7 'Assembly Directive
                        GridBind()
                        If (Not User.IsInRole("AssemblyModificationsView") And Not User.IsInRole("AssemblyModificationsEdit")) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Mod No. : " & mReportMaintenanceDetail.ModificationNumber
                        AssDirectiveHistoryRecord(MachineID, AssemblyStatusID, MaintID, MonitorDetail)
                    Case 8 'Component Service
                        GridBind()
                        If (Not User.IsInRole("ComponentServiceMonitorView") And Not User.IsInRole("ComponentServiceMonitorEdit")) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Part Info : " & mReportMaintenanceDetail.CompSerialNo
                        CompServiceHistoryRecord(MachineID, AssemblyStatusID, MaintID, CompStatusID, DoneOnDate, MonitorDetail)
                    Case 9 'Component Inspection
                        GridBind()
                        If (Not User.IsInRole("ComponentInspectionsView") And Not User.IsInRole("ComponentInspectionsEdit")) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Part Info : " & mReportMaintenanceDetail.CompSerialNo
                        CompInspHistoryRecord(MachineID, AssemblyStatusID, MaintID, CompStatusID, DoneOnDate, MonitorDetail)
                    Case 10 'Component Modification
                        GridBind()
                        If (Not User.IsInRole("ComponentModificationsView") And Not User.IsInRole("ComponentModificationsEdit")) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        MonitorDetail = MonitorDetail & " Part Info : " & mReportMaintenanceDetail.CompSerialNo & " Mod No. : " & mReportMaintenanceDetail.ModificationNumber
                        CompModHistoryRecord(MachineID, AssemblyStatusID, MaintID, CompStatusID, DoneOnDate, MonitorDetail)
                    Case Else
                End Select
            Case "ViewSpareList" 'Added By Prashant 20-Dec-2018 
                Dim mStatusMasterID As Guid
                mStatusMasterID = New Guid(e.CommandArgument.ToString)
                Session("StatusMasterID") = mStatusMasterID
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSpareListWindow", "OpenSpareListWindow()", True)
            Case "WONumberRec"
                mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(New Guid(e.CommandArgument.ToString))
                Dim mnWO As nWO = nWO.GetWO(mnWOListForDueJobs(0).ID, False)
                Session("mnWO") = mnWO
                Session("IsShowAllWOs") = True
                Dim str As String
                str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End Select
    End Sub

    Private Sub hdnBtnInspectionHistory_Click(sender As Object, e As System.EventArgs) Handles hdnBtnInspectionHistory.Click 'Assembly Inspections
        RefreshGrid()
        GridBind()
        upnlGrid.Update()
    End Sub

    Private Sub hdnBtnDirectiveHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnDirectiveHistory.Click
        RefreshGrid()
        GridBind()
        upnlGrid.Update()
    End Sub

    Private Sub hdnBtnServiceHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnServiceHistory.Click
        RefreshGrid()
        GridBind()
        upnlGrid.Update()
    End Sub

    Private Sub hdnBtnInspHistory_Click(sender As Object, e As System.EventArgs) Handles hdnBtnInspHistory.Click 'Comp Inspections
        RefreshGrid()
        GridBind()
        upnlGrid.Update()
    End Sub

    Private Sub hdnBtnCompDirectiveHistory_Click(sender As Object, e As System.EventArgs) Handles hdnBtnCompDirectiveHistory.Click
        RefreshGrid()
        GridBind()
        upnlGrid.Update()
    End Sub

    Private Sub hdnBtnCompServiceHistory_Click(sender As Object, e As System.EventArgs) Handles hdnBtnCompServiceHistory.Click
        RefreshGrid()
        GridBind()
        upnlGrid.Update()
    End Sub
    'End
    'New addition by Rupali on 19-Jun-09 for Sorting Order

    Private Sub dgDueJob_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueJob.Sorting
        'ReportMaintenanceDetails.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        'ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
        'dgDueJob.DataSource = ReportMaintenanceDetails
        'DataBind()
        Dim mReportMaintenanceDetail As ReportMaintenanceDetail = reportmaintdetailslist.First
        Dim prop As System.Reflection.PropertyInfo
        prop = mReportMaintenanceDetail.GetType.GetProperties.First(Function(pr) pr.Name = e.SortExpression)
        reportmaintdetailslist = reportmaintdetailslist.OrderBy(Function(c) prop.GetValue(c, Nothing)).ToList
        Session("reportmaintdetailslist") = reportmaintdetailslist
        If cmbAircraft.SelectedValue.Equals(Guid.Empty.ToString) Then
            dgDueJob.DataSource = reportmaintdetailslist
            dgDueJob.DataBind()
            lblDuePeriodList.Text = "Due Job List : " & reportmaintdetailslist.Count.ToString & " record(s)"
            SetGrid()
        Else
            Dim mJobs = (From c As ReportMaintenanceDetail In reportmaintdetailslist
                         Where (c.RegNo.ToUpper().Contains(cmbAircraft.SelectedItem.ToString.ToUpper))
                         Select c).ToList
            dgDueJob.DataSource = mJobs
            dgDueJob.DataBind()
            lblDuePeriodList.Text = "Due Job List : " & mJobs.Count.ToString & " record(s)"
            SetGrid()
        End If
    End Sub

    'Added By Vikrant on 14-Jun-2018 For ALL14062018
    Private Sub lnkbtnCreateWorkOrder_Click(sender As Object, e As System.EventArgs) Handles lnkbtnCreateWorkOrder.Click
        chkMaintenanceTypeList = Request.Form("chkMaintenanceTypeList")
        MaintenanceTypeValues = chkMaintenanceTypeList.Split(","c)



        If CDate(txtFromDate.Text) < CDate(AsOnDateForWOCreation) Then
            MSGBoxCtrl.Show("Selection Alert!", "WO creation date should be greater than equal to " + AsOnDateForWOCreation, "", MsgBoxStyle.OkOnly, "WODateAlert")
            Exit Sub
        End If
        If cmbAircraft.SelectedValue.Equals(Guid.Empty.ToString) Then
            MSGBoxCtrl.Show("Selection Alert!", "Please select Aircraft for WO Creation.", "", MsgBoxStyle.OkOnly, "SelectAlert")
            Exit Sub
        End If
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.Show("Selection Alert!", "Please select at least one Scheduled Job for WO Creation.", "", MsgBoxStyle.OkOnly, "SelectAlert")
            Exit Sub
        Else
			'Adde By Vikrant On 22-Aug-2020
			If AppSettings("ClientCode") = "IND" Or
			   AppSettings("ClientCode") = "STR" Or
			   AppSettings("ClientCode") = "Deccan" Or
			   AppSettings("ClientCode") = "SPZ" Or
			   AppSettings("ClientCode") = "IPA" Or
			   AppSettings("ClientCode") = "FBW" Or
			   AppSettings("ClientCode") = "IRM" Or
			   AppSettings("ClientCode") = "AFC" Or
			   AppSettings("ClientCode") = "PTW" Or
				AppSettings("ClientCode") = "FIT" Or
				AppSettings("ClientCode") = "RAJ" Or
				AppSettings("ClientCode") = "ASH" Or
				AppSettings("ClientCode") = "SIT" Then ' AppSettings("ClientCode") = "SAP" Or SPZ Code added by Saylee on 13-Jun-2022 ''Deccan Code added by Vikrant On 16-Feb-2021
				Dim values As String() = checkString.Split(","c)
				If values.Count > 1 Then
					MSGBoxCtrl.Show("Selection Alert!", "Only One Scheduled Job can be selected at a time for WO Creation.", "", MsgBoxStyle.OkOnly, "SelectAlert")
					Exit Sub
				End If
			End If
			'End
			Session("IsBackFromWO") = "True"
            If AppSettings("IsEngineeringWORequired").ToLower = "true" Then
                ServiceCount = (From num In MaintenanceTypeValues
                                Where num = 5 Or num = 8
                                Select num).Count()
                InspectionCount = (From num In MaintenanceTypeValues
                                   Where num = 6 Or num = 9
                                   Select num).Count()
                ModificationCount = (From num In MaintenanceTypeValues
                                     Where num = 7 Or num = 10
                                     Select num).Count()

                If (ServiceCount > 0 Or InspectionCount > 0) And ModificationCount > 0 Then
                    MSGBoxCtrl.Show("Selection Alert!", "Please select either AMP Task or SB/AD to create a Work Order. The selected job types are different.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

            End If

            AddJobs(ServiceCount:=ServiceCount, InspectionCount:=InspectionCount, ModificationCount:=ModificationCount)
            Dim URLFromDueReportPreview As Stack = New Stack
            URLFromDueReportPreview.Push(Request.Url)
            Session("URLFromDueReportPreview") = URLFromDueReportPreview
            Response.Redirect("wfnWODetail_Ajax.aspx?BackPage=index.aspx")
        End If
    End Sub
    'End

    Private Sub dgDueJob_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDueJob.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim StatusMasterID As Guid = (DataBinder.Eval(e.Row.DataItem, "StatusMasterID"))
            mSpareListByMaintenanceActivity = SpareListByMaintenanceActivity.GetList(Today.Date.ToString, StatusMasterID.ToString)
            Dim grdDueJob As GridView = DirectCast(e.Row.FindControl("dgDueJob"), GridView)
            mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(StatusMasterID, True)
            mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(StatusMasterID)
            If mSpareListByMaintenanceActivity.Count = 0 And mMaintenanceKit.MaintenanceKitDetails.Count = 0 And mMaintenanceTask.MaintenanceTaskDetails.Count = 0 Then
                Dim btnImageButton As ImageButton = CType(e.Row.FindControl("btnImageButton"), ImageButton)
                btnImageButton.Visible = False
            End If

            ''Added by Prashant  9-Sep-2020 ALL09092020
            'Dim grdLinkActivity As GridView = DirectCast(e.Row.FindControl("grdLinkActivity"), GridView)        
            'Select Case CInt(e.Row.Cells(22).Text)
            '    Case 5 'Assembly Service
            '        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(StatusMasterID.ToString)
            '    Case 6 'Assembly Inspections
            '        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(StatusMasterID.ToString)
            '    Case 7 'Assembly Directive
            '        mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(StatusMasterID.ToString)
            '    Case Else
            'End Select

            'If Not mLinkMaintenanceList Is Nothing Then
            '    If mLinkMaintenanceList.Count > 0 Then
            '        e.Row.Cells(20).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
            '    End If

            '    grdLinkActivity.DataSource = mLinkMaintenanceList
            '    grdLinkActivity.DataBind()
            'End If

            'End of Added by Prashant  9-Sep-2020 ALL09092020
            'mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(StatusMasterID, True)
            'If mMaintenanceKit.MaintenanceKitDetails.Count = 0 Then
            '    Dim btnImageButtonViewToolsList As ImageButton = CType(e.Row.FindControl("btnImageButtonViewToolsList"), ImageButton)
            '    btnImageButtonViewToolsList.Visible = False
            'End If

            'mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(StatusMasterID)
            'If mMaintenanceTask.MaintenanceTaskDetails.Count = 0 Then
            '    Dim btnImageButtonViewTaskCardList As ImageButton = CType(e.Row.FindControl("btnImageButtonViewTaskCardList"), ImageButton)
            '    btnImageButtonViewTaskCardList.Visible = False
            'End If

        End If
    End Sub

#End Region

#Region "Checked Selection"

    'Added By Vikrant on 14-Jun-2018 For ALL14062018
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function

    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        RefreshGrid()
        Dim mJobs = (From c As ReportMaintenanceDetail In reportmaintdetailslist
                     Where (c.RegNo.ToUpper().Contains(cmbAircraft.SelectedItem.ToString.ToUpper))
                     Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                     Select c).ToList
        Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
        Session("reportmaintdetailslist") = reportmaintdetailslist
        dgDueJob.DataSource = mJobs
        dgDueJob.DataBind()
        lblDuePeriodList.Text = "Due Job List : " & mJobs.count.ToString & " record(s)"
        SetGrid()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'End

#End Region

    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click

        Dim mReportMaintenanceDetail As ReportMaintenanceDetail
        Dim reportmaintdetailslist1 As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)
        Dim checkString = Request.Form("chkSelect")

        If checkString Is Nothing Then

            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
            Exit Sub

        Else

            Dim values = checkString.Split(","c)
            If Not values Is Nothing Then
                For Each value As String In values
                    mReportMaintenanceDetail = ReportMaintenanceDetails(New Guid(value))
                    reportmaintdetailslist1.Add(mReportMaintenanceDetail)
                Next
            Else

            End If

            Dim da As New ObjectAdapter
            Dim ds As New dsReportMaintenanceDetail

            Dim ReportName, searchstr, searchstr1, Assembly1, searchstr6, ReferenceNo, searchstr8, searchstr16, x, OperatorName As String

            ReportName = "Weekly Call Out"
            searchstr = Session("searchstr")
            searchstr1 = Session("searchstr1")
            Assembly1 = Session("Assembly1")
            searchstr6 = Session("searchstr6")
            searchstr8 = Session("searchstr8")
            searchstr16 = Session("searchstr16")
            ReferenceNo = Session("ReferenceNo")
            x = Session("X")
            OperatorName = Session("OperatorName")
            Dim mCompanyDetail As New CompanyDetail
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")


            Dim rptDueDetail As Engine.ReportClass

            If Not rptMachineCertificates Is Nothing Then

                If rptMachineCertificates.Count = 0 Then
                    rptDueDetail = New crDueDetailAircraftWithCommentHeligo
                Else
                    rptDueDetail = New crDueDetailPerAircraftCertificatesWithCommentHeligo 'This change is applied to Heligo
                End If

            End If

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            mCompanyDetail.WebSite, ReportName, searchstr, searchstr1, Assembly1, AppSettings("ClientCode"), "Aircraft is flown up to: " & New SmartDate(x).FormattedText, mModuleList.Item("Due-PeriodWise").FormRevisionNo, AppSettings("SINote"), searchstr6, OperatorName, searchstr8, ReferenceNo, AppSettings("Logo"), AppSettings("FormNo"), mModuleList.Item("Due-PeriodWise").FormRevisionNo, "", "", SearchStr16:=searchstr16)
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            If rptMachineCertificates.Count <> 0 Then da.Fill(ds, "MachineCertificateList", rptMachineCertificates)
            da.Fill(ds, "ReportMaintenanceDetailList", reportmaintdetailslist1)
            da.Fill(ds, Report)
            da.Fill(ds, "rptStatusList", ReportStatusList)
            da.Fill(ds, rptSnagCorrectiveActionListForDue) 'Added By Prashant 20-Nov-2009
            da.Fill(ds, mrptImage)
            rptDueDetail.SetDataSource(ds)
            Session("CrystalReport") = rptDueDetail
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)

        End If

    End Sub

End Class