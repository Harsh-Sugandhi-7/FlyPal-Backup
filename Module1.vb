Imports System.IO
Imports Org.pdfbox.pdmodel
Imports Org.pdfbox.util
Imports Org.pdfbox.pdmodel.interactive.documentnavigation.destination
Imports Org.pdfbox.pdmodel.interactive.documentnavigation.outline
Imports Org.pdfbox.pdfwriter
Imports Org.pdfbox.pdmodel.edit
Imports java.util

Imports System
Imports System.Text
Imports iTextSharp.text.pdf.parser


Module Module1

#Region "Meging of PDF - Using PDFBox"
    Public Function StartMergingPDF(ByVal pdfList As System.Collections.ArrayList) As String

        Dim outFile As String = "C:\Temp\" & "temp_myMergedPdf.pdf"

        'Try to merge the pdf files
        Dim MergeResult As Boolean = MergePdfFiles(pdfList, outFile)
        Dim BookmarkResult As Boolean = False
        If MergeResult Then
            'If successful, create bookmark data
            Dim bookmarkData As DataTable = CreateBookmarkDataTable(pdfList)
            'If successful, try to add bookmarks to the merged file


            If Not IsNothing(bookmarkData) Then
                BookmarkResult = AddBookMarks(outFile, bookmarkData)
                If Not BookmarkResult Then
                    Console.WriteLine("It blew....")
                    Console.ReadLine()
                Else
                    'Add bookmarks OKed, delete the temp file
                    File.Delete(outFile)
                End If
            End If
        End If

        If MergeResult And BookmarkResult Then
            Return (outFile.Replace("temp_", ""))
        Else
            Return ""
        End If
    End Function

    Private Function MergePdfFiles(ByVal pdfFileList As System.Collections.ArrayList,
                                   ByVal outputFileFullName As String) As Boolean
        Dim result As Boolean = False
        Dim pdfMerger As PDFMergerUtility = Nothing
        Dim fileCount As Integer = pdfFileList.Count
        If fileCount >= 1 Then
            Try
                'Instantiate an instance of Pdf Merger Utility
                pdfMerger = New PDFMergerUtility
                With pdfMerger
                    'Set output destination
                    .setDestinationFileName(outputFileFullName)
                    'Looping thru the file list and add source to the merger
                    For i As Integer = 0 To fileCount - 1 Step 1
                        .addSource(pdfFileList(i))
                    Next i
                    'Merge the documents
                    pdfMerger.mergeDocuments()
                    result = True
                End With
            Catch ex As Exception
                WriteToLog("MergePDFFile(" & outputFileFullName & "): " & ex.Message)
                Return False
            End Try
        End If
        Return result
    End Function

    Private Function CreateBookmarkDataTable(ByVal pdfFileList As System.Collections.ArrayList) As DataTable
        Dim bookmarkData As New DataTable
        Dim row As DataRow = Nothing
        Dim bookmarkTitle As String = String.Empty
        Dim pageNumber As Integer = 0
        Try
            bookmarkData.Columns.Add("BookmarkTitle", GetType(String))
            bookmarkData.Columns.Add("PageNumber", GetType(Integer))
            Dim count As Integer = pdfFileList.Count
            If count > 0 Then
                For i As Integer = 0 To count - 1 Step 1
                    bookmarkTitle = Path.GetFileNameWithoutExtension(pdfFileList(i))
                    row = bookmarkData.NewRow()
                    row.Item("BookmarkTitle") = bookmarkTitle
                    row.Item("PageNumber") = pageNumber
                    bookmarkData.Rows.Add(row)
                    pageNumber += GetPageCount(pdfFileList(i))
                Next
            End If
        Catch ex As Exception
            WriteToLog("CreateBookmarkDataTable(): " & ex.Message)
            Return Nothing
        End Try
        Return bookmarkData
    End Function

    Private Function GetPageCount(ByVal pdfFile As String) As Integer
        Dim pageCount As Integer
        Dim pdfDoc As PDDocument = Nothing
        Try
            pdfDoc = PDDocument.load(pdfFile)
            pageCount = pdfDoc.getNumberOfPages
        Catch ex As Exception
            WriteToLog("GetPageCount(" & pdfFile & "): " & ex.Message)
            Return 0
        Finally
            If Not pdfDoc Is Nothing Then
                pdfDoc.close()
            End If
        End Try
        Return pageCount
    End Function

    Private Function AddBookMarks(ByVal pdfFile As String,
                                  ByVal bookmarkTable As DataTable) As Boolean
        Dim result As Boolean = False
        Dim PdfDoc As PDDocument = Nothing
        Dim outFile As String = String.Empty
        Dim rowCount As Integer = bookmarkTable.Rows.Count
        Try
            If rowCount > 0 Then
                'Set the output file full path
                outFile = pdfFile.Replace("temp_", "")
                'Load the input pdf file
                PdfDoc = PDDocument.load(pdfFile)
                If Not PdfDoc.isEncrypted() Then
                    'Create new document outline and assign it to the pdf document
                    Dim outline As PDDocumentOutline = New PDDocumentOutline
                    PdfDoc.getDocumentCatalog().setDocumentOutline(outline)

                    'Create new outline item for the document outline
                    Dim pagesOutline As PDOutlineItem = New PDOutlineItem
                    pagesOutline.setTitle("All Pages")
                    outline.appendChild(pagesOutline)

                    'Get the list of pages in the document
                    Dim pages As List = PdfDoc.getDocumentCatalog().getAllPages()

                    Dim i, pageNumber As Integer
                    Dim row As DataRow = Nothing
                    Dim bookmarkTitle As String = String.Empty
                    'loop thru the bookmark datatable and add bookmarks to the document accordingly
                    For i = 0 To rowCount - 1 Step 1
                        'Read the row's data
                        row = bookmarkTable.Rows(i)
                        pageNumber = CInt(row.Item("PageNumber"))
                        bookmarkTitle = CStr(row.Item("BookmarkTitle"))
                        'Get the page at pageNumber from pages list
                        Dim page As PDPage = CType(pages.get(pageNumber), PDPage)

                        'Dim a As org.pdfbox.pdmodel.edit.PDPageContentStream = New org.pdfbox.pdmodel.edit.PDPageContentStream(PdfDoc, page, True, True)
                        'a.beginText()
                        'a.drawString("Kalpesh Shah")
                        'a.endText()

                        Dim dest As PDPageFitWidthDestination = New PDPageFitWidthDestination
                        dest.setPage(page)
                        'Then set bookmark to it
                        Dim bookmark As PDOutlineItem = New PDOutlineItem
                        bookmark.setDestination(dest)
                        bookmark.setTitle(bookmarkTitle)
                        'Add this bookmark to the document's outline
                        pagesOutline.appendChild(bookmark)
                    Next i
                    'Expand the bookmark tree
                    pagesOutline.openNode()
                    outline.openNode()
                    'Save the the document to a file

                    PdfDoc.save(outFile)
                    result = True
                Else
                    WriteToLog("Can't add bookmarks to <" & pdfFile & "> because the document is encrypted.")
                End If
            Else
                WriteToLog("Can't add bookmarks to <" & pdfFile & "> because BookmarkTable has no data.")
            End If
        Catch ex As Exception
            WriteToLog("AddBookmarks(" & pdfFile & "): " & ex.Message)
            Return False
        Finally
            If Not PdfDoc Is Nothing Then
                PdfDoc.close()
            End If
        End Try
        Return result
    End Function

    Private Sub WriteToLog(ByVal txt As String)
        Dim logPath As String = Environment.CurrentDirectory & "\Log"
        Try
            If Not Directory.Exists(logPath) Then
                Directory.CreateDirectory(logPath)
            End If
            Dim logFile As String = logPath & "\PdfMerger.log"
            Dim prefix As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss - ")

            'Using writer As New StreamWriter(logFile, True)
            'writer.WriteLine(prefix & txt)
            'End Using

            Dim writer As StreamWriter = New StreamWriter(logFile, True)
            With writer
                writer.WriteLine(prefix & txt)
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try

    End Sub
#End Region

#Region "Water Mark - Using iSharp"
    '' <summary>    
    '' Add and image as the watermark on each page of the source pdf to create a new pdf with watermark    
    '' </summary>    
    '' <param name="sourceFile">the full path to the source pdf'' </param>    
    '' <param name="outputFile">the full path where the watermarked pdf will be saved to</param>    
    '' <param name="watermarkImage">the full path to the image file to use as the watermark</param>    
    '' <remarks>
    '' The watermark image will be align in the center of each page
    '' </remarks>    
    Public Sub AddWatermarkImage(ByVal sourceFile As String, ByVal outputFile As String, ByVal watermarkImage As String)
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim stamper As iTextSharp.text.pdf.PdfStamper = Nothing
        Dim img As iTextSharp.text.Image = Nothing
        Dim underContent As iTextSharp.text.pdf.PdfContentByte = Nothing
        Dim rect As iTextSharp.text.Rectangle = Nothing
        Dim X, Y As Single
        Dim pageCount As Integer = 0
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourceFile)
            rect = reader.GetPageSizeWithRotation(1)
            stamper = New iTextSharp.text.pdf.PdfStamper(reader, New System.IO.FileStream(outputFile, IO.FileMode.Create))
            img = iTextSharp.text.Image.GetInstance(watermarkImage)
            If img.Width > rect.Width OrElse img.Height > rect.Height Then
                img.ScaleToFit(rect.Width, rect.Height)
                X = (rect.Width - img.ScaledWidth) / 2
                Y = (rect.Height - img.ScaledHeight) / 2
            Else : X = (rect.Width - img.Width) / 2
                Y = (rect.Height - img.Height) / 2
            End If
            img.SetAbsolutePosition(X, Y)
            pageCount = reader.NumberOfPages()
            For i As Integer = 1 To pageCount
                underContent = stamper.GetUnderContent(i)
                underContent.AddImage(img)
            Next
            stamper.Close()
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '20
    '15,15
    '' <summary>    
    '' '' Add text as the watermark to each page of the source pdf to create a new pdf with text watermark    
    '' '' </summary>    
    '' '' <param name="sourceFile">the full path to the source pdf file</param>    
    '' '' <param name="outputFile">the full path where the watermarked pdf file will be saved to</param>    
    '' '' <param name="watermarkText">the text to use as the watermark</param>    
    '' '' <param name="watermarkFont">the font to use for the watermark. The default font is HELVETICA</param>    
    '' '' <param name="watermarkFontSize">the size of the font. The default size is 48</param>    
    '' '' <param name="watermarkFontColor">the color of the watermark. The default color is blue</param>    
    '' '' <param name="watermarkFontOpacity">the opacity of the watermark. The default opacity is 0.3</param>    
    '' '' <param name="watermarkRotation">the rotation in degree of the watermark. The default rotation is 45 degree</param>    
    '' '' <remarks></remarks>    
    Public Function AddWatermarkText(ByVal sourceFile As String, ByVal outputFile As String, ByVal watermarkText As String,
    Optional ByVal watermarkFont As iTextSharp.text.pdf.BaseFont = Nothing,
    Optional ByVal watermarkFontSize As Single = 14,
    Optional ByVal watermarkFontColor As iTextSharp.text.BaseColor = Nothing,
    Optional ByVal watermarkFontOpacity As Single = 0.3F,
    Optional ByVal watermarkRotation As Single = 90.0F,
    Optional ByVal PrevPageCount As Integer = 0,
    Optional ByVal ShowWatermarkOnCenter As Boolean = False,
    Optional ByVal ReportName As String = "") As Integer        'Open this line to show WaterMark Veritcal
        'Optional ByVal watermarkRotation As Single = 0) '45.0F)'Open this line to show WaterMark Horizontal 


        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim stamper As iTextSharp.text.pdf.PdfStamper = Nothing
        Dim gstate As iTextSharp.text.pdf.PdfGState = Nothing
        Dim underContent As iTextSharp.text.pdf.PdfContentByte = Nothing
        Dim rect As iTextSharp.text.Rectangle = Nothing

        Dim pageCount As Integer = 0

        Try

            reader = New iTextSharp.text.pdf.PdfReader(sourceFile)
            rect = reader.GetPageSizeWithRotation(1)
            stamper = New iTextSharp.text.pdf.PdfStamper(reader, New System.IO.FileStream(outputFile, IO.FileMode.Create))
            If watermarkFont Is Nothing Then
                watermarkFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA,
                iTextSharp.text.pdf.BaseFont.CP1252,
                iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED)
            End If
            If watermarkFontColor Is Nothing Then
                watermarkFontColor = iTextSharp.text.BaseColor.BLUE
            End If
            gstate = New iTextSharp.text.pdf.PdfGState
            gstate.FillOpacity = watermarkFontOpacity
            gstate.StrokeOpacity = watermarkFontOpacity
            pageCount = reader.NumberOfPages()

            For i As Integer = 1 To pageCount

                underContent = stamper.GetUnderContent(i)
                rect = reader.GetPageSizeWithRotation(i)
                With underContent
                    .SaveState()
                    .SetGState(gstate)
                    .SetColorFill(watermarkFontColor)
                    .BeginText()
                    .SetFontAndSize(watermarkFont, watermarkFontSize)
                    .SetTextMatrix(30, 30) '30,30

                    'Commented by Yogita becaz Bharti wants WaterMark at Center. Using this line WaterMArk will come at Right Top Corner of Page.
                    '.ShowTextAligned(iTextSharp.text.Element.ALIGN_RIGHT, watermarkText, rect.Width - 50, rect.Height - 40, watermarkRotation)

                    If ShowWatermarkOnCenter Then
                        .SetFontAndSize(watermarkFont, 70)
                        'Open this line to show WaterMark at the Center of Page.
                        .ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, watermarkText, rect.Width / 2, rect.Height / 2, watermarkRotation)

                    Else
                        'Using this line WaterMark will come at Bottom Middle (half cm from Bottom) Side of Page.
                        If ReportName = "EmpCAAuthorization" Then
                            .ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, watermarkText & " #" & (i + PrevPageCount).ToString, rect.Width / 2, rect.Height / 88, watermarkRotation)
                        Else
                            .ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, watermarkText & " #" & (i + PrevPageCount).ToString, rect.Width / 2, rect.Height / 44, watermarkRotation)
                        End If
                    End If


                    'Using this line WaterMark will come at left Middle (1cm from Left) Side of Page.       '15
                    '.ShowTextAligned(iTextSharp.text.Element.ALIGN_LEFT, watermarkText & "#" & (i + PrevPageCount).ToString, rect.Width / 17, rect.Height / 2, watermarkRotation)

                    'Using this line WaterMark will come at Bottom Middle (4cm from Bottom) Side of Page.
                    '.ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, watermarkText & " #" & (i + PrevPageCount).ToString, rect.Width / 2, rect.Height / 8, watermarkRotation)



                    .EndText()
                    .RestoreState()
                End With
            Next
            stamper.Close()
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try


        Return (pageCount + PrevPageCount)

    End Function
    '' <summary>    
    '' '' Add text as the watermark to each page of the source pdf to create a new pdf with text watermark    
    '' '' </summary>    
    '' '' <param name="sourceFile">the full path to the source pdf file</param>    
    '' '' <param name="outputFile">the full path where the watermarked pdf file will be saved to</param>    
    '' '' <param name="watermarkText">the string array conntaining the text to use as the watermark. 
    '' Each element is treated as a line in the watermark</param>    
    '' '' <param name="watermarkFont">the font to use for the watermark. The default font is HELVETICA</param>    
    '' '' <param name="watermarkFontSize">the size of the font. The default size is 48</param>    
    '' '' <param name="watermarkFontColor">the color of the watermark. The default color is blue</param>    
    '' '' <param name="watermarkFontOpacity">the opacity of the watermark. The default opacity is 0.3</param>    
    '' '' <param name="watermarkRotation">the rotation in degree of the watermark. 
    '' The default rotation is 45 degree</param>    
    '' '' <remarks></remarks>    
    Public Sub AddWatermarkText(ByVal sourceFile As String, ByVal outputFile As String, ByVal watermarkText() As String,
                                  Optional ByVal watermarkFont As iTextSharp.text.pdf.BaseFont = Nothing,
                                  Optional ByVal watermarkFontSize As Single = 48,
                                  Optional ByVal watermarkFontColor As iTextSharp.text.BaseColor = Nothing,
                                  Optional ByVal watermarkFontOpacity As Single = 0.3F,
                                  Optional ByVal watermarkRotation As Single = 45.0F)

        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim stamper As iTextSharp.text.pdf.PdfStamper = Nothing
        Dim gstate As iTextSharp.text.pdf.PdfGState = Nothing
        Dim underContent As iTextSharp.text.pdf.PdfContentByte = Nothing
        Dim rect As iTextSharp.text.Rectangle = Nothing
        Dim currentY As Single = 0.0F
        Dim offset As Single = 0.0F
        Dim pageCount As Integer = 0
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourceFile)
            rect = reader.GetPageSizeWithRotation(1)
            stamper = New iTextSharp.text.pdf.PdfStamper(reader, New System.IO.FileStream(outputFile, IO.FileMode.Create))
            If watermarkFont Is Nothing Then
                watermarkFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA,
                iTextSharp.text.pdf.BaseFont.CP1252,
                iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED)
            End If
            If watermarkFontColor Is Nothing Then

                watermarkFontColor = iTextSharp.text.BaseColor.BLUE
            End If
            gstate = New iTextSharp.text.pdf.PdfGState
            gstate.FillOpacity = watermarkFontOpacity
            gstate.StrokeOpacity = watermarkFontOpacity
            pageCount = reader.NumberOfPages()
            For i As Integer = 1 To pageCount

                underContent = stamper.GetOverContent(i)
                With underContent
                    .SaveState()
                    .SetGState(gstate)
                    .SetColorFill(watermarkFontColor)
                    .BeginText()
                    .SetFontAndSize(watermarkFont, watermarkFontSize)
                    .SetTextMatrix(30, 30)
                    If watermarkText.Length > 1 Then
                        currentY = (rect.Height / 2) + ((watermarkFontSize * watermarkText.Length) / 2)
                    Else : currentY = (rect.Height / 2)

                    End If
                    For j As Integer = 0 To watermarkText.Length - 1
                        If j > 0 Then
                            offset = (j * watermarkFontSize) + (watermarkFontSize / 4) * j
                        Else
                            offset = 0.0F
                        End If
                        .ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, watermarkText(j), rect.Width / 2, currentY - offset, watermarkRotation)
                    Next
                    .EndText()
                    .RestoreState()
                End With
            Next
            stamper.Close()
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Extracting page - Using iSharp"
    '' <summary>
    '' Extract the text from pdf pages and return it as a string
    '' </summary>
    '' <param name="sourcePDF">Full path to the source pdf file</param>
    '' <param name="fromPageNum">[Optional] the page number (inclusive) to start text extraction </param>
    '' <param name="toPageNum">[Optional] the page number (inclusive) to stop text extraction</param>
    '' <returns>A string containing the text extracted from the specified pages</returns>
    '' <remarks>If fromPageNum is not specified, text extraction will start from page 1. If
    '' toPageNum is not specified, text extraction will end at the last page of the source pdf file.</remarks>

    Public Function getPageNoByOrderNo(ByVal sourcePDF As String, ByVal OrderNo As String) As Integer
        Dim reader As New iTextSharp.text.pdf.PdfReader(sourcePDF)
        Dim PageNo As Integer = 0
        Dim PageString As String = ""

        For PageNo = 1 To reader.NumberOfPages
            PageString = ""
            PageString = ParsePdfText(sourcePDF, PageNo, PageNo, reader)

            If PageString.IndexOf(OrderNo) = -1 Then
                'Allow to search next Page
            Else
                Return PageNo 'Returning Page No. containing specified Text.
            End If
        Next
    End Function
    Public Function getPageNoBySpecificText(ByVal PageNo As Integer, ByVal sourcePDF As String, ByVal TextToSearch As String) As Integer

        Dim pdfReader As New iTextSharp.text.pdf.PdfReader(sourcePDF)

        For page As Integer = PageNo To pdfReader.NumberOfPages

            Dim strategy As ITextExtractionStrategy = New SimpleTextExtractionStrategy()

            Dim currentText As String = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy)

            currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.[Default], Encoding.UTF8, Encoding.[Default].GetBytes(currentText)))

            'text.Append(currentText)
            If currentText.IndexOf(TextToSearch) = -1 Then
                'Allow to search next Page
            Else
                pdfReader.Close()

                Return page 'Returning Page No. containing specified Text.
            End If

        Next

    End Function


    Public Function ParsePdfText(ByVal sourcePDF As String,
    ByVal fromPageNum As Integer,
    ByVal toPageNum As Integer, ByVal reader As iTextSharp.text.pdf.PdfReader) As String

        Dim sb As New System.Text.StringBuilder
        Try

            Dim pageBytes() As Byte = Nothing
            Dim token As iTextSharp.text.pdf.PRTokeniser = Nothing
            Dim tknType As Integer = -1
            Dim tknValue As String = String.Empty

            If fromPageNum = 0 Then
                fromPageNum = 1
            End If
            If toPageNum = 0 Then
                toPageNum = reader.NumberOfPages
            End If

            If fromPageNum > toPageNum Then
                Throw New ApplicationException("Parameter error: The value of fromPageNum can " &
                "not be larger than the value of toPageNum")
            End If

            For i As Integer = fromPageNum To toPageNum Step 1

                pageBytes = reader.GetPageContent(i)
                Dim a As New iTextSharp.text.pdf.RandomAccessFileOrArray(pageBytes)

                If Not IsNothing(pageBytes) Then

                    token = New iTextSharp.text.pdf.PRTokeniser(a)

                    While token.NextToken()
                        tknType = token.TokenType()
                        tknValue = token.StringValue
                        If tknType = iTextSharp.text.pdf.PRTokeniser.TokType.STRING Then
                            sb.Append(token.StringValue)
                            'I need to add these additional tests to properly add whitespace to the output string
                        ElseIf tknType = 1 AndAlso tknValue = "-600" Then
                            sb.Append(" ")
                        ElseIf tknType = 10 AndAlso tknValue = "TJ" Then
                            sb.Append(" ")
                            'ElseIf tknType = iTextSharp.text.pdf.PRTokeniser.TK_OTHER Then
                            '    sb.Append(token.StringValue)
                        End If
                    End While
                End If
            Next i
        Catch ex As Exception
            MessageBox.Show("Exception occured. " & ex.Message)
            Return String.Empty
        End Try
        Return sb.ToString()
    End Function

    Public Function ParsePdfText(ByVal sourcePDF As String,
    Optional ByVal fromPageNum As Integer = 0,
    Optional ByVal toPageNum As Integer = 0) As String

        Dim sb As New System.Text.StringBuilder
        Try

            Dim reader As New iTextSharp.text.pdf.PdfReader(sourcePDF)
            Dim pageBytes() As Byte = Nothing
            Dim token As iTextSharp.text.pdf.PRTokeniser = Nothing
            Dim tknType As Integer = -1
            Dim tknValue As String = String.Empty

            If fromPageNum = 0 Then
                fromPageNum = 1
            End If
            If toPageNum = 0 Then
                toPageNum = reader.NumberOfPages
            End If

            If fromPageNum > toPageNum Then
                Throw New ApplicationException("Parameter error: The value of fromPageNum can " &
                "not be larger than the value of toPageNum")
            End If

            For i As Integer = fromPageNum To toPageNum Step 1
                pageBytes = reader.GetPageContent(i)

                Dim a As New iTextSharp.text.pdf.RandomAccessFileOrArray(pageBytes)

                If Not IsNothing(pageBytes) Then
                    token = New iTextSharp.text.pdf.PRTokeniser(a)
                    While token.NextToken()
                        tknType = token.TokenType()
                        tknValue = token.StringValue
                        If tknType = iTextSharp.text.pdf.PRTokeniser.TokType.STRING Then
                            sb.Append(token.StringValue)
                            'I need to add these additional tests to properly add whitespace to the output string
                        ElseIf tknType = 1 AndAlso tknValue = "-600" Then
                            sb.Append(" ")
                        ElseIf tknType = 10 AndAlso tknValue = "TJ" Then
                            sb.Append(" ")
                        End If
                    End While
                End If
            Next i
        Catch ex As Exception
            MessageBox.Show("Exception occured. " & ex.Message)
            Return String.Empty
        End Try
        Return sb.ToString()
    End Function

    '' <summary>
    '' Textually compare 2 pdf files page by page and write the difference to a text file.
    '' </summary>
    '' <param name="pdf1">the full path to 1st pdf file</param>
    '' <param name="pdf2">the full path to 2nd pdf file</param>
    '' <param name="resultFile">the full path to the result file</param>
    '' <param name="fromPageNum">page number to start comparing</param>
    '' <param name="toPageNum">page number to stop comparing</param>
    '' <remarks>If no values are specified for fromPageNum and toPageNum, the sub will
    '' compare every page in the input pdfs.</remarks>
    Public Sub ComparePdfs(ByVal pdf1 As String, ByVal pdf2 As String,
    ByVal resultFile As String,
    Optional ByVal fromPageNum As Integer = 0,
    Optional ByVal toPageNum As Integer = 0)
        Try
            'For pdf1
            Dim reader1 As New iTextSharp.text.pdf.PdfReader(pdf1)
            Dim pageCount1 As Integer = reader1.NumberOfPages
            Dim pageBytes1() As Byte = Nothing
            Dim token1 As iTextSharp.text.pdf.PRTokeniser = Nothing
            Dim tknType1 As Integer = -1
            Dim tknValue1 As String = String.Empty

            'For pdf2
            Dim reader2 As New iTextSharp.text.pdf.PdfReader(pdf2)
            Dim pageCount2 As Integer = reader2.NumberOfPages
            Dim pageBytes2() As Byte = Nothing
            Dim token2 As iTextSharp.text.pdf.PRTokeniser = Nothing
            Dim tknType2 As Integer = -1
            Dim tknValue2 As String = String.Empty

            If fromPageNum = 0 Then
                fromPageNum = 1
            End If

            If toPageNum = 0 Then
                toPageNum = Math.Min(pageCount1, pageCount2)
            Else
                If toPageNum > pageCount1 OrElse toPageNum > pageCount2 Then
                    toPageNum = Math.Min(pageCount1, pageCount2)
                End If
            End If

            If fromPageNum > toPageNum Then
                Throw New ApplicationException("Parameter error: The value of fromPageNum can " &
                "not be larger than the value of toPageNum")
            End If

            Dim writer As New System.IO.StreamWriter(resultFile)
            For i As Integer = fromPageNum To toPageNum Step 1
                writer.WriteLine("Differences found in page " & i)

                pageBytes1 = reader1.GetPageContent(i)
                pageBytes2 = reader2.GetPageContent(i)

                Dim a As New iTextSharp.text.pdf.RandomAccessFileOrArray(pageBytes1)
                Dim b As New iTextSharp.text.pdf.RandomAccessFileOrArray(pageBytes2)

                If Not IsNothing(pageBytes1) AndAlso Not IsNothing(pageBytes2) Then
                    token1 = New iTextSharp.text.pdf.PRTokeniser(a)
                    token2 = New iTextSharp.text.pdf.PRTokeniser(b)

                    While token1.NextToken() AndAlso token2.NextToken()

                        tknType1 = token1.TokenType()
                        tknValue1 = token1.StringValue

                        tknType2 = token2.TokenType()
                        tknValue2 = token2.StringValue

                        If tknType1 = iTextSharp.text.pdf.PRTokeniser.TokType.STRING AndAlso
                        tknType2 = iTextSharp.text.pdf.PRTokeniser.TokType.STRING Then
                            If String.Compare(tknValue1, tknValue2) <> 0 Then
                                writer.WriteLine("Pdf1: " & tknValue1 & " <> Pdf2: " & tknValue2)
                            End If
                        End If
                    End While
                End If
            Next i
            writer.Close()
            reader1.Close()
            reader2.Close()
        Catch ex As Exception
            MessageBox.Show("Exception occured. " & ex.Message)
        End Try
    End Sub

    '' <summary>
    '' Extract a single page from source pdf to a new pdf
    '' </summary>
    '' <param name="sourcePdf">the full path to source pdf file</param>
    '' <param name="pageNumberToExtract">the page number to extract</param>
    '' <param name="outPdf">the full path for the output pdf</param>
    '' <remarks></remarks>
    Public Sub ExtractPdfPage(ByVal sourcePdf As String, ByVal pageNumberToExtract As Integer, ByVal outPdf As String)
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim doc As iTextSharp.text.Document = Nothing
        Dim pdfCpy As iTextSharp.text.pdf.PdfCopy = Nothing
        Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourcePdf)
            doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
            pdfCpy = New iTextSharp.text.pdf.PdfCopy(doc, New IO.FileStream(outPdf, IO.FileMode.Create))
            doc.Open()
            page = pdfCpy.GetImportedPage(reader, pageNumberToExtract)
            pdfCpy.AddPage(page)
            doc.Close()
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '' <summary>
    '' Extract selected pages from a source pdf to a new pdf
    '' </summary>
    '' <param name="sourcePdf">the full path to source pdf to a new pdf</param>
    '' <param name="pageNumbersToExtract">the page numbers to extract (i.e {1, 3, 5, 6})</param>
    '' <param name="outPdf">The full path for the output pdf</param>
    '' <remarks>The output pdf will contains the extracted pages in the order of the page numbers listed
    '' in pageNumbersToExtract parameter.</remarks>
    Public Sub ExtractPdfPage(ByVal sourcePdf As String, ByVal pageNumbersToExtract() As Integer, ByVal outPdf As String)
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim doc As iTextSharp.text.Document = Nothing
        Dim pdfCpy As iTextSharp.text.pdf.PdfCopy = Nothing
        Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourcePdf)
            doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1))
            pdfCpy = New iTextSharp.text.pdf.PdfCopy(doc, New IO.FileStream(outPdf, IO.FileMode.Create))
            doc.Open()
            For Each pageNum As Integer In pageNumbersToExtract
                page = pdfCpy.GetImportedPage(reader, pageNum)
                pdfCpy.AddPage(page)
            Next
            doc.Close()
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '' <summary>
    '' Split a single pdf file into multiple pdfs with equal number of pages.
    '' </summary>
    '' <param name="sourcePdf">the full path to the source pdf</param>
    '' <param name="parts">the number of splitted pdfs to split to</param>
    '' <param name="baseNameOutPdf">the base file name (full path) for splitted pdfs.
    '' The actual output pdf file names will be serialized. </param>
    '' <remarks>The last splitted pdf may not have
    '' the same number of pages as the rest, depending on the combination of number of pages in the source pdf 
    '' and the number of parts to be splitted. For example, if the original pdf has 9 pages and it is to be 
    '' splitted into 5 parts, the last splitted pdf will have only 1 page while all others have 2 pages.</remarks>
    Public Sub SplitPdfByParts(ByVal sourcePdf As String, ByVal parts As Integer, ByVal baseNameOutPdf As String)
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim doc As iTextSharp.text.Document = Nothing
        Dim pdfCpy As iTextSharp.text.pdf.PdfCopy = Nothing
        Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
        Dim pageCount As Integer = 0
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourcePdf)
            pageCount = reader.NumberOfPages
            If pageCount < parts Then
                Throw New ArgumentException("Not enough pages in source pdf to split")
            Else
                Dim n As Integer = pageCount 'parts
                Dim currentPage As Integer = 1
                Dim ext As String = IO.Path.GetExtension(baseNameOutPdf)
                Dim outfile As String = String.Empty
                For i As Integer = 1 To parts
                    outfile = baseNameOutPdf.Replace(ext, "_" & i & ext)
                    doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(currentPage))
                    pdfCpy = New iTextSharp.text.pdf.PdfCopy(doc, New IO.FileStream(outfile, IO.FileMode.Create))
                    doc.Open()
                    If i < parts Then
                        For j As Integer = 1 To n
                            page = pdfCpy.GetImportedPage(reader, currentPage)
                            pdfCpy.AddPage(page)
                            currentPage += 1
                        Next j
                    Else
                        For j As Integer = currentPage To pageCount
                            page = pdfCpy.GetImportedPage(reader, j)
                            pdfCpy.AddPage(page)
                        Next j
                    End If
                    doc.Close()
                Next
            End If
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '' <summary>
    '' Split source pdf into multiple pdfs with specifc number of pages
    '' </summary>
    '' <param name="sourcePdf">the full path to source pdf</param>
    '' <param name="numOfPages">the number of pages each splitted pdf should contain</param>
    '' <param name="baseNameOutPdf">the base file name (full path) for splitted pdfs.
    '' The actual output pdf file names will be serialized. </param>
    '' <remarks>The last splitted pdf may not have
    '' the same number of pages as the rest, depending on the combination of number of pages in the source pdf 
    '' and the number of target pages in each splitted pdf. For example, if the original pdf has 9 pages and it is to be 
    '' splitted with 2 pages for each pdf, the last splitted pdf will have only 1 page while all others have 2 pages.</remarks>
    Public Sub SplitPdfByPages(ByVal sourcePdf As String, ByVal numOfPages As Integer, ByVal baseNameOutPdf As String)
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim doc As iTextSharp.text.Document = Nothing
        Dim pdfCpy As iTextSharp.text.pdf.PdfCopy = Nothing
        Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
        Dim pageCount As Integer = 0
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourcePdf)
            pageCount = reader.NumberOfPages
            If pageCount < numOfPages Then
                Throw New ArgumentException("Not enough pages in source pdf to split")
            Else
                Dim ext As String = IO.Path.GetExtension(baseNameOutPdf)
                Dim outfile As String = String.Empty
                Dim n As Integer = CInt(Math.Ceiling(pageCount / numOfPages))
                Dim currentPage As Integer = 1
                For i As Integer = 1 To n
                    outfile = baseNameOutPdf.Replace(ext, "_" & i & ext)
                    doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(currentPage))
                    pdfCpy = New iTextSharp.text.pdf.PdfCopy(doc, New IO.FileStream(outfile, IO.FileMode.Create))
                    doc.Open()
                    If i < n Then
                        For j As Integer = 1 To numOfPages
                            page = pdfCpy.GetImportedPage(reader, currentPage)
                            pdfCpy.AddPage(page)
                            currentPage += 1
                        Next j
                    Else
                        For j As Integer = currentPage To pageCount
                            page = pdfCpy.GetImportedPage(reader, j)
                            pdfCpy.AddPage(page)
                        Next j
                    End If
                    doc.Close()
                Next
            End If
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

    'Public Sub MergePDF_New()
    '    Dim completedDocument As Byte() = Nothing
    '    Using streamCompleted As New MemoryStream()
    '        Using document As New iTextSharp.text.Document()
    '            document.Open()
    '            Dim copy As New iTextSharp.text.pdf.PdfCopy(document, streamCompleted)
    '            copy.Open()

    '            For Each item As var In eventItems
    '                Dim mergedDocument As Byte() = Nothing
    '                Dim reader As New PdfReader(pdfTemplates(item.DataTokens(NotifyTokenType.OrganisationID)))
    '                Using streamTemplate As New MemoryStream()
    '                    Using stamper As New PdfStamper(reader, streamTemplate)
    '                        For Each token As var In item.DataTokens
    '                            If stamper.AcroFields.Fields.Any(Function(fld) fld.Key = token.Key.ToString()) Then
    '                                stamper.AcroFields.SetField(token.Key.ToString(), token.Value)
    '                            End If
    '                        Next
    '                        stamper.FormFlattening = True
    '                        stamper.Writer.CloseStream = False
    '                    End Using

    '                    mergedDocument = New Byte(streamTemplate.Length - 1) {}
    '                    streamTemplate.Position = 0
    '                    streamTemplate.Read(mergedDocument, 0, CInt(streamTemplate.Length))
    '                End Using
    '                reader = New PdfReader(mergedDocument)

    '                For i As Integer = 1 To reader.NumberOfPages
    '                    document.SetPageSize(PageSize.A4)
    '                    copy.AddPage(copy.GetImportedPage(reader, i))
    '                Next
    '            Next
    '        End Using
    '        completedDocument = New Byte(streamCompleted.Length - 1) {}
    '        streamCompleted.Position = 0
    '        streamCompleted.Read(completedDocument, 0, CInt(streamCompleted.Length))
    '    End Using


    'End Sub
End Module
