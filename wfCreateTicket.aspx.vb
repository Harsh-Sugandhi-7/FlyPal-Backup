Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class wfCreateTicket
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mUser As User
#End Region

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

                Case MsgBoxResult.Cancel

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""

                    'Response.Redirect("wfCopyServices.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfCopyServices.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub

    Protected Async Sub btnSubmit_Click(sender As Object, e As EventArgs)
        Dim name As String = txtName.Text
        Dim email As String = txtEmail.Text
        Dim subject As String = txtSubject.Text
        Dim description As String = txtDescription.Text



        Dim ccEmails As New List(Of String)
        If Not String.IsNullOrWhiteSpace(txtCC.Text) Then
            Dim ccMails As String = txtCC.Text.Trim '& ",support@bytzsoft.com"
            ccEmails = ccMails.Split(","c).Select(Function(cc) cc.Trim()).Where(Function(cc) Not String.IsNullOrEmpty(cc)).ToList()
        End If



        Dim uploadTokens As New List(Of String)

        ' Handle multiple file uploads
        For Each postedFile As HttpPostedFile In fuAttachments.PostedFiles
            If postedFile.ContentLength > 0 Then
                Dim fileBytes As Byte() = New Byte(postedFile.ContentLength - 1) {}
                postedFile.InputStream.Read(fileBytes, 0, postedFile.ContentLength)
                Dim token As String = Await UploadFileToZendesk(fileBytes, Path.GetFileName(postedFile.FileName))
                If Not String.IsNullOrEmpty(token) Then
                    uploadTokens.Add(token)
                End If
            End If
        Next


        ' CreateZendeskTicket(name, email, subject, description).Wait()
        Await CreateZendeskTicket(name, email, subject, description, uploadTokens, ccEmails)

    End Sub


    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            mUser = SI.UTILITY.User.GetUser(HttpContext.Current.User.Identity.Name)
            txtName.Text = HttpContext.Current.User.Identity.Name
            txtEmail.Text = mUser.UserEmail
        End If

    End Sub

    Private Async Function CreateZendeskTicket(name As String,
                                               email As String,
                                               subject As String,
                                               description As String,
                                               uploadTokens As List(Of String),
                                               ccEmails As List(Of String)) As Task(Of Task)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim client As New HttpClient()
        Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes("amar@bytzsoft.com/token:mlBqMCjiqnQAti0raihPWq0pDku81RetiiK9J3Rb"))
        client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Basic", credentials)





        Dim ticket = New With {
            .ticket = New With {
                .requester = New With {.name = name, .email = email},
                .subject = subject,
                .comment = New With {
                    .body = description,
                    .uploads = uploadTokens
                },
                .email_ccs = ccEmails.Select(Function(cc) New With {
                    .user_email = cc,
                    .action = "put"
                }).ToList(),
                .collaborators = New List(Of String) From {
                    "support@bytzsoft.com"
                }
            }
}

        Dim json = JsonConvert.SerializeObject(ticket)
        Dim content = New StringContent(json, Encoding.UTF8, "application/json")
        Dim response = Await client.PostAsync("https://bytzsoft.zendesk.com/api/v2/tickets.json", content)

        Dim responseString = Await response.Content.ReadAsStringAsync()
        If response.IsSuccessStatusCode Then
            Dim responseObject = JsonConvert.DeserializeObject(Of ZendeskTicketResponse)(responseString)
            MSGBoxCtrl.Show("Alert", "Ticket created successfully", "Ticket ID: " & responseObject.ticket.id, MsgBoxStyle.OkOnly, "")
            txtSubject.Text = ""
            txtDescription.Text = ""
            txtCC.Text = ""
        Else
            ' Handle error
        End If
    End Function

    'Private Async Function CreateZendeskTicket(name As String, email As String, subject As String, description As String) As Tasks.Task(Of Task)
    '    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

    '    Dim client As New HttpClient()
    '    Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes("amar@bytzsoft.com/token:mlBqMCjiqnQAti0raihPWq0pDku81RetiiK9J3Rb"))

    '    client.DefaultRequestHeaders.Add("Authorization", "Basic " & credentials)


    '    Dim ticket = New With {.ticket = New With {.requester = New With {.name = name, .email = email}, .subject = subject, .comment = New With {.body = description}}}


    '    Dim json = JsonConvert.SerializeObject(ticket)

    '    Dim content = New StringContent(json, Encoding.UTF8, "application/json")
    '    Dim response = Await client.PostAsync("https://bytzsoft.zendesk.com/api/v2/tickets.json", content)

    '    Dim responseString = Await response.Content.ReadAsStringAsync()
    '    If response.IsSuccessStatusCode Then
    '        Dim responseObject = JsonConvert.DeserializeObject(Of ZendeskTicketResponse)(responseString)
    '        'HttpContext.Current.Response.Write("Ticket ID: " & responseObject.ticket.id)
    '        'HttpContext.Current.Response.Write("Ticket Status: " & responseObject.ticket.status)
    '        'HttpContext.Current.Response.Write("JSON Payload: " & json)
    '        MSGBoxCtrl.Show("Alert",
    '                        "Ticket created successfully",
    '                        "Ticket ID: " & responseObject.ticket.id,
    '                        MsgBoxStyle.OkOnly,
    '                        "")

    '        txtName.Text = ""
    '        txtEmail.Text = ""
    '        txtSubject.Text = ""
    '        txtDescription.Text = ""
    '    Else
    '        Dim errorMessage = Await response.Content.ReadAsStringAsync()
    '        ' HttpContext.Current.Response.Write("Error: " & response.ReasonPhrase & " - " & errorMessage)
    '    End If
    'End Function


    Private Async Function UploadFileToZendesk(fileBytes As Byte(), fileName As String) As Task(Of String)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim client As New HttpClient()
        Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes("amar@bytzsoft.com/token:mlBqMCjiqnQAti0raihPWq0pDku81RetiiK9J3Rb"))
        client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Basic", credentials)

        Dim content As New ByteArrayContent(fileBytes)
        content.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/octet-stream")

        Dim uploadUrl As String = $"https://bytzsoft.zendesk.com/api/v2/uploads.json?filename={fileName}"
        Dim response = Await client.PostAsync(uploadUrl, content)
        Dim result = Await response.Content.ReadAsStringAsync()

        If response.IsSuccessStatusCode Then
            Dim json As JObject = JObject.Parse(result)
            Return json("upload")("token").ToString()
        Else
            Return Nothing
        End If
    End Function

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
End Class

Public Class ZendeskTicketResponse
    Public Property ticket As Ticket
End Class

Public Class Ticket
    Public Property id As Integer
    Public Property url As String
    Public Property subject As String
    Public Property status As String
End Class