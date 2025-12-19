Public Class wfrptChangeGRORate_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mInvoiceItemList As InvoiceItemList
    Public mGROCRate As Decimal
    Public mInvoiceID As Guid
    Public mInvoiceItemID As Guid
    Dim PartNo As String
    Dim Location As String
    Dim SearchIndex1 As String
    Dim EventLogID As Guid
    Public mIsReturnFromOHRepair As Boolean
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Public mDistinctTextListForInvoice As DistinctTextListForInvoice
    Dim ReceiptText, InvoiceText As String
    Dim RecNo, InvNo As Integer
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mInvoiceItemList = CType(Session("mInvoiceItemList"), InvoiceItemList)
        mGROCRate = CType(Session("mGROCRate"), Decimal)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        Location = IIf(IsNothing(Session("Location")), "", Session("Location"))
        SearchIndex1 = Session("SearchIndex1")
        mDistinctTextListForReceipt = Session("mDistinctTextListForReceipt")
        mDistinctTextListForInvoice = Session("mDistinctTextListForInvoice")

        RecNo = Session("RecNo")
        InvNo = Session("InvNo")
        ReceiptText = Session("ReceiptText")
        InvoiceText = Session("InvoiceText")
    End Sub
    Private Sub SetSession()
        Session("mInvoiceItemList") = mInvoiceItemList
        Session("PartNo") = PartNo
        Session("Location") = Location
        Session("SearchIndex1") = SearchIndex1
        Session("mDistinctTextListForReceipt") = mDistinctTextListForReceipt
        Session("mDistinctTextListForInvoice") = mDistinctTextListForInvoice
        Session("RecNo") = RecNo
        Session("InvNo") = InvNo
        Session("ReceiptText") = ReceiptText
        Session("InvoiceText") = InvoiceText
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mInvoiceItemList")
        Session.Remove("PartNo")
        Session.Remove("Location")
        Session.Remove("SearchIndex1")
        Session.Remove("mDistinctTextListForReceipt")
        Session.Remove("mDistinctTextListForInvoice")
        Session.Remove("RecNo")
        Session.Remove("InvNo")
        Session.Remove("ReceiptText")
        Session.Remove("InvoiceText")
    End Sub
    Private Sub ChangeRate(ByVal mGROCRate As Decimal, ByVal mInvoiceID As Guid, ByVal mInvoiceItemID As Guid, ByVal mIsReturnFromOHRepair As Boolean)
        Session("mInvoiceID") = mInvoiceID
        Session("mInvoiceItemID") = mInvoiceItemID
        Session("mGROCRate") = mGROCRate
        Session("mIsReturnFromOHRepair") = mIsReturnFromOHRepair
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        Location = ""
    End Sub
    Private Sub FindNow(ByVal LookinType As Integer, Optional ByVal ItemName As String = "", Optional ByVal Location As String = "", Optional ByVal ReceiptText As String = "", Optional ByVal InvoiceText As String = "", Optional ByVal RecNo As Integer = 0, Optional ByVal InvNo As Integer = 0)
        'This step is Imp when details form  is opened dirctly.
        If LookinType = -1 Then
            LookinType = 0
        End If

        dgPartSearch.DataSource = Nothing
        mInvoiceItemList = Nothing


        'Get List From the Database as per Criteria
        mInvoiceItemList = InvoiceItemList.GetInvoiceItemList(ItemName, Location, True, ReceiptText, InvoiceText, RecNo:=RecNo, InvNo:=InvNo)

        'Set DataSource of the Grid
        Session("mInvoiceItemList") = mInvoiceItemList
        BindGrid()
    End Sub
    Public Sub SetControl()
        FindNow(SearchIndex1, PartNo, Location, ReceiptText, InvoiceText, RecNo, InvNo)
        ControlVisibility()
    End Sub
    Private Sub ControlVisibility()
        txtSearchFor.Visible = IIf((cmbSearch.SelectedIndex = 1 Or cmbSearch.SelectedIndex = 2), True, False)
        cmbRecText.Visible = CBool(IIf(cmbSearch.SelectedIndex = 3, True, False))
        lblNo.Visible = IIf(((cmbSearch.SelectedIndex = 3 Or cmbSearch.SelectedIndex = 4) And (cmbRecText.SelectedIndex > 0 Or cmbIssueText.SelectedIndex > 0)), True, False)
        txtNo.Visible = IIf(((cmbSearch.SelectedIndex = 3 Or cmbSearch.SelectedIndex = 4) And (cmbRecText.SelectedIndex > 0 Or cmbIssueText.SelectedIndex > 0)), True, False)
        cmbIssueText.Visible = CBool(IIf(cmbSearch.SelectedIndex = 4, True, False))

        upnlSearchCriteria.Update()
    End Sub
    Private Sub BindGrid()
        dgPartSearch.DataSource = mInvoiceItemList
        dgPartSearch.DataBind()
        lblResult.Text = "List of Parts :" & mInvoiceItemList.Count & " Record(s) found. "
        upnlGrid.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mInvoiceItemList = InvoiceItemList.GetInvoiceItemList("", "", True)
        Session("mInvoiceItemList") = mInvoiceItemList
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("30", , True, "(All)")
        cmbRecText.DataSource = mDistinctTextListForReceipt
        cmbRecText.DataBind()
        Session("mDistinctTextListForReceipt") = mDistinctTextListForReceipt

        mDistinctTextListForInvoice = DistinctTextListForInvoice.GetDistinctTextListForInvoice("31", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForInvoice
        cmbIssueText.DataBind()
        Session("mDistinctTextListForInvoice") = mDistinctTextListForInvoice
        BindGrid()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptChangeGRORate_Ajax.aspx"
            If cmbSearch.Enabled = True Then
                SetFocus(cmbSearch)
            End If
            DataFieldBind()
            SetControl()
        End If
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub
    Private Sub dgPartSearch_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
        Select Case e.CommandName
            Case "ChangeGRORate"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
                If AppSettings("LockBackDatedTransaction") = "True" Then
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
                        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
                        If (CDate(mInvoiceItemList(index).DateFormatted.ToString) >= FirstDayofLastMonth) Then
                            If (CDate(mInvoiceItemList(index).DateFormatted.ToString) < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                                MSGBoxCtrl.Show("Alert!", "Previous Months transactions rate can only be change until " & DateSerial(Year(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), Month(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Else
                            MSGBoxCtrl.Show("Alert!", "Previous Months transactions rate can only be change until " & DateSerial(Year(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), Month(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If
                End If
                Dim mPart As String
                mGROCRate = mInvoiceItemList(index).GROCRate
                mPart = mInvoiceItemList(index).ItemName
                Dim mInvoiceID As Guid = mInvoiceItemList(index).InvoiceID
                Dim mInvoiceItemID As Guid = mInvoiceItemList(index).InvoiceItemID
                Dim mIsReturnFromOHRepair As Boolean = mInvoiceItemList(index).IsReturnFromOHRepair
                ChangeRate(mGROCRate, mInvoiceID, mInvoiceItemID, mIsReturnFromOHRepair)
                MarkLog(Util.Action.Edit, "ChangePartGRORate", "Part : " + mPart + " GRORate : " + mGROCRate.ToString, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                BindValueForChangeRate()
                pnlGRORate.Visible = True
                upnlChangeGRORate.Update()
                mdlPopUpChangeGRORate.Show()
                BindGrid()
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartSearch.PageIndex = 0

        SearchIndex1 = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex) 'Added by Prashant 12/11/07
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        Location = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")

        Session("PartNo") = PartNo
        Session("Location") = Location
        Session("SearchIndex1") = SearchIndex1
        'No = Val(txtNo.Text)
        If cmbSearch.SelectedIndex = 3 Then
            RecNo = Val(txtNo.Text)
        ElseIf cmbSearch.SelectedIndex = 4 Then
            InvNo = Val(txtNo.Text)
        End If
        Session("RecNo") = RecNo
        Session("InvNo") = InvNo

        FindNow(cmbSearch.SelectedIndex, PartNo, Location, ReceiptText, InvoiceText, RecNo, InvNo)
        BindGrid()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "ChangePartGRORate", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mInvoiceItemList = Nothing
        mDistinctTextListForReceipt = Nothing
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartSearch.Sorting
        mInvoiceItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInvoiceItemList") = mInvoiceItemList
        BindGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region "Rate"
#Region "Business Methods"
    Private Sub BindValueForChangeRate()
        txtCurrentGRORate.Text = mGROCRate.ToString
        If txtChangeGRORate.Enabled = True Then
            setFocus(txtChangeGRORate)
        End If
    End Sub
    Private Sub GetSessionForRate()
        mInvoiceID = CType(Session("mInvoiceID"), Guid)
        mInvoiceItemID = CType(Session("mInvoiceItemID"), Guid)
        mGROCRate = CType(Session("mGROCRate"), Decimal)
        mIsReturnFromOHRepair = CBool(Session("mIsReturnFromOHRepair"))
    End Sub
    Private Sub RemoveSessionForRate()
        Session.Remove("mInvoiceID")
        Session.Remove("mInvoiceItemID")
        Session.Remove("mGROCRate")
    End Sub
    Private Sub ClearControlsForRate()
        txtChangeGRORate.Text = ""
    End Sub
#End Region

#Region "Events"

    Private Sub btnGRORateOk_Click(sender As Object, e As System.EventArgs) Handles btnGRORateOk.Click
        If IsValid Then
            Try
                GetSessionForRate()
                Dim mInvoice As Invoice
                Dim mInvoiceItem As InvoiceItem
                mInvoice = Invoice.GetInvoice(mInvoiceID)
                mInvoiceItem = mInvoice.InvoiceItems.Item(mInvoiceItemID)
                mInvoice.InvoiceItems.UpdateItemsConversionFactore(mInvoice.ConversionFactor)
                mInvoiceItem.GROCRate = CDec(Val(txtChangeGRORate.Text.Trim))
                mInvoice.CalculateTotal()
                mInvoice.Save()
                MarkLog(Util.Action.Save, "ChangePartGRORate", "Old GRORate : " + mGROCRate.ToString + "New GRORate : " + txtChangeGRORate.Text.Trim, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                RemoveSessionForRate()
                ClearControlsForRate()
                mdlPopUpChangeGRORate.Hide()
                pnlGRORate.Visible = False
                upnlChangeGRORate.Update()
                SetControl()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub btnGRORateClose_Click(sender As Object, e As System.EventArgs) Handles btnGRORateClose.Click
        MarkLog(Util.Action.Close, "ChangePartGRORate", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionForRate()
        ClearControlsForRate()
        mdlPopUpChangeGRORate.Hide()
        pnlGRORate.Visible = False
        upnlChangeGRORate.Update()
    End Sub

    Private Sub cmbSearch_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        'If cmbSearch.SelectedIndex = 0 Then
        RecNo = 0
        InvNo = 0
        ReceiptText = String.Empty
        InvoiceText = String.Empty
        cmbRecText.SelectedIndex = 0
        cmbIssueText.SelectedIndex = 0
        Session("RecNo") = RecNo
        Session("InvNo") = InvNo
        Session("ReceiptText") = ReceiptText
        Session("InvoiceText") = InvoiceText
        'End If
        BindGrid()
        ControlVisibility()
    End Sub

    Private Sub cmbRecText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbRecText.SelectedIndexChanged

        InvNo = 0
        InvoiceText = String.Empty
        Session("InvNo") = InvNo

        Session("InvoiceText") = InvoiceText
        ReceiptText = IIf((cmbRecText.SelectedIndex = 0), "", cmbRecText.SelectedItem.ToString)
        Session("ReceiptText") = ReceiptText
        txtNo.Text = "0"
        ControlVisibility()

    End Sub

    Private Sub cmbIssueText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbIssueText.SelectedIndexChanged
        RecNo = 0
        ReceiptText = String.Empty
        Session("RecNo") = RecNo
        Session("ReceiptText") = ReceiptText
        InvoiceText = IIf((cmbIssueText.SelectedIndex = 0), "", cmbIssueText.SelectedItem.ToString)
        Session("InvoiceText") = InvoiceText
        txtNo.Text = "0"
        ControlVisibility()
    End Sub

#End Region
#End Region

End Class