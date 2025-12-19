Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Imports System.Web.Mail
Imports Flypal.SendMailFile
Public Class wfrptStoreBalanceRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItemList As ItemList
    Public mStore As Store
    Public mStoreList As StoreList
    Public mCustomerList As VendorList
    Public PartNo As String = ""
    Public Description As String = ""
    Public ModelName, AssemblyType, Location As String
    Public AssemblyTypeID As Integer = 0
    Public strCustomer As String
    Public flag As Int16
    Public mAssemblyTypeList As AssemblyTypeList
    Public mModelList As ModelList
    Public mStoreID As Guid         'As on 09-05-2008 By Kalpesh Shah
    Public mCustomerID As Guid      'As on 09-05-2008 By Kalpesh Shah
    Public ToDate As String
    Public mCategoryLists As CategoryList  'Added By Prashant 21-Jan-2010
    Public mCategory As Category
    Public mCategoryID As Guid
    Public StrCategory As String
    Public LookInType As Integer = 2
    Public CustomerID As String = "{00000000-0000-0000-0000-000000000000}"
    Public AssemblyTypID As Integer = 0
    Dim NameOfStore As String = ""  'Added by Prashant 5-Apr-2013 'ALL05042013
    Public strStore As String = ""
    Public mPartStatusList As PartStatusList 'Added by Vikrant On 03-May-2013 For ALL03052013-3
    Dim value As String
    Dim ReportName As String
    Dim mStoreBlanceSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
    Dim mText As String = ""
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Public mItemTagList As ItemTagList
#End Region

#Region " Helper Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
                    Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                    Select c.Name).Take(count).ToList
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCustomerList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim type As String = contextKey.Split("=")(1)
        Dim mVendorListAutoComplete As VendorListAutoComplete = VendorListAutoComplete.GetVendorListAutoComplete(prefixText, type)
        If count = 0 Then
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).ToArray
        Else
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).Take(count).ToArray
        End If
    End Function
    Private Sub GetSession()
        mCustomerList = CType(Session("mCustomerList"), VendorList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelList = CType(Session("mModelList"), ModelList)
        Location = Session("Location")
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList)
        LookInType = Session("LookInType")
        CustomerID = Session("CutomerID")
        AssemblyTypID = Session("AssemblyTypID")
        mPartStatusList = Session("mPartStatusList") 'Added by Vikrant On 03-May-2013 For ALL03052013-3
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mCustomerList") = mCustomerList
        Session("mStoreList") = mStoreList
        Session("mItemList") = mItemList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mModelList") = mModelList
        Session("mCategoryLists") = mCategoryLists
        Session("LookInType") = LookInType
        Session("CutomerID") = CustomerID
        Session("AssemblyTypID") = AssemblyTypID
        Session("mPartStatusList") = mPartStatusList 'Added by Vikrant On 03-May-2013 For ALL03052013-3
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemList")
        Session.Remove("mCustomerList")
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelList")
        Session.Remove("Location")
        Session.Remove("LookInType")
        Session.Remove("CutomerID")
        Session.Remove("AssemblyTypID")
        Session.Remove("mPartStatusList") 'Added by Vikrant On 03-May-2013 For ALL03052013-3
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility2()
        lblDateRange.Visible = True
        lblStoreName.Visible = True
        lblCustomerName.Visible = IIf(txtCustomerList.Enabled = True, True, False)
        lblCategoryName.Visible = True
        lblModel1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblCritPartStatus.Visible = True 'Added by Vikrant On 03-May-2013 For ALL03052013-3
    End Sub
    Private Sub ControlVisibility3()
        lblDateRange.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblStoreName.Visible = False
        lblCustomerName.Visible = False
        lblCategoryName.Visible = False
        lblModel1.Visible = False
        lblCritPartStatus.Visible = False 'Added by Vikrant On 03-May-2013 For ALL03052013-3
    End Sub
    Private Sub SetCustomerID()
        If txtCustomerList.Text.Trim <> "" Then
            If hdnCustomerID.Value <> String.Empty Then
                mCustomerID = New Guid(hdnCustomerID.Value.ToString)
            End If
        End If
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
        Location = txtLocation.Text
        If txtCustomerList.Text.Trim = "" Then
            lblCustomerName.Text = "Customer : All"
        Else
            strCustomer = mCustomerList(txtCustomerList.Text.Trim).Name
            lblCustomerName.Text = "Customer :" & strCustomer
        End If
        ModelName = txtModelList.Text.Trim
        lblModel1.Text = "Model : " & IIf(txtModelList.Text.Trim <> "", ModelName, "All")
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
        lblCritPartStatus.Text = "Part Status : " & IIf(cmbPartStatusList.SelectedIndex > 0, cmbPartStatusList.SelectedItem.Text, "All") 'Added by Vikrant On 03-May-2013 For ALL03052013-3
        Session("mCategory") = mCategory
        Session("mCategoryID") = mCategoryID

        If rdoLandScape.Checked = True Or rdoLandScapeDetail.Checked = True Then
            If rdoBase.Checked = True Then
                value = "Base Value"
                If chkConsiderGROExpenseValues.Checked = True Then
                    ReportName = "GRO Expense Store Balance Report (Base Value)"
                Else
                    ReportName = "Store Balance Report (Base Value)"
                End If
            ElseIf rdoLanding.Checked = True Then
                value = "Landing Value"
                If chkConsiderGROExpenseValues.Checked = True Then
                    ReportName = IIf(chkWithGST.Visible, IIf(chkWithGST.Checked, "GRO Expense Store Balance Report (Landing Value)", "GRO Expense Store Balance Report (Landing Value excluding GST)"), "GRO Expense Store Balance Report (Landing Value)")
                Else
                    ReportName = IIf(chkWithGST.Visible, IIf(chkWithGST.Checked, "Store Balance Report (Landing Value)", "Store Balance Report (Landing Value excluding GST)"), "Store Balance Report (Landing Value)")
                End If
            Else
                value = "Commercial Value"
                ReportName = "Store Balance Report (Commercial Value)"
            End If
        Else
            value = "Landing Value"
            ReportName = "Store Balance Report"
        End If
        If chkHighValue.Checked And txtCEffectiveRate.Text <> "" Then 'Added By Prashant 14-Aug-2014 For ALL14082014
            mText = "Report shows valued parts with " + value + " greater than  " + txtCEffectiveRate.Text
        Else
            mText = ""
        End If
        mStoreBlanceSearchingCriteria = lblDateRange.Text + ", " + lblCustomerName.Text + ", " + lblStoreName.Text + ", " + Location + ", Supplier : " + txtSupplierList.Text.Trim + ", " + lblCategoryName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblAssembly1.Text + ", " + lblModel1.Text + ", " + lblCritPartStatus.Text + ", " + IIf(chkIsValued.Checked = True, "Valued", "Not Valued") + ", " + value + ", Format " + IIf(rdoLandScapeDetail.Checked, "Land Scape Detail", IIf(rdoLandScape.Checked, "LandScape", "Portrait")) + ", Sort By " + cmbSortBy.SelectedItem.Text + ", Applicability " + IIf(chkNoApplicability.Checked = True, "Records With No Applicability", "") + IIf(chkIsOTP.Checked, ", One Time Purchase Item Only", "")
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False, Optional ByVal RotableStoreReport As Boolean = False)
        Try
            'Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt As rptStoreBalance
            SetCustomerID()
            Dim mModelID As Guid = Guid.Empty
            If txtModelList.Text.Trim <> "" Then
                mModelID = mModelList.Item(txtModelList.Text.Trim).ID
            End If
            Dim mSupplierID As Guid = mCustomerList.Item(txtSupplierList.Text.Trim).ID  'Added by VIkrant on 14 May 2012 For ALL11052012-13
            SetValues()
            mCategoryID = Session("mCategoryID")

            Dim ds As New dsStoreBalance
            If chkCategorywise.Checked And chkCategorywise.Enabled = True Then 'Added By Vikrant on 18-July-2012 For All18072012
                'New CaategoryWise Report For All Clients
                myReport = New crptStoreBalanceCategorywiseLandscapeDetail
                rpt = rptStoreBalance.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, False, 0, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, chkReceiptNo.Checked, chkRelNoteNo.Checked, chkSupplier.Checked, chkSupplierInvNo.Checked, chkSupplierInvDate.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkBatchNo.Checked, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), chkNoApplicability.Checked, chkOrderInfo.Checked, EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), RotableStoreReport:=RotableStoreReport, ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
            Else 'Old Condition 
                If cmbSortBy.SelectedValue = 1 Then
                    If rdoLandScapeDetail.Checked = True Then
                        myReport = New crptStoreBalanceByDescriptionLandScapeDetailNew  'Landscape Detail
                    Else
                        myReport = New crptStoreBalanceByDescriptionNew
                    End If
                    rpt = rptStoreBalance.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, False, 1, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, chkReceiptNo.Checked, chkRelNoteNo.Checked, chkSupplier.Checked, chkSupplierInvNo.Checked, chkSupplierInvDate.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkBatchNo.Checked, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), chkNoApplicability.Checked, chkOrderInfo.Checked, ConsiderGROExpenseValues:=chkConsiderGROExpenseValues.Checked, EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), RotableStoreReport:=RotableStoreReport, ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
                ElseIf (cmbSortBy.SelectedValue = 2) Then
                    If rdoLandScapeDetail.Checked = True Then
                        myReport = New crptStoreBalanceByFolioLandScapeDetailNew        'Landscape Detail
                    Else
                        myReport = New crptStoreBalanceByFolioNew
                    End If
                    rpt = rptStoreBalance.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, True, 2, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, chkReceiptNo.Checked, chkRelNoteNo.Checked, chkSupplier.Checked, chkSupplierInvNo.Checked, chkSupplierInvDate.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkBatchNo.Checked, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), chkNoApplicability.Checked, chkOrderInfo.Checked, ConsiderGROExpenseValues:=chkConsiderGROExpenseValues.Checked, EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), RotableStoreReport:=RotableStoreReport, ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
                ElseIf (cmbSortBy.SelectedValue = 3) Then
                    If rdoLandScapeDetail.Checked = True Then
                        myReport = New crptStoreBalanceByAmountLandScapeDetailNew         'Landscape Detail  By Amount Descending ''
                    End If
                    rpt = rptStoreBalance.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, False, 0, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, chkReceiptNo.Checked, chkRelNoteNo.Checked, chkSupplier.Checked, chkSupplierInvNo.Checked, chkSupplierInvDate.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkBatchNo.Checked, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), chkNoApplicability.Checked, chkOrderInfo.Checked, ConsiderGROExpenseValues:=chkConsiderGROExpenseValues.Checked, EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), RotableStoreReport:=RotableStoreReport, ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
                Else
                    If rdoLandScapeDetail.Checked = True Then
                        myReport = New crptStoreBalanceByPartNoLandScapeDetailNew          'Landscape Detail ''
                    Else
                        myReport = New crptStoreBalanceByPartNoNew
                    End If
                    rpt = rptStoreBalance.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, False, 0, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, chkReceiptNo.Checked, chkRelNoteNo.Checked, chkSupplier.Checked, chkSupplierInvNo.Checked, chkSupplierInvDate.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkBatchNo.Checked, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), chkNoApplicability.Checked, chkOrderInfo.Checked, ConsiderGROExpenseValues:=chkConsiderGROExpenseValues.Checked, EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), RotableStoreReport:=RotableStoreReport, ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
                End If
            End If
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "",
                                                                  ToDate, PartNo, txtSupplierList.Text.Trim, mText,
                                                                  StrCategory, IIf(cmbPartStatusList.SelectedIndex > 0, cmbPartStatusList.SelectedItem.Text, ""),
                                                                  IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""),
                                                                  "", ModelName, Description, ReportName, , , txtLocation.Text,
                                                                  value.Split(" ")(0).ToString & " Rate", AppSettings("Logo"),
                                                                  Search1:=IIf(chkNoApplicability.Checked = True, "Records With No Applicability", ""),
                                                                  Search2:=txtBottomLine.Text.Trim, Search3:=AppSettings("ClientCode"),
                                                                  Search4:=IIf(chkIsOTP.Checked, "Yes", ""),
                                                                  Search5:=cmbItemTag.SelectedItem.Text)
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 701)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "",
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                    ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("StoreBalance").SmtpHost, SmtpPort:=mModuleList.Item("StoreBalance").SmtpPort,
                    SmtpUser:=mModuleList.Item("StoreBalance").SmtpUser, SmtpPassword:=mModuleList.Item("StoreBalance").SmtpPassword)
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
                MarkLog(Util.Action.Print, "StoreBalance", mStoreBlanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRange.Text, "",
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("StoreBalance").SmtpHost, SmtpPort:=mModuleList.Item("StoreBalance").SmtpPort,
                    SmtpUser:=mModuleList.Item("StoreBalance").SmtpUser, SmtpPassword:=mModuleList.Item("StoreBalance").SmtpPassword)
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
    Private Sub SetReport1(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            'Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt As rptStoreBalanceLandScape
            SetCustomerID()
            Dim mModelID As Guid = Guid.Empty
            If txtModelList.Text.Trim <> "" Then
                mModelID = mModelList.Item(txtModelList.Text.Trim).ID
            End If
            Dim mSupplierID As Guid = mCustomerList.Item(txtSupplierList.Text.Trim).ID  'Added by VIkrant on 14 May 2012 For ALL11052012-13
            SetValues()
            mCategoryID = Session("mCategoryID")
            Dim ds As New dsStoreBalance
            If cmbSortBy.SelectedValue = 1 Then
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                    myReport = New crptStoreBalanceByDescriptionandBatchNoLandScapeNew
                Else
                    myReport = New crptStoreBalanceByDescriptionLandScapeNew
                End If
                rpt = rptStoreBalanceLandScape.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
            ElseIf (cmbSortBy.SelectedValue = 2) Then
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                    myReport = New crptStoreBalanceByFolioandBatchNoLandScapeNew
                Else
                    myReport = New crptStoreBalanceByFolioLandScapeNew
                End If
                rpt = rptStoreBalanceLandScape.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, True, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
            ElseIf (cmbSortBy.SelectedValue = 3) Then
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                    myReport = New crptStoreBalanceByLocationandBatchNoLandScapeNew
                Else
                    myReport = New crptStoreBalanceByLocationLandScapeNew
                End If
                rpt = rptStoreBalanceLandScape.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, True, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
            Else
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                    myReport = New crptStoreBalanceByPartNoandBatchNoLandScapeNew
                Else
                    myReport = New crptStoreBalanceByPartNoLandScapeNew
                End If
                rpt = rptStoreBalanceLandScape.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, False, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
            End If
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "",
                                                                  ToDate, PartNo, txtSupplierList.Text.Trim, "",
                                                                  StrCategory, IIf(cmbPartStatusList.SelectedIndex > 0, cmbPartStatusList.SelectedItem.Text, ""),
                                                                  IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""),
                                                                  "", ModelName, Description, ReportName, , ,
                                                                  txtLocation.Text, "", AppSettings("Logo"),
                                                                  Search1:="", Search2:=txtBottomLine.Text.Trim, Search3:=AppSettings("ClientCode"),
                                                                  Search4:=IIf(chkIsOTP.Checked, "Yes", ""),
                                                                  Search5:=cmbItemTag.SelectedItem.Text)
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 701)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "",
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                    ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("StoreBalance").SmtpHost, SmtpPort:=mModuleList.Item("StoreBalance").SmtpPort,
                    SmtpUser:=mModuleList.Item("StoreBalance").SmtpUser, SmtpPassword:=mModuleList.Item("StoreBalance").SmtpPassword)
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
                MarkLog(Util.Action.Print, "StoreBalance", mStoreBlanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRange.Text, "",
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("StoreBalance").SmtpHost, SmtpPort:=mModuleList.Item("StoreBalance").SmtpPort,
                    SmtpUser:=mModuleList.Item("StoreBalance").SmtpUser, SmtpPassword:=mModuleList.Item("StoreBalance").SmtpPassword)
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
    Private Sub ControlVisibility() 'Added By Vikrant on 18-July-2012 For All18072012
        chkCategorywise.Enabled = IIf(rdoLandScapeDetail.Checked, True, False)
        chkConsiderGROExpenseValues.Enabled = IIf(rdoLandScapeDetail.Checked, True, False)
        If chkCategorywise.Checked And chkCategorywise.Enabled = True Then
            cmbSortBy.Enabled = False
        Else
            cmbSortBy.Enabled = True
        End If

        If AppSettings("ClientCode") = "BA" And rdoLandScapeDetail.Checked Then
            btnDisplayForRotables.Visible = True
        Else
            btnDisplayForRotables.Visible = False
        End If

    End Sub 'End
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

        LookInType = 2
        CustomerID = "{00000000-0000-0000-0000-000000000000}"
        Session("LookInType") = LookInType
        Session("CustomerID") = CustomerID

        'Model
        mModelList = ModelList.GetModelList(0, "", , , "(All)")
        Session("mModelList") = mModelList

        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists

        cmbSortBy.SelectedValue = 0
        'Added by Vikrant On 03-May-2013 For ALL03052013-3
        mPartStatusList = PartStatusList.GetPartStatusList(True, "All")
        cmbPartStatusList.DataSource = mPartStatusList
        Session("mPartStatusList") = mPartStatusList
        'End
        mItemTagList = ItemTagList.GetItemTagList(True, AddTopItem:="(All)")
        cmbItemTag.DataSource = mItemTagList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        AddAttributes()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack And Session("sender") = "" Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
            DataFieldBind()
            'Ajay 08-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "StoreBalance") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
        End If
        ControlVisibility()
        upnlButtons.Update()
        upnlModelSelection.Update()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If rdoPortrait.Checked = True Then
            SetReport(False, False, False)
        ElseIf rdoLandScape.Checked = True Then
            SetReport1(False, False)
        ElseIf rdoLandScapeDetail.Checked = True Then
            SetReport(False, False, False)
        End If
    End Sub
    Private Sub btnDisplayForRotables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplayForRotables.Click
        If rdoLandScapeDetail.Checked = True Then
            SetReport(False, False, True)
        End If
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            If rdoPortrait.Checked = True Then
                email = New Thread(Sub() SetReport(False, True))
            ElseIf rdoLandScape.Checked = True Then
                email = New Thread(Sub() SetReport1(False, True))
            ElseIf rdoLandScapeDetail.Checked = True Then
                email = New Thread(Sub() SetReport(False, True))
            End If
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
        '   Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        Session("UserEmailID") = mModuleList.Item("StoreBalance").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("StoreBalance").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As rptStoreBalance
        Dim objsearch As rptSearchingCriteria
        Dim ds As New dsStoreBalance
        SetCustomerID()
        Dim mModelID As Guid = Guid.Empty
        If txtModelList.Text.Trim <> "" Then
            mModelID = mModelList.Item(txtModelList.Text.Trim).ID
        End If
        Dim mSupplierID As Guid = mCustomerList.Item(txtSupplierList.Text.Trim).ID
        SetValues()
        rpt = rptStoreBalance.GetStoreBalance(PartNo, Description, strStore, cmbAboutBalance.SelectedValue, New Guid(cmbStore.SelectedValue.ToString), mCustomerID, False, 0, mModelID.ToString, AssemblyTypeID, chkCustomerStock.Checked, ToDate, chkIsValued.Checked, chkReceiptNo.Checked, chkRelNoteNo.Checked, chkSupplier.Checked, chkSupplierInvNo.Checked, chkSupplierInvDate.Checked, txtLocation.Text, mCategoryID.ToString, value, mSupplierID.ToString, chkBatchNo.Checked, chkShowInSTock.Checked, CInt(cmbPartStatusList.SelectedValue), chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), chkNoApplicability.Checked, chkOrderInfo.Checked, ConsiderGROExpenseValues:=chkConsiderGROExpenseValues.Checked, EffRateWithGST:=chkWithGST.Checked, IsOneTimePurchaseItemOnly:=IIf(chkIsOTP.Checked, 1, 0), ItemTagID:=CInt(cmbItemTag.SelectedValue.ToString))
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "",
                                                              ToDate, PartNo, txtSupplierList.Text.Trim, mText,
                                                              StrCategory, IIf(cmbPartStatusList.SelectedIndex > 0, cmbPartStatusList.SelectedItem.Text, ""),
                                                              IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""),
                                                              "", ModelName, Description, ReportName, , ,
                                                              txtLocation.Text, value.Split(" ")(0).ToString & " Rate", AppSettings("Logo"),
                                                              IIf(chkNoApplicability.Checked = True, "Records With No Applicability", ""),
                                                              Search2:=txtBottomLine.Text.Trim, Search3:=AppSettings("ClientCode"),
                                                              Search4:=IIf(chkIsOTP.Checked, "Yes", "No"),
                                                              Search5:=cmbItemTag.SelectedItem.Text)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, "rptSearchingCriteria", objsearch)
        da.Fill(ds, "ExcelrptStoreBalance", rpt)

        Dim columnToRemove2 As String() = {"FromDate", "CompanyName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkOrderNo", "Search2", "Search3", "Search6", "Search7", "Search8", "Search9", "Search10", "Aircraft"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
            End If
        Next

        Dim columnToRemove As String()
        If AppSettings("ClientCode") = "BA" Then
            columnToRemove = {"ItemID", "SupplierName", "ReceiptType", "TransTypeID", "Folio", "IsSortByFolio", "OnOrder", "GroupBy", "Heading",
                              "SortedBy", "CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "InvQty", "IsReceiptNo", "IsReleaseNoteNo", "IsSupplierName",
                              "IsSupplierInvNo", "IsSupplierInvDate", "TotalAmountPartWise", "CategoryID", "IsBatchNo", "CurrencyID", "Rate",
                              "ReferencedDocuments", "CurrencyName", "StoreID", "CustomerID", "SupplierID", "IsValued", "PartStatusID", "LandingRateForAPI",
                              "BaseRateForAPI", "CommercialRateForAPI", "CRateForAPI", "CEFFRateForAPI", "CCommercialRateForAPI", "StockStatus",
                              "IsOwnedByCustomer", "IsOrderInfo", "DisplayUnitID", "UnitID", "Factor", "IsExpiryNA", "IsExpiryUnlimited"}
        Else
            columnToRemove = {"ItemID", "SupplierName", "ReceiptType", "TransTypeID", "Folio", "IsSortByFolio", "OnOrder", "GroupBy", "Heading",
                              "SortedBy", "CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "InvQty", "IsReceiptNo", "IsReleaseNoteNo", "IsSupplierName",
                              "IsSupplierInvNo", "IsSupplierInvDate", "TotalAmountPartWise", "CategoryID", "IsBatchNo", "CurrencyID", "Rate",
                              "ReferencedDocuments", "CurrencyName", "StoreID", "CustomerID", "SupplierID", "IsValued", "PartStatusID", "LandingRateForAPI",
                              "BaseRateForAPI", "CommercialRateForAPI", "CRateForAPI", "CEFFRateForAPI", "CCommercialRateForAPI", "StockStatus",
                              "IsOwnedByCustomer", "IsOrderInfo", "EssentialCatagory", "DisplayUnitID", "UnitID", "Factor", "IsExpiryNA", "IsExpiryUnlimited"}
        End If
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("ExcelrptStoreBalance").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("ExcelrptStoreBalance").Columns.Remove(columnToRemove(i))
            End If
        Next

        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("PartName") Then
            ds.Tables("ExcelrptStoreBalance").Columns("PartName").ColumnName = "Part Number"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("PartDescription") Then
            ds.Tables("ExcelrptStoreBalance").Columns("PartDescription").ColumnName = "Description"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("ReceiptNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("ReceiptNo").ColumnName = "Receipt No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("ReceiptDate") Then
            ds.Tables("ExcelrptStoreBalance").Columns("ReceiptDate").ColumnName = "Receipt Date"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("SerialNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("SerialNo").ColumnName = "Serial No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("BatchNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("BatchNo").ColumnName = "Batch No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("StoreName") Then
            ds.Tables("ExcelrptStoreBalance").Columns("StoreName").ColumnName = "Store"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("BalQty") Then
            ds.Tables("ExcelrptStoreBalance").Columns("BalQty").ColumnName = "Bal.Qty."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("CRate") Then
            ds.Tables("ExcelrptStoreBalance").Columns("CRate").ColumnName = "Rate"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("InvCurrencySymbol") Then
            ds.Tables("ExcelrptStoreBalance").Columns("InvCurrencySymbol").ColumnName = "Inv. Curr."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("LandingRate") Then
            ds.Tables("ExcelrptStoreBalance").Columns("LandingRate").ColumnName = "Landing Rate"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("BaseCurrencySymbol") Then
            ds.Tables("ExcelrptStoreBalance").Columns("BaseCurrencySymbol").ColumnName = "Base Curr."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("InvoiceBalanceQty") Then
            ds.Tables("ExcelrptStoreBalance").Columns("InvoiceBalanceQty").ColumnName = "Inv. Bal. Qty."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("StartDate") Then
            ds.Tables("ExcelrptStoreBalance").Columns("StartDate").ColumnName = "Start Date"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("BatchNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("BatchNo").ColumnName = "Batch No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("CureQtrYear") Then
            ds.Tables("ExcelrptStoreBalance").Columns("CureQtrYear").ColumnName = "Cure. Qtr. Year"
        End If

        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("ExpQtrYear") Then
            ds.Tables("ExcelrptStoreBalance").Columns("ExpQtrYear").ColumnName = "Exp. Qtr. Year"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("ExpiryNAOrUnlimited") Then
            ds.Tables("ExcelrptStoreBalance").Columns("ExpiryNAOrUnlimited").ColumnName = "Expiry NA/Unlimited"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("SourceName") Then
            ds.Tables("ExcelrptStoreBalance").Columns("SourceName").ColumnName = "Source"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("SupplierInvNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("SupplierInvNo").ColumnName = "Supplier Inv. No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("SupplierInvDate") Then
            ds.Tables("ExcelrptStoreBalance").Columns("SupplierInvDate").ColumnName = "Supplier Inv. Date"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("IPCReference") Then
            ds.Tables("ExcelrptStoreBalance").Columns("IPCReference").ColumnName = "IPC Reference"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("BinCardNumber") Then
            ds.Tables("ExcelrptStoreBalance").Columns("BinCardNumber").ColumnName = "Bin Card Number"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("CodeNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("CodeNo").ColumnName = "Code No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("ReleaseNoteNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("ReleaseNoteNo").ColumnName = "Release Note No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("ReleaseNoteDate") Then
            ds.Tables("ExcelrptStoreBalance").Columns("ReleaseNoteDate").ColumnName = "Release Note Date"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("ItemType") Then
            ds.Tables("ExcelrptStoreBalance").Columns("ItemType").ColumnName = "Part Type"
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("OrderNo") Then
            ds.Tables("ExcelrptStoreBalance").Columns("OrderNo").ColumnName = "Order No."
        End If
        If ds.Tables("ExcelrptStoreBalance").Columns.Contains("OrderDate") Then
            ds.Tables("ExcelrptStoreBalance").Columns("OrderDate").ColumnName = "Order Date"
        End If


        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(ds.Tables("rptSearchingCriteria"))
        dsNew.Tables("rptSearchingCriteria").Columns("KitName").ColumnName = "Model"
        dsNew.Tables("rptSearchingCriteria").Columns("ToDate").ColumnName = "As On Date"
        dsNew.Tables("rptSearchingCriteria").Columns("WorkShop").ColumnName = "Bin Location"
        dsNew.Tables("rptSearchingCriteria").Columns("Nomenclature").ColumnName = "Part Status"
        dsNew.Tables("rptSearchingCriteria").Columns("WorkOrderText").ColumnName = "Value"
        dsNew.Tables("rptSearchingCriteria").Columns("Search1").ColumnName = "Applicability"
        dsNew.Tables("rptSearchingCriteria").Columns("BranchName").ColumnName = "valued parts value greater than entered value "
        dsNew.Tables("rptSearchingCriteria").Columns("Search4").ColumnName = "One Time Purchase Part(s) Only"
        dsNew.Tables("rptSearchingCriteria").Columns("Search5").ColumnName = "Item Tag"
        dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
        dsNew.Merge(ds.Tables("ExcelrptStoreBalance"))
        dsNew.Tables("ExcelrptStoreBalance").TableName = ReportName
		Session("ExcelFileName") = ReportName
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "StoreBalance", "Export To Excel " + mStoreBlanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then
            txtCustomerList.Enabled = True
            If txtCustomerList.Text.Trim <> "" Then
                SetCustomerID()
                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            Else
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)          'All
                cmbStore.DataSource = mStoreList
            End If
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
        Else
            LookInType = 2
            CustomerID = "{00000000-0000-0000-0000-000000000000}"
            txtCustomerList.Text = ""
            txtCustomerList.Enabled = False 'VVVVVVVVVV
            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)             'Self
            cmbStore.DataSource = mStoreList
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
        End If
        upnlCustomerSelection.Update()
    End Sub
    Private Sub rdoPortrait_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoPortrait.CheckedChanged
        cmbSortBy.Items.Remove(New System.Web.UI.WebControls.ListItem("Amount (Desc)", "3"))
        cmbSortBy.Items.Remove(New System.Web.UI.WebControls.ListItem("Location", "3"))
        chkReceiptNo.Visible = False
        chkRelNoteNo.Visible = False
        chkSupplier.Visible = False
        chkSupplierInvNo.Visible = False
        chkSupplierInvDate.Visible = False
        chkOrderInfo.Visible = False
        chkBatchNo.Visible = False 'Added By Vikrant on 18-July-2012 For All18072012
        chkHighValue.Enabled = False  'Added By Prashant 13-Oct-2014 For ALL13102014
        txtCEffectiveRate.Enabled = False  'Added By Prashant 13-Oct-2014 For ALL13102014
        chkHighValue.Checked = False  'Added By Prashant 13-Oct-2014 For ALL13102014
        txtCEffectiveRate.Text = ""  'Added By Prashant 13-Oct-2014 For ALL13102014
        chkNoApplicability.Visible = False
        chkNoApplicability.Checked = False
        chkConsiderGROExpenseValues.Enabled = False
        chkConsiderGROExpenseValues.Checked = False
        If rdoPortrait.Checked = True Then
            rdoBase.Enabled = False
            rdoLanding.Enabled = False
            rdoCommercial.Enabled = False
        End If
        txtModelList.Enabled = True
        upnlModelSelection.Update()
        chkCategorywise.Checked = False 'Added By Vikrant on 18-July-2012 For All18072012
    End Sub
    Private Sub rdoLandScape_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoLandScape.CheckedChanged
        cmbSortBy.Items.Remove(New System.Web.UI.WebControls.ListItem("Amount (Desc)", "3"))
        cmbSortBy.Items.Add(New System.Web.UI.WebControls.ListItem("Location", "3"))
        chkReceiptNo.Visible = False
        chkRelNoteNo.Visible = False
        chkSupplier.Visible = False
        chkSupplierInvNo.Visible = False
        chkSupplierInvDate.Visible = False
        chkOrderInfo.Visible = False
        chkBatchNo.Visible = False 'Added By Vikrant on 18-July-2012 For All18072012
        chkHighValue.Enabled = False  'Added By Prashant 13-Oct-2014 For ALL13102014
        txtCEffectiveRate.Enabled = False  'Added By Prashant 13-Oct-2014 For ALL13102014
        chkHighValue.Checked = False  'Added By Prashant 13-Oct-2014 For ALL13102014
        txtCEffectiveRate.Text = ""  'Added By Prashant 13-Oct-2014 For ALL13102014
        chkNoApplicability.Visible = False
        chkNoApplicability.Checked = False
        chkConsiderGROExpenseValues.Enabled = False
        chkConsiderGROExpenseValues.Checked = False
        If rdoPortrait.Checked = False Then
            rdoBase.Enabled = True
            rdoLanding.Enabled = True
            rdoCommercial.Enabled = True
        End If
        txtModelList.Enabled = True
        upnlModelSelection.Update()
        chkCategorywise.Checked = False 'Added By Vikrant on 18-July-2012 For All18072012
        chkCustomerStock_CheckedChanged(sender, e)
    End Sub
    Private Sub rdoLandScapeDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoLandScapeDetail.CheckedChanged
        cmbSortBy.Items.Remove(New System.Web.UI.WebControls.ListItem("Location", "3"))
        cmbSortBy.Items.Add(New System.Web.UI.WebControls.ListItem("Amount (Desc)", "3"))
        If rdoLandScapeDetail.Checked = True Then
            chkReceiptNo.Visible = True
            chkRelNoteNo.Visible = True
            chkSupplier.Visible = True
            chkSupplierInvNo.Visible = True
            chkSupplierInvDate.Visible = True
            chkOrderInfo.Visible = True
            rdoBase.Enabled = True
            rdoLanding.Enabled = True
            rdoCommercial.Enabled = True
            chkBatchNo.Visible = True 'Added By Vikrant on 18-July-2012 For All18072012
            chkHighValue.Enabled = True  'Added By Prashant 13-Oct-2014 For ALL13102014
            chkNoApplicability.Visible = True
            chkConsiderGROExpenseValues.Enabled = True
        Else
            chkReceiptNo.Visible = False
            chkNoApplicability.Visible = False
            chkNoApplicability.Checked = False
            chkRelNoteNo.Visible = False
            chkSupplier.Visible = False
            chkSupplierInvNo.Visible = False
            chkSupplierInvDate.Visible = False
            chkOrderInfo.Visible = False
            chkBatchNo.Visible = False 'Added By Vikrant on 18-July-2012 For All18072012
            chkHighValue.Enabled = False  'Added By Prashant 13-Oct-2014 For ALL13102014
            chkConsiderGROExpenseValues.Checked = False
            chkConsiderGROExpenseValues.Enabled = False
        End If
        If chkNoApplicability.Checked = True Then
            txtModelList.Text = ""
            txtModelList.Enabled = False
        Else
            txtModelList.Enabled = True
        End If
        upnlModelSelection.Update()
    End Sub
    Private Sub txtCustomerList_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCustomerList.TextChanged
        If chkCustomerStock.Checked Then
            If txtCustomerList.Text.Trim <> "" Then                       'If Customer Selected
                SetCustomerID()
                mStoreList = StoreList.GetStoreList(mCustomerID, "(All)", True)      'Passing selected customer 
                cmbStore.DataSource = mStoreList
            Else
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)           'All
                cmbStore.DataSource = mStoreList
            End If
        End If
        cmbStore.DataBind()
        Session("mStoreList") = mStoreList
        upnlCustomerSelection.Update()
    End Sub
    Private Sub AddAttributes()
        txtCustomerList.Attributes.Add("onblur", "callEvent()")
    End Sub
    Private Sub chkCategorywise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCategorywise.CheckedChanged     'Added By Vikrant on 18-July-2012 For All18072012
    End Sub 'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub chkHighValue_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkHighValue.CheckedChanged 'Added By Prashant 14-Aug-2014 For ALL14082014
        If chkHighValue.Checked = True Then
            txtCEffectiveRate.Enabled = True
        Else
            txtCEffectiveRate.Enabled = False
            txtCEffectiveRate.Text = ""
        End If
        upnlFormatSelection.Update()
    End Sub
    Private Sub chkNoApplicability_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNoApplicability.CheckedChanged
        If chkNoApplicability.Checked = True Then
            txtModelList.Text = ""
            txtModelList.Enabled = False
        Else
            txtModelList.Enabled = True
        End If
    End Sub
    Private Sub chkConsiderGROExpenseValues_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkConsiderGROExpenseValues.CheckedChanged
        If chkConsiderGROExpenseValues.Checked = True Then
            rdoCommercial.Enabled = False
            rdoCommercial.Checked = False
            rdoLandScape.Checked = True
        Else
            rdoCommercial.Enabled = True
        End If
    End Sub
    'Ajay 08-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 07-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "StoreBalance")
    End Sub
    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 07-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "StoreBalance")
    End Sub
    '-----
#End Region

End Class