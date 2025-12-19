Imports System.Web.Mail
Imports Flypal.SendMailFile
Public Class wfrptValuationSummaryAnalysis_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public FromDate As String
    Public ToDate As String
    Public strStore As String
    Public Type As Int16
    Public mStoreID As Guid
    Dim NameOfStore As String = String.Empty
    Dim EventLogDetails As String = String.Empty
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
   Private Sub RemoveSession()
        Session.Remove("mStoreList")
   End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
     Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblStoreName.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText
        If cmbStore.SelectedIndex = 0 Then          ''Store
            strStore = ""
            lblStoreName.Text = "Store : All"
            NameOfStore = ""
        Else
            strStore = Store.GetStore(New Guid(cmbStore.SelectedValue)).Name
            NameOfStore = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "")
            lblStoreName.Text = "Store : " & NameOfStore
        End If
        mStoreID = mStoreList.Item(cmbStore.SelectedIndex).ID
        If optAll.Checked Then
            Type = 1  'All Status except Canceled
        Else
            Type = 0  'Only Authorized
        End If
        EventLogDetails = lblDateRangeFrom.Text + ", " + lblStoreName.Text + "," + IIf(rbBase.Checked, "By Base Value", IIf(rbLanding.Checked, "By Landing Value", "By Commercial Value")) + "," + IIf(optAll.Checked, "All", "Only Authorized")
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False, Optional ByVal ByMail As Boolean = False)
        Try
            'Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt1 As rptValuationSummaryForOpeningBalance
            Dim rpt2 As rptValuationSummaryForPurchase
            Dim rpt3 As rptValuationSummaryForConsumption
            Dim rpt4 As rptValuationSummaryForClosingStock
            Dim mStoreID As Guid = mStoreList.Item(cmbStore.SelectedIndex).ID
            Dim Value As String = ""
            Dim ReportName As String = ""
            SetValues()
            Dim ds As New dsValuationSummary
            If rbBase.Checked = True Then
                Value = "Base Value"
                ReportName = "Valuation Report (Base Value)"
            ElseIf rbLanding.Checked = True Then
                Value = "Landing Value"
                ReportName = "Valuation Report (Landing Value)"
            ElseIf rbCommercial.Checked = True Then
                Value = "Commercial Value"
                ReportName = "Valuation Report (Commercial Value)"
            End If
            myReport = New crptValuationSummary
            rpt1 = rptValuationSummaryForOpeningBalance.GetValuation(FromDate, ToDate, strStore, mStoreID, Type, Value)
            rpt2 = rptValuationSummaryForPurchase.GetValuation(FromDate, ToDate, strStore, mStoreID, Type, Value)
            rpt3 = rptValuationSummaryForConsumption.GetValuation(FromDate, ToDate, strStore, mStoreID, Type, Value)
            rpt4 = rptValuationSummaryForClosingStock.GetValuation(FromDate, ToDate, strStore, mStoreID, Type, Value)

            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, "", "", "", "", "", NameOfStore, "", "", "", ReportName, 0, "", "", "", AppSettings("Logo"))
            If ByMail = False Then
                If (rpt1.Count <= 0 And rpt2.Count <= 0 And rpt2.Count <= 0 And rpt3.Count <= 0 And rpt4.Count <= 0) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1325)
                End If
            End If
            If (ByMail = True And (rpt1.Count <= 0 And rpt2.Count <= 0 And rpt2.Count <= 0 And rpt3.Count <= 0 And rpt4.Count <= 0)) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                       SmtpHost:=mModuleList.Item("ValuationSummaryAnalysis").SmtpHost, SmtpPort:=mModuleList.Item("ValuationSummaryAnalysis").SmtpPort, _
                    SmtpUser:=mModuleList.Item("ValuationSummaryAnalysis").SmtpUser, SmtpPassword:=mModuleList.Item("ValuationSummaryAnalysis").SmtpPassword)
                Exit Sub
            End If
            ds.Clear()
            If IsExcel = False Then
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, mrptImage)
            End If
            da.Fill(ds, rpt1)
            da.Fill(ds, rpt2)
            da.Fill(ds, rpt3)
            da.Fill(ds, rpt4)
            da.Fill(ds, objsearch)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
                MarkLog(Util.Action.Print, "ValuationSummaryAnalysis", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRangeFrom.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                       SmtpHost:=mModuleList.Item("ValuationSummaryAnalysis").SmtpHost, SmtpPort:=mModuleList.Item("ValuationSummaryAnalysis").SmtpPort, _
                    SmtpUser:=mModuleList.Item("ValuationSummaryAnalysis").SmtpUser, SmtpPassword:=mModuleList.Item("ValuationSummaryAnalysis").SmtpPassword)
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
#End Region

#Region " Data Binding "
    Private Sub SetCombo()
        If cmbFromYear.Items.Count = 0 Or cmbFromYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbFromYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbFromYear.SelectedIndex = 10
        End If

        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbFromMonth.Items.Add(mon)
        Next
    End Sub
    Private Sub DataFieldBind()
        'Store
        mStoreList = StoreList.GetStoreList(0, "", "(All)", IsForUserStoreRights:=True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        DataBind()
    End Sub
    Private Sub DateBind()
        txtFromDate.Text = DateSerial(cmbFromYear.SelectedValue, cmbFromMonth.SelectedIndex + 1, 1).ToString(AppSettings("DateFormat").ToString)
        txtFromDate.DataBind()
        txtToDate.Text = CDate(txtFromDate.Text).AddYears(1).AddDays(-1).ToString(AppSettings("DateFormat").ToString)
        txtToDate.DataBind()
        txtToYear.Text = CDate(txtToDate.Text).Year.ToString
        txtToYear.DataBind()
        txtToMonth.Text = MonthName(Month(CDate(txtToDate.Text)), False)
        txtToMonth.DataBind()

        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
        lblStoreCount.DataBind()
        upnlMonth.Update()
    End Sub

#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()
            rbLanding.Checked = True
            SetCombo()
            DataFieldBind()
            DateBind()
        End If
    End Sub
    Protected Sub cmbFromMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFromMonth.SelectedIndexChanged
        DateBind()
    End Sub
    Protected Sub cmbFromYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFromYear.SelectedIndexChanged
       DateBind()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        'If DateDiff(DateInterval.Day, CDate(txtToDate.Text), CDate(txtFromDate.Text)) > 366 Then
        If DateDiff(DateInterval.Day, New SmartDate(txtFromDate.Text.ToString).Date, New SmartDate(txtToDate.Text.ToString).Date) >= 366 Then
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Date range should be 12 month.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        SetReport(False)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("ValuationSummaryAnalysis").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("ValuationSummaryAnalysis").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    'Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
    '    Dim da As New CSLA.Data.ObjectAdapter
    '    Dim objsearch As rptSearchingCriteria
    '    Dim rpt As rptValuation
    '    Dim mStoreID As Guid = mStoreList.Item(cmbStore.SelectedIndex).ID
    '    DimGuid.Empty.ToString As Guid = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
    '    DimGuid.Empty.ToString As Guid = mModelList.Item(cmbModelType.SelectedIndex).ID
    '    Dim Guid.Empty As Guid = mCategoryList.Item(cmbCategory.SelectedIndex).ID 'Added by Shweta on 13-August-2013 for ALL13082013-1
    '    Dim Value As String = ""
    '    Dim ReportName As String = ""
    '    SetValues()
    '    Dim ds As New dsValuation

    '    If rbBase.Checked = True Then
    '        Value = "Base Value"
    '        ReportName = "Valuation Report (Base Value)"
    '    ElseIf rbLanding.Checked = True Then
    '        Value = "Landing Value"
    '        ReportName = "Valuation Report (Landing Value)"
    '    ElseIf rbCommercial.Checked = True Then
    '        Value = "Commercial Value"
    '        ReportName = "Valuation Report (Commercial Value)"
    '    End If

    '    If chkCategoryWise.Checked Then 'Added by Vikrant on 20-July-2012 For All18072012
    '        If rdoCategorySummary.Checked Then
    '            rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, "", "", "", mStoreID, Guid.Empty.ToString, Type, False, Guid.Empty.ToString.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked, chkShowInValuation.Checked, Guid.Empty.ToString)
    '        ElseIf rdoCategoryDetail.Checked Then
    '            rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, "", "", "", mStoreID, Guid.Empty.ToString, Type, False, Guid.Empty.ToString.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked, chkShowInValuation.Checked, Guid.Empty.ToString)
    '        End If
    '    Else
    '        If cmbSortBy.SelectedValue = 1 Then
    '            rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, "", "", "", mStoreID, Guid.Empty.ToString, Type, False, Guid.Empty.ToString.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, Guid.Empty.ToString)
    '        ElseIf (cmbSortBy.SelectedValue = 2) Then
    '            rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, "", "", "", mStoreID, Guid.Empty.ToString, Type, True, Guid.Empty.ToString.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, Guid.Empty.ToString)
    '        Else
    '            rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, "", "", "", mStoreID, Guid.Empty.ToString, Type, False, Guid.Empty.ToString.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, Guid.Empty.ToString)
    '        End If
    '    End If
    '    objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, strVendor, IIf(cmbAssemblyType.SelectedIndex > 0, cmbAssemblyType.SelectedItem.Text, ""), "", "", NameOfStore, "", IIf(cmbModelType.SelectedIndex > 0, cmbModelType.SelectedItem.Text, ""), "", ReportName, 0, "", IIf((cmbCustomer.Enabled And cmbCustomer.SelectedIndex > 0), cmbCustomer.SelectedItem.Text, ""), "", AppSettings("Logo"))

    '    If rpt.Count <= 0 Then
    '        MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '        Exit Sub
    '    End If

    '    ds.Clear()
    '    da.Fill(ds, "ExcelrptValuation", rpt)
    '    da.Fill(ds, "ExcelrptSearchingCriteria", objsearch)

    '    Dim columnToRemove1 As String() = {"ItemID", "StoreName", "NomenclatureName", "CategoryName", "PendingInvQty", "Folio", "IsSortByFolio", "CategoryGLCode", "IssuedQty", "IssuedAmount", "DiscardQty", "DiscardAmount", "CategoryID", "InQty", "OutQty"}
    '    Dim columnToRemove2 As String() = {"CompanyName", "Aircraft", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
    '    For i As Integer = 0 To columnToRemove1.Length - 1
    '        If ds.Tables("ExcelrptValuation").Columns.Contains(columnToRemove1(i)) Then
    '            ds.Tables("ExcelrptValuation").Columns.Remove(columnToRemove1(i))
    '        End If
    '    Next

    '    For i As Integer = 0 To columnToRemove2.Length - 1
    '        If ds.Tables("ExcelrptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
    '            ds.Tables("ExcelrptSearchingCriteria").Columns.Remove(columnToRemove2(i))
    '        End If
    '    Next

    '    Dim dsNew As New DataSet
    '    dsNew.Clear()

    '    dsNew.Merge(ds.Tables("ExcelrptSearchingCriteria"))
    '    dsNew.Merge(ds.Tables("ExcelrptValuation"))

    '    dsNew.Tables("ExcelrptSearchingCriteria").Columns("WorkShop").ColumnName = "Customer"
    '    dsNew.Tables("ExcelrptSearchingCriteria").Columns("BranchName").ColumnName = "Assembly"
    '    dsNew.Tables("ExcelrptSearchingCriteria").Columns("KitName").ColumnName = "Model"

    '    dsNew.Tables("ExcelrptSearchingCriteria").TableName = "Searching Criteria"
    '    dsNew.Tables("ExcelrptValuation").TableName = ReportName

    '    Session("dsNew") = dsNew
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    'End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region


End Class