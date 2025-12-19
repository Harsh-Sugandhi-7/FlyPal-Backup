Public Class wfrptStoreToAircraft_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public mMachineNameValueList As MachineNameValueList
    Dim FromDate As String = "1-1-1900"
    Dim ToDate As String = "1-1-2200"
    Dim PartNo As String = String.Empty
    Dim Description As String = String.Empty
    Dim ToAircraft As String = String.Empty
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    'Added By Abhishek On 11-OCT-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteriaForReceipt
    Dim ds As New dsStoreTransactions '
    Dim mStoreToAircraftList As rptStoreToAircraft
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mStoreList") = mStoreList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mStoreList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)

        'Added By Saylee on 18-June 2007							
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
        lblStore1.Visible = True
        lblAircraft1.Visible = True
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

        lblStore1.Text = "Store : " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")

        ToAircraft = txtAircraftList.Text.Trim
        lblAircraft1.Text = "Aircraft : " & ToAircraft

        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + "," + "Store : " + cmbStore.SelectedItem.ToString + "," + "Transaction Type : " + IIf(rbLoanTrans.Checked, "Loan Transaction", "Plain Transaction") + "," + lblPartNo.Text + "," + lblDesc.Text
    End Sub
    Private Sub SetReport()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForReceipt

        SetValues()

        Dim ds As New dsStoreTransactions 'dsStockUtil
        myReport = New crptStoreToAircraft 'crptStockUtilization

        mMachineNameValueList = MachineNameValueList.GetMachineList(ToDate, , , , , , , False, , True)

        Dim mStoreToAircraftList As rptStoreToAircraft
        'mStoreToAircraftList = rptStoreToAircraft.GetStoreToAircraft(New Guid(cmbStore.SelectedValue), mMachineNameValueList.Item(txtAircraftList.Text.Trim).ID, PartNo, Description, FromDate, ToDate, rbLoanTrans.Checked, chkIsStoreToAircraft.Checked, chkIsAircraftToStore.Checked)

        If rbLoanTrans.Checked = True Then
            mStoreToAircraftList = rptStoreToAircraft.GetStoreToAircraft(New Guid(cmbStore.SelectedValue), mMachineNameValueList.Item(txtAircraftList.Text.Trim).ID, PartNo, Description, FromDate, ToDate, True, chkIsStoreToAircraft.Checked, chkIsAircraftToStore.Checked)  'Vikrant
        ElseIf rbShowPlaneTransactions.Checked = True Then
            mStoreToAircraftList = rptStoreToAircraft.GetStoreToAircraft(New Guid(cmbStore.SelectedValue), mMachineNameValueList.Item(txtAircraftList.Text.Trim).ID, PartNo, Description, FromDate, ToDate, False, chkIsStoreToAircraftPlane.Checked, chkIsAircraftToStorePlane.Checked) 'Vikrant
        End If

        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), FromDate, ToDate, "", "", "", "", "", "", "", "", txtAircraftList.Text.Trim, "", "", "", "", PartNo, Description, "", "", IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All"), AppSettings("Logo"), "", "", "", "", "", "", "") 'Changed By Utkarsh For Report Logo.)

        If mStoreToAircraftList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 717)
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
        MarkLog(Util.Action.Print, "StoreToAircraft", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                    'Response.Redirect("wfrptStoreToAircraft.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfrptStoreToAircraft.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Store
        mStoreList = StoreList.GetStoreList(0, "", "(All)", IsForUserStoreRights:=True)
        cmbStore.DataSource = mStoreList 'Added By Prashant On 30-Apr-2013 For ALL29042013-4
        Session("mStoreList") = mStoreList

        'Aircraft
        'mMachineNameValueList = tmpMachineList.GetMachineList(, , , , , "(All)")
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , False, , ForInventory:=True)

        'cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        'DataBind()
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
        lblStoreCount.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            'SetFocus(txtStoreList) '3/1/2012
            'DataFieldBind()
            'Added By Prashant On 30-Apr-2013 For ALL29042013-4
            mStoreList = StoreList.GetStoreList(0, "", "(Select)", IsForUserStoreRights:=True)
            cmbStore.DataSource = mStoreList
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            'End
            lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
            lblStoreCount.DataBind()
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
        RemoveSession()
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

            mMachineNameValueList = MachineNameValueList.GetMachineList(ToDate, , , , , , , False, , True)

            Dim mStoreToAircraftList As rptStoreToAircraft
            'mStoreToAircraftList = rptStoreToAircraft.GetStoreToAircraft(New Guid(cmbStore.SelectedValue), mMachineNameValueList.Item(txtAircraftList.Text.Trim).ID, PartNo, Description, FromDate, ToDate, rbLoanTrans.Checked, chkIsStoreToAircraft.Checked, chkIsAircraftToStore.Checked)

            If rbLoanTrans.Checked = True Then
                mStoreToAircraftList = rptStoreToAircraft.GetStoreToAircraft(New Guid(cmbStore.SelectedValue), mMachineNameValueList.Item(txtAircraftList.Text.Trim).ID, PartNo, Description, FromDate, ToDate, True, chkIsStoreToAircraft.Checked, chkIsAircraftToStore.Checked)  'Vikrant
            ElseIf rbShowPlaneTransactions.Checked = True Then
                mStoreToAircraftList = rptStoreToAircraft.GetStoreToAircraft(New Guid(cmbStore.SelectedValue), mMachineNameValueList.Item(txtAircraftList.Text.Trim).ID, PartNo, Description, FromDate, ToDate, False, chkIsStoreToAircraftPlane.Checked, chkIsAircraftToStorePlane.Checked) 'Vikrant
            End If

            objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), FromDate, ToDate, "", "", "", "", "", "", "", "", txtAircraftList.Text.Trim, "", "", "", "", PartNo, Description, "", "", IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All"), AppSettings("Logo"), "", "", "", "", "", "", "") 'Changed By Utkarsh For Report Logo.)

            If mStoreToAircraftList.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 717)
            End If
            da.Fill(ds, "ExcelrptStoreToAircraft", mStoreToAircraftList)
            da.Fill(ds, "rptSearchingCriteriaForReceipt", objsearch)
            Dim columnToRemove1 As String() = {"SerialNo", "TransTypeID", "WorkShop", "WorkOrderText", "WorkOrderNo", "Store", "CompanyName", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Supplier", "Status", "DCNo", "InvText", "InvNo", "Amend", "currencyName", "ProductVersion", "SINote", "CurrencySymbol", "ToInvDate", "FromInvDate", "SuppInvNo", "Charge", "IntOrderNo", "QuotationNo"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"ID", "Text", "No", "Store", "ToAircraftName", "ToAircraftID", "TextNo", "FromStoreID", "FromStoreName", "ToStoreID", "ToStoreName", "TransTypeID", "ItemID", "BaseTransaction", "IsLoanTransactionRequesting"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ExcelrptStoreToAircraft").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ExcelrptStoreToAircraft").Columns.Remove(columnToRemove2(i))
                End If
            Next
            If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains("FromStore") Then
                ds.Tables("rptSearchingCriteriaForReceipt").Columns("FromStore").ColumnName = "Store"
            End If
            If ds.Tables("ExcelrptStoreToAircraft").Columns.Contains("ItemName") Then
                ds.Tables("ExcelrptStoreToAircraft").Columns("ItemName").ColumnName = "Part Number Description"
            End If
            If ds.Tables("ExcelrptStoreToAircraft").Columns.Contains("TransTypeName") Then
                ds.Tables("ExcelrptStoreToAircraft").Columns("TransTypeName").ColumnName = "Transaction Type"
            End If
            If ds.Tables("ExcelrptStoreToAircraft").Columns.Contains("ExcelReceiptNumber") Then
                ds.Tables("ExcelrptStoreToAircraft").Columns("ExcelReceiptNumber").ColumnName = "Receipt Number"
            End If
            If ds.Tables("ExcelrptStoreToAircraft").Columns.Contains("ExcelIssueNumber") Then
                ds.Tables("ExcelrptStoreToAircraft").Columns("ExcelIssueNumber").ColumnName = "Issue Number"
            End If

            If ds.Tables("ExcelrptStoreToAircraft").Columns.Contains("SerialNo") Then
                ds.Tables("ExcelrptStoreToAircraft").Columns("SerialNo").ColumnName = "Serial  No."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
            dsNew.Merge(ds.Tables("ExcelrptStoreToAircraft"))

            dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptStoreToAircraft").TableName = "Store Transaction with Aircraft"
			Session("ExcelFileName") = "Store Transaction with Aircraft"
			Session("dsNew") = dsNew
			'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
			'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
			'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
			'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "StoreToAircraft", "Export To Excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
End Class