Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Public Class wfrptPartConsumptionList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public FromDate As String
    Public ToDate As String
    Public mMachineList As MachineList
    Public mWorkShopList As WorkShopList
    Public mCategoryList As CategoryList
    Public strCategory, Aircraft, WorkShop As String
    'Added By Vikrant On 28-Jun-2013 For ALL28062013-1
    Public mAssemblyTypeList As AssemblyTypeList
    Public mModelList As ModelList
    Public AssemblyTypeID As Integer = 0
    Public ModelID As Guid
    'End
    Dim EventLogID As Guid 'Added by Prashant
    Dim mPartConsumptionSearchingCriteria As String = String.Empty
    Dim Value As String = ""
    Dim ReportName As String = ""
    Dim mText As String = ""
    Dim email As Thread

    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim mVendorList As VendorList
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim mStoreList As StoreList
#End Region

#Region " Helper Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, AssemblyTypID)
        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
               Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                   Select c.Name).Take(count).ToList
        End If
    End Function
    Private Sub GetSession()
        mMachineList = Session("mMachineListForPartConsumptionList")
        mCategoryList = Session("mCategoryList")
        mWorkShopList = Session("mWorkShopList")
        'Added By Vikrant On 28-Jun-2013 For ALL28062013-1
        mAssemblyTypeList = CType(Session("mAssemblyTypeList"), AssemblyTypeList)
        mModelList = CType(Session("mModelList"), ModelList)
        'End

        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mMachineListForPartConsumptionList") = mMachineList
        Session("mWorkShopList") = mWorkShopList
        Session("mCategoryList") = mCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineListForPartConsumptionList")
        Session.Remove("mWorkShopList")
        Session.Remove("mCategoryList")
        'Added By Vikrant On 28-Jun-2013 For ALL28062013-1
        Session.Remove("mAssemblyTypeList")
        Session.Remove("mModelList")
        'End
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        If Index = 6 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblAircraftCrit.Visible = IIf(cmbDocType.SelectedIndex = 1, True, False)
        lblWorkShopCrit.Visible = IIf(cmbDocType.SelectedIndex = 2, True, False)
        lblCategory1.Visible = True
        'Added By Vikrant On 28-Jun-2013 For ALL28062013-1
        lblAssembly1.Visible = True
        lblModel1.Visible = True
        'End

        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblAircraftCrit.Visible = False
        lblWorkShopCrit.Visible = False
        lblCategory1.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then      ''Date Range
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
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

        If cmbDocType.SelectedIndex = 1 Then
            Aircraft = IIf(cmbMachine.SelectedIndex <= 0, "All", cmbMachine.SelectedItem.ToString)
        Else
            Aircraft = "None"
        End If
        lblAircraftCrit.Text = "Aircraft : " & IIf(cmbMachine.SelectedIndex <= 0, "All", cmbMachine.SelectedItem.ToString)

        If cmbDocType.SelectedIndex = 2 Then
            WorkShop = IIf(cmbWorkShop.SelectedIndex <= 0, "All", cmbWorkShop.SelectedItem.ToString)
        Else
            WorkShop = "None"
        End If
        lblWorkShopCrit.Text = "WorkShop : " & IIf(cmbWorkShop.SelectedIndex <= 0, "All", cmbWorkShop.SelectedItem.ToString)
        'Added By Vikrant On 18-Dec-2012 For ALL18122012
        If rdoBase.Checked = True Then
            Value = "Base Value"
            ReportName = "Part Consumption Report (Base Value)"
        ElseIf rdoLanding.Checked = True Then
            Value = "Landing Value"
            ReportName = "Part Consumption Report (Landing Value)"
        Else
            Value = "Commercial Value"
            ReportName = "Part Consumption Report (Commercial Value)"
        End If
        'End
        'Added By Vikrant On 28-Jun-2013 For ALL28062013-1
        If cmbAssemblyType.SelectedIndex = 0 Then
            lblAssembly1.Text = "Assembly : All"
            AssemblyTypeID = 0
        Else
            lblAssembly1.Text = "Assembly : " & cmbAssemblyType.SelectedItem.ToString
            AssemblyTypeID = mAssemblyTypeList.Item(cmbAssemblyType.SelectedIndex).ID
        End If
        If txtModelList.Text.Trim <> "" Then
            ModelID = mModelList.Item(txtModelList.Text.Trim).ID
        Else
            ModelID = Guid.Empty
        End If
        lblModel1.Text = "Model : " & IIf(txtModelList.Text.Trim <> "", txtModelList.Text.Trim, "All")
        'End

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        PartNo = IIf(Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Not IsNothing(Description), Description, "")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")


        mPartConsumptionSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblCategory1.Text.Trim + ", " + lblAircraftCrit.Text + ", " + lblWorkShopCrit.Text.Trim + ", " + lblAssembly1.Text.Trim + ", " + lblModel1.Text.Trim
        If chkHighValue.Checked And txtCEffectiveRate.Text <> "" Then 'Added By Prashant 14-Aug-2014 For ALL14082014
            mText = "Report shows valued parts with " + Value + " greater than  " + txtCEffectiveRate.Text
        Else
            mText = ""
        End If
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objsearch As rptSearchingCriteria
            Dim rpt As PartConsumptionList

            SetValues()
            Dim ds As New dsIssue
            'Commented & Added By Vikrant On 12-Feb-2013 For Heligo12022013-1
            'myReport = New crptPartConsumption
            If chkPartwise.Checked Then
                myReport = New crptPartwiseConsumption
            Else
                If cmbFormat.SelectedIndex = 0 Then
                    myReport = New crptPartConsumption
                ElseIf cmbFormat.SelectedIndex = 2 Then 'Added By Vikrant On 12-Jun-2013 For ALL11062013
                    myReport = New crptPartConsumptionFormat3
                ElseIf cmbFormat.SelectedIndex = 3 Then ''Added By Prashant On 18-Jun-2014 For Deccan18062014
                    myReport = New crptPartConsumptionFormat4
                Else
                    myReport = New crptPartConsumptionFormat2
                End If
                'End
            End If

            If cmbDocType.SelectedIndex = 1 Then
                rpt = PartConsumptionList.GetPartConsumptionList(FromDate, ToDate, strCategory, cmbMachine.SelectedValue.ToString, , Value, _
                                                                 CInt(cmbFormat.SelectedValue), CInt(cmbDocType.SelectedValue), ModelID.ToString, _
                                                                 AssemblyTypeID, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), PartNo, _
                                                                 Description, SupplierID:=cmbSupplier.SelectedValue.ToString, IsValued:=Val(cmbStoreType.SelectedValue), _
                                                                 ClientCode:=AppSettings("ClientCode"), StoreID:=cmbStore.SelectedValue.ToString)
            ElseIf cmbDocType.SelectedIndex = 2 Then
                rpt = PartConsumptionList.GetPartConsumptionList(FromDate, ToDate, strCategory, , cmbWorkShop.SelectedValue.ToString, Value, _
                                                                 CInt(cmbFormat.SelectedValue), CInt(cmbDocType.SelectedValue), ModelID.ToString, _
                                                                 AssemblyTypeID, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), PartNo, _
                                                                 Description, SupplierID:=cmbSupplier.SelectedValue.ToString, IsValued:=Val(cmbStoreType.SelectedValue), _
                                                                 ClientCode:=AppSettings("ClientCode"), StoreID:=cmbStore.SelectedValue.ToString)
            Else
                rpt = PartConsumptionList.GetPartConsumptionList(FromDate, ToDate, strCategory, , , Value, CInt(cmbFormat.SelectedValue), _
                                                                 CInt(cmbDocType.SelectedValue), ModelID.ToString, AssemblyTypeID, chkHighValue.Checked, _
                                                                 CDec(Val(txtCEffectiveRate.Text)), PartNo, Description, SupplierID:=cmbSupplier.SelectedValue.ToString, _
                                                                  IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), StoreID:=cmbStore.SelectedValue.ToString)
            End If

            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, Description, _
                                                                  IIf(FromDate = "1-1-1900", "", FromDate), strCategory, IIf(ToDate = "1-1-2200", "", ToDate), _
                                                                  IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.Text), Aircraft, _
                                                                  IIf(cmbAssemblyType.SelectedIndex > 0, cmbAssemblyType.SelectedItem.Text, ""), _
                                                                  IIf(txtModelList.Text.Trim <> "", txtModelList.Text.Trim, ""), ReportName, 0, FromStore:=cmbStore.SelectedItem.Text, _
                                                                 WorkShop:=WorkShop, WorkOrderText:=mText, WorkOrderNo:=AppSettings("Logo"))
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1250)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                          SmtpHost:=mModuleList.Item("PartConsumptionList").SmtpHost, SmtpPort:=mModuleList.Item("PartConsumptionList").SmtpPort, _
                                          SmtpUser:=mModuleList.Item("PartConsumptionList").SmtpUser, SmtpPassword:=mModuleList.Item("PartConsumptionList").SmtpPassword)
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
            MarkLog(Util.Action.Print, "PartConsumptionList", mPartConsumptionSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRangeFrom.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                          SmtpHost:=mModuleList.Item("PartConsumptionList").SmtpHost, SmtpPort:=mModuleList.Item("PartConsumptionList").SmtpPort, _
                                          SmtpUser:=mModuleList.Item("PartConsumptionList").SmtpUser, SmtpPassword:=mModuleList.Item("PartConsumptionList").SmtpPassword)
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
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    'Added By Vikrant On 12-Feb-2013 For Heligo12022013-1
    Private Sub ControlVisibilityForIssue()
        If cmbDocType.SelectedIndex <= 0 Then
            lblCostCenter.Visible = False
            cmbMachine.Visible = False
            cmbWorkShop.Visible = False
        Else
            lblCostCenter.Visible = True
            If cmbDocType.SelectedIndex = 1 Then
                cmbWorkShop.Visible = False
                lblCostCenter.Text = "Aircraft"
                cmbMachine.Visible = True
            ElseIf cmbDocType.SelectedIndex = 2 Then
                cmbMachine.Visible = False
                lblCostCenter.Text = "WorkShop"
                cmbWorkShop.Visible = True
            End If
        End If

        If cmbFormat.SelectedIndex = 1 And cmbDocType.SelectedIndex = 0 Then
            lblNote.Visible = True
            lblNote.Text = "Note:Issue to Discard Transactions are included in this format."
        ElseIf cmbFormat.SelectedIndex = 2 Then
            lblNote.Visible = True
            lblNote.Text = "Note:Shows " & IIf(cmbDocType.SelectedIndex > 0, cmbDocType.SelectedItem.Text, "Aircraft/WorkOrder/WorkShop") & " wise Records."
        ElseIf cmbFormat.SelectedIndex = 3 Then
            lblNote.Visible = True
            lblNote.Text = "Note:Issue to Aircraft, WorkShop, Customer Transactions are included in this format."
        Else
            lblNote.Visible = False

        End If
        If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            lblValuedStores.Visible = True
            cmbStoreType.Visible = True
        Else
            lblValuedStores.Visible = False
            cmbStoreType.Visible = False
        End If
        upnlSelectionOfFormat.Update()
    End Sub
    'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList()
        ChklistCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList

        mMachineList = MachineList.GetMachineList(, , , , , , , , , , True, "(SELECT)")
        Session("mMachineListForPartConsumptionList") = mMachineList
        cmbMachine.DataSource = mMachineList

        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
        Session("mWorkShopList") = mWorkShopList
        cmbWorkShop.DataSource = mWorkShopList

        'Added By Vikrant On 28-Jun-2013 For ALL28062013-1
        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList("(All)")
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList

        mModelList = ModelList.GetModelList(0, "", , , "(All)")
        Session("mModelList") = mModelList
        'End

        mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", , IsSupplier:=True)
        cmbSupplier.DataSource = mVendorList

        mStoreList = StoreList.GetStoreList(LookInType:=0, StoreName:="", SelectTag:="(All)")
        cmbStore.DataSource = mStoreList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            'Ajay 09-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "PartConsumptionList") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
        ControlVisibilityForIssue()
        upnlModelSelection.Update()
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
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False, False)
        Else
            upnlValidationsummary.Update()
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
            '  Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
            Session("UserEmailID") = mModuleList.Item("PartConsumptionList").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("PartConsumptionList").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Added By Vikrant On 12-Feb-2013 For Heligo12022013-1
    Private Sub cmbDocType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDocType.SelectedIndexChanged
        ControlVisibilityForIssue()
    End Sub
    'End
    'Added By Vikrant On 04-Mar-2013 For All04032013
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim objsearch As rptSearchingCriteria
            Dim rpt As PartConsumptionList
            Dim ds As New dsExcelIssue
            SetValues()

            If cmbDocType.SelectedIndex = 1 Then
                rpt = PartConsumptionList.GetPartConsumptionList(FromDate, ToDate, strCategory, cmbMachine.SelectedValue.ToString, , Value, CInt(cmbFormat.SelectedValue), _
                                                                 CInt(cmbDocType.SelectedValue), ModelID.ToString, AssemblyTypeID, chkHighValue.Checked, _
                                                                 CDec(Val(txtCEffectiveRate.Text)), PartNo, Description, SupplierID:=cmbSupplier.SelectedValue.ToString, _
                                                                 IsValued:=Val(cmbStoreType.SelectedValue),  ClientCode:=AppSettings("ClientCode"), StoreID:=cmbStore.SelectedValue.ToString)
            ElseIf cmbDocType.SelectedIndex = 2 Then
                rpt = PartConsumptionList.GetPartConsumptionList(FromDate, ToDate, strCategory, , cmbWorkShop.SelectedValue.ToString, Value, CInt(cmbFormat.SelectedValue), _
                                                                 CInt(cmbDocType.SelectedValue), ModelID.ToString, AssemblyTypeID, chkHighValue.Checked, _
                                                                 CDec(Val(txtCEffectiveRate.Text)), PartNo, Description, SupplierID:=cmbSupplier.SelectedValue.ToString, _
                                                                 IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), StoreID:=cmbStore.SelectedValue.ToString)
            Else
                rpt = PartConsumptionList.GetPartConsumptionList(FromDate, ToDate, strCategory, , , Value, CInt(cmbFormat.SelectedValue), CInt(cmbDocType.SelectedValue), _
                                                                 ModelID.ToString, AssemblyTypeID, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), PartNo, _
                                                                 Description, SupplierID:=cmbSupplier.SelectedValue.ToString, _
                                                                 IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), StoreID:=cmbStore.SelectedValue.ToString)
            End If

            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), IIf(FromDate = "1-1-1900", "", FromDate), _
                                                                  IIf(ToDate = "1-1-2200", "", ToDate), PartNo, Description, "", strCategory, "", _
                                                                  IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.Text), Aircraft, _
                                                                  IIf(cmbAssemblyType.SelectedIndex > 0, cmbAssemblyType.SelectedItem.Text, ""), _
                                                                  IIf(txtModelList.Text.Trim <> "", txtModelList.Text.Trim, ""), ReportName, 0, _
                                                                  FromStore:=cmbStore.SelectedItem.Text, WorkShop:=WorkShop, WorkOrderText:=mText, _
                                                                  WorkOrderNo:=AppSettings("Logo"))
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ds.Clear()
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            da.Fill(ds, "PartConsumptionList", rpt)
            Dim columnToRemove2 As String() = {"CompanyName", "BranchName", "Nomenclature", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "Description"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"MachineName", "WorkShopName", "WONo", "TransTypeID", "ToTypeID", "OriginalReceiptNo", "InvQty", "IssueDate", "OriginalSupplierInvoiceNo", "Factor", "DisplayUnitID", "UnitID"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("PartConsumptionList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("PartConsumptionList").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("PartConsumptionList").Columns.Contains("EffectiveRate") Then
                ds.Tables("PartConsumptionList").Columns("EffectiveRate").ColumnName = "Rate"
            End If
            If ds.Tables("PartConsumptionList").Columns.Contains("IssueDateFormatted") Then
                ds.Tables("PartConsumptionList").Columns("IssueDateFormatted").ColumnName = "Issue Date"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("SupplierName") Then
                ds.Tables("rptSearchingCriteria").Columns("SupplierName").ColumnName = "Description"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("PartNo") Then
                ds.Tables("rptSearchingCriteria").Columns("PartNo").ColumnName = "Part Name"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("store") Then
                ds.Tables("rptSearchingCriteria").Columns("store").ColumnName = "Supplier"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            ds.Tables("PartConsumptionList").TableName = "Part Consumption List"
            ds.Tables.Remove("UnusedReturnedPartsList")
			ds.Tables.Remove("ReportData")
			Session("ExcelFileName") = "Part Consumption List"
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "PartConsumptionList", "Export To excel " + mPartConsumptionSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    'End
    'Added By Vikrant On 28-Jun-2013 For ALL28062013-1
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        'txtModelList.Text = ""
        If cmbAssemblyType.SelectedIndex > 0 Then
            AssemblyTypeID = CInt(mAssemblyTypeList(cmbAssemblyType.SelectedIndex).ID)
            Session("AssemblyTypID") = AssemblyTypeID
        Else
            AssemblyTypeID = 0
            Session("AssemblyTypID") = AssemblyTypeID
        End If
        If cmbAssemblyType.Enabled = True Then
            setFocus(cmbAssemblyType)
        End If
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        If Not IsDate(txtFromDate.Text.Trim) Then
            txtFromDate.Text = ""
        End If
    End Sub
    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        If Not IsDate(txtToDate.Text.Trim) Then
            txtToDate.Text = ""
        End If
    End Sub
    Private Sub chkHighValue_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkHighValue.CheckedChanged 'Added By Prashant 14-Aug-2014 For ALL14082014
        If chkHighValue.Checked = True Then
            txtCEffectiveRate.Enabled = True
        Else
            txtCEffectiveRate.Enabled = False
            txtCEffectiveRate.Text = ""
        End If
        upnlHighValue.Update()
    End Sub
    Private Sub chkPartwise_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkPartwise.CheckedChanged
        If chkPartwise.Checked = True Then
            cmbFormat.Enabled = False
            cmbFormat.SelectedIndex = 0
        Else
            cmbFormat.Enabled = True
        End If
        ControlVisibilityForIssue()
    End Sub
    'Ajay 09-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "PartConsumptionList")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "PartConsumptionList")
    End Sub
    '-----
#End Region


End Class