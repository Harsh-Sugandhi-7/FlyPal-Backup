Imports System.Configuration.ConfigurationManager

Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing

Public Class wfrptSnagMonthWiseGraph_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList ' tmpMachineList

    Dim ToDate As String
    Dim MachineID As String
    Dim Aircraft As String
    Dim RegNo As String
    Public mrptSnagMonthWiseGraph As rptSnagMonthWiseGraph
    Dim IsMajor As Boolean
    Dim IsInvestigationStatus As Boolean
    Dim IsMajorMinor As Integer
    Dim MajorMinor As String
    Dim IsSnagMEL As Integer
    Dim SnagMEL As String
    Dim string1 As String
    Dim string2 As String
    Dim string3 As String
    Dim string4 As String
    Dim string5 As String

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mrptSnagMonthWiseGraph = CType(Session("mrptSnagMonthWiseGraph"), rptSnagMonthWiseGraph)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub SetSession()
        Session("mrptSnagMonthWiseGraph") = mrptSnagMonthWiseGraph
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptSnagMonthWiseGraph")
        Session.Remove("mMachineNameValueList")
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub Display()
        lblAircraft1.Visible = True
        lblYear1.Visible = True
    End Sub
    Private Sub SetValues()
        ToDate = cmbYear.SelectedItem.Text
        If rbMajor.Checked = True Then
            IsMajorMinor = 1  'Major
            MajorMinor = 1    'To Show on report Major/Minor/All
        ElseIf rbAll.Checked = True Then
            IsMajorMinor = 2  '"All" means Bot Major and Minor
            MajorMinor = 2
        Else
            IsMajorMinor = 0  'Minor
            MajorMinor = 0
        End If
        If rbAllSnagMEL.Checked = True Then
            IsSnagMEL = 0  'ALL Snag AND MEL
            SnagMEL = 0
        ElseIf rbSnag.Checked = True Then
            IsSnagMEL = 1  'Snag
            SnagMEL = 1
        Else
            IsSnagMEL = 2  'MEL
            SnagMEL = 2
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "All")
        MachineID = cmbAircraft.SelectedValue.ToString
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblYear1.Text = "Year : " & IIf(ToDate <> "", ToDate, "")
        string1 = IIf(ToDate <> "", ToDate, "")
        string2 = IIf(Aircraft <> "", Aircraft, "")

        mCompleteSearchingCriteria = lblYear1.Text + ", " + lblAircraft1.Text + ", " + "Type :" + IIf(rbAll.Checked, "All", IIf(rbMajor.Checked, "Major", "Minor")) + ", " + "Part :" + IIf(rbAllSnagMEL.Checked, "All", IIf(rbSnag.Checked, IIf(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag"), IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL")))
    End Sub

    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptSnagMonthWiseGraph_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub SetReport(Optional IsPrint As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsSnagMonthWiseGraph
        Dim mCompanyDetail As New CompanyDetail
        Dim OperatorName As String = ""

        SetValues()
        myReport = New crSnagMonthWiseGraph

        mrptSnagMonthWiseGraph = rptSnagMonthWiseGraph.GetSnagMonthWiseGraphReport(CInt(ToDate), MachineID, IsMajorMinor, IsSnagMEL)

        'Added by Saylee on 11-Aug-2011
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            If cmbAircraft.SelectedIndex > 0 Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(MachineID))
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            End If
        End If

        Dim mReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, _
         mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
         SnagMEL, string1, Aircraft, MajorMinor, string4, string5, AppSettings("Product Version"), AppSettings("SINote"), OperatorName, "", "", AppSettings("MELSnagNomenclature").ToString, AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        'If CType(CType(mrptSnagMonthWiseGraph.CurrentItem, Object), Flypal.rptSnagMonthWiseGraph.SnagMonthWiseGraph).SnagCount = 0 Then
        If mrptSnagMonthWiseGraph.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfrptSnagMonthWiseGraph.aspx?Backpage="
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1013)
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, mrptSnagMonthWiseGraph)
        da.Fill(ds, mReportData)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        'If IsPrint Then
        '    Dim Str As String
        '    Str = "openTranDetail();"
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        'Else
        '    Chart1.Visible = True
        '    Dim ChartArea1 As New ChartArea
        '    Dim Legend1 As New Legend
        '    Dim Title1 As New Title
        '    Dim Series1 As New Series

        '    Dim xValues As String() = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        '    For i As Integer = 0 To mrptSnagMonthWiseGraph.Count - 1
        '        Chart1.Series("Series1").Points.AddXY(xValues(i), mrptSnagMonthWiseGraph(i).SnagCount)
        '        Chart1.Series("Series1").IsValueShownAsLabel = True
        '        Chart1.Series("Series1").LegendText = String.Empty
        '        Chart1.Series("Series1").LabelAngle = -90
        '        'Chart1.Series("Series1").Color = GetColor(i)
        '        Chart1.Series("Series1").Points([i]).Color = GetColor(i)
        '    Next

        '    Dim SnagMonthWise_ChartCount As Integer = mrptSnagMonthWiseGraph.Count * 20
        '    If SnagMonthWise_ChartCount > 550 Then
        '        Chart1.Width = SnagMonthWise_ChartCount
        '    End If
        '    Chart1.DataSource = ds.Tables("mrptSnagMonthWiseGraph")
        '    Chart1.DataBind()
        '    upnlChart.Update()

        'End If
        ' ShowChart(ds)
        Chart1.Visible = True
        Dim ChartArea1 As New ChartArea
        Dim Legend1 As New Legend
        Dim Title1 As New Title
        Dim Series1 As New Series

        Dim xValues As String() = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        For i As Integer = 0 To mrptSnagMonthWiseGraph.Count - 1
            Chart1.Series("Series1").Points.AddXY(xValues(i), mrptSnagMonthWiseGraph(i).SnagCount)
            Chart1.Series("Series1").IsValueShownAsLabel = True
            Chart1.Series("Series1").LegendText = String.Empty
            Chart1.Series("Series1").LabelAngle = -90
            'Chart1.Series("Series1").Color = GetColor(i)
            Chart1.Series("Series1").Points([i]).Color = GetColor(i)

        Next
        Chart1.ChartAreas(0).AxisY.Title = IIf(AppSettings("MELSnagNomenclature"), "Defect Count", "Snag Count")
        Dim SnagMonthWise_ChartCount As Integer = mrptSnagMonthWiseGraph.Count * 20
        If SnagMonthWise_ChartCount > 550 Then
            Chart1.Width = SnagMonthWise_ChartCount
        End If
        Chart1.DataSource = ds.Tables("mrptSnagMonthWiseGraph")
        Chart1.DataBind()
        upnlChart.Update()

        If IsPrint Then
            Dim pdfDoc As iTextSharp.text.Document = New iTextSharp.text.Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
            Dim mPDFWriter As PdfWriter
            mPDFWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
            pdfDoc.Open()

            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim stream As MemoryStream = New MemoryStream

            ' ChartBarPie.SaveImage(stream, ChartImageFormat.Png)

            Chart1.SaveImage(stream, ChartImageFormat.Png)

            '''Header
            Dim DataTable As PdfPTable = New PdfPTable(4)

            Dim Header_1 As New PdfPCell '= New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
            Dim Header_2 As PdfPCell = New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Report For No. of Defect per Month", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
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
            Header_3.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_3.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_3.Colspan = 1

            Dim Header_4 As PdfPCell = New PdfPCell(New Phrase(lblAircraft1.Text, FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            Header_4.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_4.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_4.Colspan = 1

            DataTable1.WidthPercentage = 95
            DataTable1.AddCell(Header_3)
            DataTable1.AddCell(Header_4)
            pdfDoc.Add(DataTable1)

            Dim DataTable2 As PdfPTable = New PdfPTable(2)

            Dim Header_5 As PdfPCell = New PdfPCell(New Phrase("Type :" + IIf(rbAll.Checked, "All", IIf(rbMajor.Checked, "Major", "Minor")), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            Header_5.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_5.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_5.Colspan = 1

            Dim Header_6 As PdfPCell = New PdfPCell(New Phrase("Part :" + IIf(rbAllSnagMEL.Checked, "All", IIf(rbSnag.Checked, IIf(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag"), IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL"))), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            Header_6.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_6.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_6.Colspan = 1

            DataTable2.WidthPercentage = 95
            DataTable2.AddCell(Header_5)
            DataTable2.AddCell(Header_6)
            pdfDoc.Add(DataTable2)


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
        End If

        MarkLog(Util.Action.Print, "SnagMonthWise", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '1013
    End Sub
    Private Sub ShowChart(ds As dsSnagMonthWiseGraph)
        Chart1.Visible = True
        Dim ChartArea1 As New ChartArea
        Dim Legend1 As New Legend
        Dim Title1 As New Title
        Dim Series1 As New Series

        Dim xValues As String() = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        For i As Integer = 0 To mrptSnagMonthWiseGraph.Count - 1
            Chart1.Series("Series1").Points.AddXY(xValues(i), mrptSnagMonthWiseGraph(i).SnagCount)
            Chart1.Series("Series1").IsValueShownAsLabel = True
            Chart1.Series("Series1").LegendText = String.Empty
            Chart1.Series("Series1").LabelAngle = -90
            'Chart1.Series("Series1").Color = GetColor(i)
            Chart1.Series("Series1").Points([i]).Color = GetColor(i)
        Next
        Chart1.ChartAreas(0).AxisY.Title = IIf(AppSettings("MELSnagNomenclature"), "Defect Count", "Snag Count")
        Dim SnagMonthWise_ChartCount As Integer = mrptSnagMonthWiseGraph.Count * 20
        If SnagMonthWise_ChartCount > 550 Then
            Chart1.Width = SnagMonthWise_ChartCount
        End If
        Chart1.DataSource = ds.Tables("mrptSnagMonthWiseGraph")
        Chart1.DataBind()
        upnlChart.Update()
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
#End Region

#Region " Data Binding "
    Private Sub SetCombo()
        Dim i As Integer
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If
    End Sub
    Private Sub DataFieldBind()
        'mMachineNameValueList =  tmpMachineList.GetMachineList("", "", "", "", "", "(All)")

        mMachineNameValueList = MachineNameValueList.GetMachineList("", (Guid.Empty).ToString, 0, 0, "", "", "", True, "(All)", , True)

        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        'cmbAircraft.DataBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            If Session("MiddleFrame") <> "wfrptSnagMonthWiseGraph_Ajax.aspx" Then Session("MiddleFrame") = "wfrptSnagMonthWiseGraph_Ajax.aspx"
            rbAll.Checked = True
            rbAllSnagMEL.Checked = True
            SetCombo()
            DataFieldBind()
            'Added by Archana on 6-Aug-09
            If cmbYear.Enabled = True Then
                setFocus(cmbYear)
            End If
            SetReport()
        End If
        Dim scriptManager As ScriptManager = scriptManager.GetCurrent(Me.Page)
        scriptManager.RegisterPostBackControl(Me.btnPrint)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlDisplaySearchCriteria.Update()

    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay1.Click
        SetReport()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        SetReport(True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        'Response.End()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
    '    If cmbAircraft.Enabled = True Then
    '        setFocus(cmbAircraft)
    '    End If
    'End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Protected Sub cmbYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbYear.SelectedIndexChanged, cmbAircraft.SelectedIndexChanged, rbAll.CheckedChanged, rbAllSnagMEL.CheckedChanged, rbMajor.CheckedChanged, rbMEL.CheckedChanged, rbMinor.CheckedChanged, rbSnag.CheckedChanged
        SetReport()
        upnlChart.Update()
    End Sub
#End Region

    
End Class