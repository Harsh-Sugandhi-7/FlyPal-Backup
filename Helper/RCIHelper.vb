'************************************
'Created by:	Harsh Sugandhi
'Created on:	16th October 2025
'Created for:	To handle the Common methods required in RCI.
'************************************


Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web.Script.Serialization

Imports Newtonsoft.Json.Linq


Public Class RCIHelper

#Region " Variable(s) Declaration "

	Private _EmailHelper As New EmailHelper
	Private _ReportHelper As New ReportHelper
	Private _CommonMethod As New CommonMethods
	Private _AuthorizationHelper As New AuthorizationHelper

	Private CellStyle = "text-align: left; padding: 4px;"
	Private UserName As String = Thread.CurrentPrincipal.Identity.Name
	Private Style = "font-family: Calibri, Arial, sans-serif; font-size: 11pt;"

#End Region

#Region " Helper Method(s) "

	Public Function CheckForCalibratedAndEquipmentMaintenanceItems(StatusID As Integer,
																   TransTypeID As Integer,
																   ReceiptCumInvoiceItems As JArray) As ReturnMessage

		Dim CalibratedItemMessage As String = String.Empty
		Dim EquipmentMaintenancePartMessage As String = String.Empty
		Dim returnMessage As New StringBuilder
		Try

			If StatusID <> 2 OrElse TransTypeID <> 10 Then
				Return New ReturnMessage(Status:="Success", Message:="No Calibrated & Equipment-Maintenance Item(s) to Comply.")
			End If

			For i As Integer = 0 To ReceiptCumInvoiceItems.Count - 1

				Dim format As DateTime
				Dim CalibrationDoneOnDateString = ReceiptCumInvoiceItems(i)("mReceiptItem")("mCalibrationDoneOnDate")("mDate").ToString
				Dim CalibrationDoneOnDate As String = If(CalibrationDoneOnDateString, "")
				Dim IsCalibrationDoneOnDateNULL As Boolean = String.IsNullOrEmpty(CalibrationDoneOnDate) OrElse
															 Not DateTime.TryParse(CalibrationDoneOnDate, format) OrElse
															 format = DateTime.MinValue

				If Not IsCalibrationDoneOnDateNULL Then
					HttpContext.Current.Session("ShowedMSGForCalibration") = "Showed MSG For Calibration"
					CalibratedItemMessage = $"Receipt contains Calibrated Items.{Environment.NewLine} Do you wish to Comply ?"
				End If

				Dim ReceiptItemServiceInspections As JArray = CType(ReceiptCumInvoiceItems(i)("mReceiptItem")("mReceiptItemServiceInspections"), JArray)

				If ReceiptItemServiceInspections.Count > 0 Then

					For j As Integer = 0 To ReceiptItemServiceInspections.Count - 1

						Dim parsed As DateTime
						Dim ServicedInspectedCheckDoneOnDateString = ReceiptItemServiceInspections(j)("mServiedInspectedCheckDoneOnDate")("mDate").ToString
						Dim ServicedInspectedCheckDoneOnDate As String = If(ServicedInspectedCheckDoneOnDateString, "")
						Dim IsServicedInspectedCheckDoneOnDateNULL As Boolean = String.IsNullOrEmpty(ServicedInspectedCheckDoneOnDate) OrElse
																				Not DateTime.TryParse(ServicedInspectedCheckDoneOnDate, parsed) OrElse
																				parsed = DateTime.MinValue

						If Not IsServicedInspectedCheckDoneOnDateNULL Then
							EquipmentMaintenancePartMessage = $"Receipt contains Equipment Maintenance Parts.{Environment.NewLine} Do you wish to Comply ?"
							HttpContext.Current.Session("ShowedMSGForConditionCheck") = "Showed MSG For Condition Check"
						End If

					Next

				End If

			Next

			If Len(CalibratedItemMessage) > 0 AndAlso Len(EquipmentMaintenancePartMessage) > 0 Then
				Return New ReturnMessage(Status:="Success", Message:=$"Receipt contains Calibrated Items & Equipment Maintenance Parts.{Environment.NewLine} Do you wish to Comply ?")
			ElseIf Len(CalibratedItemMessage) > 0 AndAlso Len(EquipmentMaintenancePartMessage) = 0 Then
				Return New ReturnMessage(Status:="Success", Message:=$"{returnMessage.AppendLine(CalibratedItemMessage)}{Environment.NewLine}")
			ElseIf Len(CalibratedItemMessage) = 0 AndAlso Len(EquipmentMaintenancePartMessage) > 0 Then
				Return New ReturnMessage(Status:="Success", Message:=$"{returnMessage.AppendLine(EquipmentMaintenancePartMessage)}{Environment.NewLine}")
			Else
				Return New ReturnMessage(Status:="Success", Message:="No Calibrated & Equipment-Maintenance Item(s) to Comply.")
			End If

		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception", Message:=$"{ex.GetBaseException}")
		End Try

	End Function

	Public Function ComplyCalibratedItems(ReceiptCumInvoiceItems As ReceiptCumInvoiceItems) As StringBuilder

		Dim CalibrationItemChildList As CalibrationItemChildList
		Dim OldCalibrationItemChild As CalibrationItemChild
		Dim CalibrationItem As CalibrationItem
		Dim CalibrationItemChild As CalibrationItemChild
		Dim compliedItems As New StringBuilder
		Dim returnMessage As New StringBuilder

		Try

			For Each Item As ReceiptCumInvoiceItem In ReceiptCumInvoiceItems

				If Not IsDBNull(Item.CalibrationDoneOnDate) Then

					CalibrationItemChildList = CalibrationItemChildList.GetCalibrationChildList(FromDate:="1/1/1900",
																								ToDate:="1/1/3300",
																								ItemName:=Item.ItemName,
																								Description:=Item.ItemDescription,
																								SerialNo:=Item.SerialNo)

					If CalibrationItemChildList.Count > 0 Then

						CalibrationItem = CalibrationItem.GetCalibrationItem(ID:=CalibrationItemChildList(0).CalibrationItemID)
						OldCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(CalibrationItemID:=CalibrationItemChildList(0).ID)

						If OldCalibrationItemChild.IsApplicable Then

							If CDate(OldCalibrationItemChild.DoneOnDate) < CDate(Item.CalibrationDoneOnDate) Then

								CalibrationItemChild = CalibrationItemChild.NewComplyCalibrationItemChild(CalibrationItemID:=CalibrationItem.ID,
																										  CalDoneOnDate:=Item.CalibrationDoneOnDate.ToString,
																										  PreviousCalibrationItemChildID:=OldCalibrationItemChild.ID)

								CalibrationItemChild.ItemName = OldCalibrationItemChild.ItemName
								CalibrationItemChild.Description = OldCalibrationItemChild.Description
								CalibrationItemChild.SerialNo = OldCalibrationItemChild.SerialNo
								CalibrationItemChild.Frequency = OldCalibrationItemChild.CalibrationItemChildFrequency
								CalibrationItemChild.CalibrationPeriodInID = OldCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID
								CalibrationItemChild.CalibrationItemChildFrequency = OldCalibrationItemChild.CalibrationItemChildFrequency
								CalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = OldCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID
								CalibrationItemChild.DoneOnDate = Item.CalibrationDoneOnDate
								CalibrationItemChild.Location = OldCalibrationItemChild.Location

								If CalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 1 Then
									CalibrationItemChild.NextDueDate = CDate(Item.CalibrationDoneOnDate).AddDays(OldCalibrationItemChild.CalibrationItemChildFrequency)
								ElseIf CalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 2 Then
									CalibrationItemChild.NextDueDate = CDate(Item.CalibrationDoneOnDate).AddMonths(OldCalibrationItemChild.CalibrationItemChildFrequency)
								ElseIf CalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 3 Then
									CalibrationItemChild.NextDueDate = CDate(Item.CalibrationDoneOnDate).AddYears(OldCalibrationItemChild.CalibrationItemChildFrequency)
								End If

								compliedItems.Append($"Part No. : {CalibrationItemChild.ItemName } Serial No. : {CalibrationItemChild.SerialNo}")
								CalibrationItemChild = CalibrationItemChild.Save()

							End If

						End If

					End If

				End If

			Next

			If compliedItems.Length > 0 Then
				returnMessage.Append($"Following Item(s) Complied Successfully! {Environment.NewLine}")
				returnMessage.Append($"{compliedItems}")
				Return returnMessage
			End If

			returnMessage.Append($"Item(s) has already been Complied.")
			Return returnMessage

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function ComplyConditionCheckItems(ReceiptCumInvoiceItems As ReceiptCumInvoiceItems) As StringBuilder

		Dim ConditionCheckItemChildList As ConditionCheckItemChildList
		Dim OldConditionCheckItemChild As ConditionCheckItemChild
		Dim ConditionCheckItem As ConditionCheckItem
		Dim ConditionCheckItemChild As ConditionCheckItemChild
		Dim compliedConditionCheckItems As New StringBuilder
		Dim returnMessage As New StringBuilder
		Try

			For Each Item As ReceiptCumInvoiceItem In ReceiptCumInvoiceItems

				For Each Inspection As ReceiptItemServiceInspection In Item.ReceiptItem.ReceiptItemServiceInspections

					If Not IsDBNull(Inspection.ServiedInspectedCheckDoneOnDate) Then

						ConditionCheckItemChildList = ConditionCheckItemChildList.GetConditionCheckItemChildList(FromDate:="1/1/1900",
																												 ToDate:="1/1/3300",
																												 ItemName:=Item.ItemName,
																												 Description:=Item.ItemDescription,
																												 SerialNo:=Item.SerialNo,
																												 ItemServiceInspectionsID:=Inspection.ItemServiceInspectionsID.ToString)

						If ConditionCheckItemChildList.Count > 0 Then

							ConditionCheckItem = ConditionCheckItem.GetConditionCheckItem(ID:=ConditionCheckItemChildList(0).ConditionCheckItemID)
							OldConditionCheckItemChild = ConditionCheckItemChild.GetConditionCheckItemChild(ConditionCheckItemID:=ConditionCheckItemChildList(0).ID)

							If OldConditionCheckItemChild.IsApplicable = True Then

								If CDate(OldConditionCheckItemChild.DoneOnDate) < CDate(Inspection.ServiedInspectedCheckDoneOnDate) Then

									ConditionCheckItemChild = ConditionCheckItemChild.NewComplyConditionCheckItemChild(ConditionCheckItemID:=ConditionCheckItem.ID,
																												   DoneOnDate:=New SmartDate(Inspection.ServiedInspectedCheckDoneOnDate.ToString, False),
																												   PreviousConditionCheckItemChildID:=OldConditionCheckItemChild.ID)

									ConditionCheckItemChild.ItemName = OldConditionCheckItemChild.ItemName
									ConditionCheckItemChild.Description = OldConditionCheckItemChild.Description
									ConditionCheckItemChild.SerialNo = OldConditionCheckItemChild.SerialNo
									ConditionCheckItemChild.Frequency = OldConditionCheckItemChild.Frequency
									ConditionCheckItemChild.ConditionCheckIntervalIn = OldConditionCheckItemChild.ConditionCheckIntervalIn
									ConditionCheckItemChild.DoneOnDate = Inspection.ServiedInspectedCheckDoneOnDate
									ConditionCheckItemChild.Location = OldConditionCheckItemChild.Location

									If Inspection.ItemServiceInspectionFrequencyPeriod = 1 Then
										ConditionCheckItemChild.NextDueDate = CDate(Inspection.ServiedInspectedCheckDoneOnDate).AddDays(OldConditionCheckItemChild.Frequency)
									ElseIf Inspection.ItemServiceInspectionFrequencyPeriod = 2 Then
										ConditionCheckItemChild.NextDueDate = CDate(Inspection.ServiedInspectedCheckDoneOnDate).AddMonths(OldConditionCheckItemChild.Frequency)
									ElseIf Inspection.ItemServiceInspectionFrequencyPeriod = 3 Then
										ConditionCheckItemChild.NextDueDate = CDate(Inspection.ServiedInspectedCheckDoneOnDate).AddYears(OldConditionCheckItemChild.Frequency)
									End If

									compliedConditionCheckItems.Append($"Part No. : {ConditionCheckItemChild.ItemName} Serial No. : {ConditionCheckItemChild.SerialNo} Description : {Inspection.ItemServiceInspectionDescription}")
									ConditionCheckItemChild = ConditionCheckItemChild.Save()

								End If

							End If

						End If

					End If

				Next

			Next

			If compliedConditionCheckItems.Length > 0 Then
				returnMessage.Append($"Following Item(s) Complied Successfully! {Environment.NewLine}")
				returnMessage.Append($"{compliedConditionCheckItems}")
				Return returnMessage
			End If

			returnMessage.Append($"Item(s) has already been Complied.")
			Return returnMessage

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function CheckDuplicateSerialNo(currentItem As ReceiptItemsDTO,
										   existingItems As List(Of ReceiptItemsDTO),
										   Optional clientCode As String = "") As Boolean

		Try

			For Each prev As ReceiptItemsDTO In existingItems

				If prev.ID = currentItem.ID Then Return True

				If currentItem.FromItemTypeID = 3 Then

					If currentItem.IsSerialized Then

						If (prev.SerialNo = currentItem.SerialNo) AndAlso
						   ((prev.OrderItemID = currentItem.OrderItemID AndAlso currentItem.OrderItemID <> Guid.Empty) _
							OrElse (prev.ItemID = currentItem.ItemID AndAlso currentItem.OrderItemID <> Guid.Empty)) Then
							Return True
						End If

					Else

						If (prev.ReleaseNoteNo = currentItem.ReleaseNoteNo) AndAlso
						   (prev.ExpiryDate = currentItem.ExpiryDate) AndAlso
						   (prev.BatchNo = currentItem.BatchNo) AndAlso
						   (prev.OrderItemID = currentItem.OrderItemID AndAlso currentItem.OrderItemID <> Guid.Empty) Then
							Return True
						End If

					End If

				ElseIf currentItem.FromItemTypeID = 4 Then

					If currentItem.IsSerialized Then

						If (prev.SerialNo = currentItem.SerialNo) AndAlso
						   ((prev.IssueItemID = currentItem.IssueItemID AndAlso currentItem.IssueItemID <> Guid.Empty) _
							OrElse (prev.ItemID = currentItem.ItemID AndAlso currentItem.IssueItemID <> Guid.Empty)) Then
							Return True
						End If

					Else

						If (prev.IssueItemID = currentItem.IssueItemID AndAlso currentItem.IssueItemID <> Guid.Empty) Then
							Return True
						End If

					End If

				ElseIf {12, 16, 17}.Contains(currentItem.FromItemTypeID) Then

					If currentItem.IsSerialized Then

						If (prev.SerialNo = currentItem.SerialNo) AndAlso (prev.ItemID = currentItem.ItemID) Then
							Return True
						End If

					End If

					If clientCode = "IRM" AndAlso currentItem.FromItemTypeID = 12 AndAlso Not currentItem.IsSerialized Then

						If (prev.ReleaseNoteNo = currentItem.ReleaseNoteNo) AndAlso
						   (prev.ExpiryDate = currentItem.ExpiryDate) AndAlso
						   (prev.BatchNo = currentItem.BatchNo) Then

							Return True

						End If

					End If

				End If

			Next

			Return False

		Catch ex As Exception
			Return False
		End Try

	End Function

	Public Function CheckIfReceiptQuantityExceedsOrderQuantity(ReceiptCumInvoice As ReceiptCumInvoice) As Boolean

		Dim InvoiceItem
		Try

			Dim InvoiceItems = From ReceiptCumInvoiceItem In ReceiptCumInvoice.ReceiptCumInvoiceItems
							   Group ReceiptCumInvoiceItem By ReceiptCumInvoiceItem.OrderItemID Into Group
							   Select New With {
													OrderItemID,
													.TotalDisplayQty = Group.Sum(Function(x) x.DisplayQty),
													.FirstItemID = Group.First().ID,
													.Items = Group.ToList()
												}


			For Each InvoiceItem In InvoiceItems

				' Get Order Item Detail
				Dim OrderItemDetailForReceipt As OrderItemDetailForReceipt =
				OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(OrderItemID:=InvoiceItem.OrderItemID)

				Dim TotalReceiptCount As Decimal = Order.GetTotalReceiptCountAgainstOrderItem(OrderItemID:=InvoiceItem.OrderItemID,
																							  SkipRecItemID:=InvoiceItem.FirstItemID.ToString,
																							  SkipReceiptID:=ReceiptCumInvoice.ID.ToString)

				' Get already saved Receipt Quantity (Excluding Current batch)
				Dim TotalReceiptQuantity As Decimal = 0

				If TotalReceiptCount = 0 Then
					TotalReceiptQuantity = Order.GetTotalReceiptQtyAgainstOrderItem(OrderItemID:=InvoiceItem.OrderItemID,
																					SkipRecItemID:=InvoiceItem.FirstItemID.ToString,
																					SkipReceiptID:=ReceiptCumInvoice.ID.ToString)
				Else
					TotalReceiptQuantity = Order.GetTotalReceiptQtyAgainstOrderItem(OrderItemID:=InvoiceItem.OrderItemID,
																					SkipRecItemID:=InvoiceItem.FirstItemID.ToString)

				End If

				If InvoiceItem.TotalDisplayQty > OrderItemDetailForReceipt.Qty Then
					Return True     ' Case 1: Current Receipt Exceeds Order Quantity
				ElseIf (TotalReceiptQuantity + InvoiceItem.TotalDisplayQty) > OrderItemDetailForReceipt.Qty Then
					Return True     ' Case 2: Total Receipt Exceeds Order Quantity
				Else
					Return False    ' Case 3: Within Limit, Safe to Save
				End If

			Next

			Return False

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function UpdateOrderQuantity(ReceiptCumInvoice As ReceiptCumInvoice) As Boolean

		Dim InvoiceItem
		Dim Order As Order
		Dim Vendor As Vendor
		Dim GSTPercentage As GSTPercentage
		Dim OrderQuantityUpdated As Boolean = False
		Try

			Dim InvoiceItems = From ReceiptCumInvoiceItem In ReceiptCumInvoice.ReceiptCumInvoiceItems
							   Group ReceiptCumInvoiceItem By ReceiptCumInvoiceItem.OrderItemID Into Group
							   Select New With {
													OrderItemID,
													.TotalDisplayQty = Group.Sum(Function(x) x.DisplayQty),
													.FirstItemID = Group.First().ID,
													.Items = Group.ToList()
												}

			For Each InvoiceItem In InvoiceItems

				Dim TotalReceiptQuantity As Decimal
				Dim OrderItemDetailForReceipt As OrderItemDetailForReceipt =
					OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(InvoiceItem.OrderItemID)

				Order = Order.GetOrder(ID:=OrderItemDetailForReceipt.OrderID)
				Dim receiptItemChild

				For Each receiptItemChild In InvoiceItem.Items

					TotalReceiptQuantity = Order.GetTotalReceiptQtyAgainstOrderItem(OrderItemID:=InvoiceItem.OrderItemID,
																					SkipRecItemID:=receiptItemChild.ID.ToString)

				Next

				If InvoiceItem.TotalDisplayQty > Order.OrderItems(InvoiceItem.OrderItemID).Qty - TotalReceiptQuantity Then

					Dim OldOrderItemQuantity As Decimal = Order.OrderItems(InvoiceItem.OrderItemID).Qty
					Dim NewOrderItemQuantity As Decimal = OldOrderItemQuantity + (InvoiceItem.TotalDisplayQty - (Order.OrderItems(InvoiceItem.OrderItemID).Qty - TotalReceiptQuantity))

					Order.OrderItems(InvoiceItem.OrderItemID).Qty = NewOrderItemQuantity

					If Order.TransTypeID = 5 And Order.AgainstTypeID = 7 Then
						Order.OrderItems(InvoiceItem.OrderItemID).OrderItemQuotationItems(0).Qty = NewOrderItemQuantity
					End If

					Order.OrderItems(InvoiceItem.OrderItemID).Note = $"Order Item quantity updated to {NewOrderItemQuantity} from {OldOrderItemQuantity} by automatic process from Goods Receipt."
					receiptItemChild.ExcessQty = NewOrderItemQuantity - OldOrderItemQuantity

					If CBool(AppSettings("IsGSTApplicable")) Then

						Dim ItemData As ItemByID = ItemByID.GetItemByID(ID:=Order.OrderItems(InvoiceItem.OrderItemID).ItemID)
						Vendor = Vendor.GetVendor(ID:=Order.VendorID)

						If Vendor.ClientCountryName.Equals("INDIA", StringComparison.InvariantCultureIgnoreCase) Then

							If Vendor.CountryName.Equals("INDIA", StringComparison.InvariantCultureIgnoreCase) And
							   Order.OrderDate >= CDate("01-Jul-2017") Then

								GSTPercentage = GSTPercentage.GetPercentage(TransactionDate:=Order.OrderDate,
																			Type:=1,
																			ItemID:=Order.OrderItems(InvoiceItem.OrderItemID).ItemID.ToString)

								If GSTPercentage IsNot Nothing Then

									If Len(Vendor.StateCode) > 0 Then

										If Vendor.StateCode = Vendor.ClientStateCode Then

											Order.OrderItems(InvoiceItem.OrderItemID).CGSTCAmount = ((Order.OrderItems(InvoiceItem.OrderItemID).CGSTPercentage * Order.OrderItems(InvoiceItem.OrderItemID).CRate * Order.OrderItems(InvoiceItem.OrderItemID).Qty) / 100)
											Order.OrderItems(InvoiceItem.OrderItemID).SGSTCAmount = ((Order.OrderItems(InvoiceItem.OrderItemID).SGSTPercentage * Order.OrderItems(InvoiceItem.OrderItemID).CRate * Order.OrderItems(InvoiceItem.OrderItemID).Qty) / 100)
											Order.OrderItems(InvoiceItem.OrderItemID).TotalCAmount = (Order.OrderItems(InvoiceItem.OrderItemID).CRate * Order.OrderItems(InvoiceItem.OrderItemID).Qty) + Order.OrderItems(InvoiceItem.OrderItemID).CGSTCAmount + Order.OrderItems(InvoiceItem.OrderItemID).SGSTCAmount
											Order.OrderItems(InvoiceItem.OrderItemID).IGSTPercentage = 0
											Order.OrderItems(InvoiceItem.OrderItemID).IGSTCAmount = 0

										Else

											Order.OrderItems(InvoiceItem.OrderItemID).IGSTCAmount = ((Order.OrderItems(InvoiceItem.OrderItemID).IGSTPercentage * Order.OrderItems(InvoiceItem.OrderItemID).CRate * Order.OrderItems(InvoiceItem.OrderItemID).Qty) / 100)
											Order.OrderItems(InvoiceItem.OrderItemID).CGSTCAmount = 0
											Order.OrderItems(InvoiceItem.OrderItemID).SGSTCAmount = 0
											Order.OrderItems(InvoiceItem.OrderItemID).TotalCAmount = (Order.OrderItems(InvoiceItem.OrderItemID).CRate * Order.OrderItems(InvoiceItem.OrderItemID).Qty) + Order.OrderItems(InvoiceItem.OrderItemID).IGSTCAmount

										End If

									Else

										Order.OrderItems(InvoiceItem.OrderItemID).CGSTCAmount = 0
										Order.OrderItems(InvoiceItem.OrderItemID).SGSTCAmount = 0
										Order.OrderItems(InvoiceItem.OrderItemID).IGSTCAmount = 0
										Order.OrderItems(InvoiceItem.OrderItemID).TotalCAmount = 0

									End If

								End If

							Else

								Order.OrderItems(InvoiceItem.OrderItemID).CGSTCAmount = 0
								Order.OrderItems(InvoiceItem.OrderItemID).SGSTCAmount = 0
								Order.OrderItems(InvoiceItem.OrderItemID).IGSTCAmount = 0
								Order.OrderItems(InvoiceItem.OrderItemID).TotalCAmount = 0

							End If

						Else

							Order.OrderItems(InvoiceItem.OrderItemID).CGSTCAmount = 0
							Order.OrderItems(InvoiceItem.OrderItemID).SGSTCAmount = 0
							Order.OrderItems(InvoiceItem.OrderItemID).IGSTCAmount = 0
							Order.OrderItems(InvoiceItem.OrderItemID).TotalCAmount = 0

						End If

					End If

					Order.CalculateTotal()
					Order.Save()
					OrderQuantityUpdated = True

					MarkLog(Action.Save,
							"Order",
							$"Order Quantity updated by {Thread.CurrentPrincipal.Identity.Name} on {Today.Date}",
							ErrorType.NoError,
							Order.ID,
							EventLogID)

				End If

			Next

			Return OrderQuantityUpdated

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Notification(s) After Authorization "

	Public Function SendNotificationsAfterAuthorization(_ReceiptCumInvoice As ReceiptCumInvoice)

		Dim From As String
		Dim RCIDetail As String
		Try

			Select Case _ReceiptCumInvoice.FromTypeID
				Case 14  'Vendor
					From = _ReceiptCumInvoice.VendorName
				Case 2   'Aircraft
					From = _ReceiptCumInvoice.RegNo
				Case 8   'Store
					From = _ReceiptCumInvoice.StoreName
				Case 16  'WorkShop
					From = _ReceiptCumInvoice.WorkShopName
				Case 17  'WorkOrder                                
					From = _ReceiptCumInvoice.WONumber
			End Select

			If (
					Not _ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.AlternateItemID.Equals(Guid.Empty) And
					(
						_ReceiptCumInvoice.TransTypeID = 7 Or
						_ReceiptCumInvoice.TransTypeID = 10 Or
						_ReceiptCumInvoice.TransTypeID = 54
					)
				) Then
				RCIDetail = $"{_ReceiptCumInvoice.ReceiptNo} Dated : {_ReceiptCumInvoice.RecCumInvDateFormatted} from {From} Note:- Order Part Is amended As alternate part Is received."
			Else
				RCIDetail = $"{_ReceiptCumInvoice.ReceiptNo} Dated : {_ReceiptCumInvoice.RecCumInvDateFormatted} from {From}"
			End If

			If _ReceiptCumInvoice.TransTypeID = 7 Or _ReceiptCumInvoice.TransTypeID = 10 Then
				MailIfAlternatePartReceived(_ReceiptCumInvoice:=_ReceiptCumInvoice)
			End If

			MailForRequestedPartsReceived(_ReceiptCumInvoice:=_ReceiptCumInvoice)
			MailForAuthorization(_ReceiptCumInvoice:=_ReceiptCumInvoice)

			If Not _ReceiptCumInvoice.ReceiptCumInvoiceItems(0).ReqEmployeeID.Equals(Guid.Empty) Then
				PUSHNotification(_ReceiptCumInvoice:=_ReceiptCumInvoice)
			End If

		Catch ex As Exception

			Dim errorMsg = $"Error in 'SendNotificationsAfterAuthorization': 
						   ReceiptNo='{If(_ReceiptCumInvoice?.ReceiptNo, "N / A")}', 
						   Original Exception: {ex.GetType().Name} - {ex.Message}"

			Throw New Exception(errorMsg, ex.GetBaseException())

		End Try

	End Function

	Private Function MailIfAlternatePartReceived(_ReceiptCumInvoice As ReceiptCumInvoice)

		Dim EmailBody As String = ""
		Dim AlternateParts As List(Of ReceiptCumInvoiceItem)
		Try

			If CBool(AppSettings("MailsRequire")) Then

				If _AuthorizationHelper.IsBTPLUser(UserName:=UserName) Then Exit Function

				If _ReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then

					AlternateParts = (
										From ReceiptCumInvoiceItem As ReceiptCumInvoiceItem In _ReceiptCumInvoice.ReceiptCumInvoiceItems
										Where Not ReceiptCumInvoiceItem.AlternateItemID.Equals(Guid.Empty)
										Select ReceiptCumInvoiceItem
									 ).ToList

				End If

				If AlternateParts.Count > 0 Then

					EmailBody = GenerateReportBodyForAlternatePartReceived(_ReceiptCumInvoice:=_ReceiptCumInvoice,
																			AlternateParts:=AlternateParts)

					_EmailHelper.SendEmail(Info:=EmailBody,
										   UserName:=UserName,
										   ToMailID:="",
										   Text:=_ReceiptCumInvoice.ReceiptNo,
										   Subject:="Alternate Part(s) Received.",
										   TransTypeID:=_ReceiptCumInvoice.TransTypeID)

				End If

			End If

		Catch ex As Exception

			Dim errorMsg = $"Error in 'SendMailIfAlternatePartReceived': 
						   ReceiptNo='{If(_ReceiptCumInvoice?.ReceiptNo, "N / A")}', 
						   AlternatePartsCount={If(AlternateParts?.Count, 0)} {vbCrLf}
						   Original Exception: {ex.GetType().Name} - {ex.Message}"

			Throw New Exception(errorMsg, ex.GetBaseException())

		End Try

	End Function

	Private Function GenerateReportBodyForAlternatePartReceived(_ReceiptCumInvoice As ReceiptCumInvoice,
																AlternateParts As List(Of ReceiptCumInvoiceItem)) As String

		Try

			Dim userName = Thread.CurrentPrincipal?.Identity?.Name Or "Unknown"
			Dim formattedDate = New SmartDate(Date.Today).FormattedText

			Dim header = $"<p><font face=""Calibri"">Following Alternate Part(s) received in <b>{_ReceiptCumInvoice.ReceiptNo}</b> Dated <b>{_ReceiptCumInvoice.RecCumInvDateFormatted}</b></font></p>" &
						 $"<p><font face=""Calibri"">by User : <b>{userName}</b> Last Modified Dated : <b>{formattedDate}</b></font></p>"

			Dim tableHeader = "<TABLE BORDER=1 CELLSPACING=0 CELLPADDING=0 ID=""Table2"">" &
							  "<tr><td align=""left""><font face=""Calibri""><b>Sr. No.</b></font></td>" &
							  "<td align=""left""><font face=""Calibri""><b>Ordered Part #</b></font></td>" &
							  "<td align=""left""><font face=""Calibri""><b>Received Part #</b></font></td></tr>"

			Dim rows = String.Join(separator:="",
								   values:=AlternateParts.Select(selector:=Function(item, index) $"<tr>" &
																	$"<td align=""left""><font face=""Calibri"">{index + 1}</font></td>" &
																	$"<td align=""left""><font face=""Calibri"">{item.OrderItemDetailForReceipt.ItemName}</font></td>" &
																	$"<td align=""left""><font face=""Calibri"">{item.ItemName}</font></td>" &
																	$"</tr>"
																 ))

			Dim tableFooter = "</TABLE>"

			Return $"{header}{tableHeader}{rows}{tableFooter}"

		Catch ex As Exception

			Dim errorMsg = $"Error in 'GenerateReportBodyForAlternatePartReceived': 
						   ReceiptNo='{If(_ReceiptCumInvoice?.ReceiptNo, "N / A")}', 
						   AlternatePartsCount={If(AlternateParts?.Count, 0)} {vbCrLf}
						   Original Exception: {ex.GetType().Name} - {ex.Message}"

			Throw New Exception(errorMsg, ex.GetBaseException())

		End Try

	End Function

	Private Function MailForRequestedPartsReceived(_ReceiptCumInvoice As ReceiptCumInvoice)

		Dim EmailBody As String = ""
		Dim ReceiptCumInvoiceItems As List(Of ReceiptCumInvoiceItem)
		Try

			If CBool(AppSettings("MailsRequire")) Then

				If _AuthorizationHelper.IsBTPLUser(UserName:=UserName) Then Exit Function

				If _ReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then

					ReceiptCumInvoiceItems = (
												From ReceiptCumInvoiceItem As ReceiptCumInvoiceItem In _ReceiptCumInvoice.ReceiptCumInvoiceItems
												Where Not ReceiptCumInvoiceItem.ReqEmployeeEmailIDs = ""
												Select ReceiptCumInvoiceItem
											 ).ToList

				End If

				If ReceiptCumInvoiceItems.Count > 0 Then

					Dim EmailIDs As New StringBuilder

					For i As Integer = 0 To ReceiptCumInvoiceItems.Count - 1

						If Not EmailIDs.ToString.Contains(ReceiptCumInvoiceItems(i).ReqEmployeeEmailIDs) Then
							EmailIDs.Append($"{ReceiptCumInvoiceItems(i).ReqEmployeeEmailIDs},")
						End If

					Next

					EmailBody = GenerateReportBodyForRequestedPartsReceived(_ReceiptCumInvoice:=_ReceiptCumInvoice,
																			ReceiptCumInvoiceItems:=ReceiptCumInvoiceItems)

					_EmailHelper.SendEmail(Info:=EmailBody,
										   UserName:=UserName,
										   ToMailID:="",
										   Text:=_ReceiptCumInvoice.ReceiptNo,
										   Subject:="Requested Part(s) Received.",
										   TransTypeID:=_ReceiptCumInvoice.TransTypeID)

				End If

			End If

		Catch ex As Exception

			Dim errorMsg = $"Error in 'SendMailForRequestedPartsReceived': 
						     ReceiptNo='{If(_ReceiptCumInvoice?.ReceiptNo, "N / A")}', 
						     Original Exception: {ex.GetType().Name} - {ex.Message}"

			Throw New Exception(errorMsg, ex.GetBaseException())

		End Try

	End Function

	Private Function GenerateReportBodyForRequestedPartsReceived(_ReceiptCumInvoice As ReceiptCumInvoice,
																 ReceiptCumInvoiceItems As List(Of ReceiptCumInvoiceItem)) As String

		Try

			Dim userName = Thread.CurrentPrincipal?.Identity?.Name Or "Unknown"
			Dim receiptNo = _ReceiptCumInvoice?.ReceiptNo Or "N/A"
			Dim receiptDate = _ReceiptCumInvoice?.RecCumInvDateFormatted Or "N/A"

			Dim header = $"<p style=""{Style}"">Following Requested Part(s) received in <b>{receiptNo}</b> Dated <b>{receiptDate}</b></p>" &
						 $"<p style=""{Style}"">by User: <b>{userName}</b></p>"

			Dim tableHeader = $"<table border=""1"" cellspacing=""0"" cellpadding=""4"" " &
							  $"style=""border-collapse: collapse; {Style}"">" &
							  $"<thead><tr>" &
							  $"<th style=""{CellStyle}"">Sr. No.</th>" &
							  $"<th style=""{CellStyle}"">Requested Part No.</th>" &
							  $"<th style=""{CellStyle}"">Serial No.</th>" &
							  $"<th style=""{CellStyle}"">Requisition No.</th>" &
							  $"<th style=""{CellStyle}"">Requisition Date</th>" &
							  $"<th style=""{CellStyle}"">Requested Qty.</th>" &
							  $"<th style=""{CellStyle}"">Receipt Qty.</th>" &
							  $"<th style=""{CellStyle}"">Requested By</th>" &
							  $"</tr></thead><tbody>"

			Dim itemGroups = ReceiptCumInvoiceItems _
			.Select(Function(x, i) New With {.Item = x, .Index = i}) _
			.GroupBy(Function(x) x.Item.ReqItemID) _
			.ToDictionary(
				Function(g) g.First().Index,
				Function(g) g.Count()
			)

			Dim BuildRow As Func(Of ReceiptCumInvoiceItem, Integer, String) =
			Function(ReceiptCumInvoiceItem, Index)

				Dim isFirstInGroup = itemGroups.ContainsKey(Index)
				Dim rowSpan = If(isFirstInGroup, $" rowSpan=""{itemGroups(Index)}""", "")
				Dim requestedQty = If(isFirstInGroup,
					$"<td{rowSpan} style=""{CellStyle}"">{ReceiptCumInvoiceItem.ReqQty}</td>",
					"")

				Dim requestedDate = If(ReceiptCumInvoiceItem.ReqDate <> Date.MinValue, ReceiptCumInvoiceItem.ReqDate.ToString("dd-MMM-yyyy"), "")
				Dim receiptQty = If(ReceiptCumInvoiceItem.Qty <> Nothing, CDec(ReceiptCumInvoiceItem.Qty).ToString("##0.00##"), "0.00")

				Return $"<tr>" &
					   $"<td style=""{CellStyle}"">{Index + 1}</td>" &
					   $"<td style=""{CellStyle}"">{_CommonMethod.HtmlEncode(InputString:=ReceiptCumInvoiceItem.ItemName)}</td>" &
					   $"<td style=""{CellStyle}"">{_CommonMethod.HtmlEncode(InputString:=ReceiptCumInvoiceItem.SerialNo)}</td>" &
					   $"<td style=""{CellStyle}"">{_CommonMethod.HtmlEncode(InputString:=ReceiptCumInvoiceItem.ReqNo)}</td>" &
					   $"<td style=""{CellStyle}"">{requestedDate}</td>" &
					   $"<td style=""{CellStyle}"">{requestedQty}</td>" &
					   $"<td style=""{CellStyle}"">{receiptQty}</td>" &
					   $"<td style=""{CellStyle}"">{_CommonMethod.HtmlEncode(InputString:=ReceiptCumInvoiceItem.ReqEmployeeName)}</td>" &
					   "</tr>"

			End Function

			Dim rows = String.Join("", ReceiptCumInvoiceItems.Select(BuildRow))
			Dim tableFooter = "</tbody></table>"

			Return $"{header}{tableHeader}{rows}{tableFooter}"

		Catch ex As Exception

			Dim errorMsg = $"Error in 'GenerateReportBodyForRequestedPartsReceived': 
						   ReceiptNo='{If(_ReceiptCumInvoice?.ReceiptNo, "N / A")}', 
						   Original Exception: {ex.GetType().Name} - {ex.Message}"

			Throw New Exception(errorMsg, ex.GetBaseException())

		End Try

	End Function

	Private Function MailForAuthorization(_ReceiptCumInvoice As ReceiptCumInvoice)

		Dim EmailBody As String
		Dim EmployeeEmailIDs As String
		Dim User = User.GetUser(UserName:=UserName)
		Dim UserEmail = User?.UserEmail
		Try

			If CBool(AppSettings("MailsRequire")) Then

				If _AuthorizationHelper.IsBTPLUser(UserName:=UserName) Then Exit Function

				Dim Result = _ReportHelper.GetReceiptCumInvoiceDetailedReport(RequestFromAPI:=False,
																			  ReceiptID:=_ReceiptCumInvoice.Receipt.ID,
																			  InvoiceID:=_ReceiptCumInvoice.Invoice.ID,
																			  ReceiptCumInvoiceObject:=_ReceiptCumInvoice)

				Dim EmployeeEmailID As EmployeeEmailID = EmployeeEmailID.GetEmployeeEmailID(ReceiptID:=_ReceiptCumInvoice.ID.ToString)

				If EmployeeEmailID.Count > 0 Then

					If EmployeeEmailID(0).EmployeeEmailID <> "" Then
						EmployeeEmailIDs = $"{User.UserEmail},{EmployeeEmailID(0).EmployeeEmailID}"
					End If

				End If

				EmailBody = $"<p style={Style}>" &
							$"Receipt No.: <b>{_ReceiptCumInvoice.ReceiptNo}</b> " &
							$"Dated: <b>{_ReceiptCumInvoice.RecCumInvDateFormatted}</b> " &
							$"has been Authorized By User: <b>{UserName}</b> " &
							$"on: <b>{New SmartDate(Date.Today).FormattedText}</b>." &
							$"</p>"

				_EmailHelper.SendEmail(Info:=EmailBody,
									   UserName:=UserName,
									   ToMailID:=EmployeeEmailIDs,
									   Text:=_ReceiptCumInvoice.ReceiptNo,
									   Subject:="Goods Receipt Details.",
									   TransTypeID:=_ReceiptCumInvoice.TransTypeID,
									   CrystalReport:=CType(Result.Item4, Engine.ReportClass))


			End If

		Catch ex As Exception

			Dim errorMsg = $"Error in 'SendAuthorizationEmail': 
						     ReceiptNo='{If(_ReceiptCumInvoice?.ReceiptNo, "N / A")}', 
						     Original Exception: {ex.GetType().Name} - {ex.Message}"

			Throw New Exception(errorMsg, ex.GetBaseException())

		End Try

	End Function

	Private Sub PUSHNotification(_ReceiptCumInvoice As ReceiptCumInvoice)

		Dim PreviousStepStatus As Boolean = False
		Try

			'Step # 1: Get User Devices
			Dim UserDeviceList As APP_UserDeviceList = APP_UserDeviceList.GetUserDeviceList(FetchTypeID:=6) '6:Receipt
			PreviousStepStatus = (UserDeviceList.Count = 0)

			If PreviousStepStatus = False Then Exit Sub


			'Step # 2: Record PUSH Notification in the table
			Dim UserIDs(50) As Guid
			UserIDs = (
						From UserDeviceInfo As APP_UserDeviceList.UserDeviceinfo In UserDeviceList
						Select (UserDeviceInfo.UserID)
					  ).Distinct().ToArray()

			Dim Notifications(UserIDs.Count - 1) As APP_UserNotification

			For i As Integer = 0 To UserIDs.Count - 1

				If UserIDs(i).Equals(Guid.Empty) Then Exit For

				Dim UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(ID:=Guid.NewGuid)

				Try

					With UserNotification

						.UserID = UserIDs(i)
						.SentOn = Now
						.Message = $"Requested Part(s) Received In:- {_ReceiptCumInvoice.ReceiptNo} Dated:- {_ReceiptCumInvoice.RecCumInvDateFormatted} By User:- {Thread.CurrentPrincipal.Identity.Name}"
						.ModuleType = 6 'Requisition-Order-Receipt
						.ModuleID = _ReceiptCumInvoice.ID

					End With

					UserNotification = CType(UserNotification.Save, APP_UserNotification)
					Notifications(i) = UserNotification
					PreviousStepStatus = True

				Catch ex As Exception
					PreviousStepStatus = False
				End Try

			Next


			If PreviousStepStatus = False Then Exit Sub

			For Each UserNotification As APP_UserNotification In Notifications

				Dim Counter As Integer = 0
				Dim RCounter As Integer = 0
				Dim ErrorCount As Integer = 0

				'Step # 3: Trigger PUSH Notification
StartStep3:     ErrorCount += 1

				Net.ServicePointManager.Expect100Continue = True
				Net.ServicePointManager.SecurityProtocol = 3072

				Dim request = TryCast(Net.WebRequest.Create(requestUriString:="https://onesignal.com/api/v1/notifications"), Net.HttpWebRequest)

				request.KeepAlive = True
				request.Method = "POST"
				request.ContentType = "application/json; charset=utf-8"
				request.Headers.Add("authorization", "Basic YmE0YTUwZDgtMmJkYS00MjMzLWI5NjgtZTkxZmE5MzQ0NzMw")

				Dim filters As Object()
				Dim Serializer = New JavaScriptSerializer()
				ReDim filters(((UserDeviceList.Count - 1) * 2) + 1)
				Dim Index As Integer = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("wfReceiptCumInvoice_Ajax.aspx")
				Dim NotificationDetail As String = $"{HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, Index)}APP/Launcher.aspx?NotificationID={UserNotification}&ModuleID={_ReceiptCumInvoice}&username={UserNotification.UserName}&EventLogSessionID={Guid.NewGuid}&ModuleTypeID=5"

				For Each UserDeviceInfo As APP_UserDeviceList.UserDeviceinfo In UserDeviceList

					If UserNotification.UserID.Equals(UserDeviceInfo.UserID) Then


						If Counter = 0 Then

							filters(Counter) = New With {
															Key .field = "tag",
															Key .key = "DeviceID",
															Key .value = UserDeviceList(0).DeviceID.ToString
														}
							Counter += 1

						Else

							RCounter += 1

							filters(Counter) = New With {Key .[operator] = "OR"}
							Counter += 1

							filters(Counter) = New With {
															Key .field = "tag",
															Key .key = "DeviceID",
															Key .value = UserDeviceList(RCounter).DeviceID.ToString
														 }
							Counter += 1

						End If

					End If

				Next

				Dim responseContent As String = Nothing
				Dim obj = New With {
										Key .app_id = "f877b4d2-b6e5-4595-a381-87165f6e46a0",
										Key .contents = New With {Key .en = UserNotification.Message},
										Key .headings = New With {Key .en = "FlyPal"},
										filters,
										Key .data = New With {Key .url = NotificationDetail.ToString}
									}
				Dim param = Serializer.Serialize(obj)
				Dim byteArray As Byte() = Encoding.UTF8.GetBytes(param)

				Try

					Using writer = request.GetRequestStream()
						writer.Write(byteArray, 0, byteArray.Length)
					End Using

					Using response As Net.HttpWebResponse = request.GetResponse()

						Using reader = New StreamReader(stream:=response.GetResponseStream())
							responseContent = reader.ReadToEnd()
						End Using

					End Using

				Catch ex As Net.WebException

					Diagnostics.Debug.WriteLine(message:=ex.Message)
					Diagnostics.Debug.WriteLine(message:=New StreamReader(stream:=ex.Response.GetResponseStream()).ReadToEnd())

					If ErrorCount <= 3 Then GoTo StartStep3

				End Try

				Diagnostics.Debug.WriteLine(responseContent)

			Next

		Catch ex As Exception

			Dim errorMsg = $"Error in 'PUSHNotification': 
						     ReceiptNo='{If(_ReceiptCumInvoice?.ReceiptNo, "N / A")}', 
						     Original Exception: {ex.GetType().Name} - {ex.Message}"

			Throw New Exception(errorMsg, ex.GetBaseException())

		End Try

	End Sub

#End Region

End Class
