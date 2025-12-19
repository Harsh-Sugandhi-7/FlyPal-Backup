Imports System.Collections.Generic

'Added by Utkarsh on 05-Feb-2014

Public Class wfrptSearchPendingPayment_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mVendorList As VendorList
    Public mVendor As Vendor

    Dim FromDate As String
    Dim ToDate As String    
    Dim PartNo As String
    Dim Description As String
    Dim Supplier As String
    Dim VendorInvoiceNo As String
    Dim EventLogDetail As String


    Dim SuppInvNo As String = ""
    'added by Abhishek on 9/8/2017 
    Dim rpt As rptPendingPayment
    Dim objsearch As rptSearchingCriteriaForReceipt
    Dim dsPenPay As New dsPendingPayment
    Dim da As New CSLA.Data.ObjectAdapter
    Public Aircraft As String = String.Empty
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
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
        upnlDates.Update()
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
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.Trim
            ToDate = txtToDate.Text.Trim
            lblDateRangeFrom.Text = "Date Range : " & FromDate & " To " & ToDate & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        Dim suplid As Guid = New Guid(Request.Form("cmbSupplier").ToString)
        If suplid.Equals(Guid.Empty) Then
            Supplier = ""
            lblVendorName.Text = "Supplier : All"
        Else
            Supplier = mVendorList(suplid).Name
            lblVendorName.Text = "Supplier : " & Supplier
        End If
        If txtVendorInvoiceNo.Text = "" Then
            lblVendorInvNoName.Text = "Supplier Invoice No. : All"
        Else
            lblVendorInvNoName.Text = "Supplier Invoice No. : " & txtVendorInvoiceNo.Text
        End If

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblVendorName.Text + ", " + lblVendorInvNoName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        Supplier = ""
        txtVendorInvoiceNo.Text = ""
        PartNo = ""
        Description = ""
    End Sub
    Private Sub SetReport()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As rptPendingPayment
        myReport = New crptPendingPayment
        Dim objsearch As rptSearchingCriteriaForReceipt
        GetSession()
        SetValues()
        Dim dsPenPay As New dsPendingPayment
       
        ' objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, "", "", "", "", "", "", "", "", "", Supplier, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", "")
        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, "", "", "", "", "", "", "", "", "", Supplier, "", "", "", PartNo, "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", "")

        rpt = rptPendingPayment.GetPendingPayment(FromDate, ToDate, Supplier, txtVendorInvoiceNo.Text.Trim)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011

        ElseIf rpt.Count > 0 Then

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 502)

            '******************************
        End If
        dsPenPay.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(dsPenPay) 'Added by Shweta on 20-Feb-2012
        da.Fill(dsPenPay, rpt)
        da.Fill(dsPenPay, objsearch)
        da.Fill(dsPenPay, mrptImage) 'Added by Shweta on 20-Feb-2012
        myReport.SetDataSource(dsPenPay)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "PendingPayment", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'ResetValues()
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbSupplier.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        GetSession()
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
        'SetValues()
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
        lblDateRangeFrom.Visible = True
        lblToDate1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblVendorName.Visible = True
        lblVendorInvNoName.Visible = True
        upnlCriteria.Update()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mVendorList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub

#End Region

    'added by Abhishek on 9/8/2017 
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            'Dim PeriodColumnsForExportToExcel As New List(Of String)
            SetValues()
            rpt = rptPendingPayment.GetPendingPayment(FromDate, ToDate, Supplier, txtVendorInvoiceNo.Text.Trim)

            ' objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, Supplier, "", "", "", "", "", "", Description, "", "", "", "", "", AppSettings("Logo"))
            objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, "", "", "", "", "", "", "", "", "", Supplier, "", "", "", PartNo, "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", "")

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            dsPenPay.Clear()

            da.Fill(dsPenPay, objsearch)
            da.Fill(dsPenPay, "ExcelrptPendingPayment", rpt)

            Dim columnToRemove1 As String() = {"InvoiceID", "CGrandTotal", "Currency", "PendingAmt", "CTotalAmt"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If dsPenPay.Tables("ExcelrptPendingPayment").Columns.Contains(columnToRemove1(i)) Then
                    dsPenPay.Tables("ExcelrptPendingPayment").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"CompanyName", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "Store", "Status", "DCNo", "", "Description", "InvText", "InvNo", "FromStore", "Amend", "QuotationNo", "IntOrderNo", "SerialNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "WorkShop", "WorkOrderText", "WorkOrderNo"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsPenPay.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                    dsPenPay.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If dsPenPay.Tables("ExcelrptPendingPayment").Columns.Contains("_Date") Then
                dsPenPay.Tables("ExcelrptPendingPayment").Columns("_Date").ColumnName = "Invoice Date"
            End If

            If dsPenPay.Tables("ExcelrptPendingPayment").Columns.Contains("SuppInvNo") Then
                dsPenPay.Tables("ExcelrptPendingPayment").Columns("SuppInvNo").ColumnName = "Supplier Invoice No"
            End If

            If dsPenPay.Tables("ExcelrptPendingPayment").Columns.Contains("SuppInvDate") Then
                dsPenPay.Tables("ExcelrptPendingPayment").Columns("SuppInvDate").ColumnName = "Supplier Invoice Date"
            End If
            If dsPenPay.Tables("ExcelrptPendingPayment").Columns.Contains("SupplierName") Then
                dsPenPay.Tables("ExcelrptPendingPayment").Columns("SupplierName").ColumnName = "Supplier Name"
            End If

            If dsPenPay.Tables("ExcelrptPendingPayment").Columns.Contains("TotalAmount") Then
                dsPenPay.Tables("ExcelrptPendingPayment").Columns("TotalAmount").ColumnName = "Total Amount"
            End If

            If dsPenPay.Tables("ExcelrptPendingPayment").Columns.Contains("PendingAmount") Then
                dsPenPay.Tables("ExcelrptPendingPayment").Columns("PendingAmount").ColumnName = "Pending Amount"
                '(" + CType(objsearch.CurrentItem, Flypal.rptSearchingCriteriaForReceipt.Search).CurrencySymbol + ")
            End If

                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(dsPenPay.Tables("rptSearchingCriteriaForReceipt"))
                dsNew.Merge(dsPenPay.Tables("ExcelrptPendingPayment"))

                dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
                dsNew.Tables("ExcelrptPendingPayment").TableName = "Pending Payment"
			Session("ExcelFileName") = "Pending Payment"
			Session("dsNew") = dsNew
			Session("DataTableToBeFormattedForExportToExcel") = "Pending Payment"
                'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
                'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "PendingPayment", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
    End Sub
End Class