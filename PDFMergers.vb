

Imports System.Collections.Generic

Imports iTextSharp.text
Imports iTextSharp.text.pdf


Public NotInheritable Class PDFMergers
	Private Sub New()
	End Sub
	'' <summary>
	'' Merge pdf files.
	'' </summary>
	'' <param name="sourceFiles">PDF files being merged.</param>
	'' <returns></returns>
	Public Shared Function MergeFiles(ByVal sourceFiles As List(Of Byte())) As Byte()
		Dim document As iTextSharp.text.Document = New iTextSharp.text.Document()
		Dim output As New MemoryStream()
		Try
			Try
				' Initialize pdf writer
				Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
				writer.PageEvent = New PdfPageEvents()

				' Open document to write
				document.Open()
				Dim content As PdfContentByte = writer.DirectContent

				' Iterate through all pdf documents
				For fileCounter As Integer = 0 To sourceFiles.Count - 1
					' Create pdf reader
					Dim reader As New PdfReader(sourceFiles(fileCounter))
					Dim numberOfPages As Integer = reader.NumberOfPages

					' Iterate through all pages
					For currentPageIndex As Integer = 1 To numberOfPages
						' Determine page size for the current page
						document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex))
						' Create page
						document.NewPage()
						Dim importedPage As PdfImportedPage = writer.GetImportedPage(reader, currentPageIndex)
						' Determine page orientation
						Dim pageOrientation As Integer = reader.GetPageRotation(currentPageIndex)
						If (pageOrientation = 90) OrElse (pageOrientation = 270) Then
							content.AddTemplate(importedPage, 0, -1.0F, 1.0F, 0, 0, _
							 reader.GetPageSizeWithRotation(currentPageIndex).Height)
						Else
							content.AddTemplate(importedPage, 1.0F, 0, 0, 1.0F, 0, _
							 0)
						End If
					Next
				Next
			Catch exception As Exception
				Throw New Exception("There has an unexpected exception occured during the pdf merging process.", exception)
			End Try
		Finally
			document.Close()
		End Try
		Return output.GetBuffer()
	End Function
End Class

'' <summary>
'' Implements custom page events.
'' </summary>
Friend Class PdfPageEvents
	Implements IPdfPageEvent

#Region "members"

	Private _baseFont As BaseFont = Nothing
	Private _content As PdfContentByte

#End Region

#Region "IPdfPageEvent Members"

	Public Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
		_baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
		_content = writer.DirectContent
	End Sub

	Public Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
	End Sub

	Public Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
		' Write header text
		Dim headerText As String = "PDF Merger by Smart-Soft"
		_content.BeginText()
		_content.SetFontAndSize(_baseFont, 8)
		_content.SetTextMatrix(GetCenterTextPosition(headerText, writer), writer.PageSize.Height - 10)
		_content.ShowText(headerText)
		_content.EndText()

		' Write footer text (page numbers)
		Dim text As String = "Page " & Convert.ToString(writer.PageNumber)
		_content.BeginText()
		_content.SetFontAndSize(_baseFont, 8)
		_content.SetTextMatrix(GetCenterTextPosition(text, writer), 10)
		_content.ShowText(text)
		_content.EndText()
	End Sub

	Public Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
	End Sub

	Public Sub OnParagraph(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single)
	End Sub

	Public Sub OnParagraphEnd(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single)
	End Sub

	Public Sub OnChapter(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single, ByVal title As Paragraph)
	End Sub

	Public Sub OnChapterEnd(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single)
	End Sub

	Public Sub OnSection(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single, ByVal depth As Integer, ByVal title As Paragraph)
	End Sub

	Public Sub OnSectionEnd(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single)
	End Sub

	Public Sub OnGenericTag(ByVal writer As PdfWriter, ByVal document As Document, ByVal rect As Rectangle, ByVal text As String)
	End Sub

#End Region

	Private Function GetCenterTextPosition(ByVal text As String, ByVal writer As PdfWriter) As Single
		Return writer.PageSize.Width / 2 - _baseFont.GetWidthPoint(text, 8) / 2
	End Function

	Public Sub OnChapter1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single, ByVal title As iTextSharp.text.Paragraph) Implements iTextSharp.text.pdf.IPdfPageEvent.OnChapter

	End Sub

	Public Sub OnChapterEnd1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnChapterEnd

	End Sub

	Public Sub OnCloseDocument1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnCloseDocument

	End Sub

	Public Sub OnEndPage1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnEndPage

	End Sub

	Public Sub OnGenericTag1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal rect As iTextSharp.text.Rectangle, ByVal text As String) Implements iTextSharp.text.pdf.IPdfPageEvent.OnGenericTag

	End Sub

	Public Sub OnOpenDocument1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnOpenDocument

	End Sub

	Public Sub OnParagraph1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnParagraph

	End Sub

	Public Sub OnParagraphEnd1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnParagraphEnd

	End Sub

	Public Sub OnSection1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single, ByVal depth As Integer, ByVal title As iTextSharp.text.Paragraph) Implements iTextSharp.text.pdf.IPdfPageEvent.OnSection

	End Sub

	Public Sub OnSectionEnd1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnSectionEnd

	End Sub

	Public Sub OnStartPage1(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnStartPage

	End Sub
End Class
