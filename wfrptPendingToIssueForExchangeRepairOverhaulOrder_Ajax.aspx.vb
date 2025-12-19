Public Class wfrptPendingToIssueForExchangeRepairOverhaulOrder_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mVendorList As VendorList
    Public ToDate As String
    Public PartNo As String
    Public Description As String
    Dim mPendingToIssueForExchangeRepairOverhaulOrderSearchingCriteria As String = String.Empty

    'Added by Abhishek on 14-SEP-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim mCompanyDetail As New CompanyDetail
    Dim ds As New dsPendingToIssueForExchangeRepairOverhaulOrder
    Dim rpt As rptPendingToIssueForExchangeRepairOverhaulOrder

#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorList"), VendorList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mAircraftList") = mVendorList
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Controlvisibility(ByVal Index As Int16)
        lblSupplier1.Visible = False
        lblDateRange.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub SetValues()
        ToDate = txtAsOnDate.Text.ToString
        lblDateRange.Text = "Date : " & New SmartDate(txtAsOnDate.Text.ToString).FormattedText

        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")

        Session("PartNo") = PartNo
        Session("Description") = Description

        lblPartNo.Text = "Part No. : " + IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " + IIf(Description <> "", Description, "All")
        lblSupplier1.Text = "Store : " + IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, "All")
        mPendingToIssueForExchangeRepairOverhaulOrderSearchingCriteria = lblDateRange.Text.Trim + ", " + lblSupplier1.Text.Trim + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim
    End Sub
    Private Sub SetReport(Optional ByVal IsForExcel As Boolean = False)
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsPendingToIssueForExchangeRepairOverhaulOrder
        Dim rpt As rptPendingToIssueForExchangeRepairOverhaulOrder

        myReport = New crptPendingToIssueForExchangeRepairOverhaulOrder
        rpt = rptPendingToIssueForExchangeRepairOverhaulOrder.GetPendingToIssueForExchangeRepairOverhaulOrder(ToDate, cmbSupplier.SelectedValue.ToString, _
                                                                                                              PartNo, Description, cmbOrderType.SelectedValue, _
                                                                                                              SerialNo:=txtSerialNo.Text.Trim)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1286)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Pending To Issue For Exchange Repair Overhaul (Core Unit Return)", _
                New SmartDate(ToDate).FormattedText, IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), PartNo, Description, _
                cmbOrderType.SelectedItem.Text, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6:=txtSerialNo.Text.Trim, _
                SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"))
        If IsForExcel = False Then
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "PendingToIssueForERO", mPendingToIssueForExchangeRepairOverhaulOrderSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsForExcel = True Then
            ds.Clear()
            da.Fill(ds, Report)
            da.Fill(ds, "ExcelrptPendingToIssueForExchangeRepairOverhaulOrder", rpt)

            Dim columnToRemove1 As String() = {"OrderDate", "ReceiptDate", "OrderText", "OrderNo", "ReceiptText", "ReceiptNo", "Amend"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "ReportDate", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "CurrencyName", "CurrencySymbol", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next
            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "DateTo "
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Supplier "
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Part No."
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Description"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Order Type"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Serial No."
            End If

            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("OrderNumber") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("OrderNumber").ColumnName = "Order No."
            End If
            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("VendorName") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("VendorName").ColumnName = "Supplier"
            End If

            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("ItemName") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("ItemDescription") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("ItemDescription").ColumnName = "Description"
            End If
            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("OrderItemSerialNo") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("OrderItemSerialNo").ColumnName = "Exchange Serial No."
            End If

            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
            End If
            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("ReceiptNumber") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("ReceiptNumber").ColumnName = "Receipt No."
            End If
            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("ReceiptItemSerialNo") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("ReceiptItemSerialNo").ColumnName = "Receipt Serial No. "
            End If

            If ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns.Contains("ElapsedDays") Then
                ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").Columns("ElapsedDays").ColumnName = "Elapsed Days"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptPendingToIssueForExchangeRepairOverhaulOrder").TableName = "Pending To Issue For ERO  "
			Session("ExcelFileName") = "Pending To Issue For ERO"
			Session("dsNew") = dsNew
			Session("DataTableToBeFormattedForExportToExcel") = "Pending To Issue For ERO "
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "PendingToIssueForERO", "Export To Excel " + mPendingToIssueForExchangeRepairOverhaulOrderSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
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
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(ALL)", False, True)
        cmbSupplier.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtAsOnDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            DataFieldBind()
            Controlvisibility(2)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRange.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblSupplier1.Visible = True
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mVendorList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'Added by Abhishek on 14-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
#End Region
End Class
