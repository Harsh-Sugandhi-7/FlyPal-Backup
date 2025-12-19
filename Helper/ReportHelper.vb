'************************************
'Created by:	Harsh Sugandhi
'Created on:	29th November 2024
'Created for:	To place all the Reports related methods in one place for managing redundant code.
'************************************


Imports System.Collections.Generic
Imports System.Text

Public Class ReportHelper

#Region " Helper Method(s) "

	Private Function ConvertCrystalReportToBinary(Report As Engine.ReportClass,
												  Optional StatusID As Integer = 0,
												  Optional ShowWatermark As Boolean = False) As Byte()

		Dim tempDir As String = Path.GetTempPath()
		Dim pdfPath As String
		Dim finalPdfPath As String
		Dim RandomNo As New Random()
		Try

			Directory.CreateDirectory(path:=tempDir)
			pdfPath = Path.Combine(tempDir, $"Rep_{RandomNo.Next()}.pdf")

			Dim diskOptions As New DiskFileDestinationOptions()
			diskOptions.DiskFileName = pdfPath

			With Report.ExportOptions

				.DestinationOptions = diskOptions
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat

			End With

			Report.Export()
			Report.Close()
			Report.Dispose()

			finalPdfPath = pdfPath

			If StatusID < 2 AndAlso ShowWatermark Then

				Dim watermarkPath As String = Path.Combine(tempDir, $"Rep_Watermarked_{RandomNo.Next()}.pdf")

				Try

					AddWatermarkText(sourceFile:=pdfPath,
									 outputFile:=watermarkPath,
									 watermarkText:="PREVIEW", , ,
									 watermarkFontColor:=iTextSharp.text.BaseColor.GRAY, ,
									 watermarkRotation:=0.0,
									 PrevPageCount:=0,
									 ShowWatermarkOnCenter:=True)

					finalPdfPath = watermarkPath

					If File.Exists(pdfPath) Then
						File.Delete(pdfPath)
					End If

				Catch ex As Exception
					Throw ex
				End Try

			End If

			Dim fileBytes As Byte()

			Using fs As New FileStream(finalPdfPath, FileMode.Open, FileAccess.Read, FileShare.Read)

				ReDim fileBytes(fs.Length - 1)
				fs.Read(fileBytes, 0, fileBytes.Length)

			End Using

			Try

				If File.Exists(finalPdfPath) Then
					File.Delete(finalPdfPath)
				End If

			Catch ex As IOException
				Throw ex
			End Try

			Return fileBytes

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	Public Function GenerateEmailBody(ModuleName As String,
									  AuthorizedBy As String,
									  AuthorizationDate As String,
									  Details As IDictionary(Of String, String),
									  Optional AttachmentNote As String = Nothing) As String

		Dim tableRows As New StringBuilder()

		Dim Padding = "padding: 5px;"
		Dim FontBold = "font-weight: bold;"
		Dim MinWidth = "min-width: 150px;"
		Dim Style = "font-family: Calibri, Arial, sans-serif; font-size: 11pt;"

		Try

			If String.IsNullOrEmpty(value:=AttachmentNote) Then
				AttachmentNote = $"A copy of the authorized {ModuleName} is attached for your Information & Planning."
			End If

			For Each kvp As KeyValuePair(Of String, String) In Details

				tableRows.AppendLine($"<tr>
												<td style=""{Padding} {FontBold} {MinWidth}"">{kvp.Key}:</td>
												<td style=""{Padding}"">{kvp.Value}</td>
											</tr>")

			Next

			tableRows.AppendLine($"<tr>
											<td style=""{Padding} {FontBold}"">Authorized By:</td>
											<td style=""{Padding}"">{AuthorizedBy}</td>
										</tr>
										<tr>
											<td style=""{Padding} {FontBold}"">Authorization Date:</td>
											<td style=""{Padding}"">{AuthorizationDate}</td>
										</tr>")

			Dim html = $"<html>
						<head>
							<meta charset='utf-8'>
						</head>
						<body>
							<p style=""{Style}"">
								The following <strong>{ModuleName}</strong> has been <strong>successfully authorized.</strong>:
							</p>

							<table style=""{Style} border-collapse: collapse;"">
								{tableRows.ToString.Trim}
							</table>

							<br>

							<p style=""{Style}"">
								{AttachmentNote}
							</p>

							<p style=""{Style}"">
								Thank you.
							</p>
						</body>
					</html>"

			Return html

		Catch ex As Exception When TypeOf ex IsNot ArgumentNullException

			Dim errMsg As String = $"Failed to generate Email body for '{ModuleName}'. " &
								   $"Details count: {If(Details?.Count, 0)}. " &
								   $"Error: {ex.Message}"
			Throw New InvalidOperationException(errMsg, ex)

		End Try

	End Function

#End Region

#Region " List Page "

	'Sankalp 22-09-25
	Public Function ListReport(List As Object,
							   ColumnHeaders() As String,
							   IsForAPI As Boolean,
							   ReportOf As String) As (Object, String)

		Dim CrystalReport As New Object
		Dim DataAdapter As New ObjectAdapter
		Dim DataSet As New dsCommon
		Dim StatusList As New rptStatusList
		Dim CompanyLogo As rptImage = rptImage.GetImage(DataSet)

		Try

			If ReportOf = "EnquiryList" Then

				CrystalReport = New crEnquiryList
				List = CType(List, EnquiryList)

				StatusList.Add(obj:=New rptStatus(GroupType:=0,
												  LHLabel:=ColumnHeaders(0),
												  LHData:=ColumnHeaders(1),
												  LHLabel1:=ColumnHeaders(2),
												  LHData1:=ColumnHeaders(3),
												  LHLabel2:=ColumnHeaders(4),
												  LHData2:=ColumnHeaders(5)))

				For Each EnquiryInfo As EnquiryList.EnquiryInfo In List

					StatusList.Add(New rptStatus(GroupType:=1,
													 LHLabel:=EnquiryInfo.DateFormatted,
													 LHData:=EnquiryInfo.EnquiryNo,
													 LHLabel1:=EnquiryInfo.VendorName,
													 LHData1:=EnquiryInfo.Status,
													 LHLabel2:=EnquiryInfo.UserName,
													 LHData2:=EnquiryInfo.AuthorizedBy))

				Next

			ElseIf ReportOf = "IssueList" Then

				CrystalReport = New crIssueList
				List = CType(List, IssueList)

				StatusList.Add(obj:=New rptStatus(GroupType:=0,
												  LHLabel:=ColumnHeaders(0),
												  LHData:=ColumnHeaders(1),
												  LHLabel1:=ColumnHeaders(2),
												  LHData1:=ColumnHeaders(3),
												  LHLabel2:=ColumnHeaders(4),
												  LHData2:=ColumnHeaders(5),
												  LHData3:=ColumnHeaders(6)))

				For Each IssueInfo As IssueList.IssueInfo In List

					StatusList.Add(New rptStatus(,
													 GroupType:=1, ,
													 LHLabel:=IssueInfo.ILDateFormatted,
													 LHData:=IssueInfo.IssueNo,
													 LHLabel1:=IssueInfo.IssueType,
													 LHData1:=IssueInfo.StoreName,
													 LHLabel2:=IssueInfo.Destination,
													 LHData2:=IssueInfo.StatusName,
													 LHData3:=IssueInfo.AuthorizedByName))

				Next

			End If

			Dim CompanyDetail As CompanyDetail = CompanyDetail.GetCompanyDetail("", "", "",
																				"", "", "", "")
			Dim ReportData As New ReportData(CompanyName:=CompanyDetail.CompanyName,
											 Address:=CompanyDetail.Address,
											 Tel1:=CompanyDetail.Tel1,
											 Tel2:=CompanyDetail.Tel2,
											 Fax:=CompanyDetail.Fax,
											 Email:=CompanyDetail.Email,
											 WebSite:=CompanyDetail.WebSite,
											 ReportName:="Enquiry List Report",
											 SearchStr1:="", SearchStr2:="", SearchStr3:="", SearchStr4:="", SearchStr5:="",
											 ProductVersion:=AppSettings("Product Version"),
											 SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="",
											 SearchStr8:="", SearchStr9:="",
											 SearchStr10:=AppSettings("Logo"))

			DataAdapter.Fill(DataSet, CompanyLogo)
			DataAdapter.Fill(DataSet, StatusList)
			DataAdapter.Fill(DataSet, ReportData)

			CrystalReport.SetDataSource(DataSet)

			If Not IsForAPI Then
				Return (CrystalReport, "Success")
			Else

				Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(CrystalReport, Engine.ReportClass))
				Return (fileContent, "Success")

			End If

		Catch ex As Exception
			Return (ex.Message, "Error")
		End Try

	End Function

#End Region

#Region " Detail Page "

	Public Function GetPODetailedReport(OrderID As Guid,
										Optional ByMail As Boolean = False,
										Optional IsPROCUREMENTANDPAYMENTFORM As Boolean = False) As ReturnMessage

		Try

			Dim MailBody As String
			Dim DataSet As New dsOrder
			Dim OrderDetail As rptOrders
			Dim LetterHead As rptLetterHead
			Dim OrderChilds As rptOrderChields
			Dim DataAdapter As New ObjectAdapter
			Dim CrystalReport As Engine.ReportClass
			Dim _Order As Order = Order.GetOrder(ID:=OrderID)
			Dim ListOfKitItemsForOrderItemCount As String = "0"
			Dim mListOfKitItemsForOrderItem As ListOfKitItemsForOrderItem
			Dim CompanyLogo As rptImage = rptImage.GetImage(DataSet:=DataSet)

			If IsPROCUREMENTANDPAYMENTFORM = True Then 'Added By Prashant 28-Jan-2025
				CrystalReport = New crptPROCUREMENTFORM
			Else
				If AppSettings("ClientCode") = "ASH" Then
					CrystalReport = New crptOrderAshleyAviation
				Else

					If CDate(_Order.OrderDate) <= CDate("30-Jun-2017") Or _Order.Visibility = 3 Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
							CrystalReport = New crptOrderDetailPortraitForFlyGeorgia
						ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "JA" Then
							CrystalReport = New crptOrderDetailPortraitForJA
						ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "PTW" Then
							CrystalReport = New crptOrderDetailPortraitForPattaya
						ElseIf AppSettings("ClientCode") = "KAS" Then 'Added By Prashant on 27-Jan-2025
							CrystalReport = New crptOrderDetailPortraitKasas
						Else

							If _Order.TransTypeID = 5 Then

								If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
									CrystalReport = New crptOrderDetailPortraitForInd
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
									   (AppSettings("ClientCode") = "Heligo" Or
										AppSettings("ClientCode") = "UHPL") Then
									CrystalReport = New crptOrderDetailPortraitForHeligo
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "HL" Then
									CrystalReport = New crptOrderDetailPortraitForHL
								ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
									CrystalReport = New crptOrderDetailPortraitForYA
								ElseIf (AppSettings("ClientCode") = "CGA") Then
									CrystalReport = New crptOrderDetailPortraitForChhattisgarh
								ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
									CrystalReport = New crptOrderDetailPortraitBA
								ElseIf (AppSettings("ClientCode") = "MID") Then
									CrystalReport = New crptOrderDetailPortraitForMidex
								ElseIf (AppSettings("ClientCode") = "GEP") Then
									CrystalReport = New crptOrderDetailPortraitForGEP
								ElseIf (AppSettings("ClientCode") = "LAMA") Then
									CrystalReport = New crptOrderDetailPortraitLAMA
								ElseIf AppSettings("ClientCode") = "HSC" Then
									CrystalReport = New crptOrderDetailPortraitForHeliStar
								ElseIf AppSettings("ClientCode") = "ARA" Then
									CrystalReport = New crptOrderDetailPortraitForARAirWays
								Else
									CrystalReport = New crptOrderDetailPortrait
								End If

							ElseIf _Order.TransTypeID = 31 Then

								If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
									CrystalReport = New crptOrderExchOHDetailPortraitForInd
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
									   (AppSettings("ClientCode") = "Heligo" Or
										AppSettings("ClientCode") = "UHPL") Then
									CrystalReport = New crptOrderExchOHDetailPortraitForHeligo
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
									   (AppSettings("ClientCode") = "Deccan" Or
										AppSettings("ClientCode") = "ADeccan" Or
										AppSettings("ClientCode") = "IIC" Or
										AppSettings("ClientCode") = "SPZ") Then
									CrystalReport = New crptOrderExchOHDetailPortraitForDeccan
								ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
									CrystalReport = New crptOrderExchOHDetailPortraitForYA
								ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
									CrystalReport = New crptOrderExchOHDetailPortraitBA
								ElseIf (AppSettings("ClientCode") = "CGA") Then
									CrystalReport = New crptOrderExchOHDetailPortraitForChhattisgarh
								ElseIf (AppSettings("ClientCode") = "MID") Then
									CrystalReport = New crptOrderExchOHDetailPortraitForMidex
								ElseIf (AppSettings("ClientCode") = "GEP") Then
									CrystalReport = New crptOrderExchOHDetailPortraitForGEP
								ElseIf (AppSettings("ClientCode") = "LAMA") Then
									CrystalReport = New crptOrderExchOHDetailPortraitLAMA
								ElseIf AppSettings("ClientCode") = "HSC" Then
									CrystalReport = New crptOrderExchOHDetailPortraitForHeliStar
								ElseIf AppSettings("ClientCode") = "ARA" Then
									CrystalReport = New crptOrderDetailPortraitForARAirWays
								Else
									CrystalReport = New crptOrderExchOHDetailPortrait
								End If

							ElseIf _Order.TransTypeID = 38 Then

								If _Order.IsOverhaul = True Then

									If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
										CrystalReport = New crptOrderExchOHDetailPortraitForInd
									ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
										   (AppSettings("ClientCode") = "Heligo" Or
											AppSettings("ClientCode") = "UHPL") Then
										CrystalReport = New crptOrderExchOHDetailPortraitForHeligo
									ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
										   (AppSettings("ClientCode") = "Deccan" Or
											AppSettings("ClientCode") = "ADeccan" Or
											AppSettings("ClientCode") = "IIC" Or
											AppSettings("ClientCode") = "SPZ") Then
										CrystalReport = New crptOrderExchOHDetailPortraitForDeccan
									ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
										CrystalReport = New crptOrderExchOHDetailPortraitForYA
									ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
										CrystalReport = New crptOrderExchOHDetailPortraitBA
									ElseIf (AppSettings("ClientCode") = "CGA") Then
										CrystalReport = New crptOrderExchOHDetailPortraitForChhattisgarh
									ElseIf (AppSettings("ClientCode") = "MID") Then
										CrystalReport = New crptOrderExchOHDetailPortraitForMidex
									ElseIf (AppSettings("ClientCode") = "GEP") Then
										CrystalReport = New crptOrderExchOHDetailPortraitForGEP
									ElseIf (AppSettings("ClientCode") = "LAMA") Then
										CrystalReport = New crptOrderExchOHDetailPortraitLAMA
									ElseIf AppSettings("ClientCode") = "HSC" Then
										CrystalReport = New crptOrderExchOHDetailPortraitForHeliStar
									ElseIf AppSettings("ClientCode") = "ARA" Then
										CrystalReport = New crptOrderDetailPortraitForARAirWays
									Else
										CrystalReport = New crptOrderExchOHDetailPortrait
									End If

								Else

									If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
										CrystalReport = New crptOrderWOForInd
									ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
										   (AppSettings("ClientCode") = "Heligo" Or
											AppSettings("ClientCode") = "UHPL") Then
										CrystalReport = New crptOrderWOForHeligo
									ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
										   (AppSettings("ClientCode") = "Deccan" Or
											AppSettings("ClientCode") = "ADeccan" Or
											AppSettings("ClientCode") = "IIC" Or
											AppSettings("ClientCode") = "SPZ") Then
										CrystalReport = New crptOrderWOForDeccan
									ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
										CrystalReport = New crptOrderWOForYA
									ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
										CrystalReport = New crptOrderWOBA
									ElseIf (AppSettings("ClientCode") = "CGA") Then
										CrystalReport = New crptOrderWOForChhattisgarh
									ElseIf (AppSettings("ClientCode") = "MID") Then
										CrystalReport = New crptOrderExchOHDetailPortraitForMidex
									ElseIf (AppSettings("ClientCode") = "GEP") Then
										CrystalReport = New crptOrderWOForGEP
									ElseIf (AppSettings("ClientCode") = "LAMA") Then
										CrystalReport = New crptOrderWOLAMA
									ElseIf AppSettings("ClientCode") = "HSC" Then
										CrystalReport = New crptOrderWOForHeliStar
									ElseIf AppSettings("ClientCode") = "ARA" Then
										CrystalReport = New crptOrderDetailPortraitForARAirWays
									Else
										CrystalReport = New crptOrderWO
									End If

								End If

							ElseIf _Order.TransTypeID = 39 Then

								If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
									CrystalReport = New crptOrderDetailPortraitForInd
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
									   (AppSettings("ClientCode") = "Heligo" Or
										AppSettings("ClientCode") = "UHPL") Then
									CrystalReport = New crptOrderDetailPortraitForHeligo
								ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "HL" Then
									CrystalReport = New crptOrderDetailPortraitForHL
								ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
									CrystalReport = New crptOrderDetailPortraitForYA
								ElseIf (AppSettings("ClientCode") = "CGA") Then
									CrystalReport = New crptOrderDetailPortraitForChhattisgarh
								ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
									CrystalReport = New crptOrderDetailPortraitBA
								ElseIf (AppSettings("ClientCode") = "GEP") Then
									CrystalReport = New crptOrderDetailPortraitForGEP
								ElseIf (AppSettings("ClientCode") = "LAMA") Then
									CrystalReport = New crptOrderDetailPortraitLAMA
								ElseIf AppSettings("ClientCode") = "HSC" Then
									CrystalReport = New crptOrderDetailPortraitForHeliStar
								Else
									CrystalReport = New crptOrderDetailPortrait
								End If

							End If

						End If

					Else
						CrystalReport = New crptOrderGSTDetail
					End If

				End If
			End If

			OrderDetail = rptOrders.GetOrders(OrderID:=_Order.ID)
			OrderChilds = rptOrderChields.GetOrderChields(OrderID:=_Order.ID)

			Dim EmployeeName As String = If(_Order.AuthorizedBy = "", "", User.GetUser(UserName:=_Order.AuthorizedBy).EmpNoName)
			Dim mEmployeeInfoFromUser As User = If((Not _Order.UserName = ""), User.GetUser(UserName:=_Order.UserName), Nothing)

			If CBool(AppSettings("ShowKitItems")) Then

				mListOfKitItemsForOrderItem = ListOfKitItemsForOrderItem.GetListOfKitItemsForOrderItems(_Order.ID)
				DataAdapter.Fill(DataSet, mListOfKitItemsForOrderItem)
				ListOfKitItemsForOrderItemCount = mListOfKitItemsForOrderItem.Count.ToString

			End If

			LetterHead = rptLetterHead.GetLetterHeadInfo(ID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
														 ReportName:=EmployeeName,
														 ListOfKitItemsForOrderItemCount,
														 AppSettings("Logo"),
														 SearchString3:=AppSettings("AdvancePayment"),
														 ClientCode:=AppSettings("ClientCode"),
														 SearchString4:=AppSettings("HSNACSCodeVisibleInPartMaster"),
														 SearchString5:=_Order.OrderItems.Count.ToString,
														 SearchString6:=_Order.OrderConfirmationNo,
														 SearchString7:=mEmployeeInfoFromUser.EmployeeName,
														 SearchString8:=mEmployeeInfoFromUser.EmployeeEmail,
														 SearchString9:=mEmployeeInfoFromUser.EmployeePhoneNo)

			Dim BaseCurrencySymbol As String = If(LetterHead.Count > 0, LetterHead(0).BaseCurrencysymbol, "")

			DataAdapter.Fill(DataSet, OrderDetail)
			DataAdapter.Fill(DataSet, OrderChilds)
			DataAdapter.Fill(DataSet, LetterHead)
			DataAdapter.Fill(DataSet, CompanyLogo)

			CrystalReport.SetDataSource(DataSet)

			If ByMail Then

				Dim OrderNo As String = $"{_Order.Text} - {_Order.OrderNo} {IIf(_Order.Amend = "", "", $" - {_Order.Amend}")}"

				Dim Details As New Dictionary(Of String, String) From {
					{"Order No", $"{OrderNo}"},
					{"Order Date", _Order.OrderDateFormatted}
				}

				MailBody = GenerateEmailBody(ModuleName:="Purchase Order",
											 Details:=Details,
											 AuthorizedBy:=Thread.CurrentPrincipal.Identity.Name,
											 AuthorizationDate:=New SmartDate(Today.Date).FormattedText)

			End If

			Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(CrystalReport, Engine.ReportClass),
																	 StatusID:=_Order.StatusID,
																	 ShowWatermark:=CBool(AppSettings("ShowWatermark")))

			Return New ReturnMessage(Status:="Success", Message:=$"{MailBody}", Result:=$"{LetterHead(0).Name}", ReportData:=fileContent)

		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception", Message:=$"Error occurred while displaying report. Refer the Error{ex.Message}")
		End Try

	End Function

	Public Function GetReceiptCumInvoiceDetailedReport(ReceiptID As Guid,
													   InvoiceID As Guid,
													   RequestFromAPI As Boolean,
													   Optional ReceiptCumInvoiceObject As ReceiptCumInvoice = Nothing,
													   Optional ByMail As Boolean = False) As (String, String, String, String, Object)

		Try

			Dim MailBody As String = ""
			Dim dataSet As New dsRecCumInvReg
			Dim dataAdapter As New ObjectAdapter
			Dim crystalReport As Engine.ReportClass
			Dim ReceiptCumInvoice As ReceiptCumInvoice
			Dim rptReceiptCumInvoice As rptReceiptCumInvoice
			Dim ReceiptCumInvoiceChildList As rptReceiptCumInvoiceChildList
			Dim SearchingCriteriaForReceipt As rptSearchingCriteriaForReceipt

			If RequestFromAPI Then
				ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=ReceiptID,
																		   InvoiceID:=InvoiceID)
			Else
				ReceiptCumInvoice = CType(ReceiptCumInvoiceObject, ReceiptCumInvoice)
			End If

			rptReceiptCumInvoice = rptReceiptCumInvoice.GetReceiptCumInvoice(ReceiptCumInvoice)

			If CDate(ReceiptCumInvoice.RecCumInvDate) <= CDate("30-Jun-2017") Or ReceiptCumInvoice.Visibility = 3 Then

				If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
					crystalReport = New crptReceiptCumInvoiceIndamar
				ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "TAAL" Then
					crystalReport = New crptReceiptCumInvoiceDetailPortraitTAAL
				ElseIf ((AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "RAL") And (ReceiptCumInvoice.TransTypeID = 7 Or ReceiptCumInvoice.TransTypeID = 10) Then
					crystalReport = New crptGoodReceiptNote
				ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
					crystalReport = New crptReceiptCumInvoiceDetailPortraitForBuddhaAir
				ElseIf (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022  'Added By Prashant 17-Dec-2014 ALL17122014
					crystalReport = New crptReceiptCumInvoiceDetailPortraitDeccan
				ElseIf AppSettings("ClientCode") = "GEP" Then
					crystalReport = New crptReceiptCumInvoiceDetailPortraitForGEPL
				ElseIf AppSettings("ClientCode") = "HSC" Then
					crystalReport = New crptReceiptCumInvoiceDetailPortraitForHeliStar
				Else
					crystalReport = New crptReceiptCumInvoiceDetailPortrait
				End If

			Else
				crystalReport = New crptReceiptCumInvoiceGSTDetail
			End If

			If AppSettings("ClientCode") IsNot Nothing AndAlso AppSettings("ClientCode") = "RAL" And
			   (
					ReceiptCumInvoice.TransTypeID = 7 Or
					ReceiptCumInvoice.TransTypeID = 10
			   ) Then

				ReceiptCumInvoiceChildList = rptReceiptCumInvoiceChildList.GetReceiptCumInvoiceChild(ReceiptCumInvoice:=ReceiptCumInvoice, ClientCode:="RAL")

				SearchingCriteriaForReceipt =
					rptSearchingCriteriaForReceipt.
						GetSearchingCriteriaForReceipt(companyID:=New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"),
													   FromDate:="", ToDate:="", InternalReceiptNo:=AppSettings("ClientCode"),
													   ReleaseNoteNo:="",
													   RecText:=AppSettings("Barcode") = "True", IssText:="",
													   OrdText:=ReceiptCumInvoice.ReceiptCumInvoiceItems.Item(0).OrderItemDetailForReceipt.OrderDateFormatted.ToString,
													   RecNo:="", IssNo:="",
													   OrdNo:=ReceiptCumInvoice.ReceiptCumInvoiceItems.Item(0).OrderItemDetailForReceipt.OrderNumber,
													   Aircraft:="", Supplier:="", Store:="", Status:="", DCNo:="", PartNo:="",
													   Description:="", InvText:="", InvNo:="", FromStore:="", Amend:="",
													   QuotationNo:="", IntOrderNo:="", SerialNo:="", Charge:="", SuppInvNo:="",
													   FromInvDate:="", ToInvDate:="",
													   TransTypeID:=0, WorkShop:=AppSettings("Logo"))
			Else

				ReceiptCumInvoiceChildList = rptReceiptCumInvoiceChildList.GetReceiptCumInvoiceChild(ReceiptCumInvoice:=ReceiptCumInvoice)

				SearchingCriteriaForReceipt =
					rptSearchingCriteriaForReceipt.
						GetSearchingCriteriaForReceipt(companyID:=New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"),
													   FromDate:="", ToDate:="",
													   InternalReceiptNo:=AppSettings("ClientCode"),
													   ReleaseNoteNo:=AppSettings("IssueDate"),
													   RecText:=AppSettings("Barcode") = "True",
													   IssText:="",
													   OrdText:="",
													   RecNo:=AppSettings("FormNumberOnReceipt"),
													   IssNo:=AppSettings("IssueNumber"),
													   OrdNo:=AppSettings("HSNACSCodeVisibleInPartMaster"),
													   Aircraft:="", Supplier:="", Store:="", Status:="", DCNo:="",
													   PartNo:="", Description:="", InvText:="", InvNo:="",
													   FromStore:="", Amend:="", QuotationNo:="", IntOrderNo:="",
													   SerialNo:="", Charge:="", SuppInvNo:="", FromInvDate:="",
													   ToInvDate:="", TransTypeID:=0, WorkShop:=AppSettings("Logo"),
													   WorkOrderText:=AppSettings("PrintBarCodeOnItemDetail"),
													   WorkOrderNo:="")
			End If

			Dim companyLogo As rptImage = rptImage.GetImage(dataSet)

			dataAdapter.Fill(dataSet, rptReceiptCumInvoice)
			dataAdapter.Fill(dataSet, ReceiptCumInvoiceChildList)
			dataAdapter.Fill(dataSet, SearchingCriteriaForReceipt)
			dataAdapter.Fill(dataSet, companyLogo)

			crystalReport.SetDataSource(dataSet)

			If ByMail Then

				Dim rciDetails As New Dictionary(Of String, String) From {
					{"RCI No", $"{ReceiptCumInvoice.ReceiptNo}"},
					{"RCI Date", ReceiptCumInvoice.RecCumInvDateFormatted}
				}

				MailBody = GenerateEmailBody(ModuleName:="Receipt Cum Invoice",
											 Details:=rciDetails,
											 AuthorizedBy:=Thread.CurrentPrincipal.Identity.Name,
											 AuthorizationDate:=New SmartDate(Today.Date).FormattedText)

			End If

			Dim ReceiptNo As String = $"{ReceiptCumInvoice.ReceiptNo}"
			Dim CompanyName As String = $"{SearchingCriteriaForReceipt(0).CompanyName}"

			If RequestFromAPI Then
				Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(crystalReport, Engine.ReportClass))
				Return ("Success", MailBody, ReceiptNo, CompanyName, fileContent)
			End If

			Return ("Success", MailBody, ReceiptNo, CompanyName, crystalReport)

		Catch ex As Exception
			Return ("Exception", "", "Refer the Exception", $"{ex.Message}", Nothing)
		End Try

	End Function

	Public Function GetRequestForQuotationDetailedReport(RequestFromAPI As Boolean,
														 SuppliersCount As Integer,
														 IsVendorDetailsRequired() As Boolean,
														 Optional ByMail As Boolean = False,
														 Optional EnquiryObject As Enquiry = Nothing,
														 Optional ID As String = "{00000000-0000-0000-0000-000000000000}") As ReturnMessage

		Dim _Enquiry As Enquiry
		Dim dataAdapter As New ObjectAdapter
		Dim CrystalReport As Engine.ReportClass
		Dim rptEnquiry As rptEnquiry
		Dim enquiryChilds As rptEnquiryChilds
		Dim letterHead As rptLetterHead
		Dim dataSet As New dsEnquiry
		Dim NoOfSuppliers As Integer = 0
		Dim VendorDetails As String = String.Empty
		Dim companyName As String
		Dim MailBody As String

		Try

			If RequestFromAPI Then
				_Enquiry = Enquiry.GetEnquiry(ID:=New Guid(ID))
			Else
				_Enquiry = CType(EnquiryObject, Enquiry)
			End If

			If (
					(CType(_Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation) Or
					(CType(_Enquiry.TransTypeID, Trans) = Trans.RentialLeaseEnquiry) Or
					(CType(_Enquiry.TransTypeID, Trans) = Trans.OverHaulRepairEnquiry)
			   ) Then

				If AppSettings("ClientCode") = "BA" Then

					If CType(_Enquiry.TransTypeID, Trans) = Trans.RequestingForQuotation Then
						CrystalReport = New crptDeccanEnquiryDetailPortraitForBA  'Req No is in it
					Else
						CrystalReport = New crptDeccanEnquiryDetailPortrait
					End If

				Else
					CrystalReport = New crptDeccanEnquiryDetailPortrait
				End If

			Else
				CrystalReport = New crptEnquiryDetailPortrait
			End If

			For i As Integer = 0 To SuppliersCount - 1

				If IsVendorDetailsRequired IsNot Nothing AndAlso IsVendorDetailsRequired(i) Then

					NoOfSuppliers += 1
					VendorDetails = "<b>" + _Enquiry.EnquirySuppliers(i).VendorName + "</b>" + "</br>" +
									_Enquiry.EnquirySuppliers(i).VendorAddress + "</br>" +
									IIf(_Enquiry.EnquirySuppliers(i).Phone <> "", "Tel No: " + _Enquiry.EnquirySuppliers(i).Phone, "") + "</br>" +
									IIf(_Enquiry.EnquirySuppliers(i).VendorMail <> "", "Email: " + _Enquiry.EnquirySuppliers(i).VendorMail, "") + "</br>" +
									IIf(_Enquiry.EnquirySuppliers(i).ContactPerson <> "", "Kind Attn.: " + _Enquiry.EnquirySuppliers(i).ContactPerson, "")

				End If

			Next

			rptEnquiry = rptEnquiry.GetEnquiry(EnquiryID:=_Enquiry.ID)
			enquiryChilds = rptEnquiryChilds.GetEnquiryChilds(EnquiryID:=_Enquiry.ID)

			letterHead = rptLetterHead.GetLetterHeadInfo(ID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
														 ReportName:="",
														 IIf(NoOfSuppliers = 1, "True", "False"),
														 AppSettings("Logo"),
														 VendorDetails)

			If ByMail Then

				Dim Details As New Dictionary(Of String, String) From {
					{"Enquiry No", $"{_Enquiry.EnquiryNo}"},
					{"Enquiry Date", _Enquiry.DateFormatted}
				}

				MailBody = GenerateEmailBody(ModuleName:="Enquiry",
											 Details:=Details,
											 AuthorizedBy:=Thread.CurrentPrincipal.Identity.Name,
											 AuthorizationDate:=New SmartDate(Today.Date).FormattedText)

			End If

			companyName = letterHead(0).Name.Trim

			Dim companyLogo As rptImage = rptImage.GetImage(DataSet:=dataSet)

			dataAdapter.Fill(dataSet, rptEnquiry)
			dataAdapter.Fill(dataSet, enquiryChilds)
			dataAdapter.Fill(dataSet, letterHead)
			dataAdapter.Fill(dataSet, companyLogo)

			CrystalReport.SetDataSource(dataSet)

			If RequestFromAPI Then
				Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(CrystalReport, Engine.ReportClass))
				Return New ReturnMessage(Status:="Success",
										 Message:=$"{MailBody}",
										 Result:=$"{companyName}",
										 ReportData:=fileContent)
			End If

			Return New ReturnMessage(Status:="Success",
									 Message:=$"{MailBody} {companyName}",
									 Result:=CrystalReport)

		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{ex.Message}")
		End Try

	End Function

	'Sankalp 22-09-25
	Public Function GetIssueDetailedReport(Id As Guid,
										   IsForAPI As Boolean,
										   Optional ByMail As Boolean = False) As (Object, String, String)

		Dim mIssue As Issue
		Dim objIssueReceiptItemPeriodChildList As New rptIssueReceiptItemPeriodChildList
		Dim da As New ObjectAdapter
		Dim rpt As Engine.ReportClass
		Dim obj As rptIssues
		Dim objChilds As rptIssueChields
		Dim letter As rptLetterHead
		Dim ds As New dsIssue
		Dim mrptImage As rptImage
		Dim companyName As String
		Dim MailInfo As String

		Try

			mIssue = Issue.GetIssue(Id)

			If AppSettings("ClientCode") = "IRM" Then
				rpt = New crptIssueDetailPotraitIRM

			Else 'Existing Code

				If mIssue.TransTypeID = Flypal.Trans.DisacrdPart Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
						rpt = New crptIssueLandScapeDiscard
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						rpt = New crptIssueDetailPotraitTAALDiscard
					ElseIf AppSettings("ClientCode") = "HSC" Then
						rpt = New crptIssueDetailPotraitDiscardForHeliStar
					Else
						rpt = New crptIssueDetailPotraitDiscard
					End If

				Else

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
						rpt = New crptIssueLandScape
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						rpt = New crptIssueDetailPotraitTAAL
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "YA") Then

						If mIssue.TransTypeID = 16 Then
							rpt = New crptIssueDetailPotraitYA
						Else
							rpt = New crptIssueDetailPotrait
						End If

						objIssueReceiptItemPeriodChildList = rptIssueReceiptItemPeriodChildList.GetPeriodChildList(mIssue.ID)

					ElseIf AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ" Then ' SPZ Code added by Saylee on 13-Jun-2022  'Added By Vikrant On 18-Jun-2014 For Deccan18062014 -1
						rpt = New crptIssueDetailPotraitDeccan
					ElseIf AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then

						If mIssue.TransTypeID = Flypal.Trans.LoanIssueToWorkShop And AppSettings("ClientCode") = "BA" Then
							rpt = New crptIssueDetailPotraitForBAWithLoanIssueWorkshop
						Else
							rpt = New crptIssueDetailPotraitForBA
						End If

					ElseIf AppSettings("ClientCode") = "HSC" Then
						rpt = New crptIssueDetailPotraitForHeliStar
					ElseIf AppSettings("ClientCode") = "STR" Then

						If mIssue.TransTypeID = 60 Then 'Issue To work order As Tools
							rpt = New crptToolsCheckOutStarAir
						Else
							rpt = New crptIssueDetailPotrait
						End If

					Else
						rpt = New crptIssueDetailPotrait
					End If

				End If

			End If

			mrptImage = rptImage.GetImage(ds)
			obj = rptIssues.GetIssues(mIssue.ID)
			objChilds = rptIssueChields.GetIssuechilds(mIssue.ID)

			Dim mSearchstring As String
			If (AppSettings("Barcode") IsNot Nothing) AndAlso AppSettings("Barcode") = "True" Then
				mSearchstring = "True"
			Else
				mSearchstring = "False"
			End If
			Dim ReqDate As String = ""
			Dim ReqNo As String = ""
			If mIssue.ReqDateFormatted IsNot Nothing Then
				ReqDate = mIssue.ReqDateFormatted.ToString
			End If
			If mIssue.ReqTextNo IsNot Nothing Then
				ReqNo = mIssue.ReqTextNo
			End If

			letter = rptLetterHead.GetLetterHeadInfo(ID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
													 ReportName:="",
													 SearchString1:=mSearchstring,
													 SearchString2:=AppSettings("Logo"),
													 SearchString3:=AppSettings("PrintBarCodeOnItemDetail"),
													 ClientCode:=AppSettings("ClientCode"),
													 SearchString4:="",
													 SearchString5:="",
													 SearchString6:=ReqNo,
													 SearchString7:=ReqDate,
													 SearchString8:=mIssue.nWO.WONumber)

			companyName = letter(0).Name.Trim

			Dim BaseCurrencySymbol As String = ""

			If letter.Count > 0 Then
				BaseCurrencySymbol = letter(0).BaseCurrencysymbol
			End If

			da.Fill(ds, obj)
			da.Fill(ds, objChilds)
			da.Fill(ds, letter)
			da.Fill(ds, mrptImage)
			rpt.SetDataSource(ds)


			If ByMail Then

				MailInfo = MailInfo + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri""> Requested Parts/Components has been Issued/Dispatched." + " </font></P> ")
				MailInfo = MailInfo + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Issue No.: <b> " & mIssue.IssueNo.ToString & "</b> Issue Date: <b> " + mIssue.IDateFormatted + "</b> Issued By: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>.</font></P> ")
				MailInfo = MailInfo + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri""> Please View Parts/Components Details in Attached File. </font></P>")
				MailInfo = MailInfo + ("</body></html>")

			End If

			If Not IsForAPI Then
				Return (rpt, companyName, MailInfo)
			Else
				Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(rpt, Engine.ReportClass))
				Return (fileContent, "Success", $"{MailInfo} , {companyName}")
			End If

		Catch ex As Exception
			Return (ex.Message, "Error", "")
		End Try

	End Function

	Public Function RequisitionDetailReport(IsForAPI As Boolean,
											Optional ByMail As Boolean = False,
											Optional EmployeeName As String = "",
											Optional BranchName As String = "",
											Optional FormRevisionNo As String = "",
											Optional FormRevisionDate As String = "",
											Optional RequisitionObject As RequisitionNew = Nothing,
											Optional ID As String = "{00000000-0000-0000-0000-000000000000}") As (String, String, Object)

		Dim CrystalReport As Engine.ReportClass
		Dim dataAdapter As New ObjectAdapter
		Dim dataSet As New DataSet

		Dim CompanyDetail As New CompanyDetail
		Dim RequisitionCustomer As RequisitionCustomer

		Dim ClientCode As String = AppSettings("ClientCode")
		Dim DateFormat As String = AppSettings("DateFormat")
		Dim CustomerName As String
		Dim CustomerAddress As String
		Dim AircraftType As String
		Dim RequiredByDate As String

		Dim MailInfo As String
		Dim _Requisition As RequisitionNew
		Dim AircraftList As New StringBuilder
		Dim WorkOrderNumberList As New StringBuilder
		Dim AircraftTypeList As New StringBuilder

		Dim User As User = UserManagerController.FetchUser()
		Dim UserName = User.Name

		Try

			If IsForAPI Then
				_Requisition = RequisitionNew.GetRequisition(ID:=New Guid(ID))
			Else
				_Requisition = CType(RequisitionObject, RequisitionNew)
			End If

			If ClientCode = "Nova" Then

				CrystalReport = New crptRequisitionDetailNovoAir
				dataSet = New dsRequisitionNew
				dataAdapter.Fill(dataSet, _Requisition)
				dataAdapter.Fill(dataSet, _Requisition.RequisitionItemsNew)

			Else

				If _Requisition.TransTypeID = Trans.EngineeringRequisition Or _Requisition.TransTypeID = Trans.WorkShopRequisition Then

					If _Requisition.ReqTypeID = 1 Then 'Part Request

						Dim IssueAgainstRequisitionItem As IssueAgainstRequisitionItem =
							IssueAgainstRequisitionItem.
								GetIssueAgainstRequisitionItem(RequisitionID:=_Requisition.ID,
															   ClientCode:=ClientCode)
						Dim Hashtable As New Hashtable
						dataSet = New dsIssueAgainstRequisitionItem

						If ClientCode = "STR" Then
							CrystalReport = New crptIssueAgainstRequisitionItemStarAir
						Else
							CrystalReport = New crptIssueAgainstRequisitionItem
						End If

						For i As Integer = 0 To IssueAgainstRequisitionItem.Count - 1

							If Not Hashtable.ContainsValue(IssueAgainstRequisitionItem(i).RegNo) Then

								Hashtable.Add(i, IssueAgainstRequisitionItem(i).RegNo)
								AircraftList.Append(Hashtable(i) + ",")

							End If

						Next

						If AircraftList.Length > 0 Then
							AircraftList.Replace(",", "", AircraftList.Length - 1, 1)
						End If

						dataAdapter.Fill(dataSet, IssueAgainstRequisitionItem)

					Else 'Part Purchase

						If (ClientCode = "Deccan" Or
							ClientCode = "ADeccan" Or
							ClientCode = "IIC" Or
							ClientCode = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 

							CrystalReport = New crptPurchaseOrderAgainstRequisitionItemDeccan
						Else
							CrystalReport = New crptPurchaseOrderAgainstRequisitionItem
						End If

						Dim PurchaseOrderAgainstRequisitionItem As PurchaseOrderAgainstRequisitionItem =
							PurchaseOrderAgainstRequisitionItem.
								GetPurchaseOrderAgainstRequisitionItem(RequisitionID:=_Requisition.ID)

						dataSet = New dsPurchaseOrderAgainstRequisitionItem
						dataAdapter.Fill(dataSet, PurchaseOrderAgainstRequisitionItem)

					End If

				ElseIf _Requisition.TransTypeID = Trans.StoresRequisition Then

					CrystalReport = New crptMaterialReplanishmentNote
					Dim MaterialReplenishmentsNote As MaterialReplanishmentNote =
						MaterialReplanishmentNote.
							GetMaterialReplanishmentNote(ReqID:=_Requisition.ID.ToString)

					dataSet = New dsIssueAgainstRequisitionItem
					dataAdapter.Fill(dataSet, MaterialReplenishmentsNote)

				ElseIf _Requisition.TransTypeID = Trans.PlanningRequisition Then

					Dim Hashtable As New Hashtable
					Dim WONoList As New Hashtable
					Dim AircraftTypeHashtable As New Hashtable
					_Requisition = RequisitionNew.GetRequisition(ID:=_Requisition.ID) ' Need to fetch again to get saved user name. As uer name changed on this page again on authorize if other user authorized it.

					For i As Integer = 0 To _Requisition.RequisitionItemsNew.Count - 1

						If Not Hashtable.ContainsValue(_Requisition.RequisitionItemsNew(i).RegNo) And
						   _Requisition.RequisitionItemsNew(i).RegNo <> "" Then

							Hashtable.Add(i, _Requisition.RequisitionItemsNew(i).RegNo)
							AircraftList.Append(Hashtable(i) + ",")

						End If

						If Not WONoList.ContainsValue(_Requisition.RequisitionItemsNew(i).WONoNRCNo) And
						   _Requisition.RequisitionItemsNew(i).WONoNRCNo <> "" Then

							WONoList.Add(i, _Requisition.RequisitionItemsNew(i).WONoNRCNo)
							WorkOrderNumberList.Append(WONoList(i) + ",")

						End If

						If ClientCode = "STR" Or ClientCode = "IRM" Then

							RequisitionCustomer = RequisitionCustomer.GetCustomer(_Requisition.RequisitionItemsNew(i).MachineID)

							If Not AircraftTypeHashtable.ContainsValue(RequisitionCustomer.AircraftType) And
							   RequisitionCustomer.AircraftType <> "" Then

								AircraftTypeHashtable.Add(i, RequisitionCustomer.AircraftType)
								AircraftTypeList.Append(AircraftTypeHashtable(i) + ",")

							End If

							RequiredByDate = CDate(_Requisition.ReqDate).AddDays(_Requisition.RequisitionItemsNew(i).Days).ToString(DateFormat)

						Else

							If Not _Requisition.RequisitionItemsNew(i).MachineID.Equals(Guid.Empty) Then

								RequisitionCustomer = RequisitionCustomer.GetCustomer(MachineID:=_Requisition.RequisitionItemsNew(i).MachineID)
								CustomerName = RequisitionCustomer.CustomerName
								CustomerAddress = RequisitionCustomer.CustomerAddress
								AircraftType = RequisitionCustomer.AircraftType

								Exit For

							End If

						End If

					Next

					If AircraftList.Length > 0 Then
						AircraftList.Replace(",", "", AircraftList.Length - 1, 1)
					End If

					If WorkOrderNumberList.Length > 0 Then
						WorkOrderNumberList.Replace(",", "", WorkOrderNumberList.Length - 1, 1)
					End If

					If AircraftTypeList.Length > 0 Then
						AircraftTypeList.Replace(",", "", AircraftTypeList.Length - 1, 1)
						AircraftType = AircraftTypeList.ToString
					End If

					If (ClientCode = "BRD" Or ClientCode = "LAMA") Then
						CrystalReport = New crptPlanningRequisitionDetailBRD
					ElseIf (ClientCode = "STR" Or ClientCode = "IRM") Then
						CrystalReport = New crptRequisitionDetailNewForStarAir
					ElseIf ClientCode = "Heligo" Then
						CrystalReport = New crptPlanningRequisitionDetailHeligo
					Else
						CrystalReport = New crptPlanningRequisitionDetail
					End If

					dataSet = New dsRequisitionNew
					dataAdapter.Fill(dataSet, _Requisition)
					dataAdapter.Fill(dataSet, _Requisition.RequisitionItemsNew)

				End If


			End If

			Dim ReportData As New ReportData(CompanyDetail.CompanyName,
											 CompanyDetail.Address,
											 CompanyDetail.Tel1,
											 CompanyDetail.Tel2,
											 CompanyDetail.Fax,
											 CompanyDetail.Email,
											 CompanyDetail.WebSite,
											 ReportName:="",
											 SearchStr1:=_Requisition.RequisitionNo,
											 SearchStr2:=_Requisition.UserName,
											 SearchStr3:=EmployeeName,
											 SearchStr4:=_Requisition.RecommendedBy,
											 SearchStr5:=_Requisition.Supervisor,
											 ProductVersion:=AppSettings("Product Version"),
											 SINote:=AppSettings("SINote"),
											 SearchStr6:=_Requisition.TransTypeID.ToString, ,
											 SearchStr8:=_Requisition.AuthorizedBy,
											 SearchStr9:=IIf(_Requisition.TransTypeID = 65, $"{AircraftList} / {BranchName}", BranchName),
											 SearchStr10:=AppSettings("Logo"),
											 SearchStr11:=AppSettings("ClientCode"),
											 SearchStr12:=CustomerName,
											 SearchStr13:=CustomerAddress,
											 SearchStr14:=AircraftType,
											 SearchStr15:=WorkOrderNumberList.ToString,
											 SearchStr16:=AircraftList.ToString,
											 SearchStr17:=RequiredByDate,
											 SearchStr18:=FormRevisionNo,
											 SearchStr19:=FormRevisionDate)

			Dim CompanyLogo As rptImage = rptImage.GetImage(DataSet:=dataSet)

			dataAdapter.Fill(dataSet, TableName:="rptImage", source:=CompanyLogo)
			dataAdapter.Fill(dataSet, source:=ReportData)

			CrystalReport.SetDataSource(dataSet)

			If ByMail Then

				MailInfo = ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Parts(s) has been requested by <b>" + UserName + "</b>" +
							" in Requisition " + _Requisition.RequisitionNo +
							" ,Created on " + New SmartDate(_Requisition.ReqDateFormatted.ToString).FormattedText + " in FlyPal System." + "</font></P></br> ")

				MailInfo += ("<table BORDER=1 Style=""border-collapse: collapse"" BORDER-COLOR=""black"" ID=""table2"">")
				MailInfo += ("<tr>" & "<td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Sr. No.</b>" & "</font>" &
							 "</td><td align=""center"" width=""200"" style=""background-color: #E4E2E1; color: black;"" >" &
							 "<font face=""Calibri""><b>Part No</b>" &
							 "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #E4E2E1; color: black;"" >" &
							 "<font face=""Calibri""><b>Description</b>" & "</font>" &
							 "</td><td align=""center"" style=""background-color: #E4E2E1; color: black;"">" &
							 "<font face=""Calibri""><b>Qty</b>" & "</font>" &
							 "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" &
							 "<font face=""Calibri""><b>UOM</b>" & "</font>" & "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" &
							 "<font face=""Calibri""><b>Reg</b>" & "</font>" & "</td>  <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" &
							 "<font face=""Calibri""><b>WO.No.</b>" & "</font>" & "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" &
							 "<font face=""Calibri""><b>Requirement Reason</b>" & "</font>" &
							 "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" &
							 "<font face=""Calibri""><b>Remark</b>" & "</font>" & "</td></tr>")

				For i As Integer = 0 To _Requisition.RequisitionItemsNew.Count - 1

					MailInfo += ("<tr>")
					MailInfo += ("<td width=20px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).SrNo.ToString) + "."
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=200px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).PartNo)
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=200px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).Description)
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=50px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).RequestedQty.ToString)
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=50px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).Unit.ToString)
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=20px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).RegNo.ToString)
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=20px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += IIf(_Requisition.RequisitionItemsNew(i).WONo.ToString = "",
									"-",
									_Requisition.RequisitionItemsNew(i).WONo.ToString)
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=50px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).ReasonForRequest.ToString)
					MailInfo += ("</font>")
					MailInfo += ("</td>")

					MailInfo += ("<td width=50px >")
					MailInfo += ("<font face=""Calibri"">")
					MailInfo += (_Requisition.RequisitionItemsNew(i).Remark.ToString)
					MailInfo += ("</font>")
					MailInfo += ("</td>")
					MailInfo += ("</tr>")

				Next

				MailInfo += ("</table>")
				MailInfo += ("<p><font face=""Calibri"">")
				MailInfo += ("<font face=""Calibri"">Please Login to FlyPal® for detailed Information." + "</font> ")
				MailInfo += ("</body></html>")

			End If

			If Not IsForAPI Then

				Return ("Success", MailInfo, CrystalReport)

			Else

				Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(CrystalReport, Engine.ReportClass))

				Return ("Success", MailInfo, fileContent)

			End If

		Catch ex As Exception
			Return ("Exception", ex.Message, Nothing)
		End Try

	End Function

	'Sankalp 11-12-25
	Public Function DisplayDocketChargeReport(DocketChargeID As Guid) As ReturnMessage

		Dim companyLogo As rptImage
		Dim letterHead As rptLetterHead
		Dim dataSet As New dsOtherCharge
		Dim OtherCharges As rptOtherCharges
		Dim dataAdapter As New ObjectAdapter
		Dim crystalReport As New crptOtherCharge
		Dim OtherChargeChilds As rptOtherChargeChilds

		Try

			OtherCharges = rptOtherCharges.GetOtherChargse(ID:=DocketChargeID)
			OtherChargeChilds = rptOtherChargeChilds.GetOtherChargeChilds(ID:=DocketChargeID)

			letterHead = rptLetterHead.GetLetterHeadInfo(ID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
														 ReportName:="",
														 SearchString1:="",
														 SearchString2:=AppSettings("Logo"))

			companyLogo = rptImage.GetImage(dataSet)

			dataAdapter.Fill(dataSet, OtherCharges)
			dataAdapter.Fill(dataSet, OtherChargeChilds)
			dataAdapter.Fill(dataSet, companyLogo)
			dataAdapter.Fill(dataSet, letterHead)
			crystalReport.SetDataSource(dataSet)

			Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(crystalReport, Engine.ReportClass))
			Return New ReturnMessage(Status:="Success", Message:="Report displayed Successfully!!", Result:=fileContent)

		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception", Message:=$"Error occurred while displaying report. Refer the Error{ex.Message}")
		End Try

	End Function

#End Region

#Region " Tag Report Method(s) "

	Public Function GetReceiptCumInvoiceTagReport(ReceiptID As Guid) As ReturnMessage

		Try

			Dim obj As rptStoresAcceptanceTag
			Dim letter As rptLetterHead
			Dim mModuleList As ModuleList
			mModuleList = ModuleList.GetModuleList(AddTopItem:="Select")

			obj = rptStoresAcceptanceTag.GetStoresAcceptanceTag(ReceiptID:=ReceiptID)

			letter = rptLetterHead.GetLetterHeadInfo(ID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
													 ReportName:="",
													 SearchString1:=AppSettings("WODocumentNo"),
													 SearchString2:=AppSettings("WORevisionNo"),
													 SearchString3:=AppSettings("Barcode"),
													 ClientCode:=AppSettings("ClientCode"),
													 SearchString4:=mModuleList.Item("Acceptance Tag").FormRevisionNo)

			Dim da As New ObjectAdapter
			Dim myReport As Engine.ReportClass

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Taj" Or AppSettings("ClientCode") = "HSC" Then

				If (AppSettings("Barcode") IsNot Nothing) AndAlso AppSettings("Barcode") = "True" Then
					myReport = New crptStoreAcceptanceTag6
				Else
					myReport = New crptStoreAcceptanceTag6WithoutBarcode
				End If

			ElseIf AppSettings("ClientCode") = "CE" Or AppSettings("ClientCode") = "Heligo" Then
				myReport = New crptServiceableUnserviceableTagForCE
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
				myReport = New crptStoreAcceptanceTagYATA
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Novo" Then
				myReport = New crptStoreAcceptanceTagNOVO
			ElseIf AppSettings("ClientCode") = "IRM" Then
				myReport = New crptStoreAcceptanceTagIRM
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "IND" Then
				myReport = New crptStoreAcceptanceTagIND
			ElseIf AppSettings("ClientCode") = "PTW" Then
				myReport = New crptStoreAcceptanceTagForPattaya
			ElseIf AppSettings("ClientCode") = "7AR" Then
				myReport = New crptStoreAcceptanceTagWithoutBarcodeFor7Air
			Else

				If CBool(AppSettings("Barcode")) Then
					myReport = New crptStoreAcceptanceTag1
				Else
					myReport = New crptStoreAcceptanceTag1WithoutBarcode
				End If

			End If

			Dim ds As New dsStoresAcceptanceTag

			Dim CompanyLogo As rptImage = rptImage.GetImage(ds)

			da.Fill(ds, obj)
			da.Fill(ds, letter)
			da.Fill(ds, CompanyLogo)

			myReport.SetDataSource(ds)

			Dim fileContent As Byte() = ConvertCrystalReportToBinary(Report:=CType(myReport, Engine.ReportClass))

			Return New ReturnMessage(Status:="Success", Message:="", ReportData:=fileContent)

		Catch ex As Exception
			Return New ReturnMessage(Status:="Error", Message:=ex.Message)
		End Try

	End Function

#End Region

End Class
