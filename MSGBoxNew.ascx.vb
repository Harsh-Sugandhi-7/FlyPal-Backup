Public Class MSGBoxNew
	Inherits System.Web.UI.UserControl

	Public Event UserControlButtonClicked As EventHandler

	Private _Title As String
	Private _Message As String
	Private mExtraMessage As String

	Private mResult As MsgBoxResult
	Private mSender As String

	Public Enum Message_text
		Save = 0
		Delete = 1
		CloseConfirm = 2
		Duplicate = 3
		SingleRecordConfirmation = 4
		SelectAtleastOne = 5
		CurrentlySelected = 6
		ErrorMessage = 7
		ReferenceDelete = 8
		PasswordValid = 9
		Authorization = 10
		ProcedureError = 11
		InvalidColumn = 12
		InvalidTable = 13
		SelectRestriction = 14
		AddNewMaster = 15
		Blank = 16
		CompulsorySelect = 17
		NumericOverFlow = 18
		TranItemOverFlow = 19
		AddSerializedItem = 20
		PenddingCustomer = 21
		ModelItemAvailable = 22
		ModelNotAssemblyType = 23
		InfoRequired = 24
		NoRecordFound = 25
		CheckQty = 26
		checkSelected = 27
		PendingAmount = 28
		BackConfirm = 29
		OpeningStockDeleteConfirm = 30
		OpeningStockUpdateConfirm = 31
		PendingQty = 32
		Exception = 33
		OneTimeMonitoring = 34
		Expiry = 35
		ComplyComponentMonitorServiceStatus = 36
		AirframeDelete = 37
		AssemblyStatusAcess = 38
		AcessList = 39
		MachineMonitor = 40
		ComponentPeriodExist = 41
		HoursRemove = 42
		StartDateRemove = 43
		DueLimit = 44
		AssemblyMonitorInspectionStatus = 45
		AssemblyMonitorModStatus = 46
		PeriodRequired = 47
		PeriodUnitRequired = 48
		AirframeEdit = 49
		MasterRecordEdit = 50
		MasterRecordRevert = 51
		RevertInstallation = 52
		PeriodExist = 53
		PeriodUnitExist = 54
		ConfirmRevert = 55
		AssemblyRemoved = 56
		CannotRevert = 57
		AssemblyAlreadyInstalled = 58
		MonitorExist = 59
		MonitorDone = 60
		ModificationMonitor = 61
		DoneOnDate = 62
		MonitoringNotApplicable = 63
		DatabaseException = 64
		ComponentIsRemoved = 65
		EntryRestriction = 66
		ComponentIsAlreadyInstalled = 67
		Restriction = 68
		saveAlert = 69
		ChargeAlert = 70
		StatusAuthorized = 71
		StatusCanceled = 72
		StatusAmended = 73
		Cancel = 74
		OrderCreate = 75
		OrderAdd = 76
		Password = 77
		ValidationAlert = 78
		LogExist = 79
		Amend = 80
		Custom = 81
		AttachmentAlert = 82
		NoAttachmentAlert = 83
		Remove = 84
		SelectConformation = 85
		StatusSubmitted = 86
		RemoveItem = 87
		RemoveCharge = 88
		CancelAircraft = 89
		MasterRecordDelete = 90
		NoOfAircrafts = 91
		NoneAircraftsChecked = 92
		PeriodNotPresent = 93
		FinancialYearSelection = 94
		LineMaintenanceReturn = 95
		PartExpired = 96
		BaseUnitEntry = 97
		DeleteAlert = 98
		EnterFlightLog = 99
		StatusCompleted = 100
		WOIssueReturn = 101
		Submission = 102
		BaseUnitEntryEdit = 103
		Discard = 104
		DiscardAuthorization = 105
		DiscardBER = 106  'Added by Saylee on 6-Nov-2012
		DiscardBERConfirmation = 107  'Added by Saylee on 6-Nov-2012
		CrewSelection = 108 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
		SameCrews = 109 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
		DutyAsSelection = 110 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
		SameDutyAs = 111 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
		Alert = 112 'Added by Prashant 14-Nov-2013 ' ALL11102013
		Confirmation = 113 'Added by Prashant 2-Jan-2014 
		ReferenceDeleting = 114
		RemoveTerm = 115
		SavedSuccessFully = 116
		SubmittedSuccessFully = 117
		CompletedSuccessFully = 118
		DeletedSuccessFully = 119
		CanceledSuccessFully = 120
		AuthorizedSuccessFully = 121
		AircraftNotConfigured = 122
		StatusPlanned = 123
		PlannedSuccessFully = 124
		RejectWO = 125
		RejectedWOSuccessFully = 126
	End Enum

	Public Enum Message_title
		Save = 0
		Delete = 1
		CloseConfirm = 2
		Duplicate = 3
		SingleRecordConfirmation = 4
		SelectAtleastOne = 5
		CurrentlySelected = 6
		ErrorMessage = 7
		ReferenceDelete = 8
		PasswordValid = 9
		Authorization = 10
		DataBaseError = 11
		SelectRestriction = 12
		NumericOverFlow = 13
		TranItemOverFlow = 14
		AddSerializedItem = 15
		PenddingCustomer = 16
		InfoRequired = 17
		NoRecordFound = 18
		CheckQty = 19
		CheckSelected = 20
		PendingAmount = 21
		BackConfirm = 22
		OpeningStockDeleteConfirm = 23
		OpeningStockUpdateConfirm = 24
		PendingQty = 25
		Exception = 26
		OneTimeMonitoring = 27
		Expiry = 28
		ComplyComponentMonitorServiceStatus = 29
		AirframeDelete = 30
		AssemblyStatusAcess = 31
		AcessList = 32
		MachineMonitor = 33
		ComponentPeriodExist = 34
		HoursRemove = 35
		StartDateRemove = 36
		DueLimit = 37
		AssemblyMonitorInspectionStatus = 38
		AssemblyMonitorModStatus = 39
		PeriodRequired = 40
		PeriodUnitRequired = 41
		AirframeEdit = 42
		MasterRecordEdit = 43
		MasterRecordRevert = 44
		RevertInstallation = 45
		PeriodExist = 46
		PeriodUnitExist = 47
		ConfirmRevert = 48
		AssemblyRemoved = 49
		CannotRevert = 50
		AssemblyAlreadyInstalled = 51
		MonitorExist = 52
		MonitorDone = 53
		ModificationMonitor = 54
		DoneOnDate = 55
		MonitoringNotApplicable = 56
		DatabaseException = 57
		ComponentIsRemoved = 58
		EntryRestriction = 59
		ComponentIsAlreadyInstalled = 60
		Restriction = 61
		SaveAlert = 62
		ChargeAlert = 63
		StatusAuthorized = 64
		StatusCanceled = 65
		StatusAmended = 66
		Cancel = 67
		OrderCreate = 68
		OrderAdd = 69
		Password = 70
		ValidationAlert = 71
		LogExist = 72
		Amend = 73
		Custom = 74
		AttachmentAlert = 75
		NoAttachmentAlert = 76
		Remove = 77
		SelectConformation = 78
		StatusSubmitted = 79
		RemoveItem = 80
		RemoveCharge = 81
		CancelAircraft = 82
		MasterRecordDelete = 83
		NoOfAircrafts = 84
		PeriodNotPresent = 85
		FinancialYearSelection = 86
		LineMaintenanceReturn = 87
		PartExpired = 88
		BaseUnitEntry = 89
		DeleteAlert = 90
		EnterFlightLog = 91
		StatusCompleted = 92
		WOIssueReturn = 93
		Submission = 94
		BaseUnitEntryEdit = 95
		Discard = 96
		DiscardAuthorization = 97
		DiscardBER = 98
		DiscardBERConfirmation = 99 'Added by Saylee on 6-Nov-2012
		CrewSelection = 108 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
		DutyAsSelection = 110 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
		SameDutyAs = 111 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
		Alert = 112 'Added by Prashant 14-Nov-2013 ' ALL11102013
		Confirmation = 113 'Added by Prashant 2-Jan-2014 
		ReferenceDeleting = 114
		RemoveTerm = 115
		SavedSuccessFully = 116
		SubmittedSuccessFully = 117
		CompletedSuccessFully = 118
		DeletedSuccessFully = 119
		CanceledSuccessFully = 120
		AuthorizedSuccessFully = 121
		AircraftNotConfigured = 122
		StatusPlanned = 123
		PlannedSuccessFully = 124
		RejectWO = 125
		RejectedWOSuccessFully = 126
	End Enum

	Public WriteOnly Property Message() As Integer
		Set(ByVal Value As Integer)
			Select Case Value
				Case 0
					_Message = "<p>Would you like to save the changes made in this record?</p>"
				Case 1
					_Message = "<p> Are you sure you want to delete this record? </p> "
				Case 2
					_Message = "<p>Would you like to save the changes made in this record? </p>"
				Case 3
					_Message = "<p> You are trying to add duplicate record. Only unique record is allowed. </p> "
				Case 4
					_Message = "<p> Please enter atleast one branch by clicking on the Add New Branch and Try Again. </p> "
				Case 5
					_Message = "<strong>  Please select atleast one </strong><p>" & mExtraMessage & "</p>"
				Case 6
					_Message = "<p> This record is already selected so you cannot delete this.</p> "
				Case 7
					_Message = "<strong> Error. </strong> "
				Case 8
					_Message = "This record cannot be deleted. It is used in other transaction(s) </p>"
				Case 9
					_Message = "<p>Please enter valid Username and Password.</p>"
				Case 10
					'_Message = "<strong> You have clicked to View the " & mExtraMessage & ". You Don\'t have the authority to View the " & mExtraMessage & " </strong> <p>Please Contact to the Administrator.</p>"
					_Message = "<strong> Access Denied.User not authorized</strong> <p>" & " Please contact the Administrator.</p>"
				Case 11
					_Message = "<strong> There is an error in database   </strong><p> " & mExtraMessage & "</p>"
				Case 12
					_Message = "<strong> There is an error in database   </strong><p> " & mExtraMessage & " </p>"
				Case 13
					_Message = "<strong>There is an error in database  </strong><p> " & mExtraMessage & " </p>"
				Case 14
					_Message = " <p>Please select " & mExtraMessage & " </p>"
				Case 15
					_Message = "<p> You are adding New </p>" & mExtraMessage & "But Your Currently Filled Data Will Vanished. Do You what to Continue?"
				Case 16
					_Message = "<strong> You have clicked on Delete Link Button </strong><p> You are deleting Blank Record. But You can not delete Blank Record </p>"
				Case 17
					_Message = "<p>Please select " & mExtraMessage & " </p>"
				Case 18
					_Message = "<p> Numerical Overflow occurred in the " & mExtraMessage & " . Record cannot be saved </p>"
				Case 19
					_Message = "<strong> You have selected the complete " & mExtraMessage & " </strong><p> But Item already exists in the ItemList. So you can not add it again. </p>"
				Case 20
					_Message = "<p> Serialized item is selected . " & mExtraMessage & ". </p>"
				Case 21
					_Message = "<strong> You have Click on the Complete Button.</strong><p> But Inquiry is Pending for the Customer </p>"
				Case 22
					_Message = "<p> Trying to make current Common Part No. as Non Assembly Type? Items already exists in this Common part.</p>"
				Case 23
					_Message = "<p> It cannot be added, because it is not marked as Assembly Type.</p>"
				Case 24 'Added by Kalpesh Dated: 15-Feb-06
					_Message = "<p> " & mExtraMessage & " information required  </p>"
				Case 25 'Added by Kalpesh Dated: 15-Feb-06
					_Message = "<strong> No record found </strong><p>" & mExtraMessage & "</strong> </p>"
				Case 26
					_Message = "</strong>" & mExtraMessage & "</strong>"  '"<strong> Check Quantity </strong><p>" & mExtraMessage & "</strong> </p>"
				Case 27
					_Message = "<strong> Select Same Supplier(s) Invoice  </strong><p>" & mExtraMessage & "</strong> </p>"
				Case 28
					_Message = "<p> Payment amount exceeding the balance limit.</p>"
				Case 29
					_Message = "<p> Would you like to save the changes made in this record? .</p> "
				Case 30
					_Message = "<p> Record cannot be saved, as its already used in " & mExtraMessage & "</p>"
				Case 31
					_Message = "<p> Opening stock quantity is Invalid. Record cannot be saved.</p>"
				Case 32
					_Message = "<p> Record cannot be saved." & mExtraMessage & "</p>"
				Case 33
					_Message = "<p> Record cannot be saved." & mExtraMessage & "</p>"
				Case 34
					_Message = "<strong>One Time Monitoring is already done. " & mExtraMessage & "Can not be complied again</p>"
				Case 35
					_Message = "<p>Expiry compliance is already done. " & mExtraMessage & "Can not be complied again</p>"
				Case 36
					_Message = "<strong>Comply Component Service Status</strong><p>" & mExtraMessage & "</p>"
				Case 37
					_Message = "<strong> Airframe cannot be deleted.</strong><p>You are trying to delete the airframe.Airframe can not be Deleted</p>"
				Case 38
					_Message = "<strong> Assembly Status is not accessible!</strong><p>You are trying to access assembly status. Assembly Status is not accessible without saving the aircraft.</p>"
				Case 39
					_Message = "<strong> Component Status is not accessible!</strong><p>" & mExtraMessage & "</p>"
				Case 40
					_Message = "<strong> Aircraft Monitor Entry!</strong><p>" & mExtraMessage & "</p>"
				Case 41
					_Message = "<strong> Component Period already exist</strong><p>" & mExtraMessage & "</p>"
				Case 42
					_Message = "<strong> Period remove restriction</strong><p>" & mExtraMessage & "</p>"
				Case 43
					_Message = "<strong> Start Date remove restriction</strong><p>" & mExtraMessage & "</p>"
				Case 44
					_Message = "<strong> Select Proper Due Limit</strong><p>" & mExtraMessage & "</p>"
				Case 45
					_Message = "<strong>Trying to Save Assembly Inspection Status</strong><p>" & mExtraMessage & "</p>"
				Case 46
					_Message = "<strong>Trying to Save Assembly Modification Status</strong><p>" & mExtraMessage & "</p>"
				Case 47
					_Message = "<strong>Record can not be saved without Period.</strong><p>" & mExtraMessage & "</p>"
				Case 48
					_Message = "<strong>Record can not be saved without Period Unit.</strong><p>" & mExtraMessage & "</p>"
				Case 49
					_Message = "<strong>Airframe can not be edited.</strong><p>" & mExtraMessage & "</p>"
				Case 50
					_Message = "<strong>Master record can not be edited from here.</strong><p>" & mExtraMessage & "</p>"
				Case 51
					_Message = "<strong>Master record can not be Reverted from here.</strong><p>" & mExtraMessage & "</p>"
				Case 52
					_Message = "<strong>Currently Removed.First Revert Removal and then Revert Installation.</strong><p>" & mExtraMessage & "</p>"
				Case 53
					_Message = "<strong>Period already present.</strong><p>" & mExtraMessage & "</p>"
				Case 54
					_Message = "<strong>Period Unit already present.</strong><p>" & mExtraMessage & "</p>"
				Case 55
					_Message = "<strong>Confirm revert installation.</strong><p>" & mExtraMessage & "</p>"
				Case 56
					_Message = "<strong>Assembly is currently removed.</strong><p>" & mExtraMessage & "</p>"
				Case 57
					_Message = "<strong>Can not revert the installation.</strong><p>" & mExtraMessage & "</p>"
				Case 58
					_Message = "<strong>Selected Assembly Already Installed.</strong><p>" & mExtraMessage & "</p>"
				Case 59
					_Message = "<strong>Aircraft Monitor Entry Already Exist.</strong><p>" & mExtraMessage & "</p>"
				Case 60
					_Message = "<strong>Monitoring is Already done.</strong><p>" & mExtraMessage & "</p>"
				Case 61
					_Message = "<strong>Modificaion is not applicable.</strong><p>" & mExtraMessage & "</p>"
				Case 62
					_Message = "<strong>Selectd date is out of valid range.</strong><p>" & mExtraMessage & "</p>"
				Case 63
					_Message = "<strong>Monitoring is not applicable.</strong><p>" & mExtraMessage & "</p>"
				Case 64
					_Message = "<strong>Database Exception</strong><p>" & mExtraMessage & "</p>"
				Case 65
					_Message = "<strong>Already Removed, can't remove again.</strong><p>" & mExtraMessage & "</p>"
				Case 66
					_Message = "<strong>Aircraft does not have installed assembly</strong><p>" & mExtraMessage & "</p>"
				Case 67
					_Message = "<strong>Selected Component Already Installed.</strong><p>" & mExtraMessage & "</p>"
				Case 68
					_Message = "<strong>Assembly Required for Selected Aircraft</strong><p>" & mExtraMessage & "</p>"
				Case 69
					_Message = "<strong>Record can not be saved.</strong><p>" & mExtraMessage & "</p>"
				Case 70
					_Message = "<strong>Charge can not Added.</strong><p>" & mExtraMessage & "</p>"
				Case 71
					_Message = "<strong>Do you want to Authorize the </strong>" & mExtraMessage & "?"
				Case 72
					_Message = "<strong>Do you want to Cancel the </strong>" & mExtraMessage & "?"
				Case 73
					_Message = "<strong>Do you want to Amend the </strong>" & mExtraMessage & "?"
				Case 74
					_Message = "<Strong>You can not cancel this </strong>" & mExtraMessage
				Case 75
					_Message = "<Strong>New Order Created Successfully </strong>" & mExtraMessage
				Case 76
					_Message = "<Strong>Selected Item is Added in the Order Successfully </strong>" & mExtraMessage
				Case 77
					_Message = "<Strong></strong>" & mExtraMessage
				Case 78
					_Message = "<Strong>Record is Not Valid </strong><p>" & mExtraMessage & "</p>"
				Case 79
					_Message = "<strong>Log Already entered </strong><p><p>" & mExtraMessage & "</p>"
				Case 80
					_Message = "<strong>You can not Amend this </strong>" & mExtraMessage
				Case 81
					_Message = "<strong>" & _Message & "</strong><p>" & mExtraMessage & "</p>"
				Case 82
					_Message = "<strong>This file is already attached to this Document.</strong><p>" & mExtraMessage & "</p>"
				Case 83
					_Message = "<strong>There is no file attached to this Document or Path was not set.</strong><p>" & mExtraMessage & "</p>"
				Case 84
					_Message = "Are you sure you want to Remove this?"
				Case 85
					_Message = "<strong> You have selected same store names. </strong> <p>" & mExtraMessage & "</p> "
				Case 86
					_Message = "<strong>Would like to submit the </strong>" & mExtraMessage & "?"
				Case 87
					_Message = "<strong> You have clicked on the Remove Link to Remove Item. </strong> <p> Are you sure to Remove this Item? Click on Yes to Remove the Current Item. No to Cancel Remove. </p> "
				Case 88
					_Message = "<strong> You have clicked on the Remove Link to Remove Charge. </strong> <p> Are you sure to Remove this Charge? Click on Yes to Remove the Current Charge. No to Cancel Remove. </p> "
				Case 89
					_Message = "<strong> You have taken decision to cancel the registration of this Aircraft.</strong><p>Are you Sure to Cancel this?</p><p>" & mExtraMessage & "</p>"
				Case 90
					_Message = "<strong>Master record can not be Deleted from here.</strong><p>" & mExtraMessage & "</p>"
				Case 91
					_Message = "<strong>Please check only 10 number of Aircrafts at a time.</strong><p>" & mExtraMessage & "</p>"
				Case 92
					_Message = "<strong>Please check atleast 1 Aircraft.</strong><p>" & mExtraMessage & "</p>"
				Case 93
					_Message = "<strong>Period is not present in Assembly/Component Status.</strong><p>" & mExtraMessage & "</p>"
				Case 94
					_Message = "<strong>From Year and To Year Should not be same. There must be one year difference.</strong><p>" & mExtraMessage & "</p>"
				Case 95
					_Message = "<strong>Do you want to Open the </strong>" & mExtraMessage & "?"
				Case 96
					_Message = "<strong>This Part has Expired, you are trying to Issue Expired Part </strong>" & mExtraMessage & "?"
				Case 97
					_Message = "<strong>Base Unit Entry cannot Deleted </strong><p>" & mExtraMessage & "</p>"
				Case 98
					_Message = "<strong>You have clicked on the Delete link to Delete this entry. </strong><p>" & mExtraMessage & "</p>"
				Case 99
					_Message = "<strong>Enter Flight Log. </strong><p>" & mExtraMessage & "</p>"
				Case 100
					_Message = "<strong>Do you want to Complete the </strong>" & mExtraMessage & "?"
				Case 101
					_Message = "<strong>Do you want to Open the </strong>" & mExtraMessage & "?"
				Case 102
					_Message = "<strong>Do you want to " + IIf(AppSettings("ClientCode") = "IND", "Authorize", "Submit") + " the </strong>" & mExtraMessage & "?"
				Case 103
					_Message = "<strong>Base Unit Entry cannot be Edited </strong><p>" & mExtraMessage & "</p>"
				Case 104
					_Message = "<strong>You are about to Discard Serialized/Rotable Part(s).</strong> <p>Do You want to continue?</p> " & mExtraMessage
				Case 105
					_Message = "<strong> You are about to Authorize Discard Serialized/Rotable Part(s). </strong> <p>Do You want to continue?</p> " & mExtraMessage
				Case 106
					_Message = "<strong> You have Discarded BER part.</strong><p>" & mExtraMessage & "</p>"
				Case 107
					_Message = "<strong> You have clicked on the Discard link to Discard this entry. </strong> <p> Are you sure to discard this? Click on Yes to Discard the Current Record. No to Cancel discard. </p> "
				Case 108 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
					_Message = "<strong>  Select Crew 1 and Crew 2.</strong><p>" & mExtraMessage & "</p>"
				Case 109 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
					_Message = "<strong> Crew 1 and Crew 2 should not be same.</strong><p>" & mExtraMessage & "</p>"
				Case 110 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
					_Message = "<strong> Select Duty As 1 and Duty As 2.</strong><p>" & mExtraMessage & "</p>"
				Case 111 'Added by Prashant 17-Oct-2013 ' ALL03102013-1
					_Message = "<strong> Duty As 1 and Duty As 2 should not be same.</strong><p>" & mExtraMessage & "</p>"
				Case 112 'Added by Prashant 14-Nov-2013 ' ALL11102013
					_Message = "<strong> Alert! </strong><p>" & mExtraMessage & "</p>"
				Case 113 'Added by Prashant 2-Jan-2014 
					_Message = "<strong>Confirmation!</strong><p>" & mExtraMessage & "</p>"
				Case 114
					_Message = "Entry cannot be deleted. It is already used in " & mExtraMessage & "</p>"
				Case 115
					_Message = "<strong>Are you sure you want to Remove this Term? </strong>"
				Case 116
					_Message = "<strong>Record Saved Successfully!!</strong><p>" & mExtraMessage & "</p>"
				Case 117
					_Message = "<strong>Record Submitted Successfully!!</strong><p>" & mExtraMessage & "</p>"
				Case 118
					_Message = "<strong>Record Completed Successfully!!</strong><p>" & mExtraMessage & "</p>"
				Case 119
					_Message = "<strong>Record Deleted Successfully!!</strong><p>" & mExtraMessage & "</p>"
				Case 120
					_Message = "<strong>Record Canceled SuccessFully</strong><p>" & mExtraMessage & "</p>"
				Case 121
					_Message = "<strong>Record Authorized SuccessFully</strong><p>" & mExtraMessage & "</p>"
				Case 122
					_Message = "Aircraft Not Configured In Master <p>" & mExtraMessage & "</p>"
				Case 123
					_Message = "<strong>Do you want to Plan the </strong>" & mExtraMessage & "?"
				Case 124
					_Message = "<strong>Record Planned Successfully!!</strong><p>" & mExtraMessage & "</p>"
				Case 125
					'RejectWO 
					_Message = "<strong>Do you want to Reject the </strong>" & mExtraMessage & "?"
				Case 126
					'RejectedWOSuccessFully
					_Message = "<strong>Record Rejected Successfully!!</strong><p>" & mExtraMessage & "</p>"
				Case Else
			End Select
		End Set
	End Property

	Public WriteOnly Property Title() As Integer
		Set(ByVal Value As Integer)
			Select Case Value
				Case 0
					_Title = "Save Confirmation!"
				Case 1
					_Title = "Delete Confirmation!"
				Case 2
					_Title = "Close Confirmation!"
				Case 3
					_Title = "Duplicate Alert!"
				Case 4
					_Title = "Required Entry Alert!"
				Case 5
					_Title = "Selection Alert!"
				Case 6
					_Title = "Deletion Alert!"
				Case 7
					_Title = "Error !"
				Case 8
					_Title = "Reference !"
				Case 9
					_Title = "Invalid Password !"
				Case 10
					_Title = "Not Authorized !"
				Case 11
					_Title = "Database Error !"
				Case 12
					_Title = "Selection Warning !"
				Case 13
					_Title = "Numeric Overflow!"
				Case 14
					_Title = "Transaction Item Overflow!"
				Case 15
					_Title = "Add serialized item!"
				Case 16
					_Title = "Pending Customer Alert!"
				Case 17
					_Title = "Information Required Alert!"
				Case 18
					_Title = "Information Alert!"
				Case 19
					_Title = "Check Quantity!"
				Case 20
					_Title = "Check Selection"
				Case 21
					_Title = "Pending Amount Alert !"
				Case 22
					_Title = "Back Confirmation!"
				Case 23
					_Title = "Opening Stock Delete Confirmation!"
				Case 24
					_Title = "Opening Stock Update Alert!"
				Case 25
					_Title = "Pending Quantity Alert!"
				Case 26
					_Title = "Exception!"
				Case 27
					_Title = "Monitoring!"
				Case 28
					_Title = "Expiry!"
				Case 29
					_Title = "Master record can not be edited!"
				Case 30
					_Title = "Airframe deleted!"
				Case 31
					_Title = "Assembly Status Access!"
				Case 32
					_Title = "Status List Access!"
				Case 33
					_Title = "Aircraft Monitor!"
				Case 34
					_Title = "Component Period exist!"
				Case 35
					_Title = "Period Remove!"
				Case 36
					_Title = "Start Date Remove!"
				Case 37
					_Title = "Invalid Due Limit!"
				Case 38
					_Title = "Assembly Inspection Staus!"
				Case 39
					_Title = "Assembly Modification Staus!"
				Case 40
					_Title = "Period Rquired!"
				Case 41
					_Title = "Period Unit Rquired!"
				Case 42
					_Title = "Airframe Edit!"
				Case 43
					_Title = "Master Record Edit!"
				Case 44
					_Title = "Master Record Revert!"
				Case 45
					_Title = "Revert Installation!"
				Case 46
					_Title = "Period already exist!"
				Case 47
					_Title = "Period unit already exist!"
				Case 48
					_Title = "Confirm Revert!"
				Case 49
					_Title = "Assembly currently removed!"
				Case 50
					_Title = "Revert Assembly Installation!"
				Case 51
					_Title = "Assembly already installed!"
				Case 52
					_Title = "Monitor Exist!"
				Case 53
					_Title = "Monitoring Done!"
				Case 54
					_Title = "Modification Monitor!"
				Case 55
					_Title = "Done on date validation!"
				Case 56
					_Title = "Monitoring Not Applicable!"
				Case 57
					_Title = "Database Exception!"
				Case 58
					_Title = "Component Remove!"
				Case 59
					_Title = "Entry Restriction!"
				Case 60
					_Title = "Component Already installed!"
				Case 61
					_Title = "Restriction!"
				Case 62
					_Title = "Save Alert!"
				Case 63
					_Title = "Charge Alert!"
				Case 64
					_Title = "Authorization!"
				Case 65
					_Title = "Cancellation!"
				Case 66
					_Title = "Amend!"
				Case 67
					_Title = "Cancellation!"
				Case 68
					_Title = "Order Created!"
				Case 69
					_Title = "Order Added!"
				Case 70
					_Title = "Password!"
				Case 71
					_Title = "Validation Alert!"
				Case 72
					_Title = "Log Already Exists!"
				Case 73
					_Title = "Amend!"
				Case 74
					_Title = _Title
				Case 75
					_Title = "Attachment Alert!"
				Case 76
					_Title = "No Attachment Alert!"
				Case 77
					_Title = "Remove Confirmation!"
				Case 78
					_Title = "Selection Alert!"
				Case 79
					_Title = "Status Submit!"
				Case 80
					_Title = "Remove Item Confirmation!"
				Case 81
					_Title = "Remove Charge Confirmation!"
				Case 82
					_Title = "Cancel Aircraft!"
					'CancelAircraft
				Case 83
					_Title = "Master Record Delete!"
					'MasterRecordDelete
				Case 84
					_Title = "Aircraft Limit!"
				Case 85
					_Title = "Period Not Present !"
				Case 86
					_Title = "Financial Year Selection Alert!"
				Case 87
					_Title = "Line Maintenance Return!"
				Case 88
					_Title = "Part Expired !"
				Case 89
					_Title = "Base Unit Entry"
				Case 90
					_Title = "Delete Alert !"
				Case 91
					_Title = "Information Alert!"
				Case 92
					_Title = "Completion!"
				Case 93
					_Title = "Issue Return!"
				Case 94
					_Title = IIf(AppSettings("ClientCode") = "IND", "Authorization!", "Submission!")
				Case 95
					_Title = "Base Unit Entry Edit"
				Case 96
					_Title = "Discard Confirmation!"
				Case 97
					_Title = "Discard Authorization!"
				Case 98
					_Title = "BER Part Discarded Successfully!"
				Case 99
					_Title = "Discard Confirmation!"
				Case 108  'Added by Prashant 17-Oct-2013 ' ALL03102013-1
					_Title = "Crew Selection!"
				Case 110  'Added by Prashant 17-Oct-2013 ' ALL03102013-1
					_Title = "Duty As Selection!"
				Case 112  'Added by Prashant 14-Nov-2013 ' ALL11102013
					_Title = "Alert!"
				Case 113  'Added by Prashant 2-Jan-2014 
					_Title = "Confirmation!"
				Case 114
					_Title = "Reference !"
				Case 116
					_Title = "Save !"
				Case 117
					_Title = "Successful Submission !"
				Case 118
					_Title = "Successful Completion !"
				Case 119
					_Title = "Successful Deletion !"
				Case 120
					_Title = "Successful Cancellation !"
				Case 121
					_Title = "Successful Authorization !"
				Case 122
					_Title = "Aircraft Not Configured !"
				Case 123
					_Title = "Work Order Planning !"
				Case 124
					_Title = "Planned Successfully !"
				Case 125
					_Title = "Work Order Rejection !"
				Case 126
					_Title = "Rejected Successfully !"
			End Select
		End Set
	End Property

	Public Sub HideControl()
		mdlPopupBox.Hide()
	End Sub

	Public ReadOnly Property Result() As MsgBoxResult
		Get
			Return mResult
		End Get
	End Property

	Public ReadOnly Property Sender As String
		Get
			Return mSender
		End Get
	End Property

	Private Sub DisplayButtons(ByVal button1 As MsgBoxStyle)
		ButtonDiv.Controls.Clear()

		Dim btnOk, btnCancel, btnAbort, btnRetry, btnIgnore, btnYes, btnNo As Button

		Select Case button1
			Case MsgBoxStyle.OkOnly             '0
				btnOk = New Button
				btnOk.Text = "Ok"
				btnOk.ID = "btnOk"
				btnOk.ForeColor = Color.White
				btnOk.CssClass = "styled-button-2"
				btnOk.Attributes.Add("runat", "server")
				btnOk.CausesValidation = False
				ButtonDiv.Controls.Add(btnOk)
				AddHandler btnOk.Click, AddressOf btnOk_Click

			Case MsgBoxStyle.OkCancel           '1
				btnOk = New Button
				btnOk.ID = "btnOk"
				btnOk.Text = "Ok"
				btnOk.ForeColor = Color.White
				btnOk.CssClass = "styled-button-2"
				btnOk.Attributes.Add("runat", "server")
				btnOk.CausesValidation = False

				btnCancel = New Button
				btnCancel.ID = "btCancel"
				btnCancel.Text = "Cancel"
				btnCancel.ForeColor = Color.White
				btnCancel.CssClass = "styled-button-2"
				btnCancel.Attributes.Add("runat", "server")
				btnCancel.CausesValidation = False
				Dim label1 As New Label
				label1.Width = 3

				ButtonDiv.Controls.Add(btnOk)
				ButtonDiv.Controls.Add(label1)
				ButtonDiv.Controls.Add(btnCancel)

				AddHandler btnOk.Click, AddressOf btnOk_Click
				AddHandler btnCancel.Click, AddressOf btnCancel_Click

			Case MsgBoxStyle.AbortRetryIgnore   '2
				btnAbort = New Button
				btnAbort.Text = "Abort"
				btnAbort.ID = "btnAbort"
				btnAbort.ForeColor = Color.White
				btnAbort.CssClass = "styled-button-2"
				btnAbort.Attributes.Add("runat", "server")
				btnAbort.CausesValidation = False

				btnRetry = New Button
				btnRetry.Text = "Retry"
				btnRetry.ID = "btnRetry"
				btnRetry.ForeColor = Color.White
				btnRetry.CssClass = "styled-button-2"
				btnRetry.Attributes.Add("runat", "server")
				btnRetry.CausesValidation = False

				btnIgnore = New Button
				btnIgnore.Text = "Ignore"
				btnIgnore.ID = "btnIgnore"
				btnIgnore.ForeColor = Color.White
				btnIgnore.CssClass = "styled-button-2"
				btnIgnore.Attributes.Add("runat", "server")
				btnIgnore.CausesValidation = False

				Dim label1 As New Label
				label1.Width = 3

				Dim label2 As New Label
				label2.Width = 3

				ButtonDiv.Controls.Add(btnAbort)
				ButtonDiv.Controls.Add(label1)

				ButtonDiv.Controls.Add(btnRetry)
				ButtonDiv.Controls.Add(label2)

				ButtonDiv.Controls.Add(btnIgnore)

				AddHandler btnAbort.Click, AddressOf btnAbort_Click
				AddHandler btnRetry.Click, AddressOf btnRetry_Click
				AddHandler btnIgnore.Click, AddressOf btnIgnore_Click

			Case MsgBoxStyle.YesNoCancel        '3
				btnYes = New Button
				btnYes.Text = "Yes"
				btnYes.ID = "btnYes"
				btnYes.ForeColor = Color.White
				btnYes.CssClass = "styled-button-2"
				btnYes.Attributes.Add("runat", "server")
				btnYes.CausesValidation = False

				btnNo = New Button
				btnNo.Text = "No"
				btnNo.ID = "btnNo"
				btnNo.ForeColor = Color.White
				btnNo.CssClass = "styled-button-2"
				btnNo.Attributes.Add("runat", "server")
				btnNo.CausesValidation = False

				btnCancel = New Button
				btnCancel.Text = "Cancel"
				btnCancel.ID = "btnCancel"
				btnCancel.ForeColor = Color.White
				btnCancel.CssClass = "styled-button-2"
				btnCancel.Attributes.Add("runat", "server")
				btnCancel.CausesValidation = False

				Dim label1 As New Label
				label1.Width = 3

				Dim label2 As New Label
				label2.Width = 3

				ButtonDiv.Controls.Add(btnYes)
				ButtonDiv.Controls.Add(label1)

				ButtonDiv.Controls.Add(btnNo)
				ButtonDiv.Controls.Add(label2)

				ButtonDiv.Controls.Add(btnCancel)

				AddHandler btnYes.Click, AddressOf btnYes_Click
				AddHandler btnNo.Click, AddressOf btnNo_Click
				AddHandler btnCancel.Click, AddressOf btnCancel_Click

			Case MsgBoxStyle.YesNo              '4
				btnYes = New Button
				btnYes.Text = "Yes"
				btnYes.ID = "btnYes"
				btnYes.ForeColor = Color.White
				btnYes.CssClass = "styled-button-2"
				btnYes.Attributes.Add("runat", "server")
				btnYes.CausesValidation = False

				btnNo = New Button
				btnNo.Text = "No"
				btnNo.ID = "btnNo"
				btnNo.ForeColor = Color.White
				btnNo.CssClass = "styled-button-2"
				btnNo.Attributes.Add("runat", "server")
				btnNo.CausesValidation = False

				Dim label1 As New Label
				label1.Width = 8

				ButtonDiv.Controls.Add(btnYes)
				ButtonDiv.Controls.Add(label1)
				ButtonDiv.Controls.Add(btnNo)

				AddHandler btnYes.Click, AddressOf btnYes_Click
				AddHandler btnNo.Click, AddressOf btnNo_Click

			Case MsgBoxStyle.RetryCancel        '5
				btnRetry = New Button
				btnRetry.Text = "Retry"
				btnRetry.ID = "btnRetry"
				btnRetry.ForeColor = Color.White
				btnRetry.CssClass = "styled-button-2"
				btnRetry.Attributes.Add("runat", "server")
				btnRetry.CausesValidation = False

				btnCancel = New Button
				btnCancel.Text = "Cancel"
				btnCancel.ID = "btnCancel"
				btnCancel.ForeColor = Color.White
				btnCancel.CssClass = "styled-button-2"
				btnCancel.Attributes.Add("runat", "server")
				btnCancel.CausesValidation = False

				Dim label1 As New Label
				label1.Width = 3

				ButtonDiv.Controls.Add(btnRetry)
				ButtonDiv.Controls.Add(label1)
				ButtonDiv.Controls.Add(btnCancel)

				AddHandler btnRetry.Click, AddressOf btnRetry_Click
				AddHandler btnCancel.Click, AddressOf btnCancel_Click

				'Added by Utkarsh on 31-Oct-2013 For Information button show(Does not post back)
			Case MsgBoxStyle.Information
				Dim btnInfoOk As Button
				btnInfoOk = New Button
				btnInfoOk.Text = "Ok"
				btnInfoOk.ForeColor = Color.White
				btnInfoOk.ID = "btnInfoOk"
				btnInfoOk.ClientIDMode = UI.ClientIDMode.Static
				btnInfoOk.CssClass = "styled-button-2"
				btnInfoOk.CausesValidation = False

				ButtonDiv.Controls.Add(btnInfoOk)

				'End
		End Select

		upnlMessageBox.Update()
	End Sub
	'Public Sub showNotification(ByVal MessageTitle As Message_title, ByVal MessageText As Message_text, ByVal ExtraMessage As String, ByVal ButtonToShow As MsgBoxStyle, ByVal Sender As String)
	'    Title = MessageTitle
	'    lblMsgTitle.Text = _Title
	'    mExtraMessage = ExtraMessage
	'    Message = MessageText
	'    lblMsgText.Text = _Message

	'    'ucNotificationMsg1.showNotification(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
	'    ucNotificationMsg1.showNotification(_Title, _Message)
	'    upnlNotificationMsg.Update()
	'End Sub
	'Public Sub showNotification(ByVal MessageTitle As String, ByVal MessageText As String, ByVal ExtraMessage As String)
	'    Message = 81
	'    _Message = MessageText

	'    Title = 74
	'    _Title = MessageTitle

	'    mExtraMessage = ExtraMessage

	'    'ucNotificationMsg1.showNotification(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
	'    ucNotificationMsg1.showNotification(_Title, _Message)
	'    upnlNotificationMsg.Update()
	'End Sub
	Public Sub showbox(ByVal MessageTitle As Message_title, ByVal MessageText As Message_text, ByVal ExtraMessage As String, ByVal ButtonToShow As MsgBoxStyle, ByVal Sender As String)

		mExtraMessage = ExtraMessage

		Title = MessageTitle
		lblMsgTitle.Text = _Title

		Message = MessageText
		lblMsgText.Text = _Message

		mSender = Sender

		Session("ButtonToShow") = ButtonToShow
		Session("Sender") = mSender

		'''DisplayButtons(ButtonToShow)

		'''mdlPopupBox.Show()
		If mSender <> "" Then
			DisplayButtons(ButtonToShow)

			mdlPopupBox.Show()
		Else
			ucNotificationMsgNew.showNotification(_Title, _Message)
			upnlNotificationMsg.Update()
		End If
	End Sub
	Public Sub show(ByVal MessageTitle As Message_title, ByVal MessageText As Message_text, ByVal ExtraMessage As String, ByVal ButtonToShow As MsgBoxStyle, ByVal Sender As String)

		mExtraMessage = ExtraMessage

		Title = MessageTitle
		lblMsgTitle.Text = _Title

		Message = MessageText
		lblMsgText.Text = _Message

		mSender = Sender

		Session("ButtonToShow") = ButtonToShow
		Session("Sender") = mSender

		'''DisplayButtons(ButtonToShow)

		'''mdlPopupBox.Show()
		If mSender <> "" Then
			DisplayButtons(ButtonToShow)

			mdlPopupBox.Show()
		Else
			ucNotificationMsgNew.showNotification(_Title, _Message)
			upnlNotificationMsg.Update()
		End If
	End Sub

	Public Sub Show(ByVal MessageTitle As String, ByVal MessageText As String, ByVal ExtraMessage As String, ByVal ButtonToShow As MsgBoxStyle, ByVal Sender As String)

		mExtraMessage = ExtraMessage
		_Message = MessageText
		Message = 81


		Title = 74
		_Title = MessageTitle


		'----------------------------------------------
		lblMsgTitle.Text = _Title

		lblMsgText.Text = _Message '"<strong>" & _Message & "</strong><p>" & mExtraMessage & "</p>"

		mSender = Sender

		Session("ButtonToShow") = ButtonToShow
		Session("Sender") = mSender

		''DisplayButtons(ButtonToShow)

		''mdlPopupBox.Show()
		If mSender <> "" Then
			DisplayButtons(ButtonToShow)

			mdlPopupBox.Show()
		Else
			ucNotificationMsgNew.showNotification(_Title, _Message)
			upnlNotificationMsg.Update()
		End If
	End Sub

	Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		mResult = Result.Ok
		mSender = Session("Sender")

		mdlPopupBox.Hide()

		Session.Remove("ButtonToShow")
		Session.Remove("Sender")

		RaiseEvent UserControlButtonClicked(sender, e)
	End Sub

	Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		mResult = MsgBoxResult.Cancel
		mSender = Session("Sender")

		mdlPopupBox.Hide()

		Session.Remove("ButtonToShow")
		Session.Remove("Sender")

		RaiseEvent UserControlButtonClicked(sender, e)
	End Sub

	Protected Sub btnAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		mResult = MsgBoxResult.Abort
		mSender = Session("Sender")

		mdlPopupBox.Hide()

		Session.Remove("ButtonToShow")
		Session.Remove("Sender")

		RaiseEvent UserControlButtonClicked(sender, e)
	End Sub

	Protected Sub btnRetry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		mResult = MsgBoxResult.Retry
		mSender = Session("Sender")

		mdlPopupBox.Hide()

		Session.Remove("ButtonToShow")
		Session.Remove("Sender")

		RaiseEvent UserControlButtonClicked(sender, e)
	End Sub

	Protected Sub btnIgnore_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		mResult = MsgBoxResult.Ignore
		mSender = Session("Sender")

		mdlPopupBox.Hide()

		Session.Remove("ButtonToShow")
		Session.Remove("Sender")

		RaiseEvent UserControlButtonClicked(sender, e)
	End Sub

	Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		mResult = MsgBoxResult.Yes
		mSender = Session("Sender")

		mdlPopupBox.Hide()

		Session.Remove("ButtonToShow")
		Session.Remove("Sender")
		RaiseEvent UserControlButtonClicked(sender, e)
	End Sub

	Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
		mResult = MsgBoxResult.No
		mSender = Session("Sender")

		mdlPopupBox.Hide()

		Session.Remove("ButtonToShow")
		Session.Remove("Sender")

		RaiseEvent UserControlButtonClicked(sender, e)
	End Sub

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Session("ButtonToShow") IsNot Nothing Then
			DisplayButtons(CType(Session("ButtonToShow"), MsgBoxStyle))
		End If

	End Sub

End Class