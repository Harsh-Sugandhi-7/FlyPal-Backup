'************************************
'Created by:	Harsh Sugandhi
'Created on:	15th September 2025
'Created for:	To handle the SQL Exceptions in centralize / common way.
'************************************


Public Class SQLExceptionHelper


#Region " Varriable(s) "

	Private _MessageBox As New MSGBox

#End Region

#Region " Helper Method "

	Public Function UserFriendlyExceptionMessage(ex As Object,
												 Optional ModuleName As String = "",
												 Optional UseAsException As Boolean = False) As String

		Dim returnMessage As String

		Try

			If UseAsException Then

				ex = CType(ex, Exception)

				If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or
				   InStr(ex.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or
				   InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or
				   InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or
				   InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
					returnMessage = $"Record cannot be saved.{Environment.NewLine}{ex.Message.Substring(ex.Message.IndexOf("PartNo.:"))} {Environment.NewLine}Goods Receipt Qty cannot be greater than Order / Issue Qty."
				ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
					returnMessage = $"Record cannot be saved.{Environment.NewLine}{ex.Message.Substring(ex.Message.IndexOf("PartNo.:"))} {Environment.NewLine}Receipt quantity exceeds Order quantity.{Environment.NewLine} Please Amend the Purchase Order used in Receipt for excess quantity."
				ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
					returnMessage = $"The selected Charge can’t be found.{Environment.NewLine}It may have been removed or is no longer available. Please delete it from your selection and choose a new charge to continue."
				ElseIf InStr(ex.Message, "FKtabConditionCheckItemtabReceiptItem", CompareMethod.Text) Then
					returnMessage = "Record cannot be deleted because it is currently used in other records in the system."
				Else
					returnMessage = $"There is some problem in saving Record.{Environment.NewLine} Please check the Entry and try again."
				End If

				Return returnMessage.Replace("<p>", "").
									 Replace("</p>", "").
									 Replace("<strong>", "").
									 Replace("</strong>", "").
									 Replace("<BR>", "").
									 Replace("<br />", "").
									 Replace("<br>", "")
			Else
				ex = CType(ex, SqlException)
			End If

			If ex.Number = 8114 Or ex.Number = 8115 Then
				returnMessage = _MessageBox.MessageBoxForAPI(MessageText:=MSGBox.Message_Text.NumericOverFlow,
															 ExtraMessage:=" Rate or Qty or Conversion Factor. ")
			ElseIf ex.Number = 8145 Then
				returnMessage = _MessageBox.MessageBoxForAPI(MessageText:=MSGBox.Message_Text.ProcedureError,
															 ExtraMessage:=ex.Procedure)
			ElseIf ex.Number = 2627 Then
				returnMessage = _MessageBox.MessageBoxForAPI(MessageText:=MSGBox.Message_Text.Duplicate,
															 ExtraMessage:=ex.Procedure)
			ElseIf ex.Number = 547 OrElse UseAsException Then

				Select Case ModuleName
					Case "Enquiry"

						If InStr(ex.Message, "CCtabRequisitionItemEnquiryBalQty", CompareMethod.Text) Or
						   InStr(ex.Message, "CCtabRequisitionItemEnquiryBalQty", CompareMethod.Text) Then
							returnMessage = "Enquiry Quantity cannot be greater than Requisition Quantity."
						ElseIf InStr(ex.Message, "FKtabEnquiryTermtabTerm", CompareMethod.Text) Then
							returnMessage = "Term is not Available. Selected Term no longer exist in the Database. Remove Term & Try again"
						ElseIf ex.ToString.Contains("CCtabEnquiryNo") Then
							returnMessage = "Transaction Text-Series does not exist for this Transaction. Kindly contact Administrator."
						Else
							returnMessage = _MessageBox.MessageBoxForAPI(MSGBox.Message_Text.ReferenceDelete)
						End If

					Case "Order"

						If InStr(ex.Message, "CCtabQuotationItemPurchaseBalQty", CompareMethod.Text) Then
							returnMessage = "Order Qty cannot be greater than Quotation Qty."
						ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
							returnMessage = $"Cannot Save / Authorized !!! Order Qty cannot be less than Received Qty."
						ElseIf InStr(ex.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
							returnMessage = "Cannot Save / Authorized !!! Order Qty cannot be less than Issued Qty."
						ElseIf InStr(ex.Message, "FK_tabOrderCharge_tabCharge", CompareMethod.Text) Then
							returnMessage = "Other Charge is not available. Selected Charge is no longer exist in the Database. Remove the Charge and try again"
						ElseIf InStr(ex.Message, "FKtabOrderTermtabTerm", CompareMethod.Text) Then
							returnMessage = "Term is not available. Selected Term is no longer exist in the Database. Remove the Term and try again"
						Else
							returnMessage = "Record cannot be saved."
						End If

					Case "RCI"

						If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or
						   InStr(ex.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or
						   InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or
						   InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or
						   InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
							returnMessage = $"Record cannot be saved.{Environment.NewLine}{ex.Message.Substring(ex.Message.IndexOf("PartNo.:"))} {Environment.NewLine}Goods Receipt Qty cannot be greater than Order / Issue Qty."
						ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
							returnMessage = $"Record cannot be saved.{Environment.NewLine}{ex.Message.Substring(ex.Message.IndexOf("PartNo.:"))} {Environment.NewLine}Receipt quantity exceeds Order quantity.{Environment.NewLine} Please Amend the Purchase Order used in Receipt for excess quantity."
						ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
							returnMessage = $"The selected Charge can’t be found.{Environment.NewLine}It may have been removed or is no longer available. Please delete it from your selection and choose a new charge to continue."
						ElseIf InStr(ex.Message, "FKtabConditionCheckItemtabReceiptItem", CompareMethod.Text) Then
							returnMessage = "Record cannot be deleted because it is currently used in other records in the system."
						Else
							returnMessage = $"There is some problem in Saving Goods Receipt.{Environment.NewLine} Please check the Entry and try again."
						End If

					Case "Other Docket Charges"

						If InStr(ex.Message, "FKtabOtherChargeDetailstabCharge", CompareMethod.Text) Then
							returnMessage = $"The selected Charge can’t be found.{Environment.NewLine}It may have been removed or is no longer available. Please delete it from your selection and choose a new charge to continue."
						End If

					Case Else
						returnMessage = _MessageBox.MessageBoxForAPI(MSGBox.Message_Text.ReferenceDelete)
				End Select

			ElseIf ex.Number = 50000 Then

				If ex.State = 2 Then
					returnMessage = $"Cannot Save !!! {ex.Message}"
				End If

			End If

			If ModuleName = "Requisition" AndAlso ex.Message.Contains("CK_tabReq_NoRequired") Then
				returnMessage = "Requisition No. should be greater than zero."
			End If

			Return returnMessage.Replace("<p>", "").
								 Replace("</p>", "").
								 Replace("<strong>", "").
								 Replace("</strong>", "").
								 Replace("<BR>", "").
								 Replace("<br />", "").
								 Replace("<br>", "")

		Catch Exception As Exception
			Throw Exception.GetBaseException
		End Try

	End Function

	Public Function UserFriendlyExceptionMessageForDelete(SqlException As SqlException,
														  Optional ModuleName As String = "") As String

		Dim stringInfo, returnMessage As String
		Try

			If SqlException.Number = 547 Or SqlException.Number = 50000 Then

				Select Case ModuleName
					Case "Order"

						If SqlException.Message.Contains("tabCWP") Then
							stringInfo = "CWP."
						ElseIf SqlException.Message.Contains("tabReceiptItem") Then
							stringInfo = "Receipt."
						ElseIf SqlException.Message.Contains("tabIssueItem") Then
							stringInfo = "Issue."
						ElseIf SqlException.Message.Contains("tabOrderItemFollowUp") Then
							stringInfo = "Order Follow Up."
						ElseIf SqlException.Message.Contains("tabPaymentAdviceItem") Then
							stringInfo = "Payment Advice."
						ElseIf SqlException.Message.Contains("tabReqItem") Then
							stringInfo = "Requisition Item."
						End If

					Case "RCI"

						If SqlException.Message.Contains("tabInvoiceItem") Then
							stringInfo = "Invoice."
						ElseIf SqlException.Message.Contains("tabIssueItem") Then
							stringInfo = "Issue."
						ElseIf SqlException.Message.Contains("tabOrderItem") Then
							stringInfo = "Order."
						ElseIf SqlException.Message.Contains("tabConditionCheckItem") Then
							stringInfo = "Equipment Maintenance."
						ElseIf SqlException.Message.Contains("tabCalibrationItem") Then
							stringInfo = "Calibration."
						ElseIf SqlException.Message.Contains("tabOtherChargeInvoices") Then
							stringInfo = "Docket Charge."
						ElseIf SqlException.Message.Contains("tabComponentReservation") Then
							stringInfo = "Component Reservation."
						ElseIf SqlException.Message.Contains("Cannot delete record") Then

							stringInfo = If(HttpContext.Current.User.Identity.Name.ToUpper = "BTPLAdmin".ToUpper,
											SqlException.Message.Substring(SqlException.Message.IndexOf("use") + 3),
											"Issue.")

						End If

					Case "Vendor"

						If SqlException.Message.Contains("tabCalloutCustomer") Then
							stringInfo = "Callout."
						ElseIf SqlException.Message.Contains("tabEnqSupplier") Then
							stringInfo = "Enquiry."
						ElseIf SqlException.Message.Contains("tabExportInvoice") Then
							stringInfo = "Export Invoice."
						ElseIf SqlException.Message.Contains("tabInvoice") Then
							stringInfo = "Invoice."
						ElseIf SqlException.Message.Contains("tabLineMaintInvoice") Then
							stringInfo = "Line Maint. Invoice."
						ElseIf SqlException.Message.Contains("tabLineMaintOrder") Then
							stringInfo = "Line Maint. Order."
						ElseIf SqlException.Message.Contains("tabMachinetabVendor") Then
							stringInfo = "Aircraft."
						ElseIf SqlException.Message.Contains("tabMaintenanceInvoice") Then
							stringInfo = "Maintenance Invoice."
						ElseIf SqlException.Message.Contains("tabnWO") Then
							stringInfo = "Work Order."
						ElseIf SqlException.Message.Contains("tabOrder") Then
							stringInfo = "Order."
						ElseIf SqlException.Message.Contains("tabOtherChargeDetails") Then
							stringInfo = "Other Charge."
						ElseIf SqlException.Message.Contains("tabPayment") Then
							stringInfo = "Payment."
						ElseIf SqlException.Message.Contains("tabProject") Then
							stringInfo = "Project."
						ElseIf SqlException.Message.Contains("tabReceipt") Then
							stringInfo = "Receipt."
						ElseIf SqlException.Message.Contains("tabIssue") Then
							stringInfo = "Issue."
						ElseIf SqlException.Message.Contains("tabSalesInvoice") Then
							stringInfo = "Sales Invoice."
						ElseIf SqlException.Message.Contains("tabSalesOrder") Then
							stringInfo = "Sales Order."
						ElseIf SqlException.Message.Contains("FKtabStoretabVendor") Then
							stringInfo = "Store."
						ElseIf SqlException.Message.Contains("tabVendorApproval") Then
							stringInfo = "Vendor Approval."
						ElseIf SqlException.Message.Contains("FKtabItemtabVendor") Then
							stringInfo = "Item Master."
						ElseIf SqlException.Message.Contains("tabCustomerContract") Then
							stringInfo = "Customer Contract."
						ElseIf SqlException.Message.Contains("tabProject") Then
							stringInfo = "Project."
						ElseIf SqlException.Message.Contains("tabnWO") Then
							stringInfo = "Work Order."
						End If

				End Select

				returnMessage = _MessageBox.MessageBoxForAPI(MessageText:=MSGBox.Message_Text.ReferenceDelete,
															 ExtraMessage:=stringInfo)

			End If

			Return returnMessage.Replace("<p>", "").
								 Replace("</p>", "").
								 Replace("<strong>", "").
								 Replace("</strong>", "").
								 Replace("<BR>", "").
								 Replace("<br />", "").
								 Replace("<br>", "")

		Catch SqlException
			Throw SqlException.GetBaseException
		End Try

	End Function

#End Region

End Class
