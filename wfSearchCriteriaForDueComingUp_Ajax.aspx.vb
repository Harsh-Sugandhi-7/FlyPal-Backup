'Created By : Saylee
'Dated      : 29-Aug-2018


Imports System.Linq
Imports System.Collections.Generic
Imports System.Text

Public Class wfSearchCriteriaForDueComingUp_Ajax
    Inherits System.Web.UI.Page

    
#Region " Variable Declaration "
    Dim mDueLimits As DueLimits
    Dim mPerDayLimits As PerDayLimits
    Dim ReportStatusList As New rptStatusList
    Dim mMachineNameValueList As MachineNameValueList
    Dim mtmpMachineList As tmpMachineList
    Private mIsPreview As Boolean = False
    Dim AircraftIndex As Integer
    Dim mAssemblyList As AssemblyList
    Dim AssemblyName As String
    Dim AsonDate As String
    Dim AssemblyType As String
    Dim Assembly1 As String
    Dim MachineName As String
    Dim TypeName As String
    Dim AOnDate As String
    Dim AOdate As String
    Dim mServiceTypeList As PartMonitorServiceTypeList
    Dim mInspectionTypeList As ModelMonitorInspTypeList
    Dim mModificationTypeList As ModelMonitorModTypeList
    Dim InspIndex As Integer
    Private Flag As Int16
    Dim SerIndex As Integer
    Dim ModIndex As Integer
    Dim AvgMnths As Integer
    Dim Average As String
    Private percent As String
    Dim Aircraft As String
    Dim IsSerSelect As Boolean = False
    Dim IsModSelect As Boolean = False
    Dim IsInsSelect As Boolean = False
    Dim mDueLimit As DueLimit
    Dim ServiceTypeID(50) As Integer
    Dim InspectionTypeID(50) As Integer
    Dim ModificationTypeID(50) As Integer
    Dim searchstr7 As String = ""
    Dim mEventLogDetails As String = String.Empty
    Dim mIsExcel As Boolean
    Dim PerDayLimitForDaysPeriod As Integer = -1
    Dim mCompanyDetail As New CompanyDetail

    Private mFAScsReportList As FAScsReportList

    Dim mrptModelMonitorDueStatusList As rptModelMonitorDueStatusList
    Dim MonitorServiceTypeIDs As String = ""
    Dim MonitorInspTypeIDs As String = ""
    Dim MonitorModTypeIDs As String = ""
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

    Dim ChkRegNos As String()
    Dim MachineIDs As String = ""

#End Region

#Region " Helper Methods "
    Private Sub addAttributes()
        txtAvgMnths.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAvgMnths').value,event)")
        txtForecastingLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtForecastingLimit').value,event)")
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            'mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text) 'Added by Saylee on 12-Nov-2012
            'Added By Vikrant On 14-Jan-2016 For ALL14012016
            If mDueLimits.Item(i).PeriodID = 2 Then
                PerDayLimitForDaysPeriod = CInt(IIf(mDueLimits.Item(i).PeriodLimit <> "", mDueLimits.Item(i).PeriodLimit, 0))
            End If
            'End
        Next i
        Session("mDueLimits") = mDueLimits

        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdvPerDayLimit.Rows.Count - 1
            txtPerDatLimit = CType(Me.gdvPerDayLimit.Rows(i1).FindControl("txtLimitPerDay"), TextBox)
            'mPerDayLimits.Item(i1).PeriodLimit = CDec(Val(Trim(txtPerDatLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mPerDayLimits.Item(i1).PeriodLimit = Trim(txtPerDatLimit.Text)  'Added by Saylee on 12-Nov-2012
        Next i1
        Session("mPerDayLimits") = mPerDayLimits

    End Sub
    Private Sub GetSession()
        mDueLimits = CType(Session("mDueLimits"), DueLimits)
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits)

        AOnDate = Session("AOnDate")
      
        AvgMnths = Session("AvgMnths")

        'Added by Saylee on 12-Feb-2009
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)

        mMachineNameValueList = Session("mMachineNameValueList")
        mCompanyDetail = Session("mCompanyDetail")
        mFAScsReportList = Session("mFAScsReportList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mDueLimits") = mDueLimits
        Session("mPerDayLimits") = mPerDayLimits
        Session("AOnDate") = AOnDate
     
        Session("AvgMnths") = AvgMnths

        Session("mAssemblyList") = mAssemblyList
        Session("SerIndex") = SerIndex
        Session("InspIndex") = InspIndex
        Session("ModIndex") = ModIndex
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("mModificationTypeList") = mModificationTypeList

        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub ClearAll()

        If Session("MiddleFrame") <> "wfSearchCriteriaForDueComingUp_Ajax.aspx?" Then

            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("AvgMnths")

            Session.Remove("mAssemblyList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")

            Session.Remove("mMachineNameValueList")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("mModificationTypeList")
            Session.Remove("mFAScsReportList")
        End If
        mIsExcel = False
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblAvgMnths1.Visible = True
        lblDateRangeFrom.Visible = True
        lblPercent.Visible = True
        lblAssembly1.Visible = True
        ''lblType1.Visible = True
        upnlSearchingCriteria.Update()
    End Sub
    Private Sub SetValues()
        ''If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
        ''    MachineName = "{00000000-0000-0000-0000-000000000000}"
        ''    AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        ''    Assembly1 = ""
        ''    lblAssembly1.Text = ""
        ''Else
        ''    MachineName = cmbAircraft.SelectedValue.ToString

        ''    'Added by Saylee on 12-Feb-2009
        ''    If cmbAssembly.SelectedItem.Text = "(All)" Then
        ''        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        ''        Assembly1 = ""
        ''        AssemblyType = "(All)"
        ''        lblAssembly1.Text = "Assembly Name  : " + "<b> All </b>"         'Added Code
        ''    Else
        ''        AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
        ''        AssemblyName = cmbAssembly.SelectedValue.ToString
        ''        Assembly1 = cmbAssembly.SelectedItem.Text
        ''        lblAssembly1.Text = "Assembly Name : " & "<b>" + Assembly1 + "</b>"  'Added Code
        ''    End If
        ''End If


        ChkRegNos = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
                     Where c.Selected = True
                     Select (c.Value)).ToArray

        If ChkRegNos.Length > 0 Then
            For i As Integer = 0 To ChkRegNos.Length - 1
                If MachineIDs = "" Then
                    MachineIDs = ChkRegNos(i).ToString
                Else
                    MachineIDs = MachineIDs + "," + ChkRegNos(i).ToString
                End If

            Next
        End If

        If ChkRegNos.Count = 1 Then
            MachineName = ChkRegNos(0).ToString
            If cmbAssembly.SelectedItem.Text = "(All)" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : " + "<b> All </b>"         'Added Code
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & "<b>" + Assembly1 + "</b>"  'Added Code
            End If

            Aircraft = ListRegNo.SelectedItem.Text
            lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", "<b>" + Aircraft + "</b>", "All")
        Else
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
            Aircraft = ""
            lblAircraft1.Text = ""
        End If


        Average = txtAvgMnths.Text
        If Not IsDate(txtFromDate.Text.Trim) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text.Trim
        End If
        ''Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        '' TypeName = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedItem.Text, "")

        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & "<b>" + txtFromDate.Text.Trim + "</b>"
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If
        '' lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", "<b>" + Aircraft + "</b>", "All")
        lblAvgMnths1.Text = "Average Months : " & IIf(Average <> "", Average, "All")
        percent = txtPercentage.Text
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")
        ''lblType1.Text = "Type : " & IIf(TypeName <> "", TypeName, "All")

        'Set Service/Inspection/Directive checkbox list values
        'Service
        If chkService.Checked Then
            IsSerSelect = True
            ServiceTypeID = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
                         Where c.Selected = True
                         Select CInt(c.Value)).ToArray


            For i As Integer = 0 To ServiceTypeID.Length - 1
                If MonitorServiceTypeIDs = "" Then
                    MonitorServiceTypeIDs = ServiceTypeID(i).ToString
                Else
                    MonitorServiceTypeIDs = MonitorServiceTypeIDs + "," + ServiceTypeID(i).ToString
                End If

            Next
        End If
        'Inspection
        If chkInspection.Checked Then
            IsInsSelect = True

            InspectionTypeID = (From c In ListInspectionType.Items
                         Where c.Selected = True
                         Select CInt(c.Value)).ToArray

            For i As Integer = 0 To InspectionTypeID.Length - 1
                If MonitorInspTypeIDs = "" Then
                    MonitorInspTypeIDs = InspectionTypeID(i).ToString
                Else
                    MonitorInspTypeIDs = MonitorInspTypeIDs + "," + InspectionTypeID(i).ToString
                End If

            Next
        End If
        'Directive
        If chkDirective.Checked Then
            IsModSelect = True
            ModificationTypeID = (From c In ListDirectiveType.Items
                         Where c.Selected = True
                        Select CInt(c.Value)).ToArray

            For i As Integer = 0 To ModificationTypeID.Length - 1
                If MonitorModTypeIDs = "" Then
                    MonitorModTypeIDs = ModificationTypeID(i).ToString
                Else
                    MonitorModTypeIDs = MonitorModTypeIDs + "," + ModificationTypeID(i).ToString
                End If

            Next
        End If
        'End

        'If cmbType.Items.Item(x).ToString = "All" Then
        '    IsSerSelect = True
        '    IsInsSelect = True
        '    IsModSelect = True
        '    ServiceTypeID(0) = 0
        '    InspectionTypeID(0) = 0
        '    ModificationTypeID(0) = 0
        'End If
        Dim DueLimits As String = String.Empty
        Dim EstimatedFlyingHours As String = String.Empty
        Dim status As String = String.Empty
        Dim Format As String = String.Empty
        'Due Limits
        status = IIf(rbdDueLimits.Checked, rbdDueLimits.Text, rbdPercent.Text)
        If rbdDueLimits.Checked Then
            DueLimits = status & " : " & String.Join(", ", (From c As DueLimit In mDueLimits
                        Select c.PeriodName & " : " & c.PeriodLimitFormatted).ToArray)
        Else
            DueLimits = status & " : " & txtPercentage.Text.Trim
        End If
        'Estimated Flying Hours
        status = IIf(rbdAvrageMonths.Checked, rbdAvrageMonths.Text, rbdSpecifyValues.Text)
        If rbdSpecifyValues.Checked Then
            EstimatedFlyingHours = status & " : " & String.Join(", ", (From c As PerDayLimit In mPerDayLimits
                        Select c.PeriodName & " : " & c.PeriodLimitFormatted).ToArray)
        Else
            EstimatedFlyingHours = status & " : " & txtAvgMnths.Text.Trim
        End If

        mEventLogDetails = lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + DueLimits + ", " + EstimatedFlyingHours


    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        'txtFromDate.Value = AsonDate
        If AsonDate <> "" Then
            txtFromDate.Text = Format(AsonDate, AppSettings("DateFormat"))
        End If
        AsonDate = ""
        AvgMnths = 0

        IsSerSelect = False
        IsInsSelect = False
        IsModSelect = False
        ServiceTypeID(0) = 0
        InspectionTypeID(0) = 0
        ModificationTypeID(0) = 0
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        btnDisplay.Enabled = True
    End Sub

    Public Sub ReportDetail(IsExcel As Boolean, Optional ByVal IsPreviewClicked As Boolean = False)
        Try
            If rbdPercent.Checked Then mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))

            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""


            If Not cmbAircraft.SelectedItem.ToString = "(All)" Or
               AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "AAP" Then   ''Or AppSettings("ClientCode") = "APFT" Added by Saylee on 20-Aug-2018 for ALL20082018,common report to show Current values
                mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
                Dim mOtherPeriodExists As String = "False"

                For i As Integer = 0 To mtmpMachineList.Count - 1
                    If mtmpMachineList(i).AllPeriods <> "" Then
                        mOtherPeriodExists = "True"
                        Exit For
                    End If
                Next

                For i As Integer = 0 To mtmpMachineList.Count - 1
                    searchstr7 = mtmpMachineList(i).Owner.ToString  ' Changed By Utkarsh On 11-Apr-2011 '"Owner/Operator :- " +
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , mtmpMachineList(i).TSO, , mtmpMachineList(i).CSO, , , , , , , , , mtmpMachineList(i).Cycles, mtmpMachineList(i).AllPeriods.Replace("<BR>", vbCrLf), mOtherPeriodExists, Year(txtFromDate.Text).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                    Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted
                Next
            End If


        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (ReportDetail): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try

    End Sub
    Private Sub SetExcel(mrptModelMonitorDueStatusList As rptModelMonitorDueStatusList, SearchingCriteria As ReportData, ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsrptModelMonitorDueStatusList

        Dim reportmaintdetailslist As List(Of rptModelMonitorDueStatusList.rptModelMonitorDueStatusListInfo) = New List(Of rptModelMonitorDueStatusList.rptModelMonitorDueStatusListInfo)

        reportmaintdetailslist = (From c As rptModelMonitorDueStatusList.rptModelMonitorDueStatusListInfo In mrptModelMonitorDueStatusList.AsParallel
                                 Order By c.MininumRemainingValue, c.RegNo, c.AssemblyType, c.AssemblyModel, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                 Select c).ToList
        Session("mrptModelMonitorDueStatusList") = mrptModelMonitorDueStatusList
        Session("reportmaintdetailslist") = reportmaintdetailslist

        'da.Fill(ds, "rptModelMonitorDueStatusList", ReportMaintenanceDetails)
        da.Fill(ds, "rptModelMonitorDueStatusList", reportmaintdetailslist)
        da.Fill(ds, "ReportData", SearchingCriteria)

        Dim columnToRemove As String() = { _
                                                  "ID", _
                                                  "ModificationNumber", _
                                                  "Position", _
                                                  "AssemblyType", _
                                                  "RegNo", _
                                                  "AssemblyID", _
                                                  "AssemblyStatusID",
                                                  "AssemblySerialNo", _
                                                  "AssemblyModel", _
                                                  "MonitorType", _
                                                  "Reference", _
                                                  "ModelMonitorInspTypeID", _
                                                  "HourType", _
                                                  "DoneOn", _
                                                  "DoneOnFormatted", _
                                                  "PeriodName", _
                                                  "PeriodNameForWeb", _
                                                  "PeriodID", _
                                                  "PeriodUnitID", _
                                                  "PeriodUnitName", _
                                                  "PeriodUnitNameForWeb", _
                                                  "RemainingValueDec", _
                                                  "ModelMonitorID", _
                                                  "IsApplicable", _
                                                  "MininumRemainingValue", _
                                                  "DueStatus", _
                                                  "MonitorTypeID", _
                                                  "ModNumber", _
                                                  "IsDue", _
                                                  "IsComingUpDueExists", _
                                                  "CompSerialNo", _
                                                  "PartNo", _
                                                  "MonitorTypeName", _
                                                  "MaintenanceEvent", _
                                                  "CompStatusID",
                                                  "mIsComingUpDueExists", _
                                                  "mIsDue", _
                                                  "EstimatedDateFormatted", _
                                                  "ModelMonitorInspID", _
                                                  "AirframeDueAsofValue", _
                                                  "AssemblyCurrentValue", _
                                                  "Description", _
                                                  "FrequencyValue", _
                                                  "CurrentValue", _
                                                  "DueOnValue", _
                                                  "ElapsedValue", _
                                                  "RemainingValue", _
                                                  "AssemblyDueAsofValue", _
                                                  "AirframeDueAsofValue" _
                                    }

        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptModelMonitorDueStatusList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptModelMonitorDueStatusList").Columns.Remove(columnToRemove(i))
            End If
        Next
        Dim columnscnt As Integer = ds.Tables("rptModelMonitorDueStatusList").Columns.Count

        'set Column Sequence
        ds.Tables("rptModelMonitorDueStatusList").Columns("MaintenanceOnExcel").SetOrdinal(0)
        ds.Tables("rptModelMonitorDueStatusList").Columns("MaintenanceInfoExcel").SetOrdinal(1)
        ds.Tables("rptModelMonitorDueStatusList").Columns("FrequencyValueExcel").SetOrdinal(2)
        ds.Tables("rptModelMonitorDueStatusList").Columns("CurrentValueExcel").SetOrdinal(3)
        ds.Tables("rptModelMonitorDueStatusList").Columns("ElapsedValueExcel").SetOrdinal(4)
        'ds.Tables("rptModelMonitorDueStatusList").Columns("DoneOnFormatted").SetOrdinal(5)
        'ds.Tables("rptModelMonitorDueStatusList").Columns("ExtensionValueExcel").SetOrdinal(7)
        ds.Tables("rptModelMonitorDueStatusList").Columns("DueOnValueExcel").SetOrdinal(8)
        ds.Tables("rptModelMonitorDueStatusList").Columns("AssemblyDueAsofValue").SetOrdinal(9)
        ds.Tables("rptModelMonitorDueStatusList").Columns("RemainingValueExcel").SetOrdinal(10)
        ds.Tables("rptModelMonitorDueStatusList").Columns("Note").SetOrdinal(12)
        ds.Tables("rptModelMonitorDueStatusList").Columns("Remark").SetOrdinal(13)


        Dim ColumnName As String = String.Empty
        For i As Integer = 0 To ds.Tables("rptModelMonitorDueStatusList").Columns.Count - 1
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "ModNumber" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Directive No"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "FrequencyValueExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Frequency"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "CurrentValueExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Since New"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "ElapsedValueExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Elapsed"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "DueOnValueExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Due At"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "AssemblyDueAsofValueExcel" Then
                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                    ColumnName = "Due At Airframe"
                    ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = ColumnName
                Else
                    ColumnName = "Due At Assembly"
                    ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = ColumnName
                End If

            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "RemainingValueExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Remaining"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "DoneOnFormatted" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Done At"
            End If

            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "ExtensionValueExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Extension"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "MaintenanceOnExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Maintenance On"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "MaintenanceInfoExcel" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Maintenance Info"
            End If
            If ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "EstimatedDate" Then
                ds.Tables("rptModelMonitorDueStatusList").Columns(i).ColumnName = "Estimated Date"
            End If
          
        Next
        Dim columnToRemoveCriteria As String() = { _
                                                 "ReportDate", _
                                                 "ID", _
                                                 "CompanyName", _
                                                 "Address", _
                                                 "Tel1", _
                                                 "Tel2", _
                                                 "Fax", _
                                                 "Email", _
                                                 "WebSite", _
                                                 "ReportName", _
                                                 "SearchStr5", _
                                                 "SearchStr7", _
                                                 "SearchStr9", _
                                                 "ProductVersion", _
                                                 "SINote", _
                                                 "CurrencyName", _
                                                 "CurrencySymbol", _
                                                 "SearchStr10", _
                                                 "SearchStr4", _
                                                 "SearchStr12", _
                                                 "SearchStr11", _
                                                 "ShortName", _
                                                 "SearchStr15", _
                                                 "SearchStr16", _
                                                 "SearchStr17", _
                                                 "SearchStr18", _
                                                 "SearchStr19", _
                                                 "SearchStr20", _
                                                 "SearchStr21", _
                                                 "SearchStr22", _
                                                 "SearchStr23", _
                                                 "SearchStr24", _
                                                 "SearchStr25" _
                                            }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        'set Column Sequence
        ds.Tables("ReportData").Columns("SearchStr14").SetOrdinal(0)
        ds.Tables("ReportData").Columns("SearchStr13").SetOrdinal(1)
        ds.Tables("ReportData").Columns("SearchStr3").SetOrdinal(2)
        ds.Tables("ReportData").Columns("SearchStr1").SetOrdinal(3)
        ds.Tables("ReportData").Columns("SearchStr2").SetOrdinal(4)
        ds.Tables("ReportData").Columns("SearchStr6").SetOrdinal(5)
        ds.Tables("ReportData").Columns("SearchStr8").SetOrdinal(6)


        For i As Integer = 0 To ds.Tables("ReportData").Columns.Count - 1
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Due Limit"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Average Months"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr13" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Reg No."
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr14" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "As On Date"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Assembly"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr6" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Flight Log Updated Till"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr8" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Last Maintenance Done On"
            End If
        Next
        'Dim dataview As DataView = ds.Tables("rptModelMonitorDueStatusList").DefaultView
        'dataview.Sort = "MinimumRemainingValue"


        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Merge(ds.Tables("rptModelMonitorDueStatusList"))


        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("rptModelMonitorDueStatusList").TableName = ReportName
        Session("DataTableToBeFormattedForExportToExcel") = ReportName
		Session("ExcelFileName") = ReportName.Replace("/", " ")
		PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "Since New", "Elapsed", "Remaining", "Due At", "Done At", "Effective From", "AssemblySerialNo", "Maintenance On", ColumnName, "Extension", "Maintenance Info"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "Due-Periodwise With Coming UP", "Export To Excel " + mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal ByExcel As Boolean = False, Optional ByVal IsPreviewClicked As Boolean = False)
        Try
            ReportStatusList = New rptStatusList
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsReportMaintenanceDetail
            Dim rptMachineCertificates As MachineCertificateList
            ''Dim rptSnagCorrectiveActionListForDue As SnagCorrectiveActionListForDue   'Added By Prashant 20-Nov-2009
            'Dim rptDueDetail As New crDueReportDetailPortrait

            Dim mCompanyDetail As New CompanyDetail
            Dim searchstr As String = ""
            Dim searchstr6 As String = ""
            Dim searchstr8 As String = ""
            Dim OperatorName As String = ""


            SetValues()
            mDueLimits = CType(mDueLimits.Save, DueLimits)
            Session("mDueLimits") = mDueLimits

            mrptModelMonitorDueStatusList = rptModelMonitorDueStatusList.GetDueStatusList(AsonDate, New Guid(MachineName), New Guid(AssemblyName), mDueLimits, _
                                           Val(txtForecastingLimit.Text.Trim), MonitorInspTypeIDs, , MonitorServiceTypeIDs, , MonitorModTypeIDs, , IsInsSelect, _
                                           IsSerSelect, IsModSelect, True, Val(txtAvgMnths.Text), rbdSpecifyValues.Checked, mPerDayLimits, MachineIDs:=MachineIDs)

            Dim mSpareListforDueRecords As SpareListforDueRecords

            If mrptModelMonitorDueStatusList.Count > 0 Then
                mSpareListforDueRecords = SpareListforDueRecords.GetList(AsonDate, mrptModelMonitorDueStatusList)
            End If


            'Code Added by Deven on 02-Mar-20098*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/***/*/*/*/*/*/*/*/
            Dim x As String
            x = txtFromDate.Text.Trim
            If Aircraft <> "" Then

                Dim mloglist As LogList
                'mloglist = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), , AsonDate)
                mloglist = LogList.GetLogList(New Guid(ListRegNo.SelectedValue.ToString), , AsonDate)

                '-------------------------------------------

                If mloglist.Count > 0 Then
                    x = mloglist(0).LogDate.ToShortDateString
                Else
                    x = txtFromDate.Text.Trim
                End If

                '--------------------------------------------------------
            End If
          
            '*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/***/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/

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
            searchstr = searchstr & ", " & "As On Date:" & txtFromDate.Text.Trim
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
                If CDec(Val(txtAvgMnths.Text)).ToString <> "" Then
                    searchstr1 = "Estimated Due Date as Per Average of Last" & " " & CDec(Val(txtAvgMnths.Text)).ToString & " Months"
                Else
                    searchstr1 = ""
                End If
            End If
            '===========================================
            Dim ReportName As String
            Dim ReportNameForPDF As String
            'Code Added By Deven on 07/04/2008------------
            Dim rptDueDetail As CrystalDecisions.CrystalReports.Engine.ReportClass

          
                'NextCode:
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                'If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
                If Aircraft = "" Then
                    ReportName = "Work Order List"
                    ReportNameForPDF = "Work Order List"
                Else
                    ' ReportName = "Work Order List Number " + "__________________________" + " / " + cmbAircraft.SelectedItem.Text + " / " + MonthName(Month(New SmartDate(txtFromDate.Text.Trim).FormattedText), True).ToString + "." + " / " + Year(New SmartDate(txtFromDate.Text.Trim).FormattedText).ToString + " ."
                    ReportName = "Work Order List Number " + "__________________________" + " / " + Aircraft + " / " + MonthName(Month(New SmartDate(txtFromDate.Text.Trim).FormattedText), True).ToString + "." + " / " + Year(New SmartDate(txtFromDate.Text.Trim).FormattedText).ToString + " ."

                    ReportNameForPDF = "Work Order List"
                End If
            ElseIf ((AppSettings("ClientCode") = "Heligo")) Then

                ReportName = "Weekly Call Out"
                ReportNameForPDF = "Weekly Call Out"
            Else
                ReportName = "Maintenance Status Report"
                ReportNameForPDF = "Maintenance Status Report"
            End If

            
            Dim LastFlownDate As String = ""
            Dim LastMaintenanceActivityDate As String = ""
            Dim mMaxLogNo As MaxLogNo = MaxLogNo.GetMaxLogNo(AsonDate, New Guid(MachineName), New Guid(AssemblyName))

            If mMaxLogNo.Count <> 0 Then
                LastFlownDate = mMaxLogNo(0).LogDate.ToString 'Last Flight Log Date
            Else
                LastFlownDate = CType(Session("AircraftAsOnDate"), String)  'New SmartDate(txtFromDate.Value.ToString).FormattedText
            End If

            'Added by Saylee on 2-Aug-2011
            ''Last Maintenance Activity
            ' If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            If Aircraft <> "" Then
                Dim mLastMachineMaintenanceActivity As LastMachineMaintenanceActivity = LastMachineMaintenanceActivity.GetLastMaintenanceActivity(AsonDate, New Guid(MachineName), New Guid(AssemblyName))
                If Not mLastMachineMaintenanceActivity.ID.Equals(Guid.Empty) Then
                    LastMaintenanceActivityDate = ", Last Maintenance Done on  " + "( " + mLastMachineMaintenanceActivity.Date.ToString + " )"
                    searchstr8 = mLastMachineMaintenanceActivity.Date.ToString
                End If
                ''***************************************
            End If

            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022
                searchstr6 = "Flying Hours updated till " + "( " + LastFlownDate + " ) " + LastMaintenanceActivityDate + " & Work Order Number - _______________________"
            Else
                searchstr6 = LastFlownDate 'Mostly on Heligo Report
            End If

            'Added by vikrant on 11-Aug-2011
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                If Aircraft <> "" Then
                    Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(ListRegNo.SelectedValue))
                    If cmbAircraft.SelectedIndex > 0 Then
                        If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
                    End If
                End If
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                OperatorName = searchstr7
            End If
            '--------------------------------------------------------
            Dim ReferenceNo As String = Trim(txtRefNo.Text) 'Added by Vikrant For HLI11102011 
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, ReportName, searchstr, searchstr1, Assembly1, AppSettings("ClientCode"), "Aircraft is flown up to: " & New SmartDate(x).FormattedText, AppSettings("Product Version"), AppSettings("SINote"), searchstr6, OperatorName, searchstr8, ReferenceNo, AppSettings("Logo"), AppSettings("FormNo"), mModuleList.Item("Due-Periodwise With Up-Coming").FormRevisionNo, SearchStr13:=Aircraft, SearchStr14:=txtFromDate.Text)
            'Replace  AppSettings("RevisionNo") with  mModuleList.Item("Due-Periodwise With Up-Coming").FormRevisionNo by Shital

            If ByMail = False Then
                If mrptModelMonitorDueStatusList.Count = 0 Or mrptModelMonitorDueStatusList.IsDueExists = False Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1407)

                End If
            End If
            If (ByMail = True And mrptModelMonitorDueStatusList.Count <= 0) Or mrptModelMonitorDueStatusList.IsDueExists = False Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportNameForPDF, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                     SmtpHost:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpHost, SmtpPort:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpPort, _
                SmtpUser:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpUser, SmtpPassword:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpPassword)
                Exit Sub
            End If

            '11-Sep-2008-------------------------------
            Dim dsDue As New dsrptModelMonitorDueStatusList
            If Not mIsPreview Then
                ds.Clear()
                dsDue.Clear()
                Dim mrptImage As rptImage

                ' If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
                If Aircraft <> "" Then
                    ' If rptMachineCertificates.Count <> 0 Then da.Fill(ds, rptMachineCertificates)
                Else
                    If MachineName = "{00000000-0000-0000-0000-000000000000}" Then
                        'Added by Saylee on 02-Aug-2018 for showing Due Certificates, when "ALL", ALL03082018
                        rptMachineCertificates = MachineCertificateList.GetMachineCertificateList(Guid.Empty, AsonDate, IsForDue:=True, Days:=IIf(AppSettings("ClientCode") = "Heligo", -1, PerDayLimitForDaysPeriod))
                        If rptMachineCertificates.Count <> 0 Then da.Fill(ds, rptMachineCertificates)
                    End If
                End If

                If Aircraft <> "" Or
                   AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then
                    mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
                    Dim mOtherPeriodExists As String = "False"

                    For i As Integer = 0 To mtmpMachineList.Count - 1
                        If mtmpMachineList(i).AllPeriods <> "" Then
                            mOtherPeriodExists = "True"
                            Exit For
                        End If
                    Next

                    For i As Integer = 0 To mtmpMachineList.Count - 1
                        searchstr7 = mtmpMachineList(i).Owner.ToString  ' Changed By Utkarsh On 11-Apr-2011 '"Owner/Operator :- " +
                        ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , mtmpMachineList(i).TSO, , mtmpMachineList(i).CSO, , , , , , , , , mtmpMachineList(i).Cycles, mtmpMachineList(i).AllPeriods.Replace("<BR>", vbCrLf), mOtherPeriodExists, Year(txtFromDate.Text).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                        Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted
                    Next
                End If

                If mSpareListforDueRecords.Count > 0 Then
                    rptDueDetail = New crDueReportDetailWithSpares

                Else
                    rptDueDetail = New crDueReportDetail

                End If

                mrptImage = rptImage.GetImage(dsDue)
                da.Fill(dsDue, mrptModelMonitorDueStatusList)
                da.Fill(dsDue, Report)
                da.Fill(dsDue, ReportStatusList)
                da.Fill(dsDue, mrptImage)
                da.Fill(dsDue, mSpareListforDueRecords)

                rptDueDetail.SetDataSource(dsDue)

                Session("CrystalReport") = rptDueDetail


                If ByMail Then
                    SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportNameForPDF, lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text, _
                                              "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                     SmtpHost:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpHost, SmtpPort:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpPort, _
                SmtpUser:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpUser, SmtpPassword:=mModuleList.Item("Due-Periodwise With Up-Coming").SmtpPassword)
                ElseIf ByExcel Then
                    SetExcel(mrptModelMonitorDueStatusList, Report, ReportName)
                Else
                    Dim Str As String
                    Str = "openTranDetail();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)
                    MarkLog(Util.Action.Print, "Due-Periodwise With Coming UP", mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                End If

                'ResetValues()

                'Saving Periods Limits
                Try
                    ''SetGridObject()
                    'mDueLimits = CType(mDueLimits.Save, DueLimits)
                    'Session("mDueLimits") = mDueLimits
                    'DataFieldBind()
                    'ControlVisibility()
                Catch ex As Exception
                    '
                End Try
            Else

                Dim reportmaintdetailslist As List(Of rptModelMonitorDueStatusList.rptModelMonitorDueStatusListInfo) = New List(Of rptModelMonitorDueStatusList.rptModelMonitorDueStatusListInfo)

                reportmaintdetailslist = (From c As rptModelMonitorDueStatusList.rptModelMonitorDueStatusListInfo In mrptModelMonitorDueStatusList.AsParallel
                                         Order By c.MininumRemainingValue, c.RegNo, c.AssemblyType, c.AssemblyModel, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                         Select c).ToList

                'Dim reportmaintdetailslist As List(Of rptModelMonitorDueStatusList) = New List(Of rptModelMonitorDueStatusList)

                'reportmaintdetailslist = (From c As rptModelMonitorDueStatusList.rptModelMonitorDueStatusListInfo In mrptModelMonitorDueStatusList.AsParallel
                '                         Order By c.MininumRemainingValue, c.RegNo, c.AssemblyType, c.AssemblyModel, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                '                         Select c).ToList
                Session("mrptModelMonitorDueStatusList") = mrptModelMonitorDueStatusList
                Session("reportmaintdetailslist") = reportmaintdetailslist
                GenerateSearchCriteriaString() 'Added By Vikrant On 03-Jun-2016 For ALL03062016
                'Added By Vikrant on 14-Jun-2018 For ALL14062018
                Session("AsOnDateForWOCreation") = txtFromDate.Text
                '  Session("MachineIDForWOCreation") = cmbAircraft.SelectedValue.ToString
                If Aircraft <> "" Then
                    Session("MachineIDForWOCreation") = ListRegNo.SelectedValue.ToString
                End If

                'End
                Dim str As String
                str = "openledgersame('wfDueResult_Ajax.aspx?');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScript", str, True)
            End If
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
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
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
    Private Sub ControltovisibilityForDetails()

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            Label4.Visible = False
            lblLimit.Visible = False
            txtForecastingLimit.Visible = False
            lblStep7.Text = "Step VIII. Display Report"
            Label5.Text = "Step VII. Format Selection"
        End If
        '---Added by Vikrant For HLI11102011 ---------------
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
            If User.IsInRole("DuePeriodWiseView") = True Then
                lblStep8.Visible = True
                lblRefNo.Visible = True
                txtRefNo.Visible = True
                Label5.Text = "Step IX. Format selection"
                lblStep7.Text = "Step X. Display Report"
            Else
                lblStep8.Visible = False
                lblRefNo.Visible = False
                txtRefNo.Visible = False
            End If
        End If
        '---------------------------------------------------
        upnlDetails.Update()
    End Sub
    Private Sub SetTitle()
        lbltitle.Text = "Search criteria for Due"
        upnlTitle.Update()
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

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Session("mCompanyDetail") = mCompanyDetail
        If mCompanyDetail.ShortName = "TS" Then
            'cmbFormat.Items.Add(New ListItem("Format 3(Enlarge Copy with Limited Columns)", "2"))
        End If

        mFAScsReportList = FAScsReportList.GetFAScsReportList()
        Session("mFAScsReportList") = mFAScsReportList

        DataBind()
        If mCompanyDetail.ShortName = "TS" Then
            'cmbFormat.SelectedIndex = 2
        End If
    End Sub
    Public Sub SetTypeCombo()
        If mServiceTypeList Is Nothing Then
            mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, True)
        End If
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypeList Is Nothing Then
            mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()    ''ModelMonitorInspTypeList.serach.ExludingRoutineInspections)
        End If
        ListInspectionType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList

        If mModificationTypeList Is Nothing Then
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList(, True)
        End If

        ListDirectiveType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()
        FillMonitorTypeList()
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        ''  mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , ,  True, "(All)", , True)
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.Date.ToString(AppSettings("DateFormat")), , , , , , , , , , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()

        ListRegNo.DataSource = mMachineNameValueList
        ListRegNo.DataBind()
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
    Private Sub FillMonitorTypeList()
        chkService.Checked = True
        chkInspection.Checked = True
        chkDirective.Checked = True

        For i As Integer = 0 To ListServiceType.Items.Count - 1
            ListServiceType.Items(i).Selected = True
        Next

        For i As Integer = 0 To ListInspectionType.Items.Count - 1
            ListInspectionType.Items(i).Selected = True
        Next

        For i As Integer = 0 To ListDirectiveType.Items.Count - 1
            ListDirectiveType.Items(i).Selected = True
        Next

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
    'Added By Vikrant On 03-Jun-2016 For ALL03062016
    Private Sub GenerateSearchCriteriaString()
        Dim SearchCriteriaValues As New Hashtable
        SearchCriteriaValues.Add("AsonDate", txtFromDate.Text)
        SearchCriteriaValues.Add("MachineID", MachineName)
        SearchCriteriaValues.Add("DueLimitObj", mDueLimits)
        SearchCriteriaValues.Add("IsrbdPercentChecked", rbdPercent.Checked)
        SearchCriteriaValues.Add("Percentage", Val(txtPercentage.Text))
        SearchCriteriaValues.Add("AssemblyID", AssemblyName)
        SearchCriteriaValues.Add("AverageMonths", AvgMnths)
        SearchCriteriaValues.Add("IsSpecifyValuesChecked", rbdSpecifyValues.Checked)
        SearchCriteriaValues.Add("PerDayLimitsObj", mPerDayLimits)
        SearchCriteriaValues.Add("IsServiceRequired", IsSerSelect)
        SearchCriteriaValues.Add("IsModRequired", IsModSelect)
        SearchCriteriaValues.Add("IsInspRequired", IsInsSelect)
        SearchCriteriaValues.Add("ForDueStatus", Val(txtForecastingLimit.Text))
        SearchCriteriaValues.Add("SelectedAircraftText", cmbAircraft.SelectedItem.ToString)
        SearchCriteriaValues.Add("ServiceTypeID", ServiceTypeID)
        SearchCriteriaValues.Add("InspectionTypeID", InspectionTypeID)
        SearchCriteriaValues.Add("ModificationTypeID", ModificationTypeID)
        SearchCriteriaValues.Add("Aircraft", IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, ""))


        Session("SearchCriteriaValues") = SearchCriteriaValues
    End Sub
    'End
#End Region

#Region "Eventes"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfSearchCriteriaForDueComingUp_Ajax.aspx?"
            ResetValues()
            ''SetFocus(txtFromDate)
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            setFocus(cmbAircraft)
            DataFieldBind()
            SetTypeCombo()

            ControltovisibilityForDetails()
            'ControlvisibilityForAvgPeriod()
            rbdAvrageMonths.Checked = True
            SetSession()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetValues()

            If MachineIDs.ToString = "" Then
                MSGBoxCtrl.Show("Alert..!!", "Please Select atlease one Aircraft.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            mIsExcel = False
            SetReport(, mIsExcel)
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

        mDueLimits = Nothing
        mAssemblyList = Nothing
        'Added By Saylee on 20-Feb-2009
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mModificationTypeList = Nothing
        '=============================
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
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
        upnlDueLimits.Update()
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
        upnlDueLimits.Update()
    End Sub
    '11-Sep-2008--------------------
    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        mIsPreview = True
        If IsValid = True Then
            SetReport(IsPreviewClicked:=True)
        Else
            upnlValidations.Update()
        End If
    End Sub
    '-------------------------------
    Private Sub rbdAvrageMonths_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdAvrageMonths.CheckedChanged
        lblAvgMnths.Visible = True
        txtAvgMnths.Visible = True
        lblMonths.Visible = True
        pnlAvragePeriod.Visible = False
        lblInfo.Visible = False
        upnlAvrgperiod.Update()
    End Sub
    Private Sub rbdSpecifyValues_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdSpecifyValues.CheckedChanged
        lblAvgMnths.Visible = False
        txtAvgMnths.Visible = False
        lblMonths.Visible = False
        pnlAvragePeriod.Visible = True
        lblInfo.Visible = True
        upnlAvrgperiod.Update()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            cmbAssembly.SelectedIndex = 0
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('False')", True)
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            ''MachineName = cmbAircraft.SelectedValue.ToString
            ''mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtFromDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            'cmbAssembly.DataSource = mAssemblyStatusList
            'Session("mAssemblyStatusList") = mAssemblyStatusList
            'cmbAssembly.DataBind()

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.Trim.ToString, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()

            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('True')", True)
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        DataFieldBind()
        ControlVisibility()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub txtFromDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text.Trim
        If AOnDate = AOdate Then
        Else
            Dim tmpdate As Date
            If Date.TryParse(txtFromDate.Text.Trim, tmpdate) Then
                SetComboOfMachine(AOdate)
                lblAssembly.Enabled = False
                cmbAssembly.Enabled = False
                mAssemblyList = Nothing
                Session("mAssemblyList") = mAssemblyList
                cmbAssembly.ClearSelection()
                cmbAssembly.DataSource = mAssemblyList
                cmbAssembly.Controls.Clear()
                cmbAssembly.DataBind()
                DataFieldBind()
                ControlVisibility()
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('False')", True)
            End If
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
    Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("Due-Periodwise With Up-Coming").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("Due-Periodwise With Up-Coming").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnByExcel_Click(sender As Object, e As System.EventArgs) Handles btnByExcel.Click
        If IsValid = True Then
            mIsExcel = True
            SetReport(, mIsExcel)
        End If
    End Sub

    Private Sub ListRegNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListRegNo.SelectedIndexChanged

        ChkRegNos = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
                   Where c.Selected = True
                   Select (c.Value)).ToArray

        If ChkRegNos.Length = 1 Then
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, ChkRegNos(0).ToString, txtFromDate.Text.Trim.ToString, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
            upnlAssembly.Update()
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('True')", True)
        Else
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            cmbAssembly.SelectedIndex = 0
            upnlAssembly.Update()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('False')", True)
        End If
    End Sub
#End Region

   
End Class