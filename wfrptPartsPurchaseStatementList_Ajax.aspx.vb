Partial Class wfrptPartsPurchaseStatementList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mReceivingStoreList As StoreList
    Public FromDate As String
    Public ToDate As String
    Public mCategoryList As CategoryList  'Added By Utkarsh On 12-Oct-2012 FOR ALL11102012-2
    Public strCategory As String
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    Dim Rate As String
    Dim email As Thread
    Public mModelList As ModelList
    Public PartNo As String = ""
    Public Description As String = ""
    Public mSupplierList As VendorList
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = Session("mCategoryList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblCategory1.Visible = True
        lblReceiptType.Visible = True
        lblModel1.Visible = True
        lblStoreName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblSupp.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then      ''Date Range
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        strCategory = String.Empty

        For i As Integer = 0 To ChklistCategory.Items.Count - 1
            If ChklistCategory.Items(i).Selected Then
                If strCategory.Length = 0 Then
                    strCategory = ChklistCategory.Items(i).Text
                Else
                    strCategory = strCategory + "," + ChklistCategory.Items(i).Text
                End If
            End If
        Next
        lblCategory1.Text = "Category Name : " & IIf(strCategory.Length > 0, strCategory, "All")
        'End
        Rate = IIf(rdoBase.Checked Or rdoCommercial.Checked, IIf(rdoBase.Checked, "By Base Value", "By Commercial Value"), "By Landing Value")
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        If cmbModelType.SelectedIndex = 0 Then       ''Model
            lblModel1.Text = "Model : All"
        Else
            lblModel1.Text = "Model : " & IIf(cmbModelType.SelectedIndex > 0, cmbModelType.SelectedItem.Text, "")
        End If
        If cmbReceivingStore.SelectedIndex = 0 Then
            lblStoreName.Text = "Store : All"
        Else
            lblStoreName.Text = "Store : " & IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, "")
        End If
        lblReceiptType.Text = "Receipt Type : " + cmbReceiptType.SelectedItem.ToString
        If cmbSupplier.SelectedIndex = 0 Then
            lblSupp.Text = "Supplier : All"
        Else
            lblSupp.Text = "Supplier : " & IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, "")
        End If
        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + ", " + lblCategory1.Text + ", " + Rate + ", " + PartNo + ", " + Description + ", " + lblModel1.Text + ", " + lblReceiptType.Text + ", " + lblStoreName.Text + ", " + lblSupp.Text
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt As PartsPurchaseStatementList
            SetValues()
            'Added By Shweta on 18-Dec-2012
            Dim Value As String = ""
            Dim ReportName As String = ""
            If rdoBase.Checked = True Then
                Value = "Base Value"
                ReportName = " Part Purchase Statement (Base Value)"
            ElseIf rdoLanding.Checked = True Then
                Value = "Landing Value"
                ReportName = " Part Purchase Statement (Landing Value)"
            Else
                Value = "Commercial Value"
                ReportName = " Part Purchase Statement (Commercial Value)"
            End If
            'End
            'Added BY Shweta on 12-Feb-2013 For Heligo12022013
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                myReport = New crptPartsPurchaseStatementListForHeligo
            Else
                myReport = New crptPartsPurchaseStatementList
            End If
            rpt = PartsPurchaseStatementList.GetPartsPurchaseStatementList(FromDate:=FromDate, ToDate:=ToDate, PartName:=PartNo, Description:=Description, _
                                                                           CategoryList:=strCategory, Value:=Value, ClientCode:=AppSettings("ClientCode"), _
                                                                           ModelID:=cmbModelType.SelectedValue, ReceiptType:=CInt(cmbReceiptType.SelectedValue), _
                                                                           StoreID:=cmbReceivingStore.SelectedValue.ToString, VendorID:=cmbSupplier.SelectedValue.ToString) 'Value Parameter added by Shweta on 18-Dec-2012
            'End If
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1251)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                          SmtpHost:=mModuleList.Item("PurchaseStatement").SmtpHost, SmtpPort:=mModuleList.Item("PurchaseStatement").SmtpPort, _
                                          SmtpUser:=mModuleList.Item("PurchaseStatement").SmtpUser, SmtpPassword:=mModuleList.Item("PurchaseStatement").SmtpPassword)
                Exit Sub
            End If
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, cmbSupplier.SelectedItem.Text, "", _
                                                                  strCategory, IIf(cmbModelType.SelectedIndex > 0, cmbModelType.SelectedItem.Text, ""), _
                                                                  IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), _
                                                                  cmbReceiptType.SelectedItem.Text, ReportName, Description, "", 0, "", "", "", _
                                                                  AppSettings("Logo"))
            Dim ds As New dsPartPurchaseStatementList
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            If IsExcel = False Then
                da.Fill(ds, mrptImage)
            End If
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            MarkLog(Util.Action.Print, "PartPurchaseStatementList", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRangeFrom.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                          SmtpHost:=mModuleList.Item("PurchaseStatement").SmtpHost, SmtpPort:=mModuleList.Item("PurchaseStatement").SmtpPort, _
                                          SmtpUser:=mModuleList.Item("PurchaseStatement").SmtpUser, SmtpPassword:=mModuleList.Item("PurchaseStatement").SmtpPassword)
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
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList()
        ChklistCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList
        'Model
        mModelList = ModelList.GetModelList(1, "", , , "(All)")
        cmbModelType.DataSource = mModelList

        mReceivingStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbReceivingStore.DataSource = mReceivingStoreList
        lblStoreCount.Text = "You have " + (mReceivingStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mReceivingStoreList.TotalStorelistCount.ToString + " Store(s)"

        mSupplierList = VendorList.GetVendorstList(0, SelectTag:="(All)", IsSupplier:=True)
        cmbSupplier.DataSource = mSupplierList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            'Ajay 09-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "PurchaseStatement") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            DataFieldBind()
            cmbDateRange.SelectedIndex = 6
            ControlVisibility(6)
            setDatePeroid(6)
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False, False)
        Else
            upnlValidationSummary.Update()
        End If
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
        If IsValid Then
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

            Session("UserEmailID") = mModuleList.Item("PurchaseStatement").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("PurchaseStatement").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim rpt As PartsPurchaseStatementList
            Dim objsearch As rptSearchingCriteria
            Dim ds As New dsExcelPartPurchaseStatementList
            SetValues()
            Dim Value As String = ""
            Dim ReportName As String = ""
            If rdoBase.Checked = True Then
                Value = "Base Value"
                ReportName = " Part Purchase Statement (Base Value)"
            ElseIf rdoLanding.Checked = True Then
                Value = "Landing Value"
                ReportName = " Part Purchase Statement (Landing Value)"
            Else
                Value = "Commercial Value"
                ReportName = " Part Purchase Statement (Commercial Value)"
            End If
            rpt = PartsPurchaseStatementList.GetPartsPurchaseStatementList(FromDate:=FromDate, ToDate:=ToDate, PartName:=PartNo, Description:=Description, _
                                                                           CategoryList:=strCategory, Value:=Value, ClientCode:=AppSettings("ClientCode"), _
                                                                           ModelID:=cmbModelType.SelectedValue, ReceiptType:=CInt(cmbReceiptType.SelectedValue), _
                                                                           StoreID:=cmbReceivingStore.SelectedValue.ToString, VendorID:=cmbSupplier.SelectedValue.ToString)
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, cmbSupplier.SelectedItem.Text, "", _
                                                                  strCategory, IIf(cmbModelType.SelectedIndex > 0, cmbModelType.SelectedItem.Text, ""), _
                                                                  IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), _
                                                                  cmbReceiptType.SelectedItem.Text, ReportName, Description, "", 0, "", "", "", _
                                                                  AppSettings("Logo"))
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ds.Clear()
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            da.Fill(ds, "PartsPurchaseStatementList", rpt)
            Dim columnToRemove2 As String() = {"CompanyName", "BranchName", "SupplierName", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"PartID", "VendorInvoiceDate", "VendorInvoiceNo", "CRate", "Amount", "Amend", "EffRate", "CAmount", "CEffRate", "OrderTransTypeID", "OrderIsOverhaul", "ReceiptTransTypeID", "VendorID"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("PartsPurchaseStatementList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("PartsPurchaseStatementList").Columns.Remove(columnToRemove(i))
                End If
            Next
            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("rptSearchingCriteria").Columns("Nomenclature").ColumnName = "Model"
            ds.Tables("rptSearchingCriteria").Columns("Aircraft").ColumnName = "Type"
            ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            ds.Tables("PartsPurchaseStatementList").Columns("TotalCEffRatePrice").ColumnName = "Invoice Price"
            ds.Tables("PartsPurchaseStatementList").Columns("TotalPrice").ColumnName = "Base Price"
            ds.Tables("PartsPurchaseStatementList").Columns("ReceiptType").ColumnName = "Type"
			ds.Tables("PartsPurchaseStatementList").TableName = "Part Purchase Statement"
			Session("ExcelFileName") = "Part Purchase Statement"
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "PurchaseStatement", "Export To excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
    End Sub
    'Ajay 09-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "PurchaseStatement")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "PurchaseStatement")
    End Sub
    '-----
#End Region



End Class