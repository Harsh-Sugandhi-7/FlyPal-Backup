'AJAX Conversion By Vikrant On 10-Nov-2014

Public Class wfPendingSalesOrderList_Ajax
    Inherits System.Web.UI.Page

#Region "Enumaration"
    Public Enum UserRightsFor
        urfNew = 1
        urfEdit = 2
        urfDelete = 3
        urfView = 4
        urfPrint = 5
        urfSave = 6
    End Enum
#End Region

#Region " Variables "

    Public mPendingSalesOrderItemList As PendingSalesOrderItemList
    Public mSalesOrder As SalesOrder
    Public mSalesOrderList As SalesOrderList
    Public mPendingToIssueItemList As PendingToIssueItemList
    Public mSalesOrderTextList As DistinctTextListForSalesOrder
    Public mQuotationTextList As DistinctTextListForQuotation
    Public mTransTypeID As Trans
    Dim mPartName As String
    Public mItemName As String
    Dim mStoreID As Guid
    Dim mSalesOrderItemID As Guid = Guid.Empty
    Public mIssue As Issue

    Dim mSearchIndex, mDateIndex, mFromDate, mToDate, mText, ItemName, mNo, mQuotationText, mQuotationNo As String
#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mIssue = Session("mIssue")
        mTransTypeID = Session("mTransTypeID")
        mPendingSalesOrderItemList = Session("mPendingSalesOrderItemList")
        mSalesOrder = Session("mSalesOrder")
        mSalesOrderList = Session("mSalesOrderList")
        mSalesOrderTextList = Session("mSalesOrderTextList")
        mQuotationTextList = Session("mQuotationTextList")
        mSearchIndex = Session("mSearchIndex")
        mFromDate = Session("mFromDate")
        mToDate = Session("mToDate")
        mNo = IIf(IsNothing(Session("mNo")), 0, Session("mNo"))
        mDateIndex = Session("mDateIndex")
        mText = Session("mText")
        mQuotationText = Session("mQuotationText")
        mQuotationNo = Session("mQuotationNo")
        ItemName = Session("ItemName")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSearchIndex")
        Session.Remove("mFromDate")
        Session.Remove("mToDate")
        Session.Remove("mNo")
        Session.Remove("mDateIndex")
        Session.Remove("mText")
        Session.Remove("mQuotationText")
        Session.Remove("mQuotationNo")
        Session.Remove("ItemName")
        Session.Remove("mPendingSalesOrderItemList")
    End Sub
    Private Sub SetSession()
        Session("mIssue") = mIssue
        Session("mPendingSalesOrderItemList") = mPendingSalesOrderItemList
        Session("mSalesOrder") = mSalesOrder
        Session("mSalesOrderList") = mSalesOrderList
        Session("mSalesOrderTextList") = mSalesOrderTextList
        Session("mQuotationTextList") = mQuotationTextList
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2050", Optional ByVal StatusID As Integer = 0, Optional ByVal VendorName As String = "", Optional ByVal QuotationText As String = "", Optional ByVal QuotationNo As Integer = 0)
        mSalesOrderList = SalesOrderList.GetSalesOrderList(ItemName, Text, No, FromDate, ToDate, StatusID, VendorName, QuotationText, QuotationNo, mIssue.VendorID.ToString)
        dgSalesOrderList.DataSource = mSalesOrderList
        DataBind()
        Session("mSalesOrderList") = mSalesOrderList
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtItemName.Text = ""
        txtQuotationNo.Text = ""
    End Sub
    Private Sub SetControl()
        setPeroid(mDateIndex)
        CallFindNow(mSearchIndex)
        cmbSearch.SelectedIndex = mSearchIndex
        cmbDate.SelectedIndex = mDateIndex
        ' cmbText.SelectedValue = IIf(Text = "", "(All)", Text)
        'cmbQuotationText.SelectedValue = IIf(QuotationText = "", "(All)", QuotationText)
        txtNo.Text = mNo
        ControlVisibility(mSearchIndex, mDateIndex)
        lblResult.Text = "List of Sales Order as per criteria : " & mSalesOrderList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility(ByVal mSearchIndex As Int32, Optional ByVal mDateIndex As Int32 = 0)
        ' SalesOrder date
        cmbDate.Visible = IIf(mSearchIndex = 1, True, False)
        'lblFromDate.Visible = IIf(mSearchIndex = 1 And mDateIndex <> 0, True, False)
        'lblToDate.Visible = IIf(mSearchIndex = 1 And mDateIndex <> 0, True, False)
        'txtFromDate.Visible = IIf(mSearchIndex = 1 And mDateIndex <> 0, True, False)
        'txtToDate.Visible = IIf(mSearchIndex = 1 And mDateIndex <> 0, True, False)

        'SalesOrder Text and No and Amend
        cmbText.Visible = IIf(mSearchIndex = 2, True, False)
        txtNo.Visible = IIf(mSearchIndex = 2, True, False)
        lblNo.Visible = IIf(mSearchIndex = 2 Or mSearchIndex = 4, True, False)

        'Item Name
        txtItemName.Visible = IIf(mSearchIndex = 3, True, False)

        'Quotation
        cmbQuotationText.Visible = IIf(mSearchIndex = 4, True, False)
        txtQuotationNo.Visible = IIf(mSearchIndex = 4, True, False)


        If mSearchIndex = 1 And mDateIndex = 6 Then
            'txtFromDate.Visible = True
            'txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf mSearchIndex = 1 And (mDateIndex = 1 Or mDateIndex = 2 Or mDateIndex = 3 Or mDateIndex = 4 Or mDateIndex = 5) Then
            'txtFromDate.Visible = True
            'txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub setPeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = "1-Jan-1900"
                txtToDate.Text = "1-Jan-2200"
            Case 1 'Last 1 Week
                txtFromDate.Text = Today.AddDays(-6).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = Today.AddDays(1).AddMonths(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
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
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End If
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                'FromDate = IIf(FromDate = "01/01/1900" Or Not IsDate(FromDate), Today.Date, FromDate)
                'ToDate = IIf(ToDate = "01/01/2050" Or Not IsDate(ToDate), Today.Date, ToDate)
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        Select Case Index
            Case -1
                Call FindNow(, , , , , 2)   'for all records
            Case 0  'all
                Call FindNow(, , , , , 2)   'for all records
            Case 1 'SalesOrder date
                Call FindNow("", "", 0, txtFromDate.Text, txtToDate.Text, 2, "", "", 0)
            Case 2  'SalesOrder Text , No 
                Call FindNow("", IIf(cmbText.SelectedIndex <= 0, "", cmbText.SelectedValue), CInt(Val(txtNo.Text)), "1/1/1800", mToDate, 2, "", "", 0)
            Case 3  'ItemName
                Call FindNow(txtItemName.Text, "", 0, "1/1/1900", mToDate, 2, "", "", 0)
            Case 4 ' Quotation
                Call FindNow("", "", 0, "1/1/1900", mToDate, 2, "", IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedValue), CInt(Val(txtQuotationNo.Text)))
        End Select
    End Sub
    Private Sub setVariables()
        mSearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        mDateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        mFromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "01/01/1900")
        mToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "01/01/2050")
        mText = IIf(cmbText.SelectedIndex <= 0, "", cmbText.SelectedValue)
        mQuotationText = IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedValue)
        mQuotationNo = txtQuotationNo.Text.Trim
        ItemName = txtItemName.Text.Trim
        mNo = txtNo.Text.Trim
        Session("mFromDate") = mFromDate
        Session("mToDate") = mToDate
        Session("mSearchIndex") = mSearchIndex
        Session("mDateIndex") = mDateIndex
        Session("mText") = mText
        Session("mQuotationText") = mQuotationText
        Session("mQuotationNo") = mQuotationNo
        Session("ItemName") = ItemName
        Session("mNo") = mNo
    End Sub
    Private Sub setObject(ByVal Index As Int32)
        mIssue.IssueItems.CurrentItem.SalesOrderItemID = mPendingSalesOrderItemList(Index).SalesOrderItemID
        mItemName = mPendingSalesOrderItemList(Index).ItemName
        Session("mItemName") = mPendingSalesOrderItemList(Index).ItemName
        Session("SalesOrderItemID") = mPendingSalesOrderItemList(Index).SalesOrderItemID
        Session("mIssue") = mIssue
        Session("SalesOrderQty") = mPendingSalesOrderItemList(Index).IssueBalanceQty.ToString
    End Sub
#End Region

#Region "DataFieldBind "
    Private Sub DataFieldBind()
        GetSession()
        mFromDate = IIf(IsNothing(mFromDate), "01/01/1900", mFromDate)
        mToDate = IIf(IsNothing(mToDate), "01/01/2050", mToDate)
        mSearchIndex = IIf(IsNothing(mSearchIndex), 1, mSearchIndex)
        mDateIndex = IIf(IsNothing(mDateIndex), 2, mDateIndex)
        ' StatusId = Session("StatusId")
        mText = Session("mText")
        ItemName = Session("ItemName")
        mQuotationNo = Session("mQuotationNo")
        mNo = Session("mNo")

        mSalesOrderTextList = DistinctTextListForSalesOrder.GetDistinctTextList("9", , True, "(All)") '9 For SalesOrder
        cmbText.DataSource = mSalesOrderTextList
        mQuotationTextList = DistinctTextListForQuotation.GetDistinctTextList("8", , True, "(All)") '8 Quotation
        cmbQuotationText.DataSource = mQuotationTextList

        mSalesOrderList = Session("mSalesOrderList")
        mPendingSalesOrderItemList = Session("mPendingSalesOrderItemList")

        dgSalesOrderItems.PageIndex = 0
        dgSalesOrderList.DataSource = mSalesOrderList
        dgSalesOrderItems.DataSource = mPendingSalesOrderItemList

        DataBind()
        lblResult.Text = "List of Sales Orders as per criteria : " & mSalesOrderList.Count & " Record(s) found."

    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' ClearAll()
        GetSession()
        mPartName = Request.QueryString("Name")
        If Not IsPostBack And Session("Sender") = "" Then
            If mIssue.IsNew Then
                If cmbSearch.Enabled = True Then
                    cmbSearch.Focus()
                End If
                mSalesOrderList = SalesOrderList.GetSalesOrderList(, , , , , 2)
                Session("mSalesOrderList") = mSalesOrderList
                Session("mPendingSalesOrderItemList") = mPendingSalesOrderItemList
                DataFieldBind()
                SetControl()
            Else
                mSalesOrderList = Nothing
                dgSalesOrderList.DataSource = Nothing
                If mIssue.IssueItems.CurrentIndex > 0 Then     '(mIssue.IssueItems(mIssue.IssueItems.CurrentIndex - 1)).Equals(Nothing)
                    mSalesOrderList = SalesOrderList.GetSalesOrderBySalesOrderItem(mIssue.IssueItems(mIssue.IssueItems.CurrentIndex - 1).SalesOrderItemID) '(mIssue.IssueItems.CurrentItem.SalesOrderItemID)
                    'Set DataSource of the Grid
                    dgSalesOrderList.DataSource = mSalesOrderList
                    'Getting Sales Order Item :-----------------------------------------------------------------------
                    'Open the Selected Record in SalesOrderItem Details Form.
                    mPendingSalesOrderItemList = PendingSalesOrderItemList.GetPendingSalesOrderItemList(mSalesOrderList.Item(0).ID, mIssue.IDate.ToString)
                    Session("mSalesOrderList") = mSalesOrderList
                    Session("mPendingSalesOrderItemList") = mPendingSalesOrderItemList
                    DataFieldBind()
                    SetControl()
                    lblResult.Text = "List of Sales Orders as per criteria  : " & mSalesOrderList.Count & " Record(s) found."
                    lblResult1.Text = "List of Sales Orders as per criteria  : " & mPendingSalesOrderItemList.Count & " Record(s) found."
                Else
                    mSalesOrderList = SalesOrderList.GetSalesOrderList(, , , , , 2)
                    Session("mSalesOrderList") = mSalesOrderList
                    Session("mPendingSalesOrderItemList") = mPendingSalesOrderItemList
                    DataFieldBind()
                    SetControl()
                End If
            End If
        End If
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        ClearControls()
        'cmbDate.SelectedIndex = 0
        'cmbQuotationText.SelectedIndex = 0
        'cmbText.SelectedIndex = 0
        cmbDate.ClearSelection()
        cmbQuotationText.ClearSelection()
        cmbText.ClearSelection()
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeroid(DateIndex)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If
    End Sub
    Private Sub cmbQuotationText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQuotationText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeroid(DateIndex)
        If cmbQuotationText.Enabled = True Then
            cmbQuotationText.Focus()
        End If
    End Sub
    Private Sub cmbText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeroid(DateIndex)
        If cmbText.Enabled = True Then
            cmbText.Focus()
        End If
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeroid(DateIndex)
        If cmbDate.Enabled = True Then
            cmbDate.Focus()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(mSearchIndex)
        dgSalesOrderItems.DataBind()
        ''  SetControl()
        lblResult.Text = "List of Sales Orders as per criteria  : " & mSalesOrderList.Count & " Record(s) found."
        upnlSalesOrderListGrid.Update()
    End Sub
    Private Sub dgSalesOrderList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSalesOrderList.RowCommand
        Select Case e.CommandName
            Case "Select"
                dgSalesOrderList.DataSource = mSalesOrderList
                dgSalesOrderList.DataBind()
                Dim Index As Integer = CInt(e.CommandArgument) + dgSalesOrderList.PageIndex * dgSalesOrderList.PageSize
                mPendingSalesOrderItemList = PendingSalesOrderItemList.GetPendingSalesOrderItemList(mSalesOrderList.Item(Index).ID, mIssue.IDate.ToString)
                Session("mPendingSalesOrderItemList") = mPendingSalesOrderItemList
                dgSalesOrderItems.DataSource = mPendingSalesOrderItemList
                dgSalesOrderItems.DataBind()
                'DataFieldBind()
                'SetControl()
                lblResult1.Visible = True
                lblResult1.Text = "Sales Order Item List : " & mPendingSalesOrderItemList.Count & " Record(s) found"
                upnlSalesOrderItemListGrid.Update()
        End Select
    End Sub
    Private Sub dgSalesOrderItems_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSalesOrderItems.RowCommand
        Select Case e.CommandName
            Case "Select"
                dgSalesOrderItems.DataSource = mPendingSalesOrderItemList
                dgSalesOrderItems.DataBind()
                Dim Index As Integer = CInt(e.CommandArgument) + dgSalesOrderItems.PageIndex * dgSalesOrderItems.PageSize
                DataFieldBind()
                SetControl()
                setObject(Index)
                Session("CheckQty") = "True"
                Session("AddSalesOrderParts") = "True"
                'Session("mIssue") = mIssue
                ' Session("mTransTypeId") = mTransTypeID
                Session.Remove("mSalesOrderList")
                Session.Remove("mPendingSalesOrderItemList")
                RemoveSession()
                Session("FromSalesOrder") = "1"
                'Response.Redirect("wfPendingToIssueItemList.aspx?BackPage=wfIssue.aspx&ChildPage=wfIssueItem.aspx&PartName=" & mPartName)
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End Select
    End Sub
    Private Sub dgSalesOrderList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSalesOrderList.PageIndexChanging
        dgSalesOrderList.PageIndex = e.NewPageIndex
        dgSalesOrderList.DataSource = mSalesOrderList
        Session("mSalesOrderList") = mSalesOrderList
        dgSalesOrderList.DataBind()
    End Sub
    Private Sub dgSalesOrderItems_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSalesOrderItems.PageIndexChanging
        dgSalesOrderItems.PageIndex = e.NewPageIndex
        dgSalesOrderItems.DataSource = mPendingSalesOrderItemList
        Session("mPendingSalesOrderItemList") = mPendingSalesOrderItemList
        dgSalesOrderItems.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("mIssue") = mIssue
        RemoveSession()
        Response.Redirect("wfIssueItem_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
        'Response.Redirect("wfPendingToIssueItemList.aspx?BackPage=wfIssue.aspx&ChildPage=wfIssueItem.aspx")
    End Sub
    'Added By Prashant 18-June-2009 for sorting
    Private Sub dgSalesOrderList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSalesOrderList.Sorting
        mSalesOrderList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mSalesOrderList") = mSalesOrderList
        dgSalesOrderList.DataSource = mSalesOrderList
        dgSalesOrderList.DataBind()
    End Sub
    Private Sub dgSalesOrderItems_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSalesOrderItems.Sorting
        mPendingSalesOrderItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingSalesOrderItemList") = mPendingSalesOrderItemList
        dgSalesOrderItems.DataSource = mPendingSalesOrderItemList
        dgSalesOrderItems.DataBind()
    End Sub
    '-----------------------------------------
#End Region

End Class