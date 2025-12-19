Public Class wfrptStoreToStore_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public FromDate As String = "1-1-1900"
    Public ToDate As String = "1-1-2200"
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim FromStore As String = ""
    Dim ToStore As String = ""
    Dim mStoreList As StoreList     'Added By Prashant 3-May-2013 'ALL29042013
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    'Added By Abhishek On 11-OCT-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteriaForReceipt
    Dim mStoreToAircraftList As rptStoreToStore
    Dim ds As New dsStoreTransactions
#End Region

#Region " Helper Methods "
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
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblIssueStore1.Visible = True
        lblReceiveStore1.Visible = True
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
        If cmbDateRange.SelectedIndex = 0 Then      'Date Range
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(txtFromDate.Text).FormattedText & " To " & New SmartDate(txtToDate.Text).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If

        If txtFromDate.Text = "" Then
            FromDate = "1-1-1900"
        ElseIf txtToDate.Text = "" Then
            ToDate = "1-1-2200"
        End If

        FromStore = IIf(cmbFromStoreList.SelectedIndex > 0, cmbFromStoreList.SelectedItem.Text, "All") 'Added By Prashant 3-May-2013  'ALL29040213
        lblIssueStore1.Text = "From Store Name : " & FromStore

        ToStore = IIf(cmbToStoreList.SelectedIndex > 0, cmbToStoreList.SelectedItem.Text, "All") 'Added By Prashant 3-May-2013  'ALL29040213
        lblReceiveStore1.Text = "To Store Name : " & ToStore

        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + "," + "Issuing Store : " + cmbFromStoreList.SelectedItem.ToString + "," + "Receiving Store : " + cmbToStoreList.SelectedItem.ToString + "," + "Transaction Type : " + IIf(rbLoanTrans.Checked, "Loan Transaction", "Plain Transaction") + "," + lblPartNo.Text + "," + lblDesc.Text
    End Sub
    Private Sub SetReport()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForReceipt
        Dim mStoreToAircraftList As rptStoreToStore
        Dim ds As New dsStoreTransactions
        myReport = New crptStoreToStore

        SetValues()

        'Dim mStoreList As StoreList = StoreList.GetStoreList(0, , True)

        If rbLoanTrans.Checked = True Then
            mStoreToAircraftList = rptStoreToStore.GetStoreToStore(New Guid(cmbFromStoreList.SelectedValue.ToString), New Guid(cmbToStoreList.SelectedValue.ToString), PartNo, Description, FromDate, ToDate, True, chkIsLoanIssued.Checked, chkIsLoanTaken.Checked, chkIsLoanReturn.Checked, chkIsLoanGetBack.Checked)
        ElseIf rbShowPlaneTransactions.Checked = True Then
            mStoreToAircraftList = rptStoreToStore.GetStoreToStore(New Guid(cmbFromStoreList.SelectedValue.ToString), New Guid(cmbToStoreList.SelectedValue.ToString), PartNo, Description, FromDate, ToDate, False, chkIssuedToStore.Checked, chkReceivedByStore.Checked, False, False)
        End If

        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), FromDate, ToDate, "", "", "", "", "", "", "", "", "", "", IIf(cmbToStoreList.SelectedIndex > 0, cmbToStoreList.SelectedItem.Text, ""), "", "", PartNo, Description, "", "", IIf(cmbFromStoreList.SelectedIndex > 0, cmbFromStoreList.SelectedItem.Text, ""), AppSettings("Logo"), "", "", "", "", "", "", "") 'Changed By Utkarsh For Report Logo.

        If mStoreToAircraftList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 716)
        End If

        ds.Clear()
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, mStoreToAircraftList)
        da.Fill(ds, objsearch)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "StoreToStore", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    'Response.Redirect("wfrptStoreToStore_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfrptStoreToStore_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(0, "", "(Select)", IsForUserStoreRights:=True) 'Added By Prashant 3-May-2013 'ALL29042013
        cmbFromStoreList.DataSource = mStoreList
        cmbToStoreList.DataSource = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)" 'Added by Saylee on 3-Jun-2020, LockDown 5.0

        DataBind()
    End Sub
    
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
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
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
    'Added By Abhishek On 11-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            If rbLoanTrans.Checked = True Then
                mStoreToAircraftList = rptStoreToStore.GetStoreToStore(New Guid(cmbFromStoreList.SelectedValue.ToString), New Guid(cmbToStoreList.SelectedValue.ToString), PartNo, Description, FromDate, ToDate, True, chkIsLoanIssued.Checked, chkIsLoanTaken.Checked, chkIsLoanReturn.Checked, chkIsLoanGetBack.Checked)
            ElseIf rbShowPlaneTransactions.Checked = True Then
                mStoreToAircraftList = rptStoreToStore.GetStoreToStore(New Guid(cmbFromStoreList.SelectedValue.ToString), New Guid(cmbToStoreList.SelectedValue.ToString), PartNo, Description, FromDate, ToDate, False, chkIssuedToStore.Checked, chkReceivedByStore.Checked, False, False)
            End If

            objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), FromDate, ToDate, "", "", "", "", "", "", "", "", "", "", IIf(cmbToStoreList.SelectedIndex > 0, cmbToStoreList.SelectedItem.Text, ""), "", "", PartNo, Description, "", "", IIf(cmbFromStoreList.SelectedIndex > 0, cmbFromStoreList.SelectedItem.Text, ""), AppSettings("Logo"), "", "", "", "", "", "", "") 'Changed By Utkarsh For Report Logo.

            If mStoreToAircraftList.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 716)
            End If
            da.Fill(ds, "ExcelrptStoreToStore", mStoreToAircraftList)
            da.Fill(ds, "rptSearchingCriteriaForReceipt", objsearch)
            Dim columnToRemove1 As String() = {"SerialNo", "TransTypeID", "WorkShop", "WorkOrderText", "WorkOrderNo", "CompanyName", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "Supplier", "Status", "DCNo", "InvText", "InvNo", "Amend", "currencyName", "ProductVersion", "SINote", "CurrencySymbol", "ToInvDate", "FromInvDate", "SuppInvNo", "Charge", "IntOrderNo", "QuotationNo"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"ID", "Text", "No", "TextNo", "FromStoreID", "FromStoreName", "ToStoreID", "ToStoreName", "TransTypeID", "ItemID", "BaseTransaction", "IsLoanTransactionRequesting"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ExcelrptStoreToStore").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ExcelrptStoreToStore").Columns.Remove(columnToRemove2(i))
                End If
            Next
            If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains("Store") Then
                ds.Tables("rptSearchingCriteriaForReceipt").Columns("Store").ColumnName = "To Store"
            End If
            If ds.Tables("ExcelrptStoreToStore").Columns.Contains("ItemName") Then
                ds.Tables("ExcelrptStoreToStore").Columns("ItemName").ColumnName = "Part Number Description"
            End If
            If ds.Tables("ExcelrptStoreToStore").Columns.Contains("TransTypeName") Then
                ds.Tables("ExcelrptStoreToStore").Columns("TransTypeName").ColumnName = "Transaction Type"
            End If
            If ds.Tables("ExcelrptStoreToStore").Columns.Contains("ExcelReceiptNumber") Then
                ds.Tables("ExcelrptStoreToStore").Columns("ExcelReceiptNumber").ColumnName = "Receipt Number"
            End If
            If ds.Tables("ExcelrptStoreToStore").Columns.Contains("ExcelIssueNumber") Then
                ds.Tables("ExcelrptStoreToStore").Columns("ExcelIssueNumber").ColumnName = "Issue Number"
            End If

            If ds.Tables("ExcelrptStoreToStore").Columns.Contains("SerialNo") Then
                ds.Tables("ExcelrptStoreToStore").Columns("SerialNo").ColumnName = "Serial  No."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
            dsNew.Merge(ds.Tables("ExcelrptStoreToStore"))

            dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptStoreToStore").TableName = "Store Transaction With Store"
			Session("ExcelFileName") = "Store Transaction With Store"
			Session("dsNew") = dsNew
			'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
			'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
			'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
			'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "StoreToStore", "Export To Excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
End Class