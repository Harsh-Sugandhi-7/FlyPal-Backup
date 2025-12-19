'Added By Vikrant On 07-June-2013 For ALL06062013
Imports System.Web.Services

Partial Class wfOrderItemListForFollowUp
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mOrderItemListForFollowUp As OrderItemListForFollowUp
    Dim mDistinctTextListForOrder As DistinctTextListForOrder
    Dim AsOnDate As String
    Dim mrptOrderListSum As rptOrderRegisterSum
    Dim SearchIndex, Priority, OrderText, Name, No, Amend, OrderType As String
    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Dim mOrderItemFollowUps As OrderItemFollowUps
    Dim mOrderItemListForFollowUpExportToexcel As OrderItemListForFollowUpExportToexcel  'Added By Prashant on 7-July-2021 Heligo07072021
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mOrderItemListForFollowUp = Session("mOrderItemListForFollowUp")
        mDistinctTextListForOrder = Session("mDistinctTextListForOrder")
        SearchIndex = Session("SearchIndex")
        OrderText = Session("OrderText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        Amend = IIf(IsNothing(Session("Amend")), "", Session("Amend"))
        OrderType = Session("OrderType")
        Priority = Session("Priority")
        mOrderItemListForFollowUpExportToexcel = Session("mOrderItemListForFollowUpExportToexcel")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("SearchIndex")
        Session.Remove("Priority")
        Session.Remove("OrderText")
        Session.Remove("Name")
        Session.Remove("No")
        Session.Remove("Amend")
        Session.Remove("OrderType")
        Session.Remove("mOrderItemListForFollowUp")
        Session.Remove("mDistinctTextListForOrder")
        Session.Remove("mOrderItemListForFollowUpExportToexcel")
    End Sub
    Private Sub SetControl()
        CallFindNow(SearchIndex)
        dgOrderList.DataBind()

        cmbSearch.SelectedIndex = 0
        cmbPriority.SelectedValue = Priority

        If mDistinctTextListForOrder.Contains(OrderText) Then
            cmbOrderText.SelectedValue = IIf(OrderText = "", "(All)", OrderText)
        Else
            cmbOrderText.SelectedValue = "(All)"
        End If

        txtName.Text = Name
        txtNo.Text = No
        txtAmend.Text = Amend

        lblResult.Text = "List of Order Item(s) as per Criteria :" & mOrderItemListForFollowUp.Count & " Record(s) found."
    End Sub
    Private Sub FindNow(Optional ByVal AsOnDate As String = "1/1/3300", Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal Amend As String = "", Optional ByVal TransTypeID As Integer = 0, Optional ByVal PriorityID As Integer = 0, Optional ByVal VendorName As String = "", Optional ByVal ReceivedOrderedItemFollowUp As Boolean = False)
        mOrderItemListForFollowUp = Nothing
        dgOrderList.DataSource = Nothing
        mOrderItemListForFollowUp = OrderItemListForFollowUp.GetOrderItemListForFollowUp(AsOnDate, ItemName, Text, No, Amend, TransTypeID, Priority, VendorName, chkReceivedorderitemfollowup.Checked)
        mOrderItemListForFollowUpExportToexcel = OrderItemListForFollowUpExportToexcel.GetOrderItemListForFollowUpExportToexcel(AsOnDate, ItemName, Text, No, Amend, TransTypeID, Priority, VendorName, chkReceivedorderitemfollowup.Checked)
        Session("mOrderItemListForFollowUp") = mOrderItemListForFollowUp
        Session("mOrderItemListForFollowUpExportToexcel") = mOrderItemListForFollowUpExportToexcel
        dgOrderList.DataSource = mOrderItemListForFollowUp
        lblResult.Text = "List of Order Item(s) as per Criteria :" & mOrderItemListForFollowUp.Count & " Record(s) found."
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer) '
        Dim tmpmTransTypeID As Trans = 0

        Select Case Index
            Case 0  'all
                Call FindNow(AsOnDate, "", "", 0, "", 0, 0, "", chkReceivedorderitemfollowup.Checked)     'for all records
            Case 1  'Order Text , No And Amend
                Call FindNow(AsOnDate, "", OrderText, CInt(Val(No)), Amend, 0, 0, "", chkReceivedorderitemfollowup.Checked)
            Case 2  'ItemName
                Call FindNow(AsOnDate, Name, "", 0, "", 0, 0, "", chkReceivedorderitemfollowup.Checked)
            Case 3 'Priority
                Call FindNow(AsOnDate, "", "", 0, "", 0, CInt(Priority), "", chkReceivedorderitemfollowup.Checked)
            Case 4 ' OrderType
                Call FindNow(AsOnDate, "", "", 0, "", CInt(cmbOrderTypeList.SelectedValue), 0, "", chkReceivedorderitemfollowup.Checked)
            Case 5 ' Supplier
                Call FindNow(AsOnDate, "", "", 0, "", 0, 0, Name, chkReceivedorderitemfollowup.Checked)
        End Select
        dgOrderList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32)
        cmbOrderText.Visible = IIf(SearchIndex = 1, True, False)
        lblNo.Visible = IIf(SearchIndex = 1 And cmbOrderText.SelectedIndex <> 0, True, False)
        txtNo.Visible = IIf(SearchIndex = 1 And cmbOrderText.SelectedIndex <> 0, True, False)
        txtAmend.Visible = IIf(SearchIndex = 1 And cmbOrderText.SelectedIndex <> 0, True, False)
        txtName.Visible = IIf(SearchIndex = 2 Or SearchIndex = 5, True, False)
        cmbPriority.Visible = IIf(SearchIndex = 3, True, False)
        cmbOrderTypeList.Visible = IIf(SearchIndex = 4, True, False)
        btnPrintTop.Enabled = mOrderItemListForFollowUp.Count > 0  'Added By Prashant 17-Jul-2013 ALL16072013
        btnPrintBottom.Enabled = mOrderItemListForFollowUp.Count > 0 'Added By Prashant 17-Jul-2013 ALL16072013
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ClearControls()
        cmbPriority.SelectedIndex = 0
        cmbOrderTypeList.SelectedIndex = 0
        txtNo.Text = ""
        txtAmend.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub SetGridColumnColor()
        Dim mLabel As Label  'Label
        Dim mRemainingDays As Integer  'Label
        Dim OrderItemFollowCount As Integer 'Added By Prashant 17-Jul-2013 ALL16072013
        For j As Integer = 0 To dgOrderList.Rows.Count - 1
            'mRemainingDays = Me.dgOrderList.Rows.Item(j).Cells(14).Text
            mRemainingDays = dgOrderList.Rows(j).Cells(14).Text
            'OrderItemFollowCount = CType(Me.dgOrderList.Rows.Item(j).Cells(24).Text, Integer)
            If dgOrderList.Rows(j).Cells(24).Text.ToString = "" Then

            Else
                OrderItemFollowCount = CType(dgOrderList.Rows(j).Cells(24).Text, Integer)
            End If


            If mRemainingDays < 0 Then
                'mLabel = CType(dgOrderList.Rows.Item(j).Cells(4).FindControl("lblColor"), Label)
                mLabel = CType(dgOrderList.Rows(j).Cells(4).FindControl("lblColor"), Label)
                'mLabel = CType(dgOrderList.Rows(j).FindControl("lblColor"), Label)

                mLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff0000")
            ElseIf mRemainingDays <= 15 Then
                'mLabel = CType(dgOrderList.Rows.Item(j).Cells(4).FindControl("lblColor"), Label)
                mLabel = CType(dgOrderList.Rows(j).Cells(4).FindControl("lblColor"), Label)



                mLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffff00")
            ElseIf mRemainingDays > 15 Then
                'mLabel = CType(dgOrderList.Rows.Item(j).Cells(4).FindControl("lblColor"), Label)
                mLabel = CType(dgOrderList.Rows(j).Cells(4).FindControl("lblColor"), Label)
                mLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#008000")
            End If
            'dgOrderList.Rows.Item(j).Cells(23).Enabled = IIf(OrderItemFollowCount > 0, False, True)
            dgOrderList.Rows(j).Cells(23).Enabled = IIf(OrderItemFollowCount > 0, False, True)
        Next
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        SearchIndex = IIf(IsNothing(SearchIndex), 0, SearchIndex)

        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder

        txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat").ToString)
        AsOnDate = Now.Date.ToString(AppSettings("DateFormat").ToString)

        mOrderItemListForFollowUp = OrderItemListForFollowUp.GetOrderItemListForFollowUp(AsOnDate)
        dgOrderList.DataSource = mOrderItemListForFollowUp

        OrderType = IIf(IsNothing(OrderType), 0, OrderType)

        Session("SearchIndex") = SearchIndex
        Session("mOrderItemListForFollowUp") = mOrderItemListForFollowUp
        Session("mDistinctTextListForOrder") = mDistinctTextListForOrder

        DataBind()
        lblResult.Text = "List of Order Item(s) as per Criteria :" & mOrderItemListForFollowUp.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            RemoveSession()
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            DataFieldBind()
            SetControl()
            Session("MiddleFrame") = "wfOrderItemListForFollowUp.aspx?"
        End If
        ControlVisibility(SearchIndex)
        SetGridColumnColor()
    End Sub
    Private Sub dgOrderList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgOrderList.PageIndexChanged
        dgOrderList.PageIndex = e.NewPageIndex
        dgOrderList.DataSource = mOrderItemListForFollowUp
        Session("mOrderItemListForFollowUp") = mOrderItemListForFollowUp
        dgOrderList.DataBind()
        SetGridColumnColor()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbOrderText.SelectedIndex = 0
        ClearControls()
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        SearchIndex = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        Priority = IIf(cmbPriority.SelectedIndex <= 0, 0, cmbPriority.SelectedValue)
        OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedItem.Text)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        Amend = txtAmend.Text.Trim

        Session("SearchIndex") = SearchIndex
        Session("Priority") = Priority

        Session("OrderText") = OrderText
        Session("Name") = Name
        Session("No") = No
        Session("Amend") = Amend
        'If Not (txtAsOnDate.IsDateValue) Then
        '    AsOnDate = ""
        'Else
        '    AsOnDate = txtAsOnDate.Text.ToString
        'End If

        If txtAsOnDate.Text = "" Then
            AsOnDate = ""
        Else
            AsOnDate = txtAsOnDate.Text.ToString
        End If

        CallFindNow(SearchIndex)
        ControlVisibility(SearchIndex)
        dgOrderList.DataBind()
        lblResult.Text = "List of Order Item(s) as per Criteria :" & mOrderItemListForFollowUp.Count & " Record(s) found."
        SetGridColumnColor()
    End Sub
    Private Sub cmbOrderText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrderText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbOrderText.Enabled = True Then
            setFocus(cmbOrderText)
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnCloseBottom.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub dgOrderList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgOrderList.SortCommand
    '    mOrderItemListForFollowUp.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    '    Session("mOrderItemListForFollowUp") = mOrderItemListForFollowUp
    '    dgOrderList.DataSource = mOrderItemListForFollowUp
    '    dgOrderList.DataBind()
    '    SetGridColumnColor()
    'End Sub
    'Added By Prashant 17-Jul-2013 ALL16072013
    Public Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim Rpt As New crOrderItemListForFollowUp
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsOrderItemListForFollowUp
        Dim mCompanyDetail As New CompanyDetail

        If cmbSearch.SelectedIndex = 0 Then
            'All
            SearchStr1 = "The report shows all records till date. " + New SmartDate(txtAsOnDate.Text).FormattedText
            SearchStr2 = ""
        ElseIf cmbSearch.SelectedIndex = 1 Then
            'Order
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbOrderText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text + " " + "_" + txtAmend.Text
        ElseIf cmbSearch.SelectedIndex = 2 Then
            'Part Number
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 3 Then
            'Vendor
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 4 Then
            'Order Type
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbOrderTypeList.SelectedItem.Text
        ElseIf cmbSearch.SelectedIndex = 5 Then
            'Vendor
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        End If

        If chkReceivedorderitemfollowup.Checked = True Then
            SearchStr3 = "received order items follow up records"
        Else
            SearchStr3 = ""
        End If

        mOrderItemListForFollowUp = Session("mOrderItemListForFollowUp")

        Dim mTempOrderItemFollowUps As OrderItemFollowUps
        mTempOrderItemFollowUps = OrderItemFollowUps.GetOrderItemFollowUps()

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Order Item List For Follow Up Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mOrderItemListForFollowUp.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfOrderItemListForFollowUp.aspx?BackPage=" & Request.QueryString("BackPage") & "&OrderType=" & OrderType
            msg1.Show()
            Exit Sub
        End If
        If IsExcel = False Then  'PDF format
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mOrderItemListForFollowUp)
            da.Fill(ds, mTempOrderItemFollowUps)
            da.Fill(ds, Report)
            Rpt.SetDataSource(ds)
            Session("CrystalReport") = Rpt
            Dim Str1 As String
            Str1 = "<script language=Javascript>openTranDetail();</script>"
            ClientScript.RegisterStartupScript(Me.GetType, "openTranDetail", Str1)
        Else  'Excel format
            mOrderItemListForFollowUpExportToexcel = Session("mOrderItemListForFollowUpExportToexcel")
            da.Fill(ds, mOrderItemListForFollowUpExportToexcel)
            da.Fill(ds, Report)

            Dim columnToRemove2 As String() = {"ID", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "CompanyName", "Address", "Tel1", "Tel2", _
                                               "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr8", _
                                               "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", _
                                               "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", _
                                               "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"CurrencySymbol", "OrderFollowUpSrNo", "OrderID", "OrderText", "OrderNo", "Amend", "OrderItemID", "ItemID", _
                                              "TransTypeID", "TransTypeName", "IsOverhaul", "SrNo", "OrderFollowUpID", "FollowUpText", "FollowUpNo"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("OrderItemListForFollowUpExportToexcel").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("OrderItemListForFollowUpExportToexcel").Columns.Remove(columnToRemove(i))
                End If
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()
            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "Search"
            dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "By"
            dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = " "

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Merge(ds.Tables("OrderItemListForFollowUpExportToexcel"))

            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("OrderTextNo").ColumnName = "Order No."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("OrderDate").ColumnName = "Order Date"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("IntOrderNo").ColumnName = "Int. Order No."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("OrderType").ColumnName = "Order Type"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("SupplierName").ColumnName = "Supplier"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("PartName").ColumnName = "Part No."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("PartDescription").ColumnName = "Description"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("SerialNo").ColumnName = "Serial No."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("DeliveryInDays").ColumnName = "Deliv. In Days"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("PriorityName").ColumnName = "Priority"

            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("RemainingDays").ColumnName = "Remaining Days"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("OrdQty").ColumnName = "Ord. Qty."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("RecQty").ColumnName = "Rec. Qty."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("BalQty").ColumnName = "Bal. Qty."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("CAmount").ColumnName = "Bal. Amount"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("CurrencyName").ColumnName = "Currency"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Amount").ColumnName = "Bal. Amount In Base Curr."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("FollowUpTextNo").ColumnName = "Follow Up No."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("FollowUpDate").ColumnName = "Follow Up Date"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("AWBNo").ColumnName = "AWB No."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("ProformaNo").ColumnName = "Proforma No."
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("ReturnInDays").ColumnName = "Return In Days"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("ShipmentStatus").ColumnName = "Shipment Status"
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("FollowUpRemarks").ColumnName = "Remark"


            'set Column Sequence
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Order No.").SetOrdinal(0)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Order Date").SetOrdinal(1)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Int. Order No.").SetOrdinal(2)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Order Type").SetOrdinal(3)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Supplier").SetOrdinal(4)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Part No.").SetOrdinal(5)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Description").SetOrdinal(6)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Serial No.").SetOrdinal(7)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Deliv. In Days").SetOrdinal(8)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Priority").SetOrdinal(9)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Remaining Days").SetOrdinal(10)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Ord. Qty.").SetOrdinal(11)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Rec. Qty.").SetOrdinal(12)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Bal. Qty.").SetOrdinal(13)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Bal. Amount").SetOrdinal(14)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Currency").SetOrdinal(15)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Bal. Amount In Base Curr.").SetOrdinal(16)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Follow Up No.").SetOrdinal(17)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Follow Up Date").SetOrdinal(18)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("AWB No.").SetOrdinal(19)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Proforma No.").SetOrdinal(20)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Return In Days").SetOrdinal(21)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("TD").SetOrdinal(22)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Shipment Status").SetOrdinal(23)
            dsNew.Tables("OrderItemListForFollowUpExportToexcel").Columns("Remark").SetOrdinal(24)

            dsNew.Tables("OrderItemListForFollowUpExportToexcel").TableName = "Order Item List For Follow Up"
			Session("ExcelFileName") = "Order Item List For Follow Up"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "OrderFollowUp", "Export To excel", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021

        End If

    End Sub
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrintBottom.Click
        SetReport(False)
    End Sub

    'Private Sub dgOrderList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgOrderList.ItemCommand
    '    Dim indx As Int32 = e.Item.ItemIndex + dgOrderList.CurrentPageIndex * dgOrderList.PageSize
    '    Select Case e.CommandName
    '        Case "FollowUp"
    '            Dim mOrderItemFollowUps As OrderItemFollowUps
    '            Dim OrderItemID As Guid = New Guid(e.Item.Cells(2).Text)
    '            mOrderItemFollowUps = OrderItemFollowUps.GetOrderItemFollowUps(OrderItemID.ToString)
    '            Session("OrderItemID") = OrderItemID
    '            Session("mOrderItemFollowUps") = mOrderItemFollowUps
    '            'MarkLog(Util.Action.Edit, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
    '            'End
    '            Session("OrderDate") = e.Item.Cells(4).Text
    '            Session("OrderTextNo") = e.Item.Cells(5).Text
    '            Session("SupplierName") = e.Item.Cells(8).Text
    '            Session("PartNo") = e.Item.Cells(9).Text
    '            Session("SrNo") = mOrderItemListForFollowUp(indx).SrNo
    '            Session("OrderID") = mOrderItemListForFollowUp(indx).OrderID
    '            Dim str As String
    '            str = "<script language='javascript'> openledgersame('wfOrderItemForFollowUp.aspx?BackPage=wfOrderItemListForFollowUp.aspx'); </script>"
    '            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
    '    End Select
    'End Sub

    'Private Sub dgOrderList_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOrderList.ItemDataBound
    '    If e.Item.ItemType <> ListItemType.Header Then
    '        Dim OrderItemID As Guid = (DataBinder.Eval(e.Item.DataItem, "OrderItemID"))
    '        mOrderItemFollowUps = OrderItemFollowUps.GetOrderItemFollowUps(OrderItemID.ToString)
    '        'If mOrderItemFollowUps.Count > 0 Then
    '        '    e.Item.Cells(0).BackColor = Color.Green
    '        'Else
    '        '    e.Item.Cells(0).BackColor = Color.Maroon
    '        '    e.Item.Cells(0).ToolTip = "No record found.."
    '        'End If

    '        If mOrderItemFollowUps.Count > 0 Then
    '            e.Item.Cells(0).BackColor = Color.Yellow
    '        End If

    '    End If
    'End Sub

    Private Sub btnExportTop_Click(sender As Object, e As System.EventArgs) Handles btnExportTop.Click, btnExportBottom.Click
        SetReport(True)
    End Sub
#End Region

#Region "Service Methods"
    <WebMethod()>
    Public Shared Function GetOrderItemForFO(ByVal OrderItemID As String) As String
        'Dim tempID As Guid = New Guid(OrderItemID)
        Dim mOrderItemFollowUps As OrderItemFollowUps
        mOrderItemFollowUps = OrderItemFollowUps.GetOrderItemFollowUps(OrderItemID)
        Dim Table As String = ""
        Table = Table & "<table width=""100%"" Class=""clsGridNewStyle"" style=""border-collapse:collapse;""  cellSpacing=""5"" rules=""all"">"
        Table = Table & "<tr class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=True horizontalAlign=""left"">"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=True horizontalAlign=""left"" align=center>"
        Table = Table & "Sr No.</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"">Date</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"">No.</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"">AWB No.</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"">Proforma No.</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"" align=right>Return In Days</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"">TD</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"">Shipment Status</td>"
        Table = Table & "<td class=""clsdgHeader"" backColor=""white"" foreColor=""black"" font-Bold=true horizontalAlign=""left"">Remark</td>"
        Table = Table & "</tr>"

        For Each item As OrderItemFollowUp In mOrderItemFollowUps
            Table = Table & "<tr Class=clsdgItem>"
            Table = Table & "<td align=center>" & item.SrNo & "</td>"
            Table = Table & "<td>" & item.FollowUpDateFormatted & "</td>"
            Table = Table & "<td>" & item.FollowUpTextNo & "</td>"
            Table = Table & "<td>" & item.AWBNo & "</td>"
            Table = Table & "<td>" & item.ProformaNo & "</td>"
            Table = Table & "<td align=right>" & item.ReturnInDays & "</td>"
            Table = Table & "<td>" & item.TD & "</td>"
            Table = Table & "<td>" & item.ShipmentStatus & "</td>"
            Table = Table & "<td>" & item.FollowUpRemarks & "</td>"
            Table = Table & "</tr>"
        Next

        Table = Table & "</table>"

        If mOrderItemFollowUps.Count > 0 Then
            Return Table
        Else
            Return "No record found.."
        End If

    End Function

    Private Sub dgOrderList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgOrderList.RowDataBound
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim mOrderItemFollowUps As OrderItemFollowUps
            Dim OrderItemID As Guid = (DataBinder.Eval(e.Row.DataItem, "OrderItemID"))
            mOrderItemFollowUps = OrderItemFollowUps.GetOrderItemFollowUps(OrderItemID.ToString)

            If mOrderItemFollowUps.Count > 0 Then
                e.Row.Cells(0).BackColor = Color.Yellow
            End If

            Dim dgOrderListchild As GridView = DirectCast(e.Row.FindControl("dgOrderListchild"), GridView)
            dgOrderListchild.DataSource = mOrderItemFollowUps
            dgOrderListchild.DataBind()


        End If
    End Sub

    Private Sub dgOrderList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgOrderList.RowCommand
        'Dim indx As Int32 = e.Ro.ItemIndex + dgOrderList.CurrentPageIndex * dgOrderList.PageSize

        Dim indx As Int32 = e.CommandArgument.ToString + dgOrderList.PageIndex * dgOrderList.PageSize

        Select Case e.CommandName
            Case "FollowUp"
                Dim mOrderItemFollowUps As OrderItemFollowUps
                Dim OrderItemID As Guid = mOrderItemListForFollowUp(indx).OrderItemID 'New Guid(e.Item.Cells(2).Text)
                mOrderItemFollowUps = OrderItemFollowUps.GetOrderItemFollowUps(OrderItemID.ToString)
                Session("OrderItemID") = OrderItemID
                Session("mOrderItemFollowUps") = mOrderItemFollowUps
                'MarkLog(Util.Action.Edit, mModuleName, mRCIDetail, Util.ErrorType.NoError, mReceiptCumInvoice.ID, EventLogID)
                'End
                Session("OrderDate") = mOrderItemListForFollowUp(indx).OrderDate
                Session("OrderTextNo") = mOrderItemListForFollowUp(indx).OrderTextNo
                Session("SupplierName") = mOrderItemListForFollowUp(indx).SupplierName
                Session("PartNo") = mOrderItemListForFollowUp(indx).PartName
                Session("SrNo") = mOrderItemListForFollowUp(indx).SrNo
                Session("OrderID") = mOrderItemListForFollowUp(indx).OrderID
                Dim str As String
                'str = "<script language='javascript'> openledgersame('wfOrderItemForFollowUp.aspx?BackPage=wfOrderItemListForFollowUp.aspx'); </script>"
                str = "openledgersame('wfOrderItemForFollowUp.aspx?BackPage=wfOrderItemListForFollowUp.aspx');"
                'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScript", "OpenScript();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End Select
    End Sub

    Private Sub dgOrderList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgOrderList.Sorting
        mOrderItemListForFollowUp.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mOrderItemListForFollowUp") = mOrderItemListForFollowUp
        dgOrderList.DataSource = mOrderItemListForFollowUp
        dgOrderList.DataBind()
        SetGridColumnColor()
    End Sub

#End Region


End Class

