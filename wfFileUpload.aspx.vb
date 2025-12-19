'*************************************************
'Created By Utkarsh ON 13-Nov-2013 
'Modified by Harsh Sugandhi on 24th Oct 2025
'*************************************************


Public Class wfFileUpload
	Inherits Page

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try

			If Not IsPostBack Then

				'Added By Vikrant On 04-Jun-2014 For All03062014-1
				If Session("ShowNotification") = True Then
					lblMessage.Visible = True
				Else
					lblMessage.Visible = False
				End If

				ClientScript.RegisterStartupScript([GetType], "Page Load Script", "OnPageLoad();", True)

			End If

			If FileUpload.HasFile Then

				Try

					Session("FileUpload.FileExtension") = Mid(FileUpload.PostedFile.FileName, FileUpload.PostedFile.FileName.LastIndexOf(".") + 1)
					Session("FileUpload.FileSize") = FileUpload.PostedFile.ContentLength
					Session("FileUpload.FileContent") = FileUpload.FileBytes
					Session("FileUpload.FileName") = Mid(FileUpload.PostedFile.FileName, FileUpload.PostedFile.FileName.LastIndexOf("\") + 2)

					ClientScript.RegisterStartupScript([GetType], "On Uploading", "onUploadComplete(true);", True)

				Catch ex As Exception

					ClientScript.RegisterStartupScript([GetType],
													   "Alert Script",
													   "alert(" + ex.Message + ");",
													   True)

				End Try

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
End Class