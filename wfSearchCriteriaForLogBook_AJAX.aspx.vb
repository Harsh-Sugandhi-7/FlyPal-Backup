'************************************
'CREATED By : Saylee
'Dated      : 23-Jan-2014
'Modified by Harsh Sugandhi on 26th May 2025 for FLYPAL-2443 Flight Log Register change in Excel Report.
'************************************


Public Class wfSearchCriteriaForLogBook_AJAX
    Inherits Page

#Region " Variable Declaration "

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
    Dim FlightClassificationName As String
    Dim FlightClassificationName1 As String
    Dim AssemblyTypeID As Integer
    Dim EventLogID As Guid
    Dim LogBookSearchingCriteria As String = String.Empty
    Dim AsOnDate, AODate As String
    Dim AMPNoStr As String = ""
    Public LogType As Integer

    Dim ReportStatusList As New rptStatusList
    Dim MachineList As MachineList
    Dim MachineNameValueList As MachineNameValueList 'Added By Utkarsh On 19-Apr-2011
    Dim AssemblyList As AssemblyList
    Dim dataAdapter As New ObjectAdapter
    Dim CrystalReport As Engine.ReportClass
    Dim CompanyDetail As New CompanyDetail
    Dim ReportLogRegister As ReportLogRegister
    Dim AssemblyLogDifferencePeriodList As AssemblyLogDifferncePeriodList
    Dim dsLogRegister As New dsLogRegister
    Dim ReportHistoryCumLogRegister As New ReportHistoryCumLogRegister
    Dim ELE_AssemblyLogDifferencePeriodList As AssemblyLogDifferncePeriodList
    Dim dsHistoryCumLogRegister As New dsHistoryCumLogRegister
    Public FlightLogClassificationList As FlightLogClassificationList
    Dim ModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim LastMPDAMPRef As LastMPDAMPRef

#End Region

#Region " Helper Methods "

    Private Sub GetSession()

        MachineNameValueList = CType(Session("MachineNameValueList"), MachineNameValueList) 'Added By Utkarsh On 19-Apr-2011
        AssemblyList = CType(Session("AssemblyList"), AssemblyList)
        FlightLogClassificationList = CType(Session("FlightLogClassificationList"), FlightLogClassificationList)
        LogType = Session("LogType")
        AsOnDate = Session("AsOnDate")
		ModuleList = CType(Session("mModuleList"), ModuleList) 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType

	End Sub

    Private Sub ClearAll()

        LogType = Session("LogType")

        If Session("MiddleFrame") <> "wfSearchCriteriaForLogBook_Ajax.aspx?LogType=" + CStr(LogType) Then
            Session.Remove("MachineNameValueList") 'Added By Utkarsh On 19-Apr-2011
            Session.Remove("AssemblyList")
        End If

    End Sub

    Private Sub SetSession()
        Session("FlightLogClassificationList") = FlightLogClassificationList
    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        Try

            If control.Enabled = False Or control.Visible = False Then Exit Sub
            Dim str As String
            str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
            ClientScript.RegisterStartupScript([GetType], "Focus On Control", str)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Sub SetComboOfMachine(AsOnDate As String)

        Try

            MachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=AsOnDate, , , , , , ,
                                                                       IsTagRequired:=True,
                                                                       TagText:="(SELECT)", ,
                                                                       SkipIsForInventoryAircarft:=True)
            cmbAircraft.DataSource = MachineNameValueList
            Session("MachineNameValueList") = MachineNameValueList
            cmbAircraft.DataBind()
            upnlDetails.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub Display()

        Try

            lblAircraft1.Visible = True
            lblAssembly1.Visible = True
            lblDateRangeFrom.Visible = True
            lblDateRangeTo.Visible = True
            lblFlightLogClassification1.Visible = (LogType = 1)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ControlVisibility()

        Try

            If LogType = 1 Then

                lblSelectReferenceDocument.Visible = IIf(cmbFormat.Visible = True And cmbFormat.SelectedIndex = 1 And LogType = 1, True, False)
                chkLogNo.Visible = IIf(cmbFormat.Visible = True And cmbFormat.SelectedIndex = 1, True, False)
                chkLogPageNo.Visible = IIf(cmbFormat.Visible = True And cmbFormat.SelectedIndex = 1, True, False)
                chkFlightNo.Visible = IIf(cmbFormat.Visible = True And cmbFormat.SelectedIndex = 1, True, False)
                chkRemark.Visible = IIf(cmbFormat.Visible = True And cmbFormat.SelectedIndex = 1, True, False) 'Added By Vikrant on 27-Sep-2012 For ALL26092012
                chkFlightLogClassifications.Visible = IIf(cmbFormat.Visible = True And cmbFormat.SelectedIndex = 1, True, False)
                chkShowCompliance.Visible = False 'Added by Saylee on 2nd Jan-2013
                chkShowInstRem.Visible = False
                chkShowMaintActivity.Visible = False
                chkShowPirepsMELSnag.Visible = False
                lblSelectActivity.Visible = False
                btnExport.Visible = True
                table4.Visible = True
                chkMonthWise.Visible = IIf(cmbFormat.Visible = True And cmbFormat.SelectedIndex = 0, True, False) '--Added By Utkarsh On 15-Feb-2011

            Else

                lblSelectReferenceDocument.Visible = True
                chkLogNo.Visible = True
                chkLogPageNo.Visible = True
                chkFlightNo.Visible = True
                chkShowCompliance.Visible = True 'Added by Saylee on 2nd Jan-2013
                chkShowInstRem.Visible = True
                chkShowMaintActivity.Visible = True
                chkShowPirepsMELSnag.Visible = True
                lblSelectActivity.Visible = True
                chkMonthWise.Visible = False

                If AppSettings("ClientCode") = "BA" Or
                   AppSettings("ClientCode") = "PAS" Or
                   AppSettings("ClientCode") = "Novo" Or
                   AppSettings("ClientCode") = "YA" Or
                   AppSettings("ClientCode") = "TA" Then

                    lblSelectReferenceDocument.Visible = False
                    lblSelectActivity.Text = "Step IV. Selection of Activities"
                    lblDisplayReport.Text = "Step VI. Display Report"
                    chkLogNo.Visible = False
                    chkLogPageNo.Visible = False
                    chkFlightNo.Visible = False

                End If

                btnExport.Visible = False
                table4.Visible = False

            End If

            If (cmbFormat.Visible = True And cmbFormat.SelectedIndex = 1 And LogType = 1) Then
                lblDisplayReport.Text = "Step VIII. Display Report"
            ElseIf (cmbFormat.Visible = True And cmbFormat.SelectedIndex = 0 And LogType = 1) Then
                lblDisplayReport.Text = "Step VII. Display Report"
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetValues()

        Dim Ref_Doc As String = "Ref. Doc. : "
        Try

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
                AssemblyType = AssemblyList(cmbAircraftAssembly.SelectedIndex).AssemblyType
                SerialNo = AssemblyList(cmbAircraftAssembly.SelectedIndex).SerialNo
                Model = AssemblyList(cmbAircraftAssembly.SelectedIndex).ModelName
                RegNo = MachineNameValueList(cmbAircraft.SelectedIndex).RegNo  'Added By Utkarsh On 19-Apr-2011
                AssemblyTypeID = AssemblyList(cmbAircraftAssembly.SelectedIndex).AssemblyTypeID

            Else
                AssemblyText = ""
            End If

            'Added By Prashant 25-Oct-2018 ALL25102018
            FlightClassificationName1 = String.Empty
            FlightClassificationName = String.Empty
            Dim SelectedFlightLogClassificationCount As Integer = 0 'Added By Vikrant On 13-Dec-2021 for Issue: If Classification is not selected in log then Hours total differ on Classification search criteria(ALL Classification)

            For i As Integer = 0 To ChkFlightLogClassificationList.Items.Count - 1

                If ChkFlightLogClassificationList.Items(i).Selected Then
                    SelectedFlightLogClassificationCount += 1 'Added By Vikrant On 13-Dec-2021 for Issue: If Classification is not selected in log then Hours total differ on Classification search criteria(ALL Classification)

                    If FlightClassificationName.Length = 0 Then
                        FlightClassificationName = ChkFlightLogClassificationList.Items(i).Text
                        FlightClassificationName1 = ChkFlightLogClassificationList.Items(i).Text
                    Else
                        FlightClassificationName = FlightClassificationName + "," + ChkFlightLogClassificationList.Items(i).Text
                        FlightClassificationName1 = FlightClassificationName + "," + ChkFlightLogClassificationList.Items(i).Text
                    End If

                End If

            Next

            'Added By Vikrant On 13-Dec-2021 for Issue: If Classification is not selected in log then Hours total differ on Classification search criteria(ALL Classification)
            If SelectedFlightLogClassificationCount = ChkFlightLogClassificationList.Items.Count Then

                If Not Request.Form("chkSelectAllFlightLogClassification") Is Nothing Then 'Select all check box is checked
                    FlightClassificationName = "(All)"
                End If

            End If
            'End of Added By Prashant 25-Oct-2018 ALL25102018

            lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
            lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
            lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
            lblAssembly1.Text = "Assembly : " & IIf(AssemblyText <> "", AssemblyText, "")
            lblFlightLogClassification1.Text = "Flight Log Classification :" & IIf(FlightClassificationName1 <> "", FlightClassificationName1, "All")

            Ref_Doc = IIf(chkLogNo.Checked, Ref_Doc + "Log No.", Ref_Doc)
            Ref_Doc = IIf(chkLogPageNo.Checked, Ref_Doc + ", Log Page No.", Ref_Doc)
            Ref_Doc = IIf(chkFlightNo.Checked, Ref_Doc + ", Flight No.", Ref_Doc)

            If LogType = 1 Then

                LogBookSearchingCriteria = lblDateRangeFrom.Text + ", " +
                                           lblDateRangeTo.Text + ", " +
                                           lblAircraft1.Text + ", " +
                                           lblAssembly1.Text + ", " +
                                           lblFlightLogClassification1.Text + ", " +
                                           Ref_Doc

            Else

                LogBookSearchingCriteria = lblDateRangeFrom.Text + ", " +
                                           lblDateRangeTo.Text + ", " +
                                           lblAircraft1.Text + ", " +
                                           lblAssembly1.Text + ", " +
                                           Ref_Doc

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ResetValues()

        Try

            StartDate = txtFromDate.Text.ToString
            EndDate = txtToDate.Text.ToString
            MachineID = "{00000000-0000-0000-0000-000000000000}"
            AssemblyID = "{00000000-0000-0000-0000-000000000000}"
            AssemblyType = ""
            Aircraft = ""
            AssemblyText = ""
            AssemblyTypeID = 1

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetReport(Optional ByMail As Boolean = False) 'Parameter Added by Shital on 6-Sep-2016

        Dim DayWiseLogBook As Boolean = False 'Added By Vikrant On 27-Dec-2018 For StarAir27122018
        Dim SearchStr1 As String = ""
        Dim SearchStr2 As String = ""
        Dim SearchStr7 As String  'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 

        Dim ReportName As String = ""

        Try

            SetValues()

            If chkLogNo.Checked Then
                SearchStr1 = "Log No."
            Else
                SearchStr1 = ""
            End If

            If chkLogPageNo.Checked = False Then
                '
            ElseIf SearchStr1 = "" Then
                SearchStr1 = "Log Page No."
            Else
                SearchStr1 = SearchStr1 + "/" + "Log Page No."
            End If

            If chkFlightNo.Checked = False Then
                '
            ElseIf SearchStr1 = "" Then
                SearchStr1 = "Flight No."
            Else
                SearchStr1 = SearchStr1 + "/" + "Flight No."
            End If

            If chkFlightLogClassifications.Checked = False Then
                '
            ElseIf SearchStr1 = "" Then
                SearchStr1 = "Classification"
            Else
                SearchStr1 = SearchStr1 + "/" + "Classification"
            End If

            'Added By Utkarsh On 19-Apr-2011
            MachineList = MachineList.GetMachineListMonitoringStatus(CurrentDate:=Now.ToShortDateString, , , , , , , , , , , ,
                                                                     AssemblyRequired:=True, ,
                                                                     AssemblyID:=cmbAircraftAssembly.SelectedValue, , , , , , , , , , , , , , , , , , , , , , , , , , ,
                                                                     IsTagRequired:=True,
                                                                     TagText:="(SELECT)",
                                                                     SkipIsForInventoryAircarft:=True)
            '***********************************
            If AssemblyList(cmbAircraftAssembly.SelectedIndex).Position <> "" Then
                SerialNoPosition = AssemblyList(cmbAircraftAssembly.SelectedIndex).SerialNo + "(" + AssemblyList(cmbAircraftAssembly.SelectedIndex).Position + ")"
            Else
                SerialNoPosition = AssemblyList(cmbAircraftAssembly.SelectedIndex).SerialNo
            End If

            If AppSettings("ShowMaintenanceForNewClients") = "True" Then

                LastMPDAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(MachineID:=New Guid(cmbAircraft.SelectedValue.ToString))

                If (LastMPDAMPRef.AMPNo <> "") Then
                    AMPNoStr = "AMP No.: " + LastMPDAMPRef.AMPNo + ",Rev No.: " + LastMPDAMPRef.RevNo + ",Dated: " + LastMPDAMPRef.FromDateFormatted
                Else
                    AMPNoStr = ""
                End If

            Else
                AMPNoStr = ""
            End If

            If LogType = 1 Then

                If cmbFormat.SelectedIndex = 0 Then

                    If chkMonthWise.Checked = True Then
                        CrystalReport = New crLogBookMonthwiseRegister
                    Else

                        If AppSettings("ClientCode") = "STR" Then 'Added By Vikrant On 27-Dec-2018 For StarAir27122018
                            CrystalReport = New crLogBookDaywiseRegister
                            DayWiseLogBook = True
                        Else
                            CrystalReport = New crLogBookRegister
                        End If

                    End If

                    ReportStatusList.Add(New rptStatus(,
                                                            GroupType:=0,
                                                            LHCaption:=New SmartDate(StartDate).FormattedText + " " + "To" + " " + New SmartDate(EndDate).FormattedText,
                                                            LHLabel:=AssemblyType + " " + "Details", , ,
                                                            LHData1:=MachineList(New Guid(cmbAircraft.SelectedValue)).RegNo, ,
                                                            LHData2:=AssemblyList(cmbAircraftAssembly.SelectedIndex).ModelName,
                                                            LHData3:=SerialNoPosition, , , , , , , , , , , ,
                                                            RHCaption:="Period",
                                                            RHLabel:="B/F", ,
                                                            RHLabel1:="Total Diff.", ,
                                                            RHLabel2:="After" + " " + New SmartDate(EndDate).FormattedText))

                    ReportName = AssemblyType + " " + "Log Book Entry"

                Else

                    'Added By Vikrant on 27-Sep-2012 For ALL26092012
                    If chkRemark.Checked Then
                        CrystalReport = New crLogRegisterWithRemark
                    Else
                        CrystalReport = New crLogRegister
                    End If
                    'End

                    ReportStatusList.Add(New rptStatus(,
                                                           GroupType:=0,
                                                           LHCaption:=New SmartDate(StartDate).FormattedText + " " + "To" + " " + New SmartDate(EndDate).FormattedText,
                                                           LHLabel:=AssemblyType + " " + "Details", ,
                                                           LHLabel1:=IIf(chkTakeOffTouchDown.Checked = True, "1", "0"),
                                                           LHData1:=MachineList(New Guid(cmbAircraft.SelectedValue)).RegNo, ,
                                                           LHData2:=AssemblyList(cmbAircraftAssembly.SelectedIndex).ModelName,
                                                           LHData3:=SerialNoPosition, , , , , , , , , , , ,
                                                           RHCaption:="Period",
                                                           RHLabel:="Before" + " " + New SmartDate(StartDate).FormattedText, ,
                                                           RHLabel1:="Total Diff.", ,
                                                           RHLabel2:="After" + " " + New SmartDate(EndDate).FormattedText))

                    ReportName = "Log Register of" + " " + AssemblyType

                End If

                AssemblyLogDifferencePeriodList = AssemblyLogDifferncePeriodList.
                                                         GetAssemblyLogDifferencePeriodList(FromDate:=StartDate,
                                                                                            ToDate:=EndDate,
                                                                                            AssemblyID:=New Guid(AssemblyID),
                                                                                            IsReport:=True)

6:              ReportLogRegister = ReportLogRegister.GetLogRegister(StartDate:=StartDate,
                                                                     EndDate:=EndDate,
                                                                     AssemblyID:=AssemblyID,
                                                                     MachineID:=MachineID,
                                                                     CalculateTotal:=True,
                                                                     FlightLogClassificationName:=FlightClassificationName,
                                                                     StatusSelectLog:=0,
                                                                     IsLogNo:=chkLogNo.Checked,
                                                                     IsLogPageNo:=chkLogPageNo.Checked,
                                                                     IsFlightNo:=chkFlightNo.Checked,,,
                                                                     IsFlightLogClassification:=chkFlightLogClassifications.Checked,
                                                                     GetLogPeriodsDayWise:=DayWiseLogBook,
                                                                     ShowSinceTSO:=chkShowSinceTSO.Checked,
                                                                     IsUTC:=IIf(rdbUTC.Checked,
                                                                                True,
                                                                                False),
                                                                     IsForLogBook:=True)

                'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 

                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then

                    If cmbAircraft.SelectedIndex > 0 Then
                        SearchStr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
                    Else
                        SearchStr7 = ""
                    End If

                Else
                    SearchStr7 = ""
                End If
                'End

                If chkShowSinceTSO.Checked Then 'Added by Saylee on 14-Feb-2020 for ALL14022020
                    SearchStr2 = "True"
                Else
                    SearchStr2 = "False"
                End If

                Dim Report As New ReportData(CompanyDetail.CompanyName,
                                             CompanyDetail.Address,
                                             CompanyDetail.Tel1,
                                             CompanyDetail.Tel2,
                                             CompanyDetail.Fax,
                                             CompanyDetail.Email,
                                             CompanyDetail.WebSite,
                                             ReportName,
                                             SearchStr1,
                                             SearchStr2,
                                             SearchStr3:="",
                                             SearchStr4:=IIf(AssemblyTypeID = 4, "True", "False"),
                                             SearchStr5:=FlightClassificationName,
                                             ProductVersion:=AppSettings("Product Version"),
                                             SINote:=AppSettings("SINote"),
                                             SearchStr6:="",
                                             SearchStr7:=SearchStr7,
                                             SearchStr8:="",
                                             SearchStr9:=rdbLocal.Checked.ToString,
                                             SearchStr10:=AppSettings("Logo"),
                                             SearchStr11:=AMPNoStr)  'Changed By Utkarsh For Report Logo.

                If ByMail = False Then  'If Case added by shital on 6-Sep-2016

                    If ReportLogRegister.Count = 0 Then

                        MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                        MSGBox.Message_text.NoRecordFound,
                                        "There are records for this search criteria",
                                        MsgBoxStyle.OkOnly,
                                        "")

                        Exit Sub

                    Else
                        RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 624)
                    End If

                End If

                'added by shital on 6-Sep-2016
                If (ByMail = True And ReportLogRegister.Count <= 0) Then

                    SendMailFile.SendMailFile(,
                                              UserName:=Thread.CurrentPrincipal.Identity.Name,
                                              Subject:=ReportName,
                                              Text:="Flight Log Book ",
                                              Info:="There is no record for this search criteria.",
                                              VendorEmailID:="",
                                              ToMailID:=Session("ToSendMailIDs"),
                                              CCMailID:=Session("CcSendMailIDs"),
                                              ReportPath:="",
                                              ReportByMail:=True,
                                              Remark:=Session("SendMailRemark"),
                                              ReportGeneratedBy:=Session("ReportGenratedBy"),
                                              SmtpHost:=Session("SmtpHost"),
                                              SmtpPort:=Session("SmtpPort"),
                                              SmtpUser:=Session("SmtpUser"),
                                              SmtpPassword:=Session("SmtpPassword"))

                    Exit Sub

                End If

                '-----------Added by Utkarsh for Report Logo---------------
                Dim companyLogo As rptImage = rptImage.GetImage(dsLogRegister)
                '----------------------------------------------------------
                dataAdapter.Fill(dsLogRegister, AssemblyLogDifferencePeriodList)
                dataAdapter.Fill(dsLogRegister, ReportLogRegister)
                dataAdapter.Fill(dsLogRegister, Report)
                dataAdapter.Fill(dsLogRegister, ReportStatusList)
                dataAdapter.Fill(dsLogRegister, companyLogo) 'Added by Utkarsh for Report Logo
                CrystalReport.SetDataSource(dsLogRegister)
                Session("CrystalReport") = CrystalReport

                'added by shital on 6-Sep-2016
                If (ByMail = True) Then

                    SendMailFile.SendMailFile(rpt:=Session("CrystalReport"),
                                              UserName:=Thread.CurrentPrincipal.Identity.Name,
                                              Subject:="Flight Log Book ",
                                              Text:="Flight Log Book ",
                                              Info:=" For " + lblDateRangeFrom.Text + ", " + lblAircraft1.Text, ,
                                              ToMailID:=Session("ToSendMailIDs"),
                                              CCMailID:=Session("CcSendMailIDs"),
                                              ReportPath:="",
                                              ReportByMail:=True,
                                              Remark:=Session("SendMailRemark"),
                                              ReportGeneratedBy:=Session("ReportGenratedBy"),
                                              SmtpHost:=Session("SmtpHost"),
                                              SmtpPort:=Session("SmtpPort"),
                                              SmtpUser:=Session("SmtpUser"),
                                              SmtpPassword:=Session("SmtpPassword"))

                Else

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "openTranDetail",
                                                        "openTranDetail();",
                                                        True)

                    MarkLog(Action.Print,
                            "FlightLogBook",
                            LogBookSearchingCriteria,
                            ErrorType.NoError,
                            Guid.Empty,
                            EventLogID)

                End If

                ResetValues()

            Else  'LogType = 2

                If cmbFormat.SelectedIndex = 0 Then

                    If AppSettings("ClientCode") = "YA" Or
                       AppSettings("ClientCode") = "TA" Or
                       AppSettings("ClientCode") = "SAA" Then

                        CrystalReport = New crHistoryCumLogRegisterYATA
                    Else
                        CrystalReport = New crHistoryCumLogRegisterBA
                    End If

                Else

                    If AppSettings("ClientCode") = "STR" Then
                        CrystalReport = New crHistoryCumLogRegisterSTR
                    Else
                        CrystalReport = New crHistoryCumLogRegister
                    End If

                End If

                Dim Eng1SerialNo, Eng2SerialNo, Eng1Position, Eng2Position, Prop1SerialNo, Prop2SerialNo, Prop1Position, Prop2Position, APU1SerialNo,
                    APU1Position As String
                Dim mEngineCount, mPropellerCount As Integer

                Eng1SerialNo = ""
                Eng2SerialNo = ""
                Eng1Position = ""
                Eng2Position = ""
                Prop1SerialNo = ""
                Prop2SerialNo = ""
                Prop1Position = ""
                Prop2Position = ""
                APU1SerialNo = ""
                APU1Position = ""

                For Each ObjAssemblyStatus As AssemblyList.AssemblyInfo In AssemblyList

                    If ObjAssemblyStatus.AssemblyTypeID = 2 Then  'Engine

                        If mEngineCount = 0 Then
                            Eng1SerialNo = ObjAssemblyStatus.SerialNo
                            Eng1Position = ObjAssemblyStatus.Position
                            mEngineCount += 1
                        Else
                            Eng2SerialNo = ObjAssemblyStatus.SerialNo
                            Eng2Position = ObjAssemblyStatus.Position
                        End If

                    ElseIf ObjAssemblyStatus.AssemblyTypeID = 3 Then 'Propeller

                        If mPropellerCount = 0 Then
                            Prop1SerialNo = ObjAssemblyStatus.SerialNo
                            Prop1Position = ObjAssemblyStatus.Position
                            mPropellerCount += 1
                        Else
                            Prop2SerialNo = ObjAssemblyStatus.SerialNo
                            Prop2Position = ObjAssemblyStatus.Position
                        End If

                    ElseIf ObjAssemblyStatus.AssemblyTypeID = 4 Then 'APU Auxiliary Power Unit Added By Prashant 11-Oct-2021 STR23092021 -2
                        APU1SerialNo = ObjAssemblyStatus.SerialNo
                        APU1Position = ObjAssemblyStatus.Position
                    End If

                Next

                If chkShowCompliance.Checked = False And
                   chkShowInstRem.Checked = False And
                   chkShowMaintActivity.Checked = False And
                   chkShowPirepsMELSnag.Checked = False Then

                    MSGBoxCtrl.Show("Selection Alert!",
                                    "Select atleast one Maintenance Activity.",
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Exit Sub

                End If

                ReportHistoryCumLogRegister = ReportHistoryCumLogRegister.
                                               GetHistoryCumLogRegister(StartDate:=StartDate,
                                                                        EndDate:=EndDate,
                                                                        WONo:="",
                                                                        AssemblyType:=AssemblyType,
                                                                        FromOrToOrOnModel:=Model,
                                                                        FromOrToOrOnSerialNo:=SerialNo,
                                                                        OfModel:="",
                                                                        OfSerialNo:="",
                                                                        OfPart:="",
                                                                        OfCompSerialNo:="", MachineID,
                                                                        IsAssemblyRequired:=True,
                                                                        IsCompRequired:=True,
                                                                        IsRemoved:=chkShowInstRem.Checked,
                                                                        IsInstalled:=chkShowInstRem.Checked,
                                                                        IsComplied:=True,
                                                                        AssemblyID:=AssemblyID,
                                                                        IsLogNo:=chkLogNo.Checked,
                                                                        IsLogPageNo:=chkLogPageNo.Checked,
                                                                        IsFlightNo:=chkFlightNo.Checked,
                                                                        ShowCompliance:=chkShowCompliance.Checked,
                                                                        IsMELRequired:=chkShowPirepsMELSnag.Checked,
                                                                        IsMaintenanceActivityRequired:=chkShowMaintActivity.Checked,
                                                                        AssemblyTypeID:=AssemblyTypeID)

                Dim BringForwardHrs As String = ""
                Dim BringForwardCycle As String = ""

                If ReportHistoryCumLogRegister.Count > 0 Then

                    BringForwardHrs = ReportHistoryCumLogRegister(0).Col1Final
                    BringForwardCycle = ReportHistoryCumLogRegister(0).Col2Final

                End If

                ReportStatusList.Add(New rptStatus(,
                                                        GroupType:=0,
                                                        LHCaption:=New SmartDate(StartDate).FormattedText + " " + "To" + " " + New SmartDate(EndDate).FormattedText,
                                                        LHLabel:=AssemblyType + " " + "Details", , ,
                                                        LHData1:=cmbAircraft.SelectedItem.ToString, ,
                                                        LHData2:=AssemblyList(cmbAircraftAssembly.SelectedIndex).ModelName,
                                                        LHData3:=SerialNoPosition,
                                                        LHData4:=AssemblyList(cmbAircraftAssembly.SelectedIndex).HourType.ToString,
                                                        LHData5:=MachineList(New Guid(cmbAircraft.SelectedValue)).AssemblyStatusList(1).AssemblyStatusPeriodList(2, "").AssemblyStartValueFormatted,
                                                        LHData6:=Eng1SerialNo,
                                                        LHData7:=Eng1Position,
                                                        LHData8:=Eng2SerialNo,
                                                        LHData9:=Eng2Position,
                                                        LHData10:=Prop1SerialNo,
                                                        LHData11:=Prop1Position,
                                                        LHData12:=Prop2SerialNo,
                                                        LHData13:=Prop2Position,
                                                        LHData14:=BringForwardHrs.ToString,
                                                        RHCaption:="Period",
                                                        RHLabel:="Before" + " " + New SmartDate(StartDate).FormattedText,
                                                        RHData:=BringForwardCycle.ToString,
                                                        RHLabel1:="Total Diff.",
                                                        RHData1:="",
                                                        RHLabel2:="After" + " " + New SmartDate(EndDate).FormattedText,
                                                        RHData2:=APU1SerialNo,
                                                        RHData3:=APU1Position,
                                                        RHData4:="",
                                                        RHData5:="",
                                                        RHData6:="",
                                                        RHData7:="",
                                                        RHData8:="",
                                                        RHData9:="",
                                                        RHData10:="",
                                                        RHData11:="",
                                                        RHData12:="",
                                                        RHData13:="",
                                                        RHData14:="",
                                                        RHData15:="",
                                                        RHData16:="",
                                                        RHData17:="",
                                                        RHData18:="",
                                                        RHData19:="",
                                                        RHData20:="",
                                                        RHData21:="",
                                                        RHData22:="",
                                                        RHData23:="",
                                                        RHData24:="",
                                                        RHData25:="",
                                                        RHData26:="",
                                                        RHData27:="",
                                                        RHData28:="",
                                                        RHData29:="",
                                                        RHData30:="",
                                                        RHData31:="",
                                                        RHData32:="",
                                                        RHData33:="",
                                                        RHData34:="",
                                                        RHData35:=""))

                'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 

                If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then

                    If cmbAircraft.SelectedIndex > 0 Then
                        SearchStr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
                    Else
                        SearchStr7 = ""
                    End If

                Else
                    SearchStr7 = ""
                End If

                Dim Report As New ReportData(CompanyDetail.CompanyName,
                                             CompanyDetail.Address,
                                             CompanyDetail.Tel1,
                                             CompanyDetail.Tel2,
                                             CompanyDetail.Fax,
                                             CompanyDetail.Email,
                                             CompanyDetail.WebSite,
                                             ReportName:="Electronic Log Register of" + " " + AssemblyType,
                                             SearchStr1:=New SmartDate(StartDate).FormattedText,
                                             SearchStr2:=New SmartDate(EndDate).FormattedText,
                                             SearchStr3:=SearchStr1, IIf(AssemblyTypeID = 4, "True", "False"),
                                             SearchStr5:=AppSettings("ClientCode"),
                                             ProductVersion:=AppSettings("Product Version"),
                                             SINote:=AppSettings("SINote"),
                                             SearchStr6:="",
                                             SearchStr7:=SearchStr7,
                                             SearchStr8:=ModuleList.Item("ElectronicLogBook").FormRevisionNo,
                                             SearchStr9:="",
                                             SearchStr10:=AppSettings("Logo"),
                                             SearchStr11:=AMPNoStr) 'Changed By Utkarsh For Report Logo.

                If ByMail = False Then 'If case Added By Shital On 6-Sep-2016

                    If ReportHistoryCumLogRegister.Count = 0 Then

                        MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                        MSGBox.Message_text.NoRecordFound,
                                        "There are no records for this search criteria",
                                        MsgBoxStyle.OkOnly,
                                        "")

                        Exit Sub
                    Else
                        RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 630)
                    End If

                End If

                If (ByMail = True And ReportHistoryCumLogRegister.Count <= 0) Then

                    SendMailFile.SendMailFile(,
                                              UserName:=Thread.CurrentPrincipal.Identity.Name,
                                              Subject:="Electronic Log Register of" + " " + AssemblyType, "Electronic Log Book ",
                                              Info:="There is no record for this search criteria.",
                                              VendorEmailID:="",
                                              ToMailID:=Session("ToSendMailIDs"),
                                              CCMailID:=Session("CcSendMailIDs"),
                                              ReportPath:="",
                                              ReportByMail:=True,
                                              Remark:=Session("SendMailRemark"),
                                              ReportGeneratedBy:=Session("ReportGenratedBy"),
                                              SmtpHost:=Session("SmtpHost"),
                                              SmtpPort:=Session("SmtpPort"),
                                              SmtpUser:=Session("SmtpUser"),
                                              SmtpPassword:=Session("SmtpPassword"))

                    Exit Sub

                End If

                Dim companyLogo As rptImage = rptImage.GetImage(dsHistoryCumLogRegister)
                dataAdapter.Fill(dsHistoryCumLogRegister, ReportHistoryCumLogRegister)
                dataAdapter.Fill(dsHistoryCumLogRegister, Report)
                dataAdapter.Fill(dsHistoryCumLogRegister, ReportStatusList)
                dataAdapter.Fill(dsHistoryCumLogRegister, companyLogo) 'Added by Utkarsh for Report Logo)
                CrystalReport.SetDataSource(dsHistoryCumLogRegister)
                Session("CrystalReport") = CrystalReport

                'added by shital on 6-Sep-2016
                If (ByMail = True) Then

                    SendMailFile.SendMailFile(rpt:=Session("CrystalReport"),
                                              UserName:=Thread.CurrentPrincipal.Identity.Name,
                                              Subject:="Electronic Log Register of" + " " + AssemblyType,
                                              Text:="Electronic Log Book ",
                                              Info:=" For " + lblDateRangeFrom.Text + ", " + lblAircraft1.Text, ,
                                              ToMailID:=Session("ToSendMailIDs"),
                                              CCMailID:=Session("CcSendMailIDs"),
                                              ReportPath:="",
                                              ReportByMail:=True,
                                              Remark:=Session("SendMailRemark"),
                                              ReportGeneratedBy:=Session("ReportGenratedBy"),
                                              SmtpHost:=Session("SmtpHost"),
                                              SmtpPort:=Session("SmtpPort"),
                                              SmtpUser:=Session("SmtpUser"),
                                              SmtpPassword:=Session("SmtpPassword"))

                Else

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "openTranDetail",
                                                        "openTranDetail();",
                                                        True)

                    MarkLog(Action.Print,
                            "ElectronicLogBook",
                            LogBookSearchingCriteria,
                            ErrorType.NoError,
                            Guid.Empty,
                            EventLogID)

                End If

                ResetValues()

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MessageBoxResult()

        Try

            Dim MsgBoxResult As MsgBoxResult
            MsgBoxResult = CType(Request.QueryString("MsgResult"), MsgBoxResult)

            If MsgBoxResult > 0 Then

                Select Case MsgBoxResult
                    Case MsgBoxResult.Yes
                    '
                    Case MsgBoxResult.No
                    '
                    Case MsgBoxResult.Ok
                        Session("Sender") = ""
                        Session("LogType") = LogType
                        Response.Redirect("wfSearchCriteriaForLogBook_Ajax.aspx?LogType=" + CStr(LogType))
                    Case Else
                        '
                End Select

            ElseIf MsgBoxResult = -1 Then
                Session("Sender") = ""
                Response.Redirect("wfSearchCriteriaForLogBook_Ajax.aspx?LogType=" + CStr(LogType))
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Data Binding "

    Public Sub CustomValidations(s As Object, e As ServerValidateEventArgs)

        Try

            Dim CustomValidator As CustomValidator
            CustomValidator = CType(s, CustomValidator)

            If CustomValidator.ControlToValidate = "cmbAircraft" Then

                If cmbAircraft.SelectedIndex = 0 Then
                    CustomValidator.ErrorMessage = "Please select the Aircraft"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If

            End If

            If CustomValidator.ControlToValidate = "cmbFormat" Then

                If Not (chkLogNo.Checked Or chkLogPageNo.Checked Or chkFlightNo.Checked) And (cmbFormat.SelectedIndex = 1) And (cmbFormat.Visible = True) Then
                    CustomValidator.ErrorMessage = "Please select either of the Log No. ,Log Page No. ,Flight No."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DataFieldBind()

        Try

            FlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("")
            ChkFlightLogClassificationList.DataSource = FlightLogClassificationList
            Session("FlightLogClassificationList") = FlightLogClassificationList

            DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            ClearAll()
            GetSession()

            EventLogID = CType(Session("EventLogID"), Guid)

            If Not IsPostBack Then

                LogType = Request.QueryString("LogType")
                Session("LogType") = LogType
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

                If LogType = 1 Then

                    lbltitle.Text = "Flight Log Register"
                    btnClose.ToolTip = "Close the Flight Log Register screen"
                    chkShowCompliance.Visible = False 'Added by Saylee on 2nd Jan-2013
                    chkShowInstRem.Visible = False
                    chkShowMaintActivity.Visible = False
                    chkShowPirepsMELSnag.Visible = False
                    lblSelectActivity.Visible = False
                    chkTakeOffTouchDown.Visible = IIf(cmbFormat.SelectedIndex = 1, True, False)
                    'Added By Vikrant On 27-Dec-2018 For StarAir27122018
                    cmbFormat.Items.Add(New ListItem("Format 1 (Summary)", "0"))
                    cmbFormat.Items.Add(New ListItem("Format 2 (Detail)", "1"))
                    'End
                Else

                    lbltitle.Text = "Electronic Log Register"
                    btnClose.ToolTip = "Close the Electronic Log Register"
                    ChkFlightLogClassificationList.Visible = False
                    cmbFormat.Visible = True
                    chkMonthWise.Visible = False '--Added By Utkarsh On 15-Feb-2011
                    lblFormat.Visible = True
                    lblSelectFormat.Visible = True
                    lblSelectFlightClassification.Visible = False
                    lblSelectFormat.Text = "Step IV. Selection of Format"
                    lblSelectReferenceDocument.Text = "Step V. Selection of Reference Document"
                    lblSelectActivity.Text = "Step VI. Selection of Activities"
                    lblDisplayReport.Text = "Step VII. Display Report"
                    chkShowCompliance.Visible = True 'Added by Saylee on 2nd Jan-2013
                    chkShowInstRem.Visible = True
                    chkShowMaintActivity.Visible = True
                    chkShowPirepsMELSnag.Visible = True
                    lblSelectActivity.Visible = True
                    chkTakeOffTouchDown.Visible = False

                    If AppSettings("ClientCode") = "BA" Or
                       AppSettings("ClientCode") = "PAS" Or
                       AppSettings("ClientCode") = "Novo" Or
                       AppSettings("ClientCode") = "YA" Or
                       AppSettings("ClientCode") = "TA" Then

                        lblSelectReferenceDocument.Visible = False
                        lblSelectActivity.Text = "Step IV. Selection of Activities"
                        lblDisplayReport.Text = "Step VI. Display Report"
                        chkLogNo.Visible = False
                        chkLogPageNo.Visible = False
                        chkFlightNo.Visible = False
                        chkFlightLogClassifications.Visible = False

                    End If
                    'Added By Vikrant On 27-Dec-2018 For StarAir27122018
                    cmbFormat.Items.Add(New ListItem("Format 1"))
                    cmbFormat.Items.Add(New ListItem("Format 2"))
                    'End
                End If

                cmbFormat.DataBind()
                Session("MiddleFrame") = "wfSearchCriteriaForLogBook_Ajax.aspx?LogType=" + CStr(LogType)
                ResetValues()
                lblAssembly.Enabled = False
                cmbAircraftAssembly.Enabled = False
                AsOnDate = Now.Date.ToString(AppSettings("DateFormat"))
                Session("AsOnDate") = AsOnDate
                SetComboOfMachine(AsOnDate)
                'Added by Shital on 02-jun-2021
                rdbLocal.Visible = IIf(AppSettings("ClientCode") = "GEP" Or AppSettings("ClientCode") = "SHN", True, False)
                rdbUTC.Visible = IIf(AppSettings("ClientCode") = "GEP" Or AppSettings("ClientCode") = "SHN", True, False)
                DataFieldBind()

            End If
            ControlVisibility()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ShowCurrentSearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click

        Try

            Display()
            SetValues()
            upnlCriteria.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

        Try

            If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

            If IsValid = True Then
                SetReport(False)
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            MachineList = Nothing
            MachineNameValueList = Nothing 'Added By Utkarsh On 19-Apr-2011
            AssemblyList = Nothing
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub AircraftChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged

        Try

            If cmbAircraft.SelectedIndex = 0 Then
                lblAssembly.Enabled = False
                cmbAircraftAssembly.Enabled = False
            Else

                lblAssembly.Enabled = True
                cmbAircraftAssembly.Enabled = True
                MachineName = cmbAircraft.SelectedValue.ToString

                Dim AssemblyList As AssemblyList
                AssemblyList = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, , True)
                Session("AssemblyList") = AssemblyList
                cmbAircraftAssembly.DataSource = AssemblyList
                cmbAircraftAssembly.DataBind()

                'Added by Saylee on 20-Feb-2020
                Dim AssemblyOHServiceCount As New AssemblyOHServiceCount
                AssemblyOHServiceCount = AssemblyOHServiceCount.GetAssemblyOHServiceCount(New Guid(cmbAircraftAssembly.SelectedValue.ToString), txtFromDate.Text.ToString)
                If AssemblyOHServiceCount IsNot Nothing Then
                    chkShowSinceTSO.Visible = AssemblyOHServiceCount.Count > 0
                End If
                '**************************************
                'added by Shital on 02-jun-2021
                rdbLocal.Checked = IIf(MachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsUTC = False, True, False)
                rdbUTC.Checked = IIf(MachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsUTC = True, True, False)
                upnlLocalUTC.Update()
                '-------

            End If

            ChkFlightLogClassificationList.ClearSelection()
            upnlDetails.Update()

            If cmbAircraft.Enabled = True Then
                SetFocus(cmbAircraft)
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub AircraftAssemblyChanged(sender As Object, e As EventArgs) Handles cmbAircraftAssembly.SelectedIndexChanged

        Try

            'Added by Saylee on 20-Feb-2020
            Dim AssemblyOHServiceCount As New AssemblyOHServiceCount
            AssemblyOHServiceCount = AssemblyOHServiceCount.GetAssemblyOHServiceCount(New Guid(cmbAircraftAssembly.SelectedValue.ToString), txtFromDate.Text.ToString)

            If AssemblyOHServiceCount IsNot Nothing Then
                chkShowSinceTSO.Visible = AssemblyOHServiceCount.Count > 0
            End If
            '**************************************

            ChkFlightLogClassificationList.ClearSelection()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ReportFormatChanged(sender As Object, e As EventArgs) Handles cmbFormat.SelectedIndexChanged

        Try

            chkMonthWise.Checked = False

            If cmbFormat.SelectedIndex = 1 Then

                If LogType = 1 Then chkTakeOffTouchDown.Visible = True

                If LogType = 1 Then
                    rdbLocal.Visible = True
                    rdbUTC.Visible = True
                End If

                btnDisplay.Enabled = True

            Else

                If LogType = 1 Then chkTakeOffTouchDown.Visible = False

                If LogType = 1 Then
                    rdbLocal.Visible = False
                    rdbUTC.Visible = False
                End If

                If cmbFormat.SelectedIndex = 2 Then
                    btnDisplay.Enabled = False
                Else
                    btnDisplay.Enabled = True
                End If

                ChkFlightLogClassificationList.ClearSelection()

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub FromDateChanged(sender As Object, e As EventArgs) Handles txtFromDate.TextChanged

        Try

            AODate = txtFromDate.Text.Trim

            If AsOnDate = AODate Then
            Else

                SetComboOfMachine(AODate)
                lblAssembly.Enabled = False
                cmbAircraftAssembly.Enabled = False
                AssemblyList = Nothing
                Session("AssemblyList") = AssemblyList
                cmbAircraftAssembly.ClearSelection()
                cmbAircraftAssembly.DataSource = AssemblyList
                cmbAircraftAssembly.Controls.Clear()
                cmbAircraftAssembly.DataBind()
                upnlDetails.Update()
                DataFieldBind()

            End If

            upnlDate.Update()
            upnlDetails.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ExportToExcel(sender As Object, e As EventArgs) Handles btnExport.Click

        Try

            If IsValid Then

                SetValues()

                MachineList = MachineList.GetMachineListMonitoringStatus(CurrentDate:=Now.ToShortDateString, , , , , , , , , , , ,
                                                                          AssemblyRequired:=True, ,
                                                                          AssemblyID:=cmbAircraftAssembly.SelectedValue, , , , , , , , , , , , , , , , , , , , , , , , , , ,
                                                                          IsTagRequired:=True,
                                                                          TagText:="(SELECT)",
                                                                          SkipIsForInventoryAircarft:=True)

                If LogType = 1 Then

                    Dim ReportName As String = ""

                    ReportLogRegister = ReportLogRegister.GetLogRegister(StartDate:=StartDate,
                                                                         EndDate:=EndDate,
                                                                         AssemblyID:=AssemblyID,
                                                                         MachineID:=MachineID,
                                                                         CalculateTotal:=True,
                                                                         FlightLogClassificationName:=FlightClassificationName,
                                                                         StatusSelectLog:=0,
                                                                         IsLogNo:=chkLogNo.Checked,
                                                                         IsLogPageNo:=chkLogPageNo.Checked,
                                                                         IsFlightNo:=chkFlightNo.Checked, ,
                                                                         IsFlightLogClassification:=chkFlightLogClassifications.Checked,
                                                                         ShowSinceTSO:=chkShowSinceTSO.Checked)

                    Dim Report As New ReportData(CompanyName:=CompanyDetail.CompanyName,
                                                 Address:=CompanyDetail.Address,
                                                 Tel1:=CompanyDetail.Tel1,
                                                 Tel2:=CompanyDetail.Tel2,
                                                 Fax:=CompanyDetail.Fax,
                                                 Email:=CompanyDetail.Email,
                                                 WebSite:=CompanyDetail.WebSite,
                                                 ReportName:=ReportName,
                                                 ProductVersion:=AppSettings("Product Version"),
                                                 SINote:=AppSettings("SINote"),
                                                 SearchStr1:=txtFromDate.Text,
                                                 SearchStr2:=txtToDate.Text,
                                                 SearchStr3:=cmbAircraft.SelectedItem.Text,
                                                 SearchStr4:=cmbAircraftAssembly.SelectedItem.Text,
                                                 SearchStr5:=FlightClassificationName,
                                                 SearchStr6:=cmbFormat.SelectedItem.Text,
                                                 SearchStr7:="",
                                                 SearchStr8:="",
                                                 SearchStr9:="",
                                                 SearchStr10:=AppSettings("Logo"))  'Changed By Utkarsh For Report Logo.

                    If ReportLogRegister.Count = 0 Then

                        MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                            MSGBox.Message_text.NoRecordFound,
                                            "There are no records for this search criteria",
                                            MsgBoxStyle.OkOnly,
                                            "")
                        Exit Sub

                    End If

                    If cmbFormat.SelectedIndex = 0 Then

                        ReportName = AssemblyType + " " + "Log Book Entry"

                        dataAdapter.Fill(dsLogRegister, "ExcelReportLogRegister", ReportLogRegister)
                        dataAdapter.Fill(dsLogRegister, "ExcelReportData", Report)

                        Dim columnToRemove As String() = {"DeparturePlaceCode", "ArrivalPlaceCode", "LogID", "AssemblyID", "LogDate", "Col1Label", "Col2Label", "Col3Label", "Col4Label", "ColLabel", "ColDiff", "ColFinal", "BlockTime", "PilotName", "CoPilotName", "DepartureTime", "ArrivalTime", "Col1Value", "Col2Value", "Col3Value", "Col4Value", "LogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocuments", "ReferencedDocumentsHeading", "TotalTimeInAir", "Col2DffMonthly", "LogPageNoFormattedForExcel", "Remark", "ArrivalUTCTime", "DepartureUTCTime", "ArrivalLocalUTCTime", "DepartureLocalUTCTime", "RegNo", "DepartureFrom", "ArrivalTo", "TimeInAir", "TimeOnGround", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col2DiffInDecimal", "Col2DiffPeriodID", "Col2DiffPeriodUnitID", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", "IsLogPageNo", "LogNoLogPageNo", "IntLogNo", "CONFLTTIMES", "CONBLOCKTIMES", "Type", "AuditedBy", "LogPageNoFormatted", "LogDatetmp", "LogTypeID", "LogDateForOrderBy", "MachineID", "FlightLogClassificationID", "TakeOffLocalUTCTime", "TouchDownLocalUTCTime", "TotalTimeInAirDaily", "Col2DffDaily", "Col1FinalInInteger", "Col2FinalInInteger", "Col3FinalInInteger", "Col4FinalInInteger", "TotalTimeInAirDailyInInteger", "Col2DffDailyInInteger", "Col5DiffInDecimal", "Col5DiffPeriodID", "Col5DiffPeriodUnitID", "Col5Value", "Col5FinalInInteger", "Col5Label", "LogTypeName", "DepartureCityName", "DepartureCityGMT", "ArrivalCityName", "ArrivalCityGMT", "IsUTC", "IsTLP", "IsFlightLogClassification", "FinalHrsCyclesLandings"}
                        Dim columnToRemove1 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ShortName", "FinalHrsCyclesLandings"}
                        Dim columnToRemove2 As String() = {"FinalHrsCyclesLandings"} 'Added by Harsh for FLYPAL-2443 =>This Property is used for web {UI only}

                        If AppSettings("ClientCode") <> "KLP" Then
                            columnToRemove2 = {"TakeOffDateOnlyForExcel", "TakeOffTimeOnlyForExcel", "TakeOffUTCDateOnlyForExcel", "TakeOffUTCTimeOnlyForExcel", "TouchDownDateOnlyForExcel", "TouchDownTimeOnlyForExcel", "TouchDownUTCDateOnlyForExcel", "TouchDownUTCTimeOnlyForExcel", "DepartureDateOnlyForExcel", "DepartureTimeOnlyForExcel", "ArrivalDateOnlyForExcel", "ArrivalTimeOnlyForExcel", "DepartureUTCDateOnlyForExcel", "DepartureUTCTimeOnlyForExcel", "DepartureUTCTimeOnlyForExcel", "ArrivalUTCDateOnlyForExcel", "ArrivalUTCTimeOnlyForExcel"}
                        Else
                            columnToRemove2 = {"TakeOffTime", "TouchDownTime", "TakeOffUTCTime", "TouchDownUTCTime", "TakeOffLocalUTCTimeForExcel", "TouchDownLocalUTCTimeForExcel"}
                        End If

                        For i As Integer = 0 To columnToRemove.Length - 1

                            If dsLogRegister.Tables("ExcelReportLogRegister").Columns.Contains(columnToRemove(i)) Then
                                dsLogRegister.Tables("ExcelReportLogRegister").Columns.Remove(columnToRemove(i))
                            End If

                        Next

                        For i As Integer = 0 To columnToRemove1.Length - 1

                            If dsLogRegister.Tables("ExcelReportData").Columns.Contains(columnToRemove1(i)) Then
                                dsLogRegister.Tables("ExcelReportData").Columns.Remove(columnToRemove1(i))
                            End If

                        Next

                        For i As Integer = 0 To columnToRemove2.Length - 1

                            If dsLogRegister.Tables("ExcelReportLogRegister").Columns.Contains(columnToRemove2(i)) Then
                                dsLogRegister.Tables("ExcelReportLogRegister").Columns.Remove(columnToRemove2(i))
                            End If

                        Next

                        Dim dsNew As New DataSet

                        dsNew.Clear()

                        dsNew.Merge(dsLogRegister.Tables("ExcelReportData"))
                        dsNew.Merge(dsLogRegister.Tables("ExcelReportLogRegister"))

                        dsNew.Tables("ExcelReportLogRegister").Columns("LogDateFormatted").ColumnName = "Day"
                        dsNew.Tables("ExcelReportLogRegister").Columns("DepartureArrivalPlaceCode").ColumnName = "Sector"
                        dsNew.Tables("ExcelReportLogRegister").Columns("Departure_ICAO").ColumnName = "Departure_ICAO"
                        dsNew.Tables("ExcelReportLogRegister").Columns("Arrival_ICAO").ColumnName = "Arrival_ICAO"

                        dsNew.Tables("ExcelReportLogRegister").Columns("Departure_ICAO").SetOrdinal(2)
                        dsNew.Tables("ExcelReportLogRegister").Columns("Arrival_ICAO").SetOrdinal(3)

                        dsNew.Tables("ExcelReportLogRegister").Columns("Col1Diff").ColumnName = "Daily Flying Hours"
                        dsNew.Tables("ExcelReportLogRegister").Columns("Col1Final").ColumnName = "Cumulative Flying Hours"
                        dsNew.Tables("ExcelReportLogRegister").Columns("Col2Diff").ColumnName = "Daily Landing"
                        dsNew.Tables("ExcelReportLogRegister").Columns("Col2Final").ColumnName = "Cumulative Landing"
                        dsNew.Tables("ExcelReportLogRegister").Columns("EmpNoForPilot").ColumnName = "Pilot Code"
                        dsNew.Tables("ExcelReportLogRegister").Columns("EmpNoForCoPilot").ColumnName = "Co-Pilot Code"

                        If ReportLogRegister(0).Col3Label = "" Then

                            Dim Col3 As String() = {"Col3Diff", "Col3Final"}
                            For i As Integer = 0 To Col3.Length - 1

                                If dsNew.Tables("ExcelReportLogRegister").Columns.Contains(Col3(i)) Then
                                    dsNew.Tables("ExcelReportLogRegister").Columns.Remove(Col3(i))
                                End If

                            Next

                        Else

                            dsNew.Tables("ExcelReportLogRegister").Columns("Col3Diff").ColumnName = ReportLogRegister(0).Col3Label
                            dsNew.Tables("ExcelReportLogRegister").Columns("Col3Final").ColumnName = "Final Of " + ReportLogRegister(0).Col3Label

                        End If

                        If ReportLogRegister(0).Col4Label = "" Then

                            Dim Col4 As String() = {"Col4Diff", "Col4Final"}

                            For i As Integer = 0 To Col4.Length - 1

                                If dsNew.Tables("ExcelReportLogRegister").Columns.Contains(Col4(i)) Then
                                    dsNew.Tables("ExcelReportLogRegister").Columns.Remove(Col4(i))
                                End If

                            Next

                        Else

                            dsNew.Tables("ExcelReportLogRegister").Columns("Col4Diff").ColumnName = ReportLogRegister(0).Col4Label
                            dsNew.Tables("ExcelReportLogRegister").Columns("Col4Final").ColumnName = "Final Of " + ReportLogRegister(0).Col4Label

                        End If

                        If ReportLogRegister(0).Col5Label = "" Then

                            Dim Col5 As String() = {"Col5Diff", "Col5Final"}

                            For i As Integer = 0 To Col5.Length - 1

                                If dsNew.Tables("ExcelReportLogRegister").Columns.Contains(Col5(i)) Then
                                    dsNew.Tables("ExcelReportLogRegister").Columns.Remove(Col5(i))
                                End If

                            Next

                        Else

                            dsNew.Tables("ExcelReportLogRegister").Columns("Col5Diff").ColumnName = ReportLogRegister(0).Col5Label
                            dsNew.Tables("ExcelReportLogRegister").Columns("Col5Final").ColumnName = "Final Of " + ReportLogRegister(0).Col5Label

                        End If

                        If AppSettings("ClientCode") <> "KLP" Then

                            If MachineList(New Guid(cmbAircraft.SelectedValue)).IsUTC Then
                                dsNew.Tables("ExcelReportLogRegister").Columns("TakeOffLocalUTCTimeForExcel").ColumnName = "Take-Off-UTCTime"
                                dsNew.Tables("ExcelReportLogRegister").Columns("TouchDownLocalUTCTimeForExcel").ColumnName = "Touch-Down UTCTime"
                            Else
                                dsNew.Tables("ExcelReportLogRegister").Columns("TakeOffLocalUTCTimeForExcel").ColumnName = "Take-Off LocalTime"
                                dsNew.Tables("ExcelReportLogRegister").Columns("TouchDownLocalUTCTimeForExcel").ColumnName = "Touch-Down LocalTime"
                            End If

                        Else

                            dsNew.Tables("ExcelReportLogRegister").Columns("TakeOffDateOnlyForExcel").ColumnName = "Take-Off-Date"
                            dsNew.Tables("ExcelReportLogRegister").Columns("TakeOffTimeOnlyForExcel").ColumnName = "Take-Off-Time"
                            dsNew.Tables("ExcelReportLogRegister").Columns("TakeOffUTCDateOnlyForExcel").ColumnName = "Take-Off-UTCDate"
                            dsNew.Tables("ExcelReportLogRegister").Columns("TakeOffUTCTimeOnlyForExcel").ColumnName = "Take-Off-UTCTime"
                            dsNew.Tables("ExcelReportLogRegister").Columns("TouchDownDateOnlyForExcel").ColumnName = "Touch-Down-Date"
                            dsNew.Tables("ExcelReportLogRegister").Columns("TouchDownTimeOnlyForExcel").ColumnName = "Touch-Down-Time"
                            dsNew.Tables("ExcelReportLogRegister").Columns("TouchDownUTCDateOnlyForExcel").ColumnName = "Touch-Down-UTCDate"
                            dsNew.Tables("ExcelReportLogRegister").Columns("TouchDownUTCTimeOnlyForExcel").ColumnName = "Touch-Down-UTCTime"

                        End If

                        dsNew.Tables("ExcelReportData").Columns("SearchStr1").ColumnName = "From Date"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr2").ColumnName = "To Date"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr3").ColumnName = "Aircraft"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr4").ColumnName = "Assembly"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr5").ColumnName = "Flight Log Classification"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr6").ColumnName = "Format"

                        dsNew.Tables("ExcelReportData").TableName = "Searching Criteria"
                        dsNew.Tables("ExcelReportLogRegister").TableName = ReportName

                        Session("dsNew") = dsNew
                        Session("ExcelFileName") = ReportName
                    Else

                        ReportName = "Log Register of" + " " + AssemblyType

                        dataAdapter.Fill(dsLogRegister, "ExcelReportData", Report)
                        dataAdapter.Fill(dsLogRegister, "ExcelReportLogRegisterDetail", ReportLogRegister)

                        Dim columnToRemove As String() = {"FinalHrsCyclesLandings"} 'Added by Harsh for FLYPAL-2443 =>This Property is used for web {UI only}
                        Dim columnToRemove1 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ShortName", "FinalHrsCyclesLandings"}
                        Dim columnToRemove2 As String() = {"FinalHrsCyclesLandings"} 'Added by Harsh for FLYPAL-2443 =>This Property is used for web {UI only}

                        If MachineList(New Guid(cmbAircraft.SelectedValue)).IsUTC Then 'Added By Prashant  2-Feb-2017

                            If AppSettings("ClientCode") = "GEP" Then
                                columnToRemove = {"Type", "LogTypeID", "LogID", "AssemblyID", "LogDate", "Col1Label", "Col2Label", "Col3Label", "Col4Label", "ColLabel", "ColDiff", "ColFinal", "PilotName", "CoPilotName", "Col1Value", "Col2Value", "Col3Value", "Col4Value", "LogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocuments", "ReferencedDocumentsHeading", "TotalTimeInAir", "Col2DffMonthly", "Remark", "ArrivalLocalUTCTime", "DepartureLocalUTCTime", "RegNo", "TimeInAir", "TimeOnGround", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col2DiffInDecimal", "Col2DiffPeriodID", "Col2DiffPeriodUnitID", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", "IsLogPageNo", "LogNoLogPageNo", "IntLogNo", "DepartureArrivalPlaceCode", "LogDateFormatted", "LogPageNoFormatted", "EmpNoForPilot", "EmpNoForCoPilot", "TakeOffLocalUTCTime", "TouchDownLocalUTCTime", "MachineID", "FlightLogClassificationID", "TotalTimeInAirDaily", "Col2DffDaily", "Col1FinalInInteger", "Col2FinalInInteger", "Col3FinalInInteger", "Col4FinalInInteger", "TotalTimeInAirDailyInInteger", "Col2DffDailyInInteger", "Col5DiffInDecimal", "Col5DiffPeriodID", "Col5DiffPeriodUnitID", "Col5Value", "Col5FinalInInteger", "Col5Label", "IsFlightLogClassification", "IsTLP", "IsUTC", "ArrivalCityName", "ArrivalCityGMT", "DepartureCityName", "DepartureCityGMT", "TakeOffLocalUTCTimeForExcel", "TouchDownLocalUTCTimeForExcel", "DeparturePlaceCode", "ArrivalPlaceCode", "AuditedBy", "LogDatetmp", "LogDateForOrderBy", "LogNo", "FlightNo", "LogTypeName"}
                            Else
                                columnToRemove = {"Type", "LogTypeID", "LogID", "AssemblyID", "LogDate", "Col1Label", "Col2Label", "Col3Label", "Col4Label", "ColLabel", "ColDiff", "ColFinal", "PilotName", "CoPilotName", "Col1Value", "Col2Value", "Col3Value", "Col4Value", "LogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocuments", "ReferencedDocumentsHeading", "TotalTimeInAir", "Col2DffMonthly", "Remark", "ArrivalLocalUTCTime", "DepartureLocalUTCTime", "RegNo", "DepartureFrom", "ArrivalTo", "TimeInAir", "TimeOnGround", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col2DiffInDecimal", "Col2DiffPeriodID", "Col2DiffPeriodUnitID", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", "IsLogPageNo", "LogNoLogPageNo", "IntLogNo", "DepartureArrivalPlaceCode", "LogDateFormatted", "LogPageNoFormatted", "EmpNoForPilot", "EmpNoForCoPilot", "TakeOffLocalUTCTime", "TouchDownLocalUTCTime", "MachineID", "FlightLogClassificationID", "TotalTimeInAirDaily", "Col2DffDaily", "Col1FinalInInteger", "Col2FinalInInteger", "Col3FinalInInteger", "Col4FinalInInteger", "TotalTimeInAirDailyInInteger", "Col2DffDailyInInteger", "Col5DiffInDecimal", "Col5DiffPeriodID", "Col5DiffPeriodUnitID", "Col5Value", "Col5FinalInInteger", "Col5Label", "IsFlightLogClassification", "IsTLP", "IsUTC", "ArrivalCityName", "ArrivalCityGMT", "DepartureCityName", "DepartureCityGMT", "AuditedBy", "LogDatetmp", "LogDateForOrderBy", "LogNo", "FlightNo", "LogTypeName"}
                            End If

                        Else

                            If AppSettings("ClientCode") = "GEP" Then
                                columnToRemove = {"Type", "LogTypeID", "LogID", "AssemblyID", "LogDate", "Col1Label", "Col2Label", "Col3Label", "Col4Label", "ColLabel", "ColDiff", "ColFinal", "PilotName", "CoPilotName", "Col1Value", "Col2Value", "Col3Value", "Col4Value", "LogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocuments", "ReferencedDocumentsHeading", "TotalTimeInAir", "Col2DffMonthly", "Remark", "ArrivalLocalUTCTime", "DepartureLocalUTCTime", "RegNo", "TimeInAir", "TimeOnGround", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col2DiffInDecimal", "Col2DiffPeriodID", "Col2DiffPeriodUnitID", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", "IsLogPageNo", "LogNoLogPageNo", "IntLogNo", "DepartureArrivalPlaceCode", "LogDateFormatted", "LogPageNoFormatted", "EmpNoForPilot", "EmpNoForCoPilot", "TakeOffLocalUTCTime", "TouchDownLocalUTCTime", "MachineID", "FlightLogClassificationID", "TotalTimeInAirDaily", "Col2DffDaily", "Col1FinalInInteger", "Col2FinalInInteger", "Col3FinalInInteger", "Col4FinalInInteger", "TotalTimeInAirDailyInInteger", "Col2DffDailyInInteger", "Col5DiffInDecimal", "Col5DiffPeriodID", "Col5DiffPeriodUnitID", "Col5Value", "Col5FinalInInteger", "Col5Label", "IsFlightLogClassification", "TouchDownLocalUTCTime", "IsTLP", "IsUTC", "ArrivalCityGMT", "TakeOffLocalUTCTime", "TouchDownLocalUTCTime", "DepartureCityGMT", "TakeOffLocalUTCTimeForExcel", "TouchDownLocalUTCTimeForExcel", "DeparturePlaceCode", "ArrivalPlaceCode", "AuditedBy", "LogDatetmp", "LogDateForOrderBy", "LogNo", "FlightNo", "LogTypeName", "FinalHrsCyclesLandings"}
                            Else
                                columnToRemove = {"Type", "LogTypeID", "LogID", "AssemblyID", "LogDate", "Col1Label", "Col2Label", "Col3Label", "Col4Label", "ColLabel", "ColDiff", "ColFinal", "PilotName", "CoPilotName", "Col1Value", "Col2Value", "Col3Value", "Col4Value", "LogPageNo", "IsLogNo", "IsFlightNo", "ReferencedDocuments", "ReferencedDocumentsHeading", "TotalTimeInAir", "Col2DffMonthly", "Remark", "ArrivalLocalUTCTime", "DepartureLocalUTCTime", "RegNo", "DepartureFrom", "ArrivalTo", "TimeInAir", "TimeOnGround", "Col1DiffInDecimal", "Col1DiffPeriodID", "Col1DiffPeriodUnitID", "Col2DiffInDecimal", "Col2DiffPeriodID", "Col2DiffPeriodUnitID", "Col3DiffInDecimal", "Col3DiffPeriodID", "Col3DiffPeriodUnitID", "Col4DiffInDecimal", "Col4DiffPeriodID", "Col4DiffPeriodUnitID", "IsLogPageNo", "LogNoLogPageNo", "IntLogNo", "DepartureArrivalPlaceCode", "LogDateFormatted", "LogPageNoFormatted", "EmpNoForPilot", "EmpNoForCoPilot", "TakeOffLocalUTCTime", "TouchDownLocalUTCTime", "MachineID", "FlightLogClassificationID", "TotalTimeInAirDaily", "Col2DffDaily", "Col1FinalInInteger", "Col2FinalInInteger", "Col3FinalInInteger", "Col4FinalInInteger", "TotalTimeInAirDailyInInteger", "Col2DffDailyInInteger", "Col5DiffInDecimal", "Col5DiffPeriodID", "Col5DiffPeriodUnitID", "Col5Value", "Col5FinalInInteger", "Col5Label", "IsFlightLogClassification", "TouchDownLocalUTCTime", "IsTLP", "IsUTC", "ArrivalCityGMT", "TakeOffLocalUTCTime", "TouchDownLocalUTCTime", "DepartureCityGMT", "AuditedBy", "LogDatetmp", "LogDateForOrderBy", "LogNo", "FlightNo", "LogTypeName", "FinalHrsCyclesLandings"}
                            End If

                        End If

                        If AppSettings("ClientCode") <> "KLP" Then
                            columnToRemove2 = {"TakeOffDateOnlyForExcel", "TakeOffTimeOnlyForExcel", "TakeOffUTCDateOnlyForExcel", "TakeOffUTCTimeOnlyForExcel", "TouchDownDateOnlyForExcel", "TouchDownTimeOnlyForExcel", "TouchDownUTCDateOnlyForExcel", "TouchDownUTCTimeOnlyForExcel", "TakeOffLocalUTCTimeForExcel", "TouchDownLocalUTCTimeForExcel", "DepartureDateOnlyForExcel", "DepartureTimeOnlyForExcel", "ArrivalDateOnlyForExcel", "ArrivalTimeOnlyForExcel", "DepartureUTCDateOnlyForExcel", "DepartureUTCTimeOnlyForExcel", "DepartureUTCTimeOnlyForExcel", "ArrivalUTCDateOnlyForExcel", "ArrivalUTCTimeOnlyForExcel", "FinalHrsCyclesLandings"}
                        Else
                            columnToRemove2 = {"TakeOffTime", "TouchDownTime", "TakeOffUTCTime", "TouchDownUTCTime", "TakeOffLocalUTCTimeForExcel", "TouchDownLocalUTCTimeForExcel", "DepartureTime", "ArrivalTime", "DepartureUTCTime", "ArrivalUTCTime", "FinalHrsCyclesLandings"}
                        End If

                        For i As Integer = 0 To columnToRemove.Length - 1

                            If dsLogRegister.Tables("ExcelReportLogRegisterDetail").Columns.Contains(columnToRemove(i)) Then
                                dsLogRegister.Tables("ExcelReportLogRegisterDetail").Columns.Remove(columnToRemove(i))
                            End If

                        Next

                        For i As Integer = 0 To columnToRemove1.Length - 1

                            If dsLogRegister.Tables("ExcelReportData").Columns.Contains(columnToRemove1(i)) Then
                                dsLogRegister.Tables("ExcelReportData").Columns.Remove(columnToRemove1(i))
                            End If

                        Next

                        For i As Integer = 0 To columnToRemove2.Length - 1

                            If dsLogRegister.Tables("ExcelReportLogRegisterDetail").Columns.Contains(columnToRemove2(i)) Then
                                dsLogRegister.Tables("ExcelReportLogRegisterDetail").Columns.Remove(columnToRemove2(i))
                            End If

                        Next

                        Dim dsNew As New DataSet

                        dsNew.Clear()

                        dsNew.Merge(dsLogRegister.Tables("ExcelReportData"))
                        dsNew.Merge(dsLogRegister.Tables("ExcelReportLogRegisterDetail"))

                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("LogPageNoFormattedForExcel").ColumnName = "Log Page"

                        If AppSettings("ClientCode") = "GEP" Then

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DepartureFrom").ColumnName = "Routing From"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalTo").ColumnName = "Routing to"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Routing From").SetOrdinal(1)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Routing to").SetOrdinal(2)

                        Else

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DeparturePlaceCode").ColumnName = "Routing From"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalPlaceCode").ColumnName = "Routing To"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Departure_ICAO").ColumnName = "Departure_ICAO"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Arrival_ICAO").ColumnName = "Arrival_ICAO"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Departure_ICAO").SetOrdinal(3)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Arrival_ICAO").SetOrdinal(4)

                        End If

                        If AppSettings("ClientCode") <> "KLP" Then

                            If MachineList(New Guid(cmbAircraft.SelectedValue)).IsUTC Then 'Added By Prashant  2-Feb-2017

                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TakeOffUTCTime").ColumnName = "Take Off UTC"
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TouchDownUTCTime").ColumnName = "Touch Down UTC"
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DepartureUTCTime").ColumnName = "Departure UTC"
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalUTCTime").ColumnName = "Arrival UTC"

                            Else

                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TakeOffTime").ColumnName = "Take Off"
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TouchDownTime").ColumnName = "Touch Down"
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DepartureTime").ColumnName = "Departure"
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalTime").ColumnName = "Arrival"

                            End If

                        End If

                        If AppSettings("ClientCode") = "KLP" Then

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DepartureDateOnlyForExcel").ColumnName = "Departure Date"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DepartureTimeOnlyForExcel").ColumnName = "Departure Time"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalDateOnlyForExcel").ColumnName = "Arrival Date"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalTimeOnlyForExcel").ColumnName = "Arrival Time"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TakeOffDateOnlyForExcel").ColumnName = "Take-Off-Date"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TakeOffTimeOnlyForExcel").ColumnName = "Take-Off-Time"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TouchDownDateOnlyForExcel").ColumnName = "Touch-Down-Date"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TouchDownTimeOnlyForExcel").ColumnName = "Touch-Down-Time"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TakeOffUTCDateOnlyForExcel").ColumnName = "Take-Off-UTCDate"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TakeOffUTCTimeOnlyForExcel").ColumnName = "Take-Off-UTCTime"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DepartureUTCDateOnlyForExcel").ColumnName = "Departure UTCDate"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("DepartureUTCTimeOnlyForExcel").ColumnName = "Departure UTCTime"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalUTCDateOnlyForExcel").ColumnName = "Arrival UTCDate"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("ArrivalUTCTimeOnlyForExcel").ColumnName = "Arrival UTCTime"

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TouchDownUTCDateOnlyForExcel").ColumnName = "Touch-Down-UTCDate"
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TouchDownUTCTimeOnlyForExcel").ColumnName = "Touch-Down-UTCTime"

                            'Ordinal
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Log Page").SetOrdinal(0)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Routing From").SetOrdinal(1)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Routing To").SetOrdinal(2)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Departure Date").SetOrdinal(3)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Departure Time").SetOrdinal(4)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Take-Off-Date").SetOrdinal(5)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Take-Off-Time").SetOrdinal(6)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Touch-Down-Date").SetOrdinal(7)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Touch-Down-Time").SetOrdinal(8)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Arrival Date").SetOrdinal(9)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Arrival Time").SetOrdinal(10)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Departure UTCDate").SetOrdinal(11)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Departure UTCTime").SetOrdinal(12)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Take-Off-UTCDate").SetOrdinal(13)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Take-Off-UTCTime").SetOrdinal(14)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Touch-Down-UTCDate").SetOrdinal(15)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Touch-Down-UTCTime").SetOrdinal(16)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Arrival UTCDate").SetOrdinal(17)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Arrival UTCTime").SetOrdinal(18)
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Flight Time").SetOrdinal(19)

                        End If

                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col1Diff").ColumnName = "Flight Time"
                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("CONFLTTIMES").ColumnName = "CON. FLT. TIMES"
                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("CONBLOCKTIMES").ColumnName = "CON. BLOCK TIMES"
                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col1Final").ColumnName = "TOTAL A/C TIME"
                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col2Final").ColumnName = "TOTAL CYCLES"
                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col2Diff").ColumnName = "CYLCES FLOWN"
                        dsNew.Tables("ExcelReportLogRegisterDetail").Columns("FlightLogClassificationName").ColumnName = "Flight Log Classification"

                        If ReportLogRegister(0).Col2Label = "" Then

                            Dim Col2 As String() = {"CYLCES FLOWN", "TOTAL CYCLES"}

                            For i As Integer = 0 To Col2.Length - 1

                                If dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Contains(Col2(i)) Then
                                    dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Remove(Col2(i))
                                End If

                            Next

                        Else

                            If AppSettings("ClientCode") = "IND" Then
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("CYLCES FLOWN").ColumnName = ReportLogRegister(0).Col2Label
                                dsNew.Tables("ExcelReportLogRegisterDetail").Columns("TOTAL CYCLES").ColumnName = "Final Of " + ReportLogRegister(0).Col2Label
                            End If

                        End If

                        If ReportLogRegister(0).Col3Label = "" Then

                            Dim Col3 As String() = {"Col3Diff", "Col3Final"}

                            For i As Integer = 0 To Col3.Length - 1

                                If dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Contains(Col3(i)) Then
                                    dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Remove(Col3(i))
                                End If

                            Next

                        Else

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col3Diff").ColumnName = ReportLogRegister(0).Col3Label
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col3Final").ColumnName = "Final Of " + ReportLogRegister(0).Col3Label

                        End If

                        If ReportLogRegister(0).Col4Label = "" Then

                            Dim Col4 As String() = {"Col4Diff", "Col4Final"}

                            For i As Integer = 0 To Col4.Length - 1

                                If dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Contains(Col4(i)) Then
                                    dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Remove(Col4(i))
                                End If

                            Next

                        Else

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col4Diff").ColumnName = ReportLogRegister(0).Col4Label
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col4Final").ColumnName = "Final Of " + ReportLogRegister(0).Col4Label

                        End If

                        If ReportLogRegister(0).Col5Label = "" Then

                            Dim Col5 As String() = {"Col5Diff", "Col5Final"}

                            For i As Integer = 0 To Col5.Length - 1

                                If dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Contains(Col5(i)) Then
                                    dsNew.Tables("ExcelReportLogRegisterDetail").Columns.Remove(Col5(i))
                                End If

                            Next

                        Else

                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col5Diff").ColumnName = ReportLogRegister(0).Col5Label
                            dsNew.Tables("ExcelReportLogRegisterDetail").Columns("Col5Final").ColumnName = "Final Of " + ReportLogRegister(0).Col5Label

                        End If

                        dsNew.Tables("ExcelReportData").Columns("SearchStr1").ColumnName = "From Date"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr2").ColumnName = "To Date"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr3").ColumnName = "Aircraft"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr4").ColumnName = "Assembly"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr5").ColumnName = "Flight Log Classification"
                        dsNew.Tables("ExcelReportData").Columns("SearchStr6").ColumnName = "Format"

                        dsNew.Tables("ExcelReportData").TableName = "Searching Criteria"
                        dsNew.Tables("ExcelReportLogRegisterDetail").TableName = ReportName

                        Session("dsNew") = dsNew
                        Session("ExcelFileName") = ReportName
                    End If

                    ScriptManager.RegisterStartupScript(Me,
                                                       [GetType],
                                                       "Display Report In Excel",
                                                       "displayReportInExcel();",
                                                       True)
                    'Added by Prashant on 19-Jan-2021
                    MarkLog(Action.Print,
                            IIf(cmbFormat.SelectedIndex = 0,
                                "FlightLogBook",
                                "ElectronicLogBook"),
                            "Export To Excel " + LogBookSearchingCriteria,
                            ErrorType.NoError,
                            Guid.Empty,
                            EventLogID)

                End If
            Else
                upnlValidationSummary.Update()
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    'Added by Shital on 6-Sep-2016
    Private Sub SendReportByMail(sender As Object, e As EventArgs) Handles btnByMail.Click

        Try

            If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

            If IsValid = True Then

                'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

                If LogType = 1 Then

                    Session("UserEmailID") = ModuleList.Item("FlightLogBook").SendToMailID
                    Session("UserCcEmailID") = ModuleList.Item("FlightLogBook").SendCCMailID
                    Session("SmtpHost") = ModuleList.Item("FlightLogBook").SmtpHost
                    Session("SmtpPort") = ModuleList.Item("FlightLogBook").SmtpPort
                    Session("SmtpUser") = ModuleList.Item("FlightLogBook").SmtpUser
                    Session("SmtpPassword") = ModuleList.Item("FlightLogBook").SmtpPassword

                ElseIf LogType = 2 Then

                    Session("UserEmailID") = ModuleList.Item("ElectronicLogBook").SendToMailID
                    Session("UserCcEmailID") = ModuleList.Item("ElectronicLogBook").SendCCMailID
                    Session("SmtpHost") = ModuleList.Item("ElectronicLogBook").SmtpHost
                    Session("SmtpPort") = ModuleList.Item("ElectronicLogBook").SmtpPort
                    Session("SmtpUser") = ModuleList.Item("ElectronicLogBook").SmtpUser
                    Session("SmtpPassword") = ModuleList.Item("ElectronicLogBook").SmtpPassword

                End If

                '--------------------------
                Dim Str As String
                Str = "OpenByMaiWindow();"
                ScriptManager.RegisterStartupScript(Me, [GetType], "OpenByMaiWindow", Str, True)

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub HdnBtnSendMail(sender As Object, e As EventArgs) Handles hdnimgLogBtnSendMail.Click

        Dim email As Thread
        Try

            email = New Thread(Sub() SetReport(True)) With {
                .IsBackground = True
            }
            email.Start()

        Catch ex As Exception

            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim TodayDate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & TodayDate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgMELBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)

        End Try

    End Sub

#End Region

End Class