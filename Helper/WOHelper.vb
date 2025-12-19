'************************************
'Created by:	Harsh Sugandhi
'Created on:	28th April 2025
'Created for:	Helper Method to Fetch the WO based on the client code to access it across the Application.
'************************************


Public Class WOHelper

#Region " Helper Method(s) "

	Public Shared Function FetchWO(ID As Guid) As nWO

		Dim _WO As nWO
		Try

			If AppSettings("ClientCode") = "STR" Or
			   AppSettings("ClientCode") = "SHN" Or
			   AppSettings("ClientCode") = "MYT" Or
			   AppSettings("ClientCode") = "" Then

				_WO = nWO.GetWO(ID:=ID,
								AllWOJobType:=False,
								getAircraftValuesAsOnCompletionDate:=True)
			Else
				_WO = nWO.GetWO(ID:=ID,
								AllWOJobType:=False)
			End If

			Return _WO

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class
