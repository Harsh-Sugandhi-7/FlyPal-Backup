
'CREATED By : Saylee
'Dated      : 24-Jan-2014

Imports System.Linq
Imports System.Collections.Generic
Public Class wfSearchCriteriaForDuePeriod_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mDueLimits As DueLimits

    Dim mPerDayLimits As PerDayLimits

    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList

    Dim mtmpMachineList As tmpMachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail

    Dim ObjMachine As MachineInfo
    Dim ObjAssemblyStatus As AssemblyStatusInfo
    Dim ObjAssemblyStatusPeriod As AssemblyStatusPeriodInfo
    Dim ObjCompStatus As CompStatusInfo
    Dim ObjCompStatusPeriod As CompStatusPeriodInfo

    Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
    Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
    Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
    Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
    Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
    Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo
    Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
    Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
    Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
    Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo
    Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
    Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

    Private Flag As Int16
    Dim AOdate As String
    Dim AOnDate As String
    Dim Average As String
    Dim Aircraft As String
    Dim Report As Integer = 1
    Dim Periodcount As Integer
    Dim MachineName As String
    Dim AsonDate As String
    Dim Type As Integer = 1
    Dim AssemblyID As Guid
    Dim Count As Integer
    Dim mDueLimit As DueLimit
    Dim AvgMnths As Integer

    Private ATAChapter As String
    Private RegNo As String
    Private AssemblyType As String
    Private Model As String
    Private AssemblySerialNo As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private Note As String
    Private Description As String
    Private SerialNo As String
    Private EstimatedDate As String
    Private Freq1 As String
    Private Freq2 As String
    Private Freq3 As String

    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String

    Private SinceNew As String
    Private SinceNew1 As String
    Private SinceNew2 As String
    Private RemainingTime As String
    Private RemainingTime1 As String
    Private RemainingTime2 As String
    Private DueAsof As String
    Private DueAsof1 As String
    Private DueAsof2 As String
    Private DoneAt As String
    Private DoneAt1 As String
    Private DoneAt2 As String
    Private AssemblyModel As String
    Private MaintenanceEvent As String

    Private MinimumRemainingValue As Decimal
    Private AssemblyTypeID As Integer
    Private percent As String
    Private DueType As Integer

    Private mIsPreview As Boolean = False '11-Sep-2008

    Dim mCofASearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
    Dim PeriodLimt As String = String.Empty  'Added by Prashant on 04-Dec-2013
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub addAttributes()
        txtAvgMnths.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAvgMnths').value,event)")
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text)))
        Next i
        Session("mDueLimits") = mDueLimits

        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdvPerDayLimit.Rows.Count - 1
            txtPerDatLimit = CType(Me.gdvPerDayLimit.Rows(i1).FindControl("txtLimitPerDay"), TextBox)
            mPerDayLimits.Item(i1).PeriodLimit = CDec(Val(Trim(txtPerDatLimit.Text)))
            PeriodLimt = PeriodLimt + ", " + Trim(txtPerDatLimit.Text)
        Next i1
        Session("mPerDayLimits") = mPerDayLimits

    End Sub
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mDueLimits = CType(Session("mDueLimits"), DueLimits)
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits)

        AOnDate = Session("AOnDate")
        Report = Session("Report")
        Type = Session("Type")
        AvgMnths = Session("AvgMnths")

        DueType = Session("DueType")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mDueLimits") = mDueLimits
        Session("mPerDayLimits") = mPerDayLimits
        Session("AOnDate") = AOnDate
        Session("Report") = Report
        Session("Type") = Type
        Session("AvgMnths") = AvgMnths
        Session("DueType") = DueType
    End Sub
    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfSearchCriteriaForDuePeriod_AJAX.aspx?DueType=" & DueType Then
            Session.Remove("mMachineList")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("Type")
            Session.Remove("AvgMnths")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblAvgMnths1.Visible = True
        lblDateRangeFrom.Visible = True
        lblPercent.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        If cmbAircraft.SelectedItem.Text = "(All)" Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
        End If
        Average = txtAvgMnths.Text
        If Not IsDate(txtFromDate.Text) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text.ToString
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & New SmartDate(txtFromDate.Text.ToString).FormattedText
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If

        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
        lblAvgMnths1.Text = "Average Months : " & IIf(Average <> "", Average, "All")
        percent = txtPercentage.Text
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")

        mCofASearchingCriteria = lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + "Period : " + cmbPeriod.SelectedItem.Text + ", " + lblAvgMnths1.Text + ", " + PeriodLimt

    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        'txtFromDate.Value = AsonDate
        If AsonDate <> "" Then
            txtFromDate.Text = AsonDate
        End If
        AsonDate = ""
        AvgMnths = 0
    End Sub

    Public Function ReportDetail() As ReportMaintenanceDetailList

        If rbdPercent.Checked Then mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))

        mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, , AvgMnths, rbdSpecifyValues.Checked, mPerDayLimits, CInt(cmbPeriod.SelectedValue), , , , , , , True, True, True, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            For Each ObjMachine In mMachineList

                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                    LHLabel2 = ""
                    LHData2 = ""
                    For Count = 0 To Periodcount - 1
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                            LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                        End If
                    Next
                    AssemblyID = ObjAssemblyStatus.AssemblyID
                    'ReportStatusList.Add(New rptStatus(AssemblyID.ToString, 0, , ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                    '    "Serial No.", ObjAssemblyStatus.SerialNo, , , , , , , , , , , , , , , , LHLabel2, LHData2))
                Next
            Next
        End If

        'Code Added By Deven on 07/04/2008
        If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
            For i As Integer = 0 To mtmpMachineList.Count - 1
                ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , , , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
            Next
        End If
        '--------------------------------------------


        ReportMaintenanceDetails.Add(mMachineList, Report)
        Return ReportMaintenanceDetails
    End Function

    Private Sub SetReport()

        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail


        Dim mCompanyDetail As New CompanyDetail
        Dim searchstr As String = ""
        Dim OperatorName As String = ""

        SetValues()

        ReportDetail()

        If rbdDueLimits.Checked = True Then
            For Each mDueLimit In mDueLimits
                If CDec(Val(mDueLimit.PeriodLimit)) >= 0 Then
                    If searchstr = "" Then
                        searchstr = "For Next" & " " & searchstr & " " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                    Else
                        searchstr = searchstr & ", " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                    End If
                End If
            Next
        Else
            searchstr = "For Next" & " " & CDec(Val(txtPercentage.Text)).ToString & "% of Frequency"
        End If

        'Added By Rajnish on 26-11-2007
        searchstr = searchstr & ", " & "As On Date:" & New SmartDate(txtFromDate.Text.ToString).FormattedText
        '------------------------------

        'code added By Deven on 11-04-2008 ====================
        Dim searchstr1 As String
        Dim mPerDayLimit As PerDayLimit
        If rbdSpecifyValues.Checked = True Then
            For Each mPerDayLimit In mPerDayLimits
                If CDec(Val(mPerDayLimit.PeriodLimit)) >= 0 Then
                    If searchstr1 = "" Then
                        searchstr1 = "Estimated Due Date as" & " " & searchstr1 & " " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                    Else
                        searchstr1 = searchstr1 & ", " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                    End If
                End If
            Next
            searchstr1 = searchstr1 & " per Day "
        Else
            searchstr1 = "Estimated Due Date as Per Average of Last" & " " & CDec(Val(txtAvgMnths.Text)).ToString & " Months"
        End If
        '===========================================
        'Code Added By Deven on 07/04/2008------------
        Dim rptDueDetail As CrystalDecisions.CrystalReports.Engine.ReportClass
        If DueType = 1 Then
            If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
                rptDueDetail = New crPeriodDueReportDetailLandscapePerAircraft
            Else
                rptDueDetail = New crPeriodDueReportDetailLandscapePerAircraft
            End If
        Else
            rptDueDetail = New crPeriodDueReportDetailLandscapePerAircraft
        End If
        '-------------------------------------------
        Dim str1 As String
        If cmbPeriod.SelectedValue = 1 Then
            str1 = "Hrs"
        ElseIf cmbPeriod.SelectedValue = 2 Then
            str1 = "Days"
        ElseIf cmbPeriod.SelectedValue = 3 Then
            str1 = "Cyls"
        ElseIf cmbPeriod.SelectedValue = 4 Then
            str1 = "NgC"
        ElseIf cmbPeriod.SelectedValue = 5 Then
            str1 = "NfC"
        ElseIf cmbPeriod.SelectedValue = 6 Then
            str1 = "RINS"
        ElseIf cmbPeriod.SelectedValue = 7 Then
            str1 = "Lndg"
        ElseIf cmbPeriod.SelectedValue = 8 Then
            str1 = "Starts"
        ElseIf cmbPeriod.SelectedValue = 9 Then
            str1 = "AC"
        ElseIf cmbPeriod.SelectedValue = 10 Then
            str1 = "CR"
        ElseIf cmbPeriod.SelectedValue = 11 Then
            str1 = "Bl"
        ElseIf cmbPeriod.SelectedValue = 12 Then
            str1 = "IC"
        ElseIf cmbPeriod.SelectedValue = 13 Then
            str1 = "CTC"
        ElseIf cmbPeriod.SelectedValue = 14 Then
            str1 = "PTC"
        ElseIf cmbPeriod.SelectedValue = 15 Then
            str1 = "GM"
        End If

        'Added by vikrant on 11-aug-2011
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))

            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName

        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, cmbPeriod.SelectedItem.Text & "wise Due Report", searchstr, searchstr1, "", "", str1, mModuleList.Item("Due - Per Period").FormRevisionNo, AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"))  'Changed By Utkarsh For Report Logo.
        'Replace  AppSettings("Product Version") by mModuleList.Item("Due - Per Period").FormRevisionNo for Suhan

        If ReportMaintenanceDetails.Count = 0 Then
            '''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            '''msg1.ReplacePage = "wfSearchCriteriaForDuePeriod.aspx?Backpage=" & "&DueType=" & DueType
            '''msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1120)
        End If

        '11-Sep-2008-------------------------------
        If Not mIsPreview Then
            ds.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, ReportMaintenanceDetails)
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            rptDueDetail.SetDataSource(ds)
            Session("CrystalReport") = rptDueDetail

            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "Due - Per Period", mCofASearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            ResetValues()

            'Saving Periods Limits
            Try
                SetGridObject()
                mDueLimits = CType(mDueLimits.Save, DueLimits)
                Session("mDueLimits") = mDueLimits
                DataFieldBind()
                ControlVisibility()
            Catch ex As Exception
                '
            End Try
        Else
            Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)

            reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
                                     Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                     Select c).ToList
            Session("reportmaintdetailslist") = reportmaintdetailslist
            Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
            Dim str As String
            str = "openledgersame('wfDueResult_Ajax.aspx?');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScript", str, True)
            MarkLog(Util.Action.Print, "Due - Per Period", mCofASearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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

#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mDueLimits = DueLimits.GetDueLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdvDuePeriodLimits.DataSource = mDueLimits
        Session("mDueLimits") = mDueLimits
        upnlDueLimits.Update()
        mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdvPerDayLimit.DataSource = mPerDayLimits
        Session("mPerDayLimits") = mPerDayLimits
        upnlAvrgperiod.Update()

        cmbPeriod.DataSource = mDueLimits

        DataBind()
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>", SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        cmbAircraft.DataBind()
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim str As String = ""
        Dim Childs As Integer
        Dim child As DueLimit
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetGridObject()
        If Not mDueLimits.IsValid Then
            For Childs = 0 To mDueLimits.Count - 1
                child = mDueLimits(Childs)
                For i As Integer = 0 To child.GetBrokenRulesCollection.Count - 1
                    str = str + child.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            Next
        End If

        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Private Sub ControlVisibility()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            If rbdDueLimits.Checked Then
                txtLimit.Enabled = True
            ElseIf rbdPercent.Checked Then
                txtLimit.Enabled = False
            End If
        Next i
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DueType = Request.QueryString("DueType")
            Session("DueType") = DueType
            Session("MiddleFrame") = "wfSearchCriteriaForDuePeriod_AJAX.aspx?DueType=" & DueType
            ResetValues()
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            DataFieldBind()
            Report = 1
        End If
        ''addAttributes()
        SetSession()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then Exit Sub

        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mDueLimits = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text.ToString
        If AOnDate = AOdate Then
        Else
            SetComboOfMachine(AOdate)
        End If
    End Sub

    Private Sub txtAvgMnths_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAvgMnths.TextChanged
        If txtAvgMnths.Text = "" Then
        Else
            AvgMnths = CInt(txtAvgMnths.Text)
            Session("AvgMnths") = AvgMnths
        End If
    End Sub
    Private Sub rbdPercent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdPercent.CheckedChanged
        txtPercentage.Enabled = True
        txtPercentage.Text = "10"
        mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = False
        Next i
    End Sub
    Private Sub rbdDueLimits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdDueLimits.CheckedChanged
        txtPercentage.Enabled = False
        txtPercentage.Text = ""
        mDueLimits.UnSetPercentageWise()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = True
        Next i
    End Sub
    '11-Sep-2008--------------------
    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mIsPreview = True
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    '-------------------------------
    Private Sub rbdAvrageMonths_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdAvrageMonths.CheckedChanged
        lblAvgMnths.Visible = True
        txtAvgMnths.Visible = True
        lblMonths.Visible = True
        pnlAvragePeriod.Visible = False
        lblInfo.Visible = False
    End Sub

    Private Sub rbdSpecifyValues_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdSpecifyValues.CheckedChanged
        lblAvgMnths.Visible = False
        txtAvgMnths.Visible = False
        lblMonths.Visible = False
        pnlAvragePeriod.Visible = True
        lblInfo.Visible = True
    End Sub

    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        DataFieldBind()
        ControlVisibility()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class