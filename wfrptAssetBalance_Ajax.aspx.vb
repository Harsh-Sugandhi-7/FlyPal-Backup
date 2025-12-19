Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Imports System.Web.Mail
Imports Flypal.SendMailFile
Public Class wfrptAssetBalance_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public mCustomerList As VendorList
    Public PartNo As String = ""
    Public Description As String = ""
    Public ModelName As String
    Public mModelList As ModelList
    Public mCustomerID As Guid
    Public ToDate As String
    Public mCategoryLists As CategoryList
    Public mCategory As Category
    Public mCategoryID As Guid
    Public StrCategory As String
    Public CustomerID As String = "{00000000-0000-0000-0000-000000000000}"
    Dim ReportName As String
    Dim mStoreBlanceSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Dim mText As String = ""
    Dim email As Thread
    Public mATAList As ATAList
    Dim mModelID As Guid = Guid.Empty
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mModelList = CType(Session("mModelList"), ModelList)
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList)
        CustomerID = Session("CutomerID")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mCustomerList") = mCustomerList
        Session("mStoreList") = mStoreList
        Session("mModelList") = mModelList
        Session("mCategoryLists") = mCategoryLists
         Session("CutomerID") = CustomerID
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCustomerList")
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mModelList")
        Session.Remove("CutomerID")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility2()
        lblDateRange.Visible = True
        lblStoreName.Visible = True
        lblSuppName.Visible = True
        lblCategoryName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblATA.Visible = True
        lblModel1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRange.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblStoreName.Visible = False
        lblSuppName.Visible = False
        lblCategoryName.Visible = False
        lblATA.Visible = False
        lblModel1.Visible = False
    End Sub
    Private Sub SetCustomerID()
        mCustomerID = Guid.Empty
    End Sub
    Private Sub SetValues()
        If txtDate.Text = String.Empty Then
            ToDate = "1/1/3050"
            lblDateRange.Text = "Date Range  : All"
        Else
            ToDate = txtDate.Text
            lblDateRange.Text = "As On Date : " & New SmartDate(txtDate.Text).FormattedText
        End If
        SetCustomerID()
        lblStoreName.Text = "Store : " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")
        lblATA.Text = "ATA : " & IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "All")

        If txtSupplierList.Text.Trim = "" Then
            lblSuppName.Text = "Supplier : All"
        Else
            lblSuppName.Text = "Supplier :" & txtSupplierList.Text.Trim
        End If
        'ModelName = txtModelList.Text.Trim
        'lblModel1.Text = "Model : " & IIf(txtModelList.Text.Trim <> "", ModelName, "All")
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        If cmbCategory.SelectedIndex = 0 Then
            StrCategory = ""
            mCategoryID = Guid.Empty
            lblCategoryName.Text = "Category Name : All"
        Else
            mCategory = Category.GetCategory(New Guid(cmbCategory.SelectedValue))
            StrCategory = mCategory.Name
            mCategoryID = mCategory.ID
            lblCategoryName.Text = "Category Name : " & StrCategory
        End If
        If (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = False) Then
            ModelName = ""
            lblModel1.Text = "Model : " & "All"
        ElseIf (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = True) Then
            ModelName = "Common/No Applicability"
        Else
            ModelName = cmbModel.SelectedItem.Text
            mModelID = New Guid(cmbModel.SelectedValue)
            lblModel1.Text = "Model : " & cmbModel.SelectedItem.Text
        End If
        Session("mCategory") = mCategory
        Session("mCategoryID") = mCategoryID
        ReportName = "Asset Balance Report"
        mStoreBlanceSearchingCriteria = lblDateRange.Text + ", " + lblStoreName.Text + ", Supplier : " + txtSupplierList.Text.Trim + ", " + lblCategoryName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblATA.Text + ", " + lblModel1.Text '+ IIf(chkIsValued.Checked = True, "Valued", "Not Valued") + ", " + value + ", Format " + IIf(rdoLandScapeDetail.Checked, "Land Scape Detail", IIf(rdoLandScape.Checked, "LandScape", "Portrait")) + ", Sort By " + cmbSortBy.SelectedItem.Text + ", Applicability " + IIf(chkNoApplicability.Checked = True, "Records With No Applicability", "")
    End Sub
    Private Sub SetReport1(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt As AssetBalance
            SetCustomerID()
            Dim mSupplierID As Guid = mCustomerList.Item(txtSupplierList.Text.Trim).ID
            SetValues()
            mCategoryID = Session("mCategoryID")
            Dim ds As New dsAssetBalance
            If chkWithBalAmount.Checked = True Then
                myReport = New crptAssetBalanceWithBalAmount
            Else
                myReport = New crptAssetBalance
            End If

            rpt = AssetBalance.GetAssetBalance(PartNo:=PartNo, Description:=Description, StoreID:=New Guid(cmbStore.SelectedValue.ToString), ToDate:=ToDate, CategoryID:=mCategoryID.ToString, SupplierID:=mSupplierID.ToString, ATAChapterID:=cmbATAChapter.SelectedValue.ToString, CommonOrApplicability:=chkCommonOrApplicability.Checked, ModelID:=mModelID.ToString)

            objsearch = rptSearchingCriteria.GetSearchingCriteria(companyID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:="", ToDate:=ToDate, PartNo:=PartNo, SupplierName:=txtSupplierList.Text.Trim, BranchName:=mText, Category:=StrCategory, Nomenclature:="", store:=IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Aircraft:="", KitName:=ModelName, Description:=Description, RelNoteNo:=ReportName, TransTypeID:=0, FromStore:="", WorkShop:="", WorkOrderText:="", WorkOrderNo:=AppSettings("Logo"), Search1:=IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, ""), Search2:=ModelName, Search3:="", Search4:="", Search5:="", Search6:="", Search7:="", Search8:="", Search9:="", Search10:="")
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1333)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("AircraftConsumption").SmtpHost, SmtpPort:=mModuleList.Item("AircraftConsumption").SmtpPort, SmtpUser:=mModuleList.Item("AircraftConsumption").SmtpUser, SmtpPassword:=mModuleList.Item("AircraftConsumption").SmtpPassword)

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
            MarkLog(Util.Action.Print, "AssetBalance", mStoreBlanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRange.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"))
            End If
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (SetReport1 Sub Method): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, True, True)
        Session("mCustomerList") = mCustomerList

        'Store
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        CustomerID = "{00000000-0000-0000-0000-000000000000}"
        Session("CustomerID") = CustomerID

        'Model
        mModelList = ModelList.GetModelList(0, "", , , "(All)")
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()
        Session("mModelList") = mModelList

        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists

        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack And Session("sender") = "" Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport1(False, False)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport1(False, True))
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
        'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        Session("UserEmailID") = mModuleList.Item("AssetBalance").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AssetBalance").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As AssetBalance
        Dim objsearch As rptSearchingCriteria
        Dim ds As New dsAssetBalance
        SetCustomerID()
        Dim mSupplierID As Guid = mCustomerList.Item(txtSupplierList.Text.Trim).ID
        SetValues()
        rpt = AssetBalance.GetAssetBalance(PartNo:=PartNo, Description:=Description, StoreID:=New Guid(cmbStore.SelectedValue.ToString), ToDate:=ToDate, CategoryID:=mCategoryID.ToString, SupplierID:=mSupplierID.ToString, ATAChapterID:=cmbATAChapter.SelectedValue.ToString, CommonOrApplicability:=chkCommonOrApplicability.Checked, ModelID:=mModelID.ToString)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(companyID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:="", ToDate:=ToDate, PartNo:=PartNo, SupplierName:=txtSupplierList.Text.Trim, BranchName:=mText, Category:=StrCategory, Nomenclature:="", store:=IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Aircraft:="", KitName:=ModelName, Description:=Description, RelNoteNo:=ReportName, TransTypeID:=0, FromStore:="", WorkShop:="", WorkOrderText:="", WorkOrderNo:=AppSettings("Logo"), Search1:=IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, ""), Search2:=ModelName, Search3:="", Search4:="", Search5:="", Search6:="", Search7:="", Search8:="", Search9:="", Search10:="")

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, "rptSearchingCriteria", objsearch)

        Dim OrderByPartNo = (From c In rpt
                            Order By c.PartNo
                            Select c).ToList

        da.Fill(ds, "AssetBalance", OrderByPartNo)

        Dim columnToRemove2 As String() = {"FromDate", "CompanyName", "BranchName", "KitName", "WorkOrderText", "RelNoteNo", "WorkShop", "Nomenclature", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkOrderNo", "Search1", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "Aircraft"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
            End If
        Next

        Dim columnToRemove As String() = {"CureQtrs", "CureYear", "ExpYear", "ExpQtrs", "Folio", "IsSortByFolio", "OnOrder", "GroupBy", "Heading", "SortedBy", "CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "InvQty", "IsReceiptNo", "IsReleaseNoteNo", "IsSupplierName", "IsSupplierInvNo", "IsSupplierInvDate", "TotalAmountPartWise", "CategoryID", "IsBatchNo", "CurrencyID", "Rate", "ReferencedDocuments", "CurrencyName", "ReceiptItemQty", "IssueItemQty"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("AssetBalance").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("AssetBalance").Columns.Remove(columnToRemove(i))
            End If
        Next
        If ds.Tables("AssetBalance").Columns.Contains("EffRate") Then
            ds.Tables("AssetBalance").Columns("EffRate").ColumnName = "Rate"
        End If
        If ds.Tables("AssetBalance").Columns.Contains("CureQtrYear") Then
            ds.Tables("AssetBalance").Columns("CureQtrYear").ColumnName = "CureQtr/Year"
        End If
        If ds.Tables("AssetBalance").Columns.Contains("ExpQtrYear") Then
            ds.Tables("AssetBalance").Columns("ExpQtrYear").ColumnName = "ExpQtr/Year"
        End If
        If ds.Tables("AssetBalance").Columns.Contains("ReceiptItemCodeNo") Then
            ds.Tables("AssetBalance").Columns("ReceiptItemCodeNo").ColumnName = "Code No."
        End If
        If ds.Tables("AssetBalance").Columns.Contains("ItemMinStockLevel") Then
            ds.Tables("AssetBalance").Columns("ItemMinStockLevel").ColumnName = "Min"
        End If
        If ds.Tables("AssetBalance").Columns.Contains("ItemMaxStockLevel") Then
            ds.Tables("AssetBalance").Columns("ItemMaxStockLevel").ColumnName = "Max"
        End If
        If ds.Tables("AssetBalance").Columns.Contains("ItemMinReOrderLevel") Then
            ds.Tables("AssetBalance").Columns("ItemMinReOrderLevel").ColumnName = "Re-Order Level"
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(ds.Tables("rptSearchingCriteria"))
        dsNew.Tables("rptSearchingCriteria").Columns("ToDate").ColumnName = "As On Date"
        dsNew.Tables("rptSearchingCriteria").Columns("Search2").ColumnName = "Model"
        dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
        dsNew.Merge(ds.Tables("AssetBalance"))
        dsNew.Tables("AssetBalance").TableName = ReportName
		Session("ExcelFileName") = ReportName
		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "AssetBalance", "Export To excel " + mStoreBlanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub chkCommonOrApplicability_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkCommonOrApplicability.CheckedChanged
        If chkCommonOrApplicability.Checked = True Then
            cmbModel.Enabled = False
            cmbModel.SelectedIndex = 0
            cmbModel.DataBind()
        Else
            cmbModel.Enabled = True
        End If
    End Sub
#End Region

End Class