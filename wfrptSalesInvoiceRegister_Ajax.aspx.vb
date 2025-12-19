'Created by Bhushan

Public Class wfrptSalesInvoiceRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Fromdate As String = ""
    Dim ToDate As String = ""
    Dim IssText As String = ""
    Dim IssNo As String = ""
    Dim Supplier As String = ""
    Dim Status As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim SalesOrdNo As String = ""
    Dim SalesOrdText As String = ""
    Dim InvNo As String = ""
    Dim InvText As String = ""
    Dim SerialNo As String = ""
    Public mVendor As Vendor
    Dim mItemList As ItemList
    Dim mVendorList As VendorList
    Dim mSalesOrderTextList As DistinctTextListForSalesOrder
    Dim mInvoiceTextList As DistinctTextListForInvoice
    Dim mIssueTextList As DistinctTextListForIssue

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid

    Dim GSTRecords As Integer = 0
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptSalesInvoiceRegister
    Dim da As New CSLA.Data.ObjectAdapter
    Dim dsSalesInvoice As New dsSalesInvoice
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItemList = Session("mItemList")
        PartNo = Session("PartNo")
        Description = Session("Description")
        mVendorList = CType(Session("mVendorList"), VendorList)
        mSalesOrderTextList = CType(Session("mSalesOrderTextList"), DistinctTextListForSalesOrder)
        mIssueTextList = CType(Session("mIssueTextList"), DistinctTextListForIssue)
        mInvoiceTextList = CType(Session("mInvoiceTextList"), DistinctTextListForInvoice)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub setSession()
        Session("mItemList") = mItemList
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("mVendorList") = mVendorList
        Session("mSalesOrderTextList") = mSalesOrderTextList
        Session("mIssueTextList") = mIssueTextList
        Session("mInvoiceTextList") = mInvoiceTextList
    End Sub
    Private Sub RemoveSession()
        mItemList = Nothing
        PartNo = Nothing
        Description = Nothing
        mVendorList = Nothing
        mSalesOrderTextList = Nothing
        mIssueTextList = Nothing
        mInvoiceTextList = Nothing
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mVendorList")
        Session.Remove("mSalesOrderTextList")
        Session.Remove("mIssueTextList")
        Session.Remove("mInvoiceTextList")
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
        lblVendor.Visible = True
        lblOrderNo.Visible = True
        lblStatus.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor.Visible = False
        lblOrderNo.Visible = False
        lblStatus.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
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

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        Supplier = txtCustomer.Text
        lblVendor.Text = "Customer  :  " & Supplier

        Supplier = IIf(txtCustomer.Text <> "", txtCustomer.Text, "")
        IssText = IIf(cmbDocType.SelectedIndex = 1, IIf(txtIssueTextList.Text <> "", txtIssueTextList.Text, ""), "")
        IssNo = IIf(cmbDocType.SelectedIndex = 1, txtNo.Text.Trim, "")
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        PartNo = IIf(Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Not IsNothing(Description), Description, "")
        SalesOrdNo = IIf(cmbDocType.SelectedIndex = 3, txtNo.Text.Trim, "")
        SalesOrdText = IIf(cmbDocType.SelectedIndex = 3, IIf(txtSalesOrderText.Text <> "", txtSalesOrderText.Text, ""), "")
        InvNo = IIf(cmbDocType.SelectedIndex = 2, txtNo.Text.Trim, "")
        InvText = IIf(cmbDocType.SelectedIndex = 2, IIf(txtSalesInvoiceTextList.Text <> "", txtSalesInvoiceTextList.Text, ""), "")

        lblStatus.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblVendor.Text = "Customer : " & IIf(Supplier <> "", Supplier, "All")

        Select Case cmbDocType.SelectedIndex
            Case 0
                lblOrderNo.Text = "Document Type : All "
            Case 1
                If IssText = "" Then
                    lblOrderNo.Text = "Issue No. : All "
                Else
                    lblOrderNo.Text = "Issue No. : " + IssText + "-" + IssNo
                End If
            Case 2
                If InvText = "" Then
                    lblOrderNo.Text = "Sales Invoice No. : All "
                Else
                    lblOrderNo.Text = "Sales Invoice No. : " + InvText + "-" + InvNo
                End If
            Case 3
                If SalesOrdText = "" Then
                    lblOrderNo.Text = "Sales Order No. : All "
                Else
                    lblOrderNo.Text = "Sales Order No. : " + SalesOrdText + "-" + SalesOrdNo
                End If
        End Select

        mCompleteSearchingCriteria = lblDateRange.Text + ", " + IIf(cmbDocType.SelectedIndex = 0, "All", cmbDocType.SelectedItem.Text) + ", " + _
                                        lblOrderNo.Text + ", " + lblVendor.Text + ", " + lblStatus.Text + ", " + IIf(chkDetail.Checked, "Detailed Report", "") + ", " +
                                        " Format " + IIf(optLandscape.Checked, "LandScape", "Portrait") + ", " + lblPartNo.Text + ", " + lblDesc.Text

    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim objReg As rptSalesInvoiceRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsSalesInvoice As New dsSalesInvoice
        Dim GSTRecords As Integer = 0
        SetValues()
        If chkDetail.Checked Then
            If optPortrait.Checked = True Then
                myReport = New crptSalesInvoiceRegister
            ElseIf optLandscape.Checked = True Then
                myReport = New crptSalesInvoiceRegisterLandscape
            ElseIf optWithGST.Checked = True Then
                GSTRecords = 1
                myReport = New crptSalesInvoiceGSTRegisterLandscape
            End If
        Else
            If optPortrait.Checked = True Then
                myReport = New crptSalesInvoiceRegSummary
            ElseIf optLandscape.Checked = True Then
                myReport = New crptSalesInvoiceRegSummaryLandscape
            ElseIf optWithGST.Checked = True Then
                GSTRecords = 1
                myReport = New crptSalesInvoiceGSTRegisterLandscape
            End If
        End If
        objReg = rptSalesInvoiceRegister.GetSalesInvoiceList(InvText, InvNo, Fromdate, ToDate, IssText, IssNo, SalesOrdText, SalesOrdNo, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue), CInt(cmbSalesInvoiceType.SelectedValue), GSTRecords:=GSTRecords)
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, IIf(cmbSalesInvoiceType.SelectedIndex > 0, cmbSalesInvoiceType.SelectedItem.Text, ""), "", "", IssText, SalesOrdText, "", IssNo, SalesOrdNo, "", Supplier, "", Status, "", PartNo, Description, InvText, InvNo, "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))

        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 636)
        End If

        dsSalesInvoice.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(dsSalesInvoice)

        da.Fill(dsSalesInvoice, mrptImage)
        da.Fill(dsSalesInvoice, objReg)
        da.Fill(dsSalesInvoice, objSearch)

        myReport.SetDataSource(dsSalesInvoice)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "SalesInvoicveReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                    'DataFieldBind()
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
            ControlVisibility(6)
            SetDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            optWithGST.DataBind()
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
        txtIssueTextList.Text = ""
        txtSalesInvoiceTextList.Text = ""
        txtSalesOrderText.Text = ""
        Dim Index As Int16 = IIf(cmbDocType.SelectedIndex > 0, cmbDocType.SelectedIndex, 0)
        lblDocTypeNo.Visible = (Index > 0)
        lblDocTypeNo.Text = IIf(Index = 0, "", IIf(Index = 1, "Issue No.  ", IIf(Index = 2, "Invoice No.  ", IIf(Index = 3, "Sales Order No.  ", ""))))
        txtIssueTextList.Visible = (Index = 1)
        txtSalesInvoiceTextList.Visible = (Index = 2)
        txtSalesOrderText.Visible = (Index = 3)
        txtNo.Visible = (Index = 1 Or Index = 2 Or Index = 3)
        If cmbDocType.Enabled = True Then
            SetFocus(cmbDocType)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
    'Added by Abhishek on 22-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid() Then
            SetValues()
            GenerateXLSXFile(CreateDataTable())
        End If
    End Sub
    Private Function CreateDataTable() As DataTable

        If optWithGST.Checked = True Then
                 GSTRecords = 1
          End If
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchSalesInvoiceList"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Text", InvText)
        cmd.Parameters.AddWithValue("@No", InvNo)
        cmd.Parameters.AddWithValue("@FromDate", Fromdate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)

        cmd.Parameters.AddWithValue("@IssueText", IssText)
        cmd.Parameters.AddWithValue("@IssueNo", IssNo)
        cmd.Parameters.AddWithValue("@SalesOrderText", SalesOrdText)
        cmd.Parameters.AddWithValue("@SalesOrderNo", SalesOrdNo)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@Description", Description)
        cmd.Parameters.AddWithValue("@StatusID", Status)
        cmd.Parameters.AddWithValue("@GSTRecords", GSTRecords)

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
       

        'dataTable.Columns.Remove("Rem1")
        'dataTable.Columns.Remove("Rem2")
        'dataTable.Columns.Remove("Rem3")
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(ByVal tbl As DataTable)

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), IIf(cmbDateRange.SelectedIndex = 0, "", Fromdate), IIf(cmbDateRange.SelectedIndex = 0, "", ToDate), IIf(cmbSalesInvoiceType.SelectedIndex > 0, cmbSalesInvoiceType.SelectedItem.Text, ""), "", "", IssText, SalesOrdText, "", IssNo, SalesOrdNo, "", Supplier, "", Status, "", PartNo, Description, InvText, InvNo, "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        da.Fill(dsSalesInvoice, objSearch)
        Dim columnToRemove As String() = {"ID", "CompanyName", "RecText", "RecNo", "Store", "DCNo", "Aircraft", "ReleaseNoteNo", "", "", "FromStore", "SerialNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ShowLogo", "WorkShop", "WorkOrderText", "WorkOrderNo", "Amend", "QuotationNo", "IntOrderNo"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If dsSalesInvoice.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove(i)) Then
                dsSalesInvoice.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove(i))
            End If
        Next
If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsSalesInvoice.Tables("rptSearchingCriteriaForReceipt"))
        dsNew.Merge(tbl)

        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InternalReceiptNo").ColumnName = "Type"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InvText").ColumnName = "Sales Invoice"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InvNo").ColumnName = "Sales Invoice No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("IssText").ColumnName = "Issue"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("IssNo").ColumnName = "Issue No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdText").ColumnName = "Order"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdNo").ColumnName = "Order No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Supplier").ColumnName = "Customer"

        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Type").SetOrdinal(0)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("FromDate").SetOrdinal(1)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("ToDate").SetOrdinal(2)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Sales Invoice").SetOrdinal(3)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Sales Invoice No.").SetOrdinal(4)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Issue").SetOrdinal(5)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Issue No.").SetOrdinal(6)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Order").SetOrdinal(7)
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Order No.").SetOrdinal(8)

        dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = "Sales Invoicve Register"
		Session("ExcelFileName") = "Sales Invoicve Register"
		Session("ExcelFileName") = "Sales Invoicve Register"
		Session("dsNew") = dsNew
		'Session("DataTable") = tbl
		'Session("ReportName") = "RCI Register"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "SalesInvoicveReg", "Export To Excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
End Class