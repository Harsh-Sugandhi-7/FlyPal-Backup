'Added By Vikrant On 30-Aug-2016 For ALL30082016
Imports System.Collections.Generic
Public Class wfReOrderLevelItemsForRequisition_Ajax
    Inherits System.Web.UI.Page

#Region "Variables and Declarations"
    Dim mRequisitionItemListNew As RequisitionItemListNew
    Public mRequisitionNew As RequisitionNew
    Public mCategoryLists As CategoryList
    Private checkedIds As New List(Of String)()
    Dim mCount As Integer = 0
    Dim EventLogID As Guid
    Dim ReOrderLevelItemsForRequisitionOpenFrom As Integer = 0 ''0 from Requstion Details, 1 From Re-Order Level items with Availability Nil Report
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mRequisitionItemListNew = Session("mRequisitionItemListNew")
        mRequisitionNew = Session("mRequisitionNew")
        mCount = CInt(Session("mCount"))
    End Sub
    Private Sub SetSession()
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub DataFieldBind()
        mCategoryLists = CategoryList.GetCategoryList("ALL")
        cmbCategory.DataSource = mCategoryLists
        If ReOrderLevelItemsForRequisitionOpenFrom = 0 Then ''0 from Requstion Details
            cmbCategory.SelectedIndex = 1
        End If
        cmbCategory.DataBind()

        ''Çommented by Ajay on 13-Jan-2023
        'mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList(txtPartNo.Text.Trim, 1, IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", True, False), _
        '                                                                        cmbCategory.SelectedValue.ToString, txtDescriptionSearch.Text, chkConsiderAlternatePart.Checked)

        '''Added by Ajay on 13-Jan-2023
        mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList("", 1, IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", True, False), _
                                                                                 cmbCategory.SelectedValue.ToString, "", chkConsiderAlternatePart.Checked, txtSearchBox.Text.Trim)


        If ReOrderLevelItemsForRequisitionOpenFrom = 0 Then ''0 from Requstion Details
            If Not mRequisitionItemListNew Is Nothing Then
                For Each Child As RequisitionItemNew In mRequisitionItemListNew
                    Child.IsSelect = mRequisitionNew.RequisitionItemsNew.Contains(Child.ItemID)
                    If mRequisitionNew.RequisitionItemsNew.Contains(Child.ItemID) Then
                        checkedIds.Add(Child.ID.ToString)
                    End If
                Next
            End If
        End If
        dgPartList.DataSource = mRequisitionItemListNew
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        If AppSettings("ClientCode") = "BA" Then
            dgPartList.Columns(10).HeaderText = "Available Qty.(Open Receipt)"
            dgPartList.Columns(12).HeaderText = "On Requisition Qty.(Open Req.)"
            dgPartList.Columns(13).HeaderText = "On Order Qty.(Open Order)"
        End If
        dgPartList.DataBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Then
            dgPartList.Columns(14).Visible = True
        Else
            dgPartList.Columns(14).Visible = False
        End If
        If mRequisitionItemListNew.Count = 100 Then
            lblResult.Text = "List of Parts Top : " & mRequisitionItemListNew.Count & " Record(s) found."
        Else
            lblResult.Text = "List of Parts : " & mRequisitionItemListNew.Count & " Record(s) found."
        End If
        lblCount.Text = "Total No Of Records : " & mRequisitionItemListNew.TotalRecords.ToString
        chkConsiderAlternatePart.DataBind()
        'Ajay 30-01-2023
        Label2.DataBind()
        Span1.DataBind()
        Label1.DataBind()
        Label3.DataBind()
        '---
    End Sub
    Private Sub FindNow()
        dgPartList.PageIndex = 0

        ''Commented by Ajay on 13-Jan-2023
        ''mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList(txtPartNo.Text.Trim, 1, IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", True, False), _
        ''                                                                        cmbCategory.SelectedValue.ToString, txtDescriptionSearch.Text.Trim, chkConsiderAlternatePart.Checked)

        ''Added by Ajay on 13-Jan-2023
        mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList("", 1, IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", True, False), _
                                                                               cmbCategory.SelectedValue.ToString, "", chkConsiderAlternatePart.Checked, txtSearchBox.Text.Trim)

        If ReOrderLevelItemsForRequisitionOpenFrom = 0 Then ''0 from Requstion Details
            '--
            If Not mRequisitionItemListNew Is Nothing Then
                For Each Child As RequisitionItemNew In mRequisitionItemListNew
                    Child.IsSelect = mRequisitionNew.RequisitionItemsNew.Contains(Child.ItemID)
                    If mRequisitionNew.RequisitionItemsNew.Contains(Child.ItemID) Then
                        checkedIds.Add(Child.ID.ToString)
                    End If
                Next
            End If
            '--
        End If

        dgPartList.DataSource = mRequisitionItemListNew
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        dgPartList.DataBind()

        '--
        ' --
        If mRequisitionItemListNew.Count = 100 Then
            lblResult.Text = "List of Parts Top : " & mRequisitionItemListNew.Count & " Record(s) found."
        Else
            lblResult.Text = "List of Parts : " & mRequisitionItemListNew.Count & " Record(s) found."
        End If
        lblCount.Text = "Total No Of Records : " & mRequisitionItemListNew.TotalRecords.ToString
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
        upnlFindNow.Update()
        upnlTitle.Update()
     End Sub
    'Private Sub SetObject()
    '    Dim checkString = Request.Form("chkSelect")
    '    ' Set Selectedvalue  
    '    If Not checkString Is Nothing Then
    '        Dim values = checkString.Split(","c)
    '        For Each value As String In values
    '            'If mFuelLogListPendingForInvoices.Contains(New Guid(value)) Then
    '            mRequisitionItemListNew(New Guid(value)).IsSelect = True
    '            'End If
    '        Next

    '        For i As Integer = 0 To mRequisitionItemListNew.Count - 1
    '            If mRequisitionItemListNew(i).IsSelect = True And Array.IndexOf(values, mRequisitionItemListNew(i).ID.ToString) = -1 Then
    '                mRequisitionItemListNew(i).IsSelect = False
    '            End If
    '        Next
    '    End If
    '    For i As Integer = 0 To mRequisitionItemListNew.Count - 1
    '        If mRequisitionItemListNew(i).IsSelect = False Then
    '            If mRequisitionNew.RequisitionItemsNew.Contains(mRequisitionItemListNew(i).ItemID) Then
    '                mRequisitionNew.RequisitionItemsNew.Remove(mRequisitionItemListNew(i).ItemID, "")
    '            End If
    '        End If
    '    Next
    '    Session("mRequisitionNew") = mRequisitionNew
    'End Sub
    Private Sub SetObject()
        Dim chkBox As CheckBox
        For i As Integer = 0 To dgPartList.Rows.Count - 1
            chkBox = CType(dgPartList.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
            Dim mID As New Guid(dgPartList.DataKeys(i).Values(0).ToString)
            mRequisitionItemListNew(mID).IsSelect = chkBox.Checked
        Next

        'For i As Integer = 0 To mRequisitionItemListNew.Count - 1
        '    If mRequisitionItemListNew(i).IsSelect = True And Array.IndexOf(values, mRequisitionItemListNew(i).ID.ToString) = -1 Then
        '        mRequisitionItemListNew(i).IsSelect = False
        '    End If
        'Next
        'End If
        For i As Integer = 0 To mRequisitionItemListNew.Count - 1
            If mRequisitionItemListNew(i).IsSelect = False Then
                If mRequisitionNew.RequisitionItemsNew.Contains(mRequisitionItemListNew(i).ItemID) Then
                    mRequisitionNew.RequisitionItemsNew.Remove(mRequisitionItemListNew(i).ItemID, "")
                End If
            End If
        Next
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub AddItemsToList()
        Dim chkBox As CheckBox
        For i As Integer = 0 To dgPartList.Rows.Count - 1
            chkBox = CType(dgPartList.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
            Dim mID As New Guid(dgPartList.DataKeys(i).Values(0).ToString)
            mRequisitionItemListNew(mID).IsSelect = chkBox.Checked
            If chkBox.Checked = True Then
                mCount = mCount + 1
                Session("mCount") = mCount
            End If
        Next
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
    End Sub
    'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    Private Sub ControlVisibility()
        If AppSettings("ShowFirstPriorityParts") = "True" Then
            dgPartList.Columns(5).Visible = True
        Else
            dgPartList.Columns(5).Visible = False
        End If
    End Sub
    'End
    Private Sub VisibilityOfControl(ByVal TempReOrderLevelItemsForRequisitionOpenFrom As Integer)
        If TempReOrderLevelItemsForRequisitionOpenFrom = 1 Then ''1 From Re-Order Level items with Availability Nil Report
            dgPartList.Columns(1).Visible = False
            lblCount.Visible = False
            lblTitle.InnerText = "Re-Order Level items with Availability Nil"
            btnOkTop.Visible = False
            ''btnOk.Visible = False
            btnCloseTop.Text = "Close"
            '' btnClose.Text = "Close"
        ElseIf TempReOrderLevelItemsForRequisitionOpenFrom = 0 Then ''0 from Requstion Details
            btnExportTop.Visible = False
            '' btnExport.Visible = False
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        ReOrderLevelItemsForRequisitionOpenFrom = Request.QueryString("ReOrderLevelItemsForRequisitionOpenFrom")
        If Not IsPostBack Then
            cmbShowEntries.SelectedIndex = 4 'Ajay 11-Jan-2023
            '' txtPartNo.Focus()
            txtSearchBox.Focus()
            DataFieldBind()

        End If
        If IsPostBack Then  'Added by Prashant 7-Mar-2019
            AddItemsToList()
        End If
        VisibilityOfControl(ReOrderLevelItemsForRequisitionOpenFrom)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click   '',btnClose.Click
        Session("AddReOrderParts") = "False"
        Session.Remove("mRequisitionItemListNew")
        If ReOrderLevelItemsForRequisitionOpenFrom = 0 Then ''0 from Requstion Details
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        ElseIf ReOrderLevelItemsForRequisitionOpenFrom = 1 Then ''1 From Re-Order Level items with Availability Nil Report
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    Private Sub dgPartList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartList.RowCommand
        Dim Index As Integer
        Select Case e.CommandName

            Case "ShowPartStatus"
                dgPartList.DataSource = mRequisitionItemListNew
                dgPartList.DataBind()
                ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
                Index = CInt(e.CommandArgument)
                Dim PartNoStatus As String = dgPartList.Rows(CInt(e.CommandArgument)).Cells(1).Text
                Dim DescriptionStatus As String = dgPartList.Rows(CInt(e.CommandArgument)).Cells(2).Text
                Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(PartNoStatus)
                Dim ItemIDStatus As Guid
                If mFetchItemByName.Count > 0 Then
                    ItemIDStatus = mFetchItemByName(0).ID
                Else
                    ItemIDStatus = Guid.Empty
                End If

                If Not ItemIDStatus.Equals(Guid.Empty) Then
                    Dim mItemStatus As Item = Item.GetItem(ItemIDStatus)
                    Dim LinkID As Guid = mItemStatus.LinkID
                    Dim Unit As String = mItemStatus.UnitName


                    Dim mStockPartStatus As rptStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
                    Dim mOnOrderPartStatus As rptOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
                    Dim mReturnablePartStatus As rptReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
                    Dim mTransitPartList As rptTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
                    Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode"))

                    Session("PartNoStatus") = PartNoStatus
                    Session("DescriptionStatus") = DescriptionStatus
                    Session("Unit") = Unit

                    Session("mStockPartStatus") = mStockPartStatus
                    Session("mOnOrderPartStatus") = mOnOrderPartStatus
                    Session("mReturnablePartStatus") = mReturnablePartStatus
                    Session("mTransitPartList") = mTransitPartList
                    Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
                    Session("LinkID") = LinkID
                    'Added By Vikrant On 30-Aug-2016 For ALL30082016
                    Dim URL As Stack = New Stack
                    URL.Push(Request.Url)
                    Session("URL") = URL
                    'End
                    Response.Redirect("wfrptShowPartNoStatus_Ajax.aspx?BackPage=wfReOrderLevelItemsForRequisition_Ajax.aspx")
                Else
                    'Alert Messege-Part Needs To Be Added In Part Master
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Part Needs To Be Added In Part Master.", False), True)
                End If
                'End
        End Select
    End Sub
    Private Sub dgPartList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
        dgPartList.PageIndex = e.NewPageIndex
        dgPartList.DataSource = mRequisitionItemListNew
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        dgPartList.DataBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    End Sub
    Private Sub dgPartList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartList.Sorting
        mRequisitionItemListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        dgPartList.DataSource = mRequisitionItemListNew
        dgPartList.DataBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    End Sub
    Private Sub txtPartNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkConsiderAlternatePart.CheckedChanged, cmbCategory.SelectedIndexChanged, txtSearchBox.TextChanged  ''txtPartNo.TextChanged,  txtDescriptionSearch.TextChanged
        FindNow()
    End Sub
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkTop.Click '',btnOk.Click
        If mCount = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one item", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        SetObject()
        'Dim checkString = Request.Form("chkSelect")
        'If checkString Is Nothing Then
        If mCount = 0 Then
            'Do nothing 
        Else
            Session("AddReOrderParts") = "True"
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    'Ajay 13-Jan-2023
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        dgPartList.PageSize = CInt(cmbShowEntries.SelectedItem.ToString)
        dgPartList.DataSource = mRequisitionItemListNew
        dgPartList.DataBind()
        ControlVisibility()
        upnlFindNow.Update()

    End Sub
    Private Sub dgPartList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgPartList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If (e.Row.Cells(15).Text <> "" And e.Row.Cells(15).Text <> "&nbsp;") Then 'Contracted With Supplier
                e.Row.Cells(15).Font.Bold = True 'Contracted With Supplier
                e.Row.Cells(15).BackColor = Color.Olive   'Contracted With Supplier
            End If
            'Added By Vikrant On 27-Sep-2018 For BA27092018

            If (CInt(Val(e.Row.Cells(22).Text)) > 0) Then               ''OnOrderQty
                e.Row.Cells(13).ToolTip = e.Row.Cells(17).Text
            End If
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Then
                'Stock + OnOrder + OnRequisitionQty > Min Stock Level
                If (CDbl(e.Row.Cells(20).Text) + CDbl(e.Row.Cells(22).Text) + CDbl(e.Row.Cells(21).Text)) > CDbl(e.Row.Cells(18).Text) Then ''+ CDbl(e.Row.Cells(21).Text) Added By Prashant on 14-Mar-2022 BA14032022
                    e.Row.Cells(13).Font.Bold = True
                    Dim mycolor As New Color
                    mycolor = Color.FromArgb(255, 128, 128)
                    e.Row.Cells(13).BackColor = mycolor
                ElseIf (CDbl(e.Row.Cells(20).Text) + CDbl(e.Row.Cells(22).Text) + CDbl(e.Row.Cells(21).Text)) <= CDbl(e.Row.Cells(18).Text) Then ''+ CDbl(e.Row.Cells(21).Text) Added By Prashant on 14-Mar-2022 BA14032022
                    e.Row.Cells(13).Font.Bold = True
                    e.Row.Cells(13).BackColor = Color.SkyBlue
                End If
            End If
            'End
            'Added by Vikrant On 11-Jul-2019 For ALL11072019	
            If AppSettings("ShowFirstPriorityParts") = "True" AndAlso (e.Row.Cells(4).Text <> "" And e.Row.Cells(4).Text <> "&nbsp;") And (e.Row.Cells(2).Text <> e.Row.Cells(5).Text) Then
                e.Row.Cells(5).Font.Bold = True
            End If
            'End
        End If
    End Sub
    Private Sub btnExportTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportTop.Click   '', btnExport.Click
        If mRequisitionItemListNew.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim ds As New dsRequisitionItemListNew
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail

        ''Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        ''        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        ''        mCompanyDetail.WebSite, "Re-Order Level items", SearchStr1:=txtPartNo.Text.Trim, _
        ''        SearchStr2:=txtDescriptionSearch.Text.Trim, SearchStr3:=cmbCategory.SelectedItem.ToString, _
        ''        SearchStr4:=chkConsiderAlternatePart.Checked, SearchStr5:="", ProductVersion:=("Product Version"), SINote:=AppSettings("SINote"), _
        ''        SearchStr6:="", SearchStr7:="", SearchStr8:="", _
        ''        SearchStr9:="", SearchStr10:=AppSettings("Logo"), _
        ''        SearchStr11:="", _
        ''        SearchStr12:="", _
        ''        SearchStr13:="", SearchStr14:="")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, "Re-Order Level items", SearchStr1:=txtSearchBox.Text.Trim, _
              SearchStr2:="", SearchStr3:=cmbCategory.SelectedItem.ToString, _
              SearchStr4:=chkConsiderAlternatePart.Checked, SearchStr5:="", ProductVersion:=("Product Version"), SINote:=AppSettings("SINote"), _
              SearchStr6:="", SearchStr7:="", SearchStr8:="", _
              SearchStr9:="", SearchStr10:=AppSettings("Logo"), _
              SearchStr11:="", _
              SearchStr12:="", _
              SearchStr13:="", SearchStr14:="")
        ds.Clear()
        da.Fill(ds, "ReportData", Report)
        da.Fill(ds, "RequisitionItemListNew", mRequisitionItemListNew)
        Dim columnToRemove As String() = {"ID", "SrNo", "ReqID", "WOID", "WONo", "NRCNo", "WONoNRCNo", "MachineID", "RegNo", "ReasonForRequest", "ItemID", "IPCReference", "RequestedQty", _
                                          "PriorityID", "PriorityName", "AvailableQty", "PurchaseQty", "ReasonForPurchase", "IssueBalQty", "PurchaseBalQty", "EnquiryBalQty", _
                                          "QuotationBalQty", "OrderBalQty", "Remark", "Note", "IsNewPart", "IsSelect", "RequisitionNo", "ReqDate", "ReqDateFormatted", "Days", "WorkShopID", _
                                          "WorkShopName", "Unit", "IsOneTimePurchase", "OneTimePurchase", "OrderID", "MinReOrderLevel", "ManualReference", "CustomerName", _
                                          "CustomerAddress", "AircraftType", "ModelID", "ModelName", "PendingCEQty", "IssuedQty", "AuthorizedBy", "LocationName", _
                                          "EmployeeName", "UnitID", "RemainingCEQty", "ContractedVendorID", "ContractedVendorName", "PONosYetToReceive", _
                                          "RequisitionItemTypeID", "RequisitionItemTypeName", "FirstPriorityPart", "DueDate", "IsExchangePurchase", _
                                          "IsItemIDEmpty", "orderItemReceiptBalanceQuantity", "ReOrderQty", "TSNValue", "TSNValueFormatted", "CSNValue", _
                                          "CSNValueFormatted", "AvailableQtyForItemGridOfOpenAuthorizedReceipt", "OnRequisitionQtyOfOpenAuthorizedRequisition", _
                                          "OnOrderQtyOfOpenAuthorizedOrder", "IsValid", "IsDirty", "IsDeleted", "IsNew"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("RequisitionItemListNew").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("RequisitionItemListNew").Columns.Remove(columnToRemove(i))
            End If
        Next
        Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", _
                                           "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", _
                                           "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", _
                                           "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ShortName", "SearchStr2"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
            End If
        Next
        If ds.Tables("RequisitionItemListNew").Columns.Contains("PartNo") Then
            ds.Tables("RequisitionItemListNew").Columns("PartNo").ColumnName = "Part No."
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("AlternatePart") Then
            ds.Tables("RequisitionItemListNew").Columns("AlternatePart").ColumnName = "Alternate Part"
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("MinStockLevel") Then
            ds.Tables("RequisitionItemListNew").Columns("MinStockLevel").ColumnName = "Min. Level"
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("MaxStockLevel") Then
            ds.Tables("RequisitionItemListNew").Columns("MaxStockLevel").ColumnName = "Max. Level"
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("AvailableQtyForItemGrid") Then
            ds.Tables("RequisitionItemListNew").Columns("AvailableQtyForItemGrid").ColumnName = "Available Qty."
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("AvailableQtyForItemGridOfOpenReceipt") Then
            ds.Tables("RequisitionItemListNew").Columns("AvailableQtyForItemGridOfOpenReceipt").ColumnName = "Available Qty.(Open Receipt)"
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("OnRequisitionQty") Then
            ds.Tables("RequisitionItemListNew").Columns("OnRequisitionQty").ColumnName = "On Requisition Qty."
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("OnRequisitionQtyOfOpenRequisition") Then
            ds.Tables("RequisitionItemListNew").Columns("OnRequisitionQtyOfOpenRequisition").ColumnName = "On Requisition Qty.(Open Requisition)"
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("OnOrderQty") Then
            ds.Tables("RequisitionItemListNew").Columns("OnOrderQty").ColumnName = "On Order Qty."
        End If
        If ds.Tables("RequisitionItemListNew").Columns.Contains("OnOrderQtyOfOpenOrder") Then
            ds.Tables("RequisitionItemListNew").Columns("OnOrderQtyOfOpenOrder").ColumnName = "On Order Qty.(Open Order)"
        End If
        '''If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
        '''    ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Part No"
        '''End If
        '''If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
        '''    ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Description"
        '''End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
            ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Search Text"
        End If

        If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
            ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Category"
        End If
        If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
            ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Consider Alternate Part(s) Stock also for Re-Order Qty. calculation"
        End If

        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Tables("ReportData").TableName = "Searching Criteria"

        dsNew.Merge(ds.Tables("RequisitionItemListNew"))
		dsNew.Tables("RequisitionItemListNew").TableName = "Re-Order Level items"
		Session("ExcelFileName") = "Re-Order Level items"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "Re-OrderLevelitemswithAvailabilityNil", "Export To Excel ", Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region "Checked Selection"
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
#End Region



End Class