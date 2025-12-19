'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq


Public Class MELSnagCorrectiveActionController
	Inherits ApiController



#Region "MELSnagCorrectiveActionLog"
	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELSnagCorrectiveActionLog(Optional LogID As String = "{00000000-0000-0000-0000-000000000000}") As MELSnagCorrectiveActionLog

		Try

			Return MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(LogID:=LogID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function
#End Region

#Region "MELSnagPartList"
	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELSnagPartList(CurrentDate As String,
									   Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
									   Optional AddTopItem As String = "") As MELSnagPartList

		Try

			Return MELSnagPartList.GetMELSnagPartList(CurrentDate:=CurrentDate,
													  MachineID:=MachineID,
													  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function
#End Region

#Region "IncidentTypeList"

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetIncidentTypeList(Optional AddTopItem As String = "") As IncidentTypeList

		Try

			Return IncidentTypeList.GetIncidentTypeList(AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function
#End Region

#Region "MELPartList"
	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELPartList(CurrentDate As String,
									   Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
									   Optional AddTopItem As String = "") As MELPartList

		Try

			Return MELPartList.GetMELPartList(CurrentDate:=CurrentDate,
													  MachineID:=MachineID,
													  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELPartList(ModelID As String,
										Optional Name As String = "",
										Optional Description As String = "",
										Optional AddTopItem As String = "") As MELPartList

		Try

			Return MELPartList.GetMELPartList(New Guid(ModelID), Name, Description, AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region
#Region "MELSnagCorrectiveAction"

#Region "        GetMELSnagCorrectiveAction        "
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELSnagCorrectiveAction(ByVal ID As String) As MELSnagCorrectiveAction
		Return MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(New Guid(ID))
	End Function
#End Region

#Region "        MELSnagCorrectiveAction Save        "
	<HttpPost>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function PostMELSnagCorrectiveAction(<FromBody()> ByVal value As Object) As IHttpActionResult
		Try
			Dim jsonObject As JObject = JObject.Parse(value.ToString)
			Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
			Dim returnstring As String

			returnstring = SetMELSnagCorrectiveAction(jsonObject, mIsNew)


			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "MELSnagCorrectiveAction saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring)
			'End If
			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="MELSnagCorrectiveAction Saved Successfully!"))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=returnstring))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try
	End Function
	Private Function Get_Whichever_LesserDate(LogDate As String, OccurranceDate As String) As String
		Dim mLogDate As SmartDate = New SmartDate(LogDate)
		Dim mOccurranceDate As SmartDate = New SmartDate(OccurranceDate)

		If CDate(mLogDate.ToString) < CDate(mOccurranceDate.ToString) Then
			Return mLogDate.ToString
		Else
			Return mOccurranceDate.ToString
		End If
	End Function
	Public Function SetMELSnagCorrectiveAction(jsonObject As JObject, IsNew As Boolean) As String
		Try
			Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction
			Dim mTempAssemblyList As AssemblyList
			Dim mReportLogRegister As New ReportLogRegister
			Dim MachineID As Guid
			Dim mDateFormatString As String = ""
			Dim mMELSnagPartList As MELSnagPartList
			MachineID = mReportLogRegister(New Guid(jsonObject(propertyName:="mLogID").ToString)).MachineID
			mTempAssemblyList = AssemblyList.GetAssemblyList(1, MachineID.ToString)
			mDateFormatString = jsonObject(propertyName:="mDateOfOccurence")("mFormat")

			If IsNew Then
				'mMELSnagCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(ID:=New Guid(jsonObject("mID").ToString))
				mMELSnagCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(ID:=Guid.NewGuid)
			Else
				mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=New Guid(jsonObject("mID").ToString))
			End If

			If mMELSnagCorrectiveAction IsNot Nothing And Not mMELSnagCorrectiveAction.IsNew Then

			Else

			End If

			If mMELSnagCorrectiveAction IsNot Nothing And Not mMELSnagCorrectiveAction.IsNew Then
				Dim tmpLogDetail As Log
				tmpLogDetail = Log.GetLog(mMELSnagCorrectiveAction.LogID)

				mReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(tmpLogDetail.Date.ToString, mMELSnagCorrectiveAction.DateOfOccurrence.ToString), "1/1/2100", mTempAssemblyList(0).ID.ToString, mMELSnagCorrectiveAction.MachineID.ToString, False, , 0, , , , "(SELECT)", True, , True)

				mMELSnagPartList = MELSnagPartList.GePartList(mMELSnagCorrectiveAction.DateOfOccurrence.ToString, MachineID.ToString, "(SELECT)")
			Else
				mReportLogRegister = ReportLogRegister.GetRectifiedLog(jsonObject(propertyName:="mDateOfOccurence").ToString, "1/1/2100", mTempAssemblyList(0).ID.ToString, MachineID.ToString, False, , 0, , , , "(SELECT)", True, , True)
				mMELSnagPartList = MELSnagPartList.GePartList(jsonObject(propertyName:="mDateOfOccurence").ToString, MachineID.ToString, "(SELECT)")
			End If


			'mMELSnagCorrectiveAction.Name = jsonObject(propertyName:="mName")
			With mMELSnagCorrectiveAction
				.LogID = New Guid(jsonObject(propertyName:="mLogID").ToString)
				.LogNo = mReportLogRegister(New Guid(jsonObject(propertyName:="mLogID").ToString)).LogNo

				.DateOfOccurrence = CDate(jsonObject(propertyName:="mDateOfOccurence").First.First).ToString(format:=mDateFormatString)

				.DefectReportNo = Trim(jsonObject(propertyName:="mDefectReportNo").ToString)
				.No = Val(jsonObject(propertyName:="mNo").ToString)
				.Sector = Trim(jsonObject(propertyName:="mSector").ToString)
				.LastMajorCheckHour = Trim(jsonObject(propertyName:="mLastMajorCheckHour").ToString)
				.SnagReportedBy = Trim(jsonObject(propertyName:="mSnagReportedBy").ToString)
				.ReportedBy = Trim(jsonObject(propertyName:="mReportedBy").ToString)
				.PartID = New Guid(jsonObject("mPartID").ToString)
				'If Not .PartID.Equals(Guid.Empty) Then
				'    .PartSerialNo = mMELSnagPartList(New Guid(.PartID.ToString)).SerialNo
				'Else
				.PartSerialNo = Trim(jsonObject("mPartSerialNo").ToString)
				' End If

				.Description = Trim(jsonObject(propertyName:="mDescription").ToString)
				.ComponentHour = Trim(jsonObject(propertyName:="mComponentHour").ToString)
				.Defect = Trim(Trim(jsonObject(propertyName:="mDefect").ToString))
				.CauseOfDefect = Trim(jsonObject(propertyName:="mCauseOfDefect").ToString)
				.Action = Trim(jsonObject(propertyName:="mAction").ToString)
				.ActionAgainstStaff = Trim(jsonObject(propertyName:="mActionAgainstStaff").ToString)
				.PreventionTaken = Trim(jsonObject(propertyName:="mPreventionTaken").ToString)
				.IsMEL = Trim(jsonObject(propertyName:="mIsMEL").ToString)
				.MELCategoryID = Val(jsonObject("mMELCategoryID").ToString)
				.ATAChapterID = New Guid(jsonObject("mATAChapterID").ToString)
				.IsMajor = CBool(jsonObject("mIsMajor"))
				.IsMinor = Not CBool(jsonObject("mIsMajor"))
				.InvestigationStatus = CBool(jsonObject("mInvestigationStatus"))
				.MachineID = New Guid(MachineID.ToString)
				.IsHours = CBool(jsonObject("mIsHours"))
				.FrequencyInDays = Val(jsonObject("mFrequencyInDays").ToString)
				.FrequencyInHours = Trim(jsonObject("mFrequencyInHours").ToString)
				.RectifiedStation = Trim(jsonObject("mRectifiedStation").ToString)

				.DueDate = CDate(jsonObject("mDueDate").First.First).ToString(format:=mDateFormatString)
				.RectifiedDate = CDate(jsonObject("mRectifiedDate").First.First).ToString(format:=mDateFormatString)

				.RectifiedLogID = New Guid(jsonObject("mRectifiedLogID").ToString)

				.PartNo = Trim(jsonObject(propertyName:="mPartNo").ToString)
				.IsRepetitive = CBool(jsonObject("mIsRepetitive"))
				.Remark = Trim(jsonObject(propertyName:="mRemark").ToString)
				.SubATAID = New Guid(jsonObject("mSubATAID").ToString)
				.IsPireps = CBool(jsonObject("mIsPireps"))
				.IsMaintenanceDefect = Not CBool(jsonObject("mIsPireps"))
				.IsInReliability = CBool(jsonObject("mIsInReliability"))
				.AssemblyStatusID = New Guid(jsonObject("mAssemblyStatusID").ToString)
				.ExtensionApplied = CBool(jsonObject("mExtensionApplied"))
				.ExtensionInDays = Val(jsonObject(propertyName:="mExtensionInDays").ToString)
				.ExtensionApprovalNo = Trim(jsonObject(propertyName:="mExtensionApprovalNo").ToString)
				.IncidentTypeID = Val(jsonObject(propertyName:="mIncidentTypeID").ToString)
				.IncidentTypeName = Trim(jsonObject(propertyName:="mIncidentTypeName").ToString)
				.IsAttachmentAdded = CBool(jsonObject("mIsAttachmentAdded"))

				.IsDeviationList = CBool(jsonObject("mIsDeviationList"))
				.AddToWatchList = CBool(jsonObject("mAddToWatchList"))
				.DueInHrs = Trim(jsonObject(propertyName:="mDueInHrs")("mValue").ToString)
				.DueInCycles = Trim(jsonObject(propertyName:="mDueInCycles")("mValue").ToString)
				.IsIncident = CBool(jsonObject("mIsIncident"))
				.ExtensionInHours = Val(jsonObject(propertyName:="mExtensionInHours").ToString)
				.ExtensionInCycles = Val(jsonObject(propertyName:="mExtensionInCycles").ToString)
				.IsAOG = CBool(jsonObject("mIsAOG"))
				'.IsAOG = IIf(cmbInvestigation.SelectedIndex = 3,
				'             True,
				'             False)

			End With

			Dim ItemArray As JArray = CType(jsonObject("mMaintenanceDoneByEmployees"), JArray)

			For i As Integer = 0 To ItemArray.Count - 1
				Dim mID As Guid = New Guid(ItemArray(i)("mID").ToString)
				Dim mIsNew As Boolean = CBool(ItemArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(ItemArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(ItemArray(i)("mIsDirty"))
				'  Dim mMaintenanceDoneByEmployees As MaintenanceDoneByEmployees
				Dim mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee



				Dim MaintenanceID As Guid = New Guid(ItemArray(i)("mMaintenanceID").ToString)
				Dim MaintenanceTypeID As Integer = Val(ItemArray(i)("mMaintenanceTypeID").ToString)
				Dim EmployeeID As Guid = New Guid(ItemArray(i)("mEmployeeID").ToString)
				Dim LicenseNo As String = Trim(ItemArray(i)("mLicenceNo")).ToString
				Dim ActualManHours As String = Trim(ItemArray(i)("mActualManHours").First.First).ToString
				Dim EmpName As String = Trim(ItemArray(i)("mEmployeeName")).ToString

				If IsNew Then

					mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Add(MaintenanceID:=MaintenanceID,
												 MaintenanceTypeID:=MaintenanceTypeID, EmployeeID:=EmployeeID, LicenceNo:=LicenseNo,
												 ActualManHours:=ActualManHours, EmpName:=EmpName)

					mMaintenanceDoneByEmployee = mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.CurrentItem

				Else
					mMaintenanceDoneByEmployee = mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(mID)
				End If

				If mIsDeleted Then
					mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Remove(mMaintenanceDoneByEmployee)
				End If

				If mIsNew Or mIsDirty Then
					With mMaintenanceDoneByEmployee
						.MaintenanceID = MaintenanceID
						.MaintenanceTypeID = MaintenanceTypeID
						.EmployeeID = EmployeeID
						.LicenceNo = LicenseNo
						.RequiredManHours = ActualManHours
						.EmployeeName = EmpName
					End With
				End If
			Next

			If mMELSnagCorrectiveAction.IsValid Then
				mMELSnagCorrectiveAction.Save()
			Else
				Dim str As String = ""

				For i As Integer = 0 To mMELSnagCorrectiveAction.GetBrokenRulesCollection.Count - 1
					str = str + mMELSnagCorrectiveAction.GetBrokenRulesCollection(i).Description
				Next
				'MaintenanceDoneByEmployees
				For i As Integer = 0 To mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count - 1
					If Not mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(i).IsValid Then
						Dim x As Integer
						For x = 0 To mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(i).GetBrokenRulesCollection.Count - 1
							str = str + mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Item(i).GetBrokenRulesCollection(x).Description
						Next
					End If
				Next
				Return str
			End If



			Dim mModuleList As ModuleList = ModuleList.GetModuleList("MELSnagCorrectiveAction")

			If mModuleList.Item("MELSnagCorrectiveAction").MailsRequire = True Then

				If User.Identity.Name.ToUpper = "BTPLADMIN" Then

				Else

					SendMailFile.SendMailFile(UserName:=User.Identity.Name,
											  Subject:="MELSnagCorrectiveAction successfully saved from the new UI. Client " + AppSettings("ClientCode"),
											  Info:=mMELSnagCorrectiveAction.DefectNo + " Dated : " + mMELSnagCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " + mMELSnagCorrectiveAction.LogNo + " User Name:- " + User.Identity.Name,
											  ToMailID:="support@bytzsoft.com")

				End If

			End If



			Return "Success"

		Catch ex As SqlException
			Return ex.Message
		End Try
	End Function
#End Region


#End Region

#Region "MELSnagCorrectiveActionList"
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELSnagCorrectiveActionList(Optional ByVal FromDate As String = "1-1-1900",
	Optional ByVal ToDate As String = "1-1-3300",
	Optional ByVal MachineID As String = "{00000000-0000-0000-0000-000000000000}",
	Optional ByVal InvestigationStatus As Integer = 0,
	Optional ByVal TimeFormat As String = "",
	Optional ByVal ATACode As Integer = 0,
	Optional ByVal ATANomenclature As String = "",
	Optional ByVal IsMELPartsRequired As Boolean = False) As MELSnagCorrectiveActionList
		Return MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(FromDate, ToDate, MachineID, InvestigationStatus, TimeFormat, ATACode, ATANomenclature, IsMELPartsRequired)
	End Function
#End Region

#Region "MELSnagCorrectiveActionListForDue"
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELSnagCorrectiveActionListForDue(ByVal AsonDate As String,
														 ByVal MachineID As Guid,
														 ByVal ATAID As Guid,
														 ByVal MELCategoryID As Integer,
														 ByVal IsMajor As Integer,
														 Optional ByVal TimeFormat As String = "",
														 Optional ByVal IsPireps As Integer = 0,
														 Optional ByVal SkipIsForInventoryAircarft As Boolean = False,
														 Optional ByVal DueDaysLimit As Decimal = 0) As MELSnagCorrectiveActionListForDue
		Return MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsonDate, MachineID, ATAID, MELCategoryID, IsMajor, TimeFormat, IsPireps, SkipIsForInventoryAircarft, DueDaysLimit)
	End Function
#End Region

#Region "MELSnagCorrectiveActionListNew"
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELSnagCorrectiveActionListNew(Optional FromDate As String = "1-1-1900",
															 Optional ToDate As String = "1-1-3300",
															 Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
															 Optional InvestigationStatus As Integer = 0,
															 Optional TimeFormat As String = "",
															 Optional ATACode As Integer = 0,
															 Optional ATANomenclature As String = "",
															 Optional MELSnag As Integer = 0,
															 Optional AssemblyStatusID As String = "{00000000-0000-0000-0000-000000000000}",
															 Optional ExtensionApplied As Integer = 0,
															 Optional IsInReliability As Integer = 0,
															 Optional DefectType As Integer = 0,
															 Optional IncidentTypeID As Integer = -1,
															 Optional LogID As String = "{00000000-0000-0000-0000-000000000000}",
															 Optional AddedToWatchList As Integer = 0) As MELSnagCorrectiveActionListNew
		Return MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate,
														   ToDate,
														   MachineID,
														   InvestigationStatus,
														   TimeFormat,
														   ATACode,
														   ATANomenclature,
														   MELSnag,
														   AssemblyStatusID,
														   ExtensionApplied,
														   IsInReliability,
														   DefectType,
														   IncidentTypeID:=IncidentTypeID,
														   LogID:=LogID,
														   AddedToWatchList:=AddedToWatchList)
	End Function
#End Region

#Region "PirepsMELMaintenanceDefectCount"
	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetPirepsMELMaintenanceDefectCount(Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
													   Optional FromDate As String = "1/1/1900",
													   Optional ToDate As String = "1/1/4400") As PirepsMELMaintenanceDefectCount
		Try
			Return PirepsMELMaintenanceDefectCount.GetPirepsMELMaintenanceDefectCount(MachineID, FromDate, ToDate)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function
#End Region

#Region "PirepsMELMonthlyCountGraphicalList"
	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetPirepsMELCount(ByVal Year As Integer,
									  Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
									  Optional ActivityName As String = "") As PirepsMELMonthlyCountGraphicalList
		Try
			Return PirepsMELMonthlyCountGraphicalList.GetPirepsMELCount(Year, MachineID, ActivityName)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function
#End Region

#Region "MELSnagCountATAWise"
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELSnagCountATAWise(Optional ByVal ATAChapterID As String = "{00000000-0000-0000-0000-000000000000}",
													Optional ByVal MachineID As String = "{00000000-0000-0000-0000-000000000000}",
													Optional ByVal MELSnagCorrectiveActionID As String = "{00000000-0000-0000-0000-000000000000}",
													Optional ByVal MELLastInDays As Integer = 0,
													Optional OccuranceDate As String = "1/1/2032",
													Optional MELCheckON As Integer = 0) As MELSnagCountATAWise
		Return MELSnagCountATAWise.GetMELSnagCountATAWise(ATAChapterID, MachineID, MELSnagCorrectiveActionID, MELLastInDays, OccuranceDate, MELCheckON)
	End Function
#End Region

#Region "rptMELSnagCorrectiveAction"
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetrptMELSnagCorrectiveAction(ByVal ID As String) As rptMELSnagCorrectiveAction
		Return rptMELSnagCorrectiveAction.GetrptMELSnagCorrectiveAction(ID)
	End Function
#End Region

#Region " Return Message Class "

	Public Class ReturnMessage

		Public Status As String
		Public Message As String
		Public ReportData As Byte()
		Public EventLogID As Guid

		Public Sub New(Status As String,
				   Message As String,
				   Optional ReportData As Byte() = Nothing,
				   Optional EventLogID As String = "{00000000-0000-0000-0000-000000000000}")

			Me.Status = Status
			Me.Message = Message
			Me.ReportData = ReportData
			Me.EventLogID = New Guid(EventLogID)

		End Sub

	End Class

#End Region

#Region " Post Method(s) "

	<HttpPost>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub PostValue(<FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub DeleteValue(id As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region



End Class