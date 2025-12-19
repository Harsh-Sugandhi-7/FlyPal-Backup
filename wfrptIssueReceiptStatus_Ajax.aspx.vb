Public Class wfrptIssueReceiptStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mFromStoreList As StoreList
    Public mToStoreList As StoreList
    Public mtmpMachineList As tmpMachineList
    Public mVendorList As VendorList
    Public rpt As rptIssueRegForReminder

    Public mStore As Store
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim FromStore As String = ""
    Dim ToStore As String = ""
    Dim Supplier As String = ""
    Dim Aircraft As String = ""
    Dim Store1 As String = ""
    Public mWorkShopList As WorkShopList
    Dim WorkShop As String = ""
    Dim NameOfToStore As String = ""  'Added by Prashant 5-Apr-2013 'ALL05042013
    Dim NameOfFromStore As String = ""  'Added by Prashant 5-Apr-2013 'ALL05042013
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Public mrptIssueReceiptStatus As rptIssueReceiptStatus
    Public mrptSuppCustReceiptIssueLoanStatus As rptSuppCustReceiptIssueLoanStatus
    Dim ReportName As String = String.Empty

    'Added by Abhishek on 18-SEP-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteriaForReceipt
    Dim mCompanyDetail As New CompanyDetail
    Dim ds As New dsIssueReceiptStatus
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility2()
        lblVendor1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblFromStore1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblVendor1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub SetValues()
        If cmbFromStore.SelectedIndex = 0 Then       'From Store
            FromStore = ""
            NameOfFromStore = ""
            lblFromStore1.Text = "From Store Name : All"
        Else
            mStore = Store.GetStore(New Guid(cmbFromStore.SelectedValue))
            FromStore = mStore.Name
            NameOfFromStore = IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, "")
            lblFromStore1.Text = "From Store Name : " & cmbFromStore.SelectedItem.Text
        End If
        Supplier = IIf((cmbSearchType.SelectedValue = 3 Or cmbSearchType.SelectedValue = 4) And cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, "")
        Aircraft = IIf(cmbSearchType.SelectedValue = 2 And cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If (cmbSearchType.SelectedValue = 1 And cmbStore.SelectedIndex > 0) Then
            Store1 = Store.GetStore(New Guid(cmbStore.SelectedValue.ToString)).Name
            NameOfToStore = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "")
        Else
            Store1 = ""
            NameOfToStore = ""
        End If
        WorkShop = IIf(cmbSearchType.SelectedValue = 5 And cmbWorkShop.SelectedIndex > 0, cmbWorkShop.SelectedItem.Text, "")
        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        Select Case cmbSearchType.SelectedValue
            Case 1 'Store
                lblVendor1.Text = "To Store Name : " & IIf(NameOfToStore <> "", NameOfToStore, "All")
            Case 2 'Aircraft
                lblVendor1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
            Case 3 'Supplier
                lblVendor1.Text = "Supplier : " & IIf(Supplier <> "", Supplier, "All")
            Case 4 'Customer
                lblVendor1.Text = "Customer : " & IIf(Supplier <> "", Supplier, "All")
            Case 5 'WorkShop
                lblVendor1.Text = "WorkShop : " & IIf(WorkShop <> "", WorkShop, "All")
        End Select
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForReceipt
        Dim mCompanyDetail As New CompanyDetail
        Dim mCount As Integer
        SetValues()
        Dim ds As New dsIssueReceiptStatus
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        callFindNowReport1()
        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1-1-1900", "1-1-3300", ReportName, "", mCompanyDetail.CurrencySymbol, "", "", "", "", "", Aircraft, Supplier, NameOfToStore, "", "", PartNo, Description, "", "", NameOfFromStore, "", "", "", "", "", "", "", "", 0, WorkShop, "", AppSettings("Logo"))
        If cmbGivenTaken.SelectedValue = 2 Then
            myReport = New crptSuppCustReceiptIssueLoanStatus
            mCount = mrptSuppCustReceiptIssueLoanStatus.Count
        Else
            myReport = New crptIssueReceiptStatus
            mCount = mrptIssueReceiptStatus.Count
        End If
        If mCount <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1305)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        If cmbGivenTaken.SelectedValue = 2 Then
            da.Fill(ds, mrptSuppCustReceiptIssueLoanStatus)
        Else
            da.Fill(ds, mrptIssueReceiptStatus)
        End If
        da.Fill(ds, objsearch)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "IssueReceiptLoanStatus", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'From Store
        mFromStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbFromStore.DataSource = mFromStoreList
        Session("mFromStoreList") = mFromStoreList

        'To Store
        mToStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mToStoreList
        Session("mToStoreList") = mToStoreList
        lblStoreCount.Text = "You have " + (mToStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mToStoreList.TotalStorelistCount.ToString + " Store(s)"


        mtmpMachineList = tmpMachineList.GetAircraftList(Today.Date.ToShortDateString, True, "(All)")
        cmbAircraft.DataSource = mtmpMachineList
        Session("mtmpMachineList") = mtmpMachineList

        mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", True)
        cmbSupplier.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(All)")
        cmbWorkShop.DataSource = mWorkShopList
        Session("mWorkShopList") = mWorkShopList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            RemoveSession()
            DataFieldBind()
            cmbSearchType_SelectedIndexChanged(sender, e)
        End If
    End Sub
    Protected Sub cmbSearchType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSearchType.SelectedIndexChanged
        Dim SelectedValue As Integer = cmbSearchType.SelectedValue
        cmbGivenTaken.SelectedValue = 1
        Select Case SelectedValue
            Case 1 'Store
                lbltitle.Text = "Stores Loan Transactions"
                lblStep.Text = "Step III. Selection of Store"
                lblVendor.Text = "Store"
             Case 2 'Aircraft
                lbltitle.Text = "Aircraft Loan Transactions"
                lblStep.Text = "Step III. Selection of Aircraft"
                lblVendor.Text = "Aircraft"
                cmbAircraft.SelectedIndex = 0
             Case 3 'Supplier
                lbltitle.Text = "Supplier Loan Transactions"
                lblStep.Text = "Step III. Selection of Supplier"
                lblVendor.Text = "Supplier"
                mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", False, True)

                cmbSupplier.DataSource = mVendorList
                Session("mVendorList") = mVendorList
                cmbSupplier.SelectedIndex = 0
                cmbSupplier.DataBind()
            Case 4 'Customer
                lbltitle.Text = "Customer Loan Transactions"
                lblStep.Text = "Step III. Selection of Customer"
                lblVendor.Text = "Customer"
                mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", True)
                cmbSupplier.DataSource = mVendorList
                Session("mVendorList") = mVendorList
                cmbSupplier.SelectedIndex = 0
                cmbSupplier.DataBind()
                cmbSupplier.Visible = True
            Case 5 'Work Shop
                lbltitle.Text = "WorkShop Loan Transactions"
                lblStep.Text = "Step III. Selection ofWorkShop"
                lblVendor.Text = "WorkShop"
        End Select
        cmbStore.Visible = IIf(cmbSearchType.SelectedValue = 1, True, False)
        cmbAircraft.Visible = IIf(cmbSearchType.SelectedValue = 2, True, False)
        cmbSupplier.Visible = IIf(cmbSearchType.SelectedValue = 3 Or cmbSearchType.SelectedValue = 4, True, False)
        cmbWorkShop.Visible = IIf(cmbSearchType.SelectedValue = 5, True, False)
        cmbGivenTaken.Enabled = IIf(cmbSearchType.SelectedValue = 3 Or cmbSearchType.SelectedValue = 4, True, False)
        upnlType.Update()
        upnlHeader.Update()
        upnlLabel.Update()
    End Sub
  Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
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

#Region " Issue Receipt Status "
    Private Sub callFindNowReport1()
        If cmbSearchType.SelectedValue = 1 Then 'Store
            FindNowReport1("", "", "1-1-1900", "1-1-3300", Store1, cmbSupplier.SelectedValue.ToString, cmbAircraft.SelectedValue.ToString, 1, 0, FromStore, "", PartNo, Description, cmbWorkShop.SelectedValue.ToString)
            ReportName = "Store Loan Transactions"
        End If
        If cmbSearchType.SelectedValue = 2 Then 'Aircraft
            FindNowReport1("", "", "1-1-1900", "1-1-3300", Store1, cmbSupplier.SelectedValue.ToString, cmbAircraft.SelectedValue.ToString, 2, 0, FromStore, "", PartNo, Description, cmbWorkShop.SelectedValue.ToString)
            ReportName = "Aircraft Loan Transactions"
        End If
        If cmbSearchType.SelectedValue = 3 Then 'Supplier
            FindNowReport1("", "", "1-1-1900", "1-1-3300", Store1, cmbSupplier.SelectedValue.ToString, cmbAircraft.SelectedValue.ToString, 8, 0, FromStore, "", PartNo, Description, cmbWorkShop.SelectedValue.ToString)
            ReportName = "Supplier Loan Transactions"
        End If
        If cmbSearchType.SelectedValue = 4 Then 'Customer
            FindNowReport1("", "", "1-1-1900", "1-1-3300", Store1, cmbSupplier.SelectedValue.ToString, cmbAircraft.SelectedValue.ToString, 1, 0, FromStore, "", PartNo, Description, cmbWorkShop.SelectedValue.ToString)
            ReportName = "Customer Loan Transactions"
        End If
        If cmbSearchType.SelectedValue = 5 Then 'WorkShop
            FindNowReport1("", "", "1-1-1900", "1-1-3300", Store1, cmbSupplier.SelectedValue.ToString, cmbAircraft.SelectedValue.ToString, 16, 0, FromStore, "", PartNo, Description, cmbWorkShop.SelectedValue.ToString)
            ReportName = "WorkShop Loan Transactions"
        End If
    End Sub
    Private Sub FindNowReport1(Optional ByVal Text As String = "", Optional ByVal No As String = "", Optional ByVal FromDate As String = "1-1-1800", Optional ByVal ToDate As String = "1-1-3050", Optional ByVal ToStoreName As String = "", Optional ByVal ToVendorName As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal ToAircraftName As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal ToTypeID As Integer = 0, Optional ByVal StatusID As Integer = 0, Optional ByVal FromStoreName As String = "", Optional ByVal SerialNo As String = "", Optional ByVal ItemName As String = "", Optional ByVal Description As String = "", Optional ByVal ToWorkShopName As String = "{00000000-0000-0000-0000-000000000000}")
        Select Case cmbSearchType.SelectedValue
            Case 1 'Store
                mrptIssueReceiptStatus = rptIssueReceiptStatus.GetIssueReceiptStatus(cmbSearchType.SelectedValue, FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description)
            Case 2 'Aircraft
                mrptIssueReceiptStatus = rptIssueReceiptStatus.GetIssueReceiptStatus(cmbSearchType.SelectedValue, FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, , ToAircraftName)
            Case 3 'Supplier
                If cmbGivenTaken.SelectedValue = 2 Then
                    mrptSuppCustReceiptIssueLoanStatus = rptSuppCustReceiptIssueLoanStatus.GetSuppCustReceiptIssueLoanStatus(cmbSearchType.SelectedValue, FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, ToVendorName)
                Else
                    mrptIssueReceiptStatus = rptIssueReceiptStatus.GetIssueReceiptStatus(cmbSearchType.SelectedValue, FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, ToVendorName)
                End If
            Case 4 'Customer
                If cmbGivenTaken.SelectedValue = 2 Then
                    mrptSuppCustReceiptIssueLoanStatus = rptSuppCustReceiptIssueLoanStatus.GetSuppCustReceiptIssueLoanStatus(cmbSearchType.SelectedValue, FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, ToVendorName)
                Else
                    mrptIssueReceiptStatus = rptIssueReceiptStatus.GetIssueReceiptStatus(cmbSearchType.SelectedValue, FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, , , ToVendorName)
                End If
            Case 5 'Workshop
                mrptIssueReceiptStatus = rptIssueReceiptStatus.GetIssueReceiptStatus(cmbSearchType.SelectedValue, FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description, , , , ToWorkShopName)
        End Select
    End Sub
#End Region

    'Added by Abhishek on 18-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim mCount As Integer
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            callFindNowReport1()
            objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "1-1-1900", "1-1-3300", ReportName, "", mCompanyDetail.CurrencySymbol, "", "", "", "", "", Aircraft, Supplier, NameOfToStore, "", "", PartNo, Description, "", "", NameOfFromStore, "", "", "", "", "", "", "", "", 0, WorkShop, "", AppSettings("Logo"))
            If cmbGivenTaken.SelectedValue = 2 Then
                myReport = New crptSuppCustReceiptIssueLoanStatus
                mCount = mrptSuppCustReceiptIssueLoanStatus.Count
                ds.Clear()
                da.Fill(ds, objsearch)
                da.Fill(ds, "ExcelrptSuppCustReceiptIssueLoanStatus", mrptSuppCustReceiptIssueLoanStatus)

                Dim columnToRemove1 As String() = {"IssueID", "IssueDate", "IssueText", "IssueNo", "IssueToName", "ToTypeID", "ToVendorName", "ToAircraftName", "Remark", "Person", "CreatedBy", "AuthorizedBy", "IssueChieldId", "SrNo", "ReleaseNoteNo", "ReleaseNoteDate", "Qty", "Returnable", "Note", "ReceiptBalanceQty", "InvoiceBalanceQty", "TermName", "GroupBy", "Heading", "Status", "SubHeading", "SearchType", "EROOrderNumber", "EROOrderQty", "ToWorkShopName", "EffRate", "Total", "OrderType", "ItemID", "LoanQty", "FromStoreID", "ToStoreID", "ToVendorID", "ToAircraftID", "TotalEROReceivedQty", "ToWorkShopID", "TransTypeID", "IsOverHaul", "LoanReturningCount", "LoanGiven", "LoanRecovery"}
                For i As Integer = 0 To columnToRemove1.Length - 1
                    If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains(columnToRemove1(i)) Then
                        ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Remove(columnToRemove1(i))
                    End If
                Next

                If cmbSearchType.SelectedItem.ToString = "Store" Then
                    Dim columnToRemove2 As String() = {"WorkShop", "Supplier", "Aircraft", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                        End If
                    Next
                ElseIf cmbSearchType.SelectedItem.ToString = "Aircraft" Then
                    Dim columnToRemove3 As String() = {"WorkShop", "Supplier", "Store", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove3.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove3(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove3(i))
                        End If
                    Next
                ElseIf cmbSearchType.SelectedItem.ToString = "Supplier" Then
                    Dim columnToRemove4 As String() = {"WorkShop", "Store", "Aircraft", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove4.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove4(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove4(i))
                        End If
                    Next
                ElseIf cmbSearchType.SelectedItem.ToString = "Customer" Then
                    Dim columnToRemove5 As String() = {"WorkShop", "Aircraft", "Store", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove5.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove5(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove5(i))
                        End If
                    Next
                Else
                    Dim columnToRemove6 As String() = {"Store", "Supplier", "Aircraft", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove6.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove6(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove6(i))
                        End If
                    Next
                End If
                If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains("Supplier") Then
                    ds.Tables("rptSearchingCriteriaForReceipt").Columns("Supplier").ColumnName = "Customer"
                End If

                'Dim columnToRemove2 As String() = {"CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "Store", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkShop", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                'For i As Integer = 0 To columnToRemove2.Length - 1
                '    If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                '        ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                '    End If
                'Next




                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("PartName") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("PartName").ColumnName = "Part Number"
                End If

                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("Description") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("Description").ColumnName = "Description"
                End If

                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("SerialNo") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("SerialNo").ColumnName = "Serial No."
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("ToStoreName") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("ToStoreName").ColumnName = "Loan Taken By"
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("FromStoreName") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("FromStoreName").ColumnName = "Return From Store"
                End If
       
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanTaken") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("LoanTaken").ColumnName = "Loan Taken"
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanReturn") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("LoanReturn").ColumnName = "Loan Returning"
                End If
           
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
                dsNew.Merge(ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus"))

                dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
                dsNew.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").TableName = "Loan Transcations "
				Session("ExcelFileName") = "Loan Transcations "
				Session("dsNew") = dsNew
				Session("DataTableToBeFormattedForExportToExcel") = "Loan Transcations"
                'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
                'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
                'da.Fill(ds, "rptSuppCustReceiptIssueLoanStatus", mrptSuppCustReceiptIssueLoanStatus)

                'Dim columnToRemove1 As String() = {"IssueID", "IssueDate", "IssueText", "IssueNo", "IssueToName", "ToTypeID", "ToVendorName", "ToAircraftName", "Remark", "Person", "CreatedBy", "AuthorizedBy", "IssueChieldId", "SrNo", "ReleaseNoteNo", "ReleaseNoteDate", "Qty", "Returnable", "Note", "ReceiptBalanceQty", "InvoiceBalanceQty", "TermName", "GroupBy", "Heading", "Status", "SubHeading", "SearchType", "EROOrderNumber", "EROOrderQty", "ToWorkShopName", "EffRate", "Total", "OrderType", "ItemID", "LoanQty"}
                'For i As Integer = 0 To columnToRemove1.Length - 1
                '    If ds.Tables("Excelrptissuesuppcustreceiptloanstatus").Columns.Contains(columnToRemove1(i)) Then
                '        ds.Tables("Excelrptissuesuppcustreceiptloanstatus").Columns.Remove(columnToRemove1(i))
                '    End If
                'Next




                'Dim columnToRemove2 As String() = {"CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "Supplier", "Store", "Status", "DCNo", "InvText", "InvNo", "FromStore", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkShop", "WorkOrderText", "WorkOrderNo"}
                'For i As Integer = 0 To columnToRemove2.Length - 1
                '    If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                '        ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                '    End If
                'Next




                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("PartName") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("PartName").ColumnName = "Part Number"
                'End If

                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("Description") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("Description").ColumnName = "Description"
                'End If

                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("SerialNo") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("SerialNo").ColumnName = "Serial No."
                'End If
                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("FromStoreName") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("FromStoreName").ColumnName = "From Store"
                'End If
                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("ToStoreName") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("ToStoreName").ColumnName = "To"
                'End If
                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanGiven") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("LoanGiven").ColumnName = "Load Given"
                'End If
                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanTaken") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("LoanTaken").ColumnName = "Loan Taken"
                'End If
                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanReturn") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("LoanReturn").ColumnName = "Loan Returning"
                'End If
                'If ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanRecovery") Then
                '    ds.Tables("rptSuppCustReceiptIssueLoanStatus").Columns("LoanRecovery").ColumnName = "Loan Recovery"
                'End If
                'Dim dsNew As New DataSet
                'dsNew.Clear()

                'dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
                'dsNew.Merge(ds.Tables("rptSuppCustReceiptIssueLoanStatus"))

                'dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
                'dsNew.Tables("rptSuppCustReceiptIssueLoanStatus").TableName = "Loan Transcations "

                'Session("dsNew") = dsNew
                'Session("DataTableToBeFormattedForExportToExcel") = "Loan Transcations"
                ''PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
                ''Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                ''Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

            Else
                myReport = New crptIssueReceiptStatus
                mCount = mrptIssueReceiptStatus.Count
                ds.Clear()
                da.Fill(ds, objsearch)
                da.Fill(ds, "ExcelrptSuppCustReceiptIssueLoanStatus", mrptIssueReceiptStatus)

                Dim columnToRemove1 As String() = {"IssueID", "IssueDate", "IssueText", "IssueNo", "IssueToName", "ToTypeID", "ToVendorName", "ToAircraftName", "Remark", "Person", "CreatedBy", "AuthorizedBy", "IssueChieldId", "SrNo", "ReleaseNoteNo", "ReleaseNoteDate", "Qty", "Returnable", "Note", "ReceiptBalanceQty", "InvoiceBalanceQty", "TermName", "GroupBy", "Heading", "Status", "SubHeading", "SearchType", "EROOrderNumber", "EROOrderQty", "ToWorkShopName", "EffRate", "Total", "OrderType", "FromStoreID", "ToStoreID", "ToVendorID", "ToAircraftID", "TotalEROReceivedQty", "ToWorkShopID", "TransTypeID", "IsOverHaul", "LoanReturningCount", "ItemID", "LoanQty"}
                For i As Integer = 0 To columnToRemove1.Length - 1
                    If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains(columnToRemove1(i)) Then
                        ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Remove(columnToRemove1(i))
                    End If
                Next


                If cmbSearchType.SelectedItem.ToString = "Store" Then
                    Dim columnToRemove2 As String() = {"WorkShop", "Supplier", "Aircraft", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                        End If
                    Next
                ElseIf cmbSearchType.SelectedItem.ToString = "Aircraft" Then
                    Dim columnToRemove3 As String() = {"WorkShop", "Supplier", "Store", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove3.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove3(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove3(i))
                        End If
                    Next
                ElseIf cmbSearchType.SelectedItem.ToString = "Supplier" Then
                    Dim columnToRemove4 As String() = {"WorkShop", "Store", "Aircraft", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove4.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove4(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove4(i))
                        End If
                    Next
                ElseIf cmbSearchType.SelectedItem.ToString = "Customer" Then
                    Dim columnToRemove5 As String() = {"WorkShop", "Aircraft", "Store", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove5.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove5(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove5(i))
                        End If
                    Next
                Else
                    Dim columnToRemove6 As String() = {"Store", "Supplier", "Aircraft", "Customer", "CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                    For i As Integer = 0 To columnToRemove6.Length - 1
                        If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove6(i)) Then
                            ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove6(i))
                        End If
                    Next
                End If



                'Dim columnToRemove2 As String() = {"CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "Store", "Status", "DCNo", "InvText", "InvNo", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkShop", "WorkOrderText", "WorkOrderNo", "SerialNo"}
                'For i As Integer = 0 To columnToRemove2.Length - 1
                '    If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                '        ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                '    End If
                'Next


                If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains("Supplier") Then
                    ds.Tables("rptSearchingCriteriaForReceipt").Columns("Supplier").ColumnName = "Customer"
                End If

                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("PartName") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("PartName").ColumnName = "Part Number"
                End If

                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("Description") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("Description").ColumnName = "Description"
                End If

                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("SerialNo") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("SerialNo").ColumnName = "Serial No."
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("FromStoreName") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("FromStoreName").ColumnName = "From Store"
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("ToStoreName") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("ToStoreName").ColumnName = "To"
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanGiven") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("LoanGiven").ColumnName = "Load Given"
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanTaken") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("LoanTaken").ColumnName = "Loan Taken"
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanReturn") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("LoanReturn").ColumnName = "Loan Returning"
                End If
                If ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns.Contains("LoanRecovery") Then
                    ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").Columns("LoanRecovery").ColumnName = "Loan Recovery"
                End If
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
                dsNew.Merge(ds.Tables("ExcelrptSuppCustReceiptIssueLoanStatus"))

                dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
                dsNew.Tables("ExcelrptSuppCustReceiptIssueLoanStatus").TableName = "Loan Transcations"
				Session("ExcelFileName") = "Loan Transcations"
				Session("dsNew") = dsNew
				Session("DataTableToBeFormattedForExportToExcel") = "Loan Transcations"
                'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
                'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
                'da.Fill(ds, "rptIssueReceiptStatus", mrptIssueReceiptStatus)

                'Dim columnToRemove1 As String() = {"IssueID", "IssueDate", "IssueText", "IssueNo", "IssueToName", "ToTypeID", "ToVendorName", "ToAircraftName", "Remark", "Person", "CreatedBy", "AuthorizedBy", "IssueChieldId", "SrNo", "ReleaseNoteNo", "ReleaseNoteDate", "Qty", "Returnable", "Note", "ReceiptBalanceQty", "InvoiceBalanceQty", "TermName", "GroupBy", "Heading", "Status", "SubHeading", "SearchType", "EROOrderNumber", "EROOrderQty", "ToWorkShopName", "EffRate", "Total", "OrderType", "FromStoreID", "ToStoreID", "ToVendorID", "ToAircraftID", "TotalEROReceivedQty", "ToWorkShopID", "TransTypeID", "IsOverHaul", "LoanReturningCount", "ItemID", "LoanQty"}
                'For i As Integer = 0 To columnToRemove1.Length - 1
                '    If ds.Tables("rptIssueReceiptStatus").Columns.Contains(columnToRemove1(i)) Then
                '        ds.Tables("rptIssueReceiptStatus").Columns.Remove(columnToRemove1(i))
                '    End If
                'Next




                'Dim columnToRemove2 As String() = {"CompanyName", "FromDate", "ToDate", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "Supplier", "Store", "Status", "DCNo", "InvText", "InvNo", "FromStore", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkShop", "WorkOrderText", "WorkOrderNo"}
                'For i As Integer = 0 To columnToRemove2.Length - 1
                '    If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                '        ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                '    End If
                'Next




                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("PartName") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("PartName").ColumnName = "Part Number"
                'End If

                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("Description") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("Description").ColumnName = "Description"
                'End If

                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("SerialNo") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("SerialNo").ColumnName = "Serial No."
                'End If
                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("FromStoreName") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("FromStoreName").ColumnName = "From Store"
                'End If
                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("ToStoreName") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("ToStoreName").ColumnName = "To"
                'End If
                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("LoanGiven") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("LoanGiven").ColumnName = "Load Given"
                'End If
                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("LoanTaken") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("LoanTaken").ColumnName = "Loan Taken"
                'End If
                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("LoanReturn") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("LoanReturn").ColumnName = "Loan Returning"
                'End If
                'If ds.Tables("rptIssueReceiptStatus").Columns.Contains("LoanRecovery") Then
                '    ds.Tables("rptIssueReceiptStatus").Columns("LoanRecovery").ColumnName = "Loan Recovery"
                'End If
                'Dim dsNew As New DataSet
                'dsNew.Clear()

                'dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
                'dsNew.Merge(ds.Tables("rptIssueReceiptStatus"))

                'dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
                'dsNew.Tables("rptIssueReceiptStatus").TableName = "Loan Transcations"

                'Session("dsNew") = dsNew
                'Session("DataTableToBeFormattedForExportToExcel") = "Loan Transcations"
                ''PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
                ''Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                ''Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

            End If
            If mCount <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1305)
            End If


            MarkLog(Util.Action.Print, "IssueReceiptLoanStatus", "Export To excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)

        End If
    End Sub
End Class