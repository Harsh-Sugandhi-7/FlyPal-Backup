'************************************
'Created BY: Saylee
'Modified by Harsh Sugandhi on 6th May 2025 for FLYPAL-2360 API for LogParameterList Grid View.
'************************************


Imports System.Net
Imports System.Web.Http

Imports Flypal.UpdateFuelsOfAllAboveLogs

Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


Public Class LogController
	Inherits ApiController

#Region " Variable(s) "

	Dim settings As New JsonSerializerSettings()
	Dim IsFlightTimeGreaterThanAvgFlightTime As Boolean = False
	Dim TakeOffTouchDown As Boolean = CType(AppSettings("TakeOffTouchDown"), Boolean)

	Private mCompanyDetail As New CompanyDetail
	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Log Date For Last Hrs landing "

	Public Function GetLogDateForLastHrsLandings(LogDate As String,
												 PeriodID As Integer,
												 values As Decimal) As LogDateForLastHrsLdgs
		Try

			Return LogDateForLastHrsLdgs.GetLogDateForLastHrsLdgs(LogDate,
																  PeriodID,
																  values)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " LOG LIST "

	<HttpGet>
	Public Function GetLogList(MachineID As Guid,
							   Optional SouLocalDateTime As String = "",
							   Optional DesLocalDateTime As String = "",
							   Optional Show_100_Records As Boolean = False,
							   Optional LogPageNo As String = "",
							   Optional SkipMaintLogAndVoidLog As Boolean = False) As LogList

		Try

			Dim _LogList As LogList = LogList.GetLogList(MachineID:=MachineID,
														 SouLocalDateTime:=SouLocalDateTime,
														 DesLocalDateTime:=DesLocalDateTime,
														 Show_100_Records:=Show_100_Records,
														 LogPageNo:=LogPageNo,
														 SkipMaintLogAndVoidLog:=SkipMaintLogAndVoidLog)


			Return _LogList

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetLogList(MachineID As Guid,
							   [Date] As String,
							   UsedForInstallRemove As Boolean) As LogList
		Try

			Return LogList.GetLogList(MachineID:=MachineID,
									  [Date]:=[Date],
									  UsedForInstallRemove:=UsedForInstallRemove)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetLogList(AssemblyStatusID As Guid,
							   [Date] As String,
							   UsedForCompInstallRemove As Boolean,
							   str As String) As LogList
		Try

			Return LogList.GetLogList(AssemblyStatusID:=AssemblyStatusID,
									  [Date]:=[Date],
									  UsedForCompInstallRemove:=UsedForCompInstallRemove,
									  str:=str)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Log Type List "

	<HttpGet>
	Public Function GetLogTypeList(Optional Name As String = "",
								   Optional AddTopItem As String = "") As LogTypeList

		Try

			Return LogTypeList.GetLogTypeList(Name:=Name,
											  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Update Fuels Of All Above Logs "

	<HttpGet>
	Public Function GetUpdateFuelsOfAllAboveLogs(Optional LogID As String = "",
												 Optional MachineID As String = "") As UpdateFuelsOfAllAboveLogs

		Try

			Return UpdateFuelsOfAllAboveLogs.GetLogFuelAndOilList(LogID:=New Guid(LogID),
																  MachineID:=New Guid(MachineID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function UpdateLogFuels(Optional LogFuelID As String = "",
								   Optional FuelOnArrival As Decimal = 0,
								   Optional mUpdateFuelsOfAllAboveLogsInfo As UpdateFuelsOfAllAboveLogsInfo = Nothing) As String


		Dim mUpdateFuelsOfAllAboveLogs As UpdateFuelsOfAllAboveLogs
		Try

			mUpdateFuelsOfAllAboveLogs.UpdateLogFuels(LogFuelID:=New Guid(LogFuelID),
													  FuelOnArrival:=FuelOnArrival,
													  mUpdateFuelsOfAllAboveLogsInfo:=mUpdateFuelsOfAllAboveLogsInfo)
			Return ""

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Max Log-No "

	<HttpGet>
	Public Function GetUpperLog(Optional LogID As String = "",
								Optional MachineID As String = "") As MaxLogNo

		Try

			Return MaxLogNo.GetUpperLog(LogID:=New Guid(LogID),
										MachineID:=New Guid(MachineID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Fuel Type List "

#Region " GET / NEW Methods  "

	<HttpGet>
	Public Function GetFuelTypeList(Optional Name As String = "",
									Optional AddTopItem As String = "") As FuelTypeList

		Try

			Return FuelTypeList.GetFuelTypeList(Name:=Name,
												AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetFuelType(ID As String) As FuelType

		Try

			Return FuelType.GetFuelType(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewFuelType() As FuelType

		Try

			Return FuelType.NewFuelType(Guid.NewGuid)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Method(s) "

	Public Function SetFuelType(JObject As JObject,
								IsNew As Boolean) As String

		Try

			Dim mFuelType As FuelType

			If IsNew Then
				mFuelType = FuelType.NewFuelType(ID:=Guid.NewGuid)
			Else
				mFuelType = FuelType.GetFuelType(ID:=New Guid(JObject("mID").ToString))
			End If

			mFuelType.Name = JObject(propertyName:="mName")

			mFuelType.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="FuelType",
																						   ex:=ex)

			Return returnMessage

		End Try

	End Function

#End Region

#Region " Save Method "

	<HttpPost>
	Public Function PostFuelType(<FromBody()> requestBody As JObject) As IHttpActionResult

		Try

			Dim mIsNew As Boolean = CBool(requestBody("mIsNew"))
			Dim returnString As String

			returnString = SetFuelType(requestBody, mIsNew)

			If returnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Fuel Type Saved Successfully!"))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=returnString))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

#End Region

#Region " Delete Method "

	<HttpDelete>
	Public Function DeleteFuelType(ID As Guid) As IHttpActionResult

		Try

			FuelType.DeleteFuelType(ID:=ID)

			Return Ok(New ReturnMessage("Success", "Fuel Type Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="FuelType",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))


		End Try

	End Function

#End Region

#End Region

#Region " LogFuelList "

	<HttpGet>
	Public Function GetLogFuelList(Optional LogID As String = "") As LogFuelList

		Try

			Return LogFuelList.GetLogFuelList(LogID:=New Guid(LogID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Get MaxLog Of Aircraft "

	<HttpGet>
	Public Function GetMaxLogOfAircraft(Optional MachineID As String = "") As MaxLogOfAircraft

		Try

			Return MaxLogOfAircraft.GetMaxLogOfAircraft(MachineID:=New Guid(MachineID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " CRS Log Transfer "

	' *****************  CRSLogTransfer List ******************************************************

	Public Function ImportCRSLogs() As ReturnMessage

		Dim mLogPageNo As String = ""
		Dim mCRSLogs As CRSLogTransfer
		Dim mLog As Log
		Dim mError As Boolean = False
		Dim mMachine As Machine

		mCRSLogs = CRSLogTransfer.GetLogList()

		If mCRSLogs.Count > 0 Then

			Try

				For i As Integer = 0 To mCRSLogs.Count - 1

					mMachine = Machine.GetMachine(mCRSLogs(i).MachineID, False)
					If mMachine.IsReadOnly Then

						FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs", OpenMode.Append, OpenAccess.ReadWrite)
						WriteLine(1, mMachine.RegNo + " is ReadOnly aircraft, so log " + mCRSLogs(i).LogPageNo + " cannot be transferred into system. " + vbCrLf)
						FileClose(1)
						mError = True
						SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")
						MarkLog(Action.Save, "Flight Log", "Log(s) failed for importing " + mCRSLogs(i).LogPageNo + " : " + mMachine.RegNo + " is ReadOnly aircraft, so cannot be transferred into system. ", ErrorType.UnhandledError, Guid.Empty, EventLogID)

						GoTo 2

					ElseIf mMachine.NotInUse Then

						FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs", OpenMode.Append, OpenAccess.ReadWrite)
						WriteLine(1, mMachine.RegNo + " is Not In Use since " + mMachine.NotInUseDateFormatted + ", so log " + mCRSLogs(i).LogPageNo + " cannot be transferred into system. " + vbCrLf)
						FileClose(1)
						mError = True
						SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")
						MarkLog(Action.Save, "Flight Log", "Log(s) failed for importing " + mCRSLogs(i).LogPageNo + " : " + mMachine.RegNo + " is Not In Use since " + mMachine.NotInUseDateFormatted + ", so log " + mCRSLogs(i).LogPageNo + " cannot be transferred into system.", ErrorType.UnhandledError, Guid.Empty, EventLogID)

						GoTo 2

					End If

					Dim dtString As DateTime = CType(mCRSLogs(i).DateFormatted.ToString.Trim + " " + "23:59", DateTime)
					mLog = Log.NewCRSLog(Guid.NewGuid, mMachine, mCRSLogs(i).DateFormatted, "", dtString.ToString, 1)
					mLog.IsSyncFromCRS = True
					mLog.IsUTC = True
					mLog.IsTakeoffTouchDown = CType(AppSettings("TakeOffTouchDown"), Boolean)
					mLog.LogPageNo = mCRSLogs(i).LogPageNo
					mLog.FlightNo = mCRSLogs(i).FlightNo

					If Not mCRSLogs(i).PICID.Equals(Guid.Empty) Then
						mLog.PilotID1 = mCRSLogs(i).PICID
					Else
						mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
					End If

					mLog.PilotID2 = mCRSLogs(i).SICID
					mLog.FlightLogClassificationID = mCRSLogs(i).FlightLogClassificationID
					mLog.SourceID = mCRSLogs(i).FromPlaceID
					mLog.DestinationID = mCRSLogs(i).ToPlaceID
					mLog.SouUniverseDateTime = mCRSLogs(i).UTCChocksOffDateTimeFormatted
					mLog.TakeOffUniverseDateTime = mCRSLogs(i).UTCTakeOffDateTimeFormatted
					mLog.TouchDownUniverseDateTime = mCRSLogs(i).UTCTouchDownDateTimeFormatted
					mLog.DesUniverseDateTime = mCRSLogs(i).UTCChocksOnDateTimeFormatted

					mLog.Remark = mCRSLogs(i).Remark
					For j As Integer = 0 To mLog.LogAFAssemblies.Count - 1

						If mLog.LogAFAssemblies(j).LogPeriods.Contains(3) Then mLog.LogAFAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						If mLog.LogAFAssemblies(j).LogPeriods.Contains(7) Then mLog.LogAFAssemblies(j).Landings = mCRSLogs(i).Landings.ToString

					Next

					For j As Integer = 0 To mLog.LogAPUAssemblies.Count - 1

						If mLog.LogAPUAssemblies(j).LogPeriods.Contains(3) Then
							mLog.LogAPUAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						End If

					Next

					For j As Integer = 0 To mLog.LogCGBAssemblies.Count - 1

						If mLog.LogCGBAssemblies(j).LogPeriods.Contains(3) Then
							mLog.LogCGBAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						End If

					Next

					For j As Integer = 0 To mLog.LogEngAssemblies.Count - 1

						If mLog.LogEngAssemblies(j).LogPeriods.Contains(3) Then
							mLog.LogEngAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						End If

					Next

					If mLog.LogAFAssemblies.ShowCycles Then mLog.UpdateChildPeriods(3, "Cycles", mCRSLogs(i).Cycles.ToString)
					If mLog.LogAFAssemblies.ShowLandings Then mLog.UpdateChildPeriods(7, "Landings", mCRSLogs(i).Landings.ToString)

					'LogDetail Child (TLP child)
					If mMachine.IsTLP Then
						mLog = SETLogDetail(mCRSLogs(i), mLog)

					End If

					If mLog.IsValid Then

						mLog.CRSLogTransferID = mCRSLogs(i).ID
						mLog.Save()

						'Discrepancies

						If mCRSLogs(i).DiscrepancyCount > 0 Then

							Dim mCRSLogTransferDiscrepancies As CRSLogTransferDiscrepancies = CRSLogTransferDiscrepancies.GetLogDiscrepancyList(mCRSLogs(i).ID)

							If mCRSLogTransferDiscrepancies.Count > 0 Then

								For m As Integer = 0 To mCRSLogTransferDiscrepancies.Count - 1

									Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction
									Dim mAssemblylist As AssemblyList = AssemblyList.GetAssemblyListForComboBox(0, mMachine.ID.ToString, mLog.DateFormatted.ToString, "", True)

									mMELSnagCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(mAssemblylist(0).AssemblyStatusID.ToString)

									mMELSnagCorrectiveAction.Defect = mCRSLogTransferDiscrepancies(m).Discrepancy
									mMELSnagCorrectiveAction.Sector = mLog.SourceName

									If Not mCRSLogTransferDiscrepancies(m).ReportCrewID.Equals(Guid.Empty) Then
										Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(mCRSLogTransferDiscrepancies(m).EmployeeName, User.Identity.Name, WithoutLicenseNoAlso:=1)
										mMELSnagCorrectiveAction.ReportedBy = mLicenses(0).LicenseNoEmpName
									End If

									mMELSnagCorrectiveAction.DefectReportNo = "Dscr" + "/" + mMachine.RegNo
									mMELSnagCorrectiveAction.LogID = mLog.ID
									mMELSnagCorrectiveAction.DateOfOccurrence = mLog.DateFormatted
									mMELSnagCorrectiveAction.RegNo = mLog.RegNo

									If mLog.LogAFAssemblies(0).FinalLandings = "" Or mLog.LogAFAssemblies(0).FinalLandings = "0" Then
										mMELSnagCorrectiveAction.LastMajorCheckHour = mLog.LogAFAssemblies(0).FinalHours + " H"
									Else
										mMELSnagCorrectiveAction.LastMajorCheckHour = mLog.LogAFAssemblies(0).FinalHours + " H" + ", " + mLog.LogAFAssemblies(0).FinalLandings + " L"
									End If

									If mLog.LogAFAssemblies(0).FinalCycles = "" Or mLog.LogAFAssemblies(0).FinalCycles = "0" Then
										mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveAction.LastMajorCheckHour
									Else
										mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveAction.LastMajorCheckHour + ", " + mLog.LogAFAssemblies(0).FinalCycles + " C"
									End If

									mMELSnagCorrectiveAction.IsSyncFromCRS = True

									If mMELSnagCorrectiveAction.IsValid Then
										mMELSnagCorrectiveAction.Save()
										mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mMELSnagCorrectiveAction.ID)
										SendMail("", IsForNewDiscrepancyImported:=True, ImportedDiscrepancy:=mMELSnagCorrectiveAction)
									End If

								Next

							End If

						End If
						'********************

						mLogPageNo = mCRSLogs(i).LogPageNo

					Else

						Dim str As String = ""
						str = GetBrokenRules(mLog)
						str = str.Replace("<BR>", vbCrLf)
						mError = True

						If str <> "" Then

							FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs.txt", OpenMode.Append, OpenAccess.ReadWrite)
							WriteLine(1, str + vbCrLf)
							FileClose(1)
							SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")
							MarkLog(Action.Save, "Flight Log", "Log(s) failed for importing " + mCRSLogs(i).AircraftRegNo + " (" + mCRSLogs(i).LogPageNo + ") :" + str, ErrorType.UnhandledError, Guid.Empty, EventLogID)

							GoTo 2

						End If

					End If

2:              Next

			Catch ex As Exception
				Throw ex.GetBaseException
			End Try

			If mError = True Then
				Return New ReturnMessage("Success", "Log(s) Imported Successfully with some error(s).")
			Else
				Return New ReturnMessage("Error", "Log(s) Imported Successfully")
			End If

		End If

	End Function


	<HttpGet>
	Public Function GetLogDiscrepancyList(CRSLogTransferID As Guid) As CRSLogTransferDiscrepancies

		Try

			Return CRSLogTransferDiscrepancies.GetLogDiscrepancyList(CRSLogTransferID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#Region " Methods "

	Private Function SetLogDetail(mCRSLogsInfo As CRSLogTransfer.CRSLogTransferInfo, mLog As Log) As Log

		Dim mLogDetail As LogDetail
		Try

			mLogDetail = LogDetail.NewChildLogDetail(mLog.ID, mLog.Date.ToString)

			With mLogDetail

				If mLog.IsUTC = True Then

					If Not IsDate(mCRSLogsInfo.UTCChocksOffDateTimeFormatted) Then
						.SouUniverseDateTime = System.DBNull.Value
					Else
						.SouUniverseDateTime = mCRSLogsInfo.UTCChocksOffDateTimeFormatted
					End If

					If Not IsDate(mCRSLogsInfo.UTCTakeOffDateTimeFormatted) Then
						.TakeOffUniverseDateTime = System.DBNull.Value
					Else
						.TakeOffUniverseDateTime = mCRSLogsInfo.UTCTakeOffDateTimeFormatted
					End If

					If Not IsDate(mCRSLogsInfo.UTCTouchDownDateTimeFormatted) Then
						.TouchDownUniverseDateTime = System.DBNull.Value
					Else
						.TouchDownUniverseDateTime = mCRSLogsInfo.UTCTouchDownDateTimeFormatted
					End If

					If Not IsDate(mCRSLogsInfo.UTCChocksOnDateTimeFormatted) Then
						.DesUniverseDateTime = System.DBNull.Value
					Else
						.DesUniverseDateTime = mCRSLogsInfo.UTCChocksOnDateTimeFormatted
					End If

				End If

				mLogDetail.SourceID = mLog.SourceID
				mLogDetail.DestinationID = mLog.DestinationID

				.FlightNo = mCRSLogsInfo.FlightNo.ToString.Trim
				.Landings = Val(mCRSLogsInfo.Landings.ToString.Trim)

			End With

			mLog.LogDetails.Add(mLogDetail)

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Sub SendMail(FilePath As String,
						Optional IsForNewDiscrepancyImported As Boolean = False,
						Optional ImportedDiscrepancy As MELSnagCorrectiveAction = Nothing)

		Dim str As String
		Dim mModuleList As ModuleList = ModuleList.GetModuleList("DiscrepancyAction")

		Try

			If IsForNewDiscrepancyImported = False Then

				str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Attached file has list of log(s) which failed while transferring from CRS and sent by  <b>" + User.Identity.Name + "</b>" + " in FlyPal System." + "</font></P></br> ")
				str = str + ("<p><font face=""Calibri"">")
				str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
				str = str + ("</body></html>")

				SendMailFile.SendMailFile(Nothing,
										  User.Identity.Name,
										  "List of Failed Logs from CRS",
										  "",
										  Info:=str,
										  VendorEmailID:="",
										  ToMailID:=mModuleList.Item("DiscrepancyAction").SendToMailID,
										  CCMailID:="", BCCMailID:="support@bytzsoft.com",
										  ReportPath:=FilePath,
										  SmtpHost:=mModuleList.Item("DiscrepancyAction").SmtpHost,
										  SmtpPort:=mModuleList.Item("DiscrepancyAction").SmtpPort,
										  SmtpUser:=mModuleList.Item("DiscrepancyAction").SmtpUser,
										  SmtpPassword:=mModuleList.Item("DiscrepancyAction").SmtpPassword)


			Else

				If ImportedDiscrepancy IsNot Nothing Then

					str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">New Discrepancy has been added in FlyPal System and need your attention." + "</font></P></br> ")
					str = str + "<p><font face=""Calibri"">"
					str = str + "<b> Aircraft : </b>" + ImportedDiscrepancy.RegNo + "<b>" + "  Log No : " + "</b>" + ImportedDiscrepancy.LogNo
					str = str + "</font></p>"
					str = str + "<p><font face=""Calibri"">"
					str = str + ("<b>Discrepancy No. : " + "</b>" + ImportedDiscrepancy.DefectNo + "<b>  Date of Occurrence : </b>" +
							 ImportedDiscrepancy.DateOfOccurrenceFormatted)
					str = str + "</font></p>"
					str = str + "<p><font face=""Calibri"">"
					str = str + "<b>" + " Discrepancy : " + "</b>" + ImportedDiscrepancy.Defect
					str = str + "</font></p>"
					str = str + "<p><font face=""Calibri"">"
					str = str + "<b>" + " Reported By : " + "</b>" + ImportedDiscrepancy.ReportedBy
					str = str + "</font></p>"
					str = str + "</body></html>"
					str = str + ("</br><p><font face=""Calibri"">")
					str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
					str = str + ("</body></html>")

					SendMailFile.SendMailFile(Nothing,
											  User.Identity.Name,
											  "New Discrepancy Reported",
											  "",
											  Info:=str,
											  VendorEmailID:="",
											  ToMailID:=mModuleList.Item("DiscrepancyAction").SendToMailID,
											  CCMailID:="",
											  BCCMailID:="",
											  ReportPath:=FilePath,
											  SmtpHost:=mModuleList.Item("DiscrepancyAction").SmtpHost,
											  SmtpPort:=mModuleList.Item("DiscrepancyAction").SmtpPort,
											  SmtpUser:=mModuleList.Item("DiscrepancyAction").SmtpUser,
											  SmtpPassword:=mModuleList.Item("DiscrepancyAction").SmtpPassword)

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Function GetBrokenRules(mLog As Log) As String    'For DgLog Fuel Oils

		Dim str As String = ""

		Dim LogStr As String = mLog.RegNo + " :" + mLog.LogNoLogPageNo + " "
		Try

			If Not mLog.IsValid Then

				For j As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
					str = str + LogStr + mLog.GetBrokenRulesCollection(j).Description
				Next

			End If

			'AirFrame
			For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1

				If Not mLog.LogAFAssemblies(i).IsValid Then

					Dim x As Integer

					For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description
					Next

				End If

			Next

			'Engine
			For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1

				If Not mLog.LogEngAssemblies(i).IsValid Then

					Dim x As Integer

					For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description
					Next

				End If

			Next

			'APU
			For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1

				If Not mLog.LogAPUAssemblies(i).IsValid Then

					Dim x As Integer

					For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description
					Next

				End If

			Next

			'Log Oils
			For i As Integer = 0 To mLog.LogOils.Count - 1

				If Not mLog.LogOils(i).IsValid Then

					For j As Integer = 0 To mLog.LogOils(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogOils.Item(i).GetBrokenRulesCollection(j).Description
					Next

				End If

			Next

			For i As Integer = 0 To mLog.FuelUpLifts.Count - 1

				If Not mLog.FuelUpLifts(i).IsValid Then

					For j As Integer = 0 To mLog.FuelUpLifts(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.FuelUpLifts.Item(i).GetBrokenRulesCollection(j).Description
					Next

				End If

			Next

			For i As Integer = 0 To mLog.LogFuels.Count - 1

				If Not mLog.LogFuels(i).IsValid Then

					For j As Integer = 0 To mLog.LogFuels(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogFuels.Item(i).GetBrokenRulesCollection(j).Description
					Next

				End If

			Next

			For i As Integer = 0 To mLog.LogDetails.Count - 1

				If Not mLog.LogDetails(i).IsValid Then

					For j As Integer = 0 To mLog.LogDetails(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogDetails.Item(i).GetBrokenRulesCollection(j).Description
					Next

				End If

			Next

			If str <> "" Then
				Return str.Replace("<br />", "").Replace("<BR>", "")
			End If

			Return ""

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#End Region

#Region " LOG DETAIL "

	''**************** LOG DETAIL   **********************************

	<HttpGet>
	Public Function NewLog(MachineID As Guid,
						   Optional LogDate As String = "",
						   Optional mSouLocalDateTime As String = "",
						   Optional mSouUTCDateTime As String = "",
						   Optional LogTypeID As Integer = 1) As Log

		Try

			Dim Machine As Machine = Machine.GetMachine(MachineID:=MachineID)
			Dim Log As Log = Log.NewLog(Machine:=Machine,
										LogDate:=LogDate,
										mSouLocalDateTime:=mSouLocalDateTime,
										mSouUTCDateTime:=mSouUTCDateTime,
										LogTypeID:=LogTypeID)

			Return Log

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewCRSLog(LogID As String,
							  MachineID As String,
							  Optional LogDate As String = "",
							  Optional mSouLocalDateTime As String = "",
							  Optional mSouUTCDateTime As String = "",
							  Optional LogTypeID As Integer = 1) As Log

		Try

			Dim Machine As Machine = Machine.GetMachine(MachineID:=New Guid(MachineID))

			Dim Log As Log = Log.NewCRSLog(LogID:=New Guid(LogID),
										   Machine:=Machine,
										   LogDate:=LogDate,
										   mSouLocalDateTime:=mSouLocalDateTime,
										   mSouUTCDateTime:=mSouUTCDateTime,
										   LogTypeID:=LogTypeID)

			Return Log

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetLog(ID As String,
						   Optional IsFromTroubleshooting As Boolean = False) As Log

		Try

			Return Log.GetLog(ID:=New Guid(ID),
							  IsFromTroubleshooting:=IsFromTroubleshooting)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	''**************** LOG DELETE   **********************************
	<HttpDelete>
	Public Function DeleteLog(ID As Guid,
							  MachineID As Guid,
							  SouLocalDateTime As String,
							  DesLocalDateTime As String) As IHttpActionResult

		Try

			Log.DeleteLog(ID:=ID,
						  MachineID:=MachineID,
						  SouLocalDateTime:=SouLocalDateTime,
						  DesLocalDateTime:=DesLocalDateTime)

			Return Ok(New ReturnMessage("Success", "Log Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Log",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function


	<HttpPost>
	Public Function CalculateTime(<FromBody()> requestBody As JObject) As Log

		Dim IsNew As Boolean = CBool(requestBody("mIsNew"))
		Dim _Log As Log

		Try

			_Log = CType(SetLog(jsonObject:=requestBody,
								IsNew:=IsNew,
								IsForCalculation:=True), Log)
			Return _Log

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Log Assembly Methods "

	Public Function SetAFAssemblyObject(AFLogAssembliesArray As JArray, mLog As Log) As Log

		Try

			For i As Integer = 0 To AFLogAssembliesArray.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogAFAssemblies(i).ID,
									  New Guid(AFLogAssembliesArray(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(AFLogAssembliesArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(AFLogAssembliesArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(AFLogAssembliesArray(i)("mIsDirty"))

				If mIsNew Or mIsDirty Then

					Dim mLogPeriods As JArray = CType(AFLogAssembliesArray(i)("mLogPeriods"), JArray)
					For j As Integer = 0 To mLogPeriods.Count - 1

						Dim mPeriodID As Integer = CType(((mLogPeriods(j)("mPeriodID")).Parent.First).ToString, Integer)

						Select Case mPeriodID
							Case 1 'Hours
								If mLog.LogAFAssemblies.ShowHours Then mLog.LogAFAssemblies.Item(i).Hours = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 3 'Cycles
								If mLog.LogAFAssemblies.ShowCycles Then mLog.LogAFAssemblies.Item(i).Cycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 4 'Ng Cycles
								If mLog.LogAFAssemblies.ShowNGCycles Then mLog.LogAFAssemblies.Item(i).NGCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 5 'Nf Cycles
								If mLog.LogAFAssemblies.ShowNFCycles Then mLog.LogAFAssemblies.Item(i).NFCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 6 'RINS
								If mLog.LogAFAssemblies.ShowRINS Then mLog.LogAFAssemblies.Item(i).RINS = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 7 'Landings
								If mLog.LogAFAssemblies.ShowLandings Then mLog.LogAFAssemblies.Item(i).Landings = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 8 'Starts
								If mLog.LogAFAssemblies.ShowStarts Then mLog.LogAFAssemblies.Item(i).Starts = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 11 'Bleeds
								If mLog.LogAFAssemblies.ShowBleeds Then mLog.LogAFAssemblies.Item(i).Bleeds = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 12 'Impeller Cycles
								If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.LogAFAssemblies.Item(i).ImpellerCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 13 'CT Cycles
								If mLog.LogAFAssemblies.ShowCTCycles Then mLog.LogAFAssemblies.Item(i).CTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 14 'PT Cycles
								If mLog.LogAFAssemblies.ShowPTCycles Then mLog.LogAFAssemblies.Item(i).PTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 15 'Generator Mods
								If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.LogAFAssemblies.Item(i).GeneratorMods = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
						End Select

					Next

				End If

				If mLog.LogAFAssemblies.ShowCycles Then mLog.UpdateChildPeriods(3, "Cycles", mLog.LogAFAssemblies(i).Cycles)
				If mLog.LogAFAssemblies.ShowNGCycles Then mLog.UpdateChildPeriods(4, "NgCycles", mLog.LogAFAssemblies(i).NGCycles)
				If mLog.LogAFAssemblies.ShowNFCycles Then mLog.UpdateChildPeriods(5, "NfCycles", mLog.LogAFAssemblies(i).NFCycles)
				If mLog.LogAFAssemblies.ShowRINS Then mLog.UpdateChildPeriods(6, "RINS", mLog.LogAFAssemblies(i).RINS)
				If mLog.LogAFAssemblies.ShowLandings Then mLog.UpdateChildPeriods(7, "Landings", mLog.LogAFAssemblies(i).Landings)
				If mLog.LogAFAssemblies.ShowStarts Then mLog.UpdateChildPeriods(8, "Starts", mLog.LogAFAssemblies(i).Starts)
				If mLog.LogAFAssemblies.ShowBleeds Then mLog.UpdateChildPeriods(11, "Bleeds", mLog.LogAFAssemblies(i).Bleeds)
				If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.UpdateChildPeriods(12, "ImpellerCycles", mLog.LogAFAssemblies(i).ImpellerCycles)
				If mLog.LogAFAssemblies.ShowCTCycles Then mLog.UpdateChildPeriods(13, "CTCycles", mLog.LogAFAssemblies(i).CTCycles)
				If mLog.LogAFAssemblies.ShowPTCycles Then mLog.UpdateChildPeriods(14, "PTCycles", mLog.LogAFAssemblies(i).PTCycles)
				If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.UpdateChildPeriods(15, "GeneratorMods", mLog.LogAFAssemblies(i).GeneratorMods)

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetEngAssemblyObject(EngLogAssembliesArray As JArray, mLog As Log) As Log

		Try

			For i As Integer = 0 To EngLogAssembliesArray.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogEngAssemblies(i).ID,
									  New Guid(EngLogAssembliesArray(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(EngLogAssembliesArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(EngLogAssembliesArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(EngLogAssembliesArray(i)("mIsDirty"))

				If mIsNew Or mIsDirty Then

					Dim mLogPeriods As JArray = CType(EngLogAssembliesArray(i)("mLogPeriods"), JArray)
					For j As Integer = 0 To mLogPeriods.Count - 1

						Dim mPeriodID As Integer = CType(((mLogPeriods(j)("mPeriodID")).Parent.First).ToString, Integer)

						Select Case mPeriodID
							Case 1 'Hours
								If mLog.LogEngAssemblies.ShowHours Then mLog.LogEngAssemblies.Item(i).Hours = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 3 'Cycles
								If mLog.LogEngAssemblies.ShowCycles Then mLog.LogEngAssemblies.Item(i).Cycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 4 'Ng Cycles
								If mLog.LogEngAssemblies.ShowNGCycles Then mLog.LogEngAssemblies.Item(i).NGCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 5 'Nf Cycles
								If mLog.LogEngAssemblies.ShowNFCycles Then mLog.LogEngAssemblies.Item(i).NFCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 6 'RINS
								If mLog.LogEngAssemblies.ShowRINS Then mLog.LogEngAssemblies.Item(i).RINS = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 7 'Landings
								If mLog.LogEngAssemblies.ShowLandings Then mLog.LogEngAssemblies.Item(i).Landings = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 8 'Starts
								If mLog.LogEngAssemblies.ShowStarts Then mLog.LogEngAssemblies.Item(i).Starts = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 11 'Bleeds
								If mLog.LogEngAssemblies.ShowBleeds Then mLog.LogEngAssemblies.Item(i).Bleeds = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 12 'Impeller Cycles
								If mLog.LogEngAssemblies.ShowImpellerCycles Then mLog.LogEngAssemblies.Item(i).ImpellerCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 13 'CT Cycles
								If mLog.LogEngAssemblies.ShowCTCycles Then mLog.LogEngAssemblies.Item(i).CTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 14 'PT Cycles
								If mLog.LogEngAssemblies.ShowPTCycles Then mLog.LogEngAssemblies.Item(i).PTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 15 'Generator Mods
								If mLog.LogEngAssemblies.ShowGeneratorMods Then mLog.LogEngAssemblies.Item(i).GeneratorMods = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 16 'Rapid Take Off
								If mLog.LogEngAssemblies.ShowRapidTakeOffFactors Then mLog.LogEngAssemblies.Item(i).RapidTakeOffFactor = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
						End Select

					Next

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetAPUAssemblyObject(APULogAssembliesArray As JArray, mLog As Log) As Log

		Try

			For i As Integer = 0 To APULogAssembliesArray.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogAPUAssemblies(i).ID,
									  New Guid(APULogAssembliesArray(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(APULogAssembliesArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(APULogAssembliesArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(APULogAssembliesArray(i)("mIsDirty"))

				If mIsNew Or mIsDirty Then

					Dim mLogPeriods As JArray = CType(APULogAssembliesArray(i)("mLogPeriods"), JArray)
					For j As Integer = 0 To mLogPeriods.Count - 1

						Dim mPeriodID As Integer = CType(((mLogPeriods(j)("mPeriodID")).Parent.First).ToString, Integer)

						Select Case mPeriodID
							Case 1 'Hours
								If mLog.LogAPUAssemblies.ShowHours Then mLog.LogAPUAssemblies.Item(i).Hours = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 3 'Cycles
								If mLog.LogAPUAssemblies.ShowCycles Then mLog.LogAPUAssemblies.Item(i).Cycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 4 'Ng Cycles
								If mLog.LogAPUAssemblies.ShowNGCycles Then mLog.LogAPUAssemblies.Item(i).NGCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 5 'Nf Cycles
								If mLog.LogAPUAssemblies.ShowNFCycles Then mLog.LogAPUAssemblies.Item(i).NFCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 6 'RINS
								If mLog.LogAPUAssemblies.ShowRINS Then mLog.LogAPUAssemblies.Item(i).RINS = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 7 'Landings
								If mLog.LogAPUAssemblies.ShowLandings Then mLog.LogAPUAssemblies.Item(i).Landings = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 8 'Starts
								If mLog.LogAPUAssemblies.ShowStarts Then mLog.LogAPUAssemblies.Item(i).Starts = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 11 'Bleeds
								If mLog.LogAPUAssemblies.ShowBleeds Then mLog.LogAPUAssemblies.Item(i).Bleeds = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 12 'Impeller Cycles
								If mLog.LogAPUAssemblies.ShowImpellerCycles Then mLog.LogAPUAssemblies.Item(i).ImpellerCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 13 'CT Cycles
								If mLog.LogAPUAssemblies.ShowCTCycles Then mLog.LogAPUAssemblies.Item(i).CTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 14 'PT Cycles
								If mLog.LogAPUAssemblies.ShowPTCycles Then mLog.LogAPUAssemblies.Item(i).PTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 15 'Generator Mods
								If mLog.LogAPUAssemblies.ShowGeneratorMods Then mLog.LogAPUAssemblies.Item(i).GeneratorMods = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First

						End Select

					Next

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetCGBAssemblyObject(CGBLogAssembliesArray As JArray, mLog As Log) As Log

		Try

			For i As Integer = 0 To CGBLogAssembliesArray.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogCGBAssemblies(i).ID,
									  New Guid(CGBLogAssembliesArray(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(CGBLogAssembliesArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(CGBLogAssembliesArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(CGBLogAssembliesArray(i)("mIsDirty"))

				If mIsNew Or mIsDirty Then

					Dim mLogPeriods As JArray = CType(CGBLogAssembliesArray(i)("mLogPeriods"), JArray)
					For j As Integer = 0 To mLogPeriods.Count - 1

						Dim mPeriodID As Integer = CType(((mLogPeriods(j)("mPeriodID")).Parent.First).ToString, Integer)

						Select Case mPeriodID
							Case 1 'Hours
								If mLog.LogCGBAssemblies.ShowHours Then mLog.LogCGBAssemblies.Item(i).Hours = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 3 'Cycles
								If mLog.LogCGBAssemblies.ShowCycles Then mLog.LogCGBAssemblies.Item(i).Cycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 4 'Ng Cycles
								If mLog.LogCGBAssemblies.ShowNGCycles Then mLog.LogCGBAssemblies.Item(i).NGCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 5 'Nf Cycles
								If mLog.LogCGBAssemblies.ShowNFCycles Then mLog.LogCGBAssemblies.Item(i).NFCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 6 'RINS
								If mLog.LogCGBAssemblies.ShowRINS Then mLog.LogCGBAssemblies.Item(i).RINS = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 7 'Landings
								If mLog.LogCGBAssemblies.ShowLandings Then mLog.LogCGBAssemblies.Item(i).Landings = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 8 'Starts
								If mLog.LogCGBAssemblies.ShowStarts Then mLog.LogCGBAssemblies.Item(i).Starts = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 11 'Bleeds
								If mLog.LogCGBAssemblies.ShowBleeds Then mLog.LogCGBAssemblies.Item(i).Bleeds = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 12 'Impeller Cycles
								If mLog.LogCGBAssemblies.ShowImpellerCycles Then mLog.LogCGBAssemblies.Item(i).ImpellerCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 13 'CT Cycles
								If mLog.LogCGBAssemblies.ShowCTCycles Then mLog.LogCGBAssemblies.Item(i).CTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 14 'PT Cycles
								If mLog.LogCGBAssemblies.ShowPTCycles Then mLog.LogCGBAssemblies.Item(i).PTCycles = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 15 'Generator Mods
								If mLog.LogCGBAssemblies.ShowGeneratorMods Then mLog.LogCGBAssemblies.Item(i).GeneratorMods = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
							Case 16 'Rapid Take Off
								If mLog.LogCGBAssemblies.ShowRapidTakeOffFactors Then mLog.LogCGBAssemblies.Item(i).RapidTakeOffFactor = (mLogPeriods(j)("mDifferenceValue")("mValue")).Parent.First
						End Select

					Next

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetLogFuelObject(LogFuelsArray As JArray, mLog As Log) As Log

		Try

			For i As Integer = 0 To LogFuelsArray.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogFuels(i).ID,
									  New Guid(LogFuelsArray(i)("mID").ToString))
				Dim mIsNew As Boolean = CBool(LogFuelsArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(LogFuelsArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(LogFuelsArray(i)("mIsDirty"))

				Dim mLogFuel As LogFuel

				'If mIsNew Then

				'    mLog.LogFuels.Add(LogID:=mLog.ID,
				'                      MachineTankID:=New Guid(Trim(LogFuelsArray(i)("mMachineTankID").ToString)),
				'                      TankName:=Trim(LogFuelsArray(i)("mTankName").ToString),
				'                      UnitID:=Val(Trim(LogFuelsArray(i)("mUnitID").ToString)))

				'    mLogFuel = mLog.LogFuels.CurrentItem

				'Else
				'    mLogFuel = mLog.LogFuels(mID)
				'End If

				mLogFuel = mLog.LogFuels(mID)

				If mIsNew Or mIsDirty Then

					With mLogFuel

						.FuelUplifted = Val(Trim(LogFuelsArray(i)("mFuelUplifted").ToString))
						.FuelOnArrival = Val(Trim(LogFuelsArray(i)("mFuelOnArrival").ToString))
						.WOFuelUplifted = Val(LogFuelsArray(i)("mWOFuelUplifted").ToString)
						.WOFuelDrainedOut = Val(LogFuelsArray(i)("mWOFuelDrainedOut").ToString)
						.BurnOnGround = Val(LogFuelsArray(i)("mBurnOnGround").ToString)

					End With

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetFuelUpLiftsObject(LogFuelUpLiftsArray As JArray, mLog As Log) As Log

		Try

			For i As Integer = 0 To LogFuelUpLiftsArray.Count - 1

				Dim mFuelUpLift As FuelUpLift
				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.FuelUpLifts(i).ID,
									  New Guid(LogFuelUpLiftsArray(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(LogFuelUpLiftsArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(LogFuelUpLiftsArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(LogFuelUpLiftsArray(i)("mIsDirty"))

				If mIsNew Then
					mLog.FuelUpLifts.Add(mLog.ID, CInt(Trim(LogFuelUpLiftsArray(i)("mUnitID").ToString)))
					mFuelUpLift = mLog.FuelUpLifts.CurrentItem
				Else
					mFuelUpLift = mLog.FuelUpLifts(mID)
				End If

				If mIsNew Or mIsDirty Then

					With mFuelUpLift

						.UpLift = CDec(Val(Trim(LogFuelUpLiftsArray(i)("mUpLift").ToString)))
						.UnitID = CInt(Trim(LogFuelUpLiftsArray(i)("mUnitID").ToString))
						.TOWeight = Trim(LogFuelUpLiftsArray(i)("mTOWeight").ToString)
						.Altitude = Trim(LogFuelUpLiftsArray(i)("mAltitude").ToString)
						.Remark = Trim(LogFuelUpLiftsArray(i)("mRemark").ToString)
						.FuelTypeID = New Guid(Trim(LogFuelUpLiftsArray(i)("mFuelTypeID")).ToString)

					End With

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetFlightCrew(FlightCrew As JArray, mLog As Log) As Log

		Dim mDateFormatString As String = ""
		Try

			For i As Integer = 0 To FlightCrew.Count - 1

				Dim mFlightCrew As LogCrew
				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogCrews(i).ID,
									  New Guid(FlightCrew(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(FlightCrew(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(FlightCrew(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(FlightCrew(i)("mIsDirty"))
				mDateFormatString = FlightCrew(i)("mDate")("mFormat")

				If mIsNew Then
					mLog.LogCrews.Add(mLog.ID)
					mFlightCrew = mLog.LogCrews.CurrentItem
				Else
					mFlightCrew = mLog.LogCrews(mID)
				End If

				If mIsNew Or mIsDirty Then

					With mFlightCrew
						.CrewID = New Guid(Trim(FlightCrew(i)("mCrewID").ToString))
						.DutyAsID = Val(Trim(FlightCrew(i)("mDutyAsID").ToString))
						.CrewName = FlightCrew(i)("mCrewName").ToString
						.DutyType = FlightCrew(i)("mDutyType").ToString
					End With

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetLogParametersObject(LogParameter As JArray, mLog As Log) As Log

		Try

			For i As Integer = 0 To LogParameter.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogParameters(i).ID,
									  New Guid(LogParameter(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(LogParameter(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(LogParameter(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(LogParameter(i)("mIsDirty"))
				Dim mLogParameter As LogParameter

				'If mIsNew Then
				'    mLog.LogParameters.Add(mLog.ID, New Guid(Trim(LogParameter(i)("mParameterID").ToString)))
				'    mLogParameter = mLog.LogParameters.CurrentItem
				'Else
				'    mLogParameter = mLog.LogParameters(mID)
				'End If
				mLogParameter = mLog.LogParameters(mID)

				If mIsNew Or mIsDirty Then

					With mLogParameter

						.ParameterID = New Guid(Trim(LogParameter(i)("mParameterID").ToString))
						.AssemblyID = New Guid(Trim(LogParameter(i)("mAssemblyID").ToString))
						.ParameterValue = LogParameter(i)("mParameterValue").ToString

					End With

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetLogOilsObject(LogOilsArray As JArray, mLog As Log) As Log

		Dim mDateFormatString As String = ""
		Try

			For i As Integer = 0 To LogOilsArray.Count - 1

				Dim mLogOil As LogOil
				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogOils(i).ID,
									  New Guid(LogOilsArray(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(LogOilsArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(LogOilsArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(LogOilsArray(i)("mIsDirty"))
				mDateFormatString = LogOilsArray(i)("mOilUpdatedDateTime")("mFormat")

				'If mIsNew Then
				'    mLogOil = mLog.LogOils.CurrentItem
				'Else
				'    mLogOil = mLog.LogOils(mID)
				'End If

				mLogOil = mLog.LogOils(mID)

				If mIsNew Or mIsDirty Then

					With mLogOil
						.Value = Val(Trim(LogOilsArray(i)("mValue").ToString))
						.OilUpdatedDateTime = CDate(LogOilsArray(i)("mOilUpdatedDateTime").First.First).ToString(format:=mDateFormatString)
					End With

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetMELSnagCorrectiveActionObject(MELSnagCorrectiveActionsArray As JArray, mLog As Log) As Log

		Dim mTempAssemblyList As AssemblyList
		Dim mReportLogRegister As New ReportLogRegister
		Dim mDateFormatString As String = ""
		mTempAssemblyList = AssemblyList.GetAssemblyList(1, mLog.MachineID.ToString)
		mReportLogRegister = ReportLogRegister.GetRectifiedLog(mLog.Date.ToString, mLog.Date.ToString, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "(SELECT)", True, mLog.ID.ToString, True)

		Try

			For i As Integer = 0 To MELSnagCorrectiveActionsArray.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.mMELSnagCorrectiveActions(i).ID,
									  New Guid(MELSnagCorrectiveActionsArray(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(MELSnagCorrectiveActionsArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(MELSnagCorrectiveActionsArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(MELSnagCorrectiveActionsArray(i)("mIsDirty"))
				mDateFormatString = MELSnagCorrectiveActionsArray(i)("mDate")("mFormat")

				Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction

				If mIsNew Then
					mLog.mMELSnagCorrectiveActions.add(mLog.ID)
					mMELSnagCorrectiveAction = mLog.mMELSnagCorrectiveActions.CurrentItem
				Else
					mMELSnagCorrectiveAction = mLog.MELSnagCorrectiveActions(mID)
				End If

				If mIsNew Or mIsDirty Then

					With mMELSnagCorrectiveAction

						.LogID = New Guid(MELSnagCorrectiveActionsArray(i)("mLogID").ToString)
						.LogNo = mReportLogRegister(New Guid(MELSnagCorrectiveActionsArray(i)("mLogID").ToString)).LogNo
						.DateOfOccurrence = CDate(MELSnagCorrectiveActionsArray(i)("mDateOfOccurrence").First.First).ToString(format:=mDateFormatString)
						.DefectReportNo = Trim(MELSnagCorrectiveActionsArray(i)("mDefectReportNo").ToString)
						.No = Val(MELSnagCorrectiveActionsArray(i)("mNo").ToString)
						.Sector = Trim(MELSnagCorrectiveActionsArray(i)("mSector").ToString)
						.LastMajorCheckHour = Trim(MELSnagCorrectiveActionsArray(i)("mLastMajorCheckHour").ToString)
						.SnagReportedBy = Trim(MELSnagCorrectiveActionsArray(i)("mSnagReportedBy").ToString)
						.ReportedBy = Trim(MELSnagCorrectiveActionsArray(i)("mReportedBy").ToString)
						.PartID = New Guid(MELSnagCorrectiveActionsArray(i)("mPartID").ToString)
						.PartSerialNo = Trim(MELSnagCorrectiveActionsArray(i)("mPartSerialNo").ToString)
						.Description = Trim(MELSnagCorrectiveActionsArray(i)("mDescription").ToString)
						.ComponentHour = Trim(MELSnagCorrectiveActionsArray(i)("mComponentHour").ToString)
						.Defect = Trim(Trim(MELSnagCorrectiveActionsArray(i)("mDefect").ToString))
						.CauseOfDefect = Trim(MELSnagCorrectiveActionsArray(i)("mCauseOfDefect").ToString)
						.Action = Trim(MELSnagCorrectiveActionsArray(i)("mAction").ToString)
						.ActionAgainstStaff = Trim(MELSnagCorrectiveActionsArray(i)("mActionAgainstStaff").ToString)
						.PreventionTaken = Trim(MELSnagCorrectiveActionsArray(i)("mPreventionTaken").ToString)
						.IsMEL = Trim(MELSnagCorrectiveActionsArray(i)("mIsMEL").ToString)
						.MELCategoryID = Val(MELSnagCorrectiveActionsArray(i)("mMELCategoryID").ToString)
						.ATAChapterID = New Guid(MELSnagCorrectiveActionsArray(i)("mATAChapterID").ToString)
						.IsMajor = CBool(MELSnagCorrectiveActionsArray(i)("mIsMajor"))
						.IsMinor = CBool(MELSnagCorrectiveActionsArray(i)("mIsMinor"))
						.InvestigationStatus = CBool(MELSnagCorrectiveActionsArray(i)("mInvestigationStatus"))
						.MachineID = New Guid(mLog.MachineID.ToString)
						.IsHours = CBool(MELSnagCorrectiveActionsArray(i)("mIsHours"))
						.FrequencyInDays = Val(MELSnagCorrectiveActionsArray(i)("mFrequencyInDays").ToString)
						.FrequencyInHours = Trim(MELSnagCorrectiveActionsArray(i)("mFrequencyInHours").ToString)
						.RectifiedStation = Trim(MELSnagCorrectiveActionsArray(i)("mRectifiedStation").ToString)
						.DueDate = CDate(MELSnagCorrectiveActionsArray(i)("mDueDate").First.First).ToString(format:=mDateFormatString)
						.RectifiedDate = CDate(MELSnagCorrectiveActionsArray(i)("mRectifiedDate").First.First).ToString(format:=mDateFormatString)
						.RectifiedLogID = New Guid(MELSnagCorrectiveActionsArray(i)("mRectifiedLogID").ToString)
						.PartNo = Trim(MELSnagCorrectiveActionsArray(i)("mPartNo").ToString)
						.IsRepetitive = CBool(MELSnagCorrectiveActionsArray(i)("mIsRepetitive"))
						.Remark = Trim(MELSnagCorrectiveActionsArray(i)("mRemark").ToString)
						.SubATAID = New Guid(MELSnagCorrectiveActionsArray(i)("mSubATAID").ToString)
						.IsPireps = CBool(MELSnagCorrectiveActionsArray(i)("mIsPireps"))
						.IsMaintenanceDefect = CBool(MELSnagCorrectiveActionsArray(i)("mIsMaintenanceDefect"))
						.IsInReliability = CBool(MELSnagCorrectiveActionsArray(i)("mIsInReliability"))
						.AssemblyStatusID = New Guid(MELSnagCorrectiveActionsArray(i)("mAssemblyStatusID").ToString)
						.ExtensionApplied = CBool(MELSnagCorrectiveActionsArray(i)("mExtensionApplied"))
						.ExtensionInDays = Val(MELSnagCorrectiveActionsArray(i)("mExtensionInDays").ToString)
						.ExtensionApprovalNo = Trim(MELSnagCorrectiveActionsArray(i)("mExtensionApprovalNo").ToString)
						.IncidentTypeID = Val(MELSnagCorrectiveActionsArray(i)("mIncidentTypeID").ToString)
						.IncidentTypeName = Trim(MELSnagCorrectiveActionsArray(i)("mIncidentTypeName").ToString)
						.IsAttachmentAdded = CBool(MELSnagCorrectiveActionsArray(i)("mIsAttachmentAdded"))
						.IsDeviationList = CBool(MELSnagCorrectiveActionsArray(i)("mIsDeviationList"))
						.AddToWatchList = CBool(MELSnagCorrectiveActionsArray(i)("mAddToWatchList"))
						.DueInHrs = Trim(MELSnagCorrectiveActionsArray(i)("mDueInHrs").ToString)
						.DueInCycles = Trim(MELSnagCorrectiveActionsArray(i)("mDueInCycles").ToString)
						.IsIncident = CBool(MELSnagCorrectiveActionsArray(i)("mIsIncident"))
						.ExtensionInHours = Val(MELSnagCorrectiveActionsArray(i)("mExtensionInHours").ToString)
						.ExtensionInCycles = Val(MELSnagCorrectiveActionsArray(i)("mExtensionInCycles").ToString)
						.IsAOG = CBool(MELSnagCorrectiveActionsArray(i)("mIsAOG"))

					End With

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SetLogMaintenanceObject(LogMaintenance As JArray, mLog As Log) As Log

		Dim mDateFormatString As String = ""
		Try

			For i As Integer = 0 To LogMaintenance.Count - 1

				Dim mID As Guid = IIf(mLog.IsNew,
									  mLog.LogMaintenances(i).ID,
									  New Guid(LogMaintenance(i)("mID").ToString))

				Dim mIsNew As Boolean = CBool(LogMaintenance(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(LogMaintenance(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(LogMaintenance(i)("mIsDirty"))
				mDateFormatString = LogMaintenance(i)("mDate")("mFormat")

				Dim mLogMaintenance As LogMaintenance
				If mIsNew Then
					mLog.LogMaintenances.Add(mLog.ID)
					mLogMaintenance = mLog.LogMaintenances.CurrentItem
				Else
					mLogMaintenance = mLog.LogMaintenances(mID)
				End If

				If mIsNew Or mIsDirty Then

					With mLogMaintenance

						.Maintenance = Trim(LogMaintenance(i)("mMaintenance").ToString)
						.NRCWONO = Trim(LogMaintenance(i)("mNRCWONO").ToString)
						.Place = Trim(LogMaintenance(i)("mPlace").ToString)
						.ClosedDate = CDate(LogMaintenance(i)("mClosedDate").First.First).ToString(format:=mDateFormatString)
						.AssemblyStatusID = New Guid(Trim(LogMaintenance(i)("mAssemblyStatusID").ToString))
						.ImageFile = LogMaintenance(i)("mImageFile")
						.ImageSize = Val(LogMaintenance(i)("mImageSize").ToString)
						.FileExtension = Trim(LogMaintenance(i)("mFileExtension").ToString)

						Dim ItemArray As JArray = CType(LogMaintenance("mMaintenanceDoneByEmployees"), JArray)

						For j As Integer = 0 To ItemArray.Count - 1

							mID = New Guid(ItemArray(j)("mID").ToString)
							mIsNew = CBool(ItemArray(j)("mIsNew"))
							mIsDeleted = CBool(ItemArray(j)("mIsDeleted"))
							mIsDirty = CBool(ItemArray(j)("mIsDirty"))

							Dim mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee
							Dim MaintenanceID As Guid = New Guid(ItemArray(j)("mMaintenanceID").ToString)
							Dim MaintenanceTypeID As Integer = Val(ItemArray(j)("mMaintenanceTypeID").ToString)
							Dim EmployeeID As Guid = New Guid(ItemArray(j)("mEmployeeID").ToString)
							Dim LicenseNo As String = Trim(ItemArray(j)("mLicenceNo")).ToString
							Dim ActualManHours As String = Trim(ItemArray(j)("mActualManHours").First.First).ToString
							Dim EmpName As String = Trim(ItemArray(j)("mEmployeeName")).ToString

							If mIsNew Then

								mLogMaintenance.MaintenanceDoneByEmployees.Add(MaintenanceID:=MaintenanceID,
																			   MaintenanceTypeID:=MaintenanceTypeID,
																			   EmployeeID:=EmployeeID,
																			   LicenceNo:=LicenseNo,
																			   ActualManHours:=ActualManHours,
																			   EmpName:=EmpName)

								mMaintenanceDoneByEmployee = mLogMaintenance.MaintenanceDoneByEmployees.CurrentItem

							Else
								mMaintenanceDoneByEmployee = mLogMaintenance.MaintenanceDoneByEmployees(mID)
							End If

							If mIsDeleted Then
								mLogMaintenance.MaintenanceDoneByEmployees.Remove(mMaintenanceDoneByEmployee)
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

					End With

				End If

			Next

			Return mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " SAVE LOG DETAIL "

	<HttpPost>
	Public Function SaveLog(<FromBody()> value As Object) As IHttpActionResult

		Dim jsonObject As JObject = JObject.Parse(value.ToString)
		Dim IsNew As Boolean = CBool(jsonObject("mIsNew"))
		Dim returnstring As String

		Try

			returnstring = SetLog(jsonObject:=jsonObject,
								  IsNew:=IsNew)

			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "Log saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring)
			'End If

			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Log Saved Successfully!"))

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

	Private Function SetLog(jsonObject As JObject,
							IsNew As Boolean,
							Optional IsForCalculation As Boolean = False) As Object
		Try

			Dim mDateFormatString As String = ""
			mDateFormatString = jsonObject(propertyName:="mDate")("mFormat")

			Dim mLog As Log
			Dim mMachine As Machine

			If IsNew Then

				mMachine = Machine.GetMachine(MachineID:=New Guid(jsonObject("mMachineID").ToString))

				mLog = Log.NewLog(Machine:=mMachine,
								  LogDate:=CDate(jsonObject(propertyName:="mDate").First.First).ToString(format:=mDateFormatString),
								  mSouLocalDateTime:=CDate(jsonObject(propertyName:="mSouLocalDateTime").First.First).ToString,
								  mSouUTCDateTime:=CDate(jsonObject(propertyName:="mSouUniverseDateTime").First.First).ToString,
								  LogTypeID:=CInt(jsonObject("mLogTypeID")))


			Else

				mLog = Log.GetLog(ID:=New Guid(jsonObject("mID").ToString))
				mMachine = Machine.GetMachine(mLog.MachineID)

			End If

			If Not CheckZeroDifferenceValue(mLog) Then

				If mLog.LogAFAssemblies.AssemblyRemoved Or
				   mLog.LogEngAssemblies.AssemblyRemoved Or
				   mLog.PropLogAssemblies.AssemblyRemoved Or
				   mLog.LogAPUAssemblies.AssemblyRemoved Or
				   mLog.LogCGBAssemblies.AssemblyRemoved Or
				   mLog.LogNGBAssemblies.AssemblyRemoved Or
				   mLog.LogGEAssemblies.AssemblyRemoved Or
				   mLog.LogMRHAssemblies.AssemblyRemoved Or
				   mLog.LogSPSAssemblies.AssemblyRemoved Or
				   mLog.LogSSAAssemblies.AssemblyRemoved Then

					Return "Required Assembly of the Aircraft is Not Installed on this Date of Log."

				End If

			End If

			mLog.IsUTC = mMachine.IsUTC
			mLog.IsTLP = mMachine.IsTLP

			Dim AFLogAssembliesArray As JArray = CType(jsonObject("mAFLogAssemblies"), JArray)
			Dim EngLogAssembliesArray As JArray = CType(jsonObject("mEngLogAssemblies"), JArray)
			Dim PropLogAssembliesArray As JArray = CType(jsonObject("mPropLogAssemblies"), JArray)
			Dim APULogAssembliesArray As JArray = CType(jsonObject("mAPULogAssemblies"), JArray)
			Dim CGBLogAssembliesArray As JArray = CType(jsonObject("mCGBLogAssemblies"), JArray)
			Dim MGBLogAssembliesArray As JArray = CType(jsonObject("mMGBLogAssemblies"), JArray)
			Dim GELogAssembliesArray As JArray = CType(jsonObject("mGELogAssemblies"), JArray)
			Dim MRHLogAssembliesArray As JArray = CType(jsonObject("MRHLogAssemblies"), JArray)
			Dim SPSLogAssembliesArray As JArray = CType(jsonObject("mSPSLogAssemblies"), JArray)
			Dim SSALogAssembliesArray As JArray = CType(jsonObject("mSSALogAssemblies"), JArray)
			Dim LogFuelsArray As JArray = CType(jsonObject("mLogFuels"), JArray)
			Dim LogOilsArray As JArray = CType(jsonObject("mLogOils"), JArray)
			Dim FuelUpLiftsArray As JArray = CType(jsonObject("mFuelUpLifts"), JArray)
			Dim LogParametersArray As JArray = CType(jsonObject("mLogParameters"), JArray)
			Dim MELSnagCorrectiveActionsArray As JArray = CType(jsonObject("mMELSnagCorrectiveActions"), JArray)
			Dim FileAttachmentsArray As JArray = CType(jsonObject("mFileAttachments"), JArray)
			Dim FlightCrew As JArray = CType(jsonObject("mLogCrews"), JArray)
			Dim LogMaintenancesArray As JArray = CType(jsonObject("mLogMaintenances"), JArray)

			'Set LOG 
			With mLog

				.Date = CDate(jsonObject(propertyName:="mDate").First.First).ToString(format:=mDateFormatString)
				.LogText = jsonObject(propertyName:="mLogText")
				.LogNo = jsonObject(propertyName:="mLogNo")
				.PilotID1 = New Guid(jsonObject("mPilot1ID").ToString)
				.PilotID2 = New Guid(jsonObject("mPilot2ID").ToString)
				.SourceID = New Guid(jsonObject("mSourceID").ToString)
				.DestinationID = New Guid(jsonObject("mDestinationID").ToString)

				If .IsUTC = True Then
					.SouUniverseDateTime = CDate(jsonObject(propertyName:="mSouUniverseDateTime").First.First).ToString
					.DesUniverseDateTime = CDate(jsonObject(propertyName:="mDesUniverseDateTime").First.First).ToString
				Else
					.SouLocalDateTime = CDate(jsonObject(propertyName:="mSouLocalDateTime").First.First).ToString
					.DesLocalDateTime = CDate(jsonObject(propertyName:="mDesLocalDateTime").First.First).ToString
				End If

				.SouDayLightTime = jsonObject(propertyName:="mSouDayLightTime")
				.DesDayLightTime = jsonObject(propertyName:="mDesDayLightTime")

				If .IsUTC Then

					If TakeOffTouchDown Then
						.TouchDownUniverseDateTime = CDate(jsonObject(propertyName:="mTouchDownUniverseDateTime").First.First).ToString
						.TakeOffUniverseDateTime = CDate(jsonObject(propertyName:="mTakeOffUniverseDateTime").First.First).ToString
					End If

				Else

					If TakeOffTouchDown Then
						.TouchDownLocalDateTime = CDate(jsonObject(propertyName:="mTouchDownLocalDateTime").First.First).ToString
						.TakeOffLocalDateTime = CDate(jsonObject(propertyName:="mTakeOffLocalDateTime").First.First).ToString
					End If

				End If

				'Detail Page code ***************************
				If AppSettings("SetBlockTime") = "True" Then

					If Not .BlockTime.Equals(jsonObject(propertyName:="mBlockTime")) Then
						.BlockTime = New Period(1, CType((jsonObject(propertyName:="mBlockTime").First.First).ToString, Decimal), 1, False, False, 1).ValueFormatted ''jsonObject(propertyName:="mBlockTime")
					End If

				End If

				If AppSettings("LogDetailPage") = "OldPage" Then

					.TimeInAir = New Period(1, CType((jsonObject(propertyName:="mTimeInAir").First.First).ToString, Decimal), 1, False, False, 1).ValueFormatted

					If Not AppSettings("Log") = "True" Then .TimeOnGround = New Period(1, CType((jsonObject(propertyName:="mTimeOnGround").First.First).ToString, Decimal), 1, False, False, 1).ValueFormatted ''jsonObject(propertyName:="mTimeOnGround")

				End If

				.PercentTimeOnGround = Val(jsonObject(propertyName:="mPercentTimeOnGround"))

				If mMachine.HourType = 2 Then

					.PrevHobbsValue = jsonObject(propertyName:="mPrevHobbsValue")
					.PrevHobbsOffsetValue = Trim(jsonObject(propertyName:="mPrevHobbsOffsetValue"))
					.CurrentHobbsOffsetValue = Trim(jsonObject(propertyName:="mCurrentHobbsOffsetValue"))
					.CurrentHobbsValue = Trim(jsonObject(propertyName:="mCurrentHobbsValue"))
					.OffSet = Trim(jsonObject(propertyName:="mOffSet"))

				End If

				.LogPageNo = Trim(jsonObject(propertyName:="mLogPageNo"))
				.FlightNo = Trim(jsonObject(propertyName:="mFlightNo"))
				.Remark = Trim(jsonObject(propertyName:="mRemark"))
				.FlightLogClassificationID = New Guid(Trim(jsonObject(propertyName:="mFlightLogClassificationID")))
				.FlightLogClassificationName = Trim(jsonObject(propertyName:="mFlightLogClassificationName"))
				.IsValZero = CBool(jsonObject(propertyName:="mIsValZero"))
				.IsAttachmentAdded = CBool(jsonObject(propertyName:="mIsAttachmentAdded"))

			End With

			For i As Integer = 0 To FileAttachmentsArray.Count - 1

				Dim mID As Guid = New Guid(FileAttachmentsArray(i)("mID").ToString)
				Dim mIsNew As Boolean = CBool(FileAttachmentsArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(FileAttachmentsArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(FileAttachmentsArray(i)("mIsDirty"))

				mLog.FileAttachments(i).FileName = FileAttachmentsArray(i)("mFileName").ToString

			Next

			mLog.IsAttachmentAdded = IIf(mLog.FileAttachments.Count > 0, True, CBool(jsonObject(propertyName:="mIsAttachmentAdded")))

			'************************* Airframe *************************
			mLog = SetAFAssemblyObject(AFLogAssembliesArray, mLog)

			'************************* Engine *************************
			mLog = SetEngAssemblyObject(EngLogAssembliesArray, mLog)

			'************************* APU *************************
			mLog = SetAPUAssemblyObject(APULogAssembliesArray, mLog)

			'************************* CGB *************************
			mLog = SetCGBAssemblyObject(CGBLogAssembliesArray, mLog)

			'************************* LogFuelsArray *************************
			If LogFuelsArray.Count > 0 Then
				mLog = SetLogFuelObject(LogFuelsArray, mLog)
			End If

			'************************* LogOilsArray *************************
			If LogOilsArray.Count > 0 Then
				mLog = SetLogOilsObject(LogOilsArray, mLog)
			End If

			'************************* FuelUpLiftsArray *************************
			If FuelUpLiftsArray.Count > 0 Then
				mLog = SetFuelUpLiftsObject(FuelUpLiftsArray, mLog)
			End If

			'************************* FlightCrew *************************
			If FlightCrew.Count > 0 Then
				mLog = SetFlightCrew(FlightCrew, mLog)
			End If

			'************************* LogParametersArray *************************
			If LogParametersArray.Count > 0 Then
				mLog = SetLogParametersObject(LogParametersArray, mLog)
			End If

			'************************* MELSnagCorrectiveActionsArray *************************
			If MELSnagCorrectiveActionsArray.Count > 0 Then
				mLog = SetMELSnagCorrectiveActionObject(MELSnagCorrectiveActionsArray, mLog)
			End If

			'************************* LogMaintenance *************************
			If LogMaintenancesArray.Count > 0 Then
				mLog = SetLogMaintenanceObject(LogMaintenancesArray, mLog)
			End If

			'************************* Pilot 1 *************************
			'IF Pilot 1 deleted...
			If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

				If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
					mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
				End If

			ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then

				If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
					Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
					LogCrew.CrewID = mLog.PilotID1
					LogCrew.DutyAsID = 1
					mLog.LogCrews.Add(LogCrew)
				End If

			ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

				If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then

					If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
						Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
						LogCrew.CrewID = mLog.PilotID1
						LogCrew.DutyAsID = 1
						mLog.LogCrews.Add(LogCrew)
					End If

				Else

					If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
						Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
						LogCrew.CrewID = mLog.PilotID1
						LogCrew.DutyAsID = 1
						mLog.LogCrews.Add(LogCrew)
					Else
						mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
					End If

				End If

			End If

			'************************* Pilot 2 *************************
			'IF Pilot 2 deleted...
			If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

				If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
					mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
				End If

			ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then

				If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

					Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
					LogCrew.CrewID = mLog.PilotID2
					LogCrew.DutyAsID = 2
					mLog.LogCrews.Add(LogCrew)

				End If

			ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

				If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then

					If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
						Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
						LogCrew.CrewID = mLog.PilotID2
						LogCrew.DutyAsID = 2
						mLog.LogCrews.Add(LogCrew)
					End If

				Else

					If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
						Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
						LogCrew.CrewID = mLog.PilotID2
						LogCrew.DutyAsID = 2
						mLog.LogCrews.Add(LogCrew)
					Else
						mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
					End If

				End If

			End If

			'************************* Log Save *************************
			If mLog.IsValid Then

				If Not IsForCalculation Then

					mLog.Save()

					Try

						Dim mMaxLogOfAircraft As MaxLogOfAircraft
						mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

						If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

							If Not (AppSettings("ClientCode") = "Heligo" Or
								AppSettings("ClientCode") = "UHPL" Or
								AppSettings("ClientCode") = "APFT" Or
								AppSettings("ClientCode") = "AAP") Then

								If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then
									SetPBHValues(mLog, IsNew)
								End If

							Else

								If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then
									SetPBHValues(mLog, IsNew)
								End If

							End If

						End If

					Catch ex As Exception
						Return ex.InnerException.ToString
					Finally

					End Try

					'************************* Mail Sending *************************
					Dim mModuleList As ModuleList = ModuleList.GetModuleList("Flight Log")

					If mModuleList.Item("Flight Log").MailsRequire = True Then

						If User.Identity.Name.ToUpper = "BTPLADMIN" Then

						Else

							SendMailFile.SendMailFile(UserName:=User.Identity.Name,
													  Subject:="Log successfully saved from the new UI. Client " + AppSettings("ClientCode"),
													  Info:=mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " User Name:- " + User.Identity.Name,
													  ToMailID:="support@bytzsoft.com")

						End If

					End If

					Return "Success"

				Else

					Return mLog

				End If

			Else

				Dim BrokenRules As String = ""

				For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
					BrokenRules = BrokenRules + mLog.GetBrokenRulesCollection(i).Description
				Next

				'************************* AirFrame *************************
				For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1

					If Not mLog.LogAFAssemblies(i).IsValid Then

						Dim x As Integer

						For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
							BrokenRules = BrokenRules + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description
						Next

					End If

				Next

				'************************* Engine *************************
				For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1

					If Not mLog.LogEngAssemblies(i).IsValid Then

						Dim x As Integer

						For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
							BrokenRules = BrokenRules + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description
						Next

					End If

				Next

				'************************* APu *************************
				For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1

					If Not mLog.LogAPUAssemblies(i).IsValid Then

						Dim x As Integer

						For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
							BrokenRules = BrokenRules + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description
						Next

					End If

				Next

				Return BrokenRules

			End If

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " Other Set / Get Methods "

	<HttpGet>
	Public Function SetPilotOnLoad() As String

		Try

			If (AppSettings("ClientCode") = "Heligo" Or
				AppSettings("ClientCode") = "UHPL" Or
				AppSettings("ClientCode") = "APFT" Or
				AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018

				Return "None"

			Else
				Return ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Check Zero-Difference Value "

	Private Function CheckZeroDifferenceValue(mLog As Log) As Boolean

		Try

			If mLog.IsHobbs Then

				If Val(mLog.TotalTime) <> 0 Then
					Return False
				End If

				If Val(mLog.TimeInAir) <> 0 Then
					Return False
				End If

			Else

				If mLog.TimeInAir = "0:00" OrElse mLog.TimeInAir = "" Then
				Else
					Return False
				End If

				If mLog.TotalTime = "0:00" OrElse mLog.TotalTime = "" Then
				Else
					Return False
				End If

				If mLog.BlockTime = "0:00" OrElse mLog.BlockTime = "" Then
				Else
					Return False
				End If

				If mLog.TimeOnGround = "0:00" OrElse mLog.TimeOnGround = "" Then
				Else
					Return False
				End If

			End If

			If Val(mLog.TotalLandings) <> 0 Then
				Return False
			End If

			Dim checkcol = mLog.LogAFAssemblies
			If Not callZeroDifferenceValue(checkcol, mLog) Then
				Return False
			End If

			checkcol = mLog.LogAPUAssemblies
			If Not callZeroDifferenceValue(checkcol, mLog) Then
				Return False
			End If

			checkcol = mLog.LogEngAssemblies
			If Not callZeroDifferenceValue(checkcol, mLog) Then
				Return False
			End If

			checkcol = mLog.LogCGBAssemblies
			If Not callZeroDifferenceValue(checkcol, mLog) Then
				Return False
			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CallZeroDifferenceValue(obj As Object, mLog As Log) As Boolean

		Try

			For i As Integer = 0 To obj.Count - 1

				If mLog.IsHobbs Then

					If Val(obj(i).Hours) <> 0 Then
						Return False
					End If

				Else

					If obj(i).Hours <> "0:00" Then
						Return False
					End If

				End If

				If obj(i).ShowLandings Then

					If Val(obj(i).Landings) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowCycles Then

					If Val(obj(i).Cycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowStarts Then

					If Val(obj(i).Starts) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowNGCycles Then

					If Val(obj(i).NGCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowNFCycles Then

					If Val(obj(i).NFCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowRINS Then

					If Val(obj(i).RINS) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowBleeds Then

					If Val(obj(i).Bleeds) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowImpellerCycles Then

					If Val(obj(i).ImpellerCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowCTCycles Then

					If Val(obj(i).CTCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowPTCycles Then

					If Val(obj(i).PTCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowGeneratorMods Then

					If Val(obj(i).GeneratorMods) <> 0 Then
						Return False
					End If

				End If

			Next

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Avg Flight-Time Deviation "

	<HttpGet>
	Public Function GetAvgFlightTimeDeviation(LogDate As String,
											  TimeInAir As String,
											  SourcePlaceID As String,
											  DesPlaceID As String,
											  MachineID As String) As IHttpActionResult
		Dim Message As String = String.Empty
		Try

			Dim mMachine As Machine = Machine.GetMachine(MachineID:=New Guid(MachineID))

			If AvgFlightTimeDeviation(LogDate:=LogDate,
									  TimeInAir:=TimeInAir,
									  SourcePlaceID:=SourcePlaceID,
									  DesPlaceID:=DesPlaceID,
									  ModelID:=mMachine.AssemblyStatus.Assembly.ModelID.ToString) AndAlso
			   Not (AppSettings("ClientCode") = "Heligo" Or
					AppSettings("ClientCode") = "UHPL" Or
					AppSettings("ClientCode") = "APFT" Or
					AppSettings("ClientCode") = "AAP") Then

				Message = $"Airborne Time of this flight is 
                          {IIf(IsFlightTimeGreaterThanAvgFlightTime = True,
							   "Greater",
							   "Less")}  
                          than the Average Flight Time for this current sector.
                          Do you still want to Save Log? "

			End If

			Return Json(New With {.response = Message})

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function AvgFlightTimeDeviation(LogDate As String,
										   TimeInAir As String,
										   SourcePlaceID As String,
										   DesPlaceID As String,
										   ModelID As String) As Boolean

		Try

			If AppSettings("NoOfLogsToConsiderForAvgFlightTime") <> "0" And
			   AppSettings("DeviationInAvgFlightTimeInPercentage") <> "0" Then

				Dim mLastLogDetails As LastLogDetails =
					LastLogDetails.GetLastLogDetails(False,
														LogDate,
														CType(AppSettings("NoOfLogsToConsiderForAvgFlightTime"), Integer),
														SourcePlaceID.ToString,
														DesPlaceID.ToString,
														ModelID.ToString)

				If mLastLogDetails.Count > 0 Then

					Dim CurrentLogTimeInAirInDec As Decimal = New Period(1, TimeInAir, 0, False, False).DbValueDec
					Dim AllowedDeviationInDec = (mLastLogDetails.AvgFlightTime * CType(AppSettings("DeviationInAvgFlightTimeInPercentage"), Integer) / 100)
					Dim ActualDeviationInDec As Decimal = Math.Abs(CurrentLogTimeInAirInDec - mLastLogDetails.AvgFlightTime)

					If ActualDeviationInDec > AllowedDeviationInDec Then

						If CurrentLogTimeInAirInDec > mLastLogDetails.AvgFlightTime Then
							IsFlightTimeGreaterThanAvgFlightTime = True
						End If

						Return True

					Else
						Return False
					End If

				Else
					Return False
				End If

			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " PBH "

	Private Sub SetPBHValues(TmpLog As Log, IsLogNew As Boolean)

		Try

			If mCompanyDetail.IsCombinedHours = False Then

				If IsLogNew Then

					Dim mPBH As PBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")

					If Not mPBH.MachineID.Equals(Guid.Empty) Then

						If CDate(Today.Date) >= CDate(mPBH.StartDate) Then

							mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec

							If IsLogNew Then
								mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
							Else
								mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec + New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec), 1, False, False).Value
							End If

							mPBH.RemainingHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.ElapsedHoursDec, 1, False, False).Value
							mPBH.LastLogDetails = TmpLog.DateFormatted

							'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
							'Also mark Not InUse in tabMachine at same time 
							If mPBH.RemainingHoursDec <= 0 Then

								mPBH.IsNotActive = True
								mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
								mPBH.MachineNotInUse = True

							End If

							mPBH.Save()

						End If

					End If

				End If

			ElseIf mCompanyDetail.IsCombinedHours = True Then

				Dim mPBH As PBH

				If IsLogNew Then

					Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
					mPBH = PBH.GetPBH(mPBHList(0).ID)

					If CDate(Today.Date) >= CDate(mPBH.StartDate) Then

						mPBH.RemainingHours = New Period(1, mPBH.RemainingHoursDec - New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec, 1, False, False).Value

						If mPBH.CarryForwardHoursDec < 0 Then
							mPBH.ElapsedHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.RemainingHoursDec, 1, False, False).Value
						Else
							mPBH.ElapsedHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.RemainingHoursDec, 1, False, False).Value
						End If

						mPBH.LastLogDetails = TmpLog.DateFormatted

						If mPBH.RemainingHoursDec <= 0 Then

							mPBH.IsNotActive = True
							mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
							mPBH.MachineNotInUse = True

						End If

						mPBH.Save()

					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Minimum Equipment Level "

	<HttpGet>
	Public Function GetMinimumEquipmentLevel(LogDate As String,
											 MachineID As String) As IHttpActionResult

		Dim IsMELCount As Boolean = False
		Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
		Dim Message As String = String.Empty
		Try

			mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(MachineID:=MachineID.ToString,
																										  IsMELPartsRequired:=True)

			If mTempMELSnagCorrectiveActionList.Count > 0 Then

				For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1

					If mTempMELSnagCorrectiveActionList(i).DueDate.ToString <> "" Then

						If (CDate(LogDate.ToString) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And
						   (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then

							IsMELCount = True

							Exit For
						Else
							IsMELCount = False
						End If

					End If

				Next

			End If

			If IsMELCount = True Then
				Message = " Installed Components does not fulfill Minimum Equipment Level to Fly. Do you want to continue? "
			End If

			Return Json(New With {.response = Message})

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Duty-Type List "

	<HttpGet>
	Public Function GetDutyTypeList(Optional IsTagRequired As String = "",
									Optional TagText As String = "") As DutyTypeList

		Try

			Return DutyTypeList.GetDutyTypeList(IsTagRequired:=IsTagRequired,
												TagText:=TagText)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Check Avg Fuel Consumption "

	<HttpGet>
	Public Function GetCheckAvgFuelConsumption(ID As String,
											   ClientCode As String) As String

		Dim mLog As Log = Log.GetLog(ID:=New Guid(ID))
		Dim AvgFuelConsumption, AvgOilConsumption, TotalFuelConsume As Decimal
		Dim EngineName As String

		Try

			If ClientCode = "APFT" Or ClientCode = "AAP" Then

				Dim MaxFuelLimit As String = "28 lt/hr"

				If ClientCode = "AAP" Then
					MaxFuelLimit = "22.1 lt/hr"
				End If

				TotalFuelConsume = mLog.LogFuels(0).Consumtion
				Dim BT As Decimal
				BT = (mLog.BlockTimeDec)

				If BT <> 0 Then

					AvgFuelConsumption = (TotalFuelConsume / BT) * 60
					EngineName = mLog.LogFuels(0).TankName.ToString

					If (AvgFuelConsumption > 28 And ClientCode = "APFT") Or (AvgFuelConsumption > 22.1 And ClientCode = "AAP") Then
						Dim str As String = "ALERT: Maximum fuel consumption is " + MaxFuelLimit + " .This Flight Log is exceeding the above limit."
					End If

				End If

				If mLog.LogFuels.Count = 2 Then

					TotalFuelConsume = mLog.LogFuels(1).Consumtion

					If BT <> 0 Then

						AvgFuelConsumption = (TotalFuelConsume / BT) * 60
						EngineName = mLog.LogFuels(1).TankName.ToString

						If (AvgFuelConsumption > 28 And ClientCode = "APFT") Or (AvgFuelConsumption > 22.1 And ClientCode = "AAP") Then

							Dim str As String = "ALERT: Maximum fuel consumption is " + MaxFuelLimit + " .This Flight Log is exceeding the above limit."
							Return str

						End If

					End If

				End If

				Dim Value1 As Decimal
				Value1 = mLog.LogOils(0).Value

				If BT <> 0 Then

					AvgOilConsumption = (Val(Value1.ToString) / BT) * 60
					EngineName = mLog.LogOils(0).AssemblyName.ToString

					Dim AvgOilConsumption0 As String
					If AvgOilConsumption > 0 Then
						AvgOilConsumption0 = AvgOilConsumption.ToString(".##")
					Else
						AvgOilConsumption0 = AvgOilConsumption.ToString
					End If

					If AvgOilConsumption > 0.1 Then

						Dim str As String = "ALERT: Maximum Oil consumption is 0.1 lt/hr" + " .This Flight Log is exceeding the above limit."
						Return str

					End If

				End If

				If mLog.LogOils.Count = 2 Then

					Dim Value2 As Decimal
					Value2 = mLog.LogOils(1).Value

					If BT <> 0 Then

						AvgOilConsumption = (Val(Value2.ToString) / BT) * 60
						EngineName = mLog.LogOils(1).AssemblyName.ToString

						Dim AvgOilConsumption1 As String

						If AvgOilConsumption > 0 Then
							AvgOilConsumption1 = AvgOilConsumption.ToString(".##")
						Else
							AvgOilConsumption1 = AvgOilConsumption.ToString
						End If

						EngineName = mLog.LogOils(1).AssemblyName.ToString

						If AvgOilConsumption > 0.1 Then

							Dim str As String = "ALERT: Maximum Oil consumption is 0.1 lt/hr" + " .This Flight Log is exceeding the above limit."
							Return str

						End If

					End If

				End If

			End If

			Return ""

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Log-Parameter List "

	<HttpGet>
	Public Function GetLogParameterList(LogID As Guid) As IHttpActionResult

		Dim LogParameterListHelper As New LogParameterListHelper
		Try

			Dim _Log As Log = Log.GetLog(LogID)
			Dim _LogParameterList As LogParameters = LogParameters.NewLogParameters()
			Dim _AssemblyParameterListForAssemblyStatus As AssemblyParameterListForAssemblyStatus

			_AssemblyParameterListForAssemblyStatus =
				AssemblyParameterListForAssemblyStatus.
					GetAssemblyParameterListForAssemblyStatus(LogDate:=_Log.Date.ToString,
															  MachineID:=_Log.MachineID)

			Dim result = LogParameterListHelper.
							GetLogParameterList(_AssemblyParameterListForAssemblyStatus:=_AssemblyParameterListForAssemblyStatus,
												_Log:=_Log,
												_LogParameterList:=_LogParameterList)

			Dim unifiedRows = LogParameterListHelper.
								BuildUnifiedGridRows(staticRowData:=result.Item1,
													 dynamicRowData:=result.Item2,
													 _AssemblyParameterListForAssemblyStatus:=_AssemblyParameterListForAssemblyStatus,
													 _Log:=_Log)

			Return Json(unifiedRows)

		Catch ex As Exception
			Return InternalServerError(ex)
		End Try

	End Function

#End Region

#Region " Log Selection List "

	<HttpGet>
	Public Function GetLogSelectionList(LogDate As String,
										AssemblyID As String,
										MachineID As String,
										CalculateTotal As Boolean,
										StatusSelectLog As Integer,
										Optional FlightLogClassificationName As String = " ",
										Optional IsLogNo As Boolean = False,
										Optional IsLogPageNo As Boolean = False,
										Optional IsFlightNo As Boolean = False,
										Optional SkipVoidLog As Boolean = False,
										Optional SkipMaintLog As Boolean = False,
										Optional IsFlightLogClassification As Boolean = False,
										Optional GetLogPeriodsDayWise As Boolean = False,
										Optional ShowSinceTSO As Boolean = False,
										Optional IsUTC As Boolean = False,
										Optional IsForLogBook As Boolean = False) As LogListForSelection

		Try

			Return LogListForSelection.GetLogList(StartDate:=LogDate,
												  EndDate:=LogDate,
												  AssemblyID:=AssemblyID,
												  MachineID:=MachineID,
												  CalculateTotal:=CalculateTotal,
												  FlightLogClassificationName:=FlightLogClassificationName,
												  StatusSelectLog:=StatusSelectLog,
												  IsLogNo:=IsLogNo,
												  IsLogPageNo:=IsLogPageNo,
												  IsFlightNo:=IsFlightNo,
												  SkipVoidLog:=SkipVoidLog,
												  SkipMaintLog:=SkipMaintLog,
												  IsFlightLogClassification:=IsFlightLogClassification,
												  GetLogPeriodsDayWise:=GetLogPeriodsDayWise,
												  ShowSinceTSO:=ShowSinceTSO,
												  IsUTC:=IsUTC,
												  IsForLogBook:=IsForLogBook)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class