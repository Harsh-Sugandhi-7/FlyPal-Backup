'AJAX Conversion By Vikrant On 24-Feb-2014
Imports System.Web.Mail
Imports Flypal.SendMailFile
Public Class wfrptValuationAnalysis_Ajax
    Inherits System.Web.UI.Page
   
#Region " Variable Declaration "
    Public mVendorList As VendorList
    Public mStoreList As StoreList
    Public mCustomerList As VendorList
    Public mCategoryList As CategoryList
    Public mNomenclatureList As NomenclatureList
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Public strVendor As String
    Public strCategory As String
    Public strCustomer As String
    Public strStore As String
    Public strNomenclature, ModelName, AssemblyType As String
    Public AssemblyTypeID As Integer
    Public Type As Int16
    Public mAssemblyTypeList As AssemblyTypeList
    Public mModelList As ModelList
    Public mStoreID As Guid         'As on 09-05-2008 By Kalpesh Shah
    Public mCustomerID As Guid      'As on 09-05-2008 By Kalpesh Shah
    Dim NameOfStore As String = String.Empty  'Added by Prashant 5-Apr-2013 'ALL05042013
    Dim EventLogDetails As String = String.Empty
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        mVendorList = CType(Session("mVendorlist"), VendorList)
        mNomenclatureList = CType(Session("mNomenclatureList"), NomenclatureList)
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelList = CType(Session("mModelList"), ModelList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mStoreList") = mStoreList
        Session("mCategoryList") = mCategoryList
        Session("mCustomerList") = mCustomerList
        Session("mVendorlist") = mVendorList
        Session("mNomenclatureList") = mNomenclatureList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mModelList") = mModelList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mStoreList")
        Session.Remove("mCategoryList")
        Session.Remove("mVendorlist")
        Session.Remove("mNomenclatureList")
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelList")
        Session.Remove("mCustomerList")
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
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        upnlDates.Update()
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblCustomerName.Visible = True
        lblStoreName.Visible = True
        lblCategoryName.Visible = True
        lblVendorName.Visible = True
        lblNomenclatureName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
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
        If cmbCustomer.SelectedIndex = 0 Then                  'Customer
            lblCustomerName.Text = "Customer : All"
        Else
            strCustomer = Vendor.GetVendor(New Guid(cmbCustomer.SelectedValue)).Name
            lblCustomerName.Text = "Customer :" & strCustomer
        End If
        If cmbStore.SelectedIndex = 0 Then          ''Store
            strStore = ""
            lblStoreName.Text = "Store : All"
            NameOfStore = ""
        Else
            strStore = Store.GetStore(New Guid(cmbStore.SelectedValue)).Name
            NameOfStore = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "")
            lblStoreName.Text = "Store : " & NameOfStore
        End If
        mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID  'As on 09-05-2008 By Kalpesh Shah
        mStoreID = mStoreList.Item(cmbStore.SelectedIndex).ID           'As on 09-05-2008 By Kalpesh Shah

        If cmbCategory.SelectedIndex = 0 Then       ''Category
            strCategory = ""
            lblCategoryName.Text = "Category : All"
        Else
            strCategory = Category.GetCategory(New Guid(cmbCategory.SelectedValue)).Name
            lblCategoryName.Text = "Category : " & strCategory
        End If

        If cmbVendor.SelectedIndex = 0 Then         ''Vendor
            strVendor = ""
            lblVendorName.Text = "Supplier : All"
        Else
            strVendor = Vendor.GetVendor(New Guid(cmbVendor.SelectedValue)).Name
            lblVendorName.Text = "Supplier : " & strVendor
        End If

        If cmbNomenclature.SelectedIndex = 0 Then   ''Nomenclature
            strNomenclature = ""
            lblNomenclatureName.Text = "Nomenclature : All"
        Else
            strNomenclature = NomenClature.GetNomenclature(New Guid(cmbNomenclature.SelectedValue)).Name
            lblNomenclatureName.Text = "Nomenclature : " & strNomenclature
        End If

        If cmbModelType.SelectedIndex = 0 Then       ''Model
            ModelName = ""
            lblModel1.Text = "Model : All"
        Else
            ModelName = Model.GetModel(New Guid(cmbModelType.SelectedValue)).Name
            lblModel1.Text = "Model : " & ModelName
        End If
        If cmbAssemblyType.SelectedIndex = 0 Then    ''Assembly
            lblAssembly1.Text = "Assembly : All"
            AssemblyTypeID = 0
        Else
            AssemblyType = cmbAssemblyType.SelectedItem.ToString
            lblAssembly1.Text = "Assembly : " & AssemblyType
            AssemblyTypeID = mAssemblyTypeList.Item(cmbAssemblyType.SelectedIndex).ID
        End If

        If optAll.Checked Then
            Type = 1  'All Status except Canceled
        Else
            Type = 0  'Only Authorized
        End If

        'Added By Utkarsh ON 28-Nov-2012 FOR ALL28112012
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        'End
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

        EventLogDetails = lblDateRangeFrom.Text + "," + IIf(chkCustomerStock.Checked, "Checked Customer Stock", "") + "," + lblCustomerName.Text + "," + lblStoreName.Text + "," + lblNomenclatureName.Text + "," + lblAssembly1.Text + "," + lblCategoryName.Text + "," + lblVendorName.Text + "," + lblModel1.Text + "," + IIf(rbBase.Checked, "By Base Value", IIf(rbLanding.Checked, "By Landing Value", "By Commercial Value")) + "," + lblPartNo.Text + "," + lblDesc.Text + "," + IIf(chkShowInValuation.Checked, chkShowInValuation.Text, "") + "," + IIf(chkCategoryWise.Checked, "Sort By : Part No " + IIf(rdoCategoryDetail.Checked, "Detail", "Summary"), "Sort By : " + cmbSortBy.SelectedItem.Text) + "," + "Transactions : " + IIf(optAll.Checked, "All", "Only Authorized")
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False, Optional ByVal ByMail As Boolean = False)
        Try
            'Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt As rptValuation
            Dim mStoreID As Guid = mStoreList.Item(cmbStore.SelectedIndex).ID
            Dim mCustomerID As Guid = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
            Dim mModelID As Guid = mModelList.Item(cmbModelType.SelectedIndex).ID
            Dim mCategoryID As Guid = mCategoryList.Item(cmbCategory.SelectedIndex).ID 'Added by Shweta on 13-August-2013 for ALL13082013-1
            Dim Value As String = ""
            Dim ReportName As String = ""
            SetValues()
            Dim ds As New dsValuation
            If rbBase.Checked = True Then
                Value = "Base Value"
                ReportName = "Valuation Report (Base Value)"
            ElseIf rbLanding.Checked = True Then
                Value = "Landing Value"
                ReportName = IIf(chkWithGST.Visible, IIf(chkWithGST.Checked, "Valuation Report (Landing Value)", "Valuation Report (Landing Value excluding GST)"), "Valuation Report (Landing Value)")
            ElseIf rbCommercial.Checked = True Then
                Value = "Commercial Value"
                ReportName = "Valuation Report (Commercial Value)"
            End If
            If chkCategoryWise.Checked Then 'Added by Vikrant on 20-July-2012 For All18072012
                If rdoCategorySummary.Checked Then
                    'old
                    myReport = New crptCategoryWiseValuationForHeligo
                    rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked, chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked) 'CategoryID added by Shweta on 13-August-2013 for ALL13082013-1
                    'mrptCurrencyListForValuation = rptCurrencyListForValuation.GetrptCurrencyListForValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked)
                ElseIf rdoCategoryDetail.Checked Then
                    'New
                    myReport = New crptCategoryWiseValuationDetail
                    rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked, chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked) 'CategoryID added by Shweta on 13-August-2013 for ALL13082013-1
                    ' mrptCurrencyListForValuation = rptCurrencyListForValuation.GetrptCurrencyListForValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked)
                End If
            Else
                'old
                If cmbSortBy.SelectedValue = 1 Then
                    myReport = New crptValuationByDescriptionDeccan
                    rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked) 'CategoryID added by Shweta on 13-August-2013 for ALL13082013-1
                    ' mrptCurrencyListForValuation = rptCurrencyListForValuation.GetrptCurrencyListForValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value)
                ElseIf (cmbSortBy.SelectedValue = 2) Then
                    myReport = New crptValuationByFolio
                    rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, True, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked) 'CategoryID added by Shweta on 13-August-2013 for ALL13082013-1
                    'mrptCurrencyListForValuation = rptCurrencyListForValuation.GetrptCurrencyListForValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, True, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value)
                Else
                    myReport = New crptValuationByPartNoDeccan
                    rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked) 'CategoryID added by Shweta on 13-August-2013 for ALL13082013-1
                    'mrptCurrencyListForValuation = rptCurrencyListForValuation.GetrptCurrencyListForValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value)
                End If
            End If
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, strVendor, "", strCategory, strNomenclature, NameOfStore, "", "", Description, ReportName, 0, "", "", "", AppSettings("Logo"))
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 801)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("ValuationAnalysis").SmtpHost, SmtpPort:=mModuleList.Item("ValuationAnalysis").SmtpPort, _
                    SmtpUser:=mModuleList.Item("ValuationAnalysis").SmtpUser, SmtpPassword:=mModuleList.Item("ValuationAnalysis").SmtpPassword)
                Exit Sub
            End If
            ds.Clear()
            If IsExcel = False Then
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, mrptImage)
            End If
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
                MarkLog(Util.Action.Print, "ValuationAnalysis", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRangeFrom.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("ValuationAnalysis").SmtpHost, SmtpPort:=mModuleList.Item("ValuationAnalysis").SmtpPort, _
                    SmtpUser:=mModuleList.Item("ValuationAnalysis").SmtpUser, SmtpPassword:=mModuleList.Item("ValuationAnalysis").SmtpPassword)
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
    Private Sub ControlVisibility() 'Added By Vikrant on 20-July-2012 For All18072012
        If chkCategoryWise.Checked Then
            rdoCategorySummary.Enabled = True
            rdoCategoryDetail.Enabled = True
            cmbSortBy.Enabled = False
        Else
            rdoCategorySummary.Enabled = False
            rdoCategoryDetail.Enabled = False
            cmbSortBy.Enabled = True
        End If
    End Sub 'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Customer
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, False)
        cmbCustomer.DataSource = mCustomerList
        Session("mCustomerList") = mCustomerList

        'Store
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
        'Category
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList

        'Vendor
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbVendor.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        'Nomenclature
        mNomenclatureList = NomenclatureList.GetNomenclatureList("(All)")
        cmbNomenclature.DataSource = mNomenclatureList
        Session(" mNomenclatureList") = mNomenclatureList

        'Assembly
        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList("(All)")
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList

        'Model
        mModelList = ModelList.GetModelList(0, "", , , "(All)")
        cmbModelType.DataSource = mModelList
        Session("mModelList") = mModelList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            rbLanding.Checked = True
            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
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
        Session("UserEmailID") = mModuleList.Item("ValuationAnalysis").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("ValuationAnalysis").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptValuation
        Dim mStoreID As Guid = mStoreList.Item(cmbStore.SelectedIndex).ID
        Dim mCustomerID As Guid = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
        Dim mModelID As Guid = mModelList.Item(cmbModelType.SelectedIndex).ID
        Dim mCategoryID As Guid = mCategoryList.Item(cmbCategory.SelectedIndex).ID 'Added by Shweta on 13-August-2013 for ALL13082013-1
        Dim Value As String = ""
        Dim ReportName As String = ""
        SetValues()
        Dim ds As New dsValuation

        If rbBase.Checked = True Then
            Value = "Base Value"
            ReportName = "Valuation Report (Base Value)"
        ElseIf rbLanding.Checked = True Then
            Value = "Landing Value"
            ReportName = IIf(chkWithGST.Visible, IIf(chkWithGST.Checked, "Valuation Report (Landing Value)", "Valuation Report (Landing Value excluding GST)"), "Valuation Report (Landing Value)")
        ElseIf rbCommercial.Checked = True Then
            Value = "Commercial Value"
            ReportName = "Valuation Report (Commercial Value)"
        End If

        If chkCategoryWise.Checked Then 'Added by Vikrant on 20-July-2012 For All18072012
            If rdoCategorySummary.Checked Then
                rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked, chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked)
            ElseIf rdoCategoryDetail.Checked Then
                rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, chkCategoryWise.Checked, chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked)
            End If
        Else
            If cmbSortBy.SelectedValue = 1 Then
                rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked)
            ElseIf (cmbSortBy.SelectedValue = 2) Then
                rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, True, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked)
            Else
                rpt = rptValuation.GetValuation(FromDate, ToDate, PartNo, strVendor, strStore, strCategory, Description, strNomenclature, mStoreID, mCustomerID, Type, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, Value, , chkShowInValuation.Checked, mCategoryID.ToString, EffRateWithGST:=chkWithGST.Checked)
            End If
        End If
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, strVendor, IIf(cmbAssemblyType.SelectedIndex > 0, cmbAssemblyType.SelectedItem.Text, ""), strCategory, strNomenclature, NameOfStore, "", IIf(cmbModelType.SelectedIndex > 0, cmbModelType.SelectedItem.Text, ""), Description, ReportName, 0, "", IIf((cmbCustomer.Enabled And cmbCustomer.SelectedIndex > 0), cmbCustomer.SelectedItem.Text, ""), "", AppSettings("Logo"))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, "ExcelrptValuation", rpt)
        da.Fill(ds, "ExcelrptSearchingCriteria", objsearch)

        Dim columnToRemove1 As String() = {"ItemID", "StoreName", "NomenclatureName", "PendingInvQty", "Folio", "IsSortByFolio", "CategoryGLCode", "IssuedQty", "IssuedAmount", "DiscardQty", "DiscardAmount", "CategoryID", "InQty", "OutQty"}
        Dim columnToRemove2 As String() = {"CompanyName", "Aircraft", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
        For i As Integer = 0 To columnToRemove1.Length - 1
            If ds.Tables("ExcelrptValuation").Columns.Contains(columnToRemove1(i)) Then
                ds.Tables("ExcelrptValuation").Columns.Remove(columnToRemove1(i))
            End If
        Next

        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ExcelrptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ExcelrptSearchingCriteria").Columns.Remove(columnToRemove2(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ExcelrptSearchingCriteria"))
        dsNew.Merge(ds.Tables("ExcelrptValuation"))

        dsNew.Tables("ExcelrptValuation").Columns("CategoryName").ColumnName = "Category"
        dsNew.Tables("ExcelrptValuation").Columns("OtherPurchaseQty").ColumnName = "Internal / Loan Inward Qty. "
        dsNew.Tables("ExcelrptValuation").Columns("OtherPurchaseAmount").ColumnName = "Internal / Loan Inward Amount "
        dsNew.Tables("ExcelrptValuation").Columns("OtherConsumedQty").ColumnName = "Internal / Loan Outward Qty. "
        dsNew.Tables("ExcelrptValuation").Columns("OtherConsumedAmount").ColumnName = "Internal / Loan Outward Amount "

        dsNew.Tables("ExcelrptSearchingCriteria").Columns("WorkShop").ColumnName = "Customer"
        dsNew.Tables("ExcelrptSearchingCriteria").Columns("BranchName").ColumnName = "Assembly"
        dsNew.Tables("ExcelrptSearchingCriteria").Columns("KitName").ColumnName = "Model"

        dsNew.Tables("ExcelrptSearchingCriteria").TableName = "Searching Criteria"
        dsNew.Tables("ExcelrptValuation").TableName = ReportName
		Session("ExcelFileName") = ReportName
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "ValuationAnalysis", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustomer.SelectedIndexChanged
        'Requested for Customer Stores  
        If chkCustomerStock.Checked Then
            If Not cmbCustomer.SelectedIndex <= 0 Then 'If Customer Selected
                mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID

                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
        End If
        cmbStore.DataBind()
        Session("mStoreList") = mStoreList
        If cmbCustomer.Enabled = True Then
            setFocus(cmbCustomer)
        End If
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then
            lblCustomer.Enabled = True
            cmbCustomer.Enabled = True

            If Not cmbCustomer.SelectedIndex <= 0 Then                       'If Customer Selected
                mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            setFocus(cmbCustomer)
        Else
            cmbCustomer.SelectedIndex = 0
            lblCustomer.Enabled = False
            cmbCustomer.Enabled = False

            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)         'Self
            cmbStore.DataSource = mStoreList

            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            If cmbStore.Enabled = True Then
                setFocus(cmbStore)
            End If
        End If
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        If Not cmbAssemblyType.SelectedIndex <= 0 Then
            mModelList = ModelList.GetModelList(CInt(mAssemblyTypeList(cmbAssemblyType.SelectedIndex).ID), "", , , "(All)")
            cmbModelType.DataSource = mModelList
            cmbModelType.DataBind()
            Session("mModelList") = mModelList
        ElseIf cmbAssemblyType.SelectedIndex <= 0 Then
            mModelList = ModelList.GetModelList(0, "", , , "(All)")
            cmbModelType.DataSource = mModelList
            cmbModelType.DataBind()
            Session("mModelList") = mModelList
        End If
        If cmbAssemblyType.Enabled = True Then
            setFocus(cmbAssemblyType)
        End If
    End Sub
#End Region

End Class