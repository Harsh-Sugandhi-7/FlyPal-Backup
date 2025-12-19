

'Createdf By     :   Saylee
'Dated           :   19-May-2016


Public Class wfrptTLPRegister
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mrptTLPRegister As rptTLPRegister
    Private mMachineNameValueList As MachineNameValueList
    Dim DateIndex, FromDate, ToDate As String
    Dim AOnDate, AOdate As String
    Dim mIsExcel As Boolean
    Dim EventLogID As Guid
    Private mIsPreview As Boolean = False
    Public mFlightLogClassificationList As FlightLogClassificationList
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Public EventLogDetails As String = String.Empty
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mrptTLPRegister = Session("mrptTLPRegister")
        mMachineNameValueList = Session("mMachineNameValueList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mrptTLPRegister") = mrptTLPRegister
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mrptTLPRegister")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblAircraft1.Visible = True
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptTLPRegister.aspx?" Then
            Session.Remove("mrptTLPRegister")
            Session.Remove("mMachineNameValueList")
        End If
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-1-1900")
                txtToDate.Text = CDate("1-1-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
    End Sub
    Public Sub setValues()
        Dim mAircraft As String = ""
        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText

        If cmbAircraft.SelectedIndex > 0 Then
            mAircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft : " & mAircraft
        Else
            mAircraft = ""
            lblAircraft1.Text = "Aircraft : "
        End If
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal ByExcel As Boolean = False)
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsTLPRegister As New dsTLPRegister


        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String = String.Empty
        Dim SearchStr4 As String = String.Empty
        Dim SearchStr5 As String = String.Empty

        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        SearchStr1 = New SmartDate(FromDate).FormattedText
        SearchStr2 = New SmartDate(ToDate).FormattedText

        If cmbAircraft.SelectedIndex > 0 Then
            SearchStr3 = cmbAircraft.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        If cmbFlightLogClassification.SelectedIndex > 0 Then
            SearchStr4 = cmbFlightLogClassification.SelectedItem.Text
        End If

        If cmbFormat.SelectedIndex = 0 Then
            If AppSettings("ClientCode") = "Novo" Then
                myReport = New crptTLPRegisterNOVOWithIsPFIChanges
            Else
                myReport = New crptTLPRegister
            End If
        Else
            If rdbICAO.Checked Then
                SearchStr5 = "True"
            Else
                SearchStr5 = "False"
            End If

            myReport = New crptTLPFuelRegister
        End If



        EventLogDetails = "Date Range: " + txtFromDate.Text.Trim + ", " + txtToDate.Text.Trim + ", " + "Aircraft : " + SearchStr3 + ", " + "Classification : " & SearchStr4
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
             mCompanyDetail.WebSite, "", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        mrptTLPRegister = rptTLPRegister.GetTLPRegister(txtFromDate.Text.ToString, txtToDate.Text.ToString, cmbAircraft.SelectedValue.ToString, ByExcel, cmbFlightLogClassification.SelectedValue.ToString)

        If mrptTLPRegister.Count <= 1 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAuditFindings.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(dsTLPRegister)
        '----------------------------------------------------------
        da.Fill(dsTLPRegister, mrptTLPRegister)
        da.Fill(dsTLPRegister, Report)
        da.Fill(dsTLPRegister, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(dsTLPRegister)
        Session("CrystalReport") = myReport

        If ByMail Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "TLP Register", "TLP Register", _
                                      "From Date : " + SearchStr1 + ", " + "To Date : " + SearchStr2 + ", " + "Aircraft : " & IIf(SearchStr3 <> "", SearchStr3, "All"), _
                                      "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                      ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=mModuleList.Item("TLPRegister").SmtpHost, SmtpPort:=mModuleList.Item("TLPRegister").SmtpPort, _
                                      SmtpUser:=mModuleList.Item("TLPRegister").SmtpUser, SmtpPassword:=mModuleList.Item("TLPRegister").SmtpPassword)
        ElseIf ByExcel Then
            SetExcel(mrptTLPRegister, Report, "Aircraft Utilization")
        Else
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "TLPRegister", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            ResetValues()
        End If

    End Sub
    Private Sub SetExcel(mrptTLPRegister As rptTLPRegister, SearchingCriteria As ReportData, ReportName As String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsTLPRegister As New dsTLPRegister
        da.Fill(dsTLPRegister, "rptTLPRegister", mrptTLPRegister)
        da.Fill(dsTLPRegister, "ReportData", SearchingCriteria)

        Dim columnToRemoveUTC As String()
        Dim columnToRemove As String()
        If AppSettings("ClientCode") = "Novo" Then
            columnToRemove = {
                                                 "ID",
                                                 "IsPFIDone",
                                                 "PFIDoneByName",
                                                 "PFIDoneByNo",
                                                 "LogID",
                                                 "GroupBy",
                                                 "Heading",
                                                 "IsUTC",
                                                 "HourType",
                                                 "IsLogPageNo",
                                                 "IsLogNo",
                                                 "IsFlightNo",
                                                 "ReferencedDocuments",
                                                 "ReferencedDocumentsHeading",
                                                 "DepartureFrom",
                                                 "ArrivalTo",
                                                 "DepartureTime",
                                                 "ArrivalTime",
                                                 "LogPageNoFormatted",
                                                 "LogDetailID",
                                                 "DepartureUTCTime",
                                                 "ArrivalUTCTime",
                                                 "FlyingHours",
                                                 "FlightNo",
                                                 "LogDetailTimeOnGround",
                                                 "SrNo", "IsTLP", "PFIDoneByID",
                                                 "DepartureFromICAO", "DepartureFromCode",
                                                 "ArrivalToICAO", "ArrivalToCode",
                                                 "ChocksOffLocalTimeONLY", "ChocksOffUTCTimeONLY",
                                                 "PassengersOnboard", "Owner", "FuelAdded"
                                        }
        Else
            columnToRemove = {
                                                             "ID",
                                                             "IsPFIDone",
                                                             "PFIDoneByEmpNoName",
                                                             "PFIDoneStatus",
                                                             "PFIDoneByName",
                                                             "PFIDoneByNo",
                                                             "LogID",
                                                             "GroupBy",
                                                             "Heading",
                                                             "IsUTC",
                                                             "HourType",
                                                             "IsLogPageNo",
                                                             "IsLogNo",
                                                             "IsFlightNo",
                                                             "ReferencedDocuments",
                                                             "ReferencedDocumentsHeading",
                                                             "DepartureFrom",
                                                             "ArrivalTo",
                                                             "DepartureTime",
                                                             "ArrivalTime",
                                                             "LogPageNoFormatted",
                                                             "LogDetailID",
                                                             "DepartureUTCTime",
                                                             "ArrivalUTCTime",
                                                             "FlyingHours",
                                                             "FlightNo",
                                                             "LogDetailTimeOnGround",
                                                             "SrNo", "IsTLP", "PFIDoneByID",
                                                             "DepartureFromICAO", "DepartureFromCode",
                                                             "ArrivalToICAO", "ArrivalToCode",
                                                             "ChocksOffLocalTimeONLY", "ChocksOffUTCTimeONLY",
                                                             "PassengersOnboard", "Owner", "FuelAdded"
                                                    }
        End If



        For i As Integer = 0 To columnToRemove.Length - 1
            If dsTLPRegister.Tables("rptTLPRegister").Columns.Contains(columnToRemove(i)) Then
                dsTLPRegister.Tables("rptTLPRegister").Columns.Remove(columnToRemove(i))
            End If
        Next
        If mrptTLPRegister.Count > 0 Then
            If mrptTLPRegister.Item(0).IsUTC Then
                columnToRemoveUTC = {"TakeOffTime", "TouchDownTime", "ChocksOffTime", "ChocksOnTime"}

                dsTLPRegister.Tables("rptTLPRegister").Columns("ChocksOffUTCTime").SetOrdinal(10)
                dsTLPRegister.Tables("rptTLPRegister").Columns("ChocksOnUTCTime").SetOrdinal(11)

                dsTLPRegister.Tables("rptTLPRegister").Columns("TakeOffUTCTime").SetOrdinal(13)
                dsTLPRegister.Tables("rptTLPRegister").Columns("TouchDownUTCTime").SetOrdinal(14)

            Else
                columnToRemoveUTC = {"TakeOffUTCTime", "TouchDownUTCTime", "ChocksOffUTCTime", "ChocksOnUTCTime"}

                dsTLPRegister.Tables("rptTLPRegister").Columns("ChocksOffTime").SetOrdinal(10)
                dsTLPRegister.Tables("rptTLPRegister").Columns("ChocksOnTime").SetOrdinal(11)

                dsTLPRegister.Tables("rptTLPRegister").Columns("TakeOffTime").SetOrdinal(13)
                dsTLPRegister.Tables("rptTLPRegister").Columns("TouchDownTime").SetOrdinal(14)


            End If
            For i As Integer = 0 To columnToRemoveUTC.Length - 1
                If dsTLPRegister.Tables("rptTLPRegister").Columns.Contains(columnToRemoveUTC(i)) Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns.Remove(columnToRemoveUTC(i))
                End If
            Next
        End If

        If cmbFormat.SelectedIndex = 1 Then
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDetailFlightNo").SetOrdinal(0)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDate").SetOrdinal(1)
            dsTLPRegister.Tables("rptTLPRegister").Columns("ChocksOffUTCTime").SetOrdinal(2)
            dsTLPRegister.Tables("rptTLPRegister").Columns("TakeOffUTCTime").SetOrdinal(3)
            dsTLPRegister.Tables("rptTLPRegister").Columns("TouchDownUTCTime").SetOrdinal(4)
            dsTLPRegister.Tables("rptTLPRegister").Columns("ChocksOnUTCTime").SetOrdinal(5)
            dsTLPRegister.Tables("rptTLPRegister").Columns("Source").SetOrdinal(6)
            dsTLPRegister.Tables("rptTLPRegister").Columns("Destination").SetOrdinal(7)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDetailBlockTime").SetOrdinal(8)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDetailTimeInAir").SetOrdinal(9)
            dsTLPRegister.Tables("rptTLPRegister").Columns("Landings").SetOrdinal(10)



            Dim columnToRemoveFormat2 As String()
            columnToRemoveFormat2 = {"TotalBlock", "TotalTimeInAir", "LogNo", "TotalTimeOnGround", "LogPageNo", "Pilot", "CoPilot", "CrewNames", "SrNoStr"}
            For i As Integer = 0 To columnToRemoveFormat2.Length - 1
                If dsTLPRegister.Tables("rptTLPRegister").Columns.Contains(columnToRemoveFormat2(i)) Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns.Remove(columnToRemoveFormat2(i))
                End If
            Next
        Else
            'set Column Sequence
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDate").SetOrdinal(0)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogNo").SetOrdinal(1)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogPageNo").SetOrdinal(2)
            dsTLPRegister.Tables("rptTLPRegister").Columns("TotalTimeInAir").SetOrdinal(3)
            dsTLPRegister.Tables("rptTLPRegister").Columns("TotalBlock").SetOrdinal(4)
            dsTLPRegister.Tables("rptTLPRegister").Columns("TotalTimeOnGround").SetOrdinal(5)
            dsTLPRegister.Tables("rptTLPRegister").Columns("SrNoStr").SetOrdinal(6)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDetailFlightNo").SetOrdinal(7)

            dsTLPRegister.Tables("rptTLPRegister").Columns("Source").SetOrdinal(8)
            dsTLPRegister.Tables("rptTLPRegister").Columns("Destination").SetOrdinal(9)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDetailBlockTime").SetOrdinal(12)
            dsTLPRegister.Tables("rptTLPRegister").Columns("LogDetailTimeInAir").SetOrdinal(15)
            dsTLPRegister.Tables("rptTLPRegister").Columns("Landings").SetOrdinal(16)
            If AppSettings("ClientCode") = "Novo" Then
                dsTLPRegister.Tables("rptTLPRegister").Columns("PFIDoneStatus").SetOrdinal(17)
                dsTLPRegister.Tables("rptTLPRegister").Columns("PFIDoneByEmpNoName").SetOrdinal(18)
            End If
        End If



        For i As Integer = 0 To dsTLPRegister.Tables("rptTLPRegister").Columns.Count - 1

            If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDetailTimeOnGround" Then
                dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Ground Time"
            End If

            If cmbFormat.SelectedIndex = 1 Then
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDate" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Date"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "ChocksOffUTCTime" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "UTC Chocks Off"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "TakeOffUTCTime" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "UTC Take Off"
                End If

                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "TouchDownUTCTime" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "UTC Touch Down"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "ChocksOnUTCTime" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "UTC Chocks On"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Source" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "From"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Destination" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "To"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDetailBlockTime" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Block Hour"
                End If

                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDetailTimeInAir" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Flight Hour"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Landings" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Flight Cycle"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDetailFlightNo" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Flight Number"
                End If
            Else
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "SrNoStr" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Sr. No."
                End If

                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDetailBlockTime" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Block Time"
                End If

                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDetailTimeInAir" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Time In Air"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "LogDetailFlightNo" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "Flight No."
                End If
            End If

            If AppSettings("ClientCode") = "Novo" Then
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "PFIDoneStatus" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "PFI Done Status"
                End If
                If dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "PFIDoneByEmpNoName" Then
                    dsTLPRegister.Tables("rptTLPRegister").Columns(i).ColumnName = "PFI Done By"
                End If
            End If
        Next

        Dim columnToRemoveCriteria As String() = {
                                              "ReportDate",
                                              "ID",
                                              "CompanyName",
                                              "Address",
                                              "Tel1",
                                              "Tel2",
                                              "Fax",
                                              "Email",
                                              "WebSite",
                                              "ReportName",
                                              "SearchStr5",
                                              "SearchStr6",
                                              "SearchStr7",
                                              "SearchStr8",
                                              "SearchStr9",
                                              "ProductVersion",
                                              "SINote",
                                              "CurrencyName",
                                              "CurrencySymbol",
                                              "SearchStr10",
                                              "SearchStr11",
                                              "SearchStr12",
                                              "SearchStr13",
                                              "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20",
                                              "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"
                                           }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If dsTLPRegister.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                dsTLPRegister.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        For i As Integer = 0 To dsTLPRegister.Tables("ReportData").Columns.Count - 1
            If dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "From Date"
            End If
            If dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "To Date"
            End If
            If dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
            If dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                dsTLPRegister.Tables("ReportData").Columns(i).ColumnName = "Classification"
            End If
        Next

        Dim columnscnt As Integer = dsTLPRegister.Tables("rptTLPRegister").Columns.Count

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsTLPRegister.Tables("ReportData"))
        dsNew.Merge(dsTLPRegister.Tables("rptTLPRegister"))

        dsNew.Tables("ReportData").TableName = "Searching Criteria"

        If cmbFormat.SelectedIndex = 1 Then
            dsNew.Tables("rptTLPRegister").TableName = "Aircraft Utilization"
            Session("DataTableToBeFormattedForExportToExcel") = "Aircraft Utilization"
        Else
            dsNew.Tables("rptTLPRegister").TableName = "TLP Register"
            Session("DataTableToBeFormattedForExportToExcel") = "TLP Register"
        End If
		Session("ExcelFileName") = "TLP Register"

		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "TLPRegister", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Bindings "
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        upnlSearchCriteria.Update()
    End Sub
    Public Sub DatafieldBind()
        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(ALL)")
        cmbFlightLogClassification.DataSource = mFlightLogClassificationList
        cmbFlightLogClassification.DataBind()
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
    End Sub
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

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then

            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            DatafieldBind()
            ResetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        mrptTLPRegister = rptTLPRegister.GetTLPRegister(txtFromDate.Text.ToString, txtToDate.Text.ToString, cmbAircraft.SelectedValue.ToString, False, cmbFlightLogClassification.SelectedValue.ToString)

        If mrptTLPRegister.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAuditFindings.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else


            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

            Session("UserEmailID") = mModuleList.Item("TLPRegister").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("TLPRegister").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        End If
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Dim email As Thread
        Try
            email = New Thread(Sub() SetReport(True))
            mIsPreview = False
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
    Private Sub btnByExcel_Click(sender As Object, e As System.EventArgs) Handles btnByExcel.Click

        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        If IsValid = True Then
            mIsExcel = True
            SetReport(, mIsExcel)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        setValues()
        upnlCurrentCriteria.Update()

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        ResetValues()
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedIndex = 0 Then
            ' btnDisplay.Visible = True
            btnByMail.Visible = True
            phRadiobuttons.Visible = False
        Else
            '  btnDisplay.Visible = False
            btnByMail.Visible = False
            phRadiobuttons.Visible = True
        End If
        upnlButton.Update()
    End Sub
#End Region

End Class