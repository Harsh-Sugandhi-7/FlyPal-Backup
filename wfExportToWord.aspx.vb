Imports OfficeOpenXml
Imports System.Collections.Generic
Public Class wfExportToWord
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim myFile As String
            Dim a As New Random
            myFile = AppSettings("DOCPath") & "Log Entry" & a.Next & ".doc"
            Session("CrystalReport").ExportToDisk(CrystalDecisions.[Shared].ExportFormatType.WordForWindows, myFile)
            Response.ClearContent()
            Response.ClearHeaders()
            Response.WriteFile(myFile)
            Response.ContentType = "Application/vnd.word"
            Dim path As String = AppSettings("DOCPath") & "\FlyPalReport.doc"
            Response.AppendHeader("Content-Disposition", "attachment; filename=FlyPalReport.doc")
            Response.Flush()
            System.IO.File.Delete(myFile)
            Response.End()
        Catch ex As Exception
            ex.GetBaseException()
        Finally
            Session.Remove("PeriodColumnsForExportToExcel")
            Session.Remove("PeriodColumnsForExportToExcel")
        End Try
    End Sub
    Private Sub wfExportToExcel_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Session.Remove("ReportName")
        Session.Remove("DataTable")
    End Sub
End Class