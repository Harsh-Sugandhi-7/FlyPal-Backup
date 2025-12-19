'************************************
'Created by:	Harsh Sugandhi
'Created on:	16th October 2025
'Created for:	To handle the Attachment name while View / Download Attachment.
'************************************


Imports Newtonsoft.Json.Linq


Public Class AttachmentHelper


#Region " Variable(s) Declarations "

	Private _MessageBox As New MSGBox

#End Region

#Region " Helper Method(s) "

	Public Function DownloadAttachmentWithName(AttachmentObject As Object,
											   Optional Index As Integer = 0,
											   Optional ModuleName As String = "")

		Dim AttachmentName, Extension As String
		Dim DocPath As String = $"{AppSettings("DOCPath")}"
		Dim CompletePathForDownload As String
		Dim FileStream As FileStream

		Try

			Select Case ModuleName
				Case "TroubleShoot"

					AttachmentObject = CType(AttachmentObject, LogMaintenance)
					AttachmentName = HttpContext.Current.Session("FileUpload.FileName")
					Extension = AttachmentObject.FileExtension

				Case "WOJobTask"

					AttachmentObject = CType(AttachmentObject, TaskCardAttachments)

					If AttachmentObject.TaskCardAttachments.Count = 1 Then
						AttachmentObject.TaskCardAttachments.CurrentIndex = 0
					Else
						AttachmentObject.TaskCardAttachments.CurrentIndex = Index - 1
					End If

					AttachmentName = AttachmentObject.TaskCardAttachments.CurrentItem.FileName
					Extension = AttachmentObject.FileExtension

					AttachmentObject = AttachmentObject.TaskCardAttachments.CurrentItem

				Case "Multiple Attachments"

					AttachmentObject = CType(AttachmentObject, FileAttachments)

					If AttachmentObject.Count = 1 Then
						AttachmentObject.CurrentIndex = 0
					Else
						AttachmentObject.CurrentIndex = Index - 1
					End If

					AttachmentName = AttachmentObject.CurrentItem.FileName.Split(".")(0)
					Extension = AttachmentObject.CurrentItem.Extension

					AttachmentObject = AttachmentObject.CurrentItem

				Case "Employee"

					AttachmentObject = CType(AttachmentObject, Employee)
					AttachmentName = AttachmentObject.FileName.Split(".")(0)
					Extension = AttachmentObject.Extension

				Case Nothing, ""

					AttachmentObject = CType(AttachmentObject, FileAttach)
					AttachmentName = AttachmentObject.FileName.Split(".")(0)
					Extension = AttachmentObject.Extension

			End Select

			AttachmentName = $"{If(String.IsNullOrEmpty(AttachmentName), "Attachment", AttachmentName)}{Extension}"

			If AttachmentObject.Size > 0 Then

				CompletePathForDownload = $"{DocPath}{AttachmentName}"

				If Not File.Exists(path:=$"{DocPath}") Then

					'Delete File if exist
					File.Delete(path:=CompletePathForDownload)

					'Create the file.
					FileStream = File.Create(path:=CompletePathForDownload)

					'Add some information to the file.
					FileStream.Write(AttachmentObject.ImageFile, 0, AttachmentObject.ImageFile.Length)
					FileStream.Close()

					HttpContext.Current.Session("DOCPath") = CompletePathForDownload
					HttpContext.Current.Session("AttachmentName") = AttachmentName

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function SaveAttachments(AttachmentArray As JArray,
									ModuleObject As Object,
									ModuleName As String) As (Object, String)

		Dim returnMessage As String = String.Empty
		Try

			Select Case ModuleName
				Case "RCI"
					ModuleObject = CType(ModuleObject, ReceiptCumInvoice)
				Case "Order"
					ModuleObject = CType(ModuleObject, Order)
			End Select

			For i As Integer = 0 To AttachmentArray.Count - 1

				Dim ID As New Guid(AttachmentArray(i)("mID").ToString)
				Dim IsNew As Boolean = CBool(AttachmentArray(i)("mIsNew"))
				Dim IsDirty As Boolean = CBool(AttachmentArray(i)("mIsDirty"))
				Dim IsDeleted As Boolean = CBool(AttachmentArray(i)("mIsDeleted"))

				If IsNew AndAlso IsDirty Then

					If Not ModuleObject.FileAttachments.Contains(ReferenceID:=ModuleObject.ID,
																 FileName:=AttachmentArray(i)("mFileName")) Then

						ModuleObject.FileAttachments.Add(ReferenceID:=ModuleObject.ID,
														 FileName:=AttachmentArray(i)("mFileName"))

						With ModuleObject.FileAttachments.CurrentItem

							.ImageFile = CType(AttachmentArray(i)("mImageFile"), Byte())
							.Size = CInt(AttachmentArray(i)("mSize").ToString)
							.Extension = AttachmentArray(i)("mExtension")
							.FileName = AttachmentArray(i)("mFileName")

						End With

					Else

						returnMessage = _MessageBox.MessageBoxForAPI(MSGBox.Message_Text.Duplicate)
						returnMessage = returnMessage.Replace("<p>", "").
													  Replace("</p>", "")

						Return (ModuleObject, returnMessage)

					End If

				End If

				If IsDeleted Then
					ModuleObject.FileAttachments.Remove(ID:=ID, overloadParam:="")
				End If

			Next

			ModuleObject.IsAttachmentAdded = ModuleObject.FileAttachments.Count > 0

			Return (ModuleObject, "")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class
