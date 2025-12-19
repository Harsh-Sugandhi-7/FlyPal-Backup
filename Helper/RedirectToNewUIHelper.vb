'************************************
'Created by:	Harsh Sugandhi
'Created on:	10th October 2025
'Created for:	To handle the navigation to New Application.
'************************************


Public Class RedirectToNewUIHelper

#Region " Helper Method(s) "

	Public Function NavigationLinkForNewUI(Request As HttpRequest,
										   NavigationLink As String) As String

		Try

			Dim CurrentUrl As String = Request.Url.ToString()

			' Extract the protocol and domain (hostname)
			Dim Protocol As String = Request.Url.Scheme ' e.g https"

			Dim Domain As String = AppSettings("HostName") 'e.g bytzsoft.in"

			' Extract the path part (after domain)
			Dim Path As String = Request.Url.AbsolutePath ' e.g /FlyPalWebAPI/Login.aspx"

			' Identify the virtual directory (the first part of the path)
			Dim Segments As String() = Path.Split("/"c)
			Dim VirtualDirectory As String = Segments(1).ToLower ' e.g FlyPalWebAPI or any other directory name

			Dim Url As String = $"{Protocol}://{Domain}/{VirtualDirectory}/{NavigationLink}"

			Return Url

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class
