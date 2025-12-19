
'Created by Saylee 24-Jul-2018 for ALL01082018


Imports System.Configuration.ConfigurationManager
Imports System.Collections.Generic
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing
Imports System.Linq
Imports System.Text


Public Class wfrptEngineOilConsumptionForMonth_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mMachineNameValueList As MachineNameValueList
    Public mEngineOilConsumptionForMonth As EngineOilConsumptionForMonth
    Dim EventLogDetail As String
    Dim MachineID As Guid
    Dim mAssemblylist As AssemblyList
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim stream As MemoryStream = New MemoryStream
    Public mEngineOilConsumptionForMonthSummary As EngineOilConsumptionForMonthSummary 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mAssemblylist") = mAssemblylist
    End Sub
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
        mAssemblylist = Session("mAssemblylist")
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If

        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
    End Sub
    Private Sub DataFieldBinding()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mAssemblylist")
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
        lblAssembly1.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            If cmbAssemblyList.SelectedItem.Text = "(SELECT)" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                lblAssembly1.Text = "Assembly Name  : " + "<b> All </b>"         'Added Code
            Else
                AssemblyName = cmbAssemblyList.SelectedValue.ToString
                Assembly1 = cmbAssemblyList.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & "<b>" + Assembly1 + "</b>"  'Added Code
            End If
        End If

        lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        MachineID = New Guid(Request.Form("cmbAircraft").ToString)
        lblModel1.Text = "Aircraft : " & IIf(MachineID.Equals(Guid.Empty), "", mMachineNameValueList(MachineID).RegNo)
        EventLogDetail = lblyear1.Text + ", " + lblModel1.Text
    End Sub

    Private Function GetColor(ByVal i As Integer) As System.Drawing.Color
        Select Case i

            Case 0
                Return Drawing.Color.Brown
            Case 1
                Return Drawing.Color.Blue
            Case 2
                Return Drawing.Color.Green
            Case 3
                Return Drawing.Color.Yellow
            Case 4
                Return Drawing.Color.Orange
            Case 5
                Return Drawing.Color.Silver
            Case 6
                Return Drawing.Color.Purple
            Case 7
                Return Drawing.Color.Red
            Case 8
                Return Drawing.Color.Orchid
            Case 9
                Return Drawing.Color.YellowGreen
            Case 10
                Return Drawing.Color.Gold
            Case 11
                Return Drawing.Color.BlanchedAlmond
            Case 12 To 31
                Return New System.Drawing.Color()

        End Select
    End Function
    Public Sub SetLineGraph()
        ChartLine.Visible = True
        Dim ChartArea1 As New ChartArea
        Dim Legend1 As New Legend
        Dim Title1 As New Title
        Dim Series1 As New Series
        Dim ds As New dsEngineOilConsumptionForMonth
        SetValues()

        'Dim mLogDateMonthwise As LogDateMonthwise = LogDateMonthwise.GetLogDate(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)
        'Dim xValues As String()
        'For i As Integer = 0 To mLogDateMonthwise.Count - 1
        '    If i = 0 Then ReDim xValues(mLogDateMonthwise.Count - 1)
        '    xValues(i) = mLogDateMonthwise(i).DateFormatted
        'Next
        'ChartLine.Series.Add("Series1")

        Dim j As Integer = 0
        Dim IsForSingleAssembly As Boolean = False
        For k As Integer = 1 To mAssemblylist.Count - 1
            Dim AssemblyID As Guid = Guid.Empty

            If AssemblyName = Guid.Empty.ToString Then
                AssemblyID = mAssemblylist(k).ID
            Else
                AssemblyID = New Guid(AssemblyName)
                IsForSingleAssembly = True
            End If

            If mAssemblylist(k).ID = AssemblyID Then
                j = j + 1
                Dim mEngineConsumption As EngineOilConsumptionForMonth = EngineOilConsumptionForMonth.GetEngineOilConsumptionForMonth(MachineID.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , , mAssemblylist(k).ID.ToString)
                Dim mEngine As List(Of EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo) = New List(Of EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo)

                mEngine = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                         Order By c.LogDate
                                         Select c).ToList

                ChartLine.Series.Add(mAssemblylist(k).SerialNo)
                ChartLine.Series(mAssemblylist(k).SerialNo).ChartType = SeriesChartType.Line
                ChartLine.Series(mAssemblylist(k).SerialNo).IsVisibleInLegend = True
                ChartLine.Series(mAssemblylist(k).SerialNo).EmptyPointStyle.IsValueShownAsLabel = True
                ChartLine.Series(mAssemblylist(k).SerialNo).EmptyPointStyle.IsVisibleInLegend = False
                ChartLine.Series(mAssemblylist(k).SerialNo).SmartLabelStyle.Enabled = True
                ChartLine.Series(mAssemblylist(k).SerialNo).MarkerStyle = MarkerStyle.Circle

                Dim LastLogDate = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                                    Order By c.LogDate Descending
                                                    Where c.IsOrigional = True
                                                    Select c.LogDateFormatted).FirstOrDefault

                Dim IsForFirstTime As Boolean = True
                Dim ExitForLoop As Boolean = False

                If AppSettings("ClientCode") = "Novo" And rdoSummary.Checked Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                    Dim MonthWeeks As Integer() = {6, 12, 18, 24}
                    Dim AvgHourRate As Decimal
                    Dim HrsSum
                    Dim OilsSum


                    For i As Integer = 0 To MonthWeeks.Length
                        If LastLogDate = "" Then 'No Logs Entered
                            Exit For
                        Else 'Logs Entered for Month
                            'If i <> MonthWeeks.Length Then
                            '    If MonthWeeks(i) <= Day(CDate(LastLogDate)) Or i = 0 Then
                            '        ''Do nothing 
                            '    Else
                            '        If (Not IsForFirstTime) Then
                            '            Exit For
                            '        Else
                            '            If MonthWeeks(i - 1) = Day(CDate(LastLogDate)) Then
                            '                Exit For
                            '            End If
                            '        End If
                            '        IsForFirstTime = False
                            '    End If
                            'Else
                            '    If Day(CDate(LastLogDate)) > MonthWeeks(i - 1) Then
                            '        ''Do nothing 
                            '    Else
                            '        Exit For
                            '    End If
                            'End If
                            If i = 0 Then 'Add entry as it is on graph
                                If Day(CDate(LastLogDate)) <= MonthWeeks(i) Then 'Last Log date falls in week so dont go for next week
                                    ExitForLoop = True
                                End If
                            ElseIf i = 1 Then 'Check for last log date
                                If Day(CDate(LastLogDate)) <= MonthWeeks(i) Then 'Last Log date falls in week so dont go for next week
                                    ExitForLoop = True
                                End If
                            ElseIf i = 2 Then 'Check for last log date
                                If Day(CDate(LastLogDate)) <= MonthWeeks(i) Then 'Last Log date falls in week so dont go for next week
                                    ExitForLoop = True
                                End If
                            ElseIf i = 3 Then 'Check for last log date
                                If Day(CDate(LastLogDate)) <= MonthWeeks(i) Then 'Last Log date falls in week so dont go for next week
                                    ExitForLoop = True
                                End If
                                'ElseIf i = 4 Then 'Check for last log date
                                '    If Day(CDate(LastLogDate)) <= MonthWeeks(i) Then 'Last Log date falls in week so dont go for next week
                                '        ExitForLoop = True
                                '    End If
                            End If
                        End If


                        'IsForFirstTime = False
                        If i = 0 Then
                            'AvgHourRate = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                            '                          Order By c.LogDate
                            '                          Where (Day(c.LogDate) <= MonthWeeks(i) And c.IsOrigional = True)
                            '                          Select c.HourRate).Sum() / 6


                            HrsSum = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                                Order By c.LogDate
                                                Where (Day(c.LogDate) <= MonthWeeks(i) And c.IsOrigional = True)
                                                Select c.TimeInAirHourDecimal).Sum()

                            OilsSum = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                               Order By c.LogDate
                                               Where (Day(c.LogDate) <= MonthWeeks(i) And c.IsOrigional = True)
                                               Select c.OilValue).Sum()
                            If HrsSum > 0 Then
                                AvgHourRate = OilsSum / HrsSum
                            Else
                                AvgHourRate = 0
                            End If

                        Else
                            If i = MonthWeeks.Length Then
                                'AvgHourRate = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                '                           Order By c.LogDate
                                '                           Where Day(c.LogDate) > MonthWeeks(i - 1)
                                '                           Select c.HourRate).Sum() / (DateTime.DaysInMonth(CInt(cmbYear.SelectedValue), cmbMonth.SelectedIndex + 1) - 24)
                                HrsSum = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                                Order By c.LogDate
                                                Where (Day(c.LogDate) > MonthWeeks(i - 1) And c.IsOrigional = True)
                                                Select c.TimeInAirHourDecimal).Sum()

                                OilsSum = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                                   Order By c.LogDate
                                                   Where (Day(c.LogDate) > MonthWeeks(i - 1) And c.IsOrigional = True)
                                                   Select c.OilValue).Sum()
                                If HrsSum > 0 Then
                                    AvgHourRate = OilsSum / HrsSum
                                Else
                                    AvgHourRate = 0
                                End If
                            Else
                                'AvgHourRate = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                '                           Order By c.LogDate
                                '                           Where Day(c.LogDate) > MonthWeeks(i - 1) And Day(c.LogDate) <= MonthWeeks(i)
                                '                           Select c.HourRate).Sum() / 6
                                HrsSum = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                               Order By c.LogDate
                                               Where (Day(c.LogDate) > MonthWeeks(i - 1) And Day(c.LogDate) <= MonthWeeks(i) And c.IsOrigional = True)
                                               Select c.TimeInAirHourDecimal).Sum()

                                OilsSum = (From c As EngineOilConsumptionForMonth.EngineOilConsumptionForMonthInfo In mEngineConsumption.AsParallel
                                                   Order By c.LogDate
                                                   Where (Day(c.LogDate) > MonthWeeks(i - 1) And Day(c.LogDate) <= MonthWeeks(i) And c.IsOrigional = True)
                                                   Select c.OilValue).Sum()
                                If HrsSum > 0 Then
                                    AvgHourRate = OilsSum / HrsSum
                                Else
                                    AvgHourRate = 0
                                End If
                            End If
                        End If

                        If i = MonthWeeks.Length Then
                            If Day(CDate(LastLogDate)) < DateTime.DaysInMonth(CInt(cmbYear.SelectedValue), cmbMonth.SelectedIndex + 1) Then
                                ChartLine.Series(mAssemblylist(k).SerialNo).Points.AddXY(Day(CDate(LastLogDate)), AvgHourRate)
                            Else
                                ChartLine.Series(mAssemblylist(k).SerialNo).Points.AddXY(DateTime.DaysInMonth(CInt(cmbYear.SelectedValue), cmbMonth.SelectedIndex + 1), AvgHourRate)
                            End If

                        Else
                            If Day(CDate(LastLogDate)) < MonthWeeks(i) Then
                                ChartLine.Series(mAssemblylist(k).SerialNo).Points.AddXY(Day(CDate(LastLogDate)), AvgHourRate)
                            Else
                                ChartLine.Series(mAssemblylist(k).SerialNo).Points.AddXY(MonthWeeks(i), AvgHourRate)
                            End If
                        End If

                        ChartLine.Series(mAssemblylist(k).SerialNo).LegendText = mAssemblylist(k).ModelSerialNoPostion
                        ChartLine.Series(mAssemblylist(k).SerialNo).LabelAngle = -90
                        'ChartLine.Series("Series1").Color = GetColor(i)
                        ChartLine.Series(mAssemblylist(k).SerialNo).Color = GetColor(j)
                        ChartLine.Series(mAssemblylist(k).SerialNo).Points([i]).Color = GetColor(j)
                        If AvgHourRate = 0.0 Then
                            ChartLine.Series(mAssemblylist(k).SerialNo).Points([i]).MarkerSize = 0
                        End If
                        If ExitForLoop Then
                            Exit For
                        End If

                    Next
                Else 'Existing
                    For i As Integer = 0 To mEngine.Count - 1
                        ChartLine.Series(mAssemblylist(k).SerialNo).Points.AddXY(mEngine(i).LogDate, mEngine(i).HourRate)
                        ChartLine.Series(mAssemblylist(k).SerialNo).LegendText = mEngine(i).AssemblyName
                        ChartLine.Series(mAssemblylist(k).SerialNo).LabelAngle = -90
                        'ChartLine.Series("Series1").Color = GetColor(i)
                        ChartLine.Series(mAssemblylist(k).SerialNo).Color = GetColor(j)
                        ChartLine.Series(mAssemblylist(k).SerialNo).Points([i]).Color = GetColor(j)
                        If mEngine(i).HourRate = 0.0 Then
                            ChartLine.Series(mAssemblylist(k).SerialNo).Points([i]).MarkerSize = 0
                        End If
                    Next
                End If


                If j = 1 Then
                    Dim TrendLine As New System.Web.UI.DataVisualization.Charting.Series()
                    'Dim TrendLine As New System.Web.UI.DataVisualization.Charting.Series("Limit: 0.27 QTZ/HR AS PER P & WC EMM 72-00-00")

                    'If AppSettings("ClientCode") = "Novo" Then
                    '    TrendLine.Name = "Limit: 0.27 QTZ/HR AS PER P & WC EMM 72-00-00"
                    '    TrendLine.LegendText = "Limit: 0.27 QTZ/HR AS PER P & WC EMM 72-00-00"
                    'End If

                    With TrendLine
                        .ChartType = SeriesChartType.Line
                        .Color = Color.Red
                        .BorderWidth = 1
                        .IsValueShownAsLabel = True
                        .IsVisibleInLegend = True
                        .Name = "Red Line"
                        .IsVisibleInLegend = False
                    End With

                    ChartLine.ChartAreas(0).AxisY.Interval = 0.1

                    If AppSettings("ClientCode") = "Novo" And rdoSummary.Checked Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                        ChartLine.ChartAreas(0).AxisX.Interval = 6
                        ChartLine.ChartAreas(0).AxisX.IsStartedFromZero = True
                        ChartLine.ChartAreas(0).AxisX.Minimum = 0
                        Dim Weeks As Integer() = {6, 12, 18, 24}
                        'ChartLine.Series("Limit: 0.27 QTZ/HR AS PER P & WC EMM 72-00-00").Points.AddXY(0, 0.27)
                        IsForFirstTime = True
                        For i As Integer = 0 To Weeks.Length - 1
                            If LastLogDate = "" Then
                                Exit For
                            Else
                                If Weeks(i) <= Day(CDate(LastLogDate)) Then
                                    ''Do nothing 
                                Else
                                    If Not IsForFirstTime Then
                                        Exit For
                                    End If
                                    IsForFirstTime = False
                                End If
                            End If
                            ChartLine.Series("TrendLine").Points.AddXY(Weeks(i), 0.27)
                        Next
                        ChartLine.Series("TrendLine").Points.AddXY(DateTime.DaysInMonth(CInt(cmbYear.SelectedValue), cmbMonth.SelectedIndex + 1), 0.27)
                        ChartLine.Series("TrendLine").Name = "Limit: 0.27 QTZ/HR AS PER P & WC EMM 72-00-00"
                    ElseIf AppSettings("ClientCode") = "Novo" And rdoDetail.Checked Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                        For i As Integer = 0 To mEngine.Count - 1
                            ChartLine.Series("TrendLine").Points.AddXY(mEngine(i).LogDate, 0.27)
                        Next
                        ChartLine.Series("TrendLine").Name = "Limit: 0.27 QTZ/HR AS PER P & WC EMM 72-00-00"
                    Else 'Existing
                        For i As Integer = 0 To mEngine.Count - 1
                            ChartLine.Series("TrendLine").Points.AddXY(mEngine(i).LogDate, 1.28)
                        Next
                    End If


                End If

                If IsForSingleAssembly = True Then
                    Exit For
                End If
            End If
        Next


        ChartLine.DataSource = ds.Tables("mEngine1Consumption")

        ChartLine.DataBind()
        ChartLine.SaveImage(stream, ChartImageFormat.Png)
        upnlLine.Update()
    End Sub

    Public Sub ExportToPdf()

        Try


            SetLineGraph()
            Dim dsLine As New dsEngineOilUpliftForMonth
            Dim da As New CSLA.Data.ObjectAdapter

            Dim mCompanyDetail As New CompanyDetail
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

            Dim MyFile2 = "C:\Temp\" & mMachineNameValueList(MachineID).RegNo & "Graph" & ".pdf"

            Dim pdfDoc As iTextSharp.text.Document = New iTextSharp.text.Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
            Dim mPDFWriter As PdfWriter

            mPDFWriter = PdfWriter.GetInstance(pdfDoc, New FileStream(MyFile2, FileMode.Create)) 'Response.OutputStream
            pdfDoc.Open()

            Dim mrptImage As rptImage
            mrptImage = rptImage.GetImage(dsLine)

            MachineID = New Guid(Request.Form("cmbAircraft").ToString)

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Graphical Representation of Flying Hours", "Detail for" + " " + mMachineNameValueList(MachineID).RegNo, cmbYear.SelectedItem.Text, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))  'Changed By Utkarsh For Report Logo.


            da.Fill(dsLine, Report)
            da.Fill(dsLine, mrptImage)

            '''Header
            Dim DataTable As PdfPTable = New PdfPTable(4)

            Dim Header_1 As New PdfPCell '= New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
            Dim Header_2 As PdfPCell = New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Oil Consumption Report", FontFactory.GetFont("Tahoma", 11, 1)))
            If Not mrptImage Is Nothing Then
                Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(mrptImage(0).ImageFile)
                image.ScaleToFit(60, 60)
                image.Alignment = 0
                Header_1.AddElement(image)
                Header_1.Border = iTextSharp.text.Rectangle.NO_BORDER
                Header_1.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
                Header_1.Colspan = 1
                DataTable.AddCell(Header_1)
            End If

            'Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY


            Header_2.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_2.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER
            Header_2.Colspan = 3


            DataTable.AddCell(Header_2)
            DataTable.WidthPercentage = 100
            pdfDoc.Add(DataTable)




            '''**********************************

            '''Criteria
            Dim DataTable1 As PdfPTable = New PdfPTable(2)
            Dim Header_3 As PdfPCell = New PdfPCell(New Phrase(lblyear1.Text, FontFactory.GetFont("Tahoma", 8, 1)))
            Dim Header_4 As PdfPCell = New PdfPCell(New Phrase(lblModel1.Text, FontFactory.GetFont("Tahoma", 8, 1)))
            Dim Header_5 As PdfPCell
            If AppSettings("ClientCode") = "Novo" Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                Header_5 = New PdfPCell(New Phrase("Red Line : 0.27 Quarts per Hour", FontFactory.GetFont("Tahoma", 8, 1)))
            Else 'Existing
                Header_5 = New PdfPCell(New Phrase("Red Line : 1.28 Quarts per Hour", FontFactory.GetFont("Tahoma", 8, 1)))
            End If


            'Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY
            'Year
            Header_3.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_3.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_3.Colspan = 2

            'Aircraft
            Header_4.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_4.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_4.Colspan = 2

            'Red Line
            Header_5.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_5.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_5.Colspan = 2


            DataTable1.WidthPercentage = 95
            DataTable1.AddCell(Header_3)
            DataTable1.AddCell(Header_4)
            DataTable1.AddCell(Header_5)

            pdfDoc.Add(DataTable1)
            '''**********************************


            '''Chart
            Dim chartImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(stream.GetBuffer)
            chartImage.ScalePercent(75.0!)
            chartImage.Alignment = Element.ALIGN_MIDDLE

            Dim p1 As Paragraph = New Paragraph()
            p1.Alignment = Element.ALIGN_CENTER

            pdfDoc.Add(p1)


            chartImage.SetAbsolutePosition(0, pdfDoc.PageSize.Height / 2)
            pdfDoc.Add(chartImage)
            '************************************


            '''Footer
            Dim table As New PdfPTable(2)
            table.WidthPercentage = 95

            Dim Product As PdfPCell = New PdfPCell(New Phrase(AppSettings("Product Version"), FontFactory.GetFont("Tahoma", 8, 1)))
            Dim SINote As PdfPCell = New PdfPCell(New Phrase(AppSettings("SINote"), FontFactory.GetFont("Tahoma", 8, 1)))

            Product.Border = iTextSharp.text.Rectangle.NO_BORDER
            Product.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Product.Colspan = 1
            SINote.Border = iTextSharp.text.Rectangle.NO_BORDER
            SINote.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_RIGHT

            SINote.Colspan = 1
            table.AddCell(Product)
            table.AddCell(SINote)

            'table.SetWidthPercentage(95.0)

            table.TotalWidth = 580.0F

            table.WriteSelectedRows(0, -1, 0, 50, mPDFWriter.DirectContent)

            'pdfDoc.Add(table)
            '************************************

            'Response.ContentType = "application/pdf"
            'Response.AddHeader("content-disposition", "attachment;filename=Chart.pdf")
            'Response.Cache.SetCacheability(HttpCacheability.NoCache)


            pdfDoc.Close()

            'Response.Write(pdfDoc)

            Dim pdfList As New System.Collections.ArrayList
            pdfList.Add(MyFile2)
            Dim PDFNo As Integer = 1
            PDFNo = PDFNo + 1



            'Tabular Presentation 
            da = New CSLA.Data.ObjectAdapter

            Dim ds As New dsEngineOilConsumptionForMonth
            SetValues()
            Dim myReport

            If rdoSummary.Checked Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                myReport = New crEngineOilConsumptionForMonthSummary
            Else 'Existing
                myReport = New crEngineOilConsumptionForMonth
            End If
            'End
            'Crystal Report

            'Dim mEngineOilUpliftForMonth As EngineOilUpliftForMonth
            'mEngineOilUpliftForMonth = EngineOilUpliftForMonth.GetEngineOilUpliftForMonth(MachineID.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer))

            If rdoSummary.Checked Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                mEngineOilConsumptionForMonthSummary = EngineOilConsumptionForMonthSummary.GetEngineOilConsumptionForMonth(MachineID.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , , AssemblyName)
            Else 'Existing
                mEngineOilConsumptionForMonth = EngineOilConsumptionForMonth.GetEngineOilConsumptionForMonth(MachineID.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , , AssemblyName)
            End If
            'End

            Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                     mCompanyDetail.WebSite, "", cmbMonth.SelectedItem.Text, cmbYear.SelectedItem.Text, mMachineNameValueList(MachineID).RegNo, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))


            If rdoDetail.Checked Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                If mEngineOilConsumptionForMonth.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1278)
                End If

            ElseIf rdoSummary.Checked Then
                If mEngineOilConsumptionForMonthSummary.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1278)
                End If
            End If

            ds.Clear()
            mrptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            If rdoSummary.Checked Then 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
                da.Fill(ds, mEngineOilConsumptionForMonthSummary)
            Else 'Existing
                da.Fill(ds, mEngineOilConsumptionForMonth)
            End If

            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport


            Dim tmp As Integer
            Dim a As New Random

            tmp = a.Next

            'Dim MyFile1 = "C:\Temp\" & tmp & PDFNo.ToString & ".pdf"
            Dim MyFile1 = "C:\Temp\" & mMachineNameValueList(MachineID).RegNo & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions


            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            Dim pageCount As Integer = 0

            pdfList.Add(MyFile1)
            Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
            Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

            Dim filesByte As New List(Of Byte())()
            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
            Next

            File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

            AddWatermarkText(MergedPath, MergedPath_WM, mMachineNameValueList(MachineID).RegNo, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
            ''//********************************************Set Sessions*********************************************************//
            Session("CrystalReport") = MergedPath_WM
            Session("PrintReportWithAttachment") = "True"
            Dim DeleteThis As String = mMachineNameValueList(MachineID).RegNo
            Dim Files As String() = Directory.GetFiles("C:\Temp\")

            For Each file__1 As String In Files
                If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                    File.Delete(file__1)
                End If
            Next
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Added By Vikrant On 27-Mar-2019 For NOVO27032019
    Private Sub ControlVisibility()
        If AppSettings("ClientCode") = "Novo" Then
            lblSummaryDetail.Visible = True
            rdoDetail.Visible = True
            rdoSummary.Visible = True
            lblStep4.Text = "Step V. Display Report"
        Else
            lblSummaryDetail.Visible = False
            rdoDetail.Visible = False
            rdoSummary.Visible = False
            lblStep4.Text = "Step IV. Display Report"
        End If
    End Sub
    'End
#End Region


#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not Page.IsPostBack Then

            lblAssembly.Enabled = False
            cmbAssemblyList.Enabled = False
            SetCombo()
            DataFieldBinding()
            ControlVisibility() 'Added By Vikrant On 27-Mar-2019 For NOVO27032019
        End If

        Dim scriptManager As ScriptManager = scriptManager.GetCurrent(Me.Page)
        scriptManager.RegisterPostBackControl(Me.btnPrint)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssemblyList.Enabled = False

            cmbAssemblyList.SelectedIndex = 0
        Else
            lblAssembly.Enabled = True
            cmbAssemblyList.Enabled = True
            Dim mdate As New Date(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, Date.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1))
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(2, cmbAircraft.SelectedValue, mdate.ToString, "(SELECT)", True)
            cmbAssemblyList.DataSource = mAssemblylist
            cmbAssemblyList.DataBind()
            Session("mAssemblylist") = mAssemblylist
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetLineGraph()
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Page.IsValid Then
            ExportToPdf()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbMonth_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonth.SelectedIndexChanged, cmbYear.SelectedIndexChanged
        Dim mdate As New Date(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, Date.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1))
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(2, cmbAircraft.SelectedValue, mdate.ToString, "(SELECT)", True)
        cmbAssemblyList.DataSource = mAssemblylist
        cmbAssemblyList.DataBind()
        Session("mAssemblylist") = mAssemblylist
    End Sub
#End Region



End Class