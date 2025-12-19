Imports System.Configuration.ConfigurationManager

Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing

Public Class wfSearchCriteriaForFlyingHrs_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Year As String
    Dim Type As Integer = 0
    Dim Aircraft As String
    Dim SelectYear As String

    Dim da As New CSLA.Data.ObjectAdapter
    Dim ds As New dsGraFlyingHrs
    Dim dsLine As New dsLineFlyingHrs
    Dim ReportPieGraph As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim ReportBarGraph As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim ReportLineGraph As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail
    Dim obj As ReportFlyingHrs
    Dim MonthlyTrendList As ReportMonthlyTrendList
    Dim mMachineList As MachineList

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForFlyingHrs_Ajax.aspx?" Then
            Session.Remove("mMachineList")
        End If
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Function GetColor(ByVal i As Integer) As System.Drawing.Color
        Select Case i

            Case 0
                Return Drawing.Color.Brown
            Case 1
                Return Drawing.Color.Orange
            Case 2
                Return Drawing.Color.Yellow
            Case 3
                Return Drawing.Color.Green
            Case 4
                Return Drawing.Color.Blue
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
            Case 12 To 60
                Return New System.Drawing.Color()

        End Select
    End Function
    Public Sub SetLineGraph(MonthlyTrendList As ReportMonthlyTrendList)

        ChartLine.Visible = True
        Dim ChartArea1 As New ChartArea
        Dim Legend1 As New Legend
        Dim Title1 As New Title
        Dim Series1 As New Series

        Dim xValues As String() = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        For i As Integer = 0 To MonthlyTrendList.Count - 1
            ChartLine.Series("Series1").Points.AddXY(xValues(i), IIf(MonthlyTrendList(i).FlyingHrs <> 0, CDec(Format(MonthlyTrendList(i).FlyingHrs / 60, "###0.00")), 0))
            ChartLine.Series("Series1").IsValueShownAsLabel = True
            ChartLine.Series("Series1").LegendText = String.Empty
            ChartLine.Series("Series1").LabelAngle = -90
            'ChartLine.Series("Series1").Color = GetColor(i)
            ChartLine.Series("Series1").Points([i]).Color = GetColor(i)
        Next

        Dim MonthlyTrendList_ChartCount As Integer = MonthlyTrendList.Count * 20
        If MonthlyTrendList_ChartCount > 550 Then
            ChartLine.Width = MonthlyTrendList_ChartCount
        End If
        ChartLine.DataSource = ds.Tables("MonthlyTrendList")
        ChartLine.DataBind()
        upnlLine.Update()
    End Sub
    Public Sub SetPieBarGraph(obj As ReportFlyingHrs, Optional IsForBar As Boolean = False)
        SetValues()
        ChartBarPie.Visible = True
        Dim ChartArea1 As New ChartArea
        Dim Legend1 As New Legend
        Dim Title1 As New Title
        Dim Series1 As New Series


        ' 
        If IsForBar = True Then
            ChartBarPie.Series("Series1").ChartType = SeriesChartType.Column
            ChartBarPie.Legends(0).Enabled = False
            ChartBarPie.Series("Series1").IsValueShownAsLabel = True
        Else
            ChartBarPie.Series("Series1").ChartType = SeriesChartType.Pie
            ChartBarPie.Legends(0).Enabled = True
        End If

        For i As Integer = 0 To obj.Count - 1
            ChartBarPie.Series("Series1").Points.AddXY(obj(i).RegNo, IIf(obj(i).FlyingHrs <> 0, CDec(Format(obj(i).FlyingHrs, "###0.00")), 0))
            ChartBarPie.Series("Series1").LabelAngle = -90
            ' ChartBarPie.Series("Series1").Color = GetColor(i)
            ' ChartBarPie.Series("Series1")("PieLabelStyle") = "Disabled"
            ChartBarPie.Series("Series1").Points([i]).Color = GetColor(i)
        Next

        Dim obj_ChartCount As Integer = obj.Count * 20
        If obj_ChartCount > 550 Then
            ChartBarPie.Width = obj_ChartCount
        End If

        If obj.Count > 0 Then
            ChartBarPie.Visible = True
            ChartBarPie.DataSource = ds.Tables("obj")
            ChartBarPie.DataBind()
            upnlBarPie.Update()

        Else
            ChartBarPie.Visible = False
        End If

    End Sub

    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblYear1.Visible = True
    End Sub
    Private Sub SetValues()
        SelectYear = IIf(cmbYear.SelectedIndex > -1, cmbYear.SelectedItem.Text, "")
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        lblYear1.Text = "Year : " & IIf(SelectYear <> "", SelectYear, "")
        If rdbPieGraph.Checked Then
            lblAircraft1.Text = "Aircraft : "
        ElseIf rdbBarGraph.Checked Then
            lblAircraft1.Text = "Aircraft : "
        ElseIf rdbLineGraph.Checked Then
            lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        End If

        mCompleteSearchingCriteria = lblYear1.Text + ", " + lblAircraft1.Text

    End Sub

    Private Sub SetReport()
        obj = New ReportFlyingHrs
        SetValues()

        ReportPieGraph = New crPieFlyingHrs
        ReportBarGraph = New crBarFlyingHrs
        ReportLineGraph = New crLineFlyingHrs

        If (rdbPieGraph.Checked) Or (rdbBarGraph.Checked) Then  'Pie, Bar

            obj = ReportFlyingHrs.GetGraFlyingHrs(SelectYear)

            SetPieBarGraph(obj, rdbBarGraph.Checked)

            If obj.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfSearchCriteriaForFlyingHrs.aspx?Backpage="
                'msg1.Show()

                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 908)
            End If

        ElseIf (rdbLineGraph.Checked) Then  'Line

            MonthlyTrendList = ReportMonthlyTrendList.GetReportMonthlyTrendList(CInt(SelectYear), cmbAircraft.SelectedValue.ToString)
            SetLineGraph(MonthlyTrendList)
            If MonthlyTrendList.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfSearchCriteriaForFlyingHrs.aspx?Backpage="
                'msg1.Show()

                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 908)
            End If

        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, "Graphical Representation of Flying Hours", "Detail for" + " " + cmbAircraft.SelectedItem.Text, cmbYear.SelectedItem.Text, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))  'Changed By Utkarsh For Report Logo.

        ds.Clear()
        dsLine.Clear()

        If rdbPieGraph.Checked Then
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, obj)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo

            ReportPieGraph.SetDataSource(ds)
            Session("CrystalReport") = ReportPieGraph

        ElseIf rdbBarGraph.Checked Then
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, obj)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo

            ReportBarGraph.SetDataSource(ds)
            Session("CrystalReport") = ReportBarGraph

        ElseIf rdbLineGraph.Checked Then
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(dsLine)
            '----------------------------------------------------------
            da.Fill(dsLine, Report)
            da.Fill(dsLine, MonthlyTrendList)
            da.Fill(dsLine, mrptImage) 'Added by Utkarsh for Report Logo

            ReportLineGraph.SetDataSource(dsLine)
            Session("CrystalReport") = ReportLineGraph
        End If

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "PieChartAnalysis", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub

#End Region

#Region " Data Binding "
    ''Added by Archana on 5-Aug-09
    'Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "cmbAircraft" Then
    '        If cmbAircraft.SelectedIndex <= 0 And cmbAircraft.Enabled = True Then
    '            custValidator.ErrorMessage = "Please select the Aircraft"
    '            e.IsValid = False
    '        Else
    '            e.IsValid = True
    '        End If
    '    End If
    'End Sub
    Private Sub DataFieldBind()
        mMachineList = MachineList.GetMachineListMonitoringStatus(Now.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(Select)")
        Session("mMachineList") = mMachineList

        cmbAircraft.DataSource = mMachineList
        cmbAircraft.DataBind()

        SetValues()
        If rdbPieGraph.Checked Then
            obj = ReportFlyingHrs.GetGraFlyingHrs(SelectYear)
            SetPieBarGraph(obj, False)
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid)

        Dim i As Integer
        Dim prevyear As Integer
        Dim nextyear As Integer

        Year = Now.Year
        prevyear = Year - 10
        nextyear = Year + 10

        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForFlyingHrs_Ajax.aspx?"

            For i = prevyear To nextyear
                cmbYear.Items.Add(i)
            Next

            lblAircraft.Enabled = False
            cmbAircraft.Enabled = False

            If cmbYear.Enabled = True Then
                SetFocus(cmbYear)
            End If

            'Added by Archana on 5-Aug-09
            cmbYear.SelectedValue = Now.Year
            DataFieldBind()
        End If

        Dim scriptManager As ScriptManager = scriptManager.GetCurrent(Me.Page)
        'scriptManager.RegisterPostBackControl(Me.lnkExportToPDF)
        scriptManager.RegisterPostBackControl(Me.btnDisplay)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        ''If IsValid Then
        ''    SetReport()
        ''End If

        'lnkExportToPDF_Click(sender, e)
        Try
            Dim mCompanyDetail As New CompanyDetail
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            SetValues()
            If rdbPieGraph.Checked Or rdbBarGraph.Checked Then
                obj = ReportFlyingHrs.GetGraFlyingHrs(SelectYear)

                If obj.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 908)
                End If
            End If


            If rdbPieGraph.Checked Then
                SetPieBarGraph(obj, False)
            ElseIf rdbBarGraph.Checked Then
                SetPieBarGraph(obj, True)
            ElseIf rdbLineGraph.Checked Then
                MonthlyTrendList = ReportMonthlyTrendList.GetReportMonthlyTrendList(CInt(SelectYear), cmbAircraft.SelectedValue.ToString)
                If MonthlyTrendList.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 908)
                End If

                SetLineGraph(MonthlyTrendList)
            End If

            Dim pdfDoc As iTextSharp.text.Document = New iTextSharp.text.Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
            Dim mPDFWriter As PdfWriter
            mPDFWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
            pdfDoc.Open()
            Dim stream As MemoryStream = New MemoryStream
            Dim mrptImage As rptImage

            ' ChartBarPie.SaveImage(stream, ChartImageFormat.Png)
            If rdbPieGraph.Checked Or rdbBarGraph.Checked Then
                ChartBarPie.SaveImage(stream, ChartImageFormat.Png)
                mrptImage = rptImage.GetImage(ds)
                'If Not mrptImage Is Nothing Then mrptImage(0).ImageFile
            Else
                ChartLine.SaveImage(stream, ChartImageFormat.Png)
                mrptImage = rptImage.GetImage(dsLine)
            End If


            '''Header
            Dim DataTable As PdfPTable = New PdfPTable(4)

            Dim Header_1 As New PdfPCell '= New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
            Dim Header_2 As PdfPCell = New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
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
            DataTable.WidthPercentage = 95
            pdfDoc.Add(DataTable)




            '''**********************************

            '''Criteria
            Dim DataTable1 As PdfPTable = New PdfPTable(2)
            Dim Header_3 As PdfPCell = New PdfPCell(New Phrase(lblYear1.Text, FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            'Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY
            Header_3.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_3.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_3.Colspan = 2
            DataTable1.WidthPercentage = 95
            DataTable1.AddCell(Header_3)

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

            Dim Product As PdfPCell = New PdfPCell(New Phrase(AppSettings("Product Version"), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            Dim SINote As PdfPCell = New PdfPCell(New Phrase(AppSettings("SINote"), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))

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

            '  pdfDoc.Add(table)
            '************************************

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=Chart.pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)


            pdfDoc.Close()
            Response.Write(pdfDoc)
            ' Response.End()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub rdbPieGraph_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbPieGraph.CheckedChanged
        If rdbPieGraph.Checked Then
            lblAircraft.Enabled = False
            cmbAircraft.Enabled = False
            cmbAircraft.SelectedIndex = 0
            SetValues()
            obj = ReportFlyingHrs.GetGraFlyingHrs(SelectYear)
            SetPieBarGraph(obj, False)
        End If
        'Added by Archana on 5-Aug-09
        ' DataFieldBind()
    End Sub
    Private Sub rdbBarGraph_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbBarGraph.CheckedChanged
        If rdbBarGraph.Checked Then
            lblAircraft.Enabled = False
            cmbAircraft.Enabled = False
            cmbAircraft.SelectedIndex = 0
            SetValues()
            obj = ReportFlyingHrs.GetGraFlyingHrs(SelectYear)
            SetPieBarGraph(obj, True)
        End If
        'Added by Archana on 5-Aug-09
        ' DataFieldBind()
    End Sub
    Private Sub rdbLineGraph_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbLineGraph.CheckedChanged
        If rdbLineGraph.Checked Then
            lblAircraft.Enabled = True
            cmbAircraft.Enabled = True
            SetFocus(cmbAircraft)

        End If

    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged

        If cmbAircraft.SelectedIndex > 0 Then
            SetValues()
            MonthlyTrendList = ReportMonthlyTrendList.GetReportMonthlyTrendList(CInt(SelectYear), cmbAircraft.SelectedValue.ToString)
            SetLineGraph(MonthlyTrendList)
        End If

    End Sub
    Private Sub cmbYear_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbYear.SelectedIndexChanged
        SetValues()
        If rdbPieGraph.Checked Or rdbBarGraph.Checked Then obj = ReportFlyingHrs.GetGraFlyingHrs(SelectYear)

        If rdbPieGraph.Checked Then
            SetPieBarGraph(obj, False)
        ElseIf rdbBarGraph.Checked Then
            SetPieBarGraph(obj, True)
        ElseIf rdbLineGraph.Checked Then
            MonthlyTrendList = ReportMonthlyTrendList.GetReportMonthlyTrendList(CInt(SelectYear), cmbAircraft.SelectedValue.ToString)
            SetLineGraph(MonthlyTrendList)
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    ' Protected Sub lnkExportToPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkExportToPDF.Click
    'Try
    '    Dim mCompanyDetail As New CompanyDetail
    '    mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
    '    SetValues()
    '    If rdbPieGraph.Checked Or rdbBarGraph.Checked Then obj = ReportFlyingHrs.GetGraFlyingHrs(SelectYear)

    '    If rdbPieGraph.Checked Then
    '        SetPieBarGraph(obj, False)
    '    ElseIf rdbBarGraph.Checked Then
    '        SetPieBarGraph(obj, True)
    '    ElseIf rdbLineGraph.Checked Then
    '        MonthlyTrendList = ReportMonthlyTrendList.GetReportMonthlyTrendList(CInt(SelectYear), cmbAircraft.SelectedValue.ToString)
    '        SetLineGraph(MonthlyTrendList)
    '    End If

    '    Dim pdfDoc As iTextSharp.text.Document = New iTextSharp.text.Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
    '    Dim mPDFWriter As PdfWriter
    '    mPDFWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
    '    pdfDoc.Open()
    '    Dim stream As MemoryStream = New MemoryStream
    '    Dim mrptImage As rptImage

    '    ' ChartBarPie.SaveImage(stream, ChartImageFormat.Png)
    '    If rdbPieGraph.Checked Or rdbBarGraph.Checked Then
    '        ChartBarPie.SaveImage(stream, ChartImageFormat.Png)
    '        mrptImage = rptImage.GetImage(ds)
    '        'If Not mrptImage Is Nothing Then mrptImage(0).ImageFile
    '    Else
    '        ChartLine.SaveImage(stream, ChartImageFormat.Png)
    '        mrptImage = rptImage.GetImage(dsLine)
    '    End If


    '    '''Header
    '    Dim DataTable As PdfPTable = New PdfPTable(4)

    '    Dim Header_1 As New PdfPCell '= New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
    '    Dim Header_2 As PdfPCell = New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
    '    If Not mrptImage Is Nothing Then
    '        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(mrptImage(0).ImageFile)
    '        image.ScaleToFit(60, 60)
    '        image.Alignment = 0
    '        Header_1.AddElement(image)
    '        Header_1.Border = iTextSharp.text.Rectangle.NO_BORDER
    '        Header_1.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
    '        Header_1.Colspan = 1
    '        DataTable.AddCell(Header_1)
    '    End If

    '    'Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY


    '    Header_2.Border = iTextSharp.text.Rectangle.NO_BORDER
    '    Header_2.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER
    '    Header_2.Colspan = 3


    '    DataTable.AddCell(Header_2)
    '    DataTable.WidthPercentage = 95
    '    pdfDoc.Add(DataTable)




    '    '''**********************************

    '    '''Criteria
    '    Dim DataTable1 As PdfPTable = New PdfPTable(2)
    '    Dim Header_3 As PdfPCell = New PdfPCell(New Phrase(lblYear1.Text, FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
    '    'Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY
    '    Header_3.Border = iTextSharp.text.Rectangle.NO_BORDER
    '    Header_3.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
    '    Header_3.Colspan = 2
    '    DataTable1.WidthPercentage = 95
    '    DataTable1.AddCell(Header_3)

    '    pdfDoc.Add(DataTable1)
    '    '''**********************************


    '    '''Chart
    '    Dim chartImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(stream.GetBuffer)
    '    chartImage.ScalePercent(75.0!)
    '    chartImage.Alignment = Element.ALIGN_MIDDLE

    '    Dim p1 As Paragraph = New Paragraph()
    '    p1.Alignment = Element.ALIGN_CENTER

    '    pdfDoc.Add(p1)


    '    chartImage.SetAbsolutePosition(0, pdfDoc.PageSize.Height / 2)
    '    pdfDoc.Add(chartImage)
    '    '************************************


    '    '''Footer
    '    Dim table As New PdfPTable(2)
    '    table.WidthPercentage = 95

    '    Dim Product As PdfPCell = New PdfPCell(New Phrase(AppSettings("Product Version"), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
    '    Dim SINote As PdfPCell = New PdfPCell(New Phrase(AppSettings("SINote"), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))

    '    Product.Border = iTextSharp.text.Rectangle.NO_BORDER
    '    Product.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
    '    Product.Colspan = 1
    '    SINote.Border = iTextSharp.text.Rectangle.NO_BORDER
    '    SINote.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_RIGHT

    '    SINote.Colspan = 1
    '    table.AddCell(Product)
    '    table.AddCell(SINote)

    '    'table.SetWidthPercentage(95.0)

    '    table.TotalWidth = 580.0F

    '    table.WriteSelectedRows(0, -1, 0, 50, mPDFWriter.DirectContent)

    '    '  pdfDoc.Add(table)
    '    '************************************

    '    Response.ContentType = "application/pdf"
    '    Response.AddHeader("content-disposition", "attachment;filename=Chart.pdf")
    '    Response.Cache.SetCacheability(HttpCacheability.NoCache)


    '    pdfDoc.Close()
    '    Response.Write(pdfDoc)
    '    ' Response.End()
    'Catch ex As Exception
    '    Throw ex
    'End Try
    ' End Sub

#End Region



End Class