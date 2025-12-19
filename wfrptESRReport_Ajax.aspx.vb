Imports System.Linq
Imports System.Text

Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Imports System.Linq.Enumerable
Imports System
Imports System.IO


'Create By Utkarsh On 10-Nov-2011
Partial Class wfrptESRReport_Ajax
    Inherits System.Web.UI.Page


#Region "Variable Declaration"
    Public mModelList As ModelList
    Dim mESRTypeWiseAircraftRegDetails As ESRTypeWiseAircraftRegDetails

    Dim ChkModelIDs As String()
    Dim ModelIDs As New StringBuilder
    Dim ChkRegNos As String()
    Dim MachineIDs As New StringBuilder

    Dim ChkModelNames As String()
    Dim mModelNames As New StringBuilder
    Dim ChkMachineNames As String()
    Dim mMachineNames As New StringBuilder
    Dim mMachineNameValueList As MachineNameValueList

    Dim ChkAircraftModelIDs As String()
    Dim AircraftModelIDs As New StringBuilder
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mModelList") = mModelList
    End Sub
    Private Sub GetSession()
        mModelList = Session("mModelList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        Catch ex As SqlException

                        End Try
                    End If
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub AddAttributes()
        btnDisplay.Attributes("onclick") = "javascript: document.body.style.cursor = 'wait';"
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBinding()
        'Commented and added by Shweta o 29-August-2013 for -ALL29082013-1
        'mModelList = ModelList.GetModelList(1, "", , , "(SELECT)")
        mModelList = ModelList.GetAirframeModelList()
        'end
        ListModel.DataSource = mModelList
        'cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList
        'cmbModel.DataBind()
        ListModel.DataBind()

        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.Date.ToString(AppSettings("DateFormat")), , , , , , , , , , True)
        ListRegNo.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        ListRegNo.DataBind()
        txtFromDate.Text = Today.Date.AddMonths(-1).ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelList")
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
    End Sub
    Private Sub SetValues()
        'For i As Integer = 0 To ListModel.Items.Count - 1
        '    ListModel.Items(i).Selected = True
        'Next

        'For i As Integer = 0 To ListRegNo.Items.Count - 1
        '    ListRegNo.Items(i).Selected = True
        'Next


        ChkModelIDs = (From c As System.Web.UI.WebControls.ListItem In ListModel.Items
                       Where c.Selected = True
                       Select (c.Value)).ToArray

        ChkModelNames = (From c As System.Web.UI.WebControls.ListItem In ListModel.Items
                      Where c.Selected = True
                      Select (c.Text)).ToArray
        If ChkModelIDs.Length > 0 Then


            ModelIDs.Append("<ModelID>")
            For i As Integer = 0 To ChkModelIDs.Count - 1
                ModelIDs.Append("<id>")
                ModelIDs.Append(ChkModelIDs(i))
                ModelIDs.Append("</id>")


                mModelNames.Append(ChkModelNames(i))
                mModelNames.Append(",")
                mModelNames.Append(" ")
            Next
            ModelIDs.Append("</ModelID>")


        End If


        ChkRegNos = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
                       Where c.Selected = True
                       Select (c.Value)).ToArray

        ChkMachineNames = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
                       Where c.Selected = True
                       Select (c.Text)).ToArray
        If ChkRegNos.Length > 0 Then
            MachineIDs.Append("<MachineID>")
            For i As Integer = 0 To ChkRegNos.Count - 1
                MachineIDs.Append("<id>")
                MachineIDs.Append(ChkRegNos(i))
                MachineIDs.Append("</id>")

                mMachineNames.Append(ChkMachineNames(i))
                mMachineNames.Append(",")
                mMachineNames.Append(" ")
            Next
            MachineIDs.Append("</MachineID>")


            Dim ModelListAsPerAircraft As ModelList = ModelList.GetAirframeModelList(, MachineIDs.ToString)  'Used for Distribution List

            ChkAircraftModelIDs = (From c As ModelList.ModelInfo In ModelListAsPerAircraft
                         Select (c.ID.ToString)).ToArray

            If ChkAircraftModelIDs.Length > 0 Then


                AircraftModelIDs.Append("<ModelID>")
                For i As Integer = 0 To ChkAircraftModelIDs.Count - 1
                    AircraftModelIDs.Append("<id>")
                    AircraftModelIDs.Append(ChkAircraftModelIDs(i))
                    AircraftModelIDs.Append("</id>")
                Next
                AircraftModelIDs.Append("</ModelID>")
            End If

        End If

        lblModel1.Text = IIf(mModelNames.ToString = "", "Aircraft(s) : " + mMachineNames.ToString.Trim.TrimEnd(","), "Model(s) : " + mModelNames.ToString.Trim.TrimEnd(","))
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Try

            SetValues()


            Dim da As New CSLA.Data.ObjectAdapter
            Dim mCompanyDetail As CompanyDetail
            Dim ReportName As String = String.Empty
            Dim ds As New dsESR
            Dim mrptImage As rptImage

            ReportName = "ENGINEERING STATISTICS REPORT"
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                     mCompanyDetail.WebSite, ReportName, CDate(txtFromDate.Text).ToString("MMM"), CDate(txtFromDate.Text).ToString("yyyy"), CDate(txtToDate.Text).ToString("MMM"), CDate(txtToDate.Text).ToString("yyyy"), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), SearchStr10:=AppSettings("Logo"), SearchStr21:=mModuleList.Item("ESR").FormRevisionNo)

            'Page 1 : First Page
            Dim myReport = New crESRFirstPage
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            Dim PDFNo As Integer = 1
            Dim PDFNoChild As Integer = 1
            Dim tmp As Integer
            Dim a As New Random
            Dim pageCount As Integer = 0

            Dim pdfList As New System.Collections.ArrayList

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ESRFirstPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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


            ''''''Page 2 Distribution List

            Dim mESRDistributionList As DistributionList
            myReport = New crESRDistribution

            mESRDistributionList = DistributionList.GetDistributionList(Guid.Empty, , , , IIf(ModelIDs.ToString = "", AircraftModelIDs.ToString, ModelIDs.ToString))


            Dim TempESRDistributionList = From c In mESRDistributionList
                                          Order By c.Name Ascending
                                          Select New With {Key c.Name, Key c.CategoryName, Key c.Remark} Distinct.ToList 'Added By Prashant 9-Feb-2022 ALL09022022

            ds.Clear()
            da.Fill(ds, "DistributionList", TempESRDistributionList)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRDistributionPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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


            ''''''Page 4 GLOSSARY / DEFINITION OF TERMS
            'Added by Shital on 17-Nov-2021

            myReport = New crESRDefinationofTermPage

            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRDEFINITIONOFTERMS" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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


            'Added by Vikrant on 17-Nov-2021
            'Page 5 : TYPE WISE AIRCRAFT REGISTRATION DETAILS
            Dim mESRTypeWiseAircraftRegDetails As ESRTypeWiseAircraftRegDetails
            myReport = New crptESRTypewiseAircraftRegDetails
            mESRTypeWiseAircraftRegDetails = ESRTypeWiseAircraftRegDetails.GetList(txtFromDate.Text, txtToDate.Text, ModelIDs.ToString, MachineIDs.ToString)

            ds.Clear()
            da.Fill(ds, mESRTypeWiseAircraftRegDetails)
            da.Fill(ds, Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRTypeWiseAircraftRegDetails" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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

            'Page 6 :AIRCRAFT OPERATIONAL REVIEW
            Dim mESRAircraftOperationalReview As ESRAircraftOperationalReview
            myReport = New crptESRAircraftOperationalReview
            mESRAircraftOperationalReview = ESRAircraftOperationalReview.GetList(txtFromDate.Text, txtToDate.Text, ModelIDs.ToString, MachineIDs.ToString)

            ds.Clear()
            da.Fill(ds, mESRAircraftOperationalReview)
            da.Fill(ds, Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRAircraftOperationalReview" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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
            'End

            '-----
            'Page 7 :GRAPH: Aircraft in service & Hours Flown 

            Dim mESRAircraftInService As ESRAircraftInService
            Dim mESRAircraftWiseHours As ESRAircraftWiseHours
            myReport = New crESRAircraftInService
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            mESRAircraftInService = ESRAircraftInService.GetList(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, ModelIDStr:=ModelIDs.ToString, _
                                                                 MachineIDStr:=MachineIDs.ToString)

            mESRAircraftWiseHours = ESRAircraftWiseHours.GetList(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, ModelIDStr:=ModelIDs.ToString, _
                                                                 MachineIDStr:=MachineIDs.ToString)
            ds.Clear()
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mESRAircraftInService)
            da.Fill(ds, mESRAircraftWiseHours)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            With myReport
                If mESRAircraftInService.Count > 0 And mESRAircraftWiseHours.Count > 0 Then
                    .ReportHeadersection8.SectionFormat.EnableSuppress = True
                End If
            End With

            MyFile1 = "C:\Temp\" & "ESRAircraftInService" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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


            '-----


            'Page 8 :ENGINE OPERATIONAL REVIEW

            myReport = New crESREngineOperationalReview

            Dim mESREngineOperationalReview As rptESREngineOperationalReview = rptESREngineOperationalReview.GetrptESREngineOperationalReview(txtFromDate.Text, txtToDate.Text, ModelIDs.ToString, MachineIDs.ToString)

            ds.Clear()
            da.Fill(ds, mESREngineOperationalReview)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESREngineOperationalReview" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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


            '----


            'Page 9 :ENGINE PREMATURE REMOVAL DETAILS (For the period of  Jul.21 to Sept.21)
            'Added by Shital on 18-Nov-2021

            myReport = New crESRPrematureEngineRemoval
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            Dim mESRPrematureRemovedEngineStatus As ESRPrematureRemovedEngineStatus
            mESRPrematureRemovedEngineStatus = ESRPrematureRemovedEngineStatus.GetESRPrematureRemovedEngineStatus(2, txtFromDate.Text.ToString, txtToDate.Text.ToString, ModelIDs.ToString, MachineIDs.ToString)
            Dim mESRmajorDefectList As ESRMajorDefectList
            mESRmajorDefectList = ESRMajorDefectList.GetESRMajorDefectList(txtFromDate.Text.ToString, txtToDate.Text.ToString, ModelIDs.ToString, MachineIDs.ToString)

            Dim mESRMELDefectList As ESRMELDefectList = ESRMELDefectList.GetESRMELDefectList(txtFromDate.Text.ToString, txtToDate.Text.ToString, ModelIDs.ToString, MachineIDs.ToString)

            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)
            da.Fill(ds, mESRPrematureRemovedEngineStatus)
            da.Fill(ds, mESRmajorDefectList)
            da.Fill(ds, mESRMELDefectList)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRPrematureEngineRemoval" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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

            '-----

            'Page 11 :DETAILS OF GROUND INCIDENTS (For the period of Jul.21 to Sept.21))---------------------------------------------------------------------------------------------------------

            myReport = New crESRGroundIncidents

            Dim TempESRMELDefectIncidentList = (From c In mESRMELDefectList
                               Where c.IncidentTypeID = 3 Or c.IncidentTypeID = -1 _
                               Order By c.RegNo Ascending
                             Group By Defect = c.Defect, RegNo = c.RegNo, DateOfOccurenceFormatted = c.DateOfOccurenceFormatted, Sector = c.Sector, Action = c.Action Into Group
                             Select New With {.Defect = Defect, .RegNo = RegNo, .DateOfOccurenceFormatted = DateOfOccurenceFormatted, .Sector = Sector, .Action = Action, .ReceiptItemCollection = Group}).ToList


            Dim TempESRMELDefectGroundIncidentsList = (From c In mESRMELDefectList
                               Where c.IncidentTypeID = 6 Or c.IncidentTypeID = -1 _
                               Order By c.RegNo Ascending
                             Group By Defect = c.Defect, RegNo = c.RegNo, DateOfOccurenceFormatted = c.DateOfOccurenceFormatted, Sector = c.Sector, Action = c.Action Into Group
                             Select New With {.Defect = Defect, .RegNo = RegNo, .DateOfOccurenceFormatted = DateOfOccurenceFormatted, .Sector = Sector, .Action = Action, .ReceiptItemCollection = Group}).ToList

            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, "ESRMELDefectList", TempESRMELDefectIncidentList)
            da.Fill(ds, "ESRMELDefectGroundIncidentsList", TempESRMELDefectGroundIncidentsList)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRGroundIncidents" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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
            'End Of Page 11 :DETAILS OF GROUND INCIDENTS (For the period of Jul.21 to Sept.21))---------------------------------------------------------------------------------------------------------

            '-----ESR ATA Wise Defect List
            myReport = New crESRATAWiseDefectList

            Dim mrptESRATAWiseDefectList As rptESRATAWiseDefectList = rptESRATAWiseDefectList.GetrptESRATAWiseDefectList(txtFromDate.Text, txtToDate.Text, ModelIDs.ToString, MachineIDs.ToString)

            ds.Clear()
            da.Fill(ds, mrptESRATAWiseDefectList)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRATAWiseDefectList" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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





            ' //********************************************Send Files for Merging****************************************************//
            Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
            Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

            Dim filesByte As New List(Of Byte())()

            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
            Next

            File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

            Dim PageNO_Glossary_Definition As Integer = getPageNoBySpecificText(1, MergedPath, "GLOSSARY / DEFINITION OF TERMS") + 1
            Dim PageNO_REGISTRATION As Integer = getPageNoBySpecificText(1, MergedPath, "TYPE WISE AIRCRAFT REGISTRATION DETAILS") + 1
            Dim PageNO_OperationalReview As Integer = getPageNoBySpecificText(1, MergedPath, "AIRCRAFT OPERATIONAL REVIEW") + 1
            Dim PageNO_Aircraft_in_service As Integer = getPageNoBySpecificText(1, MergedPath, "GRAPH: Aircraft in service & Hours Flown") + 1
            Dim PageNO_EngineOperationalReview As Integer = getPageNoBySpecificText(1, MergedPath, "ENGINE OPERATIONAL REVIEW") + 1
            Dim PageNO_PrematureEngineRemoval As Integer = getPageNoBySpecificText(1, MergedPath, "ENGINE PREMATURE REMOVAL DETAILS") + 1
            Dim ENGINE_IN_FLIGHT_SHUTDOWN_DETAILS As Integer = getPageNoBySpecificText(1, MergedPath, "ENGINE IN-FLIGHT SHUTDOWN DETAILS") + 1
            Dim DETAILS_OF_MAJOR_DEFECTS As Integer = getPageNoBySpecificText(1, MergedPath, "DETAILS OF MAJOR DEFECTS") + 1
            Dim PageNO_DETAILS_OF_GROUND_INCIDENTS As Integer = getPageNoBySpecificText(1, MergedPath, "DETAILS OF INCIDENTS") + 1
            Dim PageNO_ATAWiseDefectList As Integer = getPageNoBySpecificText(1, MergedPath, "SYSTEM WISE / ATA CHAPTER WISE BREAK UP OF REPORTED DEFECTS") + 1



            '''''''''''Page 3 INDEX
            ''''''Added by Shital on 17-Nov-2021

            myReport = New crESRIndexPage

            Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                 mCompanyDetail.WebSite, ReportName, CDate(txtFromDate.Text).ToString("MMM"), CDate(txtFromDate.Text).ToString("yyyy"), CDate(txtToDate.Text).ToString("MMM"), CDate(txtToDate.Text).ToString("yyyy"), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), SearchStr10:=AppSettings("Logo"), SearchStr11:=PageNO_Glossary_Definition.ToString, SearchStr12:=PageNO_REGISTRATION.ToString, SearchStr13:=PageNO_OperationalReview.ToString, SearchStr14:=PageNO_Aircraft_in_service.ToString, SearchStr15:=PageNO_EngineOperationalReview.ToString, SearchStr16:=PageNO_PrematureEngineRemoval.ToString, SearchStr17:=PageNO_ATAWiseDefectList.ToString, SearchStr18:=PageNO_DETAILS_OF_GROUND_INCIDENTS, SearchStr19:=ENGINE_IN_FLIGHT_SHUTDOWN_DETAILS, SearchStr20:=DETAILS_OF_MAJOR_DEFECTS, SearchStr21:=mModuleList.Item("ESR").FormRevisionNo)

            ds.Clear()

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRIndex" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

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


            MergedPath = "C:\Temp\" & "temp_myMergedPdfF.pdf"
            MergedPath_WM = "C:\Temp\" & "temp_myMergedPdf_WMF.pdf"

            filesByte = New List(Of Byte())()


            Dim i As Integer = 1
            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
                i = i + 1
                If i = 3 Then
                    filesByte.Add(File.ReadAllBytes(MyFile1))  'First Index Page
                End If
            Next

            File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

            'AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
            AddWatermarkText(MergedPath, MergedPath_WM, "Page ", , , iTextSharp.text.BaseColor.BLACK, , 0.0, pageCount) 'Added on 24-Jun-2019
            ''//********************************************Set Sessions*********************************************************//
            Session("CrystalReport") = MergedPath_WM
            Session("PrintReportWithAttachment") = "True"

            '//*******************************************Delete created file*********************************************************//

            'Commented and Added by Saylee on 2-Dec-2014
            ' Dim MyFile, MyFile_Ext As String
            'For j As Integer = 1 To PDFNo - 1
            '    MyFile = "C:\Temp\" & WONo & j.ToString & ".pdf"
            '    MyFile_Ext = "C:\Temp\" & WONo & j.ToString & "_Ext" & ".pdf"

            '    System.IO.File.Delete(MyFile)
            '    System.IO.File.Delete(MyFile_Ext)
            'Next

            Dim DeleteThis As String = "ESR"
            Dim Files As String() = Directory.GetFiles("C:\Temp\")

            For Each file__1 As String In Files
                If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                    File.Delete(file__1)
                End If
            Next
            'End

            RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1520)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)


            ''If ByMail = False Then
            ''    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            ''Else
            ''    SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblyear1.Text + ", " + lblModel1.Text, "", _
            ''                              Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
            ''                              ReportGenratedBy:=Session("ReportGenratedBy"), _
            ''        SmtpHost:=mModuleList.Item("ESR").SmtpHost, SmtpPort:=mModuleList.Item("ESR").SmtpPort, _
            ''        SmtpUser:=mModuleList.Item("ESR").SmtpUser, SmtpPassword:=mModuleList.Item("ESR").SmtpPassword)
            ''End If
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (SetReport Sub Method): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidate As CustomValidator
        custValidate = CType(s, CustomValidator)
        If custValidate.ControlToValidate = "cmbModel" Then
            'If cmbModel.SelectedIndex <= 0 Then
            '    custValidate.ErrorMessage = "Select the Model"
            '    e.IsValid = False
            'Else
            '    e.IsValid = True
            'End If
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'AddAttributes()
        If Not Page.IsPostBack Then
            DataFieldBinding()
        End If
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub ChkAircraftwise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAircraftwise.CheckedChanged
        If ChkAircraftwise.Checked Then
            lblAircraft.Visible = True
            ListRegNo.Visible = True
            upnlsearch.Update()
        Else
            'lblAircraft.Visible = False
            'ListRegNo.Visible = False
            upnlsearch.Update()
        End If
    End Sub
    Private Sub btnByMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnByMail.Click
        If Page.IsValid Then
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

            Session("UserEmailID") = mModuleList.Item("ESR").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("ESR").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub hdnimgBtnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            Dim email As New Thread(Sub() SetReport(True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    
End Class
