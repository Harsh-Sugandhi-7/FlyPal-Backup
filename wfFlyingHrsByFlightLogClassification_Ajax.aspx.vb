Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing
Imports System.Web.Script.Serialization

Public Class wfFlyingHrsByFlightLogClassification_Ajax
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
    Dim obj As FlyingHrsByFlightLogClassification
    Dim mMachineNameValueList As MachineNameValueList
    Public mFlightLogClassificationList As FlightLogClassificationList
    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineListFlyingHrsByFlightLogClassification"), MachineNameValueList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfFlyingHrsByFlightLogClassification_Ajax.aspx?" Then
            Session.Remove("mMachineListFlyingHrsByFlightLogClassification")
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
    Public Sub SetPieBarGraph(obj As FlyingHrsByFlightLogClassification, Optional IsForBar As Boolean = False)
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
            ChartBarPie.Series("Series1").Points.AddXY(obj(i).Name, IIf(obj(i).FlyingHrs <> 0, CDec(Format(obj(i).FlyingHrs, "###0.00")), 0))
            ChartBarPie.Series("Series1").LabelAngle = -90
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
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblFlightLogClassification.Text = "Classification : " & IIf(cmbFlightLogClassification.SelectedIndex > 0, cmbFlightLogClassification.SelectedItem.Text, "")
        mCompleteSearchingCriteria = lblYear1.Text + ", " + lblAircraft1.Text + ", " + lblFlightLogClassification.Text
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Now.ToShortDateString, IsTagRequired:=True, TagText:="(ALL)")
        Session("mMachineListFlyingHrsByFlightLogClassification") = mMachineNameValueList

        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()

        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(ALL)")
        cmbFlightLogClassification.DataSource = mFlightLogClassificationList
        cmbFlightLogClassification.DataBind()
        Session("mFlightLogClassificationList") = mFlightLogClassificationList

        SetValues()
        obj = FlyingHrsByFlightLogClassification.GetFlyingHrsByFlightLogClassification(Year:=SelectYear, MachineID:=cmbAircraft.SelectedValue.ToString, FlightLogClassificationID:=cmbFlightLogClassification.SelectedValue.ToString)
        SetPieBarGraph(obj, False)
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
            Session("MiddleFrame") = "wfFlyingHrsByFlightLogClassification_Ajax.aspx?"

            For i = prevyear To nextyear
                cmbYear.Items.Add(i)
            Next

            If cmbYear.Enabled = True Then
                SetFocus(cmbYear)
            End If

            cmbYear.SelectedValue = Now.Year
            DataFieldBind()
        End If
        Dim scriptManager As ScriptManager = scriptManager.GetCurrent(Me.Page)
        scriptManager.RegisterPostBackControl(Me.btnDisplay)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        obj = FlyingHrsByFlightLogClassification.GetFlyingHrsByFlightLogClassification(Year:=SelectYear, MachineID:=cmbAircraft.SelectedValue.ToString, FlightLogClassificationID:=cmbFlightLogClassification.SelectedValue.ToString)
        SetPieBarGraph(obj, False)
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
            Dim mCompanyDetail As New CompanyDetail
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            SetValues()
            obj = FlyingHrsByFlightLogClassification.GetFlyingHrsByFlightLogClassification(Year:=SelectYear, MachineID:=cmbAircraft.SelectedValue.ToString, FlightLogClassificationID:=cmbFlightLogClassification.SelectedValue.ToString)
            If obj.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1341)
            End If
            SetPieBarGraph(obj, False)

            Dim pdfDoc As iTextSharp.text.Document = New iTextSharp.text.Document(PageSize.A4, 10.0!, 10.0!, 10.0!, 0.0!)
            Dim mPDFWriter As PdfWriter
            mPDFWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
            pdfDoc.Open()
            Dim stream As MemoryStream = New MemoryStream
            Dim mrptImage As rptImage

            ChartBarPie.SaveImage(stream, ChartImageFormat.Png)
            mrptImage = rptImage.GetImage(ds)

            '''Header
            Dim DataTable As PdfPTable = New PdfPTable(4)

            Dim Header_1 As New PdfPCell
            Dim Header_2 As PdfPCell = New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours Per Classification", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
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

            Dim Header_4 As PdfPCell = New PdfPCell(New Phrase(lblAircraft1.Text, FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            'Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY
            Header_4.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_4.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_4.Colspan = 2
            DataTable1.WidthPercentage = 95
            DataTable1.AddCell(Header_4)

            Dim Header_5 As PdfPCell = New PdfPCell(New Phrase(lblFlightLogClassification.Text, FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            'Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY
            Header_5.Border = iTextSharp.text.Rectangle.NO_BORDER
            Header_5.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            Header_5.Colspan = 2
            DataTable1.WidthPercentage = 95
            DataTable1.AddCell(Header_5)

            pdfDoc.Add(DataTable1)

            'Dim DataTable4 As PdfPTable = New PdfPTable(4)
            'Dim Header_4 As PdfPCell = New PdfPCell(New Phrase(lblAircraft1.Text, FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            ''Header_1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY
            'Header_4.Border = iTextSharp.text.Rectangle.NO_BORDER
            'Header_4.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            'Header_4.Colspan = 2
            'DataTable4.WidthPercentage = 95
            'DataTable4.AddCell(Header_4)

            'pdfDoc.Add(DataTable4)
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
            table.TotalWidth = 580.0F
            table.WriteSelectedRows(0, -1, 0, 50, mPDFWriter.DirectContent)

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=Chart.pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)

            pdfDoc.Close()
            Response.Write(pdfDoc)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        'If cmbAircraft.SelectedIndex > 0 Then
        SetValues()
        obj = FlyingHrsByFlightLogClassification.GetFlyingHrsByFlightLogClassification(Year:=SelectYear, MachineID:=cmbAircraft.SelectedValue.ToString, FlightLogClassificationID:=cmbFlightLogClassification.SelectedValue.ToString)
        SetPieBarGraph(obj, False)
        'End If
    End Sub
    Private Sub cmbYear_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbYear.SelectedIndexChanged
        SetValues()
        obj = FlyingHrsByFlightLogClassification.GetFlyingHrsByFlightLogClassification(Year:=SelectYear, MachineID:=cmbAircraft.SelectedValue.ToString, FlightLogClassificationID:=cmbFlightLogClassification.SelectedValue.ToString)
        SetPieBarGraph(obj, False)
    End Sub
    Private Sub cmbFlightLogClassification_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbFlightLogClassification.SelectedIndexChanged
        'If cmbFlightLogClassification.SelectedIndex > 0 Then
        SetValues()
        obj = FlyingHrsByFlightLogClassification.GetFlyingHrsByFlightLogClassification(Year:=SelectYear, MachineID:=cmbAircraft.SelectedValue.ToString, FlightLogClassificationID:=cmbFlightLogClassification.SelectedValue.ToString)
        SetPieBarGraph(obj, False)
        'End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class