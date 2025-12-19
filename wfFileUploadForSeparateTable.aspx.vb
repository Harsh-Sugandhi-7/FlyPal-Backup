'*************************************************
'Created By Utkarsh ON 13-Nov-2013 
'Modified by Harsh Sugandhi on 24th Oct 2025
'*************************************************

Public Class wfFileUploadForSeparateTable
	Inherits Page

	Dim FileAttach As FileAttach 'Added by Vikrant On 25-Nov-2014

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try

			If Not IsPostBack Then

				'Added By Vikrant On 04-Jun-2014 For All03062014-1
				If Session("ShowNotification") = True Then
					lblMessage.Visible = True
				Else
					lblMessage.Visible = False
				End If
				'ENd
				ClientScript.RegisterStartupScript([GetType], "Page Load Script", "OnPageLoad();", True)

			End If

			If FileUpload.HasFile Then

				Try

					'Added by Vikrant On 25-Nov-2014
					FileAttach = Session("mFileAttach")

					If FileAttach IsNot Nothing Then

						FileAttach.Extension = Mid(FileUpload.PostedFile.FileName, FileUpload.PostedFile.FileName.LastIndexOf(".") + 1)
						FileAttach.Size = FileUpload.PostedFile.ContentLength
						FileAttach.ImageFile = FileUpload.FileBytes
						FileAttach.FileName = Split(FileUpload.FileName, ".")(0)

						Session("mFileAttach") = FileAttach

					Else

						Session("Extension") = Mid(FileUpload.PostedFile.FileName, FileUpload.PostedFile.FileName.LastIndexOf(".") + 1)
						Session("Size") = FileUpload.PostedFile.ContentLength
						Session("ImageFile") = FileUpload.FileBytes
						Session("FileName") = Split(FileUpload.FileName, ".")(0)

					End If

					Session("FileUpload.FileName") = Split(FileUpload.FileName, ".")(0)

					ClientScript.RegisterStartupScript([GetType],
													   "On Uploading",
													   "onUploadComplete(true);",
													   True)

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