Public Class wfrptUnderWarrantyUnserviceableItemList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim SearchIndex, FromDate, ToDate, Name, No As String
    Dim EventLogID As Guid
    Dim mIssue As Issue
    Dim mReceiptCumInvoice As ReceiptCumInvoice
    Dim mReceipt As Receipt
    Dim mOrder As Order
    Dim mUnderWarrantyUnserviceableItemList As UnderWarrantyUnserviceableItemList
    Dim mUnderWarrantyUnserviceableItemTransactionDetails As UnderWarrantyUnserviceableItemTransactionDetails
#End Region

#Region " Business Methods"
    Private Sub GetSession()
        mUnderWarrantyUnserviceableItemList = Session("mUnderWarrantyUnserviceableItemList")
        mUnderWarrantyUnserviceableItemTransactionDetails = Session("mUnderWarrantyUnserviceableItemTransactionDetails")
        SearchIndex = Session("SearchIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        Name = IIf(IsNothing(Session("Name")), "", Session("Name"))
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUnderWarrantyUnserviceableItemList")
        Session.Remove("mUnderWarrantyUnserviceableItemTransactionDetails")
        Session.Remove("SearchIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("Name")
        Session.Remove("No")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptUnderWarrantyUnserviceableItemList_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/3300")
        mUnderWarrantyUnserviceableItemList = Nothing
        dgItemList.DataSource = Nothing
        mUnderWarrantyUnserviceableItemList = UnderWarrantyUnserviceableItemList.GetUnderWarrantyUnserviceableItemList(ItemName, "", "", FromDate, ToDate)
        Session("mUnderWarrantyUnserviceableItemList") = mUnderWarrantyUnserviceableItemList
        dgItemList.DataSource = mUnderWarrantyUnserviceableItemList
        lblResult.Text = "List of Unserviceable Item(s) as per Criteria :" & mUnderWarrantyUnserviceableItemList.Count & " Record(s) found."
    End Sub
    Private Sub DataFieldBind()
        SearchIndex = IIf(IsNothing(SearchIndex), 0, SearchIndex)

        If FromDate Is Nothing And ToDate Is Nothing Then
            FromDate = CDate(Today.AddDays(1).AddMonths(-2)).ToString(AppSettings("DateFormat"))
            ToDate = Now.Date.ToString(AppSettings("DateFormat"))
        End If
        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
        txtSearch.Text = Name
        mUnderWarrantyUnserviceableItemList = UnderWarrantyUnserviceableItemList.GetUnderWarrantyUnserviceableItemList(txtSearch.Text.Trim, "", "", txtFromDate.Text, txtToDate.Text)

        dgItemList.DataSource = mUnderWarrantyUnserviceableItemList
        dgItemList.DataBind()

        Session("SearchIndex") = SearchIndex
        Session("mUnderWarrantyUnserviceableItemList") = mUnderWarrantyUnserviceableItemList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        DataBind()
        lblResult.Text = "List of Unserviceable Item(s) as per Criteria :" & mUnderWarrantyUnserviceableItemList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptUnderWarrantyUnserviceableItemList_Ajax.aspx?"
            DataFieldBind()
        End If
    End Sub
    Private Sub dgItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgItemList.PageIndexChanging
        dgItemList.PageIndex = e.NewPageIndex
        dgItemList.DataSource = mUnderWarrantyUnserviceableItemList
        Session("mUnderWarrantyUnserviceableItemList") = mUnderWarrantyUnserviceableItemList
        dgItemList.DataBind()
    End Sub
    Private Sub dgItemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgItemList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim ItemID As Guid = (DataBinder.Eval(e.Row.DataItem, "ItemID"))
            Dim mSerialNo As String = (DataBinder.Eval(e.Row.DataItem, "SerialNo"))
            Dim ReceiptItemID As Guid = (DataBinder.Eval(e.Row.DataItem, "ReceiptItemID"))
            Dim dgTransactionDetails As GridView = DirectCast(e.Row.FindControl("dgTransactionDetails"), GridView)
            mUnderWarrantyUnserviceableItemTransactionDetails = UnderWarrantyUnserviceableItemTransactionDetails.GetUnderWarrantyUnserviceableItemTransactionDetails(ItemID.ToString, mSerialNo, ReceiptItemID.ToString)
            dgTransactionDetails.DataSource = mUnderWarrantyUnserviceableItemTransactionDetails
            dgTransactionDetails.DataBind()
        End If
    End Sub
    Protected Sub dgTransactionDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "TranasactionNo"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim id As Guid = New Guid(CType(sender, GridView).DataKeys(index).Item(0).ToString)
                Dim Type As Integer = CType(sender, GridView).DataKeys(index).Item(2)
                Dim InvoiceID As Guid = New Guid(CType(sender, GridView).DataKeys(index).Item(1).ToString)
                Select Case Type
                    Case 1, 2, 4, 5, 6, 11, 12 ''TransTypeID=9      Received From Aircraft As Removed    TransTypeID=61     Received From Work Order TransTypeID=66     Received From Aircraft As core unit return TransTypeID=10     Received From Supplier As Exchange/Repair
                        mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(id, InvoiceID)
                        Session("mReceiptCumInvoice") = mReceiptCumInvoice
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfReceiptCumInvoice_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 7, 8 'Order
                        mOrder = Order.GetOrder(id)
                        Session("mOrder") = mOrder
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPurchaseOrder_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 3, 9, 10 'Issue
                        mIssue = Issue.GetIssue(id)
                        Session("mIssue") = mIssue
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfIssue_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                    Case 13
                        mReceipt = Receipt.GetReceipt(id)
                        Session("mReceipt") = mReceipt
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfReceipt_Ajax.aspx?Type=FromReqItemStatusReport');", True)
                End Select
        End Select
    End Sub
    Private Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Dim Rpt As New crpWarrantyPartStatus
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsWarrantyPartStatus
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportDetails As New rptStatusList
        mUnderWarrantyUnserviceableItemList = Session("mUnderWarrantyUnserviceableItemList")

        For i As Integer = 0 To mUnderWarrantyUnserviceableItemList.Count - 1
            mUnderWarrantyUnserviceableItemTransactionDetails = UnderWarrantyUnserviceableItemTransactionDetails.GetUnderWarrantyUnserviceableItemTransactionDetails(mUnderWarrantyUnserviceableItemList(i).ItemID.ToString, mUnderWarrantyUnserviceableItemList(i).SerialNo, mUnderWarrantyUnserviceableItemList(i).ReceiptItemID.ToString)
            For j As Integer = 0 To mUnderWarrantyUnserviceableItemTransactionDetails.Count - 1
                ReportDetails.Add(New rptStatus(, 0, mUnderWarrantyUnserviceableItemTransactionDetails(j).ReceiptItemID.ToString, mUnderWarrantyUnserviceableItemTransactionDetails(j).TypeName, mUnderWarrantyUnserviceableItemTransactionDetails(j).TranasactionNo, mUnderWarrantyUnserviceableItemTransactionDetails(j).DateFormatted, mUnderWarrantyUnserviceableItemTransactionDetails(j).SerialNo, mUnderWarrantyUnserviceableItemTransactionDetails(j).Rate, mUnderWarrantyUnserviceableItemTransactionDetails(j).GROEffRate))
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Warranty Part Status", txtFromDate.Text, txtToDate.Text, txtSearch.Text, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mUnderWarrantyUnserviceableItemList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1309)
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mUnderWarrantyUnserviceableItemList)
        da.Fill(ds, mUnderWarrantyUnserviceableItemTransactionDetails)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "UnserviceablePartUnderWarranty", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Name = txtSearch.Text.Trim
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        Session("Name") = Name
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        FindNow(txtSearch.Text.Trim, FromDate, ToDate)     'for all records
        dgItemList.PageIndex = 0
        dgItemList.DataBind()
        upnlGrid.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgItemList.Sorting
        mUnderWarrantyUnserviceableItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mUnderWarrantyUnserviceableItemList") = mUnderWarrantyUnserviceableItemList
        dgItemList.DataSource = mUnderWarrantyUnserviceableItemList
        dgItemList.DataBind()
    End Sub
#End Region

End Class