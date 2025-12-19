'***********************************
'Created by:  Harsh & Sankalp
'Created on:  25th September 2025
'Created for: FLYPAL-2705 & FLYPAL-2706
'***********************************


Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class RequisitionNewController
	Inherits ApiController


#Region " Varriable(s) "

	Private _CheckForSubscriptionExpired As New CheckForSubscriptionExpired
	Private _ReportHelper As New ReportHelper
	Private _EmailHelper As New EmailHelper
	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _MessageBox As New MSGBox

#End Region

#Region " Get Method(s) "

	<HttpGet>
	<Route("api/RequisitionNew/RequisitionList")>
	Public Function RequisitionList(Optional ItemName As String = "",
									Optional Text As String = "",
									Optional No As Integer = 0,
									Optional FromDate As String = "1/1/1900",
									Optional ToDate As String = "1/1/3300",
									Optional StatusID As Integer = 0,
									Optional Location As String = "",
									Optional Employee As String = "",
									Optional LocationID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional Aircraft As String = "",
									Optional ReqTypeID As Integer = 0,
									Optional TransTypeID As Trans = Trans.EngineeringRequisition,
									Optional WorkShopID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional Description As String = "",
									Optional IsFromQuotationComparison As Boolean = False,
									Optional WOID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional DoneOrder As Boolean = False,
									Optional SearchText As String = "") As IHttpActionResult

		Dim _RequisitionListNew As RequisitionListNew

		Try

			_RequisitionListNew = RequisitionListNew.GetRequisitionList(ItemName:=ItemName,
																		Text:=Text,
																		No:=No,
																		FromDate:=FromDate,
																		ToDate:=ToDate,
																		StatusID:=StatusID,
																		Location:=Location,
																		Employee:=Employee,
																		LocationID:=LocationID,
																		Aircraft:=Aircraft,
																		ReqTypeID:=ReqTypeID,
																		TransTypeID:=TransTypeID,
																		WorkShopID:=WorkShopID,
																		Description:=Description,
																		IsFromQuotationComparison:=IsFromQuotationComparison,
																		WOID:=WOID,
																		DoneOrder:=DoneOrder,
																		SearchText:=SearchText)

			Return Ok(_RequisitionListNew)

		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError, ex.GetBaseException)
		End Try

	End Function

	<HttpGet>
	<Route("api/RequisitionNew/NewRequisition")>
	Public Function NewRequisition(Optional TransTypeID As Trans = Trans.EngineeringRequisition) As IHttpActionResult

		Dim RequisitionNew As RequisitionNew
		Try

			RequisitionNew = RequisitionNew.NewRequisition(TransTypeID:=TransTypeID)

			Return Ok(RequisitionNew)

		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError, ex.GetBaseException)
		End Try

	End Function

	<HttpGet>
	<Route("api/RequisitionNew/NewRequisition")>
	Public Function NewRequisition(ID As String,
								   Optional TransTypeID As Trans = Trans.EngineeringRequisition) As IHttpActionResult

		Dim RequisitionNew As RequisitionNew
		Try

			RequisitionNew = RequisitionNew.NewRequisition(ID:=New Guid(ID),
														   TransTypeID:=TransTypeID)


			Return Ok(RequisitionNew)

		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError, ex.GetBaseException)
		End Try

	End Function

	<HttpGet>
	<Route("api/RequisitionNew/RequisitionText")>
	Public Function RequisitionText([Of] As String,
									Optional WithType As Integer = 0,
									Optional IsSelectTagRequired As Boolean = False,
									Optional Tag As String = "<SELECT>",
									Optional TransTypeID As Integer = 0) As IHttpActionResult

		Dim mDistinctTextListForRequisition As DistinctTextListForRequisition

		Try

			mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList([Of]:=[Of],
																								 WithType:=WithType,
																								 IsSelectTagRequired:=IsSelectTagRequired,
																								 Tag:=Tag,
																								 TransTypeID:=TransTypeID)
			Return Ok(mDistinctTextListForRequisition)
		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString))
		End Try

	End Function

	<HttpGet>
	<Route("api/RequisitionNew/WOList")>
	Public Function WOList(Optional Text As String = "",
						   Optional No As Int32 = 0,
						   Optional FromDate As String = "",
						   Optional ToDate As String = "",
						   Optional RegNo As String = "",
						   Optional ModelName As String = "",
						   Optional StatusID As Integer = 0,
						   Optional WOStatusID As Integer = 0,
						   Optional AddTopItem As String = "",
						   Optional SerialNo As String = "",
						   Optional TransTypeID As Integer = 0,
						   Optional IsAllJobsCompletedButWONotCompletedListRequired As Boolean = False,
						   Optional IsForCAMOUpdate As Boolean = False,
						   Optional IsForBilling As Boolean = False,
						   Optional BillingRequired As Integer = -1,
						   Optional IsForQC As Boolean = False,
						   Optional IsCAMOUpdatedRequired As Integer = -1,
						   Optional ShowOnlyCompletedWOs As Boolean = False,
						   Optional AssemblyStatusID As String = "{00000000-0000-0000-0000-000000000000}",
						   Optional CustomerID As String = "{00000000-0000-0000-0000-000000000000}") As IHttpActionResult

		Dim mWOList As nWOList
		Try

			mWOList = nWOList.GetWOList(Text:=Text,
										No:=No,
										FromDate:=FromDate,
										ToDate:=ToDate,
										RegNo:=RegNo,
										ModelName:=ModelName,
										StatusID:=StatusID,
										WOStatusID:=WOStatusID,
										AddTopItem:=AddTopItem,
										SerialNo:=SerialNo,
										TransTypeID:=TransTypeID,
										IsAllJobsCompletedButWONotCompletedListRequired:=IsAllJobsCompletedButWONotCompletedListRequired,
										IsForCAMOUpdate:=IsForCAMOUpdate,
										IsForBilling:=IsForBilling,
										BillingRequired:=BillingRequired,
										IsForQC:=IsForQC,
										IsCAMOUpdatedRequired:=IsCAMOUpdatedRequired,
										ShowOnlyCompletedWOs:=ShowOnlyCompletedWOs,
										AssemblyStatusID:=AssemblyStatusID,
										CustomerID:=CustomerID)


			Return Ok(mWOList)

		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString))
		End Try

	End Function

	<HttpGet>
	<Route("api/RequisitionNew/RequisitionItemList")>
	Public Function RequisitionItemList(Optional PartNo As String = "",
										Optional IsReOrderLevelItemsRequired As Integer = 0,
										Optional IsBAReorderQtyFormulaRequired As Boolean = False,
										Optional CategoryID As String = "{00000000-0000-0000-0000-000000000000}",
										Optional Description As String = "",
										Optional IsPartAlternatePartStockConsideredTogether As Boolean = False,
										Optional SearchText As String = "") As IHttpActionResult

		Dim mRequisitionItemListNew As RequisitionItemListNew
		Try

			mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList(PartNo:=PartNo,
																					IsReOrderLevelItemsRequired:=IsReOrderLevelItemsRequired,
																					IsBAReorderQtyFormulaRequired:=IsBAReorderQtyFormulaRequired,
																					CategoryID:=CategoryID,
																					Description:=Description,
																					IsPartAlternatePartStockConsideredTogether:=IsPartAlternatePartStockConsideredTogether,
																					SearchText:=SearchText)
			Return Ok(mRequisitionItemListNew)

		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString))
		End Try

	End Function

	<HttpGet>
	<Route("api/RequisitionNew/RequisitionBranchList")>
	Public Function RequisitionBranchList(Optional TransTypeID As Integer = 0,
										  Optional IsTagRequired As Boolean = False,
										  Optional AddTopItem As String = "") As IHttpActionResult

		Dim _RequisitionEngineeringBranchesList As RequisitionEngineeringBranchesList
		Try

			_RequisitionEngineeringBranchesList = RequisitionEngineeringBranchesList.GetRequisitionEngineeringBranchesList(TransTypeID:=TransTypeID,
																														   IsTagRequired:=IsTagRequired,
																														   AddTopItem:=AddTopItem)
			Return Ok(_RequisitionEngineeringBranchesList)

		Catch ex As Exception
			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString))
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function SaveRequisition(<FromBody()> requestBody As JObject) As IHttpActionResult

		Dim ReturnMessage As ReturnMessage

		Try

			ReturnMessage = SetRequisitionDetails(requestBody:=requestBody)

			If ReturnMessage.Status = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Enquiry Saved Successfully!",
												   TransactionID:=ReturnMessage.TransactionID.ToString))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													  Message:=ReturnMessage.Message))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString))

		End Try

	End Function

	Private Function SetRequisitionDetails(requestBody As JObject) As ReturnMessage

		Dim _Requisition As RequisitionNew
		Dim SubscriptionMessage As String

		Dim RequisitionIsNew As Boolean = CBool(requestBody("mIsNew"))
		Dim DateFormatString As String = requestBody(propertyName:="mDate")("mFormat")

		Dim RequisitionItems As JArray = CType(requestBody("mRequisitionItems"), JArray)

		Dim ReqTypeID As Integer = If(requestBody(propertyName:="TransTypeID").Equals(Trans.StoresRequisition),
									   0,
									   If(requestBody(propertyName:="mVendorEnqNo"), 1, 2))

		Dim RequisitionEngineeringBrancheID As Integer
		If requestBody(propertyName:="TransTypeID").Equals(Trans.EngineeringRequisition) Or
		   requestBody(propertyName:="TransTypeID").Equals(Trans.WorkShopRequisition) Then

			RequisitionEngineeringBrancheID = requestBody(propertyName:="RequisitionEngineeringBrancheID")

		ElseIf requestBody(propertyName:="TransTypeID").Equals(Trans.PlanningRequisition) Then
			RequisitionEngineeringBrancheID = 4
		Else
			RequisitionEngineeringBrancheID = 0
		End If

		Try

			If RequisitionIsNew Then

				SubscriptionMessage = _CheckForSubscriptionExpired.
										CheckForSubscriptionExpired(TransactionDate:=CDate(requestBody(propertyName:="mDate").First.First),
																	ModuleName:="Requisition")

				If SubscriptionMessage <> "Success" Then

					Return New ReturnMessage(Status:="Error",
											 Message:=SubscriptionMessage)

				End If

				_Requisition = RequisitionNew.NewRequisition(TransTypeID:=CInt(requestBody("mTransTypeID")))

			Else
				_Requisition = RequisitionNew.GetRequisition(ID:=New Guid(requestBody("mID").ToString))
			End If

			With _Requisition

				.ReqDate = CDate(requestBody(propertyName:="mDate").First.First).ToString(format:=DateFormatString)
				.LocationID = New Guid(requestBody(propertyName:="mLocationID").ToString)
				.Text = requestBody(propertyName:="mText")
				.No = requestBody(propertyName:="mNo")
				.EmployeeName = requestBody(propertyName:="mEmployeeName").ToString
				.UserName = User.Identity.Name
				.ReqTypeID = ReqTypeID
				.RequisitionEngineeringBrancheID = RequisitionEngineeringBrancheID
				.RecommendedBy = Trim(requestBody(propertyName:="mRecommendedBy"))
				.Supervisor = Trim(requestBody(propertyName:="mSupervisor"))
				.WorkShopID = New Guid(requestBody(propertyName:="mWorkShopID").ToString)
				.IndentTypeID = CBool(requestBody(propertyName:="mIndentTypeID"))
				.Remark = CInt(requestBody("mRemark"))

			End With

			'************************* Requisition Items *************************
			If RequisitionItems.Count > 0 Then

				_Requisition = SetRequisitionItems(_Requisition:=_Requisition,
												   RequisitionItemsArray:=RequisitionItems,
												   RequisitionIsNew:=RequisitionIsNew)

			End If


			If _Requisition.RequisitionItemsNew.Count > 0 Then

				Dim ReturnMessage As String

				ReturnMessage = CheckEmployeeWorkingStatus()

				If ReturnMessage IsNot Nothing Then

					Return New ReturnMessage(Status:="Error",
											 Message:=ReturnMessage.Replace("<br />", ""))

				Else

					_Requisition.Save()

					Return New ReturnMessage(Status:="Success",
											 Message:="",
											 TransactionID:=_Requisition.ID.ToString)

				End If

			Else

				Return New ReturnMessage(Status:="Error",
										 Message:="Requisition can not be saved without Item.")

			End If

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Requisition",
																						   ex:=ex)

			Return New ReturnMessage(Status:="Error",
									 Message:=returnMessage)

		Catch ex As Exception

			Return New ReturnMessage(Status:="Error",
									 Message:=ex.GetBaseException.ToString)

		End Try

	End Function

	Private Function SetRequisitionItems(_Requisition As RequisitionNew,
										 RequisitionItemsArray As JArray,
										 RequisitionIsNew As Boolean) As RequisitionNew

		Try

			For i As Integer = 0 To RequisitionItemsArray.Count - 1

				Dim RequisitionItemsID As Guid = IIf(RequisitionIsNew,
													 Guid.Empty,
													 New Guid(RequisitionItemsArray(i)("mID").ToString))

				Dim RequisitionItemsIsNew As Boolean = CBool(RequisitionItemsArray(i)("mIsNew"))
				Dim RequisitionItemsIsDeleted As Boolean = CBool(RequisitionItemsArray(i)("mIsDeleted"))
				Dim RequisitionItemsIsDirty As Boolean = CBool(RequisitionItemsArray(i)("mIsDirty"))

				Dim RequisitionItemsDetails = RequisitionItemsArray(i)("mItemDetailForRequisition")
				Dim RequisitionItemID As New Guid(RequisitionItemsDetails("mItemID").ToString)
				Dim RequisitionItemName As String = RequisitionItemsDetails("mItemName").ToString
				Dim RequisitionItemDescription As String = RequisitionItemsDetails("mItemDescription").ToString

				Dim RequisitionItem As RequisitionItemNew

				If RequisitionIsNew Then

					_Requisition.RequisitionItemsNew.Add(RequisitionID:=_Requisition.ID,
														 WorkShopID:=_Requisition.WorkShopID)

					RequisitionItem = _Requisition.RequisitionItemsNew.CurrentItem

					GoTo SetRequisitionItemsData

				Else

					If RequisitionItemsIsNew Then

						_Requisition.RequisitionItemsNew.Add(RequisitionID:=_Requisition.ID,
															 WorkShopID:=_Requisition.WorkShopID)

						RequisitionItem = _Requisition.RequisitionItemsNew.CurrentItem

					Else
						RequisitionItem = _Requisition.RequisitionItemsNew(ID:=RequisitionItemsID)
					End If

					If RequisitionItemsIsDeleted Then
						_Requisition.RequisitionItemsNew.Remove(RequisitionItem)
					End If

				End If

				If RequisitionItemsIsNew Or RequisitionItemsIsDirty Then

SetRequisitionItemsData: With RequisitionItem

						.RequestedQty = CDec(RequisitionItemsArray(i)("mRequestedQty").ToString)

					End With

				End If

			Next

			Return _Requisition

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	<Route("api/RequisitionNew/DeleteRequisition")>
	Public Function DeleteRequisition(RequisitionID As String) As IHttpActionResult

		Try

			Dim _Requisition As RequisitionNew = RequisitionNew.GetRequisition(ID:=New Guid(RequisitionID))

			_Requisition.Delete()
			_Requisition.Save()

			Return Ok(New ReturnMessage("Success",
											   "Requisition Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Requisition",
																						   ex:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

#Region " Report(s) "

	<HttpPost>
	<Route("api/RequisitionNew/DisplayReport")>
	Public Function DisplayReport(<FromBody()> requestBody As JObject) As IHttpActionResult

		If requestBody Is Nothing Then

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage(Status:="Error",
												   Message:="Request body cannot be null."))

		End If

		Try

			Dim EmployeeName As String = requestBody("EmployeeName")
			Dim BranchName As String = requestBody("BranchName")
			Dim FormRevisionNo As String = requestBody("FormRevisionNo")
			Dim FormRevisionDate As String = requestBody("FormRevisionDate")
			Dim ID As String = CStr(requestBody("ID"))

			Dim result = _ReportHelper.RequisitionDetailReport(IsForAPI:=True,
															   ByMail:=False,
															   EmployeeName:=EmployeeName,
															   BranchName:=BranchName,
															   FormRevisionNo:=FormRevisionNo,
															   FormRevisionDate:=FormRevisionDate,
															   ID:=ID)
			If result.Item2.ToString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Report displayed Successfully!!",
												   ReportData:=result.Item1))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:="Error occurred while displaying report."))

			End If

		Catch ex As Exception
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

#End Region

#Region " Send Email "

	<HttpPost>
	<Route("api/RequisitionNew/SendEmail")>
	Public Function SendEmail(<FromBody()> requestBody As EmailRequest) As IHttpActionResult

		Try

			Dim TempReportPath As String = ""
			Dim User As User = UserManagerController.FetchUser()
			Dim UserName = User.Name

			Dim RequisitionID As String = requestBody.RequisitionID
			Dim AttachmentName As String = IIf(requestBody.AttachmentName IsNot Nothing, requestBody.AttachmentName, "")
			Dim TransTypeID As Integer = requestBody.TransTypeID
			Dim ToMailID As String = IIf(requestBody.ToMailID IsNot Nothing, requestBody.ToMailID, "")
			Dim CCMailID As String = IIf(requestBody.CCMailID IsNot Nothing, requestBody.CCMailID, "")
			Dim BCCMailID As String = IIf(requestBody.BCCMailID IsNot Nothing, requestBody.BCCMailID, "")
			Dim Remark As String = IIf(requestBody.Remark IsNot Nothing, requestBody.Remark, "")
			Dim ReportGeneratedBy As String = IIf(requestBody.ReportGeneratedBy IsNot Nothing, requestBody.ReportGeneratedBy, "")

			Dim UserEmailDetails As TransactionList = TransactionList.GetTransactionList()
			Dim SmtpHost = UserEmailDetails.Item(TransTypeID).SmtpHost
			Dim SmtpPort = UserEmailDetails.Item(TransTypeID).SmtpPort
			Dim SmtpUser = UserEmailDetails.Item(TransTypeID).SmtpUser
			Dim SmtpPassword = UserEmailDetails.Item(TransTypeID).SmtpPassword
			Dim FormRevisionNo = UserEmailDetails.Item(TransTypeID).FormRevisionNo
			Dim FormRevisionDate = UserEmailDetails.Item(TransTypeID).FormRevisionDate

			Dim result = _ReportHelper.RequisitionDetailReport(IsForAPI:=True,
															   ByMail:=True,
															   ID:=RequisitionID)

			If result.Item1.ToString = "Success" Then

				TempReportPath = _EmailHelper.SaveReportToTempFile(ReportBytes:=result.Item3,
																   AttachmentName:=AttachmentName)

			End If

			Dim Info As String = IIf(result.Item2 Is Nothing, "", CStr(result.Item2))

			SendMailFile.SendMailFile(rpt:=Nothing,
									  UserName:=UserName,
									  Subject:="Requisition Details",
									  Text:=AttachmentName,
									  Info:=Info,
									  ToMailID:=ToMailID,
									  CCMailID:=CCMailID,
									  ReportPath:=TempReportPath,
									  BCCMailID:=BCCMailID,
									  Remark:=Remark,
									  ReportGeneratedBy:=ReportGeneratedBy,
									  SmtpHost:=SmtpHost,
									  SmtpPort:=SmtpPort,
									  SmtpUser:=SmtpUser,
									  SmtpPassword:=SmtpPassword,
									  TransTypeID:=TransTypeID)

			Return Ok(New ReturnMessage("Success", "Email Sent Successfully!"))

		Catch ex As Exception
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

#End Region

#Region " Helper Method(s) "

	Private Function CheckEmployeeWorkingStatus() As String

		Dim EmployeeStatus As EmployeeStatus
		Dim EmployeeStatusMessage As String
		Try

			If EmployeeStatus(0).Information <> "" Then

				EmployeeStatusMessage = EmployeeStatus(0).Information

			End If

			Return EmployeeStatusMessage

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class
