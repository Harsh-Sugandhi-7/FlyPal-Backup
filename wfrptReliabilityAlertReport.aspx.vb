
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System.Linq.Enumerable
Imports System
Imports System.IO
Imports System.Text

Public Class wfrptReliabilityAlertReport
    Inherits System.Web.UI.Page


#Region "Variable Declaration"
    Public mModelList As ModelList
    Public mATAList As ATAList

    Public IsAllATASelected As Boolean = False
    Public ATAIDs As New StringBuilder
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mModelList") = mModelList
    End Sub
    Private Sub GetSession()
        mModelList = Session("mModelList")
    End Sub
    Private Sub AddAttributes()
        btnDisplay.Attributes("onclick") = "javascript: document.body.style.cursor = 'wait';"
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
    Private Sub SetATASearchingCriteria()
        'ATAIDs.Append("<Customer>")
        For i As Integer = 0 To cmbATAList.Items.Count - 1
            If cmbATAList.Items(i).Selected Then
                If ATAIDs.Length = 0 Then
                    ATAIDs.Append(cmbATAList.Items(i).Value)
                Else
                    ATAIDs.Append(",")
                    ATAIDs.Append(cmbATAList.Items(i).Value)
                End If
                ' ATAIDs.Append("<id>")

            End If
        Next
        '  ATAIDs.Append("</Customer>")
    End Sub
    Private Sub DataFieldBinding()
        'Commented and added by Shweta o 29-August-2013 for -ALL29082013-1
        'mModelList = ModelList.GetModelList(1, "", , , "(SELECT)")
        mModelList = ModelList.GetAirframeModelList("(SELECT)")
        'end
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList
        cmbModel.DataBind()

        mATAList = ATAList.GetATAList()
        cmbATA.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATA.DataBind()

        cmbATAList.DataSource = mATAList
        cmbATAList.DataBind()

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelList")
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
        lblATA1.Visible = True
        upnlCriteria.Visible = True
    End Sub
    Private Sub SetValues()
        lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        lblModel1.Text = "Model : " & IIf(cmbModel.SelectedIndex > 0, cmbModel.SelectedItem.Text, "")
        lblATA1.Text = "ATA : " & IIf(cmbATA.SelectedIndex > 0, cmbATA.SelectedItem.Text, "")
    End Sub
    Private Sub SetReport()
        Dim SetNoRecord As Int32 = 0
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport 'dsReliabilityFlyingHoursRecord
        'dsDailyUtilizationGraph   '
        ReportName = "Pireps/Maintenance Defects Alert Level"
        SetValues()
        SetATASearchingCriteria()
        'If ATAIDs.Length = 1 Then
        '    Dim mrptATAWiseMonthlyAlertLevel As rptATAWiseMonthlyAlertLevel = rptATAWiseMonthlyAlertLevel.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), cmbModel.SelectedValue.ToString, IIf(rdbPireps.Checked = True, True, False), cmbATA.SelectedValue.ToString)

        '    Dim myReport = New crptATAWiseMonthlyAlertLevel


        '    'Added By Utkash ON 03-May-2013 FOR ALL03052013
        '    Dim StartDateM As New SmartDate
        '    Dim EndDateM As New SmartDate
        '    StartDateM.Text = CStr(DateAdd(DateInterval.Month, cmbMonth.SelectedIndex, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), 1, 1)))
        '    EndDateM.Text = CStr(DateAdd("d", -1, DateAdd("m", 1, StartDateM.Date)))


        '    mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        '    Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        '             mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        '             mCompanyDetail.WebSite, "", AppSettings("ClientCode"), IIf(rdbPireps.Checked = True, "Pireps", "Maintenance Defect"), AppSettings("Logo"), "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", cmbModel.SelectedItem.ToString, cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


        '    'myReport.SetDataSource(ds)


        '    If mrptATAWiseMonthlyAlertLevel.Count = 0 Then
        '        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
        '        msg1.ReplacePage = "wfrptReliabilityAlertReport.aspx?"
        '        msg1.Show()
        '        Exit Sub
        '    Else
        '        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1306)
        '    End If

        '    Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '    ds.Clear()
        '    da.Fill(ds, mrptATAWiseMonthlyAlertLevel)
        '    da.Fill(ds, Report)
        '    da.Fill(ds, mrptImage)
        '    myReport.SetDataSource(ds)


        '    Session("CrystalReport") = myReport

        '    'Dim Str As String
        '    'Str = "<script language=Javascript>openTranDetail();</script>"
        '    'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        '    Dim Str As String
        '    Str = "openTranDetail();"
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        'Else 'ALL ATA

        mATAList = Session("mATAList")
        Dim pdfList As New System.Collections.ArrayList
        Dim pageCount As Integer = 0
        'Code
        For i As Integer = 0 To cmbATAList.Items.Count - 1
            If cmbATAList.Items(i).Selected Then


                Dim ATAID As Guid = New Guid(cmbATAList.Items(i).Value)
                Dim ATA As ATAList.ATAInfo = mATAList(ATAID)
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

                Dim PDFNo As Integer = 1
                Dim PDFNoChild As Integer = 1
                Dim tmp As Integer
                Dim a As New Random

                tmp = a.Next

                Dim MyFile1 = "C:\Temp\" & "AlertRep" & tmp & PDFNo.ToString & ".pdf"

                '  myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions


                '   pdfList.Add(MyFile1)
                '  PDFNo = PDFNo + 1


                Dim mrptATAWiseMonthlyAlertLevel As rptATAWiseMonthlyAlertLevel = rptATAWiseMonthlyAlertLevel.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), cmbModel.SelectedValue.ToString, IIf(rdbPireps.Checked = True, True, False), ATA.ID.ToString)

                myReport = New crptATAWiseMonthlyAlertLevel


                'Added By Utkash ON 03-May-2013 FOR ALL03052013
                Dim StartDateM As New SmartDate
                Dim EndDateM As New SmartDate
                StartDateM.Text = CStr(DateAdd(DateInterval.Month, cmbMonth.SelectedIndex, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), 1, 1)))
                EndDateM.Text = CStr(DateAdd("d", -1, DateAdd("m", 1, StartDateM.Date)))


                mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                         mCompanyDetail.WebSite, "", AppSettings("ClientCode"), IIf(rdbPireps.Checked = True, "Pireps", "Maintenance Defect"), AppSettings("Logo"), "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", cmbModel.SelectedItem.ToString, cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


                'myReport.SetDataSource(ds)


                'If mrptATAWiseMonthlyAlertLevel.Count = 0 Then
                '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                '    msg1.ReplacePage = "wfrptReliabilityAlertReport.aspx?"
                '    msg1.Show()
                '    Exit Sub
                'Else
                '    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1306)
                'End If

                If mrptATAWiseMonthlyAlertLevel.Count <> 0 Then


                    Dim mrptImage As rptImage = rptImage.GetImage(ds)
                    ds.Clear()
                    da.Fill(ds, mrptATAWiseMonthlyAlertLevel)
                    da.Fill(ds, Report)
                    da.Fill(ds, mrptImage)
                    myReport.SetDataSource(ds)


                    Session("CrystalReport") = myReport

                    MyFile1 = "C:\Temp\" & "AlertRep" & tmp & PDFNo.ToString & ".pdf"
                    myExportOption = New CrystalDecisions.Shared.ExportOptions
                    myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions

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

                    pdfList.Add(MyFile1)
                    PDFNo = PDFNo + 1
                End If

            End If
        Next

        If pdfList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            'msg1.ReplacePage = "wfrptReliabilityAlertReport.aspx?"
            'msg1.Show()
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1306)
        End If

        Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
        Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

        Dim filesByte As New List(Of Byte())()
        For Each file__1 As String In pdfList 'files
            filesByte.Add(File.ReadAllBytes(file__1))
        Next

        File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

        AddWatermarkText(MergedPath, MergedPath_WM, "", , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
        ''//********************************************Set Sessions*********************************************************//
        Session("CrystalReport") = MergedPath_WM
        Session("PrintReportWithAttachment") = "True"
        Dim DeleteThis As String = "AlertRep"
        Dim Files As String() = Directory.GetFiles("C:\Temp\")

        For Each file__1 As String In Files
            If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                File.Delete(file__1)
            End If
        Next
        'End
        'Dim Str As String
        'Str = "openTranDetail();"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        'End If

    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidate As CustomValidator
        custValidate = CType(s, CustomValidator)
        If custValidate.ControlToValidate = "cmbModel" Then
            If cmbModel.SelectedIndex <= 0 Then
                custValidate.ErrorMessage = "Select the Model"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf custValidate.ControlToValidate = "cmbATA" Then
            '    If cmbATA.SelectedIndex <= 0 Then
            '        custValidate.ErrorMessage = "Select the ATA"
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'AddAttributes()
        If Not Page.IsPostBack Then
            SetCombo()
            DataFieldBinding()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region
End Class