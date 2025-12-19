Imports CSLA

<Serializable()> _
Public Class GetCountList
	Inherits CSLA.ReadOnlyCollectionBase

#Region " Enumaration "
	Public Enum TransactionType
		Aircraft = 1
		Vendor = 2
		Part = 3
		Kit = 4
		Currency = 5
		Store = 6
		AlternatePart = 7
		City = 8
		Place = 9
		Manufacturer = 10
		Company = 11
		ATA = 12
		WorkShop = 13
		Employee = 14
		Technician = 15
		Skill = 16
		TechnicianSkill = 17
		MachinePhysical = 18
		PhysicalDetail = 19
		Department = 20
		TrainingOrganization = 21
		Training = 22
		UnitConverter = 23
		CompanyEquipment = 24
		EmployeeSkill = 25
		EmployeeServices = 26
		EmployeeTraining = 27
		EmployeeDocuments = 28
		EmployeeDesignation = 29
		EmployeeNextToKinInfo = 30
		EmployeeDisciplinary = 31
		EmployeeLeave = 32
		AircraftAssembly = 33
		AircraftAssemblyService = 34
		AircraftAssemblyInspection = 35
		AircraftAssemblyModification = 36
		AircraftComponent = 37
		AircraftComponentService = 38
		AircraftComponentInspection = 39
		AircraftComponentModification = 40
		TaskCard = 41
		EmployeeDepartment = 42
		Requisition = 43
		IssueListForUnusedRetrun = 44
		EquipmentCalibration = 45
		ManualSubscription = 46
		Serviceability = 47
		LogFuelOil = 48
		LogParameterList = 49
		WorkInvoice = 50
		Reliability = 51
		NewRequisitionForPurchaseApproval = 52
		IssueToRequisition = 53
		ReceivedFromAircraftAsCoreUnitReturn = 54
		ReceivedFromSupplierAsNone = 55
		IssueListForBER = 56

		LineMaintenanceOrder = 57
		LineMaintenanceInvoice = 58
		LineMaintenanceOrderRegister = 59
		LineMaintenanceInvoiceRegister = 60

		LogMaintenanceActivity = 61
		ExportInvoice = 62

		IssueToCustomerAsRepairedReturn = 63
		WOJobList = 64
		RecivedFromWorkOrder = 65
		IssuetoWorkOrderAsSpares = 66
		IssuetoWorkOrderAsTools = 67
		WOReturnIssue = 68
		RecivedFromWorkOrderAsReturn = 69
		IssuetoSupplierNone = 70
		FDTLEntry = 71
		FTL = 72
		FDTL = 73
		MultiCompliance = 74
		AircraftInformationBoard = 75
		ReceiptfromCustomer = 76
		ReceivedfromSupplierRental_Lease = 77
		IssuetoSupplierasRental_Lease = 78
		Schedule = 79
		Compliance = 80
		MEL_SnagCorrectiveAction = 81
		SalesEnquiry_EnquiryfromCustomer = 82
		SalesQuotation = 83
		SalesOrder = 84
		CreateRequisition = 85
		PurchaseOrder = 86
		ReceiptagainstPurchaseOrder = 87
		ReceiptcumInvoiceagainstPurchaseOrder = 88
		ReceivedfromStore = 89
		ReceivedfromAircraft = 90
		ReceivedfromVendor_exh_OH = 91
		ReceivedfromStoreLoanTaken = 92
		ReceivedfromAircraftReturned = 93
		ReceivedfromStoreLoanReturned = 94
		IssuetoAircraft = 95
		IssuetoStore = 96
		IssuetoVendor_Unitcare_exh_OH = 97
		IssuetoStoreLoanGiven = 98
		IssuetoAircraftLoanGiven = 99
		IssuetoStoreLoanReturned = 100
		IssueforPartDiscard = 101
		PurchaseInvoice = 102
		MaintenanceInvoice = 103
		Payment = 104
		PurchaseOtherChargesDocket = 105
		SalesInvoice = 106
		IssuetoCustomer = 107
		IssuetoCustomerLoanGiven = 108
		IssuetoVendorLoanGiven = 109
		ReceivedfromCustomerReturned = 110
		ReceivedfromVendorReturned = 111
		PurchaseOrderforExchange_Repair = 112
		PurchaseEnquiryRFQ = 113
		PurchaseQuotation = 114
		StoreValidation = 115
		EngineeringIssueApproval = 116
		FinanceApproval = 117
		EngineeringPurchaseApproval = 118
		PurchaseEnquiryforRepairandOverhaul = 119
		PurchaseEnquiryforRentalandLease = 120
		PurchaseQuotationforRepairandOverhaul = 121
		PurchaseQuotationforRentalandLease = 122
		PurchaseOrderforRepairandOverhaul = 123
		PurchaseOrderforRentalandLease = 124
		FlightLogBook = 125
		AssemblyRemoval = 126
		AssemblyInstallation = 127
		AssemblyServiceMonitor = 128
		AssemblyInspections = 129
		AssemblyDirectives = 130
		ComponentRemoval = 131
		ComponentInstallation = 132
		ComponentServiceMonitor = 133
		ComponentInspections = 134
		ComponentModifications = 135
		QCCallout = 136
		WorkOrder = 137
		WorkOrderInvoice = 138
		WOJobTaskList = 139
		LogisticPurchaseApproval = 140
		IssuetoWorkShop = 141
		IssuetoWorkShopLoanGiven = 142
		AssembledFromWorkShop = 143
		ReceiptagainstloanissuedtoWorkShop = 144
		DisassembledFromWorkShop = 145
		ReceivedFromCustomerAsForRepair = 146
		LoadandTrimSheet = 147
		ReceiptasLoanFromSupplier = 148
		IssueforLoanReturntoSupplier = 149
		ReceiptasLoanFromCustomer = 150
		IssueforLoanReturntoCustomer = 151
		IssuetoWorkOrder = 153
		RenewalCertificate = 153
		AllPurchaseOrders = 154 'Added By Kalpesh in 21-APR-13
	End Enum
#End Region

#Region " Data Structure "
	<Serializable()> _
	Public Structure GetCountInfo
		Private mGetCount As Integer

		Public Property GetCount() As Integer
			Get
				Return mGetCount
			End Get
			Set(ByVal Value As Integer)
				mGetCount = Value
			End Set
		End Property

	End Structure
#End Region

#Region " Business Properties and Method "
	Default Public ReadOnly Property Item(ByVal index As Integer) As GetCountInfo
		Get
			Return CType(List.Item(index), GetCountInfo)
		End Get
	End Property
#End Region

#Region "Shared Methods"
	Public Shared Function GetCountList(ByVal TransType As TransactionType) As GetCountList
		Return CType(DataPortal.Fetch(New Criteria(TransType)), GetCountList)
	End Function

	'Added By Kalpesh in 21-APR-13
	Public Shared Function GetPurchaseOrderCountList(ByVal TransType As TransactionType) As GetCountList

		Dim EnumID As Integer

		If TransType = 5 Then EnumID = 86
		If TransType = 31 Then EnumID = 112
		If TransType = 38 Then EnumID = 123
		If TransType = 39 Then EnumID = 124
		If TransType = 0 Then EnumID = 154

		Return CType(DataPortal.Fetch(New Criteria(EnumID)), GetCountList)
	End Function
#End Region

#Region "Criteria"
	'Criteria for identifing existing object
	<Serializable()> _
	Public Class Criteria
		Public TransType As TransactionType
		Public Sub New(ByVal TransType As TransactionType)
			Me.TransType = TransType
		End Sub
	End Class
#End Region

#Region "Constructor"
	Private Sub New()
		'Prevent direct creation
	End Sub
#End Region

#Region "Data Access"
	'Called bt DataPortal to load data from the database
	Protected Overrides Sub DataPortal_Fetch(ByVal Criteria As Object)
		Dim crit As Criteria = CType(Criteria, Criteria)
		Dim cn As New SqlConnection(DB("FlyPal"))
		Dim cm As New SqlCommand
		Try
			cn.Open()
			With cm
				.Connection = cn
				.CommandType = CommandType.StoredProcedure
				.CommandText = "GetCountListFetch"

				.Parameters.AddWithValue("@TransactionTypeID", CInt(crit.TransType))

				Dim dr As New CSLA.Data.SafeDataReader(.ExecuteReader)
				Try

					Dim info As GetCountInfo
					While dr.Read()
						info.GetCount = dr.GetInt32(0)

						InnerList.Add(info)
					End While
				Catch ex As Exception
					Throw ex.InnerException
				Finally
					dr.Close()
				End Try
			End With
		Catch ex As Exception
			Throw ex.InnerException
		Finally
			cn.Close()
		End Try
	End Sub
#End Region
End Class
