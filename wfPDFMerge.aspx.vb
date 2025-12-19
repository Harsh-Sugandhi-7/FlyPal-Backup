Imports System.Collections.Generic
Imports System.Text
Imports iTextSharp.text.pdf.parser

Public Class wfPDFMerge
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '//********************************************Send Files for Merging****************************************************//


        If Not IsPostBack Then
            Dim files As String() = Directory.GetFiles("D:\", "*.pdf")

            Dim filesByte As New List(Of Byte())()
            For Each file__1 As String In files
                filesByte.Add(File.ReadAllBytes(file__1))
            Next

            File.WriteAllBytes("D:\12.pdf", Flypal.PDFMergers.MergeFiles(filesByte))

            '//********************************************Open Merged file*********************************************************//
            Session("CrystalReport") = "D:\12.pdf"
            Session("PrintReportWithAttachment") = "True"

            Dim str As String = ""
            str = "<script language=Javascript>openTranDetail();</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", str)
        End If


    End Sub

    'http://www.vbforums.com/showthread.php?490456-VB.NET-Extract-Pages-and-Split-Pdf-Files-Using-iTextSharp

    Protected Sub btnPrintPDF_Click(sender As Object, e As EventArgs) Handles btnPrintPDF.Click

        Dim filename As String = "C:\C\8051629211.pdf"

        If File.Exists(filename) Then

            Try

                Dim text As New StringBuilder()

                Dim pdfReader As New iTextSharp.text.pdf.PdfReader(filename)

                For page As Integer = 1 To pdfReader.NumberOfPages


                    Dim strategy As ITextExtractionStrategy = New SimpleTextExtractionStrategy()

                    Dim currentText As String = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy)

                    text.Append(System.Environment.NewLine)

                    text.Append(vbLf & " Page Number:" & page)

                    text.Append(System.Environment.NewLine)

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.[Default], Encoding.UTF8, Encoding.[Default].GetBytes(currentText)))

                    text.Append(currentText)
                Next

                pdfReader.Close()
                pdftext.Text += text.ToString()

            Catch ex As Exception

                MessageBox.Show("Error: " + ex.Message, "Error")

            End Try
        End If

    End Sub
End Class