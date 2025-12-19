Imports System.Collections.Generic

Public Class wfrptSearchOrderHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String
    Public Description As String = ""
    Public Supplier As String
    Dim EventLogID As Guid 'Added by Prashant
    Dim mOrderHistorySearchingCriteria As String = String.Empty
    'Added by Abhishek on 10-OCT-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteria
    Dim rpt As rptOrderHistory
    Dim ds As New dsOrder
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        If Index = 6 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = False
        lblVendorName.Visible = False
        lblPartNo.Visible = False
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblVendorName.Visible = True
        lblPartNo.Visible = True
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
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(txtFromDate.Text.ToString).FormattedText & " To " & New SmartDate(txtToDate.Text.ToString).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        If txtSupplierList.Text = "" Then
            Supplier = ""
            lblVendorName.Text = "Supplier Name  : All"
        Else
            Supplier = txtSupplierList.Text
            lblVendorName.Text = "Supplier Name : " & Supplier
        End If
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")  'Shweta
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "") 'Shweta
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No./Description : " & IIf(txtSearch.Text <> "", txtSearch.Text.Trim, "") 'Added by shweta on 19/1/2012
        mOrderHistorySearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblVendorName.Text.Trim + ", " + lblPartNo.Text.Trim
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptOrderHistory
        SetValues()
        Dim ds As New dsOrder
        myReport = New crptOrderHistory
        rpt = rptOrderHistory.GetOrderHistory(FromDate, ToDate, Supplier, PartNo, Description, TransTypeID:=CInt(cmbOrderType.SelectedValue))
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate,
                                                              txtSearch.Text, Supplier, BranchName:=cmbOrderType.SelectedItem.Text,
                                                              "", "", "", "", "", "",
                                                              AppSettings("Logo"))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 711)
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
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "OrderHistory", mOrderHistorySearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
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
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
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
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'Added by Abhishek on 10-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            Dim dsOrderHistoryInExcel As New dsOrderHistoryInExcel
            Dim mOrderHistoryInExcel As OrderHistoryInExcel
            SetValues()
            mOrderHistoryInExcel = OrderHistoryInExcel.GetOrderHistoryInExcel(FromDate, ToDate, Supplier,
                                                                              PartNo, Description,
                                                                              TransTypeID:=CInt(cmbOrderType.SelectedValue))

            If mOrderHistoryInExcel.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate,
                                                                  txtSearch.Text, Supplier, BranchName:=cmbOrderType.SelectedItem.Text, "", "",
                                                                  "", "", "", "", AppSettings("Logo"))
            da.Fill(dsOrderHistoryInExcel, "OrderHistoryInExcel", mOrderHistoryInExcel)
            da.Fill(dsOrderHistoryInExcel, "rptSearchingCriteria", objsearch)

            Dim columnToRemoveFromOrderHistoryInExcel As String() = {"Unit", "UnitName"}
            For i As Integer = 0 To columnToRemoveFromOrderHistoryInExcel.Length - 1
                If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains(columnToRemoveFromOrderHistoryInExcel(i)) Then
                    dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Remove(columnToRemoveFromOrderHistoryInExcel(i))
                End If
            Next

            Dim columnToRemove As String() = {"CompanyName", "Category", "Nomenclature", "Store", "Aircraft", "KitName", "RelNoteNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "WorkOrderText", "WorkShop", "FromStore", "ProductVersion", "SINote", "TransTypeID", "WorkOrderNo"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If dsOrderHistoryInExcel.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove(i)) Then
                    dsOrderHistoryInExcel.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove(i))
                End If
            Next
            If dsOrderHistoryInExcel.Tables("rptSearchingCriteria").Columns.Contains("BranchName") Then
                dsOrderHistoryInExcel.Tables("rptSearchingCriteria").Columns("BranchName").ColumnName = "Order Type"
            End If

            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("OrderDate") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("OrderDate").ColumnName = "Date"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("OrderNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("OrderNo").ColumnName = "Order Number"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("PartName") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("PartName").ColumnName = "Part Number"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("OrderQty") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("OrderQty").ColumnName = "Order Qty."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReceiptQty") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReceiptQty").ColumnName = "Received Qty."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReminingQty") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReminingQty").ColumnName = "Remaining Qty."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReqNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReqNo").ColumnName = "MRN/PPS No."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReqUserName") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReqUserName").ColumnName = "MRN/PPS BY"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReqDate") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReqDate").ColumnName = "MRN/PPS Date"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReqItemRegNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReqItemRegNo").ColumnName = "Required For"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReceiptNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReceiptNo").ColumnName = "Receipt No."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("RecDate") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("RecDate").ColumnName = "Receipt Date"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("SupplierName") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("SupplierName").ColumnName = "Supplier"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("ReleaseNoteNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("ReleaseNoteNo").ColumnName = "Release Note No."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("SerialNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("InvDate") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("InvDate").ColumnName = "Invoice Date"
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("InvoiceNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("InvoiceNo").ColumnName = "Invoice No."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("SuppInvNo") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("SuppInvNo").ColumnName = "Supplier Inv.No."
            End If
            If dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns.Contains("InvQty") Then
                dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("InvQty").ColumnName = "Inv.Qty."
            End If

            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Date").SetOrdinal(0)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Order Number").SetOrdinal(1)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Description").SetOrdinal(2)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Part Number").SetOrdinal(3)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Order Qty.").SetOrdinal(4)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Received Qty.").SetOrdinal(5)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Remaining Qty.").SetOrdinal(6)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("MRN/PPS No.").SetOrdinal(7)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("MRN/PPS BY").SetOrdinal(8)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("MRN/PPS Date").SetOrdinal(9)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Required For").SetOrdinal(10)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Receipt No.").SetOrdinal(11)
            dsOrderHistoryInExcel.Tables("OrderHistoryInExcel").Columns("Receipt Date").SetOrdinal(12)

            Dim dsNew As New DataSet
            dsNew.Clear()
            dsNew.Merge(dsOrderHistoryInExcel.Tables("rptSearchingCriteria"))
            dsNew.Merge(dsOrderHistoryInExcel.Tables("OrderHistoryInExcel"))

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("OrderHistoryInExcel").TableName = "Order History"

            Dim OrderHistoryInExcelTemp As New List(Of String)
            OrderHistoryInExcelTemp.AddRange(New String() {"Order Qty."})
            Session("OrderHistoryOrderQuantityColumns") = OrderHistoryInExcelTemp
			Session("ExcelFileName") = "Order History"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "OrderHistory", "Export To Excel " + mOrderHistorySearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub

#End Region

End Class
