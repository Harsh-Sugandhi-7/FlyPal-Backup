'AJAX Conversion By Vikrant On 23-Jan-2014

Public Class wfDueJobKitReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mSelectDueJobsForReport As SelectDueJobsForReport
    Public mDueLimits As DueLimits
    Dim mDueLimit As DueLimit
    Dim mMachineNameValueList As MachineNameValueList
    Dim mStoreList As StoreList
    Public strStore, strAircraft, Searchstr4 As String
    Dim EventLogDetail As String
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mSelectDueJobsForReport = Session("mSelectDueJobsForReport")
        mDueLimits = Session("mDueLimits")
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub SetSession()
        Session("mSelectDueJobsForReport") = mSelectDueJobsForReport
        Session("mDueLimits") = mDueLimits
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSelectDueJobsForReport")
        Session.Remove("mDueLimits")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriod.Rows.Count - 1
            txtLimit = CType(Me.dgDuePeriod.Rows(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text)
        Next i
        Session("mDueLimits") = mDueLimits
    End Sub
    Private Sub SetReport()
        GetSession()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsSelectDueJobForReport
        Dim mCompanyDetail As New CompanyDetail
        Dim mMaintenanceKitDetails As MaintenanceKitDetails
        Dim OnTypeID As Integer
        Dim MonitorTypeID As Integer

        If rbSummary.Checked = True Then
            myReport = New crSelectDueJobSummaryForReport
        ElseIf rbDetail.Checked = True Then
            myReport = New crSelectDueJobForReport
        End If
        SetGridObject()
        'mSelectDueJobsForReport = SelectDueJobsForReport.GetSelectDueJobsForReport(txtAsOnDate.Value.ToString, mDueLimits, IIf(cmbAircraft.SelectedValue.ToString = "", Guid.Empty.ToString, cmbAircraft.SelectedValue.ToString).ToString, CInt(IIf(txtAvgMonth.Text <> "", txtAvgMonth.Text, 0)))
        mSelectDueJobsForReport = SelectDueJobsForReport.GetSelectDueJobsForReport(txtAsOnDate.Text, mDueLimits, IIf(cmbAircraft.SelectedValue.ToString = "", Guid.Empty.ToString, cmbAircraft.SelectedValue.ToString).ToString, 0)

        '----For Multiple ID's----
        Dim StringMaintenanceTypeID As String = ""
        Dim StringBeforeDataID As String = ""
        Dim StringIsAssembly As String = ""

        For i As Integer = 0 To mSelectDueJobsForReport.Count - 1

            'MonitorTypeID (1:Servicing/2:Inspection/3:Modification)
            If mSelectDueJobsForReport.Item(i).DataType = "Servicing" Then
                MonitorTypeID = 1
            ElseIf mSelectDueJobsForReport.Item(i).DataType = "Inspection" Then
                MonitorTypeID = 2
            ElseIf mSelectDueJobsForReport.Item(i).DataType = "Modification" Then
                MonitorTypeID = 3
            End If

            'OnTypeID (1:Assembly/2:Component)
            If mSelectDueJobsForReport.Item(i).OnAssemblyOrComponent = "Assembly" Then
                OnTypeID = 1
            ElseIf mSelectDueJobsForReport.Item(i).OnAssemblyOrComponent = "Component" Then
                OnTypeID = 2
            End If

            If StringMaintenanceTypeID.Length = 0 Then
                StringMaintenanceTypeID = MonitorTypeID.ToString
            Else
                StringMaintenanceTypeID = StringMaintenanceTypeID + "," + MonitorTypeID.ToString
            End If

            'BeforeDataID
            If StringBeforeDataID.Length = 0 Then
                StringBeforeDataID = "{" + mSelectDueJobsForReport.Item(i).ID.ToString + "}"
            Else
                StringBeforeDataID = StringBeforeDataID + "," + "{" + mSelectDueJobsForReport.Item(i).ID.ToString + "}"
            End If

            'IsAssembly
            If StringIsAssembly.Length = 0 Then
                StringIsAssembly = IIf(OnTypeID = 1, 1, 0).ToString
            Else
                StringIsAssembly = StringIsAssembly + "," + IIf(OnTypeID = 1, 1, 0).ToString
            End If
        Next
        'Store
        If cmbStore.SelectedIndex = 0 Then
            strStore = ""
        Else
            strStore = cmbStore.SelectedItem.ToString
        End If
        'Aircraft
        strAircraft = cmbAircraft.SelectedItem.ToString
        'Period
        For Each mDueLimit In mDueLimits
            If CDec(Val(mDueLimit.PeriodLimit)) >= 0 Then
                If Searchstr4 = "" Then
                    Searchstr4 = "For Next" & " " & Searchstr4 & " " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                Else
                    Searchstr4 = Searchstr4 & ", " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                End If
            End If
        Next

        mMaintenanceKitDetails = MaintenanceKitDetails.GetMaintenanceKitDetailsForReport(StringMaintenanceTypeID, StringBeforeDataID, StringIsAssembly, strStore)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                        mCompanyDetail.WebSite, "Inspection Spares Required", New SmartDate(txtAsOnDate.Text).FormattedText, strAircraft, strStore, Searchstr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mSelectDueJobsForReport.Count = 0 And rbDetail.Checked = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mMaintenanceKitDetails.Count = 0 And rbSummary.Checked = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1130)
        End If

        da.Fill(ds, mSelectDueJobsForReport)
        da.Fill(ds, mMaintenanceKitDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage
        mrptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", "openTranDetail();", True)
        EventLogDetail = "AsOnDate : " + txtAsOnDate.Text + "," + "Aircraft : " + strAircraft + "," + "Store : " + strStore + "," + IIf(rbDetail.Checked, "Detail", "Summary") + "," + Searchstr4
        MarkLog(Util.Action.Print, "InspectionSparesRequiredReport", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , False, , , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        mDueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))
        dgDuePeriod.DataSource = mDueLimits

        'Store
        mStoreList = StoreList.GetStoreList(0, "", "(All)")
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList

        'mSelectDueJobsForReport = SelectDueJobsForReport.GetSelectDueJobsForReport(txtAsOnDate.Value.ToString, mDueLimits, IIf(cmbAircraft.SelectedValue.ToString = "", Guid.Empty.ToString, cmbAircraft.SelectedValue.ToString).ToString, CInt(IIf(txtAvgMonth.Text <> "", txtAvgMonth.Text, 0)))
        mSelectDueJobsForReport = SelectDueJobsForReport.GetSelectDueJobsForReport(txtAsOnDate.Text, mDueLimits, IIf(mMachineNameValueList.Count = 0, Guid.Empty.ToString, mMachineNameValueList.Item(0).ID.ToString).ToString, 0)
        dgDueJob.DataSource = mSelectDueJobsForReport
        Session("mSelectDueJobsForReport") = mSelectDueJobsForReport
        lblResult.Text = "List of Due Jobs as per criteria :" & mSelectDueJobsForReport.Count & " Record(s) found."
        Session("mDueLimits") = mDueLimits
        DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If txtAsOnDate.Text = "" Then
            txtAsOnDate.Text = Date.Now.ToString(AppSettings("DateFormat"))
        End If
        If Not IsPostBack Then
            If cmbAircraft.Enabled = True Then
                setFocus(cmbAircraft)
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            SetGridObject()
            'mSelectDueJobsForReport = SelectDueJobsForReport.GetSelectDueJobsForReport(txtAsOnDate.Value.ToString, mDueLimits, IIf(cmbAircraft.SelectedValue.ToString = "", Guid.Empty.ToString, cmbAircraft.SelectedValue.ToString).ToString, CInt(IIf(txtAvgMonth.Text <> "", txtAvgMonth.Text, 0)))
            mSelectDueJobsForReport = SelectDueJobsForReport.GetSelectDueJobsForReport(txtAsOnDate.Text, mDueLimits, cmbAircraft.SelectedValue.ToString, 0)

            dgDueJob.DataSource = mSelectDueJobsForReport
            lblResult.Text = "List of Due Jobs as per criteria :" & mSelectDueJobsForReport.Count & " Record(s) found."
            Session("mSelectDueJobsForReport") = mSelectDueJobsForReport
            mDueLimits = Session("mDueLimits")
            dgDuePeriod.DataSource = mDueLimits
            DataBind()
            upnlGrid.Update()
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If IsValid Then 
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    'New addition by Rupali on 18-Jun-09 for Sorting Order
    Private Sub dgDueJob_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueJob.Sorting
        mSelectDueJobsForReport.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mSelectDueJobsForReport") = mSelectDueJobsForReport
        dgDueJob.DataSource = mSelectDueJobsForReport
        dgDueJob.DataBind()
    End Sub
#End Region

End Class