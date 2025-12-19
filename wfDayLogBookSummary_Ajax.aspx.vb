'Added by Utkarsh on 31-Jan-2014

Imports System.Linq
Imports System.Collections.Generic
Imports AjaxControlToolkit
Imports System.Collections.Specialized

Public Class wfDayLogBookSummary_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ReportStatusList As New rptStatusList
    'Dim mMachineNameValueList As MachineNameValueList 'Added By Utkarsh On 19-Apr-2011
    Public Shared mAssemblylist As AssemblyList 'Added By Utkarsh On 19-Apr-2011
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As Guid
    Dim AssemblyID As Guid
    Dim Aircraft As String
    Dim AssemblyType As String
    Dim AssemblyText As String
    Dim Model As String
    Dim SerialNo, SerialNoPosition As String

    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail

    Dim mReportDayLogBookRegister As ReportDayLogBookRegister
    Dim mAssemblyDayLogBookDifferncePeriodList As AssemblyDayLogBookDifferncePeriodList
    Dim dsDayLogBookRegister As New dsDayLogBookRegister

    Dim objEleLogRegister As New ReportHistoryCumLogRegister
    Dim objEleLogDetail As AssemblyLogDifferncePeriodList
    Dim dsEleLogRegister As New dsHistoryCumLogRegister
    Public mFlightLogClassificationList As FlightLogClassificationList
    Dim FlightClassificationName1 As String
    Dim EventLogDetail As String

    Dim mIsExcel As Boolean
    Dim mModuleList As ModuleList    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        ' mMachineList = CType(Session("mMachineList"), MachineList) 'Commented By Utkarsh On 19-Apr-2011
        ' mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)'Commented By Utkarsh On 19-Apr-2011
        'mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList) 'Added By Utkarsh On 19-Apr-2011
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList) 'Added By Utkarsh On 19-Apr-2011
        mFlightLogClassificationList = CType(Session("mFlightLogClassificationList"), FlightLogClassificationList)
        mModuleList = Session("mModuleList")    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDayLogBookSummary_Ajax.aspx?" Then
            Session.Remove("mFlightLogClassificationList")
            Session.Remove("mAssemblylist")
        End If
    End Sub
    Private Sub SetSession()
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblAssembly1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblFlightLogClassification1.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text.Trim) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.Trim
        End If
        If Not IsDate(txtToDate.Text.Trim) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.Trim
        End If
        MachineID = New Guid(cmbAircraft.SelectedValue.ToString)
        Aircraft = IIf(MachineID.Equals(Guid.Empty), "", cmbAircraft.SelectedItem.Text)
        If Not MachineID.Equals(Guid.Empty) Then
            AssemblyID = New Guid(cmbAircraftAssembly.SelectedValue.ToString)
            AssemblyText = IIf(AssemblyID.Equals(Guid.Empty), "", cmbAircraftAssembly.SelectedItem.Text)

            'Changed By Utkarsh On 19-Apr-2011
            AssemblyType = mAssemblylist(AssemblyID).AssemblyType
            '*********************************
        Else
            AssemblyText = ""
        End If
        Dim classificationID As Guid = New Guid(Request.Form("cmbFlightLogClassification").ToString)
        FlightClassificationName1 = IIf(classificationID.Equals(Guid.Empty), "", mFlightLogClassificationList(classificationID).Name)
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblAssembly1.Text = "Assembly : " & IIf(AssemblyText <> "", AssemblyText, "")
        lblFlightLogClassification1.Text = "Flight Log Classification :" & IIf(FlightClassificationName1 <> "", FlightClassificationName1, "")

        EventLogDetail = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + lblFlightLogClassification1.Text

    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.Trim
        EndDate = txtToDate.Text.Trim
        MachineID = Guid.Empty
        AssemblyID = Guid.Empty
        AssemblyType = ""
        Aircraft = ""
        AssemblyText = ""
    End Sub

    'Modified by Harsh Sugandhi on 9th September 2024 FLYPAL-1876 Added TSO column report
    Private Sub SetReport(Optional ByExcel As Boolean = False,
                          Optional ByMail As Boolean = False)

        Dim OperatorName As String = ""
        Dim FlightLogClassification As String = ""

        Try

            SetValues()

            'Added By Utkarsh On 19-Apr-2011

            If mAssemblylist(cmbAircraftAssembly.SelectedIndex).Position <> "" Then
                SerialNoPosition = mAssemblylist(AssemblyID).SerialNo + "(" + mAssemblylist(AssemblyID).Position + ")"
            Else
                SerialNoPosition = mAssemblylist(AssemblyID).SerialNo
            End If
            '*******************************

            myReport = New crDayLogBookRegister

            mAssemblyDayLogBookDifferncePeriodList = AssemblyDayLogBookDifferncePeriodList.GetAssemblyDayLogBookDifferncePeriodList(StartDate,
                                                                                                                                    EndDate,
                                                                                                                                    AssemblyID,
                                                                                                                                    True,
                                                                                                                                    MachineID:=MachineID.ToString)

            mReportDayLogBookRegister = ReportDayLogBookRegister.GetDayLogBookRegister(StartDate,
                                                                                       EndDate,
                                                                                       AssemblyID.ToString,
                                                                                       MachineID.ToString,
                                                                                       True,
                                                                                       FlightClassificationName1)

            'Added By Utkarsh On 19-Apr-2011
            ReportStatusList.Add(New rptStatus(,
                                                   0,
                                                   StartDate + " " + "To" + " " + EndDate,
                                                   AssemblyType + " " + "Details", , ,
                                                   Aircraft, ,
                                                   mAssemblylist(AssemblyID).ModelName,
                                                   SerialNoPosition, , , , , , , , , , , ,
                                                   "Period",
                                                   "Before" + " " + StartDate, ,
                                                   "Total Diff.", ,
                                                   "After" + " " + EndDate))
            '*******************************

            'Added by Saylee on 11-Aug-2011

            If (Not AppSettings("ClientCode") Is Nothing) AndAlso
               (AppSettings("ClientCode") = "Indamer") Then

                If cmbAircraft.SelectedIndex > 0 Then

                    Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(MachineID)
                    If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName

                End If

            End If


            If cmbFlightLogClassification.SelectedIndex > 0 Then FlightLogClassification = cmbFlightLogClassification.SelectedItem.Text

            Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                         mCompanyDetail.Address,
                                         mCompanyDetail.Tel1,
                                         mCompanyDetail.Tel2,
                                         mCompanyDetail.Fax,
                                         mCompanyDetail.Email,
                                         mCompanyDetail.WebSite,
                                         "Day Log Book Register of" + " " + AssemblyType,
                                         txtFromDate.Text,
                                         txtToDate.Text,
                                         cmbAircraft.SelectedItem.Text,
                                         cmbAircraftAssembly.SelectedItem.Text,
                                         FlightClassificationName1,
                                         AppSettings("Product Version"),
                                         AppSettings("SINote"),
                                         OperatorName,
                                         "",
                                         "",
                                         "",
                                         AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.


            If mReportDayLogBookRegister.Count = 0 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "There is no record for this search criteria",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

                'Added By Utkarsh On 7-Jun-2011 For All07062011
            ElseIf mReportDayLogBookRegister.Count > 0 Then

                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1110)

            End If  '*******************************

            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(dsDayLogBookRegister)
            '----------------------------------------------------------
            da.Fill(dsDayLogBookRegister, mAssemblyDayLogBookDifferncePeriodList)
            da.Fill(dsDayLogBookRegister, mReportDayLogBookRegister)
            da.Fill(dsDayLogBookRegister, Report)
            da.Fill(dsDayLogBookRegister, ReportStatusList)
            da.Fill(dsDayLogBookRegister, mrptImage) 'Added by Utkarsh for Report Logo
            myReport.SetDataSource(dsDayLogBookRegister)
            Session("CrystalReport") = myReport

            If ByMail Then

                SendMailFile.SendMailFile(Session("CrystalReport"),
                                          Thread.CurrentPrincipal.Identity.Name,
                                          "Day Log Book Register of" + " " + AssemblyType,
                                          "Day Log Book Register of" + " " + AssemblyType,
                                          lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text,
                                          "", Session("ToSendMailIDs"),
                                          Session("CcSendMailIDs"),
                                          "",
                                          True,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                                          SmtpHost:=mModuleList.Item("DayLogBook").SmtpHost,
                                          SmtpPort:=mModuleList.Item("DayLogBook").SmtpPort,
                                          SmtpUser:=mModuleList.Item("DayLogBook").SmtpUser,
                                          SmtpPassword:=mModuleList.Item("DayLogBook").SmtpPassword)

            ElseIf ByExcel Then

                SetExcel(mReportDayLogBookRegister,
                         Report,
                         "Day Log Book Register of" + " " + AssemblyType)

                MarkLog(Action.Print,
                        "DayLogBook",
                        IIf(ByExcel = True, "Export To excel ", "") + EventLogDetail,
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID) 'Added by Shital on 18-Jan-2021

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openTranDetail",
                                                    "openTranDetail();",
                                                    True)

                MarkLog(Action.Print,
                        "DayLogBook",
                        EventLogDetail,
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID)

            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Private Sub SetExcel(mReportDayLogBookRegister As ReportDayLogBookRegister, SearchingCriteria As ReportData, ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail

       

        da.Fill(ds, "ReportData", SearchingCriteria)
        da.Fill(ds, "ReportDayLogBookRegister", mReportDayLogBookRegister)


        Dim dsNew As New DataSet
        dsNew.Clear()


        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Merge(ds.Tables("ReportDayLogBookRegister"))
     

        dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
        dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
        dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
        dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Assembly"
        dsNew.Tables("ReportData").Columns("SearchStr5").ColumnName = "Flight Log Classification"


        Dim columnToRemove As String()
        'If AppSettings("LogBookTimeEntry") = "UTC" Then
        columnToRemove = {"Type", "LogTypeID", "TakeOffUTCTime", "DepartureUTCTime", "ArrivalUTCTime", "TouchDownUTCTime", "ArrivalTime", "DepartureTime", "TouchDownTime", _
                          "TakeOffTime", "LogID", "AssemblyID", "Col1Label", "Col2Label", "Col3Label", "Col4Label", "ColLabel", "ColDiff", "ColFinal", "PilotName", "CoPilotName", _
                          "Col1Value", "Col2Value", "Col3Value", "Col4Value", "LogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocuments", "ReferencedDocumentsHeading", _
                          "TotalTimeInAir", "Col2DffMonthly", "Remark", "ArrivalLocalUTCTime", "DepartureLocalUTCTime", "RegNo", "DepartureFrom", "ArrivalTo", _
                          "FlightLogClassificationName", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col2DiffInDecimal", "Col2DiffPeriodID", _
                          "Col2DiffPeriodUnitID", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", _
                          "IsLogPageNo", "LogNoLogPageNo", "IntLogNo", "DepartureArrivalPlaceCode", "LogDateFormatted", "LogPageNoFormatted", "EmpNoForPilot", "EmpNoForCoPilot", _
                          "Col1FinalP", "Col1PeriodID", "Col2FinalP", "Col2PeriodID", "Col3FinalP", "Col3PeriodID", "Col4FinalP", "Col4PeriodID", "PeriodID", "DepartureTime", _
                          "ArrivalTime", "LogNo"}
        'Else
        '    columnToRemove = {"Type", "LogTypeID", "TakeOffUTCTime", "DepartureUTCTime", "ArrivalUTCTime", "TouchDownUTCTime", "LogID", "AssemblyID", "Col1Label", "Col2Label", "Col3Label", "Col4Label", "ColLabel", "ColDiff", "ColFinal", "PilotName", "CoPilotName", "Col1Value", "Col2Value", "Col3Value", "Col4Value", "LogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocuments", "ReferencedDocumentsHeading", "TotalTimeInAir", "Col2DffMonthly", "Remark", "ArrivalLocalUTCTime", "DepartureLocalUTCTime", "RegNo", "DepartureFrom", "ArrivalTo", "FlightLogClassificationName", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col2DiffInDecimal", "Col2DiffPeriodID", "Col2DiffPeriodUnitID", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", "IsLogPageNo", "LogNoLogPageNo", "IntLogNo", "DepartureArrivalPlaceCode", "LogDateFormatted", "LogPageNoFormatted", "EmpNoForPilot", "EmpNoForCoPilot"}
        'End If

        For i As Integer = 0 To columnToRemove.Length - 1
            If dsNew.Tables("ReportDayLogBookRegister").Columns.Contains(columnToRemove(i)) Then
                dsNew.Tables("ReportDayLogBookRegister").Columns.Remove(columnToRemove(i))
            End If
        Next

        dsNew.Tables("ReportDayLogBookRegister").Columns("LogDate").SetOrdinal(0)
        dsNew.Tables("ReportDayLogBookRegister").Columns("TimeInAir").SetOrdinal(1)
        dsNew.Tables("ReportDayLogBookRegister").Columns("TimeOnGround").SetOrdinal(2)
        dsNew.Tables("ReportDayLogBookRegister").Columns("BlockTime").SetOrdinal(3)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col1Diff").SetOrdinal(4)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col1Final").SetOrdinal(5)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col2Diff").SetOrdinal(6)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col2Final").SetOrdinal(7)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col3Diff").SetOrdinal(8)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col3Final").SetOrdinal(9)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col4Diff").SetOrdinal(10)
        dsNew.Tables("ReportDayLogBookRegister").Columns("Col4Final").SetOrdinal(11)

        For i As Integer = 0 To dsNew.Tables("ReportDayLogBookRegister").Columns.Count - 1
            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col1Diff" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Total Hours"
            End If
            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col1Final" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Final Hours"
            End If

            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col2Diff" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Total Cycles/Landings"
            End If
            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col2Final" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Final Cycles/Landings"
            End If

            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col3Diff" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Total"
            End If
            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col3Final" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Final"
            End If
            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col4Diff" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Total "
            End If
            If dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Col4Final" Then
                dsNew.Tables("ReportDayLogBookRegister").Columns(i).ColumnName = "Final "
            End If
        Next

        Dim columnToRemove1 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", _
                                            "SearchStr6", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", _
                                           "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ShortName" _
                                          }
        For i As Integer = 0 To columnToRemove1.Length - 1
            If dsNew.Tables("ReportData").Columns.Contains(columnToRemove1(i)) Then
                dsNew.Tables("ReportData").Columns.Remove(columnToRemove1(i))
            End If
        Next

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("ReportDayLogBookRegister").TableName = "Day Log Book of Airframe"


        ' PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "Since New", "Elapsed", "Remaining", "Due At", "Done At", "Effective From", "AssemblySerialNo", "Maintenance On", ColumnName, "Extension", "Maintenance Info"})
        ' Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)

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
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    'Response.Redirect("wfDayLogBookSummary.aspx?")
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfDayLogBookSummary.aspx?")
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)")
        'cmbAircraft.DataSource = mMachineNameValueList
        'Session("mMachineNameValueList") = mMachineNameValueList
        ''************************************
        'cmbAircraft.DataBind()

        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(SELECT)")
        cmbFlightLogClassification.DataSource = mFlightLogClassificationList
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        cmbFlightLogClassification.DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfDayLogBookSummary_Ajax.aspx?"
            ResetValues()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        mIsExcel = True
        If IsValid = True Then
            SetReport(mIsExcel)
        End If
    End Sub
    Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        If IsValid = True Then

            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            '   Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
            Session("UserEmailID") = mModuleList.Item("DayLogBook").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("DayLogBook").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        End If
    End Sub
    Private Sub hdnimgLogBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgLogBtnSendMail.Click
        Dim email As Thread
        Try
            email = New Thread(Sub() SetReport(, ByMail:=True))
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
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ' mMachineList = Nothing  'Commented by Utkarsh On 19-Apr-2011
        ' mAssemblyStatusList = Nothing   'Commented by Utkarsh On 19-Apr-2011
        'mMachineNameValueList = Nothing  'Added by Utkarsh On 19-Apr-2011
        mAssemblylist = Nothing 'Added by Utkarsh On 19-Apr-2011
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region "Service Methods"
    'Service method to fetch Aircraft list
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetAircraftList(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim machineList As List(Of CascadingDropDownNameValue) = New List(Of CascadingDropDownNameValue)()
        Dim mMachineNameValueList As MachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        machineList = (From c In mMachineNameValueList
                  Select New CascadingDropDownNameValue(c.RegNo, c.ID.ToString())).ToList
        Return machineList.ToArray
    End Function
    'Service method to fetch Assembly list
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetAssemblyList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As AjaxControlToolkit.CascadingDropDownNameValue()

        Dim kv As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim machineid As Guid

        If (Not kv.ContainsKey("Machine") Or Not Guid.TryParse(kv("Machine"), machineid)) Then
            Return Nothing
        End If

        If machineid.Equals(Guid.Empty) Then
            Return Nothing
        End If

        Dim fromdate As String = IIf(String.IsNullOrEmpty(contextKey), Now.Date.ToString, contextKey)
        Dim asmblylist As List(Of CascadingDropDownNameValue) = New List(Of CascadingDropDownNameValue)()
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, machineid.ToString, fromdate, , True)
        HttpContext.Current.Session("mAssemblylist") = mAssemblylist
        asmblylist = (From c In mAssemblylist
                  Select New CascadingDropDownNameValue(c.ModelSerialNoPostion, c.ID.ToString())).ToList
        Return asmblylist.ToArray
    End Function

#End Region

End Class