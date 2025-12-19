Public Class wfrptInvoiceRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Fromdate As String = ""
    Dim ToDate As String = ""
    Dim RecText As String = ""
    Dim RecNo As String = ""
    Dim Supplier As String = ""
    Dim Status As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim OrdNo As String = ""
    Dim OrdText As String = ""
    Dim InvNo As String = ""
    Dim InvText As String = ""
    Dim SerialNo As String = ""
    Public mVendor As Vendor
    Dim mItemList As ItemList
    Dim mVendorList As VendorList
    Dim mOrderTextList As DistinctTextListForOrder
    Dim mInvoiceTextList As DistinctTextListForInvoice
    Dim mReceiptTextList As DistinctTextListForReceipt

    Dim Store As String = ""
    Public mStoreList As StoreList

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Dim mText As String = ""
    Public mCustomerList As VendorList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        ''mItemList = Session("mItemList")
        PartNo = Session("PartNo")
        Description = Session("Description")
        ''mVendorList = CType(Session("mVendorList"), VendorList)
        ''mOrderTextList = CType(Session("mOrderTextList"), DistinctTextListForOrder)
        ''mReceiptTextList = CType(Session("mReceiptTextList"), DistinctTextListForReceipt)
        ''mInvoiceTextList = CType(Session("mInvoiceTextList"), DistinctTextListForInvoice)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub setSession()
        ''Session("mItemList") = mItemList
        ''Session("PartNo") = PartNo
        ''Session("Description") = Description
        ''Session("mVendorList") = mVendorList
        ''Session("mOrderTextList") = mOrderTextList
        ''Session("mReceiptTextList") = mReceiptTextList
        ''Session("mInvoiceTextList") = mInvoiceTextList
    End Sub
    Private Sub RemoveSession()
        mItemList = Nothing
        PartNo = Nothing
        Description = Nothing
        mVendorList = Nothing
        mOrderTextList = Nothing
        mReceiptTextList = Nothing
        mInvoiceTextList = Nothing
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mVendorList")
        Session.Remove("mOrderTextList")
        Session.Remove("mReceiptTextList")
        Session.Remove("mInvoiceTextList")
    End Sub

    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)
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
        If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            lblValuedStores.Visible = True
            cmbStoreType.Visible = True
            lblCase.Visible = True
        Else
            lblValuedStores.Visible = False
            cmbStoreType.Visible = False
            lblCase.Visible = False
            lblStep7.Text = "Step IX. Display Report"
        End If
    End Sub

    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblOrderNo.Visible = True
        lblStatus.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblStore.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor.Visible = False
        lblOrderNo.Visible = False
        lblStatus.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblStore.Visible = False
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            Fromdate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If

        'NEWLY ADDED---------------
        Supplier = txtSupplierList.Text.Trim
        lblVendor.Text = "Supplier Name  :  " & Supplier

        'NEWLY ADDED---------------
        lblStore.Text = "Store Name  :  " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")
        '---------------------------

        Supplier = txtSupplierList.Text.Trim
        RecText = txtReceiptTextList.Text.Trim
        RecNo = IIf(cmbDocType.SelectedIndex = 1, txtNo.Text.Trim, "")
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If

        OrdNo = IIf(cmbDocType.SelectedIndex = 3, txtNo.Text.Trim, "")
        OrdText = txtOrderTextList.Text.Trim
        InvNo = IIf(cmbDocType.SelectedIndex = 2, txtNo.Text.Trim, "")
        InvText = txtInvoiceTextList.Text.Trim

        lblStatus.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblPartNo.Text = "Part No. : " & PartNo
        lblDesc.Text = "Description : " & Description
        lblVendor.Text = "Supplier Name : " & Supplier


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
                If InvText = "" Then
                    lblOrderNo.Text = "Invoice No. : All "
                Else
                    lblOrderNo.Text = "Invoice No. : " + InvText + "-" + InvNo
                End If
            Case 3
                If OrdText = "" Then
                    lblOrderNo.Text = "Order No. : All "
                Else
                    lblOrderNo.Text = "Order No. : " + OrdText + "-" + OrdNo
                End If
        End Select
        If chkHighValue.Checked And txtCEffectiveRate.Text <> "" Then 'Added By Prashant 14-Aug-2014 For ALL14082014
            mText = "Report shows valued parts with landing rate greater than  " + txtCEffectiveRate.Text
        Else
            mText = ""
        End If

        mCompleteSearchingCriteria = lblDateRange.Text + ", " + IIf(cmbDocType.SelectedIndex = 0, "All", cmbDocType.SelectedItem.Text) + ", " + lblOrderNo.Text + ", " + _
                                 lblVendor.Text + ", " + lblStore.Text + ", " + IIf(chkOnlyReceivedinSelectedStore.Checked, "Only Received in Selected Store", "") + ", " + _
                                 lblStatus.Text + ", " + " Format " + IIf(optLandscape.Checked, "LandScape", "Portrait") + lblPartNo.Text + ", " + lblDesc.Text


    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchInvoiceRegister"
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@Text", InvText)
        cmd.Parameters.AddWithValue("@No", InvNo)
        cmd.Parameters.AddWithValue("@FromDate", Fromdate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@ReceiptText", RecText)
        cmd.Parameters.AddWithValue("@ReceiptNo", RecNo)
        cmd.Parameters.AddWithValue("@OrderText", OrdText)
        cmd.Parameters.AddWithValue("@OrderNo", OrdNo)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@Description", Description)
        cmd.Parameters.AddWithValue("@StatusID", CInt(cmbStatus.SelectedValue))
        cmd.Parameters.AddWithValue("@StoreName", "")
        cmd.Parameters.AddWithValue("@StoreID", cmbStore.SelectedValue.ToString)
        cmd.Parameters.AddWithValue("@HighValue", chkHighValue.Checked)
        cmd.Parameters.AddWithValue("@RateValue", CDec(Val(txtCEffectiveRate.Text)))
        cmd.Parameters.AddWithValue("@ReceiptType", CInt(cmbReceiptType.SelectedValue))
        cmd.Parameters.AddWithValue("@IsValued", Val(cmbStoreType.SelectedValue))                   'Added By Prashant 15-Jan-2018 For Deccan15012018
        cmd.Parameters.AddWithValue("@ClientCode", AppSettings("ClientCode"))                       'Added By Prashant 15-Jan-2018 For Deccan15012018
        cmd.Parameters.AddWithValue("@IsCustomerStore", chkCustomerStock.Checked)
        cmd.Parameters.AddWithValue("@CustomerID", cmbCustomer.SelectedValue.ToString)
        cmd.Parameters.AddWithValue("@IsConsiderInvoice", ChkConsiderInv.Checked)                   'Added by Shital on 07-May-2019
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
        Dim dsOrder As New dsOrder

        Dim objSearch As rptSearchingCriteriaForReceipt
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, "", "", RecText, "", OrdText, RecNo, "", OrdNo, "", Supplier, IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Status, "", PartNo, Description, InvText, InvNo, mText, "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))

        dsOrder.Clear()
        da.Fill(dsOrder, objSearch)

        Dim columnToRemove As String() = {"ID", "CompanyName", "Aircraft", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "RecNo", "Store", "IssNo", "DCNo", "InvText", "InvNo", "FromStore", "SerialNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ShowLogo", "WorkShop", "WorkOrderText", "WorkOrderNo"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If dsOrder.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove(i)) Then
                dsOrder.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsOrder.Tables("rptSearchingCriteriaForReceipt"))
        dsNew.Merge(tbl)

        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdText").ColumnName = "Order Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdNo").ColumnName = "Order No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("PartNo").ColumnName = "Part No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Description").ColumnName = "Part Description"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("QuotationNo").ColumnName = "Quotation No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("IntOrderNo").ColumnName = "Int. Order No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("FromDate").ColumnName = "From Date"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("ToDate").ColumnName = "To Date"

        dsNew.Tables("TMainReport").Columns("AmountInBaseCur").ColumnName = "Amount (in " + objSearch(0).CurrencySymbol + ")"
        'dsNew.Tables("TMainReport").Columns("TotalOrderAmountInBaseCurrency").ColumnName = "Total Order Amount (in " + objSearch(0).CurrencySymbol + ")"

        dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = "Invoice Register"
		Session("ExcelFileName") = "Invoice Register"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "InvoiceReg", "Export To excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim objReg As rptInvoiceRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsReceipt As New dsInvoice

        SetValues()

        If chkOnlyReceivedinSelectedStore.Checked = False Then
            If chkDetail.Checked Then
                If optPortrait.Checked Then
                    myReport = New crptInvoiceRegister
                Else
                    myReport = New crptInvoiceRegisterLandscape
                End If
            Else
                If optPortrait.Checked Then
                    myReport = New crptInvoiceRegSummary 'crptInvoiceRegister
                Else
                    myReport = New crptInvoiceRegSummaryLandscape
                End If
            End If
        Else
            If optWithRate.Checked Then
                'myReport = New crptInvoiceRegisterWithRate
                myReport = New crptInvoiceRegisterLandscapeWithRate
            ElseIf optWithEffRate.Checked Then
                'myReport = New crptInvoiceRegisterWithEffRate
                myReport = New crptInvoiceRegisterLandscapeWithEffRate
            End If
        End If
        objReg = rptInvoiceRegister.GetInvoiceList(InvText, InvNo, Fromdate, ToDate, RecText, RecNo, OrdText, OrdNo, Supplier, PartNo, Description, _
                                                   CInt(cmbStatus.SelectedValue), "", chkOnlyReceivedinSelectedStore.Checked, cmbStore.SelectedValue.ToString, _
                                                   chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), CInt(cmbReceiptType.SelectedValue), _
                                                   IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), _
                                                   EffRateWithGST:=chkWithGST.Checked, IsCustomerStore:=chkCustomerStock.Checked, _
                                                   CustomerID:=cmbCustomer.SelectedValue.ToString, IsConsiderInvoice:=ChkConsiderInv.Checked)
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, IIf(chkWithGST.Visible, IIf(chkWithGST.Checked, "Invoice Register", "Invoice Register (Values excluding GST)"), "Invoice Register"), _
                                                                                  "", RecText, "", OrdText, RecNo, "", OrdNo, "", Supplier, _
                                                                                  IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Status, _
                                                                                   "", PartNo, Description, InvText, InvNo, mText, IIf(cmbCustomer.SelectedIndex > 0, cmbCustomer.SelectedItem.Text, ""), "", "", "", "", "", "", _
                                                                                  "", 0, "", "", AppSettings("Logo"))

        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objReg.Count > 0 Then 'Added By Utkarsh On 7-Jun-2011 For All07062011
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 621)
        End If

        dsReceipt.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt) 'Added by Shweta on 20-Feb-2012
            da.Fill(dsReceipt, mrptImage) 'Added by Shweta on 20-Feb-2012
        End If

        da.Fill(dsReceipt, objReg)
        da.Fill(dsReceipt, objSearch)

        myReport.SetDataSource(dsReceipt)

        Session("CrystalReport") = myReport
        MarkLog(Util.Action.Print, "InvoiceReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub

    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
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

    Private Sub SetDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select

        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))

    End Sub

    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Added By Prashant On 2-May-2013 For ALL29042013-4
        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        'Customer
        mCustomerList = VendorList.GetVendorstList(0, , , , , , "(All)", True, False)
        cmbCustomer.DataSource = mCustomerList

        DataBind()
        'End
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()

            If cmbDateRange.Enabled = True Then
                SetFocus(cmbDateRange)
            End If

            DataFieldBind()

            ControlVisibility(6)
            SetDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            optWithEffRate.Enabled = False
            optWithRate.Enabled = False
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        SetDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()

        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub cmbDocType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDocType.SelectedIndexChanged
        txtNo.Text = ""
        Dim Index As Int16 = IIf(cmbDocType.SelectedIndex > 0, cmbDocType.SelectedIndex, 0)
        lblDocTypeNo.Visible = (Index > 0)
        lblDocTypeNo.Text = IIf(Index = 0, "", IIf(Index = 1, "Receipt No. ", IIf(Index = 2, "Invoice No. ", IIf(Index = 3, "Order No. ", ""))))
        txtReceiptTextList.Visible = (Index = 1)
        txtInvoiceTextList.Visible = (Index = 2)
        txtOrderTextList.Visible = (Index = 3)
        txtNo.Visible = (Index = 1 Or Index = 2 Or Index = 3)
        txtReceiptTextList.Text = ""
        txtInvoiceTextList.Text = ""
        txtOrderTextList.Text = ""
        If cmbDocType.Enabled = True Then
            SetFocus(cmbDocType)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetValues()
        GenerateXLSXFile(CreateDataTable())
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged

    '    If cmbDocType.SelectedIndex = 1 Then                            'Receipt cum Invoice against Purchase Order Register
    '        Me.txtReceiptTextList.Visible = Not CType(sender, Boolean)
    '    ElseIf cmbDocType.SelectedIndex = 2 Then                       'Received from Store Register
    '        Me.txtInvoiceTextList.Visible = Not CType(sender, Boolean)
    '    ElseIf cmbDocType.SelectedIndex = 3 Then                     'Received from Aircraft Register
    '        Me.txtOrderTextList.Visible = Not CType(sender, Boolean)
    '    End If
    'End Sub
    'Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
    '    If cmbDocType.SelectedIndex = 1 Then                            'Receipt cum Invoice against Purchase Order Register
    '        Me.txtReceiptTextList.Visible = Not CType(sender, Boolean)
    '    ElseIf cmbDocType.SelectedIndex = 2 Then                       'Received from Store Register
    '        Me.txtInvoiceTextList.Visible = Not CType(sender, Boolean)
    '    ElseIf cmbDocType.SelectedIndex = 3 Then                     'Received from Aircraft Register
    '        Me.txtOrderTextList.Visible = Not CType(sender, Boolean)
    '    End If
    'End Sub
    Private Sub chkOnlyReceivedinSelectedStore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOnlyReceivedinSelectedStore.CheckedChanged
        If chkOnlyReceivedinSelectedStore.Checked = True Then
            chkDetail.Checked = False
            chkDetail.Enabled = False
            optPortrait.Enabled = False
            optLandscape.Enabled = False

            optWithEffRate.Enabled = True
            optWithRate.Enabled = True
        Else
            chkDetail.Enabled = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True

            optWithEffRate.Enabled = False
            optWithRate.Enabled = False
        End If
        upnlStatus.Update()
    End Sub
    'Commented and Added By Prashant On 30-Apr-2013 For ALL29042013-4
    'Private Sub txtStoreList_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If txtStoreList.Text.Trim = "" And chkOnlyReceivedinSelectedStore.Checked = True Then
    '        chkOnlyReceivedinSelectedStore.Checked = False
    '        chkOnlyReceivedinSelectedStore.Enabled = False
    '        chkDetail.Enabled = True
    '        optPortrait.Enabled = True
    '        optLandscape.Enabled = True
    '    ElseIf txtStoreList.Text.Trim = "" Then

    '        chkOnlyReceivedinSelectedStore.Enabled = False
    '    Else
    '        chkOnlyReceivedinSelectedStore.Enabled = True
    '    End If
    'End Sub
    Private Sub cmbStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        If cmbStore.SelectedIndex <= 0 And chkOnlyReceivedinSelectedStore.Checked = True Then
            chkOnlyReceivedinSelectedStore.Checked = False
            chkOnlyReceivedinSelectedStore.Enabled = False
            chkDetail.Enabled = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True
        ElseIf cmbStore.SelectedIndex <= 0 Then
            chkOnlyReceivedinSelectedStore.Enabled = False
        Else
            chkOnlyReceivedinSelectedStore.Enabled = True
        End If
    End Sub
    'End
    Private Sub chkHighValue_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkHighValue.CheckedChanged 'Added By Prashant 14-Aug-2014 For ALL14082014
        If chkHighValue.Checked = True Then
            txtCEffectiveRate.Enabled = True
        Else
            txtCEffectiveRate.Enabled = False
            txtCEffectiveRate.Text = ""
        End If
        upnlHighValue.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub cmbCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustomer.SelectedIndexChanged
        'Requested for Customer Stores  
        If chkCustomerStock.Checked Then
            If Not cmbCustomer.SelectedIndex <= 0 Then 'If Customer Selected
                'mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID

                mStoreList = StoreList.GetStoreList(New Guid(cmbCustomer.SelectedValue.ToString), "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
        End If
        cmbStore.DataBind()
        Session("mStoreList") = mStoreList
        If cmbCustomer.Enabled = True Then
            SetFocus(cmbCustomer)
        End If
        upnlReceivingStore.Update()
    End Sub
    Private Sub chkCustomerStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustomerStock.CheckedChanged
        If chkCustomerStock.Checked = True Then
            lblCustomer.Enabled = True
            cmbCustomer.Enabled = True

            If Not cmbCustomer.SelectedIndex <= 0 Then                       'If Customer Selected
                'mCustomerID = mCustomerList.Item(cmbCustomer.SelectedIndex).ID
                mStoreList = StoreList.GetStoreList(New Guid(cmbCustomer.SelectedValue.ToString), "(All)", True)    'Passing selected customer 
                cmbStore.DataSource = mStoreList
            ElseIf cmbCustomer.SelectedIndex = 0 Then
                mStoreList = StoreList.GetStoreList(2, "", "(All)", True)       'All
                cmbStore.DataSource = mStoreList
            End If
            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            SetFocus(cmbCustomer)
        Else
            cmbCustomer.SelectedIndex = 0
            lblCustomer.Enabled = False
            cmbCustomer.Enabled = False

            mStoreList = StoreList.GetSelfStoreList("", "(All)", True)         'Self
            cmbStore.DataSource = mStoreList

            cmbStore.DataBind()
            Session("mStoreList") = mStoreList
            If cmbStore.Enabled = True Then
                SetFocus(cmbStore)
            End If
        End If
        upnlReceivingStore.Update()
    End Sub

    Private Sub ChkConsiderInv_CheckedChanged(sender As Object, e As System.EventArgs) Handles ChkConsiderInv.CheckedChanged
        If ChkConsiderInv.Checked Then
            optPortrait.Checked = False
            optLandscape.Checked = True
            optPortrait.Enabled = False
        Else
            optPortrait.Checked = True
            optLandscape.Checked = False
            optPortrait.Enabled = True
        End If
        upnlConsiderInv.Update()
        upnlStatus.Update()
    End Sub

#End Region
End Class