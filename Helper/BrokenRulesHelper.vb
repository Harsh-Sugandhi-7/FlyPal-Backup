'************************************
'Created by:	Harsh Sugandhi
'Created on:	10th October 2025
'Created for:	To handle the Broken Rules for every Module.
'************************************


Imports System.Text

Public Class BrokenRulesHelper

#Region " Helper Method(s) "

	Public Function FetchBrokenRules(CommonObject As Object,
									 ModuleName As String) As StringBuilder

		Dim BrokenRules As New StringBuilder()
		Try

			Select Case ModuleName
				Case "Order"

					CommonObject = CType(CommonObject, Order)

					For i As Integer = 0 To CommonObject.GetBrokenRulesCollection.Count - 1
						BrokenRules.AppendLine(CommonObject.GetBrokenRulesCollection(i).Description)
					Next

					For i As Integer = 0 To CommonObject.OrderItems.Count - 1

						If Not CommonObject.OrderItems(i).IsValid Then

							For x As Integer = 0 To CommonObject.OrderItems(i).GetBrokenRulesCollection.Count - 1
								BrokenRules.AppendLine($"{CommonObject.OrderItems.Item(i).ItemName} : {CommonObject.OrderItems.Item(i).GetBrokenRulesCollection(x).Description}")
							Next

						End If

					Next

				Case "RCI"

					CommonObject = CType(CommonObject, ReceiptCumInvoice)

					For i As Integer = 0 To CommonObject.GetBrokenRulesCollection.Count - 1
						BrokenRules.AppendLine(CommonObject.GetBrokenRulesCollection(i).Description)
					Next

					For i As Integer = 0 To CommonObject.ReceiptCumInvoiceItems.Count - 1

						If Not CommonObject.ReceiptCumInvoiceItems(i).IsValid Then

							For x As Integer = 0 To CommonObject.ReceiptCumInvoiceItems(i).GetBrokenRulesCollection.Count - 1
								BrokenRules.AppendLine($"{CommonObject.ReceiptCumInvoiceItems.Item(i).ItemName} : {CommonObject.ReceiptCumInvoiceItems.Item(i).GetBrokenRulesCollection(x).Description}")
							Next

						End If

					Next

				Case "Enquiry"

					CommonObject = CType(CommonObject, Enquiry)

					For i As Integer = 0 To CommonObject.GetBrokenRulesCollection.Count - 1
						BrokenRules.AppendLine(CommonObject.GetBrokenRulesCollection(i).Description)
					Next

					'Enquiry Items
					For i As Integer = 0 To CommonObject.EnquiryItems.Count - 1

						If Not CommonObject.EnquiryItems(i).IsValid Then

							For x As Integer = 0 To CommonObject.EnquiryItems(i).GetBrokenRulesCollection.Count - 1
								BrokenRules.AppendLine($"{CommonObject.EnquiryItems.Item(i).ItemName} : {CommonObject.EnquiryItems.Item(i).GetBrokenRulesCollection(x).Description}")
							Next

						End If

					Next

					'Enquiry Requisition Items
					For i As Integer = 0 To CommonObject.EnquiryItems.Count - 1

						For j As Integer = 0 To CommonObject.EnquiryItems(i).RequisitionItemEnquiryItems.Count - 1

							If Not CommonObject.EnquiryItems(i).RequisitionItemEnquiryItems(j).IsValid Then

								For k As Integer = 0 To CommonObject.EnquiryItems(i).RequisitionItemEnquiryItems(j).GetBrokenRulesCollection.Count - 1
									BrokenRules.AppendLine(CommonObject.EnquiryItems(i).RequisitionItemEnquiryItems.Item(j).GetBrokenRulesCollection(k).Description)
								Next

							End If

						Next

					Next

			End Select

			Return BrokenRules.Replace("<BR>", "")

		Catch ex As Exception
			Return BrokenRules.AppendLine(ex.Message)
		End Try

	End Function

#End Region

End Class
