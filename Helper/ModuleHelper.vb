'************************************
'Created by:	Harsh Sugandhi
'Created on:	07th October 2025
'Created for:	To fetch module name based on TransTypeID.
'************************************


Public Class ModuleHelper

#Region " Helper Method(s) "

	Public Function GetModuleName(TransTypeID As Integer) As String

		Dim ModuleName As String
		Dim TransactionList As TransactionList
		Try

			TransactionList = TransactionList.GetTransactionList()
			ModuleName = TransactionList.Item(TransTypeID:=TransTypeID).ModuleName

			Return ModuleName

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SendEmailToBytzSoft(TransTypeID As Integer,
										Username As String,
										ModuleFrom As String,
										Action As String,
										ClientCode As String,
										TransactionNo As String,
										TransactionDate As String)

		Try

			Dim ModuleName As String = GetModuleName(TransTypeID:=TransTypeID)
			Dim ModuleList As ModuleList = ModuleList.GetModuleList(ModuleName)

			If ModuleList.Item(ModuleName).MailsRequire AndAlso (Not Username.Equals("BTPLADMIN", StringComparison.CurrentCultureIgnoreCase)) Then

				SendMailFile.SendMailFile(UserName:=Username,
										  Subject:=$"{ModuleFrom} successfully {Action} from the New UI. Client {ClientCode}",
										  Info:=$"{ModuleFrom} No:- {TransactionNo} Date:- {TransactionDate} User Name:- {Username}",
										  ToMailID:=If(AppSettings("SenEmailToFromNewApplication"), "support@bytzsoft.com"))

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class
