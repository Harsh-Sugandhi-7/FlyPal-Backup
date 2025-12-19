Imports System.Web.Http

Public Class OtherChargeController
	Inherits ApiController


#Region " Get Method(s) "

	<HttpGet>
	Public Function GetOtherChargeList(Optional Text As String = "",
									   Optional No As Integer = 0,
									   Optional FromDate As String = "1/1/1900",
									   Optional ToDate As String = "1/1/2200",
									   Optional IsCustomPaging As Boolean = False,
									   Optional CurrentPage As Integer = 0,
									   Optional PageSize As Integer = 25,
									   Optional InvoiceText As String = "",
									   Optional InvoiceNo As Integer = 0,
									   Optional ReceiptText As String = "",
									   Optional ReceiptNo As Integer = 0,
									   Optional ItemName As String = "",
									   Optional OrderText As String = "",
									   Optional OrderNo As Integer = 0,
									   Optional VendorName As String = "") As OtherChargeList
		Try

			Return OtherChargeList.GetOtherChargeList(Text:=Text,
													  No:=No,
													  FromDate:=FromDate,
													  ToDate:=ToDate,
													  IsCustomPaging:=IsCustomPaging,
													  CurrentPage:=CurrentPage,
													  PageSize:=PageSize,
													  InvoiceText:=InvoiceText,
													  InvoiceNo:=InvoiceNo,
													  ReceiptText:=ReceiptText,
													  ReceiptNo:=ReceiptNo,
													  ItemName:=ItemName,
													  OrderText:=OrderText,
													  OrderNo:=OrderNo,
													  VendorName:=VendorName)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetOtherCharge(ID As Guid) As OtherCharge

		Try
			Return OtherCharge.GetOtherCharge(ID:=ID)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/OtherCharge/ChargeList")>
	<Route("api/OtherCharge")>
	Public Function ChargeList(Optional Name As String = "",
							   Optional Type As Integer = -1,
							   Optional IsSelectTagRequired As Boolean = False,
							   Optional IsPlusOnly As Boolean = False) As ChargeList
		Try

			Return ChargeList.GetChargeList(Name:=Name,
											Type:=Type,
											IsSelectTagRequired:=IsSelectTagRequired,
											IsPlusOnly:=IsPlusOnly)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function OtherChargeTypeList() As OtherChargeTypeList

		Try

			Return OtherChargeTypeList.GetOtherChargeTypeList()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Get New Method(s) "

	<HttpGet>
	Public Function GetNewOtherCharge(ID As Guid) As OtherCharge

		Try
			Return OtherCharge.NewOtherCharge(ID:=ID)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetNewOtherChargeDetail(ID As Guid) As OtherChargeDetail

		Try

			Dim mOtherCharge As OtherCharge = OtherCharge.NewOtherCharge(ID:=ID)
			mOtherCharge.OtherChargeDetails.Add(mOtherCharge.ID)

			Return mOtherCharge.OtherChargeDetails.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Sub PostValue(<FromBody()> Value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(ID As Integer, <FromBody()> Value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	Public Sub DeleteValue(ID As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
