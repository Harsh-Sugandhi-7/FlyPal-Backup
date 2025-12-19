'Created By: Saylee
'Dated:     3-Jul-2019

Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net

Imports System.Data
'Imports OWC10
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports System.Web.UI.DataVisualization.Charting
Imports Microsoft.Office.Interop.Owc11
Imports CrystalDecisions.CrystalReports.Engine

Imports System
Imports System.Configuration

Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging

Imports System.Web.UI.WebControls
Imports DayPilot.Utils
Imports DayPilot.Web.Ui
Imports DayPilot.Web.Ui.Data
Imports DayPilot.Web.Ui.Enums
Imports DayPilot.Web.Ui.Events
Imports DayPilot.Web.Ui.Events.Scheduler
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports CommandEventArgs = DayPilot.Web.Ui.Events.CommandEventArgs
Imports CornerShape = DayPilot.Web.Ui.Enums.Scheduler.CornerShape


Public Class wfAuditCalendarPlanList_AJAX
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Shared Count As Integer = 0
    'Shared PlannedList As String = ""
    'Public mWOStatusList As nWOStatusList

    'Shared tmpMonth As Integer = 0
    'Shared tmpYear As Integer = 0

    Public mAuditListForCalandarYear As AuditListForCalandarYear
    Public mAuditCalendar As AuditCalandar
#End Region

#Region " Methods "
    Private Sub GetSession()
        Count = Session("Count")
        mAuditListForCalandarYear = Session("mAuditListForCalandarYear")
        mAuditCalendar = Session("mAuditCalendar")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfAuditCalendarPlanList_AJAX.aspx?") <= 0 Then
            'PlannedList = ""
            Session.Remove("mAuditListForCalandarYear")
            Session.Remove("mAuditCalendar")
        End If


    End Sub
    Private Sub DatafieldBind()
        mAuditListForCalandarYear = AuditListForCalandarYear.GetAuditListForCalandarYear()
        Session("mAuditListForCalandarYear") = mAuditListForCalandarYear
        If AppSettings("ClientCode") = "GEP" Then
            lblTitle.Text = " GEPL CAMO QUALITY SYSTEM - AUDIT PLAN"
            lblCompany.Visible = False
        Else
            Dim mCompanyDetail As New CompanyDetail
            Dim Company As String
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Company = mCompanyDetail.CompanyName
            lblCompany.Text = Company.ToString
            lblCompany.Visible = True
        End If
        UpdatePanel1.Update()
    End Sub
    Private Sub LoadResource(Optional ByVal From As Integer = 0)
        DayPilotScheduler1.Resources.Clear()

        For Each info As AuditListForCalandarYear.AuditListForCalandarYearInfo In mAuditListForCalandarYear

            Dim id As String = info.AuditID.ToString
            Dim code As String = info.AuditOnUI

            DayPilotScheduler1.Resources.Add(code, id)

        Next

        DayPilotScheduler1.StartDate = New DateTime(DateTime.Today.Year, 1, 1)
        DayPilotScheduler1.Days = 365
        If From = 0 Then
            cmbYear.SelectedValue = Right(DateTime.Today.Year.ToString, 1)
        End If
        GetSpotTypeList()
    End Sub
    Private Sub GetSpotTypeList()
        Dim StartDateM As New SmartDate
        Dim EndDateM As New SmartDate
        Dim year As String = cmbYear.SelectedItem.ToString
        StartDateM = New SmartDate(DateAdd(DateInterval.Month, 0, DateSerial(Val(year), 1, 1)), False)
        EndDateM = New SmartDate(CStr(DateSerial(StartDateM.Date.Year, StartDateM.Date.Month + 11, DateTime.DaysInMonth(StartDateM.Date.Year, StartDateM.Date.Month))), False)


        mAuditCalendar = AuditCalandar.GetAuditCalandarList(StartDateM.ToString, EndDateM.ToString)


        DayPilotScheduler1.DataSource = mAuditCalendar
        DayPilotScheduler1.DataBind()
        DayPilotScheduler1.Update()

        SetExportProperties()
        upnlSchedulerList.Update()

        Session("mAuditCalendar") = mAuditCalendar
    End Sub
    Private Sub SetExportProperties()
        'DayPilotScheduler1.Width = Unit.Percentage(100);
        'DayPilotScheduler1.Width = System.Web.UI..Pixel(800)
        DayPilotScheduler1.DurationBarColor = ColorTranslator.FromHtml("#ccc")

        ' match the theme
        DayPilotScheduler1.HourNameBackColor = ColorTranslator.FromHtml("#eee")
        DayPilotScheduler1.BackColor = Color.White
        DayPilotScheduler1.NonBusinessBackColor = Color.White
        DayPilotScheduler1.BorderColor = ColorTranslator.FromHtml("#999")
        DayPilotScheduler1.HeaderFontColor = ColorTranslator.FromHtml("#100F0F")  'ColorTranslator.FromHtml("#666")
        DayPilotScheduler1.CellBorderColor = ColorTranslator.FromHtml("#eee")
        DayPilotScheduler1.EventFontColor = ColorTranslator.FromHtml("#100F0F")  'ColorTranslator.FromHtml("#666")
        DayPilotScheduler1.EventFontSize = "10pt"
        DayPilotScheduler1.EventBorderColor = ColorTranslator.FromHtml("#999")
        DayPilotScheduler1.EventBackColor = ColorTranslator.FromHtml("#fafafa")

        DayPilotScheduler1.EventHeight = 35
        DayPilotScheduler1.CellWidth = 80

        DayPilotScheduler1.RowHeaderWidth = 470
        DayPilotScheduler1.RowHeaderWidthAutoFit = True

        DayPilotScheduler1.HeaderFontSize = "11pt"
    End Sub
    Public Sub Print()
        Dim mCompanyDetail As New CompanyDetail
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim StartDateM As New SmartDate
        Dim EndDateM As New SmartDate
        Dim year As String = cmbYear.SelectedItem.ToString
        StartDateM = New SmartDate(DateAdd(DateInterval.Month, 0, DateSerial(Val(year), 1, 1)), False)
        EndDateM = New SmartDate(CStr(DateSerial(StartDateM.Date.Year, StartDateM.Date.Month + 11, DateTime.DaysInMonth(StartDateM.Date.Year, StartDateM.Date.Month))), False)


        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        'Dim rpt As AuditPlanner
        Dim ds As New dsAuditPlanner
        myReport = New crptAuditPlanner
        'rpt = AuditPlanner.GetAuditPlannerList(StartDateM.ToString, EndDateM.ToString)
        If mAuditCalendar.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                        mCompanyDetail.Email, WebSite:="", ReportName:="CAMO QUALITY SYSTEM - AUDIT PLAN",
                                        SearchStr1:="Date Range: " + StartDateM.FormattedText + " to " + EndDateM.FormattedText, SearchStr2:="", SearchStr3:="",
                                        SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                        SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"),
                                        SearchStr11:="", SearchStr12:="", SearchStr13:="", SearchStr14:="", SearchStr15:="", SearchStr16:="",
                                        SearchStr17:="", SearchStr18:="", SearchStr19:="", SearchStr20:="", SearchStr21:="", SearchStr22:="",
                                        SearchStr23:="", SearchStr24:="", SearchStr25:="", SearchStr26:="", SearchStr27:="", SearchStr28:="",
                                        SearchStr29:="", SearchStr30:="", SearchStr31:="", SearchStr32:="", SearchStr33:="", SearchStr34:="",
                                        SearchStr35:="", SearchStr36:="", SearchStr37:="", SearchStr38:="", SearchStr39:="", SearchStr40:="",
                                        SearchStr41:="", SearchStr42:="", SearchStr43:="", SearchStr44:="", SearchStr45:="", SearchStr46:="",
                                        SearchStr47:="", SearchStr48:="", SearchStr49:="", SearchStr50:="")


        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        'da.Fill(ds, rpt)
        da.Fill(ds, mAuditListForCalandarYear)
        da.Fill(ds, mAuditCalendar)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mReport)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfAuditCalendarPlanList_AJAX.aspx?"
            Dim i As Integer
            If cmbYear.Items.Count = 0 Then 'Or cmbYear.SelectedValue = "" Then
                For i = -10 To 10
                    cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today).Year)
                Next
                cmbYear.DataBind()
                cmbYear.SelectedIndex = 10
            End If

            DatafieldBind()
            LoadResource(0)

            'btnPrint.Visible = IIf(AppSettings("ClientCode") = "GEP", True, False)
            btnPrint.Visible = IIf(AppSettings("ClientCode") = "KAS", True, False)
            upnlSchedulerList.Update()
            '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)

        End If


    End Sub
    Protected Sub cmbYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbYear.SelectedIndexChanged
        Try

            DayPilotScheduler1.StartDate = New DateTime(cmbYear.SelectedItem.ToString, 1, 1)
            DayPilotScheduler1.Days = 365

            GetSpotTypeList()

            upnlSchedulerList.Update()

        Catch ex As Exception

        End Try

    End Sub
    'Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        If CInt(Session("Count")) > 0 Then
    '            'Count = CInt(Session("Count")) + 1
    '            'hdncount.Value = Count
    '            'Session("Count") = Count
    '            If CInt(Session("Count")) = 3 Then
    '                Count = 0
    '                Session("Count") = Count
    '                Session("ChangeForm") = "ChangeForm"
    '                'Timer1.Enabled = False
    '            End If
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)
    '            Exit Sub
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Protected Sub DayPilotScheduler1_BeforeEventRender(sender As Object, e As DayPilot.Web.Ui.Events.Scheduler.BeforeEventRenderEventArgs) Handles DayPilotScheduler1.BeforeEventRender
        mAuditCalendar = Session("mAuditCalendar")

        Dim info As AuditCalandar.AuditCalandarInfo = mAuditCalendar.Item(New Guid(e.Id.ToString))

        '  e.BackgroundColor = "#FAD0A3"


        e.ToolTip = info.AuditNoCalc

    End Sub
    Protected Sub DayPilotScheduler1_EventClick(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.EventClickEventArgs) Handles DayPilotScheduler1.EventClick

    End Sub
    Protected Sub DayPilotScheduler1_TimeRangeSelected(ByVal sender As Object, ByVal e As DayPilot.Web.Ui.Events.TimeRangeSelectedEventArgs) Handles DayPilotScheduler1.TimeRangeSelected
        Dim mDate As Date = e.Start
        Dim thisCulture = Globalization.CultureInfo.CurrentCulture
        DatafieldBind()
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            DatafieldBind()
            LoadResource(1)
            Print()


            'Commneted on 15-Sep-2023
            ''Dim mCompanyDetail As New CompanyDetail
            ''mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

            ''Dim ds As New dsGraFlyingHrs

            ''Dim pdfDoc As iTextSharp.text.Document = New iTextSharp.text.Document(iTextSharp.text.PageSize.A4_LANDSCAPE, 10.0!, 10.0!, 10.0!, 0.0!)
            ''pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate())
            ''Dim mPDFWriter As PdfWriter
            ''mPDFWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
            ''pdfDoc.Open()

            ''Dim stream As MemoryStream = New MemoryStream
            ''Dim mrptImage As rptImage

            ''DayPilotScheduler1.Export(ImageFormat.Png)
            ''mrptImage = rptImage.GetImage(ds)

            '''''Header
            ''Dim DataTable As PdfPTable = New PdfPTable(4)
            ''Dim Header_1 As New PdfPCell '= New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "Graphical Representation of Flying Hours", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
            ''Dim Header_2 As PdfPCell
            ''If AppSettings("ClientCode") = "GEP" Then
            ''    Header_2 = New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "GEPL CAMO QUALITY SYSTEM - AUDIT PLAN", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
            ''Else
            ''    Header_2 = New PdfPCell(New Phrase(mCompanyDetail.CompanyName + vbCrLf + vbCrLf + "CAMO QUALITY SYSTEM - AUDIT PLAN", FontFactory.GetFont(FontFactory.HELVETICA, 9, 1)))
            ''End If

            ''If Not mrptImage Is Nothing Then
            ''    Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(mrptImage(0).ImageFile)
            ''    image.ScaleToFit(60, 60)
            ''    image.Alignment = 0
            ''    Header_1.AddElement(image)
            ''    Header_1.Border = iTextSharp.text.Rectangle.NO_BORDER
            ''    Header_1.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER
            ''    Header_1.Colspan = 1
            ''    DataTable.AddCell(Header_1)
            ''End If

            ''Header_2.Border = iTextSharp.text.Rectangle.NO_BORDER
            ''Header_2.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER
            ''Header_2.Colspan = 3


            ''DataTable.AddCell(Header_2)
            ''DataTable.WidthPercentage = 95
            ''pdfDoc.Add(DataTable)


            ''Dim p1 As Paragraph = New Paragraph()
            ''p1.Alignment = Element.ALIGN_CENTER

            ''pdfDoc.Add(p1)


            '''Calendar
            ''GetSpotTypeList()
            ''DayPilotScheduler1.Layout = LayoutEnum.TableBased

            ''Dim img As MemoryStream = DayPilotScheduler1.Export(ImageFormat.Png)

            ''Dim chartImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(img.GetBuffer)
            ''chartImage.ScalePercent(46.0!)
            ''chartImage.Alignment = Element.ALIGN_MIDDLE

            ''p1 = New Paragraph()
            ''p1.Alignment = Element.ALIGN_CENTER

            ''pdfDoc.Add(p1)


            ''chartImage.SetAbsolutePosition(20, pdfDoc.PageSize.Height / 10)
            ''pdfDoc.Add(chartImage)



            ''''Footer : Sign details
            ''Dim tableSign As New PdfPTable(2)

            ''p1 = New Paragraph()
            ''p1.Alignment = Element.ALIGN_CENTER
            ''pdfDoc.Add(p1)



            '''''Footer
            ''Dim table As New PdfPTable(1)
            ''table.WidthPercentage = 95

            ''Dim Product As PdfPCell = New PdfPCell(New Phrase(AppSettings("Product Version"), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))
            '''Dim SINote As PdfPCell = New PdfPCell(New Phrase(AppSettings("SINote"), FontFactory.GetFont(FontFactory.HELVETICA, 6, 1)))

            ''Product.Border = iTextSharp.text.Rectangle.NO_BORDER
            ''Product.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT
            ''Product.Colspan = 1
            ''' SINote.Border = iTextSharp.text.Rectangle.NO_BORDER
            '''SINote.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_RIGHT

            ''' SINote.Colspan = 1
            ''table.AddCell(Product)
            ''' table.AddCell(SINote)

            '''table.SetWidthPercentage(95.0)

            ''table.TotalWidth = 580.0F

            ''table.WriteSelectedRows(0, -1, 0, 30, mPDFWriter.DirectContent)

            ''' pdfDoc.Add(table)
            '''************************************

            ''Response.ContentType = "application/pdf"
            ''Response.AddHeader("content-disposition", "attachment;filename=Chart.pdf")
            ''Response.Cache.SetCacheability(HttpCacheability.NoCache)





            ''pdfDoc.Close()
            ''Response.Write(pdfDoc)
            '------------------------
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region " Export To PDF "
#End Region

    '#Region "Web Methods"

    '    '    <WebMethod(EnableSession:=True)> _
    '    <System.Web.Services.WebMethod()> _
    '    Public Shared Function TestOnWebService() As String


    '        Dim StartDateM As New SmartDate
    '        Dim EndDateM As New SmartDate
    '        Dim year As String = "2015" 'Today.Year.ToString
    '        StartDateM = New SmartDate(DateAdd(DateInterval.Month, 0, DateSerial(Val(year), 1, 1)), False)
    '        EndDateM = New SmartDate(CStr(DateSerial(StartDateM.Date.Year, StartDateM.Date.Month + 11, DateTime.DaysInMonth(StartDateM.Date.Year, StartDateM.Date.Month))), False)
    '        'If Not tmpWOstatusID = Val(WOStatusID) Or Not (tmpCustomerID.Equals(New Guid(CustomerID))) Or Not (tmpMonth = Val(month) Or Not (tmpYear = Val(year))) Then
    '        '    'mnWOPlannedList = nWOList.GetWOList(WOStatusID:=4)
    '        '    tmpWOstatusID = WOStatusID
    '        '    tmpCustomerID = New Guid(CustomerID)
    '        '    tmpMonth = Val(month)
    '        '    tmpYear = Val(year)
    '        '    mnWOPlannedList = nWOListForPlanCalendar.GetWOListForPlanCalendar(WOStatusID:=tmpWOstatusID, CustomerID:=CustomerID, FromDate:=StartDateM.ToString, ToDate:=EndDateM.ToString)
    '        '    PlannedList = New JavaScriptSerializer().Serialize(mnWOPlannedList)
    '        'End If
    '        Dim mAuditCalendar As AuditCalandar
    '        mAuditCalendar = AuditCalandar.GetAuditCalandarList(StartDateM.ToString, EndDateM.ToString)
    '        PlannedList = New JavaScriptSerializer().Serialize(mAuditCalendar)

    '        Dim jss = New JavaScriptSerializer()

    '        Dim data = jss.Deserialize(Of Object)(PlannedList) 'JsonConvert.DeserializeObject(Of MaintenanceActiivtyStatusList.MaintenanceActiivtyStatusListInfo)(DueValues)


    '        PlannedList = PlannedList.Replace("AuditStandard", "title").Replace("StartDate", "start").Replace("ID", "id")
    '        '  PlannedList = PlannedList.Replace("DescriptionCalender", "title").Replace("WOPlanedAndWODateCalender", "start").Replace("ID", "id")
    '        Return PlannedList
    '    End Function
    '#End Region

End Class