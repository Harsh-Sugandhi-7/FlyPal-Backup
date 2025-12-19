'************************************
'Modified by Harsh Sugandhi
'************************************


Partial Class CrystalReports
	Inherits Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable Declaration "

	Dim CompanyLogo As String
	Dim DataSet As String

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load


		Try

			If Session("PrintReportWithAttachment") = "True" Then

				Dim CrystalReport As String

				Session("PrintReportWithAttachment") = "False"

				CrystalReport = If(Session("CrystalReport"), Session("myReport"))

				Response.ClearContent()
				Response.ClearHeaders()
				Response.ContentType = "Application/pdf"
				Response.WriteFile(CrystalReport)
				Response.Flush()
				File.Delete(CrystalReport)
				Response.End()

			Else

				Dim CrystalReport As Engine.ReportClass
				Dim ExportOptions As ExportOptions
				Dim DiskFileDestinationOptions As DiskFileDestinationOptions
				Dim DiskFileName As String
				Dim RepNo As Integer
				Dim RandomNumber As New Random
				Dim FilePath, VirtualDirectoryName As String

				CompanyLogo = AppSettings("FilePath") & "\ABC1" & ".bmp"
				DataSet = AppSettings("FilePath") & "\dsQuotation.xsd"

				CrystalReport = CType(If(Session("CrystalReport"), Session("myReport")), Engine.ReportClass)

				If CType(Session("RepNo"), String) = "" Then
					RepNo = 1
					Session("RepNo") = RepNo
				Else
					RepNo = CType(Session("RepNo"), Integer)
					RepNo += 1
				End If

				FilePath = "C:\Temp"                                                     ' Set the path.
				VirtualDirectoryName = Dir(FilePath, vbDirectory)       ' Retrieve the first entry.

				If VirtualDirectoryName = "" Then                                        ' The folder is not there & to be created
					MkDir("C:\Temp\")                                               ' Folder created
				End If

				If CBool(Session("IsExcel")) Then

					If Session("PrintInfoBoard") = "PrintInfoBoard" Then

						Dim StringWriter As New StringWriter
						StringWriter = Session("tw")
						Session("PrintInfoBoard") = ""
						Response.ContentType = "application/vnd.ms-excel"
						Me.EnableViewState = False
						Response.Write(StringWriter.ToString())
						Response.End()

					Else

						DiskFileName = "C:\Temp\Rep" & RandomNumber.Next & ".xls"
						DiskFileDestinationOptions = New DiskFileDestinationOptions With {
							.DiskFileName = DiskFileName
						}
						ExportOptions = CrystalReport.ExportOptions

						With ExportOptions
							.DestinationOptions = DiskFileDestinationOptions
							.ExportDestinationType = .ExportDestinationType.DiskFile
							.ExportFormatType = .ExportFormatType.Excel
						End With

						CrystalReport.Export()
						Response.Clear()
						Response.ClearContent()
						Response.ClearHeaders()
						Response.ContentType = "application/x-msexcel"
						Response.Expires = 0
						Response.AddHeader("content-disposition", "attachment;filename=" & DiskFileName)
						Response.WriteFile(DiskFileName)
						Response.Flush()
						Response.End()
						File.Delete(DiskFileName)

					End If

					Session("IsExcel") = False

				Else

					If Session("PrintInfoBoard") = "PrintInfoBoard" Then

						Dim StringWriter As New StringWriter
						StringWriter = Session("tw")
						Session("PrintInfoBoard") = ""
						Response.ClearContent()
						Response.ClearHeaders()
						Response.ContentType = "application/pdf"
						Me.EnableViewState = False
						Response.Write(StringWriter.ToString())
						Response.Flush()
						Response.End()

					Else

						DiskFileName = "C:\Temp\Rep" & RandomNumber.Next & ".PDF"
						DiskFileDestinationOptions = New DiskFileDestinationOptions With {
							.DiskFileName = DiskFileName
						}
						ExportOptions = CrystalReport.ExportOptions

						With ExportOptions
							.DestinationOptions = DiskFileDestinationOptions
							.ExportDestinationType = .ExportDestinationType.DiskFile
							.ExportFormatType = .ExportFormatType.PortableDocFormat
						End With

						CrystalReport.Export()
						CrystalReport.Close()
						CrystalReport.Dispose()
						Response.ClearContent()
						Response.ClearHeaders()


						If Session("ShowWatermark") = "True" Then

							Session.Remove("ShowWatermark")
							Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

							AddWatermarkText(DiskFileName,
											 MergedPath_WM,
											 "PREVIEW", , ,
											 iTextSharp.text.BaseColor.GRAY, ,
											 0.0, 0,
											 ShowWatermarkOnCenter:=True)

							DiskFileName = MergedPath_WM

						End If

						Dim FileName As String = DiskFileName.Split("\Rep")(2)
						Response.ContentType = "Application/pdf"

						Response.AppendHeader("Content-Disposition", "inline; filename=" & FileName)
						Response.WriteFile(DiskFileName)
						Response.Flush()

						File.Delete(DiskFileName)
						Response.End()

					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Page_Unload(sender As Object, e As EventArgs) Handles MyBase.Unload

		If CompanyLogo IsNot Nothing Then File.Delete(CompanyLogo)
		If DataSet IsNot Nothing Then File.Delete(DataSet)

	End Sub

#End Region

End Class
