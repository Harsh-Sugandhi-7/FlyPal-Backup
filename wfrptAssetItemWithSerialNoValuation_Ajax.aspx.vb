Public Class wfrptAssetItemWithSerialNoValuation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String = ""
    Public Description As String = ""
    Dim EventLogID As Guid 'Added by Prashant
    Dim mAssetValuationSearchingCriteria As String = String.Empty
    Public mModelList As ModelList 'Added By Prashant 3-Mar-2014  ALL03032014
    Dim mModel As String = ""
    Public mStoreList As StoreList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
     Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        If Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 0 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
       lblModel.Visible = True
        lblStoreName.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblModel.Visible = False
        lblStoreName.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 1 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Quater
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
            Case 3 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 4 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then      ''Date Range
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        If (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = False) Then
            mModel = ""
        ElseIf (cmbModel.SelectedIndex = 0 And chkCommonOrApplicability.Checked = True) Then
            mModel = "Common/No Applicability"
        Else
            mModel = cmbModel.SelectedItem.Text
        End If
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblModel.Text = "Model : " & IIf(cmbModel.SelectedIndex = 0, "", cmbModel.SelectedItem.Text)
        lblStoreName.Text = "Store : " & IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedItem.Text)
        mAssetValuationSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblModel.Text + ", " + lblStoreName.Text
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As AssetItemWithSerialNoValuation
        Dim Value As String = ""
        Dim ReportName As String = ""
        SetValues()
        Dim ds As New dsAssetItemWithSerialNoValuation
        myReport = New crptAssetItemWithSerialNoValuation
        rpt = AssetItemWithSerialNoValuation.GetAssetItemValuation(FromDate, ToDate, PartNo, Description, "", cmbModel.SelectedValue, 0, _
                                                                   chkCommonOrApplicability.Checked, StoreID:=cmbStore.SelectedValue, CategoryType:=cmbCategory.SelectedValue)
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, "", mModel, IIf(cmbCategory.SelectedValue = 0, "", cmbCategory.SelectedItem.Text), _
                                                              Description, store:=IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedItem.Text), _
                                                              Aircraft:="", KitName:="", Description:="", RelNoteNo:="", TransTypeID:=0, FromStore:="", _
                                                              WorkShop:="", WorkOrderText:="", WorkOrderNo:=AppSettings("Logo"), _
                                                              Search1:=txtBottomLine.Text.Trim)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1259)
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
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "AssetValuation", mAssetValuationSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        mModelList = ModelList.GetAirframeModelList("ALL")
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()
        'Store
        mStoreList = StoreList.GetStoreList(0, "", "ALL", True)
        cmbStore.DataSource = mStoreList
        DataBind()
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

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
            DataFieldBind()
            ControlVisibility(5)
            setDatePeroid(5)
            cmbDateRange.SelectedIndex = 5
        End If
        MessageBoxResult()
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
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            Dim da As New CSLA.Data.ObjectAdapter
            Dim objsearch As rptSearchingCriteria
            Dim rpt As AssetItemWithSerialNoValuation
            SetValues()
            Dim ds As New dsAssetItemWithSerialNoValuation
           
            rpt = AssetItemWithSerialNoValuation.GetAssetItemValuation(FromDate, ToDate, PartNo, Description, "", cmbModel.SelectedValue, 0, _
                                                                       chkCommonOrApplicability.Checked, StoreID:=cmbStore.SelectedValue, _
                                                                       CategoryType:=cmbCategory.SelectedValue)
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, "", "", IIf(cmbCategory.SelectedValue = 0, "", cmbCategory.SelectedItem.Text), "", store:=IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedItem.Text), Aircraft:=mModel, KitName:="", Description:=Description, RelNoteNo:="", TransTypeID:=0, FromStore:="", WorkShop:="", WorkOrderText:="", WorkOrderNo:=AppSettings("Logo"), Search1:=txtBottomLine.Text.Trim)

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ds.Clear()
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)

            Dim columnToRemove1 As String() = {"ItemID", "CategoryID", "CategoryName", "CategoryGLCode", "PrimaryCategoryID", "ATACode", "ATANomenclature", "ConsumedQty", "ConsumedAmount", "TransTypeID"}
            Dim columnToRemove2 As String() = {"CompanyName", "BranchName", "SupplierName", "Nomenclature", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("AssetItemWithSerialNoValuation").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("AssetItemWithSerialNoValuation").Columns.Remove(columnToRemove1(i))
                End If
            Next
            ds.Tables("rptSearchingCriteria").Columns("Aircraft").ColumnName = "Model"
            ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            ds.Tables("AssetItemWithSerialNoValuation").TableName = "Asset Val For Rot and Tools"
            ds.Tables.Remove("rptImage")
			Session("ExcelFileName") = "Asset Val For Rot and Tools"
			Session("dsNew") = ds
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "AssetValuation", "Export To excel  " + mAssetValuationSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
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