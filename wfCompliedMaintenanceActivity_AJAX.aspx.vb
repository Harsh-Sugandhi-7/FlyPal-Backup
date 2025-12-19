
'CREATED By : Saylee
'Dated      : 28-Jan-2014

Imports System.Linq
Imports System.Collections.Generic
Imports System.Text


Public Class wfCompliedMaintenanceActivity_AJAX
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Dim mCompliedDirectiveStatusActivityList As CompliedDirectiveStatusActivityList
    Dim mCompliedServiceStatusActivityList As CompliedServiceStatusActivityList
    Dim mCompliedInspStatusActivityList As CompliedInspStatusActivityList
    Dim mRectifiedMELSnagCorrectiveActionList As RectifiedMELSnagCorrectiveActionList

    Dim FromDate As String
    Dim ToDate As String
    Dim Directive As String
    Dim Aircraft As String
    Dim AircraftIndex As Integer
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim AssemblyType As String
    Dim ModelName As String
    Dim ModTypeName As String
    Dim MachineName As String
    Dim ModelID As String

    Private mMachineList As MachineList
    Dim mMachineNameValueList As MachineNameValueList
    Public mModificationTypeList As ModelMonitorModTypeList
    Public mInspectionTypesList As ModelMonitorInspTypeList
    Public mServiceTypeList As PartMonitorServiceTypeList

    Private mModTypeList As ModTypeList
    Private mAssemblyList As AssemblyList

    Dim IsSerSelect As Boolean = False
    Dim IsModSelect As Boolean = False
    Dim IsInsSelect As Boolean = False

    Dim ServiceTypeID(50) As Integer
    Dim InspectionTypeID(50) As Integer
    Dim ModificationTypeID As String

    Dim ArrCnt As Integer = 0

    Public EventLogID As Guid
    Public EventLogDetail As String = ""

    Dim ServiceTypeName(50) As String
    Dim InspectionTypeName(50) As String
    Dim ModificationTypeName(50) As String
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypesList = CType(Session("mInspectionTypesList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mAssemblyList") = mAssemblyList
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypesList") = mInspectionTypesList
        Session("mModificationTypeList") = mModificationTypeList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfCompliedMaintenanceActivity_AJAX.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblyList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType, "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblAssembly1.Visible = True
    End Sub
    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            If cmbAssembly.SelectedItem.Text = "<SELECT>" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : All"
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1
            End If
        End If

        If Not IsDate(txtFromDate.Text) Then
            FromDate = ""
        Else
            FromDate = txtFromDate.Text.ToString
        End If
        If Not IsDate(txtToDate.Text) Then
            ToDate = ""
        Else
            ToDate = txtToDate.Text.ToString
        End If

        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        lblDateRangeFrom.Text = "From Date : " & IIf(FromDate <> "", New SmartDate(FromDate).FormattedText, "") & "   To Date : " & IIf(ToDate <> "", New SmartDate(ToDate).FormattedText, "")


        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")

        Dim Type As String = ""

        'Set Service/Inspection/Directive checkbox list values
        'Service
        If chkService.Checked Then
            IsSerSelect = True
            Type = "Services: "
            ServiceTypeID = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
                         Where c.Selected = True
                         Select CInt(c.Value)).ToArray
            ServiceTypeName = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
                         Where c.Selected = True
                         Select (c.Text)).ToArray

            For i As Integer = 0 To ServiceTypeName.Length - 1
                If i = InspectionTypeName.Length - 1 Then
                    Type = Type + ServiceTypeName(i)
                Else
                    Type = Type + ServiceTypeName(i) + " , "
                End If
            Next
        End If
        'Inspection
        If chkInspection.Checked Then
            IsInsSelect = True
            Type = "Inspections: "
            InspectionTypeID = (From c As System.Web.UI.WebControls.ListItem In ListInspectionType.Items
                         Where c.Selected = True
                         Select CInt(c.Value)).ToArray

            InspectionTypeName = (From c As System.Web.UI.WebControls.ListItem In ListInspectionType.Items
                         Where c.Selected = True
                         Select (c.Text)).ToArray

            For i As Integer = 0 To InspectionTypeName.Length - 1
                If i = InspectionTypeName.Length - 1 Then
                    Type = Type + InspectionTypeName(i)
                Else
                    Type = Type + InspectionTypeName(i) + " , "
                End If
            Next
        End If
        'Directive
        If chkDirective.Checked Then
            Dim tmpModificationTypeID As New StringBuilder
            IsModSelect = True
            Type = "Directives: "

            For i As Integer = 0 To ListDirectiveType.Items.Count - 1
                Dim appval As String = ""
                If i = ListDirectiveType.Items.Count - 1 Then
                    appval = ""
                Else
                    appval = ","
                End If

                If ListDirectiveType.Items(i).Selected = True Then
                    tmpModificationTypeID = tmpModificationTypeID.Append(ListDirectiveType.Items(i).Value + appval)
                End If

            Next

            'tmpModificationTypeID = tmpModificationTypeID.Append((From c As System.Web.UI.WebControls.ListItem In chkListDirectiveType.Items
            '             Where c.Selected = True
            '            Select CStr(c.Value) + ",").ToList)

            ModificationTypeName = (From c As System.Web.UI.WebControls.ListItem In ListDirectiveType.Items
                       Where c.Selected = True
                       Select (c.Text)).ToArray

            If tmpModificationTypeID.Length > 0 Then
                '' ModificationTypeID = IIf(tmpModificationTypeID.Length > 0, tmpModificationTypeID.ToString.Substring(0, tmpModificationTypeID.Length - 1), "")
                ModificationTypeID = IIf(tmpModificationTypeID.Length > 0, tmpModificationTypeID.ToString.Substring(0, tmpModificationTypeID.Length), "")
            Else
                ModificationTypeID = ""
            End If

            For i As Integer = 0 To ModificationTypeName.Length - 1
                If i = ModificationTypeName.Length - 1 Then
                    Type = Type + ModificationTypeName(i)
                Else
                    Type = Type + ModificationTypeName(i) + " , "
                End If
            Next


        End If
        EventLogDetail = lblDateRangeFrom.Text + " , " + lblAircraft1.Text + " , " + lblAssembly1.Text + Type
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        IsSerSelect = False
        IsInsSelect = False
        IsModSelect = False
        ServiceTypeID(0) = 0
        InspectionTypeID(0) = 0
        ModificationTypeID = ""
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Private Sub SetReport(Optional ByVal ByExcel As Boolean = False)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompliedServiceStatusActivityList As CompliedServiceStatusActivityList
        Dim mCompliedInspStatusActivityList As CompliedInspStatusActivityList
        Dim mCompanyDetail As CompanyDetail
        Dim ds As New dsCompliedActivityList
        Dim ReportName As String
        Dim Operatorname As String = ""
        SetValues()


        Dim AirframeHRS As String = ""
        Dim LastARCDate As String = ""

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then Operatorname = mMachineOperatorName.OperatorName
        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "SUH")) Then
            Dim mAircrafyCurrValue As AircraftCurrentStatusList = AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(, cmbAircraft.SelectedItem.Text.ToString, , , , txtToDate.Text.ToString)
            AirframeHRS = mAircrafyCurrValue(0).ShowPeriods
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        If chkService.Checked Then
            If (cmbAssembly.SelectedItem.Text = "(All)") Or (cmbAssembly.SelectedItem.Text = "<SELECT>") Then
                mCompliedServiceStatusActivityList = CompliedServiceStatusActivityList.GetCompliedServiceActivityList(FromDate, ToDate, New Guid(MachineName), mAssemblyList, ServiceTypeID, Val(txtPercentage.Text), chkApplicable.Checked)
            Else
                mCompliedServiceStatusActivityList = CompliedServiceStatusActivityList.GetCompliedServiceActivityList(FromDate, ToDate, New Guid(MachineName), New Guid(AssemblyName), ServiceTypeID, Val(txtPercentage.Text), chkApplicable.Checked)
            End If


            If chkPercentLife.Checked Then
                ReportName = "Life Percentage of Complied Service Activities"
                myReport = New crptCompliedServiceStatusPercentLifeReport
            Else
                ReportName = "Complied Service Status Activity Report"
                myReport = New crptCompliedServiceStatusActivityList
            End If


            If mCompliedServiceStatusActivityList.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf mCompliedServiceStatusActivityList.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1271)
            End If

        End If
        If chkInspection.Checked Then
            If (cmbAssembly.SelectedItem.Text = "(All)") Or (cmbAssembly.SelectedItem.Text = "<SELECT>") Then
                mCompliedInspStatusActivityList = CompliedInspStatusActivityList.GetCompliedInspActivityList(FromDate, ToDate, New Guid(MachineName), mAssemblyList, InspectionTypeID, Val(txtPercentage.Text), chkApplicable.Checked)
            Else
                mCompliedInspStatusActivityList = CompliedInspStatusActivityList.GetCompliedInspActivityList(FromDate, ToDate, New Guid(MachineName), New Guid(AssemblyName), InspectionTypeID, Val(txtPercentage.Text), chkApplicable.Checked)
            End If

            If chkPercentLife.Checked Then
                ReportName = "Percentage Left Over of Complied Inspection Activities"
                myReport = New crptCompliedInspStatusPercentLifeReport
            Else
                ReportName = "Complied Inspection Status Activity Report"
                myReport = New crptCompliedInspStatusActivityList
            End If

            If mCompliedInspStatusActivityList.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf mCompliedInspStatusActivityList.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1271)
            End If

        End If
        If chkDirective.Checked Then
            mCompliedDirectiveStatusActivityList = CompliedDirectiveStatusActivityList.GetCompliedModActivityList(FromDate, ToDate, New Guid(MachineName), ModificationTypeID, cmbAssembly.SelectedValue, Val(txtPercentage.Text), chkApplicable.Checked)



            If chkPercentLife.Checked Then
                ReportName = "Life Percentage of Complied Directive Activities"
                myReport = New crptCompliedDirectiveStatusPercentLifeReport
            Else
                ReportName = "Complied Directive Status Activity Report"
                myReport = New crptCompliedDirectiveStatusActivityList
            End If


            If mCompliedDirectiveStatusActivityList.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf mCompliedDirectiveStatusActivityList.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1271)
            End If

        End If


        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "SUH")) And chkPercentLife.Checked = False And cmbFormat.SelectedIndex = 1 Then
            myReport = New crptCompliedStatusActivityListSUHAN
        End If

        If chkSnag.Checked Then
            mRectifiedMELSnagCorrectiveActionList = RectifiedMELSnagCorrectiveActionList.GetRectifiedMELSnagCorrectiveActionList(FromDate, ToDate, New Guid(cmbAircraft.SelectedValue))
            ReportName = IIf(AppSettings("MELSnagNomenclature") = "True", "Rectified ADD/Defect Activity Report", "Rectified MEL/Snag Activity Report") 'Appsettings Added By Vikrant On 07-Sep-2020 For ALL07092020
            myReport = New crptRectifiedMELSnagCorrectiveActionReport

            If mRectifiedMELSnagCorrectiveActionList.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf mRectifiedMELSnagCorrectiveActionList.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1271)
            End If

        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                 mCompanyDetail.WebSite, ReportName, New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, Aircraft, Assembly1, Operatorname, mModuleList.Item("CompliedMaintenanceActivities").FormRevisionNo, AppSettings("SINote"), txtPercentage.Text.Trim, AirframeHRS, txtARCDate.Text.ToString, AppSettings("MELSnagNomenclature").ToString, AppSettings("Logo"))
        'Replace AppSettings("Product Version") with mModuleList.Item("CompliedMaintenanceActivities").FormRevisionNo for  SUHAN

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        If chkService.Checked = True Then 'Service
            da.Fill(ds, mCompliedServiceStatusActivityList)
            da.Fill(ds, "CompliedStatusActivityList", mCompliedServiceStatusActivityList)
            If ByExcel = True Then SetServiceExcel(mCompliedServiceStatusActivityList, Report, ReportName) : Exit Sub 'Added by Saylee on 9-Oct-2019, ALL09102019-1

        ElseIf chkInspection.Checked = True Then 'Inspection
            da.Fill(ds, mCompliedInspStatusActivityList)
            da.Fill(ds, "CompliedStatusActivityList", mCompliedInspStatusActivityList)
            If ByExcel = True Then SetInspExcel(mCompliedInspStatusActivityList, Report, ReportName) : Exit Sub 'Added by Saylee on 9-Oct-2019, ALL09102019-1
        ElseIf chkDirective.Checked = True Then 'Inspection
            da.Fill(ds, mCompliedDirectiveStatusActivityList)
            da.Fill(ds, "CompliedStatusActivityList", mCompliedDirectiveStatusActivityList)
            If ByExcel = True Then SetDirectiveExcel(mCompliedDirectiveStatusActivityList, Report, ReportName) : Exit Sub 'Added by Saylee on 9-Oct-2019, ALL09102019-1
        ElseIf chkSnag.Checked = True Then
            da.Fill(ds, mRectifiedMELSnagCorrectiveActionList)
            If ByExcel = True Then SetSnagExcel(mRectifiedMELSnagCorrectiveActionList, Report, ReportName) : Exit Sub 'Added by Saylee on 9-Oct-2019, ALL09102019-1
        End If

        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        ' MarkLog(Util.Action.Print, "CompliedMaintenanceActivities", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        MarkLog(Util.Action.Print, "CompliedMaintenanceActivities", IIf(ByExcel = True, "Export To excel ", "") + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)  'Added by Shital on 18-Jan-2021 Export To excel
        ResetValues()
    End Sub
    '***************************************************************************************
    'Added by Saylee on 9-Oct-2019, ALL09102019-1
    Private Sub SetSnagExcel(ExRectifiedMELSnagCorrectiveActionList As RectifiedMELSnagCorrectiveActionList, ExReport As ReportData, ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsExcel As New dsCompliedActivityList
        da.Fill(dsExcel, ExRectifiedMELSnagCorrectiveActionList)
        da.Fill(dsExcel, ExReport)

        Dim columnToRemove As String() = { _
                                                "ActionAgainstStaff", _
                                                "AircraftType", _
                                                "ATAChapterID", _
                                                "ATACode", _
                                                "ATANomenclature", _
                                                "CauseOfDefect", _
                                                "ComponentHour", _
                                                "DateOfOccurence", _
                                                "DefectReportNo", _
                                                "Description", _
                                                "DispATACode", _
                                                "DispSubATACode", _
                                                "DueDate", _
                                                "DueDateFormatted", _
                                                "FrequencyInDays", _
                                                "FrequencyInHours", _
                                                "ID", _
                                                "InvestigationStatus", _
                                                "InvestigationStatusText", _
                                                "IsHours", _
                                                "IsMajor", _
                                                "IsRepetitive", _
                                                "LastMajorCheckHour", _
                                                "LogDate", _
                                                "LogID", _
                                                "LogNo", _
                                                "LogNoPageNo", _
                                                "LogPageNo", _
                                                "MajorMinorTag", _
                                                "MELCategoryID", _
                                                "MELCategoryName", _
                                                "No", _
                                                "PartID", _
                                                "PartName", _
                                                "PartNo", _
                                                "PartSerialNo", _
                                                "PreventionTaken", _
                                                "RectifiedDate", _
                                                "RectifiedLogID", _
                                                "RectifiedLogNo", _
                                                "RectifiedLogPageNo", _
                                                "RectifiedLogText", _
                                                "RectifiedStation", _
                                                "RegNo", _
                                                "Remark", _
                                                "Sector", _
                                                "SerialNo", _
                                                "SnagReportedBy", _
                                                "SubATACode", _
                                                "SubATANomenclature", _
                                                "EngineType" _
                                        }

        For i As Integer = 0 To columnToRemove.Length - 1
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns.Contains(columnToRemove(i)) Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns.Remove(columnToRemove(i))
            End If
        Next

        'set Column Sequence
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("DateOfOccurenceFormatted").SetOrdinal(0)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("Defect").SetOrdinal(1)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("ReportedBy").SetOrdinal(2)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("Action").SetOrdinal(3)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("ATAChapter").SetOrdinal(4)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("SubATACodeDisplay").SetOrdinal(5)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("IsMEL").SetOrdinal(6)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("RectifiedDateFormatted").SetOrdinal(7)
        dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns("RectifiedMechanic").SetOrdinal(8)

        Dim ColumnName As String = String.Empty

        For i As Integer = 0 To dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns.Count - 1
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "DateOfOccurenceFormatted" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Date Of Occurence"
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "ReportedBy" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Observed By"
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Defect" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Defect Observed"
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Action" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Rectification Action"
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "ATAChapter" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "ATA Chapter"
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "SubATACodeDisplay" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Sub-ATA Chapter"
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "IsMEL" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL") 'Appsettings Added By Vikrant On 07-Sep-2020 For ALL07092020
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "RectifiedDateFormatted" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Rectified Date"
            End If
            If dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "RectifiedMechanic" Then
                dsExcel.Tables("RectifiedMELSnagCorrectiveActionList").Columns(i).ColumnName = "Rectification By"
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
                                               "SearchStr7", _
                                               "SearchStr8", _
                                               "SearchStr9", _
                                               "ProductVersion", _
                                               "SINote", _
                                               "CurrencyName", _
                                               "CurrencySymbol", _
                                               "SearchStr10", _
                                               "SearchStr11", _
                                               "SearchStr12", _
                                               "SearchStr13", _
                                               "SearchStr14", _
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
                                               "SearchStr25", _
                                               "SearchStr6", _
                                               "SearchStr5" _
                                }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If dsExcel.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                dsExcel.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next
        dsExcel.Tables("ReportData").Columns("SearchStr1").SetOrdinal(0)
        dsExcel.Tables("ReportData").Columns("SearchStr2").SetOrdinal(1)
        dsExcel.Tables("ReportData").Columns("SearchStr3").SetOrdinal(2)
        dsExcel.Tables("ReportData").Columns("SearchStr4").SetOrdinal(3)
      

        For i As Integer = 0 To dsExcel.Tables("ReportData").Columns.Count - 1
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "From Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "To Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Assembly"
            End If
            'If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
            '    dsExcel.Tables("ReportData").Columns(i).ColumnName = "Operator"
            'End If
           
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsExcel.Tables("ReportData"))
        dsNew.Merge(dsExcel.Tables("RectifiedMELSnagCorrectiveActionList"))

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("RectifiedMELSnagCorrectiveActionList").TableName = ReportName
        Session("DataTableToBeFormattedForExportToExcel") = ReportName
		Session("ExcelFileName") = ReportName
		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
    Private Sub SetServiceExcel(ExCompliedServiceStatusActivityList As CompliedServiceStatusActivityList, ExReport As ReportData, ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsExcel As New dsCompliedActivityList
        da.Fill(dsExcel, ExCompliedServiceStatusActivityList)
        da.Fill(dsExcel, ExReport)

        Dim columnToRemove As String() = { _
                                                "AssemblyCompID", _
                                                "AssemblyID", _
                                                "AsOnDate", _
                                                "AsOnDateFormatted", _
                                                "ApprovalRemark", _
                                                "AssemblyCompSerialNo", _
                                                "AssemblyName", _
                                                "AssemblyPosition", _
                                                "AssemblySerialNo", _
                                                "AssemblyStatusID", _
                                                "AssemblyType", _
                                                "AssemblyTypeID", _
                                                "CompName", _
                                                "CompPosition", _
                                                "CompSerialNo", _
                                                "CompStatusID", _
                                                "DoneOn", _
                                                "DueOnValueFormattedForGrid", _
                                                "DueStatus", _
                                                "ExtensionDate", _
                                                "ExtensionDateFormatted", _
                                                "HourType", _
                                                "IsApplicable", _
                                                "IsCompleted", _
                                                "IsMaster", _
                                                "MachineID", _
                                                "MachineInfo", _
                                                "MinimumPercentLife", _
                                                "ModelPartMonitorServiceCode", _
                                                "ModelPartMonitorServiceID", _
                                                "ModelPartMonitorServiceTypeCode", _
                                                "MonitorServiceStatusID", _
                                                "MonitorServiceStatusPeriodID", _
                                                "MonitorTypeID", _
                                                "Note", _
                                                "PeriodID", _
                                                "PeriodUnitID", _
                                                "PeriodUnitName", _
                                                "PeriodUnitNameForWeb", _
                                                "PreviousDueOnValue", _
                                                "RequiredManHours", _
                                                "ServiceType", _
                                                "WasteLife", _
                                                "FrequencyValue", _
                                                "DoneOnValue", _
                                                "DueOnValue", _
                                                "AssemblyInfo", _
                                                "CompInfo", _
                                                "AssemblyCompName", _
                                                "MonitorStatusID", _
                                                "MonitorStatusPeriodID", _
                                                "ModelPartMonitorID" _
                                             }



        For i As Integer = 0 To columnToRemove.Length - 1
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns.Contains(columnToRemove(i)) Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns.Remove(columnToRemove(i))
            End If
        Next


        'set Column Sequence
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("ATA").SetOrdinal(0)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("Description").SetOrdinal(1)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("Reference").SetOrdinal(2)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("AssemblyInfoExcel").SetOrdinal(3)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("CompInfoExcel").SetOrdinal(4)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("ModelPartMonitorServiceTypeName").SetOrdinal(5)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("MonitorType").SetOrdinal(6)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("DoneOnWONo").SetOrdinal(7)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("EmpName").SetOrdinal(8)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("Place").SetOrdinal(9)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("DoneOnFormatted").SetOrdinal(10)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("FrequencyValueExcel").SetOrdinal(11)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("DoneOnValueExcel").SetOrdinal(12)
        dsExcel.Tables("CompliedServiceStatusActivityList").Columns("DueOnValueExcel").SetOrdinal(13)


        If Not chkPercentLife.Checked Then
            dsExcel.Tables("CompliedServiceStatusActivityList").Columns.Remove("WasteLifeExcel")
            dsExcel.Tables("CompliedServiceStatusActivityList").Columns.Remove("WastePercentLife")
            dsExcel.Tables("CompliedServiceStatusActivityList").Columns("DoneRemark").SetOrdinal(14)
        Else
            dsExcel.Tables("CompliedServiceStatusActivityList").Columns("WasteLifeExcel").SetOrdinal(14)
            dsExcel.Tables("CompliedServiceStatusActivityList").Columns("WastePercentLife").SetOrdinal(15)
            dsExcel.Tables("CompliedServiceStatusActivityList").Columns("DoneRemark").SetOrdinal(16)
        End If




        Dim ColumnName As String = String.Empty

        For i As Integer = 0 To dsExcel.Tables("CompliedServiceStatusActivityList").Columns.Count - 1
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "AssemblyInfoExcel" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Assembly Info"
            End If

            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "CompInfoExcel" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Comp Info"
            End If

            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "ModelPartMonitorServiceTypeName" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Service Type"
            End If
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "MonitorType" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Monitor Type"
            End If
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "DoneOnWONo" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Work Done On"
            End If
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "EmpName" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Work Done By"
            End If
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "DoneOnFormatted" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Done On"
            End If
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "FrequencyValueExcel" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Frequency"
            End If
            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "DoneOnValueExcel" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "DoneOn Value"
            End If

            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "DueOnValueExcel" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Due On"
            End If

            If chkPercentLife.Checked Then
                If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "WasteLifeExcel" Then
                    dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Left Over Life"
                End If
                If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "WastePercentLife" Then
                    dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "% Left Over"
                End If
            End If

            If dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "DoneRemark" Then
                dsExcel.Tables("CompliedServiceStatusActivityList").Columns(i).ColumnName = "Remark"
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
                                                "SearchStr7", _
                                                "SearchStr8", _
                                                "SearchStr9", _
                                                "ProductVersion", _
                                                "SINote", _
                                                "CurrencyName", _
                                                "CurrencySymbol", _
                                                "SearchStr10", _
                                                "SearchStr11", _
                                                 "SearchStr12", _
                                                 "SearchStr13", _
                                                "SearchStr14", _
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
                                               "SearchStr25", _
                                               "SearchStr5" _
                                                       }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If dsExcel.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                dsExcel.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next


      
        dsExcel.Tables("ReportData").Columns("SearchStr1").SetOrdinal(0)
        dsExcel.Tables("ReportData").Columns("SearchStr2").SetOrdinal(1)
        dsExcel.Tables("ReportData").Columns("SearchStr3").SetOrdinal(2)
        dsExcel.Tables("ReportData").Columns("SearchStr4").SetOrdinal(3)
        dsExcel.Tables("ReportData").Columns("SearchStr6").SetOrdinal(4)

        For i As Integer = 0 To dsExcel.Tables("ReportData").Columns.Count - 1
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "From Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "To Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Assembly"
            End If
            'If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
            '    dsExcel.Tables("ReportData").Columns(i).ColumnName = "Operator"
            'End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr6" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "% LeftOver"
            End If
        Next

        If Not chkPercentLife.Checked Then
            dsExcel.Tables("ReportData").Columns.Remove("% LeftOver")
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsExcel.Tables("ReportData"))
        dsNew.Merge(dsExcel.Tables("CompliedServiceStatusActivityList"))

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("CompliedServiceStatusActivityList").TableName = ReportName
		Session("DataTableToBeFormattedForExportToExcel") = ReportName
		Session("ExcelFileName") = ReportName
		PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "DoneOn Value", "Due On", "Left Over Life"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub

    Private Sub SetInspExcel(ByVal ExCompliedInspStatusActivityList As CompliedInspStatusActivityList, ByVal ExReport As ReportData, ByVal ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsExcel As New dsCompliedActivityList
        da.Fill(dsExcel, ExCompliedInspStatusActivityList)
        da.Fill(dsExcel, ExReport)

        Dim columnToRemove As String() = { _
                                                "AssemblyCompID", _
                                                "AssemblyID", _
                                                "AsOnDate", _
                                                "AsOnDateFormatted", _
                                                "ApprovalRemark", _
                                                "AssemblyCompSerialNo", _
                                                "AssemblyName", _
                                                "AssemblyPosition", _
                                                "AssemblySerialNo", _
                                                "AssemblyStatusID", _
                                                "AssemblyType", _
                                                "AssemblyTypeID", _
                                                "CompName", _
                                                "CompPosition", _
                                                "CompSerialNo", _
                                                "CompStatusID", _
                                                "DoneOn", _
                                                "DueOnValueFormattedForGrid", _
                                                "DueStatus", _
                                                "ExtensionDate", _
                                                "ExtensionDateFormatted", _
                                                "HourType", _
                                                "IsApplicable", _
                                                "IsCompleted", _
                                                "IsMaster", _
                                                "MachineID", _
                                                "MachineInfo", _
                                                "MinimumPercentLife", _
                                                "ModelPartMonitorInspCode", _
                                                "ModelPartMonitorInspID", _
                                                "ModelPartMonitorInspTypeCode", _
                                                "MonitorInspStatusID", _
                                                "MonitorInspStatusPeriodID", _
                                                "MonitorTypeID", _
                                                "Note", _
                                                "PeriodID", _
                                                "PeriodUnitID", _
                                                "PeriodUnitName", _
                                                "PeriodUnitNameForWeb", _
                                                "PreviousDueOnValue", _
                                                "RequiredManHours", _
                                                "InspType", _
                                                "WasteLife", _
                                                "FrequencyValue", _
                                                "DoneOnValue", _
                                                "DueOnValue", _
                                                "AssemblyInfo", _
                                                "CompInfo", _
                                                "MonitorStatusID", _
                                                "MonitorStatusPeriodID", _
                                                "ModelPartMonitorID" _
                                            }

        For i As Integer = 0 To columnToRemove.Length - 1
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns.Contains(columnToRemove(i)) Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns.Remove(columnToRemove(i))
            End If
        Next

        'set Column Sequence
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("ATA").SetOrdinal(0)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("Description").SetOrdinal(1)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("Reference").SetOrdinal(2)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("AssemblyInfoExcel").SetOrdinal(3)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("CompInfoExcel").SetOrdinal(4)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("ModelPartMonitorInspTypeName").SetOrdinal(5)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("MonitorType").SetOrdinal(6)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("DoneOnWONo").SetOrdinal(7)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("EmpName").SetOrdinal(8)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("Place").SetOrdinal(9)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("DoneOnFormatted").SetOrdinal(10)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("FrequencyValueExcel").SetOrdinal(11)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("DoneOnValueExcel").SetOrdinal(12)
        dsExcel.Tables("CompliedInspStatusActivityList").Columns("DueOnValueExcel").SetOrdinal(13)



        If Not chkPercentLife.Checked Then
            dsExcel.Tables("CompliedInspStatusActivityList").Columns.Remove("WasteLifeExcel")
            dsExcel.Tables("CompliedInspStatusActivityList").Columns.Remove("WastePercentLife")
            dsExcel.Tables("CompliedInspStatusActivityList").Columns("DoneRemark").SetOrdinal(14)
        Else
            dsExcel.Tables("CompliedInspStatusActivityList").Columns("WasteLifeExcel").SetOrdinal(14)
            dsExcel.Tables("CompliedInspStatusActivityList").Columns("WastePercentLife").SetOrdinal(15)
            dsExcel.Tables("CompliedInspStatusActivityList").Columns("DoneRemark").SetOrdinal(16)
        End If


        Dim ColumnName As String = String.Empty

        For i As Integer = 0 To dsExcel.Tables("CompliedInspStatusActivityList").Columns.Count - 1
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "AssemblyInfoExcel" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Assembly Info"
            End If

            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "CompInfoExcel" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Comp Info"
            End If

            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "ModelPartMonitorInspTypeName" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Insp Type"
            End If
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "MonitorType" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Monitor Type"
            End If
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "DoneOnWONo" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Work Done On"
            End If
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "EmpName" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Work Done By"
            End If
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "DoneOnFormatted" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Done On"
            End If
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "FrequencyValueExcel" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Frequency"
            End If
            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "DoneOnValueExcel" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "DoneOn Value"
            End If

            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "DueOnValueExcel" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Due On"
            End If

            If chkPercentLife.Checked Then
                If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "WasteLifeExcel" Then
                    dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Left Over Life"
                End If
                If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "WastePercentLife" Then
                    dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "% Left Over"
                End If
            End If



            If dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "DoneRemark" Then
                dsExcel.Tables("CompliedInspStatusActivityList").Columns(i).ColumnName = "Remark"
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
                                                "SearchStr7", _
                                                "SearchStr8", _
                                                "SearchStr9", _
                                                "ProductVersion", _
                                                "SINote", _
                                                "CurrencyName", _
                                                "CurrencySymbol", _
                                                "SearchStr10", _
                                                "SearchStr11", _
                                                 "SearchStr12", _
                                                 "SearchStr13", _
                                                "SearchStr14", _
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
                                               "SearchStr25", _
                                               "SearchStr5" _
                                                       }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If dsExcel.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                dsExcel.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next
        dsExcel.Tables("ReportData").Columns("SearchStr1").SetOrdinal(0)
        dsExcel.Tables("ReportData").Columns("SearchStr2").SetOrdinal(1)
        dsExcel.Tables("ReportData").Columns("SearchStr3").SetOrdinal(2)
        dsExcel.Tables("ReportData").Columns("SearchStr4").SetOrdinal(3)

        dsExcel.Tables("ReportData").Columns("SearchStr6").SetOrdinal(4)

        For i As Integer = 0 To dsExcel.Tables("ReportData").Columns.Count - 1
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "From Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "To Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Assembly"
            End If
            'If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
            '    dsExcel.Tables("ReportData").Columns(i).ColumnName = "Operator"
            'End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr6" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "% LeftOver"
            End If
        Next

        If Not chkPercentLife.Checked Then
            dsExcel.Tables("ReportData").Columns.Remove("% LeftOver")
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsExcel.Tables("ReportData"))
        dsNew.Merge(dsExcel.Tables("CompliedInspStatusActivityList"))

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("CompliedInspStatusActivityList").TableName = ReportName
        Session("DataTableToBeFormattedForExportToExcel") = ReportName
        PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "DoneOn Value", "Due On", "Left Over Life"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub

    Private Sub SetDirectiveExcel(ByVal ExCompliedDirectiveStatusActivityList As CompliedDirectiveStatusActivityList, ByVal ExReport As ReportData, ByVal ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsExcel As New dsCompliedActivityList
        da.Fill(dsExcel, ExCompliedDirectiveStatusActivityList)
        da.Fill(dsExcel, ExReport)

        Dim columnToRemove As String() = { _
                                                "AssemblyCompID", _
                                                "AssemblyID", _
                                                "AsOnDate", _
                                                "AsOnDateFormatted", _
                                                "ApprovalRemark", _
                                                "AssemblyCompSerialNo", _
                                                "AssemblyName", _
                                                "AssemblyPosition", _
                                                "AssemblySerialNo", _
                                                "AssemblyStatusID", _
                                                "AssemblyType", _
                                                "AssemblyTypeID", _
                                                "CompName", _
                                                "CompPosition", _
                                                "CompSerialNo", _
                                                "CompStatusID", _
                                                "DoneOn", _
                                                "DueOnValueFormattedForGrid", _
                                                "DueStatus", _
                                                "ExtensionDate", _
                                                "ExtensionDateFormatted", _
                                                "HourType", _
                                                "IsApplicable", _
                                                "IsCompleted", _
                                                "IsMaster", _
                                                "MachineID", _
                                                "MachineInfo", _
                                                "MinimumPercentLife", _
                                                "ModelPartMonitorModCode", _
                                                "ModelPartMonitorModID", _
                                                "ModelPartMonitorModTypeCode", _
                                                "MonitorModStatusID", _
                                                "MonitorModStatusPeriodID", _
                                                "MonitorTypeID", _
                                                "Note", _
                                                "PeriodID", _
                                                "PeriodUnitID", _
                                                "PeriodUnitName", _
                                                "PeriodUnitNameForWeb", _
                                                "PreviousDueOnValue", _
                                                "RequiredManHours", _
                                                "DirectiveType", _
                                                "WasteLife", _
                                                "FrequencyValue", _
                                                "DoneOnValue", _
                                                "DueOnValue", _
                                                "AssemblyInfo", _
                                                "CompInfo", _
                                                "DoneOnWONO", _
                                                "DoneBy", _
                                                "Place", _
                                                "Description", _
                                                "CurrentValue", _
                                                "MonitorStatusID", _
                                                "MonitorStatusPeriodID", _
                                                "ModelPartMonitorID" _
                                            }

        For i As Integer = 0 To columnToRemove.Length - 1
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Contains(columnToRemove(i)) Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Remove(columnToRemove(i))
            End If
        Next

        'set Column Sequence
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("ATA").SetOrdinal(0)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DirectiveNo").SetOrdinal(1)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("ModDescription").SetOrdinal(2)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("Reference").SetOrdinal(3)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("AssemblyInfoExcel").SetOrdinal(4)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("CompInfoExcel").SetOrdinal(5)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("ModelPartMonitorModTypeName").SetOrdinal(6)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("MonitorType").SetOrdinal(7)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("WorkDoneInfo").SetOrdinal(8)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DoneOnFormatted").SetOrdinal(9)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("FrequencyValueExcel").SetOrdinal(10)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DoneOnValueExcel").SetOrdinal(11)
        dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DueOnValueExcel").SetOrdinal(12)



        If Not chkPercentLife.Checked Then
            dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Remove("WasteLifeExcel")
            dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Remove("WastePercentLife")
            dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DoneRemark").SetOrdinal(13)
        Else
            dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("WasteLifeExcel").SetOrdinal(13)
            dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("WastePercentLife").SetOrdinal(14)
            dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns("DoneRemark").SetOrdinal(15)
        End If


        Dim ColumnName As String = String.Empty

        For i As Integer = 0 To dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns.Count - 1

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DirectiveNo" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Mod No."
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "ModDescription" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Description"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "AssemblyInfoExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Assembly Info"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "CompInfoExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Comp Info"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "ModelPartMonitorDirectiveTypeName" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Directive Type"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "MonitorType" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Monitor Type"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DoneOnWONo" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Work Done On"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "EmpName" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Work Done By"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DoneOnFormatted" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Done On"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "FrequencyValueExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Frequency"
            End If
            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DoneOnValueExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DoneOn Value"
            End If

            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DueOnValueExcel" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Due On"
            End If

            If chkPercentLife.Checked Then
                If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "WasteLifeExcel" Then
                    dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Left Over Life"
                End If
                If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "WastePercentLife" Then
                    dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "% Left Over"
                End If
            End If




            If dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "DoneRemark" Then
                dsExcel.Tables("CompliedDirectiveStatusActivityList").Columns(i).ColumnName = "Remark"
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
                                                "SearchStr7", _
                                                "SearchStr8", _
                                                "SearchStr9", _
                                                "ProductVersion", _
                                                "SINote", _
                                                "CurrencyName", _
                                                "CurrencySymbol", _
                                                "SearchStr10", _
                                                "SearchStr11", _
                                                 "SearchStr12", _
                                                 "SearchStr13", _
                                                "SearchStr14", _
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
                                               "SearchStr25", _
                                               "SearchStr5" _
                                                       }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If dsExcel.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                dsExcel.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next
        dsExcel.Tables("ReportData").Columns("SearchStr1").SetOrdinal(0)
        dsExcel.Tables("ReportData").Columns("SearchStr2").SetOrdinal(1)
        dsExcel.Tables("ReportData").Columns("SearchStr3").SetOrdinal(2)
        dsExcel.Tables("ReportData").Columns("SearchStr4").SetOrdinal(3)

        dsExcel.Tables("ReportData").Columns("SearchStr6").SetOrdinal(4)

        For i As Integer = 0 To dsExcel.Tables("ReportData").Columns.Count - 1
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "From Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "To Date"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Aircraft"
            End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "Assembly"
            End If
            'If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr5" Then
            '    dsExcel.Tables("ReportData").Columns(i).ColumnName = "Operator"
            'End If
            If dsExcel.Tables("ReportData").Columns(i).ColumnName = "SearchStr6" Then
                dsExcel.Tables("ReportData").Columns(i).ColumnName = "% LeftOver"
            End If
        Next

        If Not chkPercentLife.Checked Then
            dsExcel.Tables("ReportData").Columns.Remove("% LeftOver")
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsExcel.Tables("ReportData"))
        dsNew.Merge(dsExcel.Tables("CompliedDirectiveStatusActivityList"))

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("CompliedDirectiveStatusActivityList").TableName = ReportName
        Session("DataTableToBeFormattedForExportToExcel") = ReportName
        PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "DoneOn Value", "Due On", "Left Over Life"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
    '*******************************************************************************************
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
                    ''Response.Redirect("wfCompliedMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            ''Response.Redirect("wfCompliedMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
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
        ElseIf custValidator.ControlToValidate = "txtFromDate" Then
            If txtFromDate.Text = "" Or txtToDate.Text = "" Then
                custValidator.ErrorMessage = "Please Enter Valid Date."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtARCDate" And AppSettings("ClientCode") = "SUH" Then
            If txtARCDate.Text = "" Then
                custValidator.ErrorMessage = "Please select ARC Date."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Public Sub SetTypeCombo()
        If mServiceTypeList Is Nothing Then
            mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeListForNoFrequency(, , True)
        End If
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypesList Is Nothing Then
            mInspectionTypesList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()
        End If
        ListInspectionType.DataSource = mInspectionTypesList
        Session("mInspectionTypesList") = mInspectionTypesList

        If mModificationTypeList Is Nothing Then
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeListForNoFrequency(, , True)
        End If

        ListDirectiveType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()

        FillMonitorTypeList()

        ''''For i As Integer = 0 To chkListServiceType.Items.Count - 1
        ''''    chkListServiceType.Items(i).Selected = True
        ''''Next

        ''''For i As Integer = 0 To chkListInspectionType.Items.Count - 1
        ''''    chkListInspectionType.Items(i).Enabled = False
        ''''Next

        ''''For i As Integer = 0 To chkListDirectiveType.Items.Count - 1
        ''''    chkListDirectiveType.Items(i).Enabled = False
        ''''Next
    End Sub

    Public Sub SetCombo()
        ''GetSession()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()
        Session("mMachineNameValueList") = mMachineNameValueList

        mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeListForNoFrequency(, , True)     'ServiceType
        mInspectionTypesList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()          'Inspection Type 
        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeListForNoFrequency(, , True)     'Modification Type
    End Sub
    Private Sub FillMonitorTypeList()
        chkService.Checked = True
        For i As Integer = 0 To ListServiceType.Items.Count - 1
            ListServiceType.Items(i).Selected = True
        Next
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfCompliedMaintenanceActivity_AJAX.aspx?"
            ResetValues()
            SetCombo()
            cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            setFocus(cmbAircraft)
            SetTypeCombo()
            SetSession()

            If cmbFormat.SelectedIndex = 1 Then
                lblARCDate.Visible = True
                txtARCDate.Visible = True
            Else
                lblARCDate.Visible = False
                txtARCDate.Visible = False
            End If
            upnlARCDate.Update()
        End If

        '  MessageBoxResult()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
            upnlCriteria.Update()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid = True Then

            If chkPercentLife.Checked = True And txtPercentage.Text = "" Then
                MSGBoxCtrl.show("Alert!", "Percent Life", "Enter value for Percent Life.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If AppSettings("ClientCode") = "SUH" Then
                If txtARCDate.Text = "" And cmbFormat.SelectedIndex = 1 Then
                    MSGBoxCtrl.show("Alert!", "ARC Date", "Please select ARC Date.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If

            SetReport()
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyList = Nothing
        mServiceTypeList = Nothing
        mInspectionTypesList = Nothing
        mModificationTypeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            cmbAssembly.Enabled = False
            cmbAssembly.SelectedIndex = 0
        Else
            cmbAssembly.Enabled = True

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
        End If

        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        upnlAssembly.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub chkPercentLife_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkPercentLife.CheckedChanged
        txtPercentage.Text = ""
        If chkPercentLife.Checked Then
            txtPercentage.Enabled = True
        Else
            txtPercentage.Enabled = False
        End If
        upnlPercentLife.Update()
    End Sub
    Private Sub btnByExcel_Click(sender As Object, e As System.EventArgs) Handles btnByExcel.Click
        If IsValid = True Then
            SetReport(True)

        End If
    End Sub
    Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedIndex = 1 Then
            lblARCDate.Visible = True
            txtARCDate.Visible = True
        Else
            lblARCDate.Visible = False
            txtARCDate.Visible = False
        End If
        upnlARCDate.update()
    End Sub
#End Region

  
    'Private Sub chkService_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkService.CheckedChanged, chkInspection.CheckedChanged, chkDirective.CheckedChanged
    '    If chkService.Checked Then
    '        ListServiceType.Enabled = True
    '        ListDirectiveType.Enabled = False
    '        ListInspectionType.Enabled = False
    '        upnlDirectiveType.Update()
    '        UpnlInspectionType.Update()
    '    ElseIf chkInspection.Checked Then
    '        ListServiceType.Enabled = False
    '        ListDirectiveType.Enabled = False
    '        ListInspectionType.Enabled = True
    '        UpnlServiceType.Update()
    '        upnlDirectiveType.Update()
    '    ElseIf chkDirective.Checked Then
    '        ListServiceType.Enabled = False
    '        ListDirectiveType.Enabled = True
    '        ListInspectionType.Enabled = False
    '        UpnlServiceType.Update()
    '        upnlDirectiveType.Update()
    '    End If
    'End Sub

   
   
End Class