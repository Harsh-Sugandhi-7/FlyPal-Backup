Imports System.Net

Imports Newtonsoft.Json.Linq
'Added by Shital on 02-Nov2021

Public Class SendSMS

	Public Shared Sub SendSMS(ByVal SenderID As String, ByVal TemplateID As String, ByVal Message As String, ByVal MobileNos As String)

		Dim loginstr As String = "http://sms.webasha.com/vb/apikey.php?apikey=0OTEClMgQnvL4Vr0&senderid=FLYPAL&templateid=" + TemplateID + "&number=" + MobileNos + "&message=" + Message

		ServicePointManager.Expect100Continue = True
		ServicePointManager.SecurityProtocol = 3072

		Dim wclient As WebClient = New WebClient()

		Dim Result1 As String = wclient.DownloadString(loginstr)

		Dim ser As JObject = JObject.Parse(Result1)
		Dim ReturnMessage As String = ""
		ReturnMessage = ser("status") 'Success

	End Sub
End Class
