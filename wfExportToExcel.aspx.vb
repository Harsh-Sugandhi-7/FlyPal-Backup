'************************************
'Modified by Harsh Sugandhi on 15th December 2025
'************************************


Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Imports OfficeOpenXml


Public Class wfExportToExcel
	Inherits Page


#Region " Helper Method(s) "

	Private Shared Sub FormatWorksheetData(DataTable As DataTable,
										   ExcelWorksheet As ExcelWorksheet,
										   PeriodColumns As List(Of String))

		Dim columnCount As Integer = DataTable.Columns.Count
		Dim rowCount As Integer = DataTable.Rows.Count
		Dim ExcelRange As ExcelRange
		Try

			For i As Integer = 1 To columnCount

				If PeriodColumns.Contains(ExcelWorksheet.Cells(1, i).Value.ToString()) Then

					ExcelRange = ExcelWorksheet.Cells(2, i, rowCount + 1, i)
					ExcelRange.AutoFitColumns()
					ExcelRange.Style.WrapText = True

				End If

				ExcelRange = ExcelWorksheet.Cells(2, i, rowCount + 1, i)
				ExcelRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top

			Next

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Shared Sub FormatMSPDue(DataTable As DataTable,
									ExcelWorksheet As ExcelWorksheet,
									ColumnsList As List(Of String))

		Dim columnCount As Integer = DataTable.Columns.Count
		Dim rowCount As Integer = DataTable.Rows.Count
		Try

			For i As Integer = 1 To columnCount

				If ColumnsList.Contains(ExcelWorksheet.Cells(1, i).Value.ToString()) Then

					For k As Integer = 2 To rowCount + 1

						If Val(ExcelWorksheet.Cells(k, 8).Text) < 0 Then
							ExcelWorksheet.Cells(k, 8).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
							ExcelWorksheet.Cells(k, 8).Style.Fill.BackgroundColor.SetColor(Color.Red)
						Else
							ExcelWorksheet.Cells(k, 8).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
							ExcelWorksheet.Cells(k, 8).Style.Fill.BackgroundColor.SetColor(Color.Green)
						End If

					Next

				End If

			Next

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Shared Sub Format(DataTable As DataTable,
							  ExcelWorksheet As ExcelWorksheet,
							  ColumnsList As List(Of String))

		Dim columnCount As Integer = DataTable.Columns.Count
		Dim rowCount As Integer = DataTable.Rows.Count
		Try

			For i As Integer = 1 To columnCount

				If ColumnsList.Contains(ExcelWorksheet.Cells(1, i).Value.ToString()) Then

					For k As Integer = 2 To rowCount + 1

						If Val(ExcelWorksheet.Cells(k, 4).Text) = 0 Then
							ExcelWorksheet.Cells(k, 4).Value = "---"
							ExcelWorksheet.Cells(k, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right
						End If

						If Val(ExcelWorksheet.Cells(k, 5).Text) < 0 Then
							ExcelWorksheet.Cells(k, 5).Value = "---"
							ExcelWorksheet.Cells(k, 5).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right
						End If

					Next

				End If

			Next

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Shared Sub FormatOrderHistoryOrderQuantityColumn(DataTable As DataTable,
															 ExcelWorksheet As ExcelWorksheet,
															 ColumnsList As List(Of String))

		Dim columnCount As Integer = DataTable.Columns.Count
		Dim rowCount As Integer = DataTable.Rows.Count
		Try

			For i As Integer = 1 To columnCount

				If ColumnsList.Contains(ExcelWorksheet.Cells(1, i).Value.ToString()) Then

					For k As Integer = 2 To rowCount + 1

						If Val(ExcelWorksheet.Cells(k, 5).Text) = 0 Then
							ExcelWorksheet.Cells(k, 5).Value = ""
						End If

					Next

				End If

			Next

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Shared Sub AlignColumnsToTheRight(DataTable As DataTable,
											  ExcelWorksheet As ExcelWorksheet,
											  ColumnsList As List(Of String))

		Dim columnCount As Integer = DataTable.Columns.Count
		Dim rowCount As Integer = DataTable.Rows.Count
		Try

			For i As Integer = 1 To columnCount

				Dim header As String = ExcelWorksheet.Cells(1, i).Value.ToString()

				If ColumnsList.Contains(header) Then

					For k As Integer = 2 To rowCount + 1
						Dim cell = ExcelWorksheet.Cells(k, i)
						cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right
					Next

				End If

			Next

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Page Event(s) "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		Dim ExcelFileName As String
		Dim ColumnsList As List(Of String)
		Dim ExcelPackage As New ExcelPackage
		Dim ExcelWorksheet As ExcelWorksheet
		Dim PeriodColumns As List(Of String)
		Dim MELListInExcelColumns As List(Of String)
		Dim OrderQuantityColumnsList As List(Of String)
		Dim DataTableToBeFormattedForExportToExcel As String
		Dim OrderHistoryOrderQuantityColumns As List(Of String)

		Try

			ColumnsList = Session("MSPColumns")
			MELListInExcelColumns = Session("MELListInExcel")
			PeriodColumns = Session("PeriodColumnsForExportToExcel")
			Dim DataSet As DataSet = CType(Session("dsNew"), DataSet)
			OrderQuantityColumnsList = Session("OrderQuantityColumns")
			Dim path As String = $"{AppSettings("DOCPath")}\FlyPalReport.xlsx"
			OrderHistoryOrderQuantityColumns = Session("OrderHistoryOrderQuantityColumns")
			DataTableToBeFormattedForExportToExcel = Session("DataTableToBeFormattedForExportToExcel")
			ExcelFileName = If(If(Session("ExcelFileName"), Session("DataTableToBeFormattedForExportToExcel")), "FlyPalReport")
			ExcelFileName = Regex.Replace(ExcelFileName, "[\\/:*?""<>|]", "_")

			For Each dataTable As DataTable In DataSet.Tables

				ExcelWorksheet = ExcelPackage.Workbook.Worksheets.Add(dataTable.TableName)

				If dataTable.TableName = "RVSM Report" Then

					ExcelWorksheet.Cells("F1:H1").Value = "ALTIMETER"
					ExcelWorksheet.Cells("F1:H1").Merge = True
					ExcelWorksheet.Cells("I1:K1").Value = "ALTIMETRY SYSTEM ERROR(ASE) (245FT)"
					ExcelWorksheet.Cells("I1:K1").Merge = True
					ExcelWorksheet.Cells("L1:L1").Value = "ASSIGNED ALTITUDED DEVIATION(AAD) (300FT)"
					ExcelWorksheet.Cells("M1:N1").Value = "TOTAL VERTICAL ERROR(TVE) (300FT)"
					ExcelWorksheet.Cells("M1:N1").Merge = True '#5b9bd5
					ExcelWorksheet.Cells("F1:H1").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
					ExcelWorksheet.Cells("I1:K1").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
					ExcelWorksheet.Cells("L1").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
					ExcelWorksheet.Cells("M1:N1").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
					ExcelWorksheet.Cells("F1:H1").Style.Font.Bold = True
					ExcelWorksheet.Cells("L1").Style.Font.Bold = True
					ExcelWorksheet.Cells("I1:K1").Style.Font.Bold = True
					ExcelWorksheet.Cells("M1:N1").Style.Font.Bold = True
					ExcelWorksheet.Cells("F1:H1").Style.Font.Bold = True
					ExcelWorksheet.Cells("L1").Style.Font.Bold = True
					ExcelWorksheet.Cells("I1:K1").Style.Font.Bold = True
					ExcelWorksheet.Cells("M1:N1").Style.Font.Bold = True
					ExcelWorksheet.Cells("F1:H1").Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("I1:K1").Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("L1").Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("M1:N1").Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("F1:H1").Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("I1:K1").Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("L1").Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("M1:N1").Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("F1:H1").Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("I1:K1").Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("L1").Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("M1:N1").Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("F1:H1").Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("I1:K1").Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("L1").Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("M1:N1").Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
					ExcelWorksheet.Cells("F1:H1").Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213))
					ExcelWorksheet.Cells("L1").Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213))
					ExcelWorksheet.Cells("I1:K1").Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213))
					ExcelWorksheet.Cells("M1:N1").Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213))

					ExcelWorksheet.Cells("A2").LoadFromDataTable(dataTable, True, Table.TableStyles.Light9)

				Else

					ExcelWorksheet.Cells("A1").LoadFromDataTable(dataTable, True, Table.TableStyles.Light9)

					If dataTable.TableName.Equals("Searching Criteria") Then

						ExcelWorksheet.Cells("A8:F8").Value = "Report is available on second sheet of this file."
						ExcelWorksheet.Cells("A8:F8").Style.Font.Bold = True
						ExcelWorksheet.Cells("A8:F8").Style.Font.Size = 14
						ExcelWorksheet.Cells("A8:F8").Style.Font.Color.SetColor(Color.Red)
						ExcelWorksheet.Cells("A8:F8").Style.Fill.PatternType = Style.ExcelFillStyle.Solid
						ExcelWorksheet.Cells("A8:F8").Style.Fill.BackgroundColor.SetColor(Color.Yellow)
						ExcelWorksheet.Cells("A8:F8").Merge = True

					Else

						' Added by Vikrant on 09-Nov-2017 for Thrust Level 
						If Session("FormatReportTableInExcel") = "True" Then
							Session.Remove("FormatReportTableInExcel")
							ExcelWorksheet.Cells(dataTable.Rows.Count, 1, dataTable.Rows.Count + 1, dataTable.Columns.Count).Style.Font.Bold = True
						End If

					End If

				End If

				If PeriodColumns IsNot Nothing Then

					If dataTable.TableName.Equals(DataTableToBeFormattedForExportToExcel) Then
						FormatWorksheetData(dataTable, ExcelWorksheet, PeriodColumns)
					End If

				End If

				If ColumnsList IsNot Nothing Then

					If dataTable.TableName.Equals("MSP Due") Then
						FormatMSPDue(dataTable, ExcelWorksheet, ColumnsList)
					End If

				End If

				If OrderQuantityColumnsList IsNot Nothing Then

					If dataTable.TableName.Equals("Re-Order Items") Then
						Format(dataTable, ExcelWorksheet, OrderQuantityColumnsList)
					End If

				End If

				If OrderHistoryOrderQuantityColumns IsNot Nothing Then

					If dataTable.TableName.Equals("Order History") Then
						FormatOrderHistoryOrderQuantityColumn(dataTable, ExcelWorksheet, OrderHistoryOrderQuantityColumns)
					End If

				End If

				If MELListInExcelColumns IsNot Nothing Then

					If dataTable.TableName.Equals("Master Minimum Equipment List") Then
						AlignColumnsToTheRight(dataTable, ExcelWorksheet, MELListInExcelColumns)
					End If

				End If

			Next

			Response.ClearContent()
			Response.ClearHeaders()

			Response.Clear()
			Response.Buffer = True
			Response.ContentType = "application/vnd.ms-excel"
			Response.Headers.Remove("Content-Disposition")
			Response.AddHeader("Content-Disposition", $"attachment; filename=""{ExcelFileName}.xlsx""")

			Dim bytes = ExcelPackage.GetAsByteArray()
			Response.OutputStream.Write(bytes, 0, bytes.Length)

			Response.Flush()
			HttpContext.Current.ApplicationInstance.CompleteRequest()

		Catch ex As Exception
			Throw ex.GetBaseException()
		Finally
			Session.Remove("PeriodColumnsForExportToExcel")
			Session.Remove("PeriodColumnsForExportToExcel")
		End Try

	End Sub

	Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload

		Try

			Session.Remove("ReportName")
			Session.Remove("DataTable")

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

End Class