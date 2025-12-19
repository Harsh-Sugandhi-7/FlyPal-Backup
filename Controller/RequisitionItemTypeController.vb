Imports System.Web.Http


Public Class RequisitionItemTypeController
	Inherits ApiController


#Region " Get Method(s) "

	<HttpGet>
	<Route("api/RequisitionItemType/GetRequisitionItemTypeList")>
	Public Function GetRequisitionItemTypeList(Optional RequisitionItemTypeID As Integer = -1,
											   Optional IsSelectTagRequired As Boolean = False) As RequisitionItemTypeList

		Try

			Return RequisitionItemTypeList.GetRequisitionItemTypeList(RequisitionItemTypeID:=RequisitionItemTypeID,
																	  IsSelectTagRequired:=IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	Public Sub PostValue(<FromBody()> Value As String)

	End Sub

#End Region

#Region " Put Method(s) "

	Public Sub PutValue(ID As Integer, <FromBody()> Value As String)

	End Sub

#End Region

#Region " Delete Method(s) "

	Public Sub DeleteValue(ID As Integer)

	End Sub

#End Region

End Class
