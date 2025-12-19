
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System.Linq.Enumerable
Imports System
Imports System.IO
Imports System.Text

Public Class wfCWPList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mCWP As CWP
    Private mCWPList As CWPList
    Private mDistinctCWPText As DistinctCWPText
    Dim DateIndex, FromDate, ToDate, CWPText, StatusID, No, PartName, SerialNo, BarcodeNo As String
    Dim EventLogID As Guid
    Dim totcnt As Integer
    Dim mFileAttach As FileAttach

    Dim mCWPStatusList As CWPStatusList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCWP = Session("mCWP")
        mCWPList = Session("mCWPList")
        mDistinctCWPText = Session("mDistinctCWPText")

        CWPText = Session("CWPText")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")

        DateIndex = Session("DateIndex")
        StatusID = Session("StatusID")

        'RegNo = Session("RegNo")
        SerialNo = Session("SerialNo")
        PartName = Session("PartName")
        BarcodeNo = Session("BarcodeNo")
    End Sub
    Private Sub SetSession()
        Session("mCWP") = mCWP
        Session("mCWPList") = mCWPList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate

        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID

        Session("No") = No
        ' Session("RegNo") = RegNo
        Session("PartName") = PartName
        Session("SerialNo") = SerialNo
        Session("CWPText") = CWPText

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCWPList")
        Session.Remove("mCWP")

        Session.Remove("FromDate")
        Session.Remove("ToDate")

        Session.Remove("DateIndex")
        Session.Remove("StatusID")

        Session.Remove("No")
        Session.Remove("RegNo")
        Session.Remove("PartName")
        Session.Remove("SerialNo")
        Session.Remove("CWPText")
        Session.Remove("BarcodeNo")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfCWPList_Ajax.aspx") <= 0 Then
            RemoveSession()
            Session.Remove("mCWPList")
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetGrid()
        txtNo.Visible = IIf(cmbCWP.SelectedIndex > 0, True, False)
        lblNo.Visible = IIf(cmbCWP.SelectedIndex > 0, True, False)
        'Dim B As Boolean
        'Dim img As New ImageButton
        'For j As Integer = 0 To dgCWPList.Rows.Count - 1
        '    B = CType(Me.dgCWPList.Rows.Item(j).Cells(16).Text, Boolean)
        '    If B = False Then
        '        img = dgCWPList.Rows.Item(j).Cells(15).FindControl("History")
        '        img.Visible = False
        '        dgCWPList.Rows.Item(j).Cells(15).Enabled = False
        '    End If
        'Next
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub setVariables()

        DateIndex = IIf(cmbDate.SelectedIndex <= 0, 1, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

        CWPText = IIf(cmbCWP.SelectedIndex <= 0, "", cmbCWP.SelectedValue)

        'StatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        '  RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedValue)
        PartName = txtPart.Text.Trim
        SerialNo = txtSerialNo.Text.Trim
        No = IIf(cmbCWP.SelectedIndex <= 0, 0, txtNo.Text.Trim)
        BarcodeNo = txtBarcodeNo.Text.Trim
        StatusID = IIf(cmbCWPStatus.SelectedIndex <= 0, 0, cmbCWPStatus.SelectedValue.ToString)
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID
        Session("No") = No
        'Session("RegNo") = RegNo
        Session("PartName") = PartName
        Session("SerialNo") = SerialNo
        Session("CWPText") = CWPText
        Session("BarcodeNo") = BarcodeNo
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal RegNo As String = "", Optional ByVal PartName As String = "", Optional ByVal SerialNo As String = "", Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "", Optional ByVal BarcodeNo As String = "")
        mCWPList = Nothing
        dgCWPList.DataSource = Nothing

        mCWPList = CWPList.GetCWPList(Text, No, FromDate, ToDate, RegNo, PartName, SerialNo, StatusID, AddTopItem, , BarcodeNo)
        dgCWPList.DataSource = mCWPList
        Session("mCWPList") = mCWPList
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        setVariables()
        FindNow(CWPText, Val(No), FromDate, ToDate, "", PartName, SerialNo, Val(StatusID), "", BarcodeNo)
        dgCWPList.DataBind()

        cmbDate.SelectedIndex = DateIndex
        'cmbAircraft.SelectedValue = IIf(RegNo = "", "(ALL)", RegNo)

        'cmbModel.SelectedValue = IIf(ModelName = "", "(ALL)", ModelName) 
        cmbCWP.SelectedValue = IIf(CWPText = "", "(ALL)", CWPText)
        txtNo.Text = No


        ControlVisibility(DateIndex)
        dgCWPList.DataBind()
        lblResult.Text = "List of Component WorkPackage as per criteria :" & mCWPList.Count & " Record(s) found."

    End Sub
    Private Sub ControlVisibility(Optional ByVal DateIndex As Int32 = 0)
        If DateIndex = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub SetTitle()
        Dim mCWPList As CWPList
        mCWPList = CWPList.GetCWPList
        Session("totcnt") = mCWPList.Count
        totcnt = Session("totcnt")
        lbltitle.InnerText = "List of Component Package   [Total No of Record(s):-" + totcnt.ToString() + "]"
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean) 'Added By Vikrant On 01-Dec-2014
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 1) 'Sort = 1 - Removal
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewHistory(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim myReportReverse As CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsCWP
        mCWP = CWP.GetCWP(ID)

        Dim CWPNo As String = "CWP-" & mCWP.CWPNo.ToString + "-"

        mCWPList = CWPList.GetCWPList(, , , , , mCWP.PartNo, mCWP.SerialNo)

        myReport = New crptFrontHistoryCard
        myReportReverse = New crptReverseHistoryCard

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, "", mCWP.PartNo, mCWP.SerialNo, "", AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", , "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mCWPList)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        myReportReverse.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)


        Dim PDFNo As Integer = 1
        Dim PDFNoChild As Integer = 1
        Dim tmp As Integer
        Dim a As New Random

        tmp = a.Next

        Dim MyFile1 = "C:\Temp\" & CWPNo & tmp & PDFNo.ToString & ".pdf"

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

        Dim pdfList As New System.Collections.ArrayList

        pdfList.Add(MyFile1)
        PDFNo = PDFNo + 1

        Session("myReportReverse") = myReportReverse
        tmp = a.Next

        MyFile1 = "C:\Temp\" & CWPNo & tmp & PDFNo.ToString & ".pdf"

        myReportReverse = CType(Session("myReportReverse"), CrystalDecisions.CrystalReports.Engine.ReportClass)

        Dim myDiskOptionJob As CrystalDecisions.Shared.DiskFileDestinationOptions
        Dim CrFormatTypeOptions As New CrystalDecisions.Shared.PdfFormatOptions
        myDiskOptionJob = New CrystalDecisions.Shared.DiskFileDestinationOptions
        myDiskOptionJob.DiskFileName = MyFile1
        myExportOption = myReportReverse.ExportOptions
        With myExportOption
            .DestinationOptions = myDiskOptionJob
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
            .FormatOptions = CrFormatTypeOptions
        End With

        Try
            myReportReverse.Export()
            myReportReverse.Close()
            myReportReverse.Dispose()
            GC.Collect()
        Catch ex As Exception
            Throw ex
        End Try


        pageCount = 0

        pdfList.Add(MyFile1)
        PDFNo = PDFNo + 1

        ' //********************************************Send Files for Merging****************************************************//
        Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
        
        Dim filesByte As New List(Of Byte())()
        For Each file__1 As String In pdfList 'files
            filesByte.Add(File.ReadAllBytes(file__1))
        Next

        File.WriteAllBytes(MergedPath, FlyPal.PDFMergers.MergeFiles(filesByte))

        ''//********************************************Set Sessions*********************************************************//
        Session("CrystalReport") = MergedPath
        Session("PrintReportWithAttachment") = "True"

        '//*******************************************Delete created file*********************************************************//

        Dim DeleteThis As String = CWPNo
        Dim Files As String() = Directory.GetFiles("C:\Temp\")

        For Each file__1 As String In Files
            If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                File.Delete(file__1)
            End If
        Next
        'End

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim CWPTextNo As String
                        Try
                            Dim mCWP As CWP
                            Session("sender") = ""
                            mCWP = CType(Session("mCWP"), CWP)
                            CWPTextNo = mCWP.CWPTextNo
                            CWP.DeleteCWP(mCWP.ID)
                            'mCWP.Delete()
                            'mCWP.Save()
                            DataFieldBind()
                            SetControl()
                            SetGrid()

                            upnlGrid.Update()
                            upnlResult.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "CWP", "Can't delete : " + CWPTextNo + " is Currently in use", Util.ErrorType.NoError, mCWP.ID, EventLogID)
                            End If
                            DataFieldBind()
                            SetControl()
                            SetGrid()
                            upnlGrid.Update()
                            upnlResult.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "CWP", CWPTextNo, Util.ErrorType.NoError, mCWP.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region "DataFieldBind"
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

        mDistinctCWPText = DistinctCWPText.GetDistinctCWPText("(ALL)")
        cmbCWP.DataSource = mDistinctCWPText
        Session("mDistinctCWPText") = mDistinctCWPText

        mCWPStatusList = CWPStatusList.GetCWPStatusList("(ALL)")
        cmbCWPStatus.DataSource = mCWPStatusList
        Session("mCWPStatusList") = mCWPStatusList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfCWPList_Ajax.aspx"
            DataFieldBind()
            SetControl()
        End If

        SetGrid()
        SetTitle()
    End Sub

    Private Sub cmbDate_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If

        setVariables()
        FindNow(CWPText, Val(No), FromDate, ToDate, "", PartName, SerialNo, 0, "", BarcodeNo)
        dgCWPList.DataBind()
        SetGrid()
        ControlVisibility()
        lblResult.Text = "List of Component WorkPackage as per criteria :" & mCWPList.Count & " Record(s) found"
        upnlGrid.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
        upnlCWP.Update()
        upnlCWPNo.Update()
        upnlCWPlblNo.Update()
    End Sub
    Private Sub txtToDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtFromDate.TextChanged, txtToDate.TextChanged, txtPart.TextChanged, txtSerialNo.TextChanged, cmbSchedule.SelectedIndexChanged, cmbCWP.SelectedIndexChanged, txtNo.TextChanged, cmbCWPStatus.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setPeriod(DateIndex)

        setVariables()
        FindNow(CWPText, Val(No), FromDate, ToDate, "", PartName, SerialNo, Val(StatusID), "", BarcodeNo)
        dgCWPList.DataBind()
        SetGrid()
        ControlVisibility()
        lblResult.Text = "List of Component WorkPackage as per criteria :" & mCWPList.Count & " Record(s) found"
        upnlGrid.Update()
        upnlResult.Update()
        upnlCWPNo.Update()
        upnlCWP.Update()
        upnlCWPlblNo.Update()
    End Sub
    Private Sub dgCWPList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCWPList.PageIndexChanging
        dgCWPList.PageIndex = e.NewPageIndex
        dgCWPList.DataSource = mCWPList
        Session("mCWPList") = mCWPList
        dgCWPList.DataBind()
        SetGrid()
    End Sub


    Private Sub dgCWPList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCWPList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("CWPView") And Not User.IsInRole("CWPEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "CWP", User.Identity.Name & " is not Authorized User to edit " + mCWP.CWPTextNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If

                mCWP = CWP.GetCWP(mID)

                Session("mCWP") = mCWP
                Dim mCWPDetail As String = "CWP : " + mCWP.CWPTextNo + " dated : " + mCWP.CWPStartDateFormatted
                MarkLog(Util.Action.Edit, "Aircraft", mCWPDetail, Util.ErrorType.NoError, mCWP.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCWP_Ajax.aspx?BackPage=Index.aspx');", True)

            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("CWPDelete")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "CWP", User.Identity.Name & " is not Authorized User to delete " + mCWP.CWPTextNo, Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    Exit Sub
                    '************************************
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                    mCWP = CWP.GetCWP(mID)
                    Session("mCWP") = mCWP
                End If
            Case "HistoryRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim IsHistoryExists As Boolean = mCWPList(mID).IsHistoryExists
                SetGrid()
                ViewHistory(mID, IsHistoryExists)
        End Select
    End Sub
    Private Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        mCWP = CWP.NewCWP()
        MarkLog(Util.Action.[New], "CWP", "", Util.ErrorType.NoError, mCWP.ID, EventLogID)
        mCWP.ScheduleID = Val(cmbCWPType.SelectedValue)
        Session("mCWP") = mCWP
        SetGrid()
        upnlGridView.Update()
        '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
        Dim str As String
        str = "openledgersame('wfCWPPendingOrderItemList_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Session.Remove("Idx")
        Session.Remove("SearchForText")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsCWP
        mCWPList = Session("mCWPList")

        Dim CWPTextNo As String = ""
        CWPTextNo = IIf(cmbCWP.SelectedIndex > 0, cmbCWP.SelectedItem.ToString + "-" + txtNo.Text, "(ALL)")
        myReport = New crptCWPList
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, "", IIf(cmbDate.SelectedIndex > 0, txtFromDate.Text.ToString, "(ALL)"), IIf(cmbDate.SelectedIndex > 0, txtToDate.Text.ToString, "(ALL)"), CWPTextNo, AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", , "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mCWPList)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

    Private Sub dgCWPList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCWPList.Sorting
        mCWPList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgCWPList.DataSource = mCWPList
        Session("mCWPList") = mCWPList
        dgCWPList.DataBind()
        SetGrid()
    End Sub
    Private Sub txtBarcodeNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtBarcodeNo.TextChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setPeriod(DateIndex)

        setVariables()
        FindNow(CWPText, Val(No), FromDate, ToDate, "", PartName, SerialNo, 0, "", BarcodeNo)
        dgCWPList.DataBind()
        SetGrid()

        lblResult.Text = "List of Component WorkPackage as per criteria :" & mCWPList.Count & " Record(s) found"
        upnlGrid.Update()
        upnlResult.Update()
        upnlCWPNo.Update()
        upnlCWP.Update()
    End Sub
#End Region

   
End Class