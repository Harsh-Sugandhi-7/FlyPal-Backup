'************************************
'Created by:	Harsh Sugandhi
'Created on:	15th September 2025
'************************************


Public Class WebConfigKeys

#Region " Variables "

	Public TempDir As String

	Public Mode As String

	Public Log As String

	Public ProductVersion As String '* 

	Public SINote As String

	Public DOCPath As String

	Public ClientCode As String

	Public APUHalf As String

	Public LogBookTimeEntry As String

	Public DateFormat As String

	Public TimeFormat As String

	Public dueDay As String

	Public EOFooterLine As String

	Public BillShipFromPrevOrder As String

	Public LastOrderCurrency As String

	Public WODocumentNo As String

	Public Barcode As String

	Public DateFormatLOG As String

	Public TimeFormatLOG As String

	Public DateTimeFormatLOG As String

	Public TakeOffTouchDown As String

	Public LogDetailPage As String

	Public FilePath As String

	Public Logo As String

	Public IsValZero As String

	Public TLP As String

	Public ReferenceNo As String

	Public AutoCompleteTransText As String

	Public LinkMaintenance As String

	Public NewRequisition As String

	Public PasswordSettings As String

	Public OtherChargeDocket As String

	Public DefaultImagePath As String

	Public IsSyncCRS As String

	Public IsSyncSMS As String

	Public SMSCompanyID As String

	Public GovernmentAuthority As String '*

	Public ShowNotInUse As String

	Public WOIssueNo As String

	Public nWOShowHrsInDecimal As String

	Public ShowKitItems As String

	Public AMTInWords As String

	Public GridPageSize As String

	Public PortNo As String

	Public AdvancePayment As String

	Public CRS As String

	Public CVService As String

	Public FormNo As String

	Public RevisionNo As String

	Public WoNo As String

	Public aspnetMaxHttpCollectionKeys As String

	Public LockBackDatedTransaction As String

	Public APUMultiplier As String

	Public ShippingAddress As String

	Public ShowForNotInUseAircrafts As String

	Public WarrantyForNewOH As String

	Public WarrantyForExchangeRepaired As String

	Public SetItemApplicability As String

	Public IsMAINTLogVOIDLogRequired As String

	Public ShowExtraMasterTabs As String

	Public CodeNo As String

	Public ChartImageHandler As String

	Public FormnNoInAudit As String

	Public IssueNoInAudit As String

	Public RevisionNoInAudit As String

	Public PrintBarCodeOnItemDetail As String

	Public IsShowAllRecordsVisible As String

	Public ShowLogDetailsOnAddingSameLogDate As String

	Public NoOfLogsToConsiderForAvgFlightTime As String

	Public DeviationInAvgFlightTimeInPercentage As String

	Public USWOAllowed As String

	Public IsGSTApplicable As String

	Public ConcurrentUsersRestriction As String

	Public ShowThrustMonitoring As String

	Public ShowImportLink As String

	Public SetAirbornePlusGroundAsBlockTime As String

	Public ChangeGSTPercentage As String

	Public StartingDateForCnEConsideration As String

	Public NoOfDaysForCnEConsideration As String

	Public PaymentAdviceToAC As String

	Public ACtoPaymentAdvice As String

	Public WONRCIssueRev As String '*

	Public WOCRSIssueRev As String '*

	Public ShowExtraLogTabs As String

	Public WorkOrderSubmitMail As String

	Public HTTPSecurity As String

	Public SetBlockTime As String

	Public SetModelCodeTypeWise As String

	Public InstallExistingAssemblyWithNewValue As String

	Public SaveAttachmentInSeperateDB As String

	Public FormNoInspReport As String

	Public ReleaseNoteNoRequire As String

	Public ShowAllValuesPageEnable As String

	Public ShowFirstPriorityParts As String

	Public FormNumberOnIssue As String

	Public IssueNumber As String

	Public IssueDate As String

	Public RevisionNumber As String

	Public RevisionDate As String

	Public FormNumberOnReceipt As String

	Public ShowWODashBoard As String

	Public ShowNewWOFlow As String

	Public CorporateID As String

	Public MELOccurranceInMonths As String

	Public MEL_Check_ON As String

	Public MEL_Occurrance_In_Days As String

	Public ServiceReferenceLocation As String

	Public MELSnagNomenclature As String

	Public ShowExportToExcelButton As String

	Public IsCustomerRequire As String

	Public HSNACSCodeVisibleInPartMaster As String

	Public setWOJobDescriptionFromPreviousSimilarWO As String

	Public ToAllowPrintTagForOpenReceipt As String

	Public ShowFanBladeDistributionMonitoring As String

	Public ShowWatermark As String

	Public ShowDashBoard As String

	Public ShowMaintenanceForNewClients As String

	Public ShowCAMOOnlyForNewClients As String

	Public ShowAMOOnlyForNewClients As String

	Public ShowNewDiscrepancyFlow As String

	Public IsGSTApplicableWOInvoice As String

	Public HSNACSCodeVisibleInCapabilityTaskMaster As String

	Public IsEngineeringWORequired As String

	Public ShowMultipleWOJobActions As String

	Public ShowMaintenanceForNewClientsWithTaskCard As String

	Public WOParametersRequired As String

	Public NewUi As String

	Public HostName As String

	Public CAMOAPPROVALREFERENCENO As String

	Public SubscriptionExtensionInDays As String

	Public ShowEngineDerateOptions As String

	Public LegacyDataPath As String

#End Region

#Region " Constructor "

	Public Sub New()

		Try

			TempDir = AppSettings("TempDir")
			Mode = AppSettings("Mode")
			Log = AppSettings("Log")
			ProductVersion = AppSettings("Product Version")
			SINote = AppSettings("SINote")
			DOCPath = AppSettings("DOCPath")
			ClientCode = AppSettings("ClientCode") ''Heligo / Indamer / Deccan /KamAir /RAL(Religare)

			LogBookTimeEntry = AppSettings("LogBookTimeEntry")
			DateFormat = AppSettings("DateFormat") '<!--Appsettings("DateFormat" )
			TimeFormat = AppSettings("TimeFormat") '<!--hh:mm tt / HH: mm HH: mm-->
			dueDay = AppSettings("dueDay")
			EOFooterLine = AppSettings("EOFooterLine")
			BillShipFromPrevOrder = AppSettings("BillShipFromPrevOrder") '<!--If "True" then get BillingAddress And ShippingAddress             of previous purchase order else default address --> 
			LastOrderCurrency = AppSettings("LastOrderCurrency") '<!--If "True" then get Last Order Currency of Supplier -->
			WODocumentNo = AppSettings("WODocumentNo")
			Barcode = AppSettings("Barcode")
			'<!--**********************Log Page Change ******************************************-->
			DateFormatLOG = AppSettings("DateFormatLOG") '<!--Date Format for Calander Control ofNew Log Page Same as DateFormat -->
			TimeFormatLOG = AppSettings("TimeFormatLOG") '<!--hh:mm TT :  12 Hour Format/hh:mm :  24 Hour Format =New Log Page Time Fomat for Calander Control -->
			DateTimeFormatLOG = AppSettings("DateTimeFormatLOG") '<!--hh:mm tt / HH: mm HH: mm-->
			TakeOffTouchDown = AppSettings("TakeOffTouchDown") '<!--True/False-->
			LogDetailPage = AppSettings("LogDetailPage") '<!--OldPage/NewPage-->
			'<!--************************************************************************************************************-->
			FilePath = AppSettings("FilePath")
			Logo = AppSettings("Logo") '<!--To show logo else 'False' -->
			IsValZero = AppSettings("IsValZero")
			TLP = AppSettings("TLP") '<!-- New Flight Log Page (Technical Log Page)  -->
			ReferenceNo = AppSettings("ReferenceNo") '<!--True/False--> '<!--for Reference No. field on Issue Details form   -->
			AutoCompleteTransText = AppSettings("AutoCompleteTransText") '<!--True/False-->
			LinkMaintenance = AppSettings("LinkMaintenance") '<!--True/False-->'<!--for Link Maintenance Actiity  -->
			NewRequisition = AppSettings("NewRequisition") '<!--True/False--> <!--for New Requisition  -->
			PasswordSettings = AppSettings("PasswordSettings") '<!-- True/False-->
			OtherChargeDocket = AppSettings("OtherChargeDocket") '<!-- True/False-->
			DefaultImagePath = AppSettings("DefaultImagePath") '<!-- Image folder path for reportDefault.jpg-->
			IsSyncCRS = AppSettings("IsSyncCRS") '<!-- True/False-->
			IsSyncSMS = AppSettings("IsSyncSMS") '<!-- True/False-->
			SMSCompanyID = AppSettings("SMSCompanyID")
			GovernmentAuthority = AppSettings("Government Authority")
			ShowNotInUse = AppSettings("ShowNotInUse") '<!--11-Sep-2012     True:Show All Aircrafts(IsInUse And NotInUse)/False:Only IsInUse Aircrats -->
			WOIssueNo = AppSettings("WOIssueNo")
			nWOShowHrsInDecimal = AppSettings("nWOShowHrsInDecimal") '<!-- True: To show EstimatedManHr of nWOJobTask And TaskCard form in Decimal. False: Show it in Hours: Minutes-->
			ShowKitItems = AppSettings("ShowKitItems") '<!-- True : To show Kit Items of each part in Purchase Order Print -->
			AMTInWords = AppSettings("AMTInWords") '<!--If Dollar then AMT in Million,Billion Or If INR then AMT in Crore, Lacs -->
			AMTInWords = AppSettings("GridPageSize")
			PortNo = AppSettings("PortNo") '<!--If 25 Or 587 -->
			AdvancePayment = AppSettings("AdvancePayment") '<!--True if want to show textbox Advance Payment on PO else false -->
			CRS = AppSettings("CRS")
			CVService = AppSettings("CVService")

			FormNo = AppSettings("FormNo") '<!-- "FORM NO. - HCPL/CAME/02" "Form No. - UHPL/CAME/03" "" -->
			RevisionNo = AppSettings("RevisionNo") '<!-- "Rev. No.: 01 of 07.04.2014"  "" -->
			WoNo = AppSettings("WoNo") '<!-- "WoNo. - HCPL/QC/43" -->
			aspnetMaxHttpCollectionKeys = AppSettings("aspnet:MaxHttpCollectionKeys")
			LockBackDatedTransaction = AppSettings("LockBackDatedTransaction")
			APUMultiplier = AppSettings("APUMultiplier")
			ShippingAddress = AppSettings("ShippingAddress")
			ShowForNotInUseAircrafts = AppSettings("ShowForNotInUseAircrafts") '<!-- 'Used only for : Only "ALL" criteria - fetching NotInUse Machine List only for Removed Components(wfRemovedCompList form)  -->
			WarrantyForNewOH = AppSettings("WarrantyForNewOH") '<!--1 year i.e 365 days-->
			WarrantyForExchangeRepaired = AppSettings("WarrantyForExchangeRepaired") '<!--6 months-->
			SetItemApplicability = AppSettings("SetItemApplicability") '<!-- True For allow False Not allow to add from Issue Item-->
			IsMAINTLogVOIDLogRequired = AppSettings("IsMAINTLogVOIDLogRequired") '<!-- True for allowing to enter MAINT Log/VOID Log (Only if DesUnivarsalTimings are available) else Set to False (for Heligo/UHPL : False)-->
			ShowExtraMasterTabs = AppSettings("ShowExtraMasterTabs") '<!-- True: shows Tabs Previous Reg, Leased Info, Maintenance Policy-->
			CodeNo = AppSettings("CodeNo")
			ChartImageHandler = AppSettings("ChartImageHandler")
			FormnNoInAudit = AppSettings("FormnNoInAudit")
			IssueNoInAudit = AppSettings("IssueNoInAudit")
			RevisionNoInAudit = AppSettings("RevisionNoInAudit")
			PrintBarCodeOnItemDetail = AppSettings("PrintBarCodeOnItemDetail")
			IsShowAllRecordsVisible = AppSettings("IsShowAllRecordsVisible")
			ShowLogDetailsOnAddingSameLogDate = AppSettings("ShowLogDetailsOnAddingSameLogDate") '<!--True,False -->
			NoOfLogsToConsiderForAvgFlightTime = AppSettings("NoOfLogsToConsiderForAvgFlightTime")
			DeviationInAvgFlightTimeInPercentage = AppSettings("DeviationInAvgFlightTimeInPercentage")
			USWOAllowed = AppSettings("USWOAllowed")
			IsGSTApplicable = AppSettings("IsGSTApplicable") '<!--True,False -->
			ConcurrentUsersRestriction = AppSettings("ConcurrentUsersRestriction")
			ShowThrustMonitoring = AppSettings("ShowThrustMonitoring")
			ShowImportLink = AppSettings("ShowImportLink")
			SetAirbornePlusGroundAsBlockTime = AppSettings("SetAirbornePlusGroundAsBlockTime")
			ChangeGSTPercentage = AppSettings("ChangeGSTPercentage")
			StartingDateForCnEConsideration = AppSettings("StartingDateForCnEConsideration")
			NoOfDaysForCnEConsideration = AppSettings("NoOfDaysForCnEConsideration") '<!--True,False -->
			PaymentAdviceToAC = AppSettings("PaymentAdviceToAC")
			ACtoPaymentAdvice = AppSettings("ACtoPaymentAdvice")
			WONRCIssueRev = AppSettings("WO-NRCIssueRev")
			WOCRSIssueRev = AppSettings("WO-CRSIssueRev")
			ShowExtraLogTabs = AppSettings("ShowExtraLogTabs")
			WorkOrderSubmitMail = AppSettings("WorkOrderSubmitMail") '<!-- True,False-->
			HTTPSecurity = AppSettings("HTTPSecurity") '<!-- http://,https:// -->
			SetBlockTime = AppSettings("SetBlockTime") '<!-- True,False Set True For APFT-->
			SetModelCodeTypeWise = AppSettings("SetModelCodeTypeWise") '<!-- True,False Set True For APFT-->
			InstallExistingAssemblyWithNewValue = AppSettings("InstallExistingAssemblyWithNewValue") '<!-- True,False Set True For APFT-->
			SaveAttachmentInSeperateDB = AppSettings("SaveAttachmentInSeperateDB") '<!-- True,False Set True For BEAS-->
			FormNoInspReport = AppSettings("FormNoInspReport")
			ReleaseNoteNoRequire = AppSettings("ReleaseNoteNoRequire")
			ShowAllValuesPageEnable = AppSettings("ShowAllValuesPageEnable") '<!-- True for View values change -->
			ShowFirstPriorityParts = AppSettings("ShowFirstPriorityParts") '<!-- True For BA-->
			'<!-- Following Keys used for HSC client on issue detail print-->
			FormNumberOnIssue = AppSettings("FormNumberOnIssue")
			IssueNumber = AppSettings("IssueNumber")
			IssueDate = AppSettings("IssueDate")
			RevisionNumber = AppSettings("RevisionNumber")
			RevisionDate = AppSettings("RevisionDate")
			FormNumberOnReceipt = AppSettings("FormNumberOnReceipt")
			'<!-- End Keys used for HSC client on issue detail print-->
			ShowWODashBoard = AppSettings("ShowWODashBoard") '<!-- True,False Set True For Indamar-->
			ShowNewWOFlow = AppSettings("ShowNewWOFlow") '<!-- True,False Set True For IND,ATL-->
			CorporateID = AppSettings("CorporateID")
			MELOccurranceInMonths = AppSettings("MELOccurranceInMonths")
			MEL_Check_ON = AppSettings("MEL_Check_ON") '<!-- This Value Is 0-ALL, 1-Calculate In Days, 2-Flight Days : used in MEL to check MEL occurance for last mentioned value (0, 1, 0R 2) to decide for marking as repeatitive MEL -->
			MEL_Occurrance_In_Days = AppSettings("MEL_Occurrance_In_Days")
			ServiceReferenceLocation = AppSettings("ServiceReferenceLocation") '<!-- Local,Remote -->
			MELSnagNomenclature = AppSettings("MELSnagNomenclature") '<!-- True for Passion False For All Others.If True MEL Is changed to ADD And Snag Is changed to Defect-->
			ShowExportToExcelButton = AppSettings("ShowExportToExcelButton")
			IsCustomerRequire = AppSettings("IsCustomerRequire")
			HSNACSCodeVisibleInPartMaster = AppSettings("HSNACSCodeVisibleInPartMaster")
			setWOJobDescriptionFromPreviousSimilarWO = AppSettings("setWOJobDescriptionFromPreviousSimilarWO") '<!-- True,False  set True for APFT False For Others-->
			'<!-- ************************************************************************Key to Delete********************************************************-->

			ToAllowPrintTagForOpenReceipt = AppSettings("ToAllowPrintTagForOpenReceipt") '<!-- True for BA False For All Others-->
			ShowExportToExcelButton = AppSettings("ShowExportToExcelButton")
			IsCustomerRequire = AppSettings("IsCustomerRequire")
			HSNACSCodeVisibleInPartMaster = AppSettings("HSNACSCodeVisibleInPartMaster")
			ShowFanBladeDistributionMonitoring = AppSettings("ShowFanBladeDistributionMonitoring")
			ShowWatermark = AppSettings("ShowWatermark") '<!-- Key Is used in PO for showing Preview on reports if printed before authorization -->
			ShowDashBoard = AppSettings("ShowDashBoard")
			ShowMaintenanceForNewClients = AppSettings("ShowMaintenanceForNewClients") '<!--True/False-->
			ShowCAMOOnlyForNewClients = AppSettings("ShowCAMOOnlyForNewClients") '<!--True/False-->
			ShowAMOOnlyForNewClients = AppSettings("ShowAMOOnlyForNewClients")
			ShowNewDiscrepancyFlow = AppSettings("ShowNewDiscrepancyFlow")
			IsGSTApplicableWOInvoice = AppSettings("IsGSTApplicableWOInvoice")
			HSNACSCodeVisibleInCapabilityTaskMaster = AppSettings("HSNACSCodeVisibleInCapabilityTaskMaster")
			IsEngineeringWORequired = AppSettings("IsEngineeringWORequired")
			ShowMultipleWOJobActions = AppSettings("ShowMultipleWOJobActions")
			ShowMaintenanceForNewClientsWithTaskCard = AppSettings("ShowMaintenanceForNewClientsWithTaskCard")
			WOParametersRequired = AppSettings("WOParametersRequired")
			NewUi = AppSettings("NewUi")
			HostName = AppSettings("HostName")
			CAMOAPPROVALREFERENCENO = AppSettings("CAMOAPPROVALREFERENCENO")
			SubscriptionExtensionInDays = AppSettings("SubscriptionExtensionInDays")
			ShowEngineDerateOptions = AppSettings("ShowEngineDerateOptions")
			LegacyDataPath = AppSettings("LegacyDataPath")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class