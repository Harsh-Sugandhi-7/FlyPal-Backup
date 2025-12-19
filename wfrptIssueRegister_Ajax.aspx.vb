Imports System.Text
Public Class wfrptIssueRegister_Ajax
    Inherits System.Web.UI.Page
    'Added
#Region " Variable Declaration "
    Dim Fromdate As String = ""
    Dim ToDate As String = ""
    Dim RecText As String = ""
    Dim RecNo As String = ""
    Dim InternalReceiptNo As String = ""
    Dim Supplier As String = ""
    Dim Aircraft As String = ""
    Dim Store As String = ""
    Dim ToStore As String = ""
    Dim Status As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim OrdNo As String = ""
    Dim OrdText As String = ""
    Dim IssNo As String = ""
    Dim IssText As String = ""
    Dim ReleaseNoteNo As String = ""
    Dim SerialNo As String = ""
    Public mVendor As Vendor
    Dim mItemList As ItemList
    Dim mVendorList As VendorList
    Dim mStoreList As StoreList
    Dim mOrderTextList As DistinctTextListForOrder
    Dim mReceiptTextList As DistinctTextListForReceipt
    Dim mIssueTextList As DistinctTextListForIssue
    Dim mTransTypeID As Integer
    Public Shadows Title As String
    Public IssueType As String
    Dim mWorkShopList As WorkShopList
    Dim WorkShop As String = ""
    Dim WorkOrderText As String = ""
    Dim WorkOrderNo As String = ""
    'Public mWOList As FlyPal22.Maintain.WOList
    Dim mDistinctWOText As nDistinctWOText
    'Added By Vikrant on 29-Aug-2012
    Dim RequisitionText As String = ""
    Dim RequisitionNo As String = ""
    'End
    Dim mIssueRegisterSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    'Added By Vikrant On 13-May-2016 For All13062016
    Dim AircraftName As New StringBuilder
    Dim AircraftIds As New StringBuilder
    Public mMachineNameValueList As MachineNameValueList
    'End
#End Region

#Region " Business Methods "
    Private Function GetTitle() As String
        Dim mTransTypeList As TransactionList
        Dim mTitle As String
        mTransTypeList = TransactionList.GetTransactionList()
        mTransTypeID = CType(cmbIssue.SelectedValue, Int16)

        mTransTypeList = TransactionList.GetTransactionList("Issue")

        mTitle = mTransTypeList.GetTransactionTypeName(cmbIssue.SelectedValue).ToString + " Register"

        If chkDetail.Checked Then
            If mTitle = "" Then
                Return "Issue Register (Detail Report)"
            Else
                Return mTitle + " (Detail Report)"
            End If
        Else
            If mTitle = "" Then
                Return "Issue Register (Summary Report)"
            Else
                Return mTitle + " (Summary Report)"
            End If
        End If
        Return mTitle
    End Function
    Private Sub GetSession()
        mItemList = Session("mItemList")
        PartNo = Session("PartNo")
        Description = Session("Description")
        mVendorList = CType(Session("mVendorList"), VendorList)
        mStoreList = CType(Session("mStoreList"), StoreList)
        mOrderTextList = CType(Session("mOrderTextList"), DistinctTextListForOrder)
        mReceiptTextList = CType(Session("mReceiptTextList"), DistinctTextListForReceipt)
        mIssueTextList = CType(Session("mIssueTextList"), DistinctTextListForIssue)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mTransTypeID = CType(Session("mTransTypeID"), Int16)
        'Rajnish 14-08-2008
        'mWOList = Session("mWOList")
        mDistinctWOText = Session("mDistinctWOText")
        mMachineNameValueList = Session("mMachineNameValueList") 'Added By Vikrant On 13-May-2016 For All13062016
    End Sub
    Private Sub setSession()
        Session("mItemList") = mItemList
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("mVendorList") = mVendorList
        Session("mStoreList") = mStoreList
        Session("mOrderTextList") = mOrderTextList
        Session("mReceiptTextList") = mReceiptTextList
        Session("mIssueTextList") = mIssueTextList
        Session("mTransTypeID") = mTransTypeID
        'Session("mWOList") = mWOList
        Session("mDistinctWOText") = mDistinctWOText
    End Sub
    Private Sub RemoveSession()
        mItemList = Nothing
        PartNo = Nothing
        Description = Nothing
        mVendorList = Nothing
        mStoreList = Nothing
        mOrderTextList = Nothing
        mReceiptTextList = Nothing
        mIssueTextList = Nothing
        'mWOList = Nothing
        mDistinctWOText = Nothing
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mVendorList")
        Session.Remove("mStoreList")
        Session.Remove("mOrderTextList")
        Session.Remove("mReceiptTextList")
        Session.Remove("mIssueTextList")
        Session.Remove("mTransTypeID")
        Session.Remove("mMachineNameValueList") 'Added By Vikrant On 13-May-2016 For All13062016
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchIssueRegister"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@FromDate", Fromdate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@Text", IssText)
        cmd.Parameters.AddWithValue("@No", IssNo)
        cmd.Parameters.AddWithValue("@StoreName", Store)
        cmd.Parameters.AddWithValue("@AircraftName", Aircraft)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@ToTypeID", IIf(mTransTypeID = 14 Or mTransTypeID = 44, 0, CInt(cmbType.SelectedValue)))
        cmd.Parameters.AddWithValue("@StatusID ", cmbStatus.SelectedValue)
        cmd.Parameters.AddWithValue("@ReceiptText", RecText)
        cmd.Parameters.AddWithValue("@ReceiptNo ", RecNo)
        cmd.Parameters.AddWithValue("@SerialNo", SerialNo)
        cmd.Parameters.AddWithValue("@ReleaseNoteNo", ReleaseNoteNo)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@Description", Description)
        cmd.Parameters.AddWithValue("@FromStoreID", New Guid(cmbFromStore.SelectedValue.ToString))
        cmd.Parameters.AddWithValue("@ToStoreID", New Guid(cmbStore.SelectedValue.ToString))
        cmd.Parameters.AddWithValue("@TransTypeID", mTransTypeID)
        cmd.Parameters.AddWithValue("@WorkShopName", WorkShop)
        cmd.Parameters.AddWithValue("@WorkOrderText", WorkOrderText)
        cmd.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo)
        cmd.Parameters.AddWithValue("@ReportByValue", IIf(rdoBase.Checked, "Base Value", IIf(rdoLanding.Checked, "Landing Value", "Commercial Value")))
        cmd.Parameters.AddWithValue("@ZeroValueOnly", chkZeroValueOnly.Checked)
        cmd.Parameters.AddWithValue("@FromIsValuedStoresOnly", chkIsValued.Checked)
        cmd.Parameters.AddWithValue("@AircraftIds", AircraftIds.ToString)

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        dataTable.Columns.Remove("Rem1")
        dataTable.Columns.Remove("Rem2")
        dataTable.Columns.Remove("Rem3")
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsIssue

        Dim objSearch As rptSearchingCriteriaForReceipt
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, AppSettings("Logo"), ReleaseNoteNo, RecText, IssText, "", RecNo, IssNo, "", AircraftName.ToString, Supplier, IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Status, "", PartNo, Description, "", "", ToStore, Title, "", "", SerialNo, IIf(rdoBase.Checked, "Base Value", IIf(rdoLanding.Checked, "Landing Value", "Commercial Value")), "", "", "", cmbIssue.SelectedValue, WorkShop, WorkOrderText, WorkOrderNo)


        ds.Clear()
        da.Fill(ds, objSearch)

        Dim columnToRemove As String() = {"ID", "CompanyName", "InternalReceiptNo", "OrdText", "OrdNo", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ClientCode"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
        dsNew.Merge(tbl)


        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("FromStore").ColumnName = "Issue From Store"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Store").ColumnName = "Issue To Store"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Charge").ColumnName = IIf(rdoBase.Checked, "Base Value", IIf(rdoLanding.Checked, "Landing Value", "Commercial Value"))

        dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = "Issue Register"
		Session("ExcelFileName") = "Issue Register"
		Session("dsNew") = dsNew
        'Session("DataTable") = tbl
        'Session("ReportName") = "RCI Register"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "IssueReg", "Export To excel " + mIssueRegisterSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
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
        If cmbFormat.Visible Then
            If cmbFormat.SelectedIndex = 0 Then
                chkDetail.Enabled = True
                optPortrait.Enabled = True
                optLandscape.Enabled = True
                chkIsValued.Enabled = True
                chkShowInValuation.Enabled = True
            Else
                chkDetail.Enabled = False
                optPortrait.Enabled = False
                optLandscape.Enabled = False
                chkIsValued.Enabled = False
                chkShowInValuation.Enabled = False
            End If
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblOrderNo.Visible = True
        lblSerialNo.Visible = True
        lblReleaseNoteNo.Visible = True
        lblStatus.Visible = True
        lblToStore.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblIssuetype.Visible = True
        lblWONo.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor.Visible = False
        lblOrderNo.Visible = False
        lblSerialNo.Visible = False
        lblReleaseNoteNo.Visible = False
        lblStatus.Visible = False
        lblToStore.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblWONo.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))  '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
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

        'Added by Utkarsh On 20-Dec-2011

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        'End

        Aircraft = IIf(cmbType.SelectedIndex = 2, txtAircraft.Text.Trim, "")
        Store = IIf(cmbType.SelectedIndex = 3 And cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "") 'Added By Prashant 30-Apr-2012 'ALL29042013
        RecText = IIf(cmbDocType.SelectedIndex = 1, IIf(txtReceiptTextList.Text.Trim <> "", txtReceiptTextList.Text.Trim, ""), "")
        RecNo = IIf(cmbDocType.SelectedIndex = 1, txtNo.Text.Trim, "")
        SerialNo = txtSerialNo.Text.Trim

        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        PartNo = IIf(Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Not IsNothing(Description), Description, "")
        OrdNo = IIf(cmbDocType.SelectedIndex = 3, txtNo.Text.Trim, "")
        IssNo = IIf(cmbDocType.SelectedIndex = 2, txtNo.Text.Trim, "")
        IssText = IIf(cmbDocType.SelectedIndex = 2, IIf(txtIssueTextList.Text <> "", txtIssueTextList.Text, ""), "")
        ReleaseNoteNo = txtReleaseNoteNo.Text.Trim
        ToStore = IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, "") 'Added By Prashant 30-Apr-2012 'ALL29042013
        WorkShop = IIf(cmbType.SelectedIndex = 5, txtWorkShop.Text, "")
        WorkOrderNo = IIf(cmbType.SelectedIndex = 6, txtWONo.Text.Trim, "")
        WorkOrderText = IIf(cmbType.SelectedIndex = 6, txtWorkOrder.Text, "")
        'Added By Vikrant On 13-May-2016 For All13062016
        For i As Integer = 0 To ChkAircraftList.Items.Count - 1
            If ChkAircraftList.Items(i).Selected Then
                AircraftName.Append(ChkAircraftList.Items(i).Text + ",")
                AircraftIds.Append(ChkAircraftList.Items(i).Value + ",")
            End If
        Next
        If AircraftName.ToString <> "" Then
            AircraftName.Remove(AircraftName.Length - 1, 1)
        End If
        If AircraftIds.ToString <> "" Then
            AircraftIds.Remove(AircraftIds.Length - 1, 1)
        End If
        'End
        lblReleaseNoteNo.Text = "Release Note No. : " & IIf(ReleaseNoteNo <> "", ReleaseNoteNo, "All")
        lblSerialNo.Text = "Serial No. :" & IIf(SerialNo <> "", SerialNo, "All")
        lblStatus.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblToStore.Text = "To Store : " & IIf(ToStore <> "", ToStore, "All")
        IssueType = IIf(cmbIssue.SelectedIndex > 0, cmbIssue.SelectedItem.Text, "")
        lblIssuetype.Text = "Issue Type : " & IIf(IssueType <> "", IssueType, "All")

        lblWONo.Text = "WO No. : " + txtWorkOrder.Text + "-" + WorkOrderNo

        Select Case cmbDocType.SelectedIndex
            Case 0
                lblOrderNo.Text = "Document Type : All "
            Case 1
                If RecText = "" Then
                    lblOrderNo.Text = "Receipt No. : All "
                Else
                    lblOrderNo.Text = "Receipt No. : " + RecText + "-" + RecNo
                End If
            Case 2
                If IssText = "" Then
                    lblOrderNo.Text = "Issue No. : All "
                Else
                    lblOrderNo.Text = "Issue No. : " + IssText + "-" + IssNo
                End If
            Case 3
                If OrdText = "" Then
                    lblOrderNo.Text = "Order No. : All "
                Else
                    lblOrderNo.Text = "Order No. : " + OrdText + "-" + OrdNo
                End If
        End Select
        Select Case cmbType.SelectedIndex
            Case 0
                lblVendor.Text = "To Type : All"
            Case 1 'Vendor
                lblVendor.Text = IIf(mTransTypeID = 25 Or mTransTypeID = 26 Or mTransTypeID = 78, "Customer : " & IIf(Supplier <> "", Supplier, "All"), "Supplier : " & IIf(Supplier <> "", Supplier, "All"))
            Case 2 'Aircraft
                lblVendor.Text = "Aircraft : " & IIf(AircraftName.ToString <> "", AircraftName.ToString, "All")
            Case 3 'Store
                lblVendor.Text = "Store : " & IIf(Store <> "", Store, "All")
            Case 4 'Discard
                lblVendor.Text = "Discard "
            Case 5  'WorkShop
                lblVendor.Text = "WorkShop : " & IIf(WorkShop <> "", WorkShop, "All")
            Case 6  'WorkOrder
                lblVendor.Text = "WorkOrder : " & IIf(WorkOrderText <> "", WorkOrderText, "All")
                'Case 7  'Requisition Added By Vikrant on 29-Aug-2012
                'lblVendor.Text = "Requisition : " & IIf(WorkOrderText <> "", WorkOrderText, "All")
        End Select
        mIssueRegisterSearchingCriteria = lblDateRange.Text.ToString + ", " + lblIssuetype.Text.ToString + ", " + lblOrderNo.Text.ToString + ", " + lblVendor.Text.ToString + ", " + (lblReleaseNoteNo.Text.Trim) + ", " + lblSerialNo.Text + ", " + lblToStore.Text + ", " + lblStatus.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + "Format " + IIf(chkDetail.Checked, IIf(optLandscape.Checked, "Detail LandScape", "Detail Portrait"), IIf(optLandscape.Checked, "Detail LandScape", "Detail Portrait"))
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim objReg As rptIssueReg
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsReceipt As New dsIssue 'dsReceipt
        Dim Value As String = ""
        SetValues()
        Title = GetTitle()

        If rdoBase.Checked = True Then
            Value = "Base Value"
        ElseIf rdoLanding.Checked = True Then
            Value = "Landing Value"
        Else
            Value = "Commercial Value"
        End If

        objReg = rptIssueReg.GetrptIssueList(IssText, IssNo, Fromdate, ToDate, IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Supplier, Aircraft,
                                             Val(cmbType.SelectedValue), Val(cmbStatus.SelectedValue), RecText, RecNo, ReleaseNoteNo, SerialNo, PartNo, Description,
                                             cmbFromStore.SelectedValue.ToString, cmbStore.SelectedValue.ToString, cmbIssue.SelectedValue, WorkShop, WorkOrderText,
                                             WorkOrderNo, Value, ZeroValueOnly:=chkZeroValueOnly.Checked, FromIsValuedStoresOnly:=IIf(cmbFormat.SelectedIndex = 0 And AppSettings("ClientCode") = "Taj", True, chkIsValued.Checked),
                                             AircraftIds:=AircraftIds.ToString, ConsiderShowInValuationOnly:=chkShowInValuation.Checked,
                                             IsForFormat2:=IIf(cmbFormat.SelectedIndex = 0, False, True), ClientCode:=AppSettings("ClientCode"))
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, AppSettings("Logo"), ReleaseNoteNo, RecText, IssText, AppSettings("ClientCode"), RecNo, IssNo, "", AircraftName.ToString, Supplier, IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Status, IIf(chkShowInValuation.Checked = True, "<b>Note: " + "Report shows records marked with Show in Valuation", ""), PartNo, Description, "", "", ToStore, Title, "", "", SerialNo, Value, "", "", "", cmbIssue.SelectedValue, WorkShop, WorkOrderText, WorkOrderNo)

        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 633)
        End If

        If cmbFormat.SelectedIndex = 0 Then
            If chkDetail.Checked Then
                If optPortrait.Checked Then
                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        myReport = New crptIssueRegisterDetailPortrait
                    Else
                        myReport = New crptIssueRegister
                    End If
                Else
                    If chkWithRate.Checked Then
                        myReport = New crptIssueRegisterLandscape           'With rate
                    Else
                        myReport = New crptIssueRegisterDetailLandscape     'Without rate
                    End If
                End If
            Else
                If optPortrait.Checked Then
                    myReport = New crptIssueRegSummary
                Else
                    myReport = New crptIssueRegSummarylandscape
                End If
            End If
        Else
            myReport = New crptIssueRegisterDetailLandscapeTajAirFormat2
        End If
        dsReceipt.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt)
        da.Fill(dsReceipt, objReg)
        da.Fill(dsReceipt, mrptImage)
        da.Fill(dsReceipt, objSearch)
        myReport.SetDataSource(dsReceipt)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "IssueReg", mIssueRegisterSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetTitle()
        cmbType.Enabled = False
        txtNo.Text = ""
        txtWONo.Text = ""
        Dim Index As Int16 = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedIndex, 0)
        lblType1.Visible = (Index > 0 And Index <> 2)
        lblType1.Text = IIf(Index = 0 Or Index = 7, "", IIf(Index = 1, IIf(mTransTypeID = 26 Or mTransTypeID = 25 Or mTransTypeID = 51 Or mTransTypeID = 58 Or mTransTypeID = 78, "Customer ", "Supplier  "), IIf(Index = 2, "Aircraft  ", IIf(Index = 3, "Store  ", IIf(Index = 6, "WorkOrder  ", IIf(Index = 5, "WorkShop  ", ""))))))
        txtCustomer.Visible = IIf(cmbType.SelectedItem.Text = "Customer", True, False)
        txtSupplier.Visible = IIf(cmbType.SelectedItem.Text = "Supplier", True, False)
        'txtAircraft.Visible = (Index = 2)
        cmbStore.Visible = (Index = 3) 'Added By Prashant 30-Apr-2012 'ALL29042013
        txtWorkShop.Visible = (Index = 5)
        txtWorkOrder.Visible = (Index = 6)
        txtWONo.Visible = (Index = 6)
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        lbltitle.Text = "Issue Register "
        chkSelectAll.Visible = (Index = 2)
        ChkAircraftList.Visible = (Index = 2)
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
        ' btnDisplay.Attributes("onclick") = "javascript: document.body.style.cursor = 'wait';"
    End Sub
    Private Sub SetCustomer()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Customer", "1"))
        cmbType.Items.Add(New ListItem("Aircraft", "2"))
        cmbType.Items.Add(New ListItem("Store", "8"))
        cmbType.Items.Add(New ListItem("Discard", "7"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
        cmbType.Items.Add(New ListItem("WorkOrder", "17"))
        'cmbType.Items.Add(New ListItem("Requisition", "18")) 'Added By Vikrant on 29-Aug-2012
    End Sub
    Private Sub SetVendor()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Supplier", "1"))
        cmbType.Items.Add(New ListItem("Aircraft", "2"))
        cmbType.Items.Add(New ListItem("Store", "8"))
        cmbType.Items.Add(New ListItem("Discard", "7"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
        cmbType.Items.Add(New ListItem("WorkOrder", "17"))
        'cmbType.Items.Add(New ListItem("Requisition", "18")) 'Added By Vikrant on 29-Aug-2012
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()  'Added By Prashant 30-Apr-2012 'ALL29042013
        mStoreList = StoreList.GetStoreList(0, "", "(All)")
        cmbStore.DataSource = mStoreList
        cmbFromStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList

        'Added By Vikrant On 13-May-2016 For All13062016
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString)
        Session("mMachineNameValueList") = mMachineNameValueList
        ChkAircraftList.DataSource = mMachineNameValueList
        'End
        DataBind()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()

            If cmbIssue.Enabled = True Then
                setFocus(cmbIssue)
            End If

            DataFieldBind()     'Added By Prashant 30-Apr-2012 'ALL29042013

            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            SetTitle()
            Me.cmbIssue.Items.Clear()

            cmbIssue.Items.Add(New ListItem("(All)", "00"))
            If User.IsInRole("IssueToAircraftView") Then cmbIssue.Items.Add(New ListItem("To Aircraft", "14"))
            If User.IsInRole("IssueToStoreView") Then cmbIssue.Items.Add(New ListItem("To Store", "15"))
            If User.IsInRole("IssueToCustomerView") Then cmbIssue.Items.Add(New ListItem("To Customer", "25"))
            If User.IsInRole("IssueToCustomerView") Then cmbIssue.Items.Add(New ListItem("Loan To Customer", "26"))
            If User.IsInRole("IssueLoanToStoreView") Then cmbIssue.Items.Add(New ListItem("Loan To Another Store", "17"))
            If User.IsInRole("IssueLoanToAircraftView") Then cmbIssue.Items.Add(New ListItem("Loan To Aircraft", "20"))
            If User.IsInRole("LoanIssueToVendorView") Then cmbIssue.Items.Add(New ListItem("Loan To Supplier", "24"))
            If User.IsInRole("IssueLoanReturnToStoreView") Then cmbIssue.Items.Add(New ListItem("Loan Return To Store", "18"))
            If User.IsInRole("IssueToVendorForExchangeView") Then cmbIssue.Items.Add(New ListItem("To Supplier For Exchange/Repair", "16"))
            If User.IsInRole("IssueToDiscardView") Then cmbIssue.Items.Add(New ListItem("Part Discard", "19"))
            If User.IsInRole("IssueToWorkShopView") Then cmbIssue.Items.Add(New ListItem("To WorkShop", "44"))
            If User.IsInRole("IssueLoanToWorkShopView") Then cmbIssue.Items.Add(New ListItem("Loan To WorkShop", "45"))
            If User.IsInRole("IssueforLoanReturntoSupplierView") Then cmbIssue.Items.Add(New ListItem("Loan Return to Supplier", "49"))
            If User.IsInRole("IssueforLoanReturntoCustomerView") Then cmbIssue.Items.Add(New ListItem("Loan Return to Customer", "51"))
            If User.IsInRole("IssueToWorkOrderView") Then cmbIssue.Items.Add(New ListItem("To WorkOrder", "52"))
            If User.IsInRole("IssuetoSupplierasRentalLeaseView") Then cmbIssue.Items.Add(New ListItem("To Supplier As Rental/Lease", "55"))
            If User.IsInRole("IssueToCustomerAsRepairedReturnView") Then cmbIssue.Items.Add(New ListItem("Issue To Customer As Repaired Return", "58"))
            If User.IsInRole("IssueToWorkOrderAsSparesView") Then cmbIssue.Items.Add(New ListItem("To WorkOrder As Spares", "59"))
            If User.IsInRole("IssueToWorkOrderAsToolsView") Then cmbIssue.Items.Add(New ListItem("To WorkOrder As Tools", "60"))
            If User.IsInRole("IssueToWorkOrderAsToolsView") Then cmbIssue.Items.Add(New ListItem("To Customer As None", "78"))
            If User.IsInRole("IssuetoSupplierNoneView") Then cmbIssue.Items.Add(New ListItem("To Supplier As None", "63"))
            'If User.IsInRole("IssueToRequisitionView") Then cmbIssue.Items.Add(New ListItem("To Requisition", "72")) 'Added By Vikrant on 28-Aug-2012
            If mTransTypeID = 0 Then
                lblStep4.Text = "Step IV. Selection of All"
            End If

            cmbFormat.SelectedIndex = 0
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
        upnlCurrentSearchCriteria.Update()
    End Sub
    Private Sub cmbDocType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDocType.SelectedIndexChanged
        txtNo.Text = ""
        Dim Index As Int16 = IIf(cmbDocType.SelectedIndex > 0, cmbDocType.SelectedIndex, 0)
        lblDocTypeNo.Visible = (Index > 0)
        lblDocTypeNo.Text = IIf(Index = 0, "", IIf(Index = 1, "Receipt No.  ", IIf(Index = 2, "Issue No.  ", IIf(Index = 3, "Order No.  ", ""))))
        txtReceiptTextList.Visible = (Index = 1)
        txtIssueTextList.Visible = (Index = 2)
        txtNo.Visible = (Index = 1 Or Index = 2)
        txtReceiptTextList.Text = ""
        txtIssueTextList.Text = ""
        If cmbDocType.Enabled = True Then
            setFocus(cmbDocType)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbIssue_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIssue.SelectedIndexChanged
        If cmbIssue.Enabled = True Then
            setFocus(cmbIssue)
        End If
        mTransTypeID = CType(cmbIssue.SelectedValue, Int16)
        Select Case (mTransTypeID)
            Case 0
                lblStep4.Text = "Step IV. Selection of All"
                cmbType.SelectedIndex = 0
            Case 14
                lblStep4.Text = "Step IV. Selection of Aircraft"
                cmbType.SelectedIndex = 2
            Case 15
                lblStep4.Text = "Step IV. Selection of Store"
                cmbType.SelectedIndex = 3
            Case 16
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 17
                lblStep4.Text = "Step IV. Selection of Store"
                cmbType.SelectedIndex = 3
            Case 18
                lblStep4.Text = "Step IV. Selection of Store"
                cmbType.SelectedIndex = 3
            Case 19
                lblStep4.Text = "Step IV. Discard"
                cmbType.SelectedIndex = 4
            Case 20
                lblStep4.Text = "Step IV. Selection of Aircraft"
                cmbType.SelectedIndex = 2
            Case 24
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 25, 78
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 26
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 44
                lblStep4.Text = "Step IV. Selection of WorkShop"
                cmbType.SelectedIndex = 5
            Case 45
                lblStep4.Text = "Step IV. Selection of WorkShop"
                cmbType.SelectedIndex = 5
            Case 49
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 51
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 52
                lblStep4.Text = "Step IV. Selection of WorkOrder"
                cmbType.SelectedIndex = 6
            Case 55, 63
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 58
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 59
                lblStep3.Text = "Step III. Selection of WorkOrder"
                cmbType.SelectedIndex = 6
            Case 60
                lblStep3.Text = "Step III. Selection of WorkOrder"
                cmbType.SelectedIndex = 6
                'Added By Vikrant on 28-Aug-2012
                'Case 72
                'lblStep4.Text = "Step IV. Requisition"
                'cmbType.SelectedIndex = 7
                'End
        End Select
        txtAircraft.Text = ""
        txtSupplier.Text = ""
        txtCustomer.Text = ""
        txtWorkOrder.Text = ""
        txtWorkShop.Text = ""
        cmbStore.SelectedIndex = 0 'Added By Prashant 30-Apr-2012 'ALL29042013
        ChkAircraftList.ClearSelection()
        SetTitle()

        UpnlToTypeSelection.Update()
    End Sub
    Private Sub chkDetail_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkDetail.CheckedChanged
        If chkDetail.Checked = True And optLandscape.Checked = True Then 'Added By Prashant 8-May-2013  'ALL08052013
            rdoBase.Enabled = True
            rdoLanding.Enabled = True
            rdoCommercial.Enabled = True
            chkWithRate.Enabled = True
            chkWithRate.Checked = True
        Else
            rdoBase.Enabled = False
            rdoLanding.Enabled = False
            rdoCommercial.Enabled = False
            chkWithRate.Enabled = False
            chkWithRate.Checked = False
            chkZeroValueOnly.Enabled = False
            chkZeroValueOnly.Checked = False
        End If
        upnlReportFormatSelection.Update()
    End Sub
    Private Sub optLandscape_CheckedChanged(sender As Object, e As System.EventArgs) Handles optLandscape.CheckedChanged
        If chkDetail.Checked = True And optLandscape.Checked = True Then 'Added By Prashant 8-May-2013  'ALL08052013
            rdoBase.Enabled = True
            rdoLanding.Enabled = True
            rdoCommercial.Enabled = True
            chkWithRate.Enabled = True
            chkWithRate.Checked = True
            chkZeroValueOnly.Enabled = True
        Else
            rdoBase.Enabled = False
            rdoLanding.Enabled = False
            rdoCommercial.Enabled = False
            chkWithRate.Enabled = False
            chkWithRate.Checked = False
            chkZeroValueOnly.Enabled = False
            chkZeroValueOnly.Checked = False
        End If
        upnlReportFormatSelection.Update()
    End Sub
    Private Sub optPortrait_CheckedChanged(sender As Object, e As System.EventArgs) Handles optPortrait.CheckedChanged
        If chkDetail.Checked = True And optLandscape.Checked = True Then 'Added By Prashant 8-May-2013  'ALL08052013
            rdoBase.Enabled = True
            rdoLanding.Enabled = True
            rdoCommercial.Enabled = True
            chkWithRate.Enabled = True
            chkWithRate.Checked = True
        Else
            rdoBase.Enabled = False
            rdoLanding.Enabled = False
            rdoCommercial.Enabled = False
            chkWithRate.Enabled = False
            chkWithRate.Checked = False
            chkZeroValueOnly.Enabled = False
            chkZeroValueOnly.Checked = False
        End If
        upnlReportFormatSelection.Update()
    End Sub
    Private Sub chkWithRate_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkWithRate.CheckedChanged
        If chkWithRate.Checked = True Then
            rdoBase.Enabled = True
            rdoLanding.Enabled = True
            rdoCommercial.Enabled = True
            chkZeroValueOnly.Enabled = True
        Else
            rdoBase.Enabled = False
            rdoLanding.Enabled = False
            rdoCommercial.Enabled = False
            chkZeroValueOnly.Enabled = False
            chkZeroValueOnly.Checked = False
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'Added By Vikrant On 13-May-2016 For All13062016
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        SetValues()
        GenerateXLSXFile(CreateDataTable())
    End Sub
    'End
    Private Sub cmbFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedIndex = 0 Then
            chkDetail.Enabled = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True
            chkIsValued.Enabled = True
            chkShowInValuation.Enabled = True
        Else
            chkDetail.Enabled = False
            optPortrait.Enabled = False
            optLandscape.Enabled = False
            chkIsValued.Enabled = False
            chkIsValued.Checked = False
            chkShowInValuation.Enabled = False
            chkShowInValuation.Checked = False
        End If
    End Sub
#End Region

    
   
End Class