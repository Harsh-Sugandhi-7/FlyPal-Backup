Public Class wfrptPendingToReceiptsFromIssue_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mFromStoreList As StoreList
    Public mToStoreList As StoreList
    Public mtmpMachineList As tmpMachineList
    Public mVendorList As VendorList
    Public mVendor As Vendor
    Public rpt As rptIssueRegForReminder

    Public mStore As Store
    Dim FromDate As String
    Dim ToDate As String
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim FromStore As String = ""
    Dim ToStore As String = ""
    Dim Supplier As String = ""
    Dim Aircraft As String = ""
    Dim Store1 As String = ""
    Public mPendingToReceipt As Int16
    Public mWorkShopList As WorkShopList
    Dim WorkShop As String = ""
    Dim NameOfToStore As String = ""  'Added by Prashant 5-Apr-2013 'ALL05042013
    Dim NameOfFromStore As String = ""  'Added by Prashant 5-Apr-2013 'ALL05042013
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim ModuleName As String = String.Empty

#End Region

#Region " Enumeration "

    Enum PendingToReceipt
        AgainstExchangeRepairFromVendor = 1
        AgainstLoanIssueAircraft = 2
        AgainstLoanIssueToStore = 3
        AsLoanFromStore = 4
        FromStore = 5
        LoanIssueToVendor = 6
        LoanIssueToCustomer = 7
        LoanIssueToWorkShop = 8
    End Enum

#End Region

#Region " Helper Methods "

    Private Sub GetSession()
        mFromStoreList = CType(Session("mFromStoreList"), StoreList)
        mToStoreList = CType(Session("mToStoreList"), StoreList)
        mtmpMachineList = CType(Session("mtmpMachineList"), tmpMachineList)
        mVendorList = CType(Session("mVendorList"), VendorList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mPendingToReceipt = CType(Session("mPendingToReceipt"), Int16)
        mWorkShopList = CType(Session("mWorkShopList"), WorkShopList)
    End Sub

    Private Sub SetSession()
        Session("mFromStoreList") = mFromStoreList
        Session("mToStoreList") = mToStoreList
        Session("mtmpMachineList") = mtmpMachineList
        Session("mVendorList") = mVendorList
        Session("mWorkShopList") = mWorkShopList
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mFromStoreList")
        Session.Remove("mToStoreList")
        Session.Remove("mtmpMachineList")
        Session.Remove("mVendorList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mWorkShopList")
    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        Try

            If control.Enabled = False Or control.Visible = False Then Exit Sub
            Dim str As String
            str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
            ClientScript.RegisterStartupScript([GetType], "Focus Script", str)

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub ControlVisibility(index As Integer)

        Try

            If index = 6 Then

                lblFromDate.Visible = True
                lblToDate.Visible = True
                txtFromDate.Visible = True
                txtToDate.Visible = True
                txtFromDate.Enabled = True
                txtToDate.Enabled = True

            ElseIf index = 1 Or index = 2 Or index = 3 Or index = 4 Or index = 5 Then

                lblFromDate.Visible = True
                lblToDate.Visible = True
                txtFromDate.Visible = True
                txtToDate.Visible = True
                txtFromDate.Enabled = False
                txtToDate.Enabled = False

            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub SetDatePeriod(Index As Int32)

        Try

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
                Case 3 'Last 1 Quarter

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

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub ControlVisibility2()

        Try

            lblDateRangeFrom.Visible = True
            lblVendor1.Visible = True
            lblPartNo.Visible = True
            lblDesc.Visible = True
            lblFromStore1.Visible = True
            'Added By Utkarsh On 12-Jul-2012 FOR ALL12072012
            lblOrderType1.Visible = (mPendingToReceipt = 1)

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub ControlVisibility3()

        Try

            lblDateRangeFrom.Visible = False
            lblVendor1.Visible = False
            lblPartNo.Visible = False
            lblDesc.Visible = False
            'Added By Utkarsh On 12-Jul-2012 FOR ALL12072012
            lblOrderType1.Visible = False

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub SetValues()

        Try

            If cmbDateRange.SelectedIndex = 0 Then      'Date Range
                FromDate = "1-1-1900"
                ToDate = "1-1-2200"
                lblDateRangeFrom.Text = "Date Range : All"
            Else
                FromDate = txtFromDate.Text
                ToDate = txtToDate.Text
                lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
            End If

            If cmbFromStore.SelectedIndex = 0 Then       'From Store
                FromStore = ""
                NameOfFromStore = ""   'Added by Prashant 5-Apr-2013 'ALL05042013
                lblFromStore1.Text = "From Store Name : All"
            Else
                mStore = Store.GetStore(New Guid(cmbFromStore.SelectedValue))
                FromStore = mStore.Name
                NameOfFromStore = IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, "")   'Added by Prashant 5-Apr-2013 'ALL05042013
                lblFromStore1.Text = "From Store Name : " & cmbFromStore.SelectedItem.Text
            End If

            If cmbSupplier.SelectedIndex = 0 Then
                Supplier = ""
                lblVendor1.Text = "Supplier : All"
            Else
                mVendor = Vendor.GetVendor(New Guid(cmbSupplier.SelectedValue))
                Supplier = mVendor.Name
                lblVendor1.Text = "Supplier :  " & cmbSupplier.SelectedItem.Text
            End If

            Supplier = IIf((cmbType.SelectedIndex = 1 Or cmbType.SelectedIndex = 4) And cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, "")
            Aircraft = IIf(cmbType.SelectedIndex = 2 And cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

            If (cmbType.SelectedIndex = 3 And cmbStore.SelectedIndex > 0) Then
                Store1 = Store.GetStore(New Guid(cmbStore.SelectedValue.ToString)).Name
                NameOfToStore = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "")
            Else
                Store1 = ""
                NameOfToStore = ""
            End If

            WorkShop = IIf(cmbType.SelectedIndex = 5 And cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "")

            'Added By Vikrant On 28-Nov-2012 For ALL28112012
            If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
                PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
                Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
            Else
                PartNo = Trim(txtPartDescription.Text)
                Description = Trim(txtPartDescription.Text)
            End If

            Session("PartNo") = PartNo
            Session("Description") = Description
            'End

            'Added By Utkarsh On 12-Jul-2012 FOR ALL12072012
            lblOrderType1.Text = "Order Type : " & IIf(cmbOrderType.SelectedIndex = 0, "All", cmbOrderType.SelectedItem.Text)
            'End

            lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
            lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

            Select Case cmbType.SelectedValue
                Case 0
                    lblVendor.Text = "To Type : All"
                Case 1 'Supplier
                    lblVendor1.Text = "Supplier : " & IIf(Supplier <> "", Supplier, "All")
                Case 2 'Aircraft
                    lblVendor1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
                Case 3 'Store
                    lblVendor1.Text = "To Store Name : " & IIf(NameOfToStore <> "", NameOfToStore, "All")
                Case 4 'Customer
                    lblVendor1.Text = "Customer : " & IIf(Supplier <> "", Supplier, "All")
                Case 5 'WorkShop
                    lblVendor1.Text = "WorkShop : " & IIf(WorkShop <> "", WorkShop, "All")
            End Select

            mSearchCriteriaForEventLog = lblDateRangeFrom.Text + ", " + lblFromStore1.Text + "," + lblOrderType1.Text + ", " + ", " + lblVendor1.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Function CreateDataTable() As DataTable

        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim ToTypeID As Integer = 0
        Dim TransTypeID As Integer = 0

        Try

            If cmbType.SelectedIndex = 1 Or cmbType.SelectedIndex = 4 Then
                ToTypeID = 1
            ElseIf cmbType.SelectedIndex = 2 Then
                ToTypeID = 2
            ElseIf cmbType.SelectedIndex = 3 Then
                ToTypeID = 8
            ElseIf cmbType.SelectedIndex = 5 Then
                ToTypeID = 16
            End If

            If mPendingToReceipt = 1 Then
                TransTypeID = 16
            ElseIf mPendingToReceipt = 2 Then
                TransTypeID = 20
            ElseIf mPendingToReceipt = 3 Or mPendingToReceipt = 4 Then
                TransTypeID = 17
            ElseIf mPendingToReceipt = 5 Then
                TransTypeID = 15
            ElseIf mPendingToReceipt = 6 Then
                TransTypeID = 24
            ElseIf mPendingToReceipt = 7 Then
                TransTypeID = 26
            ElseIf mPendingToReceipt = 8 Then
                TransTypeID = 45
            End If

            Dim con = New SqlConnection(conString)

            con.Open()

            Dim cmd As New SqlCommand()
            cmd.Connection = con
            cmd.CommandText = "ExcelrptPendingReceiptToIssue"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@SearchTypeID", mPendingToReceipt)
            cmd.Parameters.AddWithValue("@TransTypeID", TransTypeID)
            cmd.Parameters.AddWithValue("@FromDate", FromDate)
            cmd.Parameters.AddWithValue("@ToDate", ToDate)
            cmd.Parameters.AddWithValue("@FromStoreName", FromStore)
            cmd.Parameters.AddWithValue("@ToTypeID", ToTypeID)
            cmd.Parameters.AddWithValue("@ToStoreName", Store1)
            cmd.Parameters.AddWithValue("@ToAircraftName", Aircraft)
            cmd.Parameters.AddWithValue("@ToVendorName", Supplier)
            cmd.Parameters.AddWithValue("@ItemName", PartNo)
            cmd.Parameters.AddWithValue("@Description", Description)
            cmd.Parameters.AddWithValue("@ToCustomerName", Supplier)
            cmd.Parameters.AddWithValue("@ToWorkShopName", New Guid(cmbWorkShop.SelectedValue))
            cmd.Parameters.AddWithValue("@FromStoreID", New Guid(cmbFromStore.SelectedValue))
            cmd.Parameters.AddWithValue("@ToStoreID", New Guid(cmbStore.SelectedValue))

            Dim adaptor = New SqlDataAdapter

            adaptor.SelectCommand = cmd
            adaptor.Fill(dataTable)
            con.Close()
            dataTable.Columns.Remove("Rem1")
            dataTable.Columns.Remove("Rem2")
            dataTable.Columns.Remove("Rem3")

            Return dataTable

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Function

    Private Sub GenerateXLSXFile(tbl As DataTable)

        Try

            If tbl Is Nothing OrElse tbl.Rows.Count = 0 Then

                MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "There are no records for this search criteria",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            End If

            Dim da As New ObjectAdapter
            Dim dsOrder As New dsOrder
            Dim mCompanyDetail As CompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

            Dim objSearch As rptSearchingCriteriaForReceipt = rptSearchingCriteriaForReceipt.
                                                                GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"),
                                                                                                FromDate,
                                                                                                ToDate,
                                                                                                "", "",
                                                                                                mCompanyDetail.CurrencySymbol,
                                                                                                "", "", "", "", "",
                                                                                                Aircraft,
                                                                                                Supplier,
                                                                                                NameOfToStore,
                                                                                                "", "",
                                                                                                PartNo, Description,
                                                                                                "", "",
                                                                                                NameOfFromStore,
                                                                                                "", "", "", "",
                                                                                                "", "", "", "",
                                                                                                0,
                                                                                                "", "",
                                                                                                AppSettings("Logo"))

            ' Fill dataset
            dsOrder.Clear()
            da.Fill(dsOrder, objSearch)

            ' Remove unwanted columns
            Dim columnToRemove As String() = {
                "ID", "CompanyName", "InternalReceiptNo", "ReleaseNoteNo", "RecText",
                "IssText", "OrdText", "RecNo", "OrdNo", "Status", "IssNo", "DCNo",
                "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "SerialNo",
                "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol",
                "currencyName", "ProductVersion", "SINote", "TransTypeID", "ShowLogo",
                "WorkShop", "WorkOrderText", "WorkOrderNo"
            }

            For Each col As String In columnToRemove

                If dsOrder.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(col) Then
                    dsOrder.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(col)
                End If

            Next

            ' Prepare new DataSet
            Dim dsNew As New DataSet
            dsNew.Clear()

            ' Merge criteria table
            dsNew.Merge(dsOrder.Tables("rptSearchingCriteriaForReceipt"))

            ' Ensure tbl has the correct name before merging
            If String.IsNullOrEmpty(tbl.TableName) Then
                tbl.TableName = "TMainReport"
            End If
            dsNew.Merge(tbl)

            ' Rename columns in criteria table
            Dim critTable = dsNew.Tables("rptSearchingCriteriaForReceipt")
            If critTable IsNot Nothing Then

                RenameColumnIfExists(critTable, "Store", "To Store")
                RenameColumnIfExists(critTable, "FromStore", "From Store")
                RenameColumnIfExists(critTable, "PartNo", "Part No.")
                RenameColumnIfExists(critTable, "Description", "Part Description")
                RenameColumnIfExists(critTable, "FromDate", "From Date")
                RenameColumnIfExists(critTable, "ToDate", "To Date")

            End If

            ' Rename columns in main report if present
            Dim mainTable = dsNew.Tables("TMainReport")
            If mainTable IsNot Nothing AndAlso
               mainTable.Columns.Contains("AmountInBaseCurrency") Then

                mainTable.Columns("AmountInBaseCurrency").ColumnName =
                "Amount (in " & objSearch(0).CurrencySymbol & ")"

            End If

            ' Set descriptive table name for main report
            If mainTable IsNot Nothing Then

                Select Case cmbReceiptType.SelectedValue
                    Case 1 : mainTable.TableName = "Pending Returnable Exchange/Repair Issued To Supplier"
                    Case 2 : mainTable.TableName = "Pending returnable against Loan issue to Aircraft"
                    Case 3 : mainTable.TableName = "Pending returnable against Loan issue to Store"
                    Case 4 : mainTable.TableName = "Pending to receipts as Loan taken from another Store"
                    Case 5 : mainTable.TableName = "Pending to receipts from another Store"
                    Case 6 : mainTable.TableName = "Pending returnable against Loan Issue To Supplier"
                    Case 7 : mainTable.TableName = "Pending returnable against Loan Issue To Customer"
                    Case 8 : mainTable.TableName = "Pending returnable against Loan Issue To WorkShop"
                End Select

                Session("ExcelFileName") = mainTable.TableName

            End If

            ' Store dataset in session
            Session("dsNew") = dsNew

            ' Trigger client-side open file script
            ScriptManager.RegisterStartupScript(Me, [GetType], "Open Excel", "openFile();", True)

            ' Logging
            Dim ModuleName As String = ""
            Select Case mPendingToReceipt
                Case 1 : ModuleName = "PendingRCIFromVendor"
                Case 2 : ModuleName = "PendingRCIFromAircraftForLoanReturn"
                Case 3 : ModuleName = "PendingRCIFromStoreForLoanReturn"
                Case 4 : ModuleName = "PendingRCIFromStoreForLoan"
                Case 5 : ModuleName = "PendingRCIFromStore"
                Case 6 : ModuleName = "PendingRCIFromSupplierForLoanReturn"
                Case 7 : ModuleName = "PendingRCIFromCustomerForLoanReturn"
                Case 8 : ModuleName = "PendingRCIFromWorkShopForLoanReturn"
            End Select

            MarkLog(Action.Print,
                    ModuleName,
                    "Export To Excel " & mSearchCriteriaForEventLog,
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

        Catch ex As Exception

            MSGBoxCtrl.Show("Error",
                            "An error occurred while generating the Excel file.",
                            ex.Message,
                            MsgBoxStyle.Critical,
                            "")

        End Try

    End Sub

    ' Helper method for safe column renaming
    Private Sub RenameColumnIfExists(table As DataTable, oldName As String, newName As String)

        Try

            If table.Columns.Contains(oldName) Then
                table.Columns(oldName).ColumnName = newName
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub CallFindNowReport()

        Try

            If cmbType.SelectedIndex = 1 Then 'Supplier
                FindNowReport("", "", FromDate, ToDate, Store1, Supplier, Aircraft, 1, 0, FromStore, "", PartNo, Description, WorkShop, FromStoreID:=cmbFromStore.SelectedValue)
            End If

            'Aircraft
            If cmbType.SelectedIndex = 2 Then 'Aircraft
                FindNowReport("", "", FromDate, ToDate, Store1, Supplier, Aircraft, 2, 0, FromStore, "", PartNo, Description, WorkShop, FromStoreID:=cmbFromStore.SelectedValue, ToStoreID:=cmbStore.SelectedValue)
            End If

            'Store
            If cmbType.SelectedIndex = 3 Then 'Store
                FindNowReport("", "", FromDate, ToDate, Store1, Supplier, Aircraft, 8, 0, FromStore, "", PartNo, Description, WorkShop, FromStoreID:=cmbFromStore.SelectedValue, ToStoreID:=cmbStore.SelectedValue)
            End If

            If cmbType.SelectedIndex = 4 Then 'Customer
                FindNowReport("", "", FromDate, ToDate, Store1, Supplier, Aircraft, 1, 0, FromStore, "", PartNo, Description, WorkShop, FromStoreID:=cmbFromStore.SelectedValue, ToStoreID:=cmbStore.SelectedValue)
            End If

            If cmbType.SelectedIndex = 5 Then 'WorkShop
                FindNowReport("", "", FromDate, ToDate, Store1, Supplier, Aircraft, 16, 0, FromStore, "", PartNo, Description, cmbWorkShop.SelectedValue.ToString, FromStoreID:=cmbFromStore.SelectedValue, ToStoreID:=cmbStore.SelectedValue)
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub FindNowReport(Optional Text As String = "",
                              Optional No As String = "",
                              Optional FromDate As String = "1-1-1800",
                              Optional ToDate As String = "1-1-3050",
                              Optional ToStoreName As String = "",
                              Optional ToVendorName As String = "",
                              Optional ToAircraftName As String = "",
                              Optional ToTypeID As Integer = 0,
                              Optional StatusID As Integer = 0,
                              Optional FromStoreName As String = "",
                              Optional SerialNo As String = "",
                              Optional ItemName As String = "",
                              Optional Description As String = "",
                              Optional ToWorkShopName As String = "",
                              Optional FromStoreID As String = "{00000000-0000-0000-0000-000000000000}",
                              Optional ToStoreID As String = "{00000000-0000-0000-0000-000000000000}")

        Try

            Select Case mPendingToReceipt
                Case PendingToReceipt.AgainstExchangeRepairFromVendor
                    rpt = rptIssueRegForReminder.GetPendingToReceiptAgainstExchangeRepairFromVendor(FromDate, ToDate, FromStoreName, ToVendorName, ItemName, Description, cmbOrderType.SelectedValue, FromStoreID:=FromStoreID) 'Changed by Utkash ON 12-Jul-2012 FOR ALL12072012
                Case PendingToReceipt.AgainstLoanIssueAircraft
                    rpt = rptIssueRegForReminder.GetPendingToReceiptAgainstLoanIssueAircraft(FromDate, ToDate, FromStoreName, ToAircraftName, ItemName, Description, FromStoreID:=FromStoreID)
                Case PendingToReceipt.AgainstLoanIssueToStore
                    rpt = rptIssueRegForReminder.GetPendingToReceiptAgainstLoanIssueToStore(FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, FromStoreID:=FromStoreID, ToStoreID:=ToStoreID)
                Case PendingToReceipt.AsLoanFromStore
                    rpt = rptIssueRegForReminder.GetPendingToReceiptAsLoanFromStore(FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, FromStoreID:=FromStoreID, ToStoreID:=ToStoreID)
                Case PendingToReceipt.FromStore
                    rpt = rptIssueRegForReminder.GetPendingToReceiptFromStore(FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, FromStoreID:=FromStoreID, ToStoreID:=ToStoreID)
                Case PendingToReceipt.LoanIssueToVendor
                    rpt = rptIssueRegForReminder.GetPendingToReceiptAgainstLoanIssueToVendor(FromDate, ToDate, FromStoreName, ToVendorName, ItemName, Description, FromStoreID:=FromStoreID)
                Case PendingToReceipt.LoanIssueToCustomer
                    rpt = rptIssueRegForReminder.GetPendingToReceiptAgainstLoanIssueToCustomer(FromDate, ToDate, FromStoreName, ToVendorName, ItemName, Description, FromStoreID:=FromStoreID)
                Case PendingToReceipt.LoanIssueToWorkShop
                    rpt = rptIssueRegForReminder.GetPendingToReceiptAgainstLoanIssueToWorkShop(FromDate, ToDate, FromStoreName, ToWorkShopName, ItemName, Description, FromStoreID:=FromStoreID)
            End Select

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub SetReport()

        Try

            Dim da As New ObjectAdapter
            Dim myReport As Engine.ReportClass
            Dim objsearch As rptSearchingCriteriaForReceipt
            Dim mCompanyDetail As New CompanyDetail
            Dim ds As New dsIssue

            SetValues()

            If mPendingToReceipt = PendingToReceipt.AgainstExchangeRepairFromVendor Then
                myReport = New crptPendingToReceiptIssuesForExchangeRepair
            Else

                If mPendingToReceipt = 5 Then

                    If cmbFormat.SelectedIndex = 0 Then
                        myReport = New crptPendingToReceiptIssues
                    Else
                        myReport = New crptTransitToReceiptIssues
                    End If

                Else
                    myReport = New crptPendingToReceiptIssues
                End If

            End If

            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            CallFindNowReport()
            objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"),
                                                                                      FromDate,
                                                                                      ToDate,
                                                                                      "", "",
                                                                                      mCompanyDetail.CurrencySymbol, "", "", "", "", "",
                                                                                      Aircraft,
                                                                                      Supplier,
                                                                                      NameOfToStore, "", "",
                                                                                      PartNo,
                                                                                      Description, "", "",
                                                                                      NameOfFromStore, "", "", "", "", "", "", "", "",
                                                                                      0, "", "",
                                                                                      AppSettings("Logo"))

            If rpt.Count <= 0 Then

                MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "There is no record for this search criteria",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            Else

                Select Case mPendingToReceipt
                    Case 1
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 507)
                        ModuleName = "PendingRCIFromVendor"
                    Case 2
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 506)
                        ModuleName = "PendingRCIFromAircraftForLoanReturn"
                    Case 3
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 505)
                        ModuleName = "PendingRCIFromStoreForLoanReturn"
                    Case 4
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 504)
                        ModuleName = "PendingRCIFromStoreForLoan"
                    Case 5
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 503)
                        ModuleName = "PendingRCIFromStore"
                    Case 6
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1116)
                        ModuleName = "PendingRCIFromSupplierForLoanReturn"
                    Case 7
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1117)
                        ModuleName = "PendingRCIFromCustomerForLoanReturn"
                    Case 8
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1118)
                        ModuleName = "PendingRCIFromWorkShopForLoanReturn"
                End Select

            End If

            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            da.Fill(ds, mrptImage)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "openTranDetail",
                                                Str,
                                                True)

            MarkLog(Action.Print,
                    ModuleName,
                    mSearchCriteriaForEventLog,
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    'DataFieldBind()
            End Select
        End If
    End Sub

    Private Sub SetTitle()
        Select Case mPendingToReceipt
            Case PendingToReceipt.AgainstExchangeRepairFromVendor
                lblVendor.Text = "Supplier"
                cmbSupplier.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 1
            Case PendingToReceipt.AgainstLoanIssueAircraft
                lblVendor.Text = "Aircraft"
                cmbAircraft.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 2
            Case PendingToReceipt.AgainstLoanIssueToStore
                lblVendor.Text = "Store"
                cmbAircraft.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 3
            Case PendingToReceipt.AsLoanFromStore
                lblVendor.Text = "Store"
                cmbAircraft.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 3
            Case PendingToReceipt.FromStore
                lblVendor.Text = "Store"
                cmbAircraft.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 3
            Case PendingToReceipt.LoanIssueToVendor
                lblVendor.Text = "Supplier"
                cmbSupplier.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 1
            Case PendingToReceipt.LoanIssueToCustomer
                lblVendor.Text = "Customer"
                cmbSupplier.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 4
            Case PendingToReceipt.LoanIssueToWorkShop
                lblVendor.Text = "WorkShop"
                'cmbWorkShop.SelectedIndex = 0
                cmbType.Enabled = False
                cmbType.SelectedIndex = 5
        End Select
        cmbFromStore.SelectedIndex = 0
        cmbStore.SelectedIndex = 0
        cmbSupplier.SelectedIndex = 0
        cmbAircraft.SelectedIndex = 0
        'cmbWorkShop.SelectedIndex = 0
        Dim Index As Int16 = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedIndex, 0)
        lblVendor.Visible = (Index > 0)
        lblVendor.Text = IIf(Index = 0, "", IIf(Index = 1, "Supplier  ", IIf(Index = 2, "Aircraft  ", IIf(Index = 3, "Store  ", IIf(Index = 4, "Customer", IIf(Index = 5, "WorkShop", ""))))))
        cmbSupplier.Visible = (Index = 1 Or Index = 4)
        cmbAircraft.Visible = (Index = 2)
        cmbStore.Visible = (Index = 3)
        cmbWorkShop.Visible = (Index = 5)
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        Try

            'From Store
            mFromStoreList = StoreList.GetStoreList(0, "", "(ALL)", True)
            cmbFromStore.DataSource = mFromStoreList
            Session("mFromStoreList") = mFromStoreList

            'To Store
            mToStoreList = StoreList.GetStoreList(0, "", "(ALL)", True)
            cmbStore.DataSource = mToStoreList
            Session("mToStoreList") = mToStoreList
            lblStoreCount.Text = "You have " + (mToStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mToStoreList.TotalStorelistCount.ToString + " Store(s)"

            mtmpMachineList = tmpMachineList.GetAircraftList(Today.Date.ToShortDateString, True, "(ALL)")
            cmbAircraft.DataSource = mtmpMachineList
            Session("mtmpMachineList") = mtmpMachineList

            'Customer / Supplier
            If mPendingToReceipt = 7 Then
                mVendorList = VendorList.GetVendorstList(0, , , , , , "(ALL)", True)
                cmbSupplier.DataSource = mVendorList
                Session("mVendorList") = mVendorList
            Else
                mVendorList = VendorList.GetVendorstList(0, , , , , , "(ALL)", False, True)
                cmbSupplier.DataSource = mVendorList
                Session("mVendorList") = mVendorList
            End If

            mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(ALL)")
            cmbWorkShop.DataSource = mWorkShopList
            Session("mWorkShopList") = mWorkShopList

            DataBind()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)

            If Not IsPostBack And CType(Session("Sender"), String) = "" Then

                RemoveSession()
                mPendingToReceipt = 5 'Request.QueryString("PendingToReceipt")
                Session("mPendingToReceipt") = mPendingToReceipt

                If cmbDateRange.Enabled = True Then
                    SetFocus(cmbDateRange)
                End If

                Me.cmbReceiptType.Items.Clear()
                If User.IsInRole("PendingRCIFromStoreView") Then cmbReceiptType.Items.Add(New ListItem("Pending to receipts from other Store", "5"))
                If User.IsInRole("PendingRCIFromStoreForLoanView") Then cmbReceiptType.Items.Add(New ListItem("Pending to receipts as loan taken from other Store", "4"))
                If User.IsInRole("PendingRCIFromStoreForLoanReturnView") Then cmbReceiptType.Items.Add(New ListItem("Pending returnable against loan issued to Store", "3"))
                If User.IsInRole("PendingRCIFromAircraftForLoanReturnView") Then cmbReceiptType.Items.Add(New ListItem("Pending returnable against loan issued to Aircraft", "2"))
                If User.IsInRole("PendingRCIFromVendorView") Then cmbReceiptType.Items.Add(New ListItem("Pending returnable Exchange/Repair issued to Supplier", "1"))
                If User.IsInRole("PendingreturnableagainstloanissuedtoSupplierView") Then cmbReceiptType.Items.Add(New ListItem("Pending returnable against loan issued to Supplier", "6"))
                If User.IsInRole("PendingreturnableagainstloanissuedtoCustomerView") Then cmbReceiptType.Items.Add(New ListItem("Pending returnable against loan issued to Customer", "7"))
                If User.IsInRole("PendingreturnableagainstloanissuedtoWorkShopView") Then cmbReceiptType.Items.Add(New ListItem("Pending returnable against loan issued to WorkShop", "8"))

                DataFieldBind()
                ControlVisibility(6)
                SetDatePeriod(6)
                cmbDateRange.SelectedIndex = 6
                SetTitle()

            End If

            If mPendingToReceipt = 5 Then

                lblStep.Text = "Step IV. Selection of Store"
                btnClose.ToolTip = " Click to close the Pending To Receipts From Other Store screen"
                cmbFormat.Visible = True
                lblFormat.Visible = True
                lblFormatSelection.Visible = True
                lblStep3.Text = "Step V. Selection of Part Number/Description"
                lblFormatSelection.Text = "Step VI. Format Selection"
                lblStep4.Text = "Step VII. Display Report"

            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub DateRangeChanged(sender As Object, e As EventArgs) Handles cmbDateRange.SelectedIndexChanged

        Try

            Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
            ControlVisibility(Index)
            SetDatePeriod(Index)

            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub DisplayCurrentSearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click

        Try

            SetValues()
            ControlVisibility2()
            upnlSelection.Update()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

        Try

            If Page.IsValid Then
                SetReport()
            Else
                upnlValidationsummary.Update()
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub ExportToExcel(sender As Object, e As EventArgs) Handles btnExport.Click

        Try

            If IsValid() Then
                SetValues()
                GenerateXLSXFile(CreateDataTable())
            Else
                upnlValidationsummary.Update()
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            RemoveSession()
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub ReceiptTypeChanged(sender As Object, e As EventArgs) Handles cmbReceiptType.SelectedIndexChanged

        Try

            mPendingToReceipt = cmbReceiptType.SelectedValue
            Session("mPendingToReceipt") = mPendingToReceipt

            If mPendingToReceipt = 5 Then
                lblStep.Text = "Step IV. Selection of Store"
                btnClose.ToolTip = " Click to close the Pending To Receipts From Other Store screen"
                lblStep3.Text = "Step V. Selection of Part Number/Description"
                lblFormatSelection.Text = "Step VI. Format Selection"
            ElseIf mPendingToReceipt = 4 Then
                lblStep.Text = "Step IV. Selection of Store"
                btnClose.ToolTip = " Click to close the Pending To Receipts as Loan Taken From Other Store screen"
            ElseIf mPendingToReceipt = 3 Then
                lblStep.Text = "Step IV. Selection of Store"
                btnClose.ToolTip = " Click to close the Pending Returnable against Loan Issued To Store screen"
            ElseIf mPendingToReceipt = 2 Then
                lblStep.Text = "Step IV. Selection of Aircraft"
                btnClose.ToolTip = " Click to close the Pending Returnable against Loan Issued To Aircraft screen"
            ElseIf mPendingToReceipt = 1 Then
                lblStep.Text = "Step IV. Selection of Supplier"
                btnClose.ToolTip = " Click to close the Pending Returnable Exchange/Repair issued To Supplier screen"
                lblStep3.Text = "Step VI. Selection of Part Number/Description"
            ElseIf mPendingToReceipt = 6 Then
                lblStep.Text = "Step IV. Selection of Supplier"
                btnClose.ToolTip = " Click to close the Pending Returnable against Loan Issued To Supplier screen"
            ElseIf mPendingToReceipt = 7 Then
                lblStep.Text = "Step IV. Selection of Customer"
                btnClose.ToolTip = " Click to close the Pending Returnable against Loan Issued To Customer screen"
            ElseIf mPendingToReceipt = 8 Then
                lblStep.Text = "Step IV. Selection of WorkShop"
                btnClose.ToolTip = " Click to close the Pending Returnable against Loan Issued To WorkShop screen"
            End If

            If mPendingToReceipt = 5 Or mPendingToReceipt = 1 Then
                lblStep4.Text = "Step VII. Display Report"
            Else
                lblStep4.Text = "Step VI. Display Report"
            End If

            cmbFormat.Visible = (mPendingToReceipt = 5)
            lblFormat.Visible = (mPendingToReceipt = 5)
            lblFormatSelection.Visible = (mPendingToReceipt = 5)
            lblOrdertype.Visible = (mPendingToReceipt = 1)
            cmbOrderType.Visible = (mPendingToReceipt = 1)
            lblOrder.Visible = (mPendingToReceipt = 1)
            upnlOrdertype.Update()
            SetTitle()

            upnlType.Update()
            upnltitle.Update()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

End Class