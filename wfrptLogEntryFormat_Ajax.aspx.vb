Imports System.Collections.Generic
Imports Flypal.LogEntryFormat
Public Class wfrptLogEntryFormat_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList 'Added By Utkarsh On 19-Apr-2011
    Dim mAssemblylist As AssemblyList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As String
    Dim AssemblyID As String
    Dim Aircraft As String
    Dim AssemblyType As String
    Dim AssemblyText As String
    Dim Model As String
    Dim SerialNo As String
    Dim RegNo, SerialNoPosition As String
    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mLogEntryFormat As New LogEntryFormat
    Dim AssemblyTypeID As Integer

    Dim EventLogID As Guid
    Dim mLogBookSearchingCriteria As String = String.Empty
    Dim AOnDate, AOdate As String
    Public mLogEntList As List(Of LogEntryFormatInfo) = New List(Of LogEntryFormatInfo)
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList) 'Added By Utkarsh On 19-Apr-2011
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AOnDate = Session("AOnDate")
        mLogEntryFormat = Session("mLogEntryFormat")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptLogEntryFormat_Ajax.aspx?" Then
            Session.Remove("mMachineNameValueList") 'Added By Utkarsh On 19-Apr-2011
            Session.Remove("mAssemblylist")
            Session.Remove("mLogEntryFormat")
        End If
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        upnlDetails.Update()
    End Sub
    Private Sub Display()
        'lblAircraft1.Visible = True
        'lblAssembly1.Visible = True
        'lblDateRangeFrom.Visible = True
        'lblDateRangeTo.Visible = True
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.ToString
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.ToString
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If cmbAircraft.SelectedIndex > 0 Then
            AssemblyText = IIf(cmbAircraftAssembly.SelectedIndex > -1, cmbAircraftAssembly.SelectedItem.Text, "")
            MachineID = cmbAircraft.SelectedValue.ToString
            AssemblyID = cmbAircraftAssembly.SelectedValue.ToString
            AssemblyType = mAssemblylist(cmbAircraftAssembly.SelectedIndex).AssemblyType
            SerialNo = mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo
            Model = mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName
            RegNo = mMachineNameValueList(cmbAircraft.SelectedIndex).RegNo  'Added By Utkarsh On 19-Apr-2011
            AssemblyTypeID = mAssemblylist(cmbAircraftAssembly.SelectedIndex).AssemblyTypeID
        Else
            AssemblyText = ""
        End If


        'lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        'lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        'lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        'lblAssembly1.Text = "Assembly : " & IIf(AssemblyText <> "", AssemblyText, "")
        Dim Ref_Doc As String = "Ref. Doc. : "
        Ref_Doc = IIf(chkLogNo.Checked, Ref_Doc + "Log No.", Ref_Doc)
        Ref_Doc = IIf(chkLogPageNo.Checked, Ref_Doc + ", Log Page No.", Ref_Doc)
        Ref_Doc = IIf(chkFlightNo.Checked, Ref_Doc + ", Flight No.", Ref_Doc)
        mLogBookSearchingCriteria = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "") + ", " + "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "") + ", " + "Aircraft : " & IIf(Aircraft <> "", Aircraft, "") + ", " + "Assembly : " & IIf(AssemblyText <> "", AssemblyText, "") + ", " + Ref_Doc
    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.ToString
        EndDate = txtToDate.Text.ToString
        MachineID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyType = ""
        Aircraft = ""
        AssemblyText = ""
        AssemblyTypeID = 1
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, ByVal ByMail As Boolean, Optional ByVal IsView As Boolean = False)
        Session("IsExcel") = IsExcel
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        mLogEntryFormat = New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        SetValues()
        Dim str1 As String = ""
        If chkLogNo.Checked Then
            str1 = "Log No."
        Else
            str1 = ""
        End If
        If chkLogPageNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Log Page No."
        Else
            str1 = str1 + "/" + "Log Page No."
        End If

        If chkFlightNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Flight No."
        Else
            str1 = str1 + "/" + "Flight No."
        End If
        If chkFlightLogClassifications.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Classification"
        Else
            str1 = str1 + "/" + "Classification"
        End If
        If AppSettings("ClientCode") = "STR" Then
            RptCommonHistory = New crptLogEntryFormatSTR
        Else
            RptCommonHistory = New crptLogEntryFormat
        End If

        mLogEntryFormat = LogEntryFormat.GetHistoryList(StartDate, EndDate, "", AssemblyType, Model, SerialNo, "", "", "", "", MachineID, True, True, _
                          chkShowInstRem.Checked, chkShowInstRem.Checked, False, AssemblyID:=AssemblyID, IsLogNo:=chkLogNo.Checked, IsLogPageNo:=chkLogPageNo.Checked, IsFlightNo:=chkFlightNo.Checked, IsMELRequired:=chkShowPirepsMELSnag.Checked, IsMaintenanceActivityRequired:=chkShowMaintActivity.Checked, AssemblyTypeID:=AssemblyTypeID, ShowService:=chkShowService.Checked, ShowInsp:=chkShowInsp.Checked, ShowDir:=chkShowDir.Checked)

        'Added By Prashant 7-Apr-2019
        Dim checkString
        If ByMail Then
            checkString = Session("checkString")
            Session.Remove("checkString")
        Else
            checkString = Request.Form("chkSelectList")
        End If

        If Not checkString Is Nothing And gdPartSearch.Rows.Count > 0 Then
            Dim values = checkString.Split(","c)
            Dim mEntryFormat As New LogEntryFormatInfo
            For Each value As String In values
                mEntryFormat = mLogEntryFormat(New Guid(value))
                mLogEntList.Add(mEntryFormat)
            Next
        End If
        'End of Added By Prashant 7-Apr-2019

        Session("mLogEntryFormat") = mLogEntryFormat

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "LOG BOOK ENTRY", str1, New SmartDate(EndDate).FormattedText, cmbAircraft.SelectedItem.ToString, _
            cmbAircraftAssembly.SelectedItem.ToString, IIf(AssemblyType.Equals("Airframe"), "AIRCRAFT", AssemblyType.ToUpper), _
            AppSettings("Product Version"), AppSettings("SINote"), txtMaintenanceCarriedOut.Text.Trim, chkPrintthisline.Checked, _
            New SmartDate(StartDate).FormattedText, Trim(txtBottomLine.Text), AppSettings("Logo"))

        If mLogEntryFormat.Count = 0 Then
            If (ByMail = True) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "LOG BOOK ENTRY", "LOG BOOK ENTRY", "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("LogEntryFormat").SmtpHost, SmtpPort:=mModuleList.Item("LogEntryFormat").SmtpPort, SmtpUser:=mModuleList.Item("LogEntryFormat").SmtpUser, SmtpPassword:=mModuleList.Item("LogEntryFormat").SmtpPassword)

                Exit Sub
            End If
            If IsView = True Then
                gdPartSearch.DataSource = mLogEntryFormat
                gdPartSearch.DataBind()
                upnlView.Update()
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011
        ElseIf mLogEntryFormat.Count > 0 And Not IsExcel And Not ByMail Then
            RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1415)
            'End
            MarkLog(Util.Action.Print, "LogEntryFormat", mLogBookSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        If IsView = True Then
            gdPartSearch.DataSource = mLogEntryFormat
            gdPartSearch.DataBind()
            upnlView.Update()
            Exit Sub
        End If
        If IsExcel = False Then 'If PDF format
            ds.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            If checkString Is Nothing Then
                da.Fill(ds, "LogEntryFormat", mLogEntryFormat)      'This is direct from object records 
            Else
                da.Fill(ds, "LogEntryFormat", mLogEntList)          'This is when select from Grid view
            End If
            'da.Fill(ds, mLogEntryFormat)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            RptCommonHistory.SetDataSource(ds)
            Session("CrystalReport") = RptCommonHistory
            If ByMail = True Then  'By Mail format
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, IIf(AssemblyType.Equals("Airframe"), "AIRCRAFT", AssemblyType.ToUpper) + " LOGBOOK ENTRY", _
                                         IIf(AssemblyType.Equals("Airframe"), "AIRCRAFT", AssemblyType.ToUpper) + " LOGBOOK ENTRY", " For " + "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "") + " " + "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "") + ", " + "Aircraft : " & IIf(Aircraft <> "", Aircraft, "") + ", " + "Assembly : " & IIf(AssemblyText <> "", AssemblyText, ""), , _
                                         Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                         ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("LogEntryFormat").SmtpHost, SmtpPort:=mModuleList.Item("LogEntryFormat").SmtpPort, SmtpUser:=mModuleList.Item("LogEntryFormat").SmtpUser, SmtpPassword:=mModuleList.Item("LogEntryFormat").SmtpPassword)

            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            End If
            ResetValues()
        ElseIf IsExcel = True Then  'Excel format
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "ExceltmpHistoryList", mLogEntryFormat)

            Dim columnToRemove As String()

            columnToRemove = {"mTSIValueFormatted", "mTSOValueFormatted", "ATANomenclature", "PeriodID", "DoneOnDateFormatted", "DoneOnDate", "Type1", "ID", _
                                    "ParentValue", "ChildValue", "Date", "ATACode", "AssignedManHours", "RequiredManHours", _
                                    "TSOValue", "TSIValue", "TSOOfHours", "TSOOfLanding", "TSOOfDate", "TSOOfCycle", "TSIOfHours", "TSIOfLanding", "TSIOfDate", "TSIOfCycle", "Description", "TSIValueFormatted", "TSOValueFormatted", "ChildValueFormatted", "ParentValueFormatted", "LogID"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExceltmpHistoryList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExceltmpHistoryList").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("ExceltmpHistoryList").Columns.Contains("DateFormatted") Then
                ds.Tables("ExceltmpHistoryList").Columns("DateFormatted").ColumnName = "Date"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("DescriptionForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("DescriptionForExcel").ColumnName = "Description"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("LogPageNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("LogPageNo").ColumnName = "Log Page No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("WorkOrderNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("WorkOrderNo").ColumnName = "Work Order No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("RegNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("RegNo").ColumnName = "Aircraft Reg / Tail number"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("FromOrToOrOnModel") Then
                ds.Tables("ExceltmpHistoryList").Columns("FromOrToOrOnModel").ColumnName = "Assembly"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("AssemblySerialNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("AssemblySerialNo").ColumnName = "Assembly Serial No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("Type") Then
                ds.Tables("ExceltmpHistoryList").Columns("Type").ColumnName = "Assembly Type"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("OfModelOrPart") Then
                ds.Tables("ExceltmpHistoryList").Columns("OfModelOrPart").ColumnName = "Model/Part"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("SerialNo") Then
                ds.Tables("ExceltmpHistoryList").Columns("SerialNo").ColumnName = "Installation/Removal/Compliance On/of Serial No."
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("TSOValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("TSOValueFormattedForExcel").ColumnName = "TSO"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("TSIValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("TSIValueFormattedForExcel").ColumnName = "TSI"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("HistoryType") Then
                ds.Tables("ExceltmpHistoryList").Columns("HistoryType").ColumnName = "Maint.Activity"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("ChildValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("ChildValueFormattedForExcel").ColumnName = "TSN"
            End If
            If ds.Tables("ExceltmpHistoryList").Columns.Contains("ParentValueFormattedForExcel") Then
                ds.Tables("ExceltmpHistoryList").Columns("ParentValueFormattedForExcel").ColumnName = "Parent Value"
            End If

            Dim columnToRemove2 As String() = {"ID", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "SearchStr3", "ProductVersion", "SINote", "ReportDate", "SearchStr6", "SearchStr10", "ShortName", "SearchStr12", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Installation To/Removal From/Compliance On Model No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Installation To/Removal From/Compliance On Serial No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr7") Then
                ds.Tables("ReportData").Columns("SearchStr7").ColumnName = "Installation/Removal/Compliance On/of Model No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
                ds.Tables("ReportData").Columns("SearchStr8").ColumnName = "Installation/Removal/Compliance On/of Serial No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr9") Then
                ds.Tables("ReportData").Columns("SearchStr9").ColumnName = "Installation/Removal/Compliance On/of Part"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr11") Then
                ds.Tables("ReportData").Columns("SearchStr11").ColumnName = "Installation/Removal/Compliance On/of Comp Serial No."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ExceltmpHistoryList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"

            If ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Or (AppSettings("ClientCode") = "YA") Or (AppSettings("ClientCode") = "TA")) Then
                dsNew.Tables("ExceltmpHistoryList").TableName = "Technical Department"
            Else
                dsNew.Tables("ExceltmpHistoryList").TableName = "Common History Register"
            End If
			Session("ExcelFileName") = dsNew.Tables("ExceltmpHistoryList").TableName
			Session("dsNew") = dsNew
			ResetValues()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "LogEntryFormat", "Export To excel " + mLogBookSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
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
    Private Sub ControlVisibility()
        If mLogEntryFormat.Count > 10 Then
            btnByMail.Visible = True
            btnView.Visible = True
            btnDisplay.Visible = True
            btnExportToWord.Visible = True
            btnClose.Visible = True
        Else
            btnByMail.Visible = False
            btnView.Visible = False
            btnDisplay.Visible = False
            btnExportToWord.Visible = False
            btnClose.Visible = False
        End If
        upnlButtons.Update()
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Session("MiddleFrame") = "wfrptLogEntryFormat_Ajax.aspx?"
            ResetValues()
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            Session("AOnDate") = AOnDate
            SetComboOfMachine(AOnDate)
            DataBind()
        End If
    End Sub
    'Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
    '    Display()
    '    SetValues()
    '    upnlCriteria.Update()
    'End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click, btnTopDisplay.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If IsValid Then
            If (chkShowService.Checked = False And chkShowInsp.Checked = False And chkShowDir.Checked = False And chkShowMaintActivity.Checked = False And chkShowPirepsMELSnag.Checked = False And chkShowInstRem.Checked = False) Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one Activity", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetReport(False, False)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnTopClose.Click
        mMachineNameValueList = Nothing 'Added By Utkarsh On 19-Apr-2011
        mAssemblylist = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAircraftAssembly.Enabled = False
        Else
            lblAssembly.Enabled = True
            cmbAircraftAssembly.Enabled = True
            MachineName = cmbAircraft.SelectedValue.ToString

            'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtFromDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , ).Item(0), MachineInfo).AssemblyStatusList
            'cmbAircraftAssembly.DataSource = mAssemblyStatusList
            'Session("mAssemblyStatusList") = mAssemblyStatusList
            'cmbAircraftAssembly.DataBind()

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, , True)
            Session("mAssemblyList") = mAssemblylist
            cmbAircraftAssembly.DataSource = mAssemblylist
            cmbAircraftAssembly.DataBind()

        End If
        upnlDetails.Update()
        If cmbAircraft.Enabled = True Then
            cmbAircraft.Focus()
        End If
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text.Trim
        If AOnDate = AOdate Then
        Else
            'If Date.TryParse(txtFromDate.Text.Trim, tmpdate) Then
            SetComboOfMachine(AOdate)
            lblAssembly.Enabled = False
            cmbAircraftAssembly.Enabled = False
            mAssemblylist = Nothing
            Session("mAssemblyList") = mAssemblylist
            cmbAircraftAssembly.ClearSelection()
            cmbAircraftAssembly.DataSource = mAssemblylist
            cmbAircraftAssembly.Controls.Clear()
            cmbAircraftAssembly.DataBind()
            upnlDetails.Update()
        End If
        upnlDate.Update()
        upnlDetails.Update()
    End Sub
    'Added by Shital on 6-Sep-2016
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click, btnTopReportByMail.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If IsValid = True Then
            If (chkShowService.Checked = False And chkShowInsp.Checked = False And chkShowDir.Checked = False And chkShowMaintActivity.Checked = False And chkShowPirepsMELSnag.Checked = False And chkShowInstRem.Checked = False) Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one Activity", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

            Session("UserEmailID") = mModuleList.Item("LogEntryFormat").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("LogEntryFormat").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            Dim checkString = Request.Form("chkSelectList")
            Session("checkString") = checkString
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        End If
    End Sub
    Private Sub hdnimgLogBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgLogBtnSendMail.Click
        Dim email As Thread
        Try
            email = New Thread(Sub() SetReport(False, True))
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgMELBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnExportToWord_Click(sender As Object, e As System.EventArgs) Handles btnExportToWord.Click, btnTopInWord.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If IsValid Then
            If (chkShowService.Checked = False And chkShowInsp.Checked = False And chkShowDir.Checked = False And chkShowMaintActivity.Checked = False And chkShowPirepsMELSnag.Checked = False And chkShowInstRem.Checked = False) Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one Activity", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        Dim checkString = Request.Form("chkSelectList")
        If checkString Is Nothing And gdPartSearch.Rows.Count > 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, " Record", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            If gdPartSearch.Rows.Count > 0 Then
                Dim values = checkString.Split(","c)
                Dim mEntryFormat As New LogEntryFormatInfo
                For Each value As String In values
                    mEntryFormat = mLogEntryFormat(New Guid(value))
                    mLogEntList.Add(mEntryFormat)
                Next
            End If
        End If
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        SetValues()

        Dim str1 As String = ""
        If chkLogNo.Checked Then
            str1 = "Log No."
        Else
            str1 = ""
        End If
        If chkLogPageNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Log Page No."
        Else
            str1 = str1 + "/" + "Log Page No."
        End If

        If chkFlightNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Flight No."
        Else
            str1 = str1 + "/" + "Flight No."
        End If
        If chkFlightLogClassifications.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Classification"
        Else
            str1 = str1 + "/" + "Classification"
        End If

        If checkString Is Nothing And gdPartSearch.Rows.Count = 0 Then
            mLogEntryFormat = LogEntryFormat.GetHistoryList(StartDate, EndDate, "", AssemblyType, Model, SerialNo, "", "", "", "", MachineID, True, True, _
                         chkShowInstRem.Checked, chkShowInstRem.Checked, False, AssemblyID:=AssemblyID, IsLogNo:=chkLogNo.Checked, IsLogPageNo:=chkLogPageNo.Checked, IsFlightNo:=chkFlightNo.Checked, IsMELRequired:=chkShowPirepsMELSnag.Checked, IsMaintenanceActivityRequired:=chkShowMaintActivity.Checked, AssemblyTypeID:=AssemblyTypeID, ShowService:=chkShowService.Checked, ShowInsp:=chkShowInsp.Checked, ShowDir:=chkShowDir.Checked)

            If mLogEntryFormat.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf mLogEntryFormat.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1415)
                MarkLog(Util.Action.Print, "LogEntryFormat", mLogBookSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If

        End If

        'RptCommonHistory = New crptLogEntryForWordFormat
        RptCommonHistory = New crptLogEntryFormat
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "LOG BOOK ENTRY", str1, New SmartDate(EndDate).FormattedText, cmbAircraft.SelectedItem.ToString, _
            cmbAircraftAssembly.SelectedItem.ToString, IIf(AssemblyType.Equals("Airframe"), "AIRCRAFT", AssemblyType.ToUpper), _
            AppSettings("Product Version"), AppSettings("SINote"), txtMaintenanceCarriedOut.Text.Trim, chkPrintthisline.Checked, _
            New SmartDate(StartDate).FormattedText, Trim(txtBottomLine.Text), AppSettings("Logo"))

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        If checkString Is Nothing And gdPartSearch.Rows.Count = 0 Then
            da.Fill(ds, "LogEntryFormat", mLogEntryFormat)      'This is direct from object records 
        Else
            da.Fill(ds, "LogEntryFormat", mLogEntList)          'This is when select from Grid view
        End If
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        RptCommonHistory.SetDataSource(ds)
        Session("CrystalReport") = RptCommonHistory
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openWordFile", "openWordFile();", True)

    End Sub
    Private Sub btnView_Click(sender As Object, e As System.EventArgs) Handles btnView.Click, btnTopView.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If IsValid Then
            If (chkShowService.Checked = False And chkShowInsp.Checked = False And chkShowDir.Checked = False And chkShowMaintActivity.Checked = False And chkShowPirepsMELSnag.Checked = False And chkShowInstRem.Checked = False) Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one Activity", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        SetReport(IsExcel:=False, ByMail:=False, IsView:=True)
        ControlVisibility()
    End Sub
    Private Sub gdPartSearch_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdPartSearch.PageIndexChanging
        gdPartSearch.PageIndex = e.NewPageIndex
        gdPartSearch.SelectedIndex = -1
        gdPartSearch.EditIndex = -1
        gdPartSearch.DataSource = mLogEntryFormat
        gdPartSearch.DataBind()
        upnlView.Update()
    End Sub
#End Region


End Class