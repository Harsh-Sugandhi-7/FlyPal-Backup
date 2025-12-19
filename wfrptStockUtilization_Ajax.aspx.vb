Public Class wfrptStockUtilization_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Fromdate As String = ""
    Dim ToDate As String = ""
    Dim Supplier As String = ""
    Dim FromStore As String = ""
    Dim Status As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim IssNo As String = ""
    Dim IssText As String = ""
    Dim ReleaseNoteNo As String = ""
    Dim SerialNo As String = ""
    Public mVendor As Vendor
    Dim mTransTypeID As Integer
    Public Shadows Title, IssueType As String
    Dim WorkShop As String = ""
    Public strCategory As String
    Public mCategoryList As CategoryList
    Dim mStoreList As StoreList
    Dim mStockUtilizationSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mTransTypeID = CType(Session("mTransTypeID"), Int16)
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mTransTypeID")
        Session.Remove("mCategoryList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
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
        lblVendor.Visible = True
        lblOrderNo.Visible = True
        lblSerialNo.Visible = True
        lblReleaseNoteNo.Visible = True
        lblStatus.Visible = True
        lblFromStore.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblIssuetype.Visible = True
        lblCategoryName.Visible = True
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
        mTransTypeID = CType(cmbIssue.SelectedValue, Int16)
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            Fromdate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If

        If cmbType.SelectedItem.Text = "Customer" Then
            Supplier = txtCustomer.Text.Trim
        ElseIf cmbType.SelectedItem.Text = "Supplier" Then
            Supplier = txtSupplier.Text.Trim
        End If

        If cmbCategory.SelectedIndex = 0 Then       ''Category
            strCategory = ""
            lblCategoryName.Text = "Category : All"
        Else
            strCategory = Category.GetCategory(New Guid(cmbCategory.SelectedValue)).Name
            lblCategoryName.Text = "Category : " & strCategory
        End If

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        SerialNo = txtSerialNo.Text.Trim
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        PartNo = IIf(Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Not IsNothing(Description), Description, "")
        IssNo = txtNo.Text.Trim
        IssText = txtIssueTextList.Text.Trim
        ReleaseNoteNo = txtReleaseNoteNo.Text.Trim
        FromStore = IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, "")
        WorkShop = IIf(cmbType.SelectedIndex = 3, txtWorkShop.Text, "")
        lblReleaseNoteNo.Text = "Release Note No. : " & IIf(ReleaseNoteNo <> "", ReleaseNoteNo, "All")
        lblSerialNo.Text = "Serial No. :" & IIf(SerialNo <> "", SerialNo, "All")
        lblStatus.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblFromStore.Text = "From Store : " & IIf(FromStore <> "", FromStore, "All")
        IssueType = IIf(cmbIssue.SelectedIndex > 0, cmbIssue.SelectedItem.Text, "")
        lblIssuetype.Text = "Issue Type : " & IIf(IssueType <> "", IssueType, "All")

        If IssText = "" Then
            lblOrderNo.Text = "Issue No. : All "
        Else
            lblOrderNo.Text = "Issue No. : " + IssText + "-" + IssNo
        End If

        Select Case cmbType.SelectedIndex
            Case 0
                lblVendor.Text = "To Type : All"
            Case 1 'Vendor
                lblVendor.Text = IIf(mTransTypeID = 25, "Customer : " & IIf(Supplier <> "", Supplier, "All"), "Supplier : " & IIf(Supplier <> "", Supplier, "All"))
            Case 2 'Discard
                lblVendor.Text = "Discard "
            Case 3 'WorkShop
                lblVendor.Text = "WorkShop : " & IIf(WorkShop <> "", WorkShop, "All")
        End Select

        mStockUtilizationSearchingCriteria = lblDateRangeFrom.Text + ", " + lblCategoryName.Text + ", " + lblReleaseNoteNo.Text + ", " + lblSerialNo.Text + ", " + lblStatus.Text.Trim + ", " + lblCategoryName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblFromStore.Text + ", " + lblIssuetype.Text + ", " + lblOrderNo.Text + ", " + lblVendor.Text
    End Sub
    Public Sub SetReport(Optional ByVal IsExcel As Boolean = False)       'Added by Shweta on 13/12/2012 for ALL13122012
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptStockUtilization
        Dim ReportByValue As String = ""
        Dim ReportName As String = ""
        Dim RateType As String = ""
        Dim FromStoreID As String
        FromStoreID = IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedValue.ToString, Guid.Empty.ToString)
        SetValues()

        'Added By Utkarsh ON 18-Dec-2012 FOR ALL18122012
        If rdoBase.Checked = True Then
            ReportByValue = "Base Value"
            ReportName = "Stock Utilizaton (Base Value)"
            RateType = "Base Rate"
        ElseIf rdoLanding.Checked = True Then
            ReportByValue = "Landing Value"
            ReportName = "Stock Utilizaton (Landing Value)"
            RateType = "Landing Rate"
        Else
            ReportByValue = "Commercial Value"
            ReportName = "Stock Utilizaton (Commercial Value)"
            RateType = "Commercial Rate"
        End If
        'End

        myReport = New crptStockUtilizations
        rpt = rptStockUtilization.GetrptStockUtilizationList(IssText, IssNo, Fromdate, ToDate, Supplier, Val(cmbStatus.SelectedValue), ReleaseNoteNo, SerialNo, PartNo, Description, FromStoreID, cmbIssue.SelectedValue, WorkShop, cmbCategory.SelectedValue.ToString, ReportByValue)
        'Added "ReportName, ReportByValue" Criteria By Utkarsh ON 18-Dec-2012 FOR ALL18122012
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:=Fromdate, ToDate:=ToDate, PartNo:=PartNo, SupplierName:=Supplier, BranchName:=IIf(IssText <> "", IssText + "-" + CStr(IssNo), ""), Category:=strCategory, Nomenclature:=" ", store:=ReportByValue, Aircraft:=Status, KitName:=ReportName, Description:=Description, RelNoteNo:=ReleaseNoteNo, TransTypeID:=cmbIssue.SelectedValue, FromStore:=FromStore, WorkShop:=WorkShop, WorkOrderText:=SerialNo, WorkOrderNo:=AppSettings("Logo"))
        'End
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 712)
        End If
        If IsExcel = False Then     'PDF format
            Dim ds As New dsStockUtilization
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "StockUtil", mStockUtilizationSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcelStockUtilization
            ds.Clear()
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            da.Fill(ds, "rptStockUtilization", rpt)

            Dim columnToRemove2 As String() = {"CompanyName", "Nomenclature", "Store", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"IssueID", "SrNo", "ReleaseNoteInfo", "IssueText", "IssueNo", "Factor"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("rptStockUtilization").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("rptStockUtilization").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("rptStockUtilization").Columns.Contains("CommercialRate") Then
                ds.Tables("rptStockUtilization").Columns("CommercialRate").ColumnName = RateType
            End If

            If ds.Tables("rptSearchingCriteria").Columns.Contains("BranchName") Then
                ds.Tables("rptSearchingCriteria").Columns("BranchName").ColumnName = "Issue No."
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Aircraft") Then
                ds.Tables("rptSearchingCriteria").Columns("Aircraft").ColumnName = "Status"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("WorkOrderText") Then
                ds.Tables("rptSearchingCriteria").Columns("WorkOrderText").ColumnName = "SerialNo"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
			ds.Tables("rptStockUtilization").TableName = ReportName
			Session("ExcelFileName") = ReportName
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "StockUtil", "Export To Excel " + mStockUtilizationSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub SetTitle()
        cmbType.Enabled = False
        txtNo.Text = ""
        Dim Index As Int16 = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedIndex, 0)
        lblType1.Visible = (Index > 0)
        lblType1.Text = IIf(Index = 0, "", IIf(Index = 1, IIf(mTransTypeID = 25, "Customer ", "Supplier  "), IIf(Index = 2, "", IIf(Index = 3, "Work Shop ", ""))))
        txtCustomer.Visible = IIf(cmbType.SelectedItem.Text = "Customer", True, False)
        txtSupplier.Visible = IIf(cmbType.SelectedItem.Text = "Supplier", True, False)
        txtWorkShop.Visible = (Index = 3)
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetCustomer()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Customer", "1"))
        cmbType.Items.Add(New ListItem("Discard", "7"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
    End Sub
    Private Sub SetVendor()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Supplier", "1"))
        cmbType.Items.Add(New ListItem("Discard", "7"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
    End Sub
    Private Sub DataFieldBind()
        'Category
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList

        mStoreList = StoreList.GetStoreList(0, "", "(All)", IsForUserStoreRights:=True)
        cmbFromStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList

        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        DataBind()
    End Sub
    Private Sub FillCombo()
        Me.cmbIssue.Items.Clear()
        cmbIssue.Items.Add(New ListItem("(All)", "0"))
        cmbIssue.Items.Add(New ListItem("Issue to Supplier as None", "63"))
        cmbIssue.Items.Add(New ListItem("Issue to Customer as Sales", "25"))
        cmbIssue.Items.Add(New ListItem("Issue To Discard", "19"))
        cmbIssue.Items.Add(New ListItem("Issue to Work Shop as None", "44"))
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

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack Then
            RemoveSession()
            If cmbIssue.Enabled = True Then
                setFocus(cmbIssue)
            End If
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            SetTitle()
            DataFieldBind()
            FillCombo()
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
        ControlVisibility2()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx") ' Addedd By Prashant 9-Jan-2013
    End Sub
    Private Sub cmbIssue_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIssue.SelectedIndexChanged
        If cmbIssue.Enabled = True Then
            setFocus(cmbIssue)
        End If
        mTransTypeID = CType(cmbIssue.SelectedValue, Int16)
        Select Case (mTransTypeID)
            Case 0
                cmbType.SelectedIndex = 0
            Case 19
                cmbType.SelectedIndex = 2
            Case 25
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 44
                cmbType.SelectedIndex = 3
            Case 63
                SetVendor()
                cmbType.SelectedIndex = 1
        End Select
        txtSupplier.Text = ""
        txtCustomer.Text = ""
        txtWorkShop.Text = ""
        SetTitle()
        upnlSelectionOfIssue.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class

