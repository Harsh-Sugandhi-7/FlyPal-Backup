Imports System.Web.Http

Public Class rptAircraftMonthlyFlyingController
	Inherits ApiController

	Public Function GetMachineList(CurrentDate As String,
								   Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
								   Optional MachineTypeID As Integer = 0,
								   Optional MachineCategoryID As Integer = 0,
								   Optional MachineCategoryName As String = "",
								   Optional Owner As String = "",
								   Optional RegNo As String = "",
								   Optional IsTagRequired As Boolean = False,
								   Optional TagText As String = "",
								   Optional ForInventory As Boolean = False,
								   Optional SkipIsForInventoryAircarft As Boolean = False,
								   Optional ModelID As String = "{00000000-0000-0000-0000-000000000000}",
								   Optional SkipReadOnlyAircrafts As Boolean = False,
								   Optional IsUnitRequired As Boolean = False,
								   Optional UnitID As Integer = 0,
								   Optional ModelIDStr As String = "",
								   Optional MachineIDStr As String = "",
								   Optional IsForPBH As Boolean = False) As MachineNameValueList

		Try

			Dim User As User = UserManagerController.FetchUser()
			Dim UserName = User.Name

			Return MachineNameValueList.GetMachineList(CurrentDate:=CurrentDate,
													   MachineID:=MachineID,
													   MachineTypeID:=MachineTypeID,
													   MachineCategoryID:=MachineCategoryID,
													   Owner:=Owner,
													   RegNo:=RegNo,
													   IsTagRequired:=IsTagRequired,
													   TagText:=TagText,
													   ForInventory:=ForInventory,
													   SkipIsForInventoryAircarft:=SkipIsForInventoryAircarft,
													   ModelID:=ModelID,
													   Username:=UserName,
													   SkipReadOnlyAircrafts:=SkipReadOnlyAircrafts,
													   IsUnitRequired:=IsUnitRequired,
													   UnitID:=UnitID,
													   ModelIDStr:=ModelIDStr,
													   MachineIDStr:=MachineIDStr,
													   IsForPBH:=IsForPBH)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function


	Public Function GetValue(id As Integer) As String
		Return "value"
	End Function

	Public Sub PostValue(<FromBody()> value As String)

	End Sub

	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

	Public Sub DeleteValue(id As Integer)

	End Sub

End Class
