Imports System.Web.Http


Public Class IsInUseController
	Inherits ApiController

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetIsInUseEnquiryInQuotation(Optional QuotationID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseEnquiryINQuotation(QuotationID:=New Guid(QuotationID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseQuotationInSalesOrder(Optional QuotationID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseQuotationINSalesOrder(QuotationID:=New Guid(QuotationID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseRequisitionInOrder(Optional RequisitionID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseRequisitionINOrder(RequisitionID:=New Guid(RequisitionID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseSalesOrderInOrder(Optional SalesOrderID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseSalesOrderINOrder(SalesOrderID:=New Guid(SalesOrderID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseOrderInReceipt(Optional OrderID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseOrderINReceipt(OrdereID:=New Guid(OrderID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseReceiptInIssue(Optional ReceiptID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseReceiptINIssue(ReceiptID:=New Guid(ReceiptID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseReceiptInInvoice(Optional ReceiptID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseReceiptINInvoice(ReceiptID:=New Guid(ReceiptID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseIssueInReceipt(Optional IssueID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseIssueINReceipt(IssueID:=New Guid(IssueID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseInvoiceInPayment(Optional InvoiceID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseInvoiceINPayment(InvoiceID:=New Guid(InvoiceID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseSalesOrderInIssue(Optional SalesOrderID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseSalesOrderINIssue(SalesOrderID:=New Guid(SalesOrderID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseInvoiceInOtherCharge(Optional InvoiceID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseInvoiceINOtherCharge(InvoiceID:=New Guid(InvoiceID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseWOInIssue(Optional WOID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseWOINIssue(WOID:=New Guid(WOID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUsenWOInIssue(Optional nWOID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUsenWOINIssue(nWOID:=New Guid(nWOID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseLineOrderInLineInvoice(Optional LineOrderItemID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseLineOrderInLineInvoice(LineOrderItemID:=New Guid(LineOrderItemID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetIsInUseForRequisitionInEnqQuoOrderIssue(Optional ReqID As String = "{00000000-0000-0000-0000-000000000000}") As IsInUse

		Try

			Return IsInUse.GetIsInUseForRequisitionInEnqQuoOrderIssue(ReqID:=New Guid(ReqID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Sub PostValue(<FromBody()> value As String)

		Try


		Catch ex As Exception

			Throw ex.GetBaseException

		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

		Try


		Catch ex As Exception

			Throw ex.GetBaseException

		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Sub DeleteValue(id As Integer)

		Try


		Catch ex As Exception

			Throw ex.GetBaseException

		End Try

	End Sub

#End Region

End Class
