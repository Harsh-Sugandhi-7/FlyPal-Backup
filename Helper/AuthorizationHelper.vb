'************************************
'Created by:	Harsh Sugandhi
'Created on:	10th October 2025
'Created for:	To handle the Authorization for User for every Module.
'************************************


Imports System.Security.Principal


Public Class AuthorizationHelper

#Region " Helper Method(s) "

	Public Function GetRoleNameString(Action As Action,
									  Optional ModuleName As String = "",
									  Optional TransTypeID As Integer = 0) As String

		Dim RoleNameString As String
		Try

			If TransTypeID <> 0 Then

				'Add cases as and when used in other Modules as well
				Select Case TransTypeID
					Case 101
						RoleNameString = "Work-Pack"
					Case 104
						RoleNameString = "AMO Project"
					Case 115
						RoleNameString = "DiscrepancyAction"
					Case 116
						RoleNameString = "CabinDefect"
				End Select
			Else
				RoleNameString = ModuleName
			End If

			Select Case Action
				Case Action.New
					Return $"{RoleNameString}New"
				Case Action.Edit
					Return $"{RoleNameString}Edit"
				Case Action.Delete
					Return $"{RoleNameString}Delete"
				Case Action.View
					Return $"{RoleNameString}View"
				Case Action.Print
					Return $"{RoleNameString}Print"
				Case Action.Authorize
					Return $"{RoleNameString}Authorized"
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function CheckIfUserHasRights(User As IPrincipal,
										 Action() As Action,
										 MSGBoxCtrl As MSGBox,
										 Optional ModuleName As String = "",
										 Optional TransTypeID As Integer = 0,
										 Optional MSGBoxSender As String = "",
										 Optional ExtraMessage As String = "",
										 Optional MarkLogDetail As String = "",
										 Optional IsForSave As Boolean = False) As Boolean

		Dim hasRight As Boolean = False

		Try

			MarkLogDetail = If(MarkLogDetail Is Nothing Or MarkLogDetail = "",
							   $"{User.Identity.Name} is not Authorized User to {Action(0)}",
							   MarkLogDetail)

			For i As Integer = 0 To Action.Length - 1

				If User.IsInRole(role:=GetRoleNameString(ModuleName:=ModuleName,
														 Action:=Action(i),
														 TransTypeID:=TransTypeID)) Then

					hasRight = True

					MarkLog(If(IsForSave, Util.Action.Save, Action(i)),
							ModuleName,
							MarkLogDetail,
							ErrorType.HandledError,
							Guid.Empty,
							EventLogID)

					Exit For

				End If

			Next

			If Not hasRight Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								ExtraMessage,
								MsgBoxStyle.OkOnly,
								MSGBoxSender)

				Return False

			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function IsBTPLUser(UserName As String) As Boolean

		Try

			If UserName.Equals("BTPLAdmin", StringComparison.InvariantCultureIgnoreCase) Or
			   UserName.Equals("BYTZAdmin", StringComparison.InvariantCultureIgnoreCase) Then
				Return True
			End If

			Return False

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class
